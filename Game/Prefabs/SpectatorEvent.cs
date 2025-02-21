// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SpectatorEvent
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
  public class SpectatorEvent : ComponentBase
  {
    public EventTargetType m_RandomSiteType = EventTargetType.TransportDepot;
    public float m_PreparationDuration = 0.1f;
    public float m_ActiveDuration = 0.1f;
    public float m_TerminationDuration = 0.1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SpectatorEventData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.SpectatorEvent>());
      components.Add(ComponentType.ReadWrite<Duration>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      SpectatorEventData componentData;
      componentData.m_RandomSiteType = this.m_RandomSiteType;
      componentData.m_PreparationDuration = this.m_PreparationDuration;
      componentData.m_ActiveDuration = this.m_ActiveDuration;
      componentData.m_TerminationDuration = this.m_TerminationDuration;
      entityManager.SetComponentData<SpectatorEventData>(entity, componentData);
    }
  }
}
