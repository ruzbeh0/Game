// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarNavigationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Creatures;
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
  public class CarNavigationSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private CarNavigationSystem.Actions m_Actions;
    private EntityQuery m_VehicleQuery;
    private CarNavigationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Actions = this.World.GetOrCreateSystemManaged<CarNavigationSystem.Actions>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Car>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<ParkedCar>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_LaneReservationQueue = new NativeQueue<CarNavigationHelpers.LaneReservation>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_LaneEffectsQueue = new NativeQueue<CarNavigationHelpers.LaneEffects>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_LaneSignalQueue = new NativeQueue<CarNavigationHelpers.LaneSignal>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_TrafficAmbienceQueue = new NativeQueue<CarNavigationSystem.TrafficAmbienceEffect>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_BlockedLane_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OutOfControl_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle dependencies3;
      JobHandle dependencies4;
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CarNavigationSystem.UpdateNavigationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentTypeHandle,
        m_OutOfControlType = this.__TypeHandle.__Game_Vehicles_OutOfControl_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Vehicles_CarNavigation_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferTypeHandle,
        m_EntityStorageInfoLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_AreaLaneData = this.__TypeHandle.__Game_Net_AreaLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneReservationData = this.__TypeHandle.__Game_Net_LaneReservation_RO_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup,
        m_LaneSignalData = this.__TypeHandle.__Game_Net_LaneSignal_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_CreatureData = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSideEffectData = this.__TypeHandle.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_TrailerLaneData = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup,
        m_BlockedLanes = this.__TypeHandle.__Game_Objects_BlockedLane_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies2),
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_MovingObjectSearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(true, out dependencies4),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_LaneObjectBuffer = this.m_Actions.m_LaneObjectUpdater.Begin(Allocator.TempJob),
        m_LaneReservations = this.m_Actions.m_LaneReservationQueue.AsParallelWriter(),
        m_LaneEffects = this.m_Actions.m_LaneEffectsQueue.AsParallelWriter(),
        m_LaneSignals = this.m_Actions.m_LaneSignalQueue.AsParallelWriter(),
        m_TrafficAmbienceEffects = this.m_Actions.m_TrafficAmbienceQueue.AsParallelWriter()
      }.ScheduleParallel<CarNavigationSystem.UpdateNavigationJob>(this.m_VehicleQuery, JobUtils.CombineDependencies(this.Dependency, dependencies1, dependencies2, dependencies3, dependencies4));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_Dependency = jobHandle;
      this.Dependency = jobHandle;
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
    public CarNavigationSystem()
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
      public ComponentTypeHandle<Car> m_CarType;
      [ReadOnly]
      public ComponentTypeHandle<OutOfControl> m_OutOfControlType;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<CarNavigation> m_NavigationType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Blocker> m_BlockerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<CarNavigationLane> m_NavigationLaneType;
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
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
      public ComponentLookup<AreaLane> m_AreaLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
      [ReadOnly]
      public ComponentLookup<LaneCondition> m_LaneConditionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<Car> m_CarData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<VehicleSideEffectData> m_PrefabSideEffectData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CarTrailerLane> m_TrailerLaneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<BlockedLane> m_BlockedLanes;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public LaneObjectCommandBuffer m_LaneObjectBuffer;
      public NativeQueue<CarNavigationHelpers.LaneReservation>.ParallelWriter m_LaneReservations;
      public NativeQueue<CarNavigationHelpers.LaneEffects>.ParallelWriter m_LaneEffects;
      public NativeQueue<CarNavigationHelpers.LaneSignal>.ParallelWriter m_LaneSignals;
      public NativeQueue<CarNavigationSystem.TrafficAmbienceEffect>.ParallelWriter m_TrafficAmbienceEffects;

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
        NativeArray<Blocker> nativeArray4 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray5 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarNavigation> nativeArray6 = chunk.GetNativeArray<CarNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray8 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_NavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor2 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor3 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<OutOfControl>(ref this.m_OutOfControlType))
        {
          NativeList<BlockedLane> nativeList = new NativeList<BlockedLane>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            CarNavigation carNavigation = nativeArray6[index];
            CarCurrentLane currentLane = nativeArray5[index];
            Blocker blocker = nativeArray4[index];
            PathOwner pathOwner = nativeArray8[index];
            PrefabRef prefabRef = nativeArray7[index];
            DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[entity];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            Moving moving = new Moving();
            if (nativeArray3.Length != 0)
              moving = nativeArray3[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            CarNavigationHelpers.CurrentLaneCache currentLaneCache = new CarNavigationHelpers.CurrentLaneCache(ref currentLane, blockedLane, this.m_EntityStorageInfoLookup, this.m_MovingObjectSearchTree);
            // ISSUE: reference to a compiler-generated method
            this.UpdateOutOfControl(entity, transform, objectGeometryData, ref carNavigation, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements, blockedLane, nativeList);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, nativeList, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, carNavigation, objectGeometryData);
            nativeArray6[index] = carNavigation;
            nativeArray5[index] = currentLane;
            nativeArray8[index] = pathOwner;
            nativeArray4[index] = blocker;
            nativeList.Clear();
            if (bufferAccessor3.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateOutOfControlTrailers(carNavigation, bufferAccessor3[index], nativeList);
            }
          }
          nativeList.Dispose();
        }
        else if (nativeArray3.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Common.Target> nativeArray9 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Car> nativeArray10 = chunk.GetNativeArray<Car>(ref this.m_CarType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Odometer> nativeArray11 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PseudoRandomSeed> nativeArray12 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
          NativeList<Entity> tempBuffer = new NativeList<Entity>();
          CarLaneSelectBuffer laneSelectBuffer = new CarLaneSelectBuffer();
          bool flag = nativeArray11.Length != 0;
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            Moving moving = nativeArray3[index];
            Game.Common.Target target = nativeArray9[index];
            Car car = nativeArray10[index];
            CarNavigation navigation = nativeArray6[index];
            CarCurrentLane currentLane = nativeArray5[index];
            PseudoRandomSeed pseudoRandomSeed = nativeArray12[index];
            Blocker blocker = nativeArray4[index];
            PathOwner pathOwner = nativeArray8[index];
            PrefabRef prefabRef = nativeArray7[index];
            DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[entity];
            // ISSUE: reference to a compiler-generated field
            CarData prefabCarData = this.m_PrefabCarData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            if (bufferAccessor3.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateCarLimits(ref prefabCarData, bufferAccessor3[index]);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            CarNavigationHelpers.CurrentLaneCache currentLaneCache = new CarNavigationHelpers.CurrentLaneCache(ref currentLane, blockedLane, this.m_EntityStorageInfoLookup, this.m_MovingObjectSearchTree);
            int priority = VehicleUtils.GetPriority(car);
            Odometer odometer = new Odometer();
            if (flag)
              odometer = nativeArray11[index];
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationLanes(ref random, priority, entity, transform, moving, target, car, prefabCarData, ref laneSelectBuffer, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNavigationTarget(ref random, priority, entity, transform, moving, car, pseudoRandomSeed, prefabRef, prefabCarData, objectGeometryData, ref navigation, ref currentLane, ref blocker, ref odometer, ref pathOwner, ref tempBuffer, navigationLanes, pathElements);
            // ISSUE: reference to a compiler-generated method
            this.ReserveNavigationLanes(ref random, priority, entity, prefabCarData, objectGeometryData, car, ref navigation, ref currentLane, navigationLanes);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, new NativeList<BlockedLane>(), this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: object of a compiler-generated type is created
            this.m_TrafficAmbienceEffects.Enqueue(new CarNavigationSystem.TrafficAmbienceEffect()
            {
              m_Amount = this.CalculateNoise(ref currentLane, prefabRef, prefabCarData),
              m_Position = transform.m_Position
            });
            nativeArray6[index] = navigation;
            nativeArray5[index] = currentLane;
            nativeArray8[index] = pathOwner;
            nativeArray4[index] = blocker;
            if (flag)
              nativeArray11[index] = odometer;
            if (bufferAccessor3.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateTrailers(navigation, currentLane, bufferAccessor3[index]);
            }
          }
          laneSelectBuffer.Dispose();
          if (!tempBuffer.IsCreated)
            return;
          tempBuffer.Dispose();
        }
        else
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity entity = nativeArray1[index];
            Game.Objects.Transform transform = nativeArray2[index];
            CarNavigation navigation = nativeArray6[index];
            CarCurrentLane currentLane = nativeArray5[index];
            Blocker blocker = nativeArray4[index];
            PathOwner pathOwner = nativeArray8[index];
            PrefabRef prefabRef = nativeArray7[index];
            DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
            DynamicBuffer<PathElement> pathElements = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[entity];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            CarNavigationHelpers.CurrentLaneCache currentLaneCache = new CarNavigationHelpers.CurrentLaneCache(ref currentLane, blockedLane, this.m_EntityStorageInfoLookup, this.m_MovingObjectSearchTree);
            // ISSUE: reference to a compiler-generated method
            this.UpdateStopped(transform, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            currentLaneCache.CheckChanges(entity, ref currentLane, new NativeList<BlockedLane>(), this.m_LaneObjectBuffer, this.m_LaneObjects, transform, new Moving(), navigation, objectGeometryData);
            nativeArray5[index] = currentLane;
            nativeArray8[index] = pathOwner;
            nativeArray4[index] = blocker;
            if (bufferAccessor3.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateStoppedTrailers(navigation, bufferAccessor3[index]);
            }
          }
        }
      }

      private void UpdateCarLimits(ref CarData prefabCarData, DynamicBuffer<LayoutElement> layout)
      {
        for (int index = 1; index < layout.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarData carData = this.m_PrefabCarData[this.m_PrefabRefData[layout[index].m_Vehicle].m_Prefab];
          prefabCarData.m_Acceleration = math.min(prefabCarData.m_Acceleration, carData.m_Acceleration);
          prefabCarData.m_Braking = math.min(prefabCarData.m_Braking, carData.m_Braking);
          prefabCarData.m_MaxSpeed = math.min(prefabCarData.m_MaxSpeed, carData.m_MaxSpeed);
          prefabCarData.m_Turning = math.min(prefabCarData.m_Turning, carData.m_Turning);
        }
      }

      private void UpdateTrailers(
        CarNavigation navigation,
        CarCurrentLane currentLane,
        DynamicBuffer<LayoutElement> layout)
      {
        Entity lane = currentLane.m_Lane;
        float2 nextPosition = currentLane.m_CurvePosition.xy;
        bool forceNext = (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Connection) > (Game.Vehicles.CarLaneFlags) 0;
        for (int index = 1; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          CarTrailerLane trailerLane = this.m_TrailerLaneData[vehicle];
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[vehicle];
          // ISSUE: reference to a compiler-generated field
          Moving moving = this.m_MovingData[vehicle];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[vehicle];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[vehicle].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.TrailerLaneCache trailerLaneCache = new CarNavigationHelpers.TrailerLaneCache(ref trailerLane, blockedLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
          if (trailerLane.m_Lane == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryFindCurrentLane(ref trailerLane, transform, moving);
          }
          // ISSUE: reference to a compiler-generated method
          this.UpdateTrailer(vehicle, transform, objectGeometryData, lane, nextPosition, forceNext, ref trailerLane);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          trailerLaneCache.CheckChanges(vehicle, ref trailerLane, new NativeList<BlockedLane>(), this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
          // ISSUE: reference to a compiler-generated field
          this.m_TrailerLaneData[vehicle] = trailerLane;
          lane = trailerLane.m_Lane;
          nextPosition = trailerLane.m_CurvePosition;
        }
      }

      private void UpdateOutOfControlTrailers(
        CarNavigation navigation,
        DynamicBuffer<LayoutElement> layout,
        NativeList<BlockedLane> tempBlockedLanes)
      {
        for (int index = 1; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          CarTrailerLane trailerLane = this.m_TrailerLaneData[vehicle];
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[vehicle];
          // ISSUE: reference to a compiler-generated field
          Moving moving = this.m_MovingData[vehicle];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[vehicle];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[vehicle].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.TrailerLaneCache trailerLaneCache = new CarNavigationHelpers.TrailerLaneCache(ref trailerLane, blockedLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
          // ISSUE: reference to a compiler-generated method
          this.UpdateOutOfControl(vehicle, transform, objectGeometryData, ref trailerLane, blockedLane, tempBlockedLanes);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          trailerLaneCache.CheckChanges(vehicle, ref trailerLane, tempBlockedLanes, this.m_LaneObjectBuffer, this.m_LaneObjects, transform, moving, navigation, objectGeometryData);
          // ISSUE: reference to a compiler-generated field
          this.m_TrailerLaneData[vehicle] = trailerLane;
          tempBlockedLanes.Clear();
        }
      }

      private void UpdateStoppedTrailers(
        CarNavigation navigation,
        DynamicBuffer<LayoutElement> layout)
      {
        for (int index = 1; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          CarTrailerLane trailerLane = this.m_TrailerLaneData[vehicle];
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[vehicle];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BlockedLane> blockedLane = this.m_BlockedLanes[vehicle];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[vehicle].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CarNavigationHelpers.TrailerLaneCache trailerLaneCache = new CarNavigationHelpers.TrailerLaneCache(ref trailerLane, blockedLane, this.m_PrefabRefData, this.m_MovingObjectSearchTree);
          if (trailerLane.m_Lane == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryFindCurrentLane(ref trailerLane, transform, new Moving());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          trailerLaneCache.CheckChanges(vehicle, ref trailerLane, new NativeList<BlockedLane>(), this.m_LaneObjectBuffer, this.m_LaneObjects, transform, new Moving(), navigation, objectGeometryData);
          // ISSUE: reference to a compiler-generated field
          this.m_TrailerLaneData[vehicle] = trailerLane;
        }
      }

      private void UpdateStopped(
        Game.Objects.Transform transform,
        ref CarCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements)
      {
        if (currentLane.m_Lane == Entity.Null || (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Obsolete) != (Game.Vehicles.CarLaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryFindCurrentLane(ref currentLane, transform, new Moving());
          navigationLanes.Clear();
          pathElements.Clear();
          pathOwner.m_ElementIndex = 0;
          pathOwner.m_State |= PathFlags.Obsolete;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.QueueReached) == (Game.Vehicles.CarLaneFlags) 0 || this.m_CarData.HasComponent(blocker.m_Blocker) && (this.m_CarData[blocker.m_Blocker].m_Flags & CarFlags.Queueing) != (CarFlags) 0)
          return;
        currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.QueueReached;
        blocker = new Blocker();
      }

      private void UpdateOutOfControl(
        Entity entity,
        Game.Objects.Transform transform,
        ObjectGeometryData prefabObjectGeometryData,
        ref CarTrailerLane trailerLane,
        DynamicBuffer<BlockedLane> blockedLanes,
        NativeList<BlockedLane> tempBlockedLanes)
      {
        float3 position = transform.m_Position;
        float3 float3 = math.forward(transform.m_Rotation);
        Line3.Segment line = new Line3.Segment(position - float3 * math.max(0.1f, (float) (-(double) prefabObjectGeometryData.m_Bounds.min.z - (double) prefabObjectGeometryData.m_Size.x * 0.5)), position + float3 * math.max(0.1f, prefabObjectGeometryData.m_Bounds.max.z - prefabObjectGeometryData.m_Size.x * 0.5f));
        float range = prefabObjectGeometryData.m_Size.x * 0.5f;
        Bounds3 bounds3 = MathUtils.Expand(MathUtils.Bounds(line), (float3) range);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CarNavigationHelpers.FindBlockedLanesIterator iterator = new CarNavigationHelpers.FindBlockedLanesIterator()
        {
          m_Bounds = bounds3,
          m_Line = line,
          m_Radius = range,
          m_BlockedLanes = tempBlockedLanes,
          m_SubLanes = this.m_Lanes,
          m_MasterLaneData = this.m_MasterLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabLaneData = this.m_PrefabLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindBlockedLanesIterator>(ref iterator);
        trailerLane = new CarTrailerLane();
      }

      private void UpdateOutOfControl(
        Entity entity,
        Game.Objects.Transform transform,
        ObjectGeometryData prefabObjectGeometryData,
        ref CarNavigation carNavigation,
        ref CarCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements,
        DynamicBuffer<BlockedLane> blockedLanes,
        NativeList<BlockedLane> tempBlockedLanes)
      {
        float3 position = transform.m_Position;
        float3 float3 = math.forward(transform.m_Rotation);
        Line3.Segment line = new Line3.Segment(position - float3 * math.max(0.1f, (float) (-(double) prefabObjectGeometryData.m_Bounds.min.z - (double) prefabObjectGeometryData.m_Size.x * 0.5)), position + float3 * math.max(0.1f, prefabObjectGeometryData.m_Bounds.max.z - prefabObjectGeometryData.m_Size.x * 0.5f));
        float range = prefabObjectGeometryData.m_Size.x * 0.5f;
        Bounds3 bounds3 = MathUtils.Expand(MathUtils.Bounds(line), (float3) range);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CarNavigationHelpers.FindBlockedLanesIterator iterator = new CarNavigationHelpers.FindBlockedLanesIterator()
        {
          m_Bounds = bounds3,
          m_Line = line,
          m_Radius = range,
          m_BlockedLanes = tempBlockedLanes,
          m_SubLanes = this.m_Lanes,
          m_MasterLaneData = this.m_MasterLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabLaneData = this.m_PrefabLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindBlockedLanesIterator>(ref iterator);
        carNavigation = new CarNavigation()
        {
          m_TargetPosition = transform.m_Position
        };
        currentLane = new CarCurrentLane();
        blocker = new Blocker();
        pathOwner.m_ElementIndex = 0;
        navigationLanes.Clear();
        pathElements.Clear();
      }

      private void UpdateNavigationLanes(
        ref Unity.Mathematics.Random random,
        int priority,
        Entity entity,
        Game.Objects.Transform transform,
        Moving moving,
        Game.Common.Target target,
        Car car,
        CarData prefabCarData,
        ref CarLaneSelectBuffer laneSelectBuffer,
        ref CarCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements)
      {
        int invalidPath = 10000000;
        if (currentLane.m_Lane == Entity.Null || (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Obsolete) != (Game.Vehicles.CarLaneFlags) 0)
        {
          invalidPath = -1;
          // ISSUE: reference to a compiler-generated method
          this.TryFindCurrentLane(ref currentLane, transform, moving);
        }
        else if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0 && (pathOwner.m_State & PathFlags.Append) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearNavigationLanes(ref currentLane, navigationLanes, invalidPath);
        }
        else if ((pathOwner.m_State & PathFlags.Updated) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.FillNavigationPaths(ref random, priority, entity, transform, target, car, ref laneSelectBuffer, ref currentLane, ref blocker, ref pathOwner, navigationLanes, pathElements, ref invalidPath);
        }
        if (invalidPath == 10000000)
          return;
        // ISSUE: reference to a compiler-generated method
        this.ClearNavigationLanes(moving, prefabCarData, ref currentLane, navigationLanes, invalidPath);
        pathElements.Clear();
        pathOwner.m_ElementIndex = 0;
        pathOwner.m_State |= PathFlags.Obsolete;
      }

      private void ClearNavigationLanes(
        ref CarCurrentLane currentLane,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        int invalidPath)
      {
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ClearedForPathfind) == (Game.Vehicles.CarLaneFlags) 0)
          currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
        if (invalidPath > 0)
        {
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            if ((navigationLanes[index].m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.ClearedForPathfind)) == (Game.Vehicles.CarLaneFlags) 0)
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

      private void ClearNavigationLanes(
        Moving moving,
        CarData prefabCarData,
        ref CarCurrentLane currentLane,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        int invalidPath)
      {
        if (invalidPath >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.ClearNavigationForPathfind(moving, prefabCarData, ref currentLane, navigationLanes, ref this.m_CarLaneData, ref this.m_CurveData);
        }
        else
          currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
        invalidPath = math.max(invalidPath, 0);
        if (invalidPath >= navigationLanes.Length)
          return;
        navigationLanes.RemoveRange(invalidPath, navigationLanes.Length - invalidPath);
      }

      private void TryFindCurrentLane(
        ref CarCurrentLane currentLane,
        Game.Objects.Transform transform,
        Moving moving)
      {
        float num1 = 0.266666681f;
        currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.TransformTarget | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Obsolete | Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight | Game.Vehicles.CarLaneFlags.Area);
        currentLane.m_Lane = Entity.Null;
        currentLane.m_ChangeLane = Entity.Null;
        float3 float3 = transform.m_Position + moving.m_Velocity * (num1 * 2f);
        float num2 = 100f;
        Bounds3 bounds3 = new Bounds3(float3 - num2, float3 + num2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CarNavigationHelpers.FindLaneIterator iterator = new CarNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = float3,
          m_MinDistance = num2,
          m_Result = currentLane,
          m_CarType = RoadTypes.Car,
          m_SubLanes = this.m_Lanes,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_CarLaneData = this.m_CarLaneData,
          m_MasterLaneData = this.m_MasterLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCarLaneData = this.m_PrefabCarLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
        currentLane = iterator.m_Result;
      }

      private void TryFindCurrentLane(
        ref CarTrailerLane trailerLane,
        Game.Objects.Transform transform,
        Moving moving)
      {
        float num1 = 0.266666681f;
        float3 float3 = transform.m_Position + moving.m_Velocity * (num1 * 2f);
        float num2 = 100f;
        Bounds3 bounds3 = new Bounds3(float3 - num2, float3 + num2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CarNavigationHelpers.FindLaneIterator iterator = new CarNavigationHelpers.FindLaneIterator()
        {
          m_Bounds = bounds3,
          m_Position = float3,
          m_MinDistance = num2,
          m_CarType = RoadTypes.Car,
          m_SubLanes = this.m_Lanes,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_CarLaneData = this.m_CarLaneData,
          m_MasterLaneData = this.m_MasterLaneData,
          m_ConnectionLaneData = this.m_ConnectionLaneData,
          m_CurveData = this.m_CurveData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCarLaneData = this.m_PrefabCarLaneData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<CarNavigationHelpers.FindLaneIterator>(ref iterator);
        trailerLane.m_Lane = iterator.m_Result.m_Lane;
        trailerLane.m_CurvePosition = iterator.m_Result.m_CurvePosition.xy;
        trailerLane.m_NextLane = Entity.Null;
        trailerLane.m_NextPosition = new float2();
      }

      private void FillNavigationPaths(
        ref Unity.Mathematics.Random random,
        int priority,
        Entity entity,
        Game.Objects.Transform transform,
        Game.Common.Target target,
        Car car,
        ref CarLaneSelectBuffer laneSelectBuffer,
        ref CarCurrentLane currentLane,
        ref Blocker blocker,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements,
        ref int invalidPath)
      {
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Waypoint)) == (Game.Vehicles.CarLaneFlags) 0)
        {
          for (int index = 0; index <= 8; ++index)
          {
            if (index >= navigationLanes.Length)
            {
              if (index == 8)
              {
                if ((pathOwner.m_State & PathFlags.Pending) == (PathFlags) 0)
                {
                  int max = math.min(40000, pathElements.Length - pathOwner.m_ElementIndex);
                  if (max > 0)
                  {
                    int num = random.NextInt(max) * (random.NextInt(max) + 1) / max;
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_EntityStorageInfoLookup.Exists(pathElements[pathOwner.m_ElementIndex + num].m_Target))
                    {
                      invalidPath = navigationLanes.Length;
                      return;
                    }
                    break;
                  }
                  break;
                }
                break;
              }
              index = navigationLanes.Length;
              if (pathOwner.m_ElementIndex >= pathElements.Length)
              {
                if ((pathOwner.m_State & PathFlags.Pending) == (PathFlags) 0)
                {
                  CarNavigationLane navLaneData = new CarNavigationLane();
                  if (index > 0)
                  {
                    CarNavigationLane navigationLane = navigationLanes[index - 1];
                    // ISSUE: reference to a compiler-generated method
                    if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.TransformTarget) == (Game.Vehicles.CarLaneFlags) 0 && (car.m_Flags & (CarFlags.StayOnRoad | CarFlags.AnyLaneTarget)) != (CarFlags.StayOnRoad | CarFlags.AnyLaneTarget) && this.GetTransformTarget(ref navLaneData.m_Lane, target))
                    {
                      if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) == (Game.Vehicles.CarLaneFlags) 0)
                      {
                        Entity lane = navLaneData.m_Lane;
                        navLaneData.m_Lane = navigationLane.m_Lane;
                        navLaneData.m_Flags = navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.Connection | Game.Vehicles.CarLaneFlags.Area);
                        navLaneData.m_CurvePosition = navigationLane.m_CurvePosition.yy;
                        float3 position = new float3();
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (VehicleUtils.CalculateTransformPosition(ref position, lane, this.m_TransformData, this.m_PositionData, this.m_PrefabRefData, this.m_PrefabBuildingData))
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.UpdateSlaveLane(ref navLaneData, position);
                        }
                        if ((car.m_Flags & CarFlags.StayOnRoad) != (CarFlags) 0)
                        {
                          navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.GroupTarget;
                          navigationLanes.Add(navLaneData);
                          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                          break;
                        }
                        navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.GroupTarget;
                        navigationLanes.Add(navLaneData);
                        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                      }
                      else
                      {
                        navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.TransformTarget;
                        navigationLanes.Add(navLaneData);
                        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                        break;
                      }
                    }
                    else
                    {
                      navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath;
                      navigationLanes[index - 1] = navigationLane;
                      currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                      break;
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.TransformTarget) != (Game.Vehicles.CarLaneFlags) 0 || (car.m_Flags & CarFlags.StayOnRoad) != (CarFlags) 0 || !this.GetTransformTarget(ref navLaneData.m_Lane, target))
                    {
                      currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.EndOfPath;
                      break;
                    }
                    navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.TransformTarget;
                    navigationLanes.Add(navLaneData);
                    currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                    break;
                  }
                }
                else
                  break;
              }
              else
              {
                PathElement pathElement = pathElements[pathOwner.m_ElementIndex++];
                CarNavigationLane navLaneData = new CarNavigationLane();
                navLaneData.m_Lane = pathElement.m_Target;
                navLaneData.m_CurvePosition = pathElement.m_TargetDelta;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_CarLaneData.HasComponent(navLaneData.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ParkingLaneData.HasComponent(navLaneData.m_Lane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Net.ParkingLane parkingLane = this.m_ParkingLaneData[navLaneData.m_Lane];
                    navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
                    if ((parkingLane.m_Flags & ParkingLaneFlags.ParkingLeft) != (ParkingLaneFlags) 0)
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.TurnLeft;
                    if ((parkingLane.m_Flags & ParkingLaneFlags.ParkingRight) != (ParkingLaneFlags) 0)
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.TurnRight;
                    navigationLanes.Add(navLaneData);
                    if (index > 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      float3 targetPosition = MathUtils.Position(this.m_CurveData[navLaneData.m_Lane].m_Bezier, navLaneData.m_CurvePosition.y);
                      CarNavigationLane navigationLane = navigationLanes[index - 1];
                      // ISSUE: reference to a compiler-generated method
                      this.UpdateSlaveLane(ref navigationLane, targetPosition);
                      navigationLanes[index - 1] = navigationLane;
                    }
                    currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                    break;
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ConnectionLaneData.HasComponent(navLaneData.m_Lane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[navLaneData.m_Lane];
                    navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.FixedLane;
                    if ((connectionLane.m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.Area;
                    else
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.Connection;
                    currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                    if ((connectionLane.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
                    {
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.ParkingSpace;
                      navigationLanes.Add(navLaneData);
                      break;
                    }
                    navigationLanes.Add(navLaneData);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_LaneData.HasComponent(navLaneData.m_Lane))
                    {
                      if (pathOwner.m_ElementIndex >= pathElements.Length && (pathOwner.m_State & PathFlags.Pending) != (PathFlags) 0)
                      {
                        --pathOwner.m_ElementIndex;
                        break;
                      }
                      if (index > 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        float3 targetPosition = MathUtils.Position(this.m_CurveData[navLaneData.m_Lane].m_Bezier, navLaneData.m_CurvePosition.y);
                        CarNavigationLane navigationLane = navigationLanes[index - 1];
                        // ISSUE: reference to a compiler-generated method
                        this.UpdateSlaveLane(ref navigationLane, targetPosition);
                        navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.Waypoint;
                        if (pathOwner.m_ElementIndex >= pathElements.Length)
                          navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath;
                        navigationLanes[index - 1] = navigationLane;
                      }
                      else
                      {
                        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Waypoint;
                        if (pathOwner.m_ElementIndex >= pathElements.Length)
                          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.EndOfPath;
                      }
                      currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                      break;
                    }
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TransformData.HasComponent(navLaneData.m_Lane))
                    {
                      if (pathOwner.m_ElementIndex >= pathElements.Length && (pathOwner.m_State & PathFlags.Pending) != (PathFlags) 0)
                      {
                        --pathOwner.m_ElementIndex;
                        break;
                      }
                      if ((car.m_Flags & CarFlags.StayOnRoad) == (CarFlags) 0 || pathElements.Length > pathOwner.m_ElementIndex)
                      {
                        navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.TransformTarget;
                        navigationLanes.Add(navLaneData);
                        if (index > 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          float3 position = this.m_TransformData[navLaneData.m_Lane].m_Position;
                          CarNavigationLane navigationLane = navigationLanes[index - 1];
                          // ISSUE: reference to a compiler-generated method
                          this.UpdateSlaveLane(ref navigationLane, position);
                          navigationLanes[index - 1] = navigationLane;
                        }
                        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
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
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.CarLane carLane = this.m_CarLaneData[navLaneData.m_Lane];
                  if ((carLane.m_Flags & Game.Net.CarLaneFlags.Forward) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
                  {
                    bool flag1 = (carLane.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.GentleTurnLeft)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
                    bool flag2 = (carLane.m_Flags & (Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
                    if (flag1 & !flag2)
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.TurnLeft;
                    if (flag2 & !flag1)
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.TurnRight;
                  }
                  if ((carLane.m_Flags & (Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout)) == Game.Net.CarLaneFlags.Roundabout)
                    navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.Roundabout;
                  if ((carLane.m_Flags & Game.Net.CarLaneFlags.Twoway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
                    navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.CanReverse;
                  Owner componentData1;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((carLane.m_Flags & Game.Net.CarLaneFlags.Unsafe) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && ((carLane.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.UTurnRight)) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || this.m_OwnerData.TryGetComponent(navLaneData.m_Lane, out componentData1) && this.m_CurveData.HasComponent(componentData1.m_Owner)))
                    navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.RequestSpace;
                  navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                  currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                  if (index == 0)
                  {
                    Game.Net.ParkingLane componentData2;
                    // ISSUE: reference to a compiler-generated field
                    if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ParkingSpace) != (Game.Vehicles.CarLaneFlags) 0 && this.m_ParkingLaneData.TryGetComponent(currentLane.m_Lane, out componentData2))
                    {
                      currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
                      if ((componentData2.m_Flags & ParkingLaneFlags.ParkingRight) != (ParkingLaneFlags) 0)
                        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.TurnLeft;
                      if ((componentData2.m_Flags & ParkingLaneFlags.ParkingLeft) != (ParkingLaneFlags) 0)
                        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.TurnRight;
                    }
                  }
                  else
                  {
                    CarNavigationLane navigationLane = navigationLanes[index - 1];
                    Game.Net.ParkingLane componentData3;
                    // ISSUE: reference to a compiler-generated field
                    if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.ParkingSpace) != (Game.Vehicles.CarLaneFlags) 0 && this.m_ParkingLaneData.TryGetComponent(navigationLane.m_Lane, out componentData3))
                    {
                      navigationLane.m_Flags &= ~(Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
                      if ((componentData3.m_Flags & ParkingLaneFlags.ParkingRight) != (ParkingLaneFlags) 0)
                        navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.TurnLeft;
                      if ((componentData3.m_Flags & ParkingLaneFlags.ParkingLeft) != (ParkingLaneFlags) 0)
                        navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.TurnRight;
                      navigationLanes[index - 1] = navigationLane;
                    }
                  }
                  if (index == 0 && (currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.FixedLane | Game.Vehicles.CarLaneFlags.Connection)) == Game.Vehicles.CarLaneFlags.FixedLane)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GetSlaveLaneFromMasterLane(ref random, ref navLaneData, currentLane);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.GetSlaveLaneFromMasterLane(ref random, ref navLaneData);
                  }
                  if ((pathElement.m_Flags & PathElementFlags.PathStart) != (PathElementFlags) 0)
                  {
                    Entity lane;
                    float prevCurvePos;
                    if (index == 0)
                    {
                      lane = currentLane.m_Lane;
                      prevCurvePos = currentLane.m_CurvePosition.z;
                    }
                    else
                    {
                      lane = navigationLanes[index - 1].m_Lane;
                      prevCurvePos = navigationLanes[index - 1].m_CurvePosition.y;
                    }
                    bool sameLane;
                    // ISSUE: reference to a compiler-generated method
                    if (this.IsContinuous(lane, prevCurvePos, pathElement.m_Target, pathElement.m_TargetDelta.x, out sameLane))
                    {
                      if (sameLane)
                      {
                        if (index == 0)
                        {
                          currentLane.m_CurvePosition.z = pathElement.m_TargetDelta.y;
                          continue;
                        }
                        CarNavigationLane navigationLane = navigationLanes[index - 1];
                        navigationLane.m_CurvePosition.y = pathElement.m_TargetDelta.y;
                        navigationLanes[index - 1] = navigationLane;
                        continue;
                      }
                    }
                    else
                      navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.Interruption;
                  }
                  navigationLanes.Add(navLaneData);
                }
              }
            }
            else
            {
              CarNavigationLane navigationLane = navigationLanes[index];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
              {
                invalidPath = index;
                return;
              }
              if ((navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Waypoint)) != (Game.Vehicles.CarLaneFlags) 0)
                break;
            }
          }
        }
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.UpdateOptimalLane) == (Game.Vehicles.CarLaneFlags) 0)
          return;
        currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.IsBlocked) != (Game.Vehicles.CarLaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.IsBlockedLane(currentLane.m_Lane, currentLane.m_CurvePosition.xz))
          {
            invalidPath = -1;
            return;
          }
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            CarNavigationLane navigationLane = navigationLanes[index];
            // ISSUE: reference to a compiler-generated method
            if (this.IsBlockedLane(navigationLane.m_Lane, navigationLane.m_CurvePosition))
            {
              invalidPath = index;
              return;
            }
          }
          currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.FixedLane | Game.Vehicles.CarLaneFlags.IsBlocked);
          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.IgnoreBlocker;
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
        CarLaneSelectIterator laneSelectIterator = new CarLaneSelectIterator()
        {
          m_OwnerData = this.m_OwnerData,
          m_LaneData = this.m_LaneData,
          m_CarLaneData = this.m_CarLaneData,
          m_SlaveLaneData = this.m_SlaveLaneData,
          m_LaneReservationData = this.m_LaneReservationData,
          m_MovingData = this.m_MovingData,
          m_CarData = this.m_CarData,
          m_ControllerData = this.m_ControllerData,
          m_Lanes = this.m_Lanes,
          m_LaneObjects = this.m_LaneObjects,
          m_Entity = entity,
          m_Blocker = blocker.m_Blocker,
          m_Priority = priority,
          m_ForbidLaneFlags = VehicleUtils.GetForbiddenLaneFlags(car),
          m_PreferLaneFlags = VehicleUtils.GetPreferredLaneFlags(car)
        };
        laneSelectIterator.SetBuffer(ref laneSelectBuffer);
        if (navigationLanes.Length != 0)
        {
          CarNavigationLane carNavigationLane = navigationLanes[navigationLanes.Length - 1];
          laneSelectIterator.CalculateLaneCosts(carNavigationLane, navigationLanes.Length - 1);
          for (int index = navigationLanes.Length - 2; index >= 0; --index)
          {
            CarNavigationLane navigationLane = navigationLanes[index];
            laneSelectIterator.CalculateLaneCosts(navigationLane, carNavigationLane, index);
            carNavigationLane = navigationLane;
          }
          laneSelectIterator.UpdateOptimalLane(ref currentLane, navigationLanes[0]);
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            CarNavigationLane navigationLane = navigationLanes[index];
            laneSelectIterator.UpdateOptimalLane(ref navigationLane);
            navigationLane.m_Flags &= ~Game.Vehicles.CarLaneFlags.Reserved;
            navigationLanes[index] = navigationLane;
          }
        }
        else
        {
          if ((double) currentLane.m_CurvePosition.x == (double) currentLane.m_CurvePosition.z)
            return;
          laneSelectIterator.UpdateOptimalLane(ref currentLane, new CarNavigationLane());
        }
      }

      private bool IsContinuous(
        Entity prevLane,
        float prevCurvePos,
        Entity pathTarget,
        float nextCurvePos,
        out bool sameLane)
      {
        sameLane = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLaneData.HasComponent(prevLane))
        {
          // ISSUE: reference to a compiler-generated field
          SlaveLane slaveLane = this.m_SlaveLaneData[prevLane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          prevLane = this.m_Lanes[this.m_OwnerData[prevLane].m_Owner][(int) slaveLane.m_MasterIndex].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_MasterLaneData.HasComponent(prevLane))
            return false;
        }
        if (prevLane == pathTarget && (double) prevCurvePos == (double) nextCurvePos)
        {
          sameLane = true;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_LaneData.HasComponent(prevLane) && this.m_LaneData.HasComponent(pathTarget) && this.m_LaneData[prevLane].m_EndNode.Equals(this.m_LaneData[pathTarget].m_StartNode);
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
          lane = this.m_Lanes[this.m_OwnerData[lane].m_Owner][(int) slaveLane.m_MasterIndex].m_SubLane;
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
        if (this.m_TransformData.HasComponent(target.m_Target))
        {
          entity = target.m_Target;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PositionData.HasComponent(target.m_Target))
          return false;
        entity = target.m_Target;
        return true;
      }

      private void UpdateSlaveLane(ref CarNavigationLane navLaneData, float3 targetPosition)
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
        navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.FixedLane;
      }

      private void GetSlaveLaneFromMasterLane(
        ref Unity.Mathematics.Random random,
        ref CarNavigationLane navLaneData,
        CarCurrentLane currentLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_MasterLaneData.HasComponent(navLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          MasterLane masterLane = this.m_MasterLaneData[navLaneData.m_Lane];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
          if ((currentLaneData.m_LaneFlags & Game.Vehicles.CarLaneFlags.TransformTarget) != (Game.Vehicles.CarLaneFlags) 0)
          {
            float3 position = new float3();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (VehicleUtils.CalculateTransformPosition(ref position, currentLaneData.m_Lane, this.m_TransformData, this.m_PositionData, this.m_PrefabRefData, this.m_PrefabBuildingData))
            {
              // ISSUE: reference to a compiler-generated field
              int index = NetUtils.ChooseClosestLane((int) masterLane.m_MinIndex, (int) masterLane.m_MaxIndex, position, lane, this.m_CurveData, navLaneData.m_CurvePosition.y);
              navLaneData.m_Lane = lane[index].m_SubLane;
              navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.FixedStart;
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
            navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.FixedStart;
          }
        }
        else
          navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.FixedLane;
      }

      private void GetSlaveLaneFromMasterLane(ref Unity.Mathematics.Random random, ref CarNavigationLane navLaneData)
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
          navLaneData.m_Flags |= Game.Vehicles.CarLaneFlags.FixedLane;
      }

      private bool GetNextLane(Entity prevLane, Entity nextLane, out Entity selectedLane)
      {
        SlaveLane componentData1;
        Lane componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLaneData.TryGetComponent(nextLane, out componentData1) && this.m_LaneData.TryGetComponent(prevLane, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[this.m_OwnerData[nextLane].m_Owner];
          int num = math.min((int) componentData1.m_MaxIndex, lane.Length - 1);
          for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num; ++minIndex)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneData[lane[minIndex].m_SubLane].m_StartNode.Equals(componentData2.m_EndNode))
            {
              selectedLane = lane[minIndex].m_SubLane;
              return true;
            }
          }
        }
        selectedLane = Entity.Null;
        return false;
      }

      private void CheckBlocker(
        ref CarCurrentLane currentLane,
        ref Blocker blocker,
        ref CarLaneSpeedIterator laneIterator)
      {
        if (laneIterator.m_Blocker != blocker.m_Blocker)
          currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.IgnoreBlocker | Game.Vehicles.CarLaneFlags.QueueReached);
        if (laneIterator.m_Blocker != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_MovingData.HasComponent(laneIterator.m_Blocker))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarData.HasComponent(laneIterator.m_Blocker))
            {
              // ISSUE: reference to a compiler-generated field
              if ((this.m_CarData[laneIterator.m_Blocker].m_Flags & CarFlags.Queueing) != (CarFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Queue) != (Game.Vehicles.CarLaneFlags) 0)
              {
                if ((double) laneIterator.m_MaxSpeed <= 3.0)
                  currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.QueueReached;
              }
              else
              {
                currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
                if ((double) laneIterator.m_MaxSpeed <= 3.0)
                  currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.IsBlocked;
              }
            }
            else
            {
              currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
              if ((double) laneIterator.m_MaxSpeed <= 3.0)
                currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.IsBlocked;
            }
          }
          else if (laneIterator.m_Blocker != blocker.m_Blocker)
            currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.UpdateOptimalLane;
        }
        blocker.m_Blocker = laneIterator.m_Blocker;
        blocker.m_Type = laneIterator.m_BlockerType;
        blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(laneIterator.m_MaxSpeed * 2.29499984f), 0, (int) byte.MaxValue);
      }

      private void UpdateTrailer(
        Entity entity,
        Game.Objects.Transform transform,
        ObjectGeometryData prefabObjectGeometryData,
        Entity nextLane,
        float2 nextPosition,
        bool forceNext,
        ref CarTrailerLane trailerLane)
      {
        if (forceNext)
        {
          trailerLane.m_Lane = nextLane;
          trailerLane.m_CurvePosition = nextPosition;
          trailerLane.m_NextLane = Entity.Null;
          trailerLane.m_NextPosition = new float2();
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CurveData.HasComponent(nextLane))
            return;
          // ISSUE: reference to a compiler-generated field
          double num = (double) MathUtils.Distance(this.m_CurveData[nextLane].m_Bezier, transform.m_Position, out trailerLane.m_CurvePosition.x);
        }
        else
        {
          if (nextLane != Entity.Null)
          {
            if (trailerLane.m_Lane == nextLane)
            {
              trailerLane.m_CurvePosition.y = nextPosition.y;
              trailerLane.m_NextLane = Entity.Null;
              trailerLane.m_NextPosition = new float2();
              nextLane = Entity.Null;
              nextPosition = new float2();
            }
            else if (trailerLane.m_NextLane == nextLane)
            {
              trailerLane.m_NextPosition.y = nextPosition.y;
              nextLane = Entity.Null;
              nextPosition = new float2();
            }
            else if (trailerLane.m_NextLane == Entity.Null)
            {
              trailerLane.m_NextLane = nextLane;
              trailerLane.m_NextPosition = nextPosition;
              nextLane = Entity.Null;
              nextPosition = new float2();
            }
          }
          float3 maxValue = (float3) float.MaxValue;
          float3 float3 = new float3();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(trailerLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[trailerLane.m_Lane];
            maxValue.x = MathUtils.Distance(curve.m_Bezier, transform.m_Position, out float3.x);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(trailerLane.m_NextLane))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[trailerLane.m_NextLane];
            maxValue.y = MathUtils.Distance(curve.m_Bezier, transform.m_Position, out float3.y);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(nextLane))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[nextLane];
            maxValue.z = MathUtils.Distance(curve.m_Bezier, transform.m_Position, out float3.z);
          }
          if (math.all(maxValue.z < maxValue.xy) | forceNext)
          {
            trailerLane.m_Lane = nextLane;
            trailerLane.m_CurvePosition = new float2(float3.z, nextPosition.y);
            trailerLane.m_NextLane = Entity.Null;
            trailerLane.m_NextPosition = new float2();
          }
          else if ((double) maxValue.y < (double) maxValue.x)
          {
            trailerLane.m_Lane = trailerLane.m_NextLane;
            trailerLane.m_CurvePosition = new float2(float3.y, trailerLane.m_NextPosition.y);
            trailerLane.m_NextLane = nextLane;
            trailerLane.m_NextPosition = nextPosition;
          }
          else
            trailerLane.m_CurvePosition.x = float3.x;
        }
      }

      private void UpdateNavigationTarget(
        ref Unity.Mathematics.Random random,
        int priority,
        Entity entity,
        Game.Objects.Transform transform,
        Moving moving,
        Car car,
        PseudoRandomSeed pseudoRandomSeed,
        PrefabRef prefabRef,
        CarData prefabCarData,
        ObjectGeometryData prefabObjectGeometryData,
        ref CarNavigation navigation,
        ref CarCurrentLane currentLane,
        ref Blocker blocker,
        ref Odometer odometer,
        ref PathOwner pathOwner,
        ref NativeList<Entity> tempBuffer,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements)
      {
        float timeStep = 0.266666681f;
        float num1 = math.length(moving.m_Velocity);
        float speedLimitFactor = VehicleUtils.GetSpeedLimitFactor(car);
        float safetyTime;
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.GetDrivingStyle(this.m_SimulationFrame, pseudoRandomSeed, out safetyTime);
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Connection) != (Game.Vehicles.CarLaneFlags) 0)
        {
          prefabCarData.m_MaxSpeed = 277.777771f;
          prefabCarData.m_Acceleration = 277.777771f;
          prefabCarData.m_Braking = 277.777771f;
        }
        else
        {
          num1 = math.min(num1, prefabCarData.m_MaxSpeed);
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Obsolete)) != (PathFlags) 0)
            prefabCarData.m_Acceleration = 0.0f;
        }
        Bounds1 bounds1 = (currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.Connection | Game.Vehicles.CarLaneFlags.ResetSpeed)) == (Game.Vehicles.CarLaneFlags) 0 ? VehicleUtils.CalculateSpeedRange(prefabCarData, num1, timeStep) : new Bounds1(0.0f, prefabCarData.m_MaxSpeed);
        bool flag1 = blocker.m_Type == BlockerType.Temporary;
        bool c1 = math.asuint(navigation.m_MaxSpeed) >> 31 > 0U;
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
        CarLaneSpeedIterator laneIterator = new CarLaneSpeedIterator()
        {
          m_TransformData = this.m_TransformData,
          m_MovingData = this.m_MovingData,
          m_CarData = this.m_CarData,
          m_TrainData = this.m_TrainData,
          m_ControllerData = this.m_ControllerData,
          m_LaneReservationData = this.m_LaneReservationData,
          m_LaneConditionData = this.m_LaneConditionData,
          m_LaneSignalData = this.m_LaneSignalData,
          m_CurveData = this.m_CurveData,
          m_CarLaneData = this.m_CarLaneData,
          m_ParkingLaneData = this.m_ParkingLaneData,
          m_UnspawnedData = this.m_UnspawnedData,
          m_CreatureData = this.m_CreatureData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
          m_PrefabCarData = this.m_PrefabCarData,
          m_PrefabTrainData = this.m_PrefabTrainData,
          m_PrefabParkingLaneData = this.m_PrefabParkingLaneData,
          m_LaneOverlapData = this.m_LaneOverlaps,
          m_LaneObjectData = this.m_LaneObjects,
          m_Entity = entity,
          m_Ignore = (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.IgnoreBlocker) != (Game.Vehicles.CarLaneFlags) 0 ? blocker.m_Blocker : Entity.Null,
          m_TempBuffer = tempBuffer,
          m_Priority = priority,
          m_TimeStep = timeStep,
          m_SafeTimeStep = timeStep + safetyTime,
          m_DistanceOffset = math.select(0.0f, math.max(-0.5f, -0.5f * math.lengthsq(1.5f - num1)), (double) num1 < 1.5),
          m_SpeedLimitFactor = speedLimitFactor,
          m_CurrentSpeed = num1,
          m_PrefabCar = prefabCarData,
          m_PrefabObjectGeometry = prefabObjectGeometryData,
          m_SpeedRange = bounds1,
          m_PushBlockers = (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.PushBlockers) > (Game.Vehicles.CarLaneFlags) 0,
          m_MaxSpeed = bounds1.max,
          m_CanChangeLane = 1f,
          m_CurrentPosition = transform.m_Position
        };
        Game.Vehicles.CarLaneFlags carLaneFlags1 = (Game.Vehicles.CarLaneFlags) 0;
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.TransformTarget | Game.Vehicles.CarLaneFlags.ParkingSpace)) != (Game.Vehicles.CarLaneFlags) 0)
        {
          laneIterator.IterateTarget(navigation.m_TargetPosition);
          navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
          blocker.m_Blocker = Entity.Null;
          blocker.m_Type = BlockerType.None;
          blocker.m_MaxSpeed = byte.MaxValue;
        }
        else if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Area) != (Game.Vehicles.CarLaneFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          navigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, navigation.m_TargetPosition);
          laneIterator.IterateTarget(navigation.m_TargetPosition, 11.1111116f);
          navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
          blocker.m_Blocker = Entity.Null;
          blocker.m_Type = BlockerType.None;
          blocker.m_MaxSpeed = byte.MaxValue;
        }
        else
        {
          if (currentLane.m_Lane == Entity.Null)
          {
            navigation.m_MaxSpeed = math.max(0.0f, num1 - prefabCarData.m_Braking * timeStep);
            blocker.m_Blocker = Entity.Null;
            blocker.m_Type = BlockerType.None;
            blocker.m_MaxSpeed = byte.MaxValue;
            return;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetLaneData prefabLaneData1 = this.m_PrefabLaneData[this.m_PrefabRefData[currentLane.m_Lane].m_Prefab];
          float laneOffset1 = VehicleUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData1, currentLane.m_LanePosition);
          if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.HighBeams) != (Game.Vehicles.CarLaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.m_CarLaneData.HasComponent(currentLane.m_Lane) || !this.AllowHighBeams(transform, blocker, ref currentLane, navigationLanes, 100f, 2f))
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.HighBeams;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (this.m_CarLaneData.HasComponent(currentLane.m_Lane) && (this.m_CarLaneData[currentLane.m_Lane].m_Flags & Game.Net.CarLaneFlags.Highway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && !this.IsLit(currentLane.m_Lane) && this.AllowHighBeams(transform, blocker, ref currentLane, navigationLanes, 150f, 0.0f))
              currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.HighBeams;
          }
          Entity lane = Entity.Null;
          float2 nextOffset = (float2) 0.0f;
          if (navigationLanes.Length > 0)
          {
            CarNavigationLane navigationLane = navigationLanes[0];
            lane = navigationLane.m_Lane;
            nextOffset = navigationLane.m_CurvePosition;
          }
          Game.Net.CarLaneFlags laneFlags;
          if (currentLane.m_ChangeLane != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetLaneData prefabLaneData2 = this.m_PrefabLaneData[this.m_PrefabRefData[currentLane.m_ChangeLane].m_Prefab];
            float laneOffset2 = VehicleUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData2, -currentLane.m_LanePosition);
            if (laneIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_ChangeLane, currentLane.m_CurvePosition, lane, nextOffset, currentLane.m_ChangeProgress, laneOffset1, laneOffset2, (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.RequestSpace) > (Game.Vehicles.CarLaneFlags) 0, out laneFlags))
              goto label_36;
          }
          else if (laneIterator.IterateFirstLane(currentLane.m_Lane, currentLane.m_CurvePosition, lane, nextOffset, laneOffset1, (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.RequestSpace) > (Game.Vehicles.CarLaneFlags) 0, out laneFlags))
            goto label_36;
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            CarNavigationLane navigationLane = navigationLanes[index];
            if ((navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.TransformTarget | Game.Vehicles.CarLaneFlags.Area)) == (Game.Vehicles.CarLaneFlags) 0)
            {
              if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Connection) != (Game.Vehicles.CarLaneFlags) 0)
              {
                laneIterator.m_PrefabCar.m_MaxSpeed = 277.777771f;
                laneIterator.m_PrefabCar.m_Acceleration = 277.777771f;
                laneIterator.m_PrefabCar.m_Braking = 277.777771f;
                laneIterator.m_SpeedRange = new Bounds1(0.0f, 277.777771f);
              }
              else if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Connection) == (Game.Vehicles.CarLaneFlags) 0)
              {
                if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Interruption) != (Game.Vehicles.CarLaneFlags) 0)
                  laneIterator.m_PrefabCar.m_MaxSpeed = 3f;
              }
              else
                break;
              if ((index == 0 || (navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Roundabout)) == (Game.Vehicles.CarLaneFlags) 0) && carLaneFlags1 == (Game.Vehicles.CarLaneFlags) 0 && (navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Validated)) != Game.Vehicles.CarLaneFlags.ParkingSpace)
                carLaneFlags1 |= navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
              bool c2 = navigationLane.m_Lane == currentLane.m_Lane | navigationLane.m_Lane == currentLane.m_ChangeLane;
              float minOffset = math.select(math.select(-1f, 2f, (double) navigationLane.m_CurvePosition.y < (double) navigationLane.m_CurvePosition.x), currentLane.m_CurvePosition.y, c2);
              bool needSignal;
              int num2 = laneIterator.IterateNextLane(navigationLane.m_Lane, navigationLane.m_CurvePosition, minOffset, navigationLanes.AsNativeArray().GetSubArray(index + 1, navigationLanes.Length - 1 - index), (navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.RequestSpace) > (Game.Vehicles.CarLaneFlags) 0, ref laneFlags, out needSignal) ? 1 : 0;
              if (needSignal)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LaneSignals.Enqueue(new CarNavigationHelpers.LaneSignal(entity, navigationLane.m_Lane, priority));
              }
              if (num2 != 0)
                goto label_36;
            }
            else
              break;
          }
          laneIterator.IterateTarget(laneIterator.m_CurrentPosition);
