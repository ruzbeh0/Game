// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateEdgesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class GenerateEdgesSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private GenerateObjectsSystem m_GenerateObjectsSystem;
    private ModificationBarrier2 m_TempEdgesBarrier;
    private NativeValue<uint> m_BuildOrder;
    private EntityQuery m_CreatedEdgesQuery;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DeletedQuery;
    private GenerateEdgesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildOrder = new NativeValue<uint>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateObjectsSystem = this.World.GetOrCreateSystemManaged<GenerateObjectsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempEdgesBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedEdgesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<Edge>(), ComponentType.ReadOnly<Game.Net.BuildOrder>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<CreationDefinition>(),
          ComponentType.ReadOnly<NetCourse>(),
          ComponentType.ReadOnly<Updated>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Edge>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Edge>(), ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_CreatedEdgesQuery, this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_BuildOrder.Dispose();
      base.OnDestroy();
    }

    public NativeValue<uint> GetBuildOrder() => this.m_BuildOrder;

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle job0_1 = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DefinitionQuery.IsEmptyIgnoreFilter)
      {
        NativeQueue<GenerateEdgesSystem.LocalConnectItem> nativeQueue = new NativeQueue<GenerateEdgesSystem.LocalConnectItem>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<GenerateEdgesSystem.LocalConnectItem> nativeList = new NativeList<GenerateEdgesSystem.LocalConnectItem>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeParallelMultiHashMap<GenerateEdgesSystem.NodeMapKey, Entity> parallelMultiHashMap = new NativeParallelMultiHashMap<GenerateEdgesSystem.NodeMapKey, Entity>(this.m_DefinitionQuery.CalculateEntityCount(), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeHashMap<GenerateEdgesSystem.OldEdgeKey, Entity> nativeHashMap = new NativeHashMap<GenerateEdgesSystem.OldEdgeKey, Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeHashMap<OwnerDefinition, Entity> reusedOwnerMap = this.m_GenerateObjectsSystem.GetReusedOwnerMap(out dependencies);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        WaterSurfaceData surfaceData = this.m_WaterSystem.GetSurfaceData(out deps);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateEdgesSystem.CheckNodesJob jobData1 = new GenerateEdgesSystem.CheckNodesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
          m_LocalConnectType = this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentTypeHandle,
          m_ElevationType = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_NodeType = this.__TypeHandle.__Game_Net_Node_RW_ComponentTypeHandle,
          m_LocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
          m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_TerrainHeightData = heightData,
          m_NodeMap = parallelMultiHashMap.AsParallelWriter(),
          m_LocalConnectQueue = nativeQueue.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateEdgesSystem.FillOldEdgesJob jobData2 = new GenerateEdgesSystem.FillOldEdgesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_OldEdgeMap = nativeHashMap
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_CreationDefinition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateEdgesSystem.CheckDefinitionsJob jobData3 = new GenerateEdgesSystem.CheckDefinitionsJob()
        {
          m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RW_ComponentTypeHandle,
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_NodeMap = parallelMultiHashMap.AsParallelWriter()
        };
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GenerateEdgesSystem.CollectLocalConnectItemsJob jobData4 = new GenerateEdgesSystem.CollectLocalConnectItemsJob()
        {
          m_LocalConnectQueue = nativeQueue,
          m_LocalConnectList = nativeList
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle dependsOn1 = jobData1.ScheduleParallel<GenerateEdgesSystem.CheckNodesJob>(this.m_DefinitionQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        JobHandle job1 = jobData2.Schedule<GenerateEdgesSystem.FillOldEdgesJob>(this.m_DeletedQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        JobHandle job0_2 = jobData3.ScheduleParallel<GenerateEdgesSystem.CheckDefinitionsJob>(this.m_DefinitionQuery, dependsOn1);
        JobHandle dependsOn2 = dependsOn1;
        JobHandle jobHandle1 = jobData4.Schedule<GenerateEdgesSystem.CollectLocalConnectItemsJob>(dependsOn2);
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeArray<int> entityIndexArrayAsync = this.m_DefinitionQuery.CalculateBaseEntityIndexArrayAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, JobHandle.CombineDependencies(job0_2, jobHandle1, deps), out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TrackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Roundabout_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_LocalCurveCache_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle2 = new GenerateEdgesSystem.GenerateEdgesJob()
        {
          m_ChunkBaseEntityIndices = entityIndexArrayAsync,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
          m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
          m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
          m_LocalCurveCacheType = this.__TypeHandle.__Game_Tools_LocalCurveCache_RO_ComponentTypeHandle,
          m_UpgradedType = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_SubReplacementType = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferTypeHandle,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
          m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
          m_BuildOrderData = this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentLookup,
          m_TramTrackData = this.__TypeHandle.__Game_Net_TramTrack_RO_ComponentLookup,
          m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
          m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
          m_ConditionData = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup,
          m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
          m_AggregatedData = this.__TypeHandle.__Game_Net_Aggregated_RO_ComponentLookup,
          m_RoundaboutData = this.__TypeHandle.__Game_Net_Roundabout_RO_ComponentLookup,
          m_ElectricityConnectionData = this.__TypeHandle.__Game_Net_ElectricityConnection_RO_ComponentLookup,
          m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
          m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
          m_LocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
          m_PrefabTrackData = this.__TypeHandle.__Game_Prefabs_TrackData_RO_ComponentLookup,
          m_PrefabRoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
          m_PrefabElectricityConnectionData = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
          m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
          m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
          m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
          m_SubReplacements = this.__TypeHandle.__Game_Net_SubReplacement_RO_BufferLookup,
          m_BuildOrder = this.m_BuildOrder.value,
          m_NodeMap = parallelMultiHashMap,
          m_ReusedOwnerMap = reusedOwnerMap,
          m_OldEdgeMap = nativeHashMap,
          m_LocalConnectList = nativeList.AsDeferredJobArray(),
          m_TerrainHeightData = heightData,
          m_WaterSurfaceData = surfaceData,
          m_CommandBuffer = this.m_TempEdgesBarrier.CreateCommandBuffer().AsParallelWriter()
        }.ScheduleParallel<GenerateEdgesSystem.GenerateEdgesJob>(this.m_DefinitionQuery, JobHandle.CombineDependencies(outJobHandle, job1, dependencies));
        nativeQueue.Dispose(jobHandle1);
        nativeList.Dispose(jobHandle2);
        parallelMultiHashMap.Dispose(jobHandle2);
        nativeHashMap.Dispose(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainSystem.AddCPUHeightReader(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_WaterSystem.AddSurfaceReader(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_GenerateObjectsSystem.AddOwnerMapReader(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        this.m_TempEdgesBarrier.AddJobHandleForProducer(jobHandle2);
        job0_1 = jobHandle2;
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedEdgesQuery.IsEmptyIgnoreFilter)
      {
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_CreatedEdgesQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new GenerateEdgesSystem.UpdateBuildOrderJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_BuildOrderType = this.__TypeHandle.__Game_Net_BuildOrder_RO_ComponentTypeHandle,
          m_BuildOrder = this.m_BuildOrder
        }.Schedule<GenerateEdgesSystem.UpdateBuildOrderJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        archetypeChunkListAsync.Dispose(jobHandle);
        job0_1 = JobHandle.CombineDependencies(job0_1, jobHandle);
      }
      this.Dependency = job0_1;
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
    public GenerateEdgesSystem()
    {
    }

    private struct NodeMapKey : IEquatable<GenerateEdgesSystem.NodeMapKey>
    {
      public Entity m_OriginalEntity;
      public float3 m_Position;
      public bool m_IsPermanent;
      public bool m_IsEditor;

      public NodeMapKey(Entity originalEntity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalEntity = originalEntity;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = new float3();
        // ISSUE: reference to a compiler-generated field
        this.m_IsPermanent = false;
        // ISSUE: reference to a compiler-generated field
        this.m_IsEditor = false;
      }

      public NodeMapKey(Entity originalEntity, float3 position, bool isPermanent, bool isEditor)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalEntity = originalEntity;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = position;
        // ISSUE: reference to a compiler-generated field
        this.m_IsPermanent = isPermanent;
        // ISSUE: reference to a compiler-generated field
        this.m_IsEditor = isEditor;
      }

      public NodeMapKey(CoursePos coursePos, bool isPermanent, bool isEditor)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalEntity = coursePos.m_Entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = coursePos.m_Position;
        // ISSUE: reference to a compiler-generated field
        this.m_IsPermanent = isPermanent;
        // ISSUE: reference to a compiler-generated field
        this.m_IsEditor = isEditor;
      }

      public bool Equals(GenerateEdgesSystem.NodeMapKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OriginalEntity != Entity.Null || other.m_OriginalEntity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_OriginalEntity.Equals(other.m_OriginalEntity);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Position.Equals(other.m_Position) && this.m_IsPermanent == other.m_IsPermanent && this.m_IsEditor == other.m_IsEditor;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_OriginalEntity != Entity.Null ? this.m_OriginalEntity.GetHashCode() : this.m_Position.GetHashCode();
      }
    }

    private struct LocalConnectItem
    {
      public Layer m_ConnectLayers;
      public Layer m_LocalConnectLayers;
      public float3 m_Position;
      public float m_Radius;
      public Bounds1 m_HeightRange;
      public Entity m_Node;
      public TempFlags m_TempFlags;
      public bool m_IsPermanent;
      public bool m_IsStandalone;

      public LocalConnectItem(
        Layer connectLayers,
        Layer localConnectLayers,
        float3 position,
        float radius,
        Bounds1 heightRange,
        Entity node,
        TempFlags tempFlags,
        bool isPermanent,
        bool isStandalone)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectLayers = connectLayers;
        // ISSUE: reference to a compiler-generated field
        this.m_LocalConnectLayers = localConnectLayers;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = position;
        // ISSUE: reference to a compiler-generated field
        this.m_Radius = radius;
        // ISSUE: reference to a compiler-generated field
        this.m_HeightRange = heightRange;
        // ISSUE: reference to a compiler-generated field
        this.m_Node = node;
        // ISSUE: reference to a compiler-generated field
        this.m_TempFlags = tempFlags;
        // ISSUE: reference to a compiler-generated field
        this.m_IsPermanent = isPermanent;
        // ISSUE: reference to a compiler-generated field
        this.m_IsStandalone = isStandalone;
      }
    }

    private struct OldEdgeKey : IEquatable<GenerateEdgesSystem.OldEdgeKey>
    {
      public Entity m_Prefab;
      public Entity m_SubPrefab;
      public Entity m_Original;
      public Entity m_Owner;
      public Entity m_StartNode;
      public Entity m_EndNode;

      public bool Equals(GenerateEdgesSystem.OldEdgeKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Prefab.Equals(other.m_Prefab) && this.m_SubPrefab.Equals(other.m_SubPrefab) && this.m_Original.Equals(other.m_Original) && this.m_Owner.Equals(other.m_Owner) && this.m_StartNode.Equals(other.m_StartNode) && this.m_EndNode.Equals(other.m_EndNode);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (((((17 * 31 + this.m_Prefab.GetHashCode()) * 31 + this.m_SubPrefab.GetHashCode()) * 31 + this.m_Original.GetHashCode()) * 31 + this.m_Owner.GetHashCode()) * 31 + this.m_StartNode.GetHashCode()) * 31 + this.m_EndNode.GetHashCode();
      }
    }

    [BurstCompile]
    private struct CheckNodesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<LocalConnect> m_LocalConnectType;
      [ReadOnly]
      public ComponentTypeHandle<Elevation> m_ElevationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public NativeParallelMultiHashMap<GenerateEdgesSystem.NodeMapKey, Entity>.ParallelWriter m_NodeMap;
      public NativeQueue<GenerateEdgesSystem.LocalConnectItem>.ParallelWriter m_LocalConnectQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Node> nativeArray3 = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        bool isEditor = chunk.Has<EditorContainer>(ref this.m_EditorContainerType);
        // ISSUE: reference to a compiler-generated field
        chunk.Has<Elevation>(ref this.m_ElevationType);
        // ISSUE: reference to a compiler-generated field
        bool flag = !chunk.Has<Owner>(ref this.m_OwnerType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Game.Net.Node node = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          if (nativeArray2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeMap.Add(new GenerateEdgesSystem.NodeMapKey(nativeArray2[index].m_Original, node.m_Position, false, isEditor), nativeArray1[index]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeMap.Add(new GenerateEdgesSystem.NodeMapKey(Entity.Null, node.m_Position, true, isEditor), nativeArray1[index]);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<LocalConnect>(ref this.m_LocalConnectType))
          return;
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          Entity node1 = nativeArray1[index];
          Game.Net.Node node2 = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          LocalConnectData localConnectData = this.m_LocalConnectData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_NetData[prefabRef.m_Prefab];
          float radius = math.max(0.0f, netGeometryData.m_DefaultWidth * 0.5f + localConnectData.m_SearchDistance);
          if (nativeArray2.Length != 0)
          {
            Temp temp = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_LocalConnectQueue.Enqueue(new GenerateEdgesSystem.LocalConnectItem(localConnectData.m_Layers, netData.m_ConnectLayers, node2.m_Position, radius, localConnectData.m_HeightRange, node1, temp.m_Flags, false, flag || (double) localConnectData.m_SearchDistance == 0.0 || (netGeometryData.m_Flags & GeometryFlags.SubOwner) != 0));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_LocalConnectQueue.Enqueue(new GenerateEdgesSystem.LocalConnectItem(localConnectData.m_Layers, netData.m_ConnectLayers, node2.m_Position, radius, localConnectData.m_HeightRange, node1, (TempFlags) 0, true, flag || (double) localConnectData.m_SearchDistance == 0.0 || (netGeometryData.m_Flags & GeometryFlags.SubOwner) != 0));
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
    private struct FillOldEdgesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public NativeHashMap<GenerateEdgesSystem.OldEdgeKey, Entity> m_OldEdgeMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray4 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EditorContainer> nativeArray5 = chunk.GetNativeArray<EditorContainer>(ref this.m_EditorContainerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          Edge edge = nativeArray4[index];
          // ISSUE: variable of a compiler-generated type
          GenerateEdgesSystem.OldEdgeKey key;
          // ISSUE: reference to a compiler-generated field
          key.m_Prefab = nativeArray6[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          key.m_SubPrefab = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          key.m_Original = nativeArray3[index].m_Original;
          // ISSUE: reference to a compiler-generated field
          key.m_Owner = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          key.m_StartNode = edge.m_Start;
          // ISSUE: reference to a compiler-generated field
          key.m_EndNode = edge.m_End;
          EditorContainer editorContainer;
          if (CollectionUtils.TryGet<EditorContainer>(nativeArray5, index, out editorContainer))
          {
            // ISSUE: reference to a compiler-generated field
            key.m_SubPrefab = editorContainer.m_Prefab;
          }
          Owner owner;
          if (CollectionUtils.TryGet<Owner>(nativeArray2, index, out owner))
          {
            // ISSUE: reference to a compiler-generated field
            key.m_Owner = owner.m_Owner;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_OldEdgeMap.TryAdd(key, nativeArray1[index]);
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
    private struct CheckDefinitionsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      public NativeParallelMultiHashMap<GenerateEdgesSystem.NodeMapKey, Entity>.ParallelWriter m_NodeMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          CreationDefinition creationDefinition = nativeArray[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(creationDefinition.m_Owner) && this.m_EdgeData.HasComponent(creationDefinition.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeMap.Add(new GenerateEdgesSystem.NodeMapKey(creationDefinition.m_Original), Entity.Null);
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
    private struct CollectLocalConnectItemsJob : IJob
    {
      public NativeQueue<GenerateEdgesSystem.LocalConnectItem> m_LocalConnectQueue;
      public NativeList<GenerateEdgesSystem.LocalConnectItem> m_LocalConnectList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_LocalConnectQueue.Count;
        // ISSUE: reference to a compiler-generated field
        this.m_LocalConnectList.ResizeUninitialized(count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_LocalConnectList[index] = this.m_LocalConnectQueue.Dequeue();
        }
      }
    }

    [BurstCompile]
    private struct GenerateEdgesJob : IJobChunk
    {
      [ReadOnly]
      [DeallocateOnJobCompletion]
      public NativeArray<int> m_ChunkBaseEntityIndices;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> m_OwnerDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> m_NetCourseType;
      [ReadOnly]
      public ComponentTypeHandle<LocalCurveCache> m_LocalCurveCacheType;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> m_UpgradedType;
      [ReadOnly]
      public BufferTypeHandle<SubReplacement> m_SubReplacementType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<Game.Net.BuildOrder> m_BuildOrderData;
      [ReadOnly]
      public ComponentLookup<TramTrack> m_TramTrackData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<NetCondition> m_ConditionData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Aggregated> m_AggregatedData;
      [ReadOnly]
      public ComponentLookup<Roundabout> m_RoundaboutData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ElectricityConnection> m_ElectricityConnectionData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<TrackData> m_PrefabTrackData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_PrefabRoadData;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> m_PrefabElectricityConnectionData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<SubReplacement> m_SubReplacements;
      [ReadOnly]
      public uint m_BuildOrder;
      [ReadOnly]
      public NativeParallelMultiHashMap<GenerateEdgesSystem.NodeMapKey, Entity> m_NodeMap;
      [ReadOnly]
      public NativeHashMap<OwnerDefinition, Entity> m_ReusedOwnerMap;
      [ReadOnly]
      public NativeHashMap<GenerateEdgesSystem.OldEdgeKey, Entity> m_OldEdgeMap;
      [ReadOnly]
      public NativeArray<GenerateEdgesSystem.LocalConnectItem> m_LocalConnectList;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        int chunkBaseEntityIndex = this.m_ChunkBaseEntityIndices[unfilteredChunkIndex];
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<OwnerDefinition> nativeArray2 = chunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<NetCourse> nativeArray3 = chunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<LocalCurveCache> nativeArray4 = chunk.GetNativeArray<LocalCurveCache>(ref this.m_LocalCurveCacheType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Upgraded> nativeArray5 = chunk.GetNativeArray<Upgraded>(ref this.m_UpgradedType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<SubReplacement> bufferAccessor = chunk.GetBufferAccessor<SubReplacement>(ref this.m_SubReplacementType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            CreationDefinition definitionData = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(definitionData.m_Owner))
            {
              NetCourse course = nativeArray3[index];
              OwnerDefinition ownerData;
              CollectionUtils.TryGet<OwnerDefinition>(nativeArray2, index, out ownerData);
              Upgraded upgraded;
              CollectionUtils.TryGet<Upgraded>(nativeArray5, index, out upgraded);
              LocalCurveCache cachedCurve;
              CollectionUtils.TryGet<LocalCurveCache>(nativeArray4, index, out cachedCurve);
              DynamicBuffer<SubReplacement> subReplacements;
              CollectionUtils.TryGet<SubReplacement>(bufferAccessor, index, out subReplacements);
              // ISSUE: reference to a compiler-generated method
              this.GenerateEdge(unfilteredChunkIndex, chunkBaseEntityIndex + index, definitionData, ownerData, course, upgraded, cachedCurve, nativeArray4.Length != 0, subReplacements);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray6 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Temp> nativeArray7 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Edge> nativeArray8 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
          if (nativeArray8.Length != 0)
          {
            if (nativeArray7.Length != 0)
              return;
            for (int index = 0; index < nativeArray8.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateNodeConnections(unfilteredChunkIndex, nativeArray6[index], nativeArray8[index]);
            }
          }
          else
          {
            for (int index1 = 0; index1 < nativeArray7.Length; ++index1)
            {
              Temp temp = nativeArray7[index1];
              DynamicBuffer<ConnectedEdge> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedEdges.TryGetBuffer(temp.m_Original, out bufferData))
              {
                for (int index2 = 0; index2 < bufferData.Length; ++index2)
                {
                  Entity edge = bufferData[index2].m_Edge;
                  // ISSUE: reference to a compiler-generated method
                  if (this.ShouldDuplicate(edge, temp.m_Original, temp.m_Flags))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.DuplicateEdge(unfilteredChunkIndex, edge);
                  }
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_EdgeData.HasComponent(temp.m_Original))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.SplitEdge(unfilteredChunkIndex, temp.m_Original, nativeArray6[index1]);
                }
              }
            }
          }
        }
      }

      private void UpdateNodeConnections(int jobIndex, Entity edge, Edge edgeData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[edge];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> nodes = this.m_CommandBuffer.SetBuffer<ConnectedNode>(jobIndex, edge);
        // ISSUE: reference to a compiler-generated field
        Curve curveData = this.m_CurveData[edge];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[edge];
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefabRef.m_Prefab];
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
        }
        bool isStandalone = true;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(edge))
          isStandalone = false;
        bool isZoneable = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRoadData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          isZoneable = (this.m_PrefabRoadData[prefabRef.m_Prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0;
        }
        // ISSUE: reference to a compiler-generated method
        this.FindNodeConnections(nodes, edgeData, curveData, new Temp(), netData, netGeometryData, true, isStandalone, isZoneable);
        for (int index = 0; index < connectedNode.Length; ++index)
        {
          ConnectedNode elem = connectedNode[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          if (!this.m_NodeMap.ContainsKey(new GenerateEdgesSystem.NodeMapKey(Entity.Null, this.m_NodeData[elem.m_Node].m_Position, true, this.m_EditorContainerData.HasComponent(elem.m_Node))))
            nodes.Add(elem);
        }
      }

      private bool ShouldDuplicate(Entity edge, Entity fromNode, TempFlags startTempFlags)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        if (this.m_DeletedData.HasComponent(edge) || this.m_NodeMap.ContainsKey(new GenerateEdgesSystem.NodeMapKey(edge)))
          return false;
        // ISSUE: reference to a compiler-generated field
        Edge edge1 = this.m_EdgeData[edge];
        Entity entity1;
        NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> it;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        if (edge1.m_Start != fromNode || !this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(edge1.m_End), out entity1, out it))
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((startTempFlags & (TempFlags.Select | TempFlags.Modify | TempFlags.Regenerate | TempFlags.Upgrade)) != (TempFlags) 0 || this.m_TempData.HasComponent(entity1) && (this.m_TempData[entity1].m_Flags & (TempFlags.Select | TempFlags.Modify | TempFlags.Regenerate | TempFlags.Upgrade)) != (TempFlags) 0)
          return true;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[edge];
        for (int index = 0; index < connectedNode.Length; ++index)
        {
          Entity entity2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(connectedNode[index].m_Node), out entity2, out it) && this.m_TempData.HasComponent(entity2) && (this.m_TempData[entity2].m_Flags & (TempFlags.Select | TempFlags.Modify | TempFlags.Regenerate | TempFlags.Upgrade)) != (TempFlags) 0)
            return true;
        }
        return false;
      }

      private void DuplicateEdge(int jobIndex, Entity edge)
      {
        // ISSUE: reference to a compiler-generated field
        Edge edge1 = this.m_EdgeData[edge];
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[edge];
        NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> it;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        if (!this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(edge1.m_Start), out edge1.m_Start, out it) || !this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(edge1.m_End), out edge1.m_End, out it) || edge1.m_Start == edge1.m_End)
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef component1 = this.m_PrefabRefData[edge];
        NetGeometryData netGeometryData = new NetGeometryData();
        bool flag1 = false;
        // ISSUE: reference to a compiler-generated field
        bool flag2 = this.m_PrefabData.IsComponentEnabled(component1.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(component1.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[component1.m_Prefab];
          flag1 = true;
        }
        Temp temp = new Temp();
        temp.m_Original = edge;
        Temp componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.TryGetComponent(edge1.m_Start, out componentData1) && ((componentData1.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent) || (componentData1.m_Flags & TempFlags.Select) != (TempFlags) 0))
          temp.m_Flags |= TempFlags.SubDetail;
        Temp componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TempData.TryGetComponent(edge1.m_End, out componentData2) && ((componentData2.m_Flags & (TempFlags.Upgrade | TempFlags.Parent)) == (TempFlags.Upgrade | TempFlags.Parent) || (componentData2.m_Flags & TempFlags.Select) != (TempFlags) 0))
          temp.m_Flags |= TempFlags.SubDetail;
        Composition component2 = new Composition();
        component2.m_Edge = component1.m_Prefab;
        component2.m_StartNode = component1.m_Prefab;
        component2.m_EndNode = component1.m_Prefab;
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[component1.m_Prefab];
        EditorContainer componentData3;
        // ISSUE: reference to a compiler-generated field
        this.m_EditorContainerData.TryGetComponent(edge, out componentData3);
        Owner componentData4;
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerData.TryGetComponent(edge, out componentData4);
        OwnerDefinition ownerDefinition = new OwnerDefinition();
        Entity oldEntity1;
        // ISSUE: reference to a compiler-generated method
        bool oldEntity2 = this.TryGetOldEntity(edge1, component1.m_Prefab, componentData3.m_Prefab, edge, ref ownerDefinition, ref componentData4.m_Owner, out oldEntity1);
        if (oldEntity2)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldEntity1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldEntity1, new Updated());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          oldEntity1 = this.m_CommandBuffer.CreateEntity(jobIndex, netData.m_EdgeArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, oldEntity1, component1);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Edge>(jobIndex, oldEntity1, edge1);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Curve>(jobIndex, oldEntity1, curve);
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Composition>(jobIndex, oldEntity1, component2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Net.BuildOrder>(jobIndex, oldEntity1, this.m_BuildOrderData[edge]);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity1, this.m_PseudoRandomSeedData[edge]);
        }
        Elevation componentData5;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElevationData.TryGetComponent(edge, out componentData5))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, oldEntity1, componentData5);
        }
        Upgraded componentData6;
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpgradedData.TryGetComponent(edge, out componentData6))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(jobIndex, oldEntity1, componentData6);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (oldEntity2 && this.m_UpgradedData.HasComponent(oldEntity1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Upgraded>(jobIndex, oldEntity1);
          }
        }
        DynamicBuffer<SubReplacement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubReplacements.TryGetBuffer(edge, out bufferData))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<SubReplacement>(jobIndex, oldEntity1).CopyFrom(bufferData);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (oldEntity2 && this.m_SubReplacements.HasBuffer(oldEntity1))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<SubReplacement>(jobIndex, oldEntity1);
          }
        }
        bool isStandalone = true;
        if (componentData4.m_Owner != Entity.Null)
        {
          isStandalone = false;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, oldEntity1, componentData4);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_FixedData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          Fixed component3 = this.m_FixedData[edge];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Fixed>(jobIndex, oldEntity1, component3);
        }
        Aggregated componentData7;
        // ISSUE: reference to a compiler-generated field
        if (netGeometryData.m_AggregateType != Entity.Null && this.m_AggregatedData.TryGetComponent(edge, out componentData7))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Aggregated>(jobIndex, oldEntity1, componentData7);
        }
        // ISSUE: reference to a compiler-generated field
        if (flag2 && this.m_ConditionData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          NetCondition component4 = this.m_ConditionData[edge];
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NetCondition>(jobIndex, oldEntity1, component4);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabTrackData.HasComponent(component1.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TramTrackData.HasComponent(edge))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<TramTrack>(jobIndex, oldEntity1, new TramTrack());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (oldEntity2 && this.m_TramTrackData.HasComponent(oldEntity1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TramTrack>(jobIndex, oldEntity1);
            }
          }
        }
        bool isZoneable = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRoadData.HasComponent(component1.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          isZoneable = (this.m_PrefabRoadData[component1.m_Prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0;
          // ISSUE: reference to a compiler-generated field
          if (flag2 && this.m_RoadData.HasComponent(edge))
          {
            // ISSUE: reference to a compiler-generated field
            Road component5 = this.m_RoadData[edge];
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Road>(jobIndex, oldEntity1, component5);
          }
        }
        if (componentData3.m_Prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<EditorContainer>(jobIndex, oldEntity1, componentData3);
          PseudoRandomSeed componentData8;
          // ISSUE: reference to a compiler-generated field
          if (!flag1 && this.m_PseudoRandomSeedData.TryGetComponent(edge, out componentData8))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity1, componentData8);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_NativeData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Native>(jobIndex, oldEntity1, new Native());
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ElectricityConnectionData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Game.Net.ElectricityConnection>(jobIndex, oldEntity1, new Game.Net.ElectricityConnection());
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[edge];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> nodes = this.m_CommandBuffer.SetBuffer<ConnectedNode>(jobIndex, oldEntity1);
        // ISSUE: reference to a compiler-generated method
        this.FindNodeConnections(nodes, edge1, curve, temp, netData, netGeometryData, false, isStandalone, isZoneable);
        for (int index = 0; index < connectedNode.Length; ++index)
        {
          ConnectedNode elem = connectedNode[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          if (!this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(elem.m_Node), out Entity _, out it))
            nodes.Add(elem);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(jobIndex, oldEntity1, temp);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, edge, new Hidden());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, edge, new BatchesUpdated());
      }

      private bool TryGetNodes(
        int jobIndex,
        Entity edge,
        Entity middleNode,
        Edge edgeData,
        out Entity start,
        out Entity end,
        out float3 curveRange)
      {
        start = end = Entity.Null;
        float2 float2 = new float2(float.MinValue, float.MaxValue);
        // ISSUE: reference to a compiler-generated field
        Temp component = this.m_TempData[middleNode];
        curveRange = new float3(0.0f, component.m_CurvePosition, 1f);
        Entity entity;
        NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> it1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        if (this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(edge), out entity, out it1))
        {
          // ISSUE: reference to a compiler-generated field
          do
          {
            Temp componentData;
            // ISSUE: reference to a compiler-generated field
            if (entity != middleNode && this.m_TempData.TryGetComponent(entity, out componentData))
            {
              float num = componentData.m_CurvePosition - component.m_CurvePosition;
              if ((double) num < 0.0)
              {
                if ((double) num > (double) float2.x)
                {
                  start = entity;
                  curveRange.x = componentData.m_CurvePosition;
                  float2.x = num;
                }
              }
              else if ((double) num > 0.0 && (double) num < (double) float2.y)
              {
                end = entity;
                curveRange.z = componentData.m_CurvePosition;
                float2.y = num;
              }
            }
          }
          while (this.m_NodeMap.TryGetNextValue(out entity, ref it1));
        }
        NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> it2;
        if (start == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          if (!this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(edgeData.m_Start), out start, out it2))
            return false;
        }
        else
        {
          start = Entity.Null;
          component.m_Original = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Temp>(jobIndex, middleNode, component);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return !(end == Entity.Null) || this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(edgeData.m_End), out end, out it2);
      }

      private void SplitEdge(int jobIndex, Entity edge, Entity middleNode)
      {
        // ISSUE: reference to a compiler-generated field
        Edge edgeData = this.m_EdgeData[edge];
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[edge];
        Edge edge1 = new Edge();
        Edge edge2 = new Edge();
        edge1.m_End = middleNode;
        edge2.m_Start = middleNode;
        float3 curveRange;
        // ISSUE: reference to a compiler-generated method
        if (!this.TryGetNodes(jobIndex, edge, middleNode, edgeData, out edge1.m_Start, out edge2.m_End, out curveRange) || edge1.m_Start == edge1.m_End || edge2.m_Start == edge2.m_End)
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[edge];
        NetGeometryData componentData1;
        // ISSUE: reference to a compiler-generated field
        bool component = this.m_NetGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1);
        Curve curveData1 = new Curve();
        Curve curveData2 = new Curve();
        Game.Net.BuildOrder buildOrderData1 = new Game.Net.BuildOrder();
        Game.Net.BuildOrder buildOrderData2 = new Game.Net.BuildOrder();
        if (component)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.BuildOrder buildOrder = this.m_BuildOrderData[edge];
          buildOrderData1.m_Start = (double) curveRange.x <= 0.0 ? buildOrder.m_Start : (uint) ((int) buildOrder.m_Start + (int) (uint) ((double) (buildOrder.m_End - buildOrder.m_Start) * (double) curveRange.x) + 1);
          buildOrderData1.m_End = buildOrder.m_Start + (uint) ((double) (buildOrder.m_End - buildOrder.m_Start) * (double) curveRange.y);
          buildOrderData2.m_Start = buildOrderData1.m_End + 1U;
          buildOrderData2.m_End = (double) curveRange.z >= 1.0 ? buildOrder.m_End : buildOrder.m_Start + (uint) ((double) (buildOrder.m_End - buildOrder.m_Start) * (double) curveRange.z);
          if (buildOrderData1.m_Start > buildOrderData1.m_End)
            buildOrderData1.m_Start = buildOrderData1.m_End;
          if (buildOrderData2.m_Start > buildOrderData2.m_End)
            buildOrderData2.m_Start = buildOrderData2.m_End;
        }
        // ISSUE: reference to a compiler-generated field
        float y = this.m_NodeData[middleNode].m_Position.y;
        Fixed componentData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_FixedData.TryGetComponent(edge, out componentData2))
          componentData2 = new Fixed() { m_Index = -1 };
        if ((componentData1.m_Flags & GeometryFlags.StraightEdges) != (GeometryFlags) 0 && componentData2.m_Index < 0)
        {
          float3 float3 = MathUtils.Position(curve.m_Bezier, curveRange.y) with
          {
            y = y
          };
          if (edge1.m_Start != Entity.Null)
          {
            if ((double) curveRange.x > 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              float3 startPos = MathUtils.Position(curve.m_Bezier, curveRange.x) with
              {
                y = this.m_NodeData[edge1.m_Start].m_Position.y
              };
              curveData1.m_Bezier = NetUtils.StraightCurve(startPos, float3, componentData1.m_Hanging);
            }
            else
              curveData1.m_Bezier = NetUtils.StraightCurve(curve.m_Bezier.a, float3, componentData1.m_Hanging);
          }
          if (edge2.m_End != Entity.Null)
          {
            if ((double) curveRange.z < 1.0)
            {
              // ISSUE: reference to a compiler-generated field
              float3 endPos = MathUtils.Position(curve.m_Bezier, curveRange.z) with
              {
                y = this.m_NodeData[edge2.m_End].m_Position.y
              };
              curveData2.m_Bezier = NetUtils.StraightCurve(float3, endPos, componentData1.m_Hanging);
            }
            else
              curveData2.m_Bezier = NetUtils.StraightCurve(float3, curve.m_Bezier.d, componentData1.m_Hanging);
          }
        }
        else
        {
          MathUtils.Divide(curve.m_Bezier, out curveData1.m_Bezier, out curveData2.m_Bezier, curveRange.y);
          if (edge1.m_Start != Entity.Null && (double) curveRange.x > 0.0)
          {
            curveData1.m_Bezier = MathUtils.Cut(curve.m_Bezier, curveRange.xy);
            // ISSUE: reference to a compiler-generated field
            curveData1.m_Bezier.a.y = this.m_NodeData[edge1.m_Start].m_Position.y;
          }
          if (edge2.m_End != Entity.Null && (double) curveRange.z < 1.0)
          {
            curveData2.m_Bezier = MathUtils.Cut(curve.m_Bezier, curveRange.yz);
            // ISSUE: reference to a compiler-generated field
            curveData2.m_Bezier.d.y = this.m_NodeData[edge2.m_End].m_Position.y;
          }
          curveData1.m_Bezier.d.y = y;
          curveData2.m_Bezier.a.y = y;
        }
        curveData1.m_Length = MathUtils.Length(curveData1.m_Bezier);
        curveData2.m_Length = MathUtils.Length(curveData2.m_Bezier);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> connectedNode = this.m_ConnectedNodes[edge];
        Elevation componentData3;
        // ISSUE: reference to a compiler-generated field
        this.m_ElevationData.TryGetComponent(edge, out componentData3);
        Upgraded componentData4;
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradedData.TryGetComponent(edge, out componentData4);
        DynamicBuffer<SubReplacement> bufferData;
        // ISSUE: reference to a compiler-generated field
        this.m_SubReplacements.TryGetBuffer(edge, out bufferData);
        Owner componentData5;
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerData.TryGetComponent(edge, out componentData5);
        Aggregated componentData6 = new Aggregated();
        if (componentData1.m_AggregateType != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AggregatedData.TryGetComponent(edge, out componentData6);
        }
        NetCondition condition1 = new NetCondition();
        NetCondition condition2 = new NetCondition();
        NetCondition componentData7;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConditionData.TryGetComponent(edge, out componentData7))
        {
          condition1.m_Wear.x = (double) curveRange.x <= 0.0 ? componentData7.m_Wear.x : math.lerp(componentData7.m_Wear.x, componentData7.m_Wear.y, curveRange.x);
          condition1.m_Wear.y = math.lerp(condition1.m_Wear.x, condition1.m_Wear.y, curveRange.y);
          condition2.m_Wear.x = condition1.m_Wear.y;
          condition2.m_Wear.y = (double) curveRange.z >= 1.0 ? componentData7.m_Wear.y : math.lerp(componentData7.m_Wear.x, componentData7.m_Wear.y, curveRange.z);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool addTramTrack = this.m_TramTrackData.HasComponent(edge) && !this.m_PrefabTrackData.HasComponent(prefabRef.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        bool addNative = this.m_NativeData.HasComponent(edge);
        // ISSUE: reference to a compiler-generated field
        bool addElectricityConnection = this.m_ElectricityConnectionData.HasComponent(edge);
        Road road1 = new Road();
        Road road2 = new Road();
        Road componentData8;
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoadData.TryGetComponent(edge, out componentData8))
        {
          road1 = componentData8;
          road2 = componentData8;
          if ((double) curveRange.x > 0.0)
          {
            road1.m_TrafficFlowDistance0 = math.lerp(componentData8.m_TrafficFlowDistance0, componentData8.m_TrafficFlowDistance1, curveRange.x);
            road1.m_TrafficFlowDuration0 = math.lerp(componentData8.m_TrafficFlowDuration0, componentData8.m_TrafficFlowDuration1, curveRange.x);
          }
          road1.m_TrafficFlowDistance1 = math.lerp(componentData8.m_TrafficFlowDistance0, componentData8.m_TrafficFlowDistance1, curveRange.y);
          road1.m_TrafficFlowDuration1 = math.lerp(componentData8.m_TrafficFlowDuration0, componentData8.m_TrafficFlowDuration1, curveRange.y);
          road2.m_TrafficFlowDistance0 = road1.m_TrafficFlowDistance1;
          road2.m_TrafficFlowDuration0 = road1.m_TrafficFlowDuration1;
          if ((double) curveRange.z < 1.0)
          {
            road2.m_TrafficFlowDistance1 = math.lerp(componentData8.m_TrafficFlowDistance0, componentData8.m_TrafficFlowDistance1, curveRange.z);
            road2.m_TrafficFlowDuration1 = math.lerp(componentData8.m_TrafficFlowDuration0, componentData8.m_TrafficFlowDuration1, curveRange.z);
          }
        }
        PseudoRandomSeed pseudoRandomSeed1 = new PseudoRandomSeed();
        PseudoRandomSeed pseudoRandomSeed2 = new PseudoRandomSeed();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PseudoRandomSeedData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          Unity.Mathematics.Random random = this.m_PseudoRandomSeedData[edge].GetRandom((uint) PseudoRandomSeed.kSplitEdge);
          pseudoRandomSeed1 = new PseudoRandomSeed(ref random);
          pseudoRandomSeed2 = new PseudoRandomSeed(ref random);
        }
        EditorContainer editorContainer = new EditorContainer();
        // ISSUE: reference to a compiler-generated field
        if (this.m_EditorContainerData.HasComponent(edge))
        {
          // ISSUE: reference to a compiler-generated field
          editorContainer = this.m_EditorContainerData[edge];
        }
        if ((componentData1.m_Flags & GeometryFlags.SnapCellSize) != (GeometryFlags) 0)
        {
          if ((double) curveRange.x > 0.0)
          {
            Bezier4x3 output1;
            MathUtils.Divide(curve.m_Bezier, out output1, out Bezier4x3 _, curveRange.x);
            if (((uint) (int) math.round(MathUtils.Length(output1) / 4f) & 1U) > 0U != ((componentData8.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0))
              road1.m_Flags |= Game.Net.RoadFlags.StartHalfAligned;
            else
              road1.m_Flags &= ~Game.Net.RoadFlags.StartHalfAligned;
          }
          if (((uint) (int) math.round(curveData1.m_Length / 4f) & 1U) > 0U != ((road1.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0))
          {
            road1.m_Flags |= Game.Net.RoadFlags.EndHalfAligned;
            road2.m_Flags |= Game.Net.RoadFlags.StartHalfAligned;
          }
          else
          {
            road1.m_Flags &= ~Game.Net.RoadFlags.EndHalfAligned;
            road2.m_Flags &= ~Game.Net.RoadFlags.StartHalfAligned;
          }
          if ((double) curveRange.z < 1.0)
          {
            if (((uint) (int) math.round(curveData2.m_Length / 4f) & 1U) > 0U != ((road2.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0))
              road2.m_Flags |= Game.Net.RoadFlags.EndHalfAligned;
            else
              road2.m_Flags &= ~Game.Net.RoadFlags.EndHalfAligned;
          }
        }
        if (edge1.m_Start != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateTempEdge(jobIndex, edge1, curveData1, componentData3, componentData4, bufferData, componentData5, componentData2, componentData6, condition1, addTramTrack, addNative, addElectricityConnection, component, road1, pseudoRandomSeed1, prefabRef, componentData1, buildOrderData1, editorContainer, connectedNode);
        }
        if (!(edge2.m_End != Entity.Null))
          return;
        // ISSUE: reference to a compiler-generated method
        this.CreateTempEdge(jobIndex, edge2, curveData2, componentData3, componentData4, bufferData, componentData5, componentData2, componentData6, condition2, addTramTrack, addNative, addElectricityConnection, component, road2, pseudoRandomSeed2, prefabRef, componentData1, buildOrderData2, editorContainer, connectedNode);
      }

      private void CreateTempEdge(
        int jobIndex,
        Edge edge,
        Curve curveData,
        Elevation elevation,
        Upgraded upgraded,
        DynamicBuffer<SubReplacement> subReplacements,
        Owner owner,
        Fixed fixedData,
        Aggregated aggregated,
        NetCondition condition,
        bool addTramTrack,
        bool addNative,
        bool addElectricityConnection,
        bool hasGeometry,
        Road road,
        PseudoRandomSeed pseudoRandomSeed,
        PrefabRef prefabRef,
        NetGeometryData netGeometryData,
        Game.Net.BuildOrder buildOrderData,
        EditorContainer editorContainer,
        DynamicBuffer<ConnectedNode> oldNodes)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_PrefabData.IsComponentEnabled(prefabRef.m_Prefab);
        Composition component = new Composition();
        component.m_Edge = prefabRef.m_Prefab;
        component.m_StartNode = prefabRef.m_Prefab;
        component.m_EndNode = prefabRef.m_Prefab;
        Temp temp = new Temp();
        temp.m_Flags |= TempFlags.Essential;
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefabRef.m_Prefab];
        OwnerDefinition ownerDefinition = new OwnerDefinition();
        Entity oldEntity;
        // ISSUE: reference to a compiler-generated method
        if (this.TryGetOldEntity(edge, prefabRef.m_Prefab, editorContainer.m_Prefab, Entity.Null, ref ownerDefinition, ref owner.m_Owner, out oldEntity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldEntity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldEntity, new Updated());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          oldEntity = this.m_CommandBuffer.CreateEntity(jobIndex, netData.m_EdgeArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, oldEntity, prefabRef);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Edge>(jobIndex, oldEntity, edge);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Curve>(jobIndex, oldEntity, curveData);
        if (hasGeometry)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Composition>(jobIndex, oldEntity, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Net.BuildOrder>(jobIndex, oldEntity, buildOrderData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, pseudoRandomSeed);
        }
        if (math.any(elevation.m_Elevation != 0.0f))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, oldEntity, elevation);
        }
        if (upgraded.m_Flags != new CompositionFlags())
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(jobIndex, oldEntity, upgraded);
        }
        if (subReplacements.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddBuffer<SubReplacement>(jobIndex, oldEntity).CopyFrom(subReplacements);
        }
        bool isStandalone = true;
        if (owner.m_Owner != Entity.Null)
        {
          isStandalone = false;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, oldEntity, owner);
        }
        if (fixedData.m_Index >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Fixed>(jobIndex, oldEntity, fixedData);
        }
        if (aggregated.m_Aggregate != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Aggregated>(jobIndex, oldEntity, aggregated);
        }
        if (flag && math.any(condition.m_Wear != 0.0f))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NetCondition>(jobIndex, oldEntity, condition);
        }
        if (addTramTrack)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<TramTrack>(jobIndex, oldEntity, new TramTrack());
        }
        if (addNative)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Native>(jobIndex, oldEntity, new Native());
        }
        if (addElectricityConnection)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Game.Net.ElectricityConnection>(jobIndex, oldEntity, new Game.Net.ElectricityConnection());
        }
        if (editorContainer.m_Prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<EditorContainer>(jobIndex, oldEntity, editorContainer);
          if (!hasGeometry)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, pseudoRandomSeed);
          }
        }
        bool isZoneable = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRoadData.HasComponent(prefabRef.m_Prefab))
        {
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Road>(jobIndex, oldEntity, road);
          }
          // ISSUE: reference to a compiler-generated field
          isZoneable = (this.m_PrefabRoadData[prefabRef.m_Prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0;
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedNode> nodes = this.m_CommandBuffer.SetBuffer<ConnectedNode>(jobIndex, oldEntity);
        // ISSUE: reference to a compiler-generated method
        this.FindNodeConnections(nodes, edge, curveData, temp, netData, netGeometryData, false, isStandalone, isZoneable);
        for (int index = 0; index < oldNodes.Length; ++index)
        {
          Entity entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          if (!this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(oldNodes[index].m_Node), out entity, out NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> _))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NodeData[entity];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef1 = this.m_PrefabRefData[entity];
            // ISSUE: reference to a compiler-generated field
            LocalConnectData localConnectData = this.m_LocalConnectData[prefabRef1.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            NetGeometryData netGeometryData1 = this.m_NetGeometryData[prefabRef1.m_Prefab];
            float num1 = math.max(0.0f, netGeometryData1.m_DefaultWidth * 0.5f + localConnectData.m_SearchDistance);
            float t;
            float num2 = MathUtils.Distance(curveData.m_Bezier.xz, node.m_Position.xz, out t);
            // ISSUE: reference to a compiler-generated field
            if ((!this.m_OwnerData.HasComponent(entity) || (double) localConnectData.m_SearchDistance == 0.0 ? 1 : ((netGeometryData1.m_Flags & GeometryFlags.SubOwner) != 0 ? 1 : 0)) == 0 & isStandalone & isZoneable)
              num2 -= 8f;
            if ((double) num2 <= (double) netGeometryData.m_DefaultWidth * 0.5 + (double) num1)
            {
              float position = MathUtils.Position(curveData.m_Bezier, t).y - node.m_Position.y;
              if (MathUtils.Intersect(localConnectData.m_HeightRange, position))
                nodes.Add(new ConnectedNode(entity, t));
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(jobIndex, oldEntity, temp);
      }

      private bool TryGetNode(
        CoursePos coursePos,
        bool isPermanent,
        bool isEditor,
        out Entity node)
      {
        // ISSUE: reference to a compiler-generated field
        if (isPermanent && this.m_NodeData.HasComponent(coursePos.m_Entity))
        {
          node = coursePos.m_Entity;
          return true;
        }
        float num1 = float.MaxValue;
        node = Entity.Null;
        Entity entity;
        NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> it;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        if (this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(coursePos, isPermanent, isEditor), out entity, out it))
        {
          // ISSUE: reference to a compiler-generated field
          do
          {
            Temp componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.TryGetComponent(entity, out componentData))
            {
              float num2 = math.abs(componentData.m_CurvePosition - coursePos.m_SplitPosition);
              if ((double) num2 < (double) num1)
              {
                num1 = num2;
                node = entity;
              }
            }
            else if (entity != Entity.Null)
            {
              node = entity;
              return true;
            }
          }
          while (this.m_NodeMap.TryGetNextValue(out entity, ref it));
        }
        return node != Entity.Null;
      }

      private void GenerateEdge(
        int jobIndex,
        int entityIndex,
        CreationDefinition definitionData,
        OwnerDefinition ownerData,
        NetCourse course,
        Upgraded upgraded,
        LocalCurveCache cachedCurve,
        bool hasCachedCurve,
        DynamicBuffer<SubReplacement> subReplacements)
      {
        bool isPermanent = (definitionData.m_Flags & CreationFlags.Permanent) > (CreationFlags) 0;
        bool isEditor = definitionData.m_SubPrefab != Entity.Null;
        Edge edge = new Edge();
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (((course.m_StartPosition.m_Flags | course.m_EndPosition.m_Flags) & CoursePosFlags.DontCreate) != (CoursePosFlags) 0 || !this.TryGetNode(course.m_StartPosition, isPermanent, isEditor, out edge.m_Start) || !this.TryGetNode(course.m_EndPosition, isPermanent, isEditor, out edge.m_End) || edge.m_Start == edge.m_End)
          return;
        Entity entity;
        NativeParallelMultiHashMapIterator<GenerateEdgesSystem.NodeMapKey> it;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        if (definitionData.m_Original != Entity.Null && this.m_NodeMap.TryGetFirstValue(new GenerateEdgesSystem.NodeMapKey(definitionData.m_Original), out entity, out it))
        {
          while (!(entity != Entity.Null))
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_NodeMap.TryGetNextValue(out entity, ref it))
              goto label_6;
          }
          definitionData.m_Original = Entity.Null;
        }
