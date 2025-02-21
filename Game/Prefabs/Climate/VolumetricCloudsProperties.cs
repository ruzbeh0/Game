// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.VolumetricCloudsProperties
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
  public class VolumetricCloudsProperties : OverrideablePropertiesComponent
  {
    public MinFloatParameter m_BottomAltitude = new MinFloatParameter(1200f, 0.01f);
    public MinFloatParameter m_AltitudeRange = new MinFloatParameter(2000f, 100f);
    public ClampedFloatParameter m_DensityMultiplier = new ClampedFloatParameter(0.4f, 0.0f, 1f);
    public AnimationCurveParameter m_DensityCurve = new AnimationCurveParameter(new AnimationCurve(new Keyframe[3]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(0.15f, 1f),
      new Keyframe(1f, 0.1f)
    }));
    public ClampedFloatParameter m_ShapeFactor = new ClampedFloatParameter(0.9f, 0.0f, 1f);
    public Vector3Parameter m_ShapeOffset = new Vector3Parameter(Vector3.zero);
    public ClampedFloatParameter m_ErosionFactor = new ClampedFloatParameter(0.8f, 0.0f, 1f);
    public ClampedFloatParameter m_ErosionOcclusion = new ClampedFloatParameter(0.1f, 0.0f, 1f);
    public AnimationCurveParameter m_ErosionCurve = new AnimationCurveParameter(new AnimationCurve(new Keyframe[3]
    {
      new Keyframe(0.0f, 1f),
      new Keyframe(0.1f, 0.9f),
      new Keyframe(1f, 1f)
    }));
    public AnimationCurveParameter m_AmbientOcclusionCurve = new AnimationCurveParameter(new AnimationCurve(new Keyframe[3]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(0.25f, 0.4f),
      new Keyframe(1f, 0.0f)
    }));
    public ClampedFloatParameter m_MultiScattering = new ClampedFloatParameter(0.5f, 0.0f, 1f);

    protected override void OnBindVolumeProperties(Volume volume)
    {
      VolumetricClouds component = (VolumetricClouds) null;
      VolumeHelper.GetOrCreateVolumeComponent<VolumetricClouds>(volume, ref component);
      this.m_BottomAltitude = component.bottomAltitude;
      this.m_AltitudeRange = component.altitudeRange;
      this.m_DensityMultiplier = component.densityMultiplier;
      this.m_DensityCurve = component.densityCurve;
      this.m_ShapeFactor = component.shapeFactor;
      this.m_ShapeOffset = component.shapeOffset;
      this.m_ErosionFactor = component.erosionFactor;
      this.m_ErosionOcclusion = component.erosionOcclusion;
      this.m_ErosionCurve = component.erosionCurve;
      this.m_AmbientOcclusionCurve = component.ambientOcclusionCurve;
      this.m_MultiScattering = component.multiScattering;
      component.m_CloudPreset.Override(VolumetricClouds.CloudPresets.Custom);
    }
  }
}
