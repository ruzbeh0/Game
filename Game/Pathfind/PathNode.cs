// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathNode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  public struct PathNode : IEquatable<PathNode>, IStrideSerializable, ISerializable
  {
    private const float FLOAT_TO_INT = 32767f;
    private const float INT_TO_FLOAT = 3.051851E-05f;
    private const ulong CURVEPOS_INCLUDE = 2147418112;
    private const ulong CURVEPOS_EXCLUDE = 18446744071562133503;
    private const ulong SECONDARY_NODE = 2147483648;
    private ulong m_SearchKey;

    public PathNode(Entity owner, byte laneIndex, byte segmentIndex)
    {
      this.m_SearchKey = (ulong) ((long) owner.Index << 32 | (long) segmentIndex << 8) | (ulong) laneIndex;
    }

    public PathNode(Entity owner, byte laneIndex, byte segmentIndex, float curvePosition)
    {
      this.m_SearchKey = (ulong) ((long) owner.Index << 32 | (long) (ulong) ((double) curvePosition * (double) short.MaxValue) << 16 | (long) segmentIndex << 8) | (ulong) laneIndex;
    }

    public PathNode(Entity owner, ushort laneIndex, float curvePosition)
    {
      this.m_SearchKey = (ulong) ((long) owner.Index << 32 | (long) (ulong) ((double) curvePosition * (double) short.MaxValue) << 16) | (ulong) laneIndex;
    }

    public PathNode(Entity owner, ushort laneIndex)
    {
      this.m_SearchKey = (ulong) owner.Index << 32 | (ulong) laneIndex;
    }

    public PathNode(PathTarget pathTarget)
    {
      this.m_SearchKey = (ulong) ((long) pathTarget.m_Entity.Index << 32 | (long) (ulong) ((double) pathTarget.m_Delta * (double) short.MaxValue) << 16);
    }

    public PathNode(PathNode pathNode, float curvePosition)
    {
      this.m_SearchKey = (ulong) ((long) pathNode.m_SearchKey & -2147418113L | (long) (ulong) ((double) curvePosition * (double) short.MaxValue) << 16);
    }

    public PathNode(PathNode pathNode, bool secondaryNode)
    {
      this.m_SearchKey = math.select(pathNode.m_SearchKey & 18446744071562067967UL, pathNode.m_SearchKey | 2147483648UL, secondaryNode);
    }

    public bool IsSecondary() => (this.m_SearchKey & 2147483648UL) > 0UL;

    public bool Equals(PathNode other) => (long) this.m_SearchKey == (long) other.m_SearchKey;

    public bool EqualsIgnoreCurvePos(PathNode other)
    {
      return (((long) this.m_SearchKey ^ (long) other.m_SearchKey) & -2147418113L) == 0L;
    }

    public bool OwnerEquals(PathNode other)
    {
      return (int) (this.m_SearchKey >> 32) == (int) (other.m_SearchKey >> 32);
    }

    public override int GetHashCode() => this.m_SearchKey.GetHashCode();

    public PathNode StripCurvePos()
    {
      return new PathNode()
      {
        m_SearchKey = this.m_SearchKey & 18446744071562133503UL
      };
    }

    public void ReplaceOwner(Entity oldOwner, Entity newOwner)
    {
      if ((int) (this.m_SearchKey >> 32) != oldOwner.Index)
        return;
      this.m_SearchKey = (ulong) ((long) newOwner.Index << 32 | (long) this.m_SearchKey & (long) uint.MaxValue);
    }

    public float GetCurvePos() => (float) ((this.m_SearchKey & 2147418112UL) >> 16) * 3.051851E-05f;

    public ushort GetLaneIndex() => (ushort) (this.m_SearchKey & (ulong) ushort.MaxValue);

    public bool GetOrder(PathNode other) => other.m_SearchKey < this.m_SearchKey;

    public int GetCurvePosOrder(PathNode other)
    {
      return (int) ((long) this.m_SearchKey & 2147418112L) - (int) ((long) other.m_SearchKey & 2147418112L);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(new Entity()
      {
        Index = (int) (this.m_SearchKey >> 32)
      }, true);
      writer.Write((uint) this.m_SearchKey);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      Entity entity;
      reader.Read(out entity);
      uint num;
      reader.Read(out num);
      this.m_SearchKey = (ulong) entity.Index << 32 | (ulong) num;
    }

    public int GetStride(Context context) => 8;
  }
}
