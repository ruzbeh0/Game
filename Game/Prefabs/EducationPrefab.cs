// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EducationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class EducationPrefab : PrefabBase
  {
    public PrefabBase m_EducationServicePrefab;
    [Tooltip("Probability that a student leaves a disabled school (1024 rolls per day)")]
    [Range(0.0f, 1f)]
    public float m_InoperableSchoolLeaveProbability = 0.1f;
    [Tooltip("Probability that a student enter the high school")]
    [Range(0.0f, 1f)]
    public float m_EnterHighSchoolProbability = 0.75f;
    [Tooltip("Probability for adult to enter the high school")]
    [Range(0.0f, 1f)]
    public float m_AdultEnterHighSchoolProbability = 0.1f;
    [Tooltip("Probability for worker to enter the high school")]
    [Range(0.0f, 1f)]
    public float m_WorkerContinueEducationProbability = 0.1f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<EducationParameterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<EducationParameterData>(entity, new EducationParameterData()
      {
        m_EducationServicePrefab = systemManaged.GetEntity(this.m_EducationServicePrefab),
        m_InoperableSchoolLeaveProbability = this.m_InoperableSchoolLeaveProbability,
        m_EnterHighSchoolProbability = this.m_EnterHighSchoolProbability,
        m_AdultEnterHighSchoolProbability = this.m_AdultEnterHighSchoolProbability,
        m_WorkerContinueEducationProbability = this.m_WorkerContinueEducationProbability
      });
    }
  }
}
