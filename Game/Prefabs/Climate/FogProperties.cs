// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.FogProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Prefabs.Climate
{
  [ComponentMenu("Weather/", new Type[] {typeof (WeatherPrefab)})]
  public class FogProperties : OverrideablePropertiesComponent
  {
    protected override void OnBindVolumeProperties(Volume volume)
    {
      Fog component = (Fog) null;
      VolumeHelper.GetOrCreateVolumeComponent<Fog>(volume, ref component);
    }
  }
}
