// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FireConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class FireConfigurationPrefab : PrefabBase
  {
    public NotificationIconPrefab m_FireNotificationPrefab;
    public NotificationIconPrefab m_BurnedDownNotificationPrefab;
    public float m_DefaultStructuralIntegrity = 3000f;
    public float m_BuildingStructuralIntegrity = 15000f;
    public float m_StructuralIntegrityLevel1 = 12000f;
    public float m_StructuralIntegrityLevel2 = 13000f;
    public float m_StructuralIntegrityLevel3 = 14000f;
    public float m_StructuralIntegrityLevel4 = 15000f;
    public float m_StructuralIntegrityLevel5 = 16000f;
    public Bounds1 m_ResponseTimeRange = new Bounds1(3f, 30f);
    public float m_TelecomResponseTimeModifier = -0.15f;
    public float m_DarknessResponseTimeModifier = 0.1f;
    public AnimationCurve m_TemperatureForestFireHazard;
    public AnimationCurve m_NoRainForestFireHazard;
    public float m_DeathRateOfFireAccident = 0.01f;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_FireNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_BurnedDownNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<FireConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      FireConfigurationData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_FireNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_FireNotificationPrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_BurnedDownNotificationPrefab = existingSystemManaged.GetEntity((PrefabBase) this.m_BurnedDownNotificationPrefab);
      componentData.m_DefaultStructuralIntegrity = this.m_DefaultStructuralIntegrity;
      componentData.m_BuildingStructuralIntegrity = this.m_BuildingStructuralIntegrity;
      componentData.m_StructuralIntegrityLevel1 = this.m_StructuralIntegrityLevel1;
      componentData.m_StructuralIntegrityLevel2 = this.m_StructuralIntegrityLevel2;
      componentData.m_StructuralIntegrityLevel3 = this.m_StructuralIntegrityLevel3;
      componentData.m_StructuralIntegrityLevel4 = this.m_StructuralIntegrityLevel4;
      componentData.m_StructuralIntegrityLevel5 = this.m_StructuralIntegrityLevel5;
      componentData.m_ResponseTimeRange = this.m_ResponseTimeRange;
      componentData.m_TelecomResponseTimeModifier = this.m_TelecomResponseTimeModifier;
      componentData.m_DarknessResponseTimeModifier = this.m_DarknessResponseTimeModifier;
      componentData.m_DeathRateOfFireAccident = this.m_DeathRateOfFireAccident;
      entityManager.SetComponentData<FireConfigurationData>(entity, componentData);
    }
  }
}
