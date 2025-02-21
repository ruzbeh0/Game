// Decompiled with JetBrains decompiler
// Type: Game.Tools.UpgradeToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Audio;
using Game.City;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class UpgradeToolSystem : ObjectToolBaseSystem
  {
    public const string kToolID = "Upgrade Tool";
    private CityConfigurationSystem m_CityConfigurationSystem;
    private AudioManager m_AudioManager;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_SoundQuery;
    private EntityQuery m_ContainerQuery;
    private Entity m_UpgradingObject;
    private NativeList<ControlPoint> m_ControlPoints;
    private RandomSeed m_RandomSeed;
    private bool m_AlreadyCreated;
    private ObjectPrefab m_Prefab;
    private IProxyAction m_PlaceUpgrade;
    private IProxyAction m_Rebuild;
    private UpgradeToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Upgrade Tool";

    public ObjectPrefab prefab
    {
      get => this.m_Prefab;
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) this.m_Prefab))
          return;
        this.m_Prefab = value;
        this.m_ForceUpdate = true;
      }
    }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_PlaceUpgrade;
        yield return this.m_Rebuild;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ContainerQuery = this.GetContainerQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PlaceUpgrade = InputManager.instance.toolActionCollection.GetActionState("Place Upgrade", nameof (UpgradeToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_Rebuild = InputManager.instance.toolActionCollection.GetActionState("Rebuild", nameof (UpgradeToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints = new NativeList<ControlPoint>(1, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_RandomSeed = RandomSeed.Next();
      // ISSUE: reference to a compiler-generated field
      this.m_AlreadyCreated = false;
      this.requireZones = true;
      this.requireAreas = AreaTypeMask.Lots;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.applyActionOverride = (UnityEngine.Object) this.prefab != (UnityEngine.Object) null ? this.m_PlaceUpgrade : this.m_Rebuild;
        this.applyAction.enabled = this.actionsEnabled;
        // ISSUE: reference to a compiler-generated field
        this.cancelActionOverride = this.m_MouseCancel;
        this.cancelAction.enabled = this.actionsEnabled;
      }
    }

    public override PrefabBase GetPrefab() => (PrefabBase) this.prefab;

    public override bool TrySetPrefab(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor() || !prefab.Has<ServiceUpgrade>())
        return false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Entity entity = this.m_PrefabSystem.GetEntity(prefab);
      this.CheckedStateRef.EntityManager.CompleteDependencyBeforeRO<PlaceableObjectData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.HasComponent(entity))
        return false;
      this.prefab = prefab as ObjectPrefab;
      return true;
    }

    public override void InitializeRaycast() => base.InitializeRaycast();

    [Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradingObject = this.m_ToolSystem.selected;
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
      {
        BuildingExtensionData component;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetComponentData<BuildingExtensionData>((PrefabBase) this.prefab, out component) && component.m_HasUndergroundElements)
          this.requireNet |= Layer.Road;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoview(Entity.Null);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetAvailableSnapMask(out this.m_SnapOnMask, out this.m_SnapOffMask);
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_UpgradingObject != Entity.Null) || this.m_ToolSystem.fullUpdateRequired || (this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) != (RaycastFlags) 0)
      {
        // ISSUE: reference to a compiler-generated method
        return this.Clear(inputDeps);
      }
      if (this.cancelAction.WasPressedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        return this.Cancel(inputDeps);
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.applyAction.WasPressedThisFrame() ? this.Apply(inputDeps) : this.Update(inputDeps);
    }

    private JobHandle Cancel(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_AlreadyCreated = false;
      return inputDeps;
    }

    private JobHandle Apply(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.GetAllowApply())
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        this.applyMode = ApplyMode.Apply;
        // ISSUE: reference to a compiler-generated field
        this.m_RandomSeed = RandomSeed.Next();
        // ISSUE: reference to a compiler-generated field
        this.m_AlreadyCreated = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PlaceUpgradeSound);
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.actionMode.IsGame() && (UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.PSI.Telemetry.PlaceBuilding(this.m_UpgradingObject, (PrefabBase) this.prefab, this.EntityManager.GetComponentData<Game.Objects.Transform>(this.m_UpgradingObject).m_Position);
        }
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_PlaceBuildingFailSound);
      // ISSUE: reference to a compiler-generated method
      return this.Update(inputDeps);
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.selected == Entity.Null)
      {
        this.applyMode = ApplyMode.Clear;
        // ISSUE: reference to a compiler-generated field
        this.m_AlreadyCreated = false;
        return inputDeps;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_AlreadyCreated && !this.m_ForceUpdate)
      {
        this.applyMode = ApplyMode.None;
        return inputDeps;
      }
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_AlreadyCreated = true;
      // ISSUE: reference to a compiler-generated method
      return this.CreateTempObject(inputDeps);
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      // ISSUE: reference to a compiler-generated field
      this.m_AlreadyCreated = false;
      return inputDeps;
    }

    private JobHandle CreateTempObject(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      Game.Objects.Transform componentData1 = this.EntityManager.GetComponentData<Game.Objects.Transform>(this.m_UpgradingObject);
      ControlPoint controlPoint = new ControlPoint();
      controlPoint.m_Position = componentData1.m_Position;
      controlPoint.m_Rotation = componentData1.m_Rotation;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null && this.m_PrefabSystem.HasComponent<BuildingExtensionData>((PrefabBase) this.prefab))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        BuildingExtensionData componentData2 = this.m_PrefabSystem.GetComponentData<BuildingExtensionData>((PrefabBase) this.prefab);
        controlPoint.m_Position = ObjectUtils.LocalToWorld(componentData1, componentData2.m_Position);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ControlPoints.Add(in controlPoint);
      // ISSUE: reference to a compiler-generated method
      return this.UpdateDefinitions(inputDeps);
    }

    private JobHandle UpdateDefinitions(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpgradingObject != Entity.Null)
      {
        Entity entity = Entity.Null;
        if ((UnityEngine.Object) this.prefab != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          entity = this.m_PrefabSystem.GetEntity((PrefabBase) this.prefab);
        }
        Entity laneContainer = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.actionMode.IsEditor())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.GetContainers(this.m_ContainerQuery, out laneContainer, out Entity _);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        job0 = JobHandle.CombineDependencies(job0, this.CreateDefinitions(entity, Entity.Null, Entity.Null, this.m_UpgradingObject, Entity.Null, laneContainer, this.m_CityConfigurationSystem.defaultTheme, this.m_ControlPoints, new NativeReference<ObjectToolBaseSystem.AttachmentData>(), this.m_ToolSystem.actionMode.IsEditor(), this.m_CityConfigurationSystem.leftHandTraffic, false, false, this.brushSize, math.radians(this.brushAngle), this.brushStrength, 0.0f, UnityEngine.Time.deltaTime, this.m_RandomSeed, this.GetActualSnap(), AgeMask.Sapling, inputDeps));
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

    [Preserve]
    public UpgradeToolSystem()
    {
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
      }
    }
  }
}
