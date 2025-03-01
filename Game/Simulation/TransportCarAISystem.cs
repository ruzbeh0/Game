﻿// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportCarAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Economy;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TransportCarAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_TransportVehicleRequestArchetype;
    private EntityArchetype m_EvacuationRequestArchetype;
    private EntityArchetype m_PrisonerTransportRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private TransportBoardingHelpers.BoardingLookupData m_BoardingLookupData;
    private TransportCarAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 1;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingLookupData = new TransportBoardingHelpers.BoardingLookupData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadWrite<CarCurrentLane>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadWrite<PathOwner>(),
          ComponentType.ReadWrite<Target>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Game.Vehicles.CargoTransport>(),
          ComponentType.ReadWrite<Game.Vehicles.PublicTransport>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<TripSource>(),
          ComponentType.ReadOnly<OutOfControl>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TransportVehicleRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_EvacuationRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<EvacuationRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrisonerTransportRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PrisonerTransportRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedCarRemoveTypes = new ComponentTypeSet(new ComponentType[13]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
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
      TransportBoardingHelpers.BoardingData boardingData = new TransportBoardingHelpers.BoardingData(Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingLookupData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PrisonerTransportRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_EvacuationRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new TransportCarAISystem.TransportCarTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CurrentRouteType = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_CargoTransportType = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle,
        m_PublicTransportType = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PublicTransportVehicleData = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_CargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_TransportVehicleRequestData = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup,
        m_EvacuationRequestData = this.__TypeHandle.__Game_Simulation_EvacuationRequest_RO_ComponentLookup,
        m_PrisonerTransportRequestData = this.__TypeHandle.__Game_Simulation_PrisonerTransportRequest_RO_ComponentLookup,
        m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_BoardingVehicleData = this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_RouteColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_StorageCompanyData = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_TransportStationData = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup,
        m_TransportDepotData = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup,
        m_PrisonData = this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup,
        m_EmergencyShelterData = this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_LoadingResources = this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_TransportVehicleRequestArchetype = this.m_TransportVehicleRequestArchetype,
        m_EvacuationRequestArchetype = this.m_EvacuationRequestArchetype,
        m_PrisonerTransportRequestArchetype = this.m_PrisonerTransportRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_BoardingData = boardingData.ToConcurrent()
      }.ScheduleParallel<TransportCarAISystem.TransportCarTickJob>(this.m_VehicleQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = boardingData.ScheduleBoarding((SystemBase) this, this.m_CityStatisticsSystem, this.m_BoardingLookupData, this.m_SimulationSystem.frameIndex, jobHandle);
      boardingData.Dispose(inputDeps);
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
    public TransportCarAISystem()
    {
    }

    [BurstCompile]
    private struct TransportCarTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> m_CurrentRouteType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<CarNavigationLane> m_CarNavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PublicTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_CargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> m_EvacuationRequestData;
      [ReadOnly]
      public ComponentLookup<PrisonerTransportRequest> m_PrisonerTransportRequestData;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanyData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> m_TransportStationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> m_PrisonData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> m_EmergencyShelterData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LoadingResources> m_LoadingResources;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_TransportVehicleRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_EvacuationRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_PrisonerTransportRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public TransportBoardingHelpers.BoardingData.Concurrent m_BoardingData;

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
        NativeArray<PathInformation> nativeArray3 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentRoute> nativeArray5 = chunk.GetNativeArray<CurrentRoute>(ref this.m_CurrentRouteType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray6 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.CargoTransport> nativeArray7 = chunk.GetNativeArray<Game.Vehicles.CargoTransport>(ref this.m_CargoTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.PublicTransport> nativeArray8 = chunk.GetNativeArray<Game.Vehicles.PublicTransport>(ref this.m_PublicTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray9 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray10 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray11 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Odometer> nativeArray12 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor2 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray4[index];
          PathInformation pathInformation = nativeArray3[index];
          Car car = nativeArray9[index];
          CarCurrentLane currentLane = nativeArray6[index];
          PathOwner pathOwner = nativeArray11[index];
          Target target = nativeArray10[index];
          Odometer odometer = nativeArray12[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          CurrentRoute currentRoute = new CurrentRoute();
          if (nativeArray5.Length != 0)
            currentRoute = nativeArray5[index];
          Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
          if (nativeArray7.Length != 0)
            cargoTransport = nativeArray7[index];
          Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
          DynamicBuffer<Passenger> passengers = new DynamicBuffer<Passenger>();
          if (nativeArray8.Length != 0)
          {
            publicTransport = nativeArray8[index];
            passengers = bufferAccessor2[index];
          }
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, pathInformation, prefabRef, currentRoute, navigationLanes, passengers, serviceDispatches, ref random, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, ref target, ref odometer);
          nativeArray9[index] = car;
          nativeArray6[index] = currentLane;
          nativeArray11[index] = pathOwner;
          nativeArray10[index] = target;
          nativeArray12[index] = odometer;
          if (nativeArray7.Length != 0)
            nativeArray7[index] = cargoTransport;
          if (nativeArray8.Length != 0)
            nativeArray8[index] = publicTransport;
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        PathInformation pathInformation,
        PrefabRef prefabRef,
        CurrentRoute currentRoute,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Random random,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        ref Odometer odometer)
      {
        PublicTransportVehicleData componentData1;
        // ISSUE: reference to a compiler-generated field
        bool component1 = this.m_PublicTransportVehicleData.TryGetComponent(prefabRef.m_Prefab, out componentData1);
        CargoTransportVehicleData componentData2;
        // ISSUE: reference to a compiler-generated field
        bool component2 = this.m_CargoTransportVehicleData.TryGetComponent(prefabRef.m_Prefab, out componentData2);
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, pathInformation, serviceDispatches, ref random, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, component1);
          DynamicBuffer<LoadingResources> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (((publicTransport.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0 || (cargoTransport.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0) && this.m_LoadingResources.TryGetBuffer(vehicleEntity, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckDummyResources(jobIndex, vehicleEntity, prefabRef, bufferData);
          }
        }
        bool flag1 = (cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0;
        bool flag2 = false;
        if (component1)
        {
          if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) != (PublicTransportFlags) 0)
          {
            if (!passengers.IsCreated || passengers.Length >= componentData1.m_PassengerCapacity)
            {
              publicTransport.m_State |= PublicTransportFlags.Full;
              flag1 = false;
            }
            else
              publicTransport.m_State &= ~PublicTransportFlags.Full;
            flag2 = true;
          }
          if ((double) odometer.m_Distance >= (double) componentData1.m_MaintenanceRange && (double) componentData1.m_MaintenanceRange > 0.10000000149011612 && (publicTransport.m_State & PublicTransportFlags.Refueling) == (PublicTransportFlags) 0)
            publicTransport.m_State |= PublicTransportFlags.RequiresMaintenance;
        }
        if (component2 && (double) odometer.m_Distance >= (double) componentData2.m_MaintenanceRange && (double) componentData2.m_MaintenanceRange > 0.10000000149011612 && (cargoTransport.m_State & CargoTransportFlags.Refueling) == (CargoTransportFlags) 0)
          cargoTransport.m_State |= CargoTransportFlags.RequiresMaintenance;
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, flag2, ref cargoTransport, ref publicTransport, ref pathOwner);
          if (serviceDispatches.Length <= math.select(0, 1, flag2) && (cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.DummyTraffic | CargoTransportFlags.Disabled)) == (CargoTransportFlags) 0 && (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.DummyTraffic | PublicTransportFlags.Disabled)) == (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref publicTransport, ref cargoTransport);
          }
        }
        else
        {
          serviceDispatches.Clear();
          cargoTransport.m_RequestCount = 0;
          publicTransport.m_RequestCount = 0;
        }
        bool flag3 = false;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            flag3 = true;
            // ISSUE: reference to a compiler-generated method
            this.StopBoarding(vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, true);
          }
          if (VehicleUtils.IsStuck(pathOwner) || (cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane) || VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
        {
          if ((cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.StopBoarding(vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, false))
              {
                flag3 = true;
                // ISSUE: reference to a compiler-generated method
                if (!this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, ref target, component1))
                {
                  if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.ParkCar(jobIndex, vehicleEntity, owner, ref cargoTransport, ref publicTransport, ref car, ref currentLane);
                    return;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
                  return;
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if ((!passengers.IsCreated || passengers.Length <= 0 || !this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, component2)) && !this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, ref target, component1))
              {
                if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ParkCar(jobIndex, vehicleEntity, owner, ref cargoTransport, ref publicTransport, ref car, ref currentLane);
                  return;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
                return;
              }
            }
          }
          else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.StopBoarding(vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, false))
            {
              flag3 = true;
              if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) != (PublicTransportFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, ref target, component1))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref pathOwner, ref target);
                }
              }
              else if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref pathOwner, ref target);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) == (PublicTransportFlags) 0 && (!this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) || !this.m_WaypointData.HasComponent(target.m_Target)))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref pathOwner, ref target);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, component2))
              {
                if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) != (PublicTransportFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  if (!this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, ref target, component1))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref pathOwner, ref target);
                  }
                }
                else if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref pathOwner, ref target);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
                }
              }
            }
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          flag3 = true;
          // ISSUE: reference to a compiler-generated method
          this.StopBoarding(vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, true);
        }
        publicTransport.m_State &= ~(PublicTransportFlags.StopLeft | PublicTransportFlags.StopRight);
        Entity skipWaypoint = Entity.Null;
        if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          if (!flag3)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateStop(navigationLanes, ref currentLane, ref publicTransport, ref target);
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
        {
          if (!passengers.IsCreated || passengers.Length == 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref car, ref currentLane, ref pathOwner, ref target, component1);
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Arriving) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Arriving) != (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateStop(navigationLanes, ref currentLane, ref publicTransport, ref target);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckNavigationLanes(vehicleEntity, currentRoute, navigationLanes, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target, out skipWaypoint);
        }
        cargoTransport.m_State &= ~CargoTransportFlags.Testing;
        publicTransport.m_State &= ~PublicTransportFlags.Testing;
        if ((((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 ? 0 : ((publicTransport.m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0 ? 1 : 0)) | (flag3 ? 1 : 0)) == 0)
          return;
        if (VehicleUtils.RequireNewPath(pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.FindNewPath(vehicleEntity, prefabRef, skipWaypoint, ref currentLane, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
        }
        else
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) != (PathFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingSpace(vehicleEntity, ref random, ref currentLane, ref pathOwner, navigationLanes);
        }
      }

      private void UpdateStop(
        DynamicBuffer<CarNavigationLane> navigationLanes,
        ref CarCurrentLane currentLane,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target)
      {
        Connected componentData1;
        Transform componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) || !this.m_TransformData.TryGetComponent(componentData1.m_Connected, out componentData2))
          return;
        Entity lane = Entity.Null;
        float2 float2 = (float2) 0.0f;
        for (int index = navigationLanes.Length - 1; index >= 0; --index)
        {
          CarNavigationLane navigationLane = navigationLanes[index];
          if ((double) navigationLane.m_CurvePosition.y - (double) navigationLane.m_CurvePosition.x != 0.0)
          {
            lane = navigationLane.m_Lane;
            float2 = navigationLane.m_CurvePosition;
            break;
          }
        }
        if ((double) float2.x == (double) float2.y)
        {
          lane = currentLane.m_Lane;
          float2 = currentLane.m_CurvePosition.xz;
        }
        Curve componentData3;
        // ISSUE: reference to a compiler-generated field
        if ((double) float2.x == (double) float2.y || !this.m_CurveData.TryGetComponent(lane, out componentData3))
          return;
        float3 float3 = MathUtils.Position(componentData3.m_Bezier, float2.y);
        float2 xz1 = float3.xz;
        float3 = MathUtils.Tangent(componentData3.m_Bezier, float2.y);
        float2 xz2 = float3.xz;
        float2 y = componentData2.m_Position.xz - xz1;
        if ((double) math.dot(MathUtils.Left(math.select(xz2, -xz2, (double) float2.y < (double) float2.x)), y) > 0.0)
        {
          publicTransport.m_State |= PublicTransportFlags.StopLeft;
          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.TurnLeft;
        }
        else
        {
          publicTransport.m_State |= PublicTransportFlags.StopRight;
          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.TurnRight;
        }
      }

      private void CheckParkingSpace(
        Entity entity,
        ref Random random,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[entity];
        ComponentLookup<Blocker> blockerData = new ComponentLookup<Blocker>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.ValidateParkingSpace(entity, ref random, ref currentLane, ref pathOwner, navigationLanes, pathElement, ref this.m_ParkedCarData, ref blockerData, ref this.m_CurveData, ref this.m_UnspawnedData, ref this.m_ParkingLaneData, ref this.m_GarageLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabParkingLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false, false, false);
      }

      private void ParkCar(
        int jobIndex,
        Entity entity,
        Owner owner,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        car.m_Flags &= ~CarFlags.Emergency;
        cargoTransport.m_State &= CargoTransportFlags.RequiresMaintenance;
        publicTransport.m_State &= PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport | PublicTransportFlags.RequiresMaintenance;
        if ((publicTransport.m_State & PublicTransportFlags.PrisonerTransport) != (PublicTransportFlags) 0)
        {
          Game.Buildings.Prison componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrisonData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & (PrisonFlags.HasAvailablePrisonVans | PrisonFlags.HasPrisonerSpace)) != (PrisonFlags.HasAvailablePrisonVans | PrisonFlags.HasPrisonerSpace))
          {
            cargoTransport.m_State |= CargoTransportFlags.Disabled;
            publicTransport.m_State |= PublicTransportFlags.Disabled;
          }
        }
        else if ((publicTransport.m_State & PublicTransportFlags.Evacuating) != (PublicTransportFlags) 0)
        {
          Game.Buildings.EmergencyShelter componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EmergencyShelterData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & (EmergencyShelterFlags.HasAvailableVehicles | EmergencyShelterFlags.HasShelterSpace)) != (EmergencyShelterFlags.HasAvailableVehicles | EmergencyShelterFlags.HasShelterSpace))
          {
            cargoTransport.m_State |= CargoTransportFlags.Disabled;
            publicTransport.m_State |= PublicTransportFlags.Disabled;
          }
        }
        else
        {
          Game.Buildings.TransportDepot componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportDepotData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & TransportDepotFlags.HasAvailableVehicles) == (TransportDepotFlags) 0)
          {
            cargoTransport.m_State |= CargoTransportFlags.Disabled;
            publicTransport.m_State |= PublicTransportFlags.Disabled;
          }
        }
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

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        Entity skipWaypoint,
        ref CarCurrentLane currentLane,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) carData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Methods = PathMethod.Road | PathMethod.SpecialParking,
          m_ParkingTarget = VehicleUtils.GetParkingSource(vehicleEntity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
          m_ParkingDelta = currentLane.m_CurvePosition.z,
          m_ParkingSize = VehicleUtils.GetParkingSize(vehicleEntity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData),
          m_IgnoredRules = RuleFlags.ForbidPrivateTraffic | VehicleUtils.GetIgnoredPathfindRules(carData)
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.SpecialParking,
          m_RoadTypes = RoadTypes.Car
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = target.m_Target
        };
        if ((publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.Evacuating)) == PublicTransportFlags.Evacuating)
        {
          parameters.m_Weights = new PathfindWeights(1f, 0.2f, 0.0f, 0.1f);
          parameters.m_IgnoredRules |= RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic;
        }
        else
          parameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
        if (skipWaypoint != Entity.Null)
        {
          origin.m_Entity = skipWaypoint;
          pathOwner.m_State |= PathFlags.Append;
        }
        else
          pathOwner.m_State &= ~PathFlags.Append;
        if ((cargoTransport.m_State & (CargoTransportFlags.EnRoute | CargoTransportFlags.RouteSource)) == (CargoTransportFlags.EnRoute | CargoTransportFlags.RouteSource) || (publicTransport.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.RouteSource)) == (PublicTransportFlags.EnRoute | PublicTransportFlags.RouteSource))
          parameters.m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow;
        else if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
        {
          cargoTransport.m_State &= ~CargoTransportFlags.RouteSource;
          publicTransport.m_State &= ~PublicTransportFlags.RouteSource;
        }
        if ((cargoTransport.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
        {
          destination.m_Methods |= PathMethod.SpecialParking;
          destination.m_RandomCost = 30f;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckNavigationLanes(
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        out Entity skipWaypoint)
      {
        skipWaypoint = Entity.Null;
        if (navigationLanes.Length >= 8)
          return;
        CarNavigationLane carNavigationLane = new CarNavigationLane();
        if (navigationLanes.Length != 0)
        {
          carNavigationLane = navigationLanes[navigationLanes.Length - 1];
          if ((carNavigationLane.m_Flags & Game.Vehicles.CarLaneFlags.EndOfPath) == (Game.Vehicles.CarLaneFlags) 0)
            return;
        }
        else if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.EndOfPath) == (Game.Vehicles.CarLaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaypointData.HasComponent(target.m_Target) && this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) && (!this.m_ConnectedData.HasComponent(target.m_Target) || !this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected)))
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete)) != (PathFlags) 0)
            return;
          skipWaypoint = target.m_Target;
          // ISSUE: reference to a compiler-generated method
          this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
          if (navigationLanes.Length != 0)
          {
            if ((carNavigationLane.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) != (Game.Vehicles.CarLaneFlags) 0)
            {
              navigationLanes.RemoveAt(navigationLanes.Length - 1);
            }
            else
            {
              carNavigationLane.m_Flags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
              navigationLanes[navigationLanes.Length - 1] = carNavigationLane;
            }
          }
          else
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaypointData.HasComponent(target.m_Target) && this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route))
          {
            // ISSUE: reference to a compiler-generated field
            Connected connected = this.m_ConnectedData[target.m_Target];
            // ISSUE: reference to a compiler-generated method
            if (this.GetTransportStationFromStop(connected.m_Connected) == Entity.Null && (cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.AbandonRoute)) == (CargoTransportFlags) 0 && (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.AbandonRoute)) == (PublicTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_BoardingVehicleData[connected.m_Connected].m_Testing == vehicleEntity)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_BoardingData.EndTesting(vehicleEntity, currentRoute.m_Route, connected.m_Connected, target.m_Target);
                if ((cargoTransport.m_State & CargoTransportFlags.RequireStop) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.RequireStop) == (PublicTransportFlags) 0)
                {
                  if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete)) != (PathFlags) 0)
                    return;
                  skipWaypoint = target.m_Target;
                  // ISSUE: reference to a compiler-generated method
                  this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
                  if (navigationLanes.Length != 0)
                  {
                    if ((carNavigationLane.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) != (Game.Vehicles.CarLaneFlags) 0)
                    {
                      navigationLanes.RemoveAt(navigationLanes.Length - 1);
                    }
                    else
                    {
                      carNavigationLane.m_Flags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
                      navigationLanes[navigationLanes.Length - 1] = carNavigationLane;
                    }
                  }
                  else
                    currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
                  cargoTransport.m_State |= CargoTransportFlags.RouteSource;
                  publicTransport.m_State |= PublicTransportFlags.RouteSource;
                  return;
                }
              }
              else
              {
                if (navigationLanes.Length != 0 && (carNavigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Reserved) == (Game.Vehicles.CarLaneFlags) 0)
                {
                  if (navigationLanes.Length < 2)
                    return;
                  CarNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 2];
                  Owner componentData1;
                  Owner componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Reserved) == (Game.Vehicles.CarLaneFlags) 0 || !this.m_OwnerData.TryGetComponent(carNavigationLane.m_Lane, out componentData1) || !this.m_OwnerData.TryGetComponent(navigationLane.m_Lane, out componentData2) || componentData1.m_Owner != componentData2.m_Owner)
                    return;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_BoardingData.BeginTesting(vehicleEntity, currentRoute.m_Route, connected.m_Connected, target.m_Target);
                return;
              }
            }
          }
          cargoTransport.m_State |= CargoTransportFlags.Arriving;
          publicTransport.m_State |= PublicTransportFlags.Arriving;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_RouteLaneData.HasComponent(target.m_Target))
            return;
          // ISSUE: reference to a compiler-generated field
          RouteLane routeLane = this.m_RouteLaneData[target.m_Target];
          if (routeLane.m_StartLane != routeLane.m_EndLane)
          {
            CarNavigationLane elem = new CarNavigationLane();
            if (navigationLanes.Length != 0)
            {
              carNavigationLane.m_CurvePosition.y = 1f;
              elem.m_Lane = carNavigationLane.m_Lane;
            }
            else
            {
              currentLane.m_CurvePosition.z = 1f;
              elem.m_Lane = currentLane.m_Lane;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (NetUtils.FindNextLane(ref elem.m_Lane, ref this.m_OwnerData, ref this.m_LaneData, ref this.m_SubLanes))
            {
              if (navigationLanes.Length != 0)
              {
                carNavigationLane.m_Flags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
                navigationLanes[navigationLanes.Length - 1] = carNavigationLane;
              }
              else
                currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
              elem.m_Flags |= Game.Vehicles.CarLaneFlags.EndOfPath | Game.Vehicles.CarLaneFlags.FixedLane;
              elem.m_CurvePosition = new float2(0.0f, routeLane.m_EndCurvePos);
              navigationLanes.Add(elem);
            }
            else
            {
              if (navigationLanes.Length == 0)
                return;
              navigationLanes[navigationLanes.Length - 1] = carNavigationLane;
            }
          }
          else if (navigationLanes.Length != 0)
          {
            carNavigationLane.m_CurvePosition.y = routeLane.m_EndCurvePos;
            navigationLanes[navigationLanes.Length - 1] = carNavigationLane;
          }
          else
            currentLane.m_CurvePosition.z = routeLane.m_EndCurvePos;
        }
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Random random,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        bool isPublicTransport)
      {
        cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
        publicTransport.m_State &= ~PublicTransportFlags.Arriving;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
        if ((pathOwner.m_State & PathFlags.Append) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PathUtils.ResetPath(ref currentLane, pathElement, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.ResetParkingLaneStatus(vehicleEntity, ref currentLane, ref pathOwner, pathElement, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
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
        VehicleUtils.SetParkingCurvePos(vehicleEntity, ref random, currentLane, pathOwner, pathElement, ref this.m_ParkedCarData, ref this.m_UnspawnedData, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, ref this.m_PrefabParkingLaneData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false);
        if ((cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          car.m_Flags &= ~CarFlags.StayOnRoad;
        else if (cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EvacuationRequestData.HasComponent(request))
          {
            car.m_Flags |= CarFlags.Emergency | CarFlags.StayOnRoad;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrisonerTransportRequestData.HasComponent(request))
            {
              car.m_Flags &= ~(CarFlags.Emergency | CarFlags.StayOnRoad);
            }
            else
            {
              car.m_Flags &= ~CarFlags.Emergency;
              car.m_Flags |= CarFlags.StayOnRoad;
            }
          }
        }
        else
        {
          car.m_Flags &= ~CarFlags.Emergency;
          car.m_Flags |= CarFlags.StayOnRoad;
        }
        if (isPublicTransport)
          car.m_Flags |= CarFlags.UsePublicTransportLanes | CarFlags.PreferPublicTransportLanes | CarFlags.Interior;
        cargoTransport.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement.Length);
        publicTransport.m_PathElementTime = cargoTransport.m_PathElementTime;
      }

      private void CheckDummyResources(
        int jobIndex,
        Entity vehicleEntity,
        PrefabRef prefabRef,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        if (loadingResources.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Resources> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Resources>(jobIndex, vehicleEntity);
          for (int index = 0; index < loadingResources.Length && dynamicBuffer.Length < transportVehicleData.m_MaxResourceCount; ++index)
          {
            LoadingResources loadingResource = loadingResources[index];
            int num = math.min(loadingResource.m_Amount, transportVehicleData.m_CargoCapacity);
            loadingResource.m_Amount -= num;
            transportVehicleData.m_CargoCapacity -= num;
            if (num > 0)
              dynamicBuffer.Add(new Resources()
              {
                m_Resource = loadingResource.m_Resource,
                m_Amount = num
              });
          }
        }
        loadingResources.Clear();
      }

      private void SetNextWaypointTarget(
        CurrentRoute currentRoute,
        ref PathOwner pathOwnerData,
        ref Target targetData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[currentRoute.m_Route];
        // ISSUE: reference to a compiler-generated field
        int a = this.m_WaypointData[targetData.m_Target].m_Index + 1;
        int index = math.select(a, 0, a >= routeWaypoint.Length);
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, routeWaypoint[index].m_Waypoint);
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool allowQueued,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner)
      {
        if (!allowQueued)
        {
          if (serviceDispatches.Length > 1)
            serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
          cargoTransport.m_RequestCount = math.min(1, cargoTransport.m_RequestCount);
          publicTransport.m_RequestCount = math.min(1, publicTransport.m_RequestCount);
        }
        int index1 = math.max(cargoTransport.m_RequestCount, publicTransport.m_RequestCount);
        if (serviceDispatches.Length <= index1)
          return;
        float num1 = -1f;
        Entity request1 = Entity.Null;
        PathElement pathElement1 = new PathElement();
        bool flag = false;
        int num2 = 0;
        if (index1 >= 1 && (cargoTransport.m_State & CargoTransportFlags.Returning) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Returning) == (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[vehicleEntity];
          num2 = 1;
          if (pathOwner.m_ElementIndex < pathElement2.Length)
          {
            pathElement1 = pathElement2[pathElement2.Length - 1];
            flag = true;
          }
        }
        for (int index2 = num2; index2 < index1; ++index2)
        {
          DynamicBuffer<PathElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElements.TryGetBuffer(serviceDispatches[index2].m_Request, out bufferData) && bufferData.Length != 0)
          {
            pathElement1 = bufferData[bufferData.Length - 1];
            flag = true;
          }
        }
        for (int index3 = index1; index3 < serviceDispatches.Length; ++index3)
        {
          Entity request2 = serviceDispatches[index3].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request2))
          {
            // ISSUE: reference to a compiler-generated field
            TransportVehicleRequest transportVehicleRequest = this.m_TransportVehicleRequestData[request2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(transportVehicleRequest.m_Route) && (double) transportVehicleRequest.m_Priority > (double) num1)
            {
              num1 = transportVehicleRequest.m_Priority;
              request1 = request2;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EvacuationRequestData.HasComponent(request2))
            {
              // ISSUE: reference to a compiler-generated field
              EvacuationRequest evacuationRequest = this.m_EvacuationRequestData[request2];
              DynamicBuffer<PathElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (flag && this.m_PathElements.TryGetBuffer(request2, out bufferData) && bufferData.Length != 0)
              {
                PathElement pathElement3 = bufferData[0];
                if (pathElement3.m_Target != pathElement1.m_Target || (double) pathElement3.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(evacuationRequest.m_Target) && (double) evacuationRequest.m_Priority > (double) num1)
              {
                num1 = evacuationRequest.m_Priority;
                request1 = request2;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrisonerTransportRequestData.HasComponent(request2))
              {
                // ISSUE: reference to a compiler-generated field
                PrisonerTransportRequest transportRequest = this.m_PrisonerTransportRequestData[request2];
                DynamicBuffer<PathElement> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (flag && this.m_PathElements.TryGetBuffer(request2, out bufferData) && bufferData.Length != 0)
                {
                  PathElement pathElement4 = bufferData[0];
                  if (pathElement4.m_Target != pathElement1.m_Target || (double) pathElement4.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabRefData.HasComponent(transportRequest.m_Target) && (double) transportRequest.m_Priority > (double) num1)
                {
                  num1 = (float) transportRequest.m_Priority;
                  request1 = request2;
                }
              }
            }
          }
        }
        if (request1 != Entity.Null)
        {
          serviceDispatches[index1++] = new ServiceDispatch(request1);
          ++publicTransport.m_RequestCount;
          ++cargoTransport.m_RequestCount;
        }
        if (serviceDispatches.Length <= index1)
          return;
        serviceDispatches.RemoveRange(index1, serviceDispatches.Length - index1);
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Game.Vehicles.CargoTransport cargoTransport)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(publicTransport.m_TargetRequest) || this.m_ServiceRequestData.HasComponent(cargoTransport.m_TargetRequest))
          return;
        if ((publicTransport.m_State & PublicTransportFlags.Evacuating) != (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (((int) this.m_SimulationFrameIndex & (int) math.max(64U, 16U) - 1) != 1)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_EvacuationRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<EvacuationRequest>(jobIndex, entity1, new EvacuationRequest(entity, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(4U));
        }
        else if ((publicTransport.m_State & PublicTransportFlags.PrisonerTransport) != (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (((int) this.m_SimulationFrameIndex & (int) math.max(256U, 16U) - 1) != 1)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PrisonerTransportRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity2, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrisonerTransportRequest>(jobIndex, entity2, new PrisonerTransportRequest(entity, 1));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity2, new RequestGroup(16U));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (((int) this.m_SimulationFrameIndex & (int) math.max(256U, 16U) - 1) != 1)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TransportVehicleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity3, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<TransportVehicleRequest>(jobIndex, entity3, new TransportVehicleRequest(entity, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity3, new RequestGroup(8U));
        }
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        bool isPublicTransport)
      {
        if ((cargoTransport.m_State & CargoTransportFlags.Returning) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Returning) == (PublicTransportFlags) 0 && cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          cargoTransport.m_RequestCount = math.max(0, cargoTransport.m_RequestCount - 1);
          publicTransport.m_RequestCount = math.max(0, publicTransport.m_RequestCount - 1);
        }
        if ((cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.Disabled)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.Disabled)) != (PublicTransportFlags) 0)
        {
          cargoTransport.m_RequestCount = 0;
          publicTransport.m_RequestCount = 0;
          serviceDispatches.Clear();
          return false;
        }
        for (; cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0; publicTransport.m_RequestCount = math.max(0, publicTransport.m_RequestCount - 1))
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity route = Entity.Null;
          Entity entity1 = Entity.Null;
          CarFlags carFlags = car.m_Flags;
          if (isPublicTransport)
            carFlags |= CarFlags.UsePublicTransportLanes | CarFlags.PreferPublicTransportLanes;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            route = this.m_TransportVehicleRequestData[request].m_Route;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathInformationData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_PathInformationData[request].m_Destination;
            }
            carFlags = carFlags & ~CarFlags.Emergency | CarFlags.StayOnRoad;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EvacuationRequestData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_EvacuationRequestData[request].m_Target;
              carFlags |= CarFlags.Emergency | CarFlags.StayOnRoad;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrisonerTransportRequestData.HasComponent(request))
              {
                // ISSUE: reference to a compiler-generated field
                entity1 = this.m_PrisonerTransportRequestData[request].m_Target;
                carFlags &= ~(CarFlags.Emergency | CarFlags.StayOnRoad);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(entity1))
          {
            serviceDispatches.RemoveAt(0);
            cargoTransport.m_RequestCount = math.max(0, cargoTransport.m_RequestCount - 1);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(request))
            {
              serviceDispatches.Clear();
              cargoTransport.m_RequestCount = 0;
              publicTransport.m_RequestCount = 0;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(route))
              {
                if (currentRoute.m_Route != route)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<CurrentRoute>(jobIndex, vehicleEntity, new CurrentRoute(route));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AppendToBuffer<RouteVehicle>(jobIndex, route, new RouteVehicle(vehicleEntity));
                  Game.Routes.Color componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_RouteColorData.TryGetComponent(route, out componentData))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Game.Routes.Color>(jobIndex, vehicleEntity, componentData);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, vehicleEntity);
                  }
                }
                cargoTransport.m_State |= CargoTransportFlags.EnRoute;
                publicTransport.m_State |= PublicTransportFlags.EnRoute;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(request, vehicleEntity, true));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(request, vehicleEntity, false, true));
            }
            cargoTransport.m_State &= ~CargoTransportFlags.Returning;
            publicTransport.m_State &= ~PublicTransportFlags.Returning;
            car.m_Flags = carFlags;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceRequestData.HasComponent(publicTransport.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity4 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity4, new HandleRequest(publicTransport.m_TargetRequest, Entity.Null, true));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceRequestData.HasComponent(cargoTransport.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity5 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity5, new HandleRequest(cargoTransport.m_TargetRequest, Entity.Null, true));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathElements.HasBuffer(request))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[request];
              if (pathElement1.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[vehicleEntity];
                PathUtils.TrimPath(pathElement2, ref pathOwner);
                // ISSUE: reference to a compiler-generated field
                float num = math.max(cargoTransport.m_PathElementTime, publicTransport.m_PathElementTime) * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes))
                {
                  cargoTransport.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  publicTransport.m_PathElementTime = cargoTransport.m_PathElementTime;
                  target.m_Target = entity1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
                  publicTransport.m_State &= ~PublicTransportFlags.Arriving;
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, entity1);
            return true;
          }
        }
        return false;
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Car car,
        ref PathOwner pathOwner,
        ref Target target)
      {
        serviceDispatches.Clear();
        cargoTransport.m_RequestCount = 0;
        cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.Refueling | CargoTransportFlags.AbandonRoute);
        cargoTransport.m_State |= CargoTransportFlags.Returning;
        publicTransport.m_RequestCount = 0;
        publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.Refueling | PublicTransportFlags.AbandonRoute);
        publicTransport.m_State |= PublicTransportFlags.Returning;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        car.m_Flags &= ~CarFlags.Emergency;
        VehicleUtils.SetTarget(ref pathOwner, ref target, ownerData.m_Owner);
      }

      private bool StartBoarding(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        PrefabRef prefabRef,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        bool isCargoVehicle)
      {
        if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) != (PublicTransportFlags) 0)
        {
          publicTransport.m_State |= PublicTransportFlags.Boarding;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          publicTransport.m_DepartureFrame = (publicTransport.m_State & PublicTransportFlags.Returning) == (PublicTransportFlags) 0 ? this.m_SimulationFrameIndex + 4096U : this.m_SimulationFrameIndex + 64U;
          publicTransport.m_MaxBoardingDistance = 0.0f;
          publicTransport.m_MinWaitingDistance = float.MaxValue;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          Connected connected = this.m_ConnectedData[target.m_Target];
          // ISSUE: reference to a compiler-generated field
          if (this.m_BoardingVehicleData.HasComponent(connected.m_Connected))
          {
            // ISSUE: reference to a compiler-generated method
            Entity transportStationFromStop = this.GetTransportStationFromStop(connected.m_Connected);
            Entity nextStorageCompany = Entity.Null;
            bool refuel = false;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportStationData.HasComponent(transportStationFromStop))
            {
              // ISSUE: reference to a compiler-generated field
              CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              refuel = (this.m_TransportStationData[transportStationFromStop].m_CarRefuelTypes & carData.m_EnergyType) != 0;
            }
            if (!refuel && ((cargoTransport.m_State & CargoTransportFlags.RequiresMaintenance) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.RequiresMaintenance) != (PublicTransportFlags) 0) || (cargoTransport.m_State & CargoTransportFlags.AbandonRoute) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.AbandonRoute) != (PublicTransportFlags) 0)
            {
              cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.AbandonRoute);
              publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.AbandonRoute);
              if (currentRoute.m_Route != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              }
            }
            else
            {
              cargoTransport.m_State &= ~CargoTransportFlags.RequiresMaintenance;
              publicTransport.m_State &= ~PublicTransportFlags.RequiresMaintenance;
              cargoTransport.m_State |= CargoTransportFlags.EnRoute;
              publicTransport.m_State |= PublicTransportFlags.EnRoute;
              if (isCargoVehicle)
              {
                // ISSUE: reference to a compiler-generated method
                nextStorageCompany = this.GetNextStorageCompany(currentRoute.m_Route, target.m_Target);
              }
            }
            cargoTransport.m_State |= CargoTransportFlags.RouteSource;
            publicTransport.m_State |= PublicTransportFlags.RouteSource;
            Entity storageCompanyFromStop = Entity.Null;
            if (isCargoVehicle)
            {
              // ISSUE: reference to a compiler-generated method
              storageCompanyFromStop = this.GetStorageCompanyFromStop(connected.m_Connected);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_BoardingData.BeginBoarding(vehicleEntity, currentRoute.m_Route, connected.m_Connected, target.m_Target, storageCompanyFromStop, nextStorageCompany, refuel);
            return true;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaypointData.HasComponent(target.m_Target))
        {
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
          return false;
        }
        cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.AbandonRoute);
        publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.AbandonRoute);
        if (currentRoute.m_Route != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        }
        return false;
      }

      private bool StopBoarding(
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        ref Odometer odometer,
        bool forcedStop)
      {
        bool flag = false;
        Connected componentData1;
        BoardingVehicle componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) && this.m_BoardingVehicleData.TryGetComponent(componentData1.m_Connected, out componentData2))
          flag = componentData2.m_Vehicle == vehicleEntity;
        if (!forcedStop)
        {
          publicTransport.m_MaxBoardingDistance = math.select(publicTransport.m_MinWaitingDistance + 1f, float.MaxValue, (double) publicTransport.m_MinWaitingDistance == 3.4028234663852886E+38 || (double) publicTransport.m_MinWaitingDistance == 0.0);
          publicTransport.m_MinWaitingDistance = float.MaxValue;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((flag || (publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) != (PublicTransportFlags) 0) && (this.m_SimulationFrameIndex < cargoTransport.m_DepartureFrame || this.m_SimulationFrameIndex < publicTransport.m_DepartureFrame || (double) publicTransport.m_MaxBoardingDistance != 3.4028234663852886E+38))
            return false;
          if (passengers.IsCreated)
          {
            for (int index = 0; index < passengers.Length; ++index)
            {
              CurrentVehicle componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentVehicleData.TryGetComponent(passengers[index].m_Passenger, out componentData3) && (componentData3.m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
                return false;
            }
          }
        }
        if ((cargoTransport.m_State & CargoTransportFlags.Refueling) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Refueling) != (PublicTransportFlags) 0)
          odometer.m_Distance = 0.0f;
        if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) == (PublicTransportFlags) 0 && flag)
        {
          Entity storageCompanyFromStop = Entity.Null;
          Entity nextStorageCompany = Entity.Null;
          if (!forcedStop && (cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            storageCompanyFromStop = this.GetStorageCompanyFromStop(componentData1.m_Connected);
            if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              nextStorageCompany = this.GetNextStorageCompany(currentRoute.m_Route, target.m_Target);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_BoardingData.EndBoarding(vehicleEntity, currentRoute.m_Route, componentData1.m_Connected, target.m_Target, storageCompanyFromStop, nextStorageCompany);
          return true;
        }
        cargoTransport.m_State &= ~(CargoTransportFlags.Boarding | CargoTransportFlags.Refueling);
        publicTransport.m_State &= ~(PublicTransportFlags.Boarding | PublicTransportFlags.Refueling);
        return true;
      }

      private Entity GetTransportStationFromStop(Entity stop)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; !this.m_TransportStationData.HasComponent(stop); stop = this.m_OwnerData[stop].m_Owner)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.HasComponent(stop))
            return Entity.Null;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(stop))
        {
          // ISSUE: reference to a compiler-generated field
          Entity owner = this.m_OwnerData[stop].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportStationData.HasComponent(owner))
            return owner;
        }
        return stop;
      }

      private Entity GetStorageCompanyFromStop(Entity stop)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; !this.m_StorageCompanyData.HasComponent(stop); stop = this.m_OwnerData[stop].m_Owner)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.HasComponent(stop))
            return Entity.Null;
        }
        return stop;
      }

      private Entity GetNextStorageCompany(Entity route, Entity currentWaypoint)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[route];
        // ISSUE: reference to a compiler-generated field
        int a = this.m_WaypointData[currentWaypoint].m_Index + 1;
        for (int index1 = 0; index1 < routeWaypoint.Length; ++index1)
        {
          int index2 = math.select(a, 0, a >= routeWaypoint.Length);
          Entity waypoint = routeWaypoint[index2].m_Waypoint;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedData.HasComponent(waypoint))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity storageCompanyFromStop = this.GetStorageCompanyFromStop(this.m_ConnectedData[waypoint].m_Connected);
            if (storageCompanyFromStop != Entity.Null)
              return storageCompanyFromStop;
          }
          a = index2 + 1;
        }
        return Entity.Null;
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> __Game_Simulation_EvacuationRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrisonerTransportRequest> __Game_Simulation_PrisonerTransportRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> __Game_Routes_BoardingVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> __Game_Buildings_TransportStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> __Game_Buildings_Prison_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> __Game_Buildings_EmergencyShelter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public BufferLookup<LoadingResources> __Game_Vehicles_LoadingResources_RW_BufferLookup;

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
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.CargoTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PublicTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup = state.GetComponentLookup<TransportVehicleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_EvacuationRequest_RO_ComponentLookup = state.GetComponentLookup<EvacuationRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PrisonerTransportRequest_RO_ComponentLookup = state.GetComponentLookup<PrisonerTransportRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BoardingVehicle_RO_ComponentLookup = state.GetComponentLookup<BoardingVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Prison_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Prison>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_EmergencyShelter_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.EmergencyShelter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LoadingResources_RW_BufferLookup = state.GetBufferLookup<LoadingResources>();
      }
    }
  }
}
