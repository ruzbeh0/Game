// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialUITriggerPrefab
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
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Triggers/", new System.Type[] {})]
  public class TutorialUITriggerPrefab : TutorialTriggerPrefabBase
  {
    [NotNull]
    public TutorialUITriggerPrefab.UITriggerInfo[] m_UITriggers;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<UITriggerData>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_UITriggers.Length; ++index)
      {
        prefabs.Add(this.m_UITriggers[index].m_UITagProvider);
        if ((UnityEngine.Object) this.m_UITriggers[index].m_GoToPhase != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) this.m_UITriggers[index].m_GoToPhase);
      }
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_UITriggers.Length <= 1)
        return;
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_UITriggers.Length; ++index)
      {
        TutorialPhasePrefab goToPhase = this.m_UITriggers[index].m_GoToPhase;
        if ((UnityEngine.Object) goToPhase != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          entityManager.AddComponent<TutorialPhaseBranch>(existingSystemManaged.GetEntity((PrefabBase) goToPhase));
        }
      }
    }

    protected override void GenerateBlinkTags()
    {
      base.GenerateBlinkTags();
      for (int index = 0; index < this.m_UITriggers.Length; ++index)
      {
        if (!this.m_UITriggers[index].m_DisableBlinking)
        {
          foreach (string str in this.m_UITriggers[index].m_UITagProvider.uiTag.Split('|', StringSplitOptions.None))
            this.AddBlinkTag(str.Trim());
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
      for (int index = 0; index < this.m_UITriggers.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        linkedPrefabs.Add(existingSystemManaged.GetEntity(this.m_UITriggers[index].m_UITagProvider));
      }
    }

    public override bool phaseBranching
    {
      get
      {
        return ((IEnumerable<TutorialUITriggerPrefab.UITriggerInfo>) this.m_UITriggers).Any<TutorialUITriggerPrefab.UITriggerInfo>((Func<TutorialUITriggerPrefab.UITriggerInfo, bool>) (t => (UnityEngine.Object) t.m_GoToPhase != (UnityEngine.Object) null));
      }
    }

    [Serializable]
    public class UITriggerInfo
    {
      [NotNull]
      public PrefabBase m_UITagProvider;
      [CanBeNull]
      [Tooltip("If set, advances the tutorial to the specified phase when triggered.\n\nOverrides TutorialPhasePrefab GoToPhase.")]
      public TutorialPhasePrefab m_GoToPhase;
      public bool m_DisableBlinking;
    }
  }
}
