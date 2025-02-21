// Decompiled with JetBrains decompiler
// Type: Game.Reflection.NativeValueAccessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Collections;
using System;

#nullable disable
namespace Game.Reflection
{
  public class NativeValueAccessor<T> : 
    ITypedValueAccessor<T>,
    IValueAccessor,
    IEquatable<NativeValueAccessor<T>>
    where T : unmanaged
  {
    [NotNull]
    private readonly IValueAccessor m_Parent;

    public NativeValueAccessor([NotNull] IValueAccessor parent)
    {
      this.m_Parent = parent ?? throw new ArgumentNullException(nameof (parent));
    }

    public Type valueType => typeof (T);

    public object GetValue() => (object) this.GetTypedValue();

    public void SetValue(object value) => this.SetTypedValue((T) value);

    public T GetTypedValue() => ((NativeValue<T>) this.m_Parent.GetValue()).value;

    public void SetTypedValue(T value) => ((NativeValue<T>) this.m_Parent.GetValue()).value = value;

    public bool Equals(NativeValueAccessor<T> other)
    {
      if (other == null)
        return false;
      return this == other || this.m_Parent.Equals((object) other.m_Parent);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((NativeValueAccessor<T>) obj);
    }

    public override int GetHashCode() => this.m_Parent.GetHashCode();
  }
}
