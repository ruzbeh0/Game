// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.NumberTooltip`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.Tooltip
{
  public abstract class NumberTooltip<T> : LabelIconTooltip where T : IEquatable<T>
  {
    private T m_Value;
    private string m_Unit = "integer";
    private bool m_Signed;
    private IWriter<T> m_ValueWriter;

    public T value
    {
      get => this.m_Value;
      set
      {
        if (value.Equals(this.m_Value))
          return;
        this.m_Value = value;
        this.SetPropertiesChanged();
      }
    }

    public string unit
    {
      get => this.m_Unit;
      set
      {
        if (!(value != this.m_Unit))
          return;
        this.m_Unit = value;
        this.SetPropertiesChanged();
      }
    }

    public bool signed
    {
      get => this.m_Signed;
      set
      {
        if (value == this.m_Signed)
          return;
        this.m_Signed = value;
        this.SetPropertiesChanged();
      }
    }

    protected IWriter<T> valueWriter
    {
      get
      {
        if (this.m_ValueWriter == null)
          this.m_ValueWriter = ValueWriters.Create<T>();
        return this.m_ValueWriter;
      }
      set => this.m_ValueWriter = value;
    }

    public override string propertiesTypeName => "Game.UI.Tooltip.NumberTooltip";

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("value");
      this.valueWriter.Write(writer, this.value);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.PropertyName("signed");
      writer.Write(this.signed);
    }
  }
}
