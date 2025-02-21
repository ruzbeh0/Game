// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StatisticTriggerData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct StatisticTriggerData : IComponentData, IQueryTypeParameter
  {
    public StatisticTriggerType m_Type;
    public Entity m_StatisticEntity;
    public int m_StatisticParameter;
    public Entity m_NormalizeWithPrefab;
    public int m_NormalizeWithParameter;
    public int m_TimeFrame;
    public int m_MinSamples;
  }
}
