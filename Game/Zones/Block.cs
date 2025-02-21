// Decompiled with JetBrains decompiler
// Type: Game.Zones.Block
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  public struct Block : IComponentData, IQueryTypeParameter, IEquatable<Block>, ISerializable
  {
    public float3 m_Position;
    public float2 m_Direction;
    public int2 m_Size;

    public bool Equals(Block other)
    {
      return this.m_Position.Equals(other.m_Position) && this.m_Direction.Equals(other.m_Direction) && this.m_Size.Equals(other.m_Size);
    }

    public override int GetHashCode() => this.m_Position.GetHashCode();

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_Direction);
      writer.Write(this.m_Size);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      reader.Read(out this.m_Direction);
      reader.Read(out this.m_Size);
    }
  }
}
