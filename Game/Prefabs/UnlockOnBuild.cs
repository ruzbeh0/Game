// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UnlockOnBuild
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/Unlocking/", new Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab), typeof (NetPrefab), typeof (StaticObjectPrefab)})]
  public class UnlockOnBuild : ComponentBase
  {
    public ObjectBuiltRequirementPrefab[] m_Unlocks;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<UnlockOnBuildData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Unlocks.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Unlocks[index]);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<UnlockOnBuildData> buffer = entityManager.GetBuffer<UnlockOnBuildData>(entity);
      for (int index = 0; index < this.m_Unlocks.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) this.m_Unlocks[index]);
        buffer.Add(new UnlockOnBuildData()
        {
          m_Entity = entity1
        });
      }
    }

    public override bool ignoreUnlockDependencies => true;
  }
}
