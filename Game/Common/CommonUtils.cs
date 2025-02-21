// Decompiled with JetBrains decompiler
// Type: Game.Common.CommonUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Common
{
  public static class CommonUtils
  {
    public static Entity GetRandomEntity(
      ref Random random,
      NativeArray<ArchetypeChunk> chunks,
      EntityTypeHandle entityType)
    {
      int max = 0;
      for (int index = 0; index < chunks.Length; ++index)
      {
        ArchetypeChunk chunk = chunks[index];
        max += chunk.Count;
      }
      if (max == 0)
        return Entity.Null;
      int index1 = random.NextInt(max);
      for (int index2 = 0; index2 < chunks.Length; ++index2)
      {
        ArchetypeChunk chunk = chunks[index2];
        if (index1 < chunk.Count)
          return chunk.GetNativeArray(entityType)[index1];
        index1 -= chunk.Count;
      }
      return Entity.Null;
    }

    public static Entity GetRandomEntity<T>(
      ref Random random,
      NativeArray<ArchetypeChunk> chunks,
      EntityTypeHandle entityType,
      ComponentTypeHandle<T> componentType,
      out T componentData)
      where T : unmanaged, IComponentData
    {
      componentData = default (T);
      int max = 0;
      for (int index = 0; index < chunks.Length; ++index)
      {
        ArchetypeChunk chunk = chunks[index];
        max += chunk.Count;
      }
      if (max == 0)
        return Entity.Null;
      int index1 = random.NextInt(max);
      for (int index2 = 0; index2 < chunks.Length; ++index2)
      {
        ArchetypeChunk chunk = chunks[index2];
        if (index1 < chunk.Count)
        {
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(entityType);
          NativeArray<T> nativeArray2 = chunk.GetNativeArray<T>(ref componentType);
          componentData = nativeArray2[index1];
          return nativeArray1[index1];
        }
        index1 -= chunk.Count;
      }
      return Entity.Null;
    }

    public static Entity GetRandomEntity<T1, T2>(
      ref Random random,
      NativeArray<ArchetypeChunk> chunks,
      EntityTypeHandle entityType,
      ComponentTypeHandle<T1> componentType1,
      ComponentTypeHandle<T2> componentType2,
      out T1 componentData1,
      out T2 componentData2)
      where T1 : unmanaged, IComponentData
      where T2 : unmanaged, IComponentData
    {
      componentData1 = default (T1);
      componentData2 = default (T2);
      int max = 0;
      for (int index = 0; index < chunks.Length; ++index)
      {
        ArchetypeChunk chunk = chunks[index];
        max += chunk.Count;
      }
      if (max == 0)
        return Entity.Null;
      int index1 = random.NextInt(max);
      for (int index2 = 0; index2 < chunks.Length; ++index2)
      {
        ArchetypeChunk chunk = chunks[index2];
        if (index1 < chunk.Count)
        {
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(entityType);
          NativeArray<T1> nativeArray2 = chunk.GetNativeArray<T1>(ref componentType1);
          NativeArray<T2> nativeArray3 = chunk.GetNativeArray<T2>(ref componentType2);
          componentData1 = nativeArray2[index1];
          componentData2 = nativeArray3[index1];
          return nativeArray1[index1];
        }
        index1 -= chunk.Count;
      }
      return Entity.Null;
    }

    public static void Swap<T>(ref T a, ref T b)
    {
      T obj = a;
      a = b;
      b = obj;
    }

    public static void SwapBits(ref uint bitMask, uint a, uint b)
    {
      uint2 uint2 = math.select((uint2) 0U, new uint2(b, a), (bitMask & new uint2(a, b)) != 0U);
      bitMask = bitMask & (uint) ~((int) a | (int) b) | uint2.x | uint2.y;
    }

    public static BoundsMask GetBoundsMask(MeshLayer meshLayers)
    {
      BoundsMask boundsMask = (BoundsMask) 0;
      if ((meshLayers & (MeshLayer.Default | MeshLayer.Moving | MeshLayer.Tunnel | MeshLayer.Marker)) != (MeshLayer) 0)
        boundsMask |= BoundsMask.NormalLayers;
      if ((meshLayers & MeshLayer.Pipeline) != (MeshLayer) 0)
        boundsMask |= BoundsMask.PipelineLayer;
      if ((meshLayers & MeshLayer.SubPipeline) != (MeshLayer) 0)
        boundsMask |= BoundsMask.SubPipelineLayer;
      if ((meshLayers & MeshLayer.Waterway) != (MeshLayer) 0)
        boundsMask |= BoundsMask.WaterwayLayer;
      return boundsMask;
    }

    public static bool ExclusiveGroundCollision(CollisionMask mask1, CollisionMask mask2)
    {
      return (mask1 & mask2 & CollisionMask.OnGround) != (CollisionMask) 0 && ((mask1 | mask2) & CollisionMask.ExclusiveGround) != 0;
    }
  }
}
