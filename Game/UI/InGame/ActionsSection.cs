// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ActionsSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Audio;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Policies;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using Game.Triggers;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ActionsSection : InfoSectionBase
  {
    private ToolSystem m_ToolSystem;
    private AreaToolSystem m_AreaToolSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private UIInitializeSystem m_UIInitializeSystem;
    private PoliciesUISystem m_PoliciesUISystem;
    private LifePathEventSystem m_LifePathEventSystem;
    private GamePanelUISystem m_GamePanelUISystem;
    private TrafficRoutesSystem m_TrafficRoutesSystem;
    private AudioManager m_AudioManager;
    private EntityQuery m_SoundQuery;
    private EntityQuery m_RouteConfigQuery;
    private PolicyPrefab m_RouteOutOfServicePolicy;
    private PolicyPrefab m_BuildingOutOfServicePolicy;
    private PolicyPrefab m_EmptyingPolicy;
    private AreaPrefab m_LotPrefab;
    private bool m_EditingLot;
    private Color32[] m_TrafficRouteColors;
    private ValueBinding<bool> m_MovingBinding;
    private ValueBinding<bool> m_EditingLotBinding;
    private ValueBinding<bool> m_TrafficRoutesVisibleBinding;
    private ValueBinding<Color32[]> m_TrafficRouteColorsBinding;
    private RawValueBinding m_MoveableObjectName;

    protected override string group => nameof (ActionsSection);

    public bool editingLot => this.m_EditingLot;

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForOutsideConnections => true;

    protected override bool displayForUnderConstruction => true;

    protected override bool displayForUpgrades => true;

    private bool focusable { get; set; }

    private bool focusing { get; set; }

    private bool following { get; set; }

    private bool followable { get; set; }

    private bool moveable { get; set; }

    private bool deletable { get; set; }

    private bool disabled { get; set; }

    private bool disableable { get; set; }

    private bool hasTutorial { get; set; }

    private bool emptying { get; set; }

    private bool emptiable { get; set; }

    private bool hasLotTool { get; set; }

    private bool hasTrafficRoutes { get; set; }

    protected override void Reset()
    {
      this.focusable = false;
      this.focusing = false;
      this.following = false;
      this.followable = false;
      this.moveable = false;
      this.deletable = false;
      this.disabled = false;
      this.disableable = false;
      this.hasTutorial = false;
      this.emptying = false;
      this.emptiable = false;
      this.hasLotTool = false;
      this.hasTrafficRoutes = false;
      // ISSUE: reference to a compiler-generated field
      this.m_LotPrefab = (AreaPrefab) null;
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaToolSystem = this.World.GetOrCreateSystemManaged<AreaToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UIInitializeSystem = this.World.GetOrCreateSystemManaged<UIInitializeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LifePathEventSystem = this.World.GetOrCreateSystemManaged<LifePathEventSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GamePanelUISystem = this.World.GetOrCreateSystemManaged<GamePanelUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficRoutesSystem = this.World.GetOrCreateSystemManaged<TrafficRoutesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<RouteConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged += (Action<ToolBaseSystem>) (tool =>
      {
        // ISSUE: reference to a compiler-generated field
        if (tool != this.m_AreaToolSystem)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_EditingLot = false;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_MoveableObjectName.Update();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovingBinding.Update(tool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Move);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EditingLotBinding.Update(tool == this.m_AreaToolSystem && this.hasLotTool && this.m_EditingLot);
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding(this.group, "focus", (Action) (() => this.m_InfoUISystem.Focus(!this.focusing ? this.selectedEntity : Entity.Null))));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleMove", (Action) (() =>
      {
        if (!this.moveable)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ObjectToolSystem.StartMoving(this.selectedEntity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectToolSystem;
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "follow", (Action) (() =>
      {
        if (!this.EntityManager.HasComponent<Followed>(this.selectedEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_LifePathEventSystem.FollowCitizen(this.selectedEntity);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_GamePanelUISystem.ShowPanel<LifePathPanel>(this.selectedEntity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_LifePathEventSystem.UnfollowCitizen(this.selectedEntity);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoUISystem.SetDirty();
      })));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "delete", (Action) (() =>
      {
        if (!this.EntityManager.Exists(this.selectedEntity))
          return;
        if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_BulldozeSound);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_DeletetEntitySound);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Deleted>(this.selectedEntity);
      })));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggle", (Action) (() =>
      {
        if (this.EntityManager.HasComponent<Route>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) && this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetSelectedInfoPolicy(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_RouteOutOfServicePolicy), !this.disabled);
        }
        else
        {
          if ((!this.EntityManager.HasComponent<Building>(this.selectedEntity) || !this.EntityManager.HasComponent<Policy>(this.selectedEntity) || !this.EntityManager.HasComponent<CityServiceUpkeep>(this.selectedEntity) || !this.EntityManager.HasComponent<Efficiency>(this.selectedEntity)) && !this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.m_PoliciesUISystem.SetSelectedInfoPolicy(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_BuildingOutOfServicePolicy), !this.disabled);
        }
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleEmptying", (Action) (() => this.m_PoliciesUISystem.SetSelectedInfoPolicy(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_EmptyingPolicy), !this.emptying))));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleLotTool", (Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditingLot)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!((UnityEngine.Object) this.m_LotPrefab != (UnityEngine.Object) null))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_EditingLot = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AreaToolSystem.prefab = this.m_LotPrefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_AreaToolSystem;
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleTrafficRoutes", (Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TrafficRoutesSystem.routesVisible = !this.m_TrafficRoutesSystem.routesVisible;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoUISystem.SetDirty();
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MovingBinding = new ValueBinding<bool>(this.group, "moving", false)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_EditingLotBinding = new ValueBinding<bool>(this.group, "editingLot", false)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MoveableObjectName = new RawValueBinding(this.group, "moveableObjectName", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Move)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NameSystem.BindName(binder, this.m_InfoUISystem.selectedEntity);
        }
        else
          binder.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TrafficRouteColorsBinding = new ValueBinding<Color32[]>(this.group, "trafficRouteColors", this.m_TrafficRouteColors, (IWriter<Color32[]>) new ArrayWriter<Color32>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TrafficRoutesVisibleBinding = new ValueBinding<bool>(this.group, "trafficRoutesVisible", this.m_TrafficRoutesSystem.routesVisible)));
    }

    private void OnToggleTrafficRoutes()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficRoutesSystem.routesVisible = !this.m_TrafficRoutesSystem.routesVisible;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.SetDirty();
    }

    private void BindObjectName(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Move)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(binder, this.m_InfoUISystem.selectedEntity);
      }
      else
        binder.WriteNull();
    }

    private void OnToolChanged(ToolBaseSystem tool)
    {
      // ISSUE: reference to a compiler-generated field
      if (tool != this.m_AreaToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_EditingLot = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_MoveableObjectName.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MovingBinding.Update(tool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Move);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EditingLotBinding.Update(tool == this.m_AreaToolSystem && this.hasLotTool && this.m_EditingLot);
    }

    private void OnToggleLotTool()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_EditingLot)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (!((UnityEngine.Object) this.m_LotPrefab != (UnityEngine.Object) null))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_EditingLot = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaToolSystem.prefab = this.m_LotPrefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_AreaToolSystem;
      }
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      foreach (PolicyPrefab policy in this.m_UIInitializeSystem.policies)
      {
        switch (policy.name)
        {
          case "Route Out of Service":
            // ISSUE: reference to a compiler-generated field
            this.m_RouteOutOfServicePolicy = policy;
            continue;
          case "Out of Service":
            // ISSUE: reference to a compiler-generated field
            this.m_BuildingOutOfServicePolicy = policy;
            continue;
          case "Empty":
            // ISSUE: reference to a compiler-generated field
            this.m_EmptyingPolicy = policy;
            continue;
          default:
            continue;
        }
      }
    }

    private void OnToggle()
    {
      if (this.EntityManager.HasComponent<Route>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) && this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.SetSelectedInfoPolicy(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_RouteOutOfServicePolicy), !this.disabled);
      }
      else
      {
        if ((!this.EntityManager.HasComponent<Building>(this.selectedEntity) || !this.EntityManager.HasComponent<Policy>(this.selectedEntity) || !this.EntityManager.HasComponent<CityServiceUpkeep>(this.selectedEntity) || !this.EntityManager.HasComponent<Efficiency>(this.selectedEntity)) && !this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.SetSelectedInfoPolicy(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_BuildingOutOfServicePolicy), !this.disabled);
      }
    }

    private void OnToggleEmptying()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_PoliciesUISystem.SetSelectedInfoPolicy(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_EmptyingPolicy), !this.emptying);
    }

    private void OnDelete()
    {
      if (!this.EntityManager.Exists(this.selectedEntity))
        return;
      if (this.EntityManager.HasComponent<Building>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_BulldozeSound);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_DeletetEntitySound);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Deleted>(this.selectedEntity);
    }

    private void OnFocus()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.Focus(!this.focusing ? this.selectedEntity : Entity.Null);
    }

    private void OnToggleMove()
    {
      if (!this.moveable)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectToolSystem.StartMoving(this.selectedEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectToolSystem;
      }
    }

    private void OnFollow()
    {
      if (!this.EntityManager.HasComponent<Followed>(this.selectedEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LifePathEventSystem.FollowCitizen(this.selectedEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_GamePanelUISystem.ShowPanel<LifePathPanel>(this.selectedEntity);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LifePathEventSystem.UnfollowCitizen(this.selectedEntity);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.SetDirty();
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.selectedEntity != Entity.Null;

    protected override void OnProcess()
    {
      EntityManager entityManager;
      int num1;
      if (!this.EntityManager.HasComponent<BuildingExtensionData>(this.selectedPrefab))
      {
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Household>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          num1 = entityManager.HasComponent<PropertyRenter>(this.selectedEntity) ? 1 : 0;
        }
        else
          num1 = 1;
      }
      else
        num1 = 0;
      this.focusable = num1 != 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.focusing = (UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null && SelectedInfoUISystem.s_CameraController.controllerEnabled && SelectedInfoUISystem.s_CameraController.followedEntity == this.selectedEntity;
      entityManager = this.EntityManager;
      int num2;
      if (entityManager.HasComponent<Building>(this.selectedEntity))
      {
        entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Game.Buildings.WaterPowered>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<Owner>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity))
              goto label_11;
          }
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<SpawnableBuildingData>(this.selectedPrefab))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<SignatureBuildingData>(this.selectedPrefab))
              goto label_11;
          }
          num2 = 1;
          goto label_20;
        }
      }
