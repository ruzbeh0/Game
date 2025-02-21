// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.GradientSliderField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Widgets
{
  public class GradientSliderField : FloatSliderField<float>, IIconProvider
  {
    protected override float defaultMin => float.MinValue;

    protected override float defaultMax => float.MaxValue;

    public override float ToFieldType(double4 value) => (float) value.x;

    public ColorGradient gradient { get; set; }

    public Func<string> iconSrc { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("gradient");
      writer.Write<ColorGradient>(this.gradient);
      writer.PropertyName("iconSrc");
      writer.Write(this.iconSrc());
    }
  }
}
