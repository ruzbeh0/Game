// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialObjectPlacementTriggerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Triggers/", new System.Type[] {})]
  public class TutorialObjectPlacementTriggerPrefab : TutorialTriggerPrefabBase
  {
    [NotNull]
    public TutorialObjectPlacementTriggerPrefab.ObjectPlacementTarget[] m_Targets;
    public int m_RequiredCount;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (TutorialObjectPlacementTriggerPrefab.ObjectPlacementTarget target in this.m_Targets)
        prefabs.Add(target.m_Target);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectPlacementTriggerData>());
      components.Add(ComponentType.ReadWrite<ObjectPlacementTriggerCountData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<ObjectPlacementTriggerData> buffer = entityManager.GetBuffer<ObjectPlacementTriggerData>(entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      foreach (TutorialObjectPlacementTriggerPrefab.ObjectPlacementTarget target in this.m_Targets)
      {
        Entity entity1;
        // ISSUE: reference to a compiler-generated method
        if (existingSystemManaged.TryGetEntity(target.m_Target, out entity1))
          buffer.Add(new ObjectPlacementTriggerData(entity1, target.m_Flags));
      }
      entityManager.SetComponentData<ObjectPlacementTriggerCountData>(entity, new ObjectPlacementTriggerCountData(math.max(this.m_RequiredCount, 1)));
    }

    protected override void GenerateBlinkTags()
    {
      base.GenerateBlinkTags();
      foreach (TutorialObjectPlacementTriggerPrefab.ObjectPlacementTarget target in this.m_Targets)
      {
        UIObject component;
        if (target.m_Target.TryGet<UIObject>(out component) && component.m_Group is UIAssetCategoryPrefab group && (UnityEngine.Object) group.m_Menu != (UnityEngine.Object) null)
        {
          this.AddBlinkTagAtPosition(target.m_Target.uiTag, 0);
          this.AddBlinkTagAtPosition(group.uiTag, 1);
          this.AddBlinkTagAtPosition(group.m_Menu.uiTag, 2);
        }
      }
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Targets.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        linkedPrefabs.Add(existingSystemManaged.GetEntity(this.m_Targets[index].m_Target));
      }
    }

    [Serializable]
    public class ObjectPlacementTarget
    {
      [NotNull]
      public PrefabBase m_Target;
      public ObjectPlacementTriggerFlags m_Flags;
    }
  }
}
