// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.LocalizedNumber`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Localization
{
  public readonly struct LocalizedNumber<T> : 
    ILocElement,
    IJsonWritable,
    IEquatable<LocalizedNumber<T>>
  {
    private readonly IWriter<T> m_ValueWriter;

    public T value { get; }

    [CanBeNull]
    public string unit { get; }

    public bool signed { get; }

    public LocalizedNumber(T value, [CanBeNull] string unit = null, bool signed = false, IWriter<T> valueWriter = null)
    {
      this.value = value;
      this.unit = unit;
      this.signed = signed;
      this.m_ValueWriter = valueWriter ?? ValueWriters.Create<T>();
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("Game.UI.Localization.LocalizedNumber");
      writer.PropertyName("value");
      this.m_ValueWriter.Write(writer, this.value);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.PropertyName("signed");
      writer.Write(this.signed);
      writer.TypeEnd();
    }

    public bool Equals(LocalizedNumber<T> other)
    {
      return EqualityComparer<T>.Default.Equals(this.value, other.value) && this.unit == other.unit && this.signed == other.signed;
    }

    public override bool Equals(object obj)
    {
      return obj is LocalizedNumber<T> other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<T, string, bool>(this.value, this.unit, this.signed);
    }
  }
}
