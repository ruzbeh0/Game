// Decompiled with JetBrains decompiler
// Type: Game.Rendering.GuideLinesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class GuideLinesSystem : GameSystemBase
  {
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_MarkerNodeQuery;
    private EntityQuery m_WaterSourceQuery;
    private EntityQuery m_RenderingSettingsQuery;
    private TerrainSystem m_TerrainSystem;
    private ToolSystem m_ToolSystem;
    private NetToolSystem m_NetToolSystem;
    private RouteToolSystem m_RouteToolSystem;
    private ZoneToolSystem m_ZoneToolSystem;
    private SelectionToolSystem m_SelectionToolSystem;
    private AreaToolSystem m_AreaToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private WaterToolSystem m_WaterToolSystem;
    private OverlayRenderSystem m_OverlayRenderSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private PrefabSystem m_PrefabSystem;
    private NativeList<bool> m_AngleSides;
    private NativeList<GuideLinesSystem.TooltipInfo> m_Tooltips;
    private JobHandle m_TooltipDeps;
    private GuideLinesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetToolSystem = this.World.GetOrCreateSystemManaged<NetToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteToolSystem = this.World.GetOrCreateSystemManaged<RouteToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneToolSystem = this.World.GetOrCreateSystemManaged<ZoneToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectionToolSystem = this.World.GetOrCreateSystemManaged<SelectionToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaToolSystem = this.World.GetOrCreateSystemManaged<AreaToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterToolSystem = this.World.GetOrCreateSystemManaged<WaterToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem = this.World.GetOrCreateSystemManaged<OverlayRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<CreationDefinition>()
        },
        Any = new ComponentType[5]
        {
          ComponentType.ReadOnly<NetCourse>(),
          ComponentType.ReadOnly<WaypointDefinition>(),
          ComponentType.ReadOnly<Zoning>(),
          ComponentType.ReadOnly<Game.Areas.Node>(),
          ComponentType.ReadOnly<ObjectDefinition>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MarkerNodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Marker>(), ComponentType.ReadOnly<Orphan>(), ComponentType.ReadOnly<Game.Net.Node>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSourceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(), ComponentType.Exclude<PrefabRef>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<GuideLineSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AngleSides = new NativeList<bool>(4, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltips = new NativeList<GuideLinesSystem.TooltipInfo>(8, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AngleSides.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltips.Dispose();
      base.OnDestroy();
    }

    public NativeList<GuideLinesSystem.TooltipInfo> GetTooltips(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_TooltipDeps;
      // ISSUE: reference to a compiler-generated field
      return this.m_Tooltips;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltips.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_NetToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        JobHandle outJobHandle1;
        JobHandle dependencies1;
        JobHandle dependencies2;
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GuideLinesSystem.NetToolGuideLinesJob jobData = new GuideLinesSystem.NetToolGuideLinesJob()
        {
          m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
          m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
          m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_ConnectedEdgeType = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_PlaceableNetData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
          m_RoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
          m_ElectricityConnectionData = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
          m_WaterPipeConnectionData = this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_DefinitionChunks = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1),
          m_ControlPoints = this.m_NetToolSystem.GetControlPoints(out dependencies1),
          m_SnapLines = this.m_NetToolSystem.GetSnapLines(out dependencies2),
          m_Mode = this.m_NetToolSystem.actualMode,
          m_HighlightVoltage = Game.Prefabs.ElectricityConnection.Voltage.Invalid,
          m_HighlightWater = (bool2) false,
          m_Prefab = (UnityEngine.Object) this.m_NetToolSystem.prefab != (UnityEngine.Object) null ? this.m_PrefabSystem.GetEntity((PrefabBase) this.m_NetToolSystem.prefab) : Entity.Null,
          m_GuideLineSettingsData = this.m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>(),
          m_AngleSides = this.m_AngleSides,
          m_Tooltips = this.m_Tooltips,
          m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies3)
        };
        JobHandle jobHandle1 = JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3);
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetToolSystem.prefab is PowerLinePrefab)
        {
          Game.Prefabs.ElectricityConnection component;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetToolSystem.prefab.TryGet<Game.Prefabs.ElectricityConnection>(out component))
          {
            JobHandle outJobHandle2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            jobData.m_MarkerNodeChunks = this.m_MarkerNodeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
            // ISSUE: reference to a compiler-generated field
            jobData.m_HighlightVoltage = component.m_Voltage;
            jobHandle1 = JobHandle.CombineDependencies(jobHandle1, outJobHandle2);
          }
        }
        else
        {
          Game.Prefabs.WaterPipeConnection component;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetToolSystem.prefab is PipelinePrefab && this.m_NetToolSystem.prefab.TryGet<Game.Prefabs.WaterPipeConnection>(out component))
          {
            JobHandle outJobHandle3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            jobData.m_MarkerNodeChunks = this.m_MarkerNodeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle3);
            // ISSUE: reference to a compiler-generated field
            jobData.m_HighlightWater.x = component.m_FreshCapacity > 0;
            // ISSUE: reference to a compiler-generated field
            jobData.m_HighlightWater.y = component.m_SewageCapacity > 0;
            jobHandle1 = JobHandle.CombineDependencies(jobHandle1, outJobHandle3);
          }
        }
        JobHandle jobHandle2 = jobData.Schedule<GuideLinesSystem.NetToolGuideLinesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle1, jobHandle1));
        // ISSUE: reference to a compiler-generated field
        if (jobData.m_MarkerNodeChunks.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          jobData.m_MarkerNodeChunks.Dispose(jobHandle2);
        }
        // ISSUE: reference to a compiler-generated field
        jobData.m_DefinitionChunks.Dispose(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayRenderSystem.AddBufferWriter(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        this.m_TooltipDeps = jobHandle2;
        this.Dependency = jobHandle2;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_RouteToolSystem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Routes_Route_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          JobHandle outJobHandle;
          JobHandle dependencies4;
          JobHandle dependencies5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
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
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GuideLinesSystem.RouteToolGuideLinesJob jobData = new GuideLinesSystem.RouteToolGuideLinesJob()
          {
            m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
            m_WaypointDefinitionType = this.__TypeHandle.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle,
            m_RouteData = this.__TypeHandle.__Game_Routes_Route_RO_ComponentLookup,
            m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
            m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
            m_DefinitionChunks = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
            m_ControlPoints = this.m_RouteToolSystem.GetControlPoints(out dependencies4),
            m_MoveStartPosition = this.m_RouteToolSystem.moveStartPosition,
            m_State = this.m_RouteToolSystem.state,
            m_GuideLineSettingsData = this.m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>(),
            m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies5)
          };
          JobHandle job2 = JobHandle.CombineDependencies(dependencies4, dependencies5);
          JobHandle jobHandle = jobData.Schedule<GuideLinesSystem.RouteToolGuideLinesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, job2));
          // ISSUE: reference to a compiler-generated field
          jobData.m_DefinitionChunks.Dispose(jobHandle);
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
          this.Dependency = jobHandle;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ToolSystem.activeTool == this.m_ZoneToolSystem)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Zoning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            JobHandle outJobHandle;
            JobHandle dependencies;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            GuideLinesSystem.ZoneToolGuideLinesJob jobData = new GuideLinesSystem.ZoneToolGuideLinesJob()
            {
              m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
              m_ZoningType = this.__TypeHandle.__Game_Tools_Zoning_RO_ComponentTypeHandle,
              m_DefinitionChunks = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
              m_GuideLineSettingsData = this.m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>(),
              m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies)
            };
            JobHandle jobHandle = jobData.Schedule<GuideLinesSystem.ZoneToolGuideLinesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
            // ISSUE: reference to a compiler-generated field
            jobData.m_DefinitionChunks.Dispose(jobHandle);
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
            this.Dependency = jobHandle;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ToolSystem.activeTool == this.m_SelectionToolSystem)
            {
              Quad3 quad;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              bool selectionQuad = this.m_SelectionToolSystem.GetSelectionQuad(out quad);
              JobHandle dependencies;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              JobHandle handle = new GuideLinesSystem.SelectionToolGuideLinesJob()
              {
                m_State = this.m_SelectionToolSystem.state,
                m_SelectionType = this.m_SelectionToolSystem.selectionType,
                m_SelectionQuadIsValid = selectionQuad,
                m_SelectionQuad = quad,
                m_GuideLineSettingsData = this.m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>(),
                m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies)
              }.Schedule<GuideLinesSystem.SelectionToolGuideLinesJob>(JobHandle.CombineDependencies(this.Dependency, dependencies));
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayRenderSystem.AddBufferWriter(handle);
              this.Dependency = handle;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ToolSystem.activeTool == this.m_AreaToolSystem)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                JobHandle outJobHandle;
                NativeList<ControlPoint> moveStartPositions;
                JobHandle dependencies6;
                JobHandle dependencies7;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
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
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                GuideLinesSystem.AreaToolGuideLinesJob jobData = new GuideLinesSystem.AreaToolGuideLinesJob()
                {
                  m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
                  m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
                  m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
                  m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
                  m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
                  m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
                  m_PrefabLotData = this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup,
                  m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
                  m_DefinitionChunks = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
                  m_ControlPoints = this.m_AreaToolSystem.GetControlPoints(out moveStartPositions, out dependencies6),
                  m_MoveStartPositions = moveStartPositions,
                  m_State = this.m_AreaToolSystem.state,
                  m_Prefab = (UnityEngine.Object) this.m_AreaToolSystem.prefab != (UnityEngine.Object) null ? this.m_PrefabSystem.GetEntity((PrefabBase) this.m_AreaToolSystem.prefab) : Entity.Null,
                  m_GuideLineSettingsData = this.m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>(),
                  m_AngleSides = this.m_AngleSides,
                  m_Tooltips = this.m_Tooltips,
                  m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies7)
                };
                JobHandle job2 = JobHandle.CombineDependencies(dependencies6, dependencies7);
                JobHandle jobHandle = jobData.Schedule<GuideLinesSystem.AreaToolGuideLinesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, job2));
                // ISSUE: reference to a compiler-generated field
                jobData.m_DefinitionChunks.Dispose(jobHandle);
                // ISSUE: reference to a compiler-generated field
                this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
                // ISSUE: reference to a compiler-generated field
                this.m_TooltipDeps = jobHandle;
                this.Dependency = jobHandle;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Tools_ObjectDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  JobHandle outJobHandle;
                  JobHandle dependencies8;
                  JobHandle dependencies9;
                  JobHandle dependencies10;
                  JobHandle dependencies11;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
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
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  GuideLinesSystem.ObjectToolGuideLinesJob jobData = new GuideLinesSystem.ObjectToolGuideLinesJob()
                  {
                    m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
                    m_ObjectDefinitionType = this.__TypeHandle.__Game_Tools_ObjectDefinition_RO_ComponentTypeHandle,
                    m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
                    m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
                    m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
                    m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
                    m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
                    m_PrefabServiceUpgradeData = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup,
                    m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
                    m_PrefabLotData = this.__TypeHandle.__Game_Prefabs_LotData_RO_ComponentLookup,
                    m_PrefabPlaceableNetData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
                    m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
                    m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
                    m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
                    m_DefinitionChunks = this.m_DefinitionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
                    m_ControlPoints = this.m_ObjectToolSystem.GetControlPoints(out dependencies8),
                    m_SubSnapPoints = this.m_ObjectToolSystem.GetSubSnapPoints(out dependencies9),
                    m_NetUpgradeState = this.m_ObjectToolSystem.GetNetUpgradeStates(out dependencies10),
                    m_GuideLineSettingsData = this.m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>(),
                    m_Mode = this.m_ObjectToolSystem.actualMode,
                    m_State = this.m_ObjectToolSystem.state,
                    m_Prefab = (UnityEngine.Object) this.m_ObjectToolSystem.prefab != (UnityEngine.Object) null ? this.m_PrefabSystem.GetEntity((PrefabBase) this.m_ObjectToolSystem.prefab) : Entity.Null,
                    m_DistanceScale = this.m_ObjectToolSystem.distanceScale,
                    m_AngleSides = this.m_AngleSides,
                    m_Tooltips = this.m_Tooltips,
                    m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies11)
                  };
                  JobHandle jobHandle = jobData.Schedule<GuideLinesSystem.ObjectToolGuideLinesJob>(JobUtils.CombineDependencies(this.Dependency, outJobHandle, dependencies8, dependencies9, dependencies10, dependencies11));
                  // ISSUE: reference to a compiler-generated field
                  jobData.m_DefinitionChunks.Dispose(jobHandle);
                  // ISSUE: reference to a compiler-generated field
                  this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
                  // ISSUE: reference to a compiler-generated field
                  this.m_TooltipDeps = jobHandle;
                  this.Dependency = jobHandle;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ToolSystem.activeTool != this.m_WaterToolSystem)
                    return;
                  float3 float3 = new float3();
                  Viewer viewer;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  if (this.m_CameraUpdateSystem.TryGetViewer(out viewer))
                    float3 = viewer.right;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_WaterSourceColorElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Prefabs_GuideLineSettingsData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
                  JobHandle outJobHandle;
                  JobHandle dependencies;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
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
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  GuideLinesSystem.WaterToolGuideLinesJob jobData = new GuideLinesSystem.WaterToolGuideLinesJob()
                  {
                    m_WaterSourceDataType = this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle,
                    m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
                    m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
                    m_GuideLineSettingsData = this.__TypeHandle.__Game_Prefabs_GuideLineSettingsData_RO_ComponentLookup,
                    m_WaterSourceColors = this.__TypeHandle.__Game_Prefabs_WaterSourceColorElement_RO_BufferLookup,
                    m_Attribute = this.m_WaterToolSystem.attribute,
                    m_PositionOffset = this.m_TerrainSystem.positionOffset,
                    m_CameraRight = float3,
                    m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
                    m_WaterSourceChunks = this.m_WaterSourceQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
                    m_GuideLineSettingsEntity = this.m_RenderingSettingsQuery.GetSingletonEntity(),
                    m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies)
                  };
                  JobHandle jobHandle = jobData.Schedule<GuideLinesSystem.WaterToolGuideLinesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
                  // ISSUE: reference to a compiler-generated field
                  jobData.m_WaterSourceChunks.Dispose(jobHandle);
                  // ISSUE: reference to a compiler-generated field
                  this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
                  this.Dependency = jobHandle;
                }
              }
            }
          }
        }
      }
    }

    private static void CheckDirection(
      float2 startDir,
      float2 checkDir,
      ref float2 leftDir,
      ref float2 rightDir,
      ref int bestLeft,
      ref int bestRight,
      ref float2 leftDir2,
      ref float2 rightDir2,
      ref int bestLeft2,
      ref int bestRight2)
    {
      if (!MathUtils.TryNormalize(ref checkDir))
        return;
      int num1 = Mathf.RoundToInt(math.degrees(math.acos(math.clamp(math.dot(startDir, checkDir), -1f, 1f))));
      if (num1 == 0)
        return;
      int num2 = (double) math.dot(MathUtils.Right(startDir), checkDir) > 0.0 ? 1 : 0;
      if (num2 != 0 || num1 == 180)
      {
        if (num1 < bestRight)
        {
          rightDir = checkDir;
          bestRight = num1;
        }
        if ((num1 == 90 || num1 == 180) && num1 < bestRight2)
        {
          rightDir2 = checkDir;
          bestRight2 = num1;
        }
      }
      if (num2 != 0 && num1 != 180)
        return;
      if (num1 < bestLeft)
      {
        leftDir = checkDir;
        bestLeft = num1;
      }
      if (num1 != 90 && num1 != 180 || num1 >= bestLeft2)
        return;
      leftDir2 = checkDir;
      bestLeft2 = num1;
    }

    private static void DrawAngleIndicator(
      OverlayRenderSystem.Buffer buffer,
      NativeList<GuideLinesSystem.TooltipInfo> tooltips,
      GuideLineSettingsData guideLineSettings,
      Line3.Segment line1,
      Line3.Segment line2,
      float2 dir1,
      float2 dir2,
      float size,
      float lineWidth,
      int angle,
      bool angleSide)
    {
      if (angle == 180)
      {
        float2 float2_1 = angleSide ? MathUtils.Right(dir1) : MathUtils.Left(dir1);
        float2 float2_2 = angleSide ? MathUtils.Right(dir2) : MathUtils.Left(dir2);
        float3 b1 = line1.b;
        b1.xz -= dir1 * size;
        float3 b2 = line1.b;
        float3 b3 = line1.b;
        b2.xz += float2_1 * (size - lineWidth * 0.5f) - dir1 * size;
        b3.xz += float2_1 * size - dir1 * (size + lineWidth * 0.5f);
        float3 a1 = line2.a;
        float3 a2 = line2.a;
        a1.xz -= float2_2 * size + dir2 * (size + lineWidth * 0.5f);
        a2.xz -= float2_2 * (size - lineWidth * 0.5f) + dir2 * size;
        float3 a3 = line2.a;
        a3.xz -= dir2 * size;
        buffer.DrawLine(guideLineSettings.m_HighPriorityColor, new Line3.Segment(b1, b2), lineWidth);
        buffer.DrawLine(guideLineSettings.m_HighPriorityColor, new Line3.Segment(b3, a1), lineWidth);
        buffer.DrawLine(guideLineSettings.m_HighPriorityColor, new Line3.Segment(a2, a3), lineWidth);
        float3 b4 = line1.b;
        b4.xz += float2_1 * (size * 1.5f);
        ref NativeList<GuideLinesSystem.TooltipInfo> local1 = ref tooltips;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GuideLinesSystem.TooltipInfo tooltipInfo = new GuideLinesSystem.TooltipInfo(GuideLinesSystem.TooltipType.Angle, b4, (float) angle);
        ref GuideLinesSystem.TooltipInfo local2 = ref tooltipInfo;
        local1.Add(in local2);
      }
      else if (angle > 90)
      {
        float2 forward = math.normalize(dir1 + dir2);
        float3 b5 = line1.b;
        b5.xz -= dir1 * size;
        float3 startTangent = new float3();
        startTangent.xz = angleSide ? MathUtils.Right(dir1) : MathUtils.Left(dir1);
        float3 b6 = line1.b;
        b6.xz -= forward * size;
        float3 float3 = new float3();
        float3.xz = angleSide ? MathUtils.Right(forward) : MathUtils.Left(forward);
        float3 a = line2.a;
        a.xz -= dir2 * size;
        float3 endTangent = new float3();
        endTangent.xz = angleSide ? MathUtils.Right(dir2) : MathUtils.Left(dir2);
        buffer.DrawCurve(guideLineSettings.m_HighPriorityColor, NetUtils.FitCurve(b5, startTangent, float3, b6), lineWidth);
        buffer.DrawCurve(guideLineSettings.m_HighPriorityColor, NetUtils.FitCurve(b6, float3, endTangent, a), lineWidth);
        float3 b7 = line1.b;
        b7.xz -= forward * (size * 1.5f);
        ref NativeList<GuideLinesSystem.TooltipInfo> local3 = ref tooltips;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GuideLinesSystem.TooltipInfo tooltipInfo = new GuideLinesSystem.TooltipInfo(GuideLinesSystem.TooltipType.Angle, b7, (float) angle);
        ref GuideLinesSystem.TooltipInfo local4 = ref tooltipInfo;
        local3.Add(in local4);
      }
      else if (angle == 90)
      {
        float3 b8 = line1.b;
        b8.xz -= dir1 * size;
        float3 b9 = line1.b;
        float3 b10 = line1.b;
        b9.xz -= dir2 * (size - lineWidth * 0.5f) + dir1 * size;
        b10.xz -= dir2 * size + dir1 * (size + lineWidth * 0.5f);
        float3 a = line2.a;
        a.xz -= dir2 * size;
        buffer.DrawLine(guideLineSettings.m_HighPriorityColor, new Line3.Segment(b8, b9), lineWidth);
        buffer.DrawLine(guideLineSettings.m_HighPriorityColor, new Line3.Segment(b10, a), lineWidth);
        float3 b11 = line1.b;
        b11.xz -= math.normalizesafe(dir1 + dir2) * (size * 1.5f);
        ref NativeList<GuideLinesSystem.TooltipInfo> local5 = ref tooltips;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GuideLinesSystem.TooltipInfo tooltipInfo = new GuideLinesSystem.TooltipInfo(GuideLinesSystem.TooltipType.Angle, b11, (float) angle);
        ref GuideLinesSystem.TooltipInfo local6 = ref tooltipInfo;
        local5.Add(in local6);
      }
      else
      {
        if (angle <= 0)
          return;
        float3 b12 = line1.b;
        b12.xz -= dir1 * size;
        float3 startTangent = new float3();
        startTangent.xz = angleSide ? MathUtils.Right(dir1) : MathUtils.Left(dir1);
        float3 a = line2.a;
        a.xz -= dir2 * size;
        buffer.DrawCurve(guideLineSettings.m_HighPriorityColor, NetUtils.FitCurve(b12, startTangent, new float3()
        {
          xz = angleSide ? MathUtils.Right(dir2) : MathUtils.Left(dir2)
        }, a), lineWidth);
        float3 b13 = line1.b;
        b13.xz -= math.normalizesafe(dir1 + dir2) * (size * 1.5f);
        ref NativeList<GuideLinesSystem.TooltipInfo> local7 = ref tooltips;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GuideLinesSystem.TooltipInfo tooltipInfo = new GuideLinesSystem.TooltipInfo(GuideLinesSystem.TooltipType.Angle, b13, (float) angle);
        ref GuideLinesSystem.TooltipInfo local8 = ref tooltipInfo;
        local7.Add(in local8);
      }
    }

    private static void DrawAreaRange(
      OverlayRenderSystem.Buffer buffer,
      quaternion rotation,
      float3 position,
      LotData lotData)
    {
      float3 float3 = math.forward(rotation);
      UnityEngine.Color rangeColor = (UnityEngine.Color) lotData.m_RangeColor;
      UnityEngine.Color fillColor = rangeColor with
      {
        a = 0.0f
      };
      buffer.DrawCircle(rangeColor, fillColor, lotData.m_MaxRadius * 0.02f, OverlayRenderSystem.StyleFlags.Projected, float3.xz, position, lotData.m_MaxRadius * 2f);
    }

    private static void DrawUpgradeRange(
      OverlayRenderSystem.Buffer buffer,
      quaternion rotation,
      float3 position,
      GuideLineSettingsData guideLineSettings,
      Game.Prefabs.BuildingData ownerBuildingData,
      Game.Prefabs.BuildingData buildingData,
      ServiceUpgradeData serviceUpgradeData)
    {
      UnityEngine.Color lowPriorityColor = guideLineSettings.m_LowPriorityColor;
      UnityEngine.Color fillColor = lowPriorityColor with
      {
        a = 0.0f
      };
      float3 forward;
      float width;
      float length;
      float roundness1;
      bool circular;
      BuildingUtils.CalculateUpgradeRangeValues(rotation, ownerBuildingData, buildingData, serviceUpgradeData, out forward, out width, out length, out roundness1, out circular);
      float num1 = roundness1 * 2f;
      float num2 = length - num1;
      float roundness2 = num1 / width;
      if (circular)
      {
        buffer.DrawCircle(lowPriorityColor, fillColor, width * 0.01f, OverlayRenderSystem.StyleFlags.Projected, forward.xz, position, width);
      }
      else
      {
        Line3.Segment line = new Line3.Segment(position - forward * (num2 * 0.5f), position + forward * (num2 * 0.5f));
        buffer.DrawLine(lowPriorityColor, fillColor, width * 0.01f, OverlayRenderSystem.StyleFlags.Projected, line, width, (float2) roundness2);
      }
    }

    private static void DrawNetCourse(
      OverlayRenderSystem.Buffer buffer,
      NetCourse netCourse,
      NetGeometryData netGeometryData,
      GuideLineSettingsData guideLineSettings)
    {
      float2 float2_1 = math.select((float2) 0.0f, (float2) 1f, new bool2((netCourse.m_StartPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) > (CoursePosFlags) 0, (netCourse.m_EndPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) > (CoursePosFlags) 0));
      if ((double) netCourse.m_Length < 0.0099999997764825821)
      {
        buffer.DrawCircle(guideLineSettings.m_MediumPriorityColor, guideLineSettings.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, new float2(0.0f, 1f), netCourse.m_StartPosition.m_Position, netGeometryData.m_DefaultWidth);
      }
      else
      {
        float2 float2_2 = new float2(netCourse.m_StartPosition.m_CourseDelta, netCourse.m_EndPosition.m_CourseDelta);
        Bezier4x3 curve1 = MathUtils.Cut(netCourse.m_Curve, new float2(float2_2.x, math.lerp(float2_2.x, float2_2.y, 0.5f)));
        Bezier4x3 curve2 = MathUtils.Cut(netCourse.m_Curve, new float2(math.lerp(float2_2.x, float2_2.y, 0.5f), float2_2.y));
        buffer.DrawCurve(guideLineSettings.m_MediumPriorityColor, guideLineSettings.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve1, netGeometryData.m_DefaultWidth, new float2(float2_1.x, 0.0f));
        buffer.DrawCurve(guideLineSettings.m_MediumPriorityColor, guideLineSettings.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve2, netGeometryData.m_DefaultWidth, new float2(0.0f, float2_1.y));
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public GuideLinesSystem()
    {
    }

    public struct TooltipInfo
    {
      public GuideLinesSystem.TooltipType m_Type;
      public float3 m_Position;
      public float m_Value;

      public TooltipInfo(GuideLinesSystem.TooltipType type, float3 position, float value)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Type = type;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = position;
        // ISSUE: reference to a compiler-generated field
        this.m_Value = value;
      }
    }

    public enum TooltipType
    {
      Angle,
      Length,
    }

    [BurstCompile]
    private struct WaterToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Simulation.WaterSourceData> m_WaterSourceDataType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<GuideLineSettingsData> m_GuideLineSettingsData;
      [ReadOnly]
      public BufferLookup<WaterSourceColorElement> m_WaterSourceColors;
      [ReadOnly]
      public WaterToolSystem.Attribute m_Attribute;
      [ReadOnly]
      public float3 m_PositionOffset;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_WaterSourceChunks;
      [ReadOnly]
      public Entity m_GuideLineSettingsEntity;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        GuideLineSettingsData lineSettingsData = this.m_GuideLineSettingsData[this.m_GuideLineSettingsEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<WaterSourceColorElement> waterSourceColor = this.m_WaterSourceColors[this.m_GuideLineSettingsEntity];
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_WaterSourceChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk waterSourceChunk = this.m_WaterSourceChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Simulation.WaterSourceData> nativeArray1 = waterSourceChunk.GetNativeArray<Game.Simulation.WaterSourceData>(ref this.m_WaterSourceDataType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Transform> nativeArray2 = waterSourceChunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          bool flag = waterSourceChunk.Has<Temp>(ref this.m_TempType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Game.Simulation.WaterSourceData waterSourceData = nativeArray1[index2];
            Game.Objects.Transform transform = nativeArray2[index2];
            WaterSourceColorElement sourceColorElement = waterSourceColor[math.clamp(waterSourceData.m_ConstantDepth, 0, waterSourceColor.Length - 1)];
            float3 float3 = math.forward(transform.m_Rotation);
            float num1 = math.max(1f, waterSourceData.m_Radius);
            float num2 = num1 * 0.02f;
            float3 position = transform.m_Position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            position.y = waterSourceData.m_ConstantDepth <= 0 ? TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, position) + waterSourceData.m_Amount : this.m_PositionOffset.y + waterSourceData.m_Amount;
            if (flag)
            {
              sourceColorElement.m_Fill.a = (float) ((double) sourceColorElement.m_Fill.a * 0.5 + 0.5);
              sourceColorElement.m_Outline.a = (float) ((double) sourceColorElement.m_Outline.a * 0.5 + 0.5);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCircle(sourceColorElement.m_Outline, sourceColorElement.m_Fill, num2, (OverlayRenderSystem.StyleFlags) 0, float3.xz, position, num1 * 2f);
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCircle(sourceColorElement.m_ProjectedOutline, sourceColorElement.m_ProjectedFill, num2, OverlayRenderSystem.StyleFlags.Projected, float3.xz, position, num1 * 2f);
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              WaterToolSystem.Attribute attribute = this.m_Attribute;
              switch (attribute)
              {
                case WaterToolSystem.Attribute.Location:
                  // ISSUE: reference to a compiler-generated field
                  float2 xz1 = this.m_CameraRight.xz;
                  if (MathUtils.TryNormalize(ref xz1))
                  {
                    Line3.Segment line = new Line3.Segment(position, position);
                    line.a.xz -= xz1 * (num1 * 0.5f);
                    line.b.xz += xz1 * (num1 * 0.5f);
                    float dashLength = num1 * 0.5f - num2;
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawDashedLine(lineSettingsData.m_HighPriorityColor, lineSettingsData.m_HighPriorityColor, 0.0f, (OverlayRenderSystem.StyleFlags) 0, line, num2, dashLength, num2);
                    float2 float2 = MathUtils.Left(xz1);
                    line = new Line3.Segment(position, position);
                    line.a.xz -= float2 * (num1 * 0.5f);
                    line.b.xz += float2 * (num1 * 0.5f);
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawDashedLine(lineSettingsData.m_HighPriorityColor, lineSettingsData.m_HighPriorityColor, 0.0f, (OverlayRenderSystem.StyleFlags) 0, line, num2, dashLength, num2);
                    continue;
                  }
                  continue;
                case WaterToolSystem.Attribute.Radius:
                  // ISSUE: reference to a compiler-generated field
                  float2 xz2 = this.m_CameraRight.xz;
                  if (MathUtils.TryNormalize(ref xz2))
                  {
                    float2 float2 = MathUtils.Left(xz2);
                    float2 forward = (xz2 + float2) * 0.707106769f;
                    float3 startPos = position;
                    float3 startTangent = new float3();
                    float3 endPos = position;
                    float3 endTangent = new float3();
                    startPos.xz += forward * (num1 - num2 * 0.5f);
                    startTangent.xz = MathUtils.Right(forward);
                    endPos.xz += MathUtils.Right(forward) * (num1 - num2 * 0.5f);
                    endTangent.xz = -forward;
                    Bezier4x3 curve = NetUtils.FitCurve(startPos, startTangent, endTangent, endPos);
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCurve(lineSettingsData.m_HighPriorityColor, lineSettingsData.m_HighPriorityColor, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, num2);
                    curve.a.xz = 2f * position.xz - curve.a.xz;
                    curve.b.xz = 2f * position.xz - curve.b.xz;
                    curve.c.xz = 2f * position.xz - curve.c.xz;
                    curve.d.xz = 2f * position.xz - curve.d.xz;
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCurve(lineSettingsData.m_HighPriorityColor, lineSettingsData.m_HighPriorityColor, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, num2);
                    continue;
                  }
                  continue;
                case WaterToolSystem.Attribute.Rate:
                case WaterToolSystem.Attribute.Height:
                  // ISSUE: reference to a compiler-generated field
                  float2 xz3 = this.m_CameraRight.xz;
                  if (MathUtils.TryNormalize(ref xz3))
                  {
                    float2 float2 = MathUtils.Left(xz3);
                    float2 forward = (xz3 + float2) * 0.707106769f;
                    float3 startPos = position;
                    float3 startTangent = new float3();
                    float3 endPos = position;
                    float3 endTangent = new float3();
                    startPos.xz += forward * (num1 - num2 * 0.5f);
                    startTangent.xz = MathUtils.Left(forward);
                    endPos.xz += MathUtils.Left(forward) * (num1 - num2 * 0.5f);
                    endTangent.xz = -forward;
                    Bezier4x3 curve = NetUtils.FitCurve(startPos, startTangent, endTangent, endPos);
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCurve(lineSettingsData.m_HighPriorityColor, lineSettingsData.m_HighPriorityColor, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, num2);
                    curve.a.xz = 2f * position.xz - curve.a.xz;
                    curve.b.xz = 2f * position.xz - curve.b.xz;
                    curve.c.xz = 2f * position.xz - curve.c.xz;
                    curve.d.xz = 2f * position.xz - curve.d.xz;
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCurve(lineSettingsData.m_HighPriorityColor, lineSettingsData.m_HighPriorityColor, 0.0f, (OverlayRenderSystem.StyleFlags) 0, curve, num2);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct ObjectToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<ObjectDefinition> m_ObjectDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> m_OwnerDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> m_NetCourseType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> m_PrefabServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<LotData> m_PrefabLotData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PrefabPlaceableNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [ReadOnly]
      public NativeList<SubSnapPoint> m_SubSnapPoints;
      [ReadOnly]
      public NativeList<NetToolSystem.UpgradeState> m_NetUpgradeState;
      [ReadOnly]
      public GuideLineSettingsData m_GuideLineSettingsData;
      [ReadOnly]
      public ObjectToolSystem.Mode m_Mode;
      [ReadOnly]
      public ObjectToolSystem.State m_State;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public float m_DistanceScale;
      public NativeList<bool> m_AngleSides;
      public NativeList<GuideLinesSystem.TooltipInfo> m_Tooltips;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_NetUpgradeState.Length != 0;
        NativeParallelHashSet<float> nativeParallelHashSet = new NativeParallelHashSet<float>();
        int angleIndex = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!flag && this.m_State != ObjectToolSystem.State.Adding && this.m_State != ObjectToolSystem.State.Removing && (this.m_Mode == ObjectToolSystem.Mode.Line || this.m_Mode == ObjectToolSystem.Mode.Curve))
        {
          // ISSUE: reference to a compiler-generated method
          this.DrawControlPoints();
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<CreationDefinition> nativeArray1 = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<NetCourse> nativeArray2 = definitionChunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              CreationDefinition creationDefinition = nativeArray1[index2];
              NetCourse netCourse = nativeArray2[index2];
              NetGeometryData componentData;
              // ISSUE: reference to a compiler-generated field
              if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabNetGeometryData.TryGetComponent(creationDefinition.m_Prefab, out componentData))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.DrawNetCourse(this.m_OverlayBuffer, netCourse, componentData, this.m_GuideLineSettingsData);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<ObjectDefinition> nativeArray3 = definitionChunk.GetNativeArray<ObjectDefinition>(ref this.m_ObjectDefinitionType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<OwnerDefinition> nativeArray4 = definitionChunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
            for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
            {
              CreationDefinition creationDefinition = nativeArray1[index3];
              ObjectDefinition objectDefinition = nativeArray3[index3];
              if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0)
              {
                PlaceableObjectData componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabPlaceableObjectData.TryGetComponent(creationDefinition.m_Prefab, out componentData1) && (componentData1.m_Flags & Game.Objects.PlacementFlags.Hovering) != Game.Objects.PlacementFlags.None)
                {
                  // ISSUE: reference to a compiler-generated field
                  ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[creationDefinition.m_Prefab];
                  float x = MathUtils.Center(objectGeometryData.m_Bounds.x);
                  float width = MathUtils.Size(objectGeometryData.m_Bounds.x);
                  Line3.Segment line;
                  line.a = ObjectUtils.LocalToWorld(objectDefinition.m_Position, objectDefinition.m_Rotation, new float3(x, 0.0f, objectGeometryData.m_Bounds.min.z));
                  line.b = ObjectUtils.LocalToWorld(objectDefinition.m_Position, objectDefinition.m_Rotation, new float3(x, 0.0f, objectGeometryData.m_Bounds.max.z));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_MediumPriorityColor, this.m_GuideLineSettingsData.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, line, width, (float2) 0.02f);
                }
                DynamicBuffer<Game.Prefabs.SubArea> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabSubAreas.TryGetBuffer(creationDefinition.m_Prefab, out bufferData))
                {
                  for (int index4 = 0; index4 < bufferData.Length; ++index4)
                  {
                    LotData componentData2;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabLotData.TryGetComponent(bufferData[index4].m_Prefab, out componentData2) && (double) componentData2.m_MaxRadius > 0.0)
                    {
                      if (!nativeParallelHashSet.IsCreated)
                        nativeParallelHashSet = new NativeParallelHashSet<float>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                      if (nativeParallelHashSet.Add(componentData2.m_MaxRadius))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        GuideLinesSystem.DrawAreaRange(this.m_OverlayBuffer, objectDefinition.m_Rotation, objectDefinition.m_Position, componentData2);
                      }
                    }
                  }
                  if (nativeParallelHashSet.IsCreated)
                    nativeParallelHashSet.Clear();
                }
                ServiceUpgradeData componentData3;
                OwnerDefinition ownerDefinition;
                Game.Prefabs.BuildingData componentData4;
                Game.Prefabs.BuildingData componentData5;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabServiceUpgradeData.TryGetComponent(creationDefinition.m_Prefab, out componentData3) && (double) componentData3.m_MaxPlacementDistance != 0.0 && CollectionUtils.TryGet<OwnerDefinition>(nativeArray4, index3, out ownerDefinition) && this.m_PrefabBuildingData.TryGetComponent(ownerDefinition.m_Prefab, out componentData4) && this.m_PrefabBuildingData.TryGetComponent(creationDefinition.m_Prefab, out componentData5))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  GuideLinesSystem.DrawUpgradeRange(this.m_OverlayBuffer, ownerDefinition.m_Rotation, ownerDefinition.m_Position, this.m_GuideLineSettingsData, componentData4, componentData5, componentData3);
                }
                // ISSUE: reference to a compiler-generated method
                this.DrawSubSnapPoints(creationDefinition.m_Prefab, objectDefinition.m_Position, objectDefinition.m_Rotation, ref angleIndex);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == ObjectToolSystem.Mode.Stamp)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ControlPoints.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint = this.m_ControlPoints[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.DrawSubSnapPoints(this.m_Prefab, controlPoint.m_Position, controlPoint.m_Rotation, ref angleIndex);
          }
        }
        if (!nativeParallelHashSet.IsCreated)
          return;
        nativeParallelHashSet.Dispose();
      }

      private void DrawControlPoints()
      {
        int angleIndex = 0;
        Line3.Segment prevLine = new Line3.Segment();
        float3 prevPoint = (float3) -1000000f;
        // ISSUE: reference to a compiler-generated field
        double distanceScale = (double) this.m_DistanceScale;
        float num1 = (float) (distanceScale * 0.125);
        float num2 = (float) (distanceScale * 4.0);
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Line3.Segment line2 = new Line3.Segment(this.m_ControlPoints[0].m_Position, this.m_ControlPoints[1].m_Position);
          float x = MathUtils.Length(line2.xz);
          if ((double) x > (double) num1 * 7.0)
          {
            float2 startDir = (line2.b.xz - line2.a.xz) / x;
            float2 leftDir = new float2();
            float2 rightDir = new float2();
            float2 leftDir2 = new float2();
            float2 rightDir2 = new float2();
            int bestLeft = 181;
            int bestRight = 181;
            int bestLeft2 = 181;
            int bestRight2 = 181;
            float2 float2 = new float2();
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ControlPoints[0].m_Direction.Equals(new float2()))
            {
              // ISSUE: reference to a compiler-generated field
              float2 = this.m_ControlPoints[0].m_Direction;
            }
            else
            {
              Game.Objects.Transform componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.TryGetComponent(this.m_ControlPoints[0].m_OriginalEntity, out componentData))
                float2 = math.forward(componentData.m_Rotation).xz;
            }
            if (!float2.Equals(new float2()))
            {
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.CheckDirection(startDir, float2, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.CheckDirection(startDir, MathUtils.Right(float2), ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.CheckDirection(startDir, -float2, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.CheckDirection(startDir, MathUtils.Left(float2), ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
            }
            bool flag = bestRight < bestLeft;
            // ISSUE: reference to a compiler-generated field
            if (bestLeft == bestRight && this.m_AngleSides.Length > angleIndex)
            {
              // ISSUE: reference to a compiler-generated field
              flag = this.m_AngleSides[angleIndex];
            }
            if (bestLeft == 180 && bestRight == 180)
            {
              if (flag)
                bestLeft = 181;
              else
                bestRight = 181;
            }
            else
            {
              if (bestLeft2 <= 180 && bestRight2 <= 180)
              {
                if (bestLeft2 < bestRight2 || bestLeft2 == bestRight2 & flag)
                  bestRight2 = 181;
                else
                  bestLeft2 = 181;
              }
              if (bestLeft2 <= 180)
              {
                leftDir = leftDir2;
                bestLeft = bestLeft2;
              }
              else if (bestRight2 <= 180)
              {
                rightDir = rightDir2;
                bestRight = bestRight2;
              }
            }
            if (bestLeft <= 180)
            {
              Line3.Segment segment = new Line3.Segment(line2.a, line2.a);
              segment.a.xz += leftDir * math.min(x, num2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment, num1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment, line2, -leftDir, -startDir, math.min(x, num2) * 0.5f, num1, bestLeft, false);
            }
            if (bestRight <= 180)
            {
              Line3.Segment segment = new Line3.Segment(line2.a, line2.a);
              segment.a.xz += rightDir * math.min(x, num2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment, num1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment, line2, -rightDir, -startDir, math.min(x, num2) * 0.5f, num1, bestRight, true);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_AngleSides.Length > angleIndex)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AngleSides[angleIndex] = flag;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              while (this.m_AngleSides.Length <= angleIndex)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_AngleSides.Add(in flag);
              }
            }
          }
          ++angleIndex;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          // ISSUE: reference to a compiler-generated field
          for (int index = 1; index < this.m_ControlPoints.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint2 = this.m_ControlPoints[index];
            float num3 = (float) Mathf.RoundToInt(math.distance(controlPoint1.m_Position.xz, controlPoint2.m_Position.xz) * 2f) / 2f;
            if ((double) num3 > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeList<GuideLinesSystem.TooltipInfo> local1 = ref this.m_Tooltips;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              GuideLinesSystem.TooltipInfo tooltipInfo = new GuideLinesSystem.TooltipInfo(GuideLinesSystem.TooltipType.Length, (controlPoint1.m_Position + controlPoint2.m_Position) * 0.5f, num3);
              ref GuideLinesSystem.TooltipInfo local2 = ref tooltipInfo;
              local1.Add(in local2);
            }
            controlPoint1 = controlPoint2;
          }
        }
        ControlPoint controlPoint3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length >= 3)
        {
          // ISSUE: reference to a compiler-generated field
          float2 xz1 = this.m_ControlPoints[0].m_Position.xz;
          ref float2 local3 = ref xz1;
          // ISSUE: reference to a compiler-generated field
          controlPoint3 = this.m_ControlPoints[1];
          float2 xz2 = controlPoint3.m_Position.xz;
          if (!local3.Equals(xz2))
          {
            // ISSUE: reference to a compiler-generated field
            controlPoint3 = this.m_ControlPoints[2];
            float2 xz3 = controlPoint3.m_Position.xz;
            ref float2 local4 = ref xz3;
            // ISSUE: reference to a compiler-generated field
            controlPoint3 = this.m_ControlPoints[1];
            float2 xz4 = controlPoint3.m_Position.xz;
            if (!local4.Equals(xz4))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 startTangent = this.m_ControlPoints[1].m_Position - this.m_ControlPoints[0].m_Position;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3 = this.m_ControlPoints[2].m_Position - this.m_ControlPoints[1].m_Position;
              startTangent = MathUtils.Normalize(startTangent, startTangent.xz);
              float3 endTangent = MathUtils.Normalize(float3, float3.xz);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bezier4x3 input = NetUtils.FitCurve(this.m_ControlPoints[0].m_Position, startTangent, endTangent, this.m_ControlPoints[2].m_Position);
              float middleTangentPos = NetUtils.FindMiddleTangentPos(input.xz, new float2(0.0f, 1f));
              Bezier4x3 output1;
              Bezier4x3 output2;
              MathUtils.Divide(input, out output1, out output2, middleTangentPos);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_MediumPriorityColor, this.m_GuideLineSettingsData.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, output1, num1, new float2(1f, 0.0f));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_MediumPriorityColor, this.m_GuideLineSettingsData.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, output2, num1, new float2(0.0f, 1f));
              goto label_46;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          controlPoint3 = this.m_ControlPoints[0];
          float2 xz5 = controlPoint3.m_Position.xz;
          ref float2 local = ref xz5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
          float2 xz6 = controlPoint3.m_Position.xz;
          if (!local.Equals(xz6))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_MediumPriorityColor, this.m_GuideLineSettingsData.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, new Line3.Segment(this.m_ControlPoints[0].m_Position, this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_Position), num1, (float2) 1f);
            goto label_46;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length >= 1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_MediumPriorityColor, this.m_GuideLineSettingsData.m_MediumPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, new float2(0.0f, 1f), this.m_ControlPoints[0].m_Position, num1);
        }
