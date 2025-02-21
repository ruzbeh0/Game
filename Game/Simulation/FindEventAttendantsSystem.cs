// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FindEventAttendantsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Common;
using Game.Events;
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
  public class FindEventAttendantsSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 16;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_EventQuery;
    private EntityQuery m_HouseholdQuery;
    private NativeQueue<FindEventAttendantsSystem.Attend> m_AttendQueue;
    private EntityArchetype m_MeetingArchetype;
    private EntityArchetype m_JournalDataArchetype;
    private FindEventAttendantsSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Events.CalendarEvent>(), ComponentType.ReadWrite<FindingEventParticipants>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(ComponentType.ReadWrite<Household>(), ComponentType.ReadWrite<UpdateFrame>(), ComponentType.ReadWrite<HouseholdCitizen>(), ComponentType.Exclude<AttendingEvent>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AttendQueue = new NativeQueue<FindEventAttendantsSystem.Attend>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MeetingArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<CoordinatedMeeting>(), ComponentType.ReadWrite<CoordinatedMeetingAttendee>(), ComponentType.ReadWrite<PrefabRef>(), ComponentType.ReadWrite<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_AttendQueue.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      FindEventAttendantsSystem.ConsiderAttendanceJob jobData1 = new FindEventAttendantsSystem.ConsiderAttendanceJob()
      {
        m_EventEntities = this.m_EventQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_HouseholdCitizenType = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle,
        m_CommuterHouseholdType = this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle,
        m_UpdateFrameIndex = frameWithInterval,
        m_CitizenDatas = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Events = this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_AttendQueue = this.m_AttendQueue.AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<FindEventAttendantsSystem.ConsiderAttendanceJob>(this.m_HouseholdQuery, JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
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
      FindEventAttendantsSystem.AttendJob jobData2 = new FindEventAttendantsSystem.AttendJob()
      {
        m_EventEntities = this.m_EventQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_AttendQueue = this.m_AttendQueue,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_Durations = this.__TypeHandle.__Game_Events_Duration_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_HaveCoordinatedMeetings = this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup,
        m_CalendarEventDatas = this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentLookup,
        m_MeetingArchetype = this.m_MeetingArchetype,
        m_JournalDataArchetype = this.m_JournalDataArchetype,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData2.Schedule<FindEventAttendantsSystem.AttendJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public FindEventAttendantsSystem()
    {
    }

    private struct Attend
    {
      public Entity m_Event;
      public Entity m_Participant;
    }

    [BurstCompile]
    private struct AttendJob : IJob
    {
      public NativeQueue<FindEventAttendantsSystem.Attend> m_AttendQueue;
      public BufferLookup<TargetElement> m_TargetElements;
      [ReadOnly]
      public NativeList<Entity> m_EventEntities;
      [ReadOnly]
      public ComponentLookup<Duration> m_Durations;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<CalendarEventData> m_CalendarEventDatas;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> m_HaveCoordinatedMeetings;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_Citizens;
      public EntityArchetype m_MeetingArchetype;
      public EntityArchetype m_JournalDataArchetype;
      [ReadOnly]
      public uint m_SimulationFrame;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        FindEventAttendantsSystem.Attend attend;
        // ISSUE: reference to a compiler-generated field
        while (this.m_AttendQueue.TryDequeue(out attend))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Prefabs.HasComponent(attend.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_Prefabs[attend.m_Event].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            CalendarEventData calendarEventData = this.m_CalendarEventDatas[prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_HaveCoordinatedMeetings.HasBuffer(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(this.m_MeetingArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef()
              {
                m_Prefab = prefab
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<CoordinatedMeeting>(entity, new CoordinatedMeeting()
              {
                m_Phase = 0,
                m_Status = MeetingStatus.Waiting,
                m_Target = Entity.Null
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef()
              {
                m_Prefab = prefab
              });
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (calendarEventData.m_RandomTargetType == EventTargetType.Couple && this.m_Citizens.HasBuffer(attend.m_Participant))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[attend.m_Event];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<HouseholdCitizen> citizen = this.m_Citizens[attend.m_Participant];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<CoordinatedMeetingAttendee> dynamicBuffer = this.m_CommandBuffer.AddBuffer<CoordinatedMeetingAttendee>(entity);
                for (int index = 0; index < citizen.Length; ++index)
                {
                  targetElement.Add(new TargetElement()
                  {
                    m_Entity = citizen[index].m_Citizen
                  });
                  dynamicBuffer.Add(new CoordinatedMeetingAttendee()
                  {
                    m_Attendee = citizen[index].m_Citizen
                  });
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.AddJournalData(attend.m_Event, citizen.Length);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            UnityEngine.Debug.LogWarning((object) string.Format("Event {0} does not have a prefab", (object) attend.m_Event));
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_EventEntities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity eventEntity = this.m_EventEntities[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationFrame > this.m_Durations[eventEntity].m_StartFrame + 240U)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<FindingEventParticipants>(eventEntity);
          }
        }
      }

      private void AddJournalData(Entity eventEntity, int count)
      {
        if (!(eventEntity != Entity.Null) || count <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddEventJournalData>(this.m_CommandBuffer.CreateEntity(this.m_JournalDataArchetype), new AddEventJournalData(eventEntity, EventDataTrackingType.Attendants, count));
      }
    }

    [BurstCompile]
    private struct ConsiderAttendanceJob : IJobChunk
    {
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public NativeList<Entity> m_EventEntities;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenType;
      [ReadOnly]
      public ComponentTypeHandle<CommuterHousehold> m_CommuterHouseholdType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<CalendarEventData> m_Events;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenDatas;
      public NativeQueue<FindEventAttendantsSystem.Attend>.ParallelWriter m_AttendQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public uint m_UpdateFrameIndex;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        NativeList<Entity> nativeList = new NativeList<Entity>(0, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_EventEntities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity eventEntity = this.m_EventEntities[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Events[this.m_Prefabs[eventEntity].m_Prefab].m_RandomTargetType == EventTargetType.Couple && !chunk.Has<CommuterHousehold>(ref this.m_CommuterHouseholdType))
            nativeList.Add(in eventEntity);
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HouseholdCitizen> bufferAccessor = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Entity e = nativeArray[index1];
          DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor[index1];
          int num1 = 0;
          int num2 = 0;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            switch (this.m_CitizenDatas[dynamicBuffer[index2].m_Citizen].GetAge())
            {
              case CitizenAge.Child:
                ++num2;
                continue;
              case CitizenAge.Teen:
                continue;
              default:
                ++num1;
                continue;
            }
          }
          if (num1 >= 2 && num2 == 0)
          {
            for (int index3 = 0; index3 < nativeList.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              CalendarEventData calendarEventData = this.m_Events[this.m_Prefabs[nativeList[index3]].m_Prefab];
              if ((double) random.NextInt(100) < (double) calendarEventData.m_AffectedProbability.min)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_AttendQueue.Enqueue(new FindEventAttendantsSystem.Attend()
                {
                  m_Event = nativeList[index3],
                  m_Participant = e
                });
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<AttendingEvent>(unfilteredChunkIndex, e, new AttendingEvent()
              {
                m_Event = nativeList[index3]
              });
            }
          }
        }
        nativeList.Dispose();
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommuterHousehold> __Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CalendarEventData> __Game_Prefabs_CalendarEventData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Duration> __Game_Events_Duration_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> __Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle = state.GetBufferTypeHandle<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CommuterHousehold_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommuterHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CalendarEventData_RO_ComponentLookup = state.GetComponentLookup<CalendarEventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentLookup = state.GetComponentLookup<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup = state.GetBufferLookup<HaveCoordinatedMeetingData>(true);
      }
    }
  }
}
