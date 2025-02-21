// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipeOutsideConnectionGraphSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
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
  public class WaterPipeOutsideConnectionGraphSystem : GameSystemBase
  {
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private ModificationBarrier3 m_ModificationBarrier;
    private EntityQuery m_CreatedConnectionQuery;
    private WaterPipeOutsideConnectionGraphSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier3>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeOutsideConnection>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedConnectionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      WaterPipeOutsideConnectionGraphSystem.CreateOutsideConnectionsJob jobData = new WaterPipeOutsideConnectionGraphSystem.CreateOutsideConnectionsJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_WaterPipeNodeConnections = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EdgeArchetype = this.m_WaterPipeFlowSystem.edgeArchetype,
        m_SourceNode = this.m_WaterPipeFlowSystem.sourceNode,
        m_SinkNode = this.m_WaterPipeFlowSystem.sinkNode
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<WaterPipeOutsideConnectionGraphSystem.CreateOutsideConnectionsJob>(this.m_CreatedConnectionQuery, this.Dependency);
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
    public WaterPipeOutsideConnectionGraphSystem()
    {
    }

    [BurstCompile]
    private struct CreateOutsideConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> m_WaterPipeNodeConnections;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public EntityArchetype m_EdgeArchetype;
      public Entity m_SourceNode;
      public Entity m_SinkNode;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          WaterPipeNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaterPipeNodeConnections.TryGetComponent(nativeArray[index].m_Owner, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<TradeNode>(unfilteredChunkIndex, componentData.m_WaterPipeNode);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateOutsideFlowEdge(unfilteredChunkIndex, this.m_SourceNode, componentData.m_WaterPipeNode);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CreateOutsideFlowEdge(unfilteredChunkIndex, componentData.m_WaterPipeNode, this.m_SinkNode);
          }
        }
      }

      private void CreateOutsideFlowEdge(int jobIndex, Entity startNode, Entity endNode)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WaterPipeGraphUtils.CreateFlowEdge(this.m_CommandBuffer, jobIndex, this.m_EdgeArchetype, startNode, endNode, 0, 0);
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNodeConnection>(true);
      }
    }
  }
}
