// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarLaneSpeedIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct CarLaneSpeedIterator
  {
    public ComponentLookup<Transform> m_TransformData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Car> m_CarData;
    public ComponentLookup<Train> m_TrainData;
    public ComponentLookup<Controller> m_ControllerData;
    public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
    public ComponentLookup<LaneCondition> m_LaneConditionData;
    public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
    public ComponentLookup<Curve> m_CurveData;
    public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
    public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
    public ComponentLookup<Unspawned> m_UnspawnedData;
    public ComponentLookup<Creature> m_CreatureData;
    public ComponentLookup<PrefabRef> m_PrefabRefData;
    public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
    public ComponentLookup<CarData> m_PrefabCarData;
    public ComponentLookup<TrainData> m_PrefabTrainData;
    public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
    public BufferLookup<LaneOverlap> m_LaneOverlapData;
    public BufferLookup<LaneObject> m_LaneObjectData;
    public Entity m_Entity;
    public Entity m_Ignore;
    public NativeList<Entity> m_TempBuffer;
    public int m_Priority;
    public float m_TimeStep;
    public float m_SafeTimeStep;
    public float m_DistanceOffset;
    public float m_SpeedLimitFactor;
    public float m_CurrentSpeed;
    public CarData m_PrefabCar;
    public ObjectGeometryData m_PrefabObjectGeometry;
    public Bounds1 m_SpeedRange;
    public bool m_PushBlockers;
    public float m_MaxSpeed;
    public float m_CanChangeLane;
    public float3 m_CurrentPosition;
    public float m_Oncoming;
    public Entity m_Blocker;
    public BlockerType m_BlockerType;
    private Entity m_Lane;
    private Entity m_NextLane;
    private Curve m_Curve;
    private float2 m_CurveOffset;
    private float2 m_NextOffset;
    private float3 m_PrevPosition;
    private float m_PrevDistance;
    private float m_Distance;

    public bool IterateFirstLane(
      Entity lane,
      float3 curveOffset,
      Entity nextLane,
      float2 nextOffset,
      float laneOffset,
      bool requestSpace,
      out Game.Net.CarLaneFlags laneFlags)
    {
      Curve curve = this.m_CurveData[lane];
      bool flag = (double) curveOffset.z < (double) curveOffset.x;
      laneOffset = math.select(laneOffset, -laneOffset, flag);
      laneFlags = ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
      float3 x = MathUtils.Position(curve.m_Bezier, curveOffset.x);
      float3 lanePosition = VehicleUtils.GetLanePosition(curve.m_Bezier, curveOffset.x, laneOffset);
      this.m_PrevPosition = this.m_CurrentPosition;
      this.m_Distance = math.distance(this.m_CurrentPosition, lanePosition);
      Game.Net.CarLane componentData;
      if (this.m_CarLaneData.TryGetComponent(lane, out componentData))
      {
        componentData.m_SpeedLimit *= this.m_SpeedLimitFactor;
        laneFlags = componentData.m_Flags;
        float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabCar, componentData);
        int yieldOverride = 0;
        bool isRoundabout = false;
        if ((componentData.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        {
          isRoundabout = (componentData.m_Flags & Game.Net.CarLaneFlags.Roundabout) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
          if ((componentData.m_Flags & (Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.TrafficLights)) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && this.m_LaneSignalData.HasComponent(lane))
          {
            switch (this.m_LaneSignalData[lane].m_Signal)
            {
              case LaneSignalType.Stop:
                yieldOverride = -1;
                break;
              case LaneSignalType.Yield:
                yieldOverride = 1;
                break;
            }
          }
        }
        if (this.m_LaneConditionData.HasComponent(lane))
          VehicleUtils.ModifyDriveSpeed(ref maxDriveSpeed, this.m_LaneConditionData[lane]);
        if (this.m_Priority < 102 && this.m_LaneReservationData.HasComponent(lane) && this.m_LaneReservationData[lane].GetPriority() == 102)
          maxDriveSpeed *= 0.5f;
        if ((double) maxDriveSpeed < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(maxDriveSpeed, this.m_SpeedRange);
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.Limit;
        }
        float2 xy = curveOffset.xy;
        float num1 = this.m_Distance + -this.m_PrefabObjectGeometry.m_Bounds.max.z + this.m_DistanceOffset;
        if ((int) componentData.m_CautionEnd >= (int) componentData.m_CautionStart)
        {
          Bounds1 cautionBounds = componentData.cautionBounds;
          float2 float2 = math.select(curveOffset.xz, curveOffset.zx, flag);
          if ((double) cautionBounds.max > (double) float2.x && (double) cautionBounds.min < (double) float2.y)
          {
            float distance = num1 + curve.m_Length * math.max(0.0f, math.select(cautionBounds.min - float2.x, float2.y - cautionBounds.max, flag));
            float num2 = componentData.m_SpeedLimit * math.select(0.5f, 0.8f, (componentData.m_Flags & Game.Net.CarLaneFlags.IsSecured) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter));
            float position = math.max(num2, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance, num2, this.m_SafeTimeStep));
            if ((double) position < (double) this.m_MaxSpeed)
            {
              this.m_MaxSpeed = MathUtils.Clamp(position, this.m_SpeedRange);
              this.m_Blocker = Entity.Null;
              this.m_BlockerType = BlockerType.Caution;
            }
          }
        }
        this.m_Lane = lane;
        this.m_NextLane = nextLane;
        this.m_Curve = curve;
        this.m_CurveOffset = curveOffset.xz;
        this.m_NextOffset = nextOffset;
        this.m_CurrentPosition = x;
        this.CheckCurrentLane(num1, xy, flag);
        this.CheckOverlappingLanes(num1, xy.y, yieldOverride, componentData.m_SpeedLimit, isRoundabout, flag, requestSpace);
      }
      float3 y = MathUtils.Position(curve.m_Bezier, curveOffset.z);
      float s = math.abs(curveOffset.z - curveOffset.x);
      float num = math.max(1f / 1000f, math.lerp(math.distance(x, y), curve.m_Length * s, s));
      if ((double) num > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y;
      this.m_Distance += num;
      return (double) this.m_Distance + (double) this.m_DistanceOffset - 20.0 >= (double) VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_MaxSpeed, this.m_SafeTimeStep) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public bool IterateFirstLane(
      Entity lane1,
      Entity lane2,
      float3 curveOffset,
      Entity nextLane,
      float2 nextOffset,
      float laneDelta,
      float laneOffset1,
      float laneOffset2,
      bool requestSpace,
      out Game.Net.CarLaneFlags laneFlags)
    {
      laneDelta = math.saturate(laneDelta);
      laneFlags = ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
      Curve curve1 = this.m_CurveData[lane1];
      Curve curve2 = this.m_CurveData[lane2];
      float3 x1 = MathUtils.Position(curve1.m_Bezier, curveOffset.x);
      float3 y1 = MathUtils.Position(curve2.m_Bezier, curveOffset.x);
      float3 x2 = math.lerp(x1, y1, laneDelta);
      float3 y2 = math.lerp(VehicleUtils.GetLanePosition(curve1.m_Bezier, curveOffset.x, laneOffset1), VehicleUtils.GetLanePosition(curve2.m_Bezier, curveOffset.x, laneOffset2), laneDelta);
      this.m_PrevPosition = this.m_CurrentPosition;
      this.m_Distance = math.distance(this.m_CurrentPosition, y2);
      Game.Net.CarLane componentData;
      if (this.m_CarLaneData.TryGetComponent(lane1, out componentData))
      {
        componentData.m_SpeedLimit *= this.m_SpeedLimitFactor;
        laneFlags = componentData.m_Flags;
        float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabCar, componentData);
        int yieldOverride = 0;
        bool isRoundabout = false;
        bool flag = (double) curveOffset.z < (double) curveOffset.x;
        if ((componentData.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        {
          isRoundabout = (componentData.m_Flags & Game.Net.CarLaneFlags.Roundabout) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
          if ((componentData.m_Flags & (Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.TrafficLights)) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && this.m_LaneSignalData.HasComponent(lane1))
          {
            switch (this.m_LaneSignalData[lane1].m_Signal)
            {
              case LaneSignalType.Stop:
                yieldOverride = -1;
                break;
              case LaneSignalType.Yield:
                yieldOverride = 1;
                break;
            }
          }
        }
        if (this.m_LaneConditionData.HasComponent(lane1))
          VehicleUtils.ModifyDriveSpeed(ref maxDriveSpeed, this.m_LaneConditionData[lane1]);
        if (this.m_Priority < 102 && this.m_LaneReservationData.HasComponent(lane1) && this.m_LaneReservationData.HasComponent(lane2))
        {
          if ((double) laneDelta < 0.89999997615814209)
          {
            Game.Net.LaneReservation laneReservation1 = this.m_LaneReservationData[lane1];
            Game.Net.LaneReservation laneReservation2 = this.m_LaneReservationData[lane2];
            if (math.any(new int2(laneReservation1.GetPriority(), laneReservation2.GetPriority()) == 102))
              maxDriveSpeed *= 0.5f;
          }
          else if (this.m_LaneReservationData[lane2].GetPriority() == 102)
            maxDriveSpeed *= 0.5f;
        }
        if ((double) maxDriveSpeed < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(maxDriveSpeed, this.m_SpeedRange);
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.Limit;
        }
        float2 xy = curveOffset.xy;
        float num1 = this.m_Distance + -this.m_PrefabObjectGeometry.m_Bounds.max.z + this.m_DistanceOffset;
        if ((int) componentData.m_CautionEnd >= (int) componentData.m_CautionStart)
        {
          Bounds1 cautionBounds = componentData.cautionBounds;
          float2 float2 = math.select(curveOffset.xz, curveOffset.zx, flag);
          if ((double) cautionBounds.max > (double) float2.x && (double) cautionBounds.min < (double) float2.y)
          {
            float distance = num1 + curve1.m_Length * math.max(0.0f, math.select(cautionBounds.min - float2.x, float2.y - cautionBounds.max, flag));
            float num2 = componentData.m_SpeedLimit * math.select(0.5f, 0.8f, (componentData.m_Flags & Game.Net.CarLaneFlags.IsSecured) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter));
            float position = math.max(num2, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance, num2, this.m_SafeTimeStep));
            if ((double) position < (double) this.m_MaxSpeed)
            {
              this.m_MaxSpeed = MathUtils.Clamp(position, this.m_SpeedRange);
              this.m_Blocker = Entity.Null;
              this.m_BlockerType = BlockerType.Caution;
            }
          }
        }
        if ((double) laneDelta < 0.89999997615814209)
        {
          this.m_Lane = lane1;
          this.m_Curve = curve1;
          this.m_CurveOffset = curveOffset.xz;
          this.m_CurrentPosition = x1;
          this.CheckCurrentLane(num1, xy, flag);
          this.CheckOverlappingLanes(num1, xy.y, yieldOverride, componentData.m_SpeedLimit, isRoundabout, flag, requestSpace);
        }
        this.m_Lane = lane2;
        this.m_NextLane = nextLane;
        this.m_Curve = curve2;
        this.m_CurveOffset = curveOffset.xz;
        this.m_NextOffset = nextOffset;
        this.m_CurrentPosition = y1;
        if ((double) laneDelta == 0.0)
          this.CheckCurrentLane(num1, xy, flag, ref this.m_CanChangeLane);
        else
          this.CheckCurrentLane(num1, xy, flag);
        this.CheckOverlappingLanes(num1, xy.y, 0, componentData.m_SpeedLimit, isRoundabout, flag, requestSpace);
      }
      float3 y3 = MathUtils.Position(curve2.m_Bezier, curveOffset.z);
      float num3 = math.lerp(curve1.m_Length, curve2.m_Length, laneDelta);
      float s = math.abs(curveOffset.z - curveOffset.x);
      float num4 = math.max(1f / 1000f, math.lerp(math.distance(x2, y3), num3 * s, s));
      if ((double) num4 > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y3;
      this.m_Distance += num4;
      return (double) this.m_Distance + (double) this.m_DistanceOffset - 20.0 >= (double) VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_MaxSpeed, this.m_SafeTimeStep) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public bool IterateNextLane(
      Entity lane,
      float2 curveOffset,
      float minOffset,
      NativeArray<CarNavigationLane> nextLanes,
      bool requestSpace,
      ref Game.Net.CarLaneFlags laneFlags,
      out bool needSignal)
    {
      needSignal = false;
      Game.Net.CarLaneFlags carLaneFlags = laneFlags;
      laneFlags = ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
      Curve componentData1;
      if (!this.m_CurveData.TryGetComponent(lane, out componentData1))
        return false;
      Game.Net.CarLane componentData2;
      if (this.m_CarLaneData.TryGetComponent(lane, out componentData2))
      {
        componentData2.m_SpeedLimit *= this.m_SpeedLimitFactor;
        laneFlags = componentData2.m_Flags;
        float driveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabCar, componentData2);
        float num1 = this.m_Distance + -this.m_PrefabObjectGeometry.m_Bounds.max.z;
        int yieldOverride = 0;
        bool isRoundabout = false;
        bool flag = (double) curveOffset.y < (double) curveOffset.x;
        Entity blocker = Entity.Null;
        BlockerType blockerType = BlockerType.Limit;
        if ((componentData2.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        {
          if ((carLaneFlags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          {
            componentData2.m_Flags &= Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter;
            if ((componentData2.m_Flags & Game.Net.CarLaneFlags.SideConnection) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
              componentData2.m_Flags &= Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter;
          }
          isRoundabout = (componentData2.m_Flags & Game.Net.CarLaneFlags.Roundabout) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
          Game.Net.LaneSignal componentData3;
          if ((componentData2.m_Flags & (Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.TrafficLights)) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && this.m_LaneSignalData.TryGetComponent(lane, out componentData3))
          {
            float brakingDistance = VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_CurrentSpeed, 0.0f);
            if (!isRoundabout && (double) brakingDistance <= (double) num1 && !this.CheckSpace(lane, curveOffset, nextLanes, out blocker))
            {
              driveSpeed = 0.0f;
              blockerType = BlockerType.Continuing;
            }
            else
            {
              needSignal = true;
              switch (componentData3.m_Signal)
              {
                case LaneSignalType.Stop:
                  if (this.m_Priority < 108 && (double) brakingDistance <= (double) num1 + 1.0)
                  {
                    driveSpeed = 0.0f;
                    blocker = componentData3.m_Blocker;
                    blockerType = BlockerType.Signal;
                    yieldOverride = 1;
                    break;
                  }
                  yieldOverride = -1;
                  break;
                case LaneSignalType.SafeStop:
                  if (this.m_Priority < 108 && (double) brakingDistance <= (double) num1)
                  {
                    driveSpeed = 0.0f;
                    blocker = componentData3.m_Blocker;
                    blockerType = BlockerType.Signal;
                    break;
                  }
                  break;
                case LaneSignalType.Yield:
                  yieldOverride = 1;
                  break;
              }
            }
          }
          else if ((componentData2.m_Flags & Game.Net.CarLaneFlags.Stop) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          {
            if (this.m_Priority < 108 && (double) num1 >= 1.1000000238418579)
            {
              driveSpeed = 0.0f;
              blockerType = BlockerType.Limit;
            }
            else if (!isRoundabout && (double) VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_CurrentSpeed, 0.0f) <= (double) num1 && !this.CheckSpace(lane, curveOffset, nextLanes, out blocker))
            {
              driveSpeed = 0.0f;
              blockerType = BlockerType.Continuing;
            }
            yieldOverride = 1;
          }
          else if ((componentData2.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward)) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && !isRoundabout && (double) VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_CurrentSpeed, 0.0f) <= (double) num1 && !this.CheckSpace(lane, curveOffset, nextLanes, out blocker))
          {
            driveSpeed = 0.0f;
            blockerType = BlockerType.Continuing;
          }
        }
        float num2 = num1 + this.m_DistanceOffset;
        float position1;
        if ((double) driveSpeed == 0.0)
        {
          position1 = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, math.max(0.0f, num2 - 0.5f), this.m_SafeTimeStep);
        }
        else
        {
          if (this.m_LaneConditionData.HasComponent(lane))
            VehicleUtils.ModifyDriveSpeed(ref driveSpeed, this.m_LaneConditionData[lane]);
          position1 = math.max(driveSpeed, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, this.m_Distance, driveSpeed, this.m_TimeStep));
        }
        if ((double) position1 < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(position1, this.m_SpeedRange);
          this.m_Blocker = blocker;
          this.m_BlockerType = blockerType;
        }
        if ((int) componentData2.m_CautionEnd >= (int) componentData2.m_CautionStart)
        {
          Bounds1 cautionBounds = componentData2.cautionBounds;
          float2 float2 = math.select(curveOffset, curveOffset.yx, flag);
          if ((double) cautionBounds.max > (double) float2.x && (double) cautionBounds.min < (double) float2.y)
          {
            float distance = num2 + componentData1.m_Length * math.max(0.0f, math.select(cautionBounds.min - float2.x, float2.y - cautionBounds.max, flag));
            float num3 = componentData2.m_SpeedLimit * math.select(0.5f, 0.8f, (componentData2.m_Flags & Game.Net.CarLaneFlags.IsSecured) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter));
            float position2 = math.max(num3, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance, num3, this.m_SafeTimeStep));
            if ((double) position2 < (double) this.m_MaxSpeed)
            {
              this.m_MaxSpeed = MathUtils.Clamp(position2, this.m_SpeedRange);
              this.m_Blocker = Entity.Null;
              this.m_BlockerType = BlockerType.Caution;
            }
          }
        }
        this.m_Curve = componentData1;
        this.m_CurveOffset = curveOffset;
        this.m_Lane = lane;
        if (nextLanes.Length != 0)
        {
          CarNavigationLane nextLane = nextLanes[0];
          this.m_NextOffset = nextLane.m_CurvePosition;
          this.m_NextLane = nextLane.m_Lane;
        }
        else
        {
          this.m_NextOffset = (float2) 0.0f;
          this.m_NextLane = Entity.Null;
        }
        minOffset = math.select(minOffset, curveOffset.x, flag ? (double) curveOffset.x < 1.0 : (double) curveOffset.x > 0.0);
        this.CheckCurrentLane(num2, (float2) minOffset, flag);
        this.CheckOverlappingLanes(num2, minOffset, yieldOverride, componentData2.m_SpeedLimit, isRoundabout, flag, requestSpace);
      }
      else if (this.m_ParkingLaneData.HasComponent(lane))
      {
        float distance = this.m_Distance + -this.m_PrefabObjectGeometry.m_Bounds.max.z + this.m_DistanceOffset;
        this.m_Curve = componentData1;
        this.m_CurveOffset = curveOffset;
        this.m_Lane = lane;
        this.CheckParkingLane(distance);
      }
      float3 y = MathUtils.Position(componentData1.m_Bezier, curveOffset.y);
      float s = math.abs(curveOffset.y - curveOffset.x);
      float num = math.max(1f / 1000f, math.lerp(math.distance(this.m_CurrentPosition, y), componentData1.m_Length * s, s));
      if ((double) num > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y;
      this.m_Distance += num;
      return (double) this.m_Distance + (double) this.m_DistanceOffset - 20.0 >= (double) VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_MaxSpeed, this.m_SafeTimeStep) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public void IterateTarget(float3 targetPosition)
    {
      float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabCar, 11.1111116f, 0.2617994f);
      this.IterateTarget(targetPosition, maxDriveSpeed);
    }

    public void IterateTarget(float3 targetPosition, float maxLaneSpeed)
    {
      float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, this.m_Distance, maxLaneSpeed, this.m_TimeStep);
      this.m_Distance += math.distance(this.m_CurrentPosition, targetPosition);
      float position = math.min(maxBrakingSpeed, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, this.m_Distance, this.m_TimeStep));
      if ((double) position >= (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = MathUtils.Clamp(position, this.m_SpeedRange);
      this.m_Blocker = Entity.Null;
      this.m_BlockerType = BlockerType.None;
    }

    private bool CheckSpace(
      Entity currentLane,
      float2 curveOffset,
      NativeArray<CarNavigationLane> nextLanes,
      out Entity blocker)
    {
      blocker = Entity.Null;
      if (nextLanes.Length == 0)
        return true;
      CarNavigationLane nextLane = nextLanes[0];
      bool c = (double) nextLane.m_CurvePosition.y < (double) nextLane.m_CurvePosition.x;
      Game.Net.CarLane componentData1;
      DynamicBuffer<LaneOverlap> bufferData1;
      if ((double) nextLane.m_CurvePosition.x != (double) math.select(0.0f, 1f, c) || !this.m_CarLaneData.TryGetComponent(nextLane.m_Lane, out componentData1) || (componentData1.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights)) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) && (componentData1.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || !this.m_LaneOverlapData.TryGetBuffer(currentLane, out bufferData1))
        return true;
      Curve curve = this.m_CurveData[nextLane.m_Lane];
      int num1 = (double) curveOffset.y < (double) curveOffset.x ? 1 : 0;
      bool flag1 = false;
      float num2 = float.MaxValue;
      float y1 = MathUtils.Size(this.m_PrefabObjectGeometry.m_Bounds.z);
      float num3 = y1;
      int num4 = 1;
      OverlapFlags overlapFlags = num1 != 0 ? OverlapFlags.MergeStart : OverlapFlags.MergeEnd;
      float3 a1 = math.normalizesafe(MathUtils.Tangent(curve.m_Bezier, nextLane.m_CurvePosition.x));
      float3 float3 = MathUtils.Position(curve.m_Bezier, nextLane.m_CurvePosition.x);
      float3 y2 = math.select(a1, -a1, c);
      DynamicBuffer<LaneObject> bufferData2;
      for (int index1 = 0; index1 < bufferData1.Length; ++index1)
      {
        LaneOverlap laneOverlap = bufferData1[index1];
        if ((laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleStart | OverlapFlags.MergeMiddleEnd | OverlapFlags.Unsafe)) == (OverlapFlags) 0)
          flag1 = true;
        else if ((laneOverlap.m_Flags & overlapFlags) != (OverlapFlags) 0 && laneOverlap.m_Other != nextLane.m_Lane && this.m_LaneObjectData.TryGetBuffer(laneOverlap.m_Other, out bufferData2) && bufferData2.Length != 0)
        {
          float2 float2_1 = new float2((float) laneOverlap.m_OtherStart, (float) laneOverlap.m_OtherEnd) * 0.003921569f;
          for (int index2 = 0; index2 < bufferData2.Length; ++index2)
          {
            LaneObject laneObject = bufferData2[index2];
            Controller componentData2;
            if (!(laneObject.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData2) || !(componentData2.m_Controller == this.m_Entity)))
            {
              float2 curvePosition = laneObject.m_CurvePosition;
              Car componentData3;
              Moving componentData4;
              if ((((double) curvePosition.y < (double) curvePosition.x ? ((double) curvePosition.y >= (double) float2_1.y ? 1 : 0) : ((double) curvePosition.y <= (double) float2_1.x ? 1 : 0)) == 0 || (!this.m_CarData.TryGetComponent(laneObject.m_LaneObject, out componentData3) ? (!this.m_TrainData.HasComponent(laneObject.m_LaneObject) ? 0 : VehicleUtils.GetPriority(this.m_PrefabTrainData[this.m_PrefabRefData[laneObject.m_LaneObject].m_Prefab])) : VehicleUtils.GetPriority(componentData3)) >= this.m_Priority) && this.m_MovingData.TryGetComponent(laneObject.m_LaneObject, out componentData4))
              {
                PrefabRef prefabRef = this.m_PrefabRefData[laneObject.m_LaneObject];
                ObjectGeometryData componentData5;
                if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData5))
                {
                  num3 += MathUtils.Size(componentData5.m_Bounds.z) + 1f;
                  ++num4;
                  blocker = laneObject.m_LaneObject;
                  if ((double) num2 >= (double) y1)
                  {
                    float num5 = -componentData5.m_Bounds.min.z;
                    bool flag2 = false;
                    TrainData componentData6;
                    if (this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData6))
                    {
                      Train train = this.m_TrainData[laneObject.m_LaneObject];
                      float2 float2_2 = componentData6.m_AttachOffsets - componentData6.m_BogieOffsets;
                      num5 = math.select(float2_2.y, float2_2.x, (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0);
                      flag2 = true;
                    }
                    num2 = math.dot(this.m_TransformData[laneObject.m_LaneObject].m_Position - float3, y2) - num5;
                    float speed = math.dot(componentData4.m_Velocity, y2);
                    if ((double) speed > 1.0 / 1000.0)
                    {
                      CarData componentData7;
                      if (this.m_PrefabCarData.TryGetComponent(prefabRef.m_Prefab, out componentData7))
                        num2 += VehicleUtils.GetBrakingDistance(componentData7, speed, this.m_SafeTimeStep);
                      else if (flag2)
                        num2 += VehicleUtils.GetBrakingDistance(componentData6, speed, this.m_SafeTimeStep);
                    }
                  }
                }
              }
            }
          }
        }
      }
      if (!flag1)
        return true;
      if (this.m_LaneObjectData.TryGetBuffer(currentLane, out bufferData2))
      {
        for (int index = 0; index < bufferData2.Length; ++index)
        {
          LaneObject laneObject = bufferData2[index];
          Controller componentData8;
          Moving componentData9;
          if (!(laneObject.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData8) || !(componentData8.m_Controller == this.m_Entity)) && this.m_MovingData.TryGetComponent(laneObject.m_LaneObject, out componentData9))
          {
            PrefabRef prefabRef = this.m_PrefabRefData[laneObject.m_LaneObject];
            ObjectGeometryData componentData10;
            if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData10))
            {
              num3 += MathUtils.Size(componentData10.m_Bounds.z) + 1f;
              ++num4;
              blocker = laneObject.m_LaneObject;
              if ((double) num2 >= (double) y1)
              {
                float num6 = -componentData10.m_Bounds.min.z;
                bool flag3 = false;
                TrainData componentData11;
                if (this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData11))
                {
                  Train train = this.m_TrainData[laneObject.m_LaneObject];
                  float2 float2 = componentData11.m_AttachOffsets - componentData11.m_BogieOffsets;
                  num6 = math.select(float2.y, float2.x, (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0);
                  flag3 = true;
                }
                num2 = math.dot(this.m_TransformData[laneObject.m_LaneObject].m_Position - float3, y2) - num6;
                float speed = math.dot(componentData9.m_Velocity, y2);
                if ((double) speed > 1.0 / 1000.0)
                {
                  CarData componentData12;
                  if (this.m_PrefabCarData.TryGetComponent(prefabRef.m_Prefab, out componentData12))
                    num2 += VehicleUtils.GetBrakingDistance(componentData12, speed, this.m_SafeTimeStep);
                  else if (flag3)
                    num2 += VehicleUtils.GetBrakingDistance(componentData11, speed, this.m_SafeTimeStep);
                }
              }
            }
          }
        }
      }
      if ((double) num2 != 3.4028234663852886E+38 && (double) num2 >= (double) y1)
      {
        blocker = Entity.Null;
        return true;
      }
      float x = 0.0f;
      int num7 = 1;
      while (true)
      {
        if (this.m_LaneObjectData.TryGetBuffer(nextLane.m_Lane, out bufferData2))
        {
          for (int a2 = 0; a2 < bufferData2.Length; ++a2)
          {
            LaneObject laneObject = bufferData2[math.select(a2, bufferData2.Length - 1 - a2, c)];
            bool flag4 = (double) laneObject.m_CurvePosition.y < (double) laneObject.m_CurvePosition.x;
            if (c == flag4)
            {
              if (c)
              {
                if ((double) laneObject.m_CurvePosition.x > (double) nextLane.m_CurvePosition.x)
                  continue;
              }
              else if ((double) laneObject.m_CurvePosition.x < (double) nextLane.m_CurvePosition.x)
                continue;
              Controller componentData13;
              Moving componentData14;
              if (!(laneObject.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData13) || !(componentData13.m_Controller == this.m_Entity)) && this.m_MovingData.TryGetComponent(laneObject.m_LaneObject, out componentData14))
              {
                PrefabRef prefabRef = this.m_PrefabRefData[laneObject.m_LaneObject];
                ObjectGeometryData componentData15;
                this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData15);
                float num8 = -componentData15.m_Bounds.min.z;
                bool flag5 = false;
                TrainData componentData16;
                if (this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData16))
                {
                  Train train = this.m_TrainData[laneObject.m_LaneObject];
                  float2 float2 = componentData16.m_AttachOffsets - componentData16.m_BogieOffsets;
                  num8 = math.select(float2.y, float2.x, (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0);
                  flag5 = true;
                }
                Transform transform = this.m_TransformData[laneObject.m_LaneObject];
                float num9 = x + math.dot(transform.m_Position - float3, y2) - num8;
                float speed = math.dot(componentData14.m_Velocity, y2);
                if ((double) speed > 1.0 / 1000.0)
                {
                  CarData componentData17;
                  if (this.m_PrefabCarData.TryGetComponent(prefabRef.m_Prefab, out componentData17))
                    num9 += VehicleUtils.GetBrakingDistance(componentData17, speed, this.m_SafeTimeStep);
                  else if (flag5)
                    num9 += VehicleUtils.GetBrakingDistance(componentData16, speed, this.m_SafeTimeStep);
                }
                if ((double) num9 >= (double) num3)
                {
                  blocker = Entity.Null;
                  return true;
                }
                blocker = laneObject.m_LaneObject;
                if (--num4 == 0)
                  return false;
                num3 += MathUtils.Size(componentData15.m_Bounds.z) + 1f;
              }
            }
          }
        }
        x += curve.m_Length;
        if ((double) math.max(x, y1) < (double) num3)
        {
          if (num7 < nextLanes.Length)
          {
            nextLane = nextLanes[num7++];
            c = (double) nextLane.m_CurvePosition.y < (double) nextLane.m_CurvePosition.x;
            if ((double) nextLane.m_CurvePosition.x == (double) math.select(0.0f, 1f, c) && this.m_CarLaneData.TryGetComponent(nextLane.m_Lane, out componentData1) && ((componentData1.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights)) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || (componentData1.m_Flags & Game.Net.CarLaneFlags.Approach) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter)))
            {
              curve = this.m_CurveData[nextLane.m_Lane];
              float3 a3 = math.normalizesafe(MathUtils.Tangent(curve.m_Bezier, nextLane.m_CurvePosition.x));
              float3 = MathUtils.Position(curve.m_Bezier, nextLane.m_CurvePosition.x);
              y2 = math.select(a3, -a3, c);
            }
            else
              goto label_69;
          }
          else
            goto label_67;
        }
        else
          break;
      }
      blocker = Entity.Null;
      return true;
