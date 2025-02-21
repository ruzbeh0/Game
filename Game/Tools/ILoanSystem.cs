// Decompiled with JetBrains decompiler
// Type: Game.Tools.ILoanSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Tools
{
  public interface ILoanSystem
  {
    LoanInfo CurrentLoan { get; }

    int Creditworthiness { get; }

    LoanInfo RequestLoanOffer(int amount);

    void ChangeLoan(int amount);
  }
}
