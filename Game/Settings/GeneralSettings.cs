// Decompiled with JetBrains decompiler
// Type: Game.Settings.GeneralSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Game.SceneFlow;
using Game.Simulation;
using Unity.Entities;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  public class GeneralSettings : Setting
  {
    public const string kName = "General";
    private Colossal.IO.AssetDatabase.AssetDatabase.AutoReloadMode m_AssetDatabaseAutoReloadMode;
    private SimulationSystem.PerformancePreference m_PerformancePreference;

    [SettingsUIPlatform(Platform.PC, false)]
    public Colossal.IO.AssetDatabase.AssetDatabase.AutoReloadMode assetDatabaseAutoReloadMode
    {
      get => this.m_AssetDatabaseAutoReloadMode;
      set
      {
        this.m_AssetDatabaseAutoReloadMode = value;
        Colossal.IO.AssetDatabase.AssetDatabase.global.autoReloadMode = this.m_AssetDatabaseAutoReloadMode;
      }
    }

    public SimulationSystem.PerformancePreference performancePreference
    {
      get => this.m_PerformancePreference;
      set
      {
        if (this.m_PerformancePreference == value)
          return;
        this.m_PerformancePreference = value;
        SimulationSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<SimulationSystem>();
        if (existingSystemManaged == null)
          return;
        existingSystemManaged.performancePreference = value;
      }
    }

    [SettingsUIDeveloper]
    public GeneralSettings.FPSMode fpsMode { get; set; }

    public bool autoSave { get; set; }

    [SettingsUIDisableByCondition(typeof (GeneralSettings), "AutoSaveEnabled")]
    public GeneralSettings.AutoSaveInterval autoSaveInterval { get; set; }

    [SettingsUIDisableByCondition(typeof (GeneralSettings), "AutoSaveEnabled")]
    public GeneralSettings.AutoSaveCount autoSaveCount { get; set; }

    [SettingsUIDeveloper]
    [SettingsUIButton]
    [SettingsUIDisableByCondition(typeof (GeneralSettings), "CanSave")]
    public bool autoSaveNow
    {
      get => true;
      set
      {
        if (!GameManager.instance.gameMode.IsGameOrEditor())
          return;
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<AutoSaveSystem>()?.PerformAutoSave(this);
      }
    }

    [SettingsUIButton]
    [SettingsUIConfirmation(null, null)]
    public bool resetSettings
    {
      set => GameManager.instance.settings.Reset();
    }

    public GeneralSettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.autoSave = false;
      this.autoSaveInterval = GeneralSettings.AutoSaveInterval.FiveMinutes;
      this.autoSaveCount = GeneralSettings.AutoSaveCount.Three;
      this.fpsMode = GeneralSettings.FPSMode.Off;
      this.assetDatabaseAutoReloadMode = Colossal.IO.AssetDatabase.AssetDatabase.AutoReloadMode.None;
      this.performancePreference = SimulationSystem.PerformancePreference.Balanced;
    }

    public static bool CanSave() => !GameManager.instance.gameMode.IsGameOrEditor();

    public static bool AutoSaveEnabled() => !SharedSettings.instance.general.autoSave;

    public enum FPSMode
    {
      Off,
      Simple,
      Advanced,
      Precise,
    }

    public enum AutoSaveCount
    {
      Unlimited = 0,
      One = 1,
      Three = 3,
      Ten = 10, // 0x0000000A
      Fifty = 50, // 0x00000032
      Hundred = 100, // 0x00000064
    }

    public enum AutoSaveInterval
    {
      OneMinute = 60, // 0x0000003C
      TwoMinutes = 120, // 0x00000078
      FiveMinutes = 300, // 0x0000012C
      TenMinutes = 600, // 0x00000258
      ThirtyMinutes = 1800, // 0x00000708
      OneHour = 3600, // 0x00000E10
    }
  }
}
