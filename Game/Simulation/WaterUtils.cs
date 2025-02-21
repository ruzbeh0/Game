// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class WaterUtils
  {
    public static float3 ToSurfaceSpace(ref WaterSurfaceData data, float3 worldPosition)
    {
      return (worldPosition + data.offset) * data.scale;
    }

    public static Line3.Segment ToSurfaceSpace(ref WaterSurfaceData data, Line3.Segment worldLine)
    {
      return new Line3.Segment(WaterUtils.ToSurfaceSpace(ref data, worldLine.a), WaterUtils.ToSurfaceSpace(ref data, worldLine.b));
    }

    public static float3 ToWorldSpace(ref WaterSurfaceData data, float3 surfacePosition)
    {
      return surfacePosition / data.scale - data.offset;
    }

    public static float2 ToWorldSpace(ref WaterSurfaceData data, float2 surfaceVelocity)
    {
      return surfaceVelocity / data.scale.xz;
    }

    public static float ToWorldSpace(ref WaterSurfaceData data, float surfaceDepth)
    {
      return surfaceDepth / data.scale.y - data.offset.y;
    }

    public static float SampleDepth(ref WaterSurfaceData data, float3 worldPosition)
    {
      float2 xz = WaterUtils.ToSurfaceSpace(ref data, worldPosition).xz;
      int4 x = new int4() { xy = (int2) math.floor(xz) };
      x.zw = x.xy + 1;
      int4 int4_1 = math.clamp(x, (int4) 0, data.resolution.xzxz - 1);
      int4 int4_2 = int4_1.yyww * data.resolution.x + int4_1.xzxz;
      float4 float4;
      float4.x = data.depths[int4_2.x].m_Depth;
      float4.y = data.depths[int4_2.y].m_Depth;
      float4.z = data.depths[int4_2.z].m_Depth;
      float4.w = data.depths[int4_2.w].m_Depth;
      float2 float2_1 = math.saturate(xz - (float2) int4_1.xy);
      float2 float2_2 = math.lerp(float4.xz, float4.yw, float2_1.x);
      return WaterUtils.ToWorldSpace(ref data, math.lerp(float2_2.x, float2_2.y, float2_1.y));
    }

    public static float SamplePolluted(ref WaterSurfaceData data, float3 worldPosition)
    {
      float2 xz = WaterUtils.ToSurfaceSpace(ref data, worldPosition).xz;
      int4 x = new int4() { xy = (int2) math.floor(xz) };
      x.zw = x.xy + 1;
      int4 int4_1 = math.clamp(x, (int4) 0, data.resolution.xzxz - 1);
      int4 int4_2 = int4_1.yyww * data.resolution.x + int4_1.xzxz;
      float4 float4;
      float4.x = data.depths[int4_2.x].m_Polluted;
      float4.y = data.depths[int4_2.y].m_Polluted;
      float4.z = data.depths[int4_2.z].m_Polluted;
      float4.w = data.depths[int4_2.w].m_Polluted;
      float2 float2_1 = math.saturate(xz - (float2) int4_1.xy);
      float2 float2_2 = math.lerp(float4.xz, float4.yw, float2_1.x);
      return WaterUtils.ToWorldSpace(ref data, math.lerp(float2_2.x, float2_2.y, float2_1.y));
    }

    public static float2 SampleVelocity(ref WaterSurfaceData data, float3 worldPosition)
    {
      float2 xz = WaterUtils.ToSurfaceSpace(ref data, worldPosition).xz;
      int4 x1 = new int4() { xy = (int2) math.floor(xz) };
      x1.zw = x1.xy + 1;
      int4 int4_1 = math.clamp(x1, (int4) 0, data.resolution.xzxz - 1);
      int4 int4_2 = int4_1.yyww * data.resolution.x + int4_1.xzxz;
      float4 x2 = new float4();
      float4 y = new float4();
      x2.xy = data.depths[int4_2.x].m_Velocity;
      y.xy = data.depths[int4_2.y].m_Velocity;
      x2.zw = data.depths[int4_2.z].m_Velocity;
      y.zw = data.depths[int4_2.w].m_Velocity;
      float2 float2 = math.saturate(xz - (float2) int4_1.xy);
      float4 float4 = math.lerp(x2, y, float2.x);
      return WaterUtils.ToWorldSpace(ref data, math.lerp(float4.xy, float4.zw, float2.y));
    }

    public static float SampleHeight(
      ref WaterSurfaceData data,
      ref TerrainHeightData terrainData,
      float3 worldPosition)
    {
      return WaterUtils.SampleDepth(ref data, worldPosition) + TerrainUtils.SampleHeight(ref terrainData, worldPosition);
    }

    public static float SampleHeight(
      ref WaterSurfaceData data,
      ref TerrainHeightData terrainData,
      float3 worldPosition,
      out float waterDepth)
    {
      waterDepth = WaterUtils.SampleDepth(ref data, worldPosition);
      return waterDepth + TerrainUtils.SampleHeight(ref terrainData, worldPosition);
    }

    public static void SampleHeight(
      ref WaterSurfaceData data,
      ref TerrainHeightData terrainData,
      float3 worldPosition,
      out float terrainHeight,
      out float waterHeight,
      out float waterDepth)
    {
      terrainHeight = TerrainUtils.SampleHeight(ref terrainData, worldPosition);
      waterDepth = WaterUtils.SampleDepth(ref data, worldPosition);
      waterHeight = terrainHeight + waterDepth;
    }

    public static float GetSurfaceDepth(ref WaterSurfaceData data, int2 surfacePosition)
    {
      return data.depths[surfacePosition.y * data.resolution.x + surfacePosition.x].m_Depth;
    }

    public static float GetWorldDepth(ref WaterSurfaceData data, int2 surfacePosition)
    {
      return WaterUtils.ToWorldSpace(ref data, WaterUtils.GetSurfaceDepth(ref data, surfacePosition));
    }

    public static float3 GetWorldPosition(ref WaterSurfaceData data, int2 surfacePosition)
    {
      return WaterUtils.ToWorldSpace(ref data, new float3()
      {
        y = WaterUtils.GetSurfaceDepth(ref data, surfacePosition),
        xz = (float2) surfacePosition
      });
    }

    public static float GetSampleInterval(ref WaterSurfaceData data)
    {
      return math.cmin(1f / data.scale.xz);
    }

    public static bool Raycast(
      ref WaterSurfaceData waterData,
      ref TerrainHeightData terrainData,
      Line3.Segment worldLine,
      bool outside,
      out float t)
    {
      Line3.Segment heightmapSpace = TerrainUtils.ToHeightmapSpace(ref terrainData, worldLine);
      Bounds3 bounds = new Bounds3(new float3(0.0f, -50f, 0.0f), (float3) (terrainData.resolution - 1) + new float3(0.0f, 100f, 0.0f));
      float2 t1;
      if (outside)
      {
        if (!MathUtils.Intersect(bounds.y, heightmapSpace.y, out t1))
        {
          t = 2f;
          return false;
        }
      }
      else if (!MathUtils.Intersect(bounds, heightmapSpace, out t1))
      {
        t = 2f;
        return false;
      }
      Line3.Segment localLine = MathUtils.Cut(WaterUtils.ToSurfaceSpace(ref waterData, worldLine), t1);
      float2 terrainToWaterSpace = new float2(waterData.scale.y / terrainData.scale.y, -terrainData.offset.y * waterData.scale.y);
      int3 resolution = terrainData.resolution;
      int2 xz1 = resolution.xz;
      resolution = waterData.resolution;
      int2 xz2 = resolution.xz;
      int2 waterToTerrainFactor = xz1 / xz2;
      float3 x = localLine.b - localLine.a;
      float3 float3 = math.abs(x);
      float4 float4 = math.floor(new float4(localLine.a.xz, localLine.b.xz));
      int4 int4 = new int4((int) float4.x, (int) float4.z, (int) float4.y, (int) float4.w);
      if (math.all(int4.xz == int4.yw))
      {
        if (WaterUtils.RaycastCell(ref waterData, ref terrainData, localLine, int4.xz, terrainToWaterSpace, waterToTerrainFactor, outside, out t))
        {
          t = math.saturate(math.lerp(t1.x, t1.y, t));
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
            if (WaterUtils.RaycastCell(ref waterData, ref terrainData, localLine, pos, terrainToWaterSpace, waterToTerrainFactor, outside, out t))
            {
              t = math.saturate(math.lerp(t1.x, t1.y, t));
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
            if (WaterUtils.RaycastCell(ref waterData, ref terrainData, localLine, pos, terrainToWaterSpace, waterToTerrainFactor, outside, out t))
            {
              t = math.saturate(math.lerp(t1.x, t1.y, t));
              return true;
            }
          }
          int4.x = int4.y - int2.x;
        }
      }
      t = 2f;
      return false;
    }

    private static bool RaycastCell(
      ref WaterSurfaceData waterData,
      ref TerrainHeightData terrainData,
      Line3.Segment localLine,
      int2 pos,
      float2 terrainToWaterSpace,
      int2 waterToTerrainFactor,
      bool outside,
      out float t)
    {
      t = 2f;
      int4 a = math.clamp(new int4(pos, pos + 1), (int4) 0, waterData.resolution.xzxz - 1);
      int4 int4_1 = a.yyww * waterData.resolution.x + a.xzxz;
      int4 int4_2 = math.clamp(a * waterToTerrainFactor.xyxy, (int4) 0, terrainData.resolution.xzxz - 1);
      int4 int4_3 = int4_2.yyww * terrainData.resolution.x + int4_2.xzxz;
      float4 float4_1;
      float4_1.x = waterData.depths[int4_1.x].m_Depth;
      float4_1.y = waterData.depths[int4_1.y].m_Depth;
      float4_1.z = waterData.depths[int4_1.z].m_Depth;
      float4_1.w = waterData.depths[int4_1.w].m_Depth;
      float4 float4_2;
      float4_2.x = (float) terrainData.heights[int4_3.x];
      float4_2.y = (float) terrainData.heights[int4_3.y];
      float4_2.z = (float) terrainData.heights[int4_3.z];
      float4_2.w = (float) terrainData.heights[int4_3.w];
      float4 x = float4_2 * terrainToWaterSpace.x + terrainToWaterSpace.y + float4_1;
      Bounds3 bounds = new Bounds3();
      int4 int4_4 = math.select(a, new int4(pos, pos + 1), outside);
      bounds.min = new float3((float) int4_4.x, math.cmin(x), (float) int4_4.y);
      bounds.max = new float3((float) int4_4.z, math.cmax(x), (float) int4_4.w);
      if (MathUtils.Intersect(bounds, localLine, out float2 _))
      {
        float3 float3_1 = new float3(bounds.min.x, x.x, bounds.min.z);
        float3 float3_2 = new float3(bounds.max.x, x.y, bounds.min.z);
        float3 float3_3 = new float3(bounds.max.x, x.w, bounds.max.z);
        float3 float3_4 = new float3(bounds.min.x, x.z, bounds.max.z);
        float3 _c = MathUtils.Center(bounds);
        float3 t1;
        if (MathUtils.Intersect(new Triangle3(float3_1, float3_2, _c), localLine, out t1))
          t = math.min(t, t1.z);
        if (MathUtils.Intersect(new Triangle3(float3_2, float3_3, _c), localLine, out t1))
          t = math.min(t, t1.z);
        if (MathUtils.Intersect(new Triangle3(float3_3, float3_4, _c), localLine, out t1))
          t = math.min(t, t1.z);
        if (MathUtils.Intersect(new Triangle3(float3_4, float3_1, _c), localLine, out t1))
          t = math.min(t, t1.z);
        if ((double) t != 2.0)
          return true;
      }
      return false;
    }
  }
}