label_11:
      entityManager = this.EntityManager;
      if (entityManager.HasComponent<Game.Objects.Object>(this.selectedEntity))
      {
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Static>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<Native>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Game.Routes.TransportStop>(this.selectedEntity))
              goto label_18;
          }
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Owner>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Building>(this.selectedEntity))
            {
              entityManager = this.EntityManager;
              num2 = !entityManager.HasComponent<Game.Objects.OutsideConnection>(this.selectedEntity) ? 1 : 0;
              goto label_20;
            }
          }
        }
      }
label_18:
      num2 = 0;
label_20:
      this.moveable = num2 != 0;
      entityManager = this.EntityManager;
      this.followable = entityManager.HasComponent<Citizen>(this.selectedEntity);
      entityManager = this.EntityManager;
      this.following = entityManager.HasComponent<Followed>(this.selectedEntity);
      entityManager = this.EntityManager;
      int num3;
      if (!entityManager.HasComponent<District>(this.selectedEntity))
      {
        entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<TransportLine>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            num3 = entityManager.HasComponent<Route>(this.selectedEntity) ? 1 : 0;
            goto label_26;
          }
          else
          {
            num3 = 0;
            goto label_26;
          }
        }
      }
      num3 = 1;
label_26:
      this.deletable = num3 != 0;
      Building component1;
      Extension component2;
      Route component3;
      this.disabled = this.EntityManager.TryGetComponent<Building>(this.selectedEntity, out component1) && BuildingUtils.CheckOption(component1, BuildingOption.Inactive) || this.EntityManager.TryGetComponent<Extension>(this.selectedEntity, out component2) && (component2.m_Flags & ExtensionFlags.Disabled) != ExtensionFlags.None || this.EntityManager.TryGetComponent<Route>(this.selectedEntity, out component3) && RouteUtils.CheckOption(component3, RouteOption.Inactive);
      entityManager = this.EntityManager;
      if (entityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity))
      {
        this.disableable = true;
        this.tooltipKeys.Add("Building");
      }
      else
      {
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Policy>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<Building>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<CityServiceUpkeep>(this.selectedEntity))
            {
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<Efficiency>(this.selectedEntity))
              {
                this.disableable = true;
                this.tooltipKeys.Add("Building");
              }
            }
          }
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<Route>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<TransportLine>(this.selectedEntity))
            {
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<RouteWaypoint>(this.selectedEntity))
                this.disableable = true;
            }
          }
        }
      }
      this.emptying = this.EntityManager.TryGetComponent<Building>(this.selectedEntity, out component1) && BuildingUtils.CheckOption(component1, BuildingOption.Empty);
      entityManager = this.EntityManager;
      int num4;
      if (entityManager.HasComponent<Policy>(this.selectedEntity))
      {
        entityManager = this.EntityManager;
        GarbageFacilityData component4;
        if (entityManager.HasComponent<Building>(this.selectedEntity) && this.EntityManager.TryGetComponent<GarbageFacilityData>(this.selectedPrefab, out component4))
        {
          num4 = component4.m_LongTermStorage ? 1 : 0;
          goto label_41;
        }
      }
      num4 = 0;
