// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TrainLaneSpeedIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
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
  public struct TrainLaneSpeedIterator
  {
    public ComponentLookup<Transform> m_TransformData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Car> m_CarData;
    public ComponentLookup<Train> m_TrainData;
    public ComponentLookup<Curve> m_CurveData;
    public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
    public ComponentLookup<Controller> m_ControllerData;
    public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
    public ComponentLookup<Game.Net.LaneSignal> m_LaneSignalData;
    public ComponentLookup<Creature> m_CreatureData;
    public ComponentLookup<PrefabRef> m_PrefabRefData;
    public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
    public ComponentLookup<CarData> m_PrefabCarData;
    public ComponentLookup<TrainData> m_PrefabTrainData;
    public BufferLookup<LaneOverlap> m_LaneOverlapData;
    public BufferLookup<LaneObject> m_LaneObjectData;
    public Entity m_Controller;
    public int m_Priority;
    public float m_TimeStep;
    public float m_SafeTimeStep;
    public float m_CurrentSpeed;
    public TrainData m_PrefabTrain;
    public ObjectGeometryData m_PrefabObjectGeometry;
    public Bounds1 m_SpeedRange;
    public float3 m_RearPosition;
    public bool m_PushBlockers;
    public float m_MaxSpeed;
    public float3 m_CurrentPosition;
    public float m_Distance;
    public Entity m_Blocker;
    public BlockerType m_BlockerType;
    private Entity m_Lane;
    private Curve m_Curve;
    private float2 m_CurveOffset;
    private float3 m_PrevPosition;
    private float m_PrevDistance;

    public bool IterateFirstLane(
      Entity lane,
      float4 curveOffset,
      bool exclusive,
      bool ignoreObstacles,
      bool skipCurrent,
      out bool needSignal)
    {
      Curve curve = this.m_CurveData[lane];
      needSignal = false;
      float3 float3 = MathUtils.Position(curve.m_Bezier, curveOffset.y);
      this.m_PrevPosition = this.m_CurrentPosition;
      this.m_PrevDistance = (float) -((double) this.m_PrefabTrain.m_AttachOffsets.x - (double) this.m_PrefabTrain.m_BogieOffsets.x);
      this.m_Distance = math.distance(this.m_CurrentPosition, float3);
      this.m_Distance = math.min(this.m_Distance, math.distance(this.m_RearPosition, float3) - math.max(1f, math.csum(this.m_PrefabTrain.m_BogieOffsets)));
      this.m_Distance -= this.m_PrefabTrain.m_AttachOffsets.x - this.m_PrefabTrain.m_BogieOffsets.x;
      Game.Net.TrackLane componentData1;
      if (this.m_TrackLaneData.TryGetComponent(lane, out componentData1))
      {
        int yieldOverride = 0;
        Game.Net.LaneSignal componentData2;
        if (this.m_LaneSignalData.TryGetComponent(lane, out componentData2))
        {
          switch (componentData2.m_Signal)
          {
            case LaneSignalType.Stop:
              yieldOverride = -1;
              break;
            case LaneSignalType.Yield:
              yieldOverride = 1;
              break;
          }
          if (lane != this.m_Lane)
            needSignal = true;
        }
        this.m_Lane = lane;
        this.m_Curve = curve;
        this.m_CurveOffset = curveOffset.yw;
        this.m_CurrentPosition = float3;
        float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabTrain, componentData1);
        Game.Net.LaneReservation componentData3;
        if (!exclusive && this.m_LaneReservationData.TryGetComponent(lane, out componentData3) && componentData3.GetPriority() == 102)
          maxDriveSpeed *= 0.5f;
        if ((double) maxDriveSpeed < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(maxDriveSpeed, this.m_SpeedRange);
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.Limit;
        }
        if (!ignoreObstacles)
        {
          if (!exclusive && !skipCurrent)
            this.CheckCurrentLane(this.m_Distance, curveOffset.yz, exclusive);
          this.CheckOverlappingLanes(this.m_Distance, curveOffset.z, yieldOverride, exclusive);
        }
      }
      float3 y = MathUtils.Position(curve.m_Bezier, curveOffset.w);
      float s = math.abs(curveOffset.w - curveOffset.y);
      float num = math.max(1f / 1000f, math.lerp(math.distance(float3, y), curve.m_Length * s, s));
      if ((double) num > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y;
      this.m_Distance += num;
      return (double) this.m_Distance - 10.0 >= (double) (VehicleUtils.GetBrakingDistance(this.m_PrefabTrain, this.m_MaxSpeed, this.m_SafeTimeStep) + VehicleUtils.GetSignalDistance(this.m_PrefabTrain, this.m_MaxSpeed)) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public void IteratePrevLane(Entity lane, out bool needSignal)
    {
      needSignal = false;
      Game.Net.TrackLane componentData;
      if (!(lane != this.m_Lane) || !this.m_TrackLaneData.TryGetComponent(lane, out componentData))
        return;
      if (this.m_LaneSignalData.HasComponent(lane))
        needSignal = true;
      this.m_Lane = lane;
      float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabTrain, componentData);
      if ((double) maxDriveSpeed >= (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = MathUtils.Clamp(maxDriveSpeed, this.m_SpeedRange);
      this.m_Blocker = Entity.Null;
      this.m_BlockerType = BlockerType.Limit;
    }

    public bool IterateNextLane(
      Entity lane,
      float2 curveOffset,
      float minOffset,
      bool exclusive,
      bool ignoreObstacles,
      out bool needSignal)
    {
      needSignal = false;
      Curve componentData1;
      if (!this.m_CurveData.TryGetComponent(lane, out componentData1))
        return false;
      Game.Net.TrackLane componentData2;
      if (this.m_TrackLaneData.TryGetComponent(lane, out componentData2))
      {
        float num = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabTrain, componentData2);
        int yieldOverride = 0;
        Entity blocker = Entity.Null;
        BlockerType blockerType = BlockerType.Limit;
        Game.Net.LaneSignal componentData3;
        if (this.m_LaneSignalData.TryGetComponent(lane, out componentData3))
        {
          needSignal = true;
          switch (componentData3.m_Signal)
          {
            case LaneSignalType.Stop:
              if (this.m_Priority < 108 && (double) VehicleUtils.GetBrakingDistance(this.m_PrefabTrain, this.m_CurrentSpeed, 0.0f) <= (double) this.m_Distance + 1.0)
              {
                num = 0.0f;
                blocker = componentData3.m_Blocker;
                blockerType = BlockerType.Signal;
                yieldOverride = 1;
                break;
              }
              yieldOverride = -1;
              break;
            case LaneSignalType.SafeStop:
              if (this.m_Priority < 108 && (double) VehicleUtils.GetBrakingDistance(this.m_PrefabTrain, this.m_CurrentSpeed, 0.0f) <= (double) this.m_Distance)
              {
                num = 0.0f;
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
        float position = (double) num != 0.0 ? math.max(num, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, this.m_Distance, num, this.m_TimeStep)) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, math.max(0.0f, this.m_Distance - math.select(10f, 0.5f, (this.m_PrefabTrain.m_TrackType & TrackTypes.Tram) != 0)), this.m_SafeTimeStep);
        if ((double) position < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(position, this.m_SpeedRange);
          this.m_Blocker = blocker;
          this.m_BlockerType = blockerType;
        }
        if (!ignoreObstacles)
        {
          this.m_Lane = lane;
          this.m_Curve = componentData1;
          this.m_CurveOffset = curveOffset;
          this.CheckCurrentLane(this.m_Distance, (float2) minOffset, exclusive);
          this.CheckOverlappingLanes(this.m_Distance, minOffset, yieldOverride, exclusive);
        }
      }
      float3 y = MathUtils.Position(componentData1.m_Bezier, curveOffset.y);
      float s = math.abs(curveOffset.y - curveOffset.x);
      float num1 = math.max(1f / 1000f, math.lerp(math.distance(this.m_CurrentPosition, y), componentData1.m_Length * s, s));
      if ((double) num1 > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y;
      this.m_Distance += num1;
      return (double) this.m_Distance - 10.0 >= (double) (VehicleUtils.GetBrakingDistance(this.m_PrefabTrain, this.m_MaxSpeed, this.m_SafeTimeStep) + VehicleUtils.GetSignalDistance(this.m_PrefabTrain, this.m_MaxSpeed)) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public bool IterateTarget(Entity lane, bool ignoreObstacles)
    {
      float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, this.m_Distance, this.m_TimeStep);
      if ((double) maxBrakingSpeed >= (double) this.m_MaxSpeed)
        return false;
      this.m_MaxSpeed = MathUtils.Clamp(maxBrakingSpeed, this.m_SpeedRange);
      Game.Net.LaneReservation componentData1;
      if (this.m_LaneReservationData.TryGetComponent(lane, out componentData1))
      {
        Controller componentData2;
        if (ignoreObstacles || this.m_ControllerData.TryGetComponent(componentData1.m_Blocker, out componentData2) && componentData2.m_Controller == this.m_Controller)
        {
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.None;
        }
        else
        {
          this.m_Blocker = componentData1.m_Blocker;
          this.m_BlockerType = BlockerType.Continuing;
        }
      }
      else
      {
        this.m_Blocker = Entity.Null;
        this.m_BlockerType = BlockerType.None;
      }
      return true;
    }

    public bool IterateTarget()
    {
      float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, this.m_Distance, this.m_TimeStep);
      if ((double) maxBrakingSpeed >= (double) this.m_MaxSpeed)
        return false;
      this.m_MaxSpeed = MathUtils.Clamp(maxBrakingSpeed, this.m_SpeedRange);
      this.m_Blocker = Entity.Null;
      this.m_BlockerType = BlockerType.None;
      return true;
    }

    private void CheckCurrentLane(float distance, float2 minOffset, bool exclusive)
    {
      DynamicBuffer<LaneObject> bufferData;
      if (!this.m_LaneObjectData.TryGetBuffer(this.m_Lane, out bufferData) || bufferData.Length == 0)
        return;
      if (exclusive)
        distance -= 10f;
      else
        --distance;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        LaneObject laneObject = bufferData[index];
        Controller componentData;
        if (!(laneObject.m_LaneObject == this.m_Controller) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData) || !(componentData.m_Controller == this.m_Controller)))
        {
          if (exclusive)
          {
            float num = MathUtils.Clamp(VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance, this.m_SafeTimeStep), this.m_SpeedRange);
            if ((double) num < (double) this.m_MaxSpeed)
            {
              this.m_MaxSpeed = num;
              this.m_Blocker = laneObject.m_LaneObject;
              this.m_BlockerType = BlockerType.Continuing;
            }
          }
          else
          {
            float2 curvePosition = laneObject.m_CurvePosition;
            if ((double) curvePosition.y > (double) minOffset.y || (double) curvePosition.y >= 1.0 && (double) curvePosition.x > (double) minOffset.x)
            {
              float objectSpeed = this.GetObjectSpeed(laneObject.m_LaneObject, curvePosition.x);
              this.UpdateMaxSpeed(laneObject.m_LaneObject, BlockerType.Continuing, objectSpeed, curvePosition.x, 1f, distance);
            }
          }
        }
      }
    }

    private void CheckOverlappingLanes(
      float origDistance,
      float origMinOffset,
      int yieldOverride,
      bool exclusive)
    {
      DynamicBuffer<LaneOverlap> bufferData1;
      if (!this.m_LaneOverlapData.TryGetBuffer(this.m_Lane, out bufferData1) || bufferData1.Length == 0)
        return;
      float distance = origDistance - 10f;
      --origDistance;
      Bezier4x3 bezier = this.m_Curve.m_Bezier;
      float2 curveOffset = this.m_CurveOffset;
      float length = this.m_Curve.m_Length;
      int a = this.m_Priority;
      Game.Net.LaneReservation componentData1;
      if (this.m_LaneReservationData.TryGetComponent(this.m_Lane, out componentData1))
      {
        int priority = componentData1.GetPriority();
        a = math.select(a, 106, priority >= 108 & 106 > a);
      }
      for (int index1 = 0; index1 < bufferData1.Length; ++index1)
      {
        LaneOverlap laneOverlap = bufferData1[index1];
        float4 float4 = new float4((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd, (float) laneOverlap.m_OtherStart, (float) laneOverlap.m_OtherEnd) * 0.003921569f;
        if ((double) float4.y > (double) curveOffset.x)
        {
          BlockerType blockerType = (laneOverlap.m_Flags & (OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleEnd)) != (OverlapFlags) 0 ? BlockerType.Continuing : BlockerType.Crossing;
          Game.Net.TrackLane componentData2;
          DynamicBuffer<LaneObject> bufferData2;
          if (exclusive && this.m_TrackLaneData.TryGetComponent(laneOverlap.m_Other, out componentData2) && (componentData2.m_Flags & TrackLaneFlags.Exclusive) != (TrackLaneFlags) 0)
          {
            if (this.m_LaneReservationData.TryGetComponent(laneOverlap.m_Other, out componentData1) && componentData1.GetPriority() >= this.m_Priority)
            {
              float num = MathUtils.Clamp(VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance, this.m_SafeTimeStep), this.m_SpeedRange);
              if ((double) num < (double) this.m_MaxSpeed)
              {
                this.m_MaxSpeed = num;
                this.m_Blocker = componentData1.m_Blocker;
                this.m_BlockerType = blockerType;
              }
            }
            if (this.m_LaneObjectData.TryGetBuffer(laneOverlap.m_Other, out bufferData2) && bufferData2.Length != 0)
            {
              for (int index2 = 0; index2 < bufferData2.Length; ++index2)
              {
                LaneObject laneObject = bufferData2[index2];
                Controller componentData3;
                if (!(laneObject.m_LaneObject == this.m_Controller) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData3) || !(componentData3.m_Controller == this.m_Controller)))
                {
                  float num = MathUtils.Clamp(VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance, this.m_SafeTimeStep), this.m_SpeedRange);
                  if ((double) num < (double) this.m_MaxSpeed)
                  {
                    this.m_MaxSpeed = num;
                    this.m_Blocker = laneObject.m_LaneObject;
                    this.m_BlockerType = blockerType;
                  }
                }
              }
            }
          }
          else
          {
            this.m_Lane = laneOverlap.m_Other;
            this.m_Curve = this.m_CurveData[this.m_Lane];
            this.m_CurveOffset = float4.zw;
            Line3.Segment overlapLine = MathUtils.Line(bezier, float4.xy);
            float x = math.max(0.0f, origMinOffset - float4.x) + float4.z;
            float num1 = origDistance + length * (float4.x - curveOffset.x);
            float distanceFactor = (float) laneOverlap.m_Parallelism * (1f / 128f);
            int num2 = a;
            if ((laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart)) == (OverlapFlags) 0 && (double) float4.x > (double) origMinOffset)
            {
              int b = yieldOverride;
              if (this.m_LaneSignalData.HasComponent(this.m_Lane))
              {
                switch (this.m_LaneSignalData[this.m_Lane].m_Signal)
                {
                  case LaneSignalType.Stop:
                    ++b;
                    break;
                  case LaneSignalType.Yield:
                    --b;
                    break;
                }
              }
              int num3 = math.select((int) laneOverlap.m_PriorityDelta, b, b != 0);
              num2 -= num3;
              if (this.m_LaneReservationData.TryGetComponent(this.m_Lane, out componentData1))
              {
                double offset = (double) componentData1.GetOffset();
                int priority = componentData1.GetPriority();
                double num4 = (double) math.max(x, this.m_CurveOffset.x);
                if (offset > num4 | priority > num2)
                {
                  float num5 = MathUtils.Clamp(VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, num1, this.m_SafeTimeStep), this.m_SpeedRange);
                  if ((double) num5 < (double) this.m_MaxSpeed)
                  {
                    this.m_MaxSpeed = num5;
                    this.m_Blocker = componentData1.m_Blocker;
                    this.m_BlockerType = blockerType;
                  }
                }
              }
            }
            if (this.m_LaneObjectData.TryGetBuffer(this.m_Lane, out bufferData2) && bufferData2.Length != 0)
            {
              this.m_CurrentPosition = MathUtils.Position(this.m_Curve.m_Bezier, this.m_CurveOffset.x);
              for (int index3 = 0; index3 < bufferData2.Length; ++index3)
              {
                LaneObject laneObject = bufferData2[index3];
                Controller componentData4;
                if (!(laneObject.m_LaneObject == this.m_Controller) && (!this.m_ControllerData.TryGetComponent(laneObject.m_LaneObject, out componentData4) || !(componentData4.m_Controller == this.m_Controller)))
                {
                  if (this.m_CreatureData.HasComponent(laneObject.m_LaneObject))
                  {
                    this.CheckPedestrian(overlapLine, laneObject.m_LaneObject, laneObject.m_CurvePosition.y, num1, false);
                  }
                  else
                  {
                    float2 curvePosition = laneObject.m_CurvePosition;
                    float objectSpeed = this.GetObjectSpeed(laneObject.m_LaneObject, curvePosition.x);
                    if ((laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart)) == (OverlapFlags) 0 && ((double) float4.x >= (double) origMinOffset || (double) curvePosition.y > (double) float4.z))
                    {
                      int num6 = (!this.m_CarData.HasComponent(laneObject.m_LaneObject) ? (!this.m_TrainData.HasComponent(laneObject.m_LaneObject) ? 0 : VehicleUtils.GetPriority(this.m_PrefabTrainData[this.m_PrefabRefData[laneObject.m_LaneObject].m_Prefab])) : VehicleUtils.GetPriority(this.m_CarData[laneObject.m_LaneObject])) - num2;
                      if (num6 > 0)
                        curvePosition.y += objectSpeed * 2f / math.max(1f, this.m_Curve.m_Length);
                      else if (num6 < 0)
                        curvePosition.y -= math.max(0.0f, float4.z - x);
                    }
                    if ((double) curvePosition.y > (double) x)
                      this.UpdateMaxSpeed(laneObject.m_LaneObject, blockerType, objectSpeed, curvePosition.x, distanceFactor, num1);
                  }
                }
              }
            }
          }
        }
      }
    }

    private float GetObjectSpeed(Entity obj, float curveOffset)
    {
      return !this.m_MovingData.HasComponent(obj) ? 0.0f : math.dot(this.m_MovingData[obj].m_Velocity, math.normalizesafe(MathUtils.Tangent(this.m_Curve.m_Bezier, curveOffset)));
    }

    private void CheckPedestrian(
      Line3.Segment overlapLine,
      Entity obj,
      float targetOffset,
      float distanceOffset,
      bool giveSpace)
    {
      if ((double) targetOffset <= (double) this.m_CurveOffset.x | (double) targetOffset >= (double) this.m_CurveOffset.y)
      {
        PrefabRef prefabRef = this.m_PrefabRefData[obj];
        Transform transform = this.m_TransformData[obj];
        float num1 = this.m_PrefabObjectGeometry.m_Size.x * 0.5f;
        if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
        {
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
          num1 += objectGeometryData.m_Size.z * 0.5f;
        }
        float t;
        double num2 = (double) MathUtils.Distance(overlapLine.xz, transform.m_Position.xz, out t);
        float x = math.dot(math.forward(transform.m_Rotation).xz, math.normalizesafe(MathUtils.Position(overlapLine, t).xz - transform.m_Position.xz));
        double num3 = (double) math.select(math.min(-x, 0.0f), math.max(x, 0.0f), giveSpace);
        if (num2 - num3 >= (double) num1)
          return;
      }
      Moving componentData;
      float num = MathUtils.Clamp(this.m_PushBlockers || !this.m_MovingData.TryGetComponent(obj, out componentData) || (double) math.lengthsq(componentData.m_Velocity) < 0.0099999997764825821 ? math.max(3f, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distanceOffset, 3f, this.m_SafeTimeStep)) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distanceOffset, this.m_SafeTimeStep), this.m_SpeedRange);
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
      float distanceOffset)
    {
      PrefabRef prefabRef = this.m_PrefabRefData[obj];
      float num1 = 0.0f;
      if (this.m_PrefabTrainData.HasComponent(prefabRef.m_Prefab))
      {
        Train train = this.m_TrainData[obj];
        TrainData trainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
        float2 float2 = trainData.m_AttachOffsets - trainData.m_BogieOffsets;
        num1 = math.select(float2.y, float2.x, (train.m_Flags & Game.Vehicles.TrainFlags.Reversed) > (Game.Vehicles.TrainFlags) 0);
      }
      else if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
        num1 = -this.m_PrefabObjectGeometryData[prefabRef.m_Prefab].m_Bounds.min.z;
      if (((double) laneOffset - (double) this.m_CurveOffset.y) * (double) this.m_Curve.m_Length >= (double) num1)
        return;
      Transform transform = this.m_TransformData[obj];
      float x = math.distance(MathUtils.Position(this.m_Curve.m_Bezier, math.max(this.m_CurveOffset.x, laneOffset)), this.m_CurrentPosition) - math.max(0.0f, this.m_CurveOffset.x - laneOffset) * this.m_Curve.m_Length;
      float distance = (((double) math.dot(transform.m_Position - this.m_CurrentPosition, this.m_CurrentPosition - this.m_PrevPosition) >= 0.0 ? math.min(x, math.distance(transform.m_Position, this.m_CurrentPosition)) : math.min(x, math.distance(transform.m_Position, this.m_PrevPosition) + this.m_PrevDistance - this.m_Distance)) - num1) * distanceFactor + distanceOffset;
      float position;
      if ((double) objectSpeed > 1.0 / 1000.0 && this.m_PrefabCarData.HasComponent(prefabRef.m_Prefab))
      {
        CarData prefabCarData = this.m_PrefabCarData[prefabRef.m_Prefab];
        objectSpeed = math.max(0.0f, objectSpeed - (float) ((double) prefabCarData.m_Braking * (double) this.m_TimeStep * 2.0)) * distanceFactor;
        position = (double) this.m_PrefabTrain.m_Braking < (double) prefabCarData.m_Braking ? VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance + VehicleUtils.GetBrakingDistance(prefabCarData, objectSpeed, this.m_SafeTimeStep), this.m_SafeTimeStep) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance + objectSpeed * this.m_SafeTimeStep, objectSpeed, this.m_SafeTimeStep);
      }
      else if ((double) objectSpeed > 1.0 / 1000.0 && this.m_PrefabTrainData.HasComponent(prefabRef.m_Prefab))
      {
        TrainData prefabTrainData = this.m_PrefabTrainData[prefabRef.m_Prefab];
        objectSpeed = math.max(0.0f, objectSpeed - (float) ((double) prefabTrainData.m_Braking * (double) this.m_TimeStep * 2.0)) * distanceFactor;
        position = (double) this.m_PrefabTrain.m_Braking < (double) prefabTrainData.m_Braking ? VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance + VehicleUtils.GetBrakingDistance(prefabTrainData, objectSpeed, this.m_SafeTimeStep), this.m_SafeTimeStep) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance + objectSpeed * this.m_SafeTimeStep, objectSpeed, this.m_SafeTimeStep);
      }
      else
        position = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabTrain, distance, this.m_SafeTimeStep);
      float num2 = MathUtils.Clamp(position, this.m_SpeedRange);
      if ((double) num2 >= (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = num2;
      this.m_Blocker = obj;
      this.m_BlockerType = blockerType;
    }
  }
}
