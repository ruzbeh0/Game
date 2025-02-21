// Decompiled with JetBrains decompiler
// Type: Game.Zones.CellCheckSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Zones
{
  [CompilerGenerated]
  public class CellCheckSystem : GameSystemBase
  {
    private UpdateCollectSystem m_ZoneUpdateCollectSystem;
    private Game.Objects.UpdateCollectSystem m_ObjectUpdateCollectSystem;
    private Game.Net.UpdateCollectSystem m_NetUpdateCollectSystem;
    private Game.Areas.UpdateCollectSystem m_AreaUpdateCollectSystem;
    private SearchSystem m_ZoneSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private ZoneSystem m_ZonePrefabSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_DeletedBlocksQuery;
    private CellCheckSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneUpdateCollectSystem = this.World.GetOrCreateSystemManaged<UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Objects.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Net.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Areas.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZonePrefabSystem = this.World.GetOrCreateSystemManaged<ZoneSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedBlocksQuery = this.GetEntityQuery(ComponentType.ReadOnly<Block>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ZoneUpdateCollectSystem.isUpdated && !this.m_ObjectUpdateCollectSystem.isUpdated && !this.m_NetUpdateCollectSystem.netsUpdated && !this.m_AreaUpdateCollectSystem.lotsUpdated && !this.m_AreaUpdateCollectSystem.mapTilesUpdated)
        return;
      NativeList<CellCheckHelpers.SortedEntity> nativeList1 = new NativeList<CellCheckHelpers.SortedEntity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<CellCheckHelpers.BlockOverlap> nativeQueue1 = new NativeQueue<CellCheckHelpers.BlockOverlap>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<CellCheckHelpers.BlockOverlap> nativeList2 = new NativeList<CellCheckHelpers.BlockOverlap>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<CellCheckHelpers.OverlapGroup> list = new NativeList<CellCheckHelpers.OverlapGroup>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Bounds2> nativeQueue2 = new NativeQueue<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<CellCheckHelpers.SortedEntity> nativeArray = nativeList1.AsDeferredJobArray();
      // ISSUE: reference to a compiler-generated method
      this.Dependency = JobHandle.CombineDependencies(this.Dependency, this.CollectUpdatedBlocks(nativeList1));
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, Bounds2> searchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies1);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_DeletedBlocksQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
      JobHandle dependencies3;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      CellBlockJobs.BlockCellsJob jobData1 = new CellBlockJobs.BlockCellsJob()
      {
        m_Blocks = nativeArray,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies3),
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabRoadCompositionData = this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_BuildOrder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      CellCheckHelpers.FindOverlappingBlocksJob jobData2 = new CellCheckHelpers.FindOverlappingBlocksJob()
      {
        m_Blocks = nativeArray,
        m_SearchTree = searchTree,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup,
        m_BuildOrderData = this.__TypeHandle.__Game_Zones_BuildOrder_RO_ComponentLookup,
        m_ResultQueue = nativeQueue1.AsParallelWriter()
      };
      CellCheckHelpers.GroupOverlappingBlocksJob jobData3 = new CellCheckHelpers.GroupOverlappingBlocksJob()
      {
        m_Blocks = nativeArray,
        m_OverlapQueue = nativeQueue1,
        m_BlockOverlaps = nativeList2,
        m_OverlapGroups = list
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      CellOccupyJobs.ZoneAndOccupyCellsJob jobData4 = new CellOccupyJobs.ZoneAndOccupyCellsJob()
      {
        m_Blocks = nativeArray,
        m_DeletedBlockChunks = archetypeChunkListAsync,
        m_ZonePrefabs = this.m_ZonePrefabSystem.GetPrefabs(),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup,
        m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies4),
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PrefabSignatureBuildingData = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup,
        m_PrefabPlaceholderBuildingData = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup,
        m_PrefabZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_BuildOrder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      CellOverlapJobs.CheckBlockOverlapJob jobData5 = new CellOverlapJobs.CheckBlockOverlapJob()
      {
        m_BlockOverlaps = nativeList2.AsDeferredJobArray(),
        m_OverlapGroups = list.AsDeferredJobArray(),
        m_ZonePrefabs = this.m_ZonePrefabSystem.GetPrefabs(),
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_BuildOrderData = this.__TypeHandle.__Game_Zones_BuildOrder_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      CellCheckHelpers.UpdateBlocksJob jobData6 = new CellCheckHelpers.UpdateBlocksJob()
      {
        m_Blocks = nativeArray,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_VacantLot_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_BuildOrder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      LotSizeJobs.UpdateLotSizeJob jobData7 = new LotSizeJobs.UpdateLotSizeJob()
      {
        m_Blocks = nativeArray,
        m_ZonePrefabs = this.m_ZonePrefabSystem.GetPrefabs(),
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup,
        m_BuildOrderData = this.__TypeHandle.__Game_Zones_BuildOrder_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_ZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_SearchTree = searchTree,
        m_VacantLots = this.__TypeHandle.__Game_Zones_VacantLot_RW_BufferLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_BoundsQueue = nativeQueue2.AsParallelWriter()
      };
      JobHandle dependencies5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      LotSizeJobs.UpdateBoundsJob jobData8 = new LotSizeJobs.UpdateBoundsJob()
      {
        m_BoundsList = this.m_ZoneUpdateCollectSystem.GetUpdatedBounds(false, out dependencies5),
        m_BoundsQueue = nativeQueue2
      };
      JobHandle jobHandle1 = jobData1.Schedule<CellBlockJobs.BlockCellsJob, CellCheckHelpers.SortedEntity>(nativeList1, 1, JobHandle.CombineDependencies(this.Dependency, dependencies2, dependencies3));
      JobHandle dependsOn1 = jobData2.Schedule<CellCheckHelpers.FindOverlappingBlocksJob, CellCheckHelpers.SortedEntity>(nativeList1, 1, JobHandle.CombineDependencies(jobHandle1, dependencies1));
      JobHandle jobHandle2 = jobData3.Schedule<CellCheckHelpers.GroupOverlappingBlocksJob>(dependsOn1);
      JobHandle jobHandle3 = jobData4.Schedule<CellOccupyJobs.ZoneAndOccupyCellsJob, CellCheckHelpers.SortedEntity>(nativeList1, 1, JobHandle.CombineDependencies(jobHandle1, dependencies4, outJobHandle));
      JobHandle jobHandle4 = jobData5.Schedule<CellOverlapJobs.CheckBlockOverlapJob, CellCheckHelpers.OverlapGroup>(list, 1, JobHandle.CombineDependencies(jobHandle2, jobHandle3));
      JobHandle dependsOn2 = jobData6.Schedule<CellCheckHelpers.UpdateBlocksJob, CellCheckHelpers.SortedEntity>(nativeList1, 1, jobHandle4);
      JobHandle jobHandle5 = jobData7.Schedule<LotSizeJobs.UpdateLotSizeJob, CellCheckHelpers.SortedEntity>(nativeList1, 1, dependsOn2);
      JobHandle dependsOn3 = JobHandle.CombineDependencies(jobHandle5, dependencies5);
      JobHandle jobHandle6 = jobData8.Schedule<LotSizeJobs.UpdateBoundsJob>(dependsOn3);
      nativeList1.Dispose(jobHandle5);
      nativeQueue1.Dispose(jobHandle2);
      nativeList2.Dispose(jobHandle4);
      list.Dispose(jobHandle4);
      nativeQueue2.Dispose(jobHandle6);
      archetypeChunkListAsync.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneSearchSystem.AddSearchTreeReader(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZonePrefabSystem.AddPrefabsReader(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneUpdateCollectSystem.AddBoundsWriter(jobHandle6);
      this.Dependency = jobHandle5;
    }

    private JobHandle CollectUpdatedBlocks(
      NativeList<CellCheckHelpers.SortedEntity> updateBlocksList)
    {
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue3 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<Entity> nativeQueue4 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQuadTree<Entity, Bounds2> searchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies1);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (this.m_ZoneUpdateCollectSystem.isUpdated)
      {
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedBounds = this.m_ZoneUpdateCollectSystem.GetUpdatedBounds(true, out dependencies2);
        JobHandle jobHandle2 = new CellCheckHelpers.FindUpdatedBlocksSingleIterationJob()
        {
          m_Bounds = updatedBounds.AsDeferredJobArray(),
          m_SearchTree = searchTree,
          m_ResultQueue = nativeQueue1.AsParallelWriter()
        }.Schedule<CellCheckHelpers.FindUpdatedBlocksSingleIterationJob, Bounds2>(updatedBounds, 1, JobHandle.CombineDependencies(dependencies2, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ZoneUpdateCollectSystem.AddBoundsReader(jobHandle2);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
      }
      CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob jobData;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ObjectUpdateCollectSystem.isUpdated)
      {
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedBounds = this.m_ObjectUpdateCollectSystem.GetUpdatedBounds(out dependencies3);
        jobData = new CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob();
        jobData.m_Bounds = updatedBounds.AsDeferredJobArray();
        jobData.m_SearchTree = searchTree;
        jobData.m_ResultQueue = nativeQueue2.AsParallelWriter();
        JobHandle jobHandle3 = jobData.Schedule<CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob, Bounds2>(updatedBounds, 1, JobHandle.CombineDependencies(dependencies3, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectUpdateCollectSystem.AddBoundsReader(jobHandle3);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle3);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_NetUpdateCollectSystem.netsUpdated)
      {
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedNetBounds = this.m_NetUpdateCollectSystem.GetUpdatedNetBounds(out dependencies4);
        jobData = new CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob();
        jobData.m_Bounds = updatedNetBounds.AsDeferredJobArray();
        jobData.m_SearchTree = searchTree;
        jobData.m_ResultQueue = nativeQueue3.AsParallelWriter();
        JobHandle jobHandle4 = jobData.Schedule<CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob, Bounds2>(updatedNetBounds, 1, JobHandle.CombineDependencies(dependencies4, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetUpdateCollectSystem.AddNetBoundsReader(jobHandle4);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle4);
      }
      JobHandle job1 = dependencies1;
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.lotsUpdated)
      {
        JobHandle dependencies5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedLotBounds = this.m_AreaUpdateCollectSystem.GetUpdatedLotBounds(out dependencies5);
        jobData = new CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob();
        jobData.m_Bounds = updatedLotBounds.AsDeferredJobArray();
        jobData.m_SearchTree = searchTree;
        jobData.m_ResultQueue = nativeQueue4.AsParallelWriter();
        JobHandle jobHandle5 = jobData.Schedule<CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob, Bounds2>(updatedLotBounds, 1, JobHandle.CombineDependencies(dependencies5, dependencies1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddLotBoundsReader(jobHandle5);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle5);
        job1 = jobHandle5;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.mapTilesUpdated)
      {
        JobHandle dependencies6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedMapTileBounds = this.m_AreaUpdateCollectSystem.GetUpdatedMapTileBounds(out dependencies6);
        jobData = new CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob();
        jobData.m_Bounds = updatedMapTileBounds.AsDeferredJobArray();
        jobData.m_SearchTree = searchTree;
        jobData.m_ResultQueue = nativeQueue4.AsParallelWriter();
        JobHandle jobHandle6 = jobData.Schedule<CellCheckHelpers.FindUpdatedBlocksDoubleIterationJob, Bounds2>(updatedMapTileBounds, 1, JobHandle.CombineDependencies(dependencies6, job1));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddMapTileBoundsReader(jobHandle6);
        jobHandle1 = JobHandle.CombineDependencies(jobHandle1, jobHandle6);
      }
      JobHandle inputDeps = new CellCheckHelpers.CollectBlocksJob()
      {
        m_Queue1 = nativeQueue1,
        m_Queue2 = nativeQueue2,
        m_Queue3 = nativeQueue3,
        m_Queue4 = nativeQueue4,
        m_ResultList = updateBlocksList
      }.Schedule<CellCheckHelpers.CollectBlocksJob>(jobHandle1);
      nativeQueue1.Dispose(inputDeps);
      nativeQueue2.Dispose(inputDeps);
      nativeQueue3.Dispose(inputDeps);
      nativeQueue4.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneSearchSystem.AddSearchTreeReader(jobHandle1);
      return inputDeps;
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
    public CellCheckSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadComposition> __Game_Prefabs_RoadComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public BufferLookup<Cell> __Game_Zones_Cell_RW_BufferLookup;
      public ComponentLookup<ValidArea> __Game_Zones_ValidArea_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ValidArea> __Game_Zones_ValidArea_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildOrder> __Game_Zones_BuildOrder_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;
      public BufferLookup<VacantLot> __Game_Zones_VacantLot_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadComposition_RO_ComponentLookup = state.GetComponentLookup<RoadComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RW_BufferLookup = state.GetBufferLookup<Cell>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_ValidArea_RW_ComponentLookup = state.GetComponentLookup<ValidArea>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_ValidArea_RO_ComponentLookup = state.GetComponentLookup<ValidArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_BuildOrder_RO_ComponentLookup = state.GetComponentLookup<BuildOrder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_VacantLot_RW_BufferLookup = state.GetBufferLookup<VacantLot>();
      }
    }
  }
}
