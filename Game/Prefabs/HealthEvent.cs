// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HealthEvent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Events;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new Type[] {typeof (EventPrefab)})]
  public class HealthEvent : ComponentBase
  {
    public EventTargetType m_RandomTargetType = EventTargetType.Citizen;
    public HealthEventType m_HealthEventType;
    public Bounds1 m_OccurenceProbability = new Bounds1(0.0f, 50f);
    public Bounds1 m_TransportProbability = new Bounds1(0.0f, 100f);
    public bool m_RequireTracking = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<HealthEventData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.HealthEvent>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      HealthEventData componentData;
      componentData.m_RandomTargetType = this.m_RandomTargetType;
      componentData.m_HealthEventType = this.m_HealthEventType;
      componentData.m_OccurenceProbability = this.m_OccurenceProbability;
      componentData.m_TransportProbability = this.m_TransportProbability;
      componentData.m_RequireTracking = this.m_RequireTracking;
      entityManager.SetComponentData<HealthEventData>(entity, componentData);
    }
  }
}
