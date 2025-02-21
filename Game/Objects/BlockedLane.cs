// Decompiled with JetBrains decompiler
// Type: Game.Objects.BlockedLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [InternalBufferCapacity(0)]
  public struct BlockedLane : IBufferElementData, IEquatable<BlockedLane>, ISerializable
  {
    public Entity m_Lane;
    public float2 m_CurvePosition;

    public BlockedLane(Entity lane, float2 curvePosition)
    {
      this.m_Lane = lane;
      this.m_CurvePosition = curvePosition;
    }

    public bool Equals(BlockedLane other) => this.m_Lane.Equals(other.m_Lane);

    public override int GetHashCode() => this.m_Lane.GetHashCode();

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
    }
  }
}
