// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.HeapBlock
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Game.Rendering.Utilities
{
  [DebuggerDisplay("({begin}, {end}), Length = {Length}")]
  public struct HeapBlock : IComparable<HeapBlock>, IEquatable<HeapBlock>
  {
    public ulong begin;
    public ulong end;

    public HeapBlock(ulong begin, ulong end)
    {
      this.begin = begin;
      this.end = end;
    }

    public static HeapBlock OfSize(ulong begin, ulong size) => new HeapBlock(begin, begin + size);

    public ulong Length => this.end - this.begin;

    public bool Empty => this.Length == 0UL;

    public int CompareTo(HeapBlock other) => this.begin.CompareTo(other.begin);

    public bool Equals(HeapBlock other) => this.CompareTo(other) == 0;
  }
}
