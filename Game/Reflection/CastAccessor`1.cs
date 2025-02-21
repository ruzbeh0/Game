// Decompiled with JetBrains decompiler
// Type: Game.Reflection.CastAccessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;

#nullable disable
namespace Game.Reflection
{
  public class CastAccessor<T> : ITypedValueAccessor<T>, IValueAccessor, IEquatable<CastAccessor<T>>
  {
    [NotNull]
    private readonly IValueAccessor m_Accessor;
    [NotNull]
    private readonly Converter<object, T> m_FromObject;
    [NotNull]
    private readonly Converter<T, object> m_ToObject;

    public CastAccessor([NotNull] IValueAccessor accessor)
      : this(accessor, new Converter<object, T>(CastAccessor<T>.FromObject), new Converter<T, object>(CastAccessor<T>.ToObject))
    {
    }

    public CastAccessor(
      [NotNull] IValueAccessor accessor,
      [NotNull] Converter<object, T> fromObject,
      [NotNull] Converter<T, object> toObject)
    {
      this.m_Accessor = accessor ?? throw new ArgumentNullException(nameof (accessor));
      this.m_FromObject = fromObject ?? throw new ArgumentNullException(nameof (fromObject));
      this.m_ToObject = toObject ?? throw new ArgumentNullException(nameof (toObject));
    }

    public Type valueType => typeof (T);

    public object GetValue() => (object) this.GetTypedValue();

    public void SetValue(object value) => this.SetTypedValue((T) value);

    public T GetTypedValue() => this.m_FromObject(this.m_Accessor.GetValue());

    public void SetTypedValue(T value) => this.m_Accessor.SetValue(this.m_ToObject(value));

    public bool Equals(CastAccessor<T> other)
    {
      if (other == null)
        return false;
      return this == other || this.m_Accessor.Equals((object) other.m_Accessor);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((CastAccessor<T>) obj);
    }

    public override int GetHashCode() => this.m_Accessor.GetHashCode();

    private static T FromObject(object value) => (T) value;

    private static object ToObject(T value) => (object) value;
  }
}
