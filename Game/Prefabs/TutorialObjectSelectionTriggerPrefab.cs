// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialObjectSelectionTriggerPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Triggers/", new System.Type[] {})]
  public class TutorialObjectSelectionTriggerPrefab : TutorialTriggerPrefabBase
  {
    [NotNull]
    public TutorialObjectSelectionTriggerPrefab.ObjectSelectionTriggerInfo[] m_Triggers;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_Triggers.Length; ++index)
      {
        prefabs.Add(this.m_Triggers[index].m_Trigger);
        if ((UnityEngine.Object) this.m_Triggers[index].m_GoToPhase != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) this.m_Triggers[index].m_GoToPhase);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectSelectionTriggerData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<ObjectSelectionTriggerData> buffer = entityManager.GetBuffer<ObjectSelectionTriggerData>(entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Triggers.Length; ++index)
      {
        TutorialObjectSelectionTriggerPrefab.ObjectSelectionTriggerInfo trigger = this.m_Triggers[index];
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity(trigger.m_Trigger);
        // ISSUE: reference to a compiler-generated method
        Entity entity2 = (UnityEngine.Object) trigger.m_GoToPhase == (UnityEngine.Object) null ? Entity.Null : existingSystemManaged.GetEntity((PrefabBase) trigger.m_GoToPhase);
        buffer.Add(new ObjectSelectionTriggerData()
        {
          m_Prefab = entity1,
          m_GoToPhase = entity2
        });
      }
      if (this.m_Triggers.Length <= 1)
        return;
      for (int index = 0; index < this.m_Triggers.Length; ++index)
      {
        TutorialPhasePrefab goToPhase = this.m_Triggers[index].m_GoToPhase;
        if ((UnityEngine.Object) goToPhase != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          entityManager.AddComponent<TutorialPhaseBranch>(existingSystemManaged.GetEntity((PrefabBase) goToPhase));
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
      for (int index = 0; index < this.m_Triggers.Length; ++index)
      {
        TutorialObjectSelectionTriggerPrefab.ObjectSelectionTriggerInfo trigger = this.m_Triggers[index];
        // ISSUE: reference to a compiler-generated method
        linkedPrefabs.Add(existingSystemManaged.GetEntity(trigger.m_Trigger));
      }
    }

    public override bool phaseBranching
    {
      get
      {
        return ((IEnumerable<TutorialObjectSelectionTriggerPrefab.ObjectSelectionTriggerInfo>) this.m_Triggers).Any<TutorialObjectSelectionTriggerPrefab.ObjectSelectionTriggerInfo>((Func<TutorialObjectSelectionTriggerPrefab.ObjectSelectionTriggerInfo, bool>) (t => (UnityEngine.Object) t.m_GoToPhase != (UnityEngine.Object) null));
      }
    }

    [Serializable]
    public class ObjectSelectionTriggerInfo
    {
      [NotNull]
      public PrefabBase m_Trigger;
      [CanBeNull]
      public TutorialPhasePrefab m_GoToPhase;
    }
  }
}
