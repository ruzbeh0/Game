// Decompiled with JetBrains decompiler
// Type: Game.Simulation.LookForPartnerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Citizens;
using Game.Common;
using Game.Debug;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class LookForPartnerSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 4;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_CitizenQuery;
    private EntityQuery m_LookingQuery;
    private EntityQuery m_CitizenParametersQuery;
    private NativeQueue<LookingForPartner> m_Queue;
    [DebugWatchValue]
    private NativeValue<int> m_DebugLookingForPartner;
    private LookForPartnerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (LookForPartnerSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugLookingForPartner = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(new EntityQueryDesc()
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
      this.m_LookingQuery = this.GetEntityQuery(ComponentType.ReadOnly<LookingForPartner>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenParametersQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.m_Queue = new NativeQueue<LookingForPartner>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenParametersQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_Queue.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugLookingForPartner.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<int> nativeQueue = new NativeQueue<int>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, LookForPartnerSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      LookForPartnerSystem.LookForPartnerJob jobData1 = new LookForPartnerSystem.LookForPartnerJob()
      {
        m_DebugLookForPartnerQueue = nativeQueue,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RW_ComponentTypeHandle,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_Commuters = this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentLookup,
        m_Tourists = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_UpdateFrameIndex = updateFrame,
        m_CitizenParametersData = this.m_CitizenParametersQuery.GetSingleton<CitizenParametersData>(),
        m_Queue = this.m_Queue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<LookForPartnerSystem.LookForPartnerJob>(this.m_CitizenQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_LookingForPartner_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LookForPartnerSystem.AddPartnerSeekerJob jobData2 = new LookForPartnerSystem.AddPartnerSeekerJob()
      {
        m_DebugLookingForPartner = this.m_DebugLookingForPartner,
        m_LookingForPartners = this.__TypeHandle.__Game_Citizens_LookingForPartner_RW_BufferLookup,
        m_LookingForPartnerEntity = this.m_LookingQuery.GetSingletonEntity(),
        m_Queue = this.m_Queue
      };
      this.Dependency = jobData2.Schedule<LookForPartnerSystem.AddPartnerSeekerJob>(this.Dependency);
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
    public LookForPartnerSystem()
    {
    }

    [BurstCompile]
    private struct AddPartnerSeekerJob : IJob
    {
      public NativeValue<int> m_DebugLookingForPartner;
      public Entity m_LookingForPartnerEntity;
      public BufferLookup<LookingForPartner> m_LookingForPartners;
      public NativeQueue<LookingForPartner> m_Queue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_DebugLookingForPartner.value = this.m_Queue.Count;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LookingForPartner> lookingForPartner = this.m_LookingForPartners[this.m_LookingForPartnerEntity];
        LookingForPartner elem;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue.TryDequeue(out elem))
          lookingForPartner.Add(elem);
      }
    }

    [BurstCompile]
    private struct LookForPartnerJob : IJobChunk
    {
      [NativeDisableContainerSafetyRestriction]
      public NativeQueue<int> m_DebugLookForPartnerQueue;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_Tourists;
      [ReadOnly]
      public ComponentLookup<CommuterHousehold> m_Commuters;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      public NativeQueue<LookingForPartner>.ParallelWriter m_Queue;
      public RandomSeed m_RandomSeed;
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public CitizenParametersData m_CitizenParametersData;

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
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray2 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          // ISSUE: reference to a compiler-generated field
          Citizen citizen = this.m_Citizens[entity];
          switch (citizen.GetAge())
          {
            case CitizenAge.Child:
            case CitizenAge.Teen:
              continue;
            default:
              // ISSUE: reference to a compiler-generated field
              if ((citizen.m_State & CitizenFlags.LookingForPartner) == CitizenFlags.None && !CitizenUtils.IsDead(entity, ref this.m_HealthProblems))
              {
                Entity household = nativeArray2[index1].m_Household;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdCitizens.HasBuffer(household) && !this.m_Tourists.HasComponent(household) && !this.m_Commuters.HasComponent(household) && (this.m_Households[household].m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household];
                  int num1 = 0;
                  for (int index2 = 0; index2 < householdCitizen.Length; ++index2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    switch (this.m_Citizens[householdCitizen[index2].m_Citizen].GetAge())
                    {
                      case CitizenAge.Adult:
                      case CitizenAge.Elderly:
                        ++num1;
                        break;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (num1 < 2 && (double) random.NextFloat(1f) < (double) this.m_CitizenParametersData.m_LookForPartnerRate)
                  {
                    float num2 = citizen.GetPseudoRandom(CitizenPseudoRandom.PartnerType).NextFloat(1f);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    PartnerType partnerType = (double) num2 >= (double) this.m_CitizenParametersData.m_LookForPartnerTypeRate.x ? ((double) num2 >= (double) this.m_CitizenParametersData.m_LookForPartnerTypeRate.y ? PartnerType.Other : PartnerType.Any) : PartnerType.Same;
                    // ISSUE: reference to a compiler-generated field
                    this.m_Queue.Enqueue(new LookingForPartner()
                    {
                      m_Citizen = entity,
                      m_PartnerType = partnerType
                    });
                    citizen.m_State |= CitizenFlags.LookingForPartner;
                    // ISSUE: reference to a compiler-generated field
                    this.m_Citizens[entity] = citizen;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_DebugLookForPartnerQueue.IsCreated)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_DebugLookForPartnerQueue.Enqueue(1);
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RW_ComponentTypeHandle;
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RW_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CommuterHousehold> __Game_Citizens_CommuterHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      public BufferLookup<LookingForPartner> __Game_Citizens_LookingForPartner_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentLookup = state.GetComponentLookup<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CommuterHousehold_RO_ComponentLookup = state.GetComponentLookup<CommuterHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_LookingForPartner_RW_BufferLookup = state.GetBufferLookup<LookingForPartner>();
      }
    }
  }
}
