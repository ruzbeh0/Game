// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ICityStatisticsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public interface ICityStatisticsSystem
  {
    int GetStatisticValue(StatisticType type, int parameter = 0);

    long GetStatisticValueLong(StatisticType type, int parameter = 0);

    int GetStatisticValue(BufferLookup<CityStatistic> stats, StatisticType type, int parameter = 0);

    long GetStatisticValueLong(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0);

    NativeArray<CityStatistic> GetStatisticArray(StatisticType type, int parameter = 0);

    NativeArray<int> GetStatisticDataArray(StatisticType type, int parameter = 0);

    NativeArray<int> GetStatisticDataArray(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0);

    NativeArray<long> GetStatisticDataArrayLong(StatisticType type, int parameter = 0);

    NativeArray<long> GetStatisticDataArrayLong(
      BufferLookup<CityStatistic> stats,
      StatisticType type,
      int parameter = 0);

    NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> GetLookup();

    System.Action eventStatisticsUpdated { get; set; }

    void CompleteWriters();

    int sampleCount { get; }

    uint GetSampleFrameIndex(int index);
  }
}
