// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GarbageTruckAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
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
  public class GarbageTruckAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private IconCommandSystem m_IconCommandSystem;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_GarbageCollectionRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private GarbageTruckAISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_647374864_0;
    private EntityQuery __query_647374864_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 2;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.GarbageTruck>(), ComponentType.ReadWrite<Game.Common.Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageCollectionRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<GarbageCollectionRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
        ComponentType.ReadWrite<Game.Common.Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedAddTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<Stopped>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
      this.RequireForUpdate<GarbageParameterData>();
      this.RequireForUpdate<ServiceFeeParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      GarbageParameterData singleton = this.__query_647374864_0.GetSingleton<GarbageParameterData>();
      NativeQueue<GarbageTruckAISystem.GarbageAction> nativeQueue = new NativeQueue<GarbageTruckAISystem.GarbageAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageTruckData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_GarbageTruck_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GarbageTruckAISystem.GarbageTruckTickJob jobData1 = new GarbageTruckAISystem.GarbageTruckTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_GarbageTruckType = this.__TypeHandle.__Game_Vehicles_GarbageTruck_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabGarbageTruckData = this.__TypeHandle.__Game_Prefabs_GarbageTruckData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabSpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PrefabZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_GarbageCollectionRequestData = this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup,
        m_GarbageProducerData = this.__TypeHandle.__Game_Buildings_GarbageProducer_RO_ComponentLookup,
        m_GarbageFacilityData = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_ServiceDistricts = this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_GarbageCollectionRequestArchetype = this.m_GarbageCollectionRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_GarbageParameters = singleton,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_GarbageTruck_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GarbageTruckAISystem.GarbageActionJob jobData2 = new GarbageTruckAISystem.GarbageActionJob()
      {
        m_GarbageTruckData = this.__TypeHandle.__Game_Vehicles_GarbageTruck_RW_ComponentLookup,
        m_GarbageCollectionRequestData = this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RW_ComponentLookup,
        m_GarbageProducerData = this.__TypeHandle.__Game_Buildings_GarbageProducer_RW_ComponentLookup,
        m_EconomyResources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_Efficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_GarbageParameters = singleton,
        m_GarbageEfficiencyPenalty = this.__query_647374864_1.GetSingleton<BuildingEfficiencyParameterData>().m_GarbagePenalty,
        m_ActionQueue = nativeQueue,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle handle = jobData1.ScheduleParallel<GarbageTruckAISystem.GarbageTruckTickJob>(this.m_VehicleQuery, this.Dependency);
      JobHandle dependsOn = handle;
      JobHandle jobHandle = jobData2.Schedule<GarbageTruckAISystem.GarbageActionJob>(dependsOn);
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(handle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ServiceFeeSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle);
      this.Dependency = jobHandle;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_647374864_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<GarbageParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_647374864_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public GarbageTruckAISystem()
    {
    }

    private enum GarbageActionType
    {
      Collect,
      Unload,
      AddRequest,
      ClearLane,
      BumpDispatchIndex,
    }

    private struct GarbageAction
    {
      public Entity m_Vehicle;
      public Entity m_Target;
      public Entity m_Request;
      public GarbageTruckAISystem.GarbageActionType m_Type;
      public int m_Capacity;
      public int m_MaxAmount;
    }

    [BurstCompile]
    private struct GarbageTruckTickJob : IJobChunk
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
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      public ComponentTypeHandle<Game.Vehicles.GarbageTruck> m_GarbageTruckType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<CarNavigationLane> m_CarNavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<GarbageTruckData> m_PrefabGarbageTruckData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_PrefabZoneData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.GarbageFacility> m_GarbageFacilityData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_GarbageCollectionRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      [ReadOnly]
      public GarbageParameterData m_GarbageParameters;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<GarbageTruckAISystem.GarbageAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<PathInformation> nativeArray4 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray5 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.GarbageTruck> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.GarbageTruck>(ref this.m_GarbageTruckType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray7 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray8 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray9 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathInformation pathInformation = nativeArray4[index];
          Game.Vehicles.GarbageTruck garbageTruck = nativeArray6[index];
          Car car = nativeArray7[index];
          CarCurrentLane currentLane = nativeArray5[index];
          PathOwner pathOwner = nativeArray9[index];
          Game.Common.Target target = nativeArray8[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor2[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, prefabRef, pathInformation, navigationLanes, serviceDispatches, ref random, ref garbageTruck, ref car, ref currentLane, ref pathOwner, ref target);
          nativeArray6[index] = garbageTruck;
          nativeArray7[index] = car;
          nativeArray5[index] = currentLane;
          nativeArray9[index] = pathOwner;
          nativeArray8[index] = target;
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, pathInformation, serviceDispatches, owner, ref random, ref garbageTruck, ref car, ref currentLane, ref pathOwner);
        }
        // ISSUE: reference to a compiler-generated field
        GarbageTruckData prefabGarbageTruckData = this.m_PrefabGarbageTruckData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if (VehicleUtils.IsStuck(pathOwner) || (garbageTruck.m_State & GarbageTruckFlags.Returning) != (GarbageTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.UnloadGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner.m_Owner, ref garbageTruck, true))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(owner, serviceDispatches, ref garbageTruck, ref car, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((garbageTruck.m_State & GarbageTruckFlags.Returning) != (GarbageTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.UnloadGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner.m_Owner, ref garbageTruck, false))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.TryCollectGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner, ref garbageTruck, ref car, ref currentLane, target.m_Target);
          // ISSUE: reference to a compiler-generated method
          this.TryCollectGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner, ref garbageTruck, ref car, ref target);
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, owner, ref garbageTruck, ref pathOwner);
          // ISSUE: reference to a compiler-generated method
          if (!this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, owner, ref garbageTruck, ref car, ref currentLane, ref pathOwner, ref target))
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(owner, serviceDispatches, ref garbageTruck, ref car, ref pathOwner, ref target);
          }
        }
        else
        {
          if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
          {
            if ((garbageTruck.m_State & GarbageTruckFlags.Returning) != (GarbageTruckFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.UnloadGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner.m_Owner, ref garbageTruck, false))
                return;
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, vehicleEntity, owner, ref garbageTruck, ref car, ref currentLane);
              return;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          if (VehicleUtils.WaypointReached(currentLane))
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
            // ISSUE: reference to a compiler-generated method
            this.TryCollectGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner, ref garbageTruck, ref car, ref currentLane, Entity.Null);
          }
          else if ((garbageTruck.m_State & GarbageTruckFlags.Unloading) != (GarbageTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.UnloadGarbage(jobIndex, vehicleEntity, prefabGarbageTruckData, owner.m_Owner, ref garbageTruck, true);
          }
        }
        if ((garbageTruck.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0)
        {
          if (garbageTruck.m_Garbage >= prefabGarbageTruckData.m_GarbageCapacity || (garbageTruck.m_State & GarbageTruckFlags.Disabled) != (GarbageTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(owner, serviceDispatches, ref garbageTruck, ref car, ref pathOwner, ref target);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckGarbagePresence(owner, ref currentLane, ref garbageTruck, ref car, navigationLanes);
          }
        }
        if (garbageTruck.m_Garbage + garbageTruck.m_EstimatedGarbage >= prefabGarbageTruckData.m_GarbageCapacity)
          garbageTruck.m_State |= GarbageTruckFlags.EstimatedFull;
        else
          garbageTruck.m_State &= ~GarbageTruckFlags.EstimatedFull;
        if (garbageTruck.m_Garbage < prefabGarbageTruckData.m_GarbageCapacity && (garbageTruck.m_State & GarbageTruckFlags.Disabled) == (GarbageTruckFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, owner, ref garbageTruck, ref pathOwner);
          if ((garbageTruck.m_State & GarbageTruckFlags.Returning) != (GarbageTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, owner, ref garbageTruck, ref car, ref currentLane, ref pathOwner, ref target);
          }
          if (garbageTruck.m_RequestCount <= 1 && (garbageTruck.m_State & GarbageTruckFlags.EstimatedFull) == (GarbageTruckFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref garbageTruck);
          }
        }
        else
          serviceDispatches.Clear();
        if ((garbageTruck.m_State & GarbageTruckFlags.Unloading) != (GarbageTruckFlags) 0)
          return;
        if (VehicleUtils.RequireNewPath(pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.FindNewPath(vehicleEntity, prefabRef, ref garbageTruck, ref currentLane, ref pathOwner, ref target);
        }
        else
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) != (PathFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingSpace(vehicleEntity, ref random, ref currentLane, ref pathOwner, navigationLanes);
        }
      }

      private void CheckParkingSpace(
        Entity entity,
        ref Unity.Mathematics.Random random,
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
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working);
        garbageTruck.m_State &= GarbageTruckFlags.IndustrialWasteOnly;
        Game.Buildings.GarbageFacility componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_GarbageFacilityData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & (GarbageFacilityFlags.HasAvailableGarbageTrucks | GarbageFacilityFlags.HasAvailableSpace)) != (GarbageFacilityFlags.HasAvailableGarbageTrucks | GarbageFacilityFlags.HasAvailableSpace))
          garbageTruck.m_State |= GarbageTruckFlags.Disabled;
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
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
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
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.SpecialParking,
          m_ParkingTarget = VehicleUtils.GetParkingSource(vehicleEntity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
          m_ParkingDelta = currentLane.m_CurvePosition.z,
          m_ParkingSize = VehicleUtils.GetParkingSize(vehicleEntity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData),
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
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
        if ((garbageTruck.m_State & GarbageTruckFlags.Returning) != (GarbageTruckFlags) 0)
        {
          destination.m_Methods |= PathMethod.SpecialParking;
          destination.m_RandomCost = 30f;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref PathOwner pathOwner)
      {
        if (serviceDispatches.Length <= garbageTruck.m_RequestCount)
          return;
        int num1 = -1;
        Entity request1 = Entity.Null;
        PathElement pathElement1 = new PathElement();
        bool flag = false;
        int num2 = 0;
        if (garbageTruck.m_RequestCount >= 1 && (garbageTruck.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0)
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
        for (int index = num2; index < garbageTruck.m_RequestCount; ++index)
        {
          DynamicBuffer<PathElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElements.TryGetBuffer(serviceDispatches[index].m_Request, out bufferData) && bufferData.Length != 0)
          {
            pathElement1 = bufferData[bufferData.Length - 1];
            flag = true;
          }
        }
        for (int requestCount = garbageTruck.m_RequestCount; requestCount < serviceDispatches.Length; ++requestCount)
        {
          Entity request2 = serviceDispatches[requestCount].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageCollectionRequestData.HasComponent(request2))
          {
            // ISSUE: reference to a compiler-generated field
            GarbageCollectionRequest collectionRequest = this.m_GarbageCollectionRequestData[request2];
            DynamicBuffer<PathElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (flag && this.m_PathElements.TryGetBuffer(request2, out bufferData) && bufferData.Length != 0)
            {
              PathElement pathElement3 = bufferData[0];
              if (pathElement3.m_Target != pathElement1.m_Target || (double) pathElement3.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(collectionRequest.m_Target) && collectionRequest.m_Priority > num1)
            {
              num1 = collectionRequest.m_Priority;
              request1 = request2;
            }
          }
        }
        if (request1 != Entity.Null)
        {
          serviceDispatches[garbageTruck.m_RequestCount++] = new ServiceDispatch(request1);
          // ISSUE: reference to a compiler-generated method
          this.PreAddCollectionRequests(request1, owner, ref garbageTruck);
        }
        if (serviceDispatches.Length <= garbageTruck.m_RequestCount)
          return;
        serviceDispatches.RemoveRange(garbageTruck.m_RequestCount, serviceDispatches.Length - garbageTruck.m_RequestCount);
      }

      private void PreAddCollectionRequests(
        Entity request,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck)
      {
        DynamicBuffer<PathElement> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PathElements.TryGetBuffer(request, out bufferData1))
          return;
        DynamicBuffer<ServiceDistrict> bufferData2;
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData2);
        // ISSUE: reference to a compiler-generated method
        int dispatchIndex = this.BumpDispachIndex(request);
        Entity owner1 = Entity.Null;
        for (int index = 0; index < bufferData1.Length; ++index)
        {
          PathElement pathElement = bufferData1[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EdgeLaneData.HasComponent(pathElement.m_Target))
          {
            owner1 = Entity.Null;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner2 = this.m_OwnerData[pathElement.m_Target];
            if (!(owner2.m_Owner == owner1))
            {
              owner1 = owner2.m_Owner;
              // ISSUE: reference to a compiler-generated method
              if (this.HasSidewalk(owner2.m_Owner))
              {
                // ISSUE: reference to a compiler-generated method
                garbageTruck.m_EstimatedGarbage += this.AddCollectionRequests(owner2.m_Owner, request, dispatchIndex, bufferData2, ref garbageTruck);
              }
            }
          }
        }
      }

      private bool HasSidewalk(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane = this.m_SubLanes[owner];
          for (int index = 0; index < subLane.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane[index].m_SubLane))
              return true;
          }
        }
        return false;
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Vehicles.GarbageTruck garbageTruck)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_GarbageCollectionRequestData.HasComponent(garbageTruck.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 16U) - 1) != 2)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_GarbageCollectionRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<GarbageCollectionRequest>(jobIndex, entity1, new GarbageCollectionRequest(entity, 1, (garbageTruck.m_State & GarbageTruckFlags.IndustrialWasteOnly) != (GarbageTruckFlags) 0 ? GarbageCollectionRequestFlags.IndustrialWaste : (GarbageCollectionRequestFlags) 0));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        if ((garbageTruck.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0 && garbageTruck.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          --garbageTruck.m_RequestCount;
        }
        for (; garbageTruck.m_RequestCount > 0 && serviceDispatches.Length > 0; --garbageTruck.m_RequestCount)
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity target1 = Entity.Null;
          GarbageCollectionRequest componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageCollectionRequestData.TryGetComponent(request, out componentData))
            target1 = componentData.m_Target;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EntityLookup.Exists(target1))
          {
            serviceDispatches.RemoveAt(0);
            garbageTruck.m_EstimatedGarbage -= garbageTruck.m_EstimatedGarbage / garbageTruck.m_RequestCount;
          }
          else
          {
            garbageTruck.m_State &= ~GarbageTruckFlags.Returning;
            car.m_Flags |= CarFlags.UsePublicTransportLanes;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(request, vehicleEntity, false, true));
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageCollectionRequestData.HasComponent(garbageTruck.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(garbageTruck.m_TargetRequest, Entity.Null, true));
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
                float num1 = garbageTruck.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                int appendedCount;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes, out appendedCount))
                {
                  DynamicBuffer<ServiceDistrict> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData);
                  // ISSUE: reference to a compiler-generated method
                  int dispatchIndex = this.BumpDispachIndex(request);
                  int index1 = pathElement2.Length - appendedCount;
                  int num2 = 0;
                  for (int index2 = 0; index2 < index1; ++index2)
                  {
                    PathElement pathElement3 = pathElement2[index2];
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PedestrianLaneData.HasComponent(pathElement3.m_Target))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Owner owner1 = this.m_OwnerData[pathElement3.m_Target];
                      // ISSUE: reference to a compiler-generated method
                      num2 += this.AddCollectionRequests(owner1.m_Owner, request, dispatchIndex, bufferData, ref garbageTruck);
                    }
                  }
                  if (appendedCount > 0)
                  {
                    NativeArray<PathElement> nativeArray = new NativeArray<PathElement>(appendedCount, Allocator.Temp);
                    for (int index3 = 0; index3 < appendedCount; ++index3)
                      nativeArray[index3] = pathElement2[index1 + index3];
                    pathElement2.RemoveRange(index1, appendedCount);
                    Entity lastOwner = Entity.Null;
                    for (int index4 = 0; index4 < nativeArray.Length; ++index4)
                    {
                      // ISSUE: reference to a compiler-generated method
                      num2 += this.AddPathElement(pathElement2, nativeArray[index4], request, dispatchIndex, ref lastOwner, ref garbageTruck, bufferData);
                    }
                    nativeArray.Dispose();
                  }
                  if (garbageTruck.m_RequestCount == 1)
                    garbageTruck.m_EstimatedGarbage = num2;
                  car.m_Flags |= CarFlags.StayOnRoad;
                  garbageTruck.m_PathElementTime = num1 / (float) math.max(1, pathElement2.Length);
                  target.m_Target = target1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, target1);
            return true;
          }
        }
        return false;
      }

      private int BumpDispachIndex(Entity request)
      {
        int num = 0;
        GarbageCollectionRequest componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_GarbageCollectionRequestData.TryGetComponent(request, out componentData))
        {
          num = (int) componentData.m_DispatchIndex + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new GarbageTruckAISystem.GarbageAction()
          {
            m_Type = GarbageTruckAISystem.GarbageActionType.BumpDispatchIndex,
            m_Request = request
          });
        }
        return num;
      }

      private void ReturnToDepot(
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        ref PathOwner pathOwnerData,
        ref Game.Common.Target targetData)
      {
        serviceDispatches.Clear();
        garbageTruck.m_RequestCount = 0;
        garbageTruck.m_EstimatedGarbage = 0;
        garbageTruck.m_State |= GarbageTruckFlags.Returning;
        car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working);
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, ownerData.m_Owner);
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        Owner owner,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car carData,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
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
        // ISSUE: reference to a compiler-generated field
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
        currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Checked;
        if ((garbageTruck.m_State & GarbageTruckFlags.Returning) == (GarbageTruckFlags) 0 && garbageTruck.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageCollectionRequestData.HasComponent(request))
          {
            NativeArray<PathElement> nativeArray = new NativeArray<PathElement>(pathElement.Length, Allocator.Temp);
            nativeArray.CopyFrom(pathElement.AsNativeArray());
            pathElement.Clear();
            DynamicBuffer<ServiceDistrict> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData);
            Entity lastOwner = Entity.Null;
            int num = 0;
            // ISSUE: reference to a compiler-generated method
            int dispatchIndex = this.BumpDispachIndex(request);
            for (int index = 0; index < nativeArray.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              num = this.AddPathElement(pathElement, nativeArray[index], request, dispatchIndex, ref lastOwner, ref garbageTruck, bufferData);
            }
            if (garbageTruck.m_RequestCount == 1)
              garbageTruck.m_EstimatedGarbage = num;
            nativeArray.Dispose();
          }
          carData.m_Flags |= CarFlags.StayOnRoad;
        }
        else
          carData.m_Flags &= ~CarFlags.StayOnRoad;
        carData.m_Flags |= CarFlags.UsePublicTransportLanes;
        garbageTruck.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement.Length);
      }

      private int AddPathElement(
        DynamicBuffer<PathElement> path,
        PathElement pathElement,
        Entity request,
        int dispatchIndex,
        ref Entity lastOwner,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        DynamicBuffer<ServiceDistrict> serviceDistricts)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.HasComponent(pathElement.m_Target))
        {
          path.Add(pathElement);
          lastOwner = Entity.Null;
          return num;
        }
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[pathElement.m_Target];
        if (owner.m_Owner == lastOwner)
        {
          path.Add(pathElement);
          return num;
        }
        lastOwner = owner.m_Owner;
        float y = pathElement.m_TargetDelta.y;
        Entity sidewalk;
        // ISSUE: reference to a compiler-generated method
        if (this.FindClosestSidewalk(pathElement.m_Target, owner.m_Owner, ref y, out sidewalk))
        {
          // ISSUE: reference to a compiler-generated method
          num = this.AddCollectionRequests(owner.m_Owner, request, dispatchIndex, serviceDistricts, ref garbageTruck);
          path.Add(pathElement);
          path.Add(new PathElement(sidewalk, (float2) y));
        }
        else
          path.Add(pathElement);
        return num;
      }

      private bool FindClosestSidewalk(
        Entity lane,
        Entity owner,
        ref float curvePos,
        out Entity sidewalk)
      {
        bool closestSidewalk = false;
        sidewalk = Entity.Null;
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.TryGetBuffer(owner, out bufferData))
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = MathUtils.Position(this.m_CurveData[lane].m_Bezier, curvePos);
          float num1 = float.MaxValue;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity subLane = bufferData[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane))
            {
              float t;
              // ISSUE: reference to a compiler-generated field
              float num2 = MathUtils.Distance(MathUtils.Line(this.m_CurveData[subLane].m_Bezier), position, out t);
              if ((double) num2 < (double) num1)
              {
                curvePos = t;
                sidewalk = subLane;
                num1 = num2;
                closestSidewalk = true;
              }
            }
          }
        }
        return closestSidewalk;
      }

      private int AddCollectionRequests(
        Entity edgeEntity,
        Entity request,
        int dispatchIndex,
        DynamicBuffer<ServiceDistrict> serviceDistricts,
        ref Game.Vehicles.GarbageTruck garbageTruck)
      {
        int num = 0;
        DynamicBuffer<ConnectedBuilding> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedBuildings.TryGetBuffer(edgeEntity, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity building = bufferData[index].m_Building;
            GarbageProducer componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageProducerData.TryGetComponent(building, out componentData) && ((garbageTruck.m_State & GarbageTruckFlags.IndustrialWasteOnly) == (GarbageTruckFlags) 0 || this.IsIndustrial(this.m_PrefabRefData[building].m_Prefab)) && AreaUtils.CheckServiceDistrict(building, serviceDistricts, ref this.m_CurrentDistrictData))
            {
              num += componentData.m_Garbage;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_ActionQueue.Enqueue(new GarbageTruckAISystem.GarbageAction()
              {
                m_Type = GarbageTruckAISystem.GarbageActionType.AddRequest,
                m_Request = request,
                m_Target = building,
                m_Capacity = dispatchIndex
              });
            }
          }
        }
        return num;
      }

      private void CheckGarbagePresence(
        Owner owner,
        ref CarCurrentLane currentLane,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        if ((garbageTruck.m_State & GarbageTruckFlags.ClearChecked) != (GarbageTruckFlags) 0)
        {
          if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0)
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Checked;
          garbageTruck.m_State &= ~GarbageTruckFlags.ClearChecked;
        }
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.Waypoint | Game.Vehicles.CarLaneFlags.Checked)) == Game.Vehicles.CarLaneFlags.Waypoint)
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.CheckGarbagePresence(currentLane.m_Lane, owner, ref garbageTruck))
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
            car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working);
            // ISSUE: reference to a compiler-generated field
            if (this.m_SlaveLaneData.HasComponent(currentLane.m_Lane))
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.FixedLane;
          }
          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
        }
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0)
        {
          car.m_Flags |= (double) math.abs(currentLane.m_CurvePosition.x - currentLane.m_CurvePosition.z) < 0.5 ? CarFlags.Warning | CarFlags.Working : CarFlags.Warning;
        }
        else
        {
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            ref CarNavigationLane local = ref navigationLanes.ElementAt(index);
            if ((local.m_Flags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Checked) == (Game.Vehicles.CarLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.CheckGarbagePresence(local.m_Lane, owner, ref garbageTruck))
              {
                local.m_Flags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
                car.m_Flags &= ~CarFlags.Warning;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SlaveLaneData.HasComponent(local.m_Lane))
                  local.m_Flags &= ~Game.Vehicles.CarLaneFlags.FixedLane;
              }
              currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
              car.m_Flags &= ~CarFlags.Working;
            }
            if ((local.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.Waypoint)) != Game.Vehicles.CarLaneFlags.Reserved)
            {
              car.m_Flags &= ~CarFlags.Working;
              break;
            }
          }
        }
      }

      private bool CheckGarbagePresence(
        Entity laneEntity,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck)
      {
        Owner componentData1;
        DynamicBuffer<ConnectedBuilding> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeLaneData.HasComponent(laneEntity) && this.m_OwnerData.TryGetComponent(laneEntity, out componentData1) && this.m_ConnectedBuildings.TryGetBuffer(componentData1.m_Owner, out bufferData1))
        {
          DynamicBuffer<ServiceDistrict> bufferData2;
          // ISSUE: reference to a compiler-generated field
          this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData2);
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            Entity building = bufferData1[index].m_Building;
            GarbageProducer componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageProducerData.TryGetComponent(building, out componentData2) && componentData2.m_Garbage > this.m_GarbageParameters.m_CollectionGarbageLimit && ((garbageTruck.m_State & GarbageTruckFlags.IndustrialWasteOnly) == (GarbageTruckFlags) 0 || this.IsIndustrial(this.m_PrefabRefData[building].m_Prefab)) && AreaUtils.CheckServiceDistrict(building, bufferData2, ref this.m_CurrentDistrictData))
              return true;
          }
        }
        return false;
      }

      private void TryCollectGarbage(
        int jobIndex,
        Entity vehicleEntity,
        GarbageTruckData prefabGarbageTruckData,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        ref CarCurrentLane currentLaneData,
        Entity ignoreBuilding)
      {
        if (garbageTruck.m_Garbage >= prefabGarbageTruckData.m_GarbageCapacity)
          return;
        // ISSUE: reference to a compiler-generated method
        this.TryCollectGarbageFromLane(jobIndex, vehicleEntity, prefabGarbageTruckData, owner, ref garbageTruck, ref car, currentLaneData.m_Lane, ignoreBuilding);
      }

      private void TryCollectGarbage(
        int jobIndex,
        Entity vehicleEntity,
        GarbageTruckData prefabGarbageTruckData,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        ref Game.Common.Target target)
      {
        if (garbageTruck.m_Garbage >= prefabGarbageTruckData.m_GarbageCapacity)
          return;
        DynamicBuffer<ServiceDistrict> bufferData;
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData);
        // ISSUE: reference to a compiler-generated method
        this.TryCollectGarbageFromBuilding(jobIndex, vehicleEntity, prefabGarbageTruckData, ref garbageTruck, ref car, target.m_Target, bufferData);
      }

      private void TryCollectGarbageFromLane(
        int jobIndex,
        Entity vehicleEntity,
        GarbageTruckData prefabGarbageTruckData,
        Owner owner,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        Entity laneEntity,
        Entity ignoreBuilding)
      {
        Owner componentData;
        DynamicBuffer<ConnectedBuilding> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.HasComponent(laneEntity) || !this.m_OwnerData.TryGetComponent(laneEntity, out componentData) || !this.m_ConnectedBuildings.TryGetBuffer(componentData.m_Owner, out bufferData1))
          return;
        bool flag = false;
        DynamicBuffer<ServiceDistrict> bufferData2;
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceDistricts.TryGetBuffer(owner.m_Owner, out bufferData2);
        for (int index = 0; index < bufferData1.Length; ++index)
        {
          Entity building = bufferData1[index].m_Building;
          if (building != ignoreBuilding)
          {
            // ISSUE: reference to a compiler-generated method
            flag |= this.TryCollectGarbageFromBuilding(jobIndex, vehicleEntity, prefabGarbageTruckData, ref garbageTruck, ref car, building, bufferData2);
          }
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new GarbageTruckAISystem.GarbageAction()
        {
          m_Type = GarbageTruckAISystem.GarbageActionType.ClearLane,
          m_Vehicle = vehicleEntity,
          m_Target = laneEntity
        });
      }

      private bool TryCollectGarbageFromBuilding(
        int jobIndex,
        Entity vehicleEntity,
        GarbageTruckData prefabGarbageTruckData,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        ref Car car,
        Entity buildingEntity,
        DynamicBuffer<ServiceDistrict> serviceDistricts)
      {
        GarbageProducer componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (!this.m_GarbageProducerData.TryGetComponent(buildingEntity, out componentData) || componentData.m_Garbage <= this.m_GarbageParameters.m_CollectionGarbageLimit || (garbageTruck.m_State & GarbageTruckFlags.IndustrialWasteOnly) != (GarbageTruckFlags) 0 && !this.IsIndustrial(this.m_PrefabRefData[buildingEntity].m_Prefab) || !AreaUtils.CheckServiceDistrict(buildingEntity, serviceDistricts, ref this.m_CurrentDistrictData))
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new GarbageTruckAISystem.GarbageAction()
        {
          m_Type = GarbageTruckAISystem.GarbageActionType.Collect,
          m_Vehicle = vehicleEntity,
          m_Target = buildingEntity,
          m_Capacity = prefabGarbageTruckData.m_GarbageCapacity
        });
        // ISSUE: reference to a compiler-generated field
        if (componentData.m_Garbage >= this.m_GarbageParameters.m_RequestGarbageLimit)
        {
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(jobIndex, buildingEntity);
        }
        car.m_Flags |= CarFlags.Warning | CarFlags.Working;
        return true;
      }

      private void QuantityUpdated(int jobIndex, Entity buildingEntity, bool updateAll = false)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(buildingEntity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          bool updateAll1 = false;
          // ISSUE: reference to a compiler-generated field
          if (updateAll || this.m_QuantityData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject, new BatchesUpdated());
            updateAll1 = true;
          }
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(jobIndex, subObject, updateAll1);
        }
      }

      private bool IsIndustrial(Entity prefab)
      {
        SpawnableBuildingData componentData1;
        ZoneData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabSpawnableBuildingData.TryGetComponent(prefab, out componentData1) && this.m_PrefabZoneData.TryGetComponent(componentData1.m_ZonePrefab, out componentData2) && componentData2.m_AreaType == Game.Zones.AreaType.Industrial;
      }

      private bool UnloadGarbage(
        int jobIndex,
        Entity vehicleEntity,
        GarbageTruckData prefabGarbageTruckData,
        Entity facilityEntity,
        ref Game.Vehicles.GarbageTruck garbageTruck,
        bool instant)
      {
        // ISSUE: reference to a compiler-generated field
        if (garbageTruck.m_Garbage > 0 && this.m_GarbageFacilityData.HasComponent(facilityEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new GarbageTruckAISystem.GarbageAction()
          {
            m_Type = GarbageTruckAISystem.GarbageActionType.Unload,
            m_Vehicle = vehicleEntity,
            m_Target = facilityEntity,
            m_MaxAmount = math.select(Mathf.RoundToInt((float) (prefabGarbageTruckData.m_UnloadRate * 16) / 60f), garbageTruck.m_Garbage, instant)
          });
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(jobIndex, facilityEntity);
          return false;
        }
        if ((garbageTruck.m_State & GarbageTruckFlags.Unloading) != (GarbageTruckFlags) 0)
        {
          garbageTruck.m_State &= ~GarbageTruckFlags.Unloading;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity, new EffectsUpdated());
        }
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
    private struct GarbageActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.GarbageTruck> m_GarbageTruckData;
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
      public ComponentLookup<GarbageProducer> m_GarbageProducerData;
      public BufferLookup<Game.Economy.Resources> m_EconomyResources;
      public BufferLookup<Efficiency> m_Efficiencies;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public GarbageParameterData m_GarbageParameters;
      public float m_GarbageEfficiencyPenalty;
      public NativeQueue<GarbageTruckAISystem.GarbageAction> m_ActionQueue;
      public IconCommandBuffer m_IconCommandBuffer;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        GarbageTruckAISystem.GarbageAction garbageAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out garbageAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GarbageTruckAISystem.GarbageActionType type = garbageAction.m_Type;
          switch (type)
          {
            case GarbageTruckAISystem.GarbageActionType.Collect:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.GarbageTruck garbageTruck1 = this.m_GarbageTruckData[garbageAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              GarbageProducer garbageProducer1 = this.m_GarbageProducerData[garbageAction.m_Target];
              // ISSUE: reference to a compiler-generated field
              int num = math.min(garbageAction.m_Capacity - garbageTruck1.m_Garbage, garbageProducer1.m_Garbage);
              if (num > 0)
              {
                garbageTruck1.m_Garbage += num;
                garbageTruck1.m_EstimatedGarbage = math.max(0, garbageTruck1.m_EstimatedGarbage - num);
                garbageProducer1.m_Garbage -= num;
                // ISSUE: reference to a compiler-generated field
                if ((garbageProducer1.m_Flags & GarbageProducerFlags.GarbagePilingUpWarning) != GarbageProducerFlags.None && garbageProducer1.m_Garbage <= this.m_GarbageParameters.m_WarningGarbageLimit)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(garbageAction.m_Target, this.m_GarbageParameters.m_GarbageNotificationPrefab);
                  garbageProducer1.m_Flags &= ~GarbageProducerFlags.GarbagePilingUpWarning;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_GarbageTruckData[garbageAction.m_Vehicle] = garbageTruck1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_GarbageProducerData[garbageAction.m_Target] = garbageProducer1;
                DynamicBuffer<Efficiency> bufferData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Efficiencies.TryGetBuffer(garbageAction.m_Target, out bufferData))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  float efficiencyFactor = GarbageAccumulationSystem.GetGarbageEfficiencyFactor(garbageProducer1.m_Garbage, this.m_GarbageParameters, this.m_GarbageEfficiencyPenalty);
                  BuildingUtils.SetEfficiencyFactor(bufferData, EfficiencyFactor.Garbage, efficiencyFactor);
                  continue;
                }
                continue;
              }
              continue;
            case GarbageTruckAISystem.GarbageActionType.Unload:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.GarbageTruck garbageTruck2 = this.m_GarbageTruckData[garbageAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              int amount = math.min(garbageTruck2.m_Garbage, garbageAction.m_MaxAmount);
              if (amount > 0)
              {
                garbageTruck2.m_Garbage -= amount;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_EconomyResources.HasBuffer(garbageAction.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Game.Economy.Resources> economyResource = this.m_EconomyResources[garbageAction.m_Target];
                  EconomyUtils.AddResources(Resource.Garbage, amount, economyResource);
                }
                if ((garbageTruck2.m_State & GarbageTruckFlags.Unloading) == (GarbageTruckFlags) 0)
                {
                  garbageTruck2.m_State |= GarbageTruckFlags.Unloading;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<EffectsUpdated>(garbageAction.m_Vehicle, new EffectsUpdated());
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_GarbageTruckData[garbageAction.m_Vehicle] = garbageTruck2;
                continue;
              }
              if ((garbageTruck2.m_State & GarbageTruckFlags.Unloading) != (GarbageTruckFlags) 0)
              {
                garbageTruck2.m_State &= ~GarbageTruckFlags.Unloading;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<EffectsUpdated>(garbageAction.m_Vehicle, new EffectsUpdated());
                continue;
              }
              continue;
            case GarbageTruckAISystem.GarbageActionType.AddRequest:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              GarbageProducer garbageProducer2 = this.m_GarbageProducerData[garbageAction.m_Target] with
              {
                m_CollectionRequest = garbageAction.m_Request,
                m_DispatchIndex = (byte) garbageAction.m_Capacity
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_GarbageProducerData[garbageAction.m_Target] = garbageProducer2;
              continue;
            case GarbageTruckAISystem.GarbageActionType.ClearLane:
              DynamicBuffer<LaneObject> bufferData1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneObjects.TryGetBuffer(garbageAction.m_Target, out bufferData1))
              {
                for (int index = 0; index < bufferData1.Length; ++index)
                {
                  Entity laneObject = bufferData1[index].m_LaneObject;
                  Game.Vehicles.GarbageTruck componentData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (laneObject != garbageAction.m_Vehicle && this.m_GarbageTruckData.TryGetComponent(laneObject, out componentData))
                  {
                    componentData.m_State |= GarbageTruckFlags.ClearChecked;
                    // ISSUE: reference to a compiler-generated field
                    this.m_GarbageTruckData[laneObject] = componentData;
                  }
                }
                continue;
              }
              continue;
            case GarbageTruckAISystem.GarbageActionType.BumpDispatchIndex:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              GarbageCollectionRequest collectionRequest = this.m_GarbageCollectionRequestData[garbageAction.m_Request];
              ++collectionRequest.m_DispatchIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_GarbageCollectionRequestData[garbageAction.m_Request] = collectionRequest;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.GarbageTruck> __Game_Vehicles_GarbageTruck_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageTruckData> __Game_Prefabs_GarbageTruckData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> __Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.GarbageFacility> __Game_Buildings_GarbageFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> __Game_Areas_ServiceDistrict_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public ComponentLookup<Game.Vehicles.GarbageTruck> __Game_Vehicles_GarbageTruck_RW_ComponentLookup;
      public ComponentLookup<GarbageCollectionRequest> __Game_Simulation_GarbageCollectionRequest_RW_ComponentLookup;
      public ComponentLookup<GarbageProducer> __Game_Buildings_GarbageProducer_RW_ComponentLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RW_BufferLookup;

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
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GarbageTruck_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.GarbageTruck>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageTruckData_RO_ComponentLookup = state.GetComponentLookup<GarbageTruckData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup = state.GetComponentLookup<GarbageCollectionRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RO_ComponentLookup = state.GetComponentLookup<GarbageProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.GarbageFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_ServiceDistrict_RO_BufferLookup = state.GetBufferLookup<ServiceDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GarbageTruck_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.GarbageTruck>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageCollectionRequest_RW_ComponentLookup = state.GetComponentLookup<GarbageCollectionRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageProducer_RW_ComponentLookup = state.GetComponentLookup<GarbageProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferLookup = state.GetBufferLookup<Efficiency>();
      }
    }
  }
}
