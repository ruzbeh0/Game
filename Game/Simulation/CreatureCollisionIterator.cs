// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CreatureCollisionIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct CreatureCollisionIterator : 
    INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
    IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
  {
    public ComponentLookup<Owner> m_OwnerData;
    public ComponentLookup<Transform> m_TransformData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Creature> m_CreatureData;
    public ComponentLookup<GroupMember> m_GroupMemberData;
    public ComponentLookup<Waypoint> m_WaypointData;
    public ComponentLookup<TaxiStand> m_TaxiStandData;
    public ComponentLookup<Curve> m_CurveData;
    public ComponentLookup<AreaLane> m_AreaLaneData;
    public ComponentLookup<PrefabRef> m_PrefabRefData;
    public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
    public ComponentLookup<NetLaneData> m_PrefabLaneData;
    public BufferLookup<LaneObject> m_LaneObjects;
    public BufferLookup<Game.Areas.Node> m_AreaNodes;
    public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
    public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_MovingObjectSearchTree;
    public Entity m_Entity;
    public Entity m_Leader;
    public Entity m_CurrentLane;
    public Entity m_CurrentVehicle;
    public float m_CurvePosition;
    public float m_TimeStep;
    public ObjectGeometryData m_PrefabObjectGeometry;
    public Bounds1 m_SpeedRange;
    public float3 m_CurrentPosition;
    public float3 m_CurrentDirection;
    public float3 m_CurrentVelocity;
    public float m_TargetDistance;
    public PathOwner m_PathOwner;
    public DynamicBuffer<PathElement> m_PathElements;
    public float m_MinSpeed;
    public float3 m_TargetPosition;
    public float m_MaxSpeed;
    public float m_LanePosition;
    public Entity m_Blocker;
    public BlockerType m_BlockerType;
    public Entity m_QueueEntity;
    public Sphere3 m_QueueArea;
    public DynamicBuffer<Queue> m_Queues;
    private Line3.Segment m_TargetLine;
    private float m_PushFactor;
    private Bounds3 m_Bounds;
    private float m_Size;

    public bool IterateFirstLane(Entity currentLane, float2 currentOffset, bool isBackward)
    {
      return this.IterateFirstLane(currentLane, currentLane, currentOffset, currentOffset, isBackward);
    }

    public bool IterateFirstLane(
      Entity currentLane,
      Entity targetLane,
      float2 currentOffset,
      float2 targetOffset,
      bool isBackward)
    {
      this.m_Size = (float) (((double) this.m_PrefabObjectGeometry.m_Bounds.max.x - (double) this.m_PrefabObjectGeometry.m_Bounds.min.x) * 0.5);
      this.m_PushFactor = 0.75f;
      if (this.m_AreaLaneData.HasComponent(targetLane))
      {
        this.CalculateTargetLine(targetLane, this.m_TargetPosition, isBackward);
        this.m_MovingObjectSearchTree.Iterate<CreatureCollisionIterator>(ref this);
        this.m_StaticObjectSearchTree.Iterate<CreatureCollisionIterator>(ref this);
        return false;
      }
      Curve componentData;
      if (!this.m_CurveData.TryGetComponent(targetLane, out componentData))
        return false;
      this.CalculateTargetLine(targetLane, targetOffset.x);
      bool flag = false;
      DynamicBuffer<LaneObject> bufferData;
      if (this.m_LaneObjects.TryGetBuffer(currentLane, out bufferData))
      {
        float num = this.m_TargetDistance / math.max(1f, componentData.m_Length);
        Bounds1 bounds1 = new Bounds1(currentOffset.x - num, currentOffset.x + num);
        flag = MathUtils.Intersect(bounds1, currentOffset.y);
        for (int index = 0; index < bufferData.Length; ++index)
        {
          LaneObject laneObject = bufferData[index];
          Bounds1 bounds2 = MathUtils.Bounds(laneObject.m_CurvePosition.x, laneObject.m_CurvePosition.y);
          if (MathUtils.Intersect(bounds1, bounds2) && laneObject.m_LaneObject != this.m_Entity)
            this.CheckCollision(laneObject.m_LaneObject);
        }
      }
      this.m_StaticObjectSearchTree.Iterate<CreatureCollisionIterator>(ref this);
      return flag;
    }

    public bool IterateNextLane(Entity nextLane, float2 nextOffset)
    {
      bool flag = false;
      DynamicBuffer<LaneObject> bufferData;
      if (this.m_LaneObjects.TryGetBuffer(nextLane, out bufferData))
      {
        float num = 5f / math.max(1f, this.m_CurveData[nextLane].m_Length);
        Bounds1 bounds1 = new Bounds1(nextOffset.x - num, nextOffset.x + num);
        flag = MathUtils.Intersect(bounds1, nextOffset.y);
        for (int index = 0; index < bufferData.Length; ++index)
        {
          LaneObject laneObject = bufferData[index];
          Bounds1 bounds2 = MathUtils.Bounds(laneObject.m_CurvePosition.x, laneObject.m_CurvePosition.y);
          if (MathUtils.Intersect(bounds1, bounds2) && laneObject.m_LaneObject != this.m_Entity)
            this.CheckCollision(laneObject.m_LaneObject);
        }
      }
      else if (this.m_AreaLaneData.HasComponent(nextLane))
        this.m_MovingObjectSearchTree.Iterate<CreatureCollisionIterator>(ref this);
      return flag;
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

    private void CalculateTargetLine(Entity targetLane, float3 targetPosition, bool isBackward)
    {
      Owner owner = this.m_OwnerData[targetLane];
      AreaLane areaLane = this.m_AreaLaneData[targetLane];
      DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[owner.m_Owner];
      float3 float3 = targetPosition - this.m_CurrentPosition;
      float y = math.length(float3.xz);
      if ((double) y < (double) this.m_TargetDistance)
      {
        this.m_TargetLine = new Line3.Segment(targetPosition, targetPosition);
      }
      else
      {
        if ((double) y > (double) this.m_TargetDistance)
          targetPosition = this.m_CurrentPosition + float3 * (this.m_TargetDistance / y);
        float2 float2 = math.select(MathUtils.Right(float3.xz), MathUtils.Left(float3.xz), isBackward) * (0.5f / math.max(0.1f, y));
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
        bounds1.min = math.clamp(bounds1.min + this.m_Size, this.m_TargetDistance * -0.9f, 0.0f);
        bounds1.max = math.clamp(bounds1.max - this.m_Size, 0.0f, this.m_TargetDistance * 0.9f);
        this.m_TargetLine.a = MathUtils.Position(line, bounds1.min);
        this.m_TargetLine.b = MathUtils.Position(line, bounds1.max);
      }
      this.m_Bounds = MathUtils.Expand(MathUtils.Bounds(this.m_TargetLine) | this.m_CurrentPosition, (float3) this.m_Size);
    }

    private void CalculateTargetLine(Entity targetLane, float targetOffset)
    {
      Curve curve = this.m_CurveData[targetLane];
      float num1 = math.max(0.0f, this.m_PrefabLaneData[this.m_PrefabRefData[targetLane].m_Prefab].m_Width * 0.5f - this.m_Size);
      float3 float3 = MathUtils.Position(curve.m_Bezier, targetOffset);
      float2 float2 = MathUtils.Right(math.normalizesafe(MathUtils.Tangent(curve.m_Bezier, targetOffset).xz)) * num1;
      this.m_TargetLine = new Line3.Segment(float3, float3);
      this.m_TargetLine.a.xz -= float2;
      this.m_TargetLine.b.xz += float2;
      float t;
      float num2 = MathUtils.Distance(this.m_TargetLine, this.m_CurrentPosition, out t);
      if ((double) num2 > (double) this.m_TargetDistance)
        this.m_TargetLine += (this.m_CurrentPosition - MathUtils.Position(this.m_TargetLine, t)) * (float) (1.0 - (double) this.m_TargetDistance / (double) num2);
      this.m_Bounds = MathUtils.Expand(MathUtils.Bounds(this.m_TargetLine) | this.m_CurrentPosition, (float3) this.m_Size);
    }

    private void CheckCollision(Entity other)
    {
      Transform componentData1;
      if (!this.m_TransformData.TryGetComponent(other, out componentData1))
        return;
      ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[other].m_Prefab];
      if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.WalkThrough) != Game.Objects.GeometryFlags.None)
        return;
      Moving componentData2;
      if (this.m_MovingData.TryGetComponent(other, out componentData2))
      {
        float num1 = (float) ((double) this.m_Size + ((double) objectGeometryData.m_Bounds.max.x - (double) objectGeometryData.m_Bounds.min.x) * 0.5 + 0.5);
        Line3.Segment segment1 = new Line3.Segment(this.m_CurrentPosition, this.m_TargetPosition);
        Line3.Segment segment2 = new Line3.Segment(componentData1.m_Position, componentData1.m_Position + componentData2.m_Velocity);
        if ((double) math.dot(segment2.a + componentData2.m_Velocity * (this.m_TimeStep * 2f) - segment1.a - this.m_CurrentVelocity * this.m_TimeStep, this.m_TargetPosition - this.m_CurrentPosition) < 0.0)
          return;
        float2 t;
        float num2 = MathUtils.Distance(segment1, segment2, out t);
        if ((double) num2 >= (double) num1)
          return;
        float3 float3_1 = MathUtils.Position(segment1, t.x * 0.99f);
        float3 float3_2 = MathUtils.Position(segment2, t.y);
        float3 float3_3 = math.normalizesafe(this.m_TargetPosition - this.m_CurrentPosition);
        float3 float3_4 = float3_2;
        float3 x = float3_1 - float3_4;
        float3 position = this.m_TargetPosition + math.normalizesafe(x - float3_3 * math.dot(x, float3_3)) * ((num1 - num2) * this.m_PushFactor);
        this.m_PushFactor /= 2f;
        if (this.m_TargetLine.a.Equals(this.m_TargetLine.b))
        {
          this.m_TargetPosition = this.m_TargetLine.a;
        }
        else
        {
          double num3 = (double) MathUtils.Distance(this.m_TargetLine, position, out this.m_LanePosition);
          this.m_TargetPosition = MathUtils.Position(this.m_TargetLine, this.m_LanePosition);
          this.m_LanePosition -= 0.5f;
        }
        float num4 = math.min(1f, (float) (0.699999988079071 + 0.30000001192092896 * (double) math.dot(float3_3, math.normalizesafe(componentData2.m_Velocity)) + (double) num2 / (double) num1)) * this.m_SpeedRange.max;
        float3 float3_5 = componentData1.m_Position - this.m_CurrentPosition;
        float num5 = math.length(float3_5);
        float num6 = math.dot(float3_5, float3_3);
        Entity queueEntity = Entity.Null;
        Sphere3 queueArea = new Sphere3();
        BlockerType blockerType = BlockerType.Crossing;
        if ((double) num5 < (double) num1 && (double) num6 > 0.0)
        {
          blockerType = BlockerType.Continuing;
          if (this.CheckQueue(other, out queueEntity, out queueArea))
          {
            if ((double) num5 > 0.0099999997764825821)
            {
              float num7 = (float) ((double) num6 * ((double) num1 - (double) num5) / ((double) num1 * (double) num5));
              num4 = math.min(num4, math.max(0.0f, math.max(1f, math.lerp(math.dot(float3_3, componentData2.m_Velocity), this.m_SpeedRange.max, num2 / num1)) - num7));
            }
            else
              num4 = 0.0f;
          }
          else if ((double) num5 > 0.0099999997764825821 && ((objectGeometryData.m_Flags & ~this.m_PrefabObjectGeometry.m_Flags & Game.Objects.GeometryFlags.LowCollisionPriority) == Game.Objects.GeometryFlags.None || (double) math.dot(componentData2.m_Velocity, float3_5) < 0.0))
          {
            float num8 = (float) ((double) num6 * ((double) num1 - (double) num5) / ((double) num1 * (double) num5));
            num4 = math.min(num4, math.max(this.m_MinSpeed, math.max(1f, math.lerp(math.dot(float3_3, componentData2.m_Velocity), this.m_SpeedRange.max, num2 / num1)) - num8));
          }
        }
        float num9 = MathUtils.Clamp(num4, this.m_SpeedRange);
        if ((double) num9 >= (double) this.m_MaxSpeed)
          return;
        this.m_MaxSpeed = num9;
        this.m_Blocker = other;
        this.m_BlockerType = blockerType;
        CreatureUtils.SetQueue(ref this.m_QueueEntity, ref this.m_QueueArea, queueEntity, queueArea);
      }
      else
      {
        float num10 = (float) ((double) this.m_Size + ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) == Game.Objects.GeometryFlags.None ? (double) (math.cmax(objectGeometryData.m_Bounds.max.xz - objectGeometryData.m_Bounds.min.xz) * 0.5f) : (double) math.cmax(objectGeometryData.m_LegSize.xz)) + 0.25);
        Line3.Segment line = new Line3.Segment(this.m_CurrentPosition, this.m_TargetPosition);
        float t;
        float num11 = MathUtils.Distance(line, componentData1.m_Position, out t);
        if ((double) num11 >= (double) num10)
          return;
        float3 float3 = MathUtils.Position(line, t * 0.99f);
        float3 y = math.normalizesafe(this.m_TargetPosition - this.m_CurrentPosition);
        float3 position1 = componentData1.m_Position;
        float3 x1 = float3 - position1;
        float3 position2 = this.m_TargetPosition + math.normalizesafe(x1 - y * math.dot(x1, y)) * ((num10 - num11) * this.m_PushFactor);
        this.m_PushFactor /= 2f;
        if (this.m_TargetLine.a.Equals(this.m_TargetLine.b))
        {
          this.m_TargetPosition = this.m_TargetLine.a;
        }
        else
        {
          double num12 = (double) MathUtils.Distance(this.m_TargetLine, position2, out this.m_LanePosition);
          this.m_TargetPosition = MathUtils.Position(this.m_TargetLine, this.m_LanePosition);
          this.m_LanePosition -= 0.5f;
        }
        float num13 = math.min(1f, (float) (0.699999988079071 + (double) num11 / (double) num10)) * this.m_SpeedRange.max;
        float3 x2 = componentData1.m_Position - this.m_CurrentPosition;
        float num14 = math.length(x2);
        float num15 = math.dot(x2, y);
        if ((double) num14 < (double) num10 && (double) num15 > 0.0 && (double) num14 > 0.0099999997764825821)
        {
          float num16 = (float) ((double) num15 * ((double) num10 - (double) num14) / ((double) num10 * (double) num14));
          num13 = math.min(num13, math.max(0.5f, math.max(1f, this.m_SpeedRange.max * num11 / num10) - num16));
        }
        float num17 = MathUtils.Clamp(num13, this.m_SpeedRange);
        if ((double) num17 >= (double) this.m_MaxSpeed)
          return;
        this.m_MaxSpeed = num17;
        this.m_Blocker = other;
        this.m_BlockerType = BlockerType.Limit;
        this.m_QueueEntity = Entity.Null;
        this.m_QueueArea = new Sphere3();
      }
    }

    private bool CheckQueue(Entity other, out Entity queueEntity, out Sphere3 queueArea)
    {
      queueEntity = Entity.Null;
      queueArea = new Sphere3();
      Creature componentData1;
      if (this.m_CreatureData.TryGetComponent(other, out componentData1) && (double) componentData1.m_QueueArea.radius > 0.0)
      {
        Transform transform = this.m_TransformData[other];
        float3 y = math.forward(transform.m_Rotation);
        if ((double) math.dot(transform.m_Position - this.m_CurrentPosition, this.m_CurrentDirection) < (double) math.dot(this.m_CurrentPosition - transform.m_Position, y))
          return false;
        if (this.m_Leader != Entity.Null)
        {
          GroupMember componentData2;
          if (this.m_GroupMemberData.TryGetComponent(other, out componentData2))
            other = componentData2.m_Leader;
          if (other == this.m_Leader)
          {
            queueEntity = componentData1.m_QueueEntity;
            queueArea = componentData1.m_QueueArea;
            return true;
          }
        }
        else
        {
          GroupMember componentData3;
          if (this.m_GroupMemberData.TryGetComponent(other, out componentData3))
            other = componentData3.m_Leader;
          if (other != this.m_Entity && this.ShouldQueue(componentData1.m_QueueEntity, componentData1.m_QueueArea, out queueArea))
          {
            queueEntity = componentData1.m_QueueEntity;
            return true;
          }
        }
      }
      return false;
    }

    private bool ShouldQueue(Entity entity, Sphere3 area, out Sphere3 queueArea)
    {
      if (!this.m_Queues.IsCreated || (this.m_PathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete | PathFlags.Updated)) != (PathFlags) 0)
      {
        queueArea = new Sphere3();
        return false;
      }
      Entity target = Entity.Null;
      if (this.m_PathElements.Length > this.m_PathOwner.m_ElementIndex)
      {
        PathElement pathElement = this.m_PathElements[this.m_PathOwner.m_ElementIndex];
        if (this.m_WaypointData.HasComponent(pathElement.m_Target) || this.m_TaxiStandData.HasComponent(pathElement.m_Target))
          target = pathElement.m_Target;
      }
      for (int index = 0; index < this.m_Queues.Length; ++index)
      {
        Queue queue = this.m_Queues[index];
        if (queue.m_TargetEntity == entity)
        {
          Curve componentData;
          if ((queue.m_TargetEntity == target || queue.m_TargetEntity == this.m_CurrentLane) && this.m_CurveData.TryGetComponent(this.m_CurrentLane, out componentData))
          {
            float laneOffset = CreatureUtils.GetLaneOffset(this.m_PrefabObjectGeometry, this.m_PrefabLaneData[this.m_PrefabRefData[this.m_CurrentLane].m_Prefab], this.m_LanePosition);
            queue.m_TargetArea.position = CreatureUtils.GetLanePosition(componentData.m_Bezier, this.m_CurvePosition, laneOffset);
          }
          queue.m_ObsoleteTime = (ushort) 0;
          this.m_Queues[index] = queue;
          if ((double) queue.m_TargetArea.radius > 0.0 && MathUtils.Intersect(queue.m_TargetArea, area))
          {
            Sphere3 queueArea1 = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, this.m_CurrentPosition, this.m_TargetPosition);
            queueArea = MathUtils.Sphere(area, MathUtils.Sphere(queueArea1, queue.m_TargetArea));
            return true;
          }
          queueArea = new Sphere3();
          return false;
        }
      }
      if (this.m_CurrentLane == entity)
      {
        Queue elem;
        elem.m_TargetEntity = entity;
        elem.m_TargetArea = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, this.GetTargetPosition(this.m_PathOwner.m_ElementIndex - 1, this.m_CurrentLane, this.m_CurvePosition));
        elem.m_ObsoleteTime = (ushort) 0;
        this.m_Queues.Add(elem);
        if (MathUtils.Intersect(elem.m_TargetArea, area))
        {
          Sphere3 queueArea2 = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, this.m_CurrentPosition, this.m_TargetPosition);
          queueArea = MathUtils.Sphere(area, MathUtils.Sphere(queueArea2, elem.m_TargetArea));
          return true;
        }
        queueArea = new Sphere3();
        return false;
      }
      if (this.m_CurrentVehicle == Entity.Null)
      {
        for (int elementIndex = this.m_PathOwner.m_ElementIndex; elementIndex < this.m_PathElements.Length; ++elementIndex)
        {
          PathElement pathElement = this.m_PathElements[elementIndex];
          if (pathElement.m_Target == entity)
          {
            Queue elem;
            elem.m_TargetEntity = entity;
            elem.m_TargetArea = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, this.GetTargetPosition(elementIndex, pathElement.m_Target, pathElement.m_TargetDelta.y));
            elem.m_ObsoleteTime = (ushort) 0;
            this.m_Queues.Add(elem);
            if (MathUtils.Intersect(elem.m_TargetArea, area))
            {
              Sphere3 queueArea3 = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, this.m_CurrentPosition, this.m_TargetPosition);
              queueArea = MathUtils.Sphere(area, MathUtils.Sphere(queueArea3, elem.m_TargetArea));
              return true;
            }
            queueArea = new Sphere3();
            return false;
          }
        }
      }
      this.m_Queues.Add(new Queue()
      {
        m_TargetEntity = entity
      });
      queueArea = new Sphere3();
      return false;
    }

    private float3 GetTargetPosition(int elementIndex, Entity targetElement, float curvePos)
    {
      while (this.m_WaypointData.HasComponent(targetElement) || this.m_TaxiStandData.HasComponent(targetElement))
      {
        if (--elementIndex >= this.m_PathOwner.m_ElementIndex)
        {
          PathElement pathElement = this.m_PathElements[elementIndex];
          targetElement = pathElement.m_Target;
          curvePos = pathElement.m_TargetDelta.y;
        }
        else
        {
          targetElement = this.m_CurrentLane;
          curvePos = this.m_CurvePosition;
          break;
        }
      }
      Curve componentData1;
      if (this.m_CurveData.TryGetComponent(targetElement, out componentData1))
      {
        float laneOffset = CreatureUtils.GetLaneOffset(this.m_PrefabObjectGeometry, this.m_PrefabLaneData[this.m_PrefabRefData[targetElement].m_Prefab], this.m_LanePosition);
        return CreatureUtils.GetLanePosition(componentData1.m_Bezier, curvePos, laneOffset);
      }
      Transform componentData2;
      return this.m_TransformData.TryGetComponent(targetElement, out componentData2) ? componentData2.m_Position : this.m_TargetPosition;
    }

    public void IterateBlocker(HumanData prefabHumanData, Entity other)
    {
      Entity queueEntity;
      Sphere3 queueArea;
      Moving componentData;
      if (!this.CheckQueue(other, out queueEntity, out queueArea) || !this.m_MovingData.TryGetComponent(other, out componentData))
        return;
      Transform transform = this.m_TransformData[other];
      ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[other].m_Prefab];
      float num1 = (float) (((double) this.m_PrefabObjectGeometry.m_Bounds.max.x - (double) this.m_PrefabObjectGeometry.m_Bounds.min.x) * 0.5 + ((double) objectGeometryData.m_Bounds.max.x - (double) objectGeometryData.m_Bounds.min.x) * 0.5 + 0.5);
      float3 x = transform.m_Position - this.m_CurrentPosition;
      float3 float3 = math.normalizesafe(this.m_TargetPosition - this.m_CurrentPosition);
      float distance = math.max(0.0f, math.length(x) * 2f - num1 - math.dot(x, float3));
      float maxResultSpeed = math.max(0.0f, math.dot(float3, componentData.m_Velocity));
      float num2 = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabHumanData, distance, maxResultSpeed, this.m_TimeStep), this.m_SpeedRange);
      if ((double) num2 > (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = num2;
      this.m_Blocker = other;
      this.m_BlockerType = BlockerType.Continuing;
      CreatureUtils.SetQueue(ref this.m_QueueEntity, ref this.m_QueueArea, queueEntity, queueArea);
    }

    public void IterateBlocker(AnimalData prefabAnimalData, Entity other)
    {
      Entity queueEntity;
      Sphere3 queueArea;
      Moving componentData;
      if (!this.CheckQueue(other, out queueEntity, out queueArea) || !this.m_MovingData.TryGetComponent(other, out componentData))
        return;
      Transform transform = this.m_TransformData[other];
      ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[this.m_PrefabRefData[other].m_Prefab];
      float num1 = (float) (((double) this.m_PrefabObjectGeometry.m_Bounds.max.x - (double) this.m_PrefabObjectGeometry.m_Bounds.min.x) * 0.5 + ((double) objectGeometryData.m_Bounds.max.x - (double) objectGeometryData.m_Bounds.min.x) * 0.5 + 0.5);
      float3 x = transform.m_Position - this.m_CurrentPosition;
      float3 float3 = math.normalizesafe(this.m_TargetPosition - this.m_CurrentPosition);
      float distance = math.max(0.0f, math.length(x) * 2f - num1 - math.dot(x, float3));
      float maxResultSpeed = math.max(0.0f, math.dot(float3, componentData.m_Velocity));
      float num2 = MathUtils.Clamp(CreatureUtils.GetMaxBrakingSpeed(prefabAnimalData, distance, maxResultSpeed, this.m_TimeStep), this.m_SpeedRange);
      if ((double) num2 > (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = num2;
      this.m_Blocker = other;
      this.m_BlockerType = BlockerType.Continuing;
      CreatureUtils.SetQueue(ref this.m_QueueEntity, ref this.m_QueueArea, queueEntity, queueArea);
    }
  }
}
