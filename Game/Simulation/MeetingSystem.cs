// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MeetingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
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
  public class MeetingSystem : GameSystemBase
  {
    private EntityQuery m_MeetingGroup;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private IconCommandSystem m_IconCommandSystem;
    private MeetingSystem.TypeHandle __TypeHandle;

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
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MeetingGroup = this.GetEntityQuery(ComponentType.ReadWrite<CoordinatedMeeting>(), ComponentType.ReadWrite<CoordinatedMeetingAttendee>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MeetingGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CoordinatedMeeting_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CoordinatedMeetingAttendee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MeetingSystem.MeetingJob jobData = new MeetingSystem.MeetingJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AttendeeType = this.__TypeHandle.__Game_Citizens_CoordinatedMeetingAttendee_RO_BufferTypeHandle,
        m_MeetingType = this.__TypeHandle.__Game_Citizens_CoordinatedMeeting_RW_ComponentTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_CurrentBuildings = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_MeetingDatas = this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup,
        m_AttendingMeetings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_CalendarEvents = this.__TypeHandle.__Game_Prefabs_CalendarEventData_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<MeetingSystem.MeetingJob>(this.m_MeetingGroup, this.Dependency);
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
    public MeetingSystem()
    {
    }

    [BurstCompile]
    private struct MeetingJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<CoordinatedMeeting> m_MeetingType;
      [ReadOnly]
      public BufferTypeHandle<CoordinatedMeetingAttendee> m_AttendeeType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildings;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> m_MeetingDatas;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_AttendingMeetings;
      [ReadOnly]
      public ComponentLookup<CalendarEventData> m_CalendarEvents;
      public RandomSeed m_RandomSeed;
      public uint m_SimulationFrame;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
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
        NativeArray<CoordinatedMeeting> nativeArray2 = chunk.GetNativeArray<CoordinatedMeeting>(ref this.m_MeetingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CoordinatedMeetingAttendee> bufferAccessor = chunk.GetBufferAccessor<CoordinatedMeetingAttendee>(ref this.m_AttendeeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          CoordinatedMeeting coordinatedMeeting = nativeArray2[index1];
          Entity prefab = nativeArray3[index1].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<HaveCoordinatedMeetingData> meetingData = this.m_MeetingDatas[prefab];
          CalendarEventData calendarEventData = new CalendarEventData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CalendarEvents.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            calendarEventData = this.m_CalendarEvents[prefab];
          }
          bool flag1 = calendarEventData.m_RandomTargetType == EventTargetType.Couple;
          HaveCoordinatedMeetingData coordinatedMeetingData = new HaveCoordinatedMeetingData();
          if (coordinatedMeeting.m_Status != MeetingStatus.Done)
            coordinatedMeetingData = meetingData[coordinatedMeeting.m_Phase];
          DynamicBuffer<CoordinatedMeetingAttendee> dynamicBuffer = bufferAccessor[index1];
          Entity entity1 = coordinatedMeeting.m_Target;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenters.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            entity1 = this.m_PropertyRenters[entity1].m_Property;
          }
          Entity entity2 = new Entity();
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Entity attendee = dynamicBuffer[index2].m_Attendee;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Citizens.HasComponent(attendee) || (this.m_Citizens[attendee].m_State & CitizenFlags.MovingAwayReachOC) != CitizenFlags.None || this.m_HealthProblems.HasComponent(attendee))
            {
              coordinatedMeeting.m_Status = MeetingStatus.Done;
              nativeArray2[index1] = coordinatedMeeting;
              break;
            }
            if (flag1)
            {
              if (index2 == 0)
              {
                // ISSUE: reference to a compiler-generated field
                entity2 = this.m_HouseholdMembers[attendee].m_Household;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (entity2 != this.m_HouseholdMembers[attendee].m_Household)
                {
                  coordinatedMeeting.m_Status = MeetingStatus.Done;
                  nativeArray2[index1] = coordinatedMeeting;
                  break;
                }
              }
            }
          }
          if (coordinatedMeeting.m_Status == MeetingStatus.Waiting)
          {
            if (coordinatedMeeting.m_Target != Entity.Null)
            {
              coordinatedMeeting.m_Status = MeetingStatus.Traveling;
              nativeArray2[index1] = coordinatedMeeting;
            }
          }
          else if (coordinatedMeeting.m_Status == MeetingStatus.Traveling)
          {
            bool flag2 = false;
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              Entity attendee = dynamicBuffer[index3].m_Attendee;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_CurrentBuildings.HasComponent(attendee) || this.m_CurrentBuildings[attendee].m_CurrentBuilding != entity1)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
            {
              if (coordinatedMeetingData.m_Notification != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Add(this.m_CurrentBuildings[dynamicBuffer[0].m_Attendee].m_CurrentBuilding, coordinatedMeetingData.m_Notification, clusterLayer: IconClusterLayer.Transaction);
              }
              coordinatedMeeting.m_Status = MeetingStatus.Attending;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              coordinatedMeeting.m_PhaseEndTime = this.m_SimulationFrame + this.m_RandomSeed.GetRandom((int) this.m_SimulationFrame).NextUInt(coordinatedMeetingData.m_Delay.x, coordinatedMeetingData.m_Delay.y);
              nativeArray2[index1] = coordinatedMeeting;
            }
          }
          else if (coordinatedMeeting.m_Status == MeetingStatus.Attending)
          {
            // ISSUE: reference to a compiler-generated field
            bool flag3 = this.m_SimulationFrame <= coordinatedMeeting.m_PhaseEndTime;
            if (flag3)
            {
              for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
              {
                Entity attendee = dynamicBuffer[index4].m_Attendee;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Citizens.HasComponent(attendee) && (this.m_Citizens[attendee].m_State & CitizenFlags.MovingAwayReachOC) == CitizenFlags.None && !this.m_HealthProblems.HasComponent(attendee) && (!this.m_CurrentBuildings.HasComponent(attendee) || this.m_CurrentBuildings[attendee].m_CurrentBuilding != entity1))
                {
                  flag3 = false;
                  break;
                }
              }
            }
            if (!flag3)
            {
              ++coordinatedMeeting.m_Phase;
              if (coordinatedMeeting.m_Phase >= meetingData.Length)
              {
                coordinatedMeeting.m_Status = MeetingStatus.Done;
              }
              else
              {
                coordinatedMeeting.m_Target = new Entity();
                coordinatedMeeting.m_Status = MeetingStatus.Waiting;
              }
              nativeArray2[index1] = coordinatedMeeting;
            }
          }
          else if (coordinatedMeeting.m_Status == MeetingStatus.Done)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, nativeArray1[index1]);
            for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
            {
              Entity attendee = dynamicBuffer[index5].m_Attendee;
              // ISSUE: reference to a compiler-generated field
              if (this.m_HouseholdMembers.HasComponent(attendee))
              {
                // ISSUE: reference to a compiler-generated field
                Entity household = this.m_HouseholdMembers[attendee].m_Household;
                // ISSUE: reference to a compiler-generated field
                if (this.m_AttendingMeetings.HasComponent(household))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(unfilteredChunkIndex, household);
                }
              }
              if (attendee != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(unfilteredChunkIndex, attendee);
              }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<CoordinatedMeetingAttendee> __Game_Citizens_CoordinatedMeetingAttendee_RO_BufferTypeHandle;
      public ComponentTypeHandle<CoordinatedMeeting> __Game_Citizens_CoordinatedMeeting_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> __Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> __Game_Citizens_AttendingMeeting_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CalendarEventData> __Game_Prefabs_CalendarEventData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CoordinatedMeetingAttendee_RO_BufferTypeHandle = state.GetBufferTypeHandle<CoordinatedMeetingAttendee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CoordinatedMeeting_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CoordinatedMeeting>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup = state.GetBufferLookup<HaveCoordinatedMeetingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_AttendingMeeting_RO_ComponentLookup = state.GetComponentLookup<AttendingMeeting>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CalendarEventData_RO_ComponentLookup = state.GetComponentLookup<CalendarEventData>(true);
      }
    }
  }
}
