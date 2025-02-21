// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PoliceAircraftAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
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
  public class PoliceAircraftAISystem : GameSystemBase
  {
    private const float MAX_WORK_DISTANCE = 200f;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_PolicePatrolRequestArchetype;
    private EntityArchetype m_PoliceEmergencyRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedAircraftRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private PoliceAircraftAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 10;

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
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<AircraftCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.PoliceCar>(), ComponentType.ReadWrite<Game.Common.Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicePatrolRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PolicePatrolRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceEmergencyRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PoliceEmergencyRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedAircraftRemoveTypes = new ComponentTypeSet(new ComponentType[12]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<AircraftNavigation>(),
        ComponentType.ReadWrite<AircraftNavigationLane>(),
        ComponentType.ReadWrite<AircraftCurrentLane>(),
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
      NativeQueue<PoliceAircraftAISystem.PoliceAction> nativeQueue = new NativeQueue<PoliceAircraftAISystem.PoliceAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PoliceCarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PoliceAircraftAISystem.PoliceAircraftTickJob jobData1 = new PoliceAircraftAISystem.PoliceAircraftTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_PoliceCarType = this.__TypeHandle.__Game_Vehicles_PoliceCar_RW_ComponentTypeHandle,
        m_AircraftType = this.__TypeHandle.__Game_Vehicles_Aircraft_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PointOfInterestType = this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_AircraftNavigationLaneType = this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_PrefabHelicopterData = this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup,
        m_PrefabPoliceCarData = this.__TypeHandle.__Game_Prefabs_PoliceCarData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_PolicePatrolRequestData = this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup,
        m_PoliceEmergencyRequestData = this.__TypeHandle.__Game_Simulation_PoliceEmergencyRequest_RO_ComponentLookup,
        m_CrimeProducerData = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentLookup,
        m_PoliceStationData = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_BlockerData = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_PolicePatrolRequestArchetype = this.m_PolicePatrolRequestArchetype,
        m_PoliceEmergencyRequestArchetype = this.m_PoliceEmergencyRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedAircraftRemoveTypes = this.m_MovingToParkedAircraftRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_RandomSeed = RandomSeed.Next(),
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies),
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PoliceAircraftAISystem.PoliceActionJob jobData2 = new PoliceAircraftAISystem.PoliceActionJob()
      {
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CrimeProducerData = this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<PoliceAircraftAISystem.PoliceAircraftTickJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<PoliceAircraftAISystem.PoliceActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
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
    public PoliceAircraftAISystem()
    {
    }

    private struct PoliceAction
    {
      public PoliceAircraftAISystem.PoliceActionType m_Type;
      public Entity m_Target;
      public Entity m_Request;
      public float m_CrimeReductionRate;
    }

    private enum PoliceActionType
    {
      ReduceCrime,
      AddPatrolRequest,
      SecureAccidentSite,
    }

    [BurstCompile]
    private struct PoliceAircraftTickJob : IJobChunk
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
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> m_PoliceCarType;
      public ComponentTypeHandle<Aircraft> m_AircraftType;
      public ComponentTypeHandle<AircraftCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      public ComponentTypeHandle<PointOfInterest> m_PointOfInterestType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<AircraftNavigationLane> m_AircraftNavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<HelicopterData> m_PrefabHelicopterData;
      [ReadOnly]
      public ComponentLookup<PoliceCarData> m_PrefabPoliceCarData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
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
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Blocker> m_BlockerData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public EntityArchetype m_PolicePatrolRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_PoliceEmergencyRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAircraftRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<PoliceAircraftAISystem.PoliceAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<AircraftCurrentLane> nativeArray5 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.PoliceCar> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.PoliceCar>(ref this.m_PoliceCarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Aircraft> nativeArray7 = chunk.GetNativeArray<Aircraft>(ref this.m_AircraftType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray8 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PointOfInterest> nativeArray9 = chunk.GetNativeArray<PointOfInterest>(ref this.m_PointOfInterestType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor1 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<AircraftNavigationLane> bufferAccessor2 = chunk.GetBufferAccessor<AircraftNavigationLane>(ref this.m_AircraftNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathInformation pathInformation = nativeArray4[index];
          Game.Vehicles.PoliceCar policeCar = nativeArray6[index];
          Aircraft aircraft = nativeArray7[index];
          AircraftCurrentLane currentLane = nativeArray5[index];
          PathOwner pathOwner = nativeArray10[index];
          Game.Common.Target target = nativeArray8[index];
          PointOfInterest pointOfInterest = nativeArray9[index];
          DynamicBuffer<AircraftNavigationLane> navigationLanes = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          DynamicBuffer<Passenger> passengers = new DynamicBuffer<Passenger>();
          if (bufferAccessor1.Length != 0)
            passengers = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, ref random, entity, owner, prefabRef, pathInformation, passengers, navigationLanes, serviceDispatches, ref policeCar, ref aircraft, ref currentLane, ref pathOwner, ref target, ref pointOfInterest);
          nativeArray6[index] = policeCar;
          nativeArray7[index] = aircraft;
          nativeArray5[index] = currentLane;
          nativeArray10[index] = pathOwner;
          nativeArray8[index] = target;
          nativeArray9[index] = pointOfInterest;
        }
      }

      private void Tick(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target,
        ref PointOfInterest pointOfInterest)
      {
        // ISSUE: reference to a compiler-generated field
        PoliceCarData prefabPoliceCarData = this.m_PrefabPoliceCarData[prefabRef.m_Prefab];
        policeCar.m_EstimatedShift = math.select(policeCar.m_EstimatedShift - 1U, 0U, policeCar.m_EstimatedShift == 0U);
        if (++policeCar.m_ShiftTime >= prefabPoliceCarData.m_ShiftDuration)
          policeCar.m_State |= PoliceCarFlags.ShiftEnded;
        if ((currentLane.m_LaneFlags & (AircraftLaneFlags.Flying | AircraftLaneFlags.Landing | AircraftLaneFlags.TakingOff)) == AircraftLaneFlags.Flying)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdatePointOfInterest(vehicleEntity, ref random, ref policeCar, ref aircraft, ref target, ref pointOfInterest);
        }
        else
        {
          pointOfInterest.m_IsValid = false;
          aircraft.m_Flags &= ~AircraftFlags.Working;
        }
        if ((aircraft.m_Flags & AircraftFlags.Emergency) == (AircraftFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryReduceCrime(vehicleEntity, prefabPoliceCarData, ref currentLane);
        }
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, pathInformation, serviceDispatches, ref policeCar, ref aircraft, ref currentLane);
        }
        if (VehicleUtils.IsStuck(pathOwner))
        {
          // ISSUE: reference to a compiler-generated field
          Blocker blocker = this.m_BlockerData[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          int num = this.m_ParkedCarData.HasComponent(blocker.m_Blocker) ? 1 : 0;
          if (num != 0)
          {
            Entity entity = blocker.m_Blocker;
            Controller componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControllerData.TryGetComponent(entity, out componentData))
              entity = componentData.m_Controller;
            DynamicBuffer<LayoutElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_LayoutElements.TryGetBuffer(entity, out bufferData);
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity, bufferData);
          }
          if (num != 0 || blocker.m_Blocker == Entity.Null)
          {
            pathOwner.m_State &= ~PathFlags.Stuck;
            // ISSUE: reference to a compiler-generated field
            this.m_BlockerData[vehicleEntity] = new Blocker();
          }
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
          this.ReturnToDepot(owner, serviceDispatches, ref policeCar, ref aircraft, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane) || (policeCar.m_State & (PoliceCarFlags.AtTarget | PoliceCarFlags.Disembarking)) != (PoliceCarFlags) 0)
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
                this.ParkAircraft(jobIndex, vehicleEntity, owner, ref aircraft, ref policeCar, ref currentLane);
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
              this.ParkAircraft(jobIndex, vehicleEntity, owner, ref aircraft, ref policeCar, ref currentLane);
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
            flag &= this.SecureAccidentSite(ref policeCar, passengers, serviceDispatches);
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
            if ((policeCar.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Disabled)) != (PoliceCarFlags) 0 || !this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, passengers, ref policeCar, ref aircraft, ref currentLane, ref pathOwner, ref target))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(owner, serviceDispatches, ref policeCar, ref aircraft, ref pathOwner, ref target);
            }
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
        if ((aircraft.m_Flags & AircraftFlags.Emergency) == (AircraftFlags) 0 && (policeCar.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Disabled)) != (PoliceCarFlags) 0)
        {
          if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(owner, serviceDispatches, ref policeCar, ref aircraft, ref pathOwner, ref target);
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
            this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, passengers, ref policeCar, ref aircraft, ref currentLane, ref pathOwner, ref target);
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
          // ISSUE: reference to a compiler-generated method
          this.FindNewPath(vehicleEntity, prefabRef, ref policeCar, ref aircraft, ref currentLane, ref pathOwner, ref target);
        }
        else
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) != (PathFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingSpace(ref aircraft, ref currentLane, ref pathOwner);
        }
      }

      private void CheckParkingSpace(
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        Game.Objects.SpawnLocation componentData;
        // ISSUE: reference to a compiler-generated field
        if ((currentLane.m_LaneFlags & AircraftLaneFlags.EndOfPath) == (AircraftLaneFlags) 0 || !this.m_SpawnLocationData.TryGetComponent(currentLane.m_Lane, out componentData))
          return;
        if ((componentData.m_Flags & SpawnLocationFlags.ParkedVehicle) != (SpawnLocationFlags) 0)
        {
          if ((aircraft.m_Flags & AircraftFlags.IgnoreParkedVehicle) != (AircraftFlags) 0)
            return;
          aircraft.m_Flags |= AircraftFlags.IgnoreParkedVehicle;
          pathOwner.m_State |= PathFlags.Obsolete;
        }
        else
          aircraft.m_Flags &= ~AircraftFlags.IgnoreParkedVehicle;
      }

      private void ParkAircraft(
        int jobIndex,
        Entity entity,
        Owner owner,
        ref Aircraft aircraft,
        ref Game.Vehicles.PoliceCar policeCar,
        ref AircraftCurrentLane currentLane)
      {
        aircraft.m_Flags &= ~(AircraftFlags.Emergency | AircraftFlags.IgnoreParkedVehicle);
        policeCar.m_State = PoliceCarFlags.Empty;
        Game.Buildings.PoliceStation componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PoliceStationData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & PoliceStationFlags.HasAvailablePoliceHelicopters) == (PoliceStationFlags) 0)
          policeCar.m_State |= PoliceCarFlags.Disabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_MovingToParkedAircraftRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_MovingToParkedAddTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(jobIndex, entity, new ParkedCar(currentLane.m_Lane, currentLane.m_CurvePosition.x));
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocationData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLane.m_Lane);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(Entity.Null, entity));
      }

      private void UpdatePointOfInterest(
        Entity entity,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Aircraft aircraft,
        ref Game.Common.Target target,
        ref PointOfInterest pointOfInterest)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[entity];
        Game.Objects.Transform componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.TryGetComponent(target.m_Target, out componentData))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CrimeProducerData.HasComponent(target.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 position = new float3(0.0f, 0.0f, this.m_PrefabObjectGeometryData[this.m_PrefabRefData[target.m_Target].m_Prefab].m_Bounds.max.z);
            componentData.m_Position = ObjectUtils.LocalToWorld(componentData, position);
          }
          if ((double) math.distancesq(transform.m_Position.xz, componentData.m_Position.xz) < 40000.0)
          {
            pointOfInterest.m_Position = componentData.m_Position;
            pointOfInterest.m_IsValid = true;
            aircraft.m_Flags |= AircraftFlags.Working;
            return;
          }
        }
        if ((aircraft.m_Flags & AircraftFlags.Emergency) == (AircraftFlags) 0)
        {
          float3 float3_1 = math.forward(transform.m_Rotation);
          float3_1 = MathUtils.Normalize(float3_1, float3_1.xz);
          float3 float3_2 = transform.m_Position + float3_1 * 50f;
          if (pointOfInterest.m_IsValid && (double) math.distancesq(float3_2.xz, pointOfInterest.m_Position.xz) < 40000.0)
          {
            aircraft.m_Flags |= AircraftFlags.Working;
            return;
          }
          float _radius = 125f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          PoliceAircraftAISystem.PoliceAircraftTickJob.FindPointOfInterestIterator iterator = new PoliceAircraftAISystem.PoliceAircraftTickJob.FindPointOfInterestIterator()
          {
            m_Circle = new Circle2(_radius, transform.m_Position.xz + float3_1.xz * _radius),
            m_Random = random,
            m_TransformData = this.m_TransformData,
            m_CrimeProducerData = this.m_CrimeProducerData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectSearchTree.Iterate<PoliceAircraftAISystem.PoliceAircraftTickJob.FindPointOfInterestIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          random = iterator.m_Random;
          // ISSUE: reference to a compiler-generated field
          if (iterator.m_TotalProbability != 0)
          {
            // ISSUE: reference to a compiler-generated field
            pointOfInterest.m_Position = iterator.m_Result;
            pointOfInterest.m_IsValid = true;
            aircraft.m_Flags |= AircraftFlags.Working;
            return;
          }
        }
        pointOfInterest.m_IsValid = false;
        aircraft.m_Flags &= ~AircraftFlags.Working;
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
        policeCar.m_State &= ~PoliceCarFlags.Disembarking;
        return true;
      }

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        HelicopterData helicopterData = this.m_PrefabHelicopterData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) helicopterData.m_FlyingMaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Methods = PathMethod.Road | PathMethod.Flying,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.Flying,
          m_RoadTypes = RoadTypes.Helicopter,
          m_FlyingTypes = RoadTypes.Helicopter
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Entity = target.m_Target
        };
        if ((policeCar.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
        {
          destination.m_Type = SetupTargetType.AccidentLocation;
          destination.m_Value2 = 30f;
          destination.m_Methods = PathMethod.Flying;
          destination.m_FlyingTypes = RoadTypes.Helicopter;
        }
        else if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
        {
          destination.m_Methods = PathMethod.Flying;
          destination.m_FlyingTypes = RoadTypes.Helicopter;
        }
        else
        {
          destination.m_Methods = PathMethod.Road;
          destination.m_RoadTypes = RoadTypes.Helicopter;
        }
        if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
        {
          parameters.m_Weights = new PathfindWeights(1f, 0.0f, 0.0f, 0.0f);
        }
        else
        {
          parameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
          destination.m_RandomCost = 30f;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private bool SecureAccidentSite(
        ref Game.Vehicles.PoliceCar policeCar,
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
              // ISSUE: reference to a compiler-generated field
              if ((this.m_AccidentSiteData[emergencyRequest.m_Site].m_Flags & AccidentSiteFlags.Secured) == (AccidentSiteFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_ActionQueue.Enqueue(new PoliceAircraftAISystem.PoliceAction()
                {
                  m_Type = PoliceAircraftAISystem.PoliceActionType.SecureAccidentSite,
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
              if (this.m_EntityLookup.Exists(policePatrolRequest.m_Target) && !flag3 && (double) policePatrolRequest.m_Priority > (double) num1)
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
              if (this.m_EntityLookup.Exists(emergencyRequest.m_Site) && (!flag3 || (double) emergencyRequest.m_Priority > (double) num1))
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
          PathInformation componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformationData.TryGetComponent(entity, out componentData))
            policeCar.m_EstimatedShift += (uint) Mathf.RoundToInt(componentData.m_Duration * 3.75f);
        }
        if (serviceDispatches.Length <= policeCar.m_RequestCount)
          return;
        serviceDispatches.RemoveRange(policeCar.m_RequestCount, serviceDispatches.Length - policeCar.m_RequestCount);
      }

      private void RequestTargetIfNeeded(int jobIndex, Entity entity, ref Game.Vehicles.PoliceCar policeCar)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(policeCar.m_TargetRequest) || (policeCar.m_PurposeMask & PolicePurpose.Patrol) == (PolicePurpose) 0 || (policeCar.m_State & (PoliceCarFlags.Empty | PoliceCarFlags.EstimatedShiftEnd)) != PoliceCarFlags.Empty || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 16U) - 1) != 10)
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

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
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
          if (!this.m_EntityLookup.Exists(entity1))
          {
            serviceDispatches.RemoveAt(0);
            policeCar.m_EstimatedShift -= policeCar.m_EstimatedShift / (uint) policeCar.m_RequestCount;
          }
          else
          {
            aircraft.m_Flags &= ~AircraftFlags.IgnoreParkedVehicle;
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
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1))
                {
                  if ((policeCarFlags & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0)
                  {
                    aircraft.m_Flags |= AircraftFlags.Emergency | AircraftFlags.StayMidAir;
                  }
                  else
                  {
                    for (int index = 0; index < pathElement2.Length; ++index)
                    {
                      PathElement pathElement3 = pathElement2[index];
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_ConnectionLaneData.HasComponent(pathElement3.m_Target) && (this.m_ConnectionLaneData[pathElement3.m_Target].m_Flags & (ConnectionLaneFlags.Outside | ConnectionLaneFlags.Airway)) == ConnectionLaneFlags.Airway)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.AddPatrolRequests(pathElement3.m_Target, request);
                      }
                    }
                    aircraft.m_Flags &= ~AircraftFlags.Emergency;
                    aircraft.m_Flags |= AircraftFlags.StayMidAir;
                  }
                  if (policeCar.m_RequestCount == 1)
                    policeCar.m_EstimatedShift = (uint) Mathf.RoundToInt(num * 3.75f);
                  policeCar.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  target.m_Target = entity1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
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
        Owner owner,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Aircraft aircraft,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        serviceDispatches.Clear();
        policeCar.m_RequestCount = 0;
        policeCar.m_EstimatedShift = 0U;
        policeCar.m_State &= ~(PoliceCarFlags.AccidentTarget | PoliceCarFlags.AtTarget | PoliceCarFlags.Cancelled);
        policeCar.m_State |= PoliceCarFlags.Returning;
        aircraft.m_Flags &= ~(AircraftFlags.Emergency | AircraftFlags.IgnoreParkedVehicle);
        VehicleUtils.SetTarget(ref pathOwner, ref target, owner.m_Owner);
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.PoliceCar policeCar,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[vehicleEntity];
        PathUtils.ResetPath(ref currentLane, pathElement1);
        if ((policeCar.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0 && policeCar.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PolicePatrolRequestData.HasComponent(request))
          {
            for (int index = 0; index < pathElement1.Length; ++index)
            {
              PathElement pathElement2 = pathElement1[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectionLaneData.HasComponent(pathElement2.m_Target) && (this.m_ConnectionLaneData[pathElement2.m_Target].m_Flags & (ConnectionLaneFlags.Outside | ConnectionLaneFlags.Airway)) == ConnectionLaneFlags.Airway)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddPatrolRequests(pathElement2.m_Target, request);
              }
            }
            aircraft.m_Flags &= ~AircraftFlags.Emergency;
            aircraft.m_Flags |= AircraftFlags.StayMidAir;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceEmergencyRequestData.HasComponent(request))
            {
              aircraft.m_Flags |= AircraftFlags.Emergency | AircraftFlags.StayMidAir;
            }
            else
            {
              aircraft.m_Flags &= ~AircraftFlags.Emergency;
              aircraft.m_Flags |= AircraftFlags.StayMidAir;
            }
          }
          if (policeCar.m_RequestCount == 1)
            policeCar.m_EstimatedShift = (uint) Mathf.RoundToInt(pathInformation.m_Duration * 3.75f);
        }
        else
          aircraft.m_Flags &= ~(AircraftFlags.StayOnTaxiway | AircraftFlags.StayMidAir);
        policeCar.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement1.Length);
      }

      private void AddPatrolRequests(Entity laneEntity, Entity request)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[laneEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PoliceAircraftAISystem.PoliceAircraftTickJob.AddRequestIterator iterator = new PoliceAircraftAISystem.PoliceAircraftTickJob.AddRequestIterator()
        {
          m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), (float3) 300f),
          m_Curve = curve.m_Bezier.xz,
          m_Distance = 300f,
          m_Request = request,
          m_TransformData = this.m_TransformData,
          m_CrimeProducerData = this.m_CrimeProducerData,
          m_ActionQueue = this.m_ActionQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<PoliceAircraftAISystem.PoliceAircraftTickJob.AddRequestIterator>(ref iterator);
      }

      private void TryReduceCrime(
        Entity vehicleEntity,
        PoliceCarData prefabPoliceCarData,
        ref AircraftCurrentLane currentLane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectionLaneData.HasComponent(currentLane.m_Lane) || (this.m_ConnectionLaneData[currentLane.m_Lane].m_Flags & (ConnectionLaneFlags.Outside | ConnectionLaneFlags.Airway)) != ConnectionLaneFlags.Airway || (currentLane.m_LaneFlags & AircraftLaneFlags.Checked) != (AircraftLaneFlags) 0)
          return;
        currentLane.m_LaneFlags |= AircraftLaneFlags.Checked;
        // ISSUE: reference to a compiler-generated method
        this.ReduceCrime(currentLane.m_Lane, prefabPoliceCarData.m_CrimeReductionRate);
      }

      private void TryReduceCrime(
        Entity vehicleEntity,
        PoliceCarData prefabPoliceCarData,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CrimeProducerData.HasComponent(target.m_Target) || (double) this.m_CrimeProducerData[target.m_Target].m_Crime <= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ActionQueue.Enqueue(new PoliceAircraftAISystem.PoliceAction()
        {
          m_Type = PoliceAircraftAISystem.PoliceActionType.ReduceCrime,
          m_Target = target.m_Target,
          m_CrimeReductionRate = prefabPoliceCarData.m_CrimeReductionRate
        });
      }

      private void ReduceCrime(Entity laneEntity, float reduction)
      {
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[laneEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PoliceAircraftAISystem.PoliceAircraftTickJob.ReduceCrimeIterator iterator = new PoliceAircraftAISystem.PoliceAircraftTickJob.ReduceCrimeIterator()
        {
          m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), (float3) 450.000031f),
          m_Curve = curve.m_Bezier.xz,
          m_Distance = 750f * new float2(0.2f, 0.6f),
          m_Reduction = reduction,
          m_TransformData = this.m_TransformData,
          m_CrimeProducerData = this.m_CrimeProducerData,
          m_ActionQueue = this.m_ActionQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<PoliceAircraftAISystem.PoliceAircraftTickJob.ReduceCrimeIterator>(ref iterator);
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

      private struct FindPointOfInterestIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Circle2 m_Circle;
        public Unity.Mathematics.Random m_Random;
        public float3 m_Result;
        public int m_TotalProbability;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<CrimeProducer> m_CrimeProducerData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Circle);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity building)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Circle) || !this.m_CrimeProducerData.HasComponent(building))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 position = new float3(0.0f, 0.0f, this.m_PrefabObjectGeometryData[this.m_PrefabRefData[building].m_Prefab].m_Bounds.max.z);
          // ISSUE: reference to a compiler-generated field
          float3 world = ObjectUtils.LocalToWorld(this.m_TransformData[building], position);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distance(this.m_Circle.position, world.xz) >= (double) this.m_Circle.radius)
            return;
          int num = 100;
          // ISSUE: reference to a compiler-generated field
          this.m_TotalProbability += num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Random.NextInt(this.m_TotalProbability) >= num)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Result = world;
        }
      }

      private struct AddRequestIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Bezier4x2 m_Curve;
        public float m_Distance;
        public Entity m_Request;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<CrimeProducer> m_CrimeProducerData;
        public NativeQueue<PoliceAircraftAISystem.PoliceAction>.ParallelWriter m_ActionQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity building)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_CrimeProducerData.HasComponent(building) || (double) MathUtils.Distance(this.m_Curve, this.m_TransformData[building].m_Position.xz, out float _) > (double) this.m_Distance)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PoliceAircraftAISystem.PoliceAction()
          {
            m_Type = PoliceAircraftAISystem.PoliceActionType.AddPatrolRequest,
            m_Target = building,
            m_Request = this.m_Request
          });
        }
      }

      private struct ReduceCrimeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Bezier4x2 m_Curve;
        public float2 m_Distance;
        public float m_Reduction;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<CrimeProducer> m_CrimeProducerData;
        public NativeQueue<PoliceAircraftAISystem.PoliceAction>.ParallelWriter m_ActionQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity building)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_CrimeProducerData.HasComponent(building))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = MathUtils.Distance(this.m_Curve, this.m_TransformData[building].m_Position.xz, out float _);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) num >= (double) this.m_Distance.y || (double) this.m_CrimeProducerData[building].m_Crime <= 0.0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new PoliceAircraftAISystem.PoliceAction()
          {
            m_Type = PoliceAircraftAISystem.PoliceActionType.ReduceCrime,
            m_Target = building,
            m_CrimeReductionRate = this.m_Reduction * (float) (1.0 - (double) math.max(0.0f, num - this.m_Distance.x) / ((double) this.m_Distance.y - (double) this.m_Distance.x))
          });
        }
      }
    }

    [BurstCompile]
    private struct PoliceActionJob : IJob
    {
      [ReadOnly]
      public uint m_SimulationFrame;
      public ComponentLookup<CrimeProducer> m_CrimeProducerData;
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      public NativeQueue<PoliceAircraftAISystem.PoliceAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PoliceAircraftAISystem.PoliceAction policeAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out policeAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PoliceAircraftAISystem.PoliceActionType type = policeAction.m_Type;
          switch (type)
          {
            case PoliceAircraftAISystem.PoliceActionType.ReduceCrime:
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
            case PoliceAircraftAISystem.PoliceActionType.AddPatrolRequest:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              CrimeProducer crimeProducer2 = this.m_CrimeProducerData[policeAction.m_Target] with
              {
                m_PatrolRequest = policeAction.m_Request
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CrimeProducerData[policeAction.m_Target] = crimeProducer2;
              continue;
            case PoliceAircraftAISystem.PoliceActionType.SecureAccidentSite:
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
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> __Game_Vehicles_PoliceCar_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Aircraft> __Game_Vehicles_Aircraft_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PointOfInterest> __Game_Common_PointOfInterest_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<AircraftNavigationLane> __Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HelicopterData> __Game_Prefabs_HelicopterData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PoliceCarData> __Game_Prefabs_PoliceCarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
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
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public ComponentLookup<Blocker> __Game_Vehicles_Blocker_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
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
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PoliceCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Aircraft_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Aircraft>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PointOfInterest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PointOfInterest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<AircraftNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HelicopterData_RO_ComponentLookup = state.GetComponentLookup<HelicopterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PoliceCarData_RO_ComponentLookup = state.GetComponentLookup<PoliceCarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
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
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentLookup = state.GetComponentLookup<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RW_ComponentLookup = state.GetComponentLookup<CrimeProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RW_ComponentLookup = state.GetComponentLookup<AccidentSite>();
      }
    }
  }
}
