// Decompiled with JetBrains decompiler
// Type: Game.Settings.VolumetricsQualitySettings
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
  [SettingsUISection("VolumetricsQualitySettings")]
  [SettingsUIDisableByCondition(typeof (VolumetricsQualitySettings), "IsOptionsDisabled")]
  public class VolumetricsQualitySettings : QualitySetting<VolumetricsQualitySettings>
  {
    private static Fog m_FogComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    [SettingsUISlider(min = 0.001f, max = 1f, step = 0.1f, unit = "floatSingleFraction")]
    public float budget { get; set; }

    [SettingsUISlider(min = 0.001f, max = 1f, step = 0.1f, unit = "floatSingleFraction")]
    public float resolutionDepthRatio { get; set; }

    static VolumetricsQualitySettings()
    {
      QualitySetting<VolumetricsQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, VolumetricsQualitySettings.disabled);
      QualitySetting<VolumetricsQualitySettings>.RegisterSetting(QualitySetting.Level.Low, VolumetricsQualitySettings.lowQuality);
      QualitySetting<VolumetricsQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, VolumetricsQualitySettings.mediumQuality);
      QualitySetting<VolumetricsQualitySettings>.RegisterSetting(QualitySetting.Level.High, VolumetricsQualitySettings.highQuality);
    }

    public VolumetricsQualitySettings()
    {
    }

    public VolumetricsQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<Fog>(profile, ref VolumetricsQualitySettings.m_FogComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) VolumetricsQualitySettings.m_FogComponent != (Object) null))
        return;
      this.ApplyState<bool>((VolumeParameter<bool>) VolumetricsQualitySettings.m_FogComponent.enableVolumetricFog, this.enabled);
      this.ApplyState<float>((VolumeParameter<float>) VolumetricsQualitySettings.m_FogComponent.m_VolumetricFogBudget, this.budget);
      this.ApplyState<float>((VolumeParameter<float>) VolumetricsQualitySettings.m_FogComponent.m_ResolutionDepthRatio, this.resolutionDepthRatio);
    }

    private static VolumetricsQualitySettings highQuality
    {
      get
      {
        return new VolumetricsQualitySettings()
        {
          budget = 0.666f,
          resolutionDepthRatio = 0.5f,
          enabled = true
        };
      }
    }

    private static VolumetricsQualitySettings mediumQuality
    {
      get
      {
        return new VolumetricsQualitySettings()
        {
          budget = 0.33f,
          resolutionDepthRatio = 0.666f,
          enabled = true
        };
      }
    }

    private static VolumetricsQualitySettings lowQuality
    {
      get
      {
        return new VolumetricsQualitySettings()
        {
          budget = 0.166f,
          resolutionDepthRatio = 0.666f,
          enabled = true
        };
      }
    }

    private static VolumetricsQualitySettings disabled
    {
      get
      {
        return new VolumetricsQualitySettings()
        {
          enabled = false
        };
      }
    }

    public override bool IsOptionsDisabled() => this.IsOptionFullyDisabled() || !this.enabled;
  }
}
