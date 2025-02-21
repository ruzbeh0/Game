// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchInstanceSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Colossal.Rendering;
using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class BatchInstanceSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private BatchManagerSystem m_BatchManagerSystem;
    private PreCullingSystem m_PreCullingSystem;
    private UndergroundViewSystem m_UndergroundViewSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ToolSystem m_ToolSystem;
    private BatchInstanceSystem.Groups m_Groups;
    private BatchInstanceSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatchManagerSystem = this.World.GetOrCreateSystemManaged<BatchManagerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UndergroundViewSystem = this.World.GetOrCreateSystemManaged<UndergroundViewSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Groups = this.World.GetOrCreateSystemManaged<BatchInstanceSystem.Groups>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> nativeBatchInstances = this.m_BatchManagerSystem.GetNativeBatchInstances(true, out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Groups.m_GroupActionQueue = new NativeParallelQueue<BatchInstanceSystem.GroupActionData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Groups.m_VelocityQueue = new NativeQueue<BatchInstanceSystem.VelocityData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Groups.m_FadeQueue = new NativeQueue<BatchInstanceSystem.FadeData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BatchGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Marker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Marker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_NetObject_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_FadeBatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Override_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Warning_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Error_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatchInstanceSystem.BatchInstanceJob jobData = new BatchInstanceSystem.BatchInstanceJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_ErrorData = this.__TypeHandle.__Game_Tools_Error_RO_ComponentLookup,
        m_WarningData = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentLookup,
        m_OverrideData = this.__TypeHandle.__Game_Tools_Override_RO_ComponentLookup,
        m_HighlightedData = this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_MeshGroups = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferLookup,
        m_MeshBatches = this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferLookup,
        m_FadeBatches = this.__TypeHandle.__Game_Rendering_FadeBatch_RW_BufferLookup,
        m_StoppedData = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RO_ComponentLookup,
        m_NetObjectData = this.__TypeHandle.__Game_Objects_NetObject_RO_ComponentLookup,
        m_QuantityData = this.__TypeHandle.__Game_Objects_Quantity_RO_ComponentLookup,
        m_ObjectElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_ObjectMarkerData = this.__TypeHandle.__Game_Objects_Marker_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_UnderConstructionData = this.__TypeHandle.__Game_Objects_UnderConstruction_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_NetOutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_UtilityLaneData = this.__TypeHandle.__Game_Net_UtilityLane_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_NetMarkerData = this.__TypeHandle.__Game_Net_Marker_RO_ComponentLookup,
        m_CutRanges = this.__TypeHandle.__Game_Net_CutRange_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ZoneBlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabGrowthScaleData = this.__TypeHandle.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabQuantityObjectData = this.__TypeHandle.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabMeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabUtilityLaneData = this.__TypeHandle.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup,
        m_PrefabCompositionMeshData = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup,
        m_PrefabCompositionMeshRef = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup,
        m_PrefabSubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_PrefabSubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_PrefabBatchGroups = this.__TypeHandle.__Game_Prefabs_BatchGroup_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_MarkersVisible = this.m_RenderingSystem.markersVisible,
        m_UnspawnedVisible = this.m_RenderingSystem.unspawnedVisible,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_UseLodFade = this.m_RenderingSystem.lodCrossFade,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_DilatedUtilityTypes = this.m_UndergroundViewSystem.utilityTypes,
        m_VisibleMask = this.m_PreCullingSystem.visibleMask,
        m_BecameVisible = this.m_PreCullingSystem.becameVisible,
        m_BecameHidden = this.m_PreCullingSystem.becameHidden,
        m_BatchInstances = nativeBatchInstances,
        m_CullingData = this.m_PreCullingSystem.GetUpdatedData(true, out dependencies2),
        m_GroupActionQueue = this.m_Groups.m_GroupActionQueue.AsWriter(),
        m_VelocityQueue = this.m_Groups.m_VelocityQueue.AsParallelWriter(),
        m_FadeQueue = this.m_Groups.m_FadeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData.Schedule<BatchInstanceSystem.BatchInstanceJob, PreCullingData>(jobData.m_CullingData, 4, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BatchManagerSystem.AddNativeBatchInstancesReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Groups.m_Dependency = jobHandle;
      this.Dependency = jobHandle;
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
    public BatchInstanceSystem()
    {
    }

    [BurstCompile]
    private struct BatchInstanceJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Error> m_ErrorData;
      [ReadOnly]
      public ComponentLookup<Warning> m_WarningData;
      [ReadOnly]
      public ComponentLookup<Override> m_OverrideData;
      [ReadOnly]
      public ComponentLookup<Highlighted> m_HighlightedData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public BufferLookup<MeshGroup> m_MeshGroups;
      [NativeDisableParallelForRestriction]
      public BufferLookup<MeshBatch> m_MeshBatches;
      [NativeDisableParallelForRestriction]
      public BufferLookup<FadeBatch> m_FadeBatches;
      [ReadOnly]
      public ComponentLookup<Stopped> m_StoppedData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Stack> m_StackData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.NetObject> m_NetObjectData;
      [ReadOnly]
      public ComponentLookup<Quantity> m_QuantityData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ObjectElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Marker> m_ObjectMarkerData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> m_UnderConstructionData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Net.OutsideConnection> m_NetOutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.UtilityLane> m_UtilityLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Marker> m_NetMarkerData;
      [ReadOnly]
      public BufferLookup<CutRange> m_CutRanges;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_ZoneBlockData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> m_PrefabGrowthScaleData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> m_PrefabQuantityObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_PrefabMeshData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> m_PrefabCompositionMeshData;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> m_PrefabCompositionMeshRef;
      [ReadOnly]
      public BufferLookup<SubMesh> m_PrefabSubMeshes;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_PrefabSubMeshGroups;
      [ReadOnly]
      public BufferLookup<BatchGroup> m_PrefabBatchGroups;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public bool m_MarkersVisible;
      [ReadOnly]
      public bool m_UnspawnedVisible;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public bool m_UseLodFade;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [ReadOnly]
      public UtilityTypes m_DilatedUtilityTypes;
      [ReadOnly]
      public BoundsMask m_VisibleMask;
      [ReadOnly]
      public BoundsMask m_BecameVisible;
      [ReadOnly]
      public BoundsMask m_BecameHidden;
      [ReadOnly]
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_BatchInstances;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public NativeParallelQueue<BatchInstanceSystem.GroupActionData>.Writer m_GroupActionQueue;
      public NativeQueue<BatchInstanceSystem.FadeData>.ParallelWriter m_FadeQueue;
      public NativeQueue<BatchInstanceSystem.VelocityData>.ParallelWriter m_VelocityQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        PreCullingData cullingData = this.m_CullingData[index];
        if ((cullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.BatchesUpdated | PreCullingFlags.FadeContainer)) == (PreCullingFlags) 0)
          return;
        if ((cullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.RemoveInstances(cullingData);
        }
        else if ((cullingData.m_Flags & PreCullingFlags.Object) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateObjectInstances(cullingData);
        }
        else if ((cullingData.m_Flags & PreCullingFlags.Net) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateNetInstances(cullingData);
        }
        else if ((cullingData.m_Flags & PreCullingFlags.Lane) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateLaneInstances(cullingData);
        }
        else if ((cullingData.m_Flags & PreCullingFlags.Zone) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateZoneInstances(cullingData);
        }
        else
        {
          if ((cullingData.m_Flags & PreCullingFlags.FadeContainer) == (PreCullingFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.UpdateFadeInstances(cullingData);
        }
      }

      private void RemoveInstances(PreCullingData cullingData)
      {
        DynamicBuffer<MeshBatch> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MeshBatches.TryGetBuffer(cullingData.m_Entity, out bufferData1))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool fadeOut = this.m_UseLodFade && (cullingData.m_Flags & (PreCullingFlags.Temp | PreCullingFlags.Zone)) == (PreCullingFlags) 0 && ((cullingData.m_Flags & PreCullingFlags.Deleted) == (PreCullingFlags) 0 ? !this.m_UnspawnedVisible && this.m_UnspawnedData.HasComponent(cullingData.m_Entity) || (this.m_CullingInfoData[cullingData.m_Entity].m_Mask & (this.m_VisibleMask | this.m_BecameHidden)) == (BoundsMask) 0 : (cullingData.m_Flags & PreCullingFlags.Applied) == (PreCullingFlags) 0);
        Entity entity = Entity.Null;
        if (fadeOut)
        {
          DynamicBuffer<TransformFrame> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformFrames.TryGetBuffer(cullingData.m_Entity, out bufferData2))
          {
            entity = cullingData.m_Entity;
            uint updateFrame1;
            uint updateFrame2;
            float framePosition;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_FrameTime, new UpdateFrame((uint) cullingData.m_UpdateFrame).m_Index, out updateFrame1, out updateFrame2, out framePosition);
            float3 float3 = math.lerp(bufferData2[(int) updateFrame1].m_Velocity, bufferData2[(int) updateFrame2].m_Velocity, framePosition);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_VelocityQueue.Enqueue(new BatchInstanceSystem.VelocityData()
            {
              m_Source = entity,
              m_Velocity = float3
            });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_RelativeData.HasComponent(cullingData.m_Entity))
            {
              CurrentVehicle componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentVehicleData.TryGetComponent(cullingData.m_Entity, out componentData1))
              {
                entity = componentData1.m_Vehicle;
              }
              else
              {
                Owner componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.TryGetComponent(cullingData.m_Entity, out componentData2))
                {
                  entity = componentData2.m_Owner;
                  // ISSUE: reference to a compiler-generated field
                  while (this.m_RelativeData.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    entity = this.m_OwnerData[entity].m_Owner;
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.RemoveInstances(bufferData1, fadeOut, entity);
        bufferData1.Clear();
      }

      private unsafe void UpdateObjectInstances(PreCullingData cullingData)
      {
        DynamicBuffer<MeshBatch> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MeshBatches.TryGetBuffer(cullingData.m_Entity, out bufferData1))
          return;
        MeshLayer meshLayer1 = MeshLayer.Default;
        bool flag1 = (cullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 && (cullingData.m_Flags & (PreCullingFlags.Created | PreCullingFlags.Applied)) != PreCullingFlags.Applied;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag2 = this.m_InterpolatedTransformData.HasComponent(cullingData.m_Entity) || this.m_StoppedData.HasComponent(cullingData.m_Entity);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = this.m_ObjectMarkerData.HasComponent(cullingData.m_Entity);
        bool flag4 = false;
        // ISSUE: reference to a compiler-generated field
        SubMeshFlags subMeshFlags1 = (SubMeshFlags) (2359296 | (this.m_LeftHandTraffic ? 65536 : 131072));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ErrorData.HasComponent(cullingData.m_Entity) || this.m_WarningData.HasComponent(cullingData.m_Entity) || this.m_OverrideData.HasComponent(cullingData.m_Entity) || this.m_HighlightedData.HasComponent(cullingData.m_Entity))
        {
          meshLayer1 |= MeshLayer.Outline;
          subMeshFlags1 |= SubMeshFlags.OutlineOnly;
          flag4 = true;
        }
        int length = bufferData1.Length;
        MeshBatch* meshBatchPtr = stackalloc MeshBatch[length];
        UnsafeUtility.MemCpy((void*) meshBatchPtr, bufferData1.GetUnsafeReadOnlyPtr(), (long) (length * UnsafeUtility.SizeOf<MeshBatch>()));
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        bufferData1.Clear();
        bool hasMeshMatches = false;
        // ISSUE: reference to a compiler-generated field
        if (flag1 && this.m_BecameVisible != (BoundsMask) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          flag1 = (this.m_CullingInfoData[cullingData.m_Entity].m_Mask & this.m_VisibleMask) != this.m_BecameVisible;
        }
        DynamicBuffer<SubMesh> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSubMeshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData2))
        {
          MeshLayer meshLayer2 = meshLayer1;
          SubMeshFlags subMeshFlags2 = subMeshFlags1;
          int3 tileCounts = (int3) 0;
          if ((cullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[cullingData.m_Entity];
            if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
            {
              if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent | TempFlags.SubDetail)) != (TempFlags) 0)
              {
                meshLayer2 |= MeshLayer.Outline;
                subMeshFlags2 |= SubMeshFlags.OutlineOnly;
              }
              flag4 = (temp.m_Flags & TempFlags.Essential) > (TempFlags) 0;
            }
            else
              goto label_53;
          }
          if (flag3)
            meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Marker;
          else if (flag2)
          {
            meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Moving;
          }
          else
          {
            Game.Objects.Elevation componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectElevationData.TryGetComponent(cullingData.m_Entity, out componentData) && (double) componentData.m_Elevation < 0.0)
              meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Tunnel;
          }
          Tree componentData1;
          float3 float3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TreeData.TryGetComponent(cullingData.m_Entity, out componentData1))
          {
            GrowthScaleData componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabGrowthScaleData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
              subMeshFlags2 |= BatchDataHelpers.CalculateTreeSubMeshData(componentData1, componentData2, out float3);
            else
              subMeshFlags2 |= SubMeshFlags.RequireAdult;
          }
          Stack componentData3;
          StackData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_StackData.TryGetComponent(cullingData.m_Entity, out componentData3) && this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData4))
            subMeshFlags2 |= BatchDataHelpers.CalculateStackSubMeshData(componentData3, componentData4, out tileCounts, out float3, out float3 _);
          Game.Objects.NetObject componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetObjectData.TryGetComponent(cullingData.m_Entity, out componentData5))
            subMeshFlags2 |= BatchDataHelpers.CalculateNetObjectSubMeshData(componentData5);
          Quantity componentData6;
          // ISSUE: reference to a compiler-generated field
          if (this.m_QuantityData.TryGetComponent(cullingData.m_Entity, out componentData6))
          {
            QuantityObjectData componentData7;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabQuantityObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData7))
            {
              // ISSUE: reference to a compiler-generated field
              subMeshFlags2 |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData6, componentData7, this.m_EditorMode);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              subMeshFlags2 |= BatchDataHelpers.CalculateQuantitySubMeshData(componentData6, new QuantityObjectData(), this.m_EditorMode);
            }
          }
          UnderConstruction componentData8;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_UnderConstructionData.TryGetComponent(cullingData.m_Entity, out componentData8) || !(componentData8.m_NewPrefab == Entity.Null))
          {
            Destroyed componentData9;
            ObjectGeometryData componentData10;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_DestroyedData.TryGetComponent(cullingData.m_Entity, out componentData9) && this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData10) && (componentData10.m_Flags & (Game.Objects.GeometryFlags.Physical | Game.Objects.GeometryFlags.HasLot)) == (Game.Objects.GeometryFlags.Physical | Game.Objects.GeometryFlags.HasLot))
            {
              if ((double) componentData9.m_Cleared < 0.0)
                meshLayer2 &= ~MeshLayer.Outline;
              else
                goto label_53;
            }
            DynamicBuffer<MeshGroup> bufferData3 = new DynamicBuffer<MeshGroup>();
            int num = 1;
            DynamicBuffer<SubMeshGroup> bufferData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData4) && this.m_MeshGroups.TryGetBuffer(cullingData.m_Entity, out bufferData3))
              num = bufferData3.Length;
            for (int index = 0; index < num; ++index)
            {
              SubMeshGroup subMeshGroup;
              if (bufferData4.IsCreated)
              {
                MeshGroup meshGroup;
                CollectionUtils.TryGet<MeshGroup>(bufferData3, index, out meshGroup);
                subMeshGroup = bufferData4[(int) meshGroup.m_SubMeshGroup];
              }
              else
                subMeshGroup.m_SubMeshRange = new int2(0, bufferData2.Length);
              for (int x = subMeshGroup.m_SubMeshRange.x; x < subMeshGroup.m_SubMeshRange.y; ++x)
              {
                SubMesh subMesh = bufferData2[x];
                if ((subMesh.m_Flags & subMeshFlags2) == subMesh.m_Flags)
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Prefabs.MeshData meshData = this.m_PrefabMeshData[subMesh.m_SubMesh];
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<BatchGroup> prefabBatchGroup = this.m_PrefabBatchGroups[subMesh.m_SubMesh];
                  MeshLayer layers = meshLayer2;
                  if (meshData.m_DefaultLayers != (MeshLayer) 0 && (meshLayer2 & (MeshLayer.Moving | MeshLayer.Marker)) == (MeshLayer) 0 || (meshData.m_DefaultLayers & (MeshLayer.Pipeline | MeshLayer.SubPipeline)) != (MeshLayer) 0)
                  {
                    MeshLayer meshLayer3 = layers & ~(MeshLayer.Default | MeshLayer.Moving | MeshLayer.Tunnel | MeshLayer.Marker);
                    Owner componentData11;
                    // ISSUE: reference to a compiler-generated field
                    this.m_OwnerData.TryGetComponent(cullingData.m_Entity, out componentData11);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    layers = meshLayer3 | Game.Net.SearchSystem.GetLayers(componentData11, new Game.Net.UtilityLane(), meshData.m_DefaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData);
                  }
                  if ((layers & MeshLayer.Outline) != (MeshLayer) 0 && (meshData.m_State & MeshFlags.Decal) != (MeshFlags) 0 && !flag4)
                    layers &= ~MeshLayer.Outline;
                  if ((subMesh.m_Flags & SubMeshFlags.OutlineOnly) != (SubMeshFlags) 0)
                    layers &= MeshLayer.Outline;
                  int tileCount = math.select(math.select(math.select(1, tileCounts.x, (subMesh.m_Flags & SubMeshFlags.IsStackStart) > (SubMeshFlags) 0), tileCounts.y, (subMesh.m_Flags & SubMeshFlags.IsStackMiddle) > (SubMeshFlags) 0), tileCounts.z, (subMesh.m_Flags & SubMeshFlags.IsStackEnd) > (SubMeshFlags) 0);
                  if (tileCount >= 1)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddInstance(meshBatchPtr, ref length, bufferData1, prefabBatchGroup, layers, MeshType.Object, (ushort) 0, flag1, cullingData.m_Entity, index, x - subMeshGroup.m_SubMeshRange.x, tileCount, ref hasMeshMatches);
                  }
                }
              }
            }
          }
        }
