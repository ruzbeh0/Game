// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WorkProviderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Debug;
using Game.Notifications;
using Game.Objects;
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
  public class WorkProviderSystem : GameSystemBase
  {
    private const int kUpdatesPerDay = 512;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_WorkProviderGroup;
    private NativeQueue<WorkProviderSystem.LayOffReason> m_LayOffQueue;
    [DebugWatchValue]
    private NativeArray<int> m_LayOffs;
    private WorkProviderSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_543653704_0;
    private EntityQuery __query_543653704_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 32;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkProviderGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<WorkProvider>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadWrite<Employee>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<CompanyData>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LayOffQueue = new NativeQueue<WorkProviderSystem.LayOffReason>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LayOffs = new NativeArray<int>(4, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WorkProviderGroup);
      this.RequireForUpdate<WorkProviderParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
      Unity.Assertions.Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LayOffs.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LayOffQueue.Dispose();
      base.OnDestroy();
    }

    public static int CalculateTotalWage(
      DynamicBuffer<Employee> employees,
      ref EconomyParameterData econParams)
    {
      int totalWage = 0;
      for (int index = 0; index < employees.Length; ++index)
        totalWage += econParams.GetWage((int) employees[index].m_Level);
      return totalWage;
    }

    public static int CalculateTotalWage(
      int totalWorkers,
      WorkplaceComplexity complexity,
      int buildingLevel,
      EconomyParameterData econParams)
    {
      int totalWage = 0;
      // ISSUE: reference to a compiler-generated method
      Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(totalWorkers, complexity, buildingLevel);
      for (int index = 0; index < 5; ++index)
        totalWage += numberOfWorkplaces[index] * econParams.GetWage(index);
      return totalWage;
    }

    public static Workplaces CalculateNumberOfWorkplaces(
      int totalWorkers,
      WorkplaceComplexity complexity,
      int buildingLevel)
    {
      Workplaces numberOfWorkplaces = new Workplaces();
      int num1 = 4 * (int) complexity + buildingLevel - 1;
      int y = totalWorkers;
      int num2 = 0;
      for (int index = 0; index < 5; ++index)
      {
        int num3 = math.max(0, 8 - math.abs(num1 - 4 * index));
        if (index == 0)
          num3 += math.max(0, 8 - math.abs(num1 + 4));
        if (index == 4)
          num3 += math.max(0, 8 - math.abs(num1 - 20));
        int x = totalWorkers * num3 / 16;
        int num4 = totalWorkers * num3 % 16;
        if (y > x && num4 + num2 > 0)
        {
          ++x;
          num2 -= 16;
        }
        num2 += num4;
        int num5 = math.min(x, y);
        y -= num5;
        numberOfWorkplaces[index] = num5;
      }
      return numberOfWorkplaces;
    }

    public static float GetWorkforce(
      DynamicBuffer<Employee> employees,
      ComponentLookup<Citizen> citizens)
    {
      float workforce = 0.0f;
      for (int index = 0; index < employees.Length; ++index)
      {
        if (citizens.HasComponent(employees[index].m_Worker))
        {
          Employee employee = employees[index];
          Citizen citizen = citizens[employee.m_Worker];
          // ISSUE: reference to a compiler-generated method
          workforce += WorkProviderSystem.GetWorkerWorkforce(citizen.Happiness, (int) employee.m_Level);
        }
      }
      return workforce;
    }

    public static float GetAverageWorkforce(
      int maxWorkers,
      WorkplaceComplexity complexity,
      int buildingLevel)
    {
      // ISSUE: reference to a compiler-generated method
      Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(maxWorkers, complexity, buildingLevel);
      float averageWorkforce = 0.0f;
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        averageWorkforce += (float) numberOfWorkplaces[index] * WorkProviderSystem.GetWorkerWorkforce(50, index);
      }
      return averageWorkforce;
    }

    public static float GetAverageWorkforce(Workplaces workplaces)
    {
      float averageWorkforce = 0.0f;
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        averageWorkforce += (float) workplaces[index] * WorkProviderSystem.GetWorkerWorkforce(50, index);
      }
      return averageWorkforce;
    }

    public static float GetAverageWorkforce(DynamicBuffer<Employee> employees)
    {
      float averageWorkforce = 0.0f;
      foreach (Employee employee in employees)
      {
        // ISSUE: reference to a compiler-generated method
        averageWorkforce += WorkProviderSystem.GetWorkerWorkforce(50, (int) employee.m_Level);
      }
      return averageWorkforce;
    }

    public static float GetWorkerWorkforce(int happiness, int level)
    {
      return (float) (((level == 0 ? 2.0 : 1.0) + 2.5 * (double) level) * (0.75 + (double) happiness / 200.0));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 512, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WorkProviderSystem.WorkProviderTickJob jobData1 = new WorkProviderSystem.WorkProviderTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RW_ComponentTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RW_BufferTypeHandle,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_TravelPurposes = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_MovingAways = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_Efficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SchoolDatas = this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup,
        m_Attacheds = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_SubAreaBufs = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_Deleteds = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_Lots = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_Geometries = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_StudentBufs = this.__TypeHandle.__Game_Buildings_Student_RO_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_LayOffQueue = this.m_LayOffQueue.AsParallelWriter(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_WorkProviderParameterData = this.__query_543653704_0.GetSingleton<WorkProviderParameterData>(),
        m_BuildingEfficiencyParameterData = this.__query_543653704_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<WorkProviderSystem.WorkProviderTickJob>(this.m_WorkProviderGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WorkProviderSystem.LayOffCountJob jobData2 = new WorkProviderSystem.LayOffCountJob()
      {
        m_LayOffs = this.m_LayOffs,
        m_LayOffQueue = this.m_LayOffQueue
      };
      this.Dependency = jobData2.Schedule<WorkProviderSystem.LayOffCountJob>(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_543653704_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<WorkProviderParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_543653704_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
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
    public WorkProviderSystem()
    {
    }

    private enum LayOffReason
    {
      Unknown,
      MovingAway,
      TooMany,
      NoBuilding,
      Count,
    }

    [BurstCompile]
    private struct WorkProviderTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposes;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<SchoolData> m_SchoolDatas;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAways;
      [ReadOnly]
      public ComponentLookup<Attached> m_Attacheds;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreaBufs;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleteds;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_Lots;
      [ReadOnly]
      public ComponentLookup<Geometry> m_Geometries;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> m_StudentBufs;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Efficiency> m_Efficiencies;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<WorkProviderSystem.LayOffReason>.ParallelWriter m_LayOffQueue;
      public IconCommandBuffer m_IconCommandBuffer;
      public WorkProviderParameterData m_WorkProviderParameterData;
      public BuildingEfficiencyParameterData m_BuildingEfficiencyParameterData;
      public uint m_UpdateFrameIndex;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<Game.Objects.OutsideConnection>() && (int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          WorkplaceData componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_WorkplaceDatas.TryGetComponent((Entity) this.m_PrefabRefs[nativeArray1[index]], out componentData))
          {
            ref WorkProvider local = ref nativeArray2.ElementAt<WorkProvider>(index);
            Entity entity = Entity.Null;
            Entity property;
            if (chunk.Has<CompanyData>())
            {
              if (nativeArray3.Length > 0)
              {
                property = nativeArray3[index].m_Property;
                if (property == Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Liquidate(unfilteredChunkIndex, nativeArray1[index], bufferAccessor[index]);
                  continue;
                }
                if (chunk.Has<Game.Companies.ExtractorCompany>())
                {
                  // ISSUE: reference to a compiler-generated method
                  this.UpdateExtractorCompanyMaxWorkers(ref local, nativeArray1[index], property, componentData);
                }
              }
              else
                continue;
            }
            else
            {
              property = nativeArray1[index];
              if (chunk.Has<Game.Objects.OutsideConnection>())
              {
                Workplaces workplaces = new Workplaces()
                {
                  m_Uneducated = 0,
                  m_PoorlyEducated = 0,
                  m_Educated = 200,
                  m_WellEducated = 200,
                  m_HighlyEducated = 200
                };
                local.m_MaxWorkers = workplaces.TotalCount;
              }
              else if (chunk.Has<Game.Buildings.School>())
              {
                // ISSUE: reference to a compiler-generated method
                this.UpdateSchoolMaxWorkers(ref local, nativeArray1[index]);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int buildingLevel = PropertyUtils.GetBuildingLevel((Entity) this.m_PrefabRefs[property], this.m_SpawnableBuildingDatas);
            // ISSUE: reference to a compiler-generated method
            Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(local.m_MaxWorkers, componentData.m_Complexity, buildingLevel);
            Workplaces freeWorkplaces = numberOfWorkplaces;
            // ISSUE: reference to a compiler-generated method
            this.RefreshFreeWorkplace(unfilteredChunkIndex, nativeArray1[index], bufferAccessor[index], ref freeWorkplaces);
            if (!chunk.Has<Game.Objects.OutsideConnection>())
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateNotificationAndEfficiency(property, ref local, bufferAccessor[index], numberOfWorkplaces, freeWorkplaces);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.Liquidate(unfilteredChunkIndex, nativeArray1[index], bufferAccessor[index]);
          }
        }
      }

      private void UpdateSchoolMaxWorkers(ref WorkProvider workProvider, Entity schoolEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int workplaceMaxWorkers = CityUtils.GetCityServiceWorkplaceMaxWorkers(schoolEntity, ref this.m_PrefabRefs, ref this.m_InstalledUpgrades, ref this.m_Deleteds, ref this.m_WorkplaceDatas, ref this.m_SchoolDatas, ref this.m_StudentBufs);
        workProvider.m_MaxWorkers = workplaceMaxWorkers;
      }

      private void UpdateExtractorCompanyMaxWorkers(
        ref WorkProvider workProvider,
        Entity companyEntity,
        Entity buildingEntity,
        WorkplaceData workplaceData)
      {
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefs[companyEntity].m_Prefab;
        float area = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Attacheds.HasComponent(buildingEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          area = ExtractorAISystem.GetArea(this.m_SubAreaBufs[this.m_Attacheds[buildingEntity].m_Parent], this.m_Lots, this.m_Geometries);
        }
        // ISSUE: reference to a compiler-generated field
        workProvider.m_MaxWorkers = math.max(CompanyUtils.GetExtractorFittingWorkers(area, 1f, this.m_IndustrialProcessDatas[prefab]) / 2, workplaceData.m_MinimumWorkersLimit);
      }

      private void RefreshFreeWorkplace(
        int sortKey,
        Entity workplaceEntity,
        DynamicBuffer<Employee> employeeBuf,
        ref Workplaces freeWorkplaces)
      {
        for (int index = 0; index < employeeBuf.Length; ++index)
        {
          Employee employee = employeeBuf[index];
          Worker componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Citizens.HasComponent(employee.m_Worker) || CitizenUtils.IsDead(employee.m_Worker, ref this.m_HealthProblems) || !this.m_Workers.TryGetComponent(employee.m_Worker, out componentData) || componentData.m_Workplace != workplaceEntity || this.m_MovingAways.HasComponent(this.m_HouseholdMembers[employee.m_Worker].m_Household))
          {
            employeeBuf.RemoveAtSwapBack(index);
            --index;
            // ISSUE: reference to a compiler-generated field
            this.m_LayOffQueue.Enqueue(WorkProviderSystem.LayOffReason.MovingAway);
          }
          else if (freeWorkplaces[(int) employee.m_Level] <= 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveWorker(sortKey, employee.m_Worker);
            employeeBuf.RemoveAtSwapBack(index);
            --index;
            // ISSUE: reference to a compiler-generated field
            this.m_LayOffQueue.Enqueue(WorkProviderSystem.LayOffReason.TooMany);
          }
          else
            freeWorkplaces[(int) employee.m_Level]--;
        }
        if (freeWorkplaces.TotalCount > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<FreeWorkplaces>(sortKey, workplaceEntity, new FreeWorkplaces(freeWorkplaces));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<FreeWorkplaces>(sortKey, workplaceEntity);
        }
      }

      private void UpdateNotificationAndEfficiency(
        Entity buildingEntity,
        ref WorkProvider workProvider,
        DynamicBuffer<Employee> employees,
        Workplaces maxWorkplaces,
        Workplaces freeWorkplaces)
      {
        int num1 = maxWorkplaces.m_Uneducated + maxWorkplaces.m_PoorlyEducated;
        int num2 = freeWorkplaces.m_Uneducated + freeWorkplaces.m_PoorlyEducated;
        // ISSUE: reference to a compiler-generated field
        bool enabled1 = num1 > 0 && (double) num2 / (double) num1 >= (double) this.m_WorkProviderParameterData.m_UneducatedNotificationLimit;
        // ISSUE: reference to a compiler-generated method
        this.UpdateCooldown(ref workProvider.m_UneducatedCooldown, enabled1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateNotification(buildingEntity, this.m_WorkProviderParameterData.m_UneducatedNotificationPrefab, (int) workProvider.m_UneducatedCooldown >= (int) this.m_WorkProviderParameterData.m_UneducatedNotificationDelay, ref workProvider.m_UneducatedNotificationEntity);
        int num3 = maxWorkplaces.m_Educated + 2 * maxWorkplaces.m_WellEducated + 2 * maxWorkplaces.m_HighlyEducated;
        int num4 = freeWorkplaces.m_Educated + 2 * freeWorkplaces.m_WellEducated + 2 * freeWorkplaces.m_HighlyEducated;
        // ISSUE: reference to a compiler-generated field
        bool enabled2 = num3 > 0 && (double) (num4 / num3) >= (double) this.m_WorkProviderParameterData.m_EducatedNotificationLimit;
        // ISSUE: reference to a compiler-generated method
        this.UpdateCooldown(ref workProvider.m_EducatedCooldown, enabled2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateNotification(buildingEntity, this.m_WorkProviderParameterData.m_EducatedNotificationPrefab, (int) workProvider.m_EducatedCooldown >= (int) this.m_WorkProviderParameterData.m_EducatedNotificationDelay, ref workProvider.m_EducatedNotificationEntity);
        DynamicBuffer<Efficiency> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Efficiencies.TryGetBuffer(buildingEntity, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated method
        float averageWorkforce1 = WorkProviderSystem.GetAverageWorkforce(maxWorkplaces);
        float efficiency1;
        float efficiency2;
        float efficiency3;
        if ((double) averageWorkforce1 > 0.0)
        {
          float currentWorkforce;
          float averageWorkforce2;
          float sickWorkforce;
          // ISSUE: reference to a compiler-generated method
          this.CalculateCurrentWorkforce(employees, maxWorkplaces.TotalCount, out currentWorkforce, out averageWorkforce2, out sickWorkforce);
          float num5 = averageWorkforce1 - averageWorkforce2 - sickWorkforce;
          // ISSUE: reference to a compiler-generated method
          this.UpdateCooldown(ref workProvider.m_EfficiencyCooldown, (double) num5 > 1.0 / 1000.0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float x = num5 * math.saturate((float) workProvider.m_EfficiencyCooldown / this.m_BuildingEfficiencyParameterData.m_MissingEmployeesEfficiencyDelay) * this.m_BuildingEfficiencyParameterData.m_MissingEmployeesEfficiencyPenalty;
          // ISSUE: reference to a compiler-generated field
          float y = sickWorkforce * this.m_BuildingEfficiencyParameterData.m_SickEmployeesEfficiencyPenalty;
          float2 float2 = BuildingUtils.ApproximateEfficiencyFactors((averageWorkforce1 - x - y) / averageWorkforce1, new float2(x, y));
          efficiency1 = float2.x;
          efficiency2 = float2.y;
          efficiency3 = (double) averageWorkforce2 > 0.0 ? currentWorkforce / averageWorkforce2 : 1f;
        }
        else
        {
          workProvider.m_EfficiencyCooldown = (short) 0;
          efficiency1 = 1f;
          efficiency2 = 1f;
          efficiency3 = 1f;
        }
        BuildingUtils.SetEfficiencyFactor(bufferData, EfficiencyFactor.NotEnoughEmployees, efficiency1);
        BuildingUtils.SetEfficiencyFactor(bufferData, EfficiencyFactor.SickEmployees, efficiency2);
        BuildingUtils.SetEfficiencyFactor(bufferData, EfficiencyFactor.EmployeeHappiness, efficiency3);
      }

      private void UpdateCooldown(ref short cooldown, bool enabled)
      {
        if (!enabled)
        {
          if (cooldown <= (short) 0)
            return;
          cooldown = (short) 0;
        }
        else
        {
          if (cooldown >= short.MaxValue)
            return;
          ++cooldown;
        }
      }

      private void UpdateNotification(
        Entity building,
        Entity notificationPrefab,
        bool enabled,
        ref Entity currentTarget)
      {
        if (currentTarget != Entity.Null && (!enabled || currentTarget != building))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(currentTarget, notificationPrefab);
          currentTarget = Entity.Null;
        }
        if (!enabled || !(currentTarget == Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_IconCommandBuffer.Add(building, notificationPrefab, IconPriority.Problem);
        currentTarget = building;
      }

      private void CalculateCurrentWorkforce(
        DynamicBuffer<Employee> employees,
        int maxCount,
        out float currentWorkforce,
        out float averageWorkforce,
        out float sickWorkforce)
      {
        currentWorkforce = 0.0f;
        averageWorkforce = 0.0f;
        sickWorkforce = 0.0f;
        int num = math.min(employees.Length, maxCount);
        for (int index = 0; index < num; ++index)
        {
          Employee employee = employees[index];
          // ISSUE: reference to a compiler-generated field
          Citizen citizen = this.m_Citizens[employee.m_Worker];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_HealthProblems.HasComponent(employee.m_Worker))
          {
            // ISSUE: reference to a compiler-generated method
            currentWorkforce += WorkProviderSystem.GetWorkerWorkforce(citizen.Happiness, (int) employee.m_Level);
            // ISSUE: reference to a compiler-generated method
            averageWorkforce += WorkProviderSystem.GetWorkerWorkforce(50, (int) employee.m_Level);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            sickWorkforce += WorkProviderSystem.GetWorkerWorkforce(50, (int) employee.m_Level);
          }
        }
      }

      private void Liquidate(int sortKey, Entity provider, DynamicBuffer<Employee> employees)
      {
        for (int index = 0; index < employees.Length; ++index)
        {
          Entity worker = employees[index].m_Worker;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Workers.HasComponent(worker))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LayOffQueue.Enqueue(WorkProviderSystem.LayOffReason.NoBuilding);
            // ISSUE: reference to a compiler-generated method
            this.RemoveWorker(sortKey, worker);
          }
        }
        employees.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<FreeWorkplaces>(sortKey, provider);
      }

      private void RemoveWorker(int sortKey, Entity worker)
      {
        TravelPurpose componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TravelPurposes.TryGetComponent(worker, out componentData))
        {
          switch (componentData.m_Purpose)
          {
            case Game.Citizens.Purpose.GoingToWork:
            case Game.Citizens.Purpose.Working:
            case Game.Citizens.Purpose.Studying:
            case Game.Citizens.Purpose.GoingToSchool:
              switch (componentData.m_Purpose)
              {
                case Game.Citizens.Purpose.Studying:
                case Game.Citizens.Purpose.GoingToSchool:
                  UnityEngine.Debug.LogWarning((object) string.Format("Worker {0} had incorrect TravelPurpose {1}!", (object) worker.Index, (object) (int) componentData.m_Purpose));
                  break;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TravelPurpose>(sortKey, worker);
              break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Worker>(sortKey, worker);
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
    private struct LayOffCountJob : IJob
    {
      public NativeQueue<WorkProviderSystem.LayOffReason> m_LayOffQueue;
      public NativeArray<int> m_LayOffs;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        WorkProviderSystem.LayOffReason index;
        // ISSUE: reference to a compiler-generated field
        while (this.m_LayOffQueue.TryDequeue(out index))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LayOffs[(int) index]++;
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RW_ComponentTypeHandle;
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SchoolData> __Game_Prefabs_SchoolData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Buildings.Student> __Game_Buildings_Student_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RW_BufferTypeHandle = state.GetBufferTypeHandle<Employee>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferLookup = state.GetBufferLookup<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SchoolData_RO_ComponentLookup = state.GetComponentLookup<SchoolData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferLookup = state.GetBufferLookup<Game.Buildings.Student>(true);
      }
    }
  }
}
