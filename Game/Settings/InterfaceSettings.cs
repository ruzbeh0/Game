// Decompiled with JetBrains decompiler
// Type: Game.Settings.InterfaceSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;
using Game.Input;
using Game.SceneFlow;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  [SettingsUIGroupOrder(new string[] {"Language", "Style", "Popup", "Hint", "Unit", "Block"})]
  public class InterfaceSettings : Setting
  {
    public const string kName = "Interface";
    public const string kLanguageGroup = "Language";
    public const string kStyleGroup = "Style";
    public const string kPopupGroup = "Popup";
    public const string kHintGroup = "Hint";
    public const string kUnitGroup = "Unit";
    public const string kBlockGroup = "Block";

    [Exclude]
    [SettingsUISection("Language")]
    [SettingsUIDropdown(typeof (InterfaceSettings), "GetLanguageValues")]
    public string currentLocale
    {
      get
      {
        return this.locale == "os" ? GameManager.instance.localizationManager.activeLocaleId : this.locale;
      }
      set => this.locale = value;
    }

    [SettingsUIHidden]
    public string locale { get; set; }

    [SettingsUISection("Style")]
    [SettingsUIDropdown(typeof (InterfaceSettings), "GetInterfaceStyleValues")]
    public string interfaceStyle { get; set; }

    [SettingsUISection("Style")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scalarMultiplier = 100f)]
    public float interfaceTransparency { get; set; }

    [SettingsUIDeveloper]
    [SettingsUISection("Style")]
    public bool interfaceScaling { get; set; }

    [SettingsUISection("Style")]
    [SettingsUISlider(min = 100f, max = 140f, step = 10f, unit = "percentage", scalarMultiplier = 100f)]
    public float textScale { get; set; }

    [SettingsUISection("Popup")]
    public bool unlockHighlightsEnabled { get; set; }

    [SettingsUISection("Popup")]
    public bool chirperPopupsEnabled { get; set; }

    [SettingsUISection("Popup")]
    [SettingsUIHidden]
    public bool showWhatsNewPanel { get; set; }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("Hint")]
    public InterfaceSettings.InputHintsType inputHintsType { get; set; }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("Hint")]
    public InterfaceSettings.KeyboardLayout keyboardLayout { get; set; }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("Hint")]
    public bool shortcutHints { get; set; }

    [SettingsUISection("Unit")]
    public InterfaceSettings.TimeFormat timeFormat { get; set; }

    [SettingsUISection("Unit")]
    public InterfaceSettings.TemperatureUnit temperatureUnit { get; set; }

    [SettingsUISection("Unit")]
    public InterfaceSettings.UnitSystem unitSystem { get; set; }

    [SettingsUISection("Block")]
    public bool blockingPopupsEnabled { get; set; }

    [SettingsUIHidden]
    public List<string> dismissedConfirmations { get; set; } = new List<string>();

    public InterfaceSettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.locale = "os";
      this.interfaceStyle = "default";
      this.interfaceTransparency = 0.5f;
      this.interfaceScaling = true;
      this.textScale = 1f;
      this.unlockHighlightsEnabled = true;
      this.chirperPopupsEnabled = true;
      this.showWhatsNewPanel = true;
      this.blockingPopupsEnabled = true;
      this.inputHintsType = InterfaceSettings.InputHintsType.AutoDetect;
      this.keyboardLayout = InterfaceSettings.KeyboardLayout.AutoDetect;
      this.shortcutHints = true;
      this.timeFormat = InterfaceSettings.TimeFormat.TwentyFourHours;
      this.temperatureUnit = InterfaceSettings.TemperatureUnit.Celsius;
      this.unitSystem = InterfaceSettings.UnitSystem.Metric;
      this.dismissedConfirmations = new List<string>();
    }

    public override void Apply()
    {
      base.Apply();
      GameManager.instance.localizationManager.SetActiveLocale(this.locale);
    }

    [Preserve]
    public static DropdownItem<string>[] GetLanguageValues()
    {
      LocalizationManager localizationManager = GameManager.instance.localizationManager;
      string[] supportedLocales = localizationManager.GetSupportedLocales();
      List<DropdownItem<string>> dropdownItemList = new List<DropdownItem<string>>(supportedLocales.Length);
      foreach (string localeId in supportedLocales)
        dropdownItemList.Add(new DropdownItem<string>()
        {
          value = localeId,
          displayName = LocalizedString.Value(localizationManager.GetLocalizedName(localeId))
        });
      return dropdownItemList.ToArray();
    }

    [Preserve]
    public static DropdownItem<string>[] GetInterfaceStyleValues()
    {
      return new List<DropdownItem<string>>()
      {
        new DropdownItem<string>()
        {
          value = "default",
          displayName = (LocalizedString) "Options.INTERFACE_STYLE[default]"
        },
        new DropdownItem<string>()
        {
          value = "bright-blue",
          displayName = (LocalizedString) "Options.INTERFACE_STYLE[bright-blue]"
        },
        new DropdownItem<string>()
        {
          value = "dark-grey-orange",
          displayName = (LocalizedString) "Options.INTERFACE_STYLE[dark-grey-orange]"
        }
      }.ToArray();
    }

    public InputManager.GamepadType GetFinalInputHintsType()
    {
      InputManager.GamepadType finalInputHintsType;
      switch (this.inputHintsType)
      {
        case InterfaceSettings.InputHintsType.AutoDetect:
          finalInputHintsType = InputManager.instance.GetActiveGamepadType();
          break;
        case InterfaceSettings.InputHintsType.Xbox:
          finalInputHintsType = InputManager.GamepadType.Xbox;
          break;
        case InterfaceSettings.InputHintsType.PS:
          finalInputHintsType = InputManager.GamepadType.PS;
          break;
        default:
          finalInputHintsType = InputManager.GamepadType.Xbox;
          break;
      }
      return finalInputHintsType;
    }

    public enum InputHintsType
    {
      AutoDetect,
      Xbox,
      PS,
    }

    public enum KeyboardLayout
    {
      AutoDetect,
      International,
    }

    public enum TimeFormat
    {
      TwentyFourHours,
      TwelveHours,
    }

    public enum TemperatureUnit
    {
      Celsius,
      Fahrenheit,
      Kelvin,
    }

    public enum UnitSystem
    {
      Metric,
      Freedom,
    }
  }
}
