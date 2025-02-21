// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CitizenParametersData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct CitizenParametersData : IComponentData, IQueryTypeParameter
  {
    public float m_DivorceRate;
    public float m_LookForPartnerRate;
    public float2 m_LookForPartnerTypeRate;
    public float m_BaseBirthRate;
    public float m_AdultFemaleBirthRateBonus;
    public float m_StudentBirthRateAdjust;
    public float m_SwitchJobRate;
    public float m_LookForNewJobEmployableRate;
  }
}
