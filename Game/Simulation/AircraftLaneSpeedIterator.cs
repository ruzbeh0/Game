// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AircraftLaneSpeedIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct AircraftLaneSpeedIterator
  {
    public ComponentLookup<Transform> m_TransformData;
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Aircraft> m_AircraftData;
    public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
    public ComponentLookup<Curve> m_CurveData;
    public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
    public ComponentLookup<PrefabRef> m_PrefabRefData;
    public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
    public ComponentLookup<AircraftData> m_PrefabAircraftData;
    public BufferLookup<LaneOverlap> m_LaneOverlapData;
    public BufferLookup<LaneObject> m_LaneObjectData;
    public Entity m_Entity;
    public Entity m_Ignore;
    public int m_Priority;
    public float m_TimeStep;
    public float m_SafeTimeStep;
    public AircraftData m_PrefabAircraft;
    public ObjectGeometryData m_PrefabObjectGeometry;
    public Bounds1 m_SpeedRange;
    public float m_MaxSpeed;
    public float m_CanChangeLane;
    public float3 m_CurrentPosition;
    public float m_Distance;
    public Entity m_Blocker;
    public BlockerType m_BlockerType;
    private Entity m_Lane;
    private Curve m_Curve;
    private float2 m_CurveOffset;
    private float3 m_PrevPosition;
    private float m_PrevDistance;

    public bool IterateFirstLane(Entity lane, float3 curveOffset)
    {
      Curve curve = this.m_CurveData[lane];
      float3 x = MathUtils.Position(curve.m_Bezier, curveOffset.x);
      this.m_PrevPosition = this.m_CurrentPosition;
      this.m_Distance = math.distance(this.m_CurrentPosition.xz, x.xz);
      if (this.m_CarLaneData.HasComponent(lane))
      {
        float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabAircraft, this.m_CarLaneData[lane]);
        if (this.m_Priority < 102 && this.m_LaneReservationData.HasComponent(lane) && this.m_LaneReservationData[lane].GetPriority() == 102)
          maxDriveSpeed *= 0.5f;
        if ((double) maxDriveSpeed < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(maxDriveSpeed, this.m_SpeedRange);
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.Limit;
        }
        float2 xy = curveOffset.xy;
        float num = this.m_Distance + -this.m_PrefabObjectGeometry.m_Bounds.max.z;
        this.m_Lane = lane;
        this.m_Curve = curve;
        this.m_CurveOffset = curveOffset.xz;
        this.m_CurrentPosition = x;
        this.CheckCurrentLane(num, xy);
        this.CheckOverlappingLanes(num, xy.y);
      }
      float3 y = MathUtils.Position(curve.m_Bezier, curveOffset.z);
      float s = math.abs(curveOffset.z - curveOffset.x);
      float num1 = math.max(1f / 1000f, math.lerp(math.distance(x, y), curve.m_Length * s, s));
      if ((double) num1 > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y;
      this.m_Distance += num1;
      return (double) this.m_Distance - 20.0 >= (double) VehicleUtils.GetBrakingDistance(this.m_PrefabAircraft, this.m_MaxSpeed, this.m_SafeTimeStep) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public bool IterateNextLane(Entity lane, float2 curveOffset, float minOffset)
    {
      if (!this.m_CurveData.HasComponent(lane))
        return false;
      Curve curve = this.m_CurveData[lane];
      if (this.m_CarLaneData.HasComponent(lane))
      {
        float maxDriveSpeed = VehicleUtils.GetMaxDriveSpeed(this.m_PrefabAircraft, this.m_CarLaneData[lane]);
        float num = this.m_Distance + -this.m_PrefabObjectGeometry.m_Bounds.max.z;
        float position = (double) maxDriveSpeed != 0.0 ? math.max(maxDriveSpeed, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, this.m_Distance, maxDriveSpeed, this.m_TimeStep)) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, num, this.m_SafeTimeStep);
        if ((double) position < (double) this.m_MaxSpeed)
        {
          this.m_MaxSpeed = MathUtils.Clamp(position, this.m_SpeedRange);
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.Limit;
        }
        this.m_Curve = curve;
        this.m_CurveOffset = curveOffset;
        this.m_Lane = lane;
        minOffset = math.select(minOffset, curveOffset.x, (double) curveOffset.x > 0.0);
        this.CheckCurrentLane(num, (float2) minOffset);
        this.CheckOverlappingLanes(num, minOffset);
      }
      float3 y = MathUtils.Position(curve.m_Bezier, curveOffset.y);
      float s = math.abs(curveOffset.y - curveOffset.x);
      float num1 = math.max(1f / 1000f, math.lerp(math.distance(this.m_CurrentPosition, y), curve.m_Length * s, s));
      if ((double) num1 > 1.0)
      {
        this.m_PrevPosition = this.m_CurrentPosition;
        this.m_PrevDistance = this.m_Distance;
      }
      this.m_CurrentPosition = y;
      this.m_Distance += num1;
      return (double) this.m_Distance - 20.0 >= (double) VehicleUtils.GetBrakingDistance(this.m_PrefabAircraft, this.m_MaxSpeed, this.m_SafeTimeStep) | (double) this.m_MaxSpeed == (double) this.m_SpeedRange.min;
    }

    public void IterateTarget(float3 targetPosition)
    {
      float maxBrakingSpeed = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, this.m_Distance, VehicleUtils.GetMaxDriveSpeed(this.m_PrefabAircraft, 11.1111116f, 0.2617994f), this.m_TimeStep);
      this.m_Distance += math.distance(this.m_CurrentPosition.xz, targetPosition.xz);
      float position = math.min(maxBrakingSpeed, VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, this.m_Distance, this.m_TimeStep));
      if ((double) position >= (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = MathUtils.Clamp(position, this.m_SpeedRange);
      this.m_Blocker = Entity.Null;
      this.m_BlockerType = BlockerType.None;
    }

    private void CheckCurrentLane(float distance, float2 minOffset)
    {
      if (!this.m_LaneObjectData.HasBuffer(this.m_Lane))
        return;
      DynamicBuffer<LaneObject> dynamicBuffer = this.m_LaneObjectData[this.m_Lane];
      if (dynamicBuffer.Length == 0)
        return;
      --distance;
      for (int index = 0; index < dynamicBuffer.Length; ++index)
      {
        LaneObject laneObject = dynamicBuffer[index];
        if (!(laneObject.m_LaneObject == this.m_Entity) && !(laneObject.m_LaneObject == this.m_Ignore))
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

    private void CheckOverlappingLanes(float origDistance, float origMinOffset)
    {
      if (!this.m_LaneOverlapData.HasBuffer(this.m_Lane))
        return;
      DynamicBuffer<LaneOverlap> dynamicBuffer1 = this.m_LaneOverlapData[this.m_Lane];
      if (dynamicBuffer1.Length == 0)
        return;
      --origDistance;
      float2 curveOffset = this.m_CurveOffset;
      float length = this.m_Curve.m_Length;
      int priority1 = this.m_Priority;
      for (int index1 = 0; index1 < dynamicBuffer1.Length; ++index1)
      {
        LaneOverlap laneOverlap = dynamicBuffer1[index1];
        float4 float4 = new float4((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd, (float) laneOverlap.m_OtherStart, (float) laneOverlap.m_OtherEnd) * 0.003921569f;
        if ((double) float4.y > (double) curveOffset.x)
        {
          this.m_Lane = laneOverlap.m_Other;
          this.m_Curve = this.m_CurveData[this.m_Lane];
          this.m_CurveOffset = float4.zw;
          float x = math.max(0.0f, origMinOffset - float4.x) + float4.z;
          float num1 = origDistance + length * (float4.x - curveOffset.x);
          float distanceFactor = (float) laneOverlap.m_Parallelism * (1f / 128f);
          int num2 = priority1;
          BlockerType blockerType = (laneOverlap.m_Flags & (OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleEnd)) != (OverlapFlags) 0 ? BlockerType.Continuing : BlockerType.Crossing;
          if ((laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart)) == (OverlapFlags) 0 && (double) float4.x > (double) origMinOffset)
          {
            num2 -= (int) laneOverlap.m_PriorityDelta;
            if (this.m_LaneReservationData.HasComponent(this.m_Lane))
            {
              Game.Net.LaneReservation laneReservation = this.m_LaneReservationData[this.m_Lane];
              double offset = (double) laneReservation.GetOffset();
              int priority2 = laneReservation.GetPriority();
              double num3 = (double) math.max(x, this.m_CurveOffset.x);
              if (offset > num3 | priority2 > num2)
              {
                float num4 = MathUtils.Clamp(VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, num1, this.m_SafeTimeStep), this.m_SpeedRange);
                if ((double) num4 < (double) this.m_MaxSpeed)
                {
                  this.m_MaxSpeed = num4;
                  this.m_Blocker = Entity.Null;
                  this.m_BlockerType = blockerType;
                }
              }
            }
          }
          if (this.m_LaneObjectData.HasBuffer(this.m_Lane))
          {
            DynamicBuffer<LaneObject> dynamicBuffer2 = this.m_LaneObjectData[this.m_Lane];
            if (dynamicBuffer2.Length != 0)
            {
              this.m_CurrentPosition = MathUtils.Position(this.m_Curve.m_Bezier, this.m_CurveOffset.x);
              for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
              {
                LaneObject laneObject = dynamicBuffer2[index2];
                if (!(laneObject.m_LaneObject == this.m_Ignore))
                {
                  float2 curvePosition = laneObject.m_CurvePosition;
                  float objectSpeed = this.GetObjectSpeed(laneObject.m_LaneObject, curvePosition.x);
                  if ((laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeMiddleStart)) == (OverlapFlags) 0 && ((double) float4.x >= (double) origMinOffset || (double) curvePosition.y > (double) float4.z))
                  {
                    int num5 = (!this.m_AircraftData.HasComponent(laneObject.m_LaneObject) ? 0 : VehicleUtils.GetPriority(this.m_PrefabAircraftData[this.m_PrefabRefData[laneObject.m_LaneObject].m_Prefab])) - num2;
                    if (num5 > 0)
                      curvePosition.y += objectSpeed * 2f / math.max(1f, this.m_Curve.m_Length);
                    else if (num5 < 0)
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

    private float GetObjectSpeed(Entity obj, float curveOffset)
    {
      return !this.m_MovingData.HasComponent(obj) ? 0.0f : math.dot(this.m_MovingData[obj].m_Velocity, math.normalizesafe(MathUtils.Tangent(this.m_Curve.m_Bezier, curveOffset)));
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
      if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
        num1 = math.max(0.0f, -this.m_PrefabObjectGeometryData[prefabRef.m_Prefab].m_Bounds.min.z);
      if (((double) laneOffset - (double) this.m_CurveOffset.y) * (double) this.m_Curve.m_Length >= (double) num1)
        return;
      Transform transform = this.m_TransformData[obj];
      float x = math.distance(MathUtils.Position(this.m_Curve.m_Bezier, math.max(this.m_CurveOffset.x, laneOffset)).xz, this.m_CurrentPosition.xz) - math.max(0.0f, this.m_CurveOffset.x - laneOffset) * this.m_Curve.m_Length;
      float distance = (((double) math.dot(transform.m_Position.xz - this.m_CurrentPosition.xz, this.m_CurrentPosition.xz - this.m_PrevPosition.xz) >= 0.0 ? math.min(x, math.distance(transform.m_Position.xz, this.m_CurrentPosition.xz)) : math.min(x, math.distance(transform.m_Position.xz, this.m_PrevPosition.xz) + this.m_PrevDistance - this.m_Distance)) - num1) * distanceFactor + distanceOffset;
      float position;
      if ((double) objectSpeed > 1.0 / 1000.0 && this.m_PrefabAircraftData.HasComponent(prefabRef.m_Prefab))
      {
        AircraftData prefabAircraftData = this.m_PrefabAircraftData[prefabRef.m_Prefab];
        objectSpeed = math.max(0.0f, objectSpeed - (float) ((double) prefabAircraftData.m_GroundBraking * (double) this.m_TimeStep * 2.0)) * distanceFactor;
        position = (double) this.m_PrefabAircraft.m_GroundBraking < (double) prefabAircraftData.m_GroundBraking ? VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, distance + VehicleUtils.GetBrakingDistance(prefabAircraftData, objectSpeed, this.m_SafeTimeStep), this.m_SafeTimeStep) : VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, distance + objectSpeed * this.m_SafeTimeStep, objectSpeed, this.m_SafeTimeStep);
      }
      else
        position = VehicleUtils.GetMaxBrakingSpeed(this.m_PrefabAircraft, distance, this.m_SafeTimeStep);
      float num2 = MathUtils.Clamp(position, this.m_SpeedRange);
      if ((double) num2 >= (double) this.m_MaxSpeed)
        return;
      this.m_MaxSpeed = num2;
      this.m_Blocker = obj;
      this.m_BlockerType = blockerType;
    }
  }
}
