// Decompiled with JetBrains decompiler
// Type: Game.Tools.RouteToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Audio;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class RouteToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Route Tool";
    private AudioManager m_AudioManager;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_TempRouteQuery;
    private EntityQuery m_EventQuery;
    private EntityQuery m_SoundQuery;
    private IProxyAction m_AddOrModifyWaypoint;
    private IProxyAction m_RemoveWaypoint;
    private IProxyAction m_UndoWaypoint;
    private ControlPoint m_LastRaycastPoint;
    private NativeList<ControlPoint> m_ControlPoints;
    private NativeValue<RouteToolSystem.Tooltip> m_Tooltip;
    private RouteToolSystem.State m_State;
    private bool m_ControlPointsMoved;
    private bool m_ForceApply;
    private bool m_ForceCancel;
    private bool m_CanApplyModify;
    private ControlPoint m_MoveStartPosition;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private RoutePrefab m_SelectedPrefab;
    private RouteToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Route Tool";

    public RoutePrefab prefab
    {
      get => this.m_SelectedPrefab;
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) this.m_SelectedPrefab))
          return;
        this.m_SelectedPrefab = value;
        this.m_ForceUpdate = true;
        this.color = (Color32) this.m_SelectedPrefab.m_Color;
        Action<PrefabBase> eventPrefabChanged = this.m_ToolSystem.EventPrefabChanged;
        if (eventPrefabChanged == null)
          return;
        eventPrefabChanged((PrefabBase) value);
      }
    }

    public RouteToolSystem.State state => this.m_State;

    public ControlPoint moveStartPosition => this.m_MoveStartPosition;

    public RouteToolSystem.Tooltip tooltip => this.m_Tooltip.value;

    public bool underground { get; set; }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_AddOrModifyWaypoint;
        yield return this.m_RemoveWaypoint;
        yield return this.m_UndoWaypoint;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_TempRouteQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<PathUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddOrModifyWaypoint = InputManager.instance.toolActionCollection.GetActionState("Add Or Modify Waypoint", nameof (RouteToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_RemoveWaypoint = InputManager.instance.toolActionCollection.GetActionState("Remove Waypoint", nameof (RouteToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_UndoWaypoint = InputManager.instance.toolActionCollection.GetActionState("Undo Waypoint", nameof (RouteToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints = new NativeList<ControlPoint>(20, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip = new NativeValue<RouteToolSystem.Tooltip>(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LastRaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = RouteToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceApply = false;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceCancel = false;
      this.requireUnderground = false;
      this.requireNetArrows = false;
      this.requireRoutes = RouteType.None;
      this.requireNet = Layer.None;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        this.applyAction.enabled = this.actionsEnabled;
        // ISSUE: reference to a compiler-generated field
        this.applyActionOverride = this.m_AddOrModifyWaypoint;
        this.secondaryApplyAction.enabled = this.actionsEnabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.secondaryApplyActionOverride = this.m_State != RouteToolSystem.State.Create || this.m_ControlPoints.Length <= 1 ? this.m_RemoveWaypoint : this.m_UndoWaypoint;
      }
    }

    public NativeList<ControlPoint> GetControlPoints(out JobHandle dependencies)
    {
      dependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      return this.m_ControlPoints;
    }

    public override PrefabBase GetPrefab() => (PrefabBase) this.prefab;

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      if (!(prefab is RoutePrefab routePrefab))
        return false;
      this.prefab = routePrefab;
      return true;
    }

    public override void SetUnderground(bool underground) => this.underground = underground;

    public override void ElevationUp() => this.underground = false;

    public override void ElevationDown() => this.underground = true;

    public override void ElevationScroll() => this.underground = !this.underground;

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        RouteData componentData = this.m_PrefabSystem.GetComponentData<RouteData>((PrefabBase) this.prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_State == RouteToolSystem.State.Modify || this.m_State == RouteToolSystem.State.Remove) && this.EntityManager.HasComponent<Route>(this.m_MoveStartPosition.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.EntityManager.GetComponentData<PrefabRef>(this.m_MoveStartPosition.m_OriginalEntity).m_Prefab;
        }
        TransportLineData component;
        if (this.EntityManager.TryGetComponent<TransportLineData>(entity, out component))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.StaticObjects | TypeMask.Net | TypeMask.RouteWaypoints;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.transportType = component.m_TransportType;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.BuildingLots;
          if (component.m_PassengerTransport)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Passenger;
          }
          if (component.m_CargoTransport)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Cargo;
          }
          switch (component.m_TransportType)
          {
            case TransportType.Bus:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.Road | Layer.Pathway | Layer.MarkerPathway | Layer.PublicTransportRoad;
              flag = true;
              break;
            case TransportType.Train:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.TrainTrack;
              flag = true;
              break;
            case TransportType.Tram:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.Road | Layer.TramTrack | Layer.PublicTransportRoad;
              flag = true;
              break;
            case TransportType.Ship:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.Waterway;
              break;
            case TransportType.Airplane:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.Taxiway | Layer.MarkerTaxiway;
              break;
            case TransportType.Subway:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.SubwayTrack;
              flag = true;
              break;
            default:
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.netLayerMask = Layer.None;
              break;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.RouteWaypoints;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.netLayerMask = Layer.None;
        }
        if (flag && this.underground)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.collisionMask = CollisionMask.Underground;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.collisionMask = CollisionMask.OnGround | CollisionMask.Overground;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == RouteToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.RouteSegments;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.routeType = componentData.m_Type;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.None;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.netLayerMask = Layer.None;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.routeType = RouteType.None;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      // ISSUE: reference to a compiler-generated field
      bool forceApply = this.m_ForceApply;
      // ISSUE: reference to a compiler-generated field
      bool forceCancel = this.m_ForceCancel;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceApply = false;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceCancel = false;
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        this.allowUnderground = false;
        this.requireUnderground = false;
        this.requireNetArrows = false;
        this.requireNet = Layer.None;
        this.requireStops = TransportType.None;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        RouteData componentData1 = this.m_PrefabSystem.GetComponentData<RouteData>((PrefabBase) this.prefab);
        this.requireRoutes = componentData1.m_Type;
        if (componentData1.m_Type == RouteType.TransportLine)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TransportLineData componentData2 = this.m_PrefabSystem.GetComponentData<TransportLineData>((PrefabBase) this.prefab);
          this.requireNetArrows = true;
          switch (componentData2.m_TransportType)
          {
            case TransportType.Bus:
              this.requireNet |= Layer.Road | Layer.Pathway | Layer.MarkerPathway | Layer.PublicTransportRoad;
              this.allowUnderground = true;
              break;
            case TransportType.Train:
              this.requireNet |= Layer.TrainTrack;
              this.allowUnderground = true;
              break;
            case TransportType.Tram:
              this.requireNet |= Layer.Road | Layer.TramTrack | Layer.PublicTransportRoad;
              this.allowUnderground = true;
              break;
            case TransportType.Ship:
              this.requireNet |= Layer.Waterway;
              break;
            case TransportType.Airplane:
              this.requireNet |= Layer.Taxiway | Layer.MarkerTaxiway;
              break;
            case TransportType.Subway:
              this.requireNet |= Layer.SubwayTrack;
              this.allowUnderground = true;
              break;
          }
          this.requireStops = componentData2.m_TransportType;
        }
        if (this.allowUnderground)
          this.requireUnderground = this.underground;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab));
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != RouteToolSystem.State.Default && !this.applyAction.enabled)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Default;
        }
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_State != RouteToolSystem.State.Remove && this.secondaryApplyAction.WasPressedThisFrame())
          {
            // ISSUE: reference to a compiler-generated method
            return this.Cancel(inputDeps, this.secondaryApplyAction.WasReleasedThisFrame());
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == RouteToolSystem.State.Remove && (forceCancel || this.secondaryApplyAction.WasReleasedThisFrame()))
          {
            // ISSUE: reference to a compiler-generated method
            return this.Cancel(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_State != RouteToolSystem.State.Modify && this.applyAction.WasPressedThisFrame())
          {
            // ISSUE: reference to a compiler-generated method
            return this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          return this.m_State == RouteToolSystem.State.Modify && (forceApply || this.applyAction.WasReleasedThisFrame()) ? this.Apply(inputDeps) : this.Update(inputDeps);
        }
      }
      else
      {
        this.requireUnderground = false;
        this.requireNetArrows = false;
        this.requireRoutes = RouteType.None;
        this.requireNet = Layer.None;
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(Entity.Null);
        // ISSUE: reference to a compiler-generated field
        this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == RouteToolSystem.State.Modify && this.applyAction.WasReleasedThisFrame())
      {
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          return this.Cancel(inputDeps);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_State = RouteToolSystem.State.Default;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == RouteToolSystem.State.Remove && this.secondaryApplyAction.WasReleasedThisFrame())
        {
          // ISSUE: reference to a compiler-generated field
          if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Apply(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Default;
        }
      }
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint)
    {
      Entity entity;
      RaycastHit hit;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out entity, out hit))
      {
        if (this.EntityManager.HasComponent<ConnectedRoute>(hit.m_HitEntity))
          entity = hit.m_HitEntity;
        controlPoint = new ControlPoint(entity, hit);
        return true;
      }
      controlPoint = new ControlPoint();
      return false;
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint, out bool forceUpdate)
    {
      Entity entity;
      RaycastHit hit;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out entity, out hit, out forceUpdate))
      {
        if (this.EntityManager.HasComponent<ConnectedRoute>(hit.m_HitEntity))
          entity = hit.m_HitEntity;
        controlPoint = new ControlPoint(entity, hit);
        return true;
      }
      controlPoint = new ControlPoint();
      return false;
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      return inputDeps;
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TransportLineRemoveSound);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      RouteToolSystem.State state = this.m_State;
      switch (state)
      {
        case RouteToolSystem.State.Default:
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!this.GetAllowApply() || this.m_ControlPoints.Length <= 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          if (!this.EntityManager.HasComponent<Route>(controlPoint1.m_OriginalEntity) || controlPoint1.m_ElementIndex.x < 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Remove;
          // ISSUE: reference to a compiler-generated field
          this.m_MoveStartPosition = controlPoint1;
          // ISSUE: reference to a compiler-generated field
          this.m_ForceCancel = singleFrameOnly;
          ControlPoint controlPoint2;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint2;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints[0] = controlPoint2;
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
          }
          return inputDeps;
        case RouteToolSystem.State.Create:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.RemoveAtSwapBack(this.m_ControlPoints.Length - 1);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length <= 1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = RouteToolSystem.State.Default;
          }
          ControlPoint controlPoint3;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint3))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints[this.m_ControlPoints.Length - 1] = controlPoint3;
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints[this.m_ControlPoints.Length - 1] = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
            }
          }
          return inputDeps;
        case RouteToolSystem.State.Modify:
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Default;
          ControlPoint controlPoint4;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint4))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint4;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint4);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
          }
          return inputDeps;
        case RouteToolSystem.State.Remove:
          Entity native = Entity.Null;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetAllowApply() && !this.m_TempRouteQuery.IsEmptyIgnoreFilter)
          {
            this.applyMode = ApplyMode.Apply;
            // ISSUE: reference to a compiler-generated field
            NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempRouteQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
            native = archetypeChunkArray[0].GetNativeArray(this.GetEntityTypeHandle())[0];
            archetypeChunkArray.Dispose();
          }
          else
            this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          ControlPoint controlPoint5;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint5))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint5;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint5);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, native);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, native);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
          }
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      RouteToolSystem.State state = this.m_State;
      switch (state)
      {
        case RouteToolSystem.State.Default:
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!this.GetAllowApply() || this.m_ControlPoints.Length <= 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TransportLineStartSound);
          if (this.EntityManager.HasComponent<Route>(controlPoint1.m_OriginalEntity) && math.any(controlPoint1.m_ElementIndex >= 0))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = RouteToolSystem.State.Modify;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPointsMoved = controlPoint1.m_ElementIndex.y >= 0;
            // ISSUE: reference to a compiler-generated field
            this.m_MoveStartPosition = controlPoint1;
            // ISSUE: reference to a compiler-generated field
            this.m_ForceApply = singleFrameOnly;
            // ISSUE: reference to a compiler-generated field
            this.m_CanApplyModify = false;
            ControlPoint controlPoint2;
            // ISSUE: reference to a compiler-generated method
            if (this.GetRaycastResult(out controlPoint2))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint2;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints[0] = controlPoint2;
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
              JobHandle.ScheduleBatchedJobs();
              inputDeps.Complete();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPointsMoved |= !this.m_MoveStartPosition.Equals(this.m_ControlPoints[0]);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
            }
            return inputDeps;
          }
          if (controlPoint1.Equals(new ControlPoint()))
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Create;
          // ISSUE: reference to a compiler-generated field
          this.m_MoveStartPosition = new ControlPoint();
          ControlPoint controlPoint3;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint3))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint3;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint3);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint1);
            // ISSUE: reference to a compiler-generated field
            this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
          }
          return inputDeps;
        case RouteToolSystem.State.Create:
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.GetAllowApply() && !this.m_TempRouteQuery.IsEmptyIgnoreFilter && this.GetPathfindCompleted())
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            RouteData componentData = this.m_PrefabSystem.GetComponentData<RouteData>((PrefabBase) this.prefab);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distance(this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_Position, this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_Position) >= (double) RouteUtils.GetMinWaypointDistance(componentData))
            {
              Entity native = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempRouteQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Routes_Route_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ComponentTypeHandle<Route> componentTypeHandle = this.__TypeHandle.__Game_Routes_Route_RO_ComponentTypeHandle;
              if ((archetypeChunkArray[0].GetNativeArray<Route>(ref componentTypeHandle)[0].m_Flags & RouteFlags.Complete) != (RouteFlags) 0)
              {
                this.applyMode = ApplyMode.Apply;
                // ISSUE: reference to a compiler-generated field
                this.m_State = RouteToolSystem.State.Default;
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Clear();
                native = archetypeChunkArray[0].GetNativeArray(this.GetEntityTypeHandle())[0];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TransportLineCompleteSound);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TransportLineBuildSound);
                this.applyMode = ApplyMode.Clear;
              }
              archetypeChunkArray.Dispose();
              ControlPoint controlPoint4;
              // ISSUE: reference to a compiler-generated method
              if (this.GetRaycastResult(out controlPoint4))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LastRaycastPoint = controlPoint4;
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Add(in controlPoint4);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.SnapControlPoints(inputDeps, native);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.UpdateDefinitions(inputDeps, native);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
              }
              return inputDeps;
            }
          }
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
        case RouteToolSystem.State.Modify:
          // ISSUE: reference to a compiler-generated method
          bool allowApply = this.GetAllowApply();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ControlPointsMoved & allowApply && this.m_ControlPoints.Length > 0)
          {
            this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated field
            this.m_State = RouteToolSystem.State.Create;
            // ISSUE: reference to a compiler-generated field
            this.m_MoveStartPosition = new ControlPoint();
            ControlPoint controlPoint5;
            // ISSUE: reference to a compiler-generated method
            if (this.GetRaycastResult(out controlPoint5))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint5;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint5);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(this.m_ControlPoints[0]);
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
            }
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_CanApplyModify)
          {
            Entity native = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (allowApply && !this.m_TempRouteQuery.IsEmptyIgnoreFilter)
            {
              this.applyMode = ApplyMode.Apply;
              // ISSUE: reference to a compiler-generated field
              NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempRouteQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
              native = archetypeChunkArray[0].GetNativeArray(this.GetEntityTypeHandle())[0];
              archetypeChunkArray.Dispose();
            }
            else
              this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated field
            this.m_State = RouteToolSystem.State.Default;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Clear();
            ControlPoint controlPoint6;
            // ISSUE: reference to a compiler-generated method
            if (this.GetRaycastResult(out controlPoint6))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint6;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint6);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, native);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, native);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
            }
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ForceApply = true;
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
        case RouteToolSystem.State.Remove:
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = RouteToolSystem.State.Default;
          ControlPoint controlPoint7;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint7))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint7;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint7);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
          }
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private bool GetPathfindCompleted()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_TempRouteQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index1 = 0; index1 < entityArray.Length; ++index1)
        {
          Entity entity = entityArray[index1];
          EntityManager entityManager = this.EntityManager;
          DynamicBuffer<RouteWaypoint> buffer1 = entityManager.GetBuffer<RouteWaypoint>(entity, true);
          entityManager = this.EntityManager;
          DynamicBuffer<RouteSegment> buffer2 = entityManager.GetBuffer<RouteSegment>(entity, true);
          for (int index2 = 0; index2 < buffer2.Length; ++index2)
          {
            PathTargets component1;
            if (this.EntityManager.TryGetComponent<PathTargets>(buffer2[index2].m_Segment, out component1))
            {
              RouteWaypoint routeWaypoint1 = buffer1[index2];
              RouteWaypoint routeWaypoint2 = buffer1[math.select(index2 + 1, 0, index2 + 1 >= buffer1.Length)];
              Position component2;
              Position component3;
              if (this.EntityManager.TryGetComponent<Position>(routeWaypoint1.m_Waypoint, out component2) && (double) math.distancesq(component1.m_ReadyStartPosition, component2.m_Position) >= 1.0 || this.EntityManager.TryGetComponent<Position>(routeWaypoint2.m_Waypoint, out component3) && (double) math.distancesq(component1.m_ReadyEndPosition, component3.m_Position) >= 1.0)
                return false;
            }
          }
        }
      }
      return true;
    }

    private bool CheckPathUpdates()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_EventQuery.IsEmptyIgnoreFilter)
        return false;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_EventQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PathUpdated> componentTypeHandle = this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Temp> roComponentLookup = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          NativeArray<PathUpdated> nativeArray = archetypeChunkArray[index1].GetNativeArray<PathUpdated>(ref componentTypeHandle);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            if (roComponentLookup.HasComponent(nativeArray[index2].m_Owner))
              return true;
          }
        }
      }
      return false;
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      bool flag = this.CheckPathUpdates();
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == RouteToolSystem.State.Modify)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CanApplyModify = true;
      }
      ControlPoint controlPoint1;
      bool forceUpdate;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint1, out forceUpdate))
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastRaycastPoint = controlPoint1;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint1);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps, Entity.Null);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_LastRaycastPoint.Equals(controlPoint1) && !flag && !forceUpdate)
        {
          this.applyMode = ApplyMode.None;
          return inputDeps;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastRaycastPoint = controlPoint1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[this.m_ControlPoints.Length - 1] = controlPoint1;
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.SnapControlPoints(inputDeps, Entity.Null);
        JobHandle.ScheduleBatchedJobs();
        inputDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        if (controlPoint2.EqualsIgnoreHit(controlPoint3) && !flag && !forceUpdate)
        {
          this.applyMode = ApplyMode.None;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPointsMoved = true;
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.UpdateDefinitions(inputDeps, Entity.Null);
        }
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastRaycastPoint.Equals(controlPoint1))
      {
        if (flag | forceUpdate)
        {
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return this.m_ControlPoints.Length > 0 ? this.UpdateDefinitions(inputDeps, Entity.Null) : inputDeps;
        }
        this.applyMode = ApplyMode.None;
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LastRaycastPoint = controlPoint1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == RouteToolSystem.State.Default && this.m_ControlPoints.Length == 1)
      {
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[this.m_ControlPoints.Length - 1] = new ControlPoint();
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == RouteToolSystem.State.Modify && this.m_ControlPoints.Length >= 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPointsMoved = true;
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[this.m_ControlPoints.Length - 1] = this.m_MoveStartPosition;
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == RouteToolSystem.State.Remove && this.m_ControlPoints.Length >= 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPointsMoved = true;
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[this.m_ControlPoints.Length - 1] = this.m_MoveStartPosition with
        {
          m_OriginalEntity = Entity.Null
        };
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ControlPoints.Length >= 2)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPointsMoved = true;
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[this.m_ControlPoints.Length - 1] = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
      return inputDeps;
    }

    private JobHandle SnapControlPoints(JobHandle inputDeps, Entity applyTempRoute)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new RouteToolSystem.SnapJob()
      {
        m_Snap = this.GetActualSnap(),
        m_State = this.m_State,
        m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab),
        m_ApplyTempRoute = applyTempRoute,
        m_MoveStartPosition = this.m_MoveStartPosition,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
        m_PrefabTransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabTransportStopData = this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ConnectedRoutes = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup,
        m_Waypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_ControlPoints = this.m_ControlPoints
      }.Schedule<RouteToolSystem.SnapJob>(inputDeps);
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps, Entity applyTempRoute)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new RouteToolSystem.CreateDefinitionsJob()
        {
          m_State = this.m_State,
          m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab),
          m_ApplyTempRoute = applyTempRoute,
          m_MoveStartPosition = this.m_MoveStartPosition,
          m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
          m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
          m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_ConnectedRoutes = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup,
          m_Waypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
          m_ControlPoints = this.m_ControlPoints,
          m_Tooltip = this.m_Tooltip,
          m_Color = this.color,
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        }.Schedule<RouteToolSystem.CreateDefinitionsJob>(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      return job0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public RouteToolSystem()
    {
    }

    public enum State
    {
      Default,
      Create,
      Modify,
      Remove,
    }

    public enum Tooltip
    {
      None,
      CreateRoute,
      ModifyWaypoint,
      ModifySegment,
      CreateOrModify,
      AddWaypoint,
      InsertWaypoint,
      MoveWaypoint,
      MergeWaypoints,
      CompleteRoute,
      DeleteRoute,
      RemoveWaypoint,
    }

    [BurstCompile]
    private struct SnapJob : IJob
    {
      [ReadOnly]
      public Snap m_Snap;
      [ReadOnly]
      public RouteToolSystem.State m_State;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public Entity m_ApplyTempRoute;
      [ReadOnly]
      public ControlPoint m_MoveStartPosition;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<TransportStopData> m_PrefabTransportStopData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> m_ConnectedRoutes;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      public NativeList<ControlPoint> m_ControlPoints;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_Prefab;
        // ISSUE: reference to a compiler-generated field
        int index1 = this.m_ControlPoints.Length - 1;
        // ISSUE: reference to a compiler-generated field
        ControlPoint currentPoint = this.m_ControlPoints[index1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_State == RouteToolSystem.State.Modify || this.m_State == RouteToolSystem.State.Remove) && this.m_Waypoints.HasBuffer(this.m_MoveStartPosition.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          prefab = this.m_PrefabRefData[this.m_MoveStartPosition.m_OriginalEntity].m_Prefab;
        }
        // ISSUE: reference to a compiler-generated field
        RouteData routeData = this.m_PrefabRouteData[prefab];
        TransportLineData transportLineData = new TransportLineData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabTransportLineData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          transportLineData = this.m_PrefabTransportLineData[prefab];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        RouteToolSystem.State state = this.m_State;
        switch (state)
        {
          case RouteToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.FindWaypointLocation(routeData, transportLineData, ref currentPoint) && this.m_ApplyTempRoute != Entity.Null && !this.m_ConnectedRoutes.HasBuffer(currentPoint.m_OriginalEntity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Temp temp = this.m_TempData[this.m_ApplyTempRoute];
              if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
              {
                if (temp.m_Original != Entity.Null && currentPoint.m_OriginalEntity == temp.m_Original)
                {
                  currentPoint.m_OriginalEntity = Entity.Null;
                  break;
                }
                break;
              }
              if (temp.m_Original != Entity.Null && currentPoint.m_OriginalEntity == temp.m_Original)
              {
                if (currentPoint.m_ElementIndex.x >= 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[temp.m_Original];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<RouteWaypoint> waypoint2 = this.m_Waypoints[this.m_ApplyTempRoute];
                  // ISSUE: reference to a compiler-generated field
                  float3 position = this.m_PositionData[waypoint1[currentPoint.m_ElementIndex.x].m_Waypoint].m_Position;
                  currentPoint.m_ElementIndex.x = -1;
                  for (int index2 = 0; index2 < waypoint2.Length; ++index2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PositionData[waypoint2[index2].m_Waypoint].m_Position.Equals(position))
                    {
                      currentPoint.m_ElementIndex.x = index2;
                      break;
                    }
                  }
                  break;
                }
                break;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<RouteWaypoint> waypoint = this.m_Waypoints[this.m_ApplyTempRoute];
              for (int index3 = 0; index3 < waypoint.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated field
                float3 position = this.m_PositionData[waypoint[index3].m_Waypoint].m_Position;
                if ((double) math.distance(position, currentPoint.m_Position) < (double) routeData.m_SnapDistance)
                {
                  currentPoint.m_Position = position;
                  // ISSUE: reference to a compiler-generated field
                  currentPoint.m_OriginalEntity = temp.m_Original != Entity.Null ? temp.m_Original : this.m_ApplyTempRoute;
                  currentPoint.m_ElementIndex = new int2(index3, -1);
                  break;
                }
              }
              break;
            }
            break;
          case RouteToolSystem.State.Create:
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (this.FindWaypointLocation(routeData, transportLineData, ref currentPoint) && this.m_ControlPoints.Length >= 3)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint = this.m_ControlPoints[0];
              if ((double) math.distance(controlPoint.m_Position, currentPoint.m_Position) < (double) routeData.m_SnapDistance)
              {
                currentPoint.m_Position = controlPoint.m_Position;
                break;
              }
              break;
            }
            break;
          case RouteToolSystem.State.Modify:
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.FindWaypointLocation(routeData, transportLineData, ref currentPoint) && this.m_Waypoints.HasBuffer(this.m_MoveStartPosition.m_OriginalEntity) && this.m_MoveStartPosition.m_ElementIndex.x >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<RouteWaypoint> waypoint = this.m_Waypoints[this.m_MoveStartPosition.m_OriginalEntity];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int index4 = math.select(this.m_MoveStartPosition.m_ElementIndex.x - 1, waypoint.Length - 1, this.m_MoveStartPosition.m_ElementIndex.x == 0);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int index5 = math.select(this.m_MoveStartPosition.m_ElementIndex.x + 1, 0, this.m_MoveStartPosition.m_ElementIndex.x == waypoint.Length - 1);
              // ISSUE: reference to a compiler-generated field
              float3 position1 = this.m_PositionData[waypoint[index4].m_Waypoint].m_Position;
              // ISSUE: reference to a compiler-generated field
              float3 position2 = this.m_PositionData[waypoint[index5].m_Waypoint].m_Position;
              float num1 = math.distance(currentPoint.m_Position, position1);
              float num2 = math.distance(currentPoint.m_Position, position2);
              if ((double) num1 < (double) routeData.m_SnapDistance && (double) num1 <= (double) num2)
              {
                currentPoint.m_Position = position1;
                break;
              }
              if ((double) num2 < (double) routeData.m_SnapDistance)
              {
                currentPoint.m_Position = position2;
                break;
              }
              break;
            }
            break;
          case RouteToolSystem.State.Remove:
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.FindWaypointLocation(routeData, transportLineData, ref currentPoint) && (currentPoint.m_OriginalEntity != this.m_MoveStartPosition.m_OriginalEntity || math.any(currentPoint.m_ElementIndex != this.m_MoveStartPosition.m_ElementIndex)))
            {
              // ISSUE: reference to a compiler-generated field
              currentPoint = this.m_MoveStartPosition with
              {
                m_OriginalEntity = Entity.Null
              };
              break;
            }
            break;
        }
        currentPoint.m_HitPosition = currentPoint.m_Position;
        currentPoint.m_CurvePosition = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[index1] = currentPoint;
      }

      private bool ValidateStop(TransportLineData transportLineData, Entity stopEntity)
      {
        PrefabRef componentData1;
        TransportStopData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabRefData.TryGetComponent(stopEntity, out componentData1) && this.m_PrefabTransportStopData.TryGetComponent(componentData1.m_Prefab, out componentData2) && componentData2.m_TransportType == transportLineData.m_TransportType;
      }

      private bool FindWaypointLocation(
        RouteData routeData,
        TransportLineData transportLineData,
        ref ControlPoint currentPoint)
      {
        ControlPoint controlPoint = new ControlPoint();
        bool flag = false;
        while (currentPoint.m_OriginalEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_ConnectedRoutes.HasBuffer(currentPoint.m_OriginalEntity) && this.ValidateStop(transportLineData, currentPoint.m_OriginalEntity))
            return true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Waypoints.HasBuffer(currentPoint.m_OriginalEntity) && math.any(currentPoint.m_ElementIndex >= 0))
          {
            if (currentPoint.m_ElementIndex.y >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<RouteWaypoint> waypoint = this.m_Waypoints[currentPoint.m_OriginalEntity];
              int y = currentPoint.m_ElementIndex.y;
              int num1 = math.select(currentPoint.m_ElementIndex.y + 1, 0, currentPoint.m_ElementIndex.y == waypoint.Length - 1);
              // ISSUE: reference to a compiler-generated field
              float3 position1 = this.m_PositionData[waypoint[y].m_Waypoint].m_Position;
              // ISSUE: reference to a compiler-generated field
              float3 position2 = this.m_PositionData[waypoint[num1].m_Waypoint].m_Position;
              float num2 = math.distance(currentPoint.m_Position, position1);
              float num3 = math.distance(currentPoint.m_Position, position2);
              if ((double) num2 < (double) routeData.m_SnapDistance && (double) num2 <= (double) num3)
              {
                currentPoint.m_ElementIndex = new int2(y, -1);
                currentPoint.m_Position = position1;
              }
              else if ((double) num3 < (double) routeData.m_SnapDistance)
              {
                currentPoint.m_ElementIndex = new int2(num1, -1);
                currentPoint.m_Position = position2;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (currentPoint.m_ElementIndex.x >= 0 || this.m_State == RouteToolSystem.State.Default)
              return true;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.HasBuffer(currentPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[currentPoint.m_OriginalEntity];
            float num4 = routeData.m_SnapDistance;
            for (int index = 0; index < subObject1.Length; ++index)
            {
              Entity subObject2 = subObject1[index].m_SubObject;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_ConnectedRoutes.HasBuffer(subObject2) && this.ValidateStop(transportLineData, currentPoint.m_OriginalEntity))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Objects.Transform transform = this.m_TransformData[subObject2];
                float num5 = math.distance(transform.m_Position, currentPoint.m_HitPosition);
                if ((double) num5 < (double) num4)
                {
                  num4 = num5;
                  currentPoint.m_Position = transform.m_Position;
                  currentPoint.m_OriginalEntity = subObject2;
                }
              }
            }
            if ((double) num4 < (double) routeData.m_SnapDistance)
              return true;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!flag && this.m_State != RouteToolSystem.State.Default && this.m_SubLanes.HasBuffer(currentPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[currentPoint.m_OriginalEntity];
            float num6 = routeData.m_SnapDistance;
            for (int index = 0; index < subLane1.Length; ++index)
            {
              Entity subLane2 = subLane1[index].m_SubLane;
              // ISSUE: reference to a compiler-generated method
              if (this.CheckLaneType(routeData, transportLineData, subLane2))
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane2];
                float t;
                float num7 = MathUtils.Distance(curve.m_Bezier, currentPoint.m_HitPosition, out t);
                if ((double) num7 < (double) num6)
                {
                  num6 = num7;
                  controlPoint = currentPoint with
                  {
                    m_OriginalEntity = Entity.Null,
                    m_Position = MathUtils.Position(curve.m_Bezier, t)
                  };
                  flag = true;
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(currentPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            currentPoint.m_OriginalEntity = this.m_OwnerData[currentPoint.m_OriginalEntity].m_Owner;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(currentPoint.m_OriginalEntity))
            {
              // ISSUE: reference to a compiler-generated field
              currentPoint.m_Position = this.m_TransformData[currentPoint.m_OriginalEntity].m_Position;
            }
          }
          else
            currentPoint.m_OriginalEntity = Entity.Null;
        }
        if (flag)
        {
          currentPoint = controlPoint;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == RouteToolSystem.State.Default && this.m_ControlPoints.Length == 1)
        {
          currentPoint = new ControlPoint();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == RouteToolSystem.State.Modify && this.m_ControlPoints.Length >= 1)
          {
            // ISSUE: reference to a compiler-generated field
            currentPoint = this.m_MoveStartPosition;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_State == RouteToolSystem.State.Remove && this.m_ControlPoints.Length >= 1)
            {
              // ISSUE: reference to a compiler-generated field
              currentPoint = this.m_MoveStartPosition;
              currentPoint.m_OriginalEntity = Entity.Null;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ControlPoints.Length >= 2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                currentPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
              }
            }
          }
        }
        return false;
      }

      private bool CheckLaneType(
        RouteData routeData,
        TransportLineData transportLineData,
        Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SlaveLaneData.HasComponent(lane))
          return false;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[lane];
        if (routeData.m_Type == RouteType.TransportLine)
        {
          switch (transportLineData.m_TransportType)
          {
            case TransportType.Bus:
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PrefabCarLaneData.HasComponent(prefabRef.m_Prefab))
                return false;
              // ISSUE: reference to a compiler-generated field
              CarLaneData carLaneData1 = this.m_PrefabCarLaneData[prefabRef.m_Prefab];
              return (carLaneData1.m_RoadTypes & RoadTypes.Car) != RoadTypes.None && carLaneData1.m_MaxSize >= SizeClass.Large;
            case TransportType.Train:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              return this.m_PrefabTrackLaneData.HasComponent(prefabRef.m_Prefab) && (this.m_PrefabTrackLaneData[prefabRef.m_Prefab].m_TrackTypes & TrackTypes.Train) != 0;
            case TransportType.Tram:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              return this.m_PrefabTrackLaneData.HasComponent(prefabRef.m_Prefab) && (this.m_PrefabTrackLaneData[prefabRef.m_Prefab].m_TrackTypes & TrackTypes.Tram) != 0;
            case TransportType.Ship:
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PrefabCarLaneData.HasComponent(prefabRef.m_Prefab))
                return false;
              // ISSUE: reference to a compiler-generated field
              CarLaneData carLaneData2 = this.m_PrefabCarLaneData[prefabRef.m_Prefab];
              return (carLaneData2.m_RoadTypes & RoadTypes.Watercraft) != RoadTypes.None && carLaneData2.m_MaxSize >= SizeClass.Large;
            case TransportType.Airplane:
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PrefabCarLaneData.HasComponent(prefabRef.m_Prefab))
                return false;
              // ISSUE: reference to a compiler-generated field
              CarLaneData carLaneData3 = this.m_PrefabCarLaneData[prefabRef.m_Prefab];
              return (carLaneData3.m_RoadTypes & RoadTypes.Airplane) != RoadTypes.None && carLaneData3.m_MaxSize >= SizeClass.Large;
            case TransportType.Subway:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              return this.m_PrefabTrackLaneData.HasComponent(prefabRef.m_Prefab) && (this.m_PrefabTrackLaneData[prefabRef.m_Prefab].m_TrackTypes & TrackTypes.Subway) != 0;
          }
        }
        return false;
      }
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public RouteToolSystem.State m_State;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public Entity m_ApplyTempRoute;
      [ReadOnly]
      public ControlPoint m_MoveStartPosition;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> m_ConnectedRoutes;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      public Color32 m_Color;
      public NativeValue<RouteToolSystem.Tooltip> m_Tooltip;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
        // ISSUE: reference to a compiler-generated field
        int length1 = this.m_ControlPoints.Length;
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint = this.m_ControlPoints[0];
        if (length1 == 1 && controlPoint.Equals(new ControlPoint()))
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != RouteToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated field
          controlPoint = this.m_MoveStartPosition;
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_ControlPoints[length1 - 1].m_OriginalEntity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Waypoints.HasBuffer(entity1) && this.m_ControlPoints[length1 - 1].m_ElementIndex.x >= 0)
        {
          DynamicBuffer<RouteWaypoint> dynamicBuffer;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ApplyTempRoute != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[this.m_ApplyTempRoute];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer = !(entity1 == temp.m_Original) ? this.m_Waypoints[entity1] : this.m_Waypoints[this.m_ApplyTempRoute];
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer = this.m_Waypoints[entity1];
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints[length1 - 1].m_ElementIndex.x < dynamicBuffer.Length)
          {
            // ISSUE: reference to a compiler-generated field
            entity1 = dynamicBuffer[this.m_ControlPoints[length1 - 1].m_ElementIndex.x].m_Waypoint;
            Connected componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedData.TryGetComponent(entity1, out componentData))
              entity1 = componentData.m_Connected;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(entity1))
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateTempWaypointObject(entity1);
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component1 = new CreationDefinition();
        // ISSUE: reference to a compiler-generated field
        component1.m_Prefab = this.m_Prefab;
        ColorDefinition component2 = new ColorDefinition();
        // ISSUE: reference to a compiler-generated field
        component2.m_Color = this.m_Color;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Waypoints.HasBuffer(controlPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          component1.m_Prefab = this.m_PrefabRefData[controlPoint.m_OriginalEntity].m_Prefab;
        }
        // ISSUE: reference to a compiler-generated field
        float waypointDistance = RouteUtils.GetMinWaypointDistance(this.m_PrefabRouteData[component1.m_Prefab]);
        // ISSUE: reference to a compiler-generated field
        if (this.m_Waypoints.HasBuffer(controlPoint.m_OriginalEntity) && math.any(controlPoint.m_ElementIndex >= 0))
        {
          component1.m_Original = controlPoint.m_OriginalEntity;
          DynamicBuffer<RouteWaypoint> dynamicBuffer1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ApplyTempRoute != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[this.m_ApplyTempRoute];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer1 = !(controlPoint.m_OriginalEntity == temp.m_Original) ? this.m_Waypoints[controlPoint.m_OriginalEntity] : this.m_Waypoints[this.m_ApplyTempRoute];
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer1 = this.m_Waypoints[controlPoint.m_OriginalEntity];
          }
          if (controlPoint.m_ElementIndex.y >= 0)
          {
            int y = controlPoint.m_ElementIndex.y;
            int index1 = math.select(controlPoint.m_ElementIndex.y + 1, 0, controlPoint.m_ElementIndex.y == dynamicBuffer1.Length - 1);
            // ISSUE: reference to a compiler-generated field
            float3 position = this.m_ControlPoints[length1 - 1].m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool flag1 = math.any(new float2(math.distance(position, this.m_PositionData[dynamicBuffer1[y].m_Waypoint].m_Position), math.distance(position, this.m_PositionData[dynamicBuffer1[index1].m_Waypoint].m_Position)) < waypointDistance);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool flag2 = !this.m_MoveStartPosition.Equals(this.m_ControlPoints[length1 - 1]);
            // ISSUE: reference to a compiler-generated field
            bool c = this.m_State == RouteToolSystem.State.Default | flag1 || !flag2;
            int num1 = math.select(length1, length1 - 1, c);
            int length2 = dynamicBuffer1.Length + num1;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<WaypointDefinition> dynamicBuffer2 = this.m_CommandBuffer.AddBuffer<WaypointDefinition>(entity2);
            dynamicBuffer2.ResizeUninitialized(length2);
            int num2 = 0;
            for (int index2 = 0; index2 <= controlPoint.m_ElementIndex.y; ++index2)
            {
              // ISSUE: reference to a compiler-generated method
              dynamicBuffer2[num2++] = this.GetWaypointDefinition(dynamicBuffer1[index2].m_Waypoint);
            }
            for (int index3 = 0; index3 < num1; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              dynamicBuffer2[num2++] = this.GetWaypointDefinition(this.m_ControlPoints[index3]);
            }
            for (int index4 = controlPoint.m_ElementIndex.y + 1; index4 < dynamicBuffer1.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated method
              dynamicBuffer2[num2++] = this.GetWaypointDefinition(dynamicBuffer1[index4].m_Waypoint);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            RouteToolSystem.State state = this.m_State;
            switch (state)
            {
              case RouteToolSystem.State.Default:
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = RouteToolSystem.Tooltip.ModifySegment;
                break;
              case RouteToolSystem.State.Create:
              case RouteToolSystem.State.Remove:
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
                break;
              case RouteToolSystem.State.Modify:
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = flag1 || !flag2 ? RouteToolSystem.Tooltip.None : RouteToolSystem.Tooltip.InsertWaypoint;
                break;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            float3 position = this.m_ControlPoints[length1 - 1].m_Position;
            bool flag3;
            bool flag4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_State == RouteToolSystem.State.Remove)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              flag3 = !(this.m_ControlPoints[length1 - 1].m_OriginalEntity != this.m_MoveStartPosition.m_OriginalEntity) && (double) math.distance(position, this.m_PositionData[dynamicBuffer1[controlPoint.m_ElementIndex.x].m_Waypoint].m_Position) < (double) waypointDistance;
              flag4 = false;
            }
            else
            {
              int index5 = math.select(controlPoint.m_ElementIndex.x - 1, dynamicBuffer1.Length - 1, controlPoint.m_ElementIndex.x == 0);
              int index6 = math.select(controlPoint.m_ElementIndex.x + 1, 0, controlPoint.m_ElementIndex.x == dynamicBuffer1.Length - 1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              flag3 = math.any(new float2(math.distance(position, this.m_PositionData[dynamicBuffer1[index5].m_Waypoint].m_Position), math.distance(position, this.m_PositionData[dynamicBuffer1[index6].m_Waypoint].m_Position)) < waypointDistance);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              flag4 = !this.m_MoveStartPosition.Equals(this.m_ControlPoints[length1 - 1]);
            }
            bool c = flag3;
            int num3 = math.select(length1, length1 - 1, c);
            int length3 = dynamicBuffer1.Length + num3 - 1;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<WaypointDefinition> dynamicBuffer3 = this.m_CommandBuffer.AddBuffer<WaypointDefinition>(entity2);
            dynamicBuffer3.ResizeUninitialized(length3);
            int num4 = 0;
            for (int index = 0; index < controlPoint.m_ElementIndex.x; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              dynamicBuffer3[num4++] = this.GetWaypointDefinition(dynamicBuffer1[index].m_Waypoint);
            }
            for (int index = 0; index < num3; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              WaypointDefinition waypointDefinition = this.GetWaypointDefinition(this.m_ControlPoints[index]) with
              {
                m_Original = dynamicBuffer1[controlPoint.m_ElementIndex.x].m_Waypoint
              };
              dynamicBuffer3[num4++] = waypointDefinition;
            }
            for (int index = controlPoint.m_ElementIndex.x + 1; index < dynamicBuffer1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              dynamicBuffer3[num4++] = this.GetWaypointDefinition(dynamicBuffer1[index].m_Waypoint);
            }
            if (length3 <= 1)
              component1.m_Flags |= CreationFlags.Delete;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            RouteToolSystem.State state = this.m_State;
            switch (state)
            {
              case RouteToolSystem.State.Default:
                Entity waypoint = dynamicBuffer1[controlPoint.m_ElementIndex.x].m_Waypoint;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = !this.m_ConnectedData.HasComponent(waypoint) || !(this.m_ConnectedData[waypoint].m_Connected != Entity.Null) ? RouteToolSystem.Tooltip.ModifyWaypoint : RouteToolSystem.Tooltip.CreateOrModify;
                break;
              case RouteToolSystem.State.Create:
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
                break;
              case RouteToolSystem.State.Modify:
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = length3 > 1 ? (!flag3 ? (!flag4 ? RouteToolSystem.Tooltip.None : RouteToolSystem.Tooltip.MoveWaypoint) : RouteToolSystem.Tooltip.MergeWaypoints) : RouteToolSystem.Tooltip.DeleteRoute;
                break;
              case RouteToolSystem.State.Remove:
                // ISSUE: reference to a compiler-generated field
                this.m_Tooltip.value = length3 > 1 ? (!flag3 ? RouteToolSystem.Tooltip.None : RouteToolSystem.Tooltip.RemoveWaypoint) : RouteToolSystem.Tooltip.DeleteRoute;
                break;
            }
          }
        }
        else
        {
          bool c = false;
          if (length1 >= 2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            c = (double) math.distance(this.m_ControlPoints[length1 - 2].m_Position, this.m_ControlPoints[length1 - 1].m_Position) < (double) waypointDistance;
          }
          int length4 = math.select(length1, length1 - 1, c);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<WaypointDefinition> dynamicBuffer = this.m_CommandBuffer.AddBuffer<WaypointDefinition>(entity2);
          dynamicBuffer.ResizeUninitialized(length4);
          for (int index = 0; index < length4; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            dynamicBuffer[index] = this.GetWaypointDefinition(this.m_ControlPoints[index]);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          RouteToolSystem.State state = this.m_State;
          switch (state)
          {
            case RouteToolSystem.State.Default:
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = length1 != 1 ? RouteToolSystem.Tooltip.None : RouteToolSystem.Tooltip.CreateRoute;
              break;
            case RouteToolSystem.State.Create:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = !c ? (length1 < 3 || !this.m_ControlPoints[0].m_Position.Equals(this.m_ControlPoints[length1 - 1].m_Position) ? RouteToolSystem.Tooltip.AddWaypoint : RouteToolSystem.Tooltip.CompleteRoute) : RouteToolSystem.Tooltip.None;
              break;
            case RouteToolSystem.State.Modify:
            case RouteToolSystem.State.Remove:
              // ISSUE: reference to a compiler-generated field
              this.m_Tooltip.value = RouteToolSystem.Tooltip.None;
              break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity2, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<ColorDefinition>(entity2, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity2, new Updated());
      }

      private void CreateTempWaypointObject(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform = this.m_TransformData[entity];
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Original = entity;
        component1.m_Flags |= CreationFlags.Select;
        ObjectDefinition component2 = new ObjectDefinition();
        component2.m_Position = transform.m_Position;
        component2.m_Rotation = transform.m_Rotation;
        Game.Objects.Elevation componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.TryGetComponent(entity, out componentData))
        {
          component2.m_Elevation = componentData.m_Elevation;
          component2.m_ParentMesh = ObjectUtils.GetSubParentMesh(componentData.m_Flags);
        }
        else
          component2.m_ParentMesh = -1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachedData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          component1.m_Attached = this.m_AttachedData[entity].m_Parent;
          component1.m_Flags |= CreationFlags.Attach;
        }
        component2.m_Probability = 100;
        component2.m_PrefabSubIndex = -1;
        component2.m_LocalPosition = transform.m_Position;
        component2.m_LocalRotation = transform.m_Rotation;
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity1, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity1, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
      }

      private WaypointDefinition GetWaypointDefinition(Entity original)
      {
        // ISSUE: reference to a compiler-generated field
        WaypointDefinition waypointDefinition = new WaypointDefinition(this.m_PositionData[original].m_Position);
        waypointDefinition.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.HasComponent(original))
        {
          // ISSUE: reference to a compiler-generated field
          waypointDefinition.m_Connection = this.m_ConnectedData[original].m_Connected;
        }
        return waypointDefinition;
      }

      private WaypointDefinition GetWaypointDefinition(ControlPoint controlPoint)
      {
        WaypointDefinition waypointDefinition = new WaypointDefinition(controlPoint.m_Position);
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.HasComponent(controlPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          waypointDefinition.m_Connection = this.m_ConnectedData[controlPoint.m_OriginalEntity].m_Connected;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedRoutes.HasBuffer(controlPoint.m_OriginalEntity))
          {
            waypointDefinition.m_Connection = controlPoint.m_OriginalEntity;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Waypoints.HasBuffer(controlPoint.m_OriginalEntity) && controlPoint.m_ElementIndex.x >= 0)
            {
              DynamicBuffer<RouteWaypoint> dynamicBuffer;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ApplyTempRoute != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Temp temp = this.m_TempData[this.m_ApplyTempRoute];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer = !(controlPoint.m_OriginalEntity == temp.m_Original) ? this.m_Waypoints[controlPoint.m_OriginalEntity] : this.m_Waypoints[this.m_ApplyTempRoute];
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer = this.m_Waypoints[controlPoint.m_OriginalEntity];
              }
              if (controlPoint.m_ElementIndex.x < dynamicBuffer.Length)
              {
                Entity waypoint = dynamicBuffer[controlPoint.m_ElementIndex.x].m_Waypoint;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectedData.HasComponent(waypoint))
                {
                  // ISSUE: reference to a compiler-generated field
                  waypointDefinition.m_Connection = this.m_ConnectedData[waypoint].m_Connected;
                }
              }
            }
          }
        }
        return waypointDefinition;
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Route> __Game_Routes_Route_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> __Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportStopData> __Game_Prefabs_TransportStopData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Route_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Route>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopData_RO_ComponentLookup = state.GetComponentLookup<TransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferLookup = state.GetBufferLookup<ConnectedRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
      }
    }
  }
}
