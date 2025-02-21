// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneHiddenSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class LaneHiddenSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_HiddenQuery;
    private LaneHiddenSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_HiddenQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Hidden>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<SubLane>(),
          ComponentType.ReadOnly<Lane>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HiddenQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new LaneHiddenSystem.HiddenSubObjectJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<LaneHiddenSystem.HiddenSubObjectJob>(this.m_HiddenQuery, this.Dependency);
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
    public LaneHiddenSystem()
    {
    }

    [BurstCompile]
    private struct HiddenSubObjectJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        if (bufferAccessor.Length != 0)
        {
          for (int index = 0; index < bufferAccessor.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.EnsureHidden(unfilteredChunkIndex, bufferAccessor[index]);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          StackList<Entity> stackList = (StackList<Entity>) stackalloc Entity[chunk.Count];
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_HiddenData.HasComponent(nativeArray2[index].m_Owner))
              stackList.AddNoResize(nativeArray1[index]);
          }
          if (stackList.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, stackList.AsArray());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(unfilteredChunkIndex, stackList.AsArray());
        }
      }

      private void EnsureHidden(int jobIndex, DynamicBuffer<SubLane> subLanes)
      {
        StackList<Entity> stackList = (StackList<Entity>) stackalloc Entity[subLanes.Length];
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_HiddenData.HasComponent(subLane))
            stackList.AddNoResize(subLane);
        }
        if (stackList.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, stackList.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, stackList.AsArray());
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
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
      }
    }
  }
}
