// Decompiled with JetBrains decompiler
// Type: Game.Tools.SelectionToolSystem
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
  public class SelectionToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Selection Tool";
    private SearchSystem m_AreaSearchSystem;
    private MapTileSystem m_MapTileSystem;
    private MapTilePurchaseSystem m_MapTilePurchaseSystem;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private AudioManager m_AudioManager;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_DefinitionGroup;
    private EntityQuery m_TempGroup;
    private EntityQuery m_SoundQuery;
    private Entity m_SelectionEntity;
    private Entity m_LastOwner;
    private SelectionType m_LastType;
    private EntityArchetype m_SelectionArchetype;
    private SelectionToolSystem.State m_State;
    private ControlPoint m_StartPoint;
    private ControlPoint m_RaycastPoint;
    private IProxyAction m_SelectArea;
    private IProxyAction m_DeselectArea;
    private IProxyAction m_DiscardSelect;
    private IProxyAction m_DiscardDeselect;
    private bool m_ApplyBlocked;
    private SelectionToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Selection Tool";

    public SelectionType selectionType { get; set; }

    public Entity selectionOwner { get; set; }

    public SelectionToolSystem.State state => this.m_State;

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_SelectArea;
        yield return this.m_DeselectArea;
        yield return this.m_DiscardSelect;
        yield return this.m_DiscardDeselect;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem = this.World.GetOrCreateSystemManaged<MapTileSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilePurchaseSystem = this.World.GetOrCreateSystemManaged<MapTilePurchaseSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionGroup = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_TempGroup = this.GetEntityQuery(ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_SelectionArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<SelectionInfo>(), ComponentType.ReadWrite<SelectionElement>());
      // ISSUE: reference to a compiler-generated field
      this.m_SelectArea = InputManager.instance.toolActionCollection.GetActionState("Select Area", nameof (SelectionToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DeselectArea = InputManager.instance.toolActionCollection.GetActionState("Deselect Area", nameof (SelectionToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardSelect = InputManager.instance.toolActionCollection.GetActionState("Discard Select", nameof (SelectionToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_DiscardDeselect = InputManager.instance.toolActionCollection.GetActionState("Discard Deselect", nameof (SelectionToolSystem));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_State = SelectionToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyBlocked = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectionEntity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.DestroyEntity(this.m_SelectionEntity);
        // ISSUE: reference to a compiler-generated field
        this.m_SelectionEntity = Entity.Null;
      }
      // ISSUE: reference to a compiler-generated method
      base.OnStopRunning();
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        if (this.selectionType == SelectionType.MapTiles)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_ToolSystem.actionMode.IsGame() && this.m_MapTilePurchaseSystem.GetAvailableTiles() == 0)
          {
            this.applyAction.enabled = false;
            this.applyActionOverride = (IProxyAction) null;
            this.secondaryApplyAction.enabled = false;
            this.secondaryApplyActionOverride = (IProxyAction) null;
            this.cancelAction.enabled = false;
            this.cancelActionOverride = (IProxyAction) null;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempGroup.CalculateEntityCount() <= 1)
            {
              this.applyAction.enabled = this.actionsEnabled;
              // ISSUE: reference to a compiler-generated field
              this.applyActionOverride = this.m_SelectArea;
              this.secondaryApplyAction.enabled = this.actionsEnabled;
              // ISSUE: reference to a compiler-generated field
              this.secondaryApplyActionOverride = this.m_DeselectArea;
              this.cancelAction.enabled = false;
              this.cancelActionOverride = (IProxyAction) null;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              SelectionToolSystem.State state = this.m_State;
              switch (state)
              {
                case SelectionToolSystem.State.Default:
                  this.applyAction.enabled = this.actionsEnabled;
                  // ISSUE: reference to a compiler-generated field
                  this.applyActionOverride = this.m_SelectArea;
                  this.secondaryApplyAction.enabled = this.actionsEnabled;
                  // ISSUE: reference to a compiler-generated field
                  this.secondaryApplyActionOverride = this.m_DeselectArea;
                  this.cancelAction.enabled = false;
                  this.cancelActionOverride = (IProxyAction) null;
                  break;
                case SelectionToolSystem.State.Selecting:
                  this.applyAction.enabled = this.actionsEnabled;
                  // ISSUE: reference to a compiler-generated field
                  this.applyActionOverride = this.m_SelectArea;
                  this.secondaryApplyAction.enabled = false;
                  this.secondaryApplyActionOverride = (IProxyAction) null;
                  this.cancelAction.enabled = this.actionsEnabled;
                  // ISSUE: reference to a compiler-generated field
                  this.cancelActionOverride = this.m_DiscardSelect;
                  break;
                case SelectionToolSystem.State.Deselecting:
                  this.applyAction.enabled = false;
                  this.applyActionOverride = (IProxyAction) null;
                  this.secondaryApplyAction.enabled = this.actionsEnabled;
                  // ISSUE: reference to a compiler-generated field
                  this.secondaryApplyActionOverride = this.m_DeselectArea;
                  this.cancelAction.enabled = this.actionsEnabled;
                  // ISSUE: reference to a compiler-generated field
                  this.cancelActionOverride = this.m_DiscardDeselect;
                  break;
              }
            }
          }
        }
        else
        {
          this.applyAction.enabled = this.actionsEnabled;
          // ISSUE: reference to a compiler-generated field
          this.applyActionOverride = this.m_SelectArea;
          this.secondaryApplyAction.enabled = this.actionsEnabled;
          // ISSUE: reference to a compiler-generated field
          this.secondaryApplyActionOverride = this.m_DeselectArea;
          this.cancelAction.enabled = false;
          this.cancelActionOverride = (IProxyAction) null;
        }
      }
    }

    public override PrefabBase GetPrefab() => (PrefabBase) null;

    public override bool TrySetPrefab(PrefabBase prefab) => false;

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
      switch (this.selectionType)
      {
        case SelectionType.ServiceDistrict:
        case SelectionType.MapTiles:
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Areas | TypeMask.Water;
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask = TypeMask.None;
          break;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolRaycastSystem.areaTypeMask = AreaUtils.GetTypeMask(this.GetAreaType(this.selectionType));
    }

    private AreaType GetAreaType(SelectionType selectionType)
    {
      if (selectionType == SelectionType.ServiceDistrict)
        return AreaType.District;
      return selectionType == SelectionType.MapTiles ? AreaType.MapTile : AreaType.None;
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastOwner != this.selectionOwner || this.m_LastType != this.selectionType || this.m_SelectionEntity == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectionEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.DestroyEntity(this.m_SelectionEntity);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SelectionEntity = this.EntityManager.CreateEntity(this.m_SelectionArchetype);
        SelectionInfo componentData;
        componentData.m_SelectionType = this.selectionType;
        // ISSUE: reference to a compiler-generated method
        componentData.m_AreaType = this.GetAreaType(this.selectionType);
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.SetComponentData<SelectionInfo>(this.m_SelectionEntity, componentData);
        if (this.selectionOwner != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<Owner>(this.m_SelectionEntity, new Owner(this.selectionOwner));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastOwner = this.selectionOwner;
        // ISSUE: reference to a compiler-generated field
        this.m_LastType = this.selectionType;
        // ISSUE: reference to a compiler-generated field
        this.requireAreas = this.m_ToolRaycastSystem.areaTypeMask;
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.CopySelection(inputDeps);
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != SelectionToolSystem.State.Default && !this.applyAction.enabled && !this.secondaryApplyAction.enabled)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_State = SelectionToolSystem.State.Default;
      }
      // ISSUE: reference to a compiler-generated field
      if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        SelectionToolSystem.State state = this.m_State;
        switch (state)
        {
          case SelectionToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated field
            if (this.m_ApplyBlocked)
            {
              if (this.applyAction.WasReleasedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame())
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
          case SelectionToolSystem.State.Selecting:
            if (this.cancelAction.WasPressedThisFrame())
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ApplyBlocked = true;
              // ISSUE: reference to a compiler-generated method
              return this.Cancel(inputDeps);
            }
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            return this.applyAction.WasPressedThisFrame() || this.applyAction.WasReleasedThisFrame() ? this.Apply(inputDeps) : this.Update(inputDeps);
          case SelectionToolSystem.State.Deselecting:
            if (this.cancelAction.WasPressedThisFrame())
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ApplyBlocked = true;
              // ISSUE: reference to a compiler-generated method
              return this.Apply(inputDeps);
            }
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            return this.secondaryApplyAction.WasPressedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame() ? this.Cancel(inputDeps) : this.Update(inputDeps);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != SelectionToolSystem.State.Default && (this.applyAction.WasReleasedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame()))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = SelectionToolSystem.State.Default;
      }
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      SelectionToolSystem.State state = this.m_State;
      switch (state)
      {
        case SelectionToolSystem.State.Selecting:
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = new ControlPoint();
          // ISSUE: reference to a compiler-generated field
          this.m_State = SelectionToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GetRaycastResult(out this.m_RaycastPoint);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_AreaMarqueeEndSound);
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
        case SelectionToolSystem.State.Deselecting:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.m_RaycastPoint.Equals(new ControlPoint()) && this.GetAllowApply())
          {
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.ToggleTempEntity(inputDeps, false);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateSelection(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distance(this.m_StartPoint.m_Position, this.m_RaycastPoint.m_Position) > 1.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_AreaMarqueeClearEndSound);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = new ControlPoint();
          // ISSUE: reference to a compiler-generated field
          this.m_State = SelectionToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GetRaycastResult(out this.m_RaycastPoint);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
        default:
          // ISSUE: reference to a compiler-generated field
          if (!this.m_RaycastPoint.Equals(new ControlPoint()))
          {
            if (singleFrameOnly)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.GetAllowApply())
              {
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.ToggleTempEntity(inputDeps, false);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.UpdateSelection(inputDeps);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.GetRaycastResult(out this.m_RaycastPoint);
                this.applyMode = ApplyMode.Clear;
                // ISSUE: reference to a compiler-generated method
                return this.UpdateDefinitions(inputDeps);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StartPoint = this.m_RaycastPoint;
              // ISSUE: reference to a compiler-generated field
              this.m_State = SelectionToolSystem.State.Deselecting;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_AreaMarqueeClearStartSound);
          }
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      SelectionToolSystem.State state = this.m_State;
      switch (state)
      {
        case SelectionToolSystem.State.Selecting:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.m_RaycastPoint.Equals(new ControlPoint()) && this.GetAllowApply())
          {
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.ToggleTempEntity(inputDeps, true);
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.UpdateSelection(inputDeps);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distance(this.m_StartPoint.m_Position, this.m_RaycastPoint.m_Position) > 1.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_AreaMarqueeEndSound);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = new ControlPoint();
          // ISSUE: reference to a compiler-generated field
          this.m_State = SelectionToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GetRaycastResult(out this.m_RaycastPoint);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
        case SelectionToolSystem.State.Deselecting:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) math.distance(this.m_StartPoint.m_Position, this.m_RaycastPoint.m_Position) > 1.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_AreaMarqueeClearEndSound);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = new ControlPoint();
          // ISSUE: reference to a compiler-generated field
          this.m_State = SelectionToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GetRaycastResult(out this.m_RaycastPoint);
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
        default:
          // ISSUE: reference to a compiler-generated field
          if (!this.m_RaycastPoint.Equals(new ControlPoint()))
          {
            if (singleFrameOnly)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.GetAllowApply())
              {
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.ToggleTempEntity(inputDeps, true);
                // ISSUE: reference to a compiler-generated method
                inputDeps = this.UpdateSelection(inputDeps);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.GetRaycastResult(out this.m_RaycastPoint);
                this.applyMode = ApplyMode.Clear;
                // ISSUE: reference to a compiler-generated method
                return this.UpdateDefinitions(inputDeps);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StartPoint = this.m_RaycastPoint;
              // ISSUE: reference to a compiler-generated field
              this.m_State = SelectionToolSystem.State.Selecting;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_AreaMarqueeStartSound);
          }
          // ISSUE: reference to a compiler-generated method
          return this.Update(inputDeps);
      }
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      SelectionToolSystem.State state = this.m_State;
      switch (state)
      {
        case SelectionToolSystem.State.Selecting:
        case SelectionToolSystem.State.Deselecting:
          ControlPoint controlPoint1;
          bool forceUpdate1;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetRaycastResult(out controlPoint1, out forceUpdate1) && controlPoint1.Equals(this.m_RaycastPoint) && !forceUpdate1)
          {
            this.applyMode = ApplyMode.None;
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_RaycastPoint = controlPoint1;
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
        default:
          ControlPoint controlPoint2;
          bool forceUpdate2;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.GetRaycastResult(out controlPoint2, out forceUpdate2) && controlPoint2.m_OriginalEntity == this.m_RaycastPoint.m_OriginalEntity && !forceUpdate2 && !this.EntityManager.HasComponent<Updated>(this.m_RaycastPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_RaycastPoint = controlPoint2;
            this.applyMode = ApplyMode.None;
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_RaycastPoint = controlPoint2;
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
      }
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      return inputDeps;
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionGroup, this.m_ToolOutputBarrier, inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != SelectionToolSystem.State.Default || this.m_RaycastPoint.m_OriginalEntity != Entity.Null)
      {
        NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        Quad3 quad;
        // ISSUE: reference to a compiler-generated method
        this.GetSelectionQuad(out quad);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
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
        SelectionToolSystem.FindEntitiesJob jobData1 = new SelectionToolSystem.FindEntitiesJob()
        {
          m_StartPoint = this.m_State != SelectionToolSystem.State.Default ? this.m_StartPoint : new ControlPoint(),
          m_EndPoint = this.m_RaycastPoint,
          m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
          m_SelectionQuad = quad.xz,
          m_AreaType = this.GetAreaType(this.selectionType),
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_AreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
          m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
          m_Entities = nativeList
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        SelectionToolSystem.CreateDefinitionsJob jobData2 = new SelectionToolSystem.CreateDefinitionsJob()
        {
          m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
          m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
          m_MapTileData = this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup,
          m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_Entities = nativeList.AsDeferredJobArray(),
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
        };
        JobHandle jobHandle1 = jobData1.Schedule<SelectionToolSystem.FindEntitiesJob>(JobHandle.CombineDependencies(inputDeps, dependencies));
        NativeList<Entity> list = nativeList;
        JobHandle dependsOn = jobHandle1;
        JobHandle jobHandle2 = jobData2.Schedule<SelectionToolSystem.CreateDefinitionsJob, Entity>(list, 4, dependsOn);
        nativeList.Dispose(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle1);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle2);
        job0 = JobHandle.CombineDependencies(job0, jobHandle2);
      }
      return job0;
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.selectionType != SelectionType.MapTiles || !this.m_ToolSystem.actionMode.IsGame() || this.m_MapTilePurchaseSystem.GetAvailableTiles() != 0)
      {
        // ISSUE: reference to a compiler-generated method
        return base.GetRaycastResult(out controlPoint);
      }
      controlPoint = new ControlPoint();
      return false;
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint, out bool forceUpdate)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.selectionType != SelectionType.MapTiles || !this.m_ToolSystem.actionMode.IsGame() || this.m_MapTilePurchaseSystem.GetAvailableTiles() != 0)
      {
        // ISSUE: reference to a compiler-generated method
        return base.GetRaycastResult(out controlPoint, out forceUpdate);
      }
      controlPoint = new ControlPoint();
      forceUpdate = false;
      return false;
    }

    private JobHandle ToggleTempEntity(JobHandle inputDeps, bool select)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_TempGroup.IsEmptyIgnoreFilter)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_SelectionElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      return new SelectionToolSystem.ToggleEntityJob()
      {
        m_SelectionEntity = this.m_SelectionEntity,
        m_Select = select,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_NativeType = this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle,
        m_MapTileType = this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentTypeHandle,
        m_SelectionElements = this.__TypeHandle.__Game_Tools_SelectionElement_RW_BufferLookup
      }.Schedule<SelectionToolSystem.ToggleEntityJob>(this.m_TempGroup, inputDeps);
    }

    private JobHandle CopySelection(JobHandle inputDeps)
    {
      switch (this.selectionType)
      {
        case SelectionType.ServiceDistrict:
          // ISSUE: reference to a compiler-generated method
          return this.CopyServiceDistricts(inputDeps);
        case SelectionType.MapTiles:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return this.m_ToolSystem.actionMode.IsEditor() ? this.CopyStartTiles(inputDeps) : inputDeps;
        default:
          return inputDeps;
      }
    }

    private JobHandle UpdateSelection(JobHandle inputDeps)
    {
      switch (this.selectionType)
      {
        case SelectionType.ServiceDistrict:
          // ISSUE: reference to a compiler-generated method
          return this.UpdateServiceDistricts(inputDeps);
        case SelectionType.MapTiles:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return this.m_ToolSystem.actionMode.IsEditor() ? this.UpdateStartTiles(inputDeps) : inputDeps;
        default:
          return inputDeps;
      }
    }

    private JobHandle CopyStartTiles(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_SelectionElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new SelectionToolSystem.CopyStartTilesJob()
      {
        m_SelectionEntity = this.m_SelectionEntity,
        m_StartTiles = this.m_MapTileSystem.GetStartTiles(),
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SelectionElements = this.__TypeHandle.__Game_Tools_SelectionElement_RW_BufferLookup
      }.Schedule<SelectionToolSystem.CopyStartTilesJob>(inputDeps);
    }

    private JobHandle UpdateStartTiles(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_SelectionElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle producerJob = new SelectionToolSystem.UpdateStartTilesJob()
      {
        m_SelectionEntity = this.m_SelectionEntity,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SelectionElements = this.__TypeHandle.__Game_Tools_SelectionElement_RO_BufferLookup,
        m_StartTiles = this.m_MapTileSystem.GetStartTiles(),
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<SelectionToolSystem.UpdateStartTilesJob>(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(producerJob);
      return producerJob;
    }

    private JobHandle CopyServiceDistricts(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_SelectionElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new SelectionToolSystem.CopyServiceDistrictsJob()
      {
        m_SelectionEntity = this.m_SelectionEntity,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ServiceDistricts = this.__TypeHandle.__Game_Areas_ServiceDistrict_RO_BufferLookup,
        m_SelectionElements = this.__TypeHandle.__Game_Tools_SelectionElement_RW_BufferLookup,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<SelectionToolSystem.CopyServiceDistrictsJob>(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(producerJob);
      return producerJob;
    }

    private JobHandle UpdateServiceDistricts(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_ServiceDistrict_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_SelectionElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle producerJob = new SelectionToolSystem.UpdateServiceDistrictsJob()
      {
        m_SelectionEntity = this.m_SelectionEntity,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_SelectionElements = this.__TypeHandle.__Game_Tools_SelectionElement_RO_BufferLookup,
        m_ServiceDistricts = this.__TypeHandle.__Game_Areas_ServiceDistrict_RW_BufferLookup,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<SelectionToolSystem.UpdateServiceDistrictsJob>(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(producerJob);
      return producerJob;
    }

    public bool GetSelectionQuad(out Quad3 quad)
    {
      Camera main = Camera.main;
      if ((Object) main == (Object) null)
      {
        quad = new Quad3();
        return false;
      }
      UnityEngine.Transform transform = main.transform;
      float3 float3 = new float3();
      float3.xz = ((float3) transform.right).xz;
      float3 = math.normalizesafe(float3);
      float3 y = new float3();
      y.xz = MathUtils.Right(float3.xz);
      // ISSUE: reference to a compiler-generated field
      float3 hitPosition = this.m_StartPoint.m_HitPosition;
      // ISSUE: reference to a compiler-generated field
      float3 x = this.m_RaycastPoint.m_HitPosition - hitPosition;
      float num1 = math.dot(x, float3);
      float num2 = math.dot(x, y);
      if ((double) num1 < 0.0)
      {
        float3 = -float3;
        num1 = -num1;
      }
      if ((double) num2 < 0.0)
      {
        y = -y;
        num2 = -num2;
      }
      quad.a = hitPosition;
      quad.b = hitPosition + y * num2;
      quad.c = hitPosition + float3 * num1 + y * num2;
      quad.d = hitPosition + float3 * num1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      WaterSurfaceData surfaceData = this.m_WaterSystem.GetSurfaceData(out deps);
      deps.Complete();
      quad.a.y = WaterUtils.SampleHeight(ref surfaceData, ref heightData, quad.a);
      quad.b.y = WaterUtils.SampleHeight(ref surfaceData, ref heightData, quad.b);
      quad.c.y = WaterUtils.SampleHeight(ref surfaceData, ref heightData, quad.c);
      quad.d.y = WaterUtils.SampleHeight(ref surfaceData, ref heightData, quad.d);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return !this.m_StartPoint.Equals(new ControlPoint()) && !this.m_RaycastPoint.Equals(new ControlPoint());
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
    public SelectionToolSystem()
    {
    }

    public enum State
    {
      Default,
      Selecting,
      Deselecting,
    }

    [BurstCompile]
    private struct FindEntitiesJob : IJob
    {
      [ReadOnly]
      public ControlPoint m_StartPoint;
      [ReadOnly]
      public ControlPoint m_EndPoint;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public Quad2 m_SelectionQuad;
      [ReadOnly]
      public AreaType m_AreaType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_AreaGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      public NativeList<Entity> m_Entities;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_StartPoint.m_OriginalEntity != Entity.Null && this.m_AreaType != AreaType.None && this.m_Nodes.HasBuffer(this.m_StartPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Entities.Add(in this.m_StartPoint.m_OriginalEntity);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_EndPoint.m_OriginalEntity != Entity.Null && this.m_AreaType != AreaType.None && this.m_Nodes.HasBuffer(this.m_EndPoint.m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Entities.Add(in this.m_EndPoint.m_OriginalEntity);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_StartPoint.Equals(new ControlPoint()) && !this.m_EndPoint.Equals(new ControlPoint()) && this.m_AreaType != AreaType.None)
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
          SelectionToolSystem.FindEntitiesJob.AreaIterator iterator = new SelectionToolSystem.FindEntitiesJob.AreaIterator()
          {
            m_Quad = this.m_SelectionQuad,
            m_AreaType = this.m_AreaType,
            m_PrefabRefData = this.m_PrefabRefData,
            m_AreaGeometryData = this.m_AreaGeometryData,
            m_Nodes = this.m_Nodes,
            m_Triangles = this.m_Triangles,
            m_Entities = this.m_Entities
          };
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<SelectionToolSystem.FindEntitiesJob.AreaIterator>(ref iterator);
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> entities = this.m_Entities;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SelectionToolSystem.FindEntitiesJob.EntityComparer entityComparer = new SelectionToolSystem.FindEntitiesJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        SelectionToolSystem.FindEntitiesJob.EntityComparer comp = entityComparer;
        entities.Sort<Entity, SelectionToolSystem.FindEntitiesJob.EntityComparer>(comp);
        Entity entity1 = Entity.Null;
        int num = 0;
        int index = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_Entities.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_Entities[num++];
          if (entity2 != entity1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Entities[index++] = entity2;
            entity1 = entity2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index >= this.m_Entities.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Entities.RemoveRange(index, this.m_Entities.Length - index);
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Quad2 m_Quad;
        public AreaType m_AreaType;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<AreaGeometryData> m_AreaGeometryData;
        public BufferLookup<Game.Areas.Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public NativeList<Entity> m_Entities;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Quad);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Quad) || this.m_AreaGeometryData[this.m_PrefabRefData[areaItem.m_Area].m_Prefab].m_Type != this.m_AreaType || !MathUtils.Intersect(this.m_Quad, AreaUtils.GetTriangle2(this.m_Nodes[areaItem.m_Area], this.m_Triangles[areaItem.m_Area][areaItem.m_Triangle])))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Entities.Add(in areaItem.m_Area);
        }
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<MapTile> m_MapTileData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_AreaNodes.HasBuffer(entity1) || !this.m_EditorMode && this.m_MapTileData.HasComponent(entity1) && !this.m_NativeData.HasComponent(entity1))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = this.m_CommandBuffer.CreateEntity(index);
        CreationDefinition component = new CreationDefinition();
        component.m_Original = entity1;
        component.m_Flags |= CreationFlags.Select;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(index, entity2, component);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(index, entity2, new Updated());
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[entity1];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> dynamicBuffer = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(index, entity2);
        dynamicBuffer.ResizeUninitialized(areaNode.Length);
        dynamicBuffer.CopyFrom(areaNode.AsNativeArray());
      }
    }

    [BurstCompile]
    private struct ToggleEntityJob : IJobChunk
    {
      [ReadOnly]
      public Entity m_SelectionEntity;
      [ReadOnly]
      public bool m_Select;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Native> m_NativeType;
      [ReadOnly]
      public ComponentTypeHandle<MapTile> m_MapTileType;
      public BufferLookup<SelectionElement> m_SelectionElements;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode && chunk.Has<MapTile>(ref this.m_MapTileType) && !chunk.Has<Native>(ref this.m_NativeType))
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray = chunk.GetNativeArray<Temp>(ref this.m_TempType);
label_13:
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Temp temp = nativeArray[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (temp.m_Original != Entity.Null && this.m_SelectionElements.HasBuffer(this.m_SelectionEntity))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SelectionElement> selectionElement = this.m_SelectionElements[this.m_SelectionEntity];
            for (int index2 = 0; index2 < selectionElement.Length; ++index2)
            {
              if (selectionElement[index2].m_Entity.Equals(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_Select)
                {
                  selectionElement.RemoveAt(index2);
                  goto label_13;
                }
                else
                  goto label_13;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_Select)
              selectionElement.Add(new SelectionElement(temp.m_Original));
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
    private struct CopyStartTilesJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectionEntity;
      [ReadOnly]
      public NativeList<Entity> m_StartTiles;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public BufferLookup<SelectionElement> m_SelectionElements;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SelectionElements.HasBuffer(this.m_SelectionEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SelectionElement> selectionElement = this.m_SelectionElements[this.m_SelectionEntity];
        selectionElement.Clear();
        // ISSUE: reference to a compiler-generated field
        selectionElement.EnsureCapacity(this.m_StartTiles.Length);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_StartTiles.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity startTile = this.m_StartTiles[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(startTile))
            selectionElement.Add(new SelectionElement(startTile));
        }
      }
    }

    [BurstCompile]
    private struct UpdateStartTilesJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectionEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<SelectionElement> m_SelectionElements;
      public NativeList<Entity> m_StartTiles;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SelectionElements.HasBuffer(this.m_SelectionEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SelectionElement> selectionElement = this.m_SelectionElements[this.m_SelectionEntity];
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_StartTiles.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity startTile = this.m_StartTiles[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(startTile))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(startTile, new Updated());
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_StartTiles.ResizeUninitialized(selectionElement.Length);
        for (int index = 0; index < selectionElement.Length; ++index)
        {
          Entity entity = selectionElement[index].m_Entity;
          // ISSUE: reference to a compiler-generated field
          this.m_StartTiles[index] = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
        }
      }
    }

    [BurstCompile]
    private struct CopyServiceDistrictsJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectionEntity;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public BufferLookup<SelectionElement> m_SelectionElements;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnerData.HasComponent(this.m_SelectionEntity) || !this.m_SelectionElements.HasBuffer(this.m_SelectionEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[this.m_SelectionEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SelectionElement> selectionElement = this.m_SelectionElements[this.m_SelectionEntity];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ServiceDistricts.HasBuffer(owner.m_Owner))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceDistrict> serviceDistrict = this.m_ServiceDistricts[owner.m_Owner];
        selectionElement.Clear();
        selectionElement.EnsureCapacity(serviceDistrict.Length);
        for (int index = 0; index < serviceDistrict.Length; ++index)
        {
          Entity district = serviceDistrict[index].m_District;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(district))
            selectionElement.Add(new SelectionElement(district));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(owner.m_Owner);
      }
    }

    [BurstCompile]
    private struct UpdateServiceDistrictsJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectionEntity;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public BufferLookup<SelectionElement> m_SelectionElements;
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnerData.HasComponent(this.m_SelectionEntity) || !this.m_SelectionElements.HasBuffer(this.m_SelectionEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[this.m_SelectionEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SelectionElement> selectionElement = this.m_SelectionElements[this.m_SelectionEntity];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ServiceDistricts.HasBuffer(owner.m_Owner))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ServiceDistrict> serviceDistrict = this.m_ServiceDistricts[owner.m_Owner];
        serviceDistrict.ResizeUninitialized(selectionElement.Length);
        for (int index = 0; index < selectionElement.Length; ++index)
          serviceDistrict[index] = new ServiceDistrict(selectionElement[index].m_Entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(owner.m_Owner);
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MapTile> __Game_Areas_MapTile_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Native> __Game_Common_Native_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MapTile> __Game_Areas_MapTile_RO_ComponentTypeHandle;
      public BufferLookup<SelectionElement> __Game_Tools_SelectionElement_RW_BufferLookup;
      [ReadOnly]
      public BufferLookup<SelectionElement> __Game_Tools_SelectionElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> __Game_Areas_ServiceDistrict_RO_BufferLookup;
      public BufferLookup<ServiceDistrict> __Game_Areas_ServiceDistrict_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapTile_RO_ComponentLookup = state.GetComponentLookup<MapTile>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapTile_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MapTile>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_SelectionElement_RW_BufferLookup = state.GetBufferLookup<SelectionElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_SelectionElement_RO_BufferLookup = state.GetBufferLookup<SelectionElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_ServiceDistrict_RO_BufferLookup = state.GetBufferLookup<ServiceDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_ServiceDistrict_RW_BufferLookup = state.GetBufferLookup<ServiceDistrict>();
      }
    }
  }
}
