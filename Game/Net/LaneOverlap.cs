// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneOverlap
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [InternalBufferCapacity(0)]
  public struct LaneOverlap : IBufferElementData, IEmptySerializable, IComparable<LaneOverlap>
  {
    public Entity m_Other;
    public OverlapFlags m_Flags;
    public byte m_ThisStart;
    public byte m_ThisEnd;
    public byte m_OtherStart;
    public byte m_OtherEnd;
    public byte m_Parallelism;
    public sbyte m_PriorityDelta;

    public LaneOverlap(
      Entity other,
      float4 overlap,
      OverlapFlags flags,
      float parallelism,
      int priorityDelta)
    {
      int4 int4 = math.clamp((int4) math.round(overlap * (float) byte.MaxValue), (int4) 0, (int4) (int) byte.MaxValue);
      this.m_Other = other;
      this.m_ThisStart = (byte) int4.x;
      this.m_ThisEnd = (byte) int4.y;
      this.m_OtherStart = (byte) int4.z;
      this.m_OtherEnd = (byte) int4.w;
      this.m_Flags = flags;
      this.m_Parallelism = (byte) math.clamp((int) math.round(parallelism * 128f), 0, (int) byte.MaxValue);
      this.m_PriorityDelta = (sbyte) priorityDelta;
    }

    public int CompareTo(LaneOverlap other)
    {
      return ((int) this.m_ThisStart << 8 | (int) this.m_ThisEnd) - ((int) other.m_ThisStart << 8 | (int) other.m_ThisEnd);
    }
  }
}
