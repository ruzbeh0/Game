// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PollutionPrefab
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
  public class PollutionPrefab : PrefabBase
  {
    public float m_GroundMultiplier = 25f;
    public float m_AirMultiplier = 25f;
    public float m_NoiseMultiplier = 100f;
    public float m_NetAirMultiplier = 25f;
    public float m_NetNoiseMultiplier = 100f;
    public float m_GroundRadius = 150f;
    public float m_AirRadius = 75f;
    public float m_NoiseRadius = 200f;
    public float m_NetNoiseRadius = 50f;
    public float m_WindAdvectionSpeed = 8f;
    public short m_AirFade = 5;
    public short m_GroundFade = 10;
    public float m_PlantAirMultiplier = 1f / 1000f;
    public float m_PlantGroundMultiplier = 1f / 1000f;
    public float m_PlantFade = 2f;
    public float m_FertilityGroundMultiplier = 1f;
    public float m_DistanceExponent = 2f;
    public NotificationIconPrefab m_AirPollutionNotification;
    public NotificationIconPrefab m_NoisePollutionNotification;
    public NotificationIconPrefab m_GroundPollutionNotification;
    [Tooltip("If happiness effect from air pollution is less than this, show notification")]
    public int m_AirPollutionNotificationLimit = -5;
    [Tooltip("If happiness effect from noise pollution is less than this, show notification")]
    public int m_NoisePollutionNotificationLimit = -5;
    [Tooltip("If happiness effect from ground pollution is less than this, show notification")]
    public int m_GroundPollutionNotificationLimit = -5;
    public float m_AbandonedNoisePollutionMultiplier = 5f;
    public int m_HomelessNoisePollution = 100;
    [Tooltip("The divisor is that pollution value divide this will be the negative affect to land value")]
    public int m_GroundPollutionLandValueDivisor = 500;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<PollutionParameterData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<PollutionParameterData>(entity, new PollutionParameterData()
      {
        m_GroundMultiplier = this.m_GroundMultiplier,
        m_AirMultiplier = this.m_AirMultiplier,
        m_NoiseMultiplier = this.m_NoiseMultiplier,
        m_NetAirMultiplier = this.m_NetAirMultiplier,
        m_NetNoiseMultiplier = this.m_NetNoiseMultiplier,
        m_GroundRadius = this.m_GroundRadius,
        m_AirRadius = this.m_AirRadius,
        m_NoiseRadius = this.m_NoiseRadius,
        m_NetNoiseRadius = this.m_NetNoiseRadius,
        m_WindAdvectionSpeed = this.m_WindAdvectionSpeed,
        m_AirFade = this.m_AirFade,
        m_GroundFade = this.m_GroundFade,
        m_PlantAirMultiplier = this.m_PlantAirMultiplier,
        m_PlantGroundMultiplier = this.m_PlantGroundMultiplier,
        m_PlantFade = this.m_PlantFade,
        m_FertilityGroundMultiplier = this.m_FertilityGroundMultiplier,
        m_DistanceExponent = this.m_DistanceExponent,
        m_AirPollutionNotification = systemManaged.GetEntity((PrefabBase) this.m_AirPollutionNotification),
        m_NoisePollutionNotification = systemManaged.GetEntity((PrefabBase) this.m_NoisePollutionNotification),
        m_GroundPollutionNotification = systemManaged.GetEntity((PrefabBase) this.m_GroundPollutionNotification),
        m_AirPollutionNotificationLimit = this.m_AirPollutionNotificationLimit,
        m_NoisePollutionNotificationLimit = this.m_NoisePollutionNotificationLimit,
        m_GroundPollutionNotificationLimit = this.m_GroundPollutionNotificationLimit,
        m_AbandonedNoisePollutionMultiplier = this.m_AbandonedNoisePollutionMultiplier,
        m_HomelessNoisePollution = this.m_HomelessNoisePollution,
        m_GroundPollutionLandValueDivisor = this.m_GroundPollutionLandValueDivisor
      });
    }
  }
}
