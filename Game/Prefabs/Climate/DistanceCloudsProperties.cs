// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.DistanceCloudsProperties
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
  public class DistanceCloudsProperties : OverrideablePropertiesComponent
  {
    public ClampedFloatParameter m_Opacity = new ClampedFloatParameter(1f, 0.0f, 1f);
    public ClampedFloatParameter m_CumulusStrength = new ClampedFloatParameter(1f, 0.0f, 1f);
    public ClampedFloatParameter m_StratusStrength = new ClampedFloatParameter(1f, 0.0f, 1f);
    public ClampedFloatParameter m_CirrusStrength = new ClampedFloatParameter(1f, 0.0f, 1f);
    public ClampedFloatParameter m_WispyStrength = new ClampedFloatParameter(1f, 0.0f, 1f);
    public MinFloatParameter m_Altitude = new MinFloatParameter(2000f, 0.0f);

    protected override void OnBindVolumeProperties(Volume volume)
    {
      CloudLayer component = (CloudLayer) null;
      VolumeHelper.GetOrCreateVolumeComponent<CloudLayer>(volume, ref component);
      this.m_Opacity = component.opacity;
      this.m_CumulusStrength = component.layerA.opacityR;
      this.m_StratusStrength = component.layerA.opacityG;
      this.m_CirrusStrength = component.layerA.opacityB;
      this.m_WispyStrength = component.layerA.opacityA;
      this.m_Altitude = component.layerA.altitude;
    }
  }
}
