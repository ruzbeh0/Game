// Decompiled with JetBrains decompiler
// Type: Game.Net.GeometrySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class GeometrySystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_UpdatedEdgesQuery;
    private EntityQuery m_UpdatedNodesQuery;
    private EntityQuery m_AllEdgesQuery;
    private EntityQuery m_AllNodesQuery;
    private bool m_Loaded;
    private GeometrySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedEdgesQuery = this.GetEntityQuery(ComponentType.ReadWrite<EdgeGeometry>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedNodesQuery = this.GetEntityQuery(ComponentType.ReadWrite<NodeGeometry>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllEdgesQuery = this.GetEntityQuery(ComponentType.ReadWrite<EdgeGeometry>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllNodesQuery = this.GetEntityQuery(ComponentType.ReadWrite<NodeGeometry>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = loaded ? this.m_AllEdgesQuery : this.m_UpdatedEdgesQuery;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = loaded ? this.m_AllNodesQuery : this.m_UpdatedNodesQuery;
      if (entityQuery.IsEmptyIgnoreFilter && query.IsEmptyIgnoreFilter)
        return;
      JobHandle outJobHandle;
      NativeList<Entity> entityListAsync = entityQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      NativeList<GeometrySystem.IntersectionData> nativeList = new NativeList<GeometrySystem.IntersectionData>(0, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashMap<int2, float4> nativeParallelHashMap = new NativeParallelHashMap<int2, float4>(0, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      Bounds3 bounds = TerrainUtils.GetBounds(ref heightData);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GeometrySystem.AllocateBuffersJob jobData1 = new GeometrySystem.AllocateBuffersJob()
      {
        m_Entities = entityListAsync,
        m_IntersectionData = nativeList,
        m_EdgeHeightMap = nativeParallelHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.InitializeNodeGeometryJob jobData2 = new GeometrySystem.InitializeNodeGeometryJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RW_ComponentTypeHandle,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_EdgeDataFromEntity = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CompositionDataFromEntity = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_CurveDataFromEntity = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_Loaded = loaded
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.CalculateEdgeGeometryJob jobData3 = new GeometrySystem.CalculateEdgeGeometryJob()
      {
        m_Entities = entityListAsync.AsDeferredJobArray(),
        m_TerrainBounds = bounds,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_EdgeDataFromEntity = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeDataFromEntity = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_CurveDataFromEntity = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_CompositionDataFromEntity = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_PrefabCompositionLanes = this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup,
        m_PrefabCompositionCrosswalks = this.__TypeHandle.__Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RW_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RW_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.FlattenNodeGeometryJob jobData4 = new GeometrySystem.FlattenNodeGeometryJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_EdgeDataFromEntity = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_EdgeHeightMap = nativeParallelHashMap.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.FinishEdgeGeometryJob jobData5 = new GeometrySystem.FinishEdgeGeometryJob()
      {
        m_Entities = entityListAsync.AsDeferredJobArray(),
        m_TerrainHeightData = heightData,
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CompositionDataFromEntity = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RW_ComponentLookup,
        m_EdgeHeightMap = nativeParallelHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.CalculateNodeGeometryJob jobData6 = new GeometrySystem.CalculateNodeGeometryJob()
      {
        m_Entities = entityListAsync.AsDeferredJobArray(),
        m_PrefabRefDataFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_NodeDataFromEntity = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeDataFromEntity = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CompositionDataFromEntity = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_GeometryDataFromEntity = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RW_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.CalculateIntersectionGeometryJob jobData7 = new GeometrySystem.CalculateIntersectionGeometryJob()
      {
        m_Entities = entityListAsync.AsDeferredJobArray(),
        m_TerrainHeightData = heightData,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_NodeDataFromEntity = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeDataFromEntity = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_StartGeometryDataFromEntity = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndGeometryDataFromEntity = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_BufferedData = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GeometrySystem.CopyNodeGeometryJob jobData8 = new GeometrySystem.CopyNodeGeometryJob()
      {
        m_Entities = entityListAsync.AsDeferredJobArray(),
        m_BufferedData = nativeList,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RW_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      GeometrySystem.UpdateNodeGeometryJob jobData9 = new GeometrySystem.UpdateNodeGeometryJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TerrainHeightData = heightData,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_OrphanType = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_NodeGeometryType = this.__TypeHandle.__Game_Net_NodeGeometry_RW_ComponentTypeHandle
      };
      JobHandle job1 = jobData1.Schedule<GeometrySystem.AllocateBuffersJob>(outJobHandle);
      JobHandle job0_1 = jobData2.ScheduleParallel<GeometrySystem.InitializeNodeGeometryJob>(query, this.Dependency);
      JobHandle job0_2 = jobData3.Schedule<GeometrySystem.CalculateEdgeGeometryJob, Entity>(entityListAsync, 1, JobHandle.CombineDependencies(job0_1, outJobHandle));
      JobHandle jobHandle1 = jobData4.ScheduleParallel<GeometrySystem.FlattenNodeGeometryJob>(query, JobHandle.CombineDependencies(job0_2, job1));
      NativeList<Entity> list = entityListAsync;
      JobHandle dependsOn1 = jobHandle1;
      JobHandle inputDeps = jobData5.Schedule<GeometrySystem.FinishEdgeGeometryJob, Entity>(list, 1, dependsOn1);
      JobHandle dependsOn2 = inputDeps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (jobData6.m_IterationIndex = 0; jobData6.m_IterationIndex < 2; ++jobData6.m_IterationIndex)
        dependsOn2 = jobData6.Schedule<GeometrySystem.CalculateNodeGeometryJob, Entity>(entityListAsync, 1, dependsOn2);
      JobHandle dependsOn3 = jobData7.Schedule<GeometrySystem.CalculateIntersectionGeometryJob, Entity>(entityListAsync, 1, dependsOn2);
      JobHandle jobHandle2 = jobData8.Schedule<GeometrySystem.CopyNodeGeometryJob, Entity>(entityListAsync, 16, dependsOn3);
      JobHandle handle = jobData9.ScheduleParallel<GeometrySystem.UpdateNodeGeometryJob>(query, jobHandle2);
      entityListAsync.Dispose(jobHandle2);
      nativeList.Dispose(jobHandle2);
      nativeParallelHashMap.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(handle);
      this.Dependency = handle;
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
    public GeometrySystem()
    {
    }

    [BurstCompile]
    private struct InitializeNodeGeometryJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<NodeGeometry> m_NodeGeometryType;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public bool m_Loaded;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Node> nativeArray2 = chunk.GetNativeArray<Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NodeGeometry> nativeArray3 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity node1 = nativeArray1[index];
          Node node2 = nativeArray2[index];
          NodeGeometry nodeGeometry = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData1 = this.m_PrefabGeometryData[nativeArray4[index].m_Prefab];
          float2 float2_1 = (float2) 0.0f;
          int num1 = 0;
          float2 float2_2 = (float2) 0.0f;
          int num2 = 0;
          bool flag2 = false;
          bool flag3 = false;
          bool c = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node1, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData);
          EdgeIteratorValue edgeIteratorValue;
          while (edgeIterator.GetNext(out edgeIteratorValue))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData2 = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[edgeIteratorValue.m_Edge].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Loaded && (!flag1 || this.m_TempData.HasComponent(edgeIteratorValue.m_Edge)))
            {
              // ISSUE: reference to a compiler-generated field
              c |= !this.m_UpdatedData.HasComponent(edgeIteratorValue.m_Edge);
            }
            if ((netGeometryData1.m_MergeLayers & netGeometryData2.m_MergeLayers) != Layer.None)
            {
              // ISSUE: reference to a compiler-generated field
              Composition composition = this.m_CompositionDataFromEntity[edgeIteratorValue.m_Edge];
              // ISSUE: reference to a compiler-generated field
              NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[edgeIteratorValue.m_End ? composition.m_EndNode : composition.m_StartNode];
              flag2 |= (netCompositionData1.m_Flags.m_General & CompositionFlags.General.Roundabout) > (CompositionFlags.General) 0;
              flag3 |= (netCompositionData1.m_Flags.m_General & CompositionFlags.General.LevelCrossing) > (CompositionFlags.General) 0;
              bool flag4 = false;
              if ((netGeometryData2.m_Flags & GeometryFlags.SmoothElevation) == (GeometryFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[composition.m_Edge];
                // ISSUE: reference to a compiler-generated field
                flag4 = (((netCompositionData2.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) == (CompositionFlags.General) 0 ? 1 : 0) & ((netCompositionData2.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0 ? 1 : ((netCompositionData2.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0 ? 1 : 0))) != 0 & !this.m_OwnerData.HasComponent(edgeIteratorValue.m_Edge);
                if (!flag4)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveDataFromEntity[edgeIteratorValue.m_Edge];
              if (edgeIteratorValue.m_End)
                curve.m_Bezier = MathUtils.Invert(curve.m_Bezier);
              float num3 = math.distance(curve.m_Bezier.b.xz, curve.m_Bezier.a.xz);
              if ((double) num3 >= 0.10000000149011612)
              {
                if (flag4)
                {
                  float2_2 += new float2(curve.m_Bezier.b.y, 1f) / num3;
                  ++num2;
                }
                else
                {
                  float2_1 += new float2(curve.m_Bezier.b.y, 1f) / num3;
                  ++num1;
                }
              }
            }
          }
          if (flag2 | flag3)
          {
            nodeGeometry.m_Position = node2.m_Position.y;
            nodeGeometry.m_Flatness = 1f;
            nodeGeometry.m_Offset = 0.0f;
          }
          else if (num1 >= 2)
          {
            nodeGeometry.m_Position = math.lerp(node2.m_Position.y, float2_1.x / float2_1.y, 0.5f);
            nodeGeometry.m_Flatness = 0.0f;
            nodeGeometry.m_Offset = 0.0f;
          }
          else if (num2 >= 2)
          {
            nodeGeometry.m_Position = node2.m_Position.y;
            nodeGeometry.m_Flatness = 0.0f;
            nodeGeometry.m_Offset = node2.m_Position.y - math.lerp(node2.m_Position.y, float2_2.x / float2_2.y, 0.5f);
          }
          else
          {
            nodeGeometry.m_Position = node2.m_Position.y;
            nodeGeometry.m_Flatness = 0.0f;
            nodeGeometry.m_Offset = 0.0f;
          }
          nodeGeometry.m_Bounds.min.x = math.select(0.0f, 1f, c);
          nativeArray3[index] = nodeGeometry;
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
    private struct CalculateEdgeGeometryJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public Bounds3 m_TerrainBounds;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionDataFromEntity;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> m_PrefabCompositionLanes;
      [ReadOnly]
      public BufferLookup<NetCompositionCrosswalk> m_PrefabCompositionCrosswalks;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [NativeDisableParallelForRestriction]
      [WriteOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [NativeDisableParallelForRestriction]
      [WriteOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        Edge edge = this.m_EdgeDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_CompositionDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[entity].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData edgeCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData nodeCompositionData1 = this.m_PrefabCompositionData[composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData nodeCompositionData2 = this.m_PrefabCompositionData[composition.m_EndNode];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionLane> prefabCompositionLane = this.m_PrefabCompositionLanes[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionCrosswalk> compositionCrosswalk1 = this.m_PrefabCompositionCrosswalks[composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<NetCompositionCrosswalk> compositionCrosswalk2 = this.m_PrefabCompositionCrosswalks[composition.m_EndNode];
        EdgeGeometry edgeGeometry1 = new EdgeGeometry();
        StartNodeGeometry startNodeGeometry = new StartNodeGeometry();
        EndNodeGeometry endNodeGeometry = new EndNodeGeometry();
        float roundaboutSize1 = 0.0f;
        float roundaboutSize2 = 0.0f;
        // ISSUE: reference to a compiler-generated method
        this.CutCurve(edge, ref curve);
        float middleTangentPos = NetUtils.FindMiddleTangentPos(curve.m_Bezier.xz, new float2(0.0f, 1f));
        Bezier4x3 output1;
        Bezier4x3 output2;
        MathUtils.Divide(curve.m_Bezier, out output1, out output2, middleTangentPos);
        // ISSUE: reference to a compiler-generated field
        NodeGeometry nodeGeometry1 = this.m_NodeGeometryData[edge.m_Start];
        // ISSUE: reference to a compiler-generated field
        NodeGeometry nodeGeometry2 = this.m_NodeGeometryData[edge.m_End];
        if ((nodeCompositionData1.m_Flags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
        {
          roundaboutSize1 = nodeCompositionData1.m_RoundaboutSize.x;
          // ISSUE: reference to a compiler-generated method
          startNodeGeometry.m_Geometry.m_MiddleRadius = this.CalculateMiddleRadius(edge.m_Start, netGeometryData);
        }
        else if ((nodeCompositionData1.m_Flags.m_General & CompositionFlags.General.LevelCrossing) == (CompositionFlags.General) 0)
        {
          curve.m_Bezier.a.y = nodeGeometry1.m_Position;
          output1.a.y = curve.m_Bezier.a.y;
          output1.b.y += nodeGeometry1.m_Offset;
          output1.c.y -= nodeGeometry1.m_Offset * 0.375f;
          output1.d.y -= nodeGeometry1.m_Offset * (3f / 16f);
          output2.a.y -= nodeGeometry1.m_Offset * (3f / 16f);
          output2.b.y += nodeGeometry1.m_Offset * 0.125f;
        }
        if ((nodeCompositionData2.m_Flags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0)
        {
          roundaboutSize2 = nodeCompositionData2.m_RoundaboutSize.y;
          // ISSUE: reference to a compiler-generated method
          endNodeGeometry.m_Geometry.m_MiddleRadius = this.CalculateMiddleRadius(edge.m_End, netGeometryData);
        }
        else if ((nodeCompositionData2.m_Flags.m_General & CompositionFlags.General.LevelCrossing) == (CompositionFlags.General) 0)
        {
          curve.m_Bezier.d.y = nodeGeometry2.m_Position;
          output2.d.y = curve.m_Bezier.d.y;
          output2.c.y += nodeGeometry2.m_Offset;
          output2.b.y -= nodeGeometry2.m_Offset * 0.375f;
          output2.a.y -= nodeGeometry2.m_Offset * (3f / 16f);
          output1.d.y -= nodeGeometry2.m_Offset * (3f / 16f);
          output1.c.y += nodeGeometry2.m_Offset * 0.125f;
        }
        float num1 = math.distance(output1.c.xz, output1.d.xz);
        float num2 = math.distance(output2.b.xz, output2.a.xz);
        float num3 = math.lerp(output1.c.y, output2.b.y, num1 / math.max(0.1f, num1 + num2)) - output1.d.y;
        output1.c.y -= num3 * 0.4f;
        output1.d.y += num3 * 0.6f;
        output2.a.y += num3 * 0.6f;
        output2.b.y -= num3 * 0.4f;
        float2 float2_1 = edgeCompositionData.m_Width * new float2(0.5f, -0.5f) + edgeCompositionData.m_MiddleOffset;
        Bezier4x3 leftStartCurve = NetUtils.OffsetCurveLeftSmooth(output1, (float2) float2_1.x);
        Bezier4x3 rightStartCurve = NetUtils.OffsetCurveLeftSmooth(output1, (float2) float2_1.y);
        Bezier4x3 leftEndCurve = NetUtils.OffsetCurveLeftSmooth(output2, (float2) float2_1.x);
        Bezier4x3 rightEndCurve = NetUtils.OffsetCurveLeftSmooth(output2, (float2) float2_1.y);
        if ((edgeCompositionData.m_State & CompositionState.Airspace) != (CompositionState) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.OffsetAirspaceCurves(edge, ref leftStartCurve, ref rightStartCurve, ref leftEndCurve, ref rightEndCurve);
        }
        float3 float3_1 = MathUtils.StartTangent(output1);
        float3 float3_2 = MathUtils.EndTangent(output2);
        float3 float3_3 = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 float3_4 = MathUtils.Normalize(float3_2, float3_2.xz);
        float3_3.y = math.clamp(float3_3.y, -1f, 1f);
        float3_4.y = math.clamp(float3_4.y, -1f, 1f);
        float y = float3_3.y;
        float slopeSteepness = -float3_4.y;
        float2 float2_2 = nodeCompositionData1.m_Width * new float2(0.5f, -0.5f) + nodeCompositionData1.m_MiddleOffset;
        float2 float2_3 = nodeCompositionData2.m_Width * new float2(0.5f, -0.5f) + nodeCompositionData2.m_MiddleOffset;
        bool2 useEdgeWidth1 = new bool2((double) float2_1.x - (double) float2_2.x > 1.0 / 1000.0, (double) float2_2.y - (double) float2_1.y > 1.0 / 1000.0);
        bool2 useEdgeWidth2 = new bool2((double) float2_1.x - (double) float2_3.x > 1.0 / 1000.0, (double) float2_3.y - (double) float2_1.y > 1.0 / 1000.0);
        float3 leftTarget1;
        float3 rightTarget1;
        // ISSUE: reference to a compiler-generated method
        float2 x1 = this.CalculateCornerOffset(entity, edge.m_Start, y, useEdgeWidth1, curve, leftStartCurve, rightStartCurve, leftEndCurve, rightEndCurve, netGeometryData, edgeCompositionData, nodeCompositionData1, prefabCompositionLane, compositionCrosswalk1, startNodeGeometry.m_Geometry.m_MiddleRadius, roundaboutSize1, nodeGeometry1.m_Offset, false, out leftTarget1, out rightTarget1);
        float3 leftTarget2;
        float3 rightTarget2;
        // ISSUE: reference to a compiler-generated method
        float2 x2 = this.CalculateCornerOffset(entity, edge.m_End, slopeSteepness, useEdgeWidth2, curve, MathUtils.Invert(rightEndCurve), MathUtils.Invert(leftEndCurve), MathUtils.Invert(rightStartCurve), MathUtils.Invert(leftStartCurve), netGeometryData, edgeCompositionData, nodeCompositionData2, prefabCompositionLane, compositionCrosswalk2, endNodeGeometry.m_Geometry.m_MiddleRadius, roundaboutSize2, nodeGeometry2.m_Offset, true, out leftTarget2, out rightTarget2).yx;
        if (math.any(math.abs(float2_2 - float2_1) > 1f / 1000f) || math.any(math.abs(float2_3 - float2_1) > 1f / 1000f))
        {
          Bezier4x3 bezier4x3_1 = NetUtils.OffsetCurveLeftSmooth(output1, (float2) float2_2.x);
          Bezier4x3 bezier4x3_2 = NetUtils.OffsetCurveLeftSmooth(output1, (float2) float2_2.y);
          Bezier4x3 bezier4x3_3 = NetUtils.OffsetCurveLeftSmooth(output2, (float2) float2_3.x);
          Bezier4x3 bezier4x3_4 = NetUtils.OffsetCurveLeftSmooth(output2, (float2) float2_3.y);
          float3 float3_5;
          float3 float3_6;
          // ISSUE: reference to a compiler-generated method
          x1 = math.max(x1, this.CalculateCornerOffset(entity, edge.m_Start, y, useEdgeWidth1, curve, bezier4x3_1, bezier4x3_2, bezier4x3_3, bezier4x3_4, netGeometryData, edgeCompositionData, nodeCompositionData1, prefabCompositionLane, compositionCrosswalk1, 0.0f, 0.0f, nodeGeometry1.m_Offset, false, out float3_5, out float3_6));
          // ISSUE: reference to a compiler-generated method
          x2 = math.max(x2, this.CalculateCornerOffset(entity, edge.m_End, slopeSteepness, useEdgeWidth2, curve, MathUtils.Invert(bezier4x3_4), MathUtils.Invert(bezier4x3_3), MathUtils.Invert(bezier4x3_2), MathUtils.Invert(bezier4x3_1), netGeometryData, edgeCompositionData, nodeCompositionData2, prefabCompositionLane, compositionCrosswalk2, 0.0f, 0.0f, nodeGeometry2.m_Offset, true, out float3_6, out float3_5).yx);
        }
        int num4 = !math.all(x1 == 0.0f) ? 0 : (!leftTarget1.Equals(new float3()) ? 1 : (!rightTarget1.Equals(new float3()) ? 1 : 0));
        bool flag = math.all(x2 == 0.0f) && (!rightTarget2.Equals(new float3()) || !leftTarget2.Equals(new float3()));
        if (num4 != 0)
        {
          leftStartCurve.a = math.lerp(leftStartCurve.a, leftTarget1, 0.5f);
          rightStartCurve.a = math.lerp(rightStartCurve.a, rightTarget1, 0.5f);
        }
        if (flag)
        {
          leftEndCurve.d = math.lerp(leftEndCurve.d, rightTarget2, 0.5f);
          rightEndCurve.d = math.lerp(rightEndCurve.d, leftTarget2, 0.5f);
        }
        float2 float2_4 = math.min(x1, (float2) 1.98f);
        float2 float2_5 = math.min(x2, (float2) 1.98f);
        float2 float2_6 = float2_4 + float2_5;
        if ((double) float2_6.x > 1.9800000190734863)
        {
          float2_4.x *= 1.98f / float2_6.x;
          float2_5.x *= 1.98f / float2_6.x;
        }
        if ((double) float2_6.y > 1.9800000190734863)
        {
          float2_4.y *= 1.98f / float2_6.y;
          float2_5.y *= 1.98f / float2_6.y;
        }
        if ((netGeometryData.m_Flags & GeometryFlags.SymmetricalEdges) != (GeometryFlags) 0)
        {
          edgeGeometry1.m_Start.m_Left = leftStartCurve;
          edgeGeometry1.m_Start.m_Right = rightStartCurve;
          edgeGeometry1.m_End.m_Left = leftEndCurve;
          edgeGeometry1.m_End.m_Right = rightEndCurve;
          // ISSUE: reference to a compiler-generated method
          this.ConformLengths(ref edgeGeometry1.m_Start.m_Left, ref edgeGeometry1.m_End.m_Left, float2_4.x, float2_5.x);
          // ISSUE: reference to a compiler-generated method
          this.ConformLengths(ref edgeGeometry1.m_Start.m_Right, ref edgeGeometry1.m_End.m_Right, float2_4.y, float2_5.y);
          // ISSUE: reference to a compiler-generated method
          this.LimitHeightDelta(ref edgeGeometry1.m_Start.m_Left, ref edgeGeometry1.m_Start.m_Right, y, leftStartCurve, rightStartCurve, netGeometryData, edgeCompositionData, nodeCompositionData1, nodeGeometry1.m_Flatness);
          leftEndCurve = MathUtils.Invert(leftEndCurve);
          rightEndCurve = MathUtils.Invert(rightEndCurve);
          edgeGeometry1.m_End.m_Left = MathUtils.Invert(edgeGeometry1.m_End.m_Left);
          edgeGeometry1.m_End.m_Right = MathUtils.Invert(edgeGeometry1.m_End.m_Right);
          // ISSUE: reference to a compiler-generated method
          this.LimitHeightDelta(ref edgeGeometry1.m_End.m_Left, ref edgeGeometry1.m_End.m_Right, slopeSteepness, leftEndCurve, rightEndCurve, netGeometryData, edgeCompositionData, nodeCompositionData2, nodeGeometry2.m_Flatness);
          edgeGeometry1.m_End.m_Left = MathUtils.Invert(edgeGeometry1.m_End.m_Left);
          edgeGeometry1.m_End.m_Right = MathUtils.Invert(edgeGeometry1.m_End.m_Right);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          float cutOffset1 = this.CalculateCutOffset(leftStartCurve, leftEndCurve, float2_4.x, float2_5.x, edgeCompositionData.m_Width);
          // ISSUE: reference to a compiler-generated method
          float cutOffset2 = this.CalculateCutOffset(rightStartCurve, rightEndCurve, float2_4.y, float2_5.y, edgeCompositionData.m_Width);
          // ISSUE: reference to a compiler-generated method
          edgeGeometry1.m_Start.m_Left = this.Cut(leftStartCurve, leftEndCurve, float2_4.x, float2_5.x, cutOffset1);
          // ISSUE: reference to a compiler-generated method
          edgeGeometry1.m_Start.m_Right = this.Cut(rightStartCurve, rightEndCurve, float2_4.y, float2_5.y, cutOffset2);
          // ISSUE: reference to a compiler-generated method
          this.LimitHeightDelta(ref edgeGeometry1.m_Start.m_Left, ref edgeGeometry1.m_Start.m_Right, y, leftStartCurve, rightStartCurve, netGeometryData, edgeCompositionData, nodeCompositionData1, nodeGeometry1.m_Flatness);
          Bezier4x3 end1 = MathUtils.Invert(leftStartCurve);
          Bezier4x3 end2 = MathUtils.Invert(rightStartCurve);
          leftEndCurve = MathUtils.Invert(leftEndCurve);
          rightEndCurve = MathUtils.Invert(rightEndCurve);
          // ISSUE: reference to a compiler-generated method
          edgeGeometry1.m_End.m_Left = this.Cut(leftEndCurve, end1, float2_5.x, float2_4.x, cutOffset1);
          // ISSUE: reference to a compiler-generated method
          edgeGeometry1.m_End.m_Right = this.Cut(rightEndCurve, end2, float2_5.y, float2_4.y, cutOffset2);
          // ISSUE: reference to a compiler-generated method
          this.LimitHeightDelta(ref edgeGeometry1.m_End.m_Left, ref edgeGeometry1.m_End.m_Right, slopeSteepness, leftEndCurve, rightEndCurve, netGeometryData, edgeCompositionData, nodeCompositionData2, nodeGeometry2.m_Flatness);
          edgeGeometry1.m_End.m_Left = MathUtils.Invert(edgeGeometry1.m_End.m_Left);
          edgeGeometry1.m_End.m_Right = MathUtils.Invert(edgeGeometry1.m_End.m_Right);
        }
        if ((double) nodeGeometry1.m_Bounds.min.x != 0.0 || (double) nodeGeometry2.m_Bounds.min.x != 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry2 = this.m_EdgeGeometryData[entity];
          if ((double) nodeGeometry1.m_Bounds.min.x != 0.0)
          {
            edgeGeometry1.m_Start.m_Left.b.y += edgeGeometry2.m_Start.m_Left.a.y - edgeGeometry1.m_Start.m_Left.a.y;
            edgeGeometry1.m_Start.m_Right.b.y += edgeGeometry2.m_Start.m_Right.a.y - edgeGeometry1.m_Start.m_Right.a.y;
            edgeGeometry1.m_Start.m_Left.a.y = edgeGeometry2.m_Start.m_Left.a.y;
            edgeGeometry1.m_Start.m_Right.a.y = edgeGeometry2.m_Start.m_Right.a.y;
          }
          if ((double) nodeGeometry2.m_Bounds.min.x != 0.0)
          {
            edgeGeometry1.m_End.m_Left.c.y += edgeGeometry2.m_End.m_Left.d.y - edgeGeometry1.m_End.m_Left.d.y;
            edgeGeometry1.m_End.m_Right.c.y += edgeGeometry2.m_End.m_Right.d.y - edgeGeometry1.m_End.m_Right.d.y;
            edgeGeometry1.m_End.m_Left.d.y = edgeGeometry2.m_End.m_Left.d.y;
            edgeGeometry1.m_End.m_Right.d.y = edgeGeometry2.m_End.m_Right.d.y;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometryData[entity] = edgeGeometry1;
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometryData[entity] = startNodeGeometry;
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometryData[entity] = endNodeGeometry;
      }

      private void OffsetAirspaceCurves(
        Edge edge,
        ref Bezier4x3 leftStartCurve,
        ref Bezier4x3 rightStartCurve,
        ref Bezier4x3 leftEndCurve,
        ref Bezier4x3 rightEndCurve)
      {
        Bezier4x1 y1 = leftStartCurve.y;
        float4 abcd1 = y1.abcd;
        y1 = rightStartCurve.y;
        float4 abcd2 = y1.abcd;
        float4 float4_1 = (abcd1 + abcd2) * 0.5f;
        Bezier4x1 y2 = leftEndCurve.y;
        float4 abcd3 = y2.abcd;
        y2 = rightEndCurve.y;
        float4 abcd4 = y2.abcd;
        float4 float4_2 = (abcd3 + abcd4) * 0.5f;
        float2 float2_1 = (float2) 0.0f;
        float2 float2_2 = new float2(float4_1.x, float4_2.w);
        Elevation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.TryGetComponent(edge.m_Start, out componentData1))
          float2_1.x = math.csum(componentData1.m_Elevation) * 0.5f;
        Elevation componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.TryGetComponent(edge.m_End, out componentData2))
          float2_1.y = math.csum(componentData2.m_Elevation) * 0.5f;
        float4 float4_3 = math.lerp((float4) float2_1.x, (float4) float2_1.y, math.saturate((float4_1 - float2_2.x) / (float2_2.y - float2_2.x)));
        float4 float4_4 = math.lerp((float4) float2_1.x, (float4) float2_1.y, math.saturate((float4_2 - float2_2.x) / (float2_2.y - float2_2.x)));
        float3 float3_1 = math.normalizesafe(rightStartCurve.a - leftStartCurve.a) * float4_3.x;
        float3 float3_2 = math.normalizesafe(rightStartCurve.b - leftStartCurve.b) * float4_3.y;
        float3 float3_3 = math.normalizesafe(rightStartCurve.c - leftStartCurve.c) * float4_3.z;
        float3 float3_4 = math.normalizesafe(rightStartCurve.d - leftStartCurve.d) * float4_3.w;
        leftStartCurve.a -= float3_1;
        leftStartCurve.b -= float3_2;
        leftStartCurve.c -= float3_3;
        leftStartCurve.d -= float3_4;
        rightStartCurve.a += float3_1;
        rightStartCurve.b += float3_2;
        rightStartCurve.c += float3_3;
        rightStartCurve.d += float3_4;
        float3 float3_5 = math.normalizesafe(rightEndCurve.a - leftEndCurve.a) * float4_4.x;
        float3 float3_6 = math.normalizesafe(rightEndCurve.b - leftEndCurve.b) * float4_4.y;
        float3 float3_7 = math.normalizesafe(rightEndCurve.c - leftEndCurve.c) * float4_4.z;
        float3 float3_8 = math.normalizesafe(rightEndCurve.d - leftEndCurve.d) * float4_4.w;
        leftEndCurve.a -= float3_5;
        leftEndCurve.b -= float3_6;
        leftEndCurve.c -= float3_7;
        leftEndCurve.d -= float3_8;
        rightEndCurve.a += float3_5;
        rightEndCurve.b += float3_6;
        rightEndCurve.c += float3_7;
        rightEndCurve.d += float3_8;
      }

      private void CutCurve(Edge edge, ref Curve curve)
      {
        // ISSUE: reference to a compiler-generated field
        Node node1 = this.m_NodeDataFromEntity[edge.m_Start];
        // ISSUE: reference to a compiler-generated field
        Node node2 = this.m_NodeDataFromEntity[edge.m_End];
        float t1;
        double num1 = (double) MathUtils.Distance(curve.m_Bezier.xz, node1.m_Position.xz, out t1);
        float t2;
        double num2 = (double) MathUtils.Distance(curve.m_Bezier.xz, node2.m_Position.xz, out t2);
        if ((double) t1 < 1.0 / 1000.0)
          t1 = 0.0f;
        if ((double) t2 > 0.99900001287460327)
          t2 = 1f;
        if ((double) t1 == 0.0 && (double) t2 == 1.0)
          return;
        if ((double) t2 < (double) t1 + 0.019999999552965164)
        {
          float num3;
          t1 = math.max(0.0f, (num3 = (float) (((double) t1 + (double) t2) * 0.5)) - 0.01f);
          t2 = math.min(1f, num3 + 0.01f);
        }
        curve.m_Bezier = MathUtils.Cut(curve.m_Bezier, new float2(t1, t2));
        curve.m_Bezier.a.y = node1.m_Position.y;
        curve.m_Bezier.d.y = node2.m_Position.y;
      }

      private float CalculateMiddleRadius(Entity node, NetGeometryData netGeometryData)
      {
        float x1 = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.HasBuffer(node))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Objects.SubObject> subObject = this.m_SubObjects[node];
          for (int index = 0; index < subObject.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefDataFromEntity[subObject[index].m_SubObject];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PlaceableObjectData.HasComponent(prefabRef.m_Prefab) && this.m_ObjectGeometryData.HasComponent(prefabRef.m_Prefab) && (this.m_PlaceableObjectData[prefabRef.m_Prefab].m_Flags & Game.Objects.PlacementFlags.RoadNode) != Game.Objects.PlacementFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
              float x2 = math.cmax(objectGeometryData.m_Size.xz) * 0.5f;
              if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Standing) != Game.Objects.GeometryFlags.None)
                x2 = (double) objectGeometryData.m_LegSize.y < (double) netGeometryData.m_DefaultHeightRange.max ? math.max(x2, math.cmax(objectGeometryData.m_LegSize.xz) * 0.5f) : math.cmax(objectGeometryData.m_LegSize.xz) * 0.5f;
              x1 = math.max(x1, x2 + 1f);
            }
          }
        }
        return x1;
      }

      private void LimitHeightDelta(
        ref Bezier4x3 left,
        ref Bezier4x3 right,
        float slopeSteepness,
        Bezier4x3 originalLeft,
        Bezier4x3 originalRight,
        NetGeometryData prefabGeometryData,
        NetCompositionData edgeCompositionData,
        NetCompositionData nodeCompositionData,
        float nodeFlatness)
      {
        if ((nodeCompositionData.m_Flags.m_General & CompositionFlags.General.LevelCrossing) != (CompositionFlags.General) 0)
        {
          left.a.y = originalLeft.a.y;
          left.b.y = originalLeft.a.y;
          right.a.y = originalRight.a.y;
          right.b.y = originalRight.a.y;
        }
        else
        {
          float num1 = math.max(edgeCompositionData.m_NodeOffset, nodeCompositionData.m_NodeOffset) + (float) ((double) math.abs(slopeSteepness) * (double) edgeCompositionData.m_Width * 0.25);
          float num2 = (float) ((double) prefabGeometryData.m_MaxSlopeSteepness * (double) num1 * 1.5);
          float num3 = math.max(math.min(0.0f, originalLeft.a.y + num2 - left.a.y), originalLeft.a.y - num2 - left.a.y);
          float num4 = math.max(math.min(0.0f, originalRight.a.y + num2 - right.a.y), originalRight.a.y - num2 - right.a.y);
          left.a.y += num3 * nodeFlatness;
          left.b.y += num3 * nodeFlatness;
          right.a.y += num4 * nodeFlatness;
          right.b.y += num4 * nodeFlatness;
        }
      }

      private float CalculateCutOffset(
        Bezier4x3 start,
        Bezier4x3 end,
        float startOffset,
        float endOffset,
        float width)
      {
        float num = (double) startOffset < 1.0 ? ((double) endOffset < 1.0 ? MathUtils.Length(start.xz, new Bounds1(startOffset, 1f)) + MathUtils.Length(end.xz, new Bounds1(0.0f, 1f - endOffset)) : MathUtils.Length(start.xz, new Bounds1(startOffset, 2f - endOffset))) : MathUtils.Length(end.xz, new Bounds1(startOffset - 1f, 1f - endOffset));
        return (float) (1.0 - (2.0 - (double) startOffset - (double) endOffset) * 0.5 / ((double) num / (double) math.max(0.01f, width) + 1.0));
      }

      private Bezier4x3 Cut(
        Bezier4x3 start,
        Bezier4x3 end,
        float startOffset,
        float endOffset,
        float cutOffset)
      {
        if ((double) startOffset >= 1.0)
          return MathUtils.Cut(end, new float2(startOffset - 1f, startOffset - cutOffset));
        if ((double) startOffset <= (double) cutOffset)
          return MathUtils.Cut(start, new float2(startOffset, math.min(1f, 1f + cutOffset - endOffset)));
        float3 startPos = MathUtils.Position(start, startOffset);
        float3 float3_1 = MathUtils.Position(end, startOffset - cutOffset);
        float3 float3_2 = MathUtils.Tangent(start, startOffset);
        float3 float3_3 = MathUtils.Tangent(end, startOffset - cutOffset);
        float3_2 = MathUtils.Normalize(float3_2, float3_2.xz);
        float3_3 = MathUtils.Normalize(float3_3, float3_3.xz);
        float3_2.y = math.clamp(float3_2.y, -1f, 1f);
        float3_3.y = math.clamp(float3_3.y, -1f, 1f);
        float3 startTangent = float3_2;
        float3 endTangent = float3_3;
        float3 endPos = float3_1;
        return NetUtils.FitCurve(startPos, startTangent, endTangent, endPos);
      }

      private void ConformLengths(
        ref Bezier4x3 start,
        ref Bezier4x3 end,
        float startOffset,
        float endOffset)
      {
        if ((double) startOffset >= 1.0)
        {
          Bezier4x2 xz = end.xz;
          Bounds1 t = new Bounds1(startOffset - 1f, 1f - endOffset);
          float num = MathUtils.Length(xz, t);
          MathUtils.ClampLength(xz, ref t, num * 0.5f);
          start = MathUtils.Cut(end, t);
          end = MathUtils.Cut(end, new Bounds1(t.max, 1f - endOffset));
        }
        else if ((double) endOffset >= 1.0)
        {
          Bezier4x2 xz = start.xz;
          Bounds1 t = new Bounds1(startOffset, 2f - endOffset);
          float num = MathUtils.Length(xz, t);
          MathUtils.ClampLengthInverse(xz, ref t, num * 0.5f);
          end = MathUtils.Cut(start, t);
          start = MathUtils.Cut(start, new Bounds1(startOffset, t.min));
        }
        else
        {
          Bezier4x2 xz1 = start.xz;
          Bezier4x2 xz2 = end.xz;
          Bounds1 t1 = new Bounds1(startOffset, 1f);
          Bounds1 t2 = new Bounds1(0.0f, 1f - endOffset);
          float x = MathUtils.Length(xz1, t1);
          float y = MathUtils.Length(xz2, t2);
          float3 float3_1;
          float3 float3_2;
          if ((double) x > (double) y)
          {
            MathUtils.ClampLength(xz1, ref t1, math.lerp(x, y, 0.5f));
            float3_1 = MathUtils.Position(start, t1.max);
            float3_2 = MathUtils.Tangent(start, t1.max);
          }
          else
          {
            MathUtils.ClampLengthInverse(xz2, ref t2, math.lerp(x, y, 0.5f));
            float3_1 = MathUtils.Position(end, t2.min);
            float3_2 = MathUtils.Tangent(end, t2.min);
          }
          float3 startPos = MathUtils.Position(start, startOffset);
          float3 startTangent = MathUtils.Tangent(start, startOffset);
          float3 endPos = MathUtils.Position(end, 1f - endOffset);
          float3 float3_3 = MathUtils.Tangent(end, 1f - endOffset);
          startTangent = MathUtils.Normalize(startTangent, startTangent.xz);
          float3_2 = MathUtils.Normalize(float3_2, float3_2.xz);
          float3 endTangent = MathUtils.Normalize(float3_3, float3_3.xz);
          start = NetUtils.FitCurve(startPos, startTangent, float3_2, float3_1);
          end = NetUtils.FitCurve(float3_1, float3_2, endTangent, endPos);
        }
      }

      private float2 CalculateCornerOffset(
        Entity edge,
        Entity node,
        float slopeSteepness,
        bool2 useEdgeWidth,
        Curve curveData,
        Bezier4x3 leftStartCurve,
        Bezier4x3 rightStartCurve,
        Bezier4x3 leftEndCurve,
        Bezier4x3 rightEndCurve,
        NetGeometryData prefabGeometryData,
        NetCompositionData edgeCompositionData,
        NetCompositionData nodeCompositionData,
        DynamicBuffer<NetCompositionLane> nodeCompositionLanes,
        DynamicBuffer<NetCompositionCrosswalk> nodeCompositionCrosswalks,
        float middleRadius,
        float roundaboutSize,
        float bOffset,
        bool isEnd,
        out float3 leftTarget,
        out float3 rightTarget)
      {
        float2 x1 = new float2();
        leftTarget = new float3();
        rightTarget = new float3();
        if (isEnd)
        {
          curveData.m_Bezier = MathUtils.Invert(curveData.m_Bezier);
          edgeCompositionData.m_MiddleOffset = -edgeCompositionData.m_MiddleOffset;
        }
        float3 float3_1 = MathUtils.StartTangent(leftStartCurve);
        float2 xz1 = float3_1.xz;
        float3_1 = MathUtils.StartTangent(rightStartCurve);
        float2 xz2 = float3_1.xz;
        float2 y1 = leftStartCurve.a.xz - rightStartCurve.a.xz;
        MathUtils.TryNormalize(ref xz1);
        MathUtils.TryNormalize(ref xz2);
        MathUtils.TryNormalize(ref y1);
        float2 float2_1 = new float2();
        float2 float2_2 = new float2();
        float2 x2 = (float2) float.MinValue;
        float2 x3 = (float2) 0.0f;
        float num1 = prefabGeometryData.m_MinNodeOffset;
        bool flag1 = (double) middleRadius > 0.0;
        float num2 = math.max(edgeCompositionData.m_NodeOffset, nodeCompositionData.m_NodeOffset);
        float x4 = num2 + (float) ((double) math.abs(slopeSteepness) * (double) nodeCompositionData.m_Width * 0.25);
        bool flag2 = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge, node, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          if (edgeIteratorValue.m_Edge != edge)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[edgeIteratorValue.m_Edge].m_Prefab];
            if ((netGeometryData.m_IntersectLayers & prefabGeometryData.m_IntersectLayers) != Layer.None)
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge1 = this.m_EdgeDataFromEntity[edgeIteratorValue.m_Edge];
              // ISSUE: reference to a compiler-generated field
              Curve curve1 = this.m_CurveDataFromEntity[edgeIteratorValue.m_Edge];
              // ISSUE: reference to a compiler-generated field
              Composition composition = this.m_CompositionDataFromEntity[edgeIteratorValue.m_Edge];
              // ISSUE: reference to a compiler-generated field
              NetCompositionData edgeCompositionData2 = this.m_PrefabCompositionData[composition.m_Edge];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<NetCompositionLane> prefabCompositionLane = this.m_PrefabCompositionLanes[composition.m_Edge];
              NetCompositionData nodeCompositionData2;
              DynamicBuffer<NetCompositionCrosswalk> compositionCrosswalk;
              if (edgeIteratorValue.m_End)
              {
                // ISSUE: reference to a compiler-generated field
                nodeCompositionData2 = this.m_PrefabCompositionData[composition.m_EndNode];
                // ISSUE: reference to a compiler-generated field
                compositionCrosswalk = this.m_PrefabCompositionCrosswalks[composition.m_EndNode];
                edgeCompositionData2.m_MiddleOffset = -edgeCompositionData2.m_MiddleOffset;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                nodeCompositionData2 = this.m_PrefabCompositionData[composition.m_StartNode];
                // ISSUE: reference to a compiler-generated field
                compositionCrosswalk = this.m_PrefabCompositionCrosswalks[composition.m_StartNode];
              }
              // ISSUE: reference to a compiler-generated method
              this.CutCurve(edge1, ref curve1);
              float middleTangentPos = NetUtils.FindMiddleTangentPos(curve1.m_Bezier.xz, new float2(0.0f, 1f));
              Bezier4x3 output1;
              Bezier4x3 output2;
              MathUtils.Divide(curve1.m_Bezier, out output1, out output2, middleTangentPos);
              if (edgeIteratorValue.m_End)
              {
                curve1.m_Bezier = MathUtils.Invert(curve1.m_Bezier);
                Bezier4x3 bezier4x3 = MathUtils.Invert(output1);
                output1 = MathUtils.Invert(output2);
                output2 = bezier4x3;
              }
              curve1.m_Bezier.a.y = curveData.m_Bezier.a.y;
              output1.a.y = curveData.m_Bezier.a.y;
              output1.b.y += bOffset;
              bool flag3 = false;
              if ((netGeometryData.m_MergeLayers & prefabGeometryData.m_MergeLayers) != Layer.None)
              {
                num1 = math.max(num1, netGeometryData.m_MinNodeOffset);
                if ((double) middleRadius > 0.0)
                  roundaboutSize = !edgeIteratorValue.m_End ? math.max(roundaboutSize, nodeCompositionData2.m_RoundaboutSize.x) : math.max(roundaboutSize, nodeCompositionData2.m_RoundaboutSize.y);
                float3 float3_2 = MathUtils.StartTangent(output1);
                float3 float3_3 = MathUtils.Normalize(float3_2, float3_2.xz);
                float3_3.y = math.clamp(float3_3.y, -1f, 1f);
                float num3 = -float3_3.y;
                float2 float2_3;
                float2_3.x = math.dot(curve1.m_Bezier.a.xz - leftStartCurve.a.xz, xz1);
                float2_3.y = math.dot(curve1.m_Bezier.a.xz - rightStartCurve.a.xz, xz2);
                x2 = math.max(x2, float2_3 * 0.5f);
                float y2 = math.abs(slopeSteepness - num3) * nodeCompositionData.m_Width;
                bool dontCrossTracks = false;
                if (((edgeCompositionData.m_State | edgeCompositionData2.m_State) & (CompositionState.HasForwardTrackLanes | CompositionState.HasBackwardTrackLanes)) != (CompositionState) 0 && ((edgeCompositionData.m_State | edgeCompositionData2.m_State) & (CompositionState.HasForwardRoadLanes | CompositionState.HasBackwardRoadLanes)) == (CompositionState) 0 && (nodeCompositionData.m_Flags.m_General & CompositionFlags.General.Intersection) == (CompositionFlags.General) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  dontCrossTracks = this.GetTopOwner(edge) == this.GetTopOwner(edgeIteratorValue.m_Edge);
                }
                float4 float4_1 = (1f - nodeCompositionData.m_SyncVertexOffsetsLeft) * (nodeCompositionData.m_Width * 0.5f + nodeCompositionData.m_MiddleOffset);
                float4 float4_2 = nodeCompositionData.m_SyncVertexOffsetsRight * (nodeCompositionData.m_Width * 0.5f - nodeCompositionData.m_MiddleOffset);
                float4 float4_3 = (1f - nodeCompositionData2.m_SyncVertexOffsetsLeft.wzyx) * (nodeCompositionData2.m_Width * 0.5f + nodeCompositionData2.m_MiddleOffset);
                float4 float4_4 = nodeCompositionData2.m_SyncVertexOffsetsRight.wzyx * (nodeCompositionData2.m_Width * 0.5f - nodeCompositionData2.m_MiddleOffset);
                float2 float2_4 = edgeCompositionData.m_Width * 0.5f + new float2(edgeCompositionData.m_MiddleOffset, -edgeCompositionData.m_MiddleOffset);
                float2 float2_5 = edgeCompositionData2.m_Width * 0.5f + new float2(-edgeCompositionData2.m_MiddleOffset, edgeCompositionData2.m_MiddleOffset);
                float num4 = math.dot(curveData.m_Bezier.a.xz - curve1.m_Bezier.a.xz, y1);
                float2 float2_6 = new float2(math.cmax(math.abs(float4_1 - float4_4 + num4)), math.cmax(math.abs(float4_2 - float4_3 - num4)));
                float2 float2_7 = math.max(float2_6, math.abs(float2_4 - float2_5 + new float2(num4, -num4)));
                float num5 = math.max(0.1f, math.min(edgeCompositionData.m_Width, edgeCompositionData2.m_Width));
                // ISSUE: reference to a compiler-generated method
                float2 x5 = math.max(float2_7 * (num5 / (num5 + float2_7 * 0.75f)), (float2) this.CompareLanes(nodeCompositionLanes, prefabCompositionLane, num4, isEnd, edgeIteratorValue.m_End, dontCrossTracks));
                if ((nodeCompositionData.m_Flags.m_General & CompositionFlags.General.Crosswalk) != (CompositionFlags.General) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  x5 = math.max(x5, (float2) this.CheckCrosswalks(nodeCompositionCrosswalks));
                }
                if ((nodeCompositionData2.m_Flags.m_General & CompositionFlags.General.Crosswalk) != (CompositionFlags.General) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  x5 = math.max(x5, (float2) this.CheckCrosswalks(compositionCrosswalk));
                }
                float2_6 = x5 * math.max(0.0f, math.dot(float3_3.xz, -xz1));
                if ((double) y2 > 0.20000000298023224)
                  x3 = math.max(x3, (float2) y2);
                if (math.any(float2_6 > 0.1f))
                {
                  x3 = math.max(x3, float2_6);
                  flag3 = true;
                }
                if ((double) math.distancesq(curve1.m_Bezier.a.xz, curveData.m_Bezier.a.xz) > 0.0099999997764825821 || (double) math.dot(float3_3.xz, -xz1) < 0.99949997663497925)
                {
                  flag3 = true;
                  flag1 = true;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  if (this.RequireTransition(nodeCompositionData, nodeCompositionData2, edgeCompositionData, edgeCompositionData2))
                    flag3 = true;
                }
                if (flag3)
                {
                  nodeCompositionData2.m_Width += x4 * 2f;
                  nodeCompositionData2.m_Width += nodeCompositionData2.m_WidthOffset;
                }
                if (!flag2)
                  flag1 = true;
              }
              else
              {
                flag3 = true;
                flag1 = true;
                nodeCompositionData2.m_Width += num2 * 2f;
                nodeCompositionData2.m_Width += nodeCompositionData2.m_WidthOffset;
              }
              float2 float2_8 = nodeCompositionData2.m_Width * new float2(0.5f, -0.5f) - nodeCompositionData2.m_MiddleOffset;
              if (math.any(useEdgeWidth))
              {
                if (flag3)
                {
                  edgeCompositionData2.m_Width += (float) (((netGeometryData.m_MergeLayers & prefabGeometryData.m_MergeLayers) != Layer.None ? (double) x4 : (double) num2) * 2.0);
                  edgeCompositionData2.m_Width += edgeCompositionData2.m_WidthOffset;
                }
                float2 float2_9 = edgeCompositionData2.m_Width * new float2(0.5f, -0.5f) - edgeCompositionData2.m_MiddleOffset;
                if (useEdgeWidth.y)
                  float2_8.x = math.max(float2_8.x, float2_9.x);
                if (useEdgeWidth.x)
                  float2_8.y = math.min(float2_8.y, float2_9.y);
              }
              Bezier4x3 curve2 = NetUtils.OffsetCurveLeftSmooth(output1, (float2) float2_8.x);
              Bezier4x3 bezier4x3_1 = NetUtils.OffsetCurveLeftSmooth(output2, (float2) float2_8.x);
              Bezier4x3 curve3 = NetUtils.OffsetCurveLeftSmooth(output1, (float2) float2_8.y);
              Bezier4x3 bezier4x3_2 = NetUtils.OffsetCurveLeftSmooth(output2, (float2) float2_8.y);
              float3_1 = MathUtils.StartTangent(curve2);
              float2 float2_10 = float3_1.xz;
              float3_1 = MathUtils.StartTangent(curve3);
              float2 float2_11 = float3_1.xz;
              MathUtils.TryNormalize(ref float2_10);
              MathUtils.TryNormalize(ref float2_11);
              if (flag3)
              {
                float2 float2_12 = (float2) math.max(nodeCompositionData.m_Width, nodeCompositionData2.m_Width * 0.5f);
                float2 float2_13 = math.max((float2) x4, float2_12 * math.saturate(new float2(math.dot(xz2, float2_10), math.dot(xz1, float2_11)) + 1f));
                Bezier4x2 nodeCurve2;
                nodeCurve2.a = curve2.a.xz;
                nodeCurve2.b = curve2.a.xz - float2_10 * (float2_13.x * 1.33333337f);
                nodeCurve2.c = curve3.a.xz - float2_11 * (float2_13.y * 1.33333337f);
                nodeCurve2.d = curve3.a.xz;
                // ISSUE: reference to a compiler-generated method
                float2 float2_14 = this.Intersect(leftStartCurve.xz, leftEndCurve.xz, nodeCurve2, curve3.xz, bezier4x3_2.xz);
                // ISSUE: reference to a compiler-generated method
                float2 float2_15 = this.Intersect(rightStartCurve.xz, rightEndCurve.xz, nodeCurve2, curve2.xz, bezier4x3_1.xz);
                if ((double) float2_14.x > 0.0)
                {
                  if ((double) float2_14.x > (double) x1.x)
                    x1.x = float2_14.x;
                  // ISSUE: reference to a compiler-generated method
                  float2_11 = this.Tangent(curve3.xz, bezier4x3_2.xz, float2_14.y);
                  MathUtils.TryNormalize(ref float2_11);
                }
                if ((double) float2_15.x > 0.0)
                {
                  if ((double) float2_15.x > (double) x1.y)
                    x1.y = float2_15.x;
                  // ISSUE: reference to a compiler-generated method
                  float2_10 = this.Tangent(curve2.xz, bezier4x3_1.xz, float2_15.y);
                  MathUtils.TryNormalize(ref float2_10);
                }
              }
              else
              {
                leftTarget = curve3.a;
                rightTarget = curve2.a;
              }
              if (flag2)
                float2_1 = float2_11;
              else if ((double) math.dot(float2_11, y1) > 0.0)
              {
                if ((double) math.dot(float2_1, y1) <= 0.0 || (double) math.dot(float2_11, xz1) >= (double) math.dot(float2_1, xz1))
                  float2_1 = float2_11;
              }
              else if ((double) math.dot(float2_1, y1) <= 0.0 && (double) math.dot(float2_11, xz1) <= (double) math.dot(float2_1, xz1))
                float2_1 = float2_11;
              if (flag2)
                float2_2 = float2_10;
              else if ((double) math.dot(float2_10, y1) < 0.0)
              {
                if ((double) math.dot(float2_2, y1) >= 0.0 || (double) math.dot(float2_10, xz2) >= (double) math.dot(float2_2, xz2))
                  float2_2 = float2_10;
              }
              else if ((double) math.dot(float2_2, y1) >= 0.0 && (double) math.dot(float2_10, xz2) <= (double) math.dot(float2_2, xz2))
                float2_2 = float2_10;
              flag2 = false;
            }
          }
        }
        if (math.any(x2 > 0.1f | x3 > 0.1f) | flag1)
        {
          if (flag1)
            x3 = math.max(x3, (float2) num1);
          float2 x6 = x2 + x3;
          if ((double) middleRadius > 0.0)
          {
            float num6 = middleRadius + roundaboutSize;
            // ISSUE: reference to a compiler-generated field
            float3 position = this.m_NodeDataFromEntity[node].m_Position;
            float2 float2_16;
            float2_16.x = math.dot(position.xz - leftStartCurve.a.xz, xz1);
            float2_16.y = math.dot(position.xz - rightStartCurve.a.xz, xz2);
            x6 = math.max(x6, num6 + num2 + float2_16);
          }
          if ((double) x6.x > 0.0)
          {
            Bounds1 t = new Bounds1(0.0f, 1f);
            if (MathUtils.ClampLength(leftStartCurve, ref t, x6.x))
            {
              x1.x = math.max(x1.x, t.max);
            }
            else
            {
              t = new Bounds1(0.0f, 1f);
              x6.x = math.max(0.0f, x6.x - MathUtils.Length(leftStartCurve));
              MathUtils.ClampLength(leftEndCurve, ref t, x6.x);
              x1.x = math.max(x1.x, 1f + t.max);
            }
          }
          if ((double) x6.y > 0.0)
          {
            Bounds1 t = new Bounds1(0.0f, 1f);
            if (MathUtils.ClampLength(rightStartCurve, ref t, x6.y))
            {
              x1.y = math.max(x1.y, t.max);
            }
            else
            {
              t = new Bounds1(0.0f, 1f);
              x6.y = math.max(0.0f, x6.y - MathUtils.Length(rightStartCurve));
              MathUtils.ClampLength(rightEndCurve, ref t, x6.y);
              x1.y = math.max(x1.y, 1f + t.max);
            }
          }
          if ((prefabGeometryData.m_Flags & GeometryFlags.StraightEnds) != (GeometryFlags) 0)
            x1 = (float2) math.cmax(x1);
          else if ((double) x1.y > (double) x1.x)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckOppositeSide(leftStartCurve.xz, leftEndCurve.xz, rightStartCurve.xz, rightEndCurve.xz, float2_1, float2_2, ref x1.x, x1.y);
          }
          else if ((double) x1.y < (double) x1.x)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckOppositeSide(rightStartCurve.xz, rightEndCurve.xz, leftStartCurve.xz, leftEndCurve.xz, float2_2, float2_1, ref x1.y, x1.x);
          }
          leftTarget = new float3();
          rightTarget = new float3();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OutsideConnectionData.HasComponent(node))
        {
          float2 float2_17;
          // ISSUE: reference to a compiler-generated method
          float2_17.x = this.IntersectBounds(leftStartCurve, leftEndCurve);
          // ISSUE: reference to a compiler-generated method
          float2_17.y = this.IntersectBounds(rightStartCurve, rightEndCurve);
          if ((prefabGeometryData.m_Flags & GeometryFlags.StraightEnds) != (GeometryFlags) 0)
            float2_17 = (float2) math.cmin(float2_17);
          x1 = math.max(x1, float2_17);
          leftTarget = new float3();
          rightTarget = new float3();
        }
        return x1;
      }

      private Entity GetTopOwner(Entity entity)
      {
        Entity topOwner = Entity.Null;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out componentData1))
        {
          entity = componentData1.m_Owner;
          topOwner = entity;
          Temp componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TempData.TryGetComponent(entity, out componentData2) && componentData2.m_Original != Entity.Null)
          {
            entity = componentData2.m_Original;
            topOwner = entity;
          }
        }
        return topOwner;
      }

      private float IntersectBounds(Bezier4x3 startCurve, Bezier4x3 endCurve)
      {
        float x = 0.0f;
        float t;
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(startCurve.x, this.m_TerrainBounds.min.x, out t, 4))
          x = math.max(x, t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(startCurve.x, this.m_TerrainBounds.max.x, out t, 4))
          x = math.max(x, t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(startCurve.z, this.m_TerrainBounds.min.z, out t, 4))
          x = math.max(x, t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(startCurve.z, this.m_TerrainBounds.max.z, out t, 4))
          x = math.max(x, t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(endCurve.x, this.m_TerrainBounds.min.x, out t, 4))
          x = math.max(x, 1f + t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(endCurve.x, this.m_TerrainBounds.max.x, out t, 4))
          x = math.max(x, 1f + t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(endCurve.z, this.m_TerrainBounds.min.z, out t, 4))
          x = math.max(x, 1f + t);
        // ISSUE: reference to a compiler-generated field
        if (MathUtils.Intersect(endCurve.z, this.m_TerrainBounds.max.z, out t, 4))
          x = math.max(x, 1f + t);
        return x;
      }

      private void CheckOppositeSide(
        Bezier4x2 startCurve,
        Bezier4x2 endCurve,
        Bezier4x2 oppositeStartCurve,
        Bezier4x2 oppositeEndCurve,
        float2 intersectTangent,
        float2 oppositeIntersectTangent,
        ref float t,
        float oppositeT)
      {
        // ISSUE: reference to a compiler-generated method
        float2 x1 = this.Position(startCurve, endCurve, t);
        // ISSUE: reference to a compiler-generated method
        float2 float2_1 = this.Tangent(startCurve, endCurve, t);
        // ISSUE: reference to a compiler-generated method
        float2 y1 = this.Position(oppositeStartCurve, oppositeEndCurve, t);
        float2 y2 = x1 - y1;
        // ISSUE: reference to a compiler-generated method
        float2 _a = this.Position(oppositeStartCurve, oppositeEndCurve, oppositeT);
        // ISSUE: reference to a compiler-generated method
        float2 float2_2 = this.Tangent(oppositeStartCurve, oppositeEndCurve, oppositeT);
        float2 y3 = _a - x1;
        MathUtils.TryNormalize(ref intersectTangent);
        MathUtils.TryNormalize(ref oppositeIntersectTangent);
        MathUtils.TryNormalize(ref float2_1);
        MathUtils.TryNormalize(ref y2);
        MathUtils.TryNormalize(ref float2_2);
        MathUtils.TryNormalize(ref y3);
        double num1 = (double) math.dot(intersectTangent, float2_1);
        float x2 = math.dot(intersectTangent, y2);
        double num2 = (double) math.dot(oppositeIntersectTangent, float2_1);
        double num3 = (double) math.dot(oppositeIntersectTangent, float2_2);
        Line2.Segment line = new Line2.Segment(_a, _a - oppositeIntersectTangent * (math.distance(x1, y1) * 3f));
        float x3 = t;
        float2 t1;
        if (MathUtils.Intersect(startCurve, line, out t1, 4))
          x3 = math.max(x3, t1.x);
        if (MathUtils.Intersect(endCurve, line, out t1, 4))
          x3 = math.max(x3, t1.x + 1f);
        if (num1 > 0.0 && (double) x2 > 0.0)
          x2 = 1f;
        float x4 = 0.0f;
        float x5 = math.acos(math.saturate(math.dot(float2_2, y3)));
        float x6 = math.acos(math.saturate(math.dot(float2_1, y3)));
        float x7 = 1.57079637f;
        if ((double) x6 > 9.9999997473787516E-05)
          x7 = math.min(x7, math.sin(x5) / math.tan(x6) * x5);
        if ((double) x7 > (double) x6 && (double) x5 > (double) x6)
          x4 = math.lerp(t, oppositeT, (float) (((double) x7 - (double) x6) / (1.5707963705062866 - (double) x6)));
        t = math.lerp(x3, t, math.saturate(x2));
        t = math.lerp(t, oppositeT, math.saturate(math.dot(intersectTangent, oppositeIntersectTangent)));
        t = math.min(oppositeT, math.max(x4, t));
      }

      private float2 Position(Bezier4x2 startCurve, Bezier4x2 endCurve, float t)
      {
        return (double) t < 1.0 ? MathUtils.Position(startCurve, math.max(0.0f, t)) : MathUtils.Position(endCurve, math.min(1f, t - 1f));
      }

      private float2 Tangent(Bezier4x2 startCurve, Bezier4x2 endCurve, float t)
      {
        return (double) t < 1.0 ? MathUtils.Tangent(startCurve, math.max(0.0f, t)) : MathUtils.Tangent(endCurve, math.min(1f, t - 1f));
      }

      private float2 Intersect(
        Bezier4x2 startCurve1,
        Bezier4x2 endCurve1,
        Bezier4x2 nodeCurve2,
        Bezier4x2 startCurve2,
        Bezier4x2 endCurve2)
      {
        float2 t = new float2();
        // ISSUE: reference to a compiler-generated method
        this.Intersect(startCurve1, nodeCurve2, new float2(0.0f, -1f), ref t);
        // ISSUE: reference to a compiler-generated method
        this.Intersect(startCurve1, startCurve2, new float2(0.0f, 0.0f), ref t);
        // ISSUE: reference to a compiler-generated method
        this.Intersect(startCurve1, endCurve2, new float2(0.0f, 1f), ref t);
        // ISSUE: reference to a compiler-generated method
        this.Intersect(endCurve1, nodeCurve2, new float2(1f, -1f), ref t);
        // ISSUE: reference to a compiler-generated method
        this.Intersect(endCurve1, startCurve2, new float2(1f, 0.0f), ref t);
        // ISSUE: reference to a compiler-generated method
        this.Intersect(endCurve1, endCurve2, new float2(1f, 1f), ref t);
        return t;
      }

      private void Intersect(Bezier4x2 curve1, Bezier4x2 curve2, float2 offset, ref float2 t)
      {
        float2 t1;
        if (!MathUtils.Intersect(curve1, curve2, out t1, 4))
          return;
        float2 float2 = t1 + offset;
        if ((double) float2.x <= (double) t.x)
          return;
        t = float2;
      }

      private float CheckCrosswalks(
        DynamicBuffer<NetCompositionCrosswalk> prefabCompositionCrosswalks)
      {
        float x = 0.0f;
        for (int index = 0; index < prefabCompositionCrosswalks.Length; ++index)
        {
          NetCompositionCrosswalk compositionCrosswalk = prefabCompositionCrosswalks[index];
          x = math.max(x, math.max(compositionCrosswalk.m_Start.z, compositionCrosswalk.m_End.z));
        }
        return x;
      }

      private float CompareLanes(
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes1,
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes2,
        float offset,
        bool isEnd1,
        bool isEnd2,
        bool dontCrossTracks)
      {
        float4 roadLimits1;
        float4 trackLimits1;
        // ISSUE: reference to a compiler-generated method
        this.GetLaneLimits(prefabCompositionLanes1, isEnd1, !isEnd1, out roadLimits1, out trackLimits1);
        float4 roadLimits2;
        float4 trackLimits2;
        // ISSUE: reference to a compiler-generated method
        this.GetLaneLimits(prefabCompositionLanes2, isEnd2, isEnd2, out roadLimits2, out trackLimits2);
        float4 a1 = math.abs(roadLimits1 - roadLimits2 + offset);
        float4 a2 = math.abs(trackLimits1 - trackLimits2 + offset);
        float4 a3 = math.abs(trackLimits1 - trackLimits2.yxwz + offset);
        float4 x = math.select(a1, (float4) 0.0f, math.abs(roadLimits1) > 100000f | math.abs(roadLimits2) > 100000f);
        float4 float4 = math.select(a2, (float4) 0.0f, math.abs(trackLimits1) > 100000f | math.abs(trackLimits2) > 100000f);
        float4 y = math.select(math.select(a3, (float4) 0.0f, math.abs(trackLimits1) > 100000f | math.abs(trackLimits2.yxwz) > 100000f), float4, dontCrossTracks);
        return math.cmax(math.sqrt(new float2(math.cmax(x), math.cmax(math.max(float4, y)))) * new float2(3f, 4f));
      }

      private void GetLaneLimits(
        DynamicBuffer<NetCompositionLane> prefabCompositionLanes,
        bool isEnd,
        bool invert,
        out float4 roadLimits,
        out float4 trackLimits)
      {
        roadLimits = new float4(1000000f, -1000000f, 1000000f, -1000000f);
        trackLimits = new float4(1000000f, -1000000f, 1000000f, -1000000f);
        for (int index = 0; index < prefabCompositionLanes.Length; ++index)
        {
          NetCompositionLane prefabCompositionLane = prefabCompositionLanes[index];
          Game.Prefabs.LaneFlags laneFlags = (prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Invert) != 0 != isEnd ? Game.Prefabs.LaneFlags.DisconnectedEnd : Game.Prefabs.LaneFlags.DisconnectedStart;
          if ((prefabCompositionLane.m_Flags & laneFlags) == (Game.Prefabs.LaneFlags) 0)
          {
            prefabCompositionLane.m_Position.x = math.select(prefabCompositionLane.m_Position.x, -prefabCompositionLane.m_Position.x, invert);
            if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Road) != (Game.Prefabs.LaneFlags) 0)
            {
              if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
              {
                roadLimits.xz = math.min(roadLimits.xz, (float2) prefabCompositionLane.m_Position.x);
                roadLimits.yw = math.max(roadLimits.yw, (float2) prefabCompositionLane.m_Position.x);
              }
              else if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Invert) != 0 == invert)
              {
                roadLimits.x = math.min(roadLimits.x, prefabCompositionLane.m_Position.x);
                roadLimits.y = math.max(roadLimits.y, prefabCompositionLane.m_Position.x);
              }
              else
              {
                roadLimits.z = math.min(roadLimits.z, prefabCompositionLane.m_Position.x);
                roadLimits.w = math.max(roadLimits.w, prefabCompositionLane.m_Position.x);
              }
            }
            else if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Track) != (Game.Prefabs.LaneFlags) 0)
            {
              if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Twoway) != (Game.Prefabs.LaneFlags) 0)
              {
                trackLimits.xz = math.min(trackLimits.xz, (float2) prefabCompositionLane.m_Position.x);
                trackLimits.yw = math.max(trackLimits.yw, (float2) prefabCompositionLane.m_Position.x);
              }
              else if ((prefabCompositionLane.m_Flags & Game.Prefabs.LaneFlags.Invert) != 0 == invert)
              {
                trackLimits.x = math.min(trackLimits.x, prefabCompositionLane.m_Position.x);
                trackLimits.y = math.max(trackLimits.y, prefabCompositionLane.m_Position.x);
              }
              else
              {
                trackLimits.z = math.min(trackLimits.z, prefabCompositionLane.m_Position.x);
                trackLimits.w = math.max(trackLimits.w, prefabCompositionLane.m_Position.x);
              }
            }
          }
        }
      }

      private bool RequireTransition(
        NetCompositionData nodeCompositionData,
        NetCompositionData nodeCompositionData2,
        NetCompositionData edgeCompositionData,
        NetCompositionData edgeCompositionData2)
      {
        if ((double) math.abs(nodeCompositionData.m_HeightRange.min - nodeCompositionData2.m_HeightRange.min) > 0.10000000149011612 || (double) math.abs(nodeCompositionData.m_HeightRange.max - nodeCompositionData2.m_HeightRange.max) > 0.10000000149011612)
          return true;
        CompositionFlags compositionFlags1 = new CompositionFlags((CompositionFlags.General) 0, CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition, CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition);
        if (((nodeCompositionData.m_Flags | nodeCompositionData2.m_Flags) & compositionFlags1) != new CompositionFlags())
          return true;
        CompositionFlags compositionFlags2 = new CompositionFlags(CompositionFlags.General.Pavement | CompositionFlags.General.Gravel | CompositionFlags.General.Tiles, (CompositionFlags.Side) 0, (CompositionFlags.Side) 0);
        return (edgeCompositionData.m_Flags & compositionFlags2) != (edgeCompositionData2.m_Flags & compositionFlags2);
      }
    }

    private struct EdgeData
    {
      public float3 m_Left;
      public float3 m_Right;
      public Layer m_Layers;
      public Entity m_Entity;
      public float2 m_Changes;
      public float m_MaxSlope;
      public bool m_IsEnd;
      public bool m_IsTemp;
    }

    [BurstCompile]
    private struct AllocateBuffersJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_Entities;
      public NativeList<GeometrySystem.IntersectionData> m_IntersectionData;
      public NativeParallelHashMap<int2, float4> m_EdgeHeightMap;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IntersectionData.ResizeUninitialized(this.m_Entities.Length);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeHeightMap.Capacity = this.m_Entities.Length * 2;
      }
    }

    [BurstCompile]
    private struct FlattenNodeGeometryJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> m_NodeGeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      public NativeParallelHashMap<int2, float4>.ParallelWriter m_EdgeHeightMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NodeGeometry> nativeArray2 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
        NativeList<GeometrySystem.EdgeData> nativeList = new NativeList<GeometrySystem.EdgeData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Temp>(ref this.m_TempType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity node = nativeArray1[index1];
          NodeGeometry nodeGeometry = nativeArray2[index1];
          if ((double) nodeGeometry.m_Bounds.min.x == 0.0)
          {
            bool flag2 = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData);
            EdgeIteratorValue edgeIteratorValue;
            while (edgeIterator.GetNext(out edgeIteratorValue))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[edgeIteratorValue.m_Edge].m_Prefab];
              if (netGeometryData.m_MergeLayers != Layer.None)
              {
                // ISSUE: reference to a compiler-generated field
                EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeIteratorValue.m_Edge];
                // ISSUE: reference to a compiler-generated field
                bool flag3 = this.m_TempData.HasComponent(edgeIteratorValue.m_Edge);
                flag2 &= flag3;
                // ISSUE: object of a compiler-generated type is created
                nativeList.Add(new GeometrySystem.EdgeData()
                {
                  m_Left = edgeIteratorValue.m_End ? edgeGeometry.m_End.m_Left.d : edgeGeometry.m_Start.m_Right.a,
                  m_Right = edgeIteratorValue.m_End ? edgeGeometry.m_End.m_Right.d : edgeGeometry.m_Start.m_Left.a,
                  m_Layers = netGeometryData.m_MergeLayers,
                  m_Entity = edgeIteratorValue.m_Edge,
                  m_MaxSlope = netGeometryData.m_MaxSlopeSteepness,
                  m_IsEnd = edgeIteratorValue.m_End,
                  m_IsTemp = flag3
                });
              }
            }
            if (flag1 && !flag2)
            {
              for (int index2 = 0; index2 < nativeList.Length; ++index2)
              {
                // ISSUE: variable of a compiler-generated type
                GeometrySystem.EdgeData edgeData = nativeList[index2];
                EdgeGeometry componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (edgeData.m_IsTemp && this.m_EdgeGeometryData.TryGetComponent(this.m_TempData[edgeData.m_Entity].m_Original, out componentData))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeData.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  int2 key = new int2(edgeData.m_Entity.Index, math.select(0, 1, edgeData.m_IsEnd));
                  float4 float4;
                  // ISSUE: reference to a compiler-generated field
                  if (edgeData.m_IsEnd)
                  {
                    float4 = new float4(componentData.m_End.m_Left.d.yy, componentData.m_End.m_Right.d.yy);
                    float4.x += edgeGeometry.m_End.m_Left.c.y - edgeGeometry.m_End.m_Left.d.y;
                    float4.z += edgeGeometry.m_End.m_Right.c.y - edgeGeometry.m_End.m_Right.d.y;
                  }
                  else
                  {
                    float4 = new float4(componentData.m_Start.m_Right.a.yy, componentData.m_Start.m_Left.a.yy);
                    float4.x += edgeGeometry.m_Start.m_Right.b.y - edgeGeometry.m_Start.m_Right.a.y;
                    float4.z += edgeGeometry.m_Start.m_Left.b.y - edgeGeometry.m_Start.m_Left.a.y;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_EdgeHeightMap.TryAdd(key, float4);
                }
              }
            }
            else
            {
              bool flag4 = false;
              for (int index3 = 0; index3 < 100; ++index3)
              {
                bool flag5 = false;
                for (int index4 = 1; index4 < nativeList.Length; ++index4)
                {
                  ref GeometrySystem.EdgeData local1 = ref nativeList.ElementAt(index4);
                  for (int index5 = 0; index5 < index4; ++index5)
                  {
                    ref GeometrySystem.EdgeData local2 = ref nativeList.ElementAt(index5);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((local1.m_Layers & local2.m_Layers) != Layer.None)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float3 float3_1 = local2.m_Right - local1.m_Left;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float3 float3_2 = local2.m_Left - local1.m_Right;
                      float2 x1 = new float2(math.lengthsq(float3_1.xz), math.lengthsq(float3_2.xz));
                      float2 x2 = new float2(float3_1.y, float3_2.y);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      float num = local1.m_MaxSlope + local2.m_MaxSlope;
                      if (math.any(x2 * x2 > x1 * (float) ((double) num * (double) num * 1.0001000165939331)))
                      {
                        x1 = math.sqrt(x1);
                        float2 b1 = math.max((float2) 0.0f, math.abs(x2) - x1 * num);
                        bool2 c = x2 >= 0.0f;
                        float x3;
                        if (c.x != c.y)
                        {
                          x3 = math.csum(math.select(-b1, b1, c)) * 0.5f;
                        }
                        else
                        {
                          float b2 = math.max(b1.x, b1.y);
                          x3 = math.select(-b2, b2, c.x);
                        }
                        float2 float2_1;
                        if ((double) x3 >= 0.0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          float2_1.x = math.max(local1.m_Left.y, local1.m_Right.y);
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          float2_1.y = math.min(local2.m_Left.y, local2.m_Right.y);
                          float2_1 = nodeGeometry.m_Position - float2_1;
                          float2_1.x = math.max(0.0f, float2_1.x);
                          float2_1.y = math.min(0.0f, float2_1.y);
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          float2_1.x = math.min(local1.m_Left.y, local1.m_Right.y);
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          float2_1.y = math.max(local2.m_Left.y, local2.m_Right.y);
                          float2_1 = nodeGeometry.m_Position - float2_1;
                          float2_1.x = math.min(0.0f, float2_1.x);
                          float2_1.y = math.max(0.0f, float2_1.y);
                        }
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        float2_1 = math.select(float2_1, (float2) 0.0f, flag1 != new bool2(local1.m_IsTemp, local2.m_IsTemp));
                        float2 float2_2 = float2_1 * math.min(1f, math.abs(x3) / math.max(1f / 1000f, math.csum(math.abs(float2_1))));
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        local1.m_Changes.x = math.min(local1.m_Changes.x, float2_2.x);
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        local1.m_Changes.y = math.max(local1.m_Changes.y, float2_2.x);
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        local2.m_Changes.x = math.min(local2.m_Changes.x, float2_2.y);
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        local2.m_Changes.y = math.max(local2.m_Changes.y, float2_2.y);
                        flag5 = true;
                      }
                    }
                  }
                }
                if (flag5)
                {
                  for (int index6 = 0; index6 < nativeList.Length; ++index6)
                  {
                    ref GeometrySystem.EdgeData local = ref nativeList.ElementAt(index6);
                    // ISSUE: reference to a compiler-generated field
                    float num = math.csum(local.m_Changes);
                    // ISSUE: reference to a compiler-generated field
                    local.m_Left.y += num;
                    // ISSUE: reference to a compiler-generated field
                    local.m_Right.y += num;
                    // ISSUE: reference to a compiler-generated field
                    local.m_Changes = (float2) 0.0f;
                  }
                  flag4 = true;
                }
                else
                  break;
              }
              if (flag4)
              {
                for (int index7 = 0; index7 < nativeList.Length; ++index7)
                {
                  // ISSUE: variable of a compiler-generated type
                  GeometrySystem.EdgeData edgeData = nativeList[index7];
                  // ISSUE: reference to a compiler-generated field
                  if (flag1 == edgeData.m_IsTemp)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[edgeData.m_Entity];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    int2 key = new int2(edgeData.m_Entity.Index, math.select(0, 1, edgeData.m_IsEnd));
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float4 float4 = new float4(edgeData.m_Left.yy, edgeData.m_Right.yy);
                    // ISSUE: reference to a compiler-generated field
                    if (edgeData.m_IsEnd)
                    {
                      float4.x += edgeGeometry.m_End.m_Left.c.y - edgeGeometry.m_End.m_Left.d.y;
                      float4.z += edgeGeometry.m_End.m_Right.c.y - edgeGeometry.m_End.m_Right.d.y;
                    }
                    else
                    {
                      float4.x += edgeGeometry.m_Start.m_Right.b.y - edgeGeometry.m_Start.m_Right.a.y;
                      float4.z += edgeGeometry.m_Start.m_Left.b.y - edgeGeometry.m_Start.m_Left.a.y;
                    }
                    // ISSUE: reference to a compiler-generated field
                    this.m_EdgeHeightMap.TryAdd(key, float4);
                  }
                }
              }
            }
            nativeList.Clear();
          }
        }
        nativeList.Dispose();
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
    private struct FinishEdgeGeometryJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionDataFromEntity;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public NativeParallelHashMap<int2, float4> m_EdgeHeightMap;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_CompositionDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[entity].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[entity];
        float4 float4_1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeHeightMap.TryGetValue(new int2(entity.Index, 0), out float4_1))
        {
          edgeGeometry.m_Start.m_Right.b.y = float4_1.x;
          edgeGeometry.m_Start.m_Right.a.y = float4_1.y;
          edgeGeometry.m_Start.m_Left.b.y = float4_1.z;
          edgeGeometry.m_Start.m_Left.a.y = float4_1.w;
        }
        float4 float4_2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeHeightMap.TryGetValue(new int2(entity.Index, 1), out float4_2))
        {
          edgeGeometry.m_End.m_Left.c.y = float4_2.x;
          edgeGeometry.m_End.m_Left.d.y = float4_2.y;
          edgeGeometry.m_End.m_Right.c.y = float4_2.z;
          edgeGeometry.m_End.m_Right.d.y = float4_2.w;
        }
        if ((netGeometryData.m_Flags & GeometryFlags.SmoothSlopes) == (GeometryFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if ((((netCompositionData.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) == (CompositionFlags.General) 0 ? 1 : 0) & ((netCompositionData.m_Flags.m_Left & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0 ? 1 : ((netCompositionData.m_Flags.m_Right & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) == (CompositionFlags.Side) 0 ? 1 : 0)) & (!this.m_OwnerData.HasComponent(entity) ? 1 : 0)) == 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.StraightenMiddleHeights(ref edgeGeometry.m_Start.m_Left, ref edgeGeometry.m_End.m_Left);
            // ISSUE: reference to a compiler-generated method
            this.StraightenMiddleHeights(ref edgeGeometry.m_Start.m_Right, ref edgeGeometry.m_End.m_Right);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.LimitMiddleHeights(ref edgeGeometry.m_Start.m_Left, ref edgeGeometry.m_End.m_Left, netGeometryData.m_MaxSlopeSteepness, netCompositionData.m_Width);
            // ISSUE: reference to a compiler-generated method
            this.LimitMiddleHeights(ref edgeGeometry.m_Start.m_Right, ref edgeGeometry.m_End.m_Right, netGeometryData.m_MaxSlopeSteepness, netCompositionData.m_Width);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.LimitMiddleHeights(ref edgeGeometry.m_Start.m_Left, ref edgeGeometry.m_End.m_Left, netGeometryData.m_MaxSlopeSteepness, netCompositionData.m_Width);
          // ISSUE: reference to a compiler-generated method
          this.LimitMiddleHeights(ref edgeGeometry.m_Start.m_Right, ref edgeGeometry.m_End.m_Right, netGeometryData.m_MaxSlopeSteepness, netCompositionData.m_Width);
        }
        edgeGeometry.m_Start.m_Length.x = MathUtils.Length(edgeGeometry.m_Start.m_Left);
        edgeGeometry.m_Start.m_Length.y = MathUtils.Length(edgeGeometry.m_Start.m_Right);
        edgeGeometry.m_End.m_Length.x = MathUtils.Length(edgeGeometry.m_End.m_Left);
        edgeGeometry.m_End.m_Length.y = MathUtils.Length(edgeGeometry.m_End.m_Right);
        edgeGeometry.m_Bounds = MathUtils.TightBounds(edgeGeometry.m_Start.m_Left) | MathUtils.TightBounds(edgeGeometry.m_Start.m_Right) | MathUtils.TightBounds(edgeGeometry.m_End.m_Left) | MathUtils.TightBounds(edgeGeometry.m_End.m_Right);
        edgeGeometry.m_Bounds.min.y += netCompositionData.m_HeightRange.min;
        edgeGeometry.m_Bounds.max.y += netCompositionData.m_HeightRange.max;
        if ((netCompositionData.m_State & (CompositionState.LowerToTerrain | CompositionState.RaiseToTerrain)) != (CompositionState) 0)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          Bounds1 bounds1 = this.SampleTerrain(edgeGeometry.m_Start.m_Left) | this.SampleTerrain(edgeGeometry.m_Start.m_Right) | this.SampleTerrain(edgeGeometry.m_End.m_Left) | this.SampleTerrain(edgeGeometry.m_End.m_Right);
          if ((netCompositionData.m_State & CompositionState.LowerToTerrain) != (CompositionState) 0)
            edgeGeometry.m_Bounds.min.y = math.min(edgeGeometry.m_Bounds.min.y, bounds1.min);
          if ((netCompositionData.m_State & CompositionState.RaiseToTerrain) != (CompositionState) 0)
            edgeGeometry.m_Bounds.max.y = math.max(edgeGeometry.m_Bounds.max.y, bounds1.max);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeGeometryData[entity] = edgeGeometry;
      }

      private Bounds1 SampleTerrain(Bezier4x3 curve)
      {
        Bounds1 bounds1 = new Bounds1(float.MaxValue, float.MinValue);
        for (int index = 0; index <= 8; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          bounds1 |= TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, MathUtils.Position(curve, (float) index * 0.125f));
        }
        return bounds1;
      }

      private void StraightenMiddleHeights(ref Bezier4x3 start, ref Bezier4x3 end)
      {
        float4 float4_1;
        float4_1.x = math.distance(start.b.xz, start.c.xz);
        float4_1.y = float4_1.x + math.distance(start.c.xz, start.d.xz);
        float4_1.z = float4_1.y + math.distance(end.a.xz, end.b.xz);
        float4_1.w = float4_1.z + math.distance(end.b.xz, end.c.xz);
        float4 float4_2 = math.select(float4_1 / float4_1.w, (float4) 0.0f, (double) float4_1.w == 0.0);
        float3 float3 = math.lerp((float3) start.b.y, (float3) end.c.y, float4_2.xyz);
        start.c.y = float3.x;
        start.d.y = float3.y;
        end.a.y = float3.y;
        end.b.y = float3.z;
      }

      private void LimitMiddleHeights(
        ref Bezier4x3 start,
        ref Bezier4x3 end,
        float maxSlope,
        float width)
      {
        float num1 = MathUtils.Length(start.xz);
        float num2 = MathUtils.Length(end.xz);
        float num3 = num1 * maxSlope;
        float num4 = num2 * maxSlope;
        Bounds1 bounds = new Bounds1(math.max(start.a.y - num3, end.d.y - num4), math.min(start.a.y + num3, end.d.y + num4));
        if ((double) bounds.max < (double) bounds.min)
        {
          bounds = new Bounds1((float2) (float) (((double) bounds.min + (double) bounds.max) * 0.5));
        }
        else
        {
          float y = (float) (((double) bounds.min + (double) bounds.max) * 0.5);
          float s = (float) (1.0 / (0.5 * ((double) num1 + (double) num2) / (double) math.max(0.01f, width) + 1.0));
          bounds.min = math.lerp(bounds.min, y, s);
          bounds.max = math.lerp(bounds.max, y, s);
        }
        float num5 = MathUtils.Clamp(start.d.y, bounds) - start.d.y;
        start.c.y += num5;
        start.d.y += num5;
        end.a.y += num5;
        end.b.y += num5;
      }
    }

    [BurstCompile]
    private struct CalculateNodeGeometryJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionDataFromEntity;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_GeometryDataFromEntity;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public int m_IterationIndex;
      [ReadOnly]
      public NativeArray<Entity> m_Entities;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[entity];
        // ISSUE: reference to a compiler-generated field
        EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[entity];
        // ISSUE: reference to a compiler-generated field
        if (this.m_IterationIndex == 1 && (double) startNodeGeometry.m_Geometry.m_Left.m_Length.x >= 0.0 && (double) startNodeGeometry.m_Geometry.m_Right.m_Length.x >= 0.0 && (double) endNodeGeometry.m_Geometry.m_Left.m_Length.x >= 0.0 && (double) endNodeGeometry.m_Geometry.m_Right.m_Length.x >= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        Edge edge = this.m_EdgeDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_CompositionDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_GeometryDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[entity].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData edgeCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[composition.m_EndNode];
        // ISSUE: reference to a compiler-generated field
        NodeGeometry nodeGeometry1 = this.m_NodeGeometryData[edge.m_Start];
        // ISSUE: reference to a compiler-generated field
        NodeGeometry nodeGeometry2 = this.m_NodeGeometryData[edge.m_End];
        // ISSUE: reference to a compiler-generated method
        float3 middleNodePos1 = this.FindMiddleNodePos(entity, edge.m_Start) with
        {
          y = nodeGeometry1.m_Position
        };
        // ISSUE: reference to a compiler-generated method
        float3 middleNodePos2 = this.FindMiddleNodePos(entity, edge.m_End) with
        {
          y = nodeGeometry2.m_Position
        };
        // ISSUE: reference to a compiler-generated method
        float2 offset1 = this.StartOffset(edgeGeometry.m_Start, middleNodePos1.xz);
        // ISSUE: reference to a compiler-generated method
        float2 offset2 = this.EndOffset(edgeGeometry.m_End, middleNodePos2.xz);
        Segment leftSegment1;
        Segment rightSegment1;
        float2 distances1;
        float4 syncVertexOffsetsLeft1;
        float4 syncVertexOffsetsRight1;
        float roundaboutSize1;
        bool2 sideConnect1;
        bool2 middleConnect1;
        // ISSUE: reference to a compiler-generated method
        this.FindTargetSegments(entity, edge.m_Start, offset1, middleNodePos1.xz, prefabGeometryData, netCompositionData1, out leftSegment1, out rightSegment1, out distances1, out syncVertexOffsetsLeft1, out syncVertexOffsetsRight1, out roundaboutSize1, out sideConnect1, out middleConnect1);
        Segment leftSegment2;
        Segment rightSegment2;
        float2 distances2;
        float4 syncVertexOffsetsLeft2;
        float4 syncVertexOffsetsRight2;
        float roundaboutSize2;
        bool2 sideConnect2;
        bool2 middleConnect2;
        // ISSUE: reference to a compiler-generated method
        this.FindTargetSegments(entity, edge.m_End, offset2, middleNodePos2.xz, prefabGeometryData, netCompositionData2, out leftSegment2, out rightSegment2, out distances2, out syncVertexOffsetsLeft2, out syncVertexOffsetsRight2, out roundaboutSize2, out sideConnect2, out middleConnect2);
        // ISSUE: reference to a compiler-generated method
        Segment segment1 = this.Invert(edgeGeometry.m_Start);
        Segment end = edgeGeometry.m_End;
        // ISSUE: reference to a compiler-generated method
        this.AdjustSegmentWidth(ref segment1, edgeCompositionData, netCompositionData1, true);
        // ISSUE: reference to a compiler-generated method
        this.AdjustSegmentWidth(ref end, edgeCompositionData, netCompositionData2, false);
        Segment segment2 = segment1;
        Segment segment3 = end;
        float t1 = (float) ((double) netCompositionData1.m_MiddleOffset / (double) netCompositionData1.m_Width + 0.5);
        float t2 = (float) ((double) netCompositionData2.m_MiddleOffset / (double) netCompositionData2.m_Width + 0.5);
        segment1.m_Right = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, t1);
        segment2.m_Left = MathUtils.Lerp(segment2.m_Left, segment2.m_Right, t1);
        end.m_Right = MathUtils.Lerp(end.m_Left, end.m_Right, t2);
        segment3.m_Left = MathUtils.Lerp(segment3.m_Left, segment3.m_Right, t2);
        float leftRadius1 = 0.0f;
        float rightRadius1 = 0.0f;
        float leftRadius2 = 0.0f;
        float rightRadius2 = 0.0f;
        startNodeGeometry.m_Geometry.m_Middle = new Bezier4x3();
        endNodeGeometry.m_Geometry.m_Middle = new Bezier4x3();
        float2 float2_1;
        float2 float2_2;
        if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = this.m_NodeDataFromEntity[edge.m_Start].m_Position with
          {
            y = nodeGeometry1.m_Position
          };
          leftRadius1 = startNodeGeometry.m_Geometry.m_MiddleRadius + roundaboutSize1;
          rightRadius1 = leftRadius1;
          // ISSUE: reference to a compiler-generated method
          this.CalculateSegments(segment1, segment2, leftSegment1, rightSegment1, position, ref leftRadius1, ref rightRadius1, out startNodeGeometry.m_Geometry.m_Left, out startNodeGeometry.m_Geometry.m_Right, out float2_1, out float2_2);
        }
        else
        {
          if (sideConnect1.x)
          {
            // ISSUE: reference to a compiler-generated method
            startNodeGeometry.m_Geometry.m_Left = this.CalculateSideConnection(segment1, leftSegment1, rightSegment1, edgeCompositionData, netCompositionData1, false, out float2_1);
            startNodeGeometry.m_Geometry.m_Middle.d.x = 1f;
          }
          else if (middleConnect1.x)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_IterationIndex == 0)
            {
              startNodeGeometry.m_Geometry.m_Left.m_Length = (float2) -1f;
              float2_1 = (float2) -1f;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              startNodeGeometry.m_Geometry.m_Left = this.CalculateMiddleConnection(segment1, leftSegment1, out float2_1);
              startNodeGeometry.m_Geometry.m_Middle.d.x = 1f;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            startNodeGeometry.m_Geometry.m_Left = this.CalculateMiddleSegment(segment1, leftSegment1, out float2_1);
          }
          if (sideConnect1.y)
          {
            // ISSUE: reference to a compiler-generated method
            startNodeGeometry.m_Geometry.m_Right = this.CalculateSideConnection(segment2, leftSegment1, rightSegment1, edgeCompositionData, netCompositionData1, true, out float2_2);
            startNodeGeometry.m_Geometry.m_Middle.d.y = 1f;
          }
          else if (middleConnect1.y)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_IterationIndex == 0)
            {
              startNodeGeometry.m_Geometry.m_Right.m_Length = (float2) -1f;
              float2_2 = (float2) -1f;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              startNodeGeometry.m_Geometry.m_Right = this.CalculateMiddleConnection(segment2, rightSegment1, out float2_2);
              startNodeGeometry.m_Geometry.m_Middle.d.y = 1f;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            startNodeGeometry.m_Geometry.m_Right = this.CalculateMiddleSegment(segment2, rightSegment1, out float2_2);
          }
        }
        float2 float2_3;
        float2 float2_4;
        if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = this.m_NodeDataFromEntity[edge.m_End].m_Position with
          {
            y = nodeGeometry2.m_Position
          };
          leftRadius2 = endNodeGeometry.m_Geometry.m_MiddleRadius + roundaboutSize2;
          rightRadius2 = leftRadius2;
          // ISSUE: reference to a compiler-generated method
          this.CalculateSegments(end, segment3, leftSegment2, rightSegment2, position, ref leftRadius2, ref rightRadius2, out endNodeGeometry.m_Geometry.m_Left, out endNodeGeometry.m_Geometry.m_Right, out float2_3, out float2_4);
        }
        else
        {
          if (sideConnect2.x)
          {
            // ISSUE: reference to a compiler-generated method
            endNodeGeometry.m_Geometry.m_Left = this.CalculateSideConnection(end, leftSegment2, rightSegment2, edgeCompositionData, netCompositionData2, false, out float2_3);
            endNodeGeometry.m_Geometry.m_Middle.d.x = 1f;
          }
          else if (middleConnect2.x)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_IterationIndex == 0)
            {
              endNodeGeometry.m_Geometry.m_Left.m_Length = (float2) -1f;
              float2_3 = (float2) -1f;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              endNodeGeometry.m_Geometry.m_Left = this.CalculateMiddleConnection(end, leftSegment2, out float2_3);
              endNodeGeometry.m_Geometry.m_Middle.d.x = 1f;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            endNodeGeometry.m_Geometry.m_Left = this.CalculateMiddleSegment(end, leftSegment2, out float2_3);
          }
          if (sideConnect2.y)
          {
            // ISSUE: reference to a compiler-generated method
            endNodeGeometry.m_Geometry.m_Right = this.CalculateSideConnection(segment3, leftSegment2, rightSegment2, edgeCompositionData, netCompositionData2, true, out float2_4);
            endNodeGeometry.m_Geometry.m_Middle.d.y = 1f;
          }
          else if (middleConnect2.y)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_IterationIndex == 0)
            {
              endNodeGeometry.m_Geometry.m_Right.m_Length = (float2) -1f;
              float2_4 = (float2) -1f;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              endNodeGeometry.m_Geometry.m_Right = this.CalculateMiddleConnection(segment3, rightSegment2, out float2_4);
              endNodeGeometry.m_Geometry.m_Middle.d.y = 1f;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            endNodeGeometry.m_Geometry.m_Right = this.CalculateMiddleSegment(segment3, rightSegment2, out float2_4);
          }
        }
        float2 float2_5 = netCompositionData1.m_Width * 0.5f + new float2(netCompositionData1.m_MiddleOffset, -netCompositionData1.m_MiddleOffset);
        float2 float2_6 = netCompositionData2.m_Width * 0.5f + new float2(netCompositionData2.m_MiddleOffset, -netCompositionData2.m_MiddleOffset);
        if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = this.m_NodeDataFromEntity[edge.m_Start].m_Position with
          {
            y = nodeGeometry1.m_Position
          };
          // ISSUE: reference to a compiler-generated method
          startNodeGeometry.m_Geometry.m_SyncVertexTargetsLeft = this.CalculateVertexSyncTarget(ref startNodeGeometry.m_Geometry.m_Right, position, leftRadius1, false, float2_5.x, distances1.x, netCompositionData1.m_SyncVertexOffsetsLeft, syncVertexOffsetsLeft1, float2_1.y);
          // ISSUE: reference to a compiler-generated method
          startNodeGeometry.m_Geometry.m_SyncVertexTargetsRight = this.CalculateVertexSyncTarget(ref startNodeGeometry.m_Geometry.m_Right, position, rightRadius1, true, float2_5.y, distances1.y, netCompositionData1.m_SyncVertexOffsetsRight, syncVertexOffsetsRight1, float2_2.x);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!math.any(middleConnect1) || this.m_IterationIndex != 0)
          {
            // ISSUE: reference to a compiler-generated method
            startNodeGeometry.m_Geometry.m_SyncVertexTargetsLeft = this.CalculateVertexSyncTarget(ref startNodeGeometry.m_Geometry.m_Left, float2_5.x, distances1.x, netCompositionData1.m_SyncVertexOffsetsLeft, syncVertexOffsetsLeft1, float2_1.y, false);
            // ISSUE: reference to a compiler-generated method
            startNodeGeometry.m_Geometry.m_SyncVertexTargetsRight = this.CalculateVertexSyncTarget(ref startNodeGeometry.m_Geometry.m_Right, float2_5.y, distances1.y, netCompositionData1.m_SyncVertexOffsetsRight, syncVertexOffsetsRight1, float2_2.x, true);
          }
        }
        if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = this.m_NodeDataFromEntity[edge.m_End].m_Position with
          {
            y = nodeGeometry2.m_Position
          };
          // ISSUE: reference to a compiler-generated method
          endNodeGeometry.m_Geometry.m_SyncVertexTargetsLeft = this.CalculateVertexSyncTarget(ref endNodeGeometry.m_Geometry.m_Right, position, leftRadius2, false, float2_6.x, distances2.x, netCompositionData2.m_SyncVertexOffsetsLeft, syncVertexOffsetsLeft2, float2_3.y);
          // ISSUE: reference to a compiler-generated method
          endNodeGeometry.m_Geometry.m_SyncVertexTargetsRight = this.CalculateVertexSyncTarget(ref endNodeGeometry.m_Geometry.m_Right, position, rightRadius2, true, float2_6.y, distances2.y, netCompositionData2.m_SyncVertexOffsetsRight, syncVertexOffsetsRight2, float2_4.x);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!math.any(middleConnect2) || this.m_IterationIndex != 0)
          {
            // ISSUE: reference to a compiler-generated method
            endNodeGeometry.m_Geometry.m_SyncVertexTargetsLeft = this.CalculateVertexSyncTarget(ref endNodeGeometry.m_Geometry.m_Left, float2_6.x, distances2.x, netCompositionData2.m_SyncVertexOffsetsLeft, syncVertexOffsetsLeft2, float2_3.y, false);
            // ISSUE: reference to a compiler-generated method
            endNodeGeometry.m_Geometry.m_SyncVertexTargetsRight = this.CalculateVertexSyncTarget(ref endNodeGeometry.m_Geometry.m_Right, float2_6.y, distances2.y, netCompositionData2.m_SyncVertexOffsetsRight, syncVertexOffsetsRight2, float2_4.x, true);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometryData[entity] = startNodeGeometry;
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometryData[entity] = endNodeGeometry;
      }

      private Segment Invert(Segment segment)
      {
        Segment segment1;
        segment1.m_Left = MathUtils.Invert(segment.m_Right);
        segment1.m_Right = MathUtils.Invert(segment.m_Left);
        segment1.m_Length = segment.m_Length.yx;
        return segment1;
      }

      private float4 CalculateVertexSyncTarget(
        ref Segment roundaboutSegment,
        float3 middlePos,
        float radius,
        bool isRight,
        float startDistance,
        float endDistance,
        float4 a,
        float4 b,
        float t)
      {
        float2 float2;
        float num1;
        if (isRight)
        {
          float2 = 1f - new float2(a.w, b.w);
          float3 float3 = middlePos - roundaboutSegment.m_Right.d;
          float num2 = (float) (((double) float2.y * (double) endDistance - (double) float2.x * (double) startDistance) * 0.5);
          float3 = MathUtils.Normalize(float3, float3.xz) * num2;
          roundaboutSegment.m_Right.c += float3 * 0.5f;
          roundaboutSegment.m_Right.d += float3;
          a = math.saturate(1f - (1f - a - float2.x) / (1f - float2.x));
          b = math.saturate(1f - (1f - b - float2.y) / (1f - float2.y));
          num1 = math.distance(middlePos.xz, roundaboutSegment.m_Right.d.xz);
        }
        else
        {
          float2 = new float2(a.x, b.x);
          float3 float3_1 = middlePos - roundaboutSegment.m_Left.d;
          float num3 = (float) (((double) float2.y * (double) endDistance - (double) float2.x * (double) startDistance) * 0.5);
          float3 float3_2 = MathUtils.Normalize(float3_1, float3_1.xz) * num3;
          roundaboutSegment.m_Left.c += float3_2 * 0.5f;
          roundaboutSegment.m_Left.d += float3_2;
          a = math.saturate((a - float2.x) / (1f - float2.x));
          b = math.saturate((b - float2.y) / (1f - float2.y));
          num1 = math.distance(middlePos.xz, roundaboutSegment.m_Left.d.xz);
        }
        float num4 = float2.x * startDistance / num1;
        startDistance -= float2.x * startDistance;
        endDistance -= float2.y * endDistance;
        float4 a1 = new float4(a.xw, b.xw);
        float4 b1 = new float4(a.yz, b.yz);
        float4 float4_1 = math.select(a1, b1, (a1 == b1).zwxy);
        a.xw = float4_1.xy;
        b.xw = float4_1.zw;
        float num5 = math.lerp(startDistance, endDistance, t);
        float4 float4_2 = math.select(a, math.lerp(a * startDistance, b * endDistance, t) / num5, (double) num5 >= 1.4012984643248171E-45);
        float4 vertexSyncTarget;
        if (isRight)
        {
          float4 float4_3 = math.saturate((1f - float4_2) * num5 / radius);
          vertexSyncTarget = math.saturate(1f - num4 - float4_3 * (1f - num4));
        }
        else
        {
          float4 float4_4 = math.saturate(float4_2 * num5 / radius);
          vertexSyncTarget = math.saturate(num4 + float4_4 * (1f - num4));
        }
        return vertexSyncTarget;
      }

      private float4 CalculateVertexSyncTarget(
        ref Segment startSegment,
        float startDistance,
        float endDistance,
        float4 a,
        float4 b,
        float t,
        bool isRight)
      {
        float num1 = (float) (((double) startDistance + (double) endDistance) * 0.5);
        float2 float2;
        float num2;
        if (isRight)
        {
          float2 = 1f - new float2(a.w, b.w);
          float3 float3_1 = startSegment.m_Left.d - startSegment.m_Right.d;
          float num3 = (float) (((double) float2.y * (double) endDistance - (double) float2.x * (double) startDistance) * 0.5);
          num2 = num1 - num3;
          float3 float3_2 = float3_1 * (num3 / startDistance);
          startSegment.m_Right.c += float3_2 * 0.5f;
          startSegment.m_Right.d += float3_2;
          a = math.saturate(1f - (1f - a - float2.x) / (1f - float2.x));
          b = math.saturate(1f - (1f - b - float2.y) / (1f - float2.y));
        }
        else
        {
          float2 = new float2(a.x, b.x);
          float3 float3_3 = startSegment.m_Right.d - startSegment.m_Left.d;
          float num4 = (float) (((double) float2.y * (double) endDistance - (double) float2.x * (double) startDistance) * 0.5);
          num2 = num1 - num4;
          float3 float3_4 = float3_3 * (num4 / startDistance);
          startSegment.m_Left.c += float3_4 * 0.5f;
          startSegment.m_Left.d += float3_4;
          a = math.saturate((a - float2.x) / (1f - float2.x));
          b = math.saturate((b - float2.y) / (1f - float2.y));
        }
        float num5 = float2.x * startDistance / num2;
        startDistance -= float2.x * startDistance;
        endDistance -= float2.y * endDistance;
        float4 a1 = new float4(a.xw, b.xw);
        float4 b1 = new float4(a.yz, b.yz);
        float4 float4_1 = math.select(a1, b1, (a1 == b1).zwxy);
        a.xw = float4_1.xy;
        b.xw = float4_1.zw;
        float num6 = math.lerp(startDistance, endDistance, t);
        float4 float4_2 = math.select(a, math.lerp(a * startDistance, b * endDistance, t) / num6, (double) num6 >= 1.4012984643248171E-45);
        return !isRight ? math.saturate(num5 + float4_2 * (1f - num5)) : math.saturate(1f - num5 - (1f - float4_2) * (1f - num5));
      }

      private float3 FindMiddleNodePos(Entity edge, Entity node)
      {
        float3 x = new float3(1E+09f, 1E+09f, 1E+09f);
        float3 float3 = new float3(-1E+09f, -1E+09f, -1E+09f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge, node, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_GeometryDataFromEntity[edgeIteratorValue.m_Edge];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[this.m_CompositionDataFromEntity[edgeIteratorValue.m_Edge].m_Edge];
          float3 y;
          if (edgeIteratorValue.m_End)
          {
            float s = (float) (0.5 + (double) netCompositionData.m_MiddleOffset / (double) netCompositionData.m_Width);
            y = math.lerp(edgeGeometry.m_End.m_Left.d, edgeGeometry.m_End.m_Right.d, s);
          }
          else
          {
            float s = (float) (0.5 + (double) netCompositionData.m_MiddleOffset / (double) netCompositionData.m_Width);
            y = math.lerp(edgeGeometry.m_Start.m_Left.a, edgeGeometry.m_Start.m_Right.a, s);
          }
          x = math.min(x, y);
          float3 = math.max(float3, y);
        }
        return math.lerp(x, float3, 0.5f);
      }

      private float2 StartOffset(Segment segment, float2 nodePos)
      {
        float2 float2_1 = math.lerp(segment.m_Left.a.xz, segment.m_Right.a.xz, 0.5f) - nodePos;
        float3 float3 = MathUtils.StartTangent(segment.m_Left);
        float2 xz1 = float3.xz;
        float3 = MathUtils.StartTangent(segment.m_Right);
        float2 xz2 = float3.xz;
        float2 float2_2 = math.normalizesafe(xz1 + xz2);
        return math.normalizesafe(float2_1 + float2_2, new float2(0.0f, 1f));
      }

      private float2 EndOffset(Segment segment, float2 nodePos)
      {
        float2 float2_1 = math.lerp(segment.m_Left.d.xz, segment.m_Right.d.xz, 0.5f) - nodePos;
        float3 float3 = MathUtils.EndTangent(segment.m_Left);
        float2 xz1 = float3.xz;
        float3 = MathUtils.EndTangent(segment.m_Right);
        float2 xz2 = float3.xz;
        float2 float2_2 = math.normalizesafe(xz1 + xz2);
        return math.normalizesafe(float2_1 - float2_2, new float2(0.0f, 1f));
      }

      private void AdjustSegmentWidth(
        ref Segment segment,
        NetCompositionData edgeCompositionData,
        NetCompositionData nodeCompositionData,
        bool invertEdge)
      {
        Segment segment1 = segment;
        float num = math.select(edgeCompositionData.m_MiddleOffset, -edgeCompositionData.m_MiddleOffset, invertEdge);
        float2 float2 = (edgeCompositionData.m_Width * 0.5f + num - nodeCompositionData.m_MiddleOffset + nodeCompositionData.m_Width * new float2(-0.5f, 0.5f)) / edgeCompositionData.m_Width;
        --float2.y;
        if ((double) math.abs(float2.x) > 1.0 / 1000.0)
          segment.m_Left = MathUtils.Lerp(segment1.m_Left, segment1.m_Right, float2.x);
        if ((double) math.abs(float2.y) <= 1.0 / 1000.0)
          return;
        segment.m_Right = MathUtils.Lerp(segment1.m_Right, segment1.m_Left, -float2.y);
      }

      private void CheckBestCurve(
        Bezier4x3 curve,
        float2 position,
        bool isRight,
        bool isEnd,
        ref Bezier4x3 bestCurve,
        ref float bestDistance,
        ref float bestT,
        ref bool2 bestStartEnd)
      {
        float t;
        float num = MathUtils.Distance(curve.xz, position, out t);
        if ((double) num >= (double) bestDistance)
          return;
        bestDistance = num;
        bestStartEnd = new bool2(!isEnd, isEnd) & new bool2((double) t < 1.0 / 1000.0, (double) t > 0.99900001287460327);
        if (isRight)
        {
          bestCurve = curve;
          bestT = t;
        }
        else
        {
          bestCurve = MathUtils.Invert(curve);
          bestT = 1f - t;
        }
      }

      private void CheckNodeCurves(
        Entity node,
        float2 middlePos,
        NetGeometryData edgePrefabGeometryData,
        bool isEnd,
        ref Bezier4x3 bestCurve,
        ref float bestDistance,
        ref float bestT,
        ref bool2 bestStartEnd)
      {
        Bezier4x3 bestCurve1 = bestCurve;
        float bestDistance1 = bestDistance;
        float bestT1 = bestT;
        bool2 bestStartEnd1 = bestStartEnd;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData, true);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          if (edgeIteratorValue.m_Middle)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[edgeIteratorValue.m_Edge].m_Prefab];
          if ((edgePrefabGeometryData.m_MergeLayers & netGeometryData.m_MergeLayers) != Layer.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EdgeNodeGeometry edgeNodeGeometry = !edgeIteratorValue.m_End ? this.m_StartNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry : this.m_EndNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry;
            if (math.any(edgeNodeGeometry.m_Left.m_Length > 0.05f) | math.any(edgeNodeGeometry.m_Right.m_Length > 0.05f))
            {
              if ((double) edgeNodeGeometry.m_MiddleRadius > 0.0)
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckBestCurve(edgeNodeGeometry.m_Left.m_Left, middlePos, false, isEnd, ref bestCurve1, ref bestDistance1, ref bestT1, ref bestStartEnd1);
                // ISSUE: reference to a compiler-generated method
                this.CheckBestCurve(edgeNodeGeometry.m_Left.m_Right, middlePos, true, isEnd, ref bestCurve1, ref bestDistance1, ref bestT1, ref bestStartEnd1);
                // ISSUE: reference to a compiler-generated method
                this.CheckBestCurve(edgeNodeGeometry.m_Right.m_Left, middlePos, false, isEnd, ref bestCurve1, ref bestDistance1, ref bestT1, ref bestStartEnd1);
                // ISSUE: reference to a compiler-generated method
                this.CheckBestCurve(edgeNodeGeometry.m_Right.m_Right, middlePos, true, isEnd, ref bestCurve1, ref bestDistance1, ref bestT1, ref bestStartEnd1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckBestCurve(edgeNodeGeometry.m_Left.m_Left, middlePos, false, isEnd, ref bestCurve1, ref bestDistance1, ref bestT1, ref bestStartEnd1);
                // ISSUE: reference to a compiler-generated method
                this.CheckBestCurve(edgeNodeGeometry.m_Right.m_Right, middlePos, true, isEnd, ref bestCurve1, ref bestDistance1, ref bestT1, ref bestStartEnd1);
              }
            }
          }
        }
        bestCurve = bestCurve1;
        bestDistance = bestDistance1;
        bestT = bestT1;
        bestStartEnd = bestStartEnd1;
      }

      private void FindTargetSegments(
        Entity edge,
        Entity node,
        float2 offset,
        float2 middlePos,
        NetGeometryData prefabGeometryData,
        NetCompositionData compositionData,
        out Segment leftSegment,
        out Segment rightSegment,
        out float2 distances,
        out float4 syncVertexOffsetsLeft,
        out float4 syncVertexOffsetsRight,
        out float roundaboutSize,
        out bool2 sideConnect,
        out bool2 middleConnect)
      {
        float2 x = new float2(offset.y, -offset.x);
        float num1 = -2f;
        float num2 = 2f;
        float num3 = float.MaxValue;
        leftSegment = new Segment();
        rightSegment = new Segment();
        EdgeIteratorValue edgeIteratorValue1 = new EdgeIteratorValue();
        EdgeIteratorValue edgeIteratorValue2 = new EdgeIteratorValue();
        distances = (float2) 0.0f;
        roundaboutSize = 0.0f;
        sideConnect = (bool2) false;
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_OutsideConnectionData.HasComponent(node);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge, node, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData, true);
        EdgeIteratorValue edgeIteratorValue3;
        while (edgeIterator.GetNext(out edgeIteratorValue3))
        {
          if (edgeIteratorValue3.m_Middle)
          {
            if ((prefabGeometryData.m_MergeLayers & (Layer.Pathway | Layer.MarkerPathway)) != Layer.None)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NetGeometryData edgePrefabGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[edgeIteratorValue3.m_Edge].m_Prefab];
              // ISSUE: reference to a compiler-generated field
              EdgeGeometry edgeGeometry = this.m_GeometryDataFromEntity[edgeIteratorValue3.m_Edge];
              Bezier4x3 bestCurve = new Bezier4x3();
              float maxValue = float.MaxValue;
              float bestT = 0.0f;
              bool2 bestStartEnd = (bool2) false;
              // ISSUE: reference to a compiler-generated method
              this.CheckBestCurve(edgeGeometry.m_Start.m_Left, middlePos, false, false, ref bestCurve, ref maxValue, ref bestT, ref bestStartEnd);
              // ISSUE: reference to a compiler-generated method
              this.CheckBestCurve(edgeGeometry.m_Start.m_Right, middlePos, true, false, ref bestCurve, ref maxValue, ref bestT, ref bestStartEnd);
              // ISSUE: reference to a compiler-generated method
              this.CheckBestCurve(edgeGeometry.m_End.m_Left, middlePos, false, true, ref bestCurve, ref maxValue, ref bestT, ref bestStartEnd);
              // ISSUE: reference to a compiler-generated method
              this.CheckBestCurve(edgeGeometry.m_End.m_Right, middlePos, true, true, ref bestCurve, ref maxValue, ref bestT, ref bestStartEnd);
              // ISSUE: reference to a compiler-generated field
              if (this.m_IterationIndex == 1 && math.any(bestStartEnd))
              {
                // ISSUE: reference to a compiler-generated field
                Edge edge1 = this.m_EdgeDataFromEntity[edgeIteratorValue3.m_Edge];
                if (bestStartEnd.x)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckNodeCurves(edge1.m_Start, middlePos, edgePrefabGeometryData, false, ref bestCurve, ref maxValue, ref bestT, ref bestStartEnd);
                }
                if (bestStartEnd.y)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckNodeCurves(edge1.m_End, middlePos, edgePrefabGeometryData, true, ref bestCurve, ref maxValue, ref bestT, ref bestStartEnd);
                }
              }
              if ((double) maxValue < (double) num3)
              {
                num3 = maxValue;
                float3 startPos1 = MathUtils.Position(bestCurve, bestT);
                float3 float3 = new float3();
                float3.xz = math.normalizesafe(MathUtils.Left(MathUtils.Tangent(bestCurve, bestT).xz));
                float3 startPos2 = startPos1;
                startPos2.xz += MathUtils.Left(float3.xz) * (compositionData.m_Width * 0.5f);
                leftSegment.m_Left = NetUtils.StraightCurve(startPos2, startPos2 + float3);
                leftSegment.m_Right = NetUtils.StraightCurve(startPos1, startPos1 + float3);
                edgeIteratorValue1 = edgeIteratorValue3;
                distances.x = compositionData.m_Width * 0.5f;
                float3 startPos3 = startPos1;
                startPos3.xz += MathUtils.Right(float3.xz) * (compositionData.m_Width * 0.5f);
                rightSegment.m_Left = NetUtils.StraightCurve(startPos1, startPos1 + float3);
                rightSegment.m_Right = NetUtils.StraightCurve(startPos3, startPos3 + float3);
                edgeIteratorValue2 = edgeIteratorValue3;
                distances.y = compositionData.m_Width * 0.5f;
              }
            }
          }
          else if (!edgeIteratorValue1.m_Middle)
          {
            // ISSUE: reference to a compiler-generated field
            Composition composition = this.m_CompositionDataFromEntity[edgeIteratorValue3.m_Edge];
            // ISSUE: reference to a compiler-generated field
            EdgeGeometry edgeGeometry = this.m_GeometryDataFromEntity[edgeIteratorValue3.m_Edge];
            // ISSUE: reference to a compiler-generated field
            NetCompositionData edgeCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            Segment segment;
            NetCompositionData nodeCompositionData;
            if (edgeIteratorValue3.m_End)
            {
              // ISSUE: reference to a compiler-generated method
              segment = this.Invert(edgeGeometry.m_End);
              // ISSUE: reference to a compiler-generated field
              nodeCompositionData = this.m_PrefabCompositionData[composition.m_EndNode];
            }
            else
            {
              segment = edgeGeometry.m_Start;
              // ISSUE: reference to a compiler-generated field
              nodeCompositionData = this.m_PrefabCompositionData[composition.m_StartNode];
            }
            nodeCompositionData.m_MiddleOffset = -nodeCompositionData.m_MiddleOffset;
            // ISSUE: reference to a compiler-generated method
            this.AdjustSegmentWidth(ref segment, edgeCompositionData, nodeCompositionData, edgeIteratorValue3.m_End);
            float t = (float) (0.5 + (double) nodeCompositionData.m_MiddleOffset / (double) nodeCompositionData.m_Width);
            // ISSUE: reference to a compiler-generated method
            float2 y = this.StartOffset(segment, middlePos);
            if (edgeIteratorValue3.m_Edge == edge)
            {
              if ((double) num1 < -1.0)
              {
                if (prefabGeometryData.m_MergeLayers == Layer.None | flag)
                {
                  leftSegment.m_Left = MathUtils.StartReflect(segment.m_Right);
                  leftSegment.m_Right = MathUtils.StartReflect(segment.m_Left);
                  t = 1f - t;
                }
                else
                  leftSegment = segment;
                leftSegment.m_Right = MathUtils.Lerp(leftSegment.m_Left, leftSegment.m_Right, t);
                edgeIteratorValue1 = edgeIteratorValue3;
                distances.x = nodeCompositionData.m_Width * 0.5f + nodeCompositionData.m_MiddleOffset;
              }
              if ((double) num2 > 1.0)
              {
                if (prefabGeometryData.m_MergeLayers == Layer.None | flag)
                {
                  rightSegment.m_Left = MathUtils.StartReflect(segment.m_Right);
                  rightSegment.m_Right = MathUtils.StartReflect(segment.m_Left);
                  t = 1f - t;
                }
                else
                  rightSegment = segment;
                rightSegment.m_Left = MathUtils.Lerp(rightSegment.m_Left, rightSegment.m_Right, t);
                edgeIteratorValue2 = edgeIteratorValue3;
                distances.y = nodeCompositionData.m_Width * 0.5f - nodeCompositionData.m_MiddleOffset;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefDataFromEntity[edgeIteratorValue3.m_Edge].m_Prefab];
              if ((netGeometryData.m_MergeLayers & prefabGeometryData.m_MergeLayers) == Layer.None)
              {
                if ((prefabGeometryData.m_MergeLayers & (Layer.Pathway | Layer.MarkerPathway)) != Layer.None && (netGeometryData.m_MergeLayers & Layer.Road) != Layer.None)
                  sideConnect = (bool2) true;
                else
                  continue;
              }
              float num4;
              if ((double) math.dot(offset, y) < 0.0)
              {
                num4 = math.dot(x, y) * 0.5f;
              }
              else
              {
                float num5 = math.dot(x, y);
                num4 = math.select(-1f, 1f, (double) num5 >= 0.0) - num5 * 0.5f;
              }
              if ((double) num4 > (double) num1)
              {
                num1 = num4;
                leftSegment = segment;
                leftSegment.m_Right = MathUtils.Lerp(leftSegment.m_Left, leftSegment.m_Right, t);
                edgeIteratorValue1 = edgeIteratorValue3;
                distances.x = nodeCompositionData.m_Width * 0.5f + nodeCompositionData.m_MiddleOffset;
              }
              if ((double) num4 < (double) num2)
              {
                num2 = num4;
                rightSegment = segment;
                rightSegment.m_Left = MathUtils.Lerp(rightSegment.m_Left, rightSegment.m_Right, t);
                edgeIteratorValue2 = edgeIteratorValue3;
                distances.y = nodeCompositionData.m_Width * 0.5f - nodeCompositionData.m_MiddleOffset;
              }
            }
            roundaboutSize = !edgeIteratorValue3.m_End ? math.max(roundaboutSize, nodeCompositionData.m_RoundaboutSize.x) : math.max(roundaboutSize, nodeCompositionData.m_RoundaboutSize.y);
          }
        }
        middleConnect = new bool2(edgeIteratorValue1.m_Middle, edgeIteratorValue2.m_Middle);
        if (edgeIteratorValue1.m_Edge != Entity.Null && !sideConnect.x && !edgeIteratorValue1.m_Middle)
        {
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionDataFromEntity[edgeIteratorValue1.m_Edge];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[edgeIteratorValue1.m_End ? composition.m_EndNode : composition.m_StartNode];
          syncVertexOffsetsLeft = 1f - netCompositionData.m_SyncVertexOffsetsRight.wzyx;
        }
        else
          syncVertexOffsetsLeft = new float4(0.0f, 0.333333343f, 0.6666667f, 1f);
        if (edgeIteratorValue2.m_Edge != Entity.Null && !sideConnect.y && !edgeIteratorValue2.m_Middle)
        {
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionDataFromEntity[edgeIteratorValue2.m_Edge];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[edgeIteratorValue2.m_End ? composition.m_EndNode : composition.m_StartNode];
          syncVertexOffsetsRight = 1f - netCompositionData.m_SyncVertexOffsetsLeft.wzyx;
        }
        else
          syncVertexOffsetsRight = new float4(0.0f, 0.333333343f, 0.6666667f, 1f);
      }

      private void CalculateSegments(
        Segment startLeftSegment,
        Segment startRightSegment,
        Segment endLeftSegment,
        Segment endRightSegment,
        float3 nodePosition,
        ref float leftRadius,
        ref float rightRadius,
        out Segment startSegment,
        out Segment endSegment,
        out float2 leftDivPos,
        out float2 rightDivPos)
      {
        float divPos1;
        // ISSUE: reference to a compiler-generated method
        this.CalculateMiddleCurves(startLeftSegment.m_Left, endLeftSegment.m_Left, nodePosition, ref leftRadius, false, out startSegment.m_Left, out endSegment.m_Left, out divPos1);
        float divPos2;
        // ISSUE: reference to a compiler-generated method
        this.CalculateMiddleCurves(startRightSegment.m_Right, endRightSegment.m_Right, nodePosition, ref rightRadius, true, out startSegment.m_Right, out endSegment.m_Right, out divPos2);
        if ((double) math.distancesq(endSegment.m_Left.d, endSegment.m_Right.d) < 0.10000000149011612)
        {
          endSegment.m_Left.d = math.lerp(endSegment.m_Left.d, endSegment.m_Right.d, 0.5f);
          endSegment.m_Right.d = endSegment.m_Left.d;
        }
        startSegment.m_Length.x = MathUtils.Length(startSegment.m_Left);
        startSegment.m_Length.y = MathUtils.Length(startSegment.m_Right);
        endSegment.m_Length.x = MathUtils.Length(endSegment.m_Left);
        endSegment.m_Length.y = MathUtils.Length(endSegment.m_Right);
        leftDivPos = (float2) divPos1;
        rightDivPos = (float2) divPos2;
      }

      private void CalculateMiddleCurves(
        Bezier4x3 startCurve,
        Bezier4x3 endCurve,
        float3 nodePosition,
        ref float radius,
        bool right,
        out Bezier4x3 startMiddleCurve,
        out Bezier4x3 endMiddleCurve,
        out float divPos)
      {
        float2 float2_1 = math.normalizesafe(startCurve.d.xz - nodePosition.xz);
        float2 toVector = math.normalizesafe(endCurve.a.xz - nodePosition.xz);
        float3 endTangent = new float3();
        float3 float3_1 = new float3();
        float angle1;
        float2 float2_2;
        float2 forward;
        if (right)
        {
          float angle2 = MathUtils.RotationAngleLeft(float2_1, toVector) * 0.5f;
          angle1 = math.max(math.min(angle2 * 0.5f, 0.3926991f), angle2 - 1.57079637f);
          float2_2 = MathUtils.RotateLeft(float2_1, angle2);
          forward = MathUtils.RotateLeft(float2_1, angle1);
          endTangent.xz = MathUtils.Left(float2_2);
          float3_1.xz = MathUtils.Left(forward);
        }
        else
        {
          float angle3 = MathUtils.RotationAngleRight(float2_1, toVector) * 0.5f;
          angle1 = math.max(math.min(angle3 * 0.5f, 0.3926991f), angle3 - 1.57079637f);
          float2_2 = MathUtils.RotateRight(float2_1, angle3);
          forward = MathUtils.RotateRight(float2_1, angle1);
          endTangent.xz = MathUtils.Right(float2_2);
          float3_1.xz = MathUtils.Right(forward);
        }
        float3 float3_2 = MathUtils.EndTangent(startCurve);
        float3 startTangent = MathUtils.Normalize(float3_2, float3_2.xz);
        float middleDistance;
        float divPos1;
        // ISSUE: reference to a compiler-generated method
        Bezier4x3 middleCurve = this.CalculateMiddleCurve(startCurve, endCurve, nodePosition, out middleDistance, out divPos1);
        Bezier4x3 output1;
        Bezier4x3 output2;
        MathUtils.Divide(middleCurve, out output1, out output2, 0.5f);
        output1.c.y = output2.d.y;
        output1.d.y = output2.d.y;
        output2.a.y = output2.d.y;
        output2.b.y = output2.d.y;
        output2.c.y = output2.d.y;
        float num1 = math.select(middleDistance, 0.0f, (double) math.dot(middleCurve.d.xz - nodePosition.xz, float2_2) <= 0.0);
        float num2 = math.smoothstep(radius, radius * 1.2f, num1);
        float num3 = math.max(radius, num1);
        float3 d = startCurve.d;
        float3 endPos = nodePosition;
        float3 float3_3 = nodePosition;
        endPos.xz += float2_2 * num3;
        float3_3.xz += forward * num3;
        startMiddleCurve = NetUtils.FitCurve(d, startTangent, float3_1, float3_3);
        endMiddleCurve = NetUtils.FitCurve(float3_3, float3_1, endTangent, endPos);
        divPos = 0.5f;
        float num4 = math.max(0.0f, (float) (((double) angle1 - 0.39269909262657166) * 0.84882634878158569));
        startMiddleCurve.b += (startMiddleCurve.a - startMiddleCurve.b) * (num4 * 0.5f);
        startMiddleCurve.c += (startMiddleCurve.c - startMiddleCurve.d) * num4;
        startMiddleCurve = MathUtils.Lerp(startMiddleCurve, output1, num2);
        endMiddleCurve = MathUtils.Lerp(endMiddleCurve, output2, num2);
        divPos = math.lerp(divPos, divPos1, num2);
      }

      private Segment CalculateSideConnection(
        Segment startSegment,
        Segment endLeftSegment,
        Segment endRightSegment,
        NetCompositionData edgeCompositionData,
        NetCompositionData nodeCompositionData,
        bool right,
        out float2 divPos)
      {
        float3 float3_1 = -MathUtils.StartTangent(endLeftSegment.m_Left);
        float3 float3_2 = MathUtils.StartTangent(endRightSegment.m_Right);
        float3 startTangent1 = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 endTangent1 = MathUtils.Normalize(float3_2, float3_2.xz);
        startTangent1.y = math.clamp(startTangent1.y, -1f, 1f);
        endTangent1.y = math.clamp(endTangent1.y, -1f, 1f);
        Bezier4x3 curve = NetUtils.FitCurve(endLeftSegment.m_Left.a, startTangent1, endTangent1, endRightSegment.m_Right.a);
        float t1;
        double num1 = (double) MathUtils.Distance(curve, startSegment.m_Left.d, out t1);
        float t2;
        double num2 = (double) MathUtils.Distance(curve, startSegment.m_Right.d, out t2);
        float num3 = math.max(edgeCompositionData.m_NodeOffset, nodeCompositionData.m_NodeOffset);
        float3 endTangent2;
        float3 endTangent3;
        if (right)
        {
          Bounds1 t3 = new Bounds1(t2, 1f);
          MathUtils.ClampLength(curve, ref t3, num3 * 0.5f);
          t2 = t3.min;
          endTangent2 = MathUtils.Tangent(curve, t1);
          endTangent2.xz = math.normalizesafe(MathUtils.Left(endTangent2.xz));
          endTangent2.y = 0.0f;
          endTangent3 = MathUtils.Tangent(curve, t2);
          endTangent3.xz = math.normalizesafe(MathUtils.Left(endTangent3.xz) + endTangent3.xz);
          endTangent3.y = math.clamp(endTangent3.y * 0.5f, -1f, 1f);
        }
        else
        {
          Bounds1 t4 = new Bounds1(0.0f, t1);
          MathUtils.ClampLengthInverse(curve, ref t4, num3 * 0.5f);
          t1 = t4.max;
          endTangent2 = MathUtils.Tangent(curve, t2);
          endTangent2.xz = math.normalizesafe(MathUtils.Left(endTangent2.xz) - endTangent2.xz);
          endTangent2.y = math.clamp(endTangent2.y * 0.5f, -1f, 1f);
          endTangent3 = MathUtils.Tangent(curve, t2);
          endTangent3.xz = math.normalizesafe(MathUtils.Left(endTangent3.xz));
          endTangent3.y = 0.0f;
        }
        float3 endPos1 = MathUtils.Position(curve, t1);
        float3 endPos2 = MathUtils.Position(curve, t2);
        float3 float3_3 = MathUtils.EndTangent(startSegment.m_Left);
        float3 float3_4 = MathUtils.EndTangent(startSegment.m_Right);
        float3 startTangent2 = MathUtils.Normalize(float3_3, float3_3.xz);
        float3 startTangent3 = MathUtils.Normalize(float3_4, float3_4.xz);
        startTangent2.y = math.clamp(startTangent2.y, -1f, 1f);
        startTangent3.y = math.clamp(startTangent3.y, -1f, 1f);
        Segment sideConnection;
        sideConnection.m_Left = NetUtils.FitCurve(startSegment.m_Left.d, startTangent2, endTangent2, endPos1);
        sideConnection.m_Right = NetUtils.FitCurve(startSegment.m_Right.d, startTangent3, endTangent3, endPos2);
        sideConnection.m_Length.x = MathUtils.Length(sideConnection.m_Left);
        sideConnection.m_Length.y = MathUtils.Length(sideConnection.m_Right);
        divPos = (float2) 0.0f;
        return sideConnection;
      }

      private Segment CalculateMiddleConnection(
        Segment startSegment,
        Segment endSegment,
        out float2 divPos)
      {
        Segment middleConnection;
        // ISSUE: reference to a compiler-generated method
        middleConnection.m_Left = this.CalculateMiddleConnection(startSegment.m_Left, endSegment.m_Left, out divPos.x);
        // ISSUE: reference to a compiler-generated method
        middleConnection.m_Right = this.CalculateMiddleConnection(startSegment.m_Right, endSegment.m_Right, out divPos.y);
        middleConnection.m_Length.x = MathUtils.Length(middleConnection.m_Left);
        middleConnection.m_Length.y = MathUtils.Length(middleConnection.m_Right);
        return middleConnection;
      }

      private Bezier4x3 CalculateMiddleConnection(
        Bezier4x3 startCurve,
        Bezier4x3 endCurve,
        out float divPos)
      {
        float3 float3_1 = MathUtils.EndTangent(startCurve);
        float3 float3_2 = MathUtils.StartTangent(endCurve);
        float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 endTangent = MathUtils.Normalize(float3_2, float3_2.xz);
        startTangent.y = math.clamp(startTangent.y, -1f, 1f);
        endTangent.y = math.clamp(endTangent.y, -1f, 1f);
        divPos = 0.0f;
        return NetUtils.FitCurve(startCurve.d, startTangent, endTangent, endCurve.a);
      }

      private Segment CalculateMiddleSegment(
        Segment startSegment,
        Segment endSegment,
        out float2 divPos)
      {
        Segment middleSegment;
        // ISSUE: reference to a compiler-generated method
        middleSegment.m_Left = this.CalculateMiddleCurve(startSegment.m_Left, endSegment.m_Left, out divPos.x);
        // ISSUE: reference to a compiler-generated method
        middleSegment.m_Right = this.CalculateMiddleCurve(startSegment.m_Right, endSegment.m_Right, out divPos.y);
        middleSegment.m_Length.x = MathUtils.Length(middleSegment.m_Left);
        middleSegment.m_Length.y = MathUtils.Length(middleSegment.m_Right);
        return middleSegment;
      }

      private Bezier4x3 CalculateMiddleCurve(
        Bezier4x3 startCurve,
        Bezier4x3 endCurve,
        out float divPos)
      {
        float3 float3_1 = MathUtils.EndTangent(startCurve);
        float3 float3_2 = MathUtils.StartTangent(endCurve);
        float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 endTangent1 = MathUtils.Normalize(float3_2, float3_2.xz);
        startTangent.y = math.clamp(startTangent.y, -1f, 1f);
        endTangent1.y = math.clamp(endTangent1.y, -1f, 1f);
        Bezier4x3 curve = NetUtils.FitCurve(startCurve.d, startTangent, endTangent1, endCurve.a);
        divPos = NetUtils.FindMiddleTangentPos(curve.xz, new float2(0.0f, 1f));
        float3 endPos = MathUtils.Position(curve, divPos);
        float3 float3_3 = MathUtils.Tangent(curve, divPos);
        float3 endTangent2 = MathUtils.Normalize(float3_3, float3_3.xz);
        endTangent2.y = math.clamp(endTangent2.y, -1f, 1f);
        return NetUtils.FitCurve(startCurve.d, startTangent, endTangent2, endPos);
      }

      private Bezier4x3 CalculateMiddleCurve(
        Bezier4x3 startCurve,
        Bezier4x3 endCurve,
        float3 middlePosition,
        out float middleDistance,
        out float divPos)
      {
        float3 float3_1 = MathUtils.EndTangent(startCurve);
        float3 float3_2 = MathUtils.StartTangent(endCurve);
        float3 startTangent = MathUtils.Normalize(float3_1, float3_1.xz);
        float3 endTangent1 = MathUtils.Normalize(float3_2, float3_2.xz);
        startTangent.y = math.clamp(startTangent.y, -1f, 1f);
        endTangent1.y = math.clamp(endTangent1.y, -1f, 1f);
        Bezier4x3 curve = NetUtils.FitCurve(startCurve.d, startTangent, endTangent1, endCurve.a);
        middleDistance = MathUtils.Distance(curve.xz, middlePosition.xz, out float _);
        divPos = NetUtils.FindMiddleTangentPos(curve.xz, new float2(0.0f, 1f));
        float3 endPos = MathUtils.Position(curve, divPos);
        float3 float3_3 = MathUtils.Tangent(curve, divPos);
        float3 endTangent2 = MathUtils.Normalize(float3_3, float3_3.xz);
        endTangent2.y = math.clamp(endTangent2.y, -1f, 1f);
        return NetUtils.FitCurve(startCurve.d, startTangent, endTangent2, endPos);
      }
    }

    private struct IntersectionData
    {
      public Bezier4x3 m_StartMiddle;
      public Bezier4x3 m_EndMiddle;
      public Bounds3 m_StartBounds;
      public Bounds3 m_EndBounds;
    }

    [BurstCompile]
    private struct CalculateIntersectionGeometryJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeDataFromEntity;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartGeometryDataFromEntity;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndGeometryDataFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [NativeDisableParallelForRestriction]
      public NativeList<GeometrySystem.IntersectionData> m_BufferedData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        Edge edge = this.m_EdgeDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_CompositionData[entity];
        // ISSUE: reference to a compiler-generated field
        StartNodeGeometry startNodeGeometry = this.m_StartGeometryDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        EndNodeGeometry endNodeGeometry = this.m_EndGeometryDataFromEntity[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefData[entity].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_PrefabCompositionData[composition.m_EndNode];
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GeometrySystem.IntersectionData intersectionData = new GeometrySystem.IntersectionData();
        // ISSUE: reference to a compiler-generated field
        NodeGeometry nodeGeometry1 = this.m_NodeGeometryData[edge.m_Start];
        // ISSUE: reference to a compiler-generated field
        NodeGeometry nodeGeometry2 = this.m_NodeGeometryData[edge.m_End];
        bool edges1 = (netCompositionData1.m_Flags.m_General & (CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing)) > (CompositionFlags.General) 0;
        bool edges2 = (netCompositionData2.m_Flags.m_General & (CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing)) > (CompositionFlags.General) 0;
        float3 float3_1;
        if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float3_1 = this.m_NodeDataFromEntity[edge.m_Start].m_Position with
          {
            y = nodeGeometry1.m_Position
          };
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          float3_1 = this.FindIntersectionPos(entity, edge.m_Start, prefabGeometryData);
          if (edges1)
            float3_1.y = nodeGeometry1.m_Position;
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref startNodeGeometry.m_Geometry, float3_1, edges1);
        }
        float3 float3_2;
        if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          float3_2 = this.m_NodeDataFromEntity[edge.m_End].m_Position with
          {
            y = nodeGeometry2.m_Position
          };
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          float3_2 = this.FindIntersectionPos(entity, edge.m_End, prefabGeometryData);
          if (edges2)
            float3_2.y = nodeGeometry2.m_Position;
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref endNodeGeometry.m_Geometry, float3_2, edges2);
        }
        if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          float t = (float) ((double) netCompositionData1.m_MiddleOffset / (double) netCompositionData1.m_Width + 0.5);
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_StartMiddle = MathUtils.Lerp(startNodeGeometry.m_Geometry.m_Left.m_Left, startNodeGeometry.m_Geometry.m_Left.m_Right, t);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.MoveEndTo(ref intersectionData.m_StartMiddle, float3_1);
        }
        else
        {
          int num = math.all(startNodeGeometry.m_Geometry.m_Middle.d.xy != 0.0f) ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_StartMiddle = MathUtils.Lerp(startNodeGeometry.m_Geometry.m_Left.m_Right, startNodeGeometry.m_Geometry.m_Right.m_Left, 0.5f);
          if (num == 0)
          {
            // ISSUE: reference to a compiler-generated field
            intersectionData.m_StartMiddle.d = float3_1;
          }
        }
        if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
        {
          float t = (float) ((double) netCompositionData2.m_MiddleOffset / (double) netCompositionData2.m_Width + 0.5);
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_EndMiddle = MathUtils.Lerp(endNodeGeometry.m_Geometry.m_Left.m_Left, endNodeGeometry.m_Geometry.m_Left.m_Right, t);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.MoveEndTo(ref intersectionData.m_EndMiddle, float3_2);
        }
        else
        {
          int num = math.all(endNodeGeometry.m_Geometry.m_Middle.d.xy != 0.0f) ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_EndMiddle = MathUtils.Lerp(endNodeGeometry.m_Geometry.m_Left.m_Right, endNodeGeometry.m_Geometry.m_Right.m_Left, 0.5f);
          if (num == 0)
          {
            // ISSUE: reference to a compiler-generated field
            intersectionData.m_EndMiddle.d = float3_2;
          }
        }
        if (prefabGeometryData.m_MergeLayers == Layer.None)
        {
          float num1 = netCompositionData1.m_Width * 0.5f;
          float num2 = netCompositionData2.m_Width * 0.5f;
          float3 float3_3 = math.lerp(startNodeGeometry.m_Geometry.m_Left.m_Left.a, startNodeGeometry.m_Geometry.m_Right.m_Right.a, 0.5f);
          float3 float3_4 = math.lerp(endNodeGeometry.m_Geometry.m_Left.m_Left.a, endNodeGeometry.m_Geometry.m_Right.m_Right.a, 0.5f);
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_StartBounds = MathUtils.Bounds(float3_3, float3_3);
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_EndBounds = MathUtils.Bounds(float3_4, float3_4);
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_StartBounds.min.xz -= num1;
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_StartBounds.max.xz += num1;
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_EndBounds.min.xz -= num2;
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_EndBounds.max.xz += num2;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_StartBounds = MathUtils.TightBounds(startNodeGeometry.m_Geometry.m_Left.m_Left) | MathUtils.TightBounds(startNodeGeometry.m_Geometry.m_Left.m_Right) | MathUtils.TightBounds(startNodeGeometry.m_Geometry.m_Right.m_Left) | MathUtils.TightBounds(startNodeGeometry.m_Geometry.m_Right.m_Right) | MathUtils.TightBounds(intersectionData.m_StartMiddle);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          intersectionData.m_EndBounds = MathUtils.TightBounds(endNodeGeometry.m_Geometry.m_Left.m_Left) | MathUtils.TightBounds(endNodeGeometry.m_Geometry.m_Left.m_Right) | MathUtils.TightBounds(endNodeGeometry.m_Geometry.m_Right.m_Left) | MathUtils.TightBounds(endNodeGeometry.m_Geometry.m_Right.m_Right) | MathUtils.TightBounds(intersectionData.m_EndMiddle);
        }
        // ISSUE: reference to a compiler-generated field
        intersectionData.m_StartBounds.min.y += netCompositionData1.m_HeightRange.min;
        // ISSUE: reference to a compiler-generated field
        intersectionData.m_StartBounds.max.y += netCompositionData1.m_HeightRange.max;
        // ISSUE: reference to a compiler-generated field
        intersectionData.m_EndBounds.min.y += netCompositionData2.m_HeightRange.min;
        // ISSUE: reference to a compiler-generated field
        intersectionData.m_EndBounds.max.y += netCompositionData2.m_HeightRange.max;
        if ((netCompositionData1.m_State & (CompositionState.LowerToTerrain | CompositionState.RaiseToTerrain)) != (CompositionState) 0)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          Bounds1 bounds1 = this.SampleTerrain(startNodeGeometry.m_Geometry.m_Left.m_Left) | this.SampleTerrain(startNodeGeometry.m_Geometry.m_Right.m_Right);
          if ((double) startNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            bounds1 |= this.SampleTerrain(startNodeGeometry.m_Geometry.m_Left.m_Right) | this.SampleTerrain(startNodeGeometry.m_Geometry.m_Right.m_Left);
          }
          if ((netCompositionData1.m_State & CompositionState.LowerToTerrain) != (CompositionState) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            intersectionData.m_StartBounds.min.y = math.min(intersectionData.m_StartBounds.min.y, bounds1.min);
          }
          if ((netCompositionData1.m_State & CompositionState.RaiseToTerrain) != (CompositionState) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            intersectionData.m_StartBounds.max.y = math.max(intersectionData.m_StartBounds.max.y, bounds1.max);
          }
        }
        if ((netCompositionData2.m_State & (CompositionState.LowerToTerrain | CompositionState.RaiseToTerrain)) != (CompositionState) 0)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          Bounds1 bounds1 = this.SampleTerrain(endNodeGeometry.m_Geometry.m_Left.m_Left) | this.SampleTerrain(endNodeGeometry.m_Geometry.m_Right.m_Right);
          if ((double) endNodeGeometry.m_Geometry.m_MiddleRadius > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            bounds1 |= this.SampleTerrain(endNodeGeometry.m_Geometry.m_Left.m_Right) | this.SampleTerrain(endNodeGeometry.m_Geometry.m_Right.m_Left);
          }
          if ((netCompositionData2.m_State & CompositionState.LowerToTerrain) != (CompositionState) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            intersectionData.m_EndBounds.min.y = math.min(intersectionData.m_EndBounds.min.y, bounds1.min);
          }
          if ((netCompositionData2.m_State & CompositionState.RaiseToTerrain) != (CompositionState) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            intersectionData.m_EndBounds.max.y = math.max(intersectionData.m_EndBounds.max.y, bounds1.max);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_BufferedData[index] = intersectionData;
      }

      private Bounds1 SampleTerrain(Bezier4x3 curve)
      {
        Bounds1 bounds1 = new Bounds1(float.MaxValue, float.MinValue);
        for (int index = 0; index <= 8; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          bounds1 |= TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, MathUtils.Position(curve, (float) index * 0.125f));
        }
        return bounds1;
      }

      private void MoveEndTo(ref Bezier4x3 curve, float3 pos)
      {
        float num = math.distance(curve.d, pos);
        curve.b += math.normalizesafe(curve.b - curve.a) * (num * 0.333333343f);
        curve.c = pos + (curve.c - curve.d) + math.normalizesafe(curve.c - curve.d) * (num * 0.333333343f);
        curve.d = pos;
      }

      private float3 FindIntersectionPos(
        Entity edge,
        Entity node,
        NetGeometryData prefabGeometryData)
      {
        float3 intersectionPos = new float3();
        float num = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeIterator edgeIterator = new EdgeIterator(edge, node, this.m_Edges, this.m_EdgeDataFromEntity, this.m_TempData, this.m_HiddenData);
        EdgeIteratorValue edgeIteratorValue;
        while (edgeIterator.GetNext(out edgeIteratorValue))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry edgeNodeGeometry = !edgeIteratorValue.m_End ? this.m_StartGeometryDataFromEntity[edgeIteratorValue.m_Edge].m_Geometry : this.m_EndGeometryDataFromEntity[edgeIteratorValue.m_Edge].m_Geometry;
          if (edgeIteratorValue.m_Edge != edge)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData = this.m_PrefabGeometryData[this.m_PrefabRefData[edgeIteratorValue.m_Edge].m_Prefab];
            if ((prefabGeometryData.m_MergeLayers & netGeometryData.m_MergeLayers) == Layer.None)
              continue;
          }
          intersectionPos += math.lerp(edgeNodeGeometry.m_Left.m_Right.d, edgeNodeGeometry.m_Right.m_Left.d, 0.5f);
          ++num;
        }
        if ((double) num != 0.0)
          intersectionPos /= num;
        return intersectionPos;
      }

      private void Flatten(ref EdgeNodeGeometry geometry, float3 middlePos, bool edges)
      {
        if (edges)
        {
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref geometry.m_Left.m_Left, middlePos.y);
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref geometry.m_Left.m_Right, middlePos.y);
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref geometry.m_Right.m_Left, middlePos.y);
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref geometry.m_Right.m_Right, middlePos.y);
        }
        else
        {
          float num1 = math.distance(geometry.m_Left.m_Left.d.xz, middlePos.xz);
          float num2 = math.distance(geometry.m_Left.m_Right.d.xz, middlePos.xz);
          float num3 = math.distance(geometry.m_Right.m_Left.d.xz, middlePos.xz);
          float num4 = math.distance(geometry.m_Right.m_Right.d.xz, middlePos.xz);
          float middleHeight1 = math.lerp(middlePos.y, geometry.m_Left.m_Left.d.y, math.saturate(num2 / num1));
          float middleHeight2 = math.lerp(middlePos.y, geometry.m_Right.m_Right.d.y, math.saturate(num3 / num4));
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref geometry.m_Left.m_Right, middleHeight1);
          // ISSUE: reference to a compiler-generated method
          this.Flatten(ref geometry.m_Right.m_Left, middleHeight2);
        }
      }

      private void Flatten(ref Bezier4x3 curve, float middleHeight)
      {
        curve.c.y += middleHeight - curve.d.y;
        curve.d.y = middleHeight;
      }
    }

    [BurstCompile]
    private struct CopyNodeGeometryJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public NativeList<GeometrySystem.IntersectionData> m_BufferedData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        GeometrySystem.IntersectionData intersectionData = this.m_BufferedData[index];
        // ISSUE: reference to a compiler-generated field
        StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[entity];
        // ISSUE: reference to a compiler-generated field
        startNodeGeometry.m_Geometry.m_Middle = intersectionData.m_StartMiddle;
        // ISSUE: reference to a compiler-generated field
        startNodeGeometry.m_Geometry.m_Bounds = intersectionData.m_StartBounds;
        // ISSUE: reference to a compiler-generated field
        this.m_StartNodeGeometryData[entity] = startNodeGeometry;
        // ISSUE: reference to a compiler-generated field
        EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[entity];
        // ISSUE: reference to a compiler-generated field
        endNodeGeometry.m_Geometry.m_Middle = intersectionData.m_EndMiddle;
        // ISSUE: reference to a compiler-generated field
        endNodeGeometry.m_Geometry.m_Bounds = intersectionData.m_EndBounds;
        // ISSUE: reference to a compiler-generated field
        this.m_EndNodeGeometryData[entity] = endNodeGeometry;
      }
    }

    [BurstCompile]
    private struct UpdateNodeGeometryJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public ComponentTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> m_OrphanType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      public ComponentTypeHandle<NodeGeometry> m_NodeGeometryType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Node> nativeArray2 = chunk.GetNativeArray<Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Orphan> nativeArray3 = chunk.GetNativeArray<Orphan>(ref this.m_OrphanType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NodeGeometry> nativeArray4 = chunk.GetNativeArray<NodeGeometry>(ref this.m_NodeGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          Entity node1 = nativeArray1[index];
          Node node2 = nativeArray2[index];
          Bounds3 bounds3 = new Bounds3();
          if (nativeArray3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[nativeArray3[index].m_Composition];
            float num = netCompositionData.m_Width * 0.5f;
            bounds3.xz = new Bounds2(node2.m_Position.xz - num, node2.m_Position.xz + num);
            bounds3.y = node2.m_Position.y + netCompositionData.m_HeightRange;
            if ((netCompositionData.m_State & (CompositionState.LowerToTerrain | CompositionState.RaiseToTerrain)) != (CompositionState) 0)
            {
              Bounds1 bounds1 = new Bounds1(float.MaxValue, float.MinValue);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bounds1 = bounds1 | TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, new float3(node2.m_Position.xy, bounds3.min.z)) | TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, new float3(bounds3.min.x, node2.m_Position.yz)) | TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, new float3(bounds3.max.x, node2.m_Position.yz)) | TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, new float3(node2.m_Position.xy, bounds3.max.z));
              if ((netCompositionData.m_State & CompositionState.LowerToTerrain) != (CompositionState) 0)
                bounds3.min.y = math.min(bounds3.min.y, bounds1.min);
              if ((netCompositionData.m_State & CompositionState.RaiseToTerrain) != (CompositionState) 0)
                bounds3.max.y = math.max(bounds3.max.y, bounds1.max);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData = this.m_PrefabGeometryData[nativeArray5[index].m_Prefab];
            float num = netGeometryData.m_DefaultWidth * 0.5f;
            bounds3.xz = new Bounds2(node2.m_Position.xz - num, node2.m_Position.xz + num);
            bounds3.y = node2.m_Position.y + netGeometryData.m_DefaultHeightRange;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EdgeIterator edgeIterator = new EdgeIterator(Entity.Null, node1, this.m_ConnectedEdges, this.m_EdgeData, this.m_TempData, this.m_HiddenData);
          EdgeIteratorValue edgeIteratorValue;
          while (edgeIterator.GetNext(out edgeIteratorValue))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EdgeNodeGeometry edgeNodeGeometry = !edgeIteratorValue.m_End ? this.m_StartNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry : this.m_EndNodeGeometryData[edgeIteratorValue.m_Edge].m_Geometry;
            bounds3 |= edgeNodeGeometry.m_Bounds;
          }
          NodeGeometry nodeGeometry = nativeArray4[index] with
          {
            m_Bounds = bounds3
          };
          nativeArray4[index] = nodeGeometry;
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> __Game_Prefabs_NetCompositionLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionCrosswalk> __Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup;
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RW_ComponentLookup;
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RW_ComponentLookup;
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RW_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Orphan> __Game_Net_Orphan_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NodeGeometry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionLane_RO_BufferLookup = state.GetBufferLookup<NetCompositionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionCrosswalk_RO_BufferLookup = state.GetBufferLookup<NetCompositionCrosswalk>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RW_ComponentLookup = state.GetComponentLookup<EdgeGeometry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RW_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RW_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Orphan>(true);
      }
    }
  }
}
