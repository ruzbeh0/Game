// Decompiled with JetBrains decompiler
// Type: Game.Tools.DefaultToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Audio;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Input;
using Game.Net;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Vehicles;
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
  public class DefaultToolSystem : ToolBaseSystem
  {
    public const string kToolID = "Default Tool";
    private ToolOutputBarrier m_ToolOutputBarrier;
    private AudioManager m_AudioManager;
    private RenderingSystem m_RenderingSystem;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DragQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_InfomodeQuery;
    private EntityQuery m_SoundQuery;
    private EntityQuery m_UpdateQuery;
    private Entity m_LastRaycastEntity;
    private float3 m_MouseDownPosition;
    private DefaultToolSystem.State m_State;
    private IProxyAction m_DefaultToolApply;
    private int m_LastSelectedIndex;
    private DefaultToolSystem.TypeHandle __TypeHandle;

    public override string toolID => "Default Tool";

    public override bool allowUnderground => true;

    public bool underground { get; set; }

    public bool ignoreErrors { get; set; }

    public bool allowManipulation { get; set; }

    public bool debugSelect { get; set; }

    public bool debugLandValue { get; set; }

    private protected override IEnumerable<IProxyAction> toolActions
    {
      get
      {
        yield return this.m_DefaultToolApply;
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
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DefinitionQuery = this.GetDefinitionQuery();
      // ISSUE: reference to a compiler-generated field
      this.m_DragQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Owner>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<InfomodeActive>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<InfoviewRouteData>(),
          ComponentType.ReadOnly<InfoviewNetStatusData>(),
          ComponentType.ReadOnly<InfoviewHeatmapData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<ColorUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultToolApply = InputManager.instance.toolActionCollection.GetActionState("Default Tool", this.GetType().Name);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnStartRunning()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_LastRaycastEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated method
      this.SetState(DefaultToolSystem.State.Default);
      this.applyMode = ApplyMode.None;
      this.requireUnderground = false;
    }

    private protected override void UpdateActions()
    {
      using (ProxyAction.DeferStateUpdating())
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.applyActionOverride = this.m_LastRaycastEntity != Entity.Null ? this.m_DefaultToolApply : this.m_MouseApply;
        this.applyAction.enabled = this.actionsEnabled;
        // ISSUE: reference to a compiler-generated field
        this.cancelActionOverride = this.m_MouseCancel;
        this.cancelAction.enabled = this.actionsEnabled;
      }
    }

    public override PrefabBase GetPrefab() => (PrefabBase) null;

    public override bool TrySetPrefab(PrefabBase prefab) => false;

    public override void SetUnderground(bool underground) => this.underground = underground;

    public override void ElevationUp() => this.underground = false;

    public override void ElevationDown() => this.underground = true;

    public override void ElevationScroll() => this.underground = !this.underground;

    public override void InitializeRaycast()
    {
      // ISSUE: reference to a compiler-generated method
      base.InitializeRaycast();
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
      // ISSUE: reference to a compiler-generated field
      if (this.m_State != DefaultToolSystem.State.Default)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Net;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.netLayerMask = Layer.Road;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.iconLayerMask = IconLayerMask.None;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Labels | TypeMask.Icons;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.OutsideConnections | RaycastFlags.Decals | RaycastFlags.BuildingLots;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.netLayerMask = Layer.None;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.iconLayerMask = IconLayerMask.Default;
        if (this.debugSelect)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.Net;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubElements;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.netLayerMask |= Layer.All;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RenderingSystem.markersVisible)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.Markers;
          }
        }
        else if (this.debugLandValue)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ToolRaycastSystem.typeMask |= TypeMask.Terrain;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_InfomodeQuery.IsEmptyIgnoreFilter)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetInfomodeRaycastSettings();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor())
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubElements | RaycastFlags.Placeholders | RaycastFlags.Markers | RaycastFlags.UpgradeIsMain | RaycastFlags.EditorContainers;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.SubBuildings;
      }
    }

    private void SetInfomodeRaycastSettings()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_InfomodeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity = entityArray[index];
          InfoviewRouteData component1;
          if (this.EntityManager.TryGetComponent<InfoviewRouteData>(entity, out component1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.typeMask |= TypeMask.RouteWaypoints | TypeMask.RouteSegments;
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.routeType = component1.m_Type;
          }
          InfoviewNetStatusData component2;
          if (this.EntityManager.TryGetComponent<InfoviewNetStatusData>(entity, out component2))
          {
            switch (component2.m_Type)
            {
              case NetStatusType.LowVoltageFlow:
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.typeMask |= TypeMask.Lanes;
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.utilityTypeMask |= UtilityTypes.LowVoltageLine;
                break;
              case NetStatusType.HighVoltageFlow:
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.typeMask |= TypeMask.Lanes;
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.utilityTypeMask |= UtilityTypes.HighVoltageLine;
                break;
              case NetStatusType.PipeWaterFlow:
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.typeMask |= TypeMask.Lanes;
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.utilityTypeMask |= UtilityTypes.WaterPipe;
                break;
              case NetStatusType.PipeSewageFlow:
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.typeMask |= TypeMask.Lanes;
                // ISSUE: reference to a compiler-generated field
                this.m_ToolRaycastSystem.utilityTypeMask |= UtilityTypes.SewagePipe;
                break;
            }
          }
          InfoviewHeatmapData component3;
          if (this.EntityManager.TryGetComponent<InfoviewHeatmapData>(entity, out component3) && component3.m_Type == HeatmapData.LandValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolRaycastSystem.typeMask |= TypeMask.Terrain;
          }
        }
      }
    }

    private void PlaySelectedSound(Entity selected, bool forcePlay = false)
    {
      Game.Creatures.Resident component1;
      Citizen component2;
      PrefabRef component3;
      PrefabRef component4;
      SelectedSoundData component5;
      // ISSUE: reference to a compiler-generated field
      Entity clipEntity = !this.EntityManager.TryGetComponent<Game.Creatures.Resident>(selected, out component1) || !this.EntityManager.TryGetComponent<Citizen>(component1.m_Citizen, out component2) || !this.EntityManager.TryGetComponent<PrefabRef>(component1.m_Citizen, out component3) ? (!this.EntityManager.TryGetComponent<PrefabRef>(selected, out component4) || !this.EntityManager.TryGetComponent<SelectedSoundData>(component4.m_Prefab, out component5) ? this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_SelectEntitySound : component5.m_selectedSound) : CitizenUtils.GetCitizenSelectedSound(this.EntityManager, component1.m_Citizen, component2, component3.m_Prefab);
      if (forcePlay)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(clipEntity);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISoundIfNotPlaying(clipEntity);
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      this.requireUnderground = this.underground;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ForceUpdate |= !this.m_UpdateQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      if ((this.m_ToolRaycastSystem.raycastFlags & (RaycastFlags.DebugDisable | RaycastFlags.UIDisable)) == (RaycastFlags) 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        JobHandle jobHandle = this.m_State != DefaultToolSystem.State.Default || !this.applyAction.WasPressedThisFrame() ? (this.m_State == DefaultToolSystem.State.Default || !this.applyAction.WasReleasedThisFrame() ? (!this.cancelAction.WasPressedThisFrame() ? this.Update(inputDeps) : this.Cancel(inputDeps)) : this.Apply(inputDeps)) : this.Apply(inputDeps, this.applyAction.WasReleasedThisFrame(), this.cancelAction.WasPressedThisFrame());
        // ISSUE: reference to a compiler-generated method
        this.UpdateActions();
        return jobHandle;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_State == DefaultToolSystem.State.Default)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastRaycastEntity = Entity.Null;
      }
      else if (this.applyAction.WasReleasedThisFrame())
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastRaycastEntity = Entity.Null;
        // ISSUE: reference to a compiler-generated method
        this.SetState(DefaultToolSystem.State.Default);
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateActions();
      // ISSUE: reference to a compiler-generated method
      return this.Clear(inputDeps);
    }

    private JobHandle Clear(JobHandle inputDeps)
    {
      this.applyMode = ApplyMode.Clear;
      return inputDeps;
    }

    private JobHandle Cancel(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      DefaultToolSystem.State state = this.m_State;
      switch (state)
      {
        case DefaultToolSystem.State.Default:
          this.applyMode = ApplyMode.None;
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.selected = Entity.Null;
          return inputDeps;
        case DefaultToolSystem.State.Dragging:
          // ISSUE: reference to a compiler-generated method
          this.StopDragging();
          this.applyMode = ApplyMode.None;
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          this.SetState(DefaultToolSystem.State.Default);
          this.applyMode = ApplyMode.None;
          return inputDeps;
      }
    }

    private JobHandle Apply(JobHandle inputDeps, bool singleFrameOnly = false, bool toggleSelected = false)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      DefaultToolSystem.State state = this.m_State;
      switch (state)
      {
        case DefaultToolSystem.State.Default:
          if (!singleFrameOnly)
          {
            // ISSUE: reference to a compiler-generated method
            this.SetState(DefaultToolSystem.State.MouseDownPrepare);
          }
          this.applyMode = ApplyMode.None;
          // ISSUE: reference to a compiler-generated method
          return this.SelectTempEntity(inputDeps, toggleSelected);
        case DefaultToolSystem.State.Dragging:
          // ISSUE: reference to a compiler-generated method
          this.StopDragging();
          this.applyMode = ApplyMode.Apply;
          return inputDeps;
        default:
          // ISSUE: reference to a compiler-generated method
          this.SetState(DefaultToolSystem.State.Default);
          this.applyMode = ApplyMode.None;
          return inputDeps;
      }
    }

    private JobHandle Update(JobHandle inputDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      DefaultToolSystem.State state = this.m_State;
      switch (state)
      {
        case DefaultToolSystem.State.Default:
          Entity entity;
          RaycastHit hit1;
          bool forceUpdate;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetRaycastResult(out entity, out hit1, out forceUpdate) && entity == this.m_LastRaycastEntity && !forceUpdate)
          {
            this.applyMode = ApplyMode.None;
            return inputDeps;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_LastRaycastEntity = entity;
          this.applyMode = ApplyMode.Clear;
          // ISSUE: reference to a compiler-generated method
          return this.UpdateDefinitions(inputDeps, entity, hit1.m_CellIndex.x, new float3(), false);
        case DefaultToolSystem.State.MouseDownPrepare:
          RaycastHit hit2;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out Entity _, out hit2))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_MouseDownPosition = hit2.m_HitPosition;
            // ISSUE: reference to a compiler-generated method
            this.SetState(DefaultToolSystem.State.MouseDown);
          }
          this.applyMode = ApplyMode.None;
          return inputDeps;
        case DefaultToolSystem.State.MouseDown:
          RaycastHit hit3;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.GetRaycastResult(out Entity _, out hit3) && (double) math.distance(hit3.m_HitPosition, this.m_MouseDownPosition) > 1.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.StartDragging(hit3);
          }
          this.applyMode = ApplyMode.None;
          return inputDeps;
        case DefaultToolSystem.State.Dragging:
          RaycastHit hit4;
          // ISSUE: reference to a compiler-generated method
          if (this.GetRaycastResult(out Entity _, out hit4))
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DragQuery.IsEmptyIgnoreFilter)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_DragQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
              Entity native1 = archetypeChunkArray[0].GetNativeArray(this.GetEntityTypeHandle())[0];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ComponentTypeHandle<Game.Objects.Transform> componentTypeHandle = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle;
              Game.Objects.Transform native2 = archetypeChunkArray[0].GetNativeArray<Game.Objects.Transform>(ref componentTypeHandle)[0];
              archetypeChunkArray.Dispose();
              // ISSUE: reference to a compiler-generated field
              EntityCommandBuffer commandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer();
              native2.m_Position = hit4.m_HitPosition;
              commandBuffer.SetComponent<Game.Objects.Transform>(native1, native2);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.EntityManager.Exists(this.m_LastRaycastEntity))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                return this.UpdateDefinitions(inputDeps, this.m_LastRaycastEntity, hit4.m_CellIndex.x, hit4.m_HitPosition, true);
              }
            }
          }
          this.applyMode = ApplyMode.None;
          return inputDeps;
        default:
          this.applyMode = ApplyMode.None;
          return inputDeps;
      }
    }

    private void StartDragging(RaycastHit raycastHit)
    {
      Entity native = Entity.Null;
      Temp component1 = new Temp();
      Game.Objects.Transform component2 = new Game.Objects.Transform();
      bool flag = false;
      // ISSUE: reference to a compiler-generated field
      if (this.allowManipulation && !this.m_DragQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_DragQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          native = archetypeChunkArray[0].GetNativeArray(this.GetEntityTypeHandle())[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<Temp> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
          component1 = archetypeChunkArray[0].GetNativeArray<Temp>(ref componentTypeHandle1)[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<Game.Objects.Transform> componentTypeHandle2 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle;
          NativeArray<Game.Objects.Transform> nativeArray = archetypeChunkArray[0].GetNativeArray<Game.Objects.Transform>(ref componentTypeHandle2);
          if (nativeArray.Length != 0)
          {
            component2 = nativeArray[0];
            EntityManager entityManager = this.EntityManager;
            int num;
            if (!entityManager.HasComponent<Moving>(native))
            {
              entityManager = this.EntityManager;
              num = entityManager.HasComponent<Game.Objects.Marker>(native) ? 1 : 0;
            }
            else
              num = 1;
            flag = num != 0;
          }
          else
            flag = false;
        }
      }
      if (flag)
      {
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer();
        component1.m_Flags |= TempFlags.Dragging;
        component2.m_Position = raycastHit.m_HitPosition;
        commandBuffer.SetComponent<Temp>(native, component1);
        commandBuffer.SetComponent<Game.Objects.Transform>(native, component2);
        // ISSUE: reference to a compiler-generated method
        this.SetState(DefaultToolSystem.State.Dragging);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.SetState(DefaultToolSystem.State.Default);
      }
    }

    private void StopDragging()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DragQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_DragQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        Entity native1 = archetypeChunkArray[0].GetNativeArray(this.GetEntityTypeHandle())[0];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        Temp native2 = archetypeChunkArray[0].GetNativeArray<Temp>(ref componentTypeHandle)[0];
        archetypeChunkArray.Dispose();
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer();
        native2.m_Flags &= ~TempFlags.Dragging;
        commandBuffer.SetComponent<Temp>(native1, native2);
      }
      // ISSUE: reference to a compiler-generated method
      this.SetState(DefaultToolSystem.State.Default);
    }

    private void SetState(DefaultToolSystem.State state) => this.m_State = state;

    private JobHandle UpdateDefinitions(
      JobHandle inputDeps,
      Entity entity,
      int index,
      float3 position,
      bool setPosition)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      JobHandle job0 = this.DestroyDefinitions(this.m_DefinitionQuery, this.m_ToolOutputBarrier, inputDeps);
      if (entity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new DefaultToolSystem.CreateDefinitionsJob()
        {
          m_Entity = entity,
          m_Position = position,
          m_SetPosition = setPosition,
          m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
          m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
          m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
          m_RoutePositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
          m_RouteConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
          m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
          m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
          m_AggregateElements = this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferLookup,
          m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
        }.Schedule<DefaultToolSystem.CreateDefinitionsJob>(inputDeps);
        // ISSUE: reference to a compiler-generated field
        this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
        // ISSUE: reference to a compiler-generated field
        this.m_LastSelectedIndex = index;
      }
      return job0;
    }

    private JobHandle SelectTempEntity(JobHandle inputDeps, bool toggleSelected)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_TempQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.selected = Entity.Null;
        return inputDeps;
      }
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_TempQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      NativeReference<Entity> nativeReference = new NativeReference<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Debug_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle = new DefaultToolSystem.SelectEntityJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_DebugData = this.__TypeHandle.__Game_Tools_Debug_RO_ComponentLookup,
        m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_DebugSelect = this.debugSelect,
        m_Selected = nativeReference,
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer()
      }.Schedule<DefaultToolSystem.SelectEntityJob>(JobHandle.CombineDependencies(inputDeps, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
      jobHandle.Complete();
      if (!this.EntityManager.HasBuffer<AggregateElement>(nativeReference.Value))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastSelectedIndex = -1;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.selected != nativeReference.Value || this.m_ToolSystem.selectedIndex != this.m_LastSelectedIndex)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.selected = nativeReference.Value;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.selectedIndex = this.m_LastSelectedIndex;
        // ISSUE: reference to a compiler-generated method
        this.PlaySelectedSound(nativeReference.Value, true);
      }
      else if (toggleSelected)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.selected = Entity.Null;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.PlaySelectedSound(nativeReference.Value);
      }
      nativeReference.Dispose();
      return jobHandle;
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
    public DefaultToolSystem()
    {
    }

    private enum State
    {
      Default,
      MouseDownPrepare,
      MouseDown,
      Dragging,
    }

    [BurstCompile]
    private struct CreateDefinitionsJob : IJob
    {
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public float3 m_Position;
      [ReadOnly]
      public bool m_SetPosition;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<Position> m_RoutePositionData;
      [ReadOnly]
      public ComponentLookup<Connected> m_RouteConnectedData;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<AggregateElement> m_AggregateElements;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_Entity;
        OwnerDefinition ownerDefinition1 = new OwnerDefinition();
        Owner componentData1;
        Game.Objects.Transform componentData2;
        OwnerDefinition ownerDefinition2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceUpgradeData.HasComponent(this.m_Entity) && this.m_OwnerData.TryGetComponent(this.m_Entity, out componentData1) && this.m_TransformData.TryGetComponent(componentData1.m_Owner, out componentData2))
        {
          entity1 = componentData1.m_Owner;
          // ISSUE: reference to a compiler-generated method
          this.AddEntity(entity1, Entity.Null, new OwnerDefinition(), true);
          ownerDefinition2 = new OwnerDefinition();
          // ISSUE: reference to a compiler-generated field
          ownerDefinition2.m_Prefab = this.m_PrefabRefData[entity1].m_Prefab;
          ownerDefinition2.m_Position = componentData2.m_Position;
          ownerDefinition2.m_Rotation = componentData2.m_Rotation;
          ownerDefinition1 = ownerDefinition2;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddEntity(this.m_Entity, Entity.Null, ownerDefinition1, false);
        DynamicBuffer<InstalledUpgrade> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_InstalledUpgrades.TryGetBuffer(entity1, out bufferData))
          return;
        // ISSUE: reference to a compiler-generated field
        componentData2 = this.m_TransformData[entity1];
        ownerDefinition2 = new OwnerDefinition();
        // ISSUE: reference to a compiler-generated field
        ownerDefinition2.m_Prefab = this.m_PrefabRefData[entity1].m_Prefab;
        ownerDefinition2.m_Position = componentData2.m_Position;
        ownerDefinition2.m_Rotation = componentData2.m_Rotation;
        OwnerDefinition ownerDefinition3 = ownerDefinition2;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity entity2 = (Entity) bufferData[index];
          // ISSUE: reference to a compiler-generated field
          if (entity2 != this.m_Entity)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddEntity(entity2, Entity.Null, ownerDefinition3, entity1 != this.m_Entity);
          }
        }
      }

      private void AddEntity(
        Entity entity,
        Entity owner,
        OwnerDefinition ownerDefinition,
        bool isParent)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity();
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Original = entity;
        if (isParent)
          component1.m_Flags |= CreationFlags.Parent | CreationFlags.Duplicate;
        else
          component1.m_Flags |= CreationFlags.Select;
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
              // ISSUE: reference to a compiler-generated field
              if (this.m_SetPosition)
              {
                // ISSUE: reference to a compiler-generated field
                transform.m_Position = this.m_Position;
                component1.m_Flags |= CreationFlags.Dragging;
              }
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
              // ISSUE: reference to a compiler-generated field
              if (this.m_AttachedData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                component1.m_Attached = this.m_AttachedData[entity].m_Parent;
                component1.m_Flags |= CreationFlags.Attach;
              }
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
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_AreaNodes.HasBuffer(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[entity];
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Game.Areas.Node> dynamicBuffer = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(entity1);
                dynamicBuffer.ResizeUninitialized(areaNode.Length);
                dynamicBuffer.CopyFrom(areaNode.AsNativeArray());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_RouteWaypoints.HasBuffer(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<RouteWaypoint> routeWaypoint1 = this.m_RouteWaypoints[entity];
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<WaypointDefinition> dynamicBuffer = this.m_CommandBuffer.AddBuffer<WaypointDefinition>(entity1);
                  dynamicBuffer.ResizeUninitialized(routeWaypoint1.Length);
                  for (int index = 0; index < routeWaypoint1.Length; ++index)
                  {
                    RouteWaypoint routeWaypoint2 = routeWaypoint1[index];
                    WaypointDefinition waypointDefinition = new WaypointDefinition();
                    // ISSUE: reference to a compiler-generated field
                    waypointDefinition.m_Position = this.m_RoutePositionData[routeWaypoint2.m_Waypoint].m_Position;
                    waypointDefinition.m_Original = routeWaypoint2.m_Waypoint;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_RouteConnectedData.HasComponent(routeWaypoint2.m_Waypoint))
                    {
                      // ISSUE: reference to a compiler-generated field
                      waypointDefinition.m_Connection = this.m_RouteConnectedData[routeWaypoint2.m_Waypoint].m_Connected;
                    }
                    dynamicBuffer[index] = waypointDefinition;
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_IconData.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Icon icon = this.m_IconData[entity];
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<IconDefinition>(entity1, new IconDefinition(icon));
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_AggregateElements.HasBuffer(entity))
                    {
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<AggregateElement> aggregateElement = this.m_AggregateElements[entity];
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<AggregateElement> dynamicBuffer = this.m_CommandBuffer.AddBuffer<AggregateElement>(entity1);
                      dynamicBuffer.ResizeUninitialized(aggregateElement.Length);
                      dynamicBuffer.CopyFrom(aggregateElement.AsNativeArray());
                    }
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(entity1, component1);
      }
    }

    public struct SelectEntityJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Debug> m_DebugData;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public bool m_DebugSelect;
      public NativeReference<Entity> m_Selected;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        Entity entity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray1 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Controller> nativeArray3 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Owner componentData1;
            Controller controller;
            Temp componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!CollectionUtils.TryGet<Owner>(nativeArray1, index2, out componentData1) && (!CollectionUtils.TryGet<Controller>(nativeArray3, index2, out controller) || !this.m_OwnerData.TryGetComponent(controller.m_Controller, out componentData1)) || !this.m_TempData.TryGetComponent(componentData1.m_Owner, out componentData2) || (componentData2.m_Flags & TempFlags.Select) == (TempFlags) 0)
            {
              Temp temp = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_EntityLookup.Exists(temp.m_Original) && (temp.m_Flags & TempFlags.Select) != (TempFlags) 0)
                entity = temp.m_Original;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_IconData.HasComponent(entity) && !this.m_OwnerData.HasComponent(entity) && this.m_TargetData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Target target = this.m_TargetData[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_EntityLookup.Exists(target.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(target.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              Temp temp = this.m_TempData[target.m_Target];
              // ISSUE: reference to a compiler-generated field
              entity = !this.m_EntityLookup.Exists(temp.m_Original) ? Entity.Null : temp.m_Original;
            }
            else
              entity = target.m_Target;
          }
          else
            entity = Entity.Null;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_IconData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < 4 && !this.m_VehicleData.HasComponent(entity) && !this.m_BuildingData.HasComponent(entity) && this.m_OwnerData.HasComponent(entity); ++index)
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_OwnerData[entity].m_Owner;
          }
        }
        if (!(entity != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Selected.Value = entity;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_DebugSelect)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_DebugData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Debug>(entity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Debug>(entity, new Debug());
        }
      }
    }

    private new struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Icon> __Game_Notifications_Icon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AggregateElement> __Game_Net_AggregateElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Debug> __Game_Tools_Debug_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentLookup = state.GetComponentLookup<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AggregateElement_RO_BufferLookup = state.GetBufferLookup<AggregateElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Debug_RO_ComponentLookup = state.GetComponentLookup<Debug>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
      }
    }
  }
}
