// Decompiled with JetBrains decompiler
// Type: Game.Audio.Radio.Radio
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.Randomization;
using Colossal.UI.Binding;
using Game.Assets;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Settings;
using Game.Simulation;
using Game.Triggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Audio.Radio
{
  public class Radio
  {
    public Game.Audio.Radio.Radio.OnRadioEvent Reloaded;
    public Game.Audio.Radio.Radio.OnRadioEvent SettingsChanged;
    public Game.Audio.Radio.Radio.OnRadioEvent ProgramChanged;
    public Game.Audio.Radio.Radio.OnClipChanged ClipChanged;
    private const float kSimulationSecondsPerDay = 4369.067f;
    private const float kSimtimeToRealtime = 0.0505679026f;
    private static readonly string kAlertsTag = "type:Alerts";
    private static readonly string kAlertsIntroTag = "type:Alerts Intro";
    private static ILog log = LogManager.GetLogger(nameof (Radio));
    private const int kSecondsPerDay = 86400;
    private System.Collections.Generic.Dictionary<string, Game.Audio.Radio.Radio.RadioNetwork> m_Networks = new System.Collections.Generic.Dictionary<string, Game.Audio.Radio.Radio.RadioNetwork>();
    private System.Collections.Generic.Dictionary<string, Game.Audio.Radio.Radio.RuntimeRadioChannel> m_RadioChannels = new System.Collections.Generic.Dictionary<string, Game.Audio.Radio.Radio.RuntimeRadioChannel>();
    private Game.Audio.Radio.Radio.RuntimeRadioChannel m_CurrentChannel;
    private bool m_Paused;
    private bool m_Muted;
    private static readonly int kMaxHistoryLength = 5;
    private int m_ReplayIndex;
    private List<Game.Audio.Radio.Radio.ClipInfo> m_PlaylistHistory = new List<Game.Audio.Radio.Radio.ClipInfo>(Game.Audio.Radio.Radio.kMaxHistoryLength);
    private List<Game.Audio.Radio.Radio.ClipInfo> m_Queue = new List<Game.Audio.Radio.Radio.ClipInfo>(2);
    private Game.Audio.Radio.Radio.RadioPlayer m_RadioPlayer;
    private bool m_IsEnabled;
    private string m_LastSaveRadioChannel;
    private bool m_LastSaveRadioAdsState;
    private bool m_IsActive;
    private Game.Audio.Radio.Radio.RuntimeRadioChannel[] m_CachedRadioChannelDescriptors;
    private System.Collections.Generic.Dictionary<Game.Audio.Radio.Radio.SegmentType, Game.Audio.Radio.Radio.OnDemandClips> m_OnDemandClips;
    private const string kUniqueNameChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public Game.Audio.Radio.Radio.RuntimeRadioChannel currentChannel
    {
      get => this.m_CurrentChannel;
      set
      {
        if (this.m_CurrentChannel == value)
          return;
        this.m_CurrentChannel = value;
        if (this.currentClip.m_Emergency == Entity.Null)
          this.FinishCurrentClip();
        this.SetupOrSkipSegment();
        this.ClearQueue();
        this.m_ReplayIndex = 0;
      }
    }

    public bool paused
    {
      get => this.m_Paused;
      set
      {
        this.m_Paused = value;
        if (!(this.currentClip.m_Emergency == Entity.Null))
          return;
        if (this.m_Paused)
          this.m_RadioPlayer.Pause();
        else
          this.m_RadioPlayer.Unpause();
      }
    }

    public bool skipAds { get; set; }

    public bool muted
    {
      get => this.m_Muted;
      set
      {
        this.m_Muted = value;
        this.m_RadioPlayer.muted = value;
      }
    }

    public Game.Audio.Radio.Radio.ClipInfo currentClip { get; private set; }

    public bool isEnabled => this.m_IsEnabled;

    public bool isActive
    {
      get => this.m_IsActive;
      set
      {
        if (this.m_IsActive != value)
        {
          this.m_IsActive = value;
          Game.Audio.Radio.Radio.OnRadioEvent settingsChanged = this.SettingsChanged;
          if (settingsChanged != null)
            settingsChanged(this);
        }
        if (value)
          return;
        this.OnDisabled();
      }
    }

    public Game.Audio.Radio.Radio.RadioNetwork[] networkDescriptors
    {
      get => this.GetSortedUIDescriptor<Game.Audio.Radio.Radio.RadioNetwork>(this.m_Networks);
    }

    public Game.Audio.Radio.Radio.RuntimeRadioChannel[] radioChannelDescriptors
    {
      get
      {
        if (this.m_CachedRadioChannelDescriptors == null)
          this.m_CachedRadioChannelDescriptors = this.GetSortedUIDescriptor<Game.Audio.Radio.Radio.RuntimeRadioChannel>(this.m_RadioChannels);
        return this.m_CachedRadioChannelDescriptors;
      }
    }

    public string currentlyPlayingClipName => this.m_RadioPlayer.currentClipName;

    public double currentlyPlayingDuration => this.m_RadioPlayer.GetAudioSourceDuration();

    public double currentlyPlayingElapsed => this.m_RadioPlayer.GetAudioSourceTimeElapsed();

    public double currentlyPlayingRemaining => this.m_RadioPlayer.GetAudioSourceTimeRemaining();

    public int GetActiveSource() => 0;

    public double GetAudioSourceDuration(int i) => this.m_RadioPlayer.GetAudioSourceDuration();

    public double GetAudioSourceTimeElapsed(int i)
    {
      return this.m_RadioPlayer.GetAudioSourceTimeElapsed();
    }

    public double GetAudioSourceTimeRemaining(int i)
    {
      return this.m_RadioPlayer.GetAudioSourceTimeRemaining();
    }

    public double nextTimeCheck => 0.0;

    public AudioAsset pendingClip
    {
      get => this.m_Queue.Count <= 0 ? (AudioAsset) null : this.m_Queue[0].m_Asset;
    }

    public bool hasEmergency => this.currentClip.m_Emergency != Entity.Null;

    public Entity emergency => this.currentClip.m_Emergency;

    public Entity emergencyTarget => this.currentClip.m_EmergencyTarget;

    private T[] GetSortedUIDescriptor<T>(System.Collections.Generic.Dictionary<string, T> desc) where T : IComparable<T>
    {
      T[] array = desc.Values.ToArray<T>();
      System.Array.Sort<T>(array);
      return array;
    }

    public Game.Audio.Radio.Radio.RuntimeRadioChannel GetRadioChannel(string name)
    {
      Game.Audio.Radio.Radio.RuntimeRadioChannel runtimeRadioChannel;
      return this.m_RadioChannels.TryGetValue(name, out runtimeRadioChannel) ? runtimeRadioChannel : (Game.Audio.Radio.Radio.RuntimeRadioChannel) null;
    }

    public Radio(AudioMixerGroup radioGroup)
    {
      this.m_OnDemandClips = new System.Collections.Generic.Dictionary<Game.Audio.Radio.Radio.SegmentType, Game.Audio.Radio.Radio.OnDemandClips>();
      this.m_OnDemandClips[Game.Audio.Radio.Radio.SegmentType.Commercial] = new Game.Audio.Radio.Radio.OnDemandClips(this.GetCommercialClips);
      this.m_OnDemandClips[Game.Audio.Radio.Radio.SegmentType.PSA] = new Game.Audio.Radio.Radio.OnDemandClips(this.GetPSAClips);
      this.m_OnDemandClips[Game.Audio.Radio.Radio.SegmentType.Playlist] = new Game.Audio.Radio.Radio.OnDemandClips(this.GetPlaylistClips);
      this.m_OnDemandClips[Game.Audio.Radio.Radio.SegmentType.Talkshow] = new Game.Audio.Radio.Radio.OnDemandClips(this.GetTalkShowClips);
      this.m_OnDemandClips[Game.Audio.Radio.Radio.SegmentType.News] = new Game.Audio.Radio.Radio.OnDemandClips(this.GetNewsClips);
      this.m_OnDemandClips[Game.Audio.Radio.Radio.SegmentType.Weather] = new Game.Audio.Radio.Radio.OnDemandClips(this.GetWeatherClips);
      this.m_RadioPlayer = new Game.Audio.Radio.Radio.RadioPlayer(radioGroup);
      this.SettingsChanged += new Game.Audio.Radio.Radio.OnRadioEvent(this.OnSettingsChanged);
    }

    private void OnSettingsChanged(Game.Audio.Radio.Radio radio)
    {
      if (radio.isActive)
      {
        if (!((UnityEngine.Object) GameManager.instance != (UnityEngine.Object) null) || GameManager.instance.gameMode != GameMode.Game || !((UnityEngine.Object) Camera.main != (UnityEngine.Object) null) || radio.radioChannelDescriptors.Length == 0)
          return;
        radio.Enable(Camera.main.gameObject);
      }
      else
        this.Disable();
    }

    public void ForceRadioPause(bool pause)
    {
      if (pause)
        this.m_RadioPlayer.Pause();
      else
        this.m_RadioPlayer.Unpause();
    }

    public void SetSpectrumSettings(
      bool enabled,
      int numSamples,
      FFTWindow fftWindow,
      Game.Audio.Radio.Radio.Spectrum.BandType bandType,
      float spacing,
      float padding)
    {
      this.m_RadioPlayer.SetSpectrumSettings(enabled, numSamples, fftWindow, bandType, spacing, padding);
    }

    public Texture equalizerTexture => this.m_RadioPlayer.equalizerTexture;

    private bool CheckEntitlement(IContentPrerequisite target)
    {
      return target.contentPrerequisite == null || ((IEnumerable<string>) target.contentPrerequisite).All<string>((Func<string, bool>) (x => PlatformManager.instance.IsDlcOwned(PlatformManager.instance.GetDlcId(x))));
    }

    private void LoadRadio(bool enable)
    {
      try
      {
        this.Clear();
        using (Colossal.PerformanceCounter.Start((Action<TimeSpan>) (t => Game.Audio.Radio.Radio.log.DebugFormat("Loaded radio configuration in {0}ms", (object) t.TotalMilliseconds))))
        {
          Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings<Game.Audio.Radio.Radio.RadioNetwork>("Radio Network", (Action<Game.Audio.Radio.Radio.RadioNetwork, SourceMeta>) ((network, meta) =>
          {
            if (!this.CheckEntitlement((IContentPrerequisite) network))
              return;
            this.m_Networks.Add(network.name, network);
          }));
          Colossal.IO.AssetDatabase.AssetDatabase.global.LoadSettings<Game.Audio.Radio.Radio.RadioChannel>("Radio Channel", (Action<Game.Audio.Radio.Radio.RadioChannel, SourceMeta>) ((channel, meta) =>
          {
            if (!this.CheckEntitlement((IContentPrerequisite) channel))
              return;
            string str = channel.name;
            while (this.m_RadioChannels.ContainsKey(str))
              str = str + "_" + Game.Audio.Radio.Radio.MakeUniqueRandomName(str, 4);
            Game.Audio.Radio.Radio.log.InfoFormat("Radio channel id '{0}' added", (object) str);
            this.m_RadioChannels.Add(str, channel.CreateRuntime(meta.path));
          }));
        }
        this.LogRadio();
        if (!enable)
          return;
        this.Enable(Camera.main.gameObject);
      }
      catch (Exception ex)
      {
        Game.Audio.Radio.Radio.log.Error(ex);
      }
    }

    private void Clear()
    {
      this.m_CachedRadioChannelDescriptors = (Game.Audio.Radio.Radio.RuntimeRadioChannel[]) null;
      this.m_Networks.Clear();
      this.m_RadioChannels.Clear();
      this.currentChannel = (Game.Audio.Radio.Radio.RuntimeRadioChannel) null;
      this.OnDisabled();
    }

    public void Reload(bool enable = true)
    {
      this.LoadRadio(enable);
      Game.Audio.Radio.Radio.OnRadioEvent reloaded = this.Reloaded;
      if (reloaded == null)
        return;
      reloaded(this);
    }

    public void RestoreRadioSettings(string savedChannel, bool savedAds)
    {
      this.m_LastSaveRadioChannel = savedChannel;
      this.m_LastSaveRadioAdsState = savedAds;
    }

    public void Enable(GameObject listener)
    {
      if (this.currentChannel == null)
      {
        Game.Audio.Radio.Radio.RuntimeRadioChannel runtimeRadioChannel;
        if (this.m_LastSaveRadioChannel != null && this.m_RadioChannels.TryGetValue(this.m_LastSaveRadioChannel, out runtimeRadioChannel))
        {
          this.currentChannel = runtimeRadioChannel;
          this.skipAds = this.m_LastSaveRadioAdsState;
        }
        else
        {
          this.skipAds = false;
          this.currentChannel = this.radioChannelDescriptors[0];
        }
      }
      if (!this.m_IsActive || this.m_IsEnabled || !((UnityEngine.Object) listener != (UnityEngine.Object) null))
        return;
      this.m_RadioPlayer.Create(listener);
      this.m_RadioPlayer.muted = this.m_Muted;
      this.SetSpectrumSettings(SharedSettings.instance.radio.enableSpectrum, SharedSettings.instance.radio.spectrumNumSamples, SharedSettings.instance.radio.fftWindowType, SharedSettings.instance.radio.bandType, SharedSettings.instance.radio.equalizerBarSpacing, SharedSettings.instance.radio.equalizerSidesPadding);
      this.m_IsEnabled = true;
    }

    public void Disable()
    {
      this.m_RadioPlayer?.Dispose();
      this.m_IsEnabled = false;
      this.OnDisabled();
    }

    private void OnDisabled()
    {
      this.FinishCurrentClip();
      this.ClearQueue(true);
      this.m_ReplayIndex = 0;
      if (this.currentClip.m_LoadTask != null)
        this.currentClip.m_Asset.Unload(false);
      this.currentClip = new Game.Audio.Radio.Radio.ClipInfo();
      this.m_PlaylistHistory.Clear();
    }

    public void Update(float normalizedTime)
    {
      if (this.isActive)
      {
        if (this.isEnabled)
        {
          try
          {
            this.m_RadioPlayer.UpdateSpectrum();
            int timeOfDaySeconds = Mathf.RoundToInt((float) ((double) normalizedTime * 24.0 * 3600.0));
            bool flag1 = false;
            bool flag2 = false;
            foreach (Game.Audio.Radio.Radio.RuntimeRadioChannel channelDescriptor in this.radioChannelDescriptors)
            {
              bool flag3 = channelDescriptor.Update(timeOfDaySeconds);
              if (channelDescriptor == this.currentChannel)
                flag1 = flag3;
              flag2 |= flag3;
            }
            if (flag1)
            {
              Game.Audio.Radio.Radio.log.DebugFormat("Program changed callback for on-demand clips initialization");
              this.SetupOrSkipSegment();
            }
            this.QueueEmergencyClips();
            this.ValidateQueue();
            if (this.m_Queue.Count > 0)
            {
              Game.Audio.Radio.Radio.ClipInfo clipInfo = this.m_Queue[0];
              if (this.currentClip.m_Emergency == Entity.Null && clipInfo.m_Emergency != Entity.Null)
              {
                if (clipInfo.m_LoadTask != null && clipInfo.m_LoadTask.IsCompleted)
                {
                  this.m_RadioPlayer.Unpause();
                  Game.Audio.Radio.Radio.ClipInfo currentClip = this.currentClip with
                  {
                    m_ResumeAtPosition = this.m_RadioPlayer.playbackPosition
                  };
                  this.m_RadioPlayer.Play(clipInfo.m_LoadTask.Result);
                  this.currentClip = clipInfo;
                  this.m_Queue.RemoveAt(0);
                  this.QueueClip(currentClip);
                  this.InvokeClipcallback(this.currentClip.m_Asset);
                }
              }
              else if (this.m_RadioPlayer.GetAudioSourceTimeRemaining() <= 0.0 && clipInfo.m_LoadTask != null && clipInfo.m_LoadTask.IsCompleted)
              {
                if (clipInfo.m_SegmentType == Game.Audio.Radio.Radio.SegmentType.Commercial && this.skipAds)
                {
                  clipInfo.m_Asset.Unload(false);
                  this.m_Queue.RemoveAt(0);
                }
                else
                {
                  this.m_RadioPlayer.Play(clipInfo.m_LoadTask.Result, clipInfo.m_ResumeAtPosition >= 0 ? clipInfo.m_ResumeAtPosition : 0);
                  if (this.currentClip.m_LoadTask != null)
                    this.currentClip.m_Asset.Unload(false);
                  this.currentClip = clipInfo;
                  if (this.currentClip.m_SegmentType == Game.Audio.Radio.Radio.SegmentType.Playlist && this.currentClip.m_ResumeAtPosition < 0 && !this.currentClip.m_Replaying)
                  {
                    this.m_PlaylistHistory.Insert(0, this.currentClip with
                    {
                      m_LoadTask = (Task<AudioClip>) null
                    });
                    while (this.m_PlaylistHistory.Count > Game.Audio.Radio.Radio.kMaxHistoryLength)
                      this.m_PlaylistHistory.RemoveAt(this.m_PlaylistHistory.Count - 1);
                  }
                  if (!this.currentClip.m_Replaying)
                    this.m_ReplayIndex = 0;
                  this.m_Queue.RemoveAt(0);
                  if (this.paused && this.currentClip.m_Emergency == Entity.Null)
                    this.m_RadioPlayer.Pause();
                  this.InvokeClipcallback(this.currentClip.m_Asset);
                }
              }
            }
            this.QueueNextClip();
            if (!flag2)
              return;
            this.ProgramChanged(this);
            return;
          }
          catch (Exception ex)
          {
            Game.Audio.Radio.Radio.log.Fatal(ex);
            return;
          }
        }
      }
      this.ClearEmergencyQueue();
    }

    private void QueueClip(Game.Audio.Radio.Radio.ClipInfo clip, bool pushToFront = false)
    {
      if (((clip.m_Emergency != Entity.Null ? 1 : (clip.m_ResumeAtPosition >= 0 ? 1 : 0)) | (pushToFront ? 1 : 0)) != 0)
      {
        int index = this.m_Queue.FindIndex((Predicate<Game.Audio.Radio.Radio.ClipInfo>) (info => info.m_Emergency == Entity.Null));
        this.m_Queue.Insert(index < 0 ? this.m_Queue.Count : index, clip);
      }
      else
        this.m_Queue.Add(clip);
    }

    private void ValidateQueue()
    {
      for (int index = 0; index < this.m_Queue.Count; ++index)
      {
        while (index < this.m_Queue.Count && (this.m_Queue[index].m_LoadTask == null || this.m_Queue[index].m_LoadTask.IsFaulted || this.m_Queue[index].m_LoadTask.IsCanceled))
        {
          if (this.m_Queue[index].m_LoadTask != null)
            this.m_Queue[index].m_Asset.Unload(false);
          this.m_Queue.RemoveAt(index);
        }
      }
    }

    private void ClearQueue(bool clearEmergencies = false)
    {
      for (int index = 0; index < this.m_Queue.Count; ++index)
      {
        if (this.m_Queue[index].m_LoadTask != null && (clearEmergencies || this.m_Queue[index].m_Emergency == Entity.Null))
          this.m_Queue[index].m_Asset.Unload(false);
      }
      if (clearEmergencies)
        this.m_Queue.Clear();
      else
        this.m_Queue.RemoveAll((Predicate<Game.Audio.Radio.Radio.ClipInfo>) (clip => clip.m_Emergency == Entity.Null));
    }

    private void ClearEmergencyQueue()
    {
      JobHandle deps;
      NativeQueue<RadioTag> emergencyQueue = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<RadioTagSystem>().GetEmergencyQueue(out deps);
      deps.Complete();
      emergencyQueue.Clear();
    }

    private void QueueEmergencyClips()
    {
      JobHandle deps;
      NativeQueue<RadioTag> emergencyQueue = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<RadioTagSystem>().GetEmergencyQueue(out deps);
      deps.Complete();
      while (emergencyQueue.Count > 0)
      {
        RadioTag tag = emergencyQueue.Dequeue();
        if (!this.IsEmergencyClipInQueue(tag))
        {
          List<AudioAsset> audioAssetList = new List<AudioAsset>();
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().GetPrefab<PrefabBase>(tag.m_Event);
          foreach (AudioAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => asset.ContainsTag(Game.Audio.Radio.Radio.kAlertsTag)))))
          {
            if (asset.GetMetaTag(AudioAsset.Metatag.AlertType) == prefab.name)
              audioAssetList.Add(asset);
          }
          if (audioAssetList.Count > 0)
          {
            if (!this.AlertPlayingOrQueued())
              this.QueueEmergencyIntroClip(tag.m_Event, tag.m_Target);
            AudioAsset audioAsset = audioAssetList[new Unity.Mathematics.Random((uint) DateTime.Now.Ticks).NextInt(0, audioAssetList.Count)];
            this.QueueClip(new Game.Audio.Radio.Radio.ClipInfo()
            {
              m_Asset = audioAsset,
              m_Emergency = tag.m_Event,
              m_EmergencyTarget = tag.m_Target,
              m_SegmentType = Game.Audio.Radio.Radio.SegmentType.Emergency,
              m_LoadTask = audioAsset.LoadAsync(),
              m_ResumeAtPosition = -1
            });
          }
        }
      }
      emergencyQueue.Clear();
    }

    private void QueueEmergencyIntroClip(Entity emergency, Entity emergencyTarget)
    {
      List<AudioAsset> audioAssetList = new List<AudioAsset>();
      foreach (AudioAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => asset.ContainsTag(Game.Audio.Radio.Radio.kAlertsIntroTag)))))
        audioAssetList.Add(asset);
      if (audioAssetList.Count <= 0)
        return;
      AudioAsset audioAsset = audioAssetList[new Unity.Mathematics.Random((uint) DateTime.Now.Ticks).NextInt(0, audioAssetList.Count)];
      this.QueueClip(new Game.Audio.Radio.Radio.ClipInfo()
      {
        m_Asset = audioAsset,
        m_Emergency = emergency,
        m_EmergencyTarget = emergencyTarget,
        m_SegmentType = Game.Audio.Radio.Radio.SegmentType.Emergency,
        m_LoadTask = audioAsset.LoadAsync(),
        m_ResumeAtPosition = -1
      });
    }

    private bool IsEmergencyClipInQueue(RadioTag tag)
    {
      if (this.currentClip.m_Emergency != Entity.Null && this.currentClip.m_Emergency == tag.m_Event)
        return true;
      for (int index = 0; index < this.m_Queue.Count; ++index)
      {
        if (this.m_Queue[index].m_Emergency != Entity.Null && this.m_Queue[index].m_Emergency == tag.m_Event)
          return true;
      }
      return false;
    }

    private bool AlertPlayingOrQueued()
    {
      if (this.currentClip.m_Emergency != Entity.Null)
        return true;
      return this.m_Queue.Count > 0 && this.m_Queue[0].m_Emergency != Entity.Null;
    }

    private void QueueNextClip()
    {
      if (this.m_Queue.Count != 0 || !((AssetData) this.currentChannel?.currentProgram?.currentSegment?.currentClip != (IAssetData) null))
        return;
      this.QueueClip(new Game.Audio.Radio.Radio.ClipInfo()
      {
        m_Asset = this.currentChannel.currentProgram.currentSegment.currentClip,
        m_SegmentType = this.currentChannel.currentProgram.currentSegment.type,
        m_Emergency = Entity.Null,
        m_LoadTask = this.currentChannel.currentProgram.currentSegment.currentClip.LoadAsync(),
        m_ResumeAtPosition = -1
      });
      this.SetupNextClip();
    }

    private void GetPSAClips(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      Game.Audio.Radio.Radio.RadioNetwork radioNetwork;
      if (!this.m_Networks.TryGetValue(this.currentChannel.network, out radioNetwork) || radioNetwork.allowAds)
        return;
      List<AudioAsset> eventClips = this.GetEventClips(segment, AudioAsset.Metatag.PSAType);
      segment.clips = (IReadOnlyList<AudioAsset>) eventClips;
      this.Log(segment);
    }

    private void GetNewsClips(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      List<AudioAsset> eventClips = this.GetEventClips(segment, AudioAsset.Metatag.NewsType);
      segment.clips = (IReadOnlyList<AudioAsset>) eventClips;
      this.Log(segment);
    }

    private void GetWeatherClips(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      List<AudioAsset> eventClips = this.GetEventClips(segment, AudioAsset.Metatag.WeatherType, true, true);
      segment.clips = (IReadOnlyList<AudioAsset>) eventClips;
      this.Log(segment);
    }

    private List<AudioAsset> GetEventClips(
      Game.Audio.Radio.Radio.RuntimeSegment segment,
      AudioAsset.Metatag metatag,
      bool newestFirst = false,
      bool flush = false)
    {
      RadioTagSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<RadioTagSystem>();
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();
      List<AudioAsset> eventClips = new List<AudioAsset>(segment.clipsCap);
      List<AudioAsset> audioAssetList = new List<AudioAsset>();
      RadioTag radioTag;
      while (eventClips.Count < segment.clipsCap && existingSystemManaged.TryPopEvent(segment.type, newestFirst, out radioTag))
      {
        audioAssetList.Clear();
        foreach (AudioAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => ((IEnumerable<string>) segment.tags).All<string>(new Func<string, bool>(((AssetData) asset).ContainsTag))))))
        {
          // ISSUE: reference to a compiler-generated method
          if (asset.GetMetaTag(metatag) == systemManaged.GetPrefab<PrefabBase>(radioTag.m_Event).name)
            audioAssetList.Add(asset);
        }
        if (audioAssetList.Count > 0)
          eventClips.Add(audioAssetList[new Unity.Mathematics.Random((uint) DateTime.Now.Ticks).NextInt(0, audioAssetList.Count)]);
      }
      if (flush)
        existingSystemManaged.FlushEvents(segment.type);
      return eventClips;
    }

    private void GetCommercialClips(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      Game.Audio.Radio.Radio.RadioNetwork radioNetwork;
      if (!this.m_Networks.TryGetValue(this.currentChannel.network, out radioNetwork) || !radioNetwork.allowAds)
        return;
      WeightedRandom<AudioAsset> weightedRandom = new WeightedRandom<AudioAsset>();
      System.Collections.Generic.Dictionary<string, List<AudioAsset>> map = new System.Collections.Generic.Dictionary<string, List<AudioAsset>>();
      foreach (AudioAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => ((IEnumerable<string>) segment.tags).All<string>(new Func<string, bool>(((AssetData) asset).ContainsTag))))))
      {
        string metaTag = asset.GetMetaTag(AudioAsset.Metatag.Brand);
        if (metaTag != null)
        {
          List<AudioAsset> audioAssetList;
          if (!map.TryGetValue(metaTag, out audioAssetList))
          {
            audioAssetList = new List<AudioAsset>();
            map.Add(metaTag, audioAssetList);
          }
          audioAssetList.Add(asset);
        }
        else
          Game.Audio.Radio.Radio.log.ErrorFormat("Asset {0} ({1}) does not contain a brand metatag (for Commercial segment)", (object) asset.guid, (object) (asset.GetMetaTag(AudioAsset.Metatag.Title) ?? "<No title>"));
      }
      this.LogMap(map);
      JobHandle dependency;
      // ISSUE: reference to a compiler-generated method
      NativeList<BrandPopularitySystem.BrandPopularity> brandPopularity = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BrandPopularitySystem>().ReadBrandPopularity(out dependency);
      dependency.Complete();
      this.LogBrandPopularity(brandPopularity);
      for (int index = 0; index < brandPopularity.Length; ++index)
      {
        BrandPrefab prefab;
        List<AudioAsset> key;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().TryGetPrefab<BrandPrefab>(brandPopularity[index].m_BrandPrefab, out prefab) && map.TryGetValue(prefab.name, out key))
        {
          // ISSUE: reference to a compiler-generated field
          weightedRandom.AddRange((IEnumerable<AudioAsset>) key, brandPopularity[index].m_Popularity);
        }
      }
      List<AudioAsset> audioAssetList1 = new List<AudioAsset>();
      for (int index = 0; index < segment.clipsCap; ++index)
      {
        AudioAsset audioAsset = weightedRandom.NextAndRemove();
        if ((AssetData) audioAsset != (IAssetData) null)
          audioAssetList1.Add(audioAsset);
      }
      segment.clips = (IReadOnlyList<AudioAsset>) audioAssetList1;
      this.Log(segment);
    }

    private void GetPlaylistClips(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      segment.clips = (IReadOnlyList<AudioAsset>) this.GetSegmentAudioClip(segment.clipsCap, segment.tags, segment.type);
    }

    private void GetTalkShowClips(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      segment.clips = (IReadOnlyList<AudioAsset>) this.GetSegmentAudioClip(segment.clipsCap, segment.tags, segment.type);
    }

    private AudioAsset[] GetSegmentAudioClip(
      int clipsCap,
      string[] requiredTags,
      Game.Audio.Radio.Radio.SegmentType segmentType)
    {
      IEnumerable<AudioAsset> assets = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => ((IEnumerable<string>) requiredTags).All<string>(new Func<string, bool>(((AssetData) asset).ContainsTag)))));
      List<AudioAsset> audioAssetList = new List<AudioAsset>();
      audioAssetList.AddRange(assets);
      System.Random rnd = new System.Random();
      List<int> list = Enumerable.Range(0, audioAssetList.Count).OrderBy<int, int>((Func<int, int>) (x => rnd.Next())).Take<int>(clipsCap).ToList<int>();
      AudioAsset[] segmentAudioClip = new AudioAsset[clipsCap];
      for (int index = 0; index < segmentAudioClip.Length; ++index)
        segmentAudioClip[index] = audioAssetList[list[index]];
      return segmentAudioClip;
    }

    private void LogMap(System.Collections.Generic.Dictionary<string, List<AudioAsset>> map)
    {
      if (!Game.Audio.Radio.Radio.log.isDebugEnabled)
        return;
      string message = "Audio asset map:\n";
      foreach (KeyValuePair<string, List<AudioAsset>> keyValuePair in map)
      {
        message = message + keyValuePair.Key + "\n";
        foreach (AudioAsset audioAsset in keyValuePair.Value)
          message += string.Format("  {0} ({1})\n", (object) audioAsset.GetMetaTag(AudioAsset.Metatag.Title), (object) audioAsset.guid);
      }
      Game.Audio.Radio.Radio.log.Verbose((object) message);
    }

    private void LogBrandPopularity(
      NativeList<BrandPopularitySystem.BrandPopularity> brandPopularity)
    {
      if (!Game.Audio.Radio.Radio.log.isDebugEnabled)
        return;
      string message = "Brands popularity:\n";
      for (int index = 0; index < brandPopularity.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        string name = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().GetPrefab<BrandPrefab>(brandPopularity[index].m_BrandPrefab).name;
        // ISSUE: reference to a compiler-generated field
        message += string.Format("{0} - {1}\n", (object) name, (object) brandPopularity[index].m_Popularity);
      }
      Game.Audio.Radio.Radio.log.Verbose((object) message);
    }

    private bool SetupNextClip()
    {
      if (this.currentChannel?.currentProgram?.currentSegment == null)
        return false;
      if (!this.currentChannel.currentProgram.currentSegment.GoToNextClip())
      {
        this.currentChannel.currentProgram.GoToNextSegment();
        if (!this.SetupOrSkipSegment())
          return false;
      }
      return true;
    }

    private bool SetupOrSkipSegment()
    {
      if (this.currentChannel?.currentProgram == null)
        return false;
      Game.Audio.Radio.Radio.RuntimeProgram currentProgram = this.currentChannel.currentProgram;
      do
      {
        Game.Audio.Radio.Radio.RuntimeSegment currentSegment = currentProgram.currentSegment;
        if (currentSegment == null)
          return false;
        Game.Audio.Radio.Radio.OnDemandClips onDemandClips;
        if (this.m_OnDemandClips.TryGetValue(currentSegment.type, out onDemandClips))
          onDemandClips(currentSegment);
        if (currentSegment.clips.Count != 0)
          goto label_10;
      }
      while (currentProgram.GoToNextSegment());
      return false;
label_10:
      return true;
    }

    private void InvokeClipcallback(AudioAsset currentAsset)
    {
      try
      {
        Game.Audio.Radio.Radio.OnClipChanged clipChanged = this.ClipChanged;
        if (clipChanged == null)
          return;
        clipChanged(this, currentAsset);
      }
      catch (Exception ex)
      {
        Game.Audio.Radio.Radio.log.Critical(ex);
      }
    }

    public void NextSong()
    {
      if (this.m_ReplayIndex > 0)
      {
        --this.m_ReplayIndex;
        Game.Audio.Radio.Radio.ClipInfo clip = this.m_PlaylistHistory[this.m_ReplayIndex] with
        {
          m_Replaying = true
        };
        clip.m_LoadTask = clip.m_Asset.LoadAsync();
        this.QueueClip(clip, true);
      }
      this.FinishCurrentClip();
    }

    public void PreviousSong()
    {
      if (this.m_RadioPlayer.GetAudioSourceTimeElapsed() > 2.0 || this.m_ReplayIndex >= this.m_PlaylistHistory.Count - 1)
      {
        this.m_RadioPlayer.Rewind();
      }
      else
      {
        ++this.m_ReplayIndex;
        Game.Audio.Radio.Radio.ClipInfo clip = this.m_PlaylistHistory[this.m_ReplayIndex] with
        {
          m_Replaying = true
        };
        clip.m_LoadTask = clip.m_Asset.LoadAsync();
        this.QueueClip(clip, true);
        this.FinishCurrentClip();
      }
    }

    private void FinishCurrentClip() => this.m_RadioPlayer.Play((AudioClip) null);

    private static void SupportValueTypesForAOT() => JSON.SupportTypeForAOT<Game.Audio.Radio.Radio.SegmentType>();

    private static string MakeUniqueName(string name, int length)
    {
      char[] chArray = new char[length];
      for (int index = 0; index < name.Length; ++index)
        chArray[index % (length - 1)] += name[index];
      for (int index = 0; index < chArray.Length; ++index)
        chArray[index] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[(int) chArray[index] % "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length];
      return new string(chArray);
    }

    private static string MakeUniqueRandomName(string name, int length)
    {
      char[] chArray = new char[length];
      for (int index = 0; index < chArray.Length; ++index)
        chArray[index] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[UnityEngine.Random.Range(0, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length) % "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length];
      return new string(chArray);
    }

    private void Log(Game.Audio.Radio.Radio.RadioNetwork network)
    {
      Game.Audio.Radio.Radio.log.Debug((object) ("name: " + network.name));
      using (Game.Audio.Radio.Radio.log.indent.scoped)
      {
        Game.Audio.Radio.Radio.log.Verbose((object) ("description: " + network.description));
        Game.Audio.Radio.Radio.log.Verbose((object) ("icon: " + network.icon));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("uiPriority: {0}", (object) network.uiPriority));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("allowAds: {0}", (object) network.allowAds));
      }
    }

    private void Log(Game.Audio.Radio.Radio.RuntimeRadioChannel channel)
    {
      Game.Audio.Radio.Radio.log.Debug((object) ("name: " + channel.name));
      using (Game.Audio.Radio.Radio.log.indent.scoped)
      {
        Game.Audio.Radio.Radio.log.Verbose((object) ("description: " + channel.description));
        Game.Audio.Radio.Radio.log.Verbose((object) ("icon: " + channel.icon));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("uiPriority: {0}", (object) channel.uiPriority));
        Game.Audio.Radio.Radio.log.Verbose((object) ("network: " + channel.network));
        Game.Audio.Radio.Radio.log.DebugFormat("Programs ({0})", (object) channel.schedule.Length);
        using (Game.Audio.Radio.Radio.log.indent.scoped)
        {
          foreach (Game.Audio.Radio.Radio.RuntimeProgram program in channel.schedule)
            this.Log(program);
        }
      }
    }

    private void Log(AudioAsset clip)
    {
      if ((AssetData) clip == (IAssetData) null)
        Game.Audio.Radio.Radio.log.Debug((object) "id: <missing>");
      else
        Game.Audio.Radio.Radio.log.Debug((object) string.Format("id: {0} tags: {1} duration: {2}", (object) clip.guid, (object) string.Join(", ", (IEnumerable<string>) clip.tags), (object) FormatUtils.FormatTimeDebug(clip.durationMs)));
    }

    private void Log(Game.Audio.Radio.Radio.RuntimeProgram program)
    {
      Game.Audio.Radio.Radio.log.Debug((object) ("name: " + program.name + " [" + FormatUtils.FormatTimeDebug(program.startTime) + " -> " + FormatUtils.FormatTimeDebug(program.endTime) + "]"));
      using (Game.Audio.Radio.Radio.log.indent.scoped)
      {
        Game.Audio.Radio.Radio.log.Verbose((object) ("description: " + program.description));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("estimatedStart: {0} ({1}s)", (object) FormatUtils.FormatTimeDebug(program.startTime), (object) program.startTime));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("estimatedEnd: {0} ({1}s)", (object) FormatUtils.FormatTimeDebug(program.endTime), (object) program.endTime));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("loopProgram: {0}", (object) program.loopProgram));
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("estimatedDuration: {0} ({1}s) (realtime at x1: {2})", (object) FormatUtils.FormatTimeDebug(program.duration), (object) program.duration, (object) FormatUtils.FormatTimeDebug((int) ((double) program.duration * 0.050567902624607086))));
        Game.Audio.Radio.Radio.log.DebugFormat("Segments ({0})", (object) program.segments.Count);
        using (Game.Audio.Radio.Radio.log.indent.scoped)
        {
          foreach (Game.Audio.Radio.Radio.RuntimeSegment segment in (IEnumerable<Game.Audio.Radio.Radio.RuntimeSegment>) program.segments)
            this.Log(segment);
        }
      }
    }

    private void Log(Game.Audio.Radio.Radio.RuntimeSegment segment)
    {
      Game.Audio.Radio.Radio.log.Debug((object) string.Format("type: {0}", (object) segment.type));
      using (Game.Audio.Radio.Radio.log.indent.scoped)
      {
        if (segment.tags != null)
          Game.Audio.Radio.Radio.log.Debug((object) ("tags: " + string.Join(", ", segment.tags)));
        if (segment.clips == null)
          return;
        Game.Audio.Radio.Radio.log.Verbose((object) string.Format("duration total: {0}ms ({1})", (object) segment.durationMs, (object) FormatUtils.FormatTimeDebug(segment.durationMs)));
        Game.Audio.Radio.Radio.log.DebugFormat("Clips ({0})", (object) segment.clips.Count);
        using (Game.Audio.Radio.Radio.log.indent.scoped)
        {
          foreach (AudioAsset clip in (IEnumerable<AudioAsset>) segment.clips)
            this.Log(clip);
        }
        Game.Audio.Radio.Radio.log.DebugFormat("Clips cap: {0}", (object) segment.clipsCap);
      }
    }

    private void LogRadio()
    {
      if (!Game.Audio.Radio.Radio.log.isDebugEnabled)
        return;
      Game.Audio.Radio.Radio.log.DebugFormat("Networks ({0})", (object) this.m_Networks.Count);
      using (Game.Audio.Radio.Radio.log.indent.scoped)
      {
        foreach (Game.Audio.Radio.Radio.RadioNetwork network in this.m_Networks.Values)
          this.Log(network);
      }
      Game.Audio.Radio.Radio.log.DebugFormat("Channels ({0})", (object) this.m_RadioChannels.Count);
      using (Game.Audio.Radio.Radio.log.indent.scoped)
      {
        foreach (Game.Audio.Radio.Radio.RuntimeRadioChannel channel in this.m_RadioChannels.Values)
          this.Log(channel);
      }
    }

    public struct ClipInfo
    {
      public AudioAsset m_Asset;
      public Game.Audio.Radio.Radio.SegmentType m_SegmentType;
      public Entity m_Emergency;
      public Entity m_EmergencyTarget;
      public Task<AudioClip> m_LoadTask;
      public int m_ResumeAtPosition;
      public bool m_Replaying;
    }

    public delegate void OnRadioEvent(Game.Audio.Radio.Radio radio);

    public delegate void OnClipChanged(Game.Audio.Radio.Radio radio, AudioAsset asset);

    public delegate void OnDemandClips(Game.Audio.Radio.Radio.RuntimeSegment segment);

    public class RadioChannel : IContentPrerequisite
    {
      public string name;
      [CanBeNull]
      public string nameId;
      public string description;
      public string icon;
      public int uiPriority;
      public string network;
      public Game.Audio.Radio.Radio.Program[] programs;

      public string[] contentPrerequisite { get; set; }

      public Game.Audio.Radio.Radio.RuntimeRadioChannel CreateRuntime(string path)
      {
        Game.Audio.Radio.Radio.RuntimeRadioChannel runtime = new Game.Audio.Radio.Radio.RuntimeRadioChannel();
        runtime.name = this.name;
        runtime.description = this.description;
        runtime.icon = this.icon;
        runtime.uiPriority = this.uiPriority;
        runtime.network = this.network;
        runtime.Initialize(this, this.name + " (" + path + ")");
        return runtime;
      }
    }

    public class RuntimeRadioChannel : IComparable<Game.Audio.Radio.Radio.RuntimeRadioChannel>, IJsonWritable
    {
      public string name;
      public string description;
      public string icon;
      public int uiPriority;
      public string network;
      private readonly Game.Audio.Radio.Radio.RuntimeProgram kNoProgram = new Game.Audio.Radio.Radio.RuntimeProgram()
      {
        name = "No program"
      };

      public Game.Audio.Radio.Radio.RuntimeProgram currentProgram { get; private set; }

      public Game.Audio.Radio.Radio.RuntimeProgram[] schedule { get; private set; }

      public void Initialize(Game.Audio.Radio.Radio.RadioChannel radioChannel, string path)
      {
        this.BuildRuntimePrograms(radioChannel.programs, path);
      }

      public bool Update(int timeOfDaySeconds)
      {
        bool flag1 = false;
        bool flag2 = false;
        for (int index = 0; index < this.schedule.Length; ++index)
        {
          Game.Audio.Radio.Radio.RuntimeProgram runtimeProgram = this.schedule[index];
          if (timeOfDaySeconds >= runtimeProgram.startTime && timeOfDaySeconds < runtimeProgram.endTime && (runtimeProgram.loopProgram || !runtimeProgram.hasEnded))
          {
            if (this.currentProgram != runtimeProgram || runtimeProgram.hasEnded && runtimeProgram.loopProgram)
            {
              Game.Audio.Radio.Radio.log.DebugFormat("Channel {1} - Program changed to {0}", (object) runtimeProgram.name, (object) this.name);
              flag1 = true;
              runtimeProgram.Reset();
            }
            runtimeProgram.active = true;
            this.currentProgram = runtimeProgram;
            flag2 = true;
          }
          else
            runtimeProgram.active = false;
        }
        if (!flag2)
          this.currentProgram = (Game.Audio.Radio.Radio.RuntimeProgram) null;
        return flag1;
      }

      private bool IsValidTimestamp(int start, int end) => start != -1 && end != -1 && start <= end;

      private Game.Audio.Radio.Radio.RuntimeProgram CreateRuntimeProgram(
        Game.Audio.Radio.Radio.Program p,
        int startSecs,
        int endSecs,
        string path)
      {
        Game.Audio.Radio.Radio.RuntimeProgram runtimeProgram = new Game.Audio.Radio.Radio.RuntimeProgram();
        runtimeProgram.name = p.name;
        runtimeProgram.description = p.description;
        runtimeProgram.startTime = startSecs;
        runtimeProgram.endTime = endSecs;
        runtimeProgram.loopProgram = p.loopProgram;
        runtimeProgram.BuildRuntimeSegments(p, path);
        return runtimeProgram;
      }

      private Game.Audio.Radio.Radio.RuntimeProgram ShallowCopyRuntimeProgram(
        Game.Audio.Radio.Radio.RuntimeProgram p,
        int startSecs,
        int endSecs)
      {
        return new Game.Audio.Radio.Radio.RuntimeProgram()
        {
          name = p.name,
          description = p.description,
          startTime = startSecs,
          endTime = endSecs,
          segments = p.segments,
          loopProgram = p.loopProgram
        };
      }

      private void AddRuntimeProgram(
        Game.Audio.Radio.Radio.Program p,
        int startSecs,
        int endSecs,
        List<Game.Audio.Radio.Radio.RuntimeProgram> schedule,
        string path)
      {
        if (schedule.Count == 0)
        {
          schedule.Add(this.CreateRuntimeProgram(p, startSecs, endSecs, path));
        }
        else
        {
          for (int index = 0; index < schedule.Count; ++index)
          {
            Game.Audio.Radio.Radio.RuntimeProgram p1 = schedule[index];
            if (startSecs > p1.startTime && endSecs < p1.endTime)
            {
              Game.Audio.Radio.Radio.RuntimeProgram runtimeProgram = this.CreateRuntimeProgram(p, startSecs, endSecs, path);
              int num1;
              schedule.Insert(num1 = index + 1, runtimeProgram);
              int num2;
              schedule.Insert(num2 = num1 + 1, this.ShallowCopyRuntimeProgram(p1, runtimeProgram.endTime, p1.endTime));
              p1.endTime = runtimeProgram.startTime;
              return;
            }
            if (startSecs < p1.startTime && endSecs > p1.startTime)
            {
              Game.Audio.Radio.Radio.RuntimeProgram runtimeProgram = this.CreateRuntimeProgram(p, startSecs, endSecs, path);
              Game.Audio.Radio.Radio.log.WarnFormat("Program '{0}' overlaps with '{1}' in radio channel '{2}'", (object) runtimeProgram.name, (object) p1.name, (object) path);
              return;
            }
            if (startSecs < p1.startTime && endSecs < p1.startTime)
            {
              Game.Audio.Radio.Radio.RuntimeProgram runtimeProgram = this.CreateRuntimeProgram(p, startSecs, endSecs, path);
              schedule.Insert(index, runtimeProgram);
              return;
            }
          }
          schedule.Add(this.CreateRuntimeProgram(p, startSecs, endSecs, path));
        }
      }

      private void BuildRuntimePrograms(Game.Audio.Radio.Radio.Program[] programs, string path)
      {
        if (programs != null)
        {
          List<Game.Audio.Radio.Radio.RuntimeProgram> schedule = new List<Game.Audio.Radio.Radio.RuntimeProgram>();
          foreach (Game.Audio.Radio.Radio.Program program in programs)
          {
            int timeToSeconds1 = FormatUtils.ParseTimeToSeconds(program.startTime);
            int timeToSeconds2 = FormatUtils.ParseTimeToSeconds(program.endTime);
            if (this.IsValidTimestamp(timeToSeconds1, timeToSeconds2))
            {
              if (timeToSeconds1 == timeToSeconds2)
              {
                int endSecs = timeToSeconds2 + 86400;
                if (endSecs > 86400)
                {
                  this.AddRuntimeProgram(program, 0, endSecs - 86400, schedule, path);
                  this.AddRuntimeProgram(program, timeToSeconds1, 86400, schedule, path);
                }
                else
                  this.AddRuntimeProgram(program, timeToSeconds1, endSecs, schedule, path);
              }
              else
                this.AddRuntimeProgram(program, timeToSeconds1, timeToSeconds2, schedule, path);
            }
            else
              Game.Audio.Radio.Radio.log.WarnFormat("Program '{0}' has invalid timestamps ({3} ({1})->{4} ({2})) in radio channel '{5}' and was ignored!", (object) program.name, (object) timeToSeconds1, (object) timeToSeconds2, (object) FormatUtils.FormatTimeDebug(timeToSeconds1), (object) FormatUtils.FormatTimeDebug(timeToSeconds2), (object) path);
          }
          this.schedule = schedule.ToArray();
        }
        else
          Game.Audio.Radio.Radio.log.WarnFormat("No program founds in radio channel '{0}'", (object) path);
      }

      public int CompareTo(Game.Audio.Radio.Radio.RuntimeRadioChannel other)
      {
        return this.uiPriority.CompareTo(other.uiPriority);
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("name");
        writer.Write(this.name);
        writer.PropertyName("description");
        writer.Write(this.description);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.PropertyName("network");
        writer.Write(this.network);
        writer.PropertyName("currentProgram");
        writer.Write<Game.Audio.Radio.Radio.RuntimeProgram>(this.currentProgram ?? this.kNoProgram);
        writer.PropertyName("schedule");
        writer.ArrayBegin(this.schedule.Length);
        for (int index = 0; index < this.schedule.Length; ++index)
          writer.Write<Game.Audio.Radio.Radio.RuntimeProgram>(this.schedule[index]);
        writer.ArrayEnd();
        writer.TypeEnd();
      }
    }

    public class RadioNetwork : IComparable<Game.Audio.Radio.Radio.RadioNetwork>, IJsonWritable, IContentPrerequisite
    {
      public string name;
      [CanBeNull]
      public string nameId;
      public string description;
      public string descriptionId;
      public string icon;
      public bool allowAds;
      public int uiPriority;

      public string[] contentPrerequisite { get; set; }

      public int CompareTo(Game.Audio.Radio.Radio.RadioNetwork other)
      {
        return this.uiPriority.CompareTo(other.uiPriority);
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("name");
        writer.Write(this.name);
        writer.PropertyName("nameId");
        writer.Write(this.nameId);
        writer.PropertyName("description");
        writer.Write(this.description);
        writer.PropertyName("descriptionId");
        writer.Write(this.descriptionId);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.TypeEnd();
      }
    }

    public class RadioPlayer : IDisposable
    {
      private AudioMixerGroup m_RadioGroup;
      private AudioSource m_AudioSource;
      private Stopwatch m_Timer = new Stopwatch();
      private double m_Elapsed;
      private Game.Audio.Radio.Radio.Spectrum m_Spectrum;

      public bool isCreated => (UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null;

      public bool isPlaying => this.m_AudioSource.isPlaying;

      public int playbackPosition => this.m_AudioSource.timeSamples;

      public bool muted
      {
        get
        {
          return (UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null && (double) this.m_AudioSource.volume == 0.0;
        }
        set
        {
          if (!((UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null))
            return;
          this.m_AudioSource.volume = value ? 0.0f : 1f;
        }
      }

      public void Pause()
      {
        if ((UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null)
          this.m_AudioSource.Pause();
        this.m_Timer.Stop();
      }

      public void Unpause()
      {
        if ((UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null)
          this.m_AudioSource.UnPause();
        this.m_Timer.Start();
      }

      public RadioPlayer(AudioMixerGroup radioGroup) => this.m_RadioGroup = radioGroup;

      public void SetSpectrumSettings(
        bool enabled,
        int numSamples,
        FFTWindow fftWindow,
        Game.Audio.Radio.Radio.Spectrum.BandType bandType,
        float spacing,
        float padding)
      {
        if (this.m_Spectrum == null)
          return;
        if (enabled)
          this.m_Spectrum.Enable(numSamples, fftWindow, bandType, spacing, padding);
        else
          this.m_Spectrum.Disable();
      }

      public Texture equalizerTexture => this.m_Spectrum?.equalizerTexture;

      public void UpdateSpectrum()
      {
        if (!((UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null))
          return;
        this.m_Spectrum.Update(this.m_AudioSource);
      }

      public string currentClipName
      {
        get
        {
          return !this.isCreated || !((UnityEngine.Object) this.m_AudioSource.clip != (UnityEngine.Object) null) ? "None" : this.m_AudioSource.clip.name;
        }
      }

      public AudioClip currentClip => this.m_AudioSource.clip;

      private AudioSource CreateAudioSource(GameObject listener)
      {
        AudioSource audioSource = listener.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = this.m_RadioGroup;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.0f;
        return audioSource;
      }

      public void Create(GameObject listener)
      {
        this.m_AudioSource = this.CreateAudioSource(listener);
        this.m_Spectrum = new Game.Audio.Radio.Radio.Spectrum();
      }

      public void Dispose()
      {
        if (this.m_Spectrum != null)
          this.m_Spectrum.Disable();
        if (!((UnityEngine.Object) this.m_AudioSource != (UnityEngine.Object) null))
          return;
        this.m_AudioSource.Stop();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_AudioSource);
        this.m_AudioSource = (AudioSource) null;
      }

      public static double GetDuration(AudioClip clip)
      {
        return (double) clip.samples / (double) clip.frequency;
      }

      public double GetAudioSourceDuration()
      {
        return this.isCreated && (UnityEngine.Object) this.m_AudioSource.clip != (UnityEngine.Object) null ? Game.Audio.Radio.Radio.RadioPlayer.GetDuration(this.m_AudioSource.clip) : 0.0;
      }

      public double GetAudioSourceTimeElapsed()
      {
        return this.isCreated && (UnityEngine.Object) this.m_AudioSource.clip != (UnityEngine.Object) null ? (double) this.m_AudioSource.timeSamples / (double) this.m_AudioSource.clip.frequency : 0.0;
      }

      public double GetAudioSourceTimeRemaining()
      {
        return this.isCreated ? this.GetAudioSourceDuration() - (this.m_Elapsed + (double) this.m_Timer.ElapsedMilliseconds / 1000.0) : 0.0;
      }

      public void Rewind()
      {
        if (!((UnityEngine.Object) this.m_AudioSource.clip != (UnityEngine.Object) null))
          return;
        this.m_AudioSource.timeSamples = 0;
        if (this.m_AudioSource.isPlaying)
          this.m_AudioSource.Play();
        this.m_Elapsed = this.GetAudioSourceTimeElapsed();
        this.m_Timer.Restart();
      }

      public void Play(AudioClip clip, int timeSamples = 0)
      {
        if ((UnityEngine.Object) this.m_AudioSource == (UnityEngine.Object) null)
          return;
        this.m_AudioSource.clip = clip;
        this.m_AudioSource.timeSamples = timeSamples;
        this.m_AudioSource.Play();
        this.m_Elapsed = this.GetAudioSourceTimeElapsed();
        this.m_Timer.Restart();
      }
    }

    public class Program
    {
      public string name;
      public string description;
      public string icon;
      public string startTime;
      public string endTime;
      public bool loopProgram;
      public bool pairIntroOutro;
      public Game.Audio.Radio.Radio.Segment[] segments;
    }

    public class RuntimeProgram : IJsonWritable
    {
      public string name;
      public string description;
      public int startTime;
      public int endTime;
      public bool loopProgram;
      public bool active;
      public bool hasEnded;
      private int m_CurrentSegmentId;
      private List<Game.Audio.Radio.Radio.RuntimeSegment> m_Segments = new List<Game.Audio.Radio.Radio.RuntimeSegment>();

      public int duration => this.endTime - this.startTime;

      public Game.Audio.Radio.Radio.RuntimeSegment currentSegment
      {
        get
        {
          return this.m_CurrentSegmentId < this.m_Segments.Count ? this.m_Segments[this.m_CurrentSegmentId] : (Game.Audio.Radio.Radio.RuntimeSegment) null;
        }
      }

      public bool GoToNextSegment()
      {
        ++this.m_CurrentSegmentId;
        if (this.m_CurrentSegmentId < this.m_Segments.Count)
          return true;
        Game.Audio.Radio.Radio.log.DebugFormat("Program {0} has ended (last segment)", (object) this.name);
        this.hasEnded = true;
        return false;
      }

      public void Reset()
      {
        this.m_CurrentSegmentId = 0;
        this.hasEnded = false;
      }

      public IReadOnlyList<Game.Audio.Radio.Radio.RuntimeSegment> segments
      {
        get => (IReadOnlyList<Game.Audio.Radio.Radio.RuntimeSegment>) this.m_Segments;
        set => this.m_Segments = (List<Game.Audio.Radio.Radio.RuntimeSegment>) value;
      }

      private AudioAsset[] GetClips(int count, Func<int, int> rand, List<AudioAsset> clips)
      {
        AudioAsset[] clips1 = new AudioAsset[count];
        for (int index = 0; index < clips1.Length; ++index)
          clips1[index] = clips[rand(index)];
        return clips1;
      }

      public void BuildRuntimeSegments(Game.Audio.Radio.Radio.Program program, string path)
      {
        if (program.segments == null)
          return;
        foreach (Game.Audio.Radio.Radio.Segment segment1 in program.segments)
        {
          Game.Audio.Radio.Radio.Segment segment = segment1;
          if (segment.type == Game.Audio.Radio.Radio.SegmentType.Commercial || segment.type == Game.Audio.Radio.Radio.SegmentType.PSA)
          {
            this.m_Segments.Add(new Game.Audio.Radio.Radio.RuntimeSegment()
            {
              type = segment.type,
              tags = segment.tags,
              clipsCap = segment.clipsCap
            });
          }
          else
          {
            List<AudioAsset> clips = new List<AudioAsset>();
            if (segment.clips != null)
            {
              clips.Capacity += segment.clips.Length;
              clips.AddRange((IEnumerable<AudioAsset>) segment.clips);
            }
            if (segment.tags != null)
            {
              IEnumerable<AudioAsset> assets = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => ((IEnumerable<string>) segment.tags).All<string>(new Func<string, bool>(((AssetData) asset).ContainsTag)))));
              clips.AddRange(assets);
            }
            if (clips.Count > 0)
            {
              AudioAsset[] audioAssetArray;
              switch (segment.type)
              {
                case Game.Audio.Radio.Radio.SegmentType.Playlist:
                  System.Random rnd = new System.Random();
                  List<int> randomNumbers = Enumerable.Range(0, clips.Count).OrderBy<int, int>((Func<int, int>) (x => rnd.Next())).Take<int>(clips.Count).ToList<int>();
                  audioAssetArray = this.GetClips(clips.Count, (Func<int, int>) (i => randomNumbers[i]), clips);
                  break;
                case Game.Audio.Radio.Radio.SegmentType.Talkshow:
                case Game.Audio.Radio.Radio.SegmentType.News:
                  audioAssetArray = this.GetClips(clips.Count, (Func<int, int>) (i => i), clips);
                  break;
                default:
                  audioAssetArray = System.Array.Empty<AudioAsset>();
                  break;
              }
              this.m_Segments.Add(new Game.Audio.Radio.Radio.RuntimeSegment()
              {
                type = segment.type,
                tags = segment.tags,
                clipsCap = segment.clipsCap,
                clips = (IReadOnlyList<AudioAsset>) audioAssetArray
              });
            }
            else
              Game.Audio.Radio.Radio.log.WarnFormat("No clips found in a segment '{2}' of program '{0}' founds in radio channel '{1}'. Tags: {3}", (object) program.name, (object) path, (object) segment.type, (object) string.Join(", ", segment.tags));
          }
        }
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("name");
        writer.Write(this.name);
        writer.PropertyName("description");
        writer.Write(this.description);
        writer.PropertyName("startTime");
        writer.Write(this.startTime / 60);
        writer.PropertyName("endTime");
        writer.Write(this.endTime / 60);
        writer.PropertyName("duration");
        writer.Write(this.duration / 60);
        writer.PropertyName("active");
        writer.Write(this.active);
        writer.TypeEnd();
      }
    }

    public enum SegmentType
    {
      Playlist,
      Talkshow,
      PSA,
      Weather,
      News,
      Commercial,
      Emergency,
    }

    public class Segment
    {
      public Game.Audio.Radio.Radio.SegmentType type;
      public AudioAsset[] clips;
      public string[] tags;
      public int clipsCap;
    }

    public class RuntimeSegment
    {
      public Game.Audio.Radio.Radio.SegmentType type;
      public string[] tags;
      public int clipsCap;
      private int m_CapCount;
      private int m_CurrentClipId;
      private IReadOnlyList<AudioAsset> m_Clips;

      public AudioAsset currentClip
      {
        get
        {
          return this.m_CurrentClipId < this.m_Clips.Count ? this.m_Clips[this.m_CurrentClipId] : (AudioAsset) null;
        }
      }

      public bool isSetUp { get; private set; }

      public bool GoToNextClip()
      {
        ++this.m_CapCount;
        ++this.m_CurrentClipId;
        if (this.m_CurrentClipId >= this.m_Clips.Count)
        {
          Game.Audio.Radio.Radio.log.Debug((object) "Segment has ended (last clip)");
          this.Reset();
          return false;
        }
        if (this.m_CapCount < this.clipsCap)
          return true;
        Game.Audio.Radio.Radio.log.DebugFormat("Segment has ended (cap count reached {0}/{1})", (object) this.m_CapCount, (object) this.clipsCap);
        this.m_CapCount = 0;
        return false;
      }

      public bool GoToPreviousClip()
      {
        --this.m_CurrentClipId;
        if (this.m_CurrentClipId >= 0)
          return true;
        this.m_CurrentClipId = 0;
        return false;
      }

      public void Reset()
      {
        this.m_CurrentClipId = 0;
        this.m_CapCount = 0;
        this.isSetUp = false;
      }

      public IReadOnlyList<AudioAsset> clips
      {
        get => this.m_Clips;
        set
        {
          if (this.m_Clips == value)
            return;
          this.m_Clips = value;
          this.durationMs = 0.0;
          foreach (AudioAsset clip in (IEnumerable<AudioAsset>) this.m_Clips)
            this.durationMs += clip.durationMs;
        }
      }

      public void Setup(Game.Audio.Radio.Radio.OnDemandClips clipsCallback = null)
      {
        if (this.isSetUp)
          return;
        this.isSetUp = true;
        if (clipsCallback == null)
          return;
        clipsCallback(this);
      }

      public double durationMs { get; private set; }
    }

    public class Spectrum
    {
      private static readonly float[][] kMiddleFrequenciesForBands = new float[6][]
      {
        new float[4]{ 125f, 500f, 1000f, 2000f },
        new float[4]{ 250f, 400f, 600f, 800f },
        new float[8]
        {
          63f,
          125f,
          500f,
          1000f,
          2000f,
          4000f,
          6000f,
          8000f
        },
        new float[10]
        {
          31.5f,
          63f,
          125f,
          250f,
          500f,
          1000f,
          2000f,
          4000f,
          8000f,
          16000f
        },
        new float[26]
        {
          25f,
          31.5f,
          40f,
          50f,
          63f,
          80f,
          100f,
          125f,
          160f,
          200f,
          250f,
          315f,
          400f,
          500f,
          630f,
          800f,
          1000f,
          1250f,
          1600f,
          2000f,
          2500f,
          3150f,
          4000f,
          5000f,
          6300f,
          8000f
        },
        new float[31]
        {
          20f,
          25f,
          31.5f,
          40f,
          50f,
          63f,
          80f,
          100f,
          125f,
          160f,
          200f,
          250f,
          315f,
          400f,
          500f,
          630f,
          800f,
          1000f,
          1250f,
          1600f,
          2000f,
          2500f,
          3150f,
          4000f,
          5000f,
          6300f,
          8000f,
          10000f,
          12500f,
          16000f,
          20000f
        }
      };
      private static readonly float[] kBandwidthForBands = new float[6]
      {
        1.414f,
        1.26f,
        1.414f,
        1.414f,
        1.122f,
        1.122f
      };
      private static readonly string[] kKeywords = new string[6]
      {
        "EQ_FOUR_BAND",
        "EQ_FOUR_BAND_VISUAL",
        "EQ_HEIGHT_BAND",
        "EQ_TEN_BAND",
        "EQ_TWENTYSIX_BAND",
        "EQ_THIRTYONE_BAND"
      };
      private float[] m_SpectrumData;
      private NativeArray<float> m_Frequencies;
      private float m_Bandwidth;
      private Vector4[] m_Levels;
      private FFTWindow m_FFTWindow;
      private Game.Audio.Radio.Radio.Spectrum.BandType m_BandType;
      private RenderTexture m_VURender;
      private Material m_Equalizer;
      private RenderTargetIdentifier m_VURenderId;
      private const int kTexWidth = 96;
      private const int kTexHeight = 36;

      public Texture equalizerTexture => (Texture) this.m_VURender;

      public void Enable(
        int samplesCount,
        FFTWindow fftWindow,
        Game.Audio.Radio.Radio.Spectrum.BandType bandType,
        float spacing = 10f,
        float padding = 2f)
      {
        if (this.m_Frequencies.IsCreated)
          this.Disable();
        this.m_FFTWindow = fftWindow;
        this.m_BandType = bandType;
        this.m_SpectrumData = new float[samplesCount];
        int length = Game.Audio.Radio.Radio.Spectrum.kMiddleFrequenciesForBands[(int) this.m_BandType].Length;
        this.m_Frequencies = new NativeArray<float>(Game.Audio.Radio.Radio.Spectrum.kMiddleFrequenciesForBands[(int) this.m_BandType], Allocator.Persistent);
        this.m_Bandwidth = Game.Audio.Radio.Radio.Spectrum.kBandwidthForBands[(int) this.m_BandType];
        this.m_Levels = new Vector4[length];
        RenderTexture renderTexture = new RenderTexture(96, 36, 0, GraphicsFormat.R8G8B8A8_UNorm, 0);
        renderTexture.name = "RadioEqualizer";
        renderTexture.hideFlags = HideFlags.HideAndDontSave;
        this.m_VURender = renderTexture;
        this.m_VURender.Create();
        this.m_VURenderId = new RenderTargetIdentifier((Texture) this.m_VURender);
        this.m_Equalizer = new Material(Shader.Find("Hidden/HDRP/Radio/Equalizer"));
        this.m_Equalizer.SetFloat("_Padding", padding / 96f);
        this.m_Equalizer.SetFloat("_Spacing", spacing / 96f);
        this.m_Equalizer.EnableKeyword(Game.Audio.Radio.Radio.Spectrum.kKeywords[(int) this.m_BandType]);
        RenderPipelineManager.beginFrameRendering += new Action<ScriptableRenderContext, Camera[]>(this.SpectrumBlit);
      }

      public void Disable()
      {
        RenderPipelineManager.beginFrameRendering -= new Action<ScriptableRenderContext, Camera[]>(this.SpectrumBlit);
        if ((UnityEngine.Object) this.m_Equalizer != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Equalizer);
        if ((UnityEngine.Object) this.m_VURender != (UnityEngine.Object) null)
        {
          this.m_VURender.Release();
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_VURender);
        }
        this.m_SpectrumData = (float[]) null;
        if (this.m_Frequencies.IsCreated)
          this.m_Frequencies.Dispose();
        this.m_Levels = (Vector4[]) null;
      }

      private void SpectrumBlit(ScriptableRenderContext context, Camera[] camera)
      {
        CommandBuffer commandBuffer = CommandBufferPool.Get("BlitRadio");
        this.m_Equalizer.SetVectorArray("_Levels", this.m_Levels);
        commandBuffer.Blit((Texture) null, this.m_VURenderId, this.m_Equalizer);
        context.ExecuteCommandBuffer(commandBuffer);
        CommandBufferPool.Release(commandBuffer);
        this.m_VURender.IncrementUpdateCount();
      }

      public void Update(AudioSource source)
      {
        if (!((UnityEngine.Object) source != (UnityEngine.Object) null) || !this.m_Frequencies.IsCreated)
          return;
        source.GetSpectrumData(this.m_SpectrumData, 0, this.m_FFTWindow);
        new Game.Audio.Radio.Radio.Spectrum.CreateLevels(ref this.m_Frequencies, this.m_Bandwidth, this.m_SpectrumData, this.m_Levels, 0.25f, 4f).Schedule<Game.Audio.Radio.Radio.Spectrum.CreateLevels>().Complete();
      }

      public enum BandType
      {
        FourBand,
        FourBandVisual,
        EightBand,
        TenBand,
        TwentySixBand,
        ThirtyOneBand,
      }

      [BurstCompile]
      private struct CreateLevels : IJob
      {
        private NativeArray<float> m_FrequenciesForBands;
        private float m_BandWidth;
        private float m_Falldown;
        private float m_Filter;
        private int m_SpectrumLength;
        private float m_OutputSampleRate;
        [NativeDisableUnsafePtrRestriction]
        private unsafe void* m_SpectrumData;
        private int m_LevelsLength;
        [NativeDisableUnsafePtrRestriction]
        private unsafe void* m_Levels;

        public unsafe CreateLevels(
          ref NativeArray<float> frequenciesForBands,
          float bandWidth,
          float[] spectrumData,
          Vector4[] levels,
          float fallSpeed,
          float sensitivity)
        {
          this.m_FrequenciesForBands = frequenciesForBands;
          this.m_BandWidth = bandWidth;
          this.m_SpectrumLength = spectrumData.Length;
          this.m_SpectrumData = UnsafeUtility.AddressOf<float>(ref spectrumData[0]);
          this.m_LevelsLength = levels.Length;
          this.m_Levels = UnsafeUtility.AddressOf<Vector4>(ref levels[0]);
          this.m_Falldown = fallSpeed * Time.deltaTime;
          this.m_Filter = Mathf.Exp(-sensitivity * Time.deltaTime);
          this.m_OutputSampleRate = (float) UnityEngine.AudioSettings.outputSampleRate;
        }

        private int FrequencyToSpectrumIndex(float f)
        {
          return math.clamp((int) math.floor((float) ((double) f / (double) this.m_OutputSampleRate * 2.0) * (float) this.m_SpectrumLength), 0, this.m_SpectrumLength - 1);
        }

        public unsafe void Execute()
        {
          for (int index1 = 0; index1 < this.m_LevelsLength; ++index1)
          {
            int spectrumIndex1 = this.FrequencyToSpectrumIndex(this.m_FrequenciesForBands[index1] / this.m_BandWidth);
            int spectrumIndex2 = this.FrequencyToSpectrumIndex(this.m_FrequenciesForBands[index1] * this.m_BandWidth);
            float num = 0.0f;
            for (int index2 = spectrumIndex1; index2 <= spectrumIndex2; ++index2)
              num = math.max(num, UnsafeUtility.ReadArrayElement<float>(this.m_SpectrumData, index2));
            Vector4 vector4;
            vector4.x = num;
            vector4.y = num - (num - UnsafeUtility.ReadArrayElement<Vector4>(this.m_Levels, index1).y) * this.m_Filter;
            vector4.z = math.max(UnsafeUtility.ReadArrayElement<Vector4>(this.m_Levels, index1).z - this.m_Falldown, num);
            vector4.w = 0.0f;
            UnsafeUtility.WriteArrayElement<Vector4>(this.m_Levels, index1, vector4);
          }
        }
      }
    }
  }
}
