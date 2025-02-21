// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.AvailabilityJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Net;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  public static class AvailabilityJobs
  {
    private struct FullNode : IEquatable<AvailabilityJobs.FullNode>
    {
      public NodeID m_NodeID;
      public float m_CurvePos;

      public FullNode(NodeID nodeID, float curvePos)
      {
        this.m_NodeID = nodeID;
        this.m_CurvePos = curvePos;
      }

      public bool Equals(AvailabilityJobs.FullNode other)
      {
        return this.m_NodeID.Equals(other.m_NodeID) && (double) this.m_CurvePos == (double) other.m_CurvePos;
      }

      public override int GetHashCode() => this.m_NodeID.m_Index >> 2 ^ math.asint(this.m_CurvePos);
    }

    private struct NodeData
    {
      public int m_Processed;
      public int m_AccessRequirement;
      public AvailabilityJobs.NodeAvailability m_Availability;
      public EdgeID m_EdgeID;
      public EdgeID m_NextID;
      public AvailabilityJobs.FullNode m_PathNode;

      public NodeData(
        int accessRequirement,
        AvailabilityJobs.NodeAvailability availability,
        EdgeID edgeID,
        EdgeID nextID,
        AvailabilityJobs.FullNode pathNode)
      {
        this.m_Processed = 0;
        this.m_AccessRequirement = accessRequirement;
        this.m_Availability = availability;
        this.m_EdgeID = edgeID;
        this.m_NextID = nextID;
        this.m_PathNode = pathNode;
      }
    }

    private struct HeapData : 
      ILessThan<AvailabilityJobs.HeapData>,
      IComparable<AvailabilityJobs.HeapData>
    {
      public float m_Availability;
      public int m_NodeIndex;

      public HeapData(float availability, int nodeIndex)
      {
        this.m_Availability = availability;
        this.m_NodeIndex = nodeIndex;
      }

      public bool LessThan(AvailabilityJobs.HeapData other)
      {
        return (double) this.m_Availability > (double) other.m_Availability;
      }

      public int CompareTo(AvailabilityJobs.HeapData other) => this.m_NodeIndex - other.m_NodeIndex;
    }

    private struct NodeAvailability
    {
      public float m_Availability;
      public int m_Provider;

      public NodeAvailability(float availability, int provider)
      {
        this.m_Availability = availability;
        this.m_Provider = provider;
      }
    }

    private struct ProviderItem
    {
      public float m_Capacity;
      public float m_Cost;
    }

    private struct AvailabilityExecutor
    {
      private UnsafePathfindData m_PathfindData;
      private Allocator m_Allocator;
      private AvailabilityParameters m_Parameters;
      private UnsafeParallelMultiHashMap<Entity, PathTarget> m_ProviderTargets;
      private UnsafeList<AvailabilityJobs.ProviderItem> m_Providers;
      private UnsafeList<int> m_ProviderIndex;
      private UnsafeHashMap<AvailabilityJobs.FullNode, int> m_NodeIndex;
      private UnsafeMinHeap<AvailabilityJobs.HeapData> m_Heap;
      private UnsafeList<AvailabilityJobs.NodeData> m_NodeData;

      public void Initialize(
        NativePathfindData pathfindData,
        Allocator allocator,
        AvailabilityParameters parameters)
      {
        this.m_PathfindData = pathfindData.GetReadOnlyData();
        this.m_Allocator = allocator;
        this.m_Parameters = parameters;
        this.m_NodeIndex = new UnsafeHashMap<AvailabilityJobs.FullNode, int>(10000, (AllocatorManager.AllocatorHandle) allocator);
        this.m_Heap = new UnsafeMinHeap<AvailabilityJobs.HeapData>(1000, allocator);
        this.m_NodeData = new UnsafeList<AvailabilityJobs.NodeData>(10000, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Release()
      {
        if (this.m_NodeData.IsCreated)
        {
          this.m_NodeIndex.Dispose();
          this.m_Heap.Dispose();
          this.m_NodeData.Dispose();
        }
        if (this.m_ProviderTargets.IsCreated)
          this.m_ProviderTargets.Dispose();
        if (!this.m_Providers.IsCreated)
          return;
        this.m_Providers.Dispose();
        this.m_ProviderIndex.Dispose();
      }

      public void AddSources(ref UnsafeQueue<PathTarget> pathTargets)
      {
        this.m_ProviderTargets = new UnsafeParallelMultiHashMap<Entity, PathTarget>(pathTargets.Count, (AllocatorManager.AllocatorHandle) this.m_Allocator);
        PathTarget pathTarget;
        while (pathTargets.TryDequeue(out pathTarget))
          this.m_ProviderTargets.Add(pathTarget.m_Target, pathTarget);
      }

      public void AddProviders(
        ref UnsafeQueue<AvailabilityProvider> availabilityProviders)
      {
        int count = availabilityProviders.Count;
        this.m_Providers = new UnsafeList<AvailabilityJobs.ProviderItem>(count, (AllocatorManager.AllocatorHandle) this.m_Allocator);
        this.m_ProviderIndex = new UnsafeList<int>(count, (AllocatorManager.AllocatorHandle) this.m_Allocator);
        for (int index = 0; index < count; ++index)
        {
          AvailabilityProvider availabilityProvider = availabilityProviders.Dequeue();
          AvailabilityJobs.ProviderItem providerItem = new AvailabilityJobs.ProviderItem();
          providerItem.m_Capacity = availabilityProvider.m_Capacity;
          providerItem.m_Cost = availabilityProvider.m_Cost * this.m_Parameters.m_CostFactor;
          this.m_Providers.Add(in providerItem);
          this.m_ProviderIndex.Add(in index);
          float num = 0.0f;
          PathTarget pathTarget;
          NativeParallelMultiHashMapIterator<Entity> it;
          if (this.m_ProviderTargets.TryGetFirstValue(availabilityProvider.m_Provider, out pathTarget, out it))
          {
            do
            {
              EdgeID edgeId;
              if (this.m_PathfindData.m_PathEdges.TryGetValue(pathTarget.m_Entity, out edgeId))
              {
                ref Edge local = ref this.m_PathfindData.GetEdge(edgeId);
                bool3 directions = new bool3((local.m_Specification.m_Flags & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary) || (double) pathTarget.m_Delta == 1.0, (local.m_Specification.m_Flags & EdgeFlags.AllowMiddle) != 0, (local.m_Specification.m_Flags & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary) || (double) pathTarget.m_Delta == 0.0);
                num += this.AddConnections(edgeId, in local, providerItem, index, pathTarget.m_Delta, directions);
              }
            }
            while (this.m_ProviderTargets.TryGetNextValue(out pathTarget, ref it));
          }
          AvailabilityJobs.ProviderItem provider = this.m_Providers[index];
          provider.m_Cost += num;
          this.m_Providers[index] = provider;
        }
      }

      private bool HeapExtract(out AvailabilityJobs.HeapData heapData)
      {
        if (this.m_Heap.Length != 0)
        {
          heapData = this.m_Heap.Extract();
          return true;
        }
        heapData = new AvailabilityJobs.HeapData();
        return false;
      }

      private void HeapInsert(AvailabilityJobs.HeapData heapData) => this.m_Heap.Insert(heapData);

      public bool FindAvailabilityNodes()
      {
        AvailabilityJobs.HeapData heapData;
        while (this.HeapExtract(out heapData))
        {
          ref AvailabilityJobs.NodeData local1 = ref this.m_NodeData.ElementAt(heapData.m_NodeIndex);
          if (local1.m_Processed == 0)
          {
            local1.m_Processed = 1;
            int providerIndex = this.GetProviderIndex(local1.m_Availability.m_Provider);
            AvailabilityJobs.ProviderItem provider = this.m_Providers[providerIndex];
            float num1 = 0.0f;
            if (local1.m_NextID.m_Index != -1)
            {
              ref Edge local2 = ref this.m_PathfindData.GetEdge(local1.m_NextID);
              num1 += this.CheckNextEdge(local1.m_NextID, local1.m_PathNode, in local2, provider, providerIndex);
            }
            else
            {
              int num2 = this.m_PathfindData.GetConnectionCount(local1.m_PathNode.m_NodeID);
              int2 int2 = new int2(-1, local1.m_AccessRequirement);
              AvailabilityJobs.FullNode pathNode = local1.m_PathNode;
              EdgeID edgeId1 = local1.m_EdgeID;
              for (int connectionIndex = 0; connectionIndex < num2; ++connectionIndex)
              {
                EdgeID edgeId2 = new EdgeID()
                {
                  m_Index = this.m_PathfindData.GetConnection(pathNode.m_NodeID, connectionIndex)
                };
                int num3 = this.m_PathfindData.GetAccessRequirement(pathNode.m_NodeID, connectionIndex);
                if (!edgeId1.Equals(edgeId2) && !math.all(int2 != num3))
                {
                  ref Edge local3 = ref this.m_PathfindData.GetEdge(edgeId2);
                  if (!this.DisallowConnection(local3.m_Specification))
                    num1 += this.CheckNextEdge(edgeId2, pathNode, in local3, provider, providerIndex);
                }
              }
            }
            this.m_Providers.ElementAt(providerIndex).m_Cost += num1;
          }
        }
        return this.m_NodeData.Length != 0;
      }

      private float CheckNextEdge(
        EdgeID nextID,
        AvailabilityJobs.FullNode pathNode,
        in Edge edge,
        AvailabilityJobs.ProviderItem providerItem,
        int providerIndex)
      {
        float startDelta;
        bool3 directions;
        if (pathNode.Equals(new AvailabilityJobs.FullNode(edge.m_StartID, edge.m_StartCurvePos)))
        {
          startDelta = 0.0f;
          directions = new bool3((edge.m_Specification.m_Flags & EdgeFlags.Forward) != 0, (edge.m_Specification.m_Flags & (EdgeFlags.Forward | EdgeFlags.AllowMiddle)) == (EdgeFlags.Forward | EdgeFlags.AllowMiddle), false);
        }
        else if (pathNode.Equals(new AvailabilityJobs.FullNode(edge.m_EndID, edge.m_EndCurvePos)))
        {
          startDelta = 1f;
          directions = new bool3(false, (edge.m_Specification.m_Flags & (EdgeFlags.Backward | EdgeFlags.AllowMiddle)) == (EdgeFlags.Backward | EdgeFlags.AllowMiddle), (edge.m_Specification.m_Flags & EdgeFlags.Backward) != 0);
        }
        else
        {
          if (!pathNode.m_NodeID.Equals(edge.m_MiddleID))
            return 0.0f;
          startDelta = pathNode.m_CurvePos;
          directions = new bool3((edge.m_Specification.m_Flags & EdgeFlags.Forward) != 0, (edge.m_Specification.m_Flags & EdgeFlags.AllowMiddle) != 0, (edge.m_Specification.m_Flags & EdgeFlags.Backward) != 0);
        }
        return this.AddConnections(nextID, in edge, providerItem, providerIndex, startDelta, directions);
      }

      private float AddConnections(
        EdgeID id,
        in Edge edge,
        AvailabilityJobs.ProviderItem providerItem,
        int providerIndex,
        float startDelta,
        bool3 directions)
      {
        float num1 = 0.0f;
        if (directions.x)
        {
          float cost = PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, new float2(startDelta, 1f));
          num1 += cost;
          this.AddHeapData(id, in edge, this.GetAvailability(providerItem, cost), providerIndex, new AvailabilityJobs.FullNode(edge.m_EndID, edge.m_EndCurvePos));
        }
        if (directions.y)
        {
          int num2 = this.m_PathfindData.GetConnectionCount(edge.m_MiddleID);
          if (num2 != 0)
          {
            int2 int2 = new int2(-1, edge.m_Specification.m_AccessRequirement);
            for (int connectionIndex = 0; connectionIndex < num2; ++connectionIndex)
            {
              EdgeID edgeId = new EdgeID()
              {
                m_Index = this.m_PathfindData.GetConnection(edge.m_MiddleID, connectionIndex)
              };
              int num3 = this.m_PathfindData.GetAccessRequirement(edge.m_MiddleID, connectionIndex);
              if (!id.Equals(edgeId) && !math.all(int2 != num3))
              {
                ref Edge local = ref this.m_PathfindData.GetEdge(edgeId);
                if (!this.DisallowConnection(local.m_Specification))
                {
                  if (edge.m_MiddleID.Equals(local.m_StartID) & (local.m_Specification.m_Flags & EdgeFlags.Forward) != 0)
                  {
                    float startCurvePos = local.m_StartCurvePos;
                    if (directions.x & (double) startCurvePos >= (double) startDelta | directions.z & (double) startCurvePos <= (double) startDelta)
                    {
                      float cost = PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, new float2(startDelta, startCurvePos));
                      num1 += cost;
                      this.AddHeapData(id, edgeId, in edge, this.GetAvailability(providerItem, cost), providerIndex, new AvailabilityJobs.FullNode(edge.m_StartID, startCurvePos));
                    }
                  }
                  if (edge.m_MiddleID.Equals(local.m_EndID) & (local.m_Specification.m_Flags & EdgeFlags.Backward) != 0)
                  {
                    float endCurvePos = local.m_EndCurvePos;
                    if (directions.x & (double) endCurvePos >= (double) startDelta | directions.z & (double) endCurvePos <= (double) startDelta)
                    {
                      float cost = PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, new float2(startDelta, endCurvePos));
                      num1 += cost;
                      this.AddHeapData(id, edgeId, in edge, this.GetAvailability(providerItem, cost), providerIndex, new AvailabilityJobs.FullNode(edge.m_EndID, endCurvePos));
                    }
                  }
                }
              }
            }
          }
        }
        if (directions.z)
        {
          float cost = PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, new float2(startDelta, 0.0f));
          num1 += cost;
          this.AddHeapData(id, in edge, this.GetAvailability(providerItem, cost), providerIndex, new AvailabilityJobs.FullNode(edge.m_StartID, edge.m_StartCurvePos));
        }
        return num1;
      }

      private void AddHeapData(
        EdgeID id,
        in Edge edge,
        float availability,
        int providerIndex,
        AvailabilityJobs.FullNode pathNode)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref AvailabilityJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          int providerIndex1 = this.GetProviderIndex(local.m_Availability.m_Provider);
          if (providerIndex != providerIndex1)
            this.MergeProviders(providerIndex, providerIndex1);
          if (local.m_Processed != 0 || (double) availability >= (double) local.m_Availability.m_Availability)
            return;
          local = new AvailabilityJobs.NodeData(edge.m_Specification.m_AccessRequirement, new AvailabilityJobs.NodeAvailability(availability, providerIndex), id, new EdgeID()
          {
            m_Index = -1
          }, pathNode);
          this.HeapInsert(new AvailabilityJobs.HeapData(availability, num));
        }
        else
        {
          int length = this.m_NodeData.Length;
          this.m_NodeData.Add(new AvailabilityJobs.NodeData(edge.m_Specification.m_AccessRequirement, new AvailabilityJobs.NodeAvailability(availability, providerIndex), id, new EdgeID()
          {
            m_Index = -1
          }, pathNode));
          this.HeapInsert(new AvailabilityJobs.HeapData(availability, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private void AddHeapData(
        EdgeID id,
        EdgeID id2,
        in Edge edge,
        float availability,
        int providerIndex,
        AvailabilityJobs.FullNode pathNode)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref AvailabilityJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          int providerIndex1 = this.GetProviderIndex(local.m_Availability.m_Provider);
          if (providerIndex != providerIndex1)
            this.MergeProviders(providerIndex, providerIndex1);
          if (local.m_Processed != 0)
            return;
          if ((double) availability < (double) local.m_Availability.m_Availability)
          {
            local = new AvailabilityJobs.NodeData(edge.m_Specification.m_AccessRequirement, new AvailabilityJobs.NodeAvailability(availability, providerIndex), id, id2, pathNode);
            this.HeapInsert(new AvailabilityJobs.HeapData(availability, num));
          }
          else
          {
            if (id2.Equals(local.m_NextID))
              return;
            local.m_NextID.m_Index = -1;
          }
        }
        else
        {
          int length = this.m_NodeData.Length;
          this.m_NodeData.Add(new AvailabilityJobs.NodeData(edge.m_Specification.m_AccessRequirement, new AvailabilityJobs.NodeAvailability(availability, providerIndex), id, id2, pathNode));
          this.HeapInsert(new AvailabilityJobs.HeapData(availability, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private bool DisallowConnection(PathSpecification newSpec)
      {
        return (newSpec.m_Methods & PathMethod.Road) == (PathMethod) 0;
      }

      private void MergeProviders(int providerIndex1, int providerIndex2)
      {
        AvailabilityJobs.ProviderItem provider1 = this.m_Providers[providerIndex1];
        AvailabilityJobs.ProviderItem provider2 = this.m_Providers[providerIndex2];
        provider1.m_Capacity += provider2.m_Capacity;
        provider1.m_Cost += provider2.m_Cost;
        provider1.m_Capacity *= (float) ((1.0 + (double) provider1.m_Cost) / (2.0 + (double) provider1.m_Cost));
        this.m_Providers[providerIndex1] = provider1;
        this.m_ProviderIndex[providerIndex2] = providerIndex1;
      }

      private float GetAvailability(AvailabilityJobs.ProviderItem providerItem, float cost)
      {
        return providerItem.m_Capacity / (1f + providerItem.m_Cost + cost);
      }

      private int GetProviderIndex(int storedIndex)
      {
        int providerIndex = this.m_ProviderIndex[storedIndex];
        if (providerIndex != storedIndex)
        {
          int index = storedIndex;
          storedIndex = providerIndex;
          providerIndex = this.m_ProviderIndex[storedIndex];
          if (providerIndex != storedIndex)
          {
            do
            {
              storedIndex = providerIndex;
              providerIndex = this.m_ProviderIndex[storedIndex];
            }
            while (providerIndex != storedIndex);
            this.m_ProviderIndex[index] = providerIndex;
          }
        }
        return providerIndex;
      }

      public void FillResults(ref UnsafeList<AvailabilityResult> results)
      {
        for (int index1 = 0; index1 < this.m_NodeData.Length; ++index1)
        {
          ref AvailabilityJobs.NodeData local1 = ref this.m_NodeData.ElementAt(index1);
          if (local1.m_Processed != 0)
          {
            int num = this.m_PathfindData.GetConnectionCount(local1.m_PathNode.m_NodeID);
            for (int connectionIndex = 0; connectionIndex < num; ++connectionIndex)
            {
              ref Edge local2 = ref this.m_PathfindData.GetEdge(new EdgeID()
              {
                m_Index = this.m_PathfindData.GetConnection(local1.m_PathNode.m_NodeID, connectionIndex)
              });
              if (local1.m_PathNode.Equals(new AvailabilityJobs.FullNode(local2.m_StartID, local2.m_StartCurvePos)) && (local2.m_Specification.m_Flags & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              {
                int index2;
                if (this.m_NodeIndex.TryGetValue(new AvailabilityJobs.FullNode(local2.m_EndID, local2.m_EndCurvePos), out index2))
                {
                  ref AvailabilityJobs.NodeData local3 = ref this.m_NodeData.ElementAt(index2);
                  if (local3.m_Processed != 0)
                    results.Add(new AvailabilityResult()
                    {
                      m_Target = local2.m_Owner,
                      m_Availability = this.NormalizeAvailability(new float2(local1.m_Availability.m_Availability, local3.m_Availability.m_Availability), this.m_Parameters)
                    });
                }
              }
              else
              {
                int index3;
                if (local1.m_PathNode.Equals(new AvailabilityJobs.FullNode(local2.m_EndID, local2.m_EndCurvePos)) && (local2.m_Specification.m_Flags & (EdgeFlags.Forward | EdgeFlags.Backward)) == EdgeFlags.Backward && this.m_NodeIndex.TryGetValue(new AvailabilityJobs.FullNode(local2.m_StartID, local2.m_StartCurvePos), out index3))
                {
                  ref AvailabilityJobs.NodeData local4 = ref this.m_NodeData.ElementAt(index3);
                  if (local4.m_Processed != 0)
                    results.Add(new AvailabilityResult()
                    {
                      m_Target = local2.m_Owner,
                      m_Availability = this.NormalizeAvailability(new float2(local4.m_Availability.m_Availability, local1.m_Availability.m_Availability), this.m_Parameters)
                    });
                }
              }
            }
          }
        }
      }

      private float2 NormalizeAvailability(
        float2 availability,
        AvailabilityParameters availabilityParameters)
      {
        return availability * availabilityParameters.m_ResultFactor;
      }
    }

    [BurstCompile]
    public struct AvailabilityJob : IJob
    {
      [ReadOnly]
      public NativePathfindData m_PathfindData;
      public AvailabilityAction m_Action;

      public void Execute()
      {
        AvailabilityJobs.AvailabilityJob.Execute(this.m_PathfindData, Allocator.Temp, ref this.m_Action.data);
      }

      public static void Execute(
        NativePathfindData pathfindData,
        Allocator allocator,
        ref AvailabilityActionData actionData)
      {
        if (actionData.m_Providers.IsEmpty())
          return;
        AvailabilityJobs.AvailabilityExecutor availabilityExecutor = new AvailabilityJobs.AvailabilityExecutor();
        availabilityExecutor.Initialize(pathfindData, allocator, actionData.m_Parameters);
        availabilityExecutor.AddSources(ref actionData.m_Sources);
        availabilityExecutor.AddProviders(ref actionData.m_Providers);
        if (availabilityExecutor.FindAvailabilityNodes())
          availabilityExecutor.FillResults(ref actionData.m_Results);
        availabilityExecutor.Release();
      }
    }

    public struct ResultItem
    {
      public Entity m_Owner;
      public UnsafeList<AvailabilityResult> m_Results;
    }

    [BurstCompile]
    public struct ProcessResultsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeList<AvailabilityJobs.ResultItem> m_ResultItems;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<AvailabilityElement> m_AvailabilityElements;

      public void Execute(int index)
      {
        AvailabilityJobs.ResultItem resultItem = this.m_ResultItems[index];
        if (!this.m_AvailabilityElements.HasBuffer(resultItem.m_Owner))
          return;
        NativeParallelHashMap<Entity, float2> nativeParallelHashMap = new NativeParallelHashMap<Entity, float2>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < resultItem.m_Results.Length; ++index1)
        {
          AvailabilityResult result = resultItem.m_Results[index1];
          if (this.m_EdgeLaneData.HasComponent(result.m_Target) && this.m_OwnerData.HasComponent(result.m_Target))
          {
            EdgeLane edgeLane = this.m_EdgeLaneData[result.m_Target];
            Owner owner = this.m_OwnerData[result.m_Target];
            float2 availability = AvailabilityJobs.ProcessResultsJob.GetAvailability(result.m_Availability, edgeLane.m_EdgeDelta);
            float2 x;
            if (nativeParallelHashMap.TryGetValue(owner.m_Owner, out x))
            {
              float2 float2 = math.max(x, availability);
              if (math.any(float2 != x))
                nativeParallelHashMap[owner.m_Owner] = float2;
            }
            else
              nativeParallelHashMap.Add(owner.m_Owner, availability);
          }
        }
        DynamicBuffer<AvailabilityElement> availabilityElement = this.m_AvailabilityElements[resultItem.m_Owner];
        availabilityElement.Clear();
        NativeParallelHashMap<Entity, float2>.Enumerator enumerator = nativeParallelHashMap.GetEnumerator();
        while (enumerator.MoveNext())
          availabilityElement.Add(new AvailabilityElement()
          {
            m_Edge = enumerator.Current.Key,
            m_Availability = enumerator.Current.Value
          });
        enumerator.Dispose();
        nativeParallelHashMap.Dispose();
      }

      private static float2 GetAvailability(float2 availability, float2 edgeDelta)
      {
        float2 float2 = new float2(0.0f, 1f);
        return math.select(math.select((float2) 0.0f, availability.yx, edgeDelta.yx == float2), availability, edgeDelta == float2);
      }
    }
  }
}
