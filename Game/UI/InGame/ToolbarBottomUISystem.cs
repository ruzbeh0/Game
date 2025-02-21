// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ToolbarBottomUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ToolbarBottomUISystem : UISystemBase
  {
    private const string kGroup = "toolbarBottom";
    private PrefabSystem m_PrefabSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ICityStatisticsSystem m_CityStatisticsSystem;
    private CitySystem m_CitySystem;
    private ICityServiceBudgetSystem m_CityServiceBudgetSystem;
    private GetterValueBinding<string> m_CityNameBinding;
    private GetterValueBinding<int> m_MoneyBinding;
    private GetterValueBinding<int> m_MoneyDeltaBinding;
    private GetterValueBinding<int> m_PopulationBinding;
    private GetterValueBinding<int> m_PopulationDeltaBinding;
    private GetterValueBinding<bool> m_UnlimitedMoneyBinding;
    private UIToolbarBottomConfigurationPrefab m_ToolbarBottomConfigurationPrefab;
    private EntityQuery m_ToolbarBottomConfigurationQuery;
    private ToolbarBottomUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_2118611066_0;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = (ICityStatisticsSystem) this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgetSystem = (ICityServiceBudgetSystem) this.World.GetOrCreateSystemManaged<CityServiceBudgetSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolbarBottomConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIToolbarBottomConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CityNameBinding = new GetterValueBinding<string>("toolbarBottom", "cityName", (Func<string>) (() => this.m_CityConfigurationSystem.cityName ?? ""))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MoneyBinding = new GetterValueBinding<int>("toolbarBottom", "money", (Func<int>) (() => this.m_CitySystem.moneyAmount))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MoneyDeltaBinding = new GetterValueBinding<int>("toolbarBottom", "moneyDelta", (Func<int>) (() =>
      {
        int num = 0;
        for (int source = 0; source < 15; ++source)
        {
          // ISSUE: reference to a compiler-generated field
          num -= this.m_CityServiceBudgetSystem.GetExpense((ExpenseSource) source);
        }
        for (int source = 0; source < 14; ++source)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_CityServiceBudgetSystem.GetIncome((IncomeSource) source);
        }
        return num / 24;
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UnlimitedMoneyBinding = new GetterValueBinding<bool>("toolbarBottom", "unlimitedMoney", (Func<bool>) (() => this.m_CityConfigurationSystem.unlimitedMoney))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PopulationBinding = new GetterValueBinding<int>("toolbarBottom", "population", (Func<int>) (() => this.EntityManager.HasComponent<Population>(this.m_CitySystem.City) ? this.EntityManager.GetComponentData<Population>(this.m_CitySystem.City).m_Population : 0))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PopulationDeltaBinding = new GetterValueBinding<int>("toolbarBottom", "populationDelta", (Func<int>) (() =>
      {
        Population population = new Population();
        // ISSUE: reference to a compiler-generated field
        if (this.EntityManager.HasComponent<Population>(this.m_CitySystem.City))
        {
          // ISSUE: reference to a compiler-generated field
          population = this.EntityManager.GetComponentData<Population>(this.m_CitySystem.City);
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<int> statisticDataArray = this.m_CityStatisticsSystem.GetStatisticDataArray(StatisticType.Population);
        if (statisticDataArray.Length == 0)
          return population.m_Population;
        int x = statisticDataArray.Length >= 2 ? statisticDataArray[statisticDataArray.Length - 2] : 0;
        int y = statisticDataArray[statisticDataArray.Length - 1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float s = (float) (this.m_CityStatisticsSystem.GetSampleFrameIndex(this.m_CityStatisticsSystem.sampleCount - 1) % 8192U) / 8192f;
        return (population.m_Population - Mathf.RoundToInt(math.lerp((float) x, (float) y, s))) * 32 / 24;
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new GetterValueBinding<float2>("toolbarBottom", "populationTrendThresholds", (Func<float2>) (() => new float2(this.m_ToolbarBottomConfigurationPrefab.m_PopulationTrendThresholds.m_Medium, this.m_ToolbarBottomConfigurationPrefab.m_PopulationTrendThresholds.m_High))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new GetterValueBinding<float2>("toolbarBottom", "moneyTrendThresholds", (Func<float2>) (() => new float2(this.m_ToolbarBottomConfigurationPrefab.m_MoneyTrendThresholds.m_Medium, this.m_ToolbarBottomConfigurationPrefab.m_MoneyTrendThresholds.m_High))));
      this.AddBinding((IBinding) new TriggerBinding<string>("toolbarBottom", "setCityName", (Action<string>) (name =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CityConfigurationSystem.cityName = name;
        // ISSUE: reference to a compiler-generated field
        this.m_CityNameBinding.Update();
      })));
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ToolbarBottomConfigurationQuery);
    }

    private int GetMoneyDelta()
    {
      int num = 0;
      for (int source = 0; source < 15; ++source)
      {
        // ISSUE: reference to a compiler-generated field
        num -= this.m_CityServiceBudgetSystem.GetExpense((ExpenseSource) source);
      }
      for (int source = 0; source < 14; ++source)
      {
        // ISSUE: reference to a compiler-generated field
        num += this.m_CityServiceBudgetSystem.GetIncome((IncomeSource) source);
      }
      return num / 24;
    }

    private int GetPopulation()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.HasComponent<Population>(this.m_CitySystem.City) ? this.EntityManager.GetComponentData<Population>(this.m_CitySystem.City).m_Population : 0;
    }

    private int GetPopulationDelta()
    {
      Population population = new Population();
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.HasComponent<Population>(this.m_CitySystem.City))
      {
        // ISSUE: reference to a compiler-generated field
        population = this.EntityManager.GetComponentData<Population>(this.m_CitySystem.City);
      }
      // ISSUE: reference to a compiler-generated field
      NativeArray<int> statisticDataArray = this.m_CityStatisticsSystem.GetStatisticDataArray(StatisticType.Population);
      if (statisticDataArray.Length == 0)
        return population.m_Population;
      int x = statisticDataArray.Length >= 2 ? statisticDataArray[statisticDataArray.Length - 2] : 0;
      int y = statisticDataArray[statisticDataArray.Length - 1];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float s = (float) (this.m_CityStatisticsSystem.GetSampleFrameIndex(this.m_CityStatisticsSystem.sampleCount - 1) % 8192U) / 8192f;
      return (population.m_Population - Mathf.RoundToInt(math.lerp((float) x, (float) y, s))) * 32 / 24;
    }

    private void SetCityName(string name)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.cityName = name;
      // ISSUE: reference to a compiler-generated field
      this.m_CityNameBinding.Update();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CityNameBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_MoneyBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_MoneyDeltaBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationDeltaBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlimitedMoneyBinding.Update();
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_ToolbarBottomConfigurationPrefab == (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolbarBottomConfigurationPrefab = this.m_PrefabSystem.GetPrefab<UIToolbarBottomConfigurationPrefab>(this.__query_2118611066_0.GetSingletonEntity());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_2118611066_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<UIToolbarBottomConfigurationData>()
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

    [Preserve]
    public ToolbarBottomUISystem()
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TypeHandle
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
      }
    }
  }
}
