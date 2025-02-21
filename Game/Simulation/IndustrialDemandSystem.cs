// Decompiled with JetBrains decompiler
// Type: Game.Simulation.IndustrialDemandSystem
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
using Game.Objects;
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
  public class IndustrialDemandSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private static readonly int kStorageProductionDemand = 2000;
    private static readonly int kStorageCompanyEstimateLimit = 864000;
    private ResourceSystem m_ResourceSystem;
    private CitySystem m_CitySystem;
    private ClimateSystem m_ClimateSystem;
    private TaxSystem m_TaxSystem;
    private CountHouseholdDataSystem m_CountHouseholdDataSystem;
    private CountWorkplacesSystem m_CountWorkplacesSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_DemandParameterQuery;
    private EntityQuery m_FreeIndustrialQuery;
    private EntityQuery m_StorageCompanyQuery;
    private EntityQuery m_ProcessDataQuery;
    private EntityQuery m_CityServiceQuery;
    private EntityQuery m_UnlockedZoneDataQuery;
    private NativeValue<int> m_IndustrialCompanyDemand;
    private NativeValue<int> m_IndustrialBuildingDemand;
    private NativeValue<int> m_StorageCompanyDemand;
    private NativeValue<int> m_StorageBuildingDemand;
    private NativeValue<int> m_OfficeCompanyDemand;
    private NativeValue<int> m_OfficeBuildingDemand;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ResourceDemands;
    [EnumArray(typeof (DemandFactor))]
    [DebugWatchValue]
    private NativeArray<int> m_IndustrialDemandFactors;
    [EnumArray(typeof (DemandFactor))]
    [DebugWatchValue]
    private NativeArray<int> m_OfficeDemandFactors;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_IndustrialCompanyDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_IndustrialZoningDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_IndustrialBuildingDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_StorageBuildingDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_StorageCompanyDemands;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_FreeProperties;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_FreeStorages;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_Storages;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_StorageCapacities;
    [DebugWatchDeps]
    private JobHandle m_WriteDependencies;
    private JobHandle m_ReadDependencies;
    private int m_LastIndustrialCompanyDemand;
    private int m_LastIndustrialBuildingDemand;
    private int m_LastStorageCompanyDemand;
    private int m_LastStorageBuildingDemand;
    private int m_LastOfficeCompanyDemand;
    private int m_LastOfficeBuildingDemand;
    private IndustrialDemandSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 7;

    [DebugWatchValue(color = "#f7dc6f")]
    public int industrialCompanyDemand => this.m_LastIndustrialCompanyDemand;

    [DebugWatchValue(color = "#b7950b")]
    public int industrialBuildingDemand => this.m_LastIndustrialBuildingDemand;

    [DebugWatchValue(color = "#cccccc")]
    public int storageCompanyDemand => this.m_LastStorageCompanyDemand;

    [DebugWatchValue(color = "#999999")]
    public int storageBuildingDemand => this.m_LastStorageBuildingDemand;

    [DebugWatchValue(color = "#af7ac5")]
    public int officeCompanyDemand => this.m_LastOfficeCompanyDemand;

    [DebugWatchValue(color = "#6c3483")]
    public int officeBuildingDemand => this.m_LastOfficeBuildingDemand;

    public NativeArray<int> GetConsumption(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_ResourceDemands;
    }

    public NativeArray<int> GetIndustrialDemandFactors(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_IndustrialDemandFactors;
    }

    public NativeArray<int> GetOfficeDemandFactors(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_OfficeDemandFactors;
    }

    public NativeArray<int> GetResourceDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_IndustrialCompanyDemands;
    }

    public NativeArray<int> GetBuildingDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_IndustrialBuildingDemands;
    }

    public NativeArray<int> GetStorageCompanyDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_StorageCompanyDemands;
    }

    public NativeArray<int> GetStorageBuildingDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_StorageBuildingDemands;
    }

    public NativeArray<int> GetIndustrialResourceDemands(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_ResourceDemands;
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
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountHouseholdDataSystem = this.World.GetOrCreateSystemManaged<CountHouseholdDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountWorkplacesSystem = this.World.GetOrCreateSystemManaged<CountWorkplacesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreeIndustrialQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProperty>(), ComponentType.ReadOnly<PropertyOnMarket>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Abandoned>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Condemned>());
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.Exclude<ServiceCompanyData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedZoneDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoneData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeCompanyDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeBuildingDemand = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandFactors = new NativeArray<int>(18, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeDemandFactors = new NativeArray<int>(18, Allocator.Persistent);
      int resourceCount = EconomyUtils.ResourceCount;
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialZoningDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemands = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeProperties = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeStorages = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Storages = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCapacities = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DemandParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ProcessDataQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeCompanyDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeBuildingDemand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandFactors.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeDemandFactors.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialZoningDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceDemands.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeProperties.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Storages.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeStorages.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCapacities.Dispose();
      base.OnDestroy();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeCompanyDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeBuildingDemand.value = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandFactors.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeDemandFactors.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialZoningDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemands.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeProperties.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_Storages.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeStorages.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_LastIndustrialCompanyDemand = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastIndustrialBuildingDemand = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastStorageCompanyDemand = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastStorageBuildingDemand = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastOfficeCompanyDemand = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_LastOfficeBuildingDemand = 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialCompanyDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialBuildingDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StorageCompanyDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StorageBuildingDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_OfficeCompanyDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_OfficeBuildingDemand.value);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialDemandFactors.Length);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialDemandFactors);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_OfficeDemandFactors);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialCompanyDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialZoningDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialBuildingDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StorageBuildingDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StorageCompanyDemands);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_FreeProperties);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Storages);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_FreeStorages);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastIndustrialCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastIndustrialBuildingDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastStorageCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastStorageBuildingDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastOfficeCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastOfficeBuildingDemand);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num1;
      reader.Read(out num1);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialCompanyDemand.value = num1;
      int num2;
      reader.Read(out num2);
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingDemand.value = num2;
      int num3;
      reader.Read(out num3);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageCompanyDemand.value = num3;
      int num4;
      reader.Read(out num4);
      // ISSUE: reference to a compiler-generated field
      this.m_StorageBuildingDemand.value = num4;
      int num5;
      reader.Read(out num5);
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeCompanyDemand.value = num5;
      int num6;
      reader.Read(out num6);
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeBuildingDemand.value = num6;
      if (reader.context.version < Version.demandFactorCountSerialization)
      {
        NativeArray<int> src = new NativeArray<int>(13, Allocator.Temp);
        reader.Read(src);
        // ISSUE: reference to a compiler-generated field
        CollectionUtils.CopySafe<int>(src, this.m_IndustrialDemandFactors);
        reader.Read(src);
        // ISSUE: reference to a compiler-generated field
        CollectionUtils.CopySafe<int>(src, this.m_OfficeDemandFactors);
        src.Dispose();
      }
      else
      {
        int length;
        reader.Read(out length);
        // ISSUE: reference to a compiler-generated field
        if (length == this.m_IndustrialDemandFactors.Length)
        {
          // ISSUE: reference to a compiler-generated field
          reader.Read(this.m_IndustrialDemandFactors);
          // ISSUE: reference to a compiler-generated field
          reader.Read(this.m_OfficeDemandFactors);
        }
        else
        {
          NativeArray<int> src = new NativeArray<int>(length, Allocator.Temp);
          reader.Read(src);
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.CopySafe<int>(src, this.m_IndustrialDemandFactors);
          reader.Read(src);
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.CopySafe<int>(src, this.m_OfficeDemandFactors);
          src.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_IndustrialCompanyDemands);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_IndustrialZoningDemands);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_IndustrialBuildingDemands);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_StorageBuildingDemands);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_StorageCompanyDemands);
      if (reader.context.version <= Version.companyDemandOptimization)
      {
        NativeArray<int> nativeArray = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        reader.Read(nativeArray);
        reader.Read(nativeArray);
        if (reader.context.version <= Version.demandFactorCountSerialization)
        {
          reader.Read(nativeArray);
          reader.Read(nativeArray);
        }
        reader.Read(nativeArray);
        reader.Read(nativeArray);
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_FreeProperties);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_Storages);
      // ISSUE: reference to a compiler-generated field
      reader.Read(this.m_FreeStorages);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastIndustrialCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastIndustrialBuildingDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastStorageCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastStorageBuildingDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastOfficeCompanyDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastOfficeBuildingDemand);
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
      this.m_LastIndustrialCompanyDemand = this.m_IndustrialCompanyDemand.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastIndustrialBuildingDemand = this.m_IndustrialBuildingDemand.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastStorageCompanyDemand = this.m_StorageCompanyDemand.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastStorageBuildingDemand = this.m_StorageBuildingDemand.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastOfficeCompanyDemand = this.m_OfficeCompanyDemand.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastOfficeBuildingDemand = this.m_OfficeBuildingDemand.value;
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      CountCompanyDataSystem.IndustrialCompanyDatas industrialCompanyDatas = this.m_CountCompanyDataSystem.GetIndustrialCompanyDatas(out deps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle outJobHandle3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      IndustrialDemandSystem.UpdateIndustrialDemandJob jobData = new IndustrialDemandSystem.UpdateIndustrialDemandJob()
      {
        m_FreePropertyChunks = this.m_FreeIndustrialQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_StorageCompanyChunks = this.m_StorageCompanyQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_CityServiceChunks = this.m_CityServiceQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle3),
        m_UnlockedZoneDatas = this.m_UnlockedZoneDataQuery.ToComponentDataArray<ZoneData>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ServiceUpkeepType = this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle,
        m_StorageLimitDatas = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingPropertyDatas = this.__TypeHandle.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_Attached = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ServiceUpkeeps = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_Upkeeps = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_DemandParameters = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_EmployableByEducation = this.m_CountHouseholdDataSystem.GetEmployables(),
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_FreeWorkplaces = this.m_CountWorkplacesSystem.GetFreeWorkplaces(),
        m_City = this.m_CitySystem.City,
        m_IndustrialCompanyDemand = this.m_IndustrialCompanyDemand,
        m_IndustrialBuildingDemand = this.m_IndustrialBuildingDemand,
        m_StorageCompanyDemand = this.m_StorageCompanyDemand,
        m_StorageBuildingDemand = this.m_StorageBuildingDemand,
        m_OfficeCompanyDemand = this.m_OfficeCompanyDemand,
        m_OfficeBuildingDemand = this.m_OfficeBuildingDemand,
        m_IndustrialCompanyDemands = this.m_IndustrialCompanyDemands,
        m_IndustrialBuildingDemands = this.m_IndustrialBuildingDemands,
        m_StorageBuildingDemands = this.m_StorageBuildingDemands,
        m_StorageCompanyDemands = this.m_StorageCompanyDemands,
        m_Propertyless = industrialCompanyDatas.m_ProductionPropertyless,
        m_CompanyResourceDemands = industrialCompanyDatas.m_Demand,
        m_FreeProperties = this.m_FreeProperties,
        m_Productions = industrialCompanyDatas.m_Production,
        m_Storages = this.m_Storages,
        m_FreeStorages = this.m_FreeStorages,
        m_StorageCapacities = this.m_StorageCapacities,
        m_IndustrialDemandFactors = this.m_IndustrialDemandFactors,
        m_OfficeDemandFactors = this.m_OfficeDemandFactors,
        m_ResourceDemands = this.m_ResourceDemands
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<IndustrialDemandSystem.UpdateIndustrialDemandJob>(JobUtils.CombineDependencies(this.Dependency, this.m_ReadDependencies, outJobHandle1, deps, outJobHandle2, outJobHandle3));
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CountCompanyDataSystem.AddReader(this.Dependency);
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
    public IndustrialDemandSystem()
    {
    }

    [BurstCompile]
    private struct UpdateIndustrialDemandJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ZoneData> m_UnlockedZoneDatas;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_FreePropertyChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_StorageCompanyChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_CityServiceChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<CityServiceUpkeep> m_ServiceUpkeepType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<Attached> m_Attached;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimitDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_ServiceUpkeeps;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_Upkeeps;
      public EconomyParameterData m_EconomyParameters;
      public DemandParameterData m_DemandParameters;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public NativeArray<int> m_EmployableByEducation;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      [ReadOnly]
      public Workplaces m_FreeWorkplaces;
      public Entity m_City;
      public NativeValue<int> m_IndustrialCompanyDemand;
      public NativeValue<int> m_IndustrialBuildingDemand;
      public NativeValue<int> m_StorageCompanyDemand;
      public NativeValue<int> m_StorageBuildingDemand;
      public NativeValue<int> m_OfficeCompanyDemand;
      public NativeValue<int> m_OfficeBuildingDemand;
      public NativeArray<int> m_IndustrialDemandFactors;
      public NativeArray<int> m_OfficeDemandFactors;
      public NativeArray<int> m_IndustrialCompanyDemands;
      public NativeArray<int> m_IndustrialBuildingDemands;
      public NativeArray<int> m_StorageBuildingDemands;
      public NativeArray<int> m_StorageCompanyDemands;
      [ReadOnly]
      public NativeArray<int> m_Productions;
      [ReadOnly]
      public NativeArray<int> m_CompanyResourceDemands;
      public NativeArray<int> m_FreeProperties;
      [ReadOnly]
      public NativeArray<int> m_Propertyless;
      public NativeArray<int> m_FreeStorages;
      public NativeArray<int> m_Storages;
      public NativeArray<int> m_StorageCapacities;
      public NativeArray<int> m_ResourceDemands;

      public void Execute()
      {
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UnlockedZoneDatas.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_UnlockedZoneDatas[index].m_AreaType == AreaType.Industrial)
          {
            flag1 = true;
            break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        ResourceIterator iterator1 = ResourceIterator.GetIterator();
        while (iterator1.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator1.resource);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ResourceData resourceData = this.m_ResourceDatas[this.m_ResourcePrefabs[iterator1.resource]];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceDemands[resourceIndex] = this.m_CompanyResourceDemands[resourceIndex] != 0 || !EconomyUtils.IsIndustrialResource(resourceData, false, false) ? this.m_CompanyResourceDemands[resourceIndex] : 100;
          // ISSUE: reference to a compiler-generated field
          this.m_FreeProperties[resourceIndex] = 0;
          // ISSUE: reference to a compiler-generated field
          this.m_Storages[resourceIndex] = 0;
          // ISSUE: reference to a compiler-generated field
          this.m_FreeStorages[resourceIndex] = 0;
          // ISSUE: reference to a compiler-generated field
          this.m_StorageCapacities[resourceIndex] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_IndustrialDemandFactors.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IndustrialDemandFactors[index] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_OfficeDemandFactors.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OfficeDemandFactors[index] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_CityServiceChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk cityServiceChunk = this.m_CityServiceChunks[index1];
          // ISSUE: reference to a compiler-generated field
          if (cityServiceChunk.Has<CityServiceUpkeep>(ref this.m_ServiceUpkeepType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = cityServiceChunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = cityServiceChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              Entity prefab1 = nativeArray2[index2].m_Prefab;
              Entity entity = nativeArray1[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceUpkeeps.HasBuffer(prefab1))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ServiceUpkeepData> serviceUpkeep = this.m_ServiceUpkeeps[prefab1];
                for (int index3 = 0; index3 < serviceUpkeep.Length; ++index3)
                {
                  ServiceUpkeepData serviceUpkeepData = serviceUpkeep[index3];
                  if (serviceUpkeepData.m_Upkeep.m_Resource != Resource.Money)
                  {
                    int amount = serviceUpkeepData.m_Upkeep.m_Amount;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ResourceDemands[EconomyUtils.GetResourceIndex(serviceUpkeepData.m_Upkeep.m_Resource)] += amount;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_InstalledUpgrades.HasBuffer(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<InstalledUpgrade> installedUpgrade = this.m_InstalledUpgrades[entity];
                for (int index4 = 0; index4 < installedUpgrade.Length; ++index4)
                {
                  Entity upgrade = installedUpgrade[index4].m_Upgrade;
                  // ISSUE: reference to a compiler-generated field
                  if (!BuildingUtils.CheckOption(installedUpgrade[index4], BuildingOption.Inactive) && this.m_Prefabs.HasComponent(upgrade))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Entity prefab2 = this.m_Prefabs[upgrade].m_Prefab;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Upkeeps.HasBuffer(prefab2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<ServiceUpkeepData> upkeep = this.m_Upkeeps[prefab2];
                      for (int index5 = 0; index5 < upkeep.Length; ++index5)
                      {
                        ServiceUpkeepData serviceUpkeepData = upkeep[index5];
                        // ISSUE: reference to a compiler-generated field
                        this.m_ResourceDemands[EconomyUtils.GetResourceIndex(serviceUpkeepData.m_Upkeep.m_Resource)] += serviceUpkeepData.m_Upkeep.m_Amount;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        int num1 = 0;
        int num2 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Productions.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ResourceData resourceData = this.m_ResourceDatas[this.m_ResourcePrefabs[EconomyUtils.GetResource(index)]];
          if (resourceData.m_IsProduceable)
          {
            if ((double) resourceData.m_Weight > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              num1 += this.m_Productions[index];
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              num2 += this.m_Productions[index];
            }
          }
        }
        int num3 = num2 + num1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceDemands[EconomyUtils.GetResourceIndex(Resource.Software)] += num3 / this.m_EconomyParameters.m_PerOfficeResourceNeededForIndustrial;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceDemands[EconomyUtils.GetResourceIndex(Resource.Financial)] += num3 / this.m_EconomyParameters.m_PerOfficeResourceNeededForIndustrial;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ResourceDemands[EconomyUtils.GetResourceIndex(Resource.Telecom)] += num3 / this.m_EconomyParameters.m_PerOfficeResourceNeededForIndustrial;
        // ISSUE: reference to a compiler-generated field
        for (int index6 = 0; index6 < this.m_StorageCompanyChunks.Length; ++index6)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk storageCompanyChunk = this.m_StorageCompanyChunks[index6];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray3 = storageCompanyChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = storageCompanyChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
          for (int index7 = 0; index7 < nativeArray3.Length; ++index7)
          {
            Entity entity = nativeArray3[index7];
            Entity prefab3 = nativeArray4[index7].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_IndustrialProcessDatas.HasComponent(prefab3))
            {
              // ISSUE: reference to a compiler-generated field
              int resourceIndex = EconomyUtils.GetResourceIndex(this.m_IndustrialProcessDatas[prefab3].m_Output.m_Resource);
              // ISSUE: reference to a compiler-generated field
              this.m_Storages[resourceIndex]++;
              // ISSUE: reference to a compiler-generated field
              StorageLimitData storageLimitData = this.m_StorageLimitDatas[prefab3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PropertyRenters.HasComponent(entity) || !this.m_Prefabs.HasComponent(this.m_PropertyRenters[entity].m_Property))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_FreeStorages[resourceIndex]--;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_StorageCapacities[resourceIndex] += IndustrialDemandSystem.kStorageCompanyEstimateLimit;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity prefab4 = this.m_Prefabs[this.m_PropertyRenters[entity].m_Property].m_Prefab;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_StorageCapacities[resourceIndex] += storageLimitData.GetAdjustedLimit(this.m_SpawnableBuildingDatas[prefab4], this.m_BuildingDatas[prefab4]);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index8 = 0; index8 < this.m_FreePropertyChunks.Length; ++index8)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk freePropertyChunk = this.m_FreePropertyChunks[index8];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray5 = freePropertyChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray6 = freePropertyChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
          for (int index9 = 0; index9 < nativeArray6.Length; ++index9)
          {
            Entity prefab = nativeArray6[index9].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingPropertyDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[prefab];
              Attached componentData1;
              PrefabRef componentData2;
              BuildingPropertyData componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Attached.TryGetComponent(nativeArray5[index9], out componentData1) && this.m_Prefabs.TryGetComponent(componentData1.m_Parent, out componentData2) && this.m_BuildingPropertyDatas.TryGetComponent(componentData2.m_Prefab, out componentData3))
                buildingPropertyData.m_AllowedManufactured &= componentData3.m_AllowedManufactured;
              ResourceIterator iterator2 = ResourceIterator.GetIterator();
              while (iterator2.Next())
              {
                int resourceIndex = EconomyUtils.GetResourceIndex(iterator2.resource);
                if ((buildingPropertyData.m_AllowedManufactured & iterator2.resource) != Resource.NoResource)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_FreeProperties[resourceIndex]++;
                }
                if ((buildingPropertyData.m_AllowedStored & iterator2.resource) != Resource.NoResource)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_FreeStorages[resourceIndex]++;
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        int num4 = this.m_IndustrialBuildingDemand.value;
        // ISSUE: reference to a compiler-generated field
        bool flag2 = this.m_OfficeBuildingDemand.value > 0;
        // ISSUE: reference to a compiler-generated field
        int num5 = this.m_StorageBuildingDemand.value;
        // ISSUE: reference to a compiler-generated field
        this.m_IndustrialCompanyDemand.value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_IndustrialBuildingDemand.value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_StorageCompanyDemand.value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_StorageBuildingDemand.value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_OfficeCompanyDemand.value = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_OfficeBuildingDemand.value = 0;
        int num6 = 0;
        int num7 = 0;
        ResourceIterator iterator3 = ResourceIterator.GetIterator();
        while (iterator3.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator3.resource);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResourceDatas.HasComponent(this.m_ResourcePrefabs[iterator3.resource]))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ResourceData resourceData = this.m_ResourceDatas[this.m_ResourcePrefabs[iterator3.resource]];
            bool isProduceable = resourceData.m_IsProduceable;
            bool isMaterial = resourceData.m_IsMaterial;
            bool isTradable = resourceData.m_IsTradable;
            bool flag3 = (double) resourceData.m_Weight == 0.0;
            if (isTradable && !flag3)
            {
              // ISSUE: reference to a compiler-generated field
              int resourceDemand = this.m_ResourceDemands[resourceIndex];
              // ISSUE: reference to a compiler-generated field
              this.m_StorageCompanyDemands[resourceIndex] = 0;
              // ISSUE: reference to a compiler-generated field
              this.m_StorageBuildingDemands[resourceIndex] = 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (resourceDemand > IndustrialDemandSystem.kStorageProductionDemand && this.m_StorageCapacities[resourceIndex] < resourceDemand)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_StorageCompanyDemands[resourceIndex] = 1;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_FreeStorages[resourceIndex] < 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_StorageBuildingDemands[resourceIndex] = 1;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StorageCompanyDemand.value += this.m_StorageCompanyDemands[resourceIndex];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StorageBuildingDemand.value += this.m_StorageBuildingDemands[resourceIndex];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IndustrialDemandFactors[17] += math.max(0, this.m_StorageBuildingDemands[resourceIndex]);
            }
            if (isProduceable)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float num8 = isMaterial ? this.m_DemandParameters.m_ExtractorBaseDemand : this.m_DemandParameters.m_IndustrialBaseDemand;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float num9 = (float) ((1.0 + (double) this.m_ResourceDemands[resourceIndex] - (double) this.m_Productions[resourceIndex]) / ((double) this.m_ResourceDemands[resourceIndex] + 1.0));
              if (iterator3.resource == Resource.Electronics)
                CityUtils.ApplyModifier(ref num8, cityModifier, CityModifierType.IndustrialElectronicsDemand);
              else if (iterator3.resource == Resource.Software)
                CityUtils.ApplyModifier(ref num8, cityModifier, CityModifierType.OfficeSoftwareDemand);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              float num10 = (float) ((double) this.m_DemandParameters.m_TaxEffect * -5.0 * ((flag3 ? (double) TaxSystem.GetOfficeTaxRate(iterator3.resource, this.m_TaxRates) : (double) TaxSystem.GetIndustrialTaxRate(iterator3.resource, this.m_TaxRates)) - 10.0));
              int x1 = 0;
              int x2 = 0;
              // ISSUE: reference to a compiler-generated field
              float num11 = this.m_DemandParameters.m_NeutralUnemployment / 100f;
              for (int index = 0; index < 5; ++index)
              {
                if (index < 2)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  x2 += (int) ((double) this.m_EmployableByEducation[index] * (1.0 - (double) num11)) - this.m_FreeWorkplaces[index];
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  x1 += (int) ((double) this.m_EmployableByEducation[index] * (1.0 - (double) num11)) - this.m_FreeWorkplaces[index];
                }
              }
              int num12 = math.clamp(x1, -10, 10);
              int num13 = math.clamp(x2, -10, 15);
              float num14 = 50f * math.max(0.0f, num8 * num9);
              if (flag3)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IndustrialCompanyDemands[resourceIndex] = Mathf.RoundToInt(num14 + num10 + (float) num12);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IndustrialCompanyDemands[resourceIndex] = math.min(100, math.max(0, this.m_IndustrialCompanyDemands[resourceIndex]));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_OfficeCompanyDemand.value += Mathf.RoundToInt((float) this.m_IndustrialCompanyDemands[resourceIndex]);
                ++num6;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IndustrialCompanyDemands[resourceIndex] = Mathf.RoundToInt(num14 + num10 + (float) num12 + (float) num13);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IndustrialCompanyDemands[resourceIndex] = math.min(100, math.max(0, this.m_IndustrialCompanyDemands[resourceIndex]));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IndustrialCompanyDemand.value += Mathf.RoundToInt((float) this.m_IndustrialCompanyDemands[resourceIndex]);
                if (!isMaterial)
                  ++num7;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_ResourceDemands[resourceIndex] > 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IndustrialBuildingDemands[resourceIndex] = isMaterial || this.m_IndustrialCompanyDemands[resourceIndex] <= 0 ? (this.m_IndustrialCompanyDemands[resourceIndex] <= 0 ? 0 : 1) : (this.m_FreeProperties[resourceIndex] - this.m_Propertyless[resourceIndex] > 0 ? 0 : 50);
                // ISSUE: reference to a compiler-generated field
                if (this.m_IndustrialBuildingDemands[resourceIndex] > 0)
                {
                  if (flag3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OfficeBuildingDemand.value += this.m_IndustrialBuildingDemands[resourceIndex] > 0 ? this.m_IndustrialCompanyDemands[resourceIndex] : 0;
                  }
                  else if (!isMaterial)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IndustrialBuildingDemand.value += this.m_IndustrialBuildingDemands[resourceIndex] > 0 ? this.m_IndustrialCompanyDemands[resourceIndex] : 0;
                  }
                }
              }
              if (!isMaterial)
              {
                if (flag3)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!flag2 || this.m_IndustrialBuildingDemands[resourceIndex] > 0 && this.m_IndustrialCompanyDemands[resourceIndex] > 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_OfficeDemandFactors[2] = num12;
                    // ISSUE: reference to a compiler-generated field
                    this.m_OfficeDemandFactors[4] += (int) num14;
                    // ISSUE: reference to a compiler-generated field
                    this.m_OfficeDemandFactors[11] += (int) num10;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OfficeDemandFactors[13] += this.m_IndustrialBuildingDemands[resourceIndex];
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_IndustrialDemandFactors[2] = num12;
                  // ISSUE: reference to a compiler-generated field
                  this.m_IndustrialDemandFactors[1] = num13;
                  // ISSUE: reference to a compiler-generated field
                  this.m_IndustrialDemandFactors[4] += (int) num14;
                  // ISSUE: reference to a compiler-generated field
                  this.m_IndustrialDemandFactors[11] += (int) num10;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IndustrialDemandFactors[13] += this.m_IndustrialBuildingDemands[resourceIndex];
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StorageBuildingDemand.value = Mathf.CeilToInt(math.pow(20f * (float) this.m_StorageBuildingDemand.value, 0.75f));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndustrialBuildingDemand.value = flag1 ? 2 * this.m_IndustrialBuildingDemand.value / num7 : 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OfficeCompanyDemand.value *= 2 * this.m_OfficeCompanyDemand.value / num6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IndustrialBuildingDemand.value = math.clamp(this.m_IndustrialBuildingDemand.value, 0, 100);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OfficeBuildingDemand.value = math.clamp(this.m_OfficeBuildingDemand.value, 0, 100);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CityServiceUpkeep> __Game_City_CityServiceUpkeep_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> __Game_Prefabs_BuildingPropertyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityServiceUpkeep_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CityServiceUpkeep>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingPropertyData_RO_ComponentLookup = state.GetComponentLookup<BuildingPropertyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
      }
    }
  }
}
