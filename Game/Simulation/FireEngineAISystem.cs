// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FireEngineAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class FireEngineAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private EntityQuery m_VehicleQuery;
    private EntityQuery m_ConfigQuery;
    private EntityArchetype m_FireRescueRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private FireEngineAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 4;

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
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.FireEngine>(), ComponentType.ReadWrite<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireRescueRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<FireRescueRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
      // ISSUE: reference to a compiler-generated field
      FireConfigurationData singleton = this.m_ConfigQuery.GetSingleton<FireConfigurationData>();
      NativeQueue<FireEngineAISystem.FireExtinguishing> nativeQueue = new NativeQueue<FireEngineAISystem.FireExtinguishing>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
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
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_FireStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RescueTarget_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_FireEngineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_FireEngine_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      FireEngineAISystem.FireEngineTickJob jobData1 = new FireEngineAISystem.FireEngineTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_FireEngineType = this.__TypeHandle.__Game_Vehicles_FireEngine_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabFireEngineData = this.__TypeHandle.__Game_Prefabs_FireEngineData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_FireRescueRequestData = this.__TypeHandle.__Game_Simulation_FireRescueRequest_RO_ComponentLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_RescueTargetData = this.__TypeHandle.__Game_Buildings_RescueTarget_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_FireStationData = this.__TypeHandle.__Game_Buildings_FireStation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_FireRescueRequestArchetype = this.m_FireRescueRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_StructuralIntegrityData = new EventHelpers.StructuralIntegrityData((SystemBase) this, singleton),
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_ExtinguishingQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RescueTarget_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      FireEngineAISystem.FireExtinguishingJob jobData2 = new FireEngineAISystem.FireExtinguishingJob()
      {
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RW_ComponentLookup,
        m_RescueTargetData = this.__TypeHandle.__Game_Buildings_RescueTarget_RW_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentLookup,
        m_ExtinguishingQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<FireEngineAISystem.FireEngineTickJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<FireEngineAISystem.FireExtinguishingJob>(dependsOn);
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
    public FireEngineAISystem()
    {
    }

    private struct FireExtinguishing
    {
      public Entity m_Vehicle;
      public Entity m_Target;
      public Entity m_Request;
      public float m_FireIntensityDelta;
      public float m_WaterDamageDelta;
      public float m_DestroyedClearDelta;

      public FireExtinguishing(
        Entity vehicle,
        Entity target,
        Entity request,
        float intensityDelta,
        float waterDamageDelta,
        float destroyedClearDelta)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Vehicle = vehicle;
        // ISSUE: reference to a compiler-generated field
        this.m_Target = target;
        // ISSUE: reference to a compiler-generated field
        this.m_Request = request;
        // ISSUE: reference to a compiler-generated field
        this.m_FireIntensityDelta = intensityDelta;
        // ISSUE: reference to a compiler-generated field
        this.m_WaterDamageDelta = waterDamageDelta;
        // ISSUE: reference to a compiler-generated field
        this.m_DestroyedClearDelta = destroyedClearDelta;
      }
    }

    [BurstCompile]
    private struct FireEngineTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      public ComponentTypeHandle<Game.Vehicles.FireEngine> m_FireEngineType;
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
      public ComponentLookup<PathInformation> m_PathInformationData;
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
      public ComponentLookup<FireEngineData> m_PrefabFireEngineData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<RescueTarget> m_RescueTargetData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.FireStation> m_FireStationData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public EntityArchetype m_FireRescueRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EventHelpers.StructuralIntegrityData m_StructuralIntegrityData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<FireEngineAISystem.FireExtinguishing>.ParallelWriter m_ExtinguishingQueue;

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
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathInformation> nativeArray5 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray6 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.FireEngine> nativeArray7 = chunk.GetNativeArray<Game.Vehicles.FireEngine>(ref this.m_FireEngineType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray8 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray9 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isStopped = chunk.Has<Stopped>(ref this.m_StoppedType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          Transform transform = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          PathInformation pathInformation = nativeArray5[index];
          Game.Vehicles.FireEngine fireEngine = nativeArray7[index];
          Car car = nativeArray8[index];
          CarCurrentLane currentLane = nativeArray6[index];
          PathOwner pathOwner = nativeArray10[index];
          Target target = nativeArray9[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor2[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, transform, prefabRef, pathInformation, navigationLanes, serviceDispatches, isStopped, ref random, ref fireEngine, ref car, ref currentLane, ref pathOwner, ref target);
          nativeArray7[index] = fireEngine;
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
        Transform transform,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isStopped,
        ref Random random,
        ref Game.Vehicles.FireEngine fireEngine,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, ref random, pathInformation, serviceDispatches, ref fireEngine, ref car, ref currentLane, ref pathOwner);
        }
        // ISSUE: reference to a compiler-generated field
        FireEngineData fireEngineData = this.m_PrefabFireEngineData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if (VehicleUtils.IsStuck(pathOwner) || (fireEngine.m_State & FireEngineFlags.Returning) != (FireEngineFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, vehicleEntity, owner, serviceDispatches, ref fireEngine, ref car, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((fireEngine.m_State & FireEngineFlags.Returning) != (FireEngineFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          if ((fireEngine.m_State & (FireEngineFlags.Extinguishing | FireEngineFlags.Rescueing)) == (FireEngineFlags) 0 && !this.BeginExtinguishing(jobIndex, vehicleEntity, isStopped, ref fireEngine, ref currentLane, ref target))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckServiceDispatches(vehicleEntity, serviceDispatches, fireEngineData, ref fireEngine, ref pathOwner);
            // ISSUE: reference to a compiler-generated method
            if ((fireEngine.m_State & (FireEngineFlags.Empty | FireEngineFlags.Disabled)) != (FireEngineFlags) 0 || !this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, ref fireEngine, ref car, ref currentLane, ref pathOwner, ref target))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, vehicleEntity, owner, serviceDispatches, ref fireEngine, ref car, ref pathOwner, ref target);
            }
          }
        }
        else
        {
          if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
          {
            if ((fireEngine.m_State & FireEngineFlags.Returning) != (FireEngineFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, vehicleEntity, owner, fireEngineData, ref fireEngine, ref car, ref currentLane);
              return;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          if (isStopped)
          {
            // ISSUE: reference to a compiler-generated method
            this.StartVehicle(jobIndex, vehicleEntity, ref currentLane);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if ((car.m_Flags & CarFlags.Emergency) != (CarFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.IsBlocked) != (Game.Vehicles.CarLaneFlags) 0 && this.IsCloseEnough(transform, ref target))
            {
              // ISSUE: reference to a compiler-generated method
              this.EndNavigation(vehicleEntity, ref currentLane, ref pathOwner, navigationLanes);
            }
          }
        }
        if ((double) fireEngineData.m_ExtinguishingCapacity != 0.0 && fireEngine.m_RequestCount <= 1)
        {
          OnFire componentData;
          // ISSUE: reference to a compiler-generated field
          if (fireEngine.m_RequestCount == 1 && this.m_OnFireData.TryGetComponent(target.m_Target, out componentData) && (double) componentData.m_Intensity > 0.0)
            fireEngine.m_State |= FireEngineFlags.EstimatedEmpty;
          else
            fireEngine.m_State &= ~FireEngineFlags.EstimatedEmpty;
        }
        if ((fireEngine.m_State & FireEngineFlags.Empty) != (FireEngineFlags) 0)
        {
          serviceDispatches.Clear();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, fireEngineData, ref fireEngine, ref pathOwner);
          if (fireEngine.m_RequestCount <= 1 && (fireEngine.m_State & FireEngineFlags.EstimatedEmpty) == (FireEngineFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref fireEngine);
          }
        }
        if ((fireEngine.m_State & (FireEngineFlags.Extinguishing | FireEngineFlags.Rescueing)) != (FireEngineFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          if (!this.TryExtinguishFire(vehicleEntity, fireEngineData, ref fireEngine, ref target) && ((fireEngine.m_State & (FireEngineFlags.Empty | FireEngineFlags.Disabled)) != (FireEngineFlags) 0 || !this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, ref fireEngine, ref car, ref currentLane, ref pathOwner, ref target)))
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(jobIndex, vehicleEntity, owner, serviceDispatches, ref fireEngine, ref car, ref pathOwner, ref target);
          }
        }
        else if ((fireEngine.m_State & (FireEngineFlags.Returning | FireEngineFlags.Empty | FireEngineFlags.Disabled)) == FireEngineFlags.Returning)
        {
          // ISSUE: reference to a compiler-generated method
          this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, ref fireEngine, ref car, ref currentLane, ref pathOwner, ref target);
        }
        if ((car.m_Flags & CarFlags.Emergency) != (CarFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.TryAddRequests(vehicleEntity, fireEngineData, serviceDispatches, ref fireEngine, ref target);
        }
        if ((fireEngine.m_State & (FireEngineFlags.Extinguishing | FireEngineFlags.Rescueing)) != (FireEngineFlags) 0)
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
            this.FindNewPath(vehicleEntity, prefabRef, ref fireEngine, ref currentLane, ref pathOwner, ref target);
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
        FireEngineData fireEngineData,
        ref Game.Vehicles.FireEngine fireEngine,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        car.m_Flags &= ~CarFlags.Emergency;
        fireEngine.m_State = (FireEngineFlags) 0;
        fireEngine.m_ExtinguishingAmount = fireEngineData.m_ExtinguishingCapacity;
        Game.Buildings.FireStation componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_FireStationData.TryGetComponent(owner.m_Owner, out componentData))
        {
          if ((componentData.m_Flags & FireStationFlags.HasFreeFireEngines) == (FireStationFlags) 0)
            fireEngine.m_State |= FireEngineFlags.Disabled;
          if ((componentData.m_Flags & FireStationFlags.DisasterResponseAvailable) != (FireStationFlags) 0)
            fireEngine.m_State |= FireEngineFlags.DisasterResponse;
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

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        ref Game.Vehicles.FireEngine fireEngine,
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
          m_Methods = PathMethod.Road | PathMethod.Offroad | PathMethod.SpecialParking,
          m_ParkingTarget = VehicleUtils.GetParkingSource(vehicleEntity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
          m_ParkingDelta = currentLane.m_CurvePosition.z,
          m_ParkingSize = VehicleUtils.GetParkingSize(vehicleEntity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData),
          m_IgnoredRules = RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidPrivateTraffic | VehicleUtils.GetIgnoredPathfindRules(carData)
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.Offroad | PathMethod.SpecialParking,
          m_RoadTypes = RoadTypes.Car
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.Offroad,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = target.m_Target
        };
        if ((fireEngine.m_State & FireEngineFlags.Returning) == (FireEngineFlags) 0)
        {
          parameters.m_Weights = new PathfindWeights(1f, 0.0f, 0.0f, 0.0f);
          parameters.m_IgnoredRules |= RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic;
          destination.m_Value2 = 30f;
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

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        FireEngineData prefabFireEngineData,
        ref Game.Vehicles.FireEngine fireEngine,
        ref PathOwner pathOwner)
      {
        if (serviceDispatches.Length <= fireEngine.m_RequestCount)
          return;
        float num1 = -1f;
        Entity entity = Entity.Null;
        PathElement pathElement1 = new PathElement();
        bool flag = false;
        int num2 = 0;
        if (fireEngine.m_RequestCount >= 1 && (fireEngine.m_State & FireEngineFlags.Returning) == (FireEngineFlags) 0)
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
        for (int index = num2; index < fireEngine.m_RequestCount; ++index)
        {
          DynamicBuffer<PathElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElements.TryGetBuffer(serviceDispatches[index].m_Request, out bufferData) && bufferData.Length != 0)
          {
            pathElement1 = bufferData[bufferData.Length - 1];
            flag = true;
          }
        }
        for (int requestCount = fireEngine.m_RequestCount; requestCount < serviceDispatches.Length; ++requestCount)
        {
          Entity request = serviceDispatches[requestCount].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_FireRescueRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            FireRescueRequest fireRescueRequest = this.m_FireRescueRequestData[request];
            DynamicBuffer<PathElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (flag && this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
            {
              PathElement pathElement3 = bufferData[0];
              if (pathElement3.m_Target != pathElement1.m_Target || (double) pathElement3.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(fireRescueRequest.m_Target) && (double) fireRescueRequest.m_Priority > (double) num1)
            {
              num1 = fireRescueRequest.m_Priority;
              entity = request;
            }
          }
        }
        if (entity != Entity.Null)
        {
          if ((double) prefabFireEngineData.m_ExtinguishingCapacity != 0.0)
          {
            OnFire componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_OnFireData.TryGetComponent(this.m_FireRescueRequestData[entity].m_Target, out componentData) && (double) componentData.m_Intensity > 0.0)
              fireEngine.m_State |= FireEngineFlags.EstimatedEmpty;
            else if (fireEngine.m_RequestCount == 0)
              fireEngine.m_State &= ~FireEngineFlags.EstimatedEmpty;
          }
          serviceDispatches[fireEngine.m_RequestCount++] = new ServiceDispatch(entity);
        }
        if (serviceDispatches.Length <= fireEngine.m_RequestCount)
          return;
        serviceDispatches.RemoveRange(fireEngine.m_RequestCount, serviceDispatches.Length - fireEngine.m_RequestCount);
      }

      private void RequestTargetIfNeeded(int jobIndex, Entity entity, ref Game.Vehicles.FireEngine fireEngine)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_FireRescueRequestData.HasComponent(fireEngine.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(64U, 16U) - 1) != 4)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_FireRescueRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<FireRescueRequest>(jobIndex, entity1, new FireRescueRequest(entity, 1f, FireRescueRequestType.Fire));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(4U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.FireEngine fireEngine,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if ((fireEngine.m_State & FireEngineFlags.Returning) == (FireEngineFlags) 0 && fireEngine.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          --fireEngine.m_RequestCount;
        }
        for (; fireEngine.m_RequestCount > 0 && serviceDispatches.Length > 0; --fireEngine.m_RequestCount)
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity target1 = Entity.Null;
          FireRescueRequest componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_FireRescueRequestData.TryGetComponent(request, out componentData1))
            target1 = componentData1.m_Target;
          if (componentData1.m_Type == FireRescueRequestType.Fire)
          {
            OnFire componentData2;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OnFireData.TryGetComponent(target1, out componentData2) || (double) componentData2.m_Intensity == 0.0)
              target1 = Entity.Null;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_RescueTargetData.HasComponent(target1))
              target1 = Entity.Null;
          }
          if (target1 == Entity.Null)
          {
            serviceDispatches.RemoveAt(0);
          }
          else
          {
            fireEngine.m_State &= ~(FireEngineFlags.Extinguishing | FireEngineFlags.Rescueing);
            fireEngine.m_State &= ~FireEngineFlags.Returning;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(request, vehicleEntity, false, true));
            // ISSUE: reference to a compiler-generated field
            if (this.m_FireRescueRequestData.HasComponent(fireEngine.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(fireEngine.m_TargetRequest, Entity.Null, true));
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
                float num = fireEngine.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes))
                {
                  fireEngine.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  target.m_Target = target1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  car.m_Flags |= CarFlags.Emergency | CarFlags.StayOnRoad | CarFlags.UsePublicTransportLanes;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity);
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, target1);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity);
            return true;
          }
        }
        return false;
      }

      private bool BeginExtinguishing(
        int jobIndex,
        Entity entity,
        bool isStopped,
        ref Game.Vehicles.FireEngine fireEngine,
        ref CarCurrentLane currentLaneData,
        ref Target target)
      {
        if ((fireEngine.m_State & FireEngineFlags.Empty) != (FireEngineFlags) 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OnFireData.HasComponent(target.m_Target))
        {
          fireEngine.m_State |= FireEngineFlags.Extinguishing;
          if (!isStopped)
          {
            // ISSUE: reference to a compiler-generated method
            this.StopVehicle(jobIndex, entity, ref currentLaneData);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, entity);
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RescueTargetData.HasComponent(target.m_Target))
          return false;
        fireEngine.m_State |= FireEngineFlags.Rescueing;
        if (!isStopped)
        {
          // ISSUE: reference to a compiler-generated method
          this.StopVehicle(jobIndex, entity, ref currentLaneData);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, entity);
        return true;
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.FireEngine fireEngine,
        ref Car carData,
        ref PathOwner pathOwnerData,
        ref Target targetData)
      {
        serviceDispatches.Clear();
        fireEngine.m_RequestCount = 0;
        fireEngine.m_State &= ~(FireEngineFlags.Extinguishing | FireEngineFlags.Rescueing);
        fireEngine.m_State |= FireEngineFlags.Returning;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity);
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, ownerData.m_Owner);
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        ref Random random,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.FireEngine fireEngine,
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
        if ((fireEngine.m_State & FireEngineFlags.Returning) == (FireEngineFlags) 0 && fireEngine.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_FireRescueRequestData.HasComponent(serviceDispatches[0].m_Request))
          {
            carData.m_Flags |= CarFlags.Emergency | CarFlags.StayOnRoad;
          }
          else
          {
            carData.m_Flags &= ~CarFlags.Emergency;
            carData.m_Flags |= CarFlags.StayOnRoad;
          }
        }
        else
          carData.m_Flags &= ~(CarFlags.Emergency | CarFlags.StayOnRoad);
        carData.m_Flags |= CarFlags.UsePublicTransportLanes;
        fireEngine.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement.Length);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity);
      }

      private bool IsCloseEnough(Transform transform, ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransformData.HasComponent(target.m_Target))
          return false;
        // ISSUE: reference to a compiler-generated field
        Transform transform1 = this.m_TransformData[target.m_Target];
        return (double) math.distance(transform.m_Position, transform1.m_Position) <= 30.0;
      }

      private bool TryExtinguishFire(
        Entity vehicleEntity,
        FireEngineData prefabFireEngineData,
        ref Game.Vehicles.FireEngine fireEngine,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if ((fireEngine.m_State & FireEngineFlags.Empty) != (FireEngineFlags) 0 || !this.m_TransformData.HasComponent(target.m_Target))
          return false;
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[target.m_Target];
        float extinguishingSpread = prefabFireEngineData.m_ExtinguishingSpread;
        float num1 = prefabFireEngineData.m_ExtinguishingRate * fireEngine.m_Efficiency;
        float num2 = fireEngine.m_Efficiency / math.max(1f / 1000f, prefabFireEngineData.m_DestroyedClearDuration);
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
        FireEngineAISystem.FireEngineTickJob.ObjectExtinguishIterator iterator = new FireEngineAISystem.FireEngineTickJob.ObjectExtinguishIterator()
        {
          m_Bounds = new Bounds3(transform.m_Position - extinguishingSpread, transform.m_Position + extinguishingSpread),
          m_Position = transform.m_Position,
          m_Spread = extinguishingSpread,
          m_ExtinguishRate = num1,
          m_ClearRate = num2,
          m_Vehicle = vehicleEntity,
          m_Target = target.m_Target,
          m_TransformData = this.m_TransformData,
          m_OnFireData = this.m_OnFireData,
          m_DestroyedData = this.m_DestroyedData,
          m_RescueTargetData = this.m_RescueTargetData,
          m_FireRescueRequestData = this.m_FireRescueRequestData,
          m_BuildingData = this.m_BuildingData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_StructuralIntegrityData = this.m_StructuralIntegrityData,
          m_ExtinguishingQueue = this.m_ExtinguishingQueue
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OnFireData.HasComponent(target.m_Target) || this.m_RescueTargetData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated method
          iterator.TryExtinguish(target.m_Target);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<FireEngineAISystem.FireEngineTickJob.ObjectExtinguishIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        if (iterator.m_ExtinguishResult != Entity.Null)
        {
          float num3 = 0.266666681f;
          fireEngine.m_ExtinguishingAmount = math.max(0.0f, fireEngine.m_ExtinguishingAmount - num1 * num3);
          if ((double) fireEngine.m_ExtinguishingAmount == 0.0 && (double) prefabFireEngineData.m_ExtinguishingCapacity != 0.0)
            fireEngine.m_State |= FireEngineFlags.Empty;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        return iterator.m_ClearResult != Entity.Null;
      }

      private void TryAddRequests(
        Entity vehicleEntity,
        FireEngineData prefabFireEngineData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.FireEngine fireEngine,
        ref Target target)
      {
        if (fireEngine.m_RequestCount <= 0 || serviceDispatches.Length <= 0)
          return;
        Entity request = serviceDispatches[0].m_Request;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_FireRescueRequestData.HasComponent(request) || !this.m_TransformData.HasComponent(target.m_Target))
          return;
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[target.m_Target];
        float extinguishingSpread = prefabFireEngineData.m_ExtinguishingSpread;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        FireEngineAISystem.FireEngineTickJob.ObjectRequestIterator iterator = new FireEngineAISystem.FireEngineTickJob.ObjectRequestIterator()
        {
          m_Bounds = new Bounds3(transform.m_Position - extinguishingSpread, transform.m_Position + extinguishingSpread),
          m_Position = transform.m_Position,
          m_Spread = extinguishingSpread,
          m_Vehicle = vehicleEntity,
          m_Request = request,
          m_TransformData = this.m_TransformData,
          m_OnFireData = this.m_OnFireData,
          m_RescueTargetData = this.m_RescueTargetData,
          m_FireRescueRequestData = this.m_FireRescueRequestData,
          m_ExtinguishingQueue = this.m_ExtinguishingQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<FireEngineAISystem.FireEngineTickJob.ObjectRequestIterator>(ref iterator);
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

      private struct ObjectRequestIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float3 m_Position;
        public float m_Spread;
        public Entity m_Vehicle;
        public Entity m_Request;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<OnFire> m_OnFireData;
        public ComponentLookup<RescueTarget> m_RescueTargetData;
        public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
        public NativeQueue<FireEngineAISystem.FireExtinguishing>.ParallelWriter m_ExtinguishingQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_OnFireData.HasComponent(objectEntity) && !this.m_RescueTargetData.HasComponent(objectEntity) || (double) math.distance(this.m_TransformData[objectEntity].m_Position, this.m_Position) > (double) this.m_Spread)
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OnFireData.HasComponent(objectEntity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(this.m_OnFireData[objectEntity].m_RescueRequest != this.m_Request))
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ExtinguishingQueue.Enqueue(new FireEngineAISystem.FireExtinguishing(this.m_Vehicle, objectEntity, this.m_Request, 0.0f, 0.0f, 0.0f));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_RescueTargetData.HasComponent(objectEntity) || !(this.m_RescueTargetData[objectEntity].m_Request != this.m_Request))
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ExtinguishingQueue.Enqueue(new FireEngineAISystem.FireExtinguishing(this.m_Vehicle, objectEntity, this.m_Request, 0.0f, 0.0f, 0.0f));
          }
        }
      }

      private struct ObjectExtinguishIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float3 m_Position;
        public float m_Spread;
        public float m_ExtinguishRate;
        public float m_ClearRate;
        public Entity m_Vehicle;
        public Entity m_Target;
        public ComponentLookup<Transform> m_TransformData;
        public ComponentLookup<OnFire> m_OnFireData;
        public ComponentLookup<Destroyed> m_DestroyedData;
        public ComponentLookup<RescueTarget> m_RescueTargetData;
        public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public EventHelpers.StructuralIntegrityData m_StructuralIntegrityData;
        public NativeQueue<FireEngineAISystem.FireExtinguishing>.ParallelWriter m_ExtinguishingQueue;
        public Entity m_ExtinguishResult;
        public Entity m_ClearResult;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_OnFireData.HasComponent(objectEntity) && !this.m_RescueTargetData.HasComponent(objectEntity) || objectEntity == this.m_Target || (double) math.distance(this.m_TransformData[objectEntity].m_Position, this.m_Position) > (double) this.m_Spread)
            return;
          // ISSUE: reference to a compiler-generated method
          this.TryExtinguish(objectEntity);
        }

        public void TryExtinguish(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_OnFireData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[entity];
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_OnFireData[entity].m_Intensity <= 0.0)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float structuralIntegrity = this.m_StructuralIntegrityData.GetStructuralIntegrity(prefabRef.m_Prefab, this.m_BuildingData.HasComponent(entity));
            // ISSUE: reference to a compiler-generated field
            float num = (float) (0.26666668057441711 * (double) this.m_ExtinguishRate);
            float waterDamageDelta = num * 10f / structuralIntegrity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ExtinguishingQueue.Enqueue(new FireEngineAISystem.FireExtinguishing(this.m_Vehicle, entity, Entity.Null, -num, waterDamageDelta, 0.0f));
            // ISSUE: reference to a compiler-generated field
            if (!(this.m_ExtinguishResult == Entity.Null))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_ExtinguishResult = entity;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DestroyedData.HasComponent(entity))
              return;
            // ISSUE: reference to a compiler-generated field
            Destroyed destroyed = this.m_DestroyedData[entity];
            if ((double) destroyed.m_Cleared < 0.0 || (double) destroyed.m_Cleared >= 1.0)
              return;
            // ISSUE: reference to a compiler-generated field
            float destroyedClearDelta = 0.266666681f * this.m_ClearRate;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_ExtinguishingQueue.Enqueue(new FireEngineAISystem.FireExtinguishing(this.m_Vehicle, entity, Entity.Null, 0.0f, 0.0f, destroyedClearDelta));
            // ISSUE: reference to a compiler-generated field
            if (!(this.m_ClearResult == Entity.Null))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_ClearResult = entity;
          }
        }
      }
    }

    [BurstCompile]
    private struct FireExtinguishingJob : IJob
    {
      public ComponentLookup<OnFire> m_OnFireData;
      public ComponentLookup<RescueTarget> m_RescueTargetData;
      public ComponentLookup<Damaged> m_DamagedData;
      public ComponentLookup<Destroyed> m_DestroyedData;
      public NativeQueue<FireEngineAISystem.FireExtinguishing> m_ExtinguishingQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        FireEngineAISystem.FireExtinguishing fireExtinguishing;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ExtinguishingQueue.TryDequeue(out fireExtinguishing))
        {
          // ISSUE: reference to a compiler-generated field
          if (fireExtinguishing.m_Request != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_OnFireData.HasComponent(fireExtinguishing.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              OnFire onFire = this.m_OnFireData[fireExtinguishing.m_Target] with
              {
                m_RescueRequest = fireExtinguishing.m_Request
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OnFireData[fireExtinguishing.m_Target] = onFire;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_RescueTargetData.HasComponent(fireExtinguishing.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              RescueTarget rescueTarget = this.m_RescueTargetData[fireExtinguishing.m_Target] with
              {
                m_Request = fireExtinguishing.m_Request
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_RescueTargetData[fireExtinguishing.m_Target] = rescueTarget;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) fireExtinguishing.m_FireIntensityDelta != 0.0 && this.m_OnFireData.HasComponent(fireExtinguishing.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            OnFire onFire = this.m_OnFireData[fireExtinguishing.m_Target];
            // ISSUE: reference to a compiler-generated field
            onFire.m_Intensity = math.max(0.0f, onFire.m_Intensity + fireExtinguishing.m_FireIntensityDelta);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OnFireData[fireExtinguishing.m_Target] = onFire;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) fireExtinguishing.m_WaterDamageDelta != 0.0 && this.m_DamagedData.HasComponent(fireExtinguishing.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Damaged damaged = this.m_DamagedData[fireExtinguishing.m_Target];
            if ((double) damaged.m_Damage.z < 0.5)
            {
              // ISSUE: reference to a compiler-generated field
              damaged.m_Damage.z = math.min(0.5f, damaged.m_Damage.z + fireExtinguishing.m_WaterDamageDelta);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_DamagedData[fireExtinguishing.m_Target] = damaged;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) fireExtinguishing.m_DestroyedClearDelta != 0.0 && this.m_DestroyedData.HasComponent(fireExtinguishing.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Destroyed destroyed = this.m_DestroyedData[fireExtinguishing.m_Target];
            // ISSUE: reference to a compiler-generated field
            destroyed.m_Cleared = math.min(1f, destroyed.m_Cleared + fireExtinguishing.m_DestroyedClearDelta);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_DestroyedData[fireExtinguishing.m_Target] = destroyed;
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
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.FireEngine> __Game_Vehicles_FireEngine_RW_ComponentTypeHandle;
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
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
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
      public ComponentLookup<FireEngineData> __Game_Prefabs_FireEngineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> __Game_Simulation_FireRescueRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RescueTarget> __Game_Buildings_RescueTarget_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.FireStation> __Game_Buildings_FireStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public ComponentLookup<OnFire> __Game_Events_OnFire_RW_ComponentLookup;
      public ComponentLookup<RescueTarget> __Game_Buildings_RescueTarget_RW_ComponentLookup;
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RW_ComponentLookup;
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RW_ComponentLookup;

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
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_FireEngine_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.FireEngine>();
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
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
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
        this.__Game_Prefabs_FireEngineData_RO_ComponentLookup = state.GetComponentLookup<FireEngineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_FireRescueRequest_RO_ComponentLookup = state.GetComponentLookup<FireRescueRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RescueTarget_RO_ComponentLookup = state.GetComponentLookup<RescueTarget>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_FireStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.FireStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RW_ComponentLookup = state.GetComponentLookup<OnFire>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RescueTarget_RW_ComponentLookup = state.GetComponentLookup<RescueTarget>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentLookup = state.GetComponentLookup<Damaged>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RW_ComponentLookup = state.GetComponentLookup<Destroyed>();
      }
    }
  }
}
