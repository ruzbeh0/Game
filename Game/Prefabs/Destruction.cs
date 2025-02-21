// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Destruction
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
  public class Destruction : ComponentBase
  {
    public EventTargetType m_RandomTargetType;
    public float m_OccurenceProbability = 0.01f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<DestructionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Events.Destruction>());
      components.Add(ComponentType.ReadWrite<TargetElement>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      DestructionData componentData;
      componentData.m_RandomTargetType = this.m_RandomTargetType;
      componentData.m_OccurenceProbability = this.m_OccurenceProbability;
      entityManager.SetComponentData<DestructionData>(entity, componentData);
    }
  }
}
