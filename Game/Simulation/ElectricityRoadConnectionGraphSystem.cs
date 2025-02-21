// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityRoadConnectionGraphSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
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
  public class ElectricityRoadConnectionGraphSystem : GameSystemBase
  {
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_EventQuery;
    private NativeQueue<Entity> m_UpdatedEdges;
    private JobHandle m_WriteDependencies;
    private ElectricityRoadConnectionGraphSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<RoadConnectionUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedEdges = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedEdges.Dispose();
      base.OnDestroy();
    }

    public NativeQueue<Entity> GetEdgeUpdateQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdatedEdges;
    }

    public void AddQueueWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, handle);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.m_WriteDependencies);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_EventQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ElectricityRoadConnectionGraphSystem.UpdateRoadConnectionsJob jobData = new ElectricityRoadConnectionGraphSystem.UpdateRoadConnectionsJob()
        {
          m_RoadConnectionUpdatedType = this.__TypeHandle.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle,
          m_ElectricityConsumers = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
          m_Deleted = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_UpdatedEdges = this.m_UpdatedEdges.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<ElectricityRoadConnectionGraphSystem.UpdateRoadConnectionsJob>(this.m_EventQuery, this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      ElectricityRoadConnectionGraphSystem.UpdateRoadEdgesJob jobData1 = new ElectricityRoadConnectionGraphSystem.UpdateRoadEdgesJob()
      {
        m_ElectricityConsumers = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_NodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_BuildingConnections = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup,
        m_ConnectedBuildings = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_UpdatedEdges = this.m_UpdatedEdges,
        m_SinkNode = this.m_ElectricityFlowSystem.sinkNode,
        m_EdgeArchetype = this.m_ElectricityFlowSystem.edgeArchetype
      };
      this.Dependency = jobData1.Schedule<ElectricityRoadConnectionGraphSystem.UpdateRoadEdgesJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
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
    public ElectricityRoadConnectionGraphSystem()
    {
    }

    [BurstCompile]
    private struct UpdateRoadConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<RoadConnectionUpdated> m_RoadConnectionUpdatedType;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleted;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<RoadConnectionUpdated> nativeArray = chunk.GetNativeArray<RoadConnectionUpdated>(ref this.m_RoadConnectionUpdatedType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          RoadConnectionUpdated connectionUpdated = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElectricityConsumers.HasComponent(connectionUpdated.m_Building))
          {
            // ISSUE: reference to a compiler-generated field
            if (connectionUpdated.m_Old != Entity.Null && !this.m_Deleted.HasComponent(connectionUpdated.m_Old))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedEdges.Enqueue(connectionUpdated.m_Old);
            }
            if (connectionUpdated.m_New != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedEdges.Enqueue(connectionUpdated.m_New);
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

    [BurstCompile]
    private struct UpdateRoadEdgesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_NodeConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> m_BuildingConnections;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> m_ConnectedBuildings;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<Entity> m_UpdatedEdges;
      public Entity m_SinkNode;
      public EntityArchetype m_EdgeArchetype;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_UpdatedEdges.Count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_UpdatedEdges.TryDequeue(out entity))
        {
          ElectricityNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (nativeParallelHashSet.Add(entity) && this.m_NodeConnections.TryGetComponent(entity, out componentData))
          {
            int consumption;
            // ISSUE: reference to a compiler-generated method
            if (this.HasConsumersWithoutBuildingSinkConnection(entity, out consumption))
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateOrUpdateRoadEdgeSinkConnection(componentData.m_ElectricityNode, consumption);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.ClearRoadEdgeSinkConnection(componentData.m_ElectricityNode);
            }
          }
        }
      }

      private bool HasConsumersWithoutBuildingSinkConnection(Entity roadEdge, out int consumption)
      {
        bool flag = false;
        consumption = 0;
        DynamicBuffer<ConnectedBuilding> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedBuildings.TryGetBuffer(roadEdge, out bufferData))
        {
          foreach (ConnectedBuilding connectedBuilding in bufferData)
          {
            ElectricityConsumer componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElectricityConsumers.TryGetComponent(connectedBuilding.m_Building, out componentData) && !this.m_BuildingConnections.HasComponent(connectedBuilding.m_Building))
            {
              flag = true;
              consumption += componentData.m_WantedConsumption;
            }
          }
        }
        return flag;
      }

      private void CreateOrUpdateRoadEdgeSinkConnection(Entity roadEdgeFlowNode, int capacity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (ElectricityGraphUtils.TrySetFlowEdge(roadEdgeFlowNode, this.m_SinkNode, FlowDirection.Forward, capacity, ref this.m_FlowConnections, ref this.m_FlowEdges))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ElectricityGraphUtils.CreateFlowEdge(this.m_CommandBuffer, this.m_EdgeArchetype, roadEdgeFlowNode, this.m_SinkNode, FlowDirection.Forward, capacity);
      }

      private void ClearRoadEdgeSinkConnection(Entity roadEdgeFlowNode)
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!ElectricityGraphUtils.TryGetFlowEdge(roadEdgeFlowNode, this.m_SinkNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out entity))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(entity);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<RoadConnectionUpdated> __Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RoadConnectionUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferLookup = state.GetBufferLookup<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>();
      }
    }
  }
}
