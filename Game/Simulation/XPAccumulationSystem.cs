// Decompiled with JetBrains decompiler
// Type: Game.Simulation.XPAccumulationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Economy;
using Game.Prefabs;
using System;
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
  public class XPAccumulationSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 32;
    private XPSystem m_XPSystem;
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_XPSettingsQuery;
    private XPAccumulationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / XPAccumulationSystem.kUpdatesPerDay;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_XPSystem = this.World.GetOrCreateSystemManaged<XPSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_XPSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<XPParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_XPSettingsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_XP_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      XPAccumulationSystem.XPAccumulateJob jobData = new XPAccumulationSystem.XPAccumulateJob()
      {
        m_CityPopulations = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_CityXPs = this.__TypeHandle.__Game_City_XP_RW_ComponentLookup,
        m_XPParameters = this.m_XPSettingsQuery.GetSingleton<XPParameterData>(),
        m_CityStatistics = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup,
        m_StatsLookup = this.m_CityStatisticsSystem.GetLookup(),
        m_City = this.m_CitySystem.City,
        m_XPQueue = this.m_XPSystem.GetQueue(out deps)
      };
      this.Dependency = jobData.Schedule<XPAccumulationSystem.XPAccumulateJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_XPSystem.AddQueueWriter(this.Dependency);
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
    public XPAccumulationSystem()
    {
    }

    [BurstCompile]
    private struct XPAccumulateJob : IJob
    {
      [ReadOnly]
      public XPParameterData m_XPParameters;
      [ReadOnly]
      public ComponentLookup<Population> m_CityPopulations;
      [ReadOnly]
      public BufferLookup<CityStatistic> m_CityStatistics;
      [ReadOnly]
      public NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> m_StatsLookup;
      public ComponentLookup<XP> m_CityXPs;
      public NativeQueue<XPGain> m_XPQueue;
      [ReadOnly]
      public Entity m_City;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Population cityPopulation = this.m_CityPopulations[this.m_City];
        if (cityPopulation.m_Population < 10)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        XP cityXp = this.m_CityXPs[this.m_City];
        int num1 = math.max(0, cityPopulation.m_Population - cityXp.m_MaximumPopulation);
        cityXp.m_MaximumPopulation = Math.Max(cityXp.m_MaximumPopulation, cityPopulation.m_Population);
        int num2 = 0;
        for (int parameter = 0; parameter < 5; ++parameter)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num2 = CityStatisticsSystem.GetStatisticValue(this.m_StatsLookup, this.m_CityStatistics, StatisticType.ResidentialTaxableIncome, parameter);
        }
        ResourceIterator iterator = ResourceIterator.GetIterator();
        while (iterator.Next())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num2 += CityStatisticsSystem.GetStatisticValue(this.m_StatsLookup, this.m_CityStatistics, StatisticType.CommercialTaxableIncome, (int) iterator.resource) + CityStatisticsSystem.GetStatisticValue(this.m_StatsLookup, this.m_CityStatistics, StatisticType.IndustrialTaxableIncome, (int) iterator.resource) + CityStatisticsSystem.GetStatisticValue(this.m_StatsLookup, this.m_CityStatistics, StatisticType.OfficeTaxableIncome, (int) iterator.resource);
        }
        int val2 = num2 / 10;
        cityXp.m_MaximumIncome = Math.Max(cityXp.m_MaximumIncome, val2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityXPs[this.m_City] = cityXp;
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<XPGain> local1 = ref this.m_XPQueue;
        XPGain xpGain1 = new XPGain();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        xpGain1.amount = Mathf.FloorToInt(this.m_XPParameters.m_XPPerPopulation * (float) num1 / (float) XPAccumulationSystem.kUpdatesPerDay);
        xpGain1.entity = Entity.Null;
        xpGain1.reason = XPReason.Population;
        XPGain xpGain2 = xpGain1;
        local1.Enqueue(xpGain2);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<XPGain> local2 = ref this.m_XPQueue;
        xpGain1 = new XPGain();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        xpGain1.amount = Mathf.FloorToInt(this.m_XPParameters.m_XPPerHappiness * (float) cityPopulation.m_AverageHappiness / (float) XPAccumulationSystem.kUpdatesPerDay);
        xpGain1.entity = Entity.Null;
        xpGain1.reason = XPReason.Happiness;
        XPGain xpGain3 = xpGain1;
        local2.Enqueue(xpGain3);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      public ComponentLookup<XP> __Game_City_XP_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_XP_RW_ComponentLookup = state.GetComponentLookup<XP>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RO_BufferLookup = state.GetBufferLookup<CityStatistic>(true);
      }
    }
  }
}
