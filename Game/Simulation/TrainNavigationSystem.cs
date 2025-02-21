// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrainNavigationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TrainNavigationSystem : GameSystemBase
  {
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EntityQuery m_VehicleQuery;
    private LaneObjectUpdater m_LaneObjectUpdater;
    private TrainNavigationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 3;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Train>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<Moving>(), ComponentType.ReadOnly<Game.Common.Target>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<PathElement>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<TrainCurrentLane>(), ComponentType.ReadWrite<TrainNavigation>(), ComponentType.ReadWrite<TrainNavigationLane>(), ComponentType.ReadWrite<LayoutElement>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneObjectUpdater = new LaneObjectUpdater((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<TrainNavigationHelpers.LaneEffects> nativeQueue1 = new NativeQueue<TrainNavigationHelpers.LaneEffects>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<TrainNavigationHelpers.LaneSignal> nativeQueue2 = new NativeQueue<TrainNavigationHelpers.LaneSignal>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_VehicleQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrainNavigationSystem.UpdateNavigationJob jobData1 = new TrainNavigationSystem.UpdateNavigationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_NavigationData = this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup,
        m_CurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSideEffectData = this.__TypeHandle.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_LaneObjectBuffer = this.m_LaneObjectUpdater.Begin(Allocator.TempJob),
        m_LaneEffects = nativeQueue1.AsParallelWriter(),
        m_LaneSignals = nativeQueue2.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrainNavigationSystem.UpdateLaneReservationsJob jobData2 = new TrainNavigationSystem.UpdateLaneReservationsJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_CurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RW_ComponentLookup,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneFlow_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Pollution_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrainNavigationSystem.ApplyLaneEffectsJob jobData3 = new TrainNavigationSystem.ApplyLaneEffectsJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_LaneDeteriorationData = this.__TypeHandle.__Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup,
        m_PollutionData = this.__TypeHandle.__Game_Net_Pollution_RW_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentLookup,
        m_LaneFlowData = this.__TypeHandle.__Game_Net_LaneFlow_RW_ComponentLookup,
        m_LaneEffectsQueue = nativeQueue1
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrainNavigationSystem.UpdateLaneSignalsJob jobData4 = new TrainNavigationSystem.UpdateLaneSignalsJob()
      {
        m_LaneSignalQueue = nativeQueue2,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<TrainNavigationSystem.UpdateNavigationJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      JobHandle jobHandle2 = jobData2.Schedule<TrainNavigationSystem.UpdateLaneReservationsJob>(JobHandle.CombineDependencies(jobHandle1, outJobHandle));
      JobHandle jobHandle3 = jobData3.Schedule<TrainNavigationSystem.ApplyLaneEffectsJob>(jobHandle1);
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle4 = jobData4.Schedule<TrainNavigationSystem.UpdateLaneSignalsJob>(dependsOn);
      nativeQueue1.Dispose(jobHandle3);
      nativeQueue2.Dispose(jobHandle4);
      archetypeChunkListAsync.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = JobUtils.CombineDependencies(this.m_LaneObjectUpdater.Apply((SystemBase) this, jobHandle1), jobHandle2, jobHandle4, jobHandle3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public TrainNavigationSystem()
    {
    }

    [BurstCompile]
    private struct UpdateNavigationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Blocker> m_BlockerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<TrainNavigationLane> m_NavigationLaneType;
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainNavigation> m_NavigationData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainCurrentLane> m_CurrentLaneData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<VehicleSideEffectData> m_PrefabSideEffectData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;
      public NativeQueue<TrainNavigationHelpers.LaneEffects>.ParallelWriter m_LaneEffects;
      public NativeQueue<TrainNavigationHelpers.LaneSignal>.ParallelWriter m_LaneSignals;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray2 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray3 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Odometer> nativeArray4 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray5 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TrainNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<TrainNavigationLane>(ref this.m_NavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor2 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor3 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        NativeList<TrainNavigationHelpers.CurrentLaneCache> nativeList = new NativeList<TrainNavigationHelpers.CurrentLaneCache>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        bool flag = nativeArray4.Length != 0;
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity controller = nativeArray1[index1];
          Game.Common.Target target = nativeArray2[index1];
          Blocker blocker = nativeArray3[index1];
          PathOwner pathOwner = nativeArray5[index1];
          DynamicBuffer<TrainNavigationLane> navigationLanes = bufferAccessor1[index1];
          DynamicBuffer<LayoutElement> layout = bufferAccessor2[index1];
          DynamicBuffer<PathElement> pathElements = bufferAccessor3[index1];
          if (layout.Length != 0)
          {
            TrainNavigationHelpers.CurrentLaneCache currentLaneCache;
            for (int index2 = 0; index2 < layout.Length; ++index2)
            {
              Entity vehicle = layout[index2].m_Vehicle;
              // ISSUE: reference to a compiler-generated field
              TrainCurrentLane currentLane = this.m_CurrentLaneData[vehicle];
              ref NativeList<TrainNavigationHelpers.CurrentLaneCache> local1 = ref nativeList;
              // ISSUE: reference to a compiler-generated field
              currentLaneCache = new TrainNavigationHelpers.CurrentLaneCache(ref currentLane, this.m_LaneData);
              ref TrainNavigationHelpers.CurrentLaneCache local2 = ref currentLaneCache;
              local1.Add(in local2);
              // ISSUE: reference to a compiler-generated field
              this.m_CurrentLaneData[vehicle] = currentLane;
            }
            Entity vehicle1 = layout[0].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = this.m_TransformData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            Moving moving = this.m_MovingData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            Train train = this.m_TrainData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            TrainNavigation trainNavigation = this.m_NavigationData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane trainCurrentLane = this.m_CurrentLaneData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[vehicle1];
            // ISSUE: reference to a compiler-generated field
            TrainData prefabTrainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData prefabObjectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            int priority = VehicleUtils.GetPriority(prefabTrainData);
            Odometer odometer = new Odometer();
            if (flag)
              odometer = nativeArray4[index1];
            // ISSUE: reference to a compiler-generated method
            this.UpdateTrainLimits(ref prefabTrainData, layout);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationLanes(transform, train, target, prefabTrainData, ref trainCurrentLane, ref blocker, ref pathOwner, navigationLanes, layout, pathElements);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationTarget(priority, controller, transform, moving, train, target, prefabRef, prefabTrainData, prefabObjectGeometryData, ref trainNavigation, ref trainCurrentLane, ref blocker, ref odometer, navigationLanes, layout);
            // ISSUE: reference to a compiler-generated method
            this.TryReserveNavigationLanes(train, prefabTrainData, ref trainNavigation, ref trainCurrentLane, navigationLanes);
            // ISSUE: reference to a compiler-generated field
            this.m_NavigationData[vehicle1] = trainNavigation;
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentLaneData[vehicle1] = trainCurrentLane;
            for (int index3 = 0; index3 < layout.Length; ++index3)
            {
              Entity vehicle2 = layout[index3].m_Vehicle;
              // ISSUE: reference to a compiler-generated field
              TrainCurrentLane currentLane = this.m_CurrentLaneData[vehicle2];
              currentLaneCache = nativeList[index3];
              // ISSUE: reference to a compiler-generated field
              currentLaneCache.CheckChanges(vehicle2, currentLane, this.m_LaneObjectBuffer);
            }
            nativeArray5[index1] = pathOwner;
            nativeArray3[index1] = blocker;
            nativeList.Clear();
            if (flag)
              nativeArray4[index1] = odometer;
          }
        }
        nativeList.Dispose();
      }

      private void UpdateNavigationLanes(
        Game.Objects.Transform transform,
        Train train,
        Game.Common.Target target,
        TrainData prefabTrainData,
        ref TrainCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<PathElement> pathElements)
      {
        int invalidPath = 0;
        // ISSUE: reference to a compiler-generated method
        if (!this.HasValidLanes(currentLane))
        {
          ++invalidPath;
          // ISSUE: reference to a compiler-generated method
          this.TryFindCurrentLane(ref currentLane, transform, train, prefabTrainData);
        }
        else if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0 && (pathOwner.m_State & PathFlags.Append) == (PathFlags) 0)
        {
          navigationLanes.Clear();
          currentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.Return;
        }
        else if ((pathOwner.m_State & PathFlags.Updated) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.FillNavigationPaths(target, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements, ref invalidPath);
        }
        for (int index = 1; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          TrainCurrentLane currentLane1 = this.m_CurrentLaneData[vehicle];
          // ISSUE: reference to a compiler-generated method
          if (!this.HasValidLanes(currentLane1))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform1 = this.m_TransformData[vehicle];
            // ISSUE: reference to a compiler-generated field
            Train train1 = this.m_TrainData[vehicle];
            // ISSUE: reference to a compiler-generated method
            this.TryFindCurrentLane(ref currentLane1, transform1, train1, prefabTrainData);
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentLaneData[vehicle] = currentLane1;
          }
        }
        if (invalidPath == 0)
          return;
        navigationLanes.Clear();
        pathElements.Clear();
        pathOwner.m_ElementIndex = 0;
        pathOwner.m_State |= PathFlags.Obsolete;
        currentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.Return;
      }

      private bool HasValidLanes(TrainCurrentLane currentLaneData)
      {
        return !(currentLaneData.m_Front.m_Lane == Entity.Null) && !(currentLaneData.m_Rear.m_Lane == Entity.Null) && !(currentLaneData.m_FrontCache.m_Lane == Entity.Null) && !(currentLaneData.m_RearCache.m_Lane == Entity.Null) && (currentLaneData.m_Front.m_LaneFlags & TrainLaneFlags.Obsolete) == (TrainLaneFlags) 0;
      }

      private void TryFindCurrentLane(
        ref TrainCurrentLane currentLane,
        Game.Objects.Transform transform,
        Train train,
        TrainData prefabTrainData)
      {
        currentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.Obsolete;
        currentLane.m_Front.m_Lane = Entity.Null;
        currentLane.m_Rear.m_Lane = Entity.Null;
        currentLane.m_FrontCache.m_Lane = Entity.Null;
        currentLane.m_RearCache.m_Lane = Entity.Null;
        float3 pivot1;
        float3 pivot2;
        VehicleUtils.CalculateTrainNavigationPivots(transform, prefabTrainData, out pivot1, out pivot2);
        if ((train.m_Flags & Game.Vehicles.TrainFlags.Reversed) != (Game.Vehicles.TrainFlags) 0)
          CommonUtils.Swap<float3>(ref pivot1, ref pivot2);
        float range = 100f;
        Bounds3 bounds3 = MathUtils.Expand(MathUtils.Bounds(pivot1, pivot2), (float3) range);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TrainNavigationHelpers.FindLaneIterator iterator = new TrainNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_FrontPivot = pivot1,
          m_RearPivot = pivot2,
          m_MinDistance = (float2) range,
          m_Result = currentLane,
          m_TrackType = prefabTrainData.m_TrackType,
          m_SubLanes = this.m_Lanes,
          m_TrackLaneData = this.m_TrackLaneData,
          m_CurveData = this.m_CurveData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabTrackLaneData = this.m_PrefabTrackLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<TrainNavigationHelpers.FindLaneIterator>(ref iterator);
        currentLane = iterator.m_Result;
      }

      private void FillNavigationPaths(
        Game.Common.Target target,
        ref TrainCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements,
        ref int invalidPath)
      {
        if ((currentLane.m_Front.m_LaneFlags & TrainLaneFlags.EndOfPath) != (TrainLaneFlags) 0)
          return;
        for (int index = 0; index < 10000; ++index)
        {
          TrainNavigationLane elem;
          if (index >= navigationLanes.Length)
          {
            if (pathOwner.m_ElementIndex >= pathElements.Length || pathOwner.m_ElementIndex + 1 >= pathElements.Length && (pathOwner.m_State & PathFlags.Pending) != (PathFlags) 0)
              break;
            PathElement pathElement = pathElements[pathOwner.m_ElementIndex++];
            elem = new TrainNavigationLane();
            elem.m_Lane = pathElement.m_Target;
            elem.m_CurvePosition = pathElement.m_TargetDelta;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrackLaneData.HasComponent(elem.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.TrackLane trackLane = this.m_TrackLaneData[elem.m_Lane];
              if (pathOwner.m_ElementIndex >= pathElements.Length)
              {
                elem.m_Flags |= TrainLaneFlags.EndOfPath;
              }
              else
              {
                if ((pathElement.m_Flags & PathElementFlags.Return) != (PathElementFlags) 0)
                  elem.m_Flags |= TrainLaneFlags.Return;
                if ((trackLane.m_Flags & (TrackLaneFlags.Twoway | TrackLaneFlags.Switch | TrackLaneFlags.DiamondCrossing | TrackLaneFlags.CrossingTraffic)) != (TrackLaneFlags) 0 && (trackLane.m_Flags & TrackLaneFlags.MergingTraffic) == (TrackLaneFlags) 0 || (pathElement.m_Flags & PathElementFlags.Reverse) != (PathElementFlags) 0)
                  elem.m_Flags |= TrainLaneFlags.KeepClear;
              }
              if ((trackLane.m_Flags & TrackLaneFlags.Exclusive) != (TrackLaneFlags) 0)
                elem.m_Flags |= TrainLaneFlags.Exclusive;
              if ((trackLane.m_Flags & TrackLaneFlags.TurnLeft) != (TrackLaneFlags) 0)
                elem.m_Flags |= TrainLaneFlags.TurnLeft;
              if ((trackLane.m_Flags & TrackLaneFlags.TurnRight) != (TrackLaneFlags) 0)
                elem.m_Flags |= TrainLaneFlags.TurnRight;
              navigationLanes.Add(elem);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(elem.m_Lane))
              {
                elem.m_Flags |= TrainLaneFlags.Connection;
                if (pathOwner.m_ElementIndex >= pathElements.Length)
                  elem.m_Flags |= TrainLaneFlags.EndOfPath;
                navigationLanes.Add(elem);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_EntityLookup.Exists(elem.m_Lane))
                {
                  if (pathOwner.m_ElementIndex >= pathElements.Length)
                  {
                    if (navigationLanes.Length > 0)
                    {
                      TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
                      navigationLane.m_Flags |= TrainLaneFlags.EndOfPath;
                      navigationLanes[navigationLanes.Length - 1] = navigationLane;
                    }
                    else
                      currentLane.m_Front.m_LaneFlags |= TrainLaneFlags.EndOfPath;
                    elem.m_Flags |= TrainLaneFlags.ParkingSpace;
                    navigationLanes.Add(elem);
                    break;
                  }
                  continue;
                }
                ++invalidPath;
                break;
              }
            }
          }
          else
          {
            elem = navigationLanes[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_EntityLookup.Exists(elem.m_Lane))
            {
              ++invalidPath;
              break;
            }
          }
          if ((elem.m_Flags & TrainLaneFlags.EndOfPath) != (TrainLaneFlags) 0 || (elem.m_Flags & (TrainLaneFlags.Reserved | TrainLaneFlags.KeepClear | TrainLaneFlags.Connection)) == (TrainLaneFlags) 0)
            break;
        }
      }

      private void UpdateTrainLimits(
        ref TrainData prefabTrainData,
        DynamicBuffer<LayoutElement> layout)
      {
        for (int index = 1; index < layout.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          TrainData trainData = this.m_PrefabTrainData[this.m_PrefabRefData[layout[index].m_Vehicle].m_Prefab];
          prefabTrainData.m_MaxSpeed = math.min(prefabTrainData.m_MaxSpeed, trainData.m_MaxSpeed);
          prefabTrainData.m_Acceleration = math.min(prefabTrainData.m_Acceleration, trainData.m_Acceleration);
          prefabTrainData.m_Braking = math.min(prefabTrainData.m_Braking, trainData.m_Braking);
        }
      }

      private void UpdateNavigationTarget(
        int priority,
        Entity controller,
        Game.Objects.Transform transform,
        Moving moving,
        Train train,
        Game.Common.Target target,
        PrefabRef prefabRef,
        TrainData prefabTrainData,
        ObjectGeometryData prefabObjectGeometryData,
        ref TrainNavigation navigation,
        ref TrainCurrentLane currentLane,
        ref Blocker blocker,
        ref Odometer odometer,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<LayoutElement> layout)
      {
        float timeStep = 0.266666681f;
        float num1 = navigation.m_Speed;
        bool ignoreObstacles = ((currentLane.m_Front.m_LaneFlags | currentLane.m_FrontCache.m_LaneFlags | currentLane.m_Rear.m_LaneFlags | currentLane.m_RearCache.m_LaneFlags) & TrainLaneFlags.Connection) > (TrainLaneFlags) 0;
        for (int index = 1; index < layout.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          TrainCurrentLane trainCurrentLane = this.m_CurrentLaneData[layout[index].m_Vehicle];
          ignoreObstacles |= ((trainCurrentLane.m_Front.m_LaneFlags | trainCurrentLane.m_FrontCache.m_LaneFlags | trainCurrentLane.m_Rear.m_LaneFlags | trainCurrentLane.m_RearCache.m_LaneFlags) & TrainLaneFlags.Connection) > (TrainLaneFlags) 0;
        }
        if (ignoreObstacles)
        {
          prefabTrainData.m_MaxSpeed = 277.777771f;
          prefabTrainData.m_Acceleration = 277.777771f;
          prefabTrainData.m_Braking = 277.777771f;
        }
        else
          num1 = math.min(num1, prefabTrainData.m_MaxSpeed);
        Bounds1 bounds1 = ignoreObstacles || (currentLane.m_Front.m_LaneFlags & TrainLaneFlags.ResetSpeed) != (TrainLaneFlags) 0 ? new Bounds1(0.0f, prefabTrainData.m_MaxSpeed) : VehicleUtils.CalculateSpeedRange(prefabTrainData, num1, timeStep);
        float3 pivot1;
        float3 pivot2;
        VehicleUtils.CalculateTrainNavigationPivots(transform, prefabTrainData, out pivot1, out pivot2);
        if ((train.m_Flags & Game.Vehicles.TrainFlags.Reversed) != (Game.Vehicles.TrainFlags) 0)
        {
          CommonUtils.Swap<float3>(ref pivot1, ref pivot2);
          prefabTrainData.m_BogieOffsets = prefabTrainData.m_BogieOffsets.yx;
          prefabTrainData.m_AttachOffsets = prefabTrainData.m_AttachOffsets.yx;
        }
        bool flag1 = blocker.m_Type == BlockerType.Temporary;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TrainLaneSpeedIterator laneSpeedIterator = new TrainLaneSpeedIterator()
        {
          m_TransformData = this.m_TransformData,
          m_MovingData = this.m_MovingData,
          m_CarData = this.m_CarData,
          m_TrainData = this.m_TrainData,
          m_CurveData = this.m_CurveData,
          m_TrackLaneData = this.m_TrackLaneData,
          m_ControllerData = this.m_ControllerData,
          m_LaneReservationData = this.m_LaneReservationData,
          m_LaneSignalData = this.m_LaneSignalData,
          m_CreatureData = this.m_CreatureData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
          m_PrefabCarData = this.m_PrefabCarData,
          m_PrefabTrainData = this.m_PrefabTrainData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_LaneOverlapData = this.m_LaneOverlaps,
          m_LaneObjectData = this.m_LaneObjects,
          m_Controller = controller,
          m_Priority = priority,
          m_TimeStep = timeStep,
          m_SafeTimeStep = timeStep + 0.5f,
          m_CurrentSpeed = num1,
          m_SpeedRange = bounds1,
          m_RearPosition = pivot2,
          m_PushBlockers = (currentLane.m_Front.m_LaneFlags & TrainLaneFlags.PushBlockers) > (TrainLaneFlags) 0,
          m_MaxSpeed = bounds1.max,
          m_CurrentPosition = pivot1
        };
        if (currentLane.m_Front.m_Lane == Entity.Null)
        {
          navigation.m_Speed = math.max(0.0f, num1 - prefabTrainData.m_Braking * timeStep);
          blocker.m_Blocker = Entity.Null;
          blocker.m_Type = BlockerType.None;
          blocker.m_MaxSpeed = byte.MaxValue;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((currentLane.m_Front.m_LaneFlags & TrainLaneFlags.HighBeams) == (TrainLaneFlags) 0 && prefabTrainData.m_TrackType != TrackTypes.Tram && this.m_TrackLaneData.HasComponent(currentLane.m_Front.m_Lane) && (this.m_TrackLaneData[currentLane.m_Front.m_Lane].m_Flags & TrackLaneFlags.Station) == (TrackLaneFlags) 0)
            currentLane.m_Front.m_LaneFlags |= TrainLaneFlags.HighBeams;
          bool flag2 = false;
          bool needSignal = false;
          for (int index = layout.Length - 1; index >= 1; --index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane trainCurrentLane = this.m_CurrentLaneData[vehicle];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            TrainData trainData = this.m_PrefabTrainData[this.m_PrefabRefData[vehicle].m_Prefab];
            laneSpeedIterator.m_PrefabTrain = trainData;
            laneSpeedIterator.IteratePrevLane(trainCurrentLane.m_RearCache.m_Lane, out needSignal);
            if (needSignal)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, trainCurrentLane.m_RearCache.m_Lane, priority));
            }
            laneSpeedIterator.IteratePrevLane(trainCurrentLane.m_Rear.m_Lane, out needSignal);
            if (needSignal)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, trainCurrentLane.m_Rear.m_Lane, priority));
            }
            laneSpeedIterator.IteratePrevLane(trainCurrentLane.m_FrontCache.m_Lane, out needSignal);
            if (needSignal)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, trainCurrentLane.m_FrontCache.m_Lane, priority));
            }
            laneSpeedIterator.IteratePrevLane(trainCurrentLane.m_Front.m_Lane, out needSignal);
            if (needSignal)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, trainCurrentLane.m_Front.m_Lane, priority));
            }
          }
          bool exclusive = (currentLane.m_Front.m_LaneFlags & TrainLaneFlags.Exclusive) > (TrainLaneFlags) 0;
          bool skipCurrent = false;
          if (!exclusive && navigationLanes.Length != 0)
            skipCurrent = (navigationLanes[0].m_Flags & (TrainLaneFlags.Reserved | TrainLaneFlags.Exclusive)) == (TrainLaneFlags.Reserved | TrainLaneFlags.Exclusive);
          laneSpeedIterator.m_PrefabTrain = prefabTrainData;
          laneSpeedIterator.m_PrefabObjectGeometry = prefabObjectGeometryData;
          laneSpeedIterator.IteratePrevLane(currentLane.m_RearCache.m_Lane, out needSignal);
          if (needSignal)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, currentLane.m_RearCache.m_Lane, priority));
          }
          laneSpeedIterator.IteratePrevLane(currentLane.m_Rear.m_Lane, out needSignal);
          if (needSignal)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, currentLane.m_Rear.m_Lane, priority));
          }
          laneSpeedIterator.IteratePrevLane(currentLane.m_FrontCache.m_Lane, out needSignal);
          if (needSignal)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, currentLane.m_FrontCache.m_Lane, priority));
          }
          int num2 = laneSpeedIterator.IterateFirstLane(currentLane.m_Front.m_Lane, currentLane.m_Front.m_CurvePosition, exclusive, ignoreObstacles, skipCurrent, out needSignal) ? 1 : 0;
          if (needSignal)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, currentLane.m_Front.m_Lane, priority));
          }
          if (num2 == 0)
          {
            if ((currentLane.m_Front.m_LaneFlags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.Return)) == (TrainLaneFlags) 0)
            {
              for (int index = 0; index < navigationLanes.Length; ++index)
              {
                TrainNavigationLane navigationLane = navigationLanes[index];
                currentLane.m_Front.m_LaneFlags |= navigationLane.m_Flags & (TrainLaneFlags.TurnLeft | TrainLaneFlags.TurnRight);
                bool flag3 = navigationLane.m_Lane == currentLane.m_Front.m_Lane;
                if ((navigationLane.m_Flags & (TrainLaneFlags.Reserved | TrainLaneFlags.Connection)) == (TrainLaneFlags) 0)
                {
                  while ((navigationLane.m_Flags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.BlockReserve)) == (TrainLaneFlags) 0 && ++index < navigationLanes.Length)
                    navigationLane = navigationLanes[index];
                  laneSpeedIterator.IterateTarget(navigationLane.m_Lane, flag3);
                  goto label_50;
                }
                else
                {
                  if ((navigationLane.m_Flags & TrainLaneFlags.Connection) != (TrainLaneFlags) 0)
                  {
                    laneSpeedIterator.m_PrefabTrain.m_MaxSpeed = 277.777771f;
                    laneSpeedIterator.m_PrefabTrain.m_Acceleration = 277.777771f;
                    laneSpeedIterator.m_PrefabTrain.m_Braking = 277.777771f;
                    laneSpeedIterator.m_SpeedRange = new Bounds1(0.0f, 277.777771f);
                  }
                  float minOffset = math.select(-1f, currentLane.m_Front.m_CurvePosition.z, flag3);
                  int num3 = laneSpeedIterator.IterateNextLane(navigationLane.m_Lane, navigationLane.m_CurvePosition, minOffset, (navigationLane.m_Flags & TrainLaneFlags.Exclusive) > (TrainLaneFlags) 0, flag3 | ignoreObstacles, out needSignal) ? 1 : 0;
                  if (needSignal)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_LaneSignals.Enqueue(new TrainNavigationHelpers.LaneSignal(controller, navigationLane.m_Lane, priority));
                  }
                  if (num3 == 0)
                  {
                    if ((navigationLane.m_Flags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.Return)) != (TrainLaneFlags) 0)
                      break;
                  }
                  else
                    goto label_50;
                }
              }
            }
            flag2 = laneSpeedIterator.IterateTarget();
          }
