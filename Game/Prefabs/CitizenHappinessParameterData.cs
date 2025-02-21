﻿// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CitizenHappinessParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct CitizenHappinessParameterData : IComponentData, IQueryTypeParameter
  {
    public int m_PollutionBonusDivisor;
    public int m_MaxAirAndGroundPollutionBonus;
    public int m_MaxNoisePollutionBonus;
    public float m_ElectricityWellbeingPenalty;
    public float m_ElectricityPenaltyDelay;
    public AnimationCurve1 m_ElectricityFeeWellbeingEffect;
    public int m_WaterHealthPenalty;
    public int m_WaterWellbeingPenalty;
    public float m_WaterPenaltyDelay;
    public float m_WaterPollutionBonusMultiplier;
    public int m_SewageHealthEffect;
    public int m_SewageWellbeingEffect;
    public float m_SewagePenaltyDelay;
    public AnimationCurve1 m_WaterFeeHealthEffect;
    public AnimationCurve1 m_WaterFeeWellbeingEffect;
    public int4 m_WealthyMoneyAmount;
    public float m_HealthCareHealthMultiplier;
    public float m_HealthCareWellbeingMultiplier;
    public float m_EducationWellbeingMultiplier;
    public float m_NeutralEducation;
    public float m_EntertainmentWellbeingMultiplier;
    public int m_NegligibleCrime;
    public float m_CrimeMultiplier;
    public int m_MaxCrimePenalty;
    public float m_MailMultiplier;
    public int m_NegligibleMail;
    public float m_TelecomBaseline;
    public float m_TelecomBonusMultiplier;
    public float m_TelecomPenaltyMultiplier;
    public float m_WelfareMultiplier;
    public int m_HealthProblemHealthPenalty;
    public int m_DeathWellbeingPenalty;
    public int m_DeathHealthPenalty;
    public float m_ConsumptionMultiplier;
    public int m_LowWellbeing;
    public int m_LowHealth;
    public float m_TaxUneducatedMultiplier;
    public float m_TaxPoorlyEducatedMultiplier;
    public float m_TaxEducatedMultiplier;
    public float m_TaxWellEducatedMultiplier;
    public float m_TaxHighlyEducatedMultiplier;
    public int m_PenaltyEffect;
    public int m_HomelessHealthEffect;
    public int m_HomelessWellbeingEffect;
  }
}
