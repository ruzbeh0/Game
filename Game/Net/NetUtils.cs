// Decompiled with JetBrains decompiler
// Type: Game.Net.NetUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  public static class NetUtils
  {
    public const float DEFAULT_ELEVATION_STEP = 10f;
    public const float MAX_LANE_WEAR = 10f;
    public const float MAX_LOCAL_CONNECT_DISTANCE = 4f;
    public const float MAX_LOCAL_CONNECT_HEIGHT = 100f;
    public const float MAX_SNAP_HEIGHT = 50f;
    public const float UTURN_LIMIT_COS = 0.7547096f;
    public const float TURN_LIMIT_COS = -0.4848096f;
    public const float GENTLETURN_LIMIT_COS = -0.9335804f;
    public const float MAX_PASSING_CURVINESS_STREET = 0.0174532924f;
    public const float MAX_PASSING_CURVINESS_HIGHWAY = 0.008726646f;
    public const float MIN_VISIBLE_EDGE_LENGTH = 0.1f;
    public const float MIN_VISIBLE_NODE_LENGTH = 0.05f;
    public const float MIN_VISIBLE_LANE_LENGTH = 0.1f;

    public static Bezier4x3 OffsetCurveLeftSmooth(Bezier4x3 curve, float2 offset)
    {
      float3 float3_1 = MathUtils.StartTangent(curve);
      float3 float3_2 = MathUtils.Tangent(curve, 0.5f);
      float3 float3_3 = MathUtils.EndTangent(curve);
      float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
      float3 float3_4 = MathUtils.Normalize(float3_2, float3_2.xz);
      float3 endTangent = MathUtils.Normalize(float3_3, float3_3.xz);
      startTangent.y = math.clamp(startTangent.y, -1f, 1f);
      endTangent.y = math.clamp(endTangent.y, -1f, 1f);
      float3 a = curve.a;
      float3 middlePos = MathUtils.Position(curve, 0.5f);
      float3 d = curve.d;
      float4 float4 = new float4(-offset, offset);
      a.xz += startTangent.zx * float4.xz;
      middlePos.xz += float3_4.zx * (float4.xz + float4.yw) * 0.5f;
      d.xz += endTangent.zx * float4.yw;
      return NetUtils.FitCurve(a, startTangent, middlePos, endTangent, d);
    }

    public static Bezier4x3 CircleCurve(float3 center, float xOffset, float zOffset)
    {
      Bezier4x3 bezier4x3 = new Bezier4x3(center, center, center, center);
      bezier4x3.a.x += xOffset;
      bezier4x3.b.x += xOffset;
      bezier4x3.b.z += zOffset * 0.5522848f;
      bezier4x3.c.x += xOffset * 0.5522848f;
      bezier4x3.c.z += zOffset;
      bezier4x3.d.z += zOffset;
      return bezier4x3;
    }

    public static Bezier4x3 CircleCurve(
      float3 center,
      quaternion rotation,
      float xOffset,
      float zOffset)
    {
      float2 xz = math.forward(rotation).xz;
      float2 float2 = MathUtils.Right(xz);
      Bezier4x3 bezier4x3 = new Bezier4x3(center, center, center, center);
      bezier4x3.a.xz += float2 * xOffset;
      bezier4x3.b.xz += float2 * xOffset;
      bezier4x3.b.xz += xz * (zOffset * 0.5522848f);
      bezier4x3.c.xz += float2 * (xOffset * 0.5522848f);
      bezier4x3.c.xz += xz * zOffset;
      bezier4x3.d.xz += xz * zOffset;
      return bezier4x3;
    }

    public static Bezier4x3 FitCurve(
      float3 startPos,
      float3 startTangent,
      float3 middlePos,
      float3 endTangent,
      float3 endPos)
    {
      Bezier4x3 curve = NetUtils.FitCurve(startPos, startTangent, endTangent, endPos);
      float3 float3 = middlePos - MathUtils.Position(curve, 0.5f);
      float2 x = new float2(math.dot(startTangent.xz, float3.xz), math.dot(endTangent.xz, float3.xz));
      float2 float2_1 = x * math.dot(startTangent.xz, endTangent.xz);
      float2 float2_2 = x * (math.abs(x) / math.max((float2) 1E-06f, 0.375f * (math.abs(x) + math.abs(float2_1.yx))));
      curve.b += startTangent * math.max(float2_2.x, math.min(0.0f, 1f - math.distance(curve.a.xz, curve.b.xz)));
      curve.c += endTangent * math.min(float2_2.y, math.max(0.0f, math.distance(curve.d.xz, curve.c.xz) - 1f));
      return curve;
    }

    public static Bezier4x3 FitCurve(
      float3 startPos,
      float3 startTangent,
      float3 endTangent,
      float3 endPos)
    {
      float b = math.distance(startPos.xz, endPos.xz);
      Line3 line1 = new Line3(startPos, startPos + startTangent);
      Line3 line2 = new Line3(endPos, endPos - endTangent);
      float2 t;
      float2 x = !MathUtils.Intersect(line1.xz, line2.xz, out t) ? (float2) (b * 0.75f) : math.clamp(t, (float2) (b * 0.01f), (float2) b);
      float num = math.dot(startTangent.xz, endTangent.xz);
      if ((double) num > 0.0)
        x = math.lerp(x, (float2) (b / math.sqrt((float) (2.0 * (double) num + 2.0))), math.min(1f, num * num));
      else if ((double) num < 0.0)
        x = math.lerp(x, (float2) (b * 1.20710683f), math.min(1f, num * num));
      return NetUtils.FitCurve(MathUtils.Cut(line1, new float2(0.0f, x.x)), MathUtils.Cut(line2, new float2(0.0f, x.y)));
    }

    public static Bezier4x3 FitCurve(Line3.Segment startLine, Line3.Segment endLine)
    {
      float3 float3_1 = MathUtils.Tangent(startLine);
      float3 float3_2 = MathUtils.Tangent(endLine);
      float x1 = math.length(float3_1.xz);
      float x2 = math.length(float3_2.xz);
      if ((double) x1 != 0.0)
      {
        float3_1 /= x1;
      }
      else
      {
        float3_1.xz = endLine.b.xz - startLine.a.xz;
        x1 = math.length(float3_1.xz);
        if ((double) x1 != 0.0)
          float3_1 /= x1;
      }
      if ((double) x2 != 0.0)
      {
        float3_2 /= x2;
      }
      else
      {
        float3_2.xz = startLine.b.xz - endLine.a.xz;
        x2 = math.length(float3_2.xz);
        if ((double) x2 != 0.0)
          float3_2 /= x2;
      }
      float3_1.y = math.clamp(float3_1.y, -1f, 1f);
      float3_2.y = math.clamp(float3_2.y, -1f, 1f);
      float num1 = math.acos(math.saturate(-math.dot(float3_1.xz, float3_2.xz)));
      float num2 = math.tan(num1 / 2f);
      float num3 = (float) (((double) x1 + (double) x2) * 0.1666666716337204);
      float y = (double) num2 < 9.9999997473787516E-05 ? num3 * 2f : num3 * (4f * math.tan(num1 / 4f) / num2);
      return new Bezier4x3()
      {
        a = startLine.a,
        b = startLine.a + float3_1 * math.min(x1, y),
        c = endLine.a + float3_2 * math.min(x2, y),
        d = endLine.a
      };
    }

    public static Bezier4x3 StraightCurve(float3 startPos, float3 endPos)
    {
      return new Bezier4x3()
      {
        a = startPos,
        b = math.lerp(startPos, endPos, 0.333333343f),
        c = math.lerp(startPos, endPos, 0.6666667f),
        d = endPos
      };
    }

    public static Bezier4x3 StraightCurve(float3 startPos, float3 endPos, float hanging)
    {
      Bezier4x3 bezier4x3 = new Bezier4x3();
      bezier4x3.a = startPos;
      bezier4x3.b = math.lerp(startPos, endPos, 0.333333343f);
      bezier4x3.c = math.lerp(startPos, endPos, 0.6666667f);
      bezier4x3.d = endPos;
      float num = (float) ((double) math.distance(bezier4x3.a.xz, bezier4x3.d.xz) * (double) hanging * 1.3333333730697632);
      bezier4x3.b.y -= num;
      bezier4x3.c.y -= num;
      return bezier4x3;
    }

    public static float FindMiddleTangentPos(Bezier4x2 curve, float2 offset)
    {
      float y1 = math.lerp(offset.x, offset.y, 0.5f);
      float num1 = y1;
      float2 x = MathUtils.Tangent(curve, offset.x);
      float2 float2_1 = MathUtils.Tangent(curve, offset.y);
      if (!MathUtils.TryNormalize(ref x) || !MathUtils.TryNormalize(ref float2_1))
        return y1;
      float2 float2_2 = offset;
      for (int index = 0; index < 24; ++index)
      {
        float2 y2 = MathUtils.Tangent(curve, num1);
        if (MathUtils.TryNormalize(ref y2))
        {
          float num2 = math.distancesq(x, y2);
          float num3 = math.distancesq(float2_1, y2);
          if ((double) num2 < (double) num3)
            float2_2.x = num1;
          else if ((double) num2 > (double) num3)
            float2_2.y = num1;
          else
            break;
          num1 = math.lerp(float2_2.x, float2_2.y, 0.5f);
        }
        else
          break;
      }
      return math.lerp(num1, y1, math.saturate((float) (0.5 + (double) math.dot(x, float2_1) * 0.5)));
    }

    public static float CalculateCurviness(Curve curve, float width)
    {
      if ((double) curve.m_Length <= 0.10000000149011612)
        return 0.0f;
      float3 tangent1 = MathUtils.StartTangent(curve.m_Bezier);
      float3 a = curve.m_Bezier.a;
      float3 float3_1 = MathUtils.Tangent(curve.m_Bezier, 0.25f);
      float3 float3_2 = MathUtils.Position(curve.m_Bezier, 0.25f);
      float3 float3_3 = MathUtils.Tangent(curve.m_Bezier, 0.5f);
      float3 float3_4 = MathUtils.Position(curve.m_Bezier, 0.5f);
      float3 float3_5 = MathUtils.Tangent(curve.m_Bezier, 0.75f);
      float3 float3_6 = MathUtils.Position(curve.m_Bezier, 0.75f);
      float3 tangent2 = MathUtils.EndTangent(curve.m_Bezier);
      float3 d = curve.m_Bezier.d;
      float4 x;
      x.x = NetUtils.CalculateCurviness(a, tangent1, float3_2, float3_1);
      x.y = NetUtils.CalculateCurviness(float3_2, float3_1, float3_4, float3_3);
      x.z = NetUtils.CalculateCurviness(float3_4, float3_3, float3_6, float3_5);
      x.w = NetUtils.CalculateCurviness(float3_6, float3_5, d, tangent2);
      float curviness = math.cmax(x);
      if ((double) curve.m_Length < (double) width * 2.0)
        curviness = math.lerp(math.min(curviness, NetUtils.CalculateCurviness(a, tangent1, d, tangent2)), curviness, math.smoothstep(width * 0.1f, width * 2f, curve.m_Length));
      return curviness;
    }

    public static float CalculateStartCurviness(Curve curve, float width)
    {
      if ((double) curve.m_Length <= 0.10000000149011612)
        return 0.0f;
      float3 tangent1 = MathUtils.StartTangent(curve.m_Bezier);
      float3 a = curve.m_Bezier.a;
      float3 float3_1 = MathUtils.Tangent(curve.m_Bezier, 0.25f);
      float3 float3_2 = MathUtils.Position(curve.m_Bezier, 0.25f);
      float3 tangent2 = MathUtils.Tangent(curve.m_Bezier, 0.5f);
      float3 position2 = MathUtils.Position(curve.m_Bezier, 0.5f);
      float2 x;
      x.x = NetUtils.CalculateCurviness(a, tangent1, float3_2, float3_1);
      x.y = NetUtils.CalculateCurviness(float3_2, float3_1, position2, tangent2);
      return math.cmax(x);
    }

    public static float CalculateEndCurviness(Curve curve, float width)
    {
      if ((double) curve.m_Length <= 0.10000000149011612)
        return 0.0f;
      float3 tangent1 = MathUtils.Tangent(curve.m_Bezier, 0.5f);
      float3 position1 = MathUtils.Position(curve.m_Bezier, 0.5f);
      float3 float3_1 = MathUtils.Tangent(curve.m_Bezier, 0.75f);
      float3 float3_2 = MathUtils.Position(curve.m_Bezier, 0.75f);
      float3 tangent2 = MathUtils.EndTangent(curve.m_Bezier);
      float3 d = curve.m_Bezier.d;
      float2 x;
      x.x = NetUtils.CalculateCurviness(position1, tangent1, float3_2, float3_1);
      x.y = NetUtils.CalculateCurviness(float3_2, float3_1, d, tangent2);
      return math.cmax(x);
    }

    public static float CalculateCurviness(
      float3 position1,
      float3 tangent1,
      float3 position2,
      float3 tangent2)
    {
      float distance = math.distance(position1, position2);
      return MathUtils.TryNormalize(ref tangent1) && MathUtils.TryNormalize(ref tangent2) && (double) distance >= 9.9999999747524271E-07 ? NetUtils.CalculateCurviness(tangent1, tangent2, distance) : 0.0f;
    }

    public static float CalculateCurviness(float3 tangent1, float3 tangent2, float distance)
    {
      return 2f * math.sin(math.acos(math.clamp(math.dot(tangent1, tangent2), -1f, 1f)) * 0.5f) / distance;
    }

    public static quaternion GetNodeRotation(float3 tangent)
    {
      return NetUtils.GetNodeRotation(tangent, quaternion.identity);
    }

    public static quaternion GetNodeRotation(float3 tangent, quaternion defaultRotation)
    {
      tangent.y = 0.0f;
      return MathUtils.TryNormalize(ref tangent) ? quaternion.LookRotation(tangent, math.up()) : defaultRotation;
    }

    public static float ExtendedDistance(Bezier4x2 curve, float2 position, out float t)
    {
      float t1;
      float num1 = MathUtils.Distance(new Line2(curve.a, curve.a * 2f - curve.b), position, out t1);
      float t2;
      float num2 = MathUtils.Distance(curve, position, out t2);
      float t3;
      float num3 = MathUtils.Distance(new Line2(curve.d, curve.d * 2f - curve.c), position, out t3);
      if ((double) t1 >= 0.0 & (double) num1 < (double) num2 & ((double) num1 < (double) num3 | (double) t3 < 0.0))
      {
        t = -t1;
        return num1;
      }
      if ((double) t3 >= 0.0 & (double) num3 < (double) num2)
      {
        t = 1f + t3;
        return num3;
      }
      t = t2;
      return num2;
    }

    public static float ExtendedLength(Bezier4x2 curve, float t)
    {
      if ((double) t <= 0.0)
        return math.distance(curve.a, curve.b) * t;
      return (double) t <= 1.0 ? MathUtils.Length(curve, new Bounds1(0.0f, t)) : MathUtils.Length(curve) + math.distance(curve.c, curve.d) * (t - 1f);
    }

    public static float ExtendedClampLength(Bezier4x2 curve, float distance)
    {
      if ((double) distance <= 0.0)
      {
        float num = math.distance(curve.a, curve.b);
        return math.select(distance / num, 0.0f, (double) num == 0.0);
      }
      Bounds1 t = new Bounds1(0.0f, 1f);
      if (MathUtils.ClampLength(curve, ref t, distance))
        return t.max;
      distance -= MathUtils.Length(curve);
      float num1 = math.distance(curve.c, curve.d);
      return math.select((float) (1.0 + (double) distance / (double) num1), 1f, (double) num1 == 0.0);
    }

    public static void ExtendedPositionAndTangent(
      Bezier4x3 curve,
      float t,
      out float3 position,
      out float3 tangent)
    {
      if ((double) t <= 0.0)
      {
        position = MathUtils.Position(new Line3(curve.a, curve.a * 2f - curve.b), -t);
        tangent = curve.b - curve.a;
      }
      else if ((double) t <= 1.0)
      {
        position = MathUtils.Position(curve, t);
        tangent = MathUtils.Tangent(curve, t);
      }
      else
      {
        position = MathUtils.Position(new Line3(curve.d, curve.d * 2f - curve.c), t - 1f);
        tangent = curve.d - curve.c;
      }
    }

    public static int ChooseClosestLane(
      int minIndex,
      int maxIndex,
      float3 comparePosition,
      DynamicBuffer<SubLane> lanes,
      ComponentLookup<Curve> curves,
      float curvePosition)
    {
      float num1 = float.MaxValue;
      int num2 = minIndex;
      maxIndex = math.min(maxIndex, lanes.Length - 1);
      for (int index = minIndex; index <= maxIndex; ++index)
      {
        Entity subLane = lanes[index].m_SubLane;
        float num3 = MathUtils.DistanceSquared(curves[subLane].m_Bezier, comparePosition, out float _);
        if ((double) num3 < (double) num1)
        {
          num1 = num3;
          num2 = index;
        }
      }
      return num2;
    }

    public static float GetAvailability(
      DynamicBuffer<ResourceAvailability> availabilities,
      AvailableResource resource,
      float curvePos)
    {
      if (resource >= (AvailableResource) availabilities.Length)
        return 0.0f;
      float2 availability = availabilities[(int) resource].m_Availability;
      return math.lerp(availability.x, availability.y, curvePos);
    }

    public static float GetServiceCoverage(
      DynamicBuffer<ServiceCoverage> coverages,
      CoverageService service,
      float curvePos)
    {
      ServiceCoverage coverage = coverages[(int) service];
      return math.lerp(coverage.m_Coverage.x, coverage.m_Coverage.y, curvePos);
    }

    public static void AddLaneObject(
      DynamicBuffer<LaneObject> buffer,
      Entity laneObject,
      float2 curvePosition)
    {
      for (int index = 0; index < buffer.Length; ++index)
      {
        if ((double) buffer[index].m_CurvePosition.y >= (double) curvePosition.y)
        {
          buffer.Insert(index, new LaneObject(laneObject, curvePosition));
          return;
        }
      }
      buffer.Add(new LaneObject(laneObject, curvePosition));
    }

    public static void UpdateLaneObject(
      DynamicBuffer<LaneObject> buffer,
      Entity laneObject,
      float2 curvePosition)
    {
      LaneObject elem = new LaneObject(laneObject, curvePosition);
      for (int index1 = 0; index1 < buffer.Length; ++index1)
      {
        LaneObject laneObject1 = buffer[index1];
        if (laneObject1.m_LaneObject == laneObject)
        {
          for (int index2 = index1 + 1; index2 < buffer.Length; ++index2)
          {
            LaneObject laneObject2 = buffer[index2];
            if ((double) laneObject2.m_CurvePosition.y >= (double) curvePosition.y)
            {
              buffer[index2 - 1] = elem;
              return;
            }
            buffer[index2 - 1] = laneObject2;
          }
          buffer[buffer.Length - 1] = elem;
          return;
        }
        if ((double) laneObject1.m_CurvePosition.y >= (double) curvePosition.y)
        {
          buffer[index1] = elem;
          elem = laneObject1;
          for (int index3 = index1 + 1; index3 < buffer.Length; ++index3)
          {
            LaneObject laneObject3 = buffer[index3];
            buffer[index3] = elem;
            elem = laneObject3;
            if (elem.m_LaneObject == laneObject)
              return;
          }
          break;
        }
      }
      buffer.Add(elem);
    }

    public static void RemoveLaneObject(DynamicBuffer<LaneObject> buffer, Entity laneObject)
    {
      CollectionUtils.RemoveValue<LaneObject>(buffer, new LaneObject(laneObject));
    }

    public static bool CanConnect(NetData netData1, NetData netData2)
    {
      return (netData1.m_RequiredLayers & netData2.m_ConnectLayers) == netData1.m_RequiredLayers || (netData2.m_RequiredLayers & netData1.m_ConnectLayers) == netData2.m_RequiredLayers;
    }

    public static bool FindConnectedLane(
      ref Entity laneEntity,
      ref bool forward,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Edge> edgeData,
      ref BufferLookup<ConnectedEdge> connectedEdges,
      ref BufferLookup<SubLane> subLanes)
    {
      Lane lane1 = laneData[laneEntity];
      Entity entity = ownerData[laneEntity].m_Owner;
      PathNode other = forward ? lane1.m_EndNode : lane1.m_StartNode;
      if (edgeLaneData.HasComponent(laneEntity))
      {
        EdgeLane edgeLane = edgeLaneData[laneEntity];
        float num = forward ? edgeLane.m_EdgeDelta.y : edgeLane.m_EdgeDelta.x;
        if ((double) num == 0.0)
          entity = edgeData[entity].m_Start;
        else if ((double) num == 1.0)
          entity = edgeData[entity].m_End;
        DynamicBuffer<SubLane> dynamicBuffer = subLanes[entity];
        for (int index = 0; index < dynamicBuffer.Length; ++index)
        {
          Entity subLane = dynamicBuffer[index].m_SubLane;
          if (!(subLane == laneEntity))
          {
            Lane lane2 = laneData[subLane];
            if (lane2.m_StartNode.Equals(other))
            {
              laneEntity = subLane;
              forward = true;
              return true;
            }
            if (lane2.m_EndNode.Equals(other))
            {
              laneEntity = subLane;
              forward = false;
              return true;
            }
          }
        }
      }
      else
      {
        DynamicBuffer<ConnectedEdge> bufferData;
        if (connectedEdges.TryGetBuffer(entity, out bufferData))
        {
          for (int index1 = 0; index1 < bufferData.Length; ++index1)
          {
            Entity edge = bufferData[index1].m_Edge;
            DynamicBuffer<SubLane> dynamicBuffer = subLanes[edge];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity subLane = dynamicBuffer[index2].m_SubLane;
              if (!(subLane == laneEntity))
              {
                Lane lane3 = laneData[subLane];
                if (lane3.m_StartNode.Equals(other))
                {
                  laneEntity = subLane;
                  forward = true;
                  return true;
                }
                if (lane3.m_EndNode.Equals(other))
                {
                  laneEntity = subLane;
                  forward = false;
                  return true;
                }
              }
            }
          }
        }
      }
      return false;
    }

    public static CollisionMask GetCollisionMask(
      NetCompositionData compositionData,
      bool ignoreMarkers)
    {
      if ((compositionData.m_State & CompositionState.NoSubCollisions) != 0 & ignoreMarkers)
        return (CollisionMask) 0;
      CollisionMask collisionMask1 = (CollisionMask) 0;
      if ((compositionData.m_State & CompositionState.ExclusiveGround) != (CompositionState) 0)
        collisionMask1 |= CollisionMask.OnGround | CollisionMask.ExclusiveGround;
      CollisionMask collisionMask2 = (compositionData.m_Flags.m_General & CompositionFlags.General.Elevated) == (CompositionFlags.General) 0 ? ((compositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0 ? ((compositionData.m_State & CompositionState.HasSurface) == (CompositionState) 0 ? collisionMask1 | CollisionMask.Overground : collisionMask1 | CollisionMask.OnGround | CollisionMask.Overground) : collisionMask1 | CollisionMask.Underground) : collisionMask1 | CollisionMask.Overground;
      if (((compositionData.m_Flags.m_Left | compositionData.m_Flags.m_Right) & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
        collisionMask2 |= CollisionMask.Underground;
      return collisionMask2;
    }

    public static CollisionMask GetCollisionMask(LabelPosition labelPosition)
    {
      return !labelPosition.m_IsUnderground ? CollisionMask.Overground : CollisionMask.Underground;
    }

    public static bool IsTurn(
      float2 startPosition,
      float2 startDirection,
      float2 endPosition,
      float2 endDirection,
      out bool right,
      out bool gentle,
      out bool uturn)
    {
      float2 x = MathUtils.Right(startDirection);
      float4 float4;
      float4.y = math.dot(startDirection, endDirection);
      float4.w = math.dot(x, endDirection);
      float num = math.distance(startPosition, endPosition);
      if ((double) num > 0.10000000149011612)
      {
        float2 y = (startPosition - endPosition) / num;
        float4.x = math.dot(startDirection, y);
        float4.z = math.dot(x, y);
      }
      else
      {
        float4.x = -1f;
        float4.z = 0.0f;
      }
      float4 = math.lerp(float4, new float4(-1f, -1f, float4.wz), math.saturate(new float4(float4.z * float4.w * new float2(-2f, -4f), float4.xy)));
      float4 = math.select(float4, float4.yxwz, (double) float4.y > (double) float4.x);
      right = (double) float4.z < 0.0;
      gentle = (double) float4.x > -0.93358039855957031 & (double) float4.x <= -0.48480960726737976;
      uturn = (double) float4.x > 0.75470960140228271;
      return (double) float4.x > -0.93358039855957031;
    }

    public static int GetConstructionCost(
      Curve curve,
      Elevation startElevation,
      Elevation endElevation,
      PlaceableNetComposition placeableNetData)
    {
      int num1 = math.max(1, Mathf.RoundToInt(curve.m_Length / 8f));
      int num2 = math.max(0, Mathf.RoundToInt(math.max(math.cmin(startElevation.m_Elevation), math.cmin(endElevation.m_Elevation)) / 10f));
      int num3 = (int) placeableNetData.m_ConstructionCost + num2 * (int) placeableNetData.m_ElevationCost;
      return num1 * num3;
    }

    public static int GetUpkeepCost(Curve curve, PlaceableNetComposition placeableNetData)
    {
      return math.max(1, Mathf.RoundToInt(math.max(1f, math.round(curve.m_Length / 8f)) * placeableNetData.m_UpkeepCost));
    }

    public static int GetRefundAmount(
      Recent recent,
      uint simulationFrame,
      EconomyParameterData economyParameterData)
    {
      if ((double) simulationFrame < (double) recent.m_ModificationFrame + 262144.0 * (double) economyParameterData.m_RoadRefundTimeRange.x)
        return (int) ((double) recent.m_ModificationCost * (double) economyParameterData.m_RoadRefundPercentage.x);
      if ((double) simulationFrame < (double) recent.m_ModificationFrame + 262144.0 * (double) economyParameterData.m_RoadRefundTimeRange.y)
        return (int) ((double) recent.m_ModificationCost * (double) economyParameterData.m_RoadRefundPercentage.y);
      return (double) simulationFrame < (double) recent.m_ModificationFrame + 262144.0 * (double) economyParameterData.m_RoadRefundTimeRange.z ? (int) ((double) recent.m_ModificationCost * (double) economyParameterData.m_RoadRefundPercentage.z) : 0;
    }

    public static int GetUpgradeCost(int newCost, int oldCost) => math.max(0, newCost - oldCost);

    public static int GetUpgradeCost(
      int newCost,
      int oldCost,
      Recent recent,
      uint simulationFrame,
      EconomyParameterData economyParameterData)
    {
      if (newCost >= oldCost)
        return NetUtils.GetUpgradeCost(newCost, oldCost);
      recent.m_ModificationCost = math.min(recent.m_ModificationCost, oldCost - newCost);
      return -NetUtils.GetRefundAmount(recent, simulationFrame, economyParameterData);
    }

    public static bool FindNextLane(
      ref Entity entity,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref BufferLookup<SubLane> subLanes)
    {
      Owner componentData1;
      Lane componentData2;
      DynamicBuffer<SubLane> bufferData;
      if (!ownerData.TryGetComponent(entity, out componentData1) || !laneData.TryGetComponent(entity, out componentData2) || !subLanes.TryGetBuffer(componentData1.m_Owner, out bufferData))
        return false;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        Entity subLane = bufferData[index].m_SubLane;
        Lane lane = laneData[subLane];
        if (componentData2.m_EndNode.Equals(lane.m_StartNode))
        {
          entity = subLane;
          return true;
        }
      }
      return false;
    }

    public static bool FindPrevLane(
      ref Entity entity,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref BufferLookup<SubLane> subLanes)
    {
      Owner componentData1;
      Lane componentData2;
      DynamicBuffer<SubLane> bufferData;
      if (!ownerData.TryGetComponent(entity, out componentData1) || !laneData.TryGetComponent(entity, out componentData2) || !subLanes.TryGetBuffer(componentData1.m_Owner, out bufferData))
        return false;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        Entity subLane = bufferData[index].m_SubLane;
        Lane lane = laneData[subLane];
        if (componentData2.m_StartNode.Equals(lane.m_EndNode))
        {
          entity = subLane;
          return true;
        }
      }
      return false;
    }

    public static bool FindEdgeLane(
      ref Entity entity,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Lane> laneData,
      ref BufferLookup<SubLane> subLanes,
      bool startNode)
    {
      Owner componentData1;
      Lane componentData2;
      DynamicBuffer<SubLane> bufferData;
      if (!ownerData.TryGetComponent(entity, out componentData1) || !laneData.TryGetComponent(entity, out componentData2) || !subLanes.TryGetBuffer(componentData1.m_Owner, out bufferData))
        return false;
      PathNode pathNode = startNode ? componentData2.m_StartNode : componentData2.m_EndNode;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        Entity subLane = bufferData[index].m_SubLane;
        Lane lane = laneData[subLane];
        if (pathNode.EqualsIgnoreCurvePos(lane.m_MiddleNode))
        {
          entity = subLane;
          return true;
        }
      }
      return false;
    }

    public static float4 GetTrafficFlowSpeed(Road road)
    {
      return NetUtils.GetTrafficFlowSpeed(road.m_TrafficFlowDuration0 + road.m_TrafficFlowDuration1, road.m_TrafficFlowDistance0 + road.m_TrafficFlowDistance1);
    }

    public static float4 GetTrafficFlowSpeed(float4 duration, float4 distance)
    {
      return math.saturate(distance / duration);
    }

    public static float GetTrafficFlowSpeed(float duration, float distance)
    {
      return math.saturate(distance / duration);
    }

    public static Node AdjustPosition(Node node, ref TerrainHeightData terrainHeightData)
    {
      Node node1 = node;
      node1.m_Position.y = TerrainUtils.SampleHeight(ref terrainHeightData, node.m_Position);
      return node1;
    }

    public static Node AdjustPosition(Node node, ref BuildingUtils.LotInfo lotInfo)
    {
      Node node1 = node;
      double num = (double) BuildingUtils.SampleHeight(ref lotInfo, node.m_Position);
      return node1;
    }

    public static Curve AdjustPosition(
      Curve curve,
      bool fixedStart,
      bool linearMiddle,
      bool fixedEnd,
      ref TerrainHeightData terrainHeightData)
    {
      Curve curve1 = curve;
      if (!fixedStart)
        curve1.m_Bezier.a.y = TerrainUtils.SampleHeight(ref terrainHeightData, curve.m_Bezier.a);
      if (!fixedEnd)
        curve1.m_Bezier.d.y = TerrainUtils.SampleHeight(ref terrainHeightData, curve.m_Bezier.d);
      if (linearMiddle)
      {
        curve1.m_Bezier.b.y = math.lerp(curve1.m_Bezier.a.y, curve1.m_Bezier.d.y, 0.333333343f);
        curve1.m_Bezier.c.y = math.lerp(curve1.m_Bezier.a.y, curve1.m_Bezier.d.y, 0.6666667f);
      }
      else
      {
        curve1.m_Bezier.b.y = TerrainUtils.SampleHeight(ref terrainHeightData, curve.m_Bezier.b);
        curve1.m_Bezier.c.y = TerrainUtils.SampleHeight(ref terrainHeightData, curve.m_Bezier.c);
        float num1 = curve1.m_Bezier.b.y - MathUtils.Position(curve1.m_Bezier.y, 0.333333343f);
        float num2 = curve1.m_Bezier.c.y - MathUtils.Position(curve1.m_Bezier.y, 0.6666667f);
        curve1.m_Bezier.b.y += (float) ((double) num1 * 3.0 - (double) num2 * 1.5);
        curve1.m_Bezier.c.y += (float) ((double) num2 * 3.0 - (double) num1 * 1.5);
      }
      return curve1;
    }

    public static Curve AdjustPosition(
      Curve curve,
      bool fixedStart,
      bool linearMiddle,
      bool fixedEnd,
      ref TerrainHeightData terrainHeightData,
      ref WaterSurfaceData waterSurfaceData)
    {
      Curve curve1 = curve;
      if (!fixedStart)
        curve1.m_Bezier.a.y = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, curve.m_Bezier.a);
      if (!fixedEnd)
        curve1.m_Bezier.d.y = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, curve.m_Bezier.d);
      if (linearMiddle)
      {
        curve1.m_Bezier.b.y = math.lerp(curve1.m_Bezier.a.y, curve1.m_Bezier.d.y, 0.333333343f);
        curve1.m_Bezier.c.y = math.lerp(curve1.m_Bezier.a.y, curve1.m_Bezier.d.y, 0.6666667f);
      }
      else
      {
        curve1.m_Bezier.b.y = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, curve.m_Bezier.b);
        curve1.m_Bezier.c.y = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, curve.m_Bezier.c);
        float num1 = curve1.m_Bezier.b.y - MathUtils.Position(curve1.m_Bezier.y, 0.333333343f);
        float num2 = curve1.m_Bezier.c.y - MathUtils.Position(curve1.m_Bezier.y, 0.6666667f);
        curve1.m_Bezier.b.y += (float) ((double) num1 * 3.0 - (double) num2 * 1.5);
        curve1.m_Bezier.c.y += (float) ((double) num2 * 3.0 - (double) num1 * 1.5);
      }
      return curve1;
    }

    public static Curve AdjustPosition(
      Curve curve,
      bool2 fixedStart,
      bool linearMiddle,
      bool2 fixedEnd,
      ref BuildingUtils.LotInfo lotInfo)
    {
      Curve curve1 = curve;
      if (!fixedStart.x)
        curve1.m_Bezier.a.y = BuildingUtils.SampleHeight(ref lotInfo, curve.m_Bezier.a);
      if (!fixedEnd.x)
        curve1.m_Bezier.d.y = BuildingUtils.SampleHeight(ref lotInfo, curve.m_Bezier.d);
      if (linearMiddle)
      {
        if (!fixedStart.y)
          curve1.m_Bezier.b.y = math.lerp(curve1.m_Bezier.a.y, curve1.m_Bezier.d.y, 0.333333343f);
        if (!fixedEnd.y)
          curve1.m_Bezier.c.y = math.lerp(curve1.m_Bezier.a.y, curve1.m_Bezier.d.y, 0.6666667f);
      }
      else
      {
        if (!fixedStart.y)
          curve1.m_Bezier.b.y = BuildingUtils.SampleHeight(ref lotInfo, curve.m_Bezier.b);
        if (!fixedEnd.y)
          curve1.m_Bezier.c.y = BuildingUtils.SampleHeight(ref lotInfo, curve.m_Bezier.c);
        float num1 = curve1.m_Bezier.b.y - MathUtils.Position(curve1.m_Bezier.y, 0.333333343f);
        float num2 = curve1.m_Bezier.c.y - MathUtils.Position(curve1.m_Bezier.y, 0.6666667f);
        if (!fixedStart.y)
          curve1.m_Bezier.b.y += (float) ((double) num1 * 3.0 - (double) num2 * 1.5);
        if (!fixedEnd.y)
          curve1.m_Bezier.c.y += (float) ((double) num2 * 3.0 - (double) num1 * 1.5);
      }
      return curve1;
    }

    public static Game.Prefabs.SubNet GetSubNet(
      DynamicBuffer<Game.Prefabs.SubNet> subNets,
      int index,
      bool lefthandTraffic,
      ref ComponentLookup<NetGeometryData> netGeometryLookup)
    {
      Game.Prefabs.SubNet subNet = subNets[index];
      if (subNet.m_InvertMode == NetInvertMode.LefthandTraffic & lefthandTraffic || subNet.m_InvertMode == NetInvertMode.RighthandTraffic && !lefthandTraffic)
      {
        NetGeometryData componentData;
        if (netGeometryLookup.TryGetComponent(subNet.m_Prefab, out componentData) && (componentData.m_Flags & GeometryFlags.FlipTrafficHandedness) != (GeometryFlags) 0)
        {
          subNet.m_Curve = MathUtils.Invert(subNet.m_Curve);
          subNet.m_NodeIndex = subNet.m_NodeIndex.yx;
          subNet.m_ParentMesh = subNet.m_ParentMesh.yx;
        }
        else
          NetUtils.FlipUpgradeTrafficHandedness(ref subNet.m_Upgrades);
      }
      return subNet;
    }

    public static void FlipUpgradeTrafficHandedness(ref CompositionFlags flags)
    {
      uint left = (uint) flags.m_Left;
      uint right = (uint) flags.m_Right;
      CommonUtils.SwapBits(ref left, 16777216U, 33554432U);
      CommonUtils.SwapBits(ref right, 16777216U, 33554432U);
      flags.m_Left = (CompositionFlags.Side) right;
      flags.m_Right = (CompositionFlags.Side) left;
    }

    public static float GetTerrainSmoothingWidth(NetData netData)
    {
      if ((netData.m_RequiredLayers & (Layer.Taxiway | Layer.MarkerTaxiway)) != Layer.None)
        return 100f;
      return (netData.m_RequiredLayers & Layer.Waterway) != Layer.None ? 20f : 8f;
    }

    public static int GetParkingSlotCount(
      Curve curve,
      ParkingLane parkingLane,
      ParkingLaneData prefabParkingLane)
    {
      return (int) math.floor((NetUtils.GetParkingSlotSpace(curve, parkingLane, prefabParkingLane) + 0.01f) / prefabParkingLane.m_SlotInterval);
    }

    public static float GetParkingSlotInterval(
      Curve curve,
      ParkingLane parkingLane,
      ParkingLaneData prefabParkingLane,
      int slotCount)
    {
      return slotCount == 0 || (parkingLane.m_Flags & ParkingLaneFlags.FindConnections) != (ParkingLaneFlags) 0 ? prefabParkingLane.m_SlotInterval : NetUtils.GetParkingSlotSpace(curve, parkingLane, prefabParkingLane) / (float) slotCount;
    }

    private static float GetParkingSlotSpace(
      Curve curve,
      ParkingLane parkingLane,
      ParkingLaneData prefabParkingLane)
    {
      float parkingSlotSpace = curve.m_Length;
      if ((parkingLane.m_Flags & ParkingLaneFlags.FindConnections) == (ParkingLaneFlags) 0)
      {
        parkingSlotSpace = parkingSlotSpace - math.select(0.0f, 0.2f, (parkingLane.m_Flags & ParkingLaneFlags.StartingLane) != 0) - math.select(0.0f, 0.2f, (parkingLane.m_Flags & ParkingLaneFlags.EndingLane) != 0);
        if ((double) prefabParkingLane.m_SlotAngle > 0.25)
        {
          float2 y = new float2(math.cos(prefabParkingLane.m_SlotAngle), math.sin(prefabParkingLane.m_SlotAngle));
          float num = math.min(math.dot(prefabParkingLane.m_SlotSize, y), prefabParkingLane.m_SlotSize.y);
          switch (parkingLane.m_Flags & (ParkingLaneFlags.StartingLane | ParkingLaneFlags.EndingLane))
          {
            case ParkingLaneFlags.StartingLane:
            case ParkingLaneFlags.EndingLane:
              parkingSlotSpace -= num * 0.5f * math.tan(1.57079637f - prefabParkingLane.m_SlotAngle);
              break;
            case ParkingLaneFlags.StartingLane | ParkingLaneFlags.EndingLane:
              parkingSlotSpace -= num * math.tan(1.57079637f - prefabParkingLane.m_SlotAngle);
              break;
          }
        }
      }
      return parkingSlotSpace;
    }
  }
}
