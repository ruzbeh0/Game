// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RandomLikeCount
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Chirps/", new System.Type[] {typeof (ChirpPrefab)})]
  public class RandomLikeCount : ComponentBase
  {
    [Tooltip("Percentage of the educated people will give a like")]
    [Range(0.0f, 1f)]
    public float m_EducatedPercentage = 0.8f;
    [Tooltip("Percentage of the uneducated people will give a like")]
    [Range(0.0f, 1f)]
    public float m_UneducatedPercentage = 0.5f;
    [Tooltip("The like count of the chirp will actively increase during these days")]
    public float2 m_ActiveDays = new float2(0.1f, 1f);
    [Tooltip("Go viral means the like count will increase significantly at the beginning in short time")]
    public int2 m_GoViralFactor = new int2(20, 60);
    [Tooltip("Use random amount factor to make the like amount result more randomly)")]
    public float2 m_RandomAmountFactor = new float2(0.01f, 0.8f);
    [Tooltip("Use continuous factor to make the like count change continuous of not continuous, value belong to random[0,1))")]
    public float m_ContinuousFactor = 0.2f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<RandomLikeCountData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      RandomLikeCountData componentData = new RandomLikeCountData()
      {
        m_EducatedPercentage = this.m_EducatedPercentage,
        m_UneducatedPercentage = this.m_UneducatedPercentage,
        m_GoViralFactor = this.m_GoViralFactor,
        m_ActiveDays = this.m_ActiveDays,
        m_RandomAmountFactor = this.m_RandomAmountFactor,
        m_ContinuousFactor = this.m_ContinuousFactor
      };
      entityManager.SetComponentData<RandomLikeCountData>(entity, componentData);
    }
  }
}
