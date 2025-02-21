// Decompiled with JetBrains decompiler
// Type: Game.Tools.CourseSplitSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
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
using UnityEngine;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class CourseSplitSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private ToolReadyBarrier m_ToolReadyBarrier;
    private Game.Net.SearchSystem m_SearchSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_CourseQuery;
    private CourseSplitSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolReadyBarrier = this.World.GetOrCreateSystemManaged<ToolReadyBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CourseQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<NetCourse>(), ComponentType.ReadOnly<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CourseQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeHashMap<Entity, bool> nativeHashMap = new NativeHashMap<Entity, bool>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<CourseSplitSystem.Course> list1 = new NativeList<CourseSplitSystem.Course>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeQueue<CourseSplitSystem.Overlap> nativeQueue = new NativeQueue<CourseSplitSystem.Overlap>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<CourseSplitSystem.Overlap> list2 = new NativeList<CourseSplitSystem.Overlap>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelQueue<CourseSplitSystem.IntersectPos> nativeParallelQueue = new NativeParallelQueue<CourseSplitSystem.IntersectPos>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CourseSplitSystem.CheckCoursesJob jobData1 = new CourseSplitSystem.CheckCoursesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreationDefinitionType = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle,
        m_OwnerDefinitionType = this.__TypeHandle.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle,
        m_NetCourseType = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle,
        m_UpgradedType = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle,
        m_NetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_DeletedEntities = nativeHashMap,
        m_CourseList = list1
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CourseSplitSystem.FindOverlapsJob jobData2 = new CourseSplitSystem.FindOverlapsJob()
      {
        m_NetData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_SearchTree = this.m_SearchSystem.GetNetSearchTree(true, out dependencies),
        m_CourseList = list1,
        m_OverlapQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CourseSplitSystem.DequeueOverlapsJob jobData3 = new CourseSplitSystem.DequeueOverlapsJob()
      {
        m_OverlapQueue = nativeQueue,
        m_OverlapList = list2
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      CourseSplitSystem.CheckCourseIntersectionsJob jobData4 = new CourseSplitSystem.CheckCourseIntersectionsJob()
      {
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_CourseList = list1,
        m_OverlapList = list2,
        m_DeletedEntities = nativeHashMap,
        m_Results = nativeParallelQueue.AsWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FixedNetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Tools_LocalCurveCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CourseSplitSystem.CheckCourseIntersectionResultsJob jobData5 = new CourseSplitSystem.CheckCourseIntersectionResultsJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_FixedData = this.__TypeHandle.__Game_Net_Fixed_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_LocalCurveCacheData = this.__TypeHandle.__Game_Tools_LocalCurveCache_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabPlaceableData = this.__TypeHandle.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_PrefabFixedNetElements = this.__TypeHandle.__Game_Prefabs_FixedNetElement_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CourseList = list1,
        m_DeletedEntities = nativeHashMap,
        m_IntersectionQueue = nativeParallelQueue.AsReader(),
        m_CommandBuffer = this.m_ToolReadyBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData1.Schedule<CourseSplitSystem.CheckCoursesJob>(this.m_CourseQuery, this.Dependency);
      JobHandle jobHandle1 = jobData2.Schedule<CourseSplitSystem.FindOverlapsJob, CourseSplitSystem.Course>(list1, 1, JobHandle.CombineDependencies(job0, dependencies));
      JobHandle jobHandle2 = jobData3.Schedule<CourseSplitSystem.DequeueOverlapsJob>(jobHandle1);
      JobHandle jobHandle3 = jobData4.Schedule<CourseSplitSystem.CheckCourseIntersectionsJob, CourseSplitSystem.Overlap>(list2, 1, jobHandle2);
      NativeList<CourseSplitSystem.Course> list3 = list1;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle3, deps);
      JobHandle jobHandle4 = jobData5.Schedule<CourseSplitSystem.CheckCourseIntersectionResultsJob, CourseSplitSystem.Course>(list3, 1, dependsOn);
      nativeHashMap.Dispose(jobHandle4);
      list1.Dispose(jobHandle4);
      nativeQueue.Dispose(jobHandle2);
      list2.Dispose(jobHandle3);
      nativeParallelQueue.Dispose(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle4);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolReadyBarrier.AddJobHandleForProducer(jobHandle4);
      this.Dependency = jobHandle4;
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
    public CourseSplitSystem()
    {
    }

    private struct IntersectPos : IComparable<CourseSplitSystem.IntersectPos>
    {
      public CoursePos m_CoursePos;
      public Bounds1 m_CourseIntersection;
      public Bounds1 m_IntersectionHeightMin;
      public Bounds1 m_IntersectionHeightMax;
      public Bounds1 m_EdgeIntersection;
      public Bounds1 m_EdgeHeightRangeMin;
      public Bounds1 m_EdgeHeightRangeMax;
      public Bounds1 m_CanMove;
      public float m_Priority;
      public int m_CourseIndex;
      public bool m_IsNode;
      public bool m_IsOptional;
      public bool m_IsStartEnd;
      public bool m_IsTunnel;

      public int CompareTo(CourseSplitSystem.IntersectPos other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) math.sign(this.m_CourseIntersection.min - other.m_CourseIntersection.min);
      }

      public override int GetHashCode() => this.m_CourseIndex;
    }

    private struct Course
    {
      public CreationDefinition m_CreationDefinition;
      public OwnerDefinition m_OwnerDefinition;
      public NetCourse m_CourseData;
      public Upgraded m_UpgradedData;
      public Entity m_CourseEntity;
    }

    private struct Overlap
    {
      public Entity m_OverlapEntity;
      public int m_CourseIndex;
    }

    [BurstCompile]
    private struct CheckCoursesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> m_CreationDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> m_OwnerDefinitionType;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> m_NetCourseType;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> m_UpgradedType;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetGeometryData;
      public NativeHashMap<Entity, bool> m_DeletedEntities;
      public NativeList<CourseSplitSystem.Course> m_CourseList;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CreationDefinition> nativeArray2 = chunk.GetNativeArray<CreationDefinition>(ref this.m_CreationDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<OwnerDefinition> nativeArray3 = chunk.GetNativeArray<OwnerDefinition>(ref this.m_OwnerDefinitionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCourse> nativeArray4 = chunk.GetNativeArray<NetCourse>(ref this.m_NetCourseType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Upgraded> nativeArray5 = chunk.GetNativeArray<Upgraded>(ref this.m_UpgradedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.Course course = new CourseSplitSystem.Course()
          {
            m_CourseEntity = nativeArray1[index],
            m_CreationDefinition = nativeArray2[index],
            m_CourseData = nativeArray4[index]
          };
          // ISSUE: reference to a compiler-generated field
          if (course.m_CreationDefinition.m_Original != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_DeletedEntities.Add(course.m_CreationDefinition.m_Original, (course.m_CreationDefinition.m_Flags & CreationFlags.Delete) > (CreationFlags) 0);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetGeometryData.HasComponent(course.m_CreationDefinition.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryGet<Upgraded>(nativeArray5, index, out course.m_UpgradedData);
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryGet<OwnerDefinition>(nativeArray3, index, out course.m_OwnerDefinition);
              // ISSUE: reference to a compiler-generated field
              this.m_CourseList.Add(in course);
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
    private struct FindOverlapsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_NetData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      [ReadOnly]
      public NativeList<CourseSplitSystem.Course> m_CourseList;
      public NativeQueue<CourseSplitSystem.Overlap>.ParallelWriter m_OverlapQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.Course course = this.m_CourseList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((course.m_CourseData.m_StartPosition.m_Flags & course.m_CourseData.m_EndPosition.m_Flags & CoursePosFlags.DisableMerge) != (CoursePosFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float range = this.m_NetData[course.m_CreationDefinition.m_Prefab].m_DefaultWidth * 0.5f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.FindOverlapsJob.OverlapIterator iterator = new CourseSplitSystem.FindOverlapsJob.OverlapIterator()
        {
          m_Range = range,
          m_SizeLimit = range * 4f,
          m_CourseIndex = index,
          m_OverlapQueue = this.m_OverlapQueue,
          m_DeletedData = this.m_DeletedData
        };
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.FindOverlapsJob.OverlapIteratorSubData subData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MathUtils.Divide(course.m_CourseData.m_Curve.xz, out subData.m_Curve1, out subData.m_Curve2, 0.5f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        subData.m_Bounds1 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve1), (float2) range);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        subData.m_Bounds2 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve2), (float2) range);
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<CourseSplitSystem.FindOverlapsJob.OverlapIterator, CourseSplitSystem.FindOverlapsJob.OverlapIteratorSubData>(ref iterator, subData);
      }

      private struct OverlapIteratorSubData
      {
        public Bounds2 m_Bounds1;
        public Bounds2 m_Bounds2;
        public Bezier4x2 m_Curve1;
        public Bezier4x2 m_Curve2;
      }

      private struct OverlapIterator : 
        INativeQuadTreeIteratorWithSubData<Entity, QuadTreeBoundsXZ, CourseSplitSystem.FindOverlapsJob.OverlapIteratorSubData>,
        IUnsafeQuadTreeIteratorWithSubData<Entity, QuadTreeBoundsXZ, CourseSplitSystem.FindOverlapsJob.OverlapIteratorSubData>
      {
        public float m_Range;
        public float m_SizeLimit;
        public int m_CourseIndex;
        public NativeQueue<CourseSplitSystem.Overlap>.ParallelWriter m_OverlapQueue;
        public ComponentLookup<Deleted> m_DeletedData;

        public bool Intersect(
          QuadTreeBoundsXZ bounds,
          ref CourseSplitSystem.FindOverlapsJob.OverlapIteratorSubData subData)
        {
          bool2 x;
          // ISSUE: reference to a compiler-generated field
          x.x = MathUtils.Intersect(bounds.m_Bounds.xz, subData.m_Bounds1);
          // ISSUE: reference to a compiler-generated field
          x.y = MathUtils.Intersect(bounds.m_Bounds.xz, subData.m_Bounds2);
          if (!math.any(x))
            return false;
          if (math.all(x))
            return true;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (math.any(MathUtils.Size(subData.m_Bounds1) > this.m_SizeLimit))
          {
            if (x.x)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(subData.m_Curve1, out subData.m_Curve1, out subData.m_Curve2, 0.5f);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MathUtils.Divide(subData.m_Curve2, out subData.m_Curve1, out subData.m_Curve2, 0.5f);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            subData.m_Bounds1 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve1), (float2) this.m_Range);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            subData.m_Bounds2 = MathUtils.Expand(MathUtils.Bounds(subData.m_Curve2), (float2) this.m_Range);
            // ISSUE: reference to a compiler-generated field
            x.x = MathUtils.Intersect(bounds.m_Bounds.xz, subData.m_Bounds1);
            // ISSUE: reference to a compiler-generated field
            x.y = MathUtils.Intersect(bounds.m_Bounds.xz, subData.m_Bounds2);
            if (!math.any(x))
              return false;
            if (math.all(x))
              return true;
          }
          return true;
        }

        public void Iterate(
          QuadTreeBoundsXZ bounds,
          CourseSplitSystem.FindOverlapsJob.OverlapIteratorSubData subData,
          Entity overlapEntity)
        {
          bool2 x;
          // ISSUE: reference to a compiler-generated field
          x.x = MathUtils.Intersect(bounds.m_Bounds.xz, subData.m_Bounds1);
          // ISSUE: reference to a compiler-generated field
          x.y = MathUtils.Intersect(bounds.m_Bounds.xz, subData.m_Bounds2);
          // ISSUE: reference to a compiler-generated field
          if (!math.any(x) || this.m_DeletedData.HasComponent(overlapEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_OverlapQueue.Enqueue(new CourseSplitSystem.Overlap()
          {
            m_CourseIndex = this.m_CourseIndex,
            m_OverlapEntity = overlapEntity
          });
        }
      }
    }

    [BurstCompile]
    private struct DequeueOverlapsJob : IJob
    {
      public NativeQueue<CourseSplitSystem.Overlap> m_OverlapQueue;
      public NativeList<CourseSplitSystem.Overlap> m_OverlapList;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.Overlap overlap;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OverlapQueue.TryDequeue(out overlap))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OverlapList.Add(in overlap);
        }
      }
    }

    [BurstCompile]
    private struct CheckCourseIntersectionsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public NativeList<CourseSplitSystem.Course> m_CourseList;
      [ReadOnly]
      public NativeList<CourseSplitSystem.Overlap> m_OverlapList;
      [ReadOnly]
      public NativeHashMap<Entity, bool> m_DeletedEntities;
      public NativeParallelQueue<CourseSplitSystem.IntersectPos>.Writer m_Results;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.Overlap overlap = this.m_OverlapList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeGeometryData.HasComponent(overlap.m_OverlapEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Game.Net.Edge edgeData = this.m_EdgeData[overlap.m_OverlapEntity];
        bool3 bool3 = (bool3) true;
        bool flag;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeletedEntities.TryGetValue(overlap.m_OverlapEntity, out flag) & flag)
        {
          // ISSUE: reference to a compiler-generated method
          bool3.x = !this.WillBeOrphan(edgeData.m_Start);
          bool3.y = false;
          // ISSUE: reference to a compiler-generated method
          bool3.z = !this.WillBeOrphan(edgeData.m_End);
          if (!math.any(bool3.xz))
            return;
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity = overlap.m_OverlapEntity;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out componentData1) && !this.m_BuildingData.HasComponent(entity))
        {
          entity = componentData1.m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_DeletedData.HasComponent(entity))
            return;
        }
        ObjectGeometryData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData2) && (componentData2.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden)) == (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.DeleteOverridden))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[overlap.m_OverlapEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Composition composition = this.m_CompositionData[overlap.m_OverlapEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry geometry1 = this.m_EdgeGeometryData[overlap.m_OverlapEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeNodeGeometry geometry2 = this.m_StartNodeGeometryData[overlap.m_OverlapEntity].m_Geometry;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EdgeNodeGeometry geometry3 = this.m_EndNodeGeometryData[overlap.m_OverlapEntity].m_Geometry;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.Course course = this.m_CourseList[overlap.m_CourseIndex];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData prefabGeometryData = this.m_PrefabGeometryData[course.m_CreationDefinition.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int iterations = math.select(math.max(4, (int) math.ceil(math.log(course.m_CourseData.m_Length * 16f / prefabGeometryData.m_EdgeLengthRange.max) * 1.442695f)), 0, (double) course.m_CourseData.m_Length < (double) prefabGeometryData.m_DefaultWidth * 0.0099999997764825821);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 courseOffset = new float2(course.m_CourseData.m_StartPosition.m_CourseDelta, course.m_CourseData.m_EndPosition.m_CourseDelta);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos currentIntersectPos = new CourseSplitSystem.IntersectPos();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos intersectPos1 = new CourseSplitSystem.IntersectPos();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos intersectPos2 = new CourseSplitSystem.IntersectPos();
        // ISSUE: reference to a compiler-generated field
        currentIntersectPos.m_Priority = -1f;
        // ISSUE: reference to a compiler-generated field
        intersectPos1.m_Priority = -1f;
        // ISSUE: reference to a compiler-generated field
        intersectPos2.m_Priority = -1f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[overlap.m_OverlapEntity];
        // ISSUE: reference to a compiler-generated field
        NetGeometryData prefabGeometryData2 = this.m_PrefabGeometryData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData prefabCompositionData2_1 = this.m_PrefabCompositionData[composition.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData prefabCompositionData2_2 = this.m_PrefabCompositionData[composition.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData prefabCompositionData2_3 = this.m_PrefabCompositionData[composition.m_EndNode];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((prefabGeometryData.m_MergeLayers & prefabGeometryData2.m_MergeLayers) == Layer.None && !NetUtils.CanConnect(this.m_PrefabNetData[course.m_CreationDefinition.m_Prefab], this.m_PrefabNetData[prefabRef.m_Prefab]) && ((prefabGeometryData.m_Flags | prefabGeometryData2.m_Flags) & Game.Net.GeometryFlags.Marker) != (Game.Net.GeometryFlags) 0)
          return;
        if (prefabGeometryData2.m_MergeLayers == Layer.None || !bool3.y)
        {
          if (bool3.x)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeGeometry(course.m_CourseData, overlap.m_CourseIndex, ref intersectPos1, prefabGeometryData, prefabCompositionData2_2, overlap.m_OverlapEntity, edgeData.m_Start, curve.m_Bezier.a, courseOffset, 0.0f);
          }
          if (bool3.z)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeGeometry(course.m_CourseData, overlap.m_CourseIndex, ref intersectPos2, prefabGeometryData, prefabCompositionData2_3, overlap.m_OverlapEntity, edgeData.m_End, curve.m_Bezier.d, courseOffset, 1f);
          }
        }
        else
        {
          if (bool3.x)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeGeometry(course.m_CourseData, overlap.m_CourseIndex, ref intersectPos1, prefabGeometryData, prefabCompositionData2_2, overlap.m_OverlapEntity, edgeData.m_Start, geometry2, courseOffset, 0.0f, iterations);
          }
          if (bool3.z)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeGeometry(course.m_CourseData, overlap.m_CourseIndex, ref intersectPos2, prefabGeometryData, prefabCompositionData2_3, overlap.m_OverlapEntity, edgeData.m_End, geometry3, courseOffset, 1f, iterations);
          }
        }
        if (bool3.y)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckEdgeGeometry(course.m_CourseData, overlap.m_CourseIndex, ref intersectPos1, ref intersectPos2, ref currentIntersectPos, prefabGeometryData, prefabGeometryData2, prefabCompositionData2_1, overlap.m_OverlapEntity, edgeData, geometry1, curve.m_Bezier, courseOffset, iterations);
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) intersectPos1.m_Priority != -1.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results.Enqueue(intersectPos1);
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) intersectPos2.m_Priority != -1.0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results.Enqueue(intersectPos2);
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) currentIntersectPos.m_Priority == -1.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Results.Enqueue(currentIntersectPos);
      }

      private bool WillBeOrphan(Entity node)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge2 = this.m_EdgeData[edge1];
          bool flag;
          // ISSUE: reference to a compiler-generated field
          if ((edge2.m_Start == node || edge2.m_End == node) && (!this.m_DeletedEntities.TryGetValue(edge1, out flag) || !flag))
            return false;
        }
        return true;
      }

      private void CheckEdgeGeometry(
        NetCourse courseData,
        int courseIndex,
        ref CourseSplitSystem.IntersectPos startIntersectPos,
        ref CourseSplitSystem.IntersectPos endIntersectPos,
        ref CourseSplitSystem.IntersectPos currentIntersectPos,
        NetGeometryData prefabGeometryData,
        NetGeometryData prefabGeometryData2,
        NetCompositionData prefabCompositionData2,
        Entity edge,
        Game.Net.Edge edgeData,
        EdgeGeometry geometry,
        Bezier4x3 curve,
        float2 courseOffset,
        int iterations)
      {
        Bounds2 bounds1 = MathUtils.Bounds(MathUtils.Cut(courseData.m_Curve.xz, courseOffset));
        bounds1.min -= prefabGeometryData.m_DefaultWidth * 0.5f;
        bounds1.max += prefabGeometryData.m_DefaultWidth * 0.5f;
        if (!MathUtils.Intersect(bounds1, geometry.m_Bounds.xz))
          return;
        if (iterations <= 0)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos lastIntersectPos = new CourseSplitSystem.IntersectPos();
          // ISSUE: reference to a compiler-generated field
          lastIntersectPos.m_Priority = -1f;
          // ISSUE: reference to a compiler-generated method
          this.CheckEdgeSegment(courseData, courseIndex, ref startIntersectPos, ref endIntersectPos, ref currentIntersectPos, ref lastIntersectPos, prefabGeometryData, prefabGeometryData2, prefabCompositionData2, edge, edgeData, geometry.m_Start, curve, courseOffset, new float2(0.0f, 0.5f));
          // ISSUE: reference to a compiler-generated method
          this.CheckEdgeSegment(courseData, courseIndex, ref startIntersectPos, ref endIntersectPos, ref currentIntersectPos, ref lastIntersectPos, prefabGeometryData, prefabGeometryData2, prefabCompositionData2, edge, edgeData, geometry.m_End, curve, courseOffset, new float2(0.5f, 1f));
          // ISSUE: reference to a compiler-generated field
          if ((double) lastIntersectPos.m_Priority == -1.0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.Add(ref currentIntersectPos, lastIntersectPos);
        }
        else
        {
          float3 float3 = new float3(courseOffset.x, math.lerp(courseOffset.x, courseOffset.y, 0.5f), courseOffset.y);
          // ISSUE: reference to a compiler-generated method
          this.CheckEdgeGeometry(courseData, courseIndex, ref startIntersectPos, ref endIntersectPos, ref currentIntersectPos, prefabGeometryData, prefabGeometryData2, prefabCompositionData2, edge, edgeData, geometry, curve, float3.xy, iterations - 1);
          // ISSUE: reference to a compiler-generated method
          this.CheckEdgeGeometry(courseData, courseIndex, ref startIntersectPos, ref endIntersectPos, ref currentIntersectPos, prefabGeometryData, prefabGeometryData2, prefabCompositionData2, edge, edgeData, geometry, curve, float3.yz, iterations - 1);
        }
      }

      private void CheckNodeGeometry(
        NetCourse courseData,
        int courseIndex,
        ref CourseSplitSystem.IntersectPos result,
        NetGeometryData prefabGeometryData,
        NetCompositionData prefabCompositionData2,
        Entity edge,
        Entity node,
        EdgeNodeGeometry geometry,
        float2 courseOffset,
        float edgeOffset,
        int iterations)
      {
        Bounds2 bounds1 = MathUtils.Bounds(MathUtils.Cut(courseData.m_Curve.xz, courseOffset));
        bounds1.min -= prefabGeometryData.m_DefaultWidth * 0.5f;
        bounds1.max += prefabGeometryData.m_DefaultWidth * 0.5f;
        if (!MathUtils.Intersect(bounds1, geometry.m_Bounds.xz))
          return;
        if (iterations <= 0)
        {
          if ((double) geometry.m_MiddleRadius > 0.0)
          {
            Game.Net.Segment right1 = geometry.m_Right;
            Game.Net.Segment right2 = geometry.m_Right;
            right1.m_Right = MathUtils.Lerp(geometry.m_Right.m_Left, geometry.m_Right.m_Right, 0.5f);
            right1.m_Right.d = geometry.m_Middle.d;
            right2.m_Left = right1.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, geometry.m_Left, courseOffset, edgeOffset, 0.5f);
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, right1, courseOffset, edgeOffset, 1f);
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, right2, courseOffset, edgeOffset, 0.0f);
          }
          else
          {
            Game.Net.Segment left = geometry.m_Left;
            Game.Net.Segment right = geometry.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, left, courseOffset, edgeOffset, 1f);
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, right, courseOffset, edgeOffset, 0.0f);
            left.m_Right = geometry.m_Middle;
            right.m_Left = geometry.m_Middle;
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, left, courseOffset, edgeOffset, 1f);
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeSegment(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, right, courseOffset, edgeOffset, 0.0f);
          }
        }
        else
        {
          float3 float3 = new float3(courseOffset.x, math.lerp(courseOffset.x, courseOffset.y, 0.5f), courseOffset.y);
          // ISSUE: reference to a compiler-generated method
          this.CheckNodeGeometry(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, geometry, float3.xy, edgeOffset, iterations - 1);
          // ISSUE: reference to a compiler-generated method
          this.CheckNodeGeometry(courseData, courseIndex, ref result, prefabGeometryData, prefabCompositionData2, edge, node, geometry, float3.yz, edgeOffset, iterations - 1);
        }
      }

      private void CheckNodeGeometry(
        NetCourse courseData,
        int courseIndex,
        ref CourseSplitSystem.IntersectPos result,
        NetGeometryData prefabGeometryData,
        NetCompositionData prefabCompositionData2,
        Entity edge,
        Entity node,
        float3 nodePos,
        float2 courseOffset,
        float edgeOffset)
      {
        Bezier4x2 curve = MathUtils.Cut(courseData.m_Curve.xz, courseOffset);
        float range = prefabGeometryData.m_DefaultWidth * 0.5f;
        Bounds2 bounds1_1 = MathUtils.Expand(MathUtils.Bounds(curve), (float2) range);
        Circle2 circle = new Circle2(prefabCompositionData2.m_Width * 0.5f, nodePos.xz);
        Bounds2 bounds2 = MathUtils.Bounds(circle);
        if (!MathUtils.Intersect(bounds1_1, bounds2))
          return;
        float t1;
        float num1 = MathUtils.Distance(curve, nodePos.xz, out t1);
        if ((double) num1 > (double) range + (double) circle.radius)
          return;
        float t2 = math.lerp(courseOffset.x, courseOffset.y, t1);
        float3 float3 = MathUtils.Position(courseData.m_Curve, t2);
        Bounds1 bounds1_2;
        bounds1_2.min = math.lerp(courseOffset.x, courseOffset.y, t1 - 0.01f);
        bounds1_2.max = math.lerp(courseOffset.x, courseOffset.y, t1 + 0.01f);
        float num2 = math.max(0.0f, (num1 - range) / circle.radius);
        float length = math.sqrt((float) (1.0 - (double) num2 * (double) num2)) * circle.radius;
        if ((double) bounds1_2.max < 1.0)
        {
          Bounds1 t3 = new Bounds1(bounds1_2.max, 1f);
          MathUtils.ClampLength(courseData.m_Curve, ref t3, length);
          bounds1_2.max = t3.max;
        }
        if ((double) bounds1_2.min > 0.0)
        {
          Bounds1 t4 = new Bounds1(0.0f, bounds1_2.min);
          MathUtils.ClampLengthInverse(courseData.m_Curve, ref t4, length);
          bounds1_2.min = t4.min;
        }
        int num3 = -1;
        LocalTransformCache componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LocalTransformCacheData.TryGetComponent(node, out componentData))
          num3 = componentData.m_ParentMesh;
        // ISSUE: object of a compiler-generated type is created
        result = new CourseSplitSystem.IntersectPos();
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_Entity = node;
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_Position = float3;
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, t2));
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_CourseDelta = t2;
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_SplitPosition = edgeOffset;
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_Flags = courseData.m_StartPosition.m_Flags & (CoursePosFlags.IsParallel | CoursePosFlags.IsRight | CoursePosFlags.IsLeft | CoursePosFlags.IsGrid);
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_Flags |= CoursePosFlags.FreeHeight;
        // ISSUE: reference to a compiler-generated field
        result.m_CoursePos.m_ParentMesh = num3;
        // ISSUE: reference to a compiler-generated field
        result.m_CourseIntersection = bounds1_2;
        // ISSUE: reference to a compiler-generated field
        result.m_IntersectionHeightMin = new Bounds1((float2) float3.y);
        // ISSUE: reference to a compiler-generated field
        result.m_IntersectionHeightMax = new Bounds1((float2) float3.y);
        // ISSUE: reference to a compiler-generated field
        result.m_EdgeIntersection = new Bounds1(edgeOffset, edgeOffset);
        // ISSUE: reference to a compiler-generated field
        result.m_EdgeHeightRangeMin = nodePos.y + prefabCompositionData2.m_HeightRange;
        // ISSUE: reference to a compiler-generated field
        result.m_EdgeHeightRangeMax = nodePos.y + prefabCompositionData2.m_HeightRange;
        // ISSUE: reference to a compiler-generated field
        result.m_Priority = num1;
        // ISSUE: reference to a compiler-generated field
        result.m_CourseIndex = courseIndex;
        // ISSUE: reference to a compiler-generated field
        result.m_IsNode = true;
        // ISSUE: reference to a compiler-generated field
        result.m_IsTunnel = (prefabCompositionData2.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0 && ((prefabCompositionData2.m_Flags.m_Left | prefabCompositionData2.m_Flags.m_Right) & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) == (CompositionFlags.Side) 0;
      }

      private void CheckEdgeSegment(
        NetCourse courseData,
        int courseIndex,
        ref CourseSplitSystem.IntersectPos startIntersectPos,
        ref CourseSplitSystem.IntersectPos endIntersectPos,
        ref CourseSplitSystem.IntersectPos currentIntersectPos,
        ref CourseSplitSystem.IntersectPos lastIntersectPos,
        NetGeometryData prefabGeometryData,
        NetGeometryData prefabGeometryData2,
        NetCompositionData prefabCompositionData2,
        Entity edge,
        Game.Net.Edge edgeData,
        Game.Net.Segment segment,
        Bezier4x3 curve,
        float2 courseOffset,
        float2 edgeOffset)
      {
        float2 float2_1 = new float2(prefabGeometryData.m_DefaultWidth * 0.5f, prefabGeometryData.m_DefaultWidth * -0.5f);
        float3 float3_1 = MathUtils.Position(courseData.m_Curve, courseOffset.x);
        float3 float3_2 = MathUtils.Position(courseData.m_Curve, courseOffset.y);
        float3 float3_3 = MathUtils.Tangent(courseData.m_Curve, courseOffset.x);
        float3 float3_4 = MathUtils.Tangent(courseData.m_Curve, courseOffset.y);
        MathUtils.TryNormalize(ref float3_3);
        MathUtils.TryNormalize(ref float3_4);
        float2 startLeft1 = float3_1.xz - float3_3.zx * float2_1;
        float2 startRight1 = float3_1.xz + float3_3.zx * float2_1;
        float2 endLeft1 = float3_2.xz - float3_4.zx * float2_1;
        float2 endRight1 = float3_2.xz + float3_4.zx * float2_1;
        float x = 0.0f;
        float2 float2_2 = segment.m_Left.a.xz;
        float2 float2_3 = segment.m_Right.a.xz;
        for (int index = 1; index <= 8; ++index)
        {
          float num1 = (float) index / 8f;
          float2 xz1 = MathUtils.Position(segment.m_Left, num1).xz;
          float2 xz2 = MathUtils.Position(segment.m_Right, num1).xz;
          Bounds1 intersectRange1;
          Bounds1 intersectRange2_1;
          // ISSUE: reference to a compiler-generated method
          bool flag = this.QuadIntersect(startLeft1, startRight1, endLeft1, endRight1, float2_2, float2_3, xz1, xz2, out intersectRange1, out intersectRange2_1);
          if ((double) courseOffset.x == 0.0)
          {
            Line2.Segment line = new Line2.Segment(math.lerp(float2_2, float2_3, 0.5f), math.lerp(xz1, xz2, 0.5f));
            float t;
            Bounds1 intersectRange2_2;
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(line, float3_1.xz, out t) <= (double) MathUtils.Distance(line, float3_2.xz, out t) && this.CircleIntersect(new Circle2(float2_1.x, float3_1.xz), float2_2, float2_3, xz1, xz2, out intersectRange2_2))
            {
              intersectRange1 |= 0.0f;
              intersectRange2_1 |= intersectRange2_2;
              flag = true;
            }
          }
          if ((double) courseOffset.y == 1.0)
          {
            Line2.Segment line = new Line2.Segment(math.lerp(float2_2, float2_3, 0.5f), math.lerp(xz1, xz2, 0.5f));
            float t;
            Bounds1 intersectRange2_3;
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(line, float3_1.xz, out t) >= (double) MathUtils.Distance(line, float3_2.xz, out t) && this.CircleIntersect(new Circle2(float2_1.x, float3_2.xz), float2_2, float2_3, xz1, xz2, out intersectRange2_3))
            {
              intersectRange1 |= 1f;
              intersectRange2_1 |= intersectRange2_3;
              flag = true;
            }
          }
          if (flag)
          {
            float2 t1;
            float num2 = MathUtils.Distance(new Line2.Segment(float3_1.xz, float3_2.xz), new Line2.Segment(math.lerp(float2_2, float2_3, 0.5f), math.lerp(xz1, xz2, 0.5f)), out t1);
            float t2 = math.lerp(courseOffset.x, courseOffset.y, t1.x);
            float3 float3_5 = MathUtils.Position(courseData.m_Curve, t2);
            double num3 = (double) MathUtils.Distance(curve.xz, float3_5.xz, out t1.y);
            if ((prefabGeometryData2.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0)
            {
              float length = MathUtils.Snap(MathUtils.Length(curve.xz, new Bounds1(0.0f, t1.y)), 4f);
              Bounds1 t3 = new Bounds1(0.0f, 1f);
              if (MathUtils.ClampLength(curve.xz, ref t3, length))
                t1.y = t3.max;
            }
            intersectRange1.min = math.lerp(courseOffset.x, courseOffset.y, intersectRange1.min - 0.01f);
            intersectRange1.max = math.lerp(courseOffset.x, courseOffset.y, intersectRange1.max + 0.01f);
            Bounds1 bounds1;
            bounds1.min = math.lerp(x, num1, intersectRange2_1.min - 0.01f);
            bounds1.max = math.lerp(x, num1, intersectRange2_1.max + 0.01f);
            intersectRange2_1.min = math.lerp(edgeOffset.x, edgeOffset.y, bounds1.min);
            intersectRange2_1.max = math.lerp(edgeOffset.x, edgeOffset.y, bounds1.max);
            int num4 = -1;
            LocalTransformCache componentData1;
            LocalTransformCache componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.TryGetComponent(edgeData.m_Start, out componentData1) && this.m_LocalTransformCacheData.TryGetComponent(edgeData.m_End, out componentData2) && componentData1.m_ParentMesh == componentData2.m_ParentMesh)
              num4 = componentData1.m_ParentMesh;
            float t4;
            double num5 = (double) MathUtils.Distance(curve.xz, MathUtils.Position(courseData.m_Curve.xz, intersectRange1.min), out t4);
            float t5;
            double num6 = (double) MathUtils.Distance(curve.xz, MathUtils.Position(courseData.m_Curve.xz, intersectRange1.max), out t5);
            if ((double) t5 < (double) t4)
              CommonUtils.Swap<float>(ref bounds1.min, ref bounds1.max);
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.IntersectPos target = new CourseSplitSystem.IntersectPos();
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_Entity = edge;
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_Position = float3_5;
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, t2));
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_CourseDelta = t2;
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_SplitPosition = t1.y;
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_Flags = courseData.m_StartPosition.m_Flags & (CoursePosFlags.IsParallel | CoursePosFlags.IsRight | CoursePosFlags.IsLeft | CoursePosFlags.IsGrid);
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_Flags |= CoursePosFlags.FreeHeight;
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_ParentMesh = num4;
            // ISSUE: reference to a compiler-generated field
            target.m_CourseIntersection = intersectRange1;
            // ISSUE: reference to a compiler-generated field
            target.m_IntersectionHeightMin = new Bounds1((float2) MathUtils.Position(curve, t4).y);
            // ISSUE: reference to a compiler-generated field
            target.m_IntersectionHeightMax = new Bounds1((float2) MathUtils.Position(curve, t5).y);
            // ISSUE: reference to a compiler-generated field
            target.m_EdgeIntersection = intersectRange2_1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            target.m_EdgeHeightRangeMin = this.GetHeightRange(segment, bounds1.min, prefabCompositionData2.m_HeightRange);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            target.m_EdgeHeightRangeMax = this.GetHeightRange(segment, bounds1.max, prefabCompositionData2.m_HeightRange);
            // ISSUE: reference to a compiler-generated field
            target.m_Priority = num2;
            // ISSUE: reference to a compiler-generated field
            target.m_CourseIndex = courseIndex;
            // ISSUE: reference to a compiler-generated field
            target.m_IsTunnel = (prefabCompositionData2.m_Flags.m_General & CompositionFlags.General.Tunnel) > (CompositionFlags.General) 0;
            if ((double) intersectRange2_1.min <= 0.0)
            {
              // ISSUE: reference to a compiler-generated field
              target.m_CoursePos.m_Entity = edgeData.m_Start;
              // ISSUE: reference to a compiler-generated field
              target.m_IsNode = true;
            }
            else if ((double) intersectRange2_1.max >= 1.0)
            {
              // ISSUE: reference to a compiler-generated field
              target.m_CoursePos.m_Entity = edgeData.m_End;
              // ISSUE: reference to a compiler-generated field
              target.m_IsNode = true;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) startIntersectPos.m_Priority != -1.0 && (MathUtils.Intersect(startIntersectPos.m_CourseIntersection, target.m_CourseIntersection) || MathUtils.Intersect(startIntersectPos.m_EdgeIntersection, target.m_EdgeIntersection)) && this.Merge(ref target, startIntersectPos))
            {
              // ISSUE: reference to a compiler-generated field
              startIntersectPos.m_Priority = -1f;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((double) endIntersectPos.m_Priority != -1.0 && (MathUtils.Intersect(endIntersectPos.m_CourseIntersection, target.m_CourseIntersection) || MathUtils.Intersect(endIntersectPos.m_EdgeIntersection, target.m_EdgeIntersection)) && this.Merge(ref target, endIntersectPos))
            {
              // ISSUE: reference to a compiler-generated field
              endIntersectPos.m_Priority = -1f;
            }
            // ISSUE: reference to a compiler-generated field
            if ((double) lastIntersectPos.m_Priority != -1.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!MathUtils.Intersect(lastIntersectPos.m_CourseIntersection, target.m_CourseIntersection) && !MathUtils.Intersect(lastIntersectPos.m_EdgeIntersection, target.m_EdgeIntersection))
              {
                // ISSUE: reference to a compiler-generated method
                this.Add(ref currentIntersectPos, lastIntersectPos);
                lastIntersectPos = target;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.Merge(ref lastIntersectPos, target))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Add(ref currentIntersectPos, lastIntersectPos);
                  lastIntersectPos = target;
                }
              }
            }
            else
              lastIntersectPos = target;
          }
          x = num1;
          float2_2 = xz1;
          float2_3 = xz2;
        }
      }

      private bool Merge(
        ref CourseSplitSystem.IntersectPos target,
        CourseSplitSystem.IntersectPos other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (target.m_IsNode && other.m_IsNode && target.m_CoursePos.m_Entity != other.m_CoursePos.m_Entity)
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (target.m_IsNode && !other.m_IsNode)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          other.m_CoursePos.m_Entity = target.m_CoursePos.m_Entity;
          // ISSUE: reference to a compiler-generated field
          other.m_IsNode = true;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!target.m_IsNode && other.m_IsNode)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            target.m_CoursePos.m_Entity = other.m_CoursePos.m_Entity;
            // ISSUE: reference to a compiler-generated field
            target.m_IsNode = true;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) other.m_CourseIntersection.min < (double) target.m_CourseIntersection.min)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_CourseIntersection.min = other.m_CourseIntersection.min;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_IntersectionHeightMin = other.m_IntersectionHeightMin;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_EdgeHeightRangeMin = other.m_EdgeHeightRangeMin;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) other.m_CourseIntersection.min == (double) target.m_CourseIntersection.min)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            target.m_IntersectionHeightMin |= other.m_IntersectionHeightMin;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            target.m_EdgeHeightRangeMin |= other.m_EdgeHeightRangeMin;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) other.m_CourseIntersection.max > (double) target.m_CourseIntersection.max)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_CourseIntersection.max = other.m_CourseIntersection.max;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_IntersectionHeightMax = other.m_IntersectionHeightMax;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_EdgeHeightRangeMax = other.m_EdgeHeightRangeMax;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) other.m_CourseIntersection.max == (double) target.m_CourseIntersection.max)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            target.m_IntersectionHeightMax |= other.m_IntersectionHeightMax;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            target.m_EdgeHeightRangeMax |= other.m_EdgeHeightRangeMax;
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        target.m_EdgeIntersection |= other.m_EdgeIntersection;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        target.m_IsTunnel &= other.m_IsTunnel;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) other.m_Priority < (double) target.m_Priority)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_CoursePos = other.m_CoursePos;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          target.m_Priority = other.m_Priority;
        }
        return true;
      }

      private void Add(
        ref CourseSplitSystem.IntersectPos current,
        CourseSplitSystem.IntersectPos other)
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) current.m_Priority != -1.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(current.m_CourseIntersection, other.m_CourseIntersection) && !MathUtils.Intersect(current.m_EdgeIntersection, other.m_EdgeIntersection))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Results.Enqueue(current);
            current = other;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if (this.Merge(ref current, other))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_Results.Enqueue(current);
            current = other;
          }
        }
        else
          current = other;
      }

      private bool QuadIntersect(
        float2 startLeft1,
        float2 startRight1,
        float2 endLeft1,
        float2 endRight1,
        float2 startLeft2,
        float2 startRight2,
        float2 endLeft2,
        float2 endRight2,
        out Bounds1 intersectRange1,
        out Bounds1 intersectRange2)
      {
        intersectRange1.min = 1f;
        intersectRange1.max = 0.0f;
        intersectRange2.min = 1f;
        intersectRange2.max = 0.0f;
        Bounds2 bounds1;
        bounds1.min = math.min(math.min(startLeft1, startRight1), math.min(endLeft1, endRight1));
        bounds1.max = math.max(math.max(startLeft1, startRight1), math.max(endLeft1, endRight1));
        Bounds2 bounds2;
        bounds2.min = math.min(math.min(startLeft2, startRight2), math.min(endLeft2, endRight2));
        bounds2.max = math.max(math.max(startLeft2, startRight2), math.max(endLeft2, endRight2));
        if (!MathUtils.Intersect(bounds1, bounds2))
          return false;
        Triangle2 triangle1 = new Triangle2(startLeft1, endLeft1, endRight1);
        Triangle2 triangle2 = new Triangle2(endRight1, startRight1, startLeft1);
        Triangle2 triangle3 = new Triangle2(startLeft2, endLeft2, endRight2);
        Triangle2 triangle4 = new Triangle2(endRight2, startRight2, startLeft2);
        Line2.Segment line1_1 = new Line2.Segment(startLeft1, startRight1);
        Line2.Segment line1_2 = new Line2.Segment(endLeft1, endRight1);
        Line2.Segment line1_3 = new Line2.Segment(startLeft1, endLeft1);
        Line2.Segment line1_4 = new Line2.Segment(startRight1, endRight1);
        Line2.Segment line2_1 = new Line2.Segment(startLeft2, startRight2);
        Line2.Segment line2_2 = new Line2.Segment(endLeft2, endRight2);
        Line2.Segment line2_3 = new Line2.Segment(startLeft2, endLeft2);
        Line2.Segment line2_4 = new Line2.Segment(startRight2, endRight2);
        float2 t;
        if (MathUtils.Intersect(triangle1, startLeft2, out t))
        {
          intersectRange1 |= t.x + t.y;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(triangle1, startRight2, out t))
        {
          intersectRange1 |= t.x + t.y;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(triangle1, endLeft2, out t))
        {
          intersectRange1 |= t.x + t.y;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(triangle1, endRight2, out t))
        {
          intersectRange1 |= t.x + t.y;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(triangle2, startLeft2, out t))
        {
          intersectRange1 |= 1f - t.x - t.y;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(triangle2, startRight2, out t))
        {
          intersectRange1 |= 1f - t.x - t.y;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(triangle2, endLeft2, out t))
        {
          intersectRange1 |= 1f - t.x - t.y;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(triangle2, endRight2, out t))
        {
          intersectRange1 |= 1f - t.x - t.y;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(triangle3, startLeft1, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= t.x + t.y;
        }
        if (MathUtils.Intersect(triangle3, startRight1, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= t.x + t.y;
        }
        if (MathUtils.Intersect(triangle3, endLeft1, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= t.x + t.y;
        }
        if (MathUtils.Intersect(triangle3, endRight1, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= t.x + t.y;
        }
        if (MathUtils.Intersect(triangle4, startLeft1, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= 1f - t.x - t.y;
        }
        if (MathUtils.Intersect(triangle4, startRight1, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= 1f - t.x - t.y;
        }
        if (MathUtils.Intersect(triangle4, endLeft1, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= 1f - t.x - t.y;
        }
        if (MathUtils.Intersect(triangle4, endRight1, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= 1f - t.x - t.y;
        }
        if (MathUtils.Intersect(line1_1, line2_1, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(line1_1, line2_2, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(line1_1, line2_3, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_1, line2_4, out t))
        {
          intersectRange1 |= 0.0f;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_2, line2_1, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(line1_2, line2_2, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(line1_2, line2_3, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_2, line2_4, out t))
        {
          intersectRange1 |= 1f;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_3, line2_1, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(line1_3, line2_2, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(line1_3, line2_3, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_3, line2_4, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_4, line2_1, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= 0.0f;
        }
        if (MathUtils.Intersect(line1_4, line2_2, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= 1f;
        }
        if (MathUtils.Intersect(line1_4, line2_3, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= t.y;
        }
        if (MathUtils.Intersect(line1_4, line2_4, out t))
        {
          intersectRange1 |= t.x;
          intersectRange2 |= t.y;
        }
        return (double) intersectRange1.min <= (double) intersectRange1.max;
      }

      private bool CircleIntersect(
        Circle2 circle1,
        float2 startLeft2,
        float2 startRight2,
        float2 endLeft2,
        float2 endRight2,
        out Bounds1 intersectRange2)
      {
        intersectRange2.min = 1f;
        intersectRange2.max = 0.0f;
        Bounds2 bounds1 = MathUtils.Bounds(circle1);
        Bounds2 bounds2_1;
        bounds2_1.min = math.min(math.min(startLeft2, startRight2), math.min(endLeft2, endRight2));
        bounds2_1.max = math.max(math.max(startLeft2, startRight2), math.max(endLeft2, endRight2));
        Bounds2 bounds2_2 = bounds2_1;
        if (!MathUtils.Intersect(bounds1, bounds2_2))
          return false;
        Triangle2 triangle1 = new Triangle2(startLeft2, endLeft2, endRight2);
        Triangle2 triangle2 = new Triangle2(endRight2, startRight2, startLeft2);
        Line2.Segment line1 = new Line2.Segment(startLeft2, startRight2);
        Line2.Segment line2 = new Line2.Segment(endLeft2, endRight2);
        Line2.Segment line3 = new Line2.Segment(startLeft2, endLeft2);
        Line2.Segment line4 = new Line2.Segment(startRight2, endRight2);
        float2 t;
        if (MathUtils.Intersect(triangle1, circle1.position, out t))
        {
          float2 float2_1 = new float2(math.distance(triangle1.a, triangle1.b), math.distance(triangle1.a, triangle1.c));
          float2 float2_2 = circle1.radius * t / (float2_1 * (t.x + t.y));
          float2 float2_3 = math.max((float2) 0.0f, t - float2_2);
          float2 float2_4 = math.min((float2) 1f, t + float2_2);
          intersectRange2 |= float2_3.x + float2_4.y;
          intersectRange2 |= float2_4.x + float2_4.y;
        }
        if (MathUtils.Intersect(triangle2, circle1.position, out t))
        {
          float2 float2_5 = new float2(math.distance(triangle2.a, triangle2.b), math.distance(triangle2.a, triangle2.c));
          float2 float2_6 = circle1.radius * t / (float2_5 * (t.x + t.y));
          float2 float2_7 = math.max((float2) 0.0f, t - float2_6);
          float2 float2_8 = math.min((float2) 1f, t + float2_6);
          intersectRange2 |= 1f - float2_7.x - float2_8.y;
          intersectRange2 |= 1f - float2_8.x - float2_8.y;
        }
        if (MathUtils.Intersect(circle1, line1, out t))
          intersectRange2 |= 0.0f;
        if (MathUtils.Intersect(circle1, line2, out t))
          intersectRange2 |= 1f;
        if (MathUtils.Intersect(circle1, line3, out t))
          intersectRange2 |= new Bounds1(t.x, t.y);
        if (MathUtils.Intersect(circle1, line4, out t))
          intersectRange2 |= new Bounds1(t.x, t.y);
        return (double) intersectRange2.min <= (double) intersectRange2.max;
      }

      private Bounds1 GetHeightRange(Bezier4x3 curve, float curvePos, Bounds1 heightRange)
      {
        return MathUtils.Position(curve, curvePos).y + heightRange;
      }

      private Bounds1 GetHeightRange(Game.Net.Segment segment, float curvePos, Bounds1 heightRange)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return this.GetHeightRange(segment.m_Left, curvePos, heightRange) | this.GetHeightRange(segment.m_Right, curvePos, heightRange);
      }

      private void CheckNodeSegment(
        NetCourse courseData,
        int courseIndex,
        ref CourseSplitSystem.IntersectPos currentIntersectPos,
        NetGeometryData prefabGeometryData,
        NetCompositionData prefabCompositionData2,
        Entity edge,
        Entity node,
        Game.Net.Segment segment,
        float2 courseOffset,
        float edgeOffset,
        float centerOffset)
      {
        float2 float2_1 = new float2(prefabGeometryData.m_DefaultWidth * 0.5f, prefabGeometryData.m_DefaultWidth * -0.5f);
        float3 float3_1 = MathUtils.Position(courseData.m_Curve, courseOffset.x);
        float3 float3_2 = MathUtils.Position(courseData.m_Curve, courseOffset.y);
        float3 float3_3 = MathUtils.Tangent(courseData.m_Curve, courseOffset.x);
        float3 float3_4 = MathUtils.Tangent(courseData.m_Curve, courseOffset.y);
        MathUtils.TryNormalize(ref float3_3);
        MathUtils.TryNormalize(ref float3_4);
        float2 startLeft1 = float3_1.xz - float3_3.zx * float2_1;
        float2 startRight1 = float3_1.xz + float3_3.zx * float2_1;
        float2 endLeft1 = float3_2.xz - float3_4.zx * float2_1;
        float2 endRight1 = float3_2.xz + float3_4.zx * float2_1;
        float x = 0.0f;
        float2 float2_2 = segment.m_Left.a.xz;
        float2 float2_3 = segment.m_Right.a.xz;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos target = new CourseSplitSystem.IntersectPos();
        // ISSUE: reference to a compiler-generated field
        target.m_Priority = -1f;
        for (int index = 1; index <= 8; ++index)
        {
          float num1 = (float) index / 8f;
          float2 xz1 = MathUtils.Position(segment.m_Left, num1).xz;
          float2 xz2 = MathUtils.Position(segment.m_Right, num1).xz;
          Bounds1 intersectRange1;
          Bounds1 intersectRange2_1;
          // ISSUE: reference to a compiler-generated method
          bool flag = this.QuadIntersect(startLeft1, startRight1, endLeft1, endRight1, float2_2, float2_3, xz1, xz2, out intersectRange1, out intersectRange2_1);
          if (flag)
          {
            if ((prefabCompositionData2.m_Flags.m_General & (CompositionFlags.General.DeadEnd | CompositionFlags.General.Roundabout)) == CompositionFlags.General.DeadEnd)
            {
              float t1;
              double num2 = (double) MathUtils.Distance(courseData.m_Curve.xz, segment.m_Left.a.xz, out t1);
              float t2;
              double num3 = (double) MathUtils.Distance(courseData.m_Curve.xz, segment.m_Right.a.xz, out t2);
              intersectRange1 = MathUtils.Bounds(t1, t2);
              intersectRange1.min = math.select(intersectRange1.min, 0.0f, (double) courseOffset.x == 0.0 && (double) intersectRange1.min <= 0.0099999997764825821);
              intersectRange1.max = math.select(intersectRange1.max, 1f, (double) courseOffset.y == 1.0 && (double) intersectRange1.max >= 0.99000000953674316);
            }
            else
            {
              intersectRange1.min = math.lerp(courseOffset.x, courseOffset.y, intersectRange1.min - 0.01f);
              intersectRange1.max = math.lerp(courseOffset.x, courseOffset.y, intersectRange1.max + 0.01f);
            }
          }
          if ((double) courseOffset.x == 0.0)
          {
            Line2.Segment line = new Line2.Segment(math.lerp(float2_2, float2_3, centerOffset), math.lerp(xz1, xz2, centerOffset));
            float t;
            Bounds1 intersectRange2_2;
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(line, float3_1.xz, out t) <= (double) MathUtils.Distance(line, float3_2.xz, out t) && this.CircleIntersect(new Circle2(float2_1.x, float3_1.xz), float2_2, float2_3, xz1, xz2, out intersectRange2_2))
            {
              intersectRange1 |= 0.0f;
              intersectRange2_1 |= intersectRange2_2;
              flag = true;
            }
          }
          if ((double) courseOffset.y == 1.0)
          {
            Line2.Segment line = new Line2.Segment(math.lerp(float2_2, float2_3, centerOffset), math.lerp(xz1, xz2, centerOffset));
            float t;
            Bounds1 intersectRange2_3;
            // ISSUE: reference to a compiler-generated method
            if ((double) MathUtils.Distance(line, float3_1.xz, out t) >= (double) MathUtils.Distance(line, float3_2.xz, out t) && this.CircleIntersect(new Circle2(float2_1.x, float3_2.xz), float2_2, float2_3, xz1, xz2, out intersectRange2_3))
            {
              intersectRange1 |= 1f;
              intersectRange2_1 |= intersectRange2_3;
              flag = true;
            }
          }
          if (flag)
          {
            float2 t3;
            float num4 = MathUtils.Distance(new Line2.Segment(float3_1.xz, float3_2.xz), new Line2.Segment(math.lerp(float2_2, float2_3, centerOffset), math.lerp(xz1, xz2, centerOffset)), out t3);
            float t4 = math.lerp(courseOffset.x, courseOffset.y, t3.x);
            float3 float3_5 = MathUtils.Position(courseData.m_Curve, t4);
            Bounds1 bounds1;
            bounds1.min = math.lerp(x, num1, intersectRange2_1.min - 0.01f);
            bounds1.max = math.lerp(x, num1, intersectRange2_1.max + 0.01f);
            intersectRange2_1 = new Bounds1(edgeOffset, edgeOffset);
            int num5 = -1;
            LocalTransformCache componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LocalTransformCacheData.TryGetComponent(node, out componentData))
              num5 = componentData.m_ParentMesh;
            Bezier4x3 curve = MathUtils.Lerp(segment.m_Left, segment.m_Right, 0.5f);
            float t5;
            double num6 = (double) MathUtils.Distance(curve.xz, MathUtils.Position(courseData.m_Curve.xz, intersectRange1.min), out t5);
            float t6;
            double num7 = (double) MathUtils.Distance(curve.xz, MathUtils.Position(courseData.m_Curve.xz, intersectRange1.max), out t6);
            if ((double) t6 < (double) t5)
              CommonUtils.Swap<float>(ref bounds1.min, ref bounds1.max);
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.IntersectPos other = new CourseSplitSystem.IntersectPos();
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_Entity = node;
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_Position = float3_5;
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, t4));
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_CourseDelta = t4;
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_SplitPosition = edgeOffset;
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_Flags = courseData.m_StartPosition.m_Flags & (CoursePosFlags.IsParallel | CoursePosFlags.IsRight | CoursePosFlags.IsLeft | CoursePosFlags.IsGrid);
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_Flags |= CoursePosFlags.FreeHeight;
            // ISSUE: reference to a compiler-generated field
            other.m_CoursePos.m_ParentMesh = num5;
            // ISSUE: reference to a compiler-generated field
            other.m_CourseIntersection = intersectRange1;
            // ISSUE: reference to a compiler-generated field
            other.m_IntersectionHeightMin = new Bounds1((float2) MathUtils.Position(curve, t5).y);
            // ISSUE: reference to a compiler-generated field
            other.m_IntersectionHeightMax = new Bounds1((float2) MathUtils.Position(curve, t6).y);
            // ISSUE: reference to a compiler-generated field
            other.m_EdgeIntersection = intersectRange2_1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            other.m_EdgeHeightRangeMin = this.GetHeightRange(segment, bounds1.min, prefabCompositionData2.m_HeightRange);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            other.m_EdgeHeightRangeMax = this.GetHeightRange(segment, bounds1.max, prefabCompositionData2.m_HeightRange);
            // ISSUE: reference to a compiler-generated field
            other.m_Priority = num4;
            // ISSUE: reference to a compiler-generated field
            other.m_CourseIndex = courseIndex;
            // ISSUE: reference to a compiler-generated field
            other.m_IsNode = true;
            // ISSUE: reference to a compiler-generated field
            other.m_IsTunnel = (prefabCompositionData2.m_Flags.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0 && ((prefabCompositionData2.m_Flags.m_Left | prefabCompositionData2.m_Flags.m_Right) & (CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition)) == (CompositionFlags.Side) 0;
            // ISSUE: reference to a compiler-generated field
            if ((double) target.m_Priority != -1.0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!MathUtils.Intersect(target.m_CourseIntersection, other.m_CourseIntersection) && !MathUtils.Intersect(target.m_EdgeIntersection, other.m_EdgeIntersection))
              {
                // ISSUE: reference to a compiler-generated method
                this.Add(ref currentIntersectPos, target);
                target = other;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                if (!this.Merge(ref target, other))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Add(ref currentIntersectPos, target);
                  target = other;
                }
              }
            }
            else
              target = other;
          }
          x = num1;
          float2_2 = xz1;
          float2_3 = xz2;
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) target.m_Priority == -1.0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.Add(ref currentIntersectPos, target);
      }
    }

    private struct CourseHeightItem
    {
      public float m_TerrainHeight;
      public float m_TerrainBuildHeight;
      public float m_WaterHeight;
      public float m_CourseHeight;
      public float m_DistanceOffset;
      public Bounds1 m_LimitRange;
      public float2 m_LimitDistance;
      public bool m_ForceElevated;
    }

    private struct CourseHeightData
    {
      private NativeArray<CourseSplitSystem.CourseHeightItem> m_Buffer;
      private float2 m_SampleRange;
      private float m_SampleFactor;

      public CourseHeightData(
        Allocator allocator,
        NetCourse course,
        NetGeometryData netGeometryData,
        bool sampleTerrain,
        ref TerrainHeightData terrainHeightData,
        ref WaterSurfaceData waterSurfaceData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SampleRange = new float2(course.m_StartPosition.m_CourseDelta, course.m_EndPosition.m_CourseDelta);
        // ISSUE: reference to a compiler-generated field
        Bezier4x3 curve = MathUtils.Cut(course.m_Curve, this.m_SampleRange);
        int length = 1 + Mathf.CeilToInt(MathUtils.Length(curve.xz) / 4f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SampleFactor = (double) this.m_SampleRange.y <= (double) this.m_SampleRange.x ? 0.0f : (float) (length - 1) / (this.m_SampleRange.y - this.m_SampleRange.x);
        // ISSUE: reference to a compiler-generated field
        this.m_Buffer = new NativeArray<CourseSplitSystem.CourseHeightItem>(length, allocator);
        float3 float3 = course.m_StartPosition.m_Position;
        float num1 = 1f / (float) math.max(1, length - 1);
        float num2 = 0.0f;
        float num3 = math.max(course.m_StartPosition.m_Elevation.x, course.m_EndPosition.m_Elevation.x);
        for (int index = 0; index < length; ++index)
        {
          float3 worldPosition;
          float num4;
          if (index == 0)
          {
            worldPosition = course.m_StartPosition.m_Position;
            num4 = course.m_StartPosition.m_Elevation.x;
          }
          else if (index == length - 1)
          {
            worldPosition = course.m_EndPosition.m_Position;
            num4 = course.m_EndPosition.m_Elevation.x;
          }
          else
          {
            worldPosition = MathUtils.Position(curve, (float) index * num1);
            num4 = math.lerp(course.m_StartPosition.m_Elevation.x, course.m_EndPosition.m_Elevation.x, (float) index * num1);
          }
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = new CourseSplitSystem.CourseHeightItem();
          if (sampleTerrain)
          {
            float waterDepth;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, worldPosition, out courseHeightItem.m_TerrainHeight, out courseHeightItem.m_WaterHeight, out waterDepth);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            courseHeightItem.m_TerrainBuildHeight = math.select(courseHeightItem.m_TerrainHeight, courseHeightItem.m_TerrainHeight + num3, (double) num3 < -1.0);
            if ((double) waterDepth < 0.20000000298023224)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              courseHeightItem.m_WaterHeight = courseHeightItem.m_TerrainHeight;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              courseHeightItem.m_WaterHeight += netGeometryData.m_ElevationLimit * 2f;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            courseHeightItem.m_TerrainHeight = worldPosition.y - num4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            courseHeightItem.m_TerrainBuildHeight = math.select(courseHeightItem.m_TerrainHeight, courseHeightItem.m_TerrainHeight + num3, (double) num3 < -1.0);
            // ISSUE: reference to a compiler-generated field
            courseHeightItem.m_WaterHeight = -1000000f;
            // ISSUE: reference to a compiler-generated field
            courseHeightItem.m_CourseHeight = worldPosition.y;
          }
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_DistanceOffset = math.distance(float3.xz, worldPosition.xz);
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_LimitRange = new Bounds1(-1000000f, 1000000f);
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_LimitDistance = (float2) 1000000f;
          // ISSUE: reference to a compiler-generated field
          this.m_Buffer[index] = courseHeightItem;
          float3 = worldPosition;
          // ISSUE: reference to a compiler-generated field
          num2 += courseHeightItem.m_DistanceOffset;
        }
        if (!sampleTerrain)
          return;
        // ISSUE: reference to a compiler-generated method
        this.InitializeCoursePos(ref course.m_StartPosition);
        // ISSUE: reference to a compiler-generated method
        this.InitializeCoursePos(ref course.m_EndPosition);
        Bounds1 bounds1 = new Bounds1(-1000000f, 1000000f);
        Bounds1 bounds2 = new Bounds1(-1000000f, 1000000f);
        if ((double) course.m_StartPosition.m_Elevation.x >= (double) netGeometryData.m_ElevationLimit || (double) course.m_EndPosition.m_Elevation.x >= (double) netGeometryData.m_ElevationLimit || (netGeometryData.m_Flags & Game.Net.GeometryFlags.RequireElevated) != (Game.Net.GeometryFlags) 0)
        {
          bounds1.min = course.m_StartPosition.m_Position.y;
          bounds2.min = course.m_EndPosition.m_Position.y;
        }
        else
        {
          if ((double) course.m_StartPosition.m_Elevation.x > 1.0)
          {
            bounds1.min = course.m_StartPosition.m_Position.y;
            bounds2.min = math.max(bounds2.min, course.m_EndPosition.m_Position.y - (float) ((double) num2 * (double) netGeometryData.m_MaxSlopeSteepness * 0.5));
          }
          if ((double) course.m_EndPosition.m_Elevation.x > 1.0)
          {
            bounds1.min = math.max(bounds1.min, course.m_StartPosition.m_Position.y - (float) ((double) num2 * (double) netGeometryData.m_MaxSlopeSteepness * 0.5));
            bounds2.min = course.m_EndPosition.m_Position.y;
          }
        }
        if ((double) course.m_StartPosition.m_Elevation.x <= -(double) netGeometryData.m_ElevationLimit || (double) course.m_EndPosition.m_Elevation.x <= -(double) netGeometryData.m_ElevationLimit)
        {
          bounds1.max = course.m_StartPosition.m_Position.y;
          bounds2.max = course.m_EndPosition.m_Position.y;
        }
        else
        {
          if ((double) course.m_StartPosition.m_Elevation.x < -1.0)
          {
            bounds1.max = course.m_StartPosition.m_Position.y;
            bounds2.max = math.min(bounds2.max, course.m_EndPosition.m_Position.y + (float) ((double) num2 * (double) netGeometryData.m_MaxSlopeSteepness * 0.5));
          }
          if ((double) course.m_EndPosition.m_Elevation.x < -1.0)
          {
            bounds1.max = math.min(bounds1.max, course.m_StartPosition.m_Position.y + (float) ((double) num2 * (double) netGeometryData.m_MaxSlopeSteepness * 0.5));
            bounds2.max = course.m_EndPosition.m_Position.y;
          }
        }
        float num5 = -1000000f;
        float num6 = 0.0f;
        for (int index = 0; index < length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index];
          Bounds1 bounds;
          if (index == 0)
            bounds = bounds1;
          else if (index == length - 1)
          {
            bounds = bounds2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            num6 += courseHeightItem.m_DistanceOffset;
            bounds = MathUtils.Lerp(bounds1, bounds2, num6 / num2);
          }
          // ISSUE: reference to a compiler-generated field
          float y = num5 - courseHeightItem.m_DistanceOffset * netGeometryData.m_MaxSlopeSteepness;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_CourseHeight = math.select(math.max(courseHeightItem.m_TerrainHeight, courseHeightItem.m_WaterHeight), courseHeightItem.m_TerrainBuildHeight, (double) num3 < -1.0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_CourseHeight = MathUtils.Clamp(courseHeightItem.m_CourseHeight, bounds);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_CourseHeight = math.max(courseHeightItem.m_CourseHeight, y);
          // ISSUE: reference to a compiler-generated field
          num5 = courseHeightItem.m_CourseHeight;
          // ISSUE: reference to a compiler-generated field
          this.m_Buffer[index] = courseHeightItem;
        }
        float y1 = -1000000f;
        for (int index = length - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_CourseHeight = math.max(courseHeightItem.m_CourseHeight, y1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          y1 = courseHeightItem.m_CourseHeight - courseHeightItem.m_DistanceOffset * netGeometryData.m_MaxSlopeSteepness;
          // ISSUE: reference to a compiler-generated field
          this.m_Buffer[index] = courseHeightItem;
        }
      }

      public void InitializeCoursePos(ref CoursePos coursePos)
      {
        if ((coursePos.m_Flags & CoursePosFlags.FreeHeight) == (CoursePosFlags) 0)
          return;
        float num1;
        // ISSUE: reference to a compiler-generated field
        if ((double) coursePos.m_CourseDelta == (double) this.m_SampleRange.x)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          num1 = math.select(math.max(courseHeightItem.m_TerrainHeight, courseHeightItem.m_WaterHeight), courseHeightItem.m_TerrainHeight, (double) coursePos.m_Elevation.x < -1.0);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) coursePos.m_CourseDelta == (double) this.m_SampleRange.y)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[this.m_Buffer.Length - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            num1 = math.select(math.max(courseHeightItem.m_TerrainHeight, courseHeightItem.m_WaterHeight), courseHeightItem.m_TerrainHeight, (double) coursePos.m_Elevation.x < -1.0);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float f = (coursePos.m_CourseDelta - this.m_SampleRange.x) * this.m_SampleFactor;
            // ISSUE: reference to a compiler-generated field
            int index = math.clamp(Mathf.FloorToInt(f), 0, this.m_Buffer.Length - 1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem1 = this.m_Buffer[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem2 = this.m_Buffer[math.min(index + 1, this.m_Buffer.Length - 1)];
            float s = math.saturate(f - (float) index);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num2 = math.lerp(courseHeightItem1.m_TerrainHeight, courseHeightItem2.m_TerrainHeight, s);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float y = math.lerp(courseHeightItem1.m_WaterHeight, courseHeightItem2.m_WaterHeight, s);
            num1 = math.select(math.max(num2, y), num2, (double) coursePos.m_Elevation.x < -1.0);
          }
        }
        coursePos.m_Position.y = num1 + coursePos.m_Elevation.x;
      }

      public void ApplyLimitRange(
        CourseSplitSystem.IntersectPos intersectPos,
        NetGeometryData netGeometryData,
        Bounds1 limitRangeMin,
        Bounds1 limitRangeMax,
        bool shrink = false,
        bool forceElevated = false)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 float2 = (new float2(intersectPos.m_CourseIntersection.min, intersectPos.m_CourseIntersection.max) - this.m_SampleRange.x) * this.m_SampleFactor;
        int x1;
        int x2;
        if (shrink)
        {
          x1 = math.max(Mathf.RoundToInt(float2.x), 0);
          // ISSUE: reference to a compiler-generated field
          x2 = math.min(Mathf.RoundToInt(float2.y), this.m_Buffer.Length - 1);
        }
        else
        {
          x1 = math.max(Mathf.FloorToInt(float2.x), 0);
          // ISSUE: reference to a compiler-generated field
          x2 = math.min(Mathf.CeilToInt(float2.y), this.m_Buffer.Length - 1);
        }
        // ISSUE: reference to a compiler-generated field
        int index1 = math.min(x1, this.m_Buffer.Length);
        int num1 = math.max(x2, -1);
        float num2 = 1f / (float) math.max(1, num1 - index1);
        if (forceElevated)
        {
          for (int index2 = index1; index2 <= num1; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index2];
            Bounds1 limitRange = MathUtils.Lerp(limitRangeMin, limitRangeMax, (float) (index2 - index1) * num2);
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            if (this.AddLimit(ref courseHeightItem, limitRange, 0.0f) || !courseHeightItem.m_ForceElevated)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              courseHeightItem.m_CourseHeight = MathUtils.Clamp(courseHeightItem.m_CourseHeight, courseHeightItem.m_LimitRange);
              // ISSUE: reference to a compiler-generated field
              courseHeightItem.m_ForceElevated |= forceElevated;
              // ISSUE: reference to a compiler-generated field
              this.m_Buffer[index2] = courseHeightItem;
            }
          }
        }
        else
        {
          for (int index3 = index1; index3 <= num1; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index3];
            Bounds1 limitRange = MathUtils.Lerp(limitRangeMin, limitRangeMax, (float) (index3 - index1) * num2);
            // ISSUE: reference to a compiler-generated method
            if (this.AddLimit(ref courseHeightItem, limitRange, 0.0f))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              courseHeightItem.m_CourseHeight = MathUtils.Clamp(courseHeightItem.m_CourseHeight, courseHeightItem.m_LimitRange);
              // ISSUE: reference to a compiler-generated field
              this.m_Buffer[index3] = courseHeightItem;
            }
          }
        }
        if (index1 > 0)
        {
          float distance = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (index1 < this.m_Buffer.Length)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index1];
            // ISSUE: reference to a compiler-generated field
            distance += courseHeightItem.m_DistanceOffset;
          }
          for (int index4 = index1 - 1; index4 >= 0; --index4)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index4];
            Bounds1 limitRange = MathUtils.Expand(limitRangeMin, distance * netGeometryData.m_MaxSlopeSteepness);
            // ISSUE: reference to a compiler-generated method
            if (this.AddLimit(ref courseHeightItem, limitRange, distance))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              courseHeightItem.m_CourseHeight = MathUtils.Clamp(courseHeightItem.m_CourseHeight, courseHeightItem.m_LimitRange);
              // ISSUE: reference to a compiler-generated field
              distance += courseHeightItem.m_DistanceOffset;
              // ISSUE: reference to a compiler-generated field
              this.m_Buffer[index4] = courseHeightItem;
            }
            else
              break;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (num1 >= this.m_Buffer.Length - 1)
          return;
        float distance1 = 0.0f;
        // ISSUE: reference to a compiler-generated field
        for (int index5 = num1 + 1; index5 < this.m_Buffer.Length; ++index5)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index5];
          // ISSUE: reference to a compiler-generated field
          distance1 += courseHeightItem.m_DistanceOffset;
          Bounds1 limitRange = MathUtils.Expand(limitRangeMax, distance1 * netGeometryData.m_MaxSlopeSteepness);
          // ISSUE: reference to a compiler-generated method
          if (!this.AddLimit(ref courseHeightItem, limitRange, distance1))
            break;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          courseHeightItem.m_CourseHeight = MathUtils.Clamp(courseHeightItem.m_CourseHeight, courseHeightItem.m_LimitRange);
          // ISSUE: reference to a compiler-generated field
          this.m_Buffer[index5] = courseHeightItem;
        }
      }

      private bool AddLimit(
        ref CourseSplitSystem.CourseHeightItem item,
        Bounds1 limitRange,
        float distance)
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) limitRange.min > (double) item.m_LimitRange.min)
        {
          // ISSUE: reference to a compiler-generated field
          item.m_LimitDistance.x = distance;
          // ISSUE: reference to a compiler-generated field
          if ((double) limitRange.min > (double) item.m_LimitRange.max)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float s = math.select(distance / math.csum(item.m_LimitDistance), 0.5f, math.all(item.m_LimitDistance == 0.0f));
            // ISSUE: reference to a compiler-generated field
            limitRange = new Bounds1((float2) math.lerp(limitRange.min, item.m_LimitRange.max, s));
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          limitRange.min = item.m_LimitRange.min;
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) limitRange.max < (double) item.m_LimitRange.max)
        {
          // ISSUE: reference to a compiler-generated field
          item.m_LimitDistance.y = distance;
          // ISSUE: reference to a compiler-generated field
          if ((double) limitRange.max < (double) item.m_LimitRange.min)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float s = math.select(distance / math.csum(item.m_LimitDistance), 0.5f, math.all(item.m_LimitDistance == 0.0f));
            // ISSUE: reference to a compiler-generated field
            limitRange = new Bounds1((float2) math.lerp(limitRange.max, item.m_LimitRange.min, s));
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          limitRange.max = item.m_LimitRange.max;
        }
        // ISSUE: reference to a compiler-generated field
        int num = !limitRange.Equals(item.m_LimitRange) ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        item.m_LimitRange = limitRange;
        return num != 0;
      }

      public void StraightenElevation(
        CourseSplitSystem.IntersectPos firstPos,
        CourseSplitSystem.IntersectPos lastPos)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 float2 = (new float2(firstPos.m_CourseIntersection.max, lastPos.m_CourseIntersection.min) - this.m_SampleRange.x) * this.m_SampleFactor;
        int num1 = math.max(Mathf.FloorToInt(float2.x), 0);
        // ISSUE: reference to a compiler-generated field
        int num2 = math.min(Mathf.CeilToInt(float2.y), this.m_Buffer.Length - 1);
        int index1 = num1 + 1;
        while (index1 < num2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem1 = this.m_Buffer[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) courseHeightItem1.m_CourseHeight != (double) courseHeightItem1.m_TerrainBuildHeight && (double) courseHeightItem1.m_LimitRange.min < (double) courseHeightItem1.m_LimitRange.max)
          {
            // ISSUE: reference to a compiler-generated field
            float distanceOffset = courseHeightItem1.m_DistanceOffset;
            int index2 = index1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem2 = this.m_Buffer[index2];
            for (; index2 < num2; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              courseHeightItem2 = this.m_Buffer[index2 + 1];
              // ISSUE: reference to a compiler-generated field
              distanceOffset += courseHeightItem2.m_DistanceOffset;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((double) courseHeightItem2.m_CourseHeight == (double) courseHeightItem2.m_TerrainBuildHeight || (double) courseHeightItem2.m_LimitRange.min >= (double) courseHeightItem2.m_LimitRange.max)
                break;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CourseHeightItem courseHeightItem3 = this.m_Buffer[index1 - 1];
            float num3 = 0.0f;
            for (int index3 = index1; index3 <= index2; ++index3)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CourseHeightItem courseHeightItem4 = this.m_Buffer[index3];
              // ISSUE: reference to a compiler-generated field
              num3 += courseHeightItem4.m_DistanceOffset;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              courseHeightItem4.m_CourseHeight = math.lerp(courseHeightItem3.m_CourseHeight, courseHeightItem2.m_CourseHeight, num3 / distanceOffset);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              courseHeightItem4.m_CourseHeight = MathUtils.Clamp(courseHeightItem4.m_CourseHeight, courseHeightItem4.m_LimitRange);
              // ISSUE: reference to a compiler-generated field
              this.m_Buffer[index3] = courseHeightItem4;
            }
            index1 = index2 + 1;
          }
          else
            ++index1;
        }
      }

      public void SampleCourseHeight(ref NetCourse course, NetGeometryData netGeometryData)
      {
        if ((course.m_StartPosition.m_Flags & CoursePosFlags.FreeHeight) != (CoursePosFlags) 0)
        {
          bool forceElevated;
          // ISSUE: reference to a compiler-generated method
          course.m_StartPosition.m_Position.y = this.SampleHeight(course.m_StartPosition.m_CourseDelta, out forceElevated);
          if (forceElevated)
            course.m_StartPosition.m_Flags |= CoursePosFlags.ForceElevatedNode;
        }
        else
        {
          bool forceElevated;
          // ISSUE: reference to a compiler-generated method
          double num = (double) this.SampleHeight(course.m_StartPosition.m_CourseDelta, out forceElevated);
          if (forceElevated)
            course.m_StartPosition.m_Flags |= CoursePosFlags.ForceElevatedNode;
        }
        if ((course.m_EndPosition.m_Flags & CoursePosFlags.FreeHeight) != (CoursePosFlags) 0)
        {
          bool forceElevated;
          // ISSUE: reference to a compiler-generated method
          course.m_EndPosition.m_Position.y = this.SampleHeight(course.m_EndPosition.m_CourseDelta, out forceElevated);
          if (forceElevated)
            course.m_EndPosition.m_Flags |= CoursePosFlags.ForceElevatedNode;
        }
        else
        {
          bool forceElevated;
          // ISSUE: reference to a compiler-generated method
          double num = (double) this.SampleHeight(course.m_EndPosition.m_CourseDelta, out forceElevated);
          if (forceElevated)
            course.m_EndPosition.m_Flags |= CoursePosFlags.ForceElevatedNode;
        }
        if (course.m_StartPosition.m_Position.Equals(course.m_EndPosition.m_Position))
        {
          if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
            course.m_Curve.a = course.m_StartPosition.m_Position;
          else
            course.m_Curve.a.y = course.m_StartPosition.m_Position.y;
          course.m_Curve.b = course.m_Curve.a;
          course.m_Curve.c = course.m_Curve.a;
          course.m_Curve.d = course.m_Curve.a;
        }
        else if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StraightEdges) != (Game.Net.GeometryFlags) 0)
        {
          float2 float2_1 = new float2(course.m_StartPosition.m_CourseDelta, course.m_EndPosition.m_CourseDelta);
          float2 float2_2 = math.lerp((float2) float2_1.x, (float2) float2_1.y, new float2(0.333333343f, 0.6666667f));
          bool forceElevated1;
          // ISSUE: reference to a compiler-generated method
          double num1 = (double) this.SampleHeight(float2_2.x, out forceElevated1);
          bool forceElevated2;
          // ISSUE: reference to a compiler-generated method
          double num2 = (double) this.SampleHeight(float2_2.y, out forceElevated2);
          if (forceElevated1 | forceElevated2)
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.ForceElevatedEdge;
            course.m_EndPosition.m_Flags |= CoursePosFlags.ForceElevatedEdge;
          }
          if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
          {
            course.m_Curve = NetUtils.StraightCurve(course.m_StartPosition.m_Position, course.m_EndPosition.m_Position, netGeometryData.m_Hanging);
          }
          else
          {
            float3 startPos = MathUtils.Position(course.m_Curve, course.m_StartPosition.m_CourseDelta);
            float3 endPos = MathUtils.Position(course.m_Curve, course.m_EndPosition.m_CourseDelta);
            startPos.y = course.m_StartPosition.m_Position.y;
            endPos.y = course.m_EndPosition.m_Position.y;
            course.m_Curve = NetUtils.StraightCurve(startPos, endPos, netGeometryData.m_Hanging);
          }
        }
        else
        {
          float2 t = new float2(course.m_StartPosition.m_CourseDelta, course.m_EndPosition.m_CourseDelta);
          float2 float2 = math.lerp((float2) t.x, (float2) t.y, new float2(0.333333343f, 0.6666667f));
          course.m_Curve = MathUtils.Cut(course.m_Curve, t);
          if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StrictNodes) != (Game.Net.GeometryFlags) 0)
          {
            course.m_Curve.a = course.m_StartPosition.m_Position;
            course.m_Curve.d = course.m_EndPosition.m_Position;
          }
          else
          {
            course.m_Curve.a.y = course.m_StartPosition.m_Position.y;
            course.m_Curve.d.y = course.m_EndPosition.m_Position.y;
          }
          bool forceElevated3;
          // ISSUE: reference to a compiler-generated method
          course.m_Curve.b.y = this.SampleHeight(float2.x, out forceElevated3);
          bool forceElevated4;
          // ISSUE: reference to a compiler-generated method
          course.m_Curve.c.y = this.SampleHeight(float2.y, out forceElevated4);
          if (forceElevated3 | forceElevated4)
          {
            course.m_StartPosition.m_Flags |= CoursePosFlags.ForceElevatedEdge;
            course.m_EndPosition.m_Flags |= CoursePosFlags.ForceElevatedEdge;
          }
          float num3 = course.m_Curve.b.y - MathUtils.Position(course.m_Curve, 0.333333343f).y;
          float num4 = course.m_Curve.c.y - MathUtils.Position(course.m_Curve, 0.6666667f).y;
          course.m_Curve.b.y += (float) ((double) num3 * 3.0 - (double) num4 * 1.5);
          course.m_Curve.c.y += (float) ((double) num4 * 3.0 - (double) num3 * 1.5);
        }
        course.m_StartPosition.m_CourseDelta = 0.0f;
        course.m_EndPosition.m_CourseDelta = 1f;
        course.m_Length = MathUtils.Length(course.m_Curve);
      }

      public float SampleHeight(float courseDelta, out bool forceElevated)
      {
        // ISSUE: reference to a compiler-generated field
        if ((double) courseDelta == (double) this.m_SampleRange.x)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[0];
          // ISSUE: reference to a compiler-generated field
          forceElevated = courseHeightItem.m_ForceElevated;
          // ISSUE: reference to a compiler-generated field
          return courseHeightItem.m_CourseHeight;
        }
        // ISSUE: reference to a compiler-generated field
        if ((double) courseDelta == (double) this.m_SampleRange.y)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[this.m_Buffer.Length - 1];
          // ISSUE: reference to a compiler-generated field
          forceElevated = courseHeightItem.m_ForceElevated;
          // ISSUE: reference to a compiler-generated field
          return courseHeightItem.m_CourseHeight;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float f = (courseDelta - this.m_SampleRange.x) * this.m_SampleFactor;
        // ISSUE: reference to a compiler-generated field
        int index = math.clamp(Mathf.FloorToInt(f), 0, this.m_Buffer.Length - 1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.CourseHeightItem courseHeightItem1 = this.m_Buffer[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.CourseHeightItem courseHeightItem2 = this.m_Buffer[math.min(index + 1, this.m_Buffer.Length - 1)];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        forceElevated = courseHeightItem1.m_ForceElevated | courseHeightItem2.m_ForceElevated;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.lerp(courseHeightItem1.m_CourseHeight, courseHeightItem2.m_CourseHeight, math.saturate(f - (float) index));
      }

      public void GetHeightRange(
        CourseSplitSystem.IntersectPos intersectPos,
        NetGeometryData netGeometryData,
        out Bounds1 minBounds,
        out Bounds1 maxBounds,
        out bool elevated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 float2_1 = (new float2(intersectPos.m_CourseIntersection.min, intersectPos.m_CourseIntersection.max) - this.m_SampleRange.x) * this.m_SampleFactor;
        int index1 = math.max(Mathf.FloorToInt(float2_1.x), 0);
        // ISSUE: reference to a compiler-generated field
        int index2 = math.min(Mathf.CeilToInt(float2_1.y), this.m_Buffer.Length - 1);
        minBounds = new Bounds1(1000000f, -1000000f);
        maxBounds = new Bounds1(1000000f, -1000000f);
        elevated = false;
        // ISSUE: reference to a compiler-generated field
        if (index1 < this.m_Buffer.Length)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem1 = this.m_Buffer[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem2 = this.m_Buffer[math.min(index1 + 1, this.m_Buffer.Length - 1)];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          minBounds = new Bounds1((float2) math.lerp(courseHeightItem1.m_CourseHeight, courseHeightItem2.m_CourseHeight, math.saturate(float2_1.x - (float) index1)));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          elevated |= (double) courseHeightItem1.m_CourseHeight > (double) courseHeightItem1.m_TerrainHeight | (double) courseHeightItem2.m_CourseHeight > (double) courseHeightItem2.m_TerrainHeight;
        }
        if (index2 >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem3 = this.m_Buffer[math.max(index2 - 1, 0)];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem4 = this.m_Buffer[index2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          maxBounds = new Bounds1((float2) math.lerp(courseHeightItem3.m_CourseHeight, courseHeightItem4.m_CourseHeight, math.saturate(float2_1.y - (float) (index2 - 1))));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          elevated |= (double) courseHeightItem3.m_CourseHeight > (double) courseHeightItem3.m_TerrainHeight | (double) courseHeightItem4.m_CourseHeight > (double) courseHeightItem4.m_TerrainHeight;
        }
        Bounds1 bounds1 = minBounds | maxBounds;
        for (int index3 = index1 + 1; index3 < index2; ++index3)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CourseHeightItem courseHeightItem = this.m_Buffer[index3];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) courseHeightItem.m_CourseHeight < (double) bounds1.min || (double) courseHeightItem.m_CourseHeight > (double) bounds1.max)
          {
            // ISSUE: reference to a compiler-generated field
            bounds1 |= courseHeightItem.m_CourseHeight;
            float num = ((float) index3 - float2_1.x) / math.max(1f, float2_1.y - float2_1.x);
            float2 float2_2 = math.saturate(new float2((float) (2.0 - 2.0 * (double) num), 2f * num));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float4 float4 = math.max((float4) 0.0f, new float4(minBounds.min - courseHeightItem.m_CourseHeight, courseHeightItem.m_CourseHeight - minBounds.max, maxBounds.min - courseHeightItem.m_CourseHeight, courseHeightItem.m_CourseHeight - maxBounds.max)) * float2_2.xxyy;
            minBounds.min -= float4.x;
            minBounds.max += float4.y;
            maxBounds.min -= float4.z;
            maxBounds.max += float4.w;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          elevated |= (double) courseHeightItem.m_CourseHeight > (double) courseHeightItem.m_TerrainHeight;
        }
        float num1 = math.select(netGeometryData.m_DefaultHeightRange.min, netGeometryData.m_ElevatedHeightRange.min, elevated);
        float num2 = math.select(netGeometryData.m_DefaultHeightRange.max, netGeometryData.m_ElevatedHeightRange.max, elevated);
        minBounds.min += num1;
        minBounds.max += num2;
        maxBounds.min += num1;
        maxBounds.max += num2;
      }

      public void Dispose() => this.m_Buffer.Dispose();
    }

    [BurstCompile]
    private struct CheckCourseIntersectionResultsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Fixed> m_FixedData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<LocalCurveCache> m_LocalCurveCacheData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> m_PrefabPlaceableData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public BufferLookup<FixedNetElement> m_PrefabFixedNetElements;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public NativeList<CourseSplitSystem.Course> m_CourseList;
      [ReadOnly]
      public NativeHashMap<Entity, bool> m_DeletedEntities;
      [ReadOnly]
      public NativeParallelQueue<CourseSplitSystem.IntersectPos>.Reader m_IntersectionQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.Course course = this.m_CourseList[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabGeometryData[course.m_CreationDefinition.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos intersectPos1 = new CourseSplitSystem.IntersectPos()
        {
          m_CoursePos = course.m_CourseData.m_StartPosition,
          m_CourseIntersection = new Bounds1(course.m_CourseData.m_StartPosition.m_CourseDelta, course.m_CourseData.m_StartPosition.m_CourseDelta),
          m_IntersectionHeightMin = new Bounds1(float.MaxValue, float.MinValue),
          m_IntersectionHeightMax = new Bounds1(float.MaxValue, float.MinValue),
          m_EdgeIntersection = new Bounds1(course.m_CourseData.m_StartPosition.m_SplitPosition, course.m_CourseData.m_StartPosition.m_SplitPosition),
          m_EdgeHeightRangeMin = new Bounds1(1000000f, -1000000f),
          m_EdgeHeightRangeMax = new Bounds1(1000000f, -1000000f),
          m_Priority = -1f
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        intersectPos1.m_IsNode = this.m_NodeData.HasComponent(intersectPos1.m_CoursePos.m_Entity);
        // ISSUE: reference to a compiler-generated field
        intersectPos1.m_IsStartEnd = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos intersectPos2 = new CourseSplitSystem.IntersectPos()
        {
          m_CoursePos = course.m_CourseData.m_EndPosition,
          m_CourseIntersection = new Bounds1(course.m_CourseData.m_EndPosition.m_CourseDelta, course.m_CourseData.m_EndPosition.m_CourseDelta),
          m_IntersectionHeightMin = new Bounds1(float.MaxValue, float.MinValue),
          m_IntersectionHeightMax = new Bounds1(float.MaxValue, float.MinValue),
          m_EdgeIntersection = new Bounds1(course.m_CourseData.m_EndPosition.m_SplitPosition, course.m_CourseData.m_EndPosition.m_SplitPosition),
          m_EdgeHeightRangeMin = new Bounds1(1000000f, -1000000f),
          m_EdgeHeightRangeMax = new Bounds1(1000000f, -1000000f),
          m_Priority = -1f
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        intersectPos2.m_IsNode = this.m_NodeData.HasComponent(intersectPos2.m_CoursePos.m_Entity);
        // ISSUE: reference to a compiler-generated field
        intersectPos2.m_IsStartEnd = true;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = course.m_CreationDefinition.m_Owner == Entity.Null && course.m_OwnerDefinition.m_Prefab == Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.CourseHeightData courseHeightData = new CourseSplitSystem.CourseHeightData(Allocator.Temp, course.m_CourseData, netGeometryData, flag, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData);
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          courseHeightData.InitializeCoursePos(ref intersectPos1.m_CoursePos);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          courseHeightData.InitializeCoursePos(ref intersectPos2.m_CoursePos);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        courseHeightData.ApplyLimitRange(intersectPos1, netGeometryData, new Bounds1((float2) intersectPos1.m_CoursePos.m_Position.y), new Bounds1((float2) intersectPos1.m_CoursePos.m_Position.y));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        courseHeightData.ApplyLimitRange(intersectPos2, netGeometryData, new Bounds1((float2) intersectPos2.m_CoursePos.m_Position.y), new Bounds1((float2) intersectPos2.m_CoursePos.m_Position.y));
        // ISSUE: reference to a compiler-generated method
        courseHeightData.StraightenElevation(intersectPos1, intersectPos2);
        NativeList<CourseSplitSystem.IntersectPos> nativeList1 = new NativeList<CourseSplitSystem.IntersectPos>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<CourseSplitSystem.IntersectPos> nativeList2 = new NativeList<CourseSplitSystem.IntersectPos>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        nativeList1.Add(in intersectPos1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NativeParallelQueue<CourseSplitSystem.IntersectPos>.Enumerator enumerator = this.m_IntersectionQueue.GetEnumerator(index % this.m_IntersectionQueue.HashRange);
        while (enumerator.MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          if (current.m_CourseIndex == index)
            nativeList1.Add(in current);
        }
        enumerator.Dispose();
        nativeList1.Add(in intersectPos2);
        if (nativeList1.Length >= 4)
          nativeList1.AsArray().GetSubArray(1, nativeList1.Length - 2).Sort<CourseSplitSystem.IntersectPos>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool canSplitOwnedEdges = this.m_PrefabBuildingExtensionData.HasComponent(course.m_OwnerDefinition.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool canBeSplitted = canSplitOwnedEdges || course.m_CreationDefinition.m_Owner == Entity.Null && course.m_OwnerDefinition.m_Prefab == Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.MergePositions(course.m_CourseData, course.m_CreationDefinition, course.m_OwnerDefinition, netGeometryData, ref courseHeightData, nativeList1, nativeList2, flag, canBeSplitted, canSplitOwnedEdges);
        // ISSUE: reference to a compiler-generated method
        courseHeightData.StraightenElevation(intersectPos1, intersectPos2);
        nativeList1.Clear();
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.IntersectPos intersectPos3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabFixedNetElements.HasBuffer(course.m_CreationDefinition.m_Prefab))
        {
          ref NativeList<CourseSplitSystem.IntersectPos> local1 = ref nativeList1;
          intersectPos3 = nativeList2[0];
          ref CourseSplitSystem.IntersectPos local2 = ref intersectPos3;
          local1.Add(in local2);
          if (nativeList2.Length >= 2)
          {
            ref NativeList<CourseSplitSystem.IntersectPos> local3 = ref nativeList1;
            intersectPos3 = nativeList2[nativeList2.Length - 1];
            ref CourseSplitSystem.IntersectPos local4 = ref intersectPos3;
            local3.Add(in local4);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckHeightRange(course.m_CourseData, netGeometryData, ref courseHeightData, flag, nativeList2, nativeList1);
          if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.SnapCoursePositions(course.m_CourseData, nativeList1);
          }
        }
        if (nativeList1.Length >= 3)
        {
          float num = (netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0 ? 36f : 1f;
          // ISSUE: reference to a compiler-generated field
          if (nativeList1[1].m_IsOptional)
          {
            intersectPos3 = nativeList1[0];
            // ISSUE: reference to a compiler-generated field
            float2 xz1 = intersectPos3.m_CoursePos.m_Position.xz;
            intersectPos3 = nativeList1[1];
            // ISSUE: reference to a compiler-generated field
            float2 xz2 = intersectPos3.m_CoursePos.m_Position.xz;
            if ((double) math.distancesq(xz1, xz2) < (double) num)
              nativeList1.RemoveAt(1);
          }
        }
        if (nativeList1.Length >= 3)
        {
          float num = (netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0 ? 36f : 1f;
          // ISSUE: reference to a compiler-generated field
          if (nativeList1[nativeList1.Length - 2].m_IsOptional)
          {
            intersectPos3 = nativeList1[nativeList1.Length - 1];
            // ISSUE: reference to a compiler-generated field
            float2 xz3 = intersectPos3.m_CoursePos.m_Position.xz;
            intersectPos3 = nativeList1[nativeList1.Length - 2];
            // ISSUE: reference to a compiler-generated field
            float2 xz4 = intersectPos3.m_CoursePos.m_Position.xz;
            if ((double) math.distancesq(xz3, xz4) < (double) num)
              nativeList1.RemoveAt(nativeList1.Length - 2);
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateCourses(course.m_CourseData, course.m_CreationDefinition, course.m_OwnerDefinition, course.m_UpgradedData, course.m_CourseEntity, index, nativeList1, ref courseHeightData);
        nativeList1.Dispose();
        nativeList2.Dispose();
        // ISSUE: reference to a compiler-generated method
        courseHeightData.Dispose();
      }

      private void SnapCoursePositions(
        NetCourse courseData,
        NativeList<CourseSplitSystem.IntersectPos> intersectionList)
      {
        for (int index = 1; index < intersectionList.Length - 1; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersection1 = intersectionList[index - 1];
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersection2 = intersectionList[index];
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersection3 = intersectionList[index + 1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float length = MathUtils.Snap(MathUtils.Length(courseData.m_Curve.xz, new Bounds1(intersection1.m_CoursePos.m_CourseDelta, intersection2.m_CoursePos.m_CourseDelta)), 4f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Bounds1 t = new Bounds1(intersection1.m_CoursePos.m_CourseDelta, intersection3.m_CoursePos.m_CourseDelta);
          MathUtils.ClampLength(courseData.m_Curve.xz, ref t, length);
          // ISSUE: reference to a compiler-generated field
          intersection2.m_CoursePos.m_CourseDelta = t.max;
          // ISSUE: reference to a compiler-generated field
          if (((uint) (int) math.round(length / 4f) & 1U) > 0U != (intersection1.m_CoursePos.m_Flags & CoursePosFlags.HalfAlign) > (CoursePosFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            intersection2.m_CoursePos.m_Flags |= CoursePosFlags.HalfAlign;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            intersection2.m_CoursePos.m_Flags &= ~CoursePosFlags.HalfAlign;
          }
          intersectionList[index] = intersection2;
        }
      }

      private bool IsUnder(
        NetCourse courseData,
        Bounds1 heightRangeMin,
        Bounds1 heightRangeMax,
        CourseSplitSystem.IntersectPos intersectPos,
        bool canAdjustHeight)
      {
        // ISSUE: reference to a compiler-generated field
        if (intersectPos.m_IsStartEnd)
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num = math.max(heightRangeMin.max - intersectPos.m_EdgeHeightRangeMin.min, heightRangeMax.max - intersectPos.m_EdgeHeightRangeMax.min);
        if ((double) num <= 0.0)
          return true;
        if (!canAdjustHeight)
          return false;
        // ISSUE: reference to a compiler-generated field
        float y1 = MathUtils.Position(courseData.m_Curve, intersectPos.m_CourseIntersection.min).y;
        // ISSUE: reference to a compiler-generated field
        float y2 = MathUtils.Position(courseData.m_Curve, intersectPos.m_CourseIntersection.max).y;
        // ISSUE: reference to a compiler-generated field
        Bounds1 bounds1_1 = intersectPos.m_IntersectionHeightMin;
        // ISSUE: reference to a compiler-generated field
        Bounds1 bounds1_2 = intersectPos.m_IntersectionHeightMax;
        if ((double) bounds1_1.max < (double) bounds1_1.min)
        {
          // ISSUE: reference to a compiler-generated field
          bounds1_1 = new Bounds1((float2) intersectPos.m_CoursePos.m_Position.y);
        }
        if ((double) bounds1_2.max < (double) bounds1_2.min)
        {
          // ISSUE: reference to a compiler-generated field
          bounds1_2 = new Bounds1((float2) intersectPos.m_CoursePos.m_Position.y);
        }
        return (double) num < (double) bounds1_1.min - (double) y1 && (double) num < (double) bounds1_2.min - (double) y2;
      }

      private bool IsOver(
        NetCourse courseData,
        Bounds1 heightRangeMin,
        Bounds1 heightRangeMax,
        CourseSplitSystem.IntersectPos intersectPos,
        bool canAdjustHeight)
      {
        // ISSUE: reference to a compiler-generated field
        if (intersectPos.m_IsStartEnd)
          return false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num = math.max(intersectPos.m_EdgeHeightRangeMin.max - heightRangeMin.min, intersectPos.m_EdgeHeightRangeMax.max - heightRangeMax.min);
        if ((double) num <= 0.0)
          return true;
        if (!canAdjustHeight)
          return false;
        // ISSUE: reference to a compiler-generated field
        float y1 = MathUtils.Position(courseData.m_Curve, intersectPos.m_CourseIntersection.min).y;
        // ISSUE: reference to a compiler-generated field
        float y2 = MathUtils.Position(courseData.m_Curve, intersectPos.m_CourseIntersection.max).y;
        // ISSUE: reference to a compiler-generated field
        Bounds1 bounds1_1 = intersectPos.m_IntersectionHeightMin;
        // ISSUE: reference to a compiler-generated field
        Bounds1 bounds1_2 = intersectPos.m_IntersectionHeightMax;
        if ((double) bounds1_1.max < (double) bounds1_1.min)
        {
          // ISSUE: reference to a compiler-generated field
          bounds1_1 = new Bounds1((float2) intersectPos.m_CoursePos.m_Position.y);
        }
        if ((double) bounds1_2.max < (double) bounds1_2.min)
        {
          // ISSUE: reference to a compiler-generated field
          bounds1_2 = new Bounds1((float2) intersectPos.m_CoursePos.m_Position.y);
        }
        return (double) num < (double) y1 - (double) bounds1_1.max && (double) num < (double) y2 - (double) bounds1_2.max;
      }

      private void MergePositions(
        NetCourse courseData,
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        NetGeometryData prefabGeometryData,
        ref CourseSplitSystem.CourseHeightData courseHeightData,
        NativeList<CourseSplitSystem.IntersectPos> source,
        NativeList<CourseSplitSystem.IntersectPos> target,
        bool canAdjustHeight,
        bool canBeSplitted,
        bool canSplitOwnedEdges)
      {
        // ISSUE: reference to a compiler-generated field
        NetData prefabNetData1 = this.m_PrefabNetData[creationDefinition.m_Prefab];
        int index = 0;
        int num1 = 0;
        while (index < source.Length)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos1 = source[index++];
          Bounds1 minBounds1;
          Bounds1 maxBounds1;
          bool elevated;
          // ISSUE: reference to a compiler-generated method
          courseHeightData.GetHeightRange(intersectPos1, prefabGeometryData, out minBounds1, out maxBounds1, out elevated);
          // ISSUE: reference to a compiler-generated method
          if (this.IsUnder(courseData, minBounds1, maxBounds1, intersectPos1, canAdjustHeight))
          {
            if (canAdjustHeight)
            {
              float num2 = math.select(prefabGeometryData.m_DefaultHeightRange.max, prefabGeometryData.m_ElevatedHeightRange.max, elevated) + 0.5f;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              courseHeightData.ApplyLimitRange(intersectPos1, prefabGeometryData, new Bounds1(-1000000f, intersectPos1.m_EdgeHeightRangeMin.min - num2), new Bounds1(-1000000f, intersectPos1.m_EdgeHeightRangeMax.min - num2));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            bool flag1 = !this.IsOver(courseData, minBounds1, maxBounds1, intersectPos1, canAdjustHeight);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            bool flag2 = flag1 && this.CanConnect(creationDefinition, ownerDefinition, prefabNetData1, intersectPos1.m_IsNode, canSplitOwnedEdges, intersectPos1.m_CoursePos.m_Entity);
            int num3 = index;
            // ISSUE: reference to a compiler-generated field
            num1 += math.select(0, 1, intersectPos1.m_IsStartEnd);
            while (index < source.Length)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.IntersectPos intersectPos2 = source[index];
              Bounds1 minBounds2;
              Bounds1 maxBounds2;
              // ISSUE: reference to a compiler-generated method
              courseHeightData.GetHeightRange(intersectPos2, prefabGeometryData, out minBounds2, out maxBounds2, out elevated);
              bool flag3 = flag2;
              // ISSUE: reference to a compiler-generated method
              if (this.IsUnder(courseData, minBounds2, maxBounds2, intersectPos2, canAdjustHeight))
              {
                if (canAdjustHeight)
                {
                  float num4 = math.select(prefabGeometryData.m_DefaultHeightRange.max, prefabGeometryData.m_ElevatedHeightRange.max, elevated) + 0.5f;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  courseHeightData.ApplyLimitRange(intersectPos2, prefabGeometryData, new Bounds1(-1000000f, intersectPos2.m_EdgeHeightRangeMin.min - num4), new Bounds1(-1000000f, intersectPos2.m_EdgeHeightRangeMax.min - num4));
                }
                if (index > num3)
                {
                  source.RemoveAt(index);
                }
                else
                {
                  ++index;
                  ++num3;
                }
              }
              else
              {
                Bounds1 intersection;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((!intersectPos1.m_IsStartEnd || !intersectPos2.m_IsStartEnd || (creationDefinition.m_Flags & CreationFlags.SubElevation) != (CreationFlags) 0 || (intersectPos1.m_CoursePos.m_Flags & intersectPos2.m_CoursePos.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) == (CoursePosFlags.IsFirst | CoursePosFlags.IsLast)) && MathUtils.Intersect(intersectPos2.m_CourseIntersection, intersectPos1.m_CourseIntersection, out intersection))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (intersectPos2.m_CoursePos.m_Entity != intersectPos1.m_CoursePos.m_Entity)
                  {
                    // ISSUE: reference to a compiler-generated method
                    bool flag4 = !this.IsOver(courseData, minBounds2, maxBounds2, intersectPos2, canAdjustHeight);
                    if (flag1 | flag4)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      bool flag5 = flag4 && this.CanConnect(creationDefinition, ownerDefinition, prefabNetData1, intersectPos1.m_IsNode | intersectPos2.m_IsNode, canSplitOwnedEdges, intersectPos2.m_CoursePos.m_Entity);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (!flag2 & flag5 && intersectPos2.m_IsNode && !intersectPos1.m_IsNode)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        flag3 = flag1 && this.CanConnect(creationDefinition, ownerDefinition, prefabNetData1, true, canSplitOwnedEdges, intersectPos1.m_CoursePos.m_Entity);
                      }
                      if (flag3 != flag5)
                      {
                        ++index;
                        continue;
                      }
                      if (flag3 && (double) intersection.min > 0.0 && (double) intersection.max < 1.0)
                      {
                        ++index;
                        continue;
                      }
                    }
                  }
                  flag2 = flag3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (intersectPos1.m_IsNode && !intersectPos2.m_IsNode)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos2.m_CoursePos.m_Entity = intersectPos1.m_CoursePos.m_Entity;
                    // ISSUE: reference to a compiler-generated field
                    intersectPos2.m_IsNode = true;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (!intersectPos1.m_IsNode && intersectPos2.m_IsNode)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_CoursePos.m_Entity = intersectPos2.m_CoursePos.m_Entity;
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_IsNode = true;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (intersectPos2.m_CoursePos.m_Entity == Entity.Null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos2.m_CoursePos.m_Entity = intersectPos1.m_CoursePos.m_Entity;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos2.m_CoursePos.m_SplitPosition = intersectPos1.m_CoursePos.m_SplitPosition;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (intersectPos1.m_CoursePos.m_Entity == Entity.Null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_CoursePos.m_Entity = intersectPos2.m_CoursePos.m_Entity;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_CoursePos.m_SplitPosition = intersectPos2.m_CoursePos.m_SplitPosition;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) intersectPos2.m_CourseIntersection.min < (double) intersectPos1.m_CourseIntersection.min)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_CourseIntersection.min = intersectPos2.m_CourseIntersection.min;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_IntersectionHeightMin = intersectPos2.m_IntersectionHeightMin;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_EdgeHeightRangeMin = intersectPos2.m_EdgeHeightRangeMin;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((double) intersectPos2.m_CourseIntersection.min == (double) intersectPos1.m_CourseIntersection.min)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_IntersectionHeightMin |= intersectPos2.m_IntersectionHeightMin;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_EdgeHeightRangeMin |= intersectPos2.m_EdgeHeightRangeMin;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) intersectPos2.m_CourseIntersection.max > (double) intersectPos1.m_CourseIntersection.max)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_CourseIntersection.max = intersectPos2.m_CourseIntersection.max;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_IntersectionHeightMax = intersectPos2.m_IntersectionHeightMax;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_EdgeHeightRangeMax = intersectPos2.m_EdgeHeightRangeMax;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((double) intersectPos2.m_CourseIntersection.max == (double) intersectPos1.m_CourseIntersection.max)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_IntersectionHeightMax |= intersectPos2.m_IntersectionHeightMax;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      intersectPos1.m_EdgeHeightRangeMax |= intersectPos2.m_EdgeHeightRangeMax;
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  num1 += math.select(0, 1, intersectPos2.m_IsStartEnd);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos1.m_IsStartEnd |= intersectPos2.m_IsStartEnd;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos1.m_EdgeIntersection |= intersectPos2.m_EdgeIntersection;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos1.m_IsTunnel &= intersectPos2.m_IsTunnel;
                  // ISSUE: reference to a compiler-generated field
                  CoursePosFlags flags1 = intersectPos1.m_CoursePos.m_Flags;
                  // ISSUE: reference to a compiler-generated field
                  CoursePosFlags flags2 = intersectPos2.m_CoursePos.m_Flags;
                  // ISSUE: reference to a compiler-generated field
                  intersectPos1.m_CoursePos.m_Flags |= flags2 & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast);
                  // ISSUE: reference to a compiler-generated field
                  intersectPos1.m_CoursePos.m_Flags &= flags2 | ~CoursePosFlags.FreeHeight;
                  // ISSUE: reference to a compiler-generated field
                  intersectPos2.m_CoursePos.m_Flags |= flags1 & (CoursePosFlags.IsFirst | CoursePosFlags.IsLast);
                  // ISSUE: reference to a compiler-generated field
                  intersectPos2.m_CoursePos.m_Flags &= flags1 | ~CoursePosFlags.FreeHeight;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) intersectPos2.m_Priority < (double) intersectPos1.m_Priority)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_CoursePos = intersectPos2.m_CoursePos;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos1.m_Priority = intersectPos2.m_Priority;
                  }
                  if (index > num3)
                  {
                    source.RemoveAt(index);
                  }
                  else
                  {
                    ++index;
                    ++num3;
                  }
                }
                else
                  break;
              }
            }
            if (flag2)
            {
              // ISSUE: reference to a compiler-generated field
              Bounds1 limitRangeMin = intersectPos1.m_IntersectionHeightMin;
              // ISSUE: reference to a compiler-generated field
              Bounds1 limitRangeMax = intersectPos1.m_IntersectionHeightMax;
              if ((double) limitRangeMin.max < (double) limitRangeMin.min)
              {
                // ISSUE: reference to a compiler-generated field
                limitRangeMin = new Bounds1((float2) intersectPos1.m_CoursePos.m_Position.y);
              }
              if ((double) limitRangeMax.max < (double) limitRangeMax.min)
              {
                // ISSUE: reference to a compiler-generated field
                limitRangeMax = new Bounds1((float2) intersectPos1.m_CoursePos.m_Position.y);
              }
              // ISSUE: reference to a compiler-generated method
              courseHeightData.ApplyLimitRange(intersectPos1, prefabGeometryData, limitRangeMin, limitRangeMax, true);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Entity = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              elevated |= !intersectPos1.m_IsTunnel;
              if (canAdjustHeight)
              {
                float num5 = math.select(prefabGeometryData.m_DefaultHeightRange.min, prefabGeometryData.m_ElevatedHeightRange.min, elevated) - 0.5f;
                // ISSUE: reference to a compiler-generated field
                bool forceElevated = !intersectPos1.m_IsTunnel && (prefabGeometryData.m_Flags & Game.Net.GeometryFlags.ExclusiveGround) != 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                courseHeightData.ApplyLimitRange(intersectPos1, prefabGeometryData, new Bounds1(intersectPos1.m_EdgeHeightRangeMin.max - num5, 1000000f), new Bounds1(intersectPos1.m_EdgeHeightRangeMax.max - num5, 1000000f), forceElevated: forceElevated);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (intersectPos1.m_IsTunnel && !intersectPos1.m_IsStartEnd)
              {
                index = num3;
                continue;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_IsOptional = !intersectPos1.m_IsStartEnd;
            }
            // ISSUE: reference to a compiler-generated field
            if (intersectPos1.m_IsStartEnd)
              target.Add(in intersectPos1);
            else if (canBeSplitted)
            {
              if (num1 >= 2)
              {
                if (target.Length >= 2)
                  CollectionUtils.Insert<CourseSplitSystem.IntersectPos>(target, target.Length - 1, intersectPos1);
              }
              else
                target.Add(in intersectPos1);
            }
            index = num3;
          }
        }
      }

      private bool CanConnect(
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        NetData prefabNetData1,
        bool isNode,
        bool canSplitOwnedEdges,
        Entity entity2)
      {
        if (entity2 == Entity.Null)
          return true;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity2];
        // ISSUE: reference to a compiler-generated field
        NetData netData2 = this.m_PrefabNetData[prefabRef.m_Prefab];
        Game.Net.Elevation componentData1;
        NetGeometryData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (this.m_EditorMode || (canSplitOwnedEdges || isNode) && ((prefabNetData1.m_RequiredLayers & (Layer.MarkerPathway | Layer.MarkerTaxiway)) != Layer.None || (netData2.m_RequiredLayers & (Layer.MarkerPathway | Layer.MarkerTaxiway)) == Layer.None || !(creationDefinition.m_Owner == Entity.Null) || !(ownerDefinition.m_Prefab == Entity.Null)) || !this.m_OwnerData.HasComponent(entity2)) && (isNode || this.m_PrefabData.IsComponentEnabled(prefabRef.m_Prefab)) && NetUtils.CanConnect(prefabNetData1, netData2) && (((prefabNetData1.m_RequiredLayers ^ netData2.m_RequiredLayers) & Layer.TrainTrack) == Layer.None || !this.m_ElevationData.TryGetComponent(entity2, out componentData1) || !this.m_PrefabGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2) || !math.any(math.abs(componentData1.m_Elevation) >= componentData2.m_ElevationLimit));
      }

      private void GetElevationRanges(
        NetCourse courseData,
        NetGeometryData prefabGeometryData,
        ref CourseSplitSystem.CourseHeightData courseHeightData,
        Bounds1 courseDelta,
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> leftSegments,
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> rightSegments)
      {
        double num1 = (double) MathUtils.Length(courseData.m_Curve.xz, courseDelta);
        float num2 = prefabGeometryData.m_DefaultWidth * 0.5f;
        int num3 = Mathf.RoundToInt((float) (num1 / 4.0));
        leftSegments.Clear();
        rightSegments.Clear();
        if (num3 < 1)
          return;
        float3 float3_1 = MathUtils.Position(courseData.m_Curve, courseDelta.min);
        float2 min = (float2) courseDelta.min;
        float2 float2 = (float2) 0.0f;
        int2 int2_1 = (int2) 0;
        bool2 bool2 = (bool2) true;
        float _max1 = courseDelta.min;
        float _max2 = 0.0f;
        float num4 = 1f / (float) num3;
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment;
        for (int index = 1; index <= num3; ++index)
        {
          float t = math.lerp(courseDelta.min, courseDelta.max, (float) index * num4);
          float num5 = math.lerp(courseDelta.min, courseDelta.max, ((float) index - 0.5f) * num4);
          float3 float3_2 = MathUtils.Position(courseData.m_Curve, t);
          bool forceElevated;
          // ISSUE: reference to a compiler-generated method
          float3 float3_3 = MathUtils.Position(courseData.m_Curve, num5) with
          {
            y = courseHeightData.SampleHeight(num5, out forceElevated)
          };
          float3 float3_4 = new float3();
          float3_4.xz = math.normalizesafe(MathUtils.Right(MathUtils.Tangent(courseData.m_Curve.xz, num5))) * num2;
          int2 int2_2;
          bool flag;
          if (forceElevated)
          {
            int2_2 = (int2) 2;
            flag = false;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            int2_2.x = this.GetElevationType(prefabGeometryData, float3_3 - float3_4);
            // ISSUE: reference to a compiler-generated method
            int2_2.y = this.GetElevationType(prefabGeometryData, float3_3 + float3_4);
            flag = true;
          }
          if (index != 1)
          {
            if (int2_2.x != int2_1.x)
            {
              ref NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> local1 = ref leftSegments;
              // ISSUE: object of a compiler-generated type is created
              elevationSegment = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment();
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_CourseRange = new Bounds1(min.x, _max1);
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_DistanceOffset = new Bounds1(float2.x, _max2);
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_ElevationType = (int2) int2_1.x;
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_CanRemove = bool2.x;
              ref CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment local2 = ref elevationSegment;
              local1.Add(in local2);
              bool2.x = true;
              min.x = _max1;
              float2.x = _max2;
            }
            if (int2_2.y != int2_1.y)
            {
              ref NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> local3 = ref rightSegments;
              // ISSUE: object of a compiler-generated type is created
              elevationSegment = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment();
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_CourseRange = new Bounds1(min.y, _max1);
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_DistanceOffset = new Bounds1(float2.y, _max2);
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_ElevationType = (int2) int2_1.y;
              // ISSUE: reference to a compiler-generated field
              elevationSegment.m_CanRemove = bool2.y;
              ref CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment local4 = ref elevationSegment;
              local3.Add(in local4);
              bool2.y = true;
              min.y = _max1;
              float2.y = _max2;
            }
          }
          _max1 = t;
          _max2 += math.distance(float3_1.xz, float3_2.xz);
          int2_1 = int2_2;
          bool2 &= flag;
          float3_1 = float3_2;
        }
        ref NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> local5 = ref leftSegments;
        // ISSUE: object of a compiler-generated type is created
        elevationSegment = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment();
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_CourseRange = new Bounds1(min.x, _max1);
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_DistanceOffset = new Bounds1(float2.x, _max2);
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_ElevationType = (int2) int2_1.x;
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_CanRemove = bool2.x;
        ref CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment local6 = ref elevationSegment;
        local5.Add(in local6);
        ref NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> local7 = ref rightSegments;
        // ISSUE: object of a compiler-generated type is created
        elevationSegment = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment();
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_CourseRange = new Bounds1(min.y, _max1);
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_DistanceOffset = new Bounds1(float2.y, _max2);
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_ElevationType = (int2) int2_1.y;
        // ISSUE: reference to a compiler-generated field
        elevationSegment.m_CanRemove = bool2.y;
        ref CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment local8 = ref elevationSegment;
        local7.Add(in local8);
      }

      private int GetElevationType(NetGeometryData prefabGeometryData, float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        float num = position.y - TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, position);
        float3 float3 = new float3(prefabGeometryData.m_ElevationLimit * 0.5f, prefabGeometryData.m_ElevationLimit, prefabGeometryData.m_ElevationLimit * 3f);
        int2 x1 = math.select((int2) 0, new int2(1), num >= float3.xy);
        int3 x2 = math.select((int3) 0, new int3(-1), num <= -float3);
        return math.csum(x1) + math.csum(x2);
      }

      private void ExpandMajorElevationSegments(
        NetCourse courseData,
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> elevationSegments)
      {
        float x = 8f;
        for (int index = 0; index < elevationSegments.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment1 = elevationSegments[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (math.all(elevationSegment1.m_ElevationType == 1) || math.all(elevationSegment1.m_ElevationType == -1))
          {
            // ISSUE: reference to a compiler-generated field
            float length = math.min(x, MathUtils.Size(elevationSegment1.m_DistanceOffset) * 0.5f);
            // ISSUE: reference to a compiler-generated field
            int num = math.select(2, -2, elevationSegment1.m_ElevationType.x == -1);
            if (index > 0)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment2 = elevationSegments[index - 1];
              // ISSUE: reference to a compiler-generated field
              if (math.all(elevationSegment2.m_ElevationType == num))
              {
                // ISSUE: reference to a compiler-generated field
                Bounds1 courseRange = elevationSegment1.m_CourseRange;
                MathUtils.ClampLength(courseData.m_Curve.xz, ref courseRange, length);
                // ISSUE: reference to a compiler-generated field
                elevationSegment1.m_CourseRange.min = courseRange.max;
                // ISSUE: reference to a compiler-generated field
                elevationSegment1.m_DistanceOffset.min += length;
                // ISSUE: reference to a compiler-generated field
                elevationSegment2.m_CourseRange.max = courseRange.max;
                // ISSUE: reference to a compiler-generated field
                elevationSegment2.m_DistanceOffset.max += length;
                elevationSegments[index - 1] = elevationSegment2;
              }
            }
            if (index < elevationSegments.Length - 1)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment3 = elevationSegments[index + 1];
              // ISSUE: reference to a compiler-generated field
              if (math.all(elevationSegment3.m_ElevationType == num))
              {
                // ISSUE: reference to a compiler-generated field
                Bounds1 courseRange = elevationSegment1.m_CourseRange;
                MathUtils.ClampLengthInverse(courseData.m_Curve.xz, ref courseRange, length);
                // ISSUE: reference to a compiler-generated field
                elevationSegment1.m_CourseRange.max = courseRange.min;
                // ISSUE: reference to a compiler-generated field
                elevationSegment1.m_DistanceOffset.max -= length;
                // ISSUE: reference to a compiler-generated field
                elevationSegment3.m_CourseRange.min = courseRange.min;
                // ISSUE: reference to a compiler-generated field
                elevationSegment3.m_DistanceOffset.min -= length;
                elevationSegments[index + 1] = elevationSegment3;
              }
            }
            // ISSUE: reference to a compiler-generated field
            elevationSegment1.m_ElevationType = (int2) 0;
            elevationSegments[index] = elevationSegment1;
          }
        }
      }

      private void MergeSimilarElevationSegments(
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> elevationSegments)
      {
        int index1 = 0;
        int index2 = 0;
        while (index1 < elevationSegments.Length)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment1;
          for (elevationSegment1 = elevationSegments[index1++]; index1 < elevationSegments.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment2 = elevationSegments[index1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!math.any(elevationSegment2.m_ElevationType != elevationSegment1.m_ElevationType))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment1.m_CourseRange.max = elevationSegment2.m_CourseRange.max;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment1.m_DistanceOffset.max = elevationSegment2.m_DistanceOffset.max;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment1.m_CanRemove &= elevationSegment2.m_CanRemove;
            }
            else
              break;
          }
          elevationSegments[index2++] = elevationSegment1;
        }
        if (index2 >= index1)
          return;
        elevationSegments.RemoveRange(index2, index1 - index2);
      }

      private void RemoveShortElevationSegments(
        NetGeometryData prefabGeometryData,
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> elevationSegments)
      {
        float defaultWidth = prefabGeometryData.m_DefaultWidth;
        for (int index1 = 0; index1 < elevationSegments.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment1 = elevationSegments[index1];
          // ISSUE: reference to a compiler-generated field
          float num1 = MathUtils.Size(elevationSegment1.m_DistanceOffset);
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 < (double) defaultWidth && elevationSegment1.m_CanRemove)
          {
            int index2 = index1--;
            for (int index3 = index2 + 1; index3 < elevationSegments.Length; ++index3)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment2 = elevationSegments[index3];
              // ISSUE: reference to a compiler-generated field
              float num2 = MathUtils.Size(elevationSegment2.m_DistanceOffset);
              // ISSUE: reference to a compiler-generated field
              if ((double) num2 < (double) num1 && elevationSegment2.m_CanRemove)
              {
                num1 = num2;
                index2 = index3;
              }
              else
                break;
            }
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment3 = elevationSegments[index2];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment4 = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment()
            {
              m_ElevationType = (int2) -1000000
            };
            if (index2 > 0)
            {
              elevationSegment4 = elevationSegments[index2 - 1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment4.m_CourseRange.max = MathUtils.Center(elevationSegment3.m_CourseRange);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment4.m_DistanceOffset.max = MathUtils.Center(elevationSegment3.m_DistanceOffset);
              elevationSegments[index2 - 1] = elevationSegment4;
            }
            if (index2 < elevationSegments.Length - 1)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment5 = elevationSegments[index2 + 1];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (math.all(elevationSegment5.m_ElevationType == elevationSegment4.m_ElevationType))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                elevationSegment4.m_CourseRange.max = elevationSegment5.m_CourseRange.max;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                elevationSegment4.m_DistanceOffset.max = elevationSegment5.m_DistanceOffset.max;
                elevationSegments[index2 - 1] = elevationSegment4;
                elevationSegments.RemoveAt(index2 + 1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                elevationSegment5.m_CourseRange.min = MathUtils.Center(elevationSegment3.m_CourseRange);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                elevationSegment5.m_DistanceOffset.min = MathUtils.Center(elevationSegment3.m_DistanceOffset);
                elevationSegments[index2 + 1] = elevationSegment5;
              }
            }
            elevationSegments.RemoveAt(index2);
          }
        }
      }

      private void MergeSideElevationSegments(
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> leftSegments,
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> rightSegments)
      {
        int length1 = rightSegments.Length;
        rightSegments.AddRange(leftSegments.AsArray());
        int length2 = rightSegments.Length;
        int index1 = length1;
        int index2 = 0;
        leftSegments.Clear();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment1 = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment()
        {
          m_ElevationType = (int2) -1000000
        };
        bool2 bool2 = (bool2) true;
        while (true)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment2;
          // ISSUE: reference to a compiler-generated field
          do
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment3 = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment();
            // ISSUE: reference to a compiler-generated field
            elevationSegment3.m_ElevationType = (int2) -1000000;
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment4 = elevationSegment3;
            // ISSUE: object of a compiler-generated type is created
            elevationSegment3 = new CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment();
            // ISSUE: reference to a compiler-generated field
            elevationSegment3.m_ElevationType = (int2) -1000000;
            elevationSegment2 = elevationSegment3;
            if (index1 < length2 && index2 < length1)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment rightSegment1 = rightSegments[index1];
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment rightSegment2 = rightSegments[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if ((double) elevationSegment4.m_CourseRange.min <= (double) elevationSegment2.m_CourseRange.min)
              {
                elevationSegment4 = rightSegment1;
                ++index1;
              }
              else
              {
                elevationSegment2 = rightSegment2;
                ++index2;
              }
            }
            else if (index1 < length2)
              elevationSegment4 = rightSegments[index1++];
            else if (index2 < length1)
            {
              elevationSegment2 = rightSegments[index2++];
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (math.all(elevationSegment1.m_ElevationType != -1000000))
              {
                leftSegments.Add(in elevationSegment1);
                return;
              }
              goto label_19;
            }
            // ISSUE: reference to a compiler-generated field
            if (elevationSegment4.m_ElevationType.x != -1000000)
            {
              // ISSUE: reference to a compiler-generated field
              if (math.all(elevationSegment1.m_ElevationType != -1000000))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                elevationSegment1.m_CourseRange.max = elevationSegment4.m_CourseRange.min;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                elevationSegment1.m_DistanceOffset.max = elevationSegment4.m_DistanceOffset.min;
                leftSegments.Add(in elevationSegment1);
                bool2.x = true;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment1.m_CourseRange = elevationSegment4.m_CourseRange;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment1.m_DistanceOffset = elevationSegment4.m_DistanceOffset;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              elevationSegment1.m_ElevationType.x = elevationSegment4.m_ElevationType.x;
              // ISSUE: reference to a compiler-generated field
              bool2 &= elevationSegment4.m_CanRemove;
            }
          }
          while (elevationSegment2.m_ElevationType.y == -1000000);
          // ISSUE: reference to a compiler-generated field
          if (math.all(elevationSegment1.m_ElevationType != -1000000))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            elevationSegment1.m_CourseRange.max = elevationSegment2.m_CourseRange.min;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            elevationSegment1.m_DistanceOffset.max = elevationSegment2.m_DistanceOffset.min;
            leftSegments.Add(in elevationSegment1);
            bool2.y = true;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          elevationSegment1.m_CourseRange = elevationSegment2.m_CourseRange;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          elevationSegment1.m_DistanceOffset = elevationSegment2.m_DistanceOffset;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          elevationSegment1.m_ElevationType.y = elevationSegment2.m_ElevationType.y;
          // ISSUE: reference to a compiler-generated field
          bool2 &= elevationSegment2.m_CanRemove;
        }
