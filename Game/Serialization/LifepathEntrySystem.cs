// Decompiled with JetBrains decompiler
// Type: Game.Serialization.LifepathEntrySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class LifepathEntrySystem : GameSystemBase
  {
    private EntityQuery m_ChirpQuery;
    private DeserializationBarrier m_DeserializationBarrier;
    private LifepathEntrySystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Chirp>(), ComponentType.ReadOnly<LifePathEvent>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeserializationBarrier = this.World.GetOrCreateSystemManaged<DeserializationBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ChirpQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_Chirp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LifepathEntrySystem.FixLifepathChirpReferencesJob jobData = new LifepathEntrySystem.FixLifepathChirpReferencesJob()
      {
        m_EntityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ChirpType = this.__TypeHandle.__Game_Triggers_Chirp_RO_ComponentTypeHandle,
        m_EntryDatas = this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferLookup,
        m_CommandBuffer = this.m_DeserializationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<LifepathEntrySystem.FixLifepathChirpReferencesJob>(this.m_ChirpQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_DeserializationBarrier.AddJobHandleForProducer(this.Dependency);
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

    [Preserve]
    public LifepathEntrySystem()
    {
    }

    public struct FixLifepathChirpReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Chirp> m_ChirpType;
      [ReadOnly]
      public BufferLookup<LifePathEntry> m_EntryDatas;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityTypeHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Chirp> nativeArray2 = chunk.GetNativeArray<Chirp>(ref this.m_ChirpType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity sender = nativeArray2[index].m_Sender;
          DynamicBuffer<LifePathEntry> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EntryDatas.TryGetBuffer(sender, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.Contains(bufferData, entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AppendToBuffer<LifePathEntry>(unfilteredChunkIndex, sender, new LifePathEntry(entity));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity);
          }
        }
      }

      private bool Contains(DynamicBuffer<LifePathEntry> entries, Entity chirp)
      {
        for (int index = 0; index < entries.Length; ++index)
        {
          if (entries[index].m_Entity == chirp)
            return true;
        }
        return false;
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
      public ComponentTypeHandle<Chirp> __Game_Triggers_Chirp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<LifePathEntry> __Game_Triggers_LifePathEntry_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_Chirp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Chirp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_LifePathEntry_RO_BufferLookup = state.GetBufferLookup<LifePathEntry>(true);
      }
    }
  }
}