label_50:
          navigation.m_Speed = laneSpeedIterator.m_MaxSpeed;
          float num4 = math.select(1.83600008f, 2.29499984f, (prefabTrainData.m_TrackType & TrackTypes.Tram) != 0);
          blocker.m_Blocker = laneSpeedIterator.m_Blocker;
          blocker.m_Type = laneSpeedIterator.m_BlockerType;
          blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(laneSpeedIterator.m_MaxSpeed * num4), 0, (int) byte.MaxValue);
          int num5 = blocker.m_Type == BlockerType.Temporary ? 1 : 0;
          if (num5 != (flag1 ? 1 : 0) || (double) currentLane.m_Duration >= 30.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.ApplySideEffects(ref currentLane, currentLane.m_Front.m_Lane, prefabRef, prefabTrainData);
          }
          if (num5 != 0)
          {
            if ((double) currentLane.m_Duration >= 5.0)
              currentLane.m_Front.m_LaneFlags |= TrainLaneFlags.PushBlockers;
          }
          else if ((double) currentLane.m_Duration >= 5.0)
            currentLane.m_Front.m_LaneFlags &= ~TrainLaneFlags.PushBlockers;
          float num6 = num1 * timeStep;
          currentLane.m_Duration += timeStep;
          currentLane.m_Distance += num6;
          odometer.m_Distance += num6;
          TrainLaneFlags trainLaneFlags1 = TrainLaneFlags.EndOfPath | TrainLaneFlags.EndReached;
          if ((currentLane.m_Front.m_LaneFlags & trainLaneFlags1) == trainLaneFlags1)
            return;
          float num7 = navigation.m_Speed * timeStep;
          TrainBogieCache tempCache = new TrainBogieCache();
          // ISSUE: reference to a compiler-generated method
          bool resetCache = this.ShouldResetCache(currentLane.m_Front, currentLane.m_FrontCache);
          while (true)
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[currentLane.m_Front.m_Lane];
            bool flag4 = (double) curve.m_Length > 0.10000000149011612;
            // ISSUE: reference to a compiler-generated method
            if (!flag4 || !this.MoveTarget(pivot1, ref navigation.m_Front, num7, curve.m_Bezier, ref currentLane.m_Front.m_CurvePosition))
            {
              if (navigationLanes.Length != 0 && (currentLane.m_Front.m_LaneFlags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.Return)) == (TrainLaneFlags) 0)
              {
                TrainNavigationLane navigationLane = navigationLanes[0];
                // ISSUE: reference to a compiler-generated field
                if ((navigationLane.m_Flags & (TrainLaneFlags.Reserved | TrainLaneFlags.Connection)) != (TrainLaneFlags) 0 && this.m_EntityLookup.Exists(navigationLane.m_Lane))
                {
                  if (ignoreObstacles && (navigationLane.m_Flags & TrainLaneFlags.Connection) == (TrainLaneFlags) 0)
                    navigationLane.m_Flags |= TrainLaneFlags.ResetSpeed;
                  Game.Net.TrackLane componentData;
                  // ISSUE: reference to a compiler-generated field
                  if ((currentLane.m_Front.m_LaneFlags & TrainLaneFlags.HighBeams) != (TrainLaneFlags) 0 && prefabTrainData.m_TrackType != TrackTypes.Tram && this.m_TrackLaneData.TryGetComponent(navigationLane.m_Lane, out componentData) && (componentData.m_Flags & TrackLaneFlags.Station) == (TrackLaneFlags) 0)
                    navigationLane.m_Flags |= TrainLaneFlags.HighBeams;
                  if (flag4)
                  {
                    tempCache = currentLane.m_FrontCache;
                    currentLane.m_FrontCache = new TrainBogieCache(currentLane.m_Front);
                  }
                  TrainLaneFlags trainLaneFlags2 = currentLane.m_Front.m_LaneFlags & TrainLaneFlags.PushBlockers;
                  // ISSUE: reference to a compiler-generated method
                  this.ApplySideEffects(ref currentLane, currentLane.m_Front.m_Lane, prefabRef, prefabTrainData);
                  currentLane.m_Front = new TrainBogieLane(navigationLane);
                  currentLane.m_Front.m_LaneFlags |= trainLaneFlags2;
                  navigationLanes.RemoveAt(0);
                }
                else
                  goto label_79;
              }
              else
                goto label_69;
            }
            else
              break;
          }
          if (flag2 && (double) navigation.m_Speed < 0.10000000149011612 && (double) num1 < 0.10000000149011612)
          {
            currentLane.m_Front.m_LaneFlags |= TrainLaneFlags.EndReached;
            if ((currentLane.m_Front.m_LaneFlags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.Return)) == (TrainLaneFlags) 0)
            {
              for (int index = 0; index < navigationLanes.Length; ++index)
              {
                TrainLaneFlags trainLaneFlags3 = navigationLanes[index].m_Flags & (TrainLaneFlags.EndOfPath | TrainLaneFlags.Return);
                if (trainLaneFlags3 != (TrainLaneFlags) 0)
                {
                  currentLane.m_Front.m_LaneFlags |= trainLaneFlags3;
                  navigationLanes.RemoveRange(0, index + 1);
                  break;
                }
              }
              goto label_79;
            }
            else
              goto label_79;
          }
          else
            goto label_79;
