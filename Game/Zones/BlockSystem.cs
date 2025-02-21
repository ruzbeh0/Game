// Decompiled with JetBrains decompiler
// Type: Game.Zones.BlockSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  [CompilerGenerated]
  public class BlockSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedEdgesQuery;
    private ModificationBarrier4 m_ModificationBarrier;
    private BlockSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedEdgesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<SubBlock>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedEdgesQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneBlockData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_SubBlock_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new BlockSystem.UpdateBlocksJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_BuildOrderType = this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_SubBlockType = this.__TypeHandle.__Game_Zones_SubBlock_RO_BufferTypeHandle,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_BuildOrderData = this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_RoadCompositionData = this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_ZoneBlockData = this.__TypeHandle.__Game_Prefabs_ZoneBlockData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<BlockSystem.UpdateBlocksJob>(this.m_UpdatedEdgesQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public BlockSystem()
    {
    }

    [BurstCompile]
    private struct UpdateBlocksJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.BuildOrder> m_BuildOrderType;
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public BufferTypeHandle<SubBlock> m_SubBlockType;
      [ReadOnly]
      public ComponentLookup<Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.BuildOrder> m_BuildOrderData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<RoadComposition> m_RoadCompositionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<ZoneBlockData> m_ZoneBlockData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubBlock> bufferAccessor = chunk.GetBufferAccessor<SubBlock>(ref this.m_SubBlockType);
          for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
          {
            DynamicBuffer<SubBlock> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity subBlock = dynamicBuffer[index2].m_SubBlock;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, subBlock, new Deleted());
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Edge> nativeArray2 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Composition> nativeArray4 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.BuildOrder> nativeArray5 = chunk.GetNativeArray<Game.Net.BuildOrder>(ref this.m_BuildOrderType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Road> nativeArray6 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubBlock> bufferAccessor = chunk.GetBufferAccessor<SubBlock>(ref this.m_SubBlockType);
          NativeParallelHashMap<Block, Entity> oldBlockBuffer = new NativeParallelHashMap<Block, Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity owner = nativeArray1[index];
            Edge edge = nativeArray2[index];
            Curve curve = nativeArray3[index];
            Composition composition = nativeArray4[index];
            Game.Net.BuildOrder buildOrder = nativeArray5[index];
            Road road = nativeArray6[index];
            DynamicBuffer<SubBlock> blocks = bufferAccessor[index];
            // ISSUE: reference to a compiler-generated method
            this.FillOldBlockBuffer(blocks, oldBlockBuffer);
            // ISSUE: reference to a compiler-generated method
            this.CreateBlocks(unfilteredChunkIndex, owner, oldBlockBuffer, composition, edge, curve, buildOrder, road);
            // ISSUE: reference to a compiler-generated method
            this.RemoveUnusedOldBlocks(unfilteredChunkIndex, blocks, oldBlockBuffer);
            oldBlockBuffer.Clear();
          }
          oldBlockBuffer.Dispose();
        }
      }

      private void FillOldBlockBuffer(
        DynamicBuffer<SubBlock> blocks,
        NativeParallelHashMap<Block, Entity> oldBlockBuffer)
      {
        for (int index = 0; index < blocks.Length; ++index)
        {
          Entity subBlock = blocks[index].m_SubBlock;
          // ISSUE: reference to a compiler-generated field
          Block key = this.m_BlockData[subBlock];
          oldBlockBuffer.TryAdd(key, subBlock);
        }
      }

      private void RemoveUnusedOldBlocks(
        int jobIndex,
        DynamicBuffer<SubBlock> blocks,
        NativeParallelHashMap<Block, Entity> oldBlockBuffer)
      {
        for (int index = 0; index < blocks.Length; ++index)
        {
          Entity subBlock = blocks[index].m_SubBlock;
          // ISSUE: reference to a compiler-generated field
          Block key = this.m_BlockData[subBlock];
          if (oldBlockBuffer.TryGetValue(key, out Entity _))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, subBlock, new Deleted());
            oldBlockBuffer.Remove(key);
          }
        }
      }

      private void CreateBlocks(
        int jobIndex,
        Entity owner,
        NativeParallelHashMap<Block, Entity> oldBlockBuffer,
        Composition composition,
        Edge edge,
        Curve curve,
        Game.Net.BuildOrder buildOrder,
        Road road)
      {
        RoadComposition componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_RoadCompositionData.TryGetComponent(composition.m_Edge, out componentData) || (componentData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) == (Game.Prefabs.RoadFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
        if ((netCompositionData.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) != (CompositionFlags.General) 0)
          return;
        bool flag1 = (netCompositionData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0;
        bool flag2 = (netCompositionData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0;
        if (!flag1 && !flag2)
          return;
        uint buildOrder1 = math.max(buildOrder.m_Start, buildOrder.m_End);
        bool flag3 = (road.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0;
        bool flag4 = (road.m_Flags & Game.Net.RoadFlags.EndHalfAligned) != 0;
        if (flag1)
        {
          int cellWidth = ZoneUtils.GetCellWidth(netCompositionData.m_Width - netCompositionData.m_MiddleOffset * 2f);
          // ISSUE: reference to a compiler-generated method
          this.CreateBlocks(jobIndex, owner, edge.m_Start, edge.m_End, oldBlockBuffer, componentData.m_ZoneBlockPrefab, curve.m_Bezier, cellWidth, buildOrder.m_Start, buildOrder.m_End, flag3, flag4, false);
        }
        if (flag2)
        {
          int cellWidth = ZoneUtils.GetCellWidth(netCompositionData.m_Width + netCompositionData.m_MiddleOffset * 2f);
          // ISSUE: reference to a compiler-generated method
          this.CreateBlocks(jobIndex, owner, edge.m_End, edge.m_Start, oldBlockBuffer, componentData.m_ZoneBlockPrefab, MathUtils.Invert(curve.m_Bezier), cellWidth, buildOrder.m_End, buildOrder.m_Start, flag4, flag3, true);
        }
        // ISSUE: reference to a compiler-generated field
        if ((this.m_PrefabCompositionData[composition.m_StartNode].m_Flags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
        {
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated method
            this.CreateBlocks(jobIndex, owner, edge.m_Start, oldBlockBuffer, componentData.m_ZoneBlockPrefab, buildOrder1, true, false);
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated method
            this.CreateBlocks(jobIndex, owner, edge.m_Start, oldBlockBuffer, componentData.m_ZoneBlockPrefab, buildOrder1, true, true);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((this.m_PrefabCompositionData[composition.m_EndNode].m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0)
          return;
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateBlocks(jobIndex, owner, edge.m_End, oldBlockBuffer, componentData.m_ZoneBlockPrefab, buildOrder1, false, false);
        }
        if (!flag1)
          return;
        // ISSUE: reference to a compiler-generated method
        this.CreateBlocks(jobIndex, owner, edge.m_End, oldBlockBuffer, componentData.m_ZoneBlockPrefab, buildOrder1, false, true);
      }

      private void CreateBlocks(
        int jobIndex,
        Entity owner,
        Entity node,
        NativeParallelHashMap<Block, Entity> oldBlockBuffer,
        Entity blockPrefab,
        uint buildOrder,
        bool start,
        bool right)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeNodeGeometry edgeNodeGeometry = !start ? this.m_EndNodeGeometryData[owner].m_Geometry : this.m_StartNodeGeometryData[owner].m_Geometry;
        Bezier4x3 output2_1;
        Bezier4x3 output2_2;
        Bezier4x3 left1;
        Bezier4x3 left2;
        if (right)
        {
          output2_1 = edgeNodeGeometry.m_Left.m_Right;
          output2_2 = edgeNodeGeometry.m_Right.m_Right;
          left1 = edgeNodeGeometry.m_Right.m_Left;
          left2 = edgeNodeGeometry.m_Left.m_Left;
        }
        else
        {
          output2_1 = edgeNodeGeometry.m_Left.m_Right;
          output2_2 = edgeNodeGeometry.m_Right.m_Right;
          left1 = edgeNodeGeometry.m_Right.m_Left;
          left2 = edgeNodeGeometry.m_Left.m_Left;
        }
        float num1 = float.MaxValue;
        Entity entity1 = owner;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          EdgeNodeGeometry geometry;
          if (edge2.m_Start == node)
          {
            // ISSUE: reference to a compiler-generated field
            geometry = this.m_StartNodeGeometryData[edge1].m_Geometry;
          }
          else if (edge2.m_End == node)
          {
            // ISSUE: reference to a compiler-generated field
            geometry = this.m_EndNodeGeometryData[edge1].m_Geometry;
          }
          else
            continue;
          float num2 = !right ? math.distancesq(geometry.m_Right.m_Right.d, edgeNodeGeometry.m_Right.m_Left.d) : math.distancesq(geometry.m_Right.m_Left.d, edgeNodeGeometry.m_Right.m_Right.d);
          if ((double) num2 < (double) num1)
          {
            if (right)
            {
              left2 = geometry.m_Left.m_Left;
              left1 = geometry.m_Right.m_Left;
            }
            else
            {
              output2_1 = geometry.m_Left.m_Right;
              output2_2 = geometry.m_Right.m_Right;
            }
            num1 = num2;
            entity1 = edge1;
          }
        }
        Game.Net.BuildOrder componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildOrderData.TryGetComponent(entity1, out componentData1))
          buildOrder = math.max(buildOrder, math.max(componentData1.m_Start, componentData1.m_End));
        bool flag1 = false;
        Composition componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CompositionData.TryGetComponent(entity1, out componentData2))
        {
          RoadComposition componentData3;
          // ISSUE: reference to a compiler-generated field
          bool flag2 = !this.m_RoadCompositionData.TryGetComponent(componentData2.m_Edge, out componentData3) || (componentData3.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) == (Game.Prefabs.RoadFlags) 0;
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[componentData2.m_Edge];
          bool flag3 = flag2 | (netCompositionData.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) > (CompositionFlags.General) 0;
          flag1 = !right ? flag3 | (netCompositionData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) > (CompositionFlags.Side) 0 : flag3 | (netCompositionData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) > (CompositionFlags.Side) 0;
        }
        // ISSUE: reference to a compiler-generated method
        this.CutStart(ref output2_1, ref output2_2, !right & flag1);
        // ISSUE: reference to a compiler-generated method
        this.CutStart(ref left2, ref left1, right & flag1);
        Bezier4x3 output1_1 = MathUtils.Invert(left1);
        Bezier4x3 output1_2 = MathUtils.Invert(left2);
        float4 float4 = new float4(MathUtils.Length(output2_1.xz), MathUtils.Length(output2_2.xz), MathUtils.Length(output1_1.xz), MathUtils.Length(output1_2.xz));
        float y = math.csum(float4);
        if ((double) y < 8.0)
          return;
        float2 xz1 = MathUtils.StartTangent(output2_2).xz;
        float2 xz2 = MathUtils.EndTangent(output1_1).xz;
        if (!MathUtils.TryNormalize(ref xz1) || !MathUtils.TryNormalize(ref xz2))
          return;
        int totalWidth = (int) math.floor(y / 8f);
        int baseWidth = 0;
        int middleWidth = 0;
        int splitCount = 0;
        if (totalWidth <= 1)
          return;
        if (totalWidth <= 3)
        {
          middleWidth = totalWidth;
          splitCount = 1;
        }
        else if (totalWidth <= 5)
        {
          baseWidth = 2;
          splitCount = 2;
        }
        else if (totalWidth <= 7)
        {
          baseWidth = 2;
          middleWidth = totalWidth - 4;
          splitCount = 3;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.TryOption(ref baseWidth, ref middleWidth, ref splitCount, totalWidth, 3, 3);
          // ISSUE: reference to a compiler-generated method
          this.TryOption(ref baseWidth, ref middleWidth, ref splitCount, totalWidth, 3, 0);
          // ISSUE: reference to a compiler-generated method
          this.TryOption(ref baseWidth, ref middleWidth, ref splitCount, totalWidth, 2, 2);
          // ISSUE: reference to a compiler-generated method
          this.TryOption(ref baseWidth, ref middleWidth, ref splitCount, totalWidth, 2, 0);
          // ISSUE: reference to a compiler-generated method
          this.TryOption(ref baseWidth, ref middleWidth, ref splitCount, totalWidth, 3, 2);
          // ISSUE: reference to a compiler-generated method
          this.TryOption(ref baseWidth, ref middleWidth, ref splitCount, totalWidth, 2, 3);
        }
        int num3 = math.select(splitCount >> 1, 0, flag1 | right);
        int num4 = math.select(splitCount >> 1, splitCount, flag1 || !right);
        if (num3 >= num4)
          return;
        int num5 = middleWidth + baseWidth * (splitCount & -2);
        float length1 = (float) (((double) y - (double) num5 * 8.0) * 0.5);
        if ((double) length1 > 0.0)
        {
          Bounds1 t = new Bounds1(0.0f, 1f);
          Bezier4x3 bezier4x3;
          if (MathUtils.ClampLength(output2_1.xz, ref t, length1))
          {
            MathUtils.Divide(output2_1, out bezier4x3, out output2_1, t.max);
            float4.x = math.max(0.0f, float4.x - length1);
          }
          else
          {
            float length2 = math.max(0.0f, length1 - float4.x);
            if (MathUtils.ClampLength(output2_2.xz, ref t, length2))
            {
              MathUtils.Divide(output2_2, out bezier4x3, out output2_2, t.max);
              float4.y = math.max(0.0f, float4.y - length2);
            }
            else
            {
              output2_2.a = output2_2.b = output2_2.c = output2_2.d = output1_1.a;
              float4.y = 0.0f;
            }
            output2_1.a = output2_1.b = output2_1.c = output2_1.d = output2_2.a;
            float4.x = 0.0f;
          }
          t = new Bounds1(0.0f, 1f);
          if (MathUtils.ClampLengthInverse(output1_2.xz, ref t, length1))
          {
            MathUtils.Divide(output1_2, out output1_2, out bezier4x3, t.min);
            float4.w = math.max(0.0f, float4.w - length1);
          }
          else
          {
            float length3 = math.max(0.0f, length1 - float4.w);
            if (MathUtils.ClampLengthInverse(output1_1.xz, ref t, length3))
            {
              MathUtils.Divide(output1_1, out output1_1, out bezier4x3, t.min);
              float4.z = math.max(0.0f, float4.z - length3);
            }
            else
            {
              output1_1.a = output1_1.b = output1_1.c = output1_1.d = output2_2.d;
              float4.z = 0.0f;
            }
            output1_2.a = output1_2.b = output1_2.c = output1_2.d = output1_1.d;
            float4.w = 0.0f;
          }
          y = math.csum(float4);
        }
        BuildOrder component1 = new BuildOrder();
        component1.m_Order = buildOrder;
        CurvePosition component2 = new CurvePosition();
        component2.m_CurvePosition = (float2) math.select(1f, 0.0f, start);
        for (int x1 = num3; x1 < num4; ++x1)
        {
          int num6 = math.select(baseWidth, middleWidth, x1 == splitCount >> 1 && middleWidth > 0);
          int2 int2 = new int2(x1, x1 + 1);
          int2 = int2 * baseWidth + math.select((int2) 0, (int2) (middleWidth - baseWidth), int2 > splitCount >> 1 & middleWidth > 0);
          float2 cutRange1 = new float2((float) int2.x / (float) num5, (float) int2.y / (float) num5) * y;
          Bezier4x3 curve1B;
          Bezier4x3 curve2B;
          Bezier4x3 curve3B;
          Bezier4x3 curve4B;
          // ISSUE: reference to a compiler-generated method
          this.CutCurves(output2_1, output2_2, output1_1, output1_2, float4, cutRange1, out curve1B, out curve2B, out curve3B, out curve4B);
          float3 float3_1;
          float2 xz3;
          if ((double) math.distancesq(curve1B.a, curve1B.d) < 0.0099999997764825821)
          {
            if ((double) math.distancesq(curve2B.a, curve2B.d) < 0.0099999997764825821)
            {
              if ((double) math.distancesq(curve3B.a, curve3B.d) < 0.0099999997764825821)
              {
                float3_1 = MathUtils.StartTangent(curve4B);
                xz3 = float3_1.xz;
              }
              else
              {
                float3_1 = MathUtils.StartTangent(curve3B);
                xz3 = float3_1.xz;
              }
            }
            else
            {
              float3_1 = MathUtils.StartTangent(curve2B);
              xz3 = float3_1.xz;
            }
          }
          else
          {
            float3_1 = MathUtils.StartTangent(curve1B);
            xz3 = float3_1.xz;
          }
          float2 xz4;
          if ((double) math.distancesq(curve4B.a, curve4B.d) < 0.0099999997764825821)
          {
            if ((double) math.distancesq(curve3B.a, curve3B.d) < 0.0099999997764825821)
            {
              if ((double) math.distancesq(curve2B.a, curve2B.d) < 0.0099999997764825821)
              {
                float3_1 = MathUtils.EndTangent(curve1B);
                xz4 = float3_1.xz;
              }
              else
              {
                float3_1 = MathUtils.EndTangent(curve2B);
                xz4 = float3_1.xz;
              }
            }
            else
            {
              float3_1 = MathUtils.EndTangent(curve3B);
              xz4 = float3_1.xz;
            }
          }
          else
          {
            float3_1 = MathUtils.EndTangent(curve4B);
            xz4 = float3_1.xz;
          }
          if (MathUtils.TryNormalize(ref xz3) && MathUtils.TryNormalize(ref xz4))
          {
            float2 x2 = MathUtils.Right(xz3);
            float2 x3 = MathUtils.Right(xz4);
            float3 x4 = curve1B.a;
            float3 d = curve4B.d;
            float2 forward = d.xz - x4.xz;
            if (MathUtils.TryNormalize(ref forward))
            {
              float2 float2_1 = MathUtils.Right(forward);
              float t1;
              float num7 = math.max(math.max(MathUtils.MaxDot(curve1B.xz, float2_1, out t1), MathUtils.MaxDot(curve2B.xz, float2_1, out t1)), math.max(MathUtils.MaxDot(curve3B.xz, float2_1, out t1), MathUtils.MaxDot(curve4B.xz, float2_1, out t1))) - math.dot(x4.xz, float2_1);
              float b = math.distance(x4.xz, d.xz);
              x4.xz += x2 * math.clamp(num7 / math.dot(x2, float2_1), 0.0f, b);
              d.xz += x3 * math.clamp(num7 / math.dot(x3, float2_1), 0.0f, b);
              float3 float3_2 = math.lerp(x4, d, 0.5f);
              float2 float2_2 = forward * ((float) num6 * 4f);
              x4 = float3_2;
              x4.xz -= float2_2;
              float3 float3_3 = float3_2;
              float3_3.xz += float2_2;
              float2 cutRange2 = cutRange1;
              float2 t2;
              if (MathUtils.Intersect(curve1B.xz, new Line2.Segment(x4.xz, x4.xz - float2_1 * 48f), out t2, 4))
                cutRange2.x = math.max(cutRange2.x, t2.x * float4.x);
              if (MathUtils.Intersect(curve2B.xz, new Line2.Segment(x4.xz, x4.xz - float2_1 * 48f), out t2, 4))
                cutRange2.x = math.max(cutRange2.x, float4.x + t2.x * float4.y);
              if (MathUtils.Intersect(curve3B.xz, new Line2.Segment(x4.xz, x4.xz - float2_1 * 48f), out t2, 4))
                cutRange2.x = math.max(cutRange2.x, (float) ((double) float4.x + (double) float4.y + (double) t2.x * (double) float4.z));
              if (MathUtils.Intersect(curve4B.xz, new Line2.Segment(x4.xz, x4.xz - float2_1 * 48f), out t2, 4))
                cutRange2.x = math.max(cutRange2.x, (float) ((double) float4.x + (double) float4.y + (double) float4.z + (double) t2.x * (double) float4.w));
              if (MathUtils.Intersect(curve1B.xz, new Line2.Segment(float3_3.xz, float3_3.xz - float2_1 * 48f), out t2, 4))
                cutRange2.y = math.min(cutRange2.y, t2.x * float4.x);
              if (MathUtils.Intersect(curve2B.xz, new Line2.Segment(float3_3.xz, float3_3.xz - float2_1 * 48f), out t2, 4))
                cutRange2.y = math.min(cutRange2.y, float4.x + t2.x * float4.y);
              if (MathUtils.Intersect(curve3B.xz, new Line2.Segment(float3_3.xz, float3_3.xz - float2_1 * 48f), out t2, 4))
                cutRange2.y = math.min(cutRange2.y, (float) ((double) float4.x + (double) float4.y + (double) t2.x * (double) float4.z));
              if (MathUtils.Intersect(curve4B.xz, new Line2.Segment(float3_3.xz, float3_3.xz - float2_1 * 48f), out t2, 4))
                cutRange2.y = math.min(cutRange2.y, (float) ((double) float4.x + (double) float4.y + (double) float4.z + (double) t2.x * (double) float4.w));
              cutRange2.x = math.min(cutRange2.x, y);
              cutRange2.y = math.max(cutRange2.y, 0.0f);
              // ISSUE: reference to a compiler-generated method
              this.CutCurves(output2_1, output2_2, output1_1, output1_2, float4, cutRange2, out curve1B, out curve2B, out curve3B, out curve4B);
              float num8 = math.max(math.max(MathUtils.MaxDot(curve1B.xz, float2_1, out t1), MathUtils.MaxDot(curve2B.xz, float2_1, out t1)), math.max(MathUtils.MaxDot(curve3B.xz, float2_1, out t1), MathUtils.MaxDot(curve4B.xz, float2_1, out t1))) - math.dot(x4.xz, float2_1);
              x4.xz += float2_1 * (num8 + 24f);
              int num9 = (num6 + 10 - 1) / 10;
              for (int index1 = 0; index1 < num9; ++index1)
              {
                int num10 = index1 * num6 / num9;
                int num11 = (index1 + 1) * num6 / num9;
                Block block = new Block();
                block.m_Position = x4;
                block.m_Position.xz += forward * ((float) (num10 + num11) * 4f);
                block.m_Direction = -float2_1;
                block.m_Size.x = (int) (byte) (num11 - num10);
                block.m_Size.y = 6;
                Entity e;
                if (oldBlockBuffer.TryGetValue(block, out e))
                {
                  oldBlockBuffer.Remove(block);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, e, new PrefabRef(blockPrefab));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<CurvePosition>(jobIndex, e, component2);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<BuildOrder>(jobIndex, e, component1);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(jobIndex, e, new Updated());
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  ZoneBlockData zoneBlockData = this.m_ZoneBlockData[blockPrefab];
                  // ISSUE: reference to a compiler-generated field
                  Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, zoneBlockData.m_Archetype);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity2, new PrefabRef(blockPrefab));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Block>(jobIndex, entity2, block);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<CurvePosition>(jobIndex, entity2, component2);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<BuildOrder>(jobIndex, entity2, component1);
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Cell> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Cell>(jobIndex, entity2);
                  int num12 = block.m_Size.x * block.m_Size.y;
                  for (int index2 = 0; index2 < num12; ++index2)
                    dynamicBuffer.Add(new Cell());
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity2, new Owner()
                  {
                    m_Owner = owner
                  });
                }
              }
            }
          }
        }
      }

      private void TryOption(
        ref int baseWidth,
        ref int middleWidth,
        ref int splitCount,
        int totalWidth,
        int newBaseWidth,
        int newMiddleWidth)
      {
        int num1 = math.min(1, newMiddleWidth) + (math.max(0, totalWidth - newMiddleWidth) / math.max(1, newBaseWidth) & -2);
        int num2 = middleWidth + baseWidth * (splitCount & -2);
        if (newMiddleWidth + newBaseWidth * (num1 & -2) <= num2)
          return;
        baseWidth = newBaseWidth;
        middleWidth = newMiddleWidth;
        splitCount = num1;
      }

      private void CutStart(ref Bezier4x3 curve1, ref Bezier4x3 curve2, bool cutFirst)
      {
        float length = 8f;
        if (cutFirst)
        {
          curve1.a = curve1.b = curve1.c = curve1.d = curve2.a;
        }
        else
        {
          float t1;
          // ISSUE: reference to a compiler-generated method
          if (this.FindCutPos(curve1, curve1, out t1))
          {
            if ((double) t1 == 0.0)
              return;
            Bounds1 t2 = new Bounds1(0.0f, t1);
            if (!MathUtils.ClampLengthInverse(curve1.xz, ref t2, length))
              return;
            MathUtils.Divide(curve1, out Bezier4x3 _, out curve1, t2.min);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.FindCutPos(curve1, curve2, out t1);
            Bezier4x3 output1;
            if ((double) t1 != 0.0)
            {
              Bounds1 t3 = new Bounds1(0.0f, t1);
              if (MathUtils.ClampLengthInverse(curve2.xz, ref t3, length))
              {
                MathUtils.Divide(curve2, out output1, out curve2, t3.min);
                length = 0.0f;
              }
              else
                length -= MathUtils.Length(curve2.xz, t3);
            }
            if ((double) length > 0.0)
            {
              Bounds1 t4 = new Bounds1(0.0f, 1f);
              if (!MathUtils.ClampLengthInverse(curve1.xz, ref t4, length))
                return;
              MathUtils.Divide(curve1, out output1, out curve1, t4.min);
            }
            else
              curve1.a = curve1.b = curve1.c = curve1.d = curve2.a;
          }
        }
      }

      private bool FindCutPos(Bezier4x3 startCurve, Bezier4x3 curve, out float t)
      {
        Bezier4x3 curve1 = new Bezier4x3(curve.a - startCurve.a, curve.b - startCurve.a, curve.c - startCurve.a, curve.d - startCurve.a);
        float2 x = new float2(0.0f, 1f);
        float2 xz1 = MathUtils.StartTangent(startCurve).xz;
        t = 0.0f;
        if (!MathUtils.TryNormalize(ref xz1))
          return true;
        for (int index = 0; index < 8; ++index)
        {
          float t1 = math.csum(x) * 0.5f;
          float2 xz2 = MathUtils.Position(curve1, t1).xz;
          float2 xz3 = MathUtils.Tangent(curve1, t1).xz;
          if (!MathUtils.TryNormalize(ref xz3))
            return true;
          if ((double) math.dot(xz1, xz2 - xz3 * 8f) - (double) math.abs(math.dot(xz1, MathUtils.Right(xz3) * 16f)) < 0.0)
            x.x = t1;
          else
            x.y = t1;
        }
        if ((double) x.x == 0.0)
          return true;
        t = x.y;
        return (double) x.y != 1.0;
      }

      private void CutCurves(
        Bezier4x3 curve1,
        Bezier4x3 curve2,
        Bezier4x3 curve3,
        Bezier4x3 curve4,
        float4 curveLengths,
        float2 cutRange,
        out Bezier4x3 curve1B,
        out Bezier4x3 curve2B,
        out Bezier4x3 curve3B,
        out Bezier4x3 curve4B)
      {
        float2 float2 = new float2(0.0f, 1f);
        float2 t1 = float2;
        float2 t2 = float2;
        float2 t3 = float2;
        float2 t4 = float2;
        curve1B = curve1;
        curve2B = curve2;
        curve3B = curve3;
        curve4B = curve4;
        if ((double) cutRange.x - (double) curveLengths.x - (double) curveLengths.y < (double) curveLengths.z)
        {
          if ((double) cutRange.x - (double) curveLengths.x < (double) curveLengths.y)
          {
            if ((double) cutRange.x < (double) curveLengths.x)
            {
              t1.x = cutRange.x / curveLengths.x;
            }
            else
            {
              t1.x = 2f;
              t2.x = math.saturate((cutRange.x - curveLengths.x) / curveLengths.y);
            }
          }
          else
          {
            t1.x = 2f;
            t2.x = 2f;
            t3.x = math.saturate((cutRange.x - curveLengths.x - curveLengths.y) / curveLengths.z);
          }
        }
        else
        {
          t1.x = 2f;
          t2.x = 2f;
          t3.x = 2f;
          t4.x = math.saturate((cutRange.x - curveLengths.x - curveLengths.y - curveLengths.z) / curveLengths.w);
        }
        if ((double) cutRange.y > (double) curveLengths.x)
        {
          if ((double) cutRange.y - (double) curveLengths.x > (double) curveLengths.y)
          {
            if ((double) cutRange.y - (double) curveLengths.x - (double) curveLengths.y > (double) curveLengths.z)
            {
              t4.y = math.saturate((cutRange.y - curveLengths.x - curveLengths.y - curveLengths.z) / curveLengths.w);
            }
            else
            {
              t4.y = -1f;
              t3.y = math.saturate((cutRange.y - curveLengths.x - curveLengths.y) / curveLengths.z);
            }
          }
          else
          {
            t4.y = -1f;
            t3.y = -1f;
            t2.y = math.saturate((cutRange.y - curveLengths.x) / curveLengths.y);
          }
        }
        else
        {
          t4.y = -1f;
          t3.y = -1f;
          t2.y = -1f;
          t1.y = math.saturate(cutRange.y / curveLengths.x);
        }
        if (math.any(t1 != float2) && (double) t1.x <= (double) t1.y)
          curve1B = MathUtils.Cut(curve1, t1);
        if (math.any(t2 != float2) && (double) t2.x <= (double) t2.y)
          curve2B = MathUtils.Cut(curve2, t2);
        if (math.any(t3 != float2) && (double) t3.x <= (double) t3.y)
          curve3B = MathUtils.Cut(curve3, t3);
        if (math.any(t4 != float2) && (double) t4.x <= (double) t4.y)
          curve4B = MathUtils.Cut(curve4, t4);
        if ((double) t3.x == 2.0)
          curve3B.a = curve3B.b = curve3B.c = curve3B.d = curve4B.a;
        if ((double) t2.x == 2.0)
          curve2B.a = curve2B.b = curve2B.c = curve2B.d = curve3B.a;
        if ((double) t1.x == 2.0)
          curve1B.a = curve1B.b = curve1B.c = curve1B.d = curve2B.a;
        if ((double) t2.y == -1.0)
          curve2B.a = curve2B.b = curve2B.c = curve2B.d = curve1B.d;
        if ((double) t3.y == -1.0)
          curve3B.a = curve3B.b = curve3B.c = curve3B.d = curve2B.d;
        if ((double) t4.y != -1.0)
          return;
        curve4B.a = curve4B.b = curve4B.c = curve4B.d = curve3B.d;
      }

      private bool FindContinuousEdge(
        Entity node,
        Entity edge,
        float2 position,
        float2 tangent,
        int cellWidth,
        bool halfAligned,
        bool invert)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          if (!(edge1 == edge))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1];
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[edge1];
            float2 xz;
            float2 y;
            bool c;
            if (edge2.m_Start == node)
            {
              xz = curve.m_Bezier.a.xz;
              y = MathUtils.StartTangent(curve.m_Bezier).xz;
              c = invert;
            }
            else if (edge2.m_End == node)
            {
              xz = curve.m_Bezier.d.xz;
              y = -MathUtils.EndTangent(curve.m_Bezier).xz;
              c = !invert;
            }
            else
              continue;
            if (MathUtils.TryNormalize(ref y) && (double) math.dot(tangent, y) <= -0.99000000953674316 && (double) math.distance(position, xz) <= 0.0099999997764825821)
            {
              // ISSUE: reference to a compiler-generated field
              Entity edge3 = this.m_CompositionData[edge1].m_Edge;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_RoadCompositionData.HasComponent(edge3) && (this.m_RoadCompositionData[edge3].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                NetCompositionData netCompositionData = this.m_PrefabCompositionData[edge3];
                if ((netCompositionData.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) == (CompositionFlags.General) 0)
                {
                  int cellWidth1 = ZoneUtils.GetCellWidth(netCompositionData.m_Width + netCompositionData.m_MiddleOffset * math.select(-2f, 2f, c));
                  if (cellWidth == cellWidth1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Road road = this.m_RoadData[edge1];
                    if (edge2.m_Start == node)
                    {
                      if ((road.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0 != halfAligned)
                        continue;
                    }
                    else if ((road.m_Flags & Game.Net.RoadFlags.EndHalfAligned) != 0 != halfAligned)
                      continue;
                    return true;
                  }
                }
              }
            }
          }
        }
        return false;
      }

      private void CreateBlocks(
        int jobIndex,
        Entity owner,
        Entity startNode,
        Entity endNode,
        NativeParallelHashMap<Block, Entity> oldBlockBuffer,
        Entity blockPrefab,
        Bezier4x3 curve,
        int cellWidth,
        uint startOrder,
        uint endOrder,
        bool startHalf,
        bool endHalf,
        bool invert)
      {
        float2 xz1 = MathUtils.StartTangent(curve).xz;
        float2 xz2 = MathUtils.EndTangent(curve).xz;
        if (!MathUtils.TryNormalize(ref xz1) || !MathUtils.TryNormalize(ref xz2))
          return;
        // ISSUE: reference to a compiler-generated method
        bool continuousEdge1 = this.FindContinuousEdge(startNode, owner, curve.a.xz, xz1, cellWidth, startHalf, !invert);
        // ISSUE: reference to a compiler-generated method
        bool continuousEdge2 = this.FindContinuousEdge(endNode, owner, curve.d.xz, -xz2, cellWidth, endHalf, invert);
        float middleTangentPos = NetUtils.FindMiddleTangentPos(curve.xz, new float2(0.0f, 1f));
        Bezier4x3 output1;
        Bezier4x3 output2;
        MathUtils.Divide(curve, out output1, out output2, middleTangentPos);
        float num1 = (float) cellWidth * 4f;
        Bezier4x3 curve1 = NetUtils.OffsetCurveLeftSmooth(output1, (float2) -num1);
        Bezier4x3 curve2 = NetUtils.OffsetCurveLeftSmooth(output2, (float2) -num1);
        float num2 = MathUtils.Length(curve1.xz) + MathUtils.Length(curve2.xz);
        if ((double) num2 < 8.0)
          return;
        float2 xz3 = MathUtils.StartTangent(curve1).xz;
        float2 xz4 = MathUtils.EndTangent(curve2).xz;
        if (!MathUtils.TryNormalize(ref xz3) || !MathUtils.TryNormalize(ref xz4))
          return;
        float num3 = (float) (2.0 / (double) math.sqrt(math.clamp(math.degrees(math.acos(math.clamp(math.dot(xz3, xz4), -1f, 1f)) * 8f / num2) / 15f, 0.0001f, 1f)) * 8.0);
        int num4 = math.max(1, (int) ((double) num2 / (double) num3));
        BuildOrder component1 = new BuildOrder();
        component1.m_Order = startOrder;
        for (int index1 = 0; index1 < num4; ++index1)
        {
          float2 float2_1 = new float2((float) index1 / (float) num4, (float) (index1 + 1) / (float) num4);
          float2 t1 = math.min((float2) 1f, float2_1 / middleTangentPos);
          float2 t2 = math.max((float2) 0.0f, (float2_1 - middleTangentPos) / (1f - middleTangentPos));
          float3 float3_1;
          float2 xz5;
          Bezier4x3 bezier4x3_1;
          if ((double) t1.x < 1.0)
          {
            float3_1 = MathUtils.Tangent(curve1, t1.x);
            xz5 = float3_1.xz;
            bezier4x3_1 = MathUtils.Cut(curve1, t1);
          }
          else
          {
            float3_1 = MathUtils.Tangent(curve2, t2.x);
            xz5 = float3_1.xz;
            ref Bezier4x3 local1 = ref bezier4x3_1;
            ref Bezier4x3 local2 = ref bezier4x3_1;
            ref Bezier4x3 local3 = ref bezier4x3_1;
            bezier4x3_1.d = float3_1 = MathUtils.Position(curve2, t2.x);
            float3 float3_2;
            float3_1 = float3_2 = float3_1;
            local3.c = float3_2;
            float3 float3_3;
            float3_1 = float3_3 = float3_1;
            local2.b = float3_3;
            float3 float3_4 = float3_1;
            local1.a = float3_4;
          }
          float2 xz6;
          Bezier4x3 bezier4x3_2;
          if ((double) t1.y < 1.0)
          {
            float3_1 = MathUtils.Tangent(curve1, t1.y);
            xz6 = float3_1.xz;
            ref Bezier4x3 local4 = ref bezier4x3_2;
            ref Bezier4x3 local5 = ref bezier4x3_2;
            ref Bezier4x3 local6 = ref bezier4x3_2;
            bezier4x3_2.d = float3_1 = MathUtils.Position(curve1, t1.y);
            float3 float3_5;
            float3_1 = float3_5 = float3_1;
            local6.c = float3_5;
            float3 float3_6;
            float3_1 = float3_6 = float3_1;
            local5.b = float3_6;
            float3 float3_7 = float3_1;
            local4.a = float3_7;
          }
          else
          {
            float3_1 = MathUtils.Tangent(curve2, t2.y);
            xz6 = float3_1.xz;
            bezier4x3_2 = MathUtils.Cut(curve2, t2);
          }
          if (MathUtils.TryNormalize(ref xz5) && MathUtils.TryNormalize(ref xz6))
          {
            float2 x1 = MathUtils.Right(xz5);
            float2 x2 = MathUtils.Right(xz6);
            float3 x3 = bezier4x3_1.a;
            float3 y1 = bezier4x3_2.d;
            float2 forward = y1.xz - x3.xz;
            if (MathUtils.TryNormalize(ref forward))
            {
              if (index1 == 0)
              {
                if (continuousEdge1)
                {
                  x3.xz -= forward * math.select(0.0f, 4f, (cellWidth & 1) != 0 ^ startHalf);
                }
                else
                {
                  float num5 = num1 - math.select(0.0f, 8f, cellWidth > 1) + math.select(0.0f, math.select(4f, -4f, (cellWidth & 1) != 0), startHalf);
                  x3.xz -= forward * num5;
                }
              }
              if (index1 == num4 - 1)
              {
                if (continuousEdge2)
                {
                  y1.xz += forward * math.select(0.0f, 4f, (cellWidth & 1) != 0 ^ endHalf);
                }
                else
                {
                  float num6 = num1 - math.select(0.0f, 8f, cellWidth > 1) + math.select(0.0f, math.select(4f, -4f, (cellWidth & 1) != 0), endHalf);
                  y1.xz += forward * num6;
                }
              }
              float2 float2_2 = MathUtils.Right(forward);
              float t3;
              float num7 = math.max(MathUtils.MaxDot(bezier4x3_1.xz, float2_2, out t3), MathUtils.MaxDot(bezier4x3_2.xz, float2_2, out t3)) - math.dot(x3.xz, float2_2);
              float b = math.distance(x3.xz, y1.xz);
              x3.xz += x1 * math.clamp(num7 / math.dot(x1, float2_2), 0.0f, b);
              y1.xz += x2 * math.clamp(num7 / math.dot(x2, float2_2), 0.0f, b);
              int num8 = (int) math.floor((float) (((double) math.length((y1 - x3).xz) + 0.10000000149011612) / 8.0));
              if (num8 >= 2)
              {
                float3 float3_8 = math.lerp(x3, y1, 0.5f);
                float2 float2_3 = forward * ((float) num8 * 4f);
                x3 = float3_8;
                x3.xz -= float2_3;
                y1 = float3_8;
                y1.xz += float2_3;
                float2 float2_4 = float2_1;
                float2 t4;
                if (MathUtils.Intersect(bezier4x3_1.xz, new Line2.Segment(x3.xz, x3.xz - float2_2 * 48f), out t4, 4))
                  float2_4.x = math.max(float2_4.x, math.lerp(float2_1.x, float2_1.y, t4.x * middleTangentPos));
                if (MathUtils.Intersect(bezier4x3_2.xz, new Line2.Segment(x3.xz, x3.xz - float2_2 * 48f), out t4, 4))
                  float2_4.x = math.max(float2_4.x, math.lerp(float2_1.x, float2_1.y, t4.x * (1f - middleTangentPos) + middleTangentPos));
                if (MathUtils.Intersect(bezier4x3_1.xz, new Line2.Segment(y1.xz, y1.xz - float2_2 * 48f), out t4, 4))
                  float2_4.y = math.min(float2_4.y, math.lerp(float2_1.x, float2_1.y, t4.x * middleTangentPos));
                if (MathUtils.Intersect(bezier4x3_2.xz, new Line2.Segment(y1.xz, y1.xz - float2_2 * 48f), out t4, 4))
                  float2_4.y = math.min(float2_4.y, math.lerp(float2_1.x, float2_1.y, t4.x * (1f - middleTangentPos) + middleTangentPos));
                float2 t5 = math.min((float2) 1f, float2_4 / middleTangentPos);
                float2 t6 = math.max((float2) 0.0f, (float2_4 - middleTangentPos) / (1f - middleTangentPos));
                if ((double) t5.x < 1.0)
                {
                  bezier4x3_1 = MathUtils.Cut(curve1, t5);
                }
                else
                {
                  ref Bezier4x3 local7 = ref bezier4x3_1;
                  ref Bezier4x3 local8 = ref bezier4x3_1;
                  ref Bezier4x3 local9 = ref bezier4x3_1;
                  bezier4x3_1.d = float3_1 = MathUtils.Position(curve2, t6.x);
                  float3 float3_9;
                  float3_1 = float3_9 = float3_1;
                  local9.c = float3_9;
                  float3 float3_10;
                  float3_1 = float3_10 = float3_1;
                  local8.b = float3_10;
                  float3 float3_11 = float3_1;
                  local7.a = float3_11;
                }
                if ((double) t5.y < 1.0)
                {
                  ref Bezier4x3 local10 = ref bezier4x3_2;
                  ref Bezier4x3 local11 = ref bezier4x3_2;
                  ref Bezier4x3 local12 = ref bezier4x3_2;
                  bezier4x3_2.d = float3_1 = MathUtils.Position(curve1, t5.y);
                  float3 float3_12;
                  float3_1 = float3_12 = float3_1;
                  local12.c = float3_12;
                  float3 float3_13;
                  float3_1 = float3_13 = float3_1;
                  local11.b = float3_13;
                  float3 float3_14 = float3_1;
                  local10.a = float3_14;
                }
                else
                  bezier4x3_2 = MathUtils.Cut(curve2, t6);
                float num9 = math.max(MathUtils.MaxDot(bezier4x3_1.xz, float2_2, out t3), MathUtils.MaxDot(bezier4x3_2.xz, float2_2, out t3)) - math.dot(x3.xz, float2_2);
                x3.xz += float2_2 * (num9 + 24f);
                int num10 = (num8 + 10 - 1) / 10;
                for (int index2 = 0; index2 < num10; ++index2)
                {
                  int y2 = index2 * num8 / num10;
                  int x4 = (index2 + 1) * num8 / num10;
                  Block block = new Block();
                  block.m_Position = x3;
                  block.m_Position.xz += forward * ((float) (y2 + x4) * 4f);
                  block.m_Direction = -float2_2;
                  block.m_Size.x = (int) (byte) (x4 - y2);
                  block.m_Size.y = 6;
                  CurvePosition component2 = new CurvePosition()
                  {
                    m_CurvePosition = math.lerp((float2) float2_1.x, (float2) float2_1.y, new float2((float) x4, (float) y2) / (float) num8)
                  };
                  component2.m_CurvePosition = math.select(component2.m_CurvePosition, 1f - component2.m_CurvePosition, invert);
                  if (endOrder > startOrder)
                    ++component1.m_Order;
                  else if (endOrder < startOrder)
                    --component1.m_Order;
                  Entity e;
                  if (oldBlockBuffer.TryGetValue(block, out e))
                  {
                    oldBlockBuffer.Remove(block);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, e, new PrefabRef(blockPrefab));
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<CurvePosition>(jobIndex, e, component2);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<BuildOrder>(jobIndex, e, component1);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(jobIndex, e, new Updated());
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    ZoneBlockData zoneBlockData = this.m_ZoneBlockData[blockPrefab];
                    // ISSUE: reference to a compiler-generated field
                    Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, zoneBlockData.m_Archetype);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(blockPrefab));
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<Block>(jobIndex, entity, block);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<CurvePosition>(jobIndex, entity, component2);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<BuildOrder>(jobIndex, entity, component1);
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Cell> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Cell>(jobIndex, entity);
                    int num11 = block.m_Size.x * block.m_Size.y;
                    for (int index3 = 0; index3 < num11; ++index3)
                      dynamicBuffer.Add(new Cell());
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Owner>(jobIndex, entity, new Owner()
                    {
                      m_Owner = owner
                    });
                  }
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
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.BuildOrder> __Game_Net_BuildOrder_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Road> __Game_Net_Road_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubBlock> __Game_Zones_SubBlock_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.BuildOrder> __Game_Net_BuildOrder_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadComposition> __Game_Prefabs_RoadComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneBlockData> __Game_Prefabs_ZoneBlockData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.BuildOrder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_SubBlock_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubBlock>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RO_ComponentLookup = state.GetComponentLookup<Game.Net.BuildOrder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadComposition_RO_ComponentLookup = state.GetComponentLookup<RoadComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneBlockData_RO_ComponentLookup = state.GetComponentLookup<ZoneBlockData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
      }
    }
  }
}
