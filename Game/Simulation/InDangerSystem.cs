// Decompiled with JetBrains decompiler
// Type: Game.Simulation.InDangerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Events;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class InDangerSystem : GameSystemBase
  {
    public const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_InDangerQuery;
    private EntityArchetype m_EvacuationRequestArchetype;
    private InDangerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_InDangerQuery = this.GetEntityQuery(ComponentType.ReadWrite<InDanger>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EvacuationRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<EvacuationRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_InDangerQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_EvacuationRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new InDangerSystem.InDangerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_InDangerType = this.__TypeHandle.__Game_Events_InDanger_RW_ComponentTypeHandle,
        m_DurationData = this.__TypeHandle.__Game_Events_Duration_RO_ComponentLookup,
        m_EvacuationRequestData = this.__TypeHandle.__Game_Simulation_EvacuationRequest_RO_ComponentLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_EvacuationRequestArchetype = this.m_EvacuationRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<InDangerSystem.InDangerJob>(this.m_InDangerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
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
    public InDangerSystem()
    {
    }

    [BurstCompile]
    private struct InDangerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<InDanger> m_InDangerType;
      [ReadOnly]
      public ComponentLookup<Duration> m_DurationData;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> m_EvacuationRequestData;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EntityArchetype m_EvacuationRequestArchetype;
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
        NativeArray<InDanger> nativeArray2 = chunk.GetNativeArray<InDanger>(ref this.m_InDangerType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          InDanger inDanger = nativeArray2[index];
          // ISSUE: reference to a compiler-generated method
          if (!this.IsStillInDanger(ref inDanger))
          {
            inDanger.m_Flags = (DangerFlags) 0;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<InDanger>(unfilteredChunkIndex, entity);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, entity, new EffectsUpdated());
          }
          if ((inDanger.m_Flags & (DangerFlags.Evacuate | DangerFlags.UseTransport | DangerFlags.WaitingCitizens)) == (DangerFlags.Evacuate | DangerFlags.UseTransport | DangerFlags.WaitingCitizens))
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestEvacuationIfNeeded(unfilteredChunkIndex, entity, ref inDanger);
          }
          nativeArray2[index] = inDanger;
        }
      }

      private bool IsStillInDanger(ref InDanger inDanger)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_SimulationFrame < inDanger.m_EndFrame && this.m_DurationData.HasComponent(inDanger.m_Event) && this.m_SimulationFrame < this.m_DurationData[inDanger.m_Event].m_EndFrame;
      }

      private void RequestEvacuationIfNeeded(int jobIndex, Entity entity, ref InDanger inDanger)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EvacuationRequestData.HasComponent(inDanger.m_EvacuationRequest))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_EvacuationRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<EvacuationRequest>(jobIndex, entity1, new EvacuationRequest(entity, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(4U));
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
      public ComponentTypeHandle<InDanger> __Game_Events_InDanger_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Duration> __Game_Events_Duration_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> __Game_Simulation_EvacuationRequest_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RW_ComponentTypeHandle = state.GetComponentTypeHandle<InDanger>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentLookup = state.GetComponentLookup<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_EvacuationRequest_RO_ComponentLookup = state.GetComponentLookup<EvacuationRequest>(true);
      }
    }
  }
}
