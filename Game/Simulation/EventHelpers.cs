// Decompiled with JetBrains decompiler
// Type: Game.Simulation.EventHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class EventHelpers
  {
    public struct FireHazardData
    {
      public LocalEffectSystem.ReadData m_LocalEffectData;
      public float m_ForestFireHazardFactor;
      public ComponentLookup<DestructibleObjectData> m_PrefabDestructibleObjectData;
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      public ComponentLookup<ZonePropertiesData> m_ZonePropertiesData;
      public ComponentLookup<PlaceholderBuildingData> m_PlaceholderBuildingData;

      public FireHazardData(
        SystemBase system,
        LocalEffectSystem.ReadData localEffectData,
        FireConfigurationPrefab fireConfigurationPrefab,
        float temperature,
        float noRainDays)
      {
        this.m_LocalEffectData = localEffectData;
        this.m_PrefabDestructibleObjectData = system.GetComponentLookup<DestructibleObjectData>(true);
        this.m_PrefabSpawnableBuildingData = system.GetComponentLookup<SpawnableBuildingData>(true);
        this.m_ServiceCoverages = system.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        this.m_DistrictModifiers = system.GetBufferLookup<DistrictModifier>(true);
        this.m_ZonePropertiesData = system.GetComponentLookup<ZonePropertiesData>(true);
        this.m_PlaceholderBuildingData = system.GetComponentLookup<PlaceholderBuildingData>(true);
        this.m_ForestFireHazardFactor = fireConfigurationPrefab.m_TemperatureForestFireHazard.Evaluate(temperature);
        this.m_ForestFireHazardFactor *= fireConfigurationPrefab.m_NoRainForestFireHazard.Evaluate(noRainDays);
      }

      public bool GetFireHazard(
        PrefabRef prefabRef,
        Building building,
        CurrentDistrict currentDistrict,
        Damaged damaged,
        UnderConstruction underConstruction,
        out float fireHazard,
        out float riskFactor)
      {
        fireHazard = this.GetFireHazard(prefabRef);
        fireHazard = math.select(fireHazard, 0.0f, underConstruction.m_NewPrefab == Entity.Null && underConstruction.m_Progress < byte.MaxValue);
        riskFactor = 0.0f;
        if ((double) fireHazard == 0.0)
          return false;
        if (this.m_PrefabSpawnableBuildingData.HasComponent(prefabRef.m_Prefab))
        {
          SpawnableBuildingData spawnableBuildingData = this.m_PrefabSpawnableBuildingData[prefabRef.m_Prefab];
          fireHazard *= (float) (1.0 - (double) ((int) spawnableBuildingData.m_Level - 1) * 0.029999999329447746);
        }
        float num = 0.0f;
        if (this.m_ServiceCoverages.HasBuffer(building.m_RoadEdge))
        {
          num = NetUtils.GetServiceCoverage(this.m_ServiceCoverages[building.m_RoadEdge], CoverageService.FireRescue, building.m_CurvePosition);
          fireHazard *= math.max(0.01f, (float) (1.0 - (double) num * 0.0099999997764825821));
        }
        if (this.m_DistrictModifiers.HasBuffer(currentDistrict.m_District))
        {
          DynamicBuffer<DistrictModifier> districtModifier = this.m_DistrictModifiers[currentDistrict.m_District];
          AreaUtils.ApplyModifier(ref fireHazard, districtModifier, DistrictModifierType.BuildingFireHazard);
        }
        if (this.m_PlaceholderBuildingData.HasComponent(prefabRef.m_Prefab) && this.m_ZonePropertiesData.HasComponent(this.m_PlaceholderBuildingData[prefabRef.m_Prefab].m_ZonePrefab))
        {
          ZonePropertiesData zonePropertiesData = this.m_ZonePropertiesData[this.m_PlaceholderBuildingData[prefabRef.m_Prefab].m_ZonePrefab];
          fireHazard *= zonePropertiesData.m_FireHazardModifier;
        }
        riskFactor = fireHazard / (float) (1.0 + (double) num * 0.5);
        fireHazard *= this.GetFireHazardFactor(damaged);
        return true;
      }

      public bool GetFireHazard(
        PrefabRef prefabRef,
        Tree tree,
        Transform transform,
        Damaged damaged,
        out float fireHazard,
        out float riskFactor)
      {
        fireHazard = this.GetFireHazard(prefabRef);
        riskFactor = 0.0f;
        if ((double) fireHazard == 0.0)
          return false;
        fireHazard *= this.m_ForestFireHazardFactor;
        // ISSUE: reference to a compiler-generated method
        this.m_LocalEffectData.ApplyModifier(ref fireHazard, transform.m_Position, LocalModifierType.ForestFireHazard);
        riskFactor = fireHazard;
        // ISSUE: reference to a compiler-generated method
        this.m_LocalEffectData.ApplyModifier(ref riskFactor, transform.m_Position, LocalModifierType.ForestFireResponseTime);
        fireHazard *= this.GetFireHazardFactor(damaged);
        return true;
      }

      private float GetFireHazard(PrefabRef prefabRef)
      {
        return !this.m_PrefabDestructibleObjectData.HasComponent(prefabRef.m_Prefab) ? 100f : this.m_PrefabDestructibleObjectData[prefabRef.m_Prefab].m_FireHazard;
      }

      private float GetFireHazardFactor(Damaged damaged)
      {
        double num1 = (double) math.max(0.0f, 1f - math.csum(damaged.m_Damage.yz));
        double num2 = num1 * num1;
        return (float) (num2 * num2);
      }
    }

    public struct StructuralIntegrityData
    {
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      public ComponentLookup<DestructibleObjectData> m_PrefabDestructibleObjectData;
      public FireConfigurationData m_FireConfigurationData;

      public StructuralIntegrityData(SystemBase system, FireConfigurationData fireConfigurationData)
      {
        this.m_PrefabSpawnableBuildingData = system.GetComponentLookup<SpawnableBuildingData>(true);
        this.m_PrefabDestructibleObjectData = system.GetComponentLookup<DestructibleObjectData>(true);
        this.m_FireConfigurationData = fireConfigurationData;
      }

      public float GetStructuralIntegrity(Entity prefab, bool isBuilding)
      {
        if (this.m_PrefabDestructibleObjectData.HasComponent(prefab))
          return this.m_PrefabDestructibleObjectData[prefab].m_StructuralIntegrity;
        if (!isBuilding)
          return this.m_FireConfigurationData.m_DefaultStructuralIntegrity;
        if (!this.m_PrefabSpawnableBuildingData.HasComponent(prefab))
          return this.m_FireConfigurationData.m_BuildingStructuralIntegrity;
        switch (this.m_PrefabSpawnableBuildingData[prefab].m_Level)
        {
          case 1:
            return this.m_FireConfigurationData.m_StructuralIntegrityLevel1;
          case 2:
            return this.m_FireConfigurationData.m_StructuralIntegrityLevel2;
          case 3:
            return this.m_FireConfigurationData.m_StructuralIntegrityLevel3;
          case 4:
            return this.m_FireConfigurationData.m_StructuralIntegrityLevel4;
          case 5:
            return this.m_FireConfigurationData.m_StructuralIntegrityLevel5;
          default:
            return this.m_FireConfigurationData.m_BuildingStructuralIntegrity;
        }
      }
    }
  }
}
