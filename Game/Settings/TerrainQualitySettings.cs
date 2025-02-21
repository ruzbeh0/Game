// Decompiled with JetBrains decompiler
// Type: Game.Settings.TerrainQualitySettings
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
  [SettingsUISection("TerrainQualitySettings")]
  [SettingsUIDisableByCondition(typeof (TerrainQualitySettings), "IsOptionsDisabled")]
  public class TerrainQualitySettings : QualitySetting<TerrainQualitySettings>
  {
    private static TerrainRendering m_TerrainRenderingComponent;

    [SettingsUISlider(min = 2f, max = 5f, step = 1f, unit = "integer")]
    public int finalTessellation { get; set; }

    [SettingsUISlider(min = 4f, max = 64f, step = 1f, unit = "integer")]
    public int targetPatchSize { get; set; }

    public TerrainQualitySettings()
    {
    }

    static TerrainQualitySettings()
    {
      QualitySetting<TerrainQualitySettings>.RegisterSetting(QualitySetting.Level.Low, TerrainQualitySettings.lowQuality);
      QualitySetting<TerrainQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, TerrainQualitySettings.mediumQuality);
      QualitySetting<TerrainQualitySettings>.RegisterSetting(QualitySetting.Level.High, TerrainQualitySettings.highQuality);
    }

    public TerrainQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<TerrainRendering>(profile, ref TerrainQualitySettings.m_TerrainRenderingComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) TerrainQualitySettings.m_TerrainRenderingComponent != (Object) null))
        return;
      this.ApplyState<int>((VolumeParameter<int>) TerrainQualitySettings.m_TerrainRenderingComponent.finalTessellation, this.finalTessellation);
      this.ApplyState<float>((VolumeParameter<float>) TerrainQualitySettings.m_TerrainRenderingComponent.targetPatchSize, (float) this.targetPatchSize);
    }

    private static TerrainQualitySettings highQuality
    {
      get
      {
        return new TerrainQualitySettings()
        {
          finalTessellation = 4,
          targetPatchSize = 12
        };
      }
    }

    private static TerrainQualitySettings mediumQuality
    {
      get
      {
        return new TerrainQualitySettings()
        {
          finalTessellation = 3,
          targetPatchSize = 16
        };
      }
    }

    private static TerrainQualitySettings lowQuality
    {
      get
      {
        return new TerrainQualitySettings()
        {
          finalTessellation = 3,
          targetPatchSize = 24
        };
      }
    }
  }
}
