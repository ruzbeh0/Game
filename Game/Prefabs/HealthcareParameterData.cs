// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HealthcareParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct HealthcareParameterData : IComponentData, IQueryTypeParameter
  {
    public Entity m_HealthcareServicePrefab;
    public Entity m_AmbulanceNotificationPrefab;
    public Entity m_HearseNotificationPrefab;
    public Entity m_FacilityFullNotificationPrefab;
    public float m_TransportWarningTime;
    public float m_NoResourceTreatmentPenalty;
    public float m_BuildingDestoryDeathRate;
    public AnimationCurve1 m_DeathRate;
  }
}
