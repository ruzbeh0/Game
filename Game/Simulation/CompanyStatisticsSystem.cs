// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CompanyStatisticsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CompanyStatisticsSystem : GameSystemBase
  {
    private EntityQuery m_CompanyGroup;
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private CompanyStatisticsSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<WorkProvider>(),
          ComponentType.ReadOnly<PropertyRenter>(),
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<IndustrialCompany>(),
          ComponentType.ReadOnly<CommercialCompany>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 32, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CompanyStatisticsSystem.ProcessCompanyStatisticsJob jobData = new CompanyStatisticsSystem.ProcessCompanyStatisticsJob()
      {
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.GetSharedComponentTypeHandle<UpdateFrame>(),
        m_CommercialCompanyType = this.__TypeHandle.__Game_Companies_CommercialCompany_RO_ComponentTypeHandle,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter(),
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CompanyStatisticsSystem.ProcessCompanyStatisticsJob>(this.m_CompanyGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
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
    public CompanyStatisticsSystem()
    {
    }

    [BurstCompile]
    private struct ProcessCompanyStatisticsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<CommercialCompany> m_CommercialCompanyType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      public uint m_UpdateFrameIndex;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

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
        bool flag1 = chunk.Has<CommercialCompany>(ref this.m_CommercialCompanyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray1 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        NativeArray<int> nativeArray3 = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        NativeArray<int> nativeArray4 = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        NativeArray<int> nativeArray5 = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity prefab = nativeArray2[index].m_Prefab;
          WorkProvider workProvider = nativeArray1[index];
          DynamicBuffer<Employee> dynamicBuffer = bufferAccessor[index];
          // ISSUE: reference to a compiler-generated field
          int resourceIndex = EconomyUtils.GetResourceIndex(this.m_IndustrialProcessDatas[prefab].m_Output.m_Resource);
          nativeArray5[resourceIndex]++;
          nativeArray4[resourceIndex] += workProvider.m_MaxWorkers;
          nativeArray3[resourceIndex] += dynamicBuffer.Length;
        }
        ResourceIterator iterator = ResourceIterator.GetIterator();
        while (iterator.Next())
        {
          int resourceIndex = EconomyUtils.GetResourceIndex(iterator.resource);
          if (nativeArray5[resourceIndex] > 0)
          {
            bool flag2 = EconomyUtils.IsOfficeResource(iterator.resource);
            // ISSUE: reference to a compiler-generated field
            ref NativeQueue<StatisticsEvent>.ParallelWriter local1 = ref this.m_StatisticsEventQueue;
            StatisticsEvent statisticsEvent1 = new StatisticsEvent();
            statisticsEvent1.m_Statistic = flag1 ? StatisticType.ServiceCount : (flag2 ? StatisticType.OfficeCount : StatisticType.ProcessingCount);
            statisticsEvent1.m_Change = (float) nativeArray5[resourceIndex];
            statisticsEvent1.m_Parameter = resourceIndex;
            StatisticsEvent statisticsEvent2 = statisticsEvent1;
            local1.Enqueue(statisticsEvent2);
            // ISSUE: reference to a compiler-generated field
            ref NativeQueue<StatisticsEvent>.ParallelWriter local2 = ref this.m_StatisticsEventQueue;
            statisticsEvent1 = new StatisticsEvent();
            statisticsEvent1.m_Statistic = flag1 ? StatisticType.ServiceWorkers : (flag2 ? StatisticType.OfficeWorkers : StatisticType.ProcessingWorkers);
            statisticsEvent1.m_Change = (float) nativeArray3[resourceIndex];
            statisticsEvent1.m_Parameter = resourceIndex;
            StatisticsEvent statisticsEvent3 = statisticsEvent1;
            local2.Enqueue(statisticsEvent3);
            // ISSUE: reference to a compiler-generated field
            ref NativeQueue<StatisticsEvent>.ParallelWriter local3 = ref this.m_StatisticsEventQueue;
            statisticsEvent1 = new StatisticsEvent();
            statisticsEvent1.m_Statistic = flag1 ? StatisticType.ServiceMaxWorkers : (flag2 ? StatisticType.OfficeMaxWorkers : StatisticType.ProcessingMaxWorkers);
            statisticsEvent1.m_Change = (float) nativeArray4[resourceIndex];
            statisticsEvent1.m_Parameter = resourceIndex;
            StatisticsEvent statisticsEvent4 = statisticsEvent1;
            local3.Enqueue(statisticsEvent4);
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
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommercialCompany> __Game_Companies_CommercialCompany_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CommercialCompany_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommercialCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
      }
    }
  }
}
