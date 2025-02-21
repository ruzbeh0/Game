// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Float4InputField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;

#nullable disable
namespace Game.UI.Widgets
{
  public class Float4InputField : FloatField<float4>
  {
    protected override float4 defaultMin => new float4(float.MinValue);

    protected override float4 defaultMax => new float4(float.MaxValue);

    public override float4 ToFieldType(double4 value) => new float4(value);
  }
}
