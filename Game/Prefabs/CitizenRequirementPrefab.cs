// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CitizenRequirementPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/Unlocking/", new Type[] {})]
  public class CitizenRequirementPrefab : UnlockRequirementPrefab
  {
    public int m_MinimumPopulation = 100;
    public int m_MinimumHappiness;

    public override void GetDependencies(List<PrefabBase> prefabs) => base.GetDependencies(prefabs);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CitizenRequirementData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.GetBuffer<UnlockRequirement>(entity).Add(new UnlockRequirement(entity, UnlockFlags.RequireAll));
      entityManager.SetComponentData<CitizenRequirementData>(entity, new CitizenRequirementData()
      {
        m_MinimumPopulation = this.m_MinimumPopulation,
        m_MinimumHappiness = this.m_MinimumHappiness
      });
    }
  }
}
