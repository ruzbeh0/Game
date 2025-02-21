// Decompiled with JetBrains decompiler
// Type: Game.Settings.DynamicResolutionScaleSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering.Utilities;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  [SettingsUIAdvanced]
  [SettingsUISection("DynamicResolutionScaleSettings")]
  [SettingsUIDisableByCondition(typeof (DynamicResolutionScaleSettings), "IsOptionsDisabled")]
  public class DynamicResolutionScaleSettings : QualitySetting<DynamicResolutionScaleSettings>
  {
    private static Camera m_Camera;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    public bool isAdaptive { get; set; }

    public AdaptiveDynamicResolutionScale.DynResUpscaleFilter upscaleFilter { get; set; }

    [SettingsUISlider(min = 50f, max = 100f, step = 1f, unit = "percentageSingleFraction", scalarMultiplier = 100f)]
    public float minScale { get; set; }

    public DynamicResolutionScaleSettings()
    {
    }

    static DynamicResolutionScaleSettings()
    {
      QualitySetting<DynamicResolutionScaleSettings>.RegisterMockName(QualitySetting.Level.Low, "Constant");
      QualitySetting<DynamicResolutionScaleSettings>.RegisterMockName(QualitySetting.Level.Medium, "Automatic");
      QualitySetting<DynamicResolutionScaleSettings>.RegisterMockName(QualitySetting.Level.High, "Disabled");
      QualitySetting<DynamicResolutionScaleSettings>.RegisterSetting(QualitySetting.Level.Low, DynamicResolutionScaleSettings.constantQuality);
      QualitySetting<DynamicResolutionScaleSettings>.RegisterSetting(QualitySetting.Level.Medium, DynamicResolutionScaleSettings.automaticQuality);
      QualitySetting<DynamicResolutionScaleSettings>.RegisterSetting(QualitySetting.Level.High, DynamicResolutionScaleSettings.disabledQuality);
    }

    public DynamicResolutionScaleSettings(QualitySetting.Level quality)
    {
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!this.TryGetGameplayCamera(ref DynamicResolutionScaleSettings.m_Camera))
        return;
      AdaptiveDynamicResolutionScale.instance.SetParams(this.enabled, this.isAdaptive, this.minScale, this.upscaleFilter, DynamicResolutionScaleSettings.m_Camera);
    }

    private static DynamicResolutionScaleSettings constantQuality
    {
      get
      {
        return new DynamicResolutionScaleSettings()
        {
          enabled = true,
          isAdaptive = false,
          upscaleFilter = AdaptiveDynamicResolutionScale.DynResUpscaleFilter.EdgeAdaptiveScaling,
          minScale = 0.5f
        };
      }
    }

    private static DynamicResolutionScaleSettings automaticQuality
    {
      get
      {
        return new DynamicResolutionScaleSettings()
        {
          enabled = true,
          isAdaptive = true,
          upscaleFilter = AdaptiveDynamicResolutionScale.DynResUpscaleFilter.EdgeAdaptiveScaling,
          minScale = 0.5f
        };
      }
    }

    private static DynamicResolutionScaleSettings disabledQuality
    {
      get
      {
        return new DynamicResolutionScaleSettings()
        {
          enabled = false
        };
      }
    }

    public override bool IsOptionsDisabled() => !this.enabled || this.IsOptionFullyDisabled();

    public override bool IsOptionFullyDisabled()
    {
      return base.IsOptionFullyDisabled() || SharedSettings.instance.graphics.isDlssActive || SharedSettings.instance.graphics.isFsr2Active;
    }
  }
}
