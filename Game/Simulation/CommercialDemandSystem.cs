// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CommercialDemandSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Debug;
using Game.Economy;
using Game.Prefabs;
using Game.Reflection;
using Game.Tools;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CommercialDemandSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private ResourceSystem m_ResourceSystem;
    private TaxSystem m_TaxSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private CountHouseholdDataSystem m_CountHouseholdDataSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_DemandParameterQuery;
    private EntityQuery m_FreeCommercialQuery;
    private EntityQuery m_CommercialProcessDataQuery;
    private EntityQuery m_UnlockedZoneDataQuery;
    private NativeValue<int> m_CompanyDemand;
    private NativeValue<int> m_BuildingDemand;
    [EnumArray(typeof (DemandFactor))]
    [DebugWatchValue]
    private NativeArray<int> m_DemandFactors;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ResourceDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_BuildingDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_Consumption;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_FreeProperties;
    [DebugWatchDeps]
    private JobHandle m_WriteDependencies;
    private JobHandle m_ReadDependencies;
    private int m_LastCompanyDemand;
    private int m_LastBuildingDemand;
    private CommercialDemandSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 4;

    [DebugWatchValue(color = "#008fff")]
    public int companyDemand => this.m_LastCompanyDemand;

    [DebugWatchValue(color = "#2b6795")]
    public int buildingDemand => this.m_LastBuildingDemand;

    public NativeArray<int> GetDemandFactors(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_DemandFactors;
    }

    public NativeArray<int> GetResourceDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_ResourceDemands;
    }

    public NativeArray<int> GetBuildingDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_BuildingDemands;
    }

    public NativeArray<int> GetConsumption(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_Consumption;
    }

    public void AddReader(JobHandle reader)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, reader);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountHouseholdDataSystem = this.World.GetOrCreateSystemManaged<CountHouseholdDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreeCommercialQuery = this.GetEntityQuery(ComponentType.ReadOnly<CommercialProperty>(), ComponentType.ReadOnly<PropertyOnMarket>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Abandoned>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Condemned>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialProcessDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.ReadOnly<ServiceCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedZoneDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoneData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DemandFactors = new NativeArray<int>(18, Allocator.Persistent);
      int resourceCount = EconomyUtils.ResourceCount;
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeProperties = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DemandParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CommercialProcessDataQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DemandFactors.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeProperties.Dispose();
      base.OnDestroy();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_DemandFactors.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeProperties.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_LastCompanyDemand = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastBuildingDemand = 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_CompanyDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_BuildingDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_DemandFactors.Length);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_DemandFactors);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ResourceDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_BuildingDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Consumption);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_FreeProperties);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastBuildingDemand);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num1;
      reader.Read(out num1);
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyDemand.value = num1;
      int num2;
      reader.Read(out num2);
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingDemand.value = num2;
      if (reader.context.version < Version.demandFactorCountSerialization)
      {
        NativeArray<int> src = new NativeArray<int>(13, Allocator.Temp);
        reader.Read(src);
        // ISSUE: reference to a compiler-generated field
        CollectionUtils.CopySafe<int>(src, this.m_DemandFactors);
        src.Dispose();
      }
      else
      {
        int length;
        reader.Read(out length);
        // ISSUE: reference to a compiler-generated field
        if (length == this.m_DemandFactors.Length)
        {
          // ISSUE: reference to a compiler-generated field
          reader.Read(this.m_DemandFactors);
        }
        else
        {
          NativeArray<int> src = new NativeArray<int>(length, Allocator.Temp);
          reader.Read(src);
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.CopySafe<int>(src, this.m_DemandFactors);
          src.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_ResourceDemands);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_BuildingDemands);
      NativeArray<int> nativeArray = new NativeArray<int>();
      if (reader.context.version < Version.companyDemandOptimization)
      {
        nativeArray = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        reader.Read(nativeArray);
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_Consumption);
      if (reader.context.version < Version.companyDemandOptimization)
      {
        reader.Read(nativeArray);
        reader.Read(nativeArray);
        reader.Read(nativeArray);
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_FreeProperties);
      if (reader.context.version < Version.companyDemandOptimization)
      {
        reader.Read(nativeArray);
        nativeArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastBuildingDemand);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_DemandParameterQuery.IsEmptyIgnoreFilter || this.m_EconomyParameterQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastCompanyDemand = this.m_CompanyDemand.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastBuildingDemand = this.m_BuildingDemand.value;
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      CountCompanyDataSystem.CommercialCompanyDatas commercialCompanyDatas = this.m_CountCompanyDataSystem.GetCommercialCompanyDatas(out deps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Tourism_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CommercialDemandSystem.UpdateCommercialDemandJob jobData = new CommercialDemandSystem.UpdateCommercialDemandJob()
      {
        m_FreePropertyChunks = this.m_FreeCommercialQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_UnlockedZoneDatas = this.m_UnlockedZoneDataQuery.ToComponentDataArray<ZoneData>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferTypeHandle,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_CommercialCompanies = this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_DemandParameters = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>(),
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_CompanyDemand = this.m_CompanyDemand,
        m_BuildingDemand = this.m_BuildingDemand,
        m_DemandFactors = this.m_DemandFactors,
        m_City = this.m_CitySystem.City,
        m_ResourceDemands = this.m_ResourceDemands,
        m_BuildingDemands = this.m_BuildingDemands,
        m_ProduceCapacity = commercialCompanyDatas.m_ProduceCapacity,
        m_CurrentAvailables = commercialCompanyDatas.m_CurrentAvailables,
        m_ResourceNeeds = this.m_CountHouseholdDataSystem.GetResourceNeeds(),
        m_FreeProperties = this.m_FreeProperties,
        m_Propertyless = commercialCompanyDatas.m_ServicePropertyless,
        m_Tourisms = this.__TypeHandle.__Game_City_Tourism_RO_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<CommercialDemandSystem.UpdateCommercialDemandJob>(JobUtils.CombineDependencies(this.Dependency, this.m_ReadDependencies, outJobHandle, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CountHouseholdDataSystem.AddHouseholdDataReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
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
    public CommercialDemandSystem()
    {
    }

    [BurstCompile]
    private struct UpdateCommercialDemandJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ZoneData> m_UnlockedZoneDatas;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_FreePropertyChunks;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<CommercialCompany> m_CommercialCompanies;
      [ReadOnly]
      public ComponentLookup<Tourism> m_Tourisms;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public DemandParameterData m_DemandParameters;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public NativeValue<int> m_CompanyDemand;
      public NativeValue<int> m_BuildingDemand;
      public NativeArray<int> m_DemandFactors;
      public NativeArray<int> m_FreeProperties;
      public NativeArray<int> m_ResourceDemands;
      public NativeArray<int> m_BuildingDemands;
      [ReadOnly]
      public NativeArray<int> m_ResourceNeeds;
      [ReadOnly]
      public NativeArray<int> m_ProduceCapacity;
      [ReadOnly]
      public NativeArray<int> m_CurrentAvailables;
      [ReadOnly]
      public NativeArray<int> m_Propertyless;

      public void Execute()
      {
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UnlockedZoneDatas.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_UnlockedZoneDatas[index].m_AreaType == AreaType.Commercial)
          {
            flag1 = true;
            break;
          }
        }
        ResourceIterator iterator1 = ResourceIterator.GetIterator();
        while (iterator1.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator1.resource);
          // ISSUE: reference to a compiler-generated field
          this.m_FreeProperties[resourceIndex] = 0;
          // ISSUE: reference to a compiler-generated field
          this.m_BuildingDemands[resourceIndex] = 0;
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceDemands[resourceIndex] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DemandFactors.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_DemandFactors[index] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_FreePropertyChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk freePropertyChunk = this.m_FreePropertyChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray = freePropertyChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Renter> bufferAccessor = freePropertyChunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Entity prefab = nativeArray[index2].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingPropertyDatas.HasComponent(prefab))
            {
              bool flag2 = false;
              DynamicBuffer<Renter> dynamicBuffer = bufferAccessor[index2];
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_CommercialCompanies.HasComponent(dynamicBuffer[index3].m_Renter))
                {
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                // ISSUE: reference to a compiler-generated field
                BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
                ResourceIterator iterator2 = ResourceIterator.GetIterator();
                while (iterator2.Next())
                {
                  if ((buildingPropertyData.m_AllowedSold & iterator2.resource) != Resource.NoResource)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_FreeProperties[EconomyUtils.GetResourceIndex(iterator2.resource)]++;
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyDemand.value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingDemand.value = 0;
        ResourceIterator iterator3 = ResourceIterator.GetIterator();
        int num1 = 0;
        while (iterator3.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator3.resource);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (EconomyUtils.IsCommercialResource(iterator3.resource) && this.m_ResourceDatas.HasComponent(this.m_ResourcePrefabs[iterator3.resource]))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float num2 = (float) (-0.05000000074505806 * ((double) TaxSystem.GetCommercialTaxRate(iterator3.resource, this.m_TaxRates) - 10.0));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num3 = this.m_ResourceNeeds[resourceIndex] != 0 || iterator3.resource == Resource.Lodging ? this.m_ResourceNeeds[resourceIndex] : 100;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num4 = this.m_CurrentAvailables[resourceIndex] == 0 ? this.m_ProduceCapacity[resourceIndex] : this.m_CurrentAvailables[resourceIndex];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ResourceDemands[resourceIndex] = Mathf.RoundToInt((1f + num2) * math.clamp(math.max(this.m_DemandParameters.m_CommercialBaseDemand * (float) num3 - (float) num4, 0.0f), 0.0f, 100f));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (iterator3.resource == Resource.Lodging && math.max((int) ((double) this.m_Tourisms[this.m_City].m_CurrentTourists * (double) this.m_DemandParameters.m_HotelRoomPercentRequirement) - this.m_Tourisms[this.m_City].m_Lodging.y, 0) > 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ResourceDemands[resourceIndex] = 100;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResourceDemands[resourceIndex] > 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CompanyDemand.value += this.m_ResourceDemands[resourceIndex];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_BuildingDemands[resourceIndex] = this.m_FreeProperties[resourceIndex] - this.m_Propertyless[resourceIndex] > 0 ? 0 : this.m_ResourceDemands[resourceIndex];
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingDemands[resourceIndex] > 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_BuildingDemand.value += this.m_BuildingDemands[resourceIndex];
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int resourceDemand1 = this.m_BuildingDemands[resourceIndex] > 0 ? this.m_ResourceDemands[resourceIndex] : 0;
              // ISSUE: reference to a compiler-generated field
              int resourceDemand2 = this.m_ResourceDemands[resourceIndex];
              int num5 = Mathf.RoundToInt(100f * num2);
              int num6 = resourceDemand2 + num5;
              if (iterator3.resource == Resource.Lodging)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_DemandFactors[9] += resourceDemand2;
              }
              else if (iterator3.resource == Resource.Petrochemicals)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_DemandFactors[16] += resourceDemand2;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_DemandFactors[4] += resourceDemand2;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_DemandFactors[11] += num5;
              // ISSUE: reference to a compiler-generated field
              this.m_DemandFactors[13] += math.min(0, resourceDemand1 - num6);
              ++num1;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyDemand.value = num1 != 0 ? math.clamp(this.m_CompanyDemand.value / num1, 0, 100) : 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingDemand.value = num1 != 0 & flag1 ? math.clamp(this.m_BuildingDemand.value / num1, 0, 100) : 0;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Renter> __Game_Buildings_Renter_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CommercialCompany> __Game_Companies_CommercialCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tourism> __Game_City_Tourism_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferTypeHandle = state.GetBufferTypeHandle<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CommercialCompany_RO_ComponentLookup = state.GetComponentLookup<CommercialCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Tourism_RO_ComponentLookup = state.GetComponentLookup<Tourism>(true);
      }
    }
  }
}
