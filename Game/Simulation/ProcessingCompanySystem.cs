// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ProcessingCompanySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Serialization;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ProcessingCompanySystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    public const int kMaxCommercialOutputResource = 5000;
    public const float kMaximumTransportUnitCost = 0.03f;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private ResourceSystem m_ResourceSystem;
    private TaxSystem m_TaxSystem;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private ProductionSpecializationSystem m_ProductionSpecializationSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_CompanyGroup;
    private NativeArray<long> m_ProducedResources;
    private JobHandle m_ProducedResourcesDeps;
    private ProcessingCompanySystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1038562630_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / (EconomyUtils.kCompanyUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionSpecializationSystem = this.World.GetOrCreateSystemManaged<ProductionSpecializationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetExistingSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyGroup = this.GetEntityQuery(ComponentType.ReadWrite<Game.Companies.ProcessingCompany>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadWrite<Game.Economy.Resources>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Employee>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Game.Companies.ExtractorCompany>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyGroup);
      this.RequireForUpdate<EconomyParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProducedResources = new NativeArray<long>(EconomyUtils.ResourceCount, Allocator.Persistent);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Game.Version.officeFix))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_CompanyGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        IndustrialProcessData componentData = this.EntityManager.GetComponentData<IndustrialProcessData>(this.EntityManager.GetComponentData<PrefabRef>(entityArray[index]).m_Prefab);
        if (!this.EntityManager.HasComponent<ServiceAvailable>(entityArray[index]) && (double) roComponentLookup[prefabs[componentData.m_Output.m_Resource]].m_Weight == 0.0)
        {
          DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(entityArray[index]);
          if (EconomyUtils.GetResources(componentData.m_Output.m_Resource, buffer) >= 500)
            EconomyUtils.AddResources(componentData.m_Output.m_Resource, -500, buffer);
        }
      }
      entityArray.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ProducedResources.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, EconomyUtils.kCompanyUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_SpecializationBonus_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Profitability_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProcessingCompanySystem.UpdateProcessingJob jobData = new ProcessingCompanySystem.UpdateProcessingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PropertyType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_ServiceAvailableType = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_TradeCostType = this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle,
        m_CompanyDataType = this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle,
        m_TaxPayerType = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle,
        m_ProfitabilityType = this.__TypeHandle.__Game_Companies_Profitability_RW_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_Limits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_Specializations = this.__TypeHandle.__Game_City_SpecializationBonus_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_BuildingEfficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup,
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData(),
        m_ProducedResources = this.m_ProducedResources,
        m_ProductionQueue = this.m_ProductionSpecializationSystem.GetQueue(out deps).AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EconomyParameters = this.__query_1038562630_0.GetSingleton<EconomyParameterData>(),
        m_RandomSeed = RandomSeed.Next(),
        m_City = this.m_CitySystem.City,
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ProcessingCompanySystem.UpdateProcessingJob>(this.m_CompanyGroup, JobHandle.CombineDependencies(this.m_ProducedResourcesDeps, deps, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ProductionSpecializationSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ProducedResourcesDeps = new JobHandle();
    }

    public NativeArray<long> GetProducedResourcesArray(out JobHandle dependencies)
    {
      dependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      return this.m_ProducedResources;
    }

    public void AddProducedResourcesReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ProducedResourcesDeps = JobHandle.CombineDependencies(this.m_ProducedResourcesDeps, handle);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write((byte) this.m_ProducedResources.Length);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ProducedResources.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_ProducedResources[index]);
      }
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num1;
      reader.Read(out num1);
      for (int index = 0; index < (int) num1; ++index)
      {
        long num2;
        reader.Read(out num2);
        // ISSUE: reference to a compiler-generated field
        if (index < this.m_ProducedResources.Length)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ProducedResources[index] = num2;
        }
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = (int) num1; index < this.m_ProducedResources.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ProducedResources[index] = 0L;
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ProducedResources.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ProducedResources[index] = 0L;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1038562630_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EconomyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public ProcessingCompanySystem()
    {
    }

    [BurstCompile]
    private struct UpdateProcessingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      public BufferTypeHandle<Game.Economy.Resources> m_ResourceType;
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      public ComponentTypeHandle<CompanyData> m_CompanyDataType;
      public ComponentTypeHandle<TaxPayer> m_TaxPayerType;
      public ComponentTypeHandle<Profitability> m_ProfitabilityType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public BufferLookup<SpecializationBonus> m_Specializations;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Efficiency> m_BuildingEfficiencies;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;
      public NativeArray<long> m_ProducedResources;
      public NativeQueue<ProductionSpecializationSystem.ProducedResource>.ParallelWriter m_ProductionQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public EconomyParameterData m_EconomyParameters;
      public RandomSeed m_RandomSeed;
      public Entity m_City;
      public uint m_UpdateFrameIndex;

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
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SpecializationBonus> specialization = this.m_Specializations[this.m_City];
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Economy.Resources> bufferAccessor1 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourceType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor2 = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CompanyData> nativeArray4 = chunk.GetNativeArray<CompanyData>(ref this.m_CompanyDataType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Profitability> nativeArray5 = chunk.GetNativeArray<Profitability>(ref this.m_ProfitabilityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TaxPayer> nativeArray6 = chunk.GetNativeArray<TaxPayer>(ref this.m_TaxPayerType);
        // ISSUE: reference to a compiler-generated field
        chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
        // ISSUE: reference to a compiler-generated field
        bool isCommercial = chunk.Has<ServiceAvailable>(ref this.m_ServiceAvailableType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity e = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          Entity property = nativeArray3[index].m_Property;
          Profitability profitability = nativeArray5[index];
          ref CompanyData local = ref nativeArray4.ElementAt<CompanyData>(index);
          // ISSUE: reference to a compiler-generated field
          if (this.m_Buildings.HasComponent(property))
          {
            DynamicBuffer<Game.Economy.Resources> resources1 = bufferAccessor1[index];
            // ISSUE: reference to a compiler-generated field
            IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab];
            // ISSUE: reference to a compiler-generated field
            StorageLimitData limit = this.m_Limits[prefab];
            float buildingEfficiency = 1f;
            DynamicBuffer<Efficiency> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingEfficiencies.TryGetBuffer(property, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateEfficiencyFactors(industrialProcessData, isCommercial, bufferData, cityModifier, specialization);
              buildingEfficiency = BuildingUtils.GetEfficiency(bufferData);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int productionPerDay = EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, !isCommercial, bufferAccessor2[index], industrialProcessData, this.m_ResourcePrefabs, this.m_ResourceDatas, this.m_Citizens, ref this.m_EconomyParameters);
            int num1 = MathUtils.RoundToIntRandom(ref random, 1f * (float) productionPerDay / (float) EconomyUtils.kCompanyUpdatesPerDay);
            ResourceStack input1 = industrialProcessData.m_Input1;
            ResourceStack input2 = industrialProcessData.m_Input2;
            ResourceStack output = industrialProcessData.m_Output;
            float num2 = 1f;
            float num3 = 1f;
            int amount1 = 0;
            int amount2 = 0;
            if (input1.m_Resource != Resource.NoResource && (double) input1.m_Amount > 0.0)
            {
              int resources2 = EconomyUtils.GetResources(input1.m_Resource, resources1);
              num2 = (float) input1.m_Amount * 1f / (float) output.m_Amount;
              num1 = math.min(num1, (int) ((double) resources2 / (double) num2));
            }
            if (input2.m_Resource != Resource.NoResource && (double) input2.m_Amount > 0.0)
            {
              int resources3 = EconomyUtils.GetResources(input2.m_Resource, resources1);
              num3 = (float) input2.m_Amount * 1f / (float) output.m_Amount;
              num1 = math.min(num1, (int) ((double) resources3 / (double) num3));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num4 = (float) ((isCommercial ? (double) EconomyUtils.GetMarketPrice(output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) : (double) EconomyUtils.GetIndustrialPrice(output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas)) - (double) input1.m_Amount * (double) EconomyUtils.GetIndustrialPrice(input1.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) / (double) output.m_Amount - (double) input2.m_Amount * (double) EconomyUtils.GetIndustrialPrice(input2.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) / (double) output.m_Amount);
            int num5;
            if ((double) num1 > 0.0)
            {
              int num6 = 0;
              if (!isCommercial || EconomyUtils.GetResources(output.m_Resource, resources1) <= 5000)
              {
                if (input1.m_Resource != Resource.NoResource)
                {
                  amount1 = -MathUtils.RoundToIntRandom(ref local.m_RandomSeed, (float) num1 * num2);
                  int num7 = EconomyUtils.AddResources(input1.m_Resource, amount1, resources1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num6 += (double) EconomyUtils.GetWeight(input1.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) > 0.0 ? num7 : 0;
                }
                if (input2.m_Resource != Resource.NoResource)
                {
                  amount2 = -MathUtils.RoundToIntRandom(ref local.m_RandomSeed, (float) num1 * num3);
                  int num8 = EconomyUtils.AddResources(input2.m_Resource, amount2, resources1);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  num6 += (double) EconomyUtils.GetWeight(input2.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) > 0.0 ? num8 : 0;
                }
                int x = limit.m_Limit - num6;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) EconomyUtils.GetWeight(output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) > 0.0)
                {
                  num1 = math.min(x, num1);
                }
                else
                {
                  int resources4 = EconomyUtils.GetResources(output.m_Resource, resources1);
                  // ISSUE: reference to a compiler-generated field
                  num1 = math.clamp(IndustrialAISystem.kMaxVirtualResourceStorage - resources4, 0, num1);
                }
                num5 = EconomyUtils.AddResources(output.m_Resource, num1, resources1);
                // ISSUE: reference to a compiler-generated method
                this.AddProducedResource(output.m_Resource, num1);
                if (!isCommercial && local.m_RandomSeed.NextInt(400000) < num1)
                {
                  // ISSUE: reference to a compiler-generated method
                  Resource randomUpkeepResource = this.GetRandomUpkeepResource(local, output.m_Resource);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (EconomyUtils.IsMaterial(randomUpkeepResource, this.m_ResourcePrefabs, ref this.m_ResourceDatas))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<GoodsDeliveryRequest>(unfilteredChunkIndex, entity, new GoodsDeliveryRequest()
                    {
                      m_Amount = 2000,
                      m_Flags = GoodsDeliveryFlags.BuildingUpkeep | GoodsDeliveryFlags.CommercialAllowed | GoodsDeliveryFlags.IndustrialAllowed | GoodsDeliveryFlags.ImportAllowed,
                      m_Resource = randomUpkeepResource,
                      m_Target = e
                    });
                  }
                }
              }
              else
                continue;
            }
            else
              num5 = EconomyUtils.GetResources(output.m_Resource, resources1);
            profitability.m_Profitability = (byte) math.min((float) byte.MaxValue, (float) ((double) (num1 * EconomyUtils.kCompanyUpdatesPerDay) * (double) num4 / 100.0));
            nativeArray5[index] = profitability;
            TaxPayer taxPayer = nativeArray6[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int y = isCommercial ? TaxSystem.GetCommercialTaxRate(output.m_Resource, this.m_TaxRates) : TaxSystem.GetIndustrialTaxRate(output.m_Resource, this.m_TaxRates);
            if (input1.m_Resource != output.m_Resource && (double) num1 > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int num9 = (int) ((double) num1 * (double) EconomyUtils.GetIndustrialPrice(output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) - (double) amount1 * (double) EconomyUtils.GetIndustrialPrice(input1.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) + (double) amount2 * (double) EconomyUtils.GetIndustrialPrice(input2.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas));
              if (num9 > 0)
                taxPayer.m_AverageTaxRate = Mathf.RoundToInt(math.lerp((float) taxPayer.m_AverageTaxRate, (float) y, (float) num9 / (float) (num9 + taxPayer.m_UntaxedIncome)));
              taxPayer.m_UntaxedIncome += num9;
              nativeArray6[index] = taxPayer;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!isCommercial && EconomyUtils.IsMaterial(output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) && num5 > 0)
            {
              DeliveryTruckSelectItem deliveryTruckSelectItem;
              // ISSUE: reference to a compiler-generated field
              this.m_DeliveryTruckSelectData.TrySelectItem(ref random, output.m_Resource, num5, out deliveryTruckSelectItem);
              if ((double) deliveryTruckSelectItem.m_Cost / (double) math.min(num5, deliveryTruckSelectItem.m_Capacity) < 0.029999999329447746)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<ResourceExporter>(unfilteredChunkIndex, e, new ResourceExporter()
                {
                  m_Resource = output.m_Resource,
                  m_Amount = math.max(0, math.min(deliveryTruckSelectItem.m_Capacity, num5))
                });
              }
            }
          }
        }
      }

      private void UpdateEfficiencyFactors(
        IndustrialProcessData process,
        bool isCommercial,
        DynamicBuffer<Efficiency> efficiencies,
        DynamicBuffer<CityModifier> cityModifiers,
        DynamicBuffer<SpecializationBonus> specializations)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.IsOffice(process))
        {
          float efficiency = 1f;
          CityUtils.ApplyModifier(ref efficiency, cityModifiers, CityModifierType.OfficeEfficiency);
          BuildingUtils.SetEfficiencyFactor(efficiencies, EfficiencyFactor.CityModifierOfficeEfficiency, efficiency);
        }
        else if (!isCommercial)
        {
          float efficiency = 1f;
          CityUtils.ApplyModifier(ref efficiency, cityModifiers, CityModifierType.IndustrialEfficiency);
          BuildingUtils.SetEfficiencyFactor(efficiencies, EfficiencyFactor.CityModifierIndustrialEfficiency, efficiency);
        }
        if (process.m_Output.m_Resource == Resource.Software)
        {
          float efficiency = 1f;
          CityUtils.ApplyModifier(ref efficiency, cityModifiers, CityModifierType.OfficeSoftwareEfficiency);
          BuildingUtils.SetEfficiencyFactor(efficiencies, EfficiencyFactor.CityModifierSoftware, efficiency);
        }
        else if (process.m_Output.m_Resource == Resource.Electronics)
        {
          float efficiency = 1f;
          CityUtils.ApplyModifier(ref efficiency, cityModifiers, CityModifierType.IndustrialElectronicsEfficiency);
          BuildingUtils.SetEfficiencyFactor(efficiencies, EfficiencyFactor.CityModifierElectronics, efficiency);
        }
        int resourceIndex = EconomyUtils.GetResourceIndex(process.m_Output.m_Resource);
        if (specializations.Length <= resourceIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float efficiency1 = 1f + specializations[resourceIndex].GetBonus(this.m_EconomyParameters.m_MaxCitySpecializationBonus, this.m_EconomyParameters.m_ResourceProductionCoefficient);
        BuildingUtils.SetEfficiencyFactor(efficiencies, EfficiencyFactor.SpecializationBonus, efficiency1);
      }

      private bool IsOffice(IndustrialProcessData process)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !EconomyUtils.IsMaterial(process.m_Output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas);
      }

      private Resource GetRandomUpkeepResource(CompanyData companyData, Resource outputResource)
      {
        switch (companyData.m_RandomSeed.NextInt(4))
        {
          case 0:
            return Resource.Software;
          case 1:
            return Resource.Telecom;
          case 2:
            return Resource.Financial;
          case 3:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (EconomyUtils.IsMaterial(outputResource, this.m_ResourcePrefabs, ref this.m_ResourceDatas))
              return Resource.Machinery;
            return !companyData.m_RandomSeed.NextBool() ? Resource.Furniture : Resource.Paper;
          default:
            return Resource.NoResource;
        }
      }

      private unsafe void AddProducedResource(Resource resource, int amount)
      {
        if (resource == Resource.NoResource)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: cast to a reference type
        Interlocked.Add((long&) ((IntPtr) this.m_ProducedResources.GetUnsafePtr<long>() + (IntPtr) EconomyUtils.GetResourceIndex(resource) * 8), (long) amount);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ProductionQueue.Enqueue(new ProductionSpecializationSystem.ProducedResource()
        {
          m_Resource = resource,
          m_Amount = amount
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentTypeHandle;
      public BufferTypeHandle<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public BufferTypeHandle<TradeCost> __Game_Companies_TradeCost_RW_BufferTypeHandle;
      public ComponentTypeHandle<CompanyData> __Game_Companies_CompanyData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<TaxPayer> __Game_Agents_TaxPayer_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Profitability> __Game_Companies_Profitability_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SpecializationBonus> __Game_City_SpecializationBonus_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RW_BufferTypeHandle = state.GetBufferTypeHandle<TradeCost>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_TaxPayer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxPayer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Profitability_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Profitability>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_SpecializationBonus_RO_BufferLookup = state.GetBufferLookup<SpecializationBonus>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferLookup = state.GetBufferLookup<Efficiency>();
      }
    }
  }
}
