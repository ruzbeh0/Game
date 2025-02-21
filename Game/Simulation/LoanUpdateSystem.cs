// Decompiled with JetBrains decompiler
// Type: Game.Simulation.LoanUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class LoanUpdateSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 32;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CitySystem m_CitySystem;
    private TriggerSystem m_TriggerSystem;
    private SimulationSystem m_SimulationSystem;
    private LoanUpdateSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / LoanUpdateSystem.kUpdatesPerDay;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Creditworthiness_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Loan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LoanUpdateSystem.LoanUpdateJob jobData = new LoanUpdateSystem.LoanUpdateJob()
      {
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps),
        m_City = this.m_CitySystem.City,
        m_Loans = this.__TypeHandle.__Game_Simulation_Loan_RO_ComponentLookup,
        m_Creditworthinesses = this.__TypeHandle.__Game_Simulation_Creditworthiness_RO_ComponentLookup,
        m_PlayerMoneys = this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup,
        m_CityEffects = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer(),
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex
      };
      this.Dependency = jobData.Schedule<LoanUpdateSystem.LoanUpdateJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
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
    public LoanUpdateSystem()
    {
    }

    [BurstCompile]
    private struct LoanUpdateJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Loan> m_Loans;
      [ReadOnly]
      public ComponentLookup<Creditworthiness> m_Creditworthinesses;
      public ComponentLookup<PlayerMoney> m_PlayerMoneys;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityEffects;
      public NativeQueue<StatisticsEvent> m_StatisticsEventQueue;
      public NativeQueue<TriggerAction> m_TriggerBuffer;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public uint m_SimulationFrameIndex;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Loan loan = this.m_Loans[this.m_City];
        if (loan.m_Amount <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float targetInterest = LoanSystem.GetTargetInterest(loan.m_Amount, this.m_Creditworthinesses[this.m_City].m_Amount, this.m_CityEffects[this.m_City]);
        // ISSUE: reference to a compiler-generated field
        int num = Mathf.RoundToInt((float) loan.m_Amount * targetInterest / (float) LoanUpdateSystem.kUpdatesPerDay);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PlayerMoney playerMoney = this.m_PlayerMoneys[this.m_City];
        // ISSUE: reference to a compiler-generated field
        playerMoney.Add(-num / LoanUpdateSystem.kUpdatesPerDay);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PlayerMoneys[this.m_City] = playerMoney;
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Expense,
          m_Change = (float) num,
          m_Parameter = 1
        });
        // ISSUE: reference to a compiler-generated field
        if (this.m_SimulationFrameIndex - loan.m_LastModified <= 262144U || playerMoney.money <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.UnpaidLoan, Entity.Null, (float) loan.m_Amount));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Loan> __Game_Simulation_Loan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Creditworthiness> __Game_Simulation_Creditworthiness_RO_ComponentLookup;
      public ComponentLookup<PlayerMoney> __Game_City_PlayerMoney_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Loan_RO_ComponentLookup = state.GetComponentLookup<Loan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Creditworthiness_RO_ComponentLookup = state.GetComponentLookup<Creditworthiness>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_PlayerMoney_RW_ComponentLookup = state.GetComponentLookup<PlayerMoney>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
