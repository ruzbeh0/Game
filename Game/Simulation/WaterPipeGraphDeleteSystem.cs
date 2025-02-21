// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipeGraphDeleteSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterPipeGraphDeleteSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DeletedConnectionQuery;
    private EntityQuery m_DeletedValveNodeQuery;
    private WaterPipeGraphDeleteSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedConnectionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<WaterPipeNodeConnection>(),
          ComponentType.ReadOnly<WaterPipeValveConnection>(),
          ComponentType.ReadOnly<WaterPipeBuildingConnection>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedValveNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<WaterPipeValveConnection>(),
          ComponentType.ReadOnly<Node>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_DeletedConnectionQuery, this.m_DeletedValveNodeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle jobHandle1 = new JobHandle();
      EntityCommandBuffer commandBuffer;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DeletedConnectionQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterPipeValveConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaterPipeGraphDeleteSystem.DeleteConnectionsJob jobData = new WaterPipeGraphDeleteSystem.DeleteConnectionsJob();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_NodeConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_ValveConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeValveConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
        ref WaterPipeGraphDeleteSystem.DeleteConnectionsJob local = ref jobData;
        // ISSUE: reference to a compiler-generated field
        commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
        EntityCommandBuffer.ParallelWriter parallelWriter = commandBuffer.AsParallelWriter();
        // ISSUE: reference to a compiler-generated field
        local.m_CommandBuffer = parallelWriter;
        // ISSUE: reference to a compiler-generated field
        jobHandle1 = jobData.ScheduleParallel<WaterPipeGraphDeleteSystem.DeleteConnectionsJob>(this.m_DeletedConnectionQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle1);
      }
      JobHandle jobHandle2 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DeletedValveNodeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterPipeValveConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaterPipeGraphDeleteSystem.DeleteValveNodesJob jobData = new WaterPipeGraphDeleteSystem.DeleteValveNodesJob();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_ValveConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeValveConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
        ref WaterPipeGraphDeleteSystem.DeleteValveNodesJob local = ref jobData;
        // ISSUE: reference to a compiler-generated field
        commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
        EntityCommandBuffer.ParallelWriter parallelWriter = commandBuffer.AsParallelWriter();
        // ISSUE: reference to a compiler-generated field
        local.m_CommandBuffer = parallelWriter;
        // ISSUE: reference to a compiler-generated field
        jobHandle2 = jobData.ScheduleParallel<WaterPipeGraphDeleteSystem.DeleteValveNodesJob>(this.m_DeletedValveNodeQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      }
      this.Dependency = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
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
    public WaterPipeGraphDeleteSystem()
    {
    }

    [BurstCompile]
    private struct DeleteConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeNodeConnection> m_NodeConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeValveConnection> m_ValveConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        foreach (WaterPipeNodeConnection native in chunk.GetNativeArray<WaterPipeNodeConnection>(ref this.m_NodeConnectionType))
        {
          if (native.m_WaterPipeNode != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WaterPipeGraphUtils.DeleteFlowNode(this.m_CommandBuffer, unfilteredChunkIndex, native.m_WaterPipeNode, ref this.m_FlowConnections);
          }
          else
            UnityEngine.Debug.LogWarning((object) "Found null WaterPipeNode");
        }
        // ISSUE: reference to a compiler-generated field
        foreach (WaterPipeValveConnection native in chunk.GetNativeArray<WaterPipeValveConnection>(ref this.m_ValveConnectionType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WaterPipeGraphUtils.DeleteFlowNode(this.m_CommandBuffer, unfilteredChunkIndex, native.m_ValveNode, ref this.m_FlowConnections);
        }
        // ISSUE: reference to a compiler-generated field
        foreach (WaterPipeBuildingConnection native in chunk.GetNativeArray<WaterPipeBuildingConnection>(ref this.m_BuildingConnectionType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WaterPipeGraphUtils.DeleteBuildingNodes(this.m_CommandBuffer, unfilteredChunkIndex, native, ref this.m_FlowConnections, ref this.m_FlowEdges);
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

    [BurstCompile]
    private struct DeleteValveNodesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeValveConnection> m_ValveConnectionType;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
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
        NativeArray<WaterPipeValveConnection> nativeArray2 = chunk.GetNativeArray<WaterPipeValveConnection>(ref this.m_ValveConnectionType);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<WaterPipeValveConnection>(unfilteredChunkIndex, nativeArray1);
        foreach (WaterPipeValveConnection pipeValveConnection in nativeArray2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WaterPipeGraphUtils.DeleteFlowNode(this.m_CommandBuffer, unfilteredChunkIndex, pipeValveConnection.m_ValveNode, ref this.m_FlowConnections);
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
      public ComponentTypeHandle<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeValveConnection> __Game_Simulation_WaterPipeValveConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeValveConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeValveConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
