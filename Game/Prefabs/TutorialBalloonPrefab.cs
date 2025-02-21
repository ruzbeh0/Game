// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialBalloonPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Tutorials;
using Game.UI;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Phases/", new System.Type[] {})]
  public class TutorialBalloonPrefab : TutorialPhasePrefab
  {
    [NotNull]
    public TutorialBalloonPrefab.BalloonUITarget[] m_UITargets;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      for (int index = 0; index < this.m_UITargets.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_UITargets[index].m_UITagProvider != (UnityEngine.Object) null)
          prefabs.Add(this.m_UITargets[index].m_UITagProvider);
      }
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<TutorialPhaseData>(entity, new TutorialPhaseData()
      {
        m_Type = TutorialPhaseType.Balloon,
        m_OverrideCompletionDelay = this.m_OverrideCompletionDelay
      });
    }

    public override void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
      base.GenerateTutorialLinks(entityManager, linkedPrefabs);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_UITargets.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_UITargets[index].m_UITagProvider != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          linkedPrefabs.Add(existingSystemManaged.GetEntity(this.m_UITargets[index].m_UITagProvider));
        }
      }
    }

    [Serializable]
    public class BalloonUITarget : IJsonWritable
    {
      [NotNull]
      public PrefabBase m_UITagProvider;
      public TutorialBalloonPrefab.BalloonDirection m_BalloonDirection;
      public TutorialBalloonPrefab.BalloonAlignment m_BalloonAlignment;

      public BalloonUITarget()
      {
        this.m_BalloonDirection = TutorialBalloonPrefab.BalloonDirection.up;
        this.m_BalloonAlignment = TutorialBalloonPrefab.BalloonAlignment.center;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(TypeNames.kTutorialBalloonUITarget);
        writer.PropertyName("uiTag");
        writer.Write((UnityEngine.Object) this.m_UITagProvider != (UnityEngine.Object) null ? this.m_UITagProvider.uiTag : string.Empty);
        writer.PropertyName("direction");
        writer.Write(this.m_BalloonDirection.ToString());
        writer.PropertyName("alignment");
        writer.Write(this.m_BalloonAlignment.ToString());
        writer.TypeEnd();
      }
    }

    public enum BalloonDirection
    {
      up,
      down,
      left,
      right,
    }

    public enum BalloonAlignment
    {
      start,
      center,
      end,
    }
  }
}
