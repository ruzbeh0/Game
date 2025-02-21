// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IndustrialAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class IndustrialAISystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 32;
    public static readonly int kLowestCompanyWorth = -10000;
    public static readonly int kMinimumEmployee = 5;
    public static readonly int kMaxVirtualResourceStorage = 100000;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private ResourceSystem m_ResourceSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private EntityQuery m_CompanyQuery;
    private EntityQuery m_EconomyParameterQuery;
    private IndustrialAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (IndustrialAISystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.ReadOnly<BuyingCompany>(), ComponentType.ReadWrite<WorkProvider>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Resources>(), ComponentType.Exclude<ServiceAvailable>(), ComponentType.Exclude<Game.Companies.ExtractorCompany>(), ComponentType.Exclude<Game.Companies.StorageCompany>(), ComponentType.Exclude<Created>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning() => base.OnStopRunning();

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, IndustrialAISystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_PropertySeeker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      IndustrialAISystem.CompanyAITickJob jobData = new IndustrialAISystem.CompanyAITickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle,
        m_EmployeeBufType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_VehicleType = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle,
        m_StorageLimitDatas = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_PropertySeekers = this.__TypeHandle.__Game_Agents_PropertySeeker_RO_ComponentLookup,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_EfficiencyBuf = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_Layouts = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_Trucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_CitizenDatas = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Random = RandomSeed.Next(),
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_IndustrialCompanyDatas = this.m_CountCompanyDataSystem.GetIndustrialCompanyDatas(out deps),
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<IndustrialAISystem.CompanyAITickJob>(this.m_CompanyQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
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
    public IndustrialAISystem()
    {
    }

    [BurstCompile]
    private struct CompanyAITickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourceType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeBufType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_VehicleType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimitDatas;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<PropertySeeker> m_PropertySeekers;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public BufferLookup<Efficiency> m_EfficiencyBuf;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_Layouts;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_Trucks;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenDatas;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameters;
      [ReadOnly]
      public CountCompanyDataSystem.IndustrialCompanyDatas m_IndustrialCompanyDatas;
      public uint m_UpdateFrameIndex;
      public RandomSeed m_Random;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor1 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourceType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<OwnedVehicle> bufferAccessor2 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_VehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor3 = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeBufType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          WorkProvider workProvider = nativeArray2[index];
          DynamicBuffer<Employee> dynamicBuffer = bufferAccessor3[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab1 = this.m_Prefabs[entity].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          StorageLimitData storageLimitData = this.m_StorageLimitDatas[prefab1];
          // ISSUE: reference to a compiler-generated field
          IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenters.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity prefab2 = this.m_Prefabs[this.m_PropertyRenters[entity].m_Property].m_Prefab;
            int resourceIndex = EconomyUtils.GetResourceIndex(industrialProcessData.m_Output.m_Resource);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num = this.m_IndustrialCompanyDatas.m_Demand[resourceIndex] - this.m_IndustrialCompanyDatas.m_Production[resourceIndex];
            int length = dynamicBuffer.Length;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int officeFittingWorkers = CompanyUtils.GetIndustrialAndOfficeFittingWorkers(this.m_BuildingDatas[prefab2], this.m_BuildingPropertyDatas[prefab2], (int) this.m_SpawnableBuildingDatas[prefab2].m_Level, industrialProcessData);
            int resources = EconomyUtils.GetResources(industrialProcessData.m_Output.m_Resource, bufferAccessor1[index]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) EconomyUtils.GetWeight(industrialProcessData.m_Output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              if (workProvider.m_MaxWorkers > IndustrialAISystem.kMinimumEmployee && resources >= storageLimitData.m_Limit / 2 && num < 0)
                --workProvider.m_MaxWorkers;
              else if (length == workProvider.m_MaxWorkers && officeFittingWorkers - workProvider.m_MaxWorkers > 1 && resources <= storageLimitData.m_Limit / 4)
                ++workProvider.m_MaxWorkers;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (workProvider.m_MaxWorkers > IndustrialAISystem.kMinimumEmployee && resources >= IndustrialAISystem.kMaxVirtualResourceStorage && num < 0)
              {
                --workProvider.m_MaxWorkers;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (length == workProvider.m_MaxWorkers && officeFittingWorkers - workProvider.m_MaxWorkers > 1 && resources <= IndustrialAISystem.kMaxVirtualResourceStorage / 2)
                  ++workProvider.m_MaxWorkers;
              }
            }
            nativeArray2[index] = workProvider;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PropertySeekers.HasComponent(entity) && (!this.m_PropertyRenters.HasComponent(entity) || this.m_Random.GetRandom(entity.Index).NextInt(4) == 0))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (EconomyUtils.GetCompanyTotalWorth(bufferAccessor1[index], bufferAccessor2[index], this.m_Layouts, this.m_Trucks, this.m_ResourcePrefabs, this.m_ResourceDatas) > IndustrialAISystem.kLowestCompanyWorth)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PropertySeeker>(unfilteredChunkIndex, entity, new PropertySeeker());
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PropertyRenters.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity, new Deleted());
              }
            }
          }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RW_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertySeeker> __Game_Agents_PropertySeeker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferTypeHandle = state.GetBufferTypeHandle<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferTypeHandle = state.GetBufferTypeHandle<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_PropertySeeker_RO_ComponentLookup = state.GetComponentLookup<PropertySeeker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
      }
    }
  }
}
