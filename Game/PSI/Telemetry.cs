// Decompiled with JetBrains decompiler
// Type: Game.PSI.Telemetry
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Logging;
using Colossal.PSI.Common;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Input;
using Game.Policies;
using Game.Prefabs;
using Game.PSI.Internal;
using Game.SceneFlow;
using Game.Settings;
using Game.Simulation;
using Game.Tools;
using Game.Tutorials;
using Game.UI;
using Game.UI.InGame;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.PSI
{
  [Telemetry]
  public static class Telemetry
  {
    private static ILog log = LogManager.GetLogger("SceneFlow");
    private const string kHardwareEvent = "hardware";
    private const string kLanguageEvent = "language";
    private const string kGraphicsSettings = "graphics_settings";
    private const string kAchievementUnlocked = "achievement";
    private const string kTutorialEvent = "tutorial_event";
    private const string kMilestoneUnlocked = "milestone_unlocked";
    private const string kDevNodePurchased = "dev_node";
    private const string kPanelClosed = "panel_closed";
    private const string kCityStats = "city_stats";
    private const string kChirper = "chirper";
    private const string kBuildingPlaced = "building_placed";
    private const string kPolicy = "policy";
    private const string kInputIdleEnd = "idle_time_end";
    private const string kSessionOpen = "playsession_start";
    private const string kSessionClose = "playsession_close";
    private static Telemetry.Session s_Session = new Telemetry.Session();

    [TelemetryEvent("hardware", typeof (Telemetry.HardwarePayload))]
    private static void Hardware()
    {
      try
      {
        PlatformManager.instance.SendTelemetry<Telemetry.HardwarePayload>("hardware", new Telemetry.HardwarePayload()
        {
          os_version = SystemInfo.operatingSystem,
          ram = Mathf.RoundToInt((float) SystemInfo.systemMemorySize / 1024f),
          gfx_memory = Mathf.RoundToInt((float) SystemInfo.graphicsMemorySize / 1024f),
          cpucount = SystemInfo.processorCount,
          cpu_model = SystemInfo.processorType,
          gpu_model = SystemInfo.graphicsDeviceName
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("language", typeof (Telemetry.LanguagePayload))]
    private static void Language()
    {
      try
      {
        PlatformManager.instance.SendTelemetry<Telemetry.LanguagePayload>("language", new Telemetry.LanguagePayload()
        {
          os_language = Helpers.GetSystemLanguage(),
          game_language = GameManager.instance.localizationManager.activeLocaleId
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("graphics_settings", typeof (Telemetry.GraphicsSettingsPayload))]
    public static void GraphicsSettings()
    {
      try
      {
        PlatformManager.instance.SendTelemetry<Telemetry.GraphicsSettingsPayload>("graphics_settings", new Telemetry.GraphicsSettingsPayload()
        {
          display_mode = SharedSettings.instance.graphics.displayMode.ToTelemetry(),
          resolution = SharedSettings.instance.graphics.resolution.ToTelemetry(),
          graphics_quality = SharedSettings.instance.graphics.GetLevel().ToString().ToLowerInvariant()
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("achievement", typeof (Telemetry.AchievementPayload))]
    public static void AchievementUnlocked(AchievementId id)
    {
      try
      {
        IAchievement achievement;
        if (!Telemetry.s_Session.active || !PlatformManager.instance.GetAchievement(id, out achievement))
          return;
        PlatformManager.instance.SendTelemetry<Telemetry.AchievementPayload>("achievement", new Telemetry.AchievementPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          achievement_name = achievement.internalName,
          achievement_number = PlatformManager.instance.CountAchievements(true)
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("tutorial_event", typeof (Telemetry.TutorialEventPayload))]
    public static void TutorialEvent(Entity tutorial)
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        PrefabBase prefab = Telemetry.gameplayData.GetPrefab<PrefabBase>(tutorial);
        PlatformManager.instance.SendTelemetry<Telemetry.TutorialEventPayload>("tutorial_event", new Telemetry.TutorialEventPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          advice_followed = (UnityEngine.Object) prefab != (UnityEngine.Object) null ? prefab.name : (string) null
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("milestone_unlocked", typeof (Telemetry.MilestoneUnlockedPayload))]
    public static void MilestoneUnlocked(int milestoneIndex)
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        PlatformManager.instance.SendTelemetry<Telemetry.MilestoneUnlockedPayload>("milestone_unlocked", new Telemetry.MilestoneUnlockedPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          milestone_index = milestoneIndex,
          ingame_days = Telemetry.gameplayData.GetDay()
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("dev_node", typeof (Telemetry.DevNodePurchasedPayload))]
    public static void DevNodePurchased(DevTreeNodePrefab nodePrefab)
    {
      try
      {
        if (!Telemetry.s_Session.active)
          return;
        PlatformManager.instance.SendTelemetry<Telemetry.DevNodePurchasedPayload>("dev_node", new Telemetry.DevNodePurchasedPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          dev_node_name = nodePrefab.name,
          node_type = nodePrefab.m_Service.name,
          tier_id = nodePrefab.m_HorizontalPosition
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    public static void ControlSchemeChanged(InputManager.ControlScheme controlScheme)
    {
    }

    public static void PanelOpened(GamePanel panel)
    {
      if (!Telemetry.s_Session.active)
        return;
      Telemetry.s_Session.PanelOpened(panel.GetType().Name);
    }

    [TelemetryEvent("panel_closed", typeof (Telemetry.PanelClosedPayload))]
    public static void PanelClosed(GamePanel panel)
    {
      try
      {
        TimeSpan timeSpent;
        if (!Telemetry.s_Session.active || !Telemetry.s_Session.PanelClosed(panel.GetType().Name, out timeSpent))
          return;
        string name = panel.GetType().Name;
        PlatformManager.instance.SendTelemetry<Telemetry.PanelClosedPayload>("panel_closed", new Telemetry.PanelClosedPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          panel_name = name,
          time_spent = Math.Round(timeSpent.TotalSeconds, 2)
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("city_stats", typeof (Telemetry.CityStatsPayload))]
    public static void CityStats()
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        Population population = Telemetry.gameplayData.population;
        PlatformManager.instance.SendTelemetry<Telemetry.CityStatsPayload>("city_stats", new Telemetry.CityStatsPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          map_id = Telemetry.gameplayData.mapName,
          n_buildings = Telemetry.gameplayData.buildingCount,
          population = population.m_Population,
          happiness = population.m_AverageHappiness,
          ingame_days = Telemetry.gameplayData.GetDay(),
          map_tiles = Telemetry.gameplayData.ownedMapTiles,
          resource_output = Telemetry.gameplayData.GetResourcesOutputCount()
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("chirper", typeof (Telemetry.ChirperPayload))]
    public static void Chirp(Entity chirpPrefab, uint likes)
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        PrefabBase prefab = Telemetry.gameplayData.GetPrefab<PrefabBase>(chirpPrefab);
        PlatformManager.instance.SendTelemetry<Telemetry.ChirperPayload>("chirper", new Telemetry.ChirperPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          message_type = prefab.name,
          likes = likes
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("building_placed", typeof (Telemetry.BuildingPlacedPayload))]
    public static void PlaceBuilding(Entity entity, PrefabBase building, float3 position)
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active || !building.Has<BuildingPrefab>() && !building.Has<BuildingExtensionPrefab>())
          return;
        string str1 = (string) null;
        int num = 0;
        UIObject component1;
        if (building.TryGet<UIObject>(out component1) && component1.m_Group is UIAssetCategoryPrefab group)
          str1 = group.name;
        string str2 = "base_game";
        ContentPrerequisite component2;
        if (building.TryGet<ContentPrerequisite>(out component2))
          str2 = component2.m_ContentPrerequisite.name;
        PlatformManager.instance.SendTelemetry<Telemetry.BuildingPlacedPayload>("building_placed", new Telemetry.BuildingPlacedPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          map_id = Telemetry.gameplayData.mapName,
          building_id = building.name,
          type = str1,
          building_level = num,
          coordinates = string.Format("{0}|{1}", (object) position.x, (object) position.z),
          origin = str2
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("policy", typeof (Telemetry.PolicyPayload))]
    public static void Policy(ModifiedSystem.PolicyEventInfo eventInfo)
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        // ISSUE: reference to a compiler-generated field
        PolicyPrefab prefab = Telemetry.gameplayData.GetPrefab<PolicyPrefab>(eventInfo.m_Entity);
        if (prefab.m_Visibility == PolicyVisibility.HideFromPolicyList)
          return;
        // ISSUE: reference to a compiler-generated field
        PlatformManager.instance.SendTelemetry<Telemetry.PolicyPayload>("policy", new Telemetry.PolicyPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          policy_id = prefab.name,
          policy_category = prefab.m_Category,
          policy_range = eventInfo.m_PolicyRange
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    public static void InputIdleStart()
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        Telemetry.s_Session.ReportInputIdle();
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("idle_time_end", typeof (Telemetry.InputIdleEndPayload))]
    public static void InputIdleEnd()
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        PlatformManager.instance.SendTelemetry<Telemetry.InputIdleEndPayload>("idle_time_end", new Telemetry.InputIdleEndPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          simulation_speed_start = Telemetry.s_Session.startSimulationSpeed,
          simulation_speed_end = Telemetry.gameplayData.simulationSpeed,
          duration = Math.Round(Telemetry.s_Session.idleTime.TotalSeconds, 2)
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    public static void FireSessionStartEvents()
    {
      Telemetry.Hardware();
      Telemetry.Language();
      Telemetry.GraphicsSettings();
    }

    public static Telemetry.GameplayData gameplayData { get; set; }

    public static Guid GetCurrentSession() => Telemetry.s_Session.guid;

    [TelemetryEvent("playsession_start", typeof (Telemetry.OpenSessionPayload))]
    public static void OpenSession(Guid guid)
    {
      try
      {
        Telemetry.CloseSession();
        if (Telemetry.gameplayData == null || Telemetry.s_Session.active)
          return;
        Telemetry.s_Session.Open(guid);
        PlatformManager.instance.SendTelemetry<Telemetry.OpenSessionPayload>("playsession_start", new Telemetry.OpenSessionPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          map_id = Telemetry.gameplayData.mapName,
          gameplay_mode = GameManager.instance.gameMode.ToTelemetry(),
          tutorial_messages = Telemetry.gameplayData.tutorialEnabled,
          unlimited_money = Telemetry.gameplayData.unlimitedMoney,
          unlock_all = Telemetry.gameplayData.unlockAll,
          disasters = Telemetry.gameplayData.naturalDisasters
        });
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    [TelemetryEvent("playsession_close", typeof (Telemetry.CloseSessionPayload))]
    public static void CloseSession()
    {
      try
      {
        if (Telemetry.gameplayData == null || !Telemetry.s_Session.active)
          return;
        PlatformManager.instance.SendTelemetry<Telemetry.CloseSessionPayload>("playsession_close", new Telemetry.CloseSessionPayload()
        {
          playthrough_id = Telemetry.s_Session.guid,
          map_id = Telemetry.gameplayData.mapName,
          ingame_days = Telemetry.gameplayData.GetDay(),
          time_passed = Math.Round(Telemetry.s_Session.duration.TotalHours, 2)
        });
        Telemetry.s_Session.Close();
      }
      catch (Exception ex)
      {
        Telemetry.log.Warn(ex);
      }
    }

    private struct HardwarePayload
    {
      public string os_version;
      public int ram;
      public int gfx_memory;
      public int cpucount;
      public string cpu_model;
      public string gpu_model;
    }

    private struct LanguagePayload
    {
      public string os_language;
      public string game_language;
    }

    private struct GraphicsSettingsPayload
    {
      public Helpers.json_displaymode display_mode;
      public string resolution;
      public string graphics_quality;
    }

    private struct AchievementPayload
    {
      public Guid playthrough_id;
      public string achievement_name;
      public int achievement_number;
    }

    private struct TutorialEventPayload
    {
      public Guid playthrough_id;
      public string advice_followed;
    }

    private struct MilestoneUnlockedPayload
    {
      public Guid playthrough_id;
      public int milestone_index;
      public int ingame_days;
    }

    private struct DevNodePurchasedPayload
    {
      public Guid playthrough_id;
      public string dev_node_name;
      public string node_type;
      public int tier_id;
    }

    private struct PanelClosedPayload
    {
      public Guid playthrough_id;
      public string panel_name;
      public double time_spent;
    }

    private struct CityStatsPayload
    {
      public Guid playthrough_id;
      public string map_id;
      public int n_buildings;
      public int population;
      public int happiness;
      public int ingame_days;
      public int map_tiles;
      public int resource_output;
    }

    private struct ChirperPayload
    {
      public Guid playthrough_id;
      public string message_type;
      public uint likes;
    }

    private struct BuildingPlacedPayload
    {
      public Guid playthrough_id;
      public string map_id;
      public string building_id;
      public string type;
      public int building_level;
      public string coordinates;
      public string origin;
    }

    private struct PolicyPayload
    {
      public Guid playthrough_id;
      public string policy_id;
      public PolicyCategory policy_category;
      public ModifiedSystem.PolicyRange policy_range;
    }

    private struct InputIdleEndPayload
    {
      public Guid playthrough_id;
      public float simulation_speed_start;
      public float simulation_speed_end;
      public double duration;
    }

    public class GameplayData
    {
      private EntityQuery m_PopulationQuery;
      private EntityQuery m_BuildingsQuery;
      private EntityQuery m_OwnedTileQuery;
      private readonly PrefabSystem m_PrefabSystem;
      private readonly TimeUISystem m_TimeSystem;
      private readonly SimulationSystem m_SimulationSystem;
      private readonly MapMetadataSystem m_MapMetadataSystem;
      private readonly CityStatisticsSystem m_CityStatisticsSystem;
      private readonly CityConfigurationSystem m_CityConfigurationSystem;
      private readonly TutorialSystem m_TutorialSystem;

      private void InitializeStats(World world)
      {
        this.m_PopulationQuery = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Game.City.City>(), ComponentType.ReadOnly<Population>());
        this.m_BuildingsQuery = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
        this.m_OwnedTileQuery = world.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.Exclude<Native>());
      }

      public int buildingCount => this.m_BuildingsQuery.CalculateEntityCount();

      public Population population
      {
        get
        {
          Population population = new Population()
          {
            m_Population = 0,
            m_AverageHappiness = 50
          };
          if (!this.m_PopulationQuery.IsEmpty)
          {
            this.m_PopulationQuery.CompleteDependency();
            population = this.m_PopulationQuery.GetSingleton<Population>();
          }
          return population;
        }
      }

      public int ownedMapTiles => this.m_OwnedTileQuery.CalculateEntityCount();

      public int GetResourcesOutputCount()
      {
        BufferLookup<CityStatistic> bufferLookup = this.m_PrefabSystem.GetBufferLookup<CityStatistic>(true);
        int resourcesOutputCount = 0;
        ResourceIterator iterator = ResourceIterator.GetIterator();
        while (iterator.Next())
        {
          // ISSUE: reference to a compiler-generated method
          resourcesOutputCount += this.m_CityStatisticsSystem.GetStatisticValue(bufferLookup, StatisticType.ProcessingCount, EconomyUtils.GetResourceIndex(iterator.resource));
        }
        return resourcesOutputCount;
      }

      public GameplayData(World world)
      {
        this.m_PrefabSystem = world.GetExistingSystemManaged<PrefabSystem>();
        this.m_TimeSystem = world.GetExistingSystemManaged<TimeUISystem>();
        this.m_SimulationSystem = world.GetExistingSystemManaged<SimulationSystem>();
        this.m_MapMetadataSystem = world.GetExistingSystemManaged<MapMetadataSystem>();
        this.m_CityStatisticsSystem = world.GetExistingSystemManaged<CityStatisticsSystem>();
        this.m_CityConfigurationSystem = world.GetExistingSystemManaged<CityConfigurationSystem>();
        this.m_TutorialSystem = world.GetExistingSystemManaged<TutorialSystem>();
        this.InitializeStats(world);
      }

      public string mapName => this.m_MapMetadataSystem.mapName;

      public float simulationSpeed
      {
        get
        {
          return GameManager.instance.gameMode != GameMode.Game ? 0.0f : this.m_SimulationSystem.selectedSpeed;
        }
      }

      public int GetDay() => this.m_TimeSystem.GetDay();

      public T GetPrefab<T>(Entity entity) where T : PrefabBase
      {
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabSystem.GetPrefab<T>(entity);
      }

      public bool unlimitedMoney => this.m_CityConfigurationSystem.unlimitedMoney;

      public bool unlockAll => this.m_CityConfigurationSystem.unlockAll;

      public bool naturalDisasters => this.m_CityConfigurationSystem.naturalDisasters;

      public bool tutorialEnabled => this.m_TutorialSystem.tutorialEnabled;
    }

    private struct Session
    {
      public Guid guid;
      private DateTime m_InputIdleStart;
      private float m_StartSimulationSpeed;
      private DateTime m_TimeStarted;
      private Dictionary<string, DateTime> timeSpentInPanel;

      public bool active { get; private set; }

      public TimeSpan duration => DateTime.UtcNow - this.m_TimeStarted;

      public TimeSpan idleTime => DateTime.UtcNow - this.m_InputIdleStart;

      public float startSimulationSpeed => this.m_StartSimulationSpeed;

      public void Open(Guid guid)
      {
        if (guid == Guid.Empty)
          guid = Guid.NewGuid();
        this.guid = guid;
        this.timeSpentInPanel = new Dictionary<string, DateTime>();
        this.active = true;
        this.m_TimeStarted = DateTime.UtcNow;
        Telemetry.log.DebugFormat("Telemetry session {0} opened", (object) guid);
      }

      public void PanelOpened(string name) => this.timeSpentInPanel[name] = DateTime.UtcNow;

      public bool PanelClosed(string name, out TimeSpan timeSpent)
      {
        timeSpent = TimeSpan.MinValue;
        DateTime dateTime;
        if (this.timeSpentInPanel.TryGetValue(name, out dateTime))
          timeSpent = DateTime.UtcNow - dateTime;
        return this.timeSpentInPanel.Remove(name);
      }

      public void Close()
      {
        Telemetry.log.DebugFormat("Telemetry session {0} closed", (object) this.guid);
        this.guid = new Guid();
        this.active = false;
      }

      public void ReportInputIdle()
      {
        this.m_InputIdleStart = DateTime.UtcNow;
        this.m_StartSimulationSpeed = Telemetry.gameplayData.simulationSpeed;
      }
    }

    private struct OpenSessionPayload
    {
      public Guid playthrough_id;
      public string map_id;
      public Helpers.json_gameplay_mode gameplay_mode;
      public bool tutorial_messages;
      public bool unlock_all;
      public bool unlimited_money;
      public bool disasters;
    }

    private struct CloseSessionPayload
    {
      public Guid playthrough_id;
      public string map_id;
      public int ingame_days;
      public double time_passed;
    }
  }
}
