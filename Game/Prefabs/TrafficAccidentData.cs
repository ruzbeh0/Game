// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficAccidentData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TrafficAccidentData : IComponentData, IQueryTypeParameter
  {
    public EventTargetType m_RandomSiteType;
    public EventTargetType m_SubjectType;
    public TrafficAccidentType m_AccidentType;
    public float m_OccurenceProbability;
  }
}
