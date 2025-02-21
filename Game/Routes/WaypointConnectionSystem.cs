// Decompiled with JetBrains decompiler
// Type: Game.Routes.WaypointConnectionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
  public class WaypointConnectionSystem : GameSystemBase
  {
    private ModificationBarrier5 m_ModificationBarrier;
    private Game.Net.UpdateCollectSystem m_NetUpdateCollectSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private AirwaySystem m_AirwaySystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Areas.UpdateCollectSystem m_AreaUpdateCollectSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private SearchSystem m_RouteSearchSystem;
    private EntityQuery m_WaypointQuery;
    private EntityArchetype m_PathTargetEventArchetype;
    private WaypointConnectionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Net.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirwaySystem = this.World.GetOrCreateSystemManaged<AirwaySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Areas.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaypointQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Waypoint>(),
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<ConnectedRoute>()
        },
        None = new ComponentType[0]
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Waypoint>(),
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<ConnectedRoute>()
        },
        None = new ComponentType[0]
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PathTargetEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<PathTargetMoved>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_WaypointQuery.IsEmptyIgnoreFilter && !this.m_NetUpdateCollectSystem.netsUpdated && !this.m_AreaUpdateCollectSystem.lotsUpdated)
        return;
      NativeList<Entity> list = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_WaypointQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_ConnectedRoute_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        jobHandle1 = new WaypointConnectionSystem.UpdateWaypointReferencesJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_ConnectedType = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_AccessLaneType = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentTypeHandle,
          m_RouteLaneType = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
          m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_ConnectedRoutes = this.__TypeHandle.__Game_Routes_ConnectedRoute_RW_BufferLookup,
          m_UpdatedList = list
        }.Schedule<WaypointConnectionSystem.UpdateWaypointReferencesJob>(this.m_WaypointQuery, this.Dependency);
      }
      JobHandle jobHandle2 = jobHandle1;
      // ISSUE: variable of a compiler-generated type
      WaypointConnectionSystem.FindUpdatedWaypointsJob updatedWaypointsJob1;
      // ISSUE: variable of a compiler-generated type
      WaypointConnectionSystem.DequeUpdatedWaypointsJob updatedWaypointsJob2;
      // ISSUE: reference to a compiler-generated field
      if (this.m_NetUpdateCollectSystem.netsUpdated)
      {
        NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        JobHandle dependencies1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedNetBounds = this.m_NetUpdateCollectSystem.GetUpdatedNetBounds(out dependencies1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        updatedWaypointsJob1 = new WaypointConnectionSystem.FindUpdatedWaypointsJob();
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_Bounds = updatedNetBounds.AsDeferredJobArray();
        JobHandle dependencies2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        updatedWaypointsJob1.m_RouteSearchTree = this.m_RouteSearchSystem.GetSearchTree(true, out dependencies2);
        JobHandle dependencies3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        updatedWaypointsJob1.m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_AccessLaneData = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_ResultQueue = nativeQueue.AsParallelWriter();
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.FindUpdatedWaypointsJob jobData1 = updatedWaypointsJob1;
        // ISSUE: object of a compiler-generated type is created
        updatedWaypointsJob2 = new WaypointConnectionSystem.DequeUpdatedWaypointsJob();
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob2.m_UpdatedQueue = nativeQueue;
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob2.m_UpdatedList = list;
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.DequeUpdatedWaypointsJob jobData2 = updatedWaypointsJob2;
        JobHandle job1 = JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3);
        JobHandle jobHandle3 = jobData1.Schedule<WaypointConnectionSystem.FindUpdatedWaypointsJob, Bounds2>(updatedNetBounds, 1, JobHandle.CombineDependencies(this.Dependency, job1));
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, jobHandle3);
        jobHandle2 = jobData2.Schedule<WaypointConnectionSystem.DequeUpdatedWaypointsJob>(dependsOn);
        nativeQueue.Dispose(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetUpdateCollectSystem.AddNetBoundsReader(jobHandle3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_RouteSearchSystem.AddSearchTreeReader(jobHandle3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle3);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_AreaUpdateCollectSystem.lotsUpdated)
      {
        NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        JobHandle dependencies4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<Bounds2> updatedLotBounds = this.m_AreaUpdateCollectSystem.GetUpdatedLotBounds(out dependencies4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: object of a compiler-generated type is created
        updatedWaypointsJob1 = new WaypointConnectionSystem.FindUpdatedWaypointsJob();
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_Bounds = updatedLotBounds.AsDeferredJobArray();
        JobHandle dependencies5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        updatedWaypointsJob1.m_RouteSearchTree = this.m_RouteSearchSystem.GetSearchTree(true, out dependencies5);
        JobHandle dependencies6;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        updatedWaypointsJob1.m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies6);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_AccessLaneData = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob1.m_ResultQueue = nativeQueue.AsParallelWriter();
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.FindUpdatedWaypointsJob jobData3 = updatedWaypointsJob1;
        // ISSUE: object of a compiler-generated type is created
        updatedWaypointsJob2 = new WaypointConnectionSystem.DequeUpdatedWaypointsJob();
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob2.m_UpdatedQueue = nativeQueue;
        // ISSUE: reference to a compiler-generated field
        updatedWaypointsJob2.m_UpdatedList = list;
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.DequeUpdatedWaypointsJob jobData4 = updatedWaypointsJob2;
        JobHandle job1 = JobHandle.CombineDependencies(dependencies4, dependencies5, dependencies6);
        JobHandle jobHandle4 = jobData3.Schedule<WaypointConnectionSystem.FindUpdatedWaypointsJob, Bounds2>(updatedLotBounds, 1, JobHandle.CombineDependencies(this.Dependency, job1));
        JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, jobHandle4);
        jobHandle2 = jobData4.Schedule<WaypointConnectionSystem.DequeUpdatedWaypointsJob>(dependsOn);
        nativeQueue.Dispose(jobHandle2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddLotBoundsReader(jobHandle4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_RouteSearchSystem.AddSearchTreeReader(jobHandle4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle4);
      }
      NativeQueue<WaypointConnectionSystem.PathTargetInfo> nativeQueue1 = new NativeQueue<WaypointConnectionSystem.PathTargetInfo>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaypointConnectionSystem.RemoveDuplicatedWaypointsJob jobData5 = new WaypointConnectionSystem.RemoveDuplicatedWaypointsJob()
      {
        m_UpdatedList = list
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_AccessLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies7;
      JobHandle dependencies8;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaypointConnectionSystem.FindWaypointConnectionsJob jobData6 = new WaypointConnectionSystem.FindWaypointConnectionsJob()
      {
        m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabNetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabNetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_NetOutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_ObjectOutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentLookup,
        m_SurfaceData = this.__TypeHandle.__Game_Areas_Surface_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_Segments = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_Lanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_AccessLaneData = this.__TypeHandle.__Game_Routes_AccessLane_RW_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RW_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RW_ComponentLookup,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RW_ComponentLookup,
        m_UpdatedList = list.AsDeferredJobArray(),
        m_AirwayData = this.m_AirwaySystem.GetAirwayData(),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies7),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies8),
        m_PathTargetEventArchetype = this.m_PathTargetEventArchetype,
        m_PathTargetInfo = nativeQueue1.AsParallelWriter(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathTargets_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaypointConnectionSystem.ClearPathTargetsJob jobData7 = new WaypointConnectionSystem.ClearPathTargetsJob()
      {
        m_PathTargetInfo = nativeQueue1,
        m_PathTargetsData = this.__TypeHandle.__Game_Routes_PathTargets_RW_ComponentLookup
      };
      JobHandle job0 = jobData5.Schedule<WaypointConnectionSystem.RemoveDuplicatedWaypointsJob>(jobHandle2);
      JobHandle jobHandle5 = jobData6.Schedule<WaypointConnectionSystem.FindWaypointConnectionsJob, Entity>(list, 1, JobHandle.CombineDependencies(job0, dependencies7, dependencies8));
      JobHandle dependsOn1 = jobHandle5;
      JobHandle inputDeps = jobData7.Schedule<WaypointConnectionSystem.ClearPathTargetsJob>(dependsOn1);
      list.Dispose(jobHandle5);
      nativeQueue1.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle5);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle5);
      this.Dependency = inputDeps;
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
    public WaypointConnectionSystem()
    {
    }

    [BurstCompile]
    private struct UpdateWaypointReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Connected> m_ConnectedType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<AccessLane> m_AccessLaneType;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> m_RouteLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      public BufferLookup<ConnectedRoute> m_ConnectedRoutes;
      public NativeList<Entity> m_UpdatedList;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Connected> nativeArray2 = chunk.GetNativeArray<Connected>(ref this.m_ConnectedType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Temp>(ref this.m_TempType))
            return;
          if (nativeArray2.Length != 0)
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Entity waypoint = nativeArray1[index];
              Connected connected = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedRoutes.HasBuffer(connected.m_Connected))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.RemoveValue<ConnectedRoute>(this.m_ConnectedRoutes[connected.m_Connected], new ConnectedRoute(waypoint));
              }
            }
          }
          else
          {
            for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
            {
              Entity entity = nativeArray1[index1];
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedRoutes.HasBuffer(entity))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ConnectedRoute> connectedRoute = this.m_ConnectedRoutes[entity];
                for (int index2 = 0; index2 < connectedRoute.Length; ++index2)
                {
                  Entity waypoint = connectedRoute[index2].m_Waypoint;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ConnectedData.HasComponent(waypoint) && !this.m_DeletedData.HasComponent(waypoint))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_UpdatedList.Add(in waypoint);
                  }
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Created>(ref this.m_CreatedType) && !chunk.Has<Temp>(ref this.m_TempType))
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Entity waypoint = nativeArray1[index];
              Connected connected = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedRoutes.HasBuffer(connected.m_Connected))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ConnectedRoutes[connected.m_Connected].Add(new ConnectedRoute(waypoint));
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<AccessLane>(ref this.m_AccessLaneType) || chunk.Has<RouteLane>(ref this.m_RouteLaneType);
          for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
          {
            Entity entity = nativeArray1[index3];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedRoutes.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedRoute> connectedRoute = this.m_ConnectedRoutes[entity];
              for (int index4 = 0; index4 < connectedRoute.Length; ++index4)
              {
                Entity waypoint = connectedRoute[index4].m_Waypoint;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectedData.HasComponent(waypoint) && !this.m_DeletedData.HasComponent(waypoint))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_UpdatedList.Add(in waypoint);
                }
              }
            }
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedList.Add(nativeArray1[index3]);
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
    private struct FindUpdatedWaypointsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ> m_RouteSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<AccessLane> m_AccessLaneData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Bounds2 bounds2 = MathUtils.Expand(this.m_Bounds[index], (float2) 10f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.FindUpdatedWaypointsJob.RouteIterator iterator1 = new WaypointConnectionSystem.FindUpdatedWaypointsJob.RouteIterator()
        {
          m_Bounds = bounds2,
          m_WaypointData = this.m_WaypointData,
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_RouteSearchTree.Iterate<WaypointConnectionSystem.FindUpdatedWaypointsJob.RouteIterator>(ref iterator1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.FindUpdatedWaypointsJob.ObjectIterator iterator2 = new WaypointConnectionSystem.FindUpdatedWaypointsJob.ObjectIterator()
        {
          m_Bounds = bounds2,
          m_AccessLaneData = this.m_AccessLaneData,
          m_RouteLaneData = this.m_RouteLaneData,
          m_ResultQueue = this.m_ResultQueue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectSearchTree.Iterate<WaypointConnectionSystem.FindUpdatedWaypointsJob.ObjectIterator>(ref iterator2);
      }

      private struct RouteIterator : 
        INativeQuadTreeIterator<RouteSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<RouteSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Waypoint> m_WaypointData;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, RouteSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_WaypointData.HasComponent(item.m_Entity))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(item.m_Entity);
        }
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<AccessLane> m_AccessLaneData;
        public ComponentLookup<RouteLane> m_RouteLaneData;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_AccessLaneData.HasComponent(item) && !this.m_RouteLaneData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(item);
        }
      }
    }

    [BurstCompile]
    private struct DequeUpdatedWaypointsJob : IJob
    {
      public NativeQueue<Entity> m_UpdatedQueue;
      public NativeList<Entity> m_UpdatedList;

      public void Execute()
      {
        Entity updated;
        // ISSUE: reference to a compiler-generated field
        while (this.m_UpdatedQueue.TryDequeue(out updated))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdatedList.Add(in updated);
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> updatedList = this.m_UpdatedList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.DequeUpdatedWaypointsJob.EntityComparer entityComparer = new WaypointConnectionSystem.DequeUpdatedWaypointsJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.DequeUpdatedWaypointsJob.EntityComparer comp = entityComparer;
        updatedList.Sort<Entity, WaypointConnectionSystem.DequeUpdatedWaypointsJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num = 0;
        int index = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_UpdatedList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          updated = this.m_UpdatedList[num++];
          if (updated != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdatedList[index++] = updated;
            entity = updated;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index >= this.m_UpdatedList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedList.RemoveRange(index, this.m_UpdatedList.Length - index);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct RemoveDuplicatedWaypointsJob : IJob
    {
      public NativeList<Entity> m_UpdatedList;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdatedList.Length < 2)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> updatedList = this.m_UpdatedList;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.RemoveDuplicatedWaypointsJob.EntityComparer entityComparer = new WaypointConnectionSystem.RemoveDuplicatedWaypointsJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.RemoveDuplicatedWaypointsJob.EntityComparer comp = entityComparer;
        updatedList.Sort<Entity, WaypointConnectionSystem.RemoveDuplicatedWaypointsJob.EntityComparer>(comp);
        Entity entity = Entity.Null;
        int num = 0;
        int index = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_UpdatedList.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity updated = this.m_UpdatedList[num++];
          if (updated != entity)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdatedList[index++] = updated;
            entity = updated;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index >= this.m_UpdatedList.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpdatedList.RemoveRange(index, this.m_UpdatedList.Length - index);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct FindWaypointConnectionsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.OutsideConnection> m_NetOutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_ObjectOutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> m_LotData;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Surface> m_SurfaceData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_Segments;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_Lanes;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<AccessLane> m_AccessLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Connected> m_ConnectedData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public NativeArray<Entity> m_UpdatedList;
      [ReadOnly]
      public AirwayHelpers.AirwayData m_AirwayData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      [ReadOnly]
      public EntityArchetype m_PathTargetEventArchetype;
      public NativeQueue<WaypointConnectionSystem.PathTargetInfo>.ParallelWriter m_PathTargetInfo;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity updated = this.m_UpdatedList[index];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[updated];
        bool flag1 = false;
        bool onGround = false;
        float elevation1 = 0.0f;
        float3 float3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PositionData.HasComponent(updated))
        {
          // ISSUE: reference to a compiler-generated field
          float3 = this.m_PositionData[updated].m_Position;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TransformData.HasComponent(updated))
            throw new Exception("FindWaypointConnectionsJob: Position not found!");
          // ISSUE: reference to a compiler-generated field
          float3 = this.m_TransformData[updated].m_Position;
          flag1 = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.HasComponent(updated))
          {
            // ISSUE: reference to a compiler-generated field
            elevation1 = this.m_ElevationData[updated].m_Elevation;
          }
          else
            onGround = true;
        }
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        Connected connected = new Connected();
        Entity entity1 = Entity.Null;
        Entity entity2 = Entity.Null;
        Entity parent = Entity.Null;
        Entity lane = Entity.Null;
        Entity laneOwner1 = Entity.Null;
        AccessLane other1 = new AccessLane();
        bool intersectLot = false;
        Entity laneContainer;
        Entity masterLot;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.HasComponent(updated))
        {
          // ISSUE: reference to a compiler-generated field
          connected = this.m_ConnectedData[updated];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (connected.m_Connected != Entity.Null && (!this.m_PrefabRefData.HasComponent(connected.m_Connected) || this.m_DeletedData.HasComponent(connected.m_Connected)))
          {
            connected.m_Connected = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            this.m_ConnectedData[updated] = connected;
          }
          // ISSUE: reference to a compiler-generated method
          laneContainer = this.GetLaneContainer(connected.m_Connected);
          // ISSUE: reference to a compiler-generated method
          masterLot = this.GetMasterLot(connected.m_Connected);
          Attached componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachedData.TryGetComponent(connected.m_Connected, out componentData))
            parent = componentData.m_Parent;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          laneContainer = this.GetLaneContainer(updated);
          // ISSUE: reference to a compiler-generated method
          masterLot = this.GetMasterLot(updated);
          Attached componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachedData.TryGetComponent(updated, out componentData))
            parent = componentData.m_Parent;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(connected.m_Connected))
        {
          // ISSUE: reference to a compiler-generated field
          float3 = this.m_TransformData[connected.m_Connected].m_Position;
          flag1 = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.HasComponent(connected.m_Connected))
          {
            // ISSUE: reference to a compiler-generated field
            elevation1 = this.m_ElevationData[connected.m_Connected].m_Elevation;
          }
          else
            onGround = true;
        }
        RouteConnectionData componentData1;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabConnectionData.TryGetComponent(prefabRef1.m_Prefab, out componentData1);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(connected.m_Connected))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef2 = this.m_PrefabRefData[connected.m_Connected];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabConnectionData.HasComponent(prefabRef2.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            RouteConnectionData routeConnectionData = this.m_PrefabConnectionData[prefabRef2.m_Prefab];
            componentData1.m_StartLaneOffset = math.max(componentData1.m_StartLaneOffset, routeConnectionData.m_StartLaneOffset);
            componentData1.m_EndMargin = math.max(componentData1.m_EndMargin, routeConnectionData.m_EndMargin);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AccessLaneData.HasComponent(updated))
        {
          // ISSUE: reference to a compiler-generated field
          AccessLane accessLane = this.m_AccessLaneData[updated];
          other1 = accessLane;
          bool flag5 = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(connected.m_Connected))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef3 = this.m_PrefabRefData[connected.m_Connected];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabSpawnLocationData.HasComponent(prefabRef3.m_Prefab) && this.m_PrefabConnectionData.HasComponent(prefabRef3.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.SpawnLocationData spawnLocationData = this.m_PrefabSpawnLocationData[prefabRef3.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              RouteConnectionData routeConnectionData = this.m_PrefabConnectionData[prefabRef3.m_Prefab];
              if (spawnLocationData.m_ConnectionType != RouteConnectionType.None && spawnLocationData.m_ConnectionType == routeConnectionData.m_AccessConnectionType && spawnLocationData.m_ActivityMask.m_Mask == 0U)
              {
                accessLane.m_Lane = connected.m_Connected;
                accessLane.m_CurvePos = 0.0f;
                flag5 = false;
              }
            }
          }
          if (flag5)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindLane(laneContainer, float3, elevation1, componentData1.m_AccessConnectionType, componentData1.m_AccessTrackType, componentData1.m_AccessRoadType, Entity.Null, parent, masterLot, onGround, 0, out laneOwner1, out accessLane.m_Lane, out accessLane.m_CurvePos, out intersectLot);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_AccessLaneData[updated] = accessLane;
          flag2 = !accessLane.Equals(other1);
          // ISSUE: reference to a compiler-generated field
          flag3 = flag2 || this.m_UpdatedData.HasComponent(accessLane.m_Lane);
          lane = accessLane.m_Lane;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_RouteLaneData.HasComponent(updated))
        {
          // ISSUE: reference to a compiler-generated field
          RouteLane routeLane = this.m_RouteLaneData[updated];
          RouteLane other2 = routeLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(connected.m_Connected))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef4 = this.m_PrefabRefData[connected.m_Connected];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabConnectionData.HasComponent(prefabRef4.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              RouteConnectionData routeConnectionData = this.m_PrefabConnectionData[prefabRef4.m_Prefab];
              componentData1.m_StartLaneOffset = math.max(componentData1.m_StartLaneOffset, routeConnectionData.m_StartLaneOffset);
              componentData1.m_EndMargin = math.max(componentData1.m_EndMargin, routeConnectionData.m_EndMargin);
            }
          }
          float3 position = float3;
          float elevation2 = elevation1;
          // ISSUE: reference to a compiler-generated field
          if (componentData1.m_RouteConnectionType == RouteConnectionType.Air && componentData1.m_RouteRoadType == RoadTypes.Airplane && lane != Entity.Null && !this.m_ConnectionLaneData.HasComponent(lane))
          {
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[lane];
            float2 xz = (curve.m_Bezier.a + curve.m_Bezier.d - position * 2f).xz;
            if (MathUtils.TryNormalize(ref xz, 1500f))
            {
              position.xz -= xz;
              elevation2 = 1000f;
            }
          }
          Entity entity3 = Entity.Null;
          int shouldIntersectLot = 0;
          if (componentData1.m_RouteConnectionType == componentData1.m_AccessConnectionType)
          {
            switch (componentData1.m_RouteConnectionType)
            {
              case RouteConnectionType.Road:
              case RouteConnectionType.Cargo:
              case RouteConnectionType.Air:
                if (componentData1.m_AccessRoadType == componentData1.m_RouteRoadType)
                {
                  entity3 = laneOwner1;
                  break;
                }
                break;
              case RouteConnectionType.Track:
                if (componentData1.m_AccessTrackType == componentData1.m_RouteTrackType)
                {
                  entity3 = laneOwner1;
                  break;
                }
                break;
              default:
                entity3 = laneOwner1;
                shouldIntersectLot = math.select(1, -1, intersectLot);
                break;
            }
          }
          Entity laneOwner2;
          // ISSUE: reference to a compiler-generated method
          this.FindLane(laneContainer, position, elevation2, componentData1.m_RouteConnectionType, componentData1.m_RouteTrackType, componentData1.m_RouteRoadType, entity3, parent, masterLot, onGround, shouldIntersectLot, out laneOwner2, out routeLane.m_EndLane, out routeLane.m_EndCurvePos, out bool _);
          routeLane.m_StartLane = routeLane.m_EndLane;
          routeLane.m_StartCurvePos = routeLane.m_EndCurvePos;
          Game.Net.CarLane componentData2;
          // ISSUE: reference to a compiler-generated field
          if ((double) componentData1.m_StartLaneOffset > 0.0 && this.m_CarLaneData.TryGetComponent(routeLane.m_EndLane, out componentData2) && (componentData2.m_Flags & CarLaneFlags.Twoway) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
            componentData1.m_StartLaneOffset = 0.0f;
          if ((double) componentData1.m_StartLaneOffset > 0.0 || (double) componentData1.m_EndMargin > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.MoveLaneOffsets(ref routeLane.m_StartLane, ref routeLane.m_StartCurvePos, ref routeLane.m_EndLane, ref routeLane.m_EndCurvePos, componentData1.m_StartLaneOffset, componentData1.m_EndMargin);
          }
          // ISSUE: reference to a compiler-generated method
          if (lane != Entity.Null && routeLane.m_EndLane != Entity.Null && !this.ValidateConnection(lane, routeLane.m_EndLane, entity3, laneOwner2))
          {
            // ISSUE: reference to a compiler-generated method
            Entity ownerBuilding1 = this.GetOwnerBuilding(updated);
            // ISSUE: reference to a compiler-generated method
            Entity ownerBuilding2 = this.GetOwnerBuilding(lane);
            // ISSUE: reference to a compiler-generated method
            Entity ownerBuilding3 = this.GetOwnerBuilding(routeLane.m_EndLane);
            if (ownerBuilding1 == ownerBuilding2 && ownerBuilding1 == ownerBuilding3)
            {
              // ISSUE: reference to a compiler-generated field
              AccessLane accessLane = this.m_AccessLaneData[updated] with
              {
                m_Lane = Entity.Null,
                m_CurvePos = 0.0f
              };
              // ISSUE: reference to a compiler-generated field
              this.m_AccessLaneData[updated] = accessLane;
              flag2 = !accessLane.Equals(other1);
              flag3 = flag2;
            }
            routeLane.m_StartLane = Entity.Null;
            routeLane.m_EndLane = Entity.Null;
            routeLane.m_StartCurvePos = 0.0f;
            routeLane.m_EndCurvePos = 0.0f;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_RouteLaneData[updated] = routeLane;
          flag2 |= !routeLane.Equals(other2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          flag3 = ((flag3 ? 1 : 0) | (flag2 || this.m_UpdatedData.HasComponent(routeLane.m_StartLane) ? 1 : (this.m_UpdatedData.HasComponent(routeLane.m_EndLane) ? 1 : 0))) != 0;
          // ISSUE: reference to a compiler-generated field
          if (!flag1 && this.m_CurveData.HasComponent(routeLane.m_EndLane))
          {
            // ISSUE: reference to a compiler-generated field
            float3 = MathUtils.Position(this.m_CurveData[routeLane.m_EndLane].m_Bezier, routeLane.m_EndCurvePos);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PositionData.HasComponent(updated))
        {
          // ISSUE: reference to a compiler-generated field
          Position position = this.m_PositionData[updated];
          if ((double) math.distance(float3, position.m_Position) > 0.10000000149011612)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_TempData.HasComponent(updated))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity4 = this.m_CommandBuffer.CreateEntity(index, this.m_PathTargetEventArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<PathTargetMoved>(index, entity4, new PathTargetMoved(updated, position.m_Position, float3));
            }
            position.m_Position = float3;
            // ISSUE: reference to a compiler-generated field
            this.m_PositionData[updated] = position;
            flag2 = true;
            flag4 = true;
          }
        }
        if (flag2)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaypointData.HasComponent(updated))
          {
            // ISSUE: reference to a compiler-generated field
            Waypoint waypoint = this.m_WaypointData[updated];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<RouteSegment> segment = this.m_Segments[this.m_OwnerData[updated].m_Owner];
            int index1 = math.select(waypoint.m_Index - 1, segment.Length - 1, waypoint.m_Index == 0);
            RouteSegment routeSegment1 = segment[index1];
            RouteSegment routeSegment2 = segment[waypoint.m_Index];
            // ISSUE: variable of a compiler-generated type
            WaypointConnectionSystem.PathTargetInfo pathTargetInfo1;
            if (routeSegment1.m_Segment != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, routeSegment1.m_Segment, new Updated());
              if (flag4)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeQueue<WaypointConnectionSystem.PathTargetInfo>.ParallelWriter local = ref this.m_PathTargetInfo;
                // ISSUE: object of a compiler-generated type is created
                pathTargetInfo1 = new WaypointConnectionSystem.PathTargetInfo();
                // ISSUE: reference to a compiler-generated field
                pathTargetInfo1.m_Segment = routeSegment1.m_Segment;
                // ISSUE: reference to a compiler-generated field
                pathTargetInfo1.m_Start = false;
                // ISSUE: variable of a compiler-generated type
                WaypointConnectionSystem.PathTargetInfo pathTargetInfo2 = pathTargetInfo1;
                local.Enqueue(pathTargetInfo2);
              }
            }
            if (routeSegment2.m_Segment != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, routeSegment2.m_Segment, new Updated());
              if (flag4)
              {
                // ISSUE: reference to a compiler-generated field
                ref NativeQueue<WaypointConnectionSystem.PathTargetInfo>.ParallelWriter local = ref this.m_PathTargetInfo;
                // ISSUE: object of a compiler-generated type is created
                pathTargetInfo1 = new WaypointConnectionSystem.PathTargetInfo();
                // ISSUE: reference to a compiler-generated field
                pathTargetInfo1.m_Segment = routeSegment2.m_Segment;
                // ISSUE: reference to a compiler-generated field
                pathTargetInfo1.m_Start = true;
                // ISSUE: variable of a compiler-generated type
                WaypointConnectionSystem.PathTargetInfo pathTargetInfo3 = pathTargetInfo1;
                local.Enqueue(pathTargetInfo3);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(index, updated, new Updated());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_TransformData.HasComponent(updated) || !this.m_OwnerData.HasComponent(updated) || this.m_TempData.HasComponent(updated))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateSurfaces(index, this.m_TransformData[updated].m_Position);
        }
        else
        {
          if (!flag3)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(index, updated, new PathfindUpdated());
        }
      }

      private Entity GetOwnerBuilding(Entity entity)
      {
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out componentData) && !this.m_BuildingData.HasComponent(entity))
          entity = componentData.m_Owner;
        return entity;
      }

      private void UpdateSurfaces(int jobIndex, float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaypointConnectionSystem.FindWaypointConnectionsJob.SurfaceIterator iterator = new WaypointConnectionSystem.FindWaypointConnectionsJob.SurfaceIterator()
        {
          m_Position = position,
          m_JobIndex = jobIndex,
          m_SurfaceData = this.m_SurfaceData,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<WaypointConnectionSystem.FindWaypointConnectionsJob.SurfaceIterator>(ref iterator);
      }

      private bool ValidateConnection(
        Entity accessLaneEntity,
        Entity routeLaneEntity,
        Entity accessLaneOwner,
        Entity routeLaneOwner)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneData.HasComponent(accessLaneEntity) && this.m_LaneData.HasComponent(routeLaneEntity))
        {
          // ISSUE: reference to a compiler-generated field
          Lane lane1 = this.m_LaneData[accessLaneEntity];
          // ISSUE: reference to a compiler-generated field
          Lane lane2 = this.m_LaneData[routeLaneEntity];
          if (lane1.m_StartNode.EqualsIgnoreCurvePos(lane2.m_StartNode) || lane1.m_StartNode.EqualsIgnoreCurvePos(lane2.m_MiddleNode) || lane1.m_StartNode.EqualsIgnoreCurvePos(lane2.m_EndNode) || lane1.m_MiddleNode.EqualsIgnoreCurvePos(lane2.m_StartNode) || lane1.m_MiddleNode.EqualsIgnoreCurvePos(lane2.m_EndNode) || lane1.m_EndNode.EqualsIgnoreCurvePos(lane2.m_StartNode) || lane1.m_EndNode.EqualsIgnoreCurvePos(lane2.m_MiddleNode) || lane1.m_EndNode.EqualsIgnoreCurvePos(lane2.m_EndNode))
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(accessLaneOwner))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge = this.m_EdgeData[accessLaneOwner];
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          if (!this.ValidateConnectedEdges(edge.m_Start, routeLaneOwner) || !this.ValidateConnectedEdges(edge.m_End, routeLaneOwner))
            return false;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_ConnectedEdges.HasBuffer(accessLaneOwner) && !this.ValidateConnectedEdges(accessLaneOwner, routeLaneOwner))
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(routeLaneOwner))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.Edge edge = this.m_EdgeData[routeLaneOwner];
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          if (!this.ValidateConnectedEdges(edge.m_Start, accessLaneOwner) || !this.ValidateConnectedEdges(edge.m_End, accessLaneOwner))
            return false;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_ConnectedEdges.HasBuffer(routeLaneOwner) && !this.ValidateConnectedEdges(routeLaneOwner, accessLaneOwner))
            return false;
        }
        return true;
      }

      private bool ValidateConnectedEdges(Entity node, Entity other)
      {
        if (node == other)
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        for (int index = 0; index < connectedEdge.Length; ++index)
        {
          if (connectedEdge[index].m_Edge == other)
            return false;
        }
        return true;
      }

      private Entity GetLaneContainer(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Owner owner = this.m_OwnerData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetOutsideConnectionData.HasComponent(owner.m_Owner) && this.m_Lanes.HasBuffer(owner.m_Owner))
            return owner.m_Owner;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_ObjectOutsideConnectionData.HasComponent(entity) && this.m_Lanes.HasBuffer(entity) ? entity : Entity.Null;
      }

      private Entity GetMasterLot(Entity entity)
      {
        Entity masterLot = Entity.Null;
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out componentData))
        {
          entity = componentData.m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LotData.HasComponent(entity))
            masterLot = entity;
        }
        return masterLot;
      }

      private void MoveLaneOffsets(
        ref Entity startLane,
        ref float startCurvePos,
        ref Entity endLane,
        ref float endCurvePos,
        float startOffset,
        float endMargin)
      {
        Curve curve;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurveData.TryGetComponent(endLane, out curve))
          return;
        Entity prevLane = Entity.Null;
        Entity nextLane = Entity.Null;
        Curve prevCurve = new Curve();
        Curve nextCurve = new Curve();
        Owner componentData1;
        Lane componentData2;
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(endLane, out componentData1) && this.m_LaneData.TryGetComponent(endLane, out componentData2) && this.m_Lanes.TryGetBuffer(componentData1.m_Owner, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity subLane = bufferData[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane];
            if (lane.m_EndNode.Equals(componentData2.m_StartNode))
            {
              prevLane = subLane;
              // ISSUE: reference to a compiler-generated field
              prevCurve = this.m_CurveData[subLane];
              if (nextLane != Entity.Null)
                break;
            }
            else if (lane.m_StartNode.Equals(componentData2.m_EndNode))
            {
              nextLane = subLane;
              // ISSUE: reference to a compiler-generated field
              nextCurve = this.m_CurveData[subLane];
              if (prevLane != Entity.Null)
                break;
            }
          }
        }
        float prevDistance = MathUtils.Length(curve.m_Bezier.xz, new Bounds1(0.0f, endCurvePos));
        float totalPrevDistance = prevDistance;
        if (prevLane != Entity.Null)
          totalPrevDistance += MathUtils.Length(prevCurve.m_Bezier.xz);
        float nextDistance = MathUtils.Length(curve.m_Bezier.xz, new Bounds1(endCurvePos, 1f));
        float totalNextDistance = nextDistance;
        if (nextLane != Entity.Null)
          totalNextDistance += MathUtils.Length(nextCurve.m_Bezier.xz);
        float num1 = math.max(startOffset, endMargin);
        float num2 = endMargin;
        float offset = 0.0f;
        if ((double) num1 + (double) num2 > (double) totalPrevDistance + (double) totalNextDistance)
          offset = totalNextDistance - (float) ((double) num2 * ((double) totalPrevDistance + (double) totalNextDistance) / ((double) num1 + (double) num2));
        else if ((double) num1 > (double) totalPrevDistance)
          offset = num1 - totalPrevDistance;
        else if ((double) num2 > (double) totalNextDistance)
          offset = totalNextDistance - num2;
        MoveLaneOffset(offset, ref endLane, ref endCurvePos);
        if ((double) startOffset == 0.0)
        {
          startLane = endLane;
          startCurvePos = endCurvePos;
        }
        else
        {
          startOffset = offset - startOffset;
          MoveLaneOffset(startOffset, ref startLane, ref startCurvePos);
        }

        void MoveLaneOffset(float offset, ref Entity lane, ref float curvePos)
        {
          if ((double) offset < 0.0)
          {
            if ((double) offset <= -(double) totalPrevDistance)
            {
              curvePos = 0.0f;
              if (!(prevLane != Entity.Null))
                return;
              lane = prevLane;
            }
            else if ((double) offset <= -(double) prevDistance)
            {
              curvePos = 0.0f;
              offset += prevDistance;
              if ((double) offset >= 0.0 || !(prevLane != Entity.Null))
                return;
              lane = prevLane;
              Bounds1 t = new Bounds1(0.0f, 1f);
              if (!MathUtils.ClampLengthInverse(prevCurve.m_Bezier.xz, ref t, -offset))
                return;
              curvePos = t.min;
            }
            else
            {
              Bounds1 t = new Bounds1(0.0f, curvePos);
              if (MathUtils.ClampLengthInverse(curve.m_Bezier.xz, ref t, -offset))
                curvePos = t.min;
              else
                curvePos = 0.0f;
            }
          }
          else
          {
            if ((double) offset <= 0.0)
              return;
            if ((double) offset >= (double) totalNextDistance)
            {
              curvePos = 1f;
              if (!(nextLane != Entity.Null))
                return;
              lane = nextLane;
            }
            else if ((double) offset >= (double) nextDistance)
            {
              curvePos = 1f;
              offset -= nextDistance;
              if ((double) offset <= 0.0 || !(nextLane != Entity.Null))
                return;
              lane = nextLane;
              Bounds1 t = new Bounds1(0.0f, 1f);
              if (!MathUtils.ClampLength(nextCurve.m_Bezier.xz, ref t, offset))
                return;
              curvePos = t.max;
            }
            else
            {
              Bounds1 t = new Bounds1(curvePos, 1f);
              if (MathUtils.ClampLength(curve.m_Bezier.xz, ref t, offset))
                curvePos = t.max;
              else
                curvePos = 1f;
            }
          }
        }
      }

      private void FindLane(
        Entity laneContainer,
        float3 position,
        float elevation,
        RouteConnectionType connectionType,
        TrackTypes trackTypes,
        RoadTypes roadTypes,
        Entity ignoreOwner,
        Entity preferOwner,
        Entity masterLot,
        bool onGround,
        int shouldIntersectLot,
        out Entity laneOwner,
        out Entity lane,
        out float curvePos,
        out bool intersectLot)
      {
        if (connectionType == RouteConnectionType.Air)
        {
          laneOwner = Entity.Null;
          lane = Entity.Null;
          curvePos = 0.0f;
          intersectLot = false;
          float maxValue = float.MaxValue;
          if ((roadTypes & RoadTypes.Helicopter) != RoadTypes.None)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AirwayData.helicopterMap.FindClosestLane(position, this.m_CurveData, ref lane, ref curvePos, ref maxValue);
          }
          if ((roadTypes & RoadTypes.Airplane) == RoadTypes.None)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AirwayData.airplaneMap.FindClosestLane(position, this.m_CurveData, ref lane, ref curvePos, ref maxValue);
        }
        else if (laneContainer != Entity.Null && laneContainer != ignoreOwner)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane1 = this.m_Lanes[laneContainer];
          float num1 = float.MaxValue;
          laneOwner = laneContainer;
          lane = Entity.Null;
          curvePos = 0.0f;
          intersectLot = false;
          for (int index = 0; index < lane1.Length; ++index)
          {
            Entity subLane = lane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane];
              switch (connectionType)
              {
                case RouteConnectionType.Road:
                  if ((connectionLane.m_Flags & (ConnectionLaneFlags.Road | ConnectionLaneFlags.AllowMiddle)) != (ConnectionLaneFlags.Road | ConnectionLaneFlags.AllowMiddle) || (connectionLane.m_RoadTypes & roadTypes) == RoadTypes.None)
                    continue;
                  break;
                case RouteConnectionType.Pedestrian:
                  if ((connectionLane.m_Flags & (ConnectionLaneFlags.Pedestrian | ConnectionLaneFlags.AllowMiddle)) == (ConnectionLaneFlags.Pedestrian | ConnectionLaneFlags.AllowMiddle))
                    break;
                  continue;
                case RouteConnectionType.Track:
                  if ((connectionLane.m_Flags & (ConnectionLaneFlags.Track | ConnectionLaneFlags.AllowMiddle)) != (ConnectionLaneFlags.Track | ConnectionLaneFlags.AllowMiddle) || (connectionLane.m_TrackTypes & trackTypes) == TrackTypes.None)
                    continue;
                  break;
                case RouteConnectionType.Cargo:
                  if ((connectionLane.m_Flags & (ConnectionLaneFlags.AllowMiddle | ConnectionLaneFlags.AllowCargo)) != (ConnectionLaneFlags.AllowMiddle | ConnectionLaneFlags.AllowCargo))
                    continue;
                  break;
                default:
                  continue;
              }
              float t;
              // ISSUE: reference to a compiler-generated field
              float num2 = MathUtils.Distance(this.m_CurveData[subLane].m_Bezier, position, out t);
              t = math.select(t, 1f, (connectionLane.m_Flags & ConnectionLaneFlags.Start) != 0);
              if ((double) num2 < (double) num1)
              {
                num1 = num2;
                lane = subLane;
                curvePos = t;
              }
            }
          }
        }
        else
        {
          float num3 = 10f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
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
          WaypointConnectionSystem.FindWaypointConnectionsJob.LaneIterator iterator = new WaypointConnectionSystem.FindWaypointConnectionsJob.LaneIterator()
          {
            m_Bounds = new Bounds3(position - num3, position + num3),
            m_Position = position,
            m_ConnectionType = connectionType,
            m_TrackType = trackTypes,
            m_CarType = roadTypes,
            m_OnGround = onGround,
            m_CmpDistance = num3,
            m_MaxDistance = num3,
            m_Elevation = elevation,
            m_IgnoreOwner = ignoreOwner,
            m_IgnoreConnected = preferOwner,
            m_MasterLot = masterLot,
            m_OwnerData = this.m_OwnerData,
            m_PedestrianLaneData = this.m_PedestrianLaneData,
            m_CarLaneData = this.m_CarLaneData,
            m_TrackLaneData = this.m_TrackLaneData,
            m_MasterLaneData = this.m_MasterLaneData,
            m_SlaveLaneData = this.m_SlaveLaneData,
            m_ConnectionLaneData = this.m_ConnectionLaneData,
            m_CurveData = this.m_CurveData,
            m_EdgeData = this.m_EdgeData,
            m_ElevationData = this.m_NetElevationData,
            m_EdgeGeometryData = this.m_EdgeGeometryData,
            m_StartNodeGeometryData = this.m_StartNodeGeometryData,
            m_EndNodeGeometryData = this.m_EndNodeGeometryData,
            m_CompositionData = this.m_CompositionData,
            m_LotData = this.m_LotData,
            m_PrefabRefData = this.m_PrefabRefData,
            m_PrefabTrackLaneData = this.m_PrefabTrackLaneData,
            m_PrefabCarLaneData = this.m_PrefabCarLaneData,
            m_PrefabNetLaneData = this.m_PrefabNetLaneData,
            m_PrefabNetData = this.m_PrefabNetData,
            m_PrefabNetCompositionData = this.m_PrefabNetCompositionData,
            m_Lanes = this.m_Lanes,
            m_ConnectedEdges = this.m_ConnectedEdges,
            m_AreaNodes = this.m_AreaNodes,
            m_AreaTriangles = this.m_AreaTriangles
          };
          Entity entity = ignoreOwner;
          if (entity != Entity.Null)
          {
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(entity, out componentData) && !this.m_BuildingData.HasComponent(entity))
              entity = componentData.m_Owner;
          }
          float num4 = float.MaxValue;
          PrefabRef componentData1;
          Transform componentData2;
          BuildingData componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (entity != Entity.Null && this.m_PrefabRefData.TryGetComponent(entity, out componentData1) && this.m_TransformData.TryGetComponent(entity, out componentData2) && this.m_PrefabBuildingData.TryGetComponent(componentData1.m_Prefab, out componentData3))
          {
            float2 size = (float2) componentData3.m_LotSize * 8f;
            // ISSUE: reference to a compiler-generated field
            iterator.m_LotQuad = ObjectUtils.CalculateBaseCorners(componentData2.m_Position, componentData2.m_Rotation, size).xz;
            // ISSUE: reference to a compiler-generated field
            iterator.m_CheckLot = new bool4(true, (componentData3.m_Flags & Game.Prefabs.BuildingFlags.LeftAccess) > (Game.Prefabs.BuildingFlags) 0, (componentData3.m_Flags & Game.Prefabs.BuildingFlags.BackAccess) > (Game.Prefabs.BuildingFlags) 0, (componentData3.m_Flags & Game.Prefabs.BuildingFlags.RightAccess) > (Game.Prefabs.BuildingFlags) 0);
            float4 maxValue = (float4) float.MaxValue;
            float t;
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_CheckLot.x)
            {
              // ISSUE: reference to a compiler-generated field
              maxValue.x = MathUtils.Distance(iterator.m_LotQuad.ab, position.xz, out t);
            }
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_CheckLot.y)
            {
              // ISSUE: reference to a compiler-generated field
              maxValue.y = MathUtils.Distance(iterator.m_LotQuad.bc, position.xz, out t);
            }
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_CheckLot.z)
            {
              // ISSUE: reference to a compiler-generated field
              maxValue.z = MathUtils.Distance(iterator.m_LotQuad.cd, position.xz, out t);
            }
            // ISSUE: reference to a compiler-generated field
            if (iterator.m_CheckLot.w)
            {
              // ISSUE: reference to a compiler-generated field
              maxValue.w = MathUtils.Distance(iterator.m_LotQuad.da, position.xz, out t);
            }
            num4 = math.cmin(maxValue);
            // ISSUE: reference to a compiler-generated field
            iterator.m_CheckLot &= maxValue <= num4 * 2f;
          }
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<WaypointConnectionSystem.FindWaypointConnectionsJob.LaneIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          if (math.any(iterator.m_CheckLot))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            iterator.m_Bounds = new Bounds3(position - iterator.m_CmpDistance, position + iterator.m_CmpDistance);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_AreaSearchTree.Iterate<WaypointConnectionSystem.FindWaypointConnectionsJob.LaneIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          if (shouldIntersectLot != 0 && math.any(iterator.m_CheckLot))
          {
            if (shouldIntersectLot == -1)
            {
              // ISSUE: reference to a compiler-generated field
              if (iterator.m_IntersectLot)
              {
                // ISSUE: object of a compiler-generated type is created
                iterator = new WaypointConnectionSystem.FindWaypointConnectionsJob.LaneIterator();
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!iterator.m_IntersectLot && (double) iterator.m_MaxDistance > (double) num4)
              {
                // ISSUE: object of a compiler-generated type is created
                iterator = new WaypointConnectionSystem.FindWaypointConnectionsJob.LaneIterator();
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          laneOwner = iterator.m_ResultOwner;
          // ISSUE: reference to a compiler-generated field
          lane = iterator.m_ResultLane;
          // ISSUE: reference to a compiler-generated field
          curvePos = iterator.m_ResultCurvePos;
          // ISSUE: reference to a compiler-generated field
          intersectLot = iterator.m_IntersectLot;
        }
      }

      private struct SurfaceIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public float3 m_Position;
        public int m_JobIndex;
        public ComponentLookup<Game.Areas.Surface> m_SurfaceData;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position.xz) || !this.m_SurfaceData.HasComponent(item.m_Area) || !MathUtils.Intersect(AreaUtils.GetTriangle2(this.m_AreaNodes[item.m_Area], this.m_AreaTriangles[item.m_Area][item.m_Triangle]), this.m_Position.xz))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(this.m_JobIndex, item.m_Area, new Updated());
        }
      }

      private struct LaneIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public float3 m_Position;
        public RouteConnectionType m_ConnectionType;
        public TrackTypes m_TrackType;
        public RoadTypes m_CarType;
        public bool m_OnGround;
        public float m_CmpDistance;
        public float m_MaxDistance;
        public float m_Elevation;
        public Quad2 m_LotQuad;
        public bool4 m_CheckLot;
        public Entity m_IgnoreOwner;
        public Entity m_IgnoreConnected;
        public Entity m_MasterLot;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
        public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
        public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
        public ComponentLookup<MasterLane> m_MasterLaneData;
        public ComponentLookup<SlaveLane> m_SlaveLaneData;
        public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
        public ComponentLookup<Curve> m_CurveData;
        public ComponentLookup<Game.Net.Edge> m_EdgeData;
        public ComponentLookup<Game.Net.Elevation> m_ElevationData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<Game.Areas.Lot> m_LotData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
        public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
        public ComponentLookup<NetLaneData> m_PrefabNetLaneData;
        public ComponentLookup<NetData> m_PrefabNetData;
        public ComponentLookup<NetCompositionData> m_PrefabNetCompositionData;
        public BufferLookup<Game.Net.SubLane> m_Lanes;
        public BufferLookup<ConnectedEdge> m_ConnectedEdges;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;
        public Entity m_ResultLane;
        public Entity m_ResultOwner;
        public float m_ResultCurvePos;
        public bool m_IntersectLot;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) || !this.m_Lanes.HasBuffer(item.m_Area) || item.m_Area == this.m_IgnoreOwner)
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> areaNode = this.m_AreaNodes[item.m_Area];
          // ISSUE: reference to a compiler-generated field
          Triangle triangle = this.m_AreaTriangles[item.m_Area][item.m_Triangle];
          Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, triangle);
          float3 elevations = AreaUtils.GetElevations(areaNode, triangle);
          // ISSUE: reference to a compiler-generated field
          bool flag1 = this.m_LotData.HasComponent(item.m_Area);
          float2 t1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num1 = flag1 || this.m_OnGround && math.any(elevations == float.MinValue) ? MathUtils.Distance(triangle3.xz, this.m_Position.xz, out t1) : MathUtils.Distance(triangle3, this.m_Position, out t1);
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 >= (double) this.m_CmpDistance)
            return;
          float3 float3 = MathUtils.Position(triangle3, t1);
          bool flag2 = false;
          // ISSUE: reference to a compiler-generated field
          if (math.any(this.m_CheckLot))
          {
            // ISSUE: reference to a compiler-generated field
            Line3.Segment segment = new Line3.Segment(this.m_Position, float3);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            flag2 = ((((((((flag2 ? 1 : 0) | (!this.m_CheckLot.x ? 0 : (MathUtils.Intersect(this.m_LotQuad.ab, segment.xz, out t1) ? 1 : 0))) != 0 ? 1 : 0) | (!this.m_CheckLot.y ? 0 : (MathUtils.Intersect(this.m_LotQuad.bc, segment.xz, out t1) ? 1 : 0))) != 0 ? 1 : 0) | (!this.m_CheckLot.z ? 0 : (MathUtils.Intersect(this.m_LotQuad.cd, segment.xz, out t1) ? 1 : 0))) != 0 ? 1 : 0) | (!this.m_CheckLot.w ? 0 : (MathUtils.Intersect(this.m_LotQuad.da, segment.xz, out t1) ? 1 : 0))) != 0;
          }
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (flag1 && (this.m_MasterLot == Entity.Null || item.m_Area != this.m_MasterLot && (!this.m_OwnerData.TryGetComponent(item.m_Area, out componentData1) || componentData1.m_Owner != this.m_MasterLot)))
          {
            bool3 bool3 = AreaUtils.IsEdge(areaNode, triangle) & (math.cmin(triangle.m_Indices.xy) != 0 | math.cmax(triangle.m_Indices.xy) != 1) & (math.cmin(triangle.m_Indices.yz) != 0 | math.cmax(triangle.m_Indices.yz) != 1) & (math.cmin(triangle.m_Indices.zx) != 0 | math.cmax(triangle.m_Indices.zx) != 1);
            float t2;
            if (bool3.x && (double) MathUtils.Distance(triangle3.ab, float3, out t2) < 0.10000000149011612 || bool3.y && (double) MathUtils.Distance(triangle3.bc, float3, out t2) < 0.10000000149011612 || bool3.z && (double) MathUtils.Distance(triangle3.ca, float3, out t2) < 0.10000000149011612)
              return;
          }
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> lane = this.m_Lanes[item.m_Area];
          Entity entity = Entity.Null;
          float num2 = float.MaxValue;
          float num3 = 0.0f;
          for (int index = 0; index < lane.Length; ++index)
          {
            Entity subLane = lane[index].m_SubLane;
            Game.Net.ConnectionLane componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (this.m_ConnectionLaneData.TryGetComponent(subLane, out componentData2) && this.CheckLaneType(componentData2))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane];
              float2 t3;
              bool2 x = new bool2(MathUtils.Intersect(triangle3.xz, curve.m_Bezier.a.xz, out t3), MathUtils.Intersect(triangle3.xz, curve.m_Bezier.d.xz, out t3));
              if (math.any(x))
              {
                float t4;
                float num4 = MathUtils.Distance(curve.m_Bezier, float3, out t4);
                if ((double) num4 < (double) num2)
                {
                  float2 float2 = math.select(new float2(0.0f, 0.49f), math.select(new float2(0.51f, 1f), new float2(0.0f, 1f), x.x), x.y);
                  num2 = num4;
                  entity = subLane;
                  num3 = math.clamp(t4, float2.x, float2.y);
                }
              }
            }
          }
          if (!(entity != Entity.Null))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CmpDistance = num1;
          // ISSUE: reference to a compiler-generated field
          this.m_MaxDistance = num1;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultLane = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultOwner = item.m_Area;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultCurvePos = num3;
          // ISSUE: reference to a compiler-generated field
          this.m_IntersectLot = flag2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Bounds = new Bounds3(this.m_Position - num1, this.m_Position + num1);
        }

        private bool CheckLaneType(Game.Net.ConnectionLane connectionLane)
        {
          // ISSUE: reference to a compiler-generated field
          switch (this.m_ConnectionType)
          {
            case RouteConnectionType.Road:
            case RouteConnectionType.Cargo:
              // ISSUE: reference to a compiler-generated field
              return (connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0 && (connectionLane.m_RoadTypes & this.m_CarType) != RoadTypes.None;
            case RouteConnectionType.Pedestrian:
              return (connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0;
            case RouteConnectionType.Track:
              // ISSUE: reference to a compiler-generated field
              return (connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0 && (connectionLane.m_TrackTypes & this.m_TrackType) != TrackTypes.None;
            default:
              return false;
          }
        }

        private void CheckDistance(
          EdgeGeometry edgeGeometry,
          ref float maxDistance,
          ref int crossedSide,
          ref float3 maxDistancePos)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.DistanceSquared(edgeGeometry.m_Bounds.xz, this.m_Position.xz) >= (double) maxDistance * (double) maxDistance)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckDistance(edgeGeometry.m_Start.m_Left, -1, ref maxDistance, ref crossedSide, ref maxDistancePos);
          // ISSUE: reference to a compiler-generated method
          this.CheckDistance(edgeGeometry.m_Start.m_Right, 1, ref maxDistance, ref crossedSide, ref maxDistancePos);
          // ISSUE: reference to a compiler-generated method
          this.CheckDistance(edgeGeometry.m_End.m_Left, -1, ref maxDistance, ref crossedSide, ref maxDistancePos);
          // ISSUE: reference to a compiler-generated method
          this.CheckDistance(edgeGeometry.m_End.m_Right, 1, ref maxDistance, ref crossedSide, ref maxDistancePos);
        }

        private void CheckDistance(
          EdgeNodeGeometry nodeGeometry,
          ref float maxDistance,
          ref int crossedSide,
          ref float3 maxDistancePos)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.DistanceSquared(nodeGeometry.m_Bounds.xz, this.m_Position.xz) >= (double) maxDistance * (double) maxDistance)
            return;
          if ((double) nodeGeometry.m_MiddleRadius > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckDistance(nodeGeometry.m_Left.m_Left, -1, ref maxDistance, ref crossedSide, ref maxDistancePos);
            // ISSUE: reference to a compiler-generated method
            this.CheckDistance(nodeGeometry.m_Left.m_Right, 1, ref maxDistance, ref crossedSide, ref maxDistancePos);
            // ISSUE: reference to a compiler-generated method
            this.CheckDistance(nodeGeometry.m_Right.m_Left, -2, ref maxDistance, ref crossedSide, ref maxDistancePos);
            // ISSUE: reference to a compiler-generated method
            this.CheckDistance(nodeGeometry.m_Right.m_Right, 2, ref maxDistance, ref crossedSide, ref maxDistancePos);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckDistance(nodeGeometry.m_Left.m_Left, -1, ref maxDistance, ref crossedSide, ref maxDistancePos);
            // ISSUE: reference to a compiler-generated method
            this.CheckDistance(nodeGeometry.m_Right.m_Right, 1, ref maxDistance, ref crossedSide, ref maxDistancePos);
          }
        }

        private void CheckDistance(
          Bezier4x3 curve,
          int side,
          ref float maxDistance,
          ref int crossedSide,
          ref float3 maxDistancePos)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.DistanceSquared(MathUtils.Bounds(curve.xz), this.m_Position.xz) >= (double) maxDistance * (double) maxDistance)
            return;
          float t;
          // ISSUE: reference to a compiler-generated field
          float num = MathUtils.Distance(curve.xz, this.m_Position.xz, out t);
          if ((double) num >= (double) maxDistance)
            return;
          maxDistance = num;
          maxDistancePos = MathUtils.Position(curve, t);
          float2 forward = MathUtils.Tangent(curve.xz, t);
          float2 x = math.select(MathUtils.Left(forward), MathUtils.Right(forward), side > 0);
          // ISSUE: reference to a compiler-generated field
          float2 y = this.m_Position.xz - maxDistancePos.xz;
          crossedSide = math.select(0, side, (double) math.dot(x, y) > 0.0);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity netEntity)
        {
          DynamicBuffer<Game.Net.SubLane> bufferData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds.xz) || !this.m_Lanes.TryGetBuffer(netEntity, out bufferData1) || netEntity == this.m_IgnoreOwner)
            return;
          // ISSUE: reference to a compiler-generated field
          float maxDistance = this.m_MaxDistance;
          float3 maxDistancePos = new float3();
          int crossedSide = 0;
          int num1 = -1;
          bool flag1 = false;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectionType == RouteConnectionType.Pedestrian)
          {
            EdgeGeometry componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeGeometryData.TryGetComponent(netEntity, out componentData1))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckDistance(componentData1, ref maxDistance, ref crossedSide, ref maxDistancePos);
              flag1 = true;
            }
            else
            {
              DynamicBuffer<ConnectedEdge> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedEdges.TryGetBuffer(netEntity, out bufferData2))
              {
                for (int index = 0; index < bufferData2.Length; ++index)
                {
                  ConnectedEdge connectedEdge = bufferData2[index];
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.Edge edge = this.m_EdgeData[connectedEdge.m_Edge];
                  float num2 = maxDistance;
                  if (edge.m_Start == netEntity)
                  {
                    StartNodeGeometry componentData2;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_StartNodeGeometryData.TryGetComponent(connectedEdge.m_Edge, out componentData2))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CheckDistance(componentData2.m_Geometry, ref maxDistance, ref crossedSide, ref maxDistancePos);
                    }
                  }
                  else
                  {
                    EndNodeGeometry componentData3;
                    // ISSUE: reference to a compiler-generated field
                    if (edge.m_End == netEntity && this.m_EndNodeGeometryData.TryGetComponent(connectedEdge.m_Edge, out componentData3))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CheckDistance(componentData3.m_Geometry, ref maxDistance, ref crossedSide, ref maxDistancePos);
                    }
                  }
                  num1 = math.select(num1, index, (double) maxDistance != (double) num2);
                }
              }
            }
          }
          float num3 = 0.0f;
          bool flag2 = false;
          Game.Net.Elevation componentData4;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.TryGetComponent(netEntity, out componentData4))
          {
            flag2 = true;
            num3 = !flag1 || crossedSide == 0 ? math.csum(componentData4.m_Elevation) * 0.5f : math.select(componentData4.m_Elevation.x, componentData4.m_Elevation.y, crossedSide > 0);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (crossedSide != 0 && (this.m_PrefabNetData[this.m_PrefabRefData[netEntity].m_Prefab].m_RequiredLayers & (Layer.Road | Layer.TrainTrack | Layer.Pathway | Layer.TramTrack | Layer.SubwayTrack | Layer.PublicTransportRoad)) != Layer.None)
            {
              Entity entity = Entity.Null;
              if (flag1)
              {
                Composition componentData5;
                // ISSUE: reference to a compiler-generated field
                if (this.m_CompositionData.TryGetComponent(netEntity, out componentData5))
                  entity = componentData5.m_Edge;
              }
              else
              {
                DynamicBuffer<ConnectedEdge> bufferData3;
                // ISSUE: reference to a compiler-generated field
                if (num1 != -1 && this.m_ConnectedEdges.TryGetBuffer(netEntity, out bufferData3))
                {
                  ConnectedEdge connectedEdge = bufferData3[num1];
                  Composition componentData6;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CompositionData.TryGetComponent(netEntity, out componentData6))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Game.Net.Edge edge = this.m_EdgeData[connectedEdge.m_Edge];
                    if (edge.m_Start == netEntity)
                      entity = componentData6.m_StartNode;
                    else if (edge.m_End == netEntity)
                      entity = componentData6.m_EndNode;
                  }
                }
              }
              NetCompositionData componentData7;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabNetCompositionData.TryGetComponent(entity, out componentData7))
              {
                CompositionFlags.Side side = crossedSide > 0 ? componentData7.m_Flags.m_Right : componentData7.m_Flags.m_Left;
                if ((componentData7.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) != (CompositionFlags.General) 0)
                {
                  if (math.abs(crossedSide) == 1 || (side & CompositionFlags.Side.HighTransition) == (CompositionFlags.Side) 0)
                    return;
                }
                else if ((side & (CompositionFlags.Side.Raised | CompositionFlags.Side.Lowered)) != (CompositionFlags.Side) 0 && (math.abs(crossedSide) == 1 || (side & CompositionFlags.Side.LowTransition) == (CompositionFlags.Side) 0))
                  return;
              }
            }
          }
          Entity entity1 = Entity.Null;
          float4 float4_1 = (float4) float.MaxValue;
          float4 float4_2 = (float4) float.MaxValue;
          float num4 = 0.0f;
          Bounds3 bounds3 = new Bounds3();
          bool flag3 = false;
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            Entity subLane = bufferData1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            switch (this.m_ConnectionType)
            {
              case RouteConnectionType.Road:
                Game.Net.CarLane componentData8;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_CarLaneData.TryGetComponent(subLane, out componentData8) || this.m_MasterLaneData.HasComponent(subLane) || (componentData8.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || (this.m_PrefabCarLaneData[this.m_PrefabRefData[subLane].m_Prefab].m_RoadTypes & this.m_CarType) == RoadTypes.None)
                  continue;
                break;
              case RouteConnectionType.Pedestrian:
                Game.Net.PedestrianLane componentData9;
                // ISSUE: reference to a compiler-generated field
                if (!this.m_PedestrianLaneData.TryGetComponent(subLane, out componentData9) || (componentData9.m_Flags & (PedestrianLaneFlags.AllowMiddle | PedestrianLaneFlags.OnWater)) != PedestrianLaneFlags.AllowMiddle)
                  continue;
                break;
              case RouteConnectionType.Track:
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_TrackLaneData.HasComponent(subLane) || (this.m_PrefabTrackLaneData[this.m_PrefabRefData[subLane].m_Prefab].m_TrackTypes & this.m_TrackType) == TrackTypes.None)
                  continue;
                break;
              default:
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[subLane];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData = this.m_PrefabNetLaneData[this.m_PrefabRefData[subLane].m_Prefab];
            float num5 = netLaneData.m_Width * 0.5f;
            // ISSUE: reference to a compiler-generated field
            if ((double) maxDistance == (double) this.m_MaxDistance)
            {
              // ISSUE: reference to a compiler-generated field
              if (!MathUtils.Intersect(MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), (float3) num5).xz, this.m_Bounds.xz))
                continue;
            }
            else if (entity1 != Entity.Null && !MathUtils.Intersect(MathUtils.Expand(MathUtils.Bounds(curve.m_Bezier), (float3) num5).xz, bounds3.xz))
              continue;
            bool flag4 = (netLaneData.m_Flags & LaneFlags.OnWater) != 0;
            float t1;
            // ISSUE: reference to a compiler-generated field
            float4 float4_3 = (float4) MathUtils.Distance(curve.m_Bezier.xz, this.m_Position.xz, out t1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float2 float2_1 = new float2(this.m_Position.y - MathUtils.Position(curve.m_Bezier.y, t1), this.m_Elevation - num3);
            // ISSUE: reference to a compiler-generated field
            float2_1 = math.select(float2_1, (float2) 0.0f, ((!this.m_OnGround ? 0 : (!flag2 ? 1 : 0)) | (flag4 ? 1 : 0)) != 0);
            float num6 = math.max(0.0f, num5 - float4_3.x) * 0.1f / math.max(0.01f, num5);
            float4_3.x = math.max(0.0f, float4_3.x - num5) - num6;
            float4_3.y = math.cmin(math.abs(float2_1));
            float4_3.z = math.length(math.max((float2) 0.0f, float4_3.xy));
            float4_3.w = float4_3.z + math.min(0.0f, float4_3.x);
            bool flag5 = false;
            float4 float4_4 = float4_3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (math.any(this.m_CheckLot) && ((double) float4_3.x < 10.0 || (double) maxDistance < (double) this.m_MaxDistance && (double) maxDistance < 10.0))
            {
              // ISSUE: reference to a compiler-generated field
              Line2.Segment segment = new Line2.Segment(this.m_Position.xz, MathUtils.Position(curve.m_Bezier, t1).xz);
              Line2.Segment line = new Line2.Segment();
              float num7 = 1f;
              bool flag6 = false;
              float2 t2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CheckLot.x && MathUtils.Intersect(this.m_LotQuad.ab, segment, out t2) && (double) t2.y < (double) num7)
              {
                // ISSUE: reference to a compiler-generated field
                line = this.m_LotQuad.ab;
                num7 = t2.y;
                flag6 = false;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CheckLot.y && MathUtils.Intersect(this.m_LotQuad.bc, segment, out t2) && (double) t2.y < (double) num7)
              {
                // ISSUE: reference to a compiler-generated field
                line = this.m_LotQuad.bc;
                num7 = t2.y;
                flag6 = true;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CheckLot.z && MathUtils.Intersect(this.m_LotQuad.cd, segment, out t2) && (double) t2.y < (double) num7)
              {
                // ISSUE: reference to a compiler-generated field
                line = this.m_LotQuad.cd;
                num7 = t2.y;
                flag6 = false;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CheckLot.w && MathUtils.Intersect(this.m_LotQuad.da, segment, out t2) && (double) t2.y < (double) num7)
              {
                // ISSUE: reference to a compiler-generated field
                line = this.m_LotQuad.da;
                num7 = t2.y;
                flag6 = true;
              }
              if ((double) num7 != 1.0)
              {
                // ISSUE: reference to a compiler-generated field
                float num8 = MathUtils.Distance(line, this.m_Position.xz, out t2.x);
                flag5 = true;
                if ((double) num8 < (double) float4_3.x)
                {
                  float2 float2_2 = MathUtils.Position(line, t2.x);
                  float4_4.x = MathUtils.Distance(curve.m_Bezier.xz, float2_2, out t1);
                  float3 float3 = MathUtils.Position(curve.m_Bezier, t1);
                  // ISSUE: reference to a compiler-generated field
                  float2 x1 = math.normalizesafe(float3.xz - this.m_Position.xz);
                  // ISSUE: reference to a compiler-generated field
                  float2 x2 = maxDistancePos.xz - this.m_Position.xz;
                  float2 t3 = new float2();
                  float2 defaultvalue = t3;
                  float2 y = math.normalizesafe(x2, defaultvalue);
                  float s = math.saturate(math.dot(x1, y));
                  segment = new Line2.Segment(float2_2, maxDistancePos.xz + MathUtils.ClampLength(maxDistancePos.xz - float2_2, 1f));
                  if (flag6)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (MathUtils.Intersect(segment, (Line2) this.m_LotQuad.ab, out t3) || MathUtils.Intersect(segment, (Line2) this.m_LotQuad.cd, out t3))
                      continue;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (MathUtils.Intersect(segment, (Line2) this.m_LotQuad.bc, out t3) || MathUtils.Intersect(segment, (Line2) this.m_LotQuad.da, out t3))
                      continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float2_1 = new float2(this.m_Position.y - float3.y, this.m_Elevation - num3);
                  // ISSUE: reference to a compiler-generated field
                  float2_1 = math.select(float2_1, (float2) 0.0f, ((!this.m_OnGround ? 0 : (!flag2 ? 1 : 0)) | (flag4 ? 1 : 0)) != 0);
                  float num9 = math.max(0.0f, num5 - float4_4.x) * 0.1f / math.max(0.01f, num5);
                  float4_4.x = math.max(0.0f, float4_4.x - num5) - num9;
                  float4_4.x = num8 + float4_4.x * math.lerp(1f, 0.01f, s);
                  float4_4.y = math.cmin(math.abs(float2_1));
                  float4_4.z = math.length(math.max((float2) 0.0f, float4_4.xy));
                  float4_4.w = float4_4.z + math.min(0.0f, float4_4.x);
                }
              }
            }
            if ((double) float4_4.w < (double) float4_2.w)
            {
              entity1 = subLane;
              float4_1 = float4_3;
              float4_2 = float4_4;
              num4 = t1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bounds3 = new Bounds3(this.m_Position - float4_3.z, this.m_Position + float4_3.z);
              flag3 = flag5;
            }
          }
          if (!(entity1 != Entity.Null))
            return;
          // ISSUE: reference to a compiler-generated field
          if ((double) maxDistance < (double) float4_1.x && (double) maxDistance < (double) this.m_MaxDistance)
          {
            float4_1.x = maxDistance;
            float4_1.z = math.length(math.max((float2) 0.0f, float4_1.xy));
            float4_1.w = float4_1.z + math.min(0.0f, float4_1.x);
            if (!flag3)
              float4_2 = float4_1;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if ((double) float4_2.w >= (double) this.m_CmpDistance || this.DirectlyConnected(netEntity, this.m_IgnoreOwner) || this.m_IgnoreConnected != Entity.Null && netEntity != this.m_IgnoreConnected && this.DirectlyConnected(netEntity, this.m_IgnoreConnected))
            return;
          SlaveLane componentData10;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectionType == RouteConnectionType.Road && this.m_SlaveLaneData.TryGetComponent(entity1, out componentData10))
            entity1 = bufferData1[(int) componentData10.m_MasterIndex].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          this.m_CmpDistance = float4_2.w;
          // ISSUE: reference to a compiler-generated field
          this.m_MaxDistance = float4_1.x;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultLane = entity1;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultOwner = netEntity;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultCurvePos = num4;
          // ISSUE: reference to a compiler-generated field
          this.m_IntersectLot = flag3;
          // ISSUE: reference to a compiler-generated field
          if (math.any(this.m_CheckLot))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Bounds = new Bounds3(this.m_Position - float4_1.z, this.m_Position + float4_1.z);
        }

        private bool DirectlyConnected(Entity netEntity1, Entity netEntity2)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeData.HasComponent(netEntity1))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(netEntity2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge1 = this.m_EdgeData[netEntity1];
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge2 = this.m_EdgeData[netEntity2];
              if (edge1.m_Start == edge2.m_Start || edge1.m_Start == edge2.m_End || edge1.m_End == edge2.m_Start || edge1.m_End == edge2.m_End)
                return true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge = this.m_EdgeData[netEntity1];
              if (edge.m_Start == netEntity2 || edge.m_End == netEntity2)
                return true;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(netEntity2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge = this.m_EdgeData[netEntity2];
              if (edge.m_Start == netEntity1 || edge.m_End == netEntity1)
                return true;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ConnectedEdges.HasBuffer(netEntity1))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[netEntity1];
                for (int index = 0; index < connectedEdge.Length; ++index)
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Net.Edge edge = this.m_EdgeData[connectedEdge[index].m_Edge];
                  if ((!(edge.m_Start != netEntity1) || !(edge.m_End != netEntity1)) && (edge.m_Start == netEntity2 || edge.m_End == netEntity2))
                    return false;
                }
              }
            }
          }
          return false;
        }
      }
    }

    private struct PathTargetInfo
    {
      public Entity m_Segment;
      public bool m_Start;
    }

    [BurstCompile]
    private struct ClearPathTargetsJob : IJob
    {
      public NativeQueue<WaypointConnectionSystem.PathTargetInfo> m_PathTargetInfo;
      public ComponentLookup<PathTargets> m_PathTargetsData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_PathTargetInfo.Count;
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          WaypointConnectionSystem.PathTargetInfo pathTargetInfo = this.m_PathTargetInfo.Dequeue();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathTargetsData.HasComponent(pathTargetInfo.m_Segment))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PathTargets pathTargets = this.m_PathTargetsData[pathTargetInfo.m_Segment];
            // ISSUE: reference to a compiler-generated field
            if (pathTargetInfo.m_Start)
            {
              pathTargets.m_StartLane = Entity.Null;
              pathTargets.m_CurvePositions.x = 0.0f;
            }
            else
            {
              pathTargets.m_EndLane = Entity.Null;
              pathTargets.m_CurvePositions.y = 0.0f;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_PathTargetsData[pathTargetInfo.m_Segment] = pathTargets;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Connected> __Game_Routes_Connected_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AccessLane> __Game_Routes_AccessLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> __Game_Routes_RouteLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccessLane> __Game_Routes_AccessLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Lot> __Game_Areas_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Areas.Surface> __Game_Areas_Surface_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      public ComponentLookup<AccessLane> __Game_Routes_AccessLane_RW_ComponentLookup;
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RW_ComponentLookup;
      public ComponentLookup<Connected> __Game_Routes_Connected_RW_ComponentLookup;
      public ComponentLookup<Position> __Game_Routes_Position_RW_ComponentLookup;
      public ComponentLookup<PathTargets> __Game_Routes_PathTargets_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AccessLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RW_BufferLookup = state.GetBufferLookup<ConnectedRoute>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RO_ComponentLookup = state.GetComponentLookup<AccessLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Net.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Surface_RO_ComponentLookup = state.GetComponentLookup<Game.Areas.Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RW_ComponentLookup = state.GetComponentLookup<AccessLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RW_ComponentLookup = state.GetComponentLookup<RouteLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RW_ComponentLookup = state.GetComponentLookup<Connected>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RW_ComponentLookup = state.GetComponentLookup<Position>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathTargets_RW_ComponentLookup = state.GetComponentLookup<PathTargets>();
      }
    }
  }
}
