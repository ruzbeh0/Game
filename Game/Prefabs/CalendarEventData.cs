// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CalendarEventData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct CalendarEventData : IComponentData, IQueryTypeParameter
  {
    public EventTargetType m_RandomTargetType;
    public CalendarEventMonths m_AllowedMonths;
    public CalendarEventTimes m_AllowedTimes;
    public Bounds1 m_OccurenceProbability;
    public Bounds1 m_AffectedProbability;
    public int m_Duration;
  }
}
