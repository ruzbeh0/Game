// Decompiled with JetBrains decompiler
// Type: Game.Reflection.NativeArrayElementAccessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using Unity.Collections;

#nullable disable
namespace Game.Reflection
{
  public class NativeArrayElementAccessor<T> : 
    ITypedValueAccessor<T>,
    IValueAccessor,
    IEquatable<NativeArrayElementAccessor<T>>
    where T : struct
  {
    [NotNull]
    private readonly ITypedValueAccessor<NativeArray<T>> m_Parent;
    private readonly int m_Index;

    public NativeArrayElementAccessor([NotNull] ITypedValueAccessor<NativeArray<T>> parent, int index)
    {
      this.m_Parent = parent ?? throw new ArgumentNullException(nameof (parent));
      this.m_Index = index;
    }

    public Type valueType => typeof (T);

    public object GetValue() => (object) this.GetTypedValue();

    public void SetValue(object value) => this.SetTypedValue((T) value);

    public T GetTypedValue() => this.m_Parent.GetTypedValue()[this.m_Index];

    public void SetTypedValue(T value) => this.m_Parent.GetTypedValue()[this.m_Index] = value;

    public bool Equals(NativeArrayElementAccessor<T> other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return this.m_Parent.Equals((object) other.m_Parent) && this.m_Index == other.m_Index;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((NativeArrayElementAccessor<T>) obj);
    }

    public override int GetHashCode() => this.m_Parent.GetHashCode() * 397 ^ this.m_Index;
  }
}