label_46:
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ControlPoints.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint4 = this.m_ControlPoints[index];
          if (index > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPointLine(this.m_ControlPoints[index - 1], controlPoint4, num1, num2, ref angleIndex, ref prevLine);
          }
          // ISSUE: reference to a compiler-generated method
          this.DrawControlPoint(controlPoint4, num1, ref prevPoint);
        }
      }

      private void DrawControlPoint(ControlPoint point, float lineWidth, ref float3 prevPoint)
      {
        if ((double) math.distancesq(prevPoint, point.m_Position) > 0.0099999997764825821)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, point.m_Position, lineWidth * 5f);
        }
        prevPoint = point.m_Position;
      }

      private void DrawControlPointLine(
        ControlPoint point1,
        ControlPoint point2,
        float lineWidth,
        float lineLength,
        ref int angleIndex,
        ref Line3.Segment prevLine)
      {
        Line3.Segment line2 = new Line3.Segment(point1.m_Position, point2.m_Position);
        float num = math.distance(point1.m_Position.xz, point2.m_Position.xz);
        if ((double) num > (double) lineWidth * 8.0)
        {
          float3 float3 = (line2.b - line2.a) * (lineWidth * 4f / num);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(line2.a + float3, line2.b - float3), lineWidth * 3f, lineWidth * 5f, lineWidth * 3f);
        }
        // ISSUE: reference to a compiler-generated method
        this.DrawAngleIndicator(prevLine, line2, lineWidth, lineLength, angleIndex++);
        prevLine = line2;
      }

      private void DrawAngleIndicator(
        Line3.Segment line1,
        Line3.Segment line2,
        float lineWidth,
        float lineLength,
        int angleIndex)
      {
        bool angleSide = true;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AngleSides.Length > angleIndex)
        {
          // ISSUE: reference to a compiler-generated field
          angleSide = this.m_AngleSides[angleIndex];
        }
        float x = math.distance(line1.a.xz, line1.b.xz);
        float y = math.distance(line2.a.xz, line2.b.xz);
        if ((double) x > (double) lineWidth * 7.0 && (double) y > (double) lineWidth * 7.0)
        {
          float2 float2_1 = (line1.b.xz - line1.a.xz) / x;
          float2 float2_2 = (line2.a.xz - line2.b.xz) / y;
          float size = math.min(lineLength, math.min(x, y)) * 0.5f;
          int angle = Mathf.RoundToInt(math.degrees(math.acos(math.clamp(math.dot(float2_1, float2_2), -1f, 1f))));
          if (angle < 180)
            angleSide = (double) math.dot(MathUtils.Right(float2_1), float2_2) < 0.0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, line1, line2, float2_1, float2_2, size, lineWidth, angle, angleSide);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AngleSides.Length > angleIndex)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AngleSides[angleIndex] = angleSide;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          while (this.m_AngleSides.Length <= angleIndex)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AngleSides.Add(in angleSide);
          }
        }
      }

      private void DrawSubSnapPoints(
        Entity prefab,
        float3 position,
        quaternion rotation,
        ref int angleIndex)
      {
        DynamicBuffer<Game.Prefabs.SubNet> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubNets.TryGetBuffer(prefab, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Game.Prefabs.SubNet subNet = bufferData[index];
          float3 float3;
          if (subNet.m_Snapping.x)
          {
            float3 world = ObjectUtils.LocalToWorld(position, rotation, subNet.m_Curve.a);
            float2 a = new float2();
            float3 = math.mul(rotation, MathUtils.StartTangent(subNet.m_Curve));
            float2 b = math.normalizesafe(float3.xz);
            int num = subNet.m_NodeIndex.y != subNet.m_NodeIndex.x ? 1 : 0;
            float2 tangent = math.select(a, b, num != 0);
            // ISSUE: reference to a compiler-generated method
            this.DrawSubSnapPoints(subNet.m_Prefab, world, tangent, ref angleIndex);
          }
          if (subNet.m_Snapping.y)
          {
            float3 world = ObjectUtils.LocalToWorld(position, rotation, subNet.m_Curve.d);
            float3 = math.mul(rotation, -MathUtils.EndTangent(subNet.m_Curve));
            float2 tangent = math.normalizesafe(float3.xz);
            // ISSUE: reference to a compiler-generated method
            this.DrawSubSnapPoints(subNet.m_Prefab, world, tangent, ref angleIndex);
          }
        }
      }

      private void DrawSubSnapPoints(
        Entity prefab,
        float3 pos,
        float2 tangent,
        ref int angleIndex)
      {
        float2 leftDir = new float2();
        float2 rightDir = new float2();
        float2 leftDir2 = new float2();
        float2 rightDir2 = new float2();
        int bestLeft = 181;
        int bestRight = 181;
        int bestLeft2 = 181;
        int bestRight2 = 181;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SubSnapPoints.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          SubSnapPoint subSnapPoint = this.m_SubSnapPoints[index];
          if ((double) math.distancesq(pos.xz, subSnapPoint.m_Position.xz) < 0.0099999997764825821)
          {
            // ISSUE: reference to a compiler-generated method
            GuideLinesSystem.CheckDirection(tangent, subSnapPoint.m_Tangent, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
          }
        }
        float num1 = 1f;
        PlaceableNetData componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabPlaceableNetData.TryGetComponent(prefab, out componentData))
          num1 = math.min(componentData.m_SnapDistance, 16f);
        float num2 = num1 * 0.125f;
        float num3 = num1 * 4f;
        bool flag = bestRight < bestLeft;
        // ISSUE: reference to a compiler-generated field
        if (bestLeft == bestRight && this.m_AngleSides.Length > angleIndex)
        {
          // ISSUE: reference to a compiler-generated field
          flag = this.m_AngleSides[angleIndex];
        }
        if (bestLeft == 180 && bestRight == 180)
        {
          if (flag)
            bestLeft = 181;
          else
            bestRight = 181;
        }
        else
        {
          if (bestLeft2 <= 180 && bestRight2 <= 180)
          {
            if (bestLeft2 < bestRight2 || bestLeft2 == bestRight2 & flag)
              bestRight2 = 181;
            else
              bestLeft2 = 181;
          }
          if (bestLeft2 <= 180)
          {
            leftDir = leftDir2;
            bestLeft = bestLeft2;
          }
          else if (bestRight2 <= 180)
          {
            rightDir = rightDir2;
            bestRight = bestRight2;
          }
        }
        if (bestLeft <= 180 || bestRight <= 180)
        {
          if (tangent.Equals(new float2()))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_GuideLineSettingsData.m_HighPriorityColor with
            {
              a = 0.0f
            }, num2, (OverlayRenderSystem.StyleFlags) 0, new float2(0.0f, 1f), pos, num3 * 0.5f);
          }
          else
          {
            Line3.Segment segment1 = new Line3.Segment(pos, pos);
            segment1.b.xz += tangent * num3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment1, num2);
            if (bestLeft <= 180)
            {
              Line3.Segment segment2 = new Line3.Segment(pos, pos);
              segment2.a.xz += leftDir * num3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment2, num2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment2, segment1, -leftDir, -tangent, num3 * 0.5f, num2, bestLeft, false);
            }
            if (bestRight <= 180)
            {
              Line3.Segment segment3 = new Line3.Segment(pos, pos);
              segment3.a.xz += rightDir * num3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment3, num2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment3, segment1, -rightDir, -tangent, num3 * 0.5f, num2, bestRight, true);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AngleSides.Length > angleIndex)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AngleSides[angleIndex] = flag;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          while (this.m_AngleSides.Length <= angleIndex)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AngleSides.Add(in flag);
          }
        }
        ++angleIndex;
      }
    }

    [BurstCompile]
    private struct AreaToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<LotData> m_PrefabLotData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [ReadOnly]
      public NativeList<ControlPoint> m_MoveStartPositions;
      [ReadOnly]
      public AreaToolSystem.State m_State;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public GuideLineSettingsData m_GuideLineSettingsData;
      public NativeList<bool> m_AngleSides;
      public NativeList<GuideLinesSystem.TooltipInfo> m_Tooltips;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.State state = this.m_State;
        switch (state)
        {
          case AreaToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated field
            for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Game.Areas.Node> bufferAccessor = definitionChunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
              for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
              {
                CreationDefinition creationDefinition = nativeArray[index2];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabRefData.HasComponent(creationDefinition.m_Original) && this.m_OwnerData.HasComponent(creationDefinition.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[creationDefinition.m_Original];
                  // ISSUE: reference to a compiler-generated field
                  Owner owner = this.m_OwnerData[creationDefinition.m_Original];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabLotData.HasComponent(prefabRef.m_Prefab) && this.m_TransformData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    LotData lotData = this.m_PrefabLotData[prefabRef.m_Prefab];
                    // ISSUE: reference to a compiler-generated field
                    Game.Objects.Transform transform = this.m_TransformData[owner.m_Owner];
                    if ((double) lotData.m_MaxRadius > 0.0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      GuideLinesSystem.DrawAreaRange(this.m_OverlayBuffer, transform.m_Rotation, transform.m_Position, lotData);
                    }
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint1 = this.m_ControlPoints[0];
            // ISSUE: reference to a compiler-generated field
            if (this.m_Nodes.HasBuffer(controlPoint1.m_OriginalEntity) && math.any(controlPoint1.m_ElementIndex >= 0))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              AreaGeometryData areaGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefData[controlPoint1.m_OriginalEntity].m_Prefab];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, controlPoint1.m_Position, areaGeometryData.m_SnapDistance * 0.5f);
              break;
            }
            // ISSUE: reference to a compiler-generated field
            for (int index3 = 0; index3 < this.m_DefinitionChunks.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index3];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Game.Areas.Node> bufferAccessor = definitionChunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
              for (int index4 = 0; index4 < bufferAccessor.Length; ++index4)
              {
                CreationDefinition creationDefinition = nativeArray[index4];
                DynamicBuffer<Game.Areas.Node> dynamicBuffer = bufferAccessor[index4];
                // ISSUE: reference to a compiler-generated field
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabRefData.HasComponent(creationDefinition.m_Original) && dynamicBuffer.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef prefabRef = this.m_PrefabRefData[creationDefinition.m_Original];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabGeometryData.HasComponent(prefabRef.m_Prefab))
                  {
                    // ISSUE: reference to a compiler-generated field
                    AreaGeometryData areaGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, dynamicBuffer[0].m_Position, areaGeometryData.m_SnapDistance * 0.5f);
                  }
                }
              }
            }
            break;
          case AreaToolSystem.State.Create:
            // ISSUE: reference to a compiler-generated field
            for (int index5 = 0; index5 < this.m_DefinitionChunks.Length; ++index5)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index5];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Game.Areas.Node> bufferAccessor = definitionChunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
              for (int index6 = 0; index6 < bufferAccessor.Length; ++index6)
              {
                CreationDefinition creationDefinition = nativeArray[index6];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabLotData.HasComponent(creationDefinition.m_Prefab) && this.m_OwnerData.HasComponent(creationDefinition.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  LotData lotData = this.m_PrefabLotData[creationDefinition.m_Prefab];
                  // ISSUE: reference to a compiler-generated field
                  Owner owner = this.m_OwnerData[creationDefinition.m_Original];
                  // ISSUE: reference to a compiler-generated field
                  if ((double) lotData.m_MaxRadius > 0.0 && this.m_TransformData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Objects.Transform transform = this.m_TransformData[owner.m_Owner];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    GuideLinesSystem.DrawAreaRange(this.m_OverlayBuffer, transform.m_Rotation, transform.m_Position, lotData);
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            for (int index7 = 0; index7 < this.m_DefinitionChunks.Length; ++index7)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index7];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Game.Areas.Node> bufferAccessor = definitionChunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
              for (int index8 = 0; index8 < bufferAccessor.Length; ++index8)
              {
                CreationDefinition creationDefinition = nativeArray[index8];
                // ISSUE: reference to a compiler-generated field
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabGeometryData.HasComponent(creationDefinition.m_Prefab))
                {
                  DynamicBuffer<Game.Areas.Node> dynamicBuffer = bufferAccessor[index8];
                  if (dynamicBuffer.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    AreaGeometryData areaGeometryData = this.m_PrefabGeometryData[creationDefinition.m_Prefab];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, dynamicBuffer[dynamicBuffer.Length - 1].m_Position, areaGeometryData.m_SnapDistance * 0.5f);
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.DrawAngles();
            break;
          case AreaToolSystem.State.Modify:
          case AreaToolSystem.State.Remove:
            // ISSUE: reference to a compiler-generated field
            for (int index = 0; index < this.m_MoveStartPositions.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint moveStartPosition = this.m_MoveStartPositions[index];
              PrefabRef componentData1;
              Owner componentData2;
              LotData componentData3;
              Game.Objects.Transform componentData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.TryGetComponent(moveStartPosition.m_OriginalEntity, out componentData1) && this.m_OwnerData.TryGetComponent(moveStartPosition.m_OriginalEntity, out componentData2) && this.m_PrefabLotData.TryGetComponent(componentData1.m_Prefab, out componentData3) && this.m_TransformData.TryGetComponent(componentData2.m_Owner, out componentData4) && (double) componentData3.m_MaxRadius > 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.DrawAreaRange(this.m_OverlayBuffer, componentData4.m_Rotation, componentData4.m_Position, componentData3);
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_MoveStartPositions.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint moveStartPosition = this.m_MoveStartPositions[0];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Nodes.HasBuffer(moveStartPosition.m_OriginalEntity) && math.any(moveStartPosition.m_ElementIndex >= 0))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                AreaGeometryData areaGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefData[moveStartPosition.m_OriginalEntity].m_Prefab];
                // ISSUE: reference to a compiler-generated field
                ControlPoint controlPoint2 = this.m_ControlPoints[0];
                if (controlPoint2.Equals(new ControlPoint()) || controlPoint2.Equals(moveStartPosition))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, moveStartPosition.m_Position, areaGeometryData.m_SnapDistance * 0.5f);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, controlPoint2.m_Position, areaGeometryData.m_SnapDistance * 0.5f);
                }
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.DrawAngles();
            break;
        }
      }

      private void DrawAngles()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        AreaToolSystem.State state = this.m_State;
        int num1;
        switch (state)
        {
          case AreaToolSystem.State.Create:
            // ISSUE: reference to a compiler-generated field
            num1 = math.select(0, 1, this.m_ControlPoints.Length >= 2);
            break;
          case AreaToolSystem.State.Modify:
            // ISSUE: reference to a compiler-generated field
            num1 = this.m_MoveStartPositions.Length * 2;
            break;
          default:
            return;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabGeometryData.HasComponent(this.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        double num2 = (double) math.min(this.m_PrefabGeometryData[this.m_Prefab].m_SnapDistance, 16f);
        float num3 = (float) (num2 * 0.125);
        float y = (float) (num2 * 4.0);
        for (int index = 0; index < num1; ++index)
        {
          float2 leftDir = new float2();
          float2 rightDir = new float2();
          float2 leftDir2 = new float2();
          float2 rightDir2 = new float2();
          int bestLeft = 181;
          int bestRight = 181;
          int bestLeft2 = 181;
          int bestRight2 = 181;
          Line3.Segment line2;
          float x;
          float2 startDir;
          // ISSUE: reference to a compiler-generated field
          if (this.m_State == AreaToolSystem.State.Create)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            line2 = new Line3.Segment(this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_Position, this.m_ControlPoints[this.m_ControlPoints.Length - 1].m_Position);
            x = MathUtils.Length(line2.xz);
            startDir = (line2.b.xz - line2.a.xz) / x;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControlPoints.Length >= 3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float2 checkDir = math.normalizesafe(this.m_ControlPoints[this.m_ControlPoints.Length - 3].m_Position.xz - this.m_ControlPoints[this.m_ControlPoints.Length - 2].m_Position.xz);
              if (!checkDir.Equals(new float2()))
              {
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, checkDir, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              }
            }
            if (bestLeft > 180 && bestRight > 180)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
              if (!controlPoint.m_Direction.Equals(new float2()))
              {
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, controlPoint.m_Direction, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, MathUtils.Right(controlPoint.m_Direction), ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, -controlPoint.m_Direction, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, MathUtils.Left(controlPoint.m_Direction), ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            ControlPoint moveStartPosition = this.m_MoveStartPositions[index >> 1];
            // ISSUE: reference to a compiler-generated field
            if (this.m_Nodes.HasBuffer(moveStartPosition.m_OriginalEntity) && math.any(moveStartPosition.m_ElementIndex >= 0))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[moveStartPosition.m_OriginalEntity];
              int2 int2;
              if ((index & 1) == 0)
              {
                int2 a = math.select(moveStartPosition.m_ElementIndex.x + new int2(-1, -2), moveStartPosition.m_ElementIndex.y + new int2(0, -1), moveStartPosition.m_ElementIndex.y >= 0);
                int2 = math.select(a, a + node.Length, a < 0);
              }
              else
              {
                int2 a = math.select(moveStartPosition.m_ElementIndex.x + new int2(1, 2), moveStartPosition.m_ElementIndex.y + new int2(1, 2), moveStartPosition.m_ElementIndex.y >= 0);
                int2 = math.select(a, a - node.Length, a >= node.Length);
              }
              // ISSUE: reference to a compiler-generated field
              float3 position1 = this.m_ControlPoints[0].m_Position;
              float3 position2 = node[int2.x].m_Position;
              float3 position3 = node[int2.y].m_Position;
              line2 = new Line3.Segment(position2, position1);
              x = MathUtils.Length(line2.xz);
              startDir = (line2.b.xz - line2.a.xz) / x;
              float2 checkDir = math.normalizesafe(position3.xz - position2.xz);
              if (!checkDir.Equals(new float2()))
              {
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, checkDir, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              }
            }
            else
              continue;
          }
          bool flag = bestRight < bestLeft;
          // ISSUE: reference to a compiler-generated field
          if (bestLeft == bestRight && this.m_AngleSides.Length > index)
          {
            // ISSUE: reference to a compiler-generated field
            flag = this.m_AngleSides[index];
          }
          if (bestLeft == 180 && bestRight == 180)
          {
            if (flag)
              bestLeft = 181;
            else
              bestRight = 181;
          }
          else
          {
            if (bestLeft2 <= 180 && bestRight2 <= 180)
            {
              if (bestLeft2 < bestRight2 || bestLeft2 == bestRight2 & flag)
                bestRight2 = 181;
              else
                bestLeft2 = 181;
            }
            if (bestLeft2 <= 180)
            {
              leftDir = leftDir2;
              bestLeft = bestLeft2;
            }
            else if (bestRight2 <= 180)
            {
              rightDir = rightDir2;
              bestRight = bestRight2;
            }
          }
          if (bestLeft <= 180 || bestRight <= 180)
          {
            Line3.Segment line = new Line3.Segment(line2.a, line2.a);
            line.a.xz += startDir * math.min(x, y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, line, num3);
          }
          if (bestLeft <= 180)
          {
            Line3.Segment segment = new Line3.Segment(line2.a, line2.a);
            segment.a.xz += leftDir * math.min(x, y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment, num3);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment, line2, -leftDir, -startDir, math.min(x, y) * 0.5f, num3, bestLeft, false);
          }
          if (bestRight <= 180)
          {
            Line3.Segment segment = new Line3.Segment(line2.a, line2.a);
            segment.a.xz += rightDir * math.min(x, y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment, num3);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment, line2, -rightDir, -startDir, math.min(x, y) * 0.5f, num3, bestRight, true);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_AngleSides.Length > index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AngleSides[index] = flag;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            while (this.m_AngleSides.Length <= index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AngleSides.Add(in flag);
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct SelectionToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public SelectionToolSystem.State m_State;
      [ReadOnly]
      public SelectionType m_SelectionType;
      [ReadOnly]
      public bool m_SelectionQuadIsValid;
      [ReadOnly]
      public Quad3 m_SelectionQuad;
      [ReadOnly]
      public GuideLineSettingsData m_GuideLineSettingsData;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        SelectionToolSystem.State state = this.m_State;
        switch (state)
        {
          case SelectionToolSystem.State.Selecting:
          case SelectionToolSystem.State.Deselecting:
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SelectionQuadIsValid)
              break;
            float num;
            // ISSUE: reference to a compiler-generated field
            switch (this.m_SelectionType)
            {
              case SelectionType.ServiceDistrict:
                num = AreaUtils.GetMinNodeDistance(Game.Areas.AreaType.District) * 2f;
                break;
              case SelectionType.MapTiles:
                num = AreaUtils.GetMinNodeDistance(Game.Areas.AreaType.MapTile) * 2f;
                break;
              default:
                num = 16f;
                break;
            }
            float width = num * 0.125f;
            float dashLength = num * 0.7f;
            float gapLength = num * 0.3f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_SelectionQuad.ab, width, dashLength, gapLength, (float2) 1f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_SelectionQuad.ad, width, dashLength, gapLength, (float2) 1f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_SelectionQuad.bc, width, dashLength, gapLength, (float2) 1f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_SelectionQuad.dc, width, dashLength, gapLength, (float2) 1f);
            break;
        }
      }
    }

    [BurstCompile]
    private struct ZoneToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<Zoning> m_ZoningType;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public GuideLineSettingsData m_GuideLineSettingsData;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<CreationDefinition> nativeArray1 = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Zoning> nativeArray2 = definitionChunk.GetNativeArray<Zoning>(ref this.m_ZoningType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            CreationDefinition creationDefinition = nativeArray1[index2];
            Zoning zoning = nativeArray2[index2];
            if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && (zoning.m_Flags & ZoningFlags.Marquee) != (ZoningFlags) 0)
            {
              float3 a = zoning.m_Position.a;
              float3 b = zoning.m_Position.b;
              float3 c = zoning.m_Position.c;
              float3 d = zoning.m_Position.d;
              float width = 1f;
              float dashLength = 5.6f;
              float gapLength = 2.4f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(a, b), width, dashLength, gapLength, (float2) 1f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(b, c), width, dashLength, gapLength, (float2) 1f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(c, d), width, dashLength, gapLength, (float2) 1f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(d, a), width, dashLength, gapLength, (float2) 1f);
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct RouteToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public BufferTypeHandle<WaypointDefinition> m_WaypointDefinitionType;
      [ReadOnly]
      public ComponentLookup<Route> m_RouteData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [ReadOnly]
      public ControlPoint m_MoveStartPosition;
      [ReadOnly]
      public RouteToolSystem.State m_State;
      [ReadOnly]
      public GuideLineSettingsData m_GuideLineSettingsData;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControlPoints.Length <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        RouteToolSystem.State state = this.m_State;
        switch (state)
        {
          case RouteToolSystem.State.Default:
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint1 = this.m_ControlPoints[0];
            // ISSUE: reference to a compiler-generated field
            if (this.m_RouteData.HasComponent(controlPoint1.m_OriginalEntity) && math.any(controlPoint1.m_ElementIndex >= 0))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              RouteData routeData = this.m_PrefabRouteData[this.m_PrefabRefData[controlPoint1.m_OriginalEntity].m_Prefab];
              if (controlPoint1.m_ElementIndex.x >= 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, controlPoint1.m_Position, routeData.m_Width * 1.77777779f);
                break;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, controlPoint1.m_Position, routeData.m_Width * 0.8888889f);
              break;
            }
            // ISSUE: reference to a compiler-generated field
            for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<WaypointDefinition> bufferAccessor = definitionChunk.GetBufferAccessor<WaypointDefinition>(ref this.m_WaypointDefinitionType);
              for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
              {
                CreationDefinition creationDefinition = nativeArray[index2];
                // ISSUE: reference to a compiler-generated field
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabRouteData.HasComponent(creationDefinition.m_Prefab))
                {
                  DynamicBuffer<WaypointDefinition> dynamicBuffer = bufferAccessor[index2];
                  if (dynamicBuffer.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    RouteData routeData = this.m_PrefabRouteData[creationDefinition.m_Prefab];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, dynamicBuffer[0].m_Position, routeData.m_Width * 1.77777779f);
                  }
                }
              }
            }
            break;
          case RouteToolSystem.State.Create:
            // ISSUE: reference to a compiler-generated field
            for (int index3 = 0; index3 < this.m_DefinitionChunks.Length; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index3];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<WaypointDefinition> bufferAccessor = definitionChunk.GetBufferAccessor<WaypointDefinition>(ref this.m_WaypointDefinitionType);
              for (int index4 = 0; index4 < bufferAccessor.Length; ++index4)
              {
                CreationDefinition creationDefinition = nativeArray[index4];
                // ISSUE: reference to a compiler-generated field
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_PrefabRouteData.HasComponent(creationDefinition.m_Prefab))
                {
                  DynamicBuffer<WaypointDefinition> dynamicBuffer = bufferAccessor[index4];
                  if (dynamicBuffer.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    RouteData routeData = this.m_PrefabRouteData[creationDefinition.m_Prefab];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, dynamicBuffer[dynamicBuffer.Length - 1].m_Position, routeData.m_Width * 1.77777779f);
                  }
                }
              }
            }
            break;
          case RouteToolSystem.State.Modify:
          case RouteToolSystem.State.Remove:
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_RouteData.HasComponent(this.m_MoveStartPosition.m_OriginalEntity) || !math.any(this.m_MoveStartPosition.m_ElementIndex >= 0))
              break;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            RouteData routeData1 = this.m_PrefabRouteData[this.m_PrefabRefData[this.m_MoveStartPosition.m_OriginalEntity].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
            // ISSUE: reference to a compiler-generated field
            if (controlPoint2.Equals(new ControlPoint()) || controlPoint2.Equals(this.m_MoveStartPosition))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_MoveStartPosition.m_ElementIndex.x >= 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_MoveStartPosition.m_Position, routeData1.m_Width * 1.77777779f);
                break;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, this.m_MoveStartPosition.m_Position, routeData1.m_Width * 0.8888889f);
              break;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, controlPoint2.m_Position, routeData1.m_Width * 1.77777779f);
            break;
        }
      }
    }

    [BurstCompile]
    private struct NetToolGuideLinesJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> m_NetCourseType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> m_ConnectedEdgeType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PlaceableNetData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_RoadData;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> m_ElectricityConnectionData;
      [ReadOnly]
      public ComponentLookup<WaterPipeConnectionData> m_WaterPipeConnectionData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DefinitionChunks;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_MarkerNodeChunks;
      [ReadOnly]
      public NativeList<ControlPoint> m_ControlPoints;
      [ReadOnly]
      public NativeList<SnapLine> m_SnapLines;
      [ReadOnly]
      public NetToolSystem.Mode m_Mode;
      [ReadOnly]
      public Game.Prefabs.ElectricityConnection.Voltage m_HighlightVoltage;
      [ReadOnly]
      public bool2 m_HighlightWater;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public GuideLineSettingsData m_GuideLineSettingsData;
      public NativeList<bool> m_AngleSides;
      public NativeList<GuideLinesSystem.TooltipInfo> m_Tooltips;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated method
        this.DrawZones();
        // ISSUE: reference to a compiler-generated method
        this.DrawCourses();
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace)
          return;
        // ISSUE: reference to a compiler-generated method
        this.DrawSnapLines();
        // ISSUE: reference to a compiler-generated method
        this.DrawControlPoints();
        // ISSUE: reference to a compiler-generated method
        this.DrawMarkers();
      }

      private void DrawMarkers()
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MarkerNodeChunks.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_MarkerNodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk markerNodeChunk = this.m_MarkerNodeChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.Node> nativeArray1 = markerNodeChunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray2 = markerNodeChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<ConnectedEdge> bufferAccessor = markerNodeChunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Game.Net.Node node = nativeArray1[index2];
            PrefabRef prefabRef = nativeArray2[index2];
            if (bufferAccessor[index2].Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_HighlightVoltage != Game.Prefabs.ElectricityConnection.Voltage.Invalid)
              {
                ElectricityConnectionData componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ElectricityConnectionData.TryGetComponent(prefabRef.m_Prefab, out componentData) || componentData.m_Voltage != this.m_HighlightVoltage)
                  continue;
              }
              else
              {
                WaterPipeConnectionData componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (math.any(this.m_HighlightWater) && (!this.m_WaterPipeConnectionData.TryGetComponent(prefabRef.m_Prefab, out componentData) || !math.any(new int2(componentData.m_FreshCapacity, componentData.m_SewageCapacity) > 0 & this.m_HighlightWater)))
                  continue;
              }
              NetGeometryData componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_NetGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
              {
                // ISSUE: reference to a compiler-generated field
                UnityEngine.Color mediumPriorityColor = this.m_GuideLineSettingsData.m_MediumPriorityColor;
                mediumPriorityColor.a *= 0.1f;
                float defaultWidth = componentData1.m_DefaultWidth;
                float outlineWidth = (float) (((double) math.sqrt(defaultWidth + 1f) - 1.0) * 0.30000001192092896);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_MediumPriorityColor, mediumPriorityColor, outlineWidth, (OverlayRenderSystem.StyleFlags) 0, new float2(0.0f, 1f), node.m_Position, defaultWidth);
              }
            }
          }
        }
      }

      private void DrawCourses()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<CreationDefinition> nativeArray1 = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<NetCourse> nativeArray2 = definitionChunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            NetCourse netCourse = nativeArray2[index2];
            CreationDefinition creationDefinition = nativeArray1[index2];
            NetGeometryData componentData;
            // ISSUE: reference to a compiler-generated field
            if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && this.m_NetGeometryData.TryGetComponent(creationDefinition.m_Prefab, out componentData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawNetCourse(this.m_OverlayBuffer, netCourse, componentData, this.m_GuideLineSettingsData);
            }
          }
        }
      }

      private void DrawZones()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<CreationDefinition> nativeArray1 = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<NetCourse> nativeArray2 = definitionChunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            NetCourse netCourse = nativeArray2[index2];
            CreationDefinition creationDefinition = nativeArray1[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((creationDefinition.m_Flags & (CreationFlags.Permanent | CreationFlags.Upgrade)) == (CreationFlags) 0 && this.m_RoadData.HasComponent(creationDefinition.m_Prefab) && this.m_NetGeometryData.HasComponent(creationDefinition.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              NetGeometryData netGeometryData = this.m_NetGeometryData[creationDefinition.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              if (!(this.m_RoadData[creationDefinition.m_Prefab].m_ZoneBlockPrefab == Entity.Null))
              {
                float2 float2_1 = math.max((float2) math.max(math.cmin(netCourse.m_StartPosition.m_Elevation), math.cmin(netCourse.m_EndPosition.m_Elevation)), netCourse.m_Elevation);
                float2 float2_2 = math.min((float2) math.min(math.cmax(netCourse.m_StartPosition.m_Elevation), math.cmax(netCourse.m_EndPosition.m_Elevation)), netCourse.m_Elevation);
                double elevationLimit = (double) netGeometryData.m_ElevationLimit;
                bool2 x = float2_1 < (float) elevationLimit & float2_2 > -netGeometryData.m_ElevationLimit;
                x.x &= (netCourse.m_StartPosition.m_Flags & CoursePosFlags.IsLeft) > (CoursePosFlags) 0;
                x.y &= (netCourse.m_StartPosition.m_Flags & CoursePosFlags.IsRight) > (CoursePosFlags) 0;
                if (math.any(x))
                {
                  float offset = (float) (((double) ZoneUtils.GetCellWidth(netGeometryData.m_DefaultWidth) * 0.5 + 6.0) * 8.0 - 1.0);
                  int num = (netCourse.m_StartPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) > (CoursePosFlags) 0 ? 1 : 0;
                  bool flag1 = (netCourse.m_EndPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) > (CoursePosFlags) 0;
                  bool flag2 = (double) netCourse.m_Length > 0.10000000149011612;
                  if (num != 0)
                  {
                    if ((netCourse.m_StartPosition.m_Flags & CoursePosFlags.IsGrid) != (CoursePosFlags) 0)
                    {
                      if ((netCourse.m_StartPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsParallel)) == CoursePosFlags.IsFirst || (netCourse.m_StartPosition.m_Flags & (CoursePosFlags.IsLast | CoursePosFlags.IsParallel)) == (CoursePosFlags.IsLast | CoursePosFlags.IsParallel))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.DrawZoneCircle(netCourse.m_StartPosition, offset, true, x.x, x.y);
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.DrawZoneCircle(netCourse.m_StartPosition, offset, true, !flag2, x.x, x.y);
                    }
                  }
                  if (flag2)
                  {
                    if (flag1)
                    {
                      if ((netCourse.m_EndPosition.m_Flags & CoursePosFlags.IsGrid) != (CoursePosFlags) 0)
                      {
                        if ((netCourse.m_EndPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsParallel)) == CoursePosFlags.IsFirst || (netCourse.m_EndPosition.m_Flags & (CoursePosFlags.IsLast | CoursePosFlags.IsParallel)) == (CoursePosFlags.IsLast | CoursePosFlags.IsParallel))
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.DrawZoneCircle(netCourse.m_EndPosition, offset, false, x.x, x.y);
                        }
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.DrawZoneCircle(netCourse.m_EndPosition, offset, false, true, x.x, x.y);
                      }
                    }
                    float2 float2_3 = new float2(netCourse.m_StartPosition.m_CourseDelta, netCourse.m_EndPosition.m_CourseDelta);
                    Bezier4x3 source1 = MathUtils.Cut(netCourse.m_Curve, new float2(float2_3.x, math.lerp(float2_3.x, float2_3.y, 0.5f)));
                    Bezier4x3 source2 = MathUtils.Cut(netCourse.m_Curve, new float2(math.lerp(float2_3.x, float2_3.y, 0.5f), float2_3.y));
                    if (x.x)
                    {
                      Bezier4x3 result1;
                      // ISSUE: reference to a compiler-generated method
                      if (this.GetOffsetCurve(source1, offset, out result1))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, result1, 2f);
                      }
                      Bezier4x3 result2;
                      // ISSUE: reference to a compiler-generated method
                      if (this.GetOffsetCurve(source2, offset, out result2))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, result2, 2f);
                      }
                    }
                    if (x.y)
                    {
                      Bezier4x3 result3;
                      // ISSUE: reference to a compiler-generated method
                      if (this.GetOffsetCurve(source1, -offset, out result3))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, result3, 2f);
                      }
                      Bezier4x3 result4;
                      // ISSUE: reference to a compiler-generated method
                      if (this.GetOffsetCurve(source2, -offset, out result4))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, result4, 2f);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void DrawZoneCircle(
        CoursePos coursePos,
        float offset,
        bool start,
        bool left,
        bool right)
      {
        Bezier4x3 curve1 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, -offset, -offset);
        Bezier4x3 curve2 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, offset, -offset);
        Bezier4x3 curve3 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, offset, offset);
        Bezier4x3 curve4 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, -offset, offset);
        if (start)
        {
          if (left)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve1, 2f);
          }
          if (!right)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve2, 2f);
        }
        else
        {
          if (right)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve3, 2f);
          }
          if (!left)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve4, 2f);
        }
      }

      private void DrawZoneCircle(
        CoursePos coursePos,
        float offset,
        bool fullStart,
        bool fullEnd,
        bool left,
        bool right)
      {
        Bezier4x3 curve1 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, -offset, -offset);
        Bezier4x3 curve2 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, offset, -offset);
        Bezier4x3 curve3 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, offset, offset);
        Bezier4x3 curve4 = NetUtils.CircleCurve(coursePos.m_Position, coursePos.m_Rotation, -offset, offset);
        if (fullStart)
        {
          if (left)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve1, 2f);
          }
          if (right)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve2, 2f);
          }
        }
        else
        {
          if (left)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_VeryLowPriorityColor, this.m_GuideLineSettingsData.m_VeryLowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, MathUtils.Cut(curve1, new float2(0.25f, 1f)), 2f);
          }
          if (right)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_VeryLowPriorityColor, this.m_GuideLineSettingsData.m_VeryLowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, MathUtils.Cut(curve2, new float2(0.25f, 1f)), 2f);
          }
        }
        if (fullEnd)
        {
          if (right)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve3, 2f);
          }
          if (!left)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_LowPriorityColor, this.m_GuideLineSettingsData.m_LowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, curve4, 2f);
        }
        else
        {
          if (right)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_VeryLowPriorityColor, this.m_GuideLineSettingsData.m_VeryLowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, MathUtils.Cut(curve3, new float2(0.25f, 1f)), 2f);
          }
          if (!left)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCurve(this.m_GuideLineSettingsData.m_VeryLowPriorityColor, this.m_GuideLineSettingsData.m_VeryLowPriorityColor, 0.0f, OverlayRenderSystem.StyleFlags.Projected, MathUtils.Cut(curve4, new float2(0.25f, 1f)), 2f);
        }
      }

      private bool GetOffsetCurve(Bezier4x3 source, float offset, out Bezier4x3 result)
      {
        result = NetUtils.OffsetCurveLeftSmooth(source, (float2) offset);
        return (double) math.dot(source.d.xz - source.a.xz, result.d.xz - result.a.xz) > 0.0;
      }

      private void DrawSnapLines()
      {
        float num = 1f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceableNetData.HasComponent(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num = math.min(this.m_PlaceableNetData[this.m_Prefab].m_SnapDistance, 16f);
        }
        float width = num * 0.125f;
        float y1 = num * 4f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace || this.m_ControlPoints.Length < 1)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ControlPoint controlPoint = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
        NativeList<GuideLinesSystem.NetToolGuideLinesJob.SnapDir> nativeList = new NativeList<GuideLinesSystem.NetToolGuideLinesJob.SnapDir>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        bool flag = false;
