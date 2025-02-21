// Decompiled with JetBrains decompiler
// Type: Game.Zones.CellBlockJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Net;
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
  public static class CellBlockJobs
  {
    [BurstCompile]
    public struct BlockCellsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<CellCheckHelpers.SortedEntity> m_Blocks;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<RoadComposition> m_PrefabRoadCompositionData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Cell> m_Cells;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ValidArea> m_ValidAreaData;

      public void Execute(int index)
      {
        Entity entity = this.m_Blocks[index].m_Entity;
        Block block = this.m_BlockData[entity];
        DynamicBuffer<Cell> cell = this.m_Cells[entity];
        ValidArea validAreaData = new ValidArea();
        validAreaData.m_Area = new int4(0, block.m_Size.x, 0, block.m_Size.y);
        Bounds2 bounds = ZoneUtils.CalculateBounds(block);
        Quad2 corners = ZoneUtils.CalculateCorners(block);
        CellBlockJobs.BlockCellsJob.ClearBlockStatus(block, cell);
        CellBlockJobs.BlockCellsJob.NetIterator iterator1 = new CellBlockJobs.BlockCellsJob.NetIterator()
        {
          m_BlockEntity = entity,
          m_BlockData = block,
          m_Bounds = bounds,
          m_Quad = corners,
          m_ValidAreaData = validAreaData,
          m_Cells = cell,
          m_OwnerData = this.m_OwnerData,
          m_TransformData = this.m_TransformData,
          m_EdgeGeometryData = this.m_EdgeGeometryData,
          m_StartNodeGeometryData = this.m_StartNodeGeometryData,
          m_EndNodeGeometryData = this.m_EndNodeGeometryData,
          m_CompositionData = this.m_CompositionData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabCompositionData = this.m_PrefabCompositionData,
          m_PrefabRoadCompositionData = this.m_PrefabRoadCompositionData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData
        };
        this.m_NetSearchTree.Iterate<CellBlockJobs.BlockCellsJob.NetIterator>(ref iterator1);
        CellBlockJobs.BlockCellsJob.AreaIterator iterator2 = new CellBlockJobs.BlockCellsJob.AreaIterator()
        {
          m_BlockEntity = entity,
          m_BlockData = block,
          m_Bounds = bounds,
          m_Quad = corners,
          m_ValidAreaData = validAreaData,
          m_Cells = cell,
          m_NativeData = this.m_NativeData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabAreaGeometryData = this.m_PrefabAreaGeometryData,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles
        };
        this.m_AreaSearchTree.Iterate<CellBlockJobs.BlockCellsJob.AreaIterator>(ref iterator2);
        CellBlockJobs.BlockCellsJob.CleanBlockedCells(block, ref validAreaData, cell);
        this.m_ValidAreaData[entity] = validAreaData;
      }

      private static void ClearBlockStatus(Block blockData, DynamicBuffer<Cell> cells)
      {
        for (int index = 0; index < blockData.m_Size.x; ++index)
        {
          Cell cell = cells[index];
          if ((cell.m_State & (CellFlags.Blocked | CellFlags.Shared | CellFlags.Roadside | CellFlags.Occupied | CellFlags.Updating | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack)) != (CellFlags.Roadside | CellFlags.Updating))
          {
            cell.m_State = cell.m_State & ~(CellFlags.Blocked | CellFlags.Shared | CellFlags.Roadside | CellFlags.Occupied | CellFlags.Updating | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack) | CellFlags.Roadside | CellFlags.Updating;
            cell.m_Height = short.MaxValue;
            cells[index] = cell;
          }
        }
        for (int index1 = 1; index1 < blockData.m_Size.y; ++index1)
        {
          for (int index2 = 0; index2 < blockData.m_Size.x; ++index2)
          {
            int index3 = index1 * blockData.m_Size.x + index2;
            Cell cell = cells[index3];
            if ((cell.m_State & (CellFlags.Blocked | CellFlags.Shared | CellFlags.Roadside | CellFlags.Occupied | CellFlags.Updating | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack)) != CellFlags.Updating)
            {
              cell.m_State = cell.m_State & ~(CellFlags.Blocked | CellFlags.Shared | CellFlags.Roadside | CellFlags.Occupied | CellFlags.Updating | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack) | CellFlags.Updating;
              cell.m_Height = short.MaxValue;
              cells[index3] = cell;
            }
          }
        }
      }

      private static void CleanBlockedCells(
        Block blockData,
        ref ValidArea validAreaData,
        DynamicBuffer<Cell> cells)
      {
        ValidArea validArea = new ValidArea();
        validArea.m_Area.xz = blockData.m_Size;
        for (int x = validAreaData.m_Area.x; x < validAreaData.m_Area.y; ++x)
        {
          Cell cell1 = cells[x];
          Cell cell2 = cells[blockData.m_Size.x + x];
          if ((cell1.m_State & CellFlags.Blocked) == CellFlags.None & (cell2.m_State & CellFlags.Blocked) != 0)
          {
            cell1.m_State |= CellFlags.Blocked;
            cells[x] = cell1;
          }
          int y = 0;
          for (int index1 = validAreaData.m_Area.z + 1; index1 < validAreaData.m_Area.w; ++index1)
          {
            int index2 = index1 * blockData.m_Size.x + x;
            Cell cell3 = cells[index2];
            if ((cell3.m_State & CellFlags.Blocked) == CellFlags.None & (cell1.m_State & CellFlags.Blocked) != 0)
            {
              cell3.m_State |= CellFlags.Blocked;
              cells[index2] = cell3;
            }
            if ((cell3.m_State & CellFlags.Blocked) == CellFlags.None)
              y = index1 + 1;
            cell1 = cell3;
          }
          if (y > validAreaData.m_Area.z)
          {
            validArea.m_Area.xz = math.min(validArea.m_Area.xz, new int2(x, validAreaData.m_Area.z));
            validArea.m_Area.yw = math.max(validArea.m_Area.yw, new int2(x + 1, y));
          }
        }
        validAreaData = validArea;
        for (int z = validAreaData.m_Area.z; z < validAreaData.m_Area.w; ++z)
        {
          for (int x = validAreaData.m_Area.x; x < validAreaData.m_Area.y; ++x)
          {
            int index = z * blockData.m_Size.x + x;
            Cell cell = cells[index];
            if ((cell.m_State & (CellFlags.Blocked | CellFlags.RoadLeft)) == CellFlags.None && x > 0 && (cells[index - 1].m_State & (CellFlags.Blocked | CellFlags.RoadLeft)) == (CellFlags.Blocked | CellFlags.RoadLeft))
            {
              cell.m_State |= CellFlags.RoadLeft;
              cells[index] = cell;
            }
            if ((cell.m_State & (CellFlags.Blocked | CellFlags.RoadRight)) == CellFlags.None && x < blockData.m_Size.x - 1 && (cells[index + 1].m_State & (CellFlags.Blocked | CellFlags.RoadRight)) == (CellFlags.Blocked | CellFlags.RoadRight))
            {
              cell.m_State |= CellFlags.RoadRight;
              cells[index] = cell;
            }
          }
        }
      }

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Entity m_BlockEntity;
        public Block m_BlockData;
        public ValidArea m_ValidAreaData;
        public Bounds2 m_Bounds;
        public Quad2 m_Quad;
        public Quad2 m_IgnoreQuad;
        public Circle2 m_IgnoreCircle;
        public bool2 m_HasIgnore;
        public DynamicBuffer<Cell> m_Cells;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
        public ComponentLookup<RoadComposition> m_PrefabRoadCompositionData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_EdgeGeometryData.HasComponent(edgeEntity))
            return;
          this.m_HasIgnore = (bool2) false;
          if (this.m_OwnerData.HasComponent(edgeEntity))
          {
            Owner owner = this.m_OwnerData[edgeEntity];
            if (this.m_TransformData.HasComponent(owner.m_Owner))
            {
              PrefabRef prefabRef = this.m_PrefabRefData[owner.m_Owner];
              if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
              {
                Game.Objects.Transform transform = this.m_TransformData[owner.m_Owner];
                ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
                if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
                {
                  this.m_IgnoreCircle = new Circle2(math.max(objectGeometryData.m_Size - 0.16f, (float3) 0.0f).x * 0.5f, transform.m_Position.xz);
                  this.m_HasIgnore.y = true;
                }
                else
                {
                  Bounds3 bounds1 = MathUtils.Expand(objectGeometryData.m_Bounds, (float3) -0.08f);
                  float3 b = MathUtils.Center(bounds1);
                  bool3 c = bounds1.min > bounds1.max;
                  bounds1.min = math.select(bounds1.min, b, c);
                  bounds1.max = math.select(bounds1.max, b, c);
                  this.m_IgnoreQuad = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, bounds1).xz;
                  this.m_HasIgnore.x = true;
                }
              }
            }
          }
          Composition composition = this.m_CompositionData[edgeEntity];
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeEntity];
          StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[edgeEntity];
          EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[edgeEntity];
          if (MathUtils.Intersect(this.m_Bounds, edgeGeometry.m_Bounds.xz))
          {
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            RoadComposition prefabRoadData = new RoadComposition();
            if (this.m_PrefabRoadCompositionData.HasComponent(composition.m_Edge))
              prefabRoadData = this.m_PrefabRoadCompositionData[composition.m_Edge];
            this.CheckSegment(edgeGeometry.m_Start.m_Left, edgeGeometry.m_Start.m_Right, prefabCompositionData, prefabRoadData, new bool2(true, true));
            this.CheckSegment(edgeGeometry.m_End.m_Left, edgeGeometry.m_End.m_Right, prefabCompositionData, prefabRoadData, new bool2(true, true));
          }
          if (MathUtils.Intersect(this.m_Bounds, startNodeGeometry.m_Geometry.m_Bounds.xz))
          {
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_StartNode];
            RoadComposition prefabRoadData = new RoadComposition();
            if (this.m_PrefabRoadCompositionData.HasComponent(composition.m_StartNode))
              prefabRoadData = this.m_PrefabRoadCompositionData[composition.m_StartNode];
            if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
            {
              this.CheckSegment(startNodeGeometry.m_Geometry.m_Left.m_Left, startNodeGeometry.m_Geometry.m_Left.m_Right, prefabCompositionData, prefabRoadData, new bool2(true, true));
              Bezier4x3 bezier4x3 = MathUtils.Lerp(startNodeGeometry.m_Geometry.m_Right.m_Left, startNodeGeometry.m_Geometry.m_Right.m_Right, 0.5f) with
              {
                d = startNodeGeometry.m_Geometry.m_Middle.d
              };
              this.CheckSegment(startNodeGeometry.m_Geometry.m_Right.m_Left, bezier4x3, prefabCompositionData, prefabRoadData, new bool2(true, false));
              this.CheckSegment(bezier4x3, startNodeGeometry.m_Geometry.m_Right.m_Right, prefabCompositionData, prefabRoadData, new bool2(false, true));
            }
            else
            {
              this.CheckSegment(startNodeGeometry.m_Geometry.m_Left.m_Left, startNodeGeometry.m_Geometry.m_Middle, prefabCompositionData, prefabRoadData, new bool2(true, false));
              this.CheckSegment(startNodeGeometry.m_Geometry.m_Middle, startNodeGeometry.m_Geometry.m_Right.m_Right, prefabCompositionData, prefabRoadData, new bool2(false, true));
            }
          }
          if (!MathUtils.Intersect(this.m_Bounds, endNodeGeometry.m_Geometry.m_Bounds.xz))
            return;
          NetCompositionData prefabCompositionData1 = this.m_PrefabCompositionData[composition.m_EndNode];
          RoadComposition prefabRoadData1 = new RoadComposition();
          if (this.m_PrefabRoadCompositionData.HasComponent(composition.m_EndNode))
            prefabRoadData1 = this.m_PrefabRoadCompositionData[composition.m_EndNode];
          if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
          {
            this.CheckSegment(endNodeGeometry.m_Geometry.m_Left.m_Left, endNodeGeometry.m_Geometry.m_Left.m_Right, prefabCompositionData1, prefabRoadData1, new bool2(true, true));
            Bezier4x3 bezier4x3 = MathUtils.Lerp(endNodeGeometry.m_Geometry.m_Right.m_Left, endNodeGeometry.m_Geometry.m_Right.m_Right, 0.5f) with
            {
              d = endNodeGeometry.m_Geometry.m_Middle.d
            };
            this.CheckSegment(endNodeGeometry.m_Geometry.m_Right.m_Left, bezier4x3, prefabCompositionData1, prefabRoadData1, new bool2(true, false));
            this.CheckSegment(bezier4x3, endNodeGeometry.m_Geometry.m_Right.m_Right, prefabCompositionData1, prefabRoadData1, new bool2(false, true));
          }
          else
          {
            this.CheckSegment(endNodeGeometry.m_Geometry.m_Left.m_Left, endNodeGeometry.m_Geometry.m_Middle, prefabCompositionData1, prefabRoadData1, new bool2(true, false));
            this.CheckSegment(endNodeGeometry.m_Geometry.m_Middle, endNodeGeometry.m_Geometry.m_Right.m_Right, prefabCompositionData1, prefabRoadData1, new bool2(false, true));
          }
        }

        private void CheckSegment(
          Bezier4x3 left,
          Bezier4x3 right,
          NetCompositionData prefabCompositionData,
          RoadComposition prefabRoadData,
          bool2 isEdge)
        {
          if ((prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0 || (prefabCompositionData.m_State & CompositionState.BlockZone) == (CompositionState) 0)
            return;
          bool isElevated = (prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Elevated) > (CompositionFlags.General) 0 | (prefabCompositionData.m_State & CompositionState.ExclusiveGround) == (CompositionState) 0;
          if (!MathUtils.Intersect((MathUtils.Bounds(left) | MathUtils.Bounds(right)).xz, this.m_Bounds))
            return;
          isEdge &= (prefabRoadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0 & (prefabCompositionData.m_Flags.m_General & CompositionFlags.General.Elevated) == (CompositionFlags.General) 0;
          isEdge &= new bool2((prefabCompositionData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0, (prefabCompositionData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0);
          Quad3 quad2;
          quad2.a = left.a;
          quad2.b = right.a;
          Bounds3 bounds3_1 = CellBlockJobs.BlockCellsJob.NetIterator.SetHeightRange(MathUtils.Bounds(quad2.a, quad2.b), prefabCompositionData.m_HeightRange);
          for (int index = 1; index <= 8; ++index)
          {
            float t = (float) index / 8f;
            quad2.d = MathUtils.Position(left, t);
            quad2.c = MathUtils.Position(right, t);
            Bounds3 bounds3_2 = CellBlockJobs.BlockCellsJob.NetIterator.SetHeightRange(MathUtils.Bounds(quad2.d, quad2.c), prefabCompositionData.m_HeightRange);
            Bounds3 bounds2 = bounds3_1 | bounds3_2;
            if (MathUtils.Intersect(bounds2.xz, this.m_Bounds) && MathUtils.Intersect(this.m_Quad, quad2.xz))
            {
              CellFlags flags = CellFlags.Blocked;
              if (isEdge.x)
              {
                Block source = new Block()
                {
                  m_Direction = math.normalizesafe(MathUtils.Right(quad2.d.xz - quad2.a.xz))
                };
                flags |= ZoneUtils.GetRoadDirection(this.m_BlockData, source);
              }
              if (isEdge.y)
              {
                Block source = new Block()
                {
                  m_Direction = math.normalizesafe(MathUtils.Left(quad2.c.xz - quad2.b.xz))
                };
                flags |= ZoneUtils.GetRoadDirection(this.m_BlockData, source);
              }
              this.CheckOverlapX(this.m_Bounds, bounds2, this.m_Quad, quad2, this.m_ValidAreaData.m_Area, flags, isElevated);
            }
            quad2.a = quad2.d;
            quad2.b = quad2.c;
            bounds3_1 = bounds3_2;
          }
        }

        private static Bounds3 SetHeightRange(Bounds3 bounds, Bounds1 heightRange)
        {
          bounds.min.y += heightRange.min;
          bounds.max.y += heightRange.max;
          return bounds;
        }

        private void CheckOverlapX(
          Bounds2 bounds1,
          Bounds3 bounds2,
          Quad2 quad1,
          Quad3 quad2,
          int4 xxzz1,
          CellFlags flags,
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
              this.CheckOverlapZ(bounds1_1, bounds2, quad2_1, quad2, xxzz1_1, flags, isElevated);
            if (!MathUtils.Intersect(bounds1_2, bounds2.xz))
              return;
            this.CheckOverlapZ(bounds1_2, bounds2, quad2_2, quad2, xxzz1_2, flags, isElevated);
          }
          else
            this.CheckOverlapZ(bounds1, bounds2, quad1, quad2, xxzz1, flags, isElevated);
        }

        private void CheckOverlapZ(
          Bounds2 bounds1,
          Bounds3 bounds2,
          Quad2 quad1,
          Quad3 quad2,
          int4 xxzz1,
          CellFlags flags,
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
              this.CheckOverlapX(bounds1_1, bounds2, quad2_1, quad2, xxzz1_1, flags, isElevated);
            if (!MathUtils.Intersect(bounds1_2, bounds2.xz))
              return;
            this.CheckOverlapX(bounds1_2, bounds2, quad2_2, quad2, xxzz1_2, flags, isElevated);
          }
          else if (xxzz1.y - xxzz1.x >= 2)
          {
            this.CheckOverlapX(bounds1, bounds2, quad1, quad2, xxzz1, flags, isElevated);
          }
          else
          {
            int index = xxzz1.z * this.m_BlockData.m_Size.x + xxzz1.x;
            Cell cell = this.m_Cells[index];
            if ((cell.m_State & flags) == flags)
              return;
            quad1 = MathUtils.Expand(quad1, -1f / 16f);
            if (!MathUtils.Intersect(quad1, quad2.xz) || math.any(this.m_HasIgnore) && (this.m_HasIgnore.x && MathUtils.Intersect(quad1, this.m_IgnoreQuad) || this.m_HasIgnore.y && MathUtils.Intersect(quad1, this.m_IgnoreCircle)))
              return;
            if (isElevated)
              cell.m_Height = (short) math.clamp(Mathf.FloorToInt(bounds2.min.y), (int) short.MinValue, math.min((int) cell.m_Height, (int) short.MaxValue));
            else
              cell.m_State |= flags;
            this.m_Cells[index] = cell;
          }
        }
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Entity m_BlockEntity;
        public Block m_BlockData;
        public ValidArea m_ValidAreaData;
        public Bounds2 m_Bounds;
        public Quad2 m_Quad;
        public DynamicBuffer<Cell> m_Cells;
        public ComponentLookup<Native> m_NativeData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
            return;
          AreaGeometryData areaGeometryData = this.m_PrefabAreaGeometryData[this.m_PrefabRefData[areaItem.m_Area].m_Prefab];
          if ((areaGeometryData.m_Flags & (Game.Areas.GeometryFlags.PhysicalGeometry | Game.Areas.GeometryFlags.ProtectedArea)) == (Game.Areas.GeometryFlags) 0 || (areaGeometryData.m_Flags & Game.Areas.GeometryFlags.ProtectedArea) != (Game.Areas.GeometryFlags) 0 && !this.m_NativeData.HasComponent(areaItem.m_Area))
            return;
          DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[areaItem.m_Area];
          DynamicBuffer<Triangle> areaTriangle = this.m_AreaTriangles[areaItem.m_Area];
          if (areaTriangle.Length <= areaItem.m_Triangle)
            return;
          Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, areaTriangle[areaItem.m_Triangle]);
          this.CheckOverlapX(this.m_Bounds, bounds.m_Bounds.xz, this.m_Quad, triangle3.xz, this.m_ValidAreaData.m_Area);
        }

        private void CheckOverlapX(
          Bounds2 bounds1,
          Bounds2 bounds2,
          Quad2 quad1,
          Triangle2 triangle2,
          int4 xxzz1)
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
              this.CheckOverlapZ(bounds1_1, bounds2, quad2_1, triangle2, xxzz1_1);
            if (!MathUtils.Intersect(bounds1_2, bounds2))
              return;
            this.CheckOverlapZ(bounds1_2, bounds2, quad2_2, triangle2, xxzz1_2);
          }
          else
            this.CheckOverlapZ(bounds1, bounds2, quad1, triangle2, xxzz1);
        }

        private void CheckOverlapZ(
          Bounds2 bounds1,
          Bounds2 bounds2,
          Quad2 quad1,
          Triangle2 triangle2,
          int4 xxzz1)
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
              this.CheckOverlapX(bounds1_1, bounds2, quad2_1, triangle2, xxzz1_1);
            if (!MathUtils.Intersect(bounds1_2, bounds2))
              return;
            this.CheckOverlapX(bounds1_2, bounds2, quad2_2, triangle2, xxzz1_2);
          }
          else if (xxzz1.y - xxzz1.x >= 2)
          {
            this.CheckOverlapX(bounds1, bounds2, quad1, triangle2, xxzz1);
          }
          else
          {
            int index = xxzz1.z * this.m_BlockData.m_Size.x + xxzz1.x;
            Cell cell = this.m_Cells[index];
            if ((cell.m_State & CellFlags.Blocked) != CellFlags.None)
              return;
            quad1 = MathUtils.Expand(quad1, -0.02f);
            if (!MathUtils.Intersect(quad1, triangle2))
              return;
            cell.m_State |= CellFlags.Blocked;
            this.m_Cells[index] = cell;
          }
        }
      }
    }
  }
}
