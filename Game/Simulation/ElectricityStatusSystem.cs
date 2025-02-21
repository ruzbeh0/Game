// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityStatusSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
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
  public class ElectricityStatusSystem : GameSystemBase
  {
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_ElectricityParameterQuery;
    private ElectricityStatusSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => (int) sbyte.MaxValue;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Edge>(), ComponentType.ReadOnly<ElectricityNodeConnection>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ElectricityBuildingConnection>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<Game.Net.SubNet>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityParameterData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_EdgeQuery, this.m_BuildingQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ElectricityParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      IconCommandBuffer commandBuffer = this.m_IconCommandSystem.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      ElectricityParameterData singleton = this.m_ElectricityParameterQuery.GetSingleton<ElectricityParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new ElectricityStatusSystem.NetEdgeNotificationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ElectricityNodeConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle,
        m_ConnectedFlowEdges = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_SourceNode = this.m_ElectricityFlowSystem.sourceNode,
        m_SinkNode = this.m_ElectricityFlowSystem.sinkNode,
        m_ElectricityParameters = singleton,
        m_IconCommandBuffer = commandBuffer
      }.ScheduleParallel<ElectricityStatusSystem.NetEdgeNotificationJob>(this.m_EdgeQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new ElectricityStatusSystem.BuildingNotificationJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ElectricityBuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_ConnectedFlowEdges = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_SourceNode = this.m_ElectricityFlowSystem.sourceNode,
        m_ElectricityParameters = singleton,
        m_IconCommandBuffer = commandBuffer
      }.ScheduleParallel<ElectricityStatusSystem.BuildingNotificationJob>(this.m_BuildingQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
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
    public ElectricityStatusSystem()
    {
    }

    [BurstCompile]
    private struct NetEdgeNotificationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityNodeConnection> m_ElectricityNodeConnectionType;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public Entity m_SourceNode;
      public Entity m_SinkNode;
      public ElectricityParameterData m_ElectricityParameters;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityNodeConnection> nativeArray2 = chunk.GetNativeArray<ElectricityNodeConnection>(ref this.m_ElectricityNodeConnectionType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity owner = nativeArray1[index1];
          Entity electricityNode = nativeArray2[index1].m_ElectricityNode;
          bool flag = false;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedFlowEdge> connectedFlowEdge = this.m_ConnectedFlowEdges[electricityNode];
          for (int index2 = 0; index2 < connectedFlowEdge.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            ElectricityFlowEdge flowEdge = this.m_FlowEdges[connectedFlowEdge[index2].m_Edge];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (flowEdge.isBottleneck && flowEdge.m_Start != this.m_SourceNode && flowEdge.m_End != this.m_SinkNode)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(owner, this.m_ElectricityParameters.m_BottleneckNotificationPrefab, IconPriority.Problem);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(owner, this.m_ElectricityParameters.m_BottleneckNotificationPrefab);
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
    private struct BuildingNotificationJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_ElectricityBuildingConnectionType;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public Entity m_SourceNode;
      public ElectricityParameterData m_ElectricityParameters;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray2 = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_ElectricityBuildingConnectionType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity netEntity = nativeArray1[index1];
          ElectricityBuildingConnection buildingConnection = nativeArray2[index1];
          bool flag = false;
          DynamicBuffer<ConnectedFlowEdge> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (buildingConnection.m_TransformerNode != Entity.Null && this.m_ConnectedFlowEdges.TryGetBuffer(buildingConnection.m_TransformerNode, out bufferData))
          {
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_FlowEdges[bufferData[index2].m_Edge].isBottleneck)
              {
                flag = true;
                break;
              }
            }
          }
          bool enabled1 = false;
          ElectricityFlowEdge componentData1;
          // ISSUE: reference to a compiler-generated field
          if (buildingConnection.m_ProducerEdge != Entity.Null && this.m_FlowEdges.TryGetComponent(buildingConnection.m_ProducerEdge, out componentData1))
            enabled1 = componentData1.isBottleneck;
          bool enabled2 = false;
          ElectricityFlowEdge componentData2;
          // ISSUE: reference to a compiler-generated field
          if (buildingConnection.m_ConsumerEdge != Entity.Null && this.m_FlowEdges.TryGetComponent(buildingConnection.m_ConsumerEdge, out componentData2))
            enabled2 = componentData2.isBeyondBottleneck;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SetProblemNotification(netEntity, this.m_ElectricityParameters.m_TransformerNotificationPrefab, !enabled1 & flag);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SetProblemNotification(netEntity, this.m_ElectricityParameters.m_NotEnoughProductionNotificationPrefab, enabled1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SetProblemNotification(netEntity, this.m_ElectricityParameters.m_BottleneckNotificationPrefab, enabled2);
        }
      }

      private void SetProblemNotification(Entity netEntity, Entity prefab, bool enabled)
      {
        if (enabled)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(netEntity, prefab, IconPriority.Problem);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(netEntity, prefab);
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
      public ComponentTypeHandle<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
      }
    }
  }
}
