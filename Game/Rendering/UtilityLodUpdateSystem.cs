// Decompiled with JetBrains decompiler
// Type: Game.Rendering.UtilityLodUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class UtilityLodUpdateSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private UndergroundViewSystem m_UndergroundViewSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private RenderingSystem m_RenderingSystem;
    private PreCullingSystem m_PreCullingSystem;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_TreeUpdateQuery;
    private EntityQuery m_CullingUpdateQuery;
    private EntityQuery m_BatchUpdateQuery;
    private bool m_Loaded;
    private UtilityLodUpdateSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UndergroundViewSystem = this.World.GetOrCreateSystemManaged<UndergroundViewSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TreeUpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.UtilityLane>(), ComponentType.ReadOnly<LaneGeometry>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CullingUpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.UtilityLane>(), ComponentType.ReadOnly<CullingInfo>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_BatchUpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.UtilityLane>(), ComponentType.ReadOnly<MeshBatch>(), ComponentType.Exclude<Updated>(), ComponentType.Exclude<BatchesUpdated>(), ComponentType.Exclude<Deleted>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      UtilityTypes utilityTypes = this.m_UndergroundViewSystem.utilityTypes;
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.AddComponent<BatchesUpdated>(this.m_BatchUpdateQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Game.Net.SearchSystem.UpdateLaneSearchTreeJob jobData1 = new Game.Net.SearchSystem.UpdateLaneSearchTreeJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_OverriddenType = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Loaded = false,
        m_DilatedUtilityTypes = utilityTypes,
        m_SearchTree = this.m_NetSearchSystem.GetLaneSearchTree(false, out dependencies1)
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PreCullingSystem.InitializeCullingJob jobData2 = new PreCullingSystem.InitializeCullingJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UpdatedType = this.__TypeHandle.__Game_Common_Updated_RO_ComponentTypeHandle,
        m_BatchesUpdatedType = this.__TypeHandle.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle,
        m_OverriddenType = this.__TypeHandle.__Game_Common_Overridden_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_StackType = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentTypeHandle,
        m_ObjectMarkerType = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle,
        m_EdgeGeometryType = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle,
        m_StartNodeGeometryType = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle,
        m_EndNodeGeometryType = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_UtilityLaneType = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentTypeHandle,
        m_NetMarkerType = this.__TypeHandle.__Game_Net_Marker_RO_ComponentTypeHandle,
        m_ZoneBlockType = this.__TypeHandle.__Game_Zones_Block_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabLaneGeometryData = this.__TypeHandle.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabCompositionMeshRef = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup,
        m_PrefabCompositionMeshData = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_UpdateAll = true,
        m_UnspawnedVisible = this.m_RenderingSystem.unspawnedVisible,
        m_DilatedUtilityTypes = utilityTypes,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_CullingData = this.m_PreCullingSystem.GetCullingData(false, out dependencies2)
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.Schedule<Game.Net.SearchSystem.UpdateLaneSearchTreeJob>(this.m_TreeUpdateQuery, JobHandle.CombineDependencies(this.Dependency, dependencies1));
      // ISSUE: reference to a compiler-generated field
      EntityQuery cullingUpdateQuery = this.m_CullingUpdateQuery;
      JobHandle dependsOn = JobHandle.CombineDependencies(this.Dependency, dependencies2);
      JobHandle jobHandle2 = jobData2.ScheduleParallel<PreCullingSystem.InitializeCullingJob>(cullingUpdateQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataWriter(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle2);
      this.Dependency = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.ResetCulling();
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

    [Preserve]
    public UtilityLodUpdateSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Overridden> __Game_Common_Overridden_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> __Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Updated> __Game_Common_Updated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatchesUpdated> __Game_Common_BatchesUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stack> __Game_Objects_Stack_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Marker> __Game_Objects_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Marker> __Game_Net_Marker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Zones.Block> __Game_Zones_Block_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RO_BufferTypeHandle;
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> __Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Overridden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Overridden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetLaneGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_BatchesUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatchesUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Marker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshData>(true);
      }
    }
  }
}
