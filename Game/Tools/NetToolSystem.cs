// Decompiled with JetBrains decompiler
// Type: Game.Tools.NetToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Audio;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Effects;
using Game.Input;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using Game.Zones;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class NetToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Net Tool";
    private bool m_LoadingPreferences;
    private NetToolSystem.Mode m_Mode;
    private float m_Elevation;
    private float m_LastMouseElevation;
    private float m_ElevationStep;
    private int m_ParallelCount;
    private float m_ParallelOffset;
    private bool m_Underground;
    private Snap m_SelectedSnap;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Zones.SearchSystem m_ZoneSearchSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private AudioManager m_AudioManager;
    private NetInitializeSystem m_NetInitializeSystem;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_EdgeQuery;
    private EntityQuery m_NodeQuery;
    private EntityQuery m_SoundQuery;
    private EntityQuery m_ContainerQuery;
    private IProxyAction m_DowngradeNetEdge;
    private IProxyAction m_PlaceNetControlPoint;
    private IProxyAction m_PlaceNetEdge;
    private IProxyAction m_PlaceNetNode;
    private IProxyAction m_ReplaceNetEdge;
    private IProxyAction m_UndoNetControlPoint;
    private IProxyAction m_UpgradeNetEdge;
    private IProxyAction m_DiscardUpgrade;
    private IProxyAction m_DiscardDowngrade;
    private IProxyAction m_DiscardReplace;
    private bool m_ApplyBlocked;
    private NativeList<ControlPoint> m_ControlPoints;
    private NativeList<SnapLine> m_SnapLines;
    private NativeList<NetToolSystem.UpgradeState> m_UpgradeStates;
    private NativeReference<Entity> m_StartEntity;
    private NativeReference<Entity> m_LastSnappedEntity;
    private NativeReference<int> m_LastControlPointsAngle;
    private NativeReference<NetToolSystem.AppliedUpgrade> m_AppliedUpgrade;
    private ControlPoint m_LastRaycastPoint;
    private ControlPoint m_ApplyStartPoint;
    private NetToolSystem.State m_State;
    private Bounds1 m_LastElevationRange;
    private NetToolSystem.Mode m_LastActualMode;
    private float m_ApplyTimer;
    private NetPrefab m_Prefab;
    private NetPrefab m_SelectedPrefab;
    private NetLanePrefab m_LanePrefab;
    private bool m_AllowUndergroundReplace;
    private bool m_ForceCancel;
    private RandomSeed m_RandomSeed;
    private NetToolSystem.NetToolPreferences m_DefaultToolPreferences;
    private Dictionary<Entity, NetToolSystem.NetToolPreferences> m_ToolPreferences;
    private NetToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Net Tool";

    public override int uiModeIndex => (int) this.actualMode;

    public override void GetUIModes(List<ToolMode> modes)
    {
      if (this.upgradeOnly)
      {
        List<ToolMode> toolModeList = modes;
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.Mode mode = NetToolSystem.Mode.Replace;
        ToolMode toolMode = new ToolMode(mode.ToString(), 5);
        toolModeList.Add(toolMode);
      }
      else
      {
        List<ToolMode> toolModeList1 = modes;
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.Mode mode = NetToolSystem.Mode.Straight;
        ToolMode toolMode1 = new ToolMode(mode.ToString(), 0);
        toolModeList1.Add(toolMode1);
        List<ToolMode> toolModeList2 = modes;
        mode = NetToolSystem.Mode.SimpleCurve;
        ToolMode toolMode2 = new ToolMode(mode.ToString(), 1);
        toolModeList2.Add(toolMode2);
        List<ToolMode> toolModeList3 = modes;
        mode = NetToolSystem.Mode.ComplexCurve;
        ToolMode toolMode3 = new ToolMode(mode.ToString(), 2);
        toolModeList3.Add(toolMode3);
        List<ToolMode> toolModeList4 = modes;
        mode = NetToolSystem.Mode.Continuous;
        ToolMode toolMode4 = new ToolMode(mode.ToString(), 3);
        toolModeList4.Add(toolMode4);
        if (this.allowGrid)
        {
          List<ToolMode> toolModeList5 = modes;
          mode = NetToolSystem.Mode.Grid;
          ToolMode toolMode5 = new ToolMode(mode.ToString(), 4);
          toolModeList5.Add(toolMode5);
        }
        if (this.allowReplace)
        {
          List<ToolMode> toolModeList6 = modes;
          mode = NetToolSystem.Mode.Replace;
          ToolMode toolMode6 = new ToolMode(mode.ToString(), 5);
          toolModeList6.Add(toolMode6);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ToolSystem.actionMode.IsEditor())
          return;
        List<ToolMode> toolModeList7 = modes;
        mode = NetToolSystem.Mode.Point;
        ToolMode toolMode7 = new ToolMode(mode.ToString(), 6);
        toolModeList7.Add(toolMode7);
      }
    }

    public NetToolSystem.Mode mode
    {
      get => this.m_Mode;
      set
      {
        if (value == this.m_Mode)
          return;
        this.m_Mode = value;
        this.m_ForceUpdate = true;
        this.SaveToolPreferences();
      }
    }

    public NetToolSystem.Mode actualMode
    {
      get
      {
        if (this.upgradeOnly)
          return NetToolSystem.Mode.Replace;
        NetToolSystem.Mode mode = this.mode;
        switch (mode)
        {
          case NetToolSystem.Mode.Grid:
            return !this.allowGrid ? NetToolSystem.Mode.Straight : this.mode;
          case NetToolSystem.Mode.Replace:
            return !this.allowReplace ? NetToolSystem.Mode.Straight : this.mode;
          case NetToolSystem.Mode.Point:
            return !this.m_ToolSystem.actionMode.IsEditor() ? NetToolSystem.Mode.Straight : this.mode;
          default:
            return this.mode;
        }
      }
    }

    public float elevation
    {
      get => this.m_Elevation;
      set
      {
        if ((double) value == (double) this.m_Elevation)
          return;
        this.m_Elevation = value;
        this.m_ForceUpdate = true;
        this.SaveToolPreferences();
      }
    }

    public float elevationStep
    {
      get => this.m_ElevationStep;
      set
      {
        if ((double) value == (double) this.m_ElevationStep)
          return;
        this.m_ElevationStep = value;
        this.SaveToolPreferences();
      }
    }

    public int parallelCount
    {
      get => this.m_ParallelCount;
      set
      {
        if (value == this.m_ParallelCount)
          return;
        this.m_ParallelCount = value;
        this.m_ForceUpdate = true;
        this.SaveToolPreferences();
      }
    }

    public int actualParallelCount
    {
      get
      {
        return !this.allowParallel || this.allowGrid && this.mode == NetToolSystem.Mode.Grid ? 0 : this.parallelCount;
      }
    }

    public float parallelOffset
    {
      get => this.m_ParallelOffset;
      set
      {
        if ((double) value == (double) this.m_ParallelOffset)
          return;
        this.m_ParallelOffset = value;
        this.m_ForceUpdate = true;
        this.SaveToolPreferences();
      }
    }

    public bool underground
    {
      get => this.m_Underground;
      set
      {
        if (value == this.m_Underground)
          return;
        this.m_Underground = value;
        this.m_ForceUpdate = true;
        this.SaveToolPreferences();
      }
    }

    public override bool allowUnderground
    {
      get => this.actualMode == NetToolSystem.Mode.Replace && this.m_AllowUndergroundReplace;
    }

    public override Snap selectedSnap
    {
      get => this.m_SelectedSnap;
      set
      {
        if (value == this.m_SelectedSnap)
          return;
        this.m_SelectedSnap = value;
        this.m_ForceUpdate = true;
        this.SaveToolPreferences();
      }
    }

    public NetPrefab prefab
    {
      get => this.m_SelectedPrefab;
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) this.m_SelectedPrefab))
          return;
        this.m_SelectedPrefab = value;
        this.m_ForceUpdate = true;
        if ((UnityEngine.Object) value != (UnityEngine.Object) null)
          this.m_LanePrefab = (NetLanePrefab) null;
        PlaceableNetData component1;
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_SelectedPrefab, out component1))
        {
          this.upgradeOnly = (component1.m_PlacementFlags & Game.Net.PlacementFlags.UpgradeOnly) != 0;
          this.allowParallel = (component1.m_PlacementFlags & Game.Net.PlacementFlags.AllowParallel) != 0;
          NetGeometryData component2;
          this.allowGrid = this.allowParallel && (!this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) this.m_SelectedPrefab, out component2) || (double) component2.m_EdgeLengthRange.min == 0.0);
          NetData component3;
          this.allowReplace = this.m_PrefabSystem.TryGetComponentData<NetData>((PrefabBase) this.m_SelectedPrefab, out component3) && this.m_NetInitializeSystem.CanReplace(component3, this.m_ToolSystem.actionMode.IsGame());
          this.m_AllowUndergroundReplace = (double) component1.m_ElevationRange.min < 0.0 || (component1.m_PlacementFlags & Game.Net.PlacementFlags.UndergroundUpgrade) != 0;
        }
        else
        {
          this.upgradeOnly = false;
          this.allowParallel = false;
          this.allowGrid = false;
          this.allowReplace = false;
          this.m_AllowUndergroundReplace = false;
        }
        this.LoadToolPreferences();
        Action<PrefabBase> eventPrefabChanged = this.m_ToolSystem.EventPrefabChanged;
        if (eventPrefabChanged == null)
          return;
        eventPrefabChanged((PrefabBase) value);
      }
    }

    public NetLanePrefab lane
    {
      get => this.m_LanePrefab;
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) this.m_LanePrefab))
          return;
        this.m_LanePrefab = value;
        this.m_ForceUpdate = true;
        if ((UnityEngine.Object) value != (UnityEngine.Object) null)
        {
          this.m_SelectedPrefab = (NetPrefab) null;
          this.upgradeOnly = false;
          this.allowParallel = true;
          this.allowGrid = false;
          this.allowReplace = false;
        }
        this.LoadToolPreferences();
        Action<PrefabBase> eventPrefabChanged = this.m_ToolSystem.EventPrefabChanged;
        if (eventPrefabChanged == null)
          return;
        eventPrefabChanged((PrefabBase) value);
      }
    }

    public bool upgradeOnly { get; private set; }

    public bool allowParallel { get; private set; }

    public bool allowGrid { get; private set; }

    public bool allowReplace { get; private set; }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_DowngradeNetEdge;
        yield return this.m_PlaceNetControlPoint;
        yield return this.m_PlaceNetEdge;
        yield return this.m_PlaceNetNode;
        yield return this.m_ReplaceNetEdge;
        yield return this.m_UndoNetControlPoint;
        yield return this.m_UpgradeNetEdge;
        yield return this.m_DiscardUpgrade;
        yield return this.m_DiscardDowngrade;
        yield return this.m_DiscardReplace;
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
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSearchSystem = this.World.GetOrCreateSystemManaged<Game.Zones.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetInitializeSystem = this.World.GetOrCreateSystemManaged<NetInitializeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints = new NativeList<ControlPoint>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SnapLines = new NativeList<SnapLine>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeStates = new NativeList<NetToolSystem.UpgradeState>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StartEntity = new NativeReference<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LastSnappedEntity = new NativeReference<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LastControlPointsAngle = new NativeReference<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedUpgrade = new NativeReference<NetToolSystem.AppliedUpgrade>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ContainerQuery = this.GetContainerQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Lane>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Net.Edge>());
      // ISSUE: reference to a compiler-generated field
      this.m_NodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Net.Node>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DowngradeNetEdge = InputManager.instance.toolActionCollection.GetActionState("Downgrade Net Edge", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceNetControlPoint = InputManager.instance.toolActionCollection.GetActionState("Place Net Control Point", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceNetEdge = InputManager.instance.toolActionCollection.GetActionState("Place Net Edge", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceNetNode = InputManager.instance.toolActionCollection.GetActionState("Place Net Node", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_ReplaceNetEdge = InputManager.instance.toolActionCollection.GetActionState("Replace Net Edge", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_UndoNetControlPoint = InputManager.instance.toolActionCollection.GetActionState("Undo Net Control Point", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeNetEdge = InputManager.instance.toolActionCollection.GetActionState("Upgrade Net Edge", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardUpgrade = InputManager.instance.toolActionCollection.GetActionState("Discard Upgrade", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardDowngrade = InputManager.instance.toolActionCollection.GetActionState("Discard Downgrade", nameof (NetToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardReplace = InputManager.instance.toolActionCollection.GetActionState("Discard Replace", nameof (NetToolSystem));
      this.elevationStep = 10f;
      this.parallelOffset = 8f;
      this.selectedSnap &= ~(Snap.AutoParent | Snap.ContourLines);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_DefaultToolPreferences = new NetToolSystem.NetToolPreferences();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefaultToolPreferences.Save(this);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolPreferences = new Dictionary<Entity, NetToolSystem.NetToolPreferences>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SnapLines.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeStates.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StartEntity.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSnappedEntity.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LastControlPointsAngle.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedUpgrade.Dispose();
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
      this.m_SnapLines.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeStates.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_StartEntity.Value = new Entity();
      // ISSUE: reference to a compiler-generated field
      this.m_LastSnappedEntity.Value = new Entity();
      // ISSUE: reference to a compiler-generated field
      this.m_LastControlPointsAngle.Value = 0;
      // ISSUE: reference to a compiler-generated field
      ref NativeReference<NetToolSystem.AppliedUpgrade> local = ref this.m_AppliedUpgrade;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.AppliedUpgrade appliedUpgrade1 = new NetToolSystem.AppliedUpgrade();
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.AppliedUpgrade appliedUpgrade2 = appliedUpgrade1;
      local.Value = appliedUpgrade2;
      // ISSUE: reference to a compiler-generated field
      this.m_LastRaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyStartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = NetToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyTimer = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_RandomSeed = RandomSeed.Next();
      // ISSUE: reference to a compiler-generated field
      this.m_ForceCancel = false;
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyBlocked = false;
      this.requireZones = false;
      this.requireUnderground = false;
      this.requirePipelines = false;
      this.requireNetArrows = false;
      this.requireAreas = AreaTypeMask.None;
      this.requireNet = Layer.None;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.Mode actualMode = this.actualMode;
        switch (actualMode)
        {
          case NetToolSystem.Mode.Straight:
          case NetToolSystem.Mode.SimpleCurve:
          case NetToolSystem.Mode.ComplexCurve:
          case NetToolSystem.Mode.Continuous:
          case NetToolSystem.Mode.Grid:
            this.applyAction.enabled = this.actionsEnabled;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_ControlPoints.Length < NetToolSystem.GetMaxControlPointCount(this.actualMode) ? this.m_PlaceNetControlPoint : this.m_PlaceNetEdge;
            this.secondaryApplyAction.enabled = false;
            this.secondaryApplyActionOverride = (IProxyAction) null;
            // ISSUE: reference to a compiler-generated field
            this.cancelAction.enabled = this.actionsEnabled && this.m_ControlPoints.Length >= 2;
            // ISSUE: reference to a compiler-generated field
            this.cancelActionOverride = this.m_UndoNetControlPoint;
            break;
          case NetToolSystem.Mode.Replace:
            // ISSUE: reference to a compiler-generated field
            bool flag = this.m_ControlPoints.Length > 4;
            if (this.prefab.Has<NetUpgrade>())
            {
              if (!flag)
              {
                this.applyAction.enabled = this.actionsEnabled;
                // ISSUE: reference to a compiler-generated field
                this.applyActionOverride = this.m_UpgradeNetEdge;
                this.secondaryApplyAction.enabled = this.actionsEnabled;
                // ISSUE: reference to a compiler-generated field
                this.secondaryApplyActionOverride = this.m_DowngradeNetEdge;
                this.cancelAction.enabled = false;
                this.cancelActionOverride = (IProxyAction) null;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_State == NetToolSystem.State.Applying)
              {
                this.applyAction.enabled = this.actionsEnabled;
                // ISSUE: reference to a compiler-generated field
                this.applyActionOverride = this.m_UpgradeNetEdge;
                this.secondaryApplyAction.enabled = false;
                this.secondaryApplyActionOverride = (IProxyAction) null;
                this.cancelAction.enabled = this.actionsEnabled;
                // ISSUE: reference to a compiler-generated field
                this.cancelActionOverride = this.m_DiscardUpgrade;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_State != NetToolSystem.State.Cancelling)
                break;
              this.applyAction.enabled = false;
              this.applyActionOverride = (IProxyAction) null;
              this.secondaryApplyAction.enabled = this.actionsEnabled;
              // ISSUE: reference to a compiler-generated field
              this.secondaryApplyActionOverride = this.m_DowngradeNetEdge;
              this.cancelAction.enabled = this.actionsEnabled;
              // ISSUE: reference to a compiler-generated field
              this.cancelActionOverride = this.m_DiscardDowngrade;
              break;
            }
            this.applyAction.enabled = this.actionsEnabled;
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_ReplaceNetEdge;
            this.secondaryApplyAction.enabled = false;
            this.secondaryApplyActionOverride = (IProxyAction) null;
            if (flag)
            {
              this.cancelAction.enabled = this.actionsEnabled;
              // ISSUE: reference to a compiler-generated field
              this.cancelActionOverride = this.m_DiscardReplace;
              break;
            }
            this.cancelAction.enabled = false;
            this.cancelActionOverride = (IProxyAction) null;
            break;
          case NetToolSystem.Mode.Point:
            this.applyAction.enabled = this.actionsEnabled;
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_PlaceNetNode;
            this.secondaryApplyAction.enabled = false;
            this.secondaryApplyActionOverride = (IProxyAction) null;
            // ISSUE: reference to a compiler-generated field
            this.cancelAction.enabled = this.actionsEnabled && this.m_ControlPoints.Length >= 2;
            // ISSUE: reference to a compiler-generated field
            this.cancelActionOverride = this.m_UndoNetControlPoint;
            break;
        }
      }
    }

    public override PrefabBase GetPrefab()
    {
      return !((UnityEngine.Object) this.prefab != (UnityEngine.Object) null) ? (PrefabBase) this.lane : (PrefabBase) this.prefab;
    }

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      switch (prefab)
      {
        case NetPrefab netPrefab:
          this.prefab = netPrefab;
          return true;
        case NetLanePrefab netLanePrefab:
          this.lane = netLanePrefab;
          return true;
        default:
          return false;
      }
    }

    private void LoadToolPreferences()
    {
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab = this.GetPrefab();
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingPreferences = true;
      UIObjectData component;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.EntityManager.TryGetComponent<UIObjectData>(this.m_PrefabSystem.GetEntity(prefab), out component);
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.NetToolPreferences netToolPreferences;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolPreferences.TryGetValue(component.m_Group, out netToolPreferences))
      {
        // ISSUE: reference to a compiler-generated method
        netToolPreferences.Load(this);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_DefaultToolPreferences.Load(this);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_LoadingPreferences = false;
    }

    private void SaveToolPreferences()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadingPreferences)
        return;
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab = this.GetPrefab();
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
        return;
      UIObjectData component;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.EntityManager.TryGetComponent<UIObjectData>(this.m_PrefabSystem.GetEntity(prefab), out component);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ToolPreferences.ContainsKey(component.m_Group))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_ToolPreferences[component.m_Group] = new NetToolSystem.NetToolPreferences();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolPreferences[component.m_Group].Save(this);
    }

    public void ResetToolPreferences()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ToolPreferences.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefaultToolPreferences.Load(this);
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      // ISSUE: reference to a compiler-generated method
      this.ResetToolPreferences();
    }

    public NativeList<ControlPoint> GetControlPoints(out JobHandle dependencies)
    {
      dependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      return this.m_ControlPoints;
    }

    public NativeList<SnapLine> GetSnapLines(out JobHandle dependencies)
    {
      dependencies = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      return this.m_SnapLines;
    }

    private NetPrefab GetNetPrefab()
    {
      Entity laneContainer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      return this.m_ToolSystem.actionMode.IsEditor() && (UnityEngine.Object) this.m_LanePrefab != (UnityEngine.Object) null && this.GetContainers(this.m_ContainerQuery, out laneContainer, out Entity _) ? this.m_PrefabSystem.GetPrefab<NetPrefab>(laneContainer) : this.m_SelectedPrefab;
    }

    public override void SetUnderground(bool underground)
    {
      if (this.actualMode != NetToolSystem.Mode.Replace)
        return;
      this.underground = underground;
    }

    public override void ElevationUp()
    {
      // ISSUE: reference to a compiler-generated method
      NetPrefab netPrefab = this.GetNetPrefab();
      if (!((UnityEngine.Object) netPrefab != (UnityEngine.Object) null))
        return;
      if (this.actualMode == NetToolSystem.Mode.Replace)
      {
        this.underground = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = netPrefab;
        PlaceableNetData component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component) && component.m_UndergroundPrefab != Entity.Null && (double) this.elevation < 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(component.m_UndergroundPrefab);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component))
          return;
        // ISSUE: reference to a compiler-generated method
        this.CheckElevationRange(component);
        this.elevation = math.floor((float) ((double) this.elevation / (double) this.elevationStep + 1.0000100135803223)) * this.elevationStep;
        // ISSUE: reference to a compiler-generated field
        if ((double) this.elevation <= (double) component.m_ElevationRange.max + (double) this.elevationStep * 0.5 || !((UnityEngine.Object) this.m_Prefab != (UnityEngine.Object) netPrefab))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = netPrefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component);
        // ISSUE: reference to a compiler-generated method
        this.CheckElevationRange(component);
      }
    }

    public override void ElevationDown()
    {
      // ISSUE: reference to a compiler-generated method
      NetPrefab netPrefab = this.GetNetPrefab();
      if (!((UnityEngine.Object) netPrefab != (UnityEngine.Object) null))
        return;
      if (this.actualMode == NetToolSystem.Mode.Replace)
      {
        this.underground = true;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = netPrefab;
        PlaceableNetData component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component) && component.m_UndergroundPrefab != Entity.Null && (double) this.elevation < 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(component.m_UndergroundPrefab);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component))
          return;
        // ISSUE: reference to a compiler-generated method
        this.CheckElevationRange(component);
        this.elevation = math.ceil((float) ((double) this.elevation / (double) this.elevationStep - 1.0000100135803223)) * this.elevationStep;
        // ISSUE: reference to a compiler-generated field
        if ((double) this.elevation >= (double) component.m_ElevationRange.min - (double) this.elevationStep * 0.5 || !((UnityEngine.Object) this.m_Prefab == (UnityEngine.Object) netPrefab) || !(component.m_UndergroundPrefab != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(component.m_UndergroundPrefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component);
        // ISSUE: reference to a compiler-generated method
        this.CheckElevationRange(component);
      }
    }

    public override void ElevationScroll()
    {
      // ISSUE: reference to a compiler-generated method
      NetPrefab netPrefab = this.GetNetPrefab();
      if (!((UnityEngine.Object) netPrefab != (UnityEngine.Object) null))
        return;
      if (this.actualMode == NetToolSystem.Mode.Replace)
      {
        this.underground = !this.underground;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = netPrefab;
        PlaceableNetData component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component) && component.m_UndergroundPrefab != Entity.Null && (double) this.elevation < 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(component.m_UndergroundPrefab);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component))
          return;
        this.elevation = math.floor((float) ((double) this.elevation / (double) this.elevationStep + 1.0000100135803223)) * this.elevationStep;
        if ((double) this.elevation <= (double) component.m_ElevationRange.max + (double) this.elevationStep * 0.5)
          return;
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_Prefab != (UnityEngine.Object) netPrefab)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Prefab = netPrefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component);
          // ISSUE: reference to a compiler-generated method
          this.CheckElevationRange(component);
        }
        else
        {
          if (component.m_UndergroundPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_Prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(component.m_UndergroundPrefab);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component);
          }
          this.elevation = math.ceil(component.m_ElevationRange.min / this.elevationStep) * this.elevationStep;
          // ISSUE: reference to a compiler-generated method
          this.CheckElevationRange(component);
        }
      }
    }

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
      // ISSUE: reference to a compiler-generated method
      NetPrefab netPrefab = this.GetNetPrefab();
      // ISSUE: reference to a compiler-generated field
      this.m_Prefab = (NetPrefab) null;
      if (this.actualMode == NetToolSystem.Mode.Replace)
      {
        if ((UnityEngine.Object) netPrefab != (UnityEngine.Object) null)
        {
          PlaceableNetData component1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) netPrefab, out component1))
          {
            if ((component1.m_PlacementFlags & Game.Net.PlacementFlags.UndergroundUpgrade) == Game.Net.PlacementFlags.None)
            {
              if ((double) component1.m_ElevationRange.min >= 0.0 && component1.m_UndergroundPrefab == Entity.Null)
                this.underground = false;
              else if ((double) component1.m_ElevationRange.max < 0.0 && component1.m_UndergroundPrefab == Entity.Null)
                this.underground = true;
            }
          }
          else
            this.underground = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Prefab = !this.underground || !(component1.m_UndergroundPrefab != Entity.Null) ? netPrefab : this.m_PrefabSystem.GetPrefab<NetPrefab>(component1.m_UndergroundPrefab);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NetData componentData = this.m_PrefabSystem.GetComponentData<NetData>((PrefabBase) this.m_Prefab);
          NetGeometryData component2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) this.m_Prefab, out component2);
          if (this.underground)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.collisionMask = CollisionMask.Underground;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.collisionMask = CollisionMask.OnGround | CollisionMask.Overground;
          }
          if ((component2.m_Flags & Game.Net.GeometryFlags.Marker) != (Game.Net.GeometryFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Markers;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.IgnoreSecondary;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.Net;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.netLayerMask = componentData.m_RequiredLayers;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.collisionMask = (CollisionMask) 0;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.Net;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.netLayerMask = Layer.None;
        }
      }
      else if ((UnityEngine.Object) netPrefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        if (InputManager.instance.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse && this.m_State == NetToolSystem.State.Applying && SharedSettings.instance.input.elevationDraggingEnabled)
        {
          Camera main = Camera.main;
          if ((UnityEngine.Object) main != (UnityEngine.Object) null && InputManager.instance.mouseOnScreen)
          {
            Line3 raycastLine = (Line3) ToolRaycastSystem.CalculateRaycastLine(main);
            // ISSUE: reference to a compiler-generated field
            float3 hitPosition = this.m_ApplyStartPoint.m_HitPosition;
            // ISSUE: reference to a compiler-generated field
            hitPosition.y += this.m_ApplyStartPoint.m_Elevation;
            Triangle3 plane = new Triangle3(hitPosition, hitPosition + math.up(), hitPosition + (float3) main.transform.right);
            float d;
            // ISSUE: reference to a compiler-generated method
            if (NetToolSystem.TryIntersectLineWithPlane(raycastLine, plane, 0.05f, out d) && (double) d >= 0.0 && (double) d <= 1.0)
            {
              float3 y1 = MathUtils.Position(raycastLine, d);
              float x = y1.y - hitPosition.y;
              float num1 = math.distance(raycastLine.a, y1);
              float y2 = 2f * math.tan(math.radians(math.min(89f, main.fieldOfView * 0.5f))) * num1;
              // ISSUE: reference to a compiler-generated field
              if ((double) this.m_ApplyTimer >= 0.5 / (1.0 + (double) (math.abs(x) / math.max(1f, y2)) * 20.0))
              {
                float overground;
                float underground;
                // ISSUE: reference to a compiler-generated method
                this.GetSurfaceHeights(netPrefab, out overground, out underground);
                // ISSUE: reference to a compiler-generated field
                bool c1 = (double) this.m_ApplyStartPoint.m_Elevation < 0.0;
                float num2 = math.select(overground, underground, c1);
                // ISSUE: reference to a compiler-generated field
                this.elevation = this.m_ApplyStartPoint.m_Elevation + x - num2;
                this.elevation = math.round(this.elevation / this.elevationStep) * this.elevationStep;
                bool c2 = (double) this.elevation < 0.0;
                if ((double) overground != (double) underground && c2 != c1)
                {
                  float num3 = math.select(overground, underground, c2);
                  // ISSUE: reference to a compiler-generated field
                  this.elevation = this.m_ApplyStartPoint.m_Elevation + x - num3;
                  this.elevation = math.round(this.elevation / this.elevationStep) * this.elevationStep;
                  bool c3 = (double) this.elevation < 0.0;
                  if (c3 != c2)
                    this.elevation = math.select(0.0f, -this.elevationStep, c3);
                }
                // ISSUE: reference to a compiler-generated field
                if ((double) this.elevation > (double) this.m_LastMouseElevation)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetElevationUpSound);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((double) this.elevation < (double) this.m_LastMouseElevation)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetElevationDownSound);
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_LastMouseElevation = this.elevation;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ApplyTimer += UnityEngine.Time.deltaTime;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = netPrefab;
        PlaceableNetData component3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) netPrefab, out component3) && component3.m_UndergroundPrefab != Entity.Null && (double) this.elevation < 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Prefab = this.m_PrefabSystem.GetPrefab<NetPrefab>(component3.m_UndergroundPrefab);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component3))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckElevationRange(component3);
          this.elevation = MathUtils.Clamp(this.elevation, component3.m_ElevationRange);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastElevationRange = new Bounds1();
          this.elevation = 0.0f;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NetData componentData = this.m_PrefabSystem.GetComponentData<NetData>((PrefabBase) this.m_Prefab);
        NetGeometryData component4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) this.m_Prefab, out component4);
        if ((double) this.elevation < 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.collisionMask = CollisionMask.Underground;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.collisionMask = CollisionMask.OnGround | CollisionMask.Overground;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Water;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.ElevateOffset | RaycastFlags.SubElements | RaycastFlags.Outside | RaycastFlags.IgnoreSecondary;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.netLayerMask = componentData.m_ConnectLayers;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.rayOffset = new float3(0.0f, -component4.m_DefaultSurfaceHeight.max - this.elevation, 0.0f);
        Snap onMask;
        Snap offMask;
        // ISSUE: reference to a compiler-generated method
        this.GetAvailableSnapMask(out onMask, out offMask);
        // ISSUE: reference to a compiler-generated method
        int actualSnap = (int) ToolBaseSystem.GetActualSnap(this.selectedSnap, onMask, offMask);
        if ((actualSnap & 513) != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.Net;
          if ((component4.m_Flags & Game.Net.GeometryFlags.Marker) != (Game.Net.GeometryFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Markers;
          }
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
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.collisionMask = (CollisionMask) 0;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.ElevateOffset | RaycastFlags.SubElements | RaycastFlags.Outside;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Net | TypeMask.Water;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.netLayerMask = Layer.None;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.rayOffset = new float3();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ToolSystem.actionMode.IsEditor())
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubElements | RaycastFlags.UpgradeIsMain;
    }

    private static bool TryIntersectLineWithPlane(
      Line3 line,
      Triangle3 plane,
      float minDot,
      out float d)
    {
      float3 x = math.normalize(MathUtils.NormalCW(plane));
      if ((double) math.abs(math.dot(x, math.normalize(line.ab))) > (double) minDot)
      {
        float3 y = line.a - plane.a;
        d = -math.dot(x, y) / math.dot(x, line.ab);
        return true;
      }
      d = 0.0f;
      return false;
    }

    private void GetSurfaceHeights(NetPrefab prefab, out float overground, out float underground)
    {
      overground = 0.0f;
      underground = 0.0f;
      NetGeometryData component1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) prefab, out component1))
      {
        overground = component1.m_DefaultSurfaceHeight.max;
        underground = component1.m_DefaultSurfaceHeight.max;
      }
      PlaceableNetData component2;
      NetGeometryData component3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      if (!this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) prefab, out component2) || !(component2.m_UndergroundPrefab != Entity.Null) || !this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) this.m_PrefabSystem.GetPrefab<NetPrefab>(component2.m_UndergroundPrefab), out component3))
        return;
      underground = component3.m_DefaultSurfaceHeight.max;
    }

    private void CheckElevationRange(PlaceableNetData placeableNetData)
    {
      // ISSUE: reference to a compiler-generated field
      if (placeableNetData.m_ElevationRange.Equals(this.m_LastElevationRange))
        return;
      float position = MathUtils.Clamp(0.0f, placeableNetData.m_ElevationRange);
      // ISSUE: reference to a compiler-generated field
      if (!MathUtils.Intersect(this.m_LastElevationRange, position) || !MathUtils.Intersect(placeableNetData.m_ElevationRange, this.elevation))
        this.elevation = position;
      // ISSUE: reference to a compiler-generated field
      this.m_LastElevationRange = placeableNetData.m_ElevationRange;
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      // ISSUE: reference to a compiler-generated field
      if (this.m_FocusChanged)
        return inputDeps;
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.Mode actualMode = this.actualMode;
      // ISSUE: reference to a compiler-generated field
      if (actualMode != this.m_LastActualMode)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LastActualMode == NetToolSystem.Mode.Replace || actualMode == NetToolSystem.Mode.Replace)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_State = NetToolSystem.State.Default;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          int controlPointCount = NetToolSystem.GetMaxControlPointCount(actualMode);
          // ISSUE: reference to a compiler-generated field
          if (controlPointCount < this.m_ControlPoints.Length)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.RemoveRange(controlPointCount, this.m_ControlPoints.Length - controlPointCount);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastActualMode = actualMode;
      }
      // ISSUE: reference to a compiler-generated field
      bool forceCancel = this.m_ForceCancel;
      // ISSUE: reference to a compiler-generated field
      this.m_ForceCancel = false;
      if (actualMode != NetToolSystem.Mode.Replace)
      {
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.UpdateStartEntity(inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Prefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NetData componentData = this.m_PrefabSystem.GetComponentData<NetData>((PrefabBase) this.m_Prefab);
        NetGeometryData component1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) this.m_Prefab, out component1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        bool laneContainer = this.m_PrefabSystem.HasComponent<EditorContainerData>((PrefabBase) this.m_Prefab);
        this.requireZones = false;
        this.requireUnderground = this.underground;
        this.requirePipelines = false;
        this.requireNetArrows = (component1.m_Flags & Game.Net.GeometryFlags.Directional) != 0;
        this.requireAreas = AreaTypeMask.None;
        this.requireNet = componentData.m_ConnectLayers | componentData.m_RequiredLayers | component1.m_MergeLayers | component1.m_IntersectLayers;
        if (this.actualMode != NetToolSystem.Mode.Replace)
        {
          this.requireUnderground = (double) this.elevation < 0.0 && ((double) this.elevation <= (double) component1.m_ElevationLimit * -3.0 || (component1.m_Flags & Game.Net.GeometryFlags.LoweredIsTunnel) != 0);
          this.requirePipelines = (double) this.elevation < 0.0;
        }
        PlaceableNetData component2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component2))
        {
          if ((component2.m_PlacementFlags & Game.Net.PlacementFlags.OnGround) != Game.Net.PlacementFlags.None && !this.requireUnderground)
          {
            this.requireZones = true;
            this.requireAreas |= AreaTypeMask.Lots;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ToolSystem.actionMode.IsEditor())
              this.requireAreas |= AreaTypeMask.Spaces;
          }
          if (actualMode != NetToolSystem.Mode.Replace && ((double) component2.m_ElevationRange.max > 0.0 || (component1.m_Flags & Game.Net.GeometryFlags.RequireElevated) != (Game.Net.GeometryFlags) 0) && !this.requireUnderground)
            this.requireNet |= Layer.Waterway;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(this.m_ToolSystem.actionMode.IsEditor() ? Entity.Null : this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Prefab));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NetToolSystem.GetAvailableSnapMask(component1, component2, actualMode, this.m_ToolSystem.actionMode.IsEditor(), laneContainer, this.requireUnderground, out this.m_SnapOnMask, out this.m_SnapOffMask);
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != NetToolSystem.State.Default && !this.applyAction.enabled && !this.secondaryApplyAction.enabled)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = NetToolSystem.State.Default;
        }
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
        {
          if (this.actualMode == NetToolSystem.Mode.Replace)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.State state = this.m_State;
            switch (state)
            {
              case NetToolSystem.State.Default:
                // ISSUE: reference to a compiler-generated field
                if (this.m_ApplyBlocked)
                {
                  if (this.applyAction.WasReleasedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame())
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_ApplyBlocked = false;
                  }
                  // ISSUE: reference to a compiler-generated method
                  return this.Update(inputDeps, false);
                }
                if (this.applyAction.WasPressedThisFrame())
                {
                  // ISSUE: reference to a compiler-generated method
                  return this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame());
                }
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                return this.secondaryApplyAction.WasPressedThisFrame() ? this.Cancel(inputDeps, this.secondaryApplyAction.WasReleasedThisFrame()) : this.Update(inputDeps, false);
              case NetToolSystem.State.Applying:
                if (this.cancelAction.WasPressedThisFrame())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ApplyBlocked = true;
                  // ISSUE: reference to a compiler-generated field
                  this.m_State = NetToolSystem.State.Default;
                  // ISSUE: reference to a compiler-generated method
                  return this.Update(inputDeps, true);
                }
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                return this.applyAction.WasReleasedThisFrame() ? this.Apply(inputDeps) : this.Update(inputDeps, false);
              case NetToolSystem.State.Cancelling:
                if (this.cancelAction.WasPressedThisFrame())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ApplyBlocked = true;
                  // ISSUE: reference to a compiler-generated field
                  this.m_State = NetToolSystem.State.Default;
                  // ISSUE: reference to a compiler-generated method
                  return this.Update(inputDeps, true);
                }
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                return this.secondaryApplyAction.WasReleasedThisFrame() ? this.Cancel(inputDeps) : this.Update(inputDeps, false);
              default:
                // ISSUE: reference to a compiler-generated method
                return this.Update(inputDeps, false);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != NetToolSystem.State.Cancelling && this.cancelAction.WasPressedThisFrame())
            {
              // ISSUE: reference to a compiler-generated method
              return this.Cancel(inputDeps, this.cancelAction.WasReleasedThisFrame());
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_State == NetToolSystem.State.Cancelling && (forceCancel || this.cancelAction.WasReleasedThisFrame()))
            {
              // ISSUE: reference to a compiler-generated method
              return this.Cancel(inputDeps);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != NetToolSystem.State.Applying && this.applyAction.WasPressedThisFrame())
            {
              // ISSUE: reference to a compiler-generated method
              return this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame());
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            return this.m_State == NetToolSystem.State.Applying && this.applyAction.WasReleasedThisFrame() ? this.Apply(inputDeps) : this.Update(inputDeps, false);
          }
        }
      }
      else
      {
        this.requireZones = false;
        this.requireUnderground = false;
        this.requirePipelines = false;
        this.requireNetArrows = false;
        this.requireAreas = AreaTypeMask.None;
        this.requireNet = Layer.None;
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == NetToolSystem.State.Applying && (this.applyAction.WasReleasedThisFrame() || this.cancelAction.WasPressedThisFrame()))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_State = NetToolSystem.State.Default;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == NetToolSystem.State.Cancelling && (this.secondaryApplyAction.WasReleasedThisFrame() || this.cancelAction.WasPressedThisFrame()))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = NetToolSystem.State.Default;
        }
      }
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    private static int GetMaxControlPointCount(NetToolSystem.Mode mode)
    {
      switch (mode)
      {
        case NetToolSystem.Mode.Straight:
          return 2;
        case NetToolSystem.Mode.SimpleCurve:
        case NetToolSystem.Mode.Continuous:
        case NetToolSystem.Mode.Grid:
          return 3;
        case NetToolSystem.Mode.ComplexCurve:
          return 4;
        default:
          return 1;
      }
    }

    public override void GetAvailableSnapMask(out Snap onMask, out Snap offMask)
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Prefab != (UnityEngine.Object) null)
      {
        NetGeometryData component1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetComponentData<NetGeometryData>((PrefabBase) this.m_Prefab, out component1);
        PlaceableNetData component2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.m_Prefab, out component2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        bool laneContainer = this.m_PrefabSystem.HasComponent<EditorContainerData>((PrefabBase) this.m_Prefab);
        bool underground = this.underground;
        if (this.actualMode != NetToolSystem.Mode.Replace)
          underground = (double) this.elevation < 0.0 && ((double) this.elevation <= (double) component1.m_ElevationLimit * -3.0 || (component1.m_Flags & Game.Net.GeometryFlags.LoweredIsTunnel) != 0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NetToolSystem.GetAvailableSnapMask(component1, component2, this.actualMode, this.m_ToolSystem.actionMode.IsEditor(), laneContainer, underground, out onMask, out offMask);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        base.GetAvailableSnapMask(out onMask, out offMask);
      }
    }

    private static void GetAvailableSnapMask(
      NetGeometryData prefabGeometryData,
      PlaceableNetData placeableNetData,
      NetToolSystem.Mode mode,
      bool editorMode,
      bool laneContainer,
      bool underground,
      out Snap onMask,
      out Snap offMask)
    {
      if (mode == NetToolSystem.Mode.Replace)
      {
        onMask = Snap.ExistingGeometry;
        offMask = onMask;
        if ((placeableNetData.m_PlacementFlags & Game.Net.PlacementFlags.UpgradeOnly) == Game.Net.PlacementFlags.None)
        {
          onMask |= Snap.ContourLines;
          offMask |= Snap.ContourLines;
        }
        if (laneContainer)
        {
          onMask &= ~Snap.ExistingGeometry;
          offMask &= ~Snap.ExistingGeometry;
          onMask |= Snap.NearbyGeometry;
        }
        else
        {
          if ((prefabGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
            offMask &= ~Snap.ExistingGeometry;
          if ((prefabGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) == (Game.Net.GeometryFlags) 0)
            return;
          onMask |= Snap.CellLength;
          offMask |= Snap.CellLength;
        }
      }
      else
      {
        onMask = Snap.ExistingGeometry | Snap.CellLength | Snap.StraightDirection | Snap.ObjectSide | Snap.GuideLines | Snap.ZoneGrid | Snap.ContourLines;
        offMask = onMask;
        if (underground)
          onMask &= ~(Snap.ObjectSide | Snap.ZoneGrid);
        if (laneContainer)
        {
          onMask &= ~(Snap.CellLength | Snap.ObjectSide);
          offMask &= ~(Snap.CellLength | Snap.ObjectSide);
        }
        else if ((prefabGeometryData.m_Flags & Game.Net.GeometryFlags.Marker) != (Game.Net.GeometryFlags) 0)
        {
          onMask &= ~Snap.ObjectSide;
          offMask &= ~Snap.ObjectSide;
        }
        if (laneContainer)
        {
          onMask &= ~Snap.ExistingGeometry;
          offMask &= ~Snap.ExistingGeometry;
          onMask |= Snap.NearbyGeometry;
          offMask |= Snap.NearbyGeometry;
        }
        else if ((prefabGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
        {
          offMask &= ~Snap.ExistingGeometry;
          onMask |= Snap.NearbyGeometry;
          offMask |= Snap.NearbyGeometry;
        }
        if (!editorMode)
          return;
        onMask |= Snap.ObjectSurface | Snap.LotGrid | Snap.AutoParent;
        offMask |= Snap.ObjectSurface | Snap.LotGrid | Snap.AutoParent;
      }
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      if (this.actualMode == NetToolSystem.Mode.Replace)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != NetToolSystem.State.Cancelling && this.m_ControlPoints.Length >= 1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = NetToolSystem.State.Cancelling;
          // ISSUE: reference to a compiler-generated field
          this.m_ForceCancel = singleFrameOnly;
          // ISSUE: reference to a compiler-generated field
          ref NativeReference<NetToolSystem.AppliedUpgrade> local = ref this.m_AppliedUpgrade;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.AppliedUpgrade appliedUpgrade1 = new NetToolSystem.AppliedUpgrade();
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.AppliedUpgrade appliedUpgrade2 = appliedUpgrade1;
          local.Value = appliedUpgrade2;
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps, true);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_State = NetToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (this.GetAllowApply() && !this.m_EdgeQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetAppliedUpgrade(true);
          this.applyMode = ApplyMode.Apply;
          // ISSUE: reference to a compiler-generated field
          this.m_RandomSeed = RandomSeed.Next();
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradeStates.Clear();
          ControlPoint controlPoint;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint))
          {
            controlPoint.m_Elevation = this.elevation;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, false);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.FixControlPoints(inputDeps);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateCourse(inputDeps, false);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PolygonToolRemovePointSound);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
          }
        }
        else
        {
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradeStates.Clear();
          ControlPoint controlPoint;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint))
          {
            controlPoint.m_Elevation = this.elevation;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, false);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateCourse(inputDeps, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
          }
        }
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetCancelSound);
      // ISSUE: reference to a compiler-generated field
      this.m_State = NetToolSystem.State.Default;
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeStates.Clear();
      // ISSUE: reference to a compiler-generated field
      if (this.m_ControlPoints.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.RemoveAt(this.m_ControlPoints.Length - 1);
      }
      ControlPoint controlPoint1;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint1))
      {
        controlPoint1.m_HitPosition.y += this.elevation;
        controlPoint1.m_Elevation = this.elevation;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints[this.m_ControlPoints.Length - 1] = controlPoint1;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint1);
        }
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.SnapControlPoints(inputDeps, false);
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.UpdateCourse(inputDeps, false);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      }
      return inputDeps;
    }

    private void SetAppliedUpgrade(bool removing)
    {
      // ISSUE: reference to a compiler-generated field
      ref NativeReference<NetToolSystem.AppliedUpgrade> local = ref this.m_AppliedUpgrade;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.AppliedUpgrade appliedUpgrade1 = new NetToolSystem.AppliedUpgrade();
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.AppliedUpgrade appliedUpgrade2 = appliedUpgrade1;
      local.Value = appliedUpgrade2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpgradeStates.Length < 1 || this.m_ControlPoints.Length < 4)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Entity originalEntity1 = this.m_ControlPoints[this.m_ControlPoints.Length - 3].m_OriginalEntity;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Entity originalEntity2 = this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_OriginalEntity;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.UpgradeState upgradeState = this.m_UpgradeStates[this.m_UpgradeStates.Length - 1];
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.AppliedUpgrade appliedUpgrade3 = new NetToolSystem.AppliedUpgrade()
      {
        m_SubReplacementPrefab = upgradeState.m_SubReplacementPrefab,
        m_Flags = removing ? upgradeState.m_RemoveFlags : upgradeState.m_AddFlags,
        m_SubReplacementType = upgradeState.m_SubReplacementType,
        m_SubReplacementSide = upgradeState.m_SubReplacementSide
      };
      if (originalEntity1 == originalEntity2)
      {
        // ISSUE: reference to a compiler-generated field
        appliedUpgrade3.m_Entity = originalEntity1;
        // ISSUE: reference to a compiler-generated field
        this.m_AppliedUpgrade.Value = appliedUpgrade3;
      }
      else
      {
        DynamicBuffer<ConnectedEdge> buffer;
        if (!this.EntityManager.TryGetBuffer<ConnectedEdge>(originalEntity1, true, out buffer))
          return;
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity edge = buffer[index].m_Edge;
          Game.Net.Edge component;
          if (this.EntityManager.TryGetComponent<Game.Net.Edge>(edge, out component) && (component.m_Start == originalEntity1 && component.m_End == originalEntity2 || component.m_End == originalEntity1 && component.m_Start == originalEntity2))
          {
            // ISSUE: reference to a compiler-generated field
            appliedUpgrade3.m_Entity = edge;
            // ISSUE: reference to a compiler-generated field
            this.m_AppliedUpgrade.Value = appliedUpgrade3;
          }
        }
      }
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint)
    {
      Entity entity;
      RaycastHit hit;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out entity, out hit))
      {
        PlaceableNetData component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.actualMode == NetToolSystem.Mode.Replace && this.EntityManager.HasComponent<Game.Net.Node>(entity) && this.EntityManager.HasComponent<Game.Net.Edge>(hit.m_HitEntity) && this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.prefab, out component) && (component.m_PlacementFlags & Game.Net.PlacementFlags.NodeUpgrade) == Game.Net.PlacementFlags.None)
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
        PlaceableNetData component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.actualMode == NetToolSystem.Mode.Replace && this.EntityManager.HasComponent<Game.Net.Node>(entity) && this.EntityManager.HasComponent<Game.Net.Edge>(hit.m_HitEntity) && this.m_PrefabSystem.TryGetComponentData<PlaceableNetData>((PrefabBase) this.prefab, out component) && (component.m_PlacementFlags & Game.Net.PlacementFlags.NodeUpgrade) == Game.Net.PlacementFlags.None)
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      return inputDeps;
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.Mode actualMode = this.actualMode;
      if (actualMode == NetToolSystem.Mode.Replace)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != NetToolSystem.State.Applying && this.m_ControlPoints.Length >= 1 && !singleFrameOnly)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = NetToolSystem.State.Applying;
          // ISSUE: reference to a compiler-generated field
          ref NativeReference<NetToolSystem.AppliedUpgrade> local = ref this.m_AppliedUpgrade;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.AppliedUpgrade appliedUpgrade1 = new NetToolSystem.AppliedUpgrade();
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.AppliedUpgrade appliedUpgrade2 = appliedUpgrade1;
          local.Value = appliedUpgrade2;
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps, true);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_State = NetToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (this.GetAllowApply() && !this.m_EdgeQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetAppliedUpgrade(false);
          this.applyMode = ApplyMode.Apply;
          // ISSUE: reference to a compiler-generated field
          this.m_RandomSeed = RandomSeed.Next();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetBuildSound);
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradeStates.Clear();
          ControlPoint controlPoint;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint))
          {
            controlPoint.m_Elevation = this.elevation;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, false);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.FixControlPoints(inputDeps);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateCourse(inputDeps, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.Update(inputDeps, true);
        }
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != NetToolSystem.State.Applying && this.m_ControlPoints.Length >= 1 && !singleFrameOnly)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_State = NetToolSystem.State.Applying;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ApplyStartPoint = this.m_LastRaycastPoint;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ApplyStartPoint.m_HitPosition.y -= this.m_ApplyStartPoint.m_Elevation;
        // ISSUE: reference to a compiler-generated field
        this.m_ApplyTimer = 0.0f;
        // ISSUE: reference to a compiler-generated method
        return this.Update(inputDeps, true);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_State = NetToolSystem.State.Default;
      int num1;
      switch (actualMode)
      {
        case NetToolSystem.Mode.Straight:
          num1 = 2;
          break;
        case NetToolSystem.Mode.SimpleCurve:
          num1 = 3;
          break;
        case NetToolSystem.Mode.ComplexCurve:
          num1 = 4;
          break;
        case NetToolSystem.Mode.Continuous:
          num1 = 3;
          break;
        case NetToolSystem.Mode.Grid:
          num1 = 3;
          break;
        default:
          num1 = 1;
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ControlPoints.Length < num1)
      {
        this.applyMode = ApplyMode.Clear;
        ControlPoint controlPoint;
        // ISSUE: reference to a compiler-generated method
        if (this.GetRaycastResult(out controlPoint))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length <= 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetStartSound);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetNodeSound);
          }
          controlPoint.m_HitPosition.y += this.elevation;
          controlPoint.m_Elevation = this.elevation;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.SnapControlPoints(inputDeps, false);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.UpdateCourse(inputDeps, false);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.GetAllowApply() && !(actualMode == NetToolSystem.Mode.Point ? this.m_NodeQuery : this.m_EdgeQuery).IsEmptyIgnoreFilter)
        {
          this.applyMode = ApplyMode.Apply;
          // ISSUE: reference to a compiler-generated field
          this.m_RandomSeed = RandomSeed.Next();
          int num2 = 0;
          switch (actualMode)
          {
            case NetToolSystem.Mode.Continuous:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint1 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Clear();
              float num3 = math.distance(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz);
              controlPoint1.m_OriginalEntity = Entity.Null;
              controlPoint1.m_Direction = controlPoint2.m_Direction;
              controlPoint1.m_Position = controlPoint2.m_Position;
              controlPoint1.m_Position.xz += controlPoint1.m_Direction * num3;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint2);
              int num4 = num2 + 1;
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint1);
              num2 = num4 + 1;
              break;
            case NetToolSystem.Mode.Point:
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Clear();
              break;
            default:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Clear();
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.Add(in controlPoint3);
              ++num2;
              break;
          }
          ControlPoint controlPoint4;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out controlPoint4))
          {
            controlPoint4.m_HitPosition.y += this.elevation;
            controlPoint4.m_Elevation = this.elevation;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint4);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, false);
            ++num2;
          }
          if (num2 >= 1)
          {
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.FixControlPoints(inputDeps);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetBuildSound);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateCourse(inputDeps, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.Update(inputDeps, true);
        }
      }
      return inputDeps;
    }

    private JobHandle Update(JobHandle inputDeps, bool fullUpdate)
    {
      if (this.actualMode == NetToolSystem.Mode.Replace)
      {
        ControlPoint controlPoint1;
        bool forceUpdate;
        // ISSUE: reference to a compiler-generated method
        if (this.GetRaycastResult(out controlPoint1, out forceUpdate))
        {
          controlPoint1.m_Elevation = this.elevation;
          fullUpdate |= forceUpdate;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length == 0)
          {
            this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Add(in controlPoint1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, this.m_State == NetToolSystem.State.Cancelling);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateCourse(inputDeps, this.m_State == NetToolSystem.State.Cancelling);
          }
          else
          {
            this.applyMode = ApplyMode.None;
            // ISSUE: reference to a compiler-generated field
            if (fullUpdate || !this.m_LastRaycastPoint.Equals(controlPoint1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LastRaycastPoint = controlPoint1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_State == NetToolSystem.State.Applying || this.m_State == NetToolSystem.State.Cancelling)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ControlPoints.Length == 1)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ControlPoints.Add(in controlPoint1);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_ControlPoints[this.m_ControlPoints.Length - 1] = controlPoint1;
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Clear();
                // ISSUE: reference to a compiler-generated field
                this.m_UpgradeStates.Clear();
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.Add(in controlPoint1);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapControlPoints(inputDeps, this.m_State == NetToolSystem.State.Cancelling);
              JobHandle.ScheduleBatchedJobs();
              if (!fullUpdate)
              {
                inputDeps.Complete();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
                fullUpdate = !controlPoint2.EqualsIgnoreHit(controlPoint3);
              }
              if (fullUpdate)
              {
                this.applyMode = ApplyMode.Clear;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.UpdateCourse(inputDeps, this.m_State == NetToolSystem.State.Cancelling);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == NetToolSystem.State.Default)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_UpgradeStates.Clear();
            // ISSUE: reference to a compiler-generated field
            ref NativeReference<NetToolSystem.AppliedUpgrade> local = ref this.m_AppliedUpgrade;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.AppliedUpgrade appliedUpgrade1 = new NetToolSystem.AppliedUpgrade();
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.AppliedUpgrade appliedUpgrade2 = appliedUpgrade1;
            local.Value = appliedUpgrade2;
          }
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
        }
        return inputDeps;
      }
      ControlPoint controlPoint4;
      bool forceUpdate1;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint4, out forceUpdate1))
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == NetToolSystem.State.Applying)
        {
          // ISSUE: reference to a compiler-generated field
          controlPoint4 = this.m_ApplyStartPoint;
        }
        controlPoint4.m_HitPosition.y += this.elevation;
        controlPoint4.m_Elevation = this.elevation;
        fullUpdate |= forceUpdate1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length == 0)
        {
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint4);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.SnapControlPoints(inputDeps, false);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.UpdateCourse(inputDeps, false);
        }
        else
        {
          this.applyMode = ApplyMode.None;
          // ISSUE: reference to a compiler-generated field
          if (fullUpdate || !this.m_LastRaycastPoint.Equals(controlPoint4))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 2 && (double) math.distance(this.m_LastRaycastPoint.m_Position, controlPoint4.m_Position) > 0.0099999997764825821)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_AudioManager.PlayUISoundIfNotPlaying(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_NetExpandSound);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_LastRaycastPoint = controlPoint4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint5 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints[this.m_ControlPoints.Length - 1] = controlPoint4;
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps, false);
            JobHandle.ScheduleBatchedJobs();
            if (!fullUpdate)
            {
              inputDeps.Complete();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint6 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
              fullUpdate = !controlPoint5.EqualsIgnoreHit(controlPoint6);
            }
            if (fullUpdate)
            {
              this.applyMode = ApplyMode.Clear;
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateCourse(inputDeps, false);
            }
          }
        }
      }
      else
      {
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      }
      return inputDeps;
    }

    private JobHandle UpdateStartEntity(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_DefinitionQuery.IsEmptyIgnoreFilter)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      return new NetToolSystem.UpdateStartEntityJob()
      {
        m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
        m_StartEntity = this.m_StartEntity
      }.Schedule<NetToolSystem.UpdateStartEntityJob>(this.m_DefinitionQuery, inputDeps);
    }

    private JobHandle SnapControlPoints(JobHandle inputDeps, bool removeUpgrade)
    {
      Entity entity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_LanePrefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        entity = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_LanePrefab);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps1;
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.SnapJob jobData = new NetToolSystem.SnapJob()
      {
        m_Mode = this.actualMode,
        m_Snap = this.GetActualSnap(),
        m_Elevation = this.elevation,
        m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Prefab),
        m_LanePrefab = entity,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_RemoveUpgrade = removeUpgrade,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps1),
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ZoneBlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabRoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_RoadCompositionData = this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup,
        m_PlaceableData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_BuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_AssetStampData = this.__TypeHandle.__Game_Prefabs_AssetStampData_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_LocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubReplacements = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_ZoneCells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_PrefabCompositionAreas = this.__TypeHandle.__Game_Prefabs_NetCompositionArea_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
        m_ZoneSearchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies3),
        m_ControlPoints = this.m_ControlPoints,
        m_SnapLines = this.m_SnapLines,
        m_UpgradeStates = this.m_UpgradeStates,
        m_StartEntity = this.m_StartEntity,
        m_AppliedUpgrade = this.m_AppliedUpgrade,
        m_LastSnappedEntity = this.m_LastSnappedEntity,
        m_LastControlPointsAngle = this.m_LastControlPointsAngle,
        m_SourceUpdateData = this.m_AudioManager.GetSourceUpdateData(out deps2)
      };
      inputDeps = JobHandle.CombineDependencies(inputDeps, dependencies1, dependencies2);
      inputDeps = JobHandle.CombineDependencies(inputDeps, dependencies3, deps1);
      inputDeps = JobHandle.CombineDependencies(inputDeps, deps2);
      JobHandle dependsOn = inputDeps;
      JobHandle jobHandle = jobData.Schedule<NetToolSystem.SnapJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      return jobHandle;
    }

    private JobHandle FixControlPoints(JobHandle inputDeps)
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_TempQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps1 = new NetToolSystem.FixControlPointsJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_Mode = this.mode,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_ControlPoints = this.m_ControlPoints
      }.Schedule<NetToolSystem.FixControlPointsJob>(JobHandle.CombineDependencies(inputDeps, outJobHandle));
      archetypeChunkListAsync.Dispose(inputDeps1);
      return inputDeps1;
    }

    private JobHandle UpdateCourse(JobHandle inputDeps, bool removeUpgrade)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Prefab != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle deps;
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.CreateDefinitionsJob jobData = new NetToolSystem.CreateDefinitionsJob()
        {
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_RemoveUpgrade = removeUpgrade,
          m_LefthandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
          m_Mode = this.actualMode,
          m_ParallelCount = math.select(new int2(this.actualParallelCount, 0), new int2(0, this.actualParallelCount), this.m_CityConfigurationSystem.leftHandTraffic),
          m_ParallelOffset = this.parallelOffset,
          m_RandomSeed = this.m_RandomSeed,
          m_ControlPoints = this.m_ControlPoints,
          m_UpgradeStates = this.m_UpgradeStates,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_ExtensionData = this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_PlaceableData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
          m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
          m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_SubReplacements = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup,
          m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
          m_CachedNodes = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
          m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
          m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_PrefabSubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
          m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
          m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
          m_PrefabSubAreaNodes = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup,
          m_PrefabPlaceholderElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
          m_NetPrefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Prefab),
          m_WaterSurfaceData = this.m_WaterSystem.GetVelocitiesSurfaceData(out deps),
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        };
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_LanePrefab != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          jobData.m_LanePrefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_LanePrefab);
        }
        JobHandle jobHandle = jobData.Schedule<NetToolSystem.CreateDefinitionsJob>(JobHandle.CombineDependencies(inputDeps, deps));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_WaterSystem.AddVelocitySurfaceReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
      }
      return job0;
    }

    public static void CreatePath(
      ControlPoint startPoint,
      ControlPoint endPoint,
      NativeList<NetToolSystem.PathEdge> path,
      NetData prefabNetData,
      PlaceableNetData placeableNetData,
      ref ComponentLookup<Game.Net.Edge> edgeData,
      ref ComponentLookup<Game.Net.Node> nodeData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<NetData> prefabNetDatas,
      ref BufferLookup<ConnectedEdge> connectedEdgeData)
    {
      if ((double) math.distance(startPoint.m_Position, endPoint.m_Position) < (double) placeableNetData.m_SnapDistance * 0.5)
        endPoint = startPoint;
      CompositionFlags.General general = placeableNetData.m_SetUpgradeFlags.m_General | placeableNetData.m_UnsetUpgradeFlags.m_General;
      CompositionFlags.Side side = placeableNetData.m_SetUpgradeFlags.m_Left | placeableNetData.m_SetUpgradeFlags.m_Right | placeableNetData.m_UnsetUpgradeFlags.m_Left | placeableNetData.m_UnsetUpgradeFlags.m_Right;
      if (startPoint.m_OriginalEntity == endPoint.m_OriginalEntity)
      {
        if (edgeData.HasComponent(endPoint.m_OriginalEntity))
        {
          PrefabRef prefabRef = prefabRefData[endPoint.m_OriginalEntity];
          NetData netData = prefabNetDatas[prefabRef.m_Prefab];
          int num = (prefabNetData.m_RequiredLayers & netData.m_RequiredLayers) > Layer.None ? 1 : 0;
          bool flag = num == 0 && (placeableNetData.m_PlacementFlags & Game.Net.PlacementFlags.IsUpgrade) != Game.Net.PlacementFlags.None && (placeableNetData.m_PlacementFlags & Game.Net.PlacementFlags.NodeUpgrade) == Game.Net.PlacementFlags.None && ((netData.m_GeneralFlagMask & general) != (CompositionFlags.General) 0 || (netData.m_SideFlagMask & side) > (CompositionFlags.Side) 0);
          if ((num | (flag ? 1 : 0)) == 0)
            return;
          // ISSUE: object of a compiler-generated type is created
          path.Add(new NetToolSystem.PathEdge()
          {
            m_Entity = endPoint.m_OriginalEntity,
            m_Invert = (double) endPoint.m_CurvePosition < (double) startPoint.m_CurvePosition,
            m_Upgrade = flag
          });
        }
        else
        {
          if (!nodeData.HasComponent(endPoint.m_OriginalEntity))
            return;
          PrefabRef prefabRef = prefabRefData[endPoint.m_OriginalEntity];
          NetData netData = prefabNetDatas[prefabRef.m_Prefab];
          bool flag1 = (prefabNetData.m_RequiredLayers & netData.m_RequiredLayers) > Layer.None;
          if (flag1)
          {
            DynamicBuffer<ConnectedEdge> dynamicBuffer = connectedEdgeData[endPoint.m_OriginalEntity];
            for (int index = 0; index < dynamicBuffer.Length; ++index)
            {
              Entity edge1 = dynamicBuffer[index].m_Edge;
              Game.Net.Edge edge2 = edgeData[edge1];
              if (edge2.m_Start == endPoint.m_OriginalEntity || edge2.m_End == endPoint.m_OriginalEntity)
              {
                flag1 = false;
                break;
              }
            }
          }
          bool flag2 = !flag1 && (placeableNetData.m_PlacementFlags & (Game.Net.PlacementFlags.IsUpgrade | Game.Net.PlacementFlags.NodeUpgrade)) == (Game.Net.PlacementFlags.IsUpgrade | Game.Net.PlacementFlags.NodeUpgrade) && ((netData.m_GeneralFlagMask & general) != (CompositionFlags.General) 0 || (netData.m_SideFlagMask & side) > (CompositionFlags.Side) 0);
          if (!(flag1 | flag2))
            return;
          // ISSUE: object of a compiler-generated type is created
          path.Add(new NetToolSystem.PathEdge()
          {
            m_Entity = endPoint.m_OriginalEntity,
            m_Upgrade = flag2
          });
        }
      }
      else
      {
        NativeMinHeap<NetToolSystem.PathItem> nativeMinHeap = new NativeMinHeap<NetToolSystem.PathItem>(100, Allocator.Temp);
        NativeParallelHashMap<Entity, Entity> nativeParallelHashMap = new NativeParallelHashMap<Entity, Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        Game.Net.Edge componentData;
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.PathItem pathItem1;
        if (edgeData.TryGetComponent(endPoint.m_OriginalEntity, out componentData))
        {
          PrefabRef prefabRef = prefabRefData[endPoint.m_OriginalEntity];
          NetData netData = prefabNetDatas[prefabRef.m_Prefab];
          int num = (prefabNetData.m_RequiredLayers & netData.m_RequiredLayers) > Layer.None ? 1 : 0;
          if ((num | (num != 0 || (placeableNetData.m_PlacementFlags & Game.Net.PlacementFlags.IsUpgrade) == Game.Net.PlacementFlags.None ? (false ? 1 : 0) : ((netData.m_GeneralFlagMask & general) != (CompositionFlags.General) 0 ? (true ? 1 : 0) : ((netData.m_SideFlagMask & side) > (CompositionFlags.Side) 0 ? 1 : 0)))) != 0)
          {
            ref NativeMinHeap<NetToolSystem.PathItem> local1 = ref nativeMinHeap;
            // ISSUE: object of a compiler-generated type is created
            pathItem1 = new NetToolSystem.PathItem();
            // ISSUE: reference to a compiler-generated field
            pathItem1.m_Node = componentData.m_Start;
            // ISSUE: reference to a compiler-generated field
            pathItem1.m_Edge = endPoint.m_OriginalEntity;
            // ISSUE: reference to a compiler-generated field
            pathItem1.m_Cost = 0.0f;
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.PathItem pathItem2 = pathItem1;
            local1.Insert(pathItem2);
            ref NativeMinHeap<NetToolSystem.PathItem> local2 = ref nativeMinHeap;
            // ISSUE: object of a compiler-generated type is created
            pathItem1 = new NetToolSystem.PathItem();
            // ISSUE: reference to a compiler-generated field
            pathItem1.m_Node = componentData.m_End;
            // ISSUE: reference to a compiler-generated field
            pathItem1.m_Edge = endPoint.m_OriginalEntity;
            // ISSUE: reference to a compiler-generated field
            pathItem1.m_Cost = 0.0f;
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.PathItem pathItem3 = pathItem1;
            local2.Insert(pathItem3);
          }
        }
        else if (nodeData.HasComponent(endPoint.m_OriginalEntity))
        {
          // ISSUE: object of a compiler-generated type is created
          nativeMinHeap.Insert(new NetToolSystem.PathItem()
          {
            m_Node = endPoint.m_OriginalEntity,
            m_Edge = Entity.Null,
            m_Cost = 0.0f
          });
        }
        Entity key1 = Entity.Null;
        while (nativeMinHeap.Length != 0)
        {
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.PathItem pathItem4 = nativeMinHeap.Extract();
          // ISSUE: reference to a compiler-generated field
          if (pathItem4.m_Edge == startPoint.m_OriginalEntity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            nativeParallelHashMap[pathItem4.m_Node] = pathItem4.m_Edge;
            // ISSUE: reference to a compiler-generated field
            key1 = pathItem4.m_Node;
            break;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (nativeParallelHashMap.TryAdd(pathItem4.m_Node, pathItem4.m_Edge))
          {
            // ISSUE: reference to a compiler-generated field
            if (pathItem4.m_Node == startPoint.m_OriginalEntity)
            {
              // ISSUE: reference to a compiler-generated field
              key1 = pathItem4.m_Node;
              break;
            }
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> dynamicBuffer = connectedEdgeData[pathItem4.m_Node];
            PrefabRef prefabRef1 = new PrefabRef();
            // ISSUE: reference to a compiler-generated field
            if (pathItem4.m_Edge != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              prefabRef1 = prefabRefData[pathItem4.m_Edge];
            }
            for (int index = 0; index < dynamicBuffer.Length; ++index)
            {
              Entity edge = dynamicBuffer[index].m_Edge;
              // ISSUE: reference to a compiler-generated field
              if (!(edge == pathItem4.m_Edge))
              {
                componentData = edgeData[edge];
                Entity key2;
                // ISSUE: reference to a compiler-generated field
                if (componentData.m_Start == pathItem4.m_Node)
                {
                  key2 = componentData.m_End;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (componentData.m_End == pathItem4.m_Node)
                    key2 = componentData.m_Start;
                  else
                    continue;
                }
                if (!nativeParallelHashMap.ContainsKey(key2) || !(edge != startPoint.m_OriginalEntity))
                {
                  PrefabRef prefabRef2 = prefabRefData[edge];
                  NetData netData = prefabNetDatas[prefabRef2.m_Prefab];
                  int num1 = (prefabNetData.m_RequiredLayers & netData.m_RequiredLayers) > Layer.None ? 1 : 0;
                  bool flag = num1 == 0 && (placeableNetData.m_PlacementFlags & Game.Net.PlacementFlags.IsUpgrade) != Game.Net.PlacementFlags.None && ((netData.m_GeneralFlagMask & general) != (CompositionFlags.General) 0 || (netData.m_SideFlagMask & side) > (CompositionFlags.Side) 0);
                  if (num1 != 0 || flag)
                  {
                    Curve curve = curveData[edge];
                    // ISSUE: reference to a compiler-generated field
                    float num2 = pathItem4.m_Cost + curve.m_Length + math.select(0.0f, 9.9f, prefabRef2.m_Prefab != prefabRef1.m_Prefab) + math.select(0.0f, 10f, dynamicBuffer.Length > 2);
                    ref NativeMinHeap<NetToolSystem.PathItem> local = ref nativeMinHeap;
                    // ISSUE: object of a compiler-generated type is created
                    pathItem1 = new NetToolSystem.PathItem();
                    // ISSUE: reference to a compiler-generated field
                    pathItem1.m_Node = key2;
                    // ISSUE: reference to a compiler-generated field
                    pathItem1.m_Edge = edge;
                    // ISSUE: reference to a compiler-generated field
                    pathItem1.m_Cost = num2;
                    // ISSUE: variable of a compiler-generated type
                    NetToolSystem.PathItem pathItem5 = pathItem1;
                    local.Insert(pathItem5);
                  }
                }
              }
            }
          }
        }
        Entity entity1;
        Entity entity2;
        for (; nativeParallelHashMap.TryGetValue(key1, out entity1) && !(entity1 == Entity.Null); key1 = entity2)
        {
          componentData = edgeData[entity1];
          PrefabRef prefabRef = prefabRefData[entity1];
          NetData netData = prefabNetDatas[prefabRef.m_Prefab];
          bool flag3 = componentData.m_End == key1;
          bool flag4 = (prefabNetData.m_RequiredLayers & netData.m_RequiredLayers) > Layer.None;
          entity2 = flag3 ? componentData.m_Start : componentData.m_End;
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.PathEdge pathEdge;
          if (flag4 || (placeableNetData.m_PlacementFlags & Game.Net.PlacementFlags.NodeUpgrade) == Game.Net.PlacementFlags.None)
          {
            ref NativeList<NetToolSystem.PathEdge> local3 = ref path;
            // ISSUE: object of a compiler-generated type is created
            pathEdge = new NetToolSystem.PathEdge();
            // ISSUE: reference to a compiler-generated field
            pathEdge.m_Entity = entity1;
            // ISSUE: reference to a compiler-generated field
            pathEdge.m_Invert = flag3;
            // ISSUE: reference to a compiler-generated field
            pathEdge.m_Upgrade = !flag4;
            ref NetToolSystem.PathEdge local4 = ref pathEdge;
            local3.Add(in local4);
          }
          else
          {
            if (key1 == startPoint.m_OriginalEntity)
            {
              ref NativeList<NetToolSystem.PathEdge> local5 = ref path;
              // ISSUE: object of a compiler-generated type is created
              pathEdge = new NetToolSystem.PathEdge();
              // ISSUE: reference to a compiler-generated field
              pathEdge.m_Entity = key1;
              // ISSUE: reference to a compiler-generated field
              pathEdge.m_Upgrade = true;
              ref NetToolSystem.PathEdge local6 = ref pathEdge;
              local5.Add(in local6);
            }
            if (entity1 != endPoint.m_OriginalEntity)
            {
              ref NativeList<NetToolSystem.PathEdge> local7 = ref path;
              // ISSUE: object of a compiler-generated type is created
              pathEdge = new NetToolSystem.PathEdge();
              // ISSUE: reference to a compiler-generated field
              pathEdge.m_Entity = entity2;
              // ISSUE: reference to a compiler-generated field
              pathEdge.m_Upgrade = true;
              ref NetToolSystem.PathEdge local8 = ref pathEdge;
              local7.Add(in local8);
            }
          }
          if (entity1 == endPoint.m_OriginalEntity)
            break;
        }
      }
    }

    private static bool IsNearEnd(
      Entity edge,
      Curve curve,
      float3 position,
      bool invert,
      ref ComponentLookup<EdgeGeometry> edgeGeometryData)
    {
      EdgeGeometry componentData;
      if (edgeGeometryData.TryGetComponent(edge, out componentData))
      {
        Bezier4x3 bezier4x3_1 = MathUtils.Lerp(componentData.m_Start.m_Left, componentData.m_Start.m_Right, 0.5f);
        Bezier4x3 bezier4x3_2 = MathUtils.Lerp(componentData.m_End.m_Left, componentData.m_End.m_Right, 0.5f);
        float t1;
        float num1 = MathUtils.Distance(bezier4x3_1.xz, position.xz, out t1);
        float t2;
        float num2 = MathUtils.Distance(bezier4x3_2.xz, position.xz, out t2);
        float middleLength1 = componentData.m_Start.middleLength;
        float middleLength2 = componentData.m_End.middleLength;
        return (double) math.select(t1 * middleLength1, middleLength1 + t2 * middleLength2, (double) num2 < (double) num1) > ((double) middleLength1 + (double) middleLength2) * 0.5 != invert;
      }
      float t;
      double num = (double) MathUtils.Distance(curve.m_Bezier.xz, position.xz, out t);
      return (double) t > 0.5;
    }

    public static void AddControlPoints(
      NativeList<ControlPoint> controlPoints,
      NativeList<NetToolSystem.UpgradeState> upgradeStates,
      NativeReference<NetToolSystem.AppliedUpgrade> appliedUpgrade,
      ControlPoint startPoint,
      ControlPoint endPoint,
      NativeList<NetToolSystem.PathEdge> path,
      Snap snap,
      bool removeUpgrade,
      bool leftHandTraffic,
      NetGeometryData prefabGeometryData,
      RoadData prefabRoadData,
      PlaceableNetData placeableNetData,
      SubReplacement subReplacement,
      ref ComponentLookup<Game.Net.Edge> edgeData,
      ref ComponentLookup<Game.Net.Node> nodeData,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Composition> compositionData,
      ref ComponentLookup<Upgraded> upgradedData,
      ref ComponentLookup<EdgeGeometry> edgeGeometryData,
      ref ComponentLookup<PrefabRef> prefabRefData,
      ref ComponentLookup<NetData> prefabNetData,
      ref ComponentLookup<NetCompositionData> prefabCompositionData,
      ref ComponentLookup<RoadComposition> prefabRoadCompositionData,
      ref BufferLookup<ConnectedEdge> connectedEdgeData,
      ref BufferLookup<SubReplacement> subReplacementData)
    {
      controlPoints.Add(in startPoint);
      float x1 = 0.0f;
      float num1 = 0.0f;
      bool flag1 = false;
      CompositionFlags.General general1 = placeableNetData.m_SetUpgradeFlags.m_General | placeableNetData.m_UnsetUpgradeFlags.m_General;
      CompositionFlags.Side side1 = placeableNetData.m_SetUpgradeFlags.m_Left | placeableNetData.m_SetUpgradeFlags.m_Right | placeableNetData.m_UnsetUpgradeFlags.m_Left | placeableNetData.m_UnsetUpgradeFlags.m_Right;
      if (path.Length != 0)
      {
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.PathEdge pathEdge = path[path.Length - 1];
        // ISSUE: reference to a compiler-generated field
        if (edgeData.HasComponent(pathEdge.m_Entity))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = prefabRefData[pathEdge.m_Entity];
          NetData netData = prefabNetData[prefabRef.m_Prefab];
          bool flag2 = (netData.m_GeneralFlagMask & general1) > (CompositionFlags.General) 0;
          bool flag3 = (netData.m_SideFlagMask & side1) > (CompositionFlags.Side) 0;
          // ISSUE: reference to a compiler-generated field
          if (pathEdge.m_Upgrade && !flag3)
          {
            flag1 = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Composition composition = compositionData[pathEdge.m_Entity];
            // ISSUE: reference to a compiler-generated field
            Curve curve = curveData[pathEdge.m_Entity];
            NetCompositionData netCompositionData = prefabCompositionData[composition.m_Edge];
            num1 = netCompositionData.m_Width * 0.5f;
            float t;
            double num2 = (double) MathUtils.Distance(curve.m_Bezier.xz, endPoint.m_HitPosition.xz, out t);
            float3 float3_1 = MathUtils.Position(curve.m_Bezier, t);
            float3 float3_2 = MathUtils.Tangent(curve.m_Bezier, t);
            float3 float3_3 = MathUtils.Normalize(float3_2, float3_2.xz);
            float a = math.dot(endPoint.m_HitPosition.xz - float3_1.xz, MathUtils.Right(float3_3.xz));
            // ISSUE: reference to a compiler-generated field
            x1 = math.select(a, -a, pathEdge.m_Invert);
            flag1 = flag2 && (double) math.abs(x1) <= (double) netCompositionData.m_Width * 0.1666666716337204;
          }
        }
      }
      for (int index1 = 0; index1 < path.Length; ++index1)
      {
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.PathEdge pathEdge1 = path[index1];
        Game.Net.Edge componentData1;
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.UpgradeState upgradeState1;
        // ISSUE: reference to a compiler-generated field
        if (edgeData.TryGetComponent(pathEdge1.m_Entity, out componentData1))
        {
          // ISSUE: reference to a compiler-generated field
          Curve curve = curveData[pathEdge1.m_Entity];
          // ISSUE: reference to a compiler-generated field
          if (pathEdge1.m_Invert)
          {
            CommonUtils.Swap<Entity>(ref componentData1.m_Start, ref componentData1.m_End);
            curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
          }
          float x2 = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (pathEdge1.m_Upgrade)
          {
            // ISSUE: object of a compiler-generated type is created
            upgradeState1 = new NetToolSystem.UpgradeState();
            // ISSUE: reference to a compiler-generated field
            upgradeState1.m_IsUpgrading = true;
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.UpgradeState upgradeState2 = upgradeState1;
            Upgraded componentData2;
            // ISSUE: reference to a compiler-generated field
            if (upgradedData.TryGetComponent(pathEdge1.m_Entity, out componentData2))
            {
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_OldFlags = componentData2.m_Flags;
            }
            Composition componentData3;
            // ISSUE: reference to a compiler-generated field
            if (compositionData.TryGetComponent(pathEdge1.m_Entity, out componentData3))
            {
              NetCompositionData componentData4;
              if (prefabCompositionData.TryGetComponent(componentData3.m_StartNode, out componentData4))
              {
                if ((componentData4.m_Flags.m_General & CompositionFlags.General.Crosswalk) != (CompositionFlags.General) 0)
                {
                  if ((componentData4.m_Flags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    upgradeState2.m_OldFlags.m_Left |= CompositionFlags.Side.AddCrosswalk;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    upgradeState2.m_OldFlags.m_Right |= CompositionFlags.Side.AddCrosswalk;
                  }
                }
                else if ((componentData4.m_Flags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  upgradeState2.m_OldFlags.m_Left |= CompositionFlags.Side.RemoveCrosswalk;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  upgradeState2.m_OldFlags.m_Right |= CompositionFlags.Side.RemoveCrosswalk;
                }
              }
              NetCompositionData componentData5;
              if (prefabCompositionData.TryGetComponent(componentData3.m_EndNode, out componentData5))
              {
                if ((componentData5.m_Flags.m_General & CompositionFlags.General.Crosswalk) != (CompositionFlags.General) 0)
                {
                  if ((componentData5.m_Flags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    upgradeState2.m_OldFlags.m_Left |= CompositionFlags.Side.AddCrosswalk;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    upgradeState2.m_OldFlags.m_Right |= CompositionFlags.Side.AddCrosswalk;
                  }
                }
                else if ((componentData5.m_Flags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  upgradeState2.m_OldFlags.m_Left |= CompositionFlags.Side.RemoveCrosswalk;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  upgradeState2.m_OldFlags.m_Right |= CompositionFlags.Side.RemoveCrosswalk;
                }
              }
            }
            CompositionFlags compositionFlags1;
            CompositionFlags compositionFlags2;
            // ISSUE: reference to a compiler-generated field
            if ((double) x1 < 0.0 != pathEdge1.m_Invert)
            {
              compositionFlags1 = NetCompositionHelpers.InvertCompositionFlags(placeableNetData.m_SetUpgradeFlags);
              compositionFlags2 = NetCompositionHelpers.InvertCompositionFlags(placeableNetData.m_UnsetUpgradeFlags);
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_SubReplacementSide = SubReplacementSide.Left;
            }
            else
            {
              compositionFlags1 = placeableNetData.m_SetUpgradeFlags;
              compositionFlags2 = placeableNetData.m_UnsetUpgradeFlags;
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_SubReplacementSide = SubReplacementSide.Right;
            }
            CompositionFlags.Side side2 = CompositionFlags.Side.ForbidLeftTurn | CompositionFlags.Side.ForbidRightTurn | CompositionFlags.Side.AddCrosswalk | CompositionFlags.Side.RemoveCrosswalk | CompositionFlags.Side.ForbidStraight;
            CompositionFlags.Side side3 = (compositionFlags1.m_Left | compositionFlags1.m_Right) & side2;
            CompositionFlags.Side side4 = (compositionFlags2.m_Left | compositionFlags2.m_Right) & side2;
            if ((side3 | side4) != (CompositionFlags.Side) 0)
            {
              bool2 bool2 = (bool2) false;
              if (index1 > 0 & index1 < path.Length - 1)
              {
                bool2 = (bool2) true;
              }
              else
              {
                if (index1 == 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  bool y = NetToolSystem.IsNearEnd(pathEdge1.m_Entity, curve, startPoint.m_HitPosition, pathEdge1.m_Invert, ref edgeGeometryData);
                  bool2 |= new bool2(!y, y);
                  if (index1 + 1 < path.Length)
                  {
                    // ISSUE: variable of a compiler-generated type
                    NetToolSystem.PathEdge pathEdge2 = path[index1 + 1];
                    Game.Net.Edge componentData6;
                    // ISSUE: reference to a compiler-generated field
                    if (edgeData.TryGetComponent(pathEdge2.m_Entity, out componentData6))
                      bool2 |= new bool2(componentData1.m_Start == componentData6.m_Start | componentData1.m_Start == componentData6.m_End, componentData1.m_End == componentData6.m_Start | componentData1.m_End == componentData6.m_End);
                  }
                }
                if (index1 == path.Length - 1)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  bool y = NetToolSystem.IsNearEnd(pathEdge1.m_Entity, curve, endPoint.m_HitPosition, pathEdge1.m_Invert, ref edgeGeometryData);
                  bool2 |= new bool2(!y, y);
                  if (index1 - 1 >= 0)
                  {
                    // ISSUE: variable of a compiler-generated type
                    NetToolSystem.PathEdge pathEdge3 = path[index1 - 1];
                    Game.Net.Edge componentData7;
                    // ISSUE: reference to a compiler-generated field
                    if (edgeData.TryGetComponent(pathEdge3.m_Entity, out componentData7))
                      bool2 |= new bool2(componentData1.m_Start == componentData7.m_Start | componentData1.m_Start == componentData7.m_End, componentData1.m_End == componentData7.m_Start | componentData1.m_End == componentData7.m_End);
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (pathEdge1.m_Invert != leftHandTraffic)
                bool2 = bool2.yx;
              if (bool2.x)
              {
                compositionFlags1.m_Left |= side3;
                compositionFlags2.m_Left |= side4;
              }
              else
              {
                compositionFlags1.m_Left &= ~side3;
                compositionFlags2.m_Left &= ~side4;
              }
              if (bool2.y)
              {
                compositionFlags1.m_Right |= side3;
                compositionFlags2.m_Right |= side4;
              }
              else
              {
                compositionFlags1.m_Right &= ~side3;
                compositionFlags2.m_Right &= ~side4;
              }
            }
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = prefabRefData[pathEdge1.m_Entity];
            NetData netData = prefabNetData[prefabRef.m_Prefab];
            bool flag4 = (netData.m_GeneralFlagMask & general1) > (CompositionFlags.General) 0;
            bool flag5 = (netData.m_SideFlagMask & side1) > (CompositionFlags.Side) 0;
            if (flag1 || !flag5)
            {
              CompositionFlags.Side side5 = ~(CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk);
              compositionFlags1.m_Left &= side5;
              compositionFlags1.m_Right &= side5;
              compositionFlags2.m_Left &= side5;
              compositionFlags2.m_Right &= side5;
            }
            if (!flag1 || !flag4)
            {
              CompositionFlags.General general2 = ~(CompositionFlags.General.WideMedian | CompositionFlags.General.PrimaryMiddleBeautification | CompositionFlags.General.SecondaryMiddleBeautification);
              compositionFlags1.m_General &= general2;
              compositionFlags2.m_General &= general2;
            }
            if (flag1 & flag4)
            {
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_SubReplacementSide = SubReplacementSide.Middle;
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_SubReplacementType = subReplacement.m_Type;
            }
            else if (!flag1 & flag5)
            {
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_SubReplacementType = subReplacement.m_Type;
            }
            // ISSUE: reference to a compiler-generated field
            if (upgradeState2.m_SubReplacementType != SubReplacementType.None)
            {
              if (!removeUpgrade)
              {
                // ISSUE: reference to a compiler-generated field
                upgradeState2.m_SubReplacementPrefab = subReplacement.m_Prefab;
              }
              bool flag6 = false;
              bool flag7 = subReplacement.m_Prefab != Entity.Null;
              DynamicBuffer<SubReplacement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (subReplacementData.TryGetBuffer(pathEdge1.m_Entity, out bufferData))
              {
                for (int index2 = 0; index2 < bufferData.Length; ++index2)
                {
                  SubReplacement subReplacement1 = bufferData[index2];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (subReplacement1.m_Side == upgradeState2.m_SubReplacementSide && subReplacement1.m_Type == upgradeState2.m_SubReplacementType)
                  {
                    flag6 = true;
                    flag7 = subReplacement1.m_Prefab != subReplacement.m_Prefab;
                    break;
                  }
                }
              }
              if ((removeUpgrade ? (flag6 ? 1 : 0) : (flag7 ? 1 : 0)) == 0)
              {
                // ISSUE: reference to a compiler-generated field
                upgradeState2.m_SubReplacementType = SubReplacementType.None;
              }
            }
            if (removeUpgrade)
            {
              compositionFlags2.m_General = (CompositionFlags.General) 0;
              compositionFlags2.m_Left &= CompositionFlags.Side.RemoveCrosswalk;
              compositionFlags2.m_Right &= CompositionFlags.Side.RemoveCrosswalk;
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_AddFlags = compositionFlags2;
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_RemoveFlags = compositionFlags1;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_AddFlags = compositionFlags1;
              // ISSUE: reference to a compiler-generated field
              upgradeState2.m_RemoveFlags = compositionFlags2;
            }
            upgradeStates.Add(in upgradeState2);
          }
          else
          {
            ref NativeList<NetToolSystem.UpgradeState> local1 = ref upgradeStates;
            // ISSUE: object of a compiler-generated type is created
            upgradeState1 = new NetToolSystem.UpgradeState();
            ref NetToolSystem.UpgradeState local2 = ref upgradeState1;
            local1.Add(in local2);
            if ((prefabGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) == (Game.Net.GeometryFlags) 0)
            {
              x2 = x1;
              if ((snap & Snap.ExistingGeometry) != Snap.None)
              {
                // ISSUE: reference to a compiler-generated field
                Composition composition = compositionData[pathEdge1.m_Entity];
                NetCompositionData netCompositionData = prefabCompositionData[composition.m_Edge];
                RoadComposition componentData8;
                prefabRoadCompositionData.TryGetComponent(composition.m_Edge, out componentData8);
                float num3 = math.abs(netCompositionData.m_Width - prefabGeometryData.m_DefaultWidth);
                if ((snap & Snap.CellLength) != Snap.None && (componentData8.m_Flags & prefabRoadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0)
                {
                  int cellWidth1 = ZoneUtils.GetCellWidth(netCompositionData.m_Width);
                  int cellWidth2 = ZoneUtils.GetCellWidth(prefabGeometryData.m_DefaultWidth);
                  float offset = math.select(0.0f, 4f, ((cellWidth1 ^ cellWidth2) & 1) != 0);
                  float num4 = (float) math.abs(cellWidth1 - cellWidth2) * 8f;
                  x2 = math.clamp(MathUtils.Snap(x2 * ((float) ((double) num4 * 0.5 + 3.9200000762939453) / num1), 8f, offset), num4 * -0.5f, num4 * 0.5f);
                }
                else
                  x2 = (double) num3 <= 1.6000000238418579 ? 0.0f : math.clamp(MathUtils.Snap(x2 * (num3 * 0.74f / num1), num3 * 0.5f), num3 * -0.5f, num3 * 0.5f);
              }
            }
          }
          ControlPoint controlPoint1 = endPoint with
          {
            m_OriginalEntity = componentData1.m_Start,
            m_Position = curve.m_Bezier.a
          };
          ControlPoint controlPoint2 = endPoint with
          {
            m_OriginalEntity = componentData1.m_End,
            m_Position = curve.m_Bezier.d
          };
          if ((double) math.abs(x2) >= 0.0099999997764825821)
          {
            float3 float3_4 = MathUtils.StartTangent(curve.m_Bezier);
            float3 float3_5 = MathUtils.EndTangent(curve.m_Bezier);
            float3_4 = MathUtils.Normalize(float3_4, float3_4.xz);
            float3 float3_6 = MathUtils.Normalize(float3_5, float3_5.xz);
            controlPoint1.m_Position.xz += MathUtils.Right(float3_4.xz) * x2;
            controlPoint2.m_Position.xz += MathUtils.Right(float3_6.xz) * x2;
          }
          controlPoints.Add(in controlPoint1);
          controlPoints.Add(in controlPoint2);
        }
        else
        {
          Game.Net.Node componentData9;
          // ISSUE: reference to a compiler-generated field
          if (nodeData.TryGetComponent(pathEdge1.m_Entity, out componentData9))
          {
            // ISSUE: reference to a compiler-generated field
            if (pathEdge1.m_Upgrade)
            {
              // ISSUE: object of a compiler-generated type is created
              upgradeState1 = new NetToolSystem.UpgradeState();
              // ISSUE: reference to a compiler-generated field
              upgradeState1.m_IsUpgrading = true;
              // ISSUE: variable of a compiler-generated type
              NetToolSystem.UpgradeState upgradeState3 = upgradeState1;
              Upgraded componentData10;
              // ISSUE: reference to a compiler-generated field
              if (upgradedData.TryGetComponent(pathEdge1.m_Entity, out componentData10))
              {
                // ISSUE: reference to a compiler-generated field
                upgradeState3.m_OldFlags = componentData10.m_Flags;
              }
              DynamicBuffer<ConnectedEdge> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (connectedEdgeData.TryGetBuffer(pathEdge1.m_Entity, out bufferData))
              {
                CompositionFlags compositionFlags = new CompositionFlags();
                for (int index3 = 0; index3 < bufferData.Length; ++index3)
                {
                  Entity edge = bufferData[index3].m_Edge;
                  componentData1 = edgeData[edge];
                  // ISSUE: reference to a compiler-generated field
                  if (componentData1.m_Start == pathEdge1.m_Entity)
                  {
                    Composition componentData11;
                    NetCompositionData componentData12;
                    if (compositionData.TryGetComponent(edge, out componentData11) && prefabCompositionData.TryGetComponent(componentData11.m_StartNode, out componentData12))
                      compositionFlags |= componentData12.m_Flags;
                  }
                  else
                  {
                    Composition componentData13;
                    NetCompositionData componentData14;
                    // ISSUE: reference to a compiler-generated field
                    if (componentData1.m_End == pathEdge1.m_Entity && compositionData.TryGetComponent(edge, out componentData13) && prefabCompositionData.TryGetComponent(componentData13.m_EndNode, out componentData14))
                      compositionFlags |= componentData14.m_Flags;
                  }
                }
                if ((compositionFlags.m_General & CompositionFlags.General.TrafficLights) != (CompositionFlags.General) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  upgradeState3.m_OldFlags.m_General |= CompositionFlags.General.TrafficLights;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  upgradeState3.m_OldFlags.m_General |= CompositionFlags.General.RemoveTrafficLights;
                }
              }
              CompositionFlags setUpgradeFlags = placeableNetData.m_SetUpgradeFlags;
              CompositionFlags unsetUpgradeFlags = placeableNetData.m_UnsetUpgradeFlags;
              if (removeUpgrade)
              {
                unsetUpgradeFlags.m_General &= CompositionFlags.General.RemoveTrafficLights;
                unsetUpgradeFlags.m_Left = (CompositionFlags.Side) 0;
                unsetUpgradeFlags.m_Right = (CompositionFlags.Side) 0;
                // ISSUE: reference to a compiler-generated field
                upgradeState3.m_AddFlags = unsetUpgradeFlags;
                // ISSUE: reference to a compiler-generated field
                upgradeState3.m_RemoveFlags = setUpgradeFlags;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                upgradeState3.m_AddFlags = setUpgradeFlags;
                // ISSUE: reference to a compiler-generated field
                upgradeState3.m_RemoveFlags = unsetUpgradeFlags;
              }
              upgradeStates.Add(in upgradeState3);
            }
            else
            {
              ref NativeList<NetToolSystem.UpgradeState> local3 = ref upgradeStates;
              // ISSUE: object of a compiler-generated type is created
              upgradeState1 = new NetToolSystem.UpgradeState();
              ref NetToolSystem.UpgradeState local4 = ref upgradeState1;
              local3.Add(in local4);
            }
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint = endPoint with
            {
              m_OriginalEntity = pathEdge1.m_Entity,
              m_Position = componentData9.m_Position
            };
            controlPoints.Add(in controlPoint);
            controlPoints.Add(in controlPoint);
          }
        }
      }
      controlPoints.Add(in endPoint);
      // ISSUE: variable of a compiler-generated type
      NetToolSystem.AppliedUpgrade appliedUpgrade1 = appliedUpgrade.Value;
      // ISSUE: reference to a compiler-generated field
      if (!(appliedUpgrade1.m_Entity != Entity.Null))
        return;
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
      if (upgradeStates.Length != 1 || path[path.Length - 1].m_Entity != appliedUpgrade1.m_Entity || upgradeStates[0].m_AddFlags != appliedUpgrade1.m_Flags || upgradeStates[0].m_SubReplacementSide != appliedUpgrade1.m_SubReplacementSide || subReplacement.m_Type != appliedUpgrade1.m_SubReplacementType && appliedUpgrade1.m_SubReplacementType != SubReplacementType.None || subReplacement.m_Prefab != appliedUpgrade1.m_SubReplacementPrefab && appliedUpgrade1.m_SubReplacementPrefab != Entity.Null)
      {
        ref NativeReference<NetToolSystem.AppliedUpgrade> local = ref appliedUpgrade;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.AppliedUpgrade appliedUpgrade2 = new NetToolSystem.AppliedUpgrade();
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.AppliedUpgrade appliedUpgrade3 = appliedUpgrade2;
        local.Value = appliedUpgrade3;
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.UpgradeState upgradeState = upgradeStates[0] with
        {
          m_SkipFlags = true
        };
        upgradeStates[0] = upgradeState;
      }
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
    public NetToolSystem()
    {
    }

    public enum Mode
    {
      Straight,
      SimpleCurve,
      ComplexCurve,
      Continuous,
      Grid,
      Replace,
      Point,
    }

    private class NetToolPreferences
    {
      public NetToolSystem.Mode m_Mode;
      public Snap m_Snap;
      public float m_Elevation;
      public float m_ElevationStep;
      public int m_ParallelCount;
      public float m_ParallelOffset;
      public bool m_Underground;

      public void Save(NetToolSystem netTool)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mode = netTool.mode;
        // ISSUE: reference to a compiler-generated field
        this.m_Snap = netTool.selectedSnap;
        // ISSUE: reference to a compiler-generated field
        this.m_Elevation = netTool.elevation;
        // ISSUE: reference to a compiler-generated field
        this.m_ElevationStep = netTool.elevationStep;
        // ISSUE: reference to a compiler-generated field
        this.m_ParallelCount = netTool.parallelCount;
        // ISSUE: reference to a compiler-generated field
        this.m_ParallelOffset = netTool.parallelOffset;
        // ISSUE: reference to a compiler-generated field
        this.m_Underground = netTool.underground;
      }

      public void Load(NetToolSystem netTool)
      {
        // ISSUE: reference to a compiler-generated field
        netTool.mode = this.m_Mode;
        // ISSUE: reference to a compiler-generated field
        netTool.selectedSnap = this.m_Snap;
        // ISSUE: reference to a compiler-generated field
        netTool.elevation = this.m_Elevation;
        // ISSUE: reference to a compiler-generated field
        netTool.elevationStep = this.m_ElevationStep;
        // ISSUE: reference to a compiler-generated field
        netTool.parallelCount = this.m_ParallelCount;
        // ISSUE: reference to a compiler-generated field
        netTool.parallelOffset = this.m_ParallelOffset;
        // ISSUE: reference to a compiler-generated field
        netTool.underground = this.m_Underground;
      }
    }

    private enum State
    {
      Default,
      Applying,
      Cancelling,
    }

    public struct UpgradeState
    {
      public bool m_IsUpgrading;
      public bool m_SkipFlags;
      public SubReplacementSide m_SubReplacementSide;
      public SubReplacementType m_SubReplacementType;
      public CompositionFlags m_OldFlags;
      public CompositionFlags m_AddFlags;
      public CompositionFlags m_RemoveFlags;
      public Entity m_SubReplacementPrefab;
    }

    public struct PathEdge
    {
      public Entity m_Entity;
      public bool m_Invert;
      public bool m_Upgrade;
    }

    public struct PathItem : ILessThan<NetToolSystem.PathItem>
    {
      public Entity m_Node;
      public Entity m_Edge;
      public float m_Cost;

      public bool LessThan(NetToolSystem.PathItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (double) this.m_Cost < (double) other.m_Cost;
      }
    }

    [BurstCompile]
    private struct UpdateStartEntityJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> m_NetCourseType;
      public NativeReference<Entity> m_StartEntity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCourse> nativeArray = chunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          NetCourse netCourse = nativeArray[index];
          if ((netCourse.m_StartPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsParallel)) == CoursePosFlags.IsFirst)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_StartEntity.Value = netCourse.m_StartPosition.m_Entity;
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

    public struct AppliedUpgrade
    {
      public Entity m_Entity;
      public Entity m_SubReplacementPrefab;
      public CompositionFlags m_Flags;
      public SubReplacementType m_SubReplacementType;
      public SubReplacementSide m_SubReplacementSide;
    }

    [BurstCompile]
    private struct SnapJob : IJob
    {
      [ReadOnly]
      public NetToolSystem.Mode m_Mode;
      [ReadOnly]
      public Snap m_Snap;
      [ReadOnly]
      public float m_Elevation;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public Entity m_LanePrefab;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_RemoveUpgrade;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_ZoneBlockData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_PrefabRoadData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<RoadComposition> m_RoadCompositionData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PlaceableData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_BuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<AssetStampData> m_AssetStampData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<SubReplacement> m_SubReplacements;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<Cell> m_ZoneCells;
      [ReadOnly]
      public BufferLookup<NetCompositionArea> m_PrefabCompositionAreas;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_SubObjects;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_ZoneSearchTree;
      public NativeList<ControlPoint> m_ControlPoints;
      public NativeList<SnapLine> m_SnapLines;
      public NativeList<NetToolSystem.UpgradeState> m_UpgradeStates;
      public NativeReference<Entity> m_StartEntity;
      public NativeReference<Entity> m_LastSnappedEntity;
      public NativeReference<int> m_LastControlPointsAngle;
      public NativeReference<NetToolSystem.AppliedUpgrade> m_AppliedUpgrade;
      public SourceUpdateData m_SourceUpdateData;

      public void Execute()
      {
        RoadData prefabRoadData = new RoadData();
        NetGeometryData netGeometryData = new NetGeometryData();
        LocalConnectData localConnectData = new LocalConnectData();
        PlaceableNetData placeableNetData = new PlaceableNetData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetData prefabNetData = this.m_PrefabNetData[this.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRoadData.HasComponent(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          prefabRoadData = this.m_PrefabRoadData[this.m_Prefab];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabGeometryData.HasComponent(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_PrefabGeometryData[this.m_Prefab];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_LocalConnectData.HasComponent(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          localConnectData = this.m_LocalConnectData[this.m_Prefab];
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceableData.HasComponent(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          placeableNetData = this.m_PlaceableData[this.m_Prefab];
        }
        placeableNetData.m_SnapDistance = math.max(placeableNetData.m_SnapDistance, 1f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_LanePrefab != Entity.Null)
          netGeometryData.m_Flags |= Game.Net.GeometryFlags.StrictNodes;
        // ISSUE: reference to a compiler-generated field
        this.m_SnapLines.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradeStates.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace || this.m_ControlPoints.Length <= 1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StartEntity.Value = new Entity();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
          SubReplacement subReplacement = new SubReplacement();
          if ((placeableNetData.m_SetUpgradeFlags.m_General & CompositionFlags.General.SecondaryMiddleBeautification) != (CompositionFlags.General) 0 || (placeableNetData.m_SetUpgradeFlags.m_Left & CompositionFlags.Side.SecondaryBeautification) != (CompositionFlags.Side) 0 || (placeableNetData.m_SetUpgradeFlags.m_Right & CompositionFlags.Side.SecondaryBeautification) != (CompositionFlags.Side) 0)
            subReplacement.m_Type = SubReplacementType.Tree;
          NativeList<NetToolSystem.PathEdge> path = new NativeList<NetToolSystem.PathEdge>((AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NetToolSystem.CreatePath(controlPoint1, controlPoint2, path, prefabNetData, placeableNetData, ref this.m_EdgeData, ref this.m_NodeData, ref this.m_CurveData, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_ConnectedEdges);
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
          NetToolSystem.AddControlPoints(this.m_ControlPoints, this.m_UpgradeStates, this.m_AppliedUpgrade, controlPoint1, controlPoint2, path, this.m_Snap, this.m_RemoveUpgrade, this.m_LeftHandTraffic, netGeometryData, prefabRoadData, placeableNetData, subReplacement, ref this.m_EdgeData, ref this.m_NodeData, ref this.m_CurveData, ref this.m_CompositionData, ref this.m_UpgradedData, ref this.m_EdgeGeometryData, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabCompositionData, ref this.m_RoadCompositionData, ref this.m_ConnectedEdges, ref this.m_SubReplacements);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
          ControlPoint bestSnapPosition = controlPoint;
          bestSnapPosition.m_Position = bestSnapPosition.m_HitPosition;
          bestSnapPosition.m_OriginalEntity = Entity.Null;
          bestSnapPosition.m_ElementIndex = (int2) -1;
          // ISSUE: reference to a compiler-generated method
          this.HandleWorldSize(ref bestSnapPosition, controlPoint, netGeometryData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.ObjectSurface) != Snap.None && this.m_TransformData.HasComponent(controlPoint.m_OriginalEntity) && this.m_SubNets.HasBuffer(controlPoint.m_OriginalEntity))
          {
            bestSnapPosition.m_OriginalEntity = controlPoint.m_OriginalEntity;
            bestSnapPosition.m_ElementIndex = controlPoint.m_ElementIndex;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & (Snap.CellLength | Snap.StraightDirection)) != Snap.None && this.m_ControlPoints.Length >= 2)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandleControlPoints(ref bestSnapPosition, controlPoint, netGeometryData, placeableNetData);
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & (Snap.ExistingGeometry | Snap.NearbyGeometry | Snap.GuideLines)) != Snap.None)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandleExistingGeometry(ref bestSnapPosition, controlPoint, prefabRoadData, netGeometryData, prefabNetData, localConnectData, placeableNetData);
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & (Snap.ExistingGeometry | Snap.ObjectSide | Snap.NearbyGeometry)) != Snap.None)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandleExistingObjects(ref bestSnapPosition, controlPoint, prefabRoadData, netGeometryData, prefabNetData, placeableNetData);
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.LotGrid) != Snap.None)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandleLotGrid(ref bestSnapPosition, controlPoint, prefabRoadData, netGeometryData, prefabNetData, placeableNetData);
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.ZoneGrid) != Snap.None)
          {
            // ISSUE: reference to a compiler-generated method
            this.HandleZoneGrid(ref bestSnapPosition, controlPoint, prefabRoadData, netGeometryData, prefabNetData);
          }
          ControlPoint snapTargetControlPoint = bestSnapPosition;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Mode == NetToolSystem.Mode.Grid)
          {
            // ISSUE: reference to a compiler-generated method
            this.AdjustMiddlePoint(ref bestSnapPosition, netGeometryData);
            // ISSUE: reference to a compiler-generated method
            this.AdjustControlPointHeight(ref bestSnapPosition, controlPoint, netGeometryData, placeableNetData);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AdjustControlPointHeight(ref bestSnapPosition, controlPoint, netGeometryData, placeableNetData);
            // ISSUE: reference to a compiler-generated field
            if (this.m_Mode == NetToolSystem.Mode.Continuous)
            {
              // ISSUE: reference to a compiler-generated method
              this.AdjustMiddlePoint(ref bestSnapPosition, netGeometryData);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            // ISSUE: reference to a compiler-generated field
            if ((this.m_Snap & Snap.AutoParent) == Snap.None)
              bestSnapPosition.m_OriginalEntity = Entity.Null;
            else if (bestSnapPosition.m_OriginalEntity == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindParent(ref bestSnapPosition, netGeometryData);
            }
          }
          // ISSUE: reference to a compiler-generated method
          if (this.CanPlaySnapSound(ref snapTargetControlPoint, ref controlPoint))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SourceUpdateData.AddSnap();
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints[this.m_ControlPoints.Length - 1] = bestSnapPosition;
          // ISSUE: reference to a compiler-generated field
          this.m_LastSnappedEntity.Value = snapTargetControlPoint.m_OriginalEntity;
        }
      }

      private bool CanPlaySnapSound(
        ref ControlPoint snapTargetControlPoint,
        ref ControlPoint controlPoint)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabNetData.HasComponent(this.m_Prefab))
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Layer requiredLayers = this.m_PrefabNetData[this.m_Prefab].m_RequiredLayers;
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        int num2 = this.m_LastControlPointsAngle.Value;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.StraightDirection) != Snap.None && this.m_ControlPoints.Length >= 2 && snapTargetControlPoint.m_OriginalEntity == Entity.Null)
        {
          ControlPoint controlPoint1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Mode == NetToolSystem.Mode.Continuous && this.m_ControlPoints.Length == 3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            controlPoint1 = this.m_ControlPoints[0] with
            {
              m_Direction = this.m_ControlPoints[1].m_Direction
            };
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            controlPoint1 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
          }
          Line3.Segment segment = new Line3.Segment(controlPoint1.m_Position, snapTargetControlPoint.m_Position);
          float num3 = MathUtils.Length(segment.xz);
          if ((double) num3 > 1.0)
          {
            float2 direction = controlPoint1.m_Direction;
            float2 x1 = (segment.b.xz - segment.a.xz) / num3;
            float x2 = math.dot(x1, direction);
            num1 = (int) math.round(math.degrees(math.atan2((float) ((double) x1.x * (double) direction.y - (double) direction.x * (double) x1.y), x2)) + 180f);
            // ISSUE: reference to a compiler-generated field
            if (num1 % 180 == 0 && this.m_StartEntity.Value == Entity.Null)
              num1 = 0;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastControlPointsAngle.Value = num1;
        if (snapTargetControlPoint.m_OriginalEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LastSnappedEntity.Value == Entity.Null && snapTargetControlPoint.m_OriginalEntity != controlPoint.m_OriginalEntity)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadData.HasComponent(snapTargetControlPoint.m_OriginalEntity))
              return true;
            PrefabRef componentData1;
            LocalConnectData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_PrefabRefData.TryGetComponent(snapTargetControlPoint.m_OriginalEntity, out componentData1) || !this.m_LocalConnectData.TryGetComponent((Entity) componentData1, out componentData2))
              return false;
            Layer layer1 = componentData2.m_Layers & ~Layer.Road;
            if ((layer1 & requiredLayers) != Layer.None)
              return true;
            Layer layer2 = Layer.WaterPipe | Layer.SewagePipe;
            if ((layer1 & layer2) != Layer.None && (requiredLayers & layer2) != Layer.None)
              return true;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (num2 % 360 != 0 && num2 != num1 && num1 % 360 != 0 && (num1 % 90 == 0 || num1 % 45 == 0 && this.m_Mode == NetToolSystem.Mode.Continuous))
            return true;
        }
        return false;
      }

      private void HandleWorldSize(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        NetGeometryData prefabGeometryData)
      {
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds = TerrainUtils.GetBounds(ref this.m_TerrainHeightData);
        float num = prefabGeometryData.m_DefaultWidth * 0.5f;
        bool2 bool2 = (bool2) false;
        float2 b = (float2) 0.0f;
        if ((double) controlPoint.m_HitPosition.x < (double) bounds.min.x + (double) num)
        {
          bool2.x = true;
          b.x = bounds.min.x - num;
        }
        else if ((double) controlPoint.m_HitPosition.x > (double) bounds.max.x - (double) num)
        {
          bool2.x = true;
          b.x = bounds.max.x + num;
        }
        if ((double) controlPoint.m_HitPosition.z < (double) bounds.min.z + (double) num)
        {
          bool2.y = true;
          b.y = bounds.min.z - num;
        }
        else if ((double) controlPoint.m_HitPosition.z > (double) bounds.max.z - (double) num)
        {
          bool2.y = true;
          b.y = bounds.max.z + num;
        }
        if (!math.any(bool2))
          return;
        ControlPoint controlPoint1 = controlPoint with
        {
          m_OriginalEntity = Entity.Null,
          m_Direction = new float2(0.0f, 1f)
        };
        controlPoint1.m_Position.xz = math.select(controlPoint.m_HitPosition.xz, b, bool2);
        controlPoint1.m_Position.y = controlPoint.m_HitPosition.y;
        controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(2f, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
        ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint1);
        if (bool2.x)
        {
          Line3 line3 = new Line3(controlPoint1.m_Position, controlPoint1.m_Position);
          line3.a.z = bounds.min.z;
          line3.b.z = bounds.max.z;
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(line3.a, line3.b), SnapLineFlags.Hidden));
        }
        if (!bool2.y)
          return;
        controlPoint1.m_Direction = new float2(1f, 0.0f);
        Line3 line3_1 = new Line3(controlPoint1.m_Position, controlPoint1.m_Position);
        line3_1.a.x = bounds.min.x;
        line3_1.b.x = bounds.max.x;
        // ISSUE: reference to a compiler-generated field
        ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(line3_1.a, line3_1.b), SnapLineFlags.Hidden));
      }

      private void FindParent(ref ControlPoint bestSnapPosition, NetGeometryData prefabGeometryData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Line3.Segment line = this.m_ControlPoints.Length < 2 ? new Line3.Segment(bestSnapPosition.m_Position, bestSnapPosition.m_Position) : new Line3.Segment(this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_Position, bestSnapPosition.m_Position);
        float num = math.max(0.01f, (float) ((double) prefabGeometryData.m_DefaultWidth * 0.5 - 0.5));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.SnapJob.ParentObjectIterator iterator = new NetToolSystem.SnapJob.ParentObjectIterator()
        {
          m_BestSnapPosition = bestSnapPosition,
          m_Line = line,
          m_Bounds = MathUtils.Expand(MathUtils.Bounds(line), (float3) (num + 0.4f)),
          m_Radius = num,
          m_OwnerData = this.m_OwnerData,
          m_TransformData = this.m_TransformData,
          m_BuildingData = this.m_BuildingData,
          m_BuildingExtensionData = this.m_BuildingExtensionData,
          m_AssetStampData = this.m_AssetStampData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabObjectGeometryData = this.m_ObjectGeometryData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<NetToolSystem.SnapJob.ParentObjectIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        bestSnapPosition = iterator.m_BestSnapPosition;
      }

      private void HandleLotGrid(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        RoadData prefabRoadData,
        NetGeometryData prefabGeometryData,
        NetData prefabNetData,
        PlaceableNetData placeableNetData)
      {
        int cellWidth = ZoneUtils.GetCellWidth(prefabGeometryData.m_DefaultWidth);
        float num1 = (float) ((double) (cellWidth + 1) * ((double) placeableNetData.m_SnapDistance * 0.5) * 1.4142135381698608);
        float num2 = 0.0f;
        float num3 = (float) ((double) placeableNetData.m_SnapDistance * 0.5 + 0.10000000149011612);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabLaneData.HasComponent(this.m_LanePrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num2 = this.m_PrefabLaneData[this.m_LanePrefab].m_Width * 0.5f;
        }
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
        NetToolSystem.SnapJob.LotIterator iterator = new NetToolSystem.SnapJob.LotIterator()
        {
          m_Bounds = new Bounds2(controlPoint.m_HitPosition.xz - num1, controlPoint.m_HitPosition.xz + num1),
          m_Radius = prefabGeometryData.m_DefaultWidth * 0.5f,
          m_EdgeOffset = num2,
          m_MaxDistance = num3,
          m_CellWidth = cellWidth,
          m_ControlPoint = controlPoint,
          m_BestSnapPosition = bestSnapPosition,
          m_SnapLines = this.m_SnapLines,
          m_OwnerData = this.m_OwnerData,
          m_EdgeData = this.m_EdgeData,
          m_NodeData = this.m_NodeData,
          m_TransformData = this.m_TransformData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_BuildingData = this.m_BuildingData,
          m_BuildingExtensionData = this.m_BuildingExtensionData,
          m_AssetStampData = this.m_AssetStampData,
          m_PrefabObjectGeometryData = this.m_ObjectGeometryData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<NetToolSystem.SnapJob.LotIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        bestSnapPosition = iterator.m_BestSnapPosition;
      }

      private void AdjustControlPointHeight(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        NetGeometryData prefabGeometryData,
        PlaceableNetData placeableNetData)
      {
        float y = bestSnapPosition.m_Position.y;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bestSnapPosition.m_Position.y = (this.m_Snap & Snap.ObjectSurface) == Snap.None || !this.m_TransformData.HasComponent(controlPoint.m_OriginalEntity) ? ((double) this.m_Elevation >= 0.0 ? WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, bestSnapPosition.m_Position) + this.m_Elevation : TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, bestSnapPosition.m_Position) + this.m_Elevation) : controlPoint.m_HitPosition.y;
        Bounds1 bounds1_1 = prefabGeometryData.m_DefaultHeightRange + bestSnapPosition.m_Position.y;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(controlPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[controlPoint.m_OriginalEntity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            Bounds1 bounds1_2 = this.m_PrefabGeometryData[prefabRef.m_Prefab].m_DefaultHeightRange + controlPoint.m_Position.y;
            if ((double) bounds1_2.max > (double) bounds1_1.min)
            {
              bounds1_1.max = math.max(bounds1_1.max, bounds1_2.max);
              if (bestSnapPosition.m_OriginalEntity == Entity.Null)
                bestSnapPosition.m_OriginalEntity = controlPoint.m_OriginalEntity;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(bestSnapPosition.m_OriginalEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[bestSnapPosition.m_OriginalEntity];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabGeometryData.HasComponent(prefabRef1.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        Bounds1 bounds2 = this.m_PrefabGeometryData[prefabRef1.m_Prefab].m_DefaultHeightRange + y;
        if (MathUtils.Intersect(bounds1_1, bounds2))
        {
          bestSnapPosition.m_Elevation += y - bestSnapPosition.m_Position.y;
          bestSnapPosition.m_Position.y = y;
          bestSnapPosition.m_Elevation = MathUtils.Clamp(bestSnapPosition.m_Elevation, placeableNetData.m_ElevationRange);
        }
        else
          bestSnapPosition.m_OriginalEntity = Entity.Null;
      }

      private void AdjustMiddlePoint(
        ref ControlPoint bestSnapPosition,
        NetGeometryData netGeometryData)
      {
        float2 a = ((netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) == (Game.Net.GeometryFlags) 0 ? netGeometryData.m_DefaultWidth * new float2(16f, 8f) : (float) ZoneUtils.GetCellWidth(netGeometryData.m_DefaultWidth) * 8f + new float2(192f, 96f)) * 11f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length == 2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
          float2 float2 = bestSnapPosition.m_Position.xz - controlPoint.m_Position.xz;
          if (MathUtils.TryNormalize(ref float2))
            bestSnapPosition.m_Direction = float2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Mode != NetToolSystem.Mode.Grid || (double) math.distance(controlPoint.m_Position.xz, bestSnapPosition.m_Position.xz) <= (double) a.x)
            return;
          bestSnapPosition.m_Position.xz = controlPoint.m_Position.xz + float2 * a.x;
          bestSnapPosition.m_OriginalEntity = Entity.Null;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length != 3)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[this.m_ControlPoints.Length - 3];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Mode == NetToolSystem.Mode.Grid)
          {
            float2 x = bestSnapPosition.m_Position.xz - controlPoint1.m_Position.xz;
            float2 float2_1 = new float2(math.dot(x, controlPoint2.m_Direction), math.dot(x, MathUtils.Right(controlPoint2.m_Direction)));
            bool2 bool2 = math.abs(float2_1) > a;
            float2 float2_2 = math.select(float2_1, math.select(a, -a, float2_1 < 0.0f), bool2);
            controlPoint2.m_Position = controlPoint1.m_Position;
            controlPoint2.m_Position.xz += controlPoint2.m_Direction * float2_2.x;
            if (math.any(bool2))
            {
              bestSnapPosition.m_Position.xz = controlPoint2.m_Position.xz + MathUtils.Right(controlPoint2.m_Direction) * float2_2.y;
              bestSnapPosition.m_OriginalEntity = Entity.Null;
            }
          }
          else
          {
            controlPoint2.m_Elevation = (float) (((double) controlPoint1.m_Elevation + (double) bestSnapPosition.m_Elevation) * 0.5);
            float2 x = bestSnapPosition.m_Position.xz - controlPoint1.m_Position.xz;
            float2 direction = controlPoint2.m_Direction;
            float2 float2_3 = x;
            if (MathUtils.TryNormalize(ref float2_3))
            {
              float num = math.dot(direction, float2_3);
              if ((double) num >= 0.70710676908493042)
              {
                float2 _a = math.lerp(controlPoint1.m_Position.xz, bestSnapPosition.m_Position.xz, 0.5f);
                float2 t;
                if (MathUtils.Intersect(new Line2(controlPoint1.m_Position.xz, controlPoint1.m_Position.xz + direction), new Line2(_a, _a + MathUtils.Right(float2_3)), out t))
                {
                  controlPoint2.m_Position = controlPoint1.m_Position;
                  controlPoint2.m_Position.xz += direction * t.x;
                  float2 float2_4 = bestSnapPosition.m_Position.xz - controlPoint2.m_Position.xz;
                  if (MathUtils.TryNormalize(ref float2_4))
                    bestSnapPosition.m_Direction = float2_4;
                }
              }
              else if ((double) num >= 0.0)
              {
                float2 _a = math.lerp(controlPoint1.m_Position.xz, bestSnapPosition.m_Position.xz, 0.5f);
                Line2 line2_1 = new Line2(controlPoint1.m_Position.xz, controlPoint1.m_Position.xz + MathUtils.Right(direction));
                Line2 line2_2 = new Line2(_a, _a + MathUtils.Right(float2_3));
                float2 t;
                if (MathUtils.Intersect(line2_1, line2_2, out t))
                {
                  controlPoint2.m_Position = controlPoint1.m_Position;
                  controlPoint2.m_Position.xz += direction * math.abs(t.x);
                  float2 forward = bestSnapPosition.m_Position.xz - MathUtils.Position(line2_1, t.x);
                  if (MathUtils.TryNormalize(ref forward))
                    bestSnapPosition.m_Direction = math.select(MathUtils.Right(forward), MathUtils.Left(forward), (double) math.dot(MathUtils.Right(direction), float2_3) < 0.0);
                }
              }
              else
              {
                controlPoint2.m_Position = controlPoint1.m_Position;
                controlPoint2.m_Position.xz += direction * math.abs(math.dot(x, MathUtils.Right(direction)) * 0.5f);
                bestSnapPosition.m_Direction = -controlPoint2.m_Direction;
              }
            }
            else
              controlPoint2.m_Position = controlPoint1.m_Position;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          controlPoint2.m_Position.y = (double) controlPoint2.m_Elevation >= 0.0 ? WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, controlPoint2.m_Position) + controlPoint2.m_Elevation : TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, controlPoint2.m_Position) + controlPoint2.m_Elevation;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints[this.m_ControlPoints.Length - 2] = controlPoint2;
        }
      }

      private void HandleControlPoints(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        NetGeometryData prefabGeometryData,
        PlaceableNetData placeableNetData)
      {
        ControlPoint controlPoint1 = controlPoint with
        {
          m_OriginalEntity = Entity.Null,
          m_Position = controlPoint.m_HitPosition
        };
        float snapDistance = placeableNetData.m_SnapDistance;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Grid && this.m_ControlPoints.Length == 3)
        {
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.CellLength) == Snap.None)
            return;
          // ISSUE: reference to a compiler-generated field
          float2 xz = this.m_ControlPoints[0].m_Position.xz;
          // ISSUE: reference to a compiler-generated field
          float2 direction = this.m_ControlPoints[1].m_Direction;
          float2 y = MathUtils.Right(direction);
          float2 x = controlPoint.m_HitPosition.xz - xz;
          x = new float2(math.dot(x, direction), math.dot(x, y));
          float2 float2_1 = MathUtils.Snap(x, (float2) snapDistance);
          float2 float2_2 = xz + (float2_1.x * direction + float2_1.y * y);
          controlPoint1.m_Direction = direction;
          controlPoint1.m_Position.xz = float2_2;
          controlPoint1.m_Position.y = controlPoint.m_HitPosition.y;
          controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
          Line3 line3_1 = new Line3(controlPoint1.m_Position, controlPoint1.m_Position);
          Line3 line3_2 = new Line3(controlPoint1.m_Position, controlPoint1.m_Position);
          line3_1.a.xz -= controlPoint1.m_Direction * 8f;
          line3_1.b.xz += controlPoint1.m_Direction * 8f;
          line3_2.a.xz -= MathUtils.Right(controlPoint1.m_Direction) * 8f;
          line3_2.b.xz += MathUtils.Right(controlPoint1.m_Direction) * 8f;
          ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint1);
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(line3_1.a, line3_1.b), SnapLineFlags.Hidden));
          controlPoint1.m_Direction = MathUtils.Right(controlPoint1.m_Direction);
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(line3_2.a, line3_2.b), SnapLineFlags.Hidden));
        }
        else
        {
          ControlPoint controlPoint2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Mode == NetToolSystem.Mode.Continuous && this.m_ControlPoints.Length == 3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            controlPoint2 = this.m_ControlPoints[0] with
            {
              m_OriginalEntity = Entity.Null,
              m_Direction = this.m_ControlPoints[1].m_Direction
            };
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
            // ISSUE: reference to a compiler-generated field
            if (controlPoint2.m_Direction.Equals(new float2()) && this.m_ControlPoints.Length >= 3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              controlPoint2.m_Direction = math.normalizesafe(controlPoint2.m_Position.xz - this.m_ControlPoints[this.m_ControlPoints.Length - 3].m_Position.xz);
            }
          }
          float3 float3_1 = controlPoint.m_HitPosition - controlPoint2.m_Position;
          float3 float3_2 = MathUtils.Normalize(float3_1, float3_1.xz);
          float3_2.y = math.clamp(float3_2.y, -1f, 1f);
          bool flag1 = false;
          bool flag2 = false;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.StraightDirection) != Snap.None)
          {
            float maxValue = float.MaxValue;
            if (controlPoint2.m_OriginalEntity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.HandleStartDirection(controlPoint2.m_OriginalEntity, controlPoint2, controlPoint, placeableNetData, ref maxValue, ref controlPoint1.m_Position, ref float3_2);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_StartEntity.Value != Entity.Null && this.m_StartEntity.Value != controlPoint2.m_OriginalEntity && this.m_ControlPoints.Length == 2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.HandleStartDirection(this.m_StartEntity.Value, controlPoint2, controlPoint, placeableNetData, ref maxValue, ref controlPoint1.m_Position, ref float3_2);
            }
            if (!controlPoint2.m_Direction.Equals(new float2()) && (double) maxValue == 3.4028234663852886E+38)
            {
              ToolUtils.DirectionSnap(ref maxValue, ref controlPoint1.m_Position, ref float3_2, controlPoint.m_HitPosition, controlPoint2.m_Position, new float3(controlPoint2.m_Direction.x, 0.0f, controlPoint2.m_Direction.y), placeableNetData.m_SnapDistance);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((double) maxValue >= (double) placeableNetData.m_SnapDistance && this.m_Mode == NetToolSystem.Mode.Continuous && this.m_ControlPoints.Length == 3)
              {
                float2 float2_3 = MathUtils.RotateLeft(controlPoint2.m_Direction, 0.7853982f);
                ToolUtils.DirectionSnap(ref maxValue, ref controlPoint1.m_Position, ref float3_2, controlPoint.m_HitPosition, controlPoint2.m_Position, new float3(float2_3.x, 0.0f, float2_3.y), placeableNetData.m_SnapDistance);
                float2 float2_4 = MathUtils.RotateRight(controlPoint2.m_Direction, 0.7853982f);
                ToolUtils.DirectionSnap(ref maxValue, ref controlPoint1.m_Position, ref float3_2, controlPoint.m_HitPosition, controlPoint2.m_Position, new float3(float2_4.x, 0.0f, float2_4.y), placeableNetData.m_SnapDistance);
                snapDistance *= 1.41421354f;
              }
            }
            flag1 = (double) maxValue < (double) placeableNetData.m_SnapDistance;
            flag2 = (double) maxValue < (double) placeableNetData.m_SnapDistance;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.CellLength) != Snap.None && (this.m_Mode != NetToolSystem.Mode.Continuous || this.m_ControlPoints.Length == 3 & flag2))
          {
            float num = math.distance(controlPoint2.m_Position, controlPoint1.m_Position);
            controlPoint1.m_Position = controlPoint2.m_Position + float3_2 * MathUtils.Snap(num, snapDistance);
            flag1 = true;
          }
          controlPoint1.m_Direction = float3_2.xz;
          controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
          if (flag1)
            ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint1);
          if (!flag2)
            return;
          float3 position = controlPoint1.m_Position;
          float3 endPos = position;
          endPos.xz += controlPoint1.m_Direction;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(position, endPos), NetToolSystem.SnapJob.GetSnapLineFlags(prefabGeometryData.m_Flags) | SnapLineFlags.Hidden));
        }
      }

      private void HandleStartDirection(
        Entity startEntity,
        ControlPoint prev,
        ControlPoint controlPoint,
        PlaceableNetData placeableNetData,
        ref float bestDirectionDistance,
        ref float3 snapPosition,
        ref float3 snapDirection)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.HasBuffer(startEntity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[startEntity];
          for (int index = 0; index < connectedEdge.Length; ++index)
          {
            Entity edge1 = connectedEdge[index].m_Edge;
            // ISSUE: reference to a compiler-generated field
            Game.Net.Edge edge2 = this.m_EdgeData[edge1];
            if (!(edge2.m_Start != startEntity) || !(edge2.m_End != startEntity))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[edge1];
              float3 float3 = edge2.m_Start == startEntity ? MathUtils.StartTangent(curve.m_Bezier) : MathUtils.EndTangent(curve.m_Bezier);
              float3 snapDir = MathUtils.Normalize(float3, float3.xz);
              snapDir.y = math.clamp(snapDir.y, -1f, 1f);
              ToolUtils.DirectionSnap(ref bestDirectionDistance, ref snapPosition, ref snapDirection, controlPoint.m_HitPosition, prev.m_Position, snapDir, placeableNetData.m_SnapDistance);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(startEntity))
          {
            // ISSUE: reference to a compiler-generated field
            float3 float3 = MathUtils.Tangent(this.m_CurveData[startEntity].m_Bezier, prev.m_CurvePosition);
            float3 snapDir = MathUtils.Normalize(float3, float3.xz);
            snapDir.y = math.clamp(snapDir.y, -1f, 1f);
            ToolUtils.DirectionSnap(ref bestDirectionDistance, ref snapPosition, ref snapDirection, controlPoint.m_HitPosition, prev.m_Position, snapDir, placeableNetData.m_SnapDistance);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TransformData.HasComponent(startEntity))
              return;
            // ISSUE: reference to a compiler-generated field
            float3 float3 = math.forward(this.m_TransformData[startEntity].m_Rotation);
            float3 snapDir = MathUtils.Normalize(float3, float3.xz);
            snapDir.y = math.clamp(snapDir.y, -1f, 1f);
            ToolUtils.DirectionSnap(ref bestDirectionDistance, ref snapPosition, ref snapDirection, controlPoint.m_HitPosition, prev.m_Position, snapDir, placeableNetData.m_SnapDistance);
          }
        }
      }

      private void HandleZoneGrid(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        RoadData prefabRoadData,
        NetGeometryData prefabGeometryData,
        NetData prefabNetData)
      {
        int cellWidth = ZoneUtils.GetCellWidth(prefabGeometryData.m_DefaultWidth);
        float num1 = (float) ((double) (cellWidth + 1) * 4.0 * 1.4142135381698608);
        float offset = math.select(0.0f, 4f, (cellWidth & 1) == 0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.SnapJob.ZoneIterator iterator = new NetToolSystem.SnapJob.ZoneIterator()
        {
          m_Bounds = new Bounds2(controlPoint.m_HitPosition.xz - num1, controlPoint.m_HitPosition.xz + num1),
          m_HitPosition = controlPoint.m_HitPosition.xz,
          m_BestDistance = num1,
          m_ZoneBlockData = this.m_ZoneBlockData,
          m_ZoneCells = this.m_ZoneCells
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ZoneSearchTree.Iterate<NetToolSystem.SnapJob.ZoneIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        if ((double) iterator.m_BestDistance >= (double) num1)
          return;
        // ISSUE: reference to a compiler-generated field
        float2 x = controlPoint.m_HitPosition.xz - iterator.m_BestPosition.xz;
        // ISSUE: reference to a compiler-generated field
        float2 y = MathUtils.Right(iterator.m_BestDirection);
        // ISSUE: reference to a compiler-generated field
        float num2 = MathUtils.Snap(math.dot(x, iterator.m_BestDirection), 8f, offset);
        float num3 = MathUtils.Snap(math.dot(x, y), 8f, offset);
        ControlPoint controlPoint1 = controlPoint;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeData.HasComponent(controlPoint.m_OriginalEntity) && !this.m_NodeData.HasComponent(controlPoint.m_OriginalEntity))
          controlPoint1.m_OriginalEntity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        controlPoint1.m_Direction = iterator.m_BestDirection;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        controlPoint1.m_Position.xz = iterator.m_BestPosition.xz + iterator.m_BestDirection * num2 + y * num3;
        controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
        Line3 line3_1 = new Line3(controlPoint1.m_Position, controlPoint1.m_Position);
        Line3 line3_2 = new Line3(controlPoint1.m_Position, controlPoint1.m_Position);
        line3_1.a.xz -= controlPoint1.m_Direction * 8f;
        line3_1.b.xz += controlPoint1.m_Direction * 8f;
        line3_2.a.xz -= MathUtils.Right(controlPoint1.m_Direction) * 8f;
        line3_2.b.xz += MathUtils.Right(controlPoint1.m_Direction) * 8f;
        ToolUtils.AddSnapPosition(ref bestSnapPosition, controlPoint1);
        // ISSUE: reference to a compiler-generated field
        ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(line3_1.a, line3_1.b), SnapLineFlags.Hidden));
        controlPoint1.m_Direction = MathUtils.Right(controlPoint1.m_Direction);
        // ISSUE: reference to a compiler-generated field
        ToolUtils.AddSnapLine(ref bestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, NetUtils.StraightCurve(line3_2.a, line3_2.b), SnapLineFlags.Hidden));
      }

      private void HandleExistingObjects(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        RoadData prefabRoadData,
        NetGeometryData prefabGeometryData,
        NetData prefabNetData,
        PlaceableNetData placeableNetData)
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = (this.m_Snap & Snap.NearbyGeometry) != Snap.None ? placeableNetData.m_SnapDistance : 0.0f;
        // ISSUE: reference to a compiler-generated field
        float num2 = (prefabRoadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) == (Game.Prefabs.RoadFlags) 0 || (this.m_Snap & Snap.CellLength) == Snap.None ? prefabGeometryData.m_DefaultWidth * 0.5f : (float) ZoneUtils.GetCellWidth(prefabGeometryData.m_DefaultWidth) * 4f;
        float x = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & (Snap.ExistingGeometry | Snap.NearbyGeometry)) != Snap.None)
          x = math.max(x, prefabGeometryData.m_DefaultWidth + num1);
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.ObjectSide) != Snap.None)
          x = math.max(x, num2 + placeableNetData.m_SnapDistance);
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
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.SnapJob.ObjectIterator iterator = new NetToolSystem.SnapJob.ObjectIterator()
        {
          m_Bounds = new Bounds3(controlPoint.m_HitPosition - x, controlPoint.m_HitPosition + x),
          m_Snap = this.m_Snap,
          m_MaxDistance = placeableNetData.m_SnapDistance,
          m_NetSnapOffset = num1,
          m_ObjectSnapOffset = num2,
          m_SnapCellLength = (prefabRoadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0 && (this.m_Snap & Snap.CellLength) > Snap.None,
          m_NetData = prefabNetData,
          m_NetGeometryData = prefabGeometryData,
          m_ControlPoint = controlPoint,
          m_BestSnapPosition = bestSnapPosition,
          m_SnapLines = this.m_SnapLines,
          m_OwnerData = this.m_OwnerData,
          m_CurveData = this.m_CurveData,
          m_NodeData = this.m_NodeData,
          m_TransformData = this.m_TransformData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_BuildingData = this.m_BuildingData,
          m_ObjectGeometryData = this.m_ObjectGeometryData,
          m_PrefabNetData = this.m_PrefabNetData,
          m_PrefabGeometryData = this.m_PrefabGeometryData,
          m_ConnectedEdges = this.m_ConnectedEdges
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<NetToolSystem.SnapJob.ObjectIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        bestSnapPosition = iterator.m_BestSnapPosition;
      }

      private static SnapLineFlags GetSnapLineFlags(Game.Net.GeometryFlags geometryFlags)
      {
        SnapLineFlags snapLineFlags = (SnapLineFlags) 0;
        if ((geometryFlags & Game.Net.GeometryFlags.StrictNodes) == (Game.Net.GeometryFlags) 0)
          snapLineFlags |= SnapLineFlags.ExtendedCurve;
        return snapLineFlags;
      }

      private void HandleExistingGeometry(
        ref ControlPoint bestSnapPosition,
        ControlPoint controlPoint,
        RoadData prefabRoadData,
        NetGeometryData prefabGeometryData,
        NetData prefabNetData,
        LocalConnectData localConnectData,
        PlaceableNetData placeableNetData)
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = (this.m_Snap & Snap.NearbyGeometry) != Snap.None ? placeableNetData.m_SnapDistance : 0.0f;
        float num2 = prefabGeometryData.m_DefaultWidth + num1;
        float num3 = placeableNetData.m_SnapDistance * 64f;
        Bounds1 bounds1 = new Bounds1(-50f, 50f) | localConnectData.m_HeightRange;
        Bounds3 bounds3_1 = new Bounds3();
        bounds3_1.xz = new Bounds2(controlPoint.m_HitPosition.xz - num2, controlPoint.m_HitPosition.xz + num2);
        bounds3_1.y = controlPoint.m_HitPosition.y + bounds1;
        Bounds3 bounds3_2 = bounds3_1;
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.GuideLines) != Snap.None)
        {
          bounds3_2.min -= num3;
          bounds3_2.max += num3;
        }
        float num4 = -1f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((prefabGeometryData.m_Flags & (Game.Net.GeometryFlags.SnapToNetAreas | Game.Net.GeometryFlags.StandingNodes)) != (Game.Net.GeometryFlags) 0 && this.m_SubObjects.HasBuffer(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Prefabs.SubObject> subObject1 = this.m_SubObjects[this.m_Prefab];
          for (int index = 0; index < subObject1.Length; ++index)
          {
            Game.Prefabs.SubObject subObject2 = subObject1[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectGeometryData.HasComponent(subObject2.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[subObject2.m_Prefab];
              if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
                num4 = math.max(num4, objectGeometryData.m_LegSize.x);
            }
          }
        }
        float num5 = math.select(num4, prefabGeometryData.m_DefaultWidth, (double) num4 <= 0.0);
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
        // ISSUE: variable of a compiler-generated type
        NetToolSystem.SnapJob.NetIterator iterator = new NetToolSystem.SnapJob.NetIterator()
        {
          m_TotalBounds = bounds3_2,
          m_Bounds = bounds3_1,
          m_Snap = this.m_Snap,
          m_SnapOffset = num1,
          m_SnapDistance = placeableNetData.m_SnapDistance,
          m_Elevation = this.m_Elevation,
          m_GuideLength = num3,
          m_LegSnapWidth = num5,
          m_HeightRange = bounds1,
          m_NetData = prefabNetData,
          m_PrefabRoadData = prefabRoadData,
          m_NetGeometryData = prefabGeometryData,
          m_LocalConnectData = localConnectData,
          m_ControlPoint = controlPoint,
          m_BestSnapPosition = bestSnapPosition,
          m_SnapLines = this.m_SnapLines,
          m_TerrainHeightData = this.m_TerrainHeightData,
          m_WaterSurfaceData = this.m_WaterSurfaceData,
          m_OwnerData = this.m_OwnerData,
          m_EditorMode = this.m_EditorMode,
          m_NodeData = this.m_NodeData,
          m_EdgeData = this.m_EdgeData,
          m_CurveData = this.m_CurveData,
          m_CompositionData = this.m_CompositionData,
          m_EdgeGeometryData = this.m_EdgeGeometryData,
          m_RoadData = this.m_RoadData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabNetData = this.m_PrefabNetData,
          m_PrefabGeometryData = this.m_PrefabGeometryData,
          m_PrefabCompositionData = this.m_PrefabCompositionData,
          m_RoadCompositionData = this.m_RoadCompositionData,
          m_ConnectedEdges = this.m_ConnectedEdges,
          m_PrefabCompositionAreas = this.m_PrefabCompositionAreas
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.ExistingGeometry) != Snap.None && this.m_PrefabRefData.HasComponent(controlPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[controlPoint.m_OriginalEntity];
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!iterator.HandleGeometry(controlPoint, prefabRef) && (this.m_Snap & Snap.GuideLines) != Snap.None)
          {
            // ISSUE: reference to a compiler-generated method
            iterator.HandleGuideLines(controlPoint.m_OriginalEntity);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<NetToolSystem.SnapJob.NetIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        bestSnapPosition = iterator.m_BestSnapPosition;
      }

      private struct ParentObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public ControlPoint m_BestSnapPosition;
        public Line3.Segment m_Line;
        public Bounds3 m_Bounds;
        public float m_Radius;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
        public ComponentLookup<BuildingExtensionData> m_BuildingExtensionData;
        public ComponentLookup<AssetStampData> m_AssetStampData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) || this.m_OwnerData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[item];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BuildingData.HasComponent(prefabRef.m_Prefab) && !this.m_BuildingExtensionData.HasComponent(prefabRef.m_Prefab) && !this.m_AssetStampData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[item];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
          float3 float3 = MathUtils.Center(bounds.m_Bounds);
          // ISSUE: reference to a compiler-generated field
          Line3.Segment segment = this.m_Line - float3;
          int2 int2;
          int2.x = ZoneUtils.GetCellWidth(objectGeometryData.m_Size.x);
          int2.y = ZoneUtils.GetCellWidth(objectGeometryData.m_Size.z);
          float2 size = (float2) int2 * 8f;
          if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
          {
            Circle2 circle2 = new Circle2(size.x * 0.5f, (transform.m_Position - float3).xz);
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(circle2, new Circle2(this.m_Radius, segment.a.xz)))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BestSnapPosition.m_OriginalEntity = item;
              // ISSUE: reference to a compiler-generated field
              this.m_BestSnapPosition.m_ElementIndex = new int2(-1, -1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (MathUtils.Intersect(circle2, new Circle2(this.m_Radius, segment.b.xz)))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_OriginalEntity = item;
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_ElementIndex = new int2(-1, -1);
              }
              else
              {
                float num = MathUtils.Length(segment.xz);
                // ISSUE: reference to a compiler-generated field
                if ((double) num <= (double) this.m_Radius)
                  return;
                // ISSUE: reference to a compiler-generated field
                float2 float2 = MathUtils.Right((segment.b.xz - segment.a.xz) * (this.m_Radius / num));
                if (!MathUtils.Intersect(new Quad2(segment.a.xz + float2, segment.b.xz + float2, segment.b.xz - float2, segment.a.xz - float2), circle2))
                  return;
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_OriginalEntity = item;
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_ElementIndex = new int2(-1, -1);
              }
            }
          }
          else
          {
            Quad2 xz = ObjectUtils.CalculateBaseCorners(transform.m_Position - float3, transform.m_Rotation, size).xz;
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(xz, new Circle2(this.m_Radius, segment.a.xz)))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BestSnapPosition.m_OriginalEntity = item;
              // ISSUE: reference to a compiler-generated field
              this.m_BestSnapPosition.m_ElementIndex = new int2(-1, -1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (MathUtils.Intersect(xz, new Circle2(this.m_Radius, segment.b.xz)))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_OriginalEntity = item;
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_ElementIndex = new int2(-1, -1);
              }
              else
              {
                float num = MathUtils.Length(segment.xz);
                // ISSUE: reference to a compiler-generated field
                if ((double) num <= (double) this.m_Radius)
                  return;
                // ISSUE: reference to a compiler-generated field
                float2 float2 = MathUtils.Right((segment.b.xz - segment.a.xz) * (this.m_Radius / num));
                Quad2 quad2 = new Quad2(segment.a.xz + float2, segment.b.xz + float2, segment.b.xz - float2, segment.a.xz - float2);
                if (!MathUtils.Intersect(xz, quad2))
                  return;
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_OriginalEntity = item;
                // ISSUE: reference to a compiler-generated field
                this.m_BestSnapPosition.m_ElementIndex = new int2(-1, -1);
              }
            }
          }
        }
      }

      private struct LotIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public float m_Radius;
        public float m_EdgeOffset;
        public float m_MaxDistance;
        public int m_CellWidth;
        public ControlPoint m_ControlPoint;
        public ControlPoint m_BestSnapPosition;
        public NativeList<SnapLine> m_SnapLines;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Net.Edge> m_EdgeData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
        public ComponentLookup<BuildingExtensionData> m_BuildingExtensionData;
        public ComponentLookup<AssetStampData> m_AssetStampData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || this.m_OwnerData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[item];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BuildingData.HasComponent(prefabRef.m_Prefab) && !this.m_BuildingExtensionData.HasComponent(prefabRef.m_Prefab) && !this.m_AssetStampData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[item];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
          float2 float2_1 = math.normalizesafe(math.forward(transform.m_Rotation).xz, new float2(0.0f, 1f));
          float2 y = MathUtils.Right(float2_1);
          // ISSUE: reference to a compiler-generated field
          float2 x1 = this.m_ControlPoint.m_HitPosition.xz - transform.m_Position.xz;
          int2 int2;
          int2.x = ZoneUtils.GetCellWidth(objectGeometryData.m_Size.x);
          int2.y = ZoneUtils.GetCellWidth(objectGeometryData.m_Size.z);
          float2 float2_2 = (float2) int2 * 8f;
          // ISSUE: reference to a compiler-generated field
          float2 offset = math.select((float2) 0.0f, (float2) 4f, (this.m_CellWidth + int2 & 1) != 0);
          float2 a = new float2(math.dot(x1, y), math.dot(x1, float2_1));
          float2 float2_3 = MathUtils.Snap(a, (float2) 8f, offset);
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_EdgeOffset != 0.0 && (objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) == Game.Objects.GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2_3 = math.select(float2_3, float2_3 + math.select((float2) this.m_EdgeOffset, (float2) -this.m_EdgeOffset, float2_3 > 0.0f), math.abs(math.abs(float2_3) - float2_2 * 0.5f) < 4f);
          }
          // ISSUE: reference to a compiler-generated field
          bool2 bool2 = math.abs(a - float2_3) < this.m_MaxDistance;
          if (!math.any(bool2))
            return;
          float2 x2 = math.select(a, float2_3, bool2);
          float2 x3 = transform.m_Position.xz + y * x2.x + float2_1 * x2.y;
          if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distance(x3, transform.m_Position.xz) > (double) float2_2.x * 0.5 + (double) this.m_Radius + 4.0)
              return;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (math.any(math.abs(x2) > float2_2 * 0.5f + this.m_Radius + 4f))
              return;
          }
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EdgeData.HasComponent(this.m_ControlPoint.m_OriginalEntity) && !this.m_NodeData.HasComponent(this.m_ControlPoint.m_OriginalEntity))
            controlPoint.m_OriginalEntity = Entity.Null;
          controlPoint.m_Direction = y;
          controlPoint.m_Position.xz = x3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoint.m_OriginalEntity != item || this.m_ControlPoint.m_ElementIndex.x != -1)
          {
            // ISSUE: reference to a compiler-generated field
            controlPoint.m_Position.y = this.m_ControlPoint.m_HitPosition.y;
          }
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
      }

      private struct ZoneIterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public float2 m_HitPosition;
        public float3 m_BestPosition;
        public float2 m_BestDirection;
        public float m_BestDistance;
        public ComponentLookup<Game.Zones.Block> m_ZoneBlockData;
        public BufferLookup<Cell> m_ZoneCells;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_ZoneBlockData[entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> zoneCell = this.m_ZoneCells[entity];
          // ISSUE: reference to a compiler-generated field
          int2 cellIndex = math.clamp(ZoneUtils.GetCellIndex(block, this.m_HitPosition), (int2) 0, block.m_Size - 1);
          float3 cellPosition1 = ZoneUtils.GetCellPosition(block, cellIndex);
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(cellPosition1.xz, this.m_HitPosition);
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 >= (double) this.m_BestDistance)
            return;
          if ((zoneCell[cellIndex.x + cellIndex.y * block.m_Size.x].m_State & CellFlags.Visible) != CellFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_BestPosition = cellPosition1;
            // ISSUE: reference to a compiler-generated field
            this.m_BestDirection = block.m_Direction;
            // ISSUE: reference to a compiler-generated field
            this.m_BestDistance = num1;
          }
          else
          {
            for (cellIndex.y = 0; cellIndex.y < block.m_Size.y; ++cellIndex.y)
            {
              for (cellIndex.x = 0; cellIndex.x < block.m_Size.x; ++cellIndex.x)
              {
                if ((zoneCell[cellIndex.x + cellIndex.y * block.m_Size.x].m_State & CellFlags.Visible) != CellFlags.None)
                {
                  float3 cellPosition2 = ZoneUtils.GetCellPosition(block, cellIndex);
                  // ISSUE: reference to a compiler-generated field
                  float num2 = math.distance(cellPosition2.xz, this.m_HitPosition);
                  // ISSUE: reference to a compiler-generated field
                  if ((double) num2 < (double) this.m_BestDistance)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_BestPosition = cellPosition2;
                    // ISSUE: reference to a compiler-generated field
                    this.m_BestDirection = block.m_Direction;
                    // ISSUE: reference to a compiler-generated field
                    this.m_BestDistance = num2;
                  }
                }
              }
            }
          }
        }
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Snap m_Snap;
        public float m_MaxDistance;
        public float m_NetSnapOffset;
        public float m_ObjectSnapOffset;
        public bool m_SnapCellLength;
        public NetData m_NetData;
        public NetGeometryData m_NetGeometryData;
        public ControlPoint m_ControlPoint;
        public ControlPoint m_BestSnapPosition;
        public NativeList<SnapLine> m_SnapLines;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<Game.Objects.Transform> m_TransformData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingData;
        public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
        public ComponentLookup<NetData> m_PrefabNetData;
        public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
        public BufferLookup<ConnectedEdge> m_ConnectedEdges;

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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & (Snap.ExistingGeometry | Snap.NearbyGeometry)) != Snap.None && this.m_OwnerData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner = this.m_OwnerData[entity];
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.HasComponent(owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated method
              this.SnapToNode(owner.m_Owner);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.ObjectSide) == Snap.None)
            return;
          // ISSUE: reference to a compiler-generated method
          this.SnapObjectSide(entity);
        }

        private void SnapToNode(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (entity == this.m_ControlPoint.m_OriginalEntity && (this.m_Snap & Snap.ExistingGeometry) != Snap.None || this.m_ConnectedEdges.HasBuffer(entity) && this.m_ConnectedEdges[entity].Length > 0)
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[entity];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabNetData.HasComponent(prefabRef.m_Prefab) || !NetUtils.CanConnect(this.m_PrefabNetData[prefabRef.m_Prefab], this.m_NetData))
            return;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_OriginalEntity = entity,
            m_Position = node.m_Position,
            m_Direction = math.mul(node.m_Rotation, new float3(0.0f, 0.0f, 1f)).xz
          };
          MathUtils.TryNormalize(ref controlPoint.m_Direction);
          float level = 1f;
          // ISSUE: reference to a compiler-generated field
          float num1 = math.distance(node.m_Position.xz, this.m_ControlPoint.m_HitPosition.xz);
          // ISSUE: reference to a compiler-generated field
          float num2 = this.m_NetGeometryData.m_DefaultWidth * 0.5f;
          NetGeometryData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData))
            num2 += componentData.m_DefaultWidth * 0.5f;
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 >= (double) num2 + (double) this.m_NetSnapOffset)
            return;
          // ISSUE: reference to a compiler-generated field
          if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0 && (double) num1 <= (double) num2 && (double) num1 <= (double) num2)
            level = 2f;
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
        }

        private void SnapObjectSide(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TransformData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Objects.Transform transform = this.m_TransformData[entity];
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ObjectGeometryData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
          if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
            return;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BuildingData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          float2 lotSize = (float2) this.m_BuildingData[prefabRef.m_Prefab].m_LotSize;
          objectGeometryData.m_Bounds.min.xz = lotSize * -4f;
          objectGeometryData.m_Bounds.max.xz = lotSize * 4f;
          // ISSUE: reference to a compiler-generated field
          bool snapCellLength = this.m_SnapCellLength;
          // ISSUE: reference to a compiler-generated field
          objectGeometryData.m_Bounds.min.xz -= this.m_ObjectSnapOffset;
          // ISSUE: reference to a compiler-generated field
          objectGeometryData.m_Bounds.max.xz += this.m_ObjectSnapOffset;
          Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, objectGeometryData.m_Bounds);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(baseCorners.ab, snapCellLength);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(baseCorners.bc, snapCellLength);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(baseCorners.cd, snapCellLength);
          // ISSUE: reference to a compiler-generated method
          this.CheckLine(baseCorners.da, snapCellLength);
        }

        private void CheckLine(Line3.Segment line, bool snapCellLength)
        {
          float t;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.Distance(line.xz, this.m_ControlPoint.m_HitPosition.xz, out t) >= (double) this.m_MaxDistance)
            return;
          if (snapCellLength)
            t = MathUtils.Snap(t, 8f / MathUtils.Length(line.xz));
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_Direction = math.normalizesafe(MathUtils.Tangent(line.xz)),
            m_Position = MathUtils.Position(line, t)
          };
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(1f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.HasComponent(this.m_ControlPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double num = (double) MathUtils.Distance(this.m_CurveData[this.m_ControlPoint.m_OriginalEntity].m_Bezier.xz, controlPoint.m_Position.xz, out controlPoint.m_CurvePosition);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_NodeData.HasComponent(this.m_ControlPoint.m_OriginalEntity))
            {
              controlPoint.m_OriginalEntity = Entity.Null;
              controlPoint.m_ElementIndex = (int2) -1;
            }
          }
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line.a, line.b), SnapLineFlags.Secondary));
        }
      }

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public bool m_EditorMode;
        public Bounds3 m_TotalBounds;
        public Bounds3 m_Bounds;
        public Snap m_Snap;
        public float m_SnapOffset;
        public float m_SnapDistance;
        public float m_Elevation;
        public float m_GuideLength;
        public float m_LegSnapWidth;
        public Bounds1 m_HeightRange;
        public NetData m_NetData;
        public RoadData m_PrefabRoadData;
        public NetGeometryData m_NetGeometryData;
        public LocalConnectData m_LocalConnectData;
        public ControlPoint m_ControlPoint;
        public ControlPoint m_BestSnapPosition;
        public NativeList<SnapLine> m_SnapLines;
        public TerrainHeightData m_TerrainHeightData;
        public WaterSurfaceData m_WaterSurfaceData;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<Game.Net.Edge> m_EdgeData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<Road> m_RoadData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NetData> m_PrefabNetData;
        public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
        public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
        public ComponentLookup<RoadComposition> m_RoadCompositionData;
        public BufferLookup<ConnectedEdge> m_ConnectedEdges;
        public BufferLookup<NetCompositionArea> m_PrefabCompositionAreas;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_TotalBounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_TotalBounds) || entity == this.m_ControlPoint.m_OriginalEntity && (this.m_Snap & Snap.ExistingGeometry) != Snap.None || MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) && (this.m_Snap & (Snap.ExistingGeometry | Snap.NearbyGeometry)) != Snap.None && this.HandleGeometry(entity) || (this.m_Snap & Snap.GuideLines) == Snap.None)
            return;
          // ISSUE: reference to a compiler-generated method
          this.HandleGuideLines(entity);
        }

        public void HandleGuideLines(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CurveData.HasComponent(entity))
            return;
          bool flag1 = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag2 = (this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) == (Game.Net.GeometryFlags) 0 && (this.m_PrefabRoadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0 && (this.m_Snap & Snap.CellLength) > Snap.None;
          // ISSUE: reference to a compiler-generated field
          float defaultWidth = this.m_NetGeometryData.m_DefaultWidth;
          float roadWidth = defaultWidth;
          // ISSUE: reference to a compiler-generated field
          float num1 = this.m_NetGeometryData.m_DefaultWidth * 0.5f;
          bool flag3 = false;
          bool flag4 = false;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          NetData netData2 = this.m_PrefabNetData[prefabRef.m_Prefab];
          NetGeometryData netGeometryData = new NetGeometryData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            netGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!NetUtils.CanConnect(this.m_NetData, netData2) || !this.m_EditorMode && (netGeometryData.m_Flags & Game.Net.GeometryFlags.Marker) != (Game.Net.GeometryFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Composition composition = this.m_CompositionData[entity];
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            float num2 = num1 + netCompositionData.m_Width * 0.5f;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) == (Game.Net.GeometryFlags) 0)
            {
              roadWidth = netGeometryData.m_DefaultWidth;
              // ISSUE: reference to a compiler-generated field
              if (this.m_RoadCompositionData.HasComponent(composition.m_Edge))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                flag1 = (this.m_RoadCompositionData[composition.m_Edge].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0 && (this.m_Snap & Snap.CellLength) > Snap.None;
                // ISSUE: reference to a compiler-generated field
                if (flag1 && this.m_RoadData.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  Road road = this.m_RoadData[entity];
                  flag3 = (road.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0;
                  flag4 = (road.m_Flags & Game.Net.RoadFlags.EndHalfAligned) != 0;
                }
              }
            }
          }
          int cellWidth1 = ZoneUtils.GetCellWidth(defaultWidth);
          int cellWidth2 = ZoneUtils.GetCellWidth(roadWidth);
          int num3;
          float num4;
          float num5;
          if (flag2)
          {
            num3 = 1 + math.abs(cellWidth2 - cellWidth1);
            num4 = (float) (num3 - 1) * -4f;
            num5 = 8f;
          }
          else
          {
            float num6 = math.abs(roadWidth - defaultWidth);
            if ((double) num6 > 1.6000000238418579)
            {
              num3 = 3;
              num4 = num6 * -0.5f;
              num5 = num6 * 0.5f;
            }
            else
            {
              num3 = 1;
              num4 = 0.0f;
              num5 = 0.0f;
            }
          }
          float num7;
          float num8;
          float num9;
          float num10;
          if (flag1)
          {
            num7 = math.select(0.0f, 4f, ((cellWidth1 ^ cellWidth2) & 1) != 0 ^ flag3);
            num8 = math.select(0.0f, 4f, ((cellWidth1 ^ cellWidth2) & 1) != 0 ^ flag4);
            float num11 = math.select(num7, -num7, cellWidth1 > cellWidth2);
            float num12 = math.select(num8, -num8, cellWidth1 > cellWidth2);
            num9 = num11 + 8f * (float) ((math.max(2, cellWidth2) - math.max(2, cellWidth1)) / 2);
            num10 = num12 + 8f * (float) ((math.max(2, cellWidth2) - math.max(2, cellWidth1)) / 2);
          }
          else
          {
            num7 = 0.0f;
            num8 = 0.0f;
            num9 = 0.0f;
            num10 = 0.0f;
          }
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[entity];
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge1 = this.m_EdgeData[entity];
          float2 forward = -MathUtils.StartTangent(curve.m_Bezier).xz;
          float2 xz = MathUtils.EndTangent(curve.m_Bezier).xz;
          bool flag5 = MathUtils.TryNormalize(ref forward);
          bool flag6 = MathUtils.TryNormalize(ref xz);
          bool flag7 = flag5;
          if (flag5)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[edge1.m_Start];
            for (int index = 0; index < connectedEdge.Length; ++index)
            {
              Entity edge2 = connectedEdge[index].m_Edge;
              if (!(edge2 == entity))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge3 = this.m_EdgeData[edge2];
                if (edge3.m_Start == edge1.m_Start || edge3.m_End == edge1.m_Start)
                {
                  flag7 = false;
                  break;
                }
              }
            }
          }
          bool flag8 = flag6;
          if (flag6)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[edge1.m_End];
            for (int index = 0; index < connectedEdge.Length; ++index)
            {
              Entity edge4 = connectedEdge[index].m_Edge;
              if (!(edge4 == entity))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge5 = this.m_EdgeData[edge4];
                if (edge5.m_Start == edge1.m_End || edge5.m_End == edge1.m_End)
                {
                  flag8 = false;
                  break;
                }
              }
            }
          }
          if (!(flag5 | flag6))
            return;
          for (int index = 0; index < num3; ++index)
          {
            if (flag5)
            {
              float3 a = curve.m_Bezier.a;
              a.xz += MathUtils.Left(forward) * num4;
              Line3.Segment line1 = new Line3.Segment(a, a);
              // ISSUE: reference to a compiler-generated field
              line1.b.xz += forward * this.m_GuideLength;
              float t1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((double) MathUtils.Distance(line1.xz, this.m_ControlPoint.m_HitPosition.xz, out t1) < (double) this.m_SnapDistance)
              {
                // ISSUE: reference to a compiler-generated field
                ControlPoint controlPoint = this.m_ControlPoint with
                {
                  m_OriginalEntity = Entity.Null
                };
                // ISSUE: reference to a compiler-generated field
                if ((this.m_Snap & Snap.CellLength) != Snap.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t1 = MathUtils.Snap(this.m_GuideLength * t1, this.m_SnapDistance, num7) / this.m_GuideLength;
                }
                controlPoint.m_Position = MathUtils.Position(line1, t1);
                controlPoint.m_Direction = forward;
                // ISSUE: reference to a compiler-generated field
                controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
                // ISSUE: reference to a compiler-generated field
                ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line1.a, line1.b), SnapLineFlags.GuideLine));
              }
              if (index == 0 & flag7)
              {
                float3 float3 = a;
                float3.xz += forward * num9;
                Line3.Segment line2 = new Line3.Segment(float3, float3);
                // ISSUE: reference to a compiler-generated field
                line2.b.xz += MathUtils.Right(forward) * this.m_GuideLength;
                float t2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) MathUtils.Distance(line2.xz, this.m_ControlPoint.m_HitPosition.xz, out t2) < (double) this.m_SnapDistance)
                {
                  // ISSUE: reference to a compiler-generated field
                  ControlPoint controlPoint = this.m_ControlPoint with
                  {
                    m_OriginalEntity = Entity.Null
                  };
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_Snap & Snap.CellLength) != Snap.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    t2 = MathUtils.Snap(this.m_GuideLength * t2, this.m_SnapDistance) / this.m_GuideLength;
                  }
                  controlPoint.m_Position = MathUtils.Position(line2, t2);
                  controlPoint.m_Direction = MathUtils.Right(forward);
                  // ISSUE: reference to a compiler-generated field
                  controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line2.a, line2.b), SnapLineFlags.GuideLine));
                }
              }
              if (index == num3 - 1 & flag7)
              {
                float3 float3 = a;
                float3.xz += forward * num9;
                Line3.Segment line3 = new Line3.Segment(float3, float3);
                // ISSUE: reference to a compiler-generated field
                line3.b.xz += MathUtils.Left(forward) * this.m_GuideLength;
                float t3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) MathUtils.Distance(line3.xz, this.m_ControlPoint.m_HitPosition.xz, out t3) < (double) this.m_SnapDistance)
                {
                  // ISSUE: reference to a compiler-generated field
                  ControlPoint controlPoint = this.m_ControlPoint with
                  {
                    m_OriginalEntity = Entity.Null
                  };
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_Snap & Snap.CellLength) != Snap.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    t3 = MathUtils.Snap(this.m_GuideLength * t3, this.m_SnapDistance) / this.m_GuideLength;
                  }
                  controlPoint.m_Position = MathUtils.Position(line3, t3);
                  controlPoint.m_Direction = MathUtils.Left(forward);
                  // ISSUE: reference to a compiler-generated field
                  controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line3.a, line3.b), SnapLineFlags.GuideLine));
                }
              }
            }
            if (flag6)
            {
              float3 d = curve.m_Bezier.d;
              d.xz += MathUtils.Left(xz) * num4;
              Line3.Segment line4 = new Line3.Segment(d, d);
              // ISSUE: reference to a compiler-generated field
              line4.b.xz += xz * this.m_GuideLength;
              float t4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((double) MathUtils.Distance(line4.xz, this.m_ControlPoint.m_HitPosition.xz, out t4) < (double) this.m_SnapDistance)
              {
                // ISSUE: reference to a compiler-generated field
                ControlPoint controlPoint = this.m_ControlPoint with
                {
                  m_OriginalEntity = Entity.Null
                };
                // ISSUE: reference to a compiler-generated field
                if ((this.m_Snap & Snap.CellLength) != Snap.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  t4 = MathUtils.Snap(this.m_GuideLength * t4, this.m_SnapDistance, num8) / this.m_GuideLength;
                }
                controlPoint.m_Position = MathUtils.Position(line4, t4);
                controlPoint.m_Direction = xz;
                // ISSUE: reference to a compiler-generated field
                controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
                // ISSUE: reference to a compiler-generated field
                ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line4.a, line4.b), SnapLineFlags.GuideLine));
              }
              if (index == 0 & flag8)
              {
                float3 float3 = d;
                float3.xz += xz * num10;
                Line3.Segment line5 = new Line3.Segment(float3, float3);
                // ISSUE: reference to a compiler-generated field
                line5.b.xz += MathUtils.Right(xz) * this.m_GuideLength;
                float t5;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) MathUtils.Distance(line5.xz, this.m_ControlPoint.m_HitPosition.xz, out t5) < (double) this.m_SnapDistance)
                {
                  // ISSUE: reference to a compiler-generated field
                  ControlPoint controlPoint = this.m_ControlPoint with
                  {
                    m_OriginalEntity = Entity.Null
                  };
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_Snap & Snap.CellLength) != Snap.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    t5 = MathUtils.Snap(this.m_GuideLength * t5, this.m_SnapDistance) / this.m_GuideLength;
                  }
                  controlPoint.m_Position = MathUtils.Position(line5, t5);
                  controlPoint.m_Direction = MathUtils.Right(xz);
                  // ISSUE: reference to a compiler-generated field
                  controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line5.a, line5.b), SnapLineFlags.GuideLine));
                }
              }
              if (index == num3 - 1 & flag8)
              {
                float3 float3 = d;
                float3.xz += xz * num10;
                Line3.Segment line6 = new Line3.Segment(float3, float3);
                // ISSUE: reference to a compiler-generated field
                line6.b.xz += MathUtils.Left(xz) * this.m_GuideLength;
                float t6;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) MathUtils.Distance(line6.xz, this.m_ControlPoint.m_HitPosition.xz, out t6) < (double) this.m_SnapDistance)
                {
                  // ISSUE: reference to a compiler-generated field
                  ControlPoint controlPoint = this.m_ControlPoint with
                  {
                    m_OriginalEntity = Entity.Null
                  };
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_Snap & Snap.CellLength) != Snap.None)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    t6 = MathUtils.Snap(this.m_GuideLength * t6, this.m_SnapDistance) / this.m_GuideLength;
                  }
                  controlPoint.m_Position = MathUtils.Position(line6, t6);
                  controlPoint.m_Direction = MathUtils.Left(xz);
                  // ISSUE: reference to a compiler-generated field
                  controlPoint.m_SnapPriority = ToolUtils.CalculateSnapPriority(0.0f, 1f, this.m_ControlPoint.m_HitPosition.xz, controlPoint.m_Position.xz, controlPoint.m_Direction);
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint, NetUtils.StraightCurve(line6.a, line6.b), SnapLineFlags.GuideLine));
                }
              }
            }
            num4 += num5;
          }
        }

        public bool HandleGeometry(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoint with
          {
            m_OriginalEntity = entity
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = this.m_NetGeometryData.m_DefaultWidth * 0.5f + this.m_SnapOffset;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab) && (this.m_NetGeometryData.m_Flags & ~this.m_PrefabGeometryData[prefabRef.m_Prefab].m_Flags & Game.Net.GeometryFlags.StandingNodes) != (Game.Net.GeometryFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            num = this.m_LegSnapWidth * 0.5f + this.m_SnapOffset;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.HasBuffer(entity))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NodeData[entity];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[entity];
            for (int index = 0; index < connectedEdge.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge = this.m_EdgeData[connectedEdge[index].m_Edge];
              if (edge.m_Start == entity || edge.m_End == entity)
                return false;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              NetGeometryData netGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
              num += netGeometryData.m_DefaultWidth * 0.5f;
            }
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distance(node.m_Position.xz, this.m_ControlPoint.m_HitPosition.xz) >= (double) num)
              return false;
            controlPoint.m_HitPosition.y = node.m_Position.y;
            // ISSUE: reference to a compiler-generated method
            return this.HandleGeometry(controlPoint, prefabRef);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CurveData.HasComponent(entity))
            return false;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[this.m_CompositionData[entity].m_Edge];
            num += netCompositionData.m_Width * 0.5f;
          }
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.Distance(curve.m_Bezier.xz, this.m_ControlPoint.m_HitPosition.xz, out controlPoint.m_CurvePosition) >= (double) num)
            return false;
          controlPoint.m_HitPosition.y = MathUtils.Position(curve.m_Bezier, controlPoint.m_CurvePosition).y;
          // ISSUE: reference to a compiler-generated method
          return this.HandleGeometry(controlPoint, prefabRef);
        }

        public bool HandleGeometry(ControlPoint controlPoint, PrefabRef prefabRef)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabNetData.HasComponent(prefabRef.m_Prefab))
            return false;
          // ISSUE: reference to a compiler-generated field
          NetData netData1 = this.m_PrefabNetData[prefabRef.m_Prefab];
          bool snapAdded = false;
          bool flag = true;
          bool allowEdgeSnap = true;
          float y = controlPoint.m_HitPosition.y;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          controlPoint.m_HitPosition.y = (double) this.m_Elevation >= 0.0 ? WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, controlPoint.m_HitPosition) + this.m_Elevation : TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, controlPoint.m_HitPosition) + this.m_Elevation;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (!MathUtils.Intersect(this.m_NetGeometryData.m_DefaultHeightRange + controlPoint.m_HitPosition.y, netGeometryData.m_DefaultHeightRange + y))
            {
              flag = false;
              allowEdgeSnap = (netGeometryData.m_Flags & Game.Net.GeometryFlags.NoEdgeConnection) == (Game.Net.GeometryFlags) 0;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (flag && !NetUtils.CanConnect(netData1, this.m_NetData) || (this.m_NetData.m_ConnectLayers & ~netData1.m_RequiredLayers & Layer.LaneEditor) != Layer.None || !MathUtils.Intersect(this.m_HeightRange, y - controlPoint.m_HitPosition.y))
            return snapAdded;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(controlPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedEdges.HasBuffer(controlPoint.m_OriginalEntity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[controlPoint.m_OriginalEntity];
              if (connectedEdge.Length != 0)
              {
                for (int index = 0; index < connectedEdge.Length; ++index)
                {
                  Entity edge1 = connectedEdge[index].m_Edge;
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.Edge edge2 = this.m_EdgeData[edge1];
                  if (!(edge2.m_Start != controlPoint.m_OriginalEntity) || !(edge2.m_End != controlPoint.m_OriginalEntity))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.HandleCurve(controlPoint, edge1, allowEdgeSnap, ref snapAdded);
                  }
                }
                return snapAdded;
              }
            }
            ControlPoint snapPosition = controlPoint;
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NodeData[controlPoint.m_OriginalEntity];
            snapPosition.m_Position = node.m_Position;
            snapPosition.m_Direction = math.mul(node.m_Rotation, new float3(0.0f, 0.0f, 1f)).xz;
            MathUtils.TryNormalize(ref snapPosition.m_Direction);
            float level = 1f;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              float num = this.m_NetGeometryData.m_DefaultWidth * 0.5f;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                NetGeometryData netGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
                num += netGeometryData.m_DefaultWidth * 0.5f;
              }
              if ((double) math.distance(node.m_Position.xz, controlPoint.m_HitPosition.xz) <= (double) num)
                level = 2f;
            }
            snapPosition.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, controlPoint.m_HitPosition.xz, snapPosition.m_Position.xz, snapPosition.m_Direction);
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, snapPosition);
            snapAdded = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(controlPoint.m_OriginalEntity))
            {
              // ISSUE: reference to a compiler-generated method
              this.HandleCurve(controlPoint, controlPoint.m_OriginalEntity, allowEdgeSnap, ref snapAdded);
            }
          }
          return snapAdded;
        }

        private bool SnapSegmentAreas(
          ControlPoint controlPoint,
          NetCompositionData prefabCompositionData,
          DynamicBuffer<NetCompositionArea> areas,
          Game.Net.Segment segment,
          ref bool snapAdded)
        {
          bool flag = false;
          for (int index = 0; index < areas.Length; ++index)
          {
            NetCompositionArea area = areas[index];
            // ISSUE: reference to a compiler-generated field
            if ((area.m_Flags & NetAreaFlags.Buildable) != (NetAreaFlags) 0 && (double) this.m_LegSnapWidth * 0.5 < (double) (area.m_Width * 0.51f))
            {
              flag = true;
              Bezier4x3 curve = MathUtils.Lerp(segment.m_Left, segment.m_Right, (float) ((double) area.m_Position.x / (double) prefabCompositionData.m_Width + 0.5));
              float t;
              double num1 = (double) MathUtils.Distance(curve.xz, controlPoint.m_HitPosition.xz, out t);
              ControlPoint controlPoint1 = controlPoint with
              {
                m_Position = MathUtils.Position(curve, t),
                m_Direction = math.normalizesafe(MathUtils.Tangent(curve, t).xz)
              };
              if ((area.m_Flags & NetAreaFlags.Invert) != (NetAreaFlags) 0)
                controlPoint1.m_Direction = -controlPoint1.m_Direction;
              float3 float3 = MathUtils.Position(MathUtils.Lerp(segment.m_Left, segment.m_Right, (float) ((double) area.m_SnapPosition.x / (double) prefabCompositionData.m_Width + 0.5)), t);
              // ISSUE: reference to a compiler-generated field
              float maxLength = math.max(0.0f, math.min(area.m_Width * 0.5f, math.abs(area.m_SnapPosition.x - area.m_Position.x) + area.m_SnapWidth * 0.5f) - this.m_LegSnapWidth * 0.5f);
              controlPoint1.m_Position.xz += MathUtils.ClampLength(float3.xz - controlPoint1.m_Position.xz, maxLength);
              controlPoint1.m_Position.y += area.m_Position.y;
              float level = 1f;
              // ISSUE: reference to a compiler-generated field
              double num2 = (double) prefabCompositionData.m_Width * 0.5 - (double) math.abs(area.m_Position.x) + (double) this.m_LegSnapWidth * 0.5;
              if (num1 <= num2)
                level = 2f;
              controlPoint1.m_Rotation = ToolUtils.CalculateRotation(controlPoint1.m_Direction);
              controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
              // ISSUE: reference to a compiler-generated field
              ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, curve, NetToolSystem.SnapJob.GetSnapLineFlags(this.m_NetGeometryData.m_Flags)));
              snapAdded = true;
            }
          }
          return flag;
        }

        private void HandleCurve(
          ControlPoint controlPoint,
          Entity curveEntity,
          bool allowEdgeSnap,
          ref bool snapAdded)
        {
          bool flag1 = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag2 = (this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) == (Game.Net.GeometryFlags) 0 && (this.m_PrefabRoadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0 && (this.m_Snap & Snap.CellLength) > Snap.None;
          // ISSUE: reference to a compiler-generated field
          float defaultWidth = this.m_NetGeometryData.m_DefaultWidth;
          float roadWidth = defaultWidth;
          // ISSUE: reference to a compiler-generated field
          float num1 = this.m_NetGeometryData.m_DefaultWidth * 0.5f;
          bool2 bool2 = (bool2) false;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[curveEntity];
          NetGeometryData netGeometryData = new NetGeometryData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            netGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.HasComponent(curveEntity))
          {
            // ISSUE: reference to a compiler-generated field
            Composition composition = this.m_CompositionData[curveEntity];
            // ISSUE: reference to a compiler-generated field
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            num1 += prefabCompositionData.m_Width * 0.5f;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) == (Game.Net.GeometryFlags) 0)
            {
              roadWidth = netGeometryData.m_DefaultWidth;
              // ISSUE: reference to a compiler-generated field
              if (this.m_RoadCompositionData.HasComponent(composition.m_Edge))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                flag1 = (this.m_RoadCompositionData[composition.m_Edge].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != (Game.Prefabs.RoadFlags) 0 && (this.m_Snap & Snap.CellLength) > Snap.None;
                // ISSUE: reference to a compiler-generated field
                if (flag1 && this.m_RoadData.HasComponent(curveEntity))
                {
                  // ISSUE: reference to a compiler-generated field
                  Road road = this.m_RoadData[curveEntity];
                  bool2.x = (road.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0;
                  bool2.y = (road.m_Flags & Game.Net.RoadFlags.EndHalfAligned) != 0;
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.SnapToNetAreas) != (Game.Net.GeometryFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<NetCompositionArea> prefabCompositionArea = this.m_PrefabCompositionAreas[composition.m_Edge];
              // ISSUE: reference to a compiler-generated field
              EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[curveEntity];
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (this.SnapSegmentAreas(controlPoint, prefabCompositionData, prefabCompositionArea, edgeGeometry.m_Start, ref snapAdded) | this.SnapSegmentAreas(controlPoint, prefabCompositionData, prefabCompositionArea, edgeGeometry.m_End, ref snapAdded))
                return;
            }
          }
          int num2;
          float num3;
          float num4;
          if (flag2)
          {
            int cellWidth = ZoneUtils.GetCellWidth(defaultWidth);
            num2 = 1 + math.abs(ZoneUtils.GetCellWidth(roadWidth) - cellWidth);
            num3 = (float) (num2 - 1) * -4f;
            num4 = 8f;
          }
          else
          {
            float num5 = math.abs(roadWidth - defaultWidth);
            if ((double) num5 > 1.6000000238418579)
            {
              num2 = 3;
              num3 = num5 * -0.5f;
              num4 = num5 * 0.5f;
            }
            else
            {
              num2 = 1;
              num3 = 0.0f;
              num4 = 0.0f;
            }
          }
          float num6 = !flag1 ? 0.0f : math.select(0.0f, 4f, ((ZoneUtils.GetCellWidth(defaultWidth) ^ ZoneUtils.GetCellWidth(roadWidth)) & 1) != 0 ^ bool2.x);
          // ISSUE: reference to a compiler-generated field
          Curve curve1 = this.m_CurveData[curveEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(curveEntity) && !this.m_EditorMode)
            allowEdgeSnap = false;
          float2 x = math.normalizesafe(MathUtils.Left(MathUtils.StartTangent(curve1.m_Bezier).xz));
          float2 float2 = math.normalizesafe(MathUtils.Left(curve1.m_Bezier.c.xz - curve1.m_Bezier.b.xz));
          float2 y = math.normalizesafe(MathUtils.Left(MathUtils.EndTangent(curve1.m_Bezier).xz));
          bool flag3 = (double) math.dot(x, float2) > 0.99984771013259888 && (double) math.dot(float2, y) > 0.99984771013259888;
          for (int index = 0; index < num2; ++index)
          {
            Bezier4x3 curve2;
            if ((double) math.abs(num3) < 0.079999998211860657)
              curve2 = curve1.m_Bezier;
            else if (flag3)
            {
              curve2 = curve1.m_Bezier;
              curve2.a.xz += x * num3;
              curve2.b.xz += math.lerp(x, y, 0.333333343f) * num3;
              curve2.c.xz += math.lerp(x, y, 0.6666667f) * num3;
              curve2.d.xz += y * num3;
            }
            else
              curve2 = NetUtils.OffsetCurveLeftSmooth(curve1.m_Bezier, (float2) num3);
            float t;
            // ISSUE: reference to a compiler-generated field
            float num7 = (this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0 ? MathUtils.Distance(curve2.xz, controlPoint.m_HitPosition.xz, out t) : NetUtils.ExtendedDistance(curve2.xz, controlPoint.m_HitPosition.xz, out t);
            ControlPoint controlPoint1 = controlPoint;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_Snap & Snap.CellLength) != Snap.None)
            {
              float num8 = math.fmod(MathUtils.Length(curve2.xz) + math.select(0.0f, 4f, bool2.x != bool2.y) + 0.1f, 8f) * 0.5f;
              // ISSUE: reference to a compiler-generated field
              float distance = MathUtils.Snap(NetUtils.ExtendedLength(curve2.xz, t), this.m_SnapDistance, num6 + num8);
              t = NetUtils.ExtendedClampLength(curve2.xz, distance);
              // ISSUE: reference to a compiler-generated field
              if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
                t = math.saturate(t);
              controlPoint1.m_CurvePosition = t;
            }
            else
            {
              t = math.saturate(t);
              if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0)
              {
                float distance = MathUtils.Snap(NetUtils.ExtendedLength(curve2.xz, t), 4f);
                controlPoint1.m_CurvePosition = NetUtils.ExtendedClampLength(curve2.xz, distance);
              }
              else
              {
                if ((double) t >= 0.5)
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((double) math.distance(curve2.d.xz, controlPoint.m_HitPosition.xz) < (double) this.m_SnapOffset)
                    t = 1f;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((double) math.distance(curve2.a.xz, controlPoint.m_HitPosition.xz) < (double) this.m_SnapOffset)
                    t = 0.0f;
                }
                controlPoint1.m_CurvePosition = t;
              }
            }
            if (!allowEdgeSnap && (double) t > 0.0 && (double) t < 1.0)
            {
              if ((double) t >= 0.5)
              {
                // ISSUE: reference to a compiler-generated field
                if ((double) math.distance(curve2.d.xz, controlPoint.m_HitPosition.xz) < (double) num1 + (double) this.m_SnapOffset)
                {
                  t = 1f;
                  controlPoint1.m_CurvePosition = 1f;
                }
                else
                  continue;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if ((double) math.distance(curve2.a.xz, controlPoint.m_HitPosition.xz) < (double) num1 + (double) this.m_SnapOffset)
                {
                  t = 0.0f;
                  controlPoint1.m_CurvePosition = 0.0f;
                }
                else
                  continue;
              }
            }
            float3 tangent;
            NetUtils.ExtendedPositionAndTangent(curve2, t, out controlPoint1.m_Position, out tangent);
            controlPoint1.m_Direction = tangent.xz;
            MathUtils.TryNormalize(ref controlPoint1.m_Direction);
            float level = 1f;
            // ISSUE: reference to a compiler-generated field
            if ((this.m_NetGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0 && (double) num7 <= (double) num1)
              level = 2f;
            controlPoint1.m_SnapPriority = ToolUtils.CalculateSnapPriority(level, 1f, controlPoint.m_HitPosition.xz, controlPoint1.m_Position.xz, controlPoint1.m_Direction);
            // ISSUE: reference to a compiler-generated field
            ToolUtils.AddSnapPosition(ref this.m_BestSnapPosition, controlPoint1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ToolUtils.AddSnapLine(ref this.m_BestSnapPosition, this.m_SnapLines, new SnapLine(controlPoint1, curve2, NetToolSystem.SnapJob.GetSnapLineFlags(this.m_NetGeometryData.m_Flags)));
            snapAdded = true;
            num3 += num4;
          }
        }
      }
    }

    [BurstCompile]
    public struct FixControlPointsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public NetToolSystem.Mode m_Mode;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      public NativeList<ControlPoint> m_ControlPoints;

      public void Execute()
      {
        Entity entity1 = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Temp temp = nativeArray2[index2];
            Entity entity2 = nativeArray1[index2];
            if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              if (temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.FixControlPoints(temp.m_Original, Entity.Null);
              }
            }
            else if ((temp.m_Flags & (TempFlags.Replace | TempFlags.Combine)) != (TempFlags) 0)
            {
              if (temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.FixControlPoints(temp.m_Original, entity2);
              }
            }
            else
            {
              Game.Net.Edge componentData1;
              Game.Net.Edge componentData2;
              Temp componentData3;
              Temp componentData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((temp.m_Flags & TempFlags.Modify) != (TempFlags) 0 && this.m_EdgeData.TryGetComponent(entity2, out componentData1) && this.m_EdgeData.TryGetComponent(temp.m_Original, out componentData2) && (this.m_TempData.TryGetComponent(componentData1.m_Start, out componentData3) && componentData3.m_Original == componentData2.m_End || this.m_TempData.TryGetComponent(componentData1.m_End, out componentData4) && componentData4.m_Original == componentData2.m_Start))
              {
                // ISSUE: reference to a compiler-generated method
                this.InverseCurvePositions(temp.m_Original);
              }
            }
            if ((temp.m_Flags & TempFlags.IsLast) != (TempFlags) 0)
              entity1 = (temp.m_Flags & (TempFlags.Create | TempFlags.Replace)) == (TempFlags) 0 ? temp.m_Original : entity2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!(entity1 != Entity.Null) || this.m_Mode == NetToolSystem.Mode.Replace)
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ControlPoints.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoints[index] with
          {
            m_OriginalEntity = entity1
          };
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints[index] = controlPoint;
        }
      }

      private void FixControlPoints(Entity entity, Entity replace)
      {
        if (!(entity != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ControlPoints.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoints[index];
          if (controlPoint.m_OriginalEntity == entity)
          {
            controlPoint.m_OriginalEntity = replace;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints[index] = controlPoint;
          }
        }
      }

      private void InverseCurvePositions(Entity entity)
      {
        if (!(entity != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ControlPoints.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoints[index];
          if (controlPoint.m_OriginalEntity == entity)
          {
            controlPoint.m_CurvePosition = 1f - controlPoint.m_CurvePosition;
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints[index] = controlPoint;
          }
        }
      }
    }

    [BurstCompile]
    public struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_RemoveUpgrade;
      [ReadOnly]
      public bool m_LefthandTraffic;
      [ReadOnly]
      public NetToolSystem.Mode m_Mode;
      [ReadOnly]
      public int2 m_ParallelCount;
      [ReadOnly]
      public float m_ParallelOffset;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public AgeMask m_AgeMask;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [ReadOnly]
      public NativeList<NetToolSystem.UpgradeState> m_UpgradeStates;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Extension> m_ExtensionData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PlaceableData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<SubReplacement> m_SubReplacements;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_CachedNodes;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_PrefabSubObjects;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public BufferLookup<SubAreaNode> m_PrefabSubAreaNodes;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;
      [ReadOnly]
      public Entity m_NetPrefab;
      [ReadOnly]
      public Entity m_LanePrefab;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions = new NativeParallelHashMap<Entity, OwnerDefinition>();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateReplacement(ref ownerDefinitions);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          int length = this.m_ControlPoints.Length;
          if (length == 1)
          {
            // ISSUE: reference to a compiler-generated method
            this.CreateSinglePoint(ref ownerDefinitions);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            NetToolSystem.Mode mode = this.m_Mode;
            switch (mode)
            {
              case NetToolSystem.Mode.Straight:
                // ISSUE: reference to a compiler-generated method
                this.CreateStraightLine(ref ownerDefinitions, new int2(0, 1));
                break;
              case NetToolSystem.Mode.SimpleCurve:
                if (length == 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CreateStraightLine(ref ownerDefinitions, new int2(0, 1));
                  break;
                }
                // ISSUE: reference to a compiler-generated method
                this.CreateSimpleCurve(ref ownerDefinitions, 1);
                break;
              case NetToolSystem.Mode.ComplexCurve:
                switch (length)
                {
                  case 2:
                    // ISSUE: reference to a compiler-generated method
                    this.CreateStraightLine(ref ownerDefinitions, new int2(0, 1));
                    break;
                  case 3:
                    // ISSUE: reference to a compiler-generated method
                    this.CreateSimpleCurve(ref ownerDefinitions, 1);
                    break;
                  default:
                    // ISSUE: reference to a compiler-generated method
                    this.CreateComplexCurve(ref ownerDefinitions);
                    break;
                }
                break;
              case NetToolSystem.Mode.Continuous:
                if (length == 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CreateStraightLine(ref ownerDefinitions, new int2(0, 1));
                  break;
                }
                // ISSUE: reference to a compiler-generated method
                this.CreateContinuousCurve(ref ownerDefinitions);
                break;
              case NetToolSystem.Mode.Grid:
                if (length == 2)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CreateStraightLine(ref ownerDefinitions, new int2(0, 1));
                  break;
                }
                // ISSUE: reference to a compiler-generated method
                this.CreateGrid(ref ownerDefinitions);
                break;
            }
          }
        }
        if (!ownerDefinitions.IsCreated)
          return;
        ownerDefinitions.Dispose();
      }

      private bool GetLocalCurve(
        NetCourse course,
        OwnerDefinition ownerDefinition,
        out LocalCurveCache localCurveCache)
      {
        Game.Objects.Transform inverseParentTransform = ObjectUtils.InverseTransform(new Game.Objects.Transform(ownerDefinition.m_Position, ownerDefinition.m_Rotation));
        localCurveCache = new LocalCurveCache();
        localCurveCache.m_Curve.a = ObjectUtils.WorldToLocal(inverseParentTransform, course.m_Curve.a);
        localCurveCache.m_Curve.b = ObjectUtils.WorldToLocal(inverseParentTransform, course.m_Curve.b);
        localCurveCache.m_Curve.c = ObjectUtils.WorldToLocal(inverseParentTransform, course.m_Curve.c);
        localCurveCache.m_Curve.d = ObjectUtils.WorldToLocal(inverseParentTransform, course.m_Curve.d);
        return true;
      }

      private bool GetOwnerDefinition(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        Entity original,
        bool checkControlPoints,
        CoursePos startPos,
        CoursePos endPos,
        out OwnerDefinition ownerDefinition)
      {
        Entity entity1 = Entity.Null;
        ownerDefinition = new OwnerDefinition();
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorMode)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(original))
          {
            // ISSUE: reference to a compiler-generated field
            entity1 = this.m_OwnerData[original].m_Owner;
          }
          else if (checkControlPoints)
          {
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_ControlPoints.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_ControlPoints[index].m_OriginalEntity;
              // ISSUE: reference to a compiler-generated field
              if (this.m_NodeData.HasComponent(entity2))
                entity2 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              while (this.m_OwnerData.HasComponent(entity2) && !this.m_BuildingData.HasComponent(entity2))
              {
                // ISSUE: reference to a compiler-generated field
                entity2 = this.m_OwnerData[entity2].m_Owner;
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.HasComponent(entity2))
                {
                  // ISSUE: reference to a compiler-generated field
                  Temp temp = this.m_TempData[entity2];
                  if (temp.m_Original != Entity.Null)
                    entity2 = temp.m_Original;
                }
              }
              DynamicBuffer<InstalledUpgrade> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_InstalledUpgrades.TryGetBuffer(entity2, out bufferData) && bufferData.Length != 0)
                entity2 = bufferData[0].m_Upgrade;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(entity2) && this.m_SubNets.HasBuffer(entity2))
              {
                entity1 = entity2;
                break;
              }
            }
          }
        }
        OwnerDefinition ownerDefinition1;
        if (ownerDefinitions.IsCreated && ownerDefinitions.TryGetValue(entity1, out ownerDefinition1))
        {
          ownerDefinition = ownerDefinition1;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = this.m_TransformData[entity1];
            Entity owner = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              owner = this.m_OwnerData[entity1].m_Owner;
            }
            // ISSUE: reference to a compiler-generated method
            this.UpdateOwnerObject(owner, entity1, transform);
            // ISSUE: reference to a compiler-generated field
            ownerDefinition.m_Prefab = this.m_PrefabRefData[entity1].m_Prefab;
            ownerDefinition.m_Position = transform.m_Position;
            ownerDefinition.m_Rotation = transform.m_Rotation;
            if (!ownerDefinitions.IsCreated)
              ownerDefinitions = new NativeParallelHashMap<Entity, OwnerDefinition>(8, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            ownerDefinitions.Add(entity1, ownerDefinition);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((startPos.m_Flags & endPos.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) != (CoursePosFlags.IsFirst | CoursePosFlags.IsLast) && this.m_PrefabSubObjects.HasBuffer(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Prefabs.SubObject> prefabSubObject = this.m_PrefabSubObjects[this.m_NetPrefab];
          NativeParallelHashMap<Entity, int> selectedSpawnables = new NativeParallelHashMap<Entity, int>();
          for (int index = 0; index < prefabSubObject.Length; ++index)
          {
            Game.Prefabs.SubObject subObject = prefabSubObject[index];
            if ((subObject.m_Flags & SubObjectFlags.MakeOwner) != (SubObjectFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              Game.Objects.Transform courseObjectTransform = this.GetCourseObjectTransform(subObject, startPos, endPos);
              // ISSUE: reference to a compiler-generated method
              this.CreateCourseObject(subObject.m_Prefab, courseObjectTransform, ownerDefinition, ref selectedSpawnables);
              ownerDefinition.m_Prefab = subObject.m_Prefab;
              ownerDefinition.m_Position = courseObjectTransform.m_Position;
              ownerDefinition.m_Rotation = courseObjectTransform.m_Rotation;
              break;
            }
          }
          for (int index = 0; index < prefabSubObject.Length; ++index)
          {
            Game.Prefabs.SubObject subObject = prefabSubObject[index];
            if ((subObject.m_Flags & (SubObjectFlags.CoursePlacement | SubObjectFlags.MakeOwner)) == SubObjectFlags.CoursePlacement)
            {
              // ISSUE: reference to a compiler-generated method
              Game.Objects.Transform courseObjectTransform = this.GetCourseObjectTransform(subObject, startPos, endPos);
              // ISSUE: reference to a compiler-generated method
              this.CreateCourseObject(subObject.m_Prefab, courseObjectTransform, ownerDefinition, ref selectedSpawnables);
            }
          }
          if (selectedSpawnables.IsCreated)
            selectedSpawnables.Dispose();
        }
        return ownerDefinition.m_Prefab != Entity.Null;
      }

      private void UpdateOwnerObject(Entity owner, Entity original, Game.Objects.Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        Entity prefab = this.m_PrefabRefData[original].m_Prefab;
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Owner = owner;
        component1.m_Original = original;
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
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubNets(transform, prefab, original);
        // ISSUE: reference to a compiler-generated method
        this.UpdateSubAreas(transform, prefab, original);
      }

      private Game.Objects.Transform GetCourseObjectTransform(
        Game.Prefabs.SubObject subObject,
        CoursePos startPos,
        CoursePos endPos)
      {
        CoursePos coursePos = (subObject.m_Flags & SubObjectFlags.StartPlacement) != (SubObjectFlags) 0 ? startPos : endPos;
        Game.Objects.Transform courseObjectTransform;
        courseObjectTransform.m_Position = ObjectUtils.LocalToWorld(coursePos.m_Position, coursePos.m_Rotation, subObject.m_Position);
        courseObjectTransform.m_Rotation = math.mul(coursePos.m_Rotation, subObject.m_Rotation);
        return courseObjectTransform;
      }

      private void CreateCourseObject(
        Entity prefab,
        Game.Objects.Transform transform,
        OwnerDefinition ownerDefinition,
        ref NativeParallelHashMap<Entity, int> selectedSpawnables)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Prefab = prefab;
        ObjectDefinition component2 = new ObjectDefinition();
        component2.m_ParentMesh = -1;
        component2.m_Position = transform.m_Position;
        component2.m_Rotation = transform.m_Rotation;
        if (ownerDefinition.m_Prefab != Entity.Null)
        {
          Game.Objects.Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(new Game.Objects.Transform(ownerDefinition.m_Position, ownerDefinition.m_Rotation)), transform);
          component2.m_LocalPosition = local.m_Position;
          component2.m_LocalRotation = local.m_Rotation;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
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
        // ISSUE: reference to a compiler-generated method
        this.CreateSubNets(transform, prefab);
        // ISSUE: reference to a compiler-generated method
        this.CreateSubAreas(transform, prefab, ref selectedSpawnables);
      }

      private void CreateSubAreas(
        Game.Objects.Transform transform,
        Entity prefab,
        ref NativeParallelHashMap<Entity, int> selectedSpawnables)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubAreas.HasBuffer(prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Prefabs.SubArea> prefabSubArea = this.m_PrefabSubAreas[prefab];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SubAreaNode> prefabSubAreaNode = this.m_PrefabSubAreaNodes[prefab];
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(10000);
        for (int index1 = 0; index1 < prefabSubArea.Length; ++index1)
        {
          Game.Prefabs.SubArea subArea = prefabSubArea[index1];
          int num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EditorMode && this.m_PrefabPlaceholderElements.HasBuffer(subArea.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PlaceholderObjectElement> placeholderElement = this.m_PrefabPlaceholderElements[subArea.m_Prefab];
            if (!selectedSpawnables.IsCreated)
              selectedSpawnables = new NativeParallelHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            ComponentLookup<SpawnableObjectData> spawnableObjectData = this.m_PrefabSpawnableObjectData;
            NativeParallelHashMap<Entity, int> selectedSpawnables1 = selectedSpawnables;
            ref Unity.Mathematics.Random local1 = ref random;
            ref Entity local2 = ref subArea.m_Prefab;
            ref int local3 = ref num;
            if (!AreaUtils.SelectAreaPrefab(placeholderElement, spawnableObjectData, selectedSpawnables1, ref local1, out local2, out local3))
              continue;
          }
          else
            num = random.NextInt();
          // ISSUE: reference to a compiler-generated field
          AreaGeometryData areaGeometryData = this.m_PrefabAreaGeometryData[subArea.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component = new CreationDefinition();
          component.m_Prefab = subArea.m_Prefab;
          component.m_RandomSeed = num;
          if (areaGeometryData.m_Type != Game.Areas.AreaType.Lot)
            component.m_Flags |= CreationFlags.Hidden;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, new OwnerDefinition()
          {
            m_Prefab = prefab,
            m_Position = transform.m_Position,
            m_Rotation = transform.m_Rotation
          });
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> dynamicBuffer1 = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity);
          dynamicBuffer1.ResizeUninitialized(subArea.m_NodeRange.y - subArea.m_NodeRange.x + 1);
          DynamicBuffer<LocalNodeCache> dynamicBuffer2 = new DynamicBuffer<LocalNodeCache>();
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer2 = this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity);
            dynamicBuffer2.ResizeUninitialized(dynamicBuffer1.Length);
          }
          // ISSUE: reference to a compiler-generated method
          int index2 = ObjectToolBaseSystem.GetFirstNodeIndex(prefabSubAreaNode, subArea.m_NodeRange);
          int index3 = 0;
          for (int x = subArea.m_NodeRange.x; x <= subArea.m_NodeRange.y; ++x)
          {
            float3 position = prefabSubAreaNode[index2].m_Position;
            float3 world = ObjectUtils.LocalToWorld(transform, position);
            int parentMesh = prefabSubAreaNode[index2].m_ParentMesh;
            float elevation = math.select(float.MinValue, position.y, parentMesh >= 0);
            dynamicBuffer1[index3] = new Game.Areas.Node(world, elevation);
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorMode)
              dynamicBuffer2[index3] = new LocalNodeCache()
              {
                m_Position = position,
                m_ParentMesh = parentMesh
              };
            ++index3;
            if (++index2 == subArea.m_NodeRange.y)
              index2 = subArea.m_NodeRange.x;
          }
        }
      }

      private void CreateSubNets(Game.Objects.Transform transform, Entity prefab)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubNets.HasBuffer(prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Prefabs.SubNet> prefabSubNet = this.m_PrefabSubNets[prefab];
        NativeList<float4> nativeList = new NativeList<float4>(prefabSubNet.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < prefabSubNet.Length; ++index)
        {
          Game.Prefabs.SubNet subNet = prefabSubNet[index];
          float4 float4;
          if (subNet.m_NodeIndex.x >= 0)
          {
            while (nativeList.Length <= subNet.m_NodeIndex.x)
            {
              ref NativeList<float4> local1 = ref nativeList;
              float4 = new float4();
              ref float4 local2 = ref float4;
              local1.Add(in local2);
            }
            nativeList[subNet.m_NodeIndex.x] += new float4(subNet.m_Curve.a, 1f);
          }
          if (subNet.m_NodeIndex.y >= 0)
          {
            while (nativeList.Length <= subNet.m_NodeIndex.y)
            {
              ref NativeList<float4> local3 = ref nativeList;
              float4 = new float4();
              ref float4 local4 = ref float4;
              local3.Add(in local4);
            }
            nativeList[subNet.m_NodeIndex.y] += new float4(subNet.m_Curve.d, 1f);
          }
        }
        for (int index = 0; index < nativeList.Length; ++index)
          nativeList[index] /= math.max(1f, nativeList[index].w);
        for (int index = 0; index < prefabSubNet.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Prefabs.SubNet subNet = NetUtils.GetSubNet(prefabSubNet, index, this.m_LefthandTraffic, ref this.m_NetGeometryData);
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, new CreationDefinition()
          {
            m_Prefab = subNet.m_Prefab
          });
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, new OwnerDefinition()
          {
            m_Prefab = prefab,
            m_Position = transform.m_Position,
            m_Rotation = transform.m_Rotation
          });
          // ISSUE: reference to a compiler-generated method
          NetCourse component1 = new NetCourse()
          {
            m_Curve = this.TransformCurve(subNet.m_Curve, transform.m_Position, transform.m_Rotation)
          };
          component1.m_StartPosition.m_Position = component1.m_Curve.a;
          component1.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component1.m_Curve), transform.m_Rotation);
          component1.m_StartPosition.m_CourseDelta = 0.0f;
          component1.m_StartPosition.m_Elevation = (float2) subNet.m_Curve.a.y;
          component1.m_StartPosition.m_ParentMesh = subNet.m_ParentMesh.x;
          float4 float4;
          if (subNet.m_NodeIndex.x >= 0)
          {
            ref CoursePos local = ref component1.m_StartPosition;
            Game.Objects.Transform transform1 = transform;
            float4 = nativeList[subNet.m_NodeIndex.x];
            float3 xyz = float4.xyz;
            float3 world = ObjectUtils.LocalToWorld(transform1, xyz);
            local.m_Position = world;
          }
          component1.m_EndPosition.m_Position = component1.m_Curve.d;
          component1.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component1.m_Curve), transform.m_Rotation);
          component1.m_EndPosition.m_CourseDelta = 1f;
          component1.m_EndPosition.m_Elevation = (float2) subNet.m_Curve.d.y;
          component1.m_EndPosition.m_ParentMesh = subNet.m_ParentMesh.y;
          if (subNet.m_NodeIndex.y >= 0)
          {
            ref CoursePos local = ref component1.m_EndPosition;
            Game.Objects.Transform transform2 = transform;
            float4 = nativeList[subNet.m_NodeIndex.y];
            float3 xyz = float4.xyz;
            float3 world = ObjectUtils.LocalToWorld(transform2, xyz);
            local.m_Position = world;
          }
          component1.m_Length = MathUtils.Length(component1.m_Curve);
          component1.m_FixedIndex = -1;
          component1.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
          component1.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
          if (component1.m_StartPosition.m_Position.Equals(component1.m_EndPosition.m_Position))
          {
            component1.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
            component1.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity, component1);
          if (subNet.m_Upgrades != new CompositionFlags())
          {
            Upgraded component2 = new Upgraded()
            {
              m_Flags = subNet.m_Upgrades
            };
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Upgraded>(entity, component2);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, new LocalCurveCache()
            {
              m_Curve = subNet.m_Curve
            });
          }
        }
        nativeList.Dispose();
      }

      private Bezier4x3 TransformCurve(Bezier4x3 curve, float3 position, quaternion rotation)
      {
        curve.a = ObjectUtils.LocalToWorld(position, rotation, curve.a);
        curve.b = ObjectUtils.LocalToWorld(position, rotation, curve.b);
        curve.c = ObjectUtils.LocalToWorld(position, rotation, curve.c);
        curve.d = ObjectUtils.LocalToWorld(position, rotation, curve.d);
        return curve;
      }

      private void UpdateSubNets(Game.Objects.Transform transform, Entity prefab, Entity original)
      {
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace && this.m_UpgradeStates.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          nativeParallelHashSet = new NativeParallelHashSet<Entity>(this.m_UpgradeStates.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < this.m_UpgradeStates.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint1 = this.m_ControlPoints[index1 * 2 + 1];
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint2 = this.m_ControlPoints[index1 * 2 + 2];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[controlPoint1.m_OriginalEntity];
            for (int index2 = 0; index2 < connectedEdge.Length; ++index2)
            {
              Entity edge1 = connectedEdge[index2].m_Edge;
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge2 = this.m_EdgeData[edge1];
              if (edge2.m_Start == controlPoint1.m_OriginalEntity && edge2.m_End == controlPoint2.m_OriginalEntity)
                nativeParallelHashSet.Add(edge1);
              else if (edge2.m_End == controlPoint1.m_OriginalEntity && edge2.m_Start == controlPoint2.m_OriginalEntity)
                nativeParallelHashSet.Add(edge1);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.HasBuffer(original))
        {
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
              if (this.m_EdgeData.HasComponent(subNet2) && (!nativeParallelHashSet.IsCreated || !nativeParallelHashSet.Contains(subNet2)))
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
                // ISSUE: reference to a compiler-generated field
                NetCourse component2 = new NetCourse()
                {
                  m_Curve = this.m_CurveData[subNet2].m_Bezier
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
        if (!nativeParallelHashSet.IsCreated)
          return;
        nativeParallelHashSet.Dispose();
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

      private void UpdateSubAreas(Game.Objects.Transform transform, Entity prefab, Entity original)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubAreas.HasBuffer(original))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.SubArea> subArea = this.m_SubAreas[original];
        for (int index = 0; index < subArea.Length; ++index)
        {
          Entity area = subArea[index].m_Area;
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
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[area];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity).CopyFrom(areaNode.AsNativeArray());
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

      private void CreateReplacement(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_UpgradeStates.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[index1 * 2 + 1];
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint2 = this.m_ControlPoints[index1 * 2 + 2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          NetToolSystem.UpgradeState upgradeState = this.m_UpgradeStates[index1];
          if (!(controlPoint1.m_OriginalEntity == Entity.Null) && !(controlPoint2.m_OriginalEntity == Entity.Null))
          {
            if (controlPoint1.m_OriginalEntity == controlPoint2.m_OriginalEntity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (upgradeState.m_IsUpgrading || this.m_RemoveUpgrade)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.CreateUpgrade(ref ownerDefinitions, upgradeState, controlPoint1, index1 == 0, index1 == this.m_UpgradeStates.Length - 1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.CreateReplacement(ref ownerDefinitions, controlPoint1, index1 == 0, index1 == this.m_UpgradeStates.Length - 1);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[controlPoint1.m_OriginalEntity];
              for (int index2 = 0; index2 < connectedEdge.Length; ++index2)
              {
                Entity edge1 = connectedEdge[index2].m_Edge;
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge2 = this.m_EdgeData[edge1];
                if (edge2.m_Start == controlPoint1.m_OriginalEntity && edge2.m_End == controlPoint2.m_OriginalEntity)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (upgradeState.m_IsUpgrading || this.m_RemoveUpgrade)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.CreateUpgrade(ref ownerDefinitions, edge1, upgradeState, false, index1 == 0, index1 == this.m_UpgradeStates.Length - 1);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.CreateReplacement(ref ownerDefinitions, controlPoint1, controlPoint2, edge1, false, index1 == 0, index1 == this.m_UpgradeStates.Length - 1);
                  }
                }
                else if (edge2.m_End == controlPoint1.m_OriginalEntity && edge2.m_Start == controlPoint2.m_OriginalEntity)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (upgradeState.m_IsUpgrading || this.m_RemoveUpgrade)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.CreateUpgrade(ref ownerDefinitions, edge1, upgradeState, true, index1 == 0, index1 == this.m_UpgradeStates.Length - 1);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.CreateReplacement(ref ownerDefinitions, controlPoint1, controlPoint2, edge1, true, index1 == 0, index1 == this.m_UpgradeStates.Length - 1);
                  }
                }
              }
            }
          }
        }
      }

      private void CreateReplacement(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        ControlPoint point,
        bool isStart,
        bool isEnd)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, new CreationDefinition()
        {
          m_Original = point.m_OriginalEntity,
          m_Prefab = this.m_NetPrefab,
          m_SubPrefab = this.m_LanePrefab
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        NetCourse netCourse = new NetCourse()
        {
          m_Curve = new Bezier4x3(point.m_Position, point.m_Position, point.m_Position, point.m_Position)
        };
        // ISSUE: reference to a compiler-generated method
        netCourse.m_StartPosition = this.GetCoursePos(netCourse.m_Curve, point, 0.0f);
        // ISSUE: reference to a compiler-generated method
        netCourse.m_EndPosition = this.GetCoursePos(netCourse.m_Curve, point, 1f);
        netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
        netCourse.m_FixedIndex = -1;
        if (isStart)
          netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        if (isEnd)
          netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, point.m_OriginalEntity, false, netCourse.m_StartPosition, netCourse.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        else
        {
          netCourse.m_StartPosition.m_ParentMesh = -1;
          netCourse.m_EndPosition.m_ParentMesh = -1;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
      }

      private void CreateReplacement(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        ControlPoint startPoint,
        ControlPoint endPoint,
        Entity edge,
        bool invert,
        bool isStart,
        bool isEnd)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component = new CreationDefinition();
        component.m_Original = edge;
        // ISSUE: reference to a compiler-generated field
        component.m_Prefab = this.m_NetPrefab;
        // ISSUE: reference to a compiler-generated field
        component.m_SubPrefab = this.m_LanePrefab;
        component.m_Flags |= CreationFlags.Align;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[edge];
        if (invert)
        {
          curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
          component.m_Flags |= CreationFlags.Invert;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        NetCourse netCourse = new NetCourse();
        if (startPoint.m_Position.Equals(curve.m_Bezier.a) && endPoint.m_Position.Equals(curve.m_Bezier.d))
        {
          netCourse.m_Curve = curve.m_Bezier;
          netCourse.m_Length = curve.m_Length;
          netCourse.m_FixedIndex = -1;
        }
        else
        {
          float3 startTangent = MathUtils.StartTangent(curve.m_Bezier);
          float3 float3 = MathUtils.EndTangent(curve.m_Bezier);
          startTangent = MathUtils.Normalize(startTangent, startTangent.xz);
          float3 endTangent = MathUtils.Normalize(float3, float3.xz);
          netCourse.m_Curve = NetUtils.FitCurve(startPoint.m_Position, startTangent, endTangent, endPoint.m_Position);
          netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
          netCourse.m_FixedIndex = -1;
        }
        netCourse.m_StartPosition.m_Entity = startPoint.m_OriginalEntity;
        netCourse.m_StartPosition.m_Position = startPoint.m_Position;
        netCourse.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(netCourse.m_Curve));
        netCourse.m_StartPosition.m_CourseDelta = 0.0f;
        netCourse.m_EndPosition.m_Entity = endPoint.m_OriginalEntity;
        netCourse.m_EndPosition.m_Position = endPoint.m_Position;
        netCourse.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(netCourse.m_Curve));
        netCourse.m_EndPosition.m_CourseDelta = 1f;
        if (isStart)
          netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        if (isEnd)
          netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, edge, false, netCourse.m_StartPosition, netCourse.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
      }

      private void CreateUpgrade(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        NetToolSystem.UpgradeState upgradeState,
        ControlPoint point,
        bool isStart,
        bool isEnd)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component = new CreationDefinition();
        component.m_Original = point.m_OriginalEntity;
        // ISSUE: reference to a compiler-generated field
        component.m_Prefab = this.m_PrefabRefData[point.m_OriginalEntity].m_Prefab;
        component.m_Flags |= CreationFlags.Align;
        // ISSUE: reference to a compiler-generated field
        if (!upgradeState.m_SkipFlags)
        {
          Upgraded componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradedData.TryGetComponent(point.m_OriginalEntity, out componentData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          componentData.m_Flags = componentData.m_Flags & ~upgradeState.m_RemoveFlags | upgradeState.m_AddFlags & ~upgradeState.m_OldFlags;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(entity, componentData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((upgradeState.m_OldFlags & ~upgradeState.m_RemoveFlags | upgradeState.m_AddFlags) != upgradeState.m_OldFlags)
            component.m_Flags |= CreationFlags.Upgrade | CreationFlags.Parent;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        NetCourse netCourse = new NetCourse()
        {
          m_Curve = new Bezier4x3(point.m_Position, point.m_Position, point.m_Position, point.m_Position)
        };
        // ISSUE: reference to a compiler-generated method
        netCourse.m_StartPosition = this.GetCoursePos(netCourse.m_Curve, point, 0.0f);
        // ISSUE: reference to a compiler-generated method
        netCourse.m_EndPosition = this.GetCoursePos(netCourse.m_Curve, point, 1f);
        netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
        netCourse.m_FixedIndex = -1;
        if (isStart)
          netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        if (isEnd)
          netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, point.m_OriginalEntity, false, netCourse.m_StartPosition, netCourse.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        else
        {
          netCourse.m_StartPosition.m_ParentMesh = -1;
          netCourse.m_EndPosition.m_ParentMesh = -1;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
      }

      private void CreateUpgrade(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        Entity edge,
        NetToolSystem.UpgradeState upgradeState,
        bool invert,
        bool isStart,
        bool isEnd)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component = new CreationDefinition();
        component.m_Original = edge;
        // ISSUE: reference to a compiler-generated field
        component.m_Prefab = this.m_PrefabRefData[edge].m_Prefab;
        component.m_Flags |= CreationFlags.Align;
        // ISSUE: reference to a compiler-generated field
        if (!upgradeState.m_SkipFlags)
        {
          Upgraded componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradedData.TryGetComponent(edge, out componentData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          componentData.m_Flags = componentData.m_Flags & ~upgradeState.m_RemoveFlags | upgradeState.m_AddFlags & ~upgradeState.m_OldFlags;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(entity, componentData);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubReplacement> dynamicBuffer = this.m_CommandBuffer.AddBuffer<SubReplacement>(entity);
          DynamicBuffer<SubReplacement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubReplacements.TryGetBuffer(edge, out bufferData))
          {
            for (int index = 0; index < bufferData.Length; ++index)
            {
              SubReplacement elem = bufferData[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (elem.m_Side != upgradeState.m_SubReplacementSide || elem.m_Type != upgradeState.m_SubReplacementType)
                dynamicBuffer.Add(elem);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (upgradeState.m_SubReplacementType != SubReplacementType.None && upgradeState.m_SubReplacementPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer.Add(new SubReplacement()
            {
              m_Prefab = upgradeState.m_SubReplacementPrefab,
              m_Type = upgradeState.m_SubReplacementType,
              m_Side = upgradeState.m_SubReplacementSide,
              m_AgeMask = this.m_AgeMask
            });
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((upgradeState.m_OldFlags & ~upgradeState.m_RemoveFlags | upgradeState.m_AddFlags) != upgradeState.m_OldFlags || upgradeState.m_SubReplacementType != SubReplacementType.None)
            component.m_Flags |= CreationFlags.Upgrade | CreationFlags.Parent;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        Game.Net.Edge edge1 = this.m_EdgeData[edge];
        // ISSUE: reference to a compiler-generated field
        NetCourse netCourse = new NetCourse()
        {
          m_Curve = this.m_CurveData[edge].m_Bezier
        };
        netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
        netCourse.m_FixedIndex = -1;
        netCourse.m_StartPosition.m_Entity = edge1.m_Start;
        netCourse.m_StartPosition.m_Position = netCourse.m_Curve.a;
        netCourse.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(netCourse.m_Curve));
        netCourse.m_StartPosition.m_CourseDelta = 0.0f;
        netCourse.m_EndPosition.m_Entity = edge1.m_End;
        netCourse.m_EndPosition.m_Position = netCourse.m_Curve.d;
        netCourse.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(netCourse.m_Curve));
        netCourse.m_EndPosition.m_CourseDelta = 1f;
        if (invert)
        {
          if (isStart)
            netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
          if (isEnd)
            netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
        }
        else
        {
          if (isStart)
            netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
          if (isEnd)
            netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        }
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, edge, false, netCourse.m_StartPosition, netCourse.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
      }

      private void CreateSinglePoint(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions)
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint = this.m_ControlPoints[0];
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        CreationDefinition component = new CreationDefinition();
        // ISSUE: reference to a compiler-generated field
        component.m_Prefab = this.m_NetPrefab;
        // ISSUE: reference to a compiler-generated field
        component.m_SubPrefab = this.m_LanePrefab;
        component.m_RandomSeed = random.NextInt();
        component.m_Flags |= CreationFlags.SubElevation;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        NetCourse netCourse = new NetCourse()
        {
          m_Curve = new Bezier4x3(controlPoint.m_Position, controlPoint.m_Position, controlPoint.m_Position, controlPoint.m_Position)
        };
        // ISSUE: reference to a compiler-generated method
        netCourse.m_StartPosition = this.GetCoursePos(netCourse.m_Curve, controlPoint, 0.0f);
        // ISSUE: reference to a compiler-generated method
        netCourse.m_EndPosition = this.GetCoursePos(netCourse.m_Curve, controlPoint, 1f);
        netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst | CoursePosFlags.IsLast | CoursePosFlags.IsRight | CoursePosFlags.IsLeft;
        netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst | CoursePosFlags.IsLast | CoursePosFlags.IsRight | CoursePosFlags.IsLeft;
        netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
        netCourse.m_FixedIndex = -1;
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, netCourse.m_StartPosition, netCourse.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        else
        {
          netCourse.m_StartPosition.m_ParentMesh = -1;
          netCourse.m_EndPosition.m_ParentMesh = -1;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
      }

      private void CreateStraightLine(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        int2 index)
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint1 = this.m_ControlPoints[index.x];
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[index.y];
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(this.m_NetPrefab) && (double) this.m_NetGeometryData[this.m_NetPrefab].m_MaxSlopeSteepness == 0.0)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetHeight(controlPoint1, ref controlPoint2);
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        CreationDefinition creationDefinition = new CreationDefinition();
        // ISSUE: reference to a compiler-generated field
        creationDefinition.m_Prefab = this.m_NetPrefab;
        // ISSUE: reference to a compiler-generated field
        creationDefinition.m_SubPrefab = this.m_LanePrefab;
        creationDefinition.m_RandomSeed = random.NextInt();
        creationDefinition.m_Flags |= CreationFlags.SubElevation;
        NetCourse course = new NetCourse()
        {
          m_Curve = NetUtils.StraightCurve(controlPoint1.m_Position, controlPoint2.m_Position)
        };
        // ISSUE: reference to a compiler-generated method
        course.m_StartPosition = this.GetCoursePos(course.m_Curve, controlPoint1, 0.0f);
        // ISSUE: reference to a compiler-generated method
        course.m_EndPosition = this.GetCoursePos(course.m_Curve, controlPoint2, 1f);
        course.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        course.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        if (course.m_StartPosition.m_Position.Equals(course.m_EndPosition.m_Position) && course.m_StartPosition.m_Entity.Equals(course.m_EndPosition.m_Entity))
        {
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
        }
        // ISSUE: reference to a compiler-generated field
        bool2 x = this.m_ParallelCount > 0;
        if (!x.x)
        {
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
        }
        if (!x.y)
        {
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
        }
        course.m_Length = MathUtils.Length(course.m_Curve);
        course.m_FixedIndex = -1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceableData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PlaceableNetData placeableNetData = this.m_PlaceableData[this.m_NetPrefab];
          // ISSUE: reference to a compiler-generated method
          if ((double) this.CalculatedInverseWeight(course, placeableNetData.m_PlacementFlags) < 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.InvertCourse(ref course);
          }
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, creationDefinition);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, course.m_StartPosition, course.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(course, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        else
        {
          course.m_StartPosition.m_ParentMesh = -1;
          course.m_EndPosition.m_ParentMesh = -1;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, course);
        if (!math.any(x))
          return;
        NativeParallelHashMap<float4, float3> nodeMap = new NativeParallelHashMap<float4, float3>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        this.CreateParallelCourses(creationDefinition, ownerDefinition, course, nodeMap);
        nodeMap.Dispose();
      }

      private void InvertCourse(ref NetCourse course)
      {
        course.m_Curve = MathUtils.Invert(course.m_Curve);
        CommonUtils.Swap<float3>(ref course.m_StartPosition.m_Position, ref course.m_EndPosition.m_Position);
        CommonUtils.Swap<quaternion>(ref course.m_StartPosition.m_Rotation, ref course.m_EndPosition.m_Rotation);
        CommonUtils.Swap<float2>(ref course.m_StartPosition.m_Elevation, ref course.m_EndPosition.m_Elevation);
        CommonUtils.Swap<CoursePosFlags>(ref course.m_StartPosition.m_Flags, ref course.m_EndPosition.m_Flags);
        CommonUtils.Swap<int>(ref course.m_StartPosition.m_ParentMesh, ref course.m_EndPosition.m_ParentMesh);
        quaternion a = quaternion.RotateY(3.14159274f);
        course.m_StartPosition.m_Rotation = math.mul(a, course.m_StartPosition.m_Rotation);
        course.m_EndPosition.m_Rotation = math.mul(a, course.m_EndPosition.m_Rotation);
        if ((course.m_StartPosition.m_Flags & (CoursePosFlags.IsRight | CoursePosFlags.IsLeft)) == CoursePosFlags.IsLeft)
        {
          course.m_StartPosition.m_Flags &= ~CoursePosFlags.IsLeft;
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
        }
        else if ((course.m_StartPosition.m_Flags & (CoursePosFlags.IsRight | CoursePosFlags.IsLeft)) == CoursePosFlags.IsRight)
        {
          course.m_StartPosition.m_Flags &= ~CoursePosFlags.IsRight;
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
        }
        if ((course.m_EndPosition.m_Flags & (CoursePosFlags.IsRight | CoursePosFlags.IsLeft)) == CoursePosFlags.IsLeft)
        {
          course.m_EndPosition.m_Flags &= ~CoursePosFlags.IsLeft;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
        }
        else
        {
          if ((course.m_EndPosition.m_Flags & (CoursePosFlags.IsRight | CoursePosFlags.IsLeft)) != CoursePosFlags.IsRight)
            return;
          course.m_EndPosition.m_Flags &= ~CoursePosFlags.IsRight;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
        }
      }

      private float CalculatedInverseWeight(NetCourse course, Game.Net.PlacementFlags placementFlags)
      {
        float num1 = 0.0f;
        if ((placementFlags & (Game.Net.PlacementFlags.FlowLeft | Game.Net.PlacementFlags.FlowRight)) != Game.Net.PlacementFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          int num2 = math.max(1, Mathf.RoundToInt(course.m_Length * this.m_WaterSurfaceData.scale.x));
          for (int index = 0; index < num2; ++index)
          {
            float t = ((float) index + 0.5f) / (float) num2;
            float3 worldPosition = MathUtils.Position(course.m_Curve, t);
            float3 float3 = MathUtils.Tangent(course.m_Curve, t);
            // ISSUE: reference to a compiler-generated field
            float a = math.dot(WaterUtils.SampleVelocity(ref this.m_WaterSurfaceData, worldPosition), math.normalizesafe(MathUtils.Right(float3.xz)));
            num1 += math.select(a, -a, (placementFlags & Game.Net.PlacementFlags.FlowLeft) != 0);
          }
        }
        return num1;
      }

      private void FixElevation(ref ControlPoint controlPoint)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PlaceableData.HasComponent(this.m_NetPrefab))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PlaceableNetData placeableNetData = this.m_PlaceableData[this.m_NetPrefab];
        if ((double) controlPoint.m_Elevation < (double) placeableNetData.m_ElevationRange.min)
        {
          controlPoint.m_Position.y += placeableNetData.m_ElevationRange.min - controlPoint.m_Elevation;
          controlPoint.m_Elevation = placeableNetData.m_ElevationRange.min;
          controlPoint.m_OriginalEntity = Entity.Null;
        }
        else
        {
          if ((double) controlPoint.m_Elevation <= (double) placeableNetData.m_ElevationRange.max)
            return;
          controlPoint.m_Position.y += placeableNetData.m_ElevationRange.max - controlPoint.m_Elevation;
          controlPoint.m_Elevation = placeableNetData.m_ElevationRange.max;
          controlPoint.m_OriginalEntity = Entity.Null;
        }
      }

      private void SetHeight(ControlPoint startPoint, ref ControlPoint controlPoint)
      {
        float y = startPoint.m_Position.y;
        controlPoint.m_Position.y = y;
      }

      private void CreateParallelCourses(
        CreationDefinition definitionData,
        OwnerDefinition ownerDefinition,
        NetCourse courseData,
        NativeParallelHashMap<float4, float3> nodeMap)
      {
        if (courseData.m_StartPosition.m_Position.Equals(courseData.m_EndPosition.m_Position))
          return;
        // ISSUE: reference to a compiler-generated field
        float parallelOffset = this.m_ParallelOffset;
        float elevationLimit = 1f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_NetGeometryData[this.m_NetPrefab];
          parallelOffset += netGeometryData.m_DefaultWidth;
          elevationLimit = netGeometryData.m_ElevationLimit;
        }
        NetCourse courseData1 = courseData;
        NetCourse courseData2 = courseData;
        // ISSUE: reference to a compiler-generated field
        for (int index = 1; index <= this.m_ParallelCount.x; ++index)
        {
          NetCourse courseData2_1 = courseData1 with
          {
            m_Curve = NetUtils.OffsetCurveLeftSmooth(courseData1.m_Curve, (float2) parallelOffset)
          };
          float4 key = new float4(courseData1.m_Curve.a, (float) -index);
          if (!nodeMap.TryAdd(key, courseData2_1.m_Curve.a))
            courseData2_1.m_Curve.a = nodeMap[key];
          key = new float4(courseData1.m_Curve.d, (float) -index);
          if (!nodeMap.TryAdd(key, courseData2_1.m_Curve.d))
            courseData2_1.m_Curve.d = nodeMap[key];
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(-index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourse(definitionData, ownerDefinition, courseData1, courseData2_1, parallelOffset, elevationLimit, (index & 1) != 0, index == this.m_ParallelCount.x, false, 0, ref random);
          courseData1 = courseData2_1;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 1; index <= this.m_ParallelCount.y; ++index)
        {
          NetCourse courseData2_2 = courseData2 with
          {
            m_Curve = NetUtils.OffsetCurveLeftSmooth(courseData2.m_Curve, (float2) -parallelOffset)
          };
          float4 key = new float4(courseData2.m_Curve.a, (float) index);
          if (!nodeMap.TryAdd(key, courseData2_2.m_Curve.a))
            courseData2_2.m_Curve.a = nodeMap[key];
          key = new float4(courseData2.m_Curve.d, (float) index);
          if (!nodeMap.TryAdd(key, courseData2_2.m_Curve.d))
            courseData2_2.m_Curve.d = nodeMap[key];
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(index);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourse(definitionData, ownerDefinition, courseData2, courseData2_2, -parallelOffset, elevationLimit, (index & 1) != 0, false, index == this.m_ParallelCount.y, 0, ref random);
          courseData2 = courseData2_2;
        }
      }

      private void CreateParallelCourse(
        CreationDefinition definitionData,
        OwnerDefinition ownerDefinition,
        NetCourse courseData,
        NetCourse courseData2,
        float parallelOffset,
        float elevationLimit,
        bool invert,
        bool isLeft,
        bool isRight,
        int level,
        ref Unity.Mathematics.Random random)
      {
        float num1 = math.abs(parallelOffset);
        if (++level >= 10 || (double) math.distance(courseData2.m_Curve.a.xz, courseData2.m_Curve.d.xz) < (double) num1 * 2.0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourse(definitionData, ownerDefinition, courseData2, elevationLimit, invert, isLeft, isRight, ref random);
        }
        else
        {
          float3 float3_1 = MathUtils.Position(courseData2.m_Curve, 0.5f);
          float t;
          double num2 = (double) MathUtils.Distance(courseData.m_Curve.xz, float3_1.xz, out t);
          float3 float3_2 = MathUtils.Position(courseData.m_Curve, t);
          float3 float3_3 = MathUtils.Tangent(courseData.m_Curve, t);
          float3_3 = MathUtils.Normalize(float3_3, float3_3.xz);
          float2 x = float3_3.zx * new float2(-parallelOffset, parallelOffset);
          double num3 = (double) num1;
          if ((double) math.abs((float) (num2 - num3)) > (double) num1 * 0.019999999552965164 || (double) math.dot(x, float3_1.xz - float3_2.xz) < 0.0)
          {
            float3_2.xz += x;
            float3 startTangent = MathUtils.StartTangent(courseData2.m_Curve);
            float3 float3_4 = MathUtils.EndTangent(courseData2.m_Curve);
            startTangent = MathUtils.Normalize(startTangent, startTangent.xz);
            float3 endTangent = MathUtils.Normalize(float3_4, float3_4.xz);
            NetCourse courseData2_1 = courseData2;
            NetCourse courseData2_2 = courseData2;
            courseData2_1.m_Curve = NetUtils.FitCurve(courseData2.m_Curve.a, startTangent, float3_3, float3_2);
            courseData2_1.m_EndPosition.m_Flags &= ~(CoursePosFlags.IsFirst | CoursePosFlags.IsLast);
            courseData2_1.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(float3_3);
            courseData2_2.m_Curve = NetUtils.FitCurve(float3_2, float3_3, endTangent, courseData2.m_Curve.d);
            courseData2_2.m_StartPosition.m_Flags &= ~(CoursePosFlags.IsFirst | CoursePosFlags.IsLast);
            courseData2_2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(float3_3);
            double num4 = (double) MathUtils.Length(courseData2_1.m_Curve);
            float s = math.saturate((float) (num4 / (num4 + (double) MathUtils.Length(courseData2_2.m_Curve))));
            courseData2_1.m_EndPosition.m_Elevation = math.lerp(courseData2.m_StartPosition.m_Elevation, courseData2.m_EndPosition.m_Elevation, s);
            courseData2_1.m_EndPosition.m_ParentMesh = math.select(courseData2.m_StartPosition.m_ParentMesh, -1, courseData2.m_StartPosition.m_ParentMesh != courseData2.m_EndPosition.m_ParentMesh);
            courseData2_2.m_StartPosition.m_Elevation = math.lerp(courseData2.m_StartPosition.m_Elevation, courseData2.m_EndPosition.m_Elevation, s);
            courseData2_2.m_StartPosition.m_ParentMesh = math.select(courseData2.m_StartPosition.m_ParentMesh, -1, courseData2.m_StartPosition.m_ParentMesh != courseData2.m_EndPosition.m_ParentMesh);
            // ISSUE: reference to a compiler-generated method
            this.CreateParallelCourse(definitionData, ownerDefinition, courseData, courseData2_1, parallelOffset, elevationLimit, invert, isLeft, isRight, level, ref random);
            // ISSUE: reference to a compiler-generated method
            this.CreateParallelCourse(definitionData, ownerDefinition, courseData, courseData2_2, parallelOffset, elevationLimit, invert, isLeft, isRight, level, ref random);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.CreateParallelCourse(definitionData, ownerDefinition, courseData2, elevationLimit, invert, isLeft, isRight, ref random);
          }
        }
      }

      private void CreateParallelCourse(
        CreationDefinition definitionData,
        OwnerDefinition ownerDefinition,
        NetCourse courseData,
        float elevationLimit,
        bool invert,
        bool isLeft,
        bool isRight,
        ref Unity.Mathematics.Random random)
      {
        // ISSUE: reference to a compiler-generated method
        this.LinearizeElevation(ref courseData.m_Curve);
        courseData.m_StartPosition.m_Position = courseData.m_Curve.a;
        courseData.m_StartPosition.m_Entity = Entity.Null;
        courseData.m_StartPosition.m_SplitPosition = 0.0f;
        courseData.m_StartPosition.m_Flags &= ~(CoursePosFlags.IsRight | CoursePosFlags.IsLeft);
        courseData.m_StartPosition.m_Flags |= CoursePosFlags.IsParallel;
        courseData.m_EndPosition.m_Position = courseData.m_Curve.d;
        courseData.m_EndPosition.m_Entity = Entity.Null;
        courseData.m_EndPosition.m_SplitPosition = 0.0f;
        courseData.m_EndPosition.m_Flags &= ~(CoursePosFlags.IsRight | CoursePosFlags.IsLeft);
        courseData.m_EndPosition.m_Flags |= CoursePosFlags.IsParallel;
        courseData.m_Length = MathUtils.Length(courseData.m_Curve);
        courseData.m_FixedIndex = -1;
        if ((double) courseData.m_StartPosition.m_Elevation.x > -(double) elevationLimit && (double) courseData.m_StartPosition.m_Elevation.x < (double) elevationLimit)
          courseData.m_StartPosition.m_Flags |= CoursePosFlags.FreeHeight;
        if ((double) courseData.m_EndPosition.m_Elevation.x > -(double) elevationLimit && (double) courseData.m_EndPosition.m_Elevation.x < (double) elevationLimit)
          courseData.m_EndPosition.m_Flags |= CoursePosFlags.FreeHeight;
        if (invert)
        {
          courseData.m_Curve = MathUtils.Invert(courseData.m_Curve);
          CommonUtils.Swap<float3>(ref courseData.m_StartPosition.m_Position, ref courseData.m_EndPosition.m_Position);
          CommonUtils.Swap<quaternion>(ref courseData.m_StartPosition.m_Rotation, ref courseData.m_EndPosition.m_Rotation);
          CommonUtils.Swap<float2>(ref courseData.m_StartPosition.m_Elevation, ref courseData.m_EndPosition.m_Elevation);
          CommonUtils.Swap<CoursePosFlags>(ref courseData.m_StartPosition.m_Flags, ref courseData.m_EndPosition.m_Flags);
          CommonUtils.Swap<int>(ref courseData.m_StartPosition.m_ParentMesh, ref courseData.m_EndPosition.m_ParentMesh);
          quaternion a = quaternion.RotateY(3.14159274f);
          courseData.m_StartPosition.m_Rotation = math.mul(a, courseData.m_StartPosition.m_Rotation);
          courseData.m_EndPosition.m_Rotation = math.mul(a, courseData.m_EndPosition.m_Rotation);
        }
        if (isLeft | isRight)
        {
          if (invert == isLeft)
          {
            courseData.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
            courseData.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
          }
          else
          {
            courseData.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
            courseData.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
          }
        }
        definitionData.m_RandomSeed ^= random.NextInt();
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, definitionData);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, courseData);
        if (!(ownerDefinition.m_Prefab != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
      }

      private void CreateSimpleCurve(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        int middleIndex)
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint1 = this.m_ControlPoints[0];
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[middleIndex];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint1);
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint2);
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[this.m_NetPrefab];
          if ((double) netGeometryData.m_MaxSlopeSteepness == 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint2);
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint3);
          }
        }
        else
          netGeometryData.m_DefaultWidth = 0.02f;
        float t1;
        float num1 = MathUtils.Distance(new Line2.Segment(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz), controlPoint3.m_Position.xz, out t1);
        float t2;
        float num2 = MathUtils.Distance(new Line2.Segment(controlPoint3.m_Position.xz, controlPoint2.m_Position.xz), controlPoint1.m_Position.xz, out t2);
        if ((double) num1 <= (double) netGeometryData.m_DefaultWidth * 0.75 && (double) num1 <= (double) num2)
        {
          float s = t1 * (float) (0.5 + (double) num1 / (double) netGeometryData.m_DefaultWidth * 0.66666668653488159);
          controlPoint2.m_Position = math.lerp(controlPoint1.m_Position, controlPoint2.m_Position, s);
        }
        else if ((double) num2 <= (double) netGeometryData.m_DefaultWidth * 0.75)
        {
          float s = t2 * (float) (0.5 + (double) num2 / (double) netGeometryData.m_DefaultWidth * 0.66666668653488159);
          controlPoint2.m_Position = math.lerp(controlPoint3.m_Position, controlPoint2.m_Position, s);
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        CreationDefinition creationDefinition = new CreationDefinition();
        // ISSUE: reference to a compiler-generated field
        creationDefinition.m_Prefab = this.m_NetPrefab;
        // ISSUE: reference to a compiler-generated field
        creationDefinition.m_SubPrefab = this.m_LanePrefab;
        creationDefinition.m_RandomSeed = random.NextInt();
        creationDefinition.m_Flags |= CreationFlags.SubElevation;
        NetCourse course = new NetCourse();
        course.m_Curve = NetUtils.FitCurve(new Line3.Segment(controlPoint1.m_Position, controlPoint2.m_Position), new Line3.Segment(controlPoint3.m_Position, controlPoint2.m_Position));
        // ISSUE: reference to a compiler-generated method
        this.LinearizeElevation(ref course.m_Curve);
        // ISSUE: reference to a compiler-generated method
        course.m_StartPosition = this.GetCoursePos(course.m_Curve, controlPoint1, 0.0f);
        // ISSUE: reference to a compiler-generated method
        course.m_EndPosition = this.GetCoursePos(course.m_Curve, controlPoint3, 1f);
        course.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        course.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
        if (course.m_StartPosition.m_Position.Equals(course.m_EndPosition.m_Position) && course.m_StartPosition.m_Entity.Equals(course.m_EndPosition.m_Entity))
        {
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
        }
        // ISSUE: reference to a compiler-generated field
        bool2 x = this.m_ParallelCount > 0;
        if (!x.x)
        {
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
        }
        if (!x.y)
        {
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
        }
        course.m_Length = MathUtils.Length(course.m_Curve);
        course.m_FixedIndex = -1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceableData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PlaceableNetData placeableNetData = this.m_PlaceableData[this.m_NetPrefab];
          // ISSUE: reference to a compiler-generated method
          if ((double) this.CalculatedInverseWeight(course, placeableNetData.m_PlacementFlags) < 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.InvertCourse(ref course);
          }
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, creationDefinition);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        OwnerDefinition ownerDefinition;
        // ISSUE: reference to a compiler-generated method
        if (this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, course.m_StartPosition, course.m_EndPosition, out ownerDefinition))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
          LocalCurveCache localCurveCache;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_EditorMode && this.GetLocalCurve(course, ownerDefinition, out localCurveCache))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
          }
        }
        else
        {
          course.m_StartPosition.m_ParentMesh = -1;
          course.m_EndPosition.m_ParentMesh = -1;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(entity, course);
        if (!math.any(x))
          return;
        NativeParallelHashMap<float4, float3> nodeMap = new NativeParallelHashMap<float4, float3>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        this.CreateParallelCourses(creationDefinition, ownerDefinition, course, nodeMap);
        nodeMap.Dispose();
      }

      private float GetCutPosition(NetGeometryData netGeometryData, float length, float t)
      {
        return (netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0 ? math.saturate(MathUtils.Snap((float) ((double) length * (double) t + 0.15999999642372131), 8f) / length) : t;
      }

      private void CreateGrid(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions)
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint1 = this.m_ControlPoints[0];
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint1);
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint2);
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[this.m_NetPrefab];
          if ((double) netGeometryData.m_MaxSlopeSteepness == 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint2);
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint3);
          }
        }
        bool flag1 = (double) math.dot(controlPoint3.m_Position.xz - controlPoint2.m_Position.xz, MathUtils.Right(controlPoint2.m_Direction)) > 0.0 ^ (double) math.dot(controlPoint3.m_Position.xz - controlPoint1.m_Position.xz, controlPoint2.m_Direction) < 0.0;
        float3 y = new float3(controlPoint2.m_Direction.x, 0.0f, controlPoint2.m_Direction.y);
        controlPoint2.m_Position = controlPoint1.m_Position + y * math.dot(controlPoint3.m_Position - controlPoint1.m_Position, y);
        float2 float2_1 = new float2(math.distance(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz), math.distance(controlPoint2.m_Position.xz, controlPoint3.m_Position.xz));
        float2 float2_2 = (netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) == (Game.Net.GeometryFlags) 0 ? netGeometryData.m_DefaultWidth * new float2(16f, 8f) : (float) ZoneUtils.GetCellWidth(netGeometryData.m_DefaultWidth) * 8f + new float2(192f, 96f);
        float2 float2_3 = math.max((float2) 1f, math.ceil((float2_1 - 0.16f) / float2_2));
        float2 float2_4 = float2_1 / float2_3;
        float2 float2_5 = float2_3 - math.select((float2) 0.0f, (float2) 1f, float2_4 < netGeometryData.m_DefaultWidth + 3f);
        int2 int2_1 = new int2(Mathf.RoundToInt(float2_5.x), Mathf.RoundToInt(float2_5.y));
        if (int2_1.y == 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateStraightLine(ref ownerDefinitions, new int2(0, 1));
        }
        else if (int2_1.x == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateStraightLine(ref ownerDefinitions, new int2(1, this.m_ControlPoints.Length - 1));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
          // ISSUE: reference to a compiler-generated method
          CoursePos coursePos1 = this.GetCoursePos(new Bezier4x3(controlPoint1.m_Position, controlPoint1.m_Position, controlPoint1.m_Position, controlPoint1.m_Position), controlPoint1, 0.0f);
          // ISSUE: reference to a compiler-generated method
          CoursePos coursePos2 = this.GetCoursePos(new Bezier4x3(controlPoint3.m_Position, controlPoint3.m_Position, controlPoint3.m_Position, controlPoint3.m_Position), controlPoint3, 1f);
          coursePos1.m_Flags |= CoursePosFlags.IsFirst;
          coursePos2.m_Flags |= CoursePosFlags.IsLast;
          OwnerDefinition ownerDefinition1;
          // ISSUE: reference to a compiler-generated method
          bool ownerDefinition2 = this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, coursePos1, coursePos2, out ownerDefinition1);
          float length1 = math.distance(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz);
          float length2 = math.distance(controlPoint2.m_Position.xz, controlPoint3.m_Position.xz);
          Line3.Segment line1 = new Line3.Segment(controlPoint1.m_Position, controlPoint1.m_Position + controlPoint3.m_Position - controlPoint2.m_Position);
          Line3.Segment line2 = new Line3.Segment(controlPoint2.m_Position, controlPoint3.m_Position);
          Line1.Segment line3 = new Line1.Segment(controlPoint1.m_Elevation, controlPoint2.m_Elevation);
          Line1.Segment line4 = new Line1.Segment(controlPoint2.m_Elevation, controlPoint3.m_Elevation);
          int2 int2_2;
          for (int2_2.y = 0; int2_2.y <= int2_1.y; ++int2_2.y)
          {
            // ISSUE: reference to a compiler-generated method
            float cutPosition1 = this.GetCutPosition(netGeometryData, length2, (float) int2_2.y / (float) int2_1.y);
            // ISSUE: reference to a compiler-generated method
            float cutPosition2 = this.GetCutPosition(netGeometryData, length2, (float) (int2_2.y + 1) / (float) int2_1.y);
            Line3.Segment line5;
            line5.a = MathUtils.Position(line1, cutPosition1);
            line5.b = MathUtils.Position(line2, cutPosition1);
            Line3.Segment line6;
            line6.a = MathUtils.Position(line1, cutPosition2);
            line6.b = MathUtils.Position(line2, cutPosition2);
            Line1.Segment line7;
            line7.a = MathUtils.Position(line3, cutPosition1);
            line7.b = MathUtils.Position(line4, cutPosition1);
            Line1.Segment line8;
            line8.a = MathUtils.Position(line3, cutPosition2);
            line8.b = MathUtils.Position(line4, cutPosition2);
            for (int2_2.x = 0; int2_2.x < int2_1.x; ++int2_2.x)
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity();
              CreationDefinition component = new CreationDefinition();
              // ISSUE: reference to a compiler-generated field
              component.m_Prefab = this.m_NetPrefab;
              // ISSUE: reference to a compiler-generated field
              component.m_SubPrefab = this.m_LanePrefab;
              component.m_RandomSeed = random.NextInt();
              component.m_Flags |= CreationFlags.SubElevation;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
              int num = math.all(int2_2 == 0) ? 1 : 0;
              bool flag2 = math.all(new int2(int2_2.x + 1, int2_2.y) == int2_1);
              bool flag3 = (int2_2.y & 1) == 1 || int2_2.y == int2_1.y;
              ControlPoint controlPoint4;
              if (num != 0)
              {
                controlPoint4 = controlPoint1;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                float cutPosition3 = this.GetCutPosition(netGeometryData, length1, (float) int2_2.x / (float) int2_1.x);
                controlPoint4 = new ControlPoint();
                controlPoint4.m_Rotation = controlPoint1.m_Rotation;
                controlPoint4.m_Position = MathUtils.Position(line5, cutPosition3);
                controlPoint4.m_Elevation = MathUtils.Position(line7, cutPosition3);
              }
              ControlPoint controlPoint5;
              if (flag2)
              {
                controlPoint5 = controlPoint3;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                float cutPosition4 = this.GetCutPosition(netGeometryData, length1, (float) (int2_2.x + 1) / (float) int2_1.x);
                controlPoint5 = new ControlPoint();
                controlPoint5.m_Rotation = controlPoint1.m_Rotation;
                controlPoint5.m_Position = MathUtils.Position(line5, cutPosition4);
                controlPoint5.m_Elevation = MathUtils.Position(line7, cutPosition4);
              }
              NetCourse netCourse = new NetCourse()
              {
                m_Curve = NetUtils.StraightCurve(controlPoint4.m_Position, controlPoint5.m_Position)
              };
              // ISSUE: reference to a compiler-generated method
              netCourse.m_StartPosition = this.GetCoursePos(netCourse.m_Curve, controlPoint4, 0.0f);
              // ISSUE: reference to a compiler-generated method
              netCourse.m_EndPosition = this.GetCoursePos(netCourse.m_Curve, controlPoint5, 1f);
              if (!ownerDefinition2)
              {
                netCourse.m_StartPosition.m_ParentMesh = -1;
                netCourse.m_EndPosition.m_ParentMesh = -1;
              }
              netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsGrid;
              netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsGrid;
              if (int2_2.y != 0)
              {
                netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsParallel;
                netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsParallel;
              }
              if (num == 0)
                netCourse.m_StartPosition.m_Flags |= CoursePosFlags.FreeHeight;
              if (!flag2)
                netCourse.m_EndPosition.m_Flags |= CoursePosFlags.FreeHeight;
              if (int2_2.y == 0 || int2_2.y == int2_1.y)
              {
                if (int2_2.x == 0)
                  netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
                if (int2_2.x + 1 == int2_1.x)
                  netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
                netCourse.m_StartPosition.m_Flags |= flag1 ? CoursePosFlags.IsLeft : CoursePosFlags.IsRight;
                netCourse.m_EndPosition.m_Flags |= flag1 ? CoursePosFlags.IsLeft : CoursePosFlags.IsRight;
              }
              if (flag3)
              {
                netCourse.m_Curve = MathUtils.Invert(netCourse.m_Curve);
                CommonUtils.Swap<Entity>(ref netCourse.m_StartPosition.m_Entity, ref netCourse.m_EndPosition.m_Entity);
                CommonUtils.Swap<float>(ref netCourse.m_StartPosition.m_SplitPosition, ref netCourse.m_EndPosition.m_SplitPosition);
                CommonUtils.Swap<float3>(ref netCourse.m_StartPosition.m_Position, ref netCourse.m_EndPosition.m_Position);
                CommonUtils.Swap<quaternion>(ref netCourse.m_StartPosition.m_Rotation, ref netCourse.m_EndPosition.m_Rotation);
                CommonUtils.Swap<float2>(ref netCourse.m_StartPosition.m_Elevation, ref netCourse.m_EndPosition.m_Elevation);
                CommonUtils.Swap<CoursePosFlags>(ref netCourse.m_StartPosition.m_Flags, ref netCourse.m_EndPosition.m_Flags);
                CommonUtils.Swap<int>(ref netCourse.m_StartPosition.m_ParentMesh, ref netCourse.m_EndPosition.m_ParentMesh);
                quaternion a = quaternion.RotateY(3.14159274f);
                netCourse.m_StartPosition.m_Rotation = math.mul(a, netCourse.m_StartPosition.m_Rotation);
                netCourse.m_EndPosition.m_Rotation = math.mul(a, netCourse.m_EndPosition.m_Rotation);
              }
              netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
              netCourse.m_FixedIndex = -1;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
              if (ownerDefinition1.m_Prefab != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition1);
                LocalCurveCache localCurveCache;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition1, out localCurveCache))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
                }
              }
            }
            if (int2_2.y != int2_1.y)
            {
              for (int2_2.x = 0; int2_2.x <= int2_1.x; ++int2_2.x)
              {
                // ISSUE: reference to a compiler-generated field
                Entity entity = this.m_CommandBuffer.CreateEntity();
                CreationDefinition component = new CreationDefinition();
                // ISSUE: reference to a compiler-generated field
                component.m_Prefab = this.m_NetPrefab;
                // ISSUE: reference to a compiler-generated field
                component.m_SubPrefab = this.m_LanePrefab;
                component.m_RandomSeed = random.NextInt();
                component.m_Flags |= CreationFlags.SubElevation;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
                int num = math.all(int2_2 == 0) ? 1 : 0;
                bool flag4 = math.all(new int2(int2_2.x, int2_2.y + 1) == int2_1);
                bool flag5 = (int2_1.x - int2_2.x & 1) == 1 || int2_2.x == 0;
                // ISSUE: reference to a compiler-generated method
                float cutPosition5 = this.GetCutPosition(netGeometryData, length1, (float) int2_2.x / (float) int2_1.x);
                ControlPoint controlPoint6;
                if (num != 0)
                {
                  controlPoint6 = controlPoint1;
                }
                else
                {
                  controlPoint6 = new ControlPoint();
                  controlPoint6.m_Rotation = controlPoint1.m_Rotation;
                  controlPoint6.m_Position = MathUtils.Position(line5, cutPosition5);
                  controlPoint6.m_Elevation = MathUtils.Position(line7, cutPosition5);
                }
                ControlPoint controlPoint7;
                if (flag4)
                {
                  controlPoint7 = controlPoint3;
                }
                else
                {
                  controlPoint7 = new ControlPoint();
                  controlPoint7.m_Rotation = controlPoint1.m_Rotation;
                  controlPoint7.m_Position = MathUtils.Position(line6, cutPosition5);
                  controlPoint7.m_Elevation = MathUtils.Position(line8, cutPosition5);
                }
                NetCourse netCourse = new NetCourse()
                {
                  m_Curve = NetUtils.StraightCurve(controlPoint6.m_Position, controlPoint7.m_Position)
                };
                // ISSUE: reference to a compiler-generated method
                netCourse.m_StartPosition = this.GetCoursePos(netCourse.m_Curve, controlPoint6, 0.0f);
                // ISSUE: reference to a compiler-generated method
                netCourse.m_EndPosition = this.GetCoursePos(netCourse.m_Curve, controlPoint7, 1f);
                if (!ownerDefinition2)
                {
                  netCourse.m_StartPosition.m_ParentMesh = -1;
                  netCourse.m_EndPosition.m_ParentMesh = -1;
                }
                netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsGrid;
                netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsGrid;
                if (int2_2.x != int2_1.x)
                {
                  netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsParallel;
                  netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsParallel;
                }
                if (num == 0)
                  netCourse.m_StartPosition.m_Flags |= CoursePosFlags.FreeHeight;
                if (!flag4)
                  netCourse.m_EndPosition.m_Flags |= CoursePosFlags.FreeHeight;
                if (int2_2.x == 0 || int2_2.x == int2_1.x)
                {
                  if (int2_2.y == 0)
                    netCourse.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
                  if (int2_2.y + 1 == int2_1.y)
                    netCourse.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
                  netCourse.m_StartPosition.m_Flags |= flag1 ? CoursePosFlags.IsLeft : CoursePosFlags.IsRight;
                  netCourse.m_EndPosition.m_Flags |= flag1 ? CoursePosFlags.IsLeft : CoursePosFlags.IsRight;
                }
                if (flag5)
                {
                  netCourse.m_Curve = MathUtils.Invert(netCourse.m_Curve);
                  CommonUtils.Swap<Entity>(ref netCourse.m_StartPosition.m_Entity, ref netCourse.m_EndPosition.m_Entity);
                  CommonUtils.Swap<float>(ref netCourse.m_StartPosition.m_SplitPosition, ref netCourse.m_EndPosition.m_SplitPosition);
                  CommonUtils.Swap<float3>(ref netCourse.m_StartPosition.m_Position, ref netCourse.m_EndPosition.m_Position);
                  CommonUtils.Swap<quaternion>(ref netCourse.m_StartPosition.m_Rotation, ref netCourse.m_EndPosition.m_Rotation);
                  CommonUtils.Swap<float2>(ref netCourse.m_StartPosition.m_Elevation, ref netCourse.m_EndPosition.m_Elevation);
                  CommonUtils.Swap<CoursePosFlags>(ref netCourse.m_StartPosition.m_Flags, ref netCourse.m_EndPosition.m_Flags);
                  CommonUtils.Swap<int>(ref netCourse.m_StartPosition.m_ParentMesh, ref netCourse.m_EndPosition.m_ParentMesh);
                  quaternion a = quaternion.RotateY(3.14159274f);
                  netCourse.m_StartPosition.m_Rotation = math.mul(a, netCourse.m_StartPosition.m_Rotation);
                  netCourse.m_EndPosition.m_Rotation = math.mul(a, netCourse.m_EndPosition.m_Rotation);
                }
                netCourse.m_Length = MathUtils.Length(netCourse.m_Curve);
                netCourse.m_FixedIndex = -1;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<NetCourse>(entity, netCourse);
                if (ownerDefinition1.m_Prefab != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition1);
                  LocalCurveCache localCurveCache;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  if (this.m_EditorMode && this.GetLocalCurve(netCourse, ownerDefinition1, out localCurveCache))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
                  }
                }
              }
            }
          }
        }
      }

      private void CreateComplexCurve(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions)
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint1 = this.m_ControlPoints[0];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 3];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint4 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint1);
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint2);
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint3);
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[this.m_NetPrefab];
          if ((double) netGeometryData.m_MaxSlopeSteepness == 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint2);
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint3);
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint4);
          }
        }
        else
          netGeometryData.m_DefaultWidth = 0.02f;
        float t1;
        float num1 = MathUtils.Distance(new Line2.Segment(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz), controlPoint3.m_Position.xz, out t1);
        float t2;
        float num2 = MathUtils.Distance(new Line2.Segment(controlPoint3.m_Position.xz, controlPoint2.m_Position.xz), controlPoint1.m_Position.xz, out t2);
        if ((double) num1 <= (double) netGeometryData.m_DefaultWidth * 0.75 && (double) num1 <= (double) num2)
        {
          t1 *= (float) (0.5 + (double) num1 / (double) netGeometryData.m_DefaultWidth * 0.66666668653488159);
          controlPoint2.m_Position = math.lerp(controlPoint1.m_Position, controlPoint2.m_Position, t1);
        }
        else if ((double) num2 <= (double) netGeometryData.m_DefaultWidth * 0.75)
        {
          float s = t2 * (float) (0.5 + (double) num2 / (double) netGeometryData.m_DefaultWidth * 0.66666668653488159);
          controlPoint2.m_Position = math.lerp(controlPoint3.m_Position, controlPoint2.m_Position, s);
        }
        float2 x1 = controlPoint2.m_Position.xz - controlPoint1.m_Position.xz;
        float2 x2 = controlPoint3.m_Position.xz - controlPoint4.m_Position.xz;
        float2 float2_1 = x1;
        float2 y = x2;
        if (!MathUtils.TryNormalize(ref float2_1))
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateSimpleCurve(ref ownerDefinitions, 2);
        }
        else if (!MathUtils.TryNormalize(ref y))
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateSimpleCurve(ref ownerDefinitions, 1);
        }
        else
        {
          float2 float2_2 = math.lerp(controlPoint2.m_Position.xz, controlPoint3.m_Position.xz, 0.5f);
          float num3 = MathUtils.Distance(new Line2.Segment(controlPoint2.m_Position.xz, controlPoint3.m_Position.xz), controlPoint4.m_Position.xz, out t1);
          if ((double) num3 <= (double) netGeometryData.m_DefaultWidth * 0.75)
          {
            float s = t1 * (float) (0.5 + (double) num3 / (double) netGeometryData.m_DefaultWidth * 0.66666668653488159);
            controlPoint3.m_Position = math.lerp(controlPoint2.m_Position, controlPoint3.m_Position, s);
            x2 = controlPoint3.m_Position.xz - controlPoint4.m_Position.xz;
            y = x2;
            if (!MathUtils.TryNormalize(ref y))
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateSimpleCurve(ref ownerDefinitions, 1);
              return;
            }
          }
          Bezier4x3 curve = new Bezier4x3(controlPoint1.m_Position, controlPoint2.m_Position, controlPoint3.m_Position, controlPoint4.m_Position);
          float2 xz = MathUtils.Position(curve, 0.5f).xz;
          float2 x3 = float2_2 - xz;
          float x4 = math.dot(float2_1, y);
          float2 float2_3;
          if ((double) math.abs(x4) < 0.99900001287460327)
          {
            float2 float2_4 = y.yx * float2_1;
            float2 float2_5 = x3.yx * float2_1;
            float2 float2_6 = y.yx * x3;
            float num4 = (float) (((double) float2_4.x - (double) float2_4.y) * 0.375);
            float2_3 = new float2(float2_6.x - float2_6.y, float2_5.x - float2_5.y) / num4 * math.abs(x4);
          }
          else
          {
            float2 x5 = new float2(math.length(x1), math.length(x2));
            float2_3 = (double) x4 <= 0.0 ? x5 / 3f : new float2(math.dot(x3, float2_1), math.dot(x3, y)) * (x5 / (math.csum(x5) * 0.375f));
          }
          curve.b.xz += float2_1 * float2_3.x;
          curve.c.xz += y * float2_3.y;
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
          CreationDefinition creationDefinition = new CreationDefinition();
          // ISSUE: reference to a compiler-generated field
          creationDefinition.m_Prefab = this.m_NetPrefab;
          // ISSUE: reference to a compiler-generated field
          creationDefinition.m_SubPrefab = this.m_LanePrefab;
          creationDefinition.m_RandomSeed = random.NextInt();
          creationDefinition.m_Flags |= CreationFlags.SubElevation;
          NetCourse course = new NetCourse();
          course.m_Curve = curve;
          // ISSUE: reference to a compiler-generated method
          this.LinearizeElevation(ref course.m_Curve);
          // ISSUE: reference to a compiler-generated method
          course.m_StartPosition = this.GetCoursePos(course.m_Curve, controlPoint1, 0.0f);
          // ISSUE: reference to a compiler-generated method
          course.m_EndPosition = this.GetCoursePos(course.m_Curve, controlPoint4, 1f);
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
          if (course.m_StartPosition.m_Position.Equals(course.m_EndPosition.m_Position) && course.m_StartPosition.m_Entity.Equals(course.m_EndPosition.m_Entity))
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
            course.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
          }
          // ISSUE: reference to a compiler-generated field
          bool2 x6 = this.m_ParallelCount > 0;
          if (!x6.x)
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
            course.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
          }
          if (!x6.y)
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
            course.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
          }
          course.m_Length = MathUtils.Length(course.m_Curve);
          course.m_FixedIndex = -1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceableData.HasComponent(this.m_NetPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PlaceableNetData placeableNetData = this.m_PlaceableData[this.m_NetPrefab];
            // ISSUE: reference to a compiler-generated method
            if ((double) this.CalculatedInverseWeight(course, placeableNetData.m_PlacementFlags) < 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.InvertCourse(ref course);
            }
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, creationDefinition);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          OwnerDefinition ownerDefinition;
          // ISSUE: reference to a compiler-generated method
          if (this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, course.m_StartPosition, course.m_EndPosition, out ownerDefinition))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
            LocalCurveCache localCurveCache;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EditorMode && this.GetLocalCurve(course, ownerDefinition, out localCurveCache))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
            }
          }
          else
          {
            course.m_StartPosition.m_ParentMesh = -1;
            course.m_EndPosition.m_ParentMesh = -1;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity, course);
          if (!math.any(x6))
            return;
          NativeParallelHashMap<float4, float3> nodeMap = new NativeParallelHashMap<float4, float3>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourses(creationDefinition, ownerDefinition, course, nodeMap);
          nodeMap.Dispose();
        }
      }

      private void CreateContinuousCurve(
        ref NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions)
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint1 = this.m_ControlPoints[0];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint1);
        // ISSUE: reference to a compiler-generated method
        this.FixElevation(ref controlPoint2);
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(this.m_NetPrefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_NetGeometryData[this.m_NetPrefab];
          flag = (netGeometryData.m_Flags & Game.Net.GeometryFlags.NoCurveSplit) != 0;
          if ((double) netGeometryData.m_MaxSlopeSteepness == 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint2);
            // ISSUE: reference to a compiler-generated method
            this.SetHeight(controlPoint1, ref controlPoint3);
          }
        }
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(0);
        float3 startTangent = new float3(controlPoint2.m_Direction.x, 0.0f, controlPoint2.m_Direction.y);
        float3 endTangent = new float3(controlPoint3.m_Direction.x, 0.0f, controlPoint3.m_Direction.y);
        float x1 = math.dot(math.normalizesafe(controlPoint3.m_Position.xz - controlPoint1.m_Position.xz), controlPoint2.m_Direction);
        if ((double) math.abs(x1) < 0.0099999997764825821 && !flag)
        {
          int2 int2 = random.NextInt2();
          CreationDefinition creationDefinition = new CreationDefinition();
          // ISSUE: reference to a compiler-generated field
          creationDefinition.m_Prefab = this.m_NetPrefab;
          // ISSUE: reference to a compiler-generated field
          creationDefinition.m_SubPrefab = this.m_LanePrefab;
          creationDefinition.m_Flags |= CreationFlags.SubElevation;
          NetCourse course1 = new NetCourse();
          NetCourse course2 = new NetCourse();
          float num = math.distance(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz);
          controlPoint2.m_Direction = MathUtils.Right(controlPoint2.m_Direction);
          if ((double) math.dot(controlPoint3.m_Position.xz - controlPoint1.m_Position.xz, controlPoint2.m_Direction) < 0.0)
            controlPoint2.m_Direction = -controlPoint2.m_Direction;
          float3 float3 = new float3(controlPoint2.m_Direction.x, 0.0f, controlPoint2.m_Direction.y);
          controlPoint2.m_OriginalEntity = Entity.Null;
          controlPoint2.m_Position += float3 * num;
          course1.m_Curve = NetUtils.FitCurve(controlPoint1.m_Position, startTangent, float3, controlPoint2.m_Position);
          course2.m_Curve = NetUtils.FitCurve(controlPoint2.m_Position, float3, endTangent, controlPoint3.m_Position);
          Bezier4x3 output1;
          Bezier4x3 output2;
          MathUtils.Divide(NetUtils.FitCurve(controlPoint1.m_Position, startTangent, endTangent, controlPoint3.m_Position), out output1, out output2, 0.5f);
          float t = math.abs(x1) * 100f;
          course1.m_Curve = MathUtils.Lerp(course1.m_Curve, output1, t);
          course2.m_Curve = MathUtils.Lerp(course2.m_Curve, output2, t);
          // ISSUE: reference to a compiler-generated method
          this.LinearizeElevation(ref course1.m_Curve, ref course2.m_Curve);
          controlPoint2.m_Position = course1.m_Curve.d;
          controlPoint2.m_Elevation = math.lerp(controlPoint1.m_Elevation, controlPoint3.m_Elevation, 0.5f);
          // ISSUE: reference to a compiler-generated method
          course1.m_StartPosition = this.GetCoursePos(course1.m_Curve, controlPoint1, 0.0f);
          // ISSUE: reference to a compiler-generated method
          course1.m_EndPosition = this.GetCoursePos(course1.m_Curve, controlPoint2, 1f);
          // ISSUE: reference to a compiler-generated method
          course2.m_StartPosition = this.GetCoursePos(course2.m_Curve, controlPoint2, 0.0f);
          // ISSUE: reference to a compiler-generated method
          course2.m_EndPosition = this.GetCoursePos(course2.m_Curve, controlPoint3, 1f);
          course1.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
          course1.m_EndPosition.m_Flags |= CoursePosFlags.FreeHeight;
          course2.m_StartPosition.m_Flags |= CoursePosFlags.FreeHeight;
          course2.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
          // ISSUE: reference to a compiler-generated field
          bool2 x2 = this.m_ParallelCount > 0;
          if (!x2.x)
          {
            course1.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
            course1.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
            course2.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
            course2.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
          }
          if (!x2.y)
          {
            course1.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
            course1.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
            course2.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
            course2.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
          }
          course1.m_Length = MathUtils.Length(course1.m_Curve);
          course2.m_Length = MathUtils.Length(course2.m_Curve);
          course1.m_FixedIndex = -1;
          course2.m_FixedIndex = -1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceableData.HasComponent(this.m_NetPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PlaceableNetData placeableNetData = this.m_PlaceableData[this.m_NetPrefab];
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if ((double) this.CalculatedInverseWeight(course1, placeableNetData.m_PlacementFlags) + (double) this.CalculatedInverseWeight(course2, placeableNetData.m_PlacementFlags) < 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.InvertCourse(ref course1);
              // ISSUE: reference to a compiler-generated method
              this.InvertCourse(ref course2);
            }
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity1 = this.m_CommandBuffer.CreateEntity();
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity();
          creationDefinition.m_RandomSeed = int2.x;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity1, creationDefinition);
          creationDefinition.m_RandomSeed = int2.y;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity2, creationDefinition);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity2, new Updated());
          OwnerDefinition ownerDefinition;
          // ISSUE: reference to a compiler-generated method
          if (this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, course1.m_StartPosition, course2.m_EndPosition, out ownerDefinition))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, ownerDefinition);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity2, ownerDefinition);
            LocalCurveCache localCurveCache1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EditorMode && this.GetLocalCurve(course1, ownerDefinition, out localCurveCache1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity1, localCurveCache1);
            }
            LocalCurveCache localCurveCache2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EditorMode && this.GetLocalCurve(course2, ownerDefinition, out localCurveCache2))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity2, localCurveCache2);
            }
          }
          else
          {
            course1.m_StartPosition.m_ParentMesh = -1;
            course1.m_EndPosition.m_ParentMesh = -1;
            course2.m_StartPosition.m_ParentMesh = -1;
            course2.m_EndPosition.m_ParentMesh = -1;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity1, course1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity2, course2);
          if (!math.any(x2))
            return;
          NativeParallelHashMap<float4, float3> nodeMap = new NativeParallelHashMap<float4, float3>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourses(creationDefinition, ownerDefinition, course1, nodeMap);
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourses(creationDefinition, ownerDefinition, course2, nodeMap);
          nodeMap.Dispose();
        }
        else
        {
          CreationDefinition creationDefinition = new CreationDefinition();
          // ISSUE: reference to a compiler-generated field
          creationDefinition.m_Prefab = this.m_NetPrefab;
          // ISSUE: reference to a compiler-generated field
          creationDefinition.m_SubPrefab = this.m_LanePrefab;
          creationDefinition.m_RandomSeed = random.NextInt();
          creationDefinition.m_Flags |= CreationFlags.SubElevation;
          NetCourse course = new NetCourse();
          if ((double) x1 < 0.0)
          {
            float3 endPos = controlPoint3.m_Position + endTangent * x1;
            course.m_Curve = NetUtils.FitCurve(controlPoint1.m_Position, startTangent, endTangent, endPos);
            course.m_Curve.d = controlPoint3.m_Position;
          }
          else
            course.m_Curve = NetUtils.FitCurve(controlPoint1.m_Position, startTangent, endTangent, controlPoint3.m_Position);
          // ISSUE: reference to a compiler-generated method
          this.LinearizeElevation(ref course.m_Curve);
          // ISSUE: reference to a compiler-generated method
          course.m_StartPosition = this.GetCoursePos(course.m_Curve, controlPoint1, 0.0f);
          // ISSUE: reference to a compiler-generated method
          course.m_EndPosition = this.GetCoursePos(course.m_Curve, controlPoint3, 1f);
          course.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
          course.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
          if (course.m_StartPosition.m_Position.Equals(course.m_EndPosition.m_Position) && course.m_StartPosition.m_Entity.Equals(course.m_EndPosition.m_Entity))
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
            course.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
          }
          // ISSUE: reference to a compiler-generated field
          bool2 x3 = this.m_ParallelCount > 0;
          if (!x3.x)
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.IsLeft;
            course.m_EndPosition.m_Flags |= CoursePosFlags.IsLeft;
          }
          if (!x3.y)
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.IsRight;
            course.m_EndPosition.m_Flags |= CoursePosFlags.IsRight;
          }
          course.m_Length = MathUtils.Length(course.m_Curve);
          course.m_FixedIndex = -1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceableData.HasComponent(this.m_NetPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PlaceableNetData placeableNetData = this.m_PlaceableData[this.m_NetPrefab];
            // ISSUE: reference to a compiler-generated method
            if ((double) this.CalculatedInverseWeight(course, placeableNetData.m_PlacementFlags) < 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.InvertCourse(ref course);
            }
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity();
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, creationDefinition);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
          OwnerDefinition ownerDefinition;
          // ISSUE: reference to a compiler-generated method
          if (this.GetOwnerDefinition(ref ownerDefinitions, Entity.Null, true, course.m_StartPosition, course.m_EndPosition, out ownerDefinition))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity, ownerDefinition);
            LocalCurveCache localCurveCache;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_EditorMode && this.GetLocalCurve(course, ownerDefinition, out localCurveCache))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalCurveCache>(entity, localCurveCache);
            }
          }
          else
          {
            course.m_StartPosition.m_ParentMesh = -1;
            course.m_EndPosition.m_ParentMesh = -1;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity, course);
          if (!math.any(x3))
            return;
          NativeParallelHashMap<float4, float3> nodeMap = new NativeParallelHashMap<float4, float3>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          this.CreateParallelCourses(creationDefinition, ownerDefinition, course, nodeMap);
          nodeMap.Dispose();
        }
      }

      private void LinearizeElevation(ref Bezier4x3 curve)
      {
        float2 float2 = math.lerp((float2) curve.a.y, (float2) curve.d.y, new float2(0.333333343f, 0.6666667f));
        curve.b.y = float2.x;
        curve.c.y = float2.y;
      }

      private void LinearizeElevation(ref Bezier4x3 curve1, ref Bezier4x3 curve2)
      {
        curve1.d.y = curve2.a.y = math.lerp(curve1.a.y, curve2.d.y, 0.5f);
        // ISSUE: reference to a compiler-generated method
        this.LinearizeElevation(ref curve1);
        // ISSUE: reference to a compiler-generated method
        this.LinearizeElevation(ref curve2);
      }

      private CoursePos GetCoursePos(Bezier4x3 curve, ControlPoint controlPoint, float courseDelta)
      {
        CoursePos coursePos = new CoursePos();
        if (controlPoint.m_OriginalEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(controlPoint.m_OriginalEntity))
          {
            if ((double) controlPoint.m_CurvePosition <= 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              coursePos.m_Entity = this.m_EdgeData[controlPoint.m_OriginalEntity].m_Start;
              coursePos.m_SplitPosition = 0.0f;
            }
            else if ((double) controlPoint.m_CurvePosition >= 1.0)
            {
              // ISSUE: reference to a compiler-generated field
              coursePos.m_Entity = this.m_EdgeData[controlPoint.m_OriginalEntity].m_End;
              coursePos.m_SplitPosition = 1f;
            }
            else
            {
              coursePos.m_Entity = controlPoint.m_OriginalEntity;
              coursePos.m_SplitPosition = controlPoint.m_CurvePosition;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.HasComponent(controlPoint.m_OriginalEntity))
            {
              coursePos.m_Entity = controlPoint.m_OriginalEntity;
              coursePos.m_SplitPosition = controlPoint.m_CurvePosition;
            }
          }
        }
        coursePos.m_Position = controlPoint.m_Position;
        coursePos.m_Elevation = (float2) controlPoint.m_Elevation;
        coursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(curve, courseDelta));
        coursePos.m_CourseDelta = courseDelta;
        coursePos.m_ParentMesh = controlPoint.m_ElementIndex.x;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (Entity entity = controlPoint.m_OriginalEntity; this.m_OwnerData.HasComponent(entity) && !this.m_BuildingData.HasComponent(entity) && !this.m_ExtensionData.HasComponent(entity); entity = this.m_OwnerData[entity].m_Owner)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalTransformCacheData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            coursePos.m_ParentMesh = this.m_LocalTransformCacheData[entity].m_ParentMesh;
          }
          else
          {
            Game.Net.Edge componentData1;
            LocalTransformCache componentData2;
            LocalTransformCache componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.TryGetComponent(entity, out componentData1) && this.m_LocalTransformCacheData.TryGetComponent(componentData1.m_Start, out componentData2) && this.m_LocalTransformCacheData.TryGetComponent(componentData1.m_End, out componentData3))
              coursePos.m_ParentMesh = math.select(componentData2.m_ParentMesh, -1, componentData2.m_ParentMesh != componentData3.m_ParentMesh);
          }
        }
        return coursePos;
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> __Game_Tools_NetCourse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadComposition> __Game_Prefabs_RoadComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AssetStampData> __Game_Prefabs_AssetStampData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubReplacement> __Game_Net_SubReplacement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionArea> __Game_Prefabs_NetCompositionArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Extension> __Game_Buildings_Extension_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubAreaNode> __Game_Prefabs_SubAreaNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_NetCourse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCourse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadComposition_RO_ComponentLookup = state.GetComponentLookup<RoadComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AssetStampData_RO_ComponentLookup = state.GetComponentLookup<AssetStampData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RO_BufferLookup = state.GetBufferLookup<SubReplacement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionArea_RO_BufferLookup = state.GetBufferLookup<NetCompositionArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RO_ComponentLookup = state.GetComponentLookup<Extension>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferLookup = state.GetBufferLookup<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RO_BufferLookup = state.GetBufferLookup<SubAreaNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
      }
    }
  }
}
