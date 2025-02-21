// Decompiled with JetBrains decompiler
// Type: Game.Settings.RadioSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.Audio;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  public class RadioSettings : Setting
  {
    private Game.Audio.Radio.Radio m_Radio;

    [SettingsUIHidden]
    public bool enableSpectrum { get; set; }

    [SettingsUIHidden]
    public int spectrumNumSamples { get; set; }

    [SettingsUIHidden]
    public FFTWindow fftWindowType { get; set; }

    [SettingsUIHidden]
    public Game.Audio.Radio.Radio.Spectrum.BandType bandType { get; set; }

    [SettingsUIHidden]
    public float equalizerBarSpacing { get; set; }

    [SettingsUIHidden]
    public float equalizerSidesPadding { get; set; }

    public RadioSettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.spectrumNumSamples = 1024;
      this.enableSpectrum = false;
      this.fftWindowType = FFTWindow.BlackmanHarris;
      this.bandType = Game.Audio.Radio.Radio.Spectrum.BandType.TenBand;
      this.equalizerBarSpacing = 10.2f;
      this.equalizerSidesPadding = 4f;
    }

    public override void Apply()
    {
      base.Apply();
      if (this.m_Radio == null)
        this.m_Radio = AudioManager.instance.radio;
      if (this.m_Radio == null)
        return;
      this.m_Radio.SetSpectrumSettings(this.enableSpectrum, this.spectrumNumSamples, this.fftWindowType, this.bandType, this.equalizerBarSpacing, this.equalizerSidesPadding);
    }
  }
}
