// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.BuildingHappiness
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  public static class BuildingHappiness
  {
    public static void GetResidentialBuildingHappinessFactors(
      Entity city,
      NativeArray<int> taxRates,
      Entity property,
      NativeArray<int2> factors,
      ref ComponentLookup<PrefabRef> prefabs,
      ref ComponentLookup<SpawnableBuildingData> spawnableBuildings,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas,
      ref BufferLookup<CityModifier> cityModifiers,
      ref ComponentLookup<Building> buildings,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      ref BufferLookup<Game.Net.ServiceCoverage> serviceCoverages,
      ref ComponentLookup<Locked> locked,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      ref ComponentLookup<GarbageProducer> garbageProducers,
      ref ComponentLookup<CrimeProducer> crimeProducers,
      ref ComponentLookup<MailProducer> mailProducers,
      ref BufferLookup<Renter> renters,
      ref ComponentLookup<Citizen> citizenDatas,
      ref BufferLookup<HouseholdCitizen> householdCitizens,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      CitizenHappinessParameterData citizenHappinessParameters,
      GarbageParameterData garbageParameters,
      HealthcareParameterData healthcareParameters,
      ParkParameterData parkParameters,
      EducationParameterData educationParameters,
      TelecomParameterData telecomParameters,
      DynamicBuffer<HappinessFactorParameterData> happinessFactorParameters,
      NativeArray<GroundPollution> pollutionMap,
      NativeArray<NoisePollution> noisePollutionMap,
      NativeArray<AirPollution> airPollutionMap,
      CellMapData<TelecomCoverage> telecomCoverage,
      float relativeElectricityFee,
      float relativeWaterFee)
    {
      if (!prefabs.HasComponent(property))
        return;
      Entity prefab = prefabs[property].m_Prefab;
      if (!spawnableBuildings.HasComponent(prefab) || !buildingDatas.HasComponent(prefab))
        return;
      BuildingPropertyData buildingPropertyData = buildingPropertyDatas[prefab];
      DynamicBuffer<CityModifier> cityModifiers1 = cityModifiers[city];
      Game.Prefabs.BuildingData buildingData = buildingDatas[prefab];
      float num1 = (float) (buildingData.m_LotSize.x * buildingData.m_LotSize.y);
      Entity roadEdge = Entity.Null;
      float curvePosition = 0.0f;
      int level = (int) spawnableBuildings[prefab].m_Level;
      if (buildings.HasComponent(property))
      {
        Building building = buildings[property];
        roadEdge = building.m_RoadEdge;
        curvePosition = building.m_CurvePosition;
      }
      if (buildingPropertyData.m_ResidentialProperties <= 0)
        return;
      float num2 = num1 / (float) buildingPropertyData.m_ResidentialProperties;
      float num3 = 1f;
      int currentHappiness = 50;
      int leisureValue = 128;
      float num4 = 0.3f;
      float num5 = 0.25f;
      float num6 = 0.25f;
      float num7 = 0.15f;
      float num8 = 0.05f;
      float num9 = 2f;
      if (renters.HasBuffer(property))
      {
        num4 = 0.0f;
        num5 = 0.0f;
        num6 = 0.0f;
        num7 = 0.0f;
        num8 = 0.0f;
        int2 int2_1 = new int2();
        int2 int2_2 = new int2();
        int num10 = 0;
        int num11 = 0;
        DynamicBuffer<Renter> dynamicBuffer1 = renters[property];
        for (int index1 = 0; index1 < dynamicBuffer1.Length; ++index1)
        {
          Entity renter = dynamicBuffer1[index1].m_Renter;
          if (householdCitizens.HasBuffer(renter))
          {
            ++num11;
            DynamicBuffer<HouseholdCitizen> dynamicBuffer2 = householdCitizens[renter];
            for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
            {
              Entity citizen1 = dynamicBuffer2[index2].m_Citizen;
              if (citizenDatas.HasComponent(citizen1))
              {
                Citizen citizen2 = citizenDatas[citizen1];
                int2_2.x += citizen2.Happiness;
                ++int2_2.y;
                num10 += (int) citizen2.m_LeisureCounter;
                switch (citizen2.GetEducationLevel())
                {
                  case 0:
                    ++num4;
                    break;
                  case 1:
                    ++num5;
                    break;
                  case 2:
                    ++num6;
                    break;
                  case 3:
                    ++num7;
                    break;
                  case 4:
                    ++num8;
                    break;
                }
                if (citizen2.GetAge() == CitizenAge.Child)
                  ++int2_1.x;
              }
            }
            ++int2_1.y;
          }
        }
        if (int2_1.y > 0)
          num3 = (float) int2_1.x / (float) int2_1.y;
        if (int2_2.y > 0)
        {
          currentHappiness = Mathf.RoundToInt((float) int2_2.x / (float) int2_2.y);
          leisureValue = Mathf.RoundToInt((float) num10 / (float) int2_2.y);
          num4 /= (float) int2_2.y;
          num5 /= (float) int2_2.y;
          num6 /= (float) int2_2.y;
          num7 /= (float) int2_2.y;
          num8 /= (float) int2_2.y;
          num9 = (float) int2_2.y / (float) num11;
        }
      }
      Entity healthcareServicePrefab = healthcareParameters.m_HealthcareServicePrefab;
      Entity parkServicePrefab = parkParameters.m_ParkServicePrefab;
      Entity educationServicePrefab = educationParameters.m_EducationServicePrefab;
      Entity telecomServicePrefab = telecomParameters.m_TelecomServicePrefab;
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[4].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 electricitySupplyBonuses = CitizenHappinessSystem.GetElectricitySupplyBonuses(property, ref electricityConsumers, in citizenHappinessParameters);
        int2 factor = factors[3];
        ++factor.x;
        factor.y += (electricitySupplyBonuses.x + electricitySupplyBonuses.y) / 2 - happinessFactorParameters[4].m_BaseLevel;
        factors[3] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[23].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 electricityFeeBonuses = CitizenHappinessSystem.GetElectricityFeeBonuses(property, ref electricityConsumers, relativeElectricityFee, in citizenHappinessParameters);
        int2 factor = factors[26];
        ++factor.x;
        factor.y += (electricityFeeBonuses.x + electricityFeeBonuses.y) / 2 - happinessFactorParameters[23].m_BaseLevel;
        factors[26] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[8].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 waterSupplyBonuses = CitizenHappinessSystem.GetWaterSupplyBonuses(property, ref waterConsumers, in citizenHappinessParameters);
        int2 factor = factors[7];
        ++factor.x;
        factor.y += (waterSupplyBonuses.x + waterSupplyBonuses.y) / 2 - happinessFactorParameters[8].m_BaseLevel;
        factors[7] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[24].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 waterFeeBonuses = CitizenHappinessSystem.GetWaterFeeBonuses(property, ref waterConsumers, relativeWaterFee, in citizenHappinessParameters);
        int2 factor = factors[27];
        ++factor.x;
        factor.y += (waterFeeBonuses.x + waterFeeBonuses.y) / 2 - happinessFactorParameters[24].m_BaseLevel;
        factors[27] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[9].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 pollutionBonuses = CitizenHappinessSystem.GetWaterPollutionBonuses(property, ref waterConsumers, cityModifiers1, in citizenHappinessParameters);
        int2 factor = factors[8];
        ++factor.x;
        factor.y += (pollutionBonuses.x + pollutionBonuses.y) / 2 - happinessFactorParameters[9].m_BaseLevel;
        factors[8] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[10].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 sewageBonuses = CitizenHappinessSystem.GetSewageBonuses(property, ref waterConsumers, in citizenHappinessParameters);
        int2 factor = factors[9];
        ++factor.x;
        factor.y += (sewageBonuses.x + sewageBonuses.y) / 2 - happinessFactorParameters[10].m_BaseLevel;
        factors[9] = factor;
      }
      if (serviceCoverages.HasBuffer(roadEdge))
      {
        DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage = serviceCoverages[roadEdge];
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[5].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 healthcareBonuses = CitizenHappinessSystem.GetHealthcareBonuses(curvePosition, serviceCoverage, ref locked, healthcareServicePrefab, in citizenHappinessParameters);
          int2 factor = factors[4];
          ++factor.x;
          factor.y += (healthcareBonuses.x + healthcareBonuses.y) / 2 - happinessFactorParameters[5].m_BaseLevel;
          factors[4] = factor;
        }
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[12].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 entertainmentBonuses = CitizenHappinessSystem.GetEntertainmentBonuses(curvePosition, serviceCoverage, cityModifiers1, ref locked, parkServicePrefab, in citizenHappinessParameters);
          int2 factor = factors[11];
          ++factor.x;
          factor.y += (entertainmentBonuses.x + entertainmentBonuses.y) / 2 - happinessFactorParameters[12].m_BaseLevel;
          factors[11] = factor;
        }
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[13].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 educationBonuses = CitizenHappinessSystem.GetEducationBonuses(curvePosition, serviceCoverage, ref locked, educationServicePrefab, in citizenHappinessParameters, 1);
          int2 factor = factors[12];
          ++factor.x;
          factor.y += Mathf.RoundToInt((float) ((double) num3 * (double) (educationBonuses.x + educationBonuses.y) / 2.0)) - happinessFactorParameters[13].m_BaseLevel;
          factors[12] = factor;
        }
        if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[15].m_LockedEntity))
        {
          // ISSUE: reference to a compiler-generated method
          int2 wellfareBonuses = CitizenHappinessSystem.GetWellfareBonuses(curvePosition, serviceCoverage, in citizenHappinessParameters, currentHappiness);
          int2 factor = factors[14];
          ++factor.x;
          factor.y += (wellfareBonuses.x + wellfareBonuses.y) / 2 - happinessFactorParameters[15].m_BaseLevel;
          factors[14] = factor;
        }
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[6].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 pollutionBonuses = CitizenHappinessSystem.GetGroundPollutionBonuses(property, ref transforms, pollutionMap, cityModifiers1, in citizenHappinessParameters);
        int2 factor = factors[5];
        ++factor.x;
        factor.y += (pollutionBonuses.x + pollutionBonuses.y) / 2 - happinessFactorParameters[6].m_BaseLevel;
        factors[5] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[2].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 pollutionBonuses = CitizenHappinessSystem.GetAirPollutionBonuses(property, ref transforms, airPollutionMap, cityModifiers1, in citizenHappinessParameters);
        int2 factor = factors[2];
        ++factor.x;
        factor.y += (pollutionBonuses.x + pollutionBonuses.y) / 2 - happinessFactorParameters[2].m_BaseLevel;
        factors[2] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[7].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 noiseBonuses = CitizenHappinessSystem.GetNoiseBonuses(property, ref transforms, noisePollutionMap, in citizenHappinessParameters);
        int2 factor = factors[6];
        ++factor.x;
        factor.y += (noiseBonuses.x + noiseBonuses.y) / 2 - happinessFactorParameters[7].m_BaseLevel;
        factors[6] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[11].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 garbageBonuses = CitizenHappinessSystem.GetGarbageBonuses(property, ref garbageProducers, ref locked, happinessFactorParameters[11].m_LockedEntity, in garbageParameters);
        int2 factor = factors[10];
        ++factor.x;
        factor.y += (garbageBonuses.x + garbageBonuses.y) / 2 - happinessFactorParameters[11].m_BaseLevel;
        factors[10] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[1].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 crimeBonuses = CitizenHappinessSystem.GetCrimeBonuses(new CrimeVictim(), property, ref crimeProducers, ref locked, happinessFactorParameters[1].m_LockedEntity, in citizenHappinessParameters);
        int2 factor = factors[1];
        ++factor.x;
        factor.y += (crimeBonuses.x + crimeBonuses.y) / 2 - happinessFactorParameters[1].m_BaseLevel;
        factors[1] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[14].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 mailBonuses = CitizenHappinessSystem.GetMailBonuses(property, ref mailProducers, ref locked, telecomServicePrefab, in citizenHappinessParameters);
        int2 factor = factors[13];
        ++factor.x;
        factor.y += (mailBonuses.x + mailBonuses.y) / 2 - happinessFactorParameters[14].m_BaseLevel;
        factors[13] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[0].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 telecomBonuses = CitizenHappinessSystem.GetTelecomBonuses(property, ref transforms, telecomCoverage, ref locked, telecomServicePrefab, in citizenHappinessParameters);
        int2 factor = factors[0];
        ++factor.x;
        factor.y += (telecomBonuses.x + telecomBonuses.y) / 2 - happinessFactorParameters[0].m_BaseLevel;
        factors[0] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[16].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        int2 leisureBonuses = CitizenHappinessSystem.GetLeisureBonuses((byte) leisureValue);
        int2 factor = factors[15];
        ++factor.x;
        factor.y += (leisureBonuses.x + leisureBonuses.y) / 2 - happinessFactorParameters[16].m_BaseLevel;
        factors[15] = factor;
      }
      if (!locked.HasEnabledComponent<Locked>(happinessFactorParameters[17].m_LockedEntity))
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        float2 float2 = new float2(num4, num4) * (float2) CitizenHappinessSystem.GetTaxBonuses(0, taxRates, in citizenHappinessParameters) + new float2(num5, num5) * (float2) CitizenHappinessSystem.GetTaxBonuses(1, taxRates, in citizenHappinessParameters) + new float2(num6, num6) * (float2) CitizenHappinessSystem.GetTaxBonuses(2, taxRates, in citizenHappinessParameters) + new float2(num7, num7) * (float2) CitizenHappinessSystem.GetTaxBonuses(3, taxRates, in citizenHappinessParameters) + new float2(num8, num8) * (float2) CitizenHappinessSystem.GetTaxBonuses(4, taxRates, in citizenHappinessParameters);
        int2 factor = factors[16];
        ++factor.x;
        factor.y += Mathf.RoundToInt(float2.x + float2.y) / 2 - happinessFactorParameters[17].m_BaseLevel;
        factors[16] = factor;
      }
      if (locked.HasEnabledComponent<Locked>(happinessFactorParameters[3].m_LockedEntity))
        return;
      // ISSUE: reference to a compiler-generated method
      float2 apartmentWellbeing = (float2) CitizenHappinessSystem.GetApartmentWellbeing(buildingPropertyData.m_SpaceMultiplier * num2 / num9, level);
      int2 factor1 = factors[21];
      ++factor1.x;
      factor1.y += Mathf.RoundToInt(apartmentWellbeing.x + apartmentWellbeing.y) / 2 - happinessFactorParameters[3].m_BaseLevel;
      factors[21] = factor1;
    }

    public static void GetCompanyHappinessFactors(
      Entity property,
      NativeArray<int2> factors,
      ref ComponentLookup<PrefabRef> prefabs,
      ref ComponentLookup<SpawnableBuildingData> spawnableBuildings,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas,
      ref ComponentLookup<Building> buildings,
      ref ComponentLookup<OfficeBuilding> officeBuildings,
      ref BufferLookup<Renter> renters,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ref ComponentLookup<CompanyData> companies,
      ref ComponentLookup<IndustrialProcessData> industrialProcessDatas,
      ref ComponentLookup<WorkProvider> workProviders,
      ref BufferLookup<Employee> employees,
      ref ComponentLookup<WorkplaceData> workplaceDatas,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<HealthProblem> healthProblems,
      ref ComponentLookup<ServiceAvailable> serviceAvailables,
      ref ComponentLookup<ResourceData> resourceDatas,
      ref ComponentLookup<ZonePropertiesData> zonePropertiesDatas,
      ref BufferLookup<Efficiency> efficiencies,
      ref ComponentLookup<ServiceCompanyData> serviceCompanyDatas,
      ref BufferLookup<ResourceAvailability> availabilities,
      ref BufferLookup<TradeCost> tradeCosts,
      EconomyParameterData economyParameters,
      NativeArray<int> taxRates,
      NativeArray<Entity> processes,
      ResourcePrefabs resourcePrefabs)
    {
      if (!prefabs.HasComponent(property))
        return;
      Entity prefab = prefabs[property].m_Prefab;
      if (!spawnableBuildings.HasComponent(prefab) || !buildingDatas.HasComponent(prefab))
        return;
      BuildingPropertyData buildingPropertyData = buildingPropertyDatas[prefab];
      Game.Prefabs.BuildingData buildingData = buildingDatas[prefab];
      SpawnableBuildingData spawnableData = spawnableBuildings[prefab];
      int level = (int) spawnableData.m_Level;
      Building building = new Building();
      if (buildings.HasComponent(property))
        building = buildings[property];
      bool flag = false;
      Entity entity1 = new Entity();
      Entity entity2 = new Entity();
      IndustrialProcessData processData1 = new IndustrialProcessData();
      ServiceCompanyData serviceCompanyData = new ServiceCompanyData();
      Resource resource = buildingPropertyData.m_AllowedManufactured | buildingPropertyData.m_AllowedSold;
      if (resource == Resource.NoResource)
        return;
      if (renters.HasBuffer(property))
      {
        DynamicBuffer<Renter> dynamicBuffer = renters[property];
        for (int index = 0; index < dynamicBuffer.Length; ++index)
        {
          entity1 = dynamicBuffer[index].m_Renter;
          if (companies.HasComponent(entity1) && prefabs.HasComponent(entity1))
          {
            entity2 = prefabs[entity1].m_Prefab;
            if (industrialProcessDatas.HasComponent(entity2))
            {
              if (serviceCompanyDatas.HasComponent(entity2))
                serviceCompanyData = serviceCompanyDatas[entity2];
              processData1 = industrialProcessDatas[entity2];
              flag = true;
              break;
            }
          }
        }
      }
      if (flag)
      {
        BuildingHappiness.AddCompanyHappinessFactors(factors, property, prefab, entity1, entity2, processData1, serviceCompanyData, buildingPropertyData.m_AllowedSold > Resource.NoResource, level, ref officeBuildings, ref workProviders, ref employees, ref workplaceDatas, ref citizens, ref healthProblems, ref serviceAvailables, ref buildingPropertyDatas, ref resourceDatas, ref serviceCompanyDatas, ref efficiencies, ref availabilities, ref tradeCosts, taxRates, building, spawnableData, buildingData, resourcePrefabs, ref economyParameters);
      }
      else
      {
        for (int index = 0; index < processes.Length; ++index)
        {
          IndustrialProcessData processData2 = industrialProcessDatas[processes[index]];
          if (serviceCompanyDatas.HasComponent(processes[index]))
            serviceCompanyData = serviceCompanyDatas[processes[index]];
          if ((resource & processData2.m_Output.m_Resource) != Resource.NoResource)
            BuildingHappiness.AddCompanyHappinessFactors(factors, property, prefab, entity1, entity2, processData2, serviceCompanyData, buildingPropertyData.m_AllowedSold > Resource.NoResource, level, ref officeBuildings, ref workProviders, ref employees, ref workplaceDatas, ref citizens, ref healthProblems, ref serviceAvailables, ref buildingPropertyDatas, ref resourceDatas, ref serviceCompanyDatas, ref efficiencies, ref availabilities, ref tradeCosts, taxRates, building, spawnableData, buildingData, resourcePrefabs, ref economyParameters);
        }
      }
    }

    private static void AddCompanyHappinessFactors(
      NativeArray<int2> factors,
      Entity property,
      Entity prefab,
      Entity renter,
      Entity renterPrefab,
      IndustrialProcessData processData,
      ServiceCompanyData serviceCompanyData,
      bool commercial,
      int level,
      ref ComponentLookup<OfficeBuilding> officeBuildings,
      ref ComponentLookup<WorkProvider> workProviders,
      ref BufferLookup<Employee> employees,
      ref ComponentLookup<WorkplaceData> workplaceDatas,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<HealthProblem> healthProblems,
      ref ComponentLookup<ServiceAvailable> serviceAvailables,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas,
      ref ComponentLookup<ResourceData> resourceDatas,
      ref ComponentLookup<ServiceCompanyData> serviceCompanyDatas,
      ref BufferLookup<Efficiency> efficiencies,
      ref BufferLookup<ResourceAvailability> availabilities,
      ref BufferLookup<TradeCost> tradeCosts,
      NativeArray<int> taxRates,
      Building building,
      SpawnableBuildingData spawnableData,
      Game.Prefabs.BuildingData buildingData,
      ResourcePrefabs resourcePrefabs,
      ref EconomyParameterData economyParameters)
    {
    }
  }
}
