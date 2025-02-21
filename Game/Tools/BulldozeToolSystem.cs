// Decompiled with JetBrains decompiler
// Type: Game.Tools.BulldozeToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Achievements;
using Game.Areas;
using Game.Audio;
using Game.Buildings;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class BulldozeToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Bulldoze Tool";
    public System.Action EventConfirmationRequested;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private AudioManager m_AudioManager;
    private AchievementTriggerSystem m_AchievementTriggerSystem;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_RoadQuery;
    private EntityQuery m_PlantQuery;
    private EntityQuery m_SoundQuery;
    private ControlPoint m_LastRaycastPoint;
    private BulldozeToolSystem.State m_State;
    private NativeList<ControlPoint> m_ControlPoints;
    private IProxyAction m_Bulldoze;
    private IProxyAction m_BulldozeDiscard;
    private bool m_ApplyBlocked;
    private BulldozeToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Bulldoze Tool";

    public override int uiModeIndex => (int) this.actualMode;

    public override void GetUIModes(List<ToolMode> modes)
    {
      List<ToolMode> toolModeList1 = modes;
      // ISSUE: variable of a compiler-generated type
      BulldozeToolSystem.Mode mode1 = BulldozeToolSystem.Mode.MainElements;
      ToolMode toolMode1 = new ToolMode(mode1.ToString(), 0);
      toolModeList1.Add(toolMode1);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ToolSystem.actionMode.IsEditor())
        return;
      List<ToolMode> toolModeList2 = modes;
      // ISSUE: variable of a compiler-generated type
      BulldozeToolSystem.Mode mode2 = BulldozeToolSystem.Mode.SubElements;
      ToolMode toolMode2 = new ToolMode(mode2.ToString(), 1);
      toolModeList2.Add(toolMode2);
      List<ToolMode> toolModeList3 = modes;
      // ISSUE: variable of a compiler-generated type
      BulldozeToolSystem.Mode mode3 = BulldozeToolSystem.Mode.Everything;
      ToolMode toolMode3 = new ToolMode(mode3.ToString(), 2);
      toolModeList3.Add(toolMode3);
    }

    public BulldozeToolSystem.Tooltip tooltip
    {
      get
      {
        return !(this.m_LastRaycastPoint.m_OriginalEntity != Entity.Null) ? BulldozeToolSystem.Tooltip.None : BulldozeToolSystem.Tooltip.BulldozeObject;
      }
    }

    public override bool allowUnderground => true;

    public BulldozeToolSystem.Mode mode { get; set; }

    public BulldozeToolSystem.Mode actualMode
    {
      get
      {
        return !this.m_ToolSystem.actionMode.IsEditor() ? BulldozeToolSystem.Mode.MainElements : this.mode;
      }
    }

    public bool underground { get; set; }

    public bool allowManipulation { get; set; }

    public bool debugBypassBulldozeConfirmation { get; set; }

    public BulldozePrefab prefab { get; set; }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_Bulldoze;
        yield return this.m_BulldozeDiscard;
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
      this.m_AchievementTriggerSystem = this.World.GetOrCreateSystemManaged<AchievementTriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints = new NativeList<ControlPoint>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_RoadQuery = this.GetEntityQuery(ComponentType.ReadOnly<Edge>(), ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlantQuery = this.GetEntityQuery(ComponentType.ReadOnly<Plant>(), ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_Bulldoze = InputManager.instance.toolActionCollection.GetActionState("Bulldoze", nameof (BulldozeToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_BulldozeDiscard = InputManager.instance.toolActionCollection.GetActionState("Bulldoze Discard", nameof (BulldozeToolSystem));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Dispose();
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
      this.m_State = BulldozeToolSystem.State.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_ApplyBlocked = false;
      this.requireUnderground = false;
      this.requireStopIcons = false;
      this.requireAreas = AreaTypeMask.None;
      this.requireNet = Layer.None;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        // ISSUE: reference to a compiler-generated field
        this.applyActionOverride = this.m_Bulldoze;
        // ISSUE: reference to a compiler-generated field
        this.applyAction.enabled = this.actionsEnabled && this.m_State != BulldozeToolSystem.State.Waiting;
        // ISSUE: reference to a compiler-generated field
        this.cancelActionOverride = this.m_BulldozeDiscard;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.cancelAction.enabled = this.actionsEnabled && this.m_State == BulldozeToolSystem.State.Applying && this.IsMultiSelection();
      }
    }

    private bool IsMultiSelection()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ControlPoints.Length == 0)
        return false;
      EntityManager entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      if (!entityManager.HasComponent<Game.Net.Node>(this.m_ControlPoints[0].m_OriginalEntity))
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        if (!entityManager.HasComponent<Edge>(this.m_ControlPoints[0].m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_ControlPoints.Length > 1;
        }
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_ControlPoints.Length > 4;
    }

    public override PrefabBase GetPrefab() => (PrefabBase) this.prefab;

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      if (!(prefab is BulldozePrefab bulldozePrefab))
        return false;
      this.prefab = bulldozePrefab;
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
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem.typeMask = TypeMask.StaticObjects | TypeMask.Net;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem.netLayerMask = Layer.All;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.BuildingLots;
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
      // ISSUE: variable of a compiler-generated type
      BulldozeToolSystem.Mode actualMode = this.actualMode;
      switch (actualMode)
      {
        case BulldozeToolSystem.Mode.SubElements:
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubElements | RaycastFlags.NoMainElements;
          break;
        case BulldozeToolSystem.Mode.Everything:
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubElements;
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor())
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Markers | RaycastFlags.UpgradeIsMain | RaycastFlags.EditorContainers;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask |= TypeMask.Areas;
        if (this.underground)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.areaTypeMask = AreaTypeMask.Spaces;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.areaTypeMask = AreaTypeMask.Lots | AreaTypeMask.Spaces | AreaTypeMask.Surfaces;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubBuildings;
        if (!this.underground)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.Areas;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.areaTypeMask = AreaTypeMask.Lots | AreaTypeMask.Surfaces;
        }
      }
      if (this.allowManipulation)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask |= TypeMask.MovingObjects;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Placeholders | RaycastFlags.Decals;
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      this.requireUnderground = this.underground;
      this.requireStopIcons = true;
      if (this.underground)
      {
        // ISSUE: reference to a compiler-generated field
        this.requireAreas = this.m_ToolSystem.actionMode.IsEditor() ? AreaTypeMask.Spaces : AreaTypeMask.None;
        this.requireNet = Layer.None;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.requireAreas = this.m_ToolSystem.actionMode.IsEditor() ? AreaTypeMask.Lots | AreaTypeMask.Spaces | AreaTypeMask.Surfaces : AreaTypeMask.None;
        this.requireNet = Layer.Waterway;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == BulldozeToolSystem.State.Applying && !this.applyAction.enabled)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_State = BulldozeToolSystem.State.Default;
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Clear();
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      BulldozeToolSystem.State state = this.m_State;
      JobHandle jobHandle;
      switch (state)
      {
        case BulldozeToolSystem.State.Default:
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
          if (this.cancelAction.IsPressed())
          {
            this.applyMode = ApplyMode.None;
            jobHandle = inputDeps;
            break;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length > 0 && this.applyAction.WasPressedThisFrame())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = BulldozeToolSystem.State.Applying;
            // ISSUE: reference to a compiler-generated method
            jobHandle = this.Update(inputDeps, true);
            break;
          }
          // ISSUE: reference to a compiler-generated method
          jobHandle = this.Update(inputDeps, false);
          break;
        case BulldozeToolSystem.State.Applying:
          if (this.cancelAction.IsPressed())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_State = BulldozeToolSystem.State.Default;
            // ISSUE: reference to a compiler-generated field
            this.m_ApplyBlocked = true;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.RemoveRange(0, this.m_ControlPoints.Length - 1);
            }
            // ISSUE: reference to a compiler-generated method
            jobHandle = this.Update(inputDeps, true);
            break;
          }
          if (!this.applyAction.IsPressed())
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.m_BuildingQuery.IsEmptyIgnoreFilter && !this.m_ToolSystem.actionMode.IsEditor() && this.EventConfirmationRequested != null && !this.debugBypassBulldozeConfirmation && this.ConfirmationNeeded())
            {
              // ISSUE: reference to a compiler-generated field
              this.m_State = BulldozeToolSystem.State.Waiting;
              this.applyMode = ApplyMode.None;
              // ISSUE: reference to a compiler-generated field
              this.EventConfirmationRequested();
              jobHandle = inputDeps;
              break;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_State = BulldozeToolSystem.State.Default;
            // ISSUE: reference to a compiler-generated method
            jobHandle = this.Apply(inputDeps);
            break;
          }
          // ISSUE: reference to a compiler-generated method
          jobHandle = this.Update(inputDeps, false);
          break;
        case BulldozeToolSystem.State.Confirmed:
          // ISSUE: reference to a compiler-generated field
          this.m_State = BulldozeToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated method
          jobHandle = this.Apply(inputDeps);
          break;
        case BulldozeToolSystem.State.Cancelled:
          // ISSUE: reference to a compiler-generated field
          this.m_State = BulldozeToolSystem.State.Default;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControlPoints.Length >= 2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.RemoveRange(0, this.m_ControlPoints.Length - 1);
          }
          // ISSUE: reference to a compiler-generated method
          jobHandle = this.Update(inputDeps, true);
          break;
        default:
          this.applyMode = ApplyMode.None;
          jobHandle = inputDeps;
          break;
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      return jobHandle;
    }

    private bool ConfirmationNeeded()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_BuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      bool flag = false;
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Entity entity = entityArray[index];
        EntityManager entityManager = this.EntityManager;
        PrefabRef component;
        if ((entityManager.GetComponentData<Temp>(entity).m_Flags & TempFlags.Delete) != (TempFlags) 0 && this.EntityManager.TryGetComponent<PrefabRef>(entity, out component))
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<SpawnableBuildingData>(component.m_Prefab))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<SignatureBuildingData>(component.m_Prefab))
              continue;
          }
          flag = true;
        }
      }
      entityArray.Dispose();
      return flag;
    }

    public void ConfirmAction(bool confirm)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != BulldozeToolSystem.State.Waiting)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_State = confirm ? BulldozeToolSystem.State.Confirmed : BulldozeToolSystem.State.Cancelled;
    }

    private JobHandle Apply(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.GetAllowApply())
      {
        // ISSUE: reference to a compiler-generated field
        int entityCount = this.m_BuildingQuery.CalculateEntityCount();
        // ISSUE: reference to a compiler-generated field
        if (entityCount > 0 || this.m_RoadQuery.CalculateEntityCount() > 0)
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
          this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PropPlantBulldozeSound);
        }
        if (entityCount > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AchievementTriggerSystem.m_SquasherDownerBuffer.AddProgress(entityCount);
        }
        this.applyMode = ApplyMode.Apply;
        // ISSUE: reference to a compiler-generated field
        this.m_LastRaycastPoint = new ControlPoint();
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ControlPoints.Length >= 2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.RemoveRange(0, this.m_ControlPoints.Length - 1);
      }
      // ISSUE: reference to a compiler-generated method
      return this.Update(inputDeps, true);
    }

    protected override bool GetRaycastResult(out ControlPoint controlPoint)
    {
      Entity entity;
      RaycastHit hit;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out entity, out hit))
      {
        Owner component;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.actionMode.IsEditor() && this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(entity) && this.EntityManager.TryGetComponent<Owner>(entity, out component))
          controlPoint.m_OriginalEntity = component.m_Owner;
        if (this.EntityManager.HasComponent<Game.Net.Node>(entity) && this.EntityManager.HasComponent<Edge>(hit.m_HitEntity))
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
        Owner component;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.actionMode.IsEditor() && this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(entity) && this.EntityManager.TryGetComponent<Owner>(entity, out component))
          controlPoint.m_OriginalEntity = component.m_Owner;
        if (this.EntityManager.HasComponent<Game.Net.Node>(entity) && this.EntityManager.HasComponent<Edge>(hit.m_HitEntity))
          entity = hit.m_HitEntity;
        controlPoint = new ControlPoint(entity, hit);
        return true;
      }
      controlPoint = new ControlPoint();
      return false;
    }

    private JobHandle Update(JobHandle inputDeps, bool fullUpdate)
    {
      ControlPoint controlPoint1;
      bool forceUpdate;
      // ISSUE: reference to a compiler-generated method
      if (this.GetRaycastResult(out controlPoint1, out forceUpdate))
      {
        fullUpdate |= forceUpdate;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length == 0)
        {
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint1);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.SnapControlPoints(inputDeps);
          // ISSUE: reference to a compiler-generated method
          inputDeps = this.UpdateDefinitions(inputDeps);
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
            // ISSUE: reference to a compiler-generated field
            if (this.m_State == BulldozeToolSystem.State.Applying && controlPoint1.m_OriginalEntity != this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_OriginalEntity)
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
            // ISSUE: reference to a compiler-generated method
            inputDeps = this.SnapControlPoints(inputDeps);
            JobHandle.ScheduleBatchedJobs();
            inputDeps.Complete();
            ControlPoint other = new ControlPoint();
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              other = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
            }
            if (fullUpdate || !controlPoint2.EqualsIgnoreHit(other))
            {
              this.applyMode = ApplyMode.Clear;
              // ISSUE: reference to a compiler-generated method
              inputDeps = this.UpdateDefinitions(inputDeps);
            }
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == BulldozeToolSystem.State.Default)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Clear();
        }
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        inputDeps = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      }
      return inputDeps;
    }

    private JobHandle SnapControlPoints(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      return new BulldozeToolSystem.SnapJob()
      {
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Mode = this.actualMode,
        m_State = this.m_State,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_StaticData = this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ControlPoints = this.m_ControlPoints
      }.Schedule<BulldozeToolSystem.SnapJob>(inputDeps);
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new BulldozeToolSystem.CreateDefinitionsJob()
      {
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Mode = this.actualMode,
        m_State = this.m_State,
        m_ControlPoints = this.m_ControlPoints,
        m_CachedNodes = this.__TypeHandle.__Game_Tools_LocalNodeCache_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_PlaceholderData = this.__TypeHandle.__Game_Objects_Placeholder_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<BulldozeToolSystem.CreateDefinitionsJob>(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(producerJob);
      JobHandle job1 = producerJob;
      return JobHandle.CombineDependencies(job0, job1);
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
    public BulldozeToolSystem()
    {
    }

    public enum Mode
    {
      MainElements,
      SubElements,
      Everything,
    }

    public enum Tooltip
    {
      None,
      BulldozeObject,
    }

    private enum State
    {
      Default,
      Applying,
      Waiting,
      Confirmed,
      Cancelled,
    }

    private struct PathEdge
    {
      public Entity m_Edge;
      public bool m_Invert;
    }

    public struct PathItem : ILessThan<BulldozeToolSystem.PathItem>
    {
      public Entity m_Node;
      public Entity m_Edge;
      public float m_Cost;

      public bool LessThan(BulldozeToolSystem.PathItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (double) this.m_Cost < (double) other.m_Cost;
      }
    }

    [BurstCompile]
    private struct SnapJob : IJob
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public BulldozeToolSystem.Mode m_Mode;
      [ReadOnly]
      public BulldozeToolSystem.State m_State;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Static> m_StaticData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      public NativeList<ControlPoint> m_ControlPoints;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EditorMode && this.m_OutsideConnectionData.HasComponent(this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_OriginalEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.RemoveAt(this.m_ControlPoints.Length - 1);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Mode == BulldozeToolSystem.Mode.MainElements)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_StaticData.HasComponent(controlPoint.m_OriginalEntity) && !this.m_ServiceUpgradeData.HasComponent(controlPoint.m_OriginalEntity) && this.m_OwnerData.TryGetComponent(controlPoint.m_OriginalEntity, out componentData))
            {
              controlPoint.m_OriginalEntity = componentData.m_Owner;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              while (this.m_StaticData.HasComponent(controlPoint.m_OriginalEntity) && !this.m_ServiceUpgradeData.HasComponent(controlPoint.m_OriginalEntity) && this.m_OwnerData.TryGetComponent(controlPoint.m_OriginalEntity, out componentData))
                controlPoint.m_OriginalEntity = componentData.m_Owner;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints[this.m_ControlPoints.Length - 1] = controlPoint;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_State != BulldozeToolSystem.State.Applying)
            return;
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(controlPoint1.m_OriginalEntity) || this.m_NodeData.HasComponent(controlPoint1.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ControlPoints.Clear();
            NativeList<BulldozeToolSystem.PathEdge> path = new NativeList<BulldozeToolSystem.PathEdge>((AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated method
            this.CreatePath(controlPoint1, controlPoint2, path);
            // ISSUE: reference to a compiler-generated method
            this.AddControlPoints(controlPoint1, controlPoint2, path);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(controlPoint2.m_OriginalEntity) || this.m_NodeData.HasComponent(controlPoint2.m_OriginalEntity))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ControlPoints.RemoveAt(this.m_ControlPoints.Length - 1);
            }
            else
            {
              Entity owner1 = Entity.Null;
              Entity owner2 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnerData.HasComponent(controlPoint1.m_OriginalEntity))
              {
                // ISSUE: reference to a compiler-generated field
                owner1 = this.m_OwnerData[controlPoint1.m_OriginalEntity].m_Owner;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnerData.HasComponent(controlPoint2.m_OriginalEntity))
              {
                // ISSUE: reference to a compiler-generated field
                owner2 = this.m_OwnerData[controlPoint2.m_OriginalEntity].m_Owner;
              }
              if (owner1 != owner2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_ControlPoints.RemoveAt(this.m_ControlPoints.Length - 1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                for (int index = 0; index < this.m_ControlPoints.Length - 1; ++index)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ControlPoints[index].m_OriginalEntity == controlPoint2.m_OriginalEntity)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_ControlPoints.RemoveAt(this.m_ControlPoints.Length - 1);
                    break;
                  }
                }
              }
            }
          }
        }
      }

      private void CreatePath(
        ControlPoint startPoint,
        ControlPoint endPoint,
        NativeList<BulldozeToolSystem.PathEdge> path)
      {
        if ((double) math.distance(startPoint.m_Position, endPoint.m_Position) < 4.0)
          endPoint = startPoint;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[startPoint.m_OriginalEntity];
        // ISSUE: reference to a compiler-generated field
        NetData netData1 = this.m_PrefabNetData[prefabRef1.m_Prefab];
        if (startPoint.m_OriginalEntity == endPoint.m_OriginalEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EdgeData.HasComponent(endPoint.m_OriginalEntity))
            return;
          // ISSUE: object of a compiler-generated type is created
          path.Add(new BulldozeToolSystem.PathEdge()
          {
            m_Edge = endPoint.m_OriginalEntity,
            m_Invert = (double) endPoint.m_CurvePosition < (double) startPoint.m_CurvePosition
          });
        }
        else
        {
          NativeMinHeap<BulldozeToolSystem.PathItem> nativeMinHeap = new NativeMinHeap<BulldozeToolSystem.PathItem>(100, Allocator.Temp);
          NativeParallelHashMap<Entity, Entity> nativeParallelHashMap = new NativeParallelHashMap<Entity, Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(endPoint.m_OriginalEntity))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge = this.m_EdgeData[endPoint.m_OriginalEntity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetData netData2 = this.m_PrefabNetData[this.m_PrefabRefData[endPoint.m_OriginalEntity].m_Prefab];
            if ((netData1.m_RequiredLayers & netData2.m_RequiredLayers) > Layer.None)
            {
              // ISSUE: object of a compiler-generated type is created
              nativeMinHeap.Insert(new BulldozeToolSystem.PathItem()
              {
                m_Node = edge.m_Start,
                m_Edge = endPoint.m_OriginalEntity,
                m_Cost = 0.0f
              });
              // ISSUE: object of a compiler-generated type is created
              nativeMinHeap.Insert(new BulldozeToolSystem.PathItem()
              {
                m_Node = edge.m_End,
                m_Edge = endPoint.m_OriginalEntity,
                m_Cost = 0.0f
              });
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.HasComponent(endPoint.m_OriginalEntity))
            {
              // ISSUE: object of a compiler-generated type is created
              nativeMinHeap.Insert(new BulldozeToolSystem.PathItem()
              {
                m_Node = endPoint.m_OriginalEntity,
                m_Edge = Entity.Null,
                m_Cost = 0.0f
              });
            }
          }
          Entity key1 = Entity.Null;
          while (nativeMinHeap.Length != 0)
          {
            // ISSUE: variable of a compiler-generated type
            BulldozeToolSystem.PathItem pathItem = nativeMinHeap.Extract();
            // ISSUE: reference to a compiler-generated field
            if (pathItem.m_Edge == startPoint.m_OriginalEntity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nativeParallelHashMap[pathItem.m_Node] = pathItem.m_Edge;
              // ISSUE: reference to a compiler-generated field
              key1 = pathItem.m_Node;
              break;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (nativeParallelHashMap.TryAdd(pathItem.m_Node, pathItem.m_Edge))
            {
              // ISSUE: reference to a compiler-generated field
              if (pathItem.m_Node == startPoint.m_OriginalEntity)
              {
                // ISSUE: reference to a compiler-generated field
                key1 = pathItem.m_Node;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[pathItem.m_Node];
              PrefabRef prefabRef2 = new PrefabRef();
              // ISSUE: reference to a compiler-generated field
              if (pathItem.m_Edge != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                prefabRef2 = this.m_PrefabRefData[pathItem.m_Edge];
              }
              for (int index = 0; index < connectedEdge.Length; ++index)
              {
                Entity edge1 = connectedEdge[index].m_Edge;
                // ISSUE: reference to a compiler-generated field
                if (!(edge1 == pathItem.m_Edge))
                {
                  // ISSUE: reference to a compiler-generated field
                  Edge edge2 = this.m_EdgeData[edge1];
                  Entity key2;
                  // ISSUE: reference to a compiler-generated field
                  if (edge2.m_Start == pathItem.m_Node)
                  {
                    key2 = edge2.m_End;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (edge2.m_End == pathItem.m_Node)
                      key2 = edge2.m_Start;
                    else
                      continue;
                  }
                  if (!nativeParallelHashMap.ContainsKey(key2) || !(edge1 != startPoint.m_OriginalEntity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    NetData netData3 = this.m_PrefabNetData[this.m_PrefabRefData[edge1].m_Prefab];
                    if ((netData1.m_RequiredLayers & netData3.m_RequiredLayers) > Layer.None)
                    {
                      // ISSUE: reference to a compiler-generated field
                      Curve curve = this.m_CurveData[edge1];
                      // ISSUE: reference to a compiler-generated field
                      float num = pathItem.m_Cost + curve.m_Length + math.select(0.0f, 9.9f, prefabRef1.m_Prefab != prefabRef2.m_Prefab) + math.select(0.0f, 10f, connectedEdge.Length > 2);
                      // ISSUE: object of a compiler-generated type is created
                      nativeMinHeap.Insert(new BulldozeToolSystem.PathItem()
                      {
                        m_Node = key2,
                        m_Edge = edge1,
                        m_Cost = num
                      });
                    }
                  }
                }
              }
            }
          }
          Entity entity;
          Edge edge3;
          bool flag;
          for (; nativeParallelHashMap.TryGetValue(key1, out entity) && !(entity == Entity.Null); key1 = flag ? edge3.m_Start : edge3.m_End)
          {
            // ISSUE: reference to a compiler-generated field
            edge3 = this.m_EdgeData[entity];
            flag = edge3.m_End == key1;
            // ISSUE: object of a compiler-generated type is created
            path.Add(new BulldozeToolSystem.PathEdge()
            {
              m_Edge = entity,
              m_Invert = flag
            });
            if (entity == endPoint.m_OriginalEntity)
              break;
          }
        }
      }

      private void AddControlPoints(
        ControlPoint startPoint,
        ControlPoint endPoint,
        NativeList<BulldozeToolSystem.PathEdge> path)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Add(in startPoint);
        for (int index = 0; index < path.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          BulldozeToolSystem.PathEdge pathEdge = path[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[pathEdge.m_Edge];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[pathEdge.m_Edge];
          // ISSUE: reference to a compiler-generated field
          if (pathEdge.m_Invert)
          {
            CommonUtils.Swap<Entity>(ref edge.m_Start, ref edge.m_End);
            curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
          }
          ControlPoint controlPoint1 = endPoint with
          {
            m_OriginalEntity = edge.m_Start,
            m_Position = curve.m_Bezier.a
          };
          ControlPoint controlPoint2 = endPoint with
          {
            m_OriginalEntity = edge.m_End,
            m_Position = curve.m_Bezier.d
          };
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint1);
          // ISSUE: reference to a compiler-generated field
          this.m_ControlPoints.Add(in controlPoint2);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ControlPoints.Add(in endPoint);
      }
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public BulldozeToolSystem.Mode m_Mode;
      [ReadOnly]
      public BulldozeToolSystem.State m_State;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> m_CachedNodes;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_LotData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeParallelHashSet<Entity> bulldozeEntities = new NativeParallelHashSet<Entity>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_State == BulldozeToolSystem.State.Applying && this.m_ControlPoints.Length >= 2 && (this.m_EdgeData.HasComponent(this.m_ControlPoints[0].m_OriginalEntity) || this.m_NodeData.HasComponent(this.m_ControlPoints[0].m_OriginalEntity)))
        {
          // ISSUE: reference to a compiler-generated field
          int num = this.m_ControlPoints.Length / 2 - 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (num == 0 && this.m_ControlPoints[0].m_OriginalEntity == this.m_ControlPoints[1].m_OriginalEntity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Mode != BulldozeToolSystem.Mode.MainElements || !this.m_OwnerData.HasComponent(this.m_ControlPoints[0].m_OriginalEntity))
            {
              // ISSUE: reference to a compiler-generated field
              bulldozeEntities.Add(this.m_ControlPoints[0].m_OriginalEntity);
            }
          }
          else
          {
            for (int index1 = 0; index1 < num; ++index1)
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
                Edge edge2 = this.m_EdgeData[edge1];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Mode != BulldozeToolSystem.Mode.MainElements || !this.m_OwnerData.HasComponent(edge1))
                {
                  if (edge2.m_Start == controlPoint1.m_OriginalEntity && edge2.m_End == controlPoint2.m_OriginalEntity)
                    bulldozeEntities.Add(edge1);
                  else if (edge2.m_End == controlPoint1.m_OriginalEntity && edge2.m_Start == controlPoint2.m_OriginalEntity)
                    bulldozeEntities.Add(edge1);
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ControlPoints.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            bulldozeEntities.Add(this.m_ControlPoints[index].m_OriginalEntity);
          }
        }
        if (!bulldozeEntities.IsEmpty)
        {
          NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions = new NativeParallelHashMap<Entity, OwnerDefinition>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          NativeArray<Entity> nativeArray = bulldozeEntities.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.Execute(nativeArray[index], ownerDefinitions, bulldozeEntities);
          }
          nativeArray.Dispose();
          ownerDefinitions.Dispose();
        }
        bulldozeEntities.Dispose();
      }

      private void Execute(
        Entity bulldozeEntity,
        NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        NativeParallelHashSet<Entity> bulldozeEntities)
      {
        Entity entity1 = Entity.Null;
        Entity entity2 = Entity.Null;
        OwnerDefinition ownerDefinition = new OwnerDefinition();
        bool parent = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(bulldozeEntity))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            Entity entity3 = bulldozeEntity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.HasComponent(entity3) && !this.m_BuildingData.HasComponent(entity3))
            {
              // ISSUE: reference to a compiler-generated field
              entity3 = this.m_OwnerData[entity3].m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ServiceUpgradeData.HasComponent(entity3))
                entity2 = entity3;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(entity3) && this.m_SubObjects.HasBuffer(entity3))
              entity1 = entity3;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceUpgradeData.HasComponent(bulldozeEntity))
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_OwnerData[bulldozeEntity].m_Owner;
              parent = true;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(entity1))
        {
          if (!ownerDefinitions.TryGetValue(entity1, out ownerDefinition))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform1 = this.m_TransformData[entity1];
            Entity owner = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnerData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              owner = this.m_OwnerData[entity1].m_Owner;
            }
            // ISSUE: reference to a compiler-generated method
            this.AddEntity(ownerDefinitions, bulldozeEntities, entity1, owner, new OwnerDefinition(), entity2 == Entity.Null, parent, false);
            // ISSUE: reference to a compiler-generated field
            ownerDefinition.m_Prefab = this.m_PrefabRefData[entity1].m_Prefab;
            ownerDefinition.m_Position = transform1.m_Position;
            ownerDefinition.m_Rotation = transform1.m_Rotation;
            // ISSUE: reference to a compiler-generated field
            if (this.m_InstalledUpgrades.HasBuffer(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<InstalledUpgrade> installedUpgrade = this.m_InstalledUpgrades[entity1];
              for (int index = 0; index < installedUpgrade.Length; ++index)
              {
                Entity upgrade = installedUpgrade[index].m_Upgrade;
                if (upgrade != bulldozeEntity)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddEntity(ownerDefinitions, bulldozeEntities, upgrade, Entity.Null, ownerDefinition, entity2 == upgrade, parent, false);
                }
              }
            }
            ownerDefinitions.Add(entity1, ownerDefinition);
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(entity2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Transform transform2 = this.m_TransformData[entity2];
              // ISSUE: reference to a compiler-generated field
              ownerDefinition.m_Prefab = this.m_PrefabRefData[entity2].m_Prefab;
              ownerDefinition.m_Position = transform2.m_Position;
              ownerDefinition.m_Rotation = transform2.m_Rotation;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachmentData.HasComponent(bulldozeEntity))
          {
            // ISSUE: reference to a compiler-generated field
            Attachment attachment = this.m_AttachmentData[bulldozeEntity];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!bulldozeEntities.Contains(attachment.m_Attached) && this.m_TransformData.HasComponent(attachment.m_Attached) && this.m_PlaceholderData.HasComponent(bulldozeEntity) && !this.m_OwnerData.HasComponent(attachment.m_Attached))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddEntity(ownerDefinitions, bulldozeEntities, attachment.m_Attached, Entity.Null, new OwnerDefinition(), false, false, true);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_AttachedData.HasComponent(bulldozeEntity))
            {
              // ISSUE: reference to a compiler-generated field
              Attached attached = this.m_AttachedData[bulldozeEntity];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!bulldozeEntities.Contains(attached.m_Parent) && this.m_AttachmentData.HasComponent(attached.m_Parent) && this.m_AttachmentData[attached.m_Parent].m_Attached == bulldozeEntity)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddEntity(ownerDefinitions, bulldozeEntities, attached.m_Parent, Entity.Null, new OwnerDefinition(), false, false, true);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.HasBuffer(bulldozeEntity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[bulldozeEntity];
          bool flag = true;
          for (int index = 0; index < connectedEdge.Length; ++index)
          {
            Entity edge1 = connectedEdge[index].m_Edge;
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1];
            if (edge2.m_Start == bulldozeEntity || edge2.m_End == bulldozeEntity)
            {
              if (!bulldozeEntities.Contains(edge1))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddEdge(ownerDefinitions, bulldozeEntities, edge1, Entity.Null, ownerDefinition, false, true);
              }
              flag = false;
            }
          }
          if (!flag)
            return;
          // ISSUE: reference to a compiler-generated method
          this.AddEntity(ownerDefinitions, bulldozeEntities, bulldozeEntity, Entity.Null, ownerDefinition, false, false, true);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(bulldozeEntity))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddEdge(ownerDefinitions, bulldozeEntities, bulldozeEntity, Entity.Null, ownerDefinition, false, true);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_AreaNodes.HasBuffer(bulldozeEntity))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddEntity(ownerDefinitions, bulldozeEntities, bulldozeEntity, Entity.Null, ownerDefinition, false, false, true);
              DynamicBuffer<Game.Objects.SubObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_SubObjects.TryGetBuffer(bulldozeEntity, out bufferData))
                return;
              for (int index = 0; index < bufferData.Length; ++index)
              {
                Game.Objects.SubObject subObject = bufferData[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingData.HasComponent(subObject.m_SubObject))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddEntity(ownerDefinitions, bulldozeEntities, subObject.m_SubObject, Entity.Null, new OwnerDefinition(), false, false, true);
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.AddEntity(ownerDefinitions, bulldozeEntities, bulldozeEntity, Entity.Null, ownerDefinition, false, false, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!this.m_InstalledUpgrades.HasBuffer(bulldozeEntity) || !this.m_TransformData.HasComponent(bulldozeEntity))
                return;
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Transform transform = this.m_TransformData[bulldozeEntity];
              // ISSUE: reference to a compiler-generated field
              ownerDefinition.m_Prefab = this.m_PrefabRefData[bulldozeEntity].m_Prefab;
              ownerDefinition.m_Position = transform.m_Position;
              ownerDefinition.m_Rotation = transform.m_Rotation;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<InstalledUpgrade> installedUpgrade = this.m_InstalledUpgrades[bulldozeEntity];
              for (int index = 0; index < installedUpgrade.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddEntity(ownerDefinitions, bulldozeEntities, installedUpgrade[index].m_Upgrade, Entity.Null, ownerDefinition, false, false, true);
              }
            }
          }
        }
      }

      private void AddEdge(
        NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        NativeParallelHashSet<Entity> bulldozeEntities,
        Entity entity,
        Entity owner,
        OwnerDefinition ownerDefinition,
        bool upgrade,
        bool delete)
      {
        // ISSUE: reference to a compiler-generated method
        this.AddEntity(ownerDefinitions, bulldozeEntities, entity, owner, ownerDefinition, upgrade, false, delete);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_FixedData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        Edge edge = this.m_EdgeData[entity];
        // ISSUE: reference to a compiler-generated method
        this.AddFixedEdges(ownerDefinitions, bulldozeEntities, entity, edge.m_Start, owner, ownerDefinition, upgrade, delete);
        // ISSUE: reference to a compiler-generated method
        this.AddFixedEdges(ownerDefinitions, bulldozeEntities, entity, edge.m_End, owner, ownerDefinition, upgrade, delete);
      }

      private void AddFixedEdges(
        NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        NativeParallelHashSet<Entity> bulldozeEntities,
        Entity lastEdge,
        Entity lastNode,
        Entity owner,
        OwnerDefinition ownerDefinition,
        bool upgrade,
        bool delete)
      {
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        for (; this.m_FixedData.HasComponent(lastNode) && !bulldozeEntities.Contains(lastNode); lastNode = entity1)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[lastNode];
          Entity entity2 = Entity.Null;
          entity1 = Entity.Null;
          for (int index = 0; index < connectedEdge.Length; ++index)
          {
            Entity edge1 = connectedEdge[index].m_Edge;
            // ISSUE: reference to a compiler-generated field
            if (edge1 != lastEdge && this.m_FixedData.HasComponent(edge1))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge2 = this.m_EdgeData[edge1];
              if (edge2.m_Start == lastNode)
              {
                if (bulldozeEntities.Add(edge1))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddEntity(ownerDefinitions, bulldozeEntities, edge1, owner, ownerDefinition, upgrade, false, delete);
                  entity2 = edge1;
                  entity1 = edge2.m_End;
                  break;
                }
                break;
              }
              if (edge2.m_End == lastNode)
              {
                if (bulldozeEntities.Add(edge1))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddEntity(ownerDefinitions, bulldozeEntities, edge1, owner, ownerDefinition, upgrade, false, delete);
                  entity2 = edge1;
                  entity1 = edge2.m_Start;
                  break;
                }
                break;
              }
            }
          }
          lastEdge = entity2;
        }
      }

      private void AddEntity(
        NativeParallelHashMap<Entity, OwnerDefinition> ownerDefinitions,
        NativeParallelHashSet<Entity> bulldozeEntities,
        Entity entity,
        Entity owner,
        OwnerDefinition ownerDefinition,
        bool upgrade,
        bool parent,
        bool delete)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Original = entity;
        component1.m_Owner = owner;
        if (upgrade)
          component1.m_Flags |= CreationFlags.Upgrade;
        if (parent)
          component1.m_Flags |= CreationFlags.Upgrade | CreationFlags.Parent;
        if (delete)
          component1.m_Flags |= CreationFlags.Delete;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity1, new Updated());
        if (ownerDefinition.m_Prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity1, ownerDefinition);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorContainerData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            component1.m_SubPrefab = this.m_EditorContainerData[entity].m_Prefab;
          }
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[entity];
          // ISSUE: reference to a compiler-generated field
          NetCourse component2 = new NetCourse()
          {
            m_Curve = this.m_CurveData[entity].m_Bezier
          };
          component2.m_Length = MathUtils.Length(component2.m_Curve);
          component2.m_FixedIndex = -1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_FixedData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            component2.m_FixedIndex = this.m_FixedData[entity].m_Index;
          }
          component2.m_StartPosition.m_Entity = edge.m_Start;
          component2.m_StartPosition.m_Position = component2.m_Curve.a;
          component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve));
          component2.m_StartPosition.m_CourseDelta = 0.0f;
          component2.m_EndPosition.m_Entity = edge.m_End;
          component2.m_EndPosition.m_Position = component2.m_Curve.d;
          component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve));
          component2.m_EndPosition.m_CourseDelta = 1f;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(entity1, component2);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EditorContainerData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              component1.m_SubPrefab = this.m_EditorContainerData[entity].m_Prefab;
            }
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NodeData[entity];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<NetCourse>(entity1, new NetCourse()
            {
              m_Curve = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position),
              m_Length = 0.0f,
              m_FixedIndex = -1,
              m_StartPosition = {
                m_Entity = entity,
                m_Position = node.m_Position,
                m_Rotation = node.m_Rotation,
                m_CourseDelta = 0.0f
              },
              m_EndPosition = {
                m_Entity = entity,
                m_Position = node.m_Position,
                m_Rotation = node.m_Rotation,
                m_CourseDelta = 1f
              }
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.Transform transform = this.m_TransformData[entity];
              ObjectDefinition component3 = new ObjectDefinition();
              component3.m_Position = transform.m_Position;
              component3.m_Rotation = transform.m_Rotation;
              Game.Objects.Elevation componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ElevationData.TryGetComponent(entity, out componentData))
              {
                component3.m_Elevation = componentData.m_Elevation;
                component3.m_ParentMesh = ObjectUtils.GetSubParentMesh(componentData.m_Flags);
              }
              else
                component3.m_ParentMesh = -1;
              component3.m_Probability = 100;
              component3.m_PrefabSubIndex = -1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LocalTransformCacheData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                LocalTransformCache localTransformCache = this.m_LocalTransformCacheData[entity];
                component3.m_LocalPosition = localTransformCache.m_Position;
                component3.m_LocalRotation = localTransformCache.m_Rotation;
                component3.m_ParentMesh = localTransformCache.m_ParentMesh;
                component3.m_GroupIndex = localTransformCache.m_GroupIndex;
                component3.m_Probability = localTransformCache.m_Probability;
                component3.m_PrefabSubIndex = localTransformCache.m_PrefabSubIndex;
              }
              else if (ownerDefinition.m_Prefab != Entity.Null)
              {
                Game.Objects.Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(new Game.Objects.Transform(ownerDefinition.m_Position, ownerDefinition.m_Rotation)), transform);
                component3.m_LocalPosition = local.m_Position;
                component3.m_LocalRotation = local.m_Rotation;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.HasComponent(owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Objects.Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(this.m_TransformData[owner]), transform);
                  component3.m_LocalPosition = local.m_Position;
                  component3.m_LocalRotation = local.m_Rotation;
                }
                else
                {
                  component3.m_LocalPosition = transform.m_Position;
                  component3.m_LocalRotation = transform.m_Rotation;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_EditorContainerData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                EditorContainer editorContainer = this.m_EditorContainerData[entity];
                component1.m_SubPrefab = editorContainer.m_Prefab;
                component3.m_Scale = editorContainer.m_Scale;
                component3.m_Intensity = editorContainer.m_Intensity;
                component3.m_GroupIndex = editorContainer.m_GroupIndex;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<ObjectDefinition>(entity1, component3);
              // ISSUE: reference to a compiler-generated field
              ownerDefinition.m_Prefab = this.m_PrefabRefData[entity].m_Prefab;
              ownerDefinition.m_Position = transform.m_Position;
              ownerDefinition.m_Rotation = transform.m_Rotation;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_AreaNodes.HasBuffer(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[entity];
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity1).CopyFrom(areaNode.AsNativeArray());
                // ISSUE: reference to a compiler-generated field
                if (this.m_CachedNodes.HasBuffer(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<LocalNodeCache> cachedNode = this.m_CachedNodes[entity];
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity1).CopyFrom(cachedNode.AsNativeArray());
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity1, component1);
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachedData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Attached attached = this.m_AttachedData[entity];
          Entity owner1 = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(attached.m_Parent))
          {
            // ISSUE: reference to a compiler-generated field
            owner1 = this.m_OwnerData[attached.m_Parent].m_Owner;
          }
          if (!ownerDefinitions.ContainsKey(owner1))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(attached.m_Parent))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge = this.m_EdgeData[attached.m_Parent];
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity();
              CreationDefinition component4 = new CreationDefinition();
              component4.m_Original = attached.m_Parent;
              component4.m_Flags |= CreationFlags.Align;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<CreationDefinition>(entity2, component4);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(entity2, new Updated());
              // ISSUE: reference to a compiler-generated field
              NetCourse component5 = new NetCourse()
              {
                m_Curve = this.m_CurveData[attached.m_Parent].m_Bezier
              };
              component5.m_Length = MathUtils.Length(component5.m_Curve);
              component5.m_FixedIndex = -1;
              component5.m_StartPosition.m_Entity = edge.m_Start;
              component5.m_StartPosition.m_Position = component5.m_Curve.a;
              component5.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component5.m_Curve));
              component5.m_StartPosition.m_CourseDelta = 0.0f;
              component5.m_EndPosition.m_Entity = edge.m_End;
              component5.m_EndPosition.m_Position = component5.m_Curve.d;
              component5.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component5.m_Curve));
              component5.m_EndPosition.m_CourseDelta = 1f;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<NetCourse>(entity2, component5);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_NodeData.HasComponent(attached.m_Parent))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.Node node = this.m_NodeData[attached.m_Parent];
                // ISSUE: reference to a compiler-generated field
                Entity entity3 = this.m_CommandBuffer.CreateEntity();
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<CreationDefinition>(entity3, new CreationDefinition()
                {
                  m_Original = attached.m_Parent
                });
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(entity3, new Updated());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<NetCourse>(entity3, new NetCourse()
                {
                  m_Curve = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position),
                  m_Length = 0.0f,
                  m_FixedIndex = -1,
                  m_StartPosition = {
                    m_Entity = attached.m_Parent,
                    m_Position = node.m_Position,
                    m_Rotation = node.m_Rotation,
                    m_CourseDelta = 0.0f
                  },
                  m_EndPosition = {
                    m_Entity = attached.m_Parent,
                    m_Position = node.m_Position,
                    m_Rotation = node.m_Rotation,
                    m_CourseDelta = 1f
                  }
                });
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubNet> subNet1 = this.m_SubNets[entity];
          for (int index = 0; index < subNet1.Length; ++index)
          {
            Entity subNet2 = subNet1[index].m_SubNet;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.HasComponent(subNet2))
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.HasEdgeStartOrEnd(subNet2, entity) && !bulldozeEntities.Contains(subNet2))
              {
                // ISSUE: reference to a compiler-generated field
                Game.Net.Node node = this.m_NodeData[subNet2];
                // ISSUE: reference to a compiler-generated field
                Entity entity4 = this.m_CommandBuffer.CreateEntity();
                CreationDefinition component6 = new CreationDefinition();
                component6.m_Original = subNet2;
                if (delete)
                  component6.m_Flags |= CreationFlags.Delete;
                // ISSUE: reference to a compiler-generated field
                if (this.m_EditorContainerData.HasComponent(subNet2))
                {
                  // ISSUE: reference to a compiler-generated field
                  component6.m_SubPrefab = this.m_EditorContainerData[subNet2].m_Prefab;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<CreationDefinition>(entity4, component6);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(entity4, new Updated());
                if (ownerDefinition.m_Prefab != Entity.Null)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity4, ownerDefinition);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<NetCourse>(entity4, new NetCourse()
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
                Edge edge = this.m_EdgeData[subNet2];
                if (!bulldozeEntities.Contains(subNet2) && !bulldozeEntities.Contains(edge.m_Start) && !bulldozeEntities.Contains(edge.m_End))
                {
                  // ISSUE: reference to a compiler-generated field
                  Entity entity5 = this.m_CommandBuffer.CreateEntity();
                  CreationDefinition component7 = new CreationDefinition();
                  component7.m_Original = subNet2;
                  if (delete)
                    component7.m_Flags |= CreationFlags.Delete;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_EditorContainerData.HasComponent(subNet2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    component7.m_SubPrefab = this.m_EditorContainerData[subNet2].m_Prefab;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<CreationDefinition>(entity5, component7);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(entity5, new Updated());
                  if (ownerDefinition.m_Prefab != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity5, ownerDefinition);
                  }
                  // ISSUE: reference to a compiler-generated field
                  NetCourse component8 = new NetCourse()
                  {
                    m_Curve = this.m_CurveData[subNet2].m_Bezier
                  };
                  component8.m_Length = MathUtils.Length(component8.m_Curve);
                  component8.m_FixedIndex = -1;
                  component8.m_StartPosition.m_Entity = edge.m_Start;
                  component8.m_StartPosition.m_Position = component8.m_Curve.a;
                  component8.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component8.m_Curve));
                  component8.m_StartPosition.m_CourseDelta = 0.0f;
                  component8.m_EndPosition.m_Entity = edge.m_End;
                  component8.m_EndPosition.m_Position = component8.m_Curve.d;
                  component8.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component8.m_Curve));
                  component8.m_EndPosition.m_CourseDelta = 1f;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<NetCourse>(entity5, component8);
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubAreas.HasBuffer(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.SubArea> subArea = this.m_SubAreas[entity];
        for (int index1 = 0; index1 < subArea.Length; ++index1)
        {
          Entity area = subArea[index1].m_Area;
          // ISSUE: reference to a compiler-generated field
          Entity entity6 = this.m_CommandBuffer.CreateEntity();
          CreationDefinition component9 = new CreationDefinition();
          component9.m_Original = area;
          if (delete)
          {
            component9.m_Flags |= CreationFlags.Delete;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_LotData.HasComponent(area))
              component9.m_Flags |= CreationFlags.Hidden;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(entity6, component9);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(entity6, new Updated());
          if (ownerDefinition.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(entity6, ownerDefinition);
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[area];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity6).CopyFrom(areaNode.AsNativeArray());
          // ISSUE: reference to a compiler-generated field
          if (this.m_CachedNodes.HasBuffer(area))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<LocalNodeCache> cachedNode = this.m_CachedNodes[area];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<LocalNodeCache>(entity6).CopyFrom(cachedNode.AsNativeArray());
          }
          DynamicBuffer<Game.Objects.SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(area, out bufferData))
          {
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              Game.Objects.SubObject subObject = bufferData[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingData.HasComponent(subObject.m_SubObject))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddEntity(ownerDefinitions, bulldozeEntities, subObject.m_SubObject, Entity.Null, new OwnerDefinition(), false, false, true);
              }
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
          Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((edge2.m_Start == node || edge2.m_End == node) && this.m_OwnerData.HasComponent(edge1) && this.m_OwnerData[edge1].m_Owner == owner)
            return true;
        }
        return false;
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Static> __Game_Objects_Static_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LocalNodeCache> __Game_Tools_LocalNodeCache_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Placeholder> __Game_Objects_Placeholder_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentLookup = state.GetComponentLookup<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalNodeCache_RO_BufferLookup = state.GetBufferLookup<LocalNodeCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Placeholder_RO_ComponentLookup = state.GetComponentLookup<Placeholder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
      }
    }
  }
}
