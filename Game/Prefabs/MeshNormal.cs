// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshNormal
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
  public struct MeshNormal : IBufferElementData
  {
    public float3 m_Normal;

    public MeshNormal(float3 normal) => this.m_Normal = normal;

    public static void Unpack(
      NativeSlice<byte> src,
      DynamicBuffer<MeshNormal> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      dst.ResizeUninitialized(count);
      MeshNormal.Unpack(src, dst.AsNativeArray(), count, format, dimension);
    }

    public static unsafe void Unpack(
      NativeSlice<byte> src,
      NativeArray<MeshNormal> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      if (format == VertexAttributeFormat.Float32 && dimension == 3)
        src.SliceConvert<MeshNormal>().CopyTo(dst);
      else if (format == VertexAttributeFormat.Float16)
      {
        NativeMath.ArrayHalfToFloat((IntPtr) src.GetUnsafeReadOnlyPtr<byte>(), (long) count, dimension, (IntPtr) dst.GetUnsafePtr<MeshNormal>(), 3);
      }
      else
      {
        if (format != VertexAttributeFormat.SNorm16 || dimension != 2)
          throw new Exception(string.Format("Unsupported source normals format/dimension in Unpack {0} {1}", (object) format, (object) dimension));
        NativeMath.ArrayOctahedralToNormals((IntPtr) src.GetUnsafeReadOnlyPtr<byte>(), (long) count, (IntPtr) dst.GetUnsafePtr<MeshNormal>());
      }
    }
  }
}
