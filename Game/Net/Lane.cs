// Decompiled with JetBrains decompiler
// Type: Game.Net.Lane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Pathfind;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct Lane : 
    IComponentData,
    IQueryTypeParameter,
    IEquatable<Lane>,
    IStrideSerializable,
    ISerializable
  {
    public PathNode m_StartNode;
    public PathNode m_MiddleNode;
    public PathNode m_EndNode;

    public bool Equals(Lane other)
    {
      return this.m_StartNode.Equals(other.m_StartNode) && this.m_MiddleNode.Equals(other.m_MiddleNode) && this.m_EndNode.Equals(other.m_EndNode);
    }

    public override int GetHashCode() => this.m_MiddleNode.GetHashCode();

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write<PathNode>(this.m_StartNode);
      writer.Write<PathNode>(this.m_MiddleNode);
      writer.Write<PathNode>(this.m_EndNode);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read<PathNode>(out this.m_StartNode);
      reader.Read<PathNode>(out this.m_MiddleNode);
      reader.Read<PathNode>(out this.m_EndNode);
    }

    public int GetStride(Context context) => this.m_StartNode.GetStride(context) * 3;
  }
}
