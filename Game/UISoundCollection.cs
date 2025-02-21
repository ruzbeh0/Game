// Decompiled with JetBrains decompiler
// Type: Game.UISoundCollection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game
{
  [CreateAssetMenu(menuName = "Colossal/UI/UISoundCollection", order = 1)]
  public class UISoundCollection : ScriptableObject
  {
    public UISoundCollection.SoundInfo[] m_Sounds;
    private Dictionary<string, UISoundCollection.SoundInfo> m_SoundsDict;

    private void OnEnable()
    {
      if (this.m_Sounds == null)
        this.m_Sounds = new UISoundCollection.SoundInfo[0];
      this.m_SoundsDict = new Dictionary<string, UISoundCollection.SoundInfo>();
      this.RefreshSoundsDict();
    }

    public void PlaySound(int soundIndex, float volume = 1f)
    {
      if (soundIndex < 0 || this.m_Sounds.Length <= soundIndex)
        return;
      UISoundCollection.SoundInfo sound = this.m_Sounds[soundIndex];
      this.PlaySound(sound.m_Clip, volume * sound.m_Volume);
    }

    public void PlaySound(string soundName, float volume = 1f)
    {
      UISoundCollection.SoundInfo soundInfo;
      if (!this.m_SoundsDict.TryGetValue(soundName, out soundInfo))
        return;
      this.PlaySound(soundInfo.m_Clip, volume * soundInfo.m_Volume);
    }

    private void PlaySound([NotNull] AudioClip clip, float volume)
    {
      if (!(bool) (UnityEngine.Object) Camera.main)
        return;
      // ISSUE: variable of a compiler-generated type
      AudioManager instance = AudioManager.instance;
      // ISSUE: reference to a compiler-generated method
      instance?.PlayUISound(clip, volume);
    }

    public void RefreshSoundsDict()
    {
      this.m_SoundsDict.Clear();
      foreach (UISoundCollection.SoundInfo sound in this.m_Sounds)
        this.m_SoundsDict[sound.m_Name] = sound;
    }

    [Serializable]
    public class SoundInfo
    {
      public string m_Name;
      public AudioClip m_Clip;
      [Range(0.0f, 1f)]
      public float m_Volume = 1f;
    }
  }
}
