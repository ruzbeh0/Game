// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CrimeCheckSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Net;
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
  public class CrimeCheckSystem : GameSystemBase
  {
    public readonly int kUpdatesPerDay = 1;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_CitizenQuery;
    private EntityQuery m_EventQuery;
    private EntityQuery m_PoliceConfigurationQuery;
    private TriggerSystem m_TriggerSystem;
    private CrimeCheckSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (this.kUpdatesPerDay * 16);
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
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Citizen>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<HealthProblem>(),
          ComponentType.ReadOnly<Worker>(),
          ComponentType.ReadOnly<Game.Citizens.Student>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Prefabs.CrimeData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PoliceConfigurationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, this.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CrimeData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CrimeCheckSystem.CrimeCheckJob()
      {
        m_EventPrefabChunks = this.m_EventQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_CriminalType = this.__TypeHandle.__Game_Citizens_Criminal_RO_ComponentTypeHandle,
        m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle,
        m_PrefabEventType = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle,
        m_CrimeDataType = this.__TypeHandle.__Game_Prefabs_CrimeData_RO_ComponentTypeHandle,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_ServiceCoverages = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferLookup,
        m_Populations = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter(),
        m_UpdateFrameIndex = updateFrame,
        m_RandomSeed = RandomSeed.Next(),
        m_PoliceConfigurationData = this.m_PoliceConfigurationQuery.GetSingleton<PoliceConfigurationData>(),
        m_City = this.m_CitySystem.City,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<CrimeCheckSystem.CrimeCheckJob>(this.m_CitizenQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle);
      this.Dependency = jobHandle;
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
    public CrimeCheckSystem()
    {
    }

    [BurstCompile]
    private struct CrimeCheckJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EventPrefabChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<Criminal> m_CriminalType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public ComponentTypeHandle<EventData> m_PrefabEventType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Prefabs.CrimeData> m_CrimeDataType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> m_ServiceCoverages;
      [ReadOnly]
      public ComponentLookup<Population> m_Populations;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public PoliceConfigurationData m_PoliceConfigurationData;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

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
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Criminal> nativeArray3 = chunk.GetNativeArray<Criminal>(ref this.m_CriminalType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray4 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Citizen citizen = nativeArray2[index];
          switch (citizen.GetAge())
          {
            case CitizenAge.Child:
            case CitizenAge.Elderly:
              continue;
            default:
              bool isCriminal = nativeArray3.Length != 0;
              if (!isCriminal || !(nativeArray3[index].m_Event != Entity.Null))
              {
                Entity household = Entity.Null;
                if (nativeArray4.Length != 0)
                  household = nativeArray4[index].m_Household;
                Entity property = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PropertyRenterData.HasComponent(household))
                {
                  // ISSUE: reference to a compiler-generated field
                  property = this.m_PropertyRenterData[household].m_Property;
                }
                // ISSUE: reference to a compiler-generated method
                this.TryAddCrime(unfilteredChunkIndex, ref random, entity, citizen, isCriminal, household, property, cityModifier);
                continue;
              }
              continue;
          }
        }
      }

      private void TryAddCrime(
        int jobIndex,
        ref Random random,
        Entity entity,
        Citizen citizen,
        bool isCriminal,
        Entity household,
        Entity property,
        DynamicBuffer<CityModifier> cityModifiers)
      {
        float s;
        if (citizen.m_WellBeing <= (byte) 25)
        {
          s = (float) citizen.m_WellBeing / 25f;
        }
        else
        {
          float num = (float) (100 - (int) citizen.m_WellBeing) / 75f;
          s = num * num;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_EventPrefabChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk eventPrefabChunk = this.m_EventPrefabChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = eventPrefabChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<EventData> nativeArray2 = eventPrefabChunk.GetNativeArray<EventData>(ref this.m_PrefabEventType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Prefabs.CrimeData> nativeArray3 = eventPrefabChunk.GetNativeArray<Game.Prefabs.CrimeData>(ref this.m_CrimeDataType);
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = eventPrefabChunk.GetEnabledMask<Locked>(ref this.m_LockedType);
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            Game.Prefabs.CrimeData crimeData = nativeArray3[index2];
            if (crimeData.m_RandomTargetType == EventTargetType.Citizen && (!enabledMask.EnableBit.IsValid || !enabledMask[index2]))
            {
              float num = !isCriminal ? math.lerp(crimeData.m_OccurenceProbability.min, crimeData.m_OccurenceProbability.max, s) : math.lerp(crimeData.m_RecurrenceProbability.min, crimeData.m_RecurrenceProbability.max, s);
              CityUtils.ApplyModifier(ref num, cityModifiers, CityModifierType.CrimeProbability);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float max = math.max((float) ((double) this.m_Populations[this.m_City].m_Population / (double) this.m_PoliceConfigurationData.m_CrimePopulationReduction * 100.0), 100f);
              if ((double) random.NextFloat(max) < (double) num)
              {
                // ISSUE: reference to a compiler-generated field
                if (isCriminal && property != Entity.Null && this.m_BuildingData.HasComponent(property))
                {
                  // ISSUE: reference to a compiler-generated field
                  Building building = this.m_BuildingData[property];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ServiceCoverages.HasBuffer(building.m_RoadEdge))
                  {
                    // ISSUE: reference to a compiler-generated field
                    float serviceCoverage = NetUtils.GetServiceCoverage(this.m_ServiceCoverages[building.m_RoadEdge], CoverageService.Welfare, building.m_CurvePosition);
                    // ISSUE: reference to a compiler-generated field
                    if ((double) random.NextFloat(max) < (double) serviceCoverage * (double) this.m_PoliceConfigurationData.m_WelfareCrimeRecurrenceFactor)
                      continue;
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                {
                  m_Statistic = StatisticType.CrimeCount,
                  m_Change = 1f
                });
                // ISSUE: reference to a compiler-generated method
                this.CreateCrimeEvent(jobIndex, entity, nativeArray1[index2], nativeArray2[index2]);
                return;
              }
            }
          }
        }
      }

      private void CreateCrimeEvent(
        int jobIndex,
        Entity targetEntity,
        Entity eventPrefab,
        EventData eventData)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, eventData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(eventPrefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<TargetElement>(jobIndex, entity).Add(new TargetElement(targetEntity));
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenCommitedCrime, eventPrefab, targetEntity, Entity.Null));
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
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Criminal> __Game_Citizens_Criminal_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EventData> __Game_Prefabs_EventData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Prefabs.CrimeData> __Game_Prefabs_CrimeData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Criminal_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Criminal>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CrimeData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Prefabs.CrimeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferLookup = state.GetBufferLookup<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
