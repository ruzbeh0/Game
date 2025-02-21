// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterSource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (ObjectPrefab)})]
  public class WaterSource : ComponentBase
  {
    public float m_Radius = 50f;
    public float m_Amount = 1f;
    public float m_Polluted;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<WaterSourceData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Simulation.WaterSourceData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<WaterSourceData>(entity, new WaterSourceData()
      {
        m_Radius = this.m_Radius,
        m_Amount = this.m_Amount,
        m_InitialPolluted = this.m_Polluted
      });
    }
  }
}
