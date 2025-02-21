// Decompiled with JetBrains decompiler
// Type: Game.Buildings.PropertyUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Areas;
using Game.Citizens;
using Game.City;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Buildings
{
  public static class PropertyUtils
  {
    public static readonly float kHomelessApartmentSize = 0.01f;

    public static int GetBuildingLevel(
      Entity prefabEntity,
      ComponentLookup<SpawnableBuildingData> spawnableBuildingDatas)
    {
      SpawnableBuildingData componentData;
      return spawnableBuildingDatas.TryGetComponent(prefabEntity, out componentData) ? (int) componentData.m_Level : 1;
    }

    public static bool IsMixedBuilding(
      Entity buildingPrefab,
      ref ComponentLookup<BuildingPropertyData> buildingPropertyDatas)
    {
      if (!buildingPropertyDatas.HasComponent(buildingPrefab) || buildingPropertyDatas[buildingPrefab].m_ResidentialProperties <= 0)
        return false;
      return buildingPropertyDatas[buildingPrefab].m_AllowedSold != Resource.NoResource || buildingPropertyDatas[buildingPrefab].m_AllowedManufactured > Resource.NoResource;
    }

    public static int GetProperties(
      BuildingPropertyData propertyData,
      Game.Zones.AreaType areaType,
      bool storage)
    {
      switch (areaType)
      {
        case Game.Zones.AreaType.Residential:
          return propertyData.m_ResidentialProperties;
        case Game.Zones.AreaType.Commercial:
          return propertyData.m_AllowedSold == Resource.NoResource ? 0 : 1;
        case Game.Zones.AreaType.Industrial:
          return storage ? (propertyData.m_AllowedStored == Resource.NoResource ? 0 : 1) : (propertyData.m_AllowedManufactured == Resource.NoResource ? 0 : 1);
        default:
          return 0;
      }
    }

    public static int GetRentPricePerRenter(
      ConsumptionData consumptionData,
      BuildingPropertyData buildingPropertyData,
      int buildingLevel,
      int lotSize,
      float landValueBase,
      Game.Zones.AreaType areaType,
      ref EconomyParameterData economyParameterData)
    {
      float num1 = economyParameterData.m_RentPriceBuildingZoneTypeBase.x;
      float num2 = economyParameterData.m_LandValueModifier.x;
      switch (areaType)
      {
        case Game.Zones.AreaType.Commercial:
          num1 = economyParameterData.m_RentPriceBuildingZoneTypeBase.y;
          num2 = economyParameterData.m_LandValueModifier.y;
          break;
        case Game.Zones.AreaType.Industrial:
          num1 = economyParameterData.m_RentPriceBuildingZoneTypeBase.z;
          num2 = economyParameterData.m_LandValueModifier.z;
          break;
      }
      return Mathf.RoundToInt((float) (((double) landValueBase * (double) num2 + (double) num1 * (double) buildingLevel) * (double) lotSize * (double) buildingPropertyData.m_SpaceMultiplier / ((buildingPropertyData.m_ResidentialProperties <= 0 ? 0 : (buildingPropertyData.m_AllowedSold != Resource.NoResource ? 1 : (buildingPropertyData.m_AllowedManufactured > Resource.NoResource ? 1 : 0))) == 0 ? (double) buildingPropertyData.CountProperties() : (double) Mathf.RoundToInt((float) buildingPropertyData.m_ResidentialProperties / (1f - economyParameterData.m_MixedBuildingCompanyRentPercentage)))));
    }

    public static float GetPropertyScore(
      Entity property,
      Entity household,
      DynamicBuffer<HouseholdCitizen> citizenBuffer,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<BuildingPropertyData> buildingProperties,
      ref ComponentLookup<Building> buildings,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ref ComponentLookup<Household> households,
      ref ComponentLookup<Citizen> citizens,
      ref ComponentLookup<Game.Citizens.Student> students,
      ref ComponentLookup<Worker> workers,
      ref ComponentLookup<SpawnableBuildingData> spawnableDatas,
      ref ComponentLookup<CrimeProducer> crimes,
      ref BufferLookup<Game.Net.ServiceCoverage> serviceCoverages,
      ref ComponentLookup<Locked> locked,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      ref ComponentLookup<GarbageProducer> garbageProducers,
      ref ComponentLookup<MailProducer> mailProducers,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      ref ComponentLookup<Abandoned> abandoneds,
      ref ComponentLookup<Park> parks,
      ref BufferLookup<ResourceAvailability> availabilities,
      NativeArray<int> taxRates,
      NativeArray<GroundPollution> pollutionMap,
      NativeArray<AirPollution> airPollutionMap,
      NativeArray<NoisePollution> noiseMap,
      CellMapData<TelecomCoverage> telecomCoverages,
      DynamicBuffer<CityModifier> cityModifiers,
      Entity healthcareService,
      Entity entertainmentService,
      Entity educationService,
      Entity telecomService,
      Entity garbageService,
      Entity policeService,
      CitizenHappinessParameterData citizenHappinessParameterData,
      GarbageParameterData garbageParameterData)
    {
      if (!buildings.HasComponent(property))
        return float.NegativeInfinity;
      bool flag1 = (households[household].m_Flags & HouseholdFlags.MovedIn) != 0;
      bool flag2 = BuildingUtils.IsHomelessShelterBuilding(property, ref parks, ref abandoneds);
      if (flag2 && !flag1)
        return float.NegativeInfinity;
      Building buildingData = buildings[property];
      Entity prefab = prefabRefs[property].m_Prefab;
      // ISSUE: variable of a compiler-generated type
      HouseholdFindPropertySystem.GenericApartmentQuality apartmentQuality1 = PropertyUtils.GetGenericApartmentQuality(property, prefab, ref buildingData, ref buildingProperties, ref buildingDatas, ref spawnableDatas, ref crimes, ref serviceCoverages, ref locked, ref electricityConsumers, ref waterConsumers, ref garbageProducers, ref mailProducers, ref transforms, ref abandoneds, pollutionMap, airPollutionMap, noiseMap, telecomCoverages, cityModifiers, healthcareService, entertainmentService, educationService, telecomService, garbageService, policeService, citizenHappinessParameterData, garbageParameterData);
      int length = citizenBuffer.Length;
      float num1 = 0.0f;
      int num2 = 0;
      int num3 = 0;
      int averageHappiness = 0;
      int children = 0;
      int num4 = 0;
      for (int index = 0; index < citizenBuffer.Length; ++index)
      {
        Entity citizen1 = citizenBuffer[index].m_Citizen;
        Citizen citizen2 = citizens[citizen1];
        averageHappiness += citizen2.Happiness;
        if (citizen2.GetAge() == CitizenAge.Child)
        {
          ++children;
        }
        else
        {
          ++num3;
          // ISSUE: reference to a compiler-generated method
          num4 += CitizenHappinessSystem.GetTaxBonuses(citizen2.GetEducationLevel(), taxRates, in citizenHappinessParameterData).y;
        }
        if (students.HasComponent(citizen1))
        {
          ++num2;
          Game.Citizens.Student student = students[citizen1];
          if (student.m_School != property)
            num1 += student.m_LastCommuteTime;
        }
        else if (workers.HasComponent(citizen1))
        {
          ++num2;
          Worker worker = workers[citizen1];
          if (worker.m_Workplace != property)
            num1 += worker.m_LastCommuteTime;
        }
      }
      if (num2 > 0)
        num1 /= (float) num2;
      if (citizenBuffer.Length > 0)
      {
        averageHappiness /= citizenBuffer.Length;
        if (num3 > 0)
          num4 /= num3;
      }
      double serviceAvailability = (double) PropertyUtils.GetServiceAvailability(buildingData.m_RoadEdge, buildingData.m_CurvePosition, availabilities);
      float apartmentQuality2 = PropertyUtils.GetCachedApartmentQuality(length, children, averageHappiness, apartmentQuality1);
      float num5 = flag2 ? -1000f : 0.0f;
      double num6 = (double) apartmentQuality2;
      return (float) (serviceAvailability + num6) + (float) (2 * num4) - num1 + num5;
    }

    public static float GetServiceAvailability(
      Entity roadEdge,
      float curvePos,
      BufferLookup<ResourceAvailability> availabilities)
    {
      return availabilities.HasBuffer(roadEdge) ? NetUtils.GetAvailability(availabilities[roadEdge], AvailableResource.Services, curvePos) : 0.0f;
    }

    public static int2 GetElectricityBonusForApartmentQuality(
      Entity building,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      in CitizenHappinessParameterData data)
    {
      ElectricityConsumer componentData;
      if (!electricityConsumers.TryGetComponent(building, out componentData) || componentData.electricityConnected)
        return new int2();
      return new int2()
      {
        y = (int) math.round(-data.m_ElectricityWellbeingPenalty)
      };
    }

    public static int2 GetWaterBonusForApartmentQuality(
      Entity building,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      in CitizenHappinessParameterData data)
    {
      WaterConsumer componentData;
      if (!waterConsumers.TryGetComponent(building, out componentData) || componentData.waterConnected)
        return new int2();
      return new int2()
      {
        x = (int) math.round((float) -data.m_WaterHealthPenalty),
        y = (int) math.round((float) -data.m_WaterWellbeingPenalty)
      };
    }

    public static int2 GetSewageBonusForApartmentQuality(
      Entity building,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      in CitizenHappinessParameterData data)
    {
      WaterConsumer componentData;
      if (!waterConsumers.TryGetComponent(building, out componentData) || componentData.sewageConnected)
        return new int2();
      return new int2()
      {
        x = (int) math.round((float) -data.m_SewageHealthEffect),
        y = (int) math.round((float) -data.m_SewageWellbeingEffect)
      };
    }

    public static HouseholdFindPropertySystem.GenericApartmentQuality GetGenericApartmentQuality(
      Entity building,
      Entity buildingPrefab,
      ref Building buildingData,
      ref ComponentLookup<BuildingPropertyData> buildingProperties,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ref ComponentLookup<SpawnableBuildingData> spawnableDatas,
      ref ComponentLookup<CrimeProducer> crimes,
      ref BufferLookup<Game.Net.ServiceCoverage> serviceCoverages,
      ref ComponentLookup<Locked> locked,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      ref ComponentLookup<GarbageProducer> garbageProducers,
      ref ComponentLookup<MailProducer> mailProducers,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      ref ComponentLookup<Abandoned> abandoneds,
      NativeArray<GroundPollution> pollutionMap,
      NativeArray<AirPollution> airPollutionMap,
      NativeArray<NoisePollution> noiseMap,
      CellMapData<TelecomCoverage> telecomCoverages,
      DynamicBuffer<CityModifier> cityModifiers,
      Entity healthcareService,
      Entity entertainmentService,
      Entity educationService,
      Entity telecomService,
      Entity garbageService,
      Entity policeService,
      CitizenHappinessParameterData happinessParameterData,
      GarbageParameterData garbageParameterData)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HouseholdFindPropertySystem.GenericApartmentQuality apartmentQuality = new HouseholdFindPropertySystem.GenericApartmentQuality();
      bool flag = true;
      BuildingPropertyData buildingPropertyData = new BuildingPropertyData();
      SpawnableBuildingData spawnableBuildingData = new SpawnableBuildingData();
      if (buildingProperties.HasComponent(buildingPrefab))
      {
        buildingPropertyData = buildingProperties[buildingPrefab];
        flag = false;
      }
      Game.Prefabs.BuildingData buildingData1 = buildingDatas[buildingPrefab];
      if (spawnableDatas.HasComponent(buildingPrefab) && !abandoneds.HasComponent(building))
        spawnableBuildingData = spawnableDatas[buildingPrefab];
      else
        flag = true;
      // ISSUE: reference to a compiler-generated field
      apartmentQuality.apartmentSize = flag ? PropertyUtils.kHomelessApartmentSize : buildingPropertyData.m_SpaceMultiplier * (float) buildingData1.m_LotSize.x * (float) buildingData1.m_LotSize.y / math.max(1f, (float) buildingPropertyData.m_ResidentialProperties);
      // ISSUE: reference to a compiler-generated field
      apartmentQuality.level = (int) spawnableBuildingData.m_Level;
      int2 int2_1 = new int2();
      if (serviceCoverages.HasBuffer(buildingData.m_RoadEdge))
      {
        DynamicBuffer<Game.Net.ServiceCoverage> serviceCoverage = serviceCoverages[buildingData.m_RoadEdge];
        // ISSUE: reference to a compiler-generated method
        int2 healthcareBonuses = CitizenHappinessSystem.GetHealthcareBonuses(buildingData.m_CurvePosition, serviceCoverage, ref locked, healthcareService, in happinessParameterData);
        // ISSUE: reference to a compiler-generated method
        int2_1 = int2_1 + healthcareBonuses + CitizenHappinessSystem.GetEntertainmentBonuses(buildingData.m_CurvePosition, serviceCoverage, cityModifiers, ref locked, entertainmentService, in happinessParameterData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        apartmentQuality.welfareBonus = CitizenHappinessSystem.GetWelfareValue(buildingData.m_CurvePosition, serviceCoverage, in happinessParameterData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        apartmentQuality.educationBonus = (float2) CitizenHappinessSystem.GetEducationBonuses(buildingData.m_CurvePosition, serviceCoverage, ref locked, educationService, in happinessParameterData, 1);
      }
      // ISSUE: reference to a compiler-generated method
      int2 crimeBonuses = CitizenHappinessSystem.GetCrimeBonuses(new CrimeVictim(), building, ref crimes, ref locked, policeService, in happinessParameterData);
      int2 int2_2 = flag ? new int2(0, -happinessParameterData.m_MaxCrimePenalty - crimeBonuses.y) : crimeBonuses;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      int2 int2_3 = int2_1 + int2_2 + CitizenHappinessSystem.GetGroundPollutionBonuses(building, ref transforms, pollutionMap, cityModifiers, in happinessParameterData) + CitizenHappinessSystem.GetAirPollutionBonuses(building, ref transforms, airPollutionMap, cityModifiers, in happinessParameterData) + CitizenHappinessSystem.GetNoiseBonuses(building, ref transforms, noiseMap, in happinessParameterData) + CitizenHappinessSystem.GetTelecomBonuses(building, ref transforms, telecomCoverages, ref locked, telecomService, in happinessParameterData) + PropertyUtils.GetElectricityBonusForApartmentQuality(building, ref electricityConsumers, in happinessParameterData) + PropertyUtils.GetWaterBonusForApartmentQuality(building, ref waterConsumers, in happinessParameterData) + PropertyUtils.GetSewageBonusForApartmentQuality(building, ref waterConsumers, in happinessParameterData) + CitizenHappinessSystem.GetWaterPollutionBonuses(building, ref waterConsumers, cityModifiers, in happinessParameterData) + CitizenHappinessSystem.GetGarbageBonuses(building, ref garbageProducers, ref locked, garbageService, in garbageParameterData) + CitizenHappinessSystem.GetMailBonuses(building, ref mailProducers, ref locked, telecomService, in happinessParameterData);
      if (flag)
      {
        // ISSUE: reference to a compiler-generated method
        int2 homelessBonuses = CitizenHappinessSystem.GetHomelessBonuses(in happinessParameterData);
        int2_3 += homelessBonuses;
      }
      // ISSUE: reference to a compiler-generated field
      apartmentQuality.score = (float) (int2_3.x + int2_3.y);
      return apartmentQuality;
    }

    public static float GetApartmentQuality(
      int familySize,
      int children,
      Entity building,
      ref Building buildingData,
      Entity buildingPrefab,
      ref ComponentLookup<BuildingPropertyData> buildingProperties,
      ref ComponentLookup<Game.Prefabs.BuildingData> buildingDatas,
      ref ComponentLookup<SpawnableBuildingData> spawnableDatas,
      ref ComponentLookup<CrimeProducer> crimes,
      ref BufferLookup<Game.Net.ServiceCoverage> serviceCoverages,
      ref ComponentLookup<Locked> locked,
      ref ComponentLookup<ElectricityConsumer> electricityConsumers,
      ref ComponentLookup<WaterConsumer> waterConsumers,
      ref ComponentLookup<GarbageProducer> garbageProducers,
      ref ComponentLookup<MailProducer> mailProducers,
      ref ComponentLookup<PrefabRef> prefabs,
      ref ComponentLookup<Game.Objects.Transform> transforms,
      ref ComponentLookup<Abandoned> abandoneds,
      NativeArray<GroundPollution> pollutionMap,
      NativeArray<AirPollution> airPollutionMap,
      NativeArray<NoisePollution> noiseMap,
      CellMapData<TelecomCoverage> telecomCoverages,
      DynamicBuffer<CityModifier> cityModifiers,
      Entity healthcareService,
      Entity entertainmentService,
      Entity educationService,
      Entity telecomService,
      Entity garbageService,
      Entity policeService,
      CitizenHappinessParameterData happinessParameterData,
      GarbageParameterData garbageParameterData,
      int averageHappiness)
    {
      // ISSUE: variable of a compiler-generated type
      HouseholdFindPropertySystem.GenericApartmentQuality apartmentQuality = PropertyUtils.GetGenericApartmentQuality(building, buildingPrefab, ref buildingData, ref buildingProperties, ref buildingDatas, ref spawnableDatas, ref crimes, ref serviceCoverages, ref locked, ref electricityConsumers, ref waterConsumers, ref garbageProducers, ref mailProducers, ref transforms, ref abandoneds, pollutionMap, airPollutionMap, noiseMap, telecomCoverages, cityModifiers, healthcareService, entertainmentService, educationService, telecomService, garbageService, policeService, happinessParameterData, garbageParameterData);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int2 cachedWelfareBonuses = CitizenHappinessSystem.GetCachedWelfareBonuses(apartmentQuality.welfareBonus, averageHappiness);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return CitizenHappinessSystem.GetApartmentWellbeing(apartmentQuality.apartmentSize / (float) familySize, (int) spawnableDatas[buildingPrefab].m_Level) + math.sqrt((float) children) * (apartmentQuality.educationBonus.x + apartmentQuality.educationBonus.y) + (float) cachedWelfareBonuses.x + (float) cachedWelfareBonuses.y + apartmentQuality.score;
    }

    public static float GetCachedApartmentQuality(
      int familySize,
      int children,
      int averageHappiness,
      HouseholdFindPropertySystem.GenericApartmentQuality quality)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int2 cachedWelfareBonuses = CitizenHappinessSystem.GetCachedWelfareBonuses(quality.welfareBonus, averageHappiness);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return CitizenHappinessSystem.GetApartmentWellbeing(quality.apartmentSize / (float) familySize, quality.level) + math.sqrt((float) children) * (quality.educationBonus.x + quality.educationBonus.y) + (float) cachedWelfareBonuses.x + (float) cachedWelfareBonuses.y + quality.score;
    }

    [BurstCompile]
    public struct ExtractorFindCompanyJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_Entities;
      [ReadOnly]
      public NativeList<Entity> m_ExtractorCompanyEntities;
      [ReadOnly]
      public NativeList<Entity> m_CompanyPrefabs;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_Properties;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_Processes;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<Attached> m_Attached;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_Lots;
      [ReadOnly]
      public ComponentLookup<Geometry> m_Geometries;
      [ReadOnly]
      public ComponentLookup<Extractor> m_ExtractorAreas;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_ExtractorDatas;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public ExtractorParameterData m_ExtractorParameters;
      public EconomyParameterData m_EconomyParameters;
      public NativeQueue<RentAction> m_RentActionQueue;
      public EntityCommandBuffer m_CommandBuffer;
      public float m_AverageTemperature;
      [ReadOnly]
      public NativeArray<int> m_Productions;
      [ReadOnly]
      public NativeArray<int> m_Consumptions;

      private float Evaluate(Entity entity, Resource resource)
      {
        IndustrialProcessData industrialProcessData = new IndustrialProcessData();
        ResourceData resourceData = this.m_ResourceDatas[this.m_ResourcePrefabs[resource]];
        bool flag = false;
        for (int index = 0; index < this.m_CompanyPrefabs.Length; ++index)
        {
          if (this.m_Processes.HasComponent(this.m_CompanyPrefabs[index]) && this.m_WorkplaceDatas.HasComponent(this.m_CompanyPrefabs[index]))
          {
            IndustrialProcessData process = this.m_Processes[this.m_CompanyPrefabs[index]];
            if (process.m_Output.m_Resource == resource && process.m_Input1.m_Resource == Resource.NoResource)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag || !this.m_Attached.HasComponent(entity))
          return float.NegativeInfinity;
        DynamicBuffer<Game.Areas.SubArea> subArea = this.m_SubAreas[this.m_Attached[entity].m_Parent];
        float concentration;
        // ISSUE: reference to a compiler-generated method
        ExtractorCompanySystem.GetBestConcentration(resource, subArea, this.m_ExtractorAreas, this.m_Prefabs, this.m_ExtractorDatas, this.m_ExtractorParameters, this.m_ResourcePrefabs, this.m_ResourceDatas, out concentration, out int _);
        if (resourceData.m_RequireTemperature && (double) this.m_AverageTemperature < (double) resourceData.m_RequiredTemperature)
          concentration = 0.0f;
        return (double) concentration == 0.0 ? float.NegativeInfinity : concentration;
      }

      public void Execute()
      {
        if (this.m_Entities.Length == 0)
          return;
        for (int index1 = 0; index1 < this.m_Entities.Length; ++index1)
        {
          Entity entity = this.m_Entities[index1];
          if (this.m_Prefabs.HasComponent(entity))
          {
            Entity prefab1 = this.m_Prefabs[entity].m_Prefab;
            Resource resource = Resource.NoResource;
            if (this.m_Properties.HasComponent(prefab1))
            {
              Resource allowedManufactured = this.m_Properties[prefab1].m_AllowedManufactured;
              Attached componentData1;
              PrefabRef componentData2;
              BuildingPropertyData componentData3;
              if (this.m_Attached.TryGetComponent(entity, out componentData1) && this.m_Prefabs.TryGetComponent(componentData1.m_Parent, out componentData2) && this.m_Properties.TryGetComponent(componentData2.m_Prefab, out componentData3))
                allowedManufactured &= componentData3.m_AllowedManufactured;
              ResourceIterator resourceIterator = new ResourceIterator();
              float num1 = float.NegativeInfinity;
              while (resourceIterator.Next())
              {
                if ((allowedManufactured & resourceIterator.resource) != Resource.NoResource)
                {
                  float num2 = this.Evaluate(entity, resourceIterator.resource);
                  if ((double) num2 > (double) num1)
                  {
                    num1 = num2;
                    resource = resourceIterator.resource;
                  }
                }
              }
            }
            for (int index2 = 0; index2 < this.m_ExtractorCompanyEntities.Length; ++index2)
            {
              Entity extractorCompanyEntity = this.m_ExtractorCompanyEntities[index2];
              if (this.m_Prefabs.HasComponent(extractorCompanyEntity))
              {
                Entity prefab2 = this.m_Prefabs[extractorCompanyEntity].m_Prefab;
                if (this.m_Processes.HasComponent(prefab2) && this.m_Processes[prefab2].m_Output.m_Resource == resource)
                {
                  this.m_RentActionQueue.Enqueue(new RentAction()
                  {
                    m_Property = entity,
                    m_Renter = extractorCompanyEntity
                  });
                  this.m_CommandBuffer.RemoveComponent<PropertySeeker>(extractorCompanyEntity);
                  return;
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    public struct CompanyFindPropertyJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<CompanyData> m_CompanyDataType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      public ComponentTypeHandle<PropertySeeker> m_PropertySeekerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Companies.StorageCompany> m_StorageCompanyType;
      [ReadOnly]
      public NativeList<Entity> m_FreePropertyEntities;
      [ReadOnly]
      public NativeList<PrefabRef> m_PropertyPrefabs;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingPropertyDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_Availabilities;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> m_PropertiesOnMarket;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabFromEntity;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableDatas;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<LandValue> m_LandValues;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanies;
      [ReadOnly]
      public ComponentLookup<CommercialCompany> m_CommercialCompanies;
      [ReadOnly]
      public ComponentLookup<Signature> m_Signatures;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public EconomyParameterData m_EconomyParameters;
      public ZonePreferenceData m_ZonePreferences;
      public bool m_Commercial;
      public NativeQueue<RentAction>.ParallelWriter m_RentActionQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      private void Evaluate(
        int index,
        Entity company,
        ref ServiceCompanyData service,
        ref IndustrialProcessData process,
        Entity property,
        ref PropertySeeker propertySeeker,
        bool commercial,
        bool storage)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        float num = !commercial ? IndustrialFindPropertySystem.Evaluate(company, property, ref process, ref propertySeeker, this.m_Buildings, this.m_PropertiesOnMarket, this.m_PrefabFromEntity, this.m_BuildingDatas, this.m_SpawnableDatas, this.m_WorkplaceDatas, this.m_LandValues, this.m_Availabilities, this.m_EconomyParameters, this.m_ResourcePrefabs, this.m_ResourceDatas, this.m_BuildingPropertyDatas, storage) : CommercialFindPropertySystem.Evaluate(company, property, ref service, ref process, ref propertySeeker, this.m_Buildings, this.m_PrefabFromEntity, this.m_BuildingDatas, this.m_Availabilities, this.m_LandValues, this.m_ResourcePrefabs, this.m_ResourceDatas, this.m_BuildingPropertyDatas, this.m_SpawnableDatas, this.m_Renters, this.m_CommercialCompanies, ref this.m_ZonePreferences);
        if (this.m_Signatures.HasComponent(property))
          num += 5000f;
        if (!(propertySeeker.m_BestProperty == Entity.Null) && (double) num <= (double) propertySeeker.m_BestPropertyScore)
          return;
        propertySeeker.m_BestPropertyScore = num;
        propertySeeker.m_BestProperty = property;
      }

      private void SelectProperty(
        int jobIndex,
        Entity company,
        ref PropertySeeker propertySeeker,
        bool storage)
      {
        Entity bestProperty = propertySeeker.m_BestProperty;
        if (this.m_PropertiesOnMarket.HasComponent(bestProperty) && (!this.m_PropertyRenters.HasComponent(company) || !this.m_PropertyRenters[company].m_Property.Equals(bestProperty)))
        {
          this.m_RentActionQueue.Enqueue(new RentAction()
          {
            m_Property = bestProperty,
            m_Renter = company,
            m_Flags = storage ? RentActionFlags.Storage : (RentActionFlags) 0
          });
          this.m_CommandBuffer.RemoveComponent<PropertySeeker>(jobIndex, company);
        }
        else if (this.m_PropertyRenters.HasComponent(company))
        {
          this.m_CommandBuffer.RemoveComponent<PropertySeeker>(jobIndex, company);
        }
        else
        {
          propertySeeker.m_BestProperty = Entity.Null;
          propertySeeker.m_BestPropertyScore = 0.0f;
        }
      }

      private bool PropertyAllowsResource(int index, Resource resource, bool storage)
      {
        BuildingPropertyData buildingPropertyData = this.m_BuildingPropertyDatas[this.m_PropertyPrefabs[index].m_Prefab];
        Resource resource1 = !storage ? (this.m_Commercial ? buildingPropertyData.m_AllowedSold : buildingPropertyData.m_AllowedManufactured) : buildingPropertyData.m_AllowedStored;
        return (resource & resource1) > Resource.NoResource;
      }

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        NativeArray<PropertySeeker> nativeArray3 = chunk.GetNativeArray<PropertySeeker>(ref this.m_PropertySeekerType);
        chunk.GetNativeArray<CompanyData>(ref this.m_CompanyDataType);
        bool storage = chunk.Has<Game.Companies.StorageCompany>(ref this.m_StorageCompanyType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          if (!this.m_IndustrialProcessDatas.HasComponent(prefab))
            break;
          IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab];
          PropertySeeker propertySeeker = nativeArray3[index1];
          Resource resource = industrialProcessData.m_Output.m_Resource;
          ServiceCompanyData service = new ServiceCompanyData();
          if (this.m_Commercial)
            service = this.m_ServiceCompanies[prefab];
          if (!this.m_PropertyRenters.HasComponent(entity) || !(this.m_PropertyRenters[entity].m_Property != Entity.Null))
          {
            for (int index2 = 0; index2 < this.m_FreePropertyEntities.Length; ++index2)
            {
              if (this.PropertyAllowsResource(index2, resource, storage))
                this.Evaluate(index1, entity, ref service, ref industrialProcessData, this.m_FreePropertyEntities[index2], ref propertySeeker, this.m_Commercial, storage);
            }
            this.SelectProperty(unfilteredChunkIndex, entity, ref propertySeeker, storage);
            nativeArray3[index1] = propertySeeker;
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }
  }
}
