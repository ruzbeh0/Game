// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.LocalizedFraction`1
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
  public readonly struct LocalizedFraction<T> : 
    ILocElement,
    IJsonWritable,
    IEquatable<LocalizedFraction<T>>
  {
    private readonly IWriter<T> m_ValueWriter;

    public T value { get; }

    public T total { get; }

    [CanBeNull]
    public string unit { get; }

    public LocalizedFraction(T value, T total, [CanBeNull] string unit = null, IWriter<T> valueWriter = null)
    {
      this.value = value;
      this.total = total;
      this.unit = unit;
      this.m_ValueWriter = valueWriter ?? ValueWriters.Create<T>();
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("Game.UI.Localization.LocalizedFraction");
      writer.PropertyName("value");
      this.m_ValueWriter.Write(writer, this.value);
      writer.PropertyName("total");
      this.m_ValueWriter.Write(writer, this.total);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.TypeEnd();
    }

    public bool Equals(LocalizedFraction<T> other)
    {
      return EqualityComparer<T>.Default.Equals(this.value, other.value) && EqualityComparer<T>.Default.Equals(this.total, other.total) && this.unit == other.unit;
    }

    public override bool Equals(object obj)
    {
      return obj is LocalizedFraction<T> other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<T, T, string>(this.value, this.total, this.unit);
    }
  }
}
