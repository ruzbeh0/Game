// Decompiled with JetBrains decompiler
// Type: Game.Reflection.ListElementAccessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.Collections;

#nullable disable
namespace Game.Reflection
{
  public class ListElementAccessor<T> : IValueAccessor, IEquatable<ListElementAccessor<T>> where T : IList
  {
    [NotNull]
    private readonly ITypedValueAccessor<T> m_Parent;
    private readonly Type m_ElementType;
    private readonly int m_Index;

    public ListElementAccessor([NotNull] ITypedValueAccessor<T> parent, [NotNull] Type elementType, int index)
    {
      this.m_Parent = parent ?? throw new ArgumentNullException(nameof (parent));
      this.m_ElementType = elementType ?? throw new ArgumentNullException(nameof (elementType));
      this.m_Index = index;
    }

    public Type valueType => this.m_ElementType;

    public object GetValue() => this.m_Parent.GetTypedValue()[this.m_Index];

    public void SetValue(object value) => this.m_Parent.GetTypedValue()[this.m_Index] = value;

    public bool Equals(ListElementAccessor<T> other)
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
      return !(obj.GetType() != this.GetType()) && this.Equals((ListElementAccessor<T>) obj);
    }

    public override int GetHashCode() => this.m_Parent.GetHashCode() * 397 ^ this.m_Index;
  }
}
