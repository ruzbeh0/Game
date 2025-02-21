// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TaxiAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
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
  public class TaxiAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private TimeSystem m_TimeSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_TaxiRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private TaxiAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 6;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.Taxi>(), ComponentType.ReadWrite<Game.Common.Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_TaxiRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TaxiRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedCarRemoveTypes = new ComponentTypeSet(new ComponentType[12]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Game.Common.Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<ServiceDispatch>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedAddTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<Stopped>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<TaxiAISystem.RouteVehicleUpdate> nativeQueue = new NativeQueue<TaxiAISystem.RouteVehicleUpdate>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_RideNeeder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TaxiData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Odometer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TaxiAISystem.TaxiTickJob jobData1 = new TaxiAISystem.TaxiTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CurrentRouteType = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RO_ComponentTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_TaxiType = this.__TypeHandle.__Game_Vehicles_Taxi_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_PersonalCarData = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup,
        m_BlockerData = this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_StoppedData = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_BoardingVehicleData = this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup,
        m_TaxiStandData = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentLookup,
        m_WaitingPassengersData = this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_TransportDepotData = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabTaxiData = this.__TypeHandle.__Game_Prefabs_TaxiData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabCreatureData = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup,
        m_PrefabHumanData = this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_TaxiRequestData = this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_DivertData = this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_RideNeederData = this.__TypeHandle.__Game_Creatures_RideNeeder_RO_ComponentLookup,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_CarKeeperData = this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup,
        m_HouseholdMemberData = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_HouseholdData = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_WorkerData = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_TravelPurposeData = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_RouteVehicles = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup,
        m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_TaxiRequestArchetype = this.m_TaxiRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_RouteVehicleQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TaxiAISystem.UpdateRouteVehiclesJob jobData2 = new TaxiAISystem.UpdateRouteVehiclesJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_RouteVehicleQueue = nativeQueue,
        m_TaxiData = this.__TypeHandle.__Game_Vehicles_Taxi_RW_ComponentLookup,
        m_BoardingVehicleData = this.__TypeHandle.__Game_Routes_BoardingVehicle_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<TaxiAISystem.TaxiTickJob>(this.m_VehicleQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<TaxiAISystem.UpdateRouteVehiclesJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = inputDeps;
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
    public TaxiAISystem()
    {
    }

    private struct RouteVehicleUpdate
    {
      public Entity m_Route;
      public Entity m_AddVehicle;
      public Entity m_RemoveVehicle;

      public static TaxiAISystem.RouteVehicleUpdate Remove(Entity route, Entity vehicle)
      {
        // ISSUE: object of a compiler-generated type is created
        return new TaxiAISystem.RouteVehicleUpdate()
        {
          m_Route = route,
          m_RemoveVehicle = vehicle
        };
      }

      public static TaxiAISystem.RouteVehicleUpdate Add(Entity route, Entity vehicle)
      {
        // ISSUE: object of a compiler-generated type is created
        return new TaxiAISystem.RouteVehicleUpdate()
        {
          m_Route = route,
          m_AddVehicle = vehicle
        };
      }
    }

    [BurstCompile]
    private struct TaxiTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> m_CurrentRouteType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      [ReadOnly]
      public ComponentTypeHandle<Odometer> m_OdometerType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      public ComponentTypeHandle<Game.Vehicles.Taxi> m_TaxiType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public BufferTypeHandle<CarNavigationLane> m_CarNavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCarData;
      [ReadOnly]
      public ComponentLookup<Blocker> m_BlockerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Stopped> m_StoppedData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;
      [ReadOnly]
      public ComponentLookup<TaxiStand> m_TaxiStandData;
      [ReadOnly]
      public ComponentLookup<WaitingPassengers> m_WaitingPassengersData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<TaxiData> m_PrefabTaxiData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<CreatureData> m_PrefabCreatureData;
      [ReadOnly]
      public ComponentLookup<HumanData> m_PrefabHumanData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> m_TaxiRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<Divert> m_DivertData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<RideNeeder> m_RideNeederData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<CarKeeper> m_CarKeeperData;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberData;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdData;
      [ReadOnly]
      public ComponentLookup<Worker> m_WorkerData;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeData;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicles;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Common.Target> m_TargetData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_TaxiRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      [ReadOnly]
      public float m_TimeOfDay;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<TaxiAISystem.RouteVehicleUpdate>.ParallelWriter m_RouteVehicleQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentRoute> nativeArray4 = chunk.GetNativeArray<CurrentRoute>(ref this.m_CurrentRouteType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray5 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.Taxi> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.Taxi>(ref this.m_TaxiType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray7 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Odometer> nativeArray8 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor1 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor2 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isStopped = chunk.Has<Stopped>(ref this.m_StoppedType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          Game.Vehicles.Taxi taxi = nativeArray6[index];
          Car car = nativeArray7[index];
          CarCurrentLane currentLane = nativeArray5[index];
          Odometer odometer = nativeArray8[index];
          DynamicBuffer<Passenger> passengers = bufferAccessor1[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          CurrentRoute currentRoute = new CurrentRoute();
          if (nativeArray4.Length != 0)
            currentRoute = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          Game.Common.Target target = this.m_TargetData[entity];
          // ISSUE: reference to a compiler-generated field
          PathOwner pathOwner = this.m_PathOwnerData[entity];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, odometer, prefabRef, currentRoute, passengers, navigationLanes, serviceDispatches, isStopped, ref taxi, ref car, ref currentLane, ref pathOwner, ref target);
          // ISSUE: reference to a compiler-generated field
          this.m_TargetData[entity] = target;
          // ISSUE: reference to a compiler-generated field
          this.m_PathOwnerData[entity] = pathOwner;
          nativeArray6[index] = taxi;
          nativeArray7[index] = car;
          nativeArray5[index] = currentLane;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        Owner owner,
        Odometer odometer,
        PrefabRef prefabRef,
        CurrentRoute currentRoute,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isStopped,
        ref Game.Vehicles.Taxi taxi,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(entity.Index);
        bool flag = (taxi.m_State & TaxiFlags.Boarding) > (TaxiFlags) 0;
        TaxiData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabTaxiData.TryGetComponent(prefabRef.m_Prefab, out componentData) && (double) odometer.m_Distance >= (double) componentData.m_MaintenanceRange && (double) componentData.m_MaintenanceRange > 0.10000000149011612)
          taxi.m_State |= TaxiFlags.RequiresMaintenance;
        // ISSUE: reference to a compiler-generated method
        this.CheckServiceDispatches(entity, serviceDispatches, ref taxi);
        // ISSUE: reference to a compiler-generated field
        if ((taxi.m_State & (TaxiFlags.Requested | TaxiFlags.RequiresMaintenance | TaxiFlags.Dispatched | TaxiFlags.Disabled)) == (TaxiFlags) 0 && !this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
        {
          // ISSUE: reference to a compiler-generated method
          this.RequestTargetIfNeeded(jobIndex, entity, ref taxi);
        }
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated field
          CarData prefabCarData = this.m_PrefabCarData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, ref random, entity, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, prefabCarData);
        }
        // ISSUE: reference to a compiler-generated field
        if ((taxi.m_State & (TaxiFlags.Disembarking | TaxiFlags.Transporting)) == (TaxiFlags) 0 && !this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if ((taxi.m_State & TaxiFlags.Boarding) != (TaxiFlags) 0)
          {
            flag = false;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Remove(currentRoute.m_Route, entity));
            }
            else
              taxi.m_State &= ~TaxiFlags.Boarding;
          }
          if (VehicleUtils.IsStuck(pathOwner) || (taxi.m_State & TaxiFlags.Returning) != (TaxiFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane) || VehicleUtils.ParkingSpaceReached(currentLane, pathOwner) || (taxi.m_State & (TaxiFlags.Boarding | TaxiFlags.Disembarking)) != (TaxiFlags) 0)
        {
          if ((taxi.m_State & TaxiFlags.Disembarking) != (TaxiFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (this.StopDisembarking(entity, passengers, ref taxi, ref pathOwner) && !this.SelectDispatch(jobIndex, entity, currentRoute, navigationLanes, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, ref target))
            {
              if ((taxi.m_State & TaxiFlags.Returning) != (TaxiFlags) 0)
              {
                if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ParkCar(jobIndex, entity, owner, ref taxi, ref car, ref currentLane);
                  return;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                return;
              }
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
            }
          }
          else if ((taxi.m_State & TaxiFlags.Boarding) != (TaxiFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.StopBoarding(jobIndex, entity, ref random, odometer, prefabRef, currentRoute, passengers, navigationLanes, serviceDispatches, ref taxi, ref currentLane, ref pathOwner, ref target))
            {
              flag = false;
              // ISSUE: reference to a compiler-generated method
              if ((taxi.m_State & TaxiFlags.Transporting) == (TaxiFlags) 0 && !this.SelectDispatch(jobIndex, entity, currentRoute, navigationLanes, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, ref target))
              {
                // ISSUE: reference to a compiler-generated method
                this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!isStopped && this.ShouldStop(currentRoute, passengers, ref taxi))
              {
                // ISSUE: reference to a compiler-generated method
                this.StopVehicle(jobIndex, entity, ref car, ref currentLane);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (!this.StartDisembarking(odometer, passengers, ref taxi) && ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0 || !this.SelectDispatch(jobIndex, entity, currentRoute, navigationLanes, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, ref target)))
            {
              if ((taxi.m_State & TaxiFlags.Returning) != (TaxiFlags) 0)
              {
                if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ParkCar(jobIndex, entity, owner, ref taxi, ref car, ref currentLane);
                  return;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                return;
              }
              // ISSUE: reference to a compiler-generated method
              if (this.StartBoarding(jobIndex, entity, currentRoute, serviceDispatches, ref taxi, ref target))
              {
                flag = true;
                // ISSUE: reference to a compiler-generated method
                if (!isStopped && this.ShouldStop(currentRoute, passengers, ref taxi))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.StopVehicle(jobIndex, entity, ref car, ref currentLane);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.SelectDispatch(jobIndex, entity, currentRoute, navigationLanes, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, ref target))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
                }
              }
            }
          }
        }
        else if (VehicleUtils.QueueReached(currentLane))
        {
          if ((taxi.m_State & (TaxiFlags.Returning | TaxiFlags.Dispatched)) != (TaxiFlags) 0)
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Queue;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if (this.SelectDispatch(jobIndex, entity, currentRoute, navigationLanes, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, ref target))
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Queue;
            else if ((taxi.m_State & TaxiFlags.Disabled) != (TaxiFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
            }
            else if (!isStopped)
            {
              bool shouldStop;
              // ISSUE: reference to a compiler-generated method
              if (this.CanQueue(currentRoute, out shouldStop))
              {
                if (shouldStop)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.StopVehicle(jobIndex, entity, ref car, ref currentLane);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
              }
            }
          }
        }
        else if (VehicleUtils.WaypointReached(currentLane))
        {
          currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
          pathOwner.m_State &= ~PathFlags.Failed;
          pathOwner.m_State |= PathFlags.Obsolete;
        }
        else
        {
          if ((taxi.m_State & (TaxiFlags.Returning | TaxiFlags.Transporting | TaxiFlags.Disabled)) == TaxiFlags.Disabled)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(jobIndex, entity, owner, serviceDispatches, ref taxi, ref car, ref pathOwner, ref target);
          }
          if (isStopped)
          {
            // ISSUE: reference to a compiler-generated method
            this.StartVehicle(jobIndex, entity, ref car, ref currentLane);
          }
        }
        if ((taxi.m_State & TaxiFlags.Disembarking) == (TaxiFlags) 0 && !flag)
        {
          if ((taxi.m_State & (TaxiFlags.Transporting | TaxiFlags.Dispatched)) == (TaxiFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectDispatch(jobIndex, entity, currentRoute, navigationLanes, serviceDispatches, ref taxi, ref car, ref currentLane, ref pathOwner, ref target);
          }
          if ((taxi.m_State & TaxiFlags.Arriving) == (TaxiFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckNavigationLanes(navigationLanes, ref taxi, ref currentLane, ref target);
          }
          if (VehicleUtils.RequireNewPath(pathOwner))
          {
            if (isStopped && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ParkingSpace) == (Game.Vehicles.CarLaneFlags) 0)
            {
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.FindNewPath(entity, prefabRef, passengers, ref taxi, ref currentLane, ref pathOwner, ref target);
            }
          }
          else if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) == (PathFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingSpace(entity, ref random, ref taxi, ref currentLane, ref pathOwner, navigationLanes);
          }
        }
        if ((taxi.m_State & (TaxiFlags.Disembarking | TaxiFlags.Transporting | TaxiFlags.RequiresMaintenance | TaxiFlags.Dispatched | TaxiFlags.FromOutside | TaxiFlags.Disabled)) == (TaxiFlags) 0)
          car.m_Flags |= CarFlags.Sign;
        else
          car.m_Flags &= ~CarFlags.Sign;
      }

      private bool CanQueue(CurrentRoute currentRoute, out bool shouldStop)
      {
        WaitingPassengers componentData;
        DynamicBuffer<RouteVehicle> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaitingPassengersData.TryGetComponent(currentRoute.m_Route, out componentData) && this.m_RouteVehicles.TryGetBuffer(currentRoute.m_Route, out bufferData))
        {
          int num = 0;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_StoppedData.HasComponent(bufferData[index].m_Vehicle))
              ++num;
          }
          int maxTaxiCount = RouteUtils.GetMaxTaxiCount(componentData);
          shouldStop = componentData.m_Count == 0;
          return num < maxTaxiCount;
        }
        shouldStop = false;
        return false;
      }

      private bool ShouldStop(
        CurrentRoute currentRoute,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.Taxi taxi)
      {
        WaitingPassengers componentData;
        // ISSUE: reference to a compiler-generated field
        return (taxi.m_State & TaxiFlags.Dispatched) == (TaxiFlags) 0 && passengers.Length == 0 && this.m_WaitingPassengersData.TryGetComponent(currentRoute.m_Route, out componentData) && componentData.m_Count == 0;
      }

      private void ParkCar(
        int jobIndex,
        Entity entity,
        Owner owner,
        ref Game.Vehicles.Taxi taxi,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        car.m_Flags &= ~CarFlags.Sign;
        taxi.m_State &= TaxiFlags.RequiresMaintenance;
        Game.Buildings.TransportDepot componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportDepotData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & TransportDepotFlags.HasAvailableVehicles) == (TransportDepotFlags) 0)
          taxi.m_State |= TaxiFlags.Disabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_MovingToParkedCarRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_MovingToParkedAddTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(jobIndex, entity, new ParkedCar(currentLane.m_Lane, currentLane.m_CurvePosition.x));
        // ISSUE: reference to a compiler-generated field
        if (this.m_ParkingLaneData.HasComponent(currentLane.m_Lane) && currentLane.m_ChangeLane == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLane.m_Lane);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarageLaneData.HasComponent(currentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLane.m_Lane);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(currentLane.m_ChangeLane, entity));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(currentLane.m_ChangeLane, entity));
          }
        }
      }

      private void StopVehicle(
        int jobIndex,
        Entity entity,
        ref Car car,
        ref CarCurrentLane currentLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Moving>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Swaying>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Stopped>(jobIndex, entity, new Stopped());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_Lane, new PathfindUpdated());
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_ChangeLane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_ChangeLane, new PathfindUpdated());
        }
        if ((currentLaneData.m_LaneFlags & Game.Vehicles.CarLaneFlags.Queue) == (Game.Vehicles.CarLaneFlags) 0)
          return;
        car.m_Flags |= CarFlags.Queueing;
      }

      private void StartVehicle(
        int jobIndex,
        Entity entity,
        ref Car car,
        ref CarCurrentLane currentLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stopped>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Moving>(jobIndex, entity, new Moving());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, entity, new InterpolatedTransform());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Swaying>(jobIndex, entity, new Swaying());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_Lane, new PathfindUpdated());
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_ChangeLane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_ChangeLane, new PathfindUpdated());
        }
        car.m_Flags &= ~CarFlags.Queueing;
      }

      private void CheckNavigationLanes(
        DynamicBuffer<CarNavigationLane> navigationLanes,
        ref Game.Vehicles.Taxi taxi,
        ref CarCurrentLane currentLane,
        ref Game.Common.Target target)
      {
        if (navigationLanes.Length == 0 || navigationLanes.Length == 8)
          return;
        CarNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.EndOfPath) == (Game.Vehicles.CarLaneFlags) 0)
          return;
        taxi.m_State |= TaxiFlags.Arriving;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RouteLaneData.HasComponent(target.m_Target))
          return;
        // ISSUE: reference to a compiler-generated field
        RouteLane routeLane = this.m_RouteLaneData[target.m_Target];
        if (routeLane.m_StartLane != routeLane.m_EndLane)
        {
          navigationLane.m_CurvePosition.y = 1f;
          CarNavigationLane elem = new CarNavigationLane();
          elem.m_Lane = navigationLane.m_Lane;
          // ISSUE: reference to a compiler-generated method
          if (this.FindNextLane(ref elem.m_Lane))
          {
            navigationLane.m_Flags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
            navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.Queue;
            navigationLanes[navigationLanes.Length - 1] = navigationLane;
            elem.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.FixedLane | Game.Vehicles.CarLaneFlags.Queue;
            elem.m_CurvePosition = new float2(0.0f, routeLane.m_EndCurvePos);
            navigationLanes.Add(elem);
          }
          else
            navigationLanes[navigationLanes.Length - 1] = navigationLane;
        }
        else
        {
          navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.Queue;
          navigationLane.m_CurvePosition.y = routeLane.m_EndCurvePos;
          navigationLanes[navigationLanes.Length - 1] = navigationLane;
        }
      }

      private bool FindNextLane(ref Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnerData.HasComponent(lane) || !this.m_LaneData.HasComponent(lane))
          return false;
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[lane];
        // ISSUE: reference to a compiler-generated field
        Lane lane1 = this.m_LaneData[lane];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(owner.m_Owner))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner.m_Owner];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          Lane lane2 = this.m_LaneData[subLane2];
          if (lane1.m_EndNode.Equals(lane2.m_StartNode))
          {
            lane = subLane2;
            return true;
          }
        }
        return false;
      }

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.Taxi taxi,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        CarData carData1 = this.m_PrefabCarData[prefabRef.m_Prefab];
        pathOwner.m_State &= ~(PathFlags.AddDestination | PathFlags.Divert);
        PathfindParameters parameters;
        SetupQueueTarget origin;
        SetupQueueTarget destination;
        if ((taxi.m_State & (TaxiFlags.Returning | TaxiFlags.Transporting)) == TaxiFlags.Transporting)
        {
          parameters = new PathfindParameters()
          {
            m_MaxSpeed = (float2) carData1.m_MaxSpeed,
            m_WalkSpeed = (float2) 5.555556f,
            m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
            m_Methods = PathMethod.Pedestrian | PathMethod.Taxi,
            m_SecondaryIgnoredRules = RuleFlags.ForbidPrivateTraffic | VehicleUtils.GetIgnoredPathfindRules(carData1)
          };
          SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
          setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
          setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.Boarding;
          setupQueueTarget.m_RoadTypes = RoadTypes.Car;
          setupQueueTarget.m_Flags = SetupTargetFlags.SecondaryPath;
          origin = setupQueueTarget;
          setupQueueTarget = new SetupQueueTarget();
          setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
          setupQueueTarget.m_Methods = PathMethod.Pedestrian;
          setupQueueTarget.m_Entity = target.m_Target;
          setupQueueTarget.m_RandomCost = 30f;
          destination = setupQueueTarget;
          // ISSUE: reference to a compiler-generated method
          Entity leader = this.FindLeader(passengers);
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResidentData.HasComponent(leader))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef1 = this.m_PrefabRefData[leader];
            // ISSUE: reference to a compiler-generated field
            Game.Creatures.Resident resident = this.m_ResidentData[leader];
            // ISSUE: reference to a compiler-generated field
            CreatureData creatureData = this.m_PrefabCreatureData[prefabRef1.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            HumanData humanData = this.m_PrefabHumanData[prefabRef1.m_Prefab];
            parameters.m_WalkSpeed = (float2) humanData.m_WalkSpeed;
            // ISSUE: reference to a compiler-generated field
            parameters.m_Methods |= RouteUtils.GetPublicTransportMethods(resident, this.m_TimeOfDay);
            // ISSUE: reference to a compiler-generated field
            parameters.m_MaxCost = CitizenBehaviorSystem.kMaxPathfindCost;
            destination.m_ActivityMask = creatureData.m_SupportedActivities;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdMemberData.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              Entity household = this.m_HouseholdMemberData[resident.m_Citizen].m_Household;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PropertyRenterData.HasComponent(household))
              {
                // ISSUE: reference to a compiler-generated field
                parameters.m_Authorization1 = this.m_PropertyRenterData[household].m_Property;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_WorkerData.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              Worker worker = this.m_WorkerData[resident.m_Citizen];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              parameters.m_Authorization2 = !this.m_PropertyRenterData.HasComponent(worker.m_Workplace) ? worker.m_Workplace : this.m_PropertyRenterData[worker.m_Workplace].m_Property;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_CitizenData.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              Citizen citizen = this.m_CitizenData[resident.m_Citizen];
              // ISSUE: reference to a compiler-generated field
              Entity household1 = this.m_HouseholdMemberData[resident.m_Citizen].m_Household;
              // ISSUE: reference to a compiler-generated field
              Household household2 = this.m_HouseholdData[household1];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household1];
              parameters.m_Weights = CitizenUtils.GetPathfindWeights(citizen, household2, householdCitizen.Length);
            }
            CarKeeper component;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarKeeperData.TryGetEnabledComponent<CarKeeper>(resident.m_Citizen, out component) && this.m_ParkedCarData.HasComponent(component.m_Car))
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef2 = this.m_PrefabRefData[component.m_Car];
              // ISSUE: reference to a compiler-generated field
              ParkedCar parkedCar = this.m_ParkedCarData[component.m_Car];
              // ISSUE: reference to a compiler-generated field
              CarData carData2 = this.m_PrefabCarData[prefabRef2.m_Prefab];
              parameters.m_MaxSpeed.x = carData2.m_MaxSpeed;
              parameters.m_ParkingTarget = parkedCar.m_Lane;
              parameters.m_ParkingDelta = parkedCar.m_CurvePosition;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              parameters.m_ParkingSize = VehicleUtils.GetParkingSize(component.m_Car, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData);
              parameters.m_Methods |= PathMethod.Road | PathMethod.Parking;
              parameters.m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData2);
              Game.Vehicles.PersonalCar componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PersonalCarData.TryGetComponent(component.m_Car, out componentData) && (componentData.m_State & PersonalCarFlags.HomeTarget) == (PersonalCarFlags) 0)
                parameters.m_PathfindFlags |= PathfindFlags.ParkingReset;
            }
            TravelPurpose componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TravelPurposeData.TryGetComponent(resident.m_Citizen, out componentData1))
            {
              switch (componentData1.m_Purpose)
              {
                case Game.Citizens.Purpose.MovingAway:
                  // ISSUE: reference to a compiler-generated field
                  parameters.m_MaxCost = CitizenBehaviorSystem.kMaxMovingAwayCost;
                  break;
                case Game.Citizens.Purpose.EmergencyShelter:
                  parameters.m_Weights = new PathfindWeights(1f, 0.2f, 0.0f, 0.1f);
                  break;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_DivertData.HasComponent(leader))
            {
              // ISSUE: reference to a compiler-generated field
              Divert divert = this.m_DivertData[leader];
              CreatureUtils.DivertDestination(ref destination, ref pathOwner, divert);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          parameters = new PathfindParameters()
          {
            m_MaxSpeed = (float2) carData1.m_MaxSpeed,
            m_WalkSpeed = (float2) 5.555556f,
            m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
            m_Methods = PathMethod.Road | PathMethod.Boarding,
            m_ParkingTarget = VehicleUtils.GetParkingSource(vehicleEntity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
            m_ParkingDelta = currentLane.m_CurvePosition.z,
            m_ParkingSize = VehicleUtils.GetParkingSize(vehicleEntity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData),
            m_IgnoredRules = RuleFlags.ForbidPrivateTraffic | VehicleUtils.GetIgnoredPathfindRules(carData1)
          };
          origin = new SetupQueueTarget()
          {
            m_Type = SetupTargetType.CurrentLocation,
            m_Methods = PathMethod.Road | PathMethod.Boarding,
            m_RoadTypes = RoadTypes.Car
          };
          destination = new SetupQueueTarget()
          {
            m_Type = SetupTargetType.CurrentLocation,
            m_RoadTypes = RoadTypes.Car,
            m_Entity = target.m_Target
          };
          if ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0)
            destination.m_Methods = PathMethod.Boarding;
          else if ((taxi.m_State & TaxiFlags.Returning) != (TaxiFlags) 0)
          {
            parameters.m_Methods |= PathMethod.SpecialParking;
            destination.m_Methods = PathMethod.Road | PathMethod.SpecialParking;
            destination.m_RandomCost = 30f;
          }
          else
            destination.m_Methods = PathMethod.Road;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Taxi taxi)
      {
        if ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0)
        {
          if (serviceDispatches.Length > 1)
          {
            serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
          }
          else
          {
            if (serviceDispatches.Length != 0)
              return;
            taxi.m_State &= ~(TaxiFlags.Requested | TaxiFlags.Dispatched);
          }
        }
        else
        {
          TaxiRequestType taxiRequestType = TaxiRequestType.Stand;
          int num = -1;
          Entity request1 = Entity.Null;
          for (int index = 0; index < serviceDispatches.Length; ++index)
          {
            Entity request2 = serviceDispatches[index].m_Request;
            TaxiRequest componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TaxiRequestData.TryGetComponent(request2, out componentData) && this.m_PrefabRefData.HasComponent(componentData.m_Seeker) && (componentData.m_Type > taxiRequestType || componentData.m_Type == taxiRequestType && componentData.m_Priority > num))
            {
              taxiRequestType = componentData.m_Type;
              num = componentData.m_Priority;
              request1 = request2;
            }
          }
          if (request1 != Entity.Null)
          {
            serviceDispatches[0] = new ServiceDispatch(request1);
            if (serviceDispatches.Length > 1)
              serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
            taxi.m_State |= TaxiFlags.Requested;
          }
          else
          {
            serviceDispatches.Clear();
            taxi.m_State &= ~TaxiFlags.Requested;
          }
        }
      }

      private void RequestTargetIfNeeded(int jobIndex, Entity entity, ref Game.Vehicles.Taxi taxi)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TaxiRequestData.HasComponent(taxi.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(256U, 16U) - 1) != 6)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TaxiRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TaxiRequest>(jobIndex, entity1, new TaxiRequest(entity, Entity.Null, Entity.Null, (taxi.m_State & TaxiFlags.FromOutside) != (TaxiFlags) 0 ? TaxiRequestType.Outside : TaxiRequestType.None, 1));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(16U));
      }

      private bool SelectDispatch(
        int jobIndex,
        Entity entity,
        CurrentRoute currentRoute,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Taxi taxi,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        taxi.m_State &= ~TaxiFlags.Dispatched;
        if (serviceDispatches.Length == 0 || (taxi.m_State & TaxiFlags.Requested) == (TaxiFlags) 0)
        {
          taxi.m_State &= ~TaxiFlags.Requested;
          serviceDispatches.Clear();
          return false;
        }
        Entity request = serviceDispatches[0].m_Request;
        taxi.m_State &= ~TaxiFlags.Requested;
        if ((taxi.m_State & (TaxiFlags.RequiresMaintenance | TaxiFlags.Disabled)) != (TaxiFlags) 0)
        {
          serviceDispatches.Clear();
          return false;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TaxiRequestData.HasComponent(request))
        {
          serviceDispatches.Clear();
          return false;
        }
        // ISSUE: reference to a compiler-generated field
        TaxiRequest taxiRequest = this.m_TaxiRequestData[request];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(taxiRequest.m_Seeker))
        {
          serviceDispatches.Clear();
          return false;
        }
        taxi.m_State &= ~TaxiFlags.Returning;
        if (taxiRequest.m_Type == TaxiRequestType.Customer || taxiRequest.m_Type == TaxiRequestType.Outside)
        {
          taxi.m_State |= TaxiFlags.Dispatched;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, entity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(request, entity, false, true));
        }
        else
        {
          serviceDispatches.Clear();
          // ISSUE: reference to a compiler-generated field
          if (this.m_BoardingVehicleData.HasComponent(taxiRequest.m_Seeker))
          {
            if (currentRoute.m_Route != taxiRequest.m_Seeker)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CurrentRoute>(jobIndex, entity, new CurrentRoute(taxiRequest.m_Seeker));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AppendToBuffer<RouteVehicle>(jobIndex, taxiRequest.m_Seeker, new RouteVehicle(entity));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, entity);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(request, entity, true));
        }
        car.m_Flags |= CarFlags.StayOnRoad | CarFlags.UsePublicTransportLanes;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TaxiRequestData.HasComponent(taxi.m_TargetRequest))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(taxi.m_TargetRequest, Entity.Null, true));
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathElements.HasBuffer(request))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[request];
          if (pathElement1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[entity];
            PathUtils.TrimPath(pathElement2, ref pathOwner);
            // ISSUE: reference to a compiler-generated field
            float num = taxi.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes))
            {
              taxi.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
              taxi.m_ExtraPathElementCount = 0;
              taxi.m_State &= ~TaxiFlags.Arriving;
              target.m_Target = taxiRequest.m_Seeker;
              VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.ResetParkingLaneStatus(entity, ref currentLane, ref pathOwner, pathElement2, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
              return true;
            }
          }
        }
        VehicleUtils.SetTarget(ref pathOwner, ref target, taxiRequest.m_Seeker);
        return true;
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Taxi taxi,
        ref Car car,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        serviceDispatches.Clear();
        taxi.m_State &= ~(TaxiFlags.Requested | TaxiFlags.Disembarking | TaxiFlags.Dispatched);
        taxi.m_State |= TaxiFlags.Returning;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        VehicleUtils.SetTarget(ref pathOwner, ref target, ownerData.m_Owner);
      }

      private void CheckParkingSpace(
        Entity entity,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.Taxi taxi,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[entity];
        bool flag = (taxi.m_State & TaxiFlags.Returning) == (TaxiFlags) 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = VehicleUtils.ValidateParkingSpace(entity, ref random, ref currentLane, ref pathOwner, navigationLanes, pathElement, ref this.m_ParkedCarData, ref this.m_BlockerData, ref this.m_CurveData, ref this.m_UnspawnedData, ref this.m_ParkingLaneData, ref this.m_GarageLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabParkingLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, flag, false, flag);
        Game.Net.ParkingLane componentData;
        // ISSUE: reference to a compiler-generated field
        if (!(entity1 != Entity.Null) || !this.m_ParkingLaneData.TryGetComponent(entity1, out componentData))
          return;
        taxi.m_NextStartingFee = componentData.m_TaxiFee;
      }

      private void ResetPath(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Taxi taxi,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        CarData prefabCarData)
      {
        taxi.m_NextStartingFee = (ushort) 0;
        taxi.m_State &= ~TaxiFlags.Arriving;
        if ((taxi.m_State & TaxiFlags.Returning) != (TaxiFlags) 0)
        {
          car.m_Flags &= ~CarFlags.StayOnRoad;
          car.m_Flags |= CarFlags.UsePublicTransportLanes;
        }
        else
          car.m_Flags |= CarFlags.StayOnRoad | CarFlags.UsePublicTransportLanes;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.ResetParkingLaneStatus(entity, ref currentLane, ref pathOwner, pathElement, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
        // ISSUE: reference to a compiler-generated method
        this.ResetPath(ref random, ref taxi, entity, ref currentLane, ref pathOwner, prefabCarData);
      }

      private void ResetPath(
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.Taxi taxi,
        Entity entity,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        CarData prefabCarData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathUtils.ResetPath(ref currentLane, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
        bool ignoreDriveways = (taxi.m_State & TaxiFlags.Returning) == (TaxiFlags) 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num1 = VehicleUtils.SetParkingCurvePos(entity, ref random, currentLane, pathOwner, pathElement1, ref this.m_ParkedCarData, ref this.m_UnspawnedData, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, ref this.m_PrefabParkingLaneData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, ignoreDriveways);
        taxi.m_ExtraPathElementCount = math.max(0, pathElement1.Length - (num1 + 1));
        taxi.m_PathElementTime = 0.0f;
        int num2 = 0;
        for (int elementIndex = pathOwner.m_ElementIndex; elementIndex < num1; ++elementIndex)
        {
          PathElement pathElement2 = pathElement1[elementIndex];
          Game.Net.CarLane componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.TryGetComponent(pathElement2.m_Target, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[pathElement2.m_Target];
            taxi.m_PathElementTime += curve.m_Length / math.min(prefabCarData.m_MaxSpeed, componentData.m_SpeedLimit);
            ++num2;
          }
        }
        if (num2 == 0)
          return;
        taxi.m_PathElementTime /= (float) num2;
      }

      private bool StartBoarding(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Taxi taxi,
        ref Game.Common.Target target)
      {
        if ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0)
        {
          if (serviceDispatches.Length == 0)
          {
            taxi.m_State &= ~TaxiFlags.Dispatched;
            return false;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_TaxiRequestData.HasComponent(serviceDispatches[0].m_Request))
          {
            taxi.m_State |= TaxiFlags.Boarding;
            taxi.m_MaxBoardingDistance = 0.0f;
            taxi.m_MinWaitingDistance = float.MaxValue;
            return true;
          }
          taxi.m_State &= ~TaxiFlags.Dispatched;
          serviceDispatches.Clear();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TaxiStandData.HasComponent(currentRoute.m_Route))
          {
            // ISSUE: reference to a compiler-generated field
            taxi.m_NextStartingFee = this.m_TaxiStandData[currentRoute.m_Route].m_StartingFee;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Add(currentRoute.m_Route, vehicleEntity));
            return true;
          }
        }
        return false;
      }

      private Entity FindLeader(DynamicBuffer<Passenger> passengers)
      {
        for (int index = 0; index < passengers.Length; ++index)
        {
          Entity passenger = passengers[index].m_Passenger;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentVehicleData.HasComponent(passenger) && (this.m_CurrentVehicleData[passenger].m_Flags & CreatureVehicleFlags.Leader) != (CreatureVehicleFlags) 0)
            return passenger;
        }
        return Entity.Null;
      }

      private bool StopBoarding(
        int jobIndex,
        Entity entity,
        ref Unity.Mathematics.Random random,
        Odometer odometer,
        PrefabRef prefabRef,
        CurrentRoute currentRoute,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Taxi taxi,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        taxi.m_MaxBoardingDistance = math.select(taxi.m_MinWaitingDistance + 0.5f, float.MaxValue, (double) taxi.m_MinWaitingDistance == 3.4028234663852886E+38);
        taxi.m_MinWaitingDistance = float.MaxValue;
        Entity entity1 = Entity.Null;
        for (int index = 0; index < passengers.Length; ++index)
        {
          Entity passenger = passengers[index].m_Passenger;
          CurrentVehicle componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentVehicleData.TryGetComponent(passenger, out componentData1))
          {
            if ((componentData1.m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
              return false;
            if ((componentData1.m_Flags & CreatureVehicleFlags.Leader) != (CreatureVehicleFlags) 0)
              entity1 = passenger;
          }
          else
          {
            Game.Creatures.Resident componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResidentData.TryGetComponent(passenger, out componentData2) && (componentData2.m_Flags & ResidentFlags.InVehicle) != ResidentFlags.None)
              return false;
          }
        }
        if (entity1 == Entity.Null)
        {
          if ((taxi.m_State & TaxiFlags.Dispatched) != (TaxiFlags) 0)
          {
            if (serviceDispatches.Length != 0)
            {
              Entity request = serviceDispatches[0].m_Request;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TaxiRequestData.HasComponent(request))
              {
                // ISSUE: reference to a compiler-generated field
                TaxiRequest taxiRequest = this.m_TaxiRequestData[request];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_RideNeederData.HasComponent(taxiRequest.m_Seeker) && this.m_RideNeederData[taxiRequest.m_Seeker].m_RideRequest == request)
                  return false;
              }
            }
            serviceDispatches.Clear();
            taxi.m_State &= ~TaxiFlags.Dispatched;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Remove(currentRoute.m_Route, entity));
            }
            else
              taxi.m_State &= ~TaxiFlags.Boarding;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, entity);
            return true;
          }
          if ((taxi.m_State & (TaxiFlags.Requested | TaxiFlags.Disabled)) != (TaxiFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Remove(currentRoute.m_Route, entity));
            }
            else
              taxi.m_State &= ~TaxiFlags.Boarding;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, entity);
            return true;
          }
          if (VehicleUtils.RequireNewPath(pathOwner))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Remove(currentRoute.m_Route, entity));
            }
            else
              taxi.m_State &= ~TaxiFlags.Boarding;
            return true;
          }
          BoardingVehicle componentData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BoardingVehicleData.TryGetComponent(currentRoute.m_Route, out componentData) || !(componentData.m_Vehicle != entity))
            return false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Remove(currentRoute.m_Route, entity));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, entity);
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_BoardingVehicleData.HasComponent(currentRoute.m_Route))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_RouteVehicleQueue.Enqueue(TaxiAISystem.RouteVehicleUpdate.Remove(currentRoute.m_Route, entity));
        }
        else
          taxi.m_State &= ~TaxiFlags.Boarding;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[entity1];
        // ISSUE: reference to a compiler-generated field
        PathOwner pathOwner1 = this.m_PathOwnerData[entity1];
        // ISSUE: reference to a compiler-generated field
        Game.Common.Target target1 = this.m_TargetData[entity1];
        PathOwner sourceOwner = pathOwner1;
        DynamicBuffer<PathElement> targetElements = pathElement1;
        PathUtils.CopyPath(pathElement2, sourceOwner, 1, targetElements);
        pathOwner.m_ElementIndex = 0;
        serviceDispatches.Clear();
        taxi.m_State &= ~(TaxiFlags.Arriving | TaxiFlags.Dispatched);
        taxi.m_State |= TaxiFlags.Transporting;
        taxi.m_StartDistance = odometer.m_Distance;
        taxi.m_CurrentFee = taxi.m_NextStartingFee;
        target.m_Target = target1.m_Target;
        VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
        // ISSUE: reference to a compiler-generated field
        CarData prefabCarData = this.m_PrefabCarData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.ResetParkingLaneStatus(entity, ref currentLane, ref pathOwner, pathElement1, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
        // ISSUE: reference to a compiler-generated method
        this.ResetPath(ref random, ref taxi, entity, ref currentLane, ref pathOwner, prefabCarData);
        return true;
      }

      private bool StartDisembarking(
        Odometer odometer,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.Taxi taxi)
      {
        if ((taxi.m_State & TaxiFlags.Transporting) != (TaxiFlags) 0 && passengers.Length != 0)
        {
          taxi.m_State &= ~TaxiFlags.Transporting;
          taxi.m_State |= TaxiFlags.Disembarking;
          int num = Mathf.RoundToInt(math.max(0.0f, odometer.m_Distance - taxi.m_StartDistance) * 0.03f);
          taxi.m_CurrentFee = (ushort) math.clamp((int) taxi.m_CurrentFee + num, 0, (int) ushort.MaxValue);
          return true;
        }
        taxi.m_State &= ~TaxiFlags.Transporting;
        return false;
      }

      private bool StopDisembarking(
        Entity vehicleEntity,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.Taxi taxi,
        ref PathOwner pathOwner)
      {
        if (passengers.Length != 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.m_PathElements[vehicleEntity].Clear();
        pathOwner.m_ElementIndex = 0;
        taxi.m_State &= ~TaxiFlags.Disembarking;
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
    private struct UpdateRouteVehiclesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public NativeQueue<TaxiAISystem.RouteVehicleUpdate> m_RouteVehicleQueue;
      public ComponentLookup<Game.Vehicles.Taxi> m_TaxiData;
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        TaxiAISystem.RouteVehicleUpdate routeVehicleUpdate;
        // ISSUE: reference to a compiler-generated field
        while (this.m_RouteVehicleQueue.TryDequeue(out routeVehicleUpdate))
        {
          // ISSUE: reference to a compiler-generated field
          if (routeVehicleUpdate.m_RemoveVehicle != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RemoveVehicle(routeVehicleUpdate.m_RemoveVehicle, routeVehicleUpdate.m_Route);
          }
          // ISSUE: reference to a compiler-generated field
          if (routeVehicleUpdate.m_AddVehicle != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddVehicle(routeVehicleUpdate.m_AddVehicle, routeVehicleUpdate.m_Route);
          }
        }
      }

      private void RemoveVehicle(Entity vehicle, Entity route)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_BoardingVehicleData.HasComponent(route))
        {
          // ISSUE: reference to a compiler-generated field
          BoardingVehicle boardingVehicle = this.m_BoardingVehicleData[route];
          if (boardingVehicle.m_Vehicle == vehicle)
          {
            boardingVehicle.m_Vehicle = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            this.m_BoardingVehicleData[route] = boardingVehicle;
          }
        }
        // ISSUE: reference to a compiler-generated field
        Game.Vehicles.Taxi taxi = this.m_TaxiData[vehicle];
        if ((taxi.m_State & TaxiFlags.Boarding) == (TaxiFlags) 0)
          return;
        taxi.m_State &= ~TaxiFlags.Boarding;
        // ISSUE: reference to a compiler-generated field
        this.m_TaxiData[vehicle] = taxi;
      }

      private void AddVehicle(Entity vehicle, Entity route)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_BoardingVehicleData.HasComponent(route))
        {
          // ISSUE: reference to a compiler-generated field
          BoardingVehicle boardingVehicle = this.m_BoardingVehicleData[route];
          if (boardingVehicle.m_Vehicle != vehicle)
          {
            Game.Vehicles.Taxi componentData;
            // ISSUE: reference to a compiler-generated field
            if (boardingVehicle.m_Vehicle != Entity.Null && this.m_TaxiData.TryGetComponent(boardingVehicle.m_Vehicle, out componentData) && (componentData.m_State & TaxiFlags.Boarding) != (TaxiFlags) 0)
              return;
            boardingVehicle.m_Vehicle = vehicle;
            // ISSUE: reference to a compiler-generated field
            this.m_BoardingVehicleData[route] = boardingVehicle;
          }
        }
        // ISSUE: reference to a compiler-generated field
        Game.Vehicles.Taxi taxi = this.m_TaxiData[vehicle];
        if ((taxi.m_State & TaxiFlags.Boarding) != (TaxiFlags) 0)
          return;
        taxi.m_State |= TaxiFlags.Boarding;
        taxi.m_MaxBoardingDistance = 0.0f;
        taxi.m_MinWaitingDistance = float.MaxValue;
        // ISSUE: reference to a compiler-generated field
        this.m_TaxiData[vehicle] = taxi;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Blocker> __Game_Vehicles_Blocker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stopped> __Game_Objects_Stopped_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> __Game_Routes_BoardingVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaitingPassengers> __Game_Routes_WaitingPassengers_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiData> __Game_Prefabs_TaxiData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanData> __Game_Prefabs_HumanData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> __Game_Simulation_TaxiRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Divert> __Game_Creatures_Divert_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RideNeeder> __Game_Creatures_RideNeeder_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RW_ComponentLookup;
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public ComponentLookup<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RW_ComponentLookup;
      public ComponentLookup<BoardingVehicle> __Game_Routes_BoardingVehicle_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.Taxi>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PersonalCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RO_ComponentLookup = state.GetComponentLookup<Blocker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentLookup = state.GetComponentLookup<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BoardingVehicle_RO_ComponentLookup = state.GetComponentLookup<BoardingVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentLookup = state.GetComponentLookup<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RO_ComponentLookup = state.GetComponentLookup<WaitingPassengers>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TaxiData_RO_ComponentLookup = state.GetComponentLookup<TaxiData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentLookup = state.GetComponentLookup<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HumanData_RO_ComponentLookup = state.GetComponentLookup<HumanData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TaxiRequest_RO_ComponentLookup = state.GetComponentLookup<TaxiRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Divert_RO_ComponentLookup = state.GetComponentLookup<Divert>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_RideNeeder_RO_ComponentLookup = state.GetComponentLookup<RideNeeder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RO_ComponentLookup = state.GetComponentLookup<CarKeeper>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RO_BufferLookup = state.GetBufferLookup<RouteVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentLookup = state.GetComponentLookup<Game.Common.Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentLookup = state.GetComponentLookup<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Taxi>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BoardingVehicle_RW_ComponentLookup = state.GetComponentLookup<BoardingVehicle>();
      }
    }
  }
}