label_41:
      this.emptiable = num4 != 0;
      DynamicBuffer<Game.Prefabs.SubArea> buffer1;
      if (this.EntityManager.TryGetBuffer<Game.Prefabs.SubArea>(this.selectedPrefab, true, out buffer1) && buffer1.Length > 0)
      {
        for (int index = 0; index < buffer1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          AreaPrefab prefab = this.m_PrefabSystem.GetPrefab<AreaPrefab>(buffer1[index].m_Prefab);
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && prefab.Has<LotPrefab>())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LotPrefab = prefab;
            this.hasLotTool = true;
            break;
          }
        }
      }
      if (!this.hasLotTool)
      {
        entityManager = this.EntityManager;
        Attached component5;
        DynamicBuffer<Game.Areas.SubArea> buffer2;
        if (entityManager.HasComponent<SpawnableBuildingData>(this.selectedPrefab) && this.EntityManager.TryGetComponent<Attached>(this.selectedEntity, out component5) && this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(component5.m_Parent, true, out buffer2) && buffer2.Length > 0)
        {
          for (int index = 0; index < buffer2.Length; ++index)
          {
            entityManager = this.EntityManager;
            PrefabRef component6;
            if (entityManager.HasComponent<Game.Areas.Lot>(buffer2[index].m_Area) && this.EntityManager.TryGetComponent<PrefabRef>(buffer2[index].m_Area, out component6))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              AreaPrefab prefab = this.m_PrefabSystem.GetPrefab<AreaPrefab>(component6.m_Prefab);
              if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LotPrefab = prefab;
                this.hasLotTool = true;
                break;
              }
            }
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TrafficRouteColors == null)
      {
        // ISSUE: reference to a compiler-generated field
        RouteConfigurationData singleton = this.m_RouteConfigQuery.GetSingleton<RouteConfigurationData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TrafficRouteColors = new Color32[5]
        {
          this.m_PrefabSystem.GetPrefab<LivePathPrefab>(singleton.m_CarPathVisualization).color,
          this.m_PrefabSystem.GetPrefab<LivePathPrefab>(singleton.m_WatercraftPathVisualization).color,
          this.m_PrefabSystem.GetPrefab<LivePathPrefab>(singleton.m_AircraftPathVisualization).color,
          this.m_PrefabSystem.GetPrefab<LivePathPrefab>(singleton.m_TrainPathVisualization).color,
          this.m_PrefabSystem.GetPrefab<LivePathPrefab>(singleton.m_HumanPathVisualization).color
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TrafficRouteColorsBinding.Update(this.m_TrafficRouteColors);
      }
      entityManager = this.EntityManager;
      int num5;
      if (!entityManager.HasComponent<Building>(this.selectedEntity))
      {
        entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Aggregate>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Game.Net.Node>(this.selectedEntity))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Game.Net.Edge>(this.selectedEntity))
            {
              entityManager = this.EntityManager;
              if (!entityManager.HasComponent<Game.Routes.TransportStop>(this.selectedEntity))
              {
                entityManager = this.EntityManager;
                if (!entityManager.HasComponent<Game.Objects.OutsideConnection>(this.selectedEntity))
                {
                  entityManager = this.EntityManager;
                  if (!entityManager.HasComponent<Human>(this.selectedEntity))
                  {
                    entityManager = this.EntityManager;
                    if (!entityManager.HasComponent<Vehicle>(this.selectedEntity))
                    {
                      entityManager = this.EntityManager;
                      if (!entityManager.HasComponent<Citizen>(this.selectedEntity))
                      {
                        entityManager = this.EntityManager;
                        num5 = entityManager.HasComponent<Household>(this.selectedEntity) ? 1 : 0;
                        goto label_68;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      num5 = 1;
label_68:
      this.hasTrafficRoutes = num5 != 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficRoutesVisibleBinding.Update(this.m_TrafficRoutesSystem.routesVisible);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("focusable");
      writer.Write(this.focusable);
      writer.PropertyName("focusing");
      writer.Write(this.focusing);
      writer.PropertyName("following");
      writer.Write(this.following);
      writer.PropertyName("followable");
      writer.Write(this.followable);
      writer.PropertyName("moveable");
      writer.Write(this.moveable);
      writer.PropertyName("deletable");
      writer.Write(this.deletable);
      writer.PropertyName("disabled");
      writer.Write(this.disabled);
      writer.PropertyName("disableable");
      writer.Write(this.disableable);
      writer.PropertyName("emptying");
      writer.Write(this.emptying);
      writer.PropertyName("emptiable");
      writer.Write(this.emptiable);
      writer.PropertyName("hasLotTool");
      writer.Write(this.hasLotTool);
      writer.PropertyName("hasTrafficRoutes");
      writer.Write(this.hasTrafficRoutes);
    }

    [Preserve]
    public ActionsSection()
    {
    }
  }
}
