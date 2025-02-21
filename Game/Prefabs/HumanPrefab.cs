// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HumanPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Creatures;
using Game.Pathfind;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Creatures/", new Type[] {})]
  public class HumanPrefab : CreaturePrefab
  {
    public float m_WalkSpeed = 6f;
    public float m_RunSpeed = 12f;
    public float m_Acceleration = 8f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<HumanData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Human>());
      components.Add(ComponentType.ReadWrite<HumanNavigation>());
      components.Add(ComponentType.ReadWrite<Queue>());
      components.Add(ComponentType.ReadWrite<PathOwner>());
      components.Add(ComponentType.ReadWrite<PathElement>());
      components.Add(ComponentType.ReadWrite<Target>());
      components.Add(ComponentType.ReadWrite<Blocker>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<HumanData>(entity, new HumanData()
      {
        m_WalkSpeed = this.m_WalkSpeed / 3.6f,
        m_RunSpeed = this.m_RunSpeed / 3.6f,
        m_Acceleration = this.m_Acceleration
      });
    }
  }
}
