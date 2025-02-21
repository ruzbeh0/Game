// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.MinMaxField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class MinMaxField<T> : Field<T>
  {
    protected abstract T defaultMin { get; }

    protected abstract T defaultMax { get; }

    public Func<T> dynamicMin { get; set; }

    public Func<T> dynamicMax { get; set; }

    public T min { get; set; }

    public T max { get; set; }

    protected MinMaxField()
    {
      this.min = this.defaultMin;
      this.max = this.defaultMax;
    }

    public virtual bool IsEqual(T x, T y) => EqualityComparer<T>.Default.Equals(x, y);

    public abstract T ToFieldType(double4 value);

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.dynamicMin != null)
      {
        T x = this.dynamicMin();
        if (!this.IsEqual(x, this.min))
        {
          widgetChanges |= WidgetChanges.Properties;
          this.min = x;
        }
      }
      if (this.dynamicMax != null)
      {
        T x = this.dynamicMax();
        if (!this.IsEqual(x, this.max))
        {
          widgetChanges |= WidgetChanges.Properties;
          this.max = x;
        }
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("min");
      this.valueWriter.Write(writer, this.min);
      writer.PropertyName("max");
      this.valueWriter.Write(writer, this.max);
    }
  }
}
