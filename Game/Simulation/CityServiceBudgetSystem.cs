// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CityServiceBudgetSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CityServiceBudgetSystem : GameSystemBase, ICityServiceBudgetSystem
  {
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ResourceSystem m_ResourceSystem;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private TaxSystem m_TaxSystem;
    private MapTilePurchaseSystem m_MapTilePurchaseSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_BudgetDataQuery;
    private EntityQuery m_ServiceBuildingQuery;
    private EntityQuery m_ServiceQuery;
    private EntityQuery m_UpkeepGroup;
    private EntityQuery m_ServiceObjectQuery;
    private EntityQuery m_NetUpkeepQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_OutsideTradeParameterQuery;
    private EntityQuery m_HealthcareFacilityQuery;
    private EntityQuery m_DeathcareFacilityQuery;
    private EntityQuery m_GarbageFacilityQuery;
    private EntityQuery m_FireStationQuery;
    private EntityQuery m_PoliceStationQuery;
    protected NativeArray<int> m_Income;
    protected NativeArray<int> m_IncomeTemp;
    protected NativeArray<int> m_TotalIncome;
    protected NativeArray<int> m_Expenses;
    protected NativeArray<int> m_ExpensesTemp;
    private int m_TotalTaxIncome;
    private NativeReference<int> m_TotalTaxes;
    private NativeParallelHashMap<Entity, CollectedCityServiceBudgetData> m_CityServiceBudgets;
    private NativeParallelHashMap<Entity, int2> m_CityServiceUpkeepIndices;
    private NativeList<CollectedCityServiceUpkeepData> m_CityServiceUpkeeps;
    private JobHandle m_TempArrayDeps;
    private CityServiceBudgetSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_844909882_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilePurchaseSystem = this.World.GetOrCreateSystemManaged<MapTilePurchaseSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalIncome = new NativeArray<int>(14, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Income = new NativeArray<int>(14, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IncomeTemp = new NativeArray<int>(14, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Expenses = new NativeArray<int>(15, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ExpensesTemp = new NativeArray<int>(15, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceUpkeepIndices = new NativeParallelHashMap<Entity, int2>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceUpkeeps = new NativeList<CollectedCityServiceUpkeepData>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgets = new NativeParallelHashMap<Entity, CollectedCityServiceBudgetData>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TotalTaxes = new NativeReference<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceQuery = this.GetEntityQuery(ComponentType.ReadWrite<CollectedCityServiceBudgetData>(), ComponentType.ReadWrite<CollectedCityServiceUpkeepData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceObjectQuery = this.GetEntityQuery(ComponentType.ReadWrite<CollectedServiceBuildingBudgetData>(), ComponentType.ReadOnly<ServiceObjectData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepGroup = this.GetEntityQuery(ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_BudgetDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceBudgetData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpkeepQuery = this.GetEntityQuery(ComponentType.ReadOnly<Composition>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Owner>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareFacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Hospital>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeathcareFacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageFacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_FireStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.FireStation>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.PoliceStation>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideTradeParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<OutsideTradeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BudgetDataQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ServiceQuery);
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_BudgetDataQuery.CalculateEntityCount() != 0)
        return;
      this.EntityManager.CreateEntity(ComponentType.ReadWrite<ServiceBudgetData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Income.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_IncomeTemp.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalIncome.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Expenses.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ExpensesTemp.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceUpkeeps.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgets.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceUpkeepIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalTaxes.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TotalTaxIncome = this.m_TotalTaxes.Value;
      // ISSUE: reference to a compiler-generated field
      this.m_TempArrayDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_IncomeTemp.CopyTo(this.m_Income);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ExpensesTemp.CopyTo(this.m_Expenses);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<Entity> entityListAsync = this.m_ServiceQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_City_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Creditworthiness_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Loan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityServiceBudgetSystem.UpdateDataJob jobData1 = new CityServiceBudgetSystem.UpdateDataJob()
      {
        m_CityServiceEntities = entityListAsync,
        m_City = this.m_CitySystem.City,
        m_Lookup = this.m_CityStatisticsSystem.GetLookup(),
        m_Stats = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup,
        m_CollectedFees = this.m_ServiceFeeSystem.GetServiceFees(),
        m_BudgetDatas = this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RO_BufferLookup,
        m_Loans = this.__TypeHandle.__Game_Simulation_Loan_RO_ComponentLookup,
        m_BudgetEntity = this.m_BudgetDataQuery.GetSingletonEntity(),
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_CityServiceBudgets = this.m_CityServiceBudgets,
        m_CityServiceUpkeepIndices = this.m_CityServiceUpkeepIndices,
        m_CityServiceUpkeeps = this.m_CityServiceUpkeeps,
        m_CollectedBudgets = this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentLookup,
        m_CollectedUpkeeps = this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferLookup,
        m_Creditworthiness = this.__TypeHandle.__Game_Simulation_Creditworthiness_RO_ComponentLookup,
        m_Expenses = this.m_ExpensesTemp,
        m_ServiceFees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_Income = this.m_IncomeTemp,
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_Populations = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_CityData = this.__TypeHandle.__Game_City_City_RO_ComponentLookup,
        m_OutsideTradeParameterData = this.m_OutsideTradeParameterQuery.GetSingleton<OutsideTradeParameterData>(),
        m_TotalTaxes = this.m_TotalTaxes,
        m_ServiceFacilityBuildingCount = new int4(this.m_HealthcareFacilityQuery.CalculateEntityCount(), this.m_DeathcareFacilityQuery.CalculateEntityCount(), this.m_GarbageFacilityQuery.CalculateEntityCount(), this.m_FireStationQuery.CalculateEntityCount()),
        m_PoliceStationBuildingCount = this.m_PoliceStationQuery.CalculateEntityCount(),
        m_MapTileUpkeepCost = this.m_CityConfigurationSystem.unlockMapTiles ? 0 : this.m_MapTilePurchaseSystem.CalculateOwnedTilesUpkeep()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.Schedule<CityServiceBudgetSystem.UpdateDataJob>(JobHandle.CombineDependencies(outJobHandle, this.m_TempArrayDeps, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_TempArrayDeps = this.Dependency;
      entityListAsync.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityServiceBudgetSystem.ClearServiceDataJob jobData2 = new CityServiceBudgetSystem.ClearServiceDataJob()
      {
        m_BudgetDataType = this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentTypeHandle,
        m_UpkeepDataType = this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData2.ScheduleParallel<CityServiceBudgetSystem.ClearServiceDataJob>(this.m_ServiceQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUsage_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityServiceBudgetSystem.CityServiceBudgetJob jobData3 = new CityServiceBudgetSystem.CityServiceBudgetJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ServiceObjectDatas = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_ServiceUpkeepDatas = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_ServiceUsages = this.__TypeHandle.__Game_Buildings_ServiceUsage_RO_ComponentLookup,
        m_InstalledUpgradeBufs = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_EmployeeBufs = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup,
        m_BudgetDatas = this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup,
        m_UpkeepDatas = this.__TypeHandle.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup,
        m_ServiceBudgets = this.m_BudgetDataQuery.GetSingletonBuffer<ServiceBudgetData>(true),
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData3.Schedule<CityServiceBudgetSystem.CityServiceBudgetJob>(this.m_UpkeepGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new CityServiceBudgetSystem.ClearBuildingDataJob()
      {
        m_BudgetType = this.__TypeHandle.__Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentTypeHandle
      }.ScheduleParallel<CityServiceBudgetSystem.ClearBuildingDataJob>(this.m_ServiceObjectQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle job0 = new CityServiceBudgetSystem.CollectServiceBuildingBudgetDatasJob()
      {
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_Budgets = this.__TypeHandle.__Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentLookup
      }.Schedule<CityServiceBudgetSystem.CollectServiceBuildingBudgetDatasJob>(this.m_ServiceBuildingQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle job1 = new CityServiceBudgetSystem.NetServiceBudgetJob()
      {
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ServiceObjects = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_PlaceableNetCompositionData = this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup,
        m_BudgetDatas = this.__TypeHandle.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup
      }.Schedule<CityServiceBudgetSystem.NetServiceBudgetJob>(this.m_NetUpkeepQuery, this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(job0, job1);
    }

    public NativeArray<int> GetIncomeArray(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_TempArrayDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_IncomeTemp;
    }

    public NativeArray<int> GetExpenseArray(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_TempArrayDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_ExpensesTemp;
    }

    public void AddArrayReader(JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TempArrayDeps = JobHandle.CombineDependencies(this.m_TempArrayDeps, deps);
    }

    public int GetBalance() => CityServiceBudgetSystem.GetBalance(this.m_Income, this.m_Expenses);

    public static int GetBalance(NativeArray<int> income, NativeArray<int> expenses)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return CityServiceBudgetSystem.GetTotalIncome(income) + CityServiceBudgetSystem.GetTotalExpenses(expenses);
    }

    public int GetTotalIncome() => CityServiceBudgetSystem.GetTotalIncome(this.m_Income);

    public static int GetTotalIncome(NativeArray<int> income)
    {
      int totalIncome = 0;
      for (int index = 0; index < income.Length; ++index)
        totalIncome += income[index];
      return totalIncome;
    }

    public int GetTotalExpenses() => CityServiceBudgetSystem.GetTotalExpenses(this.m_Expenses);

    public static int GetTotalExpenses(NativeArray<int> expenses)
    {
      int totalExpenses = 0;
      for (int index = 0; index < expenses.Length; ++index)
        totalExpenses -= expenses[index];
      return totalExpenses;
    }

    public int GetTotalTaxIncome() => this.m_TotalTaxIncome;

    public int GetIncome(IncomeSource source)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return CityServiceBudgetSystem.GetIncome(source, this.m_Income);
    }

    public static int GetIncome(IncomeSource source, NativeArray<int> income)
    {
      return source < (IncomeSource) income.Length ? income[(int) source] : 0;
    }

    public int GetTotalIncome(IncomeSource source)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return source < (IncomeSource) this.m_TotalIncome.Length ? this.m_TotalIncome[(int) source] : 0;
    }

    public int GetExpense(ExpenseSource source)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return CityServiceBudgetSystem.GetExpense(source, this.m_Expenses);
    }

    public static int GetExpense(ExpenseSource source, NativeArray<int> expenses)
    {
      return source < (ExpenseSource) expenses.Length ? expenses[(int) source] : 0;
    }

    public int GetServiceBudget(Entity servicePrefab)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TempArrayDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return CityServiceBudgetSystem.GetServiceBudget(servicePrefab, this.m_CityServiceBudgets, this.m_BudgetDataQuery.GetSingletonEntity(), this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RO_BufferLookup);
    }

    public static int GetServiceBudget(
      Entity servicePrefab,
      NativeParallelHashMap<Entity, CollectedCityServiceBudgetData> budgets,
      Entity budgetEntity,
      BufferLookup<ServiceBudgetData> budgetDatas)
    {
      if (!budgets.ContainsKey(servicePrefab))
        return 0;
      DynamicBuffer<ServiceBudgetData> budgetData = budgetDatas[budgetEntity];
      for (int index = 0; index < budgetData.Length; ++index)
      {
        ServiceBudgetData serviceBudgetData = budgetData[index];
        if (serviceBudgetData.m_Service == servicePrefab)
          return serviceBudgetData.m_Budget;
      }
      return 100;
    }

    public void SetServiceBudget(Entity servicePrefab, int percentage)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TempArrayDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityServiceBudgets.ContainsKey(servicePrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      Entity singletonEntity = this.m_BudgetDataQuery.GetSingletonEntity();
      DynamicBuffer<ServiceBudgetData> buffer = this.EntityManager.GetBuffer<ServiceBudgetData>(singletonEntity);
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < buffer.Length; ++index)
      {
        ServiceBudgetData serviceBudgetData = buffer[index];
        if (serviceBudgetData.m_Service == servicePrefab)
        {
          flag1 = serviceBudgetData.m_Budget != percentage;
          serviceBudgetData.m_Budget = percentage;
          buffer[index] = serviceBudgetData;
          flag2 = true;
          break;
        }
      }
      if (!flag2)
      {
        flag1 = true;
        buffer.Add(new ServiceBudgetData()
        {
          m_Service = servicePrefab,
          m_Budget = percentage
        });
      }
      if (!flag1)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Updated>(singletonEntity);
    }

    public int GetServiceEfficiency(Entity servicePrefab, int budget)
    {
      // ISSUE: reference to a compiler-generated field
      return Mathf.RoundToInt(100f * this.__query_844909882_0.GetSingleton<BuildingEfficiencyParameterData>().m_ServiceBudgetEfficiencyFactor.Evaluate((float) budget / 100f));
    }

    private static int GetEstimatedServiceUpkeep(
      CollectedCityServiceBudgetData data,
      int2 indices,
      int budget,
      NativeList<CollectedCityServiceUpkeepData> upkeeps)
    {
      int baseCost = data.m_BaseCost;
      for (int x = indices.x; x < indices.x + indices.y; ++x)
      {
        CollectedCityServiceUpkeepData upkeep = upkeeps[x];
        int num = Mathf.RoundToInt((float) upkeep.m_FullCost);
        if (upkeep.m_Resource == Resource.Money)
          num = Mathf.RoundToInt((float) num * ((float) budget / 100f));
        baseCost += num;
      }
      return baseCost;
    }

    public void GetEstimatedServiceBudget(Entity servicePrefab, out int upkeep)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TempArrayDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      CityServiceBudgetSystem.GetEstimatedServiceBudget(servicePrefab, out upkeep, this.m_CityServiceBudgets, this.m_CityServiceUpkeeps, this.m_CityServiceUpkeepIndices, this.m_BudgetDataQuery.GetSingletonEntity(), this.GetBufferLookup<ServiceBudgetData>(true));
    }

    public static void GetEstimatedServiceBudget(
      Entity servicePrefab,
      out int upkeep,
      NativeParallelHashMap<Entity, CollectedCityServiceBudgetData> budgets,
      NativeList<CollectedCityServiceUpkeepData> upkeeps,
      NativeParallelHashMap<Entity, int2> upkeepIndices,
      Entity budgetEntity,
      BufferLookup<ServiceBudgetData> budgetDatas)
    {
      if (!budgets.ContainsKey(servicePrefab))
      {
        upkeep = 0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        int serviceBudget = CityServiceBudgetSystem.GetServiceBudget(servicePrefab, budgets, budgetEntity, budgetDatas);
        CollectedCityServiceBudgetData budget = budgets[servicePrefab];
        int2 upkeepIndex = upkeepIndices[servicePrefab];
        // ISSUE: reference to a compiler-generated method
        upkeep = CityServiceBudgetSystem.GetEstimatedServiceUpkeep(budget, upkeepIndex, serviceBudget, upkeeps);
      }
    }

    public int GetNumberOfServiceBuildings(Entity serviceBuildingPrefab)
    {
      return this.EntityManager.HasComponent<CollectedServiceBuildingBudgetData>(serviceBuildingPrefab) ? this.EntityManager.GetComponentData<CollectedServiceBuildingBudgetData>(serviceBuildingPrefab).m_Count : 0;
    }

    public int2 GetWorkersAndWorkplaces(Entity serviceBuildingPrefab)
    {
      if (!this.EntityManager.HasComponent<CollectedServiceBuildingBudgetData>(serviceBuildingPrefab))
        return new int2(0, 0);
      CollectedServiceBuildingBudgetData componentData = this.EntityManager.GetComponentData<CollectedServiceBuildingBudgetData>(serviceBuildingPrefab);
      return new int2(componentData.m_Workers, componentData.m_Workplaces);
    }

    public Entity[] GetServiceBuildings(Entity servicePrefab)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ServiceObjectQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<ServiceObjectData> componentDataArray = this.m_ServiceObjectQuery.ToComponentDataArray<ServiceObjectData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      List<Entity> entityList = new List<Entity>(4);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        if (componentDataArray[index].m_Service == servicePrefab)
          entityList.Add(entityArray[index]);
      }
      entityArray.Dispose();
      componentDataArray.Dispose();
      return entityList.ToArray();
    }

    private static int GetTotalTaxIncome(NativeArray<int> income)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return CityServiceBudgetSystem.GetIncome(IncomeSource.TaxCommercial, income) + CityServiceBudgetSystem.GetIncome(IncomeSource.TaxIndustrial, income) + CityServiceBudgetSystem.GetIncome(IncomeSource.TaxResidential, income) + CityServiceBudgetSystem.GetIncome(IncomeSource.TaxOffice, income);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_844909882_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
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
    public CityServiceBudgetSystem()
    {
    }

    private struct UpdateDataJob : IJob
    {
      [ReadOnly]
      public BufferLookup<CityStatistic> m_Stats;
      [ReadOnly]
      public NativeList<Entity> m_CityServiceEntities;
      public Entity m_City;
      [ReadOnly]
      public NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> m_Lookup;
      public NativeList<CollectedCityServiceUpkeepData> m_CityServiceUpkeeps;
      public NativeParallelHashMap<Entity, CollectedCityServiceBudgetData> m_CityServiceBudgets;
      [ReadOnly]
      public ComponentLookup<CollectedCityServiceBudgetData> m_CollectedBudgets;
      [ReadOnly]
      public ComponentLookup<Creditworthiness> m_Creditworthiness;
      [ReadOnly]
      public ComponentLookup<Population> m_Populations;
      [ReadOnly]
      public ComponentLookup<Game.City.City> m_CityData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<CollectedCityServiceUpkeepData> m_CollectedUpkeeps;
      public NativeParallelHashMap<Entity, int2> m_CityServiceUpkeepIndices;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public NativeArray<int> m_Income;
      public NativeArray<int> m_Expenses;
      public Entity m_BudgetEntity;
      [ReadOnly]
      public BufferLookup<ServiceBudgetData> m_BudgetDatas;
      [ReadOnly]
      public NativeList<CollectedCityServiceFeeData> m_CollectedFees;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_ServiceFees;
      [ReadOnly]
      public ComponentLookup<Loan> m_Loans;
      [ReadOnly]
      public OutsideTradeParameterData m_OutsideTradeParameterData;
      [ReadOnly]
      public int4 m_ServiceFacilityBuildingCount;
      [ReadOnly]
      public int m_PoliceStationBuildingCount;
      [ReadOnly]
      public int m_MapTileUpkeepCost;
      public NativeReference<int> m_TotalTaxes;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ServiceFees.HasBuffer(this.m_City))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceFee> serviceFee = this.m_ServiceFees[this.m_City];
        // ISSUE: reference to a compiler-generated field
        this.m_CityServiceUpkeeps.Clear();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_CityServiceEntities.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          Entity cityServiceEntity = this.m_CityServiceEntities[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CityServiceBudgets[cityServiceEntity] = this.m_CollectedBudgets[cityServiceEntity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CollectedUpkeeps.HasBuffer(cityServiceEntity))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CollectedCityServiceUpkeepData> collectedUpkeep = this.m_CollectedUpkeeps[cityServiceEntity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CityServiceUpkeepIndices[cityServiceEntity] = new int2(this.m_CityServiceUpkeeps.Length, collectedUpkeep.Length);
            for (int index2 = 0; index2 < collectedUpkeep.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CityServiceUpkeeps.Add(collectedUpkeep[index2]);
            }
          }
        }
        for (int index3 = 0; index3 < 15; ++index3)
        {
          ExpenseSource expenseSource = (ExpenseSource) index3;
          // ISSUE: reference to a compiler-generated field
          this.m_Expenses[index3] = 0;
          switch (expenseSource)
          {
            case ExpenseSource.SubsidyResidential:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = -TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Residential, TaxResultType.Expense, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case ExpenseSource.LoanInterest:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              LoanInfo loan = LoanSystem.CalculateLoan(this.m_Loans[this.m_City].m_Amount, this.m_Creditworthiness[this.m_City].m_Amount, this.m_CityModifiers[this.m_City]);
              // ISSUE: reference to a compiler-generated field
              this.m_Expenses[index3] = loan.m_DailyPayment;
              break;
            case ExpenseSource.ImportElectricity:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = ServiceFeeSystem.GetServiceFees(PlayerResource.Electricity, this.m_CollectedFees).z;
              break;
            case ExpenseSource.ImportWater:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = ServiceFeeSystem.GetServiceFees(PlayerResource.Water, this.m_CollectedFees).z;
              break;
            case ExpenseSource.ExportSewage:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = ServiceFeeSystem.GetServiceFees(PlayerResource.Sewage, this.m_CollectedFees).z;
              break;
            case ExpenseSource.ServiceUpkeep:
              // ISSUE: reference to a compiler-generated field
              for (int index4 = 0; index4 < this.m_CityServiceEntities.Length; ++index4)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int serviceBudget = CityServiceBudgetSystem.GetServiceBudget(this.m_CityServiceEntities[index4], this.m_CityServiceBudgets, this.m_BudgetEntity, this.m_BudgetDatas);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_Expenses[index3] += CityServiceBudgetSystem.GetEstimatedServiceUpkeep(this.m_CityServiceBudgets[this.m_CityServiceEntities[index4]], this.m_CityServiceUpkeepIndices[this.m_CityServiceEntities[index4]], serviceBudget, this.m_CityServiceUpkeeps);
              }
              break;
            case ExpenseSource.SubsidyCommercial:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = -TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Commercial, TaxResultType.Expense, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case ExpenseSource.SubsidyIndustrial:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = -TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Industrial, TaxResultType.Expense, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case ExpenseSource.SubsidyOffice:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Expenses[index3] = -TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Office, TaxResultType.Expense, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case ExpenseSource.ImportPoliceService:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_Expenses[index3] = -this.GetImportedPoliceServiceFee();
                break;
              }
              break;
            case ExpenseSource.ImportAmbulanceService:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_Expenses[index3] = -this.GetImportedAmbulanceServiceFee();
                break;
              }
              break;
            case ExpenseSource.ImportHearseService:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_Expenses[index3] = -this.GetImportedHearseServiceFee();
                break;
              }
              break;
            case ExpenseSource.ImportFireEngineService:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_Expenses[index3] = -this.GetImportedFireEngineServiceFee();
                break;
              }
              break;
            case ExpenseSource.ImportGarbageService:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_Expenses[index3] = -this.GetImportedGarbageServiceFee();
                break;
              }
              break;
            case ExpenseSource.MapTileUpkeep:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Expenses[index3] = this.m_MapTileUpkeepCost;
              break;
          }
        }
        for (int index = 0; index < 14; ++index)
        {
          IncomeSource incomeSource = (IncomeSource) index;
          // ISSUE: reference to a compiler-generated field
          this.m_Income[index] = 0;
          switch (incomeSource)
          {
            case IncomeSource.TaxResidential:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Residential, TaxResultType.Income, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case IncomeSource.TaxCommercial:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Commercial, TaxResultType.Income, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case IncomeSource.TaxIndustrial:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Industrial, TaxResultType.Income, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case IncomeSource.FeeHealthcare:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.Healthcare, ServiceFeeSystem.GetFee(PlayerResource.Healthcare, serviceFee), this.m_CollectedFees);
              break;
            case IncomeSource.FeeElectricity:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.Electricity, ServiceFeeSystem.GetFee(PlayerResource.Electricity, serviceFee), this.m_CollectedFees);
              break;
            case IncomeSource.FeeEducation:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.BasicEducation, ServiceFeeSystem.GetFee(PlayerResource.BasicEducation, serviceFee), this.m_CollectedFees) + ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.SecondaryEducation, ServiceFeeSystem.GetFee(PlayerResource.SecondaryEducation, serviceFee), this.m_CollectedFees) + ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.HigherEducation, ServiceFeeSystem.GetFee(PlayerResource.HigherEducation, serviceFee), this.m_CollectedFees);
              break;
            case IncomeSource.ExportElectricity:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFees(PlayerResource.Electricity, this.m_CollectedFees).y;
              break;
            case IncomeSource.ExportWater:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFees(PlayerResource.Water, this.m_CollectedFees).y;
              break;
            case IncomeSource.FeeParking:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFees(PlayerResource.Parking, this.m_CollectedFees).x;
              break;
            case IncomeSource.FeePublicTransport:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFees(PlayerResource.PublicTransport, this.m_CollectedFees).x;
              break;
            case IncomeSource.TaxOffice:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = TaxSystem.GetEstimatedTaxAmount(TaxAreaType.Office, TaxResultType.Income, this.m_Lookup, this.m_Stats, this.m_TaxRates);
              break;
            case IncomeSource.FeeGarbage:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.Garbage, ServiceFeeSystem.GetFee(PlayerResource.Garbage, serviceFee), this.m_CollectedFees);
              break;
            case IncomeSource.FeeWater:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_Income[index] = ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.Water, ServiceFeeSystem.GetFee(PlayerResource.Water, serviceFee), this.m_CollectedFees) + ServiceFeeSystem.GetServiceFeeIncomeEstimate(PlayerResource.Sewage, ServiceFeeSystem.GetFee(PlayerResource.Water, serviceFee), this.m_CollectedFees);
              break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TotalTaxes.Value = CityServiceBudgetSystem.GetTotalTaxIncome(this.m_Income);
      }

      private int GetImportedAmbulanceServiceFee()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -(int) ((double) this.m_OutsideTradeParameterData.m_AmbulanceImportServiceFee * (double) (this.m_Populations[this.m_City].m_Population / this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange + 1) * (double) this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange);
      }

      private int GetImportedHearseServiceFee()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -(int) ((double) this.m_OutsideTradeParameterData.m_HearseImportServiceFee * (double) (this.m_Populations[this.m_City].m_Population / this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange + 1) * (double) this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange);
      }

      private int GetImportedGarbageServiceFee()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -(int) ((double) this.m_OutsideTradeParameterData.m_GarbageImportServiceFee * (double) (this.m_Populations[this.m_City].m_Population / this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange + 1) * (double) this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange);
      }

      private int GetImportedFireEngineServiceFee()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -(int) ((double) this.m_OutsideTradeParameterData.m_FireEngineImportServiceFee * (double) (this.m_Populations[this.m_City].m_Population / this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange + 1) * (double) this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange);
      }

      private int GetImportedPoliceServiceFee()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return -(int) ((double) this.m_OutsideTradeParameterData.m_PoliceImportServiceFee * (double) (this.m_Populations[this.m_City].m_Population / this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange + 1) * (double) this.m_OutsideTradeParameterData.m_OCServiceTradePopulationRange);
      }
    }

    [BurstCompile]
    private struct ClearServiceDataJob : IJobChunk
    {
      public ComponentTypeHandle<CollectedCityServiceBudgetData> m_BudgetDataType;
      public BufferTypeHandle<CollectedCityServiceUpkeepData> m_UpkeepDataType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CollectedCityServiceBudgetData> nativeArray = chunk.GetNativeArray<CollectedCityServiceBudgetData>(ref this.m_BudgetDataType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CollectedCityServiceUpkeepData> bufferAccessor = chunk.GetBufferAccessor<CollectedCityServiceUpkeepData>(ref this.m_UpkeepDataType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          nativeArray[index] = new CollectedCityServiceBudgetData();
          bufferAccessor[index].Clear();
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

    [BurstCompile]
    private struct CityServiceBudgetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectDatas;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_ServiceUpkeepDatas;
      [ReadOnly]
      public ComponentLookup<ServiceUsage> m_ServiceUsages;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgradeBufs;
      [ReadOnly]
      public BufferLookup<Employee> m_EmployeeBufs;
      public ComponentLookup<CollectedCityServiceBudgetData> m_BudgetDatas;
      public BufferLookup<CollectedCityServiceUpkeepData> m_UpkeepDatas;
      [ReadOnly]
      public DynamicBuffer<ServiceBudgetData> m_ServiceBudgets;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;

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
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        bool flag = nativeArray3.Length != 0;
        NativeList<ServiceUpkeepData> totalUpkeepDatas = new NativeList<ServiceUpkeepData>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          ServiceObjectData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceObjectDatas.TryGetComponent(prefab, out componentData1))
          {
            Entity service = componentData1.m_Service;
            CollectedCityServiceBudgetData componentData2;
            DynamicBuffer<CollectedCityServiceUpkeepData> bufferData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_BudgetDatas.TryGetComponent(service, out componentData2) && this.m_UpkeepDatas.TryGetBuffer(service, out bufferData))
            {
              ++componentData2.m_Count;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceUpkeepDatas.HasBuffer(prefab))
              {
                // ISSUE: reference to a compiler-generated method
                int serviceBudget = this.GetServiceBudget(service);
                totalUpkeepDatas.Clear();
                bool mainBuildingDisabled = flag && BuildingUtils.CheckOption(nativeArray3[index1], BuildingOption.Inactive);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                CityServiceUpkeepSystem.GetUpkeepWithUsageScale(totalUpkeepDatas, this.m_ServiceUpkeepDatas, this.m_InstalledUpgradeBufs, this.m_Prefabs, this.m_ServiceUsages, entity, prefab, mainBuildingDisabled);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int upkeepOfEmployeeWage = CityServiceUpkeepSystem.GetUpkeepOfEmployeeWage(this.m_EmployeeBufs, entity, this.m_EconomyParameterData, mainBuildingDisabled);
                for (int index2 = 0; index2 < totalUpkeepDatas.Length; ++index2)
                {
                  ServiceUpkeepData serviceUpkeepData = totalUpkeepDatas[index2];
                  int amount = serviceUpkeepData.m_Upkeep.m_Amount;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int num1 = Mathf.RoundToInt((float) amount * EconomyUtils.GetMarketPrice(serviceUpkeepData.m_Upkeep.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas));
                  int num2 = num1;
                  if (serviceUpkeepData.m_Upkeep.m_Resource == Resource.Money)
                  {
                    num1 += upkeepOfEmployeeWage;
                    if (mainBuildingDisabled)
                      num1 = (int) ((double) num1 * 0.10000000149011612);
                    num2 = (int) math.round((float) num1 * ((float) serviceBudget / 100f));
                  }
                  if (amount > 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    ref CollectedCityServiceUpkeepData local = ref this.GetOrCreateUpkeepData(bufferData, serviceUpkeepData.m_Upkeep.m_Resource);
                    local.m_Amount += amount;
                    local.m_Cost += num2;
                    local.m_FullCost += num1;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.m_BudgetDatas[service] = componentData2;
            }
          }
        }
      }

      private int GetServiceBudget(Entity service)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ServiceBudgets.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceBudgets[index].m_Service == service)
          {
            // ISSUE: reference to a compiler-generated field
            return this.m_ServiceBudgets[index].m_Budget;
          }
        }
        return 100;
      }

      private ref CollectedCityServiceUpkeepData GetOrCreateUpkeepData(
        DynamicBuffer<CollectedCityServiceUpkeepData> upkeepDatas,
        Resource resource)
      {
        for (int index = 0; index < upkeepDatas.Length; ++index)
        {
          if (upkeepDatas[index].m_Resource == resource)
            return ref upkeepDatas.ElementAt(index);
        }
        int index1 = upkeepDatas.Add(new CollectedCityServiceUpkeepData()
        {
          m_Resource = resource,
          m_Amount = 0,
          m_Cost = 0,
          m_FullCost = 0
        });
        return ref upkeepDatas.ElementAt(index1);
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
    private struct ClearBuildingDataJob : IJobChunk
    {
      public ComponentTypeHandle<CollectedServiceBuildingBudgetData> m_BudgetType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CollectedServiceBuildingBudgetData> nativeArray = chunk.GetNativeArray<CollectedServiceBuildingBudgetData>(ref this.m_BudgetType);
        for (int index = 0; index < nativeArray.Length; ++index)
          nativeArray[index] = new CollectedServiceBuildingBudgetData();
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
    private struct CollectServiceBuildingBudgetDatasJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      public ComponentLookup<CollectedServiceBuildingBudgetData> m_Budgets;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        if (nativeArray2.Length != 0 && bufferAccessor.Length != 0)
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity prefab = nativeArray1[index].m_Prefab;
            CollectedServiceBuildingBudgetData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Budgets.TryGetComponent(prefab, out componentData))
            {
              ++componentData.m_Count;
              componentData.m_Workers += bufferAccessor[index].Length;
              componentData.m_Workplaces += nativeArray2[index].m_MaxWorkers;
              // ISSUE: reference to a compiler-generated field
              this.m_Budgets[prefab] = componentData;
            }
          }
        }
        else
        {
          for (int index = 0; index < chunk.Count; ++index)
          {
            Entity prefab = nativeArray1[index].m_Prefab;
            CollectedServiceBuildingBudgetData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Budgets.TryGetComponent(prefab, out componentData))
            {
              ++componentData.m_Count;
              // ISSUE: reference to a compiler-generated field
              this.m_Budgets[prefab] = componentData;
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

    [BurstCompile]
    private struct NetServiceBudgetJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjects;
      [ReadOnly]
      public ComponentLookup<PlaceableNetComposition> m_PlaceableNetCompositionData;
      public ComponentLookup<CollectedCityServiceBudgetData> m_BudgetDatas;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Composition> nativeArray2 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity prefab = nativeArray1[index].m_Prefab;
          Composition composition = nativeArray2[index];
          Curve curve = nativeArray3[index];
          ServiceObjectData componentData1;
          PlaceableNetComposition componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceableNetCompositionData.HasComponent(composition.m_Edge) && this.m_ServiceObjects.TryGetComponent(prefab, out componentData1) && this.m_PlaceableNetCompositionData.TryGetComponent(composition.m_Edge, out componentData2))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddUpkeepCost(componentData1.m_Service, NetUtils.GetUpkeepCost(curve, componentData2));
          }
        }
      }

      private void AddUpkeepCost(Entity service, int upkeep)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BudgetDatas.HasComponent(service))
          return;
        // ISSUE: reference to a compiler-generated field
        CollectedCityServiceBudgetData budgetData = this.m_BudgetDatas[service];
        budgetData.m_BaseCost += upkeep;
        // ISSUE: reference to a compiler-generated field
        this.m_BudgetDatas[service] = budgetData;
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
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceBudgetData> __Game_Simulation_ServiceBudgetData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Loan> __Game_Simulation_Loan_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<CollectedCityServiceBudgetData> __Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CollectedCityServiceUpkeepData> __Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Creditworthiness> __Game_Simulation_Creditworthiness_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.City.City> __Game_City_City_RO_ComponentLookup;
      public ComponentTypeHandle<CollectedCityServiceBudgetData> __Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentTypeHandle;
      public BufferTypeHandle<CollectedCityServiceUpkeepData> __Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ServiceUsage> __Game_Buildings_ServiceUsage_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      public ComponentLookup<CollectedCityServiceBudgetData> __Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup;
      public BufferLookup<CollectedCityServiceUpkeepData> __Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup;
      public ComponentTypeHandle<CollectedServiceBuildingBudgetData> __Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      public ComponentLookup<CollectedServiceBuildingBudgetData> __Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PlaceableNetComposition> __Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RO_BufferLookup = state.GetBufferLookup<CityStatistic>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceBudgetData_RO_BufferLookup = state.GetBufferLookup<ServiceBudgetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Loan_RO_ComponentLookup = state.GetComponentLookup<Loan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceBudgetData_RO_ComponentLookup = state.GetComponentLookup<CollectedCityServiceBudgetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<CollectedCityServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Creditworthiness_RO_ComponentLookup = state.GetComponentLookup<Creditworthiness>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_City_RO_ComponentLookup = state.GetComponentLookup<Game.City.City>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CollectedCityServiceBudgetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferTypeHandle = state.GetBufferTypeHandle<CollectedCityServiceUpkeepData>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUsage_RO_ComponentLookup = state.GetComponentLookup<ServiceUsage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceBudgetData_RW_ComponentLookup = state.GetComponentLookup<CollectedCityServiceBudgetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedCityServiceUpkeepData_RW_BufferLookup = state.GetBufferLookup<CollectedCityServiceUpkeepData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CollectedServiceBuildingBudgetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_CollectedServiceBuildingBudgetData_RW_ComponentLookup = state.GetComponentLookup<CollectedServiceBuildingBudgetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetComposition>(true);
      }
    }
  }
}
