// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialUIActivation
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
  [ComponentMenu("Tutorials/Activation/", new Type[] {typeof (TutorialPrefab), typeof (TutorialListPrefab)})]
  public class TutorialUIActivation : TutorialActivation
  {
    [NotNull]
    public PrefabBase m_UITagProvider;
    public bool m_CanDeactivate = true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add(this.m_UITagProvider);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<UIActivationData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<UIActivationData>(entity, new UIActivationData(this.m_CanDeactivate));
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      linkedPrefabs.Add(existingSystemManaged.GetEntity(this.m_UITagProvider));
    }
  }
}
