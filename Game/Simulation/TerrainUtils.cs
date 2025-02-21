// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TerrainUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public static class TerrainUtils
  {
    public static float3 ToHeightmapSpace(ref TerrainHeightData data, float3 worldPosition)
    {
      return (worldPosition + data.offset) * data.scale;
    }

    public static Line3.Segment ToHeightmapSpace(
      ref TerrainHeightData data,
      Line3.Segment worldLine)
    {
      return new Line3.Segment(TerrainUtils.ToHeightmapSpace(ref data, worldLine.a), TerrainUtils.ToHeightmapSpace(ref data, worldLine.b));
    }

    public static float ToWorldSpace(ref TerrainHeightData data, float heightmapHeight)
    {
      return heightmapHeight / data.scale.y - data.offset.y;
    }

    public static Bounds3 GetBounds(ref TerrainHeightData data)
    {
      return new Bounds3(-data.offset, (float3) (data.resolution - 1) / data.scale - data.offset);
    }

    public static Bounds3 GetEditorCameraBounds(
      TerrainSystem terrainSystem,
      ref TerrainHeightData data)
    {
      return (Object) terrainSystem?.worldHeightmap != (Object) null ? new Bounds3(new float3(terrainSystem.worldOffset.x, terrainSystem.heightScaleOffset.y, terrainSystem.worldOffset.y), new float3(terrainSystem.worldOffset.x + terrainSystem.worldSize.x, terrainSystem.heightScaleOffset.y + terrainSystem.heightScaleOffset.x, terrainSystem.worldOffset.y + terrainSystem.worldSize.y)) : TerrainUtils.GetBounds(ref data);
    }

    public static Bounds1 GetHeightRange(ref TerrainHeightData data, Bounds3 worldBounds)
    {
      float2 xz1 = TerrainUtils.ToHeightmapSpace(ref data, worldBounds.min).xz;
      float2 xz2 = TerrainUtils.ToHeightmapSpace(ref data, worldBounds.max).xz;
      int4 int4 = math.clamp(new int4()
      {
        xy = (int2) math.floor(xz1),
        zw = (int2) math.ceil(xz2)
      }, (int4) 0, data.resolution.xzxz - 1);
      Bounds1 heightRange = new Bounds1(float.MaxValue, float.MinValue);
      for (int y = int4.y; y <= int4.w; ++y)
      {
        int2 int2 = y * data.resolution.x + int4.xz;
        for (int x = int2.x; x <= int2.y; ++x)
          heightRange |= (float) data.heights[x];
      }
      heightRange.min = TerrainUtils.ToWorldSpace(ref data, heightRange.min);
      heightRange.max = TerrainUtils.ToWorldSpace(ref data, heightRange.max);
      return heightRange;
    }

    public static float SampleHeight(ref TerrainHeightData data, float3 worldPosition)
    {
      float2 xz = TerrainUtils.ToHeightmapSpace(ref data, worldPosition).xz;
      int4 x = new int4() { xy = (int2) math.floor(xz) };
      x.zw = x.xy + 1;
      int4 int4_1 = math.clamp(x, (int4) 0, data.resolution.xzxz - 1);
      int4 int4_2 = int4_1.yyww * data.resolution.x + int4_1.xzxz;
      float4 float4;
      float4.x = (float) data.heights[int4_2.x];
      float4.y = (float) data.heights[int4_2.y];
      float4.z = (float) data.heights[int4_2.z];
      float4.w = (float) data.heights[int4_2.w];
      float2 float2_1 = math.saturate(xz - (float2) int4_1.xy);
      float2 float2_2 = math.lerp(float4.xz, float4.yw, float2_1.x);
      return TerrainUtils.ToWorldSpace(ref data, math.lerp(float2_2.x, float2_2.y, float2_1.y));
    }

    public static float SampleHeight(
      ref TerrainHeightData data,
      float3 worldPosition,
      out float3 normal)
    {
      float2 xz = TerrainUtils.ToHeightmapSpace(ref data, worldPosition).xz;
      int4 x = new int4() { xy = (int2) math.floor(xz) };
      x.zw = x.xy + 1;
      int4 int4_1 = math.clamp(x, (int4) 0, data.resolution.xzxz - 1);
      int4 int4_2 = int4_1.yyww * data.resolution.x + int4_1.xzxz;
      float4 float4;
      float4.x = (float) data.heights[int4_2.x];
      float4.y = (float) data.heights[int4_2.y];
      float4.z = (float) data.heights[int4_2.z];
      float4.w = (float) data.heights[int4_2.w];
      float2 float2_1 = math.saturate(xz - (float2) int4_1.xy);
      float2 float2_2 = math.lerp(float4.xz, float4.yw, float2_1.x);
      float2 float2_3 = float4.xz - float4.yw;
      normal = math.normalizesafe(new float3(math.lerp(float2_3.x, float2_3.y, float2_1.y), 1f, float2_2.x - float2_2.y));
      return TerrainUtils.ToWorldSpace(ref data, math.lerp(float2_2.x, float2_2.y, float2_1.y));
    }

    public static bool Raycast(
      ref TerrainHeightData data,
      Line3.Segment worldLine,
      bool outside,
      out float t,
      out float3 normal)
    {
      Line3.Segment heightmapSpace = TerrainUtils.ToHeightmapSpace(ref data, worldLine);
      Bounds3 bounds = new Bounds3(new float3(0.0f, -50f, 0.0f), (float3) (data.resolution - 1) + new float3(0.0f, 100f, 0.0f));
      float2 t1;
      if (outside)
      {
        if (!MathUtils.Intersect(bounds.y, heightmapSpace.y, out t1))
        {
          t = 2f;
          normal = new float3();
          return false;
        }
      }
      else if (!MathUtils.Intersect(bounds, heightmapSpace, out t1))
      {
        t = 2f;
        normal = new float3();
        return false;
      }
      Line3.Segment localLine = MathUtils.Cut(heightmapSpace, t1);
      float3 x = localLine.b - localLine.a;
      float3 float3 = math.abs(x);
      float4 float4 = math.floor(new float4(localLine.a.xz, localLine.b.xz));
      int4 int4 = new int4((int) float4.x, (int) float4.z, (int) float4.y, (int) float4.w);
      if (math.all(int4.xz == int4.yw))
      {
        if (TerrainUtils.RaycastCell(ref data, localLine, int4.xz, outside, out t, out normal))
        {
          t = math.saturate(math.lerp(t1.x, t1.y, t));
          normal = math.normalizesafe(normal);
          return true;
        }
      }
      else if ((double) float3.x > (double) float3.z)
      {
        int2 int2 = math.select((int2) 1, (int2) -1, x.xz < 0.0f);
        int4.y += int2.x;
        float num1 = (float) math.select(1, 0, (double) x.x < 0.0) - localLine.a.x;
        float num2 = 1f / x.x;
        int2 pos;
        for (pos.x = int4.x; pos.x != int4.y; pos.x += int2.x)
        {
          float s = ((float) pos.x + num1) * num2;
          int4.w = (int) math.floor(math.lerp(localLine.a.z, localLine.b.z, s)) + int2.y;
          for (pos.y = int4.z; pos.y != int4.w; pos.y += int2.y)
          {
            if (TerrainUtils.RaycastCell(ref data, localLine, pos, outside, out t, out normal))
            {
              t = math.saturate(math.lerp(t1.x, t1.y, t));
              normal = math.normalizesafe(normal);
              return true;
            }
          }
          int4.z = int4.w - int2.y;
        }
      }
      else
      {
        int2 int2 = math.select((int2) 1, (int2) -1, x.xz < 0.0f);
        int4.w += int2.y;
        float num3 = (float) math.select(1, 0, (double) x.z < 0.0) - localLine.a.z;
        float num4 = 1f / x.z;
        int2 pos;
        for (pos.y = int4.z; pos.y != int4.w; pos.y += int2.y)
        {
          float s = ((float) pos.y + num3) * num4;
          int4.y = (int) math.floor(math.lerp(localLine.a.x, localLine.b.x, s)) + int2.x;
          for (pos.x = int4.x; pos.x != int4.y; pos.x += int2.x)
          {
            if (TerrainUtils.RaycastCell(ref data, localLine, pos, outside, out t, out normal))
            {
              t = math.saturate(math.lerp(t1.x, t1.y, t));
              normal = math.normalizesafe(normal);
              return true;
            }
          }
          int4.x = int4.y - int2.x;
        }
      }
      t = 2f;
      normal = new float3();
      return false;
    }

    private static bool RaycastCell(
      ref TerrainHeightData data,
      Line3.Segment localLine,
      int2 pos,
      bool outside,
      out float t,
      out float3 normal)
    {
      t = 2f;
      normal = new float3();
      int4 a = math.clamp(new int4(pos, pos + 1), (int4) 0, data.resolution.xzxz - 1);
      int4 int4_1 = a.yyww * data.resolution.x + a.xzxz;
      float4 x;
      x.x = (float) data.heights[int4_1.x];
      x.y = (float) data.heights[int4_1.y];
      x.z = (float) data.heights[int4_1.z];
      x.w = (float) data.heights[int4_1.w];
      Bounds3 bounds = new Bounds3();
      int4 int4_2 = math.select(a, new int4(pos, pos + 1), outside);
      bounds.min = new float3((float) int4_2.x, math.cmin(x), (float) int4_2.y);
      bounds.max = new float3((float) int4_2.z, math.cmax(x), (float) int4_2.w);
      if (!MathUtils.Intersect(bounds, localLine, out float2 _))
        return false;
      float3 float3_1 = new float3(bounds.min.x, x.x, bounds.min.z);
      float3 float3_2 = new float3(bounds.max.x, x.y, bounds.min.z);
      float3 float3_3 = new float3(bounds.max.x, x.w, bounds.max.z);
      float3 float3_4 = new float3(bounds.min.x, x.z, bounds.max.z);
      float3 _c = MathUtils.Center(bounds);
      float3 t1;
      if (MathUtils.Intersect(new Triangle3(float3_1, float3_2, _c), localLine, out t1))
      {
        t = math.min(t, t1.z);
        normal = math.cross(_c - float3_1, float3_2 - float3_1);
      }
      if (MathUtils.Intersect(new Triangle3(float3_2, float3_3, _c), localLine, out t1))
      {
        t = math.min(t, t1.z);
        normal = math.cross(_c - float3_2, float3_3 - float3_2);
      }
      if (MathUtils.Intersect(new Triangle3(float3_3, float3_4, _c), localLine, out t1))
      {
        t = math.min(t, t1.z);
        normal = math.cross(_c - float3_3, float3_4 - float3_3);
      }
      if (MathUtils.Intersect(new Triangle3(float3_4, float3_1, _c), localLine, out t1))
      {
        t = math.min(t, t1.z);
        normal = math.cross(_c - float3_4, float3_1 - float3_4);
      }
      return (double) t != 2.0;
    }
  }
}