label_36:
          navigation.m_MaxSpeed = laneIterator.m_MaxSpeed;
          // ISSUE: reference to a compiler-generated method
          this.CheckBlocker(ref currentLane, ref blocker, ref laneIterator);
          if (laneIterator.m_TempBuffer.IsCreated)
          {
            tempBuffer = laneIterator.m_TempBuffer;
            tempBuffer.Clear();
          }
        }
        float z = math.select(prefabCarData.m_PivotOffset, -prefabCarData.m_PivotOffset, c1);
        float3 position = transform.m_Position;
        if ((double) z < 0.0)
        {
          position += math.rotate(transform.m_Rotation, new float3(0.0f, 0.0f, z));
          z = -z;
        }
        float a = math.lerp(math.distance(position, navigation.m_TargetPosition), 0.0f, laneIterator.m_Oncoming);
        float num3 = math.max(1f, navigation.m_MaxSpeed * timeStep) + z;
        float minDistance = num3;
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Area) != (Game.Vehicles.CarLaneFlags) 0)
        {
          float brakingDistance = VehicleUtils.GetBrakingDistance(prefabCarData, math.min(prefabCarData.m_MaxSpeed, 11.1111116f), timeStep);
          minDistance = math.max(num3, brakingDistance + 1f + z);
          a = math.select(a, 0.0f, (double) currentLane.m_ChangeProgress != 0.0);
        }
        if (currentLane.m_ChangeLane != Entity.Null)
        {
          float num4 = 0.05f;
          float x1 = (float) (1.0 + (double) prefabObjectGeometryData.m_Bounds.max.z * (double) num4);
          float2 x2 = new float2(0.4f, 0.6f * math.saturate(num1 * num4)) * (laneIterator.m_CanChangeLane * timeStep);
          x2.x = math.min(x2.x, math.max(0.0f, 1f - currentLane.m_ChangeProgress));
          currentLane.m_ChangeProgress = math.min(x1, currentLane.m_ChangeProgress + math.csum(x2));
          if ((double) currentLane.m_ChangeProgress == (double) x1 || (currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.Connection | Game.Vehicles.CarLaneFlags.ResetSpeed)) != (Game.Vehicles.CarLaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.ApplySideEffects(ref currentLane, speedLimitFactor, prefabRef, prefabCarData);
            currentLane.m_LanePosition = -currentLane.m_LanePosition;
            currentLane.m_Lane = currentLane.m_ChangeLane;
            currentLane.m_ChangeLane = Entity.Null;
            currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
          }
        }
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight)) == (Game.Vehicles.CarLaneFlags) 0)
          currentLane.m_LaneFlags |= carLaneFlags1;
        int num5 = blocker.m_Type == BlockerType.Temporary ? 1 : 0;
        if (num5 != (flag1 ? 1 : 0) || (double) currentLane.m_Duration >= 30.0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ApplySideEffects(ref currentLane, speedLimitFactor, prefabRef, prefabCarData);
        }
        if (num5 != 0)
        {
          if ((double) currentLane.m_Duration >= 5.0)
            currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.PushBlockers;
        }
        else if ((double) currentLane.m_Duration >= 5.0)
          currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.PushBlockers;
        currentLane.m_Duration += timeStep;
        if ((double) num1 > 0.0099999997764825821)
        {
          float num6 = num1 * timeStep;
          currentLane.m_Distance += num6;
          odometer.m_Distance += num6;
          Game.Vehicles.CarLaneFlags carLaneFlags2 = currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
          float4 float4 = math.select(new float4(-0.5f, 0.5f, 1f / 500f, 0.1f), new float4(0.0f, 0.0f, 0.01f, 0.1f), new bool4(carLaneFlags2 == Game.Vehicles.CarLaneFlags.TurnRight, carLaneFlags2 == Game.Vehicles.CarLaneFlags.TurnLeft, carLaneFlags2 > (Game.Vehicles.CarLaneFlags) 0, true));
          float4.zw = math.min((float2) 1f, num6 * float4.zw);
          currentLane.m_LanePosition -= (math.max(0.0f, currentLane.m_LanePosition - 0.5f) + math.min(0.0f, currentLane.m_LanePosition + 0.5f)) * float4.w;
          currentLane.m_LanePosition = math.lerp(currentLane.m_LanePosition, random.NextFloat(float4.x, float4.y), float4.z);
        }
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ResetSpeed) != (Game.Vehicles.CarLaneFlags) 0)
        {
          if ((double) currentLane.m_Distance > 10.0 + (double) num1 * 0.5)
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.ResetSpeed;
            currentLane.m_Distance = 0.0f;
            currentLane.m_Duration = 0.0f;
          }
          else if ((double) currentLane.m_Duration > 60.0)
          {
            blocker.m_Blocker = entity;
            blocker.m_Type = BlockerType.Spawn;
          }
        }
        if ((double) a < (double) minDistance)
        {
          NetLaneData prefabLaneData3;
          NetLaneData prefabLaneData4;
          while (true)
          {
            if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ParkingSpace) != (Game.Vehicles.CarLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve1 = this.m_CurveData[currentLane.m_Lane];
              currentLane.m_CurvePosition.x = currentLane.m_CurvePosition.z;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkingLaneData.HasComponent(currentLane.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.ParkingLane parkingLane = this.m_ParkingLaneData[currentLane.m_Lane];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ParkingLaneData parkingLaneData1 = this.m_PrefabParkingLaneData[this.m_PrefabRefData[currentLane.m_Lane].m_Prefab];
                Game.Objects.Transform transform1 = new Game.Objects.Transform();
                Owner componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(currentLane.m_Lane, out componentData) && this.m_TransformData.HasComponent(componentData.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  transform1 = this.m_TransformData[componentData.m_Owner];
                }
                ParkingLaneData parkingLaneData2 = parkingLaneData1;
                ObjectGeometryData prefabGeometryData = prefabObjectGeometryData;
                Curve curve2 = curve1;
                Game.Objects.Transform ownerTransform = transform1;
                double x = (double) currentLane.m_CurvePosition.x;
                Game.Objects.Transform parkingSpaceTarget = VehicleUtils.CalculateParkingSpaceTarget(parkingLane, parkingLaneData2, prefabGeometryData, curve2, ownerTransform, (float) x);
                navigation.m_TargetPosition = parkingSpaceTarget.m_Position;
                navigation.m_TargetRotation = parkingSpaceTarget.m_Rotation;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[currentLane.m_Lane];
                navigation.m_TargetPosition = VehicleUtils.GetConnectionParkingPosition(connectionLane, curve1.m_Bezier, currentLane.m_CurvePosition.x);
                navigation.m_TargetRotation = quaternion.LookRotationSafe(MathUtils.Tangent(curve1.m_Bezier, currentLane.m_CurvePosition.x), math.up());
              }
              if ((double) math.distance(position, navigation.m_TargetPosition) >= 1.0 + (double) z)
                navigation.m_TargetRotation = new quaternion();
            }
            else if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.TransformTarget) != (Game.Vehicles.CarLaneFlags) 0)
            {
              bool flag2 = false;
              if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ResetSpeed) != (Game.Vehicles.CarLaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                quaternion navigationRotation = this.CalculateNavigationRotation(currentLane.m_Lane, navigationLanes);
                flag2 = !navigationRotation.Equals(navigation.m_TargetRotation);
                navigation.m_TargetRotation = navigationRotation;
              }
              else
                navigation.m_TargetRotation = new quaternion();
              // ISSUE: reference to a compiler-generated method
              if (this.MoveTarget(position, ref navigation.m_TargetPosition, num3, currentLane.m_Lane) | flag2)
                goto label_104;
            }
            else if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Area) != (Game.Vehicles.CarLaneFlags) 0)
            {
              navigation.m_TargetRotation = new quaternion();
              float navigationSize = VehicleUtils.GetNavigationSize(prefabObjectGeometryData);
              // ISSUE: reference to a compiler-generated method
              int num7 = this.MoveAreaTarget(ref random, transform.m_Position, pathOwner, navigationLanes, pathElements, ref navigation.m_TargetPosition, minDistance, currentLane.m_Lane, ref currentLane.m_CurvePosition, currentLane.m_LanePosition, navigationSize) ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              navigation.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, navigation.m_TargetPosition);
              currentLane.m_ChangeProgress = 0.0f;
              if (num7 != 0)
                goto label_104;
            }
            else
            {
              navigation.m_TargetRotation = new quaternion();
              if (currentLane.m_ChangeLane != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve3 = this.m_CurveData[currentLane.m_Lane];
                // ISSUE: reference to a compiler-generated field
                Curve curve4 = this.m_CurveData[currentLane.m_ChangeLane];
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef1 = this.m_PrefabRefData[currentLane.m_Lane];
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef2 = this.m_PrefabRefData[currentLane.m_ChangeLane];
                // ISSUE: reference to a compiler-generated field
                prefabLaneData3 = this.m_PrefabLaneData[prefabRef1.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                NetLaneData prefabLaneData5 = this.m_PrefabLaneData[prefabRef2.m_Prefab];
                float laneOffset3 = VehicleUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData3, currentLane.m_LanePosition);
                float laneOffset4 = VehicleUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData5, -currentLane.m_LanePosition);
                // ISSUE: reference to a compiler-generated method
                if (this.MoveTarget(position, ref navigation.m_TargetPosition, num3, curve3.m_Bezier, curve4.m_Bezier, currentLane.m_ChangeProgress, ref currentLane.m_CurvePosition, laneOffset3, laneOffset4))
                  break;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[currentLane.m_Lane];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                prefabLaneData4 = this.m_PrefabLaneData[this.m_PrefabRefData[currentLane.m_Lane].m_Prefab];
                float num8 = VehicleUtils.GetLaneOffset(prefabObjectGeometryData, prefabLaneData4, currentLane.m_LanePosition);
                if ((double) laneIterator.m_Oncoming != 0.0)
                {
                  float num9 = prefabObjectGeometryData.m_Bounds.max.x - prefabObjectGeometryData.m_Bounds.min.x;
                  // ISSUE: reference to a compiler-generated field
                  float b = math.lerp(num8, num9 * math.select(0.5f, -0.5f, this.m_LeftHandTraffic), math.min(1f, laneIterator.m_Oncoming));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num8 = math.select(num8, b, !this.m_LeftHandTraffic & (double) b > (double) num8 | this.m_LeftHandTraffic & (double) b < (double) num8);
                  currentLane.m_LanePosition = num8 / math.max(0.1f, prefabLaneData4.m_Width - num9);
                }
                float laneOffset = math.select(num8, -num8, (double) currentLane.m_CurvePosition.z < (double) currentLane.m_CurvePosition.x);
                // ISSUE: reference to a compiler-generated method
                if (this.MoveTarget(position, ref navigation.m_TargetPosition, num3, curve.m_Bezier, ref currentLane.m_CurvePosition, laneOffset))
                  goto label_86;
              }
            }
            if (navigationLanes.Length != 0)
            {
              CarNavigationLane navigationLane = navigationLanes[0];
              // ISSUE: reference to a compiler-generated field
              if ((navigationLane.m_Flags & (Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Validated)) != Game.Vehicles.CarLaneFlags.ParkingSpace && this.m_PrefabRefData.HasComponent(navigationLane.m_Lane))
              {
                if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Connection) != (Game.Vehicles.CarLaneFlags) 0)
                {
                  if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.TransformTarget) != (Game.Vehicles.CarLaneFlags) 0)
                    navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.ResetSpeed;
                  else if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Connection) == (Game.Vehicles.CarLaneFlags) 0)
                  {
                    if ((double) math.distance(position, navigation.m_TargetPosition) < 1.0 + (double) z && (double) num1 <= 3.0)
                      navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.ResetSpeed;
                    else
                      goto label_104;
                  }
                }
                Game.Net.CarLane componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.HighBeams) != (Game.Vehicles.CarLaneFlags) 0 && this.m_CarLaneData.TryGetComponent(navigationLane.m_Lane, out componentData) && (componentData.m_Flags & Game.Net.CarLaneFlags.Highway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && !this.IsLit(navigationLane.m_Lane))
                  navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.HighBeams;
                // ISSUE: reference to a compiler-generated method
                this.ApplySideEffects(ref currentLane, speedLimitFactor, prefabRef, prefabCarData);
                Entity selectedLane;
                // ISSUE: reference to a compiler-generated method
                if (currentLane.m_ChangeLane != Entity.Null && this.GetNextLane(currentLane.m_Lane, navigationLane.m_Lane, out selectedLane) && selectedLane != navigationLane.m_Lane)
                {
                  currentLane.m_Lane = selectedLane;
                  currentLane.m_ChangeLane = navigationLane.m_Lane;
                }
                else
                {
                  currentLane.m_Lane = navigationLane.m_Lane;
                  currentLane.m_ChangeLane = Entity.Null;
                  currentLane.m_ChangeProgress = 0.0f;
                }
                currentLane.m_CurvePosition = navigationLane.m_CurvePosition.xxy;
                currentLane.m_LaneFlags = navigationLane.m_Flags | currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.PushBlockers;
                navigationLanes.RemoveAt(0);
              }
              else
                goto label_104;
            }
            else
              goto label_89;
          }
          if ((prefabLaneData3.m_Flags & Game.Prefabs.LaneFlags.Twoway) == (Game.Prefabs.LaneFlags) 0)
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.CanReverse;
            goto label_104;
          }
          else
            goto label_104;
