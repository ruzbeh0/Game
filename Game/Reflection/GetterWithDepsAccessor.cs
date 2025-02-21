// Decompiled with JetBrains decompiler
// Type: Game.Reflection.GetterWithDepsAccessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;
using System.Reflection;
using Unity.Jobs;

#nullable disable
namespace Game.Reflection
{
  public class GetterWithDepsAccessor : IValueAccessor, IEquatable<GetterWithDepsAccessor>
  {
    [NotNull]
    private readonly IValueAccessor m_Parent;
    [NotNull]
    private readonly MethodInfo m_Getter;
    [CanBeNull]
    private readonly object[] m_Parameters;
    private readonly int m_DepsIndex;

    public GetterWithDepsAccessor(
      [NotNull] IValueAccessor parent,
      [NotNull] MethodInfo getter,
      [CanBeNull] object[] parameters = null,
      int depsIndex = -1)
    {
      this.m_Parent = parent ?? throw new ArgumentNullException(nameof (parent));
      this.m_Getter = getter ?? throw new ArgumentNullException(nameof (getter));
      this.m_Parameters = parameters;
      this.m_DepsIndex = depsIndex;
    }

    public Type valueType => this.m_Getter.ReturnType;

    public object GetValue()
    {
      object obj = this.m_Getter.Invoke(this.m_Parent.GetValue(), this.m_Parameters);
      if (this.m_DepsIndex == -1 || this.m_Parameters == null)
        return obj;
      ((JobHandle) this.m_Parameters[this.m_DepsIndex]).Complete();
      return obj;
    }

    public void SetValue(object value)
    {
      throw new InvalidOperationException("GetterWithDepsAccessor is readonly");
    }

    public bool Equals(GetterWithDepsAccessor other)
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
      return !(obj.GetType() != this.GetType()) && this.Equals((GetterWithDepsAccessor) obj);
    }

    public override int GetHashCode()
    {
      return this.m_Parent.GetHashCode() * 397 ^ this.m_Getter.GetHashCode();
    }
  }
}
