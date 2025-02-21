// Decompiled with JetBrains decompiler
// Type: Game.Settings.AudioSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.Audio;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  [SettingsUIGroupOrder(new string[] {"Main", "Radio", "Advanced"})]
  public class AudioSettings : Setting
  {
    public const string kName = "Audio";
    private AudioManager m_AudioManager;
    private Game.Audio.Radio.Radio m_Radio;
    public const string kMainGroup = "Main";
    public const string kRadioGroup = "Radio";
    public const string kAdvancedGroup = "Advanced";

    [SettingsUISection("Main")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float masterVolume { get; set; }

    [SettingsUISection("Main")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float uiVolume { get; set; }

    [SettingsUISection("Main")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float menuVolume { get; set; }

    [SettingsUISection("Main")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float ingameVolume { get; set; }

    [SettingsUISection("Radio")]
    public bool radioActive { get; set; }

    [SettingsUISection("Radio")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float radioVolume { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("Advanced")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float ambienceVolume { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("Advanced")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float disastersVolume { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("Advanced")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float worldVolume { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("Advanced")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float audioGroupsVolume { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("Advanced")]
    [SettingsUISlider(min = 0.0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
    public float serviceBuildingsVolume { get; set; }

    [SettingsUIAdvanced]
    [SettingsUISection("Advanced")]
    [SettingsUISlider(min = 64f, max = 512f, step = 32f, unit = "dataMegabytes")]
    public int clipMemoryBudget { get; set; }

    public AudioSettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.masterVolume = 1f;
      this.uiVolume = 1f;
      this.menuVolume = 1f;
      this.ingameVolume = 1f;
      this.radioActive = true;
      this.radioVolume = 1f;
      this.ambienceVolume = 1f;
      this.disastersVolume = 1f;
      this.worldVolume = 1f;
      this.audioGroupsVolume = 1f;
      this.serviceBuildingsVolume = 1f;
      this.clipMemoryBudget = 256;
    }

    public override void Apply()
    {
      base.Apply();
      if (this.m_AudioManager == null)
        this.m_AudioManager = AudioManager.instance;
      if (this.m_Radio == null)
        this.m_Radio = AudioManager.instance.radio;
      if (this.m_AudioManager != null)
      {
        this.m_AudioManager.masterVolume = this.masterVolume;
        this.m_AudioManager.radioVolume = this.radioVolume;
        this.m_AudioManager.uiVolume = this.uiVolume;
        this.m_AudioManager.menuVolume = this.menuVolume;
        this.m_AudioManager.ingameVolume = this.ingameVolume;
        this.m_AudioManager.ambienceVolume = this.ambienceVolume;
        this.m_AudioManager.disastersVolume = this.disastersVolume;
        this.m_AudioManager.worldVolume = this.worldVolume;
        this.m_AudioManager.audioGroupsVolume = this.audioGroupsVolume;
        this.m_AudioManager.serviceBuildingsVolume = this.serviceBuildingsVolume;
      }
      if (this.m_Radio != null)
        this.m_Radio.isActive = this.radioActive;
      AudioManager.AudioSourcePool.memoryBudget = this.clipMemoryBudget * 1048576;
    }
  }
}
