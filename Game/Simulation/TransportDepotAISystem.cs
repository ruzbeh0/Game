// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportDepotAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Events;
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
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TransportDepotAISystem : GameSystemBase
  {
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_VehiclePrefabQuery;
    private EntityQuery m_EventPrefabQuery;
    private EntityArchetype m_TransportVehicleRequestArchetype;
    private EntityArchetype m_TaxiRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_ParkedToMovingRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingTaxiAddTypes;
    private ComponentTypeSet m_ParkedToMovingBusAddTypes;
    private ComponentTypeSet m_ParkedToMovingTrainAddTypes;
    private ComponentTypeSet m_ParkedToMovingTrainControllerAddTypes;
    private EndFrameBarrier m_EndFrameBarrier;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private TransportVehicleSelectData m_TransportVehicleSelectData;
    private TransportDepotAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 32;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData = new TransportVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.TransportDepot>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclePrefabQuery = this.GetEntityQuery(TransportVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_EventPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<VehicleLaunchData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TransportVehicleRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_TaxiRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TaxiRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingRemoveTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<ParkedTrain>(), ComponentType.ReadWrite<Stopped>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingTaxiAddTypes = new ComponentTypeSet(new ComponentType[13]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Game.Common.Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingBusAddTypes = new ComponentTypeSet(new ComponentType[14]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Game.Common.Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingTrainAddTypes = new ComponentTypeSet(new ComponentType[7]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<TrainNavigation>(),
        ComponentType.ReadWrite<TrainCurrentLane>(),
        ComponentType.ReadWrite<TrainBogieFrame>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingTrainControllerAddTypes = new ComponentTypeSet(new ComponentType[14]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<TrainNavigation>(),
        ComponentType.ReadWrite<TrainNavigationLane>(),
        ComponentType.ReadWrite<TrainCurrentLane>(),
        ComponentType.ReadWrite<TrainBogieFrame>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Game.Common.Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
      Assert.IsTrue(true);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_VehiclePrefabQuery, Allocator.TempJob, out jobHandle1);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EventPrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      NativeQueue<TransportDepotAISystem.DepotAction> nativeQueue = new NativeQueue<TransportDepotAISystem.DepotAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TaxiData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Odometer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Produced_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleModel_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportDepot_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleLaunchData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_SpectatorSite_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TransportDepotAISystem.TransportDepotTickJob jobData1 = new TransportDepotAISystem.TransportDepotTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_SpectatorSiteType = this.__TypeHandle.__Game_Events_SpectatorSite_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_EventType = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle,
        m_VehicleLaunchType = this.__TypeHandle.__Game_Prefabs_VehicleLaunchData_RO_ComponentTypeHandle,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle,
        m_TransportDepotType = this.__TypeHandle.__Game_Buildings_TransportDepot_RW_ComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_TransportVehicleRequestData = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup,
        m_TaxiRequestData = this.__TypeHandle.__Game_Simulation_TaxiRequest_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_VehicleModelData = this.__TypeHandle.__Game_Routes_VehicleModel_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_RouteColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_ProducedData = this.__TypeHandle.__Game_Vehicles_Produced_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_CargoTransportData = this.__TypeHandle.__Game_Vehicles_CargoTransport_RO_ComponentLookup,
        m_TaxiData = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup,
        m_OdometerData = this.__TypeHandle.__Game_Vehicles_Odometer_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
        m_PrefabTransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_PrefabTaxiData = this.__TypeHandle.__Game_Prefabs_TaxiData_RO_ComponentLookup,
        m_PrefabPublicTransportVehicleData = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_PrefabCargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_ActivityLocationElements = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_TransportVehicleRequestArchetype = this.m_TransportVehicleRequestArchetype,
        m_TaxiRequestArchetype = this.m_TaxiRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_ParkedToMovingRemoveTypes = this.m_ParkedToMovingRemoveTypes,
        m_ParkedToMovingTaxiAddTypes = this.m_ParkedToMovingTaxiAddTypes,
        m_ParkedToMovingBusAddTypes = this.m_ParkedToMovingBusAddTypes,
        m_ParkedToMovingTrainAddTypes = this.m_ParkedToMovingTrainAddTypes,
        m_ParkedToMovingTrainControllerAddTypes = this.m_ParkedToMovingTrainControllerAddTypes,
        m_TransportVehicleSelectData = this.m_TransportVehicleSelectData,
        m_EventPrefabChunks = archetypeChunkListAsync,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      TransportDepotAISystem.TransportDepotActionJob jobData2 = new TransportDepotAISystem.TransportDepotActionJob()
      {
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentLookup,
        m_CargoTransportData = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentLookup,
        m_TaxiData = this.__TypeHandle.__Game_Vehicles_Taxi_RW_ComponentLookup,
        m_OdometerData = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData1.ScheduleParallel<TransportDepotAISystem.TransportDepotTickJob>(this.m_BuildingQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1, outJobHandle));
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.Schedule<TransportDepotAISystem.TransportDepotActionJob>(dependsOn);
      archetypeChunkListAsync.Dispose(jobHandle2);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PostUpdate(jobHandle2);
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
    public TransportDepotAISystem()
    {
    }

    private enum DepotActionType : byte
    {
      SetDisabled,
      ClearOdometer,
    }

    private struct DepotAction
    {
      public Entity m_Entity;
      public TransportDepotAISystem.DepotActionType m_Type;
      public bool m_Disabled;

      public static TransportDepotAISystem.DepotAction SetDisabled(Entity vehicle, bool disabled)
      {
        // ISSUE: object of a compiler-generated type is created
        return new TransportDepotAISystem.DepotAction()
        {
          m_Entity = vehicle,
          m_Type = TransportDepotAISystem.DepotActionType.SetDisabled,
          m_Disabled = disabled
        };
      }

      public static TransportDepotAISystem.DepotAction ClearOdometer(Entity vehicle)
      {
        // ISSUE: object of a compiler-generated type is created
        return new TransportDepotAISystem.DepotAction()
        {
          m_Entity = vehicle,
          m_Type = TransportDepotAISystem.DepotActionType.ClearOdometer
        };
      }
    }

    [BurstCompile]
    private struct TransportDepotTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<SpectatorSite> m_SpectatorSiteType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<EventData> m_EventType;
      [ReadOnly]
      public ComponentTypeHandle<VehicleLaunchData> m_VehicleLaunchType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceRequestType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> m_TaxiRequestData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<VehicleModel> m_VehicleModelData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<Produced> m_ProducedData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> m_TaxiData;
      [ReadOnly]
      public ComponentLookup<Odometer> m_OdometerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public ComponentLookup<TaxiData> m_PrefabTaxiData;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PrefabPublicTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_PrefabCargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocationElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_TransportVehicleRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_TaxiRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingTaxiAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingBusAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingTrainAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingTrainControllerAddTypes;
      [ReadOnly]
      public TransportVehicleSelectData m_TransportVehicleSelectData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EventPrefabChunks;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<TransportDepotAISystem.DepotAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.TransportDepot> nativeArray4 = chunk.GetNativeArray<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor3 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor4 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceRequestType);
        // ISSUE: reference to a compiler-generated field
        bool isOutsideConnection = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
        // ISSUE: reference to a compiler-generated field
        bool isSpectatorSite = chunk.Has<SpectatorSite>(ref this.m_SpectatorSiteType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Game.Objects.Transform transform = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          Game.Buildings.TransportDepot transportDepot = nativeArray4[index];
          DynamicBuffer<OwnedVehicle> vehicles = bufferAccessor3[index];
          DynamicBuffer<ServiceDispatch> dispatches = bufferAccessor4[index];
          TransportDepotData data = new TransportDepotData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTransportDepotData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            data = this.m_PrefabTransportDepotData[prefabRef.m_Prefab];
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<TransportDepotData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabTransportDepotData);
          }
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          float immediateEfficiency = BuildingUtils.GetImmediateEfficiency(bufferAccessor1, index);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, ref random, entity, transform, prefabRef, ref transportDepot, data, vehicles, dispatches, efficiency, immediateEfficiency, isOutsideConnection, isSpectatorSite);
          nativeArray4[index] = transportDepot;
        }
      }

      private void Tick(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Game.Objects.Transform transform,
        PrefabRef prefabRef,
        ref Game.Buildings.TransportDepot transportDepot,
        TransportDepotData prefabTransportDepotData,
        DynamicBuffer<OwnedVehicle> vehicles,
        DynamicBuffer<ServiceDispatch> dispatches,
        float efficiency,
        float immediateEfficiency,
        bool isOutsideConnection,
        bool isSpectatorSite)
      {
        int vehicleCapacity1 = BuildingUtils.GetVehicleCapacity(efficiency, prefabTransportDepotData.m_VehicleCapacity);
        int vehicleCapacity2 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabTransportDepotData.m_VehicleCapacity);
        int num1 = vehicleCapacity1;
        int num2 = 0;
        Entity entity1 = Entity.Null;
        StackList<Entity> parkedVehicles = (StackList<Entity>) stackalloc Entity[vehicles.Length];
        for (int index = 0; index < vehicles.Length; ++index)
        {
          Entity vehicle = vehicles[index].m_Vehicle;
          Game.Vehicles.PublicTransport componentData1;
          bool flag;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PublicTransportData.TryGetComponent(vehicle, out componentData1))
          {
            flag = (componentData1.m_State & PublicTransportFlags.Disabled) > (PublicTransportFlags) 0;
          }
          else
          {
            Game.Vehicles.CargoTransport componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CargoTransportData.TryGetComponent(vehicle, out componentData2))
            {
              flag = (componentData2.m_State & CargoTransportFlags.Disabled) > (CargoTransportFlags) 0;
            }
            else
            {
              Game.Vehicles.Taxi componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TaxiData.TryGetComponent(vehicle, out componentData3))
                flag = (componentData3.m_State & TaxiFlags.Disabled) > (TaxiFlags) 0;
              else
                continue;
            }
          }
          ParkedCar componentData4;
          // ISSUE: reference to a compiler-generated field
          bool component1 = this.m_ParkedCarData.TryGetComponent(vehicle, out componentData4);
          ParkedTrain componentData5;
          // ISSUE: reference to a compiler-generated field
          bool component2 = this.m_ParkedTrainData.TryGetComponent(vehicle, out componentData5);
          if (component1 | component2)
          {
            Odometer componentData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OdometerData.TryGetComponent(vehicle, out componentData6) && (double) componentData6.m_Distance != 0.0)
            {
              PublicTransportVehicleData componentData7;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabPublicTransportVehicleData.TryGetComponent(prefabRef.m_Prefab, out componentData7))
              {
                if ((double) componentData7.m_MaintenanceRange > 0.10000000149011612)
                  transportDepot.m_MaintenanceRequirement += math.saturate(componentData6.m_Distance / componentData7.m_MaintenanceRange);
              }
              else
              {
                CargoTransportVehicleData componentData8;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabCargoTransportVehicleData.TryGetComponent(prefabRef.m_Prefab, out componentData8))
                {
                  if ((double) componentData8.m_MaintenanceRange > 0.10000000149011612)
                    transportDepot.m_MaintenanceRequirement += math.saturate(componentData6.m_Distance / componentData8.m_MaintenanceRange);
                }
                else
                {
                  TaxiData componentData9;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabTaxiData.TryGetComponent(prefabRef.m_Prefab, out componentData9) && (double) componentData9.m_MaintenanceRange > 0.10000000149011612)
                    transportDepot.m_MaintenanceRequirement += math.saturate(componentData6.m_Distance / componentData9.m_MaintenanceRange);
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_ActionQueue.Enqueue(TransportDepotAISystem.DepotAction.ClearOdometer(vehicle));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (component1 && !this.m_EntityLookup.Exists(componentData4.m_Lane) || component2 && !this.m_EntityLookup.Exists(componentData5.m_ParkingLocation))
            {
              DynamicBuffer<LayoutElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              this.m_LayoutElements.TryGetBuffer(vehicle, out bufferData);
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, vehicle, bufferData);
            }
            else
              parkedVehicles.AddNoResize(vehicle);
          }
          else
          {
            --num1;
            ++num2;
            bool disabled = --vehicleCapacity2 < 0;
            if (flag != disabled)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_ActionQueue.Enqueue(TransportDepotAISystem.DepotAction.SetDisabled(vehicle, disabled));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProducedData.HasComponent(vehicle))
              entity1 = vehicle;
          }
        }
        if ((double) prefabTransportDepotData.m_MaintenanceDuration > 0.0)
        {
          float num3 = (float) (256.0 / (262144.0 * (double) prefabTransportDepotData.m_MaintenanceDuration)) * efficiency;
          transportDepot.m_MaintenanceRequirement = math.max(0.0f, transportDepot.m_MaintenanceRequirement - num3);
          num1 -= Mathf.CeilToInt(transportDepot.m_MaintenanceRequirement - 1f / 1000f);
        }
        if ((double) prefabTransportDepotData.m_ProductionDuration > 0.0)
        {
          float productionState = (float) (256.0 / (262144.0 * (double) prefabTransportDepotData.m_ProductionDuration)) * efficiency;
          if ((double) productionState > 0.0)
          {
            if (entity1 != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Produced component = this.m_ProducedData[entity1];
              if ((double) component.m_Completed < 1.0)
              {
                component.m_Completed = math.min(1f, component.m_Completed + productionState);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Produced>(jobIndex, entity1, component);
              }
              if ((double) component.m_Completed == 1.0 && !isSpectatorSite)
              {
                // ISSUE: reference to a compiler-generated method
                this.TryCreateLaunchEvent(jobIndex, entity, prefabTransportDepotData);
              }
            }
            else if (num1 > 0 && !isSpectatorSite)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnVehicle(jobIndex, ref random, entity, Entity.Null, transform, prefabRef, ref transportDepot, ref parkedVehicles, prefabTransportDepotData, productionState);
              --num1;
              ++num2;
            }
          }
        }
        int index1 = 0;
        bool flag1 = false;
        while (index1 < dispatches.Length)
        {
          Entity request = dispatches[index1].m_Request;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request) || this.m_TaxiRequestData.HasComponent(request))
          {
            if (num1 > 0)
            {
              if (!flag1)
              {
                // ISSUE: reference to a compiler-generated method
                flag1 = this.SpawnVehicle(jobIndex, ref random, entity, request, transform, prefabRef, ref transportDepot, ref parkedVehicles, prefabTransportDepotData, 0.0f);
                dispatches.RemoveAt(index1);
                if (flag1)
                  ++num2;
              }
              if (flag1)
                --num1;
            }
            else
              dispatches.RemoveAt(index1);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ServiceRequestData.HasComponent(request))
              dispatches.RemoveAt(index1);
            else
              ++index1;
          }
        }
        while (parkedVehicles.Length > math.max(0, prefabTransportDepotData.m_VehicleCapacity - num2))
        {
          int index2 = random.NextInt(parkedVehicles.Length);
          Entity entity2 = parkedVehicles[index2];
          DynamicBuffer<LayoutElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          this.m_LayoutElements.TryGetBuffer(entity2, out bufferData);
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity2, bufferData);
          parkedVehicles.RemoveAtSwapBack(index2);
        }
        for (int index3 = 0; index3 < parkedVehicles.Length; ++index3)
        {
          Entity entity3 = parkedVehicles[index3];
          Game.Vehicles.PublicTransport componentData10;
          bool flag2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PublicTransportData.TryGetComponent(entity3, out componentData10))
          {
            flag2 = (componentData10.m_State & PublicTransportFlags.Disabled) > (PublicTransportFlags) 0;
          }
          else
          {
            Game.Vehicles.CargoTransport componentData11;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CargoTransportData.TryGetComponent(entity3, out componentData11))
            {
              flag2 = (componentData11.m_State & CargoTransportFlags.Disabled) > (CargoTransportFlags) 0;
            }
            else
            {
              Game.Vehicles.Taxi componentData12;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TaxiData.TryGetComponent(entity3, out componentData12))
                flag2 = (componentData12.m_State & TaxiFlags.Disabled) > (TaxiFlags) 0;
              else
                continue;
            }
          }
          bool disabled = num1 <= 0;
          if (flag2 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(TransportDepotAISystem.DepotAction.SetDisabled(entity3, disabled));
          }
        }
        transportDepot.m_AvailableVehicles = (byte) math.clamp(num1, 0, (int) byte.MaxValue);
        if (num1 > 0)
        {
          transportDepot.m_Flags |= TransportDepotFlags.HasAvailableVehicles;
          // ISSUE: reference to a compiler-generated method
          this.RequestTargetIfNeeded(jobIndex, entity, ref transportDepot, prefabTransportDepotData, num1, vehicleCapacity1, isOutsideConnection);
        }
        else
          transportDepot.m_Flags &= ~TransportDepotFlags.HasAvailableVehicles;
        if (prefabTransportDepotData.m_DispatchCenter && (double) efficiency > 1.0 / 1000.0)
          transportDepot.m_Flags |= TransportDepotFlags.HasDispatchCenter;
        else
          transportDepot.m_Flags &= ~TransportDepotFlags.HasDispatchCenter;
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.TransportDepot transportDepot,
        TransportDepotData prefabTransportDepotData,
        int availableVehicles,
        int vehicleCapacity,
        bool isOutsideConnection)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(transportDepot.m_TargetRequest))
          return;
        if (prefabTransportDepotData.m_TransportType == TransportType.Taxi)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TaxiRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<TaxiRequest>(jobIndex, entity1, new TaxiRequest(entity, Entity.Null, Entity.Null, isOutsideConnection ? TaxiRequestType.Outside : TaxiRequestType.None, availableVehicles));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(16U));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TransportVehicleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity2, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<TransportVehicleRequest>(jobIndex, entity2, new TransportVehicleRequest(entity, (float) availableVehicles / (float) vehicleCapacity));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity2, new RequestGroup(8U));
        }
      }

      private bool TryCreateLaunchEvent(
        int jobIndex,
        Entity entity,
        TransportDepotData prefabTransportDepotData)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_EventPrefabChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk eventPrefabChunk = this.m_EventPrefabChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = eventPrefabChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<EventData> nativeArray2 = eventPrefabChunk.GetNativeArray<EventData>(ref this.m_EventType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<VehicleLaunchData> nativeArray3 = eventPrefabChunk.GetNativeArray<VehicleLaunchData>(ref this.m_VehicleLaunchType);
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = eventPrefabChunk.GetEnabledMask<Locked>(ref this.m_LockedType);
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            if ((!enabledMask.EnableBit.IsValid || !enabledMask[index2]) && nativeArray3[index2].m_TransportType == prefabTransportDepotData.m_TransportType)
            {
              Entity prefab = nativeArray1[index2];
              EventData eventData = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated field
              Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, eventData.m_Archetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity1, new PrefabRef(prefab));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetBuffer<TargetElement>(jobIndex, entity1).Add(new TargetElement(entity));
              return true;
            }
          }
        }
        return false;
      }

      private bool SpawnVehicle(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Entity request,
        Game.Objects.Transform transform,
        PrefabRef prefabRef,
        ref Game.Buildings.TransportDepot transportDepot,
        ref StackList<Entity> parkedVehicles,
        TransportDepotData prefabTransportDepotData,
        float productionState)
      {
        Entity entity1 = Entity.Null;
        Entity destination = Entity.Null;
        Entity origin = Entity.Null;
        Entity primaryPrefab = Entity.Null;
        Entity secondaryPrefab = Entity.Null;
        PublicTransportPurpose publicTransportPurpose = (PublicTransportPurpose) 0;
        Resource cargoResources = Resource.NoResource;
        int2 passengerCapacity = (int2) 0;
        int2 cargoCapacity = (int2) 0;
        TaxiRequestType taxiRequestType = TaxiRequestType.None;
        PathInformation componentData1 = new PathInformation();
        if ((double) productionState == 0.0)
        {
          TransportVehicleRequest componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.TryGetComponent(request, out componentData2))
          {
            entity1 = componentData2.m_Route;
          }
          else
          {
            TaxiRequest componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TaxiRequestData.TryGetComponent(request, out componentData3))
            {
              entity1 = componentData3.m_Seeker;
              taxiRequestType = componentData3.m_Type;
              passengerCapacity = new int2(1, int.MaxValue);
            }
          }
          PrefabRef componentData4;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.TryGetComponent(entity1, out componentData4))
            return false;
          TransportLineData componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTransportLineData.TryGetComponent(componentData4.m_Prefab, out componentData5))
          {
            publicTransportPurpose = componentData5.m_PassengerTransport ? PublicTransportPurpose.TransportLine : (PublicTransportPurpose) 0;
            cargoResources = componentData5.m_CargoTransport ? Resource.Food : Resource.NoResource;
            passengerCapacity = componentData5.m_PassengerTransport ? new int2(1, int.MaxValue) : (int2) 0;
            cargoCapacity = componentData5.m_CargoTransport ? new int2(1, int.MaxValue) : (int2) 0;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PathInformationData.TryGetComponent(request, out componentData1))
            return false;
          destination = componentData1.m_Destination;
          origin = componentData1.m_Origin;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EntityLookup.Exists(destination))
            return false;
          VehicleModel componentData6;
          // ISSUE: reference to a compiler-generated field
          if (this.m_VehicleModelData.TryGetComponent(entity1, out componentData6))
          {
            primaryPrefab = componentData6.m_PrimaryPrefab;
            secondaryPrefab = componentData6.m_SecondaryPrefab;
          }
        }
        else
        {
          DynamicBuffer<ActivityLocationElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ActivityLocationElements.TryGetBuffer(prefabRef.m_Prefab, out bufferData))
          {
            ActivityMask activityMask = new ActivityMask(ActivityType.Producing);
            for (int index = 0; index < bufferData.Length; ++index)
            {
              ActivityLocationElement activityLocationElement = bufferData[index];
              if (((int) activityLocationElement.m_ActivityMask.m_Mask & (int) activityMask.m_Mask) != 0)
                transform = ObjectUtils.LocalToWorld(transform, activityLocationElement.m_Position, activityLocationElement.m_Rotation);
            }
          }
          publicTransportPurpose = PublicTransportPurpose.Other;
        }
        Entity entity2 = Entity.Null;
        if (origin != Entity.Null && origin != entity)
        {
          if (!CollectionUtils.RemoveValueSwapBack<Entity>(ref parkedVehicles, origin))
            return false;
          entity2 = origin;
          DynamicBuffer<LayoutElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          this.m_LayoutElements.TryGetBuffer(entity2, out bufferData);
          switch (prefabTransportDepotData.m_TransportType)
          {
            case TransportType.Bus:
              // ISSUE: reference to a compiler-generated field
              ParkedCar parkedCar1 = this.m_ParkedCarData[entity2];
              Game.Vehicles.CarLaneFlags flags1 = Game.Vehicles.CarLaneFlags.EndReached | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity2, in this.m_ParkedToMovingRemoveTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_ParkedToMovingBusAddTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarCurrentLane>(jobIndex, entity2, new CarCurrentLane(parkedCar1, flags1));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkingLaneData.HasComponent(parkedCar1.m_Lane) || this.m_SpawnLocationData.HasComponent(parkedCar1.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, parkedCar1.m_Lane);
                break;
              }
              break;
            case TransportType.Train:
            case TransportType.Tram:
            case TransportType.Subway:
              for (int index = 0; index < bufferData.Length; ++index)
              {
                Entity vehicle = bufferData[index].m_Vehicle;
                // ISSUE: reference to a compiler-generated field
                ParkedTrain parkedTrain = this.m_ParkedTrainData[vehicle];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent(jobIndex, vehicle, in this.m_ParkedToMovingRemoveTypes);
                if (vehicle == entity2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent(jobIndex, vehicle, in this.m_ParkedToMovingTrainControllerAddTypes);
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SpawnLocationData.HasComponent(parkedTrain.m_ParkingLocation))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, parkedTrain.m_ParkingLocation);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent(jobIndex, vehicle, in this.m_ParkedToMovingTrainAddTypes);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<TrainCurrentLane>(jobIndex, vehicle, new TrainCurrentLane(parkedTrain));
              }
              break;
            case TransportType.Taxi:
              // ISSUE: reference to a compiler-generated field
              ParkedCar parkedCar2 = this.m_ParkedCarData[entity2];
              Game.Vehicles.CarLaneFlags flags2 = Game.Vehicles.CarLaneFlags.EndReached | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent(jobIndex, entity2, in this.m_ParkedToMovingRemoveTypes);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_ParkedToMovingTaxiAddTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarCurrentLane>(jobIndex, entity2, new CarCurrentLane(parkedCar2, flags2));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkingLaneData.HasComponent(parkedCar2.m_Lane) || this.m_SpawnLocationData.HasComponent(parkedCar2.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, parkedCar2.m_Lane);
                break;
              }
              break;
          }
        }
        if (entity2 == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          entity2 = this.m_TransportVehicleSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, transform, origin, primaryPrefab, secondaryPrefab, prefabTransportDepotData.m_TransportType, prefabTransportDepotData.m_EnergyTypes, publicTransportPurpose, cargoResources, ref passengerCapacity, ref cargoCapacity, false);
          if (entity2 == Entity.Null)
            return false;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, new Owner(entity));
          switch (prefabTransportDepotData.m_TransportType)
          {
            case TransportType.Train:
            case TransportType.Tram:
            case TransportType.Subway:
              // ISSUE: reference to a compiler-generated method
              this.RemoveCollidingParkedTrain(jobIndex, request, ref parkedVehicles);
              break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Common.Target>(jobIndex, entity2, new Game.Common.Target(destination));
        if ((double) productionState > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Moving>(jobIndex, entity2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<TransformFrame>(jobIndex, entity2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, entity2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Swaying>(jobIndex, entity2);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Produced>(jobIndex, entity2, new Produced(productionState));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Stopped>(jobIndex, entity2, new Stopped());
        }
        else
        {
          bool flag = taxiRequestType == TaxiRequestType.Customer || taxiRequestType == TaxiRequestType.Outside;
          if (flag)
          {
            if (prefabTransportDepotData.m_TransportType == TransportType.Taxi)
            {
              TaxiFlags flags = TaxiFlags.Dispatched;
              if (taxiRequestType == TaxiRequestType.Outside)
                flags |= TaxiFlags.FromOutside;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Game.Vehicles.Taxi>(jobIndex, entity2, new Game.Vehicles.Taxi(flags));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetBuffer<ServiceDispatch>(jobIndex, entity2).Add(new ServiceDispatch(request));
            }
          }
          else if (entity1 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CurrentRoute>(jobIndex, entity2, new CurrentRoute(entity1));
            if (origin == entity2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AppendToBuffer<RouteVehicle>(jobIndex, entity1, new RouteVehicle(entity2));
            }
            Game.Routes.Color componentData7;
            // ISSUE: reference to a compiler-generated field
            if (this.m_RouteColorData.TryGetComponent(entity1, out componentData7))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Routes.Color>(jobIndex, entity2, componentData7);
            }
            if (publicTransportPurpose != (PublicTransportPurpose) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Game.Vehicles.PublicTransport>(jobIndex, entity2, new Game.Vehicles.PublicTransport()
              {
                m_State = PublicTransportFlags.EnRoute
              });
            }
            if (cargoResources != Resource.NoResource)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Game.Vehicles.CargoTransport>(jobIndex, entity2, new Game.Vehicles.CargoTransport()
              {
                m_State = CargoTransportFlags.EnRoute
              });
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(request, entity2, !flag));
        }
        DynamicBuffer<PathElement> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathElements.TryGetBuffer(request, out bufferData1) && bufferData1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> targetElements = this.m_CommandBuffer.SetBuffer<PathElement>(jobIndex, entity2);
          PathUtils.CopyPath(bufferData1, new PathOwner(), 0, targetElements);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathOwner>(jobIndex, entity2, new PathOwner(PathFlags.Updated));
          if (prefabTransportDepotData.m_TransportType != TransportType.Taxi)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, entity2, componentData1);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(transportDepot.m_TargetRequest))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity4 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity4, new HandleRequest(transportDepot.m_TargetRequest, Entity.Null, true));
        }
        return true;
      }

      private void RemoveCollidingParkedTrain(
        int jobIndex,
        Entity pathEntity,
        ref StackList<Entity> parkedVehicles)
      {
        DynamicBuffer<PathElement> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PathElements.TryGetBuffer(pathEntity, out bufferData1) || bufferData1.Length == 0)
          return;
        Entity target = bufferData1[0].m_Target;
        for (int index = 0; index < parkedVehicles.Length; ++index)
        {
          Entity entity = parkedVehicles[index];
          ParkedTrain componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkedTrainData.TryGetComponent(entity, out componentData) && componentData.m_ParkingLocation == target)
          {
            DynamicBuffer<LayoutElement> bufferData2;
            // ISSUE: reference to a compiler-generated field
            this.m_LayoutElements.TryGetBuffer(entity, out bufferData2);
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity, bufferData2);
            parkedVehicles.RemoveAtSwapBack(index);
            break;
          }
        }
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
    private struct TransportDepotActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      public ComponentLookup<Game.Vehicles.Taxi> m_TaxiData;
      public ComponentLookup<Odometer> m_OdometerData;
      public NativeQueue<TransportDepotAISystem.DepotAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        TransportDepotAISystem.DepotAction depotAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out depotAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          TransportDepotAISystem.DepotActionType type = depotAction.m_Type;
          switch (type)
          {
            case TransportDepotAISystem.DepotActionType.SetDisabled:
              Game.Vehicles.PublicTransport componentData1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PublicTransportData.TryGetComponent(depotAction.m_Entity, out componentData1))
              {
                // ISSUE: reference to a compiler-generated field
                if (depotAction.m_Disabled)
                  componentData1.m_State |= PublicTransportFlags.AbandonRoute | PublicTransportFlags.Disabled;
                else
                  componentData1.m_State &= ~PublicTransportFlags.Disabled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PublicTransportData[depotAction.m_Entity] = componentData1;
              }
              Game.Vehicles.CargoTransport componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CargoTransportData.TryGetComponent(depotAction.m_Entity, out componentData2))
              {
                // ISSUE: reference to a compiler-generated field
                if (depotAction.m_Disabled)
                  componentData2.m_State |= CargoTransportFlags.AbandonRoute | CargoTransportFlags.Disabled;
                else
                  componentData2.m_State &= ~CargoTransportFlags.Disabled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CargoTransportData[depotAction.m_Entity] = componentData2;
              }
              Game.Vehicles.Taxi componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TaxiData.TryGetComponent(depotAction.m_Entity, out componentData3))
              {
                // ISSUE: reference to a compiler-generated field
                if (depotAction.m_Disabled)
                  componentData3.m_State |= TaxiFlags.Disabled;
                else
                  componentData3.m_State &= ~TaxiFlags.Disabled;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_TaxiData[depotAction.m_Entity] = componentData3;
                continue;
              }
              continue;
            case TransportDepotAISystem.DepotActionType.ClearOdometer:
              Odometer componentData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_OdometerData.TryGetComponent(depotAction.m_Entity, out componentData4))
              {
                componentData4.m_Distance = 0.0f;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_OdometerData[depotAction.m_Entity] = componentData4;
              }
              Game.Vehicles.PublicTransport componentData5;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PublicTransportData.TryGetComponent(depotAction.m_Entity, out componentData5))
              {
                componentData5.m_State &= ~PublicTransportFlags.RequiresMaintenance;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PublicTransportData[depotAction.m_Entity] = componentData5;
              }
              Game.Vehicles.CargoTransport componentData6;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CargoTransportData.TryGetComponent(depotAction.m_Entity, out componentData6))
              {
                componentData6.m_State &= ~CargoTransportFlags.RequiresMaintenance;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CargoTransportData[depotAction.m_Entity] = componentData6;
              }
              Game.Vehicles.Taxi componentData7;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TaxiData.TryGetComponent(depotAction.m_Entity, out componentData7))
              {
                componentData7.m_State &= ~TaxiFlags.RequiresMaintenance;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_TaxiData[depotAction.m_Entity] = componentData7;
                continue;
              }
              continue;
            default:
              continue;
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
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpectatorSite> __Game_Events_SpectatorSite_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EventData> __Game_Prefabs_EventData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<VehicleLaunchData> __Game_Prefabs_VehicleLaunchData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RW_ComponentTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> __Game_Simulation_TaxiRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleModel> __Game_Routes_VehicleModel_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Produced> __Game_Vehicles_Produced_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Odometer> __Game_Vehicles_Odometer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TaxiData> __Game_Prefabs_TaxiData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RW_ComponentLookup;
      public ComponentLookup<Odometer> __Game_Vehicles_Odometer_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_SpectatorSite_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpectatorSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleLaunchData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleLaunchData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TransportDepot>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup = state.GetComponentLookup<TransportVehicleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TaxiRequest_RO_ComponentLookup = state.GetComponentLookup<TaxiRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleModel_RO_ComponentLookup = state.GetComponentLookup<VehicleModel>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Produced_RO_ComponentLookup = state.GetComponentLookup<Produced>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.CargoTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Taxi>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RO_ComponentLookup = state.GetComponentLookup<Odometer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TaxiData_RO_ComponentLookup = state.GetComponentLookup<TaxiData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.CargoTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Taxi>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentLookup = state.GetComponentLookup<Odometer>();
      }
    }
  }
}
