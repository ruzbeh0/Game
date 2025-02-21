// Decompiled with JetBrains decompiler
// Type: Game.Tools.ControlPoint
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct ControlPoint : IEquatable<ControlPoint>
  {
    public float3 m_Position;
    public float3 m_HitPosition;
    public float2 m_Direction;
    public float3 m_HitDirection;
    public quaternion m_Rotation;
    public Entity m_OriginalEntity;
    public float2 m_SnapPriority;
    public int2 m_ElementIndex;
    public float m_CurvePosition;
    public float m_Elevation;

    public ControlPoint(Entity raycastEntity, RaycastHit raycastHit)
    {
      this.m_Position = raycastHit.m_Position;
      this.m_HitPosition = raycastHit.m_HitPosition;
      this.m_Direction = new float2();
      this.m_HitDirection = raycastHit.m_HitDirection;
      this.m_Rotation = quaternion.identity;
      this.m_OriginalEntity = raycastEntity;
      this.m_SnapPriority = new float2();
      this.m_ElementIndex = raycastHit.m_CellIndex;
      this.m_CurvePosition = raycastHit.m_CurvePosition;
      this.m_Elevation = 0.0f;
    }

    public bool Equals(ControlPoint other)
    {
      return this.m_Position.Equals(other.m_Position) && this.m_HitPosition.Equals(other.m_HitPosition) && this.m_Direction.Equals(other.m_Direction) && this.m_HitDirection.Equals(other.m_HitDirection) && this.m_Rotation.Equals(other.m_Rotation) && this.m_OriginalEntity.Equals(other.m_OriginalEntity) && this.m_SnapPriority.Equals(other.m_SnapPriority) && this.m_ElementIndex.Equals(other.m_ElementIndex) && (double) this.m_CurvePosition == (double) other.m_CurvePosition && (double) this.m_Elevation == (double) other.m_Elevation;
    }

    public bool EqualsIgnoreHit(ControlPoint other)
    {
      return math.all(math.abs(this.m_Position - other.m_Position) < 1f / 1000f) && math.all(math.abs(this.m_Direction - other.m_Direction) < 1f / 1000f) && math.all(math.abs(this.m_Rotation.value - other.m_Rotation.value) < 1f / 1000f) && (double) math.abs(this.m_CurvePosition - other.m_CurvePosition) < 1.0 / 1000.0 && (double) math.abs(this.m_Elevation - other.m_Elevation) < 1.0 / 1000.0 && this.m_OriginalEntity.Equals(other.m_OriginalEntity) && this.m_ElementIndex.Equals(other.m_ElementIndex);
    }

    public override int GetHashCode()
    {
      return (((((((((17 * 31 + this.m_Position.GetHashCode()) * 31 + this.m_HitPosition.GetHashCode()) * 31 + this.m_Direction.GetHashCode()) * 31 + this.m_HitDirection.GetHashCode()) * 31 + this.m_Rotation.GetHashCode()) * 31 + this.m_OriginalEntity.GetHashCode()) * 31 + this.m_SnapPriority.GetHashCode()) * 31 + this.m_ElementIndex.GetHashCode()) * 31 + this.m_CurvePosition.GetHashCode()) * 31 + this.m_Elevation.GetHashCode();
    }
  }
}
