// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HealthcarePrefab
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
  public class HealthcarePrefab : PrefabBase
  {
    public PrefabBase m_HealthcareServicePrefab;
    public NotificationIconPrefab m_AmbulanceNotificationPrefab;
    public NotificationIconPrefab m_HearseNotificationPrefab;
    public NotificationIconPrefab m_FacilityFullNotificationPrefab;
    [Tooltip("Healthcare transporting notification time in seconds")]
    public float m_TransportWarningTime = 15f;
    [Range(0.0f, 1f)]
    public float m_NoResourceTreatmentPenalty = 0.5f;
    public float m_BuildingDestoryDeathRate = 0.5f;
    public AnimationCurve m_DeathRate;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add(this.m_HealthcareServicePrefab);
      prefabs.Add((PrefabBase) this.m_AmbulanceNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_HearseNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_FacilityFullNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<HealthcareParameterData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<HealthcareParameterData>(entity, new HealthcareParameterData()
      {
        m_HealthcareServicePrefab = systemManaged.GetEntity(this.m_HealthcareServicePrefab),
        m_AmbulanceNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_AmbulanceNotificationPrefab),
        m_HearseNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_HearseNotificationPrefab),
        m_FacilityFullNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_FacilityFullNotificationPrefab),
        m_TransportWarningTime = this.m_TransportWarningTime,
        m_NoResourceTreatmentPenalty = this.m_NoResourceTreatmentPenalty,
        m_BuildingDestoryDeathRate = this.m_BuildingDestoryDeathRate,
        m_DeathRate = new AnimationCurve1(this.m_DeathRate)
      });
    }
  }
}
