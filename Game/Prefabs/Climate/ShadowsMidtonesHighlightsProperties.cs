// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.ShadowsMidtonesHighlightsProperties
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
  public class ShadowsMidtonesHighlightsProperties : OverrideablePropertiesComponent
  {
    public Vector4Parameter m_Shadows = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0.0f));
    public Vector4Parameter m_Midtones = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0.0f));
    public Vector4Parameter m_Highlights = new Vector4Parameter(new Vector4(1f, 1f, 1f, 0.0f));
    [Header("Shadow Limits")]
    public MinFloatParameter m_ShadowsStart = new MinFloatParameter(0.0f, 0.0f);
    public MinFloatParameter m_ShadowsEnd = new MinFloatParameter(0.3f, 0.0f);
    [Header("Highlight Limits")]
    public MinFloatParameter m_HighlightsStart = new MinFloatParameter(0.55f, 0.0f);
    public MinFloatParameter m_HighlightsEnd = new MinFloatParameter(1f, 0.0f);

    protected override void OnBindVolumeProperties(Volume volume)
    {
      ShadowsMidtonesHighlights component = (ShadowsMidtonesHighlights) null;
      VolumeHelper.GetOrCreateVolumeComponent<ShadowsMidtonesHighlights>(volume, ref component);
      this.m_Shadows = component.shadows;
      this.m_Midtones = component.midtones;
      this.m_Highlights = component.highlights;
      this.m_ShadowsStart = component.shadowsStart;
      this.m_ShadowsEnd = component.shadowsEnd;
      this.m_HighlightsStart = component.highlightsStart;
      this.m_HighlightsEnd = component.highlightsEnd;
    }
  }
}
