// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.ColorAdjustmentsProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Prefabs.Climate
{
  [ComponentMenu("Weather/", new System.Type[] {typeof (WeatherPrefab)})]
  public class ColorAdjustmentsProperties : OverrideablePropertiesComponent
  {
    public FloatParameter m_PostExposure = new FloatParameter(0.0f);
    public ClampedFloatParameter m_Contrast = new ClampedFloatParameter(0.0f, -100f, 100f);
    public ColorParameter m_ColorFilter = new ColorParameter(Color.white, true, false, true);
    public ClampedFloatParameter m_HueShift = new ClampedFloatParameter(0.0f, -180f, 180f);
    public ClampedFloatParameter m_Saturation = new ClampedFloatParameter(0.0f, -100f, 100f);

    protected override void OnBindVolumeProperties(Volume volume)
    {
      ColorAdjustments component = (ColorAdjustments) null;
      VolumeHelper.GetOrCreateVolumeComponent<ColorAdjustments>(volume, ref component);
      this.m_PostExposure = component.postExposure;
      this.m_Contrast = component.contrast;
      this.m_ColorFilter = component.colorFilter;
      this.m_HueShift = component.hueShift;
      this.m_Saturation = component.saturation;
    }
  }
}
