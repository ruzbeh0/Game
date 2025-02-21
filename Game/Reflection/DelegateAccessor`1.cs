// Decompiled with JetBrains decompiler
// Type: Game.Reflection.DelegateAccessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System;

#nullable disable
namespace Game.Reflection
{
  public class DelegateAccessor<T> : ITypedValueAccessor<T>, IValueAccessor
  {
    [NotNull]
    private readonly Func<T> m_Getter;
    [CanBeNull]
    private readonly Action<T> m_Setter;

    public DelegateAccessor([NotNull] Func<T> getter, [CanBeNull] Action<T> setter = null)
    {
      this.m_Getter = getter ?? throw new ArgumentNullException(nameof (getter));
      this.m_Setter = setter;
    }

    public Type valueType => typeof (T);

    public virtual object GetValue() => (object) this.GetTypedValue();

    public virtual void SetValue(object value) => this.SetTypedValue((T) value);

    public T GetTypedValue() => this.m_Getter();

    public void SetTypedValue(T value)
    {
      if (this.m_Setter == null)
        throw new InvalidOperationException("DelegateAccessor is readonly");
      this.m_Setter(value);
    }
  }
}
