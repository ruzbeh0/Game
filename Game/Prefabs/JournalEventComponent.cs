// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.JournalEventComponent
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
  public class JournalEventComponent : ComponentBase
  {
    public string m_Icon;
    public EventDataTrackingType[] m_TrackedData;
    public EventCityEffectTrackingType[] m_TrackedCityEffects;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<JournalEvent>());
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<JournalEventPrefabData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<JournalEventPrefabData>(entity, new JournalEventPrefabData()
      {
        m_DataFlags = this.GetDataFlags(),
        m_EffectFlags = this.GetEffectFlags()
      });
    }

    public int GetDataFlags()
    {
      int dataFlags = 0;
      for (int index = 0; index < this.m_TrackedData.Length; ++index)
      {
        if (EventJournalUtils.IsValid(this.m_TrackedData[index]))
          dataFlags |= 1 << (int) (this.m_TrackedData[index] & (EventDataTrackingType) 31);
      }
      return dataFlags;
    }

    public int GetEffectFlags()
    {
      int effectFlags = 0;
      for (int index = 0; index < this.m_TrackedCityEffects.Length; ++index)
      {
        if (EventJournalUtils.IsValid(this.m_TrackedCityEffects[index]))
          effectFlags |= 1 << (int) (this.m_TrackedCityEffects[index] & (EventCityEffectTrackingType) 31);
      }
      return effectFlags;
    }
  }
}
