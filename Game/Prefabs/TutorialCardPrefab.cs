// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialCardPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Tutorials;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Phases/", new Type[] {})]
  public class TutorialCardPrefab : TutorialPhasePrefab
  {
    public bool m_CenterCard;

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<TutorialPhaseData>(entity, new TutorialPhaseData()
      {
        m_Type = this.m_CenterCard ? TutorialPhaseType.CenterCard : TutorialPhaseType.Card,
        m_OverrideCompletionDelay = this.m_OverrideCompletionDelay
      });
    }
  }
}
