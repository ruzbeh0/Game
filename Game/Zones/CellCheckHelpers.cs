// Decompiled with JetBrains decompiler
// Type: Game.Zones.CellCheckHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  public static class CellCheckHelpers
  {
    public struct SortedEntity : IComparable<CellCheckHelpers.SortedEntity>
    {
      public Entity m_Entity;

      public int CompareTo(CellCheckHelpers.SortedEntity other)
      {
        return this.m_Entity.Index - other.m_Entity.Index;
      }
    }

    public struct BlockOverlap : IComparable<CellCheckHelpers.BlockOverlap>
    {
      public int m_Group;
      public uint m_Priority;
      public Entity m_Block;
      public Entity m_Other;
      public Entity m_Left;
      public Entity m_Right;

      public int CompareTo(CellCheckHelpers.BlockOverlap other)
      {
        int b1 = this.m_Group - other.m_Group;
        int b2 = math.select(math.select(0, 1, this.m_Priority > other.m_Priority), -1, this.m_Priority < other.m_Priority);
        return math.select(math.select(this.m_Block.Index - other.m_Block.Index, b2, b2 != 0), b1, b1 != 0);
      }
    }

    public struct OverlapGroup
    {
      public int m_StartIndex;
      public int m_EndIndex;
    }

    [BurstCompile]
    public struct FindUpdatedBlocksSingleIterationJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        CellCheckHelpers.FindUpdatedBlocksSingleIterationJob.Iterator iterator = new CellCheckHelpers.FindUpdatedBlocksSingleIterationJob.Iterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_ResultQueue = this.m_ResultQueue
        };
        this.m_SearchTree.Iterate<CellCheckHelpers.FindUpdatedBlocksSingleIterationJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          this.m_ResultQueue.Enqueue(blockEntity);
        }
      }
    }

    [BurstCompile]
    public struct FindUpdatedBlocksDoubleIterationJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob.FirstIterator iterator1 = new CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob.FirstIterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_ResultBounds = new Bounds2((float2) float.MaxValue, (float2) float.MinValue)
        };
        this.m_SearchTree.Iterate<CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob.FirstIterator>(ref iterator1);
        CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob.SecondIterator iterator2 = new CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob.SecondIterator()
        {
          m_Bounds = iterator1.m_ResultBounds,
          m_ResultQueue = this.m_ResultQueue
        };
        this.m_SearchTree.Iterate<CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob.SecondIterator>(ref iterator2);
      }

      private struct FirstIterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public Bounds2 m_ResultBounds;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          this.m_ResultBounds |= bounds;
        }
      }

      private struct SecondIterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          this.m_ResultQueue.Enqueue(blockEntity);
        }
      }
    }

    [BurstCompile]
    public struct CollectBlocksJob : IJob
    {
      public NativeQueue<Entity> m_Queue1;
      public NativeQueue<Entity> m_Queue2;
      public NativeQueue<Entity> m_Queue3;
      public NativeQueue<Entity> m_Queue4;
      public NativeList<CellCheckHelpers.SortedEntity> m_ResultList;

      public void Execute()
      {
        this.ProcessQueue(this.m_Queue1);
        this.ProcessQueue(this.m_Queue2);
        this.ProcessQueue(this.m_Queue3);
        this.ProcessQueue(this.m_Queue4);
        this.RemoveDuplicates();
      }

      private void ProcessQueue(NativeQueue<Entity> queue)
      {
        Entity entity;
        while (queue.TryDequeue(out entity))
          this.m_ResultList.Add(new CellCheckHelpers.SortedEntity()
          {
            m_Entity = entity
          });
      }

      private void RemoveDuplicates()
      {
        this.m_ResultList.Sort<CellCheckHelpers.SortedEntity>();
        int index1 = 0;
        int index2 = 0;
        while (index1 < this.m_ResultList.Length)
        {
          CellCheckHelpers.SortedEntity result = this.m_ResultList[index1++];
          while (index1 < this.m_ResultList.Length && this.m_ResultList[index1].m_Entity.Equals(result.m_Entity))
            ++index1;
          this.m_ResultList[index2++] = result;
        }
        if (index2 >= this.m_ResultList.Length)
          return;
        this.m_ResultList.RemoveRange(index2, this.m_ResultList.Length - index2);
      }
    }

    [BurstCompile]
    public struct FindOverlappingBlocksJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<CellCheckHelpers.SortedEntity> m_Blocks;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<ValidArea> m_ValidAreaData;
      [ReadOnly]
      public ComponentLookup<BuildOrder> m_BuildOrderData;
      public NativeQueue<CellCheckHelpers.BlockOverlap>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        Entity entity = this.m_Blocks[index].m_Entity;
        Block block = this.m_BlockData[entity];
        ValidArea validArea = this.m_ValidAreaData[entity];
        BuildOrder buildOrder = this.m_BuildOrderData[entity];
        if (validArea.m_Area.y <= validArea.m_Area.x)
          return;
        CellCheckHelpers.FindOverlappingBlocksJob.Iterator iterator = new CellCheckHelpers.FindOverlappingBlocksJob.Iterator()
        {
          m_BlockEntity = entity,
          m_BlockData = block,
          m_ValidAreaData = validArea,
          m_BuildOrderData = buildOrder,
          m_Bounds = MathUtils.Expand(ZoneUtils.CalculateBounds(block), (float2) 1.6f),
          m_Quad = MathUtils.Expand(ZoneUtils.CalculateCorners(block, validArea), -0.01f),
          m_BlockDataFromEntity = this.m_BlockData,
          m_ValidAreaDataEntity = this.m_ValidAreaData,
          m_BuildOrderDataEntity = this.m_BuildOrderData,
          m_ResultQueue = this.m_ResultQueue
        };
        this.m_SearchTree.Iterate<CellCheckHelpers.FindOverlappingBlocksJob.Iterator>(ref iterator);
        if (iterator.m_OverlapCount != 0)
          return;
        this.m_ResultQueue.Enqueue(new CellCheckHelpers.BlockOverlap()
        {
          m_Priority = buildOrder.m_Order,
          m_Block = entity
        });
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Entity m_BlockEntity;
        public Block m_BlockData;
        public ValidArea m_ValidAreaData;
        public BuildOrder m_BuildOrderData;
        public Bounds2 m_Bounds;
        public Quad2 m_Quad;
        public int m_OverlapCount;
        public ComponentLookup<Block> m_BlockDataFromEntity;
        public ComponentLookup<ValidArea> m_ValidAreaDataEntity;
        public ComponentLookup<BuildOrder> m_BuildOrderDataEntity;
        public NativeQueue<CellCheckHelpers.BlockOverlap>.ParallelWriter m_ResultQueue;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity2)
        {
          if (!MathUtils.Intersect(bounds, this.m_Bounds) || this.m_BlockEntity.Equals(blockEntity2))
            return;
          Block block = this.m_BlockDataFromEntity[blockEntity2];
          ValidArea validArea = this.m_ValidAreaDataEntity[blockEntity2];
          BuildOrder buildOrder2 = this.m_BuildOrderDataEntity[blockEntity2];
          if (validArea.m_Area.y <= validArea.m_Area.x)
            return;
          if (!MathUtils.Intersect(this.m_Quad, MathUtils.Expand(ZoneUtils.CalculateCorners(block, validArea), -0.01f)))
          {
            if (!ZoneUtils.IsNeighbor(this.m_BlockData, block, this.m_BuildOrderData, buildOrder2))
              return;
            if ((double) math.dot(block.m_Position.xz - this.m_BlockData.m_Position.xz, MathUtils.Right(this.m_BlockData.m_Direction)) > 0.0)
            {
              if (this.m_ValidAreaData.m_Area.x != 0 | validArea.m_Area.y != block.m_Size.x)
                return;
            }
            else if (this.m_ValidAreaData.m_Area.y != this.m_BlockData.m_Size.x | validArea.m_Area.x != 0)
              return;
          }
          this.m_ResultQueue.Enqueue(new CellCheckHelpers.BlockOverlap()
          {
            m_Priority = this.m_BuildOrderData.m_Order,
            m_Block = this.m_BlockEntity,
            m_Other = blockEntity2
          });
          ++this.m_OverlapCount;
        }
      }
    }

    [BurstCompile]
    public struct GroupOverlappingBlocksJob : IJob
    {
      [ReadOnly]
      public NativeArray<CellCheckHelpers.SortedEntity> m_Blocks;
      public NativeQueue<CellCheckHelpers.BlockOverlap> m_OverlapQueue;
      public NativeList<CellCheckHelpers.BlockOverlap> m_BlockOverlaps;
      public NativeList<CellCheckHelpers.OverlapGroup> m_OverlapGroups;

      public void Execute()
      {
        NativeParallelHashMap<Entity, int> nativeParallelHashMap = new NativeParallelHashMap<Entity, int>(this.m_Blocks.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<int> groups = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        CellCheckHelpers.BlockOverlap blockOverlap;
        while (this.m_OverlapQueue.TryDequeue(out blockOverlap))
        {
          int num1;
          if (nativeParallelHashMap.TryGetValue(blockOverlap.m_Block, out num1))
          {
            num1 = groups[num1];
            if (blockOverlap.m_Other != Entity.Null)
            {
              int num2;
              if (nativeParallelHashMap.TryGetValue(blockOverlap.m_Other, out num2))
              {
                num2 = groups[num2];
                if (num1 != num2)
                  num1 = this.MergeGroups(groups, num1, num2);
              }
              else
                nativeParallelHashMap.TryAdd(blockOverlap.m_Other, num1);
            }
          }
          else if (blockOverlap.m_Other != Entity.Null)
          {
            if (nativeParallelHashMap.TryGetValue(blockOverlap.m_Other, out num1))
            {
              num1 = groups[num1];
              nativeParallelHashMap.TryAdd(blockOverlap.m_Block, num1);
            }
            else
            {
              num1 = this.CreateGroup(groups);
              nativeParallelHashMap.TryAdd(blockOverlap.m_Block, num1);
              nativeParallelHashMap.TryAdd(blockOverlap.m_Other, num1);
            }
          }
          else
          {
            num1 = this.CreateGroup(groups);
            nativeParallelHashMap.TryAdd(blockOverlap.m_Block, num1);
          }
          blockOverlap.m_Group = num1;
          this.m_BlockOverlaps.Add(in blockOverlap);
        }
        if (this.m_BlockOverlaps.Length != 0)
        {
          for (int index = 0; index < groups.Length; ++index)
            groups[index] = groups[groups[index]];
          for (int index = 0; index < this.m_BlockOverlaps.Length; ++index)
          {
            blockOverlap = this.m_BlockOverlaps[index];
            blockOverlap.m_Group = groups[blockOverlap.m_Group];
            this.m_BlockOverlaps[index] = blockOverlap;
          }
          this.m_BlockOverlaps.Sort<CellCheckHelpers.BlockOverlap>();
          CellCheckHelpers.OverlapGroup overlapGroup = new CellCheckHelpers.OverlapGroup();
          overlapGroup.m_StartIndex = 0;
          int num = this.m_BlockOverlaps[0].m_Group;
          for (int index = 0; index < this.m_BlockOverlaps.Length; ++index)
          {
            int group = this.m_BlockOverlaps[index].m_Group;
            if (group != num)
            {
              overlapGroup.m_EndIndex = index;
              this.m_OverlapGroups.Add(in overlapGroup);
              overlapGroup.m_StartIndex = index;
              num = group;
            }
          }
          overlapGroup.m_EndIndex = this.m_BlockOverlaps.Length;
          this.m_OverlapGroups.Add(in overlapGroup);
        }
        groups.Dispose();
        nativeParallelHashMap.Dispose();
      }

      private int CreateGroup(NativeList<int> groups)
      {
        int length = groups.Length;
        groups.Add(in length);
        return length;
      }

      private int MergeGroups(NativeList<int> groups, int group1, int group2)
      {
        int num = math.min(group1, group2);
        groups[math.max(group1, group2)] = num;
        return num;
      }
    }

    [BurstCompile]
    public struct UpdateBlocksJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<CellCheckHelpers.SortedEntity> m_Blocks;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Cell> m_Cells;

      public void Execute(int index)
      {
        Entity entity = this.m_Blocks[index].m_Entity;
        this.SetVisible(this.m_BlockData[entity], this.m_Cells[entity]);
      }

      private void SetVisible(Block blockData, DynamicBuffer<Cell> cells)
      {
        for (int index1 = 0; index1 < blockData.m_Size.y; ++index1)
        {
          for (int index2 = 0; index2 < blockData.m_Size.x; ++index2)
          {
            int index3 = index1 * blockData.m_Size.x + index2;
            Cell cell = cells[index3];
            if ((cell.m_State & (CellFlags.Blocked | CellFlags.Redundant)) != CellFlags.None)
              cell.m_State &= ~(CellFlags.Shared | CellFlags.Visible);
            else
              cell.m_State |= CellFlags.Visible;
            cell.m_State &= ~CellFlags.Updating;
            cells[index3] = cell;
          }
        }
      }
    }
  }
}
