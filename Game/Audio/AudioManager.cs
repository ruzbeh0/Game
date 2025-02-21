// Decompiled with JetBrains decompiler
// Type: Game.Audio.AudioManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Effects;
using Game.Objects;
using Game.Prefabs;
using Game.Prefabs.Effects;
using Game.SceneFlow;
using Game.Serialization;
using Game.Settings;
using Game.Simulation;
using Game.Tools;
using Game.UI.InGame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Audio
{
  [CompilerGenerated]
  public class AudioManager : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPreDeserialize,
    IPreSerialize
  {
    private const float kDopplerLevelReduceFactor = 0.3f;
    private static readonly ILog log = LogManager.GetLogger("Audio");
    private List<AudioManager.AudioInfo> m_AudioInfos = new List<AudioManager.AudioInfo>();
    private const string kMasterVolumeProperty = "MasterVolume";
    private const string kRadioVolumeProperty = "RadioVolume";
    private const string kUIVolumeProperty = "UIVolume";
    private const string kMenuVolumeProperty = "MenuVolume";
    private const string kInGameVolumeProperty = "InGameVolume";
    private const string kAmbienceVolumeProperty = "AmbienceVolume";
    private const string kDisastersVolumeProperty = "DisastersVolume";
    private const string kWorldVolumeProperty = "WorldVolume";
    private const string kAudioGroupsVolumeProperty = "AudioGroupsVolume";
    private const string kServiceBuildingsVolumeProperty = "ServiceBuildingsVolume";
    private SynchronizationContext m_MainThreadContext;
    private AudioMixer m_Mixer;
    private AudioMixerGroup m_AmbientGroup;
    private AudioMixerGroup m_InGameGroup;
    private AudioMixerGroup m_RadioGroup;
    private AudioMixerGroup m_UIGroup;
    private AudioMixerGroup m_MenuGroup;
    private AudioMixerGroup m_WorldGroup;
    private AudioMixerGroup m_ServiceBuildingGroup;
    private AudioMixerGroup m_AudioGroupGroup;
    private AudioMixerGroup m_DisasterGroup;
    private AudioLoop m_MainMenuMusic;
    private AudioSource m_UIAudioSource;
    private AudioListener m_AudioListener;
    private NativeQueue<SourceUpdateInfo> m_SourceUpdateQueue;
    private SourceUpdateData m_SourceUpdateData;
    private JobHandle m_SourceUpdateWriter;
    private SimulationSystem m_SimulationSystem;
    private PrefabSystem m_PrefabSystem;
    private GameScreenUISystem m_GameScreenUISystem;
    private EffectControlSystem m_EffectControlSystem;
    private RandomSeed m_RandomSeed;
    private float m_FadeOutMenu;
    private float m_DeltaTime;
    private bool m_IsGamePausedLastUpdate;
    private bool m_IsMenuActivatedLastUpdate;
    private bool m_ShouldUnpauseRadioAfterGameUnpaused;
    private string m_LastSaveRadioChannel;
    private bool m_LastSaveRadioSkipAds;
    private AudioManager.FadeStatus m_AudioFadeStatus;
    private TimeSystem m_TimeSystem;
    private List<SFX> m_Clips = new List<SFX>();
    private NativeParallelHashMap<SourceInfo, int> m_CurrentEffects;
    private EntityQuery m_AmbientSettingsQuery;
    private EntityQuery m_SoundQuery;
    private EntityQuery m_WeatherAudioEntitiyQuery;
    private List<AudioManager.CameraAmbientAudioInfo> m_CameraAmbientSources = new List<AudioManager.CameraAmbientAudioInfo>();
    private List<AudioSource> m_TempAudioSources = new List<AudioSource>();
    private Game.Audio.Radio.Radio m_Radio;
    private int m_PlayCount;
    private AudioManager.TypeHandle __TypeHandle;

    public static AudioManager instance { get; private set; }

    public Game.Audio.Radio.Radio radio => this.m_Radio;

    public Entity followed { private get; set; }

    public int RegisterSFX(SFX sfx)
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.m_Clips.IndexOf(sfx);
      if (num != -1)
        return num;
      // ISSUE: reference to a compiler-generated field
      int count = this.m_Clips.Count;
      // ISSUE: reference to a compiler-generated field
      this.m_Clips.Add(sfx);
      return count;
    }

    private void SetVolume(string volumeProperty, float value)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (GameManager.instance.gameMode == GameMode.Game && this.m_GameScreenUISystem.isMenuActive && this.m_AudioFadeStatus == AudioManager.FadeStatus.None)
      {
        switch (volumeProperty)
        {
          case "DisastersVolume":
            return;
          case "WorldVolume":
            return;
          case "AudioGroupsVolume":
            return;
          case "AmbienceVolume":
            return;
          case "ServiceBuildingsVolume":
            return;
          case "RadioVolume":
            return;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Mixer.SetFloat(volumeProperty, Mathf.Log10(Mathf.Min(Mathf.Max(value, 0.0001f), 1f)) * 20f);
    }

    private float GetVolume(string volumeProperty)
    {
      float num;
      // ISSUE: reference to a compiler-generated field
      return this.m_Mixer.GetFloat(volumeProperty, out num) ? Mathf.Pow(10f, num / 20f) : 1f;
    }

    public float masterVolume
    {
      get => this.GetVolume("MasterVolume");
      set => this.SetVolume("MasterVolume", value);
    }

    public float radioVolume
    {
      get => this.GetVolume("RadioVolume");
      set => this.SetVolume("RadioVolume", value);
    }

    public float uiVolume
    {
      get => this.GetVolume("UIVolume");
      set => this.SetVolume("UIVolume", value);
    }

    public float menuVolume
    {
      get => this.GetVolume("MenuVolume");
      set => this.SetVolume("MenuVolume", value);
    }

    public float ingameVolume
    {
      get => this.GetVolume("InGameVolume");
      set => this.SetVolume("InGameVolume", value);
    }

    public float ambienceVolume
    {
      get => this.GetVolume("AmbienceVolume");
      set => this.SetVolume("AmbienceVolume", value);
    }

    public float disastersVolume
    {
      get => this.GetVolume("DisastersVolume");
      set => this.SetVolume("DisastersVolume", value);
    }

    public float worldVolume
    {
      get => this.GetVolume("WorldVolume");
      set => this.SetVolume("WorldVolume", value);
    }

    public float audioGroupsVolume
    {
      get => this.GetVolume("AudioGroupsVolume");
      set => this.SetVolume("AudioGroupsVolume", value);
    }

    public float serviceBuildingsVolume
    {
      get => this.GetVolume("ServiceBuildingsVolume");
      set => this.SetVolume("ServiceBuildingsVolume", value);
    }

    public void MoveAudioListenerForDoppler(float3 m_FollowOffset)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AudioListener.transform.position += new Vector3(m_FollowOffset.x, m_FollowOffset.y, m_FollowOffset.z);
    }

    public void UpdateAudioListener(Vector3 position, Quaternion rotation)
    {
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_AudioListener != (UnityEngine.Object) null) || GameManager.instance.gameMode != GameMode.Game || GameManager.instance.isGameLoading)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_AudioListener.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AudioListener.transform.position = position;
      // ISSUE: reference to a compiler-generated field
      this.m_AudioListener.transform.rotation = rotation;
      // ISSUE: reference to a compiler-generated field
      this.m_AudioListener.enabled = true;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CameraAmbientSources.Count <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateGlobalAudioSources(this.m_AudioListener.transform);
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      // ISSUE: reference to a compiler-generated field
      this.m_Radio.Disable();
      // ISSUE: reference to a compiler-generated method
      this.Reset();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      switch (serializationContext.purpose)
      {
        case Colossal.Serialization.Entities.Purpose.NewGame:
        case Colossal.Serialization.Entities.Purpose.LoadGame:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Radio.RestoreRadioSettings(this.m_LastSaveRadioChannel, this.m_LastSaveRadioSkipAds);
          // ISSUE: reference to a compiler-generated field
          this.m_Radio.Reload();
          break;
      }
    }

    protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGameLoadingComplete(purpose, mode);
      if (!mode.IsGameOrEditor())
        return;
      // ISSUE: reference to a compiler-generated method
      this.StopMenuMusic();
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      AudioManager.instance = this;
      // ISSUE: reference to a compiler-generated method
      AudioManager.AudioSourcePool.Reset();
      // ISSUE: reference to a compiler-generated field
      this.m_MainThreadContext = SynchronizationContext.Current;
      // ISSUE: reference to a compiler-generated field
      this.m_AmbientSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<AmbientAudioSettingsData>(), ComponentType.ReadOnly<AmbientAudioEffect>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_WeatherAudioEntitiyQuery = this.GetEntityQuery(ComponentType.ReadOnly<WeatherAudioData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AudioListener = new GameObject("AudioListener").AddComponent<AudioListener>();
      // ISSUE: reference to a compiler-generated field
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.m_AudioListener);
      // ISSUE: reference to a compiler-generated field
      this.m_Mixer = Resources.Load<AudioMixer>("Audio/MasterMixer");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AmbientGroup = this.m_Mixer.FindMatchingGroups("Ambience")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_RadioGroup = this.m_Mixer.FindMatchingGroups("Radio")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_InGameGroup = this.m_Mixer.FindMatchingGroups("InGame")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UIGroup = this.m_Mixer.FindMatchingGroups("UI")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MenuGroup = this.m_Mixer.FindMatchingGroups("Menu")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WorldGroup = this.m_Mixer.FindMatchingGroups("World")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceBuildingGroup = this.m_Mixer.FindMatchingGroups("ServiceBuildings")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AudioGroupGroup = this.m_Mixer.FindMatchingGroups("AudioGroups")[0];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_DisasterGroup = this.m_Mixer.FindMatchingGroups("Disasters")[0];
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GameScreenUISystem = this.World.GetOrCreateSystemManaged<GameScreenUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Radio = new Game.Audio.Radio.Radio(this.m_RadioGroup);
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateQueue = new NativeQueue<SourceUpdateInfo>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateData = new SourceUpdateData(this.m_SourceUpdateQueue.AsParallelWriter());
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentEffects = new NativeParallelHashMap<SourceInfo, int>(128, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_RandomSeed = new RandomSeed();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MainMenuMusic?.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Radio?.Disable();
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentEffects.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (this.m_SourceUpdateQueue.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SourceUpdateWriter.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_SourceUpdateQueue.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_TempAudioSources.Clear();
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated method
      this.ClearCameraAmbientSources();
    }

    private void ClearCameraAmbientSources()
    {
      // ISSUE: reference to a compiler-generated field
      foreach (AudioManager.CameraAmbientAudioInfo cameraAmbientSource in this.m_CameraAmbientSources)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AudioManager.AudioSourcePool.Release(cameraAmbientSource.source);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CameraAmbientSources.Clear();
    }

    public void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearCameraAmbientSources();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AudioInfos.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AudioManager.AudioSourcePool.Release(this.m_AudioInfos[index].m_AudioSource);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentEffects.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioInfos.Clear();
    }

    public async Task ResetAudioOnMainThread()
    {
      TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();
      // ISSUE: reference to a compiler-generated field
      this.m_MainThreadContext.Send((SendOrPostCallback) (_ =>
      {
        // ISSUE: reference to a compiler-generated method
        this.Reset();
        taskCompletion.SetResult(true);
      }), (object) null);
      int num = await taskCompletion.Task ? 1 : 0;
    }

    public void SetGlobalAudioSettings()
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearCameraAmbientSources();
      // ISSUE: reference to a compiler-generated field
      if (this.m_AmbientSettingsQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      Entity singletonEntity = this.m_AmbientSettingsQuery.GetSingletonEntity();
      DynamicBuffer<AmbientAudioEffect> buffer = this.World.EntityManager.GetBuffer<AmbientAudioEffect>(singletonEntity, true);
      AmbientAudioSettingsData componentData = this.World.EntityManager.GetComponentData<AmbientAudioSettingsData>(singletonEntity);
      float num1 = (componentData.m_MaxHeight - componentData.m_MinHeight) / (float) (buffer.Length + 1);
      float num2 = (float) ((double) componentData.m_MinHeight + (double) num1 * (double) buffer.Length - (double) num1 * (1.0 - (double) componentData.m_OverlapRatio));
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        EffectPrefab prefab = this.m_PrefabSystem.GetPrefab<EffectPrefab>(buffer[index].m_Effect);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          SFX component = prefab.GetComponent<SFX>();
          // ISSUE: reference to a compiler-generated method
          AudioSource audioSource = AudioManager.AudioSourcePool.Get();
          // ISSUE: reference to a compiler-generated method
          this.SetAudioSourceData(audioSource, component, component.m_Volume);
          // ISSUE: reference to a compiler-generated method
          this.UpdateAudioSource(audioSource, component, new Game.Objects.Transform()
          {
            m_Position = float3.zero,
            m_Rotation = quaternion.identity
          }, 1f, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_CameraAmbientSources.Add(new AudioManager.CameraAmbientAudioInfo()
          {
            id = index,
            height = num2,
            source = audioSource,
            transform = audioSource.transform
          });
          audioSource.maxDistance = num1 * componentData.m_OverlapRatio;
          audioSource.minDistance = audioSource.maxDistance * componentData.m_MinDistanceRatio;
          // ISSUE: reference to a compiler-generated method
          AudioManager.AudioSourcePool.Play(audioSource);
          num2 -= num1;
        }
      }
    }

    private void SetGlobalAudioSourcePosition(
      AudioManager.CameraAmbientAudioInfo info,
      float3 position)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3 float3 = position with
      {
        y = info.id != 0 ? info.height : math.max(info.height, position.y)
      };
      // ISSUE: reference to a compiler-generated field
      info.transform.position = (Vector3) float3;
    }

    public void UpdateGlobalAudioSources(UnityEngine.Transform cameraTransform)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CameraAmbientSources.Count > 0 && (UnityEngine.Object) this.m_CameraAmbientSources[0].source == (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        this.SetGlobalAudioSettings();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_CameraAmbientSources.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetGlobalAudioSourcePosition(this.m_CameraAmbientSources[index], (float3) cameraTransform.position);
      }
    }

    public SourceUpdateData GetSourceUpdateData(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_SourceUpdateWriter;
      // ISSUE: reference to a compiler-generated field
      return this.m_SourceUpdateData;
    }

    public void AddSourceUpdateWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateWriter = JobHandle.CombineDependencies(this.m_SourceUpdateWriter, jobHandle);
    }

    public void StopMenuMusic() => this.m_MainMenuMusic?.FadeOut();

    public bool PlayUISoundIfNotPlaying(Entity clipEntity, float volume = 1f)
    {
      DynamicBuffer<AudioRandomizeData> buffer;
      if (this.EntityManager.HasComponent<AudioRandomizeData>(clipEntity) && this.EntityManager.TryGetBuffer<AudioRandomizeData>(clipEntity, true, out buffer))
      {
        int index = new Unity.Mathematics.Random((uint) DateTime.Now.Ticks).NextInt(buffer.Length);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.PlayUISoundIfNotPlaying(this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(buffer[index].m_SFXEntity).m_AudioClipId].m_AudioClip, volume);
      }
      if (!this.EntityManager.HasComponent<AudioEffectData>(clipEntity))
        return false;
      // ISSUE: reference to a compiler-generated field
      SFX clip = this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(clipEntity).m_AudioClipId];
      volume = clip.m_Volume * volume;
      // ISSUE: reference to a compiler-generated method
      return this.PlayUISoundIfNotPlaying(clip.m_AudioClip, volume);
    }

    public void PlayUISound(Entity clipEntity, float volume = 1f)
    {
      DynamicBuffer<AudioRandomizeData> buffer;
      if (this.EntityManager.HasComponent<AudioRandomizeData>(clipEntity) && this.EntityManager.TryGetBuffer<AudioRandomizeData>(clipEntity, true, out buffer))
      {
        int index = new Unity.Mathematics.Random((uint) DateTime.Now.Ticks).NextInt(buffer.Length);
        // ISSUE: reference to a compiler-generated field
        SFX clip = this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(buffer[index].m_SFXEntity).m_AudioClipId];
        // ISSUE: reference to a compiler-generated method
        this.PlayUISound(clip.m_AudioClip, clip.m_Volume * volume);
      }
      if (!this.EntityManager.HasComponent<AudioEffectData>(clipEntity))
        return;
      // ISSUE: reference to a compiler-generated field
      SFX clip1 = this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(clipEntity).m_AudioClipId];
      // ISSUE: reference to a compiler-generated method
      this.PlayUISound(clip1.m_AudioClip, clip1.m_Volume * volume);
    }

    public bool PlayUISoundIfNotPlaying(AudioClip clipEntity, float volume = 1f)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_UIAudioSource == (UnityEngine.Object) null) && this.m_UIAudioSource.isPlaying)
        return false;
      // ISSUE: reference to a compiler-generated method
      this.PlayUISound(clipEntity, volume);
      return true;
    }

    public void PlayUISound(AudioClip clip, float volume = 1f)
    {
      if ((UnityEngine.Object) clip != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_UIAudioSource == (UnityEngine.Object) null)
        {
          GameObject target = new GameObject("UIAudioSource");
          // ISSUE: reference to a compiler-generated field
          this.m_UIAudioSource = target.AddComponent<AudioSource>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_UIAudioSource.outputAudioMixerGroup = this.m_UIGroup;
          // ISSUE: reference to a compiler-generated field
          this.m_UIAudioSource.dopplerLevel = 0.0f;
          // ISSUE: reference to a compiler-generated field
          this.m_UIAudioSource.playOnAwake = false;
          // ISSUE: reference to a compiler-generated field
          this.m_UIAudioSource.spatialBlend = 0.0f;
          // ISSUE: reference to a compiler-generated field
          this.m_UIAudioSource.ignoreListenerPause = true;
          UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_UIAudioSource.PlayOneShot(clip, volume);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        AudioManager.log.WarnFormat("PlayUISound invoked with no audio clip");
      }
    }

    public AudioSource PlayExclusiveUISound(Entity clipEntity)
    {
      AudioSource audioSource = (AudioSource) null;
      if (this.EntityManager.HasComponent<AudioEffectData>(clipEntity))
      {
        // ISSUE: reference to a compiler-generated field
        SFX clip = this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(clipEntity).m_AudioClipId];
        if ((UnityEngine.Object) clip.m_AudioClip != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          audioSource = AudioManager.AudioSourcePool.Get();
          audioSource.loop = clip.m_Loop;
          audioSource.pitch = clip.m_Pitch;
          audioSource.volume = clip.m_Volume;
          // ISSUE: reference to a compiler-generated field
          audioSource.outputAudioMixerGroup = this.m_UIGroup;
          audioSource.dopplerLevel = 0.0f;
          audioSource.playOnAwake = false;
          audioSource.spatialBlend = 0.0f;
          audioSource.ignoreListenerPause = true;
          audioSource.clip = clip.m_AudioClip;
          audioSource.timeSamples = 0;
          // ISSUE: reference to a compiler-generated method
          AudioManager.AudioSourcePool.Play(audioSource);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          AudioManager.log.WarnFormat("PlayUISound invoked with no audio clip");
        }
      }
      return audioSource;
    }

    public void StopExclusiveUISound(AudioSource audioSource)
    {
      if (!((UnityEngine.Object) audioSource != (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated method
      AudioManager.AudioSourcePool.Release(audioSource);
    }

    public async Task PlayMenuMusic(string tag)
    {
      AudioAsset randomAsset = Colossal.IO.AssetDatabase.AssetDatabase.global.GetRandomAsset<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((Func<AudioAsset, bool>) (asset => asset.ContainsTag(tag))));
      if (!((AssetData) randomAsset != (IAssetData) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MainMenuMusic = new AudioLoop(randomAsset, this.m_Mixer, this.m_MenuGroup);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      await this.m_MainMenuMusic.Start(this.m_PlayCount > 0);
      // ISSUE: reference to a compiler-generated field
      this.m_Radio?.Disable();
      // ISSUE: reference to a compiler-generated field
      ++this.m_PlayCount;
    }

    private void UpdateMenuMusic()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MainMenuMusic?.Update((double) this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime);
    }

    private bool GetEffect(
      DynamicBuffer<EnabledEffect> effects,
      int effectIndex,
      out EnabledEffect effect)
    {
      for (int index = 0; index < effects.Length; ++index)
      {
        effect = effects[index];
        if (effect.m_EffectIndex == effectIndex)
          return true;
      }
      effect = new EnabledEffect();
      return false;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DeltaTime = UnityEngine.Time.deltaTime;
      // ISSUE: reference to a compiler-generated method
      this.UpdateMenuMusic();
      if (GameManager.instance.isGameLoading)
        return;
      if (GameManager.instance.gameMode != GameMode.Game)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SourceUpdateWriter.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_SourceUpdateQueue.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CameraAmbientSources.Count == 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetGlobalAudioSettings();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempAudioSources.Count > 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateTempAudioSources();
        }
        // ISSUE: reference to a compiler-generated method
        this.UpdateGameAudioSetting();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Radio.Update(this.m_TimeSystem.normalizedTime);
        Camera main = Camera.main;
        if ((UnityEngine.Object) main == (UnityEngine.Object) null)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Tools.EditorContainer> roComponentLookup1 = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<AudioSourceData> dataRoBufferLookup = this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<Effect> effectRoBufferLookup1 = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<EnabledEffect> effectRoBufferLookup2 = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<AudioEffectData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Effects_EffectInstance_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<EffectInstance> roComponentLookup4 = this.__TypeHandle.__Game_Effects_EffectInstance_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        this.m_SourceUpdateWriter.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom((int) this.m_SimulationSystem.frameIndex);
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<EnabledEffectData> enabledData = this.m_EffectControlSystem.GetEnabledData(true, out dependencies);
        dependencies.Complete();
        SourceUpdateInfo sourceUpdateInfo;
        // ISSUE: reference to a compiler-generated field
        while (this.m_SourceUpdateQueue.TryDequeue(out sourceUpdateInfo))
        {
          SourceInfo sourceInfo = sourceUpdateInfo.m_SourceInfo;
          // ISSUE: reference to a compiler-generated field
          bool flag1 = this.m_CurrentEffects.ContainsKey(sourceInfo);
          if (sourceUpdateInfo.m_Type == SourceUpdateType.Add)
          {
            if (!flag1)
            {
              if (roComponentLookup2.HasComponent(sourceInfo.m_Entity))
              {
                Entity entity = roComponentLookup2[sourceInfo.m_Entity].m_Prefab;
                float num = 0.0f;
                bool flag2 = false;
                if (sourceInfo.m_EffectIndex != -1)
                {
                  DynamicBuffer<EnabledEffect> effects = effectRoBufferLookup2[sourceInfo.m_Entity];
                  DynamicBuffer<Effect> bufferData;
                  if (effectRoBufferLookup1.TryGetBuffer(entity, out bufferData))
                  {
                    EnabledEffect effect1;
                    // ISSUE: reference to a compiler-generated method
                    if (this.GetEffect(effects, sourceInfo.m_EffectIndex, out effect1))
                    {
                      Effect effect2 = bufferData[sourceInfo.m_EffectIndex];
                      EnabledEffectData enabledEffectData = enabledData[effect1.m_EnabledIndex];
                      num = enabledEffectData.m_Intensity;
                      flag2 = (enabledEffectData.m_Flags & EnabledEffectFlags.AudioDisabled) > (EnabledEffectFlags) 0;
                      entity = effect2.m_Effect;
                    }
                  }
                  else
                  {
                    Game.Tools.EditorContainer componentData;
                    if (roComponentLookup1.TryGetComponent(sourceInfo.m_Entity, out componentData) && effects.Length != 0)
                    {
                      EnabledEffect enabledEffect = effects[0];
                      EnabledEffectData enabledEffectData = enabledData[enabledEffect.m_EnabledIndex];
                      num = enabledEffectData.m_Intensity;
                      flag2 = (enabledEffectData.m_Flags & EnabledEffectFlags.AudioDisabled) > (EnabledEffectFlags) 0;
                      entity = componentData.m_Prefab;
                    }
                  }
                }
                else
                  num = roComponentLookup4[sourceInfo.m_Entity].m_Intensity;
                if (dataRoBufferLookup.HasBuffer(entity))
                {
                  DynamicBuffer<AudioSourceData> dynamicBuffer = dataRoBufferLookup[entity];
                  int index = random.NextInt(dynamicBuffer.Length);
                  Entity sfxEntity = dynamicBuffer[index].m_SFXEntity;
                  int audioClipId = roComponentLookup3[sfxEntity].m_AudioClipId;
                  // ISSUE: reference to a compiler-generated field
                  SFX clip = this.m_Clips[audioClipId];
                  float targetVolume = clip.m_Volume * num;
                  if ((double) targetVolume > 1.0 / 1000.0 && !flag2)
                  {
                    // ISSUE: reference to a compiler-generated method
                    float fadedVolume = this.GetFadedVolume(AudioManager.FadeStatus.FadeIn, clip.m_FadeTimes, 0.0f, targetVolume);
                    // ISSUE: reference to a compiler-generated method
                    AudioSource audioSource = AudioManager.AudioSourcePool.Get();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.SetAudioSourceData(audioSource, this.m_Clips[audioClipId], fadedVolume);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_CurrentEffects.Add(sourceInfo, this.m_AudioInfos.Count);
                    // ISSUE: reference to a compiler-generated method
                    AudioManager.AudioSourcePool.Play(audioSource);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: object of a compiler-generated type is created
                    this.m_AudioInfos.Add(new AudioManager.AudioInfo()
                    {
                      m_SourceInfo = sourceInfo,
                      m_SFXEntity = sfxEntity,
                      m_AudioSource = audioSource,
                      m_Status = AudioManager.FadeStatus.FadeIn
                    });
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              int currentEffect = this.m_CurrentEffects[sourceInfo];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              AudioManager.AudioInfo audioInfo = this.m_AudioInfos[currentEffect] with
              {
                m_Status = AudioManager.FadeStatus.FadeIn
              };
              // ISSUE: reference to a compiler-generated field
              this.m_AudioInfos[currentEffect] = audioInfo;
            }
          }
          else if (sourceUpdateInfo.m_Type == SourceUpdateType.Remove)
          {
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              int currentEffect = this.m_CurrentEffects[sourceInfo];
              // ISSUE: reference to a compiler-generated method
              this.Fadeout(sourceInfo, currentEffect);
            }
          }
          else if (sourceUpdateInfo.m_Type == SourceUpdateType.WrongPrefab)
          {
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated field
              int currentEffect = this.m_CurrentEffects[sourceInfo];
              // ISSUE: reference to a compiler-generated field
              this.m_CurrentEffects.Remove(sourceInfo);
              sourceInfo.m_EffectIndex = -2 - sourceInfo.m_EffectIndex;
              // ISSUE: reference to a compiler-generated field
              while (!this.m_CurrentEffects.TryAdd(sourceInfo, currentEffect))
                --sourceInfo.m_EffectIndex;
              // ISSUE: reference to a compiler-generated method
              this.Fadeout(sourceInfo, currentEffect);
            }
          }
          else if (sourceUpdateInfo.m_Type == SourceUpdateType.Temp)
          {
            if (dataRoBufferLookup.HasBuffer(sourceInfo.m_Entity))
            {
              DynamicBuffer<AudioSourceData> dynamicBuffer = dataRoBufferLookup[sourceInfo.m_Entity];
              int index = random.NextInt(dynamicBuffer.Length);
              Entity sfxEntity = dynamicBuffer[index].m_SFXEntity;
              int audioClipId = roComponentLookup3[sfxEntity].m_AudioClipId;
              // ISSUE: reference to a compiler-generated field
              SFX clip = this.m_Clips[audioClipId];
              float volume = clip.m_Volume;
              float num = math.distance((float3) main.transform.position, sourceUpdateInfo.m_Transform.m_Position);
              if ((double) volume > 1.0 / 1000.0 && (double) num < (double) clip.m_MinMaxDistance.y)
              {
                // ISSUE: reference to a compiler-generated method
                float fadedVolume = this.GetFadedVolume(AudioManager.FadeStatus.FadeIn, clip.m_FadeTimes, 0.0f, volume);
                // ISSUE: reference to a compiler-generated method
                AudioSource audioSource = AudioManager.AudioSourcePool.Get();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.SetAudioSourceData(audioSource, this.m_Clips[audioClipId], fadedVolume);
                audioSource.transform.position = (Vector3) sourceUpdateInfo.m_Transform.m_Position;
                // ISSUE: reference to a compiler-generated method
                AudioManager.AudioSourcePool.Play(audioSource);
                // ISSUE: reference to a compiler-generated field
                this.m_TempAudioSources.Add(audioSource);
              }
            }
          }
          else if (sourceUpdateInfo.m_Type == SourceUpdateType.Snap)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_SnapSound);
          }
        }
        this.Dependency.Complete();
        // ISSUE: reference to a compiler-generated method
        this.SyncAudioSources();
      }
    }

    private void UpdateGameAudioSetting()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_SimulationSystem.selectedSpeed == 0.0 && !this.m_IsGamePausedLastUpdate)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_SimulationSystem.selectedSpeed != 0.0 && this.m_IsGamePausedLastUpdate)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
          this.disastersVolume = 0.0001f;
          this.worldVolume = 0.0001f;
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_IsMenuActivatedLastUpdate && this.m_GameScreenUISystem.isMenuActive)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ShouldUnpauseRadioAfterGameUnpaused = this.m_Radio.hasEmergency || !this.m_Radio.paused;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsMenuActivatedLastUpdate && !this.m_GameScreenUISystem.isMenuActive)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
          this.ambienceVolume = 0.0001f;
          this.serviceBuildingsVolume = 0.0001f;
          this.audioGroupsVolume = 0.0001f;
          this.radioVolume = 0.0001f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Radio.ForceRadioPause(!this.m_ShouldUnpauseRadioAfterGameUnpaused);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AudioFadeStatus == AudioManager.FadeStatus.FadeOut)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AudioFadeStatus = AudioManager.FadeStatus.None;
        if ((double) this.disastersVolume > 9.9999997473787516E-05)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Mixer.SetFloat("DisastersVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.disastersVolume - UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
          // ISSUE: reference to a compiler-generated field
          this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
        }
        if ((double) this.worldVolume > 9.9999997473787516E-05)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Mixer.SetFloat("WorldVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.worldVolume - UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
          // ISSUE: reference to a compiler-generated field
          this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_GameScreenUISystem.isMenuActive)
        {
          if ((double) this.serviceBuildingsVolume > 9.9999997473787516E-05)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Mixer.SetFloat("ServiceBuildingsVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.serviceBuildingsVolume - UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
            // ISSUE: reference to a compiler-generated field
            this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
          }
          if ((double) this.ambienceVolume > 9.9999997473787516E-05)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Mixer.SetFloat("AmbienceVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.ambienceVolume - UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
            // ISSUE: reference to a compiler-generated field
            this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
          }
          if ((double) this.audioGroupsVolume > 9.9999997473787516E-05)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Mixer.SetFloat("AudioGroupsVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.audioGroupsVolume - UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
            // ISSUE: reference to a compiler-generated field
            this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
          }
          if ((double) this.radioVolume > 9.9999997473787516E-05)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Mixer.SetFloat("RadioVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.radioVolume - UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
            // ISSUE: reference to a compiler-generated field
            this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeOut;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Radio.ForceRadioPause(true);
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_AudioFadeStatus == AudioManager.FadeStatus.FadeIn)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AudioFadeStatus = AudioManager.FadeStatus.None;
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_SimulationSystem.selectedSpeed != 0.0)
          {
            if ((double) this.disastersVolume < (double) SharedSettings.instance.audio.disastersVolume)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Mixer.SetFloat("DisastersVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.disastersVolume + UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
              // ISSUE: reference to a compiler-generated field
              this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
            }
            if ((double) this.worldVolume < (double) SharedSettings.instance.audio.worldVolume)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Mixer.SetFloat("WorldVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.worldVolume + UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
              // ISSUE: reference to a compiler-generated field
              this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_GameScreenUISystem.isMenuActive)
          {
            if ((double) this.serviceBuildingsVolume < (double) SharedSettings.instance.audio.serviceBuildingsVolume)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Mixer.SetFloat("ServiceBuildingsVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.serviceBuildingsVolume + UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
              // ISSUE: reference to a compiler-generated field
              this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
            }
            if ((double) this.ambienceVolume < (double) SharedSettings.instance.audio.ambienceVolume)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Mixer.SetFloat("AmbienceVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.ambienceVolume + UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
              // ISSUE: reference to a compiler-generated field
              this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
            }
            if ((double) this.audioGroupsVolume < (double) SharedSettings.instance.audio.audioGroupsVolume)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Mixer.SetFloat("AudioGroupsVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.audioGroupsVolume + UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
              // ISSUE: reference to a compiler-generated field
              this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
            }
            if ((double) this.radioVolume < (double) SharedSettings.instance.audio.radioVolume)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Mixer.SetFloat("RadioVolume", Mathf.Log10(Mathf.Min(Mathf.Max(this.radioVolume + UnityEngine.Time.deltaTime / 1f, 0.0001f), 1f)) * 20f);
              // ISSUE: reference to a compiler-generated field
              this.m_AudioFadeStatus = AudioManager.FadeStatus.FadeIn;
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_IsGamePausedLastUpdate = (double) this.m_SimulationSystem.selectedSpeed == 0.0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_IsMenuActivatedLastUpdate = this.m_GameScreenUISystem.isMenuActive;
    }

    private void UpdateTempAudioSources()
    {
      // ISSUE: reference to a compiler-generated field
      foreach (AudioSource tempAudioSource in this.m_TempAudioSources)
      {
        if ((UnityEngine.Object) tempAudioSource != (UnityEngine.Object) null && !tempAudioSource.isPlaying)
        {
          // ISSUE: reference to a compiler-generated method
          AudioManager.AudioSourcePool.Release(tempAudioSource);
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_TempAudioSources.RemoveAll((Predicate<AudioSource>) (audiosouce => (UnityEngine.Object) audiosouce == (UnityEngine.Object) null || (UnityEngine.Object) audiosouce.gameObject == (UnityEngine.Object) null || !audiosouce.gameObject.activeSelf));
    }

    private float GetFadedVolume(
      AudioManager.FadeStatus status,
      float2 sfxFades,
      float currentVolume,
      float targetVolume)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return (double) sfxFades.x != 0.0 && status == AudioManager.FadeStatus.FadeIn && (double) math.abs(currentVolume - targetVolume) > 1.4012984643248171E-45 ? ((double) currentVolume > (double) targetVolume ? math.saturate(currentVolume - this.m_DeltaTime / sfxFades.x * targetVolume) : math.saturate(currentVolume + this.m_DeltaTime / sfxFades.x * targetVolume)) : ((double) sfxFades.y != 0.0 && status == AudioManager.FadeStatus.FadeOut && (double) currentVolume - 0.0 > 1.4012984643248171E-45 ? math.saturate(currentVolume - this.m_DeltaTime / sfxFades.y * targetVolume) : targetVolume);
    }

    private void Fadeout(SourceInfo sourceInfo, int index)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (index >= this.m_AudioInfos.Count || !((UnityEngine.Object) this.m_AudioInfos[index].m_AudioSource != (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AudioManager.AudioInfo audioInfo = this.m_AudioInfos[index] with
      {
        m_SourceInfo = sourceInfo,
        m_Status = AudioManager.FadeStatus.FadeOut
      };
      // ISSUE: reference to a compiler-generated field
      this.m_AudioInfos[index] = audioInfo;
    }

    private void RemoveAudio(SourceInfo sourceInfo, int index)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_AudioInfos[index].m_AudioSource != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AudioManager.AudioSourcePool.Release(this.m_AudioInfos[index].m_AudioSource);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentEffects.Remove(sourceInfo);
      // ISSUE: reference to a compiler-generated field
      if (index < this.m_AudioInfos.Count - 1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AudioInfos[index] = this.m_AudioInfos[this.m_AudioInfos.Count - 1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentEffects[this.m_AudioInfos[index].m_SourceInfo] = index;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AudioInfos.RemoveAt(this.m_AudioInfos.Count - 1);
    }

    private void SyncAudioSources()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AudioInfos.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AudioManager.AudioInfo audioInfo = this.m_AudioInfos[index];
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) audioInfo.m_AudioSource == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RemoveAudio(audioInfo.m_SourceInfo, index);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!audioInfo.m_AudioSource.isPlaying && this.m_CurrentEffects.ContainsKey(audioInfo.m_SourceInfo))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.RemoveAudio(audioInfo.m_SourceInfo, index);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Tools.EditorContainer> roComponentLookup1 = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Moving> roComponentLookup2 = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Temp> roComponentLookup3 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<AudioEffectData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_VehicleAudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<VehicleAudioEffectData> roComponentLookup5 = this.__TypeHandle.__Game_Prefabs_VehicleAudioEffectData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Effect> effectRoBufferLookup1 = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<EnabledEffect> effectRoBufferLookup2 = this.__TypeHandle.__Game_Effects_EnabledEffect_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EffectInstance_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<EffectInstance> roComponentLookup6 = this.__TypeHandle.__Game_Effects_EffectInstance_RO_ComponentLookup;
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<EnabledEffectData> enabledData = this.m_EffectControlSystem.GetEnabledData(true, out dependencies);
      dependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_AudioInfos.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AudioManager.AudioInfo audioInfo = this.m_AudioInfos[index];
        PrefabRef component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.EntityManager.TryGetComponent<PrefabRef>(audioInfo.m_SourceInfo.m_Entity, out component) && (UnityEngine.Object) audioInfo.m_AudioSource != (UnityEngine.Object) null)
        {
          bool flag = false;
          Game.Objects.Transform transform = new Game.Objects.Transform();
          Entity entity = new Entity();
          float intensity;
          // ISSUE: reference to a compiler-generated field
          if (audioInfo.m_SourceInfo.m_EffectIndex >= 0)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<EnabledEffect> effects = effectRoBufferLookup2[audioInfo.m_SourceInfo.m_Entity];
            DynamicBuffer<Effect> bufferData;
            if (effectRoBufferLookup1.TryGetBuffer(component.m_Prefab, out bufferData))
            {
              EnabledEffect effect1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.GetEffect(effects, audioInfo.m_SourceInfo.m_EffectIndex, out effect1))
              {
                // ISSUE: reference to a compiler-generated field
                Effect effect2 = bufferData[audioInfo.m_SourceInfo.m_EffectIndex];
                EnabledEffectData enabledEffectData = enabledData[effect1.m_EnabledIndex];
                intensity = enabledEffectData.m_Intensity;
                flag = (enabledEffectData.m_Flags & EnabledEffectFlags.AudioDisabled) > (EnabledEffectFlags) 0;
                transform = new Game.Objects.Transform(enabledEffectData.m_Position, enabledEffectData.m_Rotation);
                // ISSUE: reference to a compiler-generated field
                entity = audioInfo.m_SourceInfo.m_Entity;
              }
              else
                goto label_28;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (roComponentLookup1.HasComponent(audioInfo.m_SourceInfo.m_Entity) && effects.Length != 0)
              {
                EnabledEffect enabledEffect = effects[0];
                EnabledEffectData enabledEffectData = enabledData[enabledEffect.m_EnabledIndex];
                intensity = enabledEffectData.m_Intensity;
                flag = (enabledEffectData.m_Flags & EnabledEffectFlags.AudioDisabled) > (EnabledEffectFlags) 0;
                transform = new Game.Objects.Transform(enabledEffectData.m_Position, enabledEffectData.m_Rotation);
                // ISSUE: reference to a compiler-generated field
                entity = audioInfo.m_SourceInfo.m_Entity;
              }
              else
                goto label_28;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (audioInfo.m_SourceInfo.m_EffectIndex == -1)
            {
              // ISSUE: reference to a compiler-generated field
              EffectInstance effectInstance = roComponentLookup6[audioInfo.m_SourceInfo.m_Entity];
              intensity = effectInstance.m_Intensity;
              transform = new Game.Objects.Transform(effectInstance.m_Position, effectInstance.m_Rotation);
            }
            else
              goto label_28;
          }
          // ISSUE: reference to a compiler-generated field
          int audioClipId = roComponentLookup4[audioInfo.m_SFXEntity].m_AudioClipId;
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            audioInfo.m_Status = AudioManager.FadeStatus.FadeOut;
          }
          // ISSUE: reference to a compiler-generated field
          float3 position = (float3) audioInfo.m_AudioSource.transform.position;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.UpdateAudioSource(audioInfo.m_AudioSource, this.m_Clips[audioClipId], transform, intensity, entity == this.followed && entity != Entity.Null, index, audioInfo.m_Status, audioInfo.m_SourceInfo))
          {
            --index;
            continue;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          audioInfo.m_MaxVolume = this.m_Clips[audioClipId].m_Volume * intensity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          audioInfo.m_Velocity = ((float3) audioInfo.m_AudioSource.transform.position - position) / math.max(1E-06f, this.m_DeltaTime);
          // ISSUE: reference to a compiler-generated field
          this.m_AudioInfos[index] = audioInfo;
          // ISSUE: reference to a compiler-generated field
          if (roComponentLookup5.HasComponent(audioInfo.m_SFXEntity))
          {
            if (roComponentLookup3.HasComponent(entity))
              entity = roComponentLookup3[entity].m_Original;
            float velocity = 0.0f;
            if (roComponentLookup2.HasComponent(entity))
              velocity = math.length(roComponentLookup2[entity].m_Velocity);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateAudioSourceByVelocity(audioInfo.m_AudioSource, velocity, roComponentLookup5[audioInfo.m_SFXEntity], audioInfo.m_Status);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num = this.m_Clips[audioClipId].m_Doppler * math.saturate((float) (1.0 - ((double) this.m_SimulationSystem.smoothSpeed - 1.0) * 0.30000001192092896));
            // ISSUE: reference to a compiler-generated field
            if ((double) audioInfo.m_AudioSource.dopplerLevel != (double) num)
            {
              // ISSUE: reference to a compiler-generated field
              audioInfo.m_AudioSource.dopplerLevel = num;
              continue;
            }
            continue;
          }
          continue;
        }