label_26:
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_SnapLines.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          SnapLine snapLine = this.m_SnapLines[index1];
          float t;
          if ((snapLine.m_Flags & SnapLineFlags.Hidden) == (SnapLineFlags) 0 && (double) NetUtils.ExtendedDistance(snapLine.m_Curve.xz, controlPoint.m_Position.xz, out t) < 0.10000000149011612)
          {
            float3 position;
            float3 tangent;
            NetUtils.ExtendedPositionAndTangent(snapLine.m_Curve, t, out position, out tangent);
            tangent = MathUtils.Normalize(tangent, tangent.xz);
            position.y = controlPoint.m_Position.y;
            float3 float3 = position - snapLine.m_Curve.a;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            GuideLinesSystem.NetToolGuideLinesJob.SnapDir snapDir1 = new GuideLinesSystem.NetToolGuideLinesJob.SnapDir()
            {
              m_Direction = tangent.xz,
              m_Height = (float2) MathUtils.Normalize(float3, float3.xz).y
            };
            if ((snapLine.m_Flags & SnapLineFlags.GuideLine) != (SnapLineFlags) 0)
            {
              float y2 = math.dot(tangent.xz, float3.xz);
              // ISSUE: reference to a compiler-generated field
              snapDir1.m_Factor.x = math.max(0.0f, y2);
              // ISSUE: reference to a compiler-generated field
              snapDir1.m_Factor.y = 1000000f;
              flag = true;
            }
            else if ((snapLine.m_Flags & SnapLineFlags.Secondary) != (SnapLineFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              snapDir1.m_Factor = (float2) 1000000f;
            }
            else
              flag = true;
            for (int index2 = 0; index2 < nativeList.Length; ++index2)
            {
              // ISSUE: variable of a compiler-generated type
              GuideLinesSystem.NetToolGuideLinesJob.SnapDir snapDir2 = nativeList[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              switch (Mathf.RoundToInt(math.degrees(math.acos(math.clamp(math.dot(snapDir1.m_Direction, snapDir2.m_Direction), -1f, 1f)))))
              {
                case 0:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) snapDir1.m_Factor.x < (double) snapDir2.m_Factor.x)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Factor.x = snapDir1.m_Factor.x;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Height.x = snapDir1.m_Height.x;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) snapDir1.m_Factor.y < (double) snapDir2.m_Factor.y)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Factor.y = snapDir1.m_Factor.y;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Height.y = snapDir1.m_Height.y;
                  }
                  nativeList[index2] = snapDir2;
                  goto label_26;
                case 180:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) snapDir1.m_Factor.y < (double) snapDir2.m_Factor.x)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Factor.x = snapDir1.m_Factor.y;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Height.x = -snapDir1.m_Height.y;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) snapDir1.m_Factor.x < (double) snapDir2.m_Factor.y)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Factor.y = snapDir1.m_Factor.x;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    snapDir2.m_Height.y = -snapDir1.m_Height.x;
                  }
                  nativeList[index2] = snapDir2;
                  goto label_26;
                default:
                  continue;
              }
            }
            nativeList.Add(in snapDir1);
          }
        }
        for (int index = 0; index < nativeList.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          GuideLinesSystem.NetToolGuideLinesJob.SnapDir snapDir = nativeList[index];
          // ISSUE: reference to a compiler-generated field
          if (!flag || !math.all(snapDir.m_Factor == 1000000f))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float3 float3_1 = new float3(snapDir.m_Direction.x, 0.0f, snapDir.m_Direction.y);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(controlPoint.m_Position - float3_1 * y1, controlPoint.m_Position + float3_1 * y1), width);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 float2 = MathUtils.Snap(math.min(width / math.abs(snapDir.m_Height), (float2) y1) - snapDir.m_Factor, (float2) (width * 4f)) + snapDir.m_Factor;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) snapDir.m_Factor.x > (double) float2.x + (double) width * 3.0 && (double) snapDir.m_Factor.x < 1000000.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3_2 = new float3(snapDir.m_Direction.x, snapDir.m_Height.x, snapDir.m_Direction.y);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(controlPoint.m_Position - float3_2 * (snapDir.m_Factor.x + width), controlPoint.m_Position - float3_2 * (float2.x + width)), width, width * 2f, width * 2f);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) snapDir.m_Factor.y > (double) float2.y + (double) width * 3.0 && (double) snapDir.m_Factor.y < 1000000.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float3 float3_3 = new float3(snapDir.m_Direction.x, snapDir.m_Height.y, snapDir.m_Direction.y);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(controlPoint.m_Position + float3_3 * (snapDir.m_Factor.y + width), controlPoint.m_Position + float3_3 * (float2.y + width)), width, width * 2f, width * 2f);
            }
          }
        }
        nativeList.Dispose();
      }

      private void DrawControlPoints()
      {
        int angleIndex = 0;
        Line3.Segment prevLine = new Line3.Segment();
        float3 prevPoint = (float3) -1000000f;
        float num1 = 1f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PlaceableNetData.HasComponent(this.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num1 = math.min(this.m_PlaceableNetData[this.m_Prefab].m_SnapDistance, 16f);
        }
        float num2 = num1 * 0.125f;
        float num3 = num1 * 4f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode != NetToolSystem.Mode.Replace && this.m_ControlPoints.Length >= 2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Line3.Segment line2 = new Line3.Segment(this.m_ControlPoints[0].m_Position, this.m_ControlPoints[1].m_Position);
          float x = MathUtils.Length(line2.xz);
          if ((double) x > (double) num2 * 7.0)
          {
            float2 startDir = (line2.b.xz - line2.a.xz) / x;
            float2 leftDir = new float2();
            float2 rightDir = new float2();
            float2 leftDir2 = new float2();
            float2 rightDir2 = new float2();
            int bestLeft = 181;
            int bestRight = 181;
            int bestLeft2 = 181;
            int bestRight2 = 181;
            // ISSUE: reference to a compiler-generated field
            for (int index1 = 0; index1 < this.m_DefinitionChunks.Length; ++index1)
            {
              // ISSUE: reference to a compiler-generated field
              ArchetypeChunk definitionChunk = this.m_DefinitionChunks[index1];
              // ISSUE: reference to a compiler-generated field
              NativeArray<CreationDefinition> nativeArray1 = definitionChunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<NetCourse> nativeArray2 = definitionChunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
              for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
              {
                CreationDefinition creationDefinition = nativeArray1[index2];
                NetCourse netCourse = nativeArray2[index2];
                if ((creationDefinition.m_Flags & CreationFlags.Permanent) == (CreationFlags) 0 && (netCourse.m_StartPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsParallel)) == CoursePosFlags.IsFirst)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ConnectedEdges.HasBuffer(netCourse.m_StartPosition.m_Entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[netCourse.m_StartPosition.m_Entity];
                    for (int index3 = 0; index3 < connectedEdge.Length; ++index3)
                    {
                      Entity edge1 = connectedEdge[index3].m_Edge;
                      // ISSUE: reference to a compiler-generated field
                      Game.Net.Edge edge2 = this.m_EdgeData[edge1];
                      // ISSUE: reference to a compiler-generated field
                      Curve curve = this.m_CurveData[edge1];
                      if (edge2.m_Start == netCourse.m_StartPosition.m_Entity)
                      {
                        // ISSUE: reference to a compiler-generated method
                        GuideLinesSystem.CheckDirection(startDir, MathUtils.StartTangent(curve.m_Bezier).xz, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                      }
                      else if (edge2.m_End == netCourse.m_StartPosition.m_Entity)
                      {
                        // ISSUE: reference to a compiler-generated method
                        GuideLinesSystem.CheckDirection(startDir, -MathUtils.EndTangent(curve.m_Bezier).xz, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                      }
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_CurveData.HasComponent(netCourse.m_StartPosition.m_Entity))
                    {
                      // ISSUE: reference to a compiler-generated field
                      float3 float3 = MathUtils.Tangent(this.m_CurveData[netCourse.m_StartPosition.m_Entity].m_Bezier, netCourse.m_StartPosition.m_SplitPosition);
                      // ISSUE: reference to a compiler-generated method
                      GuideLinesSystem.CheckDirection(startDir, float3.xz, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                      // ISSUE: reference to a compiler-generated method
                      GuideLinesSystem.CheckDirection(startDir, -float3.xz, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                    }
                  }
                }
              }
            }
            if (bestLeft > 180 && bestRight > 180)
            {
              float2 float2 = new float2();
              // ISSUE: reference to a compiler-generated field
              if (!this.m_ControlPoints[0].m_Direction.Equals(new float2()))
              {
                // ISSUE: reference to a compiler-generated field
                float2 = this.m_ControlPoints[0].m_Direction;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.HasComponent(this.m_ControlPoints[0].m_OriginalEntity))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float2 = math.forward(this.m_TransformData[this.m_ControlPoints[0].m_OriginalEntity].m_Rotation).xz;
                }
              }
              if (!float2.Equals(new float2()))
              {
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, float2, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, MathUtils.Right(float2), ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, -float2, ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
                // ISSUE: reference to a compiler-generated method
                GuideLinesSystem.CheckDirection(startDir, MathUtils.Left(float2), ref leftDir, ref rightDir, ref bestLeft, ref bestRight, ref leftDir2, ref rightDir2, ref bestLeft2, ref bestRight2);
              }
            }
            bool flag = bestRight < bestLeft;
            // ISSUE: reference to a compiler-generated field
            if (bestLeft == bestRight && this.m_AngleSides.Length > angleIndex)
            {
              // ISSUE: reference to a compiler-generated field
              flag = this.m_AngleSides[angleIndex];
            }
            if (bestLeft == 180 && bestRight == 180)
            {
              if (flag)
                bestLeft = 181;
              else
                bestRight = 181;
            }
            else
            {
              if (bestLeft2 <= 180 && bestRight2 <= 180)
              {
                if (bestLeft2 < bestRight2 || bestLeft2 == bestRight2 & flag)
                  bestRight2 = 181;
                else
                  bestLeft2 = 181;
              }
              if (bestLeft2 <= 180)
              {
                leftDir = leftDir2;
                bestLeft = bestLeft2;
              }
              else if (bestRight2 <= 180)
              {
                rightDir = rightDir2;
                bestRight = bestRight2;
              }
            }
            if (bestLeft <= 180)
            {
              Line3.Segment segment = new Line3.Segment(line2.a, line2.a);
              segment.a.xz += leftDir * math.min(x, num3);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment, num2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment, line2, -leftDir, -startDir, math.min(x, num3) * 0.5f, num2, bestLeft, false);
            }
            if (bestRight <= 180)
            {
              Line3.Segment segment = new Line3.Segment(line2.a, line2.a);
              segment.a.xz += rightDir * math.min(x, num3);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawLine(this.m_GuideLineSettingsData.m_HighPriorityColor, segment, num2);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, segment, line2, -rightDir, -startDir, math.min(x, num3) * 0.5f, num2, bestRight, true);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_AngleSides.Length > angleIndex)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_AngleSides[angleIndex] = flag;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              while (this.m_AngleSides.Length <= angleIndex)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_AngleSides.Add(in flag);
              }
            }
          }
          ++angleIndex;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Continuous && this.m_ControlPoints.Length >= 3)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint1 = this.m_ControlPoints[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint2 = this.m_ControlPoints[this.m_ControlPoints.Length - 2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint3 = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
          if ((double) math.dot(controlPoint3.m_Direction, controlPoint2.m_Direction) <= -0.0099999997764825821)
          {
            float3 startTangent = new float3(controlPoint2.m_Direction.x, 0.0f, controlPoint2.m_Direction.y);
            float3 endTangent = new float3(controlPoint3.m_Direction.x, 0.0f, controlPoint3.m_Direction.y);
            float num4 = math.dot(math.normalizesafe(controlPoint3.m_Position.xz - controlPoint1.m_Position.xz), controlPoint2.m_Direction);
            Bezier4x3 curve;
            if ((double) num4 <= -0.0099999997764825821)
            {
              float3 endPos = controlPoint3.m_Position + endTangent * num4;
              curve = NetUtils.FitCurve(controlPoint1.m_Position, startTangent, endTangent, endPos) with
              {
                d = controlPoint3.m_Position
              };
            }
            else
              curve = NetUtils.FitCurve(controlPoint1.m_Position, startTangent, endTangent, controlPoint3.m_Position);
            Line2 line2_1;
            line2_1.a = MathUtils.Position(curve, 0.5f).xz;
            line2_1.b = line2_1.a + MathUtils.Tangent(curve, 0.5f).xz;
            Line2 line2_2 = new Line2(controlPoint1.m_Position.xz, controlPoint1.m_Position.xz + controlPoint2.m_Direction);
            Line2 line2_3 = new Line2(controlPoint3.m_Position.xz, controlPoint3.m_Position.xz - controlPoint3.m_Direction);
            ControlPoint controlPoint4 = controlPoint2;
            float2 t1;
            if (MathUtils.Intersect(line2_2, line2_1, out t1))
              controlPoint2.m_Position.xz = MathUtils.Position(line2_2, t1.x);
            float2 t2;
            if (MathUtils.Intersect(line2_3, line2_1, out t2))
              controlPoint4.m_Position.xz = MathUtils.Position(line2_3, t2.x);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPoint(controlPoint1, num2, ref prevPoint);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPointLine(controlPoint1, controlPoint2, num2, num3, ref angleIndex, ref prevLine);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPoint(controlPoint2, num2, ref prevPoint);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPointLine(controlPoint2, controlPoint4, num2, num3, ref angleIndex, ref prevLine);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPoint(controlPoint4, num2, ref prevPoint);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPointLine(controlPoint4, controlPoint3, num2, num3, ref angleIndex, ref prevLine);
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPoint(controlPoint3, num2, ref prevPoint);
            return;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode != NetToolSystem.Mode.Replace && this.m_ControlPoints.Length >= 3)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint5 = this.m_ControlPoints[0];
          int num5 = 0;
          // ISSUE: reference to a compiler-generated field
          for (int index = 1; index < this.m_ControlPoints.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint6 = this.m_ControlPoints[index];
            int num6 = Mathf.RoundToInt(math.distance(controlPoint5.m_Position.xz, controlPoint6.m_Position.xz));
            num5 += math.select(0, 1, num6 > 0);
            controlPoint5 = controlPoint6;
          }
          if (num5 >= 2)
          {
            // ISSUE: reference to a compiler-generated field
            ControlPoint controlPoint7 = this.m_ControlPoints[0];
            // ISSUE: reference to a compiler-generated field
            for (int index = 1; index < this.m_ControlPoints.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              ControlPoint controlPoint8 = this.m_ControlPoints[index];
              float num7 = (float) Mathf.RoundToInt(math.distance(controlPoint7.m_Position.xz, controlPoint8.m_Position.xz) * 2f) / 2f;
              if ((double) num7 > 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeList<GuideLinesSystem.TooltipInfo> local1 = ref this.m_Tooltips;
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                GuideLinesSystem.TooltipInfo tooltipInfo = new GuideLinesSystem.TooltipInfo(GuideLinesSystem.TooltipType.Length, (controlPoint7.m_Position + controlPoint8.m_Position) * 0.5f, num7);
                ref GuideLinesSystem.TooltipInfo local2 = ref tooltipInfo;
                local1.Add(in local2);
              }
              controlPoint7 = controlPoint8;
            }
          }
        }
        int num8 = 0;
        // ISSUE: reference to a compiler-generated field
        int num9 = this.m_ControlPoints.Length - 1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Mode == NetToolSystem.Mode.Replace)
        {
          num8 = 1;
          // ISSUE: reference to a compiler-generated field
          num9 = this.m_ControlPoints.Length - 2;
        }
        for (int index = num8; index <= num9; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ControlPoint controlPoint = this.m_ControlPoints[index];
          if (index > num8)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.DrawControlPointLine(this.m_ControlPoints[index - 1], controlPoint, num2, num3, ref angleIndex, ref prevLine);
          }
          // ISSUE: reference to a compiler-generated method
          this.DrawControlPoint(controlPoint, num2, ref prevPoint);
        }
      }

      private void DrawControlPoint(ControlPoint point, float lineWidth, ref float3 prevPoint)
      {
        if ((double) math.distancesq(prevPoint, point.m_Position) > 0.0099999997764825821)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawCircle(this.m_GuideLineSettingsData.m_HighPriorityColor, point.m_Position, lineWidth * 5f);
        }
        prevPoint = point.m_Position;
      }

      private void DrawControlPointLine(
        ControlPoint point1,
        ControlPoint point2,
        float lineWidth,
        float lineLength,
        ref int angleIndex,
        ref Line3.Segment prevLine)
      {
        Line3.Segment line2 = new Line3.Segment(point1.m_Position, point2.m_Position);
        float num = math.distance(point1.m_Position.xz, point2.m_Position.xz);
        if ((double) num > (double) lineWidth * 8.0)
        {
          float3 float3 = (line2.b - line2.a) * (lineWidth * 4f / num);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(this.m_GuideLineSettingsData.m_HighPriorityColor, new Line3.Segment(line2.a + float3, line2.b - float3), lineWidth * 3f, lineWidth * 5f, lineWidth * 3f);
        }
        // ISSUE: reference to a compiler-generated method
        this.DrawAngleIndicator(prevLine, line2, lineWidth, lineLength, angleIndex++);
        prevLine = line2;
      }

      private void DrawAngleIndicator(
        Line3.Segment line1,
        Line3.Segment line2,
        float lineWidth,
        float lineLength,
        int angleIndex)
      {
        bool angleSide = true;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AngleSides.Length > angleIndex)
        {
          // ISSUE: reference to a compiler-generated field
          angleSide = this.m_AngleSides[angleIndex];
        }
        float x = math.distance(line1.a.xz, line1.b.xz);
        float y = math.distance(line2.a.xz, line2.b.xz);
        if ((double) x > (double) lineWidth * 7.0 && (double) y > (double) lineWidth * 7.0)
        {
          float2 float2_1 = (line1.b.xz - line1.a.xz) / x;
          float2 float2_2 = (line2.a.xz - line2.b.xz) / y;
          float size = math.min(lineLength, math.min(x, y)) * 0.5f;
          int angle = Mathf.RoundToInt(math.degrees(math.acos(math.clamp(math.dot(float2_1, float2_2), -1f, 1f))));
          if (angle < 180)
            angleSide = (double) math.dot(MathUtils.Right(float2_1), float2_2) < 0.0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          GuideLinesSystem.DrawAngleIndicator(this.m_OverlayBuffer, this.m_Tooltips, this.m_GuideLineSettingsData, line1, line2, float2_1, float2_2, size, lineWidth, angle, angleSide);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AngleSides.Length > angleIndex)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AngleSides[angleIndex] = angleSide;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          while (this.m_AngleSides.Length <= angleIndex)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AngleSides.Add(in angleSide);
          }
        }
      }

      private struct SnapDir
      {
        public float2 m_Direction;
        public float2 m_Height;
        public float2 m_Factor;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> __Game_Tools_NetCourse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeConnectionData> __Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferTypeHandle<WaypointDefinition> __Game_Routes_WaypointDefinition_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Route> __Game_Routes_Route_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Zoning> __Game_Tools_Zoning_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LotData> __Game_Prefabs_LotData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<ObjectDefinition> __Game_Tools_ObjectDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> __Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Simulation.WaterSourceData> __Game_Simulation_WaterSourceData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<GuideLineSettingsData> __Game_Prefabs_GuideLineSettingsData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<WaterSourceColorElement> __Game_Prefabs_WaterSourceColorElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_NetCourse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCourse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup = state.GetComponentLookup<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup = state.GetComponentLookup<WaterPipeConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaypointDefinition_RO_BufferTypeHandle = state.GetBufferTypeHandle<WaypointDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Route_RO_ComponentLookup = state.GetComponentLookup<Route>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Zoning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Zoning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LotData_RO_ComponentLookup = state.GetComponentLookup<LotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_ObjectDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup = state.GetComponentLookup<ServiceUpgradeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Simulation.WaterSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GuideLineSettingsData_RO_ComponentLookup = state.GetComponentLookup<GuideLineSettingsData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterSourceColorElement_RO_BufferLookup = state.GetBufferLookup<WaterSourceColorElement>(true);
      }
    }
  }
}
