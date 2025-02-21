// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialAreaTriggerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Triggers/", new System.Type[] {})]
  public class TutorialAreaTriggerPrefab : TutorialTriggerPrefabBase
  {
    [NotNull]
    public AreaPrefab m_Prefab;
    public AreaTriggerFlags m_Flags = AreaTriggerFlags.Created | AreaTriggerFlags.Modified;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_Prefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<AreaTriggerData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      Entity entity1;
      // ISSUE: reference to a compiler-generated method
      if (!entityManager.World.GetExistingSystemManaged<PrefabSystem>().TryGetEntity((PrefabBase) this.m_Prefab, out entity1))
        return;
      entityManager.SetComponentData<AreaTriggerData>(entity, new AreaTriggerData(entity1, this.m_Flags));
    }

    protected override void GenerateBlinkTags()
    {
      base.GenerateBlinkTags();
      UIObject component;
      if (!this.m_Prefab.TryGet<UIObject>(out component) || !(component.m_Group is UIAssetCategoryPrefab group) || !((UnityEngine.Object) group.m_Menu != (UnityEngine.Object) null))
        return;
      this.AddBlinkTagAtPosition(this.m_Prefab.uiTag, 0);
      this.AddBlinkTagAtPosition(group.uiTag, 1);
      this.AddBlinkTagAtPosition(group.m_Menu.uiTag, 2);
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      linkedPrefabs.Add(existingSystemManaged.GetEntity((PrefabBase) this.m_Prefab));
    }
  }
}