label_19:;
      }

      private void CheckHeightRange(
        NetCourse courseData,
        NetGeometryData prefabGeometryData,
        ref CourseSplitSystem.CourseHeightData courseHeightData,
        bool sampleTerrain,
        NativeList<CourseSplitSystem.IntersectPos> source,
        NativeList<CourseSplitSystem.IntersectPos> target)
      {
        bool flag = false;
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> nativeList1 = new NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment> nativeList2 = new NativeList<CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < source.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos1 = source[index1];
          if (sampleTerrain && index1 != 0 && (prefabGeometryData.m_Flags & Game.Net.GeometryFlags.RequireElevated) == (Game.Net.GeometryFlags) 0)
          {
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.IntersectPos intersectPos2 = source[index1 - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Bounds1 courseDelta = new Bounds1(intersectPos2.m_CourseIntersection.max, intersectPos1.m_CourseIntersection.min);
            // ISSUE: reference to a compiler-generated method
            this.GetElevationRanges(courseData, prefabGeometryData, ref courseHeightData, courseDelta, nativeList1, nativeList2);
            // ISSUE: reference to a compiler-generated method
            this.ExpandMajorElevationSegments(courseData, nativeList1);
            // ISSUE: reference to a compiler-generated method
            this.ExpandMajorElevationSegments(courseData, nativeList2);
            // ISSUE: reference to a compiler-generated method
            this.MergeSimilarElevationSegments(nativeList1);
            // ISSUE: reference to a compiler-generated method
            this.MergeSimilarElevationSegments(nativeList2);
            // ISSUE: reference to a compiler-generated method
            this.RemoveShortElevationSegments(prefabGeometryData, nativeList1);
            // ISSUE: reference to a compiler-generated method
            this.RemoveShortElevationSegments(prefabGeometryData, nativeList2);
            // ISSUE: reference to a compiler-generated method
            this.MergeSideElevationSegments(nativeList1, nativeList2);
            // ISSUE: reference to a compiler-generated method
            this.RemoveShortElevationSegments(prefabGeometryData, nativeList1);
            for (int index2 = 1; index2 < nativeList1.Length; ++index2)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment1 = nativeList1[index2 - 1];
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.CheckCourseIntersectionResultsJob.ElevationSegment elevationSegment2 = nativeList1[index2];
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.IntersectPos intersectPos3 = new CourseSplitSystem.IntersectPos();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos3.m_CoursePos.m_CourseDelta = elevationSegment2.m_CourseRange.min;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos3.m_CoursePos.m_Flags = intersectPos1.m_CoursePos.m_Flags & (CoursePosFlags.IsParallel | CoursePosFlags.IsRight | CoursePosFlags.IsLeft | CoursePosFlags.IsGrid);
              // ISSUE: reference to a compiler-generated field
              intersectPos3.m_CoursePos.m_Flags |= CoursePosFlags.FreeHeight;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos3.m_CoursePos.m_ParentMesh = math.select(intersectPos1.m_CoursePos.m_ParentMesh, -1, intersectPos1.m_CoursePos.m_ParentMesh != intersectPos2.m_CoursePos.m_ParentMesh);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos3.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos3.m_CoursePos.m_CourseDelta);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos3.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos3.m_CoursePos.m_CourseDelta));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (elevationSegment2.m_ElevationType.x != elevationSegment1.m_ElevationType.x)
              {
                // ISSUE: reference to a compiler-generated field
                intersectPos3.m_CoursePos.m_Flags |= CoursePosFlags.LeftTransition;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (elevationSegment2.m_ElevationType.y != elevationSegment1.m_ElevationType.y)
              {
                // ISSUE: reference to a compiler-generated field
                intersectPos3.m_CoursePos.m_Flags |= CoursePosFlags.RightTransition;
              }
              if (flag)
              {
                int index3 = target.Length;
                for (int index4 = target.Length - 1; index4 >= 0; --index4)
                {
                  // ISSUE: variable of a compiler-generated type
                  CourseSplitSystem.IntersectPos intersectPos4 = target[index4];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (intersectPos4.m_IsOptional && (double) intersectPos4.m_CoursePos.m_CourseDelta > (double) intersectPos3.m_CoursePos.m_CourseDelta)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    Bounds1 t = new Bounds1(intersectPos3.m_CoursePos.m_CourseDelta, intersectPos4.m_CoursePos.m_CourseDelta);
                    float num = MathUtils.Length(courseData.m_Curve.xz, t);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos4.m_CanMove.max = math.max(intersectPos4.m_CanMove.max, prefabGeometryData.m_ElevatedLength * 0.95f - num);
                    target[index4] = intersectPos4;
                    index3 = index4;
                  }
                  else
                    break;
                }
                CollectionUtils.Insert<CourseSplitSystem.IntersectPos>(target, index3, intersectPos3);
              }
              else
                target.Add(in intersectPos3);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!intersectPos1.m_IsOptional && !this.m_FixedData.HasComponent(intersectPos1.m_CoursePos.m_Entity))
          {
            // ISSUE: reference to a compiler-generated field
            if (flag && !intersectPos1.m_IsStartEnd)
            {
              int index5 = target.Length;
              for (int index6 = target.Length - 1; index6 >= 0; --index6)
              {
                // ISSUE: variable of a compiler-generated type
                CourseSplitSystem.IntersectPos intersectPos5 = target[index6];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (intersectPos5.m_IsOptional && (double) intersectPos5.m_CoursePos.m_CourseDelta > (double) intersectPos1.m_CoursePos.m_CourseDelta)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Bounds1 t = new Bounds1(intersectPos1.m_CoursePos.m_CourseDelta, intersectPos5.m_CoursePos.m_CourseDelta);
                  float num = MathUtils.Length(courseData.m_Curve.xz, t);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos5.m_CanMove.max = math.max(intersectPos5.m_CanMove.max, prefabGeometryData.m_ElevatedLength * 0.95f - num);
                  target[index6] = intersectPos5;
                  index5 = index6;
                }
                else
                  break;
              }
              CollectionUtils.Insert<CourseSplitSystem.IntersectPos>(target, index5, intersectPos1);
            }
            else
              target.Add(in intersectPos1);
          }
          else
          {
            float num1 = prefabGeometryData.m_DefaultWidth * 0.5f;
            if ((prefabGeometryData.m_Flags & Game.Net.GeometryFlags.MiddlePillars) != (Game.Net.GeometryFlags) 0)
            {
              // ISSUE: reference to a compiler-generated field
              float num2 = MathUtils.Length(courseData.m_Curve.xz, intersectPos1.m_CourseIntersection);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Bounds1 t1 = new Bounds1(intersectPos1.m_CourseIntersection.min, intersectPos1.m_CourseIntersection.max);
              MathUtils.ClampLength(courseData.m_Curve.xz, ref t1, num2 * 0.5f);
              float num3 = num2 + num1 * 2f;
              float _max = math.max(0.0f, (float) (((double) prefabGeometryData.m_ElevatedLength * 0.949999988079071 - (double) num3) * 0.5));
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Entity = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_CourseDelta = math.min(1f, t1.max);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos1.m_CoursePos.m_CourseDelta);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos1.m_CoursePos.m_CourseDelta));
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CanMove = new Bounds1(-_max, _max);
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_IsOptional = true;
              int index7 = target.Length;
              for (int index8 = target.Length - 1; index8 >= 0; --index8)
              {
                // ISSUE: variable of a compiler-generated type
                CourseSplitSystem.IntersectPos intersectPos6 = target[index8];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!intersectPos6.m_IsOptional && !intersectPos6.m_IsStartEnd && (double) intersectPos6.m_CoursePos.m_CourseDelta > (double) intersectPos1.m_CoursePos.m_CourseDelta)
                  index7 = index8;
                else
                  break;
              }
              if (index7 < target.Length)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Bounds1 t2 = new Bounds1(intersectPos1.m_CoursePos.m_CourseDelta, target[index7].m_CoursePos.m_CourseDelta);
                float num4 = MathUtils.Length(courseData.m_Curve.xz, t2);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                intersectPos1.m_CanMove.min = math.min(intersectPos1.m_CanMove.min, num4 - prefabGeometryData.m_ElevatedLength * 0.95f);
              }
              CollectionUtils.Insert<CourseSplitSystem.IntersectPos>(target, index7, intersectPos1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Bounds1 t3 = new Bounds1(0.0f, intersectPos1.m_CourseIntersection.min);
              // ISSUE: reference to a compiler-generated field
              Bounds1 t4 = new Bounds1(intersectPos1.m_CourseIntersection.max, 1f);
              // ISSUE: reference to a compiler-generated field
              float num5 = MathUtils.Length(courseData.m_Curve.xz, intersectPos1.m_CourseIntersection) + num1 * 2f;
              float y = (float) (((double) prefabGeometryData.m_ElevatedLength * 0.949999988079071 - (double) num5) * 0.5);
              float length = math.max(0.0f, num1 + math.min(0.0f, y));
              float _max = math.max(0.0f, y);
              MathUtils.ClampLengthInverse(courseData.m_Curve.xz, ref t3, length);
              MathUtils.ClampLength(courseData.m_Curve.xz, ref t4, length);
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Entity = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_CourseDelta = math.max(0.0f, t3.min);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos1.m_CoursePos.m_CourseDelta);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos1.m_CoursePos.m_CourseDelta));
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CanMove = new Bounds1(-_max, 0.0f);
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_IsOptional = true;
              int index9 = target.Length;
              for (int index10 = target.Length - 1; index10 >= 0; --index10)
              {
                // ISSUE: variable of a compiler-generated type
                CourseSplitSystem.IntersectPos intersectPos7 = target[index10];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!intersectPos7.m_IsOptional && !intersectPos7.m_IsStartEnd && (double) intersectPos7.m_CoursePos.m_CourseDelta > (double) intersectPos1.m_CoursePos.m_CourseDelta)
                  index9 = index10;
                else
                  break;
              }
              if (index9 < target.Length)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Bounds1 t5 = new Bounds1(intersectPos1.m_CoursePos.m_CourseDelta, target[index9].m_CoursePos.m_CourseDelta);
                float num6 = MathUtils.Length(courseData.m_Curve.xz, t5);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                intersectPos1.m_CanMove.min = math.min(intersectPos1.m_CanMove.min, num6 - prefabGeometryData.m_ElevatedLength * 0.95f);
              }
              CollectionUtils.Insert<CourseSplitSystem.IntersectPos>(target, index9, intersectPos1);
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Entity = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_CourseDelta = math.min(1f, t4.max);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos1.m_CoursePos.m_CourseDelta);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos1.m_CoursePos.m_CourseDelta));
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_CanMove = new Bounds1(0.0f, _max);
              // ISSUE: reference to a compiler-generated field
              intersectPos1.m_IsOptional = true;
              target.Add(in intersectPos1);
            }
            flag = true;
          }
        }
        nativeList1.Dispose();
        nativeList2.Dispose();
        if (!flag)
          return;
        source.Clear();
        for (int index = 0; index < target.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos8 = target[index];
          // ISSUE: reference to a compiler-generated field
          if (intersectPos8.m_IsOptional)
          {
            for (; index + 1 < target.Length; ++index)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.IntersectPos intersectPos9 = target[index + 1];
              // ISSUE: reference to a compiler-generated field
              if (intersectPos9.m_IsOptional)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) intersectPos9.m_CoursePos.m_CourseDelta <= (double) intersectPos8.m_CoursePos.m_CourseDelta)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos8.m_CoursePos.m_CourseDelta = math.lerp(intersectPos8.m_CoursePos.m_CourseDelta, intersectPos9.m_CoursePos.m_CourseDelta, 0.5f);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos8.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos8.m_CoursePos.m_CourseDelta);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  intersectPos8.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos8.m_CoursePos.m_CourseDelta));
                  // ISSUE: reference to a compiler-generated field
                  intersectPos8.m_CanMove = new Bounds1(0.0f, 0.0f);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Bounds1 t = new Bounds1(intersectPos8.m_CoursePos.m_CourseDelta, intersectPos9.m_CoursePos.m_CourseDelta);
                  float num = MathUtils.Length(courseData.m_Curve.xz, t);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((double) intersectPos8.m_CanMove.max - (double) intersectPos9.m_CanMove.min >= (double) num)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float length = math.min(intersectPos8.m_CanMove.max, math.max(num * 0.5f, num + intersectPos9.m_CanMove.min));
                    MathUtils.ClampLength(courseData.m_Curve.xz, ref t, length);
                    // ISSUE: reference to a compiler-generated field
                    intersectPos8.m_CoursePos.m_CourseDelta = t.max;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos8.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos8.m_CoursePos.m_CourseDelta);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos8.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos8.m_CoursePos.m_CourseDelta));
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos8.m_CanMove.min = math.min(0.0f, math.max(intersectPos8.m_CanMove.min - length, intersectPos9.m_CanMove.min + num - length));
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    intersectPos8.m_CanMove.max = math.max(0.0f, math.min(intersectPos9.m_CanMove.max + num - length, intersectPos8.m_CanMove.max - length));
                  }
                  else
                    break;
                }
              }
              else
                break;
            }
          }
          source.Add(in intersectPos8);
        }
        target.Clear();
        for (int index = 1; index < source.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos10 = source[index - 1];
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos11 = source[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Bounds1 t = new Bounds1(intersectPos10.m_CoursePos.m_CourseDelta, intersectPos11.m_CoursePos.m_CourseDelta);
          // ISSUE: reference to a compiler-generated field
          intersectPos10.m_Priority = MathUtils.Length(courseData.m_Curve.xz, t);
          source[index - 1] = intersectPos10;
        }
        for (int index11 = 0; index11 < source.Length; ++index11)
        {
          int minIndex;
          int maxIndex;
          // ISSUE: reference to a compiler-generated method
          if (this.FindOptionalRange(source, index11, out minIndex, out maxIndex))
          {
            index11 = maxIndex;
            int bestIndex;
            // ISSUE: reference to a compiler-generated method
            for (; this.FindBestIntersectionToRemove(prefabGeometryData, source, minIndex, maxIndex, out bestIndex); --maxIndex)
            {
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.IntersectPos intersectPos12 = source[bestIndex - 1];
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.IntersectPos intersectPos13 = source[bestIndex];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos12.m_Priority += intersectPos13.m_Priority;
              source[bestIndex - 1] = intersectPos12;
              if (bestIndex == minIndex)
              {
                // ISSUE: variable of a compiler-generated type
                CourseSplitSystem.IntersectPos intersectPos14 = target[target.Length - 1];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                intersectPos14.m_Priority += intersectPos13.m_Priority;
                target[target.Length - 1] = intersectPos14;
              }
              for (int index12 = bestIndex; index12 < maxIndex; ++index12)
                source[index12] = source[index12 + 1];
            }
            for (int index13 = minIndex; index13 <= maxIndex; ++index13)
            {
              ref NativeList<CourseSplitSystem.IntersectPos> local1 = ref target;
              // ISSUE: variable of a compiler-generated type
              CourseSplitSystem.IntersectPos intersectPos = source[index13];
              ref CourseSplitSystem.IntersectPos local2 = ref intersectPos;
              local1.Add(in local2);
            }
          }
          else
          {
            ref NativeList<CourseSplitSystem.IntersectPos> local3 = ref target;
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.IntersectPos intersectPos = source[index11];
            ref CourseSplitSystem.IntersectPos local4 = ref intersectPos;
            local3.Add(in local4);
          }
        }
        for (int index = 1; index + 1 < target.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos15 = target[index];
          // ISSUE: reference to a compiler-generated field
          if (intersectPos15.m_IsOptional)
          {
            // ISSUE: variable of a compiler-generated type
            CourseSplitSystem.IntersectPos intersectPos16 = target[index - 1];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            intersectPos15.m_CanMove.min = math.min(0.0f, math.max(intersectPos15.m_CanMove.min, intersectPos15.m_Priority - prefabGeometryData.m_ElevatedLength * 0.95f));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            intersectPos15.m_CanMove.max = math.max(0.0f, math.min(intersectPos15.m_CanMove.max, prefabGeometryData.m_ElevatedLength * 0.95f - intersectPos16.m_Priority));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float length = MathUtils.Clamp((float) (((double) intersectPos15.m_Priority - (double) intersectPos16.m_Priority) * 0.5), intersectPos15.m_CanMove);
            if ((double) length != 0.0)
            {
              if ((double) length > 0.0)
              {
                // ISSUE: reference to a compiler-generated field
                Bounds1 t = new Bounds1(intersectPos15.m_CoursePos.m_CourseDelta, 1f);
                MathUtils.ClampLength(courseData.m_Curve.xz, ref t, length);
                // ISSUE: reference to a compiler-generated field
                intersectPos15.m_CoursePos.m_CourseDelta = t.max;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                intersectPos15.m_CanMove.max = math.max(0.0f, intersectPos15.m_CanMove.max - length);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                Bounds1 t = new Bounds1(0.0f, intersectPos15.m_CoursePos.m_CourseDelta);
                MathUtils.ClampLengthInverse(courseData.m_Curve.xz, ref t, -length);
                // ISSUE: reference to a compiler-generated field
                intersectPos15.m_CoursePos.m_CourseDelta = t.min;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                intersectPos15.m_CanMove.min = math.min(0.0f, intersectPos15.m_CanMove.min - length);
              }
              // ISSUE: reference to a compiler-generated field
              intersectPos15.m_Priority -= length;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos15.m_CoursePos.m_Position = MathUtils.Position(courseData.m_Curve, intersectPos15.m_CoursePos.m_CourseDelta);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              intersectPos15.m_CoursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, intersectPos15.m_CoursePos.m_CourseDelta));
              target[index] = intersectPos15;
              // ISSUE: reference to a compiler-generated field
              intersectPos16.m_Priority += length;
              target[index - 1] = intersectPos16;
            }
          }
        }
      }

      private bool FindOptionalRange(
        NativeList<CourseSplitSystem.IntersectPos> source,
        int index,
        out int minIndex,
        out int maxIndex)
      {
        minIndex = index;
        maxIndex = index - 1;
        if (index == 0)
          return false;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = minIndex; index1 + 1 < source.Length && source[index1].m_IsOptional; ++index1)
          maxIndex = index1;
        return maxIndex >= minIndex;
      }

      private bool FindBestIntersectionToRemove(
        NetGeometryData netGeometryData,
        NativeList<CourseSplitSystem.IntersectPos> source,
        int minIndex,
        int maxIndex,
        out int bestIndex)
      {
        bestIndex = minIndex;
        float num1 = netGeometryData.m_ElevatedLength;
        for (int index = minIndex; index <= maxIndex; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos1 = source[index - 1];
          // ISSUE: variable of a compiler-generated type
          CourseSplitSystem.IntersectPos intersectPos2 = source[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num2 = intersectPos1.m_Priority + intersectPos2.m_Priority;
          if ((double) num2 < (double) num1)
          {
            bestIndex = index;
            num1 = num2;
          }
        }
        return (double) num1 <= (double) netGeometryData.m_ElevatedLength * 0.949999988079071;
      }

      private void UpdateCourses(
        NetCourse courseData,
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        Upgraded upgraded,
        Entity courseEntity,
        int jobIndex,
        NativeList<CourseSplitSystem.IntersectPos> intersectionList,
        ref CourseSplitSystem.CourseHeightData courseHeightData)
      {
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_PrefabNetData[creationDefinition.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        NetGeometryData netGeometryData = this.m_PrefabGeometryData[creationDefinition.m_Prefab];
        PlaceableNetData componentData;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabPlaceableData.TryGetComponent(creationDefinition.m_Prefab, out componentData);
        DynamicBuffer<FixedNetElement> fixedNetElements = new DynamicBuffer<FixedNetElement>();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabFixedNetElements.HasBuffer(creationDefinition.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          fixedNetElements = this.m_PrefabFixedNetElements[creationDefinition.m_Prefab];
        }
        int courseIndex = 0;
        if (intersectionList.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          courseData.m_StartPosition = intersectionList[0].m_CoursePos;
          int num = 0;
          for (int index = 1; index < intersectionList.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            courseData.m_EndPosition = intersectionList[index].m_CoursePos;
            if (courseData.m_EndPosition.m_Entity == Entity.Null || courseData.m_EndPosition.m_Entity != courseData.m_StartPosition.m_Entity)
            {
              // ISSUE: reference to a compiler-generated method
              this.TryAddCourse(courseData, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, componentData, fixedNetElements, ref courseHeightData, ref courseIndex);
              ++num;
            }
            courseData.m_StartPosition = courseData.m_EndPosition;
          }
          if (num == 0)
          {
            // ISSUE: reference to a compiler-generated field
            courseData.m_StartPosition = intersectionList[0].m_CoursePos;
            // ISSUE: reference to a compiler-generated field
            courseData.m_EndPosition = intersectionList[intersectionList.Length - 1].m_CoursePos;
            // ISSUE: reference to a compiler-generated method
            this.TryAddCourse(courseData, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, componentData, fixedNetElements, ref courseHeightData, ref courseIndex);
          }
        }
        if (courseIndex != 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.DestroyEntity(jobIndex, courseEntity);
      }

      private void TryAddCourse(
        NetCourse courseData,
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        Upgraded upgraded,
        Entity courseEntity,
        int jobIndex,
        NetData netData,
        NetGeometryData netGeometryData,
        PlaceableNetData placeableNetData,
        DynamicBuffer<FixedNetElement> fixedNetElements,
        ref CourseSplitSystem.CourseHeightData courseHeightData,
        ref int courseIndex)
      {
        if (creationDefinition.m_Original != Entity.Null || creationDefinition.m_Prefab == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          courseHeightData.SampleCourseHeight(ref courseData, netGeometryData);
          // ISSUE: reference to a compiler-generated method
          this.AddCourse(courseData, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, ref courseIndex);
        }
        else
        {
          float2 xz1 = MathUtils.Tangent(courseData.m_Curve, courseData.m_StartPosition.m_CourseDelta).xz;
          float2 xz2 = MathUtils.Tangent(courseData.m_Curve, courseData.m_EndPosition.m_CourseDelta).xz;
          if (!MathUtils.TryNormalize(ref xz1) || !MathUtils.TryNormalize(ref xz2))
          {
            // ISSUE: reference to a compiler-generated method
            courseHeightData.SampleCourseHeight(ref courseData, netGeometryData);
            // ISSUE: reference to a compiler-generated method
            this.AddCourse(courseData, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, ref courseIndex);
          }
          else if ((double) math.dot(xz1, xz2) < -1.0 / 1000.0 && (netGeometryData.m_Flags & Game.Net.GeometryFlags.NoCurveSplit) == (Game.Net.GeometryFlags) 0)
          {
            float middleTangentPos = NetUtils.FindMiddleTangentPos(courseData.m_Curve.xz, new float2(courseData.m_StartPosition.m_CourseDelta, courseData.m_EndPosition.m_CourseDelta));
            CoursePos coursePos = new CoursePos();
            coursePos.m_CourseDelta = middleTangentPos;
            coursePos.m_Position = MathUtils.Position(courseData.m_Curve, middleTangentPos);
            coursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(courseData.m_Curve, middleTangentPos));
            coursePos.m_Flags = courseData.m_StartPosition.m_Flags & (CoursePosFlags.IsParallel | CoursePosFlags.IsRight | CoursePosFlags.IsLeft | CoursePosFlags.IsGrid);
            coursePos.m_Flags |= CoursePosFlags.FreeHeight;
            coursePos.m_ParentMesh = math.select(courseData.m_StartPosition.m_ParentMesh, -1, courseData.m_StartPosition.m_ParentMesh != courseData.m_EndPosition.m_ParentMesh);
            NetCourse courseData1 = courseData;
            NetCourse courseData2 = courseData;
            courseData1.m_EndPosition = coursePos;
            courseData2.m_StartPosition = coursePos;
            courseData1.m_Length = MathUtils.Length(courseData1.m_Curve, new Bounds1(courseData1.m_StartPosition.m_CourseDelta, courseData1.m_EndPosition.m_CourseDelta));
            courseData2.m_Length = MathUtils.Length(courseData2.m_Curve, new Bounds1(courseData2.m_StartPosition.m_CourseDelta, courseData2.m_EndPosition.m_CourseDelta));
            // ISSUE: reference to a compiler-generated method
            this.TryAddCoursePhase2(courseData1, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, fixedNetElements, ref courseHeightData, ref courseIndex);
            // ISSUE: reference to a compiler-generated method
            this.TryAddCoursePhase2(courseData2, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, fixedNetElements, ref courseHeightData, ref courseIndex);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.TryAddCoursePhase2(courseData, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, fixedNetElements, ref courseHeightData, ref courseIndex);
          }
        }
      }

      private void TryAddCoursePhase2(
        NetCourse courseData,
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        Upgraded upgraded,
        Entity courseEntity,
        int jobIndex,
        NetData netData,
        NetGeometryData netGeometryData,
        PlaceableNetData placeableNetData,
        DynamicBuffer<FixedNetElement> fixedNetElements,
        ref CourseSplitSystem.CourseHeightData courseHeightData,
        ref int courseIndex)
      {
        float num1 = MathUtils.Length(courseData.m_Curve.xz, new Bounds1(courseData.m_StartPosition.m_CourseDelta, courseData.m_EndPosition.m_CourseDelta));
        if (fixedNetElements.IsCreated)
        {
          float2 y1 = new float2(0.0f, 0.0f);
          float y2 = 0.0f;
          for (int index = 0; index < fixedNetElements.Length; ++index)
          {
            FixedNetElement fixedNetElement = fixedNetElements[index];
            float2 float2 = new float2(fixedNetElement.m_LengthRange.min, fixedNetElement.m_LengthRange.max) * (float2) math.select(fixedNetElement.m_CountRange, new int2(0, 10000), fixedNetElement.m_CountRange == 0);
            float num2 = math.select(0.0f, float2.y - float2.x, (double) fixedNetElement.m_LengthRange.max != (double) fixedNetElement.m_LengthRange.min);
            y1 += float2;
            y2 += num2;
          }
          float2 float2_1 = (num1 - 0.16f) / math.max((float2) 1f, y1);
          int num3;
          if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.NoCurveSplit) != (Game.Net.GeometryFlags) 0)
          {
            num3 = Mathf.CeilToInt(float2_1.y);
          }
          else
          {
            float num4 = math.acos(math.clamp(math.dot(math.normalizesafe(MathUtils.Tangent(courseData.m_Curve, courseData.m_StartPosition.m_CourseDelta).xz), math.normalizesafe(MathUtils.Tangent(courseData.m_Curve, courseData.m_EndPosition.m_CourseDelta).xz)), -1f, 1f));
            double x = (double) math.ceil(float2_1.y);
            num3 = Mathf.RoundToInt(math.lerp((float) x, math.max((float) x, math.floor(float2_1.x)), math.saturate(num4 * 0.636619747f)));
          }
          NetCourse netCourse = courseData;
          for (int index1 = 1; index1 <= num3; ++index1)
          {
            // ISSUE: reference to a compiler-generated method
            netCourse.m_EndPosition = index1 != num3 ? this.CutCourse(courseData, netGeometryData, num1 * (float) index1 / (float) num3) : courseData.m_EndPosition;
            NetCourse course1 = netCourse;
            if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StraightEdges) != (Game.Net.GeometryFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              courseHeightData.SampleCourseHeight(ref course1, netGeometryData);
            }
            else
              course1.m_Length = MathUtils.Length(course1.m_Curve.xz, new Bounds1(course1.m_StartPosition.m_CourseDelta, course1.m_EndPosition.m_CourseDelta));
            float y3 = course1.m_Length - y1.x;
            float num5 = math.max(0.0f, y3);
            float num6 = 0.0f;
            float num7 = y3 - num5;
            NativeArray<NetCourse> nativeArray = new NativeArray<NetCourse>(fixedNetElements.Length, Allocator.Temp);
            NetCourse course2 = course1;
            for (int index2 = 0; index2 < fixedNetElements.Length; ++index2)
            {
              FixedNetElement fixedNetElement = fixedNetElements[index2];
              float2 float2_2 = new float2(fixedNetElement.m_LengthRange.min, fixedNetElement.m_LengthRange.max) * (float2) math.select(fixedNetElement.m_CountRange, new int2(0, 10000), fixedNetElement.m_CountRange == 0);
              float num8 = math.select(0.0f, float2_2.y - float2_2.x, (double) fixedNetElement.m_LengthRange.max != (double) fixedNetElement.m_LengthRange.min);
              course2.m_Length = float2_2.x + num8 * num5 / math.max(1f, y2);
              course2.m_Length += float2_2.x * num7 / math.max(1f, y1.x);
              course2.m_FixedIndex = index2;
              if (index2 == fixedNetElements.Length - 1)
              {
                course2.m_EndPosition = course1.m_EndPosition;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                course2.m_EndPosition = this.CutCourse(course1, netGeometryData, num6 + course2.m_Length);
                course2.m_EndPosition.m_Flags |= CoursePosFlags.IsFixed;
                num6 += course2.m_Length;
              }
              nativeArray[index2] = course2;
              course2.m_StartPosition = course2.m_EndPosition;
            }
            if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StraightEdges) == (Game.Net.GeometryFlags) 0)
            {
              int index3 = 0;
              int index4 = fixedNetElements.Length - 1;
              float3 position1 = course1.m_StartPosition.m_Position;
              float3 position2 = course1.m_EndPosition.m_Position;
              float3 float3 = MathUtils.Tangent(course1.m_Curve, course1.m_StartPosition.m_CourseDelta);
              float3 endTangent = MathUtils.Tangent(course1.m_Curve, course1.m_EndPosition.m_CourseDelta);
              float3 = MathUtils.Normalize(float3, float3.xz);
              endTangent = MathUtils.Normalize(endTangent, endTangent.xz);
              for (int index5 = 0; index5 < fixedNetElements.Length && (fixedNetElements[index5].m_Flags & FixedNetFlags.Straight) != (FixedNetFlags) 0; ++index5)
              {
                course2 = nativeArray[index5];
                course2.m_Curve.xz = NetUtils.StraightCurve(position1, position1 + float3 * course2.m_Length).xz;
                course2.m_StartPosition.m_CourseDelta = 0.0f;
                course2.m_StartPosition.m_Position = course2.m_Curve.a;
                course2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(float3);
                course2.m_EndPosition.m_CourseDelta = 1f;
                course2.m_EndPosition.m_Position = course2.m_Curve.d;
                course2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(float3);
                nativeArray[index5] = course2;
                position1 = course2.m_EndPosition.m_Position;
                ++index3;
              }
              for (int index6 = fixedNetElements.Length - 1; index6 >= 0 && (fixedNetElements[index6].m_Flags & FixedNetFlags.Straight) != (FixedNetFlags) 0; --index6)
              {
                course2 = nativeArray[index6];
                course2.m_Curve.xz = NetUtils.StraightCurve(position2 - endTangent * course2.m_Length, position2 + float3).xz;
                course2.m_StartPosition.m_CourseDelta = 0.0f;
                course2.m_StartPosition.m_Position = course2.m_Curve.a;
                course2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(-endTangent);
                course2.m_EndPosition.m_CourseDelta = 1f;
                course2.m_EndPosition.m_Position = course2.m_Curve.d;
                course2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(-endTangent);
                nativeArray[index6] = course2;
                position2 = course2.m_StartPosition.m_Position;
                --index4;
              }
              if (index4 >= index3)
              {
                Bezier4x3 curve = NetUtils.FitCurve(position1, float3, endTangent, position2);
                float courseDelta1 = nativeArray[index3].m_StartPosition.m_CourseDelta;
                float courseDelta2 = nativeArray[index4].m_EndPosition.m_CourseDelta;
                for (int index7 = index3; index7 <= index4; ++index7)
                {
                  course2 = nativeArray[index7];
                  float2 t = new float2(course2.m_StartPosition.m_CourseDelta, course2.m_EndPosition.m_CourseDelta);
                  t = (t - courseDelta1) / math.max(1f / 1000f, courseDelta2 - courseDelta1);
                  course2.m_Curve.xz = MathUtils.Cut(curve, t).xz;
                  course2.m_StartPosition.m_CourseDelta = 0.0f;
                  course2.m_StartPosition.m_Position = course2.m_Curve.a;
                  course2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(course2.m_Curve));
                  course2.m_EndPosition.m_CourseDelta = 1f;
                  course2.m_EndPosition.m_Position = course2.m_Curve.d;
                  course2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(course2.m_Curve));
                  nativeArray[index7] = course2;
                }
              }
            }
            for (int index8 = 0; index8 < fixedNetElements.Length; ++index8)
            {
              course2 = nativeArray[index8];
              FixedNetElement fixedNetElement = fixedNetElements[index8];
              float2 float2_3 = new float2(fixedNetElement.m_LengthRange.min, fixedNetElement.m_LengthRange.max);
              int2 int2 = math.select(fixedNetElement.m_CountRange, new int2(0, 10000), fixedNetElement.m_CountRange == 0);
              int num9 = math.clamp((int) math.ceil((course2.m_Length - 0.16f) / float2_3.y), int2.x, int2.y);
              NetCourse courseData1 = course2;
              for (int index9 = 1; index9 <= num9; ++index9)
              {
                if (index9 == num9)
                {
                  courseData1.m_EndPosition = course2.m_EndPosition;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  courseData1.m_EndPosition = this.CutCourse(course2, netGeometryData, course2.m_Length * (float) index9 / (float) num9);
                  courseData1.m_EndPosition.m_Flags |= CoursePosFlags.IsFixed;
                }
                if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.StraightEdges) == (Game.Net.GeometryFlags) 0)
                {
                  NetCourse course3 = courseData1;
                  // ISSUE: reference to a compiler-generated method
                  courseHeightData.SampleCourseHeight(ref course3, netGeometryData);
                  // ISSUE: reference to a compiler-generated method
                  this.AddCourse(course3, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, ref courseIndex);
                }
                else
                {
                  courseData1.m_Length = MathUtils.Length(courseData1.m_Curve.xz, new Bounds1(courseData1.m_StartPosition.m_CourseDelta, courseData1.m_EndPosition.m_CourseDelta));
                  // ISSUE: reference to a compiler-generated method
                  this.AddCourse(courseData1, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, ref courseIndex);
                }
                courseData1.m_StartPosition = courseData1.m_EndPosition;
              }
            }
            nativeArray.Dispose();
            netCourse.m_StartPosition = netCourse.m_EndPosition;
          }
        }
        else
        {
          NetCourse courseData2 = courseData;
          // ISSUE: reference to a compiler-generated method
          courseHeightData.SampleCourseHeight(ref courseData2, netGeometryData);
          // ISSUE: reference to a compiler-generated method
          this.CalculateElevation(creationDefinition, ownerDefinition, ref courseData2, ref upgraded, netGeometryData, placeableNetData);
          float a = netGeometryData.m_EdgeLengthRange.max;
          if ((creationDefinition.m_Flags & CreationFlags.SubElevation) != (CreationFlags) 0)
          {
            CompositionFlags elevationFlags = NetCompositionHelpers.GetElevationFlags(new Game.Net.Elevation(courseData2.m_StartPosition.m_Elevation), new Game.Net.Elevation(courseData2.m_Elevation), new Game.Net.Elevation(courseData2.m_EndPosition.m_Elevation), netGeometryData);
            a = math.select(a, netGeometryData.m_ElevatedLength, (elevationFlags.m_General & CompositionFlags.General.Elevated) > (CompositionFlags.General) 0);
          }
          int num10 = (int) math.ceil((num1 - 0.16f) / a);
          if (num10 > 1)
          {
            NetCourse netCourse = courseData;
            for (int index = 1; index <= num10; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              netCourse.m_EndPosition = index != num10 ? this.CutCourse(courseData, netGeometryData, num1 * (float) index / (float) num10) : courseData.m_EndPosition;
              NetCourse course = netCourse;
              // ISSUE: reference to a compiler-generated method
              courseHeightData.SampleCourseHeight(ref course, netGeometryData);
              // ISSUE: reference to a compiler-generated method
              this.AddCourse(course, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, netGeometryData, placeableNetData, ref courseIndex);
              netCourse.m_StartPosition = netCourse.m_EndPosition;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddCourse(courseData2, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, ref courseIndex);
          }
        }
      }

      private CoursePos CutCourse(
        NetCourse course,
        NetGeometryData netGeometryData,
        float cutLength)
      {
        CoursePos coursePos = new CoursePos();
        if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.SnapCellSize) != (Game.Net.GeometryFlags) 0)
        {
          cutLength = MathUtils.Snap(cutLength + 0.16f, 4f);
          if (((uint) (int) math.round(cutLength / 4f) & 1U) > 0U != (course.m_StartPosition.m_Flags & CoursePosFlags.HalfAlign) > (CoursePosFlags) 0)
            coursePos.m_Flags |= CoursePosFlags.HalfAlign;
        }
        Bounds1 t = new Bounds1(course.m_StartPosition.m_CourseDelta, 1f);
        MathUtils.ClampLength(course.m_Curve.xz, ref t, cutLength);
        coursePos.m_CourseDelta = t.max;
        coursePos.m_Position = MathUtils.Position(course.m_Curve, coursePos.m_CourseDelta);
        coursePos.m_Rotation = NetUtils.GetNodeRotation(MathUtils.Tangent(course.m_Curve, coursePos.m_CourseDelta));
        coursePos.m_Flags |= course.m_StartPosition.m_Flags & (CoursePosFlags.IsParallel | CoursePosFlags.IsRight | CoursePosFlags.IsLeft | CoursePosFlags.ForceElevatedEdge | CoursePosFlags.IsGrid);
        coursePos.m_Flags |= CoursePosFlags.FreeHeight;
        coursePos.m_ParentMesh = math.select(course.m_StartPosition.m_ParentMesh, -1, course.m_StartPosition.m_ParentMesh != course.m_EndPosition.m_ParentMesh);
        if ((course.m_StartPosition.m_Flags & CoursePosFlags.ForceElevatedEdge) != (CoursePosFlags) 0)
          coursePos.m_Flags |= CoursePosFlags.ForceElevatedNode;
        return coursePos;
      }

      private float2 CalculateElevation(
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        NetGeometryData netGeometryData,
        Bezier4x3 curve,
        float delta,
        float offset)
      {
        float3 float3_1 = MathUtils.Position(curve, delta);
        bool flag = (netGeometryData.m_Flags & Game.Net.GeometryFlags.SubOwner) == (Game.Net.GeometryFlags) 0;
        if ((netGeometryData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
          return (float2) 0.0f;
        if (flag && ownerDefinition.m_Prefab != Entity.Null)
          return (float2) (float3_1.y - ownerDefinition.m_Position.y);
        Game.Objects.Transform componentData;
        // ISSUE: reference to a compiler-generated field
        if (flag && creationDefinition.m_Owner != Entity.Null && this.m_TransformData.TryGetComponent(creationDefinition.m_Owner, out componentData))
          return (float2) (float3_1.y - componentData.m_Position.y);
        float3 float3_2 = MathUtils.Tangent(curve, delta);
        float3 float3_3 = new float3();
        float3_3.xz = math.normalizesafe(MathUtils.Right(float3_2.xz)) * offset;
        float2 elevation;
        // ISSUE: reference to a compiler-generated field
        elevation.x = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, float3_1 - float3_3);
        // ISSUE: reference to a compiler-generated field
        elevation.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, float3_1 + float3_3);
        elevation = float3_1.y - elevation;
        return elevation;
      }

      private void CalculateElevation(
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        ref NetCourse courseData,
        ref Upgraded upgraded,
        NetGeometryData netGeometryData,
        PlaceableNetData placeableNetData)
      {
        float delta = math.lerp(courseData.m_StartPosition.m_CourseDelta, courseData.m_EndPosition.m_CourseDelta, 0.5f);
        float offset = netGeometryData.m_DefaultWidth * 0.5f;
        // ISSUE: reference to a compiler-generated method
        float2 elevation1 = this.CalculateElevation(creationDefinition, ownerDefinition, netGeometryData, courseData.m_Curve, courseData.m_StartPosition.m_CourseDelta, offset);
        // ISSUE: reference to a compiler-generated method
        float2 elevation2 = this.CalculateElevation(creationDefinition, ownerDefinition, netGeometryData, courseData.m_Curve, delta, offset);
        // ISSUE: reference to a compiler-generated method
        float2 elevation3 = this.CalculateElevation(creationDefinition, ownerDefinition, netGeometryData, courseData.m_Curve, courseData.m_EndPosition.m_CourseDelta, offset);
        bool flag = (upgraded.m_Flags.m_General & CompositionFlags.General.Elevated) > (CompositionFlags.General) 0;
        if (flag)
        {
          courseData.m_StartPosition.m_Flags |= CoursePosFlags.ForceElevatedEdge;
          courseData.m_EndPosition.m_Flags |= CoursePosFlags.ForceElevatedEdge;
          upgraded.m_Flags.m_General &= ~CompositionFlags.General.Elevated;
        }
        float2 a1 = math.select(elevation1, (float2) (netGeometryData.m_ElevationLimit * 2f), (courseData.m_StartPosition.m_Flags & CoursePosFlags.ForceElevatedNode) > (CoursePosFlags) 0 & elevation1 < netGeometryData.m_ElevationLimit * 2f);
        float2 b1 = math.select(elevation2, (float2) (netGeometryData.m_ElevationLimit * 2f), (courseData.m_StartPosition.m_Flags & courseData.m_EndPosition.m_Flags & CoursePosFlags.ForceElevatedEdge) > (CoursePosFlags) 0 & elevation2 < netGeometryData.m_ElevationLimit * 2f);
        float2 a2 = math.select(elevation3, (float2) (netGeometryData.m_ElevationLimit * 2f), (courseData.m_EndPosition.m_Flags & CoursePosFlags.ForceElevatedNode) > (CoursePosFlags) 0 & elevation3 < netGeometryData.m_ElevationLimit * 2f);
        float2 b2 = math.select(a1, (float2) 0.0f, new bool2((courseData.m_StartPosition.m_Flags & CoursePosFlags.LeftTransition) > (CoursePosFlags) 0, (courseData.m_StartPosition.m_Flags & CoursePosFlags.RightTransition) > (CoursePosFlags) 0));
        float2 b3 = math.select(a2, (float2) 0.0f, new bool2((courseData.m_EndPosition.m_Flags & CoursePosFlags.LeftTransition) > (CoursePosFlags) 0, (courseData.m_EndPosition.m_Flags & CoursePosFlags.RightTransition) > (CoursePosFlags) 0));
        courseData.m_StartPosition.m_Elevation = math.select(new float2(), b2, b2 >= netGeometryData.m_ElevationLimit | b2 <= -netGeometryData.m_ElevationLimit);
        courseData.m_Elevation = math.select(new float2(), b1, b1 >= netGeometryData.m_ElevationLimit | b1 <= -netGeometryData.m_ElevationLimit);
        courseData.m_EndPosition.m_Elevation = math.select(new float2(), b3, b3 >= netGeometryData.m_ElevationLimit | b3 <= -netGeometryData.m_ElevationLimit);
        if ((creationDefinition.m_Owner != Entity.Null || ownerDefinition.m_Prefab != Entity.Null) && !flag && (creationDefinition.m_Flags & CreationFlags.SubElevation) == (CreationFlags) 0)
        {
          if (courseData.m_StartPosition.m_ParentMesh < 0)
            courseData.m_StartPosition.m_Elevation = new float2();
          if (courseData.m_StartPosition.m_ParentMesh < 0 && courseData.m_EndPosition.m_ParentMesh < 0)
            courseData.m_Elevation = new float2();
          if (courseData.m_EndPosition.m_ParentMesh < 0)
            courseData.m_EndPosition.m_Elevation = new float2();
        }
        // ISSUE: reference to a compiler-generated method
        this.LimitElevation(ref courseData.m_StartPosition.m_Elevation, placeableNetData);
        // ISSUE: reference to a compiler-generated method
        this.LimitElevation(ref courseData.m_Elevation, placeableNetData);
        // ISSUE: reference to a compiler-generated method
        this.LimitElevation(ref courseData.m_EndPosition.m_Elevation, placeableNetData);
      }

      private void AddCourse(
        NetCourse courseData,
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        Upgraded upgraded,
        Entity courseEntity,
        int jobIndex,
        NetData netData,
        NetGeometryData netGeometryData,
        PlaceableNetData placeableNetData,
        ref int courseIndex)
      {
        // ISSUE: reference to a compiler-generated method
        this.CalculateElevation(creationDefinition, ownerDefinition, ref courseData, ref upgraded, netGeometryData, placeableNetData);
        // ISSUE: reference to a compiler-generated method
        this.AddCourse(courseData, creationDefinition, ownerDefinition, upgraded, courseEntity, jobIndex, netData, ref courseIndex);
      }

      private bool IgnoreOverlappingEdge(
        CreationDefinition creationDefinition,
        NetCourse courseData)
      {
        Game.Net.Edge componentData1;
        Game.Net.Edge componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !(creationDefinition.m_Original != Entity.Null) && !(courseData.m_StartPosition.m_Entity == Entity.Null) && !(courseData.m_EndPosition.m_Entity == Entity.Null) && ((courseData.m_StartPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsGrid)) != CoursePosFlags.IsFirst || (courseData.m_EndPosition.m_Flags & (CoursePosFlags.IsLast | CoursePosFlags.IsGrid)) != CoursePosFlags.IsLast) && ((courseData.m_StartPosition.m_Flags & (CoursePosFlags.IsLast | CoursePosFlags.IsGrid)) != CoursePosFlags.IsLast || (courseData.m_EndPosition.m_Flags & (CoursePosFlags.IsFirst | CoursePosFlags.IsGrid)) != CoursePosFlags.IsFirst) && (courseData.m_EndPosition.m_Entity == courseData.m_StartPosition.m_Entity || this.m_EdgeData.TryGetComponent(courseData.m_StartPosition.m_Entity, out componentData1) && (courseData.m_EndPosition.m_Entity == componentData1.m_Start || courseData.m_EndPosition.m_Entity == componentData1.m_End) || this.m_EdgeData.TryGetComponent(courseData.m_EndPosition.m_Entity, out componentData2) && (courseData.m_StartPosition.m_Entity == componentData2.m_Start || courseData.m_StartPosition.m_Entity == componentData2.m_End));
      }

      private void AddCourse(
        NetCourse courseData,
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        Upgraded upgraded,
        Entity courseEntity,
        int jobIndex,
        NetData netData,
        ref int courseIndex)
      {
        creationDefinition.m_RandomSeed += courseIndex;
        // ISSUE: reference to a compiler-generated method
        this.FindOriginalEdge(ref creationDefinition, ownerDefinition, netData, courseData);
        // ISSUE: reference to a compiler-generated method
        if (this.IgnoreOverlappingEdge(creationDefinition, courseData))
        {
          courseData.m_StartPosition.m_Flags |= CoursePosFlags.DontCreate;
          courseData.m_EndPosition.m_Flags |= CoursePosFlags.DontCreate;
        }
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_LocalCurveCacheData.HasComponent(courseEntity);
        LocalCurveCache component = new LocalCurveCache();
        if (flag)
        {
          Game.Objects.Transform inverseParentTransform = ObjectUtils.InverseTransform(new Game.Objects.Transform(ownerDefinition.m_Position, ownerDefinition.m_Rotation));
          component.m_Curve.a = ObjectUtils.WorldToLocal(inverseParentTransform, courseData.m_Curve.a);
          component.m_Curve.b = ObjectUtils.WorldToLocal(inverseParentTransform, courseData.m_Curve.b);
          component.m_Curve.c = ObjectUtils.WorldToLocal(inverseParentTransform, courseData.m_Curve.c);
          component.m_Curve.d = ObjectUtils.WorldToLocal(inverseParentTransform, courseData.m_Curve.d);
        }
        if (courseIndex++ == 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<CreationDefinition>(jobIndex, courseEntity, creationDefinition);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<NetCourse>(jobIndex, courseEntity, courseData);
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<LocalCurveCache>(jobIndex, courseEntity, component);
          }
          if (upgraded.m_Flags != new CompositionFlags())
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Upgraded>(jobIndex, courseEntity, upgraded);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Upgraded>(jobIndex, courseEntity);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(jobIndex, entity, creationDefinition);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<NetCourse>(jobIndex, entity, courseData);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
          if (ownerDefinition.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<OwnerDefinition>(jobIndex, entity, ownerDefinition);
          }
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<LocalCurveCache>(jobIndex, entity, component);
          }
          if (!(upgraded.m_Flags != new CompositionFlags()))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Upgraded>(jobIndex, entity, upgraded);
        }
      }

      private void LimitElevation(ref float2 elevation, PlaceableNetData placeableNetData)
      {
        elevation = math.select(elevation, (float2) placeableNetData.m_ElevationRange.min, elevation < placeableNetData.m_ElevationRange.min & (double) placeableNetData.m_ElevationRange.min >= 0.0);
        elevation = math.select(elevation, (float2) placeableNetData.m_ElevationRange.max, elevation > placeableNetData.m_ElevationRange.max & (double) placeableNetData.m_ElevationRange.max < 0.0);
      }

      private bool MatchingOwner(
        CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        Entity entity)
      {
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(entity, out componentData1))
        {
          Game.Objects.Transform componentData2;
          PrefabRef componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (creationDefinition.m_Owner != componentData1.m_Owner && (!this.m_TransformData.TryGetComponent(componentData1.m_Owner, out componentData2) || !this.m_PrefabRefData.TryGetComponent(componentData1.m_Owner, out componentData3) || !ownerDefinition.m_Position.Equals(componentData2.m_Position) || !ownerDefinition.m_Rotation.Equals(componentData2.m_Rotation) || ownerDefinition.m_Prefab != componentData3.m_Prefab))
            return false;
        }
        else if (ownerDefinition.m_Prefab != Entity.Null)
          return false;
        return true;
      }

      private void FindOriginalEdge(
        ref CreationDefinition creationDefinition,
        OwnerDefinition ownerDefinition,
        NetData netData,
        NetCourse netCourse)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (creationDefinition.m_Original != Entity.Null || (creationDefinition.m_Flags & CreationFlags.Permanent) != (CreationFlags) 0 || !this.m_ConnectedEdges.HasBuffer(netCourse.m_StartPosition.m_Entity) || !this.m_ConnectedEdges.HasBuffer(netCourse.m_EndPosition.m_Entity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[netCourse.m_StartPosition.m_Entity];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          Entity edge1 = connectedEdge[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedEntities.ContainsKey(edge1))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Edge edge2 = this.m_EdgeData[edge1];
            if (edge2.m_Start == netCourse.m_StartPosition.m_Entity && edge2.m_End == netCourse.m_EndPosition.m_Entity)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.CanReplace(netData, edge1))
              {
                creationDefinition.m_Original = edge1;
                break;
              }
              break;
            }
            if (edge2.m_Start == netCourse.m_EndPosition.m_Entity && edge2.m_End == netCourse.m_StartPosition.m_Entity)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.CanReplace(netData, edge1))
              {
                creationDefinition.m_Original = edge1;
                creationDefinition.m_Flags |= CreationFlags.Invert;
                break;
              }
              break;
            }
          }
        }
        // ISSUE: reference to a compiler-generated method
        if (!(creationDefinition.m_Original != Entity.Null) || this.MatchingOwner(creationDefinition, ownerDefinition, creationDefinition.m_Original))
          return;
        creationDefinition.m_Original = Entity.Null;
        creationDefinition.m_Flags &= ~CreationFlags.Invert;
      }

      private bool CanReplace(NetData netData, Entity original)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetData netData1 = this.m_PrefabNetData[this.m_PrefabRefData[original].m_Prefab];
        return (netData.m_RequiredLayers & netData1.m_RequiredLayers) > Layer.None;
      }

      private struct ElevationSegment
      {
        public Bounds1 m_CourseRange;
        public Bounds1 m_DistanceOffset;
        public int2 m_ElevationType;
        public bool m_CanRemove;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<OwnerDefinition> __Game_Tools_OwnerDefinition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> __Game_Tools_NetCourse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> __Game_Net_Upgraded_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Fixed> __Game_Net_Fixed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalCurveCache> __Game_Tools_LocalCurveCache_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetData> __Game_Prefabs_PlaceableNetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<FixedNetElement> __Game_Prefabs_FixedNetElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_OwnerDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<OwnerDefinition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_NetCourse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCourse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Fixed_RO_ComponentLookup = state.GetComponentLookup<Fixed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalCurveCache_RO_ComponentLookup = state.GetComponentLookup<LocalCurveCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetData_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FixedNetElement_RO_BufferLookup = state.GetBufferLookup<FixedNetElement>(true);
      }
    }
  }
}
