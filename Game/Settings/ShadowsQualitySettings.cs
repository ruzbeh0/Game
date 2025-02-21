// Decompiled with JetBrains decompiler
// Type: Game.Settings.ShadowsQualitySettings
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
  [SettingsUISection("ShadowsQualitySettings")]
  [SettingsUIDisableByCondition(typeof (ShadowsQualitySettings), "IsOptionsDisabled")]
  public class ShadowsQualitySettings : QualitySetting<ShadowsQualitySettings>
  {
    private static HDShadowSettings m_CascadeShadows;
    private static HDAdditionalLightData m_SunLightData;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    public int directionalShadowResolution { get; set; }

    public bool terrainCastShadows { get; set; }

    public float shadowCullingThresholdHeight { get; set; }

    public float shadowCullingThresholdVolume { get; set; }

    static ShadowsQualitySettings()
    {
      QualitySetting<ShadowsQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, ShadowsQualitySettings.disabled);
      QualitySetting<ShadowsQualitySettings>.RegisterSetting(QualitySetting.Level.Low, ShadowsQualitySettings.lowQuality);
      QualitySetting<ShadowsQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, ShadowsQualitySettings.mediumQuality);
      QualitySetting<ShadowsQualitySettings>.RegisterSetting(QualitySetting.Level.High, ShadowsQualitySettings.highQuality);
    }

    public ShadowsQualitySettings()
    {
    }

    public ShadowsQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<HDShadowSettings>(profile, ref ShadowsQualitySettings.m_CascadeShadows);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if ((Object) ShadowsQualitySettings.m_SunLightData == (Object) null)
        this.TryGetSunLightData(ref ShadowsQualitySettings.m_SunLightData);
      if ((Object) ShadowsQualitySettings.m_SunLightData != (Object) null)
        ShadowsQualitySettings.m_SunLightData.EnableShadows(this.enabled);
      if ((Object) ShadowsQualitySettings.m_CascadeShadows != (Object) null)
        ShadowsQualitySettings.m_CascadeShadows.active = this.enabled;
      foreach (TerrainSurface instance in TerrainSurface.instances)
        instance.castShadows = this.terrainCastShadows;
    }

    private static ShadowsQualitySettings highQuality
    {
      get
      {
        return new ShadowsQualitySettings()
        {
          enabled = true,
          directionalShadowResolution = 2048,
          terrainCastShadows = true,
          shadowCullingThresholdHeight = 1f,
          shadowCullingThresholdVolume = 1f
        };
      }
    }

    private static ShadowsQualitySettings mediumQuality
    {
      get
      {
        return new ShadowsQualitySettings()
        {
          enabled = true,
          directionalShadowResolution = 1024,
          terrainCastShadows = true,
          shadowCullingThresholdHeight = 1.5f,
          shadowCullingThresholdVolume = 1.5f
        };
      }
    }

    private static ShadowsQualitySettings lowQuality
    {
      get
      {
        return new ShadowsQualitySettings()
        {
          enabled = true,
          directionalShadowResolution = 512,
          terrainCastShadows = false,
          shadowCullingThresholdHeight = 2f,
          shadowCullingThresholdVolume = 2f
        };
      }
    }

    private static ShadowsQualitySettings disabled
    {
      get
      {
        return new ShadowsQualitySettings()
        {
          enabled = false
        };
      }
    }

    public override bool IsOptionsDisabled() => this.IsOptionFullyDisabled() || !this.enabled;
  }
}
