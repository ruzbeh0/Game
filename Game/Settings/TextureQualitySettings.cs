// Decompiled with JetBrains decompiler
// Type: Game.Settings.TextureQualitySettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Unity.Entities;
using UnityEngine.Rendering.VirtualTexturing;

#nullable disable
namespace Game.Settings
{
  [SettingsUIAdvanced]
  [SettingsUISection("TextureQualitySettings")]
  [SettingsUIDisableByCondition(typeof (TextureQualitySettings), "IsOptionsDisabled")]
  public class TextureQualitySettings : QualitySetting<TextureQualitySettings>
  {
    [SettingsUISlider(min = 0.0f, max = 3f, step = 1f, unit = "integer")]
    public int mipbias { get; set; }

    public FilterMode filterMode { get; set; }

    static TextureQualitySettings()
    {
      QualitySetting<TextureQualitySettings>.RegisterSetting(QualitySetting.Level.VeryLow, TextureQualitySettings.veryLowQuality);
      QualitySetting<TextureQualitySettings>.RegisterSetting(QualitySetting.Level.Low, TextureQualitySettings.lowQuality);
      QualitySetting<TextureQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, TextureQualitySettings.mediumQuality);
      QualitySetting<TextureQualitySettings>.RegisterSetting(QualitySetting.Level.High, TextureQualitySettings.highQuality);
    }

    public TextureQualitySettings()
    {
    }

    public TextureQualitySettings(QualitySetting.Level quality) => this.SetLevel(quality, false);

    public override void Apply()
    {
      base.Apply();
      // ISSUE: reference to a compiler-generated method
      World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<ManagedBatchSystem>()?.ResetVT(this.mipbias, this.filterMode);
    }

    private static TextureQualitySettings highQuality
    {
      get
      {
        return new TextureQualitySettings()
        {
          mipbias = 0,
          filterMode = FilterMode.Trilinear
        };
      }
    }

    private static TextureQualitySettings mediumQuality
    {
      get
      {
        return new TextureQualitySettings()
        {
          mipbias = 1,
          filterMode = FilterMode.Trilinear
        };
      }
    }

    private static TextureQualitySettings lowQuality
    {
      get
      {
        return new TextureQualitySettings()
        {
          mipbias = 2,
          filterMode = FilterMode.Bilinear
        };
      }
    }

    private static TextureQualitySettings veryLowQuality
    {
      get
      {
        return new TextureQualitySettings()
        {
          mipbias = 3,
          filterMode = FilterMode.Bilinear
        };
      }
    }
  }
}
