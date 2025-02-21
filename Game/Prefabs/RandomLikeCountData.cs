// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RandomLikeCountData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct RandomLikeCountData : IComponentData, IQueryTypeParameter
  {
    public float m_EducatedPercentage;
    public float m_UneducatedPercentage;
    public float2 m_RandomAmountFactor;
    public float2 m_ActiveDays;
    public float m_ContinuousFactor;
    public int2 m_GoViralFactor;
  }
}
