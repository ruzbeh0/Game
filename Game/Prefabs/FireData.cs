// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FireData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct FireData : IComponentData, IQueryTypeParameter
  {
    public EventTargetType m_RandomTargetType;
    public float m_StartProbability;
    public float m_StartIntensity;
    public float m_EscalationRate;
    public float m_SpreadProbability;
    public float m_SpreadRange;
  }
}
