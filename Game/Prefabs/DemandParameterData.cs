// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DemandParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct DemandParameterData : IComponentData, IQueryTypeParameter
  {
    public Entity m_ForestryPrefab;
    public Entity m_OfficePrefab;
    public int m_MinimumHappiness;
    public float m_HappinessEffect;
    public float m_TaxEffect;
    public float m_StudentEffect;
    public float m_AvailableWorkplaceEffect;
    public float m_HomelessEffect;
    public int m_NeutralHappiness;
    public float m_NeutralUnemployment;
    public float m_NeutralAvailableWorkplacePercentage;
    public float m_NeutralHomelessness;
    public int3 m_FreeResidentialRequirement;
    public float m_FreeCommercialProportion;
    public float m_FreeIndustrialProportion;
    public float m_CommercialStorageMinimum;
    public float m_CommercialStorageEffect;
    public float m_CommercialBaseDemand;
    public float m_IndustrialStorageMinimum;
    public float m_IndustrialStorageEffect;
    public float m_IndustrialBaseDemand;
    public float m_ExtractorBaseDemand;
    public float m_StorageDemandMultiplier;
    public int m_CommuterWorkerRatioLimit;
    public int m_CommuterSlowSpawnFactor;
    public float4 m_CommuterOCSpawnParameters;
    public float4 m_TouristOCSpawnParameters;
    public float4 m_CitizenOCSpawnParameters;
    public float m_TeenSpawnPercentage;
    public int3 m_FrameIntervalForSpawning;
    public float m_HouseholdSpawnSpeedFactor;
    public float m_HotelRoomPercentRequirement;
    public float4 m_NewCitizenEducationParameters;
  }
}
