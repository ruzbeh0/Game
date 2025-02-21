// Decompiled with JetBrains decompiler
// Type: Game.Settings.WaterQualitySettings
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
  [SettingsUISection("WaterQualitySettings")]
  [SettingsUIDisableByCondition(typeof (WaterQualitySettings), "IsOptionsDisabled")]
  public class WaterQualitySettings : QualitySetting<WaterQualitySettings>
  {
    private static WaterRendering m_WaterRenderingComponent;

    public bool waterflow { get; set; }

    [SettingsUISlider(min = 0.0f, max = 15f, step = 0.1f, unit = "floatSingleFraction")]
    public float maxTessellationFactor { get; set; }

    [SettingsUISlider(min = 0.0f, max = 4000f, step = 1f, unit = "floatSingleFraction")]
    public float tessellationFactorFadeStart { get; set; }

    [SettingsUISlider(min = 10f, max = 4000f, step = 1f, unit = "floatSingleFraction")]
    public float tessellationFactorFadeRange { get; set; }

    public WaterQualitySettings()
    {
    }

    static WaterQualitySettings()
    {
      QualitySetting<WaterQualitySettings>.RegisterSetting(QualitySetting.Level.Low, WaterQualitySettings.lowQuality);
      QualitySetting<WaterQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, WaterQualitySettings.mediumQuality);
      QualitySetting<WaterQualitySettings>.RegisterSetting(QualitySetting.Level.High, WaterQualitySettings.highQuality);
    }

    public WaterQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<WaterRendering>(profile, ref WaterQualitySettings.m_WaterRenderingComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if ((Object) WaterQualitySettings.m_WaterRenderingComponent != (Object) null)
      {
        this.ApplyState<float>((VolumeParameter<float>) WaterQualitySettings.m_WaterRenderingComponent.maxTessellationFactor, this.maxTessellationFactor);
        this.ApplyState<float>((VolumeParameter<float>) WaterQualitySettings.m_WaterRenderingComponent.tessellationFactorFadeStart, this.tessellationFactorFadeStart);
        this.ApplyState<float>((VolumeParameter<float>) WaterQualitySettings.m_WaterRenderingComponent.tessellationFactorFadeRange, this.tessellationFactorFadeRange);
      }
      foreach (WaterSurface instance in WaterSurface.instances)
        instance.waterFlow = this.waterflow;
    }

    private static WaterQualitySettings highQuality
    {
      get
      {
        return new WaterQualitySettings()
        {
          maxTessellationFactor = 10f,
          tessellationFactorFadeStart = 150f,
          tessellationFactorFadeRange = 1850f,
          waterflow = true
        };
      }
    }

    private static WaterQualitySettings mediumQuality
    {
      get
      {
        return new WaterQualitySettings()
        {
          maxTessellationFactor = 6f,
          tessellationFactorFadeStart = 150f,
          tessellationFactorFadeRange = 1850f,
          waterflow = true
        };
      }
    }

    private static WaterQualitySettings lowQuality
    {
      get
      {
        return new WaterQualitySettings()
        {
          maxTessellationFactor = 2f,
          tessellationFactorFadeStart = 150f,
          tessellationFactorFadeRange = 1850f,
          waterflow = false
        };
      }
    }
  }
}
