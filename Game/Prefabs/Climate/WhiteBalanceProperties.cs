// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.WhiteBalanceProperties
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
  public class WhiteBalanceProperties : OverrideablePropertiesComponent
  {
    public ClampedFloatParameter m_Temperature = new ClampedFloatParameter(0.0f, -100f, 100f);
    public ClampedFloatParameter m_Tint = new ClampedFloatParameter(0.0f, -100f, 100f);

    protected override void OnBindVolumeProperties(Volume volume)
    {
      WhiteBalance component = (WhiteBalance) null;
      VolumeHelper.GetOrCreateVolumeComponent<WhiteBalance>(volume, ref component);
      this.m_Temperature = component.temperature;
      this.m_Tint = component.tint;
    }
  }
}
