// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CreatureSpawner
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Creatures;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (ObjectPrefab)})]
  public class CreatureSpawner : ComponentBase
  {
    public int m_MaxGroupCount = 3;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<PlaceholderObjectElement>());
      components.Add(ComponentType.ReadWrite<CreatureSpawnData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Creatures.CreatureSpawner>());
      components.Add(ComponentType.ReadWrite<OwnedCreature>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      CreatureSpawnData componentData;
      componentData.m_MaxGroupCount = this.m_MaxGroupCount;
      entityManager.SetComponentData<CreatureSpawnData>(entity, componentData);
    }
  }
}
