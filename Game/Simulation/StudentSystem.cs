// Decompiled with JetBrains decompiler
// Type: Game.Simulation.StudentSystem
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
  public class StudentSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private TimeSystem m_TimeSystem;
    private CitizenBehaviorSystem m_CitizenBehaviorSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_EconomyParameterQuery;
    private EntityQuery m_GotoSchoolQuery;
    private EntityQuery m_StudentQuery;
    private EntityQuery m_TimeDataQuery;
    private EntityQuery m_PopulationQuery;
    private StudentSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public static float GetStudyOffset(Citizen citizen)
    {
      return (float) (citizen.GetPseudoRandom(CitizenPseudoRandom.WorkOffset).NextInt(21845) - 10922) / 262144f;
    }

    public static bool IsTimeToStudy(
      Citizen citizen,
      Game.Citizens.Student student,
      ref EconomyParameterData economyParameters,
      float timeOfDay,
      uint frame,
      TimeData timeData,
      int population)
    {
      int num = math.min(40, Mathf.RoundToInt(100f / math.max(1f, math.sqrt(economyParameters.m_TrafficReduction * (float) population))));
      // ISSUE: reference to a compiler-generated method
      int day = TimeSystem.GetDay(frame, timeData);
      // ISSUE: reference to a compiler-generated method
      float2 timeToStudy = StudentSystem.GetTimeToStudy(citizen, student, ref economyParameters);
      if (Unity.Mathematics.Random.CreateFromIndex((uint) citizen.m_PseudoRandom + (uint) day).NextInt(100) > num)
        return false;
      return (double) timeToStudy.x >= (double) timeToStudy.y ? (double) timeOfDay >= (double) timeToStudy.x || (double) timeOfDay <= (double) timeToStudy.y : (double) timeOfDay >= (double) timeToStudy.x && (double) timeOfDay <= (double) timeToStudy.y;
    }

    public static float2 GetTimeToStudy(
      Citizen citizen,
      Game.Citizens.Student student,
      ref EconomyParameterData economyParameters)
    {
      // ISSUE: reference to a compiler-generated method
      float studyOffset = StudentSystem.GetStudyOffset(citizen);
      float num1 = 60f * student.m_LastCommuteTime;
      if ((double) num1 < 60.0)
        num1 = 1800f;
      float num2 = num1 / 262144f;
      return new float2(math.frac(economyParameters.m_WorkDayStart + studyOffset - num2), math.frac(economyParameters.m_WorkDayEnd + studyOffset));
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
      this.m_StudentQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Citizens.Student>(), ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<TravelPurpose>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_GotoSchoolQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Citizens.Student>(), ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<CurrentBuilding>(), ComponentType.Exclude<ResourceBuyer>(), ComponentType.Exclude<TravelPurpose>(), ComponentType.Exclude<HealthProblem>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PopulationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Population>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_StudentQuery, this.m_GotoSchoolQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new StudentSystem.GoToSchoolJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_StudentType = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_Purposes = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_CarKeepers = this.__TypeHandle.__Game_Citizens_CarKeeper_RO_ComponentLookup,
        m_Properties = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_Attendings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_PopulationData = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_Frame = this.m_SimulationSystem.frameIndex,
        m_PopulationEntity = this.m_PopulationQuery.GetSingletonEntity(),
        m_TimeData = this.m_TimeDataQuery.GetSingleton<TimeData>(),
        m_CarReserverQueue = this.m_CitizenBehaviorSystem.GetCarReserveQueue(out deps),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<StudentSystem.GoToSchoolJob>(this.m_GotoSchoolQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CitizenBehaviorSystem.AddCarReserveWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new StudentSystem.StudyJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_StudentType = this.__TypeHandle.__Game_Citizens_Student_RO_ComponentTypeHandle,
        m_PurposeType = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentTypeHandle,
        m_Attendings = this.__TypeHandle.__Game_Citizens_AttendingMeeting_RO_ComponentLookup,
        m_CurrentBuildings = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_Targets = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_Schools = this.__TypeHandle.__Game_Buildings_School_RO_ComponentLookup,
        m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<StudentSystem.StudyJob>(this.m_StudentQuery, JobHandle.CombineDependencies(this.Dependency, jobHandle));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public StudentSystem()
    {
    }

    [BurstCompile]
    private struct GoToSchoolJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> m_StudentType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_Properties;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<CarKeeper> m_CarKeepers;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_Purposes;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_Attendings;
      [ReadOnly]
      public ComponentLookup<Population> m_PopulationData;
      public float m_TimeOfDay;
      public uint m_Frame;
      public TimeData m_TimeData;
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
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray2 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Citizens.Student> nativeArray3 = chunk.GetNativeArray<Game.Citizens.Student>(ref this.m_StudentType);
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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (StudentSystem.IsTimeToStudy(citizen, nativeArray3[index], ref this.m_EconomyParameters, this.m_TimeOfDay, this.m_Frame, this.m_TimeData, population))
          {
            DynamicBuffer<TripNeeded> dynamicBuffer = bufferAccessor[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Attendings.HasComponent(entity1) && (citizen.m_State & CitizenFlags.MovingAwayReachOC) == CitizenFlags.None)
            {
              Entity school = nativeArray3[index].m_School;
              Entity entity2 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              if (this.m_Properties.HasComponent(school))
              {
                // ISSUE: reference to a compiler-generated field
                entity2 = this.m_Properties[school].m_Property;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Buildings.HasComponent(school) || this.m_OutsideConnections.HasComponent(school))
                  entity2 = school;
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
                    m_TargetAgent = school,
                    m_Purpose = Game.Citizens.Purpose.GoingToSchool
                  });
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Purposes.HasComponent(entity1) && (this.m_Purposes[entity1].m_Purpose == Game.Citizens.Purpose.Studying || this.m_Purposes[entity1].m_Purpose == Game.Citizens.Purpose.GoingToSchool))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity1);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<StudentsRemoved>(unfilteredChunkIndex, school);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Game.Citizens.Student>(unfilteredChunkIndex, entity1);
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
    private struct StudyJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> m_StudentType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> m_PurposeType;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> m_Schools;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_Targets;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildings;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> m_Attendings;
      public EconomyParameterData m_EconomyParameters;
      public float m_TimeOfDay;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Citizens.Student> nativeArray2 = chunk.GetNativeArray<Game.Citizens.Student>(ref this.m_StudentType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TravelPurpose> nativeArray3 = chunk.GetNativeArray<TravelPurpose>(ref this.m_PurposeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Citizen> nativeArray4 = chunk.GetNativeArray<Citizen>(ref this.m_CitizenType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity school = nativeArray2[index].m_School;
          // ISSUE: reference to a compiler-generated method
          float studyOffset = StudentSystem.GetStudyOffset(nativeArray4[index]);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Schools.HasComponent(school))
          {
            TravelPurpose travelPurpose = nativeArray3[index];
            if (travelPurpose.m_Purpose == Game.Citizens.Purpose.GoingToSchool || travelPurpose.m_Purpose == Game.Citizens.Purpose.Studying)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Game.Citizens.Student>(unfilteredChunkIndex, entity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Targets.HasComponent(entity) && this.m_CurrentBuildings.HasComponent(entity) && this.m_CurrentBuildings[entity].m_CurrentBuilding != school)
            {
              if (nativeArray3[index].m_Purpose == Game.Citizens.Purpose.Studying)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (((double) this.m_TimeOfDay > (double) this.m_EconomyParameters.m_WorkDayEnd + (double) studyOffset || (double) this.m_TimeOfDay < (double) this.m_EconomyParameters.m_WorkDayStart + (double) studyOffset || this.m_Attendings.HasComponent(entity)) && nativeArray3[index].m_Purpose == Game.Citizens.Purpose.Studying)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<TravelPurpose>(unfilteredChunkIndex, entity);
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
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Citizens.Student> __Game_Citizens_Student_RO_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarKeeper> __Game_Citizens_CarKeeper_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AttendingMeeting> __Game_Citizens_AttendingMeeting_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.School> __Game_Buildings_School_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Citizens.Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CarKeeper_RO_ComponentLookup = state.GetComponentLookup<CarKeeper>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_AttendingMeeting_RO_ComponentLookup = state.GetComponentLookup<AttendingMeeting>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_School_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.School>(true);
      }
    }
  }
}
