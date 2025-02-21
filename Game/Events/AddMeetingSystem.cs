// Decompiled with JetBrains decompiler
// Type: Game.Events.AddMeetingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class AddMeetingSystem : GameSystemBase
  {
    private ModificationBarrier1 m_ModificationBarrier;
    private NativeQueue<AddMeetingSystem.AddMeeting> m_MeetingQueue;
    private EntityQuery m_LeisureSettingsQuery;
    private EntityArchetype m_JournalDataArchetype;
    private TriggerSystem m_TriggerSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private JobHandle m_Deps;
    private AddMeetingSystem.TypeHandle __TypeHandle;

    public NativeQueue<AddMeetingSystem.AddMeeting> GetMeetingQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_Deps;
      // ISSUE: reference to a compiler-generated field
      return this.m_MeetingQueue;
    }

    public void AddWriter(JobHandle reader)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Deps = JobHandle.CombineDependencies(this.m_Deps, reader);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<LeisureParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MeetingQueue = new NativeQueue<AddMeetingSystem.AddMeeting>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LeisureSettingsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MeetingQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AttendingEvent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      AddMeetingSystem.TravelJob jobData = new AddMeetingSystem.TravelJob()
      {
        m_MeetingQueue = this.m_MeetingQueue,
        m_AttendingEvents = this.__TypeHandle.__Game_Events_AttendingEvent_RO_ComponentLookup,
        m_EventDatas = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentLookup,
        m_HaveCoordinatedMeetings = this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_LeisureParameters = this.m_LeisureSettingsQuery.GetSingleton<LeisureParametersData>(),
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_Targets = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<AddMeetingSystem.TravelJob>(JobHandle.CombineDependencies(this.m_Deps, this.Dependency));
      // ISSUE: reference to a compiler-generated method
      this.AddWriter(this.Dependency);
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
    public AddMeetingSystem()
    {
    }

    public struct AddMeeting
    {
      public Entity m_Household;
      public LeisureType m_Type;
    }

    [BurstCompile]
    private struct TravelJob : IJob
    {
      public LeisureParametersData m_LeisureParameters;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> m_HaveCoordinatedMeetings;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<AttendingEvent> m_AttendingEvents;
      [ReadOnly]
      public ComponentLookup<EventData> m_EventDatas;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<Target> m_Targets;
      public NativeQueue<AddMeetingSystem.AddMeeting> m_MeetingQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: variable of a compiler-generated type
        AddMeetingSystem.AddMeeting addMeeting;
        // ISSUE: reference to a compiler-generated field
        while (this.m_MeetingQueue.TryDequeue(out addMeeting))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_LeisureParameters.GetPrefab(addMeeting.m_Type);
          // ISSUE: reference to a compiler-generated field
          if (this.m_HaveCoordinatedMeetings.HasBuffer(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            Entity household = addMeeting.m_Household;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_AttendingEvents.HasComponent(household) && !nativeParallelHashSet.Contains(household))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<AttendingEvent>(household);
              nativeParallelHashSet.Add(household);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(this.m_EventDatas[prefab].m_Archetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef()
              {
                m_Prefab = prefab
              });
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TargetElement> dynamicBuffer = this.m_CommandBuffer.SetBuffer<TargetElement>(entity);
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household];
              for (int index = 0; index < householdCitizen.Length; ++index)
              {
                Entity citizen = householdCitizen[index].m_Citizen;
                dynamicBuffer.Add(new TargetElement()
                {
                  m_Entity = citizen
                });
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TouristHouseholds.HasComponent(household) && this.m_TouristHouseholds[household].m_Hotel == Entity.Null && this.m_Targets.HasComponent(household) && this.m_Targets[household].m_Target != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<CoordinatedMeeting>(entity, new CoordinatedMeeting()
                {
                  m_Target = this.m_Targets[household].m_Target
                });
              }
            }
          }
        }
        nativeParallelHashSet.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<AttendingEvent> __Game_Events_AttendingEvent_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EventData> __Game_Prefabs_EventData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> __Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AttendingEvent_RO_ComponentLookup = state.GetComponentLookup<AttendingEvent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentLookup = state.GetComponentLookup<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup = state.GetBufferLookup<HaveCoordinatedMeetingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
      }
    }
  }
}