label_53:
        bufferData1.TrimExcess();
        // ISSUE: reference to a compiler-generated method
        this.RemoveInstances(meshBatchPtr, length, bufferData1, flag1, hasMeshMatches, cullingData.m_Entity);
      }

      private unsafe void UpdateNetInstances(PreCullingData cullingData)
      {
        DynamicBuffer<MeshBatch> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MeshBatches.TryGetBuffer(cullingData.m_Entity, out bufferData))
          return;
        MeshLayer meshLayer = MeshLayer.Default;
        bool flag1 = (cullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 && (cullingData.m_Flags & (PreCullingFlags.Created | PreCullingFlags.Applied)) != PreCullingFlags.Applied;
        // ISSUE: reference to a compiler-generated field
        bool flag2 = this.m_NetOutsideConnectionData.HasComponent(cullingData.m_Entity);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = this.m_NetMarkerData.HasComponent(cullingData.m_Entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ErrorData.HasComponent(cullingData.m_Entity) || this.m_WarningData.HasComponent(cullingData.m_Entity) || this.m_HighlightedData.HasComponent(cullingData.m_Entity))
          meshLayer |= MeshLayer.Outline;
        int length = bufferData.Length;
        MeshBatch* meshBatchPtr = stackalloc MeshBatch[length];
        UnsafeUtility.MemCpy((void*) meshBatchPtr, bufferData.GetUnsafeReadOnlyPtr(), (long) (length * UnsafeUtility.SizeOf<MeshBatch>()));
        bufferData.Clear();
        MeshLayer layers1 = meshLayer;
        bool flag4 = false;
        bool hasMeshMatches = false;
        // ISSUE: reference to a compiler-generated field
        if (flag1 && this.m_BecameVisible != (BoundsMask) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          flag1 = (this.m_CullingInfoData[cullingData.m_Entity].m_Mask & this.m_VisibleMask) != this.m_BecameVisible;
        }
        if ((cullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          Temp temp = this.m_TempData[cullingData.m_Entity];
          if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
          {
            if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent | TempFlags.SubDetail)) != (TempFlags) 0)
            {
              if ((temp.m_Flags & TempFlags.SubDetail) != (TempFlags) 0)
                flag4 = true;
              else
                layers1 |= MeshLayer.Outline;
            }
          }
          else
            goto label_26;
        }
        if (flag3)
          layers1 = layers1 & ~MeshLayer.Default | MeshLayer.Marker;
        Composition componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CompositionData.TryGetComponent(cullingData.m_Entity, out componentData1))
        {
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          StartNodeGeometry startNodeGeometry = this.m_StartNodeGeometryData[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          EndNodeGeometry endNodeGeometry = this.m_EndNodeGeometryData[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData1 = this.m_PrefabNetGeometryData[this.m_PrefabRefData[cullingData.m_Entity].m_Prefab];
          if (math.any(edgeGeometry.m_Start.m_Length + edgeGeometry.m_End.m_Length > 0.1f))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetInstances(meshBatchPtr, ref length, bufferData, cullingData.m_Entity, componentData1.m_Edge, NetSubMesh.Edge, layers1, flag1, ref hasMeshMatches);
          }
          if (math.any(startNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(startNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData2 = this.m_PrefabNetGeometryData[this.m_PrefabRefData[edge.m_Start].m_Prefab];
            NetSubMesh subMesh = (netGeometryData1.m_MergeLayers & netGeometryData2.m_MergeLayers) != Layer.None ? NetSubMesh.StartNode : NetSubMesh.SubStartNode;
            MeshLayer layers2 = layers1;
            Temp componentData2;
            // ISSUE: reference to a compiler-generated field
            if (flag4 && this.m_TempData.TryGetComponent(edge.m_Start, out componentData2) && ((componentData2.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent) || (componentData2.m_Flags & TempFlags.Select) != (TempFlags) 0))
              layers2 |= MeshLayer.Outline;
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetInstances(meshBatchPtr, ref length, bufferData, cullingData.m_Entity, componentData1.m_StartNode, subMesh, layers2, flag1, ref hasMeshMatches);
          }
          if (math.any(endNodeGeometry.m_Geometry.m_Left.m_Length > 0.05f) | math.any(endNodeGeometry.m_Geometry.m_Right.m_Length > 0.05f))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData3 = this.m_PrefabNetGeometryData[this.m_PrefabRefData[edge.m_End].m_Prefab];
            NetSubMesh subMesh = (netGeometryData1.m_MergeLayers & netGeometryData3.m_MergeLayers) != Layer.None ? NetSubMesh.EndNode : NetSubMesh.SubEndNode;
            MeshLayer layers3 = layers1;
            Temp componentData3;
            // ISSUE: reference to a compiler-generated field
            if (flag4 && this.m_TempData.TryGetComponent(edge.m_End, out componentData3) && ((componentData3.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent) || (componentData3.m_Flags & TempFlags.Select) != (TempFlags) 0))
              layers3 |= MeshLayer.Outline;
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetInstances(meshBatchPtr, ref length, bufferData, cullingData.m_Entity, componentData1.m_EndNode, subMesh, layers3, flag1, ref hasMeshMatches);
          }
        }
        else
        {
          Orphan componentData4;
          // ISSUE: reference to a compiler-generated field
          if (!flag2 && this.m_OrphanData.TryGetComponent(cullingData.m_Entity, out componentData4))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetInstances(meshBatchPtr, ref length, bufferData, cullingData.m_Entity, componentData4.m_Composition, NetSubMesh.Orphan1, layers1, flag1, ref hasMeshMatches);
            // ISSUE: reference to a compiler-generated method
            this.UpdateNetInstances(meshBatchPtr, ref length, bufferData, cullingData.m_Entity, componentData4.m_Composition, NetSubMesh.Orphan2, layers1, flag1, ref hasMeshMatches);
          }
        }
label_26:
        bufferData.TrimExcess();
        // ISSUE: reference to a compiler-generated method
        this.RemoveInstances(meshBatchPtr, length, bufferData, flag1, hasMeshMatches, cullingData.m_Entity);
      }

      private unsafe void UpdateNetInstances(
        MeshBatch* oldBatches,
        ref int oldBatchCount,
        DynamicBuffer<MeshBatch> meshBatches,
        Entity entity,
        Entity composition,
        NetSubMesh subMesh,
        MeshLayer layers,
        bool fadeIn,
        ref bool hasMeshMatches)
      {
        // ISSUE: reference to a compiler-generated field
        NetCompositionMeshRef compositionMeshRef = this.m_PrefabCompositionMeshRef[composition];
        NetCompositionMeshData componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabCompositionMeshData.TryGetComponent(compositionMeshRef.m_Mesh, out componentData))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<BatchGroup> prefabBatchGroup = this.m_PrefabBatchGroups[compositionMeshRef.m_Mesh];
        MeshLayer layers1 = layers;
        if (componentData.m_DefaultLayers != (MeshLayer) 0 && (layers & MeshLayer.Marker) == (MeshLayer) 0)
          layers1 = layers1 & ~MeshLayer.Default | componentData.m_DefaultLayers;
        subMesh = compositionMeshRef.m_Rotate ? NetSubMesh.RotatedEdge : subMesh;
        // ISSUE: reference to a compiler-generated method
        this.AddInstance(oldBatches, ref oldBatchCount, meshBatches, prefabBatchGroup, layers1, MeshType.Net, (ushort) 0, fadeIn, entity, 0, (int) subMesh, 1, ref hasMeshMatches);
      }

      private unsafe void UpdateLaneInstances(PreCullingData cullingData)
      {
        DynamicBuffer<MeshBatch> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MeshBatches.TryGetBuffer(cullingData.m_Entity, out bufferData1))
          return;
        MeshLayer meshLayer1 = MeshLayer.Default;
        bool flag1 = (cullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 && (cullingData.m_Flags & (PreCullingFlags.Created | PreCullingFlags.Applied)) != PreCullingFlags.Applied;
        bool flag2 = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ErrorData.HasComponent(cullingData.m_Entity) || this.m_WarningData.HasComponent(cullingData.m_Entity) || this.m_HighlightedData.HasComponent(cullingData.m_Entity))
        {
          meshLayer1 |= MeshLayer.Outline;
          flag2 = true;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SubMeshFlags subMeshFlags1 = (SubMeshFlags) ((this.m_EditorMode || this.m_MarkersVisible ? 0 : 1024) | (this.m_LeftHandTraffic ? 131072 : 65536));
        int length1 = bufferData1.Length;
        MeshBatch* meshBatchPtr = stackalloc MeshBatch[length1];
        UnsafeUtility.MemCpy((void*) meshBatchPtr, bufferData1.GetUnsafeReadOnlyPtr(), (long) (length1 * UnsafeUtility.SizeOf<MeshBatch>()));
        // ISSUE: reference to a compiler-generated field
        Curve curve1 = this.m_CurveData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        bufferData1.Clear();
        bool hasMeshMatches = false;
        // ISSUE: reference to a compiler-generated field
        if (flag1 && this.m_BecameVisible != (BoundsMask) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          flag1 = (this.m_CullingInfoData[cullingData.m_Entity].m_Mask & this.m_VisibleMask) != this.m_BecameVisible;
        }
        DynamicBuffer<SubMesh> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if ((double) curve1.m_Length > 0.10000000149011612 && this.m_PrefabSubMeshes.TryGetBuffer(prefabRef.m_Prefab, out bufferData2))
        {
          MeshLayer meshLayer2 = meshLayer1;
          SubMeshFlags subMeshFlags2 = subMeshFlags1;
          if ((cullingData.m_Flags & PreCullingFlags.Temp) != (PreCullingFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_TempData[cullingData.m_Entity];
            if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
            {
              if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent | TempFlags.SubDetail)) != (TempFlags) 0)
                meshLayer2 |= MeshLayer.Outline;
              flag2 = (temp.m_Flags & TempFlags.Essential) > (TempFlags) 0;
            }
            else
              goto label_50;
          }
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_OwnerData.TryGetComponent(cullingData.m_Entity, out componentData1) && this.IsNetOwnerTunnel(componentData1))
            meshLayer2 = meshLayer2 & ~MeshLayer.Default | MeshLayer.Tunnel;
          Game.Net.PedestrianLane componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PedestrianLaneData.TryGetComponent(cullingData.m_Entity, out componentData2) && (componentData2.m_Flags & PedestrianLaneFlags.Unsafe) != (PedestrianLaneFlags) 0)
            subMeshFlags2 |= SubMeshFlags.RequireSafe;
          Game.Net.CarLane componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.TryGetComponent(cullingData.m_Entity, out componentData3))
          {
            if ((componentData3.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
              subMeshFlags2 |= SubMeshFlags.RequireSafe;
            if ((componentData3.m_Flags & CarLaneFlags.LevelCrossing) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
              subMeshFlags2 |= SubMeshFlags.RequireLevelCrossing;
          }
          Game.Net.TrackLane componentData4;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TrackLaneData.TryGetComponent(cullingData.m_Entity, out componentData4))
          {
            if ((componentData4.m_Flags & TrackLaneFlags.LevelCrossing) == (TrackLaneFlags) 0)
              subMeshFlags2 |= SubMeshFlags.RequireLevelCrossing;
            subMeshFlags2 |= (componentData4.m_Flags & (TrackLaneFlags.Switch | TrackLaneFlags.DiamondCrossing)) == (TrackLaneFlags) 0 ? SubMeshFlags.RequireTrack : SubMeshFlags.RequireClear;
          }
          int x = 256;
          Game.Net.UtilityLane componentData5;
          UtilityLaneData componentData6;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_UtilityLaneData.TryGetComponent(cullingData.m_Entity, out componentData5) && this.m_DilatedUtilityTypes != UtilityTypes.None && this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData6) && (componentData6.m_UtilityTypes & this.m_DilatedUtilityTypes) != UtilityTypes.None)
            x = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(new float2(componentData6.m_VisualCapacity)));
          for (int index1 = 0; index1 < bufferData2.Length; ++index1)
          {
            SubMesh subMesh = bufferData2[index1];
            if ((subMesh.m_Flags & subMeshFlags2) == (SubMeshFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.MeshData meshData = this.m_PrefabMeshData[subMesh.m_SubMesh];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<BatchGroup> prefabBatchGroup = this.m_PrefabBatchGroups[subMesh.m_SubMesh];
              MeshLayer layers = meshLayer2;
              if ((subMesh.m_Flags & SubMeshFlags.RequireEditor) != (SubMeshFlags) 0)
                layers = layers & ~(MeshLayer.Default | MeshLayer.Tunnel) | MeshLayer.Marker;
              if (meshData.m_DefaultLayers != (MeshLayer) 0 && (layers & MeshLayer.Marker) == (MeshLayer) 0 || (meshData.m_DefaultLayers & (MeshLayer.Pipeline | MeshLayer.SubPipeline)) != (MeshLayer) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                layers = layers & ~(MeshLayer.Default | MeshLayer.Tunnel | MeshLayer.Marker) | Game.Net.SearchSystem.GetLayers(componentData1, componentData5, meshData.m_DefaultLayers, ref this.m_PrefabRefData, ref this.m_PrefabNetData, ref this.m_PrefabNetGeometryData);
              }
              float length2 = MathUtils.Size(meshData.m_Bounds.z);
              bool geometryTiling = (meshData.m_State & MeshFlags.Tiling) > (MeshFlags) 0;
              int tileCount = 0;
              DynamicBuffer<CutRange> bufferData3;
              int clipCount;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CutRanges.TryGetBuffer(cullingData.m_Entity, out bufferData3))
              {
                float num1 = 0.0f;
                for (int index2 = 0; index2 <= bufferData3.Length; ++index2)
                {
                  float num2;
                  float num3;
                  if (index2 < bufferData3.Length)
                  {
                    CutRange cutRange = bufferData3[index2];
                    num2 = cutRange.m_CurveDelta.min;
                    num3 = cutRange.m_CurveDelta.max;
                  }
                  else
                  {
                    num2 = 1f;
                    num3 = 1f;
                  }
                  if ((double) num2 >= (double) num1)
                  {
                    Curve curve2 = new Curve()
                    {
                      m_Length = curve1.m_Length * (num2 - num1)
                    };
                    if ((double) curve2.m_Length > 0.10000000149011612)
                      tileCount += BatchDataHelpers.GetTileCount(curve2, length2, meshData.m_TilingCount, geometryTiling, out clipCount);
                  }
                  num1 = num3;
                }
              }
              else
                tileCount = BatchDataHelpers.GetTileCount(curve1, length2, meshData.m_TilingCount, geometryTiling, out clipCount);
              if (tileCount >= 1)
              {
                if ((layers & MeshLayer.Outline) != (MeshLayer) 0 && (meshData.m_State & MeshFlags.Decal) != (MeshFlags) 0 && !flag2)
                  layers &= ~MeshLayer.Outline;
                int partition = math.min(x, (int) meshData.m_MinLod);
                // ISSUE: reference to a compiler-generated method
                this.AddInstance(meshBatchPtr, ref length1, bufferData1, prefabBatchGroup, layers, MeshType.Lane, (ushort) partition, flag1, cullingData.m_Entity, 0, index1, tileCount, ref hasMeshMatches);
              }
            }
          }
        }
