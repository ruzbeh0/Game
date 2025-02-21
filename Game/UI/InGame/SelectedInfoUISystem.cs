// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.SelectedInfoUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Events;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using Game.UI.Debug;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class SelectedInfoUISystem : UISystemBase
  {
    private const string kGroup = "selectedInfo";
    public static OrbitCameraController s_CameraController;
    private float3 m_SelectedPosition;
    private Entity m_SelectedEntity;
    private Entity m_SelectedPrefab;
    private Entity m_SelectedRoute;
    private Entity m_LastSelectedEntity;
    private EntityQuery m_TransportConfigQuery;
    private List<ISectionSource> m_TopSections;
    private List<ISectionSource> m_MiddleSections;
    private List<ISectionSource> m_BottomSections;
    private TitleSection m_TitleSection;
    private DeveloperSection m_DeveloperSection;
    private LineVisualizerSection m_LineVisualizerSection;
    private HouseholdSidebarSection m_HouseholdSidebarSection;
    private DebugUISystem m_DebugUISystem;
    private ToolSystem m_ToolSystem;
    private PrefabSystem m_PrefabSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private ValueBinding<Entity> m_SelectedEntityBinding;
    private ValueBinding<Entity> m_SelectedTrailerControllerBinding;
    private ValueBinding<string> m_SelectedUITagBinding;
    private GetterValueBinding<Entity> m_SelectedRouteBinding;
    private ValueBinding<bool> m_ActiveSelectionBinding;
    private RawValueBinding m_TopSectionsBinding;
    private RawValueBinding m_MiddleSectionsBinding;
    private RawValueBinding m_BottomSectionsBinding;
    private RawValueBinding m_IDSectionBinding;
    private RawValueBinding m_LineVisualizerSectionBinding;
    private RawValueBinding m_DeveloperSectionBinding;
    private RawValueBinding m_HouseholdSidebarSectionBinding;
    private ValueBinding<float2> m_PositionBinding;
    private RawValueBinding m_TooltipTagsBinding;
    private bool m_BindingsDirty;
    private UIUpdateState m_UpdateState;

    public override GameMode gameMode => GameMode.Game;

    public float3 selectedPosition => this.m_SelectedPosition;

    public Entity selectedEntity => this.m_SelectedEntity;

    public Entity selectedPrefab => this.m_SelectedPrefab;

    public Entity selectedRoute
    {
      get => this.m_SelectedRoute;
      set
      {
        this.m_SelectedRoute = value;
        this.m_SelectedRouteBinding.Update();
      }
    }

    public Action<Entity, Entity, float3> eventSelectionChanged { get; set; }

    public List<TooltipTags> tooltipTags { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddSections(this.m_TopSections = new List<ISectionSource>(), this.m_MiddleSections = new List<ISectionSource>(), this.m_BottomSections = new List<ISectionSource>());
      this.tooltipTags = new List<TooltipTags>();
      // ISSUE: reference to a compiler-generated field
      this.m_DebugUISystem = this.World.GetOrCreateSystemManaged<DebugUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedPrefab = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelectedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedEntityBinding = new ValueBinding<Entity>("selectedInfo", "selectedEntity", Entity.Null)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedTrailerControllerBinding = new ValueBinding<Entity>("selectedInfo", "selectedTrailerController", Entity.Null)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedUITagBinding = new ValueBinding<string>("selectedInfo", "selectedUITag", string.Empty)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedRouteBinding = new GetterValueBinding<Entity>("selectedInfo", "selectedRoute", (Func<Entity>) (() => this.m_SelectedRoute))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActiveSelectionBinding = new ValueBinding<bool>("selectedInfo", "activeSelection", false)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_TopSectionsBinding = new RawValueBinding("selectedInfo", "topSections", (Action<IJsonWriter>) (writer => this.WriteSections(this.m_TopSections, writer)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_MiddleSectionsBinding = new RawValueBinding("selectedInfo", "middleSections", (Action<IJsonWriter>) (writer => this.WriteSections(this.m_MiddleSections, writer)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_BottomSectionsBinding = new RawValueBinding("selectedInfo", "bottomSections", (Action<IJsonWriter>) (writer => this.WriteSections(this.m_BottomSections, writer)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_IDSectionBinding = new RawValueBinding("selectedInfo", "titleSection", (Action<IJsonWriter>) (writer => this.m_TitleSection.Write(writer)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_LineVisualizerSectionBinding = new RawValueBinding("selectedInfo", "lineVisualizerSection", (Action<IJsonWriter>) (writer => this.m_LineVisualizerSection.Write(writer)))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_DeveloperSectionBinding = new RawValueBinding("selectedInfo", "developerSection", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_DebugUISystem.developerInfoVisible)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_DeveloperSection.Write(binder);
        }
        else
          binder.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_HouseholdSidebarSectionBinding = new RawValueBinding("selectedInfo", "householdSidebarSection", (Action<IJsonWriter>) (writer => this.m_HouseholdSidebarSection.Write(writer)))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PositionBinding = new ValueBinding<float2>("selectedInfo", "position", new float2())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TooltipTagsBinding = new RawValueBinding("selectedInfo", "tooltipTags", (Action<IJsonWriter>) (writer =>
      {
        writer.ArrayBegin(this.tooltipTags.Count);
        for (int index = 0; index < this.tooltipTags.Count; ++index)
          writer.Write(this.tooltipTags[index].ToString());
        writer.ArrayEnd();
      }))));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("selectedInfo", "selectEntity", (Action<Entity>) (entity =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        if (!this.EntityManager.HasComponent<Game.Events.TrafficAccident>(entity))
        {
          // ISSUE: reference to a compiler-generated method
          this.SetSelection(entity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.StartFollowing(entity);
        }
      })));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding("selectedInfo", "clearSelection", (System.Action) (() => this.SetSelection(Entity.Null))));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("selectedInfo", "setSelectedRoute", (Action<Entity>) (entity => this.selectedRoute = entity)));
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateState = UIUpdateState.Create(this.World, 256);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged += (Action<ToolBaseSystem>) (obj => this.m_UpdateState.ForceUpdate());
    }

    private void OnToolChanged(ToolBaseSystem obj) => this.m_UpdateState.ForceUpdate();

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        SelectedInfoUISystem.s_CameraController.EventCameraMove -= (System.Action) (() => this.StopFollowing());
      }
      base.OnDestroy();
    }

    public void AddTopSection(ISectionSource section) => this.m_TopSections.Add(section);

    public void AddMiddleSection(ISectionSource section) => this.m_MiddleSections.Add(section);

    public void AddBottomSection(ISectionSource section) => this.m_BottomSections.Add(section);

    public void AddDeveloperInfo(ISubsectionSource subsection)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DeveloperSection.AddSubsection(subsection);
    }

    private void AddSections(
      List<ISectionSource> topSections,
      List<ISectionSource> sections,
      List<ISectionSource> bottomSections)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TitleSection = this.World.GetOrCreateSystemManaged<TitleSection>();
      // ISSUE: reference to a compiler-generated field
      this.m_LineVisualizerSection = this.World.GetOrCreateSystemManaged<LineVisualizerSection>();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdSidebarSection = this.World.GetOrCreateSystemManaged<HouseholdSidebarSection>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeveloperSection = this.World.GetOrCreateSystemManaged<DeveloperSection>();
      topSections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<NotificationsSection>());
      topSections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<AverageHappinessSection>());
      topSections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<StatusSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DescriptionSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DestroyedBuildingSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DestroyedTreeSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PoliciesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<LevelSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<UpkeepSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<UpgradePropertiesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<EfficiencySection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ResidentsSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<EmployeesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<AttractivenessSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ComfortSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<EducationSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ElectricitySection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<BatterySection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<TransformerSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<HealthcareSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DeathcareSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<GarbageSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PoliceSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ShelterSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PrisonSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<SewageSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<WaterSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ParkSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ParkingSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<MailSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<CitizenSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DummyHumanSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<AnimalSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PrivateVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PublicTransportVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<CargoTransportVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DeliveryVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<HealthcareVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DeathcareVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<FireVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PoliceVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<GarbageVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PostVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<MaintenanceVehicleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DistrictsSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<LocalServicesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PassengersSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<CargoSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<LoadSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<CompanySection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<StorageSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<SelectVehiclesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<LineSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<TicketPriceSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<VehicleCountSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ScheduleSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ColorSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<LinesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ResourceSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<RoadSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<PollutionSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<DispatchedVehiclesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<VehiclesSection>());
      sections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<UpgradesSection>());
      bottomSections.Add((ISectionSource) this.World.GetOrCreateSystemManaged<ActionsSection>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      SelectedInfoUISystem.s_CameraController = this.m_CameraUpdateSystem.orbitCameraController;
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null))
        return;
      if (GameManager.instance.gameMode == GameMode.Editor)
      {
        // ISSUE: reference to a compiler-generated field
        SelectedInfoUISystem.s_CameraController.mode = OrbitCameraController.Mode.Editor;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        SelectedInfoUISystem.s_CameraController.EventCameraMove += (System.Action) (() => this.StopFollowing());
        // ISSUE: reference to a compiler-generated field
        SelectedInfoUISystem.s_CameraController.mode = OrbitCameraController.Mode.Follow;
      }
    }

    private void OnCameraStoppedFollowing() => this.StopFollowing();

    private void StartFollowing(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.EntityManager.Exists(entity) || !((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null) || !(SelectedInfoUISystem.s_CameraController.followedEntity != entity))
        return;
      // ISSUE: reference to a compiler-generated field
      SelectedInfoUISystem.s_CameraController.followedEntity = entity;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      SelectedInfoUISystem.s_CameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) SelectedInfoUISystem.s_CameraController;
      // ISSUE: reference to a compiler-generated method
      this.SetBindingsDirty();
    }

    private void StopFollowing()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_CameraUpdateSystem.orbitCameraController.mode != OrbitCameraController.Mode.Follow)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem.orbitCameraController.followedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem.gamePlayController.TryMatchPosition((IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.gamePlayController;
      // ISSUE: reference to a compiler-generated method
      this.SetBindingsDirty();
    }

    public void RequestUpdate() => this.m_UpdateState.ForceUpdate();

    public void SetDirty() => this.SetBindingsDirty();

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.RefreshSelection();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedEntityBinding.Update(this.m_SelectedEntity);
      Controller component;
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetComponent<Controller>(this.m_SelectedEntity, out component))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedTrailerControllerBinding.Update(component.m_Controller);
      }
      else
      {
        DynamicBuffer<LayoutElement> buffer;
        if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer) && buffer.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedTrailerControllerBinding.Update(this.m_SelectedEntity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedTrailerControllerBinding.Update(Entity.Null);
        }
      }
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SelectedUITagBinding.Update(this.m_PrefabSystem.TryGetPrefab<PrefabBase>(this.m_SelectedPrefab, out prefab) ? prefab.uiTag : string.Empty);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveSelectionBinding.Update(this.m_SelectedEntity != Entity.Null);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastSelectedEntity != this.m_SelectedEntity)
      {
        // ISSUE: reference to a compiler-generated method
        this.ResetRouteVisibility();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastSelectedEntity = this.m_SelectedEntity;
        Action<Entity, Entity, float3> selectionChanged = this.eventSelectionChanged;
        if (selectionChanged != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          selectionChanged(this.m_SelectedEntity, this.m_SelectedPrefab, this.m_SelectedPosition);
        }
        // ISSUE: reference to a compiler-generated method
        this.SetBindingsDirty();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.EntityManager.HasComponent<Updated>(this.m_SelectedEntity) || this.EntityManager.HasComponent<BatchesUpdated>(this.m_SelectedEntity) || this.m_UpdateState.Advance())
        {
          // ISSUE: reference to a compiler-generated method
          this.SetBindingsDirty();
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateSections();
      // ISSUE: reference to a compiler-generated method
      this.UpdatePosition();
    }

    private void RefreshSelection()
    {
      Entity entity;
      PrefabRef component;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetSelection(out entity) && this.EntityManager.TryGetComponent<PrefabRef>(entity, out component))
      {
        Entity prefab = component.m_Prefab;
        // ISSUE: reference to a compiler-generated method
        this.FilterSelection(ref entity, ref prefab);
        // ISSUE: reference to a compiler-generated field
        int selectedIndex = this.m_ToolSystem.selectedIndex;
        float3 position;
        Bounds3 bounds;
        // ISSUE: reference to a compiler-generated method
        if (SelectedInfoUISystem.TryGetPosition(entity, this.EntityManager, ref selectedIndex, out Entity _, out position, out bounds, out quaternion _, true) || this.EntityManager.HasComponent<Household>(entity))
        {
          position.y = MathUtils.Center(bounds.y);
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedEntity = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedPrefab = prefab;
          // ISSUE: reference to a compiler-generated field
          this.m_SelectedPosition = position;
          return;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedPrefab = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedPosition = float3.zero;
    }

    private bool TryGetSelection(out Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      entity = this.m_ToolSystem.selected;
      return entity != Entity.Null;
    }

    private void FilterSelection(ref Entity entity, ref Entity prefab)
    {
      Owner component1;
      if (this.EntityManager.HasComponent<Icon>(entity) && this.EntityManager.TryGetComponent<Owner>(entity, out component1))
      {
        Owner component2;
        if (this.EntityManager.HasComponent<RouteLane>(component1.m_Owner) && this.EntityManager.HasComponent<Waypoint>(component1.m_Owner) && this.EntityManager.TryGetComponent<Owner>(component1.m_Owner, out component2))
        {
          entity = component2.m_Owner;
        }
        else
        {
          CurrentBuilding component3;
          if (this.EntityManager.TryGetComponent<CurrentBuilding>(component1.m_Owner, out component3))
          {
            if (this.EntityManager.Exists(component3.m_CurrentBuilding))
              entity = component3.m_CurrentBuilding;
          }
          else
            entity = component1.m_Owner;
        }
        PrefabRef component4;
        if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component4))
          prefab = component4.m_Prefab;
        // ISSUE: reference to a compiler-generated method
        this.SetSelection(entity);
      }
      Game.Creatures.Resident component5;
      PrefabRef component6;
      if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(entity, out component5) && this.EntityManager.TryGetComponent<PrefabRef>(component5.m_Citizen, out component6))
      {
        entity = component5.m_Citizen;
        prefab = component6.m_Prefab;
      }
      Game.Creatures.Pet component7;
      PrefabRef component8;
      if (!this.EntityManager.TryGetComponent<Game.Creatures.Pet>(entity, out component7) || !this.EntityManager.TryGetComponent<PrefabRef>(component7.m_HouseholdPet, out component8))
        return;
      entity = component7.m_HouseholdPet;
      prefab = component8.m_Prefab;
    }

    public void SetSelection(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      if (entity == this.m_SelectedEntity)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.selected = entity;
    }

    public void SetRoutesVisible()
    {
      UITransportConfigurationPrefab config;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetTransportConfig(out config) || this.m_ToolSystem.GetInfomodes(this.m_ToolSystem.activeInfoview)?.Find((Predicate<InfomodeInfo>) (mode => (UnityEngine.Object) mode.m_Mode == (UnityEngine.Object) config.m_RoutesInfomode)) != null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.SetInfomodeActive(config.m_RoutesInfomode, true, 0);
    }

    private void ResetRouteVisibility()
    {
      UITransportConfigurationPrefab config;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetTransportConfig(out config) || this.m_ToolSystem.GetInfomodes(this.m_ToolSystem.activeInfoview)?.Find((Predicate<InfomodeInfo>) (mode => (UnityEngine.Object) mode.m_Mode == (UnityEngine.Object) config.m_RoutesInfomode)) != null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.SetInfomodeActive(config.m_RoutesInfomode, false, 0);
    }

    private bool TryGetTransportConfig(out UITransportConfigurationPrefab config)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.m_PrefabSystem.TryGetSingletonPrefab<UITransportConfigurationPrefab>(this.m_TransportConfigQuery, out config);
    }

    private void SetBindingsDirty()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedEntity == Entity.Null)
        return;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_TopSections.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TopSections[index]?.RequestUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_MiddleSections.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MiddleSections[index]?.RequestUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_BottomSections.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BottomSections[index]?.RequestUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TitleSection?.RequestUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LineVisualizerSection?.RequestUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DeveloperSection?.RequestUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_HouseholdSidebarSection?.RequestUpdate();
      // ISSUE: reference to a compiler-generated field
      this.m_BindingsDirty = true;
    }

    private void UpdateSections()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedEntity == Entity.Null)
        return;
      this.tooltipTags.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TitleSection?.PerformUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_HouseholdSidebarSection?.PerformUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LineVisualizerSection?.PerformUpdate();
      // ISSUE: reference to a compiler-generated field
      this.m_LineVisualizerSectionBinding.Update();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_TopSections.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TopSections[index]?.PerformUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_MiddleSections.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MiddleSections[index]?.PerformUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_BottomSections.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BottomSections[index]?.PerformUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_DebugUISystem.developerInfoVisible)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_DeveloperSection?.PerformUpdate();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BindingsDirty)
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdateSectionBindings();
      // ISSUE: reference to a compiler-generated field
      this.m_BindingsDirty = false;
    }

    private void UpdateSectionBindings()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TooltipTagsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_IDSectionBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_DeveloperSectionBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_TopSectionsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_MiddleSectionsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_BottomSectionsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_HouseholdSidebarSectionBinding.Update();
    }

    private void UpdatePosition()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedEntity == Entity.Null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_PositionBinding.Update((float2) (Vector2) ((UnityEngine.Object) Camera.main == (UnityEngine.Object) null ? new Vector3() : Camera.main.WorldToViewportPoint((Vector3) this.selectedPosition)));
    }

    private void OnClearSelection() => this.SetSelection(Entity.Null);

    private void OnSelect(Entity entity)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      if (!this.EntityManager.HasComponent<Game.Events.TrafficAccident>(entity))
      {
        // ISSUE: reference to a compiler-generated method
        this.SetSelection(entity);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.StartFollowing(entity);
      }
    }

    private void OnSetSelectedRoute(Entity entity) => this.selectedRoute = entity;

    public void Focus(Entity entity)
    {
      if (entity == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.StopFollowing();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.StartFollowing(entity);
      }
    }

    public static bool TryGetPosition(
      Entity entity,
      EntityManager entityManager,
      ref int elementIndex,
      out Entity location,
      out float3 position,
      out Bounds3 bounds,
      out quaternion rotation,
      bool reinterpolate = false)
    {
      location = entity;
      // ISSUE: reference to a compiler-generated method
      SelectedInfoUISystem.FilterPositionTarget(ref location, entityManager);
      DynamicBuffer<TransformFrame> buffer1;
      if (entityManager.TryGetBuffer<TransformFrame>(location, true, out buffer1))
      {
        // ISSUE: reference to a compiler-generated method
        Game.Objects.Transform interpolatedPosition = SelectedInfoUISystem.GetInterpolatedPosition(location, entityManager, buffer1, reinterpolate, out bounds);
        position = interpolatedPosition.m_Position;
        rotation = interpolatedPosition.m_Rotation;
      }
      else
      {
        Relative component1;
        if (entityManager.TryGetComponent<Relative>(location, out component1))
        {
          // ISSUE: reference to a compiler-generated method
          Game.Objects.Transform relativePosition = SelectedInfoUISystem.GetRelativePosition(location, entityManager, component1, reinterpolate, out bounds);
          position = relativePosition.m_Position;
          rotation = relativePosition.m_Rotation;
        }
        else
        {
          Game.Objects.Transform component2;
          if (entityManager.TryGetComponent<Game.Objects.Transform>(location, out component2))
          {
            // ISSUE: reference to a compiler-generated method
            Game.Objects.Transform objectPosition = SelectedInfoUISystem.GetObjectPosition(location, entityManager, component2, out bounds);
            position = objectPosition.m_Position;
            rotation = objectPosition.m_Rotation;
          }
          else
          {
            DynamicBuffer<RouteWaypoint> buffer2;
            if (entityManager.TryGetBuffer<RouteWaypoint>(location, true, out buffer2))
            {
              // ISSUE: reference to a compiler-generated method
              position = SelectedInfoUISystem.GetRoutePosition(entityManager, buffer2);
              bounds = new Bounds3(position, position);
              rotation = quaternion.identity;
            }
            else
            {
              DynamicBuffer<LabelPosition> buffer3;
              if (entityManager.TryGetBuffer<LabelPosition>(location, true, out buffer3))
              {
                // ISSUE: reference to a compiler-generated method
                position = SelectedInfoUISystem.GetAggregatePosition(buffer3, ref elementIndex);
                bounds = new Bounds3(position, position);
                rotation = quaternion.identity;
              }
              else
              {
                Geometry component3;
                if (entityManager.TryGetComponent<Geometry>(location, out component3))
                {
                  position = component3.m_CenterPosition;
                  bounds = new Bounds3(position, position);
                  rotation = quaternion.identity;
                }
                else
                {
                  Icon component4;
                  if (entityManager.TryGetComponent<Icon>(location, out component4))
                  {
                    position = component4.m_Location;
                    bounds = new Bounds3(position, position);
                    rotation = quaternion.identity;
                  }
                  else
                  {
                    Game.Net.Node component5;
                    if (entityManager.TryGetComponent<Game.Net.Node>(location, out component5))
                    {
                      // ISSUE: reference to a compiler-generated method
                      position = SelectedInfoUISystem.GetNodePosition(location, entityManager, component5, out bounds, out rotation);
                    }
                    else
                    {
                      Curve component6;
                      if (entityManager.TryGetComponent<Curve>(location, out component6))
                      {
                        // ISSUE: reference to a compiler-generated method
                        position = SelectedInfoUISystem.GetCurvePosition(location, entityManager, component6, out bounds, out rotation);
                      }
                      else
                      {
                        position = float3.zero;
                        bounds = new Bounds3();
                        rotation = quaternion.identity;
                        return false;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      return true;
    }

    private static Game.Objects.Transform GetInterpolatedPosition(
      Entity entity,
      EntityManager entityManager,
      DynamicBuffer<TransformFrame> transformFrames,
      bool reinterpolate,
      out Bounds3 bounds)
    {
      InterpolatedTransform interpolatedTransform;
      // ISSUE: reference to a compiler-generated method
      if (!reinterpolate && SelectedInfoUISystem.IsNearCamera(entity, entityManager))
      {
        interpolatedTransform = entityManager.GetComponentData<InterpolatedTransform>(entity);
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        RenderingSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<RenderingSystem>();
        UpdateFrame sharedComponent = entityManager.GetSharedComponent<UpdateFrame>(entity);
        uint updateFrame1;
        uint updateFrame2;
        float framePosition;
        // ISSUE: reference to a compiler-generated method
        ObjectInterpolateSystem.CalculateUpdateFrames(systemManaged.frameIndex, systemManaged.frameTime, sharedComponent.m_Index, out updateFrame1, out updateFrame2, out framePosition);
        // ISSUE: reference to a compiler-generated method
        interpolatedTransform = ObjectInterpolateSystem.CalculateTransform(transformFrames[(int) updateFrame1], transformFrames[(int) updateFrame2], framePosition);
      }
      // ISSUE: reference to a compiler-generated method
      return SelectedInfoUISystem.GetObjectPosition(entity, entityManager, interpolatedTransform.ToTransform(), out bounds);
    }

    private static Game.Objects.Transform GetRelativePosition(
      Entity entity,
      EntityManager entityManager,
      Relative relative,
      bool reinterpolate,
      out Bounds3 bounds)
    {
      Game.Objects.Transform transform = entityManager.GetComponentData<Game.Objects.Transform>(entity);
      Entity entity1 = Entity.Null;
      CurrentVehicle component1;
      if (entityManager.TryGetComponent<CurrentVehicle>(entity, out component1))
      {
        entity1 = component1.m_Vehicle;
      }
      else
      {
        Owner component2;
        if (entityManager.TryGetComponent<Owner>(entity, out component2))
          entity1 = component2.m_Owner;
      }
      DynamicBuffer<TransformFrame> buffer;
      if (entityManager.TryGetBuffer<TransformFrame>(entity1, true, out buffer))
      {
        // ISSUE: reference to a compiler-generated method
        transform = ObjectUtils.LocalToWorld(SelectedInfoUISystem.GetInterpolatedPosition(entity1, entityManager, buffer, reinterpolate, out Bounds3 _), relative.ToTransform());
      }
      else
      {
        Game.Objects.Transform component3;
        if (entityManager.TryGetComponent<Game.Objects.Transform>(entity1, out component3))
          transform = ObjectUtils.LocalToWorld(component3, relative.ToTransform());
      }
      // ISSUE: reference to a compiler-generated method
      return SelectedInfoUISystem.GetObjectPosition(entity, entityManager, transform, out bounds);
    }

    private static bool IsNearCamera(Entity entity, EntityManager entityManager)
    {
      CullingInfo component;
      if (!entityManager.TryGetComponent<CullingInfo>(entity, out component) || component.m_CullingIndex == 0)
        return false;
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      NativeList<PreCullingData> cullingData = entityManager.World.GetOrCreateSystemManaged<PreCullingSystem>().GetCullingData(true, out dependencies);
      dependencies.Complete();
      return (cullingData[component.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) > (PreCullingFlags) 0;
    }

    private static Game.Objects.Transform GetObjectPosition(
      Entity entity,
      EntityManager entityManager,
      Game.Objects.Transform transform,
      out Bounds3 bounds)
    {
      bounds = new Bounds3(transform.m_Position, transform.m_Position);
      PrefabRef component1;
      ObjectGeometryData component2;
      if (entityManager.TryGetComponent<PrefabRef>(entity, out component1) && entityManager.TryGetComponent<ObjectGeometryData>(component1.m_Prefab, out component2))
        bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, component2);
      return transform;
    }

    private static float3 GetNodePosition(
      Entity entity,
      EntityManager entityManager,
      Game.Net.Node node,
      out Bounds3 bounds,
      out quaternion rotation)
    {
      bounds = new Bounds3(node.m_Position, node.m_Position);
      PrefabRef component1;
      NetGeometryData component2;
      if (entityManager.TryGetComponent<PrefabRef>(entity, out component1) && entityManager.TryGetComponent<NetGeometryData>(component1.m_Prefab, out component2))
        bounds.y = node.m_Position.y + component2.m_DefaultSurfaceHeight;
      rotation = node.m_Rotation;
      return node.m_Position;
    }

    private static float3 GetCurvePosition(
      Entity entity,
      EntityManager entityManager,
      Curve curve,
      out Bounds3 bounds,
      out quaternion rotation)
    {
      float3 curvePosition = MathUtils.Position(curve.m_Bezier, 0.5f);
      bounds = new Bounds3(curvePosition, curvePosition);
      rotation = quaternion.Euler(MathUtils.Tangent(curve.m_Bezier, 0.5f));
      PrefabRef component1;
      NetGeometryData component2;
      if (entityManager.TryGetComponent<PrefabRef>(entity, out component1) && entityManager.TryGetComponent<NetGeometryData>(component1.m_Prefab, out component2))
        bounds.y = curvePosition.y + component2.m_DefaultSurfaceHeight;
      return curvePosition;
    }

    private static float3 GetRoutePosition(
      EntityManager entityManager,
      DynamicBuffer<RouteWaypoint> routeWaypoints)
    {
      float3 routePosition = new float3();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3 y = (float3) ((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null ? SelectedInfoUISystem.s_CameraController.pivot : new Vector3());
      float num1 = float.MaxValue;
      for (int index = 0; index < routeWaypoints.Length; ++index)
      {
        Position component;
        if (entityManager.TryGetComponent<Position>(routeWaypoints[index].m_Waypoint, out component))
        {
          float num2 = math.distancesq(component.m_Position, y);
          if ((double) num2 < (double) num1)
          {
            routePosition = component.m_Position;
            num1 = num2;
          }
        }
      }
      return routePosition;
    }

    private static float3 GetAggregatePosition(
      DynamicBuffer<LabelPosition> labelPositions,
      ref int selectedIndex)
    {
      float3 aggregatePosition = new float3();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float3 y = (float3) ((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null ? SelectedInfoUISystem.s_CameraController.pivot : new Vector3());
      float num1 = float.MaxValue;
      int num2 = -1;
      for (int index = 0; index < labelPositions.Length; ++index)
      {
        LabelPosition labelPosition = labelPositions[index];
        float3 x = MathUtils.Position(labelPosition.m_Curve, 0.5f);
        float num3 = math.distancesq(x, y);
        if (labelPosition.m_ElementIndex == selectedIndex)
          return x;
        if ((double) num3 < (double) num1)
        {
          aggregatePosition = x;
          num1 = num3;
          num2 = labelPosition.m_ElementIndex;
        }
      }
      selectedIndex = num2;
      return aggregatePosition;
    }

    private static void FilterPositionTarget(ref Entity entity, EntityManager entityManager)
    {
      DynamicBuffer<TargetElement> buffer;
      if (entityManager.TryGetBuffer<TargetElement>(entity, true, out buffer) && buffer.Length != 0)
        entity = buffer[0].m_Entity;
      CurrentTransport component1;
      if (entityManager.TryGetComponent<CurrentTransport>(entity, out component1))
        entity = component1.m_CurrentTransport;
      if (entityManager.HasComponent<Unspawned>(entity))
      {
        CurrentVehicle component2;
        if (entityManager.TryGetComponent<CurrentVehicle>(entity, out component2))
        {
          entity = component2.m_Vehicle;
        }
        else
        {
          Game.Creatures.Resident component3;
          CurrentBuilding component4;
          if (entityManager.TryGetComponent<Game.Creatures.Resident>(entity, out component3) && entityManager.TryGetComponent<CurrentBuilding>(component3.m_Citizen, out component4))
          {
            entity = component4.m_CurrentBuilding;
          }
          else
          {
            Game.Creatures.Pet component5;
            CurrentBuilding component6;
            if (entityManager.TryGetComponent<Game.Creatures.Pet>(entity, out component5) && entityManager.TryGetComponent<CurrentBuilding>(component5.m_HouseholdPet, out component6))
              entity = component6.m_CurrentBuilding;
          }
        }
      }
      if (entityManager.HasComponent<Unspawned>(entity))
      {
        HumanCurrentLane component7;
        if (entityManager.TryGetComponent<HumanCurrentLane>(entity, out component7))
        {
          // ISSUE: reference to a compiler-generated method
          SelectedInfoUISystem.FilterPositionTarget(out entity, component7.m_Lane, entityManager);
        }
        else
        {
          AnimalCurrentLane component8;
          if (entityManager.TryGetComponent<AnimalCurrentLane>(entity, out component8))
          {
            // ISSUE: reference to a compiler-generated method
            SelectedInfoUISystem.FilterPositionTarget(out entity, component8.m_Lane, entityManager);
          }
          else
          {
            CarCurrentLane component9;
            if (entityManager.TryGetComponent<CarCurrentLane>(entity, out component9))
            {
              // ISSUE: reference to a compiler-generated method
              SelectedInfoUISystem.FilterPositionTarget(out entity, component9.m_Lane, entityManager);
            }
            else
            {
              ParkedCar component10;
              if (entityManager.TryGetComponent<ParkedCar>(entity, out component10))
              {
                // ISSUE: reference to a compiler-generated method
                SelectedInfoUISystem.FilterPositionTarget(out entity, component10.m_Lane, entityManager);
              }
              else
              {
                TrainCurrentLane component11;
                if (entityManager.TryGetComponent<TrainCurrentLane>(entity, out component11))
                {
                  // ISSUE: reference to a compiler-generated method
                  SelectedInfoUISystem.FilterPositionTarget(out entity, component11.m_Front.m_Lane, entityManager);
                }
                else
                {
                  ParkedTrain component12;
                  if (entityManager.TryGetComponent<ParkedTrain>(entity, out component12))
                  {
                    // ISSUE: reference to a compiler-generated method
                    SelectedInfoUISystem.FilterPositionTarget(out entity, component12.m_FrontLane, entityManager);
                  }
                  else
                  {
                    AircraftCurrentLane component13;
                    if (entityManager.TryGetComponent<AircraftCurrentLane>(entity, out component13))
                    {
                      // ISSUE: reference to a compiler-generated method
                      SelectedInfoUISystem.FilterPositionTarget(out entity, component13.m_Lane, entityManager);
                    }
                    else
                    {
                      WatercraftCurrentLane component14;
                      if (entityManager.TryGetComponent<WatercraftCurrentLane>(entity, out component14))
                      {
                        // ISSUE: reference to a compiler-generated method
                        SelectedInfoUISystem.FilterPositionTarget(out entity, component14.m_Lane, entityManager);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      CurrentBuilding component15;
      if (entityManager.TryGetComponent<CurrentBuilding>(entity, out component15))
        entity = component15.m_CurrentBuilding;
      PropertyRenter component16;
      if (entityManager.TryGetComponent<PropertyRenter>(entity, out component16))
        entity = component16.m_Property;
      TouristHousehold component17;
      if (!entityManager.TryGetComponent<TouristHousehold>(entity, out component17))
        return;
      entity = component17.m_Hotel;
    }

    private static void FilterPositionTarget(
      out Entity entity,
      Entity location,
      EntityManager entityManager)
    {
      entity = Entity.Null;
      if (entityManager.HasComponent<Game.Objects.Object>(location))
        entity = location;
      Owner component;
      while (entityManager.TryGetComponent<Owner>(location, out component))
      {
        location = component.m_Owner;
        if (entityManager.HasComponent<Game.Objects.Object>(location))
          entity = location;
      }
      DynamicBuffer<Game.Objects.SubObject> buffer;
      if (!entityManager.HasComponent<Game.Net.OutsideConnection>(location) || !entityManager.TryGetBuffer<Game.Objects.SubObject>(location, true, out buffer))
        return;
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity subObject = buffer[index].m_SubObject;
        if (entityManager.HasComponent<Game.Objects.OutsideConnection>(subObject))
          entity = subObject;
      }
    }

    private void WriteDeveloperSection(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_DebugUISystem.developerInfoVisible)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_DeveloperSection.Write(binder);
      }
      else
        binder.WriteNull();
    }

    private void WriteSections(List<ISectionSource> list, IJsonWriter binder)
    {
      binder.ArrayBegin(list.Count);
      for (int index = 0; index < list.Count; ++index)
        binder.Write<ISectionSource>(list[index]);
      binder.ArrayEnd();
    }

    private void WriteTooltipFlags(IJsonWriter writer)
    {
      writer.ArrayBegin(this.tooltipTags.Count);
      for (int index = 0; index < this.tooltipTags.Count; ++index)
        writer.Write(this.tooltipTags[index].ToString());
      writer.ArrayEnd();
    }

    [Preserve]
    public SelectedInfoUISystem()
    {
    }
  }
}
