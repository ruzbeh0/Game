// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.LocalizedBounds`1
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
  public readonly struct LocalizedBounds<T> : 
    ILocElement,
    IJsonWritable,
    IEquatable<LocalizedBounds<T>>
  {
    private readonly IWriter<T> m_ValueWriter;

    public T min { get; }

    public T max { get; }

    [CanBeNull]
    public string unit { get; }

    public LocalizedBounds(T min, T max, [CanBeNull] string unit = null, IWriter<T> valueWriter = null)
    {
      this.min = min;
      this.max = max;
      this.unit = unit;
      this.m_ValueWriter = valueWriter ?? ValueWriters.Create<T>();
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin("Game.UI.Localization.LocalizedBounds");
      writer.PropertyName("min");
      this.m_ValueWriter.Write(writer, this.min);
      writer.PropertyName("max");
      this.m_ValueWriter.Write(writer, this.max);
      writer.PropertyName("unit");
      writer.Write(this.unit);
      writer.TypeEnd();
    }

    public bool Equals(LocalizedBounds<T> other)
    {
      return EqualityComparer<T>.Default.Equals(this.min, other.min) && EqualityComparer<T>.Default.Equals(this.max, other.max) && this.unit == other.unit;
    }

    public override bool Equals(object obj)
    {
      return obj is LocalizedBounds<T> other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<T, T, string>(this.min, this.max, this.unit);
    }
  }
}
