// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitizenTravelPurposeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
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
  public class CitizenTravelPurposeSystem : GameSystemBase
  {
    private TimeSystem m_TimeSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_ArrivedGroup;
    private EntityQuery m_StuckGroup;
    private EntityQuery m_EconomyParameterGroup;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_ServiceBuildingQuery;
    private CitizenTravelPurposeSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ArrivedGroup = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadWrite<TravelPurpose>(), ComponentType.ReadWrite<TripNeeded>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_StuckGroup = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadWrite<TravelPurpose>(), ComponentType.ReadWrite<TripNeeded>(), ComponentType.Exclude<CurrentTransport>(), ComponentType.Exclude<CurrentBuilding>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(), ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceBuildingQuery = this.GetEntityQuery(ComponentType.ReadWrite<CityServiceUpkeep>(), ComponentType.ReadWrite<Building>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_ArrivedGroup, this.m_StuckGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<CitizenTravelPurposeSystem.Arrive> nativeQueue = new NativeQueue<CitizenTravelPurposeSystem.Arrive>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Arrived_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      CitizenTravelPurposeSystem.CitizenArriveJob jobData = new CitizenTravelPurposeSystem.CitizenArriveJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_TravelPurposeType = this.__TypeHandle.__Game_Citizens_TravelPurpose_RW_ComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle,
        m_ArrivedType = this.__TypeHandle.__Game_Citizens_Arrived_RO_ComponentTypeHandle,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_Schools = this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup,
        m_WorkProviders = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
        m_Students = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_PoliceStationData = this.__TypeHandle.__Game_Buildings_PoliceStation_RO_ComponentLookup,
        m_PrisonData = this.__TypeHandle.__Game_Buildings_Prison_RO_ComponentLookup,
        m_HospitalData = this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup,
        m_DeathcareFacilityData = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RO_ComponentLookup,
        m_EmergencyShelterData = this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_EconomyParameters = this.m_EconomyParameterGroup.GetSingleton<EconomyParameterData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_ArriveQueue = nativeQueue.AsParallelWriter(),
        m_NormalizedTime = this.m_TimeSystem.normalizedTime,
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CitizenTravelPurposeSystem.CitizenArriveJob>(this.m_ArrivedGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Patient_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CitizenPresence_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle1 = new CitizenTravelPurposeSystem.ArriveJob()
      {
        m_CitizenPresenceData = this.__TypeHandle.__Game_Buildings_CitizenPresence_RW_ComponentLookup,
        m_Patients = this.__TypeHandle.__Game_Buildings_Patient_RW_BufferLookup,
        m_Occupants = this.__TypeHandle.__Game_Buildings_Occupant_RW_BufferLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_StatisticsQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps),
        m_ArriveQueue = nativeQueue
      }.Schedule<CitizenTravelPurposeSystem.ArriveJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      nativeQueue.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new CitizenTravelPurposeSystem.CitizenStuckJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_ServiceBuildings = this.m_ServiceBuildingQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_OutsideConnections = this.m_OutsideConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<CitizenTravelPurposeSystem.CitizenStuckJob>(this.m_StuckGroup, JobUtils.CombineDependencies(outJobHandle2, outJobHandle1, jobHandle1, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
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
    public CitizenTravelPurposeSystem()
    {
    }

    [BurstCompile]
    private struct CitizenArriveJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      public ComponentTypeHandle<TravelPurpose> m_TravelPurposeType;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      [ReadOnly]
      public ComponentTypeHandle<Arrived> m_ArrivedType;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> m_Students;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_WorkProviders;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> m_Schools;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> m_PoliceStationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> m_PrisonData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> m_HospitalData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.DeathcareFacility> m_DeathcareFacilityData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> m_EmergencyShelterData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<CitizenTravelPurposeSystem.Arrive>.ParallelWriter m_ArriveQueue;
      public EconomyParameterData m_EconomyParameters;
      public float m_NormalizedTime;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TravelPurpose> nativeArray2 = chunk.GetNativeArray<TravelPurpose>(ref this.m_TravelPurposeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray3 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<HealthProblem>(ref this.m_HealthProblemType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          bool flag2 = chunk.IsComponentEnabled<Arrived>(ref this.m_ArrivedType, index);
          Entity entity = nativeArray1[index];
          TravelPurpose travelPurpose = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if (flag1 && CitizenUtils.IsDead(entity, ref this.m_HealthProblems) && travelPurpose.m_Purpose != Game.Citizens.Purpose.Deathcare && travelPurpose.m_Purpose != Game.Citizens.Purpose.InDeathcare && travelPurpose.m_Purpose != Game.Citizens.Purpose.Hospital && travelPurpose.m_Purpose != Game.Citizens.Purpose.InHospital)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
          }
          else if (travelPurpose.m_Purpose == Game.Citizens.Purpose.Sleeping)
          {
            // ISSUE: reference to a compiler-generated field
            Citizen citizen = this.m_Citizens[entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!CitizenBehaviorSystem.IsSleepTime(entity, citizen, ref this.m_EconomyParameters, this.m_NormalizedTime, ref this.m_Workers, ref this.m_Students))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
              // ISSUE: reference to a compiler-generated field
              if (nativeArray3.Length != 0 && this.m_BuildingData.HasComponent(nativeArray3[index].m_CurrentBuilding))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.WakeUp));
              }
            }
          }
          else if (travelPurpose.m_Purpose == Game.Citizens.Purpose.VisitAttractions)
          {
            if (flag2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponentEnabled<Arrived>(unfilteredChunkIndex, entity, false);
            }
            if (random.NextInt(100) == 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
            }
          }
          else if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponentEnabled<Arrived>(unfilteredChunkIndex, entity, false);
            switch (travelPurpose.m_Purpose)
            {
              case Game.Citizens.Purpose.None:
              case Game.Citizens.Purpose.Shopping:
              case Game.Citizens.Purpose.Leisure:
              case Game.Citizens.Purpose.Exporting:
              case Game.Citizens.Purpose.MovingAway:
              case Game.Citizens.Purpose.Safety:
              case Game.Citizens.Purpose.Escape:
              case Game.Citizens.Purpose.Traveling:
              case Game.Citizens.Purpose.SendMail:
              case Game.Citizens.Purpose.Disappear:
              case Game.Citizens.Purpose.WaitingHome:
              case Game.Citizens.Purpose.PathFailed:
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.GoingHome:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_BuildingData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Resident));
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.GoingToWork:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_BuildingData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Worker));
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_Workers.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_WorkProviders.HasComponent(this.m_Workers[entity].m_Workplace))
                  {
                    travelPurpose.m_Purpose = Game.Citizens.Purpose.Working;
                    nativeArray2[index] = travelPurpose;
                    continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                  continue;
                }
                continue;
              case Game.Citizens.Purpose.GoingToSchool:
                // ISSUE: reference to a compiler-generated field
                if (this.m_Students.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Schools.HasComponent(this.m_Students[entity].m_School))
                  {
                    travelPurpose.m_Purpose = Game.Citizens.Purpose.Studying;
                    nativeArray2[index] = travelPurpose;
                    continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                  continue;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.Hospital:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_HospitalData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  travelPurpose.m_Purpose = Game.Citizens.Purpose.InHospital;
                  nativeArray2[index] = travelPurpose;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Patient));
                  continue;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.EmergencyShelter:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_EmergencyShelterData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  travelPurpose.m_Purpose = Game.Citizens.Purpose.InEmergencyShelter;
                  nativeArray2[index] = travelPurpose;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Occupant));
                  continue;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.GoingToJail:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_PoliceStationData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  travelPurpose.m_Purpose = Game.Citizens.Purpose.InJail;
                  nativeArray2[index] = travelPurpose;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Occupant));
                  continue;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.GoingToPrison:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_PrisonData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  travelPurpose.m_Purpose = Game.Citizens.Purpose.InPrison;
                  nativeArray2[index] = travelPurpose;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Occupant));
                  continue;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
                continue;
              case Game.Citizens.Purpose.Deathcare:
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_DeathcareFacilityData.HasComponent(nativeArray3[index].m_CurrentBuilding))
                {
                  travelPurpose.m_Purpose = Game.Citizens.Purpose.InDeathcare;
                  nativeArray2[index] = travelPurpose;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ArriveQueue.Enqueue(new CitizenTravelPurposeSystem.Arrive(entity, nativeArray3[index].m_CurrentBuilding, CitizenTravelPurposeSystem.ArriveType.Patient));
                  continue;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
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

    private struct Arrive
    {
      public Entity m_Citizen;
      public Entity m_Target;
      public CitizenTravelPurposeSystem.ArriveType m_Type;

      public Arrive(Entity citizen, Entity target, CitizenTravelPurposeSystem.ArriveType type)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Citizen = citizen;
        // ISSUE: reference to a compiler-generated field
        this.m_Target = target;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = type;
      }
    }

    private enum ArriveType
    {
      Patient,
      Occupant,
      Resident,
      Worker,
      WakeUp,
    }

    [BurstCompile]
    private struct ArriveJob : IJob
    {
      public ComponentLookup<CitizenPresence> m_CitizenPresenceData;
      public BufferLookup<Patient> m_Patients;
      public BufferLookup<Occupant> m_Occupants;
      public ComponentLookup<Household> m_Households;
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      public NativeQueue<StatisticsEvent> m_StatisticsQueue;
      public NativeQueue<CitizenTravelPurposeSystem.Arrive> m_ArriveQueue;

      private void SetPresent(CitizenTravelPurposeSystem.Arrive arrive)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CitizenPresenceData.HasComponent(arrive.m_Target))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        CitizenPresence citizenPresence = this.m_CitizenPresenceData[arrive.m_Target];
        citizenPresence.m_Delta = (sbyte) math.min((int) sbyte.MaxValue, (int) citizenPresence.m_Delta + 1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CitizenPresenceData[arrive.m_Target] = citizenPresence;
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_ArriveQueue.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CitizenTravelPurposeSystem.Arrive arrive = this.m_ArriveQueue.Dequeue();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CitizenTravelPurposeSystem.ArriveType type = arrive.m_Type;
          switch (type)
          {
            case CitizenTravelPurposeSystem.ArriveType.Patient:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Patients.HasBuffer(arrive.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<Patient>(this.m_Patients[arrive.m_Target], new Patient(arrive.m_Citizen));
                break;
              }
              break;
            case CitizenTravelPurposeSystem.ArriveType.Occupant:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Occupants.HasBuffer(arrive.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<Occupant>(this.m_Occupants[arrive.m_Target], new Occupant(arrive.m_Citizen));
                break;
              }
              break;
            case CitizenTravelPurposeSystem.ArriveType.Resident:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity household1 = this.m_HouseholdMembers[arrive.m_Citizen].m_Household;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PropertyRenters.HasComponent(household1) && this.m_PropertyRenters[household1].m_Property == arrive.m_Target)
              {
                // ISSUE: reference to a compiler-generated field
                Household household2 = this.m_Households[household1];
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdCitizens.HasBuffer(household1) && (household2.m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_StatisticsQueue.Enqueue(new StatisticsEvent()
                  {
                    m_Statistic = StatisticType.CitizensMovedIn,
                    m_Change = (float) this.m_HouseholdCitizens[household1].Length
                  });
                }
                household2.m_Flags |= HouseholdFlags.MovedIn;
                // ISSUE: reference to a compiler-generated field
                this.m_Households[household1] = household2;
              }
              // ISSUE: reference to a compiler-generated method
              this.SetPresent(arrive);
              break;
            case CitizenTravelPurposeSystem.ArriveType.Worker:
            case CitizenTravelPurposeSystem.ArriveType.WakeUp:
              // ISSUE: reference to a compiler-generated method
              this.SetPresent(arrive);
              break;
          }
        }
      }
    }

    [BurstCompile]
    private struct CitizenStuckJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAways;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public NativeList<Entity> m_OutsideConnections;
      [ReadOnly]
      public NativeList<Entity> m_ServiceBuildings;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray2 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HealthProblem> nativeArray3 = chunk.GetNativeArray<HealthProblem>(ref this.m_HealthProblemType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray4 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        if (nativeArray2.Length < chunk.Count || this.m_OutsideConnections.Length == 0)
          return;
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity1 = nativeArray1[index];
          Entity household = nativeArray2[index].m_Household;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = (this.m_Households[household].m_Flags & HouseholdFlags.MovedIn) != HouseholdFlags.None && !this.m_MovingAways.HasComponent(household);
          HealthProblem healthProblem;
          if (CollectionUtils.TryGet<HealthProblem>(nativeArray3, index, out healthProblem) && (healthProblem.m_Flags & HealthProblemFlags.Dead) != HealthProblemFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, nativeArray1[index]);
          }
          else
          {
            Entity entity2 = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            Random random = this.m_RandomSeed.GetRandom((1 + index) * (entity1.Index + 1));
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PropertyRenters.HasComponent(household))
              {
                // ISSUE: reference to a compiler-generated field
                entity2 = this.m_PropertyRenters[household].m_Property;
              }
              // ISSUE: reference to a compiler-generated field
              if (entity2 == Entity.Null && this.m_ServiceBuildings.Length > 0)
              {
                int num = 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                do
                {
                  ++num;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  entity2 = this.m_ServiceBuildings[random.NextInt(this.m_ServiceBuildings.Length)];
                }
                while ((!this.m_Buildings.HasComponent(entity2) || this.m_Buildings[entity2].m_RoadEdge == Entity.Null) && num < 10);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Buildings.HasComponent(entity2) || this.m_Buildings[entity2].m_RoadEdge == Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, nativeArray1[index]);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              entity2 = this.m_OutsideConnections[random.NextInt(this.m_OutsideConnections.Length)];
            }
            if (entity2 != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CurrentBuilding>(unfilteredChunkIndex, nativeArray1[index], new CurrentBuilding()
              {
                m_CurrentBuilding = entity2
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, nativeArray1[index]);
              Citizen citizen = nativeArray4[index] with
              {
                m_PenaltyCounter = byte.MaxValue
              };
              nativeArray4[index] = citizen;
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
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      public ComponentTypeHandle<TravelPurpose> __Game_Citizens_TravelPurpose_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Arrived> __Game_Citizens_Arrived_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> __Game_Buildings_School_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> __Game_Buildings_PoliceStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Prison> __Game_Buildings_Prison_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> __Game_Buildings_Hospital_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.DeathcareFacility> __Game_Buildings_DeathcareFacility_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> __Game_Buildings_EmergencyShelter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      public ComponentLookup<CitizenPresence> __Game_Buildings_CitizenPresence_RW_ComponentLookup;
      public BufferLookup<Patient> __Game_Buildings_Patient_RW_BufferLookup;
      public BufferLookup<Occupant> __Game_Buildings_Occupant_RW_BufferLookup;
      public ComponentLookup<Household> __Game_Citizens_Household_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TravelPurpose>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Arrived_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Arrived>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_School_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.School>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentLookup = state.GetComponentLookup<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PoliceStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.PoliceStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Prison_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Prison>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Hospital>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DeathcareFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.DeathcareFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_EmergencyShelter_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.EmergencyShelter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CitizenPresence_RW_ComponentLookup = state.GetComponentLookup<CitizenPresence>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Patient_RW_BufferLookup = state.GetBufferLookup<Patient>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Occupant_RW_BufferLookup = state.GetBufferLookup<Occupant>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentLookup = state.GetComponentLookup<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
      }
    }
  }
}
