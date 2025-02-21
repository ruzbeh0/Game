// Decompiled with JetBrains decompiler
// Type: Game.PSI.PdxSdk.LauncherSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.Localization;
using Colossal.Logging;
using Colossal.PSI.Environment;
using Game.Settings;
using System;
using System.IO;
using UnityEngine;

#nullable disable
namespace Game.PSI.PdxSdk
{
  public static class LauncherSettings
  {
    private static readonly string kLauncherSettingsFileName = "launcher-settings.json";
    private static readonly string kLauncherSettingsPath = EnvPath.kUserDataPath + "/" + LauncherSettings.kLauncherSettingsFileName;
    private static ILog log = LogManager.GetLogger("PdxSdk");

    public static void LoadSettings(
      LocalizationManager localizationManager,
      SharedSettings gameSettings)
    {
      LauncherSettings.Settings launcherSettings;
      if (!LauncherSettings.TryGetLauncherSettings(out launcherSettings))
        return;
      launcherSettings.Apply(localizationManager, gameSettings);
    }

    public static void SaveSettings(SharedSettings gameSettings)
    {
      LauncherSettings.Settings launcherSettings;
      if (!LauncherSettings.TryGetLauncherSettings(out launcherSettings))
        return;
      try
      {
        launcherSettings.Merge(gameSettings);
        string contents = JSON.Dump((object) launcherSettings);
        File.WriteAllText(LauncherSettings.kLauncherSettingsPath, contents);
        LauncherSettings.log.Info((object) "Launcher settings saved successfully");
      }
      catch (Exception ex)
      {
        LauncherSettings.log.InfoFormat("Saving launcher settings failed: {0}", (object) ex);
      }
    }

    private static bool TryGetLauncherSettings(out LauncherSettings.Settings launcherSettings)
    {
      if (File.Exists(LauncherSettings.kLauncherSettingsPath))
      {
        try
        {
          Colossal.Json.Variant variant = JSON.Load(File.ReadAllText(LauncherSettings.kLauncherSettingsPath));
          launcherSettings = variant.Make<LauncherSettings.Settings>();
          LauncherSettings.log.Info((object) "Loaded launcher settings successfully");
          return true;
        }
        catch (Exception ex)
        {
          LauncherSettings.log.InfoFormat("Loading launcher settings failed: {0}", (object) ex);
        }
      }
      else
        LauncherSettings.log.Info((object) "Launcher settings not present");
      launcherSettings = new LauncherSettings.Settings();
      return false;
    }

    private struct Settings
    {
      public LauncherSettings.Settings.System system;

      public void Merge(SharedSettings gameSettings)
      {
        if (gameSettings.userInterface.locale != "os")
          this.system.language = gameSettings.userInterface.locale;
        this.system.display_mode = ((LauncherSettings.Settings.LauncherDisplayMode) gameSettings.graphics.displayMode).ToString();
        this.system.vsync = gameSettings.graphics.vSync;
        string str = LauncherSettings.Settings.FormatResolutionStr(gameSettings.graphics.resolution);
        if (gameSettings.graphics.displayMode == DisplayMode.Window)
          this.system.windowed_resolution = str;
        else
          this.system.fullscreen_resolution = str;
        this.system.refreshRate = gameSettings.graphics.resolution.refreshRate.value;
      }

      public void Apply(LocalizationManager localizationManager, SharedSettings settings)
      {
        bool flag = false;
        if (localizationManager.SupportsLocale(this.system.language))
          settings.userInterface.locale = this.system.language;
        LauncherSettings.Settings.LauncherDisplayMode result;
        if (Enum.TryParse<LauncherSettings.Settings.LauncherDisplayMode>(this.system.display_mode, out result))
        {
          settings.graphics.displayMode = (DisplayMode) result;
          flag = true;
        }
        settings.graphics.vSync = this.system.vsync;
        ScreenResolution resolution;
        if (this.TryFormatResolution((DisplayMode) result, out resolution))
        {
          settings.graphics.resolution = resolution;
          flag = true;
        }
        if (!flag)
          return;
        settings.graphics.ApplyResolution();
      }

      private bool TryFormatResolution(DisplayMode displayMode, out ScreenResolution resolution)
      {
        string str = displayMode == DisplayMode.Window ? this.system.windowed_resolution : this.system.fullscreen_resolution;
        if (str != null)
        {
          int length = str.IndexOf("x", StringComparison.Ordinal);
          if (length >= 0)
          {
            string s1 = str.Substring(0, length);
            string s2 = str.Substring(length + 1);
            int num;
            ref int local = ref num;
            int result;
            if (int.TryParse(s1, out local) && int.TryParse(s2, out result))
            {
              resolution = new ScreenResolution()
              {
                width = num,
                height = result,
                refreshRate = new RefreshRate()
                {
                  numerator = (uint) (this.system.refreshRate * 1000.0),
                  denominator = 1000U
                }
              };
              return resolution.isValid;
            }
          }
        }
        resolution = new ScreenResolution();
        return false;
      }

      private static string FormatResolutionStr(ScreenResolution resolution)
      {
        return resolution.width.ToString() + "x" + resolution.height.ToString();
      }

      public struct System
      {
        public string language;
        public string display_mode;
        public bool vsync;
        public string fullscreen_resolution;
        public string windowed_resolution;
        public double refreshRate;
      }

      private enum LauncherDisplayMode
      {
        fullscreen,
        borderless_fullscreen,
        windowed,
      }
    }
  }
}
