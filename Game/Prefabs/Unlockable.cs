// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Unlockable
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
  public class Unlockable : UnlockableBase
  {
    public PrefabBase[] m_RequireAll;
    public PrefabBase[] m_RequireAny;
    public bool m_IgnoreDependencies;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_RequireAll != null)
      {
        for (int index = 0; index < this.m_RequireAll.Length; ++index)
          prefabs.Add(this.m_RequireAll[index]);
      }
      if (this.m_RequireAny == null)
        return;
      for (int index = 0; index < this.m_RequireAny.Length; ++index)
        prefabs.Add(this.m_RequireAny[index]);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(
      EntityManager entityManager,
      Entity entity,
      List<PrefabBase> dependencies)
    {
      base.LateInitialize(entityManager, entity, dependencies);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<UnlockRequirement> buffer = entityManager.GetBuffer<UnlockRequirement>(entity);
      if (!this.m_IgnoreDependencies)
      {
        for (int index = 0; index < dependencies.Count; ++index)
        {
          PrefabBase dependency = dependencies[index];
          // ISSUE: reference to a compiler-generated method
          if (existingSystemManaged.IsUnlockable(dependency))
          {
            // ISSUE: reference to a compiler-generated method
            Entity entity1 = existingSystemManaged.GetEntity(dependency);
            buffer.Add(new UnlockRequirement(entity1, UnlockFlags.RequireAll));
          }
        }
      }
      if (this.m_RequireAll != null)
      {
        for (int index = 0; index < this.m_RequireAll.Length; ++index)
        {
          PrefabBase prefab = this.m_RequireAll[index];
          // ISSUE: reference to a compiler-generated method
          if (existingSystemManaged.IsUnlockable(prefab))
          {
            // ISSUE: reference to a compiler-generated method
            Entity entity2 = existingSystemManaged.GetEntity(prefab);
            buffer.Add(new UnlockRequirement(entity2, UnlockFlags.RequireAll));
          }
        }
      }
      if (this.m_RequireAny == null)
        return;
      for (int index = 0; index < this.m_RequireAny.Length; ++index)
      {
        PrefabBase prefab = this.m_RequireAny[index];
        // ISSUE: reference to a compiler-generated method
        if (existingSystemManaged.IsUnlockable(prefab))
        {
          // ISSUE: reference to a compiler-generated method
          Entity entity3 = existingSystemManaged.GetEntity(prefab);
          buffer.Add(new UnlockRequirement(entity3, UnlockFlags.RequireAny));
        }
      }
    }
  }
}
