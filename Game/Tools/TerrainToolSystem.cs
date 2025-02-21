// Decompiled with JetBrains decompiler
// Type: Game.Tools.TerrainToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Audio;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
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
  public class TerrainToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Terrain Tool";
    public const string kTerrainToolKeyGroup = "tool/terrain";
    private AudioManager m_AudioManager;
    private AudioSource m_AudioSource;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_BrushQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_SoundQuery;
    private EntityQuery m_VisibleQuery;
    private IProxyAction m_EraseMaterial;
    private IProxyAction m_EraseResource;
    private IProxyAction m_FastSoften;
    private IProxyAction m_LevelTerrain;
    private IProxyAction m_LowerTerrain;
    private IProxyAction m_PaintMaterial;
    private IProxyAction m_PaintResource;
    private IProxyAction m_RaiseTerrain;
    private IProxyAction m_SetLevelTarget;
    private IProxyAction m_SetSlopeTarget;
    private IProxyAction m_SlopeTerrain;
    private IProxyAction m_SoftenTerrain;
    private ControlPoint m_RaycastPoint;
    private ControlPoint m_StartPoint;
    private float3 m_TargetPosition;
    private float3 m_ApplyPosition;
    private bool m_TargetSet;
    private TerrainToolSystem.State m_State;
    private TerrainToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Terrain Tool";

    public TerraformingPrefab prefab { get; private set; }

    public override bool brushing => true;

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_EraseMaterial;
        yield return this.m_EraseResource;
        yield return this.m_FastSoften;
        yield return this.m_LevelTerrain;
        yield return this.m_LowerTerrain;
        yield return this.m_PaintMaterial;
        yield return this.m_PaintResource;
        yield return this.m_RaiseTerrain;
        yield return this.m_SetLevelTarget;
        yield return this.m_SetSlopeTarget;
        yield return this.m_SlopeTerrain;
        yield return this.m_SoftenTerrain;
      }
    }

    public float brushHeight
    {
      get => !this.m_TargetSet ? WaterSystem.SeaLevel : this.m_TargetPosition.y;
      set
      {
        this.m_TargetPosition.y = value;
        this.m_TargetSet = true;
      }
    }

    public void SetPrefab(TerraformingPrefab value)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TargetSet = false;
      // ISSUE: reference to a compiler-generated field
      this.m_TargetPosition = new float3(0.0f, 0.0f, 0.0f);
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyPosition = new float3(0.0f, 0.0f, 0.0f);
      this.prefab = value;
      if (!this.Enabled)
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BrushQuery = this.GetBrushQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_VisibleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Brush>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Brush>(), ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EraseMaterial = InputManager.instance.toolActionCollection.GetActionState("Erase Material", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_EraseResource = InputManager.instance.toolActionCollection.GetActionState("Erase Resource", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_FastSoften = InputManager.instance.toolActionCollection.GetActionState("Fast Soften", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_LevelTerrain = InputManager.instance.toolActionCollection.GetActionState("Level Terrain", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_LowerTerrain = InputManager.instance.toolActionCollection.GetActionState("Lower Terrain", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_PaintMaterial = InputManager.instance.toolActionCollection.GetActionState("Paint Material", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_PaintResource = InputManager.instance.toolActionCollection.GetActionState("Paint Resource", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_RaiseTerrain = InputManager.instance.toolActionCollection.GetActionState("Raise Terrain", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_SetLevelTarget = InputManager.instance.toolActionCollection.GetActionState("Set Level Target", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_SetSlopeTarget = InputManager.instance.toolActionCollection.GetActionState("Set Slope Target", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_SlopeTerrain = InputManager.instance.toolActionCollection.GetActionState("Slope Terrain", nameof (TerrainToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_SoftenTerrain = InputManager.instance.toolActionCollection.GetActionState("Soften Terrain", nameof (TerrainToolSystem));
      this.brushSize = 100f;
      this.brushAngle = 0.0f;
      this.brushStrength = 0.5f;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.brushType = this.FindDefaultBrush(this.m_BrushQuery);
      this.brushSize = 100f;
      this.brushAngle = 0.0f;
      this.brushStrength = 0.5f;
    }

    public void SetDisableFX()
    {
      // ISSUE: reference to a compiler-generated field
      if (!((Object) this.m_AudioSource != (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.StopExclusiveUISound(this.m_AudioSource);
      // ISSUE: reference to a compiler-generated field
      this.m_AudioSource = (AudioSource) null;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_RaycastPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = TerrainToolSystem.State.Default;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        this.applyAction.enabled = this.actionsEnabled;
        this.secondaryApplyAction.enabled = this.actionsEnabled;
        if (this.prefab.m_Type == TerraformingType.Shift)
        {
          if (this.prefab.m_Target == TerraformingTarget.Height)
          {
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_RaiseTerrain;
            // ISSUE: reference to a compiler-generated field
            this.secondaryApplyActionOverride = this.m_LowerTerrain;
          }
          else if (this.prefab.m_Target == TerraformingTarget.Material)
          {
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_PaintMaterial;
            // ISSUE: reference to a compiler-generated field
            this.secondaryApplyActionOverride = this.m_EraseMaterial;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.applyActionOverride = this.m_PaintResource;
            // ISSUE: reference to a compiler-generated field
            this.secondaryApplyActionOverride = this.m_EraseResource;
          }
        }
        else if (this.prefab.m_Type == TerraformingType.Level)
        {
          // ISSUE: reference to a compiler-generated field
          this.applyActionOverride = this.m_LevelTerrain;
          // ISSUE: reference to a compiler-generated field
          this.secondaryApplyActionOverride = this.m_SetLevelTarget;
        }
        else if (this.prefab.m_Type == TerraformingType.Slope)
        {
          // ISSUE: reference to a compiler-generated field
          this.applyActionOverride = this.m_SlopeTerrain;
          // ISSUE: reference to a compiler-generated field
          this.secondaryApplyActionOverride = this.m_SetSlopeTarget;
        }
        else if (this.prefab.m_Type == TerraformingType.Soften)
        {
          // ISSUE: reference to a compiler-generated field
          this.applyActionOverride = this.m_SoftenTerrain;
          // ISSUE: reference to a compiler-generated field
          this.secondaryApplyActionOverride = this.m_FastSoften;
        }
        else
        {
          this.applyActionOverride = (IProxyAction) null;
          this.secondaryApplyActionOverride = (IProxyAction) null;
        }
      }
    }

    public override PrefabBase GetPrefab() => (PrefabBase) this.prefab;

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      if (!(prefab is TerraformingPrefab terraformingPrefab))
        return false;
      // ISSUE: reference to a compiler-generated method
      this.SetPrefab(terraformingPrefab);
      return true;
    }

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
      if ((Object) this.prefab != (Object) null && (Object) this.brushType != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Outside;
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
      if ((Object) this.brushType == (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.brushType = this.FindDefaultBrush(this.m_BrushQuery);
      }
      this.requireNet = Layer.Road | Layer.TrainTrack | Layer.Pathway | Layer.TramTrack | Layer.SubwayTrack | Layer.PublicTransportRoad;
      this.requirePipelines = true;
      // ISSUE: reference to a compiler-generated field
      if (this.m_FocusChanged)
        return inputDeps;
      // ISSUE: reference to a compiler-generated field
      if ((Object) this.prefab != (Object) null && (Object) this.brushType != (Object) null && this.m_HasFocus)
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
        if (this.m_State != TerrainToolSystem.State.Default && !this.applyAction.enabled)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_State = TerrainToolSystem.State.Default;
        }
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_State != TerrainToolSystem.State.Default)
          {
            if (this.applyAction.WasPressedThisFrame() || this.applyAction.WasReleasedThisFrame())
            {
              // ISSUE: reference to a compiler-generated method
              return this.Apply(inputDeps);
            }
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            return this.secondaryApplyAction.WasPressedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame() ? this.Cancel(inputDeps) : this.Update(inputDeps);
          }
          if (this.secondaryApplyAction.WasPressedThisFrame())
          {
            // ISSUE: reference to a compiler-generated method
            return this.Cancel(inputDeps, this.secondaryApplyAction.WasReleasedThisFrame());
          }
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          return this.applyAction.WasPressedThisFrame() ? this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame()) : this.Update(inputDeps);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != TerrainToolSystem.State.Default && (this.applyAction.WasReleasedThisFrame() || this.secondaryApplyAction.WasReleasedThisFrame() || !this.m_HasFocus))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = TerrainToolSystem.State.Default;
      }
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    public override void GetAvailableSnapMask(out Snap onMask, out Snap offMask)
    {
      // ISSUE: reference to a compiler-generated method
      base.GetAvailableSnapMask(out onMask, out offMask);
      if (!((Object) this.prefab != (Object) null) || this.prefab.m_Target != TerraformingTarget.Height)
        return;
      onMask |= Snap.ContourLines;
      offMask |= Snap.ContourLines;
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated method
      this.SetDisableFX();
      return inputDeps;
    }

    private JobHandle Cancel(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == TerrainToolSystem.State.Default)
      {
        // ISSUE: reference to a compiler-generated method
        this.applyMode = this.prefab.m_Type == TerraformingType.Slope || !this.GetAllowApply() ? ApplyMode.Clear : ApplyMode.Apply;
        if (!singleFrameOnly)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = this.m_RaycastPoint;
          // ISSUE: reference to a compiler-generated field
          this.m_State = TerrainToolSystem.State.Removing;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((Object) this.m_AudioSource == (Object) null && !this.m_ToolSystem.actionMode.IsEditor())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioSource = this.m_AudioManager.PlayExclusiveUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TerraformSound);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated field
        this.m_TargetSet = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TargetPosition = this.m_RaycastPoint.m_HitPosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.InvertBrushes(this.m_TempQuery, inputDeps);
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == TerrainToolSystem.State.Removing)
      {
        // ISSUE: reference to a compiler-generated method
        this.applyMode = this.GetAllowApply() ? ApplyMode.Apply : ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = TerrainToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated method
        this.SetDisableFX();
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = TerrainToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetRaycastResult(out this.m_RaycastPoint);
      // ISSUE: reference to a compiler-generated method
      this.SetDisableFX();
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == TerrainToolSystem.State.Default)
      {
        // ISSUE: reference to a compiler-generated method
        this.applyMode = this.prefab.m_Type == TerraformingType.Slope || !this.GetAllowApply() ? ApplyMode.Clear : ApplyMode.Apply;
        if (!singleFrameOnly)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = this.m_RaycastPoint;
          // ISSUE: reference to a compiler-generated field
          this.m_State = TerrainToolSystem.State.Adding;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((Object) this.m_AudioSource == (Object) null && !this.m_ToolSystem.actionMode.IsEditor())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioSource = this.m_AudioManager.PlayExclusiveUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TerraformSound);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ApplyPosition = this.m_RaycastPoint.m_HitPosition;
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == TerrainToolSystem.State.Adding)
      {
        // ISSUE: reference to a compiler-generated method
        this.applyMode = this.GetAllowApply() ? ApplyMode.Apply : ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_State = TerrainToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.GetRaycastResult(out this.m_RaycastPoint);
        // ISSUE: reference to a compiler-generated method
        this.SetDisableFX();
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_StartPoint = new ControlPoint();
      // ISSUE: reference to a compiler-generated field
      this.m_State = TerrainToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetRaycastResult(out this.m_RaycastPoint);
      // ISSUE: reference to a compiler-generated method
      this.SetDisableFX();
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      ControlPoint controlPoint;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint))
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State != TerrainToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated method
          this.applyMode = this.GetAllowApply() ? ApplyMode.Apply : ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_StartPoint = this.m_RaycastPoint;
          // ISSUE: reference to a compiler-generated field
          this.m_RaycastPoint = controlPoint;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_RaycastPoint.Equals(controlPoint))
        {
          // ISSUE: reference to a compiler-generated method
          if (this.HaveBrushSettingsChanged())
          {
            this.applyMode = ApplyMode.Clear;
            // ISSUE: reference to a compiler-generated method
            return this.UpdateDefinitions(inputDeps);
          }
          this.applyMode = ApplyMode.None;
          return inputDeps;
        }
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = controlPoint;
        // ISSUE: reference to a compiler-generated field
        this.m_RaycastPoint = controlPoint;
        // ISSUE: reference to a compiler-generated method
        return this.UpdateDefinitions(inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_RaycastPoint.Equals(new ControlPoint()))
      {
        this.applyMode = ApplyMode.None;
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != TerrainToolSystem.State.Default)
      {
        // ISSUE: reference to a compiler-generated method
        this.applyMode = this.GetAllowApply() ? ApplyMode.Apply : ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = this.m_RaycastPoint;
        // ISSUE: reference to a compiler-generated field
        this.m_RaycastPoint = new ControlPoint();
      }
      else
      {
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_StartPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_RaycastPoint = new ControlPoint();
      }
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private bool HaveBrushSettingsChanged()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_VisibleQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Brush> componentTypeHandle = this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          NativeArray<Brush> nativeArray = archetypeChunkArray[index1].GetNativeArray<Brush>(ref componentTypeHandle);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            if (!nativeArray[index2].m_Size.Equals(this.brushSize))
              return true;
          }
        }
        return false;
      }
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      if ((Object) this.prefab != (Object) null && (Object) this.brushType != (Object) null)
      {
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
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new TerrainToolSystem.CreateDefinitionsJob()
        {
          m_Prefab = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab),
          m_Brush = this.m_PrefabSystem.GetEntity((PrefabBase) this.brushType),
          m_Size = this.brushSize,
          m_Angle = math.radians(this.brushAngle),
          m_Strength = (this.m_State == TerrainToolSystem.State.Removing ? -this.brushStrength : this.brushStrength),
          m_Time = UnityEngine.Time.deltaTime,
          m_StartPoint = this.m_StartPoint,
          m_EndPoint = this.m_RaycastPoint,
          m_Target = (this.m_TargetSet ? this.m_TargetPosition : this.m_RaycastPoint.m_HitPosition),
          m_ApplyStart = this.m_ApplyPosition,
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        }.Schedule<TerrainToolSystem.CreateDefinitionsJob>(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
        if (this.applyMode == ApplyMode.Apply)
        {
          // ISSUE: reference to a compiler-generated method
          this.EnsureCachedBrushData();
        }
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
    public TerrainToolSystem()
    {
    }

    private enum State
    {
      Default,
      Adding,
      Removing,
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public Entity m_Brush;
      [ReadOnly]
      public float m_Size;
      [ReadOnly]
      public float m_Angle;
      [ReadOnly]
      public float m_Strength;
      [ReadOnly]
      public float m_Time;
      [ReadOnly]
      public float3 m_Target;
      [ReadOnly]
      public float3 m_ApplyStart;
      [ReadOnly]
      public ControlPoint m_StartPoint;
      [ReadOnly]
      public ControlPoint m_EndPoint;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EndPoint.Equals(new ControlPoint()))
          return;
        CreationDefinition component1 = new CreationDefinition();
        // ISSUE: reference to a compiler-generated field
        component1.m_Prefab = this.m_Brush;
        BrushDefinition component2 = new BrushDefinition();
        // ISSUE: reference to a compiler-generated field
        component2.m_Tool = this.m_Prefab;
        // ISSUE: reference to a compiler-generated field
        ref ControlPoint local = ref this.m_StartPoint;
        ControlPoint other = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        component2.m_Line = !local.Equals(other) ? new Line3.Segment(this.m_StartPoint.m_Position, this.m_EndPoint.m_Position) : new Line3.Segment(this.m_EndPoint.m_Position, this.m_EndPoint.m_Position);
        // ISSUE: reference to a compiler-generated field
        component2.m_Size = this.m_Size;
        // ISSUE: reference to a compiler-generated field
        component2.m_Angle = this.m_Angle;
        // ISSUE: reference to a compiler-generated field
        component2.m_Strength = this.m_Strength;
        // ISSUE: reference to a compiler-generated field
        component2.m_Time = this.m_Time;
        // ISSUE: reference to a compiler-generated field
        component2.m_Target = this.m_Target;
        // ISSUE: reference to a compiler-generated field
        component2.m_Start = this.m_ApplyStart;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity();
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BrushDefinition>(entity, component2);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity, new Updated());
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Brush> __Game_Tools_Brush_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Brush_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Brush>(true);
      }
    }
  }
}
