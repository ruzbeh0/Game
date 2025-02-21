// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityEdgeGraphSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ElectricityEdgeGraphSystem : GameSystemBase
  {
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private ModificationBarrier2B m_ModificationBarrier;
    private EntityQuery m_CreatedEdgeQuery;
    private ElectricityEdgeGraphSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2B>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedEdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.ElectricityConnection>(), ComponentType.ReadOnly<Edge>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedEdgeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeParallelHashMap<Entity, Entity> nativeParallelHashMap = new NativeParallelHashMap<Entity, Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ElectricityEdgeGraphSystem.CreateEdgeConnectionsJob jobData = new ElectricityEdgeGraphSystem.CreateEdgeConnectionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NetEdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ConnectedNetNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_ConnectedNetEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ElectricityConnectionDatas = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
        m_ElectricityNodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_NodeMap = nativeParallelHashMap,
        m_NodeArchetype = this.m_ElectricityFlowSystem.nodeArchetype,
        m_EdgeArchetype = this.m_ElectricityFlowSystem.edgeArchetype
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<ElectricityEdgeGraphSystem.CreateEdgeConnectionsJob>(this.m_CreatedEdgeQuery, this.Dependency);
      nativeParallelHashMap.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
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
    public ElectricityEdgeGraphSystem()
    {
    }

    [BurstCompile]
    private struct CreateEdgeConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_NetEdgeType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNetNodes;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedNetEdges;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> m_ElectricityConnectionDatas;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnections;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeParallelHashMap<Entity, Entity> m_NodeMap;
      public EntityArchetype m_NodeArchetype;
      public EntityArchetype m_EdgeArchetype;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray2 = chunk.GetNativeArray<Edge>(ref this.m_NetEdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity1 = nativeArray1[index];
          Entity start = nativeArray2[index].m_Start;
          Entity end = nativeArray2[index].m_End;
          // ISSUE: reference to a compiler-generated method
          BufferedEntity netNodeConnection1 = this.GetOrCreateNetNodeConnection(start);
          // ISSUE: reference to a compiler-generated method
          BufferedEntity netNodeConnection2 = this.GetOrCreateNetNodeConnection(end);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(this.m_NodeArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<ElectricityNodeConnection>(entity1, new ElectricityNodeConnection()
          {
            m_ElectricityNode = entity2
          });
          // ISSUE: reference to a compiler-generated field
          ElectricityConnectionData electricityConnectionData = this.m_ElectricityConnectionDatas[nativeArray3[index].m_Prefab];
          // ISSUE: reference to a compiler-generated method
          this.CreateFlowEdge(netNodeConnection1.m_Value, entity2, electricityConnectionData);
          // ISSUE: reference to a compiler-generated method
          this.CreateFlowEdge(entity2, netNodeConnection2.m_Value, electricityConnectionData);
          // ISSUE: reference to a compiler-generated method
          this.CreateEdgeMiddleNodeConnections(entity1, entity2);
          // ISSUE: reference to a compiler-generated method
          this.EnsureNodeEdgeConnections(start, netNodeConnection1, electricityConnectionData);
          // ISSUE: reference to a compiler-generated method
          this.EnsureNodeEdgeConnections(end, netNodeConnection2, electricityConnectionData);
        }
      }

      private void CreateEdgeMiddleNodeConnections(Entity netEdge, Entity flowMiddleNode)
      {
        DynamicBuffer<ConnectedNode> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedNetNodes.TryGetBuffer(netEdge, out bufferData))
          return;
        foreach (ConnectedNode connectedNode in bufferData)
        {
          PrefabRef componentData1;
          ElectricityConnectionData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefs.TryGetComponent(connectedNode.m_Node, out componentData1) && this.m_ElectricityConnectionDatas.TryGetComponent(componentData1.m_Prefab, out componentData2))
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            this.CreateFlowEdge(this.GetOrCreateNetNodeConnection(connectedNode.m_Node).m_Value, flowMiddleNode, componentData2);
          }
        }
      }

      private void EnsureNodeEdgeConnections(
        Entity netNode,
        BufferedEntity flowNode,
        ElectricityConnectionData connectionData)
      {
        // ISSUE: reference to a compiler-generated field
        foreach (ConnectedEdge connectedEdge in this.m_ConnectedNetEdges[netNode])
        {
          ElectricityNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElectricityNodeConnections.TryGetComponent(connectedEdge.m_Edge, out componentData))
          {
            Entity electricityNode = componentData.m_ElectricityNode;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!flowNode.m_Stored || !ElectricityGraphUtils.HasAnyFlowEdge(flowNode.m_Value, electricityNode, ref this.m_FlowConnections, ref this.m_FlowEdges))
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateFlowEdge(flowNode.m_Value, componentData.m_ElectricityNode, connectionData);
            }
          }
        }
      }

      private BufferedEntity GetOrCreateNetNodeConnection(Entity netNode)
      {
        ElectricityNodeConnection componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElectricityNodeConnections.TryGetComponent(netNode, out componentData))
          return new BufferedEntity(componentData.m_ElectricityNode, true);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NodeMap.TryGetValue(netNode, out entity1))
          return new BufferedEntity(entity1, false);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = this.m_CommandBuffer.CreateEntity(this.m_NodeArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<ElectricityNodeConnection>(netNode, new ElectricityNodeConnection()
        {
          m_ElectricityNode = entity2
        });
        // ISSUE: reference to a compiler-generated field
        this.m_NodeMap.Add(netNode, entity2);
        return new BufferedEntity(entity2, false);
      }

      private void CreateFlowEdge(
        Entity startNode,
        Entity endNode,
        ElectricityConnectionData connectionData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ElectricityGraphUtils.CreateFlowEdge(this.m_CommandBuffer, this.m_EdgeArchetype, startNode, endNode, connectionData.m_Direction, connectionData.m_Capacity);
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup = state.GetComponentLookup<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
      }
    }
  }
}
