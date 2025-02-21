// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ReadonlyField`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Reflection;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class ReadonlyField<T> : NamedWidgetWithTooltip, IDisableCallback
  {
    protected const string kValue = "value";
    protected T m_Value;
    private IWriter<T> m_ValueWriter;
    private int m_ValueVersion = -1;

    public ITypedValueAccessor<T> accessor { get; set; }

    [CanBeNull]
    public Func<int> valueVersion { get; set; }

    protected IWriter<T> valueWriter
    {
      get => this.m_ValueWriter ?? (this.m_ValueWriter = ValueWriters.Create<T>());
      set => this.m_ValueWriter = value;
    }

    public virtual T GetValue() => this.accessor.GetTypedValue();

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.valueVersion != null)
      {
        int num = this.valueVersion();
        if (num != this.m_ValueVersion)
        {
          widgetChanges |= WidgetChanges.Properties;
          this.m_ValueVersion = num;
          this.m_Value = this.GetValue();
        }
      }
      else
      {
        T newValue = this.GetValue();
        if (!this.ValueEquals(newValue, this.m_Value))
        {
          widgetChanges |= WidgetChanges.Properties;
          this.m_Value = newValue;
        }
      }
      return widgetChanges;
    }

    protected virtual bool ValueEquals(T newValue, T oldValue)
    {
      return EqualityComparer<T>.Default.Equals(newValue, oldValue);
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("value");
      this.valueWriter.Write(writer, this.m_Value);
    }
  }
}
