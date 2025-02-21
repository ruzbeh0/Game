// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.UIntField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public class UIntField : Field<uint>
  {
    public uint min { get; set; }

    public uint max { get; set; } = uint.MaxValue;

    public uint step { get; set; } = 1;

    public uint stepMultiplier { get; set; } = 10;

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("min");
      writer.Write(this.min);
      writer.PropertyName("max");
      writer.Write(this.max);
      writer.PropertyName("step");
      writer.Write(this.step);
      writer.PropertyName("stepMultiplier");
      writer.Write(this.step);
    }
  }
}
