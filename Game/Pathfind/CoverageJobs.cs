// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.CoverageJobs
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
  public static class CoverageJobs
  {
    private struct FullNode : IEquatable<CoverageJobs.FullNode>
    {
      public NodeID m_NodeID;
      public float m_CurvePos;

      public FullNode(NodeID nodeID, float curvePos)
      {
        this.m_NodeID = nodeID;
        this.m_CurvePos = curvePos;
      }

      public bool Equals(CoverageJobs.FullNode other)
      {
        return this.m_NodeID.Equals(other.m_NodeID) && (double) this.m_CurvePos == (double) other.m_CurvePos;
      }

      public override int GetHashCode() => this.m_NodeID.m_Index >> 2 ^ math.asint(this.m_CurvePos);
    }

    private struct NodeData
    {
      public int m_Processed;
      public int m_AccessRequirement;
      public float2 m_Costs;
      public EdgeID m_EdgeID;
      public EdgeID m_NextID;
      public CoverageJobs.FullNode m_PathNode;

      public NodeData(
        int accessRequirement,
        float cost,
        float distance,
        EdgeID edgeID,
        EdgeID nextID,
        CoverageJobs.FullNode pathNode)
      {
        this.m_Processed = 0;
        this.m_AccessRequirement = accessRequirement;
        this.m_Costs = new float2(cost, distance);
        this.m_EdgeID = edgeID;
        this.m_NextID = nextID;
        this.m_PathNode = pathNode;
      }
    }

    private struct HeapData : ILessThan<CoverageJobs.HeapData>, IComparable<CoverageJobs.HeapData>
    {
      public float m_Cost;
      public int m_NodeIndex;

      public HeapData(float cost, int nodeIndex)
      {
        this.m_Cost = cost;
        this.m_NodeIndex = nodeIndex;
      }

      public bool LessThan(CoverageJobs.HeapData other)
      {
        return (double) this.m_Cost < (double) other.m_Cost;
      }

      public int CompareTo(CoverageJobs.HeapData other) => this.m_NodeIndex - other.m_NodeIndex;
    }

    private struct CoverageExecutor
    {
      private UnsafePathfindData m_PathfindData;
      private CoverageParameters m_Parameters;
      private float4 m_MinDistance;
      private float4 m_MaxDistance;
      private UnsafeHashMap<CoverageJobs.FullNode, int> m_NodeIndex;
      private UnsafeMinHeap<CoverageJobs.HeapData> m_Heap;
      private UnsafeList<CoverageJobs.NodeData> m_NodeData;

      public void Initialize(
        NativePathfindData pathfindData,
        Allocator allocator,
        CoverageParameters parameters)
      {
        this.m_PathfindData = pathfindData.GetReadOnlyData();
        this.m_Parameters = parameters;
        this.m_MinDistance = parameters.m_Range * new float4(0.0f, 0.6f, 0.0f, 0.6f);
        this.m_MaxDistance = parameters.m_Range * new float4(2f, 1.2f, 2f, 1.2f);
        this.m_NodeIndex = new UnsafeHashMap<CoverageJobs.FullNode, int>(10000, (AllocatorManager.AllocatorHandle) allocator);
        this.m_Heap = new UnsafeMinHeap<CoverageJobs.HeapData>(1000, allocator);
        this.m_NodeData = new UnsafeList<CoverageJobs.NodeData>(10000, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Release()
      {
        if (!this.m_NodeData.IsCreated)
          return;
        this.m_NodeIndex.Dispose();
        this.m_Heap.Dispose();
        this.m_NodeData.Dispose();
      }

      public void AddSources(ref UnsafeQueue<PathTarget> pathTargets)
      {
        PathTarget pathTarget;
        while (pathTargets.TryDequeue(out pathTarget))
        {
          EdgeID edgeId;
          if (this.m_PathfindData.m_PathEdges.TryGetValue(pathTarget.m_Entity, out edgeId))
          {
            ref Edge local = ref this.m_PathfindData.GetEdge(edgeId);
            bool3 directions = new bool3((local.m_Specification.m_Flags & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary) || (double) pathTarget.m_Delta == 1.0, (local.m_Specification.m_Flags & EdgeFlags.AllowMiddle) != 0, (local.m_Specification.m_Flags & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary) || (double) pathTarget.m_Delta == 0.0);
            this.AddConnections(edgeId, in local, new float2(pathTarget.m_Cost, 0.0f), pathTarget.m_Delta, directions);
          }
        }
      }

      private bool HeapExtract(out CoverageJobs.HeapData heapData)
      {
        if (this.m_Heap.Length != 0)
        {
          heapData = this.m_Heap.Extract();
          return true;
        }
        heapData = new CoverageJobs.HeapData();
        return false;
      }

      private void HeapInsert(CoverageJobs.HeapData heapData) => this.m_Heap.Insert(heapData);

      public bool FindCoveredNodes()
      {
        bool coveredNodes = false;
        CoverageJobs.HeapData heapData;
        while (this.HeapExtract(out heapData))
        {
          ref CoverageJobs.NodeData local1 = ref this.m_NodeData.ElementAt(heapData.m_NodeIndex);
          if (local1.m_Processed == 0)
          {
            local1.m_Processed = 1;
            coveredNodes = true;
            if ((double) local1.m_Costs.y < (double) this.m_MaxDistance.y)
            {
              if (local1.m_NextID.m_Index != -1)
              {
                ref Edge local2 = ref this.m_PathfindData.GetEdge(local1.m_NextID);
                this.CheckNextEdge(local1.m_NextID, local1.m_PathNode, local1.m_Costs, in local2);
              }
              else
              {
                int num1 = this.m_PathfindData.GetConnectionCount(local1.m_PathNode.m_NodeID);
                int2 int2 = new int2(-1, local1.m_AccessRequirement);
                CoverageJobs.FullNode pathNode = local1.m_PathNode;
                float2 costs = local1.m_Costs;
                EdgeID edgeId1 = local1.m_EdgeID;
                for (int connectionIndex = 0; connectionIndex < num1; ++connectionIndex)
                {
                  EdgeID edgeId2 = new EdgeID()
                  {
                    m_Index = this.m_PathfindData.GetConnection(pathNode.m_NodeID, connectionIndex)
                  };
                  int num2 = this.m_PathfindData.GetAccessRequirement(pathNode.m_NodeID, connectionIndex);
                  if (!edgeId1.Equals(edgeId2) && !math.all(int2 != num2))
                  {
                    ref Edge local3 = ref this.m_PathfindData.GetEdge(edgeId2);
                    if (!this.DisallowConnection(local3.m_Specification))
                      this.CheckNextEdge(edgeId2, pathNode, costs, in local3);
                  }
                }
              }
            }
          }
        }
        return coveredNodes;
      }

      private void CheckNextEdge(
        EdgeID nextID,
        CoverageJobs.FullNode pathNode,
        float2 baseCosts,
        in Edge edge)
      {
        float startDelta;
        bool3 directions;
        if (pathNode.Equals(new CoverageJobs.FullNode(edge.m_StartID, edge.m_StartCurvePos)))
        {
          startDelta = 0.0f;
          directions = new bool3((edge.m_Specification.m_Flags & EdgeFlags.Forward) != 0, (edge.m_Specification.m_Flags & (EdgeFlags.Forward | EdgeFlags.AllowMiddle)) == (EdgeFlags.Forward | EdgeFlags.AllowMiddle), false);
        }
        else if (pathNode.Equals(new CoverageJobs.FullNode(edge.m_EndID, edge.m_EndCurvePos)))
        {
          startDelta = 1f;
          directions = new bool3(false, (edge.m_Specification.m_Flags & (EdgeFlags.Backward | EdgeFlags.AllowMiddle)) == (EdgeFlags.Backward | EdgeFlags.AllowMiddle), (edge.m_Specification.m_Flags & EdgeFlags.Backward) != 0);
        }
        else
        {
          if (!pathNode.m_NodeID.Equals(edge.m_MiddleID))
            return;
          startDelta = pathNode.m_CurvePos;
          directions = new bool3((edge.m_Specification.m_Flags & EdgeFlags.Forward) != 0, (edge.m_Specification.m_Flags & EdgeFlags.AllowMiddle) != 0, (edge.m_Specification.m_Flags & EdgeFlags.Backward) != 0);
        }
        this.AddConnections(nextID, in edge, baseCosts, startDelta, directions);
      }

      private void AddConnections(
        EdgeID id,
        in Edge edge,
        float2 baseCosts,
        float startDelta,
        bool3 directions)
      {
        if (directions.x)
        {
          CoverageJobs.FullNode pathNode = new CoverageJobs.FullNode(edge.m_EndID, edge.m_EndCurvePos);
          float2 edgeDelta = new float2(startDelta, 1f);
          this.AddHeapData(id, in edge, baseCosts, pathNode, edgeDelta);
        }
        if (directions.y)
        {
          int num1 = this.m_PathfindData.GetConnectionCount(edge.m_MiddleID);
          if (num1 != 0)
          {
            int2 int2 = new int2(-1, edge.m_Specification.m_AccessRequirement);
            for (int connectionIndex = 0; connectionIndex < num1; ++connectionIndex)
            {
              EdgeID edgeId = new EdgeID()
              {
                m_Index = this.m_PathfindData.GetConnection(edge.m_MiddleID, connectionIndex)
              };
              int num2 = this.m_PathfindData.GetAccessRequirement(edge.m_MiddleID, connectionIndex);
              if (!id.Equals(edgeId) && !math.all(int2 != num2))
              {
                ref Edge local = ref this.m_PathfindData.GetEdge(edgeId);
                if (!this.DisallowConnection(local.m_Specification))
                {
                  if (edge.m_MiddleID.Equals(local.m_StartID) & (local.m_Specification.m_Flags & EdgeFlags.Forward) != 0)
                  {
                    float startCurvePos = local.m_StartCurvePos;
                    if (directions.x & (double) startCurvePos >= (double) startDelta | directions.z & (double) startCurvePos <= (double) startDelta)
                    {
                      CoverageJobs.FullNode pathNode = new CoverageJobs.FullNode(local.m_StartID, startCurvePos);
                      float2 edgeDelta = new float2(startDelta, startCurvePos);
                      this.AddHeapData(id, edgeId, in edge, baseCosts, pathNode, edgeDelta);
                    }
                  }
                  if (edge.m_MiddleID.Equals(local.m_EndID) & (local.m_Specification.m_Flags & EdgeFlags.Backward) != 0)
                  {
                    float endCurvePos = local.m_EndCurvePos;
                    if (directions.x & (double) endCurvePos >= (double) startDelta | directions.z & (double) endCurvePos <= (double) startDelta)
                    {
                      CoverageJobs.FullNode pathNode = new CoverageJobs.FullNode(local.m_EndID, endCurvePos);
                      float2 edgeDelta = new float2(startDelta, endCurvePos);
                      this.AddHeapData(id, edgeId, in edge, baseCosts, pathNode, edgeDelta);
                    }
                  }
                }
              }
            }
          }
        }
        if (!directions.z)
          return;
        CoverageJobs.FullNode pathNode1 = new CoverageJobs.FullNode(edge.m_StartID, edge.m_StartCurvePos);
        float2 edgeDelta1 = new float2(startDelta, 0.0f);
        this.AddHeapData(id, in edge, baseCosts, pathNode1, edgeDelta1);
      }

      private void AddHeapData(
        EdgeID id,
        in Edge edge,
        float2 baseCosts,
        CoverageJobs.FullNode pathNode,
        float2 edgeDelta)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref CoverageJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          if (local.m_Processed != 0)
            return;
          float cost = baseCosts.x + PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, edgeDelta);
          if ((double) cost >= (double) local.m_Costs.x)
            return;
          float distance = baseCosts.y + edge.m_Specification.m_Length * math.abs(edgeDelta.x - edgeDelta.y);
          local = new CoverageJobs.NodeData(edge.m_Specification.m_AccessRequirement, cost, distance, id, new EdgeID()
          {
            m_Index = -1
          }, pathNode);
          this.HeapInsert(new CoverageJobs.HeapData(cost, num));
        }
        else
        {
          int length = this.m_NodeData.Length;
          float cost = baseCosts.x + PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, edgeDelta);
          float distance = baseCosts.y + edge.m_Specification.m_Length * math.abs(edgeDelta.x - edgeDelta.y);
          this.m_NodeData.Add(new CoverageJobs.NodeData(edge.m_Specification.m_AccessRequirement, cost, distance, id, new EdgeID()
          {
            m_Index = -1
          }, pathNode));
          this.HeapInsert(new CoverageJobs.HeapData(cost, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private void AddHeapData(
        EdgeID id,
        EdgeID id2,
        in Edge edge,
        float2 baseCosts,
        CoverageJobs.FullNode pathNode,
        float2 edgeDelta)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref CoverageJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          if (local.m_Processed != 0)
            return;
          float cost = baseCosts.x + PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, edgeDelta);
          if ((double) cost < (double) local.m_Costs.x)
          {
            float distance = baseCosts.y + edge.m_Specification.m_Length * math.abs(edgeDelta.x - edgeDelta.y);
            local = new CoverageJobs.NodeData(edge.m_Specification.m_AccessRequirement, cost, distance, id, id2, pathNode);
            this.HeapInsert(new CoverageJobs.HeapData(cost, num));
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
          float cost = baseCosts.x + PathUtils.CalculateCost(in edge.m_Specification, in this.m_Parameters, edgeDelta);
          float distance = baseCosts.y + edge.m_Specification.m_Length * math.abs(edgeDelta.x - edgeDelta.y);
          this.m_NodeData.Add(new CoverageJobs.NodeData(edge.m_Specification.m_AccessRequirement, cost, distance, id, id2, pathNode));
          this.HeapInsert(new CoverageJobs.HeapData(cost, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private bool DisallowConnection(PathSpecification newSpec)
      {
        return (newSpec.m_Methods & this.m_Parameters.m_Methods) == (PathMethod) 0;
      }

      public void FillResults(ref UnsafeList<CoverageResult> results)
      {
        for (int index1 = 0; index1 < this.m_NodeData.Length; ++index1)
        {
          ref CoverageJobs.NodeData local1 = ref this.m_NodeData.ElementAt(index1);
          if (local1.m_Processed != 0)
          {
            int num = this.m_PathfindData.GetConnectionCount(local1.m_PathNode.m_NodeID);
            for (int connectionIndex = 0; connectionIndex < num; ++connectionIndex)
            {
              ref Edge local2 = ref this.m_PathfindData.GetEdge(new EdgeID()
              {
                m_Index = this.m_PathfindData.GetConnection(local1.m_PathNode.m_NodeID, connectionIndex)
              });
              if (local1.m_PathNode.Equals(new CoverageJobs.FullNode(local2.m_StartID, local2.m_StartCurvePos)) && (local2.m_Specification.m_Flags & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              {
                int index2;
                if (this.m_NodeIndex.TryGetValue(new CoverageJobs.FullNode(local2.m_EndID, local2.m_EndCurvePos), out index2))
                {
                  ref CoverageJobs.NodeData local3 = ref this.m_NodeData.ElementAt(index2);
                  if (local3.m_Processed != 0 && (double) math.min(local1.m_Costs.y, local3.m_Costs.y) < (double) this.m_MaxDistance.y)
                  {
                    float4 float4 = (new float4(local1.m_Costs, local3.m_Costs) - this.m_MinDistance) / (this.m_MaxDistance - this.m_MinDistance);
                    results.Add(new CoverageResult()
                    {
                      m_Target = local2.m_Owner,
                      m_TargetCost = math.saturate(math.max(float4.xz, float4.yw))
                    });
                  }
                }
              }
              else
              {
                int index3;
                if (local1.m_PathNode.Equals(new CoverageJobs.FullNode(local2.m_EndID, local2.m_EndCurvePos)) && (local2.m_Specification.m_Flags & (EdgeFlags.Forward | EdgeFlags.Backward)) == EdgeFlags.Backward && this.m_NodeIndex.TryGetValue(new CoverageJobs.FullNode(local2.m_StartID, local2.m_StartCurvePos), out index3))
                {
                  ref CoverageJobs.NodeData local4 = ref this.m_NodeData.ElementAt(index3);
                  if (local4.m_Processed != 0 && (double) math.min(local1.m_Costs.y, local4.m_Costs.y) < (double) this.m_MaxDistance.y)
                  {
                    float4 float4 = (new float4(local4.m_Costs, local1.m_Costs) - this.m_MinDistance) / (this.m_MaxDistance - this.m_MinDistance);
                    results.Add(new CoverageResult()
                    {
                      m_Target = local2.m_Owner,
                      m_TargetCost = math.saturate(math.max(float4.xz, float4.yw))
                    });
                  }
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    public struct CoverageJob : IJob
    {
      [ReadOnly]
      public NativePathfindData m_PathfindData;
      public CoverageAction m_Action;

      public void Execute()
      {
        CoverageJobs.CoverageJob.Execute(this.m_PathfindData, Allocator.Temp, ref this.m_Action.data);
      }

      public static void Execute(
        NativePathfindData pathfindData,
        Allocator allocator,
        ref CoverageActionData actionData)
      {
        if (actionData.m_Sources.IsEmpty())
          return;
        CoverageJobs.CoverageExecutor coverageExecutor = new CoverageJobs.CoverageExecutor();
        coverageExecutor.Initialize(pathfindData, allocator, actionData.m_Parameters);
        coverageExecutor.AddSources(ref actionData.m_Sources);
        if (coverageExecutor.FindCoveredNodes())
          coverageExecutor.FillResults(ref actionData.m_Results);
        coverageExecutor.Release();
      }
    }

    public struct ResultItem
    {
      public Entity m_Owner;
      public UnsafeList<CoverageResult> m_Results;
    }

    [BurstCompile]
    public struct ProcessResultsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeList<CoverageJobs.ResultItem> m_ResultItems;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<CoverageElement> m_CoverageElements;

      public void Execute(int index)
      {
        CoverageJobs.ResultItem resultItem = this.m_ResultItems[index];
        if (!this.m_CoverageElements.HasBuffer(resultItem.m_Owner))
          return;
        NativeParallelHashMap<Entity, float2> nativeParallelHashMap = new NativeParallelHashMap<Entity, float2>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < resultItem.m_Results.Length; ++index1)
        {
          CoverageResult result = resultItem.m_Results[index1];
          if (this.m_EdgeLaneData.HasComponent(result.m_Target))
          {
            EdgeLane edgeLane = this.m_EdgeLaneData[result.m_Target];
            Owner owner = this.m_OwnerData[result.m_Target];
            float2 cost = CoverageJobs.ProcessResultsJob.GetCost(result.m_TargetCost, edgeLane.m_EdgeDelta);
            float2 x;
            if (nativeParallelHashMap.TryGetValue(owner.m_Owner, out x))
            {
              if (math.any(cost < x))
              {
                float2 float2 = math.min(x, cost);
                nativeParallelHashMap.Remove(owner.m_Owner);
                nativeParallelHashMap.TryAdd(owner.m_Owner, float2);
              }
            }
            else
            {
              nativeParallelHashMap.TryAdd(owner.m_Owner, cost);
              nativeList.Add(in owner.m_Owner);
            }
          }
        }
        DynamicBuffer<CoverageElement> coverageElement = this.m_CoverageElements[resultItem.m_Owner];
        coverageElement.Clear();
        for (int index2 = 0; index2 < nativeList.Length; ++index2)
        {
          CoverageElement elem = new CoverageElement();
          elem.m_Edge = nativeList[index2];
          if (nativeParallelHashMap.TryGetValue(elem.m_Edge, out elem.m_Cost))
            coverageElement.Add(elem);
        }
        nativeParallelHashMap.Dispose();
        nativeList.Dispose();
      }

      private static float2 GetCost(float2 cost, float2 edgeDelta)
      {
        float2 float2 = new float2(0.0f, 1f);
        return math.select(math.select((float2) float.MaxValue, cost.yx, edgeDelta.yx == float2), cost, edgeDelta == float2);
      }
    }
  }
}
