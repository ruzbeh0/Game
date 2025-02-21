// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DisasterConfigurationPrefab
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
  public class DisasterConfigurationPrefab : PrefabBase
  {
    public NotificationIconPrefab m_WeatherDamageNotificationPrefab;
    public NotificationIconPrefab m_WeatherDestroyedNotificationPrefab;
    public NotificationIconPrefab m_WaterDamageNotificationPrefab;
    public NotificationIconPrefab m_WaterDestroyedNotificationPrefab;
    public NotificationIconPrefab m_DestroyedNotificationPrefab;
    public float m_FloodDamageRate = 200f;
    [Tooltip("Correlation between the general danger level (0.0-1.0) in the city and the probability that the cim will exit the shelter if there is no imminent danger to their home, workplace or school (1024 rolls per day).\nThe y value at 0.0 determines how quickly cims will leave the shelter when there is no danger.")]
    public AnimationCurve m_EmergencyShelterDangerLevelExitProbability;
    [Tooltip("Probability that a cim will exit an inoperable emergency shelter (1024 rolls per day)")]
    [Range(0.0f, 1f)]
    public float m_InoperableEmergencyShelterExitProbability = 0.1f;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_WeatherDamageNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_WeatherDestroyedNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_WaterDamageNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_WaterDestroyedNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_DestroyedNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<DisasterConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<DisasterConfigurationData>(entity, new DisasterConfigurationData()
      {
        m_WeatherDamageNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_WeatherDamageNotificationPrefab),
        m_WeatherDestroyedNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_WeatherDestroyedNotificationPrefab),
        m_WaterDamageNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_WaterDamageNotificationPrefab),
        m_WaterDestroyedNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_WaterDestroyedNotificationPrefab),
        m_DestroyedNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_DestroyedNotificationPrefab),
        m_FloodDamageRate = this.m_FloodDamageRate,
        m_EmergencyShelterDangerLevelExitProbability = new AnimationCurve1(this.m_EmergencyShelterDangerLevelExitProbability),
        m_InoperableEmergencyShelterExitProbability = this.m_InoperableEmergencyShelterExitProbability
      });
    }
  }
}
