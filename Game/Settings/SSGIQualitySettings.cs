// Decompiled with JetBrains decompiler
// Type: Game.Settings.SSGIQualitySettings
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
  [SettingsUISection("SSGIQualitySettings")]
  [SettingsUIDisableByCondition(typeof (SSGIQualitySettings), "IsOptionsDisabled")]
  public class SSGIQualitySettings : QualitySetting<SSGIQualitySettings>
  {
    private static GlobalIllumination m_SSGIComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    public bool fullscreen { get; set; }

    [SettingsUISlider(min = 16f, max = 128f, step = 1f, unit = "integer")]
    public int raySteps { get; set; }

    [SettingsUISlider(min = 0.001f, max = 1f, step = 0.1f, unit = "floatSingleFraction")]
    public float denoiserRadius { get; set; }

    public bool halfResolutionPass { get; set; }

    public bool secondDenoiserPass { get; set; }

    [SettingsUISlider(min = 0.0f, max = 0.5f, step = 0.01f, unit = "floatSingleFraction")]
    public float depthBufferThickness { get; set; }

    static SSGIQualitySettings()
    {
      QualitySetting<SSGIQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, SSGIQualitySettings.disabled);
      QualitySetting<SSGIQualitySettings>.RegisterSetting(QualitySetting.Level.Low, SSGIQualitySettings.lowQuality);
      QualitySetting<SSGIQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, SSGIQualitySettings.mediumQuality);
      QualitySetting<SSGIQualitySettings>.RegisterSetting(QualitySetting.Level.High, SSGIQualitySettings.highQuality);
    }

    public SSGIQualitySettings()
    {
    }

    public SSGIQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<GlobalIllumination>(profile, ref SSGIQualitySettings.m_SSGIComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) SSGIQualitySettings.m_SSGIComponent != (Object) null))
        return;
      this.ApplyState<bool>((VolumeParameter<bool>) SSGIQualitySettings.m_SSGIComponent.enable, this.enabled);
      this.ApplyState<bool>((VolumeParameter<bool>) SSGIQualitySettings.m_SSGIComponent.fullResolutionSS, this.fullscreen);
      this.ApplyState<int>((VolumeParameter<int>) SSGIQualitySettings.m_SSGIComponent.m_MaxRaySteps, this.raySteps);
      this.ApplyState<float>((VolumeParameter<float>) SSGIQualitySettings.m_SSGIComponent.m_DenoiserRadiusSS, this.denoiserRadius);
      this.ApplyState<float>((VolumeParameter<float>) SSGIQualitySettings.m_SSGIComponent.depthBufferThickness, this.depthBufferThickness);
      this.ApplyState<bool>((VolumeParameter<bool>) SSGIQualitySettings.m_SSGIComponent.m_HalfResolutionDenoiserSS, this.halfResolutionPass);
      this.ApplyState<bool>((VolumeParameter<bool>) SSGIQualitySettings.m_SSGIComponent.m_SecondDenoiserPassSS, this.secondDenoiserPass);
    }

    private static SSGIQualitySettings highQuality
    {
      get
      {
        return new SSGIQualitySettings()
        {
          enabled = true,
          fullscreen = false,
          raySteps = 128,
          denoiserRadius = 0.5f,
          depthBufferThickness = 1f / 1000f,
          halfResolutionPass = false,
          secondDenoiserPass = true
        };
      }
    }

    private static SSGIQualitySettings mediumQuality
    {
      get
      {
        return new SSGIQualitySettings()
        {
          enabled = true,
          fullscreen = false,
          raySteps = 64,
          denoiserRadius = 0.5f,
          depthBufferThickness = 1f / 1000f,
          halfResolutionPass = false,
          secondDenoiserPass = true
        };
      }
    }

    private static SSGIQualitySettings lowQuality
    {
      get
      {
        return new SSGIQualitySettings()
        {
          enabled = true,
          fullscreen = false,
          raySteps = 32,
          denoiserRadius = 0.75f,
          depthBufferThickness = 1f / 1000f,
          halfResolutionPass = true,
          secondDenoiserPass = true
        };
      }
    }

    private static SSGIQualitySettings disabled
    {
      get => new SSGIQualitySettings() { enabled = false };
    }

    public override bool IsOptionsDisabled() => this.disableSetting || !this.enabled;
  }
}
