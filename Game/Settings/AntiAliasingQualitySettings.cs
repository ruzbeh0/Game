// Decompiled with JetBrains decompiler
// Type: Game.Settings.AntiAliasingQualitySettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Settings
{
  [SettingsUIAdvanced]
  [SettingsUISection("AntiAliasingQualitySettings")]
  [SettingsUIDisableByCondition(typeof (AntiAliasingQualitySettings), "IsOptionsDisabled")]
  public class AntiAliasingQualitySettings : QualitySetting<AntiAliasingQualitySettings>
  {
    private static HDAdditionalCameraData m_GameCamera;

    public AntiAliasingQualitySettings.AntialiasingMethod antiAliasingMethod { get; set; }

    public HDAdditionalCameraData.SMAAQualityLevel smaaQuality { get; set; }

    public MSAASamples outlinesMSAA { get; set; }

    static AntiAliasingQualitySettings()
    {
      QualitySetting<AntiAliasingQualitySettings>.RegisterMockName(QualitySetting.Level.Disabled, "None");
      QualitySetting<AntiAliasingQualitySettings>.RegisterMockName(QualitySetting.Level.Low, "FXAA");
      QualitySetting<AntiAliasingQualitySettings>.RegisterMockName(QualitySetting.Level.Medium, "LowSMAA");
      QualitySetting<AntiAliasingQualitySettings>.RegisterMockName(QualitySetting.Level.High, "HighSMAA");
      QualitySetting<AntiAliasingQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, AntiAliasingQualitySettings.disabled);
      QualitySetting<AntiAliasingQualitySettings>.RegisterSetting(QualitySetting.Level.Low, AntiAliasingQualitySettings.lowQuality);
      QualitySetting<AntiAliasingQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, AntiAliasingQualitySettings.mediumQuality);
      QualitySetting<AntiAliasingQualitySettings>.RegisterSetting(QualitySetting.Level.High, AntiAliasingQualitySettings.highQuality);
    }

    public AntiAliasingQualitySettings()
    {
    }

    public AntiAliasingQualitySettings(QualitySetting.Level quality)
    {
      this.SetLevel(quality, false);
    }

    private static HDAdditionalCameraData.AntialiasingMode ToAAMode(
      AntiAliasingQualitySettings.AntialiasingMethod method)
    {
      switch (method)
      {
        case AntiAliasingQualitySettings.AntialiasingMethod.None:
          return HDAdditionalCameraData.AntialiasingMode.None;
        case AntiAliasingQualitySettings.AntialiasingMethod.FXAA:
          return HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;
        case AntiAliasingQualitySettings.AntialiasingMethod.SMAA:
          return HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        case AntiAliasingQualitySettings.AntialiasingMethod.TAA:
          return HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing;
        default:
          return HDAdditionalCameraData.AntialiasingMode.None;
      }
    }

    public override void Apply()
    {
      base.Apply();
      if (!this.TryGetGameplayCamera(ref AntiAliasingQualitySettings.m_GameCamera))
        return;
      if (!SharedSettings.instance.graphics.isDlssActive && !SharedSettings.instance.graphics.isFsr2Active)
      {
        AntiAliasingQualitySettings.m_GameCamera.antialiasing = AntiAliasingQualitySettings.ToAAMode(this.antiAliasingMethod);
        AntiAliasingQualitySettings.m_GameCamera.SMAAQuality = this.smaaQuality;
      }
      else
        AntiAliasingQualitySettings.m_GameCamera.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
    }

    private static AntiAliasingQualitySettings highQuality
    {
      get
      {
        return new AntiAliasingQualitySettings()
        {
          outlinesMSAA = MSAASamples.MSAA8x,
          antiAliasingMethod = AntiAliasingQualitySettings.AntialiasingMethod.SMAA,
          smaaQuality = HDAdditionalCameraData.SMAAQualityLevel.High
        };
      }
    }

    private static AntiAliasingQualitySettings mediumQuality
    {
      get
      {
        return new AntiAliasingQualitySettings()
        {
          outlinesMSAA = MSAASamples.MSAA4x,
          antiAliasingMethod = AntiAliasingQualitySettings.AntialiasingMethod.SMAA,
          smaaQuality = HDAdditionalCameraData.SMAAQualityLevel.Low
        };
      }
    }

    private static AntiAliasingQualitySettings lowQuality
    {
      get
      {
        return new AntiAliasingQualitySettings()
        {
          outlinesMSAA = MSAASamples.MSAA2x,
          antiAliasingMethod = AntiAliasingQualitySettings.AntialiasingMethod.FXAA
        };
      }
    }

    private static AntiAliasingQualitySettings disabled
    {
      get
      {
        return new AntiAliasingQualitySettings()
        {
          outlinesMSAA = MSAASamples.None,
          antiAliasingMethod = AntiAliasingQualitySettings.AntialiasingMethod.None
        };
      }
    }

    public override bool IsOptionFullyDisabled()
    {
      return base.IsOptionFullyDisabled() || SharedSettings.instance.graphics.isDlssActive || SharedSettings.instance.graphics.isFsr2Active;
    }

    public enum AntialiasingMethod
    {
      None,
      FXAA,
      SMAA,
      TAA,
    }
  }
}
