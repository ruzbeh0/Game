// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WorkerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
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
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WorkerSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private TimeSystem m_TimeSystem;
    private CitizenBehaviorSystem m_CitizenBehaviorSystem;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_GotoWorkQuery;
    private EntityQuery m_WorkerQuery;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_PopulationQuery;
    private SimulationSystem m_SimulationSystem;
    private TriggerSystem m_TriggerSystem;
    private WorkerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public static float GetWorkOffset(Citizen citizen)
    {
      return (float) (citizen.GetPseudoRandom(CitizenPseudoRandom.WorkOffset).NextInt(21845) - 10922) / 262144f;
    }

    public static bool IsTodayOffDay(
      Citizen citizen,
      ref EconomyParameterData economyParameters,
      uint frame,
      TimeData timeData,
      int population)
    {
      int num = math.min(40, Mathf.RoundToInt(100f / math.max(1f, math.sqrt(economyParameters.m_TrafficReduction * (float) population))));
      // ISSUE: reference to a compiler-generated method
      int day = TimeSystem.GetDay(frame, timeData);
      return Unity.Mathematics.Random.CreateFromIndex((uint) citizen.m_PseudoRandom + (uint) day).NextInt(100) > num;
    }

    public static bool IsTimeToWork(
      Citizen citizen,
      Worker worker,
      ref EconomyParameterData economyParameters,
      float timeOfDay)
    {
      // ISSUE: reference to a compiler-generated method
      float2 timeToWork = WorkerSystem.GetTimeToWork(citizen, worker, ref economyParameters, true);
      return (double) timeToWork.x >= (double) timeToWork.y ? (double) timeOfDay >= (double) timeToWork.x || (double) timeOfDay <= (double) timeToWork.y : (double) timeOfDay >= (double) timeToWork.x && (double) timeOfDay <= (double) timeToWork.y;
    }

    public static float2 GetTimeToWork(
      Citizen citizen,
      Worker worker,
      ref EconomyParameterData economyParameters,
      bool includeCommute)
    {
      // ISSUE: reference to a compiler-generated method
      float workOffset = WorkerSystem.GetWorkOffset(citizen);
      if (worker.m_Shift == Workshift.Evening)
        workOffset += 0.33f;
      else if (worker.m_Shift == Workshift.Night)
        workOffset += 0.67f;
      double num1 = (double) math.frac((float) Mathf.RoundToInt((float) (24.0 * ((double) economyParameters.m_WorkDayStart + (double) workOffset))) / 24f);
      float y = math.frac((float) Mathf.RoundToInt((float) (24.0 * ((double) economyParameters.m_WorkDayEnd + (double) workOffset))) / 24f);
      float num2 = 0.0f;
      if (includeCommute)
      {
        float num3 = 60f * worker.m_LastCommuteTime;
        if ((double) num3 < 60.0)
          num3 = 40000f;
        num2 = num3 / 262144f;
      }
      double num4 = (double) num2;
      return new float2(math.frac((float) (num1 - num4)), y);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenBehaviorSystem = this.World.GetOrCreateSystemManaged<CitizenBehaviorSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Worker>(), ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<TravelPurpose>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_GotoWorkQuery = this.GetEntityQuery(ComponentType.ReadOnly<Worker>(), ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<TravelPurpose>(), ComponentType.Exclude<HealthProblem>(), ComponentType.Exclude<ResourceBuyer>(), ComponentType.ReadWrite<TripNeeded>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Population>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_GotoWorkQuery, this.m_WorkerQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = new WorkerSystem.GoToWorkJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_WorkerType = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_CarKeepers = this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup,
        m_Properties = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_Purposes = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_Attendings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_PopulationData = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_UpdateFrameIndex = frameWithInterval,
        m_Frame = this.m_SimulationSystem.frameIndex,
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_PopulationEntity = this.m_PopulationQuery.GetSingletonEntity(),
        m_CarReserverQueue = this.m_CitizenBehaviorSystem.GetCarReserveQueue(out deps),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<WorkerSystem.GoToWorkJob>(this.m_GotoWorkQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CitizenBehaviorSystem.AddCarReserveWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new WorkerSystem.WorkJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_WorkerType = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle,
        m_PurposeType = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_Attendings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_Workplaces = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter(),
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_UpdateFrameIndex = frameWithInterval,
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_Frame = this.m_SimulationSystem.frameIndex,
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<WorkerSystem.WorkJob>(this.m_WorkerQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(jobHandle2);
      this.Dependency = jobHandle2;
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
    public WorkerSystem()
    {
    }

    [BurstCompile]
    private struct GoToWorkJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<Worker> m_WorkerType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_Purposes;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_Properties;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<CarKeeper> m_CarKeepers;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_Attendings;
      [ReadOnly]
      public ComponentLookup<Population> m_PopulationData;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;
      public uint m_Frame;
      public TimeData m_TimeData;
      public uint m_UpdateFrameIndex;
      public float m_TimeOfDay;
      public Entity m_PopulationEntity;
      public EconomyParameterData m_EconomyParameters;
      public NativeQueue<Entity>.ParallelWriter m_CarReserverQueue;
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
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Worker> nativeArray3 = chunk.GetNativeArray<Worker>(ref this.m_WorkerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray4 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int population = this.m_PopulationData[this.m_PopulationEntity].m_Population;
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity1 = nativeArray1[index];
          Citizen citizen = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!WorkerSystem.IsTodayOffDay(citizen, ref this.m_EconomyParameters, this.m_Frame, this.m_TimeData, population) && WorkerSystem.IsTimeToWork(citizen, nativeArray3[index], ref this.m_EconomyParameters, this.m_TimeOfDay))
          {
            DynamicBuffer<TripNeeded> dynamicBuffer = bufferAccessor[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Attendings.HasComponent(entity1) && (citizen.m_State & CitizenFlags.MovingAwayReachOC) == CitizenFlags.None)
            {
              Entity workplace = nativeArray3[index].m_Workplace;
              Entity entity2 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Properties.HasComponent(workplace))
              {
                // ISSUE: reference to a compiler-generated field
                entity2 = this.m_Properties[workplace].m_Property;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_Buildings.HasComponent(workplace))
                {
                  entity2 = workplace;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OutsideConnections.HasComponent(workplace))
                    entity2 = workplace;
                }
              }
              if (entity2 != Entity.Null)
              {
                if (nativeArray4[index].m_CurrentBuilding != entity2)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_CarKeepers.IsComponentEnabled(entity1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CarReserverQueue.Enqueue(entity1);
                  }
                  dynamicBuffer.Add(new TripNeeded()
                  {
                    m_TargetAgent = workplace,
                    m_Purpose = Game.Citizens.Purpose.GoingToWork
                  });
                }
              }
              else
              {
                citizen.SetFailedEducationCount(0);
                nativeArray2[index] = citizen;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Purposes.HasComponent(entity1) && (this.m_Purposes[entity1].m_Purpose == Game.Citizens.Purpose.GoingToWork || this.m_Purposes[entity1].m_Purpose == Game.Citizens.Purpose.Working))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity1);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Worker>(unfilteredChunkIndex, entity1);
                // ISSUE: reference to a compiler-generated field
                this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenBecameUnemployed, Entity.Null, entity1, workplace));
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
    private struct WorkJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Worker> m_WorkerType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> m_PurposeType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_Workplaces;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_Attendings;
      public EconomyParameterData m_EconomyParameters;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;
      public float m_TimeOfDay;
      public uint m_UpdateFrameIndex;
      public uint m_Frame;
      public TimeData m_TimeData;
      public int m_Population;
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
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Worker> nativeArray2 = chunk.GetNativeArray<Worker>(ref this.m_WorkerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TravelPurpose> nativeArray3 = chunk.GetNativeArray<TravelPurpose>(ref this.m_PurposeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray4 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity workplace = nativeArray2[index].m_Workplace;
          Worker worker = nativeArray2[index];
          Citizen citizen = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Workplaces.HasComponent(workplace))
          {
            citizen.SetFailedEducationCount(0);
            nativeArray4[index] = citizen;
            TravelPurpose travelPurpose = nativeArray3[index];
            if (travelPurpose.m_Purpose == Game.Citizens.Purpose.GoingToWork || travelPurpose.m_Purpose == Game.Citizens.Purpose.Working)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Worker>(unfilteredChunkIndex, entity);
            // ISSUE: reference to a compiler-generated field
            this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenBecameUnemployed, Entity.Null, entity, workplace));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if ((!WorkerSystem.IsTimeToWork(citizen, worker, ref this.m_EconomyParameters, this.m_TimeOfDay) || this.m_Attendings.HasComponent(entity)) && nativeArray3[index].m_Purpose == Game.Citizens.Purpose.Working)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
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
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Worker> __Game_Citizens_Worker_RO_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> __Game_Citizens_AttendingMeeting_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RO_ComponentLookup = state.GetComponentLookup<CarKeeper>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_AttendingMeeting_RO_ComponentLookup = state.GetComponentLookup<AttendingMeeting>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
      }
    }
  }
}
