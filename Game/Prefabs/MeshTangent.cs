// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshTangent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Native;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct MeshTangent : IBufferElementData
  {
    public float4 m_Tangent;

    public MeshTangent(float4 tangent) => this.m_Tangent = tangent;

    public static void Unpack(
      NativeSlice<byte> src,
      DynamicBuffer<MeshTangent> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      dst.ResizeUninitialized(count);
      MeshTangent.Unpack(src, dst.AsNativeArray(), count, format, dimension);
    }

    public static unsafe void Unpack(
      NativeSlice<byte> src,
      NativeArray<MeshTangent> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      if (format == VertexAttributeFormat.Float32 && dimension == 4)
        src.SliceConvert<MeshTangent>().CopyTo(dst);
      else if (format == VertexAttributeFormat.Float16)
      {
        NativeMath.ArrayHalfToFloat((IntPtr) src.GetUnsafeReadOnlyPtr<byte>(), (long) count, dimension, (IntPtr) dst.GetUnsafePtr<MeshTangent>(), 4);
      }
      else
      {
        if (format != VertexAttributeFormat.Float32 || dimension != 1)
          throw new Exception(string.Format("Unsupported source tangents format/dimension in Unpack {0} {1}", (object) format, (object) dimension));
        NativeMath.ArrayOctahedralToTangents((IntPtr) src.GetUnsafeReadOnlyPtr<byte>(), (long) count, (IntPtr) dst.GetUnsafePtr<MeshTangent>());
      }
    }
  }
}
