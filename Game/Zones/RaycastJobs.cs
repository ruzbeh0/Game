// Decompiled with JetBrains decompiler
// Type: Game.Zones.RaycastJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  public static class RaycastJobs
  {
    [BurstCompile]
    public struct FindZoneBlockJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      [ReadOnly]
      public NativeArray<RaycastResult> m_TerrainResults;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        int index1 = index % this.m_Input.Length;
        RaycastInput raycastInput = this.m_Input[index1];
        RaycastResult terrainResult = this.m_TerrainResults[index];
        if ((raycastInput.m_TypeMask & TypeMask.Zones) == TypeMask.None || terrainResult.m_Owner == Entity.Null)
          return;
        RaycastJobs.FindZoneBlockJob.Iterator iterator = new RaycastJobs.FindZoneBlockJob.Iterator()
        {
          m_Position = terrainResult.m_Hit.m_HitPosition.xz,
          m_BlockData = this.m_BlockData,
          m_Cells = this.m_Cells
        };
        this.m_SearchTree.Iterate<RaycastJobs.FindZoneBlockJob.Iterator>(ref iterator);
        if (!(iterator.m_Block != Entity.Null))
          return;
        terrainResult.m_Owner = iterator.m_Block;
        terrainResult.m_Hit.m_CellIndex = iterator.m_CellIndex;
        terrainResult.m_Hit.m_NormalizedDistance -= 1f / math.max(1f, MathUtils.Length(raycastInput.m_Line));
        this.m_Results.Accumulate(index1, terrainResult);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public float2 m_Position;
        public Entity m_Block;
        public int2 m_CellIndex;
        public ComponentLookup<Block> m_BlockData;
        public BufferLookup<Cell> m_Cells;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Position);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          if (!MathUtils.Intersect(bounds, this.m_Position))
            return;
          Block block = this.m_BlockData[blockEntity];
          int2 cellIndex = ZoneUtils.GetCellIndex(block, this.m_Position);
          if (!math.all(cellIndex >= 0 & cellIndex < block.m_Size) || (this.m_Cells[blockEntity][cellIndex.y * block.m_Size.x + cellIndex.x].m_State & (CellFlags.Shared | CellFlags.Visible)) != CellFlags.Visible)
            return;
          this.m_Block = blockEntity;
          this.m_CellIndex = cellIndex;
        }
      }
    }
  }
}
