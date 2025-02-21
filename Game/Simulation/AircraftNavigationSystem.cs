// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AircraftNavigationSystem
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
  public class AircraftNavigationSystem : GameSystemBase
  {
    private Game.Net.SearchSystem m_NetSearchSystem;
    private AirwaySystem m_AirwaySystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_VehicleQuery;
    private LaneObjectUpdater m_LaneObjectUpdater;
    private AircraftNavigationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 10;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Aircraft>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<Game.Common.Target>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PathElement>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<AircraftNavigation>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>());
      // ISSUE: reference to a compiler-generated field
      this.m_LaneObjectUpdater = new LaneObjectUpdater((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<AircraftNavigationHelpers.LaneReservation> nativeQueue1 = new NativeQueue<AircraftNavigationHelpers.LaneReservation>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<AircraftNavigationHelpers.LaneEffects> nativeQueue2 = new NativeQueue<AircraftNavigationHelpers.LaneEffects>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
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
      this.__TypeHandle.__Game_Prefabs_AirplaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AircraftData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AircraftNavigationSystem.UpdateNavigationJob jobData1 = new AircraftNavigationSystem.UpdateNavigationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_AircraftType = this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentTypeHandle,
        m_HelicopterType = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_PositionDataFromEntity = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_TakeoffLocationData = this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup,
        m_MovingDataFromEntity = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_AircraftData = this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabAircraftData = this.__TypeHandle.__Game_Prefabs_AircraftData_RO_ComponentLookup,
        m_PrefabHelicopterData = this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup,
        m_PrefabAirplaneData = this.__TypeHandle.__Game_Prefabs_AirplaneData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSideEffectData = this.__TypeHandle.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies2),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_AirwayData = this.m_AirwaySystem.GetAirwayData(),
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
      AircraftNavigationSystem.UpdateLaneReservationsJob jobData2 = new AircraftNavigationSystem.UpdateLaneReservationsJob()
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
      AircraftNavigationSystem.ApplyLaneEffectsJob jobData3 = new AircraftNavigationSystem.ApplyLaneEffectsJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PollutionData = this.__TypeHandle.__Game_Net_Pollution_RW_ComponentLookup,
        m_LaneEffectsQueue = nativeQueue2
      };
      JobHandle dependsOn1 = JobHandle.CombineDependencies(JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2), deps);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<AircraftNavigationSystem.UpdateNavigationJob>(this.m_VehicleQuery, dependsOn1);
      JobHandle jobHandle2 = jobData2.Schedule<AircraftNavigationSystem.UpdateLaneReservationsJob>(jobHandle1);
      JobHandle dependsOn2 = jobHandle1;
      JobHandle jobHandle3 = jobData3.Schedule<AircraftNavigationSystem.ApplyLaneEffectsJob>(dependsOn2);
      nativeQueue1.Dispose(jobHandle2);
      nativeQueue2.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle1);
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
    public AircraftNavigationSystem()
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
      public ComponentTypeHandle<Aircraft> m_AircraftType;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> m_HelicopterType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<AircraftNavigation> m_NavigationType;
      public ComponentTypeHandle<AircraftCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Blocker> m_BlockerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<AircraftNavigationLane> m_NavigationLaneType;
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
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> m_TakeoffLocationData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Aircraft> m_AircraftData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AircraftData> m_PrefabAircraftData;
      [ReadOnly]
      public ComponentLookup<HelicopterData> m_PrefabHelicopterData;
      [ReadOnly]
      public ComponentLookup<AirplaneData> m_PrefabAirplaneData;
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
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public AirwayHelpers.AirwayData m_AirwayData;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;
      public NativeQueue<AircraftNavigationHelpers.LaneReservation>.ParallelWriter m_LaneReservations;
      public NativeQueue<AircraftNavigationHelpers.LaneEffects>.ParallelWriter m_LaneEffects;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray2 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray3 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AircraftNavigation> nativeArray4 = chunk.GetNativeArray<AircraftNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AircraftCurrentLane> nativeArray5 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray6 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<AircraftNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<AircraftNavigationLane>(ref this.m_NavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor2 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        bool isHelicopter = chunk.Has<Helicopter>(ref this.m_HelicopterType);
        if (nativeArray3.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Common.Target> nativeArray8 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Aircraft> nativeArray9 = chunk.GetNativeArray<Aircraft>(ref this.m_AircraftType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Blocker> nativeArray10 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Odometer> nativeArray11 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
          bool flag = nativeArray11.Length != 0;
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            Moving moving = nativeArray3[index];
            Game.Common.Target target = nativeArray8[index];
            Aircraft aircraft = nativeArray9[index];
            AircraftNavigation navigation = nativeArray4[index];
            AircraftCurrentLane aircraftCurrentLane = nativeArray5[index];
            Blocker blocker = nativeArray10[index];
            PathOwner pathOwner = nativeArray6[index];
            PrefabRef prefabRefData = nativeArray7[index];
            DynamicBuffer<AircraftNavigationLane> navigationLanes = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            AircraftData prefabAircraftData = this.m_PrefabAircraftData[prefabRefData.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRefData.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            AircraftNavigationHelpers.CurrentLaneCache currentLaneCache = new AircraftNavigationHelpers.CurrentLaneCache(ref aircraftCurrentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
            int priority = VehicleUtils.GetPriority(prefabAircraftData);
            Odometer odometer = new Odometer();
            if (flag)
              odometer = nativeArray11[index];
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationLanes(priority, entity, isHelicopter, transform, target, aircraft, ref aircraftCurrentLane, ref blocker, ref pathOwner, navigationLanes, pathElements);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationTarget(priority, entity, isHelicopter, transform, moving, aircraft, prefabRefData, prefabAircraftData, objectGeometryData, ref navigation, ref aircraftCurrentLane, ref blocker, ref odometer, navigationLanes);
            // ISSUE: reference to a compiler-generated method
            this.ReserveNavigationLanes(priority, prefabAircraftData, aircraft, ref navigation, ref aircraftCurrentLane, navigationLanes);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref aircraftCurrentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            nativeArray4[index] = navigation;
            nativeArray5[index] = aircraftCurrentLane;
            nativeArray6[index] = pathOwner;
            nativeArray10[index] = blocker;
            if (flag)
              nativeArray11[index] = odometer;
          }
        }
        else
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            AircraftNavigation navigation = nativeArray4[index];
            AircraftCurrentLane aircraftCurrentLane = nativeArray5[index];
            PathOwner pathOwnerData = nativeArray6[index];
            PrefabRef prefabRef = nativeArray7[index];
            DynamicBuffer<AircraftNavigationLane> navigationLanes = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            AircraftNavigationHelpers.CurrentLaneCache currentLaneCache = new AircraftNavigationHelpers.CurrentLaneCache(ref aircraftCurrentLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
            // ISSUE: reference to a compiler-generated method
            this.UpdateStopped(isHelicopter, transform, ref aircraftCurrentLane, ref pathOwnerData, navigationLanes, pathElements);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref aircraftCurrentLane, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, new Moving(), navigation, objectGeometryData);
            nativeArray5[index] = aircraftCurrentLane;
            nativeArray6[index] = pathOwnerData;
          }
        }
      }

      private void UpdateStopped(
        bool isHelicopter,
        Game.Objects.Transform transform,
        ref AircraftCurrentLane currentLaneData,
        ref PathOwner pathOwnerData,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements)
      {
        if (!(currentLaneData.m_Lane == Entity.Null) && (currentLaneData.m_LaneFlags & AircraftLaneFlags.Obsolete) == (AircraftLaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.TryFindCurrentLane(ref currentLaneData, transform, isHelicopter);
        navigationLanes.Clear();
        pathElements.Clear();
        pathOwnerData.m_ElementIndex = 0;
        pathOwnerData.m_State |= PathFlags.Obsolete;
      }

      private void UpdateNavigationLanes(
        int priority,
        Entity entity,
        bool isHelicopter,
        Game.Objects.Transform transform,
        Game.Common.Target target,
        Aircraft watercraft,
        ref AircraftCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements)
      {
        int invalidPath = 10000000;
        if (currentLane.m_Lane == Entity.Null || (currentLane.m_LaneFlags & AircraftLaneFlags.Obsolete) != (AircraftLaneFlags) 0)
        {
          invalidPath = -1;
          // ISSUE: reference to a compiler-generated method
          this.TryFindCurrentLane(ref currentLane, transform, isHelicopter);
        }
        else if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0 && (pathOwner.m_State & PathFlags.Append) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearNavigationLanes(ref currentLane, navigationLanes, invalidPath);
        }
        else if ((pathOwner.m_State & PathFlags.Updated) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.FillNavigationPaths(priority, entity, isHelicopter, transform, target, watercraft, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements, ref invalidPath);
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
        ref AircraftCurrentLane currentLane,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        int invalidPath)
      {
        currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
        if (invalidPath > 0)
        {
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            if ((navigationLanes[index].m_Flags & AircraftLaneFlags.Reserved) == (AircraftLaneFlags) 0)
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
        ref AircraftCurrentLane currentLaneData,
        Game.Objects.Transform transformData,
        bool isHelicopter)
      {
        currentLaneData.m_LaneFlags &= ~AircraftLaneFlags.Obsolete;
        currentLaneData.m_Lane = Entity.Null;
        float3 position = transformData.m_Position;
        float num = 100f;
        Bounds3 bounds3 = new Bounds3(position - num, position + num);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AircraftNavigationHelpers.FindLaneIterator iterator = new AircraftNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = position,
          m_MinDistance = num,
          m_Result = currentLaneData,
          m_CarType = isHelicopter ? RoadTypes.Helicopter : RoadTypes.Airplane,
          m_SubLanes = this.m_Lanes,
          m_CarLaneData = this.m_CarLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCarLaneData = this.m_PrefabCarLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<AircraftNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        iterator.Iterate(ref this.m_AirwayData);
        currentLaneData = iterator.m_Result;
      }

      private void FillNavigationPaths(
        int priority,
        Entity entity,
        bool isHelicopter,
        Game.Objects.Transform transform,
        Game.Common.Target target,
        Aircraft aircraft,
        ref AircraftCurrentLane currentLaneData,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements,
        ref int invalidPath)
      {
        if ((currentLaneData.m_LaneFlags & AircraftLaneFlags.EndOfPath) != (AircraftLaneFlags) 0)
          return;
        for (int index = 0; index < 8; ++index)
        {
          if (index >= navigationLanes.Length)
          {
            index = navigationLanes.Length;
            if (pathOwner.m_ElementIndex >= pathElements.Length)
            {
              if ((pathOwner.m_State & PathFlags.Pending) != (PathFlags) 0)
                break;
              AircraftNavigationLane elem = new AircraftNavigationLane();
              if (index > 0)
              {
                AircraftNavigationLane navigationLane = navigationLanes[index - 1];
                if ((navigationLane.m_Flags & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  if (!this.GetTransformTarget(ref elem.m_Lane, target))
                  {
                    navigationLane.m_Flags |= AircraftLaneFlags.EndOfPath;
                    navigationLanes[index - 1] = navigationLane;
                    break;
                  }
                  elem.m_Flags |= AircraftLaneFlags.EndOfPath | AircraftLaneFlags.TransformTarget;
                  if ((aircraft.m_Flags & AircraftFlags.StayMidAir) != (AircraftFlags) 0)
                    elem.m_Flags |= AircraftLaneFlags.Airway;
                  navigationLanes.Add(elem);
                  break;
                }
                // ISSUE: reference to a compiler-generated method
                if ((navigationLane.m_Flags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0 || (aircraft.m_Flags & AircraftFlags.StayOnTaxiway) != (AircraftFlags) 0 || !this.GetTransformTarget(ref elem.m_Lane, target))
                {
                  navigationLane.m_Flags |= AircraftLaneFlags.EndOfPath;
                  navigationLanes[index - 1] = navigationLane;
                  break;
                }
                elem.m_Flags |= AircraftLaneFlags.EndOfPath | AircraftLaneFlags.TransformTarget;
                navigationLanes.Add(elem);
                break;
              }
              if ((currentLaneData.m_LaneFlags & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.GetTransformTarget(ref elem.m_Lane, target))
                {
                  currentLaneData.m_LaneFlags |= AircraftLaneFlags.EndOfPath;
                  break;
                }
                elem.m_Flags |= AircraftLaneFlags.EndOfPath | AircraftLaneFlags.TransformTarget;
                if ((aircraft.m_Flags & AircraftFlags.StayMidAir) != (AircraftFlags) 0)
                  elem.m_Flags |= AircraftLaneFlags.Airway;
                navigationLanes.Add(elem);
                break;
              }
              // ISSUE: reference to a compiler-generated method
              if ((currentLaneData.m_LaneFlags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0 || (aircraft.m_Flags & AircraftFlags.StayOnTaxiway) != (AircraftFlags) 0 || !this.GetTransformTarget(ref elem.m_Lane, target))
              {
                currentLaneData.m_LaneFlags |= AircraftLaneFlags.EndOfPath;
                break;
              }
              elem.m_Flags |= AircraftLaneFlags.EndOfPath | AircraftLaneFlags.TransformTarget;
              navigationLanes.Add(elem);
              break;
            }
            PathElement pathElement = pathElements[pathOwner.m_ElementIndex++];
            AircraftNavigationLane elem1 = new AircraftNavigationLane();
            elem1.m_Lane = pathElement.m_Target;
            elem1.m_CurvePosition = pathElement.m_TargetDelta;
            if (((index <= 0 ? currentLaneData.m_LaneFlags : navigationLanes[index - 1].m_Flags) & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0 && (aircraft.m_Flags & AircraftFlags.StayMidAir) != (AircraftFlags) 0)
              elem1.m_Flags |= AircraftLaneFlags.Airway;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData.HasComponent(elem1.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_CarLaneData[elem1.m_Lane].m_Flags & Game.Net.CarLaneFlags.Runway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
                elem1.m_Flags |= AircraftLaneFlags.Runway;
              navigationLanes.Add(elem1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(elem1.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                if ((this.m_ConnectionLaneData[elem1.m_Lane].m_Flags & ConnectionLaneFlags.Airway) != (ConnectionLaneFlags) 0)
                {
                  elem1.m_Flags |= AircraftLaneFlags.Airway;
                  navigationLanes.Add(elem1);
                  break;
                }
                elem1.m_Flags |= AircraftLaneFlags.Connection;
                navigationLanes.Add(elem1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TakeoffLocationData.HasComponent(elem1.m_Lane))
                {
                  if (isHelicopter)
                  {
                    elem1.m_Flags |= AircraftLaneFlags.TransformTarget;
                    // ISSUE: reference to a compiler-generated field
                    if ((aircraft.m_Flags & AircraftFlags.StayMidAir) == (AircraftFlags) 0 && this.m_SpawnLocationData.HasComponent(elem1.m_Lane))
                      elem1.m_Flags |= AircraftLaneFlags.ParkingSpace;
                    navigationLanes.Add(elem1);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SpawnLocationData.HasComponent(elem1.m_Lane))
                  {
                    if (pathOwner.m_ElementIndex >= pathElements.Length && (pathOwner.m_State & PathFlags.Pending) != (PathFlags) 0)
                    {
                      --pathOwner.m_ElementIndex;
                      break;
                    }
                    if ((aircraft.m_Flags & AircraftFlags.StayOnTaxiway) == (AircraftFlags) 0 || pathElements.Length > pathOwner.m_ElementIndex)
                    {
                      elem1.m_Flags |= AircraftLaneFlags.TransformTarget;
                      navigationLanes.Add(elem1);
                    }
                  }
                  else
                  {
                    invalidPath = index;
                    break;
                  }
                }
              }
            }
          }
          else
          {
            AircraftNavigationLane navigationLane = navigationLanes[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
            {
              invalidPath = index;
              break;
            }
            if ((navigationLane.m_Flags & AircraftLaneFlags.EndOfPath) != (AircraftLaneFlags) 0)
              break;
          }
        }
      }

      private bool GetTransformTarget(ref Entity entity, Game.Common.Target target)
      {
        PropertyRenter componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PropertyRenterData.TryGetComponent(target.m_Target, out componentData))
          target.m_Target = componentData.m_Property;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(target.m_Target))
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

      private void CheckBlocker(
        Aircraft aircraft,
        bool isHelicopter,
        ref AircraftCurrentLane currentLane,
        ref Blocker blocker,
        ref AircraftLaneSpeedIterator laneIterator)
      {
        if (laneIterator.m_Blocker != blocker.m_Blocker)
          currentLane.m_LaneFlags &= ~AircraftLaneFlags.IgnoreBlocker;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (laneIterator.m_Blocker != Entity.Null && this.m_MovingDataFromEntity.HasComponent(laneIterator.m_Blocker) && this.m_AircraftData.HasComponent(laneIterator.m_Blocker) && (this.m_AircraftData[laneIterator.m_Blocker].m_Flags & ~aircraft.m_Flags & AircraftFlags.Blocking) != (AircraftFlags) 0 && (double) laneIterator.m_MaxSpeed < 1.0)
          currentLane.m_LaneFlags |= AircraftLaneFlags.IgnoreBlocker;
        float num = math.select(0.918000042f, 3.06f, isHelicopter);
        blocker.m_Blocker = laneIterator.m_Blocker;
        blocker.m_Type = laneIterator.m_BlockerType;
        blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(laneIterator.m_MaxSpeed * num), 0, (int) byte.MaxValue);
      }

      private void UpdateNavigationTarget(
        int priority,
        Entity entity,
        bool isHelicopter,
        Game.Objects.Transform transform,
        Moving moving,
        Aircraft aircraft,
        PrefabRef prefabRefData,
        AircraftData prefabAircraftData,
        ObjectGeometryData prefabObjectGeometryData,
        ref AircraftNavigation navigation,
        ref AircraftCurrentLane currentLane,
        ref Blocker blocker,
        ref Odometer odometer,
        DynamicBuffer<AircraftNavigationLane> navigationLanes)
      {
        float timeStep = 0.266666681f;
        float currentSpeed = math.length(moving.m_Velocity);
        float3 position = transform.m_Position;
        if ((currentLane.m_LaneFlags & AircraftLaneFlags.Flying) != (AircraftLaneFlags) 0)
        {
          if (isHelicopter)
          {
            // ISSUE: reference to a compiler-generated field
            HelicopterData helicopterData = this.m_PrefabHelicopterData[prefabRefData.m_Prefab];
            Bounds1 bounds = (currentLane.m_LaneFlags & (AircraftLaneFlags.Connection | AircraftLaneFlags.ResetSpeed)) == (AircraftLaneFlags) 0 ? VehicleUtils.CalculateSpeedRange(helicopterData, currentSpeed, timeStep) : new Bounds1(0.0f, helicopterData.m_FlyingMaxSpeed);
            if ((currentLane.m_LaneFlags & AircraftLaneFlags.SkipLane) != (AircraftLaneFlags) 0)
              navigation.m_TargetPosition = transform.m_Position;
            float3 float3 = navigation.m_TargetPosition - transform.m_Position;
            double x = (double) math.length(float3.xz);
            float y = math.length(float3);
            float num1 = math.saturate((float) (((double) math.dot(moving.m_Velocity, float3) + 1.0) / ((double) currentSpeed * (double) y + 1.0)));
            float s = 1f - math.sqrt((float) (1.0 - (double) num1 * (double) num1));
            float distance = math.lerp((float) x, y, s);
            float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(helicopterData, distance, timeStep + 0.5f);
            navigation.m_MaxSpeed = MathUtils.Clamp(maxBrakingSpeed, bounds);
            blocker.m_Blocker = Entity.Null;
            blocker.m_Type = BlockerType.None;
            blocker.m_MaxSpeed = byte.MaxValue;
            float minDistance = math.max(VehicleUtils.GetBrakingDistance(helicopterData, helicopterData.m_FlyingMaxSpeed, timeStep), 750f) + (float) ((double) navigation.m_MaxSpeed * (double) timeStep + 1.0);
            currentLane.m_Duration += timeStep;
            currentLane.m_Distance += currentSpeed * timeStep;
            odometer.m_Distance += currentSpeed * timeStep;
            if (x < (double) minDistance)
            {
              while (true)
              {
                bool flag = (currentLane.m_LaneFlags & AircraftLaneFlags.Landing) > (AircraftLaneFlags) 0;
                if (flag && navigationLanes.Length != 0 && (navigationLanes[0].m_Flags & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0)
                {
                  currentLane.m_LaneFlags &= ~AircraftLaneFlags.Landing;
                  flag = false;
                }
                if ((currentLane.m_LaneFlags & AircraftLaneFlags.SkipLane) == (AircraftLaneFlags) 0)
                {
                  if ((currentLane.m_LaneFlags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0)
                  {
                    navigation.m_TargetDirection = new float3();
                    // ISSUE: reference to a compiler-generated method
                    int num2 = this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, currentLane.m_Lane) ? 1 : 0;
                    if (!flag)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.UpdateTargetHeight(ref navigation.m_TargetPosition, currentLane.m_Lane, helicopterData);
                    }
                    if (num2 != 0)
                      goto label_120;
                  }
                  else
                  {
                    navigation.m_TargetDirection = new float3();
                    // ISSUE: reference to a compiler-generated field
                    Curve curve = this.m_CurveData[currentLane.m_Lane];
                    // ISSUE: reference to a compiler-generated method
                    int num3 = this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, curve.m_Bezier, ref currentLane.m_CurvePosition) ? 1 : 0;
                    if (!flag)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.UpdateTargetHeight(ref navigation.m_TargetPosition, currentLane.m_Lane, helicopterData);
                    }
                    if (num3 != 0)
                      goto label_120;
                  }
                }
                if (!flag)
                {
                  if (navigationLanes.Length != 0)
                  {
                    AircraftNavigationLane navigationLane = navigationLanes[0];
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
                    {
                      navigationLane.m_Flags |= AircraftLaneFlags.Flying;
                      if ((navigationLane.m_Flags & AircraftLaneFlags.Airway) == (AircraftLaneFlags) 0)
                        navigationLane.m_Flags |= AircraftLaneFlags.Landing;
                      // ISSUE: reference to a compiler-generated method
                      this.ApplySideEffects(ref currentLane, prefabRefData, helicopterData);
                      currentLane.m_Lane = navigationLane.m_Lane;
                      currentLane.m_CurvePosition = navigationLane.m_CurvePosition.xxy;
                      currentLane.m_LaneFlags = navigationLane.m_Flags;
                      navigationLanes.RemoveAt(0);
                    }
                    else
                      goto label_120;
                  }
                  else
                    goto label_20;
                }
                else
                  break;
              }
              if ((double) math.length(navigation.m_TargetPosition - transform.m_Position) < 1.0 && (double) currentSpeed < 0.10000000149011612)
              {
                currentLane.m_LaneFlags &= ~(AircraftLaneFlags.Flying | AircraftLaneFlags.Landing);
                if (navigationLanes.Length == 0)
                {
                  currentLane.m_LaneFlags |= AircraftLaneFlags.EndReached;
                  goto label_120;
                }
                else
                  goto label_120;
              }
              else
                goto label_120;
label_20:
              if ((double) math.length(navigation.m_TargetPosition - transform.m_Position) < 1.0 && (double) currentSpeed < 0.10000000149011612)
                currentLane.m_LaneFlags |= AircraftLaneFlags.EndReached;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            AirplaneData airplaneData = this.m_PrefabAirplaneData[prefabRefData.m_Prefab];
            Bounds1 bounds = (currentLane.m_LaneFlags & (AircraftLaneFlags.Connection | AircraftLaneFlags.ResetSpeed)) == (AircraftLaneFlags) 0 ? VehicleUtils.CalculateSpeedRange(airplaneData, currentSpeed, timeStep) : new Bounds1(airplaneData.m_FlyingSpeed.x, airplaneData.m_FlyingSpeed.y);
            float3 float3_1 = navigation.m_TargetPosition - transform.m_Position;
            float x = math.length(float3_1.xz);
            float y = math.length(float3_1);
            float num4 = math.saturate((float) (((double) math.dot(moving.m_Velocity, float3_1) + 1.0) / ((double) currentSpeed * (double) y + 1.0)));
            float s = 1f - math.sqrt((float) (1.0 - (double) num4 * (double) num4));
            float distance = math.lerp(x, y, s);
            float num5;
            if ((currentLane.m_LaneFlags & AircraftLaneFlags.Landing) != (AircraftLaneFlags) 0)
              num5 = prefabAircraftData.m_GroundMaxSpeed;
            else if ((currentLane.m_LaneFlags & (AircraftLaneFlags.Approaching | AircraftLaneFlags.TakingOff)) != (AircraftLaneFlags) 0)
            {
              num5 = airplaneData.m_FlyingSpeed.y;
              distance += 1500f;
            }
            else
            {
              num5 = math.max(VehicleUtils.GetBrakingDistance(airplaneData, airplaneData.m_FlyingSpeed.y, timeStep) - (VehicleUtils.GetBrakingDistance(airplaneData, prefabAircraftData.m_GroundMaxSpeed, timeStep) + 1500f), 1500f);
              distance += 1500f;
            }
            if ((currentLane.m_LaneFlags & AircraftLaneFlags.Connection) != (AircraftLaneFlags) 0)
            {
              airplaneData.m_FlyingBraking = 277.777771f;
              navigation.m_MaxSpeed = VehicleUtils.GetMaxBrakingSpeed(airplaneData, distance, 0.0f, timeStep + 0.5f);
            }
            else
            {
              float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(airplaneData, distance, prefabAircraftData.m_GroundMaxSpeed, timeStep + 0.5f);
              navigation.m_MaxSpeed = MathUtils.Clamp(maxBrakingSpeed, bounds);
            }
            blocker.m_Blocker = Entity.Null;
            blocker.m_Type = BlockerType.None;
            blocker.m_MaxSpeed = byte.MaxValue;
            if (currentLane.m_Lane == Entity.Null)
              return;
            float minDistance = num5 + (float) ((double) navigation.m_MaxSpeed * (double) timeStep + 1.0);
            currentLane.m_Duration += timeStep;
            currentLane.m_Distance += currentSpeed * timeStep;
            odometer.m_Distance += currentSpeed * timeStep;
            if ((double) x < (double) minDistance)
            {
              float3 float3_2;
              float num6;
              AircraftNavigationLane navigationLane;
              while (true)
              {
                bool flag = (currentLane.m_LaneFlags & AircraftLaneFlags.Landing) > (AircraftLaneFlags) 0;
                if ((currentLane.m_LaneFlags & AircraftLaneFlags.Approaching) != (AircraftLaneFlags) 0)
                {
                  if (flag)
                  {
                    // ISSUE: reference to a compiler-generated field
                    float3_2 = MathUtils.Position(this.m_CurveData[currentLane.m_Lane].m_Bezier, currentLane.m_CurvePosition.x);
                    num6 = math.length((float3_2 - transform.m_Position).xz);
                    if ((double) num6 < (double) minDistance)
                    {
                      currentLane.m_LaneFlags &= ~AircraftLaneFlags.Approaching;
                      continue;
                    }
                    break;
                  }
                  currentLane.m_LaneFlags |= AircraftLaneFlags.Landing;
                  minDistance = prefabAircraftData.m_GroundMaxSpeed + (float) ((double) navigation.m_MaxSpeed * (double) timeStep + 1.0);
                  flag = true;
                }
                else if ((currentLane.m_LaneFlags & AircraftLaneFlags.TakingOff) != (AircraftLaneFlags) 0)
                {
                  if ((currentLane.m_LaneFlags & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0)
                  {
                    if ((double) math.length((navigation.m_TargetPosition - transform.m_Position).xz) < (double) minDistance)
                    {
                      currentLane.m_LaneFlags &= ~AircraftLaneFlags.TakingOff;
                      continue;
                    }
                    goto label_71;
                  }
                }
                else if ((currentLane.m_LaneFlags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0)
                {
                  navigation.m_TargetDirection = new float3();
                  // ISSUE: reference to a compiler-generated method
                  int num7 = this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, currentLane.m_Lane) ? 1 : 0;
                  if (!flag)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateTargetHeight(ref navigation.m_TargetPosition, currentLane.m_Lane, airplaneData);
                  }
                  if (num7 != 0)
                    goto label_71;
                }
                else
                {
                  navigation.m_TargetDirection = new float3();
                  // ISSUE: reference to a compiler-generated field
                  Curve curve = this.m_CurveData[currentLane.m_Lane];
                  // ISSUE: reference to a compiler-generated method
                  int num8 = this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, curve.m_Bezier, ref currentLane.m_CurvePosition) ? 1 : 0;
                  if (!flag)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateTargetHeight(ref navigation.m_TargetPosition, currentLane.m_Lane, airplaneData);
                  }
                  if (num8 != 0)
                    goto label_71;
                }
                if (flag)
                {
                  if (navigationLanes.Length == 0)
                    goto label_71;
                }
                else if (navigationLanes.Length == 0)
                  goto label_56;
                navigationLane = navigationLanes[0];
                navigationLane.m_Flags |= AircraftLaneFlags.Flying;
                if ((currentLane.m_LaneFlags & AircraftLaneFlags.TakingOff) != (AircraftLaneFlags) 0)
                {
                  if ((currentLane.m_LaneFlags & AircraftLaneFlags.Runway) != (AircraftLaneFlags) 0)
                  {
                    navigationLane.m_Flags |= AircraftLaneFlags.TakingOff;
                    if ((navigationLane.m_Flags & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      float3 targetPosition = MathUtils.Position(this.m_CurveData[navigationLane.m_Lane].m_Bezier, navigationLane.m_CurvePosition.x);
                      // ISSUE: reference to a compiler-generated method
                      this.UpdateTargetHeight(ref targetPosition, navigationLane.m_Lane, airplaneData);
                      float num9 = math.length((targetPosition - navigation.m_TargetPosition).xz);
                      float2 float2 = math.normalizesafe(moving.m_Velocity.xz);
                      navigation.m_TargetDirection.y = 0.0f;
                      navigation.m_TargetDirection.xz = float2;
                      navigation.m_TargetPosition.xz += float2 * num9;
                      navigation.m_TargetPosition.y = math.min(targetPosition.y, navigation.m_TargetPosition.y + num9 * math.tan(airplaneData.m_ClimbAngle));
                    }
                  }
                }
                else if ((navigationLane.m_Flags & AircraftLaneFlags.Airway) == (AircraftLaneFlags) 0)
                {
                  if ((navigationLane.m_Flags & AircraftLaneFlags.Connection) != (AircraftLaneFlags) 0)
                    navigationLane.m_Flags |= AircraftLaneFlags.Landing;
                  else if (flag)
                    navigationLane.m_Flags |= currentLane.m_LaneFlags & (AircraftLaneFlags.Approaching | AircraftLaneFlags.Landing);
                  else
                    goto label_67;
                }
                // ISSUE: reference to a compiler-generated method
                this.ApplySideEffects(ref currentLane, prefabRefData, airplaneData);
                currentLane.m_Lane = navigationLane.m_Lane;
                currentLane.m_CurvePosition = navigationLane.m_CurvePosition.xxy;
                currentLane.m_LaneFlags = navigationLane.m_Flags;
                navigationLanes.RemoveAt(0);
              }
              float num10 = num6 - minDistance;
              navigation.m_TargetPosition.xz = float3_2.xz - navigation.m_TargetDirection.xz * num10;
              navigation.m_TargetPosition.y = math.min(navigation.m_TargetPosition.y, float3_2.y + num10 * math.tan(airplaneData.m_ClimbAngle));
              goto label_71;
label_56:
              if ((double) math.length((navigation.m_TargetPosition - transform.m_Position).xz) < (double) currentSpeed * 2.0)
              {
                currentLane.m_LaneFlags |= AircraftLaneFlags.EndReached;
                goto label_71;
              }
              else
                goto label_71;
label_67:
              currentLane.m_LaneFlags |= AircraftLaneFlags.Approaching;
              // ISSUE: reference to a compiler-generated field
              Curve curve1 = this.m_CurveData[navigationLane.m_Lane];
              float3 float3_3 = MathUtils.Position(curve1.m_Bezier, navigationLane.m_CurvePosition.x);
              float3 float3_4 = MathUtils.Position(curve1.m_Bezier, navigationLane.m_CurvePosition.y);
              float3 float3_5 = float3_3 - navigation.m_TargetPosition;
              float2 float2_1 = float3_4.xz - float3_3.xz;
              if (!MathUtils.TryNormalize(ref float2_1))
                float2_1 = math.normalizesafe(float3_5.xz);
              float num11 = math.length(float3_5.xz);
              navigation.m_TargetDirection.y = 0.0f;
              navigation.m_TargetDirection.xz = float2_1;
              navigation.m_TargetPosition.xz = float3_3.xz - float2_1 * num11;
              navigation.m_TargetPosition.y = math.min(navigation.m_TargetPosition.y, float3_3.y + num11 * math.tan(airplaneData.m_ClimbAngle));
            }
label_71:
            if ((currentLane.m_LaneFlags & (AircraftLaneFlags.Approaching | AircraftLaneFlags.Landing)) == AircraftLaneFlags.Landing)
            {
              float3 float3_6 = navigation.m_TargetPosition - transform.m_Position;
              if ((double) transform.m_Position.y < (double) navigation.m_TargetPosition.y + 1.0)
                currentLane.m_LaneFlags &= ~AircraftLaneFlags.Flying;
            }
          }
        }
        else
        {
          if ((currentLane.m_LaneFlags & AircraftLaneFlags.Connection) != (AircraftLaneFlags) 0)
          {
            prefabAircraftData.m_GroundMaxSpeed = 277.777771f;
            prefabAircraftData.m_GroundAcceleration = 277.777771f;
            prefabAircraftData.m_GroundBraking = 277.777771f;
          }
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
          Bounds1 bounds1 = (currentLane.m_LaneFlags & (AircraftLaneFlags.Connection | AircraftLaneFlags.ResetSpeed)) == (AircraftLaneFlags) 0 ? VehicleUtils.CalculateSpeedRange(prefabAircraftData, currentSpeed, timeStep) : new Bounds1(0.0f, prefabAircraftData.m_GroundMaxSpeed);
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
          AircraftLaneSpeedIterator laneIterator = new AircraftLaneSpeedIterator()
          {
            m_TransformData = this.m_TransformData,
            m_MovingData = this.m_MovingDataFromEntity,
            m_AircraftData = this.m_AircraftData,
            m_LaneReservationData = this.m_LaneReservationData,
            m_CurveData = this.m_CurveData,
            m_CarLaneData = this.m_CarLaneData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
            m_PrefabAircraftData = this.m_PrefabAircraftData,
            m_LaneOverlapData = this.m_LaneOverlaps,
            m_LaneObjectData = this.m_LaneObjects,
            m_Entity = entity,
            m_Ignore = (currentLane.m_LaneFlags & AircraftLaneFlags.IgnoreBlocker) != (AircraftLaneFlags) 0 ? blocker.m_Blocker : Entity.Null,
            m_Priority = priority,
            m_TimeStep = timeStep,
            m_SafeTimeStep = timeStep + 0.5f,
            m_PrefabAircraft = prefabAircraftData,
            m_PrefabObjectGeometry = prefabObjectGeometryData,
            m_SpeedRange = bounds1,
            m_MaxSpeed = bounds1.max,
            m_CanChangeLane = 1f,
            m_CurrentPosition = position
          };
          if ((currentLane.m_LaneFlags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0)
          {
            laneIterator.IterateTarget(navigation.m_TargetPosition);
            navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
          }
          else
          {
            if (currentLane.m_Lane == Entity.Null)
            {
              navigation.m_MaxSpeed = math.max(0.0f, currentSpeed - prefabAircraftData.m_GroundBraking * timeStep);
              blocker.m_Blocker = Entity.Null;
              blocker.m_Type = BlockerType.None;
              blocker.m_MaxSpeed = byte.MaxValue;
              return;
            }
            if (!laneIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_CurvePosition))
            {
              for (int index = 0; index < navigationLanes.Length; ++index)
              {
                AircraftNavigationLane navigationLane = navigationLanes[index];
                if ((navigationLane.m_Flags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  VehicleUtils.CalculateTransformPosition(ref laneIterator.m_CurrentPosition, navigationLane.m_Lane, this.m_TransformData, this.m_PositionDataFromEntity, this.m_PrefabRefData, this.m_PrefabBuildingData);
                  break;
                }
                if ((navigationLane.m_Flags & AircraftLaneFlags.Connection) != (AircraftLaneFlags) 0)
                {
                  laneIterator.m_PrefabAircraft.m_GroundMaxSpeed = 277.777771f;
                  laneIterator.m_PrefabAircraft.m_GroundAcceleration = 277.777771f;
                  laneIterator.m_PrefabAircraft.m_GroundBraking = 277.777771f;
                  laneIterator.m_SpeedRange = new Bounds1(0.0f, 277.777771f);
                }
                else if ((currentLane.m_LaneFlags & AircraftLaneFlags.Connection) != (AircraftLaneFlags) 0)
                  break;
                bool c = navigationLane.m_Lane == currentLane.m_Lane;
                float minOffset = math.select(-1f, currentLane.m_CurvePosition.y, c);
                if (laneIterator.IterateNextLane(navigationLane.m_Lane, navigationLane.m_CurvePosition, minOffset))
                  goto label_94;
              }
              laneIterator.IterateTarget(laneIterator.m_CurrentPosition);
            }
label_94:
            navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
            // ISSUE: reference to a compiler-generated method
            this.CheckBlocker(aircraft, isHelicopter, ref currentLane, ref blocker, ref laneIterator);
          }
          double num12 = (double) math.length((navigation.m_TargetPosition - transform.m_Position).xz);
          float minDistance = (float) ((double) navigation.m_MaxSpeed * (double) timeStep + 1.0);
          currentLane.m_Duration += timeStep;
          currentLane.m_Distance += currentSpeed * timeStep;
          odometer.m_Distance += currentSpeed * timeStep;
          double num13 = (double) minDistance;
          if (num12 < num13)
          {
            while (true)
            {
              if ((currentLane.m_LaneFlags & AircraftLaneFlags.TransformTarget) != (AircraftLaneFlags) 0)
              {
                navigation.m_TargetDirection = new float3();
                // ISSUE: reference to a compiler-generated method
                if ((currentLane.m_LaneFlags & AircraftLaneFlags.EndReached) == (AircraftLaneFlags) 0 && this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, currentLane.m_Lane))
                  goto label_115;
              }
              else
              {
                navigation.m_TargetDirection = new float3();
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[currentLane.m_Lane];
                // ISSUE: reference to a compiler-generated method
                if (this.MoveTarget(position, ref navigation.m_TargetPosition, minDistance, curve.m_Bezier, ref currentLane.m_CurvePosition))
                  break;
              }
              if (navigationLanes.Length != 0)
              {
                AircraftNavigationLane navigationLane = navigationLanes[0];
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
                {
                  if ((currentLane.m_LaneFlags & AircraftLaneFlags.Landing) != (AircraftLaneFlags) 0)
                  {
                    if ((navigationLane.m_Flags & AircraftLaneFlags.Runway) != (AircraftLaneFlags) 0)
                      navigationLane.m_Flags |= AircraftLaneFlags.Landing;
                  }
                  else if ((navigationLane.m_Flags & (AircraftLaneFlags.Runway | AircraftLaneFlags.Airway)) != (AircraftLaneFlags) 0)
                    navigationLane.m_Flags |= AircraftLaneFlags.TakingOff;
                  if ((currentLane.m_LaneFlags & AircraftLaneFlags.Connection) != (AircraftLaneFlags) 0 && (navigationLane.m_Flags & AircraftLaneFlags.Connection) == (AircraftLaneFlags) 0)
                  {
                    if ((double) math.length((navigation.m_TargetPosition - transform.m_Position).xz) < 1.0 && (double) currentSpeed <= 3.0)
                      navigationLane.m_Flags |= AircraftLaneFlags.ResetSpeed;
                    else
                      goto label_115;
                  }
                  // ISSUE: reference to a compiler-generated method
                  this.ApplySideEffects(ref currentLane, prefabRefData, prefabAircraftData);
                  currentLane.m_Lane = navigationLane.m_Lane;
                  currentLane.m_CurvePosition = navigationLane.m_CurvePosition.xxy;
                  currentLane.m_LaneFlags = navigationLane.m_Flags;
                  navigationLanes.RemoveAt(0);
                }
                else
                  goto label_115;
              }
              else
                goto label_101;
            }
            // ISSUE: reference to a compiler-generated method
            this.ApplyLanePosition(ref navigation.m_TargetPosition, ref currentLane, prefabObjectGeometryData);
            goto label_115;
label_101:
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(currentLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[currentLane.m_Lane];
              navigation.m_TargetDirection = MathUtils.Tangent(curve.m_Bezier, currentLane.m_CurvePosition.z);
              // ISSUE: reference to a compiler-generated method
              this.ApplyLanePosition(ref navigation.m_TargetPosition, ref currentLane, prefabObjectGeometryData);
            }
            if ((double) math.length((navigation.m_TargetPosition - transform.m_Position).xz) < 1.0 && (double) currentSpeed < 0.10000000149011612)
              currentLane.m_LaneFlags |= AircraftLaneFlags.EndReached;
          }
label_115:
          if ((currentLane.m_LaneFlags & AircraftLaneFlags.TakingOff) != (AircraftLaneFlags) 0)
          {
            if (isHelicopter)
            {
              // ISSUE: reference to a compiler-generated field
              HelicopterData helicopterData = this.m_PrefabHelicopterData[prefabRefData.m_Prefab];
              currentLane.m_LaneFlags |= AircraftLaneFlags.Flying;
              // ISSUE: reference to a compiler-generated method
              this.UpdateTargetHeight(ref navigation.m_TargetPosition, currentLane.m_Lane, helicopterData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              AirplaneData airplaneData = this.m_PrefabAirplaneData[prefabRefData.m_Prefab];
              if ((double) currentSpeed >= (double) airplaneData.m_FlyingSpeed.x || (currentLane.m_LaneFlags & AircraftLaneFlags.Airway) != (AircraftLaneFlags) 0)
                currentLane.m_LaneFlags |= AircraftLaneFlags.Flying;
            }
          }
        }
label_120:
        if ((currentLane.m_LaneFlags & AircraftLaneFlags.Flying) != (AircraftLaneFlags) 0)
        {
          if (isHelicopter)
          {
            // ISSUE: reference to a compiler-generated field
            HelicopterData helicopterData = this.m_PrefabHelicopterData[prefabRefData.m_Prefab];
            float num14 = 0.0f;
            switch (helicopterData.m_HelicopterType)
            {
              case HelicopterType.Helicopter:
                num14 = 100f;
                break;
              case HelicopterType.Rocket:
                num14 = 10000f;
                break;
            }
            if ((currentLane.m_LaneFlags & AircraftLaneFlags.Landing) != (AircraftLaneFlags) 0)
            {
              float3 targetPosition = navigation.m_TargetPosition;
              // ISSUE: reference to a compiler-generated method
              this.UpdateTargetHeight(ref targetPosition, currentLane.m_Lane, helicopterData);
              // ISSUE: reference to a compiler-generated method
              this.GetCollisionHeightTarget(entity, transform, ref navigation, ref blocker, prefabObjectGeometryData, targetPosition);
              float3 float3 = targetPosition - transform.m_Position;
              float num15 = math.length(float3.xz);
              navigation.m_MinClimbAngle = math.atan(float3.y * 4f / num14);
              navigation.m_MinClimbAngle = math.min(navigation.m_MinClimbAngle, math.asin(math.saturate((float) ((double) num15 * 2.0 / (double) num14 - 1.0))));
            }
            else
            {
              float num16 = navigation.m_TargetPosition.y - transform.m_Position.y;
              navigation.m_MinClimbAngle = math.atan(num16 * 4f / num14) * 1.15f;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            AirplaneData airplaneData = this.m_PrefabAirplaneData[prefabRefData.m_Prefab];
            if ((currentLane.m_LaneFlags & (AircraftLaneFlags.Approaching | AircraftLaneFlags.Landing)) == (AircraftLaneFlags.Approaching | AircraftLaneFlags.Landing))
            {
              float3 targetPosition = navigation.m_TargetPosition;
              // ISSUE: reference to a compiler-generated method
              this.UpdateTargetHeight(ref targetPosition, currentLane.m_Lane, airplaneData);
              float3 float3 = navigation.m_TargetPosition - transform.m_Position;
              float num = math.max(0.0f, (float) ((double) math.length(float3.xz) - (double) prefabAircraftData.m_GroundMaxSpeed - (double) navigation.m_MaxSpeed * (double) timeStep - 1.0));
              float3.y += num * math.tan(airplaneData.m_ClimbAngle);
              float3.y = math.min(float3.y, targetPosition.y - transform.m_Position.y);
              navigation.m_MinClimbAngle = (float) ((double) math.atan(float3.y * 0.02f) * (double) airplaneData.m_ClimbAngle / 1.5707963705062866);
            }
            else
            {
              float num = navigation.m_TargetPosition.y - transform.m_Position.y;
              navigation.m_MinClimbAngle = (float) ((double) math.atan(num * 0.02f) * (double) airplaneData.m_ClimbAngle / 1.5707963705062866);
            }
          }
        }
        else
          navigation.m_MinClimbAngle = -1.57079637f;
      }

      private void GetCollisionHeightTarget(
        Entity entity,
        Game.Objects.Transform transform,
        ref AircraftNavigation navigation,
        ref Blocker blocker,
        ObjectGeometryData prefabObjectGeometryData,
        float3 targetPos)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AircraftNavigationHelpers.AircraftCollisionIterator iterator = new AircraftNavigationHelpers.AircraftCollisionIterator()
        {
          m_Ignore = entity,
          m_Line = new Line3.Segment(transform.m_Position, navigation.m_TargetPosition),
          m_AircraftData = this.m_AircraftData,
          m_TransformData = this.m_TransformData,
          m_ClosestT = 2f
        };
        // ISSUE: reference to a compiler-generated field
        this.m_MovingObjectSearchTree.Iterate<AircraftNavigationHelpers.AircraftCollisionIterator>(ref iterator);
        if (!(iterator.m_Result != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform1 = this.m_TransformData[iterator.m_Result];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[iterator.m_Result].m_Prefab];
        blocker.m_Blocker = iterator.m_Result;
        blocker.m_Type = BlockerType.Continuing;
        blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(navigation.m_MaxSpeed * 3.06f), 0, (int) byte.MaxValue);
        float x = (float) ((double) transform1.m_Position.y + (double) objectGeometryData.m_Bounds.max.y - (double) prefabObjectGeometryData.m_Bounds.min.y + 50.0);
        navigation.m_TargetPosition.y = math.clamp(x, navigation.m_TargetPosition.y, targetPos.y);
      }

      private void ApplyLanePosition(
        ref float3 targetPosition,
        ref AircraftCurrentLane currentLaneData,
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
        ref AircraftCurrentLane currentLaneData,
        PrefabRef prefabRefData,
        AircraftData prefabAircraftData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.CarLane carLaneData = this.m_CarLaneData[currentLaneData.m_Lane];
          float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(prefabAircraftData, carLaneData);
          float num1 = math.select(currentLaneData.m_Distance / currentLaneData.m_Duration, maxDriveSpeed, (double) currentLaneData.m_Duration == 0.0);
          float relativeSpeed = num1 / maxDriveSpeed;
          float3 sideEffects = new float3();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSideEffectData.HasComponent(prefabRefData.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleSideEffectData vehicleSideEffectData = this.m_PrefabSideEffectData[prefabRefData.m_Prefab];
            float num2 = num1 / prefabAircraftData.m_GroundMaxSpeed;
            float s = math.saturate(num2 * num2);
            sideEffects = math.lerp(vehicleSideEffectData.m_Min, vehicleSideEffectData.m_Max, s) * new float3(currentLaneData.m_Distance, currentLaneData.m_Duration, currentLaneData.m_Duration);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_LaneEffects.Enqueue(new AircraftNavigationHelpers.LaneEffects(currentLaneData.m_Lane, sideEffects, relativeSpeed));
        }
        currentLaneData.m_Duration = 0.0f;
        currentLaneData.m_Distance = 0.0f;
      }

      private void ApplySideEffects(
        ref AircraftCurrentLane currentLaneData,
        PrefabRef prefabRefData,
        HelicopterData prefabHelicopterData)
      {
        currentLaneData.m_Duration = 0.0f;
        currentLaneData.m_Distance = 0.0f;
      }

      private void ApplySideEffects(
        ref AircraftCurrentLane currentLaneData,
        PrefabRef prefabRefData,
        AirplaneData prefabAirplaneData)
      {
        currentLaneData.m_Duration = 0.0f;
        currentLaneData.m_Distance = 0.0f;
      }

      private void ReserveNavigationLanes(
        int priority,
        AircraftData prefabAircraftData,
        Aircraft watercraftData,
        ref AircraftNavigation navigationData,
        ref AircraftCurrentLane currentLaneData,
        DynamicBuffer<AircraftNavigationLane> navigationLanes)
      {
        float timeStep = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve1 = this.m_CurveData[currentLaneData.m_Lane];
          float num1 = math.max(0.0f, VehicleUtils.GetBrakingDistance(prefabAircraftData, navigationData.m_MaxSpeed, timeStep) - 0.01f);
          currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.x + num1 / math.max(1E-06f, curve1.m_Length);
          float num2 = num1 - curve1.m_Length * math.abs(currentLaneData.m_CurvePosition.z - currentLaneData.m_CurvePosition.x);
          int index = 0;
          if ((double) currentLaneData.m_CurvePosition.y <= (double) currentLaneData.m_CurvePosition.z)
            return;
          currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.z;
          AircraftNavigationLane navigationLane;
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
              this.m_LaneReservations.Enqueue(new AircraftNavigationHelpers.LaneReservation(navigationLane.m_Lane, offset, priority));
            }
            num2 -= curve2.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
            navigationLane.m_Flags |= AircraftLaneFlags.Reserved;
          }
        }
        else
          currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.x;
      }

      private void UpdateTargetHeight(
        ref float3 targetPosition,
        Entity target,
        HelicopterData helicopterData)
      {
        switch (helicopterData.m_HelicopterType)
        {
          case HelicopterType.Helicopter:
            targetPosition.y = 100f;
            break;
          case HelicopterType.Rocket:
            targetPosition.y = 10000f;
            break;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetPosition.y += WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, targetPosition);
      }

      private void UpdateTargetHeight(
        ref float3 targetPosition,
        Entity target,
        AirplaneData airplaneData)
      {
        targetPosition.y = 1000f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        targetPosition.y += WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, targetPosition);
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
        return VehicleUtils.CalculateTransformPosition(ref targetPosition, target, this.m_TransformData, this.m_PositionDataFromEntity, this.m_PrefabRefData, this.m_PrefabBuildingData) && (double) math.distance(comparePosition.xz, targetPosition.xz) >= (double) minDistance;
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
    private struct UpdateLaneReservationsJob : IJob
    {
      public NativeQueue<AircraftNavigationHelpers.LaneReservation> m_LaneReservationQueue;
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;

      public void Execute()
      {
        AircraftNavigationHelpers.LaneReservation laneReservation;
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
      public NativeQueue<AircraftNavigationHelpers.LaneEffects> m_LaneEffectsQueue;

      public void Execute()
      {
        AircraftNavigationHelpers.LaneEffects laneEffects;
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
      public ComponentTypeHandle<Aircraft> __Game_Vehicles_Aircraft_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<AircraftNavigation> __Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<AircraftNavigationLane> __Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle;
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
      public ComponentLookup<Game.Routes.TakeoffLocation> __Game_Routes_TakeoffLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aircraft> __Game_Vehicles_Aircraft_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AircraftData> __Game_Prefabs_AircraftData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HelicopterData> __Game_Prefabs_HelicopterData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AirplaneData> __Game_Prefabs_AirplaneData_RO_ComponentLookup;
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
        this.__Game_Vehicles_Aircraft_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Aircraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<AircraftNavigationLane>();
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
        this.__Game_Routes_TakeoffLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TakeoffLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Aircraft_RO_ComponentLookup = state.GetComponentLookup<Aircraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AircraftData_RO_ComponentLookup = state.GetComponentLookup<AircraftData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HelicopterData_RO_ComponentLookup = state.GetComponentLookup<HelicopterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AirplaneData_RO_ComponentLookup = state.GetComponentLookup<AirplaneData>(true);
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
