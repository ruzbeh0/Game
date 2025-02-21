// Decompiled with JetBrains decompiler
// Type: Game.Simulation.StatisticTriggerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.City;
using Game.Prefabs;
using Game.Triggers;
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
  public class StatisticTriggerSystem : GameSystemBase
  {
    public const int kUpdatesPerDay = 32;
    private ICityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_PrefabQuery;
    private TriggerSystem m_TriggerSystem;
    private StatisticTriggerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 8192;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = (ICityStatisticsSystem) this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<StatisticTriggerData>(), ComponentType.ReadOnly<TriggerData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StatisticsData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StatisticTriggerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StatisticTriggerSystem.SendTriggersJob jobData = new StatisticTriggerSystem.SendTriggersJob()
      {
        m_EntityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_StatisticTriggerDataHandle = this.__TypeHandle.__Game_Prefabs_StatisticTriggerData_RO_ComponentTypeHandle,
        m_StatisticsDatas = this.__TypeHandle.__Game_Prefabs_StatisticsData_RO_ComponentLookup,
        m_Locked = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup,
        m_CityStatistics = this.__TypeHandle.__Game_City_CityStatistic_RO_BufferLookup,
        m_StatisticsLookup = this.m_CityStatisticsSystem.GetLookup(),
        m_ActionQueue = this.m_TriggerSystem.CreateActionBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<StatisticTriggerSystem.SendTriggersJob>(this.m_PrefabQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
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
    public StatisticTriggerSystem()
    {
    }

    [BurstCompile]
    private struct SendTriggersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StatisticTriggerData> m_StatisticTriggerDataHandle;
      [ReadOnly]
      public ComponentLookup<StatisticsData> m_StatisticsDatas;
      [ReadOnly]
      public ComponentLookup<Locked> m_Locked;
      [ReadOnly]
      public BufferLookup<CityStatistic> m_CityStatistics;
      [ReadOnly]
      public NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> m_StatisticsLookup;
      public NativeQueue<TriggerAction>.ParallelWriter m_ActionQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityTypeHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<StatisticTriggerData> nativeArray2 = chunk.GetNativeArray<StatisticTriggerData>(ref this.m_StatisticTriggerDataHandle);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          StatisticTriggerData statisticTriggerData = nativeArray2[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NativeArray<int> statisticDataArray1 = CityStatisticsSystem.GetStatisticDataArray(this.m_StatisticsLookup, this.m_CityStatistics, this.m_StatisticsDatas[statisticTriggerData.m_StatisticEntity].m_StatisticType, statisticTriggerData.m_StatisticParameter);
          int timeframe = math.max(1, statisticTriggerData.m_TimeFrame);
          TriggerAction triggerAction = new TriggerAction()
          {
            m_TriggerType = TriggerType.StatisticsValue,
            m_TriggerPrefab = nativeArray1[index1]
          };
          if (statisticTriggerData.m_NormalizeWithPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            NativeArray<int> statisticDataArray2 = CityStatisticsSystem.GetStatisticDataArray(this.m_StatisticsLookup, this.m_CityStatistics, this.m_StatisticsDatas[statisticTriggerData.m_NormalizeWithPrefab].m_StatisticType, statisticTriggerData.m_NormalizeWithParameter);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (statisticDataArray1.Length >= timeframe && statisticDataArray2.Length >= timeframe && statisticDataArray1.Length >= statisticTriggerData.m_MinSamples && statisticDataArray2.Length >= statisticTriggerData.m_MinSamples && !this.m_Locked.HasEnabledComponent<Locked>(statisticTriggerData.m_StatisticEntity) && !this.m_Locked.HasEnabledComponent<Locked>(statisticTriggerData.m_NormalizeWithPrefab))
            {
              if (statisticTriggerData.m_Type == StatisticTriggerType.TotalValue)
              {
                // ISSUE: reference to a compiler-generated method
                if (this.NonZeroValues(statisticDataArray2, timeframe))
                {
                  float num = 0.0f;
                  for (int index2 = 1; index2 <= timeframe; ++index2)
                    num += (float) statisticDataArray1[statisticDataArray1.Length - index2] / (float) statisticDataArray2[statisticDataArray2.Length - index2];
                  triggerAction.m_Value = num;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ActionQueue.Enqueue(triggerAction);
                }
              }
              else if (statisticTriggerData.m_Type == StatisticTriggerType.AverageValue)
              {
                // ISSUE: reference to a compiler-generated method
                if (this.NonZeroValues(statisticDataArray2, timeframe))
                {
                  float num = 0.0f;
                  for (int index3 = 1; index3 <= timeframe; ++index3)
                    num += (float) statisticDataArray1[statisticDataArray1.Length - index3] / (float) statisticDataArray2[statisticDataArray2.Length - index3];
                  triggerAction.m_Value = num / (float) timeframe;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ActionQueue.Enqueue(triggerAction);
                }
              }
              else if (statisticTriggerData.m_Type == StatisticTriggerType.AbsoluteChange)
              {
                if (statisticDataArray2[statisticDataArray2.Length - timeframe] != 0 && statisticDataArray2[statisticDataArray2.Length - 1] != 0)
                {
                  float num1 = (float) statisticDataArray1[statisticDataArray1.Length - timeframe] / (float) statisticDataArray2[statisticDataArray2.Length - timeframe];
                  float num2 = (float) statisticDataArray1[statisticDataArray1.Length - 1] / (float) statisticDataArray2[statisticDataArray2.Length - 1];
                  triggerAction.m_Value = num2 - num1;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ActionQueue.Enqueue(triggerAction);
                }
              }
              else if (statisticTriggerData.m_Type == StatisticTriggerType.RelativeChange && statisticDataArray2[statisticDataArray2.Length - timeframe] != 0 && statisticDataArray2[statisticDataArray2.Length - 1] != 0)
              {
                float num3 = (float) (statisticDataArray1[statisticDataArray1.Length - timeframe] / statisticDataArray2[statisticDataArray2.Length - timeframe]);
                float num4 = (float) (statisticDataArray1[statisticDataArray1.Length - 1] / statisticDataArray2[statisticDataArray2.Length - 1]);
                if ((double) num3 != 0.0)
                {
                  triggerAction.m_Value = (num4 - num3) / num3;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ActionQueue.Enqueue(triggerAction);
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (statisticDataArray1.Length >= timeframe && statisticDataArray1.Length >= statisticTriggerData.m_MinSamples && !this.m_Locked.HasEnabledComponent<Locked>(statisticTriggerData.m_StatisticEntity))
            {
              if (statisticTriggerData.m_Type == StatisticTriggerType.TotalValue)
              {
                float num = 0.0f;
                for (int index4 = 1; index4 <= timeframe; ++index4)
                  num += (float) statisticDataArray1[statisticDataArray1.Length - index4];
                triggerAction.m_Value = num;
                // ISSUE: reference to a compiler-generated field
                this.m_ActionQueue.Enqueue(triggerAction);
              }
              else if (statisticTriggerData.m_Type == StatisticTriggerType.AverageValue)
              {
                float num = 0.0f;
                for (int index5 = 1; index5 <= timeframe; ++index5)
                  num += (float) statisticDataArray1[statisticDataArray1.Length - index5];
                triggerAction.m_Value = num / (float) timeframe;
                // ISSUE: reference to a compiler-generated field
                this.m_ActionQueue.Enqueue(triggerAction);
              }
              else if (statisticTriggerData.m_Type == StatisticTriggerType.AbsoluteChange)
              {
                float num5 = (float) statisticDataArray1[statisticDataArray1.Length - timeframe];
                float num6 = (float) statisticDataArray1[statisticDataArray1.Length - 1];
                triggerAction.m_Value = num6 - num5;
                // ISSUE: reference to a compiler-generated field
                this.m_ActionQueue.Enqueue(triggerAction);
              }
              else if (statisticTriggerData.m_Type == StatisticTriggerType.RelativeChange)
              {
                float num7 = (float) statisticDataArray1[statisticDataArray1.Length - timeframe];
                float num8 = (float) statisticDataArray1[statisticDataArray1.Length - 1];
                if ((double) num7 != 0.0)
                {
                  triggerAction.m_Value = (num8 - num7) / num7;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ActionQueue.Enqueue(triggerAction);
                }
              }
            }
          }
        }
      }

      private bool NonZeroValues(NativeArray<int> values, int timeframe)
      {
        for (int index = 1; index <= timeframe; ++index)
        {
          if (values[values.Length - index] == 0)
            return false;
        }
        return true;
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
      public ComponentTypeHandle<StatisticTriggerData> __Game_Prefabs_StatisticTriggerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<StatisticsData> __Game_Prefabs_StatisticsData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StatisticTriggerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StatisticTriggerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StatisticsData_RO_ComponentLookup = state.GetComponentLookup<StatisticsData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RO_BufferLookup = state.GetBufferLookup<CityStatistic>(true);
      }
    }
  }
}
