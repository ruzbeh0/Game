// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LifePathEvent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Triggers;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Triggers/", new Type[] {typeof (TriggerPrefab)})]
  public class LifePathEvent : ComponentBase
  {
    public LifePathEventType m_EventType;
    public bool m_IsChirp;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<LifePathEventData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Triggers.LifePathEvent>());
      components.Add(ComponentType.ReadWrite<PrefabRef>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      componentTypeSet.Add(ComponentType.ReadWrite<Game.Triggers.Chirp>());
      componentTypeSet.Add(ComponentType.ReadWrite<ChirpEntity>());
      componentTypeSet.Add(ComponentType.ReadWrite<PrefabRef>());
      LifePathEventData componentData;
      componentData.m_ChirpArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentData.m_IsChirp = this.m_IsChirp;
      componentData.m_EventType = this.m_EventType;
      entityManager.SetComponentData<LifePathEventData>(entity, componentData);
    }
  }
}