label_69:
          if (flag2 && (double) navigation.m_Speed < 0.10000000149011612 && (double) num1 < 0.10000000149011612)
            currentLane.m_Front.m_LaneFlags |= TrainLaneFlags.EndReached;
label_79:
          // ISSUE: reference to a compiler-generated method
          this.ClampPosition(ref navigation.m_Front.m_Position, pivot1, num7);
          navigation.m_Front.m_Direction = math.normalizesafe(navigation.m_Front.m_Direction);
          float3 position = navigation.m_Front.m_Position;
          float followDistance1 = math.csum(prefabTrainData.m_BogieOffsets);
          currentLane.m_Front.m_CurvePosition.z = currentLane.m_Front.m_CurvePosition.y;
          // ISSUE: reference to a compiler-generated method
          this.UpdateFollowerBogie(ref currentLane.m_Rear, ref currentLane.m_RearCache, ref navigation.m_Rear, ref resetCache, ref tempCache, ref currentLane.m_FrontCache, currentLane.m_Front, position, followDistance1);
          if (layout.Length == 1)
          {
            currentLane.m_RearCache = new TrainBogieCache(currentLane.m_Rear);
          }
          else
          {
            position = navigation.m_Rear.m_Position;
            followDistance1 = prefabTrainData.m_AttachOffsets.y - prefabTrainData.m_BogieOffsets.y;
          }
          TrainCurrentLane trainCurrentLane1 = currentLane;
          for (int index = 1; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            Train train1 = this.m_TrainData[vehicle];
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane currentLaneData = this.m_CurrentLaneData[vehicle];
            // ISSUE: reference to a compiler-generated field
            TrainNavigation trainNavigation = this.m_NavigationData[vehicle];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRefData = this.m_PrefabRefData[vehicle];
            // ISSUE: reference to a compiler-generated field
            TrainData prefabTrainData1 = this.m_PrefabTrainData[prefabRefData.m_Prefab];
            if ((train1.m_Flags & Game.Vehicles.TrainFlags.Reversed) != (Game.Vehicles.TrainFlags) 0)
            {
              prefabTrainData1.m_BogieOffsets = prefabTrainData1.m_BogieOffsets.yx;
              prefabTrainData1.m_AttachOffsets = prefabTrainData1.m_AttachOffsets.yx;
            }
            trainNavigation.m_Speed = navigation.m_Speed;
            currentLaneData.m_Duration += timeStep;
            currentLaneData.m_Distance += num6;
            Entity lane = currentLaneData.m_Front.m_Lane;
            float followDistance2 = followDistance1 + (prefabTrainData1.m_AttachOffsets.x - prefabTrainData1.m_BogieOffsets.x);
            // ISSUE: reference to a compiler-generated method
            this.UpdateFollowerBogie(ref currentLaneData.m_Front, ref currentLaneData.m_FrontCache, ref trainNavigation.m_Front, ref resetCache, ref tempCache, ref trainCurrentLane1.m_RearCache, trainCurrentLane1.m_Rear, position, followDistance2);
            if (currentLaneData.m_Front.m_Lane != lane || (double) currentLaneData.m_Duration >= 30.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ApplySideEffects(ref currentLaneData, lane, prefabRefData, prefabTrainData1);
            }
            position = trainNavigation.m_Front.m_Position;
            followDistance1 = math.csum(prefabTrainData1.m_BogieOffsets);
            // ISSUE: reference to a compiler-generated method
            this.UpdateFollowerBogie(ref currentLaneData.m_Rear, ref currentLaneData.m_RearCache, ref trainNavigation.m_Rear, ref resetCache, ref tempCache, ref currentLaneData.m_FrontCache, currentLaneData.m_Front, position, followDistance1);
            if (index == 1)
            {
              currentLane = trainCurrentLane1;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CurrentLaneData[layout[index - 1].m_Vehicle] = trainCurrentLane1;
            }
            if (index == layout.Length - 1)
            {
              currentLaneData.m_RearCache = new TrainBogieCache(currentLaneData.m_Rear);
            }
            else
            {
              position = trainNavigation.m_Rear.m_Position;
              followDistance1 = prefabTrainData1.m_AttachOffsets.y - prefabTrainData1.m_BogieOffsets.y;
            }
            trainCurrentLane1 = currentLaneData;
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentLaneData[vehicle] = currentLaneData;
            // ISSUE: reference to a compiler-generated field
            this.m_NavigationData[vehicle] = trainNavigation;
          }
        }
      }

      private void ClampPosition(ref float3 position, float3 original, float maxDistance)
      {
        position = original + MathUtils.ClampLength(position - original, maxDistance);
      }

      private bool ShouldResetCache(TrainBogieLane bogie, TrainBogieCache cache)
      {
        return math.all(bogie.m_CurvePosition == bogie.m_CurvePosition.x) && math.all(cache.m_CurvePosition == bogie.m_CurvePosition.x) && cache.m_Lane == bogie.m_Lane;
      }

      private void UpdateFollowerBogie(
        ref TrainBogieLane bogie,
        ref TrainBogieCache cache,
        ref TrainBogiePosition position,
        ref bool resetCache,
        ref TrainBogieCache tempCache,
        ref TrainBogieCache nextCache,
        TrainBogieLane nextBogie,
        float3 followPosition,
        float followDistance)
      {
        TrainBogieCache trainBogieCache = new TrainBogieCache();
        float3 y = position.m_Position - followPosition;
        if (resetCache)
        {
          if (bogie.m_Lane == nextBogie.m_Lane)
          {
            tempCache = new TrainBogieCache();
            nextCache = new TrainBogieCache(nextBogie);
            nextCache.m_CurvePosition.x = bogie.m_CurvePosition.w;
          }
          else if (bogie.m_Lane != Entity.Null && nextBogie.m_Lane != Entity.Null)
          {
            tempCache = new TrainBogieCache(bogie);
            nextCache = new TrainBogieCache(nextBogie);
            // ISSUE: reference to a compiler-generated field
            Curve curve1 = this.m_CurveData[bogie.m_Lane];
            // ISSUE: reference to a compiler-generated field
            Curve curve2 = this.m_CurveData[nextBogie.m_Lane];
            float3 position1 = MathUtils.Position(curve1.m_Bezier, bogie.m_CurvePosition.w);
            double num1 = (double) MathUtils.Distance(curve1.m_Bezier, MathUtils.Position(curve2.m_Bezier, nextBogie.m_CurvePosition.x), out tempCache.m_CurvePosition.y);
            double num2 = (double) MathUtils.Distance(curve2.m_Bezier, position1, out nextCache.m_CurvePosition.x);
          }
        }
        // ISSUE: reference to a compiler-generated method
        resetCache = this.ShouldResetCache(bogie, cache);
        float w;
        while (true)
        {
          if (bogie.m_Lane != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[bogie.m_Lane];
            if (bogie.m_Lane == nextBogie.m_Lane && (double) bogie.m_CurvePosition.w == (double) nextBogie.m_CurvePosition.w)
            {
              w = bogie.m_CurvePosition.w;
              bogie.m_CurvePosition.zw = (float2) nextBogie.m_CurvePosition.y;
              // ISSUE: reference to a compiler-generated method
              if (!this.MoveFollowerTarget(followPosition, ref position, followDistance, curve.m_Bezier, ref bogie.m_CurvePosition))
                bogie.m_CurvePosition.w = w;
              else
                break;
            }
            else
            {
              bogie.m_CurvePosition.z = bogie.m_CurvePosition.w;
              // ISSUE: reference to a compiler-generated method
              if (this.MoveFollowerTarget(followPosition, ref position, followDistance, curve.m_Bezier, ref bogie.m_CurvePosition))
                goto label_16;
            }
          }
          if (!(nextBogie.m_Lane == bogie.m_Lane) || !nextBogie.m_CurvePosition.xw.Equals(bogie.m_CurvePosition.xw))
          {
            trainBogieCache = cache;
            cache = new TrainBogieCache(bogie);
            if (tempCache.m_Lane != Entity.Null)
            {
              bogie = new TrainBogieLane(tempCache);
              tempCache = new TrainBogieCache();
            }
            else
            {
              bogie = new TrainBogieLane(nextCache);
              nextCache = new TrainBogieCache(nextBogie);
            }
          }
          else
            goto label_16;
        }
        bogie.m_CurvePosition.w = w;
label_16:
        float3 x = position.m_Position - followPosition;
        if ((double) math.dot(x, y) <= 0.0)
        {
          x = y;
          position.m_Direction = -y;
        }
        if (MathUtils.TryNormalize(ref x, followDistance))
        {
          position.m_Position = followPosition + x;
          position.m_Direction = math.normalizesafe(position.m_Direction);
        }
        tempCache = trainBogieCache;
      }

      private void ApplySideEffects(
        ref TrainCurrentLane currentLaneData,
        Entity lane,
        PrefabRef prefabRefData,
        TrainData prefabTrainData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TrackLaneData.HasComponent(lane) && ((double) currentLaneData.m_Duration != 0.0 || (double) currentLaneData.m_Distance != 0.0))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.TrackLane trackLaneData = this.m_TrackLaneData[lane];
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[lane];
          float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(prefabTrainData, trackLaneData);
          float num1 = 1f / math.max(1f, curve.m_Length);
          float3 sideEffects = new float3();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSideEffectData.HasComponent(prefabRefData.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleSideEffectData vehicleSideEffectData = this.m_PrefabSideEffectData[prefabRefData.m_Prefab];
            float num2 = math.select(currentLaneData.m_Distance / currentLaneData.m_Duration, maxDriveSpeed, (double) currentLaneData.m_Duration == 0.0) / prefabTrainData.m_MaxSpeed;
            float s = math.saturate(num2 * num2);
            sideEffects = math.lerp(vehicleSideEffectData.m_Min, vehicleSideEffectData.m_Max, s) * new float3(math.min(1f, currentLaneData.m_Distance * num1), currentLaneData.m_Duration, currentLaneData.m_Duration);
          }
          float num3 = math.min(prefabTrainData.m_MaxSpeed, trackLaneData.m_SpeedLimit);
          float2 flow = new float2(currentLaneData.m_Duration * num3, currentLaneData.m_Distance) * num1;
          // ISSUE: reference to a compiler-generated field
          this.m_LaneEffects.Enqueue(new TrainNavigationHelpers.LaneEffects(lane, sideEffects, flow));
        }
        currentLaneData.m_Duration = 0.0f;
        currentLaneData.m_Distance = 0.0f;
      }

      private void TryReserveNavigationLanes(
        Train trainData,
        TrainData prefabTrainData,
        ref TrainNavigation navigationData,
        ref TrainCurrentLane currentLaneData,
        DynamicBuffer<TrainNavigationLane> navigationLanes)
      {
        float timeStep = 0.266666681f;
        if ((trainData.m_Flags & Game.Vehicles.TrainFlags.Reversed) != (Game.Vehicles.TrainFlags) 0)
        {
          prefabTrainData.m_BogieOffsets = prefabTrainData.m_BogieOffsets.yx;
          prefabTrainData.m_AttachOffsets = prefabTrainData.m_AttachOffsets.yx;
        }
        if (!(currentLaneData.m_Front.m_Lane != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[currentLaneData.m_Front.m_Lane];
        float num1 = math.max(0.0f, VehicleUtils.GetBrakingDistance(prefabTrainData, navigationData.m_Speed, timeStep) - 0.01f);
        float num2 = (float) ((double) prefabTrainData.m_AttachOffsets.x - (double) prefabTrainData.m_BogieOffsets.x + 2.0) + VehicleUtils.GetSignalDistance(prefabTrainData, navigationData.m_Speed);
        if ((double) currentLaneData.m_Front.m_CurvePosition.w > (double) currentLaneData.m_Front.m_CurvePosition.x)
        {
          currentLaneData.m_Front.m_CurvePosition.z = currentLaneData.m_Front.m_CurvePosition.y + num1 / math.max(1E-06f, curve1.m_Length);
          currentLaneData.m_Front.m_CurvePosition.z = math.min(currentLaneData.m_Front.m_CurvePosition.z, currentLaneData.m_Front.m_CurvePosition.w);
        }
        else
        {
          currentLaneData.m_Front.m_CurvePosition.z = currentLaneData.m_Front.m_CurvePosition.y - num1 / math.max(1E-06f, curve1.m_Length);
          currentLaneData.m_Front.m_CurvePosition.z = math.max(currentLaneData.m_Front.m_CurvePosition.z, currentLaneData.m_Front.m_CurvePosition.w);
        }
        float num3 = num1 - curve1.m_Length * math.abs(currentLaneData.m_Front.m_CurvePosition.w - currentLaneData.m_Front.m_CurvePosition.y);
        int index = 0;
        bool flag1 = (double) num3 > 0.0;
        for (bool flag2 = (double) num3 + (double) num2 > 0.0 || (currentLaneData.m_Front.m_LaneFlags & TrainLaneFlags.KeepClear) > (TrainLaneFlags) 0; flag2 && index < navigationLanes.Length; ++index)
        {
          TrainNavigationLane navigationLane = navigationLanes[index];
          if ((navigationLane.m_Flags & TrainLaneFlags.ParkingSpace) != (TrainLaneFlags) 0)
            break;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TrackLaneData.HasComponent(navigationLane.m_Lane))
          {
            navigationLane.m_Flags |= TrainLaneFlags.TryReserve;
            if (flag1)
              navigationLane.m_Flags |= TrainLaneFlags.FullReserve;
            else
              navigationLane.m_Flags &= ~TrainLaneFlags.FullReserve;
            navigationLanes[index] = navigationLane;
          }
          // ISSUE: reference to a compiler-generated field
          Curve curve2 = this.m_CurveData[navigationLane.m_Lane];
          num3 -= curve2.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
          flag1 = (double) num3 > 0.0;
          flag2 = (double) num3 + (double) num2 > 0.0 || (navigationLane.m_Flags & TrainLaneFlags.KeepClear) > (TrainLaneFlags) 0;
        }
      }

      private bool MoveTarget(
        float3 comparePosition,
        ref TrainBogiePosition targetPosition,
        float minDistance,
        Bezier4x3 curve,
        ref float4 curveDelta)
      {
        float3 y1 = MathUtils.Position(curve, curveDelta.w);
        if ((double) math.distance(comparePosition, y1) < (double) minDistance)
        {
          float t = math.lerp(curveDelta.y, curveDelta.w, 0.5f);
          float3 y2 = MathUtils.Position(curve, t);
          if ((double) math.distance(comparePosition, y2) < (double) minDistance)
          {
            curveDelta.y = curveDelta.w;
            targetPosition.m_Position = y1;
            targetPosition.m_Direction = MathUtils.Tangent(curve, curveDelta.w);
            targetPosition.m_Direction *= math.sign(curveDelta.w - curveDelta.x);
            return false;
          }
        }
        float3 y3 = MathUtils.Position(curve, curveDelta.y);
        if ((double) math.distance(comparePosition, y3) >= (double) minDistance)
        {
          targetPosition.m_Position = y3;
          targetPosition.m_Direction = MathUtils.Tangent(curve, curveDelta.y);
          targetPosition.m_Direction *= math.sign(curveDelta.w - curveDelta.x);
          return true;
        }
        float2 yw = curveDelta.yw;
        for (int index = 0; index < 8; ++index)
        {
          float t = math.lerp(yw.x, yw.y, 0.5f);
          float3 y4 = MathUtils.Position(curve, t);
          if ((double) math.distance(comparePosition, y4) < (double) minDistance)
            yw.x = t;
          else
            yw.y = t;
        }
        curveDelta.y = yw.y;
        targetPosition.m_Position = MathUtils.Position(curve, yw.y);
        targetPosition.m_Direction = MathUtils.Tangent(curve, yw.y);
        targetPosition.m_Direction *= math.sign(curveDelta.w - curveDelta.x);
        return true;
      }

      private bool MoveFollowerTarget(
        float3 comparePosition,
        ref TrainBogiePosition targetPosition,
        float maxDistance,
        Bezier4x3 curve,
        ref float4 curveDelta)
      {
        float3 y1 = MathUtils.Position(curve, curveDelta.w);
        if ((double) math.distance(comparePosition, y1) > (double) maxDistance)
        {
          curveDelta.y = curveDelta.w;
          targetPosition.m_Position = y1;
          targetPosition.m_Direction = MathUtils.Tangent(curve, curveDelta.w);
          targetPosition.m_Direction *= math.sign(curveDelta.w - curveDelta.x);
          return false;
        }
        float2 yw = curveDelta.yw;
        for (int index = 0; index < 8; ++index)
        {
          float t = math.lerp(yw.x, yw.y, 0.5f);
          float3 y2 = MathUtils.Position(curve, t);
          if ((double) math.distance(comparePosition, y2) > (double) maxDistance)
            yw.x = t;
          else
            yw.y = t;
        }
        curveDelta.y = yw.x;
        targetPosition.m_Position = MathUtils.Position(curve, yw.x);
        targetPosition.m_Direction = MathUtils.Tangent(curve, yw.x);
        targetPosition.m_Direction *= math.sign(curveDelta.w - curveDelta.x);
        return true;
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct UpdateLaneSignalsJob : IJob
    {
      public NativeQueue<TrainNavigationHelpers.LaneSignal> m_LaneSignalQueue;
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_LaneSignalQueue.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          TrainNavigationHelpers.LaneSignal laneSignal1 = this.m_LaneSignalQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          Game.Net.LaneSignal laneSignal2 = this.m_LaneSignalData[laneSignal1.m_Lane];
          if ((int) laneSignal1.m_Priority > (int) laneSignal2.m_Priority)
          {
            laneSignal2.m_Petitioner = laneSignal1.m_Petitioner;
            laneSignal2.m_Priority = laneSignal1.m_Priority;
            // ISSUE: reference to a compiler-generated field
            this.m_LaneSignalData[laneSignal1.m_Lane] = laneSignal2;
          }
        }
      }
    }

    [BurstCompile]
    private struct UpdateLaneReservationsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutType;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_CurrentLaneData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      public BufferTypeHandle<TrainNavigationLane> m_NavigationLaneType;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ReserveCurrentLanes(this.m_Chunks[index]);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.TryReserveNavigationLanes(this.m_Chunks[index]);
        }
      }

      private void ReserveCurrentLanes(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor[index1];
          Entity prevLane = Entity.Null;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity vehicle = dynamicBuffer[index2].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane currentLaneData = this.m_CurrentLaneData[vehicle];
            // ISSUE: reference to a compiler-generated method
            this.ReserveCurrentLanes(vehicle, currentLaneData, ref prevLane, 98);
          }
        }
      }

      private void ReserveCurrentLanes(
        Entity entity,
        TrainCurrentLane currentLaneData,
        ref Entity prevLane,
        int priority)
      {
        if (currentLaneData.m_Front.m_Lane != Entity.Null && currentLaneData.m_Front.m_Lane != prevLane)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReserveLane(entity, currentLaneData.m_Front.m_Lane, priority);
        }
        if (currentLaneData.m_FrontCache.m_Lane != Entity.Null && currentLaneData.m_FrontCache.m_Lane != currentLaneData.m_Front.m_Lane)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReserveLane(entity, currentLaneData.m_FrontCache.m_Lane, priority);
        }
        if (currentLaneData.m_Rear.m_Lane != Entity.Null && currentLaneData.m_Rear.m_Lane != currentLaneData.m_FrontCache.m_Lane)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReserveLane(entity, currentLaneData.m_Rear.m_Lane, priority);
        }
        if (currentLaneData.m_RearCache.m_Lane != Entity.Null && currentLaneData.m_RearCache.m_Lane != currentLaneData.m_Rear.m_Lane)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReserveLane(entity, currentLaneData.m_RearCache.m_Lane, priority);
        }
        prevLane = currentLaneData.m_RearCache.m_Lane;
      }

      private void ReserveLane(Entity entity, Entity lane, int priority)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LaneReservationData.HasComponent(lane))
          return;
        // ISSUE: reference to a compiler-generated field
        ref Game.Net.LaneReservation local = ref this.m_LaneReservationData.GetRefRW(lane).ValueRW;
        if (priority <= (int) local.m_Next.m_Priority)
          return;
        if (priority >= (int) local.m_Prev.m_Priority)
          local.m_Blocker = entity;
        local.m_Next.m_Priority = (byte) priority;
      }

      private void TryReserveNavigationLanes(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor1 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TrainNavigationLane> bufferAccessor2 = chunk.GetBufferAccessor<TrainNavigationLane>(ref this.m_NavigationLaneType);
        for (int index = 0; index < bufferAccessor1.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray[index];
          DynamicBuffer<LayoutElement> layout = bufferAccessor1[index];
          DynamicBuffer<TrainNavigationLane> navigationLanes = bufferAccessor2[index];
          if (layout.Length >= 1)
          {
            Entity vehicle = layout[0].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane trainCurrentLane = this.m_CurrentLaneData[vehicle];
            // ISSUE: reference to a compiler-generated field
            int priority = VehicleUtils.GetPriority(this.m_PrefabTrainData[prefabRef.m_Prefab]);
            // ISSUE: reference to a compiler-generated method
            this.TryReserveNavigationLanes(vehicle, navigationLanes, layout, trainCurrentLane.m_Front.m_Lane, 98, priority);
          }
        }
      }

      private void TryReserveNavigationLanes(
        Entity entity,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<LayoutElement> layout,
        Entity prevLane,
        int priority,
        int fullPriority)
      {
        Entity entity1 = prevLane;
        int a = -1;
        int num = -1;
        for (int index = 0; index < navigationLanes.Length; ++index)
        {
          ref TrainNavigationLane local = ref navigationLanes.ElementAt(index);
          if ((local.m_Flags & (TrainLaneFlags.Reserved | TrainLaneFlags.TryReserve | TrainLaneFlags.Connection)) != (TrainLaneFlags) 0)
          {
            if ((local.m_Flags & (TrainLaneFlags.Reserved | TrainLaneFlags.Connection)) != (TrainLaneFlags) 0)
            {
              a = index;
              num = index;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (local.m_Lane == prevLane || (local.m_Flags & TrainLaneFlags.Exclusive) == (TrainLaneFlags) 0 || this.CanReserveLane(local.m_Lane, layout))
              {
                local.m_Flags &= ~TrainLaneFlags.BlockReserve;
                a = math.select(a, index, a == index - 1 && local.m_Lane == prevLane);
                num = index;
              }
              else
              {
                local.m_Flags |= TrainLaneFlags.BlockReserve;
                num = a;
                break;
              }
            }
            prevLane = local.m_Lane;
          }
          else
            break;
        }
        prevLane = entity1;
        for (int index = 0; index <= num; ++index)
        {
          ref TrainNavigationLane local = ref navigationLanes.ElementAt(index);
          if (local.m_Lane != prevLane)
          {
            bool c = (local.m_Flags & (TrainLaneFlags.TryReserve | TrainLaneFlags.FullReserve)) == (TrainLaneFlags.TryReserve | TrainLaneFlags.FullReserve);
            int priority1 = math.select(priority, fullPriority, c);
            // ISSUE: reference to a compiler-generated method
            this.ReserveLane(entity, local.m_Lane, priority1);
          }
          if ((local.m_Flags & TrainLaneFlags.TryReserve) != (TrainLaneFlags) 0)
          {
            local.m_Flags &= ~(TrainLaneFlags.TryReserve | TrainLaneFlags.FullReserve);
            local.m_Flags |= TrainLaneFlags.Reserved;
          }
          prevLane = local.m_Lane;
        }
      }

      private bool CanReserveLane(Entity lane, DynamicBuffer<LayoutElement> layout)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LaneReservationData.HasComponent(lane) || this.m_LaneReservationData[lane].GetPriority() == 0)
          return true;
        for (int index = 0; index < layout.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          TrainCurrentLane trainCurrentLane = this.m_CurrentLaneData[layout[index].m_Vehicle];
          if (trainCurrentLane.m_Front.m_Lane == lane || trainCurrentLane.m_FrontCache.m_Lane == lane || trainCurrentLane.m_Rear.m_Lane == lane || trainCurrentLane.m_RearCache.m_Lane == lane)
            return true;
        }
        return false;
      }
    }

    [BurstCompile]
    private struct ApplyLaneEffectsJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<LaneDeteriorationData> m_LaneDeteriorationData;
      public ComponentLookup<Game.Net.Pollution> m_PollutionData;
      public ComponentLookup<LaneCondition> m_LaneConditionData;
      public ComponentLookup<LaneFlow> m_LaneFlowData;
      public NativeQueue<TrainNavigationHelpers.LaneEffects> m_LaneEffectsQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_LaneEffectsQueue.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          TrainNavigationHelpers.LaneEffects laneEffects = this.m_LaneEffectsQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          Entity owner = this.m_OwnerData[laneEffects.m_Lane].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneConditionData.HasComponent(laneEffects.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            LaneDeteriorationData deteriorationData = this.m_LaneDeteriorationData[this.m_PrefabRefData[laneEffects.m_Lane].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            LaneCondition laneCondition = this.m_LaneConditionData[laneEffects.m_Lane];
            laneCondition.m_Wear = math.min(laneCondition.m_Wear + laneEffects.m_SideEffects.x * deteriorationData.m_TrafficFactor, 10f);
            // ISSUE: reference to a compiler-generated field
            this.m_LaneConditionData[laneEffects.m_Lane] = laneCondition;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneFlowData.HasComponent(laneEffects.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            LaneFlow laneFlow = this.m_LaneFlowData[laneEffects.m_Lane];
            laneFlow.m_Next += laneEffects.m_Flow;
            // ISSUE: reference to a compiler-generated field
            this.m_LaneFlowData[laneEffects.m_Lane] = laneFlow;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PollutionData.HasComponent(owner))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Pollution pollution = this.m_PollutionData[owner];
            pollution.m_Pollution += laneEffects.m_SideEffects.yz;
            // ISSUE: reference to a compiler-generated field
            this.m_PollutionData[owner] = pollution;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<TrainNavigationLane> __Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public ComponentLookup<TrainNavigation> __Game_Vehicles_TrainNavigation_RW_ComponentLookup;
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> __Game_Net_LaneSignal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleSideEffectData> __Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneDeteriorationData> __Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup;
      public ComponentLookup<Game.Net.Pollution> __Game_Net_Pollution_RW_ComponentLookup;
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RW_ComponentLookup;
      public ComponentLookup<LaneFlow> __Game_Net_LaneFlow_RW_ComponentLookup;
      public ComponentLookup<Game.Net.LaneSignal> __Game_Net_LaneSignal_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<TrainNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigation_RW_ComponentLookup = state.GetComponentLookup<TrainNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneSignal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup = state.GetComponentLookup<VehicleSideEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RW_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup = state.GetComponentLookup<LaneDeteriorationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Pollution_RW_ComponentLookup = state.GetComponentLookup<Game.Net.Pollution>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RW_ComponentLookup = state.GetComponentLookup<LaneCondition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneFlow_RW_ComponentLookup = state.GetComponentLookup<LaneFlow>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RW_ComponentLookup = state.GetComponentLookup<Game.Net.LaneSignal>();
      }
    }
  }
}
