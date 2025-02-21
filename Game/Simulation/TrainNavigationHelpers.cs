// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrainNavigationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Vehicles;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class TrainNavigationHelpers
  {
    public static void GetCurvePositions(
      ref TrainCurrentLane currentLane,
      out float2 pos1,
      out float2 pos2)
    {
      pos1 = currentLane.m_Front.m_CurvePosition.yz;
      pos2 = currentLane.m_Rear.m_CurvePosition.yz;
      if (!(currentLane.m_Front.m_Lane == currentLane.m_Rear.m_Lane))
        return;
      if ((double) pos1.y < (double) pos1.x)
      {
        pos1.y = math.min(pos1.y, pos2.y);
        pos1.x = math.max(pos1.x, pos2.x);
      }
      else
      {
        pos1.x = math.min(pos1.x, pos2.x);
        pos1.y = math.max(pos1.y, pos2.y);
      }
      pos2 = pos1;
    }

    public static void GetCurvePositions(
      ref ParkedTrain parkedTrain,
      out float2 pos1,
      out float2 pos2)
    {
      pos1 = (float2) parkedTrain.m_CurvePosition.x;
      pos2 = (float2) parkedTrain.m_CurvePosition.y;
      if (!(parkedTrain.m_FrontLane == parkedTrain.m_RearLane))
        return;
      pos1.x = math.min(pos1.x, pos2.x);
      pos1.y = math.max(pos1.y, pos2.y);
      pos2 = pos1;
    }

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

    public struct LaneReservation : IComparable<TrainNavigationHelpers.LaneReservation>
    {
      public Entity m_Lane;
      public byte m_Priority;

      public LaneReservation(Entity lane, int priority)
      {
        this.m_Lane = lane;
        this.m_Priority = (byte) priority;
      }

      public int CompareTo(TrainNavigationHelpers.LaneReservation other)
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
      private Entity m_WasCurrentLane1;
      private Entity m_WasCurrentLane2;
      private float2 m_WasCurvePosition1;
      private float2 m_WasCurvePosition2;

      public CurrentLaneCache(ref TrainCurrentLane currentLane, ComponentLookup<Lane> laneData)
      {
        if (currentLane.m_Front.m_Lane != Entity.Null && !laneData.HasComponent(currentLane.m_Front.m_Lane))
          currentLane.m_Front.m_Lane = Entity.Null;
        if (currentLane.m_Rear.m_Lane != Entity.Null && !laneData.HasComponent(currentLane.m_Rear.m_Lane))
          currentLane.m_Rear.m_Lane = Entity.Null;
        if (currentLane.m_FrontCache.m_Lane != Entity.Null && !laneData.HasComponent(currentLane.m_FrontCache.m_Lane))
          currentLane.m_FrontCache.m_Lane = Entity.Null;
        if (currentLane.m_RearCache.m_Lane != Entity.Null && !laneData.HasComponent(currentLane.m_RearCache.m_Lane))
          currentLane.m_RearCache.m_Lane = Entity.Null;
        this.m_WasCurrentLane1 = currentLane.m_Front.m_Lane;
        this.m_WasCurrentLane2 = currentLane.m_Rear.m_Lane;
        TrainNavigationHelpers.GetCurvePositions(ref currentLane, out this.m_WasCurvePosition1, out this.m_WasCurvePosition2);
      }

      public void CheckChanges(
        Entity entity,
        TrainCurrentLane currentLane,
        LaneObjectCommandBuffer buffer)
      {
        float2 pos1;
        float2 pos2;
        TrainNavigationHelpers.GetCurvePositions(ref currentLane, out pos1, out pos2);
        if (currentLane.m_Rear.m_Lane == this.m_WasCurrentLane1)
        {
          if (currentLane.m_Front.m_Lane == this.m_WasCurrentLane2)
          {
            if (currentLane.m_Front.m_Lane != Entity.Null && !this.m_WasCurvePosition2.Equals(pos1))
              buffer.Update(currentLane.m_Front.m_Lane, entity, pos1);
            if (!(currentLane.m_Rear.m_Lane != currentLane.m_Front.m_Lane) || !(currentLane.m_Rear.m_Lane != Entity.Null) || this.m_WasCurvePosition1.Equals(pos2))
              return;
            buffer.Update(currentLane.m_Rear.m_Lane, entity, pos2);
          }
          else
          {
            if (currentLane.m_Rear.m_Lane != this.m_WasCurrentLane2 && this.m_WasCurrentLane2 != Entity.Null)
              buffer.Remove(this.m_WasCurrentLane2, entity);
            if (currentLane.m_Rear.m_Lane != Entity.Null && !this.m_WasCurvePosition1.Equals(pos2))
              buffer.Update(currentLane.m_Rear.m_Lane, entity, pos2);
            if (!(currentLane.m_Front.m_Lane != this.m_WasCurrentLane1) || !(currentLane.m_Front.m_Lane != Entity.Null))
              return;
            buffer.Add(currentLane.m_Front.m_Lane, entity, pos1);
          }
        }
        else if (currentLane.m_Front.m_Lane == this.m_WasCurrentLane2)
        {
          if (currentLane.m_Front.m_Lane != this.m_WasCurrentLane1 && this.m_WasCurrentLane1 != Entity.Null)
            buffer.Remove(this.m_WasCurrentLane1, entity);
          if (currentLane.m_Front.m_Lane != Entity.Null && !this.m_WasCurvePosition2.Equals(pos1))
            buffer.Update(currentLane.m_Front.m_Lane, entity, pos1);
          if (!(currentLane.m_Rear.m_Lane != this.m_WasCurrentLane2) || !(currentLane.m_Rear.m_Lane != Entity.Null))
            return;
          buffer.Add(currentLane.m_Rear.m_Lane, entity, pos2);
        }
        else if (this.m_WasCurrentLane1 == this.m_WasCurrentLane2)
        {
          if (this.m_WasCurrentLane1 != Entity.Null)
            buffer.Remove(this.m_WasCurrentLane1, entity);
          if (currentLane.m_Front.m_Lane != Entity.Null)
            buffer.Add(currentLane.m_Front.m_Lane, entity, pos1);
          if (!(currentLane.m_Rear.m_Lane != currentLane.m_Front.m_Lane) || !(currentLane.m_Rear.m_Lane != Entity.Null))
            return;
          buffer.Add(currentLane.m_Rear.m_Lane, entity, pos2);
        }
        else if (currentLane.m_Front.m_Lane == currentLane.m_Rear.m_Lane)
        {
          if (this.m_WasCurrentLane1 != Entity.Null)
            buffer.Remove(this.m_WasCurrentLane1, entity);
          if (this.m_WasCurrentLane2 != Entity.Null)
            buffer.Remove(this.m_WasCurrentLane2, entity);
          if (!(currentLane.m_Front.m_Lane != Entity.Null))
            return;
          buffer.Add(currentLane.m_Front.m_Lane, entity, pos1);
        }
        else
        {
          if (currentLane.m_Front.m_Lane != this.m_WasCurrentLane1)
          {
            if (this.m_WasCurrentLane1 != Entity.Null)
              buffer.Remove(this.m_WasCurrentLane1, entity);
            if (currentLane.m_Front.m_Lane != Entity.Null)
              buffer.Add(currentLane.m_Front.m_Lane, entity, pos1);
          }
          else if (this.m_WasCurrentLane1 != Entity.Null && !this.m_WasCurvePosition1.Equals(pos1))
            buffer.Update(this.m_WasCurrentLane1, entity, pos1);
          if (currentLane.m_Rear.m_Lane != this.m_WasCurrentLane2)
          {
            if (this.m_WasCurrentLane2 != Entity.Null)
              buffer.Remove(this.m_WasCurrentLane2, entity);
            if (!(currentLane.m_Rear.m_Lane != Entity.Null))
              return;
            buffer.Add(currentLane.m_Rear.m_Lane, entity, pos2);
          }
          else
          {
            if (!(this.m_WasCurrentLane2 != Entity.Null) || this.m_WasCurvePosition2.Equals(pos2))
              return;
            buffer.Update(this.m_WasCurrentLane2, entity, pos2);
          }
        }
      }
    }

    public struct FindLaneIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Bounds3 m_Bounds;
      public float3 m_FrontPivot;
      public float3 m_RearPivot;
      public float2 m_MinDistance;
      public TrainCurrentLane m_Result;
      public TrackTypes m_TrackType;
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
      public ComponentLookup<Curve> m_CurveData;
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
      {
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_SubLanes.HasBuffer(edgeEntity))
          return;
        TrainLaneFlags trainLaneFlags1 = this.m_Result.m_Front.m_LaneFlags & ~TrainLaneFlags.Connection;
        TrainLaneFlags trainLaneFlags2 = this.m_Result.m_Rear.m_LaneFlags & ~TrainLaneFlags.Connection;
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[edgeEntity];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          if (this.m_TrackLaneData.HasComponent(subLane2) && (this.m_PrefabTrackLaneData[this.m_PrefabRefData[subLane2].m_Prefab].m_TrackTypes & this.m_TrackType) != TrackTypes.None)
          {
            Bezier4x3 bezier = this.m_CurveData[subLane2].m_Bezier;
            Bounds3 bounds1 = MathUtils.Bounds(bezier);
            float num1 = MathUtils.Distance(bounds1, this.m_FrontPivot);
            float num2 = MathUtils.Distance(bounds1, this.m_RearPivot);
            if ((double) num1 < (double) this.m_MinDistance.x)
            {
              float t;
              float num3 = MathUtils.Distance(bezier, this.m_FrontPivot, out t);
              if ((double) num3 < (double) this.m_MinDistance.x)
              {
                TrainLaneFlags laneFlags = trainLaneFlags1;
                if (this.m_ConnectionLaneData.HasComponent(subLane2))
                  laneFlags |= TrainLaneFlags.Connection;
                this.m_MinDistance.x = num3;
                this.m_Result.m_Front = new TrainBogieLane(subLane2, (float4) t, laneFlags);
                this.m_Result.m_FrontCache = new TrainBogieCache(this.m_Result.m_Front);
              }
            }
            if ((double) num2 < (double) this.m_MinDistance.y)
            {
              float t;
              float num4 = MathUtils.Distance(bezier, this.m_RearPivot, out t);
              if ((double) num4 < (double) this.m_MinDistance.y)
              {
                TrainLaneFlags laneFlags = trainLaneFlags2;
                if (this.m_ConnectionLaneData.HasComponent(subLane2))
                  laneFlags |= TrainLaneFlags.Connection;
                this.m_MinDistance.y = num4;
                this.m_Result.m_Rear = new TrainBogieLane(subLane2, (float4) t, laneFlags);
                this.m_Result.m_RearCache = new TrainBogieCache(this.m_Result.m_Rear);
              }
            }
          }
        }
        if (this.m_Result.m_Front.m_Lane != Entity.Null && this.m_Result.m_Rear.m_Lane == Entity.Null)
        {
          this.m_Result.m_Rear = new TrainBogieLane(this.m_Result.m_Front.m_Lane, this.m_Result.m_Front.m_CurvePosition, trainLaneFlags2 | this.m_Result.m_Front.m_LaneFlags & TrainLaneFlags.Connection);
          this.m_Result.m_RearCache = new TrainBogieCache(this.m_Result.m_Rear);
        }
        else
        {
          if (!(this.m_Result.m_Front.m_Lane == Entity.Null) || !(this.m_Result.m_Rear.m_Lane != Entity.Null))
            return;
          this.m_Result.m_Front = new TrainBogieLane(this.m_Result.m_Rear.m_Lane, this.m_Result.m_Rear.m_CurvePosition, trainLaneFlags1 | this.m_Result.m_Rear.m_LaneFlags & TrainLaneFlags.Connection);
          this.m_Result.m_FrontCache = new TrainBogieCache(this.m_Result.m_Front);
        }
      }
    }
  }
}
