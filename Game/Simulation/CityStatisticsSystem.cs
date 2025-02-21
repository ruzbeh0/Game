// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CityStatisticsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.City;
using Game.Prefabs;
using Game.Serialization;
using Game.Triggers;
using System;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CityStatisticsSystem : 
    GameSystemBase,
    ICityStatisticsSystem,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    public const int kUpdatesPerDay = 32;
    private CitySystem m_CitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private CountHouseholdDataSystem m_CountHouseholdDataSystem;
    private TriggerSystem m_TriggerSystem;
    private EntityQuery m_StatisticsPrefabQuery;
    private EntityQuery m_StatisticsQuery;
    private EntityQuery m_CityQuery;
    private NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> m_StatisticsLookup;
    private NativeQueue<StatisticsEvent> m_StatisticsEventQueue;
    private JobHandle m_Writers;
    private bool m_Initialized;
    private int m_SampleCount = 1;
    private uint m_LastSampleFrameIndex;
    private CityStatisticsSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 8192;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 0;

    public int sampleCount => this.m_SampleCount;

    public System.Action eventStatisticsUpdated { get; set; }

    public NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> GetLookup()
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_StatisticsLookup;
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
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CountHouseholdDataSystem = this.World.GetOrCreateSystemManaged<CountHouseholdDataSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<StatisticsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityStatistic>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CityQuery);
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsLookup = new NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity>(64, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsEventQueue = new NativeQueue<StatisticsEvent>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.Enabled = false;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsLookup.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsEventQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Initialized)
      {
        // ISSUE: reference to a compiler-generated method
        this.InitializeLookup();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_Tourism_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = new CityStatisticsSystem.CityStatisticsJob()
      {
        m_StatisticsEventQueue = this.m_StatisticsEventQueue,
        m_HouseholdData = this.m_CountHouseholdDataSystem.GetHouseholdCountData(),
        m_Tourisms = this.__TypeHandle.__Game_City_Tourism_RW_ComponentLookup,
        m_City = this.m_CitySystem.City,
        m_Money = ((float) this.m_CitySystem.moneyAmount)
      }.Schedule<CityStatisticsSystem.CityStatisticsJob>(JobHandle.CombineDependencies(this.Dependency, this.m_Writers));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle dependsOn = new CityStatisticsSystem.ProcessStatisticsJob()
      {
        m_Statistics = this.__TypeHandle.__Game_City_CityStatistic_RW_BufferLookup,
        m_StatisticsLookup = this.m_StatisticsLookup,
        m_Queue = this.m_StatisticsEventQueue
      }.Schedule<CityStatisticsSystem.ProcessStatisticsJob>(JobHandle.CombineDependencies(job0, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityStatistic_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StatisticsData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.Dependency = new CityStatisticsSystem.ResetEntityJob()
      {
        m_Money = this.m_CitySystem.moneyAmount,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabStats = this.__TypeHandle.__Game_Prefabs_StatisticsData_RO_ComponentLookup,
        m_Statistics = this.__TypeHandle.__Game_City_CityStatistic_RW_BufferLookup,
        m_StatisticsLookup = this.m_StatisticsLookup
      }.Schedule<CityStatisticsSystem.ResetEntityJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated method
      this.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      ++this.m_SampleCount;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LastSampleFrameIndex = this.m_SimulationSystem.frameIndex;
      System.Action statisticsUpdated = this.eventStatisticsUpdated;
      if (statisticsUpdated == null)
        return;
      statisticsUpdated();
    }

    public static int GetStatisticValue(
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated method
      long statisticValueLong = CityStatisticsSystem.GetStatisticValueLong(statisticsLookup, stats, type, parameter);
      if (statisticValueLong > (long) int.MaxValue)
        return int.MaxValue;
      return statisticValueLong >= (long) int.MinValue ? (int) statisticValueLong : int.MinValue;
    }

    public static long GetStatisticValueLong(
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated method
      double statisticValueDouble = CityStatisticsSystem.GetStatisticValueDouble(statisticsLookup, stats, type, parameter);
      if (statisticValueDouble > (double) long.MaxValue)
        return long.MaxValue;
      return statisticValueDouble >= (double) long.MinValue ? (long) statisticValueDouble : long.MinValue;
    }

    private static double GetStatisticValueDouble(
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      if (statisticsLookup.ContainsKey(key))
      {
        Entity entity = statisticsLookup[key];
        if (stats.HasBuffer(entity))
        {
          DynamicBuffer<CityStatistic> stat = stats[entity];
          if (stat.Length <= 0)
            return 0.0;
          ref DynamicBuffer<CityStatistic> local = ref stat;
          return Math.Round(local[local.Length - 1].m_TotalValue, MidpointRounding.AwayFromZero);
        }
      }
      return 0.0;
    }

    public int GetStatisticValue(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      long statisticValueLong = CityStatisticsSystem.GetStatisticValueLong(this.m_StatisticsLookup, stats, type, parameter);
      if (statisticValueLong > (long) int.MaxValue)
        return int.MaxValue;
      return statisticValueLong >= (long) int.MinValue ? (int) statisticValueLong : int.MinValue;
    }

    public long GetStatisticValueLong(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      double statisticValueDouble = CityStatisticsSystem.GetStatisticValueDouble(this.m_StatisticsLookup, stats, type, parameter);
      if (statisticValueDouble > (double) long.MaxValue)
        return long.MaxValue;
      return statisticValueDouble >= (double) long.MinValue ? (long) statisticValueDouble : long.MinValue;
    }

    private double GetStatisticValueDouble(StatisticType type, int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      DynamicBuffer<CityStatistic> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StatisticsLookup.ContainsKey(key) || !this.EntityManager.TryGetBuffer<CityStatistic>(this.m_StatisticsLookup[key], true, out buffer) || buffer.Length <= 0)
        return 0.0;
      ref DynamicBuffer<CityStatistic> local = ref buffer;
      return Math.Round(local[local.Length - 1].m_TotalValue, MidpointRounding.AwayFromZero);
    }

    public int GetStatisticValue(StatisticType type, int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated method
      long statisticValueLong = this.GetStatisticValueLong(type, parameter);
      if (statisticValueLong > (long) int.MaxValue)
        return int.MaxValue;
      return statisticValueLong >= (long) int.MinValue ? (int) statisticValueLong : int.MinValue;
    }

    public long GetStatisticValueLong(StatisticType type, int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated method
      double statisticValueDouble = this.GetStatisticValueDouble(type, parameter);
      if (statisticValueDouble > (double) long.MaxValue)
        return long.MaxValue;
      return statisticValueDouble >= (double) long.MinValue ? (long) statisticValueDouble : long.MinValue;
    }

    public static NativeArray<long> GetStatisticDataArrayLong(
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      if (statisticsLookup.ContainsKey(key))
      {
        Entity entity = statisticsLookup[key];
        if (stats.HasBuffer(entity))
        {
          DynamicBuffer<CityStatistic> stat = stats[entity];
          NativeArray<long> statisticDataArrayLong = new NativeArray<long>(stat.Length, Allocator.Temp);
          for (int index = 0; index < stat.Length; ++index)
          {
            double num = Math.Round(stat[index].m_TotalValue, MidpointRounding.AwayFromZero);
            statisticDataArrayLong[index] = num > (double) long.MaxValue ? long.MaxValue : (num < (double) long.MinValue ? long.MinValue : (long) num);
          }
          return statisticDataArrayLong;
        }
      }
      return new NativeArray<long>(1, Allocator.Temp);
    }

    public static NativeArray<int> GetStatisticDataArray(
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      if (statisticsLookup.ContainsKey(key))
      {
        Entity entity = statisticsLookup[key];
        if (stats.HasBuffer(entity))
        {
          DynamicBuffer<CityStatistic> stat = stats[entity];
          NativeArray<int> statisticDataArray = new NativeArray<int>(stat.Length, Allocator.Temp);
          for (int index = 0; index < stat.Length; ++index)
          {
            double num = Math.Round(stat[index].m_TotalValue, MidpointRounding.AwayFromZero);
            statisticDataArray[index] = num > (double) int.MaxValue ? int.MaxValue : (num < (double) int.MinValue ? int.MinValue : (int) num);
          }
          return statisticDataArray;
        }
      }
      return new NativeArray<int>(1, Allocator.Temp);
    }

    public NativeArray<long> GetStatisticDataArrayLong(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return CityStatisticsSystem.GetStatisticDataArrayLong(this.m_StatisticsLookup, stats, type, parameter);
    }

    public NativeArray<int> GetStatisticDataArray(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return CityStatisticsSystem.GetStatisticDataArray(this.m_StatisticsLookup, stats, type, parameter);
    }

    public NativeArray<long> GetStatisticDataArrayLong(StatisticType type, int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      DynamicBuffer<CityStatistic> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StatisticsLookup.ContainsKey(key) || !this.EntityManager.TryGetBuffer<CityStatistic>(this.m_StatisticsLookup[key], true, out buffer))
        return new NativeArray<long>(1, Allocator.Temp);
      NativeArray<long> statisticDataArrayLong = new NativeArray<long>(buffer.Length, Allocator.Temp);
      for (int index = 0; index < buffer.Length; ++index)
      {
        double num = Math.Round(buffer[index].m_TotalValue, MidpointRounding.AwayFromZero);
        statisticDataArrayLong[index] = num > (double) long.MaxValue ? long.MaxValue : (num < (double) long.MinValue ? long.MinValue : (long) num);
      }
      return statisticDataArrayLong;
    }

    public NativeArray<int> GetStatisticDataArray(StatisticType type, int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      DynamicBuffer<CityStatistic> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_StatisticsLookup.ContainsKey(key) || !this.EntityManager.TryGetBuffer<CityStatistic>(this.m_StatisticsLookup[key], true, out buffer))
        return new NativeArray<int>(1, Allocator.Temp);
      NativeArray<int> statisticDataArray = new NativeArray<int>(buffer.Length, Allocator.Temp);
      for (int index = 0; index < buffer.Length; ++index)
      {
        double num = Math.Round(buffer[index].m_TotalValue, MidpointRounding.AwayFromZero);
        statisticDataArray[index] = num > (double) int.MaxValue ? int.MaxValue : (num < (double) int.MinValue ? int.MinValue : (int) num);
      }
      return statisticDataArray;
    }

    public NativeArray<CityStatistic> GetStatisticArray(StatisticType type, int parameter = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(type, parameter);
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      DynamicBuffer<CityStatistic> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_StatisticsLookup.ContainsKey(key) && this.EntityManager.TryGetBuffer<CityStatistic>(this.m_StatisticsLookup[key], true, out buffer) ? buffer.AsNativeArray() : new NativeArray<CityStatistic>(1, Allocator.Temp);
    }

    public uint GetSampleFrameIndex(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_LastSampleFrameIndex - (uint) ((this.sampleCount - index - 1) * 8192);
    }

    private void InitializeLookup()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsLookup.Clear();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_StatisticsPrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<StatisticsData> componentDataArray = this.m_StatisticsPrefabQuery.ToComponentDataArray<StatisticsData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < entityArray1.Length; ++index1)
      {
        StatisticType statisticType = componentDataArray[index1].m_StatisticType;
        DynamicBuffer<StatisticParameterData> buffer;
        if (this.EntityManager.TryGetBuffer<StatisticParameterData>(entityArray1[index1], true, out buffer))
        {
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_StatisticsLookup.Add(new CityStatisticsSystem.StatisticsKey(statisticType, buffer[index2].m_Value), Entity.Null);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_StatisticsLookup.Add(new CityStatisticsSystem.StatisticsKey(statisticType, 0), Entity.Null);
        }
      }
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray2 = this.m_StatisticsQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      bool flag1 = true;
      for (int index3 = 0; index3 < entityArray2.Length; ++index3)
      {
        PrefabRef component1;
        if (this.EntityManager.TryGetComponent<PrefabRef>(entityArray2[index3], out component1))
        {
          int num = 0;
          StatisticParameter component2;
          if (this.EntityManager.TryGetComponent<StatisticParameter>(entityArray2[index3], out component2))
            num = component2.m_Value;
          DynamicBuffer<StatisticParameterData> buffer;
          if (this.EntityManager.TryGetBuffer<StatisticParameterData>(component1.m_Prefab, true, out buffer))
          {
            flag1 = false;
            for (int index4 = 0; index4 < buffer.Length; ++index4)
            {
              if (num == buffer[index4].m_Value)
              {
                flag1 = true;
                break;
              }
            }
            if (!flag1)
              break;
          }
        }
      }
      if (flag1)
      {
        for (int index = 0; index < entityArray2.Length; ++index)
        {
          PrefabRef component3;
          StatisticsData component4;
          if (this.EntityManager.TryGetComponent<PrefabRef>(entityArray2[index], out component3) && this.EntityManager.TryGetComponent<StatisticsData>(component3.m_Prefab, out component4))
          {
            int parameter = 0;
            StatisticParameter component5;
            if (this.EntityManager.TryGetComponent<StatisticParameter>(entityArray2[index], out component5))
              parameter = component5.m_Value;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(component4.m_StatisticType, parameter);
            // ISSUE: reference to a compiler-generated field
            if (this.m_StatisticsLookup.ContainsKey(key))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_StatisticsLookup[key] = entityArray2[index];
            }
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.DestroyEntity(this.m_StatisticsQuery);
        // ISSUE: reference to a compiler-generated field
        this.m_SampleCount = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Clear();
      }
      entityArray2.Dispose();
      // ISSUE: reference to a compiler-generated field
      NativeKeyValueArrays<CityStatisticsSystem.StatisticsKey, Entity> keyValueArrays = this.m_StatisticsLookup.GetKeyValueArrays((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index5 = 0; index5 < keyValueArrays.Length; ++index5)
      {
        if (keyValueArrays.Values[index5] == Entity.Null)
        {
          // ISSUE: variable of a compiler-generated type
          CityStatisticsSystem.StatisticsKey key = keyValueArrays.Keys[index5];
          StatisticType type = key.type;
          for (int index6 = 0; index6 < entityArray1.Length; ++index6)
          {
            Entity entity = entityArray1[index6];
            DynamicBuffer<StatisticParameterData> buffer1;
            bool buffer2 = this.EntityManager.TryGetBuffer<StatisticParameterData>(entity, true, out buffer1);
            StatisticsData component;
            if (this.EntityManager.TryGetComponent<StatisticsData>(entity, out component) && component.m_StatisticType == type)
            {
              if (buffer2)
              {
                bool flag2 = false;
                for (int index7 = 0; index7 < buffer1.Length; ++index7)
                {
                  if (buffer1[index7].m_Value == key.parameter)
                    flag2 = true;
                }
                if (!flag2)
                  continue;
              }
              Game.Prefabs.ArchetypeData componentData = this.EntityManager.GetComponentData<Game.Prefabs.ArchetypeData>(entity);
              // ISSUE: reference to a compiler-generated field
              this.m_StatisticsLookup[key] = StatisticsPrefab.CreateInstance(this.World.EntityManager, entity, componentData, key.parameter);
              break;
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Initialized = true;
      entityArray1.Dispose();
      componentDataArray.Dispose();
    }

    public void CompleteWriters() => this.m_Writers.Complete();

    public NativeQueue<StatisticsEvent> GetStatisticsEventQueue(out JobHandle deps)
    {
      Assert.IsTrue(this.Enabled, "Can not write to queue when system isn't running");
      // ISSUE: reference to a compiler-generated field
      deps = this.m_Writers;
      // ISSUE: reference to a compiler-generated field
      return this.m_StatisticsEventQueue;
    }

    public CityStatisticsSystem.SafeStatisticQueue GetSafeStatisticsQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_Writers;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new CityStatisticsSystem.SafeStatisticQueue(this.m_StatisticsEventQueue, this.Enabled);
    }

    public void AddWriter(JobHandle writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Writers = JobHandle.CombineDependencies(this.m_Writers, writer);
    }

    public void DiscardStatistics()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsEventQueue.Clear();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_SampleCount);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastSampleFrameIndex);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StatisticsEventQueue.Count);
      // ISSUE: reference to a compiler-generated field
      NativeArray<StatisticsEvent> array = this.m_StatisticsEventQueue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < array.Length; ++index)
        writer.Write<StatisticsEvent>(array[index]);
      array.Dispose();
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Writers.Complete();
      if (reader.context.version >= Game.Version.statisticsRefactor)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_SampleCount);
        if (reader.context.version >= Game.Version.statsLastFrameIndex)
        {
          // ISSUE: reference to a compiler-generated field
          reader.Read(out this.m_LastSampleFrameIndex);
        }
        if (!(reader.context.version >= Game.Version.statisticsFix2))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Clear();
        int num;
        reader.Read(out num);
        for (int index = 0; index < num; ++index)
        {
          StatisticsEvent statisticsEvent;
          reader.Read<StatisticsEvent>(out statisticsEvent);
          if (!(reader.context.version < Game.Version.statisticUnifying) || statisticsEvent.m_Statistic != StatisticType.Population && statisticsEvent.m_Statistic != StatisticType.Health && statisticsEvent.m_Statistic != StatisticType.Age && statisticsEvent.m_Statistic != StatisticType.Wellbeing && statisticsEvent.m_Statistic != StatisticType.AdultsCount && statisticsEvent.m_Statistic != StatisticType.HouseholdCount && statisticsEvent.m_Statistic != StatisticType.EducationCount && statisticsEvent.m_Statistic != StatisticType.Unemployed && statisticsEvent.m_Statistic != StatisticType.WorkerCount)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_StatisticsEventQueue.Enqueue(statisticsEvent);
          }
        }
      }
      else
      {
        Entity entity;
        reader.Read(out entity);
        reader.Read(out entity);
        reader.Read(out entity);
        int length;
        reader.Read(out length);
        int num;
        for (int index = 0; index < length; ++index)
        {
          reader.Read(out num);
          reader.Read(out num);
        }
        reader.Read(out length);
        NativeArray<Entity> nativeArray = new NativeArray<Entity>(length, Allocator.Temp);
        reader.Read(nativeArray);
        nativeArray.Dispose();
        reader.Read(out num);
        reader.Read(out num);
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SampleCount = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsEventQueue.Clear();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StatisticsLookup.Clear();
      // ISSUE: reference to a compiler-generated method
      this.InitializeLookup();
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
    public CityStatisticsSystem()
    {
    }

    public readonly struct StatisticsKey : IEquatable<CityStatisticsSystem.StatisticsKey>
    {
      public StatisticType type { get; }

      public int parameter { get; }

      public StatisticsKey(StatisticType type, int parameter)
      {
        this.type = type;
        this.parameter = parameter;
      }

      public override bool Equals(object obj)
      {
        // ISSUE: reference to a compiler-generated method
        return obj is CityStatisticsSystem.StatisticsKey other && this.Equals(other);
      }

      public bool Equals(CityStatisticsSystem.StatisticsKey other)
      {
        int type1 = (int) this.type;
        int parameter1 = this.parameter;
        StatisticType type2 = other.type;
        int parameter2 = other.parameter;
        int num = (int) type2;
        return type1 == num && parameter1 == parameter2;
      }

      public override int GetHashCode() => ((int) this.type * 311 + this.parameter).GetHashCode();
    }

    public struct SafeStatisticQueue
    {
      [NativeDisableContainerSafetyRestriction]
      private NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      [ReadOnly]
      public bool m_StatisticsEnabled;

      public SafeStatisticQueue(NativeQueue<StatisticsEvent> queue, bool enabled)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue = queue.AsParallelWriter();
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEnabled = enabled;
      }

      public void Enqueue(StatisticsEvent statisticsEvent)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_StatisticsEnabled)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(statisticsEvent);
      }
    }

    [BurstCompile]
    private struct CityStatisticsJob : IJob
    {
      public NativeQueue<StatisticsEvent> m_StatisticsEventQueue;
      [ReadOnly]
      public CountHouseholdDataSystem.HouseholdData m_HouseholdData;
      [ReadOnly]
      public ComponentLookup<Tourism> m_Tourisms;
      [ReadOnly]
      public Entity m_City;
      public float m_Money;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Money,
          m_Change = this.m_Money
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.LodgingTotal,
          m_Change = (float) this.m_Tourisms[this.m_City].m_Lodging.y
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.LodgingUsed,
          m_Change = (float) this.m_Tourisms[this.m_City].m_Lodging.x
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.HouseholdCount,
          m_Change = (float) this.m_HouseholdData.m_MovedInHouseholdCount
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.TouristCount,
          m_Change = (float) this.m_HouseholdData.m_TouristCitizenCount
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.AdultsCount,
          m_Change = (float) this.m_HouseholdData.m_AdultCount
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.WorkerCount,
          m_Change = (float) this.m_HouseholdData.m_CityWorkerCount
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Unemployed,
          m_Change = (float) this.m_HouseholdData.Unemployed()
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Population,
          m_Change = (float) this.m_HouseholdData.Population()
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Age,
          m_Change = (float) this.m_HouseholdData.m_ChildrenCount,
          m_Parameter = 0
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Age,
          m_Change = (float) this.m_HouseholdData.m_TeenCount,
          m_Parameter = 1
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Age,
          m_Change = (float) this.m_HouseholdData.m_AdultCount,
          m_Parameter = 2
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Age,
          m_Change = (float) this.m_HouseholdData.m_SeniorCount,
          m_Parameter = 3
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Wellbeing,
          m_Change = (float) this.m_HouseholdData.m_TotalMovedInCitizenWellbeing
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Health,
          m_Change = (float) this.m_HouseholdData.m_TotalMovedInCitizenHealth
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.EducationCount,
          m_Change = (float) this.m_HouseholdData.m_UneducatedCount,
          m_Parameter = 0
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.EducationCount,
          m_Change = (float) this.m_HouseholdData.m_PoorlyEducatedCount,
          m_Parameter = 1
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.EducationCount,
          m_Change = (float) this.m_HouseholdData.m_EducatedCount,
          m_Parameter = 2
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.EducationCount,
          m_Change = (float) this.m_HouseholdData.m_WellEducatedCount,
          m_Parameter = 3
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.EducationCount,
          m_Change = (float) this.m_HouseholdData.m_HighlyEducatedCount,
          m_Parameter = 4
        });
      }
    }

    [BurstCompile]
    private struct ProcessStatisticsJob : IJob
    {
      public NativeQueue<StatisticsEvent> m_Queue;
      [ReadOnly]
      public NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> m_StatisticsLookup;
      public BufferLookup<CityStatistic> m_Statistics;

      public void Execute()
      {
        StatisticsEvent statisticsEvent;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue.TryDequeue(out statisticsEvent))
        {
          if (statisticsEvent.m_Statistic != StatisticType.Count)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CityStatisticsSystem.StatisticsKey key = new CityStatisticsSystem.StatisticsKey(statisticsEvent.m_Statistic, statisticsEvent.m_Parameter);
            // ISSUE: reference to a compiler-generated field
            if (this.m_StatisticsLookup.ContainsKey(key))
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_StatisticsLookup[key];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Statistics.HasBuffer(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<CityStatistic> statistic = this.m_Statistics[entity];
                if (statistic.Length == 0)
                  statistic.Add(new CityStatistic()
                  {
                    m_TotalValue = 0.0,
                    m_Value = 0.0
                  });
                ref DynamicBuffer<CityStatistic> local1 = ref statistic;
                CityStatistic cityStatistic = local1[local1.Length - 1];
                if (statistic.Length == 1 && statisticsEvent.m_Statistic == StatisticType.Money)
                  cityStatistic.m_TotalValue = (double) statisticsEvent.m_Change;
                cityStatistic.m_Value += (double) statisticsEvent.m_Change;
                ref DynamicBuffer<CityStatistic> local2 = ref statistic;
                local2[local2.Length - 1] = cityStatistic;
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct ResetEntityJob : IJob
    {
      public int m_Money;
      [ReadOnly]
      public NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> m_StatisticsLookup;
      public BufferLookup<CityStatistic> m_Statistics;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<StatisticsData> m_PrefabStats;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CityStatisticsSystem.StatisticsKey> keyArray = this.m_StatisticsLookup.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_StatisticsLookup[keyArray[index]];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Statistics.HasBuffer(entity))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CityStatistic> statistic = this.m_Statistics[entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            StatisticsData prefabStat = this.m_PrefabStats[this.m_Prefabs[entity].m_Prefab];
            CityStatistic cityStatistic1;
            if (statistic.Length == 0)
            {
              ref DynamicBuffer<CityStatistic> local = ref statistic;
              cityStatistic1 = new CityStatistic();
              cityStatistic1.m_TotalValue = 0.0;
              // ISSUE: reference to a compiler-generated field
              cityStatistic1.m_Value = prefabStat.m_StatisticType == StatisticType.Money ? (double) this.m_Money : 0.0;
              CityStatistic elem = cityStatistic1;
              local.Add(elem);
            }
            ref DynamicBuffer<CityStatistic> local1 = ref statistic;
            CityStatistic cityStatistic2 = local1[local1.Length - 1];
            if (prefabStat.m_CollectionType == StatisticCollectionType.Cumulative)
            {
              ref DynamicBuffer<CityStatistic> local2 = ref statistic;
              cityStatistic1 = new CityStatistic();
              cityStatistic1.m_TotalValue = cityStatistic2.m_TotalValue + cityStatistic2.m_Value;
              cityStatistic1.m_Value = 0.0;
              CityStatistic elem = cityStatistic1;
              local2.Add(elem);
            }
            else if (prefabStat.m_CollectionType == StatisticCollectionType.Point)
            {
              ref DynamicBuffer<CityStatistic> local3 = ref statistic;
              cityStatistic1 = new CityStatistic();
              cityStatistic1.m_TotalValue = cityStatistic2.m_Value;
              cityStatistic1.m_Value = 0.0;
              CityStatistic elem = cityStatistic1;
              local3.Add(elem);
            }
            else if (prefabStat.m_CollectionType == StatisticCollectionType.Daily)
            {
              double num = 0.0;
              if (statistic.Length >= 32)
              {
                ref DynamicBuffer<CityStatistic> local4 = ref statistic;
                num = local4[local4.Length - 32].m_Value;
              }
              ref DynamicBuffer<CityStatistic> local5 = ref statistic;
              cityStatistic1 = new CityStatistic();
              cityStatistic1.m_TotalValue = cityStatistic2.m_TotalValue + cityStatistic2.m_Value - num;
              cityStatistic1.m_Value = 0.0;
              CityStatistic elem = cityStatistic1;
              local5.Add(elem);
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<Tourism> __Game_City_Tourism_RW_ComponentLookup;
      public BufferLookup<CityStatistic> __Game_City_CityStatistic_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StatisticsData> __Game_Prefabs_StatisticsData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_Tourism_RW_ComponentLookup = state.GetComponentLookup<Tourism>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityStatistic_RW_BufferLookup = state.GetBufferLookup<CityStatistic>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StatisticsData_RO_ComponentLookup = state.GetComponentLookup<StatisticsData>(true);
      }
    }
  }
}
