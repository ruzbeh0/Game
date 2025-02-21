// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.OutsideTradeParameterPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new System.Type[] {})]
  public class OutsideTradeParameterPrefab : PrefabBase
  {
    [Header("Electricity")]
    [Tooltip("Expense for importing 0.1 kW of electricity for 24h")]
    public float m_ElectricityImportPrice;
    [Tooltip("Revenue for exporting 0.1 kW of electricity for 24h")]
    public float m_ElectricityExportPrice;
    [Header("Water & Sewage")]
    [Tooltip("Expense for importing 1m^3 of water for 24h")]
    public float m_WaterImportPrice;
    [Tooltip("Revenue for exporting 1m^3 of water for 24h")]
    public float m_WaterExportPrice;
    [Tooltip("Percentage of pollution when the water export revenue becomes zero")]
    [Range(0.0f, 1f)]
    public float m_WaterExportPollutionTolerance = 0.1f;
    [Tooltip("Expense for importing 1m^3 of sewage for 24h")]
    public float m_SewageExportPrice;
    [Header("Resource Trade")]
    public float m_AirWeightMultiplier;
    public float m_RoadWeightMultiplier;
    public float m_TrainWeightMultiplier;
    public float m_ShipWeightMultiplier;
    public float m_AirDistanceMultiplier;
    public float m_RoadDistanceMultiplier;
    public float m_TrainDistanceMultiplier;
    public float m_ShipDistanceMultiplier;
    [Tooltip("Service fees for ambulance import service, multiply by population")]
    public float m_AmbulanceImportServiceFee = 1f;
    [Tooltip("Service fees for Hearse import service, multiply by population")]
    public float m_HearseImportServiceFee = 1f;
    [Tooltip("Service fees for FireEngine import service, multiply by population")]
    public float m_FireEngineImportServiceFee = 1f;
    [Tooltip("Service fees for Garbage import service, multiply by population")]
    public float m_GarbageImportServiceFee = 1f;
    [Tooltip("Service fees for Police import service, multiply by population")]
    public float m_PoliceImportServiceFee = 1f;
    [Tooltip("Service fees from outside will change based on this population range,0 - 1000 => service fee * 1000, 1000 - 2000 => service fee * 2000")]
    public int m_OCServiceTradePopulationRange = 1000;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<OutsideTradeParameterData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<OutsideTradeParameterData>(entity, new OutsideTradeParameterData()
      {
        m_ElectricityImportPrice = this.m_ElectricityImportPrice,
        m_ElectricityExportPrice = this.m_ElectricityExportPrice,
        m_WaterImportPrice = this.m_WaterImportPrice,
        m_WaterExportPrice = this.m_WaterExportPrice,
        m_WaterExportPollutionTolerance = this.m_WaterExportPollutionTolerance,
        m_SewageExportPrice = this.m_SewageExportPrice,
        m_AirDistanceMultiplier = this.m_AirDistanceMultiplier,
        m_RoadDistanceMultiplier = this.m_RoadDistanceMultiplier,
        m_TrainDistanceMultiplier = this.m_TrainDistanceMultiplier,
        m_ShipDistanceMultiplier = this.m_ShipDistanceMultiplier,
        m_AirWeightMultiplier = this.m_AirWeightMultiplier,
        m_RoadWeightMultiplier = this.m_RoadWeightMultiplier,
        m_TrainWeightMultiplier = this.m_TrainWeightMultiplier,
        m_ShipWeightMultiplier = this.m_ShipWeightMultiplier,
        m_AmbulanceImportServiceFee = this.m_AmbulanceImportServiceFee,
        m_HearseImportServiceFee = this.m_HearseImportServiceFee,
        m_FireEngineImportServiceFee = this.m_FireEngineImportServiceFee,
        m_GarbageImportServiceFee = this.m_GarbageImportServiceFee,
        m_PoliceImportServiceFee = this.m_PoliceImportServiceFee,
        m_OCServiceTradePopulationRange = this.m_OCServiceTradePopulationRange
      });
    }
  }
}
