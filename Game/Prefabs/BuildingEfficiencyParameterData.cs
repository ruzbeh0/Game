// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingEfficiencyParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct BuildingEfficiencyParameterData : IComponentData, IQueryTypeParameter
  {
    public AnimationCurve1 m_ServiceBudgetEfficiencyFactor;
    public float m_LowEfficiencyThreshold;
    public float m_ElectricityPenalty;
    public float m_ElectricityPenaltyDelay;
    public AnimationCurve1 m_ElectricityFeeFactor;
    public float m_WaterPenalty;
    public float m_WaterPenaltyDelay;
    public float m_WaterPollutionPenalty;
    public float m_SewagePenalty;
    public float m_SewagePenaltyDelay;
    public AnimationCurve1 m_WaterFeeFactor;
    public float m_GarbagePenalty;
    public int m_NegligibleMail;
    public float m_MailEfficiencyPenalty;
    public float m_TelecomBaseline;
    public float m_MissingEmployeesEfficiencyPenalty;
    public float m_MissingEmployeesEfficiencyDelay;
    public short m_ServiceBuildingEfficiencyGracePeriod;
    public float m_SickEmployeesEfficiencyPenalty;
  }
}
