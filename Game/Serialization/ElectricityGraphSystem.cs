// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ElectricityGraphSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ElectricityGraphSystem : GameSystemBase
  {
    private EntityQuery m_NetEdgeQuery;
    private EntityQuery m_BuildingQuery;
    private ElectricityGraphSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetEdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.ElectricityConnection>(), ComponentType.ReadOnly<ElectricityNodeConnection>(), ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityBuildingConnection>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ElectricityGraphSystem.EdgeJob jobData1 = new ElectricityGraphSystem.EdgeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NetEdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_ConnectedNetNodeType = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferTypeHandle,
        m_ElectricityNodeConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ElectricityConnectionDatas = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
        m_ElectricityNodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<ElectricityGraphSystem.EdgeJob>(this.m_NetEdgeQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      ElectricityGraphSystem.BuildingJob jobData2 = new ElectricityGraphSystem.BuildingJob()
      {
        m_SubNetType = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_NetNodes = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ElectricityConnectionDatas = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
        m_ElectricityNodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_ElectricityValveConnections = this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData2.ScheduleParallel<ElectricityGraphSystem.BuildingJob>(this.m_BuildingQuery, this.Dependency);
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
    public ElectricityGraphSystem()
    {
    }

    [BurstCompile]
    private struct EdgeJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_NetEdgeType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> m_ConnectedNetNodeType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityNodeConnection> m_ElectricityNodeConnectionType;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> m_ElectricityConnectionDatas;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnections;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Edge> nativeArray3 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_NetEdgeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedNode> bufferAccessor = chunk.GetBufferAccessor<ConnectedNode>(ref this.m_ConnectedNetNodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityNodeConnection> nativeArray4 = chunk.GetNativeArray<ElectricityNodeConnection>(ref this.m_ElectricityNodeConnectionType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity start = nativeArray3[index].m_Start;
          Entity end = nativeArray3[index].m_End;
          // ISSUE: reference to a compiler-generated field
          Entity electricityNode1 = this.m_ElectricityNodeConnections[start].m_ElectricityNode;
          // ISSUE: reference to a compiler-generated field
          Entity electricityNode2 = this.m_ElectricityNodeConnections[end].m_ElectricityNode;
          Entity electricityNode3 = nativeArray4[index].m_ElectricityNode;
          ElectricityConnectionData componentData;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ElectricityConnectionDatas.TryGetComponent(nativeArray2[index].m_Prefab, out componentData))
          {
            componentData.m_Capacity = 400000;
            componentData.m_Direction = FlowDirection.Both;
          }
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          if ((1 & (this.UpdateFlowEdge(electricityNode1, electricityNode3, componentData) ? 1 : 0) & (this.UpdateFlowEdge(electricityNode3, electricityNode2, componentData) ? 1 : 0)) == 0)
            Debug.LogWarning((object) string.Format("ElectricityFlowEdge for net edge {0} not found!", (object) entity.Index));
          if (bufferAccessor.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateEdgeMiddleNodeConnections(bufferAccessor[index], electricityNode3);
          }
        }
      }

      private void UpdateEdgeMiddleNodeConnections(
        DynamicBuffer<ConnectedNode> connectedNodes,
        Entity flowMiddleNode)
      {
        foreach (ConnectedNode connectedNode in connectedNodes)
        {
          PrefabRef componentData1;
          ElectricityConnectionData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_PrefabRefs.TryGetComponent(connectedNode.m_Node, out componentData1) && this.m_ElectricityConnectionDatas.TryGetComponent(componentData1.m_Prefab, out componentData2) && !this.UpdateFlowEdge(this.m_ElectricityNodeConnections[connectedNode.m_Node].m_ElectricityNode, flowMiddleNode, componentData2))
            Debug.LogWarning((object) string.Format("ElectricityFlowEdge for connected node {0} not found!", (object) connectedNode.m_Node.Index));
        }
      }

      private bool UpdateFlowEdge(
        Entity startNode,
        Entity endNode,
        ElectricityConnectionData connectionData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (ElectricityGraphUtils.TrySetFlowEdge(startNode, endNode, connectionData.m_Direction, connectionData.m_Capacity, ref this.m_FlowConnections, ref this.m_FlowEdges))
          return true;
        Entity entity1;
        ElectricityFlowEdge edge;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!ElectricityGraphUtils.TryGetFlowEdge(endNode, startNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out entity1, out edge))
          return false;
        ref Entity local1 = ref edge.m_Start;
        ref Entity local2 = ref edge.m_End;
        Entity end = edge.m_End;
        Entity start = edge.m_Start;
        local1 = end;
        Entity entity2 = start;
        local2 = entity2;
        edge.direction = connectionData.m_Direction;
        edge.m_Capacity = connectionData.m_Capacity;
        edge.m_Flow = -edge.m_Flow;
        // ISSUE: reference to a compiler-generated field
        this.m_FlowEdges[entity1] = edge;
        return true;
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
    private struct BuildingJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> m_SubNetType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public ComponentLookup<Node> m_NetNodes;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> m_ElectricityConnectionDatas;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityValveConnection> m_ElectricityValveConnections;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubNet> bufferAccessor1 = chunk.GetBufferAccessor<Game.Net.SubNet>(ref this.m_SubNetType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType);
        NativeList<ElectricityGraphSystem.BuildingJob.MarkerNodeData> result = new NativeList<ElectricityGraphSystem.BuildingJob.MarkerNodeData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < chunk.Count; ++index)
        {
          result.Clear();
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindMarkerNodes(bufferAccessor1[index], result);
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindMarkerNodes(bufferAccessor2[index], result);
          }
          foreach (ElectricityGraphSystem.BuildingJob.MarkerNodeData markerNodeData in result)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateMarkerNode(markerNodeData, nativeArray[index]);
          }
        }
      }

      private void FindMarkerNodes(
        DynamicBuffer<InstalledUpgrade> upgrades,
        NativeList<ElectricityGraphSystem.BuildingJob.MarkerNodeData> result)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          InstalledUpgrade upgrade = upgrades[index];
          DynamicBuffer<Game.Net.SubNet> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive) && this.m_SubNets.TryGetBuffer(upgrade.m_Upgrade, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.FindMarkerNodes(bufferData, result);
          }
        }
      }

      private void FindMarkerNodes(
        DynamicBuffer<Game.Net.SubNet> subNets,
        NativeList<ElectricityGraphSystem.BuildingJob.MarkerNodeData> result)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          PrefabRef componentData1;
          ElectricityConnectionData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetNodes.HasComponent(subNet) && this.m_ElectricityValveConnections.HasComponent(subNet) && this.m_PrefabRefs.TryGetComponent(subNet, out componentData1) && this.m_ElectricityConnectionDatas.TryGetComponent(componentData1.m_Prefab, out componentData2))
          {
            // ISSUE: object of a compiler-generated type is created
            result.Add(new ElectricityGraphSystem.BuildingJob.MarkerNodeData()
            {
              m_NetNode = subNet,
              m_Capacity = componentData2.m_Capacity,
              m_Direction = componentData2.m_Direction
            });
          }
        }
      }

      private void UpdateMarkerNode(
        ElectricityGraphSystem.BuildingJob.MarkerNodeData markerNodeData,
        ElectricityBuildingConnection buildingNodes)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity electricityNode = this.m_ElectricityNodeConnections[markerNodeData.m_NetNode].m_ElectricityNode;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity valveNode = this.m_ElectricityValveConnections[markerNodeData.m_NetNode].m_ValveNode;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateFlowEdge(valveNode, electricityNode, markerNodeData.m_Direction, markerNodeData.m_Capacity);
        if (buildingNodes.m_TransformerNode != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateFlowEdge(buildingNodes.m_TransformerNode, valveNode, markerNodeData.m_Direction, markerNodeData.m_Capacity);
        }
        if (buildingNodes.m_ProducerEdge != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateFlowEdge(buildingNodes.GetProducerNode(ref this.m_FlowEdges), valveNode, FlowDirection.Forward, markerNodeData.m_Capacity);
        }
        if (buildingNodes.m_ConsumerEdge != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateFlowEdge(valveNode, buildingNodes.GetConsumerNode(ref this.m_FlowEdges), FlowDirection.Forward, markerNodeData.m_Capacity);
        }
        if (buildingNodes.m_ChargeEdge != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateFlowEdge(valveNode, buildingNodes.GetChargeNode(ref this.m_FlowEdges), FlowDirection.None, markerNodeData.m_Capacity);
        }
        if (!(buildingNodes.m_DischargeEdge != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateFlowEdge(buildingNodes.GetDischargeNode(ref this.m_FlowEdges), valveNode, FlowDirection.None, markerNodeData.m_Capacity);
      }

      private void UpdateFlowEdge(
        Entity startNode,
        Entity endNode,
        FlowDirection direction,
        int capacity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (ElectricityGraphUtils.TrySetFlowEdge(startNode, endNode, direction, capacity, ref this.m_FlowConnections, ref this.m_FlowEdges))
          return;
        Debug.LogWarning((object) string.Format("ElectricityFlowEdge from {0} to {1} not found!", (object) startNode, (object) endNode));
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

      private struct MarkerNodeData
      {
        public Entity m_NetNode;
        public int m_Capacity;
        public FlowDirection m_Direction;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityValveConnection> __Game_Simulation_ElectricityValveConnection_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup = state.GetComponentLookup<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityValveConnection>(true);
      }
    }
  }
}
