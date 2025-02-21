// Decompiled with JetBrains decompiler
// Type: Game.Settings.SSAOQualitySettings
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
  [SettingsUISection("SSAOQualitySettings")]
  [SettingsUIDisableByCondition(typeof (SSAOQualitySettings), "IsOptionsDisabled")]
  public class SSAOQualitySettings : QualitySetting<SSAOQualitySettings>
  {
    private static ScreenSpaceAmbientOcclusion m_AOComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    [SettingsUISlider(min = 16f, max = 256f, step = 1f, unit = "integer")]
    public int maxPixelRadius { get; set; }

    public bool fullscreen { get; set; }

    [SettingsUISlider(min = 2f, max = 32f, step = 1f, unit = "integer")]
    public int stepCount { get; set; }

    static SSAOQualitySettings()
    {
      QualitySetting<SSAOQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, SSAOQualitySettings.disabled);
      QualitySetting<SSAOQualitySettings>.RegisterSetting(QualitySetting.Level.Low, SSAOQualitySettings.lowQuality);
      QualitySetting<SSAOQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, SSAOQualitySettings.mediumQuality);
      QualitySetting<SSAOQualitySettings>.RegisterSetting(QualitySetting.Level.High, SSAOQualitySettings.highQuality);
    }

    public SSAOQualitySettings()
    {
    }

    public SSAOQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<ScreenSpaceAmbientOcclusion>(profile, ref SSAOQualitySettings.m_AOComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) SSAOQualitySettings.m_AOComponent != (Object) null))
        return;
      this.ApplyState<float>((VolumeParameter<float>) SSAOQualitySettings.m_AOComponent.intensity, 0.0f, !this.enabled);
      this.ApplyState<bool>((VolumeParameter<bool>) SSAOQualitySettings.m_AOComponent.m_FullResolution, this.fullscreen);
      this.ApplyState<int>((VolumeParameter<int>) SSAOQualitySettings.m_AOComponent.m_MaximumRadiusInPixels, this.maxPixelRadius);
      this.ApplyState<int>((VolumeParameter<int>) SSAOQualitySettings.m_AOComponent.m_StepCount, this.stepCount);
    }

    private static SSAOQualitySettings highQuality
    {
      get
      {
        return new SSAOQualitySettings()
        {
          enabled = true,
          stepCount = 16,
          maxPixelRadius = 80,
          fullscreen = true
        };
      }
    }

    private static SSAOQualitySettings mediumQuality
    {
      get
      {
        return new SSAOQualitySettings()
        {
          enabled = true,
          stepCount = 6,
          maxPixelRadius = 40,
          fullscreen = true
        };
      }
    }

    private static SSAOQualitySettings lowQuality
    {
      get
      {
        return new SSAOQualitySettings()
        {
          enabled = true,
          stepCount = 4,
          maxPixelRadius = 32,
          fullscreen = false
        };
      }
    }

    private static SSAOQualitySettings disabled
    {
      get => new SSAOQualitySettings() { enabled = false };
    }

    public override bool IsOptionsDisabled() => this.IsOptionFullyDisabled() || !this.enabled;
  }
}
