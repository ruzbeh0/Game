// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LoanUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Tools;
using System;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class LoanUISystem : UISystemBase
  {
    private const string kGroup = "loan";
    private GetterValueBinding<int> m_LoanLimitBinding;
    private GetterValueBinding<LoanInfo> m_CurrentLoanBinding;
    private GetterValueBinding<LoanInfo> m_LoanOfferBinding;
    private ILoanSystem m_LoanSystem;
    private int m_RequestedOfferDifference;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_LoanSystem = (ILoanSystem) this.World.GetOrCreateSystemManaged<LoanSystem>();
      this.AddBinding((IBinding) (this.m_LoanLimitBinding = new GetterValueBinding<int>("loan", "loanLimit", (Func<int>) (() => this.m_LoanSystem.Creditworthiness))));
      this.AddBinding((IBinding) (this.m_CurrentLoanBinding = new GetterValueBinding<LoanInfo>("loan", "currentLoan", (Func<LoanInfo>) (() => this.m_LoanSystem.CurrentLoan), (IWriter<LoanInfo>) new LoanUISystem.LoanWriter())));
      this.AddBinding((IBinding) (this.m_LoanOfferBinding = new GetterValueBinding<LoanInfo>("loan", "loanOffer", (Func<LoanInfo>) (() => this.m_LoanSystem.RequestLoanOffer(this.m_LoanSystem.CurrentLoan.m_Amount + this.m_RequestedOfferDifference)), (IWriter<LoanInfo>) new LoanUISystem.LoanWriter())));
      this.AddBinding((IBinding) new TriggerBinding<int>("loan", "requestLoanOffer", new Action<int>(this.RequestLoanOffer)));
      this.AddBinding((IBinding) new TriggerBinding("loan", "acceptLoanOffer", new Action(this.AcceptLoanOffer)));
      this.AddBinding((IBinding) new TriggerBinding("loan", "resetLoanOffer", new Action(this.ResetLoanOffer)));
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.m_RequestedOfferDifference = 0;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_LoanLimitBinding.Update();
      this.m_CurrentLoanBinding.Update();
      this.m_LoanOfferBinding.Update();
    }

    private void RequestLoanOffer(int amount)
    {
      this.m_RequestedOfferDifference = this.m_LoanSystem.RequestLoanOffer(amount).m_Amount - this.m_LoanSystem.CurrentLoan.m_Amount;
    }

    private void AcceptLoanOffer()
    {
      this.m_LoanSystem.ChangeLoan(this.m_LoanSystem.CurrentLoan.m_Amount + this.m_RequestedOfferDifference);
      this.m_RequestedOfferDifference = 0;
    }

    private void ResetLoanOffer() => this.m_RequestedOfferDifference = 0;

    [Preserve]
    public LoanUISystem()
    {
    }

    public class LoanWriter : IWriter<LoanInfo>
    {
      public void Write(IJsonWriter writer, LoanInfo value)
      {
        writer.TypeBegin("loan.Loan");
        writer.PropertyName("amount");
        writer.Write(value.m_Amount);
        writer.PropertyName("dailyInterestRate");
        writer.Write(value.m_DailyInterestRate);
        writer.PropertyName("dailyPayment");
        writer.Write(value.m_DailyPayment);
        writer.TypeEnd();
      }
    }
  }
}
