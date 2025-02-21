// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshUV0
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
  public struct MeshUV0 : IBufferElementData
  {
    public float2 m_Uv;

    public MeshUV0(float2 uv) => this.m_Uv = uv;

    public static void Unpack(
      NativeSlice<byte> src,
      DynamicBuffer<MeshUV0> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      dst.ResizeUninitialized(count);
      MeshUV0.Unpack(src, dst.AsNativeArray(), count, format, dimension);
    }

    public static unsafe void Unpack(
      NativeSlice<byte> src,
      NativeArray<MeshUV0> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      if (format == VertexAttributeFormat.Float32 && dimension == 2)
      {
        src.SliceConvert<MeshUV0>().CopyTo(dst);
      }
      else
      {
        if (format != VertexAttributeFormat.Float16)
          throw new Exception(string.Format("Unsupported source UV0 format/dimension in Unpack {0} {1}", (object) format, (object) dimension));
        NativeMath.ArrayHalfToFloat((IntPtr) src.GetUnsafeReadOnlyPtr<byte>(), (long) count, dimension, (IntPtr) dst.GetUnsafePtr<MeshUV0>(), 2);
      }
    }
  }
}
