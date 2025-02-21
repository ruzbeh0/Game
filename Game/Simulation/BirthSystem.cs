// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BirthSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Collections;
using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Debug;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
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
  public class BirthSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 16;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private TriggerSystem m_TriggerSystem;
    [DebugWatchValue]
    private NativeValue<int> m_DebugBirth;
    private NativeCounter m_DebugBirthCounter;
    private EntityQuery m_CitizenQuery;
    private EntityQuery m_CitizenPrefabQuery;
    private EntityQuery m_CitizenParametersQuery;
    public int m_BirthChance = 20;
    private BirthSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (BirthSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugBirthCounter = new NativeCounter(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DebugBirth = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<HouseholdMember>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenData>(), ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenParametersQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenPrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenParametersQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugBirthCounter.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugBirth.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, BirthSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
      JobHandle deps;
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
      BirthSystem.CheckBirthJob jobData1 = new BirthSystem.CheckBirthJob()
      {
        m_DebugBirthCounter = this.m_DebugBirthCounter.ToConcurrent(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_MemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Students = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CitizenPrefabArchetypes = this.m_CitizenPrefabQuery.ToComponentDataListAsync<Game.Prefabs.ArchetypeData>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_CitizenPrefabs = this.m_CitizenPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_CitizenParametersData = this.m_CitizenParametersQuery.GetSingleton<CitizenParametersData>(),
        m_RandomSeed = RandomSeed.Next(),
        m_UpdateFrameIndex = updateFrame,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<BirthSystem.CheckBirthJob>(this.m_CitizenQuery, JobUtils.CombineDependencies(this.Dependency, deps, outJobHandle2, outJobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BirthSystem.SumBirthJob jobData2 = new BirthSystem.SumBirthJob()
      {
        m_DebugBirth = this.m_DebugBirth,
        m_DebugBirthCount = this.m_DebugBirthCounter
      };
      this.Dependency = jobData2.Schedule<BirthSystem.SumBirthJob>(this.Dependency);
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
    public BirthSystem()
    {
    }

    [BurstCompile]
    private struct CheckBirthJob : IJobChunk
    {
      public NativeCounter.Concurrent m_DebugBirthCounter;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_MemberType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> m_Students;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      public uint m_UpdateFrameIndex;
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public CitizenParametersData m_CitizenParametersData;
      [ReadOnly]
      public NativeList<Entity> m_CitizenPrefabs;
      [ReadOnly]
      public NativeList<Game.Prefabs.ArchetypeData> m_CitizenPrefabArchetypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      private Entity SpawnBaby(int index, Entity household, ref Random random, Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DebugBirthCounter.Increment();
        // ISSUE: reference to a compiler-generated field
        int index1 = random.NextInt(this.m_CitizenPrefabs.Length);
        // ISSUE: reference to a compiler-generated field
        Entity citizenPrefab = this.m_CitizenPrefabs[index1];
        // ISSUE: reference to a compiler-generated field
        Game.Prefabs.ArchetypeData citizenPrefabArchetype = this.m_CitizenPrefabArchetypes[index1];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(index, citizenPrefabArchetype.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(index, entity, new PrefabRef()
        {
          m_Prefab = citizenPrefab
        });
        HouseholdMember component1 = new HouseholdMember()
        {
          m_Household = household
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<HouseholdMember>(index, entity, component1);
        Citizen component2 = new Citizen()
        {
          m_BirthDay = 0,
          m_State = CitizenFlags.None
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Citizen>(index, entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CurrentBuilding>(index, entity, new CurrentBuilding()
        {
          m_CurrentBuilding = building
        });
        return entity;
      }

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
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray3 = chunk.GetNativeArray<HouseholdMember>(ref this.m_MemberType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          Citizen citizen1 = nativeArray2[index1];
          if (citizen1.GetAge() == CitizenAge.Adult && (citizen1.m_State & (CitizenFlags.Male | CitizenFlags.Tourist | CitizenFlags.Commuter)) == CitizenFlags.None)
          {
            Entity household = nativeArray3[index1].m_Household;
            Entity property = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PropertyRenters.HasComponent(household))
            {
              // ISSUE: reference to a compiler-generated field
              property = this.m_PropertyRenters[household].m_Property;
            }
            if (!(property == Entity.Null))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household];
              Entity entity2 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              float baseBirthRate = this.m_CitizenParametersData.m_BaseBirthRate;
              for (int index2 = 0; index2 < householdCitizen.Length; ++index2)
              {
                Entity citizen2 = householdCitizen[index2].m_Citizen;
                // ISSUE: reference to a compiler-generated field
                if (this.m_Citizens.HasComponent(citizen2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Citizen citizen3 = this.m_Citizens[citizen2];
                  if ((citizen3.m_State & CitizenFlags.Male) != CitizenFlags.None && citizen3.GetAge() == CitizenAge.Adult)
                  {
                    // ISSUE: reference to a compiler-generated field
                    baseBirthRate += this.m_CitizenParametersData.m_AdultFemaleBirthRateBonus;
                    break;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_Students.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                baseBirthRate *= this.m_CitizenParametersData.m_StudentBirthRateAdjust;
              }
              // ISSUE: reference to a compiler-generated field
              if ((double) random.NextFloat(1f) < (double) baseBirthRate / (double) BirthSystem.kUpdatesPerDay)
              {
                // ISSUE: reference to a compiler-generated method
                this.SpawnBaby(unfilteredChunkIndex, household, ref random, property);
                // ISSUE: reference to a compiler-generated field
                this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                {
                  m_Statistic = StatisticType.BirthRate,
                  m_Change = 1f
                });
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

    [BurstCompile]
    private struct SumBirthJob : IJob
    {
      public NativeCounter m_DebugBirthCount;
      public NativeValue<int> m_DebugBirth;

      public void Execute() => this.m_DebugBirth.value = this.m_DebugBirthCount.Count;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentLookup = state.GetComponentLookup<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
      }
    }
  }
}
