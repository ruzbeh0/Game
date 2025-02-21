// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EconomyParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct EconomyParameterData : IComponentData, IQueryTypeParameter
  {
    public float m_ExtractorCompanyExportMultiplier;
    public int m_Wage0;
    public int m_Wage1;
    public int m_Wage2;
    public int m_Wage3;
    public int m_Wage4;
    public float m_CommuterWageMultiplier;
    public int m_CompanyBankruptcyLimit;
    public int m_ResidentialMinimumEarnings;
    public int m_UnemploymentBenefit;
    public int m_Pension;
    public int m_FamilyAllowance;
    public float2 m_ResourceConsumptionMultiplier;
    public float m_ResourceConsumptionPerCitizen;
    public float m_TouristConsumptionMultiplier;
    public float m_WorkDayStart;
    public float m_WorkDayEnd;
    public float m_IndustrialEfficiency;
    public float m_CommercialEfficiency;
    public float m_ExtractorProductionEfficiency;
    public float m_TrafficReduction;
    public float m_MaxCitySpecializationBonus;
    public int m_ResourceProductionCoefficient;
    public float3 m_LandValueModifier;
    public float3 m_RentPriceBuildingZoneTypeBase;
    public float m_MixedBuildingCompanyRentPercentage;
    public float m_ResidentialUpkeepLevelExponent;
    public float m_CommercialUpkeepLevelExponent;
    public float m_IndustrialUpkeepLevelExponent;
    public int m_PerOfficeResourceNeededForIndustrial;
    public float m_UnemploymentAllowanceMaxDays;
    public int m_ShopPossibilityIncreaseDivider;
    public float m_CityServiceWageAdjustment;
    public int m_PlayerStartMoney;
    public float3 m_BuildRefundPercentage;
    public float3 m_BuildRefundTimeRange;
    public float3 m_RoadRefundPercentage;
    public float3 m_RoadRefundTimeRange;
    public int3 m_TreeCostMultipliers;
    public AnimationCurve1 m_MapTileUpkeepCostMultiplier;

    public int GetWage(int jobLevel, bool cityServiceJob = false)
    {
      float num = cityServiceJob ? this.m_CityServiceWageAdjustment : 1f;
      switch (jobLevel)
      {
        case 0:
          return (int) ((double) this.m_Wage0 * (double) num);
        case 1:
          return (int) ((double) this.m_Wage1 * (double) num);
        case 2:
          return (int) ((double) this.m_Wage2 * (double) num);
        case 3:
          return (int) ((double) this.m_Wage3 * (double) num);
        case 4:
          return (int) ((double) this.m_Wage4 * (double) num);
        default:
          return 0;
      }
    }
  }
}
