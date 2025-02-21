﻿// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DemandPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class DemandPrefab : PrefabBase
  {
    public PrefabBase m_ForestryPrefab;
    public PrefabBase m_OfficePrefab;
    [Tooltip("The minimum happiness value that affect the demanding, average happiness below this won't have any different")]
    public int m_MinimumHappiness = 30;
    [Tooltip("The weight that multiply to the happiness value differ to the neutral")]
    public float m_HappinessEffect = 2f;
    [Tooltip("The weight that multiply to the tax effect value differ to the neutral 10% tax rate")]
    public float m_TaxEffect = 1f;
    [Tooltip("The weight that multiply to the student effect value")]
    public float m_StudentEffect = 1f;
    [Tooltip("The weight that multiply to the workplace value differ to the neutral")]
    public float m_AvailableWorkplaceEffect = 8f;
    [Tooltip("The weight that multiply to the homeless value differ to the neutral")]
    public float m_HomelessEffect = 20f;
    [Tooltip("Neutral average happiness value, the happiness value showed at bottom bar")]
    public int m_NeutralHappiness = 45;
    [Tooltip("Neutral unemployment percentage, 10 = 10% unemployment is neutral")]
    public float m_NeutralUnemployment = 20f;
    [Tooltip("Neutral available workplace percentage, 10 = 10% available workplace is neutral")]
    public float m_NeutralAvailableWorkplacePercentage = 10f;
    [Tooltip("Neutral homeless percentage of whole population, 2 = 2% homeless is neutral")]
    public float m_NeutralHomelessness = 2f;
    [Tooltip("Need free unoccupied residential buildings amount, x-low density, y-medium density, z-high density")]
    public int3 m_FreeResidentialRequirement = new int3(5, 10, 10);
    public float m_FreeCommercialProportion = 10f;
    public float m_FreeIndustrialProportion = 10f;
    public float m_CommercialStorageMinimum = 0.2f;
    public float m_CommercialStorageEffect = 1.6f;
    [Tooltip("The commercial resource demand multiplier to household's needs")]
    public float m_CommercialBaseDemand = 4f;
    public float m_IndustrialStorageMinimum = 0.2f;
    public float m_IndustrialStorageEffect = 1.6f;
    public float m_IndustrialBaseDemand = 7f;
    public float m_ExtractorBaseDemand = 1.5f;
    public float m_StorageDemandMultiplier = 5E-05f;
    public int m_CommuterWorkerRatioLimit = 8;
    public int m_CommuterSlowSpawnFactor = 8;
    [Tooltip("Percentage that commuter will spawn at OCs x:Road y:Train z:Air w:Ship")]
    public float4 m_CommuterOCSpawnParameters = new float4(0.8f, 0.2f, 0.0f, 0.0f);
    [Tooltip("Percentage that tourist will spawn at OCs x:Road y:Train z:Air w:Ship")]
    public float4 m_TouristOCSpawnParameters = new float4(0.1f, 0.1f, 0.5f, 0.3f);
    [Tooltip("Percentage that citizen will spawn at OCs x:Road y:Train z:Air w:Ship")]
    public float4 m_CitizenOCSpawnParameters = new float4(0.6f, 0.2f, 0.15f, 0.05f);
    [Tooltip("Percentage that teen citizen will spawn among all new household that with child")]
    public float m_TeenSpawnPercentage = 0.5f;
    [Tooltip("Frames cooldown interval for x-residential,y-commercial,z-industrial citizen/company spawning")]
    public int3 m_FrameIntervalForSpawning = new int3(0, 2000, 2000);
    [Tooltip("The speed factor of new household spawning")]
    public float m_HouseholdSpawnSpeedFactor = 0.5f;
    [Tooltip("The spawning percent requirement of hotel rooms that need to meet the demand of tourist")]
    public float m_HotelRoomPercentRequirement = 0.5f;
    [Tooltip("Percentage of citizens' education level x:uneducated y:poorly educated z:educated w:WellEducated, then rest is the highly educated")]
    public float4 m_NewCitizenEducationParameters = new float4(0.005f, 0.5f, 0.35f, 0.13f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<DemandParameterData>());
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
      entityManager.SetComponentData<DemandParameterData>(entity, new DemandParameterData()
      {
        m_ForestryPrefab = systemManaged.GetEntity(this.m_ForestryPrefab),
        m_OfficePrefab = systemManaged.GetEntity(this.m_OfficePrefab),
        m_MinimumHappiness = this.m_MinimumHappiness,
        m_HappinessEffect = this.m_HappinessEffect,
        m_AvailableWorkplaceEffect = this.m_AvailableWorkplaceEffect,
        m_HomelessEffect = this.m_HomelessEffect,
        m_NeutralHappiness = this.m_NeutralHappiness,
        m_NeutralAvailableWorkplacePercentage = this.m_NeutralAvailableWorkplacePercentage,
        m_NeutralHomelessness = this.m_NeutralHomelessness,
        m_FreeResidentialRequirement = this.m_FreeResidentialRequirement,
        m_FreeCommercialProportion = this.m_FreeCommercialProportion,
        m_FreeIndustrialProportion = this.m_FreeIndustrialProportion,
        m_CommercialStorageMinimum = this.m_CommercialStorageMinimum,
        m_CommercialStorageEffect = this.m_CommercialStorageEffect,
        m_CommercialBaseDemand = this.m_CommercialBaseDemand,
        m_IndustrialStorageMinimum = this.m_IndustrialStorageMinimum,
        m_IndustrialStorageEffect = this.m_IndustrialStorageEffect,
        m_IndustrialBaseDemand = this.m_IndustrialBaseDemand,
        m_ExtractorBaseDemand = this.m_ExtractorBaseDemand,
        m_StorageDemandMultiplier = this.m_StorageDemandMultiplier,
        m_CommuterWorkerRatioLimit = this.m_CommuterWorkerRatioLimit,
        m_CommuterSlowSpawnFactor = this.m_CommuterSlowSpawnFactor,
        m_CommuterOCSpawnParameters = this.m_CommuterOCSpawnParameters,
        m_TouristOCSpawnParameters = this.m_TouristOCSpawnParameters,
        m_CitizenOCSpawnParameters = this.m_CitizenOCSpawnParameters,
        m_TeenSpawnPercentage = this.m_TeenSpawnPercentage,
        m_FrameIntervalForSpawning = this.m_FrameIntervalForSpawning,
        m_NeutralUnemployment = this.m_NeutralUnemployment,
        m_TaxEffect = this.m_TaxEffect,
        m_StudentEffect = this.m_StudentEffect,
        m_HouseholdSpawnSpeedFactor = this.m_HouseholdSpawnSpeedFactor,
        m_NewCitizenEducationParameters = this.m_NewCitizenEducationParameters,
        m_HotelRoomPercentRequirement = this.m_HotelRoomPercentRequirement
      });
    }
  }
}
