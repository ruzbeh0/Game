// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.BudgetUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class BudgetUISystem : UISystemBase
  {
    private const string kGroup = "budget";
    private PrefabSystem m_PrefabSystem;
    private ICityServiceBudgetSystem m_CityServiceBudgetSystem;
    private EntityQuery m_ConfigQuery;
    private GetterValueBinding<int> m_TotalIncomeBinding;
    private GetterValueBinding<int> m_TotalExpensesBinding;
    private RawValueBinding m_IncomeItemsBinding;
    private RawValueBinding m_IncomeValuesBinding;
    private RawValueBinding m_ExpenseItemsBinding;
    private RawValueBinding m_ExpenseValuesBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_CityServiceBudgetSystem = (ICityServiceBudgetSystem) this.World.GetOrCreateSystemManaged<CityServiceBudgetSystem>();
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIEconomyConfigurationData>());
      this.AddBinding((IBinding) (this.m_TotalIncomeBinding = new GetterValueBinding<int>("budget", "totalIncome", (Func<int>) (() => this.m_CityServiceBudgetSystem.GetTotalIncome()))));
      this.AddBinding((IBinding) (this.m_TotalExpensesBinding = new GetterValueBinding<int>("budget", "totalExpenses", (Func<int>) (() => this.m_CityServiceBudgetSystem.GetTotalExpenses()))));
      this.AddBinding((IBinding) (this.m_IncomeItemsBinding = new RawValueBinding("budget", "incomeItems", new Action<IJsonWriter>(this.BindIncomeItems))));
      this.AddBinding((IBinding) (this.m_IncomeValuesBinding = new RawValueBinding("budget", "incomeValues", new Action<IJsonWriter>(this.BindIncomeValues))));
      this.AddBinding((IBinding) (this.m_ExpenseItemsBinding = new RawValueBinding("budget", "expenseItems", new Action<IJsonWriter>(this.BindExpenseItems))));
      this.AddBinding((IBinding) (this.m_ExpenseValuesBinding = new RawValueBinding("budget", "expenseValues", new Action<IJsonWriter>(this.BindExpenseValues))));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_TotalIncomeBinding.Update();
      this.m_TotalExpensesBinding.Update();
      this.m_IncomeValuesBinding.Update();
      this.m_ExpenseValuesBinding.Update();
    }

    private void BindIncomeItems(IJsonWriter writer)
    {
      UIEconomyConfigurationPrefab config = this.GetConfig();
      writer.ArrayBegin(config.m_IncomeItems.Length);
      foreach (BudgetItem<IncomeSource> incomeItem in config.m_IncomeItems)
      {
        writer.TypeBegin("Game.UI.InGame.BudgetItem");
        writer.PropertyName("id");
        writer.Write(incomeItem.m_ID);
        writer.PropertyName("color");
        writer.Write(incomeItem.m_Color);
        writer.PropertyName("icon");
        writer.Write(incomeItem.m_Icon);
        writer.PropertyName("sources");
        writer.ArrayBegin(incomeItem.m_Sources.Length);
        foreach (IncomeSource source in incomeItem.m_Sources)
        {
          writer.TypeBegin("Game.UI.InGame.BudgetSource");
          writer.PropertyName("id");
          writer.Write(Enum.GetName(typeof (IncomeSource), (object) source));
          writer.PropertyName("index");
          writer.Write((int) source);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        writer.TypeEnd();
      }
      writer.ArrayEnd();
    }

    private void BindIncomeValues(IJsonWriter writer)
    {
      writer.ArrayBegin(14U);
      for (int source = 0; source < 14; ++source)
        writer.Write(this.m_CityServiceBudgetSystem.GetIncome((IncomeSource) source));
      writer.ArrayEnd();
    }

    private void BindExpenseItems(IJsonWriter writer)
    {
      UIEconomyConfigurationPrefab config = this.GetConfig();
      writer.ArrayBegin(config.m_ExpenseItems.Length);
      foreach (BudgetItem<ExpenseSource> expenseItem in config.m_ExpenseItems)
      {
        writer.TypeBegin("Game.UI.InGame.BudgetItem");
        writer.PropertyName("id");
        writer.Write(expenseItem.m_ID);
        writer.PropertyName("color");
        writer.Write(expenseItem.m_Color);
        writer.PropertyName("icon");
        writer.Write(expenseItem.m_Icon);
        writer.PropertyName("sources");
        writer.ArrayBegin(expenseItem.m_Sources.Length);
        foreach (ExpenseSource source in expenseItem.m_Sources)
        {
          writer.TypeBegin("Game.UI.InGame.BudgetSource");
          writer.PropertyName("id");
          writer.Write(Enum.GetName(typeof (ExpenseSource), (object) source));
          writer.PropertyName("index");
          writer.Write((int) source);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        writer.TypeEnd();
      }
      writer.ArrayEnd();
    }

    private void BindExpenseValues(IJsonWriter writer)
    {
      writer.ArrayBegin(15U);
      for (int source = 0; source < 15; ++source)
        writer.Write(-this.m_CityServiceBudgetSystem.GetExpense((ExpenseSource) source));
      writer.ArrayEnd();
    }

    private UIEconomyConfigurationPrefab GetConfig()
    {
      // ISSUE: reference to a compiler-generated method
      return this.m_PrefabSystem.GetSingletonPrefab<UIEconomyConfigurationPrefab>(this.m_ConfigQuery);
    }

    [Preserve]
    public BudgetUISystem()
    {
    }
  }
}
