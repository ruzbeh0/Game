// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneServiceConsumption
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Zones/", new Type[] {typeof (ZonePrefab)})]
  public class ZoneServiceConsumption : ComponentBase, IZoneBuildingComponent
  {
    public float m_Upkeep;
    public float m_ElectricityConsumption;
    public float m_WaterConsumption;
    public float m_GarbageAccumulation;
    public float m_TelecomNeed;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ZoneServiceConsumptionData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<ZoneServiceConsumptionData>(entity, new ZoneServiceConsumptionData()
      {
        m_Upkeep = this.m_Upkeep,
        m_ElectricityConsumption = this.m_ElectricityConsumption,
        m_WaterConsumption = this.m_WaterConsumption,
        m_GarbageAccumulation = this.m_GarbageAccumulation,
        m_TelecomNeed = this.m_TelecomNeed
      });
    }

    public void GetBuildingPrefabComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      components.Add(ComponentType.ReadWrite<ConsumptionData>());
    }

    public void GetBuildingArchetypeComponents(
      HashSet<ComponentType> components,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (buildingPrefab.Has<ServiceConsumption>())
        return;
      this.GetBuildingConsumptionData().AddArchetypeComponents(components);
    }

    public void InitializeBuilding(
      EntityManager entityManager,
      Entity entity,
      BuildingPrefab buildingPrefab,
      byte level)
    {
      if (buildingPrefab.Has<ServiceConsumption>())
        return;
      entityManager.SetComponentData<ConsumptionData>(entity, this.GetBuildingConsumptionData());
    }

    private ConsumptionData GetBuildingConsumptionData()
    {
      return new ConsumptionData()
      {
        m_Upkeep = 0,
        m_ElectricityConsumption = this.m_ElectricityConsumption,
        m_WaterConsumption = this.m_WaterConsumption,
        m_GarbageAccumulation = this.m_GarbageAccumulation,
        m_TelecomNeed = this.m_TelecomNeed
      };
    }
  }
}
