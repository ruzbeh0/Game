// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitizenFindJobSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Tools;
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
  public class CitizenFindJobSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 256;
    public static readonly int kJobSeekCoolDownMax = 10000;
    public static readonly int kJobSeekCoolDownMin = 5000;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_UnemployedQuery;
    private EntityQuery m_EmployedQuery;
    private EntityQuery m_CitizenParametersQuery;
    private SimulationSystem m_SimulationSystem;
    private CountWorkplacesSystem m_CountWorkplacesSystem;
    private CitizenFindJobSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (CitizenFindJobSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UnemployedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Citizen>(),
          ComponentType.ReadOnly<HouseholdMember>()
        },
        None = new ComponentType[7]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Worker>(),
          ComponentType.ReadOnly<Game.Citizens.Student>(),
          ComponentType.ReadOnly<HasJobSeeker>(),
          ComponentType.ReadOnly<HasSchoolSeeker>(),
          ComponentType.ReadOnly<HealthProblem>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EmployedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Citizen>(),
          ComponentType.ReadOnly<HouseholdMember>(),
          ComponentType.ReadOnly<Worker>()
        },
        None = new ComponentType[6]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Game.Citizens.Student>(),
          ComponentType.ReadOnly<HasJobSeeker>(),
          ComponentType.ReadOnly<HasSchoolSeeker>(),
          ComponentType.ReadOnly<HealthProblem>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenParametersQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenParametersData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountWorkplacesSystem = this.World.GetOrCreateSystemManaged<CountWorkplacesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CitizenParametersQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UnemployedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, CitizenFindJobSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_HasJobSeeker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      CitizenFindJobSystem.CitizenFindJobJob jobData1 = new CitizenFindJobSystem.CitizenFindJobJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
        m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
        m_WorkerType = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
        m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_HasJobSeekers = this.__TypeHandle.__Game_Agents_HasJobSeeker_RO_ComponentLookup,
        m_IsUnemployedFindJob = true,
        m_UpdateFrameIndex = updateFrame,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next(),
        m_AvailableWorkspacesByLevel = this.m_CountWorkplacesSystem.GetUnemployedWorkspaceByLevel(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<CitizenFindJobSystem.CitizenFindJobJob>(this.m_UnemployedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_EmployedQuery.IsEmpty && (double) RandomSeed.Next().GetRandom((int) this.m_SimulationSystem.frameIndex).NextFloat(1f) > (double) this.m_CitizenParametersQuery.GetSingleton<CitizenParametersData>().m_SwitchJobRate)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Agents_HasJobSeeker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        CitizenFindJobSystem.CitizenFindJobJob jobData2 = new CitizenFindJobSystem.CitizenFindJobJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CitizenType = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentTypeHandle,
          m_CurrentBuildingType = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle,
          m_WorkerType = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentTypeHandle,
          m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
          m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
          m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
          m_TouristHouseholds = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentLookup,
          m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
          m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
          m_HasJobSeekers = this.__TypeHandle.__Game_Agents_HasJobSeeker_RO_ComponentLookup,
          m_IsUnemployedFindJob = false,
          m_UpdateFrameIndex = updateFrame,
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
          m_RandomSeed = RandomSeed.Next(),
          m_AvailableWorkspacesByLevel = this.m_CountWorkplacesSystem.GetFreeWorkplaces(),
          m_SimulationFrame = this.m_SimulationSystem.frameIndex
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData2.ScheduleParallel<CitizenFindJobSystem.CitizenFindJobJob>(this.m_EmployedQuery, this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public CitizenFindJobSystem()
    {
    }

    [BurstCompile]
    private struct CitizenFindJobJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> m_CurrentBuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Worker> m_WorkerType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> m_TouristHouseholds;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAways;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<HasJobSeeker> m_HasJobSeekers;
      [ReadOnly]
      public Workplaces m_AvailableWorkspacesByLevel;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public bool m_IsUnemployedFindJob;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
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
        NativeArray<CurrentBuilding> nativeArray3 = chunk.GetNativeArray<CurrentBuilding>(ref this.m_CurrentBuildingType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          Entity household = this.m_HouseholdMembers[nativeArray1[index1]].m_Household;
          Citizen citizen = nativeArray2[index1];
          switch (citizen.GetAge())
          {
            case CitizenAge.Child:
            case CitizenAge.Elderly:
              continue;
            default:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((long) this.m_HasJobSeekers[nativeArray1[index1]].m_LastJobSeekFrameIndex + (long) random.NextInt(CitizenFindJobSystem.kJobSeekCoolDownMin, CitizenFindJobSystem.kJobSeekCoolDownMax) <= (long) this.m_SimulationFrame && !this.m_MovingAways.HasComponent(household))
              {
                int educationLevel = citizen.GetEducationLevel();
                HasJobSeeker hasJobSeeker;
                // ISSUE: reference to a compiler-generated field
                if (this.m_IsUnemployedFindJob)
                {
                  // ISSUE: reference to a compiler-generated field
                  int num = this.m_AvailableWorkspacesByLevel[educationLevel];
                  if (num <= 0 || num < random.NextInt(100))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ref EntityCommandBuffer.ParallelWriter local = ref this.m_CommandBuffer;
                    int sortKey = unfilteredChunkIndex;
                    Entity e = nativeArray1[index1];
                    hasJobSeeker = new HasJobSeeker();
                    hasJobSeeker.m_Seeker = Entity.Null;
                    // ISSUE: reference to a compiler-generated field
                    hasJobSeeker.m_LastJobSeekFrameIndex = this.m_SimulationFrame;
                    HasJobSeeker component = hasJobSeeker;
                    local.SetComponent<HasJobSeeker>(sortKey, e, component);
                    continue;
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<Worker> nativeArray4 = chunk.GetNativeArray<Worker>(ref this.m_WorkerType);
                  // ISSUE: reference to a compiler-generated field
                  int level = this.m_OutsideConnections.HasComponent(nativeArray4[index1].m_Workplace) ? 0 : (int) nativeArray4[index1].m_Level;
                  if (level < educationLevel)
                  {
                    int num = 0;
                    for (int index2 = level; index2 <= educationLevel; ++index2)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_AvailableWorkspacesByLevel[index2] > 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        num += this.m_AvailableWorkspacesByLevel[index2];
                      }
                    }
                    if (num <= 0 || num < random.NextInt(100))
                    {
                      // ISSUE: reference to a compiler-generated field
                      ref EntityCommandBuffer.ParallelWriter local = ref this.m_CommandBuffer;
                      int sortKey = unfilteredChunkIndex;
                      Entity e = nativeArray1[index1];
                      hasJobSeeker = new HasJobSeeker();
                      hasJobSeeker.m_Seeker = Entity.Null;
                      // ISSUE: reference to a compiler-generated field
                      hasJobSeeker.m_LastJobSeekFrameIndex = this.m_SimulationFrame;
                      HasJobSeeker component = hasJobSeeker;
                      local.SetComponent<HasJobSeeker>(sortKey, e, component);
                      continue;
                    }
                  }
                  else
                    continue;
                }
                Entity entity1 = Entity.Null;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_TouristHouseholds.HasComponent(household) && this.m_PropertyRenters.HasComponent(household))
                {
                  // ISSUE: reference to a compiler-generated field
                  entity1 = this.m_PropertyRenters[household].m_Property;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (chunk.Has<CurrentBuilding>(ref this.m_CurrentBuildingType) && (citizen.m_State & CitizenFlags.Commuter) != CitizenFlags.None)
                    entity1 = nativeArray3[index1].m_CurrentBuilding;
                }
                if (entity1 != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Owner>(unfilteredChunkIndex, entity2, new Owner()
                  {
                    m_Owner = nativeArray1[index1]
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<JobSeeker>(unfilteredChunkIndex, entity2, new JobSeeker()
                  {
                    m_Level = (byte) citizen.GetEducationLevel(),
                    m_Outside = (citizen.m_State & CitizenFlags.Commuter) != CitizenFlags.None ? (byte) 1 : (byte) 0
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<CurrentBuilding>(unfilteredChunkIndex, entity2, new CurrentBuilding()
                  {
                    m_CurrentBuilding = entity1
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponentEnabled<HasJobSeeker>(unfilteredChunkIndex, nativeArray1[index1], true);
                  // ISSUE: reference to a compiler-generated field
                  ref EntityCommandBuffer.ParallelWriter local = ref this.m_CommandBuffer;
                  int sortKey = unfilteredChunkIndex;
                  Entity e = nativeArray1[index1];
                  hasJobSeeker = new HasJobSeeker();
                  hasJobSeeker.m_Seeker = entity2;
                  // ISSUE: reference to a compiler-generated field
                  hasJobSeeker.m_LastJobSeekFrameIndex = this.m_SimulationFrame;
                  HasJobSeeker component = hasJobSeeker;
                  local.SetComponent<HasJobSeeker>(sortKey, e, component);
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
      public ComponentTypeHandle<Citizen> __Game_Citizens_Citizen_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Worker> __Game_Citizens_Worker_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HasJobSeeker> __Game_Agents_HasJobSeeker_RO_ComponentLookup;

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
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentLookup = state.GetComponentLookup<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_HasJobSeeker_RO_ComponentLookup = state.GetComponentLookup<HasJobSeeker>(true);
      }
    }
  }
}
