// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarNavigationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Vehicles;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class CarNavigationHelpers
  {
    public struct LaneSignal
    {
      public Entity m_Petitioner;
      public Entity m_Lane;
      public sbyte m_Priority;

      public LaneSignal(Entity petitioner, Entity lane, int priority)
      {
        this.m_Petitioner = petitioner;
        this.m_Lane = lane;
        this.m_Priority = (sbyte) priority;
      }
    }

    public struct LaneReservation : IComparable<CarNavigationHelpers.LaneReservation>
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

      public int CompareTo(CarNavigationHelpers.LaneReservation other)
      {
        return this.m_Lane.Index - other.m_Lane.Index;
      }
    }

    public struct LaneEffects
    {
      public Entity m_Lane;
      public float3 m_SideEffects;
      public float2 m_Flow;

      public LaneEffects(Entity lane, float3 sideEffects, float2 flow)
      {
        this.m_Lane = lane;
        this.m_SideEffects = sideEffects;
        this.m_Flow = flow;
      }
    }

    public struct CurrentLaneCache
    {
      private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      private Entity m_WasCurrentLane;
      private Entity m_WasChangeLane;
      private float2 m_WasCurvePosition;
      private DynamicBuffer<BlockedLane> m_WasBlockedLanes;

      public CurrentLaneCache(
        ref CarCurrentLane currentLane,
        DynamicBuffer<BlockedLane> blockedLanes,
        EntityStorageInfoLookup entityLookup,
        NativeQuadTree<Entity, QuadTreeBoundsXZ> searchTree)
      {
        if (!entityLookup.Exists(currentLane.m_Lane))
          currentLane.m_Lane = Entity.Null;
        if (currentLane.m_ChangeLane != Entity.Null && !entityLookup.Exists(currentLane.m_ChangeLane))
        {
          currentLane.m_ChangeLane = Entity.Null;
          currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
        }
        this.m_WasBlockedLanes = blockedLanes;
        this.m_SearchTree = searchTree;
        this.m_WasCurrentLane = currentLane.m_Lane;
        this.m_WasChangeLane = currentLane.m_ChangeLane;
        this.m_WasCurvePosition = currentLane.m_CurvePosition.xy;
      }

      public void CheckChanges(
        Entity entity,
        ref CarCurrentLane currentLane,
        NativeList<BlockedLane> newBlockedLanes,
        LaneObjectCommandBuffer buffer,
        BufferLookup<LaneObject> laneObjects,
        Transform transform,
        Moving moving,
        CarNavigation navigation,
        ObjectGeometryData objectGeometryData)
      {
        if (newBlockedLanes.IsCreated && newBlockedLanes.Length != 0)
        {
          if (laneObjects.HasBuffer(this.m_WasCurrentLane))
          {
            this.m_WasBlockedLanes.Add(new BlockedLane(this.m_WasCurrentLane, this.m_WasCurvePosition));
            buffer.Add(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
          }
          else
          {
            QuadTreeBoundsXZ bounds;
            if (this.m_SearchTree.TryGet(entity, out bounds))
            {
              Bounds3 minBounds = this.CalculateMinBounds(transform, moving, navigation, objectGeometryData);
              if (math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max))
                buffer.Update(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
            }
          }
          if (laneObjects.HasBuffer(this.m_WasChangeLane))
            this.m_WasBlockedLanes.Add(new BlockedLane(this.m_WasChangeLane, this.m_WasCurvePosition));
          int index1 = 0;
label_17:
          while (index1 < this.m_WasBlockedLanes.Length)
          {
            BlockedLane wasBlockedLane = this.m_WasBlockedLanes[index1];
            for (int index2 = 0; index2 < newBlockedLanes.Length; ++index2)
            {
              BlockedLane newBlockedLane = newBlockedLanes[index2];
              if (newBlockedLane.m_Lane == wasBlockedLane.m_Lane)
              {
                if (!newBlockedLane.m_CurvePosition.Equals(wasBlockedLane.m_CurvePosition))
                {
                  this.m_WasBlockedLanes[index1] = newBlockedLane;
                  buffer.Update(newBlockedLane.m_Lane, entity, newBlockedLane.m_CurvePosition);
                }
                newBlockedLanes.RemoveAtSwapBack(index2);
                ++index1;
                goto label_17;
              }
            }
            this.m_WasBlockedLanes.RemoveAt(index1);
            buffer.Remove(wasBlockedLane.m_Lane, entity);
          }
          for (int index3 = 0; index3 < newBlockedLanes.Length; ++index3)
          {
            BlockedLane newBlockedLane = newBlockedLanes[index3];
            this.m_WasBlockedLanes.Add(newBlockedLane);
            buffer.Add(newBlockedLane.m_Lane, entity, newBlockedLane.m_CurvePosition);
          }
        }
        else
        {
          if (this.m_WasBlockedLanes.Length != 0)
          {
            for (int index = 0; index < this.m_WasBlockedLanes.Length; ++index)
            {
              Entity lane = this.m_WasBlockedLanes[index].m_Lane;
              if (laneObjects.HasBuffer(lane))
                buffer.Remove(lane, entity);
            }
            this.m_WasBlockedLanes.Clear();
            this.m_WasCurrentLane = Entity.Null;
            this.m_WasChangeLane = Entity.Null;
          }
          if (currentLane.m_Lane == this.m_WasChangeLane)
          {
            if (laneObjects.HasBuffer(this.m_WasCurrentLane))
            {
              buffer.Remove(this.m_WasCurrentLane, entity);
              if (laneObjects.HasBuffer(currentLane.m_Lane))
              {
                if (!this.m_WasCurvePosition.Equals(currentLane.m_CurvePosition.xy))
                  buffer.Update(currentLane.m_Lane, entity, currentLane.m_CurvePosition.xy);
              }
              else
                buffer.Add(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
            }
            else if (laneObjects.HasBuffer(currentLane.m_Lane))
            {
              buffer.Remove(entity);
              if (!this.m_WasCurvePosition.Equals(currentLane.m_CurvePosition.xy))
                buffer.Update(currentLane.m_Lane, entity, currentLane.m_CurvePosition.xy);
            }
            else
            {
              QuadTreeBoundsXZ bounds;
              if (this.m_SearchTree.TryGet(entity, out bounds))
              {
                Bounds3 minBounds = this.CalculateMinBounds(transform, moving, navigation, objectGeometryData);
                if (math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max))
                  buffer.Update(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
              }
            }
            if (!laneObjects.HasBuffer(currentLane.m_ChangeLane))
              return;
            buffer.Add(currentLane.m_ChangeLane, entity, currentLane.m_CurvePosition.xy);
          }
          else
          {
            if (currentLane.m_Lane != this.m_WasCurrentLane)
            {
              if (laneObjects.HasBuffer(this.m_WasCurrentLane))
                buffer.Remove(this.m_WasCurrentLane, entity);
              else
                buffer.Remove(entity);
              if (laneObjects.HasBuffer(currentLane.m_Lane))
                buffer.Add(currentLane.m_Lane, entity, currentLane.m_CurvePosition.xy);
              else
                buffer.Add(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
            }
            else if (laneObjects.HasBuffer(this.m_WasCurrentLane))
            {
              if (!this.m_WasCurvePosition.Equals(currentLane.m_CurvePosition.xy))
                buffer.Update(this.m_WasCurrentLane, entity, currentLane.m_CurvePosition.xy);
            }
            else
            {
              QuadTreeBoundsXZ bounds;
              if (this.m_SearchTree.TryGet(entity, out bounds))
              {
                Bounds3 minBounds = this.CalculateMinBounds(transform, moving, navigation, objectGeometryData);
                if (math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max))
                  buffer.Update(entity, this.CalculateMaxBounds(transform, moving, navigation, objectGeometryData));
              }
            }
            if (currentLane.m_ChangeLane != this.m_WasChangeLane)
            {
              if (laneObjects.HasBuffer(this.m_WasChangeLane))
                buffer.Remove(this.m_WasChangeLane, entity);
              if (!laneObjects.HasBuffer(currentLane.m_ChangeLane))
                return;
              buffer.Add(currentLane.m_ChangeLane, entity, currentLane.m_CurvePosition.xy);
            }
            else
            {
              if (!laneObjects.HasBuffer(this.m_WasChangeLane) || this.m_WasCurvePosition.Equals(currentLane.m_CurvePosition.xy))
                return;
              buffer.Update(this.m_WasChangeLane, entity, currentLane.m_CurvePosition.xy);
            }
          }
        }
      }

      private Bounds3 CalculateMinBounds(
        Transform transform,
        Moving moving,
        CarNavigation navigation,
        ObjectGeometryData objectGeometryData)
      {
        float num = 0.266666681f;
        float3 x = moving.m_Velocity * num;
        float3 y = math.normalizesafe(navigation.m_TargetPosition - transform.m_Position) * math.abs(navigation.m_MaxSpeed * num);
        Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, objectGeometryData);
        bounds.min += math.min((float3) 0.0f, math.min(x, y));
        bounds.max += math.max((float3) 0.0f, math.max(x, y));
        return bounds;
      }

      private Bounds3 CalculateMaxBounds(
        Transform transform,
        Moving moving,
        CarNavigation navigation,
        ObjectGeometryData objectGeometryData)
      {
        float num1 = -1.06666672f;
        float num2 = 2f;
        float num3 = math.length(objectGeometryData.m_Size) * 0.5f;
        float3 x1 = moving.m_Velocity * num1;
        float3 x2 = moving.m_Velocity * num2;
        float3 y = math.normalizesafe(navigation.m_TargetPosition - transform.m_Position) * math.abs(navigation.m_MaxSpeed * num2);
        float3 position = transform.m_Position;
        position.y += objectGeometryData.m_Size.y * 0.5f;
        Bounds3 maxBounds;
        maxBounds.min = position - num3 + math.min(x1, math.min(x2, y));
        maxBounds.max = position + num3 + math.max(x1, math.max(x2, y));
        return maxBounds;
      }
    }

    public struct TrailerLaneCache
    {
      private NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      private Entity m_WasCurrentLane;
      private Entity m_WasNextLane;
      private float2 m_WasCurrentPosition;
      private float2 m_WasNextPosition;
      private DynamicBuffer<BlockedLane> m_WasBlockedLanes;

      public TrailerLaneCache(
        ref CarTrailerLane trailerLane,
        DynamicBuffer<BlockedLane> blockedLanes,
        ComponentLookup<PrefabRef> prefabRefData,
        NativeQuadTree<Entity, QuadTreeBoundsXZ> searchTree)
      {
        if (!prefabRefData.HasComponent(trailerLane.m_Lane))
          trailerLane.m_Lane = Entity.Null;
        if (!prefabRefData.HasComponent(trailerLane.m_NextLane))
          trailerLane.m_NextLane = Entity.Null;
        this.m_WasBlockedLanes = blockedLanes;
        this.m_SearchTree = searchTree;
        this.m_WasCurrentLane = trailerLane.m_Lane;
        this.m_WasNextLane = trailerLane.m_NextLane;
        this.m_WasCurrentPosition = trailerLane.m_CurvePosition;
        this.m_WasNextPosition = trailerLane.m_NextPosition;
      }

      public void CheckChanges(
        Entity entity,
        ref CarTrailerLane trailerLane,
        NativeList<BlockedLane> newBlockedLanes,
        LaneObjectCommandBuffer buffer,
        BufferLookup<LaneObject> laneObjects,
        Transform transform,
        Moving moving,
        CarNavigation tractorNavigation,
        ObjectGeometryData objectGeometryData)
      {
        if (newBlockedLanes.IsCreated && newBlockedLanes.Length != 0)
        {
          if (laneObjects.HasBuffer(this.m_WasCurrentLane))
          {
            this.m_WasBlockedLanes.Add(new BlockedLane(this.m_WasCurrentLane, this.m_WasCurrentPosition));
            buffer.Add(entity, this.CalculateMaxBounds(transform, moving, tractorNavigation, objectGeometryData));
          }
          else
          {
            QuadTreeBoundsXZ bounds;
            if (this.m_SearchTree.TryGet(entity, out bounds))
            {
              Bounds3 minBounds = this.CalculateMinBounds(transform, moving, tractorNavigation, objectGeometryData);
              if (math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max))
                buffer.Update(entity, this.CalculateMaxBounds(transform, moving, tractorNavigation, objectGeometryData));
            }
          }
          if (laneObjects.HasBuffer(this.m_WasNextLane))
            this.m_WasBlockedLanes.Add(new BlockedLane(this.m_WasNextLane, this.m_WasNextPosition));
          int index1 = 0;
label_17:
          while (index1 < this.m_WasBlockedLanes.Length)
          {
            BlockedLane wasBlockedLane = this.m_WasBlockedLanes[index1];
            for (int index2 = 0; index2 < newBlockedLanes.Length; ++index2)
            {
              BlockedLane newBlockedLane = newBlockedLanes[index2];
              if (newBlockedLane.m_Lane == wasBlockedLane.m_Lane)
              {
                if (!newBlockedLane.m_CurvePosition.Equals(wasBlockedLane.m_CurvePosition))
                {
                  this.m_WasBlockedLanes[index1] = newBlockedLane;
                  buffer.Update(newBlockedLane.m_Lane, entity, newBlockedLane.m_CurvePosition);
                }
                newBlockedLanes.RemoveAtSwapBack(index2);
                ++index1;
                goto label_17;
              }
            }
            this.m_WasBlockedLanes.RemoveAt(index1);
            buffer.Remove(wasBlockedLane.m_Lane, entity);
          }
          for (int index3 = 0; index3 < newBlockedLanes.Length; ++index3)
          {
            BlockedLane newBlockedLane = newBlockedLanes[index3];
            this.m_WasBlockedLanes.Add(newBlockedLane);
            buffer.Add(newBlockedLane.m_Lane, entity, newBlockedLane.m_CurvePosition);
          }
        }
        else
        {
          if (this.m_WasBlockedLanes.Length != 0)
          {
            for (int index = 0; index < this.m_WasBlockedLanes.Length; ++index)
            {
              Entity lane = this.m_WasBlockedLanes[index].m_Lane;
              if (laneObjects.HasBuffer(lane))
                buffer.Remove(lane, entity);
            }
            this.m_WasBlockedLanes.Clear();
            this.m_WasCurrentLane = Entity.Null;
            this.m_WasNextLane = Entity.Null;
          }
          if (trailerLane.m_Lane == this.m_WasNextLane)
          {
            if (laneObjects.HasBuffer(this.m_WasCurrentLane))
            {
              buffer.Remove(this.m_WasCurrentLane, entity);
              if (laneObjects.HasBuffer(trailerLane.m_Lane))
              {
                if (!this.m_WasNextPosition.Equals(trailerLane.m_CurvePosition))
                  buffer.Update(trailerLane.m_Lane, entity, trailerLane.m_CurvePosition);
              }
              else
                buffer.Add(entity, this.CalculateMaxBounds(transform, moving, tractorNavigation, objectGeometryData));
            }
            else if (laneObjects.HasBuffer(trailerLane.m_Lane))
            {
              buffer.Remove(entity);
              if (!this.m_WasNextPosition.Equals(trailerLane.m_CurvePosition))
                buffer.Update(trailerLane.m_Lane, entity, trailerLane.m_CurvePosition);
            }
            else
            {
              QuadTreeBoundsXZ bounds;
              if (this.m_SearchTree.TryGet(entity, out bounds))
              {
                Bounds3 minBounds = this.CalculateMinBounds(transform, moving, tractorNavigation, objectGeometryData);
                if (math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max))
                  buffer.Update(entity, this.CalculateMaxBounds(transform, moving, tractorNavigation, objectGeometryData));
              }
            }
            if (!laneObjects.HasBuffer(trailerLane.m_NextLane))
              return;
            buffer.Add(trailerLane.m_NextLane, entity, trailerLane.m_NextPosition);
          }
          else
          {
            if (trailerLane.m_Lane != this.m_WasCurrentLane)
            {
              if (laneObjects.HasBuffer(this.m_WasCurrentLane))
                buffer.Remove(this.m_WasCurrentLane, entity);
              else
                buffer.Remove(entity);
              if (laneObjects.HasBuffer(trailerLane.m_Lane))
                buffer.Add(trailerLane.m_Lane, entity, trailerLane.m_CurvePosition);
              else
                buffer.Add(entity, this.CalculateMaxBounds(transform, moving, tractorNavigation, objectGeometryData));
            }
            else if (laneObjects.HasBuffer(this.m_WasCurrentLane))
            {
              if (!this.m_WasCurrentPosition.Equals(trailerLane.m_CurvePosition))
                buffer.Update(this.m_WasCurrentLane, entity, trailerLane.m_CurvePosition);
            }
            else
            {
              QuadTreeBoundsXZ bounds;
              if (this.m_SearchTree.TryGet(entity, out bounds))
              {
                Bounds3 minBounds = this.CalculateMinBounds(transform, moving, tractorNavigation, objectGeometryData);
                if (math.any(minBounds.min < bounds.m_Bounds.min) | math.any(minBounds.max > bounds.m_Bounds.max))
                  buffer.Update(entity, this.CalculateMaxBounds(transform, moving, tractorNavigation, objectGeometryData));
              }
            }
            if (trailerLane.m_NextLane != this.m_WasNextLane)
            {
              if (laneObjects.HasBuffer(this.m_WasNextLane))
                buffer.Remove(this.m_WasNextLane, entity);
              if (!laneObjects.HasBuffer(trailerLane.m_NextLane))
                return;
              buffer.Add(trailerLane.m_NextLane, entity, trailerLane.m_NextPosition);
            }
            else
            {
              if (!laneObjects.HasBuffer(this.m_WasNextLane) || this.m_WasNextPosition.Equals(trailerLane.m_NextPosition))
                return;
              buffer.Update(this.m_WasNextLane, entity, trailerLane.m_NextPosition);
            }
          }
        }
      }

      private Bounds3 CalculateMinBounds(
        Transform transform,
        Moving moving,
        CarNavigation tractorNavigation,
        ObjectGeometryData objectGeometryData)
      {
        float num = 0.266666681f;
        float3 x = moving.m_Velocity * num;
        float3 y = math.normalizesafe(tractorNavigation.m_TargetPosition - transform.m_Position) * math.abs(tractorNavigation.m_MaxSpeed * num);
        Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, objectGeometryData);
        bounds.min += math.min((float3) 0.0f, math.min(x, y));
        bounds.max += math.max((float3) 0.0f, math.max(x, y));
        return bounds;
      }

      private Bounds3 CalculateMaxBounds(
        Transform transform,
        Moving moving,
        CarNavigation tractorNavigation,
        ObjectGeometryData objectGeometryData)
      {
        float num1 = -1.06666672f;
        float num2 = 2f;
        float num3 = math.length(objectGeometryData.m_Size) * 0.5f;
        float3 x1 = moving.m_Velocity * num1;
        float3 x2 = moving.m_Velocity * num2;
        float3 y = math.normalizesafe(tractorNavigation.m_TargetPosition - transform.m_Position) * math.abs(tractorNavigation.m_MaxSpeed * num2);
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
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public Bounds3 m_Bounds;
      public float3 m_Position;
      public float m_MinDistance;
      public CarCurrentLane m_Result;
      public RoadTypes m_CarType;
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      public BufferLookup<Triangle> m_AreaTriangles;
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      public ComponentLookup<MasterLane> m_MasterLaneData;
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      public ComponentLookup<Curve> m_CurveData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity ownerEntity)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_SubLanes.HasBuffer(ownerEntity))
          return;
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[ownerEntity];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          Game.Vehicles.CarLaneFlags carLaneFlags = (this.m_Result.m_LaneFlags | Game.Vehicles.CarLaneFlags.EnteringRoad | Game.Vehicles.CarLaneFlags.FixedLane) & ~(Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Connection | Game.Vehicles.CarLaneFlags.Area | Game.Vehicles.CarLaneFlags.Roundabout | Game.Vehicles.CarLaneFlags.CanReverse);
          if (this.m_CarLaneData.HasComponent(subLane2))
          {
            if (!this.m_MasterLaneData.HasComponent(subLane2) && this.m_PrefabCarLaneData[this.m_PrefabRefData[subLane2].m_Prefab].m_RoadTypes == this.m_CarType)
            {
              Game.Net.CarLane carLane = this.m_CarLaneData[subLane2];
              if ((carLane.m_Flags & Game.Net.CarLaneFlags.Twoway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
                carLaneFlags |= Game.Vehicles.CarLaneFlags.CanReverse;
              if ((carLane.m_Flags & (Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout)) == Game.Net.CarLaneFlags.Roundabout)
                carLaneFlags |= Game.Vehicles.CarLaneFlags.Roundabout;
            }
            else
              continue;
          }
          else if (this.m_ConnectionLaneData.HasComponent(subLane2))
          {
            Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane2];
            if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (connectionLane.m_RoadTypes & this.m_CarType) != RoadTypes.None)
              carLaneFlags |= Game.Vehicles.CarLaneFlags.Connection;
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
              this.m_Bounds = new Bounds3(this.m_Position - num, this.m_Position + num);
              this.m_MinDistance = num;
              this.m_Result.m_Lane = subLane2;
              this.m_Result.m_CurvePosition = (float3) t;
              this.m_Result.m_LaneFlags = carLaneFlags;
            }
          }
        }
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_SubLanes.HasBuffer(item.m_Area))
          return;
        Triangle3 triangle3 = AreaUtils.GetTriangle3(this.m_AreaNodes[item.m_Area], this.m_AreaTriangles[item.m_Area][item.m_Triangle]);
        float2 t1;
        float num1 = MathUtils.Distance(triangle3, this.m_Position, out t1);
        if ((double) num1 >= (double) this.m_MinDistance)
          return;
        float3 position = MathUtils.Position(triangle3, t1);
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[item.m_Area];
        float num2 = float.MaxValue;
        Entity entity = Entity.Null;
        float num3 = 0.0f;
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          if (this.m_ConnectionLaneData.HasComponent(subLane2))
          {
            Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane2];
            if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (connectionLane.m_RoadTypes & this.m_CarType) != RoadTypes.None)
            {
              Curve curve = this.m_CurveData[subLane2];
              float2 t2;
              if (MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t2) || MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t2))
              {
                float t3;
                float num4 = MathUtils.Distance(curve.m_Bezier, position, out t3);
                if ((double) num4 < (double) num2)
                {
                  num2 = num4;
                  entity = subLane2;
                  num3 = t3;
                }
              }
            }
          }
        }
        if (!(entity != Entity.Null))
          return;
        Game.Vehicles.CarLaneFlags carLaneFlags = (this.m_Result.m_LaneFlags | Game.Vehicles.CarLaneFlags.EnteringRoad | Game.Vehicles.CarLaneFlags.FixedLane | Game.Vehicles.CarLaneFlags.Area) & ~(Game.Vehicles.CarLaneFlags.ParkingSpace | Game.Vehicles.CarLaneFlags.Connection | Game.Vehicles.CarLaneFlags.Roundabout | Game.Vehicles.CarLaneFlags.CanReverse);
        this.m_Bounds = new Bounds3(this.m_Position - num1, this.m_Position + num1);
        this.m_MinDistance = num1;
        this.m_Result.m_Lane = entity;
        this.m_Result.m_CurvePosition = (float3) num3;
        this.m_Result.m_LaneFlags = carLaneFlags;
      }
    }

    public struct FindBlockedLanesIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds3 m_Bounds;
      public Line3.Segment m_Line;
      public float m_Radius;
      public NativeList<BlockedLane> m_BlockedLanes;
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      public ComponentLookup<MasterLane> m_MasterLaneData;
      public ComponentLookup<Curve> m_CurveData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<NetLaneData> m_PrefabLaneData;

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
          if (!this.m_MasterLaneData.HasComponent(subLane2))
          {
            Entity prefab = this.m_PrefabRefData[subLane2].m_Prefab;
            Bezier4x3 bezier = this.m_CurveData[subLane2].m_Bezier;
            NetLaneData netLaneData = this.m_PrefabLaneData[prefab];
            float range = this.m_Radius + netLaneData.m_Width * 0.4f;
            if (MathUtils.Intersect(MathUtils.Expand(MathUtils.Bounds(bezier), (float3) range), this.m_Line, out float2 _))
            {
              float2 t1;
              float num1 = MathUtils.Distance(bezier, this.m_Line, out t1);
              if ((double) num1 < (double) range)
              {
                float num2 = math.max(0.0f, num1 - netLaneData.m_Width * 0.4f);
                float length = math.sqrt(math.max(0.0f, (float) ((double) this.m_Radius * (double) this.m_Radius - (double) num2 * (double) num2))) + netLaneData.m_Width * 0.4f;
                Bounds1 t2 = new Bounds1(0.0f, t1.x);
                Bounds1 t3 = new Bounds1(t1.x, 1f);
                MathUtils.ClampLengthInverse(bezier, ref t2, length);
                MathUtils.ClampLength(bezier, ref t3, length);
                this.m_BlockedLanes.Add(new BlockedLane(subLane2, new float2(t2.min, t3.max)));
              }
            }
          }
        }
      }
    }
  }
}
