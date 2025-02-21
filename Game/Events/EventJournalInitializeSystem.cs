// Decompiled with JetBrains decompiler
// Type: Game.Events.EventJournalInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class EventJournalInitializeSystem : GameSystemBase
  {
    private EntityQuery m_CreatedEventQuery;
    private EntityArchetype m_EventJournalArchetype;
    private ModificationBarrier4 m_ModificationBarrier;
    private EventJournalInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedEventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Event>(), ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<JournalEvent>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventJournalArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<EventJournalEntry>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedEventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_JournalEventPrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EventJournalInitializeSystem.InitEventJournalEntriesJob jobData = new EventJournalInitializeSystem.InitEventJournalEntriesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_DurationType = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle,
        m_JournalEventPrefabDatas = this.__TypeHandle.__Game_Prefabs_JournalEventPrefabData_RO_ComponentLookup,
        m_JournalArchetype = this.m_EventJournalArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<EventJournalInitializeSystem.InitEventJournalEntriesJob>(this.m_CreatedEventQuery, this.Dependency);
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
    public EventJournalInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitEventJournalEntriesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Duration> m_DurationType;
      [ReadOnly]
      public ComponentLookup<JournalEventPrefabData> m_JournalEventPrefabDatas;
      public EntityArchetype m_JournalArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Duration>(ref this.m_DurationType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Duration> nativeArray3 = chunk.GetNativeArray<Duration>(ref this.m_DurationType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            Entity journalEntry = this.CreateJournalEntry(nativeArray2[index], nativeArray1[index].m_Prefab, unfilteredChunkIndex);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<JournalEvent>(unfilteredChunkIndex, nativeArray2[index], new JournalEvent()
            {
              m_JournalEntity = journalEntry
            });
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EventJournalPending>(unfilteredChunkIndex, journalEntry, new EventJournalPending()
            {
              m_StartFrame = nativeArray3[index].m_StartFrame
            });
          }
        }
        else
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            Entity journalEntry = this.CreateJournalEntry(nativeArray2[index], nativeArray1[index].m_Prefab, unfilteredChunkIndex);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<JournalEvent>(unfilteredChunkIndex, nativeArray2[index], new JournalEvent()
            {
              m_JournalEntity = journalEntry
            });
          }
        }
      }

      private Entity CreateJournalEntry(Entity eventEntity, Entity eventPrefab, int chunkIndex)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(chunkIndex, this.m_JournalArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<EventJournalEntry>(chunkIndex, entity, new EventJournalEntry()
        {
          m_Event = eventEntity
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(chunkIndex, entity, new PrefabRef()
        {
          m_Prefab = eventPrefab
        });
        // ISSUE: reference to a compiler-generated field
        JournalEventPrefabData journalEventPrefabData = this.m_JournalEventPrefabDatas[eventPrefab];
        if (journalEventPrefabData.m_DataFlags != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.InitDataBuffer(this.m_CommandBuffer.AddBuffer<EventJournalData>(chunkIndex, entity), journalEventPrefabData.m_DataFlags);
        }
        if (journalEventPrefabData.m_EffectFlags != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.InitEffectBuffer(this.m_CommandBuffer.AddBuffer<EventJournalCityEffect>(chunkIndex, entity), journalEventPrefabData.m_EffectFlags);
        }
        return entity;
      }

      private void InitDataBuffer(DynamicBuffer<EventJournalData> datas, int dataFlags)
      {
        for (int index = 0; index < 3; ++index)
        {
          if ((1 << index & dataFlags) != 0)
            datas.Add(new EventJournalData()
            {
              m_Type = (EventDataTrackingType) index,
              m_Value = 0
            });
        }
      }

      private void InitEffectBuffer(DynamicBuffer<EventJournalCityEffect> effects, int effectFlags)
      {
        for (int index = 0; index < 5; ++index)
        {
          if ((1 << index & effectFlags) != 0)
            effects.Add(new EventJournalCityEffect()
            {
              m_Type = (EventCityEffectTrackingType) index,
              m_StartValue = 0,
              m_Value = 0
            });
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<JournalEventPrefabData> __Game_Prefabs_JournalEventPrefabData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_JournalEventPrefabData_RO_ComponentLookup = state.GetComponentLookup<JournalEventPrefabData>(true);
      }
    }
  }
}
