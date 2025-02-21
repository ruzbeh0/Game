// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DeathcareFacilityAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
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
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DeathcareFacilityAISystem : GameSystemBase
  {
    private EntityQuery m_FacilityQuery;
    private EntityQuery m_HealthcareVehiclePrefabQuery;
    private EntityQuery m_HealthcareSettingsQuery;
    private EntityArchetype m_HealthcareRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_ParkedToMovingRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingCarAddTypes;
    private EndFrameBarrier m_EndFrameBarrier;
    private BudgetSystem m_BudgetSystem;
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private IconCommandSystem m_IconCommandSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private HealthcareVehicleSelectData m_HealthcareVehicleSelectData;
    private DeathcareFacilityAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 32;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_BudgetSystem = this.World.GetOrCreateSystemManaged<BudgetSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData = new HealthcareVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_FacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehiclePrefabQuery = this.GetEntityQuery(HealthcareVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<HealthcareRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<Updated>()
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FacilityQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HealthcareSettingsQuery);
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
      NativeQueue<DeathcareFacilityAISystem.DeathcareFacilityAction> nativeQueue = new NativeQueue<DeathcareFacilityAISystem.DeathcareFacilityAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
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
      this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DeathcareFacility_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DeathcareFacilityAISystem.DeathcareFacilityTickJob jobData1 = new DeathcareFacilityAISystem.DeathcareFacilityTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle,
        m_PatientType = this.__TypeHandle.__Game_Buildings_Patient_RW_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_DeathcareFacilityType = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RW_ComponentTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_PrefabDeathcareFacilityData = this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_HealthcareRequestData = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_HearseData = this.__TypeHandle.__Game_Vehicles_Hearse_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_HealthcareParameters = this.m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>(),
        m_HealthcareRequestArchetype = this.m_HealthcareRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_ParkedToMovingRemoveTypes = this.m_ParkedToMovingRemoveTypes,
        m_ParkedToMovingCarAddTypes = this.m_ParkedToMovingCarAddTypes,
        m_HealthcareVehicleSelectData = this.m_HealthcareVehicleSelectData,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Hearse_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DeathcareFacilityAISystem.DeathcareFacilityActionJob jobData2 = new DeathcareFacilityAISystem.DeathcareFacilityActionJob()
      {
        m_HearseData = this.__TypeHandle.__Game_Vehicles_Hearse_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData1.ScheduleParallel<DeathcareFacilityAISystem.DeathcareFacilityTickJob>(this.m_FacilityQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.Schedule<DeathcareFacilityAISystem.DeathcareFacilityActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle2);
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
    public DeathcareFacilityAISystem()
    {
    }

    private struct DeathcareFacilityAction
    {
      public Entity m_Entity;
      public bool m_Disabled;

      public static DeathcareFacilityAISystem.DeathcareFacilityAction SetDisabled(
        Entity vehicle,
        bool disabled)
      {
        // ISSUE: object of a compiler-generated type is created
        return new DeathcareFacilityAISystem.DeathcareFacilityAction()
        {
          m_Entity = vehicle,
          m_Disabled = disabled
        };
      }
    }

    [BurstCompile]
    private struct DeathcareFacilityTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public ComponentTypeHandle<Game.Buildings.DeathcareFacility> m_DeathcareFacilityType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      public BufferTypeHandle<Patient> m_PatientType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> m_PrefabDeathcareFacilityData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Hearse> m_HearseData;
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
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public HealthcareParameterData m_HealthcareParameters;
      [ReadOnly]
      public EntityArchetype m_HealthcareRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingCarAddTypes;
      [ReadOnly]
      public HealthcareVehicleSelectData m_HealthcareVehicleSelectData;
      public IconCommandBuffer m_IconCommandBuffer;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<DeathcareFacilityAISystem.DeathcareFacilityAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<Game.Buildings.DeathcareFacility> nativeArray3 = chunk.GetNativeArray<Game.Buildings.DeathcareFacility>(ref this.m_DeathcareFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor2 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Patient> bufferAccessor3 = chunk.GetBufferAccessor<Patient>(ref this.m_PatientType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor4 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor5 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          Game.Buildings.DeathcareFacility facility = nativeArray3[index];
          DynamicBuffer<OwnedVehicle> vehicles = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> dispatches = bufferAccessor4[index];
          DynamicBuffer<Patient> patients = new DynamicBuffer<Patient>();
          if (bufferAccessor3.Length != 0)
            patients = bufferAccessor3[index];
          DeathcareFacilityData data = new DeathcareFacilityData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabDeathcareFacilityData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            data = this.m_PrefabDeathcareFacilityData[prefabRef.m_Prefab];
          }
          if (bufferAccessor5.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<DeathcareFacilityData>(ref data, bufferAccessor5[index], ref this.m_PrefabRefDataFromEntity, ref this.m_PrefabDeathcareFacilityData);
          }
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          float immediateEfficiency = BuildingUtils.GetImmediateEfficiency(bufferAccessor1, index);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, ref facility, data, vehicles, patients, dispatches, efficiency, immediateEfficiency);
          nativeArray3[index] = facility;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.DeathcareFacility facility,
        DeathcareFacilityData prefabDeathcareFacilityData,
        DynamicBuffer<OwnedVehicle> vehicles,
        DynamicBuffer<Patient> patients,
        DynamicBuffer<ServiceDispatch> dispatches,
        float efficiency,
        float immediateEfficiency)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(entity.Index);
        int vehicleCapacity1 = BuildingUtils.GetVehicleCapacity(efficiency, prefabDeathcareFacilityData.m_HearseCapacity);
        int vehicleCapacity2 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabDeathcareFacilityData.m_HearseCapacity);
        int availableVehicles = vehicleCapacity1;
        facility.m_ProcessingState += (float) ((double) efficiency * (double) prefabDeathcareFacilityData.m_ProcessingRate * 0.0009765625);
        StackList<Entity> parkedVehicles = (StackList<Entity>) stackalloc Entity[vehicles.Length];
        for (int index = 0; index < vehicles.Length; ++index)
        {
          Entity vehicle = vehicles[index].m_Vehicle;
          Game.Vehicles.Hearse componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HearseData.TryGetComponent(vehicle, out componentData1))
          {
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
              else
                parkedVehicles.AddNoResize(vehicle);
            }
            else
            {
              --availableVehicles;
              bool disabled = --vehicleCapacity2 < 0;
              if ((componentData1.m_State & HearseFlags.Disabled) > (HearseFlags) 0 != disabled)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ActionQueue.Enqueue(DeathcareFacilityAISystem.DeathcareFacilityAction.SetDisabled(vehicle, disabled));
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
            if (componentData.m_Type == HealthcareRequestType.Hearse)
            {
              // ISSUE: reference to a compiler-generated method
              this.SpawnVehicle(jobIndex, ref random, entity, request, ref facility, ref availableVehicles, ref parkedVehicles);
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
        while (parkedVehicles.Length > math.max(0, prefabDeathcareFacilityData.m_HearseCapacity + availableVehicles - vehicleCapacity1))
        {
          int index2 = random.NextInt(parkedVehicles.Length);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, parkedVehicles[index2]);
          parkedVehicles.RemoveAtSwapBack(index2);
        }
        for (int index3 = 0; index3 < parkedVehicles.Length; ++index3)
        {
          Entity entity1 = parkedVehicles[index3];
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.Hearse hearse = this.m_HearseData[entity1];
          bool disabled = availableVehicles <= 0;
          if ((hearse.m_State & HearseFlags.Disabled) > (HearseFlags) 0 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(DeathcareFacilityAISystem.DeathcareFacilityAction.SetDisabled(entity1, disabled));
          }
        }
        facility.m_Flags &= ~(DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies | DeathcareFacilityFlags.CanProcessCorpses | DeathcareFacilityFlags.CanStoreCorpses);
        if (availableVehicles != 0)
          facility.m_Flags |= DeathcareFacilityFlags.HasAvailableHearses;
        if ((double) prefabDeathcareFacilityData.m_ProcessingRate > 0.0)
          facility.m_Flags |= DeathcareFacilityFlags.CanProcessCorpses;
        if (prefabDeathcareFacilityData.m_StorageCapacity > 0)
          facility.m_Flags |= DeathcareFacilityFlags.CanStoreCorpses;
        for (; facility.m_LongTermStoredCount > 0 && (double) facility.m_ProcessingState >= 1.0; --facility.m_LongTermStoredCount)
          --facility.m_ProcessingState;
        if (patients.IsCreated)
        {
          int index4 = 0;
          while (index4 < patients.Length)
          {
            Entity patient = patients[index4].m_Patient;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefabRefDataFromEntity.HasComponent(patient))
              patients.RemoveAt(index4);
            else if ((double) facility.m_ProcessingState >= 1.0)
            {
              --facility.m_ProcessingState;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, patient, new Deleted());
              patients.RemoveAt(index4);
            }
            else if (prefabDeathcareFacilityData.m_LongTermStorage)
            {
              ++facility.m_LongTermStoredCount;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, patient, new Deleted());
              patients.RemoveAt(index4);
            }
            else
              ++index4;
          }
          int num = facility.m_LongTermStoredCount + patients.Length;
          if (num < prefabDeathcareFacilityData.m_StorageCapacity)
            facility.m_Flags |= DeathcareFacilityFlags.HasRoomForBodies;
          if (num == 0)
            facility.m_ProcessingState = 0.0f;
          if (prefabDeathcareFacilityData.m_LongTermStorage)
          {
            if (num >= prefabDeathcareFacilityData.m_StorageCapacity)
            {
              if ((facility.m_Flags & DeathcareFacilityFlags.IsFull) == (DeathcareFacilityFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(entity, this.m_HealthcareParameters.m_FacilityFullNotificationPrefab);
                facility.m_Flags |= DeathcareFacilityFlags.IsFull;
              }
            }
            else if ((facility.m_Flags & DeathcareFacilityFlags.IsFull) != (DeathcareFacilityFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(entity, this.m_HealthcareParameters.m_FacilityFullNotificationPrefab);
              facility.m_Flags &= ~DeathcareFacilityFlags.IsFull;
            }
          }
        }
        else
          facility.m_ProcessingState = 0.0f;
        if ((facility.m_Flags & (DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies)) != (DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies))
          return;
        // ISSUE: reference to a compiler-generated method
        this.RequestTargetIfNeeded(jobIndex, entity, ref facility);
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.DeathcareFacility deathcareFacility)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(deathcareFacility.m_TargetRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HealthcareRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HealthcareRequest>(jobIndex, entity1, new HealthcareRequest(entity, HealthcareRequestType.Hearse));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(16U));
      }

      private void SpawnVehicle(
        int jobIndex,
        ref Random random,
        Entity entity,
        Entity request,
        ref Game.Buildings.DeathcareFacility deathcareFacility,
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
          Game.Vehicles.CarLaneFlags flags = Game.Vehicles.CarLaneFlags.EndReached | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(jobIndex, entity2, in this.m_ParkedToMovingCarAddTypes);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<CarCurrentLane>(jobIndex, entity2, new CarCurrentLane(parkedCar, flags));
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
          entity2 = this.m_HealthcareVehicleSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, this.m_TransformData[entity], entity, Entity.Null, componentData1.m_Type, RoadTypes.Car, false);
          if (entity2 == Entity.Null)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, new Owner(entity));
        }
        --availableVehicles;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Vehicles.Hearse>(jobIndex, entity2, new Game.Vehicles.Hearse(citizen, HearseFlags.Dispatched));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Target>(jobIndex, entity2, new Target(entity1));
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
        if (!this.m_ServiceRequestData.HasComponent(deathcareFacility.m_TargetRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity4 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity4, new HandleRequest(deathcareFacility.m_TargetRequest, Entity.Null, true));
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
    private struct DeathcareFacilityActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.Hearse> m_HearseData;
      public NativeQueue<DeathcareFacilityAISystem.DeathcareFacilityAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        DeathcareFacilityAISystem.DeathcareFacilityAction deathcareFacilityAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out deathcareFacilityAction))
        {
          Game.Vehicles.Hearse componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_HearseData.TryGetComponent(deathcareFacilityAction.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            if (deathcareFacilityAction.m_Disabled)
              componentData.m_State |= HearseFlags.Disabled;
            else
              componentData.m_State &= ~HearseFlags.Disabled;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_HearseData[deathcareFacilityAction.m_Entity] = componentData;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle;
      public BufferTypeHandle<Patient> __Game_Buildings_Patient_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Buildings.DeathcareFacility> __Game_Buildings_DeathcareFacility_RW_ComponentTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> __Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Hearse> __Game_Vehicles_Hearse_RO_ComponentLookup;
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
      public ComponentLookup<Game.Vehicles.Hearse> __Game_Vehicles_Hearse_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RW_BufferTypeHandle = state.GetBufferTypeHandle<Patient>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DeathcareFacility_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.DeathcareFacility>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup = state.GetComponentLookup<DeathcareFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentLookup = state.GetComponentLookup<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Hearse_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Hearse>(true);
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
        this.__Game_Vehicles_Hearse_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Hearse>();
      }
    }
  }
}