label_67:
      return false;
label_69:
      return false;
    }

    private bool CheckOverlapSpace(
      Entity currentLane,
      float2 curCurvePos,
      Entity nextLane,
      float2 nextCurvePos,
      float2 overlapPos,
      out Entity blocker)
    {
      blocker = Entity.Null;
      Entity laneObject1 = Entity.Null;
      Curve curve1 = this.m_CurveData[currentLane];
      float x = curve1.m_Length * (1f - curCurvePos.x);
      float y1 = MathUtils.Size(this.m_PrefabObjectGeometry.m_Bounds.z);
      float num1 = y1;
      DynamicBuffer<LaneObject> bufferData;
      if (this.m_LaneObjectData.TryGetBuffer(currentLane, out bufferData))
      {
        for (int index = 0; index < bufferData.Length; ++index)
        {
          LaneObject laneObject2 = bufferData[index];
          Controller componentData1;
          if ((double) laneObject2.m_CurvePosition.x >= (double) curCurvePos.x && !(laneObject2.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject2.m_LaneObject, out componentData1) || !(componentData1.m_Controller == this.m_Entity)))
          {
            ObjectGeometryData componentData2;
            if (this.m_MovingData.HasComponent(laneObject2.m_LaneObject) && this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[laneObject2.m_LaneObject].m_Prefab, out componentData2))
            {
              num1 += MathUtils.Size(componentData2.m_Bounds.z) + 1f;
              blocker = laneObject2.m_LaneObject;
            }
            if ((double) laneObject2.m_CurvePosition.y >= (double) overlapPos.y)
            {
              laneObject1 = laneObject2.m_LaneObject;
              break;
            }
          }
        }
      }
      if (laneObject1 == Entity.Null && this.m_CarLaneData.HasComponent(nextLane))
      {
        Curve curve2 = this.m_CurveData[nextLane];
        x += curve2.m_Length;
        if (this.m_LaneObjectData.TryGetBuffer(nextLane, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            LaneObject laneObject3 = bufferData[index];
            Controller componentData;
            if ((double) laneObject3.m_CurvePosition.x >= (double) nextCurvePos.x && !(laneObject3.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject3.m_LaneObject, out componentData) || !(componentData.m_Controller == this.m_Entity)))
            {
              laneObject1 = laneObject3.m_LaneObject;
              break;
            }
          }
        }
      }
      float num2 = math.max(x, y1);
      if (laneObject1 != Entity.Null)
      {
        Moving componentData3;
        if (this.m_MovingData.TryGetComponent(laneObject1, out componentData3))
        {
          PrefabRef prefabRef = this.m_PrefabRefData[laneObject1];
          float num3 = 0.0f;
          bool flag = false;
          TrainData componentData4;
          if (this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData4))
          {
            Train train = this.m_TrainData[laneObject1];
            float2 float2 = componentData4.m_AttachOffsets - componentData4.m_BogieOffsets;
            num3 = math.select(float2.y, float2.x, (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0);
            flag = true;
          }
          else
          {
            ObjectGeometryData componentData5;
            if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData5))
              num3 = -componentData5.m_Bounds.min.z;
          }
          Transform transform = this.m_TransformData[laneObject1];
          float3 y2 = math.normalizesafe(MathUtils.Tangent(curve1.m_Bezier, overlapPos.y));
          num2 = math.dot(transform.m_Position - MathUtils.Position(curve1.m_Bezier, overlapPos.y), y2) - num3;
          float speed = math.dot(componentData3.m_Velocity, y2);
          if ((double) speed > 1.0 / 1000.0)
          {
            CarData componentData6;
            if (this.m_PrefabCarData.TryGetComponent(prefabRef.m_Prefab, out componentData6))
              num2 += VehicleUtils.GetBrakingDistance(componentData6, speed, this.m_SafeTimeStep);
            else if (flag)
              num2 += VehicleUtils.GetBrakingDistance(componentData4, speed, this.m_SafeTimeStep);
          }
        }
        blocker = laneObject1;
      }
      if ((double) num2 < (double) num1)
        return false;
      blocker = Entity.Null;
      return true;
    }

    private void CheckParkingLane(float distance)
    {
      DynamicBuffer<LaneObject> bufferData;
      if (!this.m_LaneObjectData.TryGetBuffer(this.m_Lane, out bufferData) || bufferData.Length == 0)
        return;
      ParkingLaneData parkingLaneData = this.m_PrefabParkingLaneData[this.m_PrefabRefData[this.m_Lane].m_Prefab];
      float3 x = MathUtils.Position(this.m_Curve.m_Bezier, this.m_CurveOffset.x);
      float2 float2;
      if ((double) parkingLaneData.m_SlotInterval == 0.0)
      {
        float offset;
        float2 = (float2) (VehicleUtils.GetParkingSize(this.m_PrefabObjectGeometry, out offset).y * 0.5f);
        float2.x += 0.9f + offset;
        float2.y += 0.9f - offset;
      }
      else
        float2 = (float2) 0.1f;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        LaneObject laneObject = bufferData[index];
        Controller componentData;
        if (!(laneObject.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData) || !(componentData.m_Controller == this.m_Entity)) && !this.m_UnspawnedData.HasComponent(laneObject.m_LaneObject))
        {
          bool c = (double) laneObject.m_CurvePosition.y >= (double) this.m_CurveOffset.x;
          float3 y = MathUtils.Position(this.m_Curve.m_Bezier, laneObject.m_CurvePosition.y);
          float num1 = math.select(float2.x, float2.y, c);
          if ((double) parkingLaneData.m_SlotInterval == 0.0)
          {
            float2 parkingOffsets = VehicleUtils.GetParkingOffsets(laneObject.m_LaneObject, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData);
            num1 += math.select(parkingOffsets.y, parkingOffsets.x, c);
          }
          if ((double) math.distance(x, y) < (double) num1)
          {
            float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance, this.m_SafeTimeStep);
            float num2 = MathUtils.Clamp(math.select(maxBrakingSpeed, 3f, laneObject.m_LaneObject == this.m_Ignore & (double) maxBrakingSpeed < 1.0), this.m_SpeedRange);
            if ((double) num2 < (double) this.m_MaxSpeed)
            {
              this.m_MaxSpeed = num2;
              this.m_Blocker = laneObject.m_LaneObject;
              this.m_BlockerType = BlockerType.Continuing;
            }
          }
        }
      }
    }

    private void CheckCurrentLane(float distance, float2 minOffset, bool inverse)
    {
      DynamicBuffer<LaneObject> bufferData;
      if (!this.m_LaneObjectData.TryGetBuffer(this.m_Lane, out bufferData) || bufferData.Length == 0)
        return;
      distance -= 0.9f;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        LaneObject laneObject = bufferData[index];
        if (!(laneObject.m_LaneObject == this.m_Entity))
        {
          float2 curvePosition = laneObject.m_CurvePosition;
          bool inverse2 = (double) curvePosition.y < (double) curvePosition.x;
          Controller componentData;
          if (!(!inverse ? (!inverse2 ? (double) curvePosition.y <= (double) minOffset.y && ((double) curvePosition.y < 1.0 || (double) curvePosition.x <= (double) minOffset.x) : (double) curvePosition.x <= (double) minOffset.x) : (!inverse2 ? (double) curvePosition.x >= (double) minOffset.x : (double) curvePosition.y >= (double) minOffset.y && ((double) curvePosition.y > 0.0 || (double) curvePosition.x >= (double) minOffset.x))) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData) || !(componentData.m_Controller == this.m_Entity)))
          {
            float objectSpeed1 = this.GetObjectSpeed(laneObject.m_LaneObject, curvePosition.x);
            float objectSpeed2 = math.select(objectSpeed1, -objectSpeed1, inverse);
            BlockerType blockerType = inverse != inverse2 ? BlockerType.Oncoming : BlockerType.Continuing;
            this.UpdateMaxSpeed(laneObject.m_LaneObject, blockerType, objectSpeed2, curvePosition.x, 1f, distance, laneObject.m_LaneObject == this.m_Ignore, inverse, inverse2, this.m_CurrentPosition);
          }
        }
      }
    }

    private void CheckCurrentLane(
      float distance,
      float2 minOffset,
      bool inverse,
      ref float canUseLane)
    {
      DynamicBuffer<LaneObject> bufferData;
      if (!this.m_LaneObjectData.TryGetBuffer(this.m_Lane, out bufferData) || bufferData.Length == 0)
        return;
      distance -= 0.9f;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        LaneObject laneObject = bufferData[index];
        Controller componentData1;
        if (!(laneObject.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData1) || !(componentData1.m_Controller == this.m_Entity)))
        {
          float2 curvePosition = laneObject.m_CurvePosition;
          bool inverse2 = (double) curvePosition.y < (double) curvePosition.x;
          if (!inverse ? (!inverse2 ? (double) curvePosition.y <= (double) minOffset.y && ((double) curvePosition.y < 1.0 || (double) curvePosition.x <= (double) minOffset.x) : (double) curvePosition.x <= (double) minOffset.x) : (!inverse2 ? (double) curvePosition.x >= (double) minOffset.x : (double) curvePosition.y >= (double) minOffset.y && ((double) curvePosition.y > 0.0 || (double) curvePosition.x >= (double) minOffset.x)))
          {
            PrefabRef prefabRef = this.m_PrefabRefData[laneObject.m_LaneObject];
            float num = 0.0f;
            ObjectGeometryData componentData2;
            if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
              num = -componentData2.m_Bounds.max.z;
            if (((double) curvePosition.x - (double) minOffset.x) * (double) this.m_Curve.m_Length > (double) num)
              canUseLane = 0.0f;
          }
          else
          {
            float objectSpeed1 = this.GetObjectSpeed(laneObject.m_LaneObject, curvePosition.x);
            float objectSpeed2 = math.select(objectSpeed1, -objectSpeed1, inverse);
            BlockerType blockerType = inverse != inverse2 ? BlockerType.Oncoming : BlockerType.Continuing;
            this.UpdateMaxSpeed(laneObject.m_LaneObject, blockerType, objectSpeed2, curvePosition.x, 1f, distance, laneObject.m_LaneObject == this.m_Ignore, inverse, inverse2, this.m_CurrentPosition);
          }
        }
      }
    }

    private void CheckOverlappingLanes(
      float origDistance,
      float origMinOffset,
      int yieldOverride,
      float speedLimit,
      bool isRoundabout,
      bool inverse,
      bool requestSpace)
    {
      DynamicBuffer<LaneOverlap> bufferData1;
      if (!this.m_LaneOverlapData.TryGetBuffer(this.m_Lane, out bufferData1) || bufferData1.Length == 0)
        return;
      origDistance -= 0.9f;
      Entity lane = this.m_Lane;
      Bezier4x3 bezier = this.m_Curve.m_Bezier;
      float2 curveOffset = this.m_CurveOffset;
      float length = this.m_Curve.m_Length;
      float x = 1f;
      int a1 = this.m_Priority;
      Game.Net.LaneReservation componentData1;
      if (this.m_LaneReservationData.TryGetComponent(this.m_Lane, out componentData1))
      {
        int priority = componentData1.GetPriority();
        a1 = math.select(a1, 106, priority >= 108 & 106 > a1);
      }
      for (int index1 = 0; index1 < bufferData1.Length; ++index1)
      {
        LaneOverlap laneOverlap = bufferData1[index1];
        float4 float4 = new float4((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd, (float) laneOverlap.m_OtherStart, (float) laneOverlap.m_OtherEnd) * 0.003921569f;
        if (inverse)
        {
          if ((double) float4.x >= (double) curveOffset.x)
            continue;
        }
        else if ((double) float4.y <= (double) curveOffset.x)
          continue;
        this.m_Lane = laneOverlap.m_Other;
        this.m_Curve = this.m_CurveData[this.m_Lane];
        this.m_CurveOffset = math.select(float4.zw, float4.wz, inverse);
        Line3.Segment overlapLine = MathUtils.Line(bezier, float4.xy);
        float a2;
        OverlapFlags overlapFlags1;
        OverlapFlags overlapFlags2;
        if (inverse)
        {
          a2 = math.max(0.0f, float4.y - origMinOffset);
          overlapFlags1 = OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleEnd;
          overlapFlags2 = OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart;
        }
        else
        {
          a2 = math.max(0.0f, origMinOffset - float4.x);
          overlapFlags1 = OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart;
          overlapFlags2 = OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleEnd;
        }
        if (isRoundabout && laneOverlap.m_PriorityDelta > (sbyte) 0 && (laneOverlap.m_Flags & OverlapFlags.Road) != (OverlapFlags) 0 && (double) float4.x >= (double) curveOffset.x)
        {
          x = math.min(x, float4.x);
          float4.x = x;
        }
        float num1 = origDistance + length * math.select(float4.x - curveOffset.x, curveOffset.x - float4.y, inverse);
        float distanceFactor = (float) laneOverlap.m_Parallelism * (1f / 128f);
        bool flag1 = (double) VehicleUtils.GetBrakingDistance(this.m_PrefabCar, this.m_CurrentSpeed, this.m_TimeStep) <= (double) num1;
        int num2 = a1;
        BlockerType blockerType = (laneOverlap.m_Flags & overlapFlags2) != (OverlapFlags) 0 ? BlockerType.Continuing : BlockerType.Crossing;
        if ((laneOverlap.m_Flags & overlapFlags1) == (OverlapFlags) 0)
        {
          if ((inverse ? ((double) float4.y >= (double) origMinOffset ? 1 : 0) : ((double) float4.x <= (double) origMinOffset ? 1 : 0)) != 0)
          {
            if (isRoundabout)
            {
              if (!this.m_TempBuffer.IsCreated)
                this.m_TempBuffer = new NativeList<Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              this.m_TempBuffer.Add(in this.m_Lane);
            }
          }
          else
          {
            if (isRoundabout && this.m_TempBuffer.IsCreated)
            {
              for (int index2 = 0; index2 < this.m_TempBuffer.Length; ++index2)
              {
                if (this.m_TempBuffer[index2] == this.m_Lane)
                  goto label_38;
              }
            }
            int b = yieldOverride;
            Game.Net.LaneSignal componentData2;
            if (this.m_LaneSignalData.TryGetComponent(this.m_Lane, out componentData2))
            {
              switch (componentData2.m_Signal)
              {
                case LaneSignalType.Stop:
                  ++b;
                  break;
                case LaneSignalType.Yield:
                  --b;
                  break;
              }
            }
            int a3 = math.select((int) laneOverlap.m_PriorityDelta, b, b != 0);
            int num3 = math.select(a3, 0, requestSpace & a3 > 0);
            num2 -= num3;
            if (this.m_LaneReservationData.TryGetComponent(this.m_Lane, out componentData1))
            {
              double offset = (double) componentData1.GetOffset();
              float num4 = math.select(math.max(a2 + float4.z, this.m_CurveOffset.x), 0.0f, inverse);
              int priority = componentData1.GetPriority();
              double num5 = (double) num4;
              if (offset > num5 | priority > num2)
              {
                float num6 = MathUtils.Clamp(VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, num1, this.m_SafeTimeStep), this.m_SpeedRange);
                if ((double) num6 < (double) this.m_MaxSpeed)
                {
                  this.m_MaxSpeed = num6;
                  this.m_Blocker = Entity.Null;
                  this.m_BlockerType = blockerType;
                }
              }
              else if (num3 > 0)
              {
                float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, num1, this.m_SafeTimeStep);
                if ((double) maxBrakingSpeed >= (double) speedLimit * 0.5 && (double) maxBrakingSpeed < (double) this.m_MaxSpeed)
                {
                  this.m_MaxSpeed = maxBrakingSpeed;
                  this.m_Blocker = Entity.Null;
                  this.m_BlockerType = blockerType;
                }
              }
              Entity blocker;
              if (flag1 && priority == 96 && !this.CheckOverlapSpace(lane, curveOffset, this.m_NextLane, this.m_NextOffset, float4.xy, out blocker))
              {
                float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, num1, this.m_SafeTimeStep);
                if ((double) maxBrakingSpeed < (double) this.m_MaxSpeed)
                {
                  this.m_MaxSpeed = maxBrakingSpeed;
                  this.m_Blocker = blocker;
                  this.m_BlockerType = blockerType;
                }
              }
            }
          }
        }
