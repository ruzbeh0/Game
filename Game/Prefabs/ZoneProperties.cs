// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Economy;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Zones/", new Type[] {typeof (ZonePrefab)})]
  public class ZoneProperties : ComponentBase, IZoneBuildingComponent
  {
    public bool m_ScaleResidentials;
    public float m_ResidentialProperties;
    public float m_SpaceMultiplier = 1f;
    public ResourceInEditor[] m_AllowedSold;
    public ResourceInEditor[] m_AllowedManufactured;
    public ResourceInEditor[] m_AllowedStored;
    public float m_FireHazardModifier;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ZonePropertiesData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<ZonePropertiesData>(entity, new ZonePropertiesData()
      {
        m_ScaleResidentials = this.m_ScaleResidentials,
        m_ResidentialProperties = this.m_ResidentialProperties,
        m_SpaceMultiplier = this.m_SpaceMultiplier,
        m_FireHazardModifier = this.m_FireHazardModifier,
        m_AllowedSold = EconomyUtils.GetResources(this.m_AllowedSold),
        m_AllowedManufactured = EconomyUtils.GetResources(this.m_AllowedManufactured),
        m_AllowedStored = EconomyUtils.GetResources(this.m_AllowedStored)
      });
    }

    public void GetBuildingPrefabComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      components.Add(ComponentType.ReadWrite<BuildingPropertyData>());
    }

    public void InitializeBuilding(
      EntityManager entityManager,
      Entity entity,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (buildingPrefab.Has<BuildingProperties>())
        return;
      BuildingPropertyData buildingPropertyData = this.GetBuildingPropertyData(buildingPrefab, level);
      entityManager.SetComponentData<BuildingPropertyData>(entity, buildingPropertyData);
    }

    public void GetBuildingArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (buildingPrefab.Has<BuildingProperties>())
        return;
      BuildingPropertyData buildingPropertyData = this.GetBuildingPropertyData(buildingPrefab, level);
      BuildingProperties.AddArchetypeComponents(components, buildingPropertyData);
    }

    private BuildingPropertyData GetBuildingPropertyData(BuildingPrefab buildingPrefab, byte level)
    {
      float num = this.m_ScaleResidentials ? (float) (1.0 + 0.25 * (double) ((int) level - 1)) * (float) buildingPrefab.lotSize : 1f;
      return new BuildingPropertyData()
      {
        m_ResidentialProperties = (int) math.round(num * this.m_ResidentialProperties),
        m_AllowedSold = EconomyUtils.GetResources(this.m_AllowedSold),
        m_AllowedManufactured = EconomyUtils.GetResources(this.m_AllowedManufactured),
        m_AllowedStored = EconomyUtils.GetResources(this.m_AllowedStored),
        m_SpaceMultiplier = this.m_SpaceMultiplier
      };
    }
  }
}
