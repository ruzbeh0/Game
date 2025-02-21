// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitizenPathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public struct CitizenPathfindSetup
  {
    private EntityQuery m_LeisureProviderQuery;
    private EntityQuery m_TouristTargetQuery;
    private EntityQuery m_SchoolQuery;
    private EntityQuery m_FreeWorkplaceQuery;
    private EntityQuery m_AttractionQuery;
    private EntityQuery m_HomelessShelterQuery;
    private EntityQuery m_FindHomeQuery;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private CitySystem m_CitySystem;
    private ResourceSystem m_ResourceSystem;
    private LeisureSystem m_LeisureSystem;
    private TaxSystem m_TaxSystem;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_HealthcareParameterQuery;
    private EntityQuery m_ParkParameterQuery;
    private EntityQuery m_EducationParameterQuery;
    private EntityQuery m_TelecomParameterQuery;
    private EntityQuery m_GarbageParameterQuery;
    private EntityQuery m_PoliceParameterQuery;
    private EntityQuery m_CitizenHappinessParameterQuery;
    private EntityQuery m_ServiceFeeParameterQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
    private ComponentTypeHandle<FreeWorkplaces> m_FreeWorkplaceType;
    private ComponentTypeHandle<WorkProvider> m_WorkProviderType;
    private ComponentTypeHandle<CityServiceUpkeep> m_CityServiceType;
    private ComponentTypeHandle<Building> m_BuildingType;
    private ComponentTypeHandle<PrefabRef> m_PrefabRefType;
    private BufferTypeHandle<Renter> m_RenterType;
    private BufferTypeHandle<Game.Buildings.Student> m_StudentType;
    private BufferTypeHandle<InstalledUpgrade> m_UpgradeType;
    private ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
    private ComponentLookup<ServiceCompanyData> m_ServiceDatas;
    private ComponentLookup<Building> m_Buildings;
    private ComponentLookup<Household> m_Households;
    private ComponentLookup<Worker> m_Workers;
    private ComponentLookup<Game.Citizens.Student> m_Students;
    private ComponentLookup<Citizen> m_Citizens;
    private ComponentLookup<HealthProblem> m_HealthProblems;
    private ComponentLookup<TouristHousehold> m_TouristHouseholds;
    private BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
    private ComponentLookup<Game.Objects.Transform> m_Transforms;
    private ComponentLookup<Building> m_BuildingDatas;
    private BufferLookup<Efficiency> m_Efficiencies;
    private ComponentLookup<LodgingProvider> m_LodgingProviders;
    private ComponentLookup<AttractivenessProvider> m_AttractivenessProviders;
    private ComponentLookup<PropertyOnMarket> m_PropertiesOnMarket;
    private ComponentLookup<PropertyRenter> m_PropertyRenters;
    private ComponentLookup<CrimeProducer> m_Crimes;
    private ComponentLookup<Game.Buildings.Park> m_Parks;
    private ComponentLookup<Abandoned> m_Abandoneds;
    private ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
    private ComponentLookup<WaterConsumer> m_WaterConsumers;
    private ComponentLookup<GarbageProducer> m_GarbageProducers;
    private ComponentLookup<MailProducer> m_MailProducers;
    private ComponentLookup<PathInformation> m_PathInfos;
    private ComponentLookup<PrefabRef> m_Prefabs;
    private ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
    private ComponentLookup<LeisureProviderData> m_LeisureProviderDatas;
    private ComponentLookup<ResourceData> m_ResourceDatas;
    private ComponentLookup<SchoolData> m_SchoolDatas;
    private ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingDatas;
    private ComponentLookup<BuildingPropertyData> m_BuildingProperties;
    private ComponentLookup<SpawnableBuildingData> m_SpawnableDatas;
    private ComponentLookup<Locked> m_Lockeds;
    private BufferLookup<Game.Economy.Resources> m_Resources;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private BufferLookup<ResourceAvailability> m_Availabilities;
    private BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
    private BufferLookup<Renter> m_Renters;
    private BufferLookup<CityModifier> m_CityModifiers;
    private BufferLookup<OwnedVehicle> m_OwnedVehicles;

    public CitizenPathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_LeisureProviderQuery = system.GetSetupQuery(ComponentType.ReadOnly<Game.Buildings.LeisureProvider>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>());
      // ISSUE: reference to a compiler-generated method
      this.m_TouristTargetQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[0],
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<LodgingProvider>(),
          ComponentType.ReadOnly<AttractivenessProvider>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_SchoolQuery = system.GetSetupQuery(ComponentType.ReadOnly<Game.Buildings.School>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Game.Buildings.ServiceUpgrade>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated method
      this.m_FreeWorkplaceQuery = system.GetSetupQuery(ComponentType.ReadOnly<FreeWorkplaces>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated method
      this.m_AttractionQuery = system.GetSetupQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<AttractivenessProvider>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      EntityQueryDesc entityQueryDesc1 = new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Building>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Abandoned>(),
          ComponentType.ReadOnly<Game.Buildings.Park>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      };
      // ISSUE: reference to a compiler-generated method
      this.m_HomelessShelterQuery = system.GetSetupQuery(entityQueryDesc1);
      EntityQueryDesc entityQueryDesc2 = new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PropertyOnMarket>(),
          ComponentType.ReadOnly<ResidentialProperty>(),
          ComponentType.ReadOnly<Building>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<Abandoned>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Condemned>()
        }
      };
      // ISSUE: reference to a compiler-generated method
      this.m_FindHomeQuery = system.GetSetupQuery(entityQueryDesc1, entityQueryDesc2);
      this.m_GroundPollutionSystem = system.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      this.m_AirPollutionSystem = system.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      this.m_NoisePollutionSystem = system.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      this.m_TelecomCoverageSystem = system.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      this.m_CitySystem = system.World.GetOrCreateSystemManaged<CitySystem>();
      this.m_ResourceSystem = system.World.GetOrCreateSystemManaged<ResourceSystem>();
      this.m_LeisureSystem = system.World.GetOrCreateSystemManaged<LeisureSystem>();
      this.m_TaxSystem = system.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated method
      this.m_EconomyParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_HealthcareParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_ParkParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<ParkParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_EducationParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<EducationParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_TelecomParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<TelecomParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_GarbageParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<GarbageParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_PoliceParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated method
      this.m_CitizenHappinessParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<CitizenHappinessParameterData>());
      // ISSUE: reference to a compiler-generated method
      this.m_ServiceFeeParameterQuery = system.GetSetupQuery(ComponentType.ReadOnly<ServiceFeeParameterData>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_ServiceAvailableType = system.GetComponentTypeHandle<ServiceAvailable>(true);
      this.m_FreeWorkplaceType = system.GetComponentTypeHandle<FreeWorkplaces>(true);
      this.m_WorkProviderType = system.GetComponentTypeHandle<WorkProvider>(true);
      this.m_CityServiceType = system.GetComponentTypeHandle<CityServiceUpkeep>(true);
      this.m_BuildingType = system.GetComponentTypeHandle<Building>(true);
      this.m_PrefabRefType = system.GetComponentTypeHandle<PrefabRef>(true);
      this.m_RenterType = system.GetBufferTypeHandle<Renter>(true);
      this.m_StudentType = system.GetBufferTypeHandle<Game.Buildings.Student>(true);
      this.m_UpgradeType = system.GetBufferTypeHandle<InstalledUpgrade>(true);
      this.m_Buildings = system.GetComponentLookup<Building>(true);
      this.m_Households = system.GetComponentLookup<Household>(true);
      this.m_OutsideConnections = system.GetComponentLookup<Game.Objects.OutsideConnection>(true);
      this.m_ServiceDatas = system.GetComponentLookup<ServiceCompanyData>(true);
      this.m_Workers = system.GetComponentLookup<Worker>(true);
      this.m_Students = system.GetComponentLookup<Game.Citizens.Student>(true);
      this.m_Citizens = system.GetComponentLookup<Citizen>(true);
      this.m_TouristHouseholds = system.GetComponentLookup<TouristHousehold>(true);
      this.m_HealthProblems = system.GetComponentLookup<HealthProblem>(true);
      this.m_Transforms = system.GetComponentLookup<Game.Objects.Transform>(true);
      this.m_BuildingDatas = system.GetComponentLookup<Building>(true);
      this.m_Efficiencies = system.GetBufferLookup<Efficiency>(true);
      this.m_AttractivenessProviders = system.GetComponentLookup<AttractivenessProvider>(true);
      this.m_LodgingProviders = system.GetComponentLookup<LodgingProvider>(true);
      this.m_PropertiesOnMarket = system.GetComponentLookup<PropertyOnMarket>(true);
      this.m_PropertyRenters = system.GetComponentLookup<PropertyRenter>(true);
      this.m_Crimes = system.GetComponentLookup<CrimeProducer>(true);
      this.m_Parks = system.GetComponentLookup<Game.Buildings.Park>(true);
      this.m_Abandoneds = system.GetComponentLookup<Abandoned>(true);
      this.m_ElectricityConsumers = system.GetComponentLookup<ElectricityConsumer>(true);
      this.m_WaterConsumers = system.GetComponentLookup<WaterConsumer>(true);
      this.m_GarbageProducers = system.GetComponentLookup<GarbageProducer>(true);
      this.m_MailProducers = system.GetComponentLookup<MailProducer>(true);
      this.m_PathInfos = system.GetComponentLookup<PathInformation>(true);
      this.m_Prefabs = system.GetComponentLookup<PrefabRef>(true);
      this.m_IndustrialProcessDatas = system.GetComponentLookup<IndustrialProcessData>(true);
      this.m_LeisureProviderDatas = system.GetComponentLookup<LeisureProviderData>(true);
      this.m_ResourceDatas = system.GetComponentLookup<ResourceData>(true);
      this.m_SchoolDatas = system.GetComponentLookup<SchoolData>(true);
      this.m_PrefabBuildingDatas = system.GetComponentLookup<Game.Prefabs.BuildingData>(true);
      this.m_BuildingProperties = system.GetComponentLookup<BuildingPropertyData>(true);
      this.m_SpawnableDatas = system.GetComponentLookup<SpawnableBuildingData>(true);
      this.m_Lockeds = system.GetComponentLookup<Locked>(true);
      this.m_Resources = system.GetBufferLookup<Game.Economy.Resources>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_Availabilities = system.GetBufferLookup<ResourceAvailability>(true);
      this.m_ServiceCoverages = system.GetBufferLookup<Game.Net.ServiceCoverage>(true);
      this.m_Renters = system.GetBufferLookup<Renter>(true);
      this.m_CityModifiers = system.GetBufferLookup<CityModifier>(true);
      this.m_OwnedVehicles = system.GetBufferLookup<OwnedVehicle>(true);
      this.m_HouseholdCitizens = system.GetBufferLookup<HouseholdCitizen>(true);
    }

    public JobHandle SetupLeisureTarget(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceAvailableType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_LeisureProviderDatas.Update((SystemBase) system);
      this.m_Resources.Update((SystemBase) system);
      this.m_IndustrialProcessDatas.Update((SystemBase) system);
      this.m_ResourceDatas.Update((SystemBase) system);
      this.m_ServiceDatas.Update((SystemBase) system);
      this.m_BuildingDatas.Update((SystemBase) system);
      // ISSUE: reference to a compiler-generated method
      JobHandle handle = new CitizenPathfindSetup.SetupLeisureTargetJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceAvailableType = this.m_ServiceAvailableType,
        m_PrefabType = this.m_PrefabRefType,
        m_LeisureProviderDatas = this.m_LeisureProviderDatas,
        m_Resources = this.m_Resources,
        m_IndustrialProcessDatas = this.m_IndustrialProcessDatas,
        m_ResourceDatas = this.m_ResourceDatas,
        m_ServiceDatas = this.m_ServiceDatas,
        m_BuildingDatas = this.m_BuildingDatas,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_SetupData = setupData,
        m_LeisureSystemUpdateInterval = this.m_LeisureSystem.GetUpdateInterval(SystemUpdatePhase.GameSimulation)
      }.ScheduleParallel<CitizenPathfindSetup.SetupLeisureTargetJob>(this.m_LeisureProviderQuery, inputDeps);
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(handle);
      return handle;
    }

    public JobHandle SetupTouristTarget(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_TouristHouseholds.Update((SystemBase) system);
      this.m_LodgingProviders.Update((SystemBase) system);
      this.m_PropertyRenters.Update((SystemBase) system);
      this.m_BuildingDatas.Update((SystemBase) system);
      this.m_Availabilities.Update((SystemBase) system);
      return new CitizenPathfindSetup.SetupTouristTargetJob()
      {
        m_EntityType = this.m_EntityType,
        m_LodgingProviders = this.m_LodgingProviders,
        m_TouristHouseholds = this.m_TouristHouseholds,
        m_PropertyRenters = this.m_PropertyRenters,
        m_BuildingDatas = this.m_BuildingDatas,
        m_ResourceAvailabilityBufs = this.m_Availabilities,
        m_SetupData = setupData
      }.ScheduleParallel<CitizenPathfindSetup.SetupTouristTargetJob>(this.m_TouristTargetQuery, inputDeps);
    }

    public JobHandle SetupSchoolSeekerTo(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_StudentType.Update((SystemBase) system);
      this.m_UpgradeType.Update((SystemBase) system);
      this.m_SchoolDatas.Update((SystemBase) system);
      this.m_Efficiencies.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new CitizenPathfindSetup.SetupSchoolSeekerToJob()
      {
        m_EntityType = this.m_EntityType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_StudentType = this.m_StudentType,
        m_UpgradeType = this.m_UpgradeType,
        m_SchoolDatas = this.m_SchoolDatas,
        m_Efficiencies = this.m_Efficiencies,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<CitizenPathfindSetup.SetupSchoolSeekerToJob>(this.m_SchoolQuery, inputDeps);
    }

    public JobHandle SetupJobSeekerTo(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_FreeWorkplaceType.Update((SystemBase) system);
      this.m_WorkProviderType.Update((SystemBase) system);
      this.m_CityServiceType.Update((SystemBase) system);
      this.m_OutsideConnections.Update((SystemBase) system);
      return new CitizenPathfindSetup.SetupJobSeekerToJob()
      {
        m_EntityType = this.m_EntityType,
        m_FreeWorkplaceType = this.m_FreeWorkplaceType,
        m_WorkProviderType = this.m_WorkProviderType,
        m_CityServiceType = this.m_CityServiceType,
        m_OutsideConnections = this.m_OutsideConnections,
        m_SetupData = setupData
      }.ScheduleParallel<CitizenPathfindSetup.SetupJobSeekerToJob>(this.m_FreeWorkplaceQuery, inputDeps);
    }

    public JobHandle SetupHomeless(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_RenterType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_BuildingType.Update((SystemBase) system);
      this.m_PrefabBuildingDatas.Update((SystemBase) system);
      this.m_ServiceCoverages.Update((SystemBase) system);
      return new CitizenPathfindSetup.SetupHomelessJob()
      {
        m_EntityType = this.m_EntityType,
        m_RenterType = this.m_RenterType,
        m_PrefabType = this.m_PrefabRefType,
        m_BuildingType = this.m_BuildingType,
        m_BuildingProperties = this.m_BuildingProperties,
        m_BuildingDatas = this.m_PrefabBuildingDatas,
        m_Coverages = this.m_ServiceCoverages,
        m_SetupData = setupData
      }.ScheduleParallel<CitizenPathfindSetup.SetupHomelessJob>(this.m_HomelessShelterQuery, inputDeps);
    }

    public JobHandle SetupFindHome(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_RenterType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_BuildingType.Update((SystemBase) system);
      this.m_Buildings.Update((SystemBase) system);
      this.m_Households.Update((SystemBase) system);
      this.m_PrefabBuildingDatas.Update((SystemBase) system);
      this.m_ServiceCoverages.Update((SystemBase) system);
      this.m_PropertiesOnMarket.Update((SystemBase) system);
      this.m_Availabilities.Update((SystemBase) system);
      this.m_SpawnableDatas.Update((SystemBase) system);
      this.m_BuildingProperties.Update((SystemBase) system);
      this.m_BuildingDatas.Update((SystemBase) system);
      this.m_PathInfos.Update((SystemBase) system);
      this.m_Prefabs.Update((SystemBase) system);
      this.m_Renters.Update((SystemBase) system);
      this.m_ServiceCoverages.Update((SystemBase) system);
      this.m_Workers.Update((SystemBase) system);
      this.m_Students.Update((SystemBase) system);
      this.m_PropertyRenters.Update((SystemBase) system);
      this.m_ResourceDatas.Update((SystemBase) system);
      this.m_Citizens.Update((SystemBase) system);
      this.m_Crimes.Update((SystemBase) system);
      this.m_Lockeds.Update((SystemBase) system);
      this.m_Transforms.Update((SystemBase) system);
      this.m_CityModifiers.Update((SystemBase) system);
      this.m_HealthProblems.Update((SystemBase) system);
      this.m_HouseholdCitizens.Update((SystemBase) system);
      this.m_OwnedVehicles.Update((SystemBase) system);
      this.m_Abandoneds.Update((SystemBase) system);
      this.m_Parks.Update((SystemBase) system);
      this.m_ElectricityConsumers.Update((SystemBase) system);
      this.m_WaterConsumers.Update((SystemBase) system);
      this.m_GarbageProducers.Update((SystemBase) system);
      this.m_MailProducers.Update((SystemBase) system);
      this.m_Resources.Update((SystemBase) system);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated method
      return new CitizenPathfindSetup.SetupFindHomeJob()
      {
        m_EntityType = this.m_EntityType,
        m_RenterType = this.m_RenterType,
        m_PrefabType = this.m_PrefabRefType,
        m_Buildings = this.m_Buildings,
        m_Households = this.m_Households,
        m_BuildingDatas = this.m_PrefabBuildingDatas,
        m_Coverages = this.m_ServiceCoverages,
        m_PropertiesOnMarket = this.m_PropertiesOnMarket,
        m_Availabilities = this.m_Availabilities,
        m_SpawnableDatas = this.m_SpawnableDatas,
        m_BuildingProperties = this.m_BuildingProperties,
        m_PrefabRefs = this.m_Prefabs,
        m_ServiceCoverages = this.m_ServiceCoverages,
        m_Citizens = this.m_Citizens,
        m_Crimes = this.m_Crimes,
        m_Lockeds = this.m_Lockeds,
        m_Transforms = this.m_Transforms,
        m_CityModifiers = this.m_CityModifiers,
        m_HouseholdCitizens = this.m_HouseholdCitizens,
        m_Abandoneds = this.m_Abandoneds,
        m_Parks = this.m_Parks,
        m_ElectricityConsumers = this.m_ElectricityConsumers,
        m_WaterConsumers = this.m_WaterConsumers,
        m_GarbageProducers = this.m_GarbageProducers,
        m_MailProducers = this.m_MailProducers,
        m_HealthProblems = this.m_HealthProblems,
        m_Workers = this.m_Workers,
        m_Students = this.m_Students,
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_PollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies1),
        m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(true, out dependencies2),
        m_NoiseMap = this.m_NoisePollutionSystem.GetMap(true, out dependencies3),
        m_TelecomCoverages = this.m_TelecomCoverageSystem.GetData(true, out dependencies4),
        m_HealthcareParameters = this.m_HealthcareParameterQuery.GetSingleton<HealthcareParameterData>(),
        m_ParkParameters = this.m_ParkParameterQuery.GetSingleton<ParkParameterData>(),
        m_EducationParameters = this.m_EducationParameterQuery.GetSingleton<EducationParameterData>(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_TelecomParameters = this.m_TelecomParameterQuery.GetSingleton<TelecomParameterData>(),
        m_GarbageParameters = this.m_GarbageParameterQuery.GetSingleton<GarbageParameterData>(),
        m_PoliceParameters = this.m_PoliceParameterQuery.GetSingleton<PoliceConfigurationData>(),
        m_ServiceFeeParameterData = this.m_ServiceFeeParameterQuery.GetSingleton<ServiceFeeParameterData>(),
        m_CitizenHappinessParameterData = this.m_CitizenHappinessParameterQuery.GetSingleton<CitizenHappinessParameterData>(),
        m_City = this.m_CitySystem.City,
        m_SetupData = setupData
      }.ScheduleParallel<CitizenPathfindSetup.SetupFindHomeJob>(this.m_FindHomeQuery, JobUtils.CombineDependencies(inputDeps, dependencies1, dependencies2, dependencies3, dependencies4));
    }

    public JobHandle SetupAttraction(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_AttractivenessProviders.Update((SystemBase) system);
      return new CitizenPathfindSetup.SetupAttractionJob()
      {
        m_EntityType = this.m_EntityType,
        m_AttractivenessProviders = this.m_AttractivenessProviders,
        m_SetupData = setupData
      }.ScheduleParallel<CitizenPathfindSetup.SetupAttractionJob>(this.m_AttractionQuery, inputDeps);
    }

    [BurstCompile]
    private struct SetupTouristTargetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentLookup<LodgingProvider> m_LodgingProviders;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_ResourceAvailabilityBufs;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out targetSeeker);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Entity entity2 = nativeArray[index2];
            Entity entity3 = entity2;
            float num = 0.0f;
            bool flag = this.m_TouristHouseholds.HasComponent(entity1) && this.m_TouristHouseholds[entity1].m_Hotel == Entity.Null;
            float cost;
            if (this.m_LodgingProviders.HasComponent(entity2) & flag)
            {
              if (this.m_LodgingProviders[entity2].m_FreeRooms != 0 && this.m_PropertyRenters.HasComponent(entity2) && !(this.m_PropertyRenters[entity2].m_Property == Entity.Null))
              {
                entity3 = this.m_PropertyRenters[entity2].m_Property;
                cost = num - 5000f + -10f * (float) this.m_LodgingProviders[entity2].m_FreeRooms + math.min((float) this.m_LodgingProviders[entity2].m_Price, 500f);
              }
              else
                continue;
            }
            else
              cost = num + 5000f;
            if (this.m_BuildingDatas.HasComponent(entity3))
            {
              Building buildingData = this.m_BuildingDatas[entity3];
              if (!BuildingUtils.CheckOption(buildingData, BuildingOption.Inactive))
              {
                if (this.m_ResourceAvailabilityBufs.HasBuffer(buildingData.m_RoadEdge))
                {
                  float availability = NetUtils.GetAvailability(this.m_ResourceAvailabilityBufs[buildingData.m_RoadEdge], AvailableResource.Attractiveness, buildingData.m_CurvePosition);
                  cost -= availability * 0.01f;
                }
                targetSeeker.FindTargets(entity3, cost);
              }
            }
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

    [BurstCompile]
    private struct SetupLeisureTargetJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<LeisureProviderData> m_LeisureProviderDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceDatas;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingDatas;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public PathfindSetupSystem.SetupData m_SetupData;
      public int m_LeisureSystemUpdateInterval;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<ServiceAvailable> nativeArray2 = chunk.GetNativeArray<ServiceAvailable>(ref this.m_ServiceAvailableType);
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          LeisureType leisureType = (LeisureType) targetSeeker.m_SetupQueueTarget.m_Value;
          float num1 = targetSeeker.m_SetupQueueTarget.m_Value2;
          float num2 = targetSeeker.m_PathfindParameters.m_Weights.time * 0.01f;
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            if (!this.m_BuildingDatas.HasComponent(entity) || !BuildingUtils.CheckOption(this.m_BuildingDatas[entity], BuildingOption.Inactive))
            {
              Entity prefab = nativeArray3[index2].m_Prefab;
              if (this.m_LeisureProviderDatas.HasComponent(prefab))
              {
                LeisureProviderData leisureProviderData = this.m_LeisureProviderDatas[prefab];
                float num3 = 0.0f;
                if (leisureType == leisureProviderData.m_LeisureType)
                {
                  if ((leisureType == LeisureType.Commercial || leisureType == LeisureType.Meals) && nativeArray2.Length > 0 && this.m_ServiceDatas.HasComponent(prefab))
                  {
                    int serviceAvailable1 = nativeArray2[index2].m_ServiceAvailable;
                    if (this.m_IndustrialProcessDatas.HasComponent(prefab))
                    {
                      IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab];
                      if (industrialProcessData.m_Output.m_Resource != Resource.NoResource)
                      {
                        int serviceAvailable2 = math.min(serviceAvailable1, EconomyUtils.GetResources(industrialProcessData.m_Output.m_Resource, this.m_Resources[entity]));
                        float marketPrice = EconomyUtils.GetMarketPrice(this.m_ResourceDatas[this.m_ResourcePrefabs[industrialProcessData.m_Output.m_Resource]]);
                        num3 = (float) (0.20000000298023224 * (double) num1 * (double) marketPrice * ((double) EconomyUtils.GetServicePriceMultiplier((float) serviceAvailable2, this.m_ServiceDatas[prefab].m_MaxService) - 1.0));
                      }
                    }
                  }
                  float num4 = (float) ((double) num1 * (double) this.m_LeisureSystemUpdateInterval * 16.0) / (float) leisureProviderData.m_Efficiency;
                  float cost = num3 + num2 * num4;
                  targetSeeker.FindTargets(entity, cost);
                }
              }
            }
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

    [BurstCompile]
    private struct SetupSchoolSeekerToJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Buildings.Student> m_StudentType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_UpgradeType;
      [ReadOnly]
      public ComponentLookup<SchoolData> m_SchoolDatas;
      [ReadOnly]
      public BufferLookup<Efficiency> m_Efficiencies;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        BufferAccessor<Game.Buildings.Student> bufferAccessor1 = chunk.GetBufferAccessor<Game.Buildings.Student>(ref this.m_StudentType);
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_UpgradeType);
        bool flag1 = bufferAccessor2.Length != 0;
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out targetSeeker);
          int num1 = targetSeeker.m_SetupQueueTarget.m_Value;
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity2 = nativeArray1[index2];
            if (AreaUtils.CheckServiceDistrict(entity1, entity2, this.m_ServiceDistricts))
            {
              Entity prefab = nativeArray2[index2].m_Prefab;
              if (this.m_SchoolDatas.HasComponent(prefab))
              {
                SchoolData schoolData = this.m_SchoolDatas[prefab];
                if (flag1)
                  UpgradeUtils.CombineStats<SchoolData>(ref schoolData, bufferAccessor2[index2], ref targetSeeker.m_PrefabRef, ref this.m_SchoolDatas);
                int studentCapacity = schoolData.m_StudentCapacity;
                DynamicBuffer<Efficiency> bufferData;
                if (this.m_Efficiencies.TryGetBuffer(entity2, out bufferData))
                  studentCapacity = Mathf.RoundToInt((float) studentCapacity * math.min(1f, BuildingUtils.GetEfficiency(bufferData)));
                bool flag2 = schoolData.m_EducationLevel == (byte) 5;
                int num2 = studentCapacity - bufferAccessor1[index2].Length;
                if ((flag2 && num1 > 1 || (int) schoolData.m_EducationLevel == num1) && num2 > 0)
                  targetSeeker.FindTargets(entity2, math.max(0.0f, (float) -num2 + (flag2 ? 5000f : 0.0f)));
              }
            }
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

    [BurstCompile]
    private struct SetupJobSeekerToJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<FreeWorkplaces> m_FreeWorkplaceType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public ComponentTypeHandle<CityServiceUpkeep> m_CityServiceType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<FreeWorkplaces> nativeArray2 = chunk.GetNativeArray<FreeWorkplaces>(ref this.m_FreeWorkplaceType);
        NativeArray<WorkProvider> nativeArray3 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        float num1 = chunk.Has<CityServiceUpkeep>(ref this.m_CityServiceType) ? -4000f : 0.0f;
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          Unity.Mathematics.Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          int level = targetSeeker.m_SetupQueueTarget.m_Value % 5;
          int num2 = targetSeeker.m_SetupQueueTarget.m_Value / 5 - 1;
          float num3 = targetSeeker.m_SetupQueueTarget.m_Value2;
          SetupTargetFlags flags = targetSeeker.m_SetupQueueTarget.m_Flags;
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            FreeWorkplaces freeWorkplaces = nativeArray2[index2];
            if ((flags & SetupTargetFlags.Export) != SetupTargetFlags.None)
            {
              if (freeWorkplaces.GetFree(level) > (byte) 0 && !this.m_OutsideConnections.HasComponent(entity))
                targetSeeker.FindTargets(entity, 2000f);
            }
            else if ((flags & SetupTargetFlags.Import) == SetupTargetFlags.None || !this.m_OutsideConnections.HasComponent(entity))
            {
              int lowestFree = (int) freeWorkplaces.GetLowestFree();
              if (level >= lowestFree && level >= num2)
              {
                int bestFor = freeWorkplaces.GetBestFor(level);
                int maxWorkers = nativeArray3[index2].m_MaxWorkers;
                if (freeWorkplaces.Count > 0 && maxWorkers > 0)
                {
                  float num4 = (float) freeWorkplaces.Count / (float) maxWorkers;
                  int num5 = random.NextInt(4000);
                  int num6 = this.m_OutsideConnections.HasComponent(entity) ? 8000 : -4000;
                  targetSeeker.FindTargets(entity, (float) (6000.0 * (1.0 - (double) num4) + (double) math.max(0.0f, 2f - num3) * 4000.0 * (double) (level - bestFor)) + num1 + (float) num5 + (float) num6);
                }
              }
            }
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

    [BurstCompile]
    private struct SetupAttractionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentLookup<AttractivenessProvider> m_AttractivenessProviders;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          Unity.Mathematics.Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Entity entity = nativeArray[index2];
            if (this.m_AttractivenessProviders.HasComponent(entity))
            {
              AttractivenessProvider attractivenessProvider = this.m_AttractivenessProviders[entity];
              targetSeeker.FindTargets(entity, -100f * (float) attractivenessProvider.m_Attractiveness * random.NextFloat());
            }
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

    [BurstCompile]
    private struct SetupHomelessJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingProperties;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_Coverages;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          Unity.Mathematics.Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            Entity prefab = nativeArray2[index2].m_Prefab;
            Building building = nativeArray3[index2];
            if (building.m_RoadEdge != Entity.Null && this.m_Coverages.HasBuffer(building.m_RoadEdge) && this.m_BuildingDatas.HasComponent(prefab))
            {
              float serviceCoverage = NetUtils.GetServiceCoverage(this.m_Coverages[building.m_RoadEdge], CoverageService.Police, building.m_CurvePosition);
              float shelterCapacity = (float) HomelessShelterAISystem.GetShelterCapacity(this.m_BuildingDatas[prefab], this.m_BuildingProperties.HasComponent(prefab) ? this.m_BuildingProperties[prefab] : new BuildingPropertyData());
              targetSeeker.FindTargets(entity, (float) (100.0 * (double) serviceCoverage + 1000.0 * ((double) bufferAccessor[index2].Length / (double) shelterCapacity)) + random.NextFloat(1000f));
            }
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

    [BurstCompile]
    private struct SetupFindHomeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Renter> m_RenterType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_Coverages;
      public PathfindSetupSystem.SetupData m_SetupData;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> m_PropertiesOnMarket;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<BuildingPropertyData> m_BuildingProperties;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableDatas;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_Availabilities;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> m_Students;
      [ReadOnly]
      public ComponentLookup<CrimeProducer> m_Crimes;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_Transforms;
      [ReadOnly]
      public ComponentLookup<Locked> m_Lockeds;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_Parks;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_Abandoneds;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumers;
      [ReadOnly]
      public ComponentLookup<GarbageProducer> m_GarbageProducers;
      [ReadOnly]
      public ComponentLookup<MailProducer> m_MailProducers;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<GroundPollution> m_PollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoiseMap;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverages;
      public HealthcareParameterData m_HealthcareParameters;
      public ParkParameterData m_ParkParameters;
      public EducationParameterData m_EducationParameters;
      public EconomyParameterData m_EconomyParameters;
      public TelecomParameterData m_TelecomParameters;
      public GarbageParameterData m_GarbageParameters;
      public PoliceConfigurationData m_PoliceParameters;
      public ServiceFeeParameterData m_ServiceFeeParameterData;
      public CitizenHappinessParameterData m_CitizenHappinessParameterData;
      [ReadOnly]
      public Entity m_City;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        BufferAccessor<Renter> bufferAccessor = chunk.GetBufferAccessor<Renter>(ref this.m_RenterType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          Entity entity1 = targetSeeker.m_SetupQueueTarget.m_Entity;
          DynamicBuffer<HouseholdCitizen> bufferData;
          if (this.m_HouseholdCitizens.TryGetBuffer(entity1, out bufferData))
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity2 = nativeArray1[index2];
              Entity prefab = nativeArray2[index2].m_Prefab;
              Building building = this.m_Buildings[entity2];
              if (building.m_RoadEdge != Entity.Null && this.m_Coverages.HasBuffer(building.m_RoadEdge) && this.m_BuildingDatas.HasComponent(prefab))
              {
                if (BuildingUtils.IsHomelessShelterBuilding(entity2, ref this.m_Parks, ref this.m_Abandoneds))
                {
                  float serviceCoverage = NetUtils.GetServiceCoverage(this.m_Coverages[building.m_RoadEdge], CoverageService.Police, building.m_CurvePosition);
                  int shelterCapacity = HomelessShelterAISystem.GetShelterCapacity(this.m_BuildingDatas[prefab], this.m_BuildingProperties.HasComponent(prefab) ? this.m_BuildingProperties[prefab] : new BuildingPropertyData());
                  if (bufferAccessor[index2].Length < shelterCapacity)
                    targetSeeker.FindTargets(entity2, (float) (100.0 * (double) serviceCoverage + 1000.0 * (double) bufferAccessor[index2].Length / (double) shelterCapacity + 1000.0));
                }
                else
                {
                  int askingRent = this.m_PropertiesOnMarket[entity2].m_AskingRent;
                  int x = this.m_ServiceFeeParameterData.m_GarbageFeeRCIO.x;
                  int householdIncome = EconomyUtils.GetHouseholdIncome(bufferData, ref this.m_Workers, ref this.m_Citizens, ref this.m_HealthProblems, ref this.m_EconomyParameters, this.m_TaxRates);
                  if (CitizenUtils.IsHouseholdNeedSupport(bufferData, ref this.m_Citizens, ref this.m_Students) || askingRent + x <= householdIncome)
                  {
                    float propertyScore = PropertyUtils.GetPropertyScore(entity2, entity1, bufferData, ref this.m_PrefabRefs, ref this.m_BuildingProperties, ref this.m_Buildings, ref this.m_BuildingDatas, ref this.m_Households, ref this.m_Citizens, ref this.m_Students, ref this.m_Workers, ref this.m_SpawnableDatas, ref this.m_Crimes, ref this.m_ServiceCoverages, ref this.m_Lockeds, ref this.m_ElectricityConsumers, ref this.m_WaterConsumers, ref this.m_GarbageProducers, ref this.m_MailProducers, ref this.m_Transforms, ref this.m_Abandoneds, ref this.m_Parks, ref this.m_Availabilities, this.m_TaxRates, this.m_PollutionMap, this.m_AirPollutionMap, this.m_NoiseMap, this.m_TelecomCoverages, this.m_CityModifiers[this.m_City], this.m_HealthcareParameters.m_HealthcareServicePrefab, this.m_ParkParameters.m_ParkServicePrefab, this.m_EducationParameters.m_EducationServicePrefab, this.m_TelecomParameters.m_TelecomServicePrefab, this.m_GarbageParameters.m_GarbageServicePrefab, this.m_PoliceParameters.m_PoliceServicePrefab, this.m_CitizenHappinessParameterData, this.m_GarbageParameters);
                    targetSeeker.FindTargets(entity2, -propertyScore + (float) askingRent + (float) x - (float) householdIncome);
                  }
                }
              }
            }
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
