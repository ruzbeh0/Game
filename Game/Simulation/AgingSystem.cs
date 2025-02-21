// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AgingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Collections;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Debug;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class AgingSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 1;
    public static readonly int kMoveFromHomeResource = 1000;
    private EntityQuery m_CitizenGroup;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_HouseholdPrefabQuery;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private NativeQueue<Entity> m_MoveFromHomeQueue;
    public static bool s_DebugAgeAllCitizens = false;
    [DebugWatchValue]
    public NativeValue<int> m_BecomeTeen;
    [DebugWatchValue]
    public NativeValue<int> m_BecomeAdult;
    [DebugWatchValue]
    public NativeValue<int> m_BecomeElder;
    public NativeCounter m_BecomeTeenCounter;
    public NativeCounter m_BecomeAdultCounter;
    public NativeCounter m_BecomeElderCounter;
    private AgingSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (AgingSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Citizen>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<Game.Prefabs.HouseholdData>(), ComponentType.ReadOnly<DynamicHousehold>());
      // ISSUE: reference to a compiler-generated field
      this.m_MoveFromHomeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeTeen = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeAdult = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeElder = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeTeenCounter = new NativeCounter(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeAdultCounter = new NativeCounter(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeElderCounter = new NativeCounter(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_MoveFromHomeQueue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeTeen.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeAdult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeElder.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeTeenCounter.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeAdultCounter.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BecomeElderCounter.Dispose();
    }

    public static int GetTeenAgeLimitInDays() => 21;

    public static int GetAdultAgeLimitInDays() => 36;

    public static int GetElderAgeLimitInDays() => 84;

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, AgingSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AgingSystem.AgingJob jobData1 = new AgingSystem.AgingJob()
      {
        m_BecomeTeenCounter = this.m_BecomeTeenCounter.ToConcurrent(),
        m_BecomeAdultCounter = this.m_BecomeAdultCounter.ToConcurrent(),
        m_BecomeElderCounter = this.m_BecomeElderCounter.ToConcurrent(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_StudentType = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_Students = this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup,
        m_Purposes = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_MoveFromHomeQueue = this.m_MoveFromHomeQueue.AsParallelWriter(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_UpdateFrameIndex = updateFrame,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_DebugAgeAllCitizens = AgingSystem.s_DebugAgeAllCitizens
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<AgingSystem.AgingJob>(this.m_CitizenGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      AgingSystem.MoveFromHomeJob jobData2 = new AgingSystem.MoveFromHomeJob()
      {
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RW_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup,
        m_ArchetypeDatas = this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup,
        m_HouseholdPrefabs = this.m_HouseholdPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_MoveFromHomeQueue = this.m_MoveFromHomeQueue,
        m_BecomeTeen = this.m_BecomeTeen,
        m_BecomeAdult = this.m_BecomeAdult,
        m_BecomeElder = this.m_BecomeElder,
        m_BecomeTeenCounter = this.m_BecomeTeenCounter,
        m_BecomeAdultCounter = this.m_BecomeAdultCounter,
        m_BecomeElderCounter = this.m_BecomeElderCounter
      };
      this.Dependency = jobData2.Schedule<AgingSystem.MoveFromHomeJob>(JobHandle.CombineDependencies(outJobHandle, this.Dependency));
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
    public AgingSystem()
    {
    }

    [BurstCompile]
    private struct MoveFromHomeJob : IJob
    {
      public NativeQueue<Entity> m_MoveFromHomeQueue;
      public EntityCommandBuffer m_CommandBuffer;
      public ComponentLookup<Household> m_Households;
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> m_ArchetypeDatas;
      [ReadOnly]
      public NativeList<Entity> m_HouseholdPrefabs;
      public RandomSeed m_RandomSeed;
      public NativeCounter m_BecomeTeenCounter;
      public NativeCounter m_BecomeAdultCounter;
      public NativeCounter m_BecomeElderCounter;
      public NativeValue<int> m_BecomeTeen;
      public NativeValue<int> m_BecomeAdult;
      public NativeValue<int> m_BecomeElder;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_BecomeTeen.value = this.m_BecomeTeenCounter.Count;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_BecomeAdult.value = this.m_BecomeAdultCounter.Count;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_BecomeElder.value = this.m_BecomeElderCounter.Count;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(62347);
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_MoveFromHomeQueue.TryDequeue(out entity1))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Entity householdPrefab = this.m_HouseholdPrefabs[random.NextInt(this.m_HouseholdPrefabs.Length)];
          // ISSUE: reference to a compiler-generated field
          Game.Prefabs.ArchetypeData archetypeData = this.m_ArchetypeDatas[householdPrefab];
          // ISSUE: reference to a compiler-generated field
          HouseholdMember householdMember = this.m_HouseholdMembers[entity1];
          Entity household1 = householdMember.m_Household;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdCitizens.HasBuffer(household1))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household1];
            if (householdCitizen.Length > 1)
            {
              // ISSUE: reference to a compiler-generated field
              Household household2 = this.m_Households[household1];
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(archetypeData.m_Archetype);
              // ISSUE: reference to a compiler-generated field
              int num = math.min(AgingSystem.kMoveFromHomeResource, household2.m_Resources / 2);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Household>(entity2, new Household()
              {
                m_Flags = household2.m_Flags,
                m_Resources = num
              });
              household2.m_Resources -= num;
              // ISSUE: reference to a compiler-generated field
              this.m_Households[household1] = household2;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PropertySeeker>(entity2, new PropertySeeker());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef()
              {
                m_Prefab = householdPrefab
              });
              householdMember.m_Household = entity2;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HouseholdMember>(entity1, householdMember);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetBuffer<HouseholdCitizen>(entity2).Add(new HouseholdCitizen()
              {
                m_Citizen = entity1
              });
              for (int index = 0; index < householdCitizen.Length; ++index)
              {
                if (householdCitizen[index].m_Citizen == entity1)
                {
                  householdCitizen.RemoveAt(index);
                  break;
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct AgingJob : IJobChunk
    {
      public NativeCounter.Concurrent m_BecomeTeenCounter;
      public NativeCounter.Concurrent m_BecomeAdultCounter;
      public NativeCounter.Concurrent m_BecomeElderCounter;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> m_StudentType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> m_Students;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_Purposes;
      public TimeData m_TimeData;
      public NativeQueue<Entity>.ParallelWriter m_MoveFromHomeQueue;
      public uint m_SimulationFrame;
      public uint m_UpdateFrameIndex;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public bool m_DebugAgeAllCitizens;

      private void LeaveSchool(
        int chunkIndex,
        int i,
        Entity student,
        NativeArray<Game.Citizens.Student> students)
      {
        Entity school = students[i].m_School;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Game.Citizens.Student>(chunkIndex, student);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Students.HasBuffer(school))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<StudentsRemoved>(chunkIndex, school);
      }

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DebugAgeAllCitizens && (int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Citizens.Student> nativeArray3 = chunk.GetNativeArray<Game.Citizens.Student>(ref this.m_StudentType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        int day = TimeSystem.GetDay(this.m_SimulationFrame, this.m_TimeData);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Citizen citizen = nativeArray2[index];
          CitizenAge age = citizen.GetAge();
          int num1 = day - (int) citizen.m_BirthDay;
          int num2;
          switch (age)
          {
            case CitizenAge.Child:
              // ISSUE: reference to a compiler-generated method
              num2 = AgingSystem.GetTeenAgeLimitInDays();
              break;
            case CitizenAge.Teen:
              // ISSUE: reference to a compiler-generated method
              num2 = AgingSystem.GetAdultAgeLimitInDays();
              break;
            case CitizenAge.Adult:
              // ISSUE: reference to a compiler-generated method
              num2 = AgingSystem.GetElderAgeLimitInDays();
              break;
            default:
              continue;
          }
          if (num1 >= num2)
          {
            Entity entity = nativeArray1[index];
            switch (age)
            {
              case CitizenAge.Child:
                // ISSUE: reference to a compiler-generated field
                if (chunk.Has<Game.Citizens.Student>(ref this.m_StudentType))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.LeaveSchool(unfilteredChunkIndex, index, entity, nativeArray3);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_BecomeTeenCounter.Increment();
                citizen.SetAge(CitizenAge.Teen);
                nativeArray2[index] = citizen;
                continue;
              case CitizenAge.Teen:
                // ISSUE: reference to a compiler-generated field
                if (chunk.Has<Game.Citizens.Student>(ref this.m_StudentType))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.LeaveSchool(unfilteredChunkIndex, index, entity, nativeArray3);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_MoveFromHomeQueue.Enqueue(entity);
                // ISSUE: reference to a compiler-generated field
                this.m_BecomeAdultCounter.Increment();
                citizen.SetAge(CitizenAge.Adult);
                nativeArray2[index] = citizen;
                continue;
              case CitizenAge.Adult:
                // ISSUE: reference to a compiler-generated field
                this.m_BecomeElderCounter.Increment();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Purposes.HasComponent(entity) && (this.m_Purposes[entity].m_Purpose == Game.Citizens.Purpose.GoingToWork || this.m_Purposes[entity].m_Purpose == Game.Citizens.Purpose.Working))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Worker>(unfilteredChunkIndex, entity);
                citizen.SetAge(CitizenAge.Elderly);
                nativeArray2[index] = citizen;
                continue;
              default:
                continue;
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
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> __Game_Buildings_Student_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      public ComponentLookup<Household> __Game_Citizens_Household_RW_ComponentLookup;
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RW_ComponentLookup;
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> __Game_Prefabs_ArchetypeData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferLookup = state.GetBufferLookup<Game.Buildings.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentLookup = state.GetComponentLookup<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RW_ComponentLookup = state.GetComponentLookup<HouseholdMember>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RW_BufferLookup = state.GetBufferLookup<HouseholdCitizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ArchetypeData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.ArchetypeData>(true);
      }
    }
  }
}
