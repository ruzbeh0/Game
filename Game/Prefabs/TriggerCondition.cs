// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TriggerCondition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Triggers/", new System.Type[] {typeof (TriggerPrefab), typeof (StatisticTriggerPrefab)})]
  public class TriggerCondition : ComponentBase
  {
    [SerializeField]
    public TriggerConditionData[] m_Conditions;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      if (this.m_Conditions == null || this.m_Conditions.Length == 0)
        return;
      components.Add(ComponentType.ReadWrite<TriggerConditionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_Conditions == null || this.m_Conditions.Length == 0)
        return;
      DynamicBuffer<TriggerConditionData> buffer = entityManager.GetBuffer<TriggerConditionData>(entity);
      for (int index = 0; index < this.m_Conditions.Length; ++index)
        buffer.Add(this.m_Conditions[index]);
    }
  }
}
