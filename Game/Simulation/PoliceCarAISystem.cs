// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PoliceCarAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Events;
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
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PoliceCarAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_PolicePatrolRequestArchetype;
    private EntityArchetype m_PoliceEmergencyRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private PoliceCarAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 5;

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
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.PoliceCar>(), ComponentType.ReadWrite<Car>(), ComponentType.ReadWrite<Game.Common.Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicePatrolRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PolicePatrolRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceEmergencyRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PoliceEmergencyRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<PoliceCarAISystem.PoliceAction> nativeQueue = new NativeQueue<PoliceCarAISystem.PoliceAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_PoliceCarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      PoliceCarAISystem.PoliceCarTickJob jobData1 = new PoliceCarAISystem.PoliceCarTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_PoliceCarType = this.__TypeHandle.__Game_Vehicles_PoliceCar_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabPoliceCarData = this.__TypeHandle.__Game_Prefabs_PoliceCarData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_PolicePatrolRequestData = this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup,
        m_PoliceEmergencyRequestData = this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup,
        m_CrimeProducerData = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup,
        m_PoliceStationData = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_PolicePatrolRequestArchetype = this.m_PolicePatrolRequestArchetype,
        m_PoliceEmergencyRequestArchetype = this.m_PoliceEmergencyRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PoliceCarAISystem.PoliceActionJob jobData2 = new PoliceCarAISystem.PoliceActionJob()
      {
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_PolicePatrolRequestData = this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RW_ComponentLookup,
        m_CrimeProducerData = this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<PoliceCarAISystem.PoliceCarTickJob>(this.m_VehicleQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<PoliceCarAISystem.PoliceActionJob>(dependsOn);
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
    public PoliceCarAISystem()
    {
    }

    private struct PoliceAction
    {
      public PoliceCarAISystem.PoliceActionType m_Type;
      public Entity m_Target;
      public Entity m_Request;
      public float m_CrimeReductionRate;
      public int m_DispatchIndex;
    }

    private enum PoliceActionType
    {
      ReduceCrime,
      AddPatrolRequest,
      SecureAccidentSite,
      BumpDispatchIndex,
    }

    [BurstCompile]
    private struct PoliceCarTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> m_PoliceCarType;
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
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
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
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<PoliceCarData> m_PrefabPoliceCarData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> m_PolicePatrolRequestData;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> m_PoliceEmergencyRequestData;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> m_CrimeProducerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> m_PoliceStationData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_PolicePatrolRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_PoliceEmergencyRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<PoliceCarAISystem.PoliceAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathInformation> nativeArray5 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray6 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.PoliceCar> nativeArray7 = chunk.GetNativeArray<Game.Vehicles.PoliceCar>(ref this.m_PoliceCarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray8 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray9 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
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
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          Game.Objects.Transform transform = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          PathInformation pathInformation = nativeArray5[index];
          Game.Vehicles.PoliceCar policeCar = nativeArray7[index];
          Car car = nativeArray8[index];
          CarCurrentLane currentLane = nativeArray6[index];
          PathOwner pathOwner = nativeArray10[index];
          Game.Common.Target target = nativeArray9[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          DynamicBuffer<Passenger> passengers = new DynamicBuffer<Passenger>();
          if (bufferAccessor1.Length != 0)
            passengers = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, transform, prefabRef, pathInformation, passengers, navigationLanes, serviceDispatches, isStopped, ref random, ref policeCar, ref car, ref currentLane, ref pathOwner, ref target);
          nativeArray7[index] = policeCar;
          nativeArray8[index] = car;
          nativeArray6[index] = currentLane;
          nativeArray10[index] = pathOwner;
          nativeArray9[index] = target;
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        Game.Objects.Transform transform,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isStopped,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        PoliceCarData prefabPoliceCarData = this.m_PrefabPoliceCarData[prefabRef.m_Prefab];
        policeCar.m_EstimatedShift = math.select(policeCar.m_EstimatedShift - 1U, 0U, policeCar.m_EstimatedShift == 0U);
        if (++policeCar.m_ShiftTime >= prefabPoliceCarData.m_ShiftDuration)
          policeCar.m_State |= PoliceCarFlags.ShiftEnded;
        if ((car.m_Flags & CarFlags.Emergency) == (CarFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryReduceCrime(vehicleEntity, prefabPoliceCarData, ref currentLane, navigationLanes);
        }
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, ref random, pathInformation, serviceDispatches, ref policeCar, ref car, ref currentLane, ref pathOwner);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if (VehicleUtils.IsStuck(pathOwner) || (policeCar.m_State & PoliceCarFlags.Returning) != (PoliceCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToStation(jobIndex, vehicleEntity, owner, serviceDispatches, ref policeCar, ref car, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane) || VehicleUtils.ParkingSpaceReached(currentLane, pathOwner) || (policeCar.m_State & (PoliceCarFlags.AtTarget | PoliceCarFlags.Disembarking)) != (PoliceCarFlags) 0)
        {
          if ((policeCar.m_State & PoliceCarFlags.Returning) != (PoliceCarFlags) 0)
          {
            if ((policeCar.m_State & PoliceCarFlags.Disembarking) != (PoliceCarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.StopDisembarking(passengers, ref policeCar))
                return;
              if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
              {
                // ISSUE: reference to a compiler-generated method
                this.ParkCar(jobIndex, vehicleEntity, owner, ref policeCar, ref car, ref currentLane);
                return;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
              return;
            }
            // ISSUE: reference to a compiler-generated method
            if (this.StartDisembarking(passengers, ref policeCar))
              return;
            if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
            {
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, vehicleEntity, owner, ref policeCar, ref car, ref currentLane);
              return;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          bool flag = true;
          if ((policeCar.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            flag &= this.SecureAccidentSite(jobIndex, vehicleEntity, isStopped, ref policeCar, ref currentLane, passengers, serviceDispatches);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.TryReduceCrime(vehicleEntity, prefabPoliceCarData, ref target);
          }
          if (flag)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckServiceDispatches(vehicleEntity, serviceDispatches, passengers, ref policeCar, ref pathOwner);
            // ISSUE: reference to a compiler-generated method
            if ((policeCar.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Disabled)) != (PoliceCarFlags) 0 || !this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, passengers, ref policeCar, ref car, ref currentLane, ref pathOwner, ref target))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToStation(jobIndex, vehicleEntity, owner, serviceDispatches, ref policeCar, ref car, ref pathOwner, ref target);
            }
          }
        }
        else if (isStopped)
        {
          // ISSUE: reference to a compiler-generated method
          this.StartVehicle(jobIndex, vehicleEntity, ref currentLane);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          if ((policeCar.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.IsBlocked) != (Game.Vehicles.CarLaneFlags) 0 && this.IsCloseEnough(transform, ref target))
          {
            // ISSUE: reference to a compiler-generated method
            this.EndNavigation(vehicleEntity, ref currentLane, ref pathOwner, navigationLanes);
          }
        }
        if (policeCar.m_ShiftTime + policeCar.m_EstimatedShift >= prefabPoliceCarData.m_ShiftDuration)
          policeCar.m_State |= PoliceCarFlags.EstimatedShiftEnd;
        else
          policeCar.m_State &= ~PoliceCarFlags.EstimatedShiftEnd;
        if (passengers.Length >= prefabPoliceCarData.m_CriminalCapacity)
          policeCar.m_State |= PoliceCarFlags.Full;
        else
          policeCar.m_State &= ~PoliceCarFlags.Full;
        if (passengers.Length == 0)
          policeCar.m_State |= PoliceCarFlags.Empty;
        else
          policeCar.m_State &= ~PoliceCarFlags.Empty;
        if ((car.m_Flags & CarFlags.Emergency) == (CarFlags) 0 && (policeCar.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Disabled)) != (PoliceCarFlags) 0)
        {
          if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToStation(jobIndex, vehicleEntity, owner, serviceDispatches, ref policeCar, ref car, ref pathOwner, ref target);
          }
          serviceDispatches.Clear();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, passengers, ref policeCar, ref pathOwner);
          if ((policeCar.m_State & (PoliceCarFlags.Returning | PoliceCarFlags.Cancelled)) != (PoliceCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, passengers, ref policeCar, ref car, ref currentLane, ref pathOwner, ref target);
          }
          if (policeCar.m_RequestCount <= 1 && (policeCar.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Disabled)) == (PoliceCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref policeCar);
          }
        }
        if ((policeCar.m_State & (PoliceCarFlags.AtTarget | PoliceCarFlags.Disembarking)) != (PoliceCarFlags) 0)
          return;
        if (VehicleUtils.RequireNewPath(pathOwner))
        {
          if (isStopped && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ParkingSpace) == (Game.Vehicles.CarLaneFlags) 0)
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.FindNewPath(vehicleEntity, prefabRef, ref policeCar, ref car, ref currentLane, ref pathOwner, ref target);
          }
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
        ref Game.Vehicles.PoliceCar policeCar,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        car.m_Flags &= ~CarFlags.Emergency;
        policeCar.m_State = PoliceCarFlags.Empty;
        Game.Buildings.PoliceStation componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PoliceStationData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & PoliceStationFlags.HasAvailablePatrolCars) == (PoliceStationFlags) 0)
          policeCar.m_State |= PoliceCarFlags.Disabled;
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

      private bool StartDisembarking(DynamicBuffer<Passenger> passengers, ref Game.Vehicles.PoliceCar policeCar)
      {
        if (!passengers.IsCreated || passengers.Length <= 0)
          return false;
        policeCar.m_State |= PoliceCarFlags.Disembarking;
        return true;
      }

      private bool StopDisembarking(DynamicBuffer<Passenger> passengers, ref Game.Vehicles.PoliceCar policeCar)
      {
        if (passengers.IsCreated && passengers.Length > 0)
          return false;
        policeCar.m_State &= ~PoliceCarFlags.Disembarking;
        return true;
      }

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Car car,
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
        if ((policeCar.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
        {
          destination.m_Type = SetupTargetType.AccidentLocation;
          destination.m_Value2 = 30f;
        }
        if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
        {
          parameters.m_Weights = new PathfindWeights(1f, 0.0f, 0.0f, 0.0f);
        }
        else
        {
          parameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
          destination.m_Methods |= PathMethod.SpecialParking;
          destination.m_RandomCost = 30f;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private bool SecureAccidentSite(
        int jobIndex,
        Entity entity,
        bool isStopped,
        ref Game.Vehicles.PoliceCar policeCar,
        ref CarCurrentLane currentLaneData,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<ServiceDispatch> serviceDispatches)
      {
        if (policeCar.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PoliceEmergencyRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            PoliceEmergencyRequest emergencyRequest = this.m_PoliceEmergencyRequestData[request];
            // ISSUE: reference to a compiler-generated field
            if (this.m_AccidentSiteData.HasComponent(emergencyRequest.m_Site))
            {
              policeCar.m_State |= PoliceCarFlags.AtTarget;
              if (!isStopped)
              {
                // ISSUE: reference to a compiler-generated method
                this.StopVehicle(jobIndex, entity, ref currentLaneData);
              }
              // ISSUE: reference to a compiler-generated field
              if ((this.m_AccidentSiteData[emergencyRequest.m_Site].m_Flags & AccidentSiteFlags.Secured) == (AccidentSiteFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_ActionQueue.Enqueue(new PoliceCarAISystem.PoliceAction()
                {
                  m_Type = PoliceCarAISystem.PoliceActionType.SecureAccidentSite,
                  m_Target = emergencyRequest.m_Site
                });
              }
              return false;
            }
          }
        }
        if (passengers.IsCreated)
        {
          for (int index = 0; index < passengers.Length; ++index)
          {
            Entity passenger = passengers[index].m_Passenger;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentVehicleData.HasComponent(passenger) && (this.m_CurrentVehicleData[passenger].m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
              return false;
          }
        }
        return true;
      }

      private void EndNavigation(
        Entity vehicleEntity,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.EndOfPath;
        navigationLanes.Clear();
        pathOwner.m_ElementIndex = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_PathElements[vehicleEntity].Clear();
      }

      private bool IsCloseEnough(Game.Objects.Transform transform, ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform1 = this.m_TransformData[target.m_Target];
          return (double) math.distance(transform.m_Position, transform1.m_Position) <= 30.0;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AccidentSiteData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          AccidentSite accidentSite = this.m_AccidentSiteData[target.m_Target];
          // ISSUE: reference to a compiler-generated field
          if (this.m_TargetElements.HasBuffer(accidentSite.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[accidentSite.m_Event];
            for (int index = 0; index < targetElement.Length; ++index)
            {
              Entity entity = targetElement[index].m_Entity;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_InvolvedInAccidentData.HasComponent(entity) && this.m_TransformData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                InvolvedInAccident involvedInAccident = this.m_InvolvedInAccidentData[entity];
                // ISSUE: reference to a compiler-generated field
                Game.Objects.Transform transform2 = this.m_TransformData[entity];
                if (involvedInAccident.m_Event == accidentSite.m_Event && (double) math.distance(transform.m_Position, transform2.m_Position) <= 30.0)
                  return true;
              }
            }
          }
        }
        return false;
      }

      private void StopVehicle(int jobIndex, Entity entity, ref CarCurrentLane currentLaneData)
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
        if (!this.m_CarLaneData.HasComponent(currentLaneData.m_ChangeLane))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_ChangeLane, new PathfindUpdated());
      }

      private void StartVehicle(int jobIndex, Entity entity, ref CarCurrentLane currentLaneData)
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
        if (!this.m_CarLaneData.HasComponent(currentLaneData.m_ChangeLane))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_ChangeLane, new PathfindUpdated());
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.PoliceCar policeCar,
        ref PathOwner pathOwner)
      {
        if (serviceDispatches.Length <= policeCar.m_RequestCount)
          return;
        float num1 = -1f;
        Entity entity = Entity.Null;
        PathElement pathElement1 = new PathElement();
        PathElement pathElement2 = new PathElement();
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        int num2 = 0;
        if (policeCar.m_RequestCount >= 1 && (policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement3 = this.m_PathElements[vehicleEntity];
          num2 = 1;
          if (pathOwner.m_ElementIndex < pathElement3.Length)
          {
            pathElement1 = pathElement3[pathElement3.Length - 1];
            flag1 = true;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(serviceDispatches[0].m_Request))
            {
              pathElement2 = pathElement1;
              flag2 = true;
            }
          }
        }
        for (int index = num2; index < policeCar.m_RequestCount; ++index)
        {
          Entity request = serviceDispatches[index].m_Request;
          DynamicBuffer<PathElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
          {
            pathElement1 = bufferData[bufferData.Length - 1];
            flag1 = true;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(request))
            {
              pathElement2 = pathElement1;
              flag2 = true;
            }
          }
        }
        for (int requestCount = policeCar.m_RequestCount; requestCount < serviceDispatches.Length; ++requestCount)
        {
          Entity request = serviceDispatches[requestCount].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PolicePatrolRequestData.HasComponent(request))
          {
            if (!passengers.IsCreated || passengers.Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              PolicePatrolRequest policePatrolRequest = this.m_PolicePatrolRequestData[request];
              DynamicBuffer<PathElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (flag1 && this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
              {
                PathElement pathElement4 = bufferData[0];
                if (pathElement4.m_Target != pathElement1.m_Target || (double) pathElement4.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(policePatrolRequest.m_Target) && !flag3 && (double) policePatrolRequest.m_Priority > (double) num1)
              {
                num1 = policePatrolRequest.m_Priority;
                entity = request;
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated field
              PoliceEmergencyRequest emergencyRequest = this.m_PoliceEmergencyRequestData[request];
              DynamicBuffer<PathElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (flag2 && this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
              {
                PathElement pathElement5 = bufferData[0];
                if (pathElement5.m_Target != pathElement2.m_Target || (double) pathElement5.m_TargetDelta.x != (double) pathElement2.m_TargetDelta.y)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(emergencyRequest.m_Site) && (!flag3 || (double) emergencyRequest.m_Priority > (double) num1))
              {
                num1 = emergencyRequest.m_Priority;
                entity = request;
                flag3 = true;
              }
            }
          }
        }
        if (flag3)
        {
          int index1 = 0;
          for (int index2 = 0; index2 < policeCar.m_RequestCount; ++index2)
          {
            ServiceDispatch serviceDispatch = serviceDispatches[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(serviceDispatch.m_Request))
              serviceDispatches[index1++] = serviceDispatch;
            else if (index2 == 0 && (policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
            {
              serviceDispatches[index1++] = serviceDispatch;
              policeCar.m_State |= PoliceCarFlags.Cancelled;
              PathInformation componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathInformationData.TryGetComponent(serviceDispatch.m_Request, out componentData))
              {
                uint num3 = (uint) Mathf.RoundToInt(componentData.m_Duration * 3.75f);
                policeCar.m_EstimatedShift = math.select(policeCar.m_EstimatedShift - num3, 0U, num3 >= policeCar.m_EstimatedShift);
              }
            }
          }
          if (index1 < policeCar.m_RequestCount)
          {
            serviceDispatches.RemoveRange(index1, policeCar.m_RequestCount - index1);
            policeCar.m_RequestCount = index1;
          }
        }
        if (entity != Entity.Null)
        {
          serviceDispatches[policeCar.m_RequestCount++] = new ServiceDispatch(entity);
          if (!flag3)
          {
            // ISSUE: reference to a compiler-generated method
            this.PreAddPatrolRequests(entity);
          }
          PathInformation componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformationData.TryGetComponent(entity, out componentData))
            policeCar.m_EstimatedShift += (uint) Mathf.RoundToInt(componentData.m_Duration * 3.75f);
        }
        if (serviceDispatches.Length <= policeCar.m_RequestCount)
          return;
        serviceDispatches.RemoveRange(policeCar.m_RequestCount, serviceDispatches.Length - policeCar.m_RequestCount);
      }

      private int BumpDispachIndex(Entity request)
      {
        int num = 0;
        PolicePatrolRequest componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PolicePatrolRequestData.TryGetComponent(request, out componentData))
        {
          num = (int) componentData.m_DispatchIndex + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PoliceCarAISystem.PoliceAction()
          {
            m_Type = PoliceCarAISystem.PoliceActionType.BumpDispatchIndex,
            m_Request = request
          });
        }
        return num;
      }

      private void PreAddPatrolRequests(Entity request)
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
              this.AddPatrolRequests(owner2.m_Owner, request, dispatchIndex);
            }
          }
        }
      }

      private void RequestTargetIfNeeded(int jobIndex, Entity entity, ref Game.Vehicles.PoliceCar policeCar)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(policeCar.m_TargetRequest))
          return;
        if ((policeCar.m_PurposeMask & PolicePurpose.Patrol) != (PolicePurpose) 0 && (policeCar.m_State & (PoliceCarFlags.Empty | PoliceCarFlags.EstimatedShiftEnd)) == PoliceCarFlags.Empty)
        {
          // ISSUE: reference to a compiler-generated field
          if (((int) this.m_SimulationFrameIndex & (int) math.max(512U, 16U) - 1) != 5)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PolicePatrolRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PolicePatrolRequest>(jobIndex, entity1, new PolicePatrolRequest(entity, 1f));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((policeCar.m_PurposeMask & (PolicePurpose.Emergency | PolicePurpose.Intelligence)) == (PolicePurpose) 0 || ((int) this.m_SimulationFrameIndex & (int) math.max(64U, 16U) - 1) != 5)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PoliceEmergencyRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity2, new ServiceRequest(true));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PoliceEmergencyRequest>(jobIndex, entity2, new PoliceEmergencyRequest(entity, Entity.Null, 1f, policeCar.m_PurposeMask & (PolicePurpose.Emergency | PolicePurpose.Intelligence)));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity2, new RequestGroup(4U));
        }
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0 && policeCar.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          --policeCar.m_RequestCount;
        }
        for (; policeCar.m_RequestCount > 0 && serviceDispatches.Length > 0; --policeCar.m_RequestCount)
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity entity1 = Entity.Null;
          PoliceCarFlags policeCarFlags = (PoliceCarFlags) 0;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PolicePatrolRequestData.HasComponent(request))
          {
            if (!passengers.IsCreated || passengers.Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_PolicePatrolRequestData[request].m_Target;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_PoliceEmergencyRequestData[request].m_Site;
              policeCarFlags |= PoliceCarFlags.AccidentTarget;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(entity1))
          {
            serviceDispatches.RemoveAt(0);
            policeCar.m_EstimatedShift -= policeCar.m_EstimatedShift / (uint) policeCar.m_RequestCount;
          }
          else
          {
            policeCar.m_State &= ~(PoliceCarFlags.Returning | PoliceCarFlags.AccidentTarget | PoliceCarFlags.AtTarget | PoliceCarFlags.Cancelled);
            policeCar.m_State |= policeCarFlags;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(request, vehicleEntity, false, true));
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceRequestData.HasComponent(policeCar.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(policeCar.m_TargetRequest, Entity.Null, true));
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
                float num = policeCar.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes))
                {
                  if ((policeCarFlags & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
                  {
                    car.m_Flags &= ~CarFlags.AnyLaneTarget;
                    car.m_Flags |= CarFlags.Emergency | CarFlags.StayOnRoad | CarFlags.UsePublicTransportLanes;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    int dispatchIndex = this.BumpDispachIndex(request);
                    Entity owner1 = Entity.Null;
                    for (int index = 0; index < pathElement2.Length; ++index)
                    {
                      PathElement pathElement3 = pathElement2[index];
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_EdgeLaneData.HasComponent(pathElement3.m_Target))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Owner owner2 = this.m_OwnerData[pathElement3.m_Target];
                        if (owner2.m_Owner != owner1)
                        {
                          owner1 = owner2.m_Owner;
                          // ISSUE: reference to a compiler-generated method
                          this.AddPatrolRequests(owner2.m_Owner, request, dispatchIndex);
                        }
                      }
                    }
                    car.m_Flags &= ~CarFlags.Emergency;
                    car.m_Flags |= CarFlags.StayOnRoad | CarFlags.AnyLaneTarget | CarFlags.UsePublicTransportLanes;
                  }
                  if (policeCar.m_RequestCount == 1)
                    policeCar.m_EstimatedShift = (uint) Mathf.RoundToInt(num * 3.75f);
                  policeCar.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  target.m_Target = entity1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity);
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

      private void ReturnToStation(
        int jobIndex,
        Entity vehicleEntity,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Car carData,
        ref PathOwner pathOwnerData,
        ref Game.Common.Target targetData)
      {
        serviceDispatches.Clear();
        policeCar.m_RequestCount = 0;
        policeCar.m_EstimatedShift = 0U;
        policeCar.m_State &= ~(PoliceCarFlags.AccidentTarget | PoliceCarFlags.AtTarget | PoliceCarFlags.Cancelled);
        policeCar.m_State |= PoliceCarFlags.Returning;
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, ownerData.m_Owner);
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        ref Unity.Mathematics.Random random,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Car carData,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[vehicleEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathUtils.ResetPath(ref currentLane, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.ResetParkingLaneStatus(vehicleEntity, ref currentLane, ref pathOwner, pathElement1, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetParkingCurvePos(vehicleEntity, ref random, currentLane, pathOwner, pathElement1, ref this.m_ParkedCarData, ref this.m_UnspawnedData, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, ref this.m_PrefabParkingLaneData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false);
        if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0 && policeCar.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PolicePatrolRequestData.HasComponent(request))
          {
            Entity owner1 = Entity.Null;
            // ISSUE: reference to a compiler-generated method
            int dispatchIndex = this.BumpDispachIndex(request);
            for (int index = 0; index < pathElement1.Length; ++index)
            {
              PathElement pathElement2 = pathElement1[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_EdgeLaneData.HasComponent(pathElement2.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                Owner owner2 = this.m_OwnerData[pathElement2.m_Target];
                if (owner2.m_Owner != owner1)
                {
                  owner1 = owner2.m_Owner;
                  // ISSUE: reference to a compiler-generated method
                  this.AddPatrolRequests(owner2.m_Owner, request, dispatchIndex);
                }
              }
            }
            carData.m_Flags &= ~CarFlags.Emergency;
            carData.m_Flags |= CarFlags.StayOnRoad | CarFlags.AnyLaneTarget;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(request))
            {
              carData.m_Flags &= ~CarFlags.AnyLaneTarget;
              carData.m_Flags |= CarFlags.Emergency | CarFlags.StayOnRoad;
            }
            else
            {
              carData.m_Flags &= ~(CarFlags.Emergency | CarFlags.AnyLaneTarget);
              carData.m_Flags |= CarFlags.StayOnRoad;
            }
          }
          if (policeCar.m_RequestCount == 1)
            policeCar.m_EstimatedShift = (uint) Mathf.RoundToInt(pathInformation.m_Duration * 3.75f);
        }
        else
          carData.m_Flags &= ~(CarFlags.Emergency | CarFlags.StayOnRoad | CarFlags.AnyLaneTarget);
        carData.m_Flags |= CarFlags.UsePublicTransportLanes;
        policeCar.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement1.Length);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity);
      }

      private void AddPatrolRequests(Entity edgeEntity, Entity request, int dispatchIndex)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedBuildings.HasBuffer(edgeEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedBuilding> connectedBuilding = this.m_ConnectedBuildings[edgeEntity];
        for (int index = 0; index < connectedBuilding.Length; ++index)
        {
          Entity building = connectedBuilding[index].m_Building;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CrimeProducerData.HasComponent(building))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ActionQueue.Enqueue(new PoliceCarAISystem.PoliceAction()
            {
              m_Type = PoliceCarAISystem.PoliceActionType.AddPatrolRequest,
              m_Target = building,
              m_Request = request,
              m_DispatchIndex = dispatchIndex
            });
          }
        }
      }

      private void TryReduceCrime(
        Entity vehicleEntity,
        PoliceCarData prefabPoliceCarData,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated method
        this.TryReduceCrime(target.m_Target, prefabPoliceCarData.m_CrimeReductionRate);
      }

      private void TryReduceCrime(Entity building, float reduction)
      {
        CrimeProducer componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CrimeProducerData.TryGetComponent(building, out componentData) || (double) componentData.m_Crime <= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new PoliceCarAISystem.PoliceAction()
        {
          m_Type = PoliceCarAISystem.PoliceActionType.ReduceCrime,
          m_Target = building,
          m_CrimeReductionRate = reduction
        });
      }

      private void TryReduceCrime(
        Entity vehicleEntity,
        PoliceCarData prefabPoliceCarData,
        ref CarCurrentLane currentLane,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        EdgeLane componentData1;
        Owner componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.TryGetComponent(currentLane.m_Lane, out componentData1) || !this.m_OwnerData.TryGetComponent(currentLane.m_Lane, out componentData2))
          return;
        for (int index = 0; index < navigationLanes.Length; ++index)
        {
          CarNavigationLane navigationLane = navigationLanes[index];
          Owner componentData3;
          // ISSUE: reference to a compiler-generated field
          if ((navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Checked) == (Game.Vehicles.CarLaneFlags) 0 && this.m_OwnerData.TryGetComponent(navigationLane.m_Lane, out componentData3) && !(componentData3.m_Owner != componentData2.m_Owner))
          {
            navigationLane.m_Flags |= Game.Vehicles.CarLaneFlags.Checked;
            navigationLanes[index] = navigationLane;
          }
          else
            break;
        }
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Checked) != (Game.Vehicles.CarLaneFlags) 0)
          return;
        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
        float reduction = prefabPoliceCarData.m_CrimeReductionRate * math.abs(componentData1.m_EdgeDelta.y - componentData1.m_EdgeDelta.x);
        DynamicBuffer<ConnectedBuilding> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedBuildings.TryGetBuffer(componentData2.m_Owner, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryReduceCrime(bufferData[index].m_Building, reduction);
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
    private struct PoliceActionJob : IJob
    {
      [ReadOnly]
      public uint m_SimulationFrame;
      public ComponentLookup<PolicePatrolRequest> m_PolicePatrolRequestData;
      public ComponentLookup<CrimeProducer> m_CrimeProducerData;
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      public NativeQueue<PoliceCarAISystem.PoliceAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PoliceCarAISystem.PoliceAction policeAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out policeAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PoliceCarAISystem.PoliceActionType type = policeAction.m_Type;
          switch (type)
          {
            case PoliceCarAISystem.PoliceActionType.ReduceCrime:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              CrimeProducer crimeProducer1 = this.m_CrimeProducerData[policeAction.m_Target];
              // ISSUE: reference to a compiler-generated field
              float num = math.min(policeAction.m_CrimeReductionRate, crimeProducer1.m_Crime);
              if ((double) num > 0.0)
              {
                crimeProducer1.m_Crime -= num;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CrimeProducerData[policeAction.m_Target] = crimeProducer1;
                continue;
              }
              continue;
            case PoliceCarAISystem.PoliceActionType.AddPatrolRequest:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              CrimeProducer crimeProducer2 = this.m_CrimeProducerData[policeAction.m_Target] with
              {
                m_PatrolRequest = policeAction.m_Request,
                m_DispatchIndex = (byte) policeAction.m_DispatchIndex
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CrimeProducerData[policeAction.m_Target] = crimeProducer2;
              continue;
            case PoliceCarAISystem.PoliceActionType.SecureAccidentSite:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              AccidentSite accidentSite = this.m_AccidentSiteData[policeAction.m_Target];
              if ((accidentSite.m_Flags & AccidentSiteFlags.Secured) == (AccidentSiteFlags) 0)
              {
                accidentSite.m_Flags |= AccidentSiteFlags.Secured;
                // ISSUE: reference to a compiler-generated field
                accidentSite.m_SecuredFrame = this.m_SimulationFrame;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_AccidentSiteData[policeAction.m_Target] = accidentSite;
              continue;
            case PoliceCarAISystem.PoliceActionType.BumpDispatchIndex:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PolicePatrolRequest policePatrolRequest = this.m_PolicePatrolRequestData[policeAction.m_Request];
              ++policePatrolRequest.m_DispatchIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_PolicePatrolRequestData[policeAction.m_Request] = policePatrolRequest;
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> __Game_Vehicles_PoliceCar_RW_ComponentTypeHandle;
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
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
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
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceCarData> __Game_Prefabs_PoliceCarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> __Game_Simulation_PolicePatrolRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> __Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> __Game_Buildings_PoliceStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public ComponentLookup<PolicePatrolRequest> __Game_Simulation_PolicePatrolRequest_RW_ComponentLookup;
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RW_ComponentLookup;
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RW_ComponentLookup;

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
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PoliceCar>();
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
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
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
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PoliceCarData_RO_ComponentLookup = state.GetComponentLookup<PoliceCarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup = state.GetComponentLookup<PolicePatrolRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup = state.GetComponentLookup<PoliceEmergencyRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentLookup = state.GetComponentLookup<CrimeProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PoliceStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.PoliceStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PolicePatrolRequest_RW_ComponentLookup = state.GetComponentLookup<PolicePatrolRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RW_ComponentLookup = state.GetComponentLookup<CrimeProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RW_ComponentLookup = state.GetComponentLookup<AccidentSite>();
      }
    }
  }
}
