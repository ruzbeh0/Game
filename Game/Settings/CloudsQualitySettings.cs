// Decompiled with JetBrains decompiler
// Type: Game.Settings.CloudsQualitySettings
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
  [SettingsUISection("CloudsQualitySettings")]
  [SettingsUIDisableByCondition(typeof (CloudsQualitySettings), "IsOptionsDisabled")]
  public class CloudsQualitySettings : QualitySetting<CloudsQualitySettings>
  {
    private static VolumetricClouds m_VolumetricClouds;
    private static VisualEnvironment m_VisualEnvironment;
    private static CloudLayer m_CloudLayer;

    public bool volumetricCloudsEnabled { get; set; }

    public bool distanceCloudsEnabled { get; set; }

    public bool volumetricCloudsShadows { get; set; }

    public bool distanceCloudsShadows { get; set; }

    static CloudsQualitySettings()
    {
      QualitySetting<CloudsQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, CloudsQualitySettings.disabled);
      QualitySetting<CloudsQualitySettings>.RegisterSetting(QualitySetting.Level.Low, CloudsQualitySettings.lowQuality);
      QualitySetting<CloudsQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, CloudsQualitySettings.mediumQuality);
      QualitySetting<CloudsQualitySettings>.RegisterSetting(QualitySetting.Level.High, CloudsQualitySettings.highQuality);
    }

    public CloudsQualitySettings()
    {
    }

    public CloudsQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<VolumetricClouds>(profile, ref CloudsQualitySettings.m_VolumetricClouds);
      this.CreateVolumeComponent<VisualEnvironment>(profile, ref CloudsQualitySettings.m_VisualEnvironment);
      this.CreateVolumeComponent<CloudLayer>(profile, ref CloudsQualitySettings.m_CloudLayer);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) CloudsQualitySettings.m_VolumetricClouds != (Object) null))
        return;
      this.ApplyState<bool>((VolumeParameter<bool>) CloudsQualitySettings.m_VolumetricClouds.enable, this.volumetricCloudsEnabled);
      this.ApplyState<bool>((VolumeParameter<bool>) CloudsQualitySettings.m_VolumetricClouds.shadows, this.volumetricCloudsShadows);
      this.ApplyState<int>((VolumeParameter<int>) CloudsQualitySettings.m_VisualEnvironment.cloudType, this.distanceCloudsEnabled ? 1 : 0);
      this.ApplyState<bool>((VolumeParameter<bool>) CloudsQualitySettings.m_CloudLayer.layerA.castShadows, this.distanceCloudsShadows);
    }

    private static CloudsQualitySettings highQuality
    {
      get
      {
        return new CloudsQualitySettings()
        {
          volumetricCloudsEnabled = true,
          distanceCloudsEnabled = true,
          volumetricCloudsShadows = true,
          distanceCloudsShadows = true
        };
      }
    }

    private static CloudsQualitySettings mediumQuality
    {
      get
      {
        return new CloudsQualitySettings()
        {
          volumetricCloudsEnabled = true,
          distanceCloudsEnabled = true,
          volumetricCloudsShadows = false,
          distanceCloudsShadows = true
        };
      }
    }

    private static CloudsQualitySettings lowQuality
    {
      get
      {
        return new CloudsQualitySettings()
        {
          volumetricCloudsEnabled = false,
          distanceCloudsEnabled = true,
          volumetricCloudsShadows = false,
          distanceCloudsShadows = false
        };
      }
    }

    private static CloudsQualitySettings disabled
    {
      get
      {
        return new CloudsQualitySettings()
        {
          volumetricCloudsEnabled = false,
          distanceCloudsEnabled = false,
          volumetricCloudsShadows = false,
          distanceCloudsShadows = false
        };
      }
    }
  }
}
