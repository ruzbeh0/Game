// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StrictObjectBuiltRequirementPrefab
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
  public class StrictObjectBuiltRequirementPrefab : UnlockRequirementPrefab
  {
    public PrefabBase m_Requirement;
    public int m_MinimumCount = 1;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add(this.m_Requirement);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<StrictObjectBuiltRequirementData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      entityManager.GetBuffer<UnlockRequirement>(entity).Add(new UnlockRequirement(entity, UnlockFlags.RequireAll));
      PrefabBase requirement = this.m_Requirement;
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = existingSystemManaged.GetEntity(requirement);
      StrictObjectBuiltRequirementData componentData = new StrictObjectBuiltRequirementData()
      {
        m_Requirement = entity1,
        m_MinimumCount = this.m_MinimumCount
      };
      entityManager.SetComponentData<StrictObjectBuiltRequirementData>(entity, componentData);
    }
  }
}
