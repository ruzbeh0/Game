// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HospitalAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Game.Vehicles;
using System;
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
  public class HospitalAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private CitySystem m_CitySystem;
    private EntityQuery m_HospitalQuery;
    private EntityQuery m_HealthcareVehiclePrefabQuery;
    private EntityQuery m_HealthcareParameterQuery;
    private EntityArchetype m_HealthcareRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private EntityArchetype m_ResetTripArchetype;
    private ComponentTypeSet m_ParkedToMovingRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingCarAddTypes;
    private ComponentTypeSet m_ParkedToMovingAircraftAddTypes;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private HealthcareVehicleSelectData m_HealthcareVehicleSelectData;
    private HospitalAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData = new HealthcareVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_HospitalQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Hospital>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<HealthcareRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResetTripArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<ResetTrip>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehiclePrefabQuery = this.GetEntityQuery(HealthcareVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingRemoveTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<Stopped>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkedToMovingCarAddTypes = new ComponentTypeSet(new ComponentType[14]
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
      this.m_ParkedToMovingAircraftAddTypes = new ComponentTypeSet(new ComponentType[13]
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
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HospitalQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_HealthcareVehiclePrefabQuery, Allocator.TempJob, out jobHandle1);
      NativeQueue<HospitalAISystem.HospitalAction> nativeQueue = new NativeQueue<HospitalAISystem.HospitalAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HospitalAISystem.HospitalTickJob jobData1 = new HospitalAISystem.HospitalTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_ResourceConsumerType = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_PatientType = this.__TypeHandle.__Game_Buildings_Patient_RW_BufferTypeHandle,
        m_HospitalType = this.__TypeHandle.__Game_Buildings_Hospital_RW_ComponentTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_ServiceUsageType = this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_PrefabHospitalData = this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_HealthcareRequestData = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_AmbulanceData = this.__TypeHandle.__Game_Vehicles_Ambulance_RO_ComponentLookup,
        m_HelicopterData = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_HealthProblemData = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_HealthcareRequestArchetype = this.m_HealthcareRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_ResetTripArchetype = this.m_ResetTripArchetype,
        m_ParkedToMovingRemoveTypes = this.m_ParkedToMovingRemoveTypes,
        m_ParkedToMovingCarAddTypes = this.m_ParkedToMovingCarAddTypes,
        m_ParkedToMovingAircraftAddTypes = this.m_ParkedToMovingAircraftAddTypes,
        m_HealthcareVehicleSelectData = this.m_HealthcareVehicleSelectData,
        m_HealthcareParameterData = this.m_HealthcareParameterQuery.GetSingleton<HealthcareParameterData>(),
        m_City = this.m_CitySystem.City,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HospitalAISystem.HospitalActionJob jobData2 = new HospitalAISystem.HospitalActionJob()
      {
        m_AmbulanceData = this.__TypeHandle.__Game_Vehicles_Ambulance_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData1.ScheduleParallel<HospitalAISystem.HospitalTickJob>(this.m_HospitalQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.Schedule<HospitalAISystem.HospitalActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData.PostUpdate(jobHandle2);
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
    public HospitalAISystem()
    {
    }

    private struct HospitalAction
    {
      public Entity m_Entity;
      public bool m_Disabled;

      public static HospitalAISystem.HospitalAction SetDisabled(Entity vehicle, bool disabled)
      {
        // ISSUE: object of a compiler-generated type is created
        return new HospitalAISystem.HospitalAction()
        {
          m_Entity = vehicle,
          m_Disabled = disabled
        };
      }
    }

    [BurstCompile]
    private struct HospitalTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ResourceConsumer> m_ResourceConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      public ComponentTypeHandle<Game.Buildings.Hospital> m_HospitalType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      public BufferTypeHandle<Patient> m_PatientType;
      public ComponentTypeHandle<ServiceUsage> m_ServiceUsageType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public ComponentLookup<HospitalData> m_PrefabHospitalData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Ambulance> m_AmbulanceData;
      [ReadOnly]
      public ComponentLookup<Helicopter> m_HelicopterData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_HealthcareRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_ResetTripArchetype;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingCarAddTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingAircraftAddTypes;
      [ReadOnly]
      public HealthcareVehicleSelectData m_HealthcareVehicleSelectData;
      [ReadOnly]
      public HealthcareParameterData m_HealthcareParameterData;
      [ReadOnly]
      public Entity m_City;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<HospitalAISystem.HospitalAction>.ParallelWriter m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.Hospital> nativeArray3 = chunk.GetNativeArray<Game.Buildings.Hospital>(ref this.m_HospitalType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.ResourceConsumer> nativeArray4 = chunk.GetNativeArray<Game.Buildings.ResourceConsumer>(ref this.m_ResourceConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor2 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor3 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Patient> bufferAccessor4 = chunk.GetBufferAccessor<Patient>(ref this.m_PatientType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor5 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceUsage> nativeArray5 = chunk.GetNativeArray<ServiceUsage>(ref this.m_ServiceUsageType);
        // ISSUE: reference to a compiler-generated field
        bool outside = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
        Span<float> span = stackalloc float[28];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          ref Game.Buildings.Hospital local = ref nativeArray3.ElementAt<Game.Buildings.Hospital>(index);
          DynamicBuffer<OwnedVehicle> vehicles = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> dispatches = bufferAccessor5[index];
          byte resourceAvailability = nativeArray4.Length != 0 ? nativeArray4[index].m_ResourceAvailability : byte.MaxValue;
          DynamicBuffer<Patient> patients = new DynamicBuffer<Patient>();
          if (bufferAccessor4.Length != 0)
            patients = bufferAccessor4[index];
          HospitalData componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_PrefabHospitalData.TryGetComponent(prefabRef.m_Prefab, out componentData);
          if (bufferAccessor3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<HospitalData>(ref componentData, bufferAccessor3[index], ref this.m_PrefabRefData, ref this.m_PrefabHospitalData);
          }
          if (bufferAccessor1.Length != 0)
            BuildingUtils.GetEfficiencyFactors(bufferAccessor1[index], span);
          else
            span.Fill(1f);
          float immediateEfficiency = BuildingUtils.GetImmediateEfficiency(bufferAccessor1, index);
          ServiceUsage usage;
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, ref random, ref local, out usage, componentData, vehicles, patients, dispatches, span, immediateEfficiency, resourceAvailability, cityModifier, outside);
          if (bufferAccessor1.Length != 0)
            BuildingUtils.SetEfficiencyFactors(bufferAccessor1[index], span);
          if (nativeArray5.Length != 0)
            nativeArray5[index] = usage;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        ref Unity.Mathematics.Random random,
        ref Game.Buildings.Hospital hospital,
        out ServiceUsage usage,
        HospitalData prefabHospitalData,
        DynamicBuffer<OwnedVehicle> vehicles,
        DynamicBuffer<Patient> patients,
        DynamicBuffer<ServiceDispatch> dispatches,
        Span<float> efficiencyFactors,
        float immediateEfficiency,
        byte resourceAvailability,
        DynamicBuffer<CityModifier> cityModifiers,
        bool outside)
      {
        bool canCurePatients = false;
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        float num1 = resourceAvailability > (byte) 0 ? 1f : 1f - this.m_HealthcareParameterData.m_NoResourceTreatmentPenalty;
        if (!outside)
        {
          efficiencyFactors[17] = num1;
          float num2 = 1f;
          CityUtils.ApplyModifier(ref num2, cityModifiers, CityModifierType.HospitalEfficiency);
          efficiencyFactors[26] = num2;
        }
        float efficiency = BuildingUtils.GetEfficiency(efficiencyFactors);
        if (hospital.m_TreatmentBonus != (byte) 0 && (double) math.abs(efficiency - 0.0f) > 1.4012984643248171E-45)
        {
          canCurePatients = prefabHospitalData.m_TreatDiseases;
          flag1 = prefabHospitalData.m_TreatInjuries;
        }
        int vehicleCapacity1 = BuildingUtils.GetVehicleCapacity(efficiency, prefabHospitalData.m_AmbulanceCapacity);
        int vehicleCapacity2 = BuildingUtils.GetVehicleCapacity(efficiency, prefabHospitalData.m_MedicalHelicopterCapacity);
        int vehicleCapacity3 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabHospitalData.m_AmbulanceCapacity);
        int vehicleCapacity4 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabHospitalData.m_MedicalHelicopterCapacity);
        int availableVehicles1 = vehicleCapacity1;
        int availableVehicles2 = vehicleCapacity2;
        StackList<Entity> parkedVehicles1 = (StackList<Entity>) stackalloc Entity[vehicles.Length];
        StackList<Entity> parkedVehicles2 = (StackList<Entity>) stackalloc Entity[vehicles.Length];
        hospital.m_TreatmentBonus = (byte) math.min((int) byte.MaxValue, Mathf.RoundToInt(efficiency * num1 * (float) prefabHospitalData.m_TreatmentBonus));
        hospital.m_MinHealth = (byte) prefabHospitalData.m_HealthRange.x;
        hospital.m_MaxHealth = (byte) prefabHospitalData.m_HealthRange.y;
        for (int index = 0; index < vehicles.Length; ++index)
        {
          Entity vehicle = vehicles[index].m_Vehicle;
          Game.Vehicles.Ambulance componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AmbulanceData.TryGetComponent(vehicle, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            bool flag2 = this.m_HelicopterData.HasComponent(vehicle);
            ParkedCar componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedCarData.TryGetComponent(vehicle, out componentData2))
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_EntityLookup.Exists(componentData2.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicle);
              }
              else if (flag2)
                parkedVehicles2.AddNoResize(vehicle);
              else
                parkedVehicles1.AddNoResize(vehicle);
            }
            else
            {
              bool disabled;
              if (flag2)
              {
                --availableVehicles2;
                disabled = --vehicleCapacity4 < 0;
              }
              else
              {
                --availableVehicles1;
                disabled = --vehicleCapacity3 < 0;
              }
              if ((componentData1.m_State & AmbulanceFlags.Disabled) > (AmbulanceFlags) 0 != disabled)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ActionQueue.Enqueue(HospitalAISystem.HospitalAction.SetDisabled(vehicle, disabled));
              }
            }
          }
        }
        int index1 = 0;
        while (index1 < dispatches.Length)
        {
          Entity request = dispatches[index1].m_Request;
          HealthcareRequest componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HealthcareRequestData.TryGetComponent(request, out componentData))
          {
            if (componentData.m_Type == HealthcareRequestType.Ambulance)
            {
              // ISSUE: reference to a compiler-generated method
              RoadTypes roadType = this.CheckPathType(request);
              switch (roadType)
              {
                case RoadTypes.Car:
                  // ISSUE: reference to a compiler-generated method
                  this.SpawnVehicle(jobIndex, ref random, entity, request, roadType, canCurePatients, ref hospital, ref availableVehicles1, ref parkedVehicles1);
                  break;
                case RoadTypes.Helicopter:
                  // ISSUE: reference to a compiler-generated method
                  this.SpawnVehicle(jobIndex, ref random, entity, request, roadType, canCurePatients, ref hospital, ref availableVehicles2, ref parkedVehicles2);
                  break;
              }
              dispatches.RemoveAt(index1);
            }
            else
              ++index1;
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
        while (parkedVehicles1.Length > math.max(0, prefabHospitalData.m_AmbulanceCapacity + availableVehicles1 - vehicleCapacity1))
        {
          int index2 = random.NextInt(parkedVehicles1.Length);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, parkedVehicles1[index2]);
          parkedVehicles1.RemoveAtSwapBack(index2);
        }
        while (parkedVehicles2.Length > math.max(0, prefabHospitalData.m_MedicalHelicopterCapacity + availableVehicles2 - vehicleCapacity2))
        {
          int index3 = random.NextInt(parkedVehicles2.Length);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, parkedVehicles2[index3]);
          parkedVehicles2.RemoveAtSwapBack(index3);
        }
        for (int index4 = 0; index4 < parkedVehicles1.Length; ++index4)
        {
          Entity entity1 = parkedVehicles1[index4];
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.Ambulance ambulance = this.m_AmbulanceData[entity1];
          bool disabled = availableVehicles1 <= 0;
          if ((ambulance.m_State & AmbulanceFlags.Disabled) > (AmbulanceFlags) 0 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(HospitalAISystem.HospitalAction.SetDisabled(entity1, disabled));
          }
        }
        for (int index5 = 0; index5 < parkedVehicles2.Length; ++index5)
        {
          Entity entity2 = parkedVehicles2[index5];
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.Ambulance ambulance = this.m_AmbulanceData[entity2];
          bool disabled = availableVehicles2 <= 0;
          if ((ambulance.m_State & AmbulanceFlags.Disabled) > (AmbulanceFlags) 0 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(HospitalAISystem.HospitalAction.SetDisabled(entity2, disabled));
          }
        }
        hospital.m_Flags &= ~(HospitalFlags.HasAvailableAmbulances | HospitalFlags.HasAvailableMedicalHelicopters | HospitalFlags.CanCureDisease | HospitalFlags.HasRoomForPatients | HospitalFlags.CanProcessCorpses | HospitalFlags.CanCureInjury);
        if (availableVehicles1 != 0)
          hospital.m_Flags |= HospitalFlags.HasAvailableAmbulances;
        if (availableVehicles2 != 0)
          hospital.m_Flags |= HospitalFlags.HasAvailableMedicalHelicopters;
        if (canCurePatients)
          hospital.m_Flags |= HospitalFlags.CanCureDisease;
        if (flag1)
          hospital.m_Flags |= HospitalFlags.CanCureInjury;
        if (prefabHospitalData.m_PatientCapacity > 0)
          hospital.m_Flags |= HospitalFlags.CanProcessCorpses;
        if (patients.IsCreated)
        {
          int index6 = patients.Length - 1;
          while (patients.Length > 0 && index6 >= 0)
          {
            Entity patient = patients[index6].m_Patient;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_HealthProblemData.HasComponent(patient))
            {
              patients.RemoveAt(index6);
              --index6;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              HealthProblem healthProblem = this.m_HealthProblemData[patient];
              if ((healthProblem.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, patient, new Deleted());
                patients.RemoveAt(index6);
                --index6;
              }
              else
              {
                if (!canCurePatients && (healthProblem.m_Flags & HealthProblemFlags.Sick) != HealthProblemFlags.None || !flag1 && (healthProblem.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<CurrentBuilding>(jobIndex, patient);
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CurrentTransportData.HasComponent(patient))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ResetTripArchetype);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<ResetTrip>(jobIndex, entity3, new ResetTrip()
                    {
                      m_Creature = this.m_CurrentTransportData[patient].m_CurrentTransport,
                      m_Target = Entity.Null
                    });
                  }
                  patients.RemoveAt(index6);
                }
                --index6;
              }
            }
          }
          if (patients.Length < prefabHospitalData.m_PatientCapacity)
            hospital.m_Flags |= HospitalFlags.HasRoomForPatients;
          usage.m_Usage = (float) patients.Length / math.max(1f, (float) prefabHospitalData.m_PatientCapacity);
        }
        else
          usage.m_Usage = 0.0f;
        if ((hospital.m_Flags & (HospitalFlags.HasAvailableAmbulances | HospitalFlags.HasAvailableMedicalHelicopters)) == (HospitalFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.RequestTargetIfNeeded(jobIndex, entity, ref hospital);
      }

      private void RequestTargetIfNeeded(int jobIndex, Entity entity, ref Game.Buildings.Hospital hospital)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(hospital.m_TargetRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HealthcareRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HealthcareRequest>(jobIndex, entity1, new HealthcareRequest(entity, HealthcareRequestType.Ambulance));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(16U));
      }

      private void SpawnVehicle(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Entity request,
        RoadTypes roadType,
        bool canCurePatients,
        ref Game.Buildings.Hospital hospital,
        ref int availableVehicles,
        ref StackList<Entity> parkedVehicles)
      {
        HealthcareRequest componentData1;
        // ISSUE: reference to a compiler-generated field
        if (availableVehicles <= 0 || !this.m_HealthcareRequestData.TryGetComponent(request, out componentData1))
          return;
        Entity citizen = componentData1.m_Citizen;
        Entity entity1 = Entity.Null;
        CurrentTransport componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentTransportData.TryGetComponent(citizen, out componentData2))
        {
          entity1 = componentData2.m_CurrentTransport;
        }
        else
        {
          CurrentBuilding componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentBuildingData.TryGetComponent(citizen, out componentData3))
            entity1 = componentData3.m_CurrentBuilding;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(entity1))
          return;
        Entity entity2 = Entity.Null;
        PathInformation componentData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathInformationData.TryGetComponent(request, out componentData4) && componentData4.m_Origin != entity)
        {
          if (!CollectionUtils.RemoveValueSwapBack<Entity>(ref parkedVehicles, componentData4.m_Origin))
            return;
          // ISSUE: reference to a compiler-generated field
          ParkedCar parkedCar = this.m_ParkedCarData[componentData4.m_Origin];
          entity2 = componentData4.m_Origin;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent(jobIndex, entity2, in this.m_ParkedToMovingRemoveTypes);
          switch (roadType)
          {
            case RoadTypes.Car:
              Game.Vehicles.CarLaneFlags flags1 = Game.Vehicles.CarLaneFlags.EndReached | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_ParkedToMovingCarAddTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CarCurrentLane>(jobIndex, entity2, new CarCurrentLane(parkedCar, flags1));
              break;
            case RoadTypes.Helicopter:
              AircraftLaneFlags flags2 = AircraftLaneFlags.EndReached | AircraftLaneFlags.TransformTarget | AircraftLaneFlags.ParkingSpace;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_ParkedToMovingAircraftAddTypes);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<AircraftCurrentLane>(jobIndex, entity2, new AircraftCurrentLane(parkedCar, flags2));
              break;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.HasComponent(parkedCar.m_Lane) || this.m_SpawnLocationData.HasComponent(parkedCar.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, parkedCar.m_Lane);
          }
        }
        if (entity2 == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          entity2 = this.m_HealthcareVehicleSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, this.m_TransformData[entity], entity, Entity.Null, componentData1.m_Type, roadType, false);
          if (entity2 == Entity.Null)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, new Owner(entity));
        }
        --availableVehicles;
        AmbulanceFlags state = (AmbulanceFlags) (2 | 8);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Vehicles.Ambulance>(jobIndex, entity2, new Game.Vehicles.Ambulance(citizen, entity1, state));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Common.Target>(jobIndex, entity2, new Game.Common.Target(entity1));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<ServiceDispatch>(jobIndex, entity2).Add(new ServiceDispatch(request));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(request, entity2, false));
        DynamicBuffer<PathElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> targetElements = this.m_CommandBuffer.SetBuffer<PathElement>(jobIndex, entity2);
          PathUtils.CopyPath(bufferData, new PathOwner(), 0, targetElements);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathOwner>(jobIndex, entity2, new PathOwner(PathFlags.Updated));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, entity2, componentData4);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ServiceRequestData.HasComponent(hospital.m_TargetRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity4 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity4, new HandleRequest(hospital.m_TargetRequest, Entity.Null, true));
      }

      private RoadTypes CheckPathType(Entity request)
      {
        DynamicBuffer<PathElement> bufferData;
        PrefabRef componentData1;
        Game.Prefabs.SpawnLocationData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length >= 1 && this.m_PrefabRefData.TryGetComponent(bufferData[0].m_Target, out componentData1) && this.m_PrefabSpawnLocationData.TryGetComponent(componentData1.m_Prefab, out componentData2) ? componentData2.m_RoadTypes : RoadTypes.Car;
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
    private struct HospitalActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.Ambulance> m_AmbulanceData;
      public NativeQueue<HospitalAISystem.HospitalAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        HospitalAISystem.HospitalAction hospitalAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out hospitalAction))
        {
          Game.Vehicles.Ambulance componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AmbulanceData.TryGetComponent(hospitalAction.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            if (hospitalAction.m_Disabled)
              componentData.m_State |= AmbulanceFlags.Disabled;
            else
              componentData.m_State &= ~AmbulanceFlags.Disabled;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AmbulanceData[hospitalAction.m_Entity] = componentData;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.ResourceConsumer> __Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      public BufferTypeHandle<Patient> __Game_Buildings_Patient_RW_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.Hospital> __Game_Buildings_Hospital_RW_ComponentTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      public ComponentTypeHandle<ServiceUsage> __Game_Buildings_ServiceUsage_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<HospitalData> __Game_Prefabs_HospitalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Ambulance> __Game_Vehicles_Ambulance_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.Ambulance> __Game_Vehicles_Ambulance_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ResourceConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RW_BufferTypeHandle = state.GetBufferTypeHandle<Patient>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Hospital>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUsage_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceUsage>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HospitalData_RO_ComponentLookup = state.GetComponentLookup<HospitalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentLookup = state.GetComponentLookup<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Ambulance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentLookup = state.GetComponentLookup<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Ambulance>();
      }
    }
  }
}
