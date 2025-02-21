// Decompiled with JetBrains decompiler
// Type: Game.Citizens.CitizenInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class CitizenInitializeSystem : GameSystemBase
  {
    private EntityQuery m_Additions;
    private EntityQuery m_TimeSettingGroup;
    private EntityQuery m_CitizenPrefabs;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_DemandParameterQuery;
    private SimulationSystem m_SimulationSystem;
    private TriggerSystem m_TriggerSystem;
    private ModificationBarrier5 m_EndFrameBarrier;
    private CitizenInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_Additions = this.GetEntityQuery(ComponentType.ReadWrite<Citizen>(), ComponentType.ReadWrite<HouseholdMember>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenPrefabs = this.GetEntityQuery(ComponentType.ReadOnly<CitizenData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSettingGroup = this.GetEntityQuery(ComponentType.ReadOnly<TimeSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DemandParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Additions);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TimeDataQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TimeSettingGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DemandParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CitizenData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CrimeVictim_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_MailSender_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_HasJobSeeker_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Arrived_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CitizenInitializeSystem.InitializeCitizenJob jobData = new CitizenInitializeSystem.InitializeCitizenJob()
      {
        m_Entities = this.m_Additions.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_HouseholdMembers = this.m_Additions.ToComponentDataListAsync<HouseholdMember>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_CitizenPrefabs = this.m_CitizenPrefabs.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup,
        m_Arriveds = this.__TypeHandle.__Game_Citizens_Arrived_RW_ComponentLookup,
        m_CarKeepers = this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup,
        m_HasJobSeekers = this.__TypeHandle.__Game_Agents_HasJobSeeker_RW_ComponentLookup,
        m_MailSenders = this.__TypeHandle.__Game_Citizens_MailSender_RW_ComponentLookup,
        m_CrimeVictims = this.__TypeHandle.__Game_Citizens_CrimeVictim_RW_ComponentLookup,
        m_CitizenDatas = this.__TypeHandle.__Game_Prefabs_CitizenData_RO_ComponentLookup,
        m_DemandParameters = this.m_DemandParameterQuery.GetSingleton<DemandParameterData>(),
        m_TimeSettings = this.m_TimeSettingGroup.GetSingleton<TimeSettingsData>(),
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer()
      };
      this.Dependency = jobData.Schedule<CitizenInitializeSystem.InitializeCitizenJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
    }

    public static Entity GetPrefab(
      NativeArray<Entity> citizenPrefabs,
      Citizen citizen,
      ComponentLookup<CitizenData> citizenDatas,
      Random rnd)
    {
      int max = 0;
      for (int index = 0; index < citizenPrefabs.Length; ++index)
      {
        CitizenData citizenData = citizenDatas[citizenPrefabs[index]];
        if ((citizen.m_State & CitizenFlags.Male) == CitizenFlags.None ^ citizenData.m_Male)
          ++max;
      }
      if (max > 0)
      {
        int num = rnd.NextInt(max);
        for (int index = 0; index < citizenPrefabs.Length; ++index)
        {
          CitizenData citizenData = citizenDatas[citizenPrefabs[index]];
          if ((citizen.m_State & CitizenFlags.Male) == CitizenFlags.None ^ citizenData.m_Male)
          {
            --num;
            if (num < 0)
              return new PrefabRef()
              {
                m_Prefab = citizenPrefabs[index]
              }.m_Prefab;
          }
        }
      }
      return Entity.Null;
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
    public CitizenInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeCitizenJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public NativeList<HouseholdMember> m_HouseholdMembers;
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<Entity> m_CitizenPrefabs;
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      public ComponentLookup<Citizen> m_Citizens;
      public ComponentLookup<Arrived> m_Arriveds;
      public ComponentLookup<CrimeVictim> m_CrimeVictims;
      public ComponentLookup<CarKeeper> m_CarKeepers;
      public ComponentLookup<HasJobSeeker> m_HasJobSeekers;
      public ComponentLookup<MailSender> m_MailSenders;
      [ReadOnly]
      public ComponentLookup<CitizenData> m_CitizenDatas;
      public NativeQueue<TriggerAction> m_TriggerBuffer;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public uint m_SimulationFrame;
      [DeallocateOnJobCompletion]
      public TimeData m_TimeData;
      public DemandParameterData m_DemandParameters;
      public TimeSettingsData m_TimeSettings;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int daysPerYear = this.m_TimeSettings.m_DaysPerYear;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(0);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Entities.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_Entities[index1];
          // ISSUE: reference to a compiler-generated field
          this.m_Arriveds.SetComponentEnabled(entity1, false);
          // ISSUE: reference to a compiler-generated field
          this.m_MailSenders.SetComponentEnabled(entity1, false);
          // ISSUE: reference to a compiler-generated field
          this.m_CrimeVictims.SetComponentEnabled(entity1, false);
          // ISSUE: reference to a compiler-generated field
          this.m_CarKeepers.SetComponentEnabled(entity1, false);
          // ISSUE: reference to a compiler-generated field
          this.m_HasJobSeekers.SetComponentEnabled(entity1, false);
          // ISSUE: reference to a compiler-generated field
          Citizen citizen1 = this.m_Citizens[entity1];
          // ISSUE: reference to a compiler-generated field
          Entity household = this.m_HouseholdMembers[index1].m_Household;
          bool flag = (citizen1.m_State & CitizenFlags.Commuter) != 0;
          int num1 = (citizen1.m_State & CitizenFlags.Tourist) != 0 ? 1 : 0;
          citizen1.m_PseudoRandom = (ushort) (random.NextUInt() % 65536U);
          citizen1.m_Health = (byte) (40 + random.NextInt(20));
          citizen1.m_WellBeing = (byte) (40 + random.NextInt(20));
          citizen1.m_LeisureCounter = num1 == 0 ? (byte) (random.NextInt(92) + 128) : (byte) random.NextInt(128);
          if (random.NextBool())
            citizen1.m_State |= CitizenFlags.Male;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          Entity prefab = CitizenInitializeSystem.GetPrefab(this.m_CitizenPrefabs, citizen1, this.m_CitizenDatas, random);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PrefabRef>(entity1, new PrefabRef()
          {
            m_Prefab = prefab
          });
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household];
          householdCitizen.Add(new HouseholdCitizen()
          {
            m_Citizen = entity1
          });
          int num2 = 0;
          int2 int2 = int2.zero;
          if (citizen1.m_BirthDay == (short) 0)
          {
            citizen1.SetAge(CitizenAge.Child);
            Entity primaryTarget = Entity.Null;
            Entity entity2 = Entity.Null;
            for (int index2 = 0; index2 < householdCitizen.Length; ++index2)
            {
              Entity citizen2 = householdCitizen[index2].m_Citizen;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Citizens.HasComponent(citizen2) && this.m_Citizens[citizen2].GetAge() == CitizenAge.Adult)
              {
                if (primaryTarget == Entity.Null)
                  primaryTarget = citizen2;
                else
                  entity2 = citizen2;
              }
            }
            if (primaryTarget != Entity.Null)
            {
              if (entity2 != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenCoupleMadeBaby, Entity.Null, primaryTarget, entity1));
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenSingleMadeBaby, Entity.Null, primaryTarget, entity1));
              }
            }
          }
          else if (citizen1.m_BirthDay == (short) 1)
          {
            // ISSUE: reference to a compiler-generated method
            int adultAgeLimitInDays = AgingSystem.GetAdultAgeLimitInDays();
            // ISSUE: reference to a compiler-generated method
            num2 = adultAgeLimitInDays + random.NextInt(AgingSystem.GetElderAgeLimitInDays() - adultAgeLimitInDays);
            citizen1.SetAge(CitizenAge.Adult);
            int2.x = 0;
            int2.y = flag ? 4 : 3;
          }
          else if (citizen1.m_BirthDay == (short) 2)
          {
            double num3 = (double) citizen1.GetPseudoRandom(CitizenPseudoRandom.StudyWillingness).NextFloat();
            // ISSUE: reference to a compiler-generated field
            if ((double) random.NextFloat(1f) > (double) this.m_DemandParameters.m_TeenSpawnPercentage)
            {
              citizen1.SetAge(CitizenAge.Child);
            }
            else
            {
              citizen1.SetAge(CitizenAge.Teen);
              int2 = new int2(0, 1);
            }
          }
          else if (citizen1.m_BirthDay == (short) 3)
          {
            // ISSUE: reference to a compiler-generated method
            num2 = AgingSystem.GetElderAgeLimitInDays() + random.NextInt(3 * daysPerYear);
            citizen1.SetAge(CitizenAge.Elderly);
            int2 = new int2(0, 4);
          }
          else
          {
            num2 = 3 * daysPerYear + random.NextInt(daysPerYear);
            citizen1.SetAge(CitizenAge.Adult);
            int2 = new int2(2, 3);
          }
          float max = 0.0f;
          float num4 = 1f;
          for (int index3 = 0; index3 <= 3; ++index3)
          {
            if (index3 >= int2.x && index3 <= int2.y)
            {
              // ISSUE: reference to a compiler-generated field
              max += this.m_DemandParameters.m_NewCitizenEducationParameters[index3];
            }
            // ISSUE: reference to a compiler-generated field
            num4 -= this.m_DemandParameters.m_NewCitizenEducationParameters[index3];
          }
          if (int2.y == 4)
            max += num4;
          float num5 = random.NextFloat(max);
          for (int x = int2.x; x <= int2.y; ++x)
          {
            // ISSUE: reference to a compiler-generated field
            if (x == 4 || (double) num5 < (double) this.m_DemandParameters.m_NewCitizenEducationParameters[x])
            {
              citizen1.SetEducationLevel(x);
              break;
            }
            // ISSUE: reference to a compiler-generated field
            num5 -= this.m_DemandParameters.m_NewCitizenEducationParameters[x];
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          citizen1.m_BirthDay = (short) (TimeSystem.GetDay(this.m_SimulationFrame, this.m_TimeData) - num2);
          // ISSUE: reference to a compiler-generated field
          this.m_Citizens[entity1] = citizen1;
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RW_ComponentLookup;
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RW_BufferLookup;
      public ComponentLookup<Arrived> __Game_Citizens_Arrived_RW_ComponentLookup;
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RW_ComponentLookup;
      public ComponentLookup<HasJobSeeker> __Game_Agents_HasJobSeeker_RW_ComponentLookup;
      public ComponentLookup<MailSender> __Game_Citizens_MailSender_RW_ComponentLookup;
      public ComponentLookup<CrimeVictim> __Game_Citizens_CrimeVictim_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CitizenData> __Game_Prefabs_CitizenData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentLookup = state.GetComponentLookup<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RW_BufferLookup = state.GetBufferLookup<HouseholdCitizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Arrived_RW_ComponentLookup = state.GetComponentLookup<Arrived>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RW_ComponentLookup = state.GetComponentLookup<CarKeeper>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_HasJobSeeker_RW_ComponentLookup = state.GetComponentLookup<HasJobSeeker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_MailSender_RW_ComponentLookup = state.GetComponentLookup<MailSender>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CrimeVictim_RW_ComponentLookup = state.GetComponentLookup<CrimeVictim>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CitizenData_RO_ComponentLookup = state.GetComponentLookup<CitizenData>(true);
      }
    }
  }
}
