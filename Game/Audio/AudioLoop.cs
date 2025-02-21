// Decompiled with JetBrains decompiler
// Type: Game.Audio.AudioLoop
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

#nullable disable
namespace Game.Audio
{
  public class AudioLoop
  {
    private const string kMenuCutoffProperty = "MenuCutoff";
    private AudioAsset m_Asset;
    private AudioSource[] m_AudioSource;
    private int m_ActiveAudioSource;
    private double m_NextCheck = -1.0;
    private AudioMixerGroup m_group;
    private AudioMixer m_Mixer;
    private float m_FadeOutTime;

    public AudioLoop(AudioAsset asset, AudioMixer mixer, AudioMixerGroup group)
    {
      this.m_Asset = asset;
      this.m_group = group;
      this.m_Mixer = mixer;
    }

    public float volume
    {
      get => this.m_AudioSource[0].volume;
      set
      {
        this.m_AudioSource[0].volume = value;
        if (this.m_AudioSource.Length <= 1 || !((Object) this.m_AudioSource[1] != (Object) null))
          return;
        this.m_AudioSource[1].volume = value;
      }
    }

    public async Task Start(bool useAlternativeStart = false)
    {
      this.m_FadeOutTime = 0.0f;
      this.m_Mixer.SetFloat("MenuCutoff", 22000f);
      AudioClip audioClip1 = await this.m_Asset.LoadAsync(false);
      if (!((Object) audioClip1 != (Object) null))
        return;
      this.m_NextCheck = -1.0;
      this.m_ActiveAudioSource = 0;
      if (this.m_AudioSource == null)
      {
        this.m_AudioSource = new AudioSource[this.m_Asset.hasLoop ? 2 : 1];
        GameObject go = new GameObject("MenuAudioSource");
        this.m_AudioSource[0] = go.AddComponent<AudioSource>();
        this.m_AudioSource[0].outputAudioMixerGroup = this.m_group;
        this.m_AudioSource[0].dopplerLevel = 0.0f;
        this.m_AudioSource[0].playOnAwake = false;
        this.m_AudioSource[0].spatialBlend = 0.0f;
        this.m_AudioSource[0].loop = !this.m_Asset.hasLoop;
        this.m_AudioSource[0].clip = audioClip1;
        if (this.m_Asset.hasLoop)
        {
          AudioClip audioClip2 = await this.m_Asset.LoadAsync(false);
          this.m_AudioSource[1] = go.AddComponent<AudioSource>();
          this.m_AudioSource[1].outputAudioMixerGroup = this.m_group;
          this.m_AudioSource[1].dopplerLevel = 0.0f;
          this.m_AudioSource[1].playOnAwake = false;
          this.m_AudioSource[1].spatialBlend = 0.0f;
          this.m_AudioSource[1].loop = false;
          this.m_AudioSource[1].clip = audioClip2;
        }
        go = (GameObject) null;
      }
      this.m_AudioSource[0].volume = 1f;
      if (useAlternativeStart && this.m_Asset.hasAlternativeStart)
        this.m_AudioSource[0].timeSamples = (int) (this.m_Asset.alternativeStart * (double) this.m_AudioSource[0].clip.frequency);
      if (this.m_Asset.hasLoop)
      {
        this.m_AudioSource[1].volume = 1f;
        this.m_NextCheck = AudioSettings.dspTime + this.m_Asset.loopEnd;
        if (useAlternativeStart && this.m_Asset.hasAlternativeStart)
          this.m_NextCheck -= this.m_Asset.alternativeStart;
      }
      this.m_AudioSource[0].PlayScheduled(AudioSettings.dspTime);
    }

    public void Update(double deltaTime)
    {
      if (this.m_AudioSource == null)
        return;
      if (this.m_Asset.hasLoop && this.m_NextCheck != -1.0 && AudioSettings.dspTime > this.m_NextCheck - 5.0)
      {
        int index = 1 - this.m_ActiveAudioSource;
        this.m_AudioSource[this.m_ActiveAudioSource].SetScheduledEndTime(this.m_NextCheck);
        this.m_AudioSource[index].timeSamples = (int) (this.m_Asset.loopStart * (double) this.m_AudioSource[index].clip.frequency);
        this.m_AudioSource[index].PlayScheduled(this.m_NextCheck);
        this.m_ActiveAudioSource = index;
        this.m_NextCheck += this.m_Asset.loopDuration;
      }
      if ((double) this.m_FadeOutTime > 0.0)
      {
        this.m_FadeOutTime -= (float) deltaTime;
        this.m_Mixer.SetFloat("MenuCutoff", math.lerp(400f, 22000f, math.saturate(math.pow(this.m_FadeOutTime, 3f))));
        this.volume = (double) this.m_Asset.fadeoutTime > 0.0 ? this.m_FadeOutTime / this.m_Asset.fadeoutTime : 0.0f;
      }
      else
      {
        if ((double) this.m_FadeOutTime >= 0.0)
          return;
        this.Dispose();
      }
    }

    public bool isPlaying
    {
      get
      {
        return (Object) this.m_AudioSource[this.m_ActiveAudioSource] != (Object) null && this.m_AudioSource[this.m_ActiveAudioSource].isPlaying;
      }
    }

    public double elapsedTime
    {
      get
      {
        return (double) this.m_AudioSource[this.m_ActiveAudioSource].timeSamples / (double) this.m_AudioSource[this.m_ActiveAudioSource].clip.frequency;
      }
    }

    public void FadeOut() => this.m_FadeOutTime = this.m_Asset.fadeoutTime;

    public void Stop()
    {
      if (this.m_AudioSource == null)
        return;
      foreach (AudioSource audioSource in this.m_AudioSource)
      {
        if ((Object) audioSource != (Object) null)
          audioSource.Stop();
      }
    }

    public void Dispose()
    {
      this.Stop();
      this.m_NextCheck = -1.0;
      if (this.m_AudioSource == null)
        return;
      if (this.m_AudioSource.Length > 1 && (Object) this.m_AudioSource[1] != (Object) null)
        Object.Destroy((Object) this.m_AudioSource[1].clip);
      if ((Object) this.m_AudioSource[0] != (Object) null)
      {
        Object.Destroy((Object) this.m_AudioSource[0].clip);
        Object.Destroy((Object) this.m_AudioSource[0].gameObject);
      }
      this.m_AudioSource = (AudioSource[]) null;
    }
  }
}
