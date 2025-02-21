// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialAlternative
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/", new System.Type[] {typeof (TutorialPrefab)})]
  public class TutorialAlternative : ComponentBase
  {
    [Tooltip("If one of the alternatives is completed, this is skipped in tutorial list.")]
    [NotNull]
    public TutorialPrefab[] m_Alternatives;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Tutorials.TutorialAlternative>());
    }

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      foreach (TutorialPrefab alternative in this.m_Alternatives)
        prefabs.Add((PrefabBase) alternative);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      DynamicBuffer<Game.Tutorials.TutorialAlternative> buffer = entityManager.GetBuffer<Game.Tutorials.TutorialAlternative>(entity);
      foreach (TutorialPrefab alternative in this.m_Alternatives)
      {
        // ISSUE: reference to a compiler-generated method
        buffer.Add(new Game.Tutorials.TutorialAlternative()
        {
          m_Alternative = systemManaged.GetEntity((PrefabBase) alternative)
        });
      }
    }
  }
}
