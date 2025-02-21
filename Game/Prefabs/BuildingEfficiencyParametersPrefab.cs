// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingEfficiencyParametersPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class BuildingEfficiencyParametersPrefab : PrefabBase
  {
    [Tooltip("How the service budget (between 0.5 and 1.5) correlates the service budget efficiency factor")]
    public AnimationCurve m_ServiceBudgetEfficiencyFactor;
    [Tooltip("If the building efficiency drops below this threshold, the 'low efficiency' flag is applied, which disables certain vfx/sfx")]
    [Range(0.0f, 1f)]
    public float m_LowEfficiencyThreshold = 0.15f;
    [Header("Electricity")]
    [Tooltip("Efficiency penalty when not enough electricity is supplied")]
    [Range(0.0f, 1f)]
    public float m_ElectricityPenalty = 0.5f;
    [Tooltip("Interval in ticks until no electricity efficiency penalty is fully applied. One tick equals ~1.42 ingame minutes.")]
    [Min(1f)]
    public short m_ElectricityPenaltyDelay = 32;
    [Tooltip("Defines how the electricity fee efficiency factor correlates to the electricity fee (0-200%)")]
    public AnimationCurve m_ElectricityFeeFactor;
    [Header("Water & Sewage")]
    [Tooltip("Efficiency penalty when not enough water is supplied")]
    [Range(0.0f, 1f)]
    public float m_WaterPenalty = 0.5f;
    [Tooltip("Delay in ticks until no water efficiency penalty is fully applied. One tick equals ~1.42 ingame minutes.")]
    [Min(1f)]
    public byte m_WaterPenaltyDelay = 32;
    [Tooltip("Efficiency penalty when supplied fresh water is dirty")]
    [Range(0.0f, 1f)]
    public float m_WaterPollutionPenalty = 0.5f;
    [Tooltip("Efficiency penalty when sewage is not handled")]
    [Range(0.0f, 1f)]
    public float m_SewagePenalty = 0.5f;
    [Tooltip("Delay in ticks until no sewage efficiency penalty is fully applied. One tick equals ~1.42 ingame minutes.")]
    [Min(1f)]
    public byte m_SewagePenaltyDelay = 32;
    [Tooltip("Defines how the water fee efficiency factor correlates to the water fee (0-200%)")]
    public AnimationCurve m_WaterFeeFactor;
    [Header("Garbage")]
    [Tooltip("Efficiency penalty when garbage has accumulated")]
    [Range(0.0f, 1f)]
    public float m_GarbagePenalty = 0.5f;
    [Header("Communications")]
    [Tooltip("Amount of mail that is tolerated by the building before efficiency drops")]
    [Min(0.0f)]
    public int m_NegligibleMail = 20;
    [Tooltip("Efficiency penalty when too much mail is accumulated")]
    [Range(0.0f, 1f)]
    public float m_MailEfficiencyPenalty = 0.1f;
    [Tooltip("Minimum telecom network quality required before telecom efficiency penalty is applied")]
    [Range(0.0f, 1f)]
    public float m_TelecomBaseline = 0.3f;
    [Header("Work Provider")]
    [Tooltip("Efficiency penalty when the building has no employees (scales proportionally)")]
    [Range(0.0f, 1f)]
    public float m_MissingEmployeesEfficiencyPenalty = 0.9f;
    [Tooltip("Delay in ticks until 'not enough employees' penalty is fully applied (512 ticks per day)")]
    [Min(1f)]
    public short m_MissingEmployeesEfficiencyDelay = 16;
    [Tooltip("Extra grace period in ticks for service buildings before 'not enough employees' efficiency starts dropping (512 ticks per day)")]
    [Min(0.0f)]
    public short m_ServiceBuildingEfficiencyGracePeriod = 16;
    [Tooltip("Efficiency penalty when all employees are sick (scales proportionally)")]
    [Range(0.0f, 1f)]
    public float m_SickEmployeesEfficiencyPenalty = 0.9f;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<BuildingEfficiencyParameterData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      entityManager.SetComponentData<BuildingEfficiencyParameterData>(entity, new BuildingEfficiencyParameterData()
      {
        m_ServiceBudgetEfficiencyFactor = new AnimationCurve1(this.m_ServiceBudgetEfficiencyFactor),
        m_LowEfficiencyThreshold = this.m_LowEfficiencyThreshold,
        m_ElectricityPenalty = this.m_ElectricityPenalty,
        m_ElectricityPenaltyDelay = (float) this.m_ElectricityPenaltyDelay,
        m_ElectricityFeeFactor = new AnimationCurve1(this.m_ElectricityFeeFactor),
        m_WaterPenalty = this.m_WaterPenalty,
        m_WaterPenaltyDelay = (float) this.m_WaterPenaltyDelay,
        m_WaterPollutionPenalty = this.m_WaterPollutionPenalty,
        m_SewagePenalty = this.m_SewagePenalty,
        m_SewagePenaltyDelay = (float) this.m_SewagePenaltyDelay,
        m_WaterFeeFactor = new AnimationCurve1(this.m_WaterFeeFactor),
        m_GarbagePenalty = this.m_GarbagePenalty,
        m_NegligibleMail = this.m_NegligibleMail,
        m_MailEfficiencyPenalty = this.m_MailEfficiencyPenalty,
        m_TelecomBaseline = this.m_TelecomBaseline,
        m_MissingEmployeesEfficiencyPenalty = this.m_MissingEmployeesEfficiencyPenalty,
        m_MissingEmployeesEfficiencyDelay = (float) this.m_MissingEmployeesEfficiencyDelay,
        m_ServiceBuildingEfficiencyGracePeriod = this.m_ServiceBuildingEfficiencyGracePeriod,
        m_SickEmployeesEfficiencyPenalty = this.m_SickEmployeesEfficiencyPenalty
      });
    }
  }
}
