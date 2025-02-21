// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/", new Type[] {})]
  public class TutorialPrefab : PrefabBase
  {
    [NotNull]
    public TutorialPhasePrefab[] m_Phases;
    public int m_Priority;
    public bool m_ReplaceActive;
    public bool m_Mandatory;
    public bool m_EditorTutorial;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (TutorialPhasePrefab phase in this.m_Phases)
        prefabs.Add((PrefabBase) phase);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TutorialData>());
      components.Add(ComponentType.ReadWrite<TutorialPhaseRef>());
      if (!this.m_ReplaceActive)
        return;
      components.Add(ComponentType.ReadWrite<ReplaceActiveData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<TutorialData>(entity, new TutorialData(this.m_Priority));
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<TutorialPhaseRef> buffer1 = entityManager.GetBuffer<TutorialPhaseRef>(entity);
      NativeParallelHashSet<Entity> linkedPrefabs = new NativeParallelHashSet<Entity>(5, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      foreach (TutorialPhasePrefab phase in this.m_Phases)
      {
        // ISSUE: reference to a compiler-generated method
        Entity entity1 = existingSystemManaged.GetEntity((PrefabBase) phase);
        TutorialPhaseRef elem = new TutorialPhaseRef()
        {
          m_Phase = entity1
        };
        buffer1.Add(elem);
        phase.GenerateTutorialLinks(entityManager, linkedPrefabs);
      }
      foreach (Entity entity2 in linkedPrefabs)
      {
        DynamicBuffer<TutorialLinkData> buffer2;
        if (!entityManager.TryGetBuffer<TutorialLinkData>(entity2, false, out buffer2))
          buffer2 = entityManager.AddBuffer<TutorialLinkData>(entity2);
        buffer2.Add(new TutorialLinkData()
        {
          m_Tutorial = entity
        });
      }
      if (this.m_EditorTutorial)
        entityManager.AddComponent<EditorTutorial>(entity);
      linkedPrefabs.Dispose();
    }

    public override bool ignoreUnlockDependencies => true;
  }
}
