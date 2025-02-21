// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Fire
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
  public class Fire : ComponentBase
  {
    public EventTargetType m_RandomTargetType;
    public float m_StartProbability = 0.01f;
    public float m_StartIntensity = 1f;
    public float m_EscalationRate = 0.0166666675f;
    public float m_SpreadProbability = 1f;
    public float m_SpreadRange = 20f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<FireData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.Fire>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      FireData componentData;
      componentData.m_RandomTargetType = this.m_RandomTargetType;
      componentData.m_StartProbability = this.m_StartProbability;
      componentData.m_StartIntensity = this.m_StartIntensity;
      componentData.m_EscalationRate = this.m_EscalationRate;
      componentData.m_SpreadProbability = this.m_SpreadProbability;
      componentData.m_SpreadRange = this.m_SpreadRange;
      entityManager.SetComponentData<FireData>(entity, componentData);
    }
  }
}
