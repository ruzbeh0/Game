// Decompiled with JetBrains decompiler
// Type: Game.Zones.CellOverlapJobs
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

#nullable disable
namespace Game.Zones
{
  public static class CellOverlapJobs
  {
    [BurstCompile]
    public struct CheckBlockOverlapJob : IJobParallelForDefer
    {
      [NativeDisableParallelForRestriction]
      public NativeArray<CellCheckHelpers.BlockOverlap> m_BlockOverlaps;
      [ReadOnly]
      public NativeArray<CellCheckHelpers.OverlapGroup> m_OverlapGroups;
      [ReadOnly]
      public ZonePrefabs m_ZonePrefabs;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<BuildOrder> m_BuildOrderData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Cell> m_Cells;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ValidArea> m_ValidAreaData;

      public void Execute(int index)
      {
        CellCheckHelpers.OverlapGroup overlapGroup = this.m_OverlapGroups[index];
        CellCheckHelpers.BlockOverlap blockOverlap1 = new CellCheckHelpers.BlockOverlap();
        int index1 = 0;
        Block block1 = new Block();
        BuildOrder buildOrder1 = new BuildOrder();
        for (int startIndex = overlapGroup.m_StartIndex; startIndex < overlapGroup.m_EndIndex; ++startIndex)
        {
          CellCheckHelpers.BlockOverlap blockOverlap2 = this.m_BlockOverlaps[startIndex];
          if (blockOverlap2.m_Block != blockOverlap1.m_Block)
          {
            if (blockOverlap1.m_Block != Entity.Null)
              this.m_BlockOverlaps[index1] = blockOverlap1;
            blockOverlap1 = blockOverlap2;
            index1 = startIndex;
            block1 = this.m_BlockData[blockOverlap2.m_Block];
            ValidArea validArea = this.m_ValidAreaData[blockOverlap2.m_Block];
            buildOrder1 = this.m_BuildOrderData[blockOverlap2.m_Block];
            DynamicBuffer<Cell> cell = this.m_Cells[blockOverlap2.m_Block];
          }
          if (blockOverlap2.m_Other != Entity.Null)
          {
            Block block2 = this.m_BlockData[blockOverlap2.m_Other];
            BuildOrder buildOrder2 = this.m_BuildOrderData[blockOverlap2.m_Other];
            if (ZoneUtils.IsNeighbor(block1, block2, buildOrder1, buildOrder2))
            {
              if ((double) math.dot(block2.m_Position.xz - block1.m_Position.xz, MathUtils.Right(block1.m_Direction)) > 0.0)
                blockOverlap1.m_Left = blockOverlap2.m_Other;
              else
                blockOverlap1.m_Right = blockOverlap2.m_Other;
            }
          }
        }
        if (blockOverlap1.m_Block != Entity.Null)
          this.m_BlockOverlaps[index1] = blockOverlap1;
        CellOverlapJobs.CheckBlockOverlapJob.OverlapIterator overlapIterator1 = new CellOverlapJobs.CheckBlockOverlapJob.OverlapIterator();
        overlapIterator1.m_BlockDataFromEntity = this.m_BlockData;
        overlapIterator1.m_ValidAreaDataFromEntity = this.m_ValidAreaData;
        overlapIterator1.m_BuildOrderDataFromEntity = this.m_BuildOrderData;
        overlapIterator1.m_CellsFromEntity = this.m_Cells;
        CellOverlapJobs.CheckBlockOverlapJob.CellReduction cellReduction1 = new CellOverlapJobs.CheckBlockOverlapJob.CellReduction();
        cellReduction1.m_BlockDataFromEntity = this.m_BlockData;
        cellReduction1.m_ValidAreaDataFromEntity = this.m_ValidAreaData;
        cellReduction1.m_BuildOrderDataFromEntity = this.m_BuildOrderData;
        cellReduction1.m_CellsFromEntity = this.m_Cells;
        cellReduction1.m_Flag = CellFlags.Redundant;
        for (int startIndex = overlapGroup.m_StartIndex; startIndex < overlapGroup.m_EndIndex; ++startIndex)
        {
          CellCheckHelpers.BlockOverlap blockOverlap3 = this.m_BlockOverlaps[startIndex];
          if (blockOverlap3.m_Block != overlapIterator1.m_BlockEntity)
          {
            if (cellReduction1.m_BlockEntity != Entity.Null)
              cellReduction1.Perform();
            cellReduction1.m_BlockEntity = blockOverlap3.m_Block;
            cellReduction1.m_LeftNeightbor = blockOverlap3.m_Left;
            cellReduction1.m_RightNeightbor = blockOverlap3.m_Right;
            overlapIterator1.m_BlockEntity = blockOverlap3.m_Block;
            overlapIterator1.m_BlockData = this.m_BlockData[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_ValidAreaData = this.m_ValidAreaData[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_BuildOrderData = this.m_BuildOrderData[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_Cells = this.m_Cells[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_Quad = ZoneUtils.CalculateCorners(overlapIterator1.m_BlockData, overlapIterator1.m_ValidAreaData);
            overlapIterator1.m_Bounds = MathUtils.Bounds(overlapIterator1.m_Quad);
          }
          if (overlapIterator1.m_ValidAreaData.m_Area.y > overlapIterator1.m_ValidAreaData.m_Area.x && blockOverlap3.m_Other != Entity.Null)
            overlapIterator1.Iterate(blockOverlap3.m_Other);
        }
        if (cellReduction1.m_BlockEntity != Entity.Null)
          cellReduction1.Perform();
        overlapIterator1.m_BlockEntity = Entity.Null;
        overlapIterator1.m_CheckBlocking = true;
        cellReduction1.m_BlockEntity = Entity.Null;
        for (int startIndex = overlapGroup.m_StartIndex; startIndex < overlapGroup.m_EndIndex; ++startIndex)
        {
          CellCheckHelpers.BlockOverlap blockOverlap4 = this.m_BlockOverlaps[startIndex];
          if (blockOverlap4.m_Block != overlapIterator1.m_BlockEntity)
          {
            if (cellReduction1.m_BlockEntity != Entity.Null)
            {
              cellReduction1.m_Flag = CellFlags.Redundant;
              cellReduction1.Clear();
              cellReduction1.m_Flag = CellFlags.Blocked;
              cellReduction1.Perform();
            }
            cellReduction1.m_BlockEntity = blockOverlap4.m_Block;
            cellReduction1.m_LeftNeightbor = blockOverlap4.m_Left;
            cellReduction1.m_RightNeightbor = blockOverlap4.m_Right;
            overlapIterator1.m_BlockEntity = blockOverlap4.m_Block;
            overlapIterator1.m_BlockData = this.m_BlockData[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_ValidAreaData = this.m_ValidAreaData[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_BuildOrderData = this.m_BuildOrderData[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_Cells = this.m_Cells[overlapIterator1.m_BlockEntity];
            overlapIterator1.m_Quad = ZoneUtils.CalculateCorners(overlapIterator1.m_BlockData, overlapIterator1.m_ValidAreaData);
            overlapIterator1.m_Bounds = MathUtils.Bounds(overlapIterator1.m_Quad);
          }
          if (overlapIterator1.m_ValidAreaData.m_Area.y > overlapIterator1.m_ValidAreaData.m_Area.x && blockOverlap4.m_Other != Entity.Null)
            overlapIterator1.Iterate(blockOverlap4.m_Other);
        }
        if (cellReduction1.m_BlockEntity != Entity.Null)
        {
          cellReduction1.m_Flag = CellFlags.Redundant;
          cellReduction1.Clear();
          cellReduction1.m_Flag = CellFlags.Blocked;
          cellReduction1.Perform();
        }
        CellOverlapJobs.CheckBlockOverlapJob.CellReduction cellReduction2 = new CellOverlapJobs.CheckBlockOverlapJob.CellReduction();
        cellReduction2.m_BlockDataFromEntity = this.m_BlockData;
        cellReduction2.m_ValidAreaDataFromEntity = this.m_ValidAreaData;
        cellReduction2.m_BuildOrderDataFromEntity = this.m_BuildOrderData;
        cellReduction2.m_CellsFromEntity = this.m_Cells;
        cellReduction2.m_Flag = CellFlags.Redundant;
        for (int startIndex = overlapGroup.m_StartIndex; startIndex < overlapGroup.m_EndIndex; ++startIndex)
        {
          CellCheckHelpers.BlockOverlap blockOverlap5 = this.m_BlockOverlaps[startIndex];
          if (blockOverlap5.m_Block != cellReduction2.m_BlockEntity)
          {
            cellReduction2.m_BlockEntity = blockOverlap5.m_Block;
            cellReduction2.m_LeftNeightbor = blockOverlap5.m_Left;
            cellReduction2.m_RightNeightbor = blockOverlap5.m_Right;
            cellReduction2.Perform();
          }
        }
        CellOverlapJobs.CheckBlockOverlapJob.CellReduction cellReduction3 = new CellOverlapJobs.CheckBlockOverlapJob.CellReduction();
        cellReduction3.m_ZonePrefabs = this.m_ZonePrefabs;
        cellReduction3.m_BlockDataFromEntity = this.m_BlockData;
        cellReduction3.m_ValidAreaDataFromEntity = this.m_ValidAreaData;
        cellReduction3.m_BuildOrderDataFromEntity = this.m_BuildOrderData;
        cellReduction3.m_ZoneData = this.m_ZoneData;
        cellReduction3.m_CellsFromEntity = this.m_Cells;
        cellReduction3.m_Flag = CellFlags.Occupied;
        for (int startIndex = overlapGroup.m_StartIndex; startIndex < overlapGroup.m_EndIndex; ++startIndex)
        {
          CellCheckHelpers.BlockOverlap blockOverlap6 = this.m_BlockOverlaps[startIndex];
          if (blockOverlap6.m_Block != cellReduction3.m_BlockEntity)
          {
            cellReduction3.m_BlockEntity = blockOverlap6.m_Block;
            cellReduction3.m_LeftNeightbor = blockOverlap6.m_Left;
            cellReduction3.m_RightNeightbor = blockOverlap6.m_Right;
            cellReduction3.Perform();
          }
        }
        CellOverlapJobs.CheckBlockOverlapJob.OverlapIterator overlapIterator2 = new CellOverlapJobs.CheckBlockOverlapJob.OverlapIterator();
        overlapIterator2.m_BlockDataFromEntity = this.m_BlockData;
        overlapIterator2.m_ValidAreaDataFromEntity = this.m_ValidAreaData;
        overlapIterator2.m_BuildOrderDataFromEntity = this.m_BuildOrderData;
        overlapIterator2.m_CellsFromEntity = this.m_Cells;
        overlapIterator2.m_CheckSharing = true;
        for (int startIndex = overlapGroup.m_StartIndex; startIndex < overlapGroup.m_EndIndex; ++startIndex)
        {
          CellCheckHelpers.BlockOverlap blockOverlap7 = this.m_BlockOverlaps[startIndex];
          if (blockOverlap7.m_Block != overlapIterator2.m_BlockEntity)
          {
            overlapIterator2.m_BlockEntity = blockOverlap7.m_Block;
            overlapIterator2.m_BlockData = this.m_BlockData[overlapIterator2.m_BlockEntity];
            overlapIterator2.m_ValidAreaData = this.m_ValidAreaData[overlapIterator2.m_BlockEntity];
            overlapIterator2.m_BuildOrderData = this.m_BuildOrderData[overlapIterator2.m_BlockEntity];
            overlapIterator2.m_Cells = this.m_Cells[overlapIterator2.m_BlockEntity];
            overlapIterator2.m_Quad = ZoneUtils.CalculateCorners(overlapIterator2.m_BlockData, overlapIterator2.m_ValidAreaData);
            overlapIterator2.m_Bounds = MathUtils.Bounds(overlapIterator2.m_Quad);
          }
          if (overlapIterator2.m_ValidAreaData.m_Area.y > overlapIterator2.m_ValidAreaData.m_Area.x && blockOverlap7.m_Other != Entity.Null)
            overlapIterator2.Iterate(blockOverlap7.m_Other);
        }
      }

      private struct CellReduction
      {
        public Entity m_BlockEntity;
        public Entity m_LeftNeightbor;
        public Entity m_RightNeightbor;
        public CellFlags m_Flag;
        public ZonePrefabs m_ZonePrefabs;
        public ComponentLookup<Block> m_BlockDataFromEntity;
        public ComponentLookup<ValidArea> m_ValidAreaDataFromEntity;
        public ComponentLookup<BuildOrder> m_BuildOrderDataFromEntity;
        public ComponentLookup<ZoneData> m_ZoneData;
        public BufferLookup<Cell> m_CellsFromEntity;
        private Block m_BlockData;
        private Block m_LeftBlockData;
        private Block m_RightBlockData;
        private ValidArea m_ValidAreaData;
        private ValidArea m_LeftValidAreaData;
        private ValidArea m_RightValidAreaData;
        private BuildOrder m_BuildOrderData;
        private BuildOrder m_LeftBuildOrderData;
        private BuildOrder m_RightBuildOrderData;
        private DynamicBuffer<Cell> m_Cells;
        private DynamicBuffer<Cell> m_LeftCells;
        private DynamicBuffer<Cell> m_RightCells;

        public void Clear()
        {
          this.m_BlockData = this.m_BlockDataFromEntity[this.m_BlockEntity];
          this.m_ValidAreaData = this.m_ValidAreaDataFromEntity[this.m_BlockEntity];
          this.m_Cells = this.m_CellsFromEntity[this.m_BlockEntity];
          for (int x = this.m_ValidAreaData.m_Area.x; x < this.m_ValidAreaData.m_Area.y; ++x)
          {
            for (int z = this.m_ValidAreaData.m_Area.z; z < this.m_ValidAreaData.m_Area.w; ++z)
            {
              int index = z * this.m_BlockData.m_Size.x + x;
              Cell cell = this.m_Cells[index];
              if ((cell.m_State & this.m_Flag) != CellFlags.None)
              {
                cell.m_State &= ~this.m_Flag;
                this.m_Cells[index] = cell;
              }
            }
          }
        }

        public void Perform()
        {
          this.m_BlockData = this.m_BlockDataFromEntity[this.m_BlockEntity];
          this.m_ValidAreaData = this.m_ValidAreaDataFromEntity[this.m_BlockEntity];
          this.m_BuildOrderData = this.m_BuildOrderDataFromEntity[this.m_BlockEntity];
          this.m_Cells = this.m_CellsFromEntity[this.m_BlockEntity];
          if (this.m_LeftNeightbor != Entity.Null)
          {
            this.m_LeftBlockData = this.m_BlockDataFromEntity[this.m_LeftNeightbor];
            this.m_LeftValidAreaData = this.m_ValidAreaDataFromEntity[this.m_LeftNeightbor];
            this.m_LeftBuildOrderData = this.m_BuildOrderDataFromEntity[this.m_LeftNeightbor];
            this.m_LeftCells = this.m_CellsFromEntity[this.m_LeftNeightbor];
          }
          else
            this.m_LeftBlockData = new Block();
          if (this.m_RightNeightbor != Entity.Null)
          {
            this.m_RightBlockData = this.m_BlockDataFromEntity[this.m_RightNeightbor];
            this.m_RightValidAreaData = this.m_ValidAreaDataFromEntity[this.m_RightNeightbor];
            this.m_RightBuildOrderData = this.m_BuildOrderDataFromEntity[this.m_RightNeightbor];
            this.m_RightCells = this.m_CellsFromEntity[this.m_RightNeightbor];
          }
          else
            this.m_RightBlockData = new Block();
          CellFlags cellFlags = this.m_Flag | CellFlags.Blocked;
          for (int x = this.m_ValidAreaData.m_Area.x; x < this.m_ValidAreaData.m_Area.y; ++x)
          {
            Cell cell1 = this.m_Cells[x];
            Cell cell2 = this.m_Cells[this.m_BlockData.m_Size.x + x];
            if ((cell1.m_State & cellFlags) == CellFlags.None & (cell2.m_State & cellFlags) == this.m_Flag)
            {
              cell1.m_State |= this.m_Flag;
              this.m_Cells[x] = cell1;
            }
            for (int index1 = this.m_ValidAreaData.m_Area.z + 1; index1 < this.m_ValidAreaData.m_Area.w; ++index1)
            {
              int index2 = index1 * this.m_BlockData.m_Size.x + x;
              Cell cell3 = this.m_Cells[index2];
              if ((cell3.m_State & cellFlags) == CellFlags.None & (cell1.m_State & cellFlags) == this.m_Flag)
              {
                cell3.m_State |= this.m_Flag;
                this.m_Cells[index2] = cell3;
              }
              cell1 = cell3;
            }
          }
          int x1 = this.m_ValidAreaData.m_Area.x;
          int num = this.m_ValidAreaData.m_Area.y - 1;
          ValidArea validArea = new ValidArea();
          validArea.m_Area.xz = this.m_BlockData.m_Size;
          for (; num >= this.m_ValidAreaData.m_Area.x; --num)
          {
            if (this.m_Flag == CellFlags.Occupied)
            {
              Cell cell4 = this.m_Cells[x1];
              Cell cell5 = this.m_Cells[num];
              Entity zonePrefab1 = this.m_ZonePrefabs[cell4.m_Zone];
              Entity zonePrefab2 = this.m_ZonePrefabs[cell5.m_Zone];
              ZoneData zoneData1 = this.m_ZoneData[zonePrefab1];
              ZoneData zoneData2 = this.m_ZoneData[zonePrefab2];
              if ((zoneData1.m_ZoneFlags & ZoneFlags.SupportNarrow) == (ZoneFlags) 0)
              {
                int leftDepth = this.CalculateLeftDepth(x1, cell4.m_Zone);
                this.ReduceDepth(x1, leftDepth);
              }
              if ((zoneData2.m_ZoneFlags & ZoneFlags.SupportNarrow) == (ZoneFlags) 0)
              {
                int rightDepth = this.CalculateRightDepth(num, cell5.m_Zone);
                this.ReduceDepth(num, rightDepth);
              }
            }
            else
            {
              int leftDepth = this.CalculateLeftDepth(x1, ZoneType.None);
              this.ReduceDepth(x1, leftDepth);
              int rightDepth = this.CalculateRightDepth(num, ZoneType.None);
              this.ReduceDepth(num, rightDepth);
              if (num <= x1 && this.m_Flag == CellFlags.Blocked)
              {
                if (leftDepth != 0 && x1 != num)
                {
                  validArea.m_Area.xz = math.min(validArea.m_Area.xz, new int2(x1, this.m_ValidAreaData.m_Area.z));
                  validArea.m_Area.yw = math.max(validArea.m_Area.yw, new int2(x1 + 1, leftDepth));
                }
                if (rightDepth != 0)
                {
                  validArea.m_Area.xz = math.min(validArea.m_Area.xz, new int2(num, this.m_ValidAreaData.m_Area.z));
                  validArea.m_Area.yw = math.max(validArea.m_Area.yw, new int2(num + 1, rightDepth));
                }
              }
            }
            ++x1;
          }
          if (this.m_Flag != CellFlags.Blocked)
            return;
          this.m_ValidAreaDataFromEntity[this.m_BlockEntity] = validArea;
        }

        private int CalculateLeftDepth(int x, ZoneType zoneType)
        {
          int depth1 = this.GetDepth(x - 1, zoneType);
          int depth2 = this.GetDepth(x, zoneType);
          if (depth2 <= depth1)
            return depth2;
          int depth3 = this.GetDepth(x - 2, zoneType);
          if (depth1 != depth3 & depth1 != 0)
            return depth1;
          int depth4 = this.GetDepth(x + 1, zoneType);
          return depth4 - depth2 < depth2 - depth1 || this.GetDepth(x + 2, zoneType) != depth4 ? math.min(math.max(depth1, depth4), depth2) : depth1;
        }

        private int CalculateRightDepth(int x, ZoneType zoneType)
        {
          int depth1 = this.GetDepth(x + 1, zoneType);
          int depth2 = this.GetDepth(x, zoneType);
          if (depth2 <= depth1)
            return depth2;
          int depth3 = this.GetDepth(x + 2, zoneType);
          if (depth1 != depth3 & depth1 != 0)
            return depth1;
          int depth4 = this.GetDepth(x - 1, zoneType);
          return depth4 - depth2 < depth2 - depth1 || this.GetDepth(x - 2, zoneType) != depth4 ? math.min(math.max(depth4, depth1), depth2) : depth1;
        }

        private int GetDepth(int x, ZoneType zoneType)
        {
          if (x < 0)
          {
            x += this.m_LeftBlockData.m_Size.x;
            if (x < 0)
              return 0;
            return this.m_BuildOrderData.m_Order < this.m_LeftBuildOrderData.m_Order & this.m_Flag == CellFlags.Blocked ? this.GetDepth(this.m_BlockData, this.m_ValidAreaData, this.m_Cells, 0, this.m_Flag | CellFlags.Blocked, zoneType) : this.GetDepth(this.m_LeftBlockData, this.m_LeftValidAreaData, this.m_LeftCells, x, this.m_Flag | CellFlags.Blocked, zoneType);
          }
          if (x < this.m_BlockData.m_Size.x)
            return this.GetDepth(this.m_BlockData, this.m_ValidAreaData, this.m_Cells, x, this.m_Flag | CellFlags.Blocked, zoneType);
          x -= this.m_BlockData.m_Size.x;
          if (x >= this.m_RightBlockData.m_Size.x)
            return 0;
          return this.m_BuildOrderData.m_Order < this.m_RightBuildOrderData.m_Order & this.m_Flag == CellFlags.Blocked ? this.GetDepth(this.m_BlockData, this.m_ValidAreaData, this.m_Cells, this.m_BlockData.m_Size.x - 1, this.m_Flag | CellFlags.Blocked, zoneType) : this.GetDepth(this.m_RightBlockData, this.m_RightValidAreaData, this.m_RightCells, x, this.m_Flag | CellFlags.Blocked, zoneType);
        }

        private int GetDepth(
          Block blockData,
          ValidArea validAreaData,
          DynamicBuffer<Cell> cells,
          int x,
          CellFlags flags,
          ZoneType zoneType)
        {
          int z = validAreaData.m_Area.z;
          int index = x;
          if (this.m_Flag == CellFlags.Occupied)
          {
            for (; z < validAreaData.m_Area.w && (cells[index].m_State & flags) == CellFlags.None && cells[index].m_Zone.Equals(zoneType); ++z)
              index += blockData.m_Size.x;
          }
          else
          {
            for (; z < validAreaData.m_Area.w && (cells[index].m_State & flags) == CellFlags.None; ++z)
              index += blockData.m_Size.x;
          }
          return z;
        }

        private void ReduceDepth(int x, int newDepth)
        {
          CellFlags cellFlags = this.m_Flag | CellFlags.Blocked;
          int index1 = this.m_BlockData.m_Size.x * newDepth + x;
          for (int index2 = newDepth; index2 < this.m_ValidAreaData.m_Area.w; ++index2)
          {
            Cell cell = this.m_Cells[index1];
            if ((cell.m_State & cellFlags) != CellFlags.None)
              break;
            cell.m_State |= this.m_Flag;
            this.m_Cells[index1] = cell;
            index1 += this.m_BlockData.m_Size.x;
          }
        }
      }

      private struct OverlapIterator
      {
        public Entity m_BlockEntity;
        public Quad2 m_Quad;
        public Bounds2 m_Bounds;
        public Block m_BlockData;
        public ValidArea m_ValidAreaData;
        public BuildOrder m_BuildOrderData;
        public DynamicBuffer<Cell> m_Cells;
        public ComponentLookup<Block> m_BlockDataFromEntity;
        public ComponentLookup<ValidArea> m_ValidAreaDataFromEntity;
        public ComponentLookup<BuildOrder> m_BuildOrderDataFromEntity;
        public BufferLookup<Cell> m_CellsFromEntity;
        public bool m_CheckSharing;
        public bool m_CheckBlocking;
        public bool m_CheckDepth;
        private Block m_BlockData2;
        private ValidArea m_ValidAreaData2;
        private BuildOrder m_BuildOrderData2;
        private DynamicBuffer<Cell> m_Cells2;

        public void Iterate(Entity blockEntity2)
        {
          this.m_BlockData2 = this.m_BlockDataFromEntity[blockEntity2];
          this.m_ValidAreaData2 = this.m_ValidAreaDataFromEntity[blockEntity2];
          this.m_BuildOrderData2 = this.m_BuildOrderDataFromEntity[blockEntity2];
          this.m_Cells2 = this.m_CellsFromEntity[blockEntity2];
          if (this.m_ValidAreaData2.m_Area.y <= this.m_ValidAreaData2.m_Area.x)
            return;
          if (ZoneUtils.CanShareCells(this.m_BlockData, this.m_BlockData2, this.m_BuildOrderData, this.m_BuildOrderData2))
          {
            if (!this.m_CheckSharing)
              return;
            this.m_CheckDepth = false;
          }
          else
          {
            if (this.m_CheckSharing)
              return;
            this.m_CheckDepth = (double) math.dot(this.m_BlockData.m_Direction, this.m_BlockData2.m_Direction) < -0.69465839862823486;
          }
          Quad2 corners = ZoneUtils.CalculateCorners(this.m_BlockData2, this.m_ValidAreaData2);
          this.CheckOverlapX1(this.m_Bounds, MathUtils.Bounds(corners), this.m_Quad, corners, this.m_ValidAreaData.m_Area, this.m_ValidAreaData2.m_Area);
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
            Cell cell = this.m_Cells[index1];
            Cell cell2 = this.m_Cells2[index2];
            if (((cell.m_State | cell2.m_State) & CellFlags.Blocked) != CellFlags.None)
              return;
            if (this.m_CheckSharing)
            {
              if ((double) math.lengthsq(MathUtils.Center(quad1) - MathUtils.Center(quad2)) >= 16.0)
                return;
              if (this.CheckPriority(cell, cell2, xxzz1.z, xxzz2.z, this.m_BuildOrderData.m_Order, this.m_BuildOrderData2.m_Order) && (cell2.m_State & CellFlags.Shared) == CellFlags.None)
              {
                cell.m_State |= CellFlags.Shared;
                cell.m_State = cell.m_State & ~CellFlags.Overridden | cell2.m_State & CellFlags.Overridden;
                cell.m_Zone = cell2.m_Zone;
              }
              if ((cell2.m_State & CellFlags.Roadside) != CellFlags.None && xxzz2.z == 0)
                cell.m_State |= ZoneUtils.GetRoadDirection(this.m_BlockData, this.m_BlockData2);
              cell.m_State &= ~CellFlags.Occupied | cell2.m_State & CellFlags.Occupied;
              this.m_Cells[index1] = cell;
            }
            else if (this.CheckPriority(cell, cell2, xxzz1.z, xxzz2.z, this.m_BuildOrderData.m_Order, this.m_BuildOrderData2.m_Order))
            {
              quad1 = MathUtils.Expand(quad1, -0.01f);
              quad2 = MathUtils.Expand(quad2, -0.01f);
              if (!MathUtils.Intersect(quad1, quad2))
                return;
              cell.m_State = cell.m_State & ~CellFlags.Shared | (this.m_CheckBlocking ? CellFlags.Blocked : CellFlags.Redundant);
              this.m_Cells[index1] = cell;
            }
            else
            {
              if ((double) math.lengthsq(MathUtils.Center(quad1) - MathUtils.Center(quad2)) >= 64.0 || (cell2.m_State & CellFlags.Roadside) == CellFlags.None || xxzz2.z != 0)
                return;
              cell.m_State |= ZoneUtils.GetRoadDirection(this.m_BlockData, this.m_BlockData2);
              this.m_Cells[index1] = cell;
            }
          }
        }

        private bool CheckPriority(
          Cell cell1,
          Cell cell2,
          int depth1,
          int depth2,
          uint order1,
          uint order2)
        {
          if ((cell2.m_State & CellFlags.Updating) == CellFlags.None)
            return (cell2.m_State & CellFlags.Visible) != 0;
          if (this.m_CheckBlocking)
            return (cell1.m_State & ~cell2.m_State & CellFlags.Redundant) != 0;
          if (this.m_CheckDepth)
          {
            if (cell1.m_Zone.Equals(ZoneType.None) != cell2.m_Zone.Equals(ZoneType.None))
              return cell1.m_Zone.Equals(ZoneType.None);
            if (cell1.m_Zone.Equals(ZoneType.None) && ((cell1.m_State | cell2.m_State) & CellFlags.Overridden) == CellFlags.None && math.max(0, depth1 - 1) != math.max(0, depth2 - 1))
              return depth2 < depth1;
          }
          return ((cell1.m_State ^ cell2.m_State) & CellFlags.Visible) != CellFlags.None ? (cell2.m_State & CellFlags.Visible) != 0 : order2 < order1;
        }
      }
    }
  }
}
