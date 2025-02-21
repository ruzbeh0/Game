// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Economy;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/", new Type[] {typeof (BuildingPrefab)})]
  public class BuildingProperties : ComponentBase
  {
    public int m_ResidentialProperties;
    public ResourceInEditor[] m_AllowedSold;
    public ResourceInEditor[] m_AllowedManufactured;
    public ResourceInEditor[] m_AllowedStored;
    public float m_SpaceMultiplier;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<BuildingPropertyData>());
      if (EconomyUtils.GetResources(this.m_AllowedStored) == Resource.NoResource)
        return;
      components.Add(ComponentType.ReadWrite<WarehouseData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      if (this.prefab.Has<PlaceholderBuilding>())
        return;
      BuildingProperties.AddArchetypeComponents(components, this.GetPropertyData());
    }

    public static void AddArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPropertyData propertyData)
    {
      components.Add(ComponentType.ReadWrite<Renter>());
      if (propertyData.m_ResidentialProperties > 0)
      {
        components.Add(ComponentType.ReadWrite<ResidentialProperty>());
        components.Add(ComponentType.ReadWrite<BuildingNotifications>());
        components.Add(ComponentType.ReadWrite<PropertyToBeOnMarket>());
      }
      if (propertyData.m_AllowedSold != Resource.NoResource)
      {
        components.Add(ComponentType.ReadWrite<CommercialProperty>());
        components.Add(ComponentType.ReadWrite<PropertyToBeOnMarket>());
        components.Add(ComponentType.ReadWrite<Efficiency>());
      }
      if (propertyData.m_AllowedManufactured != Resource.NoResource)
      {
        components.Add(ComponentType.ReadWrite<IndustrialProperty>());
        components.Add(ComponentType.ReadWrite<PropertyToBeOnMarket>());
        components.Add(ComponentType.ReadWrite<Efficiency>());
        if (EconomyUtils.IsExtractorResource(propertyData.m_AllowedManufactured))
          components.Add(ComponentType.ReadWrite<ExtractorProperty>());
        if (EconomyUtils.IsOfficeResource(propertyData.m_AllowedManufactured))
          components.Add(ComponentType.ReadWrite<OfficeProperty>());
      }
      if (propertyData.m_AllowedStored == Resource.NoResource)
        return;
      components.Add(ComponentType.ReadWrite<IndustrialProperty>());
      components.Add(ComponentType.ReadWrite<StorageProperty>());
      components.Add(ComponentType.ReadWrite<PropertyToBeOnMarket>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      entityManager.SetComponentData<BuildingPropertyData>(entity, this.GetPropertyData());
    }

    public BuildingPropertyData GetPropertyData()
    {
      return new BuildingPropertyData()
      {
        m_ResidentialProperties = this.m_ResidentialProperties,
        m_SpaceMultiplier = this.m_SpaceMultiplier,
        m_AllowedSold = EconomyUtils.GetResources(this.m_AllowedSold),
        m_AllowedManufactured = EconomyUtils.GetResources(this.m_AllowedManufactured),
        m_AllowedStored = EconomyUtils.GetResources(this.m_AllowedStored)
      };
    }
  }
}
