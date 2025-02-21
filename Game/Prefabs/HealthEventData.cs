// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HealthEventData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct HealthEventData : IComponentData, IQueryTypeParameter
  {
    public EventTargetType m_RandomTargetType;
    public HealthEventType m_HealthEventType;
    public Bounds1 m_OccurenceProbability;
    public Bounds1 m_TransportProbability;
    public bool m_RequireTracking;
  }
}
