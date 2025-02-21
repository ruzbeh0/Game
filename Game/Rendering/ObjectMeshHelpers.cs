// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ObjectMeshHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Mathematics;
using Game.Prefabs;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  public static class ObjectMeshHelpers
  {
    public static Mesh CreateDefaultMesh()
    {
      int length1 = 24;
      int length2 = 36;
      Vector3[] vertices = new Vector3[length1];
      Vector3[] normals = new Vector3[length1];
      Vector4[] tangents = new Vector4[length1];
      Vector2[] uv = new Vector2[length1];
      int[] indices = new int[length2];
      int vertexIndex = 0;
      int indexIndex = 0;
      ObjectMeshHelpers.AddFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(-1f, 0.0f, 0.0f), new float3(0.0f, 1f, 0.0f));
      ObjectMeshHelpers.AddFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(0.0f, -1f, 0.0f), new float3(0.0f, 0.0f, 1f));
      ObjectMeshHelpers.AddFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(0.0f, 0.0f, -1f), new float3(1f, 0.0f, 0.0f));
      ObjectMeshHelpers.AddFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(1f, 0.0f, 0.0f), new float3(0.0f, -1f, 0.0f));
      ObjectMeshHelpers.AddFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(0.0f, 1f, 0.0f), new float3(0.0f, 0.0f, -1f));
      ObjectMeshHelpers.AddFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(0.0f, 0.0f, 1f), new float3(-1f, 0.0f, 0.0f));
      Mesh defaultMesh = new Mesh();
      defaultMesh.name = "Default object";
      defaultMesh.vertices = vertices;
      defaultMesh.normals = normals;
      defaultMesh.tangents = tangents;
      defaultMesh.uv = uv;
      defaultMesh.triangles = indices;
      return defaultMesh;
    }

    public static Mesh CreateDefaultBaseMesh()
    {
      int length1 = 16;
      int length2 = 24;
      Vector3[] vertices = new Vector3[length1];
      Vector3[] normals = new Vector3[length1];
      Vector4[] tangents = new Vector4[length1];
      Vector2[] uv = new Vector2[length1];
      int[] indices = new int[length2];
      int vertexIndex = 0;
      int indexIndex = 0;
      ObjectMeshHelpers.AddBaseFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(-1f, 0.0f, 0.0f), new float3(0.0f, 0.0f, -1f));
      ObjectMeshHelpers.AddBaseFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(0.0f, 0.0f, -1f), new float3(1f, 0.0f, 0.0f));
      ObjectMeshHelpers.AddBaseFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(1f, 0.0f, 0.0f), new float3(0.0f, 0.0f, 1f));
      ObjectMeshHelpers.AddBaseFace(vertices, normals, tangents, uv, indices, ref vertexIndex, ref indexIndex, new float3(0.0f, 0.0f, 1f), new float3(-1f, 0.0f, 0.0f));
      Mesh defaultBaseMesh = new Mesh();
      defaultBaseMesh.name = "Default base";
      defaultBaseMesh.vertices = vertices;
      defaultBaseMesh.normals = normals;
      defaultBaseMesh.tangents = tangents;
      defaultBaseMesh.uv = uv;
      defaultBaseMesh.triangles = indices;
      return defaultBaseMesh;
    }

    public static JobHandle CacheMeshData(
      RenderPrefab meshPrefab,
      GeometryAsset meshData,
      Entity entity,
      int boneCount,
      bool cacheNormals,
      EntityCommandBuffer commandBuffer)
    {
      if (boneCount != 0)
        return new ObjectMeshHelpers.CacheProceduralMeshDataJob()
        {
          m_Data = meshData.data,
          m_Entity = entity,
          m_BoneCount = boneCount,
          m_CacheNormals = cacheNormals,
          m_CommandBuffer = commandBuffer
        }.Schedule<ObjectMeshHelpers.CacheProceduralMeshDataJob>();
      return new ObjectMeshHelpers.CacheMeshDataJob()
      {
        m_Data = meshData.data,
        m_Entity = entity,
        m_MeshBounds = meshPrefab.bounds,
        m_CacheNormals = cacheNormals,
        m_CommandBuffer = commandBuffer
      }.Schedule<ObjectMeshHelpers.CacheMeshDataJob>();
    }

    public static void CacheMeshData(
      Mesh mesh,
      Entity entity,
      bool cacheNormals,
      EntityCommandBuffer commandBuffer)
    {
      DynamicBuffer<MeshVertex> dynamicBuffer1 = commandBuffer.AddBuffer<MeshVertex>(entity);
      DynamicBuffer<MeshIndex> dynamicBuffer2 = commandBuffer.AddBuffer<MeshIndex>(entity);
      DynamicBuffer<MeshNormal> dynamicBuffer3 = new DynamicBuffer<MeshNormal>();
      if (cacheNormals)
        dynamicBuffer3 = commandBuffer.AddBuffer<MeshNormal>(entity);
      Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
      Mesh.MeshData meshData = meshDataArray[0];
      int length = 0;
      int subMeshCount = meshData.subMeshCount;
      for (int index = 0; index < subMeshCount; ++index)
        length += meshData.GetSubMesh(index).indexCount;
      dynamicBuffer1.ResizeUninitialized(meshData.vertexCount);
      dynamicBuffer2.ResizeUninitialized(length);
      meshData.GetVertices(dynamicBuffer1.AsNativeArray().Reinterpret<Vector3>());
      if (cacheNormals)
      {
        dynamicBuffer3.ResizeUninitialized(meshData.vertexCount);
        meshData.GetNormals(dynamicBuffer3.AsNativeArray().Reinterpret<Vector3>());
      }
      int start = 0;
      for (int index = 0; index < subMeshCount; ++index)
      {
        int indexCount = meshData.GetSubMesh(index).indexCount;
        ref Mesh.MeshData local = ref meshData;
        NativeArray<MeshIndex> nativeArray = dynamicBuffer2.AsNativeArray();
        nativeArray = nativeArray.GetSubArray(start, indexCount);
        NativeArray<int> outIndices = nativeArray.Reinterpret<int>();
        int submesh = index;
        local.GetIndices(outIndices, submesh);
        start += indexCount;
      }
      meshDataArray.Dispose();
    }

    public static void UncacheMeshData(Entity entity, EntityCommandBuffer commandBuffer)
    {
      commandBuffer.RemoveComponent<MeshVertex>(entity);
      commandBuffer.RemoveComponent<MeshNormal>(entity);
      commandBuffer.RemoveComponent<MeshIndex>(entity);
      commandBuffer.RemoveComponent<MeshNode>(entity);
    }

    private static void AddFace(
      Vector3[] vertices,
      Vector3[] normals,
      Vector4[] tangents,
      Vector2[] uv,
      int[] indices,
      ref int vertexIndex,
      ref int indexIndex,
      float3 normal,
      float3 tangent)
    {
      float3 float3 = math.cross(normal, tangent);
      vertices[vertexIndex] = (Vector3) (normal + tangent + float3);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(1f, 0.0f);
      ++vertexIndex;
      vertices[vertexIndex] = (Vector3) (normal - tangent + float3);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(0.0f, 0.0f);
      ++vertexIndex;
      vertices[vertexIndex] = (Vector3) (normal - tangent - float3);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(0.0f, 1f);
      ++vertexIndex;
      vertices[vertexIndex] = (Vector3) (normal + tangent - float3);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(1f, 1f);
      ++vertexIndex;
      indices[indexIndex++] = vertexIndex - 4;
      indices[indexIndex++] = vertexIndex - 3;
      indices[indexIndex++] = vertexIndex - 2;
      indices[indexIndex++] = vertexIndex - 2;
      indices[indexIndex++] = vertexIndex - 1;
      indices[indexIndex++] = vertexIndex - 4;
    }

    private static void AddBaseFace(
      Vector3[] vertices,
      Vector3[] normals,
      Vector4[] tangents,
      Vector2[] uv,
      int[] indices,
      ref int vertexIndex,
      ref int indexIndex,
      float3 normal,
      float3 tangent)
    {
      float3 float3_1 = math.cross(normal, tangent) * 0.5f;
      float3 float3_2 = new float3(0.0f, -1.5f, 0.0f);
      vertices[vertexIndex] = (Vector3) (normal + tangent + float3_1 + float3_2);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(1f, 0.0f);
      ++vertexIndex;
      vertices[vertexIndex] = (Vector3) (normal - tangent + float3_1 + float3_2);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(0.0f, 0.0f);
      ++vertexIndex;
      vertices[vertexIndex] = (Vector3) (normal - tangent - float3_1 + float3_2);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(0.0f, 1f);
      ++vertexIndex;
      vertices[vertexIndex] = (Vector3) (normal + tangent - float3_1 + float3_2);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uv[vertexIndex] = new Vector2(1f, 1f);
      ++vertexIndex;
      indices[indexIndex++] = vertexIndex - 4;
      indices[indexIndex++] = vertexIndex - 3;
      indices[indexIndex++] = vertexIndex - 2;
      indices[indexIndex++] = vertexIndex - 2;
      indices[indexIndex++] = vertexIndex - 1;
      indices[indexIndex++] = vertexIndex - 4;
    }

    private static void InitializeBones(
      NativeArray<ObjectMeshHelpers.BoneData> bones,
      int indexCount)
    {
      int length = bones.Length;
      int x = indexCount / 3;
      for (int index = 0; index < length; ++index)
        bones[index] = new ObjectMeshHelpers.BoneData()
        {
          m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue),
          m_TriangleRange = new int2(x, -1)
        };
    }

    private static unsafe void FillBoneData(
      NativeArray<ObjectMeshHelpers.BoneData> bones,
      NativeArray<int> boneIndex,
      NativeArray<MeshVertex> vertices,
      int vertexOffset,
      NativeSlice<byte> boneIdsData,
      int boneIdsDim,
      VertexAttributeFormat boneIdsFormat,
      NativeSlice<byte> weightsData,
      int weightsDim,
      VertexAttributeFormat weightsFormat,
      NativeArray<byte> indexData,
      IndexFormat indexFormat,
      int indexOffset)
    {
      int* unsafeReadOnlyPtr1 = (int*) indexData.GetUnsafeReadOnlyPtr<byte>();
      ushort* unsafeReadOnlyPtr2 = (ushort*) indexData.GetUnsafeReadOnlyPtr<byte>();
      bool flag1 = boneIdsFormat == VertexAttributeFormat.UInt8;
      byte* unsafeReadOnlyPtr3 = (byte*) boneIdsData.GetUnsafeReadOnlyPtr<byte>();
      bool flag2 = weightsFormat == VertexAttributeFormat.UNorm8;
      byte* unsafeReadOnlyPtr4 = (byte*) weightsData.GetUnsafeReadOnlyPtr<byte>();
      int num1 = indexFormat == IndexFormat.UInt16 ? 2 : 4;
      int num2 = indexData.Length / (3 * num1);
      indexOffset /= 3;
      for (int index = 0; index < num2; ++index)
      {
        int3 int3_1;
        if (num1 == 2)
        {
          int3_1.x = (int) *unsafeReadOnlyPtr2;
          int3_1.y = (int) unsafeReadOnlyPtr2[1];
          int3_1.z = (int) unsafeReadOnlyPtr2[2];
        }
        else
        {
          int3_1.x = *unsafeReadOnlyPtr1;
          int3_1.y = unsafeReadOnlyPtr1[1];
          int3_1.z = unsafeReadOnlyPtr1[2];
        }
        int3_1 -= vertexOffset;
        Triangle3 triangle = new Triangle3(vertices[int3_1.x].m_Vertex, vertices[int3_1.y].m_Vertex, vertices[int3_1.z].m_Vertex);
        int3 int3_2 = int3_1 * boneIdsDim;
        int3 int3_3 = !flag1 ? new int3(*(int*) (unsafeReadOnlyPtr3 + ((IntPtr) int3_2.x * 4).ToInt64()), *(int*) (unsafeReadOnlyPtr3 + ((IntPtr) int3_2.y * 4).ToInt64()), *(int*) (unsafeReadOnlyPtr3 + ((IntPtr) int3_2.z * 4).ToInt64())) : new int3((int) unsafeReadOnlyPtr3[int3_2.x], (int) unsafeReadOnlyPtr3[int3_2.y], (int) unsafeReadOnlyPtr3[int3_2.z]);
        int3 int3_4 = int3_1 * weightsDim;
        bool3 c = weightsDim != 0 ? (!flag2 ? new float3(*(float*) (unsafeReadOnlyPtr4 + ((IntPtr) int3_4.x * 4).ToInt64()), *(float*) (unsafeReadOnlyPtr4 + ((IntPtr) int3_4.y * 4).ToInt64()), *(float*) (unsafeReadOnlyPtr4 + ((IntPtr) int3_4.z * 4).ToInt64())) < new float3(0.5f) : new float3((float) unsafeReadOnlyPtr4[int3_4.x], (float) unsafeReadOnlyPtr4[int3_4.y], (float) unsafeReadOnlyPtr4[int3_4.z]) < new float3(128)) : (bool3) false;
        int3_3 = math.select(int3_3, new int3(-1), c);
        ObjectMeshHelpers.AddTriangle(bones, boneIndex, triangle, int3_3, indexOffset + index);
        unsafeReadOnlyPtr1 += 3;
        unsafeReadOnlyPtr2 += 3;
      }
    }

    private static void AddTriangle(
      NativeArray<ObjectMeshHelpers.BoneData> bones,
      NativeArray<int> boneIndex,
      Triangle3 triangle,
      int3 boneID,
      int triangleIndex)
    {
      if (boneID.x >= 0 && boneID.x < bones.Length && math.all(boneID.xx == boneID.yz))
      {
        ObjectMeshHelpers.BoneData bone = bones[boneID.x];
        bone.m_Bounds |= MathUtils.Bounds(triangle);
        bone.m_TriangleRange.x = math.min(bone.m_TriangleRange.x, triangleIndex);
        bone.m_TriangleRange.y = math.max(bone.m_TriangleRange.y, triangleIndex);
        ++bone.m_TriangleCount;
        bones[boneID.x] = bone;
        boneIndex[triangleIndex] = boneID.x;
      }
      else
        boneIndex[triangleIndex] = -1;
    }

    private static void FillIndices(
      ObjectMeshHelpers.BoneData boneData,
      NativeArray<int> boneIndex,
      NativeArray<int> sourceIndices,
      NativeArray<int> targetIndices,
      int bone)
    {
      int num1 = 0;
      for (int x = boneData.m_TriangleRange.x; x <= boneData.m_TriangleRange.y; ++x)
      {
        if (boneIndex[x] == bone)
        {
          int3 int3 = x * 3 + new int3(0, 1, 2);
          ref NativeArray<int> local1 = ref targetIndices;
          int index1 = num1;
          int num2 = index1 + 1;
          int sourceIndex1 = sourceIndices[int3.x];
          local1[index1] = sourceIndex1;
          ref NativeArray<int> local2 = ref targetIndices;
          int index2 = num2;
          int num3 = index2 + 1;
          int sourceIndex2 = sourceIndices[int3.y];
          local2[index2] = sourceIndex2;
          ref NativeArray<int> local3 = ref targetIndices;
          int index3 = num3;
          num1 = index3 + 1;
          int sourceIndex3 = sourceIndices[int3.z];
          local3[index3] = sourceIndex3;
        }
      }
    }

    private static void FillIndices(
      ObjectMeshHelpers.BoneData boneData,
      NativeArray<int> boneIndex,
      NativeArray<ushort> sourceIndices,
      NativeArray<int> targetIndices,
      int bone)
    {
      int num1 = 0;
      for (int x = boneData.m_TriangleRange.x; x <= boneData.m_TriangleRange.y; ++x)
      {
        if (boneIndex[x] == bone)
        {
          int3 int3 = x * 3 + new int3(0, 1, 2);
          ref NativeArray<int> local1 = ref targetIndices;
          int index1 = num1;
          int num2 = index1 + 1;
          int sourceIndex1 = (int) sourceIndices[int3.x];
          local1[index1] = sourceIndex1;
          ref NativeArray<int> local2 = ref targetIndices;
          int index2 = num2;
          int num3 = index2 + 1;
          int sourceIndex2 = (int) sourceIndices[int3.y];
          local2[index2] = sourceIndex2;
          ref NativeArray<int> local3 = ref targetIndices;
          int index3 = num3;
          num1 = index3 + 1;
          int sourceIndex3 = (int) sourceIndices[int3.z];
          local3[index3] = sourceIndex3;
        }
      }
    }

    private static void CalculateTreeSize(
      int indexCount,
      Bounds3 bounds,
      out int treeDepth,
      out int treeSize,
      out float3 sizeFactor,
      out float3 sizeOffset)
    {
      treeDepth = 1;
      treeSize = 1;
      for (int index = indexCount / 3; index >= 32; index >>= 3)
        treeSize += 1 << 3 * treeDepth++;
      sizeFactor = 1f / math.max((float3) (1f / 1000f), MathUtils.Size(bounds));
      sizeOffset = 0.5f - MathUtils.Center(bounds) * sizeFactor;
    }

    private static void InitializeTree(
      NativeArray<ObjectMeshHelpers.TreeNode> treeNodes,
      int treeSize)
    {
      for (int index = 0; index < treeSize; ++index)
        treeNodes[index] = new ObjectMeshHelpers.TreeNode()
        {
          m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue),
          m_FirstTriangle = -1
        };
    }

    private static void FillTreeNodes(
      NativeArray<ObjectMeshHelpers.TreeNode> treeNodes,
      NativeArray<int> nextTriangle,
      NativeArray<MeshVertex> vertices,
      NativeArray<int> indices,
      float3 sizeOffset,
      float3 sizeFactor,
      int treeDepth)
    {
      int length = nextTriangle.Length;
      for (int index = 0; index < length; ++index)
      {
        int3 int3 = index * 3 + new int3(0, 1, 2);
        Triangle3 triangle = new Triangle3(vertices[indices[int3.x]].m_Vertex, vertices[indices[int3.y]].m_Vertex, vertices[indices[int3.z]].m_Vertex);
        ObjectMeshHelpers.AddTriangle(treeNodes, nextTriangle, sizeOffset, sizeFactor, treeDepth, triangle, index);
      }
    }

    private static void FillTreeNodes(
      NativeArray<ObjectMeshHelpers.TreeNode> treeNodes,
      NativeArray<int> nextTriangle,
      NativeArray<MeshVertex> vertices,
      NativeArray<ushort> indices,
      float3 sizeOffset,
      float3 sizeFactor,
      int treeDepth)
    {
      int length = nextTriangle.Length;
      for (int index = 0; index < length; ++index)
      {
        int3 int3 = index * 3 + new int3(0, 1, 2);
        Triangle3 triangle = new Triangle3(vertices[(int) indices[int3.x]].m_Vertex, vertices[(int) indices[int3.y]].m_Vertex, vertices[(int) indices[int3.z]].m_Vertex);
        ObjectMeshHelpers.AddTriangle(treeNodes, nextTriangle, sizeOffset, sizeFactor, treeDepth, triangle, index);
      }
    }

    private static void AddTriangle(
      NativeArray<ObjectMeshHelpers.TreeNode> treeNodes,
      NativeArray<int> nextTriangle,
      float3 sizeOffset,
      float3 sizeFactor,
      int treeDepth,
      Triangle3 triangle,
      int index)
    {
      Bounds3 bounds = MathUtils.Bounds(triangle);
      float3 float3 = MathUtils.Center(bounds) * sizeFactor + sizeOffset;
      float num1 = math.cmax(MathUtils.Size(bounds) * sizeFactor);
      int num2 = treeDepth - 1;
      int num3 = 0;
      int num4;
      for (num4 = 0; (double) num1 <= 0.5 && num4 < num2; num1 *= 2f)
        num3 += 1 << 3 * num4++;
      int y = 1 << num4;
      int3 x = math.clamp((int3) (float3 * (float) y), (int3) 0, (int3) (y - 1));
      int index1 = num3 + math.dot(x, new int3(1, y, y * y));
      ObjectMeshHelpers.TreeNode treeNode = treeNodes[index1];
      nextTriangle[index] = treeNode.m_FirstTriangle;
      treeNode.m_Bounds |= bounds;
      treeNode.m_FirstTriangle = index;
      ++treeNode.m_ItemCount;
      treeNodes[index1] = treeNode;
    }

    private static unsafe void UpdateNodes(
      NativeArray<ObjectMeshHelpers.TreeNode> treeNodes,
      int treeDepth,
      int* depthOffsets)
    {
      int num1 = treeDepth - 1;
      int num2 = 0;
      int num3 = 0;
      while (num3 < num1)
        num2 += 1 << 3 * num3++;
      while (num3 > 0)
      {
        int z = 1 << num3;
        int num4 = num2;
        num2 -= 1 << 3 * --num3;
        int y1 = 1 << num3;
        int num5 = num2;
        int3 y2 = new int3(2, z << 1, z * z << 1);
        int3 y3 = new int3(1, y1, y1 * y1);
        int4 int4_1 = new int4(0, 1, z, z + 1);
        int4 int4_2 = z * z + int4_1;
        int sourceSize = 0;
        int3 x;
        for (x.z = 0; x.z < y1; ++x.z)
        {
          for (x.y = 0; x.y < y1; ++x.y)
          {
            for (x.x = 0; x.x < y1; ++x.x)
            {
              int num6 = num4 + math.dot(x, y2);
              int index = num5 + math.dot(x, y3);
              int4 int4_3 = num6 + int4_1;
              int4 int4_4 = num6 + int4_2;
              ObjectMeshHelpers.TreeNode treeNode = treeNodes[index];
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_3.x);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_3.y);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_3.z);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_3.w);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_4.x);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_4.y);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_4.z);
              ObjectMeshHelpers.AddBounds(ref treeNode, ref sourceSize, treeNodes, int4_4.w);
              treeNodes[index] = treeNode;
            }
          }
        }
        depthOffsets[num3 + 2] = sourceSize;
      }
      *depthOffsets = 0;
      depthOffsets[1] = 1;
      for (int index = 1; index <= treeDepth; ++index)
      {
        int* numPtr = depthOffsets + index;
        *numPtr = *numPtr + depthOffsets[index - 1];
      }
    }

    private static void AddBounds(
      ref ObjectMeshHelpers.TreeNode targetNode,
      ref int sourceSize,
      NativeArray<ObjectMeshHelpers.TreeNode> treeNodes,
      int sourceIndex)
    {
      ObjectMeshHelpers.TreeNode treeNode = treeNodes[sourceIndex];
      if (treeNode.m_ItemCount == 0)
        return;
      targetNode.m_Bounds |= treeNode.m_Bounds;
      ++targetNode.m_ItemCount;
      if (treeNode.m_ItemCount == 1)
        return;
      treeNode.m_NodeIndex = sourceSize++;
      treeNodes[sourceIndex] = treeNode;
    }

    private static unsafe void FillMeshData(
      NativeArray<ObjectMeshHelpers.TreeNode> sourceNodes,
      NativeArray<int> nextTriangle,
      NativeArray<MeshNode> targetNodes,
      NativeArray<int> sourceIndices,
      NativeArray<MeshIndex> targetIndices,
      int treeDepth,
      int* depthOffsets)
    {
      int* numPtr1 = stackalloc int[128];
      int* numPtr2 = stackalloc int[128];
      int* numPtr3 = stackalloc int[128];
      int index1 = 1;
      int y1 = 0;
      numPtr1[0] = 0;
      numPtr2[0] = 0;
      numPtr3[0] = 0;
      while (--index1 >= 0)
      {
        int num1 = numPtr1[index1];
        int x1 = numPtr2[index1];
        int index2 = numPtr3[index1];
        int num2 = 1 << index2;
        ObjectMeshHelpers.TreeNode sourceNode1 = sourceNodes[num1 + x1];
        int x2 = y1;
        int firstTriangle = sourceNode1.m_FirstTriangle;
        while (firstTriangle >= 0)
        {
          int3 int3_1 = firstTriangle * 3 + new int3(0, 1, 2);
          int3 int3_2 = y1 + new int3(0, 1, 2);
          targetIndices[int3_2.x] = new MeshIndex(sourceIndices[int3_1.x]);
          targetIndices[int3_2.y] = new MeshIndex(sourceIndices[int3_1.y]);
          targetIndices[int3_2.z] = new MeshIndex(sourceIndices[int3_1.z]);
          firstTriangle = nextTriangle[firstTriangle];
          y1 += 3;
        }
        int num3 = 0;
        int4 int4_1 = (int4) -1;
        int4 int4_2 = (int4) -1;
        if (index2 + 1 < treeDepth)
        {
          int3 int3_3 = new int3(x1, x1 >> index2, x1 >> index2 + index2) & num2 - 1;
          for (int index3 = 0; index3 < 8; ++index3)
          {
            int num4 = num1 + (1 << 3 * index2);
            int index4 = index2 + 1;
            int3 x3 = int3_3 * 2 + math.select((int3) 0, (int3) 1, (index3 & new int3(1, 2, 4)) != 0);
            while (index4 < treeDepth)
            {
              int y2 = 1 << index4;
              int num5 = math.dot(x3, new int3(1, y2, y2 * y2));
              ObjectMeshHelpers.TreeNode sourceNode2 = sourceNodes[num4 + num5];
              if (sourceNode2.m_ItemCount == 1)
              {
                if (sourceNode2.m_FirstTriangle != -1)
                {
                  int3 int3_4 = sourceNode2.m_FirstTriangle * 3 + new int3(0, 1, 2);
                  int3 int3_5 = y1 + new int3(0, 1, 2);
                  targetIndices[int3_5.x] = new MeshIndex(sourceIndices[int3_4.x]);
                  targetIndices[int3_5.y] = new MeshIndex(sourceIndices[int3_4.y]);
                  targetIndices[int3_5.z] = new MeshIndex(sourceIndices[int3_4.z]);
                  y1 += 3;
                  break;
                }
                num4 += 1 << 3 * index4++;
                x3 *= 2;
              }
              else
              {
                if (sourceNode2.m_ItemCount != 0)
                {
                  if (num3 < 4)
                    int4_1[num3++] = depthOffsets[index4] + sourceNode2.m_NodeIndex;
                  else
                    int4_2[num3++ - 4] = depthOffsets[index4] + sourceNode2.m_NodeIndex;
                  numPtr1[index1] = num4;
                  numPtr2[index1] = num5;
                  numPtr3[index1] = index4;
                  ++index1;
                  break;
                }
                if (index4 != index2 + 1)
                {
                  if ((x3.x & 1) == 0)
                    ++x3.x;
                  else if ((x3.y & 1) == 0)
                    x3.xy += new int2(-1, 1);
                  else if ((x3.z & 1) == 0)
                    x3 += new int3(-1, -1, 1);
                  else
                    break;
                }
                else
                  break;
              }
            }
          }
        }
        int index5 = depthOffsets[index2] + sourceNode1.m_NodeIndex;
        targetNodes[index5] = new MeshNode()
        {
          m_Bounds = sourceNode1.m_Bounds,
          m_IndexRange = new int2(x2, y1),
          m_SubNodes1 = int4_1,
          m_SubNodes2 = int4_2
        };
      }
    }

    private static unsafe void FillMeshData(
      NativeArray<ObjectMeshHelpers.TreeNode> sourceNodes,
      NativeArray<int> nextTriangle,
      NativeArray<MeshNode> targetNodes,
      NativeArray<ushort> sourceIndices,
      NativeArray<MeshIndex> targetIndices,
      int treeDepth,
      int* depthOffsets)
    {
      int* numPtr1 = stackalloc int[128];
      int* numPtr2 = stackalloc int[128];
      int* numPtr3 = stackalloc int[128];
      int index1 = 1;
      int y1 = 0;
      numPtr1[0] = 0;
      numPtr2[0] = 0;
      numPtr3[0] = 0;
      while (--index1 >= 0)
      {
        int num1 = numPtr1[index1];
        int x1 = numPtr2[index1];
        int index2 = numPtr3[index1];
        int num2 = 1 << index2;
        ObjectMeshHelpers.TreeNode sourceNode1 = sourceNodes[num1 + x1];
        int x2 = y1;
        int firstTriangle = sourceNode1.m_FirstTriangle;
        while (firstTriangle >= 0)
        {
          int3 int3_1 = firstTriangle * 3 + new int3(0, 1, 2);
          int3 int3_2 = y1 + new int3(0, 1, 2);
          targetIndices[int3_2.x] = new MeshIndex((int) sourceIndices[int3_1.x]);
          targetIndices[int3_2.y] = new MeshIndex((int) sourceIndices[int3_1.y]);
          targetIndices[int3_2.z] = new MeshIndex((int) sourceIndices[int3_1.z]);
          firstTriangle = nextTriangle[firstTriangle];
          y1 += 3;
        }
        int num3 = 0;
        int4 int4_1 = (int4) -1;
        int4 int4_2 = (int4) -1;
        if (index2 + 1 < treeDepth)
        {
          int3 int3_3 = new int3(x1, x1 >> index2, x1 >> index2 + index2) & num2 - 1;
          for (int index3 = 0; index3 < 8; ++index3)
          {
            int num4 = num1 + (1 << 3 * index2);
            int index4 = index2 + 1;
            int3 x3 = int3_3 * 2 + math.select((int3) 0, (int3) 1, (index3 & new int3(1, 2, 4)) != 0);
            while (index4 < treeDepth)
            {
              int y2 = 1 << index4;
              int num5 = math.dot(x3, new int3(1, y2, y2 * y2));
              ObjectMeshHelpers.TreeNode sourceNode2 = sourceNodes[num4 + num5];
              if (sourceNode2.m_ItemCount == 1)
              {
                if (sourceNode2.m_FirstTriangle != -1)
                {
                  int3 int3_4 = sourceNode2.m_FirstTriangle * 3 + new int3(0, 1, 2);
                  int3 int3_5 = y1 + new int3(0, 1, 2);
                  targetIndices[int3_5.x] = new MeshIndex((int) sourceIndices[int3_4.x]);
                  targetIndices[int3_5.y] = new MeshIndex((int) sourceIndices[int3_4.y]);
                  targetIndices[int3_5.z] = new MeshIndex((int) sourceIndices[int3_4.z]);
                  y1 += 3;
                  break;
                }
                num4 += 1 << 3 * index4++;
                x3 *= 2;
              }
              else
              {
                if (sourceNode2.m_ItemCount != 0)
                {
                  if (num3 < 4)
                    int4_1[num3++] = depthOffsets[index4] + sourceNode2.m_NodeIndex;
                  else
                    int4_2[num3++ - 4] = depthOffsets[index4] + sourceNode2.m_NodeIndex;
                  numPtr1[index1] = num4;
                  numPtr2[index1] = num5;
                  numPtr3[index1] = index4;
                  ++index1;
                  break;
                }
                if (index4 != index2 + 1)
                {
                  if ((x3.x & 1) == 0)
                    ++x3.x;
                  else if ((x3.y & 1) == 0)
                    x3.xy += new int2(-1, 1);
                  else if ((x3.z & 1) == 0)
                    x3 += new int3(-1, -1, 1);
                  else
                    break;
                }
                else
                  break;
              }
            }
          }
        }
        int index5 = depthOffsets[index2] + sourceNode1.m_NodeIndex;
        targetNodes[index5] = new MeshNode()
        {
          m_Bounds = sourceNode1.m_Bounds,
          m_IndexRange = new int2(x2, y1),
          m_SubNodes1 = int4_1,
          m_SubNodes2 = int4_2
        };
      }
    }

    private struct TreeNode
    {
      public Bounds3 m_Bounds;
      public int m_FirstTriangle;
      public int m_ItemCount;
      public int m_NodeIndex;
    }

    [BurstCompile]
    private struct CacheMeshDataJob : IJob
    {
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Positions;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Normals;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<byte> m_Indices;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public GeometryAsset.Data m_Data;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public IndexFormat m_IndexFormat;
      [ReadOnly]
      public Bounds3 m_MeshBounds;
      [ReadOnly]
      public int m_VertexCount;
      [ReadOnly]
      public int m_IndexCount;
      [ReadOnly]
      public bool m_CacheNormals;
      public EntityCommandBuffer m_CommandBuffer;

      public unsafe void Execute()
      {
        DynamicBuffer<MeshVertex> dst1 = this.m_CommandBuffer.AddBuffer<MeshVertex>(this.m_Entity);
        DynamicBuffer<MeshIndex> dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<MeshIndex>(this.m_Entity);
        DynamicBuffer<MeshNode> dynamicBuffer2 = this.m_CommandBuffer.AddBuffer<MeshNode>(this.m_Entity);
        DynamicBuffer<MeshNormal> dst2 = new DynamicBuffer<MeshNormal>();
        if (this.m_CacheNormals)
          dst2 = this.m_CommandBuffer.AddBuffer<MeshNormal>(this.m_Entity);
        NativeArray<byte> nativeArray1 = new NativeArray<byte>();
        IndexFormat indexFormat;
        NativeArray<byte> nativeArray2;
        int num;
        if (this.m_Data.IsValid)
        {
          int allVertexCount = GeometryAsset.GetAllVertexCount(ref this.m_Data);
          dst1.ResizeUninitialized(allVertexCount);
          if (this.m_CacheNormals)
            dst2.ResizeUninitialized(allVertexCount);
          int start = 0;
          for (int meshIndex = 0; meshIndex < this.m_Data.meshCount; ++meshIndex)
          {
            int vertexCount = GeometryAsset.GetVertexCount(ref this.m_Data, meshIndex);
            VertexAttributeFormat format1;
            int dimension1;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Position, out format1, out dimension1);
            if (dimension1 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have a position");
            MeshVertex.Unpack(GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Position), dst1.AsNativeArray().GetSubArray(start, vertexCount), vertexCount, format1, dimension1);
            if (this.m_CacheNormals)
            {
              VertexAttributeFormat format2;
              int dimension2;
              GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Normal, out format2, out dimension2);
              if (dimension2 == 0)
                throw new Exception("Cannot cache geometry asset: mesh do not have a normal");
              MeshNormal.Unpack(GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Normal), dst2.AsNativeArray().GetSubArray(start, vertexCount), vertexCount, format2, dimension2);
            }
            start += vertexCount;
          }
          indexFormat = IndexFormat.UInt32;
          nativeArray2 = GeometryAsset.ConvertAllIndicesTo32(ref this.m_Data, Allocator.Temp).Reinterpret<byte>(4);
          num = GeometryAsset.GetAllIndicesCount(ref this.m_Data);
        }
        else
        {
          MeshVertex.Unpack(this.m_Positions, dst1, this.m_VertexCount, VertexAttributeFormat.Float32, 3);
          if (this.m_CacheNormals)
            MeshNormal.Unpack(this.m_Normals, dst2, this.m_VertexCount, VertexAttributeFormat.Float32, 3);
          indexFormat = this.m_IndexFormat;
          nativeArray2 = this.m_Indices;
          num = this.m_IndexCount;
        }
        if (num == 0)
          return;
        dynamicBuffer1.ResizeUninitialized(num);
        int treeDepth;
        int treeSize;
        float3 sizeFactor;
        float3 sizeOffset;
        ObjectMeshHelpers.CalculateTreeSize(num, this.m_MeshBounds, out treeDepth, out treeSize, out sizeFactor, out sizeOffset);
        NativeArray<ObjectMeshHelpers.TreeNode> nativeArray3 = new NativeArray<ObjectMeshHelpers.TreeNode>(treeSize, Allocator.Temp);
        NativeArray<int> nextTriangle = new NativeArray<int>(num / 3, Allocator.Temp);
        ObjectMeshHelpers.InitializeTree(nativeArray3, treeSize);
        if (indexFormat == IndexFormat.UInt32)
          ObjectMeshHelpers.FillTreeNodes(nativeArray3, nextTriangle, dst1.AsNativeArray(), nativeArray2.Reinterpret<int>(1), sizeOffset, sizeFactor, treeDepth);
        else
          ObjectMeshHelpers.FillTreeNodes(nativeArray3, nextTriangle, dst1.AsNativeArray(), nativeArray2.Reinterpret<ushort>(1), sizeOffset, sizeFactor, treeDepth);
        int* depthOffsets = stackalloc int[16];
        ObjectMeshHelpers.UpdateNodes(nativeArray3, treeDepth, depthOffsets);
        dynamicBuffer2.ResizeUninitialized(depthOffsets[treeDepth]);
        if (indexFormat == IndexFormat.UInt32)
          ObjectMeshHelpers.FillMeshData(nativeArray3, nextTriangle, dynamicBuffer2.AsNativeArray(), nativeArray2.Reinterpret<int>(1), dynamicBuffer1.AsNativeArray(), treeDepth, depthOffsets);
        else
          ObjectMeshHelpers.FillMeshData(nativeArray3, nextTriangle, dynamicBuffer2.AsNativeArray(), nativeArray2.Reinterpret<ushort>(1), dynamicBuffer1.AsNativeArray(), treeDepth, depthOffsets);
        nextTriangle.Dispose();
        nativeArray3.Dispose();
        if (!this.m_Data.IsValid)
          return;
        nativeArray2.Dispose();
      }
    }

    [BurstCompile]
    private struct CacheProceduralMeshDataJob : IJob
    {
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Positions;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Normals;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_BoneIds;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_BoneInfluences;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<byte> m_Indices;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public GeometryAsset.Data m_Data;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public IndexFormat m_IndexFormat;
      [ReadOnly]
      public int m_VertexCount;
      [ReadOnly]
      public int m_IndexCount;
      [ReadOnly]
      public int m_BoneCount;
      [ReadOnly]
      public bool m_CacheNormals;
      public EntityCommandBuffer m_CommandBuffer;

      public unsafe void Execute()
      {
        DynamicBuffer<MeshVertex> dst1 = this.m_CommandBuffer.AddBuffer<MeshVertex>(this.m_Entity);
        DynamicBuffer<MeshIndex> dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<MeshIndex>(this.m_Entity);
        DynamicBuffer<MeshNode> dynamicBuffer2 = this.m_CommandBuffer.AddBuffer<MeshNode>(this.m_Entity);
        DynamicBuffer<MeshNormal> dst2 = new DynamicBuffer<MeshNormal>();
        if (this.m_CacheNormals)
          dst2 = this.m_CommandBuffer.AddBuffer<MeshNormal>(this.m_Entity);
        NativeArray<byte> nativeArray1 = new NativeArray<byte>();
        NativeArray<ObjectMeshHelpers.BoneData> bones = new NativeArray<ObjectMeshHelpers.BoneData>();
        NativeArray<int> boneIndex = new NativeArray<int>();
        IndexFormat indexFormat;
        NativeArray<byte> indexData;
        int num1;
        if (this.m_Data.IsValid)
        {
          int allVertexCount = GeometryAsset.GetAllVertexCount(ref this.m_Data);
          dst1.ResizeUninitialized(allVertexCount);
          if (this.m_CacheNormals)
            dst2.ResizeUninitialized(allVertexCount);
          indexFormat = IndexFormat.UInt32;
          indexData = GeometryAsset.ConvertAllIndicesTo32(ref this.m_Data, Allocator.Temp).Reinterpret<byte>(4);
          int allIndicesCount = GeometryAsset.GetAllIndicesCount(ref this.m_Data);
          bones = new NativeArray<ObjectMeshHelpers.BoneData>(this.m_BoneCount, Allocator.Temp);
          boneIndex = new NativeArray<int>(allIndicesCount / 3, Allocator.Temp);
          ObjectMeshHelpers.InitializeBones(bones, allIndicesCount);
          int num2 = 0;
          num1 = 0;
          for (int meshIndex = 0; meshIndex < this.m_Data.meshCount; ++meshIndex)
          {
            int vertexCount = GeometryAsset.GetVertexCount(ref this.m_Data, meshIndex);
            int indicesCount = GeometryAsset.GetIndicesCount(ref this.m_Data, meshIndex);
            VertexAttributeFormat format1;
            int dimension1;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Position, out format1, out dimension1);
            if (dimension1 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have a position");
            NativeSlice<byte> attributeData1 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Position);
            NativeArray<MeshVertex> subArray = dst1.AsNativeArray().GetSubArray(num2, vertexCount);
            NativeArray<MeshVertex> dst3 = subArray;
            int count = vertexCount;
            int format2 = (int) format1;
            int dimension2 = dimension1;
            MeshVertex.Unpack(attributeData1, dst3, count, (VertexAttributeFormat) format2, dimension2);
            if (this.m_CacheNormals)
            {
              VertexAttributeFormat format3;
              int dimension3;
              GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Normal, out format3, out dimension3);
              if (dimension3 == 0)
                throw new Exception("Cannot cache geometry asset: mesh do not have a normal");
              MeshNormal.Unpack(GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Normal), dst2.AsNativeArray().GetSubArray(num2, vertexCount), vertexCount, format3, dimension3);
            }
            VertexAttributeFormat format4;
            int dimension4;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.BlendIndices, out format4, out dimension4);
            VertexAttributeFormat format5;
            int dimension5;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.BlendWeight, out format5, out dimension5);
            if (dimension4 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have bone ID data");
            if (format4 != VertexAttributeFormat.UInt32 && format4 != VertexAttributeFormat.UInt8)
              throw new Exception("Cannot cache geometry asset: only UInt32 or UInt8 bone IDs format is supported");
            if (dimension5 != 0 && format5 != VertexAttributeFormat.Float32 && format5 != VertexAttributeFormat.UNorm8)
              throw new Exception("Cannot cache geometry asset: only Float32 or UNorm8 bone weights formats are supported");
            NativeSlice<byte> attributeData2 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.BlendIndices);
            NativeSlice<byte> attributeData3 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.BlendWeight);
            ObjectMeshHelpers.FillBoneData(bones, boneIndex, subArray, num2, attributeData2, dimension4, format4, attributeData3, dimension5, format5, indexData.GetSubArray(num1 * 4, indicesCount * 4), IndexFormat.UInt32, num1);
            num2 += vertexCount;
            num1 += indicesCount;
          }
        }
        else
        {
          MeshVertex.Unpack(this.m_Positions, dst1, this.m_VertexCount, VertexAttributeFormat.Float32, 3);
          if (this.m_CacheNormals)
            MeshNormal.Unpack(this.m_Normals, dst2, this.m_VertexCount, VertexAttributeFormat.Float32, 3);
          indexFormat = this.m_IndexFormat;
          indexData = this.m_Indices;
          num1 = this.m_IndexCount;
          NativeSlice<byte> boneIds = this.m_BoneIds;
          NativeSlice<byte> boneInfluences = this.m_BoneInfluences;
          bones = new NativeArray<ObjectMeshHelpers.BoneData>(this.m_BoneCount, Allocator.Temp);
          boneIndex = new NativeArray<int>(num1 / 3, Allocator.Temp);
          ObjectMeshHelpers.InitializeBones(bones, num1);
          ObjectMeshHelpers.FillBoneData(bones, boneIndex, dst1.AsNativeArray(), 0, boneIds, 4, VertexAttributeFormat.UInt32, boneInfluences, 4, VertexAttributeFormat.Float32, indexData, indexFormat, 0);
        }
        if (num1 == 0)
          return;
        dynamicBuffer1.ResizeUninitialized(num1);
        int num3 = 0;
        int length = 0;
        int num4 = 0;
        int num5 = 0;
        for (int index = 0; index < this.m_BoneCount; ++index)
        {
          ObjectMeshHelpers.BoneData boneData = bones[index];
          if (boneData.m_TriangleCount != 0)
          {
            num3 += boneData.m_TriangleCount;
            num4 = math.max(num4, boneData.m_TriangleCount);
            int treeSize;
            ObjectMeshHelpers.CalculateTreeSize(boneData.m_TriangleCount * 3, boneData.m_Bounds, out int _, out treeSize, out float3 _, out float3 _);
            length += treeSize;
            num5 = math.max(num5, treeSize);
          }
          else
            ++length;
        }
        NativeArray<ObjectMeshHelpers.TreeNode> nativeArray2 = new NativeArray<ObjectMeshHelpers.TreeNode>(num5, Allocator.Temp);
        NativeArray<int> nativeArray3 = new NativeArray<int>(num4, Allocator.Temp);
        NativeArray<int> nativeArray4 = new NativeArray<int>(num4 * 3, Allocator.Temp);
        dynamicBuffer1.ResizeUninitialized(num3 * 3);
        dynamicBuffer2.ResizeUninitialized(length);
        int start1 = 0;
        int start2 = this.m_BoneCount - 1;
        int* depthOffsets = stackalloc int[16];
        for (int index1 = 0; index1 < this.m_BoneCount; ++index1)
        {
          ObjectMeshHelpers.BoneData boneData = bones[index1];
          if (boneData.m_TriangleCount != 0)
          {
            int treeDepth;
            int treeSize;
            float3 sizeFactor;
            float3 sizeOffset;
            ObjectMeshHelpers.CalculateTreeSize(boneData.m_TriangleCount * 3, boneData.m_Bounds, out treeDepth, out treeSize, out sizeFactor, out sizeOffset);
            NativeArray<ObjectMeshHelpers.TreeNode> subArray1 = nativeArray2.GetSubArray(0, treeSize);
            NativeArray<int> subArray2 = nativeArray3.GetSubArray(0, boneData.m_TriangleCount);
            NativeArray<int> subArray3 = nativeArray4.GetSubArray(0, boneData.m_TriangleCount * 3);
            ObjectMeshHelpers.InitializeTree(subArray1, treeSize);
            if (indexFormat == IndexFormat.UInt32)
              ObjectMeshHelpers.FillIndices(boneData, boneIndex, indexData.Reinterpret<int>(1), subArray3, index1);
            else
              ObjectMeshHelpers.FillIndices(boneData, boneIndex, indexData.Reinterpret<ushort>(1), subArray3, index1);
            ObjectMeshHelpers.FillTreeNodes(subArray1, subArray2, dst1.AsNativeArray(), subArray3, sizeOffset, sizeFactor, treeDepth);
            ObjectMeshHelpers.UpdateNodes(subArray1, treeDepth, depthOffsets);
            NativeArray<MeshIndex> subArray4 = dynamicBuffer1.AsNativeArray().GetSubArray(start1, boneData.m_TriangleCount * 3);
            NativeArray<MeshNode> subArray5 = dynamicBuffer2.AsNativeArray().GetSubArray(start2, depthOffsets[treeDepth]);
            MeshNode meshNode1 = subArray5[0];
            ObjectMeshHelpers.FillMeshData(subArray1, subArray2, subArray5, subArray3, subArray4, treeDepth, depthOffsets);
            for (int index2 = 0; index2 < subArray5.Length; ++index2)
            {
              MeshNode meshNode2 = subArray5[index2];
              meshNode2.m_IndexRange += start1;
              meshNode2.m_SubNodes1 = math.select(meshNode2.m_SubNodes1, meshNode2.m_SubNodes1 + start2, meshNode2.m_SubNodes1 != -1);
              meshNode2.m_SubNodes2 = math.select(meshNode2.m_SubNodes2, meshNode2.m_SubNodes2 + start2, meshNode2.m_SubNodes2 != -1);
              subArray5[index2] = meshNode2;
            }
            if (start2 != index1)
            {
              dynamicBuffer2[index1] = subArray5[0];
              subArray5[0] = meshNode1;
            }
            start1 += subArray4.Length;
            start2 += subArray5.Length - 1;
          }
          else
            dynamicBuffer2[index1] = new MeshNode()
            {
              m_IndexRange = (int2) start1,
              m_SubNodes1 = (int4) -1,
              m_SubNodes2 = (int4) -1
            };
        }
        nativeArray3.Dispose();
        nativeArray2.Dispose();
        bones.Dispose();
        boneIndex.Dispose();
        dynamicBuffer2.Length = start2 + 1;
        dynamicBuffer2.TrimExcess();
        if (!this.m_Data.IsValid)
          return;
        indexData.Dispose();
      }
    }

    private struct BoneData
    {
      public Bounds3 m_Bounds;
      public int2 m_TriangleRange;
      public int m_TriangleCount;
    }
  }
}
