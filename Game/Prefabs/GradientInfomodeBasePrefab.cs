// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GradientInfomodeBasePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.UI.Localization;
using System;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public abstract class GradientInfomodeBasePrefab : InfomodeBasePrefab, IGradientInfomode
  {
    private static readonly CachedLocalizedStringBuilder<string> kLabels = CachedLocalizedStringBuilder<string>.Id((Func<string, string>) (hash => "Infoviews.LABEL[" + hash + "]"));
    public Color m_Low = Color.red;
    public Color m_Medium = Color.yellow;
    public Color m_High = Color.green;
    public int m_Steps = 11;
    public GradientLegendType m_LegendType;
    [Tooltip("Good, Bad, Low, High, Weak, Strong, Old, Young")]
    public string m_LowLabelId;
    public string m_MediumLabelId;
    [Tooltip("Good, Bad, Low, High, Weak, Strong, Old, Young")]
    public string m_HighLabelId;

    private static LocalizedString? GetLabel(string id)
    {
      return string.IsNullOrEmpty(id) ? new LocalizedString?() : new LocalizedString?(GradientInfomodeBasePrefab.kLabels[id]);
    }

    public Color lowColor => GradientInfomodeBasePrefab.Opaque(this.m_Low);

    public Color mediumColor => GradientInfomodeBasePrefab.Opaque(this.m_Medium);

    public Color highColor => GradientInfomodeBasePrefab.Opaque(this.m_High);

    public GradientLegendType legendType => this.m_LegendType;

    public LocalizedString? lowLabel => GradientInfomodeBasePrefab.GetLabel(this.m_LowLabelId);

    public LocalizedString? mediumLabel
    {
      get => GradientInfomodeBasePrefab.GetLabel(this.m_MediumLabelId);
    }

    public LocalizedString? highLabel => GradientInfomodeBasePrefab.GetLabel(this.m_HighLabelId);

    public override void GetColors(
      out Color color0,
      out Color color1,
      out Color color2,
      out float steps,
      out float speed,
      out float tiling,
      out float fill)
    {
      color0 = this.m_Low;
      color1 = this.m_Medium;
      color2 = this.m_High;
      steps = (float) this.m_Steps;
      speed = 0.0f;
      tiling = 0.0f;
      fill = 0.0f;
    }

    private static Color Opaque(Color color)
    {
      color.a = 1f;
      return color;
    }
  }
}
