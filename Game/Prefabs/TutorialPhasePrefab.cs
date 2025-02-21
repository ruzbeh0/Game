// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialPhasePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs
{
  public abstract class TutorialPhasePrefab : PrefabBase
  {
    public string m_Image;
    public string m_OverrideImagePS;
    public string m_OverrideImageXBox;
    public string m_Icon;
    public bool m_TitleVisible = true;
    [FormerlySerializedAs("m_ShowDescription")]
    public bool m_DescriptionVisible = true;
    public bool m_CanDeactivate;
    public TutorialPhasePrefab.ControlScheme m_ControlScheme = TutorialPhasePrefab.ControlScheme.All;
    [CanBeNull]
    public TutorialTriggerPrefabBase m_Trigger;
    public float m_OverrideCompletionDelay = -1f;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (!((UnityEngine.Object) this.m_Trigger != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_Trigger);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TutorialPhaseData>());
      if ((UnityEngine.Object) this.m_Trigger != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<TutorialTrigger>());
      if (!this.m_CanDeactivate)
        return;
      components.Add(ComponentType.ReadWrite<TutorialPhaseCanDeactivate>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      if (!entityManager.HasComponent<TutorialTrigger>(entity))
        return;
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<TutorialTrigger>(entity, new TutorialTrigger()
      {
        m_Trigger = existingSystemManaged.GetEntity((PrefabBase) this.m_Trigger)
      });
    }

    public override bool ignoreUnlockDependencies => true;

    public virtual void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      if (!((UnityEngine.Object) this.m_Trigger != (UnityEngine.Object) null))
        return;
      this.m_Trigger.GenerateTutorialLinks(entityManager, linkedPrefabs);
    }

    [Flags]
    public enum ControlScheme
    {
      KeyboardAndMouse = 1,
      Gamepad = 2,
      All = Gamepad | KeyboardAndMouse, // 0x00000003
    }
  }
}
