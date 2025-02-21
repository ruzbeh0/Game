// Decompiled with JetBrains decompiler
// Type: Game.PSI.RichPresenceUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.PSI.Common;
using Colossal.PSI.Discord;
using Colossal.PSI.Steamworks;
using Colossal.Serialization.Entities;
using Game.City;
using Game.Rendering;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using Game.UI.InGame;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.PSI
{
  [CompilerGenerated]
  public class RichPresenceUpdateSystem : GameSystemBase
  {
    private const int kUpdateRate = 5;
    private const int kStateCycleInterval = 10;
    private DateTime m_StartTime;
    private DateTime m_LastRichUpdate;
    private DateTime m_LastCycleUpdate;
    private PhotoModeRenderSystem m_PhotoModeRenderSystem;
    private ToolSystem m_ToolSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private CitySystem m_CitySystem;
    private ClimateSystem m_ClimateSystem;
    private TimeUISystem m_TimeUISystem;
    private ClimateUISystem m_ClimateUISystem;
    private EntityQuery m_MilestoneLevelQuery;
    private int m_StateIndex;
    private DiscordRichPresence m_DiscordRichPresence;
    private string[] m_DiscordState;
    private SteamworksPlatform m_SteamworksPlatform;
    private string[] m_SteamState;
    private static readonly string[] kHappinessEmoji = new string[5]
    {
      "\uD83D\uDE1E",
      "\uD83D\uDE41",
      "\uD83D\uDE10",
      "\uD83D\uDE42",
      "\uD83D\uDE04"
    };
    private static readonly string[] kHealthEmoji = new string[3]
    {
      "❤️",
      "❤️❤️",
      "❤️❤️❤️"
    };
    private static readonly string[] kAltHealthEmoji = new string[3]
    {
      "❤️\u200D\uD83E\uDE79",
      "❤️",
      "\uD83D\uDC96"
    };
    private static readonly string[] kAltHealth2Emoji = new string[3]
    {
      "\uD83E\uDD2E",
      "\uD83E\uDD12️",
      "\uD83D\uDC96"
    };
    private static readonly string[] kRomanNumbers = new string[20]
    {
      "I",
      "II",
      "III",
      "IV",
      "V",
      "VI",
      "VII",
      "VIII",
      "IX",
      "X",
      "XI",
      "XII",
      "XIII",
      "XIV",
      "XV",
      "XVI",
      "XVII",
      "XVIII",
      "XIX",
      "XX"
    };

    private DiscordRichPresence discordRichPresence
    {
      get
      {
        return this.m_DiscordRichPresence ?? (this.m_DiscordRichPresence = PlatformManager.instance.GetPSI<DiscordRichPresence>("Discord"));
      }
    }

    private SteamworksPlatform steamworksPlatform
    {
      get
      {
        return this.m_SteamworksPlatform ?? (this.m_SteamworksPlatform = PlatformManager.instance.GetPSI<SteamworksPlatform>("Steamworks"));
      }
    }

    private static int GetHappinessIndex(float happiness)
    {
      if ((double) happiness > 70.0)
        return 4;
      if ((double) happiness > 55.0)
        return 3;
      if ((double) happiness > 40.0)
        return 2;
      return (double) happiness > 25.0 ? 1 : 0;
    }

    private static int GetHealthIndex(float health)
    {
      if ((double) health > 75.0)
        return 2;
      return (double) health > 35.0 ? 1 : 0;
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartTime = DateTime.Now;
      if (GameManager.instance.gameMode.IsGame())
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DiscordState = new string[5]
        {
          "#StatusInGame_Population",
          "#StatusInGame_Money",
          "#StatusInGame_Happiness",
          "#StatusInGame_Health",
          "#StatusInGame_Milestone"
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SteamState = new string[5]
        {
          "Population",
          "Money",
          "Happiness",
          "Health",
          "Milestone"
        };
      }
      else
        GameManager.instance.gameMode.IsEditor();
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateUISystem = this.World.GetOrCreateSystemManaged<ClimateUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeUISystem = this.World.GetOrCreateSystemManaged<TimeUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PhotoModeRenderSystem = this.World.GetOrCreateSystemManaged<PhotoModeRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneLevelQuery = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneLevel>());
      // ISSUE: reference to a compiler-generated method
      this.RegisterKeys();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      try
      {
        DateTime now = DateTime.Now;
        // ISSUE: reference to a compiler-generated field
        if ((now - this.m_LastRichUpdate).TotalSeconds < 5.0)
          return;
        GameMode gameMode = GameManager.instance.gameMode;
        if (gameMode.IsGameOrEditor())
        {
          // ISSUE: reference to a compiler-generated field
          if ((now - this.m_LastCycleUpdate).TotalSeconds > 10.0)
          {
            // ISSUE: reference to a compiler-generated field
            ++this.m_StateIndex;
            // ISSUE: reference to a compiler-generated field
            this.m_LastCycleUpdate = now;
          }
          if (gameMode.IsGame())
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateGameRichPresence();
          }
          else if (gameMode.IsEditor())
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEditorRichPresence();
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastRichUpdate = now;
      }
      catch (Exception ex)
      {
        COSystemBase.baseLog.Warn(ex);
      }
    }

    private int GetAverageHealth()
    {
      World objectInjectionWorld = World.DefaultGameObjectInjectionWorld;
      int averageHealth = 0;
      // ISSUE: reference to a compiler-generated field
      if (objectInjectionWorld.EntityManager.HasComponent<Population>(this.m_CitySystem.City))
      {
        // ISSUE: reference to a compiler-generated field
        averageHealth = objectInjectionWorld.EntityManager.GetComponentData<Population>(this.m_CitySystem.City).m_AverageHealth;
      }
      return averageHealth;
    }

    private string GetMilestoneNumber(int i)
    {
      // ISSUE: reference to a compiler-generated field
      return i == 0 ? (string) null : RichPresenceUpdateSystem.kRomanNumbers[i - 1];
    }

    private string GetMilestoneKey(int i) => string.Format("milestone{0}", (object) i);

    private int GetAchievedMilestone()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_MilestoneLevelQuery.IsEmptyIgnoreFilter ? 0 : this.m_MilestoneLevelQuery.GetSingleton<MilestoneLevel>().m_AchievedMilestone;
    }

    private int GetAverageHappiness()
    {
      World objectInjectionWorld = World.DefaultGameObjectInjectionWorld;
      int averageHappiness = 0;
      // ISSUE: reference to a compiler-generated field
      if (objectInjectionWorld.EntityManager.HasComponent<Population>(this.m_CitySystem.City))
      {
        // ISSUE: reference to a compiler-generated field
        averageHappiness = objectInjectionWorld.EntityManager.GetComponentData<Population>(this.m_CitySystem.City).m_AverageHappiness;
      }
      return averageHappiness;
    }

    private int GetPopulationCount()
    {
      World objectInjectionWorld = World.DefaultGameObjectInjectionWorld;
      int populationCount = 0;
      // ISSUE: reference to a compiler-generated field
      if (objectInjectionWorld.EntityManager.HasComponent<Population>(this.m_CitySystem.City))
      {
        // ISSUE: reference to a compiler-generated field
        populationCount = objectInjectionWorld.EntityManager.GetComponentData<Population>(this.m_CitySystem.City).m_Population;
      }
      return populationCount;
    }

    private string GetWeatherIconKey()
    {
      // ISSUE: reference to a compiler-generated field
      string str = this.m_ClimateSystem.classification.ToString().ToLowerInvariant();
      // ISSUE: reference to a compiler-generated field
      if ((double) (float) this.m_ClimateSystem.precipitation > 0.30000001192092896)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ClimateSystem.isRaining)
        {
          str = "rain";
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ClimateSystem.isSnowing)
            str = "snow";
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ClimateSystem.classification == ClimateSystem.WeatherClassification.Stormy && (double) (float) this.m_ClimateSystem.precipitation > 0.89999997615814209)
      {
        // ISSUE: reference to a compiler-generated field
        str = !this.m_ClimateSystem.isRaining ? "hail" : "stormy";
      }
      // ISSUE: reference to a compiler-generated method
      string lowerInvariant = this.GetLightingState().ToString().ToLowerInvariant();
      return str + lowerInvariant;
    }

    private LightingSystem.State GetLightingState()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      LightingSystem.State lightingState = this.m_TimeUISystem.GetLightingState();
      switch (lightingState)
      {
        case LightingSystem.State.Dawn:
        case LightingSystem.State.Sunrise:
          return LightingSystem.State.Day;
        case LightingSystem.State.Sunset:
        case LightingSystem.State.Dusk:
          return LightingSystem.State.Night;
        default:
          return lightingState;
      }
    }

    private void RegisterKeys()
    {
      PlatformManager instance = PlatformManager.instance;
      // ISSUE: reference to a compiler-generated field
      instance.RegisterRichPresenceKey("#StatusInGame_Building", (Func<string>) (() => "Building " + this.m_CityConfigurationSystem.cityName));
      // ISSUE: reference to a compiler-generated field
      instance.RegisterRichPresenceKey("#StatusInGame_Bulldozing", (Func<string>) (() => "Bulldozing " + this.m_CityConfigurationSystem.cityName));
      // ISSUE: reference to a compiler-generated field
      instance.RegisterRichPresenceKey("#StatusInGame_Inspecting", (Func<string>) (() => "Inspecting " + this.m_CityConfigurationSystem.cityName));
      // ISSUE: reference to a compiler-generated field
      instance.RegisterRichPresenceKey("#StatusInGame_CapturingMemories", (Func<string>) (() => "Capturing memories in " + this.m_CityConfigurationSystem.cityName));
      // ISSUE: reference to a compiler-generated method
      instance.RegisterRichPresenceKey("#StatusInGame_Population", (Func<string>) (() => string.Format("Population: {0}", (object) this.GetPopulationCount())));
      // ISSUE: reference to a compiler-generated field
      instance.RegisterRichPresenceKey("#StatusInGame_Money", (Func<string>) (() => "Money: " + this.m_CitySystem.moneyAmount.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "¢"));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      instance.RegisterRichPresenceKey("#StatusInGame_Happiness", (Func<string>) (() => "Happiness: " + RichPresenceUpdateSystem.kHappinessEmoji[RichPresenceUpdateSystem.GetHappinessIndex((float) this.GetAverageHappiness())]));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      instance.RegisterRichPresenceKey("#StatusInGame_Health", (Func<string>) (() => "Health: " + RichPresenceUpdateSystem.kHealthEmoji[RichPresenceUpdateSystem.GetHealthIndex((float) this.GetAverageHealth())]));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      instance.RegisterRichPresenceKey("#StatusInGame_Milestone", (Func<string>) (() => "Milestone " + this.GetMilestoneNumber(this.GetAchievedMilestone())));
    }

    private string GetActionKey()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_PhotoModeRenderSystem.Enabled)
        return "#StatusInGame_CapturingMemories";
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_ToolSystem.activeInfoview != (UnityEngine.Object) null)
        return "#StatusInGame_Inspecting";
      // ISSUE: reference to a compiler-generated field
      return this.m_ToolSystem.activeTool is BulldozeToolSystem ? "#StatusInGame_Bulldozing" : "#StatusInGame_Building";
    }

    private void UpdateEditorRichPresence()
    {
      // ISSUE: reference to a compiler-generated field
      this.discordRichPresence.SetRichPresence("In Editor", "Authoring UGC...", this.m_StartTime, "editor", "In-Game Editor");
    }

    private void UpdateGameRichPresence()
    {
      // ISSUE: reference to a compiler-generated method
      int achievedMilestone = this.GetAchievedMilestone();
      // ISSUE: reference to a compiler-generated method
      string milestoneNumber = this.GetMilestoneNumber(achievedMilestone);
      // ISSUE: reference to a compiler-generated field
      this.steamworksPlatform.SetRichPresence("cityname", this.m_CityConfigurationSystem.cityName);
      // ISSUE: reference to a compiler-generated field
      if (this.m_SteamState != null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.steamworksPlatform.SetRichPresence("stat", this.m_SteamState[this.m_StateIndex % (achievedMilestone == 0 ? this.m_SteamState.Length - 1 : this.m_SteamState.Length)]);
      }
      SteamworksPlatform steamworksPlatform1 = this.steamworksPlatform;
      // ISSUE: reference to a compiler-generated method
      int num = this.GetPopulationCount();
      string str1 = num.ToString();
      steamworksPlatform1.SetRichPresence("population", str1);
      SteamworksPlatform steamworksPlatform2 = this.steamworksPlatform;
      // ISSUE: reference to a compiler-generated field
      num = this.m_CitySystem.moneyAmount;
      string str2 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      steamworksPlatform2.SetRichPresence("money", str2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.steamworksPlatform.SetRichPresence("health", RichPresenceUpdateSystem.kHealthEmoji[RichPresenceUpdateSystem.GetHealthIndex((float) this.GetAverageHealth())]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.steamworksPlatform.SetRichPresence("happiness", RichPresenceUpdateSystem.kHappinessEmoji[RichPresenceUpdateSystem.GetHappinessIndex((float) this.GetAverageHappiness())]);
      this.steamworksPlatform.SetRichPresence("milestone", milestoneNumber);
      // ISSUE: reference to a compiler-generated method
      this.steamworksPlatform.SetRichPresence(this.GetActionKey());
      // ISSUE: reference to a compiler-generated field
      if (this.m_DiscordState == null)
        return;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      this.discordRichPresence.SetRichPresence(this.GetActionKey(), this.m_DiscordState[this.m_StateIndex % (achievedMilestone == 0 ? this.m_DiscordState.Length - 1 : this.m_DiscordState.Length)], this.m_StartTime, this.GetMilestoneKey(achievedMilestone), milestoneNumber != null ? "Milestone " + milestoneNumber : (string) null, this.GetWeatherIconKey(), string.Format("{0}°C", (object) math.round((float) this.m_ClimateSystem.temperature)));
    }

    [Preserve]
    public RichPresenceUpdateSystem()
    {
    }
  }
}
