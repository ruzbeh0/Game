// Decompiled with JetBrains decompiler
// Type: Game.Settings.DepthOfFieldQualitySettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Settings
{
  [SettingsUIAdvanced]
  [SettingsUISection("DepthOfFieldQualitySettings")]
  [SettingsUIDisableByCondition(typeof (DepthOfFieldQualitySettings), "IsOptionsDisabled")]
  public class DepthOfFieldQualitySettings : QualitySetting<DepthOfFieldQualitySettings>
  {
    private static DepthOfField m_DOFComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    [SettingsUISlider(min = 3f, max = 8f, step = 1f, unit = "integer")]
    public int nearSampleCount { get; set; }

    [SettingsUISlider(min = 0.0f, max = 8f, step = 0.1f, unit = "floatSingleFraction")]
    public float nearMaxRadius { get; set; }

    [SettingsUISlider(min = 3f, max = 16f, step = 1f, unit = "integer")]
    public int farSampleCount { get; set; }

    [SettingsUISlider(min = 0.0f, max = 16f, step = 0.1f, unit = "floatSingleFraction")]
    public float farMaxRadius { get; set; }

    public DepthOfFieldResolution resolution { get; set; }

    public bool highQualityFiltering { get; set; }

    static DepthOfFieldQualitySettings()
    {
      QualitySetting<DepthOfFieldQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, DepthOfFieldQualitySettings.disabled);
      QualitySetting<DepthOfFieldQualitySettings>.RegisterSetting(QualitySetting.Level.Low, DepthOfFieldQualitySettings.lowQuality);
      QualitySetting<DepthOfFieldQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, DepthOfFieldQualitySettings.mediumQuality);
      QualitySetting<DepthOfFieldQualitySettings>.RegisterSetting(QualitySetting.Level.High, DepthOfFieldQualitySettings.highQuality);
    }

    public DepthOfFieldQualitySettings()
    {
    }

    public DepthOfFieldQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<DepthOfField>(profile, ref DepthOfFieldQualitySettings.m_DOFComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) DepthOfFieldQualitySettings.m_DOFComponent != (Object) null))
        return;
      this.ApplyState<UnityEngine.Rendering.HighDefinition.DepthOfFieldMode>((VolumeParameter<UnityEngine.Rendering.HighDefinition.DepthOfFieldMode>) DepthOfFieldQualitySettings.m_DOFComponent.focusMode, UnityEngine.Rendering.HighDefinition.DepthOfFieldMode.Off, !this.enabled);
      this.ApplyState<int>((VolumeParameter<int>) DepthOfFieldQualitySettings.m_DOFComponent.m_NearSampleCount, this.nearSampleCount);
      this.ApplyState<float>((VolumeParameter<float>) DepthOfFieldQualitySettings.m_DOFComponent.m_NearMaxBlur, this.nearMaxRadius);
      this.ApplyState<int>((VolumeParameter<int>) DepthOfFieldQualitySettings.m_DOFComponent.m_FarSampleCount, this.farSampleCount);
      this.ApplyState<float>((VolumeParameter<float>) DepthOfFieldQualitySettings.m_DOFComponent.m_FarMaxBlur, this.farMaxRadius);
      this.ApplyState<DepthOfFieldResolution>((VolumeParameter<DepthOfFieldResolution>) DepthOfFieldQualitySettings.m_DOFComponent.m_Resolution, this.resolution);
      this.ApplyState<bool>((VolumeParameter<bool>) DepthOfFieldQualitySettings.m_DOFComponent.m_HighQualityFiltering, this.highQualityFiltering);
    }

    private static DepthOfFieldQualitySettings highQuality
    {
      get
      {
        return new DepthOfFieldQualitySettings()
        {
          enabled = true,
          nearSampleCount = 8,
          nearMaxRadius = 7f,
          farSampleCount = 14,
          farMaxRadius = 13f,
          resolution = DepthOfFieldResolution.Half,
          highQualityFiltering = true
        };
      }
    }

    private static DepthOfFieldQualitySettings mediumQuality
    {
      get
      {
        return new DepthOfFieldQualitySettings()
        {
          enabled = true,
          nearSampleCount = 5,
          nearMaxRadius = 4f,
          farSampleCount = 7,
          farMaxRadius = 8f,
          resolution = DepthOfFieldResolution.Half,
          highQualityFiltering = true
        };
      }
    }

    private static DepthOfFieldQualitySettings lowQuality
    {
      get
      {
        return new DepthOfFieldQualitySettings()
        {
          enabled = true,
          nearSampleCount = 3,
          nearMaxRadius = 2f,
          farSampleCount = 4,
          farMaxRadius = 5f,
          resolution = DepthOfFieldResolution.Quarter,
          highQualityFiltering = false
        };
      }
    }

    private static DepthOfFieldQualitySettings disabled
    {
      get
      {
        return new DepthOfFieldQualitySettings()
        {
          enabled = false
        };
      }
    }

    public override bool IsOptionsDisabled() => this.IsOptionFullyDisabled() || !this.enabled;
  }
}
