// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CompanyUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Companies;
using Game.Objects;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public static class CompanyUtils
  {
    public static int GetCompanyMoveAwayChance(
      Entity company,
      Entity companyPrefab,
      Entity property,
      ref ComponentLookup<ServiceAvailable> serviceAvailables,
      ref ComponentLookup<OfficeProperty> officeProperties,
      ref ComponentLookup<IndustrialProcessData> industrialProcessDatas,
      ref ComponentLookup<WorkProvider> workProviders,
      NativeArray<int> taxRates)
    {
      int num1 = 0;
      int num2 = serviceAvailables.HasComponent(company) ? 1 : 0;
      bool flag = officeProperties.HasComponent(property);
      IndustrialProcessData industrialProcessData = industrialProcessDatas[companyPrefab];
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      int num3 = num2 == 0 ? (!flag ? TaxSystem.GetIndustrialTaxRate(industrialProcessData.m_Output.m_Resource, taxRates) : TaxSystem.GetOfficeTaxRate(industrialProcessData.m_Output.m_Resource, taxRates)) : TaxSystem.GetCommercialTaxRate(industrialProcessData.m_Output.m_Resource, taxRates);
      int companyMoveAwayChance = num1 + (num3 - 10) * 5 / 2;
      WorkProvider workProvider = workProviders[company];
      if (workProvider.m_UneducatedNotificationEntity != Entity.Null)
        companyMoveAwayChance += 5;
      if (workProvider.m_EducatedNotificationEntity != Entity.Null)
        companyMoveAwayChance += 20;
      return companyMoveAwayChance;
    }

    public static int GetCommercialMaxFittingWorkers(
      Game.Prefabs.BuildingData building,
      BuildingPropertyData properties,
      int level,
      ServiceCompanyData serviceData)
    {
      return Mathf.CeilToInt((float) ((double) serviceData.m_MaxWorkersPerCell * (double) building.m_LotSize.x * (double) building.m_LotSize.y * (1.0 + 0.5 * (double) level)) * properties.m_SpaceMultiplier);
    }

    public static int GetIndustrialAndOfficeFittingWorkers(
      Game.Prefabs.BuildingData building,
      BuildingPropertyData properties,
      int level,
      IndustrialProcessData processData)
    {
      return Mathf.CeilToInt((float) ((double) processData.m_MaxWorkersPerCell * (double) building.m_LotSize.x * (double) building.m_LotSize.y * (1.0 + 0.5 * (double) level)) * properties.m_SpaceMultiplier);
    }

    public static int GetExtractorFittingWorkers(
      float area,
      float spaceMultiplier,
      IndustrialProcessData processData)
    {
      return Mathf.CeilToInt(processData.m_MaxWorkersPerCell * area * spaceMultiplier);
    }

    public static int GetCompanyMaxFittingWorkers(
      Entity companyEntity,
      Entity buildingEntity,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<ServiceCompanyData> serviceCompanyDatas,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas,
      ref ComponentLookup<SpawnableBuildingData> spawnableBuildingDatas,
      ref ComponentLookup<IndustrialProcessData> industrialProcessDatas,
      ref ComponentLookup<ExtractorCompanyData> extractorCompanyDatas,
      ref ComponentLookup<Attached> attacheds,
      ref BufferLookup<Game.Areas.SubArea> subAreaBufs,
      ref ComponentLookup<Game.Areas.Lot> lots,
      ref ComponentLookup<Geometry> geometries)
    {
      Entity entity1 = (Entity) prefabRefs[companyEntity];
      Entity entity2 = (Entity) prefabRefs[buildingEntity];
      int level = 1;
      if (spawnableBuildingDatas.HasComponent(entity2))
        level = (int) spawnableBuildingDatas[entity2].m_Level;
      if (serviceCompanyDatas.HasComponent(entity1))
        return CompanyUtils.GetCommercialMaxFittingWorkers(buildingDatas[entity2], buildingPropertyDatas[entity2], level, serviceCompanyDatas[entity1]);
      if (extractorCompanyDatas.HasComponent(entity1))
      {
        float area = 0.0f;
        if (attacheds.HasComponent(buildingEntity))
        {
          Attached attached = attacheds[buildingEntity];
          // ISSUE: reference to a compiler-generated method
          area = ExtractorAISystem.GetArea(subAreaBufs[attached.m_Parent], lots, geometries);
        }
        return math.max(1, CompanyUtils.GetExtractorFittingWorkers(area, 1f, industrialProcessDatas[entity1]) / 2);
      }
      return industrialProcessDatas.HasComponent(entity1) ? CompanyUtils.GetIndustrialAndOfficeFittingWorkers(buildingDatas[entity2], buildingPropertyDatas[entity2], level, industrialProcessDatas[entity1]) : 0;
    }
  }
}
