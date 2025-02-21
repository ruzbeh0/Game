// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RadioEvent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Triggers/", new System.Type[] {typeof (TriggerPrefab), typeof (StatisticTriggerPrefab)})]
  public class RadioEvent : ComponentBase
  {
    public Game.Audio.Radio.Radio.SegmentType m_SegmentType = Game.Audio.Radio.Radio.SegmentType.News;
    [Tooltip("Only for emergency events")]
    [ShowIf("m_SegmentType", 6, false)]
    public int m_EmergencyFrameDelay;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<RadioEventData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Triggers.RadioEvent>());
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
      RadioEventData componentData;
      componentData.m_Archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentData.m_SegmentType = this.m_SegmentType;
      componentData.m_EmergencyFrameDelay = this.m_EmergencyFrameDelay;
      entityManager.SetComponentData<RadioEventData>(entity, componentData);
    }
  }
}