label_50:
        bufferData1.TrimExcess();
        // ISSUE: reference to a compiler-generated method
        this.RemoveInstances(meshBatchPtr, length1, bufferData1, flag1, hasMeshMatches, cullingData.m_Entity);
      }

      private unsafe void UpdateZoneInstances(PreCullingData cullingData)
      {
        DynamicBuffer<MeshBatch> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MeshBatches.TryGetBuffer(cullingData.m_Entity, out bufferData))
          return;
        int length = bufferData.Length;
        MeshBatch* meshBatchPtr = stackalloc MeshBatch[length];
        UnsafeUtility.MemCpy((void*) meshBatchPtr, bufferData.GetUnsafeReadOnlyPtr(), (long) (length * UnsafeUtility.SizeOf<MeshBatch>()));
        // ISSUE: reference to a compiler-generated field
        Game.Zones.Block block = this.m_ZoneBlockData[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[cullingData.m_Entity];
        bufferData.Clear();
        bool hasMeshMatches = false;
        // ISSUE: reference to a compiler-generated field
        if ((cullingData.m_Flags & PreCullingFlags.Temp) == (PreCullingFlags) 0 || (this.m_TempData[cullingData.m_Entity].m_Flags & TempFlags.Hidden) == (TempFlags) 0)
        {
          ushort partition = (ushort) math.clamp(block.m_Size.x * block.m_Size.y - 1 >> 4, 0, 3);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BatchGroup> prefabBatchGroup = this.m_PrefabBatchGroups[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated method
          this.AddInstance(meshBatchPtr, ref length, bufferData, prefabBatchGroup, MeshLayer.Default, MeshType.Zone, partition, false, cullingData.m_Entity, 0, 0, 1, ref hasMeshMatches);
        }
        bufferData.TrimExcess();
        // ISSUE: reference to a compiler-generated method
        this.RemoveInstances(meshBatchPtr, length, bufferData, false, hasMeshMatches, cullingData.m_Entity);
      }

      private void UpdateFadeInstances(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<MeshBatch> meshBatch1 = this.m_MeshBatches[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<FadeBatch> fadeBatch = this.m_FadeBatches[cullingData.m_Entity];
        for (int index = 0; index < meshBatch1.Length; ++index)
        {
          MeshBatch meshBatch2 = meshBatch1[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_BatchInstances.GetCullingData(meshBatch2.m_GroupIndex, meshBatch2.m_InstanceIndex).lodFade.z == 0 || !this.m_UseLodFade)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_GroupActionQueue.Enqueue(new BatchInstanceSystem.GroupActionData()
            {
              m_GroupIndex = meshBatch2.m_GroupIndex,
              m_RemoveInstanceIndex = meshBatch2.m_InstanceIndex
            });
            meshBatch1.RemoveAtSwapBack(index);
            fadeBatch.RemoveAtSwapBack(index);
            --index;
          }
        }
      }

      private bool IsNetOwnerTunnel(Owner owner)
      {
        Game.Net.Elevation componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetElevationData.TryGetComponent(owner.m_Owner, out componentData) && (double) math.cmin(componentData.m_Elevation) < 0.0)
          return true;
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.TryGetBuffer(owner.m_Owner, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetElevationData.TryGetComponent(bufferData[index].m_Edge, out componentData) && (double) math.cmin(componentData.m_Elevation) < 0.0)
              return true;
          }
        }
        return false;
      }

      private unsafe void AddInstance(
        MeshBatch* oldBatches,
        ref int oldBatchCount,
        DynamicBuffer<MeshBatch> meshBatches,
        DynamicBuffer<BatchGroup> batchGroups,
        MeshLayer layers,
        MeshType type,
        ushort partition,
        bool fadeIn,
        Entity entity,
        int meshGroupIndex,
        int meshIndex,
        int tileCount,
        ref bool hasMeshMatches)
      {
        for (int index1 = 0; index1 < batchGroups.Length; ++index1)
        {
          BatchGroup batchGroup = batchGroups[index1];
          if ((batchGroup.m_Layer & layers) != (MeshLayer) 0 && batchGroup.m_Type == type && (int) batchGroup.m_Partition == (int) partition)
          {
label_12:
            for (int index2 = 0; index2 < tileCount; ++index2)
            {
              bool flag1 = fadeIn;
              for (int index3 = 0; index3 < oldBatchCount; ++index3)
              {
                MeshBatch elem = oldBatches[index3];
                bool flag2 = (int) elem.m_MeshGroup == meshGroupIndex && (int) elem.m_MeshIndex == meshIndex && (int) elem.m_TileIndex == index2;
                flag1 &= !flag2;
                if (flag2 && elem.m_GroupIndex == batchGroup.m_GroupIndex)
                {
                  meshBatches.Add(elem);
                  oldBatches[index3] = oldBatches[--oldBatchCount];
                  goto label_12;
                }
                else
                  hasMeshMatches |= flag2;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_BecameVisible != (BoundsMask) 0)
              {
                // ISSUE: reference to a compiler-generated field
                flag1 &= (this.m_BecameVisible & CommonUtils.GetBoundsMask(batchGroup.m_Layer)) == (BoundsMask) 0;
              }
              InstanceData instanceData = new InstanceData()
              {
                m_Entity = entity,
                m_MeshGroup = (byte) meshGroupIndex,
                m_MeshIndex = (byte) meshIndex,
                m_TileIndex = (byte) index2
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_GroupActionQueue.Enqueue(new BatchInstanceSystem.GroupActionData()
              {
                m_GroupIndex = batchGroup.m_GroupIndex,
                m_RemoveInstanceIndex = int.MaxValue,
                m_MergeIndex = batchGroup.m_MergeIndex,
                m_AddInstanceData = instanceData,
                m_FadeIn = flag1
              });
              meshBatches.Add(new MeshBatch()
              {
                m_GroupIndex = batchGroup.m_GroupIndex,
                m_InstanceIndex = -1,
                m_MeshGroup = instanceData.m_MeshGroup,
                m_MeshIndex = instanceData.m_MeshIndex,
                m_TileIndex = instanceData.m_TileIndex
              });
            }
          }
        }
      }

      private unsafe void RemoveInstances(
        MeshBatch* oldBatches,
        int oldBatchCount,
        DynamicBuffer<MeshBatch> meshBatches,
        bool fadeOut,
        bool hasMeshMatches,
        Entity entity)
      {
        for (int index1 = 0; index1 < oldBatchCount; ++index1)
        {
          MeshBatch meshBatch1 = oldBatches[index1];
          if (meshBatch1.m_GroupIndex != -1)
          {
            // ISSUE: reference to a compiler-generated field
            bool flag = fadeOut && this.m_UseLodFade;
            if (flag & hasMeshMatches)
            {
              for (int index2 = 0; index2 < meshBatches.Length; ++index2)
              {
                MeshBatch meshBatch2 = meshBatches[index2];
                if ((int) meshBatch1.m_MeshGroup == (int) meshBatch2.m_MeshGroup && (int) meshBatch1.m_MeshIndex == (int) meshBatch2.m_MeshIndex && (int) meshBatch1.m_TileIndex == (int) meshBatch2.m_TileIndex)
                {
                  flag = false;
                  break;
                }
              }
            }
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_FadeQueue.Enqueue(new BatchInstanceSystem.FadeData()
              {
                m_Source = Entity.Null,
                m_GroupIndex = meshBatch1.m_GroupIndex,
                m_InstanceIndex = meshBatch1.m_InstanceIndex
              });
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_GroupActionQueue.Enqueue(new BatchInstanceSystem.GroupActionData()
              {
                m_GroupIndex = meshBatch1.m_GroupIndex,
                m_RemoveInstanceIndex = meshBatch1.m_InstanceIndex
              });
            }
          }
        }
      }

      private void RemoveInstances(
        DynamicBuffer<MeshBatch> meshBatches,
        bool fadeOut,
        Entity entity)
      {
        for (int index = 0; index < meshBatches.Length; ++index)
        {
          MeshBatch meshBatch = meshBatches[index];
          if (meshBatch.m_GroupIndex != -1)
          {
            if (fadeOut)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_FadeQueue.Enqueue(new BatchInstanceSystem.FadeData()
              {
                m_Source = entity,
                m_GroupIndex = meshBatch.m_GroupIndex,
                m_InstanceIndex = meshBatch.m_InstanceIndex
              });
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_GroupActionQueue.Enqueue(new BatchInstanceSystem.GroupActionData()
              {
                m_GroupIndex = meshBatch.m_GroupIndex,
                m_RemoveInstanceIndex = meshBatch.m_InstanceIndex
              });
            }
          }
        }
      }
    }

    public struct GroupActionData : IComparable<BatchInstanceSystem.GroupActionData>
    {
      public int m_GroupIndex;
      public int m_RemoveInstanceIndex;
      public int m_MergeIndex;
      public InstanceData m_AddInstanceData;
      public bool m_FadeIn;

      public int CompareTo(BatchInstanceSystem.GroupActionData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(other.m_RemoveInstanceIndex - this.m_RemoveInstanceIndex, this.m_GroupIndex - other.m_GroupIndex, this.m_GroupIndex != other.m_GroupIndex);
      }

      public override int GetHashCode() => this.m_GroupIndex;
    }

    public struct VelocityData
    {
      public Entity m_Source;
      public float3 m_Velocity;
    }

    public struct FadeData
    {
      public Entity m_Source;
      public int m_GroupIndex;
      public int m_InstanceIndex;
    }

    [BurstCompile]
    private struct DequeueFadesJob : IJob
    {
      [ReadOnly]
      public Entity m_FadeContainer;
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData> m_BatchInstances;
      public NativeQueue<BatchInstanceSystem.VelocityData> m_VelocityQueue;
      public NativeQueue<BatchInstanceSystem.FadeData> m_FadeQueue;
      public BufferLookup<MeshBatch> m_MeshBatches;
      public BufferLookup<FadeBatch> m_FadeBatches;

      public void Execute()
      {
        NativeArray<BatchInstanceSystem.VelocityData> nativeArray = new NativeArray<BatchInstanceSystem.VelocityData>();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_VelocityQueue.IsEmpty())
        {
          // ISSUE: reference to a compiler-generated field
          nativeArray = this.m_VelocityQueue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        }
        // ISSUE: variable of a compiler-generated type
        BatchInstanceSystem.FadeData fadeData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_FadeQueue.TryDequeue(out fadeData))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref InstanceData local1 = ref this.m_BatchInstances.AccessInstanceData(fadeData.m_GroupIndex, fadeData.m_InstanceIndex);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ref CullingData local2 = ref this.m_BatchInstances.AccessCullingData(fadeData.m_GroupIndex, fadeData.m_InstanceIndex);
          // ISSUE: reference to a compiler-generated field
          local1.m_Entity = this.m_FadeContainer;
          local2.isFading = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<MeshBatch> meshBatch = this.m_MeshBatches[this.m_FadeContainer];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<FadeBatch> fadeBatch = this.m_FadeBatches[this.m_FadeContainer];
          float3 float3 = new float3();
          // ISSUE: reference to a compiler-generated field
          if (fadeData.m_Source != Entity.Null && nativeArray.IsCreated)
          {
            for (int index = 0; index < nativeArray.Length; ++index)
            {
              // ISSUE: variable of a compiler-generated type
              BatchInstanceSystem.VelocityData velocityData = nativeArray[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (velocityData.m_Source == fadeData.m_Source)
              {
                // ISSUE: reference to a compiler-generated field
                float3 = velocityData.m_Velocity;
                break;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          meshBatch.Add(new MeshBatch()
          {
            m_GroupIndex = fadeData.m_GroupIndex,
            m_InstanceIndex = fadeData.m_InstanceIndex,
            m_MeshGroup = byte.MaxValue,
            m_MeshIndex = byte.MaxValue,
            m_TileIndex = byte.MaxValue
          });
          // ISSUE: reference to a compiler-generated field
          fadeBatch.Add(new FadeBatch()
          {
            m_Source = fadeData.m_Source,
            m_Velocity = float3
          });
        }
        if (!nativeArray.IsCreated)
          return;
        nativeArray.Dispose();
      }
    }

    [BurstCompile]
    private struct GroupActionJob : IJobParallelFor
    {
      [NativeDisableParallelForRestriction]
      public BufferLookup<MeshBatch> m_MeshBatches;
      public NativeParallelQueue<BatchInstanceSystem.GroupActionData>.Reader m_GroupActions;
      [NativeDisableParallelForRestriction]
      public NativeBatchInstances<CullingData, GroupData, BatchData, InstanceData>.ParallelInstanceUpdater m_BatchInstanceUpdater;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<BatchInstanceSystem.GroupActionData> array = this.m_GroupActions.ToArray(index, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        if (array.Length >= 2)
          array.Sort<BatchInstanceSystem.GroupActionData>();
        int num1 = -1;
        GroupInstanceUpdater<CullingData, GroupData, BatchData, InstanceData> groupInstanceUpdater = new GroupInstanceUpdater<CullingData, GroupData, BatchData, InstanceData>();
        for (int index1 = 0; index1 < array.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          BatchInstanceSystem.GroupActionData groupActionData = array[index1];
          // ISSUE: reference to a compiler-generated field
          if (groupActionData.m_GroupIndex != num1)
          {
            if (num1 != -1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_BatchInstanceUpdater.EndGroup(groupInstanceUpdater);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            groupInstanceUpdater = this.m_BatchInstanceUpdater.BeginGroup(groupActionData.m_GroupIndex);
            // ISSUE: reference to a compiler-generated field
            num1 = groupActionData.m_GroupIndex;
          }
          // ISSUE: reference to a compiler-generated field
          if (groupActionData.m_RemoveInstanceIndex != int.MaxValue)
          {
            // ISSUE: reference to a compiler-generated field
            InstanceData instanceData = groupInstanceUpdater.RemoveInstance(groupActionData.m_RemoveInstanceIndex);
            if (instanceData.m_Entity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<MeshBatch> meshBatch1 = this.m_MeshBatches[instanceData.m_Entity];
              int instanceCount = groupInstanceUpdater.GetInstanceCount();
              for (int index2 = 0; index2 < meshBatch1.Length; ++index2)
              {
                MeshBatch meshBatch2 = meshBatch1[index2];
                // ISSUE: reference to a compiler-generated field
                if (meshBatch2.m_GroupIndex == groupActionData.m_GroupIndex && meshBatch2.m_InstanceIndex == instanceCount)
                {
                  // ISSUE: reference to a compiler-generated field
                  meshBatch2.m_InstanceIndex = groupActionData.m_RemoveInstanceIndex;
                  meshBatch1[index2] = meshBatch2;
                  break;
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<MeshBatch> meshBatch3 = this.m_MeshBatches[groupActionData.m_AddInstanceData.m_Entity];
            for (int index3 = 0; index3 < meshBatch3.Length; ++index3)
            {
              MeshBatch meshBatch4 = meshBatch3[index3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (meshBatch4.m_GroupIndex == groupActionData.m_GroupIndex && meshBatch4.m_InstanceIndex == -1 && (int) meshBatch4.m_MeshGroup == (int) groupActionData.m_AddInstanceData.m_MeshGroup && (int) meshBatch4.m_MeshIndex == (int) groupActionData.m_AddInstanceData.m_MeshIndex && (int) meshBatch4.m_TileIndex == (int) groupActionData.m_AddInstanceData.m_TileIndex)
              {
                // ISSUE: reference to a compiler-generated field
                int num2 = math.select((int) byte.MaxValue, 0, groupActionData.m_FadeIn);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                meshBatch4.m_InstanceIndex = groupInstanceUpdater.AddInstance(groupActionData.m_AddInstanceData, new CullingData()
                {
                  lodFade = (int4) num2
                }, groupActionData.m_MergeIndex);
                meshBatch3[index3] = meshBatch4;
                break;
              }
            }
          }
        }
        if (num1 != -1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BatchInstanceUpdater.EndGroup(groupInstanceUpdater);
        }
        array.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Error> __Game_Tools_Error_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Warning> __Game_Tools_Warning_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Override> __Game_Tools_Override_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Highlighted> __Game_Tools_Highlighted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferLookup;
      public BufferLookup<MeshBatch> __Game_Rendering_MeshBatch_RW_BufferLookup;
      public BufferLookup<FadeBatch> __Game_Rendering_FadeBatch_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Stopped> __Game_Objects_Stopped_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stack> __Game_Objects_Stack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.NetObject> __Game_Objects_NetObject_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Quantity> __Game_Objects_Quantity_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Marker> __Game_Objects_Marker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UnderConstruction> __Game_Objects_UnderConstruction_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.UtilityLane> __Game_Net_UtilityLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Marker> __Game_Net_Marker_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CutRange> __Game_Net_CutRange_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GrowthScaleData> __Game_Prefabs_GrowthScaleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<QuantityObjectData> __Game_Prefabs_QuantityObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> __Game_Prefabs_UtilityLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshRef> __Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<BatchGroup> __Game_Prefabs_BatchGroup_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Error_RO_ComponentLookup = state.GetComponentLookup<Error>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentLookup = state.GetComponentLookup<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Override_RO_ComponentLookup = state.GetComponentLookup<Override>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Highlighted_RO_ComponentLookup = state.GetComponentLookup<Highlighted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferLookup = state.GetBufferLookup<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RW_BufferLookup = state.GetBufferLookup<MeshBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_FadeBatch_RW_BufferLookup = state.GetBufferLookup<FadeBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentLookup = state.GetComponentLookup<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RO_ComponentLookup = state.GetComponentLookup<Stack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_NetObject_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.NetObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Quantity_RO_ComponentLookup = state.GetComponentLookup<Quantity>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Marker_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RO_ComponentLookup = state.GetComponentLookup<UnderConstruction>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferLookup = state.GetBufferLookup<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Net.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_UtilityLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.UtilityLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Marker_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Marker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CutRange_RO_BufferLookup = state.GetBufferLookup<CutRange>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GrowthScaleData_RO_ComponentLookup = state.GetComponentLookup<GrowthScaleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_QuantityObjectData_RO_ComponentLookup = state.GetComponentLookup<QuantityObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UtilityLaneData_RO_ComponentLookup = state.GetComponentLookup<UtilityLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshRef_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatchGroup_RO_BufferLookup = state.GetBufferLookup<BatchGroup>(true);
      }
    }
  }
}