label_28:
        // ISSUE: reference to a compiler-generated field
        audioInfo.m_Status = AudioManager.FadeStatus.FadeOut;
        // ISSUE: reference to a compiler-generated field
        this.m_AudioInfos[index] = audioInfo;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) audioInfo.m_AudioSource.volume < 1.0 / 1000.0 || (double) audioInfo.m_MaxVolume < 1.0 / 1000.0 || !this.EntityManager.Exists(audioInfo.m_SFXEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.RemoveAudio(audioInfo.m_SourceInfo, index--);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          int audioClipId = roComponentLookup4[audioInfo.m_SFXEntity].m_AudioClipId;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          audioInfo.m_AudioSource.transform.position += (Vector3) (audioInfo.m_Velocity * this.m_DeltaTime);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          audioInfo.m_AudioSource.volume = this.GetFadedVolume(audioInfo.m_Status, this.m_Clips[audioClipId].m_FadeTimes, audioInfo.m_AudioSource.volume, audioInfo.m_MaxVolume);
        }
      }
    }

    public static float3 GetClosestSourcePosition(
      float3 targetPosition,
      Game.Objects.Transform sourceTransform,
      float3 sourceOffset,
      float3 sourceSize)
    {
      float3 local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(sourceTransform), targetPosition);
      sourceOffset.x = math.clamp(local.x, sourceOffset.x - sourceSize.x, sourceOffset.x + sourceSize.x);
      sourceOffset.y = math.clamp(local.y, sourceOffset.y - sourceSize.y, sourceOffset.y + sourceSize.y);
      sourceOffset.z = math.clamp(local.z, sourceOffset.z - sourceSize.z, sourceOffset.z + sourceSize.z);
      return ObjectUtils.LocalToWorld(sourceTransform, sourceOffset);
    }

    private void UpdateAudioSourceByVelocity(
      AudioSource audioSource,
      float velocity,
      VehicleAudioEffectData vehicleData,
      AudioManager.FadeStatus status)
    {
      velocity = math.saturate((float) (((double) velocity - (double) vehicleData.m_SpeedLimits.x) / ((double) vehicleData.m_SpeedLimits.y - (double) vehicleData.m_SpeedLimits.x)));
      audioSource.pitch = math.lerp(vehicleData.m_SpeedPitches.x, vehicleData.m_SpeedPitches.y, velocity);
      float y = math.lerp(vehicleData.m_SpeedVolumes.x, vehicleData.m_SpeedVolumes.y, velocity);
      if (status == AudioManager.FadeStatus.FadeOut)
        audioSource.volume = math.min(audioSource.volume, y);
      else
        audioSource.volume = y;
    }

    private bool UpdateAudioSource(
      AudioSource audioSource,
      SFX sfx,
      Game.Objects.Transform transform,
      float intensity,
      bool disableDoppler,
      int i = -1,
      AudioManager.FadeStatus status = AudioManager.FadeStatus.None,
      SourceInfo sourceInfo = default (SourceInfo))
    {
      if ((UnityEngine.Object) audioSource == (UnityEngine.Object) null)
        return false;
      float3 float3 = transform.m_Position;
      if ((double) sfx.m_SourceSize.x > 0.0 || (double) sfx.m_SourceSize.y > 0.0 || (double) sfx.m_SourceSize.z > 0.0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        float3 = AudioManager.GetClosestSourcePosition((float3) this.m_AudioListener.transform.position, transform, float3.zero, sfx.m_SourceSize);
        disableDoppler = true;
      }
      audioSource.transform.position = (Vector3) float3;
      audioSource.dopplerLevel = disableDoppler ? 0.0f : sfx.m_Doppler;
      float targetVolume = sfx.m_Volume * intensity;
      if (i >= 0)
      {
        if (status == AudioManager.FadeStatus.FadeOut && ((double) audioSource.volume < 1.0 / 1000.0 || (double) targetVolume < 1.0 / 1000.0))
        {
          // ISSUE: reference to a compiler-generated method
          this.RemoveAudio(sourceInfo, i);
          return false;
        }
        // ISSUE: reference to a compiler-generated method
        targetVolume = this.GetFadedVolume(status, sfx.m_FadeTimes, audioSource.volume, targetVolume);
      }
      audioSource.volume = targetVolume;
      return true;
    }

    private AudioMixerGroup GetAudioMixerGroup(MixerGroup group)
    {
      switch (group)
      {
        case MixerGroup.Ambient:
          // ISSUE: reference to a compiler-generated field
          return this.m_AmbientGroup;
        case MixerGroup.Radio:
          // ISSUE: reference to a compiler-generated field
          return this.m_RadioGroup;
        case MixerGroup.UI:
          // ISSUE: reference to a compiler-generated field
          return this.m_UIGroup;
        case MixerGroup.Menu:
          // ISSUE: reference to a compiler-generated field
          return this.m_MenuGroup;
        case MixerGroup.World:
          // ISSUE: reference to a compiler-generated field
          return this.m_WorldGroup;
        case MixerGroup.ServiceBuildings:
          // ISSUE: reference to a compiler-generated field
          return this.m_ServiceBuildingGroup;
        case MixerGroup.AudioGroups:
          // ISSUE: reference to a compiler-generated field
          return this.m_AudioGroupGroup;
        case MixerGroup.Disasters:
          // ISSUE: reference to a compiler-generated field
          return this.m_DisasterGroup;
        default:
          return (AudioMixerGroup) null;
      }
    }

    private void SetAudioSourceData(AudioSource audioSource, SFX sfx, float volume)
    {
      audioSource.pitch = sfx.m_Pitch;
      audioSource.volume = volume;
      audioSource.clip = sfx.m_AudioClip;
      audioSource.loop = sfx.m_Loop;
      audioSource.minDistance = sfx.m_MinMaxDistance.x;
      audioSource.maxDistance = sfx.m_MinMaxDistance.y;
      audioSource.spatialBlend = sfx.m_SpatialBlend;
      audioSource.spread = sfx.m_Spread;
      audioSource.rolloffMode = sfx.m_RolloffMode;
      if (sfx.m_RolloffMode == AudioRolloffMode.Custom)
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, sfx.m_RolloffCurve);
      // ISSUE: reference to a compiler-generated method
      audioSource.outputAudioMixerGroup = this.GetAudioMixerGroup(sfx.m_MixerGroup);
      audioSource.dopplerLevel = 0.0f;
      audioSource.timeSamples = 0;
      if (sfx.m_RandomStartTime)
      {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint) DateTime.Now.Ticks);
        audioSource.timeSamples = random.NextInt(sfx.m_AudioClip.samples);
      }
      audioSource.ignoreListenerPause = false;
      audioSource.priority = (int) sfx.m_Priority;
    }

    public bool GetRandomizeAudio(Entity sfxEntity, out SFX sfx)
    {
      sfx = (SFX) null;
      DynamicBuffer<AudioRandomizeData> buffer;
      if (!this.EntityManager.HasComponent<AudioRandomizeData>(sfxEntity) || !this.EntityManager.TryGetBuffer<AudioRandomizeData>(sfxEntity, true, out buffer))
        return false;
      int index = new Unity.Mathematics.Random((uint) DateTime.Now.Ticks).NextInt(buffer.Length);
      Entity sfxEntity1 = buffer[index].m_SFXEntity;
      // ISSUE: reference to a compiler-generated field
      sfx = this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(sfxEntity1).m_AudioClipId];
      return true;
    }

    public void PlayLightningSFX(float3 targetPos)
    {
      // ISSUE: reference to a compiler-generated field
      WeatherAudioData componentData = this.EntityManager.GetComponentData<WeatherAudioData>(this.m_WeatherAudioEntitiyQuery.GetSingletonEntity());
      // ISSUE: reference to a compiler-generated field
      float delay = math.distance((float3) this.m_AudioListener.transform.position, targetPos) / componentData.m_LightningSoundSpeed;
      SFX sfx;
      // ISSUE: reference to a compiler-generated method
      if (!this.GetRandomizeAudio(componentData.m_LightningAudio, out sfx))
      {
        // ISSUE: reference to a compiler-generated field
        sfx = this.m_Clips[this.EntityManager.GetComponentData<AudioEffectData>(componentData.m_LightningAudio).m_AudioClipId];
      }
      if (!((UnityEngine.Object) sfx.m_AudioClip != (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated method
      AudioSource audioSource = AudioManager.AudioSourcePool.Get();
      audioSource.clip = sfx.m_AudioClip;
      audioSource.transform.position = (Vector3) targetPos;
      // ISSUE: reference to a compiler-generated method
      audioSource.outputAudioMixerGroup = this.GetAudioMixerGroup(sfx.m_MixerGroup);
      audioSource.pitch = sfx.m_Pitch;
      audioSource.volume = sfx.m_Volume;
      audioSource.spatialBlend = sfx.m_SpatialBlend;
      audioSource.dopplerLevel = sfx.m_Doppler;
      audioSource.spread = sfx.m_Spread;
      audioSource.loop = sfx.m_Loop;
      audioSource.minDistance = sfx.m_MinMaxDistance.x;
      audioSource.maxDistance = sfx.m_MinMaxDistance.y;
      audioSource.rolloffMode = sfx.m_RolloffMode;
      if (sfx.m_RolloffMode == AudioRolloffMode.Custom)
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, sfx.m_RolloffCurve);
      audioSource.timeSamples = 0;
      audioSource.ignoreListenerPause = false;
      audioSource.priority = (int) sfx.m_Priority;
      // ISSUE: reference to a compiler-generated method
      AudioManager.AudioSourcePool.PlayDelayed(audioSource, delay);
      // ISSUE: reference to a compiler-generated field
      this.m_TempAudioSources.Add(audioSource);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      if (this.radio != null && this.radio.currentChannel != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastSaveRadioChannel = this.radio.currentChannel.name;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastSaveRadioSkipAds = this.m_Radio.skipAds;
      }
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastSaveRadioChannel);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastSaveRadioSkipAds);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Game.Version.saveRadioStations))
        return;
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastSaveRadioChannel);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastSaveRadioSkipAds);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastSaveRadioChannel = string.Empty;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSaveRadioSkipAds = false;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastSaveRadioChannel = string.Empty;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSaveRadioSkipAds = false;
    }

    public void PreSerialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LastSaveRadioChannel = string.Empty;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSaveRadioSkipAds = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public AudioManager()
    {
    }

    public static class AudioSourcePool
    {
      private static int s_InstanceCount;
      private static int s_LoadedSize;
      private static int s_PlayingSize;
      private static int s_MaxLoadedSize = 268435456;
      private static Stack<AudioSource> s_Pool = new Stack<AudioSource>();
      private static System.Collections.Generic.Dictionary<AudioClip, int> s_PlayingClips = new System.Collections.Generic.Dictionary<AudioClip, int>();
      private static List<AudioClip> s_UnloadClips = new List<AudioClip>();

      public static int memoryBudget
      {
        get => AudioManager.AudioSourcePool.s_MaxLoadedSize;
        set
        {
          AudioManager.AudioSourcePool.s_MaxLoadedSize = value;
          AudioManager.AudioSourcePool.UnloadClips();
        }
      }

      public static void Stats(
        out int loadedSize,
        out int maxLoadedSize,
        out int loadedCount,
        out int playingSize,
        out int playingCount)
      {
        // ISSUE: reference to a compiler-generated field
        loadedSize = AudioManager.AudioSourcePool.s_LoadedSize;
        // ISSUE: reference to a compiler-generated field
        maxLoadedSize = AudioManager.AudioSourcePool.s_MaxLoadedSize;
        // ISSUE: reference to a compiler-generated field
        playingCount = AudioManager.AudioSourcePool.s_PlayingClips.Count;
        // ISSUE: reference to a compiler-generated field
        loadedCount = playingCount + AudioManager.AudioSourcePool.s_UnloadClips.Count;
        // ISSUE: reference to a compiler-generated field
        playingSize = AudioManager.AudioSourcePool.s_PlayingSize;
      }

      public static void Reset()
      {
        // ISSUE: reference to a compiler-generated field
        AudioManager.AudioSourcePool.s_LoadedSize = 0;
        // ISSUE: reference to a compiler-generated field
        AudioManager.AudioSourcePool.s_PlayingSize = 0;
        // ISSUE: reference to a compiler-generated field
        AudioManager.AudioSourcePool.s_PlayingClips.Clear();
        // ISSUE: reference to a compiler-generated field
        AudioManager.AudioSourcePool.s_UnloadClips.Clear();
      }

      public static AudioSource Get()
      {
        // ISSUE: reference to a compiler-generated field
        if (AudioManager.AudioSourcePool.s_Pool.Count > 0)
        {
          // ISSUE: reference to a compiler-generated field
          AudioSource audioSource = AudioManager.AudioSourcePool.s_Pool.Pop();
          if ((UnityEngine.Object) audioSource != (UnityEngine.Object) null)
          {
            audioSource.gameObject.SetActive(true);
            return audioSource;
          }
        }
        return CreateAudioSource();

        static AudioSource CreateAudioSource()
        {
          // ISSUE: reference to a compiler-generated field
          return new GameObject("AudioSource" + AudioManager.AudioSourcePool.s_InstanceCount++.ToString()).AddComponent<AudioSource>();
        }
      }

      public static void Play(AudioSource audioSource)
      {
        // ISSUE: reference to a compiler-generated method
        AudioManager.AudioSourcePool.AddClip(audioSource.clip);
        audioSource.Play();
      }

      public static void PlayDelayed(AudioSource audioSource, float delay)
      {
        // ISSUE: reference to a compiler-generated method
        AudioManager.AudioSourcePool.AddClip(audioSource.clip);
        audioSource.PlayDelayed(delay);
      }

      private static void AddClip(AudioClip audioClip)
      {
        if (audioClip.preloadAudioData)
          return;
        int num;
        // ISSUE: reference to a compiler-generated field
        if (AudioManager.AudioSourcePool.s_PlayingClips.TryGetValue(audioClip, out num))
        {
          // ISSUE: reference to a compiler-generated field
          AudioManager.AudioSourcePool.s_PlayingClips[audioClip] = num + 1;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          int clipSize = AudioManager.AudioSourcePool.GetClipSize(audioClip);
          // ISSUE: reference to a compiler-generated field
          if (!AudioManager.AudioSourcePool.s_UnloadClips.Remove(audioClip))
          {
            // ISSUE: reference to a compiler-generated field
            AudioManager.AudioSourcePool.s_LoadedSize += clipSize;
            // ISSUE: reference to a compiler-generated method
            AudioManager.AudioSourcePool.UnloadClips();
          }
          // ISSUE: reference to a compiler-generated field
          AudioManager.AudioSourcePool.s_PlayingClips.Add(audioClip, 1);
          // ISSUE: reference to a compiler-generated field
          AudioManager.AudioSourcePool.s_PlayingSize += clipSize;
        }
      }

      private static void UnloadClips()
      {
        int count;
        AudioClip unloadClip;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        for (count = 0; AudioManager.AudioSourcePool.s_LoadedSize > AudioManager.AudioSourcePool.s_MaxLoadedSize && count < AudioManager.AudioSourcePool.s_UnloadClips.Count; AudioManager.AudioSourcePool.s_LoadedSize -= AudioManager.AudioSourcePool.GetClipSize(unloadClip))
        {
          // ISSUE: reference to a compiler-generated field
          unloadClip = AudioManager.AudioSourcePool.s_UnloadClips[count++];
          unloadClip.UnloadAudioData();
        }
        if (count <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        AudioManager.AudioSourcePool.s_UnloadClips.RemoveRange(0, count);
      }

      private static void RemoveClip(AudioClip audioClip)
      {
        int num1;
        // ISSUE: reference to a compiler-generated field
        if (!AudioManager.AudioSourcePool.s_PlayingClips.TryGetValue(audioClip, out num1))
          return;
        int num2;
        if ((num2 = num1 - 1) == 0)
        {
          // ISSUE: reference to a compiler-generated method
          int clipSize = AudioManager.AudioSourcePool.GetClipSize(audioClip);
          // ISSUE: reference to a compiler-generated field
          AudioManager.AudioSourcePool.s_PlayingClips.Remove(audioClip);
          // ISSUE: reference to a compiler-generated field
          AudioManager.AudioSourcePool.s_PlayingSize -= clipSize;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (AudioManager.AudioSourcePool.s_LoadedSize > AudioManager.AudioSourcePool.s_MaxLoadedSize)
          {
            audioClip.UnloadAudioData();
            // ISSUE: reference to a compiler-generated field
            AudioManager.AudioSourcePool.s_LoadedSize -= clipSize;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            AudioManager.AudioSourcePool.s_UnloadClips.Add(audioClip);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          AudioManager.AudioSourcePool.s_PlayingClips[audioClip] = num2;
        }
      }

      private static int GetClipSize(AudioClip audioClip)
      {
        return audioClip.samples * audioClip.channels * 2;
      }

      public static void Release(AudioSource audioSource)
      {
        if (!((UnityEngine.Object) audioSource != (UnityEngine.Object) null))
          return;
        AudioClip clip = audioSource.clip;
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        audioSource.clip = (AudioClip) null;
        audioSource.volume = 1f;
        audioSource.pitch = 0.0f;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
        // ISSUE: reference to a compiler-generated field
        AudioManager.AudioSourcePool.s_Pool.Push(audioSource);
        // ISSUE: reference to a compiler-generated method
        AudioManager.AudioSourcePool.RemoveClip(clip);
      }
    }

    private enum FadeStatus
    {
      None,
      FadeIn,
      FadeOut,
    }

    private struct AudioInfo
    {
      public SourceInfo m_SourceInfo;
      public Entity m_SFXEntity;
      public AudioSource m_AudioSource;
      public AudioManager.FadeStatus m_Status;
      public float m_MaxVolume;
      public float3 m_Velocity;
    }

    private class CameraAmbientAudioInfo
    {
      public int id;
      public float height;
      public AudioSource source;
      public UnityEngine.Transform transform;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AudioSourceData> __Game_Prefabs_AudioSourceData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<EnabledEffect> __Game_Effects_EnabledEffect_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> __Game_Prefabs_AudioEffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectInstance> __Game_Effects_EffectInstance_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleAudioEffectData> __Game_Prefabs_VehicleAudioEffectData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioSourceData_RO_BufferLookup = state.GetBufferLookup<AudioSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RO_BufferLookup = state.GetBufferLookup<EnabledEffect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioEffectData_RO_ComponentLookup = state.GetComponentLookup<AudioEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EffectInstance_RO_ComponentLookup = state.GetComponentLookup<EffectInstance>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_VehicleAudioEffectData_RO_ComponentLookup = state.GetComponentLookup<VehicleAudioEffectData>(true);
      }
    }
  }
}
