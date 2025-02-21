// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PostVanAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Net;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PostVanAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_VehicleQuery;
    private EntityQuery m_PostConfigurationQuery;
    private EntityArchetype m_PostVanRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private PostVanAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 9;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.PostVan>(), ComponentType.ReadWrite<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_PostConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PostConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PostVanRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
      NativeQueue<PostVanAISystem.MailAction> nativeQueue = new NativeQueue<PostVanAISystem.MailAction>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
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
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PostVanData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_PostVan_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = new PostVanAISystem.PostVanTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_PostVanType = this.__TypeHandle.__Game_Vehicles_PostVan_RW_ComponentTypeHandle,
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
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabPostVanData = this.__TypeHandle.__Game_Prefabs_PostVanData_RO_ComponentLookup,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_MailAccumulationData = this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup,
        m_ServiceObjectData = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PostVanRequestData = this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup,
        m_MailProducerData = this.__TypeHandle.__Game_Buildings_MailProducer_RO_ComponentLookup,
        m_PostFacilityData = this.__TypeHandle.__Game_Buildings_PostFacility_RO_ComponentLookup,
        m_MailBoxData = this.__TypeHandle.__Game_Routes_MailBox_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_PostVanRequestArchetype = this.m_PostVanRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_PostConfigurationData = this.m_PostConfigurationQuery.GetSingleton<PostConfigurationData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      }.ScheduleParallel<PostVanAISystem.PostVanTickJob>(this.m_VehicleQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_MailBox_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PostVanRequest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PostVan_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle2 = new PostVanAISystem.MailActionJob()
      {
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_PostVanData = this.__TypeHandle.__Game_Vehicles_PostVan_RW_ComponentLookup,
        m_PostVanRequestData = this.__TypeHandle.__Game_Simulation_PostVanRequest_RW_ComponentLookup,
        m_MailProducerData = this.__TypeHandle.__Game_Buildings_MailProducer_RW_ComponentLookup,
        m_MailBoxData = this.__TypeHandle.__Game_Routes_MailBox_RW_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_ActionQueue = nativeQueue,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      }.Schedule<PostVanAISystem.MailActionJob>(JobHandle.CombineDependencies(jobHandle1, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle2);
      nativeQueue.Dispose(jobHandle2);
      this.Dependency = jobHandle2;
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
    public PostVanAISystem()
    {
    }

    private enum MailActionType
    {
      AddRequest,
      HandleBuilding,
      HandleMailBox,
      UnloadAll,
      ClearLane,
      BumpDispatchIndex,
    }

    private struct MailAction
    {
      public PostVanAISystem.MailActionType m_Type;
      public Entity m_Vehicle;
      public Entity m_Target;
      public Entity m_Request;
      public int m_DeliverAmount;
      public int m_CollectAmount;
    }

    [BurstCompile]
    private struct PostVanTickJob : IJobChunk
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
      public ComponentTypeHandle<Game.Vehicles.PostVan> m_PostVanType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Target> m_TargetType;
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
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<PostVanData> m_PrefabPostVanData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> m_MailAccumulationData;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> m_PostVanRequestData;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PostFacility> m_PostFacilityData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.MailBox> m_MailBoxData;
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
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_PostVanRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      [ReadOnly]
      public PostConfigurationData m_PostConfigurationData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<PostVanAISystem.MailAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<Game.Vehicles.PostVan> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.PostVan>(ref this.m_PostVanType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray7 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray8 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray9 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathInformation pathInformation = nativeArray4[index];
          Game.Vehicles.PostVan postVan = nativeArray6[index];
          Car car = nativeArray7[index];
          CarCurrentLane currentLane = nativeArray5[index];
          PathOwner pathOwner = nativeArray9[index];
          Target target = nativeArray8[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor2[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, prefabRef, pathInformation, navigationLanes, serviceDispatches, ref random, ref postVan, ref car, ref currentLane, ref pathOwner, ref target);
          nativeArray6[index] = postVan;
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
        ref Random random,
        ref Game.Vehicles.PostVan postVan,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        PostVanData prefabPostVanData = this.m_PrefabPostVanData[prefabRef.m_Prefab];
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, pathInformation, serviceDispatches, prefabPostVanData, ref random, ref postVan, ref car, ref currentLane, ref pathOwner);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if (VehicleUtils.IsStuck(pathOwner) || (postVan.m_State & PostVanFlags.Returning) != (PostVanFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            // ISSUE: reference to a compiler-generated method
            this.UnloadMail(vehicleEntity, owner.m_Owner, ref postVan);
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToFacility(owner, serviceDispatches, ref postVan, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((postVan.m_State & PostVanFlags.Returning) != (PostVanFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            // ISSUE: reference to a compiler-generated method
            this.UnloadMail(vehicleEntity, owner.m_Owner, ref postVan);
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.TryHandleBuildings(jobIndex, vehicleEntity, prefabPostVanData, ref postVan, ref currentLane, target.m_Target);
          // ISSUE: reference to a compiler-generated method
          this.TryHandleBuilding(jobIndex, vehicleEntity, prefabPostVanData, ref postVan, target.m_Target);
          // ISSUE: reference to a compiler-generated method
          this.TryHandleMailBox(vehicleEntity, prefabPostVanData, ref postVan, ref target);
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, prefabPostVanData, ref postVan, ref pathOwner);
          // ISSUE: reference to a compiler-generated method
          if (!this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, prefabPostVanData, ref postVan, ref car, ref currentLane, ref pathOwner, ref target))
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToFacility(owner, serviceDispatches, ref postVan, ref pathOwner, ref target);
          }
        }
        else
        {
          if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
          {
            if ((postVan.m_State & PostVanFlags.Returning) != (PostVanFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, vehicleEntity, owner, ref postVan, ref car, ref currentLane);
              // ISSUE: reference to a compiler-generated method
              this.UnloadMail(vehicleEntity, owner.m_Owner, ref postVan);
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
            this.TryHandleBuildings(jobIndex, vehicleEntity, prefabPostVanData, ref postVan, ref currentLane, Entity.Null);
          }
        }
        if (postVan.m_CollectedMail >= prefabPostVanData.m_MailCapacity)
          postVan.m_State |= PostVanFlags.CollectFull | PostVanFlags.EstimatedFull;
        else if (postVan.m_CollectedMail + postVan.m_CollectEstimate >= prefabPostVanData.m_MailCapacity)
        {
          postVan.m_State |= PostVanFlags.EstimatedFull;
          postVan.m_State &= ~PostVanFlags.CollectFull;
        }
        else
          postVan.m_State &= ~(PostVanFlags.CollectFull | PostVanFlags.EstimatedFull);
        if (postVan.m_DeliveringMail <= 0)
          postVan.m_State |= PostVanFlags.DeliveryEmpty | PostVanFlags.EstimatedEmpty;
        else if (postVan.m_DeliveringMail - postVan.m_DeliveryEstimate <= 0)
        {
          postVan.m_State |= PostVanFlags.EstimatedEmpty;
          postVan.m_State &= ~PostVanFlags.DeliveryEmpty;
        }
        else
          postVan.m_State &= ~(PostVanFlags.DeliveryEmpty | PostVanFlags.EstimatedEmpty);
        if ((postVan.m_State & (PostVanFlags.Returning | PostVanFlags.Delivering)) == PostVanFlags.Delivering && (postVan.m_State & (PostVanFlags.DeliveryEmpty | PostVanFlags.Disabled)) != (PostVanFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReturnToFacility(owner, serviceDispatches, ref postVan, ref pathOwner, ref target);
        }
        if ((postVan.m_State & (PostVanFlags.Returning | PostVanFlags.Collecting)) == PostVanFlags.Collecting && (postVan.m_State & (PostVanFlags.CollectFull | PostVanFlags.Disabled)) != (PostVanFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ReturnToFacility(owner, serviceDispatches, ref postVan, ref pathOwner, ref target);
        }
        if ((postVan.m_State & (PostVanFlags.DeliveryEmpty | PostVanFlags.CollectFull)) != (PostVanFlags.DeliveryEmpty | PostVanFlags.CollectFull) && (postVan.m_State & PostVanFlags.Disabled) == (PostVanFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, prefabPostVanData, ref postVan, ref pathOwner);
          if ((postVan.m_State & PostVanFlags.Returning) != (PostVanFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, prefabPostVanData, ref postVan, ref car, ref currentLane, ref pathOwner, ref target);
          }
          if (postVan.m_RequestCount <= 1 && (postVan.m_State & (PostVanFlags.EstimatedEmpty | PostVanFlags.EstimatedFull)) != (PostVanFlags.EstimatedEmpty | PostVanFlags.EstimatedFull))
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref postVan);
          }
        }
        else
          serviceDispatches.Clear();
        // ISSUE: reference to a compiler-generated method
        this.CheckBuildings(prefabPostVanData, ref postVan, ref currentLane, navigationLanes);
        if (VehicleUtils.RequireNewPath(pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.FindNewPath(vehicleEntity, prefabRef, ref postVan, ref currentLane, ref pathOwner, ref target);
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
        ref Game.Vehicles.PostVan postVan,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        postVan.m_State = (PostVanFlags) 0;
        Game.Buildings.PostFacility componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PostFacilityData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & (PostFacilityFlags.CanDeliverMailWithVan | PostFacilityFlags.CanCollectMailWithVan)) == (PostFacilityFlags) 0)
          postVan.m_State |= PostVanFlags.Disabled;
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
        ref Game.Vehicles.PostVan postVan,
        ref CarCurrentLane currentLane,
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
        if ((postVan.m_State & PostVanFlags.Returning) != (PostVanFlags) 0)
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
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        ref PathOwner pathOwner)
      {
        if (serviceDispatches.Length <= postVan.m_RequestCount)
          return;
        float num1 = -1f;
        Entity request1 = Entity.Null;
        PathElement pathElement1 = new PathElement();
        bool flag = false;
        int num2 = 0;
        if (postVan.m_RequestCount >= 1 && (postVan.m_State & PostVanFlags.Returning) == (PostVanFlags) 0)
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
        for (int index = num2; index < postVan.m_RequestCount; ++index)
        {
          DynamicBuffer<PathElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElements.TryGetBuffer(serviceDispatches[index].m_Request, out bufferData) && bufferData.Length != 0)
          {
            pathElement1 = bufferData[bufferData.Length - 1];
            flag = true;
          }
        }
        for (int requestCount = postVan.m_RequestCount; requestCount < serviceDispatches.Length; ++requestCount)
        {
          Entity request2 = serviceDispatches[requestCount].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PostVanRequestData.HasComponent(request2))
          {
            // ISSUE: reference to a compiler-generated field
            PostVanRequest postVanRequest = this.m_PostVanRequestData[request2];
            DynamicBuffer<PathElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (flag && this.m_PathElements.TryGetBuffer(request2, out bufferData) && bufferData.Length != 0)
            {
              PathElement pathElement3 = bufferData[0];
              if (pathElement3.m_Target != pathElement1.m_Target || (double) pathElement3.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(postVanRequest.m_Target) && (double) postVanRequest.m_Priority > (double) num1)
            {
              num1 = (float) postVanRequest.m_Priority;
              request1 = request2;
            }
          }
        }
        if (request1 != Entity.Null)
        {
          serviceDispatches[postVan.m_RequestCount++] = new ServiceDispatch(request1);
          if (postVan.m_DeliveringMail > 0 || postVan.m_CollectedMail < prefabPostVanData.m_MailCapacity)
          {
            // ISSUE: reference to a compiler-generated method
            this.PreAddDeliveryRequests(request1, ref postVan);
          }
        }
        if (serviceDispatches.Length <= postVan.m_RequestCount)
          return;
        serviceDispatches.RemoveRange(postVan.m_RequestCount, serviceDispatches.Length - postVan.m_RequestCount);
      }

      private void PreAddDeliveryRequests(Entity request, ref Game.Vehicles.PostVan postVan)
      {
        DynamicBuffer<PathElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PathElements.TryGetBuffer(request, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated method
        int dispatchIndex = this.BumpDispachIndex(request);
        Entity owner1 = Entity.Null;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          PathElement pathElement = bufferData[index];
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
                this.AddBuildingRequests(owner2.m_Owner, request, dispatchIndex, ref postVan.m_CollectEstimate, ref postVan.m_DeliveryEstimate);
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

      private void RequestTargetIfNeeded(int jobIndex, Entity entity, ref Game.Vehicles.PostVan postVan)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PostVanRequestData.HasComponent(postVan.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 16U) - 1) != 9)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PostVanRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PostVanRequest>(jobIndex, entity1, new PostVanRequest(entity, (PostVanRequestFlags) 0, (ushort) 1));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if ((postVan.m_State & PostVanFlags.Returning) == (PostVanFlags) 0 && postVan.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          --postVan.m_RequestCount;
        }
        for (; postVan.m_RequestCount > 0 && serviceDispatches.Length > 0; --postVan.m_RequestCount)
        {
          Entity request = serviceDispatches[0].m_Request;
          PostVanRequest postVanRequest = new PostVanRequest();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PostVanRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            postVanRequest = this.m_PostVanRequestData[request];
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(postVanRequest.m_Target))
          {
            serviceDispatches.RemoveAt(0);
            postVan.m_CollectEstimate -= postVan.m_CollectEstimate / postVan.m_RequestCount;
            postVan.m_DeliveryEstimate -= postVan.m_DeliveryEstimate / postVan.m_RequestCount;
          }
          else
          {
            postVan.m_State &= ~PostVanFlags.Returning;
            car.m_Flags |= CarFlags.UsePublicTransportLanes;
            if ((postVanRequest.m_Flags & PostVanRequestFlags.Deliver) != (PostVanRequestFlags) 0)
              postVan.m_State |= PostVanFlags.Delivering;
            else
              postVan.m_State &= ~PostVanFlags.Delivering;
            if ((postVanRequest.m_Flags & PostVanRequestFlags.Collect) != (PostVanRequestFlags) 0)
              postVan.m_State |= PostVanFlags.Collecting;
            else
              postVan.m_State &= ~PostVanFlags.Collecting;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(request, vehicleEntity, false, true));
            // ISSUE: reference to a compiler-generated field
            if (this.m_PostVanRequestData.HasComponent(postVan.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(postVan.m_TargetRequest, Entity.Null, true));
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
                float num = postVan.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                int appendedCount;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes, out appendedCount))
                {
                  if (postVan.m_DeliveringMail > 0 || postVan.m_CollectedMail < prefabPostVanData.m_MailCapacity)
                  {
                    // ISSUE: reference to a compiler-generated method
                    int dispatchIndex = this.BumpDispachIndex(request);
                    int index1 = pathElement2.Length - appendedCount;
                    int collectAmount = 0;
                    int deliveryAmount = 0;
                    for (int index2 = 0; index2 < index1; ++index2)
                    {
                      PathElement pathElement3 = pathElement2[index2];
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PedestrianLaneData.HasComponent(pathElement3.m_Target))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        this.AddBuildingRequests(this.m_OwnerData[pathElement3.m_Target].m_Owner, request, dispatchIndex, ref collectAmount, ref deliveryAmount);
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
                        this.AddPathElement(pathElement2, nativeArray[index4], request, dispatchIndex, ref lastOwner, ref collectAmount, ref deliveryAmount);
                      }
                      nativeArray.Dispose();
                    }
                    if (postVan.m_RequestCount == 1)
                    {
                      postVan.m_CollectEstimate = collectAmount;
                      postVan.m_DeliveryEstimate = deliveryAmount;
                    }
                  }
                  car.m_Flags |= CarFlags.StayOnRoad;
                  postVan.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  target.m_Target = postVanRequest.m_Target;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, postVanRequest.m_Target);
            return true;
          }
        }
        return false;
      }

      private void ReturnToFacility(
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.PostVan postVan,
        ref PathOwner pathOwnerData,
        ref Target targetData)
      {
        serviceDispatches.Clear();
        postVan.m_RequestCount = 0;
        postVan.m_CollectEstimate = 0;
        postVan.m_DeliveryEstimate = 0;
        postVan.m_State |= PostVanFlags.Returning;
        postVan.m_State &= ~(PostVanFlags.Delivering | PostVanFlags.Collecting);
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, ownerData.m_Owner);
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        PostVanData prefabPostVanData,
        ref Random random,
        ref Game.Vehicles.PostVan postVan,
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
        if ((postVan.m_State & PostVanFlags.Returning) == (PostVanFlags) 0 && postVan.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PostVanRequestData.HasComponent(request) && (postVan.m_DeliveringMail > 0 || postVan.m_CollectedMail < prefabPostVanData.m_MailCapacity))
          {
            NativeArray<PathElement> nativeArray = new NativeArray<PathElement>(pathElement.Length, Allocator.Temp);
            nativeArray.CopyFrom(pathElement.AsNativeArray());
            pathElement.Clear();
            // ISSUE: reference to a compiler-generated method
            int dispatchIndex = this.BumpDispachIndex(request);
            Entity lastOwner = Entity.Null;
            int collectAmount = 0;
            int deliveryAmount = 0;
            for (int index = 0; index < nativeArray.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddPathElement(pathElement, nativeArray[index], request, dispatchIndex, ref lastOwner, ref collectAmount, ref deliveryAmount);
            }
            nativeArray.Dispose();
            if (postVan.m_RequestCount == 1)
            {
              postVan.m_CollectEstimate = collectAmount;
              postVan.m_DeliveryEstimate = deliveryAmount;
            }
          }
          carData.m_Flags |= CarFlags.StayOnRoad;
        }
        else
          carData.m_Flags &= ~CarFlags.StayOnRoad;
        carData.m_Flags |= CarFlags.UsePublicTransportLanes;
        postVan.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement.Length);
      }

      private void AddPathElement(
        DynamicBuffer<PathElement> path,
        PathElement pathElement,
        Entity request,
        int dispatchIndex,
        ref Entity lastOwner,
        ref int collectAmount,
        ref int deliveryAmount)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.HasComponent(pathElement.m_Target))
        {
          path.Add(pathElement);
          lastOwner = Entity.Null;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Owner owner = this.m_OwnerData[pathElement.m_Target];
          if (owner.m_Owner == lastOwner)
          {
            path.Add(pathElement);
          }
          else
          {
            lastOwner = owner.m_Owner;
            float y = pathElement.m_TargetDelta.y;
            Entity sidewalk;
            // ISSUE: reference to a compiler-generated method
            if (this.FindClosestSidewalk(pathElement.m_Target, owner.m_Owner, ref y, out sidewalk))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddBuildingRequests(owner.m_Owner, request, dispatchIndex, ref collectAmount, ref deliveryAmount);
              path.Add(pathElement);
              path.Add(new PathElement(sidewalk, (float2) y));
            }
            else
              path.Add(pathElement);
          }
        }
      }

      private bool FindClosestSidewalk(
        Entity lane,
        Entity owner,
        ref float curvePos,
        out Entity sidewalk)
      {
        bool closestSidewalk = false;
        sidewalk = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = MathUtils.Position(this.m_CurveData[lane].m_Bezier, curvePos);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
          float num1 = float.MaxValue;
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane2))
            {
              float t;
              // ISSUE: reference to a compiler-generated field
              float num2 = MathUtils.Distance(MathUtils.Line(this.m_CurveData[subLane2].m_Bezier), position, out t);
              if ((double) num2 < (double) num1)
              {
                curvePos = t;
                sidewalk = subLane2;
                num1 = num2;
                closestSidewalk = true;
              }
            }
          }
        }
        return closestSidewalk;
      }

      private int BumpDispachIndex(Entity request)
      {
        int num = 0;
        PostVanRequest componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PostVanRequestData.TryGetComponent(request, out componentData))
        {
          num = (int) componentData.m_DispatchIndex + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PostVanAISystem.MailAction()
          {
            m_Type = PostVanAISystem.MailActionType.BumpDispatchIndex,
            m_Request = request
          });
        }
        return num;
      }

      private void AddBuildingRequests(
        Entity edgeEntity,
        Entity request,
        int dispatchIndex,
        ref int collectAmount,
        ref int deliveryAmount)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedBuildings.HasBuffer(edgeEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedBuilding> connectedBuilding = this.m_ConnectedBuildings[edgeEntity];
        for (int index = 0; index < connectedBuilding.Length; ++index)
        {
          Entity building = connectedBuilding[index].m_Building;
          MailProducer componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MailProducerData.TryGetComponent(building, out componentData))
          {
            collectAmount += (int) componentData.m_SendingMail;
            deliveryAmount += componentData.receivingMail;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new PostVanAISystem.MailAction()
            {
              m_Type = PostVanAISystem.MailActionType.AddRequest,
              m_Request = request,
              m_Target = building,
              m_DeliverAmount = dispatchIndex
            });
          }
        }
      }

      private void CheckBuildings(
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        ref CarCurrentLane currentLane,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        if ((postVan.m_State & PostVanFlags.ClearChecked) != (PostVanFlags) 0)
        {
          if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0)
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Checked;
          postVan.m_State &= ~PostVanFlags.ClearChecked;
        }
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.Waypoint | Game.Vehicles.CarLaneFlags.Checked)) == Game.Vehicles.CarLaneFlags.Waypoint)
        {
          // ISSUE: reference to a compiler-generated method
          if (postVan.m_DeliveringMail <= 0 && postVan.m_CollectedMail > prefabPostVanData.m_MailCapacity || !this.CheckBuildings(prefabPostVanData, ref postVan, currentLane.m_Lane))
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SlaveLaneData.HasComponent(currentLane.m_Lane))
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.FixedLane;
          }
          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
        }
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0)
          return;
        for (int index = 0; index < navigationLanes.Length; ++index)
        {
          ref CarNavigationLane local = ref navigationLanes.ElementAt(index);
          if ((local.m_Flags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Checked) == (Game.Vehicles.CarLaneFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (postVan.m_DeliveringMail <= 0 && postVan.m_CollectedMail > prefabPostVanData.m_MailCapacity || !this.CheckBuildings(prefabPostVanData, ref postVan, local.m_Lane))
            {
              local.m_Flags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SlaveLaneData.HasComponent(local.m_Lane))
                local.m_Flags &= ~Game.Vehicles.CarLaneFlags.FixedLane;
            }
            currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
          }
          if ((local.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.Waypoint)) != Game.Vehicles.CarLaneFlags.Reserved)
            break;
        }
      }

      private bool CheckBuildings(
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        Entity laneEntity)
      {
        Owner componentData1;
        DynamicBuffer<ConnectedBuilding> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeLaneData.HasComponent(laneEntity) && this.m_OwnerData.TryGetComponent(laneEntity, out componentData1) && this.m_ConnectedBuildings.TryGetBuffer(componentData1.m_Owner, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity building = bufferData[index].m_Building;
            MailProducer componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_MailProducerData.TryGetComponent(building, out componentData2) && (postVan.m_DeliveringMail > 0 && componentData2.receivingMail > 0 || postVan.m_CollectedMail < prefabPostVanData.m_MailCapacity && componentData2.m_SendingMail > (ushort) 0 && this.RequireCollect(this.m_PrefabRefData[building].m_Prefab)))
              return true;
          }
        }
        return false;
      }

      private void TryHandleBuildings(
        int jobIndex,
        Entity vehicleEntity,
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        ref CarCurrentLane currentLaneData,
        Entity ignoreBuilding)
      {
        if (postVan.m_DeliveringMail <= 0 && postVan.m_CollectedMail >= prefabPostVanData.m_MailCapacity)
          return;
        // ISSUE: reference to a compiler-generated method
        this.TryHandleBuildings(jobIndex, vehicleEntity, prefabPostVanData, ref postVan, currentLaneData.m_Lane, ignoreBuilding);
      }

      private void TryHandleBuildings(
        int jobIndex,
        Entity vehicleEntity,
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        Entity laneEntity,
        Entity ignoreBuilding)
      {
        Owner componentData;
        DynamicBuffer<ConnectedBuilding> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.HasComponent(laneEntity) || !this.m_OwnerData.TryGetComponent(laneEntity, out componentData) || !this.m_ConnectedBuildings.TryGetBuffer(componentData.m_Owner, out bufferData))
          return;
        bool flag = false;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity building = bufferData[index].m_Building;
          if (building != ignoreBuilding)
          {
            // ISSUE: reference to a compiler-generated method
            flag |= this.TryHandleBuilding(jobIndex, vehicleEntity, prefabPostVanData, ref postVan, building);
          }
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new PostVanAISystem.MailAction()
        {
          m_Type = PostVanAISystem.MailActionType.ClearLane,
          m_Vehicle = vehicleEntity,
          m_Target = laneEntity
        });
      }

      private bool TryHandleBuilding(
        int jobIndex,
        Entity vehicleEntity,
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        Entity building)
      {
        MailProducer componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MailProducerData.TryGetComponent(building, out componentData))
        {
          bool c1 = postVan.m_DeliveringMail > 0 && componentData.receivingMail > 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          bool c2 = postVan.m_CollectedMail < prefabPostVanData.m_MailCapacity && componentData.m_SendingMail > (ushort) 0 && this.RequireCollect(this.m_PrefabRefData[building].m_Prefab);
          if (c1 | c2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new PostVanAISystem.MailAction()
            {
              m_Type = PostVanAISystem.MailActionType.HandleBuilding,
              m_Vehicle = vehicleEntity,
              m_Target = building,
              m_DeliverAmount = math.select(0, prefabPostVanData.m_MailCapacity, c1),
              m_CollectAmount = math.select(0, prefabPostVanData.m_MailCapacity, c2)
            });
            // ISSUE: reference to a compiler-generated field
            if (c1 && componentData.receivingMail >= this.m_PostConfigurationData.m_MailAccumulationTolerance)
            {
              // ISSUE: reference to a compiler-generated method
              this.QuantityUpdated(jobIndex, building);
            }
            return true;
          }
        }
        return false;
      }

      private bool RequireCollect(Entity prefab)
      {
        SpawnableBuildingData componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnableBuildingData.TryGetComponent(prefab, out componentData1))
        {
          MailAccumulationData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MailAccumulationData.TryGetComponent(componentData1.m_ZonePrefab, out componentData2))
            return componentData2.m_RequireCollect;
        }
        else
        {
          ServiceObjectData componentData3;
          MailAccumulationData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceObjectData.TryGetComponent(prefab, out componentData3) && this.m_MailAccumulationData.TryGetComponent(componentData3.m_Service, out componentData4))
            return componentData4.m_RequireCollect;
        }
        return false;
      }

      private void QuantityUpdated(int jobIndex, Entity buildingEntity)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(buildingEntity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_QuantityData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, subObject, new BatchesUpdated());
          }
        }
      }

      private void TryHandleMailBox(
        Entity vehicleEntity,
        PostVanData prefabPostVanData,
        ref Game.Vehicles.PostVan postVan,
        ref Target targetData)
      {
        Game.Routes.MailBox componentData;
        // ISSUE: reference to a compiler-generated field
        if (postVan.m_CollectedMail >= prefabPostVanData.m_MailCapacity || !this.m_MailBoxData.TryGetComponent(targetData.m_Target, out componentData) || componentData.m_MailAmount <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new PostVanAISystem.MailAction()
        {
          m_Type = PostVanAISystem.MailActionType.HandleMailBox,
          m_Vehicle = vehicleEntity,
          m_Target = targetData.m_Target,
          m_CollectAmount = prefabPostVanData.m_MailCapacity
        });
      }

      private void UnloadMail(Entity vehicleEntity, Entity facility, ref Game.Vehicles.PostVan postVan)
      {
        // ISSUE: reference to a compiler-generated field
        if (postVan.m_DeliveringMail <= 0 && postVan.m_CollectedMail <= 0 || !this.m_PostFacilityData.HasComponent(facility))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new PostVanAISystem.MailAction()
        {
          m_Type = PostVanAISystem.MailActionType.UnloadAll,
          m_Vehicle = vehicleEntity,
          m_Target = facility
        });
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
    private struct MailActionJob : IJob
    {
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      public ComponentLookup<Game.Vehicles.PostVan> m_PostVanData;
      public ComponentLookup<PostVanRequest> m_PostVanRequestData;
      public ComponentLookup<MailProducer> m_MailProducerData;
      public ComponentLookup<Game.Routes.MailBox> m_MailBoxData;
      public BufferLookup<Resources> m_Resources;
      public NativeQueue<PostVanAISystem.MailAction> m_ActionQueue;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PostVanAISystem.MailAction mailAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out mailAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PostVanAISystem.MailActionType type = mailAction.m_Type;
          StatisticsEvent statisticsEvent1;
          switch (type)
          {
            case PostVanAISystem.MailActionType.AddRequest:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MailProducer mailProducer1 = this.m_MailProducerData[mailAction.m_Target] with
              {
                m_MailRequest = mailAction.m_Request,
                m_DispatchIndex = (byte) mailAction.m_DeliverAmount
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_MailProducerData[mailAction.m_Target] = mailProducer1;
              continue;
            case PostVanAISystem.MailActionType.HandleBuilding:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.PostVan postVan1 = this.m_PostVanData[mailAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MailProducer mailProducer2 = this.m_MailProducerData[mailAction.m_Target];
              // ISSUE: reference to a compiler-generated field
              int num1 = math.max(0, math.min(mailAction.m_DeliverAmount, math.min(postVan1.m_DeliveringMail, mailProducer2.receivingMail)));
              // ISSUE: reference to a compiler-generated field
              int num2 = math.max(0, math.min(mailAction.m_CollectAmount - postVan1.m_CollectedMail, (int) mailProducer2.m_SendingMail));
              if (num1 != 0 || num2 != 0)
              {
                postVan1.m_DeliveringMail -= num1;
                postVan1.m_CollectedMail += num2;
                postVan1.m_DeliveryEstimate = math.max(0, postVan1.m_DeliveryEstimate - num1);
                postVan1.m_CollectEstimate = math.max(0, postVan1.m_CollectEstimate - num2);
                mailProducer2.receivingMail -= num1;
                mailProducer2.mailDelivered |= num1 != 0;
                mailProducer2.m_SendingMail -= (ushort) num2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PostVanData[mailAction.m_Vehicle] = postVan1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_MailProducerData[mailAction.m_Target] = mailProducer2;
                if (num1 != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeQueue<StatisticsEvent>.ParallelWriter local = ref this.m_StatisticsEventQueue;
                  statisticsEvent1 = new StatisticsEvent();
                  statisticsEvent1.m_Statistic = StatisticType.DeliveredMail;
                  statisticsEvent1.m_Change = (float) num1;
                  StatisticsEvent statisticsEvent2 = statisticsEvent1;
                  local.Enqueue(statisticsEvent2);
                }
                if (num2 != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeQueue<StatisticsEvent>.ParallelWriter local = ref this.m_StatisticsEventQueue;
                  statisticsEvent1 = new StatisticsEvent();
                  statisticsEvent1.m_Statistic = StatisticType.CollectedMail;
                  statisticsEvent1.m_Change = (float) num2;
                  StatisticsEvent statisticsEvent3 = statisticsEvent1;
                  local.Enqueue(statisticsEvent3);
                  continue;
                }
                continue;
              }
              continue;
            case PostVanAISystem.MailActionType.HandleMailBox:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.PostVan postVan2 = this.m_PostVanData[mailAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Routes.MailBox mailBox = this.m_MailBoxData[mailAction.m_Target];
              // ISSUE: reference to a compiler-generated field
              int num3 = math.min(mailAction.m_CollectAmount - postVan2.m_CollectedMail, mailBox.m_MailAmount);
              if (num3 > 0)
              {
                postVan2.m_CollectedMail += num3;
                mailBox.m_MailAmount -= num3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_PostVanData[mailAction.m_Vehicle] = postVan2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_MailBoxData[mailAction.m_Target] = mailBox;
                // ISSUE: reference to a compiler-generated field
                ref NativeQueue<StatisticsEvent>.ParallelWriter local = ref this.m_StatisticsEventQueue;
                statisticsEvent1 = new StatisticsEvent();
                statisticsEvent1.m_Statistic = StatisticType.CollectedMail;
                statisticsEvent1.m_Change = (float) num3;
                StatisticsEvent statisticsEvent4 = statisticsEvent1;
                local.Enqueue(statisticsEvent4);
                continue;
              }
              continue;
            case PostVanAISystem.MailActionType.UnloadAll:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.PostVan postVan3 = this.m_PostVanData[mailAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Resources> resource = this.m_Resources[mailAction.m_Target];
              if (postVan3.m_DeliveringMail > 0)
              {
                EconomyUtils.AddResources(Resource.LocalMail, postVan3.m_DeliveringMail, resource);
                postVan3.m_DeliveringMail = 0;
              }
              if (postVan3.m_CollectedMail > 0)
              {
                EconomyUtils.AddResources(Resource.UnsortedMail, postVan3.m_CollectedMail, resource);
                postVan3.m_CollectedMail = 0;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_PostVanData[mailAction.m_Vehicle] = postVan3;
              continue;
            case PostVanAISystem.MailActionType.ClearLane:
              DynamicBuffer<LaneObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneObjects.TryGetBuffer(mailAction.m_Target, out bufferData))
              {
                for (int index = 0; index < bufferData.Length; ++index)
                {
                  Entity laneObject = bufferData[index].m_LaneObject;
                  Game.Vehicles.PostVan componentData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (laneObject != mailAction.m_Vehicle && this.m_PostVanData.TryGetComponent(laneObject, out componentData))
                  {
                    componentData.m_State |= PostVanFlags.ClearChecked;
                    // ISSUE: reference to a compiler-generated field
                    this.m_PostVanData[laneObject] = componentData;
                  }
                }
                continue;
              }
              continue;
            case PostVanAISystem.MailActionType.BumpDispatchIndex:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PostVanRequest postVanRequest = this.m_PostVanRequestData[mailAction.m_Request];
              ++postVanRequest.m_DispatchIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_PostVanRequestData[mailAction.m_Request] = postVanRequest;
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
      public ComponentTypeHandle<Game.Vehicles.PostVan> __Game_Vehicles_PostVan_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
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
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
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
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostVanData> __Game_Prefabs_PostVanData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> __Game_Prefabs_MailAccumulationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> __Game_Simulation_PostVanRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PostFacility> __Game_Buildings_PostFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.MailBox> __Game_Routes_MailBox_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public ComponentLookup<Game.Vehicles.PostVan> __Game_Vehicles_PostVan_RW_ComponentLookup;
      public ComponentLookup<PostVanRequest> __Game_Simulation_PostVanRequest_RW_ComponentLookup;
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RW_ComponentLookup;
      public ComponentLookup<Game.Routes.MailBox> __Game_Routes_MailBox_RW_ComponentLookup;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;

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
        this.__Game_Vehicles_PostVan_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PostVan>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
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
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
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
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PostVanData_RO_ComponentLookup = state.GetComponentLookup<PostVanData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup = state.GetComponentLookup<MailAccumulationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PostVanRequest_RO_ComponentLookup = state.GetComponentLookup<PostVanRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RO_ComponentLookup = state.GetComponentLookup<MailProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PostFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.PostFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_MailBox_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.MailBox>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PostVan_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PostVan>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PostVanRequest_RW_ComponentLookup = state.GetComponentLookup<PostVanRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RW_ComponentLookup = state.GetComponentLookup<MailProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_MailBox_RW_ComponentLookup = state.GetComponentLookup<Game.Routes.MailBox>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
      }
    }
  }
}
