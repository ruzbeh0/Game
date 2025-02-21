// Decompiled with JetBrains decompiler
// Type: Game.Reflection.ObjectAccessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Reflection
{
  public class ObjectAccessor<T> : 
    ITypedValueAccessor<T>,
    IValueAccessor,
    IEquatable<ObjectAccessor<T>>
  {
    protected T m_Object;
    private bool m_ReadOnly;

    public ObjectAccessor(T obj, bool readOnly = true)
    {
      this.m_Object = obj;
      this.m_ReadOnly = readOnly;
    }

    public Type valueType => this.m_Object.GetType();

    public virtual object GetValue() => (object) this.GetTypedValue();

    public virtual void SetValue(object value) => this.SetTypedValue((T) value);

    public T GetTypedValue() => this.m_Object;

    public void SetTypedValue(T value)
    {
      if (this.m_ReadOnly)
        throw new InvalidOperationException("ObjectAccessor is readonly");
      this.m_Object = value;
    }

    public bool Equals(ObjectAccessor<T> other)
    {
      if (other == null)
        return false;
      return this == other || object.Equals((object) this.m_Object, (object) other.m_Object);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((ObjectAccessor<T>) obj);
    }

    public override int GetHashCode() => this.m_Object.GetHashCode();
  }
}
