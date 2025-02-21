// Decompiled with JetBrains decompiler
// Type: Game.Objects.RaycastJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Rendering;
using Game.Vehicles;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  public static class RaycastJobs
  {
    private static bool CheckMeshIntersect(
      Line3.Segment localLine,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      int2 elementIndex,
      ref RaycastHit hit)
    {
      bool flag = false;
      for (int index = 0; index < indices.Length; index += 3)
      {
        Triangle3 triangle = new Triangle3(vertices[indices[index].m_Index].m_Vertex, vertices[indices[index + 1].m_Index].m_Vertex, vertices[indices[index + 2].m_Index].m_Vertex);
        float3 t;
        if (MathUtils.Intersect(triangle, localLine, out t) && (double) t.z < (double) hit.m_NormalizedDistance)
        {
          hit.m_HitDirection = MathUtils.NormalCW(triangle);
          hit.m_NormalizedDistance = t.z;
          hit.m_CellIndex = elementIndex;
          flag = true;
        }
      }
      return flag;
    }

    private static unsafe bool CheckMeshIntersect(
      Line3.Segment localLine,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      DynamicBuffer<MeshNode> nodes,
      int2 elementIndex,
      ref RaycastHit hit)
    {
      bool flag = false;
      int* numPtr = stackalloc int[128];
      int a1 = 0;
      if (nodes.Length != 0)
        numPtr[a1++] = 0;
      while (--a1 >= 0)
      {
        int index = numPtr[a1];
        MeshNode node = nodes[index];
        if (MathUtils.Intersect(node.m_Bounds, localLine, out float2 _))
        {
          for (int x = node.m_IndexRange.x; x < node.m_IndexRange.y; x += 3)
          {
            Triangle3 triangle = new Triangle3(vertices[indices[x].m_Index].m_Vertex, vertices[indices[x + 1].m_Index].m_Vertex, vertices[indices[x + 2].m_Index].m_Vertex);
            float3 t;
            if (MathUtils.Intersect(triangle, localLine, out t) && (double) t.z < (double) hit.m_NormalizedDistance)
            {
              hit.m_HitDirection = MathUtils.NormalCW(triangle);
              hit.m_NormalizedDistance = t.z;
              hit.m_CellIndex = elementIndex;
              flag = true;
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
      return flag;
    }

    private static unsafe bool CheckMeshIntersect(
      Line3.Segment localLine,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      DynamicBuffer<MeshNode> nodes,
      DynamicBuffer<ProceduralBone> prefabBones,
      int2 elementIndex,
      ref RaycastHit hit)
    {
      bool flag = false;
      int* numPtr = stackalloc int[128];
      for (int index1 = 0; index1 < prefabBones.Length; ++index1)
      {
        int a1 = 0;
        if (math.any(MathUtils.Size(nodes[index1].m_Bounds) > 0.0f))
          numPtr[a1++] = index1;
        while (--a1 >= 0)
        {
          int index2 = numPtr[a1];
          MeshNode node = nodes[index2];
          if (MathUtils.Intersect(node.m_Bounds, localLine, out float2 _))
          {
            for (int x = node.m_IndexRange.x; x < node.m_IndexRange.y; x += 3)
            {
              Triangle3 triangle = new Triangle3(vertices[indices[x].m_Index].m_Vertex, vertices[indices[x + 1].m_Index].m_Vertex, vertices[indices[x + 2].m_Index].m_Vertex);
              float3 t;
              if (MathUtils.Intersect(triangle, localLine, out t) && (double) t.z < (double) hit.m_NormalizedDistance)
              {
                hit.m_HitDirection = MathUtils.NormalCW(triangle);
                hit.m_NormalizedDistance = t.z;
                hit.m_CellIndex = elementIndex;
                flag = true;
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
      }
      return flag;
    }

    private static unsafe bool CheckMeshIntersect(
      Line3.Segment localLine,
      DynamicBuffer<MeshVertex> vertices,
      DynamicBuffer<MeshIndex> indices,
      DynamicBuffer<MeshNode> nodes,
      DynamicBuffer<ProceduralBone> prefabBones,
      DynamicBuffer<Bone> bones,
      Skeleton skeleton,
      int2 elementIndex,
      ref RaycastHit hit)
    {
      bool flag = false;
      int* numPtr = stackalloc int[128];
      for (int index1 = 0; index1 < prefabBones.Length; ++index1)
      {
        int a1 = 0;
        Line3.Segment line = new Line3.Segment();
        ProceduralBone prefabBone1 = prefabBones[index1];
        if (math.any(MathUtils.Size(nodes[prefabBone1.m_BindIndex].m_Bounds) > 0.0f))
        {
          numPtr[a1++] = prefabBone1.m_BindIndex;
          Bone bone1 = bones[skeleton.m_BoneOffset + index1];
          float4x4 float4x4 = float4x4.TRS(bone1.m_Position, bone1.m_Rotation, bone1.m_Scale);
          ProceduralBone prefabBone2;
          for (int parentIndex = prefabBone1.m_ParentIndex; parentIndex >= 0; parentIndex = prefabBone2.m_ParentIndex)
          {
            Bone bone2 = bones[skeleton.m_BoneOffset + parentIndex];
            prefabBone2 = prefabBones[parentIndex];
            float4x4 = math.mul(float4x4.TRS(bone2.m_Position, bone2.m_Rotation, bone2.m_Scale), float4x4);
          }
          float4x4 a2 = math.inverse(math.mul(float4x4, prefabBone1.m_BindPose));
          ref Line3.Segment local1 = ref line;
          float4 float4 = math.mul(a2, new float4(localLine.a, 1f));
          float3 xyz1 = float4.xyz;
          local1.a = xyz1;
          ref Line3.Segment local2 = ref line;
          float4 = math.mul(a2, new float4(localLine.b, 1f));
          float3 xyz2 = float4.xyz;
          local2.b = xyz2;
        }
        while (--a1 >= 0)
        {
          int index2 = numPtr[a1];
          MeshNode node = nodes[index2];
          if (MathUtils.Intersect(node.m_Bounds, line, out float2 _))
          {
            for (int x = node.m_IndexRange.x; x < node.m_IndexRange.y; x += 3)
            {
              Triangle3 triangle = new Triangle3(vertices[indices[x].m_Index].m_Vertex, vertices[indices[x + 1].m_Index].m_Vertex, vertices[indices[x + 2].m_Index].m_Vertex);
              float3 t;
              if (MathUtils.Intersect(triangle, line, out t) && (double) t.z < (double) hit.m_NormalizedDistance)
              {
                hit.m_HitDirection = MathUtils.NormalCW(triangle);
                hit.m_NormalizedDistance = t.z;
                hit.m_CellIndex = elementIndex;
                flag = true;
              }
            }
            numPtr[a1] = node.m_SubNodes1.x;
            int a3 = math.select(a1, a1 + 1, node.m_SubNodes1.x != -1);
            numPtr[a3] = node.m_SubNodes1.y;
            int a4 = math.select(a3, a3 + 1, node.m_SubNodes1.y != -1);
            numPtr[a4] = node.m_SubNodes1.z;
            int a5 = math.select(a4, a4 + 1, node.m_SubNodes1.z != -1);
            numPtr[a5] = node.m_SubNodes1.w;
            int a6 = math.select(a5, a5 + 1, node.m_SubNodes1.w != -1);
            numPtr[a6] = node.m_SubNodes2.x;
            int a7 = math.select(a6, a6 + 1, node.m_SubNodes2.x != -1);
            numPtr[a7] = node.m_SubNodes2.y;
            int a8 = math.select(a7, a7 + 1, node.m_SubNodes2.y != -1);
            numPtr[a8] = node.m_SubNodes2.z;
            int a9 = math.select(a8, a8 + 1, node.m_SubNodes2.z != -1);
            numPtr[a9] = node.m_SubNodes2.w;
            a1 = math.select(a9, a9 + 1, node.m_SubNodes2.w != -1);
          }
        }
      }
      return flag;
    }

    [BurstCompile]
    public struct RaycastStaticObjectsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeArray<RaycastSystem.EntityResult> m_Objects;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<NetObject> m_NetObjectData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<Secondary> m_SecondaryData;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructionData;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> m_PrefabGrowthScaleData;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> m_PrefabQuantityObjectData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Overridden> m_OverriddenData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_LotAreaData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<Skeleton> m_Skeletons;
      [ReadOnly]
      public BufferLookup<Bone> m_Bones;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public ComponentLookup<ImpostorData> m_PrefabImpostorData;
      [ReadOnly]
      public ComponentLookup<SharedMeshData> m_PrefabSharedMeshData;
      [ReadOnly]
      public BufferLookup<SubMesh> m_Meshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<LodMesh> m_Lods;
      [ReadOnly]
      public BufferLookup<MeshVertex> m_Vertices;
      [ReadOnly]
      public BufferLookup<MeshIndex> m_Indices;
      [ReadOnly]
      public BufferLookup<MeshNode> m_Nodes;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.EntityResult entityResult = this.m_Objects[index];
        // ISSUE: reference to a compiler-generated field
        RaycastInput input = this.m_Input[entityResult.m_RaycastIndex];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((input.m_TypeMask & (TypeMask.StaticObjects | TypeMask.Net)) == TypeMask.None || this.m_OverriddenData.HasComponent(entityResult.m_Entity) || !this.IsNearCamera(entityResult.m_Entity) || (input.m_Flags & RaycastFlags.IgnoreSecondary) != (RaycastFlags) 0 && this.m_SecondaryData.HasComponent(entityResult.m_Entity))
          return;
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entityResult.m_Entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entityResult.m_Entity];
        Line3.Segment segment1 = input.m_Line + input.m_Offset;
        bool flag = false;
        ObjectGeometryData componentData1;
        if (this.m_PrefabObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
        {
          if ((componentData1.m_Flags & GeometryFlags.Marker) != GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if ((input.m_Flags & RaycastFlags.OutsideConnections) != (RaycastFlags) 0 && this.m_OutsideConnectionData.HasComponent(entityResult.m_Entity))
            {
              if ((input.m_TypeMask & TypeMask.StaticObjects) != TypeMask.None)
                input.m_Flags |= RaycastFlags.SubElements;
            }
            else if ((input.m_Flags & RaycastFlags.Markers) == (RaycastFlags) 0)
              return;
          }
          else
          {
            Elevation componentData2;
            // ISSUE: reference to a compiler-generated field
            if (((!this.m_ElevationData.TryGetComponent(entityResult.m_Entity, out componentData2) ? ObjectUtils.GetCollisionMask(componentData1, true) : ObjectUtils.GetCollisionMask(componentData1, componentData2, true)) & input.m_CollisionMask) == (CollisionMask) 0)
            {
              if ((input.m_CollisionMask & CollisionMask.Underground) == (CollisionMask) 0 || (input.m_Flags & RaycastFlags.PartialSurface) == (RaycastFlags) 0 || (double) componentData1.m_Bounds.min.y >= 0.0)
                return;
              flag = true;
            }
          }
          quaternion q = math.inverse(transform.m_Rotation);
          Line3.Segment segment2 = new Line3.Segment();
          segment2.a = math.mul(q, segment1.a - transform.m_Position);
          segment2.b = math.mul(q, segment1.b - transform.m_Position);
          RaycastResult result = new RaycastResult();
          // ISSUE: reference to a compiler-generated field
          result.m_Owner = entityResult.m_Entity;
          // ISSUE: reference to a compiler-generated field
          result.m_Hit.m_HitEntity = entityResult.m_Entity;
          result.m_Hit.m_Position = transform.m_Position;
          result.m_Hit.m_NormalizedDistance = 1f;
          Stack componentData3;
          StackData componentData4;
          // ISSUE: reference to a compiler-generated field
          Bounds3 bounds = !this.m_StackData.TryGetComponent(entityResult.m_Entity, out componentData3) || !this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData4) ? ObjectUtils.GetBounds(componentData1) : ObjectUtils.GetBounds(componentData3, componentData1, componentData4);
          float2 t1;
          if (MathUtils.Intersect(bounds, segment2, out t1) && (double) t1.x < (double) result.m_Hit.m_NormalizedDistance)
          {
            float3 float3 = MathUtils.Position(segment1, t1.x);
            result.m_Hit.m_HitPosition = float3;
            result.m_Hit.m_NormalizedDistance = t1.x;
            result.m_Hit.m_CellIndex = new int2(-1, -1);
            float num = math.cmax(MathUtils.Size(bounds));
            float2 float2 = math.saturate(new float2(t1.x - num, t1.y + num));
            float t2;
            if (flag && MathUtils.Intersect(segment2.y, 0.0f, out t2))
            {
              if ((double) segment2.b.y > (double) segment2.a.y)
                float2.y = math.min(float2.y, t2);
              else
                float2.x = math.max(float2.x, t2);
            }
            if ((double) float2.y > (double) float2.x)
            {
              Line3.Segment localLine = MathUtils.Cut(segment2, float2);
              // ISSUE: reference to a compiler-generated field
              if (!this.RaycastMeshes(in input, ref result, entityResult.m_Entity, prefabRef, segment1, localLine, transform.m_Rotation, float2))
                result.m_Hit.m_NormalizedDistance = 1f;
            }
          }
          if ((componentData1.m_Flags & GeometryFlags.HasLot) != GeometryFlags.None && (input.m_Flags & RaycastFlags.BuildingLots) != (RaycastFlags) 0 && !flag)
            this.RaycastLot(ref result, componentData1, segment1, segment2);
          if ((double) result.m_Hit.m_NormalizedDistance >= 1.0 || !this.ValidateResult(in input, ref result))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Results.Accumulate(entityResult.m_RaycastIndex, result);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((input.m_Flags & RaycastFlags.EditorContainers) == (RaycastFlags) 0 && this.m_EditorContainerData.HasComponent(entityResult.m_Entity))
            return;
          float t;
          float num = MathUtils.Distance(segment1, transform.m_Position, out t);
          if ((double) num >= 1.0)
            return;
          RaycastResult result = new RaycastResult();
          // ISSUE: reference to a compiler-generated field
          result.m_Owner = entityResult.m_Entity;
          // ISSUE: reference to a compiler-generated field
          result.m_Hit.m_HitEntity = entityResult.m_Entity;
          result.m_Hit.m_Position = transform.m_Position;
          result.m_Hit.m_HitPosition = MathUtils.Position(segment1, t);
          result.m_Hit.m_NormalizedDistance = t - (1f - num) / math.max(1f, MathUtils.Length(segment1));
          result.m_Hit.m_CellIndex = new int2(0, -1);
          if (!this.ValidateResult(in input, ref result))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Results.Accumulate(entityResult.m_RaycastIndex, result);
        }
      }

      private bool IsNearCamera(Entity entity)
      {
        CullingInfo componentData;
        return this.m_CullingInfoData.TryGetComponent(entity, out componentData) && componentData.m_CullingIndex != 0 && (this.m_CullingData[componentData.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) > (PreCullingFlags) 0;
      }

      private bool ValidateResult(in RaycastInput input, ref RaycastResult result)
      {
        TypeMask typeMask1 = TypeMask.StaticObjects;
        float3 position = result.m_Hit.m_Position;
        Entity owner1 = Entity.Null;
        TypeMask typeMask2 = TypeMask.None;
        while (true)
        {
          Entity owner2;
          do
          {
            if ((input.m_Flags & RaycastFlags.UpgradeIsMain) != (RaycastFlags) 0)
            {
              if (!this.m_ServiceUpgradeData.HasComponent(result.m_Owner))
              {
                DynamicBuffer<InstalledUpgrade> bufferData;
                if (this.m_InstalledUpgrades.TryGetBuffer(result.m_Owner, out bufferData) && bufferData.Length != 0)
                {
                  owner1 = Entity.Null;
                  typeMask2 = TypeMask.None;
                  typeMask1 = TypeMask.StaticObjects;
                  result.m_Owner = bufferData[0].m_Upgrade;
                  goto label_17;
                }
              }
              else
                goto label_17;
            }
            else if ((input.m_Flags & RaycastFlags.SubBuildings) != (RaycastFlags) 0 && this.m_BuildingData.HasComponent(result.m_Owner) && this.m_ServiceUpgradeData.HasComponent(result.m_Owner))
              goto label_17;
            if (this.m_OwnerData.HasComponent(result.m_Owner))
            {
              owner2 = this.m_OwnerData[result.m_Owner].m_Owner;
              owner1 = result.m_Owner;
              typeMask2 = typeMask1;
              if (this.m_NodeData.HasComponent(owner2))
              {
                typeMask1 = TypeMask.Net;
                result.m_Owner = owner2;
                position = this.m_NodeData[owner2].m_Position;
                if ((input.m_TypeMask & (TypeMask.StaticObjects | TypeMask.Net)) == TypeMask.Net)
                {
                  typeMask2 = TypeMask.None;
                  result.m_Hit.m_Position = position;
                  goto label_17;
                }
              }
              else if (this.m_EdgeData.HasComponent(owner2))
              {
                typeMask1 = TypeMask.Net;
                result.m_Owner = owner2;
                if ((input.m_TypeMask & (TypeMask.StaticObjects | TypeMask.Net)) == TypeMask.Net)
                {
                  typeMask2 = TypeMask.None;
                  goto label_17;
                }
              }
              else if (this.m_LotAreaData.HasComponent(owner2))
              {
                typeMask1 = TypeMask.Areas;
                result.m_Owner = owner2;
              }
              else
                goto label_16;
            }
            else
              goto label_17;
          }
          while ((input.m_TypeMask & TypeMask.Areas) != TypeMask.None);
          break;
label_16:
          typeMask1 = TypeMask.StaticObjects;
          result.m_Owner = owner2;
        }
        return false;
label_17:
        if ((input.m_Flags & RaycastFlags.SubElements) != (RaycastFlags) 0 && (input.m_TypeMask & typeMask2) != TypeMask.None)
        {
          result.m_Owner = owner1;
          typeMask1 = typeMask2;
          if (typeMask2 == TypeMask.Net)
            result.m_Hit.m_Position = position;
        }
        else if ((input.m_Flags & RaycastFlags.NoMainElements) != (RaycastFlags) 0)
          return false;
        if ((input.m_TypeMask & typeMask1) == TypeMask.None)
          return (input.m_TypeMask & TypeMask.Net) != TypeMask.None && this.FindClosestNode(in input, ref result);
        switch (typeMask1)
        {
          case TypeMask.StaticObjects:
            return this.CheckPlaceholder(in input, ref result.m_Owner);
          case TypeMask.Net:
            return (this.m_PrefabNetData[this.m_PrefabRefData[result.m_Owner].m_Prefab].m_ConnectLayers & input.m_NetLayerMask) != Layer.None && this.CheckNetCollisionMask(in input, result.m_Owner);
          default:
            return true;
        }
      }

      private bool CheckNetCollisionMask(in RaycastInput input, Entity owner)
      {
        Composition componentData1;
        if (this.m_CompositionData.TryGetComponent(owner, out componentData1))
          return this.CheckCompositionCollisionMask(in input, componentData1.m_Edge);
        Orphan componentData2;
        if (this.m_OrphanData.TryGetComponent(owner, out componentData2))
          return this.CheckCompositionCollisionMask(in input, componentData2.m_Composition);
        DynamicBuffer<ConnectedEdge> bufferData;
        if (!this.m_ConnectedEdges.TryGetBuffer(owner, out bufferData))
          return true;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity edge1 = bufferData[index].m_Edge;
          Edge edge2 = this.m_EdgeData[edge1];
          if (edge2.m_Start == owner && this.m_CompositionData.TryGetComponent(edge1, out componentData1) && !this.CheckCompositionCollisionMask(in input, componentData1.m_StartNode) || edge2.m_End == owner && this.m_CompositionData.TryGetComponent(edge1, out componentData1) && !this.CheckCompositionCollisionMask(in input, componentData1.m_EndNode))
            return false;
        }
        return false;
      }

      private bool CheckCompositionCollisionMask(in RaycastInput input, Entity composition)
      {
        NetCompositionData componentData;
        if (this.m_PrefabCompositionData.TryGetComponent(composition, out componentData))
        {
          if ((componentData.m_State & CompositionState.Marker) != (CompositionState) 0)
          {
            if ((input.m_Flags & RaycastFlags.Markers) == (RaycastFlags) 0)
              return false;
          }
          else if ((NetUtils.GetCollisionMask(componentData, true) & input.m_CollisionMask) == (CollisionMask) 0)
            return false;
        }
        return true;
      }

      private bool CheckPlaceholder(in RaycastInput input, ref Entity entity)
      {
        if ((input.m_Flags & RaycastFlags.Placeholders) != (RaycastFlags) 0 || !this.m_PlaceholderData.HasComponent(entity))
          return true;
        if (this.m_AttachmentData.HasComponent(entity))
        {
          Attachment attachment = this.m_AttachmentData[entity];
          if (this.m_PrefabRefData.HasComponent(attachment.m_Attached))
          {
            entity = attachment.m_Attached;
            return true;
          }
        }
        return false;
      }

      private bool FindClosestNode(in RaycastInput input, ref RaycastResult result)
      {
        if (!this.m_SubNets.HasBuffer(result.m_Owner) || (input.m_Flags & (RaycastFlags.SubElements | RaycastFlags.NoMainElements)) != (RaycastFlags) 0)
          return false;
        float num1 = float.MaxValue;
        Entity entity = Entity.Null;
        float3 float3 = new float3();
        DynamicBuffer<Game.Net.SubNet> subNet1 = this.m_SubNets[result.m_Owner];
        for (int index = 0; index < subNet1.Length; ++index)
        {
          Entity subNet2 = subNet1[index].m_SubNet;
          if (this.m_NodeData.HasComponent(subNet2))
          {
            if ((this.m_PrefabNetData[this.m_PrefabRefData[subNet2].m_Prefab].m_ConnectLayers & input.m_NetLayerMask) != Layer.None)
            {
              Game.Net.Node node = this.m_NodeData[subNet2];
              float num2 = math.distance(result.m_Hit.m_HitPosition, node.m_Position);
              if ((double) num2 < (double) num1)
              {
                num1 = num2;
                entity = subNet2;
                float3 = node.m_Position;
              }
            }
          }
          else if (this.m_EdgeData.HasComponent(subNet2) && (this.m_PrefabNetData[this.m_PrefabRefData[subNet2].m_Prefab].m_ConnectLayers & input.m_NetLayerMask) != Layer.None)
          {
            Curve curve = this.m_CurveData[subNet2];
            float t;
            float num3 = MathUtils.Distance(curve.m_Bezier, result.m_Hit.m_HitPosition, out t);
            if ((double) num3 < (double) num1)
            {
              num1 = num3;
              entity = subNet2;
              float3 = MathUtils.Position(curve.m_Bezier, t);
            }
          }
        }
        if (entity == Entity.Null)
          return false;
        result.m_Owner = entity;
        result.m_Hit.m_Position = float3;
        return true;
      }

      private void RaycastLot(
        ref RaycastResult result,
        ObjectGeometryData prefabObjectData,
        Line3.Segment worldLine,
        Line3.Segment localLine)
      {
        float t;
        if (!MathUtils.Intersect(localLine.y, 0.0f, out t) || (double) t >= (double) result.m_Hit.m_NormalizedDistance)
          return;
        bool flag;
        float2 float2;
        if ((prefabObjectData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
        {
          flag = (prefabObjectData.m_Flags & GeometryFlags.CircularLeg) != 0;
          float2 = prefabObjectData.m_LegSize.xz + 0.4f;
        }
        else
        {
          flag = (prefabObjectData.m_Flags & GeometryFlags.Circular) != 0;
          float2 = prefabObjectData.m_Size.xz + 0.4f;
        }
        float3 float3 = MathUtils.Position(localLine, t);
        float2 x = float2 * 0.5f;
        if (flag)
        {
          if ((double) math.length(float3.xz) > (double) math.csum(x) * 0.5)
            return;
        }
        else if (!math.all(float3.xz >= -x & float3.xz <= x))
          return;
        result.m_Hit.m_NormalizedDistance = t;
        result.m_Hit.m_HitPosition = MathUtils.Position(worldLine, result.m_Hit.m_NormalizedDistance);
        result.m_Hit.m_HitDirection = new float3(0.0f, 1f, 0.0f);
        result.m_Hit.m_CellIndex = (int2) -1;
      }

      private bool HasCachedMesh(Entity mesh, out Entity sharedMesh)
      {
        SharedMeshData componentData;
        sharedMesh = !this.m_PrefabSharedMeshData.TryGetComponent(mesh, out componentData) ? mesh : componentData.m_Mesh;
        return this.m_Vertices.HasBuffer(sharedMesh);
      }

      private bool RaycastMeshes(
        in RaycastInput input,
        ref RaycastResult result,
        Entity entity,
        PrefabRef prefabRef,
        Line3.Segment worldLine,
        Line3.Segment localLine,
        quaternion localToWorldRotation,
        float2 cutOffset)
      {
        bool flag1 = false;
        RaycastHit hit = result.m_Hit with
        {
          m_NormalizedDistance = 2f
        };
        DynamicBuffer<SubMesh> bufferData1;
        if (this.m_Meshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData1))
        {
          SubMeshFlags subMeshFlags = (SubMeshFlags) (19136512 | (this.m_LeftHandTraffic ? 65536 : 131072));
          float3 scale1 = (float3) 1f;
          float3 offsets = (float3) 1f;
          float3 scale2 = (float3) 1f;
          int3 tileCounts = (int3) 0;
          Tree componentData1;
          if (this.m_TreeData.TryGetComponent(entity, out componentData1))
          {
            GrowthScaleData componentData2;
            if (this.m_PrefabGrowthScaleData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
              subMeshFlags |= BatchDataHelpers.CalculateTreeSubMeshData(componentData1, componentData2, out scale1);
            else
              subMeshFlags |= SubMeshFlags.RequireAdult;
          }
          Stack componentData3;
          StackData componentData4;
          if (this.m_StackData.TryGetComponent(entity, out componentData3) && this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData4))
            subMeshFlags |= BatchDataHelpers.CalculateStackSubMeshData(componentData3, componentData4, out tileCounts, out offsets, out scale2);
          else
            componentData4 = new StackData();
          NetObject componentData5;
          if (this.m_NetObjectData.TryGetComponent(entity, out componentData5))
            subMeshFlags |= BatchDataHelpers.CalculateNetObjectSubMeshData(componentData5);
          Quantity componentData6;
          if (this.m_QuantityData.TryGetComponent(entity, out componentData6))
          {
            QuantityObjectData componentData7;
            if (this.m_PrefabQuantityObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData7))
              subMeshFlags |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData6, componentData7, this.m_EditorMode);
            else
              subMeshFlags |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData6, new QuantityObjectData(), this.m_EditorMode);
          }
          UnderConstruction componentData8;
          ObjectGeometryData componentData9;
          if (this.m_UnderConstructionData.TryGetComponent(entity, out componentData8) && componentData8.m_NewPrefab == Entity.Null || this.m_DestroyedData.HasComponent(entity) && this.m_PrefabObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData9) && (componentData9.m_Flags & (GeometryFlags.Physical | GeometryFlags.HasLot)) == (GeometryFlags.Physical | GeometryFlags.HasLot))
            return false;
          bool flag2 = false;
          bool flag3 = false;
          DynamicBuffer<MeshGroup> bufferData2 = new DynamicBuffer<MeshGroup>();
          int num1 = 1;
          DynamicBuffer<SubMeshGroup> bufferData3;
          if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData3) && this.m_MeshGroups.TryGetBuffer(entity, out bufferData2))
            num1 = bufferData2.Length;
          for (int index1 = 0; index1 < num1; ++index1)
          {
            MeshGroup meshGroup;
            SubMeshGroup subMeshGroup;
            if (bufferData3.IsCreated)
            {
              CollectionUtils.TryGet<MeshGroup>(bufferData2, index1, out meshGroup);
              subMeshGroup = bufferData3[(int) meshGroup.m_SubMeshGroup];
            }
            else
            {
              subMeshGroup.m_SubMeshRange = new int2(0, bufferData1.Length);
              meshGroup = new MeshGroup();
            }
            for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
            {
              SubMesh subMesh = bufferData1[x];
              if ((subMesh.m_Flags & subMeshFlags) == subMesh.m_Flags)
              {
                int num2 = math.select(math.select(math.select(1, tileCounts.x, (subMesh.m_Flags & SubMeshFlags.IsStackStart) > (SubMeshFlags) 0), tileCounts.y, (subMesh.m_Flags & SubMeshFlags.IsStackMiddle) > (SubMeshFlags) 0), tileCounts.z, (subMesh.m_Flags & SubMeshFlags.IsStackEnd) > (SubMeshFlags) 0);
                if (num2 >= 1)
                {
                  Entity entity1 = Entity.Null;
                  Entity sharedMesh;
                  if (this.HasCachedMesh(subMesh.m_SubMesh, out sharedMesh))
                  {
                    entity1 = sharedMesh;
                  }
                  else
                  {
                    DynamicBuffer<LodMesh> bufferData4;
                    if (this.m_Lods.TryGetBuffer(subMesh.m_SubMesh, out bufferData4))
                    {
                      for (int index2 = bufferData4.Length - 1; index2 >= 0; --index2)
                      {
                        if (this.HasCachedMesh(bufferData4[index2].m_LodMesh, out sharedMesh))
                        {
                          entity1 = sharedMesh;
                          break;
                        }
                      }
                    }
                  }
                  if ((input.m_Flags & RaycastFlags.Decals) != (RaycastFlags) 0 || (this.m_PrefabMeshData[entity1 != Entity.Null ? entity1 : subMesh.m_SubMesh].m_State & MeshFlags.Decal) == (MeshFlags) 0)
                  {
                    if (entity1 == Entity.Null)
                    {
                      flag3 = true;
                    }
                    else
                    {
                      DynamicBuffer<MeshVertex> vertex = this.m_Vertices[entity1];
                      DynamicBuffer<MeshIndex> index3 = this.m_Indices[entity1];
                      DynamicBuffer<MeshNode> bufferData5 = new DynamicBuffer<MeshNode>();
                      DynamicBuffer<ProceduralBone> bufferData6 = new DynamicBuffer<ProceduralBone>();
                      DynamicBuffer<Bone> bufferData7 = new DynamicBuffer<Bone>();
                      DynamicBuffer<Skeleton> dynamicBuffer = new DynamicBuffer<Skeleton>();
                      if (this.m_Nodes.TryGetBuffer(entity1, out bufferData5) && this.m_ProceduralBones.TryGetBuffer(entity1, out bufferData6) && this.m_Bones.TryGetBuffer(entity, out bufferData7))
                      {
                        dynamicBuffer = this.m_Skeletons[entity];
                        if (dynamicBuffer.Length == 0)
                        {
                          bufferData7 = new DynamicBuffer<Bone>();
                          dynamicBuffer = new DynamicBuffer<Skeleton>();
                        }
                      }
                      flag2 |= index3.Length != 0;
                      flag3 |= index3.Length == 0;
                      int num3 = x - subMeshGroup.m_SubMeshRange.x + (int) meshGroup.m_MeshOffset;
                      for (int tileIndex = 0; tileIndex < num2; ++tileIndex)
                      {
                        float3 position = subMesh.m_Position;
                        float3 subMeshScale = scale1;
                        if ((subMesh.m_Flags & (SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd)) != (SubMeshFlags) 0)
                          BatchDataHelpers.CalculateStackSubMeshData(componentData4, offsets, scale2, tileIndex, subMesh.m_Flags, ref position, ref subMeshScale);
                        Line3.Segment localLine1 = localLine;
                        if ((subMesh.m_Flags & (SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd | SubMeshFlags.HasTransform)) != (SubMeshFlags) 0)
                        {
                          quaternion q = math.inverse(subMesh.m_Rotation);
                          localLine1.a = math.mul(q, localLine.a - position) / subMeshScale;
                          localLine1.b = math.mul(q, localLine.b - position) / subMeshScale;
                        }
                        else if (math.any(subMeshScale != 1f))
                        {
                          localLine1.a = localLine.a / subMeshScale;
                          localLine1.b = localLine.b / subMeshScale;
                        }
                        ImpostorData componentData10;
                        if (this.m_PrefabImpostorData.TryGetComponent(entity1, out componentData10) && (double) componentData10.m_Size != 0.0)
                        {
                          localLine1.a = (localLine1.a - componentData10.m_Offset) / componentData10.m_Size;
                          localLine1.b = (localLine1.b - componentData10.m_Offset) / componentData10.m_Size;
                        }
                        if (bufferData5.IsCreated)
                        {
                          if (bufferData6.IsCreated)
                          {
                            if (bufferData7.IsCreated)
                            {
                              if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, bufferData5, bufferData6, bufferData7, dynamicBuffer[num3], new int2(num3, -1), ref hit))
                                hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                            }
                            else if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, bufferData5, bufferData6, new int2(num3, -1), ref hit))
                              hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                          }
                          else if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, bufferData5, new int2(num3, -1), ref hit))
                            hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                        }
                        else if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, new int2(num3, -1), ref hit))
                          hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                      }
                    }
                  }
                }
              }
            }
          }
          flag1 = bufferData1.Length != 0 & (flag2 | !flag3);
        }
        if (!flag1 && (this.m_PrefabObjectData[prefabRef.m_Prefab].m_Flags & GeometryFlags.HasLot) == GeometryFlags.None)
        {
          result.m_Hit.m_NormalizedDistance += 10f / math.max(1f, MathUtils.Length(worldLine));
          return true;
        }
        if ((double) hit.m_NormalizedDistance >= 2.0)
          return false;
        hit.m_NormalizedDistance = math.lerp(cutOffset.x, cutOffset.y, hit.m_NormalizedDistance);
        hit.m_HitPosition = MathUtils.Position(worldLine, hit.m_NormalizedDistance);
        hit.m_HitDirection = math.normalizesafe(math.rotate(localToWorldRotation, hit.m_HitDirection));
        result.m_Hit = hit;
        return true;
      }
    }

    [BurstCompile]
    public struct GetSourceRangesJob : IJob
    {
      [ReadOnly]
      public NativeList<RaycastSystem.EntityResult> m_EdgeList;
      [ReadOnly]
      public NativeList<RaycastSystem.EntityResult> m_StaticObjectList;
      [NativeDisableParallelForRestriction]
      public NativeArray<int4> m_Ranges;

      public void Execute()
      {
        int4 int4 = new int4(this.m_EdgeList.Length + 1, 0, this.m_StaticObjectList.Length + 1, 0);
        for (int index = 0; index < this.m_Ranges.Length; ++index)
          this.m_Ranges[index] = int4;
        for (int index = 0; index < this.m_EdgeList.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          RaycastSystem.EntityResult edge = this.m_EdgeList[index];
          // ISSUE: reference to a compiler-generated field
          ref int4 local = ref this.m_Ranges.ElementAt<int4>(edge.m_RaycastIndex);
          local.x = math.min(local.x, index);
          local.y = index;
        }
        for (int index = 0; index < this.m_StaticObjectList.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          RaycastSystem.EntityResult staticObject = this.m_StaticObjectList[index];
          // ISSUE: reference to a compiler-generated field
          ref int4 local = ref this.m_Ranges.ElementAt<int4>(staticObject.m_RaycastIndex);
          local.z = math.min(local.z, index);
          local.w = index;
        }
      }
    }

    [BurstCompile]
    public struct ExtractLaneObjectsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public NativeList<RaycastSystem.EntityResult> m_EdgeList;
      [ReadOnly]
      public NativeList<RaycastSystem.EntityResult> m_StaticObjectList;
      [ReadOnly]
      public NativeArray<int4> m_Ranges;
      public NativeQueue<RaycastSystem.EntityResult>.ParallelWriter m_MovingObjectQueue;

      public void Execute(int index)
      {
        RaycastInput raycastInput = this.m_Input[index];
        if ((raycastInput.m_TypeMask & TypeMask.MovingObjects) == TypeMask.None)
          return;
        int4 range = this.m_Ranges[index];
        if (range.x > range.y && range.z > range.w)
          return;
        int2 int2 = math.max((int2) 0, range.yw - range.xz + 1);
        NativeParallelHashSet<Entity> checkedEntities1 = new NativeParallelHashSet<Entity>(int2.x * 8 + int2.y, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int x = range.x; x <= range.y; ++x)
        {
          // ISSUE: variable of a compiler-generated type
          RaycastSystem.EntityResult edge1 = this.m_EdgeList[x];
          Composition componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (edge1.m_RaycastIndex == index && this.m_CompositionData.TryGetComponent(edge1.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1.m_Entity];
            NetCompositionData compositionData1 = this.m_PrefabCompositionData[componentData.m_StartNode];
            NetCompositionData compositionData2 = this.m_PrefabCompositionData[componentData.m_Edge];
            NetCompositionData compositionData3 = this.m_PrefabCompositionData[componentData.m_EndNode];
            int collisionMask1 = (int) NetUtils.GetCollisionMask(compositionData1, false);
            CollisionMask collisionMask2 = NetUtils.GetCollisionMask(compositionData2, false);
            CollisionMask collisionMask3 = NetUtils.GetCollisionMask(compositionData3, false);
            int collisionMask4 = (int) raycastInput.m_CollisionMask;
            // ISSUE: variable of a compiler-generated type
            RaycastSystem.EntityResult entityResult;
            if ((collisionMask1 & collisionMask4) != 0)
            {
              RaycastInput input = raycastInput;
              NativeParallelHashSet<Entity> checkedEntities2 = checkedEntities1;
              // ISSUE: object of a compiler-generated type is created
              entityResult = new RaycastSystem.EntityResult();
              // ISSUE: reference to a compiler-generated field
              entityResult.m_Entity = edge2.m_Start;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              entityResult.m_RaycastIndex = edge1.m_RaycastIndex;
              // ISSUE: variable of a compiler-generated type
              RaycastSystem.EntityResult node = entityResult;
              // ISSUE: reference to a compiler-generated field
              Entity entity = edge1.m_Entity;
              this.TryCheckNode(input, checkedEntities2, node, entity);
            }
            if ((collisionMask2 & raycastInput.m_CollisionMask) != (CollisionMask) 0)
              this.TryCheckLanes(checkedEntities1, edge1);
            if ((collisionMask3 & raycastInput.m_CollisionMask) != (CollisionMask) 0)
            {
              RaycastInput input = raycastInput;
              NativeParallelHashSet<Entity> checkedEntities3 = checkedEntities1;
              // ISSUE: object of a compiler-generated type is created
              entityResult = new RaycastSystem.EntityResult();
              // ISSUE: reference to a compiler-generated field
              entityResult.m_Entity = edge2.m_End;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              entityResult.m_RaycastIndex = edge1.m_RaycastIndex;
              // ISSUE: variable of a compiler-generated type
              RaycastSystem.EntityResult node = entityResult;
              // ISSUE: reference to a compiler-generated field
              Entity entity = edge1.m_Entity;
              this.TryCheckNode(input, checkedEntities3, node, entity);
            }
          }
        }
        for (int z = range.z; z <= range.w; ++z)
        {
          // ISSUE: variable of a compiler-generated type
          RaycastSystem.EntityResult staticObject = this.m_StaticObjectList[z];
          // ISSUE: reference to a compiler-generated field
          if (staticObject.m_RaycastIndex == index)
            this.TryCheckObject(checkedEntities1, staticObject);
        }
      }

      private void TryCheckObject(
        NativeParallelHashSet<Entity> checkedEntities,
        RaycastSystem.EntityResult obj)
      {
        this.TryCheckLanes(checkedEntities, obj);
      }

      private void TryCheckNode(
        RaycastInput input,
        NativeParallelHashSet<Entity> checkedEntities,
        RaycastSystem.EntityResult node,
        Entity ignoreEdge)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node.m_Entity];
        this.TryCheckLanes(checkedEntities, node);
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          RaycastSystem.EntityResult entity = new RaycastSystem.EntityResult()
          {
            m_Entity = connectedEdge[index].m_Edge,
            m_RaycastIndex = node.m_RaycastIndex
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(entity.m_Entity == ignoreEdge) && (NetUtils.GetCollisionMask(this.m_PrefabCompositionData[this.m_CompositionData[entity.m_Entity].m_Edge], false) & input.m_CollisionMask) != (CollisionMask) 0)
            this.TryCheckLanes(checkedEntities, entity);
        }
      }

      private void TryCheckLanes(
        NativeParallelHashSet<Entity> checkedEntities,
        RaycastSystem.EntityResult entity)
      {
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!checkedEntities.Add(entity.m_Entity) || !this.m_SubLanes.TryGetBuffer(entity.m_Entity, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated field
        this.CheckLanes(entity.m_RaycastIndex, bufferData);
      }

      private void CheckLanes(int raycastIndex, DynamicBuffer<Game.Net.SubLane> lanes)
      {
        for (int index1 = 0; index1 < lanes.Length; ++index1)
        {
          DynamicBuffer<LaneObject> bufferData;
          if (this.m_LaneObjects.TryGetBuffer(lanes[index1].m_SubLane, out bufferData))
          {
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              // ISSUE: object of a compiler-generated type is created
              this.m_MovingObjectQueue.Enqueue(new RaycastSystem.EntityResult()
              {
                m_Entity = bufferData[index2].m_LaneObject,
                m_RaycastIndex = raycastIndex
              });
            }
          }
        }
      }
    }

    [BurstCompile]
    public struct RaycastMovingObjectsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeArray<RaycastSystem.EntityResult> m_ObjectList;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectData;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> m_PrefabQuantityObjectData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public ComponentLookup<SharedMeshData> m_PrefabSharedMeshData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      [ReadOnly]
      public BufferLookup<Skeleton> m_Skeletons;
      [ReadOnly]
      public BufferLookup<Bone> m_Bones;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [ReadOnly]
      public BufferLookup<SubMesh> m_Meshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<LodMesh> m_Lods;
      [ReadOnly]
      public BufferLookup<MeshVertex> m_Vertices;
      [ReadOnly]
      public BufferLookup<MeshIndex> m_Indices;
      [ReadOnly]
      public BufferLookup<MeshNode> m_Nodes;
      [ReadOnly]
      public BufferLookup<ProceduralBone> m_ProceduralBones;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.EntityResult entityResult = this.m_ObjectList[index];
        // ISSUE: reference to a compiler-generated field
        RaycastInput input = this.m_Input[entityResult.m_RaycastIndex];
        if ((input.m_TypeMask & TypeMask.MovingObjects) == TypeMask.None)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.RaycastObjects(entityResult.m_RaycastIndex, input, entityResult.m_Entity, entityResult.m_Entity);
      }

      private void RaycastObjects(
        int raycastIndex,
        RaycastInput input,
        Entity owner,
        Entity entity)
      {
        this.RaycastObject(raycastIndex, input, owner, entity);
        DynamicBuffer<SubObject> bufferData1;
        if (this.m_SubObjects.TryGetBuffer(entity, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            Entity subObject = bufferData1[index].m_SubObject;
            this.RaycastObjects(raycastIndex, input, owner, subObject);
          }
        }
        DynamicBuffer<Passenger> bufferData2;
        if (!this.m_Passengers.TryGetBuffer(entity, out bufferData2))
          return;
        for (int index = 0; index < bufferData2.Length; ++index)
        {
          Entity passenger = bufferData2[index].m_Passenger;
          this.RaycastObjects(raycastIndex, input, passenger, passenger);
        }
      }

      private void RaycastObject(
        int raycastIndex,
        RaycastInput input,
        Entity owner,
        Entity entity)
      {
        if (!this.IsNearCamera(entity))
          return;
        InterpolatedTransform componentData1;
        Transform transform = !this.m_InterpolatedTransformData.TryGetComponent(entity, out componentData1) ? this.m_TransformData[entity] : componentData1.ToTransform();
        PrefabRef prefabRefData = this.m_PrefabRefData[entity];
        ObjectGeometryData componentData2;
        if (!this.m_PrefabObjectData.TryGetComponent(prefabRefData.m_Prefab, out componentData2))
          return;
        double num1 = (double) MathUtils.DistanceSquared(input.m_Line, transform.m_Position, out float _);
        float3 size = componentData2.m_Size;
        size.xz *= 0.5f;
        double num2 = (double) math.lengthsq(size);
        if (num1 > num2)
          return;
        Bounds3 bounds = componentData2.m_Bounds;
        quaternion q = math.inverse(transform.m_Rotation);
        Line3.Segment segment = new Line3.Segment();
        segment.a = math.mul(q, input.m_Line.a - transform.m_Position);
        segment.b = math.mul(q, input.m_Line.b - transform.m_Position);
        float2 t;
        if (!MathUtils.Intersect(bounds, segment, out t))
          return;
        float3 float3 = MathUtils.Position(input.m_Line, t.x);
        RaycastResult result = new RaycastResult();
        result.m_Owner = owner;
        result.m_Hit.m_HitEntity = entity;
        result.m_Hit.m_Position = transform.m_Position;
        result.m_Hit.m_HitPosition = float3;
        result.m_Hit.m_NormalizedDistance = t.x;
        result.m_Hit.m_CellIndex = new int2(-1, -1);
        float num3 = math.cmax(MathUtils.Size(bounds));
        t = math.saturate(new float2(t.x - num3, t.y + num3));
        segment = MathUtils.Cut(segment, t);
        if (!this.RaycastMeshes(input, ref result, entity, prefabRefData, input.m_Line, segment, transform.m_Rotation, t))
          return;
        this.m_Results.Accumulate(raycastIndex, result);
      }

      private bool IsNearCamera(Entity entity)
      {
        CullingInfo componentData;
        return this.m_CullingInfoData.TryGetComponent(entity, out componentData) && componentData.m_CullingIndex != 0 && (this.m_CullingData[componentData.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) > (PreCullingFlags) 0;
      }

      private bool HasCachedMesh(Entity mesh, out Entity sharedMesh)
      {
        SharedMeshData componentData;
        sharedMesh = !this.m_PrefabSharedMeshData.TryGetComponent(mesh, out componentData) ? mesh : componentData.m_Mesh;
        return this.m_Vertices.HasBuffer(sharedMesh);
      }

      private bool RaycastMeshes(
        RaycastInput input,
        ref RaycastResult result,
        Entity entity,
        PrefabRef prefabRefData,
        Line3.Segment worldLine,
        Line3.Segment localLine,
        quaternion localToWorldRotation,
        float2 cutOffset)
      {
        bool flag1 = false;
        RaycastHit hit = result.m_Hit with
        {
          m_NormalizedDistance = 2f
        };
        DynamicBuffer<SubMesh> bufferData1;
        if (this.m_Meshes.TryGetBuffer(prefabRefData.m_Prefab, out bufferData1))
        {
          SubMeshFlags subMeshFlags = (SubMeshFlags) (2359296 | (this.m_LeftHandTraffic ? 65536 : 131072));
          Quantity componentData1;
          if (this.m_QuantityData.TryGetComponent(entity, out componentData1))
          {
            QuantityObjectData componentData2;
            if (this.m_PrefabQuantityObjectData.TryGetComponent(prefabRefData.m_Prefab, out componentData2))
              subMeshFlags |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData1, componentData2, this.m_EditorMode);
            else
              subMeshFlags |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData1, new QuantityObjectData(), this.m_EditorMode);
          }
          bool flag2 = false;
          bool flag3 = false;
          DynamicBuffer<MeshGroup> bufferData2 = new DynamicBuffer<MeshGroup>();
          int num1 = 1;
          DynamicBuffer<SubMeshGroup> bufferData3;
          if (this.m_SubMeshGroups.TryGetBuffer(prefabRefData.m_Prefab, out bufferData3) && this.m_MeshGroups.TryGetBuffer(entity, out bufferData2))
            num1 = bufferData2.Length;
          for (int index1 = 0; index1 < num1; ++index1)
          {
            MeshGroup meshGroup;
            SubMeshGroup subMeshGroup;
            if (bufferData3.IsCreated)
            {
              CollectionUtils.TryGet<MeshGroup>(bufferData2, index1, out meshGroup);
              subMeshGroup = bufferData3[(int) meshGroup.m_SubMeshGroup];
            }
            else
            {
              subMeshGroup.m_SubMeshRange = new int2(0, bufferData1.Length);
              meshGroup = new MeshGroup();
            }
            for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
            {
              SubMesh subMesh = bufferData1[x];
              if ((subMesh.m_Flags & subMeshFlags) == subMesh.m_Flags)
              {
                Entity entity1 = Entity.Null;
                Entity sharedMesh;
                if (this.HasCachedMesh(subMesh.m_SubMesh, out sharedMesh))
                {
                  entity1 = sharedMesh;
                }
                else
                {
                  DynamicBuffer<LodMesh> bufferData4;
                  if (this.m_Lods.TryGetBuffer(subMesh.m_SubMesh, out bufferData4))
                  {
                    for (int index2 = bufferData4.Length - 1; index2 >= 0; --index2)
                    {
                      if (this.HasCachedMesh(bufferData4[index2].m_LodMesh, out sharedMesh))
                      {
                        entity1 = sharedMesh;
                        break;
                      }
                    }
                  }
                }
                if ((input.m_Flags & RaycastFlags.Decals) != (RaycastFlags) 0 || (this.m_PrefabMeshData[entity1 != Entity.Null ? entity1 : subMesh.m_SubMesh].m_State & MeshFlags.Decal) == (MeshFlags) 0)
                {
                  if (entity1 == Entity.Null)
                  {
                    flag3 = true;
                  }
                  else
                  {
                    Line3.Segment localLine1 = localLine;
                    if ((subMesh.m_Flags & SubMeshFlags.HasTransform) != (SubMeshFlags) 0)
                    {
                      quaternion q = math.inverse(subMesh.m_Rotation);
                      localLine1.a = math.mul(q, localLine.a - subMesh.m_Position);
                      localLine1.b = math.mul(q, localLine.b - subMesh.m_Position);
                    }
                    DynamicBuffer<MeshVertex> vertex = this.m_Vertices[entity1];
                    DynamicBuffer<MeshIndex> index3 = this.m_Indices[entity1];
                    flag2 |= index3.Length != 0;
                    flag3 |= index3.Length == 0;
                    int num2 = x - subMeshGroup.m_SubMeshRange.x + (int) meshGroup.m_MeshOffset;
                    DynamicBuffer<MeshNode> bufferData5;
                    if (this.m_Nodes.TryGetBuffer(entity1, out bufferData5))
                    {
                      DynamicBuffer<ProceduralBone> bufferData6;
                      if (this.m_ProceduralBones.TryGetBuffer(entity1, out bufferData6))
                      {
                        DynamicBuffer<Bone> bufferData7;
                        if (this.m_Bones.TryGetBuffer(entity, out bufferData7))
                        {
                          DynamicBuffer<Skeleton> skeleton = this.m_Skeletons[entity];
                          if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, bufferData5, bufferData6, bufferData7, skeleton[num2], new int2(num2, -1), ref hit))
                            hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                        }
                        else if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, bufferData5, bufferData6, new int2(num2, -1), ref hit))
                          hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                      }
                      else if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, bufferData5, new int2(num2, -1), ref hit))
                        hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                    }
                    else if (RaycastJobs.CheckMeshIntersect(localLine1, vertex, index3, new int2(num2, -1), ref hit))
                      hit.m_HitDirection = math.rotate(subMesh.m_Rotation, hit.m_HitDirection);
                  }
                }
              }
            }
          }
          flag1 = bufferData1.Length != 0 & (flag2 | !flag3);
        }
        if (!flag1)
          return true;
        if ((double) hit.m_NormalizedDistance >= 2.0)
          return false;
        hit.m_NormalizedDistance = math.lerp(cutOffset.x, cutOffset.y, hit.m_NormalizedDistance);
        hit.m_HitPosition = MathUtils.Position(worldLine, hit.m_NormalizedDistance);
        hit.m_HitDirection = math.normalizesafe(math.rotate(localToWorldRotation, hit.m_HitDirection));
        result.m_Hit = hit;
        return true;
      }
    }
  }
}
