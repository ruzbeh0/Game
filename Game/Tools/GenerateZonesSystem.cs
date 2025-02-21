// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateZonesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateZonesSystem : GameSystemBase
  {
    private SearchSystem m_ZoneSearchSystem;
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private GenerateZonesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<Zoning>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeParallelMultiHashMap<Entity, GenerateZonesSystem.CellData> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, GenerateZonesSystem.CellData>(1000, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>(20, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Zoning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateZonesSystem.FillBlocksListJob jobData1 = new GenerateZonesSystem.FillBlocksListJob()
      {
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_ZoningType = this.__TypeHandle.__Game_Tools_Zoning_RO_ComponentTypeHandle,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_SearchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies),
        m_ZonedCells = parallelMultiHashMap,
        m_ZonedBlocks = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneBlockData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateZonesSystem.CreateBlocksJob jobData2 = new GenerateZonesSystem.CreateBlocksJob()
      {
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ZoneBlockDataData = this.__TypeHandle.__Game_Prefabs_ZoneBlockData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_ZonedCells = parallelMultiHashMap,
        m_ZonedBlocks = nativeList.AsDeferredJobArray(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.Schedule<GenerateZonesSystem.FillBlocksListJob>(this.m_DefinitionQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.Schedule<GenerateZonesSystem.CreateBlocksJob, Entity>(list, 1, dependsOn);
      parallelMultiHashMap.Dispose(jobHandle2);
      nativeList.Dispose(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneSearchSystem.AddSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = jobHandle2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public GenerateZonesSystem()
    {
    }

    private struct CellData
    {
      public int2 m_Location;
      public ZoneType m_ZoneType;
    }

    private struct BaseCell
    {
      public Entity m_Block;
      public int2 m_Location;
    }

    [BurstCompile]
    private struct FillBlocksListJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<Zoning> m_ZoningType;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_ZoneData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      public NativeParallelMultiHashMap<Entity, GenerateZonesSystem.CellData> m_ZonedCells;
      public NativeList<Entity> m_ZonedBlocks;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Zoning> nativeArray2 = chunk.GetNativeArray<Zoning>(ref this.m_ZoningType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          CreationDefinition definitionData = nativeArray1[index];
          Zoning zoningData = nativeArray2[index];
          if (definitionData.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            ZoneData zoneData = this.m_ZoneData[definitionData.m_Prefab];
            if ((zoningData.m_Flags & ZoningFlags.FloodFill) != (ZoningFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.FloodFillBlocks(definitionData, zoningData, zoneData);
            }
            if ((zoningData.m_Flags & ZoningFlags.Paint) != (ZoningFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.PaintBlocks(definitionData, zoningData, zoneData);
            }
            if ((zoningData.m_Flags & ZoningFlags.Marquee) != (ZoningFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.MarqueeBlocks(definitionData, zoningData, zoneData);
            }
          }
        }
      }

      private void MarqueeBlocks(
        CreationDefinition definitionData,
        Zoning zoningData,
        ZoneData zoneData)
      {
        ZoneType zoneType;
        if ((zoningData.m_Flags & ZoningFlags.Zone) != (ZoningFlags) 0)
        {
          zoneType = zoneData.m_ZoneType;
        }
        else
        {
          if ((zoningData.m_Flags & ZoningFlags.Dezone) == (ZoningFlags) 0)
            return;
          zoneType = ZoneType.None;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateZonesSystem.FillBlocksListJob.MarqueeIterator iterator = new GenerateZonesSystem.FillBlocksListJob.MarqueeIterator()
        {
          m_Bounds = MathUtils.Bounds(zoningData.m_Position.xz),
          m_Quad = zoningData.m_Position.xz,
          m_NewZoneType = zoneType,
          m_Overwrite = (zoningData.m_Flags & ZoningFlags.Overwrite) > (ZoningFlags) 0,
          m_BlockData = this.m_BlockData,
          m_Cells = this.m_Cells,
          m_ZonedCells = this.m_ZonedCells,
          m_ZonedBlocks = this.m_ZonedBlocks
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<GenerateZonesSystem.FillBlocksListJob.MarqueeIterator>(ref iterator);
      }

      private void PaintBlocks(
        CreationDefinition definitionData,
        Zoning zoningData,
        ZoneData zoneData)
      {
        ZoneType zoneType;
        if ((zoningData.m_Flags & ZoningFlags.Zone) != (ZoningFlags) 0)
        {
          zoneType = zoneData.m_ZoneType;
        }
        else
        {
          if ((zoningData.m_Flags & ZoningFlags.Dezone) == (ZoningFlags) 0)
            return;
          zoneType = ZoneType.None;
        }
        NativeList<GenerateZonesSystem.BaseCell> baseCells = new NativeList<GenerateZonesSystem.BaseCell>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        this.AddCells(zoningData.m_Position.xz.bc, baseCells);
        for (int index = 0; index < baseCells.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          GenerateZonesSystem.BaseCell baseCell = baseCells[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[baseCell.m_Block];
          // ISSUE: variable of a compiler-generated type
          GenerateZonesSystem.CellData cellData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cellData.m_Location = baseCell.m_Location;
          // ISSUE: reference to a compiler-generated field
          cellData.m_ZoneType = zoneType;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (math.all(cellData.m_Location >= 0 & cellData.m_Location < block.m_Size))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Cell cell = this.m_Cells[baseCell.m_Block][cellData.m_Location.y * block.m_Size.x + cellData.m_Location.x];
            if ((cell.m_State & CellFlags.Visible) != CellFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_ZonedCells.TryGetFirstValue(baseCell.m_Block, out GenerateZonesSystem.CellData _, out NativeParallelMultiHashMapIterator<Entity> _))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_ZonedBlocks.Add(in baseCell.m_Block);
              }
              if ((zoningData.m_Flags & ZoningFlags.Overwrite) == (ZoningFlags) 0 && !cell.m_Zone.Equals(ZoneType.None))
              {
                // ISSUE: reference to a compiler-generated field
                cellData.m_ZoneType = cell.m_Zone;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ZonedCells.Add(baseCell.m_Block, cellData);
            }
          }
        }
      }

      private void FloodFillBlocks(
        CreationDefinition definitionData,
        Zoning zoningData,
        ZoneData zoneData)
      {
        ZoneType zoneType;
        if ((zoningData.m_Flags & ZoningFlags.Zone) != (ZoningFlags) 0)
        {
          zoneType = zoneData.m_ZoneType;
        }
        else
        {
          if ((zoningData.m_Flags & ZoningFlags.Dezone) == (ZoningFlags) 0)
            return;
          zoneType = ZoneType.None;
        }
        NativeParallelHashSet<int> nativeParallelHashSet = new NativeParallelHashSet<int>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<GenerateZonesSystem.BaseCell> baseCells = new NativeList<GenerateZonesSystem.BaseCell>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<int2> nativeList = new NativeList<int2>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        this.AddCells(zoningData.m_Position.xz.bc, baseCells);
        for (int index = 0; index < baseCells.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          GenerateZonesSystem.BaseCell baseCell = baseCells[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[baseCell.m_Block];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> cell1 = this.m_Cells[baseCell.m_Block];
          // ISSUE: reference to a compiler-generated field
          int2 location = baseCell.m_Location;
          Cell cell2 = cell1[location.y * block.m_Size.x + location.x];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GenerateZonesSystem.FillBlocksListJob.FloodFillIterator iterator = new GenerateZonesSystem.FillBlocksListJob.FloodFillIterator()
          {
            m_BaseBlockData = block,
            m_StateMask = cell2.m_State & (CellFlags.Visible | CellFlags.Occupied),
            m_OldZoneType = cell2.m_Zone,
            m_NewZoneType = zoneType,
            m_Overwrite = (zoningData.m_Flags & ZoningFlags.Overwrite) > (ZoningFlags) 0,
            m_BlockData = this.m_BlockData,
            m_Cells = this.m_Cells,
            m_ZonedCells = this.m_ZonedCells,
            m_ZonedBlocks = this.m_ZonedBlocks
          };
          // ISSUE: reference to a compiler-generated method
          nativeParallelHashSet.Add(GenerateZonesSystem.FillBlocksListJob.PackToInt(location));
          nativeList.Add(in location);
          int num = 0;
          while (num < nativeList.Length)
          {
            location = nativeList[num++];
            // ISSUE: reference to a compiler-generated field
            iterator.m_Position = ZoneUtils.GetCellPosition(block, location).xz;
            // ISSUE: reference to a compiler-generated field
            iterator.m_FoundCells = 0;
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Iterate<GenerateZonesSystem.FillBlocksListJob.FloodFillIterator>(ref iterator);
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_FoundCells != 0)
            {
              int2 cellIndex1 = location;
              int2 cellIndex2 = location;
              int2 cellIndex3 = location;
              int2 cellIndex4 = location;
              --cellIndex1.x;
              --cellIndex2.y;
              ++cellIndex3.x;
              ++cellIndex4.y;
              // ISSUE: reference to a compiler-generated method
              if (nativeParallelHashSet.Add(GenerateZonesSystem.FillBlocksListJob.PackToInt(cellIndex1)))
                nativeList.Add(in cellIndex1);
              // ISSUE: reference to a compiler-generated method
              if (nativeParallelHashSet.Add(GenerateZonesSystem.FillBlocksListJob.PackToInt(cellIndex2)))
                nativeList.Add(in cellIndex2);
              // ISSUE: reference to a compiler-generated method
              if (nativeParallelHashSet.Add(GenerateZonesSystem.FillBlocksListJob.PackToInt(cellIndex3)))
                nativeList.Add(in cellIndex3);
              // ISSUE: reference to a compiler-generated method
              if (nativeParallelHashSet.Add(GenerateZonesSystem.FillBlocksListJob.PackToInt(cellIndex4)))
                nativeList.Add(in cellIndex4);
            }
          }
          nativeParallelHashSet.Clear();
          nativeList.Clear();
        }
      }

      private static int PackToInt(int2 cellIndex)
      {
        return cellIndex.y << 16 | cellIndex.x & (int) ushort.MaxValue;
      }

      private void AddCells(Line2.Segment line, NativeList<GenerateZonesSystem.BaseCell> baseCells)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateZonesSystem.FillBlocksListJob.BaseLineIterator iterator = new GenerateZonesSystem.FillBlocksListJob.BaseLineIterator()
        {
          m_Line = line,
          m_BlockData = this.m_BlockData,
          m_Cells = this.m_Cells,
          m_BaseCells = baseCells
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<GenerateZonesSystem.FillBlocksListJob.BaseLineIterator>(ref iterator);
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }

      private struct MarqueeIterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public Quad2 m_Quad;
        public ZoneType m_NewZoneType;
        public bool m_Overwrite;
        public ComponentLookup<Game.Zones.Block> m_BlockData;
        public BufferLookup<Cell> m_Cells;
        public NativeParallelMultiHashMap<Entity, GenerateZonesSystem.CellData> m_ZonedCells;
        public NativeList<Entity> m_ZonedBlocks;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[blockEntity];
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Quad, ZoneUtils.CalculateCorners(block)))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> cell1 = this.m_Cells[blockEntity];
          // ISSUE: variable of a compiler-generated type
          GenerateZonesSystem.CellData cellData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cellData.m_ZoneType = this.m_NewZoneType;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (cellData.m_Location.y = 0; cellData.m_Location.y < block.m_Size.y; ++cellData.m_Location.y)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            for (cellData.m_Location.x = 0; cellData.m_Location.x < block.m_Size.x; ++cellData.m_Location.x)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int index = cellData.m_Location.y * block.m_Size.x + cellData.m_Location.x;
              Cell cell2 = cell1[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((cell2.m_State & CellFlags.Visible) != CellFlags.None && MathUtils.Intersect(this.m_Quad, ZoneUtils.GetCellPosition(block, cellData.m_Location).xz) && this.m_Overwrite | cell2.m_Zone.Equals(ZoneType.None))
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ZonedCells.TryGetFirstValue(blockEntity, out GenerateZonesSystem.CellData _, out NativeParallelMultiHashMapIterator<Entity> _))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ZonedBlocks.Add(in blockEntity);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_ZonedCells.Add(blockEntity, cellData);
              }
            }
          }
        }
      }

      private struct BaseLineIterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Line2.Segment m_Line;
        public ComponentLookup<Game.Zones.Block> m_BlockData;
        public BufferLookup<Cell> m_Cells;
        public NativeList<GenerateZonesSystem.BaseCell> m_BaseCells;

        public bool Intersect(Bounds2 bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds, this.m_Line, out float2 _);
        }

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          float2 t;
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds, this.m_Line, out t))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[blockEntity];
          // ISSUE: reference to a compiler-generated field
          int2 cellIndex1 = ZoneUtils.GetCellIndex(block, this.m_Line.a);
          // ISSUE: reference to a compiler-generated field
          int2 cellIndex2 = ZoneUtils.GetCellIndex(block, this.m_Line.b);
          int2 int2_1 = math.max(math.min(cellIndex1, cellIndex2), (int2) 0);
          int2 int2_2 = math.min(math.max(cellIndex1, cellIndex2), block.m_Size - 1);
          if (!math.all(int2_2 >= int2_1))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> cell = this.m_Cells[blockEntity];
          Quad2 corners = ZoneUtils.CalculateCorners(block);
          float2 float2 = new float2(1f) / (float2) block.m_Size;
          Quad2 quad2 = new Quad2();
          quad2.a = math.lerp(corners.a, corners.d, (float) int2_1.y * float2.y);
          quad2.b = math.lerp(corners.b, corners.c, (float) int2_1.y * float2.y);
          for (int y = int2_1.y; y <= int2_2.y; ++y)
          {
            quad2.d = math.lerp(corners.a, corners.d, (float) (y + 1) * float2.y);
            quad2.c = math.lerp(corners.b, corners.c, (float) (y + 1) * float2.y);
            Quad2 quad = new Quad2();
            quad.a = math.lerp(quad2.a, quad2.b, (float) int2_1.x * float2.x);
            quad.d = math.lerp(quad2.d, quad2.c, (float) int2_1.x * float2.x);
            for (int x = int2_1.x; x <= int2_2.x; ++x)
            {
              quad.b = math.lerp(quad2.a, quad2.b, (float) (x + 1) * float2.x);
              quad.c = math.lerp(quad2.d, quad2.c, (float) (x + 1) * float2.x);
              // ISSUE: reference to a compiler-generated field
              if ((cell[y * block.m_Size.x + x].m_State & CellFlags.Visible) != CellFlags.None && MathUtils.Intersect(quad, this.m_Line, out t))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_BaseCells.Add(new GenerateZonesSystem.BaseCell()
                {
                  m_Block = blockEntity,
                  m_Location = new int2(x, y)
                });
              }
              quad.a = quad.b;
              quad.d = quad.c;
            }
            quad2.a = quad2.d;
            quad2.b = quad2.c;
          }
        }
      }

      private struct FloodFillIterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Game.Zones.Block m_BaseBlockData;
        public float2 m_Position;
        public CellFlags m_StateMask;
        public ZoneType m_OldZoneType;
        public ZoneType m_NewZoneType;
        public bool m_Overwrite;
        public ComponentLookup<Game.Zones.Block> m_BlockData;
        public BufferLookup<Cell> m_Cells;
        public NativeParallelMultiHashMap<Entity, GenerateZonesSystem.CellData> m_ZonedCells;
        public NativeList<Entity> m_ZonedBlocks;
        public int m_FoundCells;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Position);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds, this.m_Position))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[blockEntity];
          // ISSUE: variable of a compiler-generated type
          GenerateZonesSystem.CellData cellData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cellData.m_Location = ZoneUtils.GetCellIndex(block, this.m_Position);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cellData.m_ZoneType = this.m_NewZoneType;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!math.all(cellData.m_Location >= 0 & cellData.m_Location < block.m_Size))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Cell cell = this.m_Cells[blockEntity][cellData.m_Location.y * block.m_Size.x + cellData.m_Location.x];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!((cell.m_State & (CellFlags.Visible | CellFlags.Occupied)) == this.m_StateMask & cell.m_Zone.Equals(this.m_OldZoneType)) || !ZoneUtils.CanShareCells(this.m_BaseBlockData, block))
            return;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ZonedCells.TryGetFirstValue(blockEntity, out GenerateZonesSystem.CellData _, out NativeParallelMultiHashMapIterator<Entity> _))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ZonedBlocks.Add(in blockEntity);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Overwrite && !cell.m_Zone.Equals(ZoneType.None))
          {
            // ISSUE: reference to a compiler-generated field
            cellData.m_ZoneType = cell.m_Zone;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ZonedCells.Add(blockEntity, cellData);
          // ISSUE: reference to a compiler-generated field
          ++this.m_FoundCells;
        }
      }
    }

    [BurstCompile]
    private struct CreateBlocksJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ZoneBlockData> m_ZoneBlockDataData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public NativeParallelMultiHashMap<Entity, GenerateZonesSystem.CellData> m_ZonedCells;
      [ReadOnly]
      public NativeArray<Entity> m_ZonedBlocks;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity zonedBlock = this.m_ZonedBlocks[index];
        // ISSUE: reference to a compiler-generated field
        Game.Zones.Block component1 = this.m_BlockData[zonedBlock];
        // ISSUE: reference to a compiler-generated field
        PrefabRef component2 = this.m_PrefabRefData[zonedBlock];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Cell> cell1 = this.m_Cells[zonedBlock];
        // ISSUE: reference to a compiler-generated field
        ZoneBlockData zoneBlockData = this.m_ZoneBlockDataData[component2.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(index, zoneBlockData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(index, entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Game.Zones.Block>(index, entity, component1);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Cell> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Cell>(index, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(index, entity, new Temp()
        {
          m_Original = zonedBlock
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Hidden>(index, zonedBlock, new Hidden());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(index, zonedBlock, new BatchesUpdated());
        for (int index1 = 0; index1 < cell1.Length; ++index1)
          dynamicBuffer.Add(cell1[index1]);
        // ISSUE: variable of a compiler-generated type
        GenerateZonesSystem.CellData cellData;
        NativeParallelMultiHashMapIterator<Entity> it;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ZonedCells.TryGetFirstValue(zonedBlock, out cellData, out it))
          return;
        // ISSUE: reference to a compiler-generated field
        do
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int index2 = cellData.m_Location.y * component1.m_Size.x + cellData.m_Location.x;
          Cell cell2 = dynamicBuffer[index2];
          cell2.m_State |= CellFlags.Selected;
          // ISSUE: reference to a compiler-generated field
          cell2.m_Zone = cellData.m_ZoneType;
          dynamicBuffer[index2] = cell2;
        }
        while (this.m_ZonedCells.TryGetNextValue(out cellData, ref it));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Zoning> __Game_Tools_Zoning_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneBlockData> __Game_Prefabs_ZoneBlockData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Zoning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Zoning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneBlockData_RO_ComponentLookup = state.GetComponentLookup<ZoneBlockData>(true);
      }
    }
  }
}
