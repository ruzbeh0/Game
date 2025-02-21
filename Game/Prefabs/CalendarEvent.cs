// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CalendarEvent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Events;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new System.Type[] {typeof (EventPrefab)})]
  public class CalendarEvent : ComponentBase
  {
    public EventTargetType m_RandomTargetType = EventTargetType.Couple;
    public Bounds1 m_AffectedProbability = new Bounds1(25f, 25f);
    public Bounds1 m_OccurenceProbability = new Bounds1(100f, 100f);
    [EnumFlag]
    public CalendarEventMonths m_AllowedMonths;
    [EnumFlag]
    public CalendarEventTimes m_AllowedTimes;
    [Tooltip("In fourths of a day")]
    public int m_Duration;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<CalendarEventData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.CalendarEvent>());
      components.Add(ComponentType.ReadWrite<Duration>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      CalendarEventData componentData;
      componentData.m_RandomTargetType = this.m_RandomTargetType;
      componentData.m_AffectedProbability = this.m_AffectedProbability;
      componentData.m_OccurenceProbability = this.m_OccurenceProbability;
      componentData.m_AllowedMonths = this.m_AllowedMonths;
      componentData.m_AllowedTimes = this.m_AllowedTimes;
      componentData.m_Duration = this.m_Duration;
      entityManager.SetComponentData<CalendarEventData>(entity, componentData);
    }
  }
}
