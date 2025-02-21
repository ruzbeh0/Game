// Decompiled with JetBrains decompiler
// Type: Game.Rendering.FrustumPlanes
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct FrustumPlanes
  {
    public static int GetPacketCount(int cullingPlaneCount) => cullingPlaneCount + 3 >> 2;

    public static void BuildSOAPlanePackets(
      NativeArray<Plane> cullingPlanes,
      int cullingPlaneCount,
      NativeList<FrustumPlanes.PlanePacket4> result)
    {
      int packetCount = FrustumPlanes.GetPacketCount(cullingPlaneCount);
      result.ResizeUninitialized(packetCount);
      for (int index = 0; index < cullingPlaneCount; ++index)
      {
        Plane cullingPlane = cullingPlanes[index];
        FrustumPlanes.PlanePacket4 planePacket4 = result[index >> 2];
        planePacket4.Xs[index & 3] = cullingPlane.normal.x;
        planePacket4.Ys[index & 3] = cullingPlane.normal.y;
        planePacket4.Zs[index & 3] = cullingPlane.normal.z;
        planePacket4.Distances[index & 3] = cullingPlane.distance;
        result[index >> 2] = planePacket4;
      }
      for (int index = cullingPlaneCount; index < 4 * packetCount; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = result[index >> 2];
        planePacket4.Xs[index & 3] = 1f;
        planePacket4.Ys[index & 3] = 0.0f;
        planePacket4.Zs[index & 3] = 0.0f;
        planePacket4.Distances[index & 3] = 1E+09f;
        result[index >> 2] = planePacket4;
      }
    }

    private static float4 dot4(float4 xs, float4 ys, float4 zs, float4 mx, float4 my, float4 mz)
    {
      return xs * mx + ys * my + zs * mz;
    }

    public static unsafe FrustumPlanes.IntersectResult CalculateIntersectResult(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float3 extents)
    {
      float4 xxxx1 = center.xxxx;
      float4 yyyy1 = center.yyyy;
      float4 zzzz1 = center.zzzz;
      float4 xxxx2 = extents.xxxx;
      float4 yyyy2 = extents.yyyy;
      float4 zzzz2 = extents.zzzz;
      int4 x1 = (int4) 0;
      int4 x2 = (int4) 0;
      for (int index = 0; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_1 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_2 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x1 += (int4) (float4_1 + float4_2 < 0.0f);
        x2 += (int4) (float4_1 >= float4_2);
      }
      int num = math.csum(x2);
      if (math.csum(x1) != 0)
        return FrustumPlanes.IntersectResult.Out;
      return num != 4 * length ? FrustumPlanes.IntersectResult.Partial : FrustumPlanes.IntersectResult.In;
    }

    public static unsafe void Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float3 extents,
      out ulong inMask,
      out ulong outMask)
    {
      float4 xxxx1 = center.xxxx;
      float4 yyyy1 = center.yyyy;
      float4 zzzz1 = center.zzzz;
      float4 xxxx2 = extents.xxxx;
      float4 yyyy2 = extents.yyyy;
      float4 zzzz2 = extents.zzzz;
      uint4 x1 = (uint4) 0U;
      uint4 x2 = (uint4) 0U;
      uint4 x3 = (uint4) 0U;
      uint4 x4 = (uint4) 0U;
      uint4 b1 = new uint4(1U, 2U, 4U, 8U);
      uint4 b2 = new uint4(1U, 2U, 4U, 8U);
      int num = math.min(8, length);
      for (int index = 0; index < num; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_1 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_2 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x2 += math.select((uint4) 0U, b1, float4_1 + float4_2 < 0.0f);
        x1 += math.select((uint4) 0U, b1, float4_1 >= float4_2);
        b1 <<= 4;
      }
      for (int index = num; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_3 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_4 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x4 += math.select((uint4) 0U, b2, float4_3 + float4_4 < 0.0f);
        x3 += math.select((uint4) 0U, b2, float4_3 >= float4_4);
        b2 <<= 4;
      }
      inMask = (ulong) math.csum(x1) | (ulong) math.csum(x3) << 32;
      outMask = (ulong) math.csum(x2) | (ulong) math.csum(x4) << 32;
    }

    public static unsafe bool Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float3 extents)
    {
      float4 xxxx1 = center.xxxx;
      float4 yyyy1 = center.yyyy;
      float4 zzzz1 = center.zzzz;
      float4 xxxx2 = extents.xxxx;
      float4 yyyy2 = extents.yyyy;
      float4 zzzz2 = extents.zzzz;
      int4 x = (int4) 0;
      for (int index = 0; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_1 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_2 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x += (int4) (float4_1 + float4_2 < 0.0f);
      }
      return math.csum(x) == 0;
    }

    public static unsafe bool Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float radius)
    {
      float4 xxxx = center.xxxx;
      float4 yyyy = center.yyyy;
      float4 zzzz = center.zzzz;
      float4 float4_1 = new float4(radius);
      int4 x = (int4) 0;
      for (int index = 0; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_2 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx, yyyy, zzzz) + planePacket4.Distances;
        float4 float4_3 = FrustumPlanes.dot4(float4_1, float4_1, float4_1, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x += (int4) (float4_2 + float4_3 < 0.0f);
      }
      return math.csum(x) == 0;
    }

    public static unsafe void Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float3 extents,
      out ulong outMask)
    {
      float4 xxxx1 = center.xxxx;
      float4 yyyy1 = center.yyyy;
      float4 zzzz1 = center.zzzz;
      float4 xxxx2 = extents.xxxx;
      float4 yyyy2 = extents.yyyy;
      float4 zzzz2 = extents.zzzz;
      uint4 x1 = (uint4) 0U;
      uint4 x2 = (uint4) 0U;
      uint4 b1 = new uint4(1U, 2U, 4U, 8U);
      uint4 b2 = new uint4(1U, 2U, 4U, 8U);
      for (int index = 0; index < 8; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_1 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_2 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x1 += math.select((uint4) 0U, b1, float4_1 + float4_2 < 0.0f);
        b1 <<= 4;
      }
      for (int index = 8; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_3 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_4 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x2 += math.select((uint4) 0U, b2, float4_3 + float4_4 < 0.0f);
        b2 <<= 4;
      }
      outMask = (ulong) math.csum(x1) | (ulong) math.csum(x2) << 32;
    }

    public static unsafe void Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float radius,
      out ulong outMask)
    {
      float4 xxxx = center.xxxx;
      float4 yyyy = center.yyyy;
      float4 zzzz = center.zzzz;
      float4 float4_1 = new float4(radius);
      uint4 x1 = (uint4) 0U;
      uint4 x2 = (uint4) 0U;
      uint4 b1 = new uint4(1U, 2U, 4U, 8U);
      uint4 b2 = new uint4(1U, 2U, 4U, 8U);
      for (int index = 0; index < 8; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_2 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx, yyyy, zzzz) + planePacket4.Distances;
        float4 float4_3 = FrustumPlanes.dot4(float4_1, float4_1, float4_1, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x1 += math.select((uint4) 0U, b1, float4_2 + float4_3 < 0.0f);
        b1 <<= 4;
      }
      for (int index = 8; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_4 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx, yyyy, zzzz) + planePacket4.Distances;
        float4 float4_5 = FrustumPlanes.dot4(float4_1, float4_1, float4_1, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x2 += math.select((uint4) 0U, b2, float4_4 + float4_5 < 0.0f);
        b2 <<= 4;
      }
      outMask = (ulong) math.csum(x1) | (ulong) math.csum(x2) << 32;
    }

    public static unsafe void Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float3 extents,
      out uint outMask)
    {
      float4 xxxx1 = center.xxxx;
      float4 yyyy1 = center.yyyy;
      float4 zzzz1 = center.zzzz;
      float4 xxxx2 = extents.xxxx;
      float4 yyyy2 = extents.yyyy;
      float4 zzzz2 = extents.zzzz;
      uint4 x = (uint4) 0U;
      uint4 b = new uint4(1U, 2U, 4U, 8U);
      for (int index = 0; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_1 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx1, yyyy1, zzzz1) + planePacket4.Distances;
        float4 float4_2 = FrustumPlanes.dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x += math.select((uint4) 0U, b, float4_1 + float4_2 < 0.0f);
        b <<= 4;
      }
      outMask = math.csum(x);
    }

    public static unsafe void Intersect(
      FrustumPlanes.PlanePacket4* cullingPlanePackets,
      int length,
      float3 center,
      float radius,
      out uint outMask)
    {
      float4 xxxx = center.xxxx;
      float4 yyyy = center.yyyy;
      float4 zzzz = center.zzzz;
      float4 float4_1 = new float4(radius);
      uint4 x = (uint4) 0U;
      uint4 b = new uint4(1U, 2U, 4U, 8U);
      for (int index = 0; index < length; ++index)
      {
        FrustumPlanes.PlanePacket4 planePacket4 = cullingPlanePackets[index];
        float4 float4_2 = FrustumPlanes.dot4(planePacket4.Xs, planePacket4.Ys, planePacket4.Zs, xxxx, yyyy, zzzz) + planePacket4.Distances;
        float4 float4_3 = FrustumPlanes.dot4(float4_1, float4_1, float4_1, math.abs(planePacket4.Xs), math.abs(planePacket4.Ys), math.abs(planePacket4.Zs));
        x += math.select((uint4) 0U, b, float4_2 + float4_3 < 0.0f);
        b <<= 4;
      }
      outMask = math.csum(x);
    }

    public enum IntersectResult
    {
      Out,
      In,
      Partial,
    }

    public struct PlanePacket4
    {
      public float4 Xs;
      public float4 Ys;
      public float4 Zs;
      public float4 Distances;
    }
  }
}
