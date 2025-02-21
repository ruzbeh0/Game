// Decompiled with JetBrains decompiler
// Type: Game.Simulation.VehicleCollisionIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct VehicleCollisionIterator : 
    INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
    IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
  {
    public ComponentLookup<Owner> m_OwnerData;
    public ComponentLookup<Transform> m_TransformData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Controller> m_ControllerData;
    public ComponentLookup<Creature> m_CreatureData;
    public ComponentLookup<Curve> m_CurveData;
    public ComponentLookup<AreaLane> m_AreaLaneData;
    public ComponentLookup<PrefabRef> m_PrefabRefData;
    public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
    public ComponentLookup<NetLaneData> m_PrefabLaneData;
    public BufferLookup<Game.Areas.Node> m_AreaNodes;
    public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
    public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
    public TerrainHeightData m_TerrainHeightData;
    public Entity m_Entity;
    public Entity m_CurrentLane;
    public float m_CurvePosition;
    public float m_TimeStep;
    public ObjectGeometryData m_PrefabObjectGeometry;
    public Bounds1 m_SpeedRange;
    public float3 m_CurrentPosition;
    public float3 m_CurrentVelocity;
    public float m_MinDistance;
    public float3 m_TargetPosition;
    public float m_MaxSpeed;
    public float m_LanePosition;
    public Entity m_Blocker;
    public BlockerType m_BlockerType;
    private Line3.Segment m_TargetLine;
    private Bounds1 m_TargetLimits;
    private float m_PushFactor;
    private Bounds3 m_Bounds;
    private float m_Size;

    public bool IterateFirstLane(Entity currentLane)
    {
      this.m_Size = (float) (((double) this.m_PrefabObjectGeometry.m_Bounds.max.x - (double) this.m_PrefabObjectGeometry.m_Bounds.min.x) * 0.5);
      this.m_PushFactor = 0.75f;
      if (!this.m_AreaLaneData.HasComponent(currentLane))
        return false;
      this.CalculateTargetLine(currentLane, this.m_TargetPosition);
      this.m_MovingObjectSearchTree.Iterate<VehicleCollisionIterator>(ref this);
      this.m_StaticObjectSearchTree.Iterate<VehicleCollisionIterator>(ref this);
      return false;
    }

    public bool Intersect(QuadTreeBoundsXZ bounds)
    {
      return (bounds.m_Mask & (BoundsMask.NotOverridden | BoundsMask.NotWalkThrough)) == (BoundsMask.NotOverridden | BoundsMask.NotWalkThrough) && MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds);
    }

    public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
    {
      if ((bounds.m_Mask & (BoundsMask.NotOverridden | BoundsMask.NotWalkThrough)) != (BoundsMask.NotOverridden | BoundsMask.NotWalkThrough) || !MathUtils.Intersect(this.m_Bounds, bounds.m_Bounds))
        return;
      this.CheckCollision(item);
    }

    private void CalculateTargetLine(Entity targetLane, float3 targetPosition)
    {
      Owner owner = this.m_OwnerData[targetLane];
      AreaLane areaLane = this.m_AreaLaneData[targetLane];
      DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[owner.m_Owner];
      float3 float3 = targetPosition - this.m_CurrentPosition;
      float y = math.length(float3.xz);
      if ((double) y < (double) this.m_MinDistance)
      {
        this.m_TargetLine = new Line3.Segment(targetPosition, targetPosition);
        this.m_TargetLimits = new Bounds1(0.0f, 1f);
      }
      else
      {
        if ((double) y > (double) this.m_MinDistance)
          targetPosition = this.m_CurrentPosition + float3 * (this.m_MinDistance / y);
        float2 float2 = MathUtils.Right(float3.xz) * (0.5f / math.max(0.1f, y));
        Line3 line = (Line3) new Line3.Segment(targetPosition, targetPosition);
        line.a.xz -= float2;
        line.b.xz += float2;
        Bounds1 bounds1 = new Bounds1();
        if (areaLane.m_Nodes.y == areaLane.m_Nodes.z)
        {
          float3 position1 = areaNode[areaLane.m_Nodes.x].m_Position;
          float3 position2 = areaNode[areaLane.m_Nodes.y].m_Position;
          float3 position3 = areaNode[areaLane.m_Nodes.w].m_Position;
          float2 t;
          if (MathUtils.Intersect(new Line2.Segment(position1.xz, position2.xz), line.xz, out t))
            bounds1 |= t.y;
          if (MathUtils.Intersect(new Line2.Segment(position2.xz, position3.xz), line.xz, out t))
            bounds1 |= t.y;
          if (MathUtils.Intersect(new Line2.Segment(position3.xz, position1.xz), line.xz, out t))
            bounds1 |= t.y;
        }
        else
        {
          float3 position4 = areaNode[areaLane.m_Nodes.x].m_Position;
          float3 position5 = areaNode[areaLane.m_Nodes.y].m_Position;
          float3 position6 = areaNode[areaLane.m_Nodes.w].m_Position;
          float3 position7 = areaNode[areaLane.m_Nodes.z].m_Position;
          float2 t;
          if (MathUtils.Intersect(new Line2.Segment(position4.xz, position5.xz), line.xz, out t))
            bounds1 |= t.y;
          if (MathUtils.Intersect(new Line2.Segment(position5.xz, position6.xz), line.xz, out t))
            bounds1 |= t.y;
          if (MathUtils.Intersect(new Line2.Segment(position6.xz, position7.xz), line.xz, out t))
            bounds1 |= t.y;
          if (MathUtils.Intersect(new Line2.Segment(position7.xz, position4.xz), line.xz, out t))
            bounds1 |= t.y;
        }
        this.m_TargetLimits.min = math.min(bounds1.min + this.m_Size, 0.0f);
        this.m_TargetLimits.max = math.max(bounds1.max - this.m_Size, 0.0f);
        bounds1.min = math.max(this.m_TargetLimits.min, this.m_MinDistance * -0.9f);
        bounds1.max = math.min(this.m_TargetLimits.max, this.m_MinDistance * 0.9f);
        this.m_TargetLine.a = MathUtils.Position(line, bounds1.min);
        this.m_TargetLine.b = MathUtils.Position(line, bounds1.max);
        float num = 1f / math.max(1f, this.m_TargetLimits.max - this.m_TargetLimits.min);
        this.m_TargetLimits.min = (bounds1.min - this.m_TargetLimits.min) * num;
        this.m_TargetLimits.max = (bounds1.max - this.m_TargetLimits.min) * num;
      }
      this.m_Bounds = MathUtils.Expand(MathUtils.Bounds(this.m_TargetLine) | this.m_CurrentPosition, (float3) this.m_Size);
    }

    private void CheckCollision(Entity other)
    {
      Transform componentData1;
      if (other == this.m_Entity || !this.m_TransformData.TryGetComponent(other, out componentData1))
        return;
      ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[other].m_Prefab];
      if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.WalkThrough) != Game.Objects.GeometryFlags.None)
        return;
      Moving componentData2;
      if (this.m_MovingData.TryGetComponent(other, out componentData2))
      {
        Controller componentData3;
        if (this.m_CreatureData.HasComponent(other) || this.m_ControllerData.TryGetComponent(other, out componentData3) && componentData3.m_Controller == this.m_Entity)
          return;
        float num1 = (float) ((double) this.m_Size + ((double) objectGeometryData.m_Bounds.max.x - (double) objectGeometryData.m_Bounds.min.x) * 0.5 + 0.5);
        Line2.Segment segment1 = new Line2.Segment(this.m_CurrentPosition.xz, this.m_TargetPosition.xz);
        Line2.Segment segment2 = new Line2.Segment(componentData1.m_Position.xz, componentData1.m_Position.xz + componentData2.m_Velocity.xz);
        if ((double) math.dot(segment2.a + componentData2.m_Velocity.xz * (this.m_TimeStep * 2f) - segment1.a - this.m_CurrentVelocity.xz * this.m_TimeStep, this.m_TargetPosition.xz - this.m_CurrentPosition.xz) < 0.0)
          return;
        float2 t;
        float num2 = MathUtils.Distance(segment1, segment2, out t);
        if ((double) num2 >= (double) num1)
          return;
        float2 float2_1 = MathUtils.Position(segment1, t.x * 0.99f);
        float2 float2_2 = MathUtils.Position(segment2, t.y);
        float2 float2_3 = math.normalizesafe(this.m_TargetPosition.xz - this.m_CurrentPosition.xz);
        float2 float2_4 = float2_2;
        float2 x1 = float2_1 - float2_4;
        float2 position = this.m_TargetPosition.xz + math.normalizesafe(x1 - float2_3 * math.dot(x1, float2_3)) * ((num1 - num2) * this.m_PushFactor);
        this.m_PushFactor /= 2f;
        if (this.m_TargetLine.a.Equals(this.m_TargetLine.b))
        {
          this.m_TargetPosition = this.m_TargetLine.a;
        }
        else
        {
          double num3 = (double) MathUtils.Distance(this.m_TargetLine.xz, position, out this.m_LanePosition);
          this.m_TargetPosition = MathUtils.Position(this.m_TargetLine, this.m_LanePosition);
          this.m_LanePosition = (float) ((double) this.m_TargetLimits.min + (double) this.m_LanePosition * ((double) this.m_TargetLimits.max - (double) this.m_TargetLimits.min) - 0.5);
        }
        this.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, this.m_TargetPosition);
        float num4 = math.min(1f, (float) (0.699999988079071 + 0.30000001192092896 * (double) math.dot(float2_3, math.normalizesafe(componentData2.m_Velocity.xz)) + (double) num2 / (double) num1)) * this.m_SpeedRange.max;
        float2 x2 = componentData1.m_Position.xz - this.m_CurrentPosition.xz;
        float num5 = math.length(x2);
        float num6 = math.dot(x2, float2_3);
        BlockerType blockerType = BlockerType.Crossing;
        if ((double) num5 < (double) num1 && (double) num6 > 0.0)
        {
          blockerType = BlockerType.Continuing;
          if ((double) num5 > 0.0099999997764825821)
          {
            float num7 = (float) ((double) num6 * ((double) num1 - (double) num5) / ((double) num1 * (double) num5));
            num4 = math.min(num4, math.max(0.0f, math.max(1f, math.lerp(math.dot(float2_3, componentData2.m_Velocity.xz), this.m_SpeedRange.max, num2 / num1)) - num7));
          }
          else
            num4 = 0.0f;
        }
        float num8 = MathUtils.Clamp(num4, this.m_SpeedRange);
        if ((double) num8 >= (double) this.m_MaxSpeed)
          return;
        this.m_MaxSpeed = num8;
        this.m_Blocker = other;
        this.m_BlockerType = blockerType;
      }
      else
      {
        float num9 = (float) ((double) this.m_Size + ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) == Game.Objects.GeometryFlags.None ? (double) (math.cmax(objectGeometryData.m_Bounds.max.xz - objectGeometryData.m_Bounds.min.xz) * 0.5f) : (double) math.cmax(objectGeometryData.m_LegSize.xz)) + 0.25);
        Line2.Segment line = new Line2.Segment(this.m_CurrentPosition.xz, this.m_TargetPosition.xz);
        float t;
        float num10 = MathUtils.Distance(line, componentData1.m_Position.xz, out t);
        if ((double) num10 >= (double) num9)
          return;
        float2 float2 = MathUtils.Position(line, t * 0.99f);
        float2 y = math.normalizesafe(this.m_TargetPosition.xz - this.m_CurrentPosition.xz);
        float2 xz = componentData1.m_Position.xz;
        float2 x = float2 - xz;
        float2 position = this.m_TargetPosition.xz + math.normalizesafe(x - y * math.dot(x, y)) * ((num9 - num10) * this.m_PushFactor);
        this.m_PushFactor /= 2f;
        if (this.m_TargetLine.a.Equals(this.m_TargetLine.b))
        {
          this.m_TargetPosition = this.m_TargetLine.a;
        }
        else
        {
          double num11 = (double) MathUtils.Distance(this.m_TargetLine.xz, position, out this.m_LanePosition);
          this.m_TargetPosition = MathUtils.Position(this.m_TargetLine, this.m_LanePosition);
          this.m_LanePosition = (float) ((double) this.m_TargetLimits.min + (double) this.m_LanePosition * ((double) this.m_TargetLimits.max - (double) this.m_TargetLimits.min) - 0.5);
        }
        this.m_TargetPosition.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, this.m_TargetPosition);
      }
    }
  }
}
