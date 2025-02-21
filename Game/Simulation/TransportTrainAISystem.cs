// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportTrainAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
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
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TransportTrainAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_VehicleQuery;
    private EntityQuery m_CarriagePrefabQuery;
    private EntityArchetype m_TransportVehicleRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedTrainRemoveTypes;
    private ComponentTypeSet m_MovingToParkedTrainAddTypes;
    private TransportTrainCarriageSelectData m_TransportTrainCarriageSelectData;
    private TransportBoardingHelpers.BoardingLookupData m_BoardingLookupData;
    private TransportTrainAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 3;

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
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportTrainCarriageSelectData = new TransportTrainCarriageSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingLookupData = new TransportBoardingHelpers.BoardingLookupData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadWrite<TrainCurrentLane>(),
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
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedTrainRemoveTypes = new ComponentTypeSet(new ComponentType[13]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<TrainNavigation>(),
        ComponentType.ReadWrite<TrainNavigationLane>(),
        ComponentType.ReadWrite<TrainCurrentLane>(),
        ComponentType.ReadWrite<TrainBogieFrame>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedTrainAddTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedTrain>(), ComponentType.ReadWrite<Stopped>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_CarriagePrefabQuery = this.GetEntityQuery(TransportTrainCarriageSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      TransportBoardingHelpers.BoardingData boardingData = new TransportBoardingHelpers.BoardingData(Allocator.TempJob);
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TransportTrainCarriageSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_CarriagePrefabQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingLookupData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new TransportTrainAISystem.TransportTrainTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CurrentRouteType = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle,
        m_CargoTransportType = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle,
        m_PublicTransportType = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferTypeHandle,
        m_NavigationLaneType = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TransportVehicleRequestData = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PublicTransportVehicleData = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_CargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_BoardingVehicleData = this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup,
        m_RouteColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_StorageCompanyData = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_TransportStationData = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup,
        m_TransportDepotData = this.__TypeHandle.__Game_Buildings_TransportDepot_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_EconomyResources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RW_ComponentLookup,
        m_CurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup,
        m_NavigationData = this.__TypeHandle.__Game_Vehicles_TrainNavigation_RW_ComponentLookup,
        m_BlockerData = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_LoadingResources = this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_TransportVehicleRequestArchetype = this.m_TransportVehicleRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedTrainRemoveTypes = this.m_MovingToParkedTrainRemoveTypes,
        m_MovingToParkedTrainAddTypes = this.m_MovingToParkedTrainAddTypes,
        m_TransportTrainCarriageSelectData = this.m_TransportTrainCarriageSelectData,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_BoardingData = boardingData.ToConcurrent()
      }.ScheduleParallel<TransportTrainAISystem.TransportTrainTickJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = boardingData.ScheduleBoarding((SystemBase) this, this.m_CityStatisticsSystem, this.m_BoardingLookupData, this.m_SimulationSystem.frameIndex, jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_TransportTrainCarriageSelectData.PostUpdate(jobHandle2);
      boardingData.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
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
    public TransportTrainAISystem()
    {
    }

    [BurstCompile]
    private struct TransportTrainTickJob : IJobChunk
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
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public BufferTypeHandle<TrainNavigationLane> m_NavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PublicTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_CargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanyData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> m_TransportStationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      [ReadOnly]
      public BufferLookup<Resources> m_EconomyResources;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Train> m_TrainData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainCurrentLane> m_CurrentLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrainNavigation> m_NavigationData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Blocker> m_BlockerData;
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
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedTrainRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedTrainAddTypes;
      [ReadOnly]
      public TransportTrainCarriageSelectData m_TransportTrainCarriageSelectData;
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
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentRoute> nativeArray4 = chunk.GetNativeArray<CurrentRoute>(ref this.m_CurrentRouteType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.CargoTransport> nativeArray5 = chunk.GetNativeArray<Game.Vehicles.CargoTransport>(ref this.m_CargoTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.PublicTransport> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.PublicTransport>(ref this.m_PublicTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray7 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray8 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Odometer> nativeArray9 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor1 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TrainNavigationLane> bufferAccessor2 = chunk.GetBufferAccessor<TrainNavigationLane>(ref this.m_NavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity vehicleEntity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathOwner pathOwner = nativeArray8[index];
          Target target = nativeArray7[index];
          Odometer odometer = nativeArray9[index];
          DynamicBuffer<LayoutElement> layout = bufferAccessor1[index];
          DynamicBuffer<TrainNavigationLane> navigationLanes = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          CurrentRoute currentRoute = new CurrentRoute();
          if (nativeArray4.Length != 0)
            currentRoute = nativeArray4[index];
          Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
          if (nativeArray5.Length != 0)
            cargoTransport = nativeArray5[index];
          Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
          if (nativeArray6.Length != 0)
            publicTransport = nativeArray6[index];
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, ref random, vehicleEntity, owner, prefabRef, currentRoute, layout, navigationLanes, serviceDispatches, isUnspawned, ref cargoTransport, ref publicTransport, ref pathOwner, ref target, ref odometer);
          nativeArray8[index] = pathOwner;
          nativeArray7[index] = target;
          nativeArray9[index] = odometer;
          if (nativeArray5.Length != 0)
            nativeArray5[index] = cargoTransport;
          if (nativeArray6.Length != 0)
            nativeArray6[index] = publicTransport;
        }
      }

      private void Tick(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        CurrentRoute currentRoute,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isUnspawned,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target,
        ref Odometer odometer)
      {
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          DynamicBuffer<LoadingResources> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (((cargoTransport.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0) && this.m_LoadingResources.TryGetBuffer(vehicleEntity, out bufferData))
          {
            if (bufferData.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.QuantityUpdated(jobIndex, vehicleEntity, layout);
            }
            // ISSUE: reference to a compiler-generated method
            if (this.CheckLoadingResources(jobIndex, ref random, vehicleEntity, true, layout, bufferData))
            {
              pathOwner.m_State |= PathFlags.Updated;
              return;
            }
          }
          cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
          publicTransport.m_State &= ~PublicTransportFlags.Arriving;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float length = VehicleUtils.CalculateLength(vehicleEntity, layout, ref this.m_PrefabRefData, ref this.m_PrefabTrainData);
          PathElement prevElement = new PathElement();
          if ((pathOwner.m_State & PathFlags.Append) != (PathFlags) 0)
          {
            if (navigationLanes.Length != 0)
            {
              TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
              prevElement = new PathElement(navigationLane.m_Lane, navigationLane.m_CurvePosition);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (VehicleUtils.IsReversedPath(pathElement, pathOwner, vehicleEntity, layout, this.m_CurveData, this.m_CurrentLaneData, this.m_TrainData, this.m_TransformData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.ReverseTrain(vehicleEntity, layout, ref this.m_TrainData, ref this.m_CurrentLaneData, ref this.m_NavigationData);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PathUtils.ExtendReverseLocations(prevElement, pathElement, pathOwner, length, this.m_CurveData, this.m_LaneData, this.m_EdgeLaneData, this.m_OwnerData, this.m_EdgeData, this.m_ConnectedEdges, this.m_SubLanes);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_WaypointData.HasComponent(target.m_Target) || this.m_ConnectedData.HasComponent(target.m_Target) && this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected))
          {
            float distance = length * 0.5f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PathUtils.ExtendPath(pathElement, pathOwner, ref distance, ref this.m_CurveData, ref this.m_LaneData, ref this.m_EdgeLaneData, ref this.m_OwnerData, ref this.m_EdgeData, ref this.m_ConnectedEdges, ref this.m_SubLanes);
          }
          // ISSUE: reference to a compiler-generated method
          this.UpdatePantograph(layout);
        }
        Entity entity1 = vehicleEntity;
        if (layout.Length != 0)
          entity1 = layout[0].m_Vehicle;
        // ISSUE: reference to a compiler-generated field
        Train train = this.m_TrainData[entity1];
        // ISSUE: reference to a compiler-generated field
        TrainCurrentLane currentLane = this.m_CurrentLaneData[entity1];
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.CheckUnspawned(jobIndex, vehicleEntity, currentLane, isUnspawned, this.m_CommandBuffer);
        int num1 = (cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0 ? 0 : ((publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0 ? 1 : 0);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PublicTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          PublicTransportVehicleData transportVehicleData = this.m_PublicTransportVehicleData[prefabRef.m_Prefab];
          if ((double) odometer.m_Distance >= (double) transportVehicleData.m_MaintenanceRange && (double) transportVehicleData.m_MaintenanceRange > 0.10000000149011612 && (publicTransport.m_State & PublicTransportFlags.Refueling) == (PublicTransportFlags) 0)
            publicTransport.m_State |= PublicTransportFlags.RequiresMaintenance;
        }
        bool isCargoVehicle = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[prefabRef.m_Prefab];
          if ((double) odometer.m_Distance >= (double) transportVehicleData.m_MaintenanceRange && (double) transportVehicleData.m_MaintenanceRange > 0.10000000149011612 && (cargoTransport.m_State & CargoTransportFlags.Refueling) == (CargoTransportFlags) 0)
            cargoTransport.m_State |= CargoTransportFlags.RequiresMaintenance;
          isCargoVehicle = true;
        }
        if (num1 != 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, ref cargoTransport, ref publicTransport);
          if (serviceDispatches.Length == 0 && (cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.DummyTraffic | CargoTransportFlags.Disabled)) == (CargoTransportFlags) 0 && (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.DummyTraffic | PublicTransportFlags.Disabled)) == (PublicTransportFlags) 0)
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
        bool flag = false;
        if (VehicleUtils.IsStuck(pathOwner))
        {
          // ISSUE: reference to a compiler-generated field
          Blocker blocker = this.m_BlockerData[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          int num2 = this.m_ParkedTrainData.HasComponent(blocker.m_Blocker) ? 1 : 0;
          if (num2 != 0)
          {
            Entity entity2 = blocker.m_Blocker;
            Controller componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControllerData.TryGetComponent(entity2, out componentData))
              entity2 = componentData.m_Controller;
            DynamicBuffer<LayoutElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_LayoutElements.TryGetBuffer(entity2, out bufferData);
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity2, bufferData);
          }
          if (num2 != 0 || blocker.m_Blocker == Entity.Null)
          {
            pathOwner.m_State &= ~PathFlags.Stuck;
            // ISSUE: reference to a compiler-generated field
            this.m_BlockerData[vehicleEntity] = new Blocker();
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            flag = true;
            // ISSUE: reference to a compiler-generated method
            this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, true);
          }
          if (VehicleUtils.IsStuck(pathOwner) || (cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
            // ISSUE: reference to a compiler-generated field
            this.m_TrainData[entity1] = train;
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentLaneData[entity1] = currentLane;
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, false))
              {
                flag = true;
                // ISSUE: reference to a compiler-generated method
                if (!this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, layout, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref currentLane, ref pathOwner, ref target))
                {
                  // ISSUE: reference to a compiler-generated method
                  if (!this.TryParkTrain(jobIndex, vehicleEntity, owner, layout, navigationLanes, ref train, ref cargoTransport, ref publicTransport, ref currentLane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_TrainData[entity1] = train;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CurrentLaneData[entity1] = currentLane;
                  return;
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if ((this.CountPassengers(vehicleEntity, layout) <= 0 || !this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, isCargoVehicle)) && !this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, layout, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref currentLane, ref pathOwner, ref target))
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.TryParkTrain(jobIndex, vehicleEntity, owner, layout, navigationLanes, ref train, ref cargoTransport, ref publicTransport, ref currentLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicleEntity, layout);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_TrainData[entity1] = train;
                // ISSUE: reference to a compiler-generated field
                this.m_CurrentLaneData[entity1] = currentLane;
                return;
              }
            }
          }
          else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, false))
            {
              flag = true;
              if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref pathOwner, ref target);
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
            if (!this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) || !this.m_WaypointData.HasComponent(target.m_Target))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref pathOwner, ref target);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, isCargoVehicle))
              {
                if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref pathOwner, ref target);
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
        else if (VehicleUtils.ReturnEndReached(currentLane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TrainData[entity1] = train;
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentLaneData[entity1] = currentLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.ReverseTrain(vehicleEntity, layout, ref this.m_TrainData, ref this.m_CurrentLaneData, ref this.m_NavigationData);
          // ISSUE: reference to a compiler-generated method
          this.UpdatePantograph(layout);
          entity1 = vehicleEntity;
          if (layout.Length != 0)
            entity1 = layout[0].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          train = this.m_TrainData[entity1];
          // ISSUE: reference to a compiler-generated field
          currentLane = this.m_CurrentLaneData[entity1];
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          flag = true;
          // ISSUE: reference to a compiler-generated method
          this.StopBoarding(jobIndex, ref random, vehicleEntity, currentRoute, layout, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, true);
        }
        train.m_Flags &= ~(Game.Vehicles.TrainFlags.BoardingLeft | Game.Vehicles.TrainFlags.BoardingRight);
        publicTransport.m_State &= ~(PublicTransportFlags.StopLeft | PublicTransportFlags.StopRight);
        Entity skipWaypoint = Entity.Null;
        if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          if (!flag)
          {
            // ISSUE: reference to a compiler-generated field
            Train controllerTrain = this.m_TrainData[vehicleEntity];
            // ISSUE: reference to a compiler-generated method
            this.UpdateStop(entity1, controllerTrain, true, ref train, ref publicTransport, ref target);
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.CountPassengers(vehicleEntity, layout) == 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, layout, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref train, ref currentLane, ref pathOwner, ref target);
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Arriving) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Arriving) != (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          Train controllerTrain = this.m_TrainData[vehicleEntity];
          // ISSUE: reference to a compiler-generated method
          this.UpdateStop(entity1, controllerTrain, false, ref train, ref publicTransport, ref target);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckNavigationLanes(currentRoute, navigationLanes, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target, out skipWaypoint);
        }
        if ((((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 ? 0 : ((publicTransport.m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0 ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        {
          if (VehicleUtils.RequireNewPath(pathOwner))
          {
            // ISSUE: reference to a compiler-generated method
            this.FindNewPath(vehicleEntity, prefabRef, skipWaypoint, ref currentLane, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
          }
          else if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) == (PathFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingSpace(navigationLanes, ref train, ref pathOwner);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_TrainData[entity1] = train;
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentLaneData[entity1] = currentLane;
      }

      private void CheckParkingSpace(
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        ref Train train,
        ref PathOwner pathOwner)
      {
        if (navigationLanes.Length == 0)
          return;
        TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        Game.Objects.SpawnLocation componentData;
        // ISSUE: reference to a compiler-generated field
        if ((navigationLane.m_Flags & TrainLaneFlags.ParkingSpace) == (TrainLaneFlags) 0 || !this.m_SpawnLocationData.TryGetComponent(navigationLane.m_Lane, out componentData))
          return;
        if ((componentData.m_Flags & SpawnLocationFlags.ParkedVehicle) != (SpawnLocationFlags) 0)
        {
          if ((train.m_Flags & Game.Vehicles.TrainFlags.IgnoreParkedVehicle) != (Game.Vehicles.TrainFlags) 0)
            return;
          train.m_Flags |= Game.Vehicles.TrainFlags.IgnoreParkedVehicle;
          pathOwner.m_State |= PathFlags.Obsolete;
        }
        else
          train.m_Flags &= ~Game.Vehicles.TrainFlags.IgnoreParkedVehicle;
      }

      private bool TryParkTrain(
        int jobIndex,
        Entity entity,
        Owner owner,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        ref Train train,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref TrainCurrentLane currentLane)
      {
        if (navigationLanes.Length == 0)
          return false;
        TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        if ((navigationLane.m_Flags & TrainLaneFlags.ParkingSpace) == (TrainLaneFlags) 0)
          return false;
        train.m_Flags &= ~(Game.Vehicles.TrainFlags.BoardingLeft | Game.Vehicles.TrainFlags.BoardingRight | Game.Vehicles.TrainFlags.Pantograph | Game.Vehicles.TrainFlags.IgnoreParkedVehicle);
        cargoTransport.m_State &= CargoTransportFlags.RequiresMaintenance;
        publicTransport.m_State &= PublicTransportFlags.RequiresMaintenance;
        Game.Buildings.TransportDepot componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportDepotData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & TransportDepotFlags.HasAvailableVehicles) == (TransportDepotFlags) 0)
        {
          cargoTransport.m_State |= CargoTransportFlags.Disabled;
          publicTransport.m_State |= PublicTransportFlags.Disabled;
        }
        for (int index = 0; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent(jobIndex, vehicle, in this.m_MovingToParkedTrainRemoveTypes);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(jobIndex, vehicle, in this.m_MovingToParkedTrainAddTypes);
          if (index == 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ParkedTrain>(jobIndex, vehicle, new ParkedTrain(navigationLane.m_Lane, currentLane));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            TrainCurrentLane currentLane1 = this.m_CurrentLaneData[vehicle];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ParkedTrain>(jobIndex, vehicle, new ParkedTrain(navigationLane.m_Lane, currentLane1));
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocationData.HasComponent(navigationLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, navigationLane.m_Lane);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(Entity.Null, entity));
        }
        return true;
      }

      private void UpdatePantograph(DynamicBuffer<LayoutElement> layout)
      {
        bool flag = false;
        for (int index = 0; index < layout.Length; ++index)
        {
          Entity vehicle = layout[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          Train train = this.m_TrainData[vehicle];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          TrainData trainData = this.m_PrefabTrainData[this.m_PrefabRefData[vehicle].m_Prefab];
          if (flag || (trainData.m_TrainFlags & Game.Prefabs.TrainFlags.Pantograph) == (Game.Prefabs.TrainFlags) 0)
          {
            train.m_Flags &= ~Game.Vehicles.TrainFlags.Pantograph;
          }
          else
          {
            train.m_Flags |= Game.Vehicles.TrainFlags.Pantograph;
            flag = (trainData.m_TrainFlags & Game.Prefabs.TrainFlags.MultiUnit) != 0;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_TrainData[vehicle] = train;
        }
      }

      private void UpdateStop(
        Entity vehicleEntity,
        Train controllerTrain,
        bool isBoarding,
        ref Train train,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[vehicleEntity];
        Connected componentData1;
        Transform componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) || !this.m_TransformData.TryGetComponent(componentData1.m_Connected, out componentData2))
          return;
        bool flag = (double) math.dot(math.mul(transform.m_Rotation, math.right()), componentData2.m_Position - transform.m_Position) < 0.0;
        if (isBoarding)
        {
          if (flag)
            train.m_Flags |= Game.Vehicles.TrainFlags.BoardingLeft;
          else
            train.m_Flags |= Game.Vehicles.TrainFlags.BoardingRight;
        }
        if (flag ^ ((controllerTrain.m_Flags ^ train.m_Flags) & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0)
          publicTransport.m_State |= PublicTransportFlags.StopLeft;
        else
          publicTransport.m_State |= PublicTransportFlags.StopRight;
      }

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        Entity skipWaypoint,
        ref TrainCurrentLane currentLane,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        TrainData trainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) trainData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Track,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Track,
          m_TrackTypes = trainData.m_TrackType
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Track,
          m_TrackTypes = trainData.m_TrackType,
          m_Entity = target.m_Target
        };
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
          destination.m_RandomCost = 30f;
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckNavigationLanes(
        CurrentRoute currentRoute,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref TrainCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        out Entity skipWaypoint)
      {
        skipWaypoint = Entity.Null;
        if (navigationLanes.Length == 0 || navigationLanes.Length >= 10)
          return;
        TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        if ((navigationLane.m_Flags & TrainLaneFlags.EndOfPath) == (TrainLaneFlags) 0)
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
          navigationLane.m_Flags &= ~TrainLaneFlags.EndOfPath;
          navigationLanes[navigationLanes.Length - 1] = navigationLane;
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
        }
        else
        {
          cargoTransport.m_State |= CargoTransportFlags.Arriving;
          publicTransport.m_State |= PublicTransportFlags.Arriving;
        }
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
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport)
      {
        if (serviceDispatches.Length > 1)
          serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
        cargoTransport.m_RequestCount = math.min(1, cargoTransport.m_RequestCount);
        publicTransport.m_RequestCount = math.min(1, publicTransport.m_RequestCount);
        int index1 = cargoTransport.m_RequestCount + publicTransport.m_RequestCount;
        if (serviceDispatches.Length <= index1)
          return;
        float num = -1f;
        Entity request1 = Entity.Null;
        for (int index2 = index1; index2 < serviceDispatches.Length; ++index2)
        {
          Entity request2 = serviceDispatches[index2].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request2))
          {
            // ISSUE: reference to a compiler-generated field
            TransportVehicleRequest transportVehicleRequest = this.m_TransportVehicleRequestData[request2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(transportVehicleRequest.m_Route) && (double) transportVehicleRequest.m_Priority > (double) num)
            {
              num = transportVehicleRequest.m_Priority;
              request1 = request2;
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
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportVehicleRequestData.HasComponent(publicTransport.m_TargetRequest) || this.m_TransportVehicleRequestData.HasComponent(cargoTransport.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(256U, 16U) - 1) != 3)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TransportVehicleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TransportVehicleRequest>(jobIndex, entity1, new TransportVehicleRequest(entity, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(8U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<TrainNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Train train,
        ref TrainCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
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
          Entity destination = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            route = this.m_TransportVehicleRequestData[request].m_Route;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathInformationData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated field
              destination = this.m_PathInformationData[request].m_Destination;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(destination))
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
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateBatches(jobIndex, vehicleEntity, layout);
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
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(request, vehicleEntity, true));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(request, vehicleEntity, false, true));
            }
            cargoTransport.m_State &= ~CargoTransportFlags.Returning;
            publicTransport.m_State &= ~PublicTransportFlags.Returning;
            train.m_Flags &= ~Game.Vehicles.TrainFlags.IgnoreParkedVehicle;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(publicTransport.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(publicTransport.m_TargetRequest, Entity.Null, true));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(cargoTransport.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(cargoTransport.m_TargetRequest, Entity.Null, true));
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
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1))
                {
                  cargoTransport.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  publicTransport.m_PathElementTime = cargoTransport.m_PathElementTime;
                  target.m_Target = destination;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
                  publicTransport.m_State &= ~PublicTransportFlags.Arriving;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float length = VehicleUtils.CalculateLength(vehicleEntity, layout, ref this.m_PrefabRefData, ref this.m_PrefabTrainData);
                  PathElement prevElement = new PathElement();
                  if (navigationLanes.Length != 0)
                  {
                    TrainNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
                    prevElement = new PathElement(navigationLane.m_Lane, navigationLane.m_CurvePosition);
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  PathUtils.ExtendReverseLocations(prevElement, pathElement2, pathOwner, length, this.m_CurveData, this.m_LaneData, this.m_EdgeLaneData, this.m_OwnerData, this.m_EdgeData, this.m_ConnectedEdges, this.m_SubLanes);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_WaypointData.HasComponent(target.m_Target) || this.m_ConnectedData.HasComponent(target.m_Target) && this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected))
                  {
                    float distance = length * 0.5f;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    PathUtils.ExtendPath(pathElement2, pathOwner, ref distance, ref this.m_CurveData, ref this.m_LaneData, ref this.m_EdgeLaneData, ref this.m_OwnerData, ref this.m_EdgeData, ref this.m_ConnectedEdges, ref this.m_SubLanes);
                  }
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, destination);
            return true;
          }
        }
        return false;
      }

      private void UpdateBatches(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout)
      {
        if (layout.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, layout.Reinterpret<Entity>().AsNativeArray());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, vehicleEntity);
        }
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Train train,
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
        train.m_Flags &= ~Game.Vehicles.TrainFlags.IgnoreParkedVehicle;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
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
              TrainData trainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              refuel = (this.m_TransportStationData[transportStationFromStop].m_TrainRefuelTypes & trainData.m_EnergyType) != 0;
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

      private bool TryChangeCarriagePrefab(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        bool dummyTraffic,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EconomyResources.HasBuffer(vehicleEntity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Resources> economyResource = this.m_EconomyResources[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          if (economyResource.Length == 0 && this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
          {
            while (loadingResources.Length > 0)
            {
              LoadingResources loadingResource = loadingResources[0];
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_TransportTrainCarriageSelectData.SelectCarriagePrefab(ref random, loadingResource.m_Resource, loadingResource.m_Amount);
              if (entity != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[entity];
                int num = math.min(loadingResource.m_Amount, transportVehicleData.m_CargoCapacity);
                loadingResource.m_Amount -= transportVehicleData.m_CargoCapacity;
                if (loadingResource.m_Amount <= 0)
                  loadingResources.RemoveAt(0);
                else
                  loadingResources[0] = loadingResource;
                if (dummyTraffic)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetBuffer<Resources>(jobIndex, vehicleEntity).Add(new Resources()
                  {
                    m_Resource = loadingResource.m_Resource,
                    m_Amount = num
                  });
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, vehicleEntity, new PrefabRef(entity));
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(jobIndex, vehicleEntity, new Updated());
                return true;
              }
              loadingResources.RemoveAt(0);
            }
          }
        }
        return false;
      }

      private bool CheckLoadingResources(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        bool dummyTraffic,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        bool flag = false;
        if (loadingResources.Length != 0)
        {
          if (layout.Length != 0)
          {
            for (int index = 0; index < layout.Length && loadingResources.Length != 0; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              flag |= this.TryChangeCarriagePrefab(jobIndex, ref random, layout[index].m_Vehicle, dummyTraffic, loadingResources);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            flag |= this.TryChangeCarriagePrefab(jobIndex, ref random, vehicleEntity, dummyTraffic, loadingResources);
          }
          loadingResources.Clear();
        }
        return flag;
      }

      private bool StopBoarding(
        int jobIndex,
        ref Random random,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        ref Odometer odometer,
        bool isCargoVehicle,
        bool forcedStop)
      {
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LoadingResources.HasBuffer(vehicleEntity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LoadingResources> loadingResource = this.m_LoadingResources[vehicleEntity];
          if (forcedStop)
          {
            loadingResource.Clear();
          }
          else
          {
            bool dummyTraffic = (cargoTransport.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.DummyTraffic) > (PublicTransportFlags) 0;
            // ISSUE: reference to a compiler-generated method
            flag1 |= this.CheckLoadingResources(jobIndex, ref random, vehicleEntity, dummyTraffic, layout, loadingResource);
          }
        }
        if (flag1)
          return false;
        bool flag2 = false;
        Connected componentData1;
        BoardingVehicle componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) && this.m_BoardingVehicleData.TryGetComponent(componentData1.m_Connected, out componentData2))
          flag2 = componentData2.m_Vehicle == vehicleEntity;
        if (!forcedStop)
        {
          publicTransport.m_MaxBoardingDistance = math.select(publicTransport.m_MinWaitingDistance + 1f, float.MaxValue, (double) publicTransport.m_MinWaitingDistance == 3.4028234663852886E+38 || (double) publicTransport.m_MinWaitingDistance == 0.0);
          publicTransport.m_MinWaitingDistance = float.MaxValue;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (flag2 && (this.m_SimulationFrameIndex < cargoTransport.m_DepartureFrame || this.m_SimulationFrameIndex < publicTransport.m_DepartureFrame || (double) publicTransport.m_MaxBoardingDistance != 3.4028234663852886E+38))
            return false;
          if (layout.Length != 0)
          {
            for (int index = 0; index < layout.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ArePassengersReady(layout[index].m_Vehicle))
                return false;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.ArePassengersReady(vehicleEntity))
              return false;
          }
        }
        if ((cargoTransport.m_State & CargoTransportFlags.Refueling) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Refueling) != (PublicTransportFlags) 0)
          odometer.m_Distance = 0.0f;
        if (isCargoVehicle)
        {
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(jobIndex, vehicleEntity, layout);
        }
        if (flag2)
        {
          Entity storageCompanyFromStop = Entity.Null;
          Entity nextStorageCompany = Entity.Null;
          if (isCargoVehicle && !forcedStop)
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

      private void QuantityUpdated(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout)
      {
        if (layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, layout[index].m_Vehicle, new Updated());
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, vehicleEntity, new Updated());
        }
      }

      private int CountPassengers(Entity vehicleEntity, DynamicBuffer<LayoutElement> layout)
      {
        int num = 0;
        if (layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Passengers.HasBuffer(vehicle))
            {
              // ISSUE: reference to a compiler-generated field
              num += this.m_Passengers[vehicle].Length;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Passengers.HasBuffer(vehicleEntity))
          {
            // ISSUE: reference to a compiler-generated field
            num += this.m_Passengers[vehicleEntity].Length;
          }
        }
        return num;
      }

      private bool ArePassengersReady(Entity vehicleEntity)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Passengers.HasBuffer(vehicleEntity))
          return true;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Passenger> passenger1 = this.m_Passengers[vehicleEntity];
        for (int index = 0; index < passenger1.Length; ++index)
        {
          Entity passenger2 = passenger1[index].m_Passenger;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentVehicleData.HasComponent(passenger2) && (this.m_CurrentVehicleData[passenger2].m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
            return false;
        }
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RW_BufferTypeHandle;
      public BufferTypeHandle<TrainNavigationLane> __Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> __Game_Routes_BoardingVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> __Game_Buildings_TransportStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public ComponentLookup<Train> __Game_Vehicles_Train_RW_ComponentLookup;
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RW_ComponentLookup;
      public ComponentLookup<TrainNavigation> __Game_Vehicles_TrainNavigation_RW_ComponentLookup;
      public ComponentLookup<Blocker> __Game_Vehicles_Blocker_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public BufferLookup<LoadingResources> __Game_Vehicles_LoadingResources_RW_BufferLookup;
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RW_BufferLookup;

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
        this.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.CargoTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PublicTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RW_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<TrainNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup = state.GetComponentLookup<TransportVehicleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BoardingVehicle_RO_ComponentLookup = state.GetComponentLookup<BoardingVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RW_ComponentLookup = state.GetComponentLookup<Train>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RW_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigation_RW_ComponentLookup = state.GetComponentLookup<TrainNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentLookup = state.GetComponentLookup<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LoadingResources_RW_BufferLookup = state.GetBufferLookup<LoadingResources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RW_BufferLookup = state.GetBufferLookup<LayoutElement>();
      }
    }
  }
}
