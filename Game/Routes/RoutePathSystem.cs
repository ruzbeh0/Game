// Decompiled with JetBrains decompiler
// Type: Game.Routes.RoutePathSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class RoutePathSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private PathfindQueueSystem m_PathfindQueueSystem;
    private EntityQuery m_UpdatedSegmentQuery;
    private EntityQuery m_DeletedLaneQuery;
    private EntityQuery m_AppliedLaneQuery;
    private EntityQuery m_SegmentQuery;
    private NativeParallelHashSet<Entity> m_LazyUpdateSet;
    private RoutePathSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedSegmentQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Segment>(), ComponentType.ReadWrite<PathTargets>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Lane>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Applied>(), ComponentType.ReadOnly<Lane>());
      // ISSUE: reference to a compiler-generated field
      this.m_SegmentQuery = this.GetEntityQuery(ComponentType.ReadOnly<Segment>(), ComponentType.ReadOnly<PathElement>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_LazyUpdateSet = new NativeParallelHashSet<Entity>(20, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LazyUpdateSet.Dispose();
      base.OnDestroy();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LazyUpdateSet.Count());
      // ISSUE: reference to a compiler-generated field
      NativeParallelHashSet<Entity>.Enumerator enumerator = this.m_LazyUpdateSet.GetEnumerator();
      while (enumerator.MoveNext())
        writer.Write(enumerator.Current);
      enumerator.Dispose();
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LazyUpdateSet.Clear();
      int num;
      reader.Read(out num);
      for (int index = 0; index < num; ++index)
      {
        Entity entity;
        reader.Read(out entity);
        if (entity != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LazyUpdateSet.Add(entity);
        }
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LazyUpdateSet.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag1 = !this.m_DeletedLaneQuery.IsEmptyIgnoreFilter && !this.m_SegmentQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag2 = !this.m_AppliedLaneQuery.IsEmptyIgnoreFilter && !this.m_SegmentQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool flag3 = !this.m_UpdatedSegmentQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      if (!flag1 && !flag2 && !flag3 && this.m_LazyUpdateSet.IsEmpty)
        return;
      NativeQueue<Entity> nativeQueue1 = new NativeQueue<Entity>();
      NativeQueue<Entity> nativeQueue2 = new NativeQueue<Entity>();
      NativeParallelHashSet<Entity> nativeParallelHashSet1 = new NativeParallelHashSet<Entity>();
      JobHandle jobHandle1 = new JobHandle();
      JobHandle jobHandle2 = new JobHandle();
      if (flag1)
      {
        nativeQueue1 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        jobHandle1 = new RoutePathSystem.CheckRoutePathsJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_UpdateQueue = nativeQueue1.AsParallelWriter()
        }.ScheduleParallel<RoutePathSystem.CheckRoutePathsJob>(this.m_SegmentQuery, this.Dependency);
        JobHandle.ScheduleBatchedJobs();
      }
      if (flag2)
      {
        NativeParallelHashSet<RoutePathSystem.RoutePathType> nativeParallelHashSet2 = new NativeParallelHashSet<RoutePathSystem.RoutePathType>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        nativeQueue2 = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        RoutePathSystem.CheckAppliedLanesJob jobData1 = new RoutePathSystem.CheckAppliedLanesJob()
        {
          m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
          m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
          m_PedestrianLaneType = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_CarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
          m_TrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
          m_PathTypeSet = nativeParallelHashSet2
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RoutePathSystem.CheckSegmentRoutes jobData2 = new RoutePathSystem.CheckSegmentRoutes()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_RouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
          m_PathTypeSet = nativeParallelHashSet2,
          m_UpdateQueue = nativeQueue2.AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle3 = jobData1.Schedule<RoutePathSystem.CheckAppliedLanesJob>(this.m_AppliedLaneQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        EntityQuery segmentQuery = this.m_SegmentQuery;
        JobHandle dependsOn = jobHandle3;
        JobHandle inputDeps = jobData2.ScheduleParallel<RoutePathSystem.CheckSegmentRoutes>(segmentQuery, dependsOn);
        nativeParallelHashSet2.Dispose(inputDeps);
        jobHandle2 = inputDeps;
        JobHandle.ScheduleBatchedJobs();
      }
      if (flag1 | flag3)
        nativeParallelHashSet1 = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (flag3)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_UpdatedSegmentQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Segment> componentTypeHandle1 = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Owner> componentTypeHandle2 = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_PathTargets_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PathTargets> componentTypeHandle3 = this.__TypeHandle.__Game_Routes_PathTargets_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle4 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<RouteWaypoint> waypointRoBufferLookup = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<RouteLane> roComponentLookup1 = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Position> roComponentLookup2 = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<RouteConnectionData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
        this.Dependency.Complete();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<Segment> nativeArray2 = archetypeChunk.GetNativeArray<Segment>(ref componentTypeHandle1);
          NativeArray<Owner> nativeArray3 = archetypeChunk.GetNativeArray<Owner>(ref componentTypeHandle2);
          NativeArray<PathTargets> nativeArray4 = archetypeChunk.GetNativeArray<PathTargets>(ref componentTypeHandle3);
          NativeArray<PrefabRef> nativeArray5 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle5);
          bool highPriority = archetypeChunk.Has<Temp>(ref componentTypeHandle4);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            Segment segment = nativeArray2[index2];
            Owner owner = nativeArray3[index2];
            PathTargets pathTargets = nativeArray4[index2];
            PrefabRef prefabRef = nativeArray5[index2];
            if (waypointRoBufferLookup.HasBuffer(owner.m_Owner))
            {
              DynamicBuffer<RouteWaypoint> dynamicBuffer = waypointRoBufferLookup[owner.m_Owner];
              int index3 = segment.m_Index + 1;
              if (index3 == dynamicBuffer.Length)
                index3 = 0;
              Entity waypoint1 = dynamicBuffer[segment.m_Index].m_Waypoint;
              Entity waypoint2 = dynamicBuffer[index3].m_Waypoint;
              if (roComponentLookup1.HasComponent(waypoint1) && roComponentLookup1.HasComponent(waypoint2))
              {
                RouteLane startLane = roComponentLookup1[waypoint1];
                RouteLane endLane = roComponentLookup1[waypoint2];
                float2 float2 = new float2(startLane.m_EndCurvePos, endLane.m_StartCurvePos);
                if (!(pathTargets.m_StartLane == startLane.m_EndLane) || !(pathTargets.m_EndLane == endLane.m_StartLane) || !math.all(math.abs(pathTargets.m_CurvePositions - float2) < 1f / 1000f))
                {
                  pathTargets.m_StartLane = startLane.m_EndLane;
                  pathTargets.m_EndLane = endLane.m_StartLane;
                  pathTargets.m_CurvePositions = float2;
                  float3 position1 = roComponentLookup2[waypoint1].m_Position;
                  float3 position2 = roComponentLookup2[waypoint2].m_Position;
                  RouteConnectionData routeConnection = roComponentLookup3[prefabRef.m_Prefab];
                  // ISSUE: reference to a compiler-generated method
                  this.SetupPathfind(entity, position1, position2, startLane, endLane, routeConnection, highPriority);
                  nativeParallelHashSet1.Add(entity);
                  // ISSUE: reference to a compiler-generated field
                  this.m_LazyUpdateSet.Remove(entity);
                  nativeArray4[index2] = pathTargets;
                }
              }
            }
          }
        }
        archetypeChunkArray.Dispose();
      }
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<RouteWaypoint> waypointRoBufferLookup = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<RouteLane> roComponentLookup4 = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Position> roComponentLookup5 = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Segment> roComponentLookup6 = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Owner> roComponentLookup7 = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup8 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<RouteConnectionData> roComponentLookup9 = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
        jobHandle1.Complete();
        this.Dependency.Complete();
        Entity entity;
        while (nativeQueue1.TryDequeue(out entity))
        {
          if (nativeParallelHashSet1.Add(entity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LazyUpdateSet.Remove(entity);
            Segment segment = roComponentLookup6[entity];
            Owner owner = roComponentLookup7[entity];
            PrefabRef prefabRef = roComponentLookup8[entity];
            DynamicBuffer<RouteWaypoint> dynamicBuffer = waypointRoBufferLookup[owner.m_Owner];
            int index = segment.m_Index + 1;
            if (index == dynamicBuffer.Length)
              index = 0;
            Entity waypoint3 = dynamicBuffer[segment.m_Index].m_Waypoint;
            Entity waypoint4 = dynamicBuffer[index].m_Waypoint;
            RouteLane startLane = roComponentLookup4[waypoint3];
            RouteLane endLane = roComponentLookup4[waypoint4];
            float3 position3 = roComponentLookup5[waypoint3].m_Position;
            float3 position4 = roComponentLookup5[waypoint4].m_Position;
            RouteConnectionData routeConnection = roComponentLookup9[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated method
            this.SetupPathfind(entity, position3, position4, startLane, endLane, routeConnection, false);
          }
        }
      }
      if (flag2)
      {
        jobHandle2.Complete();
        Entity entity;
        while (nativeQueue2.TryDequeue(out entity))
        {
          if (!nativeParallelHashSet1.IsCreated || !nativeParallelHashSet1.Contains(entity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_LazyUpdateSet.Add(entity);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_LazyUpdateSet.IsEmpty && (!nativeParallelHashSet1.IsCreated || nativeParallelHashSet1.IsEmpty))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<RouteWaypoint> waypointRoBufferLookup = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<RouteLane> roComponentLookup10 = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Position> roComponentLookup11 = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Segment> roComponentLookup12 = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Owner> roComponentLookup13 = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Deleted> roComponentLookup14 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup15 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<RouteConnectionData> roComponentLookup16 = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
        this.Dependency.Complete();
        // ISSUE: reference to a compiler-generated field
        NativeParallelHashSet<Entity>.Enumerator enumerator = this.m_LazyUpdateSet.GetEnumerator();
        while (enumerator.MoveNext())
        {
          Entity current = enumerator.Current;
          // ISSUE: reference to a compiler-generated field
          this.m_LazyUpdateSet.Remove(current);
          enumerator.Dispose();
          // ISSUE: reference to a compiler-generated field
          enumerator = this.m_LazyUpdateSet.GetEnumerator();
          if (roComponentLookup12.HasComponent(current) && !roComponentLookup14.HasComponent(current))
          {
            Segment segment = roComponentLookup12[current];
            Owner owner = roComponentLookup13[current];
            PrefabRef prefabRef = roComponentLookup15[current];
            DynamicBuffer<RouteWaypoint> dynamicBuffer = waypointRoBufferLookup[owner.m_Owner];
            int index = segment.m_Index + 1;
            if (index == dynamicBuffer.Length)
              index = 0;
            Entity waypoint5 = dynamicBuffer[segment.m_Index].m_Waypoint;
            Entity waypoint6 = dynamicBuffer[index].m_Waypoint;
            RouteLane startLane = roComponentLookup10[waypoint5];
            RouteLane endLane = roComponentLookup10[waypoint6];
            float3 position5 = roComponentLookup11[waypoint5].m_Position;
            float3 position6 = roComponentLookup11[waypoint6].m_Position;
            RouteConnectionData routeConnection = roComponentLookup16[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated method
            this.SetupPathfind(current, position5, position6, startLane, endLane, routeConnection, false);
            break;
          }
        }
        enumerator.Dispose();
      }
      if (nativeQueue1.IsCreated)
        nativeQueue1.Dispose();
      if (nativeQueue2.IsCreated)
        nativeQueue2.Dispose();
      if (!nativeParallelHashSet1.IsCreated)
        return;
      nativeParallelHashSet1.Dispose();
    }

    private void SetupPathfind(
      Entity entity,
      float3 startPos,
      float3 endPos,
      RouteLane startLane,
      RouteLane endLane,
      RouteConnectionData routeConnection,
      bool highPriority)
    {
      PathfindParameters parameters = new PathfindParameters()
      {
        m_MaxSpeed = (float2) 277.777771f,
        m_WalkSpeed = (float2) 5.555556f,
        m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
        m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow,
        m_IgnoredRules = RuleFlags.HasBlockage | RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic,
        m_Methods = RouteUtils.GetPathMethods(routeConnection.m_RouteConnectionType, routeConnection.m_RouteTrackType, routeConnection.m_RouteRoadType)
      };
      if (routeConnection.m_RouteConnectionType != RouteConnectionType.Road || routeConnection.m_RouteRoadType != RoadTypes.Car)
        parameters.m_IgnoredRules |= RuleFlags.ForbidTransitTraffic;
      PathfindAction action = new PathfindAction(1, 1, Allocator.Persistent, parameters, SetupTargetType.None, SetupTargetType.None);
      action.data.m_StartTargets[0] = new PathTarget(startLane.m_EndLane, startLane.m_EndLane, startLane.m_EndCurvePos, 0.0f);
      action.data.m_EndTargets[0] = new PathTarget(endLane.m_StartLane, endLane.m_StartLane, endLane.m_StartCurvePos, 0.0f);
      PathEventData eventData = new PathEventData()
      {
        m_Position1 = startPos,
        m_Position2 = endPos
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindQueueSystem.Enqueue(action, entity, new JobHandle(), uint.MaxValue, (object) this, eventData, highPriority);
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
    public RoutePathSystem()
    {
    }

    public struct RoutePathType : IEquatable<RoutePathSystem.RoutePathType>
    {
      public RouteConnectionType m_ConnectionType;
      public RoadTypes m_RoadType;
      public TrackTypes m_TrackType;

      public bool Equals(RoutePathSystem.RoutePathType other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_ConnectionType == other.m_ConnectionType && this.m_RoadType == other.m_RoadType && this.m_TrackType == other.m_TrackType;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) ((TrackTypes) ((int) this.m_ConnectionType << 16 | (int) this.m_RoadType << 8) | this.m_TrackType);
      }
    }

    [BurstCompile]
    private struct CheckRoutePathsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      public NativeQueue<Entity>.ParallelWriter m_UpdateQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<PathElement> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeletedData.HasComponent(dynamicBuffer[index2].m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateQueue.Enqueue(nativeArray[index1]);
              break;
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
    private struct CheckAppliedLanesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.PedestrianLane> m_PedestrianLaneType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_TrackLaneData;
      public NativeParallelHashSet<RoutePathSystem.RoutePathType> m_PathTypeSet;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: variable of a compiler-generated type
        RoutePathSystem.RoutePathType routePathType1;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Net.CarLane>(ref this.m_CarLaneType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            CarLaneData carLaneData = this.m_CarLaneData[nativeArray[index].m_Prefab];
            if ((carLaneData.m_RoadTypes & RoadTypes.Car) != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Road;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_RoadType = RoadTypes.Car;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType2 = routePathType1;
              local.Add(routePathType2);
            }
            if ((carLaneData.m_RoadTypes & RoadTypes.Watercraft) != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Road;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_RoadType = RoadTypes.Watercraft;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType3 = routePathType1;
              local.Add(routePathType3);
            }
            if ((carLaneData.m_RoadTypes & RoadTypes.Helicopter) != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Road;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_RoadType = RoadTypes.Helicopter;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType4 = routePathType1;
              local.Add(routePathType4);
            }
            if ((carLaneData.m_RoadTypes & RoadTypes.Airplane) != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Road;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_RoadType = RoadTypes.Airplane;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType5 = routePathType1;
              local.Add(routePathType5);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Net.TrackLane>(ref this.m_TrackLaneType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            TrackLaneData trackLaneData = this.m_TrackLaneData[nativeArray[index].m_Prefab];
            if ((trackLaneData.m_TrackTypes & TrackTypes.Train) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Track;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_TrackType = TrackTypes.Train;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType6 = routePathType1;
              local.Add(routePathType6);
            }
            if ((trackLaneData.m_TrackTypes & TrackTypes.Tram) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Track;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_TrackType = TrackTypes.Tram;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType7 = routePathType1;
              local.Add(routePathType7);
            }
            if ((trackLaneData.m_TrackTypes & TrackTypes.Subway) != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local = ref this.m_PathTypeSet;
              // ISSUE: object of a compiler-generated type is created
              routePathType1 = new RoutePathSystem.RoutePathType();
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_ConnectionType = RouteConnectionType.Track;
              // ISSUE: reference to a compiler-generated field
              routePathType1.m_TrackType = TrackTypes.Subway;
              // ISSUE: variable of a compiler-generated type
              RoutePathSystem.RoutePathType routePathType8 = routePathType1;
              local.Add(routePathType8);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!chunk.Has<Game.Net.PedestrianLane>(ref this.m_PedestrianLaneType))
          return;
        // ISSUE: reference to a compiler-generated field
        ref NativeParallelHashSet<RoutePathSystem.RoutePathType> local1 = ref this.m_PathTypeSet;
        // ISSUE: object of a compiler-generated type is created
        routePathType1 = new RoutePathSystem.RoutePathType();
        // ISSUE: reference to a compiler-generated field
        routePathType1.m_ConnectionType = RouteConnectionType.Pedestrian;
        // ISSUE: variable of a compiler-generated type
        RoutePathSystem.RoutePathType routePathType9 = routePathType1;
        local1.Add(routePathType9);
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
    private struct CheckSegmentRoutes : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_RouteConnectionData;
      [ReadOnly]
      public NativeParallelHashSet<RoutePathSystem.RoutePathType> m_PathTypeSet;
      public NativeQueue<Entity>.ParallelWriter m_UpdateQueue;

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
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          RouteConnectionData componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteConnectionData.TryGetComponent(this.m_PrefabRefData[nativeArray2[index].m_Owner].m_Prefab, out componentData))
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            RoutePathSystem.RoutePathType routePathType = new RoutePathSystem.RoutePathType()
            {
              m_ConnectionType = componentData.m_RouteConnectionType,
              m_RoadType = componentData.m_RouteRoadType,
              m_TrackType = componentData.m_RouteTrackType
            };
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathTypeSet.Contains(routePathType))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdateQueue.Enqueue(nativeArray1[index]);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Segment> __Game_Routes_Segment_RO_ComponentTypeHandle;
      public ComponentTypeHandle<PathTargets> __Game_Routes_PathTargets_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathTargets_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathTargets>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
      }
    }
  }
}
