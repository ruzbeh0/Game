// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BudgetSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Economy;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class BudgetSystem : GameSystemBase, IBudgetSystem
  {
    private uint m_LastUpdate;
    private SimulationSystem m_SimulationSystem;
    protected NativeArray<int> m_Trade;
    protected NativeArray<int> m_TradeWorth;
    protected NativeArray<int2> m_HouseholdWealth;
    protected NativeArray<int2> m_ServiceWealth;
    protected NativeArray<int2> m_ProcessingWealth;
    protected int m_TotalTradeWorth;
    protected int m_TotalTaxIncome;
    private NativeArray<int> m_HouseholdCount;
    private NativeArray<int> m_ServiceCount;
    private NativeArray<int> m_ProcessingCount;
    private NativeArray<int2> m_HouseholdWorkers;
    private NativeArray<int2> m_ServiceWorkers;
    private NativeArray<int2> m_ProcessingWorkers;
    private NativeArray<float2> m_CitizenWellbeing;
    private NativeArray<int> m_TouristCount;
    private NativeArray<int> m_TouristIncome;
    private NativeArray<int2> m_LodgingData;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private ResourceSystem m_ResourceSystem;
    private BudgetSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 32768;

    public bool HasData => this.m_LastUpdate > 0U;

    public uint LastUpdate => this.m_LastUpdate;

    public int GetTotalTradeWorth() => this.m_TotalTradeWorth;

    public int GetHouseholdCount() => this.m_HouseholdCount[0];

    public int GetCompanyCount(bool service, Resource resource)
    {
      int resourceIndex = EconomyUtils.GetResourceIndex(resource);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !service ? this.m_ProcessingCount[resourceIndex] : this.m_ServiceCount[resourceIndex];
    }

    public int2 GetHouseholdWorkers() => this.m_HouseholdWorkers[0];

    public int2 GetCompanyWorkers(bool service, Resource resource)
    {
      int resourceIndex = EconomyUtils.GetResourceIndex(resource);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !service ? this.m_ProcessingWorkers[resourceIndex] : this.m_ServiceWorkers[resourceIndex];
    }

    public float2 GetCitizenWellbeing() => this.m_CitizenWellbeing[0];

    public int GetTrade(Resource resource) => this.m_Trade[EconomyUtils.GetResourceIndex(resource)];

    public int GetTradeWorth(Resource resource)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_TradeWorth[EconomyUtils.GetResourceIndex(resource)];
    }

    private void SetTradeWorth(Resource resource, ResourceData resourceData)
    {
      int resourceIndex = EconomyUtils.GetResourceIndex(resource);
      float marketPrice = EconomyUtils.GetMarketPrice(resourceData);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TradeWorth[resourceIndex] = Mathf.RoundToInt(marketPrice * (float) this.m_Trade[resourceIndex]);
    }

    public int GetHouseholdWealth()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_HouseholdWealth[0].y > 0 ? this.m_HouseholdWealth[0].x / this.m_HouseholdWealth[0].y : 0;
    }

    public int GetCompanyWealth(bool service, Resource resource)
    {
      int resourceIndex = EconomyUtils.GetResourceIndex(resource);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int2 int2 = service ? this.m_ServiceWealth[resourceIndex] : this.m_ProcessingWealth[resourceIndex];
      return int2.y > 0 ? int2.x / int2.y : 0;
    }

    public int GetTouristCount() => this.m_TouristCount[0];

    public int2 GetLodgingData() => this.m_LodgingData[0];

    public int GetTouristIncome() => this.m_TouristIncome[0];

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      int resourceCount = EconomyUtils.ResourceCount;
      // ISSUE: reference to a compiler-generated field
      this.m_Trade = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TradeWorth = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdWealth = new NativeArray<int2>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceWealth = new NativeArray<int2>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingWealth = new NativeArray<int2>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenWellbeing = new NativeArray<float2>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCount = new NativeArray<int>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdWorkers = new NativeArray<int2>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceCount = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceWorkers = new NativeArray<int2>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingCount = new NativeArray<int>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingWorkers = new NativeArray<int2>(resourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TouristCount = new NativeArray<int>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TouristIncome = new NativeArray<int>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LodgingData = new NativeArray<int2>(1, Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Trade.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeWorth.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdWealth.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceWealth.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingWealth.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenWellbeing.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCount.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceCount.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingCount.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessingWorkers.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TouristCount.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TouristIncome.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LodgingData.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate() => this.UpdateData();

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateData();
    }

    private void UpdateData()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastUpdate = this.m_SimulationSystem.frameIndex;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      BudgetSystem.TypeHandle typeHandle = this.__TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      ResourceIterator iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
      {
        if (this.EntityManager.HasComponent<ResourceData>(prefabs[iterator.resource]) && this.EntityManager.GetComponentData<ResourceData>(prefabs[iterator.resource]).m_IsTradable)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Trade[EconomyUtils.GetResourceIndex(iterator.resource)] = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.Trade, EconomyUtils.GetResourceIndex(iterator.resource));
        }
      }
      iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
      {
        if (this.EntityManager.HasComponent<ResourceData>(prefabs[iterator.resource]))
        {
          ResourceData componentData = this.EntityManager.GetComponentData<ResourceData>(prefabs[iterator.resource]);
          if (componentData.m_IsTradable)
          {
            // ISSUE: reference to a compiler-generated method
            this.SetTradeWorth(iterator.resource, componentData);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue1 = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.HouseholdCount, 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_HouseholdWealth[0] = new int2(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.HouseholdWealth, 0), statisticValue1);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCount[0] = statisticValue1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue2 = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.Population, 0);
      if (statisticValue2 > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_CitizenWellbeing[0] = new float2((float) this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.WellbeingLevel, 0) / (float) statisticValue2, (float) this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.HealthLevel, 0) / (float) statisticValue2);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue3 = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.WorkerCount, 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_HouseholdWorkers[0] = new int2(statisticValue3, statisticValue3 + this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.Unemployed, 0));
      iterator = ResourceIterator.GetIterator();
      while (iterator.Next())
      {
        int resourceIndex = EconomyUtils.GetResourceIndex(iterator.resource);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ServiceWealth[resourceIndex] = new int2(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ServiceWealth, resourceIndex), this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ServiceCount, resourceIndex));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ProcessingWealth[resourceIndex] = new int2(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ProcessingWealth, resourceIndex), this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ProcessingCount, resourceIndex));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ServiceCount[resourceIndex] = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ServiceCount, resourceIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ServiceWorkers[resourceIndex] = new int2(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ServiceWorkers, resourceIndex), this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ServiceMaxWorkers, resourceIndex));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ProcessingCount[resourceIndex] = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ProcessingCount, resourceIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ProcessingWorkers[resourceIndex] = new int2(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ProcessingWorkers, resourceIndex), this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.ProcessingMaxWorkers, resourceIndex));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TouristIncome[0] = this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.TouristIncome, 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LodgingData[0] = new int2(this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.LodgingUsed, 0), this.m_CityStatisticsSystem.GetStatisticValue(StatisticType.LodgingTotal, 0));
      // ISSUE: reference to a compiler-generated method
      this.UpdateTotalTradeWorth();
    }

    private void UpdateTotalTradeWorth()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TotalTradeWorth = 0;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_TradeWorth.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TotalTradeWorth += this.m_TradeWorth[index];
      }
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

    [Preserve]
    public BudgetSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RO_BufferLookup = state.GetBufferLookup<CityStatistic>(true);
      }
    }
  }
}
