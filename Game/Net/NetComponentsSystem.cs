// Decompiled with JetBrains decompiler
// Type: Game.Net.NetComponentsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class NetComponentsSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_UpdatedNodesQuery;
    private EntityQuery m_AllNodesQuery;
    private bool m_Loaded;
    private NetComponentsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedNodesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Node>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllNodesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Node>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_AllNodesQuery : this.m_UpdatedNodesQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Roundabout_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrafficLights_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle producerJob = new NetComponentsSystem.CheckNodeComponentsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TrafficLightsType = this.__TypeHandle.__Game_Net_TrafficLights_RO_ComponentTypeHandle,
        m_ConnectedEdgeType = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle,
        m_RoundaboutType = this.__TypeHandle.__Game_Net_Roundabout_RW_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<NetComponentsSystem.CheckNodeComponentsJob>(query, this.Dependency);
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
    public NetComponentsSystem()
    {
    }

    [BurstCompile]
    private struct CheckNodeComponentsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TrafficLights> m_TrafficLightsType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> m_ConnectedEdgeType;
      public ComponentTypeHandle<Roundabout> m_RoundaboutType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrafficLights> nativeArray2 = chunk.GetNativeArray<TrafficLights>(ref this.m_TrafficLightsType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Roundabout> nativeArray3 = chunk.GetNativeArray<Roundabout>(ref this.m_RoundaboutType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity e = nativeArray1[index1];
          DynamicBuffer<ConnectedEdge> dynamicBuffer = bufferAccessor[index1];
          CompositionFlags compositionFlags1 = new CompositionFlags();
          CompositionFlags compositionFlags2 = new CompositionFlags();
          if (nativeArray2.Length != 0)
          {
            if ((nativeArray2[index1].m_Flags & TrafficLightFlags.LevelCrossing) != (TrafficLightFlags) 0)
              compositionFlags1.m_General |= CompositionFlags.General.LevelCrossing;
            else
              compositionFlags1.m_General |= CompositionFlags.General.TrafficLights;
          }
          Roundabout component1;
          if (CollectionUtils.TryGet<Roundabout>(nativeArray3, index1, out component1))
            compositionFlags1.m_General |= CompositionFlags.General.Roundabout;
          component1.m_Radius = 0.0f;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity edge1 = dynamicBuffer[index2].m_Edge;
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1];
            bool flag = edge2.m_Start == e;
            bool c = edge2.m_End == e;
            Composition componentData;
            // ISSUE: reference to a compiler-generated field
            if (flag | c && this.m_CompositionData.TryGetComponent(edge1, out componentData))
            {
              // ISSUE: reference to a compiler-generated field
              NetCompositionData netCompositionData = this.m_PrefabCompositionData[flag ? componentData.m_StartNode : componentData.m_EndNode];
              compositionFlags2 |= netCompositionData.m_Flags;
              if ((netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                EdgeNodeGeometry edgeNodeGeometry = !flag ? this.m_EndNodeGeometryData[edge1].m_Geometry : this.m_StartNodeGeometryData[edge1].m_Geometry;
                float y = math.select(netCompositionData.m_RoundaboutSize.x, netCompositionData.m_RoundaboutSize.y, c) + edgeNodeGeometry.m_MiddleRadius;
                component1.m_Radius = math.max(component1.m_Radius, y);
              }
            }
          }
          CompositionFlags compositionFlags3 = compositionFlags1 ^ compositionFlags2;
          if ((compositionFlags3.m_General & (CompositionFlags.General.LevelCrossing | CompositionFlags.General.TrafficLights)) != (CompositionFlags.General) 0)
          {
            if ((compositionFlags2.m_General & CompositionFlags.General.LevelCrossing) != (CompositionFlags.General) 0)
            {
              if ((compositionFlags1.m_General & CompositionFlags.General.TrafficLights) != (CompositionFlags.General) 0)
              {
                TrafficLights component2 = nativeArray2[index1];
                component2.m_Flags |= TrafficLightFlags.LevelCrossing;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<TrafficLights>(unfilteredChunkIndex, e, component2);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TrafficLights>(unfilteredChunkIndex, e, new TrafficLights()
                {
                  m_Flags = TrafficLightFlags.LevelCrossing
                });
              }
            }
            else if ((compositionFlags2.m_General & CompositionFlags.General.TrafficLights) != (CompositionFlags.General) 0)
            {
              if ((compositionFlags1.m_General & CompositionFlags.General.LevelCrossing) != (CompositionFlags.General) 0)
              {
                TrafficLights component3 = nativeArray2[index1];
                component3.m_Flags &= ~TrafficLightFlags.LevelCrossing;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<TrafficLights>(unfilteredChunkIndex, e, component3);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<TrafficLights>(unfilteredChunkIndex, e, new TrafficLights());
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TrafficLights>(unfilteredChunkIndex, e);
            }
          }
          if ((compositionFlags3.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
          {
            if ((compositionFlags2.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Roundabout>(unfilteredChunkIndex, e, component1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Roundabout>(unfilteredChunkIndex, e);
            }
          }
          CollectionUtils.TrySet<Roundabout>(nativeArray3, index1, component1);
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
      public ComponentTypeHandle<TrafficLights> __Game_Net_TrafficLights_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferTypeHandle;
      public ComponentTypeHandle<Roundabout> __Game_Net_Roundabout_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrafficLights_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrafficLights>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Roundabout_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Roundabout>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
      }
    }
  }
}
