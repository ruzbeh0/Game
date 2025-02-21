// Decompiled with JetBrains decompiler
// Type: Game.Settings.MotionBlurQualitySettings
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
  [SettingsUISection("MotionBlurQualitySettings")]
  [SettingsUIDisableByCondition(typeof (MotionBlurQualitySettings), "IsOptionsDisabled")]
  public class MotionBlurQualitySettings : QualitySetting<MotionBlurQualitySettings>
  {
    private static MotionBlur m_MotionBlurComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    public int sampleCount { get; set; }

    static MotionBlurQualitySettings()
    {
      QualitySetting<MotionBlurQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, MotionBlurQualitySettings.disabled);
      QualitySetting<MotionBlurQualitySettings>.RegisterSetting(QualitySetting.Level.Low, MotionBlurQualitySettings.lowQuality);
      QualitySetting<MotionBlurQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, MotionBlurQualitySettings.mediumQuality);
      QualitySetting<MotionBlurQualitySettings>.RegisterSetting(QualitySetting.Level.High, MotionBlurQualitySettings.highQuality);
    }

    public MotionBlurQualitySettings()
    {
    }

    public MotionBlurQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<MotionBlur>(profile, ref MotionBlurQualitySettings.m_MotionBlurComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) MotionBlurQualitySettings.m_MotionBlurComponent != (Object) null))
        return;
      this.ApplyState<float>((VolumeParameter<float>) MotionBlurQualitySettings.m_MotionBlurComponent.intensity, 0.0f, !this.enabled);
      this.ApplyState<int>((VolumeParameter<int>) MotionBlurQualitySettings.m_MotionBlurComponent.m_SampleCount, this.sampleCount);
    }

    private static MotionBlurQualitySettings highQuality
    {
      get
      {
        return new MotionBlurQualitySettings()
        {
          enabled = true,
          sampleCount = 12
        };
      }
    }

    private static MotionBlurQualitySettings mediumQuality
    {
      get
      {
        return new MotionBlurQualitySettings()
        {
          enabled = true,
          sampleCount = 8
        };
      }
    }

    private static MotionBlurQualitySettings lowQuality
    {
      get
      {
        return new MotionBlurQualitySettings()
        {
          enabled = true,
          sampleCount = 4
        };
      }
    }

    private static MotionBlurQualitySettings disabled
    {
      get
      {
        return new MotionBlurQualitySettings()
        {
          enabled = false
        };
      }
    }

    public override bool IsOptionsDisabled() => this.IsOptionFullyDisabled() || !this.enabled;
  }
}
