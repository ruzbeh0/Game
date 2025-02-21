// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CountCompanyDataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Debug;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
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
  public class CountCompanyDataSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private ResourceSystem m_ResourceSystem;
    private NativeQueue<CountCompanyDataSystem.CompanyDataItem> m_DataQueue;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_DemandParameterQuery;
    private EntityQuery m_FreeIndustrialQuery;
    private EntityQuery m_IndustrialCompanyQuery;
    private EntityQuery m_StorageCompanyQuery;
    private EntityQuery m_ProcessDataQuery;
    private EntityQuery m_CityServiceQuery;
    private EntityQuery m_SpawnableQuery;
    [DebugWatchDeps]
    private JobHandle m_WriteDependencies;
    private JobHandle m_ReadDependencies;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_CurrentProductionWorkers;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_MaxProductionWorkers;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_CurrentServiceWorkers;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_MaxServiceWorkers;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_Production;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_SalesCapacities;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_CurrentAvailables;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_TotalAvailables;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_Demand;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ProductionCompanies;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ServiceCompanies;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ProductionPropertyless;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ServicePropertyless;
    private EntityQuery m_CompanyQuery;
    private CountCompanyDataSystem.TypeHandle __TypeHandle;

    public CountCompanyDataSystem.CommercialCompanyDatas GetCommercialCompanyDatas(
      out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new CountCompanyDataSystem.CommercialCompanyDatas()
      {
        m_CurrentAvailables = this.m_CurrentAvailables,
        m_ProduceCapacity = this.m_SalesCapacities,
        m_ServiceCompanies = this.m_ServiceCompanies,
        m_ServicePropertyless = this.m_ServicePropertyless,
        m_TotalAvailables = this.m_TotalAvailables,
        m_CurrentServiceWorkers = this.m_CurrentServiceWorkers,
        m_MaxServiceWorkers = this.m_MaxServiceWorkers
      };
    }

    public CountCompanyDataSystem.IndustrialCompanyDatas GetIndustrialCompanyDatas(
      out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new CountCompanyDataSystem.IndustrialCompanyDatas()
      {
        m_Demand = this.m_Demand,
        m_Production = this.m_Production,
        m_ProductionCompanies = this.m_ProductionCompanies,
        m_ProductionPropertyless = this.m_ProductionPropertyless,
        m_CurrentProductionWorkers = this.m_CurrentProductionWorkers,
        m_MaxProductionWorkers = this.m_MaxProductionWorkers
      };
    }

    public NativeArray<int> GetProduction(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_Production;
    }

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 1;

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
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<Resources>(), ComponentType.Exclude<Game.Companies.StorageCompany>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>(), ComponentType.Exclude<ServiceCompanyData>());
      int resourceCount = EconomyUtils.ResourceCount;
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentProductionWorkers = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MaxProductionWorkers = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentServiceWorkers = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MaxServiceWorkers = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Production = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SalesCapacities = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentAvailables = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TotalAvailables = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Demand = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanies = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceCompanies = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionPropertyless = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ServicePropertyless = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DataQueue = new NativeQueue<CountCompanyDataSystem.CompanyDataItem>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyQuery);
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
      this.m_CurrentProductionWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MaxProductionWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentServiceWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MaxServiceWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Production.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SalesCapacities.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentAvailables.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalAvailables.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Demand.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanies.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceCompanies.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionPropertyless.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ServicePropertyless.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DataQueue.Dispose();
      base.OnDestroy();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentProductionWorkers.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_MaxProductionWorkers.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentServiceWorkers.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_MaxServiceWorkers.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_Production.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_SalesCapacities.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentAvailables.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_TotalAvailables.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_Demand.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionCompanies.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceCompanies.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionPropertyless.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_ServicePropertyless.Fill<int>(0);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_CurrentProductionWorkers);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_MaxProductionWorkers);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_CurrentServiceWorkers);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_MaxServiceWorkers);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Production);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_SalesCapacities);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_CurrentAvailables);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_TotalAvailables);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Demand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ProductionCompanies);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ServiceCompanies);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ProductionPropertyless);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ServicePropertyless);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      NativeArray<int> src = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_CurrentProductionWorkers);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_MaxProductionWorkers);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_CurrentServiceWorkers);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_MaxServiceWorkers);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_Production);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_SalesCapacities);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_CurrentAvailables);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_TotalAvailables);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_Demand);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_ProductionCompanies);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_ServiceCompanies);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_ProductionPropertyless);
      reader.Read(src);
      // ISSUE: reference to a compiler-generated field
      CollectionUtils.CopySafe<int>(src, this.m_ServicePropertyless);
      src.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new CountCompanyDataSystem.CountCompanyDataJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_ServiceAvailableType = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_SpawnableBuildings = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_Employees = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_ResourcesBuf = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_BuildingEfficiencyBuf = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_ServiceCompanyDatas = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_DataQueue = this.m_DataQueue.AsParallelWriter()
      }.ScheduleParallel<CountCompanyDataSystem.CountCompanyDataJob>(this.m_CompanyQuery, JobHandle.CombineDependencies(this.Dependency, this.m_WriteDependencies, this.m_ReadDependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      CountCompanyDataSystem.SumJob jobData = new CountCompanyDataSystem.SumJob()
      {
        m_Demand = this.m_Demand,
        m_Production = this.m_Production,
        m_CurrentAvailables = this.m_CurrentAvailables,
        m_ProductionCompanies = this.m_ProductionCompanies,
        m_ProductionPropertyless = this.m_ProductionPropertyless,
        m_SalesCapacities = this.m_SalesCapacities,
        m_ServiceCompanies = this.m_ServiceCompanies,
        m_ServicePropertyless = this.m_ServicePropertyless,
        m_TotalAvailables = this.m_TotalAvailables,
        m_CurrentProductionWorkers = this.m_CurrentProductionWorkers,
        m_CurrentServiceWorkers = this.m_CurrentServiceWorkers,
        m_MaxProductionWorkers = this.m_MaxProductionWorkers,
        m_MaxServiceWorkers = this.m_MaxServiceWorkers,
        m_DataQueue = this.m_DataQueue
      };
      this.Dependency = jobData.Schedule<CountCompanyDataSystem.SumJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
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
    public CountCompanyDataSystem()
    {
    }

    public struct CommercialCompanyDatas
    {
      public NativeArray<int> m_CurrentServiceWorkers;
      public NativeArray<int> m_MaxServiceWorkers;
      public NativeArray<int> m_ProduceCapacity;
      public NativeArray<int> m_CurrentAvailables;
      public NativeArray<int> m_TotalAvailables;
      public NativeArray<int> m_ServiceCompanies;
      public NativeArray<int> m_ServicePropertyless;
    }

    public struct IndustrialCompanyDatas
    {
      public NativeArray<int> m_CurrentProductionWorkers;
      public NativeArray<int> m_MaxProductionWorkers;
      public NativeArray<int> m_Production;
      public NativeArray<int> m_Demand;
      public NativeArray<int> m_ProductionCompanies;
      public NativeArray<int> m_ProductionPropertyless;
    }

    private struct CompanyDataItem
    {
      public int m_Resource;
      public int m_CurrentProductionWorkers;
      public int m_MaxProductionWorkers;
      public int m_CurrentServiceWorkers;
      public int m_MaxServiceWorkers;
      public int m_Production;
      public int m_SalesCapacities;
      public int m_CurrentAvailables;
      public int m_TotalAvailables;
      public int m_Demand;
      public int m_ProductionCompanies;
      public int m_ServiceCompanies;
      public int m_ProductionPropertyless;
      public int m_ServicePropertyless;
    }

    [BurstCompile]
    private struct SumJob : IJob
    {
      public NativeArray<int> m_CurrentProductionWorkers;
      public NativeArray<int> m_MaxProductionWorkers;
      public NativeArray<int> m_CurrentServiceWorkers;
      public NativeArray<int> m_MaxServiceWorkers;
      public NativeArray<int> m_Production;
      public NativeArray<int> m_SalesCapacities;
      public NativeArray<int> m_CurrentAvailables;
      public NativeArray<int> m_TotalAvailables;
      public NativeArray<int> m_Demand;
      public NativeArray<int> m_ProductionCompanies;
      public NativeArray<int> m_ServiceCompanies;
      public NativeArray<int> m_ProductionPropertyless;
      public NativeArray<int> m_ServicePropertyless;
      public NativeQueue<CountCompanyDataSystem.CompanyDataItem> m_DataQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentProductionWorkers.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_MaxProductionWorkers.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentServiceWorkers.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_MaxServiceWorkers.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_Production.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_SalesCapacities.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentAvailables.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_TotalAvailables.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_Demand.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ProductionCompanies.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ServiceCompanies.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ProductionPropertyless.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ServicePropertyless.Fill<int>(0);
        // ISSUE: variable of a compiler-generated type
        CountCompanyDataSystem.CompanyDataItem companyDataItem;
        // ISSUE: reference to a compiler-generated field
        while (this.m_DataQueue.TryDequeue(out companyDataItem))
        {
          // ISSUE: reference to a compiler-generated field
          int resource = companyDataItem.m_Resource;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentProductionWorkers[resource] += companyDataItem.m_CurrentProductionWorkers;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_MaxProductionWorkers[resource] += companyDataItem.m_MaxProductionWorkers;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentServiceWorkers[resource] += companyDataItem.m_CurrentServiceWorkers;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_MaxServiceWorkers[resource] += companyDataItem.m_MaxServiceWorkers;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Production[resource] += companyDataItem.m_Production;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SalesCapacities[resource] += companyDataItem.m_SalesCapacities;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentAvailables[resource] += companyDataItem.m_CurrentAvailables;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_TotalAvailables[resource] += companyDataItem.m_TotalAvailables;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Demand[resource] += companyDataItem.m_Demand;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ProductionCompanies[resource] += companyDataItem.m_ProductionCompanies;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ServiceCompanies[resource] += companyDataItem.m_ServiceCompanies;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ProductionPropertyless[resource] += companyDataItem.m_ProductionPropertyless;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ServicePropertyless[resource] += companyDataItem.m_ServicePropertyless;
        }
      }
    }

    [BurstCompile]
    private struct CountCompanyDataJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildings;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public BufferLookup<Employee> m_Employees;
      [ReadOnly]
      public BufferLookup<Resources> m_ResourcesBuf;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyBuf;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDatas;
      public EconomyParameterData m_EconomyParameters;
      public NativeQueue<CountCompanyDataSystem.CompanyDataItem>.ParallelWriter m_DataQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceAvailable> nativeArray4 = chunk.GetNativeArray<ServiceAvailable>(ref this.m_ServiceAvailableType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<ServiceAvailable>(ref this.m_ServiceAvailableType);
        int resourceCount = EconomyUtils.ResourceCount;
        NativeArray<int> nativeArray5 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray6 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray7 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray8 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray9 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray10 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray11 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray12 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray13 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray14 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray15 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray16 = new NativeArray<int>(resourceCount, Allocator.Temp);
        NativeArray<int> nativeArray17 = new NativeArray<int>(resourceCount, Allocator.Temp);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          WorkProvider workProvider = nativeArray2[index];
          Entity prefab = nativeArray3[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_IndustrialProcessDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab];
            Resource resource = industrialProcessData.m_Output.m_Resource;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PropertyRenters.HasComponent(entity))
            {
              if (resource != Resource.NoResource)
              {
                int resourceIndex = EconomyUtils.GetResourceIndex(resource);
                // ISSUE: reference to a compiler-generated field
                Entity property = this.m_PropertyRenters[entity].m_Property;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Prefabs.HasComponent(property))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  SpawnableBuildingData spawnableBuilding = this.m_SpawnableBuildings[this.m_Prefabs[property].m_Prefab];
                  // ISSUE: reference to a compiler-generated field
                  WorkplaceData workplaceData = this.m_WorkplaceDatas[prefab];
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Employee> employee = this.m_Employees[entity];
                  int maxWorkers = workProvider.m_MaxWorkers;
                  if (flag)
                  {
                    // ISSUE: reference to a compiler-generated field
                    ServiceCompanyData serviceCompanyData = this.m_ServiceCompanyDatas[prefab];
                    nativeArray11[resourceIndex] += math.clamp(nativeArray4[index].m_ServiceAvailable, 0, serviceCompanyData.m_MaxService);
                    nativeArray12[resourceIndex] += serviceCompanyData.m_MaxService;
                  }
                  float buildingEfficiency = 1f;
                  DynamicBuffer<Efficiency> bufferData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_BuildingEfficiencyBuf.TryGetBuffer(property, out bufferData))
                    buildingEfficiency = BuildingUtils.GetEfficiency(bufferData);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int productionPerDay = EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, !flag, employee, industrialProcessData, this.m_ResourcePrefabs, this.m_ResourceDatas, this.m_Citizens, ref this.m_EconomyParameters);
                  if (industrialProcessData.m_Input1.m_Resource != Resource.NoResource)
                  {
                    // ISSUE: reference to a compiler-generated method
                    nativeArray13[EconomyUtils.GetResourceIndex(industrialProcessData.m_Input1.m_Resource)] += this.GetRoundedConsumption(industrialProcessData.m_Input1.m_Amount, industrialProcessData.m_Output.m_Amount, productionPerDay);
                  }
                  if (industrialProcessData.m_Input2.m_Resource != Resource.NoResource)
                  {
                    // ISSUE: reference to a compiler-generated method
                    nativeArray13[EconomyUtils.GetResourceIndex(industrialProcessData.m_Input2.m_Resource)] += this.GetRoundedConsumption(industrialProcessData.m_Input2.m_Amount, industrialProcessData.m_Output.m_Amount, productionPerDay);
                  }
                  if (flag)
                  {
                    nativeArray8[resourceIndex] += maxWorkers;
                    nativeArray7[resourceIndex] += employee.Length;
                    nativeArray10[resourceIndex] += productionPerDay;
                  }
                  else
                  {
                    nativeArray6[resourceIndex] += maxWorkers;
                    nativeArray5[resourceIndex] += employee.Length;
                    nativeArray9[resourceIndex] += productionPerDay;
                  }
                }
                else
                  continue;
              }
            }
            else if (resource != Resource.NoResource)
            {
              if (flag)
                nativeArray17[EconomyUtils.GetResourceIndex(resource)]++;
              else
                nativeArray16[EconomyUtils.GetResourceIndex(resource)]++;
            }
            if (resource != Resource.NoResource)
            {
              if (flag)
                nativeArray15[EconomyUtils.GetResourceIndex(resource)]++;
              else
                nativeArray14[EconomyUtils.GetResourceIndex(resource)]++;
            }
          }
        }
        for (int index = 0; index < nativeArray9.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CountCompanyDataSystem.CompanyDataItem companyDataItem = new CountCompanyDataSystem.CompanyDataItem()
          {
            m_Resource = index,
            m_Demand = nativeArray13[index],
            m_Production = nativeArray9[index],
            m_CurrentAvailables = nativeArray11[index],
            m_ProductionCompanies = nativeArray14[index],
            m_ProductionPropertyless = nativeArray16[index],
            m_SalesCapacities = nativeArray10[index],
            m_ServiceCompanies = nativeArray15[index],
            m_ServicePropertyless = nativeArray17[index],
            m_TotalAvailables = nativeArray12[index],
            m_CurrentProductionWorkers = nativeArray5[index],
            m_CurrentServiceWorkers = nativeArray7[index],
            m_MaxProductionWorkers = nativeArray6[index],
            m_MaxServiceWorkers = nativeArray8[index]
          };
          // ISSUE: reference to a compiler-generated field
          this.m_DataQueue.Enqueue(companyDataItem);
        }
        nativeArray5.Dispose();
        nativeArray6.Dispose();
        nativeArray7.Dispose();
        nativeArray8.Dispose();
        nativeArray9.Dispose();
        nativeArray10.Dispose();
        nativeArray11.Dispose();
        nativeArray12.Dispose();
        nativeArray13.Dispose();
        nativeArray14.Dispose();
        nativeArray15.Dispose();
        nativeArray16.Dispose();
        nativeArray17.Dispose();
      }

      private int GetRoundedConsumption(int inputAmount, int outputAmount, int production)
      {
        return (int) (((long) inputAmount * (long) production + (long) (outputAmount >> 1)) / (long) outputAmount);
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
      }
    }
  }
}
