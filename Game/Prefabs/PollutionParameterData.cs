// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PollutionParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PollutionParameterData : IComponentData, IQueryTypeParameter
  {
    public float m_GroundMultiplier;
    public float m_AirMultiplier;
    public float m_NoiseMultiplier;
    public float m_NetAirMultiplier;
    public float m_NetNoiseMultiplier;
    public float m_GroundRadius;
    public float m_AirRadius;
    public float m_NoiseRadius;
    public float m_NetNoiseRadius;
    public float m_WindAdvectionSpeed;
    public short m_AirFade;
    public short m_GroundFade;
    public float m_PlantAirMultiplier;
    public float m_PlantGroundMultiplier;
    public float m_PlantFade;
    public float m_FertilityGroundMultiplier;
    public float m_DistanceExponent;
    public Entity m_AirPollutionNotification;
    public Entity m_NoisePollutionNotification;
    public Entity m_GroundPollutionNotification;
    public int m_AirPollutionNotificationLimit;
    public int m_NoisePollutionNotificationLimit;
    public int m_GroundPollutionNotificationLimit;
    public float m_AbandonedNoisePollutionMultiplier;
    public int m_HomelessNoisePollution;
    public int m_GroundPollutionLandValueDivisor;
  }
}
