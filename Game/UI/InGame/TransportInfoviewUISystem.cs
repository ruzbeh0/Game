// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TransportInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class TransportInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "transportInfo";
    private UnlockSystem m_UnlockSystem;
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_ConfigQuery;
    private EntityQuery m_LineQuery;
    private EntityQuery m_ModifiedLineQuery;
    private RawValueBinding m_Summaries;
    private UITransportConfigurationPrefab m_Config;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_UnlockSystem = this.World.GetOrCreateSystemManaged<UnlockSystem>();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      this.m_LineQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Route>(),
          ComponentType.ReadWrite<TransportLine>(),
          ComponentType.ReadOnly<RouteWaypoint>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.m_ModifiedLineQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Route>(),
          ComponentType.ReadWrite<TransportLine>(),
          ComponentType.ReadOnly<RouteWaypoint>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.AddBinding((IBinding) (this.m_Summaries = new RawValueBinding("transportInfo", "summaries", new Action<IJsonWriter>(this.BindSummaries))));
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      if (!this.Enabled)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_Config = this.m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(this.m_ConfigQuery);
    }

    protected override bool Active => base.Active || this.m_Summaries.active;

    protected override bool Modified => !this.m_ModifiedLineQuery.IsEmptyIgnoreFilter;

    protected override void PerformUpdate() => this.m_Summaries.Update();

    private void BindSummaries(IJsonWriter writer)
    {
      NativeArray<UITransportLineData> sortedLines = TransportUIUtils.GetSortedLines(this.m_LineQuery, this.EntityManager, this.m_PrefabSystem);
      writer.TypeBegin(this.GetType().FullName + "+TransportSummaries");
      writer.PropertyName("passengerSummaries");
      writer.ArrayBegin(this.m_Config.m_PassengerSummaryItems.Length);
      foreach (UITransportSummaryItem passengerSummaryItem in this.m_Config.m_PassengerSummaryItems)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        new TransportInfoviewUISystem.PassengerSummary(this.m_PrefabSystem.GetEntity(passengerSummaryItem.m_Unlockable), Enum.GetName(typeof (TransportType), (object) passengerSummaryItem.m_Type), passengerSummaryItem.m_Icon, this.m_UnlockSystem.IsLocked(passengerSummaryItem.m_Unlockable), passengerSummaryItem.m_ShowLines ? TransportUIUtils.CountLines(sortedLines, passengerSummaryItem.m_Type) : 0, this.m_CityStatisticsSystem.GetStatisticValue(passengerSummaryItem.m_Statistic, 1), this.m_CityStatisticsSystem.GetStatisticValue(passengerSummaryItem.m_Statistic, 0)).Write(this.m_PrefabUISystem, writer);
      }
      writer.ArrayEnd();
      writer.PropertyName("cargoSummaries");
      writer.ArrayBegin(this.m_Config.m_CargoSummaryItems.Length);
      foreach (UITransportSummaryItem cargoSummaryItem in this.m_Config.m_CargoSummaryItems)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        new TransportInfoviewUISystem.CargoSummary(this.m_PrefabSystem.GetEntity(cargoSummaryItem.m_Unlockable), Enum.GetName(typeof (TransportType), (object) cargoSummaryItem.m_Type), cargoSummaryItem.m_Icon, this.m_UnlockSystem.IsLocked(cargoSummaryItem.m_Unlockable), cargoSummaryItem.m_ShowLines ? TransportUIUtils.CountLines(sortedLines, cargoSummaryItem.m_Type, true) : 0, this.m_CityStatisticsSystem.GetStatisticValue(cargoSummaryItem.m_Statistic, 0)).Write(this.m_PrefabUISystem, writer);
      }
      writer.ArrayEnd();
      writer.TypeEnd();
    }

    [Preserve]
    public TransportInfoviewUISystem()
    {
    }

    public readonly struct PassengerSummary
    {
      private readonly Entity m_Prefab;

      public string id { get; }

      public string icon { get; }

      public bool locked { get; }

      public int lineCount { get; }

      public int touristCount { get; }

      public int citizenCount { get; }

      public PassengerSummary(
        Entity prefab,
        string id,
        string icon,
        bool locked,
        int lineCount,
        int touristCount,
        int citizenCount)
      {
        this.m_Prefab = prefab;
        this.id = id;
        this.icon = icon;
        this.locked = locked;
        this.lineCount = lineCount;
        this.touristCount = touristCount;
        this.citizenCount = citizenCount;
      }

      public void Write(PrefabUISystem prefabUISystem, IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("id");
        writer.Write(this.id);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.PropertyName("locked");
        writer.Write(this.locked);
        writer.PropertyName("lineCount");
        writer.Write(this.lineCount);
        writer.PropertyName("touristCount");
        writer.Write(this.touristCount);
        writer.PropertyName("citizenCount");
        writer.Write(this.citizenCount);
        writer.PropertyName("requirements");
        // ISSUE: reference to a compiler-generated method
        prefabUISystem.BindPrefabRequirements(writer, this.m_Prefab);
        writer.TypeEnd();
      }
    }

    public readonly struct CargoSummary
    {
      private readonly Entity m_Prefab;

      public string id { get; }

      public string icon { get; }

      public bool locked { get; }

      public int lineCount { get; }

      public int cargoCount { get; }

      public CargoSummary(
        Entity prefab,
        string id,
        string icon,
        bool locked,
        int lineCount,
        int cargoCount)
      {
        this.m_Prefab = prefab;
        this.id = id;
        this.icon = icon;
        this.locked = locked;
        this.lineCount = lineCount;
        this.cargoCount = cargoCount;
      }

      public void Write(PrefabUISystem prefabUISystem, IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("id");
        writer.Write(this.id);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.PropertyName("locked");
        writer.Write(this.locked);
        writer.PropertyName("lineCount");
        writer.Write(this.lineCount);
        writer.PropertyName("cargoCount");
        writer.Write(this.cargoCount);
        writer.PropertyName("requirements");
        // ISSUE: reference to a compiler-generated method
        prefabUISystem.BindPrefabRequirements(writer, this.m_Prefab);
        writer.TypeEnd();
      }
    }
  }
}
