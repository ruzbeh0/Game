// Decompiled with JetBrains decompiler
// Type: Game.Settings.GraphicsSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Rendering;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.NVIDIA;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  [SettingsUIGroupOrder(new string[] {"Main", "DepthOfField", "UpscalersGroup", "Quality", "DynamicResolutionScaleSettings", "AntiAliasingQualitySettings", "CloudsQualitySettings", "FogQualitySettings", "VolumetricsQualitySettings", "SSAOQualitySettings", "SSGIQualitySettings", "SSRQualitySettings", "DepthOfFieldQualitySettings", "MotionBlurQualitySettings", "ShadowsQualitySettings", "TerrainQualitySettings", "WaterQualitySettings", "LevelOfDetailQualitySettings", "AnimationQualitySettings", "TextureQualitySettings"})]
  [SettingsUIShowGroupName]
  public class GraphicsSettings : GlobalQualitySettings
  {
    public const string kName = "Graphics";
    private int m_resolutionItemsVersion;
    public const string kMainGroup = "Main";
    public const string kDepthOfFieldGroup = "DepthOfField";
    public const string kQualityGroup = "Quality";
    public const string kUpscalersGroup = "UpscalersGroup";
    private bool m_ShowAllResolutions;
    private ScreenResolution m_Resolution;
    private int m_DlssQuality;
    private static Camera m_Camera;
    private static Volume m_VolumeOverride;

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("General", "Main")]
    [SettingsUIHideByCondition(typeof (ScreenHelper), "HideAdditionalResolutionOption")]
    [SettingsUISetter(typeof (GraphicsSettings), "OnShowAllResolutionChanged")]
    public bool showAllResolutions
    {
      get => this.m_ShowAllResolutions;
      set
      {
        if (value == this.m_ShowAllResolutions)
          return;
        this.m_ShowAllResolutions = value;
        this.resolution = this.resolution;
      }
    }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("General", "Main")]
    [SettingsUIDropdown(typeof (GraphicsSettings), "GetScreenResolutionValues")]
    [SettingsUIValueVersion(typeof (GraphicsSettings), "GetResolutionItemsVersion")]
    [SettingsUISetter(typeof (GraphicsSettings), "OnSetResolution")]
    public ScreenResolution resolution
    {
      get => this.m_Resolution;
      set => this.m_Resolution = ScreenHelper.GetClosestAvailable(value, this.showAllResolutions);
    }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("General", "Main")]
    [SettingsUISetter(typeof (GraphicsSettings), "OnSetDisplayMode")]
    public DisplayMode displayMode { get; set; }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("General", "Main")]
    public bool vSync { get; set; }

    [SettingsUISection("General", "Main")]
    [SettingsUISlider(min = 1f, max = 3f, step = 1f, unit = "integer", updateOnDragEnd = true)]
    public int maxFrameLatency { get; set; }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("General", "Main")]
    public GraphicsSettings.CursorMode cursorMode { get; set; }

    [SettingsUISection("General", "Main", "DepthOfField")]
    public GraphicsSettings.DepthOfFieldMode depthOfFieldMode { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("General", "DepthOfField")]
    [SettingsUIDisableByCondition(typeof (GraphicsSettings), "IsTiltShiftDisabled")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 0.1f, unit = "percentageSingleFraction", scalarMultiplier = 100f, updateOnDragEnd = true)]
    public float tiltShiftNearStart { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("General", "DepthOfField")]
    [SettingsUIDisableByCondition(typeof (GraphicsSettings), "IsTiltShiftDisabled")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 0.1f, unit = "percentageSingleFraction", scalarMultiplier = 100f, updateOnDragEnd = true)]
    public float tiltShiftNearEnd { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("General", "DepthOfField")]
    [SettingsUIDisableByCondition(typeof (GraphicsSettings), "IsTiltShiftDisabled")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 0.1f, unit = "percentageSingleFraction", scalarMultiplier = 100f, updateOnDragEnd = true)]
    public float tiltShiftFarStart { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("General", "DepthOfField")]
    [SettingsUIDisableByCondition(typeof (GraphicsSettings), "IsTiltShiftDisabled")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 0.1f, unit = "percentageSingleFraction", scalarMultiplier = 100f, updateOnDragEnd = true)]
    public float tiltShiftFarEnd { get; set; }

    [SettingsUIPlatform(Platform.PC, false)]
    [SettingsUISection("General", "UpscalersGroup")]
    [SettingsUIDisableByCondition(typeof (GraphicsSettings), "isDLSSDisabled")]
    public GraphicsSettings.DlssQuality dlssQuality { get; set; }

    public bool isDlssActive => this.IsDLSSDectected() && this.m_DlssQuality >= 0;

    public bool isFsr2Active => false;

    private bool IsDLSSDectected() => HDDynamicResolutionPlatformCapabilities.DLSSDetected;

    private bool isDLSSDisabled => !this.IsDLSSDectected() || this.isFsr2Active;

    private bool isFSRDisabled => this.isDlssActive;

    static GraphicsSettings()
    {
      QualitySetting<GlobalQualitySettings>.RegisterMockName(QualitySetting.Level.Disabled, "VeryLow");
      QualitySetting<GlobalQualitySettings>.RegisterSetting(QualitySetting.Level.High, new GlobalQualitySettings());
      QualitySetting<GlobalQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, new GlobalQualitySettings());
      QualitySetting<GlobalQualitySettings>.RegisterSetting(QualitySetting.Level.Low, new GlobalQualitySettings());
      QualitySetting<GlobalQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, new GlobalQualitySettings());
    }

    public override AutomaticSettings.SettingPageData GetPageData(string id, bool addPrefix)
    {
      AutomaticSettings.SettingPageData pageData = AutomaticSettings.FillSettingsPage((Setting) this, id, addPrefix);
      AutomaticSettings.SettingItemData settingItemData = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.AdvancedEnumDropdown, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(this.GetType(), typeof (QualitySetting.Level), "Level")
      {
        canRead = true,
        canWrite = true,
        getter = (Func<object, object>) (settings => (object) ((QualitySetting) settings).GetLevel()),
        setter = (Action<object, object>) ((settings, value) => ((QualitySetting) settings).SetLevel((QualitySetting.Level) value)),
        attributes = {
          (System.Attribute) new SettingsUIDropdownAttribute(typeof (QualitySetting), "GetQualityValues"),
          (System.Attribute) new SettingsUIPathAttribute("GraphicsSettings.globalQuality"),
          (System.Attribute) new SettingsUIDisplayNameAttribute("GraphicsSettings.globalQuality")
        }
      }, pageData.prefix)
      {
        isAdvanced = false,
        simpleGroup = "Quality",
        advancedGroup = "Quality"
      };
      pageData["General"].AddItem(settingItemData);
      foreach (QualitySetting qualitySetting in this.qualitySettings)
        qualitySetting.AddToPageData(pageData);
      return pageData;
    }

    internal override void AddToPageData(AutomaticSettings.SettingPageData pageData)
    {
    }

    public override void Apply()
    {
      base.Apply();
      this.ApplyResolution();
      QualitySettings.vSyncCount = this.vSync ? 1 : 0;
      QualitySettings.maxQueuedFrames = this.maxFrameLatency;
      Cursor.lockState = this.cursorMode.ToUnityCursorMode();
      if (this.TryGetGameplayCamera(ref GraphicsSettings.m_Camera))
        this.ApplyDLSSAutoSettings(GraphicsSettings.m_Camera);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (QualitySetting enumerateQualitySetting in this.EnumerateQualitySettings())
      {
        stringBuilder.AppendFormat("{0}: {1}", (object) enumerateQualitySetting.GetType().Name, (object) enumerateQualitySetting.GetLevel());
        if (enumerateQualitySetting != this.lastSetting)
          stringBuilder.Append(" - ");
        enumerateQualitySetting.Apply();
      }
      Setting.log.InfoFormat("Current resolution: {1} {2} - Current quality settings: {0}", (object) stringBuilder.ToString(), (object) this.resolution, (object) this.displayMode);
    }

    public void ApplyResolution()
    {
      ScreenResolution currentResolution = ScreenHelper.currentResolution;
      DisplayMode currentDisplayMode = ScreenHelper.currentDisplayMode;
      ScreenResolution resolution = this.resolution;
      if (!(currentResolution != resolution) && currentDisplayMode == this.displayMode)
        return;
      Setting.log.InfoFormat("Applying resolution: {0} {1}", (object) this.resolution, (object) this.displayMode);
      if (this.resolution.isValid)
        Screen.SetResolution(this.resolution.width, this.resolution.height, ScreenHelper.GetFullscreenMode(this.displayMode), this.resolution.refreshRate);
      else
        Setting.log.ErrorFormat("Resolution {0} {1} is invalid", (object) this.resolution, (object) this.displayMode);
    }

    private DLSSQuality ToDlssQuality(GraphicsSettings.DlssQuality dlssQuality)
    {
      switch (dlssQuality)
      {
        case GraphicsSettings.DlssQuality.MaximumQuality:
          return DLSSQuality.MaximumQuality;
        case GraphicsSettings.DlssQuality.Balanced:
          return DLSSQuality.Balanced;
        case GraphicsSettings.DlssQuality.MaximumPerformance:
          return DLSSQuality.MaximumPerformance;
        case GraphicsSettings.DlssQuality.UltraPerformance:
          return DLSSQuality.UltraPerformance;
        default:
          throw new Exception(string.Format("Unsupported upscaler quality conversion {0}", (object) dlssQuality));
      }
    }

    private void ApplyDLSSAutoSettings(Camera camera)
    {
      this.m_DlssQuality = -1;
      if (this.IsDLSSDectected())
      {
        GraphicsSettings.DlssQuality dlssQuality = this.dlssQuality;
        if (this.dlssQuality == GraphicsSettings.DlssQuality.Auto)
        {
          ScreenResolution currentResolution = ScreenHelper.currentResolution;
          long num = (long) (currentResolution.width * currentResolution.height);
          dlssQuality = num >= 2073600L ? (num > 3686400L ? (num > 8294400L ? GraphicsSettings.DlssQuality.UltraPerformance : GraphicsSettings.DlssQuality.MaximumPerformance) : GraphicsSettings.DlssQuality.MaximumQuality) : GraphicsSettings.DlssQuality.Off;
        }
        if (dlssQuality != GraphicsSettings.DlssQuality.Off)
          this.m_DlssQuality = (int) this.ToDlssQuality(dlssQuality);
      }
      bool flag = this.m_DlssQuality >= 0;
      HDAdditionalCameraData component = camera.GetComponent<HDAdditionalCameraData>();
      component.allowDeepLearningSuperSampling = flag;
      component.deepLearningSuperSamplingUseCustomQualitySettings = true;
      if (!flag)
        return;
      component.deepLearningSuperSamplingQuality = (uint) this.m_DlssQuality;
    }

    private void CreateVolumeOverride()
    {
      if (!((UnityEngine.Object) GraphicsSettings.m_VolumeOverride == (UnityEngine.Object) null))
        return;
      GraphicsSettings.m_VolumeOverride = VolumeHelper.CreateVolume("VolumeQualitySettingsOverride", 100);
    }

    private void CleanupVolumeOverride()
    {
      VolumeHelper.DestroyVolume(GraphicsSettings.m_VolumeOverride);
    }

    public T GetVolumeOverride<T>() where T : VolumeComponent
    {
      T component;
      return (UnityEngine.Object) GraphicsSettings.m_VolumeOverride != (UnityEngine.Object) null && GraphicsSettings.m_VolumeOverride.profileRef.TryGet<T>(out component) ? component : default (T);
    }

    public GraphicsSettings()
    {
      this.CreateVolumeOverride();
      this.AddQualitySetting<DynamicResolutionScaleSettings>(new DynamicResolutionScaleSettings(QualitySetting.Level.High));
      this.AddQualitySetting<AntiAliasingQualitySettings>(new AntiAliasingQualitySettings(QualitySetting.Level.High));
      this.AddQualitySetting<CloudsQualitySettings>(new CloudsQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<FogQualitySettings>(new FogQualitySettings(QualitySetting.Level.Low, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<VolumetricsQualitySettings>(new VolumetricsQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<SSAOQualitySettings>(new SSAOQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<SSGIQualitySettings>(new SSGIQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<SSRQualitySettings>(new SSRQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<DepthOfFieldQualitySettings>(new DepthOfFieldQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<MotionBlurQualitySettings>(new MotionBlurQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<ShadowsQualitySettings>(new ShadowsQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<TerrainQualitySettings>(new TerrainQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<WaterQualitySettings>(new WaterQualitySettings(QualitySetting.Level.High, GraphicsSettings.m_VolumeOverride.profileRef));
      this.AddQualitySetting<LevelOfDetailQualitySettings>(new LevelOfDetailQualitySettings(QualitySetting.Level.High));
      this.AddQualitySetting<AnimationQualitySettings>(new AnimationQualitySettings(QualitySetting.Level.High));
      this.AddQualitySetting<TextureQualitySettings>(new TextureQualitySettings(QualitySetting.Level.High));
      this.SetDefaults();
    }

    public override void SetDefaults()
    {
      this.showAllResolutions = false;
      this.resolution = ScreenHelper.currentResolution;
      this.displayMode = ScreenHelper.currentDisplayMode;
      this.depthOfFieldMode = GraphicsSettings.DepthOfFieldMode.Physical;
      this.vSync = false;
      this.tiltShiftNearStart = 0.5f;
      this.tiltShiftNearEnd = 0.25f;
      this.tiltShiftFarStart = 0.25f;
      this.tiltShiftFarEnd = 0.5f;
      this.maxFrameLatency = QualitySettings.maxQueuedFrames;
      this.cursorMode = GraphicsSettings.CursorMode.ConfinedToWindow;
      this.dlssQuality = this.IsDLSSDectected() ? GraphicsSettings.DlssQuality.Auto : GraphicsSettings.DlssQuality.Off;
      base.SetDefaults();
    }

    public void OnSetResolution(ScreenResolution resolution)
    {
      if (this.displayMode != DisplayMode.Fullscreen)
        return;
      // ISSUE: reference to a compiler-generated method
      World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<OptionsUISystem>().ShowDisplayConfirmation();
    }

    public void OnSetDisplayMode(DisplayMode mode)
    {
      if (this.displayMode == DisplayMode.Fullscreen || mode != DisplayMode.Fullscreen)
        return;
      // ISSUE: reference to a compiler-generated method
      World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<OptionsUISystem>().ShowDisplayConfirmation();
    }

    public bool IsTiltShiftDisabled()
    {
      return this.depthOfFieldMode != GraphicsSettings.DepthOfFieldMode.TiltShift;
    }

    public void OnShowAllResolutionChanged(bool value) => ++this.m_resolutionItemsVersion;

    public int GetResolutionItemsVersion() => this.m_resolutionItemsVersion;

    [Preserve]
    public static DropdownItem<ScreenResolution>[] GetScreenResolutionValues()
    {
      int num1 = SharedSettings.instance.graphics.showAllResolutions ? 1 : 0;
      ScreenResolution[] availableResolutions = ScreenHelper.GetAvailableResolutions(num1 != 0);
      List<DropdownItem<ScreenResolution>> dropdownItemList1 = new List<DropdownItem<ScreenResolution>>(availableResolutions.Length);
      string unit = num1 != 0 ? "screenFrequency" : "integer";
      foreach (ScreenResolution screenResolution in availableResolutions)
      {
        List<DropdownItem<ScreenResolution>> dropdownItemList2 = dropdownItemList1;
        DropdownItem<ScreenResolution> dropdownItem = new DropdownItem<ScreenResolution>();
        dropdownItem.value = screenResolution;
        System.Collections.Generic.Dictionary<string, ILocElement> args = new System.Collections.Generic.Dictionary<string, ILocElement>();
        int num2 = screenResolution.width;
        args.Add("WIDTH", (ILocElement) LocalizedString.Value(num2.ToString("D")));
        num2 = screenResolution.height;
        args.Add("HEIGHT", (ILocElement) LocalizedString.Value(num2.ToString("D")));
        args.Add("REFRESH_RATE", (ILocElement) new LocalizedNumber<double>(screenResolution.refreshRate.value, unit));
        dropdownItem.displayName = new LocalizedString("Options.SCREEN_RESOLUTION_FORMAT", (string) null, (IReadOnlyDictionary<string, ILocElement>) args);
        dropdownItemList2.Add(dropdownItem);
      }
      return dropdownItemList1.ToArray();
    }

    public enum DepthOfFieldMode
    {
      Disabled,
      Physical,
      TiltShift,
    }

    public enum CursorMode
    {
      Free,
      ConfinedToWindow,
    }

    public enum DlssQuality
    {
      Off,
      Auto,
      MaximumQuality,
      Balanced,
      MaximumPerformance,
      UltraPerformance,
    }
  }
}
