// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GarbageFacilityAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
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
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class GarbageFacilityAISystem : GameSystemBase
  {
    private const int kUpdatesPerDay = 1024;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private IconCommandSystem m_IconCommandSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private GarbageTruckSelectData m_GarbageTruckSelectData;
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_GarbageTruckPrefabQuery;
    private EntityQuery m_GarbageSettingsQuery;
    private EntityArchetype m_GarbageTransferRequestArchetype;
    private EntityArchetype m_GarbageCollectionRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_ParkedToMovingRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingCarAddTypes;
    private GarbageFacilityAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 80;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckSelectData = new GarbageTruckSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckPrefabQuery = this.GetEntityQuery(GarbageTruckSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTransferRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<GarbageTransferRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageCollectionRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<GarbageCollectionRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
      this.RequireForUpdate(this.m_GarbageSettingsQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_GarbageTruckPrefabQuery, Allocator.TempJob, out jobHandle1);
      NativeQueue<GarbageFacilityAISystem.GarbageFacilityAction> nativeQueue = new NativeQueue<GarbageFacilityAISystem.GarbageFacilityAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Storage_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceProductionData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageTruckData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_GarbageTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
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
      GarbageFacilityAISystem.GarbageFacilityTickJob jobData1 = new GarbageFacilityAISystem.GarbageFacilityTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_SubAreaType = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferTypeHandle,
        m_GarbageFacilityType = this.__TypeHandle.__Game_Buildings_GarbageFacility_RW_ComponentTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle,
        m_GuestVehicleType = this.__TypeHandle.__Game_Vehicles_GuestVehicle_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_GarbageCollectionRequestData = this.__TypeHandle.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup,
        m_GarbageTransferRequestData = this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_GarbageTruckData = this.__TypeHandle.__Game_Vehicles_GarbageTruck_RO_ComponentLookup,
        m_DeliveryTruckData = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_ReturnLoadData = this.__TypeHandle.__Game_Vehicles_ReturnLoad_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_AreaGeometryData = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGarbageFacilityData = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup,
        m_PrefabGarbageTruckData = this.__TypeHandle.__Game_Prefabs_GarbageTruckData_RO_ComponentLookup,
        m_PrefabStorageAreaData = this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup,
        m_PrefabDeliveryTruckData = this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup,
        m_PrefabObjectData = this.__TypeHandle.__Game_Prefabs_ObjectData_RO_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_ResourceProductionDatas = this.__TypeHandle.__Game_Prefabs_ResourceProductionData_RO_BufferLookup,
        m_AreaStorageData = this.__TypeHandle.__Game_Areas_Storage_RW_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_GarbageParameters = this.m_GarbageSettingsQuery.GetSingleton<GarbageParameterData>(),
        m_GarbageTruckSelectData = this.m_GarbageTruckSelectData,
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData(),
        m_GarbageTransferRequestArchetype = this.m_GarbageTransferRequestArchetype,
        m_GarbageCollectionRequestArchetype = this.m_GarbageCollectionRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_ParkedToMovingRemoveTypes = this.m_ParkedToMovingRemoveTypes,
        m_ParkedToMovingCarAddTypes = this.m_ParkedToMovingCarAddTypes,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_GarbageTruck_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GarbageFacilityAISystem.GarbageFacilityActionJob jobData2 = new GarbageFacilityAISystem.GarbageFacilityActionJob()
      {
        m_GarbageTruckData = this.__TypeHandle.__Game_Vehicles_GarbageTruck_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData1.ScheduleParallel<GarbageFacilityAISystem.GarbageFacilityTickJob>(this.m_BuildingQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.Schedule<GarbageFacilityAISystem.GarbageFacilityActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = inputDeps;
    }

    private static float CalculateProcessingRate(
      float maxProcessingRate,
      float efficiency,
      int garbageAmount,
      int garbageCapacity)
    {
      // ISSUE: reference to a compiler-generated method
      float garbageAmountFactor = GarbageFacilityAISystem.CalculateGarbageAmountFactor(garbageAmount, garbageCapacity);
      return efficiency * garbageAmountFactor * maxProcessingRate;
    }

    private static float CalculateGarbageAmountFactor(int garbageAmount, int garbageCapacity)
    {
      return math.saturate((float) (0.10000000149011612 + (double) garbageAmount * 1.7999999523162842 / (double) math.max(1f, (float) garbageCapacity)));
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
    public GarbageFacilityAISystem()
    {
    }

    private struct GarbageFacilityAction
    {
      public Entity m_Entity;
      public bool m_Disabled;

      public static GarbageFacilityAISystem.GarbageFacilityAction SetDisabled(
        Entity vehicle,
        bool disabled)
      {
        // ISSUE: object of a compiler-generated type is created
        return new GarbageFacilityAISystem.GarbageFacilityAction()
        {
          m_Entity = vehicle,
          m_Disabled = disabled
        };
      }
    }

    [BurstCompile]
    private struct GarbageFacilityTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.SubArea> m_SubAreaType;
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> m_GarbageFacilityType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourcesType;
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      public BufferTypeHandle<GuestVehicle> m_GuestVehicleType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
      [ReadOnly]
      public ComponentLookup<GarbageTransferRequest> m_GarbageTransferRequestData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.GarbageTruck> m_GarbageTruckData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<ReturnLoad> m_ReturnLoadData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Geometry> m_AreaGeometryData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> m_PrefabGarbageFacilityData;
      [ReadOnly]
      public ComponentLookup<GarbageTruckData> m_PrefabGarbageTruckData;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> m_PrefabStorageAreaData;
      [ReadOnly]
      public ComponentLookup<DeliveryTruckData> m_PrefabDeliveryTruckData;
      [ReadOnly]
      public ComponentLookup<ObjectData> m_PrefabObjectData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<ResourceProductionData> m_ResourceProductionDatas;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Storage> m_AreaStorageData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public GarbageParameterData m_GarbageParameters;
      [ReadOnly]
      public GarbageTruckSelectData m_GarbageTruckSelectData;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;
      [ReadOnly]
      public EntityArchetype m_GarbageTransferRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_GarbageCollectionRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingCarAddTypes;
      public IconCommandBuffer m_IconCommandBuffer;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<GarbageFacilityAISystem.GarbageFacilityAction>.ParallelWriter m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray2 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.GarbageFacility> nativeArray4 = chunk.GetNativeArray<Game.Buildings.GarbageFacility>(ref this.m_GarbageFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor3 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<GuestVehicle> bufferAccessor4 = chunk.GetBufferAccessor<GuestVehicle>(ref this.m_GuestVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.SubArea> bufferAccessor5 = chunk.GetBufferAccessor<Game.Areas.SubArea>(ref this.m_SubAreaType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor6 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Economy.Resources> bufferAccessor7 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourcesType);
        NativeList<ResourceProductionData> resourceProductionBuffer = new NativeList<ResourceProductionData>();
        // ISSUE: reference to a compiler-generated field
        bool outside = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
        int min;
        int max;
        // ISSUE: reference to a compiler-generated field
        this.m_DeliveryTruckSelectData.GetCapacityRange(Resource.Garbage, out min, out max);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray3[index];
          Game.Buildings.GarbageFacility garbageFacility = nativeArray4[index];
          DynamicBuffer<OwnedVehicle> ownedVehicles = bufferAccessor3[index];
          DynamicBuffer<ServiceDispatch> dispatches = bufferAccessor6[index];
          DynamicBuffer<GuestVehicle> guestVehicles = new DynamicBuffer<GuestVehicle>();
          if (bufferAccessor4.Length != 0)
            guestVehicles = bufferAccessor4[index];
          // ISSUE: reference to a compiler-generated field
          GarbageFacilityData data = this.m_PrefabGarbageFacilityData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResourceProductionDatas.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddResourceProductionData(this.m_ResourceProductionDatas[prefabRef.m_Prefab], ref resourceProductionBuffer);
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<GarbageFacilityData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabGarbageFacilityData);
            // ISSUE: reference to a compiler-generated method
            this.CombineResourceProductionData(bufferAccessor2[index], ref resourceProductionBuffer);
          }
          int garbageAmount = 0;
          DynamicBuffer<Game.Economy.Resources> resources = new DynamicBuffer<Game.Economy.Resources>();
          if (bufferAccessor7.Length != 0)
          {
            resources = bufferAccessor7[index];
            garbageAmount = EconomyUtils.GetResources(Resource.Garbage, resources);
          }
          int num1 = garbageAmount;
          Building building = new Building();
          if (nativeArray2.Length != 0)
            building = nativeArray2[index];
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          float immediateEfficiency = BuildingUtils.GetImmediateEfficiency(bufferAccessor1, index);
          int areaCapacity = 0;
          int areaGarbage = 0;
          if (bufferAccessor5.Length != 0)
          {
            DynamicBuffer<Game.Areas.SubArea> subAreas = bufferAccessor5[index];
            // ISSUE: reference to a compiler-generated method
            this.ProcessAreas(unfilteredChunkIndex, subAreas, ref garbageAmount, data, max, efficiency, out areaCapacity, out areaGarbage);
          }
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, building, ref random, ref garbageFacility, ref garbageAmount, data, efficiency, immediateEfficiency, areaCapacity, areaGarbage, min, max, ownedVehicles, guestVehicles, dispatches, resources, resourceProductionBuffer, outside);
          nativeArray4[index] = garbageFacility;
          if (resources.IsCreated)
          {
            EconomyUtils.SetResources(Resource.Garbage, resources, garbageAmount);
            int num2 = Mathf.RoundToInt((float) ((double) num1 / (double) math.max(1, data.m_GarbageCapacity) * 100.0));
            int num3 = Mathf.RoundToInt((float) ((double) garbageAmount / (double) math.max(1, data.m_GarbageCapacity) * 100.0));
            int4 int4_1 = new int4(0, 33, 50, 66);
            int4 int4_2 = int4_1;
            if (math.any(num2 > int4_2 != num3 > int4_1))
            {
              // ISSUE: reference to a compiler-generated method
              this.QuantityUpdated(unfilteredChunkIndex, entity);
            }
          }
          if (resourceProductionBuffer.IsCreated)
            resourceProductionBuffer.Clear();
        }
        if (!resourceProductionBuffer.IsCreated)
          return;
        resourceProductionBuffer.Dispose();
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

      private void AddResourceProductionData(
        DynamicBuffer<ResourceProductionData> resourceProductionDatas,
        ref NativeList<ResourceProductionData> resourceProductionBuffer)
      {
        if (!resourceProductionBuffer.IsCreated)
          resourceProductionBuffer = new NativeList<ResourceProductionData>(resourceProductionDatas.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        ResourceProductionData.Combine(resourceProductionBuffer, resourceProductionDatas);
      }

      private void CombineResourceProductionData(
        DynamicBuffer<InstalledUpgrade> upgrades,
        ref NativeList<ResourceProductionData> resourceProductionBuffer)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          InstalledUpgrade upgrade = upgrades[index];
          DynamicBuffer<ResourceProductionData> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive) && this.m_ResourceProductionDatas.TryGetBuffer(this.m_PrefabRefData[upgrade.m_Upgrade].m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddResourceProductionData(bufferData, ref resourceProductionBuffer);
          }
        }
      }

      private void ProcessAreas(
        int jobIndex,
        DynamicBuffer<Game.Areas.SubArea> subAreas,
        ref int garbageAmount,
        GarbageFacilityData prefabGarbageFacilityData,
        int maxGarbageLoad,
        float efficiency,
        out int areaCapacity,
        out int areaGarbage)
      {
        areaCapacity = 0;
        areaGarbage = 0;
        for (int index = 0; index < subAreas.Length; ++index)
        {
          Entity area = subAreas[index].m_Area;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AreaStorageData.HasComponent(area))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[area];
            // ISSUE: reference to a compiler-generated field
            Storage storage = this.m_AreaStorageData[area];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int storageCapacity = AreaUtils.CalculateStorageCapacity(this.m_AreaGeometryData[area], this.m_PrefabStorageAreaData[prefabRef.m_Prefab]);
            int num1 = (int) ((long) storage.m_Amount * (long) prefabGarbageFacilityData.m_GarbageCapacity / (long) (storageCapacity * 2));
            float num2 = 0.0009765625f;
            float f = (float) prefabGarbageFacilityData.m_ProcessingSpeed * num2;
            int num3 = num1 + (maxGarbageLoad + Mathf.CeilToInt(f));
            int x = math.max(math.min(garbageAmount - num3, storageCapacity - storage.m_Amount), -math.min(storage.m_Amount, prefabGarbageFacilityData.m_GarbageCapacity - garbageAmount));
            int a = Mathf.CeilToInt((float) math.abs(x) * math.saturate(efficiency));
            int num4 = math.select(a, -a, x < 0);
            if (num4 != 0)
            {
              int num5 = (int) ((long) storage.m_Amount * 100L / (long) storageCapacity);
              storage.m_Amount += num4;
              storage.m_WorkAmount += (float) a;
              garbageAmount -= num4;
              // ISSUE: reference to a compiler-generated field
              this.m_AreaStorageData[area] = storage;
              if ((int) ((long) storage.m_Amount * 100L / (long) storageCapacity) != num5)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(jobIndex, area, new Updated());
              }
            }
            areaCapacity += storageCapacity;
            areaGarbage += storage.m_Amount;
          }
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        Building building,
        ref Unity.Mathematics.Random random,
        ref Game.Buildings.GarbageFacility garbageFacility,
        ref int garbageAmount,
        GarbageFacilityData prefabGarbageFacilityData,
        float efficiency,
        float immediateEfficiency,
        int areaCapacity,
        int areaGarbage,
        int minGarbageLoad,
        int maxGarbageLoad,
        DynamicBuffer<OwnedVehicle> ownedVehicles,
        DynamicBuffer<GuestVehicle> guestVehicles,
        DynamicBuffer<ServiceDispatch> dispatches,
        DynamicBuffer<Game.Economy.Resources> resources,
        NativeList<ResourceProductionData> resourceProductionBuffer,
        bool outside)
      {
        if (outside)
        {
          int num = prefabGarbageFacilityData.m_GarbageCapacity / 2 - garbageAmount;
          if (num != 0)
            garbageAmount += num;
        }
        float maxProcessingRate = math.min((float) garbageAmount, (float) prefabGarbageFacilityData.m_ProcessingSpeed / 1024f);
        // ISSUE: reference to a compiler-generated method
        float processingRate = GarbageFacilityAISystem.CalculateProcessingRate(maxProcessingRate, efficiency, garbageAmount, prefabGarbageFacilityData.m_GarbageCapacity);
        float x = (double) maxProcessingRate > 0.0 ? processingRate / maxProcessingRate : 0.0f;
        if (resourceProductionBuffer.IsCreated)
        {
          for (int index = 0; index < resourceProductionBuffer.Length; ++index)
          {
            ResourceProductionData resourceProductionData = resourceProductionBuffer[index];
            float b = (float) resourceProductionData.m_ProductionRate / 1024f;
            if ((double) b > 0.0)
            {
              int resources1 = EconomyUtils.GetResources(resourceProductionData.m_Type, resources);
              float num = math.clamp((float) (resourceProductionData.m_StorageCapacity - resources1), 0.0f, b);
              x = math.min(x, num / b);
            }
          }
          for (int index = 0; index < resourceProductionBuffer.Length; ++index)
          {
            ResourceProductionData resourceProductionData = resourceProductionBuffer[index];
            float num = (float) resourceProductionData.m_ProductionRate / 1024f;
            EconomyUtils.AddResources(resourceProductionData.m_Type, MathUtils.RoundToIntRandom(ref random, x * num), resources);
          }
        }
        float num1 = x * maxProcessingRate;
        garbageFacility.m_ProcessingRate = Mathf.RoundToInt(num1 * 1024f);
        int intRandom = MathUtils.RoundToIntRandom(ref random, num1);
        garbageAmount -= intRandom;
        int vehicleCapacity1 = BuildingUtils.GetVehicleCapacity(efficiency, prefabGarbageFacilityData.m_VehicleCapacity);
        int vehicleCapacity2 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabGarbageFacilityData.m_VehicleCapacity);
        int availableVehicles = vehicleCapacity1;
        int transportCapacity = prefabGarbageFacilityData.m_TransportCapacity;
        int availableSpace = prefabGarbageFacilityData.m_GarbageCapacity - garbageAmount + areaCapacity - areaGarbage;
        int availableGarbage = garbageAmount + areaGarbage - intRandom * 2;
        StackList<Entity> parkedVehicles = (StackList<Entity>) stackalloc Entity[ownedVehicles.Length];
        for (int index1 = 0; index1 < ownedVehicles.Length; ++index1)
        {
          Entity vehicle1 = ownedVehicles[index1].m_Vehicle;
          Game.Vehicles.GarbageTruck componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageTruckData.TryGetComponent(vehicle1, out componentData1))
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
                parkedVehicles.AddNoResize(vehicle1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              GarbageTruckData garbageTruckData = this.m_PrefabGarbageTruckData[this.m_PrefabRefData[vehicle1].m_Prefab];
              --availableVehicles;
              availableSpace -= garbageTruckData.m_GarbageCapacity;
              bool disabled = --vehicleCapacity2 < 0;
              if ((componentData1.m_State & GarbageTruckFlags.Disabled) > (GarbageTruckFlags) 0 != disabled)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ActionQueue.Enqueue(GarbageFacilityAISystem.GarbageFacilityAction.SetDisabled(vehicle1, disabled));
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
                DynamicBuffer<LayoutElement> dynamicBuffer = new DynamicBuffer<LayoutElement>();
                // ISSUE: reference to a compiler-generated field
                if (this.m_LayoutElements.HasBuffer(vehicle1))
                {
                  // ISSUE: reference to a compiler-generated field
                  dynamicBuffer = this.m_LayoutElements[vehicle1];
                }
                if (dynamicBuffer.IsCreated && dynamicBuffer.Length != 0)
                {
                  for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                  {
                    Entity vehicle2 = dynamicBuffer[index2].m_Vehicle;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_DeliveryTruckData.HasComponent(vehicle2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Game.Vehicles.DeliveryTruck deliveryTruck = this.m_DeliveryTruckData[vehicle2];
                      if ((deliveryTruck.m_Resource & Resource.Garbage) != Resource.NoResource && (componentData3.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                        availableSpace -= deliveryTruck.m_Amount;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_ReturnLoadData.HasComponent(vehicle2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        ReturnLoad returnLoad = this.m_ReturnLoadData[vehicle2];
                        if ((returnLoad.m_Resource & Resource.Garbage) != Resource.NoResource)
                          availableSpace -= returnLoad.m_Amount;
                      }
                    }
                  }
                }
                else
                {
                  if ((componentData3.m_Resource & Resource.Garbage) != Resource.NoResource && (componentData3.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                    availableSpace -= componentData3.m_Amount;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ReturnLoadData.HasComponent(vehicle1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ReturnLoad returnLoad = this.m_ReturnLoadData[vehicle1];
                    if ((returnLoad.m_Resource & Resource.Garbage) != Resource.NoResource)
                      availableSpace -= returnLoad.m_Amount;
                  }
                }
                --transportCapacity;
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
        if (guestVehicles.IsCreated)
        {
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
              // ISSUE: reference to a compiler-generated field
              if (this.m_DeliveryTruckData.HasComponent(vehicle3))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Vehicles.DeliveryTruck deliveryTruck1 = this.m_DeliveryTruckData[vehicle3];
                if ((deliveryTruck1.m_State & DeliveryTruckFlags.DummyTraffic) == (DeliveryTruckFlags) 0)
                {
                  DynamicBuffer<LayoutElement> dynamicBuffer = new DynamicBuffer<LayoutElement>();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LayoutElements.HasBuffer(vehicle3))
                  {
                    // ISSUE: reference to a compiler-generated field
                    dynamicBuffer = this.m_LayoutElements[vehicle3];
                  }
                  if (dynamicBuffer.IsCreated && dynamicBuffer.Length != 0)
                  {
                    for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
                    {
                      Entity vehicle4 = dynamicBuffer[index4].m_Vehicle;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_DeliveryTruckData.HasComponent(vehicle4))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Game.Vehicles.DeliveryTruck deliveryTruck2 = this.m_DeliveryTruckData[vehicle4];
                        if ((deliveryTruck1.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                        {
                          if ((deliveryTruck2.m_Resource & Resource.Garbage) != Resource.NoResource)
                            availableGarbage -= deliveryTruck2.m_Amount;
                        }
                        else if ((deliveryTruck2.m_Resource & Resource.Garbage) != Resource.NoResource)
                          availableSpace -= deliveryTruck2.m_Amount;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_ReturnLoadData.HasComponent(vehicle4))
                        {
                          // ISSUE: reference to a compiler-generated field
                          ReturnLoad returnLoad = this.m_ReturnLoadData[vehicle4];
                          if ((returnLoad.m_Resource & Resource.Garbage) != Resource.NoResource)
                            availableGarbage -= returnLoad.m_Amount;
                        }
                      }
                    }
                  }
                  else
                  {
                    if ((deliveryTruck1.m_State & DeliveryTruckFlags.Buying) != (DeliveryTruckFlags) 0)
                    {
                      if ((deliveryTruck1.m_Resource & Resource.Garbage) != Resource.NoResource)
                        availableGarbage -= deliveryTruck1.m_Amount;
                    }
                    else if ((deliveryTruck1.m_Resource & Resource.Garbage) != Resource.NoResource)
                      availableSpace -= deliveryTruck1.m_Amount;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_ReturnLoadData.HasComponent(vehicle3))
                    {
                      // ISSUE: reference to a compiler-generated field
                      ReturnLoad returnLoad = this.m_ReturnLoadData[vehicle3];
                      if ((returnLoad.m_Resource & Resource.Garbage) != Resource.NoResource)
                        availableGarbage -= returnLoad.m_Amount;
                    }
                  }
                }
              }
            }
          }
        }
        if (BuildingUtils.CheckOption(building, BuildingOption.Empty))
          availableSpace = 0;
        for (int index = 0; index < dispatches.Length; ++index)
        {
          Entity request = dispatches[index].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageCollectionRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated method
            this.TrySpawnGarbageTruck(jobIndex, ref random, entity, request, prefabGarbageFacilityData, ref garbageFacility, ref availableVehicles, ref availableSpace, ref parkedVehicles);
            dispatches.RemoveAt(index--);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageTransferRequestData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated method
              this.TrySpawnDeliveryTruck(jobIndex, ref random, entity, request, ref transportCapacity, ref availableSpace, ref availableGarbage, ref garbageAmount);
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
        while (parkedVehicles.Length > math.max(0, prefabGarbageFacilityData.m_VehicleCapacity + availableVehicles - vehicleCapacity1))
        {
          int index = random.NextInt(parkedVehicles.Length);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, parkedVehicles[index]);
          parkedVehicles.RemoveAtSwapBack(index);
        }
        for (int index = 0; index < parkedVehicles.Length; ++index)
        {
          Entity entity1 = parkedVehicles[index];
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.GarbageTruck garbageTruck = this.m_GarbageTruckData[entity1];
          bool disabled = availableVehicles <= 0 || availableSpace <= 0;
          if ((garbageTruck.m_State & GarbageTruckFlags.Disabled) > (GarbageTruckFlags) 0 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(GarbageFacilityAISystem.GarbageFacilityAction.SetDisabled(entity1, disabled));
          }
        }
        garbageFacility.m_DeliverGarbagePriority = availableGarbage <= 0 || !BuildingUtils.CheckOption(building, BuildingOption.Empty) ? (availableGarbage < minGarbageLoad ? 0.0f : (float) availableGarbage / (float) (prefabGarbageFacilityData.m_GarbageCapacity + areaCapacity + maxGarbageLoad)) : 2f;
        garbageFacility.m_AcceptGarbagePriority = availableSpace < minGarbageLoad ? 0.0f : (float) availableSpace / (float) (prefabGarbageFacilityData.m_GarbageCapacity + areaCapacity + maxGarbageLoad);
        if (!outside)
        {
          if ((double) garbageFacility.m_AcceptGarbagePriority > 0.0)
          {
            GarbageTransferRequestFlags flags = GarbageTransferRequestFlags.Deliver;
            if (transportCapacity <= 0)
              flags |= GarbageTransferRequestFlags.RequireTransport;
            int amount = math.min(availableSpace, maxGarbageLoad);
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageTransferRequestData.HasComponent(garbageFacility.m_GarbageDeliverRequest))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_GarbageTransferRequestData[garbageFacility.m_GarbageDeliverRequest].m_Flags != flags)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(garbageFacility.m_GarbageDeliverRequest, Entity.Null, true));
              }
              else
                flags = (GarbageTransferRequestFlags) 0;
            }
            if (flags != (GarbageTransferRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_GarbageTransferRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<GarbageTransferRequest>(jobIndex, entity3, new GarbageTransferRequest(entity, flags, garbageFacility.m_AcceptGarbagePriority, amount));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity3, new RequestGroup(8U));
            }
          }
          if ((double) garbageFacility.m_DeliverGarbagePriority > 0.0)
          {
            GarbageTransferRequestFlags flags = GarbageTransferRequestFlags.Receive;
            if (transportCapacity <= 0)
              flags |= GarbageTransferRequestFlags.RequireTransport;
            int amount = math.min(availableGarbage, maxGarbageLoad);
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageTransferRequestData.HasComponent(garbageFacility.m_GarbageReceiveRequest))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_GarbageTransferRequestData[garbageFacility.m_GarbageReceiveRequest].m_Flags != flags)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity4 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity4, new HandleRequest(garbageFacility.m_GarbageReceiveRequest, Entity.Null, true));
              }
              else
                flags = (GarbageTransferRequestFlags) 0;
            }
            if (flags != (GarbageTransferRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity5 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_GarbageTransferRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<GarbageTransferRequest>(jobIndex, entity5, new GarbageTransferRequest(entity, flags, garbageFacility.m_DeliverGarbagePriority, amount));
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity5, new RequestGroup(8U));
            }
          }
        }
        if (prefabGarbageFacilityData.m_LongTermStorage)
        {
          if (garbageAmount + areaGarbage >= prefabGarbageFacilityData.m_GarbageCapacity + areaCapacity)
          {
            if ((garbageFacility.m_Flags & GarbageFacilityFlags.IsFull) == (GarbageFacilityFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(entity, this.m_GarbageParameters.m_FacilityFullNotificationPrefab);
              garbageFacility.m_Flags |= GarbageFacilityFlags.IsFull;
            }
          }
          else if ((garbageFacility.m_Flags & GarbageFacilityFlags.IsFull) != (GarbageFacilityFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(entity, this.m_GarbageParameters.m_FacilityFullNotificationPrefab);
            garbageFacility.m_Flags &= ~GarbageFacilityFlags.IsFull;
          }
        }
        if (availableVehicles > 0)
          garbageFacility.m_Flags |= GarbageFacilityFlags.HasAvailableGarbageTrucks;
        else
          garbageFacility.m_Flags &= ~GarbageFacilityFlags.HasAvailableGarbageTrucks;
        if (availableSpace > 0)
          garbageFacility.m_Flags |= GarbageFacilityFlags.HasAvailableSpace;
        else
          garbageFacility.m_Flags &= ~GarbageFacilityFlags.HasAvailableSpace;
        if (prefabGarbageFacilityData.m_IndustrialWasteOnly)
          garbageFacility.m_Flags |= GarbageFacilityFlags.IndustrialWasteOnly;
        else
          garbageFacility.m_Flags &= ~GarbageFacilityFlags.IndustrialWasteOnly;
        if (availableVehicles <= 0 || availableSpace <= 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.RequestTargetIfNeeded(jobIndex, entity, ref garbageFacility, prefabGarbageFacilityData, availableVehicles);
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.GarbageFacility garbageFacility,
        GarbageFacilityData prefabGarbageFacilityData,
        int availableVehicles)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(garbageFacility.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 256U) - 1) != 80)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_GarbageCollectionRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<GarbageCollectionRequest>(jobIndex, entity1, new GarbageCollectionRequest(entity, availableVehicles, prefabGarbageFacilityData.m_IndustrialWasteOnly ? GarbageCollectionRequestFlags.IndustrialWaste : (GarbageCollectionRequestFlags) 0));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private bool TrySpawnGarbageTruck(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Entity request,
        GarbageFacilityData prefabGarbageFacilityData,
        ref Game.Buildings.GarbageFacility garbageFacility,
        ref int availableVehicles,
        ref int availableSpace,
        ref StackList<Entity> parkedVehicles)
      {
        GarbageCollectionRequest componentData1;
        // ISSUE: reference to a compiler-generated field
        if (availableVehicles <= 0 || availableSpace <= 0 || !this.m_GarbageCollectionRequestData.TryGetComponent(request, out componentData1))
          return false;
        Entity target = componentData1.m_Target;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target))
          return false;
        int2 garbageCapacity = new int2(1, availableSpace);
        Entity entity1 = Entity.Null;
        PathInformation componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathInformationData.TryGetComponent(request, out componentData2) && componentData2.m_Origin != entity)
        {
          PrefabRef componentData3;
          GarbageTruckData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.TryGetComponent(componentData2.m_Origin, out componentData3) && this.m_PrefabGarbageTruckData.TryGetComponent(componentData3.m_Prefab, out componentData4))
            garbageCapacity = (int2) componentData4.m_GarbageCapacity;
          if (!CollectionUtils.RemoveValueSwapBack<Entity>(ref parkedVehicles, componentData2.m_Origin))
            return false;
          // ISSUE: reference to a compiler-generated field
          ParkedCar parkedCar = this.m_ParkedCarData[componentData2.m_Origin];
          entity1 = componentData2.m_Origin;
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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          entity1 = this.m_GarbageTruckSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, this.m_TransformData[entity], entity, Entity.Null, ref garbageCapacity, false);
          if (entity1 == Entity.Null)
            return false;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity1, new Owner(entity));
        }
        --availableVehicles;
        availableSpace -= garbageCapacity.y;
        GarbageTruckFlags flags1 = (GarbageTruckFlags) 0;
        if (prefabGarbageFacilityData.m_IndustrialWasteOnly)
          flags1 |= GarbageTruckFlags.IndustrialWasteOnly;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Vehicles.GarbageTruck>(jobIndex, entity1, new Game.Vehicles.GarbageTruck(flags1, 1));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Common.Target>(jobIndex, entity1, new Game.Common.Target(target));
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
          this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, entity1, componentData2);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(garbageFacility.m_TargetRequest))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(garbageFacility.m_TargetRequest, Entity.Null, true));
        }
        return true;
      }

      private bool TrySpawnDeliveryTruck(
        int jobIndex,
        ref Unity.Mathematics.Random random,
        Entity entity,
        Entity request,
        ref int availableDeliveryTrucks,
        ref int availableSpace,
        ref int availableGarbage,
        ref int garbageAmount)
      {
        if (availableDeliveryTrucks <= 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        GarbageTransferRequest garbageTransferRequest = this.m_GarbageTransferRequestData[request];
        // ISSUE: reference to a compiler-generated field
        PathInformation component = this.m_PathInformationData[request];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(component.m_Destination))
          return false;
        DeliveryTruckFlags state = (DeliveryTruckFlags) 0;
        Resource resource = Resource.Garbage;
        Resource returnResource = Resource.NoResource;
        int amount1 = garbageTransferRequest.m_Amount;
        int returnAmount = 0;
        if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.RequireTransport) != (GarbageTransferRequestFlags) 0)
        {
          if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Deliver) != (GarbageTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Loaded;
          if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Receive) != (GarbageTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Buying;
        }
        else
        {
          if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Deliver) != (GarbageTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Buying;
          if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Receive) != (GarbageTransferRequestFlags) 0)
            state |= DeliveryTruckFlags.Loaded;
        }
        int amount2;
        if ((state & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
        {
          amount2 = math.min(math.min(amount1, availableGarbage), garbageAmount);
          if (amount2 <= 0)
            return false;
        }
        else
        {
          returnResource = resource;
          int x = amount1;
          resource = Resource.NoResource;
          amount2 = 0;
          returnAmount = math.min(x, amount2 + availableSpace);
          if (returnAmount <= 0)
            return false;
          state = state & ~DeliveryTruckFlags.Buying | DeliveryTruckFlags.Loaded;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity vehicle = this.m_DeliveryTruckSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, ref this.m_PrefabDeliveryTruckData, ref this.m_PrefabObjectData, resource, returnResource, ref amount2, ref returnAmount, this.m_TransformData[entity], entity, state);
        if (!(vehicle != Entity.Null))
          return false;
        --availableDeliveryTrucks;
        availableSpace += amount2 - returnAmount;
        availableGarbage -= amount2;
        garbageAmount -= amount2;
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
    private struct GarbageFacilityActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.GarbageTruck> m_GarbageTruckData;
      public NativeQueue<GarbageFacilityAISystem.GarbageFacilityAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        GarbageFacilityAISystem.GarbageFacilityAction garbageFacilityAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out garbageFacilityAction))
        {
          Game.Vehicles.GarbageTruck componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageTruckData.TryGetComponent(garbageFacilityAction.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            if (garbageFacilityAction.m_Disabled)
              componentData.m_State |= GarbageTruckFlags.Disabled;
            else
              componentData.m_State &= ~GarbageTruckFlags.Disabled;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_GarbageTruckData[garbageFacilityAction.m_Entity] = componentData;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> __Game_Buildings_GarbageFacility_RW_ComponentTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RW_BufferTypeHandle;
      public BufferTypeHandle<GuestVehicle> __Game_Vehicles_GuestVehicle_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> __Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageTransferRequest> __Game_Simulation_GarbageTransferRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.GarbageTruck> __Game_Vehicles_GarbageTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ReturnLoad> __Game_Vehicles_ReturnLoad_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageTruckData> __Game_Prefabs_GarbageTruckData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> __Game_Prefabs_StorageAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DeliveryTruckData> __Game_Prefabs_DeliveryTruckData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectData> __Game_Prefabs_ObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ResourceProductionData> __Game_Prefabs_ResourceProductionData_RO_BufferLookup;
      public ComponentLookup<Storage> __Game_Areas_Storage_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.GarbageTruck> __Game_Vehicles_GarbageTruck_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.GarbageFacility>();
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
        this.__Game_Simulation_GarbageCollectionRequest_RO_ComponentLookup = state.GetComponentLookup<GarbageCollectionRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup = state.GetComponentLookup<GarbageTransferRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GarbageTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.GarbageTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ReturnLoad_RO_ComponentLookup = state.GetComponentLookup<ReturnLoad>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup = state.GetComponentLookup<GarbageFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageTruckData_RO_ComponentLookup = state.GetComponentLookup<GarbageTruckData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageAreaData_RO_ComponentLookup = state.GetComponentLookup<StorageAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeliveryTruckData_RO_ComponentLookup = state.GetComponentLookup<DeliveryTruckData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RO_ComponentLookup = state.GetComponentLookup<ObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceProductionData_RO_BufferLookup = state.GetBufferLookup<ResourceProductionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Storage_RW_ComponentLookup = state.GetComponentLookup<Storage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_GarbageTruck_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.GarbageTruck>();
      }
    }
  }
}
