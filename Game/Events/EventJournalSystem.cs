// Decompiled with JetBrains decompiler
// Type: Game.Events.EventJournalSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class EventJournalSystem : GameSystemBase, IEventJournalSystem
  {
    private EntityQuery m_StartedJournalQuery;
    private EntityQuery m_DeletedEventQuery;
    private EntityQuery m_JournalDataEventQuery;
    private EntityQuery m_ActiveJournalEffectQuery;
    private EntityQuery m_JournalEventPrefabQuery;
    private EntityQuery m_LoadedJournalQuery;
    private ISimulationSystem m_SimulationSystem;
    private IBudgetSystem m_BudgetSystem;
    private ICityServiceBudgetSystem m_CityServiceBudgetSystem;
    private CitySystem m_CitySystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private NativeQueue<Entity> m_Started;
    private NativeQueue<Entity> m_Changed;
    private NativeArray<int> m_CityEffects;
    private EventJournalSystem.TypeHandle __TypeHandle;

    public NativeList<Entity> eventJournal { get; private set; }

    public Action<Entity> eventEventDataChanged { get; set; }

    public System.Action eventEntryAdded { get; set; }

    public EventJournalEntry GetInfo(Entity journalEntity)
    {
      return this.EntityManager.GetComponentData<EventJournalEntry>(journalEntity);
    }

    public Entity GetPrefab(Entity journalEntity)
    {
      return this.EntityManager.GetComponentData<PrefabRef>(journalEntity).m_Prefab;
    }

    public bool TryGetData(Entity journalEntity, out DynamicBuffer<EventJournalData> data)
    {
      if (this.EntityManager.HasComponent<EventJournalData>(journalEntity))
      {
        data = this.EntityManager.GetBuffer<EventJournalData>(journalEntity, true);
        return true;
      }
      data = new DynamicBuffer<EventJournalData>();
      return false;
    }

    public bool TryGetCityEffects(
      Entity journalEntity,
      out DynamicBuffer<EventJournalCityEffect> data)
    {
      if (this.EntityManager.HasComponent<EventJournalCityEffect>(journalEntity))
      {
        data = this.EntityManager.GetBuffer<EventJournalCityEffect>(journalEntity, true);
        return true;
      }
      data = new DynamicBuffer<EventJournalCityEffect>();
      return false;
    }

    public IEnumerable<JournalEventComponent> eventPrefabs
    {
      get
      {
        EventJournalSystem eventJournalSystem = this;
        PrefabSystem prefabSystem = eventJournalSystem.World.GetExistingSystemManaged<PrefabSystem>();
        using (NativeArray<PrefabData> prefabs = eventJournalSystem.m_JournalEventPrefabQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          for (int i = 0; i < prefabs.Length; ++i)
            yield return prefabSystem.GetPrefab<EventPrefab>(prefabs[i]).GetComponent<JournalEventComponent>();
        }
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_StartedJournalQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<EventJournalEntry>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<EventJournalPending>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedEventQuery = this.GetEntityQuery(ComponentType.ReadOnly<JournalEvent>(), ComponentType.ReadOnly<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveJournalEffectQuery = this.GetEntityQuery(ComponentType.ReadWrite<EventJournalCityEffect>(), ComponentType.Exclude<EventJournalPending>(), ComponentType.Exclude<EventJournalCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataEventQuery = this.GetEntityQuery(ComponentType.ReadOnly<AddEventJournalData>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalEventPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<EventPrefab>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedJournalQuery = this.GetEntityQuery(ComponentType.ReadOnly<EventJournalEntry>(), ComponentType.Exclude<EventJournalPending>());
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = (ISimulationSystem) this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BudgetSystem = (IBudgetSystem) this.World.GetOrCreateSystemManaged<BudgetSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityServiceBudgetSystem = (ICityServiceBudgetSystem) this.World.GetOrCreateSystemManaged<CityServiceBudgetSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      this.eventJournal = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Started = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Changed = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CityEffects = new NativeArray<int>(5, Allocator.Persistent);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      this.eventJournal.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Changed.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Started.Clear();
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadedJournalQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_LoadedJournalQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<EventJournalEntry> componentDataArray = this.m_LoadedJournalQuery.ToComponentDataArray<EventJournalEntry>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<EventJournalSystem.JournalSortingInfo> array = new NativeArray<EventJournalSystem.JournalSortingInfo>(entityArray.Length, Allocator.TempJob);
      // ISSUE: variable of a compiler-generated type
      EventJournalSystem.JournalSortingInfo journalSortingInfo1;
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        ref NativeArray<EventJournalSystem.JournalSortingInfo> local = ref array;
        int index2 = index1;
        // ISSUE: object of a compiler-generated type is created
        journalSortingInfo1 = new EventJournalSystem.JournalSortingInfo();
        // ISSUE: reference to a compiler-generated field
        journalSortingInfo1.m_Entity = entityArray[index1];
        // ISSUE: reference to a compiler-generated field
        journalSortingInfo1.m_StartFrame = componentDataArray[index1].m_StartFrame;
        // ISSUE: variable of a compiler-generated type
        EventJournalSystem.JournalSortingInfo journalSortingInfo2 = journalSortingInfo1;
        local[index2] = journalSortingInfo2;
      }
      entityArray.Dispose();
      componentDataArray.Dispose();
      array.Sort<EventJournalSystem.JournalSortingInfo>();
      for (int index = 0; index < array.Length; ++index)
      {
        NativeList<Entity> eventJournal = this.eventJournal;
        ref NativeList<Entity> local1 = ref eventJournal;
        journalSortingInfo1 = array[index];
        // ISSUE: reference to a compiler-generated field
        ref Entity local2 = ref journalSortingInfo1.m_Entity;
        local1.Add(in local2);
      }
      array.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.eventJournal.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Changed.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Started.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CityEffects.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      bool flag = false;
      Entity entity1;
      // ISSUE: reference to a compiler-generated field
      while (this.m_Started.TryDequeue(out entity1))
      {
        this.eventJournal.Add(in entity1);
        flag = true;
      }
      if (flag)
      {
        System.Action eventEntryAdded = this.eventEntryAdded;
        if (eventEntryAdded != null)
          eventEntryAdded();
      }
      Entity entity2;
      // ISSUE: reference to a compiler-generated field
      while (this.m_Changed.TryDequeue(out entity2))
      {
        Action<Entity> eventDataChanged = this.eventEventDataChanged;
        if (eventDataChanged != null)
          eventDataChanged(entity2);
      }
      Population component1;
      Tourism component2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((!this.m_StartedJournalQuery.IsEmptyIgnoreFilter || !this.m_ActiveJournalEffectQuery.IsEmptyIgnoreFilter) && this.EntityManager.TryGetComponent<Population>(this.m_CitySystem.City, out component1) && this.EntityManager.TryGetComponent<Tourism>(this.m_CitySystem.City, out component2))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CityEffects[0] = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_CityEffects[1] = component1.m_AverageHappiness;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityEffects[2] = this.m_CityServiceBudgetSystem.GetTotalTaxIncome();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityEffects[3] = this.m_BudgetSystem.GetTotalTradeWorth();
        // ISSUE: reference to a compiler-generated field
        this.m_CityEffects[4] = component2.m_CurrentTourists;
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StartedJournalQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalEntry_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalCityEffect_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalPending_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EventJournalSystem.StartedEventsJob jobData = new EventJournalSystem.StartedEventsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PendingType = this.__TypeHandle.__Game_Events_EventJournalPending_RO_ComponentTypeHandle,
          m_CityEffectType = this.__TypeHandle.__Game_Events_EventJournalCityEffect_RW_BufferTypeHandle,
          m_EntryType = this.__TypeHandle.__Game_Events_EventJournalEntry_RW_ComponentTypeHandle,
          m_SimulationFrame = this.m_SimulationSystem.frameIndex,
          m_CityEffects = this.m_CityEffects,
          m_Started = this.m_Started.AsParallelWriter(),
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<EventJournalSystem.StartedEventsJob>(this.m_StartedJournalQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DeletedEventQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalEntry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalCompleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_JournalEvent_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EventJournalSystem.DeletedEventsJob jobData = new EventJournalSystem.DeletedEventsJob()
        {
          m_JournalEventType = this.__TypeHandle.__Game_Events_JournalEvent_RO_ComponentTypeHandle,
          m_CompletedData = this.__TypeHandle.__Game_Events_EventJournalCompleted_RO_ComponentLookup,
          m_EntryData = this.__TypeHandle.__Game_Events_EventJournalEntry_RW_ComponentLookup,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<EventJournalSystem.DeletedEventsJob>(this.m_DeletedEventQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ActiveJournalEffectQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_Fire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalEntry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: variable of a compiler-generated type
        EventJournalSystem.CheckJournalTrackingEndJob jobData1 = new EventJournalSystem.CheckJournalTrackingEndJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_EntryType = this.__TypeHandle.__Game_Events_EventJournalEntry_RO_ComponentTypeHandle,
          m_FireData = this.__TypeHandle.__Game_Events_Fire_RO_ComponentLookup,
          m_TargetElementData = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
          m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData1.ScheduleParallel<EventJournalSystem.CheckJournalTrackingEndJob>(this.m_ActiveJournalEffectQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Events_EventJournalCityEffect_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        EventJournalSystem.TrackCityEffectsJob jobData2 = new EventJournalSystem.TrackCityEffectsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CityEffectType = this.__TypeHandle.__Game_Events_EventJournalCityEffect_RW_BufferTypeHandle,
          m_CityEffects = this.m_CityEffects,
          m_Changes = this.m_Changed.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData2.ScheduleParallel<EventJournalSystem.TrackCityEffectsJob>(this.m_ActiveJournalEffectQuery, this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_JournalDataEventQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_EventJournalData_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_JournalEvent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AddEventJournalData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EventJournalSystem.TrackDataJob jobData3 = new EventJournalSystem.TrackDataJob()
      {
        m_AddDataType = this.__TypeHandle.__Game_Events_AddEventJournalData_RO_ComponentTypeHandle,
        m_JournalEvents = this.__TypeHandle.__Game_Events_JournalEvent_RO_ComponentLookup,
        m_EventJournalDatas = this.__TypeHandle.__Game_Events_EventJournalData_RW_BufferLookup,
        m_Changes = this.m_Changed
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData3.Schedule<EventJournalSystem.TrackDataJob>(this.m_JournalDataEventQuery, this.Dependency);
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
    public EventJournalSystem()
    {
    }

    [BurstCompile]
    private struct StartedEventsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<EventJournalPending> m_PendingType;
      public BufferTypeHandle<EventJournalCityEffect> m_CityEffectType;
      public ComponentTypeHandle<EventJournalEntry> m_EntryType;
      public uint m_SimulationFrame;
      [ReadOnly]
      public NativeArray<int> m_CityEffects;
      public NativeQueue<Entity>.ParallelWriter m_Started;
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
        NativeArray<EventJournalEntry> nativeArray2 = chunk.GetNativeArray<EventJournalEntry>(ref this.m_EntryType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<EventJournalCityEffect>(ref this.m_CityEffectType))
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<EventJournalCityEffect> bufferAccessor = chunk.GetBufferAccessor<EventJournalCityEffect>(ref this.m_CityEffectType);
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<EventJournalPending>(ref this.m_PendingType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<EventJournalPending> nativeArray3 = chunk.GetNativeArray<EventJournalPending>(ref this.m_PendingType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              if (nativeArray3[index].m_StartFrame <= this.m_SimulationFrame)
              {
                // ISSUE: reference to a compiler-generated method
                this.Init(bufferAccessor[index]);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<EventJournalPending>(unfilteredChunkIndex, nativeArray1[index]);
                // ISSUE: reference to a compiler-generated field
                EventJournalEntry eventJournalEntry = nativeArray2[index] with
                {
                  m_StartFrame = this.m_SimulationFrame
                };
                nativeArray2[index] = eventJournalEntry;
                // ISSUE: reference to a compiler-generated field
                this.m_Started.Enqueue(nativeArray1[index]);
              }
            }
          }
          else
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.Init(bufferAccessor[index]);
              // ISSUE: reference to a compiler-generated field
              EventJournalEntry eventJournalEntry = nativeArray2[index] with
              {
                m_StartFrame = this.m_SimulationFrame
              };
              nativeArray2[index] = eventJournalEntry;
              // ISSUE: reference to a compiler-generated field
              this.m_Started.Enqueue(nativeArray1[index]);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<EventJournalPending>(ref this.m_PendingType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<EventJournalPending> nativeArray4 = chunk.GetNativeArray<EventJournalPending>(ref this.m_PendingType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              if (nativeArray4[index].m_StartFrame <= this.m_SimulationFrame)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<EventJournalPending>(unfilteredChunkIndex, nativeArray1[index]);
                // ISSUE: reference to a compiler-generated field
                EventJournalEntry eventJournalEntry = nativeArray2[index] with
                {
                  m_StartFrame = this.m_SimulationFrame
                };
                nativeArray2[index] = eventJournalEntry;
                // ISSUE: reference to a compiler-generated field
                this.m_Started.Enqueue(nativeArray1[index]);
              }
            }
          }
          else
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              EventJournalEntry eventJournalEntry = nativeArray2[index] with
              {
                m_StartFrame = this.m_SimulationFrame
              };
              nativeArray2[index] = eventJournalEntry;
              // ISSUE: reference to a compiler-generated field
              this.m_Started.Enqueue(nativeArray1[index]);
            }
          }
        }
      }

      private void Init(DynamicBuffer<EventJournalCityEffect> effects)
      {
        for (int index = 0; index < effects.Length; ++index)
        {
          EventJournalCityEffect effect = effects[index];
          // ISSUE: reference to a compiler-generated field
          effect.m_StartValue = this.m_CityEffects[(int) effect.m_Type];
          effects[index] = effect;
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
    private struct DeletedEventsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<JournalEvent> m_JournalEventType;
      [ReadOnly]
      public ComponentLookup<EventJournalCompleted> m_CompletedData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<EventJournalEntry> m_EntryData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<JournalEvent> nativeArray = chunk.GetNativeArray<JournalEvent>(ref this.m_JournalEventType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity journalEntity = nativeArray[index].m_JournalEntity;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EntryData.HasComponent(journalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            EventJournalEntry eventJournalEntry = this.m_EntryData[journalEntity] with
            {
              m_Event = Entity.Null
            };
            // ISSUE: reference to a compiler-generated field
            this.m_EntryData[journalEntity] = eventJournalEntry;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CompletedData.HasComponent(journalEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<EventJournalCompleted>(unfilteredChunkIndex, nativeArray[index].m_JournalEntity);
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
    private struct TrackCityEffectsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public BufferTypeHandle<EventJournalCityEffect> m_CityEffectType;
      [ReadOnly]
      public NativeArray<int> m_CityEffects;
      public NativeQueue<Entity>.ParallelWriter m_Changes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<EventJournalCityEffect> bufferAccessor = chunk.GetBufferAccessor<EventJournalCityEffect>(ref this.m_CityEffectType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Update(bufferAccessor[index]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Changes.Enqueue(nativeArray[index]);
          }
        }
      }

      private bool Update(DynamicBuffer<EventJournalCityEffect> effects)
      {
        bool flag = false;
        for (int index = 0; index < effects.Length; ++index)
        {
          EventJournalCityEffect effect = effects[index];
          // ISSUE: reference to a compiler-generated field
          int cityEffect = this.m_CityEffects[(int) effect.m_Type];
          if (effect.m_Value != cityEffect)
          {
            flag = true;
            effect.m_Value = cityEffect;
            effects[index] = effect;
          }
        }
        return flag;
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
    private struct TrackDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<AddEventJournalData> m_AddDataType;
      [ReadOnly]
      public ComponentLookup<JournalEvent> m_JournalEvents;
      public BufferLookup<EventJournalData> m_EventJournalDatas;
      public NativeQueue<Entity> m_Changes;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Execute(chunk.GetNativeArray<AddEventJournalData>(ref this.m_AddDataType));
      }

      private void Execute(NativeArray<AddEventJournalData> addedDatas)
      {
        for (int index = 0; index < addedDatas.Length; ++index)
        {
          AddEventJournalData addedData = addedDatas[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_JournalEvents.HasComponent(addedData.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            Entity journalEntity = this.m_JournalEvents[addedDatas[index].m_Event].m_JournalEntity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EventJournalDatas.HasBuffer(journalEntity) && this.TryAddData(this.m_EventJournalDatas[journalEntity], addedData.m_Type, addedData.m_Count))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Changes.Enqueue(journalEntity);
            }
          }
        }
      }

      private bool TryAddData(
        DynamicBuffer<EventJournalData> eventJournalDatas,
        EventDataTrackingType type,
        int count)
      {
        for (int index = 0; index < eventJournalDatas.Length; ++index)
        {
          EventJournalData eventJournalData = eventJournalDatas[index];
          if (eventJournalData.m_Type == type)
          {
            eventJournalData.m_Value += count;
            eventJournalDatas[index] = eventJournalData;
            return true;
          }
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

    [BurstCompile]
    private struct CheckJournalTrackingEndJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<EventJournalEntry> m_EntryType;
      [ReadOnly]
      public ComponentLookup<Fire> m_FireData;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElementData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<EventJournalEntry> nativeArray1 = chunk.GetNativeArray<EventJournalEntry>(ref this.m_EntryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index].m_Event;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_FireData.HasComponent(entity) && this.m_TargetElementData.HasBuffer(entity) && this.CheckFireEnded(this.m_TargetElementData[entity]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EventJournalCompleted>(unfilteredChunkIndex, nativeArray2[index]);
          }
        }
      }

      private bool CheckFireEnded(DynamicBuffer<TargetElement> targetElements)
      {
        for (int index = 0; index < targetElements.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_OnFireData.HasComponent(targetElements[index].m_Entity))
            return false;
        }
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

    private struct JournalSortingInfo : IComparable<EventJournalSystem.JournalSortingInfo>
    {
      public Entity m_Entity;
      public uint m_StartFrame;

      public int CompareTo(EventJournalSystem.JournalSortingInfo other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) this.m_StartFrame - (int) other.m_StartFrame;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EventJournalPending> __Game_Events_EventJournalPending_RO_ComponentTypeHandle;
      public BufferTypeHandle<EventJournalCityEffect> __Game_Events_EventJournalCityEffect_RW_BufferTypeHandle;
      public ComponentTypeHandle<EventJournalEntry> __Game_Events_EventJournalEntry_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<JournalEvent> __Game_Events_JournalEvent_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<EventJournalCompleted> __Game_Events_EventJournalCompleted_RO_ComponentLookup;
      public ComponentLookup<EventJournalEntry> __Game_Events_EventJournalEntry_RW_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<EventJournalEntry> __Game_Events_EventJournalEntry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Fire> __Game_Events_Fire_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<AddEventJournalData> __Game_Events_AddEventJournalData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<JournalEvent> __Game_Events_JournalEvent_RO_ComponentLookup;
      public BufferLookup<EventJournalData> __Game_Events_EventJournalData_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalPending_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventJournalPending>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalCityEffect_RW_BufferTypeHandle = state.GetBufferTypeHandle<EventJournalCityEffect>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalEntry_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EventJournalEntry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_JournalEvent_RO_ComponentTypeHandle = state.GetComponentTypeHandle<JournalEvent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalCompleted_RO_ComponentLookup = state.GetComponentLookup<EventJournalCompleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalEntry_RW_ComponentLookup = state.GetComponentLookup<EventJournalEntry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalEntry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventJournalEntry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Fire_RO_ComponentLookup = state.GetComponentLookup<Fire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AddEventJournalData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AddEventJournalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_JournalEvent_RO_ComponentLookup = state.GetComponentLookup<JournalEvent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_EventJournalData_RW_BufferLookup = state.GetBufferLookup<EventJournalData>();
      }
    }
  }
}
