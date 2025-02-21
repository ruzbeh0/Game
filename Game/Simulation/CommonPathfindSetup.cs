// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CommonPathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Events;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct CommonPathfindSetup
  {
    private Game.Net.SearchSystem m_NetSearchSystem;
    private ComponentLookup<PathOwner> m_PathOwnerData;
    private ComponentLookup<Vehicle> m_VehicleData;
    private ComponentLookup<Composition> m_CompositionData;
    private ComponentLookup<NetCompositionData> m_NetCompositionData;
    private ComponentLookup<Creature> m_CreatureData;
    private ComponentLookup<AccidentSite> m_AccidentSiteData;
    private BufferLookup<PathElement> m_PathElements;
    private BufferLookup<Game.Areas.SubArea> m_SubAreas;
    private BufferLookup<TargetElement> m_TargetElements;

    public CommonPathfindSetup(PathfindSetupSystem system)
    {
      this.m_NetSearchSystem = system.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      this.m_PathOwnerData = system.GetComponentLookup<PathOwner>(true);
      this.m_VehicleData = system.GetComponentLookup<Vehicle>(true);
      this.m_CompositionData = system.GetComponentLookup<Composition>(true);
      this.m_NetCompositionData = system.GetComponentLookup<NetCompositionData>(true);
      this.m_CreatureData = system.GetComponentLookup<Creature>(true);
      this.m_AccidentSiteData = system.GetComponentLookup<AccidentSite>(true);
      this.m_PathElements = system.GetBufferLookup<PathElement>(true);
      this.m_SubAreas = system.GetBufferLookup<Game.Areas.SubArea>(true);
      this.m_TargetElements = system.GetBufferLookup<TargetElement>(true);
    }

    public JobHandle SetupCurrentLocation(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_PathOwnerData.Update((SystemBase) system);
      this.m_VehicleData.Update((SystemBase) system);
      this.m_CompositionData.Update((SystemBase) system);
      this.m_NetCompositionData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_SubAreas.Update((SystemBase) system);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new CommonPathfindSetup.SetupCurrentLocationJob()
      {
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_PathOwnerData = this.m_PathOwnerData,
        m_VehicleData = this.m_VehicleData,
        m_CompositionData = this.m_CompositionData,
        m_NetCompositionData = this.m_NetCompositionData,
        m_PathElements = this.m_PathElements,
        m_SubAreas = this.m_SubAreas,
        m_SetupData = setupData
      }.Schedule<CommonPathfindSetup.SetupCurrentLocationJob>(setupData.Length, 1, JobHandle.CombineDependencies(inputDeps, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      return jobHandle;
    }

    public JobHandle SetupAccidentLocation(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_CreatureData.Update((SystemBase) system);
      this.m_VehicleData.Update((SystemBase) system);
      this.m_AccidentSiteData.Update((SystemBase) system);
      this.m_CompositionData.Update((SystemBase) system);
      this.m_NetCompositionData.Update((SystemBase) system);
      this.m_TargetElements.Update((SystemBase) system);
      this.m_SubAreas.Update((SystemBase) system);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new CommonPathfindSetup.SetupAccidentLocationJob()
      {
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_CreatureData = this.m_CreatureData,
        m_VehicleData = this.m_VehicleData,
        m_AccidentSiteData = this.m_AccidentSiteData,
        m_CompositionData = this.m_CompositionData,
        m_NetCompositionData = this.m_NetCompositionData,
        m_TargetElements = this.m_TargetElements,
        m_SubAreas = this.m_SubAreas,
        m_SetupData = setupData
      }.Schedule<CommonPathfindSetup.SetupAccidentLocationJob>(setupData.Length, 1, JobHandle.CombineDependencies(inputDeps, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      return jobHandle;
    }

    public JobHandle SetupSafety(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_CompositionData.Update((SystemBase) system);
      this.m_NetCompositionData.Update((SystemBase) system);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new CommonPathfindSetup.SetupSafetyJob()
      {
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_CompositionData = this.m_CompositionData,
        m_NetCompositionData = this.m_NetCompositionData,
        m_SetupData = setupData
      }.Schedule<CommonPathfindSetup.SetupSafetyJob>(setupData.Length, 1, JobHandle.CombineDependencies(inputDeps, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      return jobHandle;
    }

    [BurstCompile]
    private struct SetupCurrentLocationJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(int index)
      {
        Entity entity1;
        PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
        // ISSUE: reference to a compiler-generated method
        this.m_SetupData.GetItem(index, out entity1, out targetSeeker);
        float y = targetSeeker.m_SetupQueueTarget.m_Value2;
        if ((targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0 && this.m_VehicleData.HasComponent(entity1))
          y = math.max(10f, y);
        EdgeFlags flags = EdgeFlags.DefaultMask;
        if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.SecondaryPath) != SetupTargetFlags.None)
          flags |= EdgeFlags.Secondary;
        PathElement pathElement = new PathElement();
        bool navigationEnd = false;
        bool flag = false;
        if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.PathEnd) != SetupTargetFlags.None)
        {
          navigationEnd = true;
          PathOwner componentData;
          DynamicBuffer<PathElement> bufferData;
          if (this.m_PathOwnerData.TryGetComponent(entity1, out componentData) && this.m_PathElements.TryGetBuffer(entity1, out bufferData) && componentData.m_ElementIndex < bufferData.Length)
          {
            pathElement = bufferData[bufferData.Length - 1];
            flag = true;
          }
        }
        if (flag)
        {
          targetSeeker.m_Buffer.Enqueue(new PathTarget(entity1, pathElement.m_Target, pathElement.m_TargetDelta.y, 0.0f));
        }
        else
        {
          if (targetSeeker.FindTargets(entity1, entity1, 0.0f, flags, true, navigationEnd) != 0 && navigationEnd || (targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) == (PathMethod) 0 && (double) y <= 0.0)
            return;
          Entity entity2 = entity1;
          if (targetSeeker.m_CurrentTransport.HasComponent(entity2))
            entity2 = targetSeeker.m_CurrentTransport[entity2].m_CurrentTransport;
          else if (targetSeeker.m_CurrentBuilding.HasComponent(entity2))
            entity2 = targetSeeker.m_CurrentBuilding[entity2].m_CurrentBuilding;
          if (!targetSeeker.m_Transform.HasComponent(entity2))
            return;
          float3 position = targetSeeker.m_Transform[entity2].m_Position;
          if ((targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) != (PathMethod) 0)
          {
            if ((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) != RoadTypes.None)
            {
              Entity lane = Entity.Null;
              float curvePos = 0.0f;
              float maxValue = float.MaxValue;
              targetSeeker.m_AirwayData.helicopterMap.FindClosestLane(position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
              if (lane != Entity.Null)
                targetSeeker.m_Buffer.Enqueue(new PathTarget(entity1, lane, curvePos, 0.0f));
            }
            if ((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Airplane) != RoadTypes.None)
            {
              Entity lane = Entity.Null;
              float curvePos = 0.0f;
              float maxValue = float.MaxValue;
              targetSeeker.m_AirwayData.airplaneMap.FindClosestLane(position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
              if (lane != Entity.Null)
                targetSeeker.m_Buffer.Enqueue(new PathTarget(entity1, lane, curvePos, 0.0f));
            }
          }
          if ((double) y <= 0.0)
            return;
          CommonPathfindSetup.TargetIterator iterator = new CommonPathfindSetup.TargetIterator()
          {
            m_Entity = entity1,
            m_Bounds = new Bounds3(position - y, position + y),
            m_Position = position,
            m_MaxDistance = y,
            m_TargetSeeker = targetSeeker,
            m_Flags = flags,
            m_CompositionData = this.m_CompositionData,
            m_NetCompositionData = this.m_NetCompositionData
          };
          this.m_NetSearchTree.Iterate<CommonPathfindSetup.TargetIterator>(ref iterator);
          Entity entity3 = entity2;
          Owner componentData;
          while (targetSeeker.m_Owner.TryGetComponent(entity3, out componentData) && !targetSeeker.m_AreaNode.HasBuffer(entity3))
            entity3 = componentData.m_Owner;
          if (!targetSeeker.m_AreaNode.HasBuffer(entity3))
            return;
          Random random = targetSeeker.m_RandomSeed.GetRandom(entity3.Index);
          DynamicBuffer<Game.Areas.SubArea> bufferData;
          this.m_SubAreas.TryGetBuffer(entity3, out bufferData);
          targetSeeker.AddAreaTargets(ref random, entity1, entity3, entity2, bufferData, 0.0f, true, EdgeFlags.DefaultMask);
        }
      }
    }

    [BurstCompile]
    private struct SetupAccidentLocationJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(int index)
      {
        Entity entity1;
        PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
        // ISSUE: reference to a compiler-generated method
        this.m_SetupData.GetItem(index, out entity1, out targetSeeker);
        if (!this.m_AccidentSiteData.HasComponent(entity1))
          return;
        AccidentSite accidentSite = this.m_AccidentSiteData[entity1];
        if (!this.m_TargetElements.HasBuffer(accidentSite.m_Event))
          return;
        DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[accidentSite.m_Event];
        EdgeFlags edgeFlags = EdgeFlags.DefaultMask;
        if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.SecondaryPath) != SetupTargetFlags.None)
          edgeFlags |= EdgeFlags.Secondary;
        bool allowAccessRestriction = true;
        this.CheckTarget(entity1, accidentSite, edgeFlags, ref targetSeeker, ref allowAccessRestriction);
        for (int index1 = 0; index1 < targetElement.Length; ++index1)
        {
          Entity entity2 = targetElement[index1].m_Entity;
          if (entity2 != entity1)
            this.CheckTarget(entity2, accidentSite, edgeFlags, ref targetSeeker, ref allowAccessRestriction);
        }
      }

      private void CheckTarget(
        Entity target,
        AccidentSite accidentSite,
        EdgeFlags edgeFlags,
        ref PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker,
        ref bool allowAccessRestriction)
      {
        if ((accidentSite.m_Flags & AccidentSiteFlags.TrafficAccident) != (AccidentSiteFlags) 0 && !this.m_CreatureData.HasComponent(target) && !this.m_VehicleData.HasComponent(target))
          return;
        int targets = targetSeeker.FindTargets(target, target, 0.0f, edgeFlags, allowAccessRestriction, false);
        allowAccessRestriction &= targets == 0;
        Entity entity1 = target;
        if (targetSeeker.m_CurrentTransport.HasComponent(entity1))
          entity1 = targetSeeker.m_CurrentTransport[entity1].m_CurrentTransport;
        else if (targetSeeker.m_CurrentBuilding.HasComponent(entity1))
          entity1 = targetSeeker.m_CurrentBuilding[entity1].m_CurrentBuilding;
        if (!targetSeeker.m_Transform.HasComponent(entity1))
          return;
        float3 position = targetSeeker.m_Transform[entity1].m_Position;
        if ((targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) != (PathMethod) 0)
        {
          if ((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) != RoadTypes.None)
          {
            Entity lane = Entity.Null;
            float curvePos = 0.0f;
            float maxValue = float.MaxValue;
            targetSeeker.m_AirwayData.helicopterMap.FindClosestLane(position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
            if (lane != Entity.Null)
              targetSeeker.m_Buffer.Enqueue(new PathTarget(target, lane, curvePos, 0.0f));
          }
          if ((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Airplane) != RoadTypes.None)
          {
            Entity lane = Entity.Null;
            float curvePos = 0.0f;
            float maxValue = float.MaxValue;
            targetSeeker.m_AirwayData.airplaneMap.FindClosestLane(position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
            if (lane != Entity.Null)
              targetSeeker.m_Buffer.Enqueue(new PathTarget(target, lane, curvePos, 0.0f));
          }
        }
        float num = targetSeeker.m_SetupQueueTarget.m_Value2;
        CommonPathfindSetup.TargetIterator iterator = new CommonPathfindSetup.TargetIterator()
        {
          m_Entity = target,
          m_Bounds = new Bounds3(position - num, position + num),
          m_Position = position,
          m_MaxDistance = num,
          m_TargetSeeker = targetSeeker,
          m_Flags = edgeFlags,
          m_CompositionData = this.m_CompositionData,
          m_NetCompositionData = this.m_NetCompositionData
        };
        this.m_NetSearchTree.Iterate<CommonPathfindSetup.TargetIterator>(ref iterator);
        Entity entity2 = entity1;
        Owner componentData;
        while (targetSeeker.m_Owner.TryGetComponent(entity2, out componentData) && !targetSeeker.m_AreaNode.HasBuffer(entity2))
          entity2 = componentData.m_Owner;
        if (!targetSeeker.m_AreaNode.HasBuffer(entity2))
          return;
        Random random = targetSeeker.m_RandomSeed.GetRandom(entity2.Index);
        DynamicBuffer<Game.Areas.SubArea> bufferData;
        this.m_SubAreas.TryGetBuffer(entity2, out bufferData);
        targetSeeker.AddAreaTargets(ref random, target, entity2, entity1, bufferData, 0.0f, true, EdgeFlags.DefaultMask);
      }
    }

    [BurstCompile]
    private struct SetupSafetyJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(int index)
      {
        Entity entity1;
        PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
        // ISSUE: reference to a compiler-generated method
        this.m_SetupData.GetItem(index, out entity1, out targetSeeker);
        Entity entity2 = entity1;
        if (targetSeeker.m_CurrentTransport.HasComponent(entity2))
          entity2 = targetSeeker.m_CurrentTransport[entity2].m_CurrentTransport;
        else if (targetSeeker.m_CurrentBuilding.HasComponent(entity2))
          entity2 = targetSeeker.m_CurrentBuilding[entity2].m_CurrentBuilding;
        if (!targetSeeker.m_Transform.HasComponent(entity2))
          return;
        float3 position = targetSeeker.m_Transform[entity2].m_Position;
        if (targetSeeker.m_Building.HasComponent(entity2))
        {
          Building building = targetSeeker.m_Building[entity2];
          if (targetSeeker.m_SubLane.HasBuffer(building.m_RoadEdge))
          {
            Random random = targetSeeker.m_RandomSeed.GetRandom(building.m_RoadEdge.Index);
            targetSeeker.AddEdgeTargets(ref random, entity1, 0.0f, EdgeFlags.DefaultMask, building.m_RoadEdge, position, 0.0f, true, true);
          }
        }
        float num = 100f;
        CommonPathfindSetup.TargetIterator iterator = new CommonPathfindSetup.TargetIterator()
        {
          m_Entity = entity2,
          m_Bounds = new Bounds3(position - num, position + num),
          m_Position = position,
          m_MaxDistance = num,
          m_TargetSeeker = targetSeeker,
          m_Flags = EdgeFlags.DefaultMask,
          m_CompositionData = this.m_CompositionData,
          m_NetCompositionData = this.m_NetCompositionData
        };
        this.m_NetSearchTree.Iterate<CommonPathfindSetup.TargetIterator>(ref iterator);
      }
    }

    public struct TargetIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_Entity;
      public Bounds3 m_Bounds;
      public float3 m_Position;
      public float m_MaxDistance;
      public PathfindTargetSeeker<PathfindSetupBuffer> m_TargetSeeker;
      public EdgeFlags m_Flags;
      public ComponentLookup<Composition> m_CompositionData;
      public ComponentLookup<NetCompositionData> m_NetCompositionData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_CompositionData.HasComponent(edgeEntity))
          return;
        NetCompositionData netCompositionData = this.m_NetCompositionData[this.m_CompositionData[edgeEntity].m_Edge];
        bool flag = false;
        if ((this.m_TargetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Road) != (PathMethod) 0)
          flag |= (netCompositionData.m_State & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes)) != 0;
        if ((this.m_TargetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Pedestrian) != (PathMethod) 0)
          flag |= (netCompositionData.m_State & CompositionState.HasPedestrianLanes) != 0;
        if (!flag)
          return;
        float cost = MathUtils.Distance(this.m_TargetSeeker.m_Curve[edgeEntity].m_Bezier, this.m_Position, out float _) - netCompositionData.m_Width * 0.5f;
        if ((double) cost >= (double) this.m_MaxDistance)
          return;
        Random random = this.m_TargetSeeker.m_RandomSeed.GetRandom(edgeEntity.Index);
        this.m_TargetSeeker.AddEdgeTargets(ref random, this.m_Entity, cost, this.m_Flags, edgeEntity, this.m_Position, this.m_MaxDistance, true, false);
      }
    }
  }
}
