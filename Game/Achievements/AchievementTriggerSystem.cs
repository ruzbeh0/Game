// Decompiled with JetBrains decompiler
// Type: Game.Achievements.AchievementTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Entities;
using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Objects;
using Game.Policies;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Routes;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Achievements
{
  [CompilerGenerated]
  public class AchievementTriggerSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private static ILog sLog = LogManager.GetLogger("Platforms");
    private ToolSystem m_ToolSystem;
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private SimulationSystem m_SimulationSystem;
    private ClimateSystem m_ClimateSystem;
    private TimeSystem m_TimeSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_CreatedObjectQuery;
    private EntityQuery m_UnlockQuery;
    private EntityQuery m_ParkQuery;
    private EntityQuery m_CreatedParkQuery;
    private EntityQuery m_LockedServiceQuery;
    private EntityQuery m_ServiceQuery;
    private EntityQuery m_LockedBuildingQuery;
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_TransportLineQuery;
    private EntityQuery m_CreatedTransportLineQuery;
    private EntityQuery m_UniqueServiceBuildingPrefabQuery;
    private EntityQuery m_UniqueServiceBuildingQuery;
    private EntityQuery m_CreatedUniqueServiceBuildingQuery;
    private EntityQuery m_PolicyModificationQuery;
    private EntityQuery m_DistrictQuery;
    private EntityQuery m_ServiceDistrictBuildingQuery;
    private EntityQuery m_FossilEnergyProducersQuery;
    private EntityQuery m_RenewableEnergyProducersQuery;
    private EntityQuery m_EnergyProducersQuery;
    private EntityQuery m_WaterPumpingStationQuery;
    private EntityQuery m_ResidentialBuildingsQuery;
    private EntityQuery m_CommercialBuildingsQuery;
    private EntityQuery m_IndustrialBuildingsQuery;
    private EntityQuery m_FollowedCitizensQuery;
    private EntityQuery m_InfoviewQuery;
    private EntityQuery m_CreatedUniqueBuildingQuery;
    private EntityQuery m_UniqueBuildingQuery;
    private EntityQuery m_PlantQuery;
    private EntityQuery m_CreatedPlantQuery;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_TimeSettingsQuery;
    public NativeCounter m_PatientsTreatedCounter;
    private int m_CachedPatientsTreatedCount;
    private int m_CachedPopulationCount;
    private int m_CachedHappiness;
    private int m_CachedAttractiveness;
    private int m_CachedTouristCount;
    private bool m_CheckUnlocks;
    private uint m_LastCheckFrameIndex;
    private static readonly int kMinCityEffectPopulation = 1000;
    private static readonly int kAllSmilesHappiness = 75;
    private static readonly int kThisIsNotMyHappyPlaceHappiness = 25;
    private static readonly int kSimplyIrresistibleAttractiveness = 90;
    private static readonly int kZeroEmissionMinProduction = 500000;
    private static readonly int kColossalGardenerLimit = 100;
    private static readonly int kTheDeepEndLoanAmount = 200000;
    private HashSet<InfoviewPrefab> m_ViewedInfoviews = new HashSet<InfoviewPrefab>();
    public AchievementTriggerSystem.ProgressBuffer m_LittleBitOfTLCBuffer;
    public AchievementTriggerSystem.UserDataProgressBuffer m_SquasherDownerBuffer;
    private AchievementTriggerSystem.TypeHandle __TypeHandle;

    public bool GetDebugData(AchievementId achievement, out string data)
    {
      if (achievement == Game.Achievements.Achievements.ALittleBitofTLC)
      {
        ref string local = ref data;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AchievementTriggerSystem.ProgressBuffer littleBitOfTlcBuffer = this.m_LittleBitOfTLCBuffer;
        // ISSUE: reference to a compiler-generated field
        string str = string.Format("{0}", (object) (littleBitOfTlcBuffer != null ? littleBitOfTlcBuffer.m_Progress : 0));
        local = str;
        return true;
      }
      if (achievement == Game.Achievements.Achievements.OneofEverything)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        data = string.Format("{0}/{1}", (object) this.CountUniqueServiceBuildings(), (object) this.CountUniqueServiceBuildingPrefabs());
        return true;
      }
      data = string.Empty;
      return false;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_LittleBitOfTLCBuffer = new AchievementTriggerSystem.ProgressBuffer(Game.Achievements.Achievements.ALittleBitofTLC, 1000, IndicateType.Absolute);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_SquasherDownerBuffer = new AchievementTriggerSystem.UserDataProgressBuffer(Game.Achievements.Achievements.SquasherDowner, 10, IndicateType.Increment, "SquasherDowner");
      // ISSUE: reference to a compiler-generated field
      this.m_PatientsTreatedCounter = new NativeCounter(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CachedPatientsTreatedCount = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedObjectQuery = this.GetEntityQuery(ComponentType.ReadOnly<ObjectAchievement>(), ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParkQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Park>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedParkQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Park>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedServiceQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.BuildingData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.BuildingData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_TransportLineQuery = this.GetEntityQuery(ComponentType.ReadOnly<TransportLine>(), ComponentType.ReadOnly<Route>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedTransportLineQuery = this.GetEntityQuery(ComponentType.ReadOnly<TransportLine>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueServiceBuildingPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<UniqueObjectData>(), ComponentType.ReadOnly<CollectedServiceBuildingBudgetData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueServiceBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedUniqueServiceBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyModificationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Modify>());
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictQuery = this.GetEntityQuery(ComponentType.ReadOnly<District>(), ComponentType.ReadOnly<Policy>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDistrictBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ServiceDistrict>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_FossilEnergyProducersQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityProducer>(), ComponentType.Exclude<RenewableElectricityProduction>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_RenewableEnergyProducersQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityProducer>(), ComponentType.ReadOnly<RenewableElectricityProduction>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_EnergyProducersQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityProducer>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPumpingStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.WaterPumpingStation>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialBuildingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialBuildingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<CommercialProperty>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialBuildingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<IndustrialProperty>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedCitizensQuery = this.GetEntityQuery(ComponentType.ReadOnly<Followed>(), ComponentType.ReadOnly<Citizen>());
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfoviewData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UniqueBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedUniqueBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlantQuery = this.GetEntityQuery(ComponentType.ReadOnly<Plant>(), ComponentType.Exclude<Owner>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedPlantQuery = this.GetEntityQuery(ComponentType.ReadOnly<Plant>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Owner>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventInfoviewChanged += (Action<InfoviewPrefab>) (infoview =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ViewedInfoviews.Add(infoview);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ViewedInfoviews.Count != this.m_InfoviewQuery.CalculateEntityCount())
          return;
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.TheInspector);
      });
    }

    private void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CachedHappiness = 50;
      // ISSUE: reference to a compiler-generated field
      this.m_CachedAttractiveness = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_CheckUnlocks = true;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      PlatformManager instance = PlatformManager.instance;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      instance.achievementsEnabled = ((instance.achievementsEnabled ? 1 : 0) & (this.m_CityConfigurationSystem.usedMods.Count != 0 || this.m_CityConfigurationSystem.unlimitedMoney || this.m_CityConfigurationSystem.unlockAll ? 0 : (!this.m_CityConfigurationSystem.unlockMapTiles ? 1 : 0))) != 0;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor() || !PlatformManager.instance.achievementsEnabled || GameManager.instance.state == GameManager.State.Loading || !GameManager.instance.gameMode.IsGameOrEditor())
        return;
      // ISSUE: reference to a compiler-generated method
      this.CheckInGameAchievements();
    }

    private void CheckInGameAchievements()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedObjectQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> componentDataArray = this.m_CreatedObjectQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index1 = 0; index1 < componentDataArray.Length; ++index1)
        {
          DynamicBuffer<ObjectAchievementData> buffer;
          if (this.EntityManager.TryGetBuffer<ObjectAchievementData>(componentDataArray[index1].m_Prefab, true, out buffer))
          {
            for (int index2 = 0; index2 < buffer.Length; ++index2)
            {
              if (buffer[index2].m_BypassCounter)
                PlatformManager.instance.UnlockAchievement(buffer[index2].m_ID);
              else
                PlatformManager.instance.IndicateAchievementProgress(buffer[index2].m_ID, 1, IndicateType.Increment);
            }
          }
        }
        componentDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedParkQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        int num = this.CountParks();
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.Groundskeeper, num, IndicateType.Absolute);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CheckUnlocks || !this.m_UnlockQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CheckUnlocks = false;
        // ISSUE: reference to a compiler-generated method
        this.CheckUnlockingAchievements();
      }
      Loan component1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetComponent<Loan>(this.m_CitySystem.City, out component1) && component1.m_LastModified > this.m_LastCheckFrameIndex && component1.m_Amount >= AchievementTriggerSystem.kTheDeepEndLoanAmount)
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.TheDeepEnd, component1.m_Amount, IndicateType.Absolute);
      Population component2;
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetComponent<Population>(this.m_CitySystem.City, out component2))
      {
        int num1 = component2.m_Population >= 10000 ? 10000 : 1000;
        int num2 = component2.m_Population / num1 * num1;
        // ISSUE: reference to a compiler-generated field
        if (num2 != this.m_CachedPopulationCount)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CachedPopulationCount = num2;
          PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.SixFigures, num2, IndicateType.Absolute);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedTransportLineQuery.IsEmptyIgnoreFilter || !this.m_PolicyModificationQuery.IsEmptyIgnoreFilter)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        using (NativeArray<Route> componentDataArray = this.m_TransportLineQuery.ToComponentDataArray<Route>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int index = 0; index < componentDataArray.Length; ++index)
          {
            if (!RouteUtils.CheckOption(componentDataArray[index], RouteOption.Inactive))
              ++num;
          }
        }
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.GoAnywhere, num, IndicateType.Absolute);
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.Spiderwebbing, num, IndicateType.Absolute);
      }
      // ISSUE: reference to a compiler-generated field
      if (component2.m_Population >= AchievementTriggerSystem.kMinCityEffectPopulation)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CachedHappiness < AchievementTriggerSystem.kAllSmilesHappiness && component2.m_AverageHappiness >= AchievementTriggerSystem.kAllSmilesHappiness)
          PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.AllSmiles);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CachedHappiness > AchievementTriggerSystem.kThisIsNotMyHappyPlaceHappiness && component2.m_AverageHappiness <= AchievementTriggerSystem.kThisIsNotMyHappyPlaceHappiness)
          PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.ThisIsNotMyHappyPlace);
        // ISSUE: reference to a compiler-generated field
        this.m_CachedHappiness = component2.m_AverageHappiness;
        Tourism component3;
        // ISSUE: reference to a compiler-generated field
        if (this.EntityManager.TryGetComponent<Tourism>(this.m_CitySystem.City, out component3))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CachedAttractiveness < AchievementTriggerSystem.kSimplyIrresistibleAttractiveness && component3.m_Attractiveness >= AchievementTriggerSystem.kSimplyIrresistibleAttractiveness)
            PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.SimplyIrresistible);
          // ISSUE: reference to a compiler-generated field
          this.m_CachedAttractiveness = component3.m_Attractiveness;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_CreatedUniqueServiceBuildingQuery.IsEmptyIgnoreFilter && this.CheckOneOfEverything())
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.OneofEverything);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<CityStatistic> statisticRoBufferLookup = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int num3 = this.m_CityStatisticsSystem.GetStatisticValue(statisticRoBufferLookup, StatisticType.TouristCount, 0) / 1000 * 1000;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CachedTouristCount != num3)
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.WelcomeOneandAll, num3, IndicateType.Absolute);
      // ISSUE: reference to a compiler-generated field
      this.m_CachedTouristCount = num3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue1 = this.m_CityStatisticsSystem.GetStatisticValue(statisticRoBufferLookup, StatisticType.EducationCount, 0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue2 = this.m_CityStatisticsSystem.GetStatisticValue(statisticRoBufferLookup, StatisticType.EducationCount, 1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue3 = this.m_CityStatisticsSystem.GetStatisticValue(statisticRoBufferLookup, StatisticType.EducationCount, 2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue4 = this.m_CityStatisticsSystem.GetStatisticValue(statisticRoBufferLookup, StatisticType.EducationCount, 3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int statisticValue5 = this.m_CityStatisticsSystem.GetStatisticValue(statisticRoBufferLookup, StatisticType.EducationCount, 4);
      int num4 = statisticValue2;
      int num5 = statisticValue1 + num4 + statisticValue3 + statisticValue4 + statisticValue5;
      if (num5 > 0 && (double) statisticValue5 / (double) num5 >= 0.15000000596046448)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.TopoftheClass);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ServiceDistrictBuildingQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        using (NativeArray<Entity> entityArray = this.m_ServiceDistrictBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int index = 0; index < entityArray.Length; ++index)
          {
            DynamicBuffer<ServiceDistrict> buffer;
            if (this.EntityManager.TryGetBuffer<ServiceDistrict>(entityArray[index], true, out buffer) && buffer.Length > 0)
              PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.HappytobeofService);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int energyProduction1 = this.CalculateEnergyProduction(this.m_RenewableEnergyProducersQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int energyProduction2 = this.CalculateEnergyProduction(this.m_FossilEnergyProducersQuery);
      // ISSUE: reference to a compiler-generated field
      int emissionMinProduction = AchievementTriggerSystem.kZeroEmissionMinProduction;
      if (energyProduction1 >= emissionMinProduction && energyProduction2 <= 0)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.ZeroEmission);
      // ISSUE: reference to a compiler-generated field
      int entityCount1 = this.m_ResidentialBuildingsQuery.CalculateEntityCount();
      // ISSUE: reference to a compiler-generated field
      int entityCount2 = this.m_CommercialBuildingsQuery.CalculateEntityCount();
      bool flag1 = false;
      bool flag2 = false;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_IndustrialBuildingsQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<OfficeBuilding> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
      try
      {
        for (int index = 0; index < entityArray1.Length; ++index)
        {
          PrefabRef prefabRef = roComponentLookup1[entityArray1[index]];
          if (roComponentLookup2.HasComponent(prefabRef.m_Prefab))
            flag2 = true;
          else
            flag1 = true;
          if (flag1 & flag2)
            break;
        }
      }
      finally
      {
        entityArray1.Dispose();
      }
      if (flag1 & flag2 && entityCount1 > 0 && entityCount2 > 0)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.StrengthThroughDiversity);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_FollowedCitizensQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray2 = this.m_FollowedCitizensQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Citizen> roComponentLookup3 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Followed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Followed> roComponentLookup4 = this.__TypeHandle.__Game_Citizens_Followed_RO_ComponentLookup;
        try
        {
          for (int index = 0; index < entityArray2.Length; ++index)
          {
            if (roComponentLookup3.HasComponent(entityArray2[index]) && roComponentLookup3[entityArray2[index]].GetAge() == CitizenAge.Elderly && roComponentLookup4.HasComponent(entityArray2[index]) && roComponentLookup4[entityArray2[index]].m_StartedFollowingAsChild)
              PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.YouLittleStalker);
          }
        }
        finally
        {
          entityArray2.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PolicyModificationQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckPolicyAchievements();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_PatientsTreatedCounter.Count > this.m_CachedPatientsTreatedCount)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LittleBitOfTLCBuffer.AddProgress(this.m_PatientsTreatedCounter.Count - this.m_CachedPatientsTreatedCount);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CachedPatientsTreatedCount = this.m_PatientsTreatedCounter.Count;
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedUniqueBuildingQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        int num6 = this.CountSignatureBuildings();
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.MakingAMark, num6, IndicateType.Absolute);
        PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.TheArchitect, num6, IndicateType.Absolute);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ResidentialBuildingsQuery.IsEmptyIgnoreFilter && !this.m_CommercialBuildingsQuery.IsEmptyIgnoreFilter && !this.m_IndustrialBuildingsQuery.IsEmptyIgnoreFilter && !this.m_EnergyProducersQuery.IsEmptyIgnoreFilter && !this.m_WaterPumpingStationQuery.IsEmptyIgnoreFilter)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.MyFirstCity);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedPlantQuery.IsEmptyIgnoreFilter && this.m_PlantQuery.CalculateEntityCount() >= AchievementTriggerSystem.kColossalGardenerLimit)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.ColossalGardener);
      // ISSUE: reference to a compiler-generated method
      if (this.CheckFourSeasons())
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.FourSeasons);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastCheckFrameIndex = this.m_SimulationSystem.frameIndex;
    }

    private bool CheckFourSeasons()
    {
      IAchievement achievement;
      if (!PlatformManager.instance.GetAchievement(Game.Achievements.Achievements.FourSeasons, out achievement) || achievement.achieved)
        return false;
      // ISSUE: reference to a compiler-generated field
      Entity currentClimate = this.m_ClimateSystem.currentClimate;
      if (currentClimate == Entity.Null)
        return false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ClimatePrefab prefab = this.m_PrefabSystem.GetPrefab<ClimatePrefab>(currentClimate);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
        return false;
      ClimateSystem.SeasonInfo[] seasons = prefab.m_Seasons;
      if ((seasons != null ? (seasons.Length < 4 ? 1 : 0) : 0) != 0 || (double) prefab.temperatureRange.min > 0.0)
        return false;
      // ISSUE: reference to a compiler-generated field
      TimeData singleton1 = this.m_TimeDataQuery.GetSingleton<TimeData>();
      // ISSUE: reference to a compiler-generated field
      TimeSettingsData singleton2 = this.m_TimeSettingsQuery.GetSingleton<TimeSettingsData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      float startingDate = this.m_TimeSystem.GetStartingDate(singleton2, singleton1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      float elapsedYears = this.m_TimeSystem.GetElapsedYears(singleton2, singleton1);
      int num = prefab.CountElapsedSeasons(startingDate, elapsedYears);
      int? length = prefab.m_Seasons?.Length;
      int valueOrDefault = length.GetValueOrDefault();
      return num >= valueOrDefault & length.HasValue;
    }

    private int CalculateEnergyProduction(EntityQuery entityQuery)
    {
      int energyProduction = 0;
      NativeArray<ElectricityProducer> componentDataArray = entityQuery.ToComponentDataArray<ElectricityProducer>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray.Length; ++index)
        energyProduction += componentDataArray[index].m_Capacity;
      componentDataArray.Dispose();
      return energyProduction;
    }

    private int CountSignatureBuildings()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabRef> componentDataArray = this.m_UniqueBuildingQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<SignatureBuildingData> roComponentLookup = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;
      int num = 0;
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        if (roComponentLookup.HasComponent(componentDataArray[index].m_Prefab))
          ++num;
      }
      componentDataArray.Dispose();
      return num;
    }

    private int CountParks()
    {
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabRef> componentDataArray = this.m_ParkQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AchievementFilterData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<AchievementFilterData> dataRoBufferLookup = this.__TypeHandle.__Game_Prefabs_AchievementFilterData_RO_BufferLookup;
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        Entity prefab = componentDataArray[index].m_Prefab;
        DynamicBuffer<AchievementFilterData> bufferData;
        // ISSUE: reference to a compiler-generated method
        if (!dataRoBufferLookup.TryGetBuffer(prefab, out bufferData) || this.CheckFilter(bufferData, Game.Achievements.Achievements.Groundskeeper, true))
          ++num;
      }
      componentDataArray.Dispose();
      return num;
    }

    private bool CheckOneOfEverything()
    {
      // ISSUE: reference to a compiler-generated method
      int num = this.CountUniqueServiceBuildingPrefabs();
      // ISSUE: reference to a compiler-generated method
      return this.CountUniqueServiceBuildings() == num;
    }

    private int CountUniqueServiceBuildingPrefabs()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AchievementFilterData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<AchievementFilterData> dataRoBufferLookup = this.__TypeHandle.__Game_Prefabs_AchievementFilterData_RO_BufferLookup;
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_UniqueServiceBuildingPrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        DynamicBuffer<AchievementFilterData> bufferData;
        // ISSUE: reference to a compiler-generated method
        if (!dataRoBufferLookup.TryGetBuffer(entityArray[index], out bufferData) || this.CheckFilter(bufferData, Game.Achievements.Achievements.OneofEverything, true))
          ++num;
      }
      entityArray.Dispose();
      return num;
    }

    private int CountUniqueServiceBuildings()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AchievementFilterData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<AchievementFilterData> dataRoBufferLookup = this.__TypeHandle.__Game_Prefabs_AchievementFilterData_RO_BufferLookup;
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabRef> componentDataArray = this.m_UniqueServiceBuildingQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        Entity prefab = componentDataArray[index].m_Prefab;
        DynamicBuffer<AchievementFilterData> bufferData;
        // ISSUE: reference to a compiler-generated method
        if (!dataRoBufferLookup.TryGetBuffer(prefab, out bufferData) || this.CheckFilter(bufferData, Game.Achievements.Achievements.OneofEverything, true))
          ++num;
      }
      componentDataArray.Dispose();
      return num;
    }

    private bool CheckFilter(
      DynamicBuffer<AchievementFilterData> datas,
      AchievementId achievementID,
      bool defaultResult = false)
    {
      for (int index = 0; index < datas.Length; ++index)
      {
        if (datas[index].m_AchievementID == achievementID)
          return datas[index].m_Allow;
      }
      return defaultResult;
    }

    private void CheckUnlockingAchievements()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ServiceQuery.IsEmptyIgnoreFilter && this.m_LockedServiceQuery.IsEmpty)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.RoyalFlush);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_BuildingQuery.IsEmptyIgnoreFilter || !this.m_LockedBuildingQuery.IsEmpty)
        return;
      PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.KeyToTheCity);
    }

    private void OnInfoviewChanged(InfoviewPrefab infoview)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ViewedInfoviews.Add(infoview);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ViewedInfoviews.Count != this.m_InfoviewQuery.CalculateEntityCount())
        return;
      PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.TheInspector);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LittleBitOfTLCBuffer.m_Progress);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Game.Version.TLCAchievement))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LittleBitOfTLCBuffer.m_Progress);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LittleBitOfTLCBuffer.m_Progress = 0;
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_PatientsTreatedCounter.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SquasherDownerBuffer.Dispose();
    }

    private void CheckPolicyAchievements()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_DistrictQuery.IsEmptyIgnoreFilter)
        return;
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_DistrictQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Policies_Policy_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Policy> policyRoBufferLookup = this.__TypeHandle.__Game_Policies_Policy_RO_BufferLookup;
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        DynamicBuffer<Policy> dynamicBuffer = policyRoBufferLookup[entityArray[index1]];
        for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
        {
          if ((dynamicBuffer[index2].m_Flags & PolicyFlags.Active) != (PolicyFlags) 0)
          {
            ++num;
            break;
          }
        }
      }
      entityArray.Dispose();
      if (num > 0)
        PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.ExecutiveDecision);
      PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.WideVariety, num, IndicateType.Absolute);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public AchievementTriggerSystem()
    {
    }

    public class ProgressBuffer
    {
      private AchievementId m_Achievement;
      private int m_IncrementStep;
      private IndicateType m_Type;
      public int m_Progress;

      public ProgressBuffer(AchievementId achievement, int incrementStep, IndicateType type)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Achievement = achievement;
        // ISSUE: reference to a compiler-generated field
        this.m_IncrementStep = incrementStep;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = type;
        // ISSUE: reference to a compiler-generated field
        this.m_Progress = 0;
      }

      public void AddProgress(int progress)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Progress += progress;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Type == IndicateType.Increment)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Progress < this.m_IncrementStep)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Progress -= this.m_IncrementStep;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PlatformManager.instance.IndicateAchievementProgress(this.m_Achievement, this.m_IncrementStep, this.m_Type);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Type != IndicateType.Absolute)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PlatformManager.instance.IndicateAchievementProgress(this.m_Achievement, this.m_Progress / this.m_IncrementStep * this.m_IncrementStep, this.m_Type);
        }
      }
    }

    public class UserDataProgressBuffer : AchievementTriggerSystem.ProgressBuffer, IDisposable
    {
      private string m_ID;
      private static byte[] sBuffer = new byte[4];

      public UserDataProgressBuffer(
        AchievementId achievement,
        int incrementStep,
        IndicateType type,
        string id)
        : base(achievement, incrementStep, type)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ID = id;
        // ISSUE: reference to a compiler-generated method
        this.Sync();
      }

      private void Sync()
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!PlatformManager.instance.UserDataLoad(this.m_ID) || !PlatformManager.instance.UserDataLoad(this.m_ID, AchievementTriggerSystem.UserDataProgressBuffer.sBuffer))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Progress = BinaryPrimitives.ReadInt32LittleEndian(ReadOnlySpan<byte>.op_Implicit(AchievementTriggerSystem.UserDataProgressBuffer.sBuffer));
        }
        catch (Exception ex)
        {
          // ISSUE: reference to a compiler-generated field
          AchievementTriggerSystem.sLog.Error(ex);
        }
      }

      private void Store()
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BinaryPrimitives.WriteInt32LittleEndian(Span<byte>.op_Implicit(AchievementTriggerSystem.UserDataProgressBuffer.sBuffer), this.m_Progress);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PlatformManager.instance.UserDataStore(this.m_ID, AchievementTriggerSystem.UserDataProgressBuffer.sBuffer);
        }
        catch (Exception ex)
        {
          // ISSUE: reference to a compiler-generated field
          AchievementTriggerSystem.sLog.Error(ex);
        }
      }

      public void Dispose() => this.Store();
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> __Game_Prefabs_OfficeBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Followed> __Game_Citizens_Followed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AchievementFilterData> __Game_Prefabs_AchievementFilterData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Policy> __Game_Policies_Policy_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RO_BufferLookup = state.GetBufferLookup<CityStatistic>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OfficeBuilding_RO_ComponentLookup = state.GetComponentLookup<OfficeBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Followed_RO_ComponentLookup = state.GetComponentLookup<Followed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AchievementFilterData_RO_BufferLookup = state.GetBufferLookup<AchievementFilterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RO_BufferLookup = state.GetBufferLookup<Policy>(true);
      }
    }
  }
}
