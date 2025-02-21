// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.VignetteProperties
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
  public class VignetteProperties : OverrideablePropertiesComponent
  {
    public ColorParameter m_Color = new ColorParameter(Color.black, false, false, true);
    public Vector2Parameter m_Center = new Vector2Parameter(new Vector2(0.5f, 0.5f));
    public ClampedFloatParameter m_Intensity = new ClampedFloatParameter(0.0f, 0.0f, 1f);
    public ClampedFloatParameter m_Smoothness = new ClampedFloatParameter(0.2f, 0.01f, 1f);
    public ClampedFloatParameter m_Roundness = new ClampedFloatParameter(1f, 0.0f, 1f);
    public BoolParameter m_Rounded = new BoolParameter(false);

    protected override void OnBindVolumeProperties(Volume volume)
    {
      Vignette component = (Vignette) null;
      VolumeHelper.GetOrCreateVolumeComponent<Vignette>(volume, ref component);
      this.m_Color = component.color;
      this.m_Center = component.center;
      this.m_Intensity = component.intensity;
      this.m_Smoothness = component.smoothness;
      this.m_Roundness = component.roundness;
      this.m_Rounded = component.rounded;
      component.mode.Override(VignetteMode.Procedural);
    }
  }
}
