// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Float2SliderField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.UI.Widgets
{
  public class Float2SliderField : FloatSliderField<float2>
  {
    protected override float2 defaultMin => new float2(float.MinValue);

    protected override float2 defaultMax => new float2(float.MaxValue);

    public override float2 ToFieldType(double4 value) => new float2(value.xy);
  }
}
