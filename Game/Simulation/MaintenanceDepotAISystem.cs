// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MaintenanceDepotAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.City;
using Game.Common;
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
  public class MaintenanceDepotAISystem : GameSystemBase
  {
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_VehiclePrefabQuery;
    private EntityArchetype m_MaintenanceRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_ParkedToMovingRemoveTypes;
    private ComponentTypeSet m_ParkedToMovingCarAddTypes;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private MaintenanceVehicleSelectData m_MaintenanceVehicleSelectData;
    private MaintenanceDepotAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 160;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleSelectData = new MaintenanceVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.MaintenanceDepot>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclePrefabQuery = this.GetEntityQuery(MaintenanceVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<MaintenanceRequest>(), ComponentType.ReadWrite<RequestGroup>());
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
      this.RequireForUpdate(this.m_BuildingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_VehiclePrefabQuery, Allocator.TempJob, out jobHandle1);
      NativeQueue<MaintenanceDepotAISystem.MaintenanceDepotAction> nativeQueue = new NativeQueue<MaintenanceDepotAISystem.MaintenanceDepotAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MaintenanceDepot_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MaintenanceDepotAISystem.MaintenanceDepotTickJob jobData1 = new MaintenanceDepotAISystem.MaintenanceDepotTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_OwnedVehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle,
        m_MaintenanceDepotType = this.__TypeHandle.__Game_Buildings_MaintenanceDepot_RW_ComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_SurfaceData = this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup,
        m_ParkData = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_MaintenanceVehicleData = this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NetConditionData = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabMaintenanceDepotData = this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_MaintenanceRequestArchetype = this.m_MaintenanceRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_ParkedToMovingRemoveTypes = this.m_ParkedToMovingRemoveTypes,
        m_ParkedToMovingCarAddTypes = this.m_ParkedToMovingCarAddTypes,
        m_MaintenanceVehicleSelectData = this.m_MaintenanceVehicleSelectData,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MaintenanceDepotAISystem.MaintenanceDepotActionJob jobData2 = new MaintenanceDepotAISystem.MaintenanceDepotActionJob()
      {
        m_MaintenanceVehicleData = this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup,
        m_ActionQueue = nativeQueue
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData1.ScheduleParallel<MaintenanceDepotAISystem.MaintenanceDepotTickJob>(this.m_BuildingQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.Schedule<MaintenanceDepotAISystem.MaintenanceDepotActionJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleSelectData.PostUpdate(jobHandle2);
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
    public MaintenanceDepotAISystem()
    {
    }

    private struct MaintenanceDepotAction
    {
      public Entity m_Entity;
      public bool m_Disabled;

      public static MaintenanceDepotAISystem.MaintenanceDepotAction SetDisabled(
        Entity vehicle,
        bool disabled)
      {
        // ISSUE: object of a compiler-generated type is created
        return new MaintenanceDepotAISystem.MaintenanceDepotAction()
        {
          m_Entity = vehicle,
          m_Disabled = disabled
        };
      }
    }

    [BurstCompile]
    private struct MaintenanceDepotTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      public ComponentTypeHandle<Game.Buildings.MaintenanceDepot> m_MaintenanceDepotType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceRequestType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Surface> m_SurfaceData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_ParkData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<NetCondition> m_NetConditionData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> m_PrefabMaintenanceDepotData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public EntityArchetype m_MaintenanceRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_ParkedToMovingCarAddTypes;
      [ReadOnly]
      public MaintenanceVehicleSelectData m_MaintenanceVehicleSelectData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<MaintenanceDepotAISystem.MaintenanceDepotAction>.ParallelWriter m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.MaintenanceDepot> nativeArray3 = chunk.GetNativeArray<Game.Buildings.MaintenanceDepot>(ref this.m_MaintenanceDepotType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor3 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor4 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceRequestType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          Game.Buildings.MaintenanceDepot maintenanceDepot = nativeArray3[index];
          DynamicBuffer<OwnedVehicle> vehicles = bufferAccessor3[index];
          DynamicBuffer<ServiceDispatch> dispatches = bufferAccessor4[index];
          MaintenanceDepotData data = new MaintenanceDepotData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabMaintenanceDepotData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            data = this.m_PrefabMaintenanceDepotData[prefabRef.m_Prefab];
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<MaintenanceDepotData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabMaintenanceDepotData);
          }
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          float immediateEfficiency = BuildingUtils.GetImmediateEfficiency(bufferAccessor1, index);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, prefabRef, ref random, ref maintenanceDepot, data, efficiency, immediateEfficiency, vehicles, dispatches);
          nativeArray3[index] = maintenanceDepot;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        ref Random random,
        ref Game.Buildings.MaintenanceDepot maintenanceDepot,
        MaintenanceDepotData prefabMaintenanceDepotData,
        float efficiency,
        float immediateEfficiency,
        DynamicBuffer<OwnedVehicle> vehicles,
        DynamicBuffer<ServiceDispatch> dispatches)
      {
        int vehicleCapacity1 = BuildingUtils.GetVehicleCapacity(efficiency, prefabMaintenanceDepotData.m_VehicleCapacity);
        int vehicleCapacity2 = BuildingUtils.GetVehicleCapacity(immediateEfficiency, prefabMaintenanceDepotData.m_VehicleCapacity);
        int availableVehicles = vehicleCapacity1;
        float efficiency1 = prefabMaintenanceDepotData.m_VehicleEfficiency * (float) (0.5 + (double) efficiency * 0.5);
        StackList<Entity> parkedVehicles = (StackList<Entity>) stackalloc Entity[vehicles.Length];
        for (int index = 0; index < vehicles.Length; ++index)
        {
          Entity vehicle = vehicles[index].m_Vehicle;
          Game.Vehicles.MaintenanceVehicle componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceVehicleData.TryGetComponent(vehicle, out componentData1))
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
              if ((componentData1.m_State & MaintenanceVehicleFlags.Disabled) > (MaintenanceVehicleFlags) 0 != disabled)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ActionQueue.Enqueue(MaintenanceDepotAISystem.MaintenanceDepotAction.SetDisabled(vehicle, disabled));
              }
            }
          }
        }
        int index1 = 0;
        while (index1 < dispatches.Length)
        {
          Entity request = dispatches[index1].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated method
            this.SpawnVehicle(jobIndex, ref random, entity, request, prefabMaintenanceDepotData, efficiency1, ref maintenanceDepot, ref availableVehicles, ref parkedVehicles);
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
        while (parkedVehicles.Length > math.max(0, prefabMaintenanceDepotData.m_VehicleCapacity + availableVehicles - vehicleCapacity1))
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
          Game.Vehicles.MaintenanceVehicle maintenanceVehicle = this.m_MaintenanceVehicleData[entity1];
          bool disabled = availableVehicles <= 0;
          if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Disabled) > (MaintenanceVehicleFlags) 0 != disabled)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ActionQueue.Enqueue(MaintenanceDepotAISystem.MaintenanceDepotAction.SetDisabled(entity1, disabled));
          }
        }
        if (availableVehicles != 0)
        {
          maintenanceDepot.m_Flags |= MaintenanceDepotFlags.HasAvailableVehicles;
          // ISSUE: reference to a compiler-generated method
          this.RequestTargetIfNeeded(jobIndex, entity, ref maintenanceDepot, availableVehicles);
        }
        else
          maintenanceDepot.m_Flags &= ~MaintenanceDepotFlags.HasAvailableVehicles;
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Buildings.MaintenanceDepot maintenanceDepot,
        int availableVehicles)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceRequestData.HasComponent(maintenanceDepot.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 256U) - 1) != 160)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MaintenanceRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<MaintenanceRequest>(jobIndex, entity1, new MaintenanceRequest(entity, availableVehicles));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private void SpawnVehicle(
        int jobIndex,
        ref Random random,
        Entity entity,
        Entity request,
        MaintenanceDepotData prefabMaintenanceDepotData,
        float efficiency,
        ref Game.Buildings.MaintenanceDepot maintenanceDepot,
        ref int availableVehicles,
        ref StackList<Entity> parkedVehicles)
      {
        if (availableVehicles <= 0)
          return;
        Entity target = Entity.Null;
        MaintenanceType allMaintenanceTypes = MaintenanceType.None;
        MaintenanceRequest componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceRequestData.TryGetComponent(request, out componentData1))
        {
          target = componentData1.m_Target;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          allMaintenanceTypes = BuildingUtils.GetMaintenanceType(target, ref this.m_ParkData, ref this.m_NetConditionData, ref this.m_EdgeData, ref this.m_SurfaceData, ref this.m_VehicleData);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target))
          return;
        Entity entity1 = Entity.Null;
        PathInformation componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathInformationData.TryGetComponent(request, out componentData2) && componentData2.m_Origin != entity)
        {
          if (!CollectionUtils.RemoveValueSwapBack<Entity>(ref parkedVehicles, componentData2.m_Origin))
            return;
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
          entity1 = this.m_MaintenanceVehicleSelectData.CreateVehicle(this.m_CommandBuffer, jobIndex, ref random, this.m_TransformData[entity], entity, Entity.Null, allMaintenanceTypes, MaintenanceType.None, (float4) float.MaxValue, false);
          if (entity1 == Entity.Null)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity1, new Owner(entity));
        }
        --availableVehicles;
        MaintenanceVehicleFlags flags1 = (MaintenanceVehicleFlags) 0;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetConditionData.HasComponent(target))
        {
          flags1 |= MaintenanceVehicleFlags.EdgeTarget;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(target))
            flags1 |= MaintenanceVehicleFlags.TransformTarget;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Vehicles.MaintenanceVehicle>(jobIndex, entity1, new Game.Vehicles.MaintenanceVehicle(flags1, 1, efficiency));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Target>(jobIndex, entity1, new Target(target));
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
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathInformation>(jobIndex, entity1, this.m_PathInformationData[request]);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ServiceRequestData.HasComponent(maintenanceDepot.m_TargetRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity3 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity3, new HandleRequest(maintenanceDepot.m_TargetRequest, Entity.Null, true));
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
    private struct MaintenanceDepotActionJob : IJob
    {
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleData;
      public NativeQueue<MaintenanceDepotAISystem.MaintenanceDepotAction> m_ActionQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        MaintenanceDepotAISystem.MaintenanceDepotAction maintenanceDepotAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out maintenanceDepotAction))
        {
          Game.Vehicles.MaintenanceVehicle componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceVehicleData.TryGetComponent(maintenanceDepotAction.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            if (maintenanceDepotAction.m_Disabled)
              componentData.m_State |= MaintenanceVehicleFlags.Disabled;
            else
              componentData.m_State &= ~MaintenanceVehicleFlags.Disabled;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_MaintenanceVehicleData[maintenanceDepotAction.m_Entity] = componentData;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.MaintenanceDepot> __Game_Buildings_MaintenanceDepot_RW_ComponentTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Surface> __Game_Objects_Surface_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> __Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCondition> __Game_Net_NetCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> __Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> __Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MaintenanceDepot_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.MaintenanceDepot>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RO_ComponentLookup = state.GetComponentLookup<ServiceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RO_ComponentLookup = state.GetComponentLookup<Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_MaintenanceVehicle_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.MaintenanceVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentLookup = state.GetComponentLookup<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup = state.GetComponentLookup<MaintenanceDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.MaintenanceVehicle>();
      }
    }
  }
}
