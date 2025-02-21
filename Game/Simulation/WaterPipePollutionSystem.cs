// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipePollutionSystem
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
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterPipePollutionSystem : GameSystemBase
  {
    private const int kUpdateInterval = 64;
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_NodeQuery;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_ParameterQuery;
    private WaterPipePollutionSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeQuery = this.GetEntityQuery(ComponentType.ReadWrite<WaterPipeNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadWrite<WaterPipeEdge>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_NodeQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EdgeQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      WaterPipeParameterData singleton = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>();
      // ISSUE: reference to a compiler-generated field
      bool flag = (ulong) (this.m_SimulationSystem.frameIndex / 64U) % (ulong) singleton.m_WaterPipePollutionSpreadInterval > 0UL;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new WaterPipePollutionSystem.NodePollutionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_FlowConnectionType = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup,
        m_StaleWaterPipePurification = singleton.m_StaleWaterPipePurification
      }.ScheduleParallel<WaterPipePollutionSystem.NodePollutionJob>(this.m_NodeQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNode_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaterPipePollutionSystem.EdgePollutionJob jobData = new WaterPipePollutionSystem.EdgePollutionJob()
      {
        m_FlowEdgeType = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle,
        m_FlowNodes = this.__TypeHandle.__Game_Simulation_WaterPipeNode_RO_ComponentLookup,
        m_SourceNode = this.m_WaterPipeFlowSystem.sourceNode,
        m_Purify = flag
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<WaterPipePollutionSystem.EdgePollutionJob>(this.m_EdgeQuery, dependsOn);
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
    public WaterPipePollutionSystem()
    {
    }

    [BurstCompile]
    public struct NodePollutionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> m_FlowConnectionType;
      public ComponentTypeHandle<WaterPipeNode> m_NodeType;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      public float m_StaleWaterPipePurification;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedFlowEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedFlowEdge>(ref this.m_FlowConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeNode> nativeArray2 = chunk.GetNativeArray<WaterPipeNode>(ref this.m_NodeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray1[index1];
          ref WaterPipeNode local = ref nativeArray2.ElementAt<WaterPipeNode>(index1);
          DynamicBuffer<ConnectedFlowEdge> dynamicBuffer = bufferAccessor[index1];
          int num1 = 0;
          float num2 = 0.0f;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            WaterPipeEdge flowEdge = this.m_FlowEdges[dynamicBuffer[index2].m_Edge];
            int num3 = flowEdge.m_Start == entity ? -flowEdge.m_FreshFlow : flowEdge.m_FreshFlow;
            if (num3 > 0)
            {
              num1 += num3;
              num2 += flowEdge.m_FreshPollution * (float) num3;
            }
          }
          // ISSUE: reference to a compiler-generated field
          local.m_FreshPollution = num1 <= 0 ? math.max(0.0f, local.m_FreshPollution - this.m_StaleWaterPipePurification) : num2 / (float) num1;
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
    public struct EdgePollutionJob : IJobChunk
    {
      public ComponentTypeHandle<WaterPipeEdge> m_FlowEdgeType;
      [ReadOnly]
      public ComponentLookup<WaterPipeNode> m_FlowNodes;
      public Entity m_SourceNode;
      public bool m_Purify;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeEdge> nativeArray = chunk.GetNativeArray<WaterPipeEdge>(ref this.m_FlowEdgeType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          ref WaterPipeEdge local = ref nativeArray.ElementAt<WaterPipeEdge>(index);
          // ISSUE: reference to a compiler-generated field
          WaterPipeNode flowNode1 = this.m_FlowNodes[local.m_Start];
          // ISSUE: reference to a compiler-generated field
          WaterPipeNode flowNode2 = this.m_FlowNodes[local.m_End];
          // ISSUE: reference to a compiler-generated field
          if (local.m_Start != this.m_SourceNode)
          {
            float num = local.m_FreshFlow <= 0 ? (local.m_FreshFlow >= 0 ? (float) (((double) flowNode1.m_FreshPollution + (double) flowNode2.m_FreshPollution) / 2.0) : flowNode2.m_FreshPollution) : flowNode1.m_FreshPollution;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Purify || (double) num == 0.0)
              local.m_FreshPollution = num;
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
      public BufferTypeHandle<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle;
      public ComponentTypeHandle<WaterPipeNode> __Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
      public ComponentTypeHandle<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeNode> __Game_Simulation_WaterPipeNode_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNode_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNode>(true);
      }
    }
  }
}
