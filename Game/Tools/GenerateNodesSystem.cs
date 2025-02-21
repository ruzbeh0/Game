// Decompiled with JetBrains decompiler
// Type: Game.Tools.GenerateNodesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
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
  public class GenerateNodesSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private Game.Net.SearchSystem m_SearchSystem;
    private TerrainSystem m_TerrainSystem;
    private GenerateObjectsSystem m_GenerateObjectsSystem;
    private ModificationBarrier1 m_ModificationBarrier;
    private EntityQuery m_DefinitionQuery;
    private EntityQuery m_DeletedQuery;
    private GenerateNodesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateObjectsSystem = this.World.GetOrCreateSystemManaged<GenerateObjectsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier1>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<CreationDefinition>(),
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<NetCourse>(),
          ComponentType.ReadOnly<ObjectDefinition>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Node>(), ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<PrefabRef>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DefinitionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<GenerateNodesSystem.UpdateData> nativeQueue = new NativeQueue<GenerateNodesSystem.UpdateData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<GenerateNodesSystem.UpdateData> nativeList = new NativeList<GenerateNodesSystem.UpdateData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeParallelHashMap<Entity, GenerateNodesSystem.DefinitionData> nativeParallelHashMap = new NativeParallelHashMap<Entity, GenerateNodesSystem.DefinitionData>(this.m_DefinitionQuery.CalculateEntityCount(), (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelMultiHashMap<GenerateNodesSystem.OldNodeKey, GenerateNodesSystem.OldNodeValue> parallelMultiHashMap = new NativeParallelMultiHashMap<GenerateNodesSystem.OldNodeKey, GenerateNodesSystem.OldNodeValue>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeHashMap<OwnerDefinition, Entity> reusedOwnerMap = this.m_GenerateObjectsSystem.GetReusedOwnerMap(out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      Bounds3 bounds = TerrainUtils.GetBounds(ref heightData);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Roundabout_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateNodesSystem.FillNodeMapJob jobData1 = new GenerateNodesSystem.FillNodeMapJob()
      {
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
        m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
        m_LocalCurveCacheType = this.__TypeHandle.__Game_Tools_LocalCurveCache_RO_ComponentTypeHandle,
        m_UpgradedType = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LocalConnectData = this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup,
        m_RoundaboutData = this.__TypeHandle.__Game_Net_Roundabout_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabLocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_Nodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_NodeQueue = nativeQueue.AsParallelWriter(),
        m_DefinitionMap = nativeParallelHashMap.AsParallelWriter(),
        m_NetSearchTree = this.m_SearchSystem.GetNetSearchTree(true, out dependencies2)
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateNodesSystem.FillOldNodesJob jobData2 = new GenerateNodesSystem.FillOldNodesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentTypeHandle,
        m_OutsideConnectionType = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OldNodeMap = parallelMultiHashMap
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateNodesSystem.CollectUpdatesJob jobData3 = new GenerateNodesSystem.CollectUpdatesJob()
      {
        m_UpdateQueue = nativeQueue,
        m_UpdateList = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Standalone_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GenerateNodesSystem.CreateNodesJob jobData4 = new GenerateNodesSystem.CreateNodesJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabLocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_NetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabRoadData = this.__TypeHandle.__Game_Prefabs_RoadData_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_LocalConnectData = this.__TypeHandle.__Game_Net_LocalConnect_RO_ComponentLookup,
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_StandaloneData = this.__TypeHandle.__Game_Net_Standalone_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_ConditionData = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup,
        m_RoadData = this.__TypeHandle.__Game_Net_Road_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_Edges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_TerrainBounds = bounds,
        m_UpdateList = nativeList,
        m_DefinitionMap = nativeParallelHashMap,
        m_ReusedOwnerMap = reusedOwnerMap,
        m_OldNodeMap = parallelMultiHashMap,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<GenerateNodesSystem.FillNodeMapJob>(this.m_DefinitionQuery, JobHandle.CombineDependencies(this.Dependency, dependencies2));
      // ISSUE: reference to a compiler-generated field
      JobHandle job1 = jobData2.Schedule<GenerateNodesSystem.FillOldNodesJob>(this.m_DeletedQuery, this.Dependency);
      JobHandle jobHandle2 = jobData3.Schedule<GenerateNodesSystem.CollectUpdatesJob>(jobHandle1);
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, job1, dependencies1);
      JobHandle jobHandle3 = jobData4.Schedule<GenerateNodesSystem.CreateNodesJob>(dependsOn);
      nativeQueue.Dispose(jobHandle2);
      nativeList.Dispose(jobHandle3);
      nativeParallelHashMap.Dispose(jobHandle3);
      parallelMultiHashMap.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_GenerateObjectsSystem.AddOwnerMapReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle3);
      this.Dependency = jobHandle3;
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
    public GenerateNodesSystem()
    {
    }

    private struct UpdateData : IEquatable<GenerateNodesSystem.UpdateData>
    {
      public bool m_OnCourse;
      public bool m_Regenerate;
      public bool m_HasCachedPosition;
      public bool m_AddEdge;
      public bool m_UpdateOnly;
      public bool m_Valid;
      public float3 m_Position;
      public float3 m_CachedPosition;
      public quaternion m_Rotation;
      public Entity m_Prefab;
      public Entity m_Original;
      public Entity m_Owner;
      public Entity m_Lane;
      public OwnerDefinition m_OwnerData;
      public float m_CurvePosition;
      public Bounds1 m_CurveBounds;
      public float2 m_Elevation;
      public CoursePosFlags m_Flags;
      public CreationFlags m_CreationFlags;
      public CompositionFlags m_UpgradeFlags;
      public int m_FixedIndex;
      public int m_RandomSeed;
      public int m_ParentMesh;

      public UpdateData(Game.Net.Node node, Entity original, bool regenerate, bool updateOnly)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OnCourse = false;
        // ISSUE: reference to a compiler-generated field
        this.m_Regenerate = regenerate;
        // ISSUE: reference to a compiler-generated field
        this.m_HasCachedPosition = false;
        // ISSUE: reference to a compiler-generated field
        this.m_AddEdge = false;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateOnly = updateOnly;
        // ISSUE: reference to a compiler-generated field
        this.m_Valid = true;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = node.m_Position;
        // ISSUE: reference to a compiler-generated field
        this.m_CachedPosition = new float3();
        // ISSUE: reference to a compiler-generated field
        this.m_Rotation = node.m_Rotation;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerData = new OwnerDefinition();
        // ISSUE: reference to a compiler-generated field
        this.m_CurvePosition = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_CurveBounds = new Bounds1(0.0f, 1f);
        // ISSUE: reference to a compiler-generated field
        this.m_Elevation = new float2();
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = (CoursePosFlags) 0;
        // ISSUE: reference to a compiler-generated field
        this.m_CreationFlags = (CreationFlags) 0;
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradeFlags = new CompositionFlags();
        // ISSUE: reference to a compiler-generated field
        this.m_FixedIndex = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_RandomSeed = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_ParentMesh = -1;
      }

      public UpdateData(
        CreationDefinition definitionData,
        OwnerDefinition ownerData,
        CoursePos coursePos,
        Upgraded upgraded,
        int fixedIndex,
        float3 cachedPosition,
        Bounds1 curveBounds,
        bool hasCachedPosition,
        bool addEdge)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OnCourse = true;
        // ISSUE: reference to a compiler-generated field
        this.m_Regenerate = true;
        // ISSUE: reference to a compiler-generated field
        this.m_HasCachedPosition = hasCachedPosition;
        // ISSUE: reference to a compiler-generated field
        this.m_AddEdge = addEdge;
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateOnly = false;
        // ISSUE: reference to a compiler-generated field
        this.m_Valid = true;
        // ISSUE: reference to a compiler-generated field
        this.m_Position = coursePos.m_Position;
        // ISSUE: reference to a compiler-generated field
        this.m_CachedPosition = cachedPosition;
        // ISSUE: reference to a compiler-generated field
        this.m_Rotation = coursePos.m_Rotation;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = definitionData.m_Prefab;
        // ISSUE: reference to a compiler-generated field
        this.m_Original = coursePos.m_Entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Owner = definitionData.m_Owner;
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = definitionData.m_SubPrefab;
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerData = ownerData;
        // ISSUE: reference to a compiler-generated field
        this.m_CurvePosition = coursePos.m_SplitPosition;
        // ISSUE: reference to a compiler-generated field
        this.m_CurveBounds = curveBounds;
        // ISSUE: reference to a compiler-generated field
        this.m_Elevation = coursePos.m_Elevation;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = coursePos.m_Flags;
        // ISSUE: reference to a compiler-generated field
        this.m_CreationFlags = definitionData.m_Flags;
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradeFlags = upgraded.m_Flags;
        // ISSUE: reference to a compiler-generated field
        this.m_FixedIndex = fixedIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_RandomSeed = definitionData.m_RandomSeed;
        // ISSUE: reference to a compiler-generated field
        this.m_ParentMesh = coursePos.m_ParentMesh;
      }

      public bool Equals(GenerateNodesSystem.UpdateData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Original != Entity.Null || other.m_Original != Entity.Null ? this.m_Original.Equals(other.m_Original) : this.m_Position.Equals(other.m_Position);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Original != Entity.Null ? this.m_Original.GetHashCode() : this.m_Position.GetHashCode();
      }
    }

    private struct NodeKey : IEquatable<GenerateNodesSystem.NodeKey>
    {
      public Entity m_Original;
      public float3 m_Position;
      public bool m_IsEditor;

      public NodeKey(GenerateNodesSystem.UpdateData data)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Original = data.m_Original;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Position = data.m_Position;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IsEditor = data.m_Lane != Entity.Null;
      }

      public bool Equals(GenerateNodesSystem.NodeKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Original != Entity.Null || other.m_Original != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_Original.Equals(other.m_Original);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Position.Equals(other.m_Position) && this.m_IsEditor == other.m_IsEditor;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Original != Entity.Null ? this.m_Original.GetHashCode() : this.m_Position.GetHashCode();
      }
    }

    private struct DefinitionData
    {
      public Entity m_Prefab;
      public Entity m_Lane;
      public CreationFlags m_Flags;

      public DefinitionData(Entity prefab, Entity lane, CreationFlags flags)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        this.m_Lane = lane;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = flags;
      }
    }

    private struct OldNodeKey : IEquatable<GenerateNodesSystem.OldNodeKey>
    {
      public Entity m_Prefab;
      public Entity m_SubPrefab;
      public Entity m_Original;
      public Entity m_Owner;
      public bool m_OutsideConnection;

      public bool Equals(GenerateNodesSystem.OldNodeKey other)
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
        return this.m_Prefab.Equals(other.m_Prefab) && this.m_SubPrefab.Equals(other.m_SubPrefab) && this.m_Original.Equals(other.m_Original) && this.m_Owner.Equals(other.m_Owner) && this.m_OutsideConnection == other.m_OutsideConnection;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (((17 * 31 + this.m_Prefab.GetHashCode()) * 31 + this.m_SubPrefab.GetHashCode()) * 31 + this.m_Original.GetHashCode()) * 31 + this.m_Owner.GetHashCode();
      }
    }

    private struct OldNodeValue
    {
      public Entity m_Entity;
      public float3 m_Position;
    }

    [BurstCompile]
    private struct FillOldNodesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> m_EditorContainerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      public NativeParallelMultiHashMap<GenerateNodesSystem.OldNodeKey, GenerateNodesSystem.OldNodeValue> m_OldNodeMap;

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
        NativeArray<Game.Net.Node> nativeArray4 = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<EditorContainer> nativeArray5 = chunk.GetNativeArray<EditorContainer>(ref this.m_EditorContainerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Game.Net.OutsideConnection>(ref this.m_OutsideConnectionType);
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          GenerateNodesSystem.OldNodeKey key;
          // ISSUE: reference to a compiler-generated field
          key.m_Prefab = nativeArray6[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          key.m_SubPrefab = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          key.m_Original = nativeArray3[index].m_Original;
          // ISSUE: reference to a compiler-generated field
          key.m_Owner = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          key.m_OutsideConnection = flag;
          // ISSUE: variable of a compiler-generated type
          GenerateNodesSystem.OldNodeValue oldNodeValue;
          // ISSUE: reference to a compiler-generated field
          oldNodeValue.m_Entity = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          oldNodeValue.m_Position = nativeArray4[index].m_Position;
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
            Transform componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.TryGetComponent(owner.m_Owner, out componentData))
            {
              Transform inverseParentTransform = ObjectUtils.InverseTransform(componentData);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              oldNodeValue.m_Position = ObjectUtils.WorldToLocal(inverseParentTransform, oldNodeValue.m_Position);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_OldNodeMap.Add(key, oldNodeValue);
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
    private struct FillNodeMapJob : IJobChunk
    {
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
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<LocalConnect> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<Roundabout> m_RoundaboutData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_PrefabLocalConnectData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_RoadData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_Nodes;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      public NativeQueue<GenerateNodesSystem.UpdateData>.ParallelWriter m_NodeQueue;
      public NativeParallelHashMap<Entity, GenerateNodesSystem.DefinitionData>.ParallelWriter m_DefinitionMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray1 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCourse> nativeArray2 = chunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          CreationDefinition creationDefinition = nativeArray1[index];
          if (creationDefinition.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_DefinitionMap.TryAdd(creationDefinition.m_Original, new GenerateNodesSystem.DefinitionData(creationDefinition.m_Prefab, creationDefinition.m_SubPrefab, creationDefinition.m_Flags));
          }
        }
        if (nativeArray2.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<OwnerDefinition> nativeArray3 = chunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<LocalCurveCache> nativeArray4 = chunk.GetNativeArray<LocalCurveCache>(ref this.m_LocalCurveCacheType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Upgraded> nativeArray5 = chunk.GetNativeArray<Upgraded>(ref this.m_UpgradedType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          CreationDefinition definitionData = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(definitionData.m_Owner))
          {
            OwnerDefinition ownerData = new OwnerDefinition();
            NetCourse netCourse = nativeArray2[index];
            LocalCurveCache localCurveCache = new LocalCurveCache();
            Upgraded upgraded = new Upgraded();
            bool isStandalone = true;
            if (nativeArray3.Length != 0)
            {
              ownerData = nativeArray3[index];
              isStandalone = false;
            }
            if (nativeArray4.Length != 0)
              localCurveCache = nativeArray4[index];
            if (nativeArray5.Length != 0)
              upgraded = nativeArray5[index];
            int2 int2 = new PseudoRandomSeed((ushort) definitionData.m_RandomSeed).GetRandom((uint) PseudoRandomSeed.kEdgeNodes).NextInt2();
            if (((netCourse.m_StartPosition.m_Flags | netCourse.m_EndPosition.m_Flags) & CoursePosFlags.DontCreate) == (CoursePosFlags) 0)
            {
              if (netCourse.m_StartPosition.m_Position.Equals(netCourse.m_EndPosition.m_Position))
              {
                if (netCourse.m_StartPosition.m_Entity != Entity.Null && netCourse.m_EndPosition.m_Entity == Entity.Null)
                {
                  definitionData.m_RandomSeed = int2.x;
                  // ISSUE: reference to a compiler-generated method
                  this.AddNode(definitionData, ownerData, netCourse.m_StartPosition, upgraded, netCourse.m_FixedIndex, localCurveCache.m_Curve.a, nativeArray4.Length != 0, false);
                }
                else
                {
                  definitionData.m_RandomSeed = int2.y;
                  // ISSUE: reference to a compiler-generated method
                  this.AddNode(definitionData, ownerData, netCourse.m_EndPosition, upgraded, netCourse.m_FixedIndex, localCurveCache.m_Curve.d, nativeArray4.Length != 0, false);
                }
              }
              else
              {
                definitionData.m_Flags &= ~(CreationFlags.Select | CreationFlags.Upgrade);
                upgraded.m_Flags = new CompositionFlags();
                definitionData.m_RandomSeed = int2.x;
                // ISSUE: reference to a compiler-generated method
                this.AddNode(definitionData, ownerData, netCourse.m_StartPosition, upgraded, netCourse.m_FixedIndex, localCurveCache.m_Curve.a, nativeArray4.Length != 0, (definitionData.m_Flags & CreationFlags.Delete) == (CreationFlags) 0);
                definitionData.m_RandomSeed = int2.y;
                // ISSUE: reference to a compiler-generated method
                this.AddNode(definitionData, ownerData, netCourse.m_EndPosition, upgraded, netCourse.m_FixedIndex, localCurveCache.m_Curve.d, nativeArray4.Length != 0, (definitionData.m_Flags & CreationFlags.Delete) == (CreationFlags) 0);
              }
            }
            bool isPermanent = (definitionData.m_Flags & CreationFlags.Permanent) > (CreationFlags) 0;
            Entity original = Entity.Null;
            if (!isPermanent && (definitionData.m_Flags & CreationFlags.Delete) != (CreationFlags) 0)
              original = definitionData.m_Original;
            if (netCourse.m_StartPosition.m_Entity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddConnectedNodes(netCourse.m_StartPosition.m_Entity, original, isPermanent);
            }
            if (netCourse.m_EndPosition.m_Entity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddConnectedNodes(netCourse.m_EndPosition.m_Entity, original, isPermanent);
            }
            if (definitionData.m_Prefab != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddEdgesForLocalConnectOrAttachment(netCourse.m_StartPosition.m_Flags, netCourse.m_StartPosition.m_Entity, netCourse.m_StartPosition.m_Position, netCourse.m_StartPosition.m_Elevation, definitionData.m_Prefab, isPermanent, isStandalone);
              if (!netCourse.m_StartPosition.m_Position.Equals(netCourse.m_EndPosition.m_Position))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddEdgesForLocalConnectOrAttachment(netCourse.m_EndPosition.m_Flags, netCourse.m_EndPosition.m_Entity, netCourse.m_EndPosition.m_Position, netCourse.m_EndPosition.m_Elevation, definitionData.m_Prefab, isPermanent, isStandalone);
                // ISSUE: reference to a compiler-generated method
                this.AddNodesForLocalConnect(MathUtils.Cut(netCourse.m_Curve, new float2(netCourse.m_StartPosition.m_CourseDelta, netCourse.m_EndPosition.m_CourseDelta)), definitionData.m_Prefab, isPermanent, isStandalone);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(definitionData.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                Entity prefab = this.m_PrefabRefData[definitionData.m_Original].m_Prefab;
                // ISSUE: reference to a compiler-generated method
                this.AddEdgesForLocalConnectOrAttachment(netCourse.m_StartPosition.m_Flags, netCourse.m_StartPosition.m_Entity, netCourse.m_StartPosition.m_Position, netCourse.m_StartPosition.m_Elevation, prefab, isPermanent, isStandalone);
                if (!netCourse.m_StartPosition.m_Position.Equals(netCourse.m_EndPosition.m_Position))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddEdgesForLocalConnectOrAttachment(netCourse.m_EndPosition.m_Flags, netCourse.m_EndPosition.m_Entity, netCourse.m_EndPosition.m_Position, netCourse.m_EndPosition.m_Elevation, prefab, isPermanent, isStandalone);
                }
              }
            }
          }
        }
      }

      private void AddConnectedNodes(Entity original, Entity deleteEdge, bool isPermanent)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(original))
        {
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[original];
          // ISSUE: reference to a compiler-generated method
          this.AddNode(edge.m_Start, true, isPermanent);
          // ISSUE: reference to a compiler-generated method
          this.AddNode(edge.m_End, true, isPermanent);
          // ISSUE: reference to a compiler-generated method
          this.AddConnectedEdges(edge.m_Start, edge.m_End, isPermanent);
          // ISSUE: reference to a compiler-generated method
          this.AddConnectedEdges(edge.m_End, edge.m_Start, isPermanent);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedNode> node1 = this.m_Nodes[original];
          for (int index = 0; index < node1.Length; ++index)
          {
            Entity node2 = node1[index].m_Node;
            // ISSUE: reference to a compiler-generated method
            this.AddNode(node2, true, isPermanent);
            // ISSUE: reference to a compiler-generated method
            this.AddConnectedEdges(node2, isPermanent);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NodeData.HasComponent(original))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[original];
          if (deleteEdge != Entity.Null && edge1.Length != 3)
            deleteEdge = Entity.Null;
          for (int index1 = 0; index1 < edge1.Length; ++index1)
          {
            Entity edge2 = edge1[index1].m_Edge;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(edge2))
            {
              if (isPermanent)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), edge2, false, true));
              }
              // ISSUE: reference to a compiler-generated field
              Edge edge3 = this.m_EdgeData[edge2];
              if (edge3.m_Start != original && edge3.m_End != original)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddNode(edge3.m_Start, true, isPermanent);
                // ISSUE: reference to a compiler-generated method
                this.AddNode(edge3.m_End, true, isPermanent);
                // ISSUE: reference to a compiler-generated method
                this.AddConnectedEdges(edge3.m_Start, edge3.m_End, isPermanent);
                // ISSUE: reference to a compiler-generated method
                this.AddConnectedEdges(edge3.m_End, edge3.m_Start, isPermanent);
              }
              else
              {
                if (edge3.m_Start != original)
                {
                  if (deleteEdge != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddNode(edge3.m_Start, true, isPermanent);
                    // ISSUE: reference to a compiler-generated method
                    this.AddConnectedEdges(edge3.m_Start, original, isPermanent);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddNode(edge3.m_Start, false, isPermanent);
                  }
                }
                if (edge3.m_End != original)
                {
                  if (deleteEdge != Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddNode(edge3.m_End, true, isPermanent);
                    // ISSUE: reference to a compiler-generated method
                    this.AddConnectedEdges(edge3.m_End, original, isPermanent);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddNode(edge3.m_End, false, isPermanent);
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedNode> node3 = this.m_Nodes[edge2];
              for (int index2 = 0; index2 < node3.Length; ++index2)
              {
                Entity node4 = node3[index2].m_Node;
                // ISSUE: reference to a compiler-generated method
                this.AddNode(node4, true, isPermanent);
                // ISSUE: reference to a compiler-generated method
                this.AddConnectedEdges(node4, isPermanent);
              }
            }
          }
        }
      }

      private void AddConnectedEdges(Entity node, Entity otherNode, bool isPermanent)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[node];
        for (int index = 0; index < edge1.Length; ++index)
        {
          Entity edge2 = edge1[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(edge2))
          {
            if (isPermanent)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), edge2, false, true));
            }
            // ISSUE: reference to a compiler-generated field
            Edge edge3 = this.m_EdgeData[edge2];
            if (edge3.m_Start != node && edge3.m_Start != otherNode)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddNode(edge3.m_Start, false, isPermanent);
            }
            if (edge3.m_End != node && edge3.m_End != otherNode)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddNode(edge3.m_End, false, isPermanent);
            }
          }
        }
      }

      private void AddConnectedEdges(Entity node, bool isPermanent)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[node];
        for (int index = 0; index < edge1.Length; ++index)
        {
          Entity edge2 = edge1[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(edge2))
          {
            if (isPermanent)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), edge2, false, true));
            }
            // ISSUE: reference to a compiler-generated field
            Edge edge3 = this.m_EdgeData[edge2];
            if (edge3.m_End == node)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddNode(edge3.m_Start, false, isPermanent);
            }
            if (edge3.m_Start == node)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddNode(edge3.m_End, false, isPermanent);
            }
          }
        }
      }

      private void AddEdgesForLocalConnectOrAttachment(
        CoursePosFlags flags,
        Entity ignoreEntity,
        float3 position,
        float2 elevation,
        Entity prefab,
        bool isPermanent,
        bool isStandalone)
      {
        Bounds1 bounds1 = new Bounds1(float.MaxValue, float.MinValue);
        float2 x = (float2) 0.0f;
        Layer layer1 = Layer.None;
        Layer layer2 = Layer.None;
        Layer layer3 = Layer.None;
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefab];
        NetGeometryData netGeometryData = new NetGeometryData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetGeometryData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          netGeometryData = this.m_NetGeometryData[prefab];
        }
        if (math.all(elevation >= netGeometryData.m_ElevationLimit * 2f) || !math.all(elevation < 0.0f) && (netData.m_RequiredLayers & (Layer.PowerlineLow | Layer.PowerlineHigh)) != Layer.None)
        {
          float y = netGeometryData.m_DefaultWidth * 0.5f;
          float num = position.y - math.cmin(elevation);
          bounds1 |= new Bounds1(num - y, num + y);
          x = math.max(x, (float2) y);
          layer1 |= Layer.Road;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabLocalConnectData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          LocalConnectData localConnectData = this.m_PrefabLocalConnectData[prefab];
          if ((localConnectData.m_Flags & LocalConnectFlags.ExplicitNodes) == (LocalConnectFlags) 0 || (flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) != (CoursePosFlags) 0)
          {
            float2 y = (float2) (netGeometryData.m_DefaultWidth * 0.5f + localConnectData.m_SearchDistance);
            y.y += math.select(0.0f, 8f, !isStandalone && (double) localConnectData.m_SearchDistance != 0.0 && (netGeometryData.m_Flags & Game.Net.GeometryFlags.SubOwner) == (Game.Net.GeometryFlags) 0);
            bounds1 |= position.y + localConnectData.m_HeightRange;
            x = math.max(x, y);
            layer2 |= localConnectData.m_Layers;
            layer3 |= netData.m_ConnectLayers;
          }
        }
        if (layer1 == Layer.None && layer2 == Layer.None && layer3 == Layer.None)
          return;
        float num1 = math.cmax(x);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        GenerateNodesSystem.FillNodeMapJob.EdgeIterator iterator = new GenerateNodesSystem.FillNodeMapJob.EdgeIterator()
        {
          m_Bounds = new Bounds3(position - num1, position + num1)
          {
            y = bounds1
          },
          m_Position = position.xz,
          m_ConnectRadius = x,
          m_AttachLayers = layer1,
          m_ConnectLayers = layer2,
          m_LocalConnectLayers = layer3,
          m_IgnoreEntity = ignoreEntity,
          m_IsPermanent = isPermanent,
          m_EdgeData = this.m_EdgeData,
          m_NodeData = this.m_NodeData,
          m_CurveData = this.m_CurveData,
          m_DeletedData = this.m_DeletedData,
          m_OwnerData = this.m_OwnerData,
          m_RoundaboutData = this.m_RoundaboutData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_NetData = this.m_NetData,
          m_NetGeometryData = this.m_NetGeometryData,
          m_RoadData = this.m_RoadData,
          m_Edges = this.m_Edges,
          m_NodeQueue = this.m_NodeQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<GenerateNodesSystem.FillNodeMapJob.EdgeIterator>(ref iterator);
      }

      private void AddNodesForLocalConnect(
        Bezier4x3 curve,
        Entity prefab,
        bool isPermanent,
        bool isStandalone)
      {
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_NetData[prefab];
        NetGeometryData componentData;
        // ISSUE: reference to a compiler-generated field
        this.m_NetGeometryData.TryGetComponent(prefab, out componentData);
        float2 x = (float2) (componentData.m_DefaultWidth * 0.5f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_RoadData.HasComponent(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          x.y += math.select(0.0f, 8f, isStandalone && (this.m_RoadData[prefab].m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0);
        }
        float num = math.cmax(x) + 4f;
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
        GenerateNodesSystem.FillNodeMapJob.NodeIterator iterator = new GenerateNodesSystem.FillNodeMapJob.NodeIterator()
        {
          m_Bounds = MathUtils.Expand(MathUtils.Bounds(curve), new float3(num, 100f, num)),
          m_Curve = curve,
          m_ConnectRadius = x,
          m_ConnectLayers = netData.m_ConnectLayers,
          m_IsPermanent = isPermanent,
          m_EdgeData = this.m_EdgeData,
          m_NodeData = this.m_NodeData,
          m_OwnerData = this.m_OwnerData,
          m_LocalConnectData = this.m_LocalConnectData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabLocalConnectData = this.m_PrefabLocalConnectData,
          m_NetGeometryData = this.m_NetGeometryData,
          m_Edges = this.m_Edges,
          m_NodeQueue = this.m_NodeQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<GenerateNodesSystem.FillNodeMapJob.NodeIterator>(ref iterator);
      }

      private void AddNode(
        CreationDefinition definitionData,
        OwnerDefinition ownerData,
        CoursePos coursePos,
        Upgraded upgraded,
        int fixedIndex,
        float3 cachedPosition,
        bool hasCachedPosition,
        bool addEdge)
      {
        if (definitionData.m_Prefab == Entity.Null && coursePos.m_Entity == Entity.Null)
          return;
        Bounds1 curveBounds = new Bounds1(0.0f, 1f);
        Curve componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurveData.TryGetComponent(coursePos.m_Entity, out componentData1))
        {
          curveBounds = new Bounds1((float2) coursePos.m_SplitPosition);
          NetGeometryData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetGeometryData.TryGetComponent(definitionData.m_Prefab, out componentData2))
          {
            float length = componentData2.m_DefaultWidth * 0.5f;
            float t1;
            double num = (double) MathUtils.Distance(componentData1.m_Bezier.xz, coursePos.m_Position.xz, out t1);
            Bounds1 t2 = new Bounds1(0.0f, t1);
            Bounds1 t3 = new Bounds1(t1, 1f);
            MathUtils.ClampLengthInverse(componentData1.m_Bezier.xz, ref t2, length);
            MathUtils.ClampLength(componentData1.m_Bezier.xz, ref t3, length);
            curveBounds |= new Bounds1(t2.min, t3.max);
          }
        }
        else
        {
          coursePos.m_SplitPosition = 0.0f;
          Game.Net.Node componentData3;
          // ISSUE: reference to a compiler-generated field
          if ((definitionData.m_Flags & CreationFlags.Permanent) != (CreationFlags) 0 && this.m_NodeData.TryGetComponent(coursePos.m_Entity, out componentData3))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(componentData3, coursePos.m_Entity, false, true));
            return;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(definitionData, ownerData, coursePos, upgraded, fixedIndex, cachedPosition, curveBounds, hasCachedPosition, addEdge));
      }

      private void AddNode(Entity original, bool regenerate, bool isPermanent)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(this.m_NodeData[original], original, regenerate && !isPermanent, isPermanent));
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

      private struct EdgeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float2 m_Position;
        public float2 m_ConnectRadius;
        public Layer m_AttachLayers;
        public Layer m_ConnectLayers;
        public Layer m_LocalConnectLayers;
        public Entity m_IgnoreEntity;
        public bool m_IsPermanent;
        public ComponentLookup<Edge> m_EdgeData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Deleted> m_DeletedData;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Roundabout> m_RoundaboutData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NetData> m_NetData;
        public ComponentLookup<NetGeometryData> m_NetGeometryData;
        public ComponentLookup<RoadData> m_RoadData;
        public BufferLookup<ConnectedEdge> m_Edges;
        public NativeQueue<GenerateNodesSystem.UpdateData>.ParallelWriter m_NodeQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || entity == this.m_IgnoreEntity || !this.m_CurveData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_NetData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((this.m_AttachLayers & netData.m_ConnectLayers) == Layer.None && ((this.m_ConnectLayers & netData.m_ConnectLayers) == Layer.None || (this.m_LocalConnectLayers & netData.m_LocalConnectLayers) == Layer.None))
            return;
          // ISSUE: reference to a compiler-generated field
          Edge edge = this.m_EdgeData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (edge.m_Start == this.m_IgnoreEntity || edge.m_End == this.m_IgnoreEntity)
            return;
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[entity];
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
          RoadData roadData = new RoadData();
          // ISSUE: reference to a compiler-generated field
          if (this.m_RoadData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            roadData = this.m_RoadData[prefabRef.m_Prefab];
          }
          // ISSUE: reference to a compiler-generated field
          double num1 = (double) MathUtils.Distance(curve.m_Bezier.xz, this.m_Position, out float _);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num2 = math.select(this.m_ConnectRadius.x, this.m_ConnectRadius.y, !this.m_OwnerData.HasComponent(entity) && (roadData.m_Flags & Game.Prefabs.RoadFlags.EnableZoning) != 0);
          double num3 = (double) netGeometryData.m_DefaultWidth * 0.5 + (double) num2;
          bool flag = num1 <= num3;
          if (!flag)
          {
            Roundabout componentData1;
            Game.Net.Node componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoundaboutData.TryGetComponent(edge.m_Start, out componentData1) && this.m_NodeData.TryGetComponent(edge.m_Start, out componentData2) && (double) math.distance(componentData2.m_Position.xz, this.m_Position) <= (double) componentData1.m_Radius + (double) num2)
              flag = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_RoundaboutData.TryGetComponent(edge.m_End, out componentData1) && this.m_NodeData.TryGetComponent(edge.m_End, out componentData2) && (double) math.distance(componentData2.m_Position.xz, this.m_Position) <= (double) componentData1.m_Radius + (double) num2)
              flag = true;
          }
          if (!flag)
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsPermanent)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), entity, false, true));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), edge.m_Start, false, true));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), edge.m_End, false, true));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node1 = this.m_NodeData[edge.m_Start];
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node2 = this.m_NodeData[edge.m_End];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(node1, edge.m_Start, true, false));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(node2, edge.m_End, true, false));
            // ISSUE: reference to a compiler-generated method
            this.AddConnectedEdges(edge.m_Start, edge.m_End);
            // ISSUE: reference to a compiler-generated method
            this.AddConnectedEdges(edge.m_End, edge.m_Start);
          }
        }

        private void AddConnectedEdges(Entity node, Entity otherNode)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[node];
          for (int index = 0; index < edge1.Length; ++index)
          {
            Entity edge2 = edge1[index].m_Edge;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DeletedData.HasComponent(edge2))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge3 = this.m_EdgeData[edge2];
              if (edge3.m_Start != node && edge3.m_Start != otherNode)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(this.m_NodeData[edge3.m_Start], edge3.m_Start, false, false));
              }
              if (edge3.m_End != node && edge3.m_End != otherNode)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(this.m_NodeData[edge3.m_End], edge3.m_End, false, false));
              }
            }
          }
        }
      }

      private struct NodeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Bezier4x3 m_Curve;
        public float2 m_ConnectRadius;
        public Layer m_ConnectLayers;
        public bool m_IsPermanent;
        public ComponentLookup<Edge> m_EdgeData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<LocalConnect> m_LocalConnectData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<LocalConnectData> m_PrefabLocalConnectData;
        public ComponentLookup<NetGeometryData> m_NetGeometryData;
        public BufferLookup<ConnectedEdge> m_Edges;
        public NativeQueue<GenerateNodesSystem.UpdateData>.ParallelWriter m_NodeQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_NodeData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckNode(entity);
        }

        private void CheckNode(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_LocalConnectData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabLocalConnectData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          LocalConnectData localConnectData = this.m_PrefabLocalConnectData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if ((this.m_ConnectLayers & localConnectData.m_Layers) == Layer.None)
            return;
          // ISSUE: reference to a compiler-generated field
          NetGeometryData netGeometryData = this.m_NetGeometryData[prefabRef.m_Prefab];
          float num1 = math.max(0.0f, netGeometryData.m_DefaultWidth * 0.5f + localConnectData.m_SearchDistance);
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[entity];
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_Bounds, new Bounds3(node.m_Position - num1, node.m_Position + num1)
          {
            y = node.m_Position.y + localConnectData.m_HeightRange
          }))
            return;
          // ISSUE: reference to a compiler-generated field
          double num2 = (double) MathUtils.Distance(this.m_Curve.xz, node.m_Position.xz, out float _);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num3 = math.select(this.m_ConnectRadius.x, this.m_ConnectRadius.y, this.m_OwnerData.HasComponent(entity) && (double) localConnectData.m_SearchDistance != 0.0 && (netGeometryData.m_Flags & Game.Net.GeometryFlags.SubOwner) == (Game.Net.GeometryFlags) 0);
          double num4 = (double) num1 + (double) num3;
          if (num2 > num4)
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsPermanent)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(new Game.Net.Node(), entity, false, true));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(node, entity, true, false));
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[entity];
            for (int index = 0; index < edge1.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge2 = this.m_EdgeData[edge1[index].m_Edge];
              if (edge2.m_Start == entity)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(this.m_NodeData[edge2.m_End], edge2.m_End, false, false));
              }
              else if (edge2.m_End == entity)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_NodeQueue.Enqueue(new GenerateNodesSystem.UpdateData(this.m_NodeData[edge2.m_Start], edge2.m_Start, false, false));
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct CollectUpdatesJob : IJob
    {
      public NativeQueue<GenerateNodesSystem.UpdateData> m_UpdateQueue;
      public NativeList<GenerateNodesSystem.UpdateData> m_UpdateList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_UpdateQueue.Count;
        NativeParallelMultiHashMap<GenerateNodesSystem.NodeKey, int> parallelMultiHashMap = new NativeParallelMultiHashMap<GenerateNodesSystem.NodeKey, int>(count, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < count; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          GenerateNodesSystem.UpdateData data = this.m_UpdateQueue.Dequeue();
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GenerateNodesSystem.NodeKey key = new GenerateNodesSystem.NodeKey(data);
          int index2 = -1;
          int index3;
          NativeParallelMultiHashMapIterator<GenerateNodesSystem.NodeKey> it;
          if (parallelMultiHashMap.TryGetFirstValue(key, out index3, out it))
          {
            do
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              GenerateNodesSystem.UpdateData update = this.m_UpdateList[index3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (update.m_Valid && MathUtils.Intersect(data.m_CurveBounds, update.m_CurveBounds))
              {
                // ISSUE: reference to a compiler-generated field
                if (data.m_OnCourse)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (update.m_OnCourse)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    data.m_AddEdge |= update.m_AddEdge;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    data.m_FixedIndex = math.min(data.m_FixedIndex, update.m_FixedIndex);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    data.m_CreationFlags |= update.m_CreationFlags;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    data.m_UpgradeFlags |= update.m_UpgradeFlags;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    data.m_RandomSeed ^= update.m_RandomSeed;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  data.m_CurveBounds |= update.m_CurveBounds;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!data.m_Regenerate || update.m_OnCourse)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    update.m_CurveBounds |= data.m_CurveBounds;
                    data = update;
                  }
                }
                if (index2 == -1)
                {
                  index2 = index3;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  ref NativeList<GenerateNodesSystem.UpdateData> local = ref this.m_UpdateList;
                  int index4 = index3;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  GenerateNodesSystem.UpdateData updateData1 = new GenerateNodesSystem.UpdateData();
                  // ISSUE: variable of a compiler-generated type
                  GenerateNodesSystem.UpdateData updateData2 = updateData1;
                  local[index4] = updateData2;
                }
              }
            }
            while (parallelMultiHashMap.TryGetNextValue(out index3, ref it));
          }
          if (index2 == -1)
          {
            // ISSUE: reference to a compiler-generated field
            parallelMultiHashMap.Add(key, this.m_UpdateList.Length);
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateList.Add(in data);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateList[index2] = data;
          }
        }
      }
    }

    [BurstCompile]
    private struct CreateNodesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_PrefabLocalConnectData;
      [ReadOnly]
      public ComponentLookup<NetData> m_NetData;
      [ReadOnly]
      public ComponentLookup<RoadData> m_PrefabRoadData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<LocalConnect> m_LocalConnectData;
      [ReadOnly]
      public ComponentLookup<EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Standalone> m_StandaloneData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<NetCondition> m_ConditionData;
      [ReadOnly]
      public ComponentLookup<Road> m_RoadData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_Edges;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public Bounds3 m_TerrainBounds;
      [ReadOnly]
      public NativeList<GenerateNodesSystem.UpdateData> m_UpdateList;
      [ReadOnly]
      public NativeParallelHashMap<Entity, GenerateNodesSystem.DefinitionData> m_DefinitionMap;
      [ReadOnly]
      public NativeHashMap<OwnerDefinition, Entity> m_ReusedOwnerMap;
      public NativeParallelMultiHashMap<GenerateNodesSystem.OldNodeKey, GenerateNodesSystem.OldNodeValue> m_OldNodeMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_UpdateList.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.Execute(index);
        }
      }

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        GenerateNodesSystem.UpdateData update = this.m_UpdateList[index];
        // ISSUE: reference to a compiler-generated field
        if (!update.m_Valid)
          return;
        // ISSUE: reference to a compiler-generated field
        if (update.m_UpdateOnly)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(update.m_Original, new Updated());
        }
        else
        {
          Game.Net.Node node1 = new Game.Net.Node();
          // ISSUE: reference to a compiler-generated field
          node1.m_Position = update.m_Position;
          // ISSUE: reference to a compiler-generated field
          node1.m_Rotation = update.m_Rotation;
          Temp component1 = new Temp();
          // ISSUE: reference to a compiler-generated field
          component1.m_Original = update.m_Original;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (update.m_OwnerData.m_Prefab == Entity.Null && update.m_OnCourse)
            component1.m_Flags |= TempFlags.Essential;
          // ISSUE: reference to a compiler-generated field
          if ((update.m_Flags & (CoursePosFlags.IsLast | CoursePosFlags.IsParallel)) == CoursePosFlags.IsLast)
            component1.m_Flags |= TempFlags.IsLast;
          Upgraded component2 = new Upgraded();
          // ISSUE: reference to a compiler-generated field
          component2.m_Flags = update.m_UpgradeFlags;
          bool flag1 = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(update.m_Original))
          {
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (update.m_OnCourse && (update.m_Owner == Entity.Null && update.m_OwnerData.m_Prefab == Entity.Null || this.m_OwnerData.TryGetComponent(update.m_Original, out componentData) && !this.TryingToDelete(componentData.m_Owner)))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              node1 = this.m_NodeData[update.m_Original];
            }
            bool alreadyOrphan = false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            flag1 = !update.m_AddEdge && this.WillBeOrphan(update.m_Original, out alreadyOrphan);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (flag1 && this.CanDelete(update.m_Original) && (!alreadyOrphan || (update.m_CreationFlags & CreationFlags.Delete) != (CreationFlags) 0))
            {
              flag1 = alreadyOrphan;
              component1.m_Flags |= TempFlags.Delete;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (update.m_OnCourse)
              {
                // ISSUE: reference to a compiler-generated field
                if ((update.m_CreationFlags & CreationFlags.Upgrade) != (CreationFlags) 0)
                {
                  component1.m_Flags |= TempFlags.Upgrade;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((update.m_CreationFlags & CreationFlags.Select) != (CreationFlags) 0)
                    component1.m_Flags |= TempFlags.Select;
                  else
                    component1.m_Flags |= TempFlags.Regenerate;
                }
                // ISSUE: reference to a compiler-generated field
                if ((update.m_CreationFlags & CreationFlags.Parent) != (CreationFlags) 0)
                  component1.m_Flags |= TempFlags.Parent;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (update.m_Regenerate)
                  component1.m_Flags |= TempFlags.Regenerate;
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((update.m_CreationFlags & CreationFlags.Upgrade) == (CreationFlags) 0 && this.m_UpgradedData.HasComponent(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component2 = this.m_UpgradedData[update.m_Original];
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(update.m_Original))
            {
              component1.m_Flags |= TempFlags.Replace;
              // ISSUE: reference to a compiler-generated field
              component1.m_CurvePosition = update.m_CurvePosition;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              flag1 = !update.m_AddEdge;
              component1.m_Flags |= TempFlags.Create;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((update.m_CreationFlags & CreationFlags.Hidden) != (CreationFlags) 0 && ((update.m_CreationFlags & CreationFlags.Delete) == (CreationFlags) 0 || (component1.m_Flags & TempFlags.Delete) != (TempFlags) 0))
            component1.m_Flags |= TempFlags.Hidden;
          PrefabRef component3 = new PrefabRef();
          // ISSUE: reference to a compiler-generated field
          component3.m_Prefab = update.m_Prefab;
          bool flag2 = false;
          bool hasNativeEdges = false;
          // ISSUE: reference to a compiler-generated field
          if (update.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Hidden>(update.m_Original, new Hidden());
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(update.m_Original, new BatchesUpdated());
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_StandaloneData.HasComponent(update.m_Original))
            {
              flag2 = true;
              Owner componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (!flag1 && (component1.m_Flags & TempFlags.Delete) == (TempFlags) 0 && this.m_OwnerData.TryGetComponent(update.m_Original, out componentData) && (componentData.m_Owner == Entity.Null || this.TryingToDelete(componentData.m_Owner)))
                flag2 = false;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!flag2 && update.m_Prefab != Entity.Null && (update.m_Owner != Entity.Null || update.m_OwnerData.m_Prefab != Entity.Null) && (update.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) == (CoursePosFlags.IsFirst | CoursePosFlags.IsLast))
            {
              flag2 = true;
              // ISSUE: reference to a compiler-generated field
              component3.m_Prefab = update.m_Prefab;
            }
            else if (flag2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              component3 = this.m_PrefabRefData[update.m_Original];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              update.m_Lane = this.GetEditorLane(update.m_Original);
            }
            else
            {
              Entity lanePrefab;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.FindNodePrefab(update.m_Original, update.m_Prefab, update.m_Lane, out component3.m_Prefab, out lanePrefab, out hasNativeEdges);
              // ISSUE: reference to a compiler-generated field
              update.m_Lane = lanePrefab;
            }
          }
          else if (flag1)
            flag2 = true;
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_NetData[component3.m_Prefab];
          NetGeometryData netGeometryData = new NetGeometryData();
          bool flag3 = false;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetGeometryData.HasComponent(component3.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            netGeometryData = this.m_NetGeometryData[component3.m_Prefab];
            flag3 = true;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (((netGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0 || !flag3) && update.m_Prefab != Entity.Null && update.m_Original != Entity.Null && (!flag2 || this.m_StandaloneData.HasComponent(update.m_Original)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.HasComponent(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Net.Node node2 = this.m_NodeData[update.m_Original];
              node1.m_Position = node2.m_Position;
              node1.m_Rotation = node2.m_Rotation;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurveData.HasComponent(update.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[update.m_Original];
                // ISSUE: reference to a compiler-generated field
                node1.m_Position = MathUtils.Position(curve.m_Bezier, update.m_CurvePosition);
                // ISSUE: reference to a compiler-generated field
                float2 xz = MathUtils.Tangent(curve.m_Bezier, update.m_CurvePosition).xz;
                if (MathUtils.TryNormalize(ref xz))
                  node1.m_Rotation = quaternion.LookRotation(new float3(xz.x, 0.0f, xz.y), math.up());
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (update.m_OwnerData.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((component1.m_Flags & TempFlags.Delete) == (TempFlags) 0 && this.m_OwnerData.HasComponent(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity owner = this.m_OwnerData[update.m_Original].m_Owner;
              // ISSUE: reference to a compiler-generated method
              if (owner != Entity.Null && this.TryingToDelete(owner))
              {
                // ISSUE: reference to a compiler-generated field
                update.m_OwnerData = new OwnerDefinition();
                // ISSUE: reference to a compiler-generated field
                update.m_Owner = Entity.Null;
              }
            }
          }
          else
          {
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (update.m_Owner == Entity.Null && this.m_OwnerData.TryGetComponent(update.m_Original, out componentData) && componentData.m_Owner != Entity.Null && !this.TryingToDelete(componentData.m_Owner) && !this.TryingToDelete(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              update.m_Owner = componentData.m_Owner;
            }
          }
          // ISSUE: reference to a compiler-generated field
          bool isOutsideConnection = !MathUtils.Intersect(MathUtils.Expand(this.m_TerrainBounds.xz, (float2) -0.1f), node1.m_Position.xz);
          Entity oldEntity = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          bool flag4 = (update.m_CreationFlags & CreationFlags.Permanent) == (CreationFlags) 0 && this.TryGetOldEntity(node1, component3.m_Prefab, update.m_Lane, component1.m_Original, isOutsideConnection, ref update.m_OwnerData, ref update.m_Owner, out oldEntity);
          if (flag4)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Deleted>(oldEntity);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(oldEntity, new Updated());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetBuffer<ConnectedEdge>(oldEntity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            oldEntity = this.m_CommandBuffer.CreateEntity(netData.m_NodeArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(oldEntity, component3);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Game.Net.Node>(oldEntity, node1);
          if (flag1 & flag3)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Orphan>(oldEntity, new Orphan());
          }
          else if (flag4)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Orphan>(oldEntity);
          }
          if (flag3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PseudoRandomSeedData.HasComponent(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(oldEntity, this.m_PseudoRandomSeedData[update.m_Original]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(oldEntity, new PseudoRandomSeed((ushort) update.m_RandomSeed));
            }
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Standalone>(oldEntity, new Standalone());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (flag4 && this.m_StandaloneData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Standalone>(oldEntity);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (update.m_Lane != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<EditorContainer>(oldEntity, new EditorContainer()
            {
              m_Prefab = update.m_Lane
            });
          }
          component2.m_Flags &= CompositionFlags.nodeMask;
          if (component2.m_Flags != new CompositionFlags())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Upgraded>(oldEntity, component2);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (flag4 && this.m_UpgradedData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Upgraded>(oldEntity);
            }
          }
          bool flag5 = true;
          bool flag6 = true;
          bool flag7 = true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(update.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElevationData.HasComponent(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Net.Elevation component4 = this.m_ElevationData[update.m_Original];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (math.any(component4.m_Elevation != 0.0f) || update.m_ParentMesh >= 0 || this.m_OwnerData.HasComponent(update.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Game.Net.Elevation>(oldEntity, component4);
                flag5 = false;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (update.m_ParentMesh >= 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Game.Net.Elevation>(oldEntity, new Game.Net.Elevation());
                flag5 = false;
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NativeData.HasComponent(update.m_Original) && flag2 | hasNativeEdges)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Native>(oldEntity, new Native());
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (flag4 && this.m_NativeData.HasComponent(oldEntity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Native>(oldEntity);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_FixedData.HasComponent(update.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Fixed>(oldEntity, this.m_FixedData[update.m_Original]);
              flag6 = false;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRoadData.HasComponent(component3.m_Prefab) && this.m_PrefabData.IsComponentEnabled(component3.m_Prefab))
            {
              NetCondition componentData1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConditionData.TryGetComponent(update.m_Original, out componentData1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<NetCondition>(oldEntity, componentData1);
              }
              Road componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_RoadData.TryGetComponent(update.m_Original, out componentData2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Road>(oldEntity, componentData2);
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLocalConnectData.HasComponent(component3.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              LocalConnectData localConnectData = this.m_PrefabLocalConnectData[component3.m_Prefab];
              if ((localConnectData.m_Flags & LocalConnectFlags.ExplicitNodes) == (LocalConnectFlags) 0 | flag1)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<LocalConnect>(oldEntity, new LocalConnect());
                flag7 = false;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                if (this.m_LocalConnectData.HasComponent(update.m_Original) && ((localConnectData.m_Flags & LocalConnectFlags.KeepOpen) != (LocalConnectFlags) 0 || this.HasLocalConnections(update.m_Original)))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<LocalConnect>(oldEntity, new LocalConnect());
                  flag7 = false;
                }
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (math.any(update.m_Elevation != 0.0f) || update.m_ParentMesh >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Game.Net.Elevation>(oldEntity, new Game.Net.Elevation(update.m_Elevation));
              flag5 = false;
            }
            // ISSUE: reference to a compiler-generated field
            if ((update.m_Flags & CoursePosFlags.IsFixed) != (CoursePosFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Fixed>(oldEntity, new Fixed()
              {
                m_Index = update.m_FixedIndex
              });
              flag6 = false;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRoadData.HasComponent(component3.m_Prefab) && this.m_PrefabData.IsComponentEnabled(component3.m_Prefab))
            {
              NetCondition componentData3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConditionData.TryGetComponent(update.m_Original, out componentData3))
              {
                // ISSUE: reference to a compiler-generated field
                componentData3.m_Wear = (float2) math.lerp(componentData3.m_Wear.x, componentData3.m_Wear.y, update.m_CurvePosition);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<NetCondition>(oldEntity, componentData3);
              }
              Road componentData4;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_RoadData.TryGetComponent(update.m_Original, out componentData4))
              {
                componentData4.m_TrafficFlowDistance0 = (componentData4.m_TrafficFlowDistance0 + componentData4.m_TrafficFlowDistance1) * 0.5f;
                componentData4.m_TrafficFlowDuration0 = (componentData4.m_TrafficFlowDuration0 + componentData4.m_TrafficFlowDuration1) * 0.5f;
                componentData4.m_TrafficFlowDistance1 = componentData4.m_TrafficFlowDistance0;
                componentData4.m_TrafficFlowDuration1 = componentData4.m_TrafficFlowDuration0;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Road>(oldEntity, componentData4);
              }
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLocalConnectData.HasComponent(component3.m_Prefab) && ((this.m_PrefabLocalConnectData[component3.m_Prefab].m_Flags & LocalConnectFlags.ExplicitNodes) == (LocalConnectFlags) 0 || (update.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) != (CoursePosFlags) 0))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<LocalConnect>(oldEntity, new LocalConnect());
              flag7 = false;
            }
          }
          if (flag4)
          {
            // ISSUE: reference to a compiler-generated field
            if (flag5 && this.m_ElevationData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Game.Net.Elevation>(oldEntity);
            }
            // ISSUE: reference to a compiler-generated field
            if (flag6 && this.m_FixedData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Fixed>(oldEntity);
            }
            // ISSUE: reference to a compiler-generated field
            if (flag7 && this.m_LocalConnectData.HasComponent(oldEntity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<LocalConnect>(oldEntity);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (update.m_OwnerData.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Owner>(oldEntity, new Owner());
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(oldEntity, update.m_OwnerData);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (update.m_Owner != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Owner>(oldEntity, new Owner(update.m_Owner));
            }
          }
          if (isOutsideConnection)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Game.Net.OutsideConnection>(oldEntity, new Game.Net.OutsideConnection());
          }
          // ISSUE: reference to a compiler-generated field
          if ((update.m_CreationFlags & CreationFlags.Permanent) == (CreationFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(oldEntity, component1);
          }
          // ISSUE: reference to a compiler-generated field
          if (!update.m_HasCachedPosition)
            return;
          LocalTransformCache component5;
          // ISSUE: reference to a compiler-generated field
          component5.m_Position = update.m_CachedPosition;
          component5.m_Rotation = quaternion.identity;
          // ISSUE: reference to a compiler-generated field
          component5.m_ParentMesh = update.m_ParentMesh;
          component5.m_GroupIndex = 0;
          component5.m_Probability = 100;
          component5.m_PrefabSubIndex = -1;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<LocalTransformCache>(oldEntity, component5);
        }
      }

      private bool TryGetOldEntity(
        Game.Net.Node node,
        Entity prefab,
        Entity subPrefab,
        Entity original,
        bool isOutsideConnection,
        ref OwnerDefinition ownerDefinition,
        ref Entity owner,
        out Entity oldEntity)
      {
        Transform transform = new Transform();
        bool flag = false;
        Entity entity1;
        // ISSUE: reference to a compiler-generated field
        if (ownerDefinition.m_Prefab != Entity.Null && this.m_ReusedOwnerMap.TryGetValue(ownerDefinition, out entity1))
        {
          transform.m_Position = ownerDefinition.m_Position;
          transform.m_Rotation = ownerDefinition.m_Rotation;
          owner = entity1;
          ownerDefinition = new OwnerDefinition();
          flag = true;
        }
        else
        {
          Transform componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.TryGetComponent(owner, out componentData))
          {
            transform = componentData;
            flag = true;
          }
        }
        // ISSUE: variable of a compiler-generated type
        GenerateNodesSystem.OldNodeKey key;
        // ISSUE: reference to a compiler-generated field
        key.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        key.m_SubPrefab = subPrefab;
        // ISSUE: reference to a compiler-generated field
        key.m_Original = original;
        // ISSUE: reference to a compiler-generated field
        key.m_Owner = owner;
        // ISSUE: reference to a compiler-generated field
        key.m_OutsideConnection = isOutsideConnection;
        // ISSUE: variable of a compiler-generated type
        GenerateNodesSystem.OldNodeValue oldNodeValue;
        NativeParallelMultiHashMapIterator<GenerateNodesSystem.OldNodeKey> it1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OldNodeMap.TryGetFirstValue(key, out oldNodeValue, out it1))
        {
          float3 float3 = node.m_Position;
          float num1 = float.MaxValue;
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = oldNodeValue.m_Entity;
          NativeParallelMultiHashMapIterator<GenerateNodesSystem.OldNodeKey> it2 = it1;
          if (flag)
            float3 = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(transform), float3);
          // ISSUE: reference to a compiler-generated field
          do
          {
            // ISSUE: reference to a compiler-generated field
            float num2 = math.distancesq(float3, oldNodeValue.m_Position);
            if ((double) num2 < (double) num1)
            {
              num1 = num2;
              // ISSUE: reference to a compiler-generated field
              entity2 = oldNodeValue.m_Entity;
              it2 = it1;
            }
          }
          while (this.m_OldNodeMap.TryGetNextValue(out oldNodeValue, ref it1));
          oldEntity = entity2;
          // ISSUE: reference to a compiler-generated field
          this.m_OldNodeMap.Remove(it2);
          return true;
        }
        oldEntity = Entity.Null;
        return false;
      }

      private bool HasLocalConnections(Entity node)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Edges.HasBuffer(node))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[node];
          for (int index = 0; index < edge1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1[index].m_Edge];
            if (edge2.m_Start != node && edge2.m_End != node)
              return true;
          }
        }
        return false;
      }

      private void FindNodePrefab(
        Entity original,
        Entity newPrefab,
        Entity newLane,
        out Entity netPrefab,
        out Entity lanePrefab,
        out bool hasNativeEdges)
      {
        netPrefab = Entity.Null;
        lanePrefab = Entity.Null;
        float num = float.MinValue;
        hasNativeEdges = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(original))
        {
          // ISSUE: reference to a compiler-generated field
          hasNativeEdges = this.m_NativeData.HasComponent(original);
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[original];
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_NetData[prefabRef.m_Prefab];
          if ((double) netData.m_NodePriority > (double) num)
          {
            netPrefab = prefabRef.m_Prefab;
            // ISSUE: reference to a compiler-generated method
            lanePrefab = this.GetEditorLane(original);
            num = netData.m_NodePriority;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Edges.HasBuffer(original))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[original];
            for (int index = 0; index < edge1.Length; ++index)
            {
              Entity edge2 = edge1[index].m_Edge;
              // ISSUE: reference to a compiler-generated field
              Edge edge3 = this.m_EdgeData[edge2];
              if (!(edge3.m_Start != original) || !(edge3.m_End != original))
              {
                // ISSUE: variable of a compiler-generated type
                GenerateNodesSystem.DefinitionData definitionData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DefinitionMap.TryGetValue(edge2, out definitionData))
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((definitionData.m_Flags & CreationFlags.Delete) == (CreationFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (definitionData.m_Prefab != Entity.Null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      hasNativeEdges |= this.m_NativeData.HasComponent(edge2);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      NetData netData = this.m_NetData[definitionData.m_Prefab];
                      if ((double) netData.m_NodePriority > (double) num)
                      {
                        // ISSUE: reference to a compiler-generated field
                        netPrefab = definitionData.m_Prefab;
                        // ISSUE: reference to a compiler-generated field
                        lanePrefab = definitionData.m_Lane;
                        num = netData.m_NodePriority;
                        continue;
                      }
                      continue;
                    }
                  }
                  else
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                hasNativeEdges |= this.m_NativeData.HasComponent(edge2);
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef = this.m_PrefabRefData[edge2];
                // ISSUE: reference to a compiler-generated field
                NetData netData1 = this.m_NetData[prefabRef.m_Prefab];
                if ((double) netData1.m_NodePriority > (double) num)
                {
                  netPrefab = prefabRef.m_Prefab;
                  // ISSUE: reference to a compiler-generated method
                  lanePrefab = this.GetEditorLane(edge2);
                  num = netData1.m_NodePriority;
                }
              }
            }
          }
        }
        if (newPrefab != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_NetData[newPrefab];
          if ((double) netData.m_NodePriority > (double) num)
          {
            netPrefab = newPrefab;
            lanePrefab = newLane;
            float nodePriority = netData.m_NodePriority;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!(netPrefab == Entity.Null) || !this.m_PrefabRefData.HasComponent(original))
          return;
        // ISSUE: reference to a compiler-generated field
        netPrefab = this.m_PrefabRefData[original].m_Prefab;
        // ISSUE: reference to a compiler-generated method
        lanePrefab = this.GetEditorLane(original);
      }

      private Entity GetEditorLane(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_EditorContainerData.HasComponent(entity) ? this.m_EditorContainerData[entity].m_Prefab : Entity.Null;
      }

      private bool TryingToDelete(Entity entity)
      {
        // ISSUE: variable of a compiler-generated type
        GenerateNodesSystem.DefinitionData definitionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_DefinitionMap.TryGetValue(entity, out definitionData) && (definitionData.m_Flags & (CreationFlags.Delete | CreationFlags.Relocate)) > (CreationFlags) 0;
      }

      private bool WillBeOrphan(Entity node, out bool alreadyOrphan)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[node];
        alreadyOrphan = true;
        for (int index = 0; index < edge1.Length; ++index)
        {
          Entity edge2 = edge1[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge3 = this.m_EdgeData[edge2];
          if (edge3.m_Start == node || edge3.m_End == node)
          {
            alreadyOrphan = false;
            // ISSUE: variable of a compiler-generated type
            GenerateNodesSystem.DefinitionData definitionData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_DefinitionMap.TryGetValue(edge2, out definitionData) || (definitionData.m_Flags & CreationFlags.Delete) == (CreationFlags) 0)
              return false;
          }
        }
        return true;
      }

      private bool CanDelete(Entity node)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (this.TryingToDelete(node) || !this.m_OwnerData.HasComponent(node))
          return true;
        // ISSUE: reference to a compiler-generated field
        Entity owner1 = this.m_OwnerData[node].m_Owner;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!(owner1 != Entity.Null) || this.m_EditorMode || this.TryingToDelete(owner1))
          return true;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> edge1 = this.m_Edges[node];
        for (int index = 0; index < edge1.Length; ++index)
        {
          Entity edge2 = edge1[index].m_Edge;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (this.TryingToDelete(edge2) && this.m_OwnerData.HasComponent(edge2))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge3 = this.m_EdgeData[edge2];
            // ISSUE: reference to a compiler-generated field
            Entity owner2 = this.m_OwnerData[edge2].m_Owner;
            if ((edge3.m_Start == node || edge3.m_End == node) && owner2 == owner1)
              return true;
          }
        }
        return false;
      }
    }

    private struct TypeHandle
    {
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
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnect> __Game_Net_LocalConnect_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Roundabout> __Game_Net_Roundabout_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RoadData> __Game_Prefabs_RoadData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Standalone> __Game_Net_Standalone_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCondition> __Game_Net_NetCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Road> __Game_Net_Road_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
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
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LocalConnect_RO_ComponentLookup = state.GetComponentLookup<LocalConnect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Roundabout_RO_ComponentLookup = state.GetComponentLookup<Roundabout>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadData_RO_ComponentLookup = state.GetComponentLookup<RoadData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Standalone_RO_ComponentLookup = state.GetComponentLookup<Standalone>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentLookup = state.GetComponentLookup<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentLookup = state.GetComponentLookup<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
      }
    }
  }
}
