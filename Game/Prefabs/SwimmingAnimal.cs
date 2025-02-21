// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SwimmingAnimal
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Creatures/", new Type[] {typeof (AnimalPrefab)})]
  public class SwimmingAnimal : ComponentBase
  {
    public float m_SwimSpeed = 20f;
    public Bounds1 m_SwimDepth = new Bounds1(5f, 20f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      AnimalData componentData = entityManager.GetComponentData<AnimalData>(entity) with
      {
        m_SwimSpeed = this.m_SwimSpeed / 3.6f,
        m_SwimDepth = this.m_SwimDepth
      };
      entityManager.SetComponentData<AnimalData>(entity, componentData);
    }
  }
}
