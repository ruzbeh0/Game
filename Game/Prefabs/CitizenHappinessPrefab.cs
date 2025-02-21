// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CitizenHappinessPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class CitizenHappinessPrefab : PrefabBase
  {
    [Header("Pollution")]
    public int m_PollutionDivisor = 600;
    public int m_MaxAirAndGroundPollution = 50;
    public int m_MaxNoisePollution = 15;
    [Header("Electricity")]
    [Tooltip("How much the lack of electricity affects well-being")]
    public float m_ElectricityWellbeingPenalty = 20f;
    [Tooltip("Delay in ticks until no electricity happiness penalty is fully applied. One tick equals ~1.42 ingame minutes.")]
    [Min(1f)]
    public byte m_ElectricityPenaltyDelay = 32;
    [Tooltip("Defines how the electricity fee (0-200%) affects well-being")]
    public AnimationCurve m_ElectricityFeeWellbeingEffect;
    [Header("Water & Sewage")]
    [Tooltip("How much the lack of water affects health")]
    public int m_WaterHealthPenalty = 20;
    [Tooltip("How much the lack of water affects well-being")]
    public int m_WaterWellbeingPenalty = 20;
    [Tooltip("Delay in ticks until no water happiness penalty is fully applied. One tick equals ~1.42 ingame minutes.")]
    [Min(1f)]
    public byte m_WaterPenaltyDelay = 32;
    [Tooltip("How much water pollution affects health")]
    public float m_WaterPollutionMultiplier = -10f;
    [Tooltip("How much the lack of sewage treatment affects health")]
    public int m_SewageHealthEffect = 10;
    [Tooltip("How much the lack of sewage treatment affects well-being")]
    public int m_SewageWellbeingEffect = 20;
    [Tooltip("Delay in ticks until no sewage treatment happiness penalty is fully applied. One tick equals ~1.42 ingame minutes.")]
    [Min(1f)]
    public byte m_SewagePenaltyDelay = 32;
    [Tooltip("Defines how the water fee (0-200%) affects health")]
    public AnimationCurve m_WaterFeeHealthEffect;
    [Tooltip("Defines how the water fee (0-200%) affects well-being")]
    public AnimationCurve m_WaterFeeWellbeingEffect;
    [Tooltip("Wealth level max money amount , x-Wretched y-Poor z-Modest w-Comfortable, above w is Wealthy")]
    public int4 m_WealthyMoneyAmount = new int4(0, 1000, 3000, 5000);
    [Header("Other")]
    public float m_HealthCareHealthMultiplier = 2f;
    public float m_HealthCareWellbeingMultiplier = 0.8f;
    public float m_EducationWellbeingMultiplier = 3f;
    public float m_NeutralEducation = 5f;
    public float m_EntertainmentWellbeingMultiplier = 20f;
    [Tooltip("Crime under this level has no effect on wellbeing")]
    public int m_NegligibleCrime = 5000;
    public float m_CrimeMultiplier = 0.0004f;
    public int m_MaxCrimePenalty = 30;
    public float m_MailMultiplier = 2f;
    public int m_NegligibleMail = 25;
    public float m_TelecomBaseline = 0.3f;
    public float m_TelecomBonusMultiplier = 10f;
    public float m_TelecomPenaltyMultiplier = 20f;
    public float m_WelfareMultiplier = 2f;
    public int m_HealthProblemHealthPenalty = 20;
    public int m_DeathWellbeingPenalty = 20;
    public int m_DeathHealthPenalty = 10;
    public float m_ConsumptionMultiplier = 1f;
    [Tooltip("This is used for statistics to divide the low well-being citizen")]
    public int m_LowWellbeing = 40;
    public int m_LowHealth = 40;
    public float m_TaxUneducatedMultiplier = -0.25f;
    public float m_TaxPoorlyEducatedMultiplier = -0.5f;
    public float m_TaxEducatedMultiplier = -1f;
    public float m_TaxWellEducatedMultiplier = -1.5f;
    public float m_TaxHighlyEducatedMultiplier = -2f;
    [Tooltip("Temporary penalty for teleporting due to traffic problems")]
    public int m_PenaltyEffect = -30;
    public int m_HomelessHealthEffect = -20;
    public int m_HomelessWellbeingEffect = -20;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<CitizenHappinessParameterData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<CitizenHappinessParameterData>(entity, new CitizenHappinessParameterData()
      {
        m_WaterPollutionBonusMultiplier = this.m_WaterPollutionMultiplier,
        m_PollutionBonusDivisor = this.m_PollutionDivisor,
        m_MaxAirAndGroundPollutionBonus = this.m_MaxAirAndGroundPollution,
        m_MaxNoisePollutionBonus = this.m_MaxNoisePollution,
        m_ElectricityWellbeingPenalty = this.m_ElectricityWellbeingPenalty,
        m_ElectricityPenaltyDelay = (float) this.m_ElectricityPenaltyDelay,
        m_ElectricityFeeWellbeingEffect = new AnimationCurve1(this.m_ElectricityFeeWellbeingEffect),
        m_WaterHealthPenalty = this.m_WaterHealthPenalty,
        m_WaterWellbeingPenalty = this.m_WaterWellbeingPenalty,
        m_WaterPenaltyDelay = (float) this.m_WaterPenaltyDelay,
        m_SewageHealthEffect = this.m_SewageHealthEffect,
        m_SewageWellbeingEffect = this.m_SewageWellbeingEffect,
        m_SewagePenaltyDelay = (float) this.m_SewagePenaltyDelay,
        m_WaterFeeHealthEffect = new AnimationCurve1(this.m_WaterFeeHealthEffect),
        m_WaterFeeWellbeingEffect = new AnimationCurve1(this.m_WaterFeeWellbeingEffect),
        m_WealthyMoneyAmount = this.m_WealthyMoneyAmount,
        m_HealthCareHealthMultiplier = this.m_HealthCareHealthMultiplier,
        m_HealthCareWellbeingMultiplier = this.m_HealthCareWellbeingMultiplier,
        m_EducationWellbeingMultiplier = this.m_EducationWellbeingMultiplier,
        m_NeutralEducation = this.m_NeutralEducation,
        m_EntertainmentWellbeingMultiplier = this.m_EntertainmentWellbeingMultiplier,
        m_NegligibleCrime = this.m_NegligibleCrime,
        m_CrimeMultiplier = this.m_CrimeMultiplier,
        m_MaxCrimePenalty = this.m_MaxCrimePenalty,
        m_MailMultiplier = this.m_MailMultiplier,
        m_NegligibleMail = this.m_NegligibleMail,
        m_TelecomBaseline = this.m_TelecomBaseline,
        m_TelecomBonusMultiplier = this.m_TelecomBonusMultiplier,
        m_TelecomPenaltyMultiplier = this.m_TelecomPenaltyMultiplier,
        m_WelfareMultiplier = this.m_WelfareMultiplier,
        m_HealthProblemHealthPenalty = this.m_HealthProblemHealthPenalty,
        m_DeathHealthPenalty = this.m_DeathHealthPenalty,
        m_DeathWellbeingPenalty = this.m_DeathWellbeingPenalty,
        m_ConsumptionMultiplier = this.m_ConsumptionMultiplier,
        m_LowWellbeing = this.m_LowWellbeing,
        m_LowHealth = this.m_LowHealth,
        m_TaxUneducatedMultiplier = this.m_TaxUneducatedMultiplier,
        m_TaxPoorlyEducatedMultiplier = this.m_TaxPoorlyEducatedMultiplier,
        m_TaxEducatedMultiplier = this.m_TaxEducatedMultiplier,
        m_TaxWellEducatedMultiplier = this.m_TaxWellEducatedMultiplier,
        m_TaxHighlyEducatedMultiplier = this.m_TaxHighlyEducatedMultiplier,
        m_PenaltyEffect = this.m_PenaltyEffect,
        m_HomelessHealthEffect = this.m_HomelessHealthEffect,
        m_HomelessWellbeingEffect = this.m_HomelessWellbeingEffect
      });
    }
  }
}
