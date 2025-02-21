// Decompiled with JetBrains decompiler
// Type: Game.Settings.FogQualitySettings
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
  [SettingsUISection("FogQualitySettings")]
  [SettingsUIDisableByCondition(typeof (FogQualitySettings), "IsOptionsDisabled")]
  public class FogQualitySettings : QualitySetting<FogQualitySettings>
  {
    private static Fog m_FogComponent;

    [SettingsUIHidden]
    public bool enabled { get; set; }

    static FogQualitySettings()
    {
      QualitySetting<FogQualitySettings>.RegisterMockName(QualitySetting.Level.Low, "Enabled");
      QualitySetting<FogQualitySettings>.RegisterSetting(QualitySetting.Level.Disabled, FogQualitySettings.disabled);
      QualitySetting<FogQualitySettings>.RegisterSetting(QualitySetting.Level.Low, FogQualitySettings.lowQuality);
    }

    public FogQualitySettings()
    {
    }

    public FogQualitySettings(QualitySetting.Level quality, VolumeProfile profile)
    {
      this.CreateVolumeComponent<Fog>(profile, ref FogQualitySettings.m_FogComponent);
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      if (!((Object) FogQualitySettings.m_FogComponent != (Object) null))
        return;
      this.ApplyState<bool>((VolumeParameter<bool>) FogQualitySettings.m_FogComponent.enabled, this.enabled);
      VolumetricsQualitySettings qualitySetting = SharedSettings.instance.graphics.GetQualitySetting<VolumetricsQualitySettings>();
      qualitySetting.disableSetting = !this.enabled;
      if (this.enabled)
        return;
      qualitySetting.enabled = this.enabled;
    }

    private static FogQualitySettings lowQuality
    {
      get => new FogQualitySettings() { enabled = true };
    }

    private static FogQualitySettings disabled
    {
      get => new FogQualitySettings() { enabled = false };
    }
  }
}
