// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PostFacilityAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
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
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PostFacilityAISystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 1024;
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_PostVanPrefabQuery;
    private EntityQuery m_PostConfigurationQuery;
    private EntityArchetype m_MailTransferRequestArchetype;
    private EntityArchetype m_PostVanRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_ParkedToMovingRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingCarAddTypes;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private PostVanSelectData m_PostVanSelectData;
    private PostFacilityAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 176;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanSelectData = new PostVanSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.PostFacility>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanPrefabQuery = this.GetEntityQuery(PostVanSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_PostConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PostConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_MailTransferRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<MailTransferRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<PostVanRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
      this.RequireForUpdate(this.m_BuildingQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PostConfigurationQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_PostVanPrefabQuery, Allocator.TempJob, out jobHandle1);
      NativeQueue<PostFacilityAISystem.PostFacilityAction> nativeQueue = new NativeQueue<PostFacilityAISystem.PostFacilityAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PostVanData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MailBoxData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ReturnLoad_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PostVan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MailTransferRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_GuestVehicle_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_MailBox_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PostFacility_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
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
      PostFacilityAISystem.PostFacilityTickJob jobData1 = new PostFacilityAISystem.PostFacilityTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_PostFacilityType = this.__TypeHandle.__Game_Buildings_PostFacility_RW_ComponentTypeHandle,
        m_MailBoxType = this.__TypeHandle.__Game_Routes_MailBox_RW_ComponentTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle,
        m_GuestVehicleType = this.__TypeHandle.__Game_Vehicles_GuestVehicle_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_PostVanRequestData = this.__TypeHandle.__Game_Simulation_PostVanRequest_RO_ComponentLookup,
        m_MailTransferRequestData = this.__TypeHandle.__Game_Simulation_MailTransferRequest_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_PostVanData = this.__TypeHandle.__Game_Vehicles_PostVan_RO_ComponentLookup,
        m_DeliveryTruckData = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_ReturnLoadData = this.__TypeHandle.__Game_Vehicles_ReturnLoad_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabPostFacilityData = this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentLookup,
        m_PrefabMailBoxData = this.__TypeHandle.__Game_Prefabs_MailBoxData_RO_ComponentLookup,
        m_PrefabPostVanData = this.__TypeHandle.__Game_Prefabs_PostVanData_RO_ComponentLookup,
        m_PrefabDeliveryTruckData = this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_PostVanSelectData = this.m_PostVanSelectData,
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData(),
        m_PostConfigurationData = this.m_PostConfigurationQuery.GetSingleton<PostConfigurationData>(),
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_PostVanRequestArchetype = this.m_PostVanRequestArchetype,
        m_MailTransferRequestArchetype = this.m_MailTransferRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_ParkedToMovingRemoveTypes = this.m_ParkedToMovingRemoveTypes,
        m_ParkedToMovingCarAddTypes = this.m_ParkedToMovingCarAddTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PostVan_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PostFacilityAISystem.PostFacilityActionJob jobData2 = new PostFacilityAISystem.PostFacilityActionJob()
      {
        m_PostVanData = this.__TypeHandle.__Game_Vehicles_PostVan_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData1.ScheduleParallel<PostFacilityAISystem.PostFacilityTickJob>(this.m_BuildingQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.Schedule<PostFacilityAISystem.PostFacilityActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanSelectData.PostUpdate(jobHandle2);
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
    public PostFacilityAISystem()
    {
    }

    private struct PostFacilityAction
    {
      public Entity m_Entity;
      public bool m_Disabled;

      public static PostFacilityAISystem.PostFacilityAction SetDisabled(
        Entity vehicle,
        bool disabled)
      {
        // ISSUE: object of a compiler-generated type is created
        return new PostFacilityAISystem.PostFacilityAction()
        {
          m_Entity = vehicle,
          m_Disabled = disabled
        };
      }
    }

    [BurstCompile]
    private struct PostFacilityTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public ComponentTypeHandle<Game.Buildings.PostFacility> m_PostFacilityType;
      public ComponentTypeHandle<Game.Routes.MailBox> m_MailBoxType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourcesType;
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      public BufferTypeHandle<GuestVehicle> m_GuestVehicleType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> m_PostVanRequestData;
      [ReadOnly]
      public ComponentLookup<MailTransferRequest> m_MailTransferRequestData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PostVan> m_PostVanData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<ReturnLoad> m_ReturnLoadData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PostFacilityData> m_PrefabPostFacilityData;
      [ReadOnly]
      public ComponentLookup<MailBoxData> m_PrefabMailBoxData;
      [ReadOnly]
      public ComponentLookup<PostVanData> m_PrefabPostVanData;
      [ReadOnly]
      public ComponentLookup<DeliveryTruckData> m_PrefabDeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public PostVanSelectData m_PostVanSelectData;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;
      [ReadOnly]
      public PostConfigurationData m_PostConfigurationData;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public EntityArchetype m_PostVanRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_MailTransferRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingCarAddTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<PostFacilityAISystem.PostFacilityAction>.ParallelWriter m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.PostFacility> nativeArray3 = chunk.GetNativeArray<Game.Buildings.PostFacility>(ref this.m_PostFacilityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Routes.MailBox> nativeArray4 = chunk.GetNativeArray<Game.Routes.MailBox>(ref this.m_MailBoxType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor3 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<GuestVehicle> bufferAccessor4 = chunk.GetBufferAccessor<GuestVehicle>(ref this.m_GuestVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor5 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Economy.Resources> bufferAccessor6 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourcesType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          Game.Buildings.PostFacility postFacility = nativeArray3[index];
          DynamicBuffer<OwnedVehicle> ownedVehicles = bufferAccessor3[index];
          DynamicBuffer<GuestVehicle> guestVehicles = bufferAccessor4[index];
          DynamicBuffer<ServiceDispatch> dispatches = bufferAccessor5[index];
          DynamicBuffer<Game.Economy.Resources> resources = bufferAccessor6[index];
          // ISSUE: reference to a compiler-generated field
          PostFacilityData data = this.m_PrefabPostFacilityData[prefabRef.m_Prefab];
          MailBoxData prefabMailBoxData = new MailBoxData();
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<PostFacilityData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabPostFacilityData);
          }
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          float immediateEfficiency = BuildingUtils.GetImmediateEfficiency(bufferAccessor1, index);
          Game.Routes.MailBox mailBox = new Game.Routes.MailBox();
          if (nativeArray4.Length != 0)
          {
            mailBox = nativeArray4[index];
            // ISSUE: reference to a compiler-generated field
            prefabMailBoxData = this.m_PrefabMailBoxData[prefabRef.m_Prefab];
          }
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, ref random, ref postFacility, ref mailBox, data, prefabMailBoxData, ownedVehicles, guestVehicles, dispatches, resources, efficiency, immediateEfficiency);
          nativeArray3[index] = postFacility;
          if (nativeArray4.Length != 0)
            nativeArray4[index] = mailBox;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        ref Unity.Mathematics.Random random,
        ref Game.Buildings.PostFacility postFacility,
        ref Game.Routes.MailBox mailBox,
        PostFacilityData prefabPostFacilityData,
        MailBoxData prefabMailBoxData,
        DynamicBuffer<OwnedVehicle> ownedVehicles,
        DynamicBuffer<GuestVehicle> guestVehicles,
        DynamicBuffer<ServiceDispatch> dispatches,
        DynamicBuffer<Game.Economy.Resources> resources,
        float efficiency,
        float immediateEfficiency)
      {
        int vehicleCapacity1 = BuildingUtils.GetVehicleCapacity(efficiency, prefabPostFacilityData.m_PostVanCapacity);
        int vehicleCapacity2 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabPostFacilityData.m_PostVanCapacity);
        int availableDeliveryVans = vehicleCapacity1;
        int vehicleCapacity3 = BuildingUtils.GetVehicleCapacity(efficiency, prefabPostFacilityData.m_PostTruckCapacity);
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        StackList<Entity> parkedPostVans = (StackList<Entity>) stackalloc Entity[ownedVehicles.Length];
        for (int index1 = 0; index1 < ownedVehicles.Length; ++index1)
        {
          Entity vehicle1 = ownedVehicles[index1].m_Vehicle;
          Game.Vehicles.PostVan componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PostVanData.TryGetComponent(vehicle1, out componentData1))
          {
            ParkedCar componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedCarData.TryGetComponent(vehicle1, out componentData2))
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_EntityLookup.Exists(componentData2.m_Lane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicle1);
              }
              else
                parkedPostVans.AddNoResize(vehicle1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PostVanData postVanData = this.m_PrefabPostVanData[this.m_PrefabRefData[vehicle1].m_Prefab];
              --availableDeliveryVans;
              num2 += componentData1.m_DeliveringMail;
              num1 += postVanData.m_MailCapacity;
              bool disabled = --vehicleCapacity2 < 0;
              if ((componentData1.m_State & PostVanFlags.Disabled) > (PostVanFlags) 0 != disabled)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ActionQueue.Enqueue(PostFacilityAISystem.PostFacilityAction.SetDisabled(vehicle1, disabled));
              }
            }
          }
          else
          {
            Game.Vehicles.DeliveryTruck componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeliveryTruckData.TryGetComponent(vehicle1, out componentData3))
            {
              if ((componentData3.m_State & DeliveryTruckFlags.DummyTraffic) == (DeliveryTruckFlags) 0)
              {
                DynamicBuffer<LayoutElement> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_LayoutElements.TryGetBuffer(vehicle1, out bufferData) && bufferData.Length != 0)
                {
                  for (int index2 = 0; index2 < bufferData.Length; ++index2)
                  {
                    Entity vehicle2 = bufferData[index2].m_Vehicle;
                    Game.Vehicles.DeliveryTruck componentData4;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_DeliveryTruckData.TryGetComponent(vehicle2, out componentData4))
                    {
                      if ((componentData3.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                      {
                        if ((componentData4.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                          num1 += componentData4.m_Amount;
                        else if ((componentData4.m_Resource & Resource.LocalMail) != Resource.NoResource)
                          num2 += componentData4.m_Amount;
                      }
                      ReturnLoad componentData5;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_ReturnLoadData.TryGetComponent(vehicle2, out componentData5))
                      {
                        if ((componentData5.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                          num1 += componentData5.m_Amount;
                        else if ((componentData5.m_Resource & Resource.LocalMail) != Resource.NoResource)
                          num2 += componentData5.m_Amount;
                      }
                    }
                  }
                }
                else
                {
                  if ((componentData3.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                  {
                    if ((componentData3.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                      num1 += componentData3.m_Amount;
                    else if ((componentData3.m_Resource & Resource.LocalMail) != Resource.NoResource)
                      num2 += componentData3.m_Amount;
                  }
                  ReturnLoad componentData6;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ReturnLoadData.TryGetComponent(vehicle1, out componentData6))
                  {
                    if ((componentData6.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                      num1 += componentData6.m_Amount;
                    else if ((componentData6.m_Resource & Resource.LocalMail) != Resource.NoResource)
                      num2 += componentData6.m_Amount;
                  }
                }
                --vehicleCapacity3;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_EntityLookup.Exists(vehicle1))
                ownedVehicles.RemoveAt(index1--);
            }
          }
        }
        for (int index3 = 0; index3 < guestVehicles.Length; ++index3)
        {
          Entity vehicle3 = guestVehicles[index3].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TargetData.HasComponent(vehicle3) || this.m_TargetData[vehicle3].m_Target != entity)
          {
            guestVehicles.RemoveAt(index3--);
          }
          else
          {
            Game.Vehicles.DeliveryTruck componentData7;
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeliveryTruckData.TryGetComponent(vehicle3, out componentData7) && (componentData7.m_State & DeliveryTruckFlags.DummyTraffic) == (DeliveryTruckFlags) 0)
            {
              DynamicBuffer<LayoutElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LayoutElements.TryGetBuffer(vehicle3, out bufferData) && bufferData.Length != 0)
              {
                for (int index4 = 0; index4 < bufferData.Length; ++index4)
                {
                  Entity vehicle4 = bufferData[index4].m_Vehicle;
                  Game.Vehicles.DeliveryTruck componentData8;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DeliveryTruckData.TryGetComponent(vehicle4, out componentData8))
                  {
                    if ((componentData7.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                    {
                      if ((componentData8.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                        num3 += componentData8.m_Amount;
                      else if ((componentData8.m_Resource & Resource.LocalMail) != Resource.NoResource)
                        num4 += componentData8.m_Amount;
                      else if ((componentData8.m_Resource & Resource.OutgoingMail) != Resource.NoResource)
                        num5 += componentData8.m_Amount;
                    }
                    else if ((componentData8.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                      num1 += componentData8.m_Amount;
                    else if ((componentData8.m_Resource & Resource.LocalMail) != Resource.NoResource)
                      num2 += componentData8.m_Amount;
                    ReturnLoad componentData9;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ReturnLoadData.TryGetComponent(vehicle4, out componentData9))
                    {
                      if ((componentData9.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                        num3 += componentData9.m_Amount;
                      else if ((componentData9.m_Resource & Resource.LocalMail) != Resource.NoResource)
                        num4 += componentData9.m_Amount;
                      else if ((componentData9.m_Resource & Resource.OutgoingMail) != Resource.NoResource)
                        num5 += componentData9.m_Amount;
                    }
                  }
                }
              }
              else
              {
                if ((componentData7.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                {
                  if ((componentData7.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                    num3 += componentData7.m_Amount;
                  else if ((componentData7.m_Resource & Resource.LocalMail) != Resource.NoResource)
                    num4 += componentData7.m_Amount;
                  else if ((componentData7.m_Resource & Resource.OutgoingMail) != Resource.NoResource)
                    num5 += componentData7.m_Amount;
                }
                else if ((componentData7.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                  num1 += componentData7.m_Amount;
                else if ((componentData7.m_Resource & Resource.LocalMail) != Resource.NoResource)
                  num2 += componentData7.m_Amount;
                ReturnLoad componentData10;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ReturnLoadData.TryGetComponent(vehicle3, out componentData10))
                {
                  if ((componentData10.m_Resource & Resource.UnsortedMail) != Resource.NoResource)
                    num3 += componentData10.m_Amount;
                  else if ((componentData10.m_Resource & Resource.LocalMail) != Resource.NoResource)
                    num4 += componentData10.m_Amount;
                  else if ((componentData10.m_Resource & Resource.OutgoingMail) != Resource.NoResource)
                    num5 += componentData10.m_Amount;
                }
              }
            }
          }
        }
        postFacility.m_Flags &= ~(PostFacilityFlags.CanDeliverMailWithVan | PostFacilityFlags.CanCollectMailWithVan | PostFacilityFlags.HasAvailableTrucks | PostFacilityFlags.AcceptsUnsortedMail | PostFacilityFlags.DeliversLocalMail | PostFacilityFlags.AcceptsLocalMail | PostFacilityFlags.DeliversUnsortedMail);
        postFacility.m_AcceptMailPriority = 0.0f;
        postFacility.m_DeliverMailPriority = 0.0f;
        int max1;
        // ISSUE: reference to a compiler-generated field
        this.m_DeliveryTruckSelectData.GetCapacityRange(Resource.LocalMail, out int _, out max1);
        int min;
        int max2;
        // ISSUE: reference to a compiler-generated field
        this.m_DeliveryTruckSelectData.GetCapacityRange(Resource.OutgoingMail, out min, out max2);
        int max3;
        // ISSUE: reference to a compiler-generated field
        this.m_DeliveryTruckSelectData.GetCapacityRange(Resource.UnsortedMail, out int _, out max3);
        int x1 = prefabPostFacilityData.m_MailCapacity / 10;
        int num6 = math.min(x1, max1);
        min = math.min(x1, max2);
        int num7 = math.min(x1, max3);
        if (prefabPostFacilityData.m_SortingRate != 0)
        {
          float num8 = 0.0009765625f;
          int num9 = Mathf.RoundToInt(num8 * (float) prefabPostFacilityData.m_SortingRate);
          int x2 = EconomyUtils.GetResources(Resource.UnsortedMail, resources);
          int num10 = math.min(x2, Mathf.RoundToInt(efficiency * num8 * (float) prefabPostFacilityData.m_SortingRate));
          postFacility.m_ProcessingFactor = (byte) math.clamp((num10 * 100 + num9 - 1) / num9, 0, (int) byte.MaxValue);
          int num11;
          int num12;
          if (num10 != 0)
          {
            // ISSUE: reference to a compiler-generated field
            int amount = (num10 * this.m_PostConfigurationData.m_OutgoingMailPercentage + random.NextInt(100)) / 100;
            x2 = EconomyUtils.AddResources(Resource.UnsortedMail, -num10, resources);
            num11 = EconomyUtils.AddResources(Resource.LocalMail, num10 - amount, resources);
            num12 = EconomyUtils.AddResources(Resource.OutgoingMail, amount, resources);
          }
          else
          {
            num11 = EconomyUtils.GetResources(Resource.LocalMail, resources);
            num12 = EconomyUtils.GetResources(Resource.OutgoingMail, resources);
          }
          int num13 = x2 + num11 + num2 + num1;
          int y = prefabPostFacilityData.m_MailCapacity - num13;
          int amount1 = math.min(mailBox.m_MailAmount, y);
          if (amount1 > 0)
          {
            mailBox.m_MailAmount -= amount1;
            x2 = EconomyUtils.AddResources(Resource.UnsortedMail, amount1, resources);
            int num14 = num13 + x2;
            y -= amount1;
          }
          int freeSpace = y - prefabMailBoxData.m_MailCapacity;
          int num15 = x2 - num3;
          int localMail = num11 - num4;
          int x3 = num12 - num5;
          int num16 = num15 + num1;
          for (int index = 0; index < dispatches.Length; ++index)
          {
            Entity request = dispatches[index].m_Request;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PostVanRequestData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated method
              this.TrySpawnPostVan(jobIndex, ref random, entity, request, resources, ref postFacility, ref availableDeliveryVans, ref localMail, ref freeSpace, ref parkedPostVans);
              dispatches.RemoveAt(index--);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailTransferRequestData.HasComponent(request))
              {
                // ISSUE: reference to a compiler-generated method
                this.TrySpawnDeliveryTruck(jobIndex, ref random, entity, request, resources, ref vehicleCapacity3, ref freeSpace);
                dispatches.RemoveAt(index--);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ServiceRequestData.HasComponent(request))
                  dispatches.RemoveAt(index--);
              }
            }
          }
          if (localMail >= num6 || x3 >= min)
          {
            MailTransferRequestFlags flags;
            int amount2;
            if (localMail >= x3)
            {
              postFacility.m_DeliverMailPriority = (float) localMail / (float) prefabPostFacilityData.m_MailCapacity;
              flags = MailTransferRequestFlags.Receive | MailTransferRequestFlags.LocalMail;
              if (vehicleCapacity3 <= 0)
                flags |= MailTransferRequestFlags.RequireTransport;
              if (freeSpace >= num7)
                flags |= MailTransferRequestFlags.ReturnUnsortedMail;
              amount2 = math.min(localMail, max1);
            }
            else
            {
              postFacility.m_DeliverMailPriority = (float) x3 / (float) prefabPostFacilityData.m_MailCapacity;
              flags = MailTransferRequestFlags.Receive | MailTransferRequestFlags.OutgoingMail;
              if (vehicleCapacity3 <= 0)
                flags |= MailTransferRequestFlags.RequireTransport;
              if (freeSpace >= num6)
                flags |= MailTransferRequestFlags.ReturnLocalMail;
              amount2 = math.min(x3, max2);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailTransferRequestData.HasComponent(postFacility.m_MailReceiveRequest))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailTransferRequestData[postFacility.m_MailReceiveRequest].m_Flags != flags)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(postFacility.m_MailReceiveRequest, Entity.Null, true));
              }
              else
                flags = (MailTransferRequestFlags) 0;
            }
            if (flags != (MailTransferRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MailTransferRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<MailTransferRequest>(jobIndex, entity2, new MailTransferRequest(entity, flags, postFacility.m_DeliverMailPriority, amount2));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity2, new RequestGroup(8U));
            }
          }
          if (freeSpace >= num7)
          {
            postFacility.m_AcceptMailPriority = (float) (1.0 - (double) num16 / (double) prefabPostFacilityData.m_MailCapacity);
            MailTransferRequestFlags flags = MailTransferRequestFlags.Deliver | MailTransferRequestFlags.RequireTransport | MailTransferRequestFlags.UnsortedMail;
            if (localMail >= num6)
              flags |= MailTransferRequestFlags.ReturnLocalMail;
            int amount3 = math.min(freeSpace, max3);
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailTransferRequestData.HasComponent(postFacility.m_MailDeliverRequest))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailTransferRequestData[postFacility.m_MailDeliverRequest].m_Flags != flags)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(postFacility.m_MailDeliverRequest, Entity.Null, true));
              }
              else
                flags = (MailTransferRequestFlags) 0;
            }
            if (flags != (MailTransferRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity4 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MailTransferRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<MailTransferRequest>(jobIndex, entity4, new MailTransferRequest(entity, flags, postFacility.m_AcceptMailPriority, amount3));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity4, new RequestGroup(8U));
            }
          }
          if (freeSpace >= num7)
            postFacility.m_Flags |= PostFacilityFlags.AcceptsUnsortedMail;
          if (localMail >= num6)
            postFacility.m_Flags |= PostFacilityFlags.DeliversLocalMail;
          if (availableDeliveryVans > 0)
          {
            if (localMail > 0)
              postFacility.m_Flags |= PostFacilityFlags.CanDeliverMailWithVan;
            if (freeSpace > 0)
              postFacility.m_Flags |= PostFacilityFlags.CanCollectMailWithVan;
          }
          if (vehicleCapacity3 > 0)
            postFacility.m_Flags |= PostFacilityFlags.HasAvailableTrucks;
        }
        else
        {
          postFacility.m_ProcessingFactor = (byte) 0;
          int num17 = EconomyUtils.GetResources(Resource.UnsortedMail, resources);
          int resources1 = EconomyUtils.GetResources(Resource.LocalMail, resources);
          int num18 = num17 + resources1 + num2 + num1;
          int y = prefabPostFacilityData.m_MailCapacity - num18;
          int amount4 = math.min(mailBox.m_MailAmount, y);
          if (amount4 > 0)
          {
            mailBox.m_MailAmount -= amount4;
            num17 = EconomyUtils.AddResources(Resource.UnsortedMail, amount4, resources);
            int num19 = num18 + num17;
            y -= amount4;
          }
          int freeSpace = y - prefabMailBoxData.m_MailCapacity;
          int x4 = num17 - num3;
          int localMail = resources1 - num4;
          int num20 = localMail + num2;
          for (int index = 0; index < dispatches.Length; ++index)
          {
            Entity request = dispatches[index].m_Request;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PostVanRequestData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated method
              this.TrySpawnPostVan(jobIndex, ref random, entity, request, resources, ref postFacility, ref availableDeliveryVans, ref localMail, ref freeSpace, ref parkedPostVans);
              dispatches.RemoveAt(index--);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailTransferRequestData.HasComponent(request))
              {
                // ISSUE: reference to a compiler-generated method
                this.TrySpawnDeliveryTruck(jobIndex, ref random, entity, request, resources, ref vehicleCapacity3, ref freeSpace);
                dispatches.RemoveAt(index--);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ServiceRequestData.HasComponent(request))
                  dispatches.RemoveAt(index--);
              }
            }
          }
          int num21 = math.max(0, num7 - x4);
          int x5 = (prefabPostFacilityData.m_MailCapacity >> 1) - num20;
          int x6 = math.min(x5, freeSpace - num21);
          if (x6 >= num6)
          {
            postFacility.m_AcceptMailPriority = (float) (1.0 - (double) num20 / (double) prefabPostFacilityData.m_MailCapacity);
            MailTransferRequestFlags flags = MailTransferRequestFlags.Deliver | MailTransferRequestFlags.RequireTransport | MailTransferRequestFlags.LocalMail;
            if (x4 >= num7)
              flags |= MailTransferRequestFlags.ReturnUnsortedMail;
            int amount5 = math.min(x6, max1);
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailTransferRequestData.HasComponent(postFacility.m_MailDeliverRequest))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailTransferRequestData[postFacility.m_MailDeliverRequest].m_Flags != flags)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity5 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity5, new HandleRequest(postFacility.m_MailDeliverRequest, Entity.Null, true));
              }
              else
                flags = (MailTransferRequestFlags) 0;
            }
            if (flags != (MailTransferRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity6 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MailTransferRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<MailTransferRequest>(jobIndex, entity6, new MailTransferRequest(entity, flags, postFacility.m_AcceptMailPriority, amount5));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity6, new RequestGroup(8U));
            }
          }
          else if (x4 >= num7)
          {
            postFacility.m_DeliverMailPriority = (float) x4 / (float) prefabPostFacilityData.m_MailCapacity;
            MailTransferRequestFlags flags = MailTransferRequestFlags.Receive | MailTransferRequestFlags.UnsortedMail;
            if (vehicleCapacity3 <= 0)
              flags |= MailTransferRequestFlags.RequireTransport;
            if (x5 >= num6)
              flags |= MailTransferRequestFlags.ReturnLocalMail;
            int amount6 = math.min(x4, max3);
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailTransferRequestData.HasComponent(postFacility.m_MailReceiveRequest))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MailTransferRequestData[postFacility.m_MailReceiveRequest].m_Flags != flags)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity7 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity7, new HandleRequest(postFacility.m_MailReceiveRequest, Entity.Null, true));
              }
              else
                flags = (MailTransferRequestFlags) 0;
            }
            if (flags != (MailTransferRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity8 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MailTransferRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<MailTransferRequest>(jobIndex, entity8, new MailTransferRequest(entity, flags, postFacility.m_DeliverMailPriority, amount6));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity8, new RequestGroup(8U));
            }
          }
          if (x6 >= num6)
            postFacility.m_Flags |= PostFacilityFlags.AcceptsLocalMail;
          if (x4 >= num7)
            postFacility.m_Flags |= PostFacilityFlags.DeliversUnsortedMail;
          if (availableDeliveryVans > 0)
          {
            if (localMail > 0)
              postFacility.m_Flags |= PostFacilityFlags.CanDeliverMailWithVan;
            if (freeSpace > 0)
              postFacility.m_Flags |= PostFacilityFlags.CanCollectMailWithVan;
          }
          if (vehicleCapacity3 > 0)
            postFacility.m_Flags |= PostFacilityFlags.HasAvailableTrucks;
        }
        while (parkedPostVans.Length > math.max(0, prefabPostFacilityData.m_PostVanCapacity + availableDeliveryVans - vehicleCapacity1))
        {
          int index = random.NextInt(parkedPostVans.Length);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, parkedPostVans[index]);
          parkedPostVans.RemoveAtSwapBack(index);
        }
        for (int index = 0; index < parkedPostVans.Length; ++index)
        {
          Entity entity9 = parkedPostVans[index];
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.PostVan postVan = this.m_PostVanData[entity9];
          bool disabled = (postFacility.m_Flags & (PostFacilityFlags.CanDeliverMailWithVan | PostFacilityFlags.CanCollectMailWithVan)) == (PostFacilityFlags) 0;
          if ((postVan.m_State & PostVanFlags.Disabled) > (PostVanFlags) 0 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(PostFacilityAISystem.PostFacilityAction.SetDisabled(entity9, disabled));
          }
        }
        if ((postFacility.m_Flags & (PostFacilityFlags.CanDeliverMailWithVan | PostFacilityFlags.CanCollectMailWithVan)) == (PostFacilityFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.RequestTargetIfNeeded(jobIndex, entity, ref postFacility, availableDeliveryVans);
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.PostFacility postFacility,
        int availablePostVans)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(postFacility.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 256U) - 1) != 176)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_PostVanRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PostVanRequest>(jobIndex, entity1, new PostVanRequest(entity, (PostVanRequestFlags) 0, (ushort) availablePostVans));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private bool TrySpawnPostVan(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Entity request,
        DynamicBuffer<Game.Economy.Resources> resources,
        ref Game.Buildings.PostFacility postFacility,
        ref int availableDeliveryVans,
        ref int localMail,
        ref int freeSpace,
        ref StackList<Entity> parkedPostVans)
      {
        int2 mailCapacity = new int2(1, math.max(localMail, freeSpace));
        if (availableDeliveryVans <= 0 || mailCapacity.y <= 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        PostVanRequest postVanRequest = this.m_PostVanRequestData[request];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(postVanRequest.m_Target) || localMail <= 0 && (postVanRequest.m_Flags & PostVanRequestFlags.Deliver) != (PostVanRequestFlags) 0)
          return false;
        Entity entity1 = Entity.Null;
        PathInformation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathInformationData.TryGetComponent(request, out componentData1) && componentData1.m_Origin != entity)
        {
          PrefabRef componentData2;
          PostVanData componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(componentData1.m_Origin, out componentData2) && this.m_PrefabPostVanData.TryGetComponent(componentData2.m_Prefab, out componentData3))
          {
            mailCapacity = (int2) componentData3.m_MailCapacity;
            if (mailCapacity.y > freeSpace)
              return false;
          }
          if (!CollectionUtils.RemoveValueSwapBack<Entity>(ref parkedPostVans, componentData1.m_Origin))
            return false;
          // ISSUE: reference to a compiler-generated field
          ParkedCar parkedCar = this.m_ParkedCarData[componentData1.m_Origin];
          entity1 = componentData1.m_Origin;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent(jobIndex, entity1, in this.m_ParkedToMovingRemoveTypes);
          Game.Vehicles.CarLaneFlags flags = Game.Vehicles.CarLaneFlags.EndReached | Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.FixedLane;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent(jobIndex, entity1, in this.m_ParkedToMovingCarAddTypes);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<CarCurrentLane>(jobIndex, entity1, new CarCurrentLane(parkedCar, flags));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ParkingLaneData.HasComponent(parkedCar.m_Lane) || this.m_SpawnLocationData.HasComponent(parkedCar.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, parkedCar.m_Lane);
          }
        }
        if (entity1 == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          Entity vehiclePrefab = this.m_PostVanSelectData.SelectVehicle(ref random, ref mailCapacity);
          if (mailCapacity.y > freeSpace)
            return false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          entity1 = this.m_PostVanSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, this.m_TransformData[entity], entity, vehiclePrefab, false);
          if (entity1 == Entity.Null)
            return false;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity1, new Owner(entity));
        }
        int deliveringMail = math.min(localMail, mailCapacity.y);
        --availableDeliveryVans;
        localMail -= deliveringMail;
        freeSpace -= mailCapacity.y;
        EconomyUtils.AddResources(Resource.LocalMail, -deliveringMail, resources);
        PostVanFlags flags1 = (PostVanFlags) 0;
        if ((postVanRequest.m_Flags & PostVanRequestFlags.Deliver) != (PostVanRequestFlags) 0)
          flags1 |= PostVanFlags.Delivering;
        if ((postVanRequest.m_Flags & PostVanRequestFlags.Collect) != (PostVanRequestFlags) 0)
          flags1 |= PostVanFlags.Collecting;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Vehicles.PostVan>(jobIndex, entity1, new Game.Vehicles.PostVan(flags1, 1, deliveringMail));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Common.Target>(jobIndex, entity1, new Game.Common.Target(postVanRequest.m_Target));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<ServiceDispatch>(jobIndex, entity1).Add(new ServiceDispatch(request));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(request, entity1, false));
        DynamicBuffer<PathElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> targetElements = this.m_CommandBuffer.SetBuffer<PathElement>(jobIndex, entity1);
          PathUtils.CopyPath(bufferData, new PathOwner(), 0, targetElements);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathOwner>(jobIndex, entity1, new PathOwner(PathFlags.Updated));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, entity1, componentData1);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(postFacility.m_TargetRequest))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(postFacility.m_TargetRequest, Entity.Null, true));
        }
        return true;
      }

      private bool TrySpawnDeliveryTruck(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Entity request,
        DynamicBuffer<Game.Economy.Resources> resources,
        ref int availableDeliveryTrucks,
        ref int freeSpace)
      {
        if (availableDeliveryTrucks <= 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        MailTransferRequest mailTransferRequest = this.m_MailTransferRequestData[request];
        // ISSUE: reference to a compiler-generated field
        PathInformation component = this.m_PathInformationData[request];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(component.m_Destination))
          return false;
        DeliveryTruckFlags state = (DeliveryTruckFlags) 0;
        Resource resource = Resource.NoResource;
        Resource returnResource = Resource.NoResource;
        int amount1 = mailTransferRequest.m_Amount;
        int returnAmount = 0;
        if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.UnsortedMail) != (MailTransferRequestFlags) 0)
          resource = Resource.UnsortedMail;
        if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.LocalMail) != (MailTransferRequestFlags) 0)
          resource = Resource.LocalMail;
        if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.OutgoingMail) != (MailTransferRequestFlags) 0)
          resource = Resource.OutgoingMail;
        if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.RequireTransport) != (MailTransferRequestFlags) 0)
        {
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.Deliver) != (MailTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Loaded;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.Receive) != (MailTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Buying;
        }
        else
        {
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.Deliver) != (MailTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Buying;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.Receive) != (MailTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Loaded;
        }
        int amount2;
        int max;
        if ((state & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
        {
          amount2 = math.min(amount1, EconomyUtils.GetResources(resource, resources));
          if (amount2 <= 0)
            return false;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.ReturnUnsortedMail) != (MailTransferRequestFlags) 0)
            returnResource = Resource.UnsortedMail;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.ReturnLocalMail) != (MailTransferRequestFlags) 0)
            returnResource = Resource.LocalMail;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.ReturnOutgoingMail) != (MailTransferRequestFlags) 0)
            returnResource = Resource.OutgoingMail;
          if (returnResource != Resource.NoResource)
          {
            int min;
            // ISSUE: reference to a compiler-generated field
            this.m_DeliveryTruckSelectData.GetCapacityRange(resource | returnResource, out min, out max);
            returnAmount = math.min(amount2 + freeSpace, math.max(amount2, min));
            if (returnAmount <= 0)
            {
              returnResource = Resource.NoResource;
              returnAmount = 0;
            }
          }
        }
        else
        {
          returnResource = resource;
          int x = amount1;
          resource = Resource.NoResource;
          amount2 = 0;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.ReturnUnsortedMail) != (MailTransferRequestFlags) 0)
            resource = Resource.UnsortedMail;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.ReturnLocalMail) != (MailTransferRequestFlags) 0)
            resource = Resource.LocalMail;
          if ((mailTransferRequest.m_Flags & MailTransferRequestFlags.ReturnOutgoingMail) != (MailTransferRequestFlags) 0)
            resource = Resource.OutgoingMail;
          if (resource != Resource.NoResource)
          {
            int min;
            // ISSUE: reference to a compiler-generated field
            this.m_DeliveryTruckSelectData.GetCapacityRange(resource | returnResource, out min, out max);
            amount2 = math.min(EconomyUtils.GetResources(resource, resources), math.max(x, min));
            if (amount2 <= 0)
            {
              resource = Resource.NoResource;
              amount2 = 0;
            }
          }
          returnAmount = math.min(x, amount2 + freeSpace);
          if (returnAmount <= 0)
          {
            returnResource = Resource.NoResource;
            returnAmount = 0;
            if (amount2 == 0)
              return false;
          }
          state = state & ~DeliveryTruckFlags.Buying | DeliveryTruckFlags.Loaded;
        }
        if (amount2 > 0)
          state |= DeliveryTruckFlags.UpdateOwnerQuantity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity vehicle = this.m_DeliveryTruckSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, ref this.m_PrefabDeliveryTruckData, ref this.m_PrefabObjectData, resource, returnResource, ref amount2, ref returnAmount, this.m_TransformData[entity], entity, state);
        if (!(vehicle != Entity.Null))
          return false;
        if (amount2 > 0)
          EconomyUtils.AddResources(resource, -amount2, resources);
        --availableDeliveryTrucks;
        freeSpace += amount2 - returnAmount;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Common.Target>(jobIndex, vehicle, new Game.Common.Target(component.m_Destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(jobIndex, vehicle, new Owner(entity));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(request, vehicle, true));
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathElements.HasBuffer(request))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement = this.m_PathElements[request];
          if (pathElement.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PathElement> targetElements = this.m_CommandBuffer.SetBuffer<PathElement>(jobIndex, vehicle);
            PathUtils.CopyPath(pathElement, new PathOwner(), 0, targetElements);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathOwner>(jobIndex, vehicle, new PathOwner(PathFlags.Updated));
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, vehicle, component);
          }
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
    private struct PostFacilityActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.PostVan> m_PostVanData;
      public NativeQueue<PostFacilityAISystem.PostFacilityAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PostFacilityAISystem.PostFacilityAction postFacilityAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out postFacilityAction))
        {
          Game.Vehicles.PostVan componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PostVanData.TryGetComponent(postFacilityAction.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            if (postFacilityAction.m_Disabled)
              componentData.m_State |= PostVanFlags.Disabled;
            else
              componentData.m_State &= ~PostVanFlags.Disabled;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PostVanData[postFacilityAction.m_Entity] = componentData;
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
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.PostFacility> __Game_Buildings_PostFacility_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Routes.MailBox> __Game_Routes_MailBox_RW_ComponentTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle;
      public BufferTypeHandle<GuestVehicle> __Game_Vehicles_GuestVehicle_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> __Game_Simulation_PostVanRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailTransferRequest> __Game_Simulation_MailTransferRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PostVan> __Game_Vehicles_PostVan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ReturnLoad> __Game_Vehicles_ReturnLoad_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostFacilityData> __Game_Prefabs_PostFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailBoxData> __Game_Prefabs_MailBoxData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostVanData> __Game_Prefabs_PostVanData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DeliveryTruckData> __Game_Prefabs_DeliveryTruckData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.PostVan> __Game_Vehicles_PostVan_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PostFacility_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.PostFacility>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_MailBox_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.MailBox>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GuestVehicle_RW_BufferTypeHandle = state.GetBufferTypeHandle<GuestVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PostVanRequest_RO_ComponentLookup = state.GetComponentLookup<PostVanRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MailTransferRequest_RO_ComponentLookup = state.GetComponentLookup<MailTransferRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PostVan_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PostVan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ReturnLoad_RO_ComponentLookup = state.GetComponentLookup<ReturnLoad>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PostFacilityData_RO_ComponentLookup = state.GetComponentLookup<PostFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MailBoxData_RO_ComponentLookup = state.GetComponentLookup<MailBoxData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PostVanData_RO_ComponentLookup = state.GetComponentLookup<PostVanData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup = state.GetComponentLookup<DeliveryTruckData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PostVan_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PostVan>();
      }
    }
  }
}
