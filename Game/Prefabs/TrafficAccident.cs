// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficAccident
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Events;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new Type[] {typeof (EventPrefab)})]
  public class TrafficAccident : ComponentBase
  {
    public EventTargetType m_RandomSiteType = EventTargetType.Road;
    public EventTargetType m_SubjectType = EventTargetType.MovingCar;
    public TrafficAccidentType m_AccidentType;
    public float m_OccurrenceProbability = 0.01f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TrafficAccidentData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.TrafficAccident>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      TrafficAccidentData componentData;
      componentData.m_RandomSiteType = this.m_RandomSiteType;
      componentData.m_SubjectType = this.m_SubjectType;
      componentData.m_AccidentType = this.m_AccidentType;
      componentData.m_OccurenceProbability = this.m_OccurrenceProbability;
      entityManager.SetComponentData<TrafficAccidentData>(entity, componentData);
    }
  }
}
