// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityGraphDeleteSystem
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
  [RequireMatchingQueriesForUpdate]
  [CompilerGenerated]
  public class ElectricityGraphDeleteSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DeletedConnectionQuery;
    private EntityQuery m_DeletedValveNodeQuery;
    private ElectricityGraphDeleteSystem.TypeHandle __TypeHandle;

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
          ComponentType.ReadOnly<ElectricityNodeConnection>(),
          ComponentType.ReadOnly<ElectricityValveConnection>(),
          ComponentType.ReadOnly<ElectricityBuildingConnection>()
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
          ComponentType.ReadOnly<ElectricityValveConnection>(),
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
        this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ElectricityGraphDeleteSystem.DeleteConnectionsJob jobData = new ElectricityGraphDeleteSystem.DeleteConnectionsJob();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_NodeConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_ValveConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
        ref ElectricityGraphDeleteSystem.DeleteConnectionsJob local = ref jobData;
        // ISSUE: reference to a compiler-generated field
        commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
        EntityCommandBuffer.ParallelWriter parallelWriter = commandBuffer.AsParallelWriter();
        // ISSUE: reference to a compiler-generated field
        local.m_CommandBuffer = parallelWriter;
        // ISSUE: reference to a compiler-generated field
        jobHandle1 = jobData.ScheduleParallel<ElectricityGraphDeleteSystem.DeleteConnectionsJob>(this.m_DeletedConnectionQuery, this.Dependency);
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
        this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ElectricityGraphDeleteSystem.DeleteValveNodesJob jobData = new ElectricityGraphDeleteSystem.DeleteValveNodesJob();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_ValveConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        jobData.m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
        ref ElectricityGraphDeleteSystem.DeleteValveNodesJob local = ref jobData;
        // ISSUE: reference to a compiler-generated field
        commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
        EntityCommandBuffer.ParallelWriter parallelWriter = commandBuffer.AsParallelWriter();
        // ISSUE: reference to a compiler-generated field
        local.m_CommandBuffer = parallelWriter;
        // ISSUE: reference to a compiler-generated field
        jobHandle2 = jobData.ScheduleParallel<ElectricityGraphDeleteSystem.DeleteValveNodesJob>(this.m_DeletedValveNodeQuery, this.Dependency);
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
    public ElectricityGraphDeleteSystem()
    {
    }

    [BurstCompile]
    private struct DeleteConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<ElectricityNodeConnection> m_NodeConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityValveConnection> m_ValveConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
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
        foreach (ElectricityNodeConnection native in chunk.GetNativeArray<ElectricityNodeConnection>(ref this.m_NodeConnectionType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ElectricityGraphUtils.DeleteFlowNode(this.m_CommandBuffer, unfilteredChunkIndex, native.m_ElectricityNode, ref this.m_FlowConnections);
        }
        // ISSUE: reference to a compiler-generated field
        foreach (ElectricityValveConnection native in chunk.GetNativeArray<ElectricityValveConnection>(ref this.m_ValveConnectionType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ElectricityGraphUtils.DeleteFlowNode(this.m_CommandBuffer, unfilteredChunkIndex, native.m_ValveNode, ref this.m_FlowConnections);
        }
        // ISSUE: reference to a compiler-generated field
        foreach (ElectricityBuildingConnection native in chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ElectricityGraphUtils.DeleteBuildingNodes(this.m_CommandBuffer, unfilteredChunkIndex, native, ref this.m_FlowConnections, ref this.m_FlowEdges);
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
      public ComponentTypeHandle<ElectricityValveConnection> m_ValveConnectionType;
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
        NativeArray<ElectricityValveConnection> nativeArray2 = chunk.GetNativeArray<ElectricityValveConnection>(ref this.m_ValveConnectionType);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<ElectricityValveConnection>(unfilteredChunkIndex, nativeArray1);
        foreach (ElectricityValveConnection electricityValveConnection in nativeArray2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ElectricityGraphUtils.DeleteFlowNode(this.m_CommandBuffer, unfilteredChunkIndex, electricityValveConnection.m_ValveNode, ref this.m_FlowConnections);
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
      public ComponentTypeHandle<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityValveConnection> __Game_Simulation_ElectricityValveConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityValveConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityValveConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
      }
    }
  }
}
