// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TaxSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Agents;
using Game.Areas;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System;
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
  public class TaxSystem : 
    GameSystemBase,
    ITaxSystem,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    public static readonly int kUpdatesPerDay = 32;
    private NativeArray<int> m_TaxRates;
    private EntityQuery m_ResidentialTaxPayerGroup;
    private EntityQuery m_CommercialTaxPayerGroup;
    private EntityQuery m_IndustrialTaxPayerGroup;
    private EntityQuery m_TaxParameterGroup;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private SimulationSystem m_SimulationSystem;
    private ResourceSystem m_ResourceSystem;
    private TaxParameterData m_TaxParameterData;
    private JobHandle m_Readers;
    private TaxSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (TaxSystem.kUpdatesPerDay * 16);
    }

    public int TaxRate
    {
      get => this.m_TaxRates[0];
      set
      {
        this.m_TaxRates[0] = math.min(this.m_TaxParameterData.m_TotalTaxLimits.y, math.max(this.m_TaxParameterData.m_TotalTaxLimits.x, value));
        this.EnsureAreaTaxRateLimits(TaxAreaType.Residential);
        this.EnsureAreaTaxRateLimits(TaxAreaType.Commercial);
        this.EnsureAreaTaxRateLimits(TaxAreaType.Industrial);
        this.EnsureAreaTaxRateLimits(TaxAreaType.Office);
      }
    }

    public TaxParameterData GetTaxParameterData()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_TaxParameterGroup.IsEmptyIgnoreFilter)
        return new TaxParameterData();
      // ISSUE: reference to a compiler-generated method
      this.EnsureTaxParameterData();
      // ISSUE: reference to a compiler-generated field
      return this.m_TaxParameterData;
    }

    public static int GetTax(TaxPayer payer)
    {
      return (int) math.round(0.01f * (float) payer.m_AverageTaxRate * (float) payer.m_UntaxedIncome);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_TaxRates.Length);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_TaxRates);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Game.Version.averageTaxRate))
        return;
      int length;
      if (reader.context.version >= Game.Version.taxRateArrayLength)
        reader.Read(out length);
      else
        length = 53;
      NativeArray<int> nativeArray = new NativeArray<int>(length, Allocator.Temp);
      reader.Read(nativeArray);
      for (int index = 0; index < nativeArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxRates[index] = nativeArray[index];
      }
      nativeArray.Dispose();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates[0] = 10;
      // ISSUE: reference to a compiler-generated field
      for (int index = 1; index < this.m_TaxRates.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxRates[index] = 0;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialTaxPayerGroup = this.GetEntityQuery(ComponentType.ReadWrite<TaxPayer>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Resources>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.ReadOnly<Household>());
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialTaxPayerGroup = this.GetEntityQuery(ComponentType.ReadWrite<TaxPayer>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Resources>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.ReadOnly<ServiceAvailable>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialTaxPayerGroup = this.GetEntityQuery(ComponentType.ReadWrite<TaxPayer>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Resources>(), ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Game.Companies.StorageCompany>(), ComponentType.Exclude<ServiceAvailable>());
      // ISSUE: reference to a compiler-generated field
      this.m_TaxParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<TaxParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates = new NativeArray<int>(90, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates[0] = 10;
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TaxParameterGroup);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Game.Version.averageTaxRate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates[0] = 10;
      // ISSUE: reference to a compiler-generated field
      for (int index = 1; index < this.m_TaxRates.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxRates[index] = 0;
      }
    }

    public NativeArray<int> GetTaxRates() => this.m_TaxRates;

    public JobHandle Readers => this.m_Readers;

    public void AddReader(JobHandle reader)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Readers = JobHandle.CombineDependencies(this.m_Readers, reader);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.EnsureTaxParameterData();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame1 = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, TaxSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TaxParameterData = this.m_TaxParameterGroup.GetSingleton<TaxParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = new TaxSystem.PayTaxJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TaxPayerType = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ResourcePrefabs = prefabs,
        m_Type = IncomeSource.TaxResidential,
        m_UpdateFrameIndex = updateFrame1,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      }.ScheduleParallel<TaxSystem.PayTaxJob>(this.m_ResidentialTaxPayerGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame2 = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, TaxSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new TaxSystem.PayTaxJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TaxPayerType = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ResourcePrefabs = prefabs,
        m_Type = IncomeSource.TaxCommercial,
        m_UpdateFrameIndex = updateFrame2,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      }.ScheduleParallel<TaxSystem.PayTaxJob>(this.m_CommercialTaxPayerGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle3 = new TaxSystem.PayTaxJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TaxPayerType = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ResourcePrefabs = prefabs,
        m_Type = IncomeSource.TaxIndustrial,
        m_UpdateFrameIndex = updateFrame2,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      }.ScheduleParallel<TaxSystem.PayTaxJob>(this.m_IndustrialTaxPayerGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle3);
      this.Dependency = JobHandle.CombineDependencies(jobHandle1, jobHandle2, jobHandle3);
    }

    public int GetTaxRate(TaxAreaType areaType) => TaxSystem.GetTaxRate(areaType, this.m_TaxRates);

    public static int GetTaxRate(TaxAreaType areaType, NativeArray<int> taxRates)
    {
      return taxRates[0] + taxRates[(int) areaType];
    }

    public int2 GetTaxRateRange(TaxAreaType areaType)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return areaType == TaxAreaType.Residential ? this.GetTaxRate(areaType) + this.GetJobLevelTaxRateRange() : this.GetTaxRate(areaType) + this.GetResourceTaxRateRange(areaType);
    }

    public int GetModifiedTaxRate(
      TaxAreaType areaType,
      Entity district,
      BufferLookup<DistrictModifier> policies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetModifiedTaxRate(areaType, this.m_TaxRates, district, policies);
    }

    public static int GetModifiedTaxRate(
      TaxAreaType areaType,
      NativeArray<int> taxRates,
      Entity district,
      BufferLookup<DistrictModifier> policies)
    {
      // ISSUE: reference to a compiler-generated method
      float taxRate = (float) TaxSystem.GetTaxRate(areaType, taxRates);
      if (policies.HasBuffer(district))
      {
        DynamicBuffer<DistrictModifier> policy = policies[district];
        AreaUtils.ApplyModifier(ref taxRate, policy, DistrictModifierType.LowCommercialTax);
      }
      return (int) math.round(taxRate);
    }

    private void EnsureTaxParameterData()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_TaxParameterData.m_TotalTaxLimits.x != this.m_TaxParameterData.m_TotalTaxLimits.y)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TaxParameterData = this.m_TaxParameterGroup.GetSingleton<TaxParameterData>();
    }

    public void SetTaxRate(TaxAreaType areaType, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates[(int) areaType] = rate - this.m_TaxRates[0];
      // ISSUE: reference to a compiler-generated method
      this.EnsureAreaTaxRateLimits(areaType);
    }

    private void EnsureAreaTaxRateLimits(TaxAreaType areaType)
    {
      int index = (int) areaType;
      switch (areaType)
      {
        case TaxAreaType.Residential:
          // ISSUE: reference to a compiler-generated field
          int2 residentialTaxLimits = this.m_TaxParameterData.m_ResidentialTaxLimits;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          this.m_TaxRates[index] = math.min(residentialTaxLimits.y, math.max(residentialTaxLimits.x, this.GetTaxRate(areaType))) - this.m_TaxRates[0];
          for (int jobLevel = 0; jobLevel < 5; ++jobLevel)
          {
            // ISSUE: reference to a compiler-generated method
            this.EnsureJobLevelTaxRateLimits(jobLevel);
          }
          break;
        case TaxAreaType.Commercial:
          // ISSUE: reference to a compiler-generated field
          int2 commercialTaxLimits = this.m_TaxParameterData.m_CommercialTaxLimits;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          this.m_TaxRates[index] = math.min(commercialTaxLimits.y, math.max(commercialTaxLimits.x, this.GetTaxRate(areaType))) - this.m_TaxRates[0];
          ResourceIterator iterator1 = ResourceIterator.GetIterator();
          while (iterator1.Next())
          {
            // ISSUE: reference to a compiler-generated method
            this.EnsureResourceTaxRateLimits(areaType, iterator1.resource);
          }
          break;
        case TaxAreaType.Industrial:
          // ISSUE: reference to a compiler-generated field
          int2 industrialTaxLimits = this.m_TaxParameterData.m_IndustrialTaxLimits;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          this.m_TaxRates[index] = math.min(industrialTaxLimits.y, math.max(industrialTaxLimits.x, this.GetTaxRate(areaType))) - this.m_TaxRates[0];
          ResourceIterator iterator2 = ResourceIterator.GetIterator();
          while (iterator2.Next())
          {
            // ISSUE: reference to a compiler-generated method
            this.EnsureResourceTaxRateLimits(areaType, iterator2.resource);
          }
          break;
        case TaxAreaType.Office:
          // ISSUE: reference to a compiler-generated field
          int2 officeTaxLimits = this.m_TaxParameterData.m_OfficeTaxLimits;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          this.m_TaxRates[index] = math.min(officeTaxLimits.y, math.max(officeTaxLimits.x, this.GetTaxRate(areaType))) - this.m_TaxRates[0];
          ResourceIterator iterator3 = ResourceIterator.GetIterator();
          while (iterator3.Next())
          {
            // ISSUE: reference to a compiler-generated method
            this.EnsureResourceTaxRateLimits(areaType, iterator3.resource);
          }
          break;
      }
    }

    private void EnsureJobLevelTaxRateLimits(int jobLevel)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_TaxRates[5 + jobLevel] = math.min(this.m_TaxParameterData.m_JobLevelTaxLimits.y, math.max(this.m_TaxParameterData.m_JobLevelTaxLimits.x, this.GetResidentialTaxRate(jobLevel))) - this.GetTaxRate(TaxAreaType.Residential);
    }

    private void ClampResidentialTaxRates()
    {
      // ISSUE: reference to a compiler-generated method
      int2 levelTaxRateRange = this.GetJobLevelTaxRateRange();
      int num = 0;
      if (levelTaxRateRange.x > 0)
        num = levelTaxRateRange.x;
      else if (levelTaxRateRange.y < 0)
        num = levelTaxRateRange.y;
      if (num == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates[1] += num;
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TaxRates[5 + index] -= num;
      }
    }

    private int2 GetJobLevelTaxRateRange()
    {
      int2 levelTaxRateRange = new int2(int.MaxValue, int.MinValue);
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        int taxRate = this.m_TaxRates[5 + index];
        levelTaxRateRange.x = math.min(levelTaxRateRange.x, taxRate);
        levelTaxRateRange.y = math.max(levelTaxRateRange.y, taxRate);
      }
      return levelTaxRateRange;
    }

    private void EnsureResourceTaxRateLimits(TaxAreaType areaType, Resource resource)
    {
      switch (areaType)
      {
        case TaxAreaType.Commercial:
          // ISSUE: reference to a compiler-generated method
          int taxRate1 = this.GetTaxRate(TaxAreaType.Commercial);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TaxRates[10 + EconomyUtils.GetResourceIndex(resource)] = math.min(this.m_TaxParameterData.m_ResourceTaxLimits.y, math.max(this.m_TaxParameterData.m_ResourceTaxLimits.x, this.GetCommercialTaxRate(resource))) - taxRate1;
          break;
        case TaxAreaType.Industrial:
          // ISSUE: reference to a compiler-generated method
          int taxRate2 = this.GetTaxRate(TaxAreaType.Industrial);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TaxRates[50 + EconomyUtils.GetResourceIndex(resource)] = math.min(this.m_TaxParameterData.m_ResourceTaxLimits.y, math.max(this.m_TaxParameterData.m_ResourceTaxLimits.x, this.GetIndustrialTaxRate(resource))) - taxRate2;
          break;
        case TaxAreaType.Office:
          // ISSUE: reference to a compiler-generated method
          int taxRate3 = this.GetTaxRate(TaxAreaType.Office);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_TaxRates[50 + EconomyUtils.GetResourceIndex(resource)] = math.min(this.m_TaxParameterData.m_ResourceTaxLimits.y, math.max(this.m_TaxParameterData.m_ResourceTaxLimits.x, this.GetOfficeTaxRate(resource))) - taxRate3;
          break;
      }
    }

    private void ClampResourceTaxRates(TaxAreaType areaType)
    {
      // ISSUE: reference to a compiler-generated method
      int2 resourceTaxRateRange = this.GetResourceTaxRateRange(areaType);
      int num = 0;
      if (resourceTaxRateRange.x > 0)
        num = resourceTaxRateRange.x;
      else if (resourceTaxRateRange.y < 0)
        num = resourceTaxRateRange.y;
      if (num == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_TaxRates[(int) areaType] += num;
      // ISSUE: reference to a compiler-generated method
      int zeroOffset = this.GetZeroOffset(areaType);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      ResourceIterator iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
      {
        TaxableResourceData component;
        if (this.EntityManager.TryGetComponent<TaxableResourceData>(prefabs[iterator.resource], out component) && component.Contains(areaType))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TaxRates[zeroOffset + EconomyUtils.GetResourceIndex(iterator.resource)] -= num;
        }
      }
    }

    private int2 GetResourceTaxRateRange(TaxAreaType areaType)
    {
      int2 resourceTaxRateRange = new int2(int.MaxValue, int.MinValue);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      ResourceIterator iterator = ResourceIterator.GetIterator();
      // ISSUE: reference to a compiler-generated method
      int zeroOffset = this.GetZeroOffset(areaType);
      while (iterator.Next())
      {
        TaxableResourceData component;
        if (this.EntityManager.TryGetComponent<TaxableResourceData>(prefabs[iterator.resource], out component) && component.Contains(areaType))
        {
          // ISSUE: reference to a compiler-generated field
          int taxRate = this.m_TaxRates[zeroOffset + EconomyUtils.GetResourceIndex(iterator.resource)];
          resourceTaxRateRange.x = math.min(resourceTaxRateRange.x, taxRate);
          resourceTaxRateRange.y = math.max(resourceTaxRateRange.y, taxRate);
        }
      }
      return resourceTaxRateRange;
    }

    private int GetZeroOffset(TaxAreaType areaType)
    {
      switch (areaType)
      {
        case TaxAreaType.Commercial:
          return 10;
        case TaxAreaType.Industrial:
        case TaxAreaType.Office:
          return 50;
        default:
          throw new ArgumentOutOfRangeException(nameof (areaType), (object) areaType, (string) null);
      }
    }

    public int GetResidentialTaxRate(int jobLevel)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetResidentialTaxRate(jobLevel, this.m_TaxRates);
    }

    public static int GetResidentialTaxRate(int jobLevel, NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetTaxRate(TaxAreaType.Residential, taxRates) + taxRates[5 + jobLevel];
    }

    public void SetResidentialTaxRate(int jobLevel, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxRates[5 + jobLevel] = rate - this.GetTaxRate(TaxAreaType.Residential);
      // ISSUE: reference to a compiler-generated method
      this.EnsureJobLevelTaxRateLimits(jobLevel);
      // ISSUE: reference to a compiler-generated method
      this.ClampResidentialTaxRates();
    }

    public int GetCommercialTaxRate(Resource resource)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetCommercialTaxRate(resource, this.m_TaxRates);
    }

    public static int GetCommercialTaxRate(Resource resource, NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetTaxRate(TaxAreaType.Commercial, taxRates) + taxRates[10 + EconomyUtils.GetResourceIndex(resource)];
    }

    public int GetModifiedCommercialTaxRate(
      Resource resource,
      Entity district,
      BufferLookup<DistrictModifier> policies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetModifiedCommercialTaxRate(resource, this.m_TaxRates, district, policies);
    }

    public static int GetModifiedCommercialTaxRate(
      Resource resource,
      NativeArray<int> taxRates,
      Entity district,
      BufferLookup<DistrictModifier> policies)
    {
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetModifiedTaxRate(TaxAreaType.Commercial, taxRates, district, policies) + taxRates[10 + EconomyUtils.GetResourceIndex(resource)];
    }

    public void SetCommercialTaxRate(Resource resource, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxRates[10 + EconomyUtils.GetResourceIndex(resource)] = rate - this.GetTaxRate(TaxAreaType.Commercial);
      // ISSUE: reference to a compiler-generated method
      this.EnsureResourceTaxRateLimits(TaxAreaType.Commercial, resource);
      // ISSUE: reference to a compiler-generated method
      this.ClampResourceTaxRates(TaxAreaType.Commercial);
    }

    public int GetIndustrialTaxRate(Resource resource)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetIndustrialTaxRate(resource, this.m_TaxRates);
    }

    public static int GetIndustrialTaxRate(Resource resource, NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetTaxRate(TaxAreaType.Industrial, taxRates) + taxRates[50 + EconomyUtils.GetResourceIndex(resource)];
    }

    public void SetIndustrialTaxRate(Resource resource, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxRates[50 + EconomyUtils.GetResourceIndex(resource)] = rate - this.GetTaxRate(TaxAreaType.Industrial);
      // ISSUE: reference to a compiler-generated method
      this.EnsureResourceTaxRateLimits(TaxAreaType.Industrial, resource);
      // ISSUE: reference to a compiler-generated method
      this.ClampResourceTaxRates(TaxAreaType.Industrial);
    }

    public int GetOfficeTaxRate(Resource resource)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetOfficeTaxRate(resource, this.m_TaxRates);
    }

    public static int GetOfficeTaxRate(Resource resource, NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetTaxRate(TaxAreaType.Office, taxRates) + taxRates[50 + EconomyUtils.GetResourceIndex(resource)];
    }

    public void SetOfficeTaxRate(Resource resource, int rate)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxRates[50 + EconomyUtils.GetResourceIndex(resource)] = rate - this.GetTaxRate(TaxAreaType.Office);
      // ISSUE: reference to a compiler-generated method
      this.EnsureResourceTaxRateLimits(TaxAreaType.Office, resource);
      // ISSUE: reference to a compiler-generated method
      this.ClampResourceTaxRates(TaxAreaType.Office);
    }

    public int GetTaxRateEffect(TaxAreaType areaType, int taxRate) => 0;

    public int GetEstimatedTaxAmount(
      TaxAreaType areaType,
      TaxResultType resultType,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetEstimatedTaxAmount(areaType, resultType, statisticsLookup, stats, this.m_TaxRates);
    }

    public static int GetEstimatedTaxAmount(
      TaxAreaType areaType,
      TaxResultType resultType,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      NativeArray<int> taxRates)
    {
      int estimatedTaxAmount = 0;
      switch (areaType)
      {
        case TaxAreaType.Residential:
          for (int jobLevel = 0; jobLevel < 5; ++jobLevel)
          {
            // ISSUE: reference to a compiler-generated method
            int residentialTaxIncome = TaxSystem.GetEstimatedResidentialTaxIncome(jobLevel, statisticsLookup, stats, taxRates);
            // ISSUE: reference to a compiler-generated method
            if (TaxSystem.MatchesResultType(residentialTaxIncome, resultType))
              estimatedTaxAmount += residentialTaxIncome;
          }
          return estimatedTaxAmount;
        case TaxAreaType.Commercial:
          ResourceIterator iterator1 = ResourceIterator.GetIterator();
          while (iterator1.Next())
          {
            // ISSUE: reference to a compiler-generated method
            int commercialTaxIncome = TaxSystem.GetEstimatedCommercialTaxIncome(iterator1.resource, statisticsLookup, stats, taxRates);
            // ISSUE: reference to a compiler-generated method
            if (TaxSystem.MatchesResultType(commercialTaxIncome, resultType))
              estimatedTaxAmount += commercialTaxIncome;
          }
          return estimatedTaxAmount;
        case TaxAreaType.Industrial:
          ResourceIterator iterator2 = ResourceIterator.GetIterator();
          while (iterator2.Next())
          {
            // ISSUE: reference to a compiler-generated method
            int industrialTaxIncome = TaxSystem.GetEstimatedIndustrialTaxIncome(iterator2.resource, statisticsLookup, stats, taxRates);
            // ISSUE: reference to a compiler-generated method
            if (TaxSystem.MatchesResultType(industrialTaxIncome, resultType))
              estimatedTaxAmount += industrialTaxIncome;
          }
          return estimatedTaxAmount;
        case TaxAreaType.Office:
          ResourceIterator iterator3 = ResourceIterator.GetIterator();
          while (iterator3.Next())
          {
            // ISSUE: reference to a compiler-generated method
            int estimatedOfficeTaxIncome = TaxSystem.GetEstimatedOfficeTaxIncome(iterator3.resource, statisticsLookup, stats, taxRates);
            // ISSUE: reference to a compiler-generated method
            if (TaxSystem.MatchesResultType(estimatedOfficeTaxIncome, resultType))
              estimatedTaxAmount += estimatedOfficeTaxIncome;
          }
          return estimatedTaxAmount;
        default:
          return 0;
      }
    }

    private static bool MatchesResultType(int amount, TaxResultType resultType)
    {
      if (resultType == TaxResultType.Any || resultType == TaxResultType.Income && amount > 0)
        return true;
      return resultType == TaxResultType.Expense && amount < 0;
    }

    public int GetEstimatedResidentialTaxIncome(
      int jobLevel,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetEstimatedResidentialTaxIncome(jobLevel, statisticsLookup, stats, this.m_TaxRates);
    }

    public int GetEstimatedCommercialTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetEstimatedCommercialTaxIncome(resource, statisticsLookup, stats, this.m_TaxRates);
    }

    public int GetEstimatedIndustrialTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetEstimatedIndustrialTaxIncome(resource, statisticsLookup, stats, this.m_TaxRates);
    }

    public int GetEstimatedOfficeTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return TaxSystem.GetEstimatedOfficeTaxIncome(resource, statisticsLookup, stats, this.m_TaxRates);
    }

    public static int GetEstimatedResidentialTaxIncome(
      int jobLevel,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return (int) ((long) TaxSystem.GetResidentialTaxRate(jobLevel, taxRates) * (long) CityStatisticsSystem.GetStatisticValue(statisticsLookup, stats, StatisticType.ResidentialTaxableIncome, jobLevel) / 100L);
    }

    public static int GetEstimatedCommercialTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return (int) ((long) TaxSystem.GetCommercialTaxRate(resource, taxRates) * (long) CityStatisticsSystem.GetStatisticValue(statisticsLookup, stats, StatisticType.CommercialTaxableIncome, EconomyUtils.GetResourceIndex(resource)) / 100L);
    }

    public static int GetEstimatedIndustrialTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return (int) ((long) TaxSystem.GetIndustrialTaxRate(resource, taxRates) * (long) CityStatisticsSystem.GetStatisticValue(statisticsLookup, stats, StatisticType.IndustrialTaxableIncome, EconomyUtils.GetResourceIndex(resource)) / 100L);
    }

    public static int GetEstimatedOfficeTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      NativeArray<int> taxRates)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return (int) ((long) TaxSystem.GetOfficeTaxRate(resource, taxRates) * (long) CityStatisticsSystem.GetStatisticValue(statisticsLookup, stats, StatisticType.OfficeTaxableIncome, EconomyUtils.GetResourceIndex(resource)) / 100L);
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
    public TaxSystem()
    {
    }

    [BurstCompile]
    private struct PayTaxJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<TaxPayer> m_TaxPayerType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public BufferTypeHandle<Resources> m_ResourceType;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_ProcessDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public uint m_UpdateFrameIndex;
      public IncomeSource m_Type;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      private void PayTax(
        ref TaxPayer taxPayer,
        Entity entity,
        DynamicBuffer<Resources> resources,
        IncomeSource taxType,
        NativeQueue<StatisticsEvent>.ParallelWriter statisticsEventQueue)
      {
        // ISSUE: reference to a compiler-generated method
        int tax = TaxSystem.GetTax(taxPayer);
        EconomyUtils.AddResources(Resource.Money, -tax, resources);
        if (tax != 0)
        {
          if (taxType == IncomeSource.TaxResidential)
          {
            ref NativeQueue<StatisticsEvent>.ParallelWriter local1 = ref statisticsEventQueue;
            StatisticsEvent statisticsEvent1 = new StatisticsEvent();
            statisticsEvent1.m_Statistic = StatisticType.Income;
            statisticsEvent1.m_Change = (float) tax;
            statisticsEvent1.m_Parameter = (int) taxType;
            StatisticsEvent statisticsEvent2 = statisticsEvent1;
            local1.Enqueue(statisticsEvent2);
            int num = 0;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdCitizens.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[entity];
              for (int index = 0; index < householdCitizen.Length; ++index)
              {
                Entity citizen = householdCitizen[index].m_Citizen;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Workers.HasComponent(citizen))
                {
                  // ISSUE: reference to a compiler-generated field
                  num = (int) this.m_Workers[citizen].m_Level;
                  break;
                }
              }
            }
            ref NativeQueue<StatisticsEvent>.ParallelWriter local2 = ref statisticsEventQueue;
            statisticsEvent1 = new StatisticsEvent();
            statisticsEvent1.m_Statistic = StatisticType.ResidentialTaxableIncome;
            // ISSUE: reference to a compiler-generated field
            statisticsEvent1.m_Change = (float) (taxPayer.m_UntaxedIncome * TaxSystem.kUpdatesPerDay);
            statisticsEvent1.m_Parameter = num;
            StatisticsEvent statisticsEvent3 = statisticsEvent1;
            local2.Enqueue(statisticsEvent3);
          }
          else
          {
            int num = 0;
            StatisticType statisticType = taxType == IncomeSource.TaxCommercial ? StatisticType.CommercialTaxableIncome : StatisticType.IndustrialTaxableIncome;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Prefabs.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              Entity prefab = this.m_Prefabs[entity].m_Prefab;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ProcessDatas.HasComponent(prefab))
              {
                // ISSUE: reference to a compiler-generated field
                Resource resource = this.m_ProcessDatas[prefab].m_Output.m_Resource;
                num = EconomyUtils.GetResourceIndex(resource);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (statisticType == StatisticType.IndustrialTaxableIncome && (double) this.m_ResourceDatas[this.m_ResourcePrefabs[resource]].m_Weight == 0.0)
                {
                  taxType = IncomeSource.TaxOffice;
                  statisticType = StatisticType.OfficeTaxableIncome;
                }
              }
            }
            ref NativeQueue<StatisticsEvent>.ParallelWriter local3 = ref statisticsEventQueue;
            StatisticsEvent statisticsEvent4 = new StatisticsEvent();
            statisticsEvent4.m_Statistic = StatisticType.Income;
            statisticsEvent4.m_Change = (float) tax;
            statisticsEvent4.m_Parameter = (int) taxType;
            StatisticsEvent statisticsEvent5 = statisticsEvent4;
            local3.Enqueue(statisticsEvent5);
            ref NativeQueue<StatisticsEvent>.ParallelWriter local4 = ref statisticsEventQueue;
            statisticsEvent4 = new StatisticsEvent();
            statisticsEvent4.m_Statistic = statisticType;
            // ISSUE: reference to a compiler-generated field
            statisticsEvent4.m_Change = (float) (taxPayer.m_UntaxedIncome * TaxSystem.kUpdatesPerDay);
            statisticsEvent4.m_Parameter = num;
            StatisticsEvent statisticsEvent6 = statisticsEvent4;
            local4.Enqueue(statisticsEvent6);
          }
        }
        taxPayer.m_UntaxedIncome = 0;
      }

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
        NativeArray<TaxPayer> nativeArray2 = chunk.GetNativeArray<TaxPayer>(ref this.m_TaxPayerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor = chunk.GetBufferAccessor<Resources>(ref this.m_ResourceType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          TaxPayer taxPayer = nativeArray2[index];
          DynamicBuffer<Resources> resources = bufferAccessor[index];
          if (taxPayer.m_UntaxedIncome != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.PayTax(ref taxPayer, nativeArray1[index], resources, this.m_Type, this.m_StatisticsEventQueue);
            nativeArray2[index] = taxPayer;
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
      public ComponentTypeHandle<TaxPayer> __Game_Agents_TaxPayer_RW_ComponentTypeHandle;
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_TaxPayer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxPayer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
      }
    }
  }
}