label_38:
        DynamicBuffer<LaneObject> bufferData2;
        if (this.m_LaneObjectData.TryGetBuffer(this.m_Lane, out bufferData2) && bufferData2.Length != 0)
        {
          int num7 = 100;
          bool giveSpace = flag1 & num7 > num2;
          for (int index3 = 0; index3 < bufferData2.Length; ++index3)
          {
            LaneObject laneObject = bufferData2[index3];
            Controller componentData3;
            if (!(laneObject.m_LaneObject == this.m_Entity) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData3) || !(componentData3.m_Controller == this.m_Entity)))
            {
              if (this.m_CreatureData.HasComponent(laneObject.m_LaneObject))
              {
                this.CheckPedestrian(overlapLine, laneObject.m_LaneObject, laneObject.m_CurvePosition.y, num1, giveSpace, inverse);
              }
              else
              {
                float2 curvePosition = laneObject.m_CurvePosition;
                bool flag2 = (double) curvePosition.y < (double) curvePosition.x;
                float objectSpeed1 = this.GetObjectSpeed(laneObject.m_LaneObject, curvePosition.x);
                if ((laneOverlap.m_Flags & overlapFlags1) == (OverlapFlags) 0 && ((inverse ? ((double) float4.y <= (double) origMinOffset ? 1 : 0) : ((double) float4.x >= (double) origMinOffset ? 1 : 0)) | (flag2 ? ((double) curvePosition.y < (double) float4.w ? 1 : 0) : ((double) curvePosition.y > (double) float4.z ? 1 : 0))) != 0)
                {
                  Car componentData4;
                  int num8 = (!this.m_CarData.TryGetComponent(laneObject.m_LaneObject, out componentData4) ? (!this.m_TrainData.HasComponent(laneObject.m_LaneObject) ? 0 : VehicleUtils.GetPriority(this.m_PrefabTrainData[this.m_PrefabRefData[laneObject.m_LaneObject].m_Prefab])) : VehicleUtils.GetPriority(componentData4)) - num2;
                  if (num8 > 0)
                    curvePosition.y += objectSpeed1 * 2f / math.max(1f, this.m_Curve.m_Length);
                  else if (num8 < 0)
                    curvePosition.y += math.select(a2, -a2, flag2);
                }
                if (flag2)
                {
                  if ((double) curvePosition.y >= (double) float4.w - (double) a2)
                    continue;
                }
                else if ((double) curvePosition.y <= (double) float4.z + (double) a2)
                  continue;
                float objectSpeed2 = math.select(objectSpeed1, -objectSpeed1, inverse);
                float3 currentPos = MathUtils.Position(this.m_Curve.m_Bezier, math.select(this.m_CurveOffset.x, this.m_CurveOffset.y, flag2 != inverse));
                this.UpdateMaxSpeed(laneObject.m_LaneObject, blockerType, objectSpeed2, curvePosition.x, distanceFactor, num1, laneObject.m_LaneObject == this.m_Ignore, inverse, flag2, currentPos);
              }
            }
          }
        }
      }
    }

    private float GetObjectSpeed(Entity obj, float curveOffset)
    {
      Moving componentData;
      if (!this.m_MovingData.TryGetComponent(obj, out componentData))
        return 0.0f;
      float3 y = math.normalizesafe(MathUtils.Tangent(this.m_Curve.m_Bezier, curveOffset));
      return math.dot(componentData.m_Velocity, y);
    }

    private void CheckPedestrian(
      Line3.Segment overlapLine,
      Entity obj,
      float targetOffset,
      float distanceOffset,
      bool giveSpace,
      bool inverse)
    {
      float2 float2 = math.select(this.m_CurveOffset, this.m_CurveOffset.yx, inverse);
      if ((double) targetOffset <= (double) float2.x | (double) targetOffset >= (double) float2.y)
      {
        PrefabRef prefabRef = this.m_PrefabRefData[obj];
        Transform transform = this.m_TransformData[obj];
        float num1 = this.m_PrefabObjectGeometry.m_Size.x * 0.5f;
        ObjectGeometryData componentData;
        if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData))
          num1 += componentData.m_Size.z * 0.5f;
        float t;
        double num2 = (double) MathUtils.Distance(overlapLine.xz, transform.m_Position.xz, out t);
        float3 float3 = math.forward(transform.m_Rotation);
        float2 xz = float3.xz;
        float3 = MathUtils.Position(overlapLine, t);
        float2 y = math.normalizesafe(float3.xz - transform.m_Position.xz);
        float x = math.dot(xz, y);
        double num3 = (double) math.select(math.min(-x, 0.0f), math.max(x, 0.0f), giveSpace);
        if (num2 - num3 >= (double) num1)
          return;
      }
      Moving componentData1;
      float num = MathUtils.Clamp(this.m_PushBlockers || !this.m_MovingData.TryGetComponent(obj, out componentData1) || (double) math.lengthsq(componentData1.m_Velocity) < 0.0099999997764825821 ? math.max(3f, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distanceOffset, 3f, this.m_SafeTimeStep)) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distanceOffset, this.m_SafeTimeStep), this.m_SpeedRange);
      if ((double) num >= (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = num;
      this.m_Blocker = obj;
      this.m_BlockerType = BlockerType.Temporary;
    }

    private void UpdateMaxSpeed(
      Entity obj,
      BlockerType blockerType,
      float objectSpeed,
      float laneOffset,
      float distanceFactor,
      float distanceOffset,
      bool ignore,
      bool inverse1,
      bool inverse2,
      float3 currentPos)
    {
      PrefabRef prefabRef = this.m_PrefabRefData[obj];
      float num1 = 0.0f;
      bool flag = false;
      TrainData componentData1;
      if (this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
      {
        Train train = this.m_TrainData[obj];
        float2 float2 = componentData1.m_AttachOffsets - componentData1.m_BogieOffsets;
        num1 = math.select(float2.y, float2.x, (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0);
        flag = true;
      }
      else
      {
        ObjectGeometryData componentData2;
        if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          num1 = -componentData2.m_Bounds.min.z;
      }
      float2 float2_1 = math.select(this.m_CurveOffset, this.m_CurveOffset.yx, inverse1 != inverse2);
      float2 float2_2 = math.select(laneOffset - float2_1, float2_1 - laneOffset, inverse2);
      if ((double) float2_2.y * (double) this.m_Curve.m_Length >= (double) num1)
        return;
      float2_2.x = math.min(0.0f, float2_2.x);
      Transform transform = this.m_TransformData[obj];
      float x = math.distance(MathUtils.Position(this.m_Curve.m_Bezier, laneOffset + math.select(-float2_2.x, float2_2.x, inverse2)), currentPos) + float2_2.x * this.m_Curve.m_Length;
      float distance = (((double) math.dot(transform.m_Position - currentPos, currentPos - this.m_PrevPosition) >= 0.0 ? math.min(x, math.distance(transform.m_Position, currentPos)) : math.min(x, math.distance(transform.m_Position, this.m_PrevPosition) + this.m_PrevDistance - this.m_Distance)) - num1) * distanceFactor + distanceOffset;
      CarData componentData3;
      float num2;
      if ((double) objectSpeed > 1.0 / 1000.0 && this.m_PrefabCarData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
      {
        objectSpeed = math.max(0.0f, objectSpeed - (float) ((double) componentData3.m_Braking * (double) this.m_TimeStep * 2.0)) * distanceFactor;
        num2 = (double) this.m_PrefabCar.m_Braking < (double) componentData3.m_Braking ? VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance + VehicleUtils.GetBrakingDistance(componentData3, objectSpeed, this.m_SafeTimeStep), this.m_SafeTimeStep) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance + objectSpeed * this.m_SafeTimeStep, objectSpeed, this.m_SafeTimeStep);
      }
      else if ((double) objectSpeed > 1.0 / 1000.0 & flag)
      {
        objectSpeed = math.max(0.0f, objectSpeed - (float) ((double) componentData1.m_Braking * (double) this.m_TimeStep * 2.0)) * distanceFactor;
        num2 = (double) this.m_PrefabCar.m_Braking < (double) componentData1.m_Braking ? VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance + VehicleUtils.GetBrakingDistance(componentData1, objectSpeed, this.m_SafeTimeStep), this.m_SafeTimeStep) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance + objectSpeed * this.m_SafeTimeStep, objectSpeed, this.m_SafeTimeStep);
      }
      else
        num2 = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabCar, distance, this.m_SafeTimeStep);
      if (blockerType == BlockerType.Oncoming)
      {
        this.m_Oncoming = math.max(this.m_Oncoming, (float) (2.0 - (double) num2 * 0.1666666716337204));
        float num3 = MathUtils.Clamp(math.max(num2, 3f), this.m_SpeedRange);
        if ((double) num3 >= (double) this.m_MaxSpeed)
          return;
        this.m_MaxSpeed = num3;
        this.m_Blocker = Entity.Null;
        this.m_BlockerType = blockerType;
      }
      else
      {
        float num4 = MathUtils.Clamp(math.select(num2, 3f, ignore & (double) num2 < 3.0), this.m_SpeedRange);
        if ((double) num4 >= (double) this.m_MaxSpeed)
          return;
        this.m_MaxSpeed = num4;
        this.m_Blocker = obj;
        this.m_BlockerType = blockerType;
      }
    }
  }
}
