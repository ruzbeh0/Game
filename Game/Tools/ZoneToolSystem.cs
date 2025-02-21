// Decompiled with JetBrains decompiler
// Type: Game.Tools.ZoneToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Audio;
using Game.Common;
using Game.Input;
using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
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
  public class ZoneToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Zone Tool";
    private ZonePrefab m_Prefab;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private AudioManager m_AudioManager;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_DefinitionGroup;
    private EntityQuery m_TempBlockQuery;
    private EntityQuery m_SoundQuery;
    private IProxyAction m_ApplyZone;
    private IProxyAction m_RemoveZone;
    private IProxyAction m_DiscardZoning;
    private IProxyAction m_DiscardDezoning;
    private IProxyAction m_DefaultDiscardApply;
    private IProxyAction m_DefaultDiscardRemove;
    private bool m_ApplyBlocked;
    private ControlPoint m_RaycastPoint;
    private ControlPoint m_StartPoint;
    private NativeValue<ControlPoint> m_SnapPoint;
    private ZoneToolSystem.State m_State;
    private ZoneToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Zone Tool";

    public override int uiModeIndex => (int) this.mode;

    public override void GetUIModes(List<ToolMode> modes)
    {
      List<ToolMode> toolModeList1 = modes;
      // ISSUE: variable of a compiler-generated type
      ZoneToolSystem.Mode mode1 = ZoneToolSystem.Mode.FloodFill;
      ToolMode toolMode1 = new ToolMode(mode1.ToString(), 0);
      toolModeList1.Add(toolMode1);
      List<ToolMode> toolModeList2 = modes;
      // ISSUE: variable of a compiler-generated type
      ZoneToolSystem.Mode mode2 = ZoneToolSystem.Mode.Marquee;
      ToolMode toolMode2 = new ToolMode(mode2.ToString(), 1);
      toolModeList2.Add(toolMode2);
      List<ToolMode> toolModeList3 = modes;
      // ISSUE: variable of a compiler-generated type
      ZoneToolSystem.Mode mode3 = ZoneToolSystem.Mode.Paint;
      ToolMode toolMode3 = new ToolMode(mode3.ToString(), 2);
      toolModeList3.Add(toolMode3);
    }

    public ZoneToolSystem.Mode mode { get; set; }

    public ZonePrefab prefab
    {
      get => this.m_Prefab;
      set
      {
        if (!((Object) this.m_Prefab != (Object) value))
          return;
        this.m_ForceUpdate = true;
        this.m_Prefab = value;
      }
    }

    public bool overwrite { get; set; }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_ApplyZone;
        yield return this.m_RemoveZone;
        yield return this.m_DiscardZoning;
        yield return this.m_DiscardDezoning;
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
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionGroup = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_TempBlockQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Zones.Block>(), ComponentType.ReadWrite<Cell>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyZone = InputManager.instance.toolActionCollection.GetActionState("Apply Zone", nameof (ZoneToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_RemoveZone = InputManager.instance.toolActionCollection.GetActionState("Remove Zone", nameof (ZoneToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardZoning = InputManager.instance.toolActionCollection.GetActionState("Discard Zoning", nameof (ZoneToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardDezoning = InputManager.instance.toolActionCollection.GetActionState("Discard Dezoning", nameof (ZoneToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultDiscardApply = InputManager.instance.toolActionCollection.GetActionState("Discard Primary", nameof (ZoneToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultDiscardRemove = InputManager.instance.toolActionCollection.GetActionState("Discard Secondary", nameof (ZoneToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_SnapPoint = new NativeValue<ControlPoint>(Allocator.Persistent);
      this.overwrite = true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SnapPoint.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      this.requireZones = true;
      this.requireAreas = AreaTypeMask.Lots;
      // ISSUE: reference to a compiler-generated field
      this.m_RaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = ZoneToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyBlocked = false;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.State state = this.m_State;
        switch (state)
        {
          case ZoneToolSystem.State.Zoning:
            this.applyAction.enabled = this.actionsEnabled;
            this.secondaryApplyAction.enabled = false;
            this.cancelAction.enabled = this.actionsEnabled;
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_ApplyZone;
            this.secondaryApplyActionOverride = (IProxyAction) null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.cancelActionOverride = this.mode == ZoneToolSystem.Mode.Marquee ? this.m_DiscardZoning : this.m_DefaultDiscardApply;
            break;
          case ZoneToolSystem.State.Dezoning:
            this.applyAction.enabled = false;
            this.secondaryApplyAction.enabled = this.actionsEnabled;
            this.cancelAction.enabled = this.actionsEnabled;
            this.applyActionOverride = (IProxyAction) null;
            // ISSUE: reference to a compiler-generated field
            this.secondaryApplyActionOverride = this.m_RemoveZone;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.cancelActionOverride = this.mode == ZoneToolSystem.Mode.Marquee ? this.m_DiscardDezoning : this.m_DefaultDiscardRemove;
            break;
          default:
            this.applyAction.enabled = this.actionsEnabled;
            this.secondaryApplyAction.enabled = this.actionsEnabled;
            this.cancelAction.enabled = false;
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_ApplyZone;
            // ISSUE: reference to a compiler-generated field
            this.secondaryApplyActionOverride = this.m_RemoveZone;
            this.cancelActionOverride = (IProxyAction) null;
            break;
        }
      }
    }

    public override PrefabBase GetPrefab() => (PrefabBase) this.prefab;

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      if (!(prefab is ZonePrefab zonePrefab))
        return false;
      this.prefab = zonePrefab;
      return true;
    }

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
      if ((Object) this.prefab != (Object) null)
      {
        Snap onMask;
        Snap offMask;
        // ISSUE: reference to a compiler-generated method
        this.GetAvailableSnapMask(out onMask, out offMask);
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode = this.mode;
        switch (mode)
        {
          case ZoneToolSystem.Mode.FloodFill:
          case ZoneToolSystem.Mode.Paint:
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Zones;
            break;
          case ZoneToolSystem.Mode.Marquee:
            // ISSUE: reference to a compiler-generated method
            if ((ToolBaseSystem.GetActualSnap(this.selectedSnap, onMask, offMask) & Snap.ExistingGeometry) != Snap.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Zones;
              break;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain;
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.typeMask = TypeMask.None;
            break;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.None;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      // ISSUE: reference to a compiler-generated field
      if (this.m_FocusChanged)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != ZoneToolSystem.State.Default && (!this.applyAction.enabled || !this.cancelAction.enabled) && (!this.secondaryApplyAction.enabled || !this.cancelAction.enabled))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = ZoneToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated method
        return this.Clear(inputDeps);
      }
      if ((Object) this.prefab != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetAvailableSnapMask(out this.m_SnapOnMask, out this.m_SnapOffMask);
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ZoneToolSystem.State state = this.m_State;
          switch (state)
          {
            case ZoneToolSystem.State.Default:
              // ISSUE: reference to a compiler-generated field
              if (this.m_ApplyBlocked)
              {
                if (this.mode != ZoneToolSystem.Mode.Marquee || this.applyAction.WasReleasedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame())
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_ApplyBlocked = false;
                }
                // ISSUE: reference to a compiler-generated method
                return this.Update(inputDeps);
              }
              if (this.secondaryApplyAction.WasPressedThisFrame())
              {
                // ISSUE: reference to a compiler-generated method
                return this.Cancel(inputDeps, this.secondaryApplyAction.WasReleasedThisFrame());
              }
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              return this.applyAction.WasPressedThisFrame() ? this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame()) : this.Update(inputDeps);
            case ZoneToolSystem.State.Zoning:
              if (this.cancelAction.WasPressedThisFrame())
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ApplyBlocked = this.mode == ZoneToolSystem.Mode.Marquee;
                // ISSUE: reference to a compiler-generated method
                return this.Cancel(inputDeps);
              }
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              return this.applyAction.WasPressedThisFrame() || this.applyAction.WasReleasedThisFrame() ? this.Apply(inputDeps) : this.Update(inputDeps);
            case ZoneToolSystem.State.Dezoning:
              if (this.cancelAction.WasPressedThisFrame())
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ApplyBlocked = this.mode == ZoneToolSystem.Mode.Marquee;
                // ISSUE: reference to a compiler-generated method
                return this.Apply(inputDeps);
              }
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              return this.secondaryApplyAction.WasPressedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame() ? this.Cancel(inputDeps) : this.Update(inputDeps);
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != ZoneToolSystem.State.Default && (this.applyAction.WasReleasedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame()))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = ZoneToolSystem.State.Default;
      }
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    public override void GetAvailableSnapMask(out Snap onMask, out Snap offMask)
    {
      // ISSUE: variable of a compiler-generated type
      ZoneToolSystem.Mode mode = this.mode;
      switch (mode)
      {
        case ZoneToolSystem.Mode.FloodFill:
        case ZoneToolSystem.Mode.Paint:
          onMask = Snap.ExistingGeometry;
          offMask = Snap.None;
          break;
        case ZoneToolSystem.Mode.Marquee:
          onMask = Snap.ExistingGeometry | Snap.CellLength;
          offMask = Snap.ExistingGeometry | Snap.CellLength;
          break;
        default:
          // ISSUE: reference to a compiler-generated method
          base.GetAvailableSnapMask(out onMask, out offMask);
          break;
      }
      onMask |= Snap.ContourLines;
      offMask |= Snap.ContourLines;
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      return inputDeps;
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == ZoneToolSystem.State.Default)
      {
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode = this.mode;
        switch (mode)
        {
          case ZoneToolSystem.Mode.FloodFill:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningRemoveFillSound);
            this.applyMode = ApplyMode.Apply;
            break;
          case ZoneToolSystem.Mode.Marquee:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningMarqueeClearStartSound);
            this.applyMode = ApplyMode.Clear;
            break;
          case ZoneToolSystem.Mode.Paint:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningStartRemovePaintSound);
            this.applyMode = ApplyMode.Apply;
            break;
        }
        if (!singleFrameOnly)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = this.m_SnapPoint.value;
          // ISSUE: reference to a compiler-generated field
          this.m_State = ZoneToolSystem.State.Dezoning;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated method
        JobHandle inputDeps1 = this.SnapPoint(inputDeps);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return JobHandle.CombineDependencies(this.SetZoneType(inputDeps1), this.UpdateDefinitions(inputDeps1));
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == ZoneToolSystem.State.Dezoning)
      {
        this.applyMode = ApplyMode.Apply;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) math.distance(this.m_StartPoint.m_Position, this.m_RaycastPoint.m_Position) > 5.0 && this.mode == ZoneToolSystem.Mode.Marquee)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningMarqueeClearEndSound);
        }
        if (this.mode == ZoneToolSystem.Mode.Paint)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningEndRemovePaintSound);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = ZoneToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.SnapPoint(inputDeps);
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = ZoneToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetRaycastResult(out this.m_RaycastPoint);
      // ISSUE: reference to a compiler-generated method
      inputDeps = this.SnapPoint(inputDeps);
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == ZoneToolSystem.State.Default)
      {
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode = this.mode;
        switch (mode)
        {
          case ZoneToolSystem.Mode.FloodFill:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningFillSound);
            this.applyMode = ApplyMode.Apply;
            break;
          case ZoneToolSystem.Mode.Marquee:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningMarqueeStartSound);
            this.applyMode = ApplyMode.Clear;
            break;
          case ZoneToolSystem.Mode.Paint:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningStartPaintSound);
            this.applyMode = ApplyMode.Apply;
            break;
        }
        if (!singleFrameOnly)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = this.m_SnapPoint.value;
          // ISSUE: reference to a compiler-generated field
          this.m_State = ZoneToolSystem.State.Zoning;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.SnapPoint(inputDeps);
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == ZoneToolSystem.State.Zoning)
      {
        this.applyMode = ApplyMode.Apply;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) math.distance(this.m_StartPoint.m_Position, this.m_RaycastPoint.m_Position) > 5.0 && this.mode == ZoneToolSystem.Mode.Marquee)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningMarqueeEndSound);
        }
        if (this.mode == ZoneToolSystem.Mode.Paint)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_ZoningEndPaintSound);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = ZoneToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.SnapPoint(inputDeps);
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = ZoneToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetRaycastResult(out this.m_RaycastPoint);
      // ISSUE: reference to a compiler-generated method
      inputDeps = this.SnapPoint(inputDeps);
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      ControlPoint controlPoint;
      bool forceUpdate;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint, out forceUpdate))
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint other = this.m_SnapPoint.value;
        // ISSUE: reference to a compiler-generated field
        if (this.m_RaycastPoint.Equals(controlPoint) && !forceUpdate)
        {
          // ISSUE: variable of a compiler-generated type
          ZoneToolSystem.Mode mode = this.mode;
          switch (mode)
          {
            case ZoneToolSystem.Mode.FloodFill:
            case ZoneToolSystem.Mode.Paint:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_State == ZoneToolSystem.State.Default || this.m_StartPoint.Equals(other))
              {
                this.applyMode = ApplyMode.None;
                return inputDeps;
              }
              break;
            case ZoneToolSystem.Mode.Marquee:
              this.applyMode = ApplyMode.None;
              return inputDeps;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_RaycastPoint = controlPoint;
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.SnapPoint(inputDeps);
          JobHandle.ScheduleBatchedJobs();
          inputDeps.Complete();
        }
        // ISSUE: reference to a compiler-generated field
        if (other.Equals(this.m_SnapPoint.value) && !forceUpdate)
        {
          // ISSUE: variable of a compiler-generated type
          ZoneToolSystem.Mode mode = this.mode;
          switch (mode)
          {
            case ZoneToolSystem.Mode.FloodFill:
            case ZoneToolSystem.Mode.Paint:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_State == ZoneToolSystem.State.Default || this.m_StartPoint.Equals(other))
              {
                this.applyMode = ApplyMode.None;
                return inputDeps;
              }
              break;
            case ZoneToolSystem.Mode.Marquee:
              this.applyMode = ApplyMode.None;
              return inputDeps;
          }
        }
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode1 = this.mode;
        switch (mode1)
        {
          case ZoneToolSystem.Mode.FloodFill:
          case ZoneToolSystem.Mode.Paint:
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != ZoneToolSystem.State.Default)
            {
              this.applyMode = ApplyMode.Apply;
              // ISSUE: reference to a compiler-generated field
              this.m_StartPoint = other;
            }
            else
              this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated method
            return this.UpdateDefinitions(inputDeps);
          case ZoneToolSystem.Mode.Marquee:
            this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated method
            return this.UpdateDefinitions(inputDeps);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_RaycastPoint.Equals(new ControlPoint()))
        {
          this.applyMode = forceUpdate ? ApplyMode.Clear : ApplyMode.None;
          return inputDeps;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_RaycastPoint = new ControlPoint();
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode = this.mode;
        switch (mode)
        {
          case ZoneToolSystem.Mode.FloodFill:
          case ZoneToolSystem.Mode.Paint:
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != ZoneToolSystem.State.Default)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StartPoint = this.m_SnapPoint.value;
              this.applyMode = ApplyMode.Apply;
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.SnapPoint(inputDeps);
              // ISSUE: reference to a compiler-generated method
              return this.UpdateDefinitions(inputDeps);
            }
            break;
        }
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated method
      inputDeps = this.SnapPoint(inputDeps);
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private JobHandle SetZoneType(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_TempBlockQuery.IsEmptyIgnoreFilter)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      return new ZoneToolSystem.SetZoneTypeJob()
      {
        m_Type = new ZoneType(),
        m_BlockType = this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle,
        m_CellType = this.__TypeHandle.__Game_Zones_Cell_RW_BufferTypeHandle
      }.ScheduleParallel<ZoneToolSystem.SetZoneTypeJob>(this.m_TempBlockQuery, inputDeps);
    }

    private JobHandle SnapPoint(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_RaycastPoint.Equals(new ControlPoint()))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SnapPoint.value = new ControlPoint();
        return inputDeps;
      }
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_TempBlockQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      UnityEngine.Transform transform = Camera.main.transform;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps1 = new ZoneToolSystem.SnapJob()
      {
        m_Snap = this.GetActualSnap(),
        m_Mode = this.mode,
        m_State = this.m_State,
        m_CameraRight = ((float3) transform.right),
        m_StartPoint = this.m_StartPoint,
        m_RaycastPoint = this.m_RaycastPoint,
        m_TempChunks = archetypeChunkListAsync,
        m_BlockType = this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle,
        m_CellType = this.__TypeHandle.__Game_Zones_Cell_RO_BufferTypeHandle,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_SnapPoint = this.m_SnapPoint
      }.Schedule<ZoneToolSystem.SnapJob>(JobHandle.CombineDependencies(inputDeps, outJobHandle));
      archetypeChunkListAsync.Dispose(inputDeps1);
      return inputDeps1;
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionGroup, this.m_ToolOutputBarrier, inputDeps);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RaycastPoint.Equals(new ControlPoint()))
      {
        UnityEngine.Transform transform = Camera.main.transform;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new ZoneToolSystem.CreateDefinitionsJob()
        {
          m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab),
          m_Mode = this.mode,
          m_State = this.m_State,
          m_CameraRight = ((float3) transform.right),
          m_Overwrite = this.overwrite,
          m_StartPoint = this.m_StartPoint,
          m_SnapPoint = this.m_SnapPoint,
          m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
          m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        }.Schedule<ZoneToolSystem.CreateDefinitionsJob>(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
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
    public ZoneToolSystem()
    {
    }

    public enum Mode
    {
      FloodFill,
      Marquee,
      Paint,
    }

    private enum State
    {
      Default,
      Zoning,
      Dezoning,
    }

    [BurstCompile]
    private struct SetZoneTypeJob : IJobChunk
    {
      [ReadOnly]
      public ZoneType m_Type;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> m_BlockType;
      public BufferTypeHandle<Cell> m_CellType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Zones.Block> nativeArray = chunk.GetNativeArray<Game.Zones.Block>(ref this.m_BlockType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Cell> bufferAccessor = chunk.GetBufferAccessor<Cell>(ref this.m_CellType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Game.Zones.Block block = nativeArray[index1];
          DynamicBuffer<Cell> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < block.m_Size.y; ++index2)
          {
            for (int index3 = 0; index3 < block.m_Size.x; ++index3)
            {
              int index4 = index2 * block.m_Size.x + index3;
              // ISSUE: reference to a compiler-generated field
              Cell cell = dynamicBuffer[index4] with
              {
                m_Zone = this.m_Type
              };
              dynamicBuffer[index4] = cell;
            }
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
    private struct SnapJob : IJob
    {
      [ReadOnly]
      public Snap m_Snap;
      [ReadOnly]
      public ZoneToolSystem.Mode m_Mode;
      [ReadOnly]
      public ZoneToolSystem.State m_State;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public ControlPoint m_StartPoint;
      [ReadOnly]
      public ControlPoint m_RaycastPoint;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_TempChunks;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> m_BlockType;
      [ReadOnly]
      public BufferTypeHandle<Cell> m_CellType;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_BlockData;
      public NativeValue<ControlPoint> m_SnapPoint;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode = this.m_Mode;
        switch (mode)
        {
          case ZoneToolSystem.Mode.FloodFill:
          case ZoneToolSystem.Mode.Paint:
            // ISSUE: reference to a compiler-generated method
            this.CheckSelectedCell();
            break;
          case ZoneToolSystem.Mode.Marquee:
            // ISSUE: reference to a compiler-generated method
            this.CheckMarqueeCell();
            break;
        }
      }

      private void CheckSelectedCell()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_TempChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk tempChunk = this.m_TempChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Zones.Block> nativeArray = tempChunk.GetNativeArray<Game.Zones.Block>(ref this.m_BlockType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Cell> bufferAccessor = tempChunk.GetBufferAccessor<Cell>(ref this.m_CellType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Game.Zones.Block block = nativeArray[index2];
            DynamicBuffer<Cell> dynamicBuffer = bufferAccessor[index2];
            // ISSUE: reference to a compiler-generated field
            int2 cellIndex = ZoneUtils.GetCellIndex(block, this.m_RaycastPoint.m_HitPosition.xz);
            if (math.all(cellIndex >= 0 & cellIndex < block.m_Size) && (dynamicBuffer[cellIndex.y * block.m_Size.x + cellIndex.x].m_State & CellFlags.Selected) != CellFlags.None)
              return;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SnapPoint.value = this.m_RaycastPoint;
      }

      private void CheckMarqueeCell()
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint raycastPoint = this.m_RaycastPoint;
        // ISSUE: reference to a compiler-generated field
        if ((this.m_Snap & Snap.ExistingGeometry) == Snap.None)
          raycastPoint.m_OriginalEntity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BlockData.HasComponent(raycastPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[raycastPoint.m_OriginalEntity];
          raycastPoint.m_Position = ZoneUtils.GetCellPosition(block, raycastPoint.m_ElementIndex);
          raycastPoint.m_HitPosition = raycastPoint.m_Position;
          // ISSUE: reference to a compiler-generated field
          this.m_SnapPoint.value = raycastPoint;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_Snap & Snap.CellLength) != Snap.None && this.m_State != ZoneToolSystem.State.Default)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 float2 = !this.m_BlockData.HasComponent(this.m_StartPoint.m_OriginalEntity) ? math.normalizesafe(this.m_CameraRight.xz) : this.m_BlockData[this.m_StartPoint.m_OriginalEntity].m_Direction;
            float2 y = MathUtils.Right(float2);
            // ISSUE: reference to a compiler-generated field
            float2 xz = this.m_StartPoint.m_HitPosition.xz;
            float2 x = raycastPoint.m_HitPosition.xz - xz;
            float num1 = MathUtils.Snap(math.dot(x, float2), 8f);
            float num2 = MathUtils.Snap(math.dot(x, y), 8f);
            // ISSUE: reference to a compiler-generated field
            raycastPoint.m_HitPosition.y = this.m_StartPoint.m_HitPosition.y;
            raycastPoint.m_HitPosition.xz = xz + float2 * num1 + y * num2;
            // ISSUE: reference to a compiler-generated field
            this.m_SnapPoint.value = raycastPoint;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SnapPoint.value = raycastPoint;
          }
        }
      }
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public ZoneToolSystem.Mode m_Mode;
      [ReadOnly]
      public ZoneToolSystem.State m_State;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public bool m_Overwrite;
      [ReadOnly]
      public ControlPoint m_StartPoint;
      [ReadOnly]
      public NativeValue<ControlPoint> m_SnapPoint;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_BlockData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint = this.m_SnapPoint.value;
        if (controlPoint.Equals(new ControlPoint()))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, new CreationDefinition()
        {
          m_Prefab = this.m_Prefab,
          m_Original = controlPoint.m_OriginalEntity
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        Zoning component = new Zoning();
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == ZoneToolSystem.State.Dezoning)
        {
          component.m_Flags |= ZoningFlags.Dezone | ZoningFlags.Overwrite;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Overwrite)
            component.m_Flags |= ZoningFlags.Zone | ZoningFlags.Overwrite;
          else
            component.m_Flags |= ZoningFlags.Zone;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ZoneToolSystem.Mode mode = this.m_Mode;
        switch (mode)
        {
          case ZoneToolSystem.Mode.FloodFill:
            component.m_Flags |= ZoningFlags.FloodFill;
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != ZoneToolSystem.State.Default)
            {
              // ISSUE: reference to a compiler-generated field
              float3 hitPosition1 = this.m_StartPoint.m_HitPosition;
              float3 hitPosition2 = controlPoint.m_HitPosition;
              component.m_Position = new Quad3(hitPosition1, hitPosition1, hitPosition2, hitPosition2);
              break;
            }
            float3 hitPosition3 = controlPoint.m_HitPosition;
            component.m_Position = new Quad3(hitPosition3, hitPosition3, hitPosition3, hitPosition3);
            break;
          case ZoneToolSystem.Mode.Marquee:
            component.m_Flags |= ZoningFlags.Marquee;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 y1 = (float3) 0.0f with
            {
              xz = this.m_State == ZoneToolSystem.State.Default || !this.m_BlockData.HasComponent(this.m_StartPoint.m_OriginalEntity) ? (this.m_State != ZoneToolSystem.State.Default || !this.m_BlockData.HasComponent(controlPoint.m_OriginalEntity) ? math.normalizesafe(this.m_CameraRight.xz) : this.m_BlockData[controlPoint.m_OriginalEntity].m_Direction) : this.m_BlockData[this.m_StartPoint.m_OriginalEntity].m_Direction
            };
            float3 y2 = (float3) 0.0f with
            {
              xz = MathUtils.Right(y1.xz)
            };
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != ZoneToolSystem.State.Default)
            {
              // ISSUE: reference to a compiler-generated field
              float3 hitPosition4 = this.m_StartPoint.m_HitPosition;
              float3 x = controlPoint.m_HitPosition - hitPosition4;
              float num1 = math.dot(x, y1);
              float num2 = math.dot(x, y2);
              if ((double) num1 < 0.0)
              {
                y1 = -y1;
                num1 = -num1;
              }
              if ((double) num2 < 0.0)
              {
                y2 = -y2;
                num2 = -num2;
              }
              component.m_Position.a = hitPosition4 - (y1 + y2) * 4f;
              component.m_Position.b = hitPosition4 - y1 * 4f + y2 * (num2 + 4f);
              component.m_Position.c = hitPosition4 + y1 * (num1 + 4f) + y2 * (num2 + 4f);
              component.m_Position.d = hitPosition4 + y1 * (num1 + 4f) - y2 * 4f;
              break;
            }
            float3 float3 = y1 * 4f;
            y2 *= 4f;
            float3 hitPosition5 = controlPoint.m_HitPosition;
            component.m_Position.a = hitPosition5 - float3 - y2;
            component.m_Position.b = hitPosition5 - float3 + y2;
            component.m_Position.c = hitPosition5 + float3 + y2;
            component.m_Position.d = hitPosition5 + float3 - y2;
            break;
          case ZoneToolSystem.Mode.Paint:
            component.m_Flags |= ZoningFlags.Paint;
            // ISSUE: reference to a compiler-generated field
            if (this.m_State != ZoneToolSystem.State.Default)
            {
              // ISSUE: reference to a compiler-generated field
              float3 hitPosition6 = this.m_StartPoint.m_HitPosition;
              float3 hitPosition7 = controlPoint.m_HitPosition;
              component.m_Position = new Quad3(hitPosition6, hitPosition6, hitPosition7, hitPosition7);
              break;
            }
            float3 hitPosition8 = controlPoint.m_HitPosition;
            component.m_Position = new Quad3(hitPosition8, hitPosition8, hitPosition8, hitPosition8);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        component.m_Position.a.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, component.m_Position.a);
        // ISSUE: reference to a compiler-generated field
        component.m_Position.b.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, component.m_Position.b);
        // ISSUE: reference to a compiler-generated field
        component.m_Position.c.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, component.m_Position.c);
        // ISSUE: reference to a compiler-generated field
        component.m_Position.d.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, component.m_Position.d);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Zoning>(entity, component);
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> __Game_Zones_Block_RO_ComponentTypeHandle;
      public BufferTypeHandle<Cell> __Game_Zones_Cell_RW_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Cell> __Game_Zones_Cell_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RW_BufferTypeHandle = state.GetBufferTypeHandle<Cell>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferTypeHandle = state.GetBufferTypeHandle<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
      }
    }
  }
}
