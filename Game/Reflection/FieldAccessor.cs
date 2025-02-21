// Decompiled with JetBrains decompiler
// Type: Game.Reflection.FieldAccessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.Reflection;

#nullable disable
namespace Game.Reflection
{
  public class FieldAccessor : IValueAccessor, IEquatable<FieldAccessor>
  {
    [NotNull]
    private readonly IValueAccessor m_Parent;
    [NotNull]
    private readonly FieldInfo m_Field;

    public FieldAccessor([NotNull] IValueAccessor parent, [NotNull] FieldInfo field)
    {
      this.m_Parent = parent ?? throw new ArgumentNullException(nameof (parent));
      this.m_Field = field ?? throw new ArgumentNullException(nameof (field));
    }

    public Type valueType => this.m_Field.FieldType;

    public IValueAccessor parent => this.m_Parent;

    public object GetValue() => this.m_Field.GetValue(this.m_Parent.GetValue());

    public void SetValue(object value)
    {
      object obj = this.m_Parent.GetValue();
      this.m_Field.SetValue(obj, value);
      if (!this.m_Parent.valueType.IsValueType)
        return;
      this.m_Parent.SetValue(obj);
    }

    public bool Equals(FieldAccessor other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return this.m_Parent.Equals((object) other.m_Parent) && this.m_Field.Equals((object) other.m_Field);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((FieldAccessor) obj);
    }

    public override int GetHashCode()
    {
      return this.m_Parent.GetHashCode() * 397 ^ this.m_Field.GetHashCode();
    }
  }
}
