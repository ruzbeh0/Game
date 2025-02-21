// Decompiled with JetBrains decompiler
// Type: Game.Tools.LoanSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.City;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class LoanSystem : GameSystemBase, ILoanSystem
  {
    private CitySystem m_CitySystem;
    private SimulationSystem m_SimulationSystem;
    private NativeQueue<LoanAction> m_ActionQueue;
    private JobHandle m_ActionQueueWriters;
    private LoanSystem.TypeHandle __TypeHandle;

    public LoanInfo CurrentLoan
    {
      get
      {
        Loan component;
        return this.EntityManager.TryGetComponent<Loan>(this.m_CitySystem.City, out component) ? this.CalculateLoan(component.m_Amount) : new LoanInfo();
      }
    }

    public int Creditworthiness
    {
      get => this.EntityManager.GetComponentData<Game.Simulation.Creditworthiness>(this.m_CitySystem.City).m_Amount;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionQueue = new NativeQueue<LoanAction>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_ActionQueue.Dispose();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ActionQueue.IsEmpty())
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Loan_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LoanSystem.LoanActionJob jobData = new LoanSystem.LoanActionJob()
      {
        m_City = this.m_CitySystem.City,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_ActionQueue = this.m_ActionQueue,
        m_Loans = this.__TypeHandle.__Game_Simulation_Loan_RW_ComponentLookup,
        m_Money = this.__TypeHandle.__Game_City_PlayerMoney_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<LoanSystem.LoanActionJob>(JobHandle.CombineDependencies(this.m_ActionQueueWriters, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_ActionQueueWriters = this.Dependency;
    }

    public LoanInfo RequestLoanOffer(int amount)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.CalculateLoan(this.ClampLoanAmount(amount));
    }

    public void ChangeLoan(int amount)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActionQueueWriters.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ActionQueue.Enqueue(new LoanAction()
      {
        m_Amount = this.ClampLoanAmount(amount)
      });
    }

    private int ClampLoanAmount(int amount)
    {
      // ISSUE: reference to a compiler-generated field
      int a = math.max(0, this.CurrentLoan.m_Amount - math.max(0, this.EntityManager.GetComponentData<PlayerMoney>(this.m_CitySystem.City).money));
      return math.clamp(amount, a, this.Creditworthiness);
    }

    public LoanInfo CalculateLoan(int amount)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return LoanSystem.CalculateLoan(amount, this.Creditworthiness, this.EntityManager.GetBuffer<CityModifier>(this.m_CitySystem.City, true));
    }

    public static LoanInfo CalculateLoan(
      int amount,
      int creditworthiness,
      DynamicBuffer<CityModifier> modifiers)
    {
      if (amount <= 0)
        return new LoanInfo();
      // ISSUE: reference to a compiler-generated method
      float targetInterest = LoanSystem.GetTargetInterest(amount, creditworthiness, modifiers);
      return new LoanInfo()
      {
        m_Amount = amount,
        m_DailyInterestRate = targetInterest,
        m_DailyPayment = Mathf.RoundToInt((float) amount * targetInterest)
      };
    }

    public static float GetTargetInterest(
      int loanAmount,
      int creditworthiness,
      DynamicBuffer<CityModifier> cityEffects)
    {
      float num = 100f * math.lerp(0.02f, 0.2f, math.saturate((float) loanAmount / math.max(1f, (float) creditworthiness)));
      CityUtils.ApplyModifier(ref num, cityEffects, CityModifierType.LoanInterest);
      return math.max(0.0f, 0.01f * num);
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
    public LoanSystem()
    {
    }

    private struct LoanActionJob : IJob
    {
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      public NativeQueue<LoanAction> m_ActionQueue;
      public ComponentLookup<Loan> m_Loans;
      public ComponentLookup<PlayerMoney> m_Money;

      public void Execute()
      {
        LoanAction loanAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out loanAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PlayerMoney playerMoney = this.m_Money[this.m_City];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          playerMoney.Add(loanAction.m_Amount - this.m_Loans[this.m_City].m_Amount);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Money[this.m_City] = playerMoney;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Loans[this.m_City] = new Loan()
          {
            m_Amount = loanAction.m_Amount,
            m_LastModified = this.m_SimulationFrameIndex
          };
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<Loan> __Game_Simulation_Loan_RW_ComponentLookup;
      public ComponentLookup<PlayerMoney> __Game_City_PlayerMoney_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Loan_RW_ComponentLookup = state.GetComponentLookup<Loan>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_PlayerMoney_RW_ComponentLookup = state.GetComponentLookup<PlayerMoney>();
      }
    }
  }
}
