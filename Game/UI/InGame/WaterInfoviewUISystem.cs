// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.WaterInfoviewUISystem
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
  public class WaterInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "waterInfo";
    private WaterStatisticsSystem m_WaterStatisticsSystem;
    private GetterValueBinding<int> m_WaterCapacity;
    private WaterTradeSystem m_WaterTradeSystem;
    private EntityQuery m_OutsideTradeParameterGroup;
    private GetterValueBinding<int> m_WaterConsumption;
    private GetterValueBinding<int> m_SewageCapacity;
    private GetterValueBinding<int> m_SewageConsumption;
    private GetterValueBinding<int> m_WaterExport;
    private GetterValueBinding<IndicatorValue> m_WaterAvailability;
    private GetterValueBinding<int> m_WaterImport;
    private GetterValueBinding<IndicatorValue> m_SewageAvailability;
    private GetterValueBinding<int> m_SewageExport;
    private GetterValueBinding<IndicatorValue> m_WaterTrade;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterStatisticsSystem = this.World.GetOrCreateSystemManaged<WaterStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterTradeSystem = this.World.GetOrCreateSystemManaged<WaterTradeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideTradeParameterGroup = this.GetEntityQuery(ComponentType.ReadOnly<OutsideTradeParameterData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WaterCapacity = new GetterValueBinding<int>("waterInfo", "waterCapacity", (Func<int>) (() => this.m_WaterStatisticsSystem.freshCapacity))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WaterConsumption = new GetterValueBinding<int>("waterInfo", "waterConsumption", (Func<int>) (() => this.m_WaterStatisticsSystem.freshConsumption))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SewageCapacity = new GetterValueBinding<int>("waterInfo", "sewageCapacity", (Func<int>) (() => this.m_WaterStatisticsSystem.sewageCapacity))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SewageConsumption = new GetterValueBinding<int>("waterInfo", "sewageConsumption", (Func<int>) (() => this.m_WaterStatisticsSystem.sewageConsumption))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WaterExport = new GetterValueBinding<int>("waterInfo", "waterExport", (Func<int>) (() => this.m_WaterTradeSystem.freshExport))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WaterImport = new GetterValueBinding<int>("waterInfo", "waterImport", (Func<int>) (() => this.m_WaterTradeSystem.freshImport))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SewageExport = new GetterValueBinding<int>("waterInfo", "sewageExport", (Func<int>) (() => this.m_WaterTradeSystem.sewageExport))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WaterAvailability = new GetterValueBinding<IndicatorValue>("waterInfo", "waterAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_WaterStatisticsSystem.freshCapacity, (float) this.m_WaterStatisticsSystem.freshConsumption)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SewageAvailability = new GetterValueBinding<IndicatorValue>("waterInfo", "sewageAvailability", (Func<IndicatorValue>) (() => IndicatorValue.Calculate((float) this.m_WaterStatisticsSystem.sewageCapacity, (float) this.m_WaterStatisticsSystem.sewageConsumption)), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_WaterTrade = new GetterValueBinding<IndicatorValue>("waterInfo", "waterTrade", (Func<IndicatorValue>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OutsideTradeParameterGroup.IsEmptyIgnoreFilter)
          return new IndicatorValue(-1f, 1f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        OutsideTradeParameterData singleton = this.m_OutsideTradeParameterGroup.GetSingleton<OutsideTradeParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return new IndicatorValue(-1f, 1f, math.clamp((float) ((double) this.m_WaterTradeSystem.freshExport * (double) singleton.m_WaterExportPrice - (double) this.m_WaterTradeSystem.freshImport * (double) singleton.m_WaterImportPrice) / math.max(0.01f, (float) this.m_WaterStatisticsSystem.freshConsumption * singleton.m_WaterExportPrice), -1f, 1f));
      }), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_WaterCapacity.active || this.m_WaterConsumption.active || this.m_SewageCapacity.active || this.m_SewageConsumption.active || this.m_WaterExport.active || this.m_WaterImport.active || this.m_SewageExport.active || this.m_WaterAvailability.active || this.m_SewageAvailability.active || this.m_WaterTrade.active;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WaterCapacity.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterConsumption.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageCapacity.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageConsumption.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterExport.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterImport.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageExport.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageAvailability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterTrade.Update();
    }

    private IndicatorValue GetWaterTrade()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_OutsideTradeParameterGroup.IsEmptyIgnoreFilter)
        return new IndicatorValue(-1f, 1f, 0.0f);
      // ISSUE: reference to a compiler-generated field
      OutsideTradeParameterData singleton = this.m_OutsideTradeParameterGroup.GetSingleton<OutsideTradeParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new IndicatorValue(-1f, 1f, math.clamp((float) ((double) this.m_WaterTradeSystem.freshExport * (double) singleton.m_WaterExportPrice - (double) this.m_WaterTradeSystem.freshImport * (double) singleton.m_WaterImportPrice) / math.max(0.01f, (float) this.m_WaterStatisticsSystem.freshConsumption * singleton.m_WaterExportPrice), -1f, 1f));
    }

    private IndicatorValue GetWaterAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_WaterStatisticsSystem.freshCapacity, (float) this.m_WaterStatisticsSystem.freshConsumption);
    }

    private IndicatorValue GetSewageAvailability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return IndicatorValue.Calculate((float) this.m_WaterStatisticsSystem.sewageCapacity, (float) this.m_WaterStatisticsSystem.sewageConsumption);
    }

    [Preserve]
    public WaterInfoviewUISystem()
    {
    }
  }
}
