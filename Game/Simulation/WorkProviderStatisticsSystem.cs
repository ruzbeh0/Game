// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WorkProviderStatisticsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WorkProviderStatisticsSystem : GameSystemBase
  {
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_WorkProviderQuery;
    private WorkProviderStatisticsSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1149970541_0;
    private EntityQuery __query_1149970541_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 8192;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WorkProviderQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<WorkProvider>(),
          ComponentType.ReadOnly<Employee>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WorkProviderQuery);
      this.RequireForUpdate<WorkProviderParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.HasEnabledComponent<Locked>(this.__query_1149970541_0.GetSingleton<PoliceConfigurationData>().m_PoliceServicePrefab))
        return;
      NativeAccumulator<AverageFloat> nativeAccumulator = new NativeAccumulator<AverageFloat>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      WorkProviderStatisticsSystem.CountSeniorWorkplacesJob jobData1 = new WorkProviderStatisticsSystem.CountSeniorWorkplacesJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WorkProviderType = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_FreeWorkplacesType = this.__TypeHandle.__Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_SeniorEmployeeLevel = this.__query_1149970541_1.GetSingleton<WorkProviderParameterData>().m_SeniorEmployeeLevel,
        m_FreeSeniorWorkplaces = nativeAccumulator.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<WorkProviderStatisticsSystem.CountSeniorWorkplacesJob>(this.m_WorkProviderQuery, this.Dependency);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WorkProviderStatisticsSystem.StatisticsJob jobData2 = new WorkProviderStatisticsSystem.StatisticsJob()
      {
        m_FreeSeniorWorkplaces = nativeAccumulator,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps)
      };
      this.Dependency = jobData2.Schedule<WorkProviderStatisticsSystem.StatisticsJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      nativeAccumulator.Dispose(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1149970541_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PoliceConfigurationData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_1149970541_1 = state.GetEntityQuery(new EntityQueryDesc()
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
    public WorkProviderStatisticsSystem()
    {
    }

    [BurstCompile]
    private struct CountSeniorWorkplacesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderType;
      [ReadOnly]
      public ComponentTypeHandle<FreeWorkplaces> m_FreeWorkplacesType;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      public int m_SeniorEmployeeLevel;
      public NativeAccumulator<AverageFloat>.ParallelWriter m_FreeSeniorWorkplaces;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray2 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<FreeWorkplaces> nativeArray3 = chunk.GetNativeArray<FreeWorkplaces>(ref this.m_FreeWorkplacesType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity prefab = nativeArray1[index].m_Prefab;
          WorkProvider workProvider = nativeArray2[index];
          WorkplaceData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_WorkplaceDatas.TryGetComponent(prefab, out componentData1))
          {
            SpawnableBuildingData componentData2;
            // ISSUE: reference to a compiler-generated field
            int buildingLevel = this.m_SpawnableBuildingDatas.TryGetComponent(prefab, out componentData2) ? (int) componentData2.m_Level : 1;
            // ISSUE: reference to a compiler-generated method
            Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(workProvider.m_MaxWorkers, componentData1.m_Complexity, buildingLevel);
            FreeWorkplaces freeWorkplaces = nativeArray3.Length != 0 ? nativeArray3[index] : new FreeWorkplaces();
            // ISSUE: reference to a compiler-generated field
            for (int seniorEmployeeLevel = this.m_SeniorEmployeeLevel; seniorEmployeeLevel < 5; ++seniorEmployeeLevel)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_FreeSeniorWorkplaces.Accumulate(new AverageFloat()
              {
                m_Count = (int) freeWorkplaces.GetFree(seniorEmployeeLevel),
                m_Total = (float) numberOfWorkplaces[seniorEmployeeLevel]
              });
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
    private struct StatisticsJob : IJob
    {
      public NativeAccumulator<AverageFloat> m_FreeSeniorWorkplaces;
      public NativeQueue<StatisticsEvent> m_StatisticsEventQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.SeniorWorkerInDemandPercentage,
          m_Change = 100f * this.m_FreeSeniorWorkplaces.GetResult().average
        });
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<FreeWorkplaces> __Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_FreeWorkplaces_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FreeWorkplaces>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
      }
    }
  }
}
