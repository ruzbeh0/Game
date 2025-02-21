// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GarbagePrefab
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
  public class GarbagePrefab : PrefabBase
  {
    public ServicePrefab m_GarbageServicePrefab;
    public NotificationIconPrefab m_GarbageNotificationPrefab;
    public NotificationIconPrefab m_FacilityFullNotificationPrefab;
    [Tooltip("The garbage produce amount of homeless")]
    public int m_HomelessGarbageProduce = 25;
    public int m_CollectionGarbageLimit = 20;
    public int m_RequestGarbageLimit = 100;
    public int m_WarningGarbageLimit = 500;
    public int m_MaxGarbageAccumulation = 2000;
    public float m_BuildingLevelBalance = 1.25f;
    public float m_EducationBalance = 2.5f;
    [Tooltip("The baseline of garbage accumulated amount to affect happiness")]
    public int m_HappinessEffectBaseline = 100;
    [Tooltip("The step of garbage accumulated amount to affect happiness, e.g. baseline(100)+step(65) = 165 accumulated garbage to have -1 happiness bonus")]
    public int m_HappinessEffectStep = 65;

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_GarbageServicePrefab);
      prefabs.Add((PrefabBase) this.m_GarbageNotificationPrefab);
      prefabs.Add((PrefabBase) this.m_FacilityFullNotificationPrefab);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<GarbageParameterData>());
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
      GarbageParameterData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_GarbageServicePrefab = systemManaged.GetEntity((PrefabBase) this.m_GarbageServicePrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_GarbageNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_GarbageNotificationPrefab);
      // ISSUE: reference to a compiler-generated method
      componentData.m_FacilityFullNotificationPrefab = systemManaged.GetEntity((PrefabBase) this.m_FacilityFullNotificationPrefab);
      componentData.m_HomelessGarbageProduce = this.m_HomelessGarbageProduce;
      componentData.m_CollectionGarbageLimit = this.m_CollectionGarbageLimit;
      componentData.m_RequestGarbageLimit = this.m_RequestGarbageLimit;
      componentData.m_WarningGarbageLimit = this.m_WarningGarbageLimit;
      componentData.m_MaxGarbageAccumulation = this.m_MaxGarbageAccumulation;
      componentData.m_BuildingLevelBalance = this.m_BuildingLevelBalance;
      componentData.m_EducationBalance = this.m_EducationBalance;
      componentData.m_HappinessEffectBaseline = this.m_HappinessEffectBaseline;
      componentData.m_HappinessEffectStep = this.m_HappinessEffectStep;
      entityManager.SetComponentData<GarbageParameterData>(entity, componentData);
    }
  }
}
