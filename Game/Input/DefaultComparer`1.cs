// Decompiled with JetBrains decompiler
// Type: Game.Input.DefaultComparer`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Game.Input
{
  public class DefaultComparer<T> : IComparer<T> where T : struct, IComparable<T>
  {
    public static DefaultComparer<T> instance = new DefaultComparer<T>();

    public int Compare(T x, T y) => x.CompareTo(y);
  }
}
