// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LandValuePrefab
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
  public class LandValuePrefab : PrefabBase
  {
    public InfoviewPrefab m_LandValueInfoViewPrefab;
    [Tooltip("This is the baseline of land value, land value less or equal this won't be showed with gizmos")]
    public float m_LandValueBaseline = 10f;
    [Tooltip("This is the multiplier to the health service coverage bonus")]
    public float m_HealthCoverageBonusMultiplier = 5f;
    [Tooltip("This is the multiplier to the education service coverage bonus")]
    public float m_EducationCoverageBonusMultiplier = 5f;
    [Tooltip("This is the multiplier to the police service coverage bonus")]
    public float m_PoliceCoverageBonusMultiplier = 5f;
    [Tooltip("This is the multiplier to both terrain attractiveness and tourism building attractiveness bonus")]
    public float m_AttractivenessBonusMultiplier = 3f;
    [Tooltip("This is the multiplier to the telecom coverage bonus")]
    public float m_TelecomCoverageBonusMultiplier = 20f;
    [Tooltip("This is the multiplier to the commercial service bonus")]
    public float m_CommercialServiceBonusMultiplier = 10f;
    [Tooltip("This is the multiplier to the bus transportation bonus")]
    public float m_BusBonusMultiplier = 5f;
    [Tooltip("This is the multiplier to the tram or Subway transportation bonus")]
    public float m_TramSubwayBonusMultiplier = 50f;
    [Tooltip("This is the max bonus money a common factor can contribute")]
    public int m_CommonFactorMaxBonus = 100;
    [Tooltip("This is the multiplier to the ground pollution penalty")]
    public float m_GroundPollutionPenaltyMultiplier = 10f;
    [Tooltip("This is the multiplier to the air pollution penalty")]
    public float m_AirPollutionPenaltyMultiplier = 5f;
    [Tooltip("This is the multiplier to the noise pollution penalty")]
    public float m_NoisePollutionPenaltyMultiplier = 0.01f;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_LandValueInfoViewPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<LandValueParameterData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      LandValueParameterData componentData = new LandValueParameterData()
      {
        m_LandValueInfoViewPrefab = systemManaged.GetEntity((PrefabBase) this.m_LandValueInfoViewPrefab),
        m_LandValueBaseline = this.m_LandValueBaseline,
        m_HealthCoverageBonusMultiplier = this.m_HealthCoverageBonusMultiplier,
        m_EducationCoverageBonusMultiplier = this.m_EducationCoverageBonusMultiplier,
        m_PoliceCoverageBonusMultiplier = this.m_PoliceCoverageBonusMultiplier,
        m_AttractivenessBonusMultiplier = this.m_AttractivenessBonusMultiplier,
        m_TelecomCoverageBonusMultiplier = this.m_TelecomCoverageBonusMultiplier,
        m_CommercialServiceBonusMultiplier = this.m_CommercialServiceBonusMultiplier,
        m_BusBonusMultiplier = this.m_BusBonusMultiplier,
        m_TramSubwayBonusMultiplier = this.m_TramSubwayBonusMultiplier,
        m_CommonFactorMaxBonus = (float) this.m_CommonFactorMaxBonus,
        m_GroundPollutionPenaltyMultiplier = this.m_GroundPollutionPenaltyMultiplier,
        m_AirPollutionPenaltyMultiplier = this.m_AirPollutionPenaltyMultiplier,
        m_NoisePollutionPenaltyMultiplier = this.m_NoisePollutionPenaltyMultiplier
      };
      entityManager.SetComponentData<LandValueParameterData>(entity, componentData);
    }
  }
}
