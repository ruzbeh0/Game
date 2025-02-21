// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.EulerAnglesField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.UI.Widgets
{
  public class EulerAnglesField : FloatField<float3>
  {
    protected override float3 defaultMin => new float3(float.MinValue);

    protected override float3 defaultMax => new float3(float.MaxValue);

    public override float3 ToFieldType(double4 value) => new float3(value.xyz);
  }
}
