// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchMeshHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Prefabs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  public static class BatchMeshHelpers
  {
    public static JobHandle GenerateMeshes(
      BatchMeshSystem meshSystem,
      NativeList<Entity> meshes,
      Mesh.MeshDataArray meshDataArray,
      JobHandle dependencies)
    {
      return new BatchMeshHelpers.GenerateBatchMeshJob()
      {
        m_Entities = meshes,
        m_MeshData = meshSystem.GetComponentLookup<Game.Prefabs.MeshData>(true),
        m_CompositionMeshData = meshSystem.GetComponentLookup<NetCompositionMeshData>(true),
        m_CompositionPieces = meshSystem.GetBufferLookup<NetCompositionPiece>(true),
        m_MeshVertices = meshSystem.GetBufferLookup<MeshVertex>(true),
        m_MeshNormals = meshSystem.GetBufferLookup<MeshNormal>(true),
        m_MeshTangents = meshSystem.GetBufferLookup<MeshTangent>(true),
        m_MeshUV0s = meshSystem.GetBufferLookup<MeshUV0>(true),
        m_MeshIndices = meshSystem.GetBufferLookup<MeshIndex>(true),
        m_MeshNodes = meshSystem.GetBufferLookup<MeshNode>(true),
        m_MeshMaterials = meshSystem.GetBufferLookup<MeshMaterial>(false),
        m_MeshDataArray = meshDataArray
      }.Schedule<BatchMeshHelpers.GenerateBatchMeshJob>(meshes.Length, 1, dependencies);
    }

    [BurstCompile]
    private struct GenerateBatchMeshJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeList<Entity> m_Entities;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_MeshData;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> m_CompositionMeshData;
      [ReadOnly]
      public BufferLookup<NetCompositionPiece> m_CompositionPieces;
      [ReadOnly]
      public BufferLookup<MeshVertex> m_MeshVertices;
      [ReadOnly]
      public BufferLookup<MeshNormal> m_MeshNormals;
      [ReadOnly]
      public BufferLookup<MeshTangent> m_MeshTangents;
      [ReadOnly]
      public BufferLookup<MeshUV0> m_MeshUV0s;
      [ReadOnly]
      public BufferLookup<MeshIndex> m_MeshIndices;
      [ReadOnly]
      public BufferLookup<MeshNode> m_MeshNodes;
      [NativeDisableParallelForRestriction]
      public BufferLookup<MeshMaterial> m_MeshMaterials;
      public Mesh.MeshDataArray m_MeshDataArray;

      public void Execute(int index)
      {
        Entity entity = this.m_Entities[index];
        Game.Prefabs.MeshData componentData1;
        if (this.m_MeshData.TryGetComponent(entity, out componentData1))
        {
          DynamicBuffer<MeshVertex> meshVertex = this.m_MeshVertices[entity];
          DynamicBuffer<MeshIndex> meshIndex = this.m_MeshIndices[entity];
          DynamicBuffer<MeshNormal> cachedNormals = new DynamicBuffer<MeshNormal>();
          if (this.m_MeshNormals.HasBuffer(entity))
            cachedNormals = this.m_MeshNormals[entity];
          DynamicBuffer<MeshNode> nodes = new DynamicBuffer<MeshNode>();
          if (this.m_MeshNodes.HasBuffer(entity))
            nodes = this.m_MeshNodes[entity];
          this.GenerateObjectMesh(componentData1, meshVertex, cachedNormals, meshIndex, nodes, this.m_MeshDataArray[index]);
        }
        else
        {
          NetCompositionMeshData componentData2;
          if (!this.m_CompositionMeshData.TryGetComponent(entity, out componentData2))
            return;
          DynamicBuffer<NetCompositionPiece> compositionPiece = this.m_CompositionPieces[entity];
          DynamicBuffer<MeshMaterial> meshMaterial = this.m_MeshMaterials[entity];
          this.GenerateCompositionMesh(componentData2, compositionPiece, meshMaterial, this.m_MeshDataArray[index]);
        }
      }

      private int GetMaterial(DynamicBuffer<MeshMaterial> materials, MeshMaterial pieceMaterial)
      {
        int length = materials.Length;
        for (int index = 0; index < length; ++index)
        {
          if (materials[index].m_MaterialIndex == pieceMaterial.m_MaterialIndex)
            return index;
        }
        materials.Add(new MeshMaterial()
        {
          m_MaterialIndex = pieceMaterial.m_MaterialIndex
        });
        return length;
      }

      private void GenerateObjectMesh(
        Game.Prefabs.MeshData objectMeshData,
        DynamicBuffer<MeshVertex> cachedVertices,
        DynamicBuffer<MeshNormal> cachedNormals,
        DynamicBuffer<MeshIndex> cachedIndices,
        DynamicBuffer<MeshNode> nodes,
        Mesh.MeshData meshData)
      {
        int vertexCount1 = 0;
        int indexCount1 = 0;
        int num1 = 0;
        NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BasePoint> basePoints = new NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BasePoint>();
        NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BaseLine> baseLines = new NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BaseLine>();
        float baseOffset = 0.0f;
        if ((objectMeshData.m_State & MeshFlags.Base) != (MeshFlags) 0)
        {
          basePoints = new NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BasePoint>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          baseLines = new NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BaseLine>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          baseOffset = math.select(0.0f, objectMeshData.m_Bounds.min.y, (objectMeshData.m_State & MeshFlags.MinBounds) > (MeshFlags) 0);
          this.AddBaseLines(basePoints, baseLines, cachedVertices, cachedNormals, cachedIndices, nodes, baseOffset);
          vertexCount1 += basePoints.Length * 2;
          indexCount1 += baseLines.Length * 6;
          ++num1;
        }
        NativeArray<VertexAttributeDescriptor> nativeArray = new NativeArray<VertexAttributeDescriptor>(5, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        BatchMeshHelpers.GenerateBatchMeshJob.SetupGeneratedMeshAttributes(nativeArray);
        bool use32bitIndices = indexCount1 >= 65536;
        meshData.SetVertexBufferParams(vertexCount1, nativeArray);
        meshData.SetIndexBufferParams(indexCount1, use32bitIndices ? IndexFormat.UInt32 : IndexFormat.UInt16);
        nativeArray.Dispose();
        meshData.subMeshCount = num1;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        if ((objectMeshData.m_State & MeshFlags.Base) != (MeshFlags) 0)
        {
          ref Mesh.MeshData local = ref meshData;
          int index = num4;
          int num5 = index + 1;
          SubMeshDescriptor desc = new SubMeshDescriptor()
          {
            firstVertex = num2,
            indexStart = num3,
            vertexCount = basePoints.Length * 2,
            indexCount = baseLines.Length * 6,
            topology = MeshTopology.Triangles
          };
          local.SetSubMesh(index, desc, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        }
        NativeArray<BatchMeshHelpers.GenerateBatchMeshJob.VertexData> vertexData = meshData.GetVertexData<BatchMeshHelpers.GenerateBatchMeshJob.VertexData>();
        NativeArray<uint> indices32 = new NativeArray<uint>();
        NativeArray<ushort> indices16 = new NativeArray<ushort>();
        if (use32bitIndices)
          indices32 = meshData.GetIndexData<uint>();
        else
          indices16 = meshData.GetIndexData<ushort>();
        int vertexCount2 = 0;
        int indexCount2 = 0;
        if ((objectMeshData.m_State & MeshFlags.Base) == (MeshFlags) 0)
          return;
        this.AddBaseVertices(basePoints, baseLines, vertexData, indices32, indices16, use32bitIndices, baseOffset, ref vertexCount2, ref indexCount2);
        basePoints.Dispose();
        baseLines.Dispose();
      }

      private void AddBaseVertices(
        NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BasePoint> basePoints,
        NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BaseLine> baseLines,
        NativeArray<BatchMeshHelpers.GenerateBatchMeshJob.VertexData> vertices,
        NativeArray<uint> indices32,
        NativeArray<ushort> indices16,
        bool use32bitIndices,
        float baseOffset,
        ref int vertexCount,
        ref int indexCount)
      {
        for (int index = 0; index < baseLines.Length; ++index)
        {
          BatchMeshHelpers.GenerateBatchMeshJob.BaseLine baseLine = baseLines[index];
          int num1 = vertexCount + baseLine.m_StartIndex * 2;
          int num2 = vertexCount + baseLine.m_EndIndex * 2;
          if (use32bitIndices)
          {
            indices32[indexCount++] = (uint) num1;
            indices32[indexCount++] = (uint) (num1 + 1);
            indices32[indexCount++] = (uint) num2;
            indices32[indexCount++] = (uint) num2;
            indices32[indexCount++] = (uint) (num1 + 1);
            indices32[indexCount++] = (uint) (num2 + 1);
          }
          else
          {
            indices16[indexCount++] = (ushort) num1;
            indices16[indexCount++] = (ushort) (num1 + 1);
            indices16[indexCount++] = (ushort) num2;
            indices16[indexCount++] = (ushort) num2;
            indices16[indexCount++] = (ushort) (num1 + 1);
            indices16[indexCount++] = (ushort) (num2 + 1);
          }
        }
        for (int index1 = 0; index1 < basePoints.Length; ++index1)
        {
          BatchMeshHelpers.GenerateBatchMeshJob.BasePoint basePoint = basePoints[index1];
          float3 n = new float3(basePoint.m_Direction.x, 0.0f, basePoint.m_Direction.y);
          float4 t = new float4(n.z, 0.0f, -n.x, 1f);
          uint octahedral1 = MathUtils.NormalToOctahedral(n);
          uint octahedral2 = MathUtils.TangentToOctahedral(t);
          ref NativeArray<BatchMeshHelpers.GenerateBatchMeshJob.VertexData> local1 = ref vertices;
          int index2 = vertexCount++;
          BatchMeshHelpers.GenerateBatchMeshJob.VertexData vertexData1 = new BatchMeshHelpers.GenerateBatchMeshJob.VertexData();
          vertexData1.m_Position = new float3(basePoint.m_Position.x, baseOffset, basePoint.m_Position.y);
          vertexData1.m_Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
          vertexData1.m_Normal = octahedral1;
          vertexData1.m_Tangent = octahedral2;
          vertexData1.m_UV0 = new half4(new float4(basePoint.m_Distance, 1f, 0.0f, 0.0f));
          BatchMeshHelpers.GenerateBatchMeshJob.VertexData vertexData2 = vertexData1;
          local1[index2] = vertexData2;
          ref NativeArray<BatchMeshHelpers.GenerateBatchMeshJob.VertexData> local2 = ref vertices;
          int index3 = vertexCount++;
          vertexData1 = new BatchMeshHelpers.GenerateBatchMeshJob.VertexData();
          vertexData1.m_Position = new float3(basePoint.m_Position.x, baseOffset - 1f, basePoint.m_Position.y);
          vertexData1.m_Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
          vertexData1.m_Normal = octahedral1;
          vertexData1.m_Tangent = octahedral2;
          vertexData1.m_UV0 = new half4(new float4(basePoint.m_Distance, 0.0f, 0.0f, 0.0f));
          BatchMeshHelpers.GenerateBatchMeshJob.VertexData vertexData3 = vertexData1;
          local2[index3] = vertexData3;
        }
      }

      private unsafe void AddBaseLines(
        NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BasePoint> basePoints,
        NativeList<BatchMeshHelpers.GenerateBatchMeshJob.BaseLine> baseLines,
        DynamicBuffer<MeshVertex> vertices,
        DynamicBuffer<MeshNormal> normals,
        DynamicBuffer<MeshIndex> indices,
        DynamicBuffer<MeshNode> nodes,
        float baseOffset)
      {
        NativeHashSet<float4> nativeHashSet = new NativeHashSet<float4>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeHashMap<float2, int> nativeHashMap = new NativeHashMap<float2, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<int> nativeList = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        float2 float2 = new float2(baseOffset - 0.01f, baseOffset + 0.01f);
        int* numPtr = stackalloc int[128];
        int a1 = 0;
        if (nodes.IsCreated && nodes.Length != 0)
          numPtr[a1++] = 0;
        Triangle1 y;
        while (--a1 >= 0)
        {
          int index = numPtr[a1];
          MeshNode node = nodes[index];
          if ((double) node.m_Bounds.min.y < (double) float2.y && (double) node.m_Bounds.max.y > (double) float2.x)
          {
            for (int x1 = node.m_IndexRange.x; x1 < node.m_IndexRange.y; x1 += 3)
            {
              int3 int3 = new int3(x1, x1 + 1, x1 + 2);
              int3 = new int3(indices[int3.x].m_Index, indices[int3.y].m_Index, indices[int3.z].m_Index);
              Triangle3 triangle3 = new Triangle3(vertices[int3.x].m_Vertex, vertices[int3.y].m_Vertex, vertices[int3.z].m_Vertex);
              y = triangle3.y;
              bool3 bool3_1 = y.abc > float2.x;
              y = triangle3.y;
              bool3 bool3_2 = y.abc < float2.y;
              bool3 bool3_3 = bool3_1 & bool3_2;
              y = triangle3.y;
              bool3 x2 = y.abc < float2.y & bool3_3.yzx & bool3_3.zxy;
              if (math.any(x2))
              {
                if (x2.x)
                  nativeHashSet.Add(new float4(triangle3.b.xz, triangle3.c.xz));
                if (x2.y)
                  nativeHashSet.Add(new float4(triangle3.c.xz, triangle3.a.xz));
                if (x2.z)
                  nativeHashSet.Add(new float4(triangle3.a.xz, triangle3.b.xz));
              }
            }
            numPtr[a1] = node.m_SubNodes1.x;
            int a2 = math.select(a1, a1 + 1, node.m_SubNodes1.x != -1);
            numPtr[a2] = node.m_SubNodes1.y;
            int a3 = math.select(a2, a2 + 1, node.m_SubNodes1.y != -1);
            numPtr[a3] = node.m_SubNodes1.z;
            int a4 = math.select(a3, a3 + 1, node.m_SubNodes1.z != -1);
            numPtr[a4] = node.m_SubNodes1.w;
            int a5 = math.select(a4, a4 + 1, node.m_SubNodes1.w != -1);
            numPtr[a5] = node.m_SubNodes2.x;
            int a6 = math.select(a5, a5 + 1, node.m_SubNodes2.x != -1);
            numPtr[a6] = node.m_SubNodes2.y;
            int a7 = math.select(a6, a6 + 1, node.m_SubNodes2.y != -1);
            numPtr[a7] = node.m_SubNodes2.z;
            int a8 = math.select(a7, a7 + 1, node.m_SubNodes2.z != -1);
            numPtr[a8] = node.m_SubNodes2.w;
            a1 = math.select(a8, a8 + 1, node.m_SubNodes2.w != -1);
          }
        }
        int a9 = 0;
        if (nodes.IsCreated && nodes.Length != 0)
          numPtr[a9++] = 0;
        while (--a9 >= 0)
        {
          int index = numPtr[a9];
          MeshNode node = nodes[index];
          if ((double) node.m_Bounds.min.y < (double) float2.y && (double) node.m_Bounds.max.y > (double) float2.x)
          {
            for (int x3 = node.m_IndexRange.x; x3 < node.m_IndexRange.y; x3 += 3)
            {
              int3 int3 = new int3(x3, x3 + 1, x3 + 2);
              int3 = new int3(indices[int3.x].m_Index, indices[int3.y].m_Index, indices[int3.z].m_Index);
              Triangle3 triangle3 = new Triangle3(vertices[int3.x].m_Vertex, vertices[int3.y].m_Vertex, vertices[int3.z].m_Vertex);
              y = triangle3.y;
              bool3 bool3_4 = y.abc > float2.x;
              y = triangle3.y;
              bool3 bool3_5 = y.abc < float2.y;
              bool3 bool3_6 = bool3_4 & bool3_5;
              y = triangle3.y;
              bool3 x4 = y.abc >= float2.y & bool3_6.yzx & bool3_6.zxy;
              if (math.any(x4))
              {
                BatchMeshHelpers.GenerateBatchMeshJob.BasePoint basePoint1;
                BatchMeshHelpers.GenerateBatchMeshJob.BasePoint basePoint2;
                BatchMeshHelpers.GenerateBatchMeshJob.BasePoint basePoint3;
                if (x4.x)
                {
                  basePoint1 = new BatchMeshHelpers.GenerateBatchMeshJob.BasePoint();
                  basePoint1.m_Position = triangle3.b.xz;
                  basePoint1.m_Direction = normals[int3.y].m_Normal.xz;
                  basePoint2 = basePoint1;
                  basePoint1 = new BatchMeshHelpers.GenerateBatchMeshJob.BasePoint();
                  basePoint1.m_Position = triangle3.c.xz;
                  basePoint1.m_Direction = normals[int3.z].m_Normal.xz;
                  basePoint3 = basePoint1;
                }
                else if (x4.y)
                {
                  basePoint1 = new BatchMeshHelpers.GenerateBatchMeshJob.BasePoint();
                  basePoint1.m_Position = triangle3.c.xz;
                  basePoint1.m_Direction = normals[int3.z].m_Normal.xz;
                  basePoint2 = basePoint1;
                  basePoint1 = new BatchMeshHelpers.GenerateBatchMeshJob.BasePoint();
                  basePoint1.m_Position = triangle3.a.xz;
                  basePoint1.m_Direction = normals[int3.x].m_Normal.xz;
                  basePoint3 = basePoint1;
                }
                else
                {
                  basePoint1 = new BatchMeshHelpers.GenerateBatchMeshJob.BasePoint();
                  basePoint1.m_Position = triangle3.a.xz;
                  basePoint1.m_Direction = normals[int3.x].m_Normal.xz;
                  basePoint2 = basePoint1;
                  basePoint1 = new BatchMeshHelpers.GenerateBatchMeshJob.BasePoint();
                  basePoint1.m_Position = triangle3.b.xz;
                  basePoint1.m_Direction = normals[int3.y].m_Normal.xz;
                  basePoint3 = basePoint1;
                }
                if (!nativeHashSet.Contains(new float4(basePoint2.m_Position, basePoint3.m_Position)) && !nativeHashSet.Contains(new float4(basePoint3.m_Position, basePoint2.m_Position)))
                {
                  basePoint2.m_Distance = -1f;
                  basePoint3.m_Distance = -1f;
                  basePoint2.m_PrevPos = (float2) float.NaN;
                  basePoint3.m_PrevPos = basePoint2.m_Position;
                  int num1;
                  if (nativeHashMap.TryGetValue(basePoint2.m_Position, out num1))
                  {
                    num1 = math.select(num1, -1 - num1, num1 < 0);
                    if (!basePoints[num1].m_Direction.Equals(basePoint2.m_Direction))
                    {
                      num1 = basePoints.Length;
                      basePoints.Add(in basePoint2);
                    }
                  }
                  else
                  {
                    num1 = basePoints.Length;
                    nativeHashMap.Add(basePoint2.m_Position, num1);
                    basePoints.Add(in basePoint2);
                  }
                  int num2;
                  if (nativeHashMap.TryGetValue(basePoint3.m_Position, out num2))
                  {
                    num2 = math.select(num2, -1 - num2, num2 < 0);
                    ref BatchMeshHelpers.GenerateBatchMeshJob.BasePoint local = ref basePoints.ElementAt(num2);
                    if (!local.m_Direction.Equals(basePoint3.m_Direction))
                    {
                      num2 = basePoints.Length;
                      basePoints.Add(in basePoint3);
                    }
                    else
                      local.m_PrevPos = basePoint2.m_Position;
                    nativeHashMap[basePoint3.m_Position] = -1 - num2;
                  }
                  else
                  {
                    num2 = basePoints.Length;
                    nativeHashMap.Add(basePoint3.m_Position, -1 - num2);
                    basePoints.Add(in basePoint3);
                  }
                  baseLines.Add(new BatchMeshHelpers.GenerateBatchMeshJob.BaseLine()
                  {
                    m_StartIndex = num1,
                    m_EndIndex = num2
                  });
                }
              }
            }
            numPtr[a9] = node.m_SubNodes1.x;
            int a10 = math.select(a9, a9 + 1, node.m_SubNodes1.x != -1);
            numPtr[a10] = node.m_SubNodes1.y;
            int a11 = math.select(a10, a10 + 1, node.m_SubNodes1.y != -1);
            numPtr[a11] = node.m_SubNodes1.z;
            int a12 = math.select(a11, a11 + 1, node.m_SubNodes1.z != -1);
            numPtr[a12] = node.m_SubNodes1.w;
            int a13 = math.select(a12, a12 + 1, node.m_SubNodes1.w != -1);
            numPtr[a13] = node.m_SubNodes2.x;
            int a14 = math.select(a13, a13 + 1, node.m_SubNodes2.x != -1);
            numPtr[a14] = node.m_SubNodes2.y;
            int a15 = math.select(a14, a14 + 1, node.m_SubNodes2.y != -1);
            numPtr[a15] = node.m_SubNodes2.z;
            int a16 = math.select(a15, a15 + 1, node.m_SubNodes2.z != -1);
            numPtr[a16] = node.m_SubNodes2.w;
            a9 = math.select(a16, a16 + 1, node.m_SubNodes2.w != -1);
          }
        }
        for (int index1 = 0; index1 < baseLines.Length; ++index1)
        {
          BatchMeshHelpers.GenerateBatchMeshJob.BaseLine baseLine = baseLines[index1];
          ref BatchMeshHelpers.GenerateBatchMeshJob.BasePoint local1 = ref basePoints.ElementAt(baseLine.m_StartIndex);
          ref BatchMeshHelpers.GenerateBatchMeshJob.BasePoint local2 = ref basePoints.ElementAt(baseLine.m_EndIndex);
          if ((double) local1.m_Distance < 0.0)
          {
            int num3 = nativeHashMap[local1.m_Position];
            if (num3 >= 0)
            {
              local1.m_Distance = 0.0f;
            }
            else
            {
              int index2 = -1 - num3;
              ref BatchMeshHelpers.GenerateBatchMeshJob.BasePoint local3 = ref basePoints.ElementAt(index2);
              if ((double) local3.m_Distance < 0.0)
              {
                int num4 = index2;
                if (!math.isnan(local3.m_PrevPos.x))
                {
                  for (int index3 = 0; index3 <= basePoints.Length; ++index3)
                  {
                    int num5 = nativeHashMap[local3.m_PrevPos];
                    if (num5 >= 0)
                    {
                      int index4 = num5;
                      local3 = ref basePoints.ElementAt(index4);
                      break;
                    }
                    nativeList.Add(in index2);
                    index2 = -1 - num5;
                    local3 = ref basePoints.ElementAt(index2);
                    if ((double) local3.m_Distance >= 0.0 || index2 == num4 || math.isnan(local3.m_PrevPos.x))
                      break;
                  }
                }
                if ((double) local3.m_Distance < 0.0)
                  local3.m_Distance = 0.0f;
                for (int index5 = nativeList.Length - 1; index5 >= 0; --index5)
                {
                  int index6 = nativeList[index5];
                  ref BatchMeshHelpers.GenerateBatchMeshJob.BasePoint local4 = ref basePoints.ElementAt(index6);
                  if (index5 != 0 || index6 != num4)
                    local4.m_Distance = local3.m_Distance + math.distance(local3.m_Position, local4.m_Position);
                  local3 = ref local4;
                }
                nativeList.Clear();
              }
              local1.m_Distance = local3.m_Distance;
            }
          }
          float num = local1.m_Distance + math.distance(local1.m_Position, local2.m_Position);
          if ((double) local2.m_Distance >= 0.0 && (double) num != (double) local2.m_Distance)
          {
            BatchMeshHelpers.GenerateBatchMeshJob.BasePoint basePoint = local2 with
            {
              m_Distance = num
            };
            baseLine.m_EndIndex = basePoints.Length;
            basePoints.Add(in basePoint);
            baseLines[index1] = baseLine;
          }
          else
            local2.m_Distance = num;
        }
        nativeHashSet.Dispose();
        nativeHashMap.Dispose();
        nativeList.Dispose();
      }

      private void GenerateCompositionMesh(
        NetCompositionMeshData compositionMeshData,
        DynamicBuffer<NetCompositionPiece> pieces,
        DynamicBuffer<MeshMaterial> materials,
        Mesh.MeshData meshData)
      {
        bool flag1 = (compositionMeshData.m_Flags.m_General & CompositionFlags.General.Node) > (CompositionFlags.General) 0;
        bool flag2 = (compositionMeshData.m_Flags.m_General & CompositionFlags.General.Roundabout) > (CompositionFlags.General) 0;
        int vertexCount = 0;
        int num1 = 0;
        int indexCount = 0;
        float middleOffset = compositionMeshData.m_MiddleOffset;
        for (int index = 0; index < materials.Length; ++index)
        {
          MeshMaterial material = materials[index] with
          {
            m_StartVertex = 0,
            m_StartIndex = 0,
            m_VertexCount = 0,
            m_IndexCount = 0
          };
          materials[index] = material;
        }
        for (int index1 = 0; index1 < pieces.Length; ++index1)
        {
          NetCompositionPiece piece = pieces[index1];
          bool flag3 = (piece.m_SectionFlags & NetSectionFlags.Invert) != 0;
          bool flag4 = (piece.m_SectionFlags & NetSectionFlags.FlipMesh) != 0;
          bool flag5 = (piece.m_PieceFlags & NetPieceFlags.PreserveShape) != 0;
          bool flag6 = flag1 && !flag2 && !flag5;
          bool flag7 = (piece.m_PieceFlags & NetPieceFlags.SkipBottomHalf) != 0;
          if (this.m_MeshVertices.HasBuffer(piece.m_Piece))
          {
            DynamicBuffer<MeshVertex> meshVertex = this.m_MeshVertices[piece.m_Piece];
            DynamicBuffer<MeshIndex> meshIndex = this.m_MeshIndices[piece.m_Piece];
            DynamicBuffer<MeshMaterial> meshMaterial = this.m_MeshMaterials[piece.m_Piece];
            for (int index2 = 0; index2 < meshMaterial.Length; ++index2)
            {
              MeshMaterial pieceMaterial = meshMaterial[index2];
              int material1 = this.GetMaterial(materials, pieceMaterial);
              MeshMaterial material2 = materials[material1];
              material2.m_VertexCount += pieceMaterial.m_VertexCount;
              material2.m_IndexCount += pieceMaterial.m_IndexCount;
              num1 = math.max(num1, pieceMaterial.m_VertexCount);
              if (flag6)
              {
                for (int index3 = 0; index3 < pieceMaterial.m_VertexCount; ++index3)
                  material2.m_VertexCount -= math.select(1, 0, (double) meshVertex[pieceMaterial.m_StartVertex + index3].m_Vertex.z >= -0.0099999997764825821);
                int3 int3_1 = pieceMaterial.m_StartIndex + math.select(new int3(0, 1, 2), new int3(2, 1, 0), flag3 != flag4);
                for (int index4 = 0; index4 < pieceMaterial.m_IndexCount; index4 += 3)
                {
                  int3 int3_2 = index4 + int3_1;
                  float3 float3;
                  float3.x = meshVertex[meshIndex[int3_2.x].m_Index].m_Vertex.z;
                  float3.y = meshVertex[meshIndex[int3_2.y].m_Index].m_Vertex.z;
                  float3.z = meshVertex[meshIndex[int3_2.z].m_Index].m_Vertex.z;
                  material2.m_IndexCount -= math.select(3, 0, math.all(float3 >= -0.01f));
                }
              }
              else if (flag7)
              {
                for (int index5 = 0; index5 < pieceMaterial.m_VertexCount; ++index5)
                  material2.m_VertexCount -= math.select(1, 0, (double) meshVertex[pieceMaterial.m_StartVertex + index5].m_Vertex.z <= 0.0099999997764825821);
                int3 int3_3 = pieceMaterial.m_StartIndex + math.select(new int3(0, 1, 2), new int3(2, 1, 0), flag3 != flag4);
                for (int index6 = 0; index6 < pieceMaterial.m_IndexCount; index6 += 3)
                {
                  int3 int3_4 = index6 + int3_3;
                  float3 float3;
                  float3.x = meshVertex[meshIndex[int3_4.x].m_Index].m_Vertex.z;
                  float3.y = meshVertex[meshIndex[int3_4.y].m_Index].m_Vertex.z;
                  float3.z = meshVertex[meshIndex[int3_4.z].m_Index].m_Vertex.z;
                  material2.m_IndexCount -= math.select(3, 0, math.all(float3 >= -0.01f));
                }
              }
              materials[material1] = material2;
            }
          }
        }
        for (int index = 0; index < materials.Length; ++index)
        {
          MeshMaterial material = materials[index] with
          {
            m_StartVertex = vertexCount,
            m_StartIndex = indexCount
          };
          vertexCount += material.m_VertexCount;
          indexCount += material.m_IndexCount;
          materials[index] = material;
        }
        NativeArray<VertexAttributeDescriptor> nativeArray1 = new NativeArray<VertexAttributeDescriptor>(5, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        BatchMeshHelpers.GenerateBatchMeshJob.SetupGeneratedMeshAttributes(nativeArray1);
        bool flag8 = indexCount >= 65536;
        meshData.SetVertexBufferParams(vertexCount, nativeArray1);
        meshData.SetIndexBufferParams(indexCount, flag8 ? IndexFormat.UInt32 : IndexFormat.UInt16);
        nativeArray1.Dispose();
        meshData.subMeshCount = materials.Length;
        for (int index = 0; index < materials.Length; ++index)
        {
          MeshMaterial material = materials[index];
          meshData.SetSubMesh(index, new SubMeshDescriptor()
          {
            firstVertex = material.m_StartVertex,
            indexStart = material.m_StartIndex,
            vertexCount = material.m_VertexCount,
            indexCount = material.m_IndexCount,
            topology = MeshTopology.Triangles
          }, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
          material.m_VertexCount = 0;
          material.m_IndexCount = 0;
          materials[index] = material;
        }
        NativeArray<BatchMeshHelpers.GenerateBatchMeshJob.VertexData> vertexData = meshData.GetVertexData<BatchMeshHelpers.GenerateBatchMeshJob.VertexData>();
        NativeArray<uint> nativeArray2 = new NativeArray<uint>();
        NativeArray<ushort> nativeArray3 = new NativeArray<ushort>();
        if (flag8)
          nativeArray2 = meshData.GetIndexData<uint>();
        else
          nativeArray3 = meshData.GetIndexData<ushort>();
        NativeArray<int> nativeArray4 = new NativeArray<int>(num1, Allocator.Temp);
        NativeArray<Bounds1> heightBounds1 = new NativeArray<Bounds1>();
        float x1 = compositionMeshData.m_Width * 0.5f + middleOffset;
        float x2 = compositionMeshData.m_Width * 0.5f - middleOffset;
        for (int index7 = 0; index7 < pieces.Length; ++index7)
        {
          NetCompositionPiece piece = pieces[index7];
          if (this.m_MeshVertices.HasBuffer(piece.m_Piece))
          {
            bool x3 = (piece.m_SectionFlags & NetSectionFlags.Invert) != 0;
            bool z1 = (piece.m_SectionFlags & NetSectionFlags.FlipMesh) != 0;
            bool flag9 = (piece.m_SectionFlags & NetSectionFlags.Median) != 0;
            bool flag10 = (piece.m_SectionFlags & NetSectionFlags.Right) != 0;
            bool flag11 = (piece.m_PieceFlags & NetPieceFlags.PreserveShape) != 0;
            bool flag12 = (piece.m_PieceFlags & NetPieceFlags.DisableTiling) != 0;
            bool flag13 = (piece.m_PieceFlags & NetPieceFlags.LowerBottomToTerrain) != 0;
            bool flag14 = (piece.m_PieceFlags & NetPieceFlags.RaiseTopToTerrain) != 0;
            bool flag15 = (piece.m_PieceFlags & NetPieceFlags.SmoothTopNormal) != 0;
            bool flag16 = flag1 && !flag2 && !flag11;
            bool flag17 = (piece.m_PieceFlags & NetPieceFlags.SkipBottomHalf) != 0;
            bool c1 = (double) piece.m_Size.x == 0.0;
            float4 float4 = math.select(new float4(-1f, 1f, -1f, 1f), new float4(1f, 1f, 1f, -1f), new bool4(x3, false, z1, x3 != z1));
            float3 offset = piece.m_Offset;
            float2 float2_1 = 1f / new float2(compositionMeshData.m_Width, piece.m_Size.z * 0.5f);
            float2 float2_2 = 1f / new float2(x1, piece.m_Size.z * 0.5f);
            float2 float2_3 = 1f / new float2(x2, piece.m_Size.z * 0.5f);
            float2 float2_4 = new float2(0.5f, 1f);
            float2 float2_5 = new float2((float) (1.0 - (double) middleOffset / (double) x1), 1f);
            float2 float2_6 = new float2(-middleOffset / x2, 1f);
            float num2 = (float) ((double) piece.m_Offset.x / (double) compositionMeshData.m_Width + 0.5);
            float num3 = (float) (1.0 + ((double) piece.m_Offset.x - (double) middleOffset) / (double) x1);
            float num4 = (piece.m_Offset.x - middleOffset) / x2;
            if (flag1 & flag11)
            {
              float num5 = 0.5f * piece.m_Size.z / compositionMeshData.m_Width;
              float2_1.y *= num5;
              float2_4.y *= num5;
              num2 = 0.5f;
            }
            else if (flag2)
            {
              num3 = 0.0f;
              num4 = 1f;
            }
            DynamicBuffer<MeshVertex> meshVertex = this.m_MeshVertices[piece.m_Piece];
            DynamicBuffer<MeshNormal> meshNormal = this.m_MeshNormals[piece.m_Piece];
            DynamicBuffer<MeshTangent> meshTangent = this.m_MeshTangents[piece.m_Piece];
            DynamicBuffer<MeshUV0> meshUv0 = this.m_MeshUV0s[piece.m_Piece];
            DynamicBuffer<MeshIndex> meshIndex = this.m_MeshIndices[piece.m_Piece];
            DynamicBuffer<MeshMaterial> meshMaterial = this.m_MeshMaterials[piece.m_Piece];
            if (flag13 | flag14 | flag15)
            {
              if (!heightBounds1.IsCreated)
                heightBounds1 = new NativeArray<Bounds1>(257, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
              this.InitializeHeightBounds(heightBounds1, piece, meshVertex, meshIndex);
            }
            for (int index8 = 0; index8 < meshMaterial.Length; ++index8)
            {
              MeshMaterial pieceMaterial = meshMaterial[index8];
              int material3 = this.GetMaterial(materials, pieceMaterial);
              MeshMaterial material4 = materials[material3];
              for (int index9 = 0; index9 < pieceMaterial.m_VertexCount; ++index9)
              {
                float z2 = meshVertex[pieceMaterial.m_StartVertex + index9].m_Vertex.z;
                nativeArray4[index9] = !((!flag16 | (double) z2 >= -0.0099999997764825821) & (!flag17 | (double) z2 <= 0.0099999997764825821)) ? -1 : material4.m_StartVertex + material4.m_VertexCount++;
              }
              int3 int3_5 = pieceMaterial.m_StartIndex + math.select(new int3(0, 1, 2), new int3(2, 1, 0), x3 != z1);
              float2 float2_7 = (float2) 0.0f;
              float x4 = 0.0f;
              bool c2 = !flag12;
              for (int index10 = 0; index10 < pieceMaterial.m_IndexCount; index10 += 3)
              {
                int3 int3_6 = index10 + int3_5;
                int3_6.x = meshIndex[int3_6.x].m_Index;
                int3_6.y = meshIndex[int3_6.y].m_Index;
                int3_6.z = meshIndex[int3_6.z].m_Index;
                int3 int3_7 = int3_6 - pieceMaterial.m_StartVertex;
                int3_7.x = nativeArray4[int3_7.x];
                int3_7.y = nativeArray4[int3_7.y];
                int3_7.z = nativeArray4[int3_7.z];
                if (math.all(int3_7 >= 0))
                {
                  if (flag8)
                  {
                    nativeArray2[material4.m_StartIndex + material4.m_IndexCount++] = (uint) int3_7.x;
                    nativeArray2[material4.m_StartIndex + material4.m_IndexCount++] = (uint) int3_7.y;
                    nativeArray2[material4.m_StartIndex + material4.m_IndexCount++] = (uint) int3_7.z;
                  }
                  else
                  {
                    nativeArray3[material4.m_StartIndex + material4.m_IndexCount++] = (ushort) int3_7.x;
                    nativeArray3[material4.m_StartIndex + material4.m_IndexCount++] = (ushort) int3_7.y;
                    nativeArray3[material4.m_StartIndex + material4.m_IndexCount++] = (ushort) int3_7.z;
                  }
                  float3 float3_1 = new float3(meshUv0[int3_6.x].m_Uv.y, meshUv0[int3_6.y].m_Uv.y, meshUv0[int3_6.z].m_Uv.y);
                  if (c2 & math.all(float3_1 >= 0.0f))
                  {
                    float3 float3_2 = new float3(meshVertex[int3_6.x].m_Vertex.z, meshVertex[int3_6.y].m_Vertex.z, meshVertex[int3_6.z].m_Vertex.z);
                    float2_7 += new float2(math.csum(math.abs(float3_1.yzx - float3_1)), math.csum(math.abs(float3_2.yzx - float3_2)));
                  }
                  else
                    x4 = math.max(math.max(x4, float3_1.x), math.max(float3_1.y, float3_1.z));
                }
              }
              float b = float2_7.x / float2_7.y;
              float num6 = math.select(math.ceil(x4), 0.0f, c2);
              for (int index11 = 0; index11 < pieceMaterial.m_VertexCount; ++index11)
              {
                int index12 = nativeArray4[index11];
                if (index12 != -1)
                {
                  int index13 = pieceMaterial.m_StartVertex + index11;
                  float3 float3 = meshVertex[index13].m_Vertex;
                  float3 a = meshNormal[index13].m_Normal;
                  float4 tangent = meshTangent[index13].m_Tangent;
                  float4 v = new float4(meshUv0[index13].m_Uv, 0.0f, 0.0f);
                  int4 int4 = new int4();
                  if (flag13)
                  {
                    Bounds1 heightBounds2 = this.GetHeightBounds(heightBounds1, piece, float3.z);
                    int4.z = math.select(int4.z, 1, (double) float3.y <= (double) heightBounds2.min + 0.0099999997764825821);
                  }
                  if (flag14)
                  {
                    Bounds1 heightBounds3 = this.GetHeightBounds(heightBounds1, piece, float3.z);
                    int4.z = math.select(int4.z, 2, (double) float3.y >= (double) heightBounds3.max - 0.0099999997764825821);
                  }
                  if (flag15)
                  {
                    Bounds1 heightBounds4 = this.GetHeightBounds(heightBounds1, piece, float3.z);
                    a = math.select(a, new float3(0.0f, 1f, 0.0f), (double) float3.y >= (double) heightBounds4.max - 0.0099999997764825821);
                  }
                  float3 = float3 * float4.xyz + offset;
                  float3 n = a * float4.xyz;
                  float4 t = tangent * float4;
                  v.y = math.select(v.y - num6, b, c2 & (double) v.y >= 0.0);
                  if (flag1)
                  {
                    if (flag11)
                    {
                      int4.xy = new int2(6, 7);
                      v.z = num2;
                      float3.xz = float3.xz * float2_1 + float2_4;
                      v.w = float3.z * 20f;
                    }
                    else if (flag9)
                    {
                      float x5 = float3.x - piece.m_Offset.x;
                      float3.x = math.select(float3.x, piece.m_Offset.x, c1);
                      if ((double) math.abs(x5) < 0.0099999997764825821)
                      {
                        if (flag2)
                        {
                          int4.xy = math.select(new int2(4, 2), new int2(5, 3), (double) float3.z >= 0.0);
                          float3.x = 0.0f;
                          float3.z = float3.z * float2_1.y + float2_4.y;
                          v.w = float3.z * 40f;
                        }
                        else
                        {
                          int4.x = 4;
                          float3.x = 0.0f;
                          float3.z = float3.z * float2_1.y + float2_4.y;
                          v.w = float3.z * 20f;
                        }
                      }
                      else if ((double) x5 > 0.0)
                      {
                        if (flag2)
                        {
                          int4.xyw = math.select(new int3(4, 2, 4), new int3(5, 3, 132), (double) float3.z >= 0.0);
                          v.z = num4;
                          float3.xz = float3.xz * float2_3 + float2_6;
                          v.w = float3.z * 40f;
                          float3.z -= (float) (int4.y - 2);
                        }
                        else
                        {
                          int4.xyw = new int3(1, 3, 4);
                          v.z = num4;
                          float3.xz = float3.xz * float2_3 + float2_6;
                          v.w = float3.z * 20f;
                        }
                      }
                      else if (flag2)
                      {
                        int4.xyw = math.select(new int3(0, 4, 2), new int3(1, 5, 130), (double) float3.z >= 0.0);
                        v.z = num3;
                        float3.xz = float3.xz * float2_2 + float2_5;
                        v.w = float3.z * 40f;
                        float3.z -= (float) int4.x;
                      }
                      else
                      {
                        int4.yw = new int2(2, 2);
                        v.z = num3;
                        float3.xz = float3.xz * float2_2 + float2_5;
                        v.w = float3.z * 20f;
                      }
                    }
                    else if (flag10)
                    {
                      if (flag2)
                      {
                        int4.xyw = math.select(new int3(4, 2, 4), new int3(5, 3, 132), (double) float3.z >= 0.0);
                        v.z = num4;
                        float3.xz = float3.xz * float2_3 + float2_6;
                        v.w = float3.z * 40f;
                        float3.z -= (float) (int4.y - 2);
                      }
                      else
                      {
                        int4.xyw = new int3(1, 3, 4);
                        v.z = num4;
                        float3.xz = float3.xz * float2_3 + float2_6;
                        v.w = float3.z * 20f;
                      }
                    }
                    else if (flag2)
                    {
                      int4.xyw = math.select(new int3(0, 4, 2), new int3(1, 5, 130), (double) float3.z >= 0.0);
                      v.z = num3;
                      float3.xz = float3.xz * float2_2 + float2_5;
                      v.w = float3.z * 40f;
                      float3.z -= (float) int4.x;
                    }
                    else
                    {
                      int4.yw = new int2(2, 2);
                      v.z = num3;
                      float3.xz = float3.xz * float2_2 + float2_5;
                      v.w = float3.z * 20f;
                    }
                    float3.xz = math.saturate(float3.xz);
                  }
                  else
                  {
                    int4.xy = math.select(new int2(0, 2), new int2(1, 3), (double) float3.z >= 0.0);
                    v.z = num2;
                    float3.xz = float3.xz * float2_1 + float2_4;
                    v.w = float3.z * 0.5f;
                    float3.z -= (float) int4.x;
                    float3.xz = math.saturate(float3.xz);
                  }
                  Color32 color32 = new Color32((byte) int4.x, (byte) int4.y, (byte) int4.z, (byte) int4.w);
                  vertexData[index12] = new BatchMeshHelpers.GenerateBatchMeshJob.VertexData()
                  {
                    m_Position = float3,
                    m_Color = color32,
                    m_Normal = MathUtils.NormalToOctahedral(n),
                    m_Tangent = MathUtils.TangentToOctahedral(t),
                    m_UV0 = new half4(v)
                  };
                }
              }
              materials[material3] = material4;
            }
          }
        }
        if (nativeArray4.IsCreated)
          nativeArray4.Dispose();
        if (!heightBounds1.IsCreated)
          return;
        heightBounds1.Dispose();
      }

      private static void SetupGeneratedMeshAttributes(NativeArray<VertexAttributeDescriptor> attrs)
      {
        attrs[0] = new VertexAttributeDescriptor();
        attrs[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.SNorm16, 2);
        attrs[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, dimension: 1);
        attrs[3] = new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4);
        attrs[4] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 4);
      }

      private void InitializeHeightBounds(
        NativeArray<Bounds1> heightBounds,
        NetCompositionPiece compositionPiece,
        DynamicBuffer<MeshVertex> pieceVertices,
        DynamicBuffer<MeshIndex> pieceIndices)
      {
        int b = heightBounds.Length - 1;
        float num1 = (float) b / compositionPiece.m_Size.z;
        for (int index = 0; index <= b; ++index)
          heightBounds[index] = new Bounds1(float.MaxValue, float.MinValue);
        for (int index = 0; index < pieceIndices.Length; index += 3)
        {
          float3 vertex1 = pieceVertices[pieceIndices[index].m_Index].m_Vertex;
          float3 vertex2 = pieceVertices[pieceIndices[index + 1].m_Index].m_Vertex;
          float3 vertex3 = pieceVertices[pieceIndices[index + 2].m_Index].m_Vertex;
          int num2 = math.clamp(Mathf.RoundToInt(vertex1.z * num1) + (b >> 1), 0, b);
          int num3 = math.clamp(Mathf.RoundToInt(vertex2.z * num1) + (b >> 1), 0, b);
          int num4 = math.clamp(Mathf.RoundToInt(vertex3.z * num1) + (b >> 1), 0, b);
          this.AddHeightBounds(heightBounds, vertex1, vertex2, num2, num3);
          this.AddHeightBounds(heightBounds, vertex2, vertex3, num3, num4);
          this.AddHeightBounds(heightBounds, vertex3, vertex1, num4, num2);
        }
      }

      private void AddHeightBounds(
        NativeArray<Bounds1> heightBounds,
        float3 aVertex,
        float3 bVertex,
        int aIndex,
        int bIndex)
      {
        if (aIndex <= bIndex)
        {
          float num1 = 1f / (float) (bIndex - aIndex + 1);
          for (int index = aIndex; index <= bIndex; ++index)
          {
            float num2 = math.lerp(aVertex.y, bVertex.y, (float) (index - aIndex) * num1);
            heightBounds[index] |= num2;
          }
        }
        else
        {
          float num3 = 1f / (float) (aIndex - bIndex + 1);
          for (int index = bIndex; index <= aIndex; ++index)
          {
            float num4 = math.lerp(bVertex.y, aVertex.y, (float) (index - bIndex) * num3);
            heightBounds[index] |= num4;
          }
        }
      }

      private Bounds1 GetHeightBounds(
        NativeArray<Bounds1> heightBounds,
        NetCompositionPiece compositionPiece,
        float z)
      {
        int b = heightBounds.Length - 1;
        float num = (float) b / compositionPiece.m_Size.z;
        int index = math.clamp(Mathf.RoundToInt(z * num) + (b >> 1), 0, b);
        return heightBounds[index];
      }

      private struct BasePoint
      {
        public float2 m_Position;
        public float2 m_Direction;
        public float2 m_PrevPos;
        public float m_Distance;
      }

      private struct BaseLine
      {
        public int m_StartIndex;
        public int m_EndIndex;
      }

      private struct VertexData
      {
        public float3 m_Position;
        public uint m_Normal;
        public uint m_Tangent;
        public Color32 m_Color;
        public half4 m_UV0;
      }
    }
  }
}
