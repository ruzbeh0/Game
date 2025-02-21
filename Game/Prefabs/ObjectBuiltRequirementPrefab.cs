// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectBuiltRequirementPrefab
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
  public class ObjectBuiltRequirementPrefab : UnlockRequirementPrefab
  {
    public int m_MinimumCount = 1;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectBuiltRequirementData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.GetBuffer<UnlockRequirement>(entity).Add(new UnlockRequirement(entity, UnlockFlags.RequireAll));
      ObjectBuiltRequirementData componentData = new ObjectBuiltRequirementData()
      {
        m_MinimumCount = this.m_MinimumCount
      };
      entityManager.SetComponentData<ObjectBuiltRequirementData>(entity, componentData);
    }
  }
}
