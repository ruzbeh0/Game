// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WatercraftNavigationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
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
  public class WatercraftNavigationSystem : GameSystemBase
  {
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private EntityQuery m_VehicleQuery;
    private LaneObjectUpdater m_LaneObjectUpdater;
    private WatercraftNavigationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 8;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Watercraft>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<Game.Common.Target>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PathElement>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<WatercraftCurrentLane>(), ComponentType.ReadWrite<WatercraftNavigation>(), ComponentType.ReadWrite<WatercraftNavigationLane>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneObjectUpdater = new LaneObjectUpdater((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<WatercraftNavigationHelpers.LaneReservation> nativeQueue1 = new NativeQueue<WatercraftNavigationHelpers.LaneReservation>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<WatercraftNavigationHelpers.LaneEffects> nativeQueue2 = new NativeQueue<WatercraftNavigationHelpers.LaneEffects>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
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
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WatercraftNavigationSystem.UpdateNavigationJob jobData1 = new WatercraftNavigationSystem.UpdateNavigationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_WatercraftType = this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_TransformDataFromEntity = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_PositionDataFromEntity = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_MovingDataFromEntity = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_WatercraftDataFromEntity = this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabWatercraftData = this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSideEffectData = this.__TypeHandle.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies2),
        m_LaneObjectBuffer = this.m_LaneObjectUpdater.Begin(Allocator.TempJob),
        m_LaneReservations = nativeQueue1.AsParallelWriter(),
        m_LaneEffects = nativeQueue2.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WatercraftNavigationSystem.UpdateLaneReservationsJob jobData2 = new WatercraftNavigationSystem.UpdateLaneReservationsJob()
      {
        m_LaneReservationQueue = nativeQueue1,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Pollution_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WatercraftNavigationSystem.ApplyLaneEffectsJob jobData3 = new WatercraftNavigationSystem.ApplyLaneEffectsJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PollutionData = this.__TypeHandle.__Game_Net_Pollution_RW_ComponentLookup,
        m_LaneEffectsQueue = nativeQueue2
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<WatercraftNavigationSystem.UpdateNavigationJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      JobHandle jobHandle2 = jobData2.Schedule<WatercraftNavigationSystem.UpdateLaneReservationsJob>(jobHandle1);
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle3 = jobData3.Schedule<WatercraftNavigationSystem.ApplyLaneEffectsJob>(dependsOn);
      nativeQueue1.Dispose(jobHandle2);
      nativeQueue2.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = JobHandle.CombineDependencies(this.m_LaneObjectUpdater.Apply((SystemBase) this, jobHandle1), jobHandle2, jobHandle3);
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
    public WatercraftNavigationSystem()
    {
    }

    [BurstCompile]
    private struct UpdateNavigationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Moving> m_MovingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<Watercraft> m_WatercraftType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<WatercraftNavigation> m_NavigationType;
      public ComponentTypeHandle<WatercraftCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Blocker> m_BlockerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<WatercraftNavigationLane> m_NavigationLaneType;
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Watercraft> m_WatercraftDataFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<WatercraftData> m_PrefabWatercraftData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<VehicleSideEffectData> m_PrefabSideEffectData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;
      public NativeQueue<WatercraftNavigationHelpers.LaneReservation>.ParallelWriter m_LaneReservations;
      public NativeQueue<WatercraftNavigationHelpers.LaneEffects>.ParallelWriter m_LaneEffects;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray1 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray2 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WatercraftCurrentLane> nativeArray3 = chunk.GetNativeArray<WatercraftCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray4 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Transform> nativeArray5 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Common.Target> nativeArray6 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Watercraft> nativeArray7 = chunk.GetNativeArray<Watercraft>(ref this.m_WatercraftType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<WatercraftNavigation> nativeArray8 = chunk.GetNativeArray<WatercraftNavigation>(ref this.m_NavigationType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Odometer> nativeArray9 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray11 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<WatercraftNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<WatercraftNavigationLane>(ref this.m_NavigationLaneType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<PathElement> bufferAccessor2 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          WatercraftLaneSelectBuffer laneSelectBuffer = new WatercraftLaneSelectBuffer();
          bool flag = nativeArray9.Length != 0;
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray4[index];
            Game.Objects.Transform transform = nativeArray5[index];
            Moving moving = nativeArray1[index];
            Game.Common.Target target = nativeArray6[index];
            Watercraft watercraft = nativeArray7[index];
            WatercraftNavigation navigation = nativeArray8[index];
            WatercraftCurrentLane watercraftCurrentLane = nativeArray3[index];
            Blocker blocker = nativeArray2[index];
            PathOwner pathOwner = nativeArray10[index];
            PrefabRef prefabRefData = nativeArray11[index];
            DynamicBuffer<WatercraftNavigationLane> navigationLanes = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            WatercraftData prefabWatercraftData = this.m_PrefabWatercraftData[prefabRefData.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRefData.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WatercraftNavigationHelpers.CurrentLaneCache currentLaneCache = new WatercraftNavigationHelpers.CurrentLaneCache(ref watercraftCurrentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
            int priority = VehicleUtils.GetPriority(prefabWatercraftData);
            Odometer odometer = new Odometer();
            if (flag)
              odometer = nativeArray9[index];
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationLanes(ref random, priority, entity, transform, target, watercraft, ref laneSelectBuffer, ref watercraftCurrentLane, ref blocker, ref pathOwner, navigationLanes, pathElements);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationTarget(priority, entity, transform, moving, watercraft, prefabRefData, prefabWatercraftData, objectGeometryData, ref navigation, ref watercraftCurrentLane, ref blocker, ref odometer, navigationLanes);
            // ISSUE: reference to a compiler-generated method
            this.ReserveNavigationLanes(priority, prefabWatercraftData, watercraft, ref navigation, ref watercraftCurrentLane, navigationLanes);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref watercraftCurrentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            nativeArray8[index] = navigation;
            nativeArray3[index] = watercraftCurrentLane;
            nativeArray10[index] = pathOwner;
            nativeArray2[index] = blocker;
            if (flag)
              nativeArray9[index] = odometer;
          }
          laneSelectBuffer.Dispose();
        }
        else
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            WatercraftCurrentLane watercraftCurrentLane = nativeArray3[index];
            Blocker blocker = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((watercraftCurrentLane.m_LaneFlags & WatercraftLaneFlags.QueueReached) != (WatercraftLaneFlags) 0 && (!this.m_WatercraftDataFromEntity.HasComponent(blocker.m_Blocker) || (this.m_WatercraftDataFromEntity[blocker.m_Blocker].m_Flags & WatercraftFlags.Queueing) == (WatercraftFlags) 0))
            {
              watercraftCurrentLane.m_LaneFlags &= ~WatercraftLaneFlags.QueueReached;
              blocker = new Blocker();
            }
            nativeArray3[index] = watercraftCurrentLane;
            nativeArray2[index] = blocker;
          }
        }
      }

      private void UpdateNavigationLanes(
        ref Unity.Mathematics.Random random,
        int priority,
        Entity entity,
        Game.Objects.Transform transform,
        Game.Common.Target target,
        Watercraft watercraft,
        ref WatercraftLaneSelectBuffer laneSelectBuffer,
        ref WatercraftCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements)
      {
        int invalidPath = 10000000;
        if (currentLane.m_Lane == Entity.Null || (currentLane.m_LaneFlags & WatercraftLaneFlags.Obsolete) != (WatercraftLaneFlags) 0)
        {
          invalidPath = -1;
          // ISSUE: reference to a compiler-generated method
          this.TryFindCurrentLane(ref currentLane, transform);
        }
        else if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0 && (pathOwner.m_State & PathFlags.Append) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearNavigationLanes(ref currentLane, navigationLanes, invalidPath);
        }
        else if ((pathOwner.m_State & PathFlags.Updated) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.FillNavigationPaths(ref random, priority, entity, transform, target, watercraft, ref laneSelectBuffer, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements, ref invalidPath);
        }
        if (invalidPath == 10000000)
          return;
        // ISSUE: reference to a compiler-generated method
        this.ClearNavigationLanes(ref currentLane, navigationLanes, invalidPath);
        pathElements.Clear();
        pathOwner.m_ElementIndex = 0;
        pathOwner.m_State |= PathFlags.Obsolete;
      }

      private void ClearNavigationLanes(
        ref WatercraftCurrentLane currentLane,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes,
        int invalidPath)
      {
        currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
        if (invalidPath > 0)
        {
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            if ((navigationLanes[index].m_Flags & WatercraftLaneFlags.Reserved) == (WatercraftLaneFlags) 0)
            {
              invalidPath = math.min(index, invalidPath);
              break;
            }
          }
        }
        invalidPath = math.max(invalidPath, 0);
        if (invalidPath >= navigationLanes.Length)
          return;
        navigationLanes.RemoveRange(invalidPath, navigationLanes.Length - invalidPath);
      }

      private void TryFindCurrentLane(
        ref WatercraftCurrentLane currentLaneData,
        Game.Objects.Transform transformData)
      {
        currentLaneData.m_LaneFlags &= ~WatercraftLaneFlags.Obsolete;
        currentLaneData.m_Lane = Entity.Null;
        currentLaneData.m_ChangeLane = Entity.Null;
        float3 position = transformData.m_Position;
        float num = 100f;
        Bounds3 bounds3 = new Bounds3(position - num, position + num);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WatercraftNavigationHelpers.FindLaneIterator iterator = new WatercraftNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = position,
          m_MinDistance = num,
          m_Result = currentLaneData,
          m_CarType = RoadTypes.Watercraft,
          m_SubLanes = this.m_Lanes,
          m_CarLaneData = this.m_CarLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_MasterLaneData = this.m_MasterLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCarLaneData = this.m_PrefabCarLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<WatercraftNavigationHelpers.FindLaneIterator>(ref iterator);
        currentLaneData = iterator.m_Result;
      }

      private void FillNavigationPaths(
        ref Unity.Mathematics.Random random,
        int priority,
        Entity entity,
        Game.Objects.Transform transform,
        Game.Common.Target target,
        Watercraft watercraft,
        ref WatercraftLaneSelectBuffer laneSelectBuffer,
        ref WatercraftCurrentLane currentLaneData,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements,
        ref int invalidPath)
      {
        if ((currentLaneData.m_LaneFlags & WatercraftLaneFlags.EndOfPath) == (WatercraftLaneFlags) 0)
        {
          for (int index = 0; index < 8; ++index)
          {
            if (index >= navigationLanes.Length)
            {
              index = navigationLanes.Length;
              if (pathOwner.m_ElementIndex >= pathElements.Length)
              {
                if ((pathOwner.m_State & PathFlags.Pending) == (PathFlags) 0)
                {
                  WatercraftNavigationLane navLaneData = new WatercraftNavigationLane();
                  if (index > 0)
                  {
                    WatercraftNavigationLane navigationLane = navigationLanes[index - 1];
                    // ISSUE: reference to a compiler-generated method
                    if ((navigationLane.m_Flags & WatercraftLaneFlags.TransformTarget) == (WatercraftLaneFlags) 0 && (watercraft.m_Flags & (WatercraftFlags.StayOnWaterway | WatercraftFlags.AnyLaneTarget)) != (WatercraftFlags.StayOnWaterway | WatercraftFlags.AnyLaneTarget) && this.GetTransformTarget(ref navLaneData.m_Lane, target))
                    {
                      if ((navigationLane.m_Flags & WatercraftLaneFlags.GroupTarget) == (WatercraftLaneFlags) 0)
                      {
                        Entity lane = navLaneData.m_Lane;
                        navLaneData.m_Lane = navigationLane.m_Lane;
                        navLaneData.m_CurvePosition = navigationLane.m_CurvePosition.yy;
                        float3 position = new float3();
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (VehicleUtils.CalculateTransformPosition(ref position, lane, this.m_TransformDataFromEntity, this.m_PositionDataFromEntity, this.m_PrefabRefData, this.m_PrefabBuildingData))
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.UpdateSlaveLane(ref navLaneData, position);
                          // ISSUE: reference to a compiler-generated method
                          this.Align(ref navLaneData, position);
                        }
                        if ((watercraft.m_Flags & WatercraftFlags.StayOnWaterway) != (WatercraftFlags) 0)
                        {
                          navLaneData.m_Flags |= WatercraftLaneFlags.EndOfPath | WatercraftLaneFlags.GroupTarget;
                          navigationLanes.Add(navLaneData);
                          currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                          break;
                        }
                        navLaneData.m_Flags |= WatercraftLaneFlags.GroupTarget;
                        navigationLanes.Add(navLaneData);
                        currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                      }
                      else
                      {
                        navLaneData.m_Flags |= WatercraftLaneFlags.EndOfPath | WatercraftLaneFlags.TransformTarget;
                        navigationLanes.Add(navLaneData);
                        currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                        break;
                      }
                    }
                    else
                    {
                      navigationLane.m_Flags |= WatercraftLaneFlags.EndOfPath;
                      navigationLanes[index - 1] = navigationLane;
                      currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                      break;
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    if ((currentLaneData.m_LaneFlags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0 || (watercraft.m_Flags & WatercraftFlags.StayOnWaterway) != (WatercraftFlags) 0 || !this.GetTransformTarget(ref navLaneData.m_Lane, target))
                    {
                      currentLaneData.m_LaneFlags |= WatercraftLaneFlags.EndOfPath;
                      break;
                    }
                    navLaneData.m_Flags |= WatercraftLaneFlags.EndOfPath | WatercraftLaneFlags.TransformTarget;
                    navigationLanes.Add(navLaneData);
                    currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                    break;
                  }
                }
                else
                  break;
              }
              else
              {
                PathElement pathElement = pathElements[pathOwner.m_ElementIndex++];
                WatercraftNavigationLane navLaneData = new WatercraftNavigationLane();
                navLaneData.m_Lane = pathElement.m_Target;
                navLaneData.m_CurvePosition = pathElement.m_TargetDelta;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_CarLaneData.HasComponent(navLaneData.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ConnectionLaneData.HasComponent(navLaneData.m_Lane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[navLaneData.m_Lane];
                    navLaneData.m_Flags |= WatercraftLaneFlags.FixedLane | WatercraftLaneFlags.Connection;
                    currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                    navigationLanes.Add(navLaneData);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_SpawnLocationData.HasComponent(navLaneData.m_Lane))
                    {
                      if (pathOwner.m_ElementIndex >= pathElements.Length && (pathOwner.m_State & PathFlags.Pending) != (PathFlags) 0)
                      {
                        --pathOwner.m_ElementIndex;
                        break;
                      }
                      if ((watercraft.m_Flags & WatercraftFlags.StayOnWaterway) == (WatercraftFlags) 0 || pathElements.Length > pathOwner.m_ElementIndex)
                      {
                        navLaneData.m_Flags |= WatercraftLaneFlags.TransformTarget;
                        navigationLanes.Add(navLaneData);
                        if (index > 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          float3 position = this.m_TransformDataFromEntity[navLaneData.m_Lane].m_Position;
                          WatercraftNavigationLane navigationLane = navigationLanes[index - 1];
                          // ISSUE: reference to a compiler-generated method
                          this.UpdateSlaveLane(ref navigationLane, position);
                          navigationLanes[index - 1] = navigationLane;
                        }
                        currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                      }
                    }
                    else
                    {
                      invalidPath = index;
                      return;
                    }
                  }
                }
                else
                {
                  navLaneData.m_Flags |= WatercraftLaneFlags.UpdateOptimalLane;
                  currentLaneData.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                  if (index == 0 && (currentLaneData.m_LaneFlags & (WatercraftLaneFlags.FixedLane | WatercraftLaneFlags.Connection)) == WatercraftLaneFlags.FixedLane)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GetSlaveLaneFromMasterLane(ref random, ref navLaneData, currentLaneData);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GetSlaveLaneFromMasterLane(ref random, ref navLaneData);
                  }
                  navigationLanes.Add(navLaneData);
                }
              }
            }
            else
            {
              WatercraftNavigationLane navigationLane = navigationLanes[index];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
              {
                invalidPath = index;
                return;
              }
              if ((navigationLane.m_Flags & WatercraftLaneFlags.EndOfPath) != (WatercraftLaneFlags) 0)
                break;
            }
          }
        }
        if ((currentLaneData.m_LaneFlags & WatercraftLaneFlags.UpdateOptimalLane) == (WatercraftLaneFlags) 0)
          return;
        currentLaneData.m_LaneFlags &= ~WatercraftLaneFlags.UpdateOptimalLane;
        if ((currentLaneData.m_LaneFlags & WatercraftLaneFlags.IsBlocked) != (WatercraftLaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.IsBlockedLane(currentLaneData.m_Lane, currentLaneData.m_CurvePosition.xz))
          {
            currentLaneData.m_CurvePosition.z = currentLaneData.m_CurvePosition.y;
            invalidPath = -1;
            return;
          }
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            WatercraftNavigationLane navigationLane = navigationLanes[index];
            // ISSUE: reference to a compiler-generated method
            if (this.IsBlockedLane(navigationLane.m_Lane, navigationLane.m_CurvePosition))
            {
              currentLaneData.m_CurvePosition.z = currentLaneData.m_CurvePosition.y;
              invalidPath = index;
              return;
            }
          }
          currentLaneData.m_LaneFlags &= ~(WatercraftLaneFlags.FixedLane | WatercraftLaneFlags.IsBlocked);
          currentLaneData.m_LaneFlags |= WatercraftLaneFlags.IgnoreBlocker;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WatercraftLaneSelectIterator laneSelectIterator = new WatercraftLaneSelectIterator()
        {
          m_OwnerData = this.m_OwnerData,
          m_LaneData = this.m_LaneData,
          m_SlaveLaneData = this.m_SlaveLaneData,
          m_LaneReservationData = this.m_LaneReservationData,
          m_MovingData = this.m_MovingDataFromEntity,
          m_WatercraftData = this.m_WatercraftDataFromEntity,
          m_Lanes = this.m_Lanes,
          m_LaneObjects = this.m_LaneObjects,
          m_Entity = entity,
          m_Blocker = blocker.m_Blocker,
          m_Priority = priority
        };
        laneSelectIterator.SetBuffer(ref laneSelectBuffer);
        if (navigationLanes.Length != 0)
        {
          WatercraftNavigationLane watercraftNavigationLane = navigationLanes[navigationLanes.Length - 1];
          laneSelectIterator.CalculateLaneCosts(watercraftNavigationLane, navigationLanes.Length - 1);
          for (int index = navigationLanes.Length - 2; index >= 0; --index)
          {
            WatercraftNavigationLane navigationLane = navigationLanes[index];
            laneSelectIterator.CalculateLaneCosts(navigationLane, watercraftNavigationLane, index);
            watercraftNavigationLane = navigationLane;
          }
          laneSelectIterator.UpdateOptimalLane(ref currentLaneData, navigationLanes[0]);
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            WatercraftNavigationLane navigationLane = navigationLanes[index];
            laneSelectIterator.UpdateOptimalLane(ref navigationLane);
            navigationLane.m_Flags &= ~WatercraftLaneFlags.Reserved;
            navigationLanes[index] = navigationLane;
          }
        }
        else
        {
          if ((double) currentLaneData.m_CurvePosition.x == (double) currentLaneData.m_CurvePosition.z)
            return;
          laneSelectIterator.UpdateOptimalLane(ref currentLaneData, new WatercraftNavigationLane());
        }
      }

      private bool IsBlockedLane(Entity lane, float2 range)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLaneData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          SlaveLane slaveLane = this.m_SlaveLaneData[lane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane1 = this.m_Lanes[this.m_OwnerData[lane].m_Owner];
          int index = (int) slaveLane.m_MinIndex - 1;
          if (index < 0 || index > lane1.Length)
            return false;
          lane = lane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_MasterLaneData.HasComponent(lane))
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(lane))
          return false;
        // ISSUE: reference to a compiler-generated field
        Game.Net.CarLane carLane = this.m_CarLaneData[lane];
        return (int) carLane.m_BlockageEnd >= (int) carLane.m_BlockageStart && (double) math.min(range.x, range.y) <= (double) carLane.m_BlockageEnd * 0.0039215688593685627 && (double) math.max(range.x, range.y) >= (double) carLane.m_BlockageStart * 0.0039215688593685627;
      }

      private bool GetTransformTarget(ref Entity entity, Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PropertyRenterData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          target.m_Target = this.m_PropertyRenterData[target.m_Target].m_Property;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformDataFromEntity.HasComponent(target.m_Target))
        {
          entity = target.m_Target;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PositionDataFromEntity.HasComponent(target.m_Target))
          return false;
        entity = target.m_Target;
        return true;
      }

      private void UpdateSlaveLane(ref WatercraftNavigationLane navLaneData, float3 targetPosition)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          SlaveLane slaveLane = this.m_SlaveLaneData[navLaneData.m_Lane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
          // ISSUE: reference to a compiler-generated field
          int index = NetUtils.ChooseClosestLane((int) slaveLane.m_MinIndex, (int) slaveLane.m_MaxIndex, targetPosition, lane, this.m_CurveData, navLaneData.m_CurvePosition.y);
          navLaneData.m_Lane = lane[index].m_SubLane;
        }
        navLaneData.m_Flags |= WatercraftLaneFlags.FixedLane;
      }

      private void Align(ref WatercraftNavigationLane navLaneData, float3 targetPosition)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurveData.HasComponent(navLaneData.m_Lane))
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[navLaneData.m_Lane];
        float3 float3 = MathUtils.Position(curve.m_Bezier, navLaneData.m_CurvePosition.y);
        if ((double) math.dot(MathUtils.Right(MathUtils.Tangent(curve.m_Bezier, navLaneData.m_CurvePosition.y).xz), targetPosition.xz - float3.xz) > 0.0)
          navLaneData.m_Flags |= WatercraftLaneFlags.AlignRight;
        else
          navLaneData.m_Flags |= WatercraftLaneFlags.AlignLeft;
      }

      private void GetSlaveLaneFromMasterLane(
        ref Unity.Mathematics.Random random,
        ref WatercraftNavigationLane navLaneData,
        WatercraftCurrentLane currentLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_MasterLaneData.HasComponent(navLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          MasterLane masterLane = this.m_MasterLaneData[navLaneData.m_Lane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
          if ((currentLaneData.m_LaneFlags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0)
          {
            float3 position = new float3();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (VehicleUtils.CalculateTransformPosition(ref position, currentLaneData.m_Lane, this.m_TransformDataFromEntity, this.m_PositionDataFromEntity, this.m_PrefabRefData, this.m_PrefabBuildingData))
            {
              // ISSUE: reference to a compiler-generated field
              int index = NetUtils.ChooseClosestLane((int) masterLane.m_MinIndex, (int) masterLane.m_MaxIndex, position, lane, this.m_CurveData, navLaneData.m_CurvePosition.y);
              navLaneData.m_Lane = lane[index].m_SubLane;
              navLaneData.m_Flags |= WatercraftLaneFlags.FixedStart;
            }
            else
            {
              int index = random.NextInt((int) masterLane.m_MinIndex, (int) masterLane.m_MaxIndex + 1);
              navLaneData.m_Lane = lane[index].m_SubLane;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            float3 comparePosition = MathUtils.Position(this.m_CurveData[currentLaneData.m_Lane].m_Bezier, currentLaneData.m_CurvePosition.z);
            // ISSUE: reference to a compiler-generated field
            int index = NetUtils.ChooseClosestLane((int) masterLane.m_MinIndex, (int) masterLane.m_MaxIndex, comparePosition, lane, this.m_CurveData, navLaneData.m_CurvePosition.x);
            navLaneData.m_Lane = lane[index].m_SubLane;
            navLaneData.m_Flags |= WatercraftLaneFlags.FixedStart;
          }
        }
        else
          navLaneData.m_Flags |= WatercraftLaneFlags.FixedLane;
      }

      private void GetSlaveLaneFromMasterLane(
        ref Unity.Mathematics.Random random,
        ref WatercraftNavigationLane navLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_MasterLaneData.HasComponent(navLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          MasterLane masterLane = this.m_MasterLaneData[navLaneData.m_Lane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
          int index = random.NextInt((int) masterLane.m_MinIndex, (int) masterLane.m_MaxIndex + 1);
          navLaneData.m_Lane = lane[index].m_SubLane;
        }
        else
          navLaneData.m_Flags |= WatercraftLaneFlags.FixedLane;
      }

      private void CheckBlocker(
        Watercraft watercraftData,
        ref WatercraftCurrentLane currentLane,
        ref Blocker blocker,
        ref WatercraftLaneSpeedIterator laneIterator)
      {
        if (laneIterator.m_Blocker != blocker.m_Blocker)
          currentLane.m_LaneFlags &= ~(WatercraftLaneFlags.IgnoreBlocker | WatercraftLaneFlags.QueueReached);
        if (laneIterator.m_Blocker != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_MovingDataFromEntity.HasComponent(laneIterator.m_Blocker))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_WatercraftDataFromEntity.HasComponent(laneIterator.m_Blocker))
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_WatercraftDataFromEntity[laneIterator.m_Blocker].m_Flags & WatercraftFlags.Queueing) != (WatercraftFlags) 0 && (currentLane.m_LaneFlags & WatercraftLaneFlags.Queue) != (WatercraftLaneFlags) 0)
              {
                if ((double) laneIterator.m_MaxSpeed < 1.0)
                  currentLane.m_LaneFlags |= WatercraftLaneFlags.QueueReached;
              }
              else
              {
                currentLane.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
                if ((double) laneIterator.m_MaxSpeed < 1.0)
                  currentLane.m_LaneFlags |= WatercraftLaneFlags.IsBlocked;
              }
            }
            else
            {
              currentLane.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
              if ((double) laneIterator.m_MaxSpeed < 1.0)
                currentLane.m_LaneFlags |= WatercraftLaneFlags.IsBlocked;
            }
          }
          else if (laneIterator.m_Blocker != blocker.m_Blocker)
            currentLane.m_LaneFlags |= WatercraftLaneFlags.UpdateOptimalLane;
        }
        blocker.m_Blocker = laneIterator.m_Blocker;
        blocker.m_Type = laneIterator.m_BlockerType;
        blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(laneIterator.m_MaxSpeed * 4.58999968f), 0, (int) byte.MaxValue);
      }

      private void UpdateNavigationTarget(
        int priority,
        Entity entity,
        Game.Objects.Transform transform,
        Moving moving,
        Watercraft watercraft,
        PrefabRef prefabRefData,
        WatercraftData prefabWatercraftData,
        ObjectGeometryData prefabObjectGeometryData,
        ref WatercraftNavigation navigation,
        ref WatercraftCurrentLane currentLane,
        ref Blocker blocker,
        ref Odometer odometer,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes)
      {
        float timeStep = 0.266666681f;
        float num1 = math.length(moving.m_Velocity.xz);
        float speedLimitFactor = 1f;
        if ((currentLane.m_LaneFlags & WatercraftLaneFlags.Connection) != (WatercraftLaneFlags) 0)
        {
          prefabWatercraftData.m_MaxSpeed = 277.777771f;
          prefabWatercraftData.m_Acceleration = 277.777771f;
          prefabWatercraftData.m_Braking = 277.777771f;
        }
        else
          num1 = math.min(num1, prefabWatercraftData.m_MaxSpeed);
        Bounds1 bounds1 = (currentLane.m_LaneFlags & (WatercraftLaneFlags.ResetSpeed | WatercraftLaneFlags.Connection)) == (WatercraftLaneFlags) 0 ? VehicleUtils.CalculateSpeedRange(prefabWatercraftData, num1, timeStep) : new Bounds1(0.0f, prefabWatercraftData.m_MaxSpeed);
        float3 pivot1;
        float3 pivot2;
        VehicleUtils.CalculateShipNavigationPivots(transform, prefabObjectGeometryData, out pivot1, out pivot2);
        float num2 = math.distance(pivot1.xz, pivot2.xz);
        float3 position = transform.m_Position;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurveData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[currentLane.m_Lane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetLaneData netLaneData = this.m_PrefabNetLaneData[this.m_PrefabRefData[currentLane.m_Lane].m_Prefab];
          float2 xz = MathUtils.Tangent(curve.m_Bezier, currentLane.m_CurvePosition.x).xz;
          if (MathUtils.TryNormalize(ref xz))
            position.xz -= MathUtils.Right(xz) * (float) (((double) netLaneData.m_Width - (double) prefabObjectGeometryData.m_Size.x) * (double) currentLane.m_LanePosition * 0.5);
        }
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
        WatercraftLaneSpeedIterator laneIterator = new WatercraftLaneSpeedIterator()
        {
          m_TransformData = this.m_TransformDataFromEntity,
          m_MovingData = this.m_MovingDataFromEntity,
          m_WatercraftData = this.m_WatercraftDataFromEntity,
          m_LaneReservationData = this.m_LaneReservationData,
          m_CurveData = this.m_CurveData,
          m_CarLaneData = this.m_CarLaneData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
          m_PrefabWatercraftData = this.m_PrefabWatercraftData,
          m_LaneOverlapData = this.m_LaneOverlaps,
          m_LaneObjectData = this.m_LaneObjects,
          m_Entity = entity,
          m_Ignore = (currentLane.m_LaneFlags & WatercraftLaneFlags.IgnoreBlocker) != (WatercraftLaneFlags) 0 ? blocker.m_Blocker : Entity.Null,
          m_Priority = priority,
          m_TimeStep = timeStep,
          m_SafeTimeStep = timeStep + 0.5f,
          m_SpeedLimitFactor = speedLimitFactor,
          m_PrefabWatercraft = prefabWatercraftData,
          m_PrefabObjectGeometry = prefabObjectGeometryData,
          m_SpeedRange = bounds1,
          m_MaxSpeed = bounds1.max,
          m_CanChangeLane = 1f,
          m_CurrentPosition = position
        };
        if ((currentLane.m_LaneFlags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0)
        {
          laneIterator.IterateTarget(navigation.m_TargetPosition);
          navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
        }
        else
        {
          if (currentLane.m_Lane == Entity.Null)
          {
            navigation.m_MaxSpeed = math.max(0.0f, num1 - prefabWatercraftData.m_Braking * timeStep);
            blocker.m_Blocker = Entity.Null;
            blocker.m_Type = BlockerType.None;
            blocker.m_MaxSpeed = byte.MaxValue;
            return;
          }
          if (currentLane.m_ChangeLane != Entity.Null)
          {
            if (laneIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_ChangeLane, currentLane.m_CurvePosition, currentLane.m_ChangeProgress))
              goto label_24;
          }
          else if (laneIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_CurvePosition))
            goto label_24;
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            WatercraftNavigationLane navigationLane = navigationLanes[index];
            if ((navigationLane.m_Flags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.CalculateTransformPosition(ref laneIterator.m_CurrentPosition, navigationLane.m_Lane, this.m_TransformDataFromEntity, this.m_PositionDataFromEntity, this.m_PrefabRefData, this.m_PrefabBuildingData);
              break;
            }
            if ((navigationLane.m_Flags & WatercraftLaneFlags.Connection) != (WatercraftLaneFlags) 0)
            {
              laneIterator.m_PrefabWatercraft.m_MaxSpeed = 277.777771f;
              laneIterator.m_PrefabWatercraft.m_Acceleration = 277.777771f;
              laneIterator.m_PrefabWatercraft.m_Braking = 277.777771f;
              laneIterator.m_SpeedRange = new Bounds1(0.0f, 277.777771f);
            }
            else if ((currentLane.m_LaneFlags & WatercraftLaneFlags.Connection) != (WatercraftLaneFlags) 0)
              break;
            bool c = navigationLane.m_Lane == currentLane.m_Lane | navigationLane.m_Lane == currentLane.m_ChangeLane;
            float minOffset = math.select(-1f, currentLane.m_CurvePosition.y, c);
            if (laneIterator.IterateNextLane(navigationLane.m_Lane, navigationLane.m_CurvePosition, minOffset))
              goto label_24;
          }
          laneIterator.IterateTarget(laneIterator.m_CurrentPosition);
label_24:
          navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
          // ISSUE: reference to a compiler-generated method
          this.CheckBlocker(watercraft, ref currentLane, ref blocker, ref laneIterator);
        }
        float3 float3 = navigation.m_TargetPosition - transform.m_Position;
        double num3 = (double) math.length(float3.xz);
        float minDistance = (float) ((double) navigation.m_MaxSpeed * (double) timeStep + 1.0 + (double) num2 * 0.5);
        if (currentLane.m_ChangeLane != Entity.Null)
        {
          float num4 = 0.05f;
          float x1 = (float) (1.0 + (double) prefabObjectGeometryData.m_Size.z * (double) num4 * 0.5);
          float2 x2 = new float2(0.4f, 0.6f * math.saturate(num1 * num4)) * (laneIterator.m_CanChangeLane * timeStep);
          x2.x = math.min(x2.x, math.max(0.0f, 1f - currentLane.m_ChangeProgress));
          currentLane.m_ChangeProgress = math.min(x1, currentLane.m_ChangeProgress + math.csum(x2));
          if ((double) currentLane.m_ChangeProgress == (double) x1)
          {
            // ISSUE: reference to a compiler-generated method
            this.ApplySideEffects(ref currentLane, speedLimitFactor, prefabRefData, prefabWatercraftData);
            currentLane.m_Lane = currentLane.m_ChangeLane;
            currentLane.m_ChangeLane = Entity.Null;
          }
        }
        currentLane.m_Duration += timeStep;
        currentLane.m_Distance += num1 * timeStep;
        odometer.m_Distance += num1 * timeStep;
        double num5 = (double) minDistance;
        if (num3 >= num5)
          return;
        while (true)
        {
          if ((currentLane.m_LaneFlags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0)
          {
            navigation.m_TargetDirection = new float3();
            // ISSUE: reference to a compiler-generated method
            if (this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, currentLane.m_Lane))
              break;
          }
          else
          {
            navigation.m_TargetDirection = new float3();
            if (currentLane.m_ChangeLane != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve1 = this.m_CurveData[currentLane.m_Lane];
              // ISSUE: reference to a compiler-generated field
              Curve curve2 = this.m_CurveData[currentLane.m_ChangeLane];
              // ISSUE: reference to a compiler-generated method
              if (this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, curve1.m_Bezier, curve2.m_Bezier, currentLane.m_ChangeProgress, ref currentLane.m_CurvePosition))
                goto label_34;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[currentLane.m_Lane];
              // ISSUE: reference to a compiler-generated method
              if (this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, curve.m_Bezier, ref currentLane.m_CurvePosition))
                goto label_36;
            }
          }
          if (navigationLanes.Length != 0)
          {
            WatercraftNavigationLane navigationLane = navigationLanes[0];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
            {
              if ((currentLane.m_LaneFlags & WatercraftLaneFlags.Connection) != (WatercraftLaneFlags) 0)
              {
                if ((navigationLane.m_Flags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0)
                  navigationLane.m_Flags |= WatercraftLaneFlags.ResetSpeed;
                else if ((navigationLane.m_Flags & WatercraftLaneFlags.Connection) == (WatercraftLaneFlags) 0)
                {
                  float3 = navigation.m_TargetPosition - transform.m_Position;
                  if ((double) math.length(float3.xz) < 1.0 && (double) num1 <= 3.0)
                    navigationLane.m_Flags |= WatercraftLaneFlags.ResetSpeed;
                  else
                    goto label_52;
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.ApplySideEffects(ref currentLane, speedLimitFactor, prefabRefData, prefabWatercraftData);
              currentLane.m_Lane = navigationLane.m_Lane;
              currentLane.m_ChangeLane = Entity.Null;
              currentLane.m_ChangeProgress = 0.0f;
              currentLane.m_CurvePosition = navigationLane.m_CurvePosition.xxy;
              currentLane.m_LaneFlags = navigationLane.m_Flags;
              currentLane.m_LanePosition = (currentLane.m_LaneFlags & (WatercraftLaneFlags.AlignLeft | WatercraftLaneFlags.AlignRight)) == (WatercraftLaneFlags) 0 ? 0.0f : math.select(-1f, 1f, (currentLane.m_LaneFlags & WatercraftLaneFlags.AlignRight) > (WatercraftLaneFlags) 0);
              navigationLanes.RemoveAt(0);
            }
            else
              goto label_51;
          }
          else
            goto label_38;
        }
        return;
label_34:
        // ISSUE: reference to a compiler-generated method
        this.ApplyLanePosition(ref navigation.m_TargetPosition, ref currentLane, prefabObjectGeometryData);
        return;
label_36:
        // ISSUE: reference to a compiler-generated method
        this.ApplyLanePosition(ref navigation.m_TargetPosition, ref currentLane, prefabObjectGeometryData);
        return;
label_38:
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurveData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[currentLane.m_Lane];
          navigation.m_TargetDirection = MathUtils.Tangent(curve.m_Bezier, currentLane.m_CurvePosition.z);
          // ISSUE: reference to a compiler-generated method
          this.ApplyLanePosition(ref navigation.m_TargetPosition, ref currentLane, prefabObjectGeometryData);
        }
        float3 = navigation.m_TargetPosition - transform.m_Position;
        if ((double) math.length(float3.xz) >= 1.0 || (double) num1 >= 0.10000000149011612)
          return;
        currentLane.m_LaneFlags |= WatercraftLaneFlags.EndReached;
        return;
label_51:
        return;
label_52:;
      }

      private void ApplyLanePosition(
        ref float3 targetPosition,
        ref WatercraftCurrentLane currentLaneData,
        ObjectGeometryData prefabObjectGeometryData)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurveData.HasComponent(currentLaneData.m_Lane))
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[currentLaneData.m_Lane];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetLaneData netLaneData = this.m_PrefabNetLaneData[this.m_PrefabRefData[currentLaneData.m_Lane].m_Prefab];
        float2 xz = MathUtils.Tangent(curve.m_Bezier, currentLaneData.m_CurvePosition.x).xz;
        if (!MathUtils.TryNormalize(ref xz))
          return;
        targetPosition.xz += MathUtils.Right(xz) * (float) (((double) netLaneData.m_Width - (double) prefabObjectGeometryData.m_Size.x) * (double) currentLaneData.m_LanePosition * 0.5);
      }

      private void ApplySideEffects(
        ref WatercraftCurrentLane currentLaneData,
        float speedLimitFactor,
        PrefabRef prefabRefData,
        WatercraftData prefabWatercraftData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.CarLane carLaneData = this.m_CarLaneData[currentLaneData.m_Lane];
          carLaneData.m_SpeedLimit *= speedLimitFactor;
          float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(prefabWatercraftData, carLaneData);
          float num1 = math.select(currentLaneData.m_Distance / currentLaneData.m_Duration, maxDriveSpeed, (double) currentLaneData.m_Duration == 0.0);
          float relativeSpeed = num1 / maxDriveSpeed;
          float3 sideEffects = new float3();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSideEffectData.HasComponent(prefabRefData.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleSideEffectData vehicleSideEffectData = this.m_PrefabSideEffectData[prefabRefData.m_Prefab];
            float num2 = num1 / prefabWatercraftData.m_MaxSpeed;
            float s = math.saturate(num2 * num2);
            sideEffects = math.lerp(vehicleSideEffectData.m_Min, vehicleSideEffectData.m_Max, s) * new float3(currentLaneData.m_Distance, currentLaneData.m_Duration, currentLaneData.m_Duration);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_LaneEffects.Enqueue(new WatercraftNavigationHelpers.LaneEffects(currentLaneData.m_Lane, sideEffects, relativeSpeed));
        }
        currentLaneData.m_Duration = 0.0f;
        currentLaneData.m_Distance = 0.0f;
      }

      private void ReserveNavigationLanes(
        int priority,
        WatercraftData prefabWatercraftData,
        Watercraft watercraftData,
        ref WatercraftNavigation navigationData,
        ref WatercraftCurrentLane currentLaneData,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes)
      {
        float timeStep = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[currentLaneData.m_Lane];
        float num1 = math.max(0.0f, VehicleUtils.GetBrakingDistance(prefabWatercraftData, navigationData.m_MaxSpeed, timeStep) - 0.01f);
        currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.x + num1 / math.max(1E-06f, curve1.m_Length);
        float num2 = num1 - curve1.m_Length * math.abs(currentLaneData.m_CurvePosition.z - currentLaneData.m_CurvePosition.x);
        int index = 0;
        if ((double) currentLaneData.m_CurvePosition.y <= (double) currentLaneData.m_CurvePosition.z)
          return;
        currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.z;
        WatercraftNavigationLane navigationLane;
        for (; index < navigationLanes.Length && (double) num2 > 0.0; navigationLanes[index++] = navigationLane)
        {
          navigationLane = navigationLanes[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CarLaneData.HasComponent(navigationLane.m_Lane))
            break;
          // ISSUE: reference to a compiler-generated field
          Curve curve2 = this.m_CurveData[navigationLane.m_Lane];
          float offset = math.min(navigationLane.m_CurvePosition.y, navigationLane.m_CurvePosition.x + num2 / math.max(1E-06f, curve2.m_Length));
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneReservationData.HasComponent(navigationLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneReservations.Enqueue(new WatercraftNavigationHelpers.LaneReservation(navigationLane.m_Lane, offset, priority));
          }
          num2 -= curve2.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
          navigationLane.m_Flags |= WatercraftLaneFlags.Reserved;
        }
      }

      private void ReserveOtherLanesInGroup(Entity lane, int priority)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SlaveLaneData.HasComponent(lane))
          return;
        // ISSUE: reference to a compiler-generated field
        SlaveLane slaveLane = this.m_SlaveLaneData[lane];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> lane1 = this.m_Lanes[this.m_OwnerData[lane].m_Owner];
        int num = math.min((int) slaveLane.m_MaxIndex, lane1.Length - 1);
        for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= num; ++minIndex)
        {
          Entity subLane = lane1[minIndex].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (subLane != lane && this.m_LaneReservationData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneReservations.Enqueue(new WatercraftNavigationHelpers.LaneReservation(subLane, 0.0f, priority));
          }
        }
      }

      private bool MoveTarget(
        float3 comparePosition,
        ref float3 targetPosition,
        float minDistance,
        Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return VehicleUtils.CalculateTransformPosition(ref targetPosition, target, this.m_TransformDataFromEntity, this.m_PositionDataFromEntity, this.m_PrefabRefData, this.m_PrefabBuildingData) && (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
      }

      private bool MoveTarget(
        float3 comparePosition,
        ref float3 targetPosition,
        float minDistance,
        Bezier4x3 curve,
        ref float3 curveDelta)
      {
        float3 float3_1 = MathUtils.Position(curve, curveDelta.z);
        if ((double) math.distance(comparePosition.xz, float3_1.xz) < (double) minDistance)
        {
          curveDelta.x = curveDelta.z;
          targetPosition = float3_1;
          return false;
        }
        float2 xz = curveDelta.xz;
        for (int index = 0; index < 8; ++index)
        {
          float t = math.lerp(xz.x, xz.y, 0.5f);
          float3 float3_2 = MathUtils.Position(curve, t);
          if ((double) math.distance(comparePosition.xz, float3_2.xz) < (double) minDistance)
            xz.x = t;
          else
            xz.y = t;
        }
        curveDelta.x = xz.y;
        targetPosition = MathUtils.Position(curve, xz.y);
        return true;
      }

      private bool MoveTarget(
        float3 comparePosition,
        ref float3 targetPosition,
        float minDistance,
        Bezier4x3 curve1,
        Bezier4x3 curve2,
        float curveSelect,
        ref float3 curveDelta)
      {
        curveSelect = math.saturate(curveSelect);
        float3 float3_1 = math.lerp(MathUtils.Position(curve1, curveDelta.z), MathUtils.Position(curve2, curveDelta.z), curveSelect);
        if ((double) math.distance(comparePosition.xz, float3_1.xz) < (double) minDistance)
        {
          curveDelta.x = curveDelta.z;
          targetPosition = float3_1;
          return false;
        }
        float2 xz = curveDelta.xz;
        for (int index = 0; index < 8; ++index)
        {
          float t = math.lerp(xz.x, xz.y, 0.5f);
          float3 float3_2 = math.lerp(MathUtils.Position(curve1, t), MathUtils.Position(curve2, t), curveSelect);
          if ((double) math.distance(comparePosition.xz, float3_2.xz) < (double) minDistance)
            xz.x = t;
          else
            xz.y = t;
        }
        curveDelta.x = xz.y;
        float3 x = MathUtils.Position(curve1, xz.y);
        float3 y = MathUtils.Position(curve2, xz.y);
        targetPosition = math.lerp(x, y, curveSelect);
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
    private struct GroupLaneReservationsJob : IJob
    {
      public NativeQueue<WatercraftNavigationHelpers.LaneReservation> m_LaneReservationQueue;
      public NativeList<WatercraftNavigationHelpers.LaneReservation> m_LaneReservationList;
      public NativeList<int2> m_LaneReservationGroups;

      public void Execute()
      {
        WatercraftNavigationHelpers.LaneReservation laneReservation;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneReservationQueue.TryDequeue(out laneReservation))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LaneReservationList.Add(in laneReservation);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LaneReservationList.Sort<WatercraftNavigationHelpers.LaneReservation>();
        Entity lane = Entity.Null;
        int x = 0;
        int2 int2;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_LaneReservationList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          laneReservation = this.m_LaneReservationList[index];
          if (lane != laneReservation.m_Lane)
          {
            if (lane != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<int2> local1 = ref this.m_LaneReservationGroups;
              int2 = new int2(x, index);
              ref int2 local2 = ref int2;
              local1.Add(in local2);
            }
            lane = laneReservation.m_Lane;
            x = index;
          }
        }
        if (!(lane != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        ref NativeList<int2> local3 = ref this.m_LaneReservationGroups;
        // ISSUE: reference to a compiler-generated field
        int2 = new int2(x, this.m_LaneReservationList.Length);
        ref int2 local4 = ref int2;
        local3.Add(in local4);
      }
    }

    [BurstCompile]
    private struct UpdateLaneReservationsJob : IJob
    {
      public NativeQueue<WatercraftNavigationHelpers.LaneReservation> m_LaneReservationQueue;
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;

      public void Execute()
      {
        WatercraftNavigationHelpers.LaneReservation laneReservation;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneReservationQueue.TryDequeue(out laneReservation))
        {
          // ISSUE: reference to a compiler-generated field
          ref Game.Net.LaneReservation local = ref this.m_LaneReservationData.GetRefRW(laneReservation.m_Lane).ValueRW;
          if ((int) laneReservation.m_Offset > (int) local.m_Next.m_Offset)
            local.m_Next.m_Offset = laneReservation.m_Offset;
          if ((int) laneReservation.m_Priority > (int) local.m_Next.m_Priority)
          {
            if ((int) laneReservation.m_Priority >= (int) local.m_Prev.m_Priority)
              local.m_Blocker = Entity.Null;
            local.m_Next.m_Priority = laneReservation.m_Priority;
          }
        }
      }
    }

    [BurstCompile]
    private struct ApplyLaneEffectsJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      public ComponentLookup<Game.Net.Pollution> m_PollutionData;
      public NativeQueue<WatercraftNavigationHelpers.LaneEffects> m_LaneEffectsQueue;

      public void Execute()
      {
        WatercraftNavigationHelpers.LaneEffects laneEffects;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneEffectsQueue.TryDequeue(out laneEffects))
        {
          // ISSUE: reference to a compiler-generated field
          Entity owner = this.m_OwnerData[laneEffects.m_Lane].m_Owner;
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Watercraft> __Game_Vehicles_Watercraft_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<WatercraftNavigation> __Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<WatercraftNavigationLane> __Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Watercraft> __Game_Vehicles_Watercraft_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftData> __Game_Prefabs_WatercraftData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleSideEffectData> __Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RW_ComponentLookup;
      public ComponentLookup<Game.Net.Pollution> __Game_Net_Pollution_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Watercraft_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Watercraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<WatercraftNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Watercraft_RO_ComponentLookup = state.GetComponentLookup<Watercraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WatercraftData_RO_ComponentLookup = state.GetComponentLookup<WatercraftData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup = state.GetComponentLookup<VehicleSideEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RW_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Pollution_RW_ComponentLookup = state.GetComponentLookup<Game.Net.Pollution>();
      }
    }
  }
}
