// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PoliceConfigurationPrefab
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
  public class PoliceConfigurationPrefab : PrefabBase
  {
    public PrefabBase m_PoliceServicePrefab;
    public NotificationIconPrefab m_TrafficAccidentNotificationPrefab;
    public NotificationIconPrefab m_CrimeSceneNotificationPrefab;
    public float m_MaxCrimeAccumulation = 100000f;
    public float m_CrimeAccumulationTolerance = 1000f;
    public int m_HomeCrimeEffect = 15;
    public int m_WorkplaceCrimeEffect = 5;
    public float m_WelfareCrimeRecurrenceFactor = 0.4f;
    public float m_CrimeIncreaseMultiplier = 2f;
    [Tooltip("Reduce the crime possibility according to the population, the possibility random will be calculated by population / CrimePopulationReduction * 100, and not less than 100")]
    public float m_CrimePopulationReduction = 2000f;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add(this.m_PoliceServicePrefab);
      prefabs.Add((PrefabBase) this.m_TrafficAccidentNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_CrimeSceneNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<PoliceConfigurationData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      PoliceConfigurationData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_PoliceServicePrefab = systemManaged.GetEntity(this.m_PoliceServicePrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TrafficAccidentNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_TrafficAccidentNotificationPrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_CrimeSceneNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_CrimeSceneNotificationPrefab);
      componentData.m_MaxCrimeAccumulation = this.m_MaxCrimeAccumulation;
      componentData.m_CrimeAccumulationTolerance = this.m_CrimeAccumulationTolerance;
      componentData.m_HomeCrimeEffect = this.m_HomeCrimeEffect;
      componentData.m_WorkplaceCrimeEffect = this.m_WorkplaceCrimeEffect;
      componentData.m_WelfareCrimeRecurrenceFactor = this.m_WelfareCrimeRecurrenceFactor;
      componentData.m_CrimeIncreaseMultiplier = this.m_CrimeIncreaseMultiplier;
      componentData.m_CrimePopulationReduction = this.m_CrimePopulationReduction;
      entityManager.SetComponentData<PoliceConfigurationData>(entity, componentData);
    }
  }
}
