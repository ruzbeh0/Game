// Decompiled with JetBrains decompiler
// Type: Game.Zones.CellOccupyJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Zones
{
  public static class CellOccupyJobs
  {
    [BurstCompile]
    public struct ZoneAndOccupyCellsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<CellCheckHelpers.SortedEntity> m_Blocks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DeletedBlockChunks;
      [ReadOnly]
      public ZonePrefabs m_ZonePrefabs;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<ValidArea> m_ValidAreaData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> m_PrefabSignatureBuildingData;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> m_PrefabPlaceholderBuildingData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_PrefabZoneData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Cell> m_Cells;

      public void Execute(int index)
      {
        Entity entity = this.m_Blocks[index].m_Entity;
        Block block = this.m_BlockData[entity];
        ValidArea validArea = this.m_ValidAreaData[entity];
        DynamicBuffer<Cell> cell = this.m_Cells[entity];
        CellOccupyJobs.ZoneAndOccupyCellsJob.ClearOverrideStatus(block, cell);
        if (validArea.m_Area.y <= validArea.m_Area.x)
          return;
        Quad2 corners = ZoneUtils.CalculateCorners(block, validArea);
        Bounds2 bounds = ZoneUtils.CalculateBounds(block);
        CellOccupyJobs.ZoneAndOccupyCellsJob.DeletedBlockIterator deletedBlockIterator = new CellOccupyJobs.ZoneAndOccupyCellsJob.DeletedBlockIterator()
        {
          m_Quad = corners,
          m_Bounds = bounds,
          m_BlockData = block,
          m_ValidAreaData = validArea,
          m_Cells = cell,
          m_BlockDataFromEntity = this.m_BlockData,
          m_ValidAreaDataFromEntity = this.m_ValidAreaData,
          m_CellsFromEntity = this.m_Cells
        };
        for (int index1 = 0; index1 < this.m_DeletedBlockChunks.Length; ++index1)
        {
          NativeArray<Entity> nativeArray = this.m_DeletedBlockChunks[index1].GetNativeArray(this.m_EntityType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            deletedBlockIterator.Iterate(nativeArray[index2]);
        }
        deletedBlockIterator.Dispose();
        CellOccupyJobs.ZoneAndOccupyCellsJob.ObjectIterator iterator = new CellOccupyJobs.ZoneAndOccupyCellsJob.ObjectIterator()
        {
          m_BlockEntity = entity,
          m_BlockData = block,
          m_Bounds = bounds,
          m_Quad = corners,
          m_Xxzz = validArea.m_Area,
          m_Cells = cell,
          m_TransformData = this.m_TransformData,
          m_ElevationData = this.m_ElevationData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
          m_PrefabSpawnableBuildingData = this.m_PrefabSpawnableBuildingData,
          m_PrefabSignatureBuildingData = this.m_PrefabSignatureBuildingData,
          m_PrefabPlaceholderBuildingData = this.m_PrefabPlaceholderBuildingData,
          m_PrefabZoneData = this.m_PrefabZoneData,
          m_PrefabData = this.m_PrefabData
        };
        this.m_ObjectSearchTree.Iterate<CellOccupyJobs.ZoneAndOccupyCellsJob.ObjectIterator>(ref iterator);
        this.SetOccupiedWithHeight(block, cell);
      }

      private static void ClearOverrideStatus(Block blockData, DynamicBuffer<Cell> cells)
      {
        for (int index1 = 0; index1 < blockData.m_Size.y; ++index1)
        {
          for (int index2 = 0; index2 < blockData.m_Size.x; ++index2)
          {
            int index3 = index1 * blockData.m_Size.x + index2;
            Cell cell = cells[index3];
            if ((cell.m_State & CellFlags.Overridden) != CellFlags.None)
            {
              cell.m_State &= ~CellFlags.Overridden;
              cell.m_Zone = ZoneType.None;
              cells[index3] = cell;
            }
          }
        }
      }

      private void SetOccupiedWithHeight(Block blockData, DynamicBuffer<Cell> cells)
      {
        for (int index1 = 0; index1 < blockData.m_Size.y; ++index1)
        {
          for (int index2 = 0; index2 < blockData.m_Size.x; ++index2)
          {
            int index3 = index1 * blockData.m_Size.x + index2;
            Cell cell = cells[index3];
            if ((cell.m_State & CellFlags.Occupied) == CellFlags.None && cell.m_Height < short.MaxValue)
            {
              ZoneData zoneData = this.m_PrefabZoneData[this.m_ZonePrefabs[cell.m_Zone]];
              if ((double) math.max((int) zoneData.m_MinOddHeight, (int) zoneData.m_MinEvenHeight) > (double) cell.m_Height - (double) blockData.m_Position.y)
              {
                cell.m_State |= CellFlags.Occupied;
                cells[index3] = cell;
              }
            }
          }
        }
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_BlockEntity;
        public Block m_BlockData;
        public Bounds2 m_Bounds;
        public Quad2 m_Quad;
        public int4 m_Xxzz;
        public DynamicBuffer<Cell> m_Cells;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<Elevation> m_ElevationData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
        public ComponentLookup<SignatureBuildingData> m_PrefabSignatureBuildingData;
        public ComponentLookup<PlaceholderBuildingData> m_PrefabPlaceholderBuildingData;
        public ComponentLookup<ZoneData> m_PrefabZoneData;
        public ComponentLookup<PrefabData> m_PrefabData;
        private bool m_ShouldOverride;
        private ZoneType m_OverrideZone;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return (bounds.m_Mask & (BoundsMask.OccupyZone | BoundsMask.NotOverridden)) == (BoundsMask.OccupyZone | BoundsMask.NotOverridden) && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          if ((bounds.m_Mask & (BoundsMask.OccupyZone | BoundsMask.NotOverridden)) != (BoundsMask.OccupyZone | BoundsMask.NotOverridden) || !MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
            return;
          bool flag = false;
          if (this.m_ElevationData.HasComponent(objectEntity))
          {
            Elevation elevation = this.m_ElevationData[objectEntity];
            if ((double) elevation.m_Elevation < 0.0)
              return;
            flag = (double) elevation.m_Elevation > 0.0;
          }
          PrefabRef prefabRef = this.m_PrefabRefData[objectEntity];
          Game.Objects.Transform transform = this.m_TransformData[objectEntity];
          if (!this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
            return;
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
          this.m_ShouldOverride = (objectGeometryData.m_Flags & GeometryFlags.OverrideZone) != 0;
          bool isElevated = flag & (objectGeometryData.m_Flags & GeometryFlags.BaseCollision) == GeometryFlags.None;
          this.m_OverrideZone = ZoneType.None;
          SpawnableBuildingData componentData1;
          if (this.m_PrefabSpawnableBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            ZoneData componentData2;
            if (this.m_PrefabSignatureBuildingData.HasComponent(prefabRef.m_Prefab) && this.m_PrefabZoneData.TryGetComponent(componentData1.m_ZonePrefab, out componentData2) && this.m_PrefabData.IsComponentEnabled(componentData1.m_ZonePrefab))
              this.m_OverrideZone = componentData2.m_ZoneType;
          }
          else
          {
            PlaceholderBuildingData componentData3;
            ZoneData componentData4;
            if (this.m_PrefabPlaceholderBuildingData.TryGetComponent(prefabRef.m_Prefab, out componentData3) && this.m_PrefabZoneData.TryGetComponent(componentData3.m_ZonePrefab, out componentData4) && this.m_PrefabData.IsComponentEnabled(componentData3.m_ZonePrefab))
              this.m_OverrideZone = componentData4.m_ZoneType;
          }
          if ((objectGeometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
            objectGeometryData.m_Bounds.min.y = math.max(objectGeometryData.m_Bounds.min.y, 0.0f);
          if ((objectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((objectGeometryData.m_Flags & GeometryFlags.CircularLeg) != GeometryFlags.None)
            {
              Circle2 circle2 = new Circle2(math.max(objectGeometryData.m_LegSize - 0.16f, (float3) 0.0f).x * 0.5f, transform.m_Position.xz);
              Bounds3 bounds1 = bounds.m_Bounds with
              {
                xz = MathUtils.Bounds(circle2)
              };
              bounds1.min.y = transform.m_Position.y + objectGeometryData.m_Bounds.min.y;
              if (MathUtils.Intersect(this.m_Quad, circle2))
                this.CheckOverlapX(this.m_Bounds, bounds1, this.m_Quad, circle2, this.m_Xxzz, isElevated);
            }
            else
            {
              Bounds3 bounds2;
              bounds2.min = objectGeometryData.m_LegSize * -0.5f + 0.08f;
              bounds2.max = objectGeometryData.m_LegSize * 0.5f - 0.08f;
              float3 b = MathUtils.Center(bounds2);
              bool3 c = bounds2.min > bounds2.max;
              bounds2.min = math.select(bounds2.min, b, c);
              bounds2.max = math.select(bounds2.max, b, c);
              Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, bounds2);
              Bounds3 bounds2_1 = MathUtils.Bounds(baseCorners);
              bounds2_1.min.y += objectGeometryData.m_Bounds.min.y;
              if (MathUtils.Intersect(this.m_Quad, baseCorners.xz))
                this.CheckOverlapX(this.m_Bounds, bounds2_1, this.m_Quad, baseCorners.xz, this.m_Xxzz, isElevated);
            }
            transform.m_Position += math.rotate(transform.m_Rotation, new float3(0.0f, objectGeometryData.m_LegSize.y, 0.0f));
            objectGeometryData.m_Bounds.min.y = 0.0f;
            isElevated = true;
          }
          if ((objectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            Circle2 circle2 = new Circle2(math.max(objectGeometryData.m_Size - 0.16f, (float3) 0.0f).x * 0.5f, transform.m_Position.xz);
            Bounds3 bounds3 = bounds.m_Bounds with
            {
              xz = MathUtils.Bounds(circle2)
            };
            bounds3.min.y = transform.m_Position.y + objectGeometryData.m_Bounds.min.y;
            if (!MathUtils.Intersect(this.m_Quad, circle2))
              return;
            this.CheckOverlapX(this.m_Bounds, bounds3, this.m_Quad, circle2, this.m_Xxzz, isElevated);
          }
          else
          {
            Bounds3 bounds4 = MathUtils.Expand(objectGeometryData.m_Bounds, (float3) -0.08f);
            float3 b = MathUtils.Center(bounds4);
            bool3 c = bounds4.min > bounds4.max;
            bounds4.min = math.select(bounds4.min, b, c);
            bounds4.max = math.select(bounds4.max, b, c);
            Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, bounds4);
            Bounds3 bounds2 = MathUtils.Bounds(baseCorners);
            bounds2.min.y += objectGeometryData.m_Bounds.min.y;
            if (!MathUtils.Intersect(this.m_Quad, baseCorners.xz))
              return;
            this.CheckOverlapX(this.m_Bounds, bounds2, this.m_Quad, baseCorners.xz, this.m_Xxzz, isElevated);
          }
        }

        private void CheckOverlapX(
          Bounds2 bounds1,
          Bounds3 bounds2,
          Quad2 quad1,
          Quad2 quad2,
          int4 xxzz1,
          bool isElevated)
        {
          if (xxzz1.y - xxzz1.x >= 2)
          {
            int4 xxzz1_1 = xxzz1;
            int4 xxzz1_2 = xxzz1;
            xxzz1_1.y = xxzz1.x + xxzz1.y >> 1;
            xxzz1_2.x = xxzz1_1.y;
            Quad2 quad2_1 = quad1;
            Quad2 quad2_2 = quad1;
            float s = (float) (xxzz1_1.y - xxzz1.x) / (float) (xxzz1.y - xxzz1.x);
            quad2_1.b = math.lerp(quad1.a, quad1.b, s);
            quad2_1.c = math.lerp(quad1.d, quad1.c, s);
            quad2_2.a = quad2_1.b;
            quad2_2.d = quad2_1.c;
            Bounds2 bounds1_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds1_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1_1, bounds2.xz))
              this.CheckOverlapZ(bounds1_1, bounds2, quad2_1, quad2, xxzz1_1, isElevated);
            if (!MathUtils.Intersect(bounds1_2, bounds2.xz))
              return;
            this.CheckOverlapZ(bounds1_2, bounds2, quad2_2, quad2, xxzz1_2, isElevated);
          }
          else
            this.CheckOverlapZ(bounds1, bounds2, quad1, quad2, xxzz1, isElevated);
        }

        private void CheckOverlapZ(
          Bounds2 bounds1,
          Bounds3 bounds2,
          Quad2 quad1,
          Quad2 quad2,
          int4 xxzz1,
          bool isElevated)
        {
          if (xxzz1.w - xxzz1.z >= 2)
          {
            int4 xxzz1_1 = xxzz1;
            int4 xxzz1_2 = xxzz1;
            xxzz1_1.w = xxzz1.z + xxzz1.w >> 1;
            xxzz1_2.z = xxzz1_1.w;
            Quad2 quad2_1 = quad1;
            Quad2 quad2_2 = quad1;
            float s = (float) (xxzz1_1.w - xxzz1.z) / (float) (xxzz1.w - xxzz1.z);
            quad2_1.d = math.lerp(quad1.a, quad1.d, s);
            quad2_1.c = math.lerp(quad1.b, quad1.c, s);
            quad2_2.a = quad2_1.d;
            quad2_2.b = quad2_1.c;
            Bounds2 bounds1_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds1_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1_1, bounds2.xz))
              this.CheckOverlapX(bounds1_1, bounds2, quad2_1, quad2, xxzz1_1, isElevated);
            if (!MathUtils.Intersect(bounds1_2, bounds2.xz))
              return;
            this.CheckOverlapX(bounds1_2, bounds2, quad2_2, quad2, xxzz1_2, isElevated);
          }
          else if (xxzz1.y - xxzz1.x >= 2)
          {
            this.CheckOverlapX(bounds1, bounds2, quad1, quad2, xxzz1, isElevated);
          }
          else
          {
            int index = xxzz1.z * this.m_BlockData.m_Size.x + xxzz1.x;
            Cell cell = this.m_Cells[index];
            if ((cell.m_State & CellFlags.Blocked) != CellFlags.None)
              return;
            quad1 = MathUtils.Expand(quad1, -0.01f);
            if (!MathUtils.Intersect(quad1, quad2))
              return;
            cell.m_State |= CellFlags.Occupied;
            if (this.m_ShouldOverride && (!this.m_OverrideZone.Equals(cell.m_Zone) || this.m_OverrideZone.Equals(ZoneType.None)))
            {
              cell.m_State |= CellFlags.Overridden;
              cell.m_Zone = this.m_OverrideZone;
            }
            this.m_Cells[index] = cell;
          }
        }

        private void CheckOverlapX(
          Bounds2 bounds1,
          Bounds3 bounds2,
          Quad2 quad1,
          Circle2 circle2,
          int4 xxzz1,
          bool isElevated)
        {
          if (xxzz1.y - xxzz1.x >= 2)
          {
            int4 xxzz1_1 = xxzz1;
            int4 xxzz1_2 = xxzz1;
            xxzz1_1.y = xxzz1.x + xxzz1.y >> 1;
            xxzz1_2.x = xxzz1_1.y;
            Quad2 quad2_1 = quad1;
            Quad2 quad2_2 = quad1;
            float s = (float) (xxzz1_1.y - xxzz1.x) / (float) (xxzz1.y - xxzz1.x);
            quad2_1.b = math.lerp(quad1.a, quad1.b, s);
            quad2_1.c = math.lerp(quad1.d, quad1.c, s);
            quad2_2.a = quad2_1.b;
            quad2_2.d = quad2_1.c;
            Bounds2 bounds1_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds1_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1_1, bounds2.xz))
              this.CheckOverlapZ(bounds1_1, bounds2, quad2_1, circle2, xxzz1_1, isElevated);
            if (!MathUtils.Intersect(bounds1_2, bounds2.xz))
              return;
            this.CheckOverlapZ(bounds1_2, bounds2, quad2_2, circle2, xxzz1_2, isElevated);
          }
          else
            this.CheckOverlapZ(bounds1, bounds2, quad1, circle2, xxzz1, isElevated);
        }

        private void CheckOverlapZ(
          Bounds2 bounds1,
          Bounds3 bounds2,
          Quad2 quad1,
          Circle2 circle2,
          int4 xxzz1,
          bool isElevated)
        {
          if (xxzz1.w - xxzz1.z >= 2)
          {
            int4 xxzz1_1 = xxzz1;
            int4 xxzz1_2 = xxzz1;
            xxzz1_1.w = xxzz1.z + xxzz1.w >> 1;
            xxzz1_2.z = xxzz1_1.w;
            Quad2 quad2_1 = quad1;
            Quad2 quad2_2 = quad1;
            float s = (float) (xxzz1_1.w - xxzz1.z) / (float) (xxzz1.w - xxzz1.z);
            quad2_1.d = math.lerp(quad1.a, quad1.d, s);
            quad2_1.c = math.lerp(quad1.b, quad1.c, s);
            quad2_2.a = quad2_1.d;
            quad2_2.b = quad2_1.c;
            Bounds2 bounds1_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds1_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1_1, bounds2.xz))
              this.CheckOverlapX(bounds1_1, bounds2, quad2_1, circle2, xxzz1_1, isElevated);
            if (!MathUtils.Intersect(bounds1_2, bounds2.xz))
              return;
            this.CheckOverlapX(bounds1_2, bounds2, quad2_2, circle2, xxzz1_2, isElevated);
          }
          else if (xxzz1.y - xxzz1.x >= 2)
          {
            this.CheckOverlapX(bounds1, bounds2, quad1, circle2, xxzz1, isElevated);
          }
          else
          {
            int index = xxzz1.z * this.m_BlockData.m_Size.x + xxzz1.x;
            Cell cell = this.m_Cells[index];
            if ((cell.m_State & CellFlags.Blocked) != CellFlags.None)
              return;
            quad1 = MathUtils.Expand(quad1, -0.01f);
            if (!MathUtils.Intersect(quad1, circle2))
              return;
            if (isElevated)
            {
              cell.m_Height = (short) math.clamp(Mathf.FloorToInt(bounds2.min.y), (int) short.MinValue, math.min((int) cell.m_Height, (int) short.MaxValue));
            }
            else
            {
              cell.m_State |= CellFlags.Occupied;
              if (this.m_ShouldOverride && (!this.m_OverrideZone.Equals(cell.m_Zone) || this.m_OverrideZone.Equals(ZoneType.None)))
              {
                cell.m_State |= CellFlags.Overridden;
                cell.m_Zone = this.m_OverrideZone;
              }
            }
            this.m_Cells[index] = cell;
          }
        }
      }

      private struct DeletedBlockIterator
      {
        public Quad2 m_Quad;
        public Bounds2 m_Bounds;
        public Block m_BlockData;
        public ValidArea m_ValidAreaData;
        public DynamicBuffer<Cell> m_Cells;
        public ComponentLookup<Block> m_BlockDataFromEntity;
        public ComponentLookup<ValidArea> m_ValidAreaDataFromEntity;
        public BufferLookup<Cell> m_CellsFromEntity;
        private Block m_BlockData2;
        private ValidArea m_ValidAreaData2;
        private DynamicBuffer<Cell> m_Cells2;
        private NativeArray<float> m_BestDistance;

        public void Iterate(Entity blockEntity2)
        {
          this.m_ValidAreaData2 = this.m_ValidAreaDataFromEntity[blockEntity2];
          if (this.m_ValidAreaData2.m_Area.y <= this.m_ValidAreaData2.m_Area.x)
            return;
          this.m_BlockData2 = this.m_BlockDataFromEntity[blockEntity2];
          if (!MathUtils.Intersect(this.m_Bounds, ZoneUtils.CalculateBounds(this.m_BlockData2)))
            return;
          Quad2 corners = ZoneUtils.CalculateCorners(this.m_BlockData2, this.m_ValidAreaData2);
          if (!MathUtils.Intersect(MathUtils.Expand(this.m_Quad, -0.01f), MathUtils.Expand(corners, -0.01f)))
            return;
          this.m_Cells2 = this.m_CellsFromEntity[blockEntity2];
          this.CheckOverlapX1(this.m_Bounds, MathUtils.Bounds(corners), this.m_Quad, corners, this.m_ValidAreaData.m_Area, this.m_ValidAreaData2.m_Area);
        }

        public void Dispose()
        {
          if (!this.m_BestDistance.IsCreated)
            return;
          this.m_BestDistance.Dispose();
        }

        private void CheckOverlapX1(
          Bounds2 bounds1,
          Bounds2 bounds2,
          Quad2 quad1,
          Quad2 quad2,
          int4 xxzz1,
          int4 xxzz2)
        {
          if (xxzz1.y - xxzz1.x >= 2)
          {
            int4 xxzz1_1 = xxzz1;
            int4 xxzz1_2 = xxzz1;
            xxzz1_1.y = xxzz1.x + xxzz1.y >> 1;
            xxzz1_2.x = xxzz1_1.y;
            Quad2 quad2_1 = quad1;
            Quad2 quad2_2 = quad1;
            float s = (float) (xxzz1_1.y - xxzz1.x) / (float) (xxzz1.y - xxzz1.x);
            quad2_1.b = math.lerp(quad1.a, quad1.b, s);
            quad2_1.c = math.lerp(quad1.d, quad1.c, s);
            quad2_2.a = quad2_1.b;
            quad2_2.d = quad2_1.c;
            Bounds2 bounds1_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds1_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1_1, bounds2))
              this.CheckOverlapZ1(bounds1_1, bounds2, quad2_1, quad2, xxzz1_1, xxzz2);
            if (!MathUtils.Intersect(bounds1_2, bounds2))
              return;
            this.CheckOverlapZ1(bounds1_2, bounds2, quad2_2, quad2, xxzz1_2, xxzz2);
          }
          else
            this.CheckOverlapZ1(bounds1, bounds2, quad1, quad2, xxzz1, xxzz2);
        }

        private void CheckOverlapZ1(
          Bounds2 bounds1,
          Bounds2 bounds2,
          Quad2 quad1,
          Quad2 quad2,
          int4 xxzz1,
          int4 xxzz2)
        {
          if (xxzz1.w - xxzz1.z >= 2)
          {
            int4 xxzz1_1 = xxzz1;
            int4 xxzz1_2 = xxzz1;
            xxzz1_1.w = xxzz1.z + xxzz1.w >> 1;
            xxzz1_2.z = xxzz1_1.w;
            Quad2 quad2_1 = quad1;
            Quad2 quad2_2 = quad1;
            float s = (float) (xxzz1_1.w - xxzz1.z) / (float) (xxzz1.w - xxzz1.z);
            quad2_1.d = math.lerp(quad1.a, quad1.d, s);
            quad2_1.c = math.lerp(quad1.b, quad1.c, s);
            quad2_2.a = quad2_1.d;
            quad2_2.b = quad2_1.c;
            Bounds2 bounds1_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds1_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1_1, bounds2))
              this.CheckOverlapX2(bounds1_1, bounds2, quad2_1, quad2, xxzz1_1, xxzz2);
            if (!MathUtils.Intersect(bounds1_2, bounds2))
              return;
            this.CheckOverlapX2(bounds1_2, bounds2, quad2_2, quad2, xxzz1_2, xxzz2);
          }
          else
            this.CheckOverlapX2(bounds1, bounds2, quad1, quad2, xxzz1, xxzz2);
        }

        private void CheckOverlapX2(
          Bounds2 bounds1,
          Bounds2 bounds2,
          Quad2 quad1,
          Quad2 quad2,
          int4 xxzz1,
          int4 xxzz2)
        {
          if (xxzz2.y - xxzz2.x >= 2)
          {
            int4 xxzz2_1 = xxzz2;
            int4 xxzz2_2 = xxzz2;
            xxzz2_1.y = xxzz2.x + xxzz2.y >> 1;
            xxzz2_2.x = xxzz2_1.y;
            Quad2 quad2_1 = quad2;
            Quad2 quad2_2 = quad2;
            float s = (float) (xxzz2_1.y - xxzz2.x) / (float) (xxzz2.y - xxzz2.x);
            quad2_1.b = math.lerp(quad2.a, quad2.b, s);
            quad2_1.c = math.lerp(quad2.d, quad2.c, s);
            quad2_2.a = quad2_1.b;
            quad2_2.d = quad2_1.c;
            Bounds2 bounds2_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds2_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1, bounds2_1))
              this.CheckOverlapZ2(bounds1, bounds2_1, quad1, quad2_1, xxzz1, xxzz2_1);
            if (!MathUtils.Intersect(bounds1, bounds2_2))
              return;
            this.CheckOverlapZ2(bounds1, bounds2_2, quad1, quad2_2, xxzz1, xxzz2_2);
          }
          else
            this.CheckOverlapZ2(bounds1, bounds2, quad1, quad2, xxzz1, xxzz2);
        }

        private void CheckOverlapZ2(
          Bounds2 bounds1,
          Bounds2 bounds2,
          Quad2 quad1,
          Quad2 quad2,
          int4 xxzz1,
          int4 xxzz2)
        {
          if (xxzz2.w - xxzz2.z >= 2)
          {
            int4 xxzz2_1 = xxzz2;
            int4 xxzz2_2 = xxzz2;
            xxzz2_1.w = xxzz2.z + xxzz2.w >> 1;
            xxzz2_2.z = xxzz2_1.w;
            Quad2 quad2_1 = quad2;
            Quad2 quad2_2 = quad2;
            float s = (float) (xxzz2_1.w - xxzz2.z) / (float) (xxzz2.w - xxzz2.z);
            quad2_1.d = math.lerp(quad2.a, quad2.d, s);
            quad2_1.c = math.lerp(quad2.b, quad2.c, s);
            quad2_2.a = quad2_1.d;
            quad2_2.b = quad2_1.c;
            Bounds2 bounds2_1 = MathUtils.Bounds(quad2_1);
            Bounds2 bounds2_2 = MathUtils.Bounds(quad2_2);
            if (MathUtils.Intersect(bounds1, bounds2_1))
              this.CheckOverlapX1(bounds1, bounds2_1, quad1, quad2_1, xxzz1, xxzz2_1);
            if (!MathUtils.Intersect(bounds1, bounds2_2))
              return;
            this.CheckOverlapX1(bounds1, bounds2_2, quad1, quad2_2, xxzz1, xxzz2_2);
          }
          else if (math.any(xxzz1.yw - xxzz1.xz >= 2) | math.any(xxzz2.yw - xxzz2.xz >= 2))
          {
            this.CheckOverlapX1(bounds1, bounds2, quad1, quad2, xxzz1, xxzz2);
          }
          else
          {
            int index1 = xxzz1.z * this.m_BlockData.m_Size.x + xxzz1.x;
            int index2 = xxzz2.z * this.m_BlockData2.m_Size.x + xxzz2.x;
            Cell cell1 = this.m_Cells[index1];
            Cell cell2 = this.m_Cells2[index2];
            if ((cell1.m_State & CellFlags.Blocked) != CellFlags.None || (cell2.m_State & (CellFlags.Shared | CellFlags.Visible | CellFlags.Overridden)) != CellFlags.Visible || !cell1.m_Zone.Equals(ZoneType.None) || cell2.m_Zone.Equals(ZoneType.None))
              return;
            float num = math.lengthsq(MathUtils.Center(quad1) - MathUtils.Center(quad2));
            if ((double) num > 32.0)
              return;
            if (this.m_BestDistance.IsCreated)
            {
              if ((double) this.m_BestDistance[index1] <= (double) num)
                return;
            }
            else
            {
              this.m_BestDistance = new NativeArray<float>(this.m_BlockData.m_Size.x * this.m_BlockData.m_Size.y, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
              for (int index3 = 0; index3 < this.m_BestDistance.Length; ++index3)
                this.m_BestDistance[index3] = float.MaxValue;
            }
            cell1.m_Zone = cell2.m_Zone;
            this.m_Cells[index1] = cell1;
            this.m_BestDistance[index1] = num;
          }
        }
      }
    }
  }
}