label_86:
          if ((prefabLaneData4.m_Flags & Game.Prefabs.LaneFlags.Twoway) == (Game.Prefabs.LaneFlags) 0)
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.CanReverse;
            goto label_104;
          }
          else
            goto label_104;
label_89:
          if ((double) math.distance(position, navigation.m_TargetPosition) < 1.0 + (double) z && (double) num1 < 0.10000000149011612)
            currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.EndReached;
        }
label_104:
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Area) != (Game.Vehicles.CarLaneFlags) 0)
        {
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
          VehicleCollisionIterator collisionIterator = new VehicleCollisionIterator()
          {
            m_OwnerData = this.m_OwnerData,
            m_TransformData = this.m_TransformData,
            m_MovingData = this.m_MovingData,
            m_ControllerData = this.m_ControllerData,
            m_CreatureData = this.m_CreatureData,
            m_CurveData = this.m_CurveData,
            m_AreaLaneData = this.m_AreaLaneData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
            m_PrefabLaneData = this.m_PrefabLaneData,
            m_AreaNodes = this.m_AreaNodes,
            m_StaticObjectSearchTree = this.m_StaticObjectSearchTree,
            m_MovingObjectSearchTree = this.m_MovingObjectSearchTree,
            m_TerrainHeightData = this.m_TerrainHeightData,
            m_Entity = entity,
            m_CurrentLane = currentLane.m_Lane,
            m_CurvePosition = currentLane.m_CurvePosition.z,
            m_TimeStep = timeStep,
            m_PrefabObjectGeometry = prefabObjectGeometryData,
            m_SpeedRange = bounds1,
            m_CurrentPosition = transform.m_Position,
            m_CurrentVelocity = moving.m_Velocity,
            m_MinDistance = minDistance,
            m_TargetPosition = navigation.m_TargetPosition,
            m_MaxSpeed = navigation.m_MaxSpeed,
            m_LanePosition = currentLane.m_LanePosition,
            m_Blocker = blocker.m_Blocker,
            m_BlockerType = blocker.m_Type
          };
          if ((double) collisionIterator.m_MaxSpeed != 0.0 && !c1)
          {
            collisionIterator.IterateFirstLane(currentLane.m_Lane);
            collisionIterator.m_MaxSpeed = math.select(collisionIterator.m_MaxSpeed, 0.0f, (double) collisionIterator.m_MaxSpeed < 0.10000000149011612);
            if (!navigation.m_TargetPosition.Equals(collisionIterator.m_TargetPosition))
            {
              navigation.m_TargetPosition = collisionIterator.m_TargetPosition;
              currentLane.m_LanePosition = math.lerp(currentLane.m_LanePosition, collisionIterator.m_LanePosition, 0.1f);
              currentLane.m_ChangeProgress = 1f;
            }
            navigation.m_MaxSpeed = collisionIterator.m_MaxSpeed;
            blocker.m_Blocker = collisionIterator.m_Blocker;
            blocker.m_Type = collisionIterator.m_BlockerType;
            blocker.m_MaxSpeed = (byte) math.clamp(Mathf.RoundToInt(collisionIterator.m_MaxSpeed * 2.29499984f), 0, (int) byte.MaxValue);
          }
          navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, math.distance(transform.m_Position.xz, navigation.m_TargetPosition.xz) / timeStep);
        }
        else
          navigation.m_MaxSpeed = math.min(navigation.m_MaxSpeed, math.distance(transform.m_Position, navigation.m_TargetPosition) / timeStep);
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.Connection | Game.Vehicles.CarLaneFlags.ResetSpeed)) != (Game.Vehicles.CarLaneFlags) 0)
          return;
        float3 x3 = navigation.m_TargetPosition - position;
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Area) != (Game.Vehicles.CarLaneFlags) 0)
        {
          x3.xz = MathUtils.ClampLength(x3.xz, num3);
          // ISSUE: reference to a compiler-generated field
          x3.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, position + x3) - position.y;
        }
        float num10 = math.length(x3);
        float3 x4 = math.forward(transform.m_Rotation);
        if (c1)
        {
          if ((double) num10 < 1.0 + (double) z || (double) math.dot(x4, math.normalizesafe(x3)) < 0.800000011920929)
          {
            navigation.m_MaxSpeed = -math.min(3f, navigation.m_MaxSpeed);
          }
          else
          {
            if ((double) num1 < 0.10000000149011612)
              return;
            navigation.m_MaxSpeed = -math.max(0.0f, math.min(navigation.m_MaxSpeed, num1 - prefabCarData.m_Braking * timeStep));
          }
        }
        else
        {
          if ((double) num10 < 1.0 + (double) z || !(currentLane.m_ChangeLane == Entity.Null) || (double) math.dot(x4, math.normalizesafe(x3)) >= 0.699999988079071)
            return;
          if ((double) num1 >= 0.10000000149011612)
          {
            navigation.m_MaxSpeed = math.max(0.0f, math.min(navigation.m_MaxSpeed, num1 - prefabCarData.m_Braking * timeStep));
          }
          else
          {
            navigation.m_MaxSpeed = -math.min(3f, navigation.m_MaxSpeed);
            if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Area) != (Game.Vehicles.CarLaneFlags) 0)
              return;
            currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.CanReverse;
          }
        }
      }

      private quaternion CalculateNavigationRotation(
        Entity sourceLocation,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        float3 float3_1 = new float3();
        bool flag = false;
        Game.Objects.Transform componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.TryGetComponent(sourceLocation, out componentData1))
        {
          float3_1 = componentData1.m_Position;
          flag = true;
        }
        for (int index = 0; index < navigationLanes.Length; ++index)
        {
          CarNavigationLane navigationLane = navigationLanes[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.TryGetComponent(navigationLane.m_Lane, out componentData1))
          {
            if (flag)
            {
              float3 forward = componentData1.m_Position - float3_1;
              if (MathUtils.TryNormalize(ref forward))
                return quaternion.LookRotationSafe(forward, math.up());
            }
            else
            {
              float3_1 = componentData1.m_Position;
              flag = true;
            }
          }
          else
          {
            Curve componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.TryGetComponent(navigationLane.m_Lane, out componentData2))
            {
              float3 float3_2 = MathUtils.Position(componentData2.m_Bezier, navigationLane.m_CurvePosition.x);
              if (flag)
              {
                float3 forward = float3_2 - float3_1;
                if (MathUtils.TryNormalize(ref forward))
                  return quaternion.LookRotationSafe(forward, math.up());
              }
              else
              {
                float3_1 = float3_2;
                flag = true;
              }
              if ((double) navigationLane.m_CurvePosition.x != (double) navigationLane.m_CurvePosition.y)
              {
                float3 a = MathUtils.Tangent(componentData2.m_Bezier, navigationLane.m_CurvePosition.x);
                float3 forward = math.select(a, -a, (double) navigationLane.m_CurvePosition.y < (double) navigationLane.m_CurvePosition.x);
                if (MathUtils.TryNormalize(ref forward))
                  return quaternion.LookRotationSafe(forward, math.up());
              }
            }
          }
        }
        return new quaternion();
      }

      private bool IsLit(Entity lane)
      {
        Owner componentData1;
        Road componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_OwnerData.TryGetComponent(lane, out componentData1) && this.m_RoadData.TryGetComponent(componentData1.m_Owner, out componentData2) && (componentData2.m_Flags & (Game.Net.RoadFlags.IsLit | Game.Net.RoadFlags.LightsOff)) == Game.Net.RoadFlags.IsLit;
      }

      private float CalculateNoise(
        ref CarCurrentLane currentLaneData,
        PrefabRef prefabRefData,
        CarData prefabCarData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSideEffectData.HasComponent(prefabRefData.m_Prefab) || !this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
          return 0.0f;
        // ISSUE: reference to a compiler-generated field
        VehicleSideEffectData vehicleSideEffectData = this.m_PrefabSideEffectData[prefabRefData.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        Game.Net.CarLane carLaneData = this.m_CarLaneData[currentLaneData.m_Lane];
        float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(prefabCarData, carLaneData);
        float num = math.select(currentLaneData.m_Distance / currentLaneData.m_Duration, maxDriveSpeed, (double) currentLaneData.m_Duration == 0.0) / prefabCarData.m_MaxSpeed;
        float s = math.saturate(num * num);
        return math.lerp(vehicleSideEffectData.m_Min.z, vehicleSideEffectData.m_Max.z, s) * currentLaneData.m_Duration;
      }

      private void ApplySideEffects(
        ref CarCurrentLane currentLane,
        float speedLimitFactor,
        PrefabRef prefabRefData,
        CarData prefabCarData)
      {
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ResetSpeed) != (Game.Vehicles.CarLaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLane.m_Lane) && ((double) currentLane.m_Duration != 0.0 || (double) currentLane.m_Distance != 0.0))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.CarLane carLaneData = this.m_CarLaneData[currentLane.m_Lane];
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[currentLane.m_Lane];
          carLaneData.m_SpeedLimit *= speedLimitFactor;
          float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(prefabCarData, carLaneData);
          float num1 = 1f / math.max(1f, curve.m_Length);
          float3 sideEffects = new float3();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSideEffectData.HasComponent(prefabRefData.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleSideEffectData vehicleSideEffectData = this.m_PrefabSideEffectData[prefabRefData.m_Prefab];
            float num2 = math.select(currentLane.m_Distance / currentLane.m_Duration, maxDriveSpeed, (double) currentLane.m_Duration == 0.0) / prefabCarData.m_MaxSpeed;
            float s = math.saturate(num2 * num2);
            sideEffects = math.lerp(vehicleSideEffectData.m_Min, vehicleSideEffectData.m_Max, s) * new float3(math.min(1f, currentLane.m_Distance * num1), currentLane.m_Duration, currentLane.m_Duration);
          }
          float num3 = math.min(prefabCarData.m_MaxSpeed, carLaneData.m_SpeedLimit);
          float2 flow = new float2(currentLane.m_Duration * num3, currentLane.m_Distance) * num1;
          // ISSUE: reference to a compiler-generated field
          this.m_LaneEffects.Enqueue(new CarNavigationHelpers.LaneEffects(currentLane.m_Lane, sideEffects, flow));
        }
        currentLane.m_Duration = 0.0f;
        currentLane.m_Distance = 0.0f;
      }

      private bool AllowHighBeams(
        Game.Objects.Transform transform,
        Blocker blocker,
        ref CarCurrentLane currentLaneData,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        float maxDistance,
        float minOffset)
      {
        Game.Objects.Transform componentData1;
        // ISSUE: reference to a compiler-generated field
        if (blocker.m_Blocker != Entity.Null && this.m_TransformData.TryGetComponent(blocker.m_Blocker, out componentData1))
        {
          float3 float3 = componentData1.m_Position - transform.m_Position;
          // ISSUE: reference to a compiler-generated field
          if ((double) math.lengthsq(float3) < (double) maxDistance * (double) maxDistance && (double) math.dot(math.forward(transform.m_Rotation), float3) > (double) minOffset && this.m_VehicleData.HasComponent(blocker.m_Blocker))
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[currentLaneData.m_Lane];
        float num = maxDistance - curve1.m_Length * math.abs(currentLaneData.m_CurvePosition.z - currentLaneData.m_CurvePosition.x);
        Entity owner = Entity.Null;
        Owner componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(currentLaneData.m_Lane, out componentData2) && owner != componentData2.m_Owner)
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.AllowHighBeams(transform, componentData2.m_Owner, maxDistance, minOffset))
            return false;
          owner = componentData2.m_Owner;
        }
        for (int index = 0; index < navigationLanes.Length && (double) num > 0.0; ++index)
        {
          CarNavigationLane navigationLane = navigationLanes[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.HasComponent(navigationLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.TryGetComponent(navigationLane.m_Lane, out componentData2) && owner != componentData2.m_Owner)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.AllowHighBeams(transform, componentData2.m_Owner, maxDistance, minOffset))
                return false;
              owner = componentData2.m_Owner;
            }
            // ISSUE: reference to a compiler-generated field
            Curve curve2 = this.m_CurveData[navigationLane.m_Lane];
            num -= curve2.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
          }
          else
            break;
        }
        return true;
      }

      private bool AllowHighBeams(
        Game.Objects.Transform transform,
        Entity owner,
        float maxDistance,
        float minOffset)
      {
        DynamicBuffer<Game.Net.SubLane> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Lanes.TryGetBuffer(owner, out bufferData1))
        {
          float3 x = math.forward(transform.m_Rotation);
          maxDistance *= maxDistance;
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            Game.Net.SubLane subLane = bufferData1[index1];
            DynamicBuffer<LaneObject> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if ((subLane.m_PathMethods & (PathMethod.Road | PathMethod.Track)) != (PathMethod) 0 && this.m_LaneObjects.TryGetBuffer(subLane.m_SubLane, out bufferData2))
            {
              for (int index2 = 0; index2 < bufferData2.Length; ++index2)
              {
                LaneObject laneObject = bufferData2[index2];
                Game.Objects.Transform componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.TryGetComponent(laneObject.m_LaneObject, out componentData))
                {
                  float3 float3 = componentData.m_Position - transform.m_Position;
                  // ISSUE: reference to a compiler-generated field
                  if ((double) math.lengthsq(float3) < (double) maxDistance && (double) math.dot(x, float3) > (double) minOffset && this.m_VehicleData.HasComponent(laneObject.m_LaneObject))
                    return false;
                }
              }
            }
          }
        }
        return true;
      }

      private void ReserveNavigationLanes(
        ref Unity.Mathematics.Random random,
        int priority,
        Entity entity,
        CarData prefabCarData,
        ObjectGeometryData prefabObjectGeometryData,
        Car carData,
        ref CarNavigation navigationData,
        ref CarCurrentLane currentLaneData,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        float timeStep = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[currentLaneData.m_Lane];
        bool c = (double) currentLaneData.m_CurvePosition.z < (double) currentLaneData.m_CurvePosition.x;
        float num1 = math.max(0.0f, VehicleUtils.GetBrakingDistance(prefabCarData, math.abs(navigationData.m_MaxSpeed), timeStep) - 0.01f);
        float num2 = num1;
        float a = (float) ((double) num2 / (double) math.max(1E-06f, curve1.m_Length) + 9.9999999747524271E-07);
        currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.x + math.select(a, -a, c);
        float num3 = num2 - curve1.m_Length * math.abs(currentLaneData.m_CurvePosition.z - currentLaneData.m_CurvePosition.x);
        int index = 0;
        if ((carData.m_Flags & CarFlags.Emergency) != (CarFlags) 0 && (double) num1 > 1.0)
        {
          if (currentLaneData.m_ChangeLane != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReserveOtherLanesInGroup(currentLaneData.m_ChangeLane, 102);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.ReserveOtherLanesInGroup(currentLaneData.m_Lane, 102);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((currentLaneData.m_LaneFlags & Game.Vehicles.CarLaneFlags.RequestSpace) != (Game.Vehicles.CarLaneFlags) 0 && this.m_LaneReservationData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LaneReservations.Enqueue(new CarNavigationHelpers.LaneReservation(currentLaneData.m_Lane, 0.0f, 96));
        }
        if (navigationLanes.Length > 0)
        {
          CarNavigationLane navigationLane = navigationLanes[0];
          // ISSUE: reference to a compiler-generated field
          if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.RequestSpace) != (Game.Vehicles.CarLaneFlags) 0 && this.m_LaneReservationData.HasComponent(navigationLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LaneReservations.Enqueue(new CarNavigationHelpers.LaneReservation(navigationLane.m_Lane, 0.0f, 96));
          }
        }
        bool2 bool2 = currentLaneData.m_CurvePosition.yz > currentLaneData.m_CurvePosition.zy;
        if ((c ? (bool2.y ? 1 : 0) : (bool2.x ? 1 : 0)) != 0)
        {
          currentLaneData.m_CurvePosition.y = currentLaneData.m_CurvePosition.z;
          CarNavigationLane navigationLane;
          for (; index < navigationLanes.Length && (double) num3 > 0.0; navigationLanes[index++] = navigationLane)
          {
            navigationLane = navigationLanes[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData.HasComponent(navigationLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve2 = this.m_CurveData[navigationLane.m_Lane];
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneReservationData.HasComponent(navigationLane.m_Lane))
              {
                float num4 = num3 / math.max(1E-06f, curve2.m_Length);
                float offset = math.max(navigationLane.m_CurvePosition.x, math.min(navigationLane.m_CurvePosition.y, navigationLane.m_CurvePosition.x + num4));
                // ISSUE: reference to a compiler-generated field
                this.m_LaneReservations.Enqueue(new CarNavigationHelpers.LaneReservation(navigationLane.m_Lane, offset, priority));
              }
              if ((carData.m_Flags & CarFlags.Emergency) != (CarFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.ReserveOtherLanesInGroup(navigationLane.m_Lane, 102);
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneSignalData.HasComponent(navigationLane.m_Lane))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_LaneSignals.Enqueue(new CarNavigationHelpers.LaneSignal(entity, navigationLane.m_Lane, priority));
                }
              }
              num3 -= curve2.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
              navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.Reserved;
            }
            else
              break;
          }
        }
        if ((carData.m_Flags & CarFlags.Emergency) != (CarFlags) 0)
        {
          float num5 = num3 + num1;
          if (random.NextInt(4) != 0)
            num5 += prefabObjectGeometryData.m_Bounds.max.z + 1f;
          for (; index < navigationLanes.Length && (double) num5 > 0.0; ++index)
          {
            CarNavigationLane navigationLane = navigationLanes[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CarLaneData.HasComponent(navigationLane.m_Lane))
              break;
            // ISSUE: reference to a compiler-generated field
            Curve curve3 = this.m_CurveData[navigationLane.m_Lane];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneReservationData.HasComponent(navigationLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneReservations.Enqueue(new CarNavigationHelpers.LaneReservation(navigationLane.m_Lane, 0.0f, priority));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneSignalData.HasComponent(navigationLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneSignals.Enqueue(new CarNavigationHelpers.LaneSignal(entity, navigationLane.m_Lane, priority));
            }
            num5 -= curve3.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
          }
        }
        else
        {
          if ((currentLaneData.m_LaneFlags & Game.Vehicles.CarLaneFlags.Roundabout) == (Game.Vehicles.CarLaneFlags) 0)
            return;
          float num6 = num3 + num1 * 0.5f;
          if (random.NextInt(2) != 0)
            num6 += prefabObjectGeometryData.m_Bounds.max.z + 1f;
          for (; index < navigationLanes.Length && (double) num6 > 0.0; ++index)
          {
            CarNavigationLane navigationLane = navigationLanes[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CarLaneData.HasComponent(navigationLane.m_Lane) || (navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Roundabout) == (Game.Vehicles.CarLaneFlags) 0)
              break;
            // ISSUE: reference to a compiler-generated field
            Curve curve4 = this.m_CurveData[navigationLane.m_Lane];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneReservationData.HasComponent(navigationLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LaneReservations.Enqueue(new CarNavigationHelpers.LaneReservation(navigationLane.m_Lane, 0.0f, priority));
            }
            num6 -= curve4.m_Length * math.abs(navigationLane.m_CurvePosition.y - navigationLane.m_CurvePosition.x);
          }
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
            this.m_LaneReservations.Enqueue(new CarNavigationHelpers.LaneReservation(subLane, 0.0f, priority));
          }
        }
      }

      private bool MoveAreaTarget(
        ref Unity.Mathematics.Random random,
        float3 comparePosition,
        PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<PathElement> pathElements,
        ref float3 targetPosition,
        float minDistance,
        Entity target,
        ref float3 curveDelta,
        float lanePosition,
        float navigationSize)
      {
        if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0)
          return true;
        // ISSUE: reference to a compiler-generated field
        Entity owner = this.m_OwnerData[target].m_Owner;
        // ISSUE: reference to a compiler-generated field
        AreaLane areaLane1 = this.m_AreaLaneData[target];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[owner];
        int start = math.min(pathOwner.m_ElementIndex, pathElements.Length);
        NativeArray<PathElement> subArray = pathElements.AsNativeArray().GetSubArray(start, pathElements.Length - start);
        int elementIndex1 = 0;
        bool flag = (double) curveDelta.z < (double) curveDelta.x;
        float lanePosition1 = math.select(lanePosition, -lanePosition, flag);
        if (areaLane1.m_Nodes.y == areaLane1.m_Nodes.z)
        {
          float3 position1 = areaNode[areaLane1.m_Nodes.x].m_Position;
          float3 position2 = areaNode[areaLane1.m_Nodes.y].m_Position;
          float3 position3 = areaNode[areaLane1.m_Nodes.w].m_Position;
          float3 right = position2;
          float3 next = position3;
          float3 comparePosition1 = comparePosition;
          int elementIndex2 = elementIndex1;
          DynamicBuffer<CarNavigationLane> navigationLanes1 = navigationLanes;
          NativeArray<PathElement> pathElements1 = subArray;
          ref float3 local = ref targetPosition;
          double minDistance1 = (double) minDistance;
          double lanePosition2 = (double) lanePosition1;
          double z = (double) curveDelta.z;
          double navigationSize1 = (double) navigationSize;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Game.Objects.Transform> transformData = this.m_TransformData;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<AreaLane> areaLaneData = this.m_AreaLaneData;
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Curve> curveData = this.m_CurveData;
          if (VehicleUtils.SetTriangleTarget(position1, right, next, comparePosition1, elementIndex2, navigationLanes1, pathElements1, ref local, (float) minDistance1, (float) lanePosition2, (float) z, (float) navigationSize1, true, transformData, areaLaneData, curveData))
            return true;
          curveDelta.y = curveDelta.z;
        }
        else
        {
          bool4 bool4_1 = new bool4(curveDelta.yz < 0.5f, curveDelta.yz > 0.5f);
          int2 int2_1 = math.select((int2) areaLane1.m_Nodes.x, (int2) areaLane1.m_Nodes.w, bool4_1.zw);
          float3 position4 = areaNode[int2_1.x].m_Position;
          float3 position5 = areaNode[areaLane1.m_Nodes.y].m_Position;
          float3 position6 = areaNode[areaLane1.m_Nodes.z].m_Position;
          float3 position7 = areaNode[int2_1.y].m_Position;
          if (math.any(bool4_1.xy & bool4_1.wz))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (VehicleUtils.SetAreaTarget(position4, position4, position5, position6, position7, owner, areaNode, comparePosition, elementIndex1, navigationLanes, subArray, ref targetPosition, minDistance, lanePosition1, curveDelta.z, navigationSize, flag, this.m_TransformData, this.m_AreaLaneData, this.m_CurveData, this.m_OwnerData))
              return true;
            curveDelta.y = 0.5f;
            bool4_1.xz = (bool2) false;
          }
          PathElement pathElement;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          if (VehicleUtils.GetPathElement(elementIndex1, navigationLanes, subArray, out pathElement) && this.m_OwnerData.TryGetComponent(pathElement.m_Target, out componentData) && componentData.m_Owner == owner)
          {
            bool4 bool4_2 = new bool4(pathElement.m_TargetDelta < 0.5f, pathElement.m_TargetDelta > 0.5f);
            if (math.any(!bool4_1.xz) & math.any(bool4_1.yw) & math.any(bool4_2.xy & bool4_2.wz))
            {
              // ISSUE: reference to a compiler-generated field
              AreaLane areaLane2 = this.m_AreaLaneData[pathElement.m_Target];
              int2 int2_2 = math.select((int2) areaLane2.m_Nodes.x, (int2) areaLane2.m_Nodes.w, bool4_2.zw);
              float3 position8 = areaNode[int2_2.x].m_Position;
              float3 prev2 = math.select(position5, position6, position8.Equals(position5));
              float3 position9 = areaNode[areaLane2.m_Nodes.y].m_Position;
              float3 position10 = areaNode[areaLane2.m_Nodes.z].m_Position;
              float3 position11 = areaNode[int2_2.y].m_Position;
              bool c = (double) pathElement.m_TargetDelta.y < (double) pathElement.m_TargetDelta.x;
              float num1 = math.select(lanePosition, -lanePosition, c);
              float3 prev = position8;
              float3 left = position9;
              float3 right = position10;
              float3 next = position11;
              Entity areaEntity = owner;
              DynamicBuffer<Game.Areas.Node> nodes = areaNode;
              float3 comparePosition2 = comparePosition;
              int elementIndex3 = elementIndex1 + 1;
              DynamicBuffer<CarNavigationLane> navigationLanes2 = navigationLanes;
              NativeArray<PathElement> pathElements2 = subArray;
              ref float3 local = ref targetPosition;
              double minDistance2 = (double) minDistance;
              double lanePosition3 = (double) num1;
              double y = (double) pathElement.m_TargetDelta.y;
              double navigationSize2 = (double) navigationSize;
              int num2 = c ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<Game.Objects.Transform> transformData = this.m_TransformData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<AreaLane> areaLaneData = this.m_AreaLaneData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<Curve> curveData = this.m_CurveData;
              // ISSUE: reference to a compiler-generated field
              ComponentLookup<Owner> ownerData = this.m_OwnerData;
              if (VehicleUtils.SetAreaTarget(prev2, prev, left, right, next, areaEntity, nodes, comparePosition2, elementIndex3, navigationLanes2, pathElements2, ref local, (float) minDistance2, (float) lanePosition3, (float) y, (float) navigationSize2, num2 != 0, transformData, areaLaneData, curveData, ownerData))
                return true;
            }
            curveDelta.y = curveDelta.z;
            return false;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (VehicleUtils.SetTriangleTarget(position5, position6, position7, comparePosition, elementIndex1, navigationLanes, subArray, ref targetPosition, minDistance, lanePosition1, curveDelta.z, navigationSize, false, this.m_TransformData, this.m_AreaLaneData, this.m_CurveData))
            return true;
          curveDelta.y = curveDelta.z;
        }
        return (double) math.distance(comparePosition.xz, targetPosition.xz) >= (double) minDistance;
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
        return VehicleUtils.CalculateTransformPosition(ref targetPosition, target, this.m_TransformData, this.m_PositionData, this.m_PrefabRefData, this.m_PrefabBuildingData) && (double) math.distance(comparePosition, targetPosition) >= (double) minDistance;
      }

      private bool MoveTarget(
        float3 comparePosition,
        ref float3 targetPosition,
        float minDistance,
        Bezier4x3 curve,
        ref float3 curveDelta,
        float laneOffset)
      {
        float3 lanePosition1 = VehicleUtils.GetLanePosition(curve, curveDelta.z, laneOffset);
        if ((double) math.distance(comparePosition, lanePosition1) < (double) minDistance)
        {
          curveDelta.x = curveDelta.z;
          targetPosition = lanePosition1;
          return false;
        }
        float2 xz = curveDelta.xz;
        for (int index = 0; index < 8; ++index)
        {
          float curvePosition = math.lerp(xz.x, xz.y, 0.5f);
          float3 lanePosition2 = VehicleUtils.GetLanePosition(curve, curvePosition, laneOffset);
          if ((double) math.distance(comparePosition, lanePosition2) < (double) minDistance)
            xz.x = curvePosition;
          else
            xz.y = curvePosition;
        }
        curveDelta.x = xz.y;
        targetPosition = VehicleUtils.GetLanePosition(curve, xz.y, laneOffset);
        return true;
      }

      private bool MoveTarget(
        float3 comparePosition,
        ref float3 targetPosition,
        float minDistance,
        Bezier4x3 curve1,
        Bezier4x3 curve2,
        float curveSelect,
        ref float3 curveDelta,
        float laneOffset1,
        float laneOffset2)
      {
        curveSelect = math.saturate(curveSelect);
        float3 lanePosition1 = VehicleUtils.GetLanePosition(curve1, curveDelta.z, laneOffset1);
        float3 lanePosition2 = VehicleUtils.GetLanePosition(curve2, curveDelta.z, laneOffset2);
        float t;
        if ((double) MathUtils.Distance(new Line3.Segment(lanePosition1, lanePosition2), comparePosition, out t) < (double) minDistance)
        {
          curveDelta.x = curveDelta.z;
          targetPosition = math.lerp(lanePosition1, lanePosition2, curveSelect);
          return false;
        }
        float2 xz = curveDelta.xz;
        for (int index = 0; index < 8; ++index)
        {
          float curvePosition = math.lerp(xz.x, xz.y, 0.5f);
          if ((double) MathUtils.Distance(new Line3.Segment(VehicleUtils.GetLanePosition(curve1, curvePosition, laneOffset1), VehicleUtils.GetLanePosition(curve2, curvePosition, laneOffset2)), comparePosition, out t) < (double) minDistance)
            xz.x = curvePosition;
          else
            xz.y = curvePosition;
        }
        curveDelta.x = xz.y;
        float3 lanePosition3 = VehicleUtils.GetLanePosition(curve1, xz.y, laneOffset1);
        float3 lanePosition4 = VehicleUtils.GetLanePosition(curve2, xz.y, laneOffset2);
        targetPosition = math.lerp(lanePosition3, lanePosition4, curveSelect);
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
      public NativeQueue<CarNavigationHelpers.LaneSignal> m_LaneSignalQueue;
      public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;

      public void Execute()
      {
        CarNavigationHelpers.LaneSignal laneSignal1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneSignalQueue.TryDequeue(out laneSignal1))
        {
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
      public NativeQueue<CarNavigationHelpers.LaneReservation> m_LaneReservationQueue;
      public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;

      public void Execute()
      {
        CarNavigationHelpers.LaneReservation laneReservation;
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

    public struct TrafficAmbienceEffect
    {
      public float3 m_Position;
      public float m_Amount;
    }

    [BurstCompile]
    private struct ApplyTrafficAmbienceJob : IJob
    {
      public NativeQueue<CarNavigationSystem.TrafficAmbienceEffect> m_EffectsQueue;
      public NativeArray<TrafficAmbienceCell> m_TrafficAmbienceMap;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        CarNavigationSystem.TrafficAmbienceEffect trafficAmbienceEffect;
        // ISSUE: reference to a compiler-generated field
        while (this.m_EffectsQueue.TryDequeue(out trafficAmbienceEffect))
        {
          // ISSUE: reference to a compiler-generated field
          int2 cell = CellMapSystem<TrafficAmbienceCell>.GetCell(trafficAmbienceEffect.m_Position, CellMapSystem<TrafficAmbienceCell>.kMapSize, TrafficAmbienceSystem.kTextureSize);
          if (cell.x >= 0 && cell.y >= 0 && cell.x < TrafficAmbienceSystem.kTextureSize && cell.y < TrafficAmbienceSystem.kTextureSize)
          {
            int index = cell.x + cell.y * TrafficAmbienceSystem.kTextureSize;
            // ISSUE: reference to a compiler-generated field
            TrafficAmbienceCell trafficAmbience = this.m_TrafficAmbienceMap[index];
            // ISSUE: reference to a compiler-generated field
            trafficAmbience.m_Accumulator += trafficAmbienceEffect.m_Amount;
            // ISSUE: reference to a compiler-generated field
            this.m_TrafficAmbienceMap[index] = trafficAmbience;
          }
        }
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
      public NativeQueue<CarNavigationHelpers.LaneEffects> m_LaneEffectsQueue;

      public void Execute()
      {
        CarNavigationHelpers.LaneEffects laneEffects;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LaneEffectsQueue.TryDequeue(out laneEffects))
        {
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OutOfControl> __Game_Vehicles_OutOfControl_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<CarNavigation> __Game_Vehicles_CarNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
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
      public ComponentLookup<AreaLane> __Game_Net_AreaLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneReservation> __Game_Net_LaneReservation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.LaneSignal> __Game_Net_LaneSignal_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Car> __Game_Vehicles_Car_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creature> __Game_Creatures_Creature_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleSideEffectData> __Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public ComponentLookup<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RW_ComponentLookup;
      public BufferLookup<BlockedLane> __Game_Objects_BlockedLane_RW_BufferLookup;

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
        this.__Game_Vehicles_Car_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OutOfControl_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OutOfControl>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
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
        this.__Game_Net_AreaLane_RO_ComponentLookup = state.GetComponentLookup<AreaLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneReservation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneReservation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RO_ComponentLookup = state.GetComponentLookup<LaneCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneSignal_RO_ComponentLookup = state.GetComponentLookup<Game.Net.LaneSignal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentLookup = state.GetComponentLookup<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentLookup = state.GetComponentLookup<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleSideEffectData_RO_ComponentLookup = state.GetComponentLookup<VehicleSideEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RW_ComponentLookup = state.GetComponentLookup<CarTrailerLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_BlockedLane_RW_BufferLookup = state.GetBufferLookup<BlockedLane>();
      }
    }
  }
}
