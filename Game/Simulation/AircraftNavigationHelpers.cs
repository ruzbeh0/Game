// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AircraftNavigationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Vehicles;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class AircraftNavigationHelpers
  {
    public struct LaneReservation : IComparable<AircraftNavigationHelpers.LaneReservation>
    {
      public Entity m_Lane;
      public byte m_Offset;
      public byte m_Priority;

      public LaneReservation(Entity lane, float offset, int priority)
      {
        this.m_Lane = lane;
        this.m_Offset = (byte) math.clamp((int) math.round(offset * (float) byte.MaxValue), 0, (int) byte.MaxValue);
        this.m_Priority = (byte) priority;
      }

      public int CompareTo(AircraftNavigationHelpers.LaneReservation other)
      {
        return this.m_Lane.Index - other.m_Lane.Index;
      }
    }

    public struct LaneEffects
    {
      public Entity m_Lane;
      public float3 m_SideEffects;
      public float m_RelativeSpeed;

      public LaneEffects(Entity lane, float3 sideEffects, float relativeSpeed)
      {
        this.m_Lane = lane;
        this.m_SideEffects = sideEffects;
        this.m_RelativeSpeed = relativeSpeed;
      }
    }

    public struct CurrentLaneCache
    {
      private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      private Entity m_WasCurrentLane;
      private float2 m_WasCurvePosition;
      private bool m_WasFlying;

      public CurrentLaneCache(
        ref AircraftCurrentLane currentLane,
        ComponentLookup<PrefabRef> prefabRefData,
        NativeQuadTree<Entity, QuadTreeBoundsXZ> searchTree)
      {
        if (!prefabRefData.HasComponent(currentLane.m_Lane))
          currentLane.m_Lane = Entity.Null;
        this.m_SearchTree = searchTree;
        this.m_WasCurrentLane = currentLane.m_Lane;
        this.m_WasCurvePosition = currentLane.m_CurvePosition.xy;
        this.m_WasFlying = (currentLane.m_LaneFlags & AircraftLaneFlags.Flying) > (AircraftLaneFlags) 0;
      }

      public void CheckChanges(
        Entity entity,
        ref AircraftCurrentLane currentLane,
        LaneObjectCommandBuffer buffer,
        BufferLookup<LaneObject> laneObjects,
        Transform transform,
        Moving moving,
        AircraftNavigation navigation,
        ObjectGeometryData objectGeometryData)
      {
        bool2 x = new bool2(this.m_WasFlying, (currentLane.m_LaneFlags & AircraftLaneFlags.Flying) > (AircraftLaneFlags) 0);
        if (currentLane.m_Lane != this.m_WasCurrentLane)
        {
          if (laneObjects.HasBuffer(this.m_WasCurrentLane))
            buffer.Remove(this.m_WasCurrentLane, entity);
          else
            x.x = true;
          if (laneObjects.HasBuffer(currentLane.m_Lane))
            buffer.Add(currentLane.m_Lane, entity, currentLane.m_CurvePosition.xy);
          else
            x.y = true;
        }
        else if (laneObjects.HasBuffer(this.m_WasCurrentLane))
        {
          if (!this.m_WasCurvePosition.Equals(currentLane.m_CurvePosition.xy))
            buffer.Update(this.m_WasCurrentLane, entity, currentLane.m_CurvePosition.xy);
        }
        else
          x = (bool2) true;
        if (!math.any(x))
          return;
        if (math.all(x))
        {
          QuadTreeBoundsXZ bounds;
          if (!this.m_SearchTree.TryGet(entity, out bounds))
            return;
          Bounds3 minBounds = this.CalculateMinBounds(transform, moving, navigation, objectGeometryData);
          if (!(math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max)))
            return;
          buffer.Update(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
        }
        else if (x.x)
          buffer.Remove(entity);
        else
          buffer.Add(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
      }

      private Bounds3 CalculateMinBounds(
        Transform transform,
        Moving moving,
        AircraftNavigation navigation,
        ObjectGeometryData objectGeometryData)
      {
        float num = 0.266666681f;
        float3 x = moving.m_Velocity * num;
        float3 y = math.normalizesafe(navigation.m_TargetPosition - transform.m_Position) * (navigation.m_MaxSpeed * num);
        Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, objectGeometryData);
        bounds.min += math.min((float3) 0.0f, math.min(x, y));
        bounds.max += math.max((float3) 0.0f, math.max(x, y));
        return bounds;
      }

      private Bounds3 CalculateMaxBounds(
        Transform transform,
        Moving moving,
        AircraftNavigation navigation,
        ObjectGeometryData objectGeometryData)
      {
        float num1 = -1.06666672f;
        float num2 = 2f;
        float num3 = math.length(objectGeometryData.m_Size) * 0.5f;
        float3 x1 = moving.m_Velocity * num1;
        float3 x2 = moving.m_Velocity * num2;
        float3 y = math.normalizesafe(navigation.m_TargetPosition - transform.m_Position) * (navigation.m_MaxSpeed * num2);
        float3 position = transform.m_Position;
        position.y += objectGeometryData.m_Size.y * 0.5f;
        Bounds3 maxBounds;
        maxBounds.min = position - num3 + math.min(x1, math.min(x2, y));
        maxBounds.max = position + num3 + math.max(x1, math.max(x2, y));
        return maxBounds;
      }
    }

    public struct FindLaneIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds3 m_Bounds;
      public float3 m_Position;
      public float m_MinDistance;
      public AircraftCurrentLane m_Result;
      public RoadTypes m_CarType;
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      public ComponentLookup<Curve> m_CurveData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_SubLanes.HasBuffer(edgeEntity))
          return;
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[edgeEntity];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          AircraftLaneFlags aircraftLaneFlags = this.m_Result.m_LaneFlags & ~(AircraftLaneFlags.Connection | AircraftLaneFlags.Airway);
          if (this.m_CarLaneData.HasComponent(subLane2))
          {
            if (this.m_PrefabCarLaneData[this.m_PrefabRefData[subLane2].m_Prefab].m_RoadTypes != this.m_CarType)
              continue;
          }
          else if (this.m_ConnectionLaneData.HasComponent(subLane2))
          {
            Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane2];
            if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (connectionLane.m_RoadTypes & this.m_CarType) != RoadTypes.None)
              aircraftLaneFlags |= AircraftLaneFlags.Connection;
            else
              continue;
          }
          else
            continue;
          Bezier4x3 bezier = this.m_CurveData[subLane2].m_Bezier;
          if ((double) MathUtils.Distance(MathUtils.Bounds(bezier), this.m_Position) < (double) this.m_MinDistance)
          {
            float t;
            float num = MathUtils.Distance(bezier, this.m_Position, out t);
            if ((double) num < (double) this.m_MinDistance)
            {
              this.m_MinDistance = num;
              this.m_Result.m_Lane = subLane2;
              this.m_Result.m_CurvePosition = (float3) t;
              this.m_Result.m_LaneFlags = aircraftLaneFlags;
            }
          }
        }
      }

      public void Iterate(ref AirwayHelpers.AirwayData airwayData)
      {
        Entity lane = Entity.Null;
        float curvePos = 0.0f;
        float distance = math.select(this.m_MinDistance, float.MaxValue, this.m_Result.m_Lane == Entity.Null);
        AirwayHelpers.AirwayMap airwayMap;
        if ((this.m_CarType & RoadTypes.Helicopter) != RoadTypes.None)
        {
          airwayMap = airwayData.helicopterMap;
          airwayMap.FindClosestLane(this.m_Position, this.m_CurveData, ref lane, ref curvePos, ref distance);
        }
        if ((this.m_CarType & RoadTypes.Airplane) != RoadTypes.None)
        {
          airwayMap = airwayData.airplaneMap;
          airwayMap.FindClosestLane(this.m_Position, this.m_CurveData, ref lane, ref curvePos, ref distance);
        }
        AircraftLaneFlags aircraftLaneFlags = this.m_Result.m_LaneFlags & ~AircraftLaneFlags.Connection;
        if (!(lane != Entity.Null))
          return;
        this.m_Result.m_Lane = lane;
        this.m_Result.m_CurvePosition = (float3) curvePos;
        this.m_Result.m_LaneFlags = aircraftLaneFlags | AircraftLaneFlags.Airway;
        this.m_MinDistance = math.min(this.m_MinDistance, distance);
      }
    }

    public struct AircraftCollisionIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_Ignore;
      public Line3.Segment m_Line;
      public ComponentLookup<Aircraft> m_AircraftData;
      public ComponentLookup<Transform> m_TransformData;
      public Entity m_Result;
      public float m_ClosestT;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _) || entity == this.m_Ignore || !this.m_AircraftData.HasComponent(entity))
          return;
        Transform transform = this.m_TransformData[entity];
        if ((double) transform.m_Position.y >= (double) this.m_Line.a.y)
          return;
        float t;
        double num = (double) MathUtils.Distance(this.m_Line, transform.m_Position, out t);
        if ((double) t >= (double) this.m_ClosestT)
          return;
        this.m_Result = entity;
        this.m_ClosestT = t;
      }
    }
  }
}
