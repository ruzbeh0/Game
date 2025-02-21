// Decompiled with JetBrains decompiler
// Type: Game.Common.RaycastHit
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Common
{
  public struct RaycastHit : IEquatable<RaycastHit>
  {
    public Entity m_HitEntity;
    public float3 m_Position;
    public float3 m_HitPosition;
    public float3 m_HitDirection;
    public int2 m_CellIndex;
    public float m_NormalizedDistance;
    public float m_CurvePosition;

    public bool Equals(RaycastHit other)
    {
      return this.m_HitEntity.Equals(other.m_HitEntity) && this.m_Position.Equals(other.m_Position) && this.m_HitPosition.Equals(other.m_HitPosition) && this.m_HitDirection.Equals(other.m_HitDirection) && this.m_CellIndex.Equals(other.m_CellIndex) && (double) this.m_NormalizedDistance == (double) other.m_NormalizedDistance && (double) this.m_CurvePosition == (double) other.m_CurvePosition;
    }

    public override int GetHashCode()
    {
      return ((((((17 * 31 + this.m_HitEntity.GetHashCode()) * 31 + this.m_Position.GetHashCode()) * 31 + this.m_HitPosition.GetHashCode()) * 31 + this.m_HitDirection.GetHashCode()) * 31 + this.m_CellIndex.GetHashCode()) * 31 + this.m_NormalizedDistance.GetHashCode()) * 31 + this.m_CurvePosition.GetHashCode();
    }
  }
}
