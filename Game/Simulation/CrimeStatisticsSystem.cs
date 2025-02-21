// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CrimeStatisticsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Game.Buildings;
using Game.City;
using Game.Common;
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
  public class CrimeStatisticsSystem : GameSystemBase
  {
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_CrimeProducerQuery;
    private CrimeStatisticsSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_263205583_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 8192;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CrimeProducerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<CrimeProducer>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CrimeProducerQuery);
      this.RequireForUpdate<PoliceConfigurationData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      PoliceConfigurationData singleton = this.__query_263205583_0.GetSingleton<PoliceConfigurationData>();
      if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_PoliceServicePrefab))
        return;
      NativeAccumulator<AverageFloat> nativeAccumulator = new NativeAccumulator<AverageFloat>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CrimeStatisticsSystem.AverageCrimeJob jobData1 = new CrimeStatisticsSystem.AverageCrimeJob()
      {
        m_CrimeProducerType = this.__TypeHandle.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle,
        m_MaxCrimeAccumulation = singleton.m_MaxCrimeAccumulation,
        m_AverageCrime = nativeAccumulator.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<CrimeStatisticsSystem.AverageCrimeJob>(this.m_CrimeProducerQuery, this.Dependency);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CrimeStatisticsSystem.StatisticsJob jobData2 = new CrimeStatisticsSystem.StatisticsJob()
      {
        m_AverageCrime = nativeAccumulator,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps)
      };
      this.Dependency = jobData2.Schedule<CrimeStatisticsSystem.StatisticsJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
      nativeAccumulator.Dispose(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_263205583_0 = state.GetEntityQuery(new EntityQueryDesc()
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
    public CrimeStatisticsSystem()
    {
    }

    [BurstCompile]
    private struct AverageCrimeJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> m_CrimeProducerType;
      public float m_MaxCrimeAccumulation;
      public NativeAccumulator<AverageFloat>.ParallelWriter m_AverageCrime;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CrimeProducer> nativeArray = chunk.GetNativeArray<CrimeProducer>(ref this.m_CrimeProducerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AverageCrime.Accumulate(new AverageFloat()
          {
            m_Count = 1,
            m_Total = nativeArray[index].m_Crime / this.m_MaxCrimeAccumulation
          });
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
      public NativeAccumulator<AverageFloat> m_AverageCrime;
      public NativeQueue<StatisticsEvent> m_StatisticsEventQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.CrimeRate,
          m_Change = 100f * this.m_AverageCrime.GetResult().average
        });
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> __Game_Buildings_CrimeProducer_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CrimeProducer>(true);
      }
    }
  }
}
