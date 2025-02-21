// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.OutsideTradeParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct OutsideTradeParameterData : IComponentData, IQueryTypeParameter
  {
    public float m_ElectricityImportPrice;
    public float m_ElectricityExportPrice;
    public float m_WaterImportPrice;
    public float m_WaterExportPrice;
    public float m_WaterExportPollutionTolerance;
    public float m_SewageExportPrice;
    public float m_AirWeightMultiplier;
    public float m_RoadWeightMultiplier;
    public float m_TrainWeightMultiplier;
    public float m_ShipWeightMultiplier;
    public float m_AirDistanceMultiplier;
    public float m_RoadDistanceMultiplier;
    public float m_TrainDistanceMultiplier;
    public float m_ShipDistanceMultiplier;
    public float m_AmbulanceImportServiceFee;
    public float m_HearseImportServiceFee;
    public float m_FireEngineImportServiceFee;
    public float m_GarbageImportServiceFee;
    public float m_PoliceImportServiceFee;
    public int m_OCServiceTradePopulationRange;

    private float GetDistanceCostSingle(OutsideConnectionTransferType type)
    {
      switch (type)
      {
        case OutsideConnectionTransferType.Road:
          return this.m_RoadDistanceMultiplier;
        case OutsideConnectionTransferType.Train:
          return this.m_TrainDistanceMultiplier;
        case OutsideConnectionTransferType.Air:
          return this.m_AirDistanceMultiplier;
        case OutsideConnectionTransferType.Ship:
          return this.m_ShipDistanceMultiplier;
        default:
          return 0.0f;
      }
    }

    public float GetDistanceCost(OutsideConnectionTransferType type)
    {
      float x = float.MaxValue;
      for (int index = 1; index < 32; index <<= 1)
        x = math.min(x, this.GetDistanceCostSingle(type));
      return x;
    }

    private float GetWeightCostSingle(OutsideConnectionTransferType type)
    {
      switch (type)
      {
        case OutsideConnectionTransferType.Road:
          return this.m_RoadWeightMultiplier;
        case OutsideConnectionTransferType.Train:
          return this.m_TrainWeightMultiplier;
        case OutsideConnectionTransferType.Air:
          return this.m_AirWeightMultiplier;
        case OutsideConnectionTransferType.Ship:
          return this.m_ShipWeightMultiplier;
        default:
          return 0.0f;
      }
    }

    public float GetWeightCost(OutsideConnectionTransferType type)
    {
      float x = float.MaxValue;
      for (int index = 1; index < 32; index <<= 1)
        x = math.min(x, this.GetWeightCostSingle(type));
      return x;
    }

    public float GetFee(PlayerResource resource, bool export = false)
    {
      switch (resource)
      {
        case PlayerResource.Electricity:
          return !export ? this.m_ElectricityImportPrice : this.m_ElectricityExportPrice;
        case PlayerResource.Water:
          return !export ? this.m_WaterImportPrice : this.m_WaterExportPrice;
        case PlayerResource.Sewage:
          return !export ? this.m_SewageExportPrice : 0.0f;
        default:
          return 0.0f;
      }
    }

    public bool Importable(PlayerResource resource) => (double) this.GetFee(resource) != 0.0;

    public bool Exportable(PlayerResource resource) => (double) this.GetFee(resource, true) != 0.0;
  }
}
