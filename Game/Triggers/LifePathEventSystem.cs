// Decompiled with JetBrains decompiler
// Type: Game.Triggers.LifePathEventSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Triggers
{
  [CompilerGenerated]
  public class LifePathEventSystem : GameSystemBase
  {
    public static readonly int kMaxFollowed = 50;
    private SimulationSystem m_SimulationSystem;
    private ModificationEndBarrier m_ModificationBarrier;
    private CreateChirpSystem m_CreateChirpSystem;
    private EntityQuery m_FollowedQuery;
    private EntityQuery m_DeletedFollowedQuery;
    private EntityQuery m_TimeDataQuery;
    private EntityArchetype m_EventArchetype;
    private NativeQueue<LifePathEventCreationData> m_Queue;
    private JobHandle m_WriteDependencies;
    private LifePathEventSystem.TypeHandle __TypeHandle;

    public bool m_DebugLifePathChirps { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreateChirpSystem = this.World.GetOrCreateSystemManaged<CreateChirpSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FollowedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Followed>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedFollowedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Followed>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<LifePathEvent>());
      // ISSUE: reference to a compiler-generated field
      this.m_Queue = new NativeQueue<LifePathEventCreationData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TimeDataQuery);
      this.Enabled = false;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Queue.Dispose();
      base.OnDestroy();
    }

    public NativeQueue<LifePathEventCreationData> GetQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_Queue;
    }

    public void AddQueueWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, handle);
    }

    public bool FollowCitizen(Entity citizen)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_FollowedQuery.CalculateEntityCount() >= LifePathEventSystem.kMaxFollowed)
        return false;
      Citizen component;
      bool flag = this.EntityManager.TryGetComponent<Citizen>(citizen, out component) && component.GetAge() == CitizenAge.Child;
      EntityManager entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      entityManager.AddComponentData<Followed>(citizen, new Followed()
      {
        m_Priority = this.m_SimulationSystem.frameIndex,
        m_StartedFollowingAsChild = flag
      });
      entityManager = this.EntityManager;
      entityManager.AddBuffer<LifePathEntry>(citizen);
      entityManager = this.EntityManager;
      entityManager.AddComponent<Updated>(citizen);
      return true;
    }

    public bool UnfollowCitizen(Entity citizen)
    {
      if (!this.EntityManager.HasComponent<Followed>(citizen))
        return false;
      this.EntityManager.RemoveComponent<Followed>(citizen);
      DynamicBuffer<LifePathEntry> buffer;
      if (this.EntityManager.TryGetBuffer<LifePathEntry>(citizen, true, out buffer))
      {
        foreach (LifePathEntry lifePathEntry in buffer)
          this.EntityManager.AddComponent<Deleted>(lifePathEntry.m_Entity);
      }
      this.EntityManager.RemoveComponent<LifePathEntry>(citizen);
      this.EntityManager.AddComponent<Updated>(citizen);
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LifePathEventSystem.CleanupLifePathEntriesJob jobData1 = new LifePathEventSystem.CleanupLifePathEntriesJob()
      {
        m_EntryType = this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferTypeHandle,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<LifePathEventSystem.CleanupLifePathEntriesJob>(this.m_DeletedFollowedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LifePathEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LifePathEventSystem.CreateLifePathEventJob jobData2 = new LifePathEventSystem.CreateLifePathEventJob()
      {
        m_LifePathEventDatas = this.__TypeHandle.__Game_Prefabs_LifePathEventData_RO_ComponentLookup,
        m_LifePathEntries = this.__TypeHandle.__Game_Triggers_LifePathEntry_RO_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_EventArchetype = this.m_EventArchetype,
        m_Queue = this.m_Queue,
        m_ChirpQueue = this.m_CreateChirpSystem.GetQueue(out deps),
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_DebugLifePathChirps = this.m_DebugLifePathChirps
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData2.Schedule<LifePathEventSystem.CreateLifePathEventJob>(JobHandle.CombineDependencies(this.Dependency, this.m_WriteDependencies, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CreateChirpSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
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
    public LifePathEventSystem()
    {
    }

    [BurstCompile]
    private struct CreateLifePathEventJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<LifePathEventData> m_LifePathEventDatas;
      [ReadOnly]
      public BufferLookup<LifePathEntry> m_LifePathEntries;
      public EntityCommandBuffer m_CommandBuffer;
      [ReadOnly]
      public EntityArchetype m_EventArchetype;
      public NativeQueue<LifePathEventCreationData> m_Queue;
      public NativeQueue<ChirpCreationData> m_ChirpQueue;
      [ReadOnly]
      public TimeData m_TimeData;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public bool m_DebugLifePathChirps;

      public void Execute()
      {
        LifePathEventCreationData eventCreationData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue.TryDequeue(out eventCreationData))
        {
          // ISSUE: reference to a compiler-generated field
          LifePathEventData lifePathEventData = this.m_LifePathEventDatas[eventCreationData.m_EventPrefab];
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_LifePathEntries.HasBuffer(eventCreationData.m_Sender);
          // ISSUE: reference to a compiler-generated field
          if (this.m_DebugLifePathChirps | flag)
          {
            if (lifePathEventData.m_IsChirp)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ChirpQueue.Enqueue(new ChirpCreationData()
              {
                m_TriggerPrefab = eventCreationData.m_EventPrefab,
                m_Sender = eventCreationData.m_Sender,
                m_Target = eventCreationData.m_Target
              });
            }
            else if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(this.m_EventArchetype);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_CommandBuffer.SetComponent<LifePathEvent>(entity, new LifePathEvent()
              {
                m_EventPrefab = eventCreationData.m_EventPrefab,
                m_Target = eventCreationData.m_Target,
                m_Date = (uint) TimeSystem.GetDay(this.m_SimulationFrame, this.m_TimeData)
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AppendToBuffer<LifePathEntry>(eventCreationData.m_Sender, new LifePathEntry(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(eventCreationData.m_Sender);
          }
        }
      }
    }

    [BurstCompile]
    private struct CleanupLifePathEntriesJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<LifePathEntry> m_EntryType;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LifePathEntry> bufferAccessor = chunk.GetBufferAccessor<LifePathEntry>(ref this.m_EntryType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          int index2 = 0;
          while (true)
          {
            int num = index2;
            DynamicBuffer<LifePathEntry> dynamicBuffer = bufferAccessor[index1];
            int length = dynamicBuffer.Length;
            if (num < length)
            {
              // ISSUE: reference to a compiler-generated field
              ref EntityCommandBuffer.ParallelWriter local = ref this.m_CommandBuffer;
              int sortKey = unfilteredChunkIndex;
              dynamicBuffer = bufferAccessor[index1];
              Entity entity = dynamicBuffer[index2].m_Entity;
              local.AddComponent<Deleted>(sortKey, entity);
              ++index2;
            }
            else
              break;
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
      public BufferTypeHandle<LifePathEntry> __Game_Triggers_LifePathEntry_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<LifePathEventData> __Game_Prefabs_LifePathEventData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LifePathEntry> __Game_Triggers_LifePathEntry_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_LifePathEntry_RO_BufferTypeHandle = state.GetBufferTypeHandle<LifePathEntry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LifePathEventData_RO_ComponentLookup = state.GetComponentLookup<LifePathEventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_LifePathEntry_RO_BufferLookup = state.GetBufferLookup<LifePathEntry>(true);
      }
    }
  }
}
