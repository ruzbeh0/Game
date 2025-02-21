// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialObjectSelectionActivation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Activation/", new Type[] {typeof (TutorialPrefab)})]
  public class TutorialObjectSelectionActivation : TutorialActivation
  {
    [NotNull]
    public PrefabBase[] m_Prefabs;
    public bool m_AllowTool = true;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (PrefabBase prefab in this.m_Prefabs)
        prefabs.Add(prefab);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectSelectionActivationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<ObjectSelectionActivationData> buffer = entityManager.GetBuffer<ObjectSelectionActivationData>(entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      foreach (PrefabBase prefab in this.m_Prefabs)
      {
        Entity entity1;
        // ISSUE: reference to a compiler-generated method
        if (existingSystemManaged.TryGetEntity(prefab, out entity1))
          buffer.Add(new ObjectSelectionActivationData(entity1, this.m_AllowTool));
      }
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Prefabs.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        linkedPrefabs.Add(existingSystemManaged.GetEntity(this.m_Prefabs[index]));
      }
    }
  }
}
