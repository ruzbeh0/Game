// Decompiled with JetBrains decompiler
// Type: Game.Reflection.ObjectWithDepsAccessor`1
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
  public class ObjectWithDepsAccessor<T> : ObjectAccessor<T>
  {
    [CanBeNull]
    private readonly FieldInfo[] m_Deps;

    public ObjectWithDepsAccessor([NotNull] T obj, [NotNull] FieldInfo[] deps)
      : base(obj)
    {
      if ((object) obj == null)
        throw new ArgumentNullException(nameof (obj));
      this.m_Deps = deps ?? throw new ArgumentNullException(nameof (deps));
    }

    public override object GetValue()
    {
      if (this.m_Deps != null)
      {
        foreach (FieldInfo dep in this.m_Deps)
          ((JobHandle) dep.GetValue((object) this.m_Object)).Complete();
      }
      return base.GetValue();
    }
  }
}
