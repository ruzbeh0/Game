// Decompiled with JetBrains decompiler
// Type: Game.Reflection.PropertyAccessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.Reflection;

#nullable disable
namespace Game.Reflection
{
  public class PropertyAccessor : IValueAccessor, IEquatable<PropertyAccessor>
  {
    [NotNull]
    private readonly IValueAccessor m_Parent;
    [NotNull]
    private readonly MethodInfo m_Getter;
    [CanBeNull]
    private readonly MethodInfo m_Setter;

    public PropertyAccessor([NotNull] IValueAccessor parent, [NotNull] MethodInfo getter, [CanBeNull] MethodInfo setter)
    {
      this.m_Parent = parent ?? throw new ArgumentNullException(nameof (parent));
      this.m_Getter = getter ?? throw new ArgumentNullException(nameof (getter));
      this.m_Setter = setter;
    }

    public Type valueType => this.m_Getter.ReturnType;

    public object GetValue() => this.m_Getter.Invoke(this.m_Parent.GetValue(), (object[]) null);

    public void SetValue(object value)
    {
      if (!(this.m_Setter != (MethodInfo) null))
        return;
      this.m_Setter.Invoke(this.m_Parent.GetValue(), new object[1]
      {
        value
      });
    }

    public bool Equals(PropertyAccessor other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return this.m_Parent.Equals((object) other.m_Parent) && this.m_Getter.Equals((object) other.m_Getter);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((PropertyAccessor) obj);
    }

    public override int GetHashCode()
    {
      return this.m_Parent.GetHashCode() * 397 ^ this.m_Getter.GetHashCode();
    }
  }
}
