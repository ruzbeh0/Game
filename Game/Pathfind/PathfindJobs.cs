// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
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
  public static class PathfindJobs
  {
    private struct FullNode : IEquatable<PathfindJobs.FullNode>
    {
      public NodeID m_NodeID;
      public float m_CurvePos;

      public FullNode(NodeID nodeID, float curvePos)
      {
        this.m_NodeID = nodeID;
        this.m_CurvePos = curvePos;
      }

      public FullNode(EdgeID edgeID, float curvePos)
      {
        this.m_NodeID = new NodeID()
        {
          m_Index = -4 - edgeID.m_Index
        };
        this.m_CurvePos = curvePos;
      }

      public bool Equals(PathfindJobs.FullNode other)
      {
        return this.m_NodeID.Equals(other.m_NodeID) && (double) this.m_CurvePos == (double) other.m_CurvePos;
      }

      public override int GetHashCode() => this.m_NodeID.m_Index >> 2 ^ math.asint(this.m_CurvePos);
    }

    [Flags]
    private enum PathfindItemFlags : ushort
    {
      End = 1,
      SingleOnly = 2,
      NextEdge = 4,
      ReducedCost = 8,
      ForbidExit = 16, // 0x0010
      ReducedAccess = 32, // 0x0020
    }

    private struct NodeData
    {
      public int m_SourceIndex;
      public float m_TotalCost;
      public float m_BaseCost;
      public int m_AccessRequirement;
      public EdgeID m_EdgeID;
      public EdgeID m_NextID;
      public float2 m_EdgeDelta;
      public PathfindJobs.FullNode m_PathNode;
      public PathfindJobs.PathfindItemFlags m_Flags;
      public PathMethod m_Method;

      public NodeData(
        int sourceIndex,
        float totalCost,
        float baseCost,
        int accessRequirement,
        EdgeID edgeID,
        EdgeID nextID,
        float2 edgeDelta,
        PathfindJobs.FullNode pathNode,
        PathfindJobs.PathfindItemFlags flags,
        PathMethod method)
      {
        this.m_SourceIndex = sourceIndex;
        this.m_TotalCost = totalCost;
        this.m_BaseCost = baseCost;
        this.m_AccessRequirement = accessRequirement;
        this.m_EdgeID = edgeID;
        this.m_NextID = nextID;
        this.m_EdgeDelta = edgeDelta;
        this.m_PathNode = pathNode;
        this.m_Flags = flags;
        this.m_Method = method;
      }
    }

    private struct HeapData : ILessThan<PathfindJobs.HeapData>, IComparable<PathfindJobs.HeapData>
    {
      public float m_TotalCost;
      public int m_NodeIndex;

      public HeapData(float totalCost, int nodeIndex)
      {
        this.m_TotalCost = totalCost;
        this.m_NodeIndex = nodeIndex;
      }

      public bool LessThan(PathfindJobs.HeapData other)
      {
        return (double) this.m_TotalCost < (double) other.m_TotalCost;
      }

      public int CompareTo(PathfindJobs.HeapData other) => this.m_NodeIndex - other.m_NodeIndex;
    }

    private struct TargetData
    {
      public Entity m_Entity;
      public float m_Cost;

      public TargetData(Entity entity, float cost)
      {
        this.m_Entity = entity;
        this.m_Cost = cost;
      }
    }

    private struct PathfindExecutor
    {
      private UnsafePathfindData m_PathfindData;
      private Allocator m_Allocator;
      private Unity.Mathematics.Random m_Random;
      private PathfindParameters m_Parameters;
      private Bounds3 m_StartBounds;
      private Bounds3 m_EndBounds;
      private int3 m_AccessMask;
      private int2 m_AuthorizationMask;
      private Entity m_ParkingOwner;
      private float m_HeuristicCostFactor;
      private float m_MaxTotalCost;
      private float m_CostOffset;
      private float m_ReducedCostFactor;
      private float2 m_ParkingSize;
      private int m_MaxResultCount;
      private bool m_InvertPath;
      private bool m_ParkingReset;
      private EdgeFlags m_Forward;
      private EdgeFlags m_Backward;
      private EdgeFlags m_ForwardMiddle;
      private EdgeFlags m_BackwardMiddle;
      private EdgeFlags m_FreeForward;
      private EdgeFlags m_FreeBackward;
      private EdgeFlags m_ParkingEdgeMask;
      private PathMethod m_ParkingMethodMask;
      private UnsafeHashMap<PathfindJobs.FullNode, PathfindJobs.TargetData> m_StartTargets;
      private UnsafeHashMap<PathfindJobs.FullNode, PathfindJobs.TargetData> m_EndTargets;
      private UnsafeHashMap<PathfindJobs.FullNode, int> m_NodeIndex;
      private UnsafeParallelMultiHashMap<EdgeID, PathTarget> m_Ends;
      private UnsafeMinHeap<PathfindJobs.HeapData> m_Heap;
      private UnsafeList<PathfindJobs.NodeData> m_NodeData;

      public void Initialize(
        NativePathfindData pathfindData,
        Allocator allocator,
        Unity.Mathematics.Random random,
        PathfindParameters parameters,
        PathfindHeuristicData pathfindHeuristicData,
        float maxPassengerTransportSpeed,
        float maxCargoTransportSpeed)
      {
        this.m_PathfindData = pathfindData.GetReadOnlyData();
        this.m_Allocator = allocator;
        this.m_Random = random;
        this.m_Parameters = parameters;
        this.m_ReducedCostFactor = 1f;
        if ((parameters.m_PathfindFlags & PathfindFlags.ParkingReset) != (PathfindFlags) 0)
        {
          this.m_ReducedCostFactor = 0.5f;
          this.m_ParkingReset = true;
        }
        if ((parameters.m_PathfindFlags & PathfindFlags.NoHeuristics) != (PathfindFlags) 0)
        {
          this.m_HeuristicCostFactor = 0.0f;
        }
        else
        {
          this.m_HeuristicCostFactor = 1000000f;
          if ((parameters.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0)
          {
            PathfindCosts pedestrianCosts = pathfindHeuristicData.m_PedestrianCosts;
            pedestrianCosts.m_Value.x += 1f / math.max(0.01f, parameters.m_WalkSpeed.x);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(pedestrianCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & PathMethod.Road) != (PathMethod) 0)
          {
            PathfindCosts carCosts = pathfindHeuristicData.m_CarCosts;
            carCosts.m_Value.x += 1f / math.max(0.01f, parameters.m_MaxSpeed.x);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(carCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & PathMethod.Track) != (PathMethod) 0)
          {
            PathfindCosts trackCosts = pathfindHeuristicData.m_TrackCosts;
            trackCosts.m_Value.x += 1f / math.max(0.01f, parameters.m_MaxSpeed.x);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(trackCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & PathMethod.Flying) != (PathMethod) 0)
          {
            PathfindCosts flyingCosts = pathfindHeuristicData.m_FlyingCosts;
            flyingCosts.m_Value.x += 1f / math.max(0.01f, parameters.m_MaxSpeed.x);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(flyingCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & PathMethod.Offroad) != (PathMethod) 0)
          {
            PathfindCosts offRoadCosts = pathfindHeuristicData.m_OffRoadCosts;
            offRoadCosts.m_Value.x += 1f / math.max(0.01f, parameters.m_MaxSpeed.x);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(offRoadCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & PathMethod.Taxi) != (PathMethod) 0)
          {
            PathfindCosts taxiCosts = pathfindHeuristicData.m_TaxiCosts;
            taxiCosts.m_Value.x += 1f / math.max(0.01f, 111.111115f);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(taxiCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & (PathMethod.PublicTransportDay | PathMethod.PublicTransportNight)) != (PathMethod) 0)
          {
            PathfindCosts pathfindCosts = new PathfindCosts();
            pathfindCosts.m_Value.x += 1f / math.max(0.01f, maxPassengerTransportSpeed);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(pathfindCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_Methods & PathMethod.CargoTransport) != (PathMethod) 0)
          {
            PathfindCosts pathfindCosts = new PathfindCosts();
            pathfindCosts.m_Value.x += 1f / math.max(0.01f, maxCargoTransportSpeed);
            this.m_HeuristicCostFactor = math.min(this.m_HeuristicCostFactor, math.dot(pathfindCosts.m_Value, parameters.m_Weights.m_Value));
          }
          if ((parameters.m_PathfindFlags & PathfindFlags.Stable) == (PathfindFlags) 0)
            this.m_HeuristicCostFactor *= 2f;
          this.m_HeuristicCostFactor *= this.m_ReducedCostFactor;
        }
        if (parameters.m_ParkingTarget != Entity.Null && (double) parameters.m_ParkingDelta >= 0.0)
          this.m_ParkingOwner = parameters.m_ParkingTarget;
        this.m_ParkingSize = (this.m_Parameters.m_Methods & PathMethod.Boarding) == (PathMethod) 0 ? this.m_Parameters.m_ParkingSize : (float2) float.MinValue;
        this.m_ParkingEdgeMask = this.m_Parameters.m_ParkingTarget == Entity.Null ? EdgeFlags.OutsideConnection : ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary);
        this.m_NodeIndex = new UnsafeHashMap<PathfindJobs.FullNode, int>(10000, (AllocatorManager.AllocatorHandle) allocator);
        this.m_Heap = new UnsafeMinHeap<PathfindJobs.HeapData>(1000, allocator);
        this.m_NodeData = new UnsafeList<PathfindJobs.NodeData>(10000, (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Release()
      {
        if (this.m_NodeData.IsCreated)
        {
          this.m_NodeIndex.Dispose();
          this.m_Heap.Dispose();
          this.m_NodeData.Dispose();
        }
        if (this.m_EndTargets.IsCreated)
        {
          this.m_EndTargets.Dispose();
          this.m_Ends.Dispose();
        }
        if (!this.m_StartTargets.IsCreated)
          return;
        this.m_StartTargets.Dispose();
      }

      public void AddTargets(
        UnsafeList<PathTarget> startTargets,
        UnsafeList<PathTarget> endTargets,
        ref ErrorCode errorCode)
      {
        float minCost1;
        int accessRequirement1;
        bool multipleRequirements1;
        this.m_StartBounds = this.GetTargetBounds(startTargets, out minCost1, out accessRequirement1, out multipleRequirements1);
        float minCost2;
        int accessRequirement2;
        bool multipleRequirements2;
        this.m_EndBounds = this.GetTargetBounds(endTargets, out minCost2, out accessRequirement2, out multipleRequirements2);
        if (multipleRequirements1 & multipleRequirements2)
        {
          multipleRequirements1 = (this.m_Parameters.m_PathfindFlags & (PathfindFlags.IgnoreExtraStartAccessRequirements | PathfindFlags.IgnoreExtraEndAccessRequirements)) != PathfindFlags.IgnoreExtraStartAccessRequirements;
          multipleRequirements2 = (this.m_Parameters.m_PathfindFlags & PathfindFlags.IgnoreExtraEndAccessRequirements) == (PathfindFlags) 0;
        }
        this.m_InvertPath = (this.m_Parameters.m_PathfindFlags & PathfindFlags.ForceForward) == (PathfindFlags) 0 && ((this.m_Parameters.m_PathfindFlags & PathfindFlags.ForceBackward) != (PathfindFlags) 0 || (this.m_Parameters.m_PathfindFlags & PathfindFlags.MultipleDestinations) == (PathfindFlags) 0 && ((this.m_Parameters.m_PathfindFlags & PathfindFlags.MultipleOrigins) != (PathfindFlags) 0 || !multipleRequirements1 && (multipleRequirements2 || (double) math.lengthsq(MathUtils.Size(this.m_StartBounds)) < (double) math.lengthsq(MathUtils.Size(this.m_EndBounds)))));
        if (this.m_InvertPath)
        {
          CommonUtils.Swap<UnsafeList<PathTarget>>(ref startTargets, ref endTargets);
          CommonUtils.Swap<Bounds3>(ref this.m_StartBounds, ref this.m_EndBounds);
          CommonUtils.Swap<float>(ref minCost1, ref minCost2);
          CommonUtils.Swap<int>(ref accessRequirement1, ref accessRequirement2);
          CommonUtils.Swap<bool>(ref multipleRequirements1, ref multipleRequirements2);
          this.m_PathfindData.SwapConnections();
          this.m_Forward = EdgeFlags.Backward;
          this.m_Backward = EdgeFlags.Forward;
          this.m_FreeForward = EdgeFlags.FreeBackward;
          this.m_FreeBackward = EdgeFlags.FreeForward;
          this.m_ParkingMethodMask = PathMethod.Road | PathMethod.Track | PathMethod.Flying | PathMethod.Offroad;
        }
        else
        {
          this.m_Forward = EdgeFlags.Forward;
          this.m_Backward = EdgeFlags.Backward;
          this.m_FreeForward = EdgeFlags.FreeForward;
          this.m_FreeBackward = EdgeFlags.FreeBackward;
          this.m_ParkingMethodMask = PathMethod.Pedestrian | PathMethod.PublicTransportDay | PathMethod.Taxi | PathMethod.PublicTransportNight;
        }
        if (multipleRequirements2)
          errorCode = ErrorCode.TooManyEndAccessRequirements;
        if ((this.m_Parameters.m_PathfindFlags & PathfindFlags.MultipleDestinations) != (PathfindFlags) 0 && this.m_InvertPath || (this.m_Parameters.m_PathfindFlags & PathfindFlags.MultipleOrigins) != (PathfindFlags) 0 && !this.m_InvertPath)
          errorCode = ErrorCode.MultipleStartResults;
        this.m_AccessMask = new int3(-1, -1, accessRequirement2);
        EdgeID edgeID;
        if (this.m_PathfindData.m_PathEdges.TryGetValue(this.m_Parameters.m_ParkingTarget, out edgeID))
          this.m_AccessMask.y = this.m_PathfindData.GetEdge(edgeID).m_Specification.m_AccessRequirement;
        this.m_AuthorizationMask = math.select((int2) -2, new int2(this.m_Parameters.m_Authorization1.Index, this.m_Parameters.m_Authorization2.Index), new bool2(this.m_Parameters.m_Authorization1 != Entity.Null, this.m_Parameters.m_Authorization2 != Entity.Null));
        this.m_ForwardMiddle = this.m_Forward | EdgeFlags.AllowMiddle;
        this.m_BackwardMiddle = this.m_Backward | EdgeFlags.AllowMiddle;
        this.AddEndTargets(endTargets, minCost2);
        this.AddStartTargets(startTargets, minCost1);
        this.m_CostOffset = minCost1 + minCost2;
        this.m_MaxTotalCost = math.select(this.m_Parameters.m_MaxCost - this.m_CostOffset, float.MaxValue, (double) this.m_Parameters.m_MaxCost == 0.0);
        this.m_MaxResultCount = math.select(1, this.m_Parameters.m_MaxResultCount, this.m_Parameters.m_MaxResultCount > 1 && (this.m_Parameters.m_PathfindFlags & (PathfindFlags.MultipleOrigins | PathfindFlags.MultipleDestinations)) != 0);
      }

      public Bounds3 GetTargetBounds(
        UnsafeList<PathTarget> pathTargets,
        out float minCost,
        out int accessRequirement,
        out bool multipleRequirements)
      {
        Bounds3 targetBounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        int length = pathTargets.Length;
        minCost = float.MaxValue;
        accessRequirement = -1;
        multipleRequirements = false;
        for (int index = 0; index < length; ++index)
        {
          PathTarget pathTarget = pathTargets[index];
          EdgeID edgeID;
          if ((pathTarget.m_Flags & EdgeFlags.Secondary) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          {
            if (!this.m_PathfindData.m_SecondaryEdges.TryGetValue(pathTarget.m_Entity, out edgeID))
              continue;
          }
          else if (!this.m_PathfindData.m_PathEdges.TryGetValue(pathTarget.m_Entity, out edgeID))
            continue;
          ref Edge local = ref this.m_PathfindData.GetEdge(edgeID);
          targetBounds |= MathUtils.Position(local.m_Location.m_Line, pathTarget.m_Delta);
          minCost = math.min(minCost, pathTarget.m_Cost);
          if (local.m_Specification.m_AccessRequirement != -1 & local.m_Specification.m_AccessRequirement != accessRequirement & (local.m_Specification.m_Flags & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          {
            multipleRequirements = accessRequirement != -1;
            accessRequirement = math.select(local.m_Specification.m_AccessRequirement, accessRequirement, multipleRequirements);
          }
        }
        return targetBounds;
      }

      private void AddEndTargets(UnsafeList<PathTarget> pathTargets, float minCost)
      {
        int length = pathTargets.Length;
        this.m_EndTargets = new UnsafeHashMap<PathfindJobs.FullNode, PathfindJobs.TargetData>(length, (AllocatorManager.AllocatorHandle) this.m_Allocator);
        this.m_Ends = new UnsafeParallelMultiHashMap<EdgeID, PathTarget>(length, (AllocatorManager.AllocatorHandle) this.m_Allocator);
        for (int index = 0; index < length; ++index)
        {
          PathTarget pathTarget = pathTargets[index];
          pathTarget.m_Cost -= minCost;
          EdgeID edgeId;
          if ((pathTarget.m_Flags & EdgeFlags.Secondary) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          {
            if (!this.m_PathfindData.m_SecondaryEdges.TryGetValue(pathTarget.m_Entity, out edgeId))
              continue;
          }
          else if (!this.m_PathfindData.m_PathEdges.TryGetValue(pathTarget.m_Entity, out edgeId))
            continue;
          PathfindJobs.FullNode key = new PathfindJobs.FullNode(edgeId, pathTarget.m_Delta);
          PathfindJobs.TargetData targetData = new PathfindJobs.TargetData(pathTarget.m_Target, pathTarget.m_Cost);
          if (!this.m_EndTargets.TryAdd(key, targetData))
          {
            PathfindJobs.TargetData endTarget = this.m_EndTargets[key];
            if ((double) targetData.m_Cost < (double) endTarget.m_Cost)
              this.m_EndTargets[key] = targetData;
          }
          this.m_Ends.Add(edgeId, pathTarget);
        }
      }

      private void AddStartTargets(UnsafeList<PathTarget> pathTargets, float minCost)
      {
        int length = pathTargets.Length;
        this.m_StartTargets = new UnsafeHashMap<PathfindJobs.FullNode, PathfindJobs.TargetData>(length, (AllocatorManager.AllocatorHandle) this.m_Allocator);
        for (int index = 0; index < length; ++index)
        {
          PathTarget pathTarget = pathTargets[index];
          pathTarget.m_Cost -= minCost;
          bool flag = (pathTarget.m_Flags & EdgeFlags.Secondary) != 0;
          EdgeID edgeId;
          if (flag)
          {
            if (!this.m_PathfindData.m_SecondaryEdges.TryGetValue(pathTarget.m_Entity, out edgeId))
              continue;
          }
          else if (!this.m_PathfindData.m_PathEdges.TryGetValue(pathTarget.m_Entity, out edgeId))
            continue;
          ref Edge local = ref this.m_PathfindData.GetEdge(edgeId);
          PathfindJobs.FullNode key = new PathfindJobs.FullNode(edgeId, pathTarget.m_Delta);
          PathfindJobs.TargetData targetData = new PathfindJobs.TargetData(pathTarget.m_Target, pathTarget.m_Cost);
          if (!this.m_StartTargets.TryAdd(key, targetData))
          {
            PathfindJobs.TargetData startTarget = this.m_StartTargets[key];
            if ((double) targetData.m_Cost < (double) startTarget.m_Cost)
              this.m_StartTargets[key] = targetData;
          }
          EdgeFlags flags1 = local.m_Specification.m_Flags;
          RuleFlags rules1 = local.m_Specification.m_Rules;
          EdgeFlags flags2 = flags1 & pathTarget.m_Flags;
          RuleFlags rules2 = rules1 & (RuleFlags) ~(flag ? (int) this.m_Parameters.m_SecondaryIgnoredRules : (int) this.m_Parameters.m_IgnoredRules);
          bool3 directions = new bool3((flags2 & this.m_Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary) || (double) pathTarget.m_Delta == 1.0, (flags2 & EdgeFlags.AllowMiddle) != 0, (flags2 & this.m_Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary) || (double) pathTarget.m_Delta == 0.0);
          bool reducedCost = this.m_ParkingReset && (local.m_Specification.m_Methods & (PathMethod.Parking | PathMethod.SpecialParking)) != 0;
          bool reducedAccess = local.m_Specification.m_AccessRequirement != -1 && (local.m_Specification.m_Flags & EdgeFlags.AllowEnter) != 0;
          this.AddConnections(int.MaxValue, edgeId, in local, flags2, rules2, pathTarget.m_Cost, pathTarget.m_Delta, directions, reducedCost, false, reducedAccess);
        }
      }

      private bool HeapExtract(out PathfindJobs.HeapData heapData)
      {
        if (this.m_Heap.Length != 0)
        {
          heapData = this.m_Heap.Extract();
          return true;
        }
        heapData = new PathfindJobs.HeapData();
        return false;
      }

      private void HeapInsert(PathfindJobs.HeapData heapData) => this.m_Heap.Insert(heapData);

      public bool FindEndNode(out int endNode, out float travelCost, out int graphTraversal)
      {
        endNode = 0;
        travelCost = -1f;
        graphTraversal = this.m_NodeData.Length;
        if (this.m_MaxResultCount == 0)
          return false;
        PathfindJobs.HeapData heapData;
        while (this.HeapExtract(out heapData))
        {
          ref PathfindJobs.NodeData local1 = ref this.m_NodeData.ElementAt(heapData.m_NodeIndex);
          if (local1.m_SourceIndex < 0)
          {
            local1.m_SourceIndex = -1 - local1.m_SourceIndex;
            if ((double) heapData.m_TotalCost > (double) this.m_MaxTotalCost)
            {
              endNode = 0;
              travelCost = float.MaxValue;
              graphTraversal = this.m_NodeData.Length;
              return false;
            }
            if ((local1.m_Flags & PathfindJobs.PathfindItemFlags.End) != (PathfindJobs.PathfindItemFlags) 0)
            {
              endNode = heapData.m_NodeIndex;
              travelCost = heapData.m_TotalCost + this.m_CostOffset;
              graphTraversal = this.m_NodeData.Length;
              --this.m_MaxResultCount;
              return true;
            }
            if ((local1.m_Flags & PathfindJobs.PathfindItemFlags.NextEdge) != (PathfindJobs.PathfindItemFlags) 0)
            {
              ref Edge local2 = ref this.m_PathfindData.GetEdge(local1.m_NextID);
              this.CheckNextEdge(heapData.m_NodeIndex, local1.m_NextID, local1.m_PathNode, local1.m_BaseCost, local1.m_Flags, in local2);
            }
            else
            {
              int num1 = this.m_PathfindData.GetConnectionCount(local1.m_PathNode.m_NodeID);
              PathfindJobs.PathfindItemFlags flags1 = local1.m_Flags;
              bool flag1 = (flags1 & PathfindJobs.PathfindItemFlags.ForbidExit) != 0;
              bool c = (flags1 & PathfindJobs.PathfindItemFlags.ReducedAccess) != 0;
              int4 int4 = new int4(this.m_AccessMask, math.select(local1.m_AccessRequirement, -1, c));
              PathfindJobs.FullNode pathNode = local1.m_PathNode;
              float baseCost = local1.m_BaseCost;
              PathMethod method = local1.m_Method;
              EdgeID edgeId1 = local1.m_EdgeID;
              for (int connectionIndex = 0; connectionIndex < num1; ++connectionIndex)
              {
                EdgeID edgeId2 = new EdgeID()
                {
                  m_Index = this.m_PathfindData.GetConnection(pathNode.m_NodeID, connectionIndex)
                };
                int num2 = this.m_PathfindData.GetAccessRequirement(pathNode.m_NodeID, connectionIndex);
                if (!edgeId1.Equals(edgeId2) && !math.all(int4 != num2))
                {
                  ref Edge local3 = ref this.m_PathfindData.GetEdge(edgeId2);
                  EdgeFlags flags2 = local3.m_Specification.m_Flags;
                  if (!this.DisallowConnection(method, flags1, in local3.m_Specification, ref flags2, local3.m_Owner))
                  {
                    bool flag2 = local3.m_Specification.m_AccessRequirement != local1.m_AccessRequirement;
                    if (!(flag1 & flag2))
                    {
                      PathfindJobs.PathfindItemFlags itemFlags = flags1;
                      if (flag2 && local3.m_Specification.m_AccessRequirement != -1)
                        itemFlags |= PathfindJobs.PathfindItemFlags.ForbidExit | PathfindJobs.PathfindItemFlags.ReducedAccess;
                      if ((flags2 & EdgeFlags.AllowExit) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
                        itemFlags &= ~PathfindJobs.PathfindItemFlags.ForbidExit;
                      if ((flags2 & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
                        itemFlags &= ~PathfindJobs.PathfindItemFlags.ReducedAccess;
                      this.CheckNextEdge(heapData.m_NodeIndex, edgeId2, pathNode, baseCost, itemFlags, in local3);
                    }
                  }
                }
              }
            }
          }
        }
        return false;
      }

      private void CheckNextEdge(
        int sourceIndex,
        EdgeID nextID,
        PathfindJobs.FullNode pathNode,
        float baseCost,
        PathfindJobs.PathfindItemFlags itemFlags,
        in Edge edge)
      {
        EdgeFlags flags = edge.m_Specification.m_Flags;
        RuleFlags rules = edge.m_Specification.m_Rules & (RuleFlags) ~((flags & EdgeFlags.Secondary) != 0 ? (int) this.m_Parameters.m_SecondaryIgnoredRules : (int) this.m_Parameters.m_IgnoredRules);
        float curvePos1 = math.select(edge.m_StartCurvePos, this.m_Parameters.m_ParkingDelta, edge.m_Owner == this.m_ParkingOwner);
        float curvePos2 = math.select(edge.m_EndCurvePos, this.m_Parameters.m_ParkingDelta, edge.m_Owner == this.m_ParkingOwner);
        float startDelta;
        bool3 directions;
        if (pathNode.Equals(new PathfindJobs.FullNode(edge.m_StartID, curvePos1)))
        {
          startDelta = 0.0f;
          directions = new bool3((flags & this.m_Forward) != 0, (flags & this.m_ForwardMiddle) == this.m_ForwardMiddle, false);
        }
        else if (pathNode.Equals(new PathfindJobs.FullNode(edge.m_EndID, curvePos2)))
        {
          startDelta = 1f;
          directions = new bool3(false, (flags & this.m_BackwardMiddle) == this.m_BackwardMiddle, (flags & this.m_Backward) != 0);
        }
        else
        {
          if (!pathNode.m_NodeID.Equals(edge.m_MiddleID))
            return;
          startDelta = pathNode.m_CurvePos;
          directions = new bool3((flags & this.m_Forward) != 0, (flags & EdgeFlags.AllowMiddle) != 0, (flags & this.m_Backward) != 0);
        }
        bool reducedCost = this.m_ParkingReset && ((itemFlags & PathfindJobs.PathfindItemFlags.ReducedCost) != (PathfindJobs.PathfindItemFlags) 0 || (edge.m_Specification.m_Methods & (PathMethod.Parking | PathMethod.SpecialParking)) != 0);
        bool forbidExit = (itemFlags & PathfindJobs.PathfindItemFlags.ForbidExit) != 0;
        bool reducedAccess = (itemFlags & PathfindJobs.PathfindItemFlags.ReducedAccess) != 0;
        this.AddConnections(sourceIndex, nextID, in edge, flags, rules, baseCost, startDelta, directions, reducedCost, forbidExit, reducedAccess);
      }

      private float CalculateCost(
        in PathSpecification pathSpecification,
        EdgeFlags flags,
        RuleFlags rules,
        float2 delta)
      {
        float speed = PathUtils.CalculateSpeed(in pathSpecification, in this.m_Parameters);
        float num = delta.y - delta.x;
        float y = math.select(0.0f, 1f, (rules & (RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic)) != 0);
        float4 x1 = pathSpecification.m_Costs.m_Value;
        x1.xy += pathSpecification.m_Length * new float2(1f / speed, y);
        x1.y += math.select(0.0f, 100f, (flags & EdgeFlags.RequireAuthorization) != 0 != math.any(pathSpecification.m_AccessRequirement == this.m_AuthorizationMask));
        bool2 x2 = new float2(num, 0.0f) >= new float2(0.0f, num);
        x2.x &= (flags & this.m_FreeForward) != 0;
        x2.y &= (flags & this.m_FreeBackward) != 0;
        x2.x |= (pathSpecification.m_Methods & this.m_Parameters.m_Methods) == PathMethod.Boarding;
        x1.xyz = math.select(x1.xyz, (float3) 0.0f, math.any(x2));
        return math.dot(x1, this.m_Parameters.m_Weights.m_Value) * math.abs(num);
      }

      private float CalculateTotalCost(
        in LocationSpecification location,
        float baseCost,
        float endDelta)
      {
        float3 float3 = MathUtils.Position(location.m_Line, endDelta);
        float3 x = math.max(this.m_EndBounds.min - float3, float3 - this.m_EndBounds.max);
        return baseCost + math.length(math.max(x, (float3) 0.0f)) * this.m_HeuristicCostFactor;
      }

      private void AddConnections(
        int sourceIndex,
        EdgeID id,
        in Edge edge,
        EdgeFlags flags,
        RuleFlags rules,
        float baseCost,
        float startDelta,
        bool3 directions,
        bool reducedCost,
        bool forbidExit,
        bool reducedAccess)
      {
        PathfindJobs.PathfindItemFlags itemFlags1 = (PathfindJobs.PathfindItemFlags) 0;
        float num1 = 1f;
        if ((flags & EdgeFlags.SingleOnly) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          itemFlags1 |= PathfindJobs.PathfindItemFlags.SingleOnly;
        if (reducedCost)
        {
          itemFlags1 |= PathfindJobs.PathfindItemFlags.ReducedCost;
          num1 = this.m_ReducedCostFactor;
        }
        if (forbidExit)
          itemFlags1 |= PathfindJobs.PathfindItemFlags.ForbidExit;
        if (reducedAccess)
          itemFlags1 |= PathfindJobs.PathfindItemFlags.ReducedAccess;
        float costFactor = num1 * math.select(this.m_Random.NextFloat(0.5f, 1f), 1f, (this.m_Parameters.m_PathfindFlags & PathfindFlags.Stable) != 0);
        if (directions.x)
        {
          float2 float2 = new float2(startDelta, 1f);
          if (this.IsValidDelta(in edge.m_Specification, rules, float2))
          {
            float curvePos = math.select(edge.m_EndCurvePos, this.m_Parameters.m_ParkingDelta, edge.m_Owner == this.m_ParkingOwner);
            PathfindJobs.FullNode pathNode = new PathfindJobs.FullNode(edge.m_EndID, curvePos);
            this.AddHeapData(sourceIndex, id, in edge, flags, rules, baseCost, costFactor, pathNode, float2, itemFlags1);
          }
        }
        if (directions.y)
        {
          int num2 = this.m_PathfindData.GetConnectionCount(edge.m_MiddleID);
          if (num2 != 0)
          {
            int4 int4 = new int4(this.m_AccessMask, math.select(edge.m_Specification.m_AccessRequirement, -1, reducedAccess));
            for (int connectionIndex = 0; connectionIndex < num2; ++connectionIndex)
            {
              EdgeID edgeId = new EdgeID()
              {
                m_Index = this.m_PathfindData.GetConnection(edge.m_MiddleID, connectionIndex)
              };
              int num3 = this.m_PathfindData.GetAccessRequirement(edge.m_MiddleID, connectionIndex);
              if (!id.Equals(edgeId) && !math.all(int4 != num3))
              {
                ref Edge local = ref this.m_PathfindData.GetEdge(edgeId);
                EdgeFlags flags1 = local.m_Specification.m_Flags;
                if (!this.DisallowConnection(edge.m_Specification.m_Methods, itemFlags1, in local.m_Specification, ref flags1, local.m_Owner))
                {
                  bool flag = local.m_Specification.m_AccessRequirement != edge.m_Specification.m_AccessRequirement;
                  if (!(forbidExit & flag))
                  {
                    PathfindJobs.PathfindItemFlags itemFlags2 = itemFlags1;
                    if (flag && local.m_Specification.m_AccessRequirement != -1)
                      itemFlags2 |= PathfindJobs.PathfindItemFlags.ForbidExit | PathfindJobs.PathfindItemFlags.ReducedAccess;
                    if ((flags1 & EdgeFlags.AllowExit) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
                      itemFlags2 &= ~PathfindJobs.PathfindItemFlags.ForbidExit;
                    if ((flags1 & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
                      itemFlags2 &= ~PathfindJobs.PathfindItemFlags.ReducedAccess;
                    NodeID middleId = edge.m_MiddleID;
                    if (middleId.Equals(local.m_StartID) & (flags1 & this.m_Forward) != 0)
                    {
                      float num4 = math.select(local.m_StartCurvePos, this.m_Parameters.m_ParkingDelta, local.m_Owner == this.m_ParkingOwner);
                      if (directions.x & (double) num4 >= (double) startDelta | directions.z & (double) num4 <= (double) startDelta)
                      {
                        float2 float2 = new float2(startDelta, num4);
                        if (this.IsValidDelta(in edge.m_Specification, rules, float2))
                        {
                          PathfindJobs.FullNode pathNode = new PathfindJobs.FullNode(local.m_StartID, num4);
                          this.AddHeapData(sourceIndex, id, edgeId, in edge, flags, rules, baseCost, costFactor, pathNode, float2, itemFlags2);
                        }
                      }
                    }
                    middleId = edge.m_MiddleID;
                    if (middleId.Equals(local.m_EndID) & (flags1 & this.m_Backward) != 0)
                    {
                      float num5 = math.select(local.m_EndCurvePos, this.m_Parameters.m_ParkingDelta, local.m_Owner == this.m_ParkingOwner);
                      if (directions.x & (double) num5 >= (double) startDelta | directions.z & (double) num5 <= (double) startDelta)
                      {
                        float2 float2 = new float2(startDelta, num5);
                        if (this.IsValidDelta(in edge.m_Specification, rules, float2))
                        {
                          PathfindJobs.FullNode pathNode = new PathfindJobs.FullNode(local.m_EndID, num5);
                          this.AddHeapData(sourceIndex, id, edgeId, in edge, flags, rules, baseCost, costFactor, pathNode, float2, itemFlags2);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        if (directions.z)
        {
          float2 float2 = new float2(startDelta, 0.0f);
          if (this.IsValidDelta(in edge.m_Specification, rules, float2))
          {
            float curvePos = math.select(edge.m_StartCurvePos, this.m_Parameters.m_ParkingDelta, edge.m_Owner == this.m_ParkingOwner);
            PathfindJobs.FullNode pathNode = new PathfindJobs.FullNode(edge.m_StartID, curvePos);
            this.AddHeapData(sourceIndex, id, in edge, flags, rules, baseCost, costFactor, pathNode, float2, itemFlags1);
          }
        }
        PathTarget pathTarget;
        NativeParallelMultiHashMapIterator<EdgeID> it;
        if (!this.m_Ends.TryGetFirstValue(id, out pathTarget, out it))
          return;
        do
        {
          bool2 xz = directions.xz;
          xz.x &= (pathTarget.m_Flags & this.m_Forward) != 0;
          xz.y &= (pathTarget.m_Flags & this.m_Backward) != 0;
          if (xz.x & (double) pathTarget.m_Delta >= (double) startDelta | xz.y & (double) pathTarget.m_Delta <= (double) startDelta)
          {
            float2 float2 = new float2(startDelta, pathTarget.m_Delta);
            if (this.IsValidDelta(in edge.m_Specification, rules, float2))
            {
              PathfindJobs.FullNode pathNode = new PathfindJobs.FullNode(id, pathTarget.m_Delta);
              float endCost = pathTarget.m_Cost * num1;
              this.AddEndData(sourceIndex, id, in edge, flags, rules, baseCost, endCost, costFactor, pathNode, float2, itemFlags1);
            }
          }
        }
        while (this.m_Ends.TryGetNextValue(out pathTarget, ref it));
      }

      private void AddHeapData(
        int sourceIndex,
        EdgeID id,
        in Edge edge,
        EdgeFlags flags,
        RuleFlags rules,
        float baseCost,
        float costFactor,
        PathfindJobs.FullNode pathNode,
        float2 edgeDelta,
        PathfindJobs.PathfindItemFlags itemFlags)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref PathfindJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          if (local.m_SourceIndex >= 0)
            return;
          float baseCost1 = baseCost + this.CalculateCost(in edge.m_Specification, flags, rules, edgeDelta) * costFactor;
          float totalCost = this.CalculateTotalCost(in edge.m_Location, baseCost1, edgeDelta.y);
          if ((double) totalCost >= (double) local.m_TotalCost)
            return;
          local = new PathfindJobs.NodeData(-1 - sourceIndex, totalCost, baseCost1, edge.m_Specification.m_AccessRequirement, id, new EdgeID(), edgeDelta, pathNode, itemFlags, edge.m_Specification.m_Methods);
          this.HeapInsert(new PathfindJobs.HeapData(totalCost, num));
        }
        else
        {
          int length = this.m_NodeData.Length;
          float baseCost2 = baseCost + this.CalculateCost(in edge.m_Specification, flags, rules, edgeDelta) * costFactor;
          float totalCost = this.CalculateTotalCost(in edge.m_Location, baseCost2, edgeDelta.y);
          this.m_NodeData.Add(new PathfindJobs.NodeData(-1 - sourceIndex, totalCost, baseCost2, edge.m_Specification.m_AccessRequirement, id, new EdgeID(), edgeDelta, pathNode, itemFlags, edge.m_Specification.m_Methods));
          this.HeapInsert(new PathfindJobs.HeapData(totalCost, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private void AddHeapData(
        int sourceIndex,
        EdgeID id,
        EdgeID id2,
        in Edge edge,
        EdgeFlags flags,
        RuleFlags rules,
        float baseCost,
        float costFactor,
        PathfindJobs.FullNode pathNode,
        float2 edgeDelta,
        PathfindJobs.PathfindItemFlags itemFlags)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref PathfindJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          if (local.m_SourceIndex >= 0)
            return;
          float baseCost1 = baseCost + this.CalculateCost(in edge.m_Specification, flags, rules, edgeDelta) * costFactor;
          float totalCost = this.CalculateTotalCost(in edge.m_Location, baseCost1, edgeDelta.y);
          if ((double) totalCost < (double) local.m_TotalCost)
          {
            local = new PathfindJobs.NodeData(-1 - sourceIndex, totalCost, baseCost1, edge.m_Specification.m_AccessRequirement, id, id2, edgeDelta, pathNode, itemFlags | PathfindJobs.PathfindItemFlags.NextEdge, edge.m_Specification.m_Methods);
            this.HeapInsert(new PathfindJobs.HeapData(totalCost, num));
          }
          else
          {
            if (id2.Equals(local.m_NextID))
              return;
            local.m_NextID = new EdgeID();
            local.m_Flags &= ~PathfindJobs.PathfindItemFlags.NextEdge;
          }
        }
        else
        {
          int length = this.m_NodeData.Length;
          float baseCost2 = baseCost + this.CalculateCost(in edge.m_Specification, flags, rules, edgeDelta) * costFactor;
          float totalCost = this.CalculateTotalCost(in edge.m_Location, baseCost2, edgeDelta.y);
          this.m_NodeData.Add(new PathfindJobs.NodeData(-1 - sourceIndex, totalCost, baseCost2, edge.m_Specification.m_AccessRequirement, id, id2, edgeDelta, pathNode, itemFlags | PathfindJobs.PathfindItemFlags.NextEdge, edge.m_Specification.m_Methods));
          this.HeapInsert(new PathfindJobs.HeapData(totalCost, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private void AddEndData(
        int sourceIndex,
        EdgeID id,
        in Edge edge,
        EdgeFlags flags,
        RuleFlags rules,
        float baseCost,
        float endCost,
        float costFactor,
        PathfindJobs.FullNode pathNode,
        float2 edgeDelta,
        PathfindJobs.PathfindItemFlags itemFlags)
      {
        int num;
        if (this.m_NodeIndex.TryGetValue(pathNode, out num))
        {
          ref PathfindJobs.NodeData local = ref this.m_NodeData.ElementAt(num);
          if (local.m_SourceIndex >= 0)
            return;
          float baseCost1 = baseCost + this.CalculateCost(in edge.m_Specification, flags, rules, edgeDelta) * costFactor;
          float totalCost = baseCost1 + endCost;
          if ((double) totalCost >= (double) local.m_TotalCost)
            return;
          local = new PathfindJobs.NodeData(-1 - sourceIndex, totalCost, baseCost1, edge.m_Specification.m_AccessRequirement, id, new EdgeID(), edgeDelta, pathNode, itemFlags | PathfindJobs.PathfindItemFlags.End, edge.m_Specification.m_Methods);
          this.HeapInsert(new PathfindJobs.HeapData(totalCost, num));
        }
        else
        {
          int length = this.m_NodeData.Length;
          float baseCost2 = baseCost + this.CalculateCost(in edge.m_Specification, flags, rules, edgeDelta) * costFactor;
          float totalCost = baseCost2 + endCost;
          this.m_NodeData.Add(new PathfindJobs.NodeData(-1 - sourceIndex, totalCost, baseCost2, edge.m_Specification.m_AccessRequirement, id, new EdgeID(), edgeDelta, pathNode, itemFlags | PathfindJobs.PathfindItemFlags.End, edge.m_Specification.m_Methods));
          this.HeapInsert(new PathfindJobs.HeapData(totalCost, length));
          this.m_NodeIndex.Add(pathNode, length);
        }
      }

      private bool IsValidDelta(in PathSpecification spec, RuleFlags rules, float2 delta)
      {
        return (rules & RuleFlags.HasBlockage) == (RuleFlags) 0 || (double) math.min(delta.x, delta.y) > (double) spec.m_BlockageEnd * 0.0039215688593685627 || (double) math.max(delta.x, delta.y) < (double) spec.m_BlockageStart * 0.0039215688593685627;
      }

      private bool DisallowConnection(
        PathMethod prevMethod,
        PathfindJobs.PathfindItemFlags itemFlags,
        in PathSpecification newSpec,
        ref EdgeFlags edgeFlags,
        Entity newOwner)
      {
        if ((newSpec.m_Methods & this.m_Parameters.m_Methods) == (PathMethod) 0 || (itemFlags & PathfindJobs.PathfindItemFlags.SingleOnly) != (PathfindJobs.PathfindItemFlags) 0 && (newSpec.m_Flags & EdgeFlags.SingleOnly) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          return true;
        if ((newSpec.m_Methods & (PathMethod.Parking | PathMethod.Boarding | PathMethod.SpecialParking)) == (PathMethod) 0)
          return false;
        if ((prevMethod & this.m_ParkingMethodMask) != (PathMethod) 0)
        {
          edgeFlags |= EdgeFlags.AllowExit;
          return this.m_Parameters.m_ParkingTarget != newOwner && (newSpec.m_Flags & this.m_ParkingEdgeMask) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary);
        }
        return (prevMethod & (PathMethod.Parking | PathMethod.Boarding | PathMethod.SpecialParking)) != (PathMethod) 0 || math.any(this.m_ParkingSize > new float2(newSpec.m_Density, newSpec.m_MaxSpeed));
      }

      public void CreatePath(
        int endNode,
        ref UnsafeList<PathfindPath> path,
        out float distance,
        out float duration,
        out int pathLength,
        out Entity origin,
        out Entity destination,
        out PathMethod methods)
      {
        distance = 0.0f;
        duration = 0.0f;
        pathLength = 0;
        methods = (PathMethod) 0;
        ref PathfindJobs.NodeData local1 = ref this.m_NodeData.ElementAt(endNode);
        PathfindJobs.FullNode key = new PathfindJobs.FullNode();
        PathfindJobs.TargetData targetData1;
        this.m_EndTargets.TryGetValue(local1.m_PathNode, out targetData1);
        destination = targetData1.m_Entity;
        PathfindParameters pathfindParameters = this.m_Parameters;
        while (true)
        {
          ref Edge local2 = ref this.m_PathfindData.GetEdge(local1.m_EdgeID);
          bool c = (local2.m_Specification.m_Flags & EdgeFlags.OutsideConnection) != 0;
          pathfindParameters.m_MaxSpeed = math.select(pathfindParameters.m_MaxSpeed, (float2) 277.777771f, c);
          pathfindParameters.m_WalkSpeed = math.select(pathfindParameters.m_WalkSpeed, (float2) 277.777771f, c);
          float length = PathUtils.CalculateLength(in local2.m_Specification, local1.m_EdgeDelta);
          float speed = PathUtils.CalculateSpeed(in local2.m_Specification, in pathfindParameters);
          distance += length;
          duration += length / speed;
          ++pathLength;
          methods |= local2.m_Specification.m_Methods & this.m_Parameters.m_Methods;
          if (path.IsCreated)
          {
            PathfindPath pathfindPath;
            pathfindPath.m_Target = local2.m_Owner;
            pathfindPath.m_TargetDelta = local1.m_EdgeDelta;
            pathfindPath.m_Flags = (PathElementFlags) 0;
            if ((local2.m_Specification.m_Flags & EdgeFlags.Secondary) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              pathfindPath.m_Flags |= PathElementFlags.Secondary;
            path.Add(in pathfindPath);
          }
          if (local1.m_SourceIndex != int.MaxValue)
            local1 = ref this.m_NodeData.ElementAt(local1.m_SourceIndex);
          else
            break;
        }
        key = new PathfindJobs.FullNode(local1.m_EdgeID, local1.m_EdgeDelta.x);
        PathfindJobs.TargetData targetData2;
        this.m_StartTargets.TryGetValue(key, out targetData2);
        origin = targetData2.m_Entity;
        if (this.m_InvertPath)
          CommonUtils.Swap<Entity>(ref origin, ref destination);
        if (!path.IsCreated || path.Length <= 0)
          return;
        if (this.m_InvertPath)
        {
          for (int index = 0; index < path.Length; ++index)
          {
            ref PathfindPath local3 = ref path.ElementAt(index);
            local3.m_TargetDelta = local3.m_TargetDelta.yx;
          }
        }
        else
        {
          int num1 = 0;
          int num2 = path.Length - 1;
          while (num1 < num2)
            CommonUtils.Swap<PathfindPath>(ref path.ElementAt(num1++), ref path.ElementAt(num2--));
        }
        path.ElementAt(0).m_Flags |= PathElementFlags.PathStart;
      }
    }

    [BurstCompile]
    public struct PathfindJob : IJob
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativePathfindData m_PathfindData;
      [ReadOnly]
      public PathfindHeuristicData m_PathfindHeuristicData;
      [ReadOnly]
      public float m_MaxPassengerTransportSpeed;
      [ReadOnly]
      public float m_MaxCargoTransportSpeed;
      public PathfindAction m_Action;

      public void Execute()
      {
        PathfindJobs.PathfindJob.Execute(this.m_PathfindData, Allocator.Temp, this.m_RandomSeed.GetRandom(0), this.m_PathfindHeuristicData, this.m_MaxPassengerTransportSpeed, this.m_MaxCargoTransportSpeed, ref this.m_Action.data);
      }

      public static void Execute(
        NativePathfindData pathfindData,
        Allocator allocator,
        Unity.Mathematics.Random random,
        PathfindHeuristicData pathfindHeuristicData,
        float maxPassengerTransportSpeed,
        float maxCargoTransportSpeed,
        ref PathfindActionData actionData)
      {
        PathfindResult pathfindResult = new PathfindResult()
        {
          m_Distance = -1f,
          m_Duration = -1f,
          m_TotalCost = -1f
        };
        if (actionData.m_StartTargets.Length == 0 || actionData.m_EndTargets.Length == 0)
        {
          actionData.m_Result.Add(in pathfindResult);
        }
        else
        {
          UnsafeList<PathfindPath> unsafeList = new UnsafeList<PathfindPath>();
          ref UnsafeList<PathfindPath> local1 = ref unsafeList;
          ref UnsafeList<PathfindPath> local2 = ref actionData.m_Path;
          PathfindParameters parameters = actionData.m_Parameters;
          actionData.m_Result.Capacity = math.max(1, parameters.m_MaxResultCount);
          if ((parameters.m_PathfindFlags & PathfindFlags.SkipPathfind) != (PathfindFlags) 0)
          {
            pathfindResult.m_Distance = 0.0f;
            pathfindResult.m_Duration = 0.0f;
            pathfindResult.m_TotalCost = actionData.m_StartTargets[0].m_Cost + actionData.m_EndTargets[0].m_Cost;
            pathfindResult.m_GraphTraversal = 1;
            pathfindResult.m_PathLength = 1;
            pathfindResult.m_Origin = actionData.m_StartTargets[0].m_Entity;
            pathfindResult.m_Destination = actionData.m_EndTargets[0].m_Entity;
          }
          else
          {
            PathfindJobs.PathfindExecutor pathfindExecutor = new PathfindJobs.PathfindExecutor();
            pathfindExecutor.Initialize(pathfindData, allocator, random, parameters, pathfindHeuristicData, maxPassengerTransportSpeed, maxCargoTransportSpeed);
            pathfindExecutor.AddTargets(actionData.m_StartTargets, actionData.m_EndTargets, ref pathfindResult.m_ErrorCode);
            int endNode;
            while (pathfindExecutor.FindEndNode(out endNode, out pathfindResult.m_TotalCost, out pathfindResult.m_GraphTraversal))
            {
              pathfindExecutor.CreatePath(endNode, ref local2, out pathfindResult.m_Distance, out pathfindResult.m_Duration, out pathfindResult.m_PathLength, out pathfindResult.m_Origin, out pathfindResult.m_Destination, out pathfindResult.m_Methods);
              actionData.m_Result.Add(in pathfindResult);
              pathfindResult = new PathfindResult()
              {
                m_Distance = -1f,
                m_Duration = -1f,
                m_TotalCost = -1f
              };
              local2 = ref unsafeList;
            }
            pathfindExecutor.Release();
          }
          if (actionData.m_Result.Length != 0)
            return;
          actionData.m_Result.Add(in pathfindResult);
        }
      }
    }

    public struct ResultItem
    {
      public Entity m_Owner;
      public UnsafeList<PathfindResult> m_Result;
      public UnsafeList<PathfindPath> m_Path;
    }

    [BurstCompile]
    public struct ProcessResultsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeList<PathfindJobs.ResultItem> m_ResultItems;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PathOwner> m_PathOwner;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PathInformation> m_PathInformation;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathInformations> m_PathInformations;

      public void Execute(int index)
      {
        PathfindJobs.ResultItem resultItem = this.m_ResultItems[index];
        bool flag = false;
        PathOwner componentData1;
        if (this.m_PathOwner.TryGetComponent(resultItem.m_Owner, out componentData1))
        {
          if (resultItem.m_Path.Length == 0 && (componentData1.m_State & PathFlags.Divert) != (PathFlags) 0)
          {
            flag = true;
            componentData1.m_State |= PathFlags.Failed;
          }
          else
          {
            DynamicBuffer<PathElement> bufferData;
            if (this.m_PathElements.TryGetBuffer(resultItem.m_Owner, out bufferData))
            {
              if ((componentData1.m_State & PathFlags.Append) != (PathFlags) 0)
              {
                if (componentData1.m_ElementIndex != 0)
                {
                  bufferData.RemoveRange(0, componentData1.m_ElementIndex);
                  componentData1.m_ElementIndex = 0;
                }
              }
              else
              {
                bufferData.Clear();
                componentData1.m_ElementIndex = 0;
              }
              if ((componentData1.m_State & PathFlags.Obsolete) == (PathFlags) 0)
              {
                for (int index1 = 0; index1 < resultItem.m_Path.Length; ++index1)
                {
                  PathfindPath pathfindPath = resultItem.m_Path[index1];
                  bufferData.Add(new PathElement()
                  {
                    m_Target = pathfindPath.m_Target,
                    m_TargetDelta = pathfindPath.m_TargetDelta,
                    m_Flags = pathfindPath.m_Flags
                  });
                }
                if ((componentData1.m_State & PathFlags.AddDestination) != (PathFlags) 0)
                {
                  PathfindResult pathfindResult = resultItem.m_Result[0];
                  bufferData.Add(new PathElement()
                  {
                    m_Target = pathfindResult.m_Destination
                  });
                }
              }
            }
            if ((componentData1.m_State & PathFlags.Obsolete) != (PathFlags) 0)
              flag = true;
            else if (resultItem.m_Path.Length == 0)
              componentData1.m_State |= PathFlags.Failed;
            if ((componentData1.m_State & PathFlags.Divert) != (PathFlags) 0)
              componentData1.m_State |= PathFlags.CachedObsolete;
            else
              componentData1.m_State &= ~PathFlags.CachedObsolete;
            componentData1.m_State |= PathFlags.Updated;
          }
          componentData1.m_State &= ~PathFlags.Pending;
          this.m_PathOwner[resultItem.m_Owner] = componentData1;
        }
        else
        {
          DynamicBuffer<PathElement> bufferData;
          if (this.m_PathElements.TryGetBuffer(resultItem.m_Owner, out bufferData))
          {
            bufferData.Clear();
            for (int index2 = 0; index2 < resultItem.m_Path.Length; ++index2)
            {
              PathfindPath pathfindPath = resultItem.m_Path[index2];
              bufferData.Add(new PathElement()
              {
                m_Target = pathfindPath.m_Target,
                m_TargetDelta = pathfindPath.m_TargetDelta,
                m_Flags = pathfindPath.m_Flags
              });
            }
          }
        }
        if (flag)
          return;
        PathInformation componentData2;
        if (this.m_PathInformation.TryGetComponent(resultItem.m_Owner, out componentData2))
        {
          PathfindResult pathfindResult = resultItem.m_Result[0];
          componentData2.m_Origin = pathfindResult.m_Origin;
          componentData2.m_Destination = pathfindResult.m_Destination;
          componentData2.m_Distance = pathfindResult.m_Distance;
          componentData2.m_Duration = pathfindResult.m_Duration;
          componentData2.m_TotalCost = pathfindResult.m_TotalCost;
          componentData2.m_Methods = pathfindResult.m_Methods;
          componentData2.m_State &= ~PathFlags.Pending;
          this.m_PathInformation[resultItem.m_Owner] = componentData2;
        }
        DynamicBuffer<PathInformations> bufferData1;
        if (!this.m_PathInformations.TryGetBuffer(resultItem.m_Owner, out bufferData1))
          return;
        CollectionUtils.ResizeInitialized<PathInformations>(bufferData1, resultItem.m_Result.Length, bufferData1[0]);
        for (int index3 = 0; index3 < resultItem.m_Result.Length; ++index3)
        {
          PathfindResult pathfindResult = resultItem.m_Result[index3];
          PathInformations pathInformations = bufferData1[index3] with
          {
            m_Origin = pathfindResult.m_Origin,
            m_Destination = pathfindResult.m_Destination,
            m_Distance = pathfindResult.m_Distance,
            m_Duration = pathfindResult.m_Duration,
            m_TotalCost = pathfindResult.m_TotalCost,
            m_Methods = pathfindResult.m_Methods
          };
          pathInformations.m_State &= ~PathFlags.Pending;
          bufferData1[index3] = pathInformations;
        }
      }
    }
  }
}
