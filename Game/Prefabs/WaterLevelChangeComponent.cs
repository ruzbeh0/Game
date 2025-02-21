// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterLevelChangeComponent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Events;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Events/", new System.Type[] {typeof (EventPrefab)})]
  public class WaterLevelChangeComponent : ComponentBase
  {
    public WaterLevelTargetType m_TargetType;
    public WaterLevelChangeType m_ChangeType;
    public float m_EscalationDelay = 1f;
    public bool m_Evacuate;
    public bool m_StayIndoors;
    [Tooltip("How dangerous the disaster is for the cims in the city. Determines how likely cims will leave shelter while the disaster is ongoing")]
    [Range(0.0f, 1f)]
    public float m_DangerLevel = 1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WaterLevelChangeData>());
      if (this.m_ChangeType != WaterLevelChangeType.RainControlled)
        return;
      components.Add(ComponentType.ReadWrite<FloodData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WaterLevelChange>());
      components.Add(ComponentType.ReadWrite<Duration>());
      components.Add(ComponentType.ReadWrite<DangerLevel>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
      if (this.m_ChangeType != WaterLevelChangeType.RainControlled)
        return;
      components.Add(ComponentType.ReadWrite<Flood>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      WaterLevelChangeData componentData;
      componentData.m_TargetType = this.m_TargetType;
      componentData.m_ChangeType = this.m_ChangeType;
      componentData.m_EscalationDelay = this.m_EscalationDelay;
      componentData.m_DangerFlags = (DangerFlags) 0;
      if (this.m_Evacuate)
        componentData.m_DangerFlags = DangerFlags.Evacuate;
      if (this.m_StayIndoors)
        componentData.m_DangerFlags = DangerFlags.StayIndoors;
      componentData.m_DangerLevel = this.m_DangerLevel;
      entityManager.SetComponentData<WaterLevelChangeData>(entity, componentData);
    }
  }
}
