// Decompiled with JetBrains decompiler
// Type: Game.Rendering.RenderingUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Prefabs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  public static class RenderingUtils
  {
    public static Matrix4x4 ToMatrix4x4(float4x4 matrix)
    {
      Matrix4x4 matrix4x4 = new Matrix4x4();
      matrix4x4.SetColumn(0, (Vector4) matrix.c0);
      matrix4x4.SetColumn(1, (Vector4) matrix.c1);
      matrix4x4.SetColumn(2, (Vector4) matrix.c2);
      matrix4x4.SetColumn(3, (Vector4) matrix.c3);
      return matrix4x4;
    }

    public static Color ToColor(float4 vector) => new Color(vector.x, vector.y, vector.z, vector.w);

    public static float4 Lerp(float4 c0, float4 c0_5, float4 c1, float t)
    {
      return (double) t <= 0.5 ? math.lerp(c0, c0_5, t * 2f) : math.lerp(c0_5, c1, (float) ((double) t * 2.0 - 1.0));
    }

    public static Bounds ToBounds(Bounds3 bounds)
    {
      return new Bounds((Vector3) MathUtils.Center(bounds), (Vector3) MathUtils.Size(bounds));
    }

    public static float4 CalculateLodParameters(float lodFactor, BatchCullingContext cullingContext)
    {
      return RenderingUtils.CalculateLodParameters(lodFactor, cullingContext.lodParameters);
    }

    public static float4 CalculateLodParameters(float lodFactor, LODParameters lodParameters)
    {
      float w = 1f / math.tan(math.radians(lodParameters.fieldOfView * 0.5f));
      lodFactor *= 540f * w;
      return new float4(lodFactor, (float) (1.0 / ((double) lodFactor * (double) lodFactor)), w + 1f, w);
    }

    public static float CalculateMinDistance(
      Bounds3 bounds,
      float3 cameraPosition,
      float3 cameraDirection,
      float4 lodParameters)
    {
      float3 x1 = bounds.min - cameraPosition;
      float3 float3 = bounds.max - cameraPosition;
      float b = math.length(math.max((float3) 0.0f, math.max(x1, -float3)));
      float3 x2 = x1 * cameraDirection;
      float3 y = float3 * cameraDirection;
      return (float) ((double) b * (double) lodParameters.z - (double) lodParameters.w * (double) math.clamp(math.csum(math.max(x2, y)), 0.0f, b));
    }

    public static float CalculateMaxDistance(
      Bounds3 bounds,
      float3 cameraPosition,
      float3 cameraDirection,
      float4 lodParameters)
    {
      float3 float3 = bounds.min - cameraPosition;
      float3 y1 = bounds.max - cameraPosition;
      float b = math.length(math.max(-float3, y1));
      float3 x = float3 * cameraDirection;
      float3 y2 = y1 * cameraDirection;
      return (float) ((double) b * (double) lodParameters.z - (double) lodParameters.w * (double) math.clamp(math.csum(math.min(x, y2)), 0.0f, b));
    }

    public static int CalculateLodLimit(float metersPerPixel, float bias)
    {
      metersPerPixel *= math.pow(2f, -bias);
      return RenderingUtils.CalculateLodLimit(metersPerPixel);
    }

    public static int CalculateLodLimit(float metersPerPixel)
    {
      float num = metersPerPixel * metersPerPixel;
      return (int) byte.MaxValue - (math.asint(num * num * num) >> 23) & (int) byte.MaxValue;
    }

    public static int CalculateLod(float distanceSq, float4 lodParameters)
    {
      distanceSq *= lodParameters.y;
      return (int) byte.MaxValue - (math.asint(distanceSq * distanceSq * distanceSq) >> 23) & (int) byte.MaxValue;
    }

    public static float CalculateDistanceFactor(int lod)
    {
      return math.pow(2f, (float) (128 - lod) * 0.166666672f);
    }

    public static float CalculateDistance(int lod, float4 lodParameters)
    {
      return RenderingUtils.CalculateDistanceFactor(lod) * lodParameters.x;
    }

    public static Bounds3 SafeBounds(Bounds3 bounds)
    {
      float3 float3 = math.min((float3) 0.0f, MathUtils.Size(bounds) - 0.01f);
      bounds.min += float3;
      bounds.max -= float3;
      return bounds;
    }

    public static float GetRenderingSize(float3 size) => math.csum(size) * 0.333333343f;

    public static float GetRenderingSize(float3 size, float indexCount)
    {
      return math.csum(size) * 0.577350259f * math.rsqrt(indexCount);
    }

    public static float GetRenderingSize(float2 size)
    {
      return RenderingUtils.GetRenderingSize(new float3(size, math.cmax(size * new float2(8f, 4f))));
    }

    public static float GetShadowRenderingSize(float2 size)
    {
      return RenderingUtils.GetRenderingSize(new float3(size, math.cmax(size * new float2(2f, 2f))));
    }

    public static float GetRenderingSize(float2 size, float indexFactor)
    {
      float z = math.cmax(size * new float2(8f, 4f));
      float indexCount = indexFactor * z;
      return RenderingUtils.GetRenderingSize(new float3(size, z), indexCount);
    }

    public static float GetRenderingSize(float3 boundsSize, StackDirection stackDirection)
    {
      switch (stackDirection)
      {
        case StackDirection.Right:
          return RenderingUtils.GetRenderingSize(boundsSize.zy);
        case StackDirection.Up:
          return RenderingUtils.GetRenderingSize(new float2(math.cmax(boundsSize.xz), math.cmin(boundsSize.xz)));
        case StackDirection.Forward:
          return RenderingUtils.GetRenderingSize(boundsSize.xy);
        default:
          return RenderingUtils.GetRenderingSize(boundsSize);
      }
    }

    public static float GetShadowRenderingSize(float3 boundsSize, StackDirection stackDirection)
    {
      switch (stackDirection)
      {
        case StackDirection.Right:
          return RenderingUtils.GetShadowRenderingSize(boundsSize.zy);
        case StackDirection.Up:
          return RenderingUtils.GetShadowRenderingSize(new float2(math.cmax(boundsSize.xz), math.cmin(boundsSize.xz)));
        case StackDirection.Forward:
          return RenderingUtils.GetShadowRenderingSize(boundsSize.xy);
        default:
          return RenderingUtils.GetRenderingSize(boundsSize);
      }
    }

    public static float GetRenderingSize(
      float3 boundsSize,
      float3 meshSize,
      float indexCount,
      StackDirection stackDirection)
    {
      switch (stackDirection)
      {
        case StackDirection.Right:
          float indexFactor1 = indexCount / math.max(1f, meshSize.x);
          return RenderingUtils.GetRenderingSize(boundsSize.zy, indexFactor1);
        case StackDirection.Up:
          float indexFactor2 = indexCount / math.max(1f, meshSize.y);
          return RenderingUtils.GetRenderingSize(new float2(math.cmax(boundsSize.xz), math.cmin(boundsSize.xz)), indexFactor2);
        case StackDirection.Forward:
          float indexFactor3 = indexCount / math.max(1f, meshSize.z);
          return RenderingUtils.GetRenderingSize(boundsSize.xy, indexFactor3);
        default:
          return RenderingUtils.GetRenderingSize(boundsSize, indexCount);
      }
    }

    public static int2 FindBoneIndex(
      Entity prefab,
      ref float3 position,
      ref quaternion rotation,
      int boneID,
      ref BufferLookup<SubMesh> subMeshBuffers,
      ref BufferLookup<ProceduralBone> proceduralBoneBuffers)
    {
      DynamicBuffer<SubMesh> bufferData1;
      if (boneID > 0 && subMeshBuffers.TryGetBuffer(prefab, out bufferData1) && boneID >= bufferData1.Length)
      {
        int num = 0;
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          SubMesh subMesh = bufferData1[index1];
          DynamicBuffer<ProceduralBone> bufferData2;
          if (proceduralBoneBuffers.TryGetBuffer(subMesh.m_SubMesh, out bufferData2))
          {
            for (int index2 = 0; index2 < bufferData2.Length; ++index2)
            {
              ProceduralBone proceduralBone = bufferData2[index2];
              if (proceduralBone.m_ConnectionID == boneID)
              {
                if ((subMesh.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
                {
                  proceduralBone.m_ObjectPosition = subMesh.m_Position + math.rotate(subMesh.m_Rotation, proceduralBone.m_ObjectPosition);
                  proceduralBone.m_ObjectRotation = math.mul(subMesh.m_Rotation, proceduralBone.m_ObjectRotation);
                }
                float4x4 a = math.inverse(math.mul(float4x4.TRS(proceduralBone.m_ObjectPosition, proceduralBone.m_ObjectRotation, (float3) 1f), proceduralBone.m_BindPose));
                position = math.transform(a, position);
                float3 forward = math.rotate(a, math.forward(rotation));
                float3 up = math.rotate(a, math.mul(rotation, math.up()));
                rotation = quaternion.LookRotation(forward, up);
                int2 boneIndex;
                boneIndex.x = num + index2;
                boneIndex.y = math.select(-1, index1, (subMesh.m_Flags & SubMeshFlags.HasTransform) > (SubMeshFlags) 0);
                return boneIndex;
              }
            }
            num += bufferData2.Length;
          }
          ++boneID;
        }
      }
      return (int2) -1;
    }

    public static BlendWeight GetBlendWeight(CharacterGroup.IndexWeight indexWeight)
    {
      return new BlendWeight()
      {
        m_Index = indexWeight.index,
        m_Weight = indexWeight.weight
      };
    }

    public static BlendWeights GetBlendWeights(CharacterGroup.IndexWeight8 indexWeight8)
    {
      return new BlendWeights()
      {
        m_Weight0 = RenderingUtils.GetBlendWeight(indexWeight8.w0),
        m_Weight1 = RenderingUtils.GetBlendWeight(indexWeight8.w1),
        m_Weight2 = RenderingUtils.GetBlendWeight(indexWeight8.w2),
        m_Weight3 = RenderingUtils.GetBlendWeight(indexWeight8.w3),
        m_Weight4 = RenderingUtils.GetBlendWeight(indexWeight8.w4),
        m_Weight5 = RenderingUtils.GetBlendWeight(indexWeight8.w5),
        m_Weight6 = RenderingUtils.GetBlendWeight(indexWeight8.w6),
        m_Weight7 = RenderingUtils.GetBlendWeight(indexWeight8.w7)
      };
    }
  }
}
