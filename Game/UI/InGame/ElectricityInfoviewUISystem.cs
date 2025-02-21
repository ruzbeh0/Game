// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ElectricityInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ElectricityInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "electricityInfo";
    private ElectricityStatisticsSystem m_ElectricityStatisticsSystem;
    private ElectricityTradeSystem m_ElectricityTradeSystem;
    private EntityQuery m_OutsideTradeParameterGroup;
    private GetterValueBinding<int> m_ElectricityProduction;
    private GetterValueBinding<int> m_ElectricityConsumption;
    private GetterValueBinding<int> m_ElectricityTransmitted;
    private GetterValueBinding<int> m_ElectricityExport;
    private GetterValueBinding<int> m_ElectricityImport;
    private GetterValueBinding<IndicatorValue> m_ElectricityAvailability;
    private GetterValueBinding<IndicatorValue> m_ElectricityTransmission;
    private GetterValueBinding<IndicatorValue> m_ElectricityTrade;
    private GetterValueBinding<IndicatorValue> m_BatteryCharge;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityStatisticsSystem = this.World.GetOrCreateSystemManaged<ElectricityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityTradeSystem = this.World.GetOrCreateSystemManaged<ElectricityTradeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideTradeParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<OutsideTradeParameterData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityProduction = new GetterValueBinding<int>("electricityInfo", "electricityProduction", (Func<int>) (() => this.m_ElectricityStatisticsSystem.production))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityConsumption = new GetterValueBinding<int>("electricityInfo", "electricityConsumption", (Func<int>) (() => this.m_ElectricityStatisticsSystem.consumption))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityTransmitted = new GetterValueBinding<int>("electricityInfo", "electricityTransmitted", (Func<int>) (() => this.m_ElectricityStatisticsSystem.fulfilledConsumption))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityExport = new GetterValueBinding<int>("electricityInfo", "electricityExport", (Func<int>) (() => this.m_ElectricityTradeSystem.export))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityImport = new GetterValueBinding<int>("electricityInfo", "electricityImport", (Func<int>) (() => this.m_ElectricityTradeSystem.import))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityAvailability = new GetterValueBinding<IndicatorValue>("electricityInfo", "electricityAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_ElectricityStatisticsSystem.production, (float) this.m_ElectricityStatisticsSystem.consumption)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityTransmission = new GetterValueBinding<IndicatorValue>("electricityInfo", "electricityTransmission", (Func<IndicatorValue>) (() => new IndicatorValue(0.0f, (float) this.m_ElectricityStatisticsSystem.consumption, (float) this.m_ElectricityStatisticsSystem.fulfilledConsumption)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ElectricityTrade = new GetterValueBinding<IndicatorValue>("electricityInfo", "electricityTrade", (Func<IndicatorValue>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OutsideTradeParameterGroup.IsEmptyIgnoreFilter)
          return new IndicatorValue(-1f, 1f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        OutsideTradeParameterData singleton = this.m_OutsideTradeParameterGroup.GetSingleton<OutsideTradeParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return new IndicatorValue(-1f, 1f, math.clamp((float) ((double) this.m_ElectricityTradeSystem.export * (double) singleton.m_ElectricityExportPrice - (double) this.m_ElectricityTradeSystem.import * (double) singleton.m_ElectricityImportPrice) / math.max(0.01f, (float) this.m_ElectricityStatisticsSystem.consumption * singleton.m_ElectricityExportPrice), -1f, 1f));
      }), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_BatteryCharge = new GetterValueBinding<IndicatorValue>("electricityInfo", "batteryCharge", (Func<IndicatorValue>) (() => new IndicatorValue(0.0f, (float) this.m_ElectricityStatisticsSystem.batteryCapacity, (float) this.m_ElectricityStatisticsSystem.batteryCharge)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_ElectricityProduction.active || this.m_ElectricityConsumption.active || this.m_ElectricityTransmitted.active || this.m_ElectricityExport.active || this.m_ElectricityImport.active || this.m_ElectricityAvailability.active || this.m_ElectricityTransmission.active || this.m_ElectricityTrade.active || this.m_BatteryCharge.active;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityProduction.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityConsumption.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityTransmitted.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityExport.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityImport.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityTransmission.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityTrade.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCharge.Update();
    }

    private IndicatorValue GetElectricityTransmission()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new IndicatorValue(0.0f, (float) this.m_ElectricityStatisticsSystem.consumption, (float) this.m_ElectricityStatisticsSystem.fulfilledConsumption);
    }

    private IndicatorValue GetElectricityAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_ElectricityStatisticsSystem.production, (float) this.m_ElectricityStatisticsSystem.consumption);
    }

    private IndicatorValue GetElectricityTrade()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_OutsideTradeParameterGroup.IsEmptyIgnoreFilter)
        return new IndicatorValue(-1f, 1f, 0.0f);
      // ISSUE: reference to a compiler-generated field
      OutsideTradeParameterData singleton = this.m_OutsideTradeParameterGroup.GetSingleton<OutsideTradeParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new IndicatorValue(-1f, 1f, math.clamp((float) ((double) this.m_ElectricityTradeSystem.export * (double) singleton.m_ElectricityExportPrice - (double) this.m_ElectricityTradeSystem.import * (double) singleton.m_ElectricityImportPrice) / math.max(0.01f, (float) this.m_ElectricityStatisticsSystem.consumption * singleton.m_ElectricityExportPrice), -1f, 1f));
    }

    private IndicatorValue GetBatteryCharge()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new IndicatorValue(0.0f, (float) this.m_ElectricityStatisticsSystem.batteryCapacity, (float) this.m_ElectricityStatisticsSystem.batteryCharge);
    }

    [Preserve]
    public ElectricityInfoviewUISystem()
    {
    }
  }
}
