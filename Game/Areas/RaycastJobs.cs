// Decompiled with JetBrains decompiler
// Type: Game.Areas.RaycastJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  public static class RaycastJobs
  {
    [BurstCompile]
    public struct FindAreaJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Space> m_SpaceData;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaData;
      [ReadOnly]
      public BufferLookup<Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_SearchTree;
      [ReadOnly]
      public NativeArray<RaycastResult> m_TerrainResults;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        int index1 = index % this.m_Input.Length;
        RaycastInput raycastInput = this.m_Input[index1];
        if ((raycastInput.m_TypeMask & TypeMask.Areas) == TypeMask.None)
          return;
        RaycastJobs.FindAreaJob.ValidationData validationData = new RaycastJobs.FindAreaJob.ValidationData()
        {
          m_Input = raycastInput,
          m_OwnerData = this.m_OwnerData,
          m_PlaceholderData = this.m_PlaceholderData,
          m_AttachmentData = this.m_AttachmentData,
          m_BuildingData = this.m_BuildingData,
          m_ServiceUpgradeData = this.m_ServiceUpgradeData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabAreaData = this.m_PrefabAreaData,
          m_Nodes = this.m_Nodes,
          m_InstalledUpgrades = this.m_InstalledUpgrades
        };
        if (index < this.m_TerrainResults.Length)
        {
          RaycastResult terrainResult = this.m_TerrainResults[index];
          if (terrainResult.m_Owner == Entity.Null)
            return;
          terrainResult.m_Owner = Entity.Null;
          terrainResult.m_Hit.m_CellIndex = new int2(-1, -1);
          terrainResult.m_Hit.m_NormalizedDistance -= 0.25f / math.max(1f, MathUtils.Length(raycastInput.m_Line));
          RaycastJobs.FindAreaJob.GroundIterator iterator = new RaycastJobs.FindAreaJob.GroundIterator()
          {
            m_Index = index1,
            m_Result = terrainResult,
            m_SpaceData = this.m_SpaceData,
            m_Nodes = this.m_Nodes,
            m_Triangles = this.m_Triangles,
            m_ValidationData = validationData,
            m_Results = this.m_Results
          };
          this.m_SearchTree.Iterate<RaycastJobs.FindAreaJob.GroundIterator>(ref iterator);
        }
        else
        {
          RaycastJobs.FindAreaJob.OvergroundIterator iterator = new RaycastJobs.FindAreaJob.OvergroundIterator()
          {
            m_Index = index1,
            m_Line = raycastInput.m_Line,
            m_SpaceData = this.m_SpaceData,
            m_Nodes = this.m_Nodes,
            m_Triangles = this.m_Triangles,
            m_ValidationData = validationData,
            m_Results = this.m_Results
          };
          this.m_SearchTree.Iterate<RaycastJobs.FindAreaJob.OvergroundIterator>(ref iterator);
        }
      }

      private struct ValidationData
      {
        public RaycastInput m_Input;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Placeholder> m_PlaceholderData;
        public ComponentLookup<Attachment> m_AttachmentData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<AreaGeometryData> m_PrefabAreaData;
        public BufferLookup<Node> m_Nodes;
        public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;

        public bool ValidateResult(ref RaycastResult result)
        {
          TypeMask typeMask1 = TypeMask.Areas;
          Entity owner = Entity.Null;
          TypeMask typeMask2 = TypeMask.None;
          while (true)
          {
            do
            {
              if ((this.m_Input.m_Flags & RaycastFlags.UpgradeIsMain) != (RaycastFlags) 0)
              {
                if (!this.m_ServiceUpgradeData.HasComponent(result.m_Owner))
                {
                  DynamicBuffer<InstalledUpgrade> bufferData;
                  if (this.m_InstalledUpgrades.TryGetBuffer(result.m_Owner, out bufferData) && bufferData.Length != 0)
                  {
                    owner = Entity.Null;
                    typeMask2 = TypeMask.None;
                    typeMask1 = TypeMask.StaticObjects;
                    result.m_Owner = bufferData[0].m_Upgrade;
                    goto label_11;
                  }
                }
                else
                  goto label_11;
              }
              else if ((this.m_Input.m_Flags & RaycastFlags.SubBuildings) != (RaycastFlags) 0 && this.m_BuildingData.HasComponent(result.m_Owner) && this.m_ServiceUpgradeData.HasComponent(result.m_Owner))
                goto label_11;
              if (this.m_OwnerData.HasComponent(result.m_Owner))
              {
                if ((this.m_Input.m_TypeMask & typeMask1) != TypeMask.None)
                {
                  owner = result.m_Owner;
                  typeMask2 = typeMask1;
                }
                result.m_Owner = this.m_OwnerData[result.m_Owner].m_Owner;
              }
              else
                goto label_11;
            }
            while (this.m_Nodes.HasBuffer(result.m_Owner));
            typeMask1 = TypeMask.StaticObjects;
          }
label_11:
          if ((this.m_Input.m_Flags & RaycastFlags.SubElements) != (RaycastFlags) 0 && (this.m_Input.m_TypeMask & typeMask2) != TypeMask.None)
          {
            result.m_Owner = owner;
            typeMask1 = typeMask2;
          }
          else if ((this.m_Input.m_Flags & RaycastFlags.NoMainElements) != (RaycastFlags) 0)
            return false;
          if ((this.m_Input.m_TypeMask & typeMask1) == TypeMask.None)
            return false;
          if (typeMask1 == TypeMask.StaticObjects)
            return this.CheckPlaceholder(ref result.m_Owner);
          return typeMask1 != TypeMask.Areas || (AreaUtils.GetTypeMask(this.m_PrefabAreaData[this.m_PrefabRefData[result.m_Owner].m_Prefab].m_Type) & this.m_Input.m_AreaTypeMask) != 0;
        }

        private bool CheckPlaceholder(ref Entity entity)
        {
          if ((this.m_Input.m_Flags & RaycastFlags.Placeholders) != (RaycastFlags) 0 || !this.m_PlaceholderData.HasComponent(entity))
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
      }

      private struct GroundIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public int m_Index;
        public RaycastResult m_Result;
        public ComponentLookup<Space> m_SpaceData;
        public BufferLookup<Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public RaycastJobs.FindAreaJob.ValidationData m_ValidationData;
        public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Result.m_Hit.m_HitPosition.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Result.m_Hit.m_HitPosition.xz) || this.m_SpaceData.HasComponent(areaItem.m_Area) || !MathUtils.Intersect(AreaUtils.GetTriangle3(this.m_Nodes[areaItem.m_Area], this.m_Triangles[areaItem.m_Area][areaItem.m_Triangle]).xz, this.m_Result.m_Hit.m_HitPosition.xz, out float2 _))
            return;
          RaycastResult result = this.m_Result with
          {
            m_Owner = areaItem.m_Area
          };
          if (!this.m_ValidationData.ValidateResult(ref result))
            return;
          this.m_Results.Accumulate(this.m_Index, result);
        }
      }

      private struct OvergroundIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public int m_Index;
        public Line3.Segment m_Line;
        public ComponentLookup<Space> m_SpaceData;
        public BufferLookup<Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public RaycastJobs.FindAreaJob.ValidationData m_ValidationData;
        public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _) || !this.m_SpaceData.HasComponent(areaItem.m_Area))
            return;
          Triangle3 triangle3 = AreaUtils.GetTriangle3(this.m_Nodes[areaItem.m_Area], this.m_Triangles[areaItem.m_Area][areaItem.m_Triangle]);
          float3 t;
          if (!MathUtils.Intersect(triangle3, this.m_Line, out t))
            return;
          AreaGeometryData areaGeometryData = this.m_ValidationData.m_PrefabAreaData[this.m_ValidationData.m_PrefabRefData[areaItem.m_Area].m_Prefab];
          RaycastResult result = new RaycastResult()
          {
            m_Owner = areaItem.m_Area
          };
          result.m_Hit.m_HitEntity = result.m_Owner;
          result.m_Hit.m_Position = MathUtils.Position(this.m_Line, t.z);
          result.m_Hit.m_HitPosition = result.m_Hit.m_Position;
          result.m_Hit.m_HitDirection = MathUtils.NormalCW(triangle3);
          result.m_Hit.m_NormalizedDistance = t.z - 1f / math.max(1f, MathUtils.Length(this.m_Line));
          result.m_Hit.m_CellIndex = new int2(-1, -1);
          if (!this.m_ValidationData.ValidateResult(ref result))
            return;
          this.m_Results.Accumulate(this.m_Index, result);
        }
      }
    }

    [BurstCompile]
    public struct RaycastLabelsJob : IJobChunk
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> m_GeometryType;
      [ReadOnly]
      public BufferTypeHandle<LabelExtents> m_LabelExtentsType;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<Geometry> nativeArray2 = chunk.GetNativeArray<Geometry>(ref this.m_GeometryType);
        BufferAccessor<LabelExtents> bufferAccessor = chunk.GetBufferAccessor<LabelExtents>(ref this.m_LabelExtentsType);
        quaternion labelRotation = AreaUtils.CalculateLabelRotation(this.m_CameraRight);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Geometry geometry = nativeArray2[index1];
          DynamicBuffer<LabelExtents> dynamicBuffer = bufferAccessor[index1];
          float3 labelPosition = AreaUtils.CalculateLabelPosition(geometry);
          for (int index2 = 0; index2 < this.m_Input.Length; ++index2)
          {
            RaycastInput raycastInput = this.m_Input[index2];
            if ((raycastInput.m_TypeMask & TypeMask.Labels) != TypeMask.None)
            {
              float4x4 labelMatrix = AreaUtils.CalculateLabelMatrix(raycastInput.m_Line.a, labelPosition, labelRotation);
              for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
              {
                Bounds2 bounds = dynamicBuffer[index3].m_Bounds;
                Quad3 quad;
                quad.a = math.transform(labelMatrix, new float3(bounds.min.xy, 0.0f));
                quad.b = math.transform(labelMatrix, new float3(bounds.min.x, bounds.max.y, 0.0f));
                quad.c = math.transform(labelMatrix, new float3(bounds.max.xy, 0.0f));
                quad.d = math.transform(labelMatrix, new float3(bounds.max.x, bounds.min.y, 0.0f));
                float t;
                if (MathUtils.Intersect(quad, raycastInput.m_Line, out t))
                {
                  float num = MathUtils.Size(bounds.y) * AreaUtils.CalculateLabelScale(raycastInput.m_Line.a, labelPosition);
                  RaycastResult raycastResult = new RaycastResult()
                  {
                    m_Owner = nativeArray1[index1]
                  };
                  raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
                  raycastResult.m_Hit.m_Position = geometry.m_CenterPosition;
                  raycastResult.m_Hit.m_HitPosition = MathUtils.Position(raycastInput.m_Line, t);
                  raycastResult.m_Hit.m_NormalizedDistance = t - num / math.max(1f, MathUtils.Length(raycastInput.m_Line));
                  this.m_Results.Accumulate(index2, raycastResult);
                }
              }
            }
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }
  }
}
