// Decompiled with JetBrains decompiler
// Type: Game.Zones.LotSizeJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  public static class LotSizeJobs
  {
    [BurstCompile]
    public struct UpdateLotSizeJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<CellCheckHelpers.SortedEntity> m_Blocks;
      [ReadOnly]
      public ZonePrefabs m_ZonePrefabs;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<ValidArea> m_ValidAreaData;
      [ReadOnly]
      public ComponentLookup<BuildOrder> m_BuildOrderData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      [NativeDisableParallelForRestriction]
      public BufferLookup<VacantLot> m_VacantLots;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<Bounds2>.ParallelWriter m_BoundsQueue;

      public void Execute(int index)
      {
        Entity entity = this.m_Blocks[index].m_Entity;
        Block block = this.m_BlockData[entity];
        ValidArea validArea = this.m_ValidAreaData[entity];
        BuildOrder buildOrder = this.m_BuildOrderData[entity];
        DynamicBuffer<Cell> cell1 = this.m_Cells[entity];
        DynamicBuffer<VacantLot> dynamicBuffer = new DynamicBuffer<VacantLot>();
        if (this.m_VacantLots.HasBuffer(entity))
        {
          dynamicBuffer = this.m_VacantLots[entity];
          dynamicBuffer.Clear();
        }
        NativeArray<Cell> cells = new NativeArray<Cell>();
        int2 expandedOffset = new int2();
        Block expandedBlock = new Block();
        int x1 = validArea.m_Area.x;
        while (x1 < validArea.m_Area.y)
        {
          Cell cell2 = cell1[validArea.m_Area.z * block.m_Size.x + x1];
          if ((cell2.m_State & (CellFlags.Blocked | CellFlags.Occupied)) == CellFlags.None && !cell2.m_Zone.Equals(ZoneType.None))
          {
            int2 min1 = new int2(x1, validArea.m_Area.z);
            int2 max1 = new int2();
            if (!cells.IsCreated)
            {
              this.FindDepth(block, cell1.AsNativeArray(), ref min1, ref max1, cell2.m_Zone);
              this.ExpandRight(block, cell1.AsNativeArray(), ref min1, ref max1, cell2.m_Zone);
              if (min1.x == 0 || math.any(max1 == block.m_Size))
                cells = this.ExpandArea(entity, block, validArea, buildOrder, cell1.AsNativeArray(), out expandedOffset, out expandedBlock);
            }
            int2 int2_1 = min1;
            int2 int2_2 = max1;
            ZoneData zoneData = this.m_ZoneData[this.m_ZonePrefabs[cell2.m_Zone]];
            Cell cell3;
            Cell cell4;
            if (cells.IsCreated)
            {
              min1 += expandedOffset;
              this.FindDepth(expandedBlock, cells, ref min1, ref max1, cell2.m_Zone);
              this.ExpandRight(expandedBlock, cells, ref min1, ref max1, cell2.m_Zone);
              if (x1 == 0)
                this.ExpandLeft(expandedBlock, cells, ref min1, ref max1, cell2.m_Zone);
              min1 -= expandedOffset;
              max1 -= expandedOffset;
              int2_1 = min1;
              int2_2 = max1;
              int sizeOffset = math.select(0, 1, (zoneData.m_ZoneFlags & ZoneFlags.SupportNarrow) == (ZoneFlags) 0);
              if (min1.x < -sizeOffset)
                this.WidthReductionLeft(block, ref min1, ref max1, sizeOffset);
              if (max1.x > block.m_Size.x + sizeOffset)
                this.WidthReductionRight(block, ref min1, ref max1, sizeOffset);
              int2 int2_3 = min1 + expandedOffset;
              int2 int2_4 = max1 + expandedOffset;
              cell3 = cells[int2_3.y * expandedBlock.m_Size.x + int2_3.x];
              cell4 = cells[int2_3.y * expandedBlock.m_Size.x + (int2_4.x - 1)];
            }
            else
            {
              cell3 = cell1[min1.y * block.m_Size.x + min1.x];
              cell4 = cell1[min1.y * block.m_Size.x + (max1.x - 1)];
            }
            LotFlags flags = (LotFlags) 0;
            if ((cell3.m_State & CellFlags.RoadLeft) != CellFlags.None)
              flags |= LotFlags.CornerLeft;
            if ((cell4.m_State & CellFlags.RoadRight) != CellFlags.None)
              flags |= LotFlags.CornerRight;
            int2 int2_5 = new int2(math.select(2, 1, (zoneData.m_ZoneFlags & ZoneFlags.SupportNarrow) != 0), 2);
            if (math.all(max1 - min1 >= int2_5))
            {
              if (!dynamicBuffer.IsCreated)
                dynamicBuffer = this.m_CommandBuffer.AddBuffer<VacantLot>(index, entity);
              if (max1.x - min1.x > 8)
              {
                int x2 = math.min(min1.x + int2_2.x + 1 >> 1, min1.x + 6 + 2);
                int x3 = math.max(int2_1.x + max1.x >> 1, max1.x - 6 - 2);
                int2 max2 = new int2(x2, max1.y);
                int2 min2 = new int2(x3, min1.y);
                int height1 = (int) cell2.m_Height;
                int height2 = (int) cell2.m_Height;
                this.FindHeight(block, cell1.AsNativeArray(), min1, max2, ref height1);
                this.FindHeight(block, cell1.AsNativeArray(), min1, max2, ref height2);
                if (cells.IsCreated)
                {
                  this.FindHeight(expandedBlock, cells, min1 + expandedOffset, max2 + expandedOffset, ref height1);
                  this.FindHeight(expandedBlock, cells, min2 + expandedOffset, max1 + expandedOffset, ref height2);
                }
                dynamicBuffer.Add(new VacantLot(min1, max2, cell2.m_Zone, height1, flags & ~LotFlags.CornerRight));
                dynamicBuffer.Add(new VacantLot(min2, max1, cell2.m_Zone, height2, flags & ~LotFlags.CornerLeft));
              }
              else
              {
                int height = (int) cell2.m_Height;
                this.FindHeight(block, cell1.AsNativeArray(), min1, max1, ref height);
                if (cells.IsCreated)
                  this.FindHeight(expandedBlock, cells, min1 + expandedOffset, max1 + expandedOffset, ref height);
                dynamicBuffer.Add(new VacantLot(min1, max1, cell2.m_Zone, height, flags));
              }
            }
            x1 = int2_2.x;
          }
          else
            ++x1;
        }
        if (dynamicBuffer.IsCreated && dynamicBuffer.Length == 0)
          this.m_CommandBuffer.RemoveComponent<VacantLot>(index, entity);
        if (cells.IsCreated)
          cells.Dispose();
        if (this.m_UpdatedData.HasComponent(entity))
          return;
        this.m_CommandBuffer.AddComponent<Updated>(index, entity, new Updated());
        this.m_BoundsQueue.Enqueue(ZoneUtils.CalculateBounds(block));
      }

      private void FindHeight(
        Block block,
        NativeArray<Cell> cells,
        int2 min,
        int2 max,
        ref int height)
      {
        min = math.max(min, (int2) 0);
        max = math.min(max, block.m_Size);
        for (int y = min.y; y < max.y; ++y)
        {
          for (int x = min.x; x < max.x; ++x)
          {
            int index = y * block.m_Size.x + x;
            Cell cell = cells[index];
            height = math.min(height, (int) cell.m_Height);
          }
        }
      }

      private void FindDepth(
        Block block,
        NativeArray<Cell> cells,
        ref int2 min,
        ref int2 max,
        ZoneType zone)
      {
        max.y = block.m_Size.y;
        for (int index1 = min.y + 1; index1 < block.m_Size.y; ++index1)
        {
          int index2 = index1 * block.m_Size.x + min.x;
          Cell cell = cells[index2];
          if ((cell.m_State & (CellFlags.Blocked | CellFlags.Occupied)) != CellFlags.None || !cell.m_Zone.Equals(zone))
          {
            max.y = index1;
            break;
          }
        }
        if (max.y <= 6)
          return;
        max.y = max.y + 1 >> 1;
      }

      private void ExpandRight(
        Block block,
        NativeArray<Cell> cells,
        ref int2 min,
        ref int2 max,
        ZoneType zone)
      {
        for (int index1 = min.x + 1; index1 < block.m_Size.x; ++index1)
        {
          for (int y = min.y; y < max.y; ++y)
          {
            int index2 = y * block.m_Size.x + index1;
            Cell cell = cells[index2];
            if ((cell.m_State & (CellFlags.Blocked | CellFlags.Occupied)) != CellFlags.None || !cell.m_Zone.Equals(zone))
            {
              max.x = index1;
              return;
            }
          }
        }
        max.x = block.m_Size.x;
      }

      private void ExpandLeft(
        Block block,
        NativeArray<Cell> cells,
        ref int2 min,
        ref int2 max,
        ZoneType zone)
      {
        for (int index1 = min.x - 1; index1 >= 0; --index1)
        {
          for (int y = min.y; y < max.y; ++y)
          {
            int index2 = y * block.m_Size.x + index1;
            Cell cell = cells[index2];
            if ((cell.m_State & (CellFlags.Blocked | CellFlags.Occupied)) != CellFlags.None || !cell.m_Zone.Equals(zone))
            {
              min.x = index1 + 1;
              return;
            }
          }
        }
        min.x = 0;
      }

      private void WidthReductionLeft(Block block, ref int2 min, ref int2 max, int sizeOffset)
      {
        int num = 3;
        if (max.x < num && min.x < -max.x)
        {
          min.x = max.x;
        }
        else
        {
          if (min.x > -num && max.x >= -min.x || max.x - min.x <= 6)
            return;
          min.x = math.min(-sizeOffset, min.x + max.x >> 1);
        }
      }

      private void WidthReductionRight(Block block, ref int2 min, ref int2 max, int sizeOffset)
      {
        int num = 3;
        if (min.x > block.m_Size.x - num && max.x - block.m_Size.x > block.m_Size.x - min.x)
        {
          max.x = min.x;
        }
        else
        {
          if (max.x - block.m_Size.x < num && block.m_Size.x - min.x >= max.x - block.m_Size.x || max.x - min.x <= 6)
            return;
          max.x = math.max(block.m_Size.x + sizeOffset, min.x + max.x + 1 >> 1);
        }
      }

      private NativeArray<Cell> ExpandArea(
        Entity entity,
        Block block,
        ValidArea validArea,
        BuildOrder buildOrder,
        NativeArray<Cell> cells,
        out int2 expandedOffset,
        out Block expandedBlock)
      {
        expandedBlock = block;
        expandedOffset.x = math.select(0, 6, validArea.m_Area.x == 0);
        expandedOffset.y = 0;
        expandedBlock.m_Size += expandedOffset + math.select(new int2(), (int2) 6, validArea.m_Area.yw == block.m_Size);
        float3 float3_1 = new float3(-block.m_Direction.y, 0.0f, block.m_Direction.x);
        float3 float3_2 = new float3(-block.m_Direction.x, 0.0f, -block.m_Direction.y);
        float2 float2 = (float2) (expandedBlock.m_Size - (expandedOffset << 1) - block.m_Size) * 4f;
        expandedBlock.m_Position += float3_1 * float2.x + float3_2 * float2.y;
        NativeArray<Cell> nativeArray = new NativeArray<Cell>(expandedBlock.m_Size.x * expandedBlock.m_Size.y, Allocator.Temp);
        int2 int2_1;
        for (int2_1.y = 0; int2_1.y < block.m_Size.y; ++int2_1.y)
        {
          for (int2_1.x = 0; int2_1.x < block.m_Size.x; ++int2_1.x)
          {
            int2 int2_2 = int2_1 + expandedOffset;
            int index1 = int2_1.y * block.m_Size.x + int2_1.x;
            int index2 = int2_2.y * expandedBlock.m_Size.x + int2_2.x;
            nativeArray[index2] = cells[index1];
          }
        }
        Quad2 corners = ZoneUtils.CalculateCorners(expandedBlock);
        LotSizeJobs.UpdateLotSizeJob.Iterator iterator = new LotSizeJobs.UpdateLotSizeJob.Iterator()
        {
          m_Entity = entity,
          m_Block = expandedBlock,
          m_ValidArea = validArea,
          m_BuildOrder = buildOrder,
          m_Bounds = MathUtils.Bounds(corners),
          m_Quad = corners,
          m_BlockData = this.m_BlockData,
          m_ValidAreaData = this.m_ValidAreaData,
          m_BuildOrderData = this.m_BuildOrderData,
          m_CellData = this.m_Cells,
          m_Cells = nativeArray
        };
        this.m_SearchTree.Iterate<LotSizeJobs.UpdateLotSizeJob.Iterator>(ref iterator);
        return nativeArray;
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Entity m_Entity;
        public Block m_Block;
        public ValidArea m_ValidArea;
        public BuildOrder m_BuildOrder;
        public Bounds2 m_Bounds;
        public Quad2 m_Quad;
        public ComponentLookup<Block> m_BlockData;
        public ComponentLookup<ValidArea> m_ValidAreaData;
        public ComponentLookup<BuildOrder> m_BuildOrderData;
        public BufferLookup<Cell> m_CellData;
        public NativeArray<Cell> m_Cells;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity entity2)
        {
          if (!MathUtils.Intersect(bounds, this.m_Bounds) || this.m_Entity.Equals(entity2))
            return;
          ValidArea validArea = this.m_ValidAreaData[entity2];
          if (validArea.m_Area.y <= validArea.m_Area.x)
            return;
          Block block = this.m_BlockData[entity2];
          if (!MathUtils.Intersect(this.m_Quad, MathUtils.Expand(ZoneUtils.CalculateCorners(block, validArea), -0.01f)))
            return;
          BuildOrder buildOrder2 = this.m_BuildOrderData[entity2];
          if (!ZoneUtils.CanShareCells(this.m_Block, block, this.m_BuildOrder, buildOrder2))
            return;
          int2 cellIndex = ZoneUtils.GetCellIndex(this.m_Block, ZoneUtils.GetCellPosition(block, new int2()).xz);
          float num1 = math.dot(this.m_Block.m_Direction, block.m_Direction);
          float num2 = math.dot(MathUtils.Left(this.m_Block.m_Direction), block.m_Direction);
          int2 x1;
          int2 x2;
          if ((double) num1 > 0.5)
          {
            x1 = new int2(1, 0);
            x2 = new int2(0, 1);
          }
          else if ((double) num1 < -0.5)
          {
            x1 = new int2(-1, 0);
            x2 = new int2(0, -1);
          }
          else if ((double) num2 > 0.5)
          {
            x1 = new int2(0, -1);
            x2 = new int2(1, 0);
          }
          else
          {
            x1 = new int2(0, 1);
            x2 = new int2(-1, 0);
          }
          DynamicBuffer<Cell> dynamicBuffer = this.m_CellData[entity2];
          int2 y;
          for (y.y = validArea.m_Area.z; y.y < validArea.m_Area.w; ++y.y)
          {
            for (y.x = validArea.m_Area.x; y.x < validArea.m_Area.y; ++y.x)
            {
              int2 int2 = cellIndex + new int2(math.dot(x1, y), math.dot(x2, y));
              if (!(math.any(int2 < 0) | math.any(int2 >= this.m_Block.m_Size)))
              {
                int index1 = y.y * block.m_Size.x + y.x;
                int index2 = int2.y * this.m_Block.m_Size.x + int2.x;
                Cell cell = dynamicBuffer[index1];
                if ((cell.m_State & (CellFlags.Blocked | CellFlags.Shared | CellFlags.Occupied | CellFlags.Redundant)) == CellFlags.None && !cell.m_Zone.Equals(ZoneType.None))
                {
                  if ((cell.m_State & (CellFlags.Roadside | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack)) != CellFlags.None)
                    cell.m_State = cell.m_State & ~(CellFlags.Roadside | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack) | ZoneUtils.GetRoadDirection(this.m_Block, block, cell.m_State);
                  this.m_Cells[index2] = cell;
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    public struct UpdateBoundsJob : IJob
    {
      public NativeQueue<Bounds2> m_BoundsQueue;
      public NativeList<Bounds2> m_BoundsList;

      public void Execute()
      {
        int count = this.m_BoundsQueue.Count;
        if (count == 0)
          return;
        this.m_BoundsList.Capacity = math.max(this.m_BoundsList.Capacity, this.m_BoundsList.Length + count);
        for (int index = 0; index < count; ++index)
          this.m_BoundsList.Add(this.m_BoundsQueue.Dequeue());
      }
    }
  }
}
