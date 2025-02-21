// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DivorceSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Collections;
using Game.Agents;
using Game.Citizens;
using Game.Common;
using Game.Debug;
using Game.Economy;
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
  public class DivorceSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 4;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    [DebugWatchValue]
    private NativeValue<int> m_DebugDivorce;
    private NativeCounter m_DebugDivorceCount;
    private EntityQuery m_HouseholdQuery;
    private EntityQuery m_HouseholdPrefabQuery;
    private EntityQuery m_CitizenParametersQuery;
    private TriggerSystem m_TriggerSystem;
    private DivorceSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (DivorceSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugDivorce = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DebugDivorceCount = new NativeCounter(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(ComponentType.ReadOnly<Household>(), ComponentType.ReadOnly<HouseholdCitizen>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<TouristHousehold>(), ComponentType.Exclude<CommuterHousehold>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Prefabs.ArchetypeData>(), ComponentType.ReadOnly<Game.Prefabs.HouseholdData>(), ComponentType.ReadOnly<DynamicHousehold>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenParametersQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdPrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenParametersQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DebugDivorce.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugDivorceCount.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, DivorceSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DivorceSystem.CheckDivorceJob jobData1 = new DivorceSystem.CheckDivorceJob()
      {
        m_DebugDivorceCount = this.m_DebugDivorceCount.ToConcurrent(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdCitizenType = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_HouseholdType = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentTypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_ArchetypeDatas = this.__TypeHandle.__Game_Prefabs_ArchetypeData_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdPrefabs = this.m_HouseholdPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_RandomSeed = RandomSeed.Next(),
        m_UpdateFrameIndex = updateFrame,
        m_CitizenParametersData = this.m_CitizenParametersQuery.GetSingleton<CitizenParametersData>(),
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<DivorceSystem.CheckDivorceJob>(this.m_HouseholdQuery, JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DivorceSystem.SumDivorceJob jobData2 = new DivorceSystem.SumDivorceJob()
      {
        m_DebugDivorce = this.m_DebugDivorce,
        m_DebugDivorceCount = this.m_DebugDivorceCount
      };
      this.Dependency = jobData2.Schedule<DivorceSystem.SumDivorceJob>(this.Dependency);
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
    public DivorceSystem()
    {
    }

    [BurstCompile]
    private struct CheckDivorceJob : IJobChunk
    {
      public NativeCounter.Concurrent m_DebugDivorceCount;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Household> m_HouseholdType;
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenType;
      public BufferTypeHandle<Resources> m_ResourceType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> m_ArchetypeDatas;
      [ReadOnly]
      public NativeList<Entity> m_HouseholdPrefabs;
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public CitizenParametersData m_CitizenParametersData;
      public RandomSeed m_RandomSeed;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      private void Divorce(
        int index,
        Entity household,
        Entity leavingCitizen,
        Entity stayingCitizen,
        ref Household oldHouseholdData,
        ref Random random,
        DynamicBuffer<HouseholdCitizen> oldCitizenBuffer,
        DynamicBuffer<Resources> oldResourceBuffer)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_DebugDivorceCount.Increment();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity householdPrefab = this.m_HouseholdPrefabs[random.NextInt(this.m_HouseholdPrefabs.Length)];
        // ISSUE: reference to a compiler-generated field
        Game.Prefabs.ArchetypeData archetypeData = this.m_ArchetypeDatas[householdPrefab];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(index, archetypeData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Household>(index, entity, new Household()
        {
          m_Flags = oldHouseholdData.m_Flags,
          m_Resources = oldHouseholdData.m_Resources / 2
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PropertySeeker>(index, entity, new PropertySeeker());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(index, entity, new PrefabRef()
        {
          m_Prefab = householdPrefab
        });
        oldHouseholdData.m_Resources /= 2;
        // ISSUE: reference to a compiler-generated field
        HouseholdMember householdMember = this.m_HouseholdMembers[leavingCitizen] with
        {
          m_Household = entity
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<HouseholdMember>(index, leavingCitizen, householdMember);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<HouseholdCitizen>(index, entity).Add(new HouseholdCitizen()
        {
          m_Citizen = leavingCitizen
        });
        int amount = EconomyUtils.GetResources(Resource.Money, oldResourceBuffer) / 2;
        EconomyUtils.SetResources(Resource.Money, oldResourceBuffer, amount);
        // ISSUE: reference to a compiler-generated field
        EconomyUtils.SetResources(Resource.Money, this.m_CommandBuffer.SetBuffer<Resources>(index, entity), amount);
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenDivorced, Entity.Null, leavingCitizen, stayingCitizen));
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenDivorced, Entity.Null, stayingCitizen, leavingCitizen));
        for (int index1 = 0; index1 < oldCitizenBuffer.Length; ++index1)
        {
          if (oldCitizenBuffer[index1].m_Citizen == leavingCitizen)
          {
            oldCitizenBuffer.RemoveAt(index1);
            break;
          }
        }
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
        NativeArray<Household> nativeArray2 = chunk.GetNativeArray<Household>(ref this.m_HouseholdType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<HouseholdCitizen> bufferAccessor1 = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor2 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourceType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity household = nativeArray1[index1];
          DynamicBuffer<HouseholdCitizen> oldCitizenBuffer = bufferAccessor1[index1];
          int max = 0;
          for (int index2 = 0; index2 < oldCitizenBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            switch (this.m_Citizens[oldCitizenBuffer[index2].m_Citizen].GetAge())
            {
              case CitizenAge.Adult:
              case CitizenAge.Elderly:
                ++max;
                break;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (max >= 2 && (double) random.NextFloat(1f) < (double) this.m_CitizenParametersData.m_DivorceRate / (double) DivorceSystem.kUpdatesPerDay)
          {
            int num = random.NextInt(max);
            Entity leavingCitizen = Entity.Null;
            for (int index3 = 0; index3 < oldCitizenBuffer.Length; ++index3)
            {
              Entity citizen = oldCitizenBuffer[index3].m_Citizen;
              // ISSUE: reference to a compiler-generated field
              switch (this.m_Citizens[citizen].GetAge())
              {
                case CitizenAge.Adult:
                case CitizenAge.Elderly:
                  if (num == 0)
                  {
                    leavingCitizen = citizen;
                    goto label_16;
                  }
                  else
                  {
                    --num;
                    break;
                  }
              }
            }
label_16:
            if (leavingCitizen != Entity.Null)
            {
              Household oldHouseholdData = nativeArray2[index1];
              Entity stayingCitizen = Entity.Null;
              for (int index4 = 0; index4 < oldCitizenBuffer.Length; ++index4)
              {
                Entity citizen = oldCitizenBuffer[index4].m_Citizen;
                // ISSUE: reference to a compiler-generated field
                switch (this.m_Citizens[citizen].GetAge())
                {
                  case CitizenAge.Adult:
                  case CitizenAge.Elderly:
                    if (citizen != leavingCitizen)
                    {
                      stayingCitizen = citizen;
                      break;
                    }
                    break;
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.Divorce(unfilteredChunkIndex, household, leavingCitizen, stayingCitizen, ref oldHouseholdData, ref random, oldCitizenBuffer, bufferAccessor2[index1]);
              nativeArray2[index1] = oldHouseholdData;
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
    private struct SumDivorceJob : IJob
    {
      public NativeCounter m_DebugDivorceCount;
      public NativeValue<int> m_DebugDivorce;

      public void Execute() => this.m_DebugDivorce.value = this.m_DebugDivorceCount.Count;
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public BufferTypeHandle<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<Household> __Game_Citizens_Household_RW_ComponentTypeHandle;
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.ArchetypeData> __Game_Prefabs_ArchetypeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle = state.GetBufferTypeHandle<HouseholdCitizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ArchetypeData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.ArchetypeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
      }
    }
  }
}
