// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Net;
using Unity.Collections;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public static class ToolUtils
  {
    public const float WATER_DEPTH_LIMIT = 0.2f;
    public const int MAX_ACTIVE_INFOMODES = 100;
    public const int INFOMODE_COLOR_GROUP_COUNT = 3;
    public const int INFOMODE_COLOR_GROUP_SIZE = 4;
    public const int INFOMODE_COLOR_GROUP_TERRAIN = 0;
    public const int INFOMODE_COLOR_GROUP_WATER = 1;
    public const int INFOMODE_COLOR_GROUP_OTHER = 2;

    public static quaternion CalculateRotation(float2 direction)
    {
      return direction.Equals(new float2()) ? quaternion.identity : quaternion.LookRotation(new float3(direction.x, 0.0f, direction.y), math.up());
    }

    public static float2 CalculateSnapPriority(
      float level,
      float priority,
      float2 origPos,
      float2 newPos,
      float2 direction)
    {
      float2 float2 = newPos - origPos;
      float2 = new float2(math.dot(float2, direction), math.dot(float2, MathUtils.Right(direction)));
      return ToolUtils.CalculateSnapPriority(level, priority, float2);
    }

    public static float2 CalculateSnapPriority(float level, float priority, float2 offset)
    {
      offset /= 8f;
      offset *= offset;
      float num1 = math.min(1f, offset.x + offset.y);
      float num2 = math.max(offset.x, offset.y) + math.min(offset.x, offset.y) * (1f / 1000f);
      return new float2(level, priority * (2f - num1 - num2));
    }

    public static bool CompareSnapPriority(float2 priority, float2 other)
    {
      bool2 bool2 = priority > other;
      return bool2.x | bool2.y & (double) priority.x == (double) other.x;
    }

    public static void AddSnapLine(
      ref ControlPoint bestSnapPosition,
      NativeList<SnapLine> snapLines,
      SnapLine snapLine)
    {
      for (int index = 0; index < snapLines.Length; ++index)
      {
        SnapLine snapLine1 = snapLines[index];
        float2 t1;
        if ((double) math.abs(math.dot(snapLine.m_ControlPoint.m_Direction, snapLine1.m_ControlPoint.m_Direction)) <= 0.99999898672103882 && MathUtils.Intersect(new Line2(snapLine.m_ControlPoint.m_Position.xz, snapLine.m_ControlPoint.m_Position.xz + snapLine.m_ControlPoint.m_Direction), new Line2(snapLine1.m_ControlPoint.m_Position.xz, snapLine1.m_ControlPoint.m_Position.xz + snapLine1.m_ControlPoint.m_Direction), out t1))
        {
          SnapLine snapLine2;
          if ((double) snapLine.m_ControlPoint.m_SnapPriority.x >= (double) snapLine1.m_ControlPoint.m_SnapPriority.x)
          {
            snapLine2 = snapLine;
            snapLine2.m_ControlPoint.m_Position.xz += snapLine.m_ControlPoint.m_Direction * t1.x;
          }
          else
          {
            snapLine2 = snapLine1;
            snapLine2.m_ControlPoint.m_Position.xz += snapLine1.m_ControlPoint.m_Direction * t1.y;
          }
          if ((snapLine2.m_Flags & SnapLineFlags.ExtendedCurve) != (SnapLineFlags) 0)
          {
            float t2;
            double num = (double) NetUtils.ExtendedDistance(snapLine2.m_Curve.xz, snapLine2.m_ControlPoint.m_Position.xz, out t2);
            float distance = MathUtils.Snap(NetUtils.ExtendedLength(snapLine2.m_Curve.xz, t2), 4f);
            snapLine2.m_ControlPoint.m_CurvePosition = NetUtils.ExtendedClampLength(snapLine2.m_Curve.xz, distance);
          }
          float level = math.max(snapLine.m_ControlPoint.m_SnapPriority.x, snapLine1.m_ControlPoint.m_SnapPriority.x);
          snapLine2.m_ControlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 2f, snapLine2.m_ControlPoint.m_HitPosition.xz, snapLine2.m_ControlPoint.m_Position.xz, snapLine2.m_ControlPoint.m_Direction);
          ToolUtils.AddSnapPosition(ref bestSnapPosition, snapLine2.m_ControlPoint);
        }
      }
      snapLines.Add(in snapLine);
    }

    public static void AddSnapPosition(ref ControlPoint bestSnapPosition, ControlPoint snapPosition)
    {
      if (!ToolUtils.CompareSnapPriority(snapPosition.m_SnapPriority, bestSnapPosition.m_SnapPriority))
        return;
      bestSnapPosition = snapPosition;
    }

    public static void DirectionSnap(
      ref float bestDirectionDistance,
      ref float3 resultPos,
      ref float3 resultDir,
      float3 refPos,
      float3 snapOrig,
      float3 snapDir,
      float snapDistance)
    {
      float3 a1 = new float3();
      float3 a2 = new float3();
      a1.xz = snapDir.xz;
      a2.xz = MathUtils.Right(snapDir.xz);
      Line3 line3_1 = new Line3(snapOrig, snapOrig + a1);
      Line3 line3_2 = new Line3(snapOrig, snapOrig + a2);
      float t1;
      float num1 = MathUtils.Distance(line3_1.xz, refPos.xz, out t1);
      float t2;
      float num2 = MathUtils.Distance(line3_2.xz, refPos.xz, out t2);
      if ((double) num1 < (double) bestDirectionDistance)
      {
        bestDirectionDistance = num1;
        if ((double) num1 < (double) snapDistance)
        {
          resultDir = math.select(a1, -a1, (double) t1 < 0.0);
          resultPos.xz = MathUtils.Position(line3_1.xz, t1);
        }
      }
      if ((double) num2 >= (double) bestDirectionDistance)
        return;
      bestDirectionDistance = num2;
      if ((double) num2 >= (double) snapDistance)
        return;
      resultDir = math.select(a2, -a2, (double) t2 < 0.0);
      resultPos.xz = MathUtils.Position(line3_2.xz, t2);
    }

    public static Bounds2 GetBounds(Brush brush)
    {
      quaternion q = quaternion.RotateY(brush.m_Angle);
      float2 xz1 = math.mul(q, new float3(brush.m_Size * 0.5f, 0.0f, 0.0f)).xz;
      float2 xz2 = math.mul(q, new float3(0.0f, 0.0f, brush.m_Size * 0.5f)).xz;
      float2 float2 = math.abs(xz1) + math.abs(xz2);
      return new Bounds2(brush.m_Position.xz - float2, brush.m_Position.xz + float2);
    }

    public static float GetRandomAge(ref Random random, AgeMask ageMask)
    {
      int num = random.NextInt(math.countbits((int) ageMask));
      for (int index = 0; index < 4; ++index)
      {
        AgeMask ageMask1 = (AgeMask) (1 << index);
        if ((ageMask & ageMask1) != (AgeMask) 0 && num-- == 0)
        {
          switch (ageMask1)
          {
            case AgeMask.Sapling:
              return 0.0f;
            case AgeMask.Young:
              return 0.175000012f;
            case AgeMask.Mature:
              return 0.425f;
            case AgeMask.Elderly:
              return 0.775000036f;
            default:
              continue;
          }
        }
      }
      return 0.0f;
    }
  }
}
