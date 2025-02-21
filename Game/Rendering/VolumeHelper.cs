// Decompiled with JetBrains decompiler
// Type: Game.Rendering.VolumeHelper
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Rendering
{
  public static class VolumeHelper
  {
    public const int kQualityVolumePriority = 100;
    public const int kGameVolumePriority = 50;
    public const int kOverrideVolumePriority = 2000;
    private const string kSectionName = "======Volumes======";
    private static List<Volume> m_Volumes = new List<Volume>();

    public static void Dispose()
    {
      for (int index = VolumeHelper.m_Volumes.Count - 1; index >= 0; --index)
        VolumeHelper.DestroyVolume(VolumeHelper.m_Volumes[index]);
    }

    private static VolumeProfile CreateVolumeProfile(string overrideName)
    {
      VolumeProfile instance = ScriptableObject.CreateInstance<VolumeProfile>();
      instance.name = overrideName + "Profile";
      instance.hideFlags = HideFlags.DontSave;
      return instance;
    }

    public static Volume CreateVolume(string name, int priority)
    {
      GameObject gameObject = OrderedGameObjectSpawner.Get("======Volumes======").Create(name);
      gameObject.hideFlags = HideFlags.DontSave;
      Volume component = gameObject.GetComponent<Volume>();
      component.priority = (float) priority;
      component.sharedProfile = VolumeHelper.CreateVolumeProfile(name);
      VolumeHelper.m_Volumes.Add(component);
      return component;
    }

    public static void DestroyVolume(Volume volume)
    {
      VolumeHelper.m_Volumes.Remove(volume);
      if ((Object) volume.sharedProfile != (Object) null)
        CoreUtils.Destroy((Object) volume.sharedProfile);
      if (!((Object) volume != (Object) null))
        return;
      CoreUtils.Destroy((Object) volume.gameObject);
    }

    public static void GetOrCreateVolumeComponent<PT>(Volume volume, ref PT component) where PT : VolumeComponent
    {
      VolumeHelper.GetOrCreateVolumeComponent<PT>(volume.profileRef, ref component);
    }

    public static void GetOrCreateVolumeComponent<PT>(VolumeProfile profile, ref PT component) where PT : VolumeComponent
    {
      if (!((Object) component == (Object) null) || profile.TryGet<PT>(out component))
        return;
      component = profile.Add<PT>();
      if (!(component is VolumeComponentWithQuality componentWithQuality))
        return;
      componentWithQuality.quality.Override(3);
    }
  }
}
