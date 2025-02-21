// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarLaneSelectIterator
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
  public struct CarLaneSelectIterator
  {
    public ComponentLookup<Owner> m_OwnerData;
    public ComponentLookup<Lane> m_LaneData;
    public ComponentLookup<CarLane> m_CarLaneData;
    public ComponentLookup<SlaveLane> m_SlaveLaneData;
    public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Car> m_CarData;
    public ComponentLookup<Controller> m_ControllerData;
    public BufferLookup<SubLane> m_Lanes;
    public BufferLookup<LaneObject> m_LaneObjects;
    public Entity m_Entity;
    public Entity m_Blocker;
    public int m_Priority;
    public Game.Net.CarLaneFlags m_ForbidLaneFlags;
    public Game.Net.CarLaneFlags m_PreferLaneFlags;
    private NativeArray<float> m_Buffer;
    private int m_BufferPos;
    private float m_LaneSwitchCost;
    private float m_LaneSwitchBaseCost;
    private Entity m_PrevLane;

    public void SetBuffer(ref CarLaneSelectBuffer buffer) => this.m_Buffer = buffer.Ensure();

    public void CalculateLaneCosts(CarNavigationLane navLaneData, int index)
    {
      if ((navLaneData.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.FixedLane)) != (Game.Vehicles.CarLaneFlags) 0 || !this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
        return;
      SlaveLane slaveLane = this.m_SlaveLaneData[navLaneData.m_Lane];
      DynamicBuffer<SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
      int num = math.min((int) slaveLane.m_MaxIndex, lane.Length - 1);
      float laneObjectCost1 = math.abs(navLaneData.m_CurvePosition.y - navLaneData.m_CurvePosition.x) * 0.49f;
      for (int minIndex = (int) slaveLane.m_MinIndex; minIndex <= num; ++minIndex)
      {
        Entity subLane = lane[minIndex].m_SubLane;
        float laneObjectCost2 = this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
        if (this.m_LaneReservationData.HasComponent(subLane))
        {
          Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[subLane];
          laneObjectCost2 += this.GetLanePriorityCost(laneReservation.GetPriority());
        }
        if (this.m_CarLaneData.HasComponent(subLane))
        {
          CarLane carLane = this.m_CarLaneData[subLane];
          laneObjectCost2 += this.GetLaneDriveCost(carLane.m_Flags);
        }
        this.m_Buffer[this.m_BufferPos++] = laneObjectCost2;
      }
    }

    private float CalculateLaneObjectCost(
      float laneObjectCost,
      int index,
      Entity lane,
      Game.Vehicles.CarLaneFlags laneFlags)
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
      Game.Vehicles.CarLaneFlags laneFlags)
    {
      float laneObjectCost1 = 0.0f;
      if (this.m_LaneObjects.HasBuffer(lane))
      {
        DynamicBuffer<LaneObject> laneObject1 = this.m_LaneObjects[lane];
        for (int index = 0; index < laneObject1.Length; ++index)
        {
          LaneObject laneObject2 = laneObject1[index];
          if ((double) laneObject2.m_CurvePosition.y > (double) minCurvePosition && !(laneObject2.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.HasComponent(laneObject2.m_LaneObject) || !(this.m_ControllerData[laneObject2.m_LaneObject].m_Controller == this.m_Entity)))
            laneObjectCost1 += this.CalculateLaneObjectCost(laneObject2, laneObjectCost, laneFlags);
        }
      }
      return laneObjectCost1;
    }

    private float CalculateLaneObjectCost(
      LaneObject laneObject,
      float laneObjectCost,
      Game.Vehicles.CarLaneFlags laneFlags)
    {
      return !this.m_MovingData.HasComponent(laneObject.m_LaneObject) && (!this.m_CarData.HasComponent(laneObject.m_LaneObject) || (this.m_CarData[laneObject.m_LaneObject].m_Flags & CarFlags.Queueing) == (CarFlags) 0 || (laneFlags & Game.Vehicles.CarLaneFlags.Queue) == (Game.Vehicles.CarLaneFlags) 0) ? math.lerp(1E+07f, 9000000f, laneObject.m_CurvePosition.y) : laneObjectCost;
    }

    public void CalculateLaneCosts(
      CarNavigationLane navLaneData,
      CarNavigationLane nextNavLaneData,
      int index)
    {
      SlaveLane componentData1;
      if ((navLaneData.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.FixedLane)) == (Game.Vehicles.CarLaneFlags) 0 && this.m_SlaveLaneData.TryGetComponent(navLaneData.m_Lane, out componentData1))
      {
        DynamicBuffer<SubLane> lane1 = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
        int num1 = math.min((int) componentData1.m_MaxIndex, lane1.Length - 1);
        this.m_LaneSwitchCost = this.m_LaneSwitchBaseCost + math.select(1f, 5f, (componentData1.m_Flags & SlaveLaneFlags.AllowChange) == (SlaveLaneFlags) 0);
        float laneObjectCost1 = (float) ((double) math.abs(navLaneData.m_CurvePosition.y - navLaneData.m_CurvePosition.x) * 0.49000000953674316 * (2.0 / (double) (2 + index)));
        SlaveLane componentData2;
        if ((nextNavLaneData.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.FixedLane)) == (Game.Vehicles.CarLaneFlags) 0 && this.m_SlaveLaneData.TryGetComponent(nextNavLaneData.m_Lane, out componentData2))
        {
          DynamicBuffer<SubLane> lane2 = this.m_Lanes[this.m_OwnerData[nextNavLaneData.m_Lane].m_Owner];
          int num2 = math.min((int) componentData2.m_MaxIndex, lane2.Length - 1);
          int num3 = this.m_BufferPos - (num2 - (int) componentData2.m_MinIndex + 1);
          for (int minIndex1 = (int) componentData1.m_MinIndex; minIndex1 <= num1; ++minIndex1)
          {
            Entity subLane = lane1[minIndex1].m_SubLane;
            Lane lane3 = this.m_LaneData[subLane];
            float x1 = 1000000f;
            int x2;
            int num4;
            if ((nextNavLaneData.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) != (Game.Vehicles.CarLaneFlags) 0)
            {
              x2 = (int) componentData2.m_MinIndex;
              num4 = num2;
            }
            else
            {
              x2 = 100000;
              num4 = -100000;
              if ((componentData1.m_Flags & SlaveLaneFlags.MiddleEnd) != (SlaveLaneFlags) 0)
              {
                for (int minIndex2 = (int) componentData2.m_MinIndex; minIndex2 <= num2; ++minIndex2)
                {
                  Lane lane4 = this.m_LaneData[lane2[minIndex2].m_SubLane];
                  if (lane3.m_EndNode.EqualsIgnoreCurvePos(lane4.m_MiddleNode))
                  {
                    x2 = math.min(x2, minIndex2);
                    num4 = minIndex2;
                  }
                }
              }
              else if ((componentData2.m_Flags & SlaveLaneFlags.MiddleStart) != (SlaveLaneFlags) 0)
              {
                for (int minIndex3 = (int) componentData2.m_MinIndex; minIndex3 <= num2; ++minIndex3)
                {
                  Lane lane5 = this.m_LaneData[lane2[minIndex3].m_SubLane];
                  if (lane3.m_MiddleNode.EqualsIgnoreCurvePos(lane5.m_StartNode))
                  {
                    x2 = math.min(x2, minIndex3);
                    num4 = minIndex3;
                  }
                }
              }
              else
              {
                for (int minIndex4 = (int) componentData2.m_MinIndex; minIndex4 <= num2; ++minIndex4)
                {
                  Lane lane6 = this.m_LaneData[lane2[minIndex4].m_SubLane];
                  if (lane3.m_EndNode.Equals(lane6.m_StartNode))
                  {
                    x2 = math.min(x2, minIndex4);
                    num4 = minIndex4;
                  }
                }
              }
            }
            if (x2 <= num4)
            {
              int num5 = num3;
              for (int minIndex5 = (int) componentData2.m_MinIndex; minIndex5 < x2; ++minIndex5)
                x1 = math.min(x1, this.m_Buffer[num5++] + this.GetLaneSwitchCost(x2 - minIndex5));
              for (int index1 = x2; index1 <= num4; ++index1)
                x1 = math.min(x1, this.m_Buffer[num5++]);
              for (int index2 = num4 + 1; index2 <= num2; ++index2)
                x1 = math.min(x1, this.m_Buffer[num5++] + this.GetLaneSwitchCost(index2 - num4));
              x1 += this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
              Game.Net.LaneReservation componentData3;
              if (this.m_LaneReservationData.TryGetComponent(subLane, out componentData3))
                x1 += this.GetLanePriorityCost(componentData3.GetPriority());
              CarLane componentData4;
              if (this.m_CarLaneData.TryGetComponent(subLane, out componentData4))
                x1 += this.GetLaneDriveCost(componentData4.m_Flags);
            }
            this.m_Buffer[this.m_BufferPos++] = x1;
          }
        }
        else if ((nextNavLaneData.m_Flags & Game.Vehicles.CarLaneFlags.TransformTarget) != (Game.Vehicles.CarLaneFlags) 0)
        {
          for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float laneObjectCost2 = this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
            Game.Net.LaneReservation componentData5;
            if (this.m_LaneReservationData.TryGetComponent(subLane, out componentData5))
              laneObjectCost2 += this.GetLanePriorityCost(componentData5.GetPriority());
            CarLane componentData6;
            if (this.m_CarLaneData.TryGetComponent(subLane, out componentData6))
              laneObjectCost2 += this.GetLaneDriveCost(componentData6.m_Flags);
            this.m_Buffer[this.m_BufferPos++] = laneObjectCost2;
          }
        }
        else
        {
          int x = 100000;
          int num6 = -100000;
          if ((nextNavLaneData.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) != (Game.Vehicles.CarLaneFlags) 0)
          {
            for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
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
            Lane lane7 = this.m_LaneData[nextNavLaneData.m_Lane];
            for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              Lane lane8 = this.m_LaneData[lane1[minIndex].m_SubLane];
              if ((componentData1.m_Flags & SlaveLaneFlags.MiddleEnd) != (SlaveLaneFlags) 0)
              {
                if (lane8.m_EndNode.EqualsIgnoreCurvePos(lane7.m_MiddleNode))
                {
                  x = math.min(x, minIndex);
                  num6 = minIndex;
                }
              }
              else if (lane8.m_EndNode.Equals(lane7.m_StartNode))
              {
                x = math.min(x, minIndex);
                num6 = minIndex;
              }
            }
          }
          for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float num7 = 0.0f;
            if (x <= num6)
              num7 += this.GetLaneSwitchCost(math.max(0, math.max(x - minIndex, minIndex - num6)));
            float num8 = num7 + this.CalculateLaneObjectCost(laneObjectCost1, index, subLane, navLaneData.m_Flags);
            Game.Net.LaneReservation componentData7;
            if (this.m_LaneReservationData.TryGetComponent(subLane, out componentData7))
              num8 += this.GetLanePriorityCost(componentData7.GetPriority());
            CarLane componentData8;
            if (this.m_CarLaneData.TryGetComponent(subLane, out componentData8))
              num8 += this.GetLaneDriveCost(componentData8.m_Flags);
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

    private float GetLaneDriveCost(Game.Net.CarLaneFlags flags)
    {
      return math.select(math.select(0.0f, 0.4f, (flags & this.m_PreferLaneFlags) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) & this.m_PreferLaneFlags > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter)), math.select(0.9f, 4.9f, this.m_Priority < 108), (flags & this.m_ForbidLaneFlags) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter));
    }

    public void UpdateOptimalLane(ref CarCurrentLane currentLane, CarNavigationLane nextNavLaneData)
    {
      Entity entity = currentLane.m_ChangeLane != Entity.Null ? currentLane.m_ChangeLane : currentLane.m_Lane;
      int2 int2_1 = (int2) 0;
      int changeIndex = 0;
      SlaveLane componentData1;
      if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.FixedLane) == (Game.Vehicles.CarLaneFlags) 0 && this.m_SlaveLaneData.TryGetComponent(entity, out componentData1))
      {
        DynamicBuffer<SubLane> lane1 = this.m_Lanes[this.m_OwnerData[entity].m_Owner];
        int num1 = math.min((int) componentData1.m_MaxIndex, lane1.Length - 1);
        this.m_LaneSwitchCost = this.m_LaneSwitchBaseCost + math.select(1f, 5f, (componentData1.m_Flags & SlaveLaneFlags.AllowChange) == (SlaveLaneFlags) 0);
        float laneObjectCost1 = 0.49f;
        for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
        {
          if (lane1[minIndex].m_SubLane == entity)
          {
            int2_1 = (int2) minIndex;
            break;
          }
        }
        if (currentLane.m_ChangeLane != Entity.Null)
        {
          for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            if (lane1[minIndex].m_SubLane == currentLane.m_Lane)
            {
              int2_1.y = minIndex;
              break;
            }
          }
        }
        float num2 = float.MaxValue;
        SlaveLane componentData2;
        if ((nextNavLaneData.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.FixedLane)) == (Game.Vehicles.CarLaneFlags) 0 && this.m_SlaveLaneData.TryGetComponent(nextNavLaneData.m_Lane, out componentData2))
        {
          DynamicBuffer<SubLane> lane2 = this.m_Lanes[this.m_OwnerData[nextNavLaneData.m_Lane].m_Owner];
          int num3 = math.min((int) componentData2.m_MaxIndex, lane2.Length - 1);
          int num4 = this.m_BufferPos - (num3 - (int) componentData2.m_MinIndex + 1);
          for (int minIndex1 = (int) componentData1.m_MinIndex; minIndex1 <= num1; ++minIndex1)
          {
            Entity subLane = lane1[minIndex1].m_SubLane;
            Lane lane3 = this.m_LaneData[subLane];
            float x1 = 1000000f;
            int x2;
            int num5;
            if ((nextNavLaneData.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) != (Game.Vehicles.CarLaneFlags) 0)
            {
              x2 = (int) componentData2.m_MinIndex;
              num5 = num3;
            }
            else
            {
              x2 = 100000;
              num5 = -100000;
              if ((componentData1.m_Flags & SlaveLaneFlags.MiddleEnd) != (SlaveLaneFlags) 0)
              {
                for (int minIndex2 = (int) componentData2.m_MinIndex; minIndex2 <= num3; ++minIndex2)
                {
                  Lane lane4 = this.m_LaneData[lane2[minIndex2].m_SubLane];
                  if (lane3.m_EndNode.EqualsIgnoreCurvePos(lane4.m_MiddleNode))
                  {
                    x2 = math.min(x2, minIndex2);
                    num5 = minIndex2;
                  }
                }
              }
              else if ((componentData2.m_Flags & SlaveLaneFlags.MiddleStart) != (SlaveLaneFlags) 0)
              {
                for (int minIndex3 = (int) componentData2.m_MinIndex; minIndex3 <= num3; ++minIndex3)
                {
                  Lane lane5 = this.m_LaneData[lane2[minIndex3].m_SubLane];
                  if (lane3.m_MiddleNode.EqualsIgnoreCurvePos(lane5.m_StartNode))
                  {
                    x2 = math.min(x2, minIndex3);
                    num5 = minIndex3;
                  }
                }
              }
              else
              {
                for (int minIndex4 = (int) componentData2.m_MinIndex; minIndex4 <= num3; ++minIndex4)
                {
                  Lane lane6 = this.m_LaneData[lane2[minIndex4].m_SubLane];
                  if (lane3.m_EndNode.Equals(lane6.m_StartNode))
                  {
                    x2 = math.min(x2, minIndex4);
                    num5 = minIndex4;
                  }
                }
              }
            }
            if (x2 <= num5)
            {
              int num6 = num4 + (x2 - (int) componentData2.m_MinIndex);
              for (int index = x2; index <= num5; ++index)
                x1 = math.min(x1, this.m_Buffer[num6++]);
              x1 += this.CalculateLaneObjectCost(laneObjectCost1, subLane, currentLane.m_CurvePosition.x, currentLane.m_LaneFlags);
              Game.Net.LaneReservation componentData3;
              if (this.m_LaneReservationData.TryGetComponent(subLane, out componentData3))
                x1 += this.GetLanePriorityCost(componentData3.GetPriority());
            }
            int2 int2_2 = math.abs(minIndex1 - int2_1);
            float num7 = x1 + this.GetLaneSwitchCost(math.select(int2_2.x, int2_2.y, int2_2.x != 0 && int2_2.y > int2_2.x));
            if ((double) num7 < (double) num2)
            {
              num2 = num7;
              entity = subLane;
              changeIndex = minIndex1;
            }
          }
        }
        else if ((nextNavLaneData.m_Flags & Game.Vehicles.CarLaneFlags.TransformTarget) != (Game.Vehicles.CarLaneFlags) 0 || nextNavLaneData.m_Lane == Entity.Null)
        {
          for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float laneObjectCost2 = this.CalculateLaneObjectCost(laneObjectCost1, subLane, currentLane.m_CurvePosition.x, currentLane.m_LaneFlags);
            Game.Net.LaneReservation componentData4;
            if (this.m_LaneReservationData.TryGetComponent(subLane, out componentData4))
              laneObjectCost2 += this.GetLanePriorityCost(componentData4.GetPriority());
            int2 int2_3 = math.abs(minIndex - int2_1);
            float num8 = laneObjectCost2 + this.GetLaneSwitchCost(math.select(int2_3.x, int2_3.y, int2_3.x != 0 && int2_3.y > int2_3.x));
            if ((double) num8 < (double) num2)
            {
              num2 = num8;
              entity = subLane;
              changeIndex = minIndex;
            }
          }
        }
        else
        {
          int x = 100000;
          int num9 = -100000;
          if ((nextNavLaneData.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) != (Game.Vehicles.CarLaneFlags) 0)
          {
            for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              if (lane1[minIndex].m_SubLane == nextNavLaneData.m_Lane)
              {
                x = minIndex;
                num9 = minIndex;
                break;
              }
            }
          }
          else
          {
            Lane lane7 = this.m_LaneData[nextNavLaneData.m_Lane];
            for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
            {
              Lane lane8 = this.m_LaneData[lane1[minIndex].m_SubLane];
              if ((componentData1.m_Flags & SlaveLaneFlags.MiddleEnd) != (SlaveLaneFlags) 0)
              {
                if (lane8.m_EndNode.EqualsIgnoreCurvePos(lane7.m_MiddleNode))
                {
                  x = math.min(x, minIndex);
                  num9 = minIndex;
                }
              }
              else if (lane8.m_EndNode.Equals(lane7.m_StartNode))
              {
                x = math.min(x, minIndex);
                num9 = minIndex;
              }
            }
          }
          for (int minIndex = (int) componentData1.m_MinIndex; minIndex <= num1; ++minIndex)
          {
            Entity subLane = lane1[minIndex].m_SubLane;
            float num10;
            if (minIndex >= x && minIndex <= num9 || x > num9)
            {
              num10 = this.CalculateLaneObjectCost(laneObjectCost1, subLane, currentLane.m_CurvePosition.x, currentLane.m_LaneFlags);
              Game.Net.LaneReservation componentData5;
              if (this.m_LaneReservationData.TryGetComponent(subLane, out componentData5))
                num10 += this.GetLanePriorityCost(componentData5.GetPriority());
            }
            else
              num10 = 1000000f;
            int2 int2_4 = math.abs(minIndex - int2_1);
            float num11 = num10 + this.GetLaneSwitchCost(math.select(int2_4.x, int2_4.y, int2_4.x != 0 && int2_4.y > int2_4.x));
            if ((double) num11 < (double) num2)
            {
              num2 = num11;
              entity = subLane;
              changeIndex = minIndex;
            }
          }
        }
      }
      if (entity != currentLane.m_Lane)
      {
        if (entity != currentLane.m_ChangeLane)
        {
          currentLane.m_ChangeLane = entity;
          currentLane.m_ChangeProgress = 0.0f;
          currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
          currentLane.m_LaneFlags |= this.GetTurnFlags(currentLane.m_Lane, int2_1.y, changeIndex);
        }
      }
      else if (currentLane.m_ChangeLane != Entity.Null)
      {
        currentLane.m_LaneFlags &= ~(Game.Vehicles.CarLaneFlags.TurnLeft | Game.Vehicles.CarLaneFlags.TurnRight);
        if ((double) currentLane.m_ChangeProgress == 0.0)
        {
          currentLane.m_ChangeLane = Entity.Null;
        }
        else
        {
          currentLane.m_Lane = currentLane.m_ChangeLane;
          currentLane.m_ChangeLane = entity;
          currentLane.m_ChangeProgress = math.saturate(1f - currentLane.m_ChangeProgress);
          currentLane.m_LaneFlags |= this.GetTurnFlags(currentLane.m_Lane, int2_1.y, changeIndex);
        }
      }
      this.m_PrevLane = !(currentLane.m_ChangeLane == Entity.Null) ? currentLane.m_ChangeLane : currentLane.m_Lane;
      this.m_LaneSwitchCost = 1E+07f;
    }

    private Game.Vehicles.CarLaneFlags GetTurnFlags(
      Entity currentLane,
      int currentIndex,
      int changeIndex)
    {
      if (changeIndex == currentIndex)
        return (Game.Vehicles.CarLaneFlags) 0;
      bool flag = false;
      CarLane componentData;
      if (this.m_CarLaneData.TryGetComponent(currentLane, out componentData))
        flag = (componentData.m_Flags & Game.Net.CarLaneFlags.Invert) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
      return changeIndex < currentIndex != flag ? Game.Vehicles.CarLaneFlags.TurnLeft : Game.Vehicles.CarLaneFlags.TurnRight;
    }

    public void UpdateOptimalLane(ref CarNavigationLane navLaneData)
    {
      if (this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
      {
        SlaveLane slaveLane1 = this.m_SlaveLaneData[navLaneData.m_Lane];
        if ((navLaneData.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.FixedLane | Game.Vehicles.CarLaneFlags.FixedStart)) == (Game.Vehicles.CarLaneFlags) 0 && this.m_LaneData.HasComponent(this.m_PrevLane))
        {
          DynamicBuffer<SubLane> lane1 = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
          int num1 = math.min((int) slaveLane1.m_MaxIndex, lane1.Length - 1);
          this.m_BufferPos -= num1 - (int) slaveLane1.m_MinIndex + 1;
          int x = 100000;
          int num2 = -100000;
          if ((navLaneData.m_Flags & Game.Vehicles.CarLaneFlags.GroupTarget) == (Game.Vehicles.CarLaneFlags) 0)
          {
            Lane lane2 = this.m_LaneData[this.m_PrevLane];
            SlaveLane slaveLane2 = new SlaveLane();
            if (this.m_SlaveLaneData.HasComponent(this.m_PrevLane))
              slaveLane2 = this.m_SlaveLaneData[this.m_PrevLane];
            if ((slaveLane2.m_Flags & SlaveLaneFlags.MiddleEnd) != (SlaveLaneFlags) 0)
            {
              for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
              {
                Lane lane3 = this.m_LaneData[lane1[minIndex].m_SubLane];
                if (lane2.m_EndNode.EqualsIgnoreCurvePos(lane3.m_MiddleNode))
                {
                  x = math.min(x, minIndex);
                  num2 = minIndex;
                }
              }
            }
            else if ((slaveLane1.m_Flags & SlaveLaneFlags.MiddleStart) != (SlaveLaneFlags) 0)
            {
              for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
              {
                Lane lane4 = this.m_LaneData[lane1[minIndex].m_SubLane];
                if (lane2.m_MiddleNode.EqualsIgnoreCurvePos(lane4.m_StartNode))
                {
                  x = math.min(x, minIndex);
                  num2 = minIndex;
                }
              }
            }
            else
            {
              for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex <= num1; ++minIndex)
              {
                Lane lane5 = this.m_LaneData[lane1[minIndex].m_SubLane];
                if (lane2.m_EndNode.Equals(lane5.m_StartNode))
                {
                  x = math.min(x, minIndex);
                  num2 = minIndex;
                }
              }
            }
          }
          if (x > num2)
          {
            x = (int) slaveLane1.m_MinIndex;
            num2 = num1;
          }
          int bufferPos = this.m_BufferPos;
          float num3 = float.MaxValue;
          int index1 = (int) slaveLane1.m_MinIndex;
          for (int minIndex = (int) slaveLane1.m_MinIndex; minIndex < x; ++minIndex)
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
        this.m_LaneSwitchCost = this.m_LaneSwitchBaseCost + math.select(1f, 5f, (slaveLane1.m_Flags & SlaveLaneFlags.AllowChange) == (SlaveLaneFlags) 0);
      }
      else
        this.m_LaneSwitchCost = 1E+07f;
      this.m_PrevLane = navLaneData.m_Lane;
      this.m_LaneSwitchBaseCost -= 0.01f;
    }

    public void DrawLaneCosts(
      CarCurrentLane currentLaneData,
      CarNavigationLane nextNavLaneData,
      ComponentLookup<Curve> curveData,
      GizmoBatcher gizmoBatcher)
    {
      if ((double) currentLaneData.m_ChangeProgress == 0.0 || currentLaneData.m_ChangeLane == Entity.Null)
        this.m_PrevLane = currentLaneData.m_Lane;
      else
        this.m_PrevLane = currentLaneData.m_ChangeLane;
    }

    public void DrawLaneCosts(
      CarNavigationLane navLaneData,
      ComponentLookup<Curve> curveData,
      GizmoBatcher gizmoBatcher)
    {
      if (this.m_SlaveLaneData.HasComponent(navLaneData.m_Lane))
      {
        SlaveLane slaveLane = this.m_SlaveLaneData[navLaneData.m_Lane];
        DynamicBuffer<SubLane> lane = this.m_Lanes[this.m_OwnerData[navLaneData.m_Lane].m_Owner];
        int num = math.min((int) slaveLane.m_MaxIndex, lane.Length - 1);
        if ((navLaneData.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.FixedLane)) == (Game.Vehicles.CarLaneFlags) 0)
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
