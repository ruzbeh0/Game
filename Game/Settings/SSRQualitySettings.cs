// Decompiled with JetBrains decompiler
// Type: Game.Settings.SSRQualitySettings
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
  [SettingsUISection("SSRQualitySettings")]
  [SettingsUIDisableByCondition(typeof (SSRQualitySettings), "IsOptionsDisabled")]
  public class SSRQualitySettings : QualitySetting<SSRQualitySettings>
  {
    private static ScreenSpaceReflection m_SSRComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    public bool enabledTransparent { get; set; }

    [SettingsUISlider(min = 1f, max = 128f, step = 1f, unit = "integer")]
    public int maxRaySteps { get; set; }

    static SSRQualitySettings()
    {
      QualitySetting<SSRQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, SSRQualitySettings.disabled);
      QualitySetting<SSRQualitySettings>.RegisterSetting(QualitySetting.Level.Low, SSRQualitySettings.lowQuality);
      QualitySetting<SSRQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, SSRQualitySettings.mediumQuality);
      QualitySetting<SSRQualitySettings>.RegisterSetting(QualitySetting.Level.High, SSRQualitySettings.highQuality);
    }

    public SSRQualitySettings()
    {
    }

    public SSRQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<ScreenSpaceReflection>(profile, ref SSRQualitySettings.m_SSRComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) SSRQualitySettings.m_SSRComponent != (Object) null))
        return;
      this.ApplyState<bool>((VolumeParameter<bool>) SSRQualitySettings.m_SSRComponent.enabled, this.enabled);
      this.ApplyState<bool>((VolumeParameter<bool>) SSRQualitySettings.m_SSRComponent.enabledTransparent, this.enabled && this.enabledTransparent);
      this.ApplyState<int>((VolumeParameter<int>) SSRQualitySettings.m_SSRComponent.m_RayMaxIterations, this.maxRaySteps);
    }

    private static SSRQualitySettings highQuality
    {
      get
      {
        return new SSRQualitySettings()
        {
          enabled = true,
          enabledTransparent = true,
          maxRaySteps = 64
        };
      }
    }

    private static SSRQualitySettings mediumQuality
    {
      get
      {
        return new SSRQualitySettings()
        {
          enabled = true,
          enabledTransparent = true,
          maxRaySteps = 32
        };
      }
    }

    private static SSRQualitySettings lowQuality
    {
      get
      {
        return new SSRQualitySettings()
        {
          enabled = true,
          enabledTransparent = false,
          maxRaySteps = 16
        };
      }
    }

    private static SSRQualitySettings disabled
    {
      get
      {
        return new SSRQualitySettings()
        {
          enabled = false,
          enabledTransparent = false
        };
      }
    }

    public override bool IsOptionsDisabled() => this.IsOptionFullyDisabled() || !this.enabled;
  }
}
