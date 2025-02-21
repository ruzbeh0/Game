// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialHealthProblemActivation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Activation/", new Type[] {typeof (TutorialPrefab)})]
  public class TutorialHealthProblemActivation : TutorialActivation
  {
    public HealthProblemFlags m_Flags;
    public int m_RequiredCount = 10;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadOnly<HealthProblemActivationData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<HealthProblemActivationData>(entity, new HealthProblemActivationData()
      {
        m_Require = this.m_Flags,
        m_RequiredCount = this.m_RequiredCount
      });
    }
  }
}
