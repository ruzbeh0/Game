// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.RadioUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.IO.AssetDatabase;
using Colossal.UI.Binding;
using Game.Audio;
using Game.Prefabs;
using Game.Rendering;
using Game.Settings;
using Game.UI.Localization;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class RadioUISystem : UISystemBase
  {
    private const string kGroup = "radio";
    private PrefabSystem m_PrefabSystem;
    private Game.Audio.Radio.Radio m_Radio;
    private GamePanelUISystem m_GamePanelUISystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private ValueBinding<bool> m_PausedBinding;
    private ValueBinding<bool> m_MutedBinding;
    private ValueBinding<bool> m_SkipAds;
    private GetterValueBinding<Game.Audio.Radio.Radio.RadioNetwork[]> m_NetworksBinding;
    private GetterValueBinding<Game.Audio.Radio.Radio.RuntimeRadioChannel[]> m_StationsBinding;
    private ValueBinding<RadioUISystem.ClipInfo> m_CurrentSegmentBinding;
    private EventBinding m_SegmentChangedBinding;
    private System.Collections.Generic.Dictionary<string, string> m_LastSelectedStations;
    private CachedLocalizedStringBuilder<string> m_EmergencyMessages;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_Radio = AudioManager.instance.radio;
      this.m_GamePanelUISystem = this.World.GetOrCreateSystemManaged<GamePanelUISystem>();
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated method
      this.m_GamePanelUISystem.SetDefaultArgs((GamePanel) new RadioPanel());
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("radio", "enabled", (Func<bool>) (() => SharedSettings.instance.audio.radioActive)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("radio", "volume", (Func<float>) (() => SharedSettings.instance.audio.radioVolume)));
      this.AddBinding((IBinding) (this.m_PausedBinding = new ValueBinding<bool>("radio", "paused", this.m_Radio.paused)));
      this.AddBinding((IBinding) (this.m_MutedBinding = new ValueBinding<bool>("radio", "muted", this.m_Radio.muted)));
      this.AddBinding((IBinding) (this.m_SkipAds = new ValueBinding<bool>("radio", "skipAds", this.m_Radio.skipAds)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("radio", "emergencyMode", (Func<bool>) (() => this.m_Radio.hasEmergency)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("radio", "emergencyFocusable", (Func<bool>) (() => this.m_Radio.emergencyTarget != Entity.Null)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Entity>("radio", "emergencyMessage", (Func<Entity>) (() => this.m_Radio.emergency), (IWriter<Entity>) new DelegateWriter<Entity>(new WriterDelegate<Entity>(this.WriteEmergencyMessage))));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<string>("radio", "selectedNetwork", (Func<string>) (() => AudioManager.instance.radio.currentChannel?.network), (IWriter<string>) new StringWriter().Nullable<string>()));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<string>("radio", "selectedStation", (Func<string>) (() => AudioManager.instance.radio.currentChannel?.name), (IWriter<string>) new StringWriter().Nullable<string>()));
      this.AddBinding((IBinding) (this.m_NetworksBinding = new GetterValueBinding<Game.Audio.Radio.Radio.RadioNetwork[]>("radio", "networks", (Func<Game.Audio.Radio.Radio.RadioNetwork[]>) (() => AudioManager.instance.radio.networkDescriptors), (IWriter<Game.Audio.Radio.Radio.RadioNetwork[]>) new ArrayWriter<Game.Audio.Radio.Radio.RadioNetwork>((IWriter<Game.Audio.Radio.Radio.RadioNetwork>) new ValueWriter<Game.Audio.Radio.Radio.RadioNetwork>()))));
      this.AddBinding((IBinding) (this.m_StationsBinding = new GetterValueBinding<Game.Audio.Radio.Radio.RuntimeRadioChannel[]>("radio", "stations", (Func<Game.Audio.Radio.Radio.RuntimeRadioChannel[]>) (() => AudioManager.instance.radio.radioChannelDescriptors), (IWriter<Game.Audio.Radio.Radio.RuntimeRadioChannel[]>) new ArrayWriter<Game.Audio.Radio.Radio.RuntimeRadioChannel>((IWriter<Game.Audio.Radio.Radio.RuntimeRadioChannel>) new ValueWriter<Game.Audio.Radio.Radio.RuntimeRadioChannel>()))));
      this.AddBinding((IBinding) (this.m_CurrentSegmentBinding = new ValueBinding<RadioUISystem.ClipInfo>("radio", "currentSegment", this.GetCurrentClipInfo(), (IWriter<RadioUISystem.ClipInfo>) new ValueWriter<RadioUISystem.ClipInfo>().Nullable<RadioUISystem.ClipInfo>())));
      this.AddBinding((IBinding) (this.m_SegmentChangedBinding = new EventBinding("radio", "segmentChanged")));
      this.AddBinding((IBinding) new TriggerBinding<float>("radio", "setVolume", new Action<float>(this.SetVolume)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("radio", "setPaused", new Action<bool>(this.SetPaused)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("radio", "setMuted", new Action<bool>(this.SetMuted)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("radio", "setSkipAds", new Action<bool>(this.SetSkipAds)));
      this.AddBinding((IBinding) new TriggerBinding("radio", "playPrevious", new Action(this.PlayPrevious)));
      this.AddBinding((IBinding) new TriggerBinding("radio", "playNext", new Action(this.PlayNext)));
      this.AddBinding((IBinding) new TriggerBinding("radio", "focusEmergency", new Action(this.FocusEmergency)));
      this.AddBinding((IBinding) new TriggerBinding<string>("radio", "selectNetwork", new Action<string>(this.SelectNetwork)));
      this.AddBinding((IBinding) new TriggerBinding<string>("radio", "selectStation", new Action<string>(this.SelectStation)));
      this.m_EmergencyMessages = CachedLocalizedStringBuilder<string>.Id((Func<string, string>) (name => "Radio.EMERGENCY_MESSAGE[" + name + "]"));
      this.m_LastSelectedStations = new System.Collections.Generic.Dictionary<string, string>();
      this.m_Radio.Reloaded += new Game.Audio.Radio.Radio.OnRadioEvent(this.OnRadioReloaded);
      this.m_Radio.ProgramChanged += new Game.Audio.Radio.Radio.OnRadioEvent(this.OnProgramChanged);
      this.m_Radio.ClipChanged += new Game.Audio.Radio.Radio.OnClipChanged(this.OnClipChanged);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.m_Radio.Reloaded -= new Game.Audio.Radio.Radio.OnRadioEvent(this.OnRadioReloaded);
      this.m_Radio.ProgramChanged -= new Game.Audio.Radio.Radio.OnRadioEvent(this.OnProgramChanged);
      this.m_Radio.ClipChanged -= new Game.Audio.Radio.Radio.OnClipChanged(this.OnClipChanged);
      base.OnDestroy();
    }

    private void WriteEmergencyMessage(IJsonWriter writer, Entity entity)
    {
      if (entity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(this.m_Radio.emergency);
        writer.Write<LocalizedString>(this.m_EmergencyMessages[prefab.name]);
      }
      else
        writer.WriteNull();
    }

    private AudioAsset.Metatag GetMetaType(Game.Audio.Radio.Radio.SegmentType type)
    {
      return type == Game.Audio.Radio.Radio.SegmentType.Playlist || type != Game.Audio.Radio.Radio.SegmentType.Commercial ? AudioAsset.Metatag.Artist : AudioAsset.Metatag.Brand;
    }

    private RadioUISystem.ClipInfo GetClipInfo(Game.Audio.Radio.Radio radio, AudioAsset asset)
    {
      if (!((AssetData) asset != (IAssetData) null))
        return (RadioUISystem.ClipInfo) null;
      if (asset.GetMetaTag(AudioAsset.Metatag.Type) == "Music")
        return new RadioUISystem.ClipInfo()
        {
          title = asset.GetMetaTag(AudioAsset.Metatag.Title),
          info = asset.GetMetaTag(AudioAsset.Metatag.Artist)
        };
      return new RadioUISystem.ClipInfo()
      {
        title = radio.currentChannel.name,
        info = radio.currentChannel.currentProgram.name
      };
    }

    private RadioUISystem.ClipInfo GetCurrentClipInfo()
    {
      return this.GetClipInfo(this.m_Radio, this.m_Radio.currentClip.m_Asset);
    }

    private void OnClipChanged(Game.Audio.Radio.Radio radio, AudioAsset asset)
    {
      this.m_StationsBinding.TriggerUpdate();
      this.m_CurrentSegmentBinding.Update(this.GetClipInfo(radio, asset));
    }

    private void OnRadioReloaded(Game.Audio.Radio.Radio radio)
    {
      this.m_NetworksBinding.Update();
      this.m_StationsBinding.Update();
      this.m_SkipAds.Update(radio.skipAds);
    }

    private void OnProgramChanged(Game.Audio.Radio.Radio radio)
    {
      this.m_StationsBinding.TriggerUpdate();
    }

    private void SetVolume(float volume)
    {
      SharedSettings.instance.audio.radioVolume = volume;
      SharedSettings.instance.audio.Apply();
    }

    private void SetPaused(bool paused)
    {
      this.m_Radio.paused = paused;
      this.m_PausedBinding.Update(paused);
    }

    private void SetMuted(bool muted)
    {
      this.m_Radio.muted = muted;
      this.m_MutedBinding.Update(muted);
    }

    private void SetSkipAds(bool skipAds)
    {
      this.m_Radio.skipAds = skipAds;
      this.m_SkipAds.Update(skipAds);
    }

    private void PlayPrevious() => AudioManager.instance.radio.PreviousSong();

    private void PlayNext() => AudioManager.instance.radio.NextSong();

    private void FocusEmergency()
    {
      if (!((UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null) || !(this.m_Radio.emergencyTarget != Entity.Null))
        return;
      this.m_CameraUpdateSystem.orbitCameraController.followedEntity = this.m_Radio.emergencyTarget;
      this.m_CameraUpdateSystem.orbitCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
      this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController;
    }

    private void SelectNetwork(string name)
    {
      string name1;
      if (this.m_LastSelectedStations.TryGetValue(name, out name1))
      {
        this.SelectStation(name1);
      }
      else
      {
        foreach (Game.Audio.Radio.Radio.RuntimeRadioChannel channelDescriptor in AudioManager.instance.radio.radioChannelDescriptors)
        {
          if (channelDescriptor.network == name)
          {
            this.SelectStation(channelDescriptor.name);
            break;
          }
        }
      }
    }

    private void SelectStation(string name)
    {
      Game.Audio.Radio.Radio.RuntimeRadioChannel radioChannel = AudioManager.instance.radio.GetRadioChannel(name);
      if (radioChannel == null)
        return;
      Game.Audio.Radio.Radio.RuntimeRadioChannel currentChannel = AudioManager.instance.radio.currentChannel;
      if (currentChannel != null)
        this.m_LastSelectedStations[currentChannel.network] = currentChannel.name;
      AudioManager.instance.radio.currentChannel = radioChannel;
    }

    [Preserve]
    public RadioUISystem()
    {
    }

    public class ClipInfo : IJsonWritable
    {
      public string title;
      [CanBeNull]
      public string info;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("radio.Clip");
        writer.PropertyName("title");
        writer.Write(this.title);
        writer.PropertyName("info");
        writer.Write(this.info);
        writer.TypeEnd();
      }
    }
  }
}
