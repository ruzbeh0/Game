// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitizenBehaviorSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Events;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CitizenBehaviorSystem : GameSystemBase
  {
    public static readonly float kMaxPathfindCost = 17000f;
    public static readonly float kMaxMovingAwayCost = CitizenBehaviorSystem.kMaxPathfindCost * 10f;
    public static readonly int kMinLeisurePossibility = 80;
    private JobHandle m_CarReserveWriters;
    private EntityQuery m_CitizenQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_LeisureParameterQuery;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_PopulationQuery;
    private SimulationSystem m_SimulationSystem;
    private TimeSystem m_TimeSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityArchetype m_HouseholdArchetype;
    private NativeQueue<Entity> m_CarReserveQueue;
    private NativeQueue<Entity>.ParallelWriter m_ParallelCarReserveQueue;
    private CitizenBehaviorSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 11;

    public static float2 GetSleepTime(
      Entity entity,
      Citizen citizen,
      ref EconomyParameterData economyParameters,
      ref ComponentLookup<Worker> workers,
      ref ComponentLookup<Game.Citizens.Student> students)
    {
      int age = (int) citizen.GetAge();
      float2 float2_1 = new float2(0.875f, 0.175f);
      float num = float2_1.y - float2_1.x;
      Unity.Mathematics.Random pseudoRandom = citizen.GetPseudoRandom(CitizenPseudoRandom.SleepOffset);
      float2 x1 = float2_1 + pseudoRandom.NextFloat(0.0f, 0.2f);
      if (age == 3)
        x1 -= 0.05f;
      if (age == 0)
        x1 -= 0.1f;
      if (age == 1)
        x1 += 0.05f;
      float2 x2 = math.frac(x1);
      float2 float2_2;
      if (workers.HasComponent(entity))
      {
        // ISSUE: reference to a compiler-generated method
        float2_2 = WorkerSystem.GetTimeToWork(citizen, workers[entity], ref economyParameters, true);
      }
      else
      {
        if (!students.HasComponent(entity))
          return x2;
        // ISSUE: reference to a compiler-generated method
        float2_2 = StudentSystem.GetTimeToStudy(citizen, students[entity], ref economyParameters);
      }
      if ((double) float2_2.x < (double) float2_2.y)
      {
        if ((double) x2.x > (double) x2.y && (double) float2_2.y > (double) x2.x)
          x2 += float2_2.y - x2.x;
        else if ((double) x2.y > (double) float2_2.x)
          x2 += (float) (1.0 - ((double) x2.y - (double) float2_2.x));
      }
      else
        x2 = new float2(float2_2.y, float2_2.y + num);
      x2 = math.frac(x2);
      return x2;
    }

    public static bool IsSleepTime(
      Entity entity,
      Citizen citizen,
      ref EconomyParameterData economyParameters,
      float normalizedTime,
      ref ComponentLookup<Worker> workers,
      ref ComponentLookup<Game.Citizens.Student> students)
    {
      // ISSUE: reference to a compiler-generated method
      float2 sleepTime = CitizenBehaviorSystem.GetSleepTime(entity, citizen, ref economyParameters, ref workers, ref students);
      return (double) sleepTime.y < (double) sleepTime.x ? (double) normalizedTime > (double) sleepTime.x || (double) normalizedTime < (double) sleepTime.y : (double) normalizedTime > (double) sleepTime.x && (double) normalizedTime < (double) sleepTime.y;
    }

    public NativeQueue<Entity>.ParallelWriter GetCarReserveQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_CarReserveWriters;
      // ISSUE: reference to a compiler-generated field
      return this.m_ParallelCarReserveQueue;
    }

    public void AddCarReserveWriter(JobHandle writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CarReserveWriters = JobHandle.CombineDependencies(this.m_CarReserveWriters, writer);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CarReserveQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ParallelCarReserveQueue = this.m_CarReserveQueue.AsParallelWriter();
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<LeisureParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Population>());
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenQuery = this.GetEntityQuery(ComponentType.ReadWrite<Citizen>(), ComponentType.Exclude<TravelPurpose>(), ComponentType.Exclude<ResourceBuyer>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.ReadOnly<HouseholdMember>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(), ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdArchetype = this.World.EntityManager.CreateArchetype(ComponentType.ReadWrite<Household>(), ComponentType.ReadWrite<HouseholdNeed>(), ComponentType.ReadWrite<HouseholdCitizen>(), ComponentType.ReadWrite<TaxPayer>(), ComponentType.ReadWrite<Game.Economy.Resources>(), ComponentType.ReadWrite<UpdateFrame>(), ComponentType.ReadWrite<Created>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LeisureParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TimeDataQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PopulationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CarReserveQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CoordinatedMeeting_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CoordinatedMeetingAttendee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdNeed_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Leisure_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      CitizenBehaviorSystem.CitizenAITickJob jobData = new CitizenBehaviorSystem.CitizenAITickJob()
      {
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HouseholdMemberType = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_LeisureType = this.__TypeHandle.__Game_Citizens_Leisure_RO_ComponentTypeHandle,
        m_HouseholdNeeds = this.__TypeHandle.__Game_Citizens_HouseholdNeed_RW_ComponentLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CarKeepers = this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup,
        m_PersonalCars = this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentLookup,
        m_MovingAway = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_Students = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentLookup,
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_InDangerData = this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup,
        m_Attendees = this.__TypeHandle.__Game_Citizens_CoordinatedMeetingAttendee_RO_BufferLookup,
        m_Meetings = this.__TypeHandle.__Game_Citizens_CoordinatedMeeting_RW_ComponentLookup,
        m_AttendingMeetings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_MeetingDatas = this.__TypeHandle.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_BuildingStudents = this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup,
        m_PopulationData = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_CommuterHouseholds = this.__TypeHandle.__Game_Citizens_CommuterHousehold_RO_ComponentLookup,
        m_HouseholdCitizenBufs = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_HouseholdArchetype = this.m_HouseholdArchetype,
        m_OutsideConnectionEntities = this.m_OutsideConnectionQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_LeisureParameters = this.m_LeisureParameterQuery.GetSingleton<LeisureParametersData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdateFrameIndex = frameWithInterval,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_NormalizedTime = this.m_TimeSystem.normalizedTime,
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_PopulationEntity = this.m_PopulationQuery.GetSingletonEntity(),
        m_CarReserverQueue = this.m_ParallelCarReserveQueue,
        m_MailSenderQueue = nativeQueue1.AsParallelWriter(),
        m_SleepQueue = nativeQueue2.AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData.ScheduleParallel<CitizenBehaviorSystem.CitizenAITickJob>(this.m_CitizenQuery, JobHandle.CombineDependencies(this.m_CarReserveWriters, JobHandle.CombineDependencies(this.Dependency, outJobHandle)));
      // ISSUE: reference to a compiler-generated field
      jobData.m_OutsideConnectionEntities.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle1);
      // ISSUE: reference to a compiler-generated method
      this.AddCarReserveWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle2 = new CitizenBehaviorSystem.CitizenReserveHouseholdCarJob()
      {
        m_CarKeepers = this.__TypeHandle.__Game_Citizens_CarKeeper_RW_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_PersonalCars = this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_ReserverQueue = this.m_CarReserveQueue
      }.Schedule<CitizenBehaviorSystem.CitizenReserveHouseholdCarJob>(JobHandle.CombineDependencies(jobHandle1, this.m_CarReserveWriters));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      // ISSUE: reference to a compiler-generated method
      this.AddCarReserveWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MailProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_MailSender_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle3 = new CitizenBehaviorSystem.CitizenTryCollectMailJob()
      {
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_MailAccumulationData = this.__TypeHandle.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup,
        m_ServiceObjectData = this.__TypeHandle.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup,
        m_MailSenderData = this.__TypeHandle.__Game_Citizens_MailSender_RW_ComponentLookup,
        m_MailProducerData = this.__TypeHandle.__Game_Buildings_MailProducer_RW_ComponentLookup,
        m_MailSenderQueue = nativeQueue1
      }.Schedule<CitizenBehaviorSystem.CitizenTryCollectMailJob>(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle3);
      nativeQueue1.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CitizenPresence_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle4 = new CitizenBehaviorSystem.CitizeSleepJob()
      {
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CitizenPresenceData = this.__TypeHandle.__Game_Buildings_CitizenPresence_RW_ComponentLookup,
        m_SleepQueue = nativeQueue2
      }.Schedule<CitizenBehaviorSystem.CitizeSleepJob>(jobHandle1);
      nativeQueue2.Dispose(jobHandle4);
      this.Dependency = JobHandle.CombineDependencies(jobHandle2, jobHandle3, jobHandle4);
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
    public CitizenBehaviorSystem()
    {
    }

    [BurstCompile]
    private struct CitizenReserveHouseholdCarJob : IJob
    {
      public ComponentLookup<CarKeeper> m_CarKeepers;
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCars;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      public NativeQueue<Entity> m_ReserverQueue;

      public void Execute()
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ReserverQueue.TryDequeue(out entity))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdMembers.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Entity household = this.m_HouseholdMembers[entity].m_Household;
            Entity car = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (this.m_Citizens[entity].GetAge() != CitizenAge.Child && HouseholdBehaviorSystem.GetFreeCar(household, this.m_OwnedVehicles, this.m_PersonalCars, ref car) && !this.m_CarKeepers.IsComponentEnabled(entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CarKeepers.SetComponentEnabled(entity, true);
              // ISSUE: reference to a compiler-generated field
              this.m_CarKeepers[entity] = new CarKeeper()
              {
                m_Car = car
              };
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.PersonalCar personalCar = this.m_PersonalCars[car] with
              {
                m_Keeper = entity
              };
              // ISSUE: reference to a compiler-generated field
              this.m_PersonalCars[car] = personalCar;
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct CitizenTryCollectMailJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> m_MailAccumulationData;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> m_ServiceObjectData;
      public ComponentLookup<MailSender> m_MailSenderData;
      public ComponentLookup<MailProducer> m_MailProducerData;
      public NativeQueue<Entity> m_MailSenderQueue;

      public void Execute()
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_MailSenderQueue.TryDequeue(out entity))
        {
          CurrentBuilding componentData1;
          MailProducer componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_CurrentBuildingData.TryGetComponent(entity, out componentData1) && this.m_MailProducerData.TryGetComponent(componentData1.m_CurrentBuilding, out componentData2) && componentData2.m_SendingMail >= (ushort) 15 && !this.RequireCollect(this.m_PrefabRefData[componentData1.m_CurrentBuilding].m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            bool flag = this.m_MailSenderData.IsComponentEnabled(entity);
            // ISSUE: reference to a compiler-generated field
            MailSender mailSender = flag ? this.m_MailSenderData[entity] : new MailSender();
            int num = math.min((int) componentData2.m_SendingMail, 100 - (int) mailSender.m_Amount);
            if (num > 0)
            {
              mailSender.m_Amount += (ushort) num;
              componentData2.m_SendingMail -= (ushort) num;
              // ISSUE: reference to a compiler-generated field
              this.m_MailProducerData[componentData1.m_CurrentBuilding] = componentData2;
              if (!flag)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_MailSenderData.SetComponentEnabled(entity, true);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_MailSenderData[entity] = mailSender;
            }
          }
        }
      }

      private bool RequireCollect(Entity prefab)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnableBuildingData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          SpawnableBuildingData spawnableBuildingData = this.m_SpawnableBuildingData[prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_MailAccumulationData.HasComponent(spawnableBuildingData.m_ZonePrefab))
          {
            // ISSUE: reference to a compiler-generated field
            return this.m_MailAccumulationData[spawnableBuildingData.m_ZonePrefab].m_RequireCollect;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceObjectData.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ServiceObjectData serviceObjectData = this.m_ServiceObjectData[prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_MailAccumulationData.HasComponent(serviceObjectData.m_Service))
            {
              // ISSUE: reference to a compiler-generated field
              return this.m_MailAccumulationData[serviceObjectData.m_Service].m_RequireCollect;
            }
          }
        }
        return false;
      }
    }

    [BurstCompile]
    private struct CitizeSleepJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      public ComponentLookup<CitizenPresence> m_CitizenPresenceData;
      public NativeQueue<Entity> m_SleepQueue;

      public void Execute()
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_SleepQueue.TryDequeue(out entity))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentBuildingData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            CurrentBuilding currentBuilding = this.m_CurrentBuildingData[entity];
            // ISSUE: reference to a compiler-generated field
            if (this.m_CitizenPresenceData.HasComponent(currentBuilding.m_CurrentBuilding))
            {
              // ISSUE: reference to a compiler-generated field
              CitizenPresence citizenPresence = this.m_CitizenPresenceData[currentBuilding.m_CurrentBuilding];
              citizenPresence.m_Delta = (sbyte) math.max(-127, (int) citizenPresence.m_Delta - 1);
              // ISSUE: reference to a compiler-generated field
              this.m_CitizenPresenceData[currentBuilding.m_CurrentBuilding] = citizenPresence;
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct CitizenAITickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> m_HouseholdMemberType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public ComponentTypeHandle<Leisure> m_LeisureType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<HouseholdNeed> m_HouseholdNeeds;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_Transforms;
      [ReadOnly]
      public ComponentLookup<CarKeeper> m_CarKeepers;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCars;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAway;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> m_Students;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      [ReadOnly]
      public ComponentLookup<InDanger> m_InDangerData;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_AttendingMeetings;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CoordinatedMeeting> m_Meetings;
      [ReadOnly]
      public BufferLookup<CoordinatedMeetingAttendee> m_Attendees;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> m_MeetingDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> m_BuildingStudents;
      [ReadOnly]
      public ComponentLookup<Population> m_PopulationData;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public ComponentLookup<CommuterHousehold> m_CommuterHouseholds;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizenBufs;
      [ReadOnly]
      public EntityArchetype m_HouseholdArchetype;
      [ReadOnly]
      public NativeList<Entity> m_OutsideConnectionEntities;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameters;
      [ReadOnly]
      public LeisureParametersData m_LeisureParameters;
      public uint m_UpdateFrameIndex;
      public float m_NormalizedTime;
      public uint m_SimulationFrame;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<Entity>.ParallelWriter m_CarReserverQueue;
      public NativeQueue<Entity>.ParallelWriter m_MailSenderQueue;
      public NativeQueue<Entity>.ParallelWriter m_SleepQueue;
      public TimeData m_TimeData;
      public Entity m_PopulationEntity;
      public RandomSeed m_RandomSeed;

      private bool CheckSleep(
        int index,
        Entity entity,
        ref Citizen citizen,
        Entity currentBuilding,
        Entity household,
        Entity home,
        DynamicBuffer<TripNeeded> trips,
        ref EconomyParameterData economyParameters,
        ref Unity.Mathematics.Random random)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!CitizenBehaviorSystem.IsSleepTime(entity, citizen, ref economyParameters, this.m_NormalizedTime, ref this.m_Workers, ref this.m_Students))
          return false;
        if (home != Entity.Null && currentBuilding == home)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<TravelPurpose>(index, entity, new TravelPurpose()
          {
            m_Purpose = Game.Citizens.Purpose.Sleeping
          });
          // ISSUE: reference to a compiler-generated field
          this.m_SleepQueue.Enqueue(entity);
          // ISSUE: reference to a compiler-generated method
          this.ReleaseCar(index, entity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.GoHome(entity, home, trips, currentBuilding);
        }
        return true;
      }

      private void GoHome(
        Entity entity,
        Entity target,
        DynamicBuffer<TripNeeded> trips,
        Entity currentBuilding)
      {
        if (target == Entity.Null || currentBuilding == target)
          return;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarKeepers.IsComponentEnabled(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CarReserverQueue.Enqueue(entity);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_MailSenderQueue.Enqueue(entity);
        TripNeeded elem = new TripNeeded()
        {
          m_TargetAgent = target,
          m_Purpose = Game.Citizens.Purpose.GoingHome
        };
        trips.Add(elem);
      }

      private void GoToOutsideConnection(
        Entity entity,
        Entity household,
        Entity currentBuilding,
        Entity targetBuilding,
        ref Citizen citizen,
        DynamicBuffer<TripNeeded> trips,
        Game.Citizens.Purpose purpose,
        ref Unity.Mathematics.Random random)
      {
        if (purpose == Game.Citizens.Purpose.MovingAway)
        {
          for (int index = 0; index < trips.Length; ++index)
          {
            if (trips[index].m_Purpose == Game.Citizens.Purpose.MovingAway)
              return;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OutsideConnections.HasComponent(currentBuilding))
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CarKeepers.IsComponentEnabled(entity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CarReserverQueue.Enqueue(entity);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_MailSenderQueue.Enqueue(entity);
          if (targetBuilding == Entity.Null)
          {
            OutsideConnectionTransferType ocTransferType = OutsideConnectionTransferType.Train | OutsideConnectionTransferType.Air | OutsideConnectionTransferType.Ship;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnedVehicles.HasBuffer(household) && this.m_OwnedVehicles[household].Length > 0)
              ocTransferType |= OutsideConnectionTransferType.Road;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            BuildingUtils.GetRandomOutsideConnectionByTransferType(ref this.m_OutsideConnectionEntities, ref this.m_OutsideConnectionDatas, ref this.m_Prefabs, random, ocTransferType, out targetBuilding);
          }
          // ISSUE: reference to a compiler-generated field
          if (targetBuilding == Entity.Null && this.m_OutsideConnectionEntities.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            targetBuilding = this.m_OutsideConnectionEntities[random.NextInt(this.m_OutsideConnectionEntities.Length)];
          }
          trips.Add(new TripNeeded()
          {
            m_TargetAgent = targetBuilding,
            m_Purpose = purpose
          });
        }
        else
        {
          if (purpose != Game.Citizens.Purpose.MovingAway)
            return;
          citizen.m_State |= CitizenFlags.MovingAwayReachOC;
        }
      }

      private void GoShopping(
        int chunkIndex,
        Entity citizen,
        Entity household,
        HouseholdNeed need,
        float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarKeepers.IsComponentEnabled(citizen))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CarReserverQueue.Enqueue(citizen);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_MailSenderQueue.Enqueue(citizen);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<ResourceBuyer>(chunkIndex, citizen, new ResourceBuyer()
        {
          m_Payer = household,
          m_Flags = SetupTargetFlags.Commercial,
          m_Location = position,
          m_ResourceNeeded = need.m_Resource,
          m_AmountNeeded = need.m_Amount
        });
      }

      private float GetTimeLeftUntilInterval(float2 interval)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (double) this.m_NormalizedTime >= (double) interval.x ? 1f - this.m_NormalizedTime + interval.x : interval.x - this.m_NormalizedTime;
      }

      private bool DoLeisure(
        int chunkIndex,
        Entity citizenEntity,
        Entity householdEntity,
        Entity currentBuilding,
        bool isTourist,
        ref Citizen citizenData,
        int population,
        ref Unity.Mathematics.Random random,
        ref EconomyParameterData economyParameters)
      {
        if (isTourist)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OutsideConnections.HasComponent(currentBuilding) && this.m_TouristHouseholds[householdEntity].m_Hotel != Entity.Null)
            return false;
        }
        else
        {
          int num = 128 - (int) citizenData.m_LeisureCounter;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OutsideConnections.HasComponent(currentBuilding) || random.NextInt(this.m_LeisureParameters.m_LeisureRandomFactor) > num)
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        int num1 = math.min(CitizenBehaviorSystem.kMinLeisurePossibility, Mathf.RoundToInt(200f / math.max(1f, math.sqrt(economyParameters.m_TrafficReduction * (float) population))));
        if (!isTourist && random.NextInt(100) > num1)
          citizenData.m_LeisureCounter = byte.MaxValue;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        float x = this.GetTimeLeftUntilInterval(CitizenBehaviorSystem.GetSleepTime(citizenEntity, citizenData, ref economyParameters, ref this.m_Workers, ref this.m_Students));
        // ISSUE: reference to a compiler-generated field
        if (this.m_Workers.HasComponent(citizenEntity))
        {
          // ISSUE: reference to a compiler-generated field
          Worker worker = this.m_Workers[citizenEntity];
          // ISSUE: reference to a compiler-generated method
          float2 timeToWork = WorkerSystem.GetTimeToWork(citizenData, worker, ref economyParameters, true);
          // ISSUE: reference to a compiler-generated method
          x = math.min(x, this.GetTimeLeftUntilInterval(timeToWork));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Students.HasComponent(citizenEntity))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Citizens.Student student = this.m_Students[citizenEntity];
            // ISSUE: reference to a compiler-generated method
            float2 timeToStudy = StudentSystem.GetTimeToStudy(citizenData, student, ref economyParameters);
            // ISSUE: reference to a compiler-generated method
            x = math.min(x, this.GetTimeLeftUntilInterval(timeToStudy));
          }
        }
        if (isTourist)
          citizenData.m_LeisureCounter = (byte) 0;
        uint num2 = (uint) ((double) x * 262144.0);
        // ISSUE: reference to a compiler-generated field
        Leisure component = new Leisure()
        {
          m_LastPossibleFrame = this.m_SimulationFrame + num2
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Leisure>(chunkIndex, citizenEntity, component);
        return true;
      }

      private void ReleaseCar(int chunkIndex, Entity citizen)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarKeepers.IsComponentEnabled(citizen))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity car = this.m_CarKeepers[citizen].m_Car;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PersonalCars.HasComponent(car))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Vehicles.PersonalCar personalCar = this.m_PersonalCars[car] with
          {
            m_Keeper = Entity.Null
          };
          // ISSUE: reference to a compiler-generated field
          this.m_PersonalCars[car] = personalCar;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponentEnabled<CarKeeper>(chunkIndex, citizen, false);
      }

      private bool AttendMeeting(
        int chunkIndex,
        Entity entity,
        ref Citizen citizen,
        Entity household,
        Entity currentBuilding,
        DynamicBuffer<TripNeeded> trips,
        ref Unity.Mathematics.Random random)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarKeepers.IsComponentEnabled(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CarReserverQueue.Enqueue(entity);
        }
        // ISSUE: reference to a compiler-generated field
        Entity meeting1 = this.m_AttendingMeetings[entity].m_Meeting;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Attendees.HasBuffer(meeting1) && this.m_Meetings.HasComponent(meeting1))
        {
          // ISSUE: reference to a compiler-generated field
          CoordinatedMeeting meeting2 = this.m_Meetings[meeting1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Prefabs.HasComponent(meeting1) && meeting2.m_Status != MeetingStatus.Done)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            HaveCoordinatedMeetingData coordinatedMeetingData = this.m_MeetingDatas[this.m_Prefabs[meeting1].m_Prefab][meeting2.m_Phase];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CoordinatedMeetingAttendee> attendee = this.m_Attendees[meeting1];
            if (meeting2.m_Status == MeetingStatus.Waiting && meeting2.m_Target == Entity.Null)
            {
              if (attendee.Length > 0 && attendee[0].m_Attendee == entity)
              {
                if (coordinatedMeetingData.m_TravelPurpose.m_Purpose == Game.Citizens.Purpose.Shopping)
                {
                  // ISSUE: reference to a compiler-generated field
                  float3 position = this.m_Transforms[currentBuilding].m_Position;
                  // ISSUE: reference to a compiler-generated method
                  this.GoShopping(chunkIndex, entity, household, new HouseholdNeed()
                  {
                    m_Resource = coordinatedMeetingData.m_TravelPurpose.m_Resource,
                    m_Amount = coordinatedMeetingData.m_TravelPurpose.m_Data
                  }, position);
                  return true;
                }
                if (coordinatedMeetingData.m_TravelPurpose.m_Purpose == Game.Citizens.Purpose.Traveling)
                {
                  Citizen citizen1 = new Citizen();
                  // ISSUE: reference to a compiler-generated method
                  this.GoToOutsideConnection(entity, household, currentBuilding, Entity.Null, ref citizen1, trips, coordinatedMeetingData.m_TravelPurpose.m_Purpose, ref random);
                }
                else if (coordinatedMeetingData.m_TravelPurpose.m_Purpose == Game.Citizens.Purpose.GoingHome)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PropertyRenters.HasComponent(household))
                  {
                    // ISSUE: reference to a compiler-generated field
                    meeting2.m_Target = this.m_PropertyRenters[household].m_Property;
                    // ISSUE: reference to a compiler-generated field
                    this.m_Meetings[meeting1] = meeting2;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.GoHome(entity, this.m_PropertyRenters[household].m_Property, trips, currentBuilding);
                  }
                }
                else
                {
                  trips.Add(new TripNeeded()
                  {
                    m_Purpose = coordinatedMeetingData.m_TravelPurpose.m_Purpose,
                    m_Resource = coordinatedMeetingData.m_TravelPurpose.m_Resource,
                    m_Data = coordinatedMeetingData.m_TravelPurpose.m_Data,
                    m_TargetAgent = new Entity()
                  });
                  return true;
                }
              }
            }
            else if (meeting2.m_Status == MeetingStatus.Waiting || meeting2.m_Status == MeetingStatus.Traveling)
            {
              for (int index = 0; index < attendee.Length; ++index)
              {
                if (attendee[index].m_Attendee == entity)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (meeting2.m_Target != Entity.Null && currentBuilding != meeting2.m_Target && (!this.m_PropertyRenters.HasComponent(meeting2.m_Target) || this.m_PropertyRenters[meeting2.m_Target].m_Property != currentBuilding))
                    trips.Add(new TripNeeded()
                    {
                      m_Purpose = coordinatedMeetingData.m_TravelPurpose.m_Purpose,
                      m_Resource = coordinatedMeetingData.m_TravelPurpose.m_Resource,
                      m_Data = coordinatedMeetingData.m_TravelPurpose.m_Data,
                      m_TargetAgent = meeting2.m_Target
                    });
                  return true;
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(chunkIndex, entity);
              return false;
            }
          }
          return meeting2.m_Status != MeetingStatus.Done;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<AttendingMeeting>(chunkIndex, entity);
        return false;
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
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HouseholdMember> nativeArray3 = chunk.GetNativeArray<HouseholdMember>(ref this.m_HouseholdMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray4 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<HealthProblem>(ref this.m_HealthProblemType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int population = this.m_PopulationData[this.m_PopulationEntity].m_Population;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity household = nativeArray3[index].m_Household;
          Entity entity1 = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          bool isTourist = this.m_TouristHouseholds.HasComponent(household);
          DynamicBuffer<TripNeeded> trips = bufferAccessor[index];
          if (household == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_HouseholdArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HouseholdMember>(unfilteredChunkIndex, entity1, new HouseholdMember()
            {
              m_Household = entity2
            });
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetBuffer<HouseholdCitizen>(unfilteredChunkIndex, entity2).Add(new HouseholdCitizen()
            {
              m_Citizen = entity1
            });
            UnityEngine.Debug.LogWarning((object) string.Format("Citizen:{0} don't have valid household", (object) entity1.Index));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Households.HasComponent(household))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity1, new Deleted());
            }
            else
            {
              Entity currentBuilding = nativeArray4[index].m_CurrentBuilding;
              // ISSUE: reference to a compiler-generated field
              if (currentBuilding == Entity.Null && this.m_MovingAway.HasComponent(household))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, household, new Deleted());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Transforms.HasComponent(currentBuilding) && (!this.m_InDangerData.HasComponent(currentBuilding) || (this.m_InDangerData[currentBuilding].m_Flags & DangerFlags.StayIndoors) == (DangerFlags) 0))
                {
                  Citizen citizen = nativeArray2[index];
                  bool flag2 = (citizen.m_State & CitizenFlags.Commuter) != 0;
                  CitizenAge age = citizen.GetAge();
                  if (flag2 && (age == CitizenAge.Elderly || age == CitizenAge.Child))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity1, new Deleted());
                  }
                  if ((citizen.m_State & CitizenFlags.MovingAwayReachOC) != CitizenFlags.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity1, new Deleted());
                  }
                  else
                  {
                    MovingAway componentData1;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_MovingAway.TryGetComponent(household, out componentData1))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.GoToOutsideConnection(entity1, household, currentBuilding, componentData1.m_Target, ref citizen, trips, Game.Citizens.Purpose.MovingAway, ref random);
                      // ISSUE: reference to a compiler-generated field
                      if (chunk.Has<Leisure>(ref this.m_LeisureType))
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.RemoveComponent<Leisure>(unfilteredChunkIndex, entity1);
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_Workers.HasComponent(entity1))
                      {
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.RemoveComponent<Worker>(unfilteredChunkIndex, entity1);
                      }
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_Students.HasComponent(entity1))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_BuildingStudents.HasBuffer(this.m_Students[entity1].m_School))
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.AddComponent<StudentsRemoved>(unfilteredChunkIndex, this.m_Students[entity1].m_School);
                        }
                        // ISSUE: reference to a compiler-generated field
                        this.m_CommandBuffer.RemoveComponent<Game.Citizens.Student>(unfilteredChunkIndex, entity1);
                      }
                      nativeArray2[index] = citizen;
                    }
                    else
                    {
                      Entity entity3 = Entity.Null;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PropertyRenters.HasComponent(household))
                      {
                        // ISSUE: reference to a compiler-generated field
                        entity3 = this.m_PropertyRenters[household].m_Property;
                      }
                      else if (isTourist)
                      {
                        // ISSUE: reference to a compiler-generated field
                        Entity hotel = this.m_TouristHouseholds[household].m_Hotel;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_PropertyRenters.HasComponent(hotel))
                        {
                          // ISSUE: reference to a compiler-generated field
                          entity3 = this.m_PropertyRenters[hotel].m_Property;
                        }
                      }
                      else if (flag2)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_OutsideConnections.HasComponent(currentBuilding))
                        {
                          entity3 = currentBuilding;
                        }
                        else
                        {
                          CommuterHousehold componentData2;
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_CommuterHouseholds.TryGetComponent(household, out componentData2))
                            entity3 = componentData2.m_OriginalFrom;
                          if (entity3 == Entity.Null)
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            entity3 = this.m_OutsideConnectionEntities[random.NextInt(this.m_OutsideConnectionEntities.Length)];
                          }
                        }
                      }
                      if (flag1)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (chunk.Has<Leisure>(ref this.m_LeisureType))
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.RemoveComponent<Leisure>(unfilteredChunkIndex, entity1);
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        if (!this.m_AttendingMeetings.HasComponent(entity1) || !this.AttendMeeting(unfilteredChunkIndex, entity1, ref citizen, household, currentBuilding, trips, ref random))
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated method
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
                          // ISSUE: reference to a compiler-generated method
                          if (this.m_Workers.HasComponent(entity1) && !WorkerSystem.IsTodayOffDay(citizen, ref this.m_EconomyParameters, this.m_SimulationFrame, this.m_TimeData, population) && WorkerSystem.IsTimeToWork(citizen, this.m_Workers[entity1], ref this.m_EconomyParameters, this.m_NormalizedTime) || this.m_Students.HasComponent(entity1) && StudentSystem.IsTimeToStudy(citizen, this.m_Students[entity1], ref this.m_EconomyParameters, this.m_NormalizedTime, this.m_SimulationFrame, this.m_TimeData, population))
                          {
                            // ISSUE: reference to a compiler-generated field
                            if (chunk.Has<Leisure>(ref this.m_LeisureType))
                            {
                              // ISSUE: reference to a compiler-generated field
                              this.m_CommandBuffer.RemoveComponent<Leisure>(unfilteredChunkIndex, entity1);
                            }
                          }
                          else
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated method
                            if (this.CheckSleep(index, entity1, ref citizen, currentBuilding, household, entity3, trips, ref this.m_EconomyParameters, ref random))
                            {
                              // ISSUE: reference to a compiler-generated field
                              if (chunk.Has<Leisure>(ref this.m_LeisureType))
                              {
                                // ISSUE: reference to a compiler-generated field
                                this.m_CommandBuffer.RemoveComponent<Leisure>(unfilteredChunkIndex, entity1);
                              }
                            }
                            else
                            {
                              if (age == CitizenAge.Adult || age == CitizenAge.Elderly)
                              {
                                // ISSUE: reference to a compiler-generated field
                                HouseholdNeed householdNeed = this.m_HouseholdNeeds[household];
                                // ISSUE: reference to a compiler-generated field
                                if (householdNeed.m_Resource != Resource.NoResource && this.m_Transforms.HasComponent(currentBuilding))
                                {
                                  // ISSUE: reference to a compiler-generated field
                                  // ISSUE: reference to a compiler-generated method
                                  this.GoShopping(unfilteredChunkIndex, entity1, household, householdNeed, this.m_Transforms[currentBuilding].m_Position);
                                  householdNeed.m_Resource = Resource.NoResource;
                                  // ISSUE: reference to a compiler-generated field
                                  this.m_HouseholdNeeds[household] = householdNeed;
                                  // ISSUE: reference to a compiler-generated field
                                  if (chunk.Has<Leisure>(ref this.m_LeisureType))
                                  {
                                    // ISSUE: reference to a compiler-generated field
                                    this.m_CommandBuffer.RemoveComponent<Leisure>(unfilteredChunkIndex, entity1);
                                    continue;
                                  }
                                  continue;
                                }
                              }
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated method
                              if (!chunk.Has<Leisure>(ref this.m_LeisureType) && this.DoLeisure(unfilteredChunkIndex, entity1, household, currentBuilding, isTourist, ref citizen, population, ref random, ref this.m_EconomyParameters))
                              {
                                nativeArray2[index] = citizen;
                              }
                              else
                              {
                                // ISSUE: reference to a compiler-generated field
                                if (!chunk.Has<Leisure>(ref this.m_LeisureType))
                                {
                                  if (currentBuilding != entity3)
                                  {
                                    // ISSUE: reference to a compiler-generated method
                                    this.GoHome(entity1, entity3, trips, currentBuilding);
                                  }
                                  else
                                  {
                                    // ISSUE: reference to a compiler-generated method
                                    this.ReleaseCar(unfilteredChunkIndex, entity1);
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
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
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Leisure> __Game_Citizens_Leisure_RO_ComponentTypeHandle;
      public ComponentLookup<HouseholdNeed> __Game_Citizens_HouseholdNeed_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RO_ComponentLookup;
      public ComponentLookup<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InDanger> __Game_Events_InDanger_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CoordinatedMeetingAttendee> __Game_Citizens_CoordinatedMeetingAttendee_RO_BufferLookup;
      public ComponentLookup<CoordinatedMeeting> __Game_Citizens_CoordinatedMeeting_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> __Game_Citizens_AttendingMeeting_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HaveCoordinatedMeetingData> __Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> __Game_Buildings_Student_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<CommuterHousehold> __Game_Citizens_CommuterHousehold_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MailAccumulationData> __Game_Prefabs_MailAccumulationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceObjectData> __Game_Prefabs_ServiceObjectData_RO_ComponentLookup;
      public ComponentLookup<MailSender> __Game_Citizens_MailSender_RW_ComponentLookup;
      public ComponentLookup<MailProducer> __Game_Buildings_MailProducer_RW_ComponentLookup;
      public ComponentLookup<CitizenPresence> __Game_Buildings_CitizenPresence_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Leisure_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Leisure>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdNeed_RW_ComponentLookup = state.GetComponentLookup<HouseholdNeed>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RO_ComponentLookup = state.GetComponentLookup<CarKeeper>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PersonalCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentLookup = state.GetComponentLookup<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RO_ComponentLookup = state.GetComponentLookup<InDanger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CoordinatedMeetingAttendee_RO_BufferLookup = state.GetBufferLookup<CoordinatedMeetingAttendee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CoordinatedMeeting_RW_ComponentLookup = state.GetComponentLookup<CoordinatedMeeting>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_AttendingMeeting_RO_ComponentLookup = state.GetComponentLookup<AttendingMeeting>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HaveCoordinatedMeetingData_RO_BufferLookup = state.GetBufferLookup<HaveCoordinatedMeetingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferLookup = state.GetBufferLookup<Game.Buildings.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CommuterHousehold_RO_ComponentLookup = state.GetComponentLookup<CommuterHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RW_ComponentLookup = state.GetComponentLookup<CarKeeper>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MailAccumulationData_RO_ComponentLookup = state.GetComponentLookup<MailAccumulationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceObjectData_RO_ComponentLookup = state.GetComponentLookup<ServiceObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_MailSender_RW_ComponentLookup = state.GetComponentLookup<MailSender>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MailProducer_RW_ComponentLookup = state.GetComponentLookup<MailProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CitizenPresence_RW_ComponentLookup = state.GetComponentLookup<CitizenPresence>();
      }
    }
  }
}
