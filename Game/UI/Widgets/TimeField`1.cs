// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.TimeField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class TimeField<T> : Field<T>
  {
    public float min { get; set; }

    public float max { get; set; } = 1f;

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("min");
      writer.Write(this.min);
      writer.PropertyName("max");
      writer.Write(this.max);
    }
  }
}
