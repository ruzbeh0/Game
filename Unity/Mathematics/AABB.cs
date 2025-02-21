// Decompiled with JetBrains decompiler
// Type: Unity.Mathematics.AABB
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Unity.Mathematics
{
  [Serializable]
  public struct AABB
  {
    public float3 Center;
    public float3 Extents;

    public float3 Size => this.Extents * 2f;

    public float3 Min => this.Center - this.Extents;

    public float3 Max => this.Center + this.Extents;

    public override string ToString()
    {
      return string.Format("AABB(Center:{0}, Extents:{1}", (object) this.Center, (object) this.Extents);
    }

    public bool Contains(float3 point)
    {
      return (double) point[0] >= (double) this.Center[0] - (double) this.Extents[0] && (double) point[0] <= (double) this.Center[0] + (double) this.Extents[0] && (double) point[1] >= (double) this.Center[1] - (double) this.Extents[1] && (double) point[1] <= (double) this.Center[1] + (double) this.Extents[1] && (double) point[2] >= (double) this.Center[2] - (double) this.Extents[2] && (double) point[2] <= (double) this.Center[2] + (double) this.Extents[2];
    }

    public bool Contains(AABB b)
    {
      return this.Contains(b.Center + math.float3(-b.Extents.x, -b.Extents.y, -b.Extents.z)) && this.Contains(b.Center + math.float3(-b.Extents.x, -b.Extents.y, b.Extents.z)) && this.Contains(b.Center + math.float3(-b.Extents.x, b.Extents.y, -b.Extents.z)) && this.Contains(b.Center + math.float3(-b.Extents.x, b.Extents.y, b.Extents.z)) && this.Contains(b.Center + math.float3(b.Extents.x, -b.Extents.y, -b.Extents.z)) && this.Contains(b.Center + math.float3(b.Extents.x, -b.Extents.y, b.Extents.z)) && this.Contains(b.Center + math.float3(b.Extents.x, b.Extents.y, -b.Extents.z)) && this.Contains(b.Center + math.float3(b.Extents.x, b.Extents.y, b.Extents.z));
    }

    private static float3 RotateExtents(float3 extents, float3 m0, float3 m1, float3 m2)
    {
      return math.abs(m0 * extents.x) + math.abs(m1 * extents.y) + math.abs(m2 * extents.z);
    }

    public static AABB Transform(float4x4 transform, AABB localBounds)
    {
      AABB aabb;
      aabb.Extents = AABB.RotateExtents(localBounds.Extents, transform.c0.xyz, transform.c1.xyz, transform.c2.xyz);
      aabb.Center = math.transform(transform, localBounds.Center);
      return aabb;
    }

    public float DistanceSq(float3 point)
    {
      return math.lengthsq(math.max(math.abs(point - this.Center), this.Extents) - this.Extents);
    }
  }
}
