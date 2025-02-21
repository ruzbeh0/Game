// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TransportationOverviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TransportationOverviewUISystem : UISystemBase
  {
    private const string kGroup = "transportationOverview";
    private NameSystem m_NameSystem;
    private UnlockSystem m_UnlockSystem;
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private PoliciesUISystem m_PoliciesUISystem;
    private SelectedInfoUISystem m_SelectedInfoUISystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private Entity m_OutOfServicePolicy;
    private Entity m_DayRoutePolicy;
    private Entity m_NightRoutePolicy;
    private EntityQuery m_ConfigQuery;
    private EntityQuery m_LineQuery;
    private EntityQuery m_ModifiedLineQuery;
    private EntityQuery m_UnlockQuery;
    private EntityArchetype m_ColorUpdateArchetype;
    private RawValueBinding m_TransportLines;
    private RawValueBinding m_PassengerTypes;
    private RawValueBinding m_CargoTypes;
    private ValueBinding<string> m_SelectedCargoType;
    private ValueBinding<string> m_SelectedPassengerType;
    private UITransportConfigurationPrefab m_Config;
    private UIUpdateState m_UpdateState;

    public override GameMode gameMode => GameMode.Game;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockSystem = this.World.GetOrCreateSystemManaged<UnlockSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ColorUpdateArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<ColorUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TransportLines = new RawValueBinding("transportationOverview", "lines", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeArray<UITransportLineData> sortedLines = TransportUIUtils.GetSortedLines(this.m_LineQuery, this.EntityManager, this.m_PrefabSystem);
        binder.ArrayBegin(sortedLines.Length);
        for (int index = 0; index < sortedLines.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.BindLine(sortedLines[index], binder);
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_PassengerTypes = new RawValueBinding("transportationOverview", "passengerTypes", (Action<IJsonWriter>) (writer => this.BindTypes(writer, this.m_Config.m_PassengerLineTypes)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_CargoTypes = new RawValueBinding("transportationOverview", "cargoTypes", (Action<IJsonWriter>) (writer => this.BindTypes(writer, this.m_Config.m_CargoLineTypes)))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedPassengerType = new ValueBinding<string>("transportationOverview", "selectedPassengerType", Enum.GetName(typeof (TransportType), (object) TransportType.Bus))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedCargoType = new ValueBinding<string>("transportationOverview", "selectedCargoType", "None")));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("transportationOverview", "delete", (Action<Entity>) (entity =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Deleted>(entity, new Deleted());
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("transportationOverview", "select", (Action<Entity>) (entity =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_SelectedInfoUISystem.SetSelection(entity);
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, Color32>("transportationOverview", "setColor", (Action<Entity, Color32>) ((entity, color) =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        commandBuffer.SetComponent<Game.Routes.Color>(entity, new Game.Routes.Color(color));
        DynamicBuffer<RouteVehicle> buffer;
        if (this.EntityManager.TryGetBuffer<RouteVehicle>(entity, true, out buffer))
        {
          for (int index = 0; index < buffer.Length; ++index)
            commandBuffer.AddComponent<Game.Routes.Color>(buffer[index].m_Vehicle, new Game.Routes.Color(color));
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = commandBuffer.CreateEntity(this.m_ColorUpdateArchetype);
        commandBuffer.SetComponent<ColorUpdated>(entity1, new ColorUpdated(entity));
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, string>("transportationOverview", "rename", (Action<Entity, string>) ((entity, name) =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.SetCustomName(entity, name);
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool>("transportationOverview", "setActive", (Action<Entity, bool>) ((entity, state) =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.SetPolicy(entity, this.m_OutOfServicePolicy, !state);
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool>("transportationOverview", "showLine", (Action<Entity, bool>) ((entity, hideOthers) =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        if (hideOthers)
        {
          // ISSUE: reference to a compiler-generated field
          commandBuffer.AddComponent<HiddenRoute>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
        }
        commandBuffer.RemoveComponent<HiddenRoute>(entity);
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool>("transportationOverview", "hideLine", (Action<Entity, bool>) ((entity, showOthers) =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        if (showOthers)
        {
          // ISSUE: reference to a compiler-generated field
          commandBuffer.RemoveComponent<HiddenRoute>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
        }
        commandBuffer.AddComponent<HiddenRoute>(entity);
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity, int>("transportationOverview", "setSchedule", (Action<Entity, int>) ((entity, schedule) =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        switch ((RouteSchedule) schedule)
        {
          case RouteSchedule.Day:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(entity, this.m_NightRoutePolicy, false);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(entity, this.m_DayRoutePolicy, true);
            break;
          case RouteSchedule.Night:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(entity, this.m_NightRoutePolicy, true);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(entity, this.m_DayRoutePolicy, false);
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(entity, this.m_NightRoutePolicy, false);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PoliciesUISystem.SetPolicy(entity, this.m_DayRoutePolicy, false);
            break;
        }
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding("transportationOverview", "resetVisibility", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        // ISSUE: reference to a compiler-generated field
        commandBuffer.RemoveComponent<HiddenRoute>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
        // ISSUE: reference to a compiler-generated field
        commandBuffer.RemoveComponent<Highlighted>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("transportationOverview", "toggleHighlight", (Action<Entity>) (entity =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        if (!this.EntityManager.HasComponent<Highlighted>(entity))
          commandBuffer.AddComponent<Highlighted>(entity);
        else
          commandBuffer.RemoveComponent<Highlighted>(entity);
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string>("transportationOverview", "setSelectedPassengerType", (Action<string>) (type => this.m_SelectedPassengerType.Update(type))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string>("transportationOverview", "setSelectedCargoType", (Action<string>) (type => this.m_SelectedCargoType.Update(type))));
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateState = UIUpdateState.Create(this.World, 256);
    }

    private string GetInitialSelectedType()
    {
      // ISSUE: reference to a compiler-generated field
      foreach (UITransportItem cargoLineType in this.m_Config.m_CargoLineTypes)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_UnlockSystem.IsLocked(cargoLineType.m_Unlockable))
          return Enum.GetName(typeof (TransportType), (object) cargoLineType.m_Type);
      }
      return Enum.GetName(typeof (TransportType), (object) TransportType.None);
    }

    private void SetSelectedPassengerType(string type) => this.m_SelectedPassengerType.Update(type);

    private void SetSelectedCargoType(string type) => this.m_SelectedCargoType.Update(type);

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      if (!this.Enabled)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_Config = this.m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(this.m_ConfigQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_OutOfServicePolicy = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Config.m_OutOfServicePolicy);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DayRoutePolicy = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Config.m_DayRoutePolicy);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NightRoutePolicy = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Config.m_NightRoutePolicy);
      // ISSUE: reference to a compiler-generated field
      this.m_CargoTypes.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_PassengerTypes.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportLines.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SelectedCargoType.Update(this.GetInitialSelectedType());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ModifiedLineQuery.IsEmptyIgnoreFilter || this.m_UpdateState.Advance())
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TransportLines.Update();
      }
      // ISSUE: reference to a compiler-generated field
      if (!PrefabUtils.HasUnlockedPrefab<RouteData>(this.EntityManager, this.m_UnlockQuery))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_CargoTypes.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_PassengerTypes.Update();
    }

    public void RequestUpdate() => this.m_UpdateState.ForceUpdate();

    private void DeleteLine(Entity entity)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Deleted>(entity, new Deleted());
    }

    private void SelectLine(Entity entity)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SelectedInfoUISystem.SetSelection(entity);
    }

    private void SetLineColor(Entity entity, Color32 color)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      commandBuffer.SetComponent<Game.Routes.Color>(entity, new Game.Routes.Color(color));
      DynamicBuffer<RouteVehicle> buffer;
      if (this.EntityManager.TryGetBuffer<RouteVehicle>(entity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
          commandBuffer.AddComponent<Game.Routes.Color>(buffer[index].m_Vehicle, new Game.Routes.Color(color));
      }
      // ISSUE: reference to a compiler-generated field
      Entity entity1 = commandBuffer.CreateEntity(this.m_ColorUpdateArchetype);
      commandBuffer.SetComponent<ColorUpdated>(entity1, new ColorUpdated(entity));
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    private void SetLineName(Entity entity, string name)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.SetCustomName(entity, name);
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    public void SetLineState(Entity entity, bool state)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PoliciesUISystem.SetPolicy(entity, this.m_OutOfServicePolicy, !state);
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    private void SetLineSchedule(Entity entity, int schedule)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      switch ((RouteSchedule) schedule)
      {
        case RouteSchedule.Day:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(entity, this.m_NightRoutePolicy, false);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(entity, this.m_DayRoutePolicy, true);
          break;
        case RouteSchedule.Night:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(entity, this.m_NightRoutePolicy, true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(entity, this.m_DayRoutePolicy, false);
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(entity, this.m_NightRoutePolicy, false);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetPolicy(entity, this.m_DayRoutePolicy, false);
          break;
      }
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    public void ShowLine(Entity entity, bool hideOthers)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      if (hideOthers)
      {
        // ISSUE: reference to a compiler-generated field
        commandBuffer.AddComponent<HiddenRoute>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
      }
      commandBuffer.RemoveComponent<HiddenRoute>(entity);
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    public void HideLine(Entity entity, bool showOthers)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      if (showOthers)
      {
        // ISSUE: reference to a compiler-generated field
        commandBuffer.RemoveComponent<HiddenRoute>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
      }
      commandBuffer.AddComponent<HiddenRoute>(entity);
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    public void ToggleHighlight(Entity entity)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      if (!this.EntityManager.HasComponent<Highlighted>(entity))
        commandBuffer.AddComponent<Highlighted>(entity);
      else
        commandBuffer.RemoveComponent<Highlighted>(entity);
    }

    public void ResetLinesVisibility()
    {
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      commandBuffer.RemoveComponent<HiddenRoute>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
      // ISSUE: reference to a compiler-generated field
      commandBuffer.RemoveComponent<Highlighted>(this.m_LineQuery, EntityQueryCaptureMode.AtPlayback);
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    private void BindPassengerTypes(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.BindTypes(writer, this.m_Config.m_PassengerLineTypes);
    }

    private void BindCargoTypes(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.BindTypes(writer, this.m_Config.m_CargoLineTypes);
    }

    private void BindTypes(IJsonWriter writer, UITransportItem[] items)
    {
      writer.ArrayBegin(items.Length);
      foreach (UITransportItem uiTransportItem in items)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        new UITransportType(this.m_PrefabSystem.GetEntity(uiTransportItem.m_Unlockable), Enum.GetName(typeof (TransportType), (object) uiTransportItem.m_Type), uiTransportItem.m_Icon, this.m_UnlockSystem.IsLocked(uiTransportItem.m_Unlockable)).Write(this.m_PrefabUISystem, writer);
      }
      writer.ArrayEnd();
    }

    private void BindLines(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      NativeArray<UITransportLineData> sortedLines = TransportUIUtils.GetSortedLines(this.m_LineQuery, this.EntityManager, this.m_PrefabSystem);
      binder.ArrayBegin(sortedLines.Length);
      for (int index = 0; index < sortedLines.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindLine(sortedLines[index], binder);
      }
      binder.ArrayEnd();
    }

    private void BindLine(UITransportLineData lineData, IJsonWriter binder)
    {
      binder.TypeBegin("Game.UI.InGame.UITransportLine");
      binder.PropertyName("name");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindName(binder, lineData.entity);
      binder.PropertyName("vkName");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindNameForVirtualKeyboard(binder, lineData.entity);
      binder.PropertyName(nameof (lineData));
      binder.Write<UITransportLineData>(lineData);
      binder.TypeEnd();
    }

    [Preserve]
    public TransportationOverviewUISystem()
    {
    }
  }
}
