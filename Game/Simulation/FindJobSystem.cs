// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FindJobSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Debug;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using Game.Vehicles;
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
  public class FindJobSystem : GameSystemBase
  {
    private const int UPDATE_INTERVAL = 16;
    private EntityQuery m_JobSeekerQuery;
    private EntityQuery m_ResultsQuery;
    private EntityQuery m_FreeQuery;
    private SimulationSystem m_SimulationSystem;
    private TriggerSystem m_TriggerSystem;
    private CountHouseholdDataSystem m_CountHouseholdDataSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private NativeArray<int> m_FreeCache;
    [DebugWatchValue]
    private NativeValue<int> m_StartedWorking;
    [DebugWatchDeps]
    private JobHandle m_WriteDeps;
    private FindJobSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountHouseholdDataSystem = this.World.GetOrCreateSystemManaged<CountHouseholdDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeQuery = this.GetEntityQuery(ComponentType.ReadOnly<FreeWorkplaces>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_StartedWorking = new NativeValue<int>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_JobSeekerQuery = this.GetEntityQuery(ComponentType.ReadWrite<JobSeeker>(), ComponentType.ReadOnly<Owner>(), ComponentType.Exclude<PathInformation>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResultsQuery = this.GetEntityQuery(ComponentType.ReadWrite<JobSeeker>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PathInformation>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreeCache = new NativeArray<int>(5, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_JobSeekerQuery, this.m_ResultsQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartedWorking.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeCache.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_JobSeekerQuery.IsEmptyIgnoreFilter && !this.m_CountHouseholdDataSystem.IsCountDataNotReady())
      {
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle job0 = new FindJobSystem.CalculateFreeWorkplaceJob()
        {
          m_FreeWorkplaces = this.m_FreeQuery.ToComponentDataListAsync<FreeWorkplaces>((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
          m_FreeCache = this.m_FreeCache
        }.Schedule<FindJobSystem.CalculateFreeWorkplaceJob>(JobHandle.CombineDependencies(outJobHandle, this.Dependency));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Agents_JobSeeker_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        FindJobSystem.FindJobJob jobData = new FindJobSystem.FindJobJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_JobSeekerType = this.__TypeHandle.__Game_Agents_JobSeeker_RW_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle,
          m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
          m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
          m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
          m_CitizenDatas = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
          m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
          m_Households = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
          m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
          m_Deleteds = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
          m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
          m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 80, 16).AsParallelWriter(),
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
          m_FreeCache = this.m_FreeCache,
          m_EmployableByEducation = this.m_CountHouseholdDataSystem.GetEmployables(),
          m_RandomSeed = RandomSeed.Next()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<FindJobSystem.FindJobJob>(this.m_JobSeekerQuery, JobHandle.CombineDependencies(job0, this.Dependency));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PathfindSetupSystem.AddQueueWriter(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ResultsQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_FreeWorkplaces_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_JobSeeker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      FindJobSystem.StartWorkingJob jobData1 = new FindJobSystem.StartWorkingJob()
      {
        m_Chunks = this.m_ResultsQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_JobSeekerType = this.__TypeHandle.__Game_Agents_JobSeeker_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PathInfoType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_EmployeeBuffers = this.__TypeHandle.__Game_Companies_Employee_RW_BufferLookup,
        m_FreeWorkplaces = this.__TypeHandle.__Game_Companies_FreeWorkplaces_RW_ComponentLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_Deleteds = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_SpawnableBuildings = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_WorkProviders = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_StartedWorking = this.m_StartedWorking
      };
      this.Dependency = jobData1.Schedule<FindJobSystem.StartWorkingJob>(JobHandle.CombineDependencies(outJobHandle1, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDeps = JobHandle.CombineDependencies(this.Dependency, this.m_WriteDeps);
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
    public FindJobSystem()
    {
    }

    [BurstCompile]
    private struct CalculateFreeWorkplaceJob : IJob
    {
      [ReadOnly]
      public NativeList<FreeWorkplaces> m_FreeWorkplaces;
      public NativeArray<int> m_FreeCache;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_FreeCache.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_FreeCache[index] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_FreeWorkplaces.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          FreeWorkplaces freeWorkplace = this.m_FreeWorkplaces[index];
          // ISSUE: reference to a compiler-generated field
          this.m_FreeCache[0] += (int) freeWorkplace.m_Uneducated;
          // ISSUE: reference to a compiler-generated field
          this.m_FreeCache[1] += (int) freeWorkplace.m_PoorlyEducated;
          // ISSUE: reference to a compiler-generated field
          this.m_FreeCache[2] += (int) freeWorkplace.m_Educated;
          // ISSUE: reference to a compiler-generated field
          this.m_FreeCache[3] += (int) freeWorkplace.m_WellEducated;
          // ISSUE: reference to a compiler-generated field
          this.m_FreeCache[4] += (int) freeWorkplace.m_HighlyEducated;
        }
      }
    }

    [BurstCompile]
    private struct FindJobJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<JobSeeker> m_JobSeekerType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenDatas;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<Household> m_Households;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleteds;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public NativeArray<int> m_FreeCache;
      [ReadOnly]
      public NativeArray<int> m_EmployableByEducation;
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
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<JobSeeker> nativeArray3 = chunk.GetNativeArray<JobSeeker>(ref this.m_JobSeekerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentBuilding> nativeArray4 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity owner = nativeArray2[index1].m_Owner;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Deleteds.HasComponent(owner) || !this.m_CitizenDatas.HasComponent(owner))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, nativeArray1[index1], new Deleted());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Entity household1 = this.m_HouseholdMembers[owner].m_Household;
            // ISSUE: reference to a compiler-generated field
            Citizen citizenData = this.m_CitizenDatas[owner];
            Entity entity1 = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PropertyRenters.HasComponent(household1))
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_PropertyRenters[household1].m_Property;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (chunk.Has<CurrentBuilding>(ref this.m_CurrentBuildingType) && (citizenData.m_State & CitizenFlags.Commuter) != CitizenFlags.None)
                entity1 = nativeArray4[index1].m_CurrentBuilding;
            }
            if (entity1 != Entity.Null)
            {
              Entity entity2 = nativeArray1[index1];
              int level = (int) nativeArray3[index1].m_Level;
              int index2 = level;
              int num1 = -1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bool flag = this.m_Workers.HasComponent(owner) && this.m_OutsideConnections.HasComponent(this.m_Workers[owner].m_Workplace);
              // ISSUE: reference to a compiler-generated field
              if (this.m_Workers.HasComponent(owner) && !flag)
              {
                // ISSUE: reference to a compiler-generated field
                num1 = (int) this.m_Workers[owner].m_Level;
              }
              if (num1 >= 0 && index2 > level && index2 <= num1)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponentEnabled<HasJobSeeker>(unfilteredChunkIndex, owner, false);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity2, new Deleted());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                while (index2 > num1 && this.m_FreeCache[index2] <= 0)
                  --index2;
                if (index2 == -1)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponentEnabled<HasJobSeeker>(unfilteredChunkIndex, owner, false);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity2, new Deleted());
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  float num2 = (float) this.m_FreeCache[index2];
                  // ISSUE: reference to a compiler-generated field
                  float max = (float) this.m_EmployableByEducation[index2] / num2;
                  if (num1 < 0 || (double) random.NextFloat(max) <= 2.0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<PathInformation>(unfilteredChunkIndex, entity2, new PathInformation()
                    {
                      m_State = PathFlags.Pending
                    });
                    // ISSUE: reference to a compiler-generated field
                    Household household2 = this.m_Households[household1];
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household1];
                    // ISSUE: reference to a compiler-generated field
                    PathfindParameters parameters = new PathfindParameters()
                    {
                      m_MaxSpeed = (float2) 111.111115f,
                      m_WalkSpeed = (float2) 1.66666675f,
                      m_Weights = CitizenUtils.GetPathfindWeights(citizenData, household2, householdCitizen.Length),
                      m_Methods = PathMethod.Pedestrian | PathMethod.PublicTransportDay | PathMethod.PublicTransportNight,
                      m_MaxCost = CitizenBehaviorSystem.kMaxPathfindCost,
                      m_PathfindFlags = PathfindFlags.Simplified | PathfindFlags.IgnorePath
                    };
                    SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
                    setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
                    setupQueueTarget.m_Methods = PathMethod.Pedestrian;
                    SetupQueueTarget origin = setupQueueTarget;
                    setupQueueTarget = new SetupQueueTarget();
                    setupQueueTarget.m_Type = SetupTargetType.JobSeekerTo;
                    setupQueueTarget.m_Methods = PathMethod.Pedestrian;
                    setupQueueTarget.m_Value = level + 5 * (index2 + 1);
                    setupQueueTarget.m_Value2 = flag ? 0.0f : max;
                    SetupQueueTarget destination = setupQueueTarget;
                    if (nativeArray3[index1].m_Outside > (byte) 0)
                      destination.m_Flags |= SetupTargetFlags.Export;
                    if (flag)
                      destination.m_Flags |= SetupTargetFlags.Import;
                    // ISSUE: reference to a compiler-generated field
                    PathUtils.UpdateOwnedVehicleMethods(household1, ref this.m_OwnedVehicles, ref parameters, ref origin, ref destination);
                    // ISSUE: reference to a compiler-generated field
                    this.m_PathfindQueue.Enqueue(new SetupQueueItem(entity2, parameters, origin, destination));
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

    [BurstCompile]
    private struct StartWorkingJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<JobSeeker> m_JobSeekerType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInfoType;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleteds;
      public BufferLookup<Employee> m_EmployeeBuffers;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      public ComponentLookup<FreeWorkplaces> m_FreeWorkplaces;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildings;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_WorkProviders;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      public NativeQueue<TriggerAction> m_TriggerBuffer;
      public EntityCommandBuffer m_CommandBuffer;
      public uint m_SimulationFrame;
      public NativeValue<int> m_StartedWorking;

      public void Execute()
      {
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray1 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathInformation> nativeArray2 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInfoType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<JobSeeker> nativeArray4 = chunk.GetNativeArray<JobSeeker>(ref this.m_JobSeekerType);
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            if ((nativeArray2[index2].m_State & PathFlags.Pending) == (PathFlags) 0)
            {
              Entity e = nativeArray3[index2];
              Entity owner = nativeArray1[index2].m_Owner;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Citizens.HasComponent(owner) && !this.m_Deleteds.HasComponent(owner))
              {
                Entity destination = nativeArray2[index2].m_Destination;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Prefabs.HasComponent(destination) && this.m_EmployeeBuffers.HasBuffer(destination))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Employee> employeeBuffer = this.m_EmployeeBuffers[destination];
                  // ISSUE: reference to a compiler-generated field
                  WorkProvider workProvider = this.m_WorkProviders[destination];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Entity prefab1 = this.m_Prefabs[this.m_PropertyRenters.HasComponent(destination) ? this.m_PropertyRenters[destination].m_Property : destination].m_Prefab;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int level1 = this.m_SpawnableBuildings.HasComponent(prefab1) ? (int) this.m_SpawnableBuildings[prefab1].m_Level : 1;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Prefabs.HasComponent(destination) && (!this.m_Workers.HasComponent(owner) || destination != this.m_Workers[owner].m_Workplace))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Entity prefab2 = this.m_Prefabs[destination].m_Prefab;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_WorkplaceDatas.HasComponent(prefab2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_FreeWorkplaces.HasComponent(destination) && this.m_FreeWorkplaces[destination].Count > 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        WorkplaceData workplaceData = this.m_WorkplaceDatas[prefab2];
                        // ISSUE: reference to a compiler-generated field
                        Citizen citizen = this.m_Citizens[owner];
                        Workshift workshift = Workshift.Day;
                        // ISSUE: reference to a compiler-generated field
                        FreeWorkplaces freeWorkplace = this.m_FreeWorkplaces[destination];
                        freeWorkplace.Refresh(employeeBuffer, workProvider.m_MaxWorkers, workplaceData.m_Complexity, level1);
                        byte level2 = nativeArray4[index2].m_Level;
                        int bestFor = freeWorkplace.GetBestFor((int) level2);
                        if (bestFor >= 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          float num2 = new Random((uint) (1 + ((int) this.m_SimulationFrame ^ (int) citizen.m_PseudoRandom))).NextFloat();
                          if ((double) num2 < (double) workplaceData.m_EveningShiftProbability)
                            workshift = Workshift.Evening;
                          else if ((double) num2 < (double) workplaceData.m_EveningShiftProbability + (double) workplaceData.m_NightShiftProbability)
                            workshift = Workshift.Night;
                          employeeBuffer.Add(new Employee()
                          {
                            m_Worker = owner,
                            m_Level = (byte) bestFor
                          });
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_Workers.HasComponent(owner))
                          {
                            // ISSUE: reference to a compiler-generated field
                            this.m_CommandBuffer.RemoveComponent<Worker>(owner);
                          }
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.AddComponent<Worker>(owner, new Worker()
                          {
                            m_Workplace = destination,
                            m_Level = (byte) bestFor,
                            m_LastCommuteTime = nativeArray2[index2].m_Duration,
                            m_Shift = workshift
                          });
                          ++num1;
                          // ISSUE: reference to a compiler-generated field
                          this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenStartedWorking, Entity.Null, owner, destination));
                          freeWorkplace.Refresh(employeeBuffer, workProvider.m_MaxWorkers, workplaceData.m_Complexity, level1);
                          // ISSUE: reference to a compiler-generated field
                          this.m_FreeWorkplaces[destination] = freeWorkplace;
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (!this.m_Workers.HasComponent(owner))
                          ;
                      }
                    }
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (CitizenUtils.IsCommuter(owner, ref this.m_Citizens))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Deleted>(owner, new Deleted());
                    continue;
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponentEnabled<HasJobSeeker>(owner, false);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(e, new Deleted());
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_StartedWorking.value += num1;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentTypeHandle<JobSeeker> __Game_Agents_JobSeeker_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<JobSeeker> __Game_Agents_JobSeeker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public BufferLookup<Employee> __Game_Companies_Employee_RW_BufferLookup;
      public ComponentLookup<FreeWorkplaces> __Game_Companies_FreeWorkplaces_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_JobSeeker_RW_ComponentTypeHandle = state.GetComponentTypeHandle<JobSeeker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_JobSeeker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<JobSeeker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RW_BufferLookup = state.GetBufferLookup<Employee>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_FreeWorkplaces_RW_ComponentLookup = state.GetComponentLookup<FreeWorkplaces>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
      }
    }
  }
}
