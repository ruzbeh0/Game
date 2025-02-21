// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BudgetApplySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class BudgetApplySystem : GameSystemBase
  {
    public static int kUpdatesPerDay = 1024;
    private CitySystem m_CitySystem;
    private CityServiceBudgetSystem m_CityServiceBudgetSystem;
    private BudgetApplySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / BudgetApplySystem.kUpdatesPerDay;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgetSystem = this.World.GetOrCreateSystemManaged<CityServiceBudgetSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps1;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BudgetApplySystem.BudgetApplyJob jobData = new BudgetApplySystem.BudgetApplyJob()
      {
        m_PlayerMoneys = this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup,
        m_City = this.m_CitySystem.City,
        m_Expenses = this.m_CityServiceBudgetSystem.GetExpenseArray(out deps1),
        m_Income = this.m_CityServiceBudgetSystem.GetIncomeArray(out deps2)
      };
      this.Dependency = jobData.Schedule<BudgetApplySystem.BudgetApplyJob>(JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityServiceBudgetSystem.AddArrayReader(this.Dependency);
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
    public BudgetApplySystem()
    {
    }

    private struct BudgetApplyJob : IJob
    {
      public NativeArray<int> m_Income;
      public NativeArray<int> m_Expenses;
      public ComponentLookup<PlayerMoney> m_PlayerMoneys;
      public Entity m_City;

      public void Execute()
      {
        int num = 0;
        for (int source = 0; source < 15; ++source)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num -= CityServiceBudgetSystem.GetExpense((ExpenseSource) source, this.m_Expenses);
        }
        for (int source = 0; source < 14; ++source)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          num += CityServiceBudgetSystem.GetIncome((IncomeSource) source, this.m_Income);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PlayerMoney playerMoney = this.m_PlayerMoneys[this.m_City];
        // ISSUE: reference to a compiler-generated field
        playerMoney.Add(num / BudgetApplySystem.kUpdatesPerDay);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PlayerMoneys[this.m_City] = playerMoney;
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<PlayerMoney> __Game_City_PlayerMoney_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_PlayerMoney_RW_ComponentLookup = state.GetComponentLookup<PlayerMoney>();
      }
    }
  }
}