label_6:
        // ISSUE: reference to a compiler-generated method
        if (course.m_StartPosition.m_Entity != Entity.Null && course.m_EndPosition.m_Entity != Entity.Null && definitionData.m_Original == Entity.Null && this.ConnectionExists(course.m_StartPosition.m_Entity, course.m_EndPosition.m_Entity))
          return;
        PrefabRef prefabRef = new PrefabRef();
        PrefabRef originalPrefabRef = new PrefabRef();
        if (definitionData.m_Original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          originalPrefabRef = this.m_PrefabRefData[definitionData.m_Original];
        }
        if (definitionData.m_Prefab == Entity.Null)
        {
          if (!(definitionData.m_Original != Entity.Null))
            return;
          prefabRef = originalPrefabRef;
        }
        else
          prefabRef.m_Prefab = definitionData.m_Prefab;
        // ISSUE: reference to a compiler-generated field
        bool flag1 = this.m_PrefabData.IsComponentEnabled(prefabRef.m_Prefab);
        Composition component1 = new Composition();
        component1.m_Edge = prefabRef.m_Prefab;
        component1.m_StartNode = prefabRef.m_Prefab;
        component1.m_EndNode = prefabRef.m_Prefab;
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefabRef.m_Prefab];
        NetGeometryData netGeometryData = new NetGeometryData();
        bool flag2 = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
          flag2 = true;
        }
        float3 startPos;
        float3 endPos;
        if ((netGeometryData.m_Flags & GeometryFlags.StrictNodes) != (GeometryFlags) 0 || !flag2)
        {
          // ISSUE: reference to a compiler-generated field
          startPos = this.m_NodeData[edge.m_Start].m_Position;
          // ISSUE: reference to a compiler-generated field
          endPos = this.m_NodeData[edge.m_End].m_Position;
        }
        else
        {
          startPos = MathUtils.Position(course.m_Curve, course.m_StartPosition.m_CourseDelta);
          endPos = MathUtils.Position(course.m_Curve, course.m_EndPosition.m_CourseDelta);
          // ISSUE: reference to a compiler-generated field
          startPos.y = this.m_NodeData[edge.m_Start].m_Position.y;
          // ISSUE: reference to a compiler-generated field
          endPos.y = this.m_NodeData[edge.m_End].m_Position.y;
        }
        Curve curve1 = new Curve();
        if ((netGeometryData.m_Flags & GeometryFlags.StraightEdges) != (GeometryFlags) 0 && course.m_FixedIndex < 0)
        {
          curve1.m_Bezier = NetUtils.StraightCurve(startPos, endPos, netGeometryData.m_Hanging);
        }
        else
        {
          curve1.m_Bezier = MathUtils.Cut(course.m_Curve, new float2(course.m_StartPosition.m_CourseDelta, course.m_EndPosition.m_CourseDelta));
          curve1.m_Bezier.a = startPos;
          curve1.m_Bezier.d = endPos;
        }
        curve1.m_Length = MathUtils.Length(curve1.m_Bezier);
        Temp temp = new Temp();
        bool flag3 = false;
        if (definitionData.m_Original != Entity.Null)
        {
          temp.m_Original = definitionData.m_Original;
          if ((definitionData.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
            temp.m_Flags |= TempFlags.Delete;
          else if ((definitionData.m_Flags & CreationFlags.Select) != (CreationFlags) 0)
            temp.m_Flags |= TempFlags.Select;
          else if ((definitionData.m_Flags & CreationFlags.Upgrade) != (CreationFlags) 0)
            temp.m_Flags |= TempFlags.Upgrade;
          else if (prefabRef.m_Prefab != originalPrefabRef.m_Prefab || (definitionData.m_Flags & CreationFlags.Invert) != (CreationFlags) 0)
            temp.m_Flags |= TempFlags.Modify;
          if ((definitionData.m_Flags & CreationFlags.Parent) != (CreationFlags) 0)
            temp.m_Flags |= TempFlags.Parent;
          if ((definitionData.m_Flags & CreationFlags.Upgrade) == (CreationFlags) 0)
          {
            Upgraded componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_UpgradedData.TryGetComponent(definitionData.m_Original, out componentData))
            {
              upgraded = componentData;
              if ((definitionData.m_Flags & CreationFlags.Invert) != (CreationFlags) 0)
                upgraded.m_Flags = NetCompositionHelpers.InvertCompositionFlags(upgraded.m_Flags);
            }
            DynamicBuffer<SubReplacement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubReplacements.TryGetBuffer(definitionData.m_Original, out bufferData))
            {
              subReplacements = bufferData;
              flag3 = (definitionData.m_Flags & CreationFlags.Invert) > (CreationFlags) 0;
            }
          }
        }
        else
          temp.m_Flags |= TempFlags.Create;
        if ((definitionData.m_Flags & CreationFlags.Hidden) != (CreationFlags) 0)
          temp.m_Flags |= TempFlags.Hidden;
        if (definitionData.m_Original != Entity.Null)
        {
          course.m_Elevation = (float2) 0.0f;
          course.m_FixedIndex = -1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.HasComponent(definitionData.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            course.m_Elevation = this.m_ElevationData[definitionData.m_Original].m_Elevation;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_FixedData.HasComponent(definitionData.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            course.m_FixedIndex = this.m_FixedData[definitionData.m_Original].m_Index;
          }
          if ((definitionData.m_Flags & CreationFlags.Invert) != (CreationFlags) 0)
            course.m_Elevation = course.m_Elevation.yx;
        }
        int num = math.any(course.m_Elevation != 0.0f) ? 1 : (course.m_StartPosition.m_ParentMesh < 0 ? 0 : (course.m_EndPosition.m_ParentMesh >= 0 ? 1 : 0));
        if (num == 0 && ((netGeometryData.m_Flags & GeometryFlags.FlattenTerrain) == (GeometryFlags) 0 || ownerData.m_Prefab != Entity.Null || definitionData.m_Owner != Entity.Null))
        {
          // ISSUE: reference to a compiler-generated field
          bool fixedStart = this.m_ElevationData.HasComponent(edge.m_Start);
          // ISSUE: reference to a compiler-generated field
          bool fixedEnd = this.m_ElevationData.HasComponent(edge.m_End);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Curve curve2 = (netGeometryData.m_Flags & GeometryFlags.OnWater) == (GeometryFlags) 0 ? NetUtils.AdjustPosition(curve1, fixedStart, fixedStart | fixedEnd, fixedEnd, ref this.m_TerrainHeightData) : NetUtils.AdjustPosition(curve1, fixedStart, fixedStart | fixedEnd, fixedEnd, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData);
          if (math.any(math.abs(curve2.m_Bezier.y.abcd - curve1.m_Bezier.y.abcd) >= 0.01f))
            curve1 = curve2;
        }
        Entity oldEntity = Entity.Null;
        // ISSUE: reference to a compiler-generated method
        bool flag4 = !isPermanent && this.TryGetOldEntity(edge, prefabRef.m_Prefab, definitionData.m_SubPrefab, definitionData.m_Original, ref ownerData, ref definitionData.m_Owner, out oldEntity);
        if (flag4)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(jobIndex, oldEntity);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, oldEntity, new Updated());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          oldEntity = this.m_CommandBuffer.CreateEntity(jobIndex, netData.m_EdgeArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, oldEntity, prefabRef);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Edge>(jobIndex, oldEntity, edge);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Curve>(jobIndex, oldEntity, curve1);
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Composition>(jobIndex, oldEntity, component1);
          PseudoRandomSeed componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PseudoRandomSeedData.TryGetComponent(definitionData.m_Original, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, componentData);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, new PseudoRandomSeed((ushort) definitionData.m_RandomSeed));
          }
        }
        if (num != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Elevation>(jobIndex, oldEntity, new Elevation(course.m_Elevation));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (flag4 && this.m_ElevationData.HasComponent(oldEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Elevation>(jobIndex, oldEntity);
          }
        }
        if (upgraded.m_Flags != new CompositionFlags())
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(jobIndex, oldEntity, upgraded);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (flag4 && this.m_UpgradedData.HasComponent(oldEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Upgraded>(jobIndex, oldEntity);
          }
        }
        if (subReplacements.IsCreated && subReplacements.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubReplacement> dynamicBuffer = this.m_CommandBuffer.AddBuffer<SubReplacement>(jobIndex, oldEntity);
          for (int index = 0; index < subReplacements.Length; ++index)
          {
            SubReplacement subReplacement = subReplacements[index];
            if (flag3)
              subReplacement.m_Side = (SubReplacementSide) -(int) subReplacement.m_Side;
            dynamicBuffer.Add(subReplacement);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (flag4 && this.m_SubReplacements.HasBuffer(oldEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<SubReplacement>(jobIndex, oldEntity);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRoadData.HasComponent(prefabRef.m_Prefab))
        {
          if (((upgraded.m_Flags.m_Left | upgraded.m_Flags.m_Right) & CompositionFlags.Side.PrimaryTrack) != (CompositionFlags.Side) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<TramTrack>(jobIndex, oldEntity, new TramTrack());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (flag4 && this.m_TramTrackData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<TramTrack>(jobIndex, oldEntity);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_NativeData.HasComponent(definitionData.m_Original) && (temp.m_Flags & (TempFlags.Modify | TempFlags.Upgrade)) == (TempFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Native>(jobIndex, oldEntity, new Native());
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (flag4 && this.m_NativeData.HasComponent(oldEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Native>(jobIndex, oldEntity);
          }
        }
        bool flag5 = true;
        if (ownerData.m_Prefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, oldEntity, new Owner());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(jobIndex, oldEntity, ownerData);
          flag5 = false;
        }
        else if (definitionData.m_Owner != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Owner>(jobIndex, oldEntity, new Owner(definitionData.m_Owner));
          flag5 = false;
        }
        bool isStandalone = flag5 | (netGeometryData.m_Flags & GeometryFlags.SubOwner) != 0;
        if (isStandalone)
          temp.m_Flags |= TempFlags.Essential;
        if (course.m_FixedIndex >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Fixed>(jobIndex, oldEntity, new Fixed()
          {
            m_Index = course.m_FixedIndex
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (flag4 && this.m_FixedData.HasComponent(oldEntity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Fixed>(jobIndex, oldEntity);
          }
        }
        if (definitionData.m_Original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Hidden>(jobIndex, definitionData.m_Original, new Hidden());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, definitionData.m_Original, new BatchesUpdated());
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Game.Net.BuildOrder>(jobIndex, oldEntity, this.m_BuildOrderData[definitionData.m_Original]);
          }
        }
        else if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.BuildOrder component2 = new Game.Net.BuildOrder()
          {
            m_Start = this.m_BuildOrder + (uint) (entityIndex * 16)
          };
          component2.m_End = component2.m_Start + 15U;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Net.BuildOrder>(jobIndex, oldEntity, component2);
        }
        bool isZoneable = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRoadData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          isZoneable = (this.m_PrefabRoadData[prefabRef.m_Prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0;
          if (flag1)
          {
            Road road = new Road();
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoadData.HasComponent(definitionData.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              road = this.m_RoadData[definitionData.m_Original];
              // ISSUE: reference to a compiler-generated method
              this.CheckRoadAlignment(definitionData, prefabRef, originalPrefabRef, netGeometryData, ref road);
            }
            // ISSUE: reference to a compiler-generated method
            this.SetRoadAlignment(course, ref road);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Road>(jobIndex, oldEntity, road);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FindNodeConnections(this.m_CommandBuffer.SetBuffer<ConnectedNode>(jobIndex, oldEntity), edge, curve1, temp, netData, netGeometryData, false, isStandalone, isZoneable);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabElectricityConnectionData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          bool flag6 = NetCompositionHelpers.TestEdgeFlags(this.m_PrefabElectricityConnectionData[prefabRef.m_Prefab], upgraded.m_Flags);
          if (flag6)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Game.Net.ElectricityConnection>(jobIndex, oldEntity, new Game.Net.ElectricityConnection());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (flag4 && this.m_ElectricityConnectionData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Game.Net.ElectricityConnection>(jobIndex, oldEntity);
            }
          }
          if (definitionData.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            bool flag7 = this.m_ElectricityConnectionData.HasComponent(definitionData.m_Original);
            if (flag6 != flag7)
            {
              temp.m_Flags &= ~TempFlags.Upgrade;
              temp.m_Flags |= TempFlags.Replace;
            }
          }
        }
        if (definitionData.m_Original != Entity.Null && originalPrefabRef.m_Prefab != prefabRef.m_Prefab)
        {
          bool flag8 = false;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRoadData.HasComponent(originalPrefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            flag8 = (this.m_PrefabRoadData[originalPrefabRef.m_Prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0;
          }
          if (isZoneable != flag8)
          {
            temp.m_Flags &= ~TempFlags.Modify;
            temp.m_Flags |= TempFlags.Replace;
          }
        }
        Aggregated componentData1;
        // ISSUE: reference to a compiler-generated field
        if (netGeometryData.m_AggregateType != Entity.Null && this.m_AggregatedData.TryGetComponent(definitionData.m_Original, out componentData1))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Aggregated>(jobIndex, oldEntity, componentData1);
        }
        NetCondition componentData2;
        // ISSUE: reference to a compiler-generated field
        if (flag1 && this.m_ConditionData.TryGetComponent(definitionData.m_Original, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NetCondition>(jobIndex, oldEntity, componentData2);
        }
        if (hasCachedCurve)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<LocalCurveCache>(jobIndex, oldEntity, cachedCurve);
        }
        if (definitionData.m_SubPrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<EditorContainer>(jobIndex, oldEntity, new EditorContainer()
          {
            m_Prefab = definitionData.m_SubPrefab
          });
          if (!flag2)
          {
            PseudoRandomSeed componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.TryGetComponent(definitionData.m_Original, out componentData3))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, componentData3);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(jobIndex, oldEntity, new PseudoRandomSeed((ushort) definitionData.m_RandomSeed));
            }
          }
        }
        if (isPermanent)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(jobIndex, oldEntity, temp);
      }

      private bool TryGetOldEntity(
        Edge edge,
        Entity prefab,
        Entity subPrefab,
        Entity original,
        ref OwnerDefinition ownerDefinition,
        ref Entity owner,
        out Entity oldEntity)
      {
        Entity entity;
        // ISSUE: reference to a compiler-generated field
        if (ownerDefinition.m_Prefab != Entity.Null && this.m_ReusedOwnerMap.TryGetValue(ownerDefinition, out entity))
        {
          owner = entity;
          ownerDefinition = new OwnerDefinition();
        }
        // ISSUE: variable of a compiler-generated type
        GenerateEdgesSystem.OldEdgeKey key;
        // ISSUE: reference to a compiler-generated field
        key.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        key.m_SubPrefab = subPrefab;
        // ISSUE: reference to a compiler-generated field
        key.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        key.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        key.m_StartNode = edge.m_Start;
        // ISSUE: reference to a compiler-generated field
        key.m_EndNode = edge.m_End;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldEdgeMap.TryGetValue(key, out oldEntity))
          return true;
        oldEntity = Entity.Null;
        return false;
      }

      private void CheckRoadAlignment(
        CreationDefinition definitionData,
        PrefabRef prefabRefData,
        PrefabRef originalPrefabRef,
        NetGeometryData netGeometryData,
        ref Road road)
      {
        bool2 bool2 = new bool2((road.m_Flags & Game.Net.RoadFlags.StartHalfAligned) != 0, (road.m_Flags & Game.Net.RoadFlags.EndHalfAligned) != 0);
        road.m_Flags &= ~(Game.Net.RoadFlags.StartHalfAligned | Game.Net.RoadFlags.EndHalfAligned);
        if ((definitionData.m_Flags & CreationFlags.Align) == (CreationFlags) 0)
          return;
        bool2 = (definitionData.m_Flags & CreationFlags.Invert) != (CreationFlags) 0 ? bool2.yx : bool2;
        if (prefabRefData.m_Prefab != originalPrefabRef.m_Prefab)
        {
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData1 = this.m_NetGeometryData[originalPrefabRef.m_Prefab];
          int cellWidth1 = ZoneUtils.GetCellWidth(netGeometryData.m_DefaultWidth);
          int cellWidth2 = ZoneUtils.GetCellWidth(netGeometryData1.m_DefaultWidth);
          bool2 ^= ((cellWidth1 ^ cellWidth2) & 1) != 0;
        }
        if (bool2.x)
          road.m_Flags |= Game.Net.RoadFlags.StartHalfAligned;
        if (!bool2.y)
          return;
        road.m_Flags |= Game.Net.RoadFlags.EndHalfAligned;
      }

      private void SetRoadAlignment(NetCourse course, ref Road road)
      {
        if ((course.m_StartPosition.m_Flags & CoursePosFlags.HalfAlign) != (CoursePosFlags) 0)
          road.m_Flags ^= Game.Net.RoadFlags.StartHalfAligned;
        if ((course.m_EndPosition.m_Flags & CoursePosFlags.HalfAlign) == (CoursePosFlags) 0)
          return;
        road.m_Flags ^= Game.Net.RoadFlags.EndHalfAligned;
      }

      private bool ConnectionExists(Entity node1, Entity node2)
      {
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.TryGetBuffer(node1, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity edge1 = bufferData[index].m_Edge;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(edge1))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge2 = this.m_EdgeData[edge1];
              if (edge2.m_Start == node2 && edge2.m_End == node1 || edge2.m_End == node2 && edge2.m_Start == node1)
                return true;
            }
          }
        }
        return false;
      }

      private void FindNodeConnections(
        DynamicBuffer<ConnectedNode> nodes,
        Edge edgeData,
        Curve curveData,
        Temp tempData,
        NetData netData,
        NetGeometryData netGeometryData,
        bool isPermanent,
        bool isStandalone,
        bool isZoneable)
      {
        float range = netGeometryData.m_DefaultWidth * 0.5f + math.select(0.0f, 8f, isStandalone & isZoneable);
        Bounds3 bounds1 = MathUtils.Expand(MathUtils.Bounds(curveData.m_Bezier), (float3) range);
        float3 float3_1 = new float3();
        Roundabout componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoundaboutData.TryGetComponent(edgeData.m_Start, out componentData1))
        {
          // ISSUE: reference to a compiler-generated field
          float3_1 = this.m_NodeData[edgeData.m_Start].m_Position;
          bounds1 |= new Bounds3(float3_1 - componentData1.m_Radius, float3_1 + componentData1.m_Radius);
        }
        float3 float3_2 = new float3();
        Roundabout componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoundaboutData.TryGetComponent(edgeData.m_End, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          float3_2 = this.m_NodeData[edgeData.m_End].m_Position;
          bounds1 |= new Bounds3(float3_2 - componentData2.m_Radius, float3_2 + componentData2.m_Radius);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_LocalConnectList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GenerateEdgesSystem.LocalConnectItem localConnect = this.m_LocalConnectList[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((localConnect.m_ConnectLayers & netData.m_ConnectLayers) != Layer.None && (localConnect.m_LocalConnectLayers & netData.m_LocalConnectLayers) != Layer.None && !(localConnect.m_Node == edgeData.m_Start) && !(localConnect.m_Node == edgeData.m_End) && ((tempData.m_Flags ^ localConnect.m_TempFlags) & TempFlags.Delete) == (TempFlags) 0 && localConnect.m_IsPermanent == isPermanent)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(bounds1, new Bounds3(localConnect.m_Position - localConnect.m_Radius, localConnect.m_Position + localConnect.m_Radius)
            {
              y = localConnect.m_Position.y + localConnect.m_HeightRange
            }))
            {
              float num1;
              float t;
              if ((netGeometryData.m_Flags & GeometryFlags.NoEdgeConnection) != (GeometryFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                float a = math.distance(curveData.m_Bezier.a.xz, localConnect.m_Position.xz);
                // ISSUE: reference to a compiler-generated field
                float b = math.distance(curveData.m_Bezier.d.xz, localConnect.m_Position.xz);
                num1 = math.select(a, b, (double) b < (double) a);
                t = math.select(0.0f, 1f, (double) b < (double) a);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                num1 = MathUtils.Distance(curveData.m_Bezier.xz, localConnect.m_Position.xz, out t);
              }
              float num2 = num1 - netGeometryData.m_DefaultWidth * 0.5f;
              if ((double) componentData1.m_Radius != 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                float num3 = math.distance(float3_1.xz, localConnect.m_Position.xz) - componentData1.m_Radius;
                if ((double) num3 < (double) num2)
                {
                  num2 = num3;
                  t = 0.0f;
                }
              }
              if ((double) componentData2.m_Radius != 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                float num4 = math.distance(float3_2.xz, localConnect.m_Position.xz) - componentData2.m_Radius;
                if ((double) num4 < (double) num2)
                {
                  num2 = num4;
                  t = 1f;
                }
              }
              // ISSUE: reference to a compiler-generated field
              if (!localConnect.m_IsStandalone & isStandalone & isZoneable)
                num2 -= 8f;
              // ISSUE: reference to a compiler-generated field
              if ((double) num2 <= (double) localConnect.m_Radius)
              {
                // ISSUE: reference to a compiler-generated field
                float position = MathUtils.Position(curveData.m_Bezier, t).y - localConnect.m_Position.y;
                // ISSUE: reference to a compiler-generated field
                if (MathUtils.Intersect(localConnect.m_HeightRange, position))
                {
                  // ISSUE: reference to a compiler-generated field
                  nodes.Add(new ConnectedNode(localConnect.m_Node, t));
                }
              }
            }
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
    private struct UpdateBuildOrderJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.BuildOrder> m_BuildOrderType;
      public NativeValue<uint> m_BuildOrder;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        uint x = this.m_BuildOrder.value;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.BuildOrder> nativeArray = this.m_Chunks[index1].GetNativeArray<Game.Net.BuildOrder>(ref this.m_BuildOrderType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Game.Net.BuildOrder buildOrder = nativeArray[index2];
            x = math.max(x, math.max(buildOrder.m_Start, buildOrder.m_End) + 1U);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_BuildOrder.value = x;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LocalConnect> __Game_Net_LocalConnect_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Elevation> __Game_Net_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> __Game_Tools_NetCourse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LocalCurveCache> __Game_Tools_LocalCurveCache_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> __Game_Net_Upgraded_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubReplacement> __Game_Net_SubReplacement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.BuildOrder> __Game_Net_BuildOrder_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TramTrack> __Game_Net_TramTrack_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCondition> __Game_Net_NetCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aggregated> __Game_Net_Aggregated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Roundabout> __Game_Net_Roundabout_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ElectricityConnection> __Game_Net_ElectricityConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackData> __Game_Prefabs_TrackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubReplacement> __Game_Net_SubReplacement_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.BuildOrder> __Game_Net_BuildOrder_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LocalConnect_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalConnect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_NetCourse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCourse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalCurveCache_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalCurveCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubReplacement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RO_ComponentLookup = state.GetComponentLookup<Game.Net.BuildOrder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TramTrack_RO_ComponentLookup = state.GetComponentLookup<TramTrack>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentLookup = state.GetComponentLookup<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Aggregated_RO_ComponentLookup = state.GetComponentLookup<Aggregated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Roundabout_RO_ComponentLookup = state.GetComponentLookup<Roundabout>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ElectricityConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ElectricityConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackData_RO_ComponentLookup = state.GetComponentLookup<TrackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup = state.GetComponentLookup<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RO_BufferLookup = state.GetBufferLookup<SubReplacement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.BuildOrder>(true);
      }
    }
  }
}
