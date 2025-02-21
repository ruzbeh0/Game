// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WatercraftLaneSelectIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct WatercraftLaneSelectIterator
  {
    public ComponentLookup<Owner> m_OwnerData;
    public ComponentLookup<Lane> m_LaneData;
    public ComponentLookup<SlaveLane> m_SlaveLaneData;
    public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Watercraft> m_WatercraftData;
    public BufferLookup<SubLane> m_Lanes;
    public BufferLookup<LaneObject> m_LaneObjects;
    public Entity m_Entity;
    public Entity m_Blocker;
    public int m_Priority;
    private NativeArray<float> m_Buffer;
    private int m_BufferPos;
    private float m_LaneSwitchCost;
    private float m_LaneSwitchBaseCost;
    private Entity m_PrevLane;

    public void SetBuffer(ref WatercraftLaneSelectBuffer buffer) => this.m_Buffer = buffer.Ensure();

    public void CalculateLaneCosts(WatercraftNavigationLane navLaneData, int index)
    {
      if ((navLaneData.m_Flags & (WatercraftLaneFlags.Reserved | WatercraftLaneFlags.FixedLane)) != (WatercraftLaneFlags) 0 || !this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
        return;
      SlaveLane slaveLane = this.m_SlaveLaneData[navLaneData.m_Lane];
      DynamicBuffer<SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
      int num = math.min((int) slaveLane.m_MaxIndex, lane.Length - 1);
      float laneObjectCost = math.abs(navLaneData.m_CurvePosition.y - navLaneData.m_CurvePosition.x) * 0.49f;
      for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= num; ++minIndex)
      {
        Entity subLane = lane[minIndex].m_SubLane;
        this.m_Buffer[this.m_BufferPos++] = this.CalculateLaneObjectCost(laneObjectCost, index, subLane, navLaneData.m_Flags);
      }
    }

    private float CalculateLaneObjectCost(
      float laneObjectCost,
      int index,
      Entity lane,
      WatercraftLaneFlags laneFlags)
    {
      float laneObjectCost1 = 0.0f;
      if (this.m_LaneObjects.HasBuffer(lane))
      {
        DynamicBuffer<LaneObject> laneObject1 = this.m_LaneObjects[lane];
        if (index < 2 && this.m_Blocker != Entity.Null)
        {
          for (int index1 = 0; index1 < laneObject1.Length; ++index1)
          {
            LaneObject laneObject2 = laneObject1[index1];
            if (laneObject2.m_LaneObject == this.m_Blocker)
              laneObjectCost1 += this.CalculateLaneObjectCost(laneObject2, laneObjectCost, laneFlags);
            else
              laneObjectCost1 += laneObjectCost;
          }
        }
        else
          laneObjectCost1 += laneObjectCost * (float) laneObject1.Length;
      }
      return laneObjectCost1;
    }

    private float CalculateLaneObjectCost(
      float laneObjectCost,
      Entity lane,
      float minCurvePosition,
      WatercraftLaneFlags laneFlags)
    {
      float laneObjectCost1 = 0.0f;
      if (this.m_LaneObjects.HasBuffer(lane))
      {
        DynamicBuffer<LaneObject> laneObject1 = this.m_LaneObjects[lane];
        for (int index = 0; index < laneObject1.Length; ++index)
        {
          LaneObject laneObject2 = laneObject1[index];
          if ((double) laneObject2.m_CurvePosition.y > (double) minCurvePosition)
            laneObjectCost1 += this.CalculateLaneObjectCost(laneObject2, laneObjectCost, laneFlags);
        }
      }
      return laneObjectCost1;
    }

    private float CalculateLaneObjectCost(
      LaneObject laneObject,
      float laneObjectCost,
      WatercraftLaneFlags laneFlags)
    {
      return !this.m_MovingData.HasComponent(laneObject.m_LaneObject) && (!this.m_WatercraftData.HasComponent(laneObject.m_LaneObject) || (this.m_WatercraftData[laneObject.m_LaneObject].m_Flags & WatercraftFlags.Queueing) == (WatercraftFlags) 0 || (laneFlags & WatercraftLaneFlags.Queue) == (WatercraftLaneFlags) 0) ? math.lerp(1E+07f, 9000000f, laneObject.m_CurvePosition.y) : laneObjectCost;
    }

    public void CalculateLaneCosts(
      WatercraftNavigationLane navLaneData,
      WatercraftNavigationLane nextNavLaneData,
      int index)
    {
      if ((navLaneData.m_Flags & (WatercraftLaneFlags.Reserved | WatercraftLaneFlags.FixedLane)) == (WatercraftLaneFlags) 0 && this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
      {
        SlaveLane slaveLane1 = this.m_SlaveLaneData[navLaneData.m_Lane];
        DynamicBuffer<SubLane> lane1 = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
        int num1 = math.min((int) slaveLane1.m_MaxIndex, lane1.Length - 1);
        this.m_LaneSwitchCost = this.m_LaneSwitchBaseCost + math.select(1f, 5f, (slaveLane1.m_Flags & SlaveLaneFlags.AllowChange) == (SlaveLaneFlags) 0);
        float laneObjectCost1 = math.abs(navLaneData.m_CurvePosition.y - navLaneData.m_CurvePosition.x) * 0.49f;
        if ((nextNavLaneData.m_Flags & (WatercraftLaneFlags.Reserved | WatercraftLaneFlags.FixedLane)) == (WatercraftLaneFlags) 0 && this.m_SlaveLaneData.HasComponent(nextNavLaneData.m_Lane))
        {
          SlaveLane slaveLane2 = this.m_SlaveLaneData[nextNavLaneData.m_Lane];
          DynamicBuffer<SubLane> lane2 = this.m_Lanes[this.m_OwnerData[nextNavLaneData.m_Lane].m_Owner];
          int num2 = math.min((int) slaveLane2.m_MaxIndex, lane2.Length - 1);
          int num3 = this.m_BufferPos - (num2 - (int) slaveLane2.m_MinIndex + 1);
          for (int minIndex1 = (int) slaveLane1.m_MinIndex; minIndex1 <= num1; ++minIndex1)
          {
            Entity subLane = lane1[minIndex1].m_SubLane;
            Lane lane3 = this.m_LaneData[subLane];
            float x1 = 1000000f;
            int x2;
            int num4;
            if ((nextNavLaneData.m_Flags & WatercraftLaneFlags.GroupTarget) != (WatercraftLaneFlags) 0)
            {
              x2 = (int) slaveLane2.m_MinIndex;
              num4 = num2;
            }
            else
            {
              x2 = 100000;
              num4 = -100000;
              for (int minIndex2 = (int) slaveLane2.m_MinIndex; minIndex2 <= num2; ++minIndex2)
              {
                Lane lane4 = this.m_LaneData[lane2[minIndex2].m_SubLane];
                if (lane3.m_EndNode.Equals(lane4.m_StartNode))
                {
                  x2 = math.min(x2, minIndex2);
                  num4 = minIndex2;
                }
              }
            }
            if (x2 <= num4)
            {
              int num5 = num3;
              for (int minIndex3 = (int) slaveLane2.m_MinIndex; minIndex3 < x2; ++minIndex3)
                x1 = math.min(x1, this.m_Buffer[num5++] + this.GetLaneSwitchCost(x2 - minIndex3));
              for (int index1 = x2; index1 <= num4; ++index1)
                x1 = math.min(x1, this.m_Buffer[num5++]);
              for (int index2 = num4 + 1; index2 <= num2; ++index2)
                x1 = math.min(x1, this.m_Buffer[num5++] + this.GetLaneSwitchCost(index2 - num4));
              x1 += this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
              if (this.m_LaneReservationData.HasComponent(subLane))
              {
                Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
                x1 += this.GetLanePriorityCost(laneReservation.GetPriority());
              }
            }
            this.m_Buffer[this.m_BufferPos++] = x1;
          }
        }
        else if ((nextNavLaneData.m_Flags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0)
        {
          for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float laneObjectCost2 = this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
            if (this.m_LaneReservationData.HasComponent(subLane))
            {
              Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
              laneObjectCost2 += this.GetLanePriorityCost(laneReservation.GetPriority());
            }
            this.m_Buffer[this.m_BufferPos++] = laneObjectCost2;
          }
        }
        else
        {
          int x = 100000;
          int num6 = -100000;
          if ((nextNavLaneData.m_Flags & WatercraftLaneFlags.GroupTarget) != (WatercraftLaneFlags) 0)
          {
            for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              if (lane1[minIndex].m_SubLane == nextNavLaneData.m_Lane)
              {
                x = minIndex;
                num6 = minIndex;
                break;
              }
            }
          }
          else
          {
            Lane lane5 = this.m_LaneData[nextNavLaneData.m_Lane];
            for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              if (this.m_LaneData[lane1[minIndex].m_SubLane].m_EndNode.Equals(lane5.m_StartNode))
              {
                x = math.min(x, minIndex);
                num6 = minIndex;
              }
            }
          }
          for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float num7 = 0.0f;
            if (x <= num6)
              num7 += this.GetLaneSwitchCost(math.max(0, math.max(x - minIndex, minIndex - num6)));
            float num8 = num7 + this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
            if (this.m_LaneReservationData.HasComponent(subLane))
            {
              Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
              num8 += this.GetLanePriorityCost(laneReservation.GetPriority());
            }
            this.m_Buffer[this.m_BufferPos++] = num8;
          }
        }
      }
      this.m_LaneSwitchBaseCost += 0.01f;
    }

    private float GetLaneSwitchCost(int numLanes)
    {
      return (float) (numLanes * numLanes * numLanes) * this.m_LaneSwitchCost;
    }

    private float GetLanePriorityCost(int lanePriority)
    {
      return (float) math.max(0, lanePriority - this.m_Priority) * 1f;
    }

    public void UpdateOptimalLane(
      ref WatercraftCurrentLane currentLaneData,
      WatercraftNavigationLane nextNavLaneData)
    {
      Entity entity = currentLaneData.m_Lane;
      if ((currentLaneData.m_LaneFlags & WatercraftLaneFlags.FixedLane) == (WatercraftLaneFlags) 0 && this.m_SlaveLaneData.HasComponent(currentLaneData.m_Lane))
      {
        SlaveLane slaveLane1 = this.m_SlaveLaneData[currentLaneData.m_Lane];
        DynamicBuffer<SubLane> lane1 = this.m_Lanes[this.m_OwnerData[currentLaneData.m_Lane].m_Owner];
        int num1 = math.min((int) slaveLane1.m_MaxIndex, lane1.Length - 1);
        this.m_LaneSwitchCost = this.m_LaneSwitchBaseCost + math.select(1f, 5f, (slaveLane1.m_Flags & SlaveLaneFlags.AllowChange) == (SlaveLaneFlags) 0);
        float laneObjectCost1 = 0.49f;
        int num2 = 0;
        for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
        {
          if (lane1[minIndex].m_SubLane == currentLaneData.m_Lane)
          {
            num2 = minIndex;
            break;
          }
        }
        float num3 = float.MaxValue;
        if ((nextNavLaneData.m_Flags & (WatercraftLaneFlags.Reserved | WatercraftLaneFlags.FixedLane)) == (WatercraftLaneFlags) 0 && this.m_SlaveLaneData.HasComponent(nextNavLaneData.m_Lane))
        {
          SlaveLane slaveLane2 = this.m_SlaveLaneData[nextNavLaneData.m_Lane];
          DynamicBuffer<SubLane> lane2 = this.m_Lanes[this.m_OwnerData[nextNavLaneData.m_Lane].m_Owner];
          int num4 = math.min((int) slaveLane2.m_MaxIndex, lane2.Length - 1);
          int num5 = this.m_BufferPos - (num4 - (int) slaveLane2.m_MinIndex + 1);
          for (int minIndex1 = (int) slaveLane1.m_MinIndex; minIndex1 <= num1; ++minIndex1)
          {
            Entity subLane = lane1[minIndex1].m_SubLane;
            Lane lane3 = this.m_LaneData[subLane];
            float x1 = 1000000f;
            int x2;
            int num6;
            if ((nextNavLaneData.m_Flags & WatercraftLaneFlags.GroupTarget) != (WatercraftLaneFlags) 0)
            {
              x2 = (int) slaveLane2.m_MinIndex;
              num6 = num4;
            }
            else
            {
              x2 = 100000;
              num6 = -100000;
              for (int minIndex2 = (int) slaveLane2.m_MinIndex; minIndex2 <= num4; ++minIndex2)
              {
                Lane lane4 = this.m_LaneData[lane2[minIndex2].m_SubLane];
                if (lane3.m_EndNode.Equals(lane4.m_StartNode))
                {
                  x2 = math.min(x2, minIndex2);
                  num6 = minIndex2;
                }
              }
            }
            if (x2 <= num6)
            {
              int num7 = num5 + (x2 - (int) slaveLane2.m_MinIndex);
              for (int index = x2; index <= num6; ++index)
                x1 = math.min(x1, this.m_Buffer[num7++]);
              x1 += this.CalculateLaneObjectCost(laneObjectCost1, subLane, currentLaneData.m_CurvePosition.x, currentLaneData.m_LaneFlags);
              if (this.m_LaneReservationData.HasComponent(subLane))
              {
                Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
                x1 += this.GetLanePriorityCost(laneReservation.GetPriority());
              }
            }
            float num8 = x1 + this.GetLaneSwitchCost(math.abs(minIndex1 - num2));
            if ((double) num8 < (double) num3)
            {
              num3 = num8;
              entity = subLane;
            }
          }
        }
        else if ((nextNavLaneData.m_Flags & WatercraftLaneFlags.TransformTarget) != (WatercraftLaneFlags) 0 || nextNavLaneData.m_Lane == Entity.Null)
        {
          for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float laneObjectCost2 = this.CalculateLaneObjectCost(laneObjectCost1, subLane, currentLaneData.m_CurvePosition.x, currentLaneData.m_LaneFlags);
            if (this.m_LaneReservationData.HasComponent(subLane))
            {
              Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
              laneObjectCost2 += this.GetLanePriorityCost(laneReservation.GetPriority());
            }
            float num9 = laneObjectCost2 + this.GetLaneSwitchCost(math.abs(minIndex - num2));
            if ((double) num9 < (double) num3)
            {
              num3 = num9;
              entity = subLane;
            }
          }
        }
        else
        {
          int x = 100000;
          int num10 = -100000;
          if ((nextNavLaneData.m_Flags & WatercraftLaneFlags.GroupTarget) != (WatercraftLaneFlags) 0)
          {
            for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              if (lane1[minIndex].m_SubLane == nextNavLaneData.m_Lane)
              {
                x = minIndex;
                num10 = minIndex;
                break;
              }
            }
          }
          else
          {
            Lane lane5 = this.m_LaneData[nextNavLaneData.m_Lane];
            for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              if (this.m_LaneData[lane1[minIndex].m_SubLane].m_EndNode.Equals(lane5.m_StartNode))
              {
                x = math.min(x, minIndex);
                num10 = minIndex;
              }
            }
          }
          for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float num11;
            if (minIndex >= x && minIndex <= num10 || x > num10)
            {
              num11 = this.CalculateLaneObjectCost(laneObjectCost1, subLane, currentLaneData.m_CurvePosition.x, currentLaneData.m_LaneFlags);
              if (this.m_LaneReservationData.HasComponent(subLane))
              {
                Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
                num11 += this.GetLanePriorityCost(laneReservation.GetPriority());
              }
            }
            else
              num11 = 1000000f;
            float num12 = num11 + this.GetLaneSwitchCost(math.abs(minIndex - num2));
            if ((double) num12 < (double) num3)
            {
              num3 = num12;
              entity = subLane;
            }
          }
        }
      }
      if (entity != currentLaneData.m_Lane)
      {
        if (entity != currentLaneData.m_ChangeLane)
        {
          currentLaneData.m_ChangeLane = entity;
          currentLaneData.m_ChangeProgress = 0.0f;
        }
      }
      else if (currentLaneData.m_ChangeLane != Entity.Null)
      {
        if ((double) currentLaneData.m_ChangeProgress == 0.0)
        {
          currentLaneData.m_ChangeLane = Entity.Null;
        }
        else
        {
          currentLaneData.m_Lane = currentLaneData.m_ChangeLane;
          currentLaneData.m_ChangeLane = entity;
          currentLaneData.m_ChangeProgress = math.saturate(1f - currentLaneData.m_ChangeProgress);
        }
      }
      this.m_PrevLane = !(currentLaneData.m_ChangeLane == Entity.Null) ? currentLaneData.m_ChangeLane : currentLaneData.m_Lane;
      this.m_LaneSwitchCost = 1E+07f;
    }

    public void UpdateOptimalLane(ref WatercraftNavigationLane navLaneData)
    {
      if (this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
      {
        SlaveLane slaveLane = this.m_SlaveLaneData[navLaneData.m_Lane];
        if ((navLaneData.m_Flags & (WatercraftLaneFlags.FixedStart | WatercraftLaneFlags.Reserved | WatercraftLaneFlags.FixedLane)) == (WatercraftLaneFlags) 0 && this.m_LaneData.HasComponent(this.m_PrevLane))
        {
          DynamicBuffer<SubLane> lane1 = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
          int num1 = math.min((int) slaveLane.m_MaxIndex, lane1.Length - 1);
          this.m_BufferPos -= num1 - (int) slaveLane.m_MinIndex + 1;
          int x = 100000;
          int num2 = -100000;
          if ((navLaneData.m_Flags & WatercraftLaneFlags.GroupTarget) == (WatercraftLaneFlags) 0)
          {
            Lane lane2 = this.m_LaneData[this.m_PrevLane];
            for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              Lane lane3 = this.m_LaneData[lane1[minIndex].m_SubLane];
              if (lane2.m_EndNode.Equals(lane3.m_StartNode))
              {
                x = math.min(x, minIndex);
                num2 = minIndex;
              }
            }
          }
          if (x > num2)
          {
            x = (int) slaveLane.m_MinIndex;
            num2 = num1;
          }
          int bufferPos = this.m_BufferPos;
          float num3 = float.MaxValue;
          int index1 = (int) slaveLane.m_MinIndex;
          for (int minIndex = (int) slaveLane.m_MinIndex; minIndex < x; ++minIndex)
          {
            float num4 = this.m_Buffer[bufferPos++] + this.GetLaneSwitchCost(x - minIndex);
            if ((double) num4 < (double) num3)
            {
              num3 = num4;
              index1 = minIndex;
            }
          }
          for (int index2 = x; index2 <= num2; ++index2)
          {
            float num5 = this.m_Buffer[bufferPos++];
            if ((double) num5 < (double) num3)
            {
              num3 = num5;
              index1 = index2;
            }
          }
          for (int index3 = num2 + 1; index3 <= num1; ++index3)
          {
            float num6 = this.m_Buffer[bufferPos++] + this.GetLaneSwitchCost(index3 - num2);
            if ((double) num6 < (double) num3)
            {
              num3 = num6;
              index1 = index3;
            }
          }
          navLaneData.m_Lane = lane1[index1].m_SubLane;
        }
        this.m_LaneSwitchCost = this.m_LaneSwitchBaseCost + math.select(1f, 5f, (slaveLane.m_Flags & SlaveLaneFlags.AllowChange) == (SlaveLaneFlags) 0);
      }
      else
        this.m_LaneSwitchCost = 1E+07f;
      this.m_PrevLane = navLaneData.m_Lane;
      this.m_LaneSwitchBaseCost -= 0.01f;
    }

    public void DrawLaneCosts(
      WatercraftCurrentLane currentLaneData,
      WatercraftNavigationLane nextNavLaneData,
      ComponentLookup<Curve> curveData,
      GizmoBatcher gizmoBatcher)
    {
      if ((double) currentLaneData.m_ChangeProgress == 0.0 || currentLaneData.m_ChangeLane == Entity.Null)
        this.m_PrevLane = currentLaneData.m_Lane;
      else
        this.m_PrevLane = currentLaneData.m_ChangeLane;
    }

    public void DrawLaneCosts(
      WatercraftNavigationLane navLaneData,
      ComponentLookup<Curve> curveData,
      GizmoBatcher gizmoBatcher)
    {
      if (this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
      {
        SlaveLane slaveLane = this.m_SlaveLaneData[navLaneData.m_Lane];
        DynamicBuffer<SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
        int num = math.min((int) slaveLane.m_MaxIndex, lane.Length - 1);
        if ((navLaneData.m_Flags & (WatercraftLaneFlags.Reserved | WatercraftLaneFlags.FixedLane)) == (WatercraftLaneFlags) 0)
        {
          this.m_BufferPos -= num - (int) slaveLane.m_MinIndex + 1;
          int bufferPos = this.m_BufferPos;
          for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= num; ++minIndex)
          {
            float cost = this.m_Buffer[bufferPos++];
            this.DrawLane(lane[minIndex].m_SubLane, navLaneData.m_CurvePosition, curveData, gizmoBatcher, cost);
          }
        }
        else
        {
          for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= num; ++minIndex)
          {
            Entity subLane = lane[minIndex].m_SubLane;
            float cost = math.select(1000000f, 0.0f, subLane == navLaneData.m_Lane);
            this.DrawLane(subLane, navLaneData.m_CurvePosition, curveData, gizmoBatcher, cost);
          }
        }
      }
      this.m_PrevLane = navLaneData.m_Lane;
    }

    private void DrawLane(
      Entity lane,
      float2 curvePos,
      ComponentLookup<Curve> curveData,
      GizmoBatcher gizmoBatcher,
      float cost)
    {
      Curve curve = curveData[lane];
      UnityEngine.Color color;
      if ((double) cost >= 100000.0)
      {
        color = UnityEngine.Color.black;
      }
      else
      {
        cost = math.sqrt(cost);
        color = (double) cost >= 2.0 ? UnityEngine.Color.Lerp(UnityEngine.Color.yellow, UnityEngine.Color.red, (float) (((double) cost - 2.0) * 0.5)) : UnityEngine.Color.Lerp(UnityEngine.Color.cyan, UnityEngine.Color.yellow, cost * 0.5f);
      }
      Bezier4x3 bezier = MathUtils.Cut(curve.m_Bezier, curvePos);
      float length = curve.m_Length * math.abs(curvePos.y - curvePos.x);
      gizmoBatcher.DrawCurve(bezier, length, color);
    }
  }
}
