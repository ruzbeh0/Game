// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.FloatSliderField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class FloatSliderField<T> : FloatField<T>
  {
    [CanBeNull]
    public string unit { get; set; }

    public bool signed { get; set; }

    public bool separateThousands { get; set; }

    public double maxValueWithFraction { get; set; }

    public bool scaleDragVolume { get; set; }

    public bool updateOnDragEnd { get; set; }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.PropertyName("signed");
      writer.Write(this.signed);
      writer.PropertyName("separateThousands");
      writer.Write(this.separateThousands);
      writer.PropertyName("maxValueWithFraction");
      writer.Write(this.maxValueWithFraction);
      writer.PropertyName("scaleDragVolume");
      writer.Write(this.scaleDragVolume);
      writer.PropertyName("updateOnDragEnd");
      writer.Write(this.updateOnDragEnd);
    }
  }
}
