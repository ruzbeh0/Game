// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ITaxSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Economy;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public interface ITaxSystem
  {
    TaxParameterData GetTaxParameterData();

    int TaxRate { get; set; }

    int GetTaxRate(TaxAreaType areaType);

    void SetTaxRate(TaxAreaType areaType, int rate);

    int2 GetTaxRateRange(TaxAreaType areaType);

    JobHandle Readers { get; }

    int GetResidentialTaxRate(int jobLevel);

    void SetResidentialTaxRate(int jobLevel, int rate);

    int GetCommercialTaxRate(Resource resource);

    void SetCommercialTaxRate(Resource resource, int rate);

    int GetIndustrialTaxRate(Resource resource);

    void SetIndustrialTaxRate(Resource resource, int rate);

    int GetOfficeTaxRate(Resource resource);

    void SetOfficeTaxRate(Resource resource, int rate);

    int GetTaxRateEffect(TaxAreaType areaType, int taxRate);

    int GetEstimatedTaxAmount(
      TaxAreaType areaType,
      TaxResultType resultType,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats);

    int GetEstimatedResidentialTaxIncome(
      int jobLevel,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats);

    int GetEstimatedCommercialTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats);

    int GetEstimatedIndustrialTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats);

    int GetEstimatedOfficeTaxIncome(
      Resource resource,
      NativeParallelHashMap<CityStatisticsSystem.StatisticsKey, Entity> statisticsLookup,
      BufferLookup<CityStatistic> stats);
  }
}
