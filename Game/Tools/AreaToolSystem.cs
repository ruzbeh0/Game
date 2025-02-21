// Decompiled with JetBrains decompiler
// Type: Game.Tools.AreaToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Audio;
using Game.Buildings;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Zones;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class AreaToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Area Tool";
    private ObjectToolSystem m_ObjectToolSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private AudioManager m_AudioManager;
    private IProxyAction m_AddOrModifyAreaNode;
    private IProxyAction m_RemoveAreaNode;
    private IProxyAction m_UndoAreaNode;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_TempAreaQuery;
    private EntityQuery m_TempBuildingQuery;
    private EntityQuery m_MapTileQuery;
    private EntityQuery m_SoundQuery;
    private ControlPoint m_LastRaycastPoint;
    private NativeList<ControlPoint> m_ControlPoints;
    private NativeList<ControlPoint> m_MoveStartPositions;
    private NativeValue<AreaToolSystem.Tooltip> m_Tooltip;
    private AreaToolSystem.Mode m_LastMode;
    private AreaToolSystem.State m_State;
    private AreaPrefab m_Prefab;
    private bool m_ControlPointsMoved;
    private bool m_AllowCreateArea;
    private bool m_ForceCancel;
    private AreaToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Area Tool";

    public override int uiModeIndex => (int) this.actualMode;

    public override void GetUIModes(List<ToolMode> modes)
    {
      List<ToolMode> toolModeList1 = modes;
      // ISSUE: variable of a compiler-generated type
      AreaToolSystem.Mode mode1 = AreaToolSystem.Mode.Edit;
      ToolMode toolMode1 = new ToolMode(mode1.ToString(), 0);
      toolModeList1.Add(toolMode1);
      if (!this.allowGenerate)
        return;
      List<ToolMode> toolModeList2 = modes;
      // ISSUE: variable of a compiler-generated type
      AreaToolSystem.Mode mode2 = AreaToolSystem.Mode.Generate;
      ToolMode toolMode2 = new ToolMode(mode2.ToString(), 1);
      toolModeList2.Add(toolMode2);
    }

    public AreaToolSystem.Mode mode { get; set; }

    public AreaToolSystem.Mode actualMode
    {
      get => !this.allowGenerate ? AreaToolSystem.Mode.Edit : this.mode;
    }

    public Entity recreate { get; set; }

    public bool underground { get; set; }

    public bool allowGenerate { get; private set; }

    public AreaToolSystem.State state => this.m_State;

    public AreaToolSystem.Tooltip tooltip => this.m_Tooltip.value;

    public AreaPrefab prefab
    {
      get => this.m_Prefab;
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) this.m_Prefab))
          return;
        this.m_Prefab = value;
        this.allowGenerate = this.m_ToolSystem.actionMode.IsEditor() && value is MapTilePrefab;
        Action<PrefabBase> eventPrefabChanged = this.m_ToolSystem.EventPrefabChanged;
        if (eventPrefabChanged == null)
          return;
        eventPrefabChanged((PrefabBase) value);
      }
    }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_AddOrModifyAreaNode;
        yield return this.m_RemoveAreaNode;
        yield return this.m_UndoAreaNode;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_TempAreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AddOrModifyAreaNode = InputManager.instance.toolActionCollection.GetActionState("Add Or Modify Area Node", nameof (AreaToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_RemoveAreaNode = InputManager.instance.toolActionCollection.GetActionState("Remove Area Node", nameof (AreaToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_UndoAreaNode = InputManager.instance.toolActionCollection.GetActionState("Undo Area Node", nameof (AreaToolSystem));
      this.selectedSnap &= ~Snap.AutoParent;
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints = new NativeList<ControlPoint>(20, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MoveStartPositions = new NativeList<ControlPoint>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip = new NativeValue<AreaToolSystem.Tooltip>(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MoveStartPositions.Dispose();
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
      this.m_MoveStartPositions.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LastRaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_LastMode = this.actualMode;
      // ISSUE: reference to a compiler-generated field
      this.m_State = AreaToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.value = AreaToolSystem.Tooltip.None;
      // ISSUE: reference to a compiler-generated field
      this.m_AllowCreateArea = false;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceCancel = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      this.recreate = Entity.Null;
      // ISSUE: reference to a compiler-generated method
      base.OnStopRunning();
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        this.applyAction.enabled = this.actionsEnabled;
        // ISSUE: reference to a compiler-generated field
        this.applyActionOverride = this.m_AddOrModifyAreaNode;
        this.secondaryApplyAction.enabled = this.actionsEnabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.secondaryApplyActionOverride = this.m_State != AreaToolSystem.State.Create || this.m_ControlPoints.Length <= 1 ? this.m_RemoveAreaNode : this.m_UndoAreaNode;
      }
    }

    public NativeList<ControlPoint> GetControlPoints(
      out NativeList<ControlPoint> moveStartPositions,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      moveStartPositions = this.m_MoveStartPositions;
      dependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      return this.m_ControlPoints;
    }

    public override PrefabBase GetPrefab() => (PrefabBase) this.prefab;

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      if (!(prefab is AreaPrefab areaPrefab))
        return false;
      this.prefab = areaPrefab;
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AreaGeometryData componentData = this.m_PrefabSystem.GetComponentData<AreaGeometryData>((PrefabBase) this.prefab);
        Snap onMask;
        Snap offMask;
        // ISSUE: reference to a compiler-generated method
        this.GetAvailableSnapMask(out onMask, out offMask);
        // ISSUE: reference to a compiler-generated method
        int actualSnap = (int) ToolBaseSystem.GetActualSnap(this.selectedSnap, onMask, offMask);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubElements;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Areas;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.areaTypeMask = AreaUtils.GetTypeMask(componentData.m_Type);
        if ((componentData.m_Flags & Game.Areas.GeometryFlags.OnWaterSurface) != (Game.Areas.GeometryFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.Water;
        }
        if ((actualSnap & 8192) != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.StaticObjects;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ToolSystem.actionMode.IsEditor())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Placeholders;
          }
          if (this.underground)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.collisionMask = CollisionMask.Underground;
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.typeMask &= ~(TypeMask.Terrain | TypeMask.Water);
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.PartialSurface;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((actualSnap & 1) == 0 && this.m_State != AreaToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask &= ~TypeMask.Areas;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Areas;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.areaTypeMask = AreaTypeMask.None;
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ToolSystem.actionMode.IsEditor())
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.UpgradeIsMain;
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_FocusChanged)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      if (this.actualMode != this.m_LastMode)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_MoveStartPositions.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_LastRaycastPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_LastMode = this.actualMode;
        // ISSUE: reference to a compiler-generated field
        this.m_State = AreaToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated field
        this.m_Tooltip.value = AreaToolSystem.Tooltip.None;
        // ISSUE: reference to a compiler-generated field
        this.m_AllowCreateArea = false;
      }
      // ISSUE: reference to a compiler-generated field
      bool forceCancel = this.m_ForceCancel;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceCancel = false;
      DynamicBuffer<Game.Areas.Node> buffer;
      if (this.EntityManager.TryGetBuffer<Game.Areas.Node>(this.recreate, true, out buffer))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_State = AreaToolSystem.State.Create;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length < 3 && buffer.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          ref NativeList<ControlPoint> local1 = ref this.m_ControlPoints;
          ControlPoint controlPoint = new ControlPoint();
          controlPoint.m_OriginalEntity = this.recreate;
          controlPoint.m_ElementIndex = new int2(0, -1);
          controlPoint.m_Position = buffer[0].m_Position;
          controlPoint.m_HitPosition = buffer[0].m_Position;
          ref ControlPoint local2 = ref controlPoint;
          local1.Add(in local2);
          // ISSUE: reference to a compiler-generated field
          ref NativeList<ControlPoint> local3 = ref this.m_ControlPoints;
          controlPoint = new ControlPoint();
          controlPoint.m_OriginalEntity = this.recreate;
          controlPoint.m_ElementIndex = new int2(1, -1);
          controlPoint.m_Position = buffer[1].m_Position;
          controlPoint.m_HitPosition = buffer[1].m_Position;
          ref ControlPoint local4 = ref controlPoint;
          local3.Add(in local4);
          // ISSUE: reference to a compiler-generated field
          ref NativeList<ControlPoint> local5 = ref this.m_ControlPoints;
          controlPoint = new ControlPoint();
          controlPoint.m_ElementIndex = new int2(-1, -1);
          controlPoint.m_Position = math.lerp(buffer[0].m_Position, buffer[1].m_Position, 0.5f);
          controlPoint.m_HitPosition = math.lerp(buffer[0].m_Position, buffer[1].m_Position, 0.5f);
          ref ControlPoint local6 = ref controlPoint;
          local5.Add(in local6);
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AreaGeometryData componentData = this.m_PrefabSystem.GetComponentData<AreaGeometryData>((PrefabBase) this.prefab);
        this.requireAreas = AreaUtils.GetTypeMask(componentData.m_Type);
        this.requireZones = componentData.m_Type == Game.Areas.AreaType.Lot;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AllowCreateArea = (this.m_ToolSystem.actionMode.IsEditor() || componentData.m_Type != Game.Areas.AreaType.Lot) && (componentData.m_Type != Game.Areas.AreaType.Surface || (componentData.m_Flags & Game.Areas.GeometryFlags.ClipTerrain) != (Game.Areas.GeometryFlags) 0 || this.m_PrefabSystem.HasComponent<RenderedAreaData>((PrefabBase) this.prefab));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(this.m_ToolSystem.actionMode.IsEditor() ? Entity.Null : this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AreaToolSystem.GetAvailableSnapMask(componentData, this.m_ToolSystem.actionMode.IsEditor(), out this.m_SnapOnMask, out this.m_SnapOffMask);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.allowUnderground = (ToolBaseSystem.GetActualSnap(this.selectedSnap, this.m_SnapOnMask, this.m_SnapOffMask) & Snap.ObjectSurface) > Snap.None;
        this.requireUnderground = this.allowUnderground && this.underground;
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != AreaToolSystem.State.Default && !this.applyAction.enabled)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated method
          return this.Clear(inputDeps);
        }
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_State != AreaToolSystem.State.Remove && this.secondaryApplyAction.WasPressedThisFrame())
          {
            // ISSUE: reference to a compiler-generated method
            return this.Cancel(inputDeps, this.secondaryApplyAction.WasReleasedThisFrame());
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == AreaToolSystem.State.Remove && (forceCancel || this.secondaryApplyAction.WasReleasedThisFrame()))
          {
            // ISSUE: reference to a compiler-generated method
            return this.Cancel(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_State != AreaToolSystem.State.Modify && this.applyAction.WasPressedThisFrame())
          {
            // ISSUE: reference to a compiler-generated method
            return this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame());
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          return this.m_State == AreaToolSystem.State.Modify && this.applyAction.WasReleasedThisFrame() ? this.Apply(inputDeps) : this.Update(inputDeps);
        }
      }
      else
      {
        this.requireAreas = AreaTypeMask.None;
        this.requireZones = false;
        this.requireUnderground = false;
        // ISSUE: reference to a compiler-generated field
        this.m_AllowCreateArea = false;
        this.allowUnderground = false;
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == AreaToolSystem.State.Modify && this.applyAction.WasReleasedThisFrame())
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
        this.m_State = AreaToolSystem.State.Default;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == AreaToolSystem.State.Remove && this.secondaryApplyAction.WasReleasedThisFrame())
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
          this.m_State = AreaToolSystem.State.Default;
        }
      }
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    public override void GetAvailableSnapMask(out Snap onMask, out Snap offMask)
    {
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        AreaToolSystem.GetAvailableSnapMask(this.m_PrefabSystem.GetComponentData<AreaGeometryData>((PrefabBase) this.prefab), this.m_ToolSystem.actionMode.IsEditor(), out onMask, out offMask);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        base.GetAvailableSnapMask(out onMask, out offMask);
      }
    }

    private static void GetAvailableSnapMask(
      AreaGeometryData prefabAreaData,
      bool editorMode,
      out Snap onMask,
      out Snap offMask)
    {
      onMask = Snap.ExistingGeometry | Snap.StraightDirection;
      offMask = onMask;
      switch (prefabAreaData.m_Type)
      {
        case Game.Areas.AreaType.Lot:
          onMask |= Snap.NetSide | Snap.ObjectSide;
          offMask |= Snap.NetSide | Snap.ObjectSide;
          if (!editorMode)
            break;
          onMask |= Snap.LotGrid | Snap.AutoParent;
          offMask |= Snap.LotGrid | Snap.AutoParent;
          break;
        case Game.Areas.AreaType.District:
          onMask |= Snap.NetMiddle;
          offMask |= Snap.NetMiddle;
          break;
        case Game.Areas.AreaType.Space:
          onMask |= Snap.NetSide | Snap.ObjectSide | Snap.ObjectSurface;
          offMask |= Snap.NetSide | Snap.ObjectSide | Snap.ObjectSurface;
          if (!editorMode)
            break;
          onMask |= Snap.LotGrid | Snap.AutoParent;
          offMask |= Snap.LotGrid | Snap.AutoParent;
          break;
        case Game.Areas.AreaType.Surface:
          onMask |= Snap.NetSide | Snap.ObjectSide;
          offMask |= Snap.NetSide | Snap.ObjectSide;
          if (!editorMode)
            break;
          onMask |= Snap.LotGrid | Snap.AutoParent;
          offMask |= Snap.LotGrid | Snap.AutoParent;
          break;
      }
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      return inputDeps;
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      AreaToolSystem.State state = this.m_State;
      switch (state)
      {
        case AreaToolSystem.State.Default:
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.actualMode == AreaToolSystem.Mode.Generate || !this.GetAllowApply() || this.m_ControlPoints.Length <= 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          if (!this.EntityManager.HasComponent<Area>(controlPoint1.m_OriginalEntity) || controlPoint1.m_ElementIndex.x < 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          DynamicBuffer<Game.Areas.Node> buffer;
          if (this.EntityManager.TryGetBuffer<Game.Areas.Node>(controlPoint1.m_OriginalEntity, true, out buffer) && buffer.Length <= 3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDeleteAreaSound);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolRemovePointSound);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Remove;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPointsMoved = false;
          // ISSUE: reference to a compiler-generated field
          this.m_ForceCancel = singleFrameOnly;
          // ISSUE: reference to a compiler-generated field
          this.m_MoveStartPositions.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_MoveStartPositions.AddRange(this.m_ControlPoints.AsArray());
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          ControlPoint controlPoint2;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint2;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint2);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint1);
          }
          return inputDeps;
        case AreaToolSystem.State.Create:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.RemoveAtSwapBack(this.m_ControlPoints.Length - 1);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length <= 1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = AreaToolSystem.State.Default;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.recreate != Entity.Null && this.m_ControlPoints.Length <= 2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectToolSystem;
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolRemovePointSound);
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
            inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
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
              inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
            }
          }
          return inputDeps;
        case AreaToolSystem.State.Modify:
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Default;
          ControlPoint controlPoint4;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint4))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint4;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint4);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
          }
          return inputDeps;
        case AreaToolSystem.State.Remove:
          NativeArray<Entity> applyTempAreas = new NativeArray<Entity>();
          NativeArray<Entity> applyTempBuildings = new NativeArray<Entity>();
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetAllowApply() && !this.m_TempAreaQuery.IsEmptyIgnoreFilter)
          {
            this.applyMode = ApplyMode.Apply;
            // ISSUE: reference to a compiler-generated field
            applyTempAreas = this.m_TempAreaQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
            // ISSUE: reference to a compiler-generated field
            applyTempBuildings = this.m_TempBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          }
          else
            this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Default;
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
            inputDeps = this.SnapControlPoints(inputDeps, applyTempAreas);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, applyTempAreas, applyTempBuildings);
          }
          if (applyTempAreas.IsCreated)
            applyTempAreas.Dispose(inputDeps);
          if (applyTempBuildings.IsCreated)
            applyTempBuildings.Dispose(inputDeps);
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
      AreaToolSystem.State state = this.m_State;
      switch (state)
      {
        case AreaToolSystem.State.Default:
          if (this.actualMode == AreaToolSystem.Mode.Generate)
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (!this.GetAllowApply() || this.m_TempAreaQuery.IsEmptyIgnoreFilter)
            {
              // ISSUE: reference to a compiler-generated method
              return this.Update(inputDeps);
            }
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> entityArray = this.m_TempAreaQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
            this.applyMode = ApplyMode.Apply;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Clear();
            ControlPoint controlPoint;
            // ISSUE: reference to a compiler-generated method
            if (this.GetRaycastResult(out controlPoint))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDropPointSound);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, entityArray);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, entityArray, new NativeArray<Entity>());
            }
            if (entityArray.IsCreated)
              entityArray.Dispose(inputDeps);
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length <= 0)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          if (this.EntityManager.HasComponent<Area>(controlPoint1.m_OriginalEntity) && math.any(controlPoint1.m_ElementIndex >= 0) && !singleFrameOnly)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = AreaToolSystem.State.Modify;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPointsMoved = false;
            // ISSUE: reference to a compiler-generated field
            this.m_MoveStartPositions.Clear();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_MoveStartPositions.AddRange(this.m_ControlPoints.AsArray());
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Clear();
            ControlPoint controlPoint2;
            // ISSUE: reference to a compiler-generated method
            if (this.GetRaycastResult(out controlPoint2))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint2;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolSelectPointSound);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
              JobHandle.ScheduleBatchedJobs();
              inputDeps.Complete();
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint3 = this.m_ControlPoints[0];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_MoveStartPositions[0].Equals(controlPoint3))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                float minNodeDistance = AreaUtils.GetMinNodeDistance(this.m_PrefabSystem.GetComponentData<AreaGeometryData>((PrefabBase) this.prefab));
                // ISSUE: reference to a compiler-generated field
                if ((double) math.distance(this.m_MoveStartPositions[0].m_Position, controlPoint3.m_Position) < (double) minNodeDistance * 0.5)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_ControlPoints[0] = this.m_MoveStartPositions[0];
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ControlPointsMoved = true;
                }
              }
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint1);
            }
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!this.GetAllowApply() || controlPoint1.Equals(new ControlPoint()) || !this.m_AllowCreateArea)
          {
            // ISSUE: reference to a compiler-generated method
            return this.Update(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Create;
          // ISSUE: reference to a compiler-generated field
          this.m_MoveStartPositions.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDropPointSound);
          ControlPoint controlPoint4;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint4))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint4;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint4);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint1);
          }
          return inputDeps;
        case AreaToolSystem.State.Create:
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetAllowApply() && !this.m_TempAreaQuery.IsEmptyIgnoreFilter)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            AreaGeometryData componentData = this.m_PrefabSystem.GetComponentData<AreaGeometryData>((PrefabBase) this.prefab);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distance(this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_Position, this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_Position) >= (double) AreaUtils.GetMinNodeDistance(componentData))
            {
              bool flag = true;
              // ISSUE: reference to a compiler-generated field
              NativeArray<Area> componentDataArray = this.m_TempAreaQuery.ToComponentDataArray<Area>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
              for (int index = 0; index < componentDataArray.Length; ++index)
                flag &= (componentDataArray[index].m_Flags & AreaFlags.Complete) != 0;
              componentDataArray.Dispose();
              NativeArray<Entity> applyTempAreas = new NativeArray<Entity>();
              if (flag)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolFinishAreaSound);
                this.applyMode = ApplyMode.Apply;
                // ISSUE: reference to a compiler-generated field
                this.m_State = AreaToolSystem.State.Default;
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Clear();
                if (this.recreate != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Move)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectToolSystem;
                  }
                  return inputDeps;
                }
                // ISSUE: reference to a compiler-generated field
                applyTempAreas = this.m_TempAreaQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDropPointSound);
                this.applyMode = ApplyMode.Clear;
              }
              ControlPoint controlPoint5;
              // ISSUE: reference to a compiler-generated method
              if (this.GetRaycastResult(out controlPoint5))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LastRaycastPoint = controlPoint5;
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Add(in controlPoint5);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.SnapControlPoints(inputDeps, applyTempAreas);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.UpdateDefinitions(inputDeps, applyTempAreas, new NativeArray<Entity>());
              }
              if (applyTempAreas.IsCreated)
                applyTempAreas.Dispose(inputDeps);
              return inputDeps;
            }
          }
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
        case AreaToolSystem.State.Modify:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ControlPointsMoved && this.GetAllowApply() && this.m_ControlPoints.Length > 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_AllowCreateArea)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint6 = this.m_ControlPoints[0];
              this.applyMode = ApplyMode.Clear;
              // ISSUE: reference to a compiler-generated field
              this.m_State = AreaToolSystem.State.Create;
              // ISSUE: reference to a compiler-generated field
              this.m_MoveStartPositions.Clear();
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Clear();
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint6);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDropPointSound);
              ControlPoint controlPoint7;
              // ISSUE: reference to a compiler-generated method
              if (this.GetRaycastResult(out controlPoint7))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_LastRaycastPoint = controlPoint7;
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Add(in controlPoint7);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Add(in controlPoint6);
              }
              return inputDeps;
            }
            this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated field
            this.m_State = AreaToolSystem.State.Default;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Clear();
            ControlPoint controlPoint8;
            // ISSUE: reference to a compiler-generated method
            if (this.GetRaycastResult(out controlPoint8))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint8;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint8);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDropPointSound);
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
            }
            return inputDeps;
          }
          NativeArray<Entity> applyTempAreas1 = new NativeArray<Entity>();
          NativeArray<Entity> applyTempBuildings = new NativeArray<Entity>();
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetAllowApply() && !this.m_TempAreaQuery.IsEmptyIgnoreFilter)
          {
            this.applyMode = ApplyMode.Apply;
            // ISSUE: reference to a compiler-generated field
            applyTempAreas1 = this.m_TempAreaQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
            // ISSUE: reference to a compiler-generated field
            applyTempBuildings = this.m_TempBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          }
          else
            this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          ControlPoint controlPoint9;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint9))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint9;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint9);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolDropPointSound);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, applyTempAreas1);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, applyTempAreas1, applyTempBuildings);
          }
          if (applyTempAreas1.IsCreated)
            applyTempAreas1.Dispose(inputDeps);
          if (applyTempBuildings.IsCreated)
            applyTempBuildings.Dispose(inputDeps);
          return inputDeps;
        case AreaToolSystem.State.Remove:
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_State = AreaToolSystem.State.Default;
          ControlPoint controlPoint10;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint10))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint10;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint10);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolRemovePointSound);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
          }
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private JobHandle Update(JobHandle inputDeps)
    {
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
          inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_LastRaycastPoint.Equals(controlPoint1) && !forceUpdate)
        {
          this.applyMode = ApplyMode.None;
          return inputDeps;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastRaycastPoint = controlPoint1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int index = math.select(0, this.m_ControlPoints.Length - 1, this.m_State == AreaToolSystem.State.Create);
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[index];
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[index] = controlPoint1;
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.SnapControlPoints(inputDeps, new NativeArray<Entity>());
        JobHandle.ScheduleBatchedJobs();
        inputDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint3 = this.m_ControlPoints[index];
        if (controlPoint2.EqualsIgnoreHit(controlPoint3))
        {
          this.applyMode = ApplyMode.None;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          float minNodeDistance = AreaUtils.GetMinNodeDistance(this.m_PrefabSystem.GetComponentData<AreaGeometryData>((PrefabBase) this.prefab));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == AreaToolSystem.State.Modify && !this.m_ControlPointsMoved && (double) math.distance(controlPoint2.m_Position, controlPoint3.m_Position) < (double) minNodeDistance * 0.5)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints[index] = controlPoint2;
            this.applyMode = ApplyMode.None;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPointsMoved = true;
            this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
          }
        }
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastRaycastPoint.Equals(controlPoint1))
      {
        if (forceUpdate)
        {
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
        }
        this.applyMode = ApplyMode.None;
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LastRaycastPoint = controlPoint1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == AreaToolSystem.State.Default && this.m_ControlPoints.Length >= 1)
      {
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Add(new ControlPoint());
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == AreaToolSystem.State.Modify && this.m_ControlPoints.Length >= 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPointsMoved = true;
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[0] = this.m_MoveStartPositions[0];
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == AreaToolSystem.State.Remove && this.m_ControlPoints.Length >= 1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPointsMoved = true;
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints[0] = this.m_MoveStartPositions[0];
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ControlPoints.Length < 2)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPointsMoved = true;
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints[this.m_ControlPoints.Length - 1] = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps, new NativeArray<Entity>(), new NativeArray<Entity>());
    }

    private JobHandle SnapControlPoints(JobHandle inputDeps, NativeArray<Entity> applyTempAreas)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AreaToolSystem.SnapJob jobData = new AreaToolSystem.SnapJob()
      {
        m_AllowCreateArea = this.m_AllowCreateArea,
        m_ControlPointsMoved = this.m_ControlPointsMoved,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Snap = this.GetActualSnap(),
        m_State = this.m_State,
        m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab),
        m_ApplyTempAreas = applyTempAreas,
        m_MoveStartPositions = this.m_MoveStartPositions,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabAreaData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_AssetStampData = this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_CachedNodes = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies1),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3),
        m_ControlPoints = this.m_ControlPoints
      };
      inputDeps = JobHandle.CombineDependencies(inputDeps, JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3));
      JobHandle dependsOn = inputDeps;
      JobHandle jobHandle = jobData.Schedule<AreaToolSystem.SnapJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      return jobHandle;
    }

    private JobHandle UpdateDefinitions(
      JobHandle inputDeps,
      NativeArray<Entity> applyTempAreas,
      NativeArray<Entity> applyTempBuildings)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        if (this.mode == AreaToolSystem.Mode.Generate)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          JobHandle jobHandle = new AreaToolSystem.RemoveMapTilesJob()
          {
            m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
            m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
            m_CacheType = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle,
            m_ControlPoints = this.m_ControlPoints,
            m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
          }.ScheduleParallel<AreaToolSystem.RemoveMapTilesJob>(this.m_MapTileQuery, inputDeps);
          // ISSUE: reference to a compiler-generated field
          this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
          job0 = JobHandle.CombineDependencies(job0, jobHandle);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Area_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle1 = new AreaToolSystem.CreateDefinitionsJob()
        {
          m_AllowCreateArea = this.m_AllowCreateArea,
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_Mode = this.actualMode,
          m_State = this.m_State,
          m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab),
          m_Recreate = this.recreate,
          m_ApplyTempAreas = applyTempAreas,
          m_ApplyTempBuildings = applyTempBuildings,
          m_MoveStartPositions = this.m_MoveStartPositions,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_ClearData = this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup,
          m_SpaceData = this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup,
          m_AreaData = this.__TypeHandle.__Game_Areas_Area_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PrefabAreaData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
          m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
          m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
          m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
          m_CachedNodes = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
          m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_ControlPoints = this.m_ControlPoints,
          m_Tooltip = this.m_Tooltip,
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        }.Schedule<AreaToolSystem.CreateDefinitionsJob>(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle1);
        job0 = JobHandle.CombineDependencies(job0, jobHandle1);
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
    public AreaToolSystem()
    {
    }

    public enum Mode
    {
      Edit,
      Generate,
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
      CreateArea,
      ModifyNode,
      ModifyEdge,
      CreateAreaOrModifyNode,
      CreateAreaOrModifyEdge,
      AddNode,
      InsertNode,
      MoveNode,
      MergeNodes,
      CompleteArea,
      DeleteArea,
      RemoveNode,
      GenerateAreas,
    }

    [BurstCompile]
    private struct SnapJob : IJob
    {
      [ReadOnly]
      public bool m_AllowCreateArea;
      [ReadOnly]
      public bool m_ControlPointsMoved;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public Snap m_Snap;
      [ReadOnly]
      public AreaToolSystem.State m_State;
      [ReadOnly]
      public Entity m_Prefab;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<Entity> m_ApplyTempAreas;
      [ReadOnly]
      public NativeList<ControlPoint> m_MoveStartPositions;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndGeometryData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_BuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<AssetStampData> m_AssetStampData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_LotData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_CachedNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      public NativeList<ControlPoint> m_ControlPoints;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AreaGeometryData areaGeometryData = this.m_PrefabAreaData[this.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int index1 = math.select(0, this.m_ControlPoints.Length - 1, this.m_State == AreaToolSystem.State.Create);
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint1 = this.m_ControlPoints[index1];
        controlPoint1.m_Position = controlPoint1.m_HitPosition;
        ControlPoint bestSnapPosition = controlPoint1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.State state = this.m_State;
        switch (state)
        {
          case AreaToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated method
            if (this.FindControlPoint(ref bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance, controlPoint1.m_OriginalEntity, false, 0))
            {
              // ISSUE: reference to a compiler-generated method
              this.FixControlPointPosition(ref bestSnapPosition);
              break;
            }
            // ISSUE: reference to a compiler-generated field
            if (!this.m_AllowCreateArea)
            {
              bestSnapPosition = new ControlPoint();
              break;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindParent(ref bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance);
              break;
            }
            bestSnapPosition.m_ElementIndex = (int2) -1;
            break;
          case AreaToolSystem.State.Create:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.FindControlPoint(ref bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance, Entity.Null, false, this.m_ControlPoints.Length - 3);
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 4)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint2 = this.m_ControlPoints[0];
              if ((double) math.distance(controlPoint2.m_Position, bestSnapPosition.m_Position) < (double) areaGeometryData.m_SnapDistance * 0.5)
                bestSnapPosition.m_Position = controlPoint2.m_Position;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindParent(ref bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ControlPoints.Length >= 2 && this.m_Nodes.HasBuffer(this.m_ControlPoints[0].m_OriginalEntity))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_ControlPoints[0] = this.m_ControlPoints[0] with
                {
                  m_ElementIndex = new int2(this.FindParentMesh(this.m_ControlPoints[0]), -1)
                };
                break;
              }
              break;
            }
            bestSnapPosition.m_ElementIndex = (int2) -1;
            break;
          case AreaToolSystem.State.Modify:
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPointsMoved)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindControlPoint(ref bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance, Entity.Null, true, 0);
              float num1 = areaGeometryData.m_SnapDistance * 0.5f;
              // ISSUE: reference to a compiler-generated field
              for (int index2 = 0; index2 < this.m_MoveStartPositions.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                ControlPoint moveStartPosition = this.m_MoveStartPositions[index2];
                // ISSUE: reference to a compiler-generated field
                if (this.m_Nodes.HasBuffer(moveStartPosition.m_OriginalEntity) && moveStartPosition.m_ElementIndex.x >= 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[moveStartPosition.m_OriginalEntity];
                  int index3 = math.select(moveStartPosition.m_ElementIndex.x - 1, node.Length - 1, moveStartPosition.m_ElementIndex.x == 0);
                  int index4 = math.select(moveStartPosition.m_ElementIndex.x + 1, 0, moveStartPosition.m_ElementIndex.x == node.Length - 1);
                  float3 position1 = node[index3].m_Position;
                  float3 position2 = node[index4].m_Position;
                  float num2 = math.distance(bestSnapPosition.m_Position, position1);
                  float num3 = math.distance(bestSnapPosition.m_Position, position2);
                  if ((double) num2 < (double) num1)
                  {
                    bestSnapPosition.m_Position = position1;
                    num1 = num2;
                  }
                  if ((double) num3 < (double) num1)
                  {
                    bestSnapPosition.m_Position = position2;
                    num1 = num3;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              bestSnapPosition.m_ElementIndex = !this.m_EditorMode ? (int2) -1 : new int2(this.FindParentMesh(controlPoint1), -1);
              break;
            }
            // ISSUE: reference to a compiler-generated method
            this.FindControlPoint(ref bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance, controlPoint1.m_OriginalEntity, false, 0);
            // ISSUE: reference to a compiler-generated method
            this.FixControlPointPosition(ref bestSnapPosition);
            break;
          case AreaToolSystem.State.Remove:
            // ISSUE: reference to a compiler-generated field
            bestSnapPosition = this.m_MoveStartPositions[0];
            break;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == AreaToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in bestSnapPosition);
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Nodes.HasBuffer(bestSnapPosition.m_OriginalEntity) || !math.any(bestSnapPosition.m_ElementIndex >= 0))
            return;
          // ISSUE: reference to a compiler-generated method
          this.AddControlPoints(bestSnapPosition, controlPoint1, areaGeometryData.m_Type, areaGeometryData.m_SnapDistance * 0.5f);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints[index1] = bestSnapPosition;
        }
      }

      private void FindParent(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        Game.Areas.AreaType type,
        float snapDistance)
      {
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.AutoParent) != Snap.None)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AreaToolSystem.SnapJob.ParentObjectIterator iterator = new AreaToolSystem.SnapJob.ParentObjectIterator()
          {
            m_BoundsOffset = (float) ((double) snapDistance * 0.125 + 0.40000000596046448),
            m_MaxDistance = snapDistance * 0.125f,
            m_TransformData = this.m_TransformData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_BuildingData = this.m_PrefabBuildingData,
            m_ObjectGeometryData = this.m_ObjectGeometryData
          };
          Entity entity1 = controlPoint.m_OriginalEntity;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(entity1, out componentData) && !this.m_BuildingData.HasComponent(entity1))
              entity1 = componentData.m_Owner;
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstalledUpgrades.TryGetBuffer(entity1, out bufferData) && bufferData.Length != 0)
              entity1 = bufferData[0].m_Upgrade;
          }
          // ISSUE: reference to a compiler-generated field
          int num = math.max(1, this.m_ControlPoints.Length - 1);
          for (int index = 0; index < num; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Line.a = index != this.m_ControlPoints.Length - 1 ? this.m_ControlPoints[index].m_Position : bestSnapPosition.m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Line.b = index + 1 < this.m_ControlPoints.Length - 1 ? this.m_ControlPoints[index + 1].m_Position : bestSnapPosition.m_Position;
            // ISSUE: reference to a compiler-generated field
            this.m_ObjectSearchTree.Iterate<AreaToolSystem.SnapJob.ParentObjectIterator>(ref iterator);
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_Parent != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = iterator.m_Parent;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorMode)
              {
                Owner componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                while (this.m_OwnerData.TryGetComponent(entity2, out componentData) && !this.m_BuildingData.HasComponent(entity2))
                  entity2 = componentData.m_Owner;
                DynamicBuffer<InstalledUpgrade> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_InstalledUpgrades.TryGetBuffer(entity2, out bufferData) && bufferData.Length != 0)
                  entity2 = bufferData[0].m_Upgrade;
              }
              // ISSUE: reference to a compiler-generated method
              bestSnapPosition.m_ElementIndex = !(entity2 != entity1) ? new int2(this.FindParentMesh(controlPoint), -1) : (int2) -1;
              // ISSUE: reference to a compiler-generated field
              bestSnapPosition.m_OriginalEntity = iterator.m_Parent;
              return;
            }
          }
        }
        bestSnapPosition.m_OriginalEntity = Entity.Null;
        bestSnapPosition.m_ElementIndex = (int2) -1;
      }

      private int FindParentMesh(ControlPoint controlPoint)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(controlPoint.m_OriginalEntity))
          return controlPoint.m_ElementIndex.x;
        DynamicBuffer<Game.Areas.Node> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Nodes.TryGetBuffer(controlPoint.m_OriginalEntity, out bufferData1) || bufferData1.Length < 2)
          return -1;
        int parentMesh = 0;
        float num1 = float.MaxValue;
        Game.Areas.Node node1 = bufferData1[bufferData1.Length - 1];
        LocalNodeCache localNodeCache1 = new LocalNodeCache();
        DynamicBuffer<LocalNodeCache> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CachedNodes.TryGetBuffer(controlPoint.m_OriginalEntity, out bufferData2))
          localNodeCache1 = bufferData2[bufferData1.Length - 1];
        for (int index = 0; index < bufferData1.Length; ++index)
        {
          Game.Areas.Node node2 = bufferData1[index];
          LocalNodeCache localNodeCache2 = new LocalNodeCache();
          if (bufferData2.IsCreated)
            localNodeCache2 = bufferData2[index];
          float t;
          float num2 = MathUtils.DistanceSquared(new Line3.Segment(node1.m_Position, node2.m_Position), controlPoint.m_HitPosition, out t);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            parentMesh = !bufferData2.IsCreated ? math.select(0, -1, ((double) t >= 0.5 ? (double) node2.m_Elevation : (double) node1.m_Elevation) == -3.4028234663852886E+38) : ((double) t >= 0.5 ? localNodeCache2.m_ParentMesh : localNodeCache1.m_ParentMesh);
          }
          node1 = node2;
          localNodeCache1 = localNodeCache2;
        }
        return parentMesh;
      }

      private void FixControlPointPosition(ref ControlPoint bestSnapPosition)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Nodes.HasBuffer(bestSnapPosition.m_OriginalEntity) || bestSnapPosition.m_ElementIndex.x < 0)
          return;
        Entity entity = bestSnapPosition.m_OriginalEntity;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ApplyTempAreas.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ApplyTempAreas.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity applyTempArea = this.m_ApplyTempAreas[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData[applyTempArea].m_Original == entity)
            {
              entity = applyTempArea;
              break;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[entity];
        bestSnapPosition.m_Position = node[bestSnapPosition.m_ElementIndex.x].m_Position;
      }

      private void AddControlPoints(
        ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        Game.Areas.AreaType type,
        float snapDistance)
      {
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
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.SnapJob.AreaIterator2 iterator = new AreaToolSystem.SnapJob.AreaIterator2()
        {
          m_EditorMode = this.m_EditorMode,
          m_AreaType = type,
          m_Bounds = new Bounds3(controlPoint.m_HitPosition - snapDistance, controlPoint.m_HitPosition + snapDistance),
          m_MaxDistance1 = snapDistance * 0.1f,
          m_MaxDistance2 = snapDistance,
          m_ControlPoint1 = bestSnapPosition,
          m_ControlPoint2 = controlPoint,
          m_ControlPoints = this.m_ControlPoints,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabAreaData = this.m_PrefabAreaData,
          m_LotData = this.m_LotData,
          m_OwnerData = this.m_OwnerData,
          m_Nodes = this.m_Nodes,
          m_Triangles = this.m_Triangles,
          m_InstalledUpgrades = this.m_InstalledUpgrades
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ApplyTempAreas.IsCreated && this.m_ApplyTempAreas.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          iterator.m_IgnoreAreas = new NativeParallelHashSet<Entity>(this.m_ApplyTempAreas.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ApplyTempAreas.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity applyTempArea = this.m_ApplyTempAreas[index];
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[applyTempArea];
            // ISSUE: reference to a compiler-generated field
            iterator.m_IgnoreAreas.Add(temp.m_Original);
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((!this.m_OwnerData.TryGetComponent(applyTempArea, out componentData) || !this.m_Nodes.HasBuffer(componentData.m_Owner)) && (temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
            {
              Entity area = (temp.m_Flags & TempFlags.Create) != (TempFlags) 0 ? applyTempArea : temp.m_Original;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[applyTempArea];
              for (int x = 0; x < node.Length; ++x)
              {
                int2 nodeIndex = new int2(x, math.select(x + 1, 0, x == node.Length - 1));
                Line3.Segment line = new Line3.Segment(node[nodeIndex.x].m_Position, node[nodeIndex.y].m_Position);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                iterator.CheckLine(line, snapDistance, area, nodeIndex, this.m_LotData.HasComponent(applyTempArea));
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<AreaToolSystem.SnapJob.AreaIterator2>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        if (!iterator.m_IgnoreAreas.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        iterator.m_IgnoreAreas.Dispose();
      }

      private bool FindControlPoint(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        Game.Areas.AreaType type,
        float snapDistance,
        Entity preferredArea,
        bool ignoreStartPositions,
        int selfSnap)
      {
        bestSnapPosition.m_OriginalEntity = Entity.Null;
        NativeList<SnapLine> snapLines = new NativeList<SnapLine>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.StraightDirection) != Snap.None)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == AreaToolSystem.State.Create)
          {
            ControlPoint controlPoint1 = controlPoint with
            {
              m_OriginalEntity = Entity.Null,
              m_Position = controlPoint.m_HitPosition
            };
            float3 resultDir = new float3();
            float maxValue = float.MaxValue;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
              if (!controlPoint2.m_Direction.Equals(new float2()))
                ToolUtils.DirectionSnap(ref maxValue, ref controlPoint1.m_Position, ref resultDir, controlPoint.m_HitPosition, controlPoint2.m_Position, new float3(controlPoint2.m_Direction.x, 0.0f, controlPoint2.m_Direction.y), snapDistance);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint4 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
              float2 float2 = math.normalizesafe(controlPoint3.m_Position.xz - controlPoint4.m_Position.xz);
              if (!float2.Equals(new float2()))
                ToolUtils.DirectionSnap(ref maxValue, ref controlPoint1.m_Position, ref resultDir, controlPoint.m_HitPosition, controlPoint4.m_Position, new float3(float2.x, 0.0f, float2.y), snapDistance);
            }
            if (!resultDir.Equals(new float3()))
            {
              controlPoint1.m_Direction = resultDir.xz;
              controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
              ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint1);
              float3 position = controlPoint1.m_Position;
              float3 endPos = position;
              endPos.xz += controlPoint1.m_Direction;
              ToolUtils.AddSnapLine(ref bestSnapPosition, snapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(position, endPos), SnapLineFlags.Hidden));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_State == AreaToolSystem.State.Modify)
            {
              // ISSUE: reference to a compiler-generated field
              for (int index = 0; index < this.m_MoveStartPositions.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated field
                ControlPoint moveStartPosition = this.m_MoveStartPositions[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_Nodes.HasBuffer(moveStartPosition.m_OriginalEntity) && math.any(moveStartPosition.m_ElementIndex >= 0))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[moveStartPosition.m_OriginalEntity];
                  if (node.Length >= 3)
                  {
                    int4 a = math.select(moveStartPosition.m_ElementIndex.x + new int4(-2, -1, 1, 2), moveStartPosition.m_ElementIndex.y + new int4(-1, 0, 1, 2), moveStartPosition.m_ElementIndex.y >= 0);
                    a = math.select(a, a + new int2(node.Length, -node.Length).xxyy, new bool4(a.xy < 0, a.zw >= node.Length));
                    float3 position1 = node[a.x].m_Position;
                    float3 position2 = node[a.y].m_Position;
                    float3 position3 = node[a.z].m_Position;
                    float3 position4 = node[a.w].m_Position;
                    float2 float2_1 = math.normalizesafe(position1.xz - position2.xz);
                    float2 float2_2 = math.normalizesafe(position4.xz - position3.xz);
                    if (!float2_1.Equals(new float2()))
                    {
                      ControlPoint controlPoint5 = controlPoint with
                      {
                        m_OriginalEntity = Entity.Null,
                        m_Position = controlPoint.m_HitPosition
                      };
                      float3 resultDir = new float3();
                      float maxValue = float.MaxValue;
                      ToolUtils.DirectionSnap(ref maxValue, ref controlPoint5.m_Position, ref resultDir, controlPoint.m_HitPosition, position2, new float3(float2_1.x, 0.0f, float2_1.y), snapDistance);
                      if (!resultDir.Equals(new float3()))
                      {
                        controlPoint5.m_Direction = resultDir.xz;
                        controlPoint5.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, controlPoint.m_HitPosition.xz, controlPoint5.m_Position.xz, controlPoint5.m_Direction);
                        ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint5);
                        float3 position5 = controlPoint5.m_Position;
                        float3 endPos = position5;
                        endPos.xz += controlPoint5.m_Direction;
                        ToolUtils.AddSnapLine(ref bestSnapPosition, snapLines, new SnapLine(controlPoint5, NetUtils.StraightCurve(position5, endPos), SnapLineFlags.Hidden));
                      }
                    }
                    if (!float2_2.Equals(new float2()))
                    {
                      ControlPoint controlPoint6 = controlPoint with
                      {
                        m_OriginalEntity = Entity.Null,
                        m_Position = controlPoint.m_HitPosition
                      };
                      float3 resultDir = new float3();
                      float maxValue = float.MaxValue;
                      ToolUtils.DirectionSnap(ref maxValue, ref controlPoint6.m_Position, ref resultDir, controlPoint.m_HitPosition, position3, new float3(float2_2.x, 0.0f, float2_2.y), snapDistance);
                      if (!resultDir.Equals(new float3()))
                      {
                        controlPoint6.m_Direction = resultDir.xz;
                        controlPoint6.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, controlPoint.m_HitPosition.xz, controlPoint6.m_Position.xz, controlPoint6.m_Direction);
                        ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint6);
                        float3 position6 = controlPoint6.m_Position;
                        float3 endPos = position6;
                        endPos.xz += controlPoint6.m_Direction;
                        ToolUtils.AddSnapLine(ref bestSnapPosition, snapLines, new SnapLine(controlPoint6, NetUtils.StraightCurve(position6, endPos), SnapLineFlags.Hidden));
                      }
                    }
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((((this.m_Snap & Snap.ExistingGeometry) != Snap.None ? 1 : (preferredArea != Entity.Null ? 1 : 0)) | (ignoreStartPositions ? 1 : 0)) != 0 || selfSnap >= 1)
        {
          // ISSUE: reference to a compiler-generated field
          float snapDistance1 = math.select(snapDistance, snapDistance * 0.5f, (this.m_Snap & Snap.ExistingGeometry) == Snap.None);
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
          // ISSUE: variable of a compiler-generated type
          AreaToolSystem.SnapJob.AreaIterator iterator = new AreaToolSystem.SnapJob.AreaIterator()
          {
            m_EditorMode = this.m_EditorMode,
            m_IgnoreStartPositions = ignoreStartPositions,
            m_Snap = this.m_Snap,
            m_AreaType = type,
            m_Bounds = new Bounds3(controlPoint.m_HitPosition - snapDistance1, controlPoint.m_HitPosition + snapDistance1),
            m_MaxDistance = snapDistance1,
            m_PreferArea = preferredArea,
            m_ControlPoint = controlPoint,
            m_BestSnapPosition = bestSnapPosition,
            m_SnapLines = snapLines,
            m_MoveStartPositions = this.m_MoveStartPositions,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabAreaData = this.m_PrefabAreaData,
            m_LotData = this.m_LotData,
            m_OwnerData = this.m_OwnerData,
            m_Nodes = this.m_Nodes,
            m_Triangles = this.m_Triangles,
            m_InstalledUpgrades = this.m_InstalledUpgrades
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ApplyTempAreas.IsCreated && this.m_ApplyTempAreas.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_IgnoreAreas = new NativeParallelHashSet<Entity>(this.m_ApplyTempAreas.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ApplyTempAreas.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              Entity applyTempArea = this.m_ApplyTempAreas[index];
              // ISSUE: reference to a compiler-generated field
              Temp temp = this.m_TempData[applyTempArea];
              // ISSUE: reference to a compiler-generated field
              iterator.m_IgnoreAreas.Add(temp.m_Original);
              Owner componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((!this.m_OwnerData.TryGetComponent(applyTempArea, out componentData) || !this.m_Nodes.HasBuffer(componentData.m_Owner)) && (temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
              {
                Entity area = (temp.m_Flags & TempFlags.Create) != (TempFlags) 0 ? applyTempArea : temp.m_Original;
                // ISSUE: reference to a compiler-generated field
                if ((this.m_Snap & Snap.ExistingGeometry) != Snap.None || area == preferredArea)
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[applyTempArea];
                  for (int x = 0; x < node.Length; ++x)
                  {
                    int2 nodeIndex = new int2(x, math.select(x + 1, 0, x == node.Length - 1));
                    Line3.Segment line = new Line3.Segment(node[nodeIndex.x].m_Position, node[nodeIndex.y].m_Position);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    iterator.CheckLine(line, snapDistance1, area, nodeIndex, !this.m_EditorMode && this.m_LotData.HasComponent(applyTempArea));
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((((this.m_Snap & Snap.ExistingGeometry) != Snap.None ? 1 : (preferredArea != Entity.Null ? 1 : 0)) | (ignoreStartPositions ? 1 : 0)) != 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AreaSearchTree.Iterate<AreaToolSystem.SnapJob.AreaIterator>(ref iterator);
          }
          for (int index = 0; index < selfSnap; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Line3.Segment line = new Line3.Segment(this.m_ControlPoints[index].m_Position, this.m_ControlPoints[index + 1].m_Position);
            // ISSUE: reference to a compiler-generated method
            iterator.CheckLine(line, snapDistance1, Entity.Null, new int2(index, index + 1), false);
          }
          // ISSUE: reference to a compiler-generated field
          bestSnapPosition = iterator.m_BestSnapPosition;
          // ISSUE: reference to a compiler-generated field
          if (iterator.m_IgnoreAreas.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            iterator.m_IgnoreAreas.Dispose();
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & (Snap.NetSide | Snap.NetMiddle)) != Snap.None && (this.m_State != AreaToolSystem.State.Default || this.m_AllowCreateArea))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AreaToolSystem.SnapJob.NetIterator iterator = new AreaToolSystem.SnapJob.NetIterator()
          {
            m_Snap = this.m_Snap,
            m_Bounds = new Bounds3(controlPoint.m_HitPosition - snapDistance, controlPoint.m_HitPosition + snapDistance),
            m_MaxDistance = snapDistance,
            m_ControlPoint = controlPoint,
            m_BestSnapPosition = bestSnapPosition,
            m_SnapLines = snapLines,
            m_CurveData = this.m_CurveData,
            m_EdgeGeometryData = this.m_EdgeGeometryData,
            m_StartGeometryData = this.m_StartGeometryData,
            m_EndGeometryData = this.m_EndGeometryData,
            m_CompositionData = this.m_CompositionData,
            m_PrefabCompositionData = this.m_PrefabCompositionData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<AreaToolSystem.SnapJob.NetIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          bestSnapPosition = iterator.m_BestSnapPosition;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & (Snap.ObjectSide | Snap.LotGrid)) != Snap.None && (this.m_State != AreaToolSystem.State.Default || this.m_AllowCreateArea))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AreaToolSystem.SnapJob.ObjectIterator iterator = new AreaToolSystem.SnapJob.ObjectIterator()
          {
            m_Bounds = new Bounds3(controlPoint.m_HitPosition - snapDistance, controlPoint.m_HitPosition + snapDistance),
            m_MaxDistance = snapDistance,
            m_Snap = this.m_Snap,
            m_ControlPoint = controlPoint,
            m_BestSnapPosition = bestSnapPosition,
            m_SnapLines = snapLines,
            m_TransformData = this.m_TransformData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_BuildingData = this.m_PrefabBuildingData,
            m_BuildingExtensionData = this.m_BuildingExtensionData,
            m_AssetStampData = this.m_AssetStampData,
            m_ObjectGeometryData = this.m_ObjectGeometryData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectSearchTree.Iterate<AreaToolSystem.SnapJob.ObjectIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          bestSnapPosition = iterator.m_BestSnapPosition;
        }
        snapLines.Dispose();
        // ISSUE: reference to a compiler-generated field
        return this.m_Nodes.HasBuffer(bestSnapPosition.m_OriginalEntity);
      }

      private struct ParentObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Line3.Segment m_Line;
        public float m_BoundsOffset;
        public float m_MaxDistance;
        public Entity m_Parent;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<BuildingData> m_BuildingData;
        public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds, (float3) this.m_BoundsOffset), this.m_Line, out float2 _);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          float2 t;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(MathUtils.Expand(bounds.m_Bounds, (float3) this.m_BoundsOffset), this.m_Line, out t) || !this.m_TransformData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[entity];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ObjectGeometryData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            float2 lotSize = (float2) this.m_BuildingData[prefabRef.m_Prefab].m_LotSize;
            // ISSUE: reference to a compiler-generated field
            objectGeometryData.m_Bounds.min.xz = lotSize * -4f - this.m_MaxDistance;
            // ISSUE: reference to a compiler-generated field
            objectGeometryData.m_Bounds.max.xz = lotSize * 4f + this.m_MaxDistance;
          }
          if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
          {
            float num = math.max(math.cmax(objectGeometryData.m_Bounds.max.xz), -math.cmin(objectGeometryData.m_Bounds.max.xz));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) MathUtils.Distance(this.m_Line.xz, transform.m_Position.xz, out float _) >= (double) num + (double) this.m_MaxDistance)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_Parent = entity;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, objectGeometryData.m_Bounds).xz, this.m_Line.xz, out t))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_Parent = entity;
          }
        }
      }

      private struct AreaIterator2 : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public bool m_EditorMode;
        public Game.Areas.AreaType m_AreaType;
        public Bounds3 m_Bounds;
        public float m_MaxDistance1;
        public float m_MaxDistance2;
        public ControlPoint m_ControlPoint1;
        public ControlPoint m_ControlPoint2;
        public NativeParallelHashSet<Entity> m_IgnoreAreas;
        public NativeList<ControlPoint> m_ControlPoints;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<AreaGeometryData> m_PrefabAreaData;
        public ComponentLookup<Game.Areas.Lot> m_LotData;
        public ComponentLookup<Owner> m_OwnerData;
        public BufferLookup<Game.Areas.Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          Owner componentData;
          DynamicBuffer<InstalledUpgrade> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || this.m_IgnoreAreas.IsCreated && this.m_IgnoreAreas.Contains(areaItem.m_Area) || this.m_OwnerData.TryGetComponent(areaItem.m_Area, out componentData) && (this.m_Nodes.HasBuffer(componentData.m_Owner) || this.m_EditorMode && this.m_InstalledUpgrades.TryGetBuffer(componentData.m_Owner, out bufferData) && bufferData.Length != 0))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AreaGeometryData areaGeometryData = this.m_PrefabAreaData[this.m_PrefabRefData[areaItem.m_Area].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (areaGeometryData.m_Type != this.m_AreaType)
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[areaItem.m_Area];
          // ISSUE: reference to a compiler-generated field
          Triangle triangle = this.m_Triangles[areaItem.m_Area][areaItem.m_Triangle];
          Triangle3 triangle3 = AreaUtils.GetTriangle3(node, triangle);
          int3 int3 = math.abs(triangle.m_Indices - triangle.m_Indices.yzx);
          bool3 x = int3 == 1 | int3 == node.Length - 1;
          if (!math.any(x))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool lockFirstEdge = !this.m_EditorMode && this.m_LotData.HasComponent(areaItem.m_Area);
          if (x.x)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(triangle3.ab, areaGeometryData.m_SnapDistance, areaItem.m_Area, triangle.m_Indices.xy, lockFirstEdge);
          }
          if (x.y)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(triangle3.bc, areaGeometryData.m_SnapDistance, areaItem.m_Area, triangle.m_Indices.yz, lockFirstEdge);
          }
          if (!x.z)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(triangle3.ca, areaGeometryData.m_SnapDistance, areaItem.m_Area, triangle.m_Indices.zx, lockFirstEdge);
        }

        public void CheckLine(
          Line3.Segment line,
          float snapDistance,
          Entity area,
          int2 nodeIndex,
          bool lockFirstEdge)
        {
          if (lockFirstEdge && math.cmin(nodeIndex) == 0 && math.cmax(nodeIndex) == 1)
            return;
          // ISSUE: reference to a compiler-generated field
          double num1 = (double) MathUtils.Distance(line.xz, this.m_ControlPoint1.m_Position.xz, out float _);
          // ISSUE: reference to a compiler-generated field
          float num2 = MathUtils.Distance(line.xz, this.m_ControlPoint2.m_HitPosition.xz, out float _);
          // ISSUE: reference to a compiler-generated field
          double maxDistance1 = (double) this.m_MaxDistance1;
          // ISSUE: reference to a compiler-generated field
          if (num1 >= maxDistance1 || (double) num2 >= (double) this.m_MaxDistance2)
            return;
          // ISSUE: reference to a compiler-generated field
          float num3 = math.distance(line.a.xz, this.m_ControlPoint2.m_HitPosition.xz);
          // ISSUE: reference to a compiler-generated field
          float num4 = math.distance(line.b.xz, this.m_ControlPoint2.m_HitPosition.xz);
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoint1 with
          {
            m_OriginalEntity = area,
            m_ElementIndex = !((double) num3 <= (double) snapDistance & (double) num3 <= (double) num4) || lockFirstEdge && nodeIndex.x < 2 ? ((double) num4 > (double) snapDistance || lockFirstEdge && nodeIndex.y < 2 ? new int2(-1, math.select(math.cmax(nodeIndex), math.cmin(nodeIndex), math.abs(nodeIndex.y - nodeIndex.x) == 1)) : new int2(nodeIndex.y, -1)) : new int2(nodeIndex.x, -1)
          };
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ControlPoints.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints[index].m_OriginalEntity == area)
              return;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint1);
        }
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public bool m_EditorMode;
        public bool m_IgnoreStartPositions;
        public Snap m_Snap;
        public Game.Areas.AreaType m_AreaType;
        public Bounds3 m_Bounds;
        public float m_MaxDistance;
        public NativeParallelHashSet<Entity> m_IgnoreAreas;
        public Entity m_PreferArea;
        public ControlPoint m_ControlPoint;
        public ControlPoint m_BestSnapPosition;
        public NativeList<SnapLine> m_SnapLines;
        public NativeList<ControlPoint> m_MoveStartPositions;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<AreaGeometryData> m_PrefabAreaData;
        public ComponentLookup<Game.Areas.Lot> m_LotData;
        public ComponentLookup<Owner> m_OwnerData;
        public BufferLookup<Game.Areas.Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || this.m_IgnoreAreas.IsCreated && this.m_IgnoreAreas.Contains(areaItem.m_Area))
            return;
          Entity area = areaItem.m_Area;
          // ISSUE: reference to a compiler-generated field
          if (areaItem.m_Area != this.m_PreferArea)
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_Snap & Snap.ExistingGeometry) == Snap.None)
            {
              bool flag = false;
              // ISSUE: reference to a compiler-generated field
              if (this.m_IgnoreStartPositions)
              {
                // ISSUE: reference to a compiler-generated field
                for (int index = 0; index < this.m_MoveStartPositions.Length; ++index)
                {
                  // ISSUE: reference to a compiler-generated field
                  ControlPoint moveStartPosition = this.m_MoveStartPositions[index];
                  flag |= moveStartPosition.m_OriginalEntity == areaItem.m_Area;
                }
              }
              if (!flag)
                return;
            }
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.TryGetComponent(areaItem.m_Area, out componentData))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_Nodes.HasBuffer(componentData.m_Owner))
                return;
              DynamicBuffer<InstalledUpgrade> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorMode && this.m_InstalledUpgrades.TryGetBuffer(componentData.m_Owner, out bufferData) && bufferData.Length != 0)
                area = Entity.Null;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AreaGeometryData areaGeometryData = this.m_PrefabAreaData[this.m_PrefabRefData[areaItem.m_Area].m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (areaGeometryData.m_Type != this.m_AreaType)
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[areaItem.m_Area];
          // ISSUE: reference to a compiler-generated field
          Triangle triangle = this.m_Triangles[areaItem.m_Area][areaItem.m_Triangle];
          Triangle3 triangle3 = AreaUtils.GetTriangle3(node, triangle);
          int3 int3_1 = math.abs(triangle.m_Indices - triangle.m_Indices.yzx);
          bool3 bool3 = int3_1 == node.Length - 1;
          bool3 x = int3_1 == 1 | bool3;
          if (!math.any(x))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_IgnoreStartPositions)
          {
            bool3 c = triangle.m_Indices.yzx < triangle.m_Indices != bool3;
            int3 int3_2 = math.select(triangle.m_Indices, triangle.m_Indices.yzx, c);
            int3 int3_3 = math.select(triangle.m_Indices.yzx, triangle.m_Indices, c);
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_MoveStartPositions.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint moveStartPosition = this.m_MoveStartPositions[index];
              if (!(moveStartPosition.m_OriginalEntity != areaItem.m_Area))
                x = x & moveStartPosition.m_ElementIndex.x != int3_2 & moveStartPosition.m_ElementIndex.x != int3_3 & moveStartPosition.m_ElementIndex.y != int3_2;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool lockFirstEdge = !this.m_EditorMode && this.m_LotData.HasComponent(areaItem.m_Area);
          // ISSUE: reference to a compiler-generated field
          float snapDistance = math.select(areaGeometryData.m_SnapDistance, areaGeometryData.m_SnapDistance * 0.5f, (this.m_Snap & Snap.ExistingGeometry) == Snap.None);
          if (x.x)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(triangle3.ab, snapDistance, area, triangle.m_Indices.xy, lockFirstEdge);
          }
          if (x.y)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckLine(triangle3.bc, snapDistance, area, triangle.m_Indices.yz, lockFirstEdge);
          }
          if (!x.z)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(triangle3.ca, snapDistance, area, triangle.m_Indices.zx, lockFirstEdge);
        }

        public void CheckLine(
          Line3.Segment line,
          float snapDistance,
          Entity area,
          int2 nodeIndex,
          bool lockFirstEdge)
        {
          float t;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (lockFirstEdge && math.cmin(nodeIndex) == 0 && math.cmax(nodeIndex) == 1 || (double) MathUtils.Distance(line.xz, this.m_ControlPoint.m_HitPosition.xz, out t) >= (double) this.m_MaxDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          float level = math.select(2f, 3f, area == this.m_PreferArea);
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(line.a.xz, this.m_ControlPoint.m_HitPosition.xz);
          // ISSUE: reference to a compiler-generated field
          float num2 = math.distance(line.b.xz, this.m_ControlPoint.m_HitPosition.xz);
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_OriginalEntity = area,
            m_Direction = line.b.xz - line.a.xz
          };
          MathUtils.TryNormalize(ref controlPoint.m_Direction);
          if ((double) num1 <= (double) snapDistance & (double) num1 <= (double) num2 && (!lockFirstEdge || nodeIndex.x >= 2))
          {
            controlPoint.m_Position = line.a;
            controlPoint.m_ElementIndex = new int2(nodeIndex.x, -1);
            // ISSUE: reference to a compiler-generated field
            controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line.a, line.b), (SnapLineFlags) 0));
          }
          else if ((double) num2 <= (double) snapDistance && (!lockFirstEdge || nodeIndex.y >= 2))
          {
            controlPoint.m_Position = line.b;
            controlPoint.m_ElementIndex = new int2(nodeIndex.y, -1);
            // ISSUE: reference to a compiler-generated field
            controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line.a, line.b), (SnapLineFlags) 0));
          }
          else
          {
            controlPoint.m_Position = MathUtils.Position(line, t);
            controlPoint.m_ElementIndex = new int2(-1, math.select(math.cmax(nodeIndex), math.cmin(nodeIndex), math.abs(nodeIndex.y - nodeIndex.x) == 1));
            // ISSUE: reference to a compiler-generated field
            controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line.a, line.b), (SnapLineFlags) 0));
          }
        }
      }

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Snap m_Snap;
        public Bounds3 m_Bounds;
        public float m_MaxDistance;
        public ControlPoint m_ControlPoint;
        public ControlPoint m_BestSnapPosition;
        public NativeList<SnapLine> m_SnapLines;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndGeometryData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<NetCompositionData> m_PrefabCompositionData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
            return;
          Composition composition = new Composition();
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            composition = this.m_CompositionData[entity];
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.NetSide) != Snap.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EdgeGeometryData.HasComponent(entity) && this.CheckComposition(composition.m_Edge))
            {
              // ISSUE: reference to a compiler-generated field
              EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[entity];
              // ISSUE: reference to a compiler-generated method
              this.SnapEdgeCurve(edgeGeometry.m_Start.m_Left);
              // ISSUE: reference to a compiler-generated method
              this.SnapEdgeCurve(edgeGeometry.m_Start.m_Right);
              // ISSUE: reference to a compiler-generated method
              this.SnapEdgeCurve(edgeGeometry.m_End.m_Left);
              // ISSUE: reference to a compiler-generated method
              this.SnapEdgeCurve(edgeGeometry.m_End.m_Right);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_StartGeometryData.HasComponent(entity) && this.CheckComposition(composition.m_StartNode))
            {
              // ISSUE: reference to a compiler-generated field
              StartNodeGeometry startNodeGeometry = this.m_StartGeometryData[entity];
              if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
              {
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(startNodeGeometry.m_Geometry.m_Left.m_Left);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(startNodeGeometry.m_Geometry.m_Left.m_Right);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(startNodeGeometry.m_Geometry.m_Right.m_Left);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(startNodeGeometry.m_Geometry.m_Right.m_Right);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(startNodeGeometry.m_Geometry.m_Left.m_Left);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(startNodeGeometry.m_Geometry.m_Right.m_Right);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EndGeometryData.HasComponent(entity) && this.CheckComposition(composition.m_EndNode))
            {
              // ISSUE: reference to a compiler-generated field
              EndNodeGeometry endNodeGeometry = this.m_EndGeometryData[entity];
              if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
              {
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(endNodeGeometry.m_Geometry.m_Left.m_Left);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(endNodeGeometry.m_Geometry.m_Left.m_Right);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(endNodeGeometry.m_Geometry.m_Right.m_Left);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(endNodeGeometry.m_Geometry.m_Right.m_Right);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(endNodeGeometry.m_Geometry.m_Left.m_Left);
                // ISSUE: reference to a compiler-generated method
                this.SnapNodeCurve(endNodeGeometry.m_Geometry.m_Right.m_Right);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if ((this.m_Snap & Snap.NetMiddle) == Snap.None || !this.m_CurveData.HasComponent(entity) || !this.CheckComposition(composition.m_Edge))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.SnapEdgeCurve(this.m_CurveData[entity].m_Bezier);
        }

        private bool CheckComposition(Entity composition)
        {
          NetCompositionData componentData;
          // ISSUE: reference to a compiler-generated field
          return !this.m_PrefabCompositionData.TryGetComponent(composition, out componentData) || (componentData.m_Flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0;
        }

        private void SnapEdgeCurve(Bezier4x3 curve)
        {
          float t;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds, MathUtils.Bounds(curve)) || (double) MathUtils.Distance(curve.xz, this.m_ControlPoint.m_HitPosition.xz, out t) >= (double) this.m_MaxDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_OriginalEntity = Entity.Null,
            m_Position = MathUtils.Position(curve, t),
            m_Direction = MathUtils.Tangent(curve, t).xz
          };
          MathUtils.TryNormalize(ref controlPoint.m_Direction);
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(1f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, curve, (SnapLineFlags) 0));
        }

        private void SnapNodeCurve(Bezier4x3 curve)
        {
          float3 float3 = MathUtils.StartTangent(curve);
          float3 y = MathUtils.Normalize(float3, float3.xz);
          y.y = math.clamp(y.y, -1f, 1f);
          Line3.Segment line = new Line3.Segment(curve.a, curve.a + y * math.dot(curve.d - curve.a, y));
          float t;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds, MathUtils.Bounds(line)) || (double) MathUtils.Distance(line.xz, this.m_ControlPoint.m_HitPosition.xz, out t) >= (double) this.m_MaxDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_OriginalEntity = Entity.Null,
            m_Direction = y.xz,
            m_Position = MathUtils.Position(line, t)
          };
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(1f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line.a, line.b), (SnapLineFlags) 0));
        }
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float m_MaxDistance;
        public Snap m_Snap;
        public ControlPoint m_ControlPoint;
        public ControlPoint m_BestSnapPosition;
        public NativeList<SnapLine> m_SnapLines;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<BuildingData> m_BuildingData;
        public ComponentLookup<BuildingExtensionData> m_BuildingExtensionData;
        public ComponentLookup<AssetStampData> m_AssetStampData;
        public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_TransformData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[entity];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ObjectGeometryData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.LotGrid) != Snap.None && (this.m_BuildingData.HasComponent(prefabRef.m_Prefab) || this.m_BuildingExtensionData.HasComponent(prefabRef.m_Prefab) || this.m_AssetStampData.HasComponent(prefabRef.m_Prefab)))
          {
            float2 float2_1 = math.normalizesafe(math.forward(transform.m_Rotation).xz, new float2(0.0f, 1f));
            float2 y = MathUtils.Right(float2_1);
            // ISSUE: reference to a compiler-generated field
            float2 x1 = this.m_ControlPoint.m_HitPosition.xz - transform.m_Position.xz;
            int2 int2;
            int2.x = ZoneUtils.GetCellWidth(objectGeometryData.m_Size.x);
            int2.y = ZoneUtils.GetCellWidth(objectGeometryData.m_Size.z);
            float2 float2_2 = (float2) int2 * 8f;
            float2 offset = math.select((float2) 0.0f, (float2) 4f, (int2 & 1) != 0);
            float2 a = new float2(math.dot(x1, y), math.dot(x1, float2_1));
            float2 b = MathUtils.Snap(a, (float2) 8f, offset);
            // ISSUE: reference to a compiler-generated field
            bool2 bool2 = math.abs(a - b) < this.m_MaxDistance;
            if (!math.any(bool2))
              return;
            float2 x2 = math.select(a, b, bool2);
            float2 x3 = transform.m_Position.xz + y * x2.x + float2_1 * x2.y;
            if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
            {
              if ((double) math.distance(x3, transform.m_Position.xz) > (double) float2_2.x * 0.5 + 4.0)
                return;
            }
            else if (math.any(math.abs(x2) > float2_2 * 0.5f + 4f))
              return;
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint = this.m_ControlPoint with
            {
              m_OriginalEntity = Entity.Null,
              m_Direction = y
            };
            controlPoint.m_Position.xz = x3;
            // ISSUE: reference to a compiler-generated field
            controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
            Line3 line3_1 = new Line3(controlPoint.m_Position, controlPoint.m_Position);
            Line3 line3_2 = new Line3(controlPoint.m_Position, controlPoint.m_Position);
            line3_1.a.xz -= controlPoint.m_Direction * 8f;
            line3_1.b.xz += controlPoint.m_Direction * 8f;
            line3_2.a.xz -= MathUtils.Right(controlPoint.m_Direction) * 8f;
            line3_2.b.xz += MathUtils.Right(controlPoint.m_Direction) * 8f;
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
            if (bool2.y)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line3_1.a, line3_1.b), SnapLineFlags.Hidden));
            }
            controlPoint.m_Direction = MathUtils.Right(controlPoint.m_Direction);
            if (!bool2.x)
              return;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line3_2.a, line3_2.b), SnapLineFlags.Hidden));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_Snap & Snap.ObjectSide) == Snap.None || (objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
              return;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              float2 lotSize = (float2) this.m_BuildingData[prefabRef.m_Prefab].m_LotSize;
              objectGeometryData.m_Bounds.min.xz = lotSize * -4f;
              objectGeometryData.m_Bounds.max.xz = lotSize * 4f;
            }
            Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, objectGeometryData.m_Bounds);
            // ISSUE: reference to a compiler-generated method
            this.CheckLine((Line3) baseCorners.ab);
            // ISSUE: reference to a compiler-generated method
            this.CheckLine((Line3) baseCorners.bc);
            // ISSUE: reference to a compiler-generated method
            this.CheckLine((Line3) baseCorners.cd);
            // ISSUE: reference to a compiler-generated method
            this.CheckLine((Line3) baseCorners.da);
          }
        }

        private void CheckLine(Line3 line)
        {
          float t;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.Distance(line.xz, this.m_ControlPoint.m_HitPosition.xz, out t) >= (double) this.m_MaxDistance)
            return;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_OriginalEntity = Entity.Null,
            m_Direction = math.normalizesafe(MathUtils.Tangent(line.xz)),
            m_Position = MathUtils.Position(line, t)
          };
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(1f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line.a, line.b), (SnapLineFlags) 0));
        }
      }
    }

    [BurstCompile]
    private struct RemoveMapTilesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<LocalNodeCache> m_CacheType;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length == 1 && this.m_ControlPoints[0].Equals(new ControlPoint()))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LocalNodeCache> bufferAccessor2 = chunk.GetBufferAccessor<LocalNodeCache>(ref this.m_CacheType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Entity entity1 = nativeArray[index];
          DynamicBuffer<Game.Areas.Node> dynamicBuffer1 = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
          CreationDefinition component = new CreationDefinition();
          component.m_Original = entity1;
          component.m_Flags |= CreationFlags.Delete;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(unfilteredChunkIndex, entity2, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, entity2, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(unfilteredChunkIndex, entity2).CopyFrom(dynamicBuffer1.AsNativeArray());
          if (bufferAccessor2.Length != 0)
          {
            DynamicBuffer<LocalNodeCache> dynamicBuffer2 = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<LocalNodeCache>(unfilteredChunkIndex, entity2).CopyFrom(dynamicBuffer2.AsNativeArray());
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public bool m_AllowCreateArea;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public AreaToolSystem.Mode m_Mode;
      [ReadOnly]
      public AreaToolSystem.State m_State;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public Entity m_Recreate;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<Entity> m_ApplyTempAreas;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<Entity> m_ApplyTempBuildings;
      [ReadOnly]
      public NativeList<ControlPoint> m_MoveStartPositions;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Clear> m_ClearData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Space> m_SpaceData;
      [ReadOnly]
      public ComponentLookup<Area> m_AreaData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_CachedNodes;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      public NativeValue<AreaToolSystem.Tooltip> m_Tooltip;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length == 1 && this.m_ControlPoints[0].Equals(new ControlPoint()))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.Mode mode = this.m_Mode;
        switch (mode)
        {
          case AreaToolSystem.Mode.Edit:
            // ISSUE: reference to a compiler-generated method
            this.Edit();
            break;
          case AreaToolSystem.Mode.Generate:
            // ISSUE: reference to a compiler-generated method
            this.Generate();
            break;
        }
      }

      private void Generate()
      {
        int2 int2;
        for (int2.y = 0; int2.y < 23; ++int2.y)
        {
          for (int2.x = 0; int2.x < 23; ++int2.x)
          {
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity();
            CreationDefinition component = new CreationDefinition();
            // ISSUE: reference to a compiler-generated field
            component.m_Prefab = this.m_Prefab;
            float2 float2 = new float2(23f, 23f) * 311.652161f;
            Bounds2 bounds2;
            bounds2.min = (float2) int2 * 623.3043f - float2;
            bounds2.max = (float2) (int2 + 1) * 623.3043f - float2;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.Node> dynamicBuffer = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity);
            dynamicBuffer.ResizeUninitialized(5);
            dynamicBuffer[0] = new Game.Areas.Node(new float3(bounds2.min.x, 0.0f, bounds2.min.y), float.MinValue);
            dynamicBuffer[1] = new Game.Areas.Node(new float3(bounds2.min.x, 0.0f, bounds2.max.y), float.MinValue);
            dynamicBuffer[2] = new Game.Areas.Node(new float3(bounds2.max.x, 0.0f, bounds2.max.y), float.MinValue);
            dynamicBuffer[3] = new Game.Areas.Node(new float3(bounds2.max.x, 0.0f, bounds2.min.y), float.MinValue);
            dynamicBuffer[4] = dynamicBuffer[0];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Tooltip.value = AreaToolSystem.Tooltip.GenerateAreas;
      }

      private void GetControlPoints(
        int index,
        out ControlPoint firstPoint,
        out ControlPoint lastPoint)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.State state = this.m_State;
        switch (state)
        {
          case AreaToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated field
            firstPoint = this.m_ControlPoints[index];
            // ISSUE: reference to a compiler-generated field
            lastPoint = this.m_ControlPoints[index];
            break;
          case AreaToolSystem.State.Create:
            firstPoint = new ControlPoint();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            lastPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
            break;
          case AreaToolSystem.State.Modify:
            // ISSUE: reference to a compiler-generated field
            firstPoint = this.m_MoveStartPositions[index];
            // ISSUE: reference to a compiler-generated field
            lastPoint = this.m_ControlPoints[0];
            break;
          case AreaToolSystem.State.Remove:
            // ISSUE: reference to a compiler-generated field
            firstPoint = this.m_MoveStartPositions[index];
            // ISSUE: reference to a compiler-generated field
            lastPoint = this.m_ControlPoints[0];
            break;
          default:
            firstPoint = new ControlPoint();
            lastPoint = new ControlPoint();
            break;
        }
      }

      private void Edit()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AreaGeometryData areaData = this.m_PrefabAreaData[this.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.State state1 = this.m_State;
        int num1;
        switch (state1)
        {
          case AreaToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_ControlPoints.Length;
            break;
          case AreaToolSystem.State.Create:
            num1 = 1;
            break;
          case AreaToolSystem.State.Modify:
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_MoveStartPositions.Length;
            break;
          case AreaToolSystem.State.Remove:
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_MoveStartPositions.Length;
            break;
          default:
            num1 = 0;
            break;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Tooltip.value = AreaToolSystem.Tooltip.None;
        bool flag1 = false;
        NativeParallelHashSet<Entity> createdEntities = new NativeParallelHashSet<Entity>(num1 * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < num1; ++index)
        {
          ControlPoint firstPoint;
          // ISSUE: reference to a compiler-generated method
          this.GetControlPoints(index, out firstPoint, out ControlPoint _);
          // ISSUE: reference to a compiler-generated field
          if (this.m_Nodes.HasBuffer(firstPoint.m_OriginalEntity) && math.any(firstPoint.m_ElementIndex >= 0))
            createdEntities.Add(firstPoint.m_OriginalEntity);
        }
        NativeList<ClearAreaData> clearAreas = new NativeList<ClearAreaData>();
        for (int index1 = 0; index1 < num1; ++index1)
        {
          ControlPoint firstPoint;
          ControlPoint lastPoint;
          // ISSUE: reference to a compiler-generated method
          this.GetControlPoints(index1, out firstPoint, out lastPoint);
          // ISSUE: reference to a compiler-generated field
          if (index1 == 0 && this.m_State == AreaToolSystem.State.Modify)
            flag1 = !firstPoint.Equals(lastPoint);
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component = new CreationDefinition();
          // ISSUE: reference to a compiler-generated field
          component.m_Prefab = this.m_Prefab;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Nodes.HasBuffer(firstPoint.m_OriginalEntity) && math.any(firstPoint.m_ElementIndex >= 0))
          {
            component.m_Original = firstPoint.m_OriginalEntity;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Recreate != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              component.m_Original = this.m_Recreate;
            }
          }
          float minNodeDistance = AreaUtils.GetMinNodeDistance(areaData);
          int2 int2 = new int2();
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> nodes = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity1);
          DynamicBuffer<LocalNodeCache> dynamicBuffer1 = new DynamicBuffer<LocalNodeCache>();
          bool isComplete = false;
          LocalNodeCache localNodeCache1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Nodes.HasBuffer(firstPoint.m_OriginalEntity) && math.any(firstPoint.m_ElementIndex >= 0))
          {
            component.m_Flags |= CreationFlags.Relocate;
            isComplete = true;
            // ISSUE: reference to a compiler-generated method
            Entity sourceArea = this.GetSourceArea(firstPoint.m_OriginalEntity);
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[sourceArea];
            DynamicBuffer<LocalNodeCache> dynamicBuffer2 = new DynamicBuffer<LocalNodeCache>();
            // ISSUE: reference to a compiler-generated field
            if (this.m_CachedNodes.HasBuffer(sourceArea))
            {
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer2 = this.m_CachedNodes[sourceArea];
            }
            float elevation = float.MinValue;
            int num2 = -1;
            if (lastPoint.m_ElementIndex.x >= 0)
            {
              num2 = lastPoint.m_ElementIndex.x;
              Owner componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnerData.TryGetComponent(firstPoint.m_OriginalEntity, out componentData1))
              {
                Entity owner;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                for (owner = componentData1.m_Owner; this.m_OwnerData.HasComponent(owner) && !this.m_BuildingData.HasComponent(owner); owner = this.m_OwnerData[owner].m_Owner)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LocalTransformCacheData.HasComponent(owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    num2 = this.m_LocalTransformCacheData[owner].m_ParentMesh;
                  }
                }
                Game.Objects.Transform componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.TryGetComponent(owner, out componentData2))
                  elevation = lastPoint.m_Position.y - componentData2.m_Position.y;
              }
              if (num2 != -1)
              {
                if ((double) elevation == -3.4028234663852886E+38)
                  elevation = 0.0f;
              }
              else
                elevation = float.MinValue;
            }
            if (firstPoint.m_ElementIndex.y >= 0)
            {
              int y = firstPoint.m_ElementIndex.y;
              int index2 = math.select(firstPoint.m_ElementIndex.y + 1, 0, firstPoint.m_ElementIndex.y == node.Length - 1);
              float2 float2 = new float2(math.distance(lastPoint.m_Position, node[y].m_Position), math.distance(lastPoint.m_Position, node[index2].m_Position));
              bool flag2 = flag1 && math.any(float2 < minNodeDistance);
              int num3 = math.select(1, 0, flag2 || !flag1);
              int length = node.Length + num3;
              nodes.ResizeUninitialized(length);
              int index3 = 0;
              if (dynamicBuffer2.IsCreated)
              {
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity1);
                dynamicBuffer1.ResizeUninitialized(length);
                for (int index4 = 0; index4 <= firstPoint.m_ElementIndex.y; ++index4)
                {
                  nodes[index3] = node[index4];
                  dynamicBuffer1[index3] = dynamicBuffer2[index4];
                  ++index3;
                }
                int2.x = index3;
                for (int index5 = 0; index5 < num3; ++index5)
                {
                  nodes[index3] = new Game.Areas.Node(lastPoint.m_Position, elevation);
                  ref DynamicBuffer<LocalNodeCache> local = ref dynamicBuffer1;
                  int index6 = index3;
                  localNodeCache1 = new LocalNodeCache();
                  localNodeCache1.m_Position = lastPoint.m_Position;
                  localNodeCache1.m_ParentMesh = num2;
                  LocalNodeCache localNodeCache2 = localNodeCache1;
                  local[index6] = localNodeCache2;
                  ++index3;
                }
                int2.y = index3;
                for (int index7 = firstPoint.m_ElementIndex.y + 1; index7 < node.Length; ++index7)
                {
                  nodes[index3] = node[index7];
                  dynamicBuffer1[index3] = dynamicBuffer2[index7];
                  ++index3;
                }
              }
              else
              {
                for (int index8 = 0; index8 <= firstPoint.m_ElementIndex.y; ++index8)
                  nodes[index3++] = node[index8];
                for (int index9 = 0; index9 < num3; ++index9)
                  nodes[index3++] = new Game.Areas.Node(lastPoint.m_Position, elevation);
                for (int index10 = firstPoint.m_ElementIndex.y + 1; index10 < node.Length; ++index10)
                  nodes[index3++] = node[index10];
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              AreaToolSystem.State state2 = this.m_State;
              switch (state2)
              {
                case AreaToolSystem.State.Default:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_Tooltip.value = !this.m_AllowCreateArea ? AreaToolSystem.Tooltip.ModifyEdge : AreaToolSystem.Tooltip.CreateAreaOrModifyEdge;
                  break;
                case AreaToolSystem.State.Modify:
                  if (!flag2 & flag1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Tooltip.value = AreaToolSystem.Tooltip.InsertNode;
                    break;
                  }
                  break;
              }
            }
            else
            {
              bool flag3 = false;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_OwnerData.HasComponent(component.m_Original) || node.Length >= 4)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_State == AreaToolSystem.State.Remove)
                {
                  flag3 = true;
                }
                else
                {
                  int index11 = math.select(firstPoint.m_ElementIndex.x - 1, node.Length - 1, firstPoint.m_ElementIndex.x == 0);
                  int index12 = math.select(firstPoint.m_ElementIndex.x + 1, 0, firstPoint.m_ElementIndex.x == node.Length - 1);
                  float2 float2 = new float2(math.distance(lastPoint.m_Position, node[index11].m_Position), math.distance(lastPoint.m_Position, node[index12].m_Position));
                  flag3 = flag1 && math.any(float2 < minNodeDistance);
                }
              }
              int num4 = math.select(0, 1, flag1 | flag3);
              int num5 = math.select(1, 0, flag3 || !flag1);
              int length = node.Length + num5 - num4;
              nodes.ResizeUninitialized(length);
              int index13 = 0;
              if (dynamicBuffer2.IsCreated)
              {
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity1);
                dynamicBuffer1.ResizeUninitialized(length);
                for (int index14 = 0; index14 <= firstPoint.m_ElementIndex.x - num4; ++index14)
                {
                  nodes[index13] = node[index14];
                  dynamicBuffer1[index13] = dynamicBuffer2[index14];
                  ++index13;
                }
                int2.x = index13;
                for (int index15 = 0; index15 < num5; ++index15)
                {
                  nodes[index13] = new Game.Areas.Node(lastPoint.m_Position, elevation);
                  ref DynamicBuffer<LocalNodeCache> local = ref dynamicBuffer1;
                  int index16 = index13;
                  localNodeCache1 = new LocalNodeCache();
                  localNodeCache1.m_Position = lastPoint.m_Position;
                  localNodeCache1.m_ParentMesh = num2;
                  LocalNodeCache localNodeCache3 = localNodeCache1;
                  local[index16] = localNodeCache3;
                  ++index13;
                }
                int2.y = index13;
                for (int index17 = firstPoint.m_ElementIndex.x + 1; index17 < node.Length; ++index17)
                {
                  nodes[index13] = node[index17];
                  dynamicBuffer1[index13] = dynamicBuffer2[index17];
                  ++index13;
                }
              }
              else
              {
                for (int index18 = 0; index18 <= firstPoint.m_ElementIndex.x - num4; ++index18)
                  nodes[index13++] = node[index18];
                for (int index19 = 0; index19 < num5; ++index19)
                  nodes[index13++] = new Game.Areas.Node(lastPoint.m_Position, elevation);
                for (int index20 = firstPoint.m_ElementIndex.x + 1; index20 < node.Length; ++index20)
                  nodes[index13++] = node[index20];
              }
              if (length < 3)
                component.m_Flags |= CreationFlags.Delete;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              AreaToolSystem.State state3 = this.m_State;
              switch (state3)
              {
                case AreaToolSystem.State.Default:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_Tooltip.value = !this.m_AllowCreateArea ? AreaToolSystem.Tooltip.ModifyNode : AreaToolSystem.Tooltip.CreateAreaOrModifyNode;
                  break;
                case AreaToolSystem.State.Modify:
                  if (length < 3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Tooltip.value = AreaToolSystem.Tooltip.DeleteArea;
                    break;
                  }
                  if (flag3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Tooltip.value = AreaToolSystem.Tooltip.MergeNodes;
                    break;
                  }
                  if (flag1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Tooltip.value = AreaToolSystem.Tooltip.MoveNode;
                    break;
                  }
                  break;
                case AreaToolSystem.State.Remove:
                  if (length < 3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Tooltip.value = AreaToolSystem.Tooltip.DeleteArea;
                    break;
                  }
                  if (flag3)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Tooltip.value = AreaToolSystem.Tooltip.RemoveNode;
                    break;
                  }
                  break;
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Recreate != Entity.Null)
              component.m_Flags |= CreationFlags.Recreate;
            bool c = false;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              c = (double) math.distance(this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_Position, this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_Position) < (double) minNodeDistance;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int num6 = math.select(this.m_ControlPoints.Length, this.m_ControlPoints.Length - 1, c);
            nodes.ResizeUninitialized(num6);
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
            {
              // ISSUE: reference to a compiler-generated field
              dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity1);
              dynamicBuffer1.ResizeUninitialized(num6);
              int2 = new int2(0, num6);
              float num7 = float.MinValue;
              int b = lastPoint.m_ElementIndex.x;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(lastPoint.m_OriginalEntity))
              {
                Entity entity2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                for (entity2 = lastPoint.m_OriginalEntity; this.m_OwnerData.HasComponent(entity2) && !this.m_BuildingData.HasComponent(entity2); entity2 = this.m_OwnerData[entity2].m_Owner)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_LocalTransformCacheData.HasComponent(entity2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    b = this.m_LocalTransformCacheData[entity2].m_ParentMesh;
                  }
                }
                Game.Objects.Transform componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.TryGetComponent(entity2, out componentData))
                  num7 = componentData.m_Position.y;
              }
              for (int index21 = 0; index21 < num6; ++index21)
              {
                int num8 = -1;
                float num9 = float.MinValue;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ControlPoints[index21].m_ElementIndex.x >= 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  num8 = math.select(this.m_ControlPoints[index21].m_ElementIndex.x, b, b != -1);
                  // ISSUE: reference to a compiler-generated field
                  num9 = math.select(num9, this.m_ControlPoints[index21].m_Position.y - num7, (double) num7 != -3.4028234663852886E+38);
                }
                if (num8 != -1)
                {
                  if ((double) num9 == -3.4028234663852886E+38)
                    num9 = 0.0f;
                }
                else
                  num9 = float.MinValue;
                // ISSUE: reference to a compiler-generated field
                nodes[index21] = new Game.Areas.Node(this.m_ControlPoints[index21].m_Position, num9);
                ref DynamicBuffer<LocalNodeCache> local = ref dynamicBuffer1;
                int index22 = index21;
                localNodeCache1 = new LocalNodeCache();
                // ISSUE: reference to a compiler-generated field
                localNodeCache1.m_Position = this.m_ControlPoints[index21].m_Position;
                localNodeCache1.m_ParentMesh = num8;
                LocalNodeCache localNodeCache4 = localNodeCache1;
                local[index22] = localNodeCache4;
              }
            }
            else
            {
              for (int index23 = 0; index23 < num6; ++index23)
              {
                // ISSUE: reference to a compiler-generated field
                nodes[index23] = new Game.Areas.Node(this.m_ControlPoints[index23].m_Position, float.MinValue);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            AreaToolSystem.State state4 = this.m_State;
            switch (state4)
            {
              case AreaToolSystem.State.Default:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ControlPoints.Length == 1 && this.m_AllowCreateArea)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Tooltip.value = AreaToolSystem.Tooltip.CreateArea;
                  break;
                }
                break;
              case AreaToolSystem.State.Create:
                if (!c)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_Tooltip.value = this.m_ControlPoints.Length < 4 || !this.m_ControlPoints[0].m_Position.Equals(this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_Position) ? AreaToolSystem.Tooltip.AddNode : AreaToolSystem.Tooltip.CompleteArea;
                  break;
                }
                break;
            }
          }
          bool flag4 = false;
          Game.Objects.Transform inverseParentTransform = new Game.Objects.Transform();
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(lastPoint.m_OriginalEntity))
          {
            if ((areaData.m_Flags & Game.Areas.GeometryFlags.ClearArea) != (Game.Areas.GeometryFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ClearAreaHelpers.FillClearAreas(this.m_PrefabRefData[lastPoint.m_OriginalEntity].m_Prefab, this.m_TransformData[lastPoint.m_OriginalEntity], nodes, isComplete, this.m_PrefabObjectGeometryData, ref clearAreas);
            }
            // ISSUE: reference to a compiler-generated method
            OwnerDefinition ownerDefinition = this.GetOwnerDefinition(lastPoint.m_OriginalEntity, component.m_Original, createdEntities, true, (areaData.m_Flags & Game.Areas.GeometryFlags.ClearArea) != 0, clearAreas);
            if (ownerDefinition.m_Prefab != Entity.Null)
            {
              inverseParentTransform.m_Position = -ownerDefinition.m_Position;
              inverseParentTransform.m_Rotation = math.inverse(ownerDefinition.m_Rotation);
              flag4 = true;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, ownerDefinition);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.HasComponent(component.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              Entity owner = this.m_OwnerData[component.m_Original].m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(owner))
              {
                if ((areaData.m_Flags & Game.Areas.GeometryFlags.ClearArea) != (Game.Areas.GeometryFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ClearAreaHelpers.FillClearAreas(this.m_PrefabRefData[owner].m_Prefab, this.m_TransformData[owner], nodes, isComplete, this.m_PrefabObjectGeometryData, ref clearAreas);
                }
                // ISSUE: reference to a compiler-generated method
                OwnerDefinition ownerDefinition = this.GetOwnerDefinition(owner, component.m_Original, createdEntities, true, (areaData.m_Flags & Game.Areas.GeometryFlags.ClearArea) != 0, clearAreas);
                if (ownerDefinition.m_Prefab != Entity.Null)
                {
                  inverseParentTransform.m_Position = -ownerDefinition.m_Position;
                  inverseParentTransform.m_Rotation = math.inverse(ownerDefinition.m_Rotation);
                  flag4 = true;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, ownerDefinition);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Objects.Transform transform = this.m_TransformData[owner];
                  inverseParentTransform.m_Position = -transform.m_Position;
                  inverseParentTransform.m_Rotation = math.inverse(transform.m_Rotation);
                  flag4 = true;
                  component.m_Owner = owner;
                }
              }
              else
                component.m_Owner = owner;
            }
          }
          if (flag4)
          {
            for (int x = int2.x; x < int2.y; ++x)
            {
              LocalNodeCache localNodeCache5 = dynamicBuffer1[x];
              localNodeCache5.m_Position = ObjectUtils.WorldToLocal(inverseParentTransform, localNodeCache5.m_Position);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity1, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
          Area componentData3;
          DynamicBuffer<Game.Objects.SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AreaData.TryGetComponent(component.m_Original, out componentData3) && this.m_SubObjects.TryGetBuffer(component.m_Original, out bufferData) && (componentData3.m_Flags & AreaFlags.Complete) != (AreaFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckSubObjects(bufferData, nodes, createdEntities, minNodeDistance, (componentData3.m_Flags & AreaFlags.CounterClockwise) != 0);
          }
          if (clearAreas.IsCreated)
            clearAreas.Clear();
        }
        if (clearAreas.IsCreated)
          clearAreas.Dispose();
        createdEntities.Dispose();
      }

      private Entity GetSourceArea(Entity originalArea)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ApplyTempAreas.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ApplyTempAreas.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity applyTempArea = this.m_ApplyTempAreas[index];
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[applyTempArea];
            if (originalArea == temp.m_Original)
              return applyTempArea;
          }
        }
        return originalArea;
      }

      private void CheckSubObjects(
        DynamicBuffer<Game.Objects.SubObject> subObjects,
        DynamicBuffer<Game.Areas.Node> nodes,
        NativeParallelHashSet<Entity> createdEntities,
        float minNodeDistance,
        bool isCounterClockwise)
      {
        for (int index1 = 0; index1 < subObjects.Length; ++index1)
        {
          Game.Objects.SubObject subObject = subObjects[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingData.HasComponent(subObject.m_SubObject))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ApplyTempBuildings.IsCreated)
            {
              bool flag = false;
              // ISSUE: reference to a compiler-generated field
              for (int index2 = 0; index2 < this.m_ApplyTempBuildings.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ApplyTempBuildings[index2] == subObject.m_SubObject)
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = this.m_TransformData[subObject.m_SubObject];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subObject.m_SubObject];
            ObjectGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData))
            {
              float num1;
              if ((componentData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
              {
                num1 = componentData.m_Size.x * 0.5f;
              }
              else
              {
                num1 = math.length(MathUtils.Size(componentData.m_Bounds.xz)) * 0.5f;
                transform.m_Position.xz -= math.rotate(transform.m_Rotation, MathUtils.Center(componentData.m_Bounds)).xz;
              }
              float num2 = 0.0f;
              int y = -1;
              bool flag = nodes.Length <= 2;
              Game.Areas.Node node;
              if (!flag)
              {
                float num3 = float.MaxValue;
                float num4 = num1 + minNodeDistance;
                float num5 = num4 * num4;
                Line2.Segment line;
                ref Line2.Segment local1 = ref line;
                node = nodes[nodes.Length - 1];
                float2 xz1 = node.m_Position.xz;
                local1.a = xz1;
                for (int index3 = 0; index3 < nodes.Length; ++index3)
                {
                  ref Line2.Segment local2 = ref line;
                  node = nodes[index3];
                  float2 xz2 = node.m_Position.xz;
                  local2.b = xz2;
                  float t;
                  float num6 = MathUtils.DistanceSquared(line, transform.m_Position.xz, out t);
                  if ((double) num6 < (double) num5)
                  {
                    flag = true;
                    break;
                  }
                  if ((double) num6 < (double) num3)
                  {
                    num3 = num6;
                    num2 = t;
                    y = index3;
                  }
                  line.a = line.b;
                }
              }
              if (!flag && y >= 0)
              {
                int2 a1 = math.select(new int2(y - 1, y), new int2(y - 2, y + 1), new bool2((double) num2 == 0.0, (double) num2 == 1.0));
                int2 a2 = math.select(a1, a1 + new int2(nodes.Length, -nodes.Length), new bool2(a1.x < 0, a1.y >= nodes.Length));
                a2 = math.select(a2, a2.yx, isCounterClockwise);
                node = nodes[a2.x];
                float2 xz3 = node.m_Position.xz;
                node = nodes[a2.y];
                float2 xz4 = node.m_Position.xz;
                flag = (double) math.dot(transform.m_Position.xz - xz3, MathUtils.Right(xz4 - xz3)) <= 0.0;
              }
              if (flag)
              {
                // ISSUE: reference to a compiler-generated field
                Entity entity = this.m_CommandBuffer.CreateEntity();
                CreationDefinition component1 = new CreationDefinition();
                component1.m_Original = subObject.m_SubObject;
                component1.m_Flags |= CreationFlags.Delete;
                ObjectDefinition component2 = new ObjectDefinition();
                component2.m_ParentMesh = -1;
                component2.m_Position = transform.m_Position;
                component2.m_Rotation = transform.m_Rotation;
                component2.m_LocalPosition = transform.m_Position;
                component2.m_LocalRotation = transform.m_Rotation;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity, component2);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
                // ISSUE: reference to a compiler-generated method
                this.UpdateSubNets(transform, prefabRef.m_Prefab, subObject.m_SubObject, new NativeList<ClearAreaData>(), true);
                // ISSUE: reference to a compiler-generated method
                this.UpdateSubAreas(transform, prefabRef.m_Prefab, subObject.m_SubObject, createdEntities, new NativeList<ClearAreaData>(), true);
              }
            }
          }
        }
      }

      private OwnerDefinition GetOwnerDefinition(
        Entity parent,
        Entity area,
        NativeParallelHashSet<Entity> createdEntities,
        bool upgrade,
        bool fullUpdate,
        NativeList<ClearAreaData> clearAreas)
      {
        OwnerDefinition ownerDefinition1 = new OwnerDefinition();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode)
          return ownerDefinition1;
        Entity entity = parent;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.HasComponent(entity) && !this.m_BuildingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_OwnerData[entity].m_Owner;
        }
        OwnerDefinition ownerDefinition2 = new OwnerDefinition();
        DynamicBuffer<InstalledUpgrade> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InstalledUpgrades.TryGetBuffer(entity, out bufferData) && bufferData.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (fullUpdate && this.m_TransformData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = this.m_TransformData[entity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ClearAreaHelpers.FillClearAreas(bufferData, area, this.m_TransformData, this.m_ClearData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_SubAreas, this.m_Nodes, this.m_Triangles, ref clearAreas);
            ClearAreaHelpers.InitClearAreas(clearAreas, transform);
            if (createdEntities.Add(entity))
            {
              Entity owner = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnerData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                owner = this.m_OwnerData[entity].m_Owner;
              }
              // ISSUE: reference to a compiler-generated method
              this.UpdateOwnerObject(owner, entity, createdEntities, transform, new OwnerDefinition(), false, clearAreas);
            }
            // ISSUE: reference to a compiler-generated field
            ownerDefinition2.m_Prefab = this.m_PrefabRefData[entity].m_Prefab;
            ownerDefinition2.m_Position = transform.m_Position;
            ownerDefinition2.m_Rotation = transform.m_Rotation;
          }
          entity = bufferData[0].m_Upgrade;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[entity];
          if (createdEntities.Add(entity))
          {
            Entity owner = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (ownerDefinition2.m_Prefab == Entity.Null && this.m_OwnerData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              owner = this.m_OwnerData[entity].m_Owner;
            }
            // ISSUE: reference to a compiler-generated method
            this.UpdateOwnerObject(owner, entity, createdEntities, transform, ownerDefinition2, upgrade, new NativeList<ClearAreaData>());
          }
          // ISSUE: reference to a compiler-generated field
          ownerDefinition1.m_Prefab = this.m_PrefabRefData[entity].m_Prefab;
          ownerDefinition1.m_Position = transform.m_Position;
          ownerDefinition1.m_Rotation = transform.m_Rotation;
        }
        return ownerDefinition1;
      }

      private void UpdateOwnerObject(
        Entity owner,
        Entity original,
        NativeParallelHashSet<Entity> createdEntities,
        Game.Objects.Transform transform,
        OwnerDefinition ownerDefinition,
        bool upgrade,
        NativeList<ClearAreaData> clearAreas)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefData[original].m_Prefab;
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Owner = owner;
        component1.m_Original = original;
        if (upgrade)
          component1.m_Flags |= CreationFlags.Upgrade | CreationFlags.Parent;
        ObjectDefinition component2 = new ObjectDefinition();
        component2.m_ParentMesh = -1;
        component2.m_Position = transform.m_Position;
        component2.m_Rotation = transform.m_Rotation;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(this.m_TransformData[owner]), transform);
          component2.m_LocalPosition = local.m_Position;
          component2.m_LocalRotation = local.m_Rotation;
        }
        else
        {
          component2.m_LocalPosition = transform.m_Position;
          component2.m_LocalRotation = transform.m_Rotation;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        if (ownerDefinition.m_Prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
        }
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubNets(transform, prefab, original, clearAreas, false);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubAreas(transform, prefab, original, createdEntities, clearAreas, false);
      }

      private void UpdateSubNets(
        Game.Objects.Transform transform,
        Entity prefab,
        Entity original,
        NativeList<ClearAreaData> clearAreas,
        bool removeAll)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubNets.HasBuffer(original))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubNet> subNet1 = this.m_SubNets[original];
        for (int index = 0; index < subNet1.Length; ++index)
        {
          Entity subNet2 = subNet1[index].m_SubNet;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(subNet2))
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.HasEdgeStartOrEnd(subNet2, original))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Node node = this.m_NodeData[subNet2];
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity();
              CreationDefinition component = new CreationDefinition();
              component.m_Original = subNet2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorContainerData.HasComponent(subNet2))
              {
                // ISSUE: reference to a compiler-generated field
                component.m_SubPrefab = this.m_EditorContainerData[subNet2].m_Prefab;
              }
              Game.Net.Elevation componentData;
              // ISSUE: reference to a compiler-generated field
              bool onGround = !this.m_NetElevationData.TryGetComponent(subNet2, out componentData) || (double) math.cmin(math.abs(componentData.m_Elevation)) < 2.0;
              if (removeAll)
                component.m_Flags |= CreationFlags.Delete;
              else if (ClearAreaHelpers.ShouldClear(clearAreas, node.m_Position, onGround))
                component.m_Flags |= CreationFlags.Delete | CreationFlags.Hidden;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, new OwnerDefinition()
              {
                m_Prefab = prefab,
                m_Position = transform.m_Position,
                m_Rotation = transform.m_Rotation
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<NetCourse>(entity, new NetCourse()
              {
                m_Curve = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position),
                m_Length = 0.0f,
                m_FixedIndex = -1,
                m_StartPosition = {
                  m_Entity = subNet2,
                  m_Position = node.m_Position,
                  m_Rotation = node.m_Rotation,
                  m_CourseDelta = 0.0f
                },
                m_EndPosition = {
                  m_Entity = subNet2,
                  m_Position = node.m_Position,
                  m_Rotation = node.m_Rotation,
                  m_CourseDelta = 1f
                }
              });
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(subNet2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge = this.m_EdgeData[subNet2];
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity();
              CreationDefinition component1 = new CreationDefinition();
              component1.m_Original = subNet2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorContainerData.HasComponent(subNet2))
              {
                // ISSUE: reference to a compiler-generated field
                component1.m_SubPrefab = this.m_EditorContainerData[subNet2].m_Prefab;
              }
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subNet2];
              Game.Net.Elevation componentData;
              // ISSUE: reference to a compiler-generated field
              bool onGround = !this.m_NetElevationData.TryGetComponent(subNet2, out componentData) || (double) math.cmin(math.abs(componentData.m_Elevation)) < 2.0;
              if (removeAll)
                component1.m_Flags |= CreationFlags.Delete;
              else if (ClearAreaHelpers.ShouldClear(clearAreas, curve.m_Bezier, onGround))
                component1.m_Flags |= CreationFlags.Delete | CreationFlags.Hidden;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, new OwnerDefinition()
              {
                m_Prefab = prefab,
                m_Position = transform.m_Position,
                m_Rotation = transform.m_Rotation
              });
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              NetCourse component2 = new NetCourse()
              {
                m_Curve = curve.m_Bezier
              };
              component2.m_Length = MathUtils.Length(component2.m_Curve);
              component2.m_FixedIndex = -1;
              component2.m_StartPosition.m_Entity = edge.m_Start;
              component2.m_StartPosition.m_Position = component2.m_Curve.a;
              component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve));
              component2.m_StartPosition.m_CourseDelta = 0.0f;
              component2.m_EndPosition.m_Entity = edge.m_End;
              component2.m_EndPosition.m_Position = component2.m_Curve.d;
              component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve));
              component2.m_EndPosition.m_CourseDelta = 1f;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<NetCourse>(entity, component2);
            }
          }
        }
      }

      private bool HasEdgeStartOrEnd(Entity node, Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((edge2.m_Start == node || edge2.m_End == node) && this.m_OwnerData.HasComponent(edge1) && this.m_OwnerData[edge1].m_Owner == owner)
            return true;
        }
        return false;
      }

      private void UpdateSubAreas(
        Game.Objects.Transform transform,
        Entity prefab,
        Entity original,
        NativeParallelHashSet<Entity> createdEntities,
        NativeList<ClearAreaData> clearAreas,
        bool removeAll)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubAreas.HasBuffer(original))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.SubArea> subArea = this.m_SubAreas[original];
        for (int index = 0; index < subArea.Length; ++index)
        {
          Entity area = subArea[index].m_Area;
          if (createdEntities.Add(area))
          {
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity();
            CreationDefinition component = new CreationDefinition();
            component.m_Original = area;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, new OwnerDefinition()
            {
              m_Prefab = prefab,
              m_Position = transform.m_Position,
              m_Rotation = transform.m_Rotation
            });
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[area];
            if (removeAll)
            {
              component.m_Flags |= CreationFlags.Delete;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpaceData.HasComponent(area))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Triangle> triangle = this.m_Triangles[area];
                if (ClearAreaHelpers.ShouldClear(clearAreas, node, triangle, transform))
                  component.m_Flags |= CreationFlags.Delete | CreationFlags.Hidden;
              }
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity).CopyFrom(node.AsNativeArray());
            // ISSUE: reference to a compiler-generated field
            if (this.m_CachedNodes.HasBuffer(area))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LocalNodeCache> cachedNode = this.m_CachedNodes[area];
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity).CopyFrom(cachedNode.AsNativeArray());
            }
          }
        }
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AssetStampData> __Game_Prefabs_AssetStampData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Clear> __Game_Areas_Clear_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Space> __Game_Areas_Space_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Area> __Game_Areas_Area_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AssetStampData_RO_ComponentLookup = state.GetComponentLookup<AssetStampData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferLookup = state.GetBufferLookup<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferTypeHandle = state.GetBufferTypeHandle<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clear_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Clear>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Space_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Space>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentLookup = state.GetComponentLookup<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
