// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingConfigurationPrefab
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
  public class BuildingConfigurationPrefab : PrefabBase
  {
    [Tooltip("The building condition increase when building received enough upkeep cost, change 16 times per game day")]
    public int m_BuildingConditionIncrement = 30;
    [Tooltip("The building condition decrease when building can't pay enough upkeep cost, change 16 times per game day")]
    public int m_BuildingConditionDecrement = 1;
    public NotificationIconPrefab m_AbandonedCollapsedNotification;
    public NotificationIconPrefab m_AbandonedNotification;
    public NotificationIconPrefab m_CondemnedNotification;
    public NotificationIconPrefab m_LevelUpNotification;
    public NotificationIconPrefab m_TurnedOffNotification;
    public NetLanePrefab m_ElectricityConnectionLane;
    public NetLanePrefab m_SewageConnectionLane;
    public NetLanePrefab m_WaterConnectionLane;
    public uint m_AbandonedDestroyDelay;
    public NotificationIconPrefab m_HighRentNotification;
    public BrandPrefab m_DefaultRenterBrand;
    public AreaPrefab m_ConstructionSurface;
    public NetLanePrefab m_ConstructionBorder;
    public ObjectPrefab m_ConstructionObject;
    public ObjectPrefab m_CollapsedObject;
    public EffectPrefab m_CollapseVFX;
    public EffectPrefab m_CollapseSFX;
    public AreaPrefab m_CollapsedSurface;
    public EffectPrefab m_FireLoopSFX;
    public EffectPrefab m_FireSpotSFX;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_AbandonedCollapsedNotification);
      prefabs.Add((PrefabBase) this.m_AbandonedNotification);
      prefabs.Add((PrefabBase) this.m_CondemnedNotification);
      prefabs.Add((PrefabBase) this.m_LevelUpNotification);
      prefabs.Add((PrefabBase) this.m_TurnedOffNotification);
      prefabs.Add((PrefabBase) this.m_ElectricityConnectionLane);
      prefabs.Add((PrefabBase) this.m_SewageConnectionLane);
      prefabs.Add((PrefabBase) this.m_WaterConnectionLane);
      prefabs.Add((PrefabBase) this.m_HighRentNotification);
      prefabs.Add((PrefabBase) this.m_DefaultRenterBrand);
      prefabs.Add((PrefabBase) this.m_ConstructionSurface);
      prefabs.Add((PrefabBase) this.m_ConstructionBorder);
      prefabs.Add((PrefabBase) this.m_ConstructionObject);
      prefabs.Add((PrefabBase) this.m_CollapsedObject);
      prefabs.Add((PrefabBase) this.m_CollapseVFX);
      prefabs.Add((PrefabBase) this.m_CollapseSFX);
      prefabs.Add((PrefabBase) this.m_CollapsedSurface);
      prefabs.Add((PrefabBase) this.m_FireLoopSFX);
      prefabs.Add((PrefabBase) this.m_FireSpotSFX);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<BuildingConfigurationData>());
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      entityManager.SetComponentData<BuildingConfigurationData>(entity, new BuildingConfigurationData()
      {
        m_BuildingConditionIncrement = this.m_BuildingConditionIncrement,
        m_BuildingConditionDecrement = this.m_BuildingConditionDecrement,
        m_AbandonedCollapsedNotification = systemManaged.GetEntity((PrefabBase) this.m_AbandonedCollapsedNotification),
        m_AbandonedNotification = systemManaged.GetEntity((PrefabBase) this.m_AbandonedNotification),
        m_CondemnedNotification = systemManaged.GetEntity((PrefabBase) this.m_CondemnedNotification),
        m_LevelUpNotification = systemManaged.GetEntity((PrefabBase) this.m_LevelUpNotification),
        m_TurnedOffNotification = systemManaged.GetEntity((PrefabBase) this.m_TurnedOffNotification),
        m_ElectricityConnectionLane = systemManaged.GetEntity((PrefabBase) this.m_ElectricityConnectionLane),
        m_SewageConnectionLane = systemManaged.GetEntity((PrefabBase) this.m_SewageConnectionLane),
        m_WaterConnectionLane = systemManaged.GetEntity((PrefabBase) this.m_WaterConnectionLane),
        m_AbandonedDestroyDelay = this.m_AbandonedDestroyDelay,
        m_HighRentNotification = systemManaged.GetEntity((PrefabBase) this.m_HighRentNotification),
        m_DefaultRenterBrand = systemManaged.GetEntity((PrefabBase) this.m_DefaultRenterBrand),
        m_ConstructionSurface = systemManaged.GetEntity((PrefabBase) this.m_ConstructionSurface),
        m_ConstructionBorder = systemManaged.GetEntity((PrefabBase) this.m_ConstructionBorder),
        m_ConstructionObject = systemManaged.GetEntity((PrefabBase) this.m_ConstructionObject),
        m_CollapsedObject = systemManaged.GetEntity((PrefabBase) this.m_CollapsedObject),
        m_CollapseVFX = systemManaged.GetEntity((PrefabBase) this.m_CollapseVFX),
        m_CollapseSFX = systemManaged.GetEntity((PrefabBase) this.m_CollapseSFX),
        m_CollapsedSurface = systemManaged.GetEntity((PrefabBase) this.m_CollapsedSurface),
        m_FireLoopSFX = systemManaged.GetEntity((PrefabBase) this.m_FireLoopSFX),
        m_FireSpotSFX = systemManaged.GetEntity((PrefabBase) this.m_FireSpotSFX)
      });
    }
  }
}
