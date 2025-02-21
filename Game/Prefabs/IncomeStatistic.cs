// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.IncomeStatistic
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Statistics/", new Type[] {typeof (StatisticsPrefab)})]
  public class IncomeStatistic : ParametricStatistic
  {
    public IncomeSourceInfo[] m_Incomes;

    public override IEnumerable<StatisticParameterData> GetParameters()
    {
      if (this.m_Incomes != null)
      {
        for (int i = 0; i < this.m_Incomes.Length; ++i)
          yield return new StatisticParameterData((int) this.m_Incomes[i].m_IncomeSource, this.m_Incomes[i].m_Color);
      }
    }

    public override string GetParameterName(int parameter)
    {
      return Enum.GetName(typeof (IncomeSource), (object) parameter);
    }
  }
}
