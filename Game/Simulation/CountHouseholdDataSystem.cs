// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CountHouseholdDataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Debug;
using Game.Economy;
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
  public class CountHouseholdDataSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private ResourceSystem m_ResourceSystem;
    private CitySystem m_CitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_HouseholdQuery;
    private EntityQuery m_RequirementQuery;
    private EntityArchetype m_UnlockEventArchetype;
    [DebugWatchDeps]
    private JobHandle m_HouseholdDataWriteDependencies;
    private JobHandle m_HouseholdDataReadDependencies;
    private NativeAccumulator<CountHouseholdDataSystem.HouseholdData> m_HouseholdCountData;
    private NativeAccumulator<CountHouseholdDataSystem.HouseholdNeedData> m_HouseholdNeedCountData;
    private CountHouseholdDataSystem.HouseholdData m_LastHouseholdCountData;
    [ResourceArray]
    [DebugWatchValue]
    private NativeArray<int> m_ResourceNeed;
    private bool m_NeedForceCountData;
    [DebugWatchValue]
    private NativeArray<int> m_EmployableByEducation;
    private CountHouseholdDataSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public CountHouseholdDataSystem.HouseholdData GetHouseholdCountData()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_LastHouseholdCountData;
    }

    [DebugWatchValue]
    public int MovingInHouseholdCount => this.m_LastHouseholdCountData.m_MovingInHouseholdCount;

    [DebugWatchValue]
    public int MovingInCitizenCount => this.m_LastHouseholdCountData.m_MovingInCitizenCount;

    [DebugWatchValue]
    public int MovingAwayHouseholdCount => this.m_LastHouseholdCountData.m_MovingAwayHouseholdCount;

    [DebugWatchValue]
    public int CommuterHouseholdCount => this.m_LastHouseholdCountData.m_CommuterHouseholdCount;

    [DebugWatchValue]
    public int TouristCitizenCount => this.m_LastHouseholdCountData.m_TouristCitizenCount;

    [DebugWatchValue]
    public int HomelessHouseholdCount => this.m_LastHouseholdCountData.m_HomelessHouseholdCount;

    [DebugWatchValue]
    public int HomelessCitizenCount => this.m_LastHouseholdCountData.m_HomelessCitizenCount;

    [DebugWatchValue]
    public int MovedInHouseholdCount => this.m_LastHouseholdCountData.m_MovedInHouseholdCount;

    [DebugWatchValue]
    public int MovedInCitizenCount => this.m_LastHouseholdCountData.m_MovedInCitizenCount;

    [DebugWatchValue]
    public int ChildrenCount => this.m_LastHouseholdCountData.m_ChildrenCount;

    [DebugWatchValue]
    public int AdultCount => this.m_LastHouseholdCountData.m_AdultCount;

    [DebugWatchValue]
    public int TeenCount => this.m_LastHouseholdCountData.m_TeenCount;

    [DebugWatchValue]
    public int SeniorCount => this.m_LastHouseholdCountData.m_SeniorCount;

    [DebugWatchValue]
    public int StudentCount => this.m_LastHouseholdCountData.m_StudentCount;

    [DebugWatchValue]
    public int UneducatedCount => this.m_LastHouseholdCountData.m_UneducatedCount;

    [DebugWatchValue]
    public int PoorlyEducatedCount => this.m_LastHouseholdCountData.m_PoorlyEducatedCount;

    [DebugWatchValue]
    public int EducatedCount => this.m_LastHouseholdCountData.m_EducatedCount;

    [DebugWatchValue]
    public int WellEducatedCount => this.m_LastHouseholdCountData.m_WellEducatedCount;

    [DebugWatchValue]
    public int HighlyEducatedCount => this.m_LastHouseholdCountData.m_HighlyEducatedCount;

    [DebugWatchValue]
    public int WorkableCitizenCount => this.m_LastHouseholdCountData.m_WorkableCitizenCount;

    [DebugWatchValue]
    public int CityWorkerCount => this.m_LastHouseholdCountData.m_CityWorkerCount;

    [DebugWatchValue]
    public int DeadCitizenCount => this.m_LastHouseholdCountData.m_DeadCitizenCount;

    [DebugWatchValue]
    public int AverageCitizenHappiness
    {
      get
      {
        return this.m_LastHouseholdCountData.m_MovedInCitizenCount == 0 ? 0 : (int) (this.m_LastHouseholdCountData.m_TotalMovedInCitizenHappiness / (long) this.m_LastHouseholdCountData.m_MovedInCitizenCount);
      }
    }

    [DebugWatchValue]
    public int AverageCitizenHealth
    {
      get
      {
        return this.m_LastHouseholdCountData.m_MovedInCitizenCount == 0 ? 0 : (int) (this.m_LastHouseholdCountData.m_TotalMovedInCitizenHealth / (long) this.m_LastHouseholdCountData.m_MovedInCitizenCount);
      }
    }

    [DebugWatchValue]
    public float UnemploymentRate
    {
      get
      {
        return this.m_LastHouseholdCountData.m_WorkableCitizenCount == 0 ? 0.0f : 100f * (float) math.max(0, this.m_LastHouseholdCountData.m_WorkableCitizenCount - this.m_LastHouseholdCountData.m_CityWorkerCount) / (float) this.m_LastHouseholdCountData.m_WorkableCitizenCount;
      }
    }

    public NativeArray<int> GetResourceNeeds() => this.m_ResourceNeed;

    public bool IsCountDataNotReady() => this.m_NeedForceCountData;

    public NativeArray<int> GetEmployables() => this.m_EmployableByEducation;

    public void AddHouseholdDataReader(JobHandle reader)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdDataReadDependencies = JobHandle.CombineDependencies(this.m_HouseholdDataReadDependencies, reader);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Household>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RequirementQuery = this.GetEntityQuery(ComponentType.ReadOnly<CitizenRequirementData>(), ComponentType.ReadWrite<UnlockRequirementData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCountData = new NativeAccumulator<CountHouseholdDataSystem.HouseholdData>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdNeedCountData = new NativeAccumulator<CountHouseholdDataSystem.HouseholdNeedData>(EconomyUtils.ResourceCount, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceNeed = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_EmployableByEducation = new NativeArray<int>(5, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HouseholdQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCountData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdNeedCountData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceNeed.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_EmployableByEducation.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write<CountHouseholdDataSystem.HouseholdData>(this.m_LastHouseholdCountData);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ResourceNeed);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_EmployableByEducation);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.economyFix)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read<CountHouseholdDataSystem.HouseholdData>(out this.m_LastHouseholdCountData);
      }
      if (reader.context.version >= Version.countHouseholdDataFix)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(this.m_ResourceNeed);
        // ISSUE: reference to a compiler-generated field
        reader.Read(this.m_EmployableByEducation);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NeedForceCountData = true;
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_LastHouseholdCountData = new CountHouseholdDataSystem.HouseholdData();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCountData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdNeedCountData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceNeed.Fill<int>(0);
      // ISSUE: reference to a compiler-generated field
      this.m_EmployableByEducation.Fill<int>(0);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastHouseholdCountData = this.m_HouseholdCountData.GetResult();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdCountData.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdNeedCountData.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Student_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdNeed_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      CountHouseholdDataSystem.CountHouseholdJob jobData1 = new CountHouseholdDataSystem.CountHouseholdJob()
      {
        m_HouseholdType = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentTypeHandle,
        m_HouseholdNeedType = this.__TypeHandle.__Game_Citizens_HouseholdNeed_RW_ComponentTypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RW_ComponentTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_HouseholdCitizenType = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle,
        m_HealthProblems = this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentLookup,
        m_Parks = this.__TypeHandle.__Game_Buildings_Park_RW_ComponentLookup,
        m_Abandoneds = this.__TypeHandle.__Game_Buildings_Abandoned_RW_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup,
        m_Students = this.__TypeHandle.__Game_Citizens_Student_RW_ComponentLookup,
        m_Workers = this.__TypeHandle.__Game_Citizens_Worker_RW_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RW_ComponentLookup,
        m_HouseholdCountData = this.m_HouseholdCountData.AsParallelWriter(),
        m_HouseholdNeedCountData = this.m_HouseholdNeedCountData.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<CountHouseholdDataSystem.CountHouseholdJob>(this.m_HouseholdQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CountHouseholdDataSystem.ResultJob jobData2 = new CountHouseholdDataSystem.ResultJob()
      {
        m_HouseholdData = this.m_HouseholdCountData,
        m_HouseholdNeedData = this.m_HouseholdNeedCountData,
        m_Populations = this.__TypeHandle.__Game_City_Population_RW_ComponentLookup,
        m_City = this.m_CitySystem.City,
        m_ResourceNeed = this.m_ResourceNeed,
        m_EmployableByEducation = this.m_EmployableByEducation
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData2.Schedule<CountHouseholdDataSystem.ResultJob>(JobHandle.CombineDependencies(this.Dependency, this.m_HouseholdDataReadDependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdDataWriteDependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      if (this.m_NeedForceCountData)
      {
        this.Dependency.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_NeedForceCountData = false;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Population_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CitizenRequirementData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CountHouseholdDataSystem.CitizenRequirementJob jobData3 = new CountHouseholdDataSystem.CitizenRequirementJob()
      {
        m_UnlockEventArchetype = this.m_UnlockEventArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CitizenRequirementType = this.__TypeHandle.__Game_Prefabs_CitizenRequirementData_RO_ComponentTypeHandle,
        m_UnlockRequirementType = this.__TypeHandle.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle,
        m_Populations = this.__TypeHandle.__Game_City_Population_RO_ComponentLookup,
        m_City = this.m_CitySystem.City
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData3.ScheduleParallel<CountHouseholdDataSystem.CitizenRequirementJob>(this.m_RequirementQuery, this.Dependency);
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
    public CountHouseholdDataSystem()
    {
    }

    public struct HouseholdNeedData : IAccumulable<CountHouseholdDataSystem.HouseholdNeedData>
    {
      public int m_HouseholdNeed;

      public void Accumulate(CountHouseholdDataSystem.HouseholdNeedData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdNeed += other.m_HouseholdNeed;
      }
    }

    public struct HouseholdData : IAccumulable<CountHouseholdDataSystem.HouseholdData>, ISerializable
    {
      public int m_MovingInHouseholdCount;
      public int m_MovingInCitizenCount;
      public int m_MovingAwayHouseholdCount;
      public int m_CommuterHouseholdCount;
      public int m_TouristCitizenCount;
      public int m_HomelessHouseholdCount;
      public int m_HomelessCitizenCount;
      public int m_MovedInHouseholdCount;
      public int m_MovedInCitizenCount;
      public int m_ChildrenCount;
      public int m_TeenCount;
      public int m_AdultCount;
      public int m_SeniorCount;
      public int m_StudentCount;
      public int m_UneducatedCount;
      public int m_PoorlyEducatedCount;
      public int m_EducatedCount;
      public int m_WellEducatedCount;
      public int m_HighlyEducatedCount;
      public int m_WorkableCitizenCount;
      public int m_CityWorkerCount;
      public int m_DeadCitizenCount;
      public long m_TotalMovedInCitizenHappiness;
      public long m_TotalMovedInCitizenWellbeing;
      public long m_TotalMovedInCitizenHealth;
      public int m_EmployableByEducation0;
      public int m_EmployableByEducation1;
      public int m_EmployableByEducation2;
      public int m_EmployableByEducation3;
      public int m_EmployableByEducation4;

      public void Accumulate(CountHouseholdDataSystem.HouseholdData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovingInHouseholdCount += other.m_MovingInHouseholdCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovingInCitizenCount += other.m_MovingInCitizenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovingAwayHouseholdCount += other.m_MovingAwayHouseholdCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommuterHouseholdCount += other.m_CommuterHouseholdCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TouristCitizenCount += other.m_TouristCitizenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_HomelessHouseholdCount += other.m_HomelessHouseholdCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_HomelessCitizenCount += other.m_HomelessCitizenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovedInHouseholdCount += other.m_MovedInHouseholdCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovedInCitizenCount += other.m_MovedInCitizenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ChildrenCount += other.m_ChildrenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TeenCount += other.m_TeenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AdultCount += other.m_AdultCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SeniorCount += other.m_SeniorCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StudentCount += other.m_StudentCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UneducatedCount += other.m_UneducatedCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PoorlyEducatedCount += other.m_PoorlyEducatedCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EducatedCount += other.m_EducatedCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WellEducatedCount += other.m_WellEducatedCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_HighlyEducatedCount += other.m_HighlyEducatedCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_WorkableCitizenCount += other.m_WorkableCitizenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CityWorkerCount += other.m_CityWorkerCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_DeadCitizenCount += other.m_DeadCitizenCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TotalMovedInCitizenHappiness += other.m_TotalMovedInCitizenHappiness;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TotalMovedInCitizenWellbeing += other.m_TotalMovedInCitizenWellbeing;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TotalMovedInCitizenHealth += other.m_TotalMovedInCitizenHealth;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation0 += other.m_EmployableByEducation0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation1 += other.m_EmployableByEducation1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation2 += other.m_EmployableByEducation2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation3 += other.m_EmployableByEducation3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation4 += other.m_EmployableByEducation4;
      }

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_MovingInHouseholdCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_MovingInCitizenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_MovingAwayHouseholdCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CommuterHouseholdCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_TouristCitizenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_HomelessHouseholdCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_HomelessCitizenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_MovedInHouseholdCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_MovedInCitizenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_ChildrenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_TeenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_AdultCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_SeniorCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_StudentCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_UneducatedCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_PoorlyEducatedCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_EducatedCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_WellEducatedCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_HighlyEducatedCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_WorkableCitizenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CityWorkerCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_DeadCitizenCount);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_TotalMovedInCitizenHealth);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_TotalMovedInCitizenHappiness);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_TotalMovedInCitizenWellbeing);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_EmployableByEducation0);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_EmployableByEducation1);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_EmployableByEducation2);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_EmployableByEducation3);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_EmployableByEducation4);
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        if (reader.context.version < Version.statisticUnifying)
        {
          int num;
          reader.Read(out num);
          reader.Read(out num);
          reader.Read(out num);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_MovingInHouseholdCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_MovingInCitizenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_MovingAwayHouseholdCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_CommuterHouseholdCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_TouristCitizenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_HomelessHouseholdCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_HomelessCitizenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_MovedInHouseholdCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_MovedInCitizenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_ChildrenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_TeenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_AdultCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_SeniorCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_StudentCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_UneducatedCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_PoorlyEducatedCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_EducatedCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_WellEducatedCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_HighlyEducatedCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_WorkableCitizenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_CityWorkerCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_DeadCitizenCount);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_TotalMovedInCitizenHealth);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_TotalMovedInCitizenHappiness);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_TotalMovedInCitizenWellbeing);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_EmployableByEducation0);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_EmployableByEducation1);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_EmployableByEducation2);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_EmployableByEducation3);
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_EmployableByEducation4);
        }
      }

      public int Population() => this.m_MovedInCitizenCount;

      public int Unemployed() => this.m_WorkableCitizenCount - this.m_CityWorkerCount;
    }

    [BurstCompile]
    private struct CountHouseholdJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Household> m_HouseholdType;
      [ReadOnly]
      public ComponentTypeHandle<HouseholdNeed> m_HouseholdNeedType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourcesType;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenType;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblems;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_Parks;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_Abandoneds;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      [ReadOnly]
      public ComponentLookup<Game.Citizens.Student> m_Students;
      [ReadOnly]
      public ComponentLookup<Worker> m_Workers;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      public NativeAccumulator<CountHouseholdDataSystem.HouseholdData>.ParallelWriter m_HouseholdCountData;
      public NativeAccumulator<CountHouseholdDataSystem.HouseholdNeedData>.ParallelWriter m_HouseholdNeedCountData;
      [ReadOnly]
      public CitizenHappinessParameterData m_CitizenHappinessParameterData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CountHouseholdDataSystem.HouseholdData householdData = new CountHouseholdDataSystem.HouseholdData();
        bool flag1 = chunk.Has<TouristHousehold>();
        bool flag2 = chunk.Has<CommuterHousehold>();
        if (chunk.Has<MovingAway>() && !flag1 && !flag2)
        {
          // ISSUE: reference to a compiler-generated field
          householdData.m_MovingAwayHouseholdCount += chunk.Count;
          // ISSUE: reference to a compiler-generated field
          this.m_HouseholdCountData.Accumulate(householdData);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Household> nativeArray1 = chunk.GetNativeArray<Household>(ref this.m_HouseholdType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<HouseholdNeed> nativeArray2 = chunk.GetNativeArray<HouseholdNeed>(ref this.m_HouseholdNeedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
          // ISSUE: reference to a compiler-generated field
          chunk.GetBufferAccessor<Resources>(ref this.m_ResourcesType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<HouseholdCitizen> bufferAccessor = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            householdData.m_CommuterHouseholdCount += chunk.Count;
          }
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            bool flag3 = true;
            bool flag4 = !(flag2 | flag1);
            Entity propertyEntity = nativeArray3.Length != 0 ? nativeArray3[index1].m_Property : Entity.Null;
            if ((nativeArray1[index1].m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None && propertyEntity == Entity.Null && chunk.Has<PropertySeeker>())
            {
              // ISSUE: reference to a compiler-generated field
              ++householdData.m_MovingInHouseholdCount;
              // ISSUE: reference to a compiler-generated field
              householdData.m_MovingInCitizenCount += bufferAccessor[index1].Length;
              flag3 = false;
              flag4 = false;
            }
            if (flag3 && nativeArray2[index1].m_Resource != Resource.NoResource)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_HouseholdNeedCountData.Accumulate(EconomyUtils.GetResourceIndex(nativeArray2[index1].m_Resource), new CountHouseholdDataSystem.HouseholdNeedData()
              {
                m_HouseholdNeed = nativeArray2[index1].m_Amount
              });
            }
            if (flag1 && chunk.Has<Target>())
            {
              // ISSUE: reference to a compiler-generated field
              householdData.m_TouristCitizenCount += bufferAccessor[index1].Length;
            }
            if (flag4)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bool flag5 = BuildingUtils.IsHomelessHousehold(nativeArray1[index1], propertyEntity, ref this.m_Parks, ref this.m_Abandoneds);
              // ISSUE: reference to a compiler-generated field
              ++householdData.m_MovedInHouseholdCount;
              if (flag5)
              {
                // ISSUE: reference to a compiler-generated field
                ++householdData.m_HomelessHouseholdCount;
              }
              DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                if (CitizenUtils.IsDead(dynamicBuffer[index2].m_Citizen, ref this.m_HealthProblems))
                {
                  // ISSUE: reference to a compiler-generated field
                  ++householdData.m_DeadCitizenCount;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  Citizen citizen = this.m_Citizens[dynamicBuffer[index2].m_Citizen];
                  switch (citizen.GetAge())
                  {
                    case CitizenAge.Child:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_ChildrenCount;
                      break;
                    case CitizenAge.Teen:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_TeenCount;
                      break;
                    case CitizenAge.Adult:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_AdultCount;
                      break;
                    case CitizenAge.Elderly:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_SeniorCount;
                      break;
                  }
                  int educationLevel = citizen.GetEducationLevel();
                  switch (educationLevel)
                  {
                    case 0:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_UneducatedCount;
                      break;
                    case 1:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_PoorlyEducatedCount;
                      break;
                    case 2:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_EducatedCount;
                      break;
                    case 3:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_WellEducatedCount;
                      break;
                    case 4:
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_HighlyEducatedCount;
                      break;
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Students.HasComponent(dynamicBuffer[index2].m_Citizen))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ++householdData.m_StudentCount;
                  }
                  bool flag6 = false;
                  bool flag7 = false;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Workers.HasComponent(dynamicBuffer[index2].m_Citizen))
                  {
                    flag6 = true;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    flag7 = this.m_OutsideConnections.HasComponent(this.m_Workers[dynamicBuffer[index2].m_Citizen].m_Workplace);
                    if (!flag7)
                    {
                      // ISSUE: reference to a compiler-generated field
                      ++householdData.m_CityWorkerCount;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (CitizenUtils.IsWorkableCitizen(dynamicBuffer[index2].m_Citizen, ref this.m_Citizens, ref this.m_Students, ref this.m_HealthProblems))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ++householdData.m_WorkableCitizenCount;
                    // ISSUE: reference to a compiler-generated field
                    if (!flag6 | flag7 || (int) this.m_Workers[dynamicBuffer[index2].m_Citizen].m_Level < educationLevel)
                    {
                      switch (educationLevel)
                      {
                        case 0:
                          // ISSUE: reference to a compiler-generated field
                          ++householdData.m_EmployableByEducation0;
                          break;
                        case 1:
                          // ISSUE: reference to a compiler-generated field
                          ++householdData.m_EmployableByEducation1;
                          break;
                        case 2:
                          // ISSUE: reference to a compiler-generated field
                          ++householdData.m_EmployableByEducation2;
                          break;
                        case 3:
                          // ISSUE: reference to a compiler-generated field
                          ++householdData.m_EmployableByEducation3;
                          break;
                        case 4:
                          // ISSUE: reference to a compiler-generated field
                          ++householdData.m_EmployableByEducation4;
                          break;
                      }
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  householdData.m_TotalMovedInCitizenHappiness += (long) citizen.Happiness;
                  // ISSUE: reference to a compiler-generated field
                  householdData.m_TotalMovedInCitizenWellbeing += (long) citizen.m_WellBeing;
                  // ISSUE: reference to a compiler-generated field
                  householdData.m_TotalMovedInCitizenHealth += (long) citizen.m_Health;
                  // ISSUE: reference to a compiler-generated field
                  ++householdData.m_MovedInCitizenCount;
                  if (flag5)
                  {
                    // ISSUE: reference to a compiler-generated field
                    ++householdData.m_HomelessCitizenCount;
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_HouseholdCountData.Accumulate(householdData);
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
    private struct ResultJob : IJob
    {
      [ReadOnly]
      public NativeAccumulator<CountHouseholdDataSystem.HouseholdData> m_HouseholdData;
      [ReadOnly]
      public NativeAccumulator<CountHouseholdDataSystem.HouseholdNeedData> m_HouseholdNeedData;
      public NativeArray<int> m_ResourceNeed;
      public NativeArray<int> m_EmployableByEducation;
      public Entity m_City;
      public ComponentLookup<Population> m_Populations;

      public void Execute()
      {
        for (int index = 0; index < EconomyUtils.ResourceCount; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceNeed[index] = this.m_HouseholdNeedData.GetResult(index).m_HouseholdNeed;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CountHouseholdDataSystem.HouseholdData result = this.m_HouseholdData.GetResult();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation[0] = result.m_EmployableByEducation0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation[1] = result.m_EmployableByEducation1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation[2] = result.m_EmployableByEducation2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation[3] = result.m_EmployableByEducation3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EmployableByEducation[4] = result.m_EmployableByEducation4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Population population = this.m_Populations[this.m_City] with
        {
          m_Population = result.Population(),
          m_PopulationWithMoveIn = result.m_MovedInCitizenCount + result.m_MovingInCitizenCount
        };
        if (population.m_Population > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          population.m_AverageHappiness = (int) (result.m_TotalMovedInCitizenHappiness / (long) result.m_MovedInCitizenCount);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          population.m_AverageHealth = (int) (result.m_TotalMovedInCitizenHealth / (long) result.m_MovedInCitizenCount);
        }
        else
        {
          population.m_AverageHappiness = 50;
          population.m_AverageHealth = 50;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Populations[this.m_City] = population;
      }
    }

    [BurstCompile]
    private struct CitizenRequirementJob : IJobChunk
    {
      [ReadOnly]
      public EntityArchetype m_UnlockEventArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CitizenRequirementData> m_CitizenRequirementType;
      public ComponentTypeHandle<UnlockRequirementData> m_UnlockRequirementType;
      [ReadOnly]
      public ComponentLookup<Population> m_Populations;
      public Entity m_City;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CitizenRequirementData> nativeArray2 = chunk.GetNativeArray<CitizenRequirementData>(ref this.m_CitizenRequirementType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnlockRequirementData> nativeArray3 = chunk.GetNativeArray<UnlockRequirementData>(ref this.m_UnlockRequirementType);
        ChunkEntityEnumerator entityEnumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        int nextIndex;
        while (entityEnumerator.NextEntityIndex(out nextIndex))
        {
          CitizenRequirementData citizenRequirement = nativeArray2[nextIndex];
          UnlockRequirementData unlockRequirement = nativeArray3[nextIndex];
          // ISSUE: reference to a compiler-generated method
          if (this.ShouldUnlock(citizenRequirement, ref unlockRequirement))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_UnlockEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Unlock>(unfilteredChunkIndex, entity, new Unlock(nativeArray1[nextIndex]));
          }
          nativeArray3[nextIndex] = unlockRequirement;
        }
      }

      private bool ShouldUnlock(
        CitizenRequirementData citizenRequirement,
        ref UnlockRequirementData unlockRequirement)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Population population = this.m_Populations[this.m_City];
        unlockRequirement.m_Progress = population.m_Population < citizenRequirement.m_MinimumPopulation || citizenRequirement.m_MinimumHappiness == 0 ? math.min(population.m_Population, citizenRequirement.m_MinimumPopulation) : math.min(population.m_AverageHappiness, citizenRequirement.m_MinimumHappiness);
        return population.m_Population >= citizenRequirement.m_MinimumPopulation && population.m_AverageHappiness >= citizenRequirement.m_MinimumHappiness;
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
      public ComponentTypeHandle<Household> __Game_Citizens_Household_RW_ComponentTypeHandle;
      public ComponentTypeHandle<HouseholdNeed> __Game_Citizens_HouseholdNeed_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RW_ComponentTypeHandle;
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public BufferTypeHandle<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle;
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RW_ComponentLookup;
      public ComponentLookup<Game.Buildings.Park> __Game_Buildings_Park_RW_ComponentLookup;
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RW_ComponentLookup;
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RW_ComponentLookup;
      public ComponentLookup<Game.Citizens.Student> __Game_Citizens_Student_RW_ComponentLookup;
      public ComponentLookup<Worker> __Game_Citizens_Worker_RW_ComponentLookup;
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RW_ComponentLookup;
      public ComponentLookup<Population> __Game_City_Population_RW_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CitizenRequirementData> __Game_Prefabs_CitizenRequirementData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<UnlockRequirementData> __Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Population> __Game_City_Population_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdNeed_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HouseholdNeed>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RW_BufferTypeHandle = state.GetBufferTypeHandle<HouseholdCitizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RW_ComponentLookup = state.GetComponentLookup<HealthProblem>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RW_ComponentLookup = state.GetComponentLookup<Game.Buildings.Park>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RW_ComponentLookup = state.GetComponentLookup<Abandoned>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentLookup = state.GetComponentLookup<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Student_RW_ComponentLookup = state.GetComponentLookup<Game.Citizens.Student>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RW_ComponentLookup = state.GetComponentLookup<Worker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RW_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RW_ComponentLookup = state.GetComponentLookup<Population>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CitizenRequirementData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CitizenRequirementData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UnlockRequirementData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnlockRequirementData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Population_RO_ComponentLookup = state.GetComponentLookup<Population>(true);
      }
    }
  }
}
