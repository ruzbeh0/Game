// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MeshVertex
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
  public struct MeshVertex : IBufferElementData
  {
    public float3 m_Vertex;

    public MeshVertex(float3 vertex) => this.m_Vertex = vertex;

    public static void Unpack(
      NativeSlice<byte> src,
      DynamicBuffer<MeshVertex> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      dst.ResizeUninitialized(count);
      MeshVertex.Unpack(src, dst.AsNativeArray(), count, format, dimension);
    }

    public static unsafe void Unpack(
      NativeSlice<byte> src,
      NativeArray<MeshVertex> dst,
      int count,
      VertexAttributeFormat format,
      int dimension)
    {
      if (format == VertexAttributeFormat.Float32 && dimension == 3)
      {
        src.SliceConvert<MeshVertex>().CopyTo(dst);
      }
      else
      {
        if (format != VertexAttributeFormat.Float16)
          throw new Exception(string.Format("Unsupported source position format/dimension in Unpack {0} {1}", (object) format, (object) dimension));
        NativeMath.ArrayHalfToFloat((IntPtr) src.GetUnsafeReadOnlyPtr<byte>(), (long) count, dimension, (IntPtr) dst.GetUnsafePtr<MeshVertex>(), 3);
      }
    }
  }
}
