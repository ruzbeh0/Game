// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ApplyToSchoolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ApplyToSchoolSystem : GameSystemBase
  {
    public static readonly int kCoolDown = 20000;
    public const uint UPDATE_INTERVAL = 8192;
    public bool debugFastApplySchool;
    private EntityQuery m_CitizenGroup;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private CitySystem m_CitySystem;
    private ApplyToSchoolSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_2069025490_0;
    private EntityQuery __query_2069025490_1;
    private EntityQuery __query_2069025490_2;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<Citizen>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<HealthProblem>(),
          ComponentType.ReadOnly<HasJobSeeker>(),
          ComponentType.ReadOnly<HasSchoolSeeker>(),
          ComponentType.ReadOnly<Game.Citizens.Student>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenGroup);
      this.RequireForUpdate<EconomyParameterData>();
      this.RequireForUpdate<TimeData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_SchoolSeekerCooldown_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      ApplyToSchoolSystem.ApplyToSchoolJob jobData = new ApplyToSchoolSystem.ApplyToSchoolJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_WorkerType = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SchoolDatas = this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup,
        m_HouseholdDatas = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_Fees = this.__TypeHandle.__Game_City_ServiceFee_RO_BufferLookup,
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_SchoolSeekerCooldowns = this.__TypeHandle.__Game_Citizens_SchoolSeekerCooldown_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_EconomyParameters = this.__query_2069025490_0.GetSingleton<EconomyParameterData>(),
        m_EducationParameters = this.__query_2069025490_1.GetSingleton<EducationParameterData>(),
        m_TimeData = this.__query_2069025490_2.GetSingleton<TimeData>(),
        m_City = this.m_CitySystem.City,
        m_UpdateFrameIndex = frameWithInterval,
        m_DebugFastApplySchool = this.debugFastApplySchool,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ApplyToSchoolSystem.ApplyToSchoolJob>(this.m_CitizenGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
    }

    public static float GetEnteringProbability(
      CitizenAge age,
      bool worker,
      int level,
      int wellbeing,
      float willingness,
      DynamicBuffer<CityModifier> cityModifiers,
      ref EducationParameterData educationParameterData)
    {
      if (level == 1)
        return age != CitizenAge.Child ? 0.0f : 1f;
      if (age == CitizenAge.Child || age == CitizenAge.Elderly)
        return 0.0f;
      if (level == 2)
        return !(age == CitizenAge.Adult | worker) ? educationParameterData.m_EnterHighSchoolProbability : educationParameterData.m_AdultEnterHighSchoolProbability;
      float num = (float) ((double) wellbeing / 60.0 * (0.5 + (double) willingness));
      if (level == 3)
        return (float) (0.5 * (worker ? (double) educationParameterData.m_WorkerContinueEducationProbability : 1.0)) * math.log((float) (1.6000000238418579 * (double) num + 1.0));
      if (level != 4)
        return 0.0f;
      float enteringProbability = (float) (0.30000001192092896 * (worker ? (double) educationParameterData.m_WorkerContinueEducationProbability : 1.0)) * num;
      CityUtils.ApplyModifier(ref enteringProbability, cityModifiers, CityModifierType.UniversityInterest);
      return enteringProbability;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_2069025490_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EconomyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2069025490_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EducationParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2069025490_2 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TimeData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public ApplyToSchoolSystem()
    {
    }

    [BurstCompile]
    public struct ApplyToSchoolJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<Worker> m_WorkerType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<SchoolData> m_SchoolDatas;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdDatas;
      [ReadOnly]
      public BufferLookup<Resources> m_Resources;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<ServiceFee> m_Fees;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAways;
      [ReadOnly]
      public ComponentLookup<SchoolSeekerCooldown> m_SchoolSeekerCooldowns;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public uint m_UpdateFrameIndex;
      public Entity m_City;
      public uint m_SimulationFrame;
      public EconomyParameterData m_EconomyParameters;
      public EducationParameterData m_EducationParameters;
      public TimeData m_TimeData;
      public bool m_DebugFastApplySchool;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DebugFastApplySchool && (int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Worker> nativeArray3 = chunk.GetNativeArray<Worker>(ref this.m_WorkerType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Citizen citizen = nativeArray2[index];
          CitizenAge age = citizen.GetAge();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (age != CitizenAge.Elderly && (this.m_DebugFastApplySchool || !this.m_SchoolSeekerCooldowns.HasComponent(nativeArray1[index]) || (long) this.m_SimulationFrame >= (long) this.m_SchoolSeekerCooldowns[nativeArray1[index]].m_SimulationFrame + (long) ApplyToSchoolSystem.kCoolDown))
          {
            // ISSUE: reference to a compiler-generated field
            SchoolLevel level = age != CitizenAge.Child || this.m_DebugFastApplySchool ? (SchoolLevel) (citizen.GetEducationLevel() + 1) : SchoolLevel.Elementary;
            int failedEducationCount = citizen.GetFailedEducationCount();
            if (failedEducationCount == 0 && age > CitizenAge.Teen && level == SchoolLevel.College)
              level = SchoolLevel.University;
            bool flag = age == CitizenAge.Child || age == CitizenAge.Teen && level >= SchoolLevel.HighSchool && level < SchoolLevel.University || age == CitizenAge.Adult && level >= SchoolLevel.HighSchool;
            // ISSUE: reference to a compiler-generated field
            Entity household = this.m_HouseholdMembers[nativeArray1[index]].m_Household;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_DebugFastApplySchool || flag && CitizenUtils.HasMovedIn(household, this.m_HouseholdDatas))
            {
              float willingness = citizen.GetPseudoRandom(CitizenPseudoRandom.StudyWillingness).NextFloat();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              float enteringProbability = ApplyToSchoolSystem.GetEnteringProbability(age, nativeArray3.IsCreated, (int) level, (int) citizen.m_WellBeing, willingness, cityModifier, ref this.m_EducationParameters);
              // ISSUE: reference to a compiler-generated field
              if (this.m_DebugFastApplySchool || (double) random.NextFloat(1f) < (double) enteringProbability)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PropertyRenters.HasComponent(household) && !this.m_TouristHouseholds.HasComponent(household) && !this.m_MovingAways.HasComponent(household))
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity property = this.m_PropertyRenters[household].m_Property;
                  // ISSUE: reference to a compiler-generated field
                  Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(unfilteredChunkIndex, entity, new Owner()
                  {
                    m_Owner = nativeArray1[index]
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<SchoolSeeker>(unfilteredChunkIndex, entity, new SchoolSeeker()
                  {
                    m_Level = (int) level
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<CurrentBuilding>(unfilteredChunkIndex, entity, new CurrentBuilding()
                  {
                    m_CurrentBuilding = property
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<HasSchoolSeeker>(unfilteredChunkIndex, nativeArray1[index], new HasSchoolSeeker()
                  {
                    m_Seeker = entity
                  });
                }
              }
              else if (level > SchoolLevel.HighSchool)
              {
                citizen.SetFailedEducationCount(math.min(3, failedEducationCount + 1));
                nativeArray2[index] = citizen;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<SchoolSeekerCooldown>(unfilteredChunkIndex, nativeArray1[index], new SchoolSeekerCooldown()
                {
                  m_SimulationFrame = this.m_SimulationFrame
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

    private struct TypeHandle
    {
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Worker> __Game_Citizens_Worker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SchoolData> __Game_Prefabs_SchoolData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceFee> __Game_City_ServiceFee_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SchoolSeekerCooldown> __Game_Citizens_SchoolSeekerCooldown_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SchoolData_RO_ComponentLookup = state.GetComponentLookup<SchoolData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_ServiceFee_RO_BufferLookup = state.GetBufferLookup<ServiceFee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_SchoolSeekerCooldown_RO_ComponentLookup = state.GetComponentLookup<SchoolSeekerCooldown>(true);
      }
    }
  }
}
