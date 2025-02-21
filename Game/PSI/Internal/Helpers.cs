// Decompiled with JetBrains decompiler
// Type: Game.PSI.Internal.Helpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Game.Settings;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.PSI.Internal
{
  public static class Helpers
  {
    private static readonly IReadOnlyDictionary<SystemLanguage, string> s_SystemLanguageToISO = (IReadOnlyDictionary<SystemLanguage, string>) new Dictionary<SystemLanguage, string>()
    {
      {
        SystemLanguage.Afrikaans,
        "af"
      },
      {
        SystemLanguage.Arabic,
        "ar"
      },
      {
        SystemLanguage.Basque,
        "eu"
      },
      {
        SystemLanguage.Belarusian,
        "be"
      },
      {
        SystemLanguage.Bulgarian,
        "bg"
      },
      {
        SystemLanguage.Catalan,
        "ca"
      },
      {
        SystemLanguage.Chinese,
        "zh"
      },
      {
        SystemLanguage.Czech,
        "cs"
      },
      {
        SystemLanguage.Danish,
        "da"
      },
      {
        SystemLanguage.Dutch,
        "nl"
      },
      {
        SystemLanguage.English,
        "en"
      },
      {
        SystemLanguage.Estonian,
        "et"
      },
      {
        SystemLanguage.Faroese,
        "fo"
      },
      {
        SystemLanguage.Finnish,
        "fi"
      },
      {
        SystemLanguage.French,
        "fr"
      },
      {
        SystemLanguage.German,
        "de"
      },
      {
        SystemLanguage.Greek,
        "el"
      },
      {
        SystemLanguage.Hebrew,
        "he"
      },
      {
        SystemLanguage.Hindi,
        "hi"
      },
      {
        SystemLanguage.Hungarian,
        "hu"
      },
      {
        SystemLanguage.Icelandic,
        "is"
      },
      {
        SystemLanguage.Indonesian,
        "id"
      },
      {
        SystemLanguage.Italian,
        "it"
      },
      {
        SystemLanguage.Japanese,
        "ja"
      },
      {
        SystemLanguage.Korean,
        "ko"
      },
      {
        SystemLanguage.Latvian,
        "lv"
      },
      {
        SystemLanguage.Lithuanian,
        "lt"
      },
      {
        SystemLanguage.Norwegian,
        "no"
      },
      {
        SystemLanguage.Polish,
        "pl"
      },
      {
        SystemLanguage.Portuguese,
        "pt"
      },
      {
        SystemLanguage.Romanian,
        "ro"
      },
      {
        SystemLanguage.Russian,
        "ru"
      },
      {
        SystemLanguage.SerboCroatian,
        "sh"
      },
      {
        SystemLanguage.Slovak,
        "sk"
      },
      {
        SystemLanguage.Slovenian,
        "sl"
      },
      {
        SystemLanguage.Spanish,
        "es"
      },
      {
        SystemLanguage.Swedish,
        "sv"
      },
      {
        SystemLanguage.Thai,
        "th"
      },
      {
        SystemLanguage.Turkish,
        "tr"
      },
      {
        SystemLanguage.Ukrainian,
        "uk"
      },
      {
        SystemLanguage.Vietnamese,
        "vi"
      },
      {
        SystemLanguage.ChineseSimplified,
        "zh-HANS"
      },
      {
        SystemLanguage.ChineseTraditional,
        "zh-HANT"
      }
    };

    public static string GetSystemLanguage()
    {
      string str;
      return Helpers.s_SystemLanguageToISO.TryGetValue(Application.systemLanguage, out str) ? str : string.Empty;
    }

    public static Helpers.json_displaymode ToTelemetry(this DisplayMode mode)
    {
      switch (mode)
      {
        case DisplayMode.Fullscreen:
          return Helpers.json_displaymode.fullscreen;
        case DisplayMode.FullscreenWindow:
          return Helpers.json_displaymode.borderless_window;
        case DisplayMode.Window:
          return Helpers.json_displaymode.windowed;
        default:
          throw new TelemetryException(string.Format("Invalid display mode {0}", (object) mode));
      }
    }

    public static string ToTelemetry(this ScreenResolution resolution)
    {
      return string.Format("{0}x{1}", (object) resolution.width, (object) resolution.height);
    }

    public static int AsInt(this bool value) => !value ? 0 : 1;

    public static Helpers.json_gameplay_mode ToTelemetry(this GameMode gameMode)
    {
      if (gameMode == GameMode.Game)
        return Helpers.json_gameplay_mode.sandbox;
      if (gameMode == GameMode.Editor)
        return Helpers.json_gameplay_mode.editor;
      throw new TelemetryException(string.Format("Invalid game mode {0}", (object) gameMode));
    }

    public enum json_displaymode
    {
      fullscreen,
      windowed,
      borderless_window,
    }

    public enum json_gameplay_mode
    {
      sandbox,
      editor,
    }
  }
}
