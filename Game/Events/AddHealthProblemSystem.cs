// Decompiled with JetBrains decompiler
// Type: Game.Events.AddHealthProblemSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class AddHealthProblemSystem : GameSystemBase
  {
    private IconCommandSystem m_IconCommandSystem;
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_AddHealthProblemQuery;
    private EntityQuery m_HealthcareSettingsQuery;
    private EntityQuery m_CitizenQuery;
    private EntityArchetype m_JournalDataArchetype;
    private TriggerSystem m_TriggerSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private AddHealthProblemSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AddHealthProblemQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Common.Event>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<AddHealthProblem>(),
          ComponentType.ReadOnly<Ignite>(),
          ComponentType.ReadOnly<Destroy>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AddHealthProblemQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HealthcareSettingsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_AddHealthProblemQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<AddHealthProblem> nativeQueue = new NativeQueue<AddHealthProblem>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<AddHealthProblem>.ParallelWriter parallelWriter1 = nativeQueue.AsParallelWriter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Ignite_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Ignite> componentTypeHandle1 = this.__TypeHandle.__Game_Events_Ignite_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Destroy_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Destroy> componentTypeHandle2 = this.__TypeHandle.__Game_Objects_Destroy_RO_ComponentTypeHandle;
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<Ignite> nativeArray1 = archetypeChunk.GetNativeArray<Ignite>(ref componentTypeHandle1);
        NativeArray<Destroy> nativeArray2 = archetypeChunk.GetNativeArray<Destroy>(ref componentTypeHandle2);
        // ISSUE: variable of a compiler-generated type
        AddHealthProblemSystem.FindCitizensInBuildingJob citizensInBuildingJob;
        NativeQueue<TriggerAction> actionBuffer;
        NativeQueue<StatisticsEvent> statisticsEventQueue;
        for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
        {
          Ignite ignite = nativeArray1[index2];
          if (this.EntityManager.HasComponent<Building>(ignite.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: object of a compiler-generated type is created
            citizensInBuildingJob = new AddHealthProblemSystem.FindCitizensInBuildingJob();
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_Event = ignite.m_Event;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_Building = ignite.m_Target;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_Flags = HealthProblemFlags.InDanger;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_DeathProbability = 0.0f;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_RandomSeed = RandomSeed.Next();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
            ref AddHealthProblemSystem.FindCitizensInBuildingJob local1 = ref citizensInBuildingJob;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            actionBuffer = this.m_TriggerSystem.CreateActionBuffer();
            NativeQueue<TriggerAction>.ParallelWriter parallelWriter2 = actionBuffer.AsParallelWriter();
            // ISSUE: reference to a compiler-generated field
            local1.m_TriggerBuffer = parallelWriter2;
            ref AddHealthProblemSystem.FindCitizensInBuildingJob local2 = ref citizensInBuildingJob;
            JobHandle deps;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            statisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps);
            NativeQueue<StatisticsEvent>.ParallelWriter parallelWriter3 = statisticsEventQueue.AsParallelWriter();
            // ISSUE: reference to a compiler-generated field
            local2.m_StatisticsEventQueue = parallelWriter3;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_AddQueue = parallelWriter1;
            // ISSUE: variable of a compiler-generated type
            AddHealthProblemSystem.FindCitizensInBuildingJob jobData = citizensInBuildingJob;
            // ISSUE: reference to a compiler-generated field
            this.Dependency = jobData.ScheduleParallel<AddHealthProblemSystem.FindCitizensInBuildingJob>(this.m_CitizenQuery, JobHandle.CombineDependencies(this.Dependency, deps));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CityStatisticsSystem.AddWriter(this.Dependency);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
          }
        }
        for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
        {
          Destroy destroy = nativeArray2[index3];
          if (this.EntityManager.HasComponent<Building>(destroy.m_Object))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: object of a compiler-generated type is created
            citizensInBuildingJob = new AddHealthProblemSystem.FindCitizensInBuildingJob();
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_Event = destroy.m_Event;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_Building = destroy.m_Object;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_Flags = HealthProblemFlags.Trapped;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_DeathProbability = this.m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>().m_BuildingDestoryDeathRate;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_RandomSeed = RandomSeed.Next();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
            ref AddHealthProblemSystem.FindCitizensInBuildingJob local3 = ref citizensInBuildingJob;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            actionBuffer = this.m_TriggerSystem.CreateActionBuffer();
            NativeQueue<TriggerAction>.ParallelWriter parallelWriter4 = actionBuffer.AsParallelWriter();
            // ISSUE: reference to a compiler-generated field
            local3.m_TriggerBuffer = parallelWriter4;
            ref AddHealthProblemSystem.FindCitizensInBuildingJob local4 = ref citizensInBuildingJob;
            JobHandle deps;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            statisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps);
            NativeQueue<StatisticsEvent>.ParallelWriter parallelWriter5 = statisticsEventQueue.AsParallelWriter();
            // ISSUE: reference to a compiler-generated field
            local4.m_StatisticsEventQueue = parallelWriter5;
            // ISSUE: reference to a compiler-generated field
            citizensInBuildingJob.m_AddQueue = parallelWriter1;
            // ISSUE: variable of a compiler-generated type
            AddHealthProblemSystem.FindCitizensInBuildingJob jobData = citizensInBuildingJob;
            // ISSUE: reference to a compiler-generated field
            this.Dependency = jobData.ScheduleParallel<AddHealthProblemSystem.FindCitizensInBuildingJob>(this.m_CitizenQuery, JobHandle.CombineDependencies(this.Dependency, deps));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_CityStatisticsSystem.AddWriter(this.Dependency);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AddHealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AddHealthProblemSystem.AddHealthProblemJob jobData1 = new AddHealthProblemSystem.AddHealthProblemJob()
      {
        m_Chunks = archetypeChunkArray,
        m_AddHealthProblemType = this.__TypeHandle.__Game_Events_AddHealthProblem_RO_ComponentTypeHandle,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_HealthcareParameterData = this.m_HealthcareSettingsQuery.GetSingleton<HealthcareParameterData>(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_HealthProblemData = this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentLookup,
        m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_JournalDataArchetype = this.m_JournalDataArchetype,
        m_AddQueue = nativeQueue,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      };
      this.Dependency = jobData1.Schedule<AddHealthProblemSystem.AddHealthProblemJob>(this.Dependency);
      nativeQueue.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
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
    public AddHealthProblemSystem()
    {
    }

    [BurstCompile]
    private struct FindCitizensInBuildingJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_Event;
      [ReadOnly]
      public Entity m_Building;
      [ReadOnly]
      public HealthProblemFlags m_Flags;
      [ReadOnly]
      public float m_DeathProbability;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      public NativeQueue<AddHealthProblem>.ParallelWriter m_AddQueue;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray2 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray3 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (nativeArray2[index].m_CurrentBuilding == this.m_Building)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            AddHealthProblem addHealthProblem = new AddHealthProblem()
            {
              m_Event = this.m_Event,
              m_Target = nativeArray1[index],
              m_Flags = this.m_Flags
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_DeathProbability > 0.0 && (double) random.NextFloat(1f) < (double) this.m_DeathProbability)
            {
              addHealthProblem.m_Flags |= HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport;
              Entity household = nativeArray3.Length != 0 ? nativeArray3[index].m_Household : Entity.Null;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              DeathCheckSystem.PerformAfterDeathActions(nativeArray1[index], household, this.m_TriggerBuffer, this.m_StatisticsEventQueue, ref this.m_HouseholdCitizens);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_AddQueue.Enqueue(addHealthProblem);
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
    private struct AddHealthProblemJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<AddHealthProblem> m_AddHealthProblemType;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public HealthcareParameterData m_HealthcareParameterData;
      public IconCommandBuffer m_IconCommandBuffer;
      public ComponentLookup<HealthProblem> m_HealthProblemData;
      public ComponentLookup<PathOwner> m_PathOwnerData;
      public ComponentLookup<Target> m_TargetData;
      public BufferLookup<TargetElement> m_TargetElements;
      public EntityArchetype m_JournalDataArchetype;
      public NativeQueue<AddHealthProblem> m_AddQueue;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<TriggerAction> m_TriggerBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_AddQueue.Count;
        int capacity = count;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          capacity += this.m_Chunks[index].Count;
        }
        NativeParallelHashMap<Entity, HealthProblem> nativeParallelHashMap = new NativeParallelHashMap<Entity, HealthProblem>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<AddHealthProblem> nativeArray = this.m_Chunks[index1].GetNativeArray<AddHealthProblem>(ref this.m_AddHealthProblemType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            AddHealthProblem addHealthProblem = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(addHealthProblem.m_Target))
            {
              HealthProblem problem2 = new HealthProblem(addHealthProblem.m_Event, addHealthProblem.m_Flags);
              HealthProblem problem1;
              if (nativeParallelHashMap.TryGetValue(addHealthProblem.m_Target, out problem1))
              {
                // ISSUE: reference to a compiler-generated method
                nativeParallelHashMap[addHealthProblem.m_Target] = this.MergeProblems(problem1, problem2);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_HealthProblemData.HasComponent(addHealthProblem.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  problem1 = this.m_HealthProblemData[addHealthProblem.m_Target];
                  // ISSUE: reference to a compiler-generated method
                  nativeParallelHashMap.TryAdd(addHealthProblem.m_Target, this.MergeProblems(problem1, problem2));
                }
                else
                  nativeParallelHashMap.TryAdd(addHealthProblem.m_Target, problem2);
              }
            }
          }
        }
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          AddHealthProblem addHealthProblem = this.m_AddQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(addHealthProblem.m_Target))
          {
            HealthProblem problem2 = new HealthProblem(addHealthProblem.m_Event, addHealthProblem.m_Flags);
            HealthProblem problem1;
            if (nativeParallelHashMap.TryGetValue(addHealthProblem.m_Target, out problem1))
            {
              // ISSUE: reference to a compiler-generated method
              nativeParallelHashMap[addHealthProblem.m_Target] = this.MergeProblems(problem1, problem2);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_HealthProblemData.HasComponent(addHealthProblem.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                problem1 = this.m_HealthProblemData[addHealthProblem.m_Target];
                // ISSUE: reference to a compiler-generated method
                nativeParallelHashMap.TryAdd(addHealthProblem.m_Target, this.MergeProblems(problem1, problem2));
              }
              else
                nativeParallelHashMap.TryAdd(addHealthProblem.m_Target, problem2);
            }
          }
        }
        if (nativeParallelHashMap.Count() != 0)
        {
          NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < keyArray.Length; ++index)
          {
            Entity entity = keyArray[index];
            HealthProblem healthProblem = nativeParallelHashMap[entity];
            // ISSUE: reference to a compiler-generated field
            if (this.m_HealthProblemData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              HealthProblem oldProblem = this.m_HealthProblemData[entity];
              // ISSUE: reference to a compiler-generated field
              if (oldProblem.m_Event != healthProblem.m_Event && this.m_TargetElements.HasBuffer(healthProblem.m_Event))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[healthProblem.m_Event], new TargetElement(entity));
              }
              if ((oldProblem.m_Flags & (HealthProblemFlags.Dead | HealthProblemFlags.RequireTransport)) == HealthProblemFlags.RequireTransport && (healthProblem.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(entity, this.m_HealthcareParameterData.m_AmbulanceNotificationPrefab);
                healthProblem.m_Timer = (byte) 0;
              }
              if ((healthProblem.m_Flags & (HealthProblemFlags.Dead | HealthProblemFlags.Injured)) != HealthProblemFlags.None && (healthProblem.m_Flags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None && ((oldProblem.m_Flags & (HealthProblemFlags.Dead | HealthProblemFlags.Injured)) == HealthProblemFlags.None || (oldProblem.m_Flags & HealthProblemFlags.RequireTransport) == HealthProblemFlags.None))
              {
                // ISSUE: reference to a compiler-generated method
                this.StopMoving(entity);
              }
              // ISSUE: reference to a compiler-generated method
              this.AddJournalData(oldProblem, healthProblem);
              // ISSUE: reference to a compiler-generated field
              this.m_HealthProblemData[entity] = healthProblem;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TargetElements.HasBuffer(healthProblem.m_Event))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[healthProblem.m_Event], new TargetElement(entity));
              }
              if ((healthProblem.m_Flags & (HealthProblemFlags.Dead | HealthProblemFlags.Injured)) != HealthProblemFlags.None && (healthProblem.m_Flags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None)
              {
                // ISSUE: reference to a compiler-generated method
                this.StopMoving(entity);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<HealthProblem>(entity, healthProblem);
              // ISSUE: reference to a compiler-generated method
              this.AddJournalData(healthProblem);
            }
            Entity prefab = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(healthProblem.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              prefab = this.m_PrefabRefData[healthProblem.m_Event].m_Prefab;
            }
            if ((healthProblem.m_Flags & HealthProblemFlags.Sick) != HealthProblemFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenGotSick, prefab, entity, healthProblem.m_Event));
            }
            else if ((healthProblem.m_Flags & HealthProblemFlags.Injured) != HealthProblemFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenGotInjured, prefab, entity, healthProblem.m_Event));
            }
            else if ((healthProblem.m_Flags & HealthProblemFlags.Trapped) != HealthProblemFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenGotTrapped, prefab, entity, healthProblem.m_Event));
            }
            else if ((healthProblem.m_Flags & HealthProblemFlags.InDanger) != HealthProblemFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenGotInDanger, prefab, entity, healthProblem.m_Event));
            }
          }
        }
        nativeParallelHashMap.Dispose();
      }

      private void StopMoving(Entity citizen)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurrentTransportData.HasComponent(citizen))
          return;
        // ISSUE: reference to a compiler-generated field
        CurrentTransport currentTransport = this.m_CurrentTransportData[citizen];
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathOwnerData.HasComponent(currentTransport.m_CurrentTransport))
        {
          // ISSUE: reference to a compiler-generated field
          PathOwner pathOwner = this.m_PathOwnerData[currentTransport.m_CurrentTransport];
          pathOwner.m_State &= ~PathFlags.Failed;
          pathOwner.m_State |= PathFlags.Obsolete;
          // ISSUE: reference to a compiler-generated field
          this.m_PathOwnerData[currentTransport.m_CurrentTransport] = pathOwner;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TargetData.HasComponent(currentTransport.m_CurrentTransport))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TargetData[currentTransport.m_CurrentTransport] = new Target();
      }

      private void AddJournalData(HealthProblem problem)
      {
        if ((problem.m_Flags & (HealthProblemFlags.Sick | HealthProblemFlags.Dead | HealthProblemFlags.Injured)) == HealthProblemFlags.None)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddEventJournalData>(this.m_CommandBuffer.CreateEntity(this.m_JournalDataArchetype), new AddEventJournalData(problem.m_Event, EventDataTrackingType.Casualties));
      }

      private void AddJournalData(HealthProblem oldProblem, HealthProblem newProblem)
      {
        if (oldProblem.m_Event != newProblem.m_Event)
        {
          // ISSUE: reference to a compiler-generated method
          this.AddJournalData(newProblem);
        }
        else
        {
          if ((oldProblem.m_Flags & (HealthProblemFlags.Sick | HealthProblemFlags.Dead | HealthProblemFlags.Injured)) != HealthProblemFlags.None)
            return;
          // ISSUE: reference to a compiler-generated method
          this.AddJournalData(newProblem);
        }
      }

      private HealthProblem MergeProblems(HealthProblem problem1, HealthProblem problem2)
      {
        HealthProblemFlags healthProblemFlags = problem1.m_Flags ^ problem2.m_Flags;
        if ((healthProblemFlags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
          return (problem1.m_Flags & HealthProblemFlags.Dead) == HealthProblemFlags.None ? problem2 : problem1;
        HealthProblem healthProblem;
        if ((healthProblemFlags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None)
        {
          healthProblem = (problem1.m_Flags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None ? problem1 : problem2;
          healthProblem.m_Flags |= (problem1.m_Flags & HealthProblemFlags.RequireTransport) != HealthProblemFlags.None ? problem2.m_Flags : problem1.m_Flags;
        }
        else if (problem1.m_Event != Entity.Null != (problem2.m_Event != Entity.Null))
        {
          healthProblem = problem1.m_Event != Entity.Null ? problem1 : problem2;
          healthProblem.m_Flags |= problem1.m_Event != Entity.Null ? problem2.m_Flags : problem1.m_Flags;
        }
        else
        {
          healthProblem = problem1;
          healthProblem.m_Flags |= problem2.m_Flags;
        }
        return healthProblem;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Ignite> __Game_Events_Ignite_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroy> __Game_Objects_Destroy_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<AddHealthProblem> __Game_Events_AddHealthProblem_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RW_ComponentLookup;
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentLookup;
      public ComponentLookup<Target> __Game_Common_Target_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Ignite_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Ignite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Destroy_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroy>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AddHealthProblem_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AddHealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RW_ComponentLookup = state.GetComponentLookup<HealthProblem>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentLookup = state.GetComponentLookup<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentLookup = state.GetComponentLookup<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
      }
    }
  }
}
