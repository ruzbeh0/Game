// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TaxableResource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Resources/", new Type[] {typeof (ResourcePrefab)})]
  public class TaxableResource : ComponentBase
  {
    public TaxAreaType[] m_TaxAreas;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TaxableResourceData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      if (this.m_TaxAreas == null)
        return;
      entityManager.SetComponentData<TaxableResourceData>(entity, new TaxableResourceData((IEnumerable<TaxAreaType>) this.m_TaxAreas));
    }
  }
}
