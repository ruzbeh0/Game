// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IntField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class IntField<T> : Field<T>
  {
    public int min { get; set; } = int.MinValue;

    public int max { get; set; } = int.MaxValue;

    public int step { get; set; } = 1;

    public int stepMultiplier { get; set; } = 10;

    public Func<int> dynamicMin { get; set; }

    public Func<int> dynamicMax { get; set; }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.dynamicMin != null)
      {
        int num = this.dynamicMin();
        if (num != this.min)
        {
          widgetChanges |= WidgetChanges.Properties;
          this.min = num;
        }
      }
      if (this.dynamicMax != null)
      {
        int num = this.dynamicMax();
        if (num != this.max)
        {
          widgetChanges |= WidgetChanges.Properties;
          this.max = num;
        }
      }
      return widgetChanges;
    }

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
      writer.Write(this.stepMultiplier);
    }
  }
}
