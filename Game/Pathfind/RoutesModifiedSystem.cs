// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.RoutesModifiedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class RoutesModifiedSystem : GameSystemBase
  {
    private PathfindQueueSystem m_PathfindQueueSystem;
    private EntityQuery m_CreatedSubElementQuery;
    private EntityQuery m_UpdatedSubElementQuery;
    private EntityQuery m_DeletedSubElementQuery;
    private EntityQuery m_AllSubElementQuery;
    private bool m_Loaded;
    private RoutesModifiedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedSubElementQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Created>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<Game.Routes.Segment>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedSubElementQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<Game.Routes.Segment>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PathfindUpdated>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<Game.Routes.Segment>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedSubElementQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<Game.Routes.Segment>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllSubElementQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<AccessLane>(),
          ComponentType.ReadOnly<RouteLane>(),
          ComponentType.ReadOnly<Game.Routes.Segment>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
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
      EntityQuery entityQuery;
      int size;
      // ISSUE: reference to a compiler-generated method
      if (this.GetLoaded())
      {
        // ISSUE: reference to a compiler-generated field
        entityQuery = this.m_AllSubElementQuery;
        size = 0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        entityQuery = this.m_CreatedSubElementQuery;
        // ISSUE: reference to a compiler-generated field
        size = this.m_UpdatedSubElementQuery.CalculateEntityCount();
      }
      int entityCount1 = entityQuery.CalculateEntityCount();
      // ISSUE: reference to a compiler-generated field
      int entityCount2 = this.m_DeletedSubElementQuery.CalculateEntityCount();
      if (entityCount1 == 0 && size == 0 && entityCount2 == 0)
        return;
      JobHandle job0 = this.Dependency;
      if (entityCount1 != 0)
      {
        CreateAction action = new CreateAction(entityCount1, Allocator.Persistent);
        JobHandle outJobHandle;
        NativeList<ArchetypeChunk> archetypeChunkListAsync = entityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TransportLine_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new RoutesModifiedSystem.AddPathEdgeJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_WaypointType = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle,
          m_PositionType = this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_AccessLaneType = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentTypeHandle,
          m_RouteLaneType = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle,
          m_SegmentType = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle,
          m_TaxiStandType = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle,
          m_TakeoffLocationType = this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentTypeHandle,
          m_ConnectedType = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle,
          m_RouteInfoType = this.__TypeHandle.__Game_Routes_RouteInfo_RO_ComponentTypeHandle,
          m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle,
          m_WaitingPassengersType = this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle,
          m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
          m_TransportStopData = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
          m_TransportLineData = this.__TypeHandle.__Game_Routes_TransportLine_RO_ComponentLookup,
          m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
          m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
          m_Waypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
          m_PrefabTransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
          m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
          m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
          m_TransportPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup,
          m_PedestrianPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup,
          m_CarPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup,
          m_TrackPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup,
          m_ConnectionPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup,
          m_Actions = action.m_CreateData
        }.Schedule<RoutesModifiedSystem.AddPathEdgeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
        archetypeChunkListAsync.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PathfindQueueSystem.Enqueue(action, jobHandle);
      }
      if (size != 0)
      {
        UpdateAction action = new UpdateAction(size, Allocator.Persistent);
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpdatedSubElementQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TransportLine_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new RoutesModifiedSystem.UpdatePathEdgeJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_WaypointType = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentTypeHandle,
          m_PositionType = this.__TypeHandle.__Game_Routes_Position_RO_ComponentTypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_AccessLaneType = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentTypeHandle,
          m_RouteLaneType = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentTypeHandle,
          m_SegmentType = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentTypeHandle,
          m_TaxiStandType = this.__TypeHandle.__Game_Routes_TaxiStand_RO_ComponentTypeHandle,
          m_TakeoffLocationType = this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentTypeHandle,
          m_ConnectedType = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentTypeHandle,
          m_RouteInfoType = this.__TypeHandle.__Game_Routes_RouteInfo_RO_ComponentTypeHandle,
          m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle,
          m_WaitingPassengersType = this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle,
          m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
          m_TransportStopData = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
          m_TransportLineData = this.__TypeHandle.__Game_Routes_TransportLine_RO_ComponentLookup,
          m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
          m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
          m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
          m_MasterLaneData = this.__TypeHandle.__Game_Net_MasterLane_RO_ComponentLookup,
          m_Waypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
          m_PrefabTransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
          m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
          m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
          m_TransportPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup,
          m_PedestrianPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup,
          m_CarPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup,
          m_TrackPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup,
          m_ConnectionPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup,
          m_Actions = action.m_UpdateData
        }.Schedule<RoutesModifiedSystem.UpdatePathEdgeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
        archetypeChunkListAsync.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PathfindQueueSystem.Enqueue(action, jobHandle);
      }
      if (entityCount2 != 0)
      {
        DeleteAction action = new DeleteAction(entityCount2, Allocator.Persistent);
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_DeletedSubElementQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new RoutesModifiedSystem.RemovePathEdgeJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_Actions = action.m_DeleteData
        }.Schedule<RoutesModifiedSystem.RemovePathEdgeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        job0 = JobHandle.CombineDependencies(job0, jobHandle);
        archetypeChunkListAsync.Dispose(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PathfindQueueSystem.Enqueue(action, jobHandle);
      }
      this.Dependency = job0;
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
    public RoutesModifiedSystem()
    {
    }

    [BurstCompile]
    private struct AddPathEdgeJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Waypoint> m_WaypointType;
      [ReadOnly]
      public ComponentTypeHandle<Position> m_PositionType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<AccessLane> m_AccessLaneType;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> m_RouteLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.Segment> m_SegmentType;
      [ReadOnly]
      public ComponentTypeHandle<TaxiStand> m_TaxiStandType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TakeoffLocation> m_TakeoffLocationType;
      [ReadOnly]
      public ComponentTypeHandle<Connected> m_ConnectedType;
      [ReadOnly]
      public ComponentTypeHandle<RouteInfo> m_RouteInfoType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public ComponentTypeHandle<WaitingPassengers> m_WaitingPassengersType;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStopData;
      [ReadOnly]
      public ComponentLookup<TransportLine> m_TransportLineData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> m_TransportPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> m_PedestrianPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> m_CarPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> m_TrackPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> m_ConnectionPathfindData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      [WriteOnly]
      public NativeArray<CreateActionData> m_Actions;

      public void Execute()
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<AccessLane> nativeArray3 = chunk.GetNativeArray<AccessLane>(ref this.m_AccessLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<RouteLane> nativeArray4 = chunk.GetNativeArray<RouteLane>(ref this.m_RouteLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.SpawnLocation> nativeArray5 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
          if (nativeArray3.Length != 0 || nativeArray4.Length != 0 || nativeArray5.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Waypoint> nativeArray6 = chunk.GetNativeArray<Waypoint>(ref this.m_WaypointType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Position> nativeArray7 = chunk.GetNativeArray<Position>(ref this.m_PositionType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Connected> nativeArray8 = chunk.GetNativeArray<Connected>(ref this.m_ConnectedType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray9 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Routes.TakeoffLocation> nativeArray10 = chunk.GetNativeArray<Game.Routes.TakeoffLocation>(ref this.m_TakeoffLocationType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<TaxiStand> nativeArray11 = chunk.GetNativeArray<TaxiStand>(ref this.m_TaxiStandType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<WaitingPassengers> nativeArray12 = chunk.GetNativeArray<WaitingPassengers>(ref this.m_WaitingPassengersType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              AccessLane accessLane = new AccessLane();
              if (nativeArray3.Length != 0)
                accessLane = nativeArray3[index2];
              Game.Objects.SpawnLocation spawnLocation = new Game.Objects.SpawnLocation();
              if (nativeArray5.Length != 0)
                spawnLocation = nativeArray5[index2];
              CreateActionData createActionData = new CreateActionData();
              createActionData.m_Owner = entity;
              Entity lane1 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneData.HasComponent(spawnLocation.m_ConnectedLane1))
              {
                lane1 = spawnLocation.m_ConnectedLane1;
                // ISSUE: reference to a compiler-generated field
                Lane lane2 = this.m_LaneData[spawnLocation.m_ConnectedLane1];
                createActionData.m_StartNode = new PathNode(lane2.m_MiddleNode, spawnLocation.m_CurvePosition1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneData.HasComponent(accessLane.m_Lane))
                {
                  lane1 = accessLane.m_Lane;
                  // ISSUE: reference to a compiler-generated field
                  Lane lane3 = this.m_LaneData[accessLane.m_Lane];
                  createActionData.m_StartNode = new PathNode(lane3.m_MiddleNode, accessLane.m_CurvePos);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransportStopData.HasComponent(accessLane.m_Lane))
                  {
                    lane1 = accessLane.m_Lane;
                    createActionData.m_StartNode = new PathNode(accessLane.m_Lane, (ushort) 2);
                  }
                  else
                    createActionData.m_StartNode = new PathNode(entity, (ushort) 2);
                }
              }
              createActionData.m_MiddleNode = new PathNode(entity, (ushort) 1);
              createActionData.m_Location = nativeArray7.Length == 0 ? PathUtils.GetLocationSpecification(nativeArray9[index2].m_Position) : PathUtils.GetLocationSpecification(nativeArray7[index2].m_Position);
              if (nativeArray6.Length != 0)
              {
                createActionData.m_EndNode = new PathNode(entity, (ushort) 0);
                Owner owner = new Owner();
                if (nativeArray2.Length != 0)
                  owner = nativeArray2[index2];
                Game.Routes.TransportStop transportStop = new Game.Routes.TransportStop();
                bool isWaypoint = true;
                if (nativeArray8.Length != 0)
                {
                  Connected connected = nativeArray8[index2];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransportStopData.HasComponent(connected.m_Connected))
                  {
                    // ISSUE: reference to a compiler-generated field
                    transportStop = this.m_TransportStopData[connected.m_Connected];
                    isWaypoint = false;
                  }
                }
                WaitingPassengers waitingPassengers = new WaitingPassengers();
                if (nativeArray12.Length != 0)
                  waitingPassengers = nativeArray12[index2];
                TransportLine transportLine;
                // ISSUE: reference to a compiler-generated method
                TransportLineData transportLineData = this.GetTransportLineData(owner.m_Owner, out transportLine);
                // ISSUE: reference to a compiler-generated method
                PathfindTransportData linePathfindData = this.GetTransportLinePathfindData(transportLineData);
                createActionData.m_Specification = PathUtils.GetTransportStopSpecification(transportStop, transportLine, waitingPassengers, transportLineData, linePathfindData, isWaypoint);
              }
              else
              {
                RouteLane routeLane = new RouteLane();
                if (nativeArray4.Length != 0)
                  routeLane = nativeArray4[index2];
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneData.HasComponent(routeLane.m_EndLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  Lane lane4 = this.m_LaneData[routeLane.m_EndLane];
                  createActionData.m_EndNode = new PathNode(lane4.m_MiddleNode, routeLane.m_EndCurvePos);
                }
                else
                  createActionData.m_EndNode = new PathNode(entity, (ushort) 0);
                createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                if (nativeArray5.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  SpawnLocationData spawnLocationData = this.GetSpawnLocationData(entity);
                  if (spawnLocationData.m_ConnectionType != RouteConnectionType.None)
                  {
                    // ISSUE: reference to a compiler-generated method
                    createActionData.m_Specification = this.GetSpawnLocationPathSpecification(createActionData.m_Location.m_Line.a, spawnLocationData.m_ConnectionType, spawnLocationData.m_RoadTypes, spawnLocation.m_ConnectedLane1, spawnLocation.m_CurvePosition1, 0, spawnLocation.m_AccessRestriction, spawnLocationData.m_RequireAuthorization, (spawnLocation.m_Flags & SpawnLocationFlags.AllowEnter) != 0);
                    if (nativeArray11.Length != 0)
                    {
                      createActionData.m_EndNode = new PathNode(entity, (ushort) 0);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      RouteConnectionData routeConnectionData = this.GetRouteConnectionData(entity);
                      if ((spawnLocationData.m_ConnectionType == RouteConnectionType.Road || spawnLocationData.m_ConnectionType == RouteConnectionType.Cargo || spawnLocationData.m_ConnectionType == RouteConnectionType.Parking) && spawnLocationData.m_ConnectionType != routeConnectionData.m_AccessConnectionType)
                      {
                        int laneCrossCount = 1;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_MasterLaneData.HasComponent(spawnLocation.m_ConnectedLane1))
                        {
                          // ISSUE: reference to a compiler-generated field
                          MasterLane masterLane = this.m_MasterLaneData[spawnLocation.m_ConnectedLane1];
                          laneCrossCount = (int) masterLane.m_MaxIndex - (int) masterLane.m_MinIndex + 1;
                        }
                        bool flag = false;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_LaneData.HasComponent(spawnLocation.m_ConnectedLane2))
                        {
                          // ISSUE: reference to a compiler-generated field
                          Lane lane5 = this.m_LaneData[spawnLocation.m_ConnectedLane2];
                          createActionData.m_SecondaryStartNode = new PathNode(lane5.m_MiddleNode, spawnLocation.m_CurvePosition2);
                          createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                        }
                        else
                        {
                          flag = true;
                          createActionData.m_SecondaryStartNode = new PathNode(entity, (ushort) 3);
                          createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                        }
                        // ISSUE: reference to a compiler-generated method
                        createActionData.m_SecondarySpecification = this.GetSpawnLocationPathSpecification(createActionData.m_Location.m_Line.a, spawnLocationData.m_ConnectionType, spawnLocationData.m_RoadTypes, spawnLocation.m_ConnectedLane2, spawnLocation.m_CurvePosition2, laneCrossCount, spawnLocation.m_AccessRestriction, spawnLocationData.m_RequireAuthorization, (spawnLocation.m_Flags & SpawnLocationFlags.AllowEnter) != 0);
                        if (flag)
                          createActionData.m_SecondarySpecification.m_Flags &= ~(EdgeFlags.Forward | EdgeFlags.Backward);
                      }
                    }
                  }
                }
                else if (nativeArray10.Length != 0)
                {
                  Game.Routes.TakeoffLocation takeoffLocation = nativeArray10[index2];
                  // ISSUE: reference to a compiler-generated method
                  RouteConnectionData routeConnectionData = this.GetRouteConnectionData(entity);
                  // ISSUE: reference to a compiler-generated method
                  createActionData.m_Specification = this.GetSpawnLocationPathSpecification(createActionData.m_Location.m_Line.a, routeConnectionData.m_RouteConnectionType, routeConnectionData.m_RouteRoadType, routeLane.m_EndLane, routeLane.m_EndCurvePos, 0, takeoffLocation.m_AccessRestriction, false, (takeoffLocation.m_Flags & TakeoffLocationFlags.AllowEnter) != 0);
                  Game.Net.CarLane componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (routeConnectionData.m_RouteConnectionType == RouteConnectionType.Air && routeConnectionData.m_RouteRoadType == RoadTypes.Airplane && this.m_CarLaneData.TryGetComponent(accessLane.m_Lane, out componentData) && (componentData.m_Flags & CarLaneFlags.Twoway) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                    createActionData.m_Specification.m_Flags &= (double) accessLane.m_CurvePos >= 0.5 ? ~EdgeFlags.Backward : ~EdgeFlags.Forward;
                }
                if (nativeArray11.Length != 0)
                {
                  TaxiStand taxiStand = nativeArray11[index2];
                  Game.Routes.TransportStop transportStop = new Game.Routes.TransportStop();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransportStopData.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    transportStop = this.m_TransportStopData[entity];
                  }
                  WaitingPassengers waitingPassengers = new WaitingPassengers();
                  if (nativeArray12.Length != 0)
                    waitingPassengers = nativeArray12[index2];
                  // ISSUE: reference to a compiler-generated method
                  PathfindTransportData transportPathfindData = this.GetNetLaneTransportPathfindData(lane1);
                  createActionData.m_SecondaryStartNode = createActionData.m_StartNode;
                  createActionData.m_SecondarySpecification = PathUtils.GetTaxiStopSpecification(transportStop, taxiStand, waitingPassengers, transportPathfindData);
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.m_Actions[num++] = createActionData;
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Routes.Segment> nativeArray13 = chunk.GetNativeArray<Game.Routes.Segment>(ref this.m_SegmentType);
          if (nativeArray13.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<RouteInfo> nativeArray14 = chunk.GetNativeArray<RouteInfo>(ref this.m_RouteInfoType);
            for (int index3 = 0; index3 < nativeArray13.Length; ++index3)
            {
              Entity owner1 = nativeArray1[index3];
              Owner owner2 = nativeArray2[index3];
              Game.Routes.Segment segment = nativeArray13[index3];
              RouteInfo routeInfo = new RouteInfo();
              if (nativeArray14.Length != 0)
                routeInfo = nativeArray14[index3];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[owner2.m_Owner];
              int index4 = math.select(segment.m_Index + 1, 0, segment.m_Index == waypoint1.Length - 1);
              Entity waypoint2 = waypoint1[segment.m_Index].m_Waypoint;
              Entity waypoint3 = waypoint1[index4].m_Waypoint;
              // ISSUE: reference to a compiler-generated field
              Position position1 = this.m_PositionData[waypoint2];
              // ISSUE: reference to a compiler-generated field
              Position position2 = this.m_PositionData[waypoint3];
              // ISSUE: reference to a compiler-generated method
              TransportLineData transportLineData = this.GetTransportLineData(owner2.m_Owner, out TransportLine _);
              // ISSUE: reference to a compiler-generated method
              PathfindTransportData linePathfindData = this.GetTransportLinePathfindData(transportLineData);
              // ISSUE: reference to a compiler-generated field
              this.m_Actions[num++] = new CreateActionData()
              {
                m_Owner = owner1,
                m_StartNode = new PathNode(waypoint2, (ushort) 0),
                m_MiddleNode = new PathNode(owner1, (ushort) 0),
                m_EndNode = new PathNode(waypoint3, (ushort) 0),
                m_Specification = PathUtils.GetTransportLineSpecification(transportLineData, linePathfindData, routeInfo),
                m_Location = PathUtils.GetLocationSpecification(position1.m_Position, position2.m_Position)
              };
            }
          }
        }
      }

      private SpawnLocationData GetSpawnLocationData(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabSpawnLocationData.HasComponent(prefabRef.m_Prefab) ? this.m_PrefabSpawnLocationData[prefabRef.m_Prefab] : new SpawnLocationData();
      }

      private RouteConnectionData GetRouteConnectionData(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabRouteConnectionData.HasComponent(prefabRef.m_Prefab) ? this.m_PrefabRouteConnectionData[prefabRef.m_Prefab] : new RouteConnectionData();
      }

      private TransportLineData GetTransportLineData(Entity owner, out TransportLine transportLine)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportLineData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          transportLine = this.m_TransportLineData[owner];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_PrefabTransportLineData[this.m_PrefabRefData[owner].m_Prefab];
        }
        transportLine = new TransportLine();
        return new TransportLineData();
      }

      private PathfindTransportData GetTransportLinePathfindData(TransportLineData transportLineData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_TransportPathfindData.HasComponent(transportLineData.m_PathfindPrefab) ? this.m_TransportPathfindData[transportLineData.m_PathfindPrefab] : new PathfindTransportData();
      }

      private PathfindTransportData GetNetLaneTransportPathfindData(Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              return this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
            }
          }
        }
        return new PathfindTransportData();
      }

      private PathSpecification GetSpawnLocationPathSpecification(
        float3 position,
        RouteConnectionType connectionType,
        RoadTypes roadType,
        Entity lane,
        float curvePos,
        int laneCrossCount,
        Entity accessRestriction,
        bool requireAuthorization,
        bool allowEnter)
      {
        NetLaneData netLaneData = new NetLaneData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
          }
        }
        switch (connectionType)
        {
          case RouteConnectionType.Road:
          case RouteConnectionType.Cargo:
          case RouteConnectionType.Parking:
            float distance1 = 0.0f;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[lane];
              distance1 = math.distance(position, MathUtils.Position(curve.m_Bezier, curvePos));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
              return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData, roadType, distance1, accessRestriction, requireAuthorization, allowEnter);
            }
            Game.Net.CarLane carLane = new Game.Net.CarLane();
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              carLane = this.m_CarLaneData[lane];
            }
            else
              carLane.m_SpeedLimit = 277.777771f;
            PathfindCarData carPathfindData = new PathfindCarData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(connectionType, carPathfindData, carLane, distance1, laneCrossCount, accessRestriction, requireAuthorization, allowEnter);
          case RouteConnectionType.Pedestrian:
            float distance2 = 0.0f;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[lane];
              distance2 = math.distance(position, MathUtils.Position(curve.m_Bezier, curvePos));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
              return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData, roadType, distance2, accessRestriction, requireAuthorization, allowEnter);
            }
            PathfindPedestrianData pedestrianPathfindData = new PathfindPedestrianData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              pedestrianPathfindData = this.m_PedestrianPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(pedestrianPathfindData, distance2, accessRestriction, requireAuthorization, allowEnter);
          case RouteConnectionType.Track:
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
              return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData, roadType, 0.0f, accessRestriction, requireAuthorization, allowEnter);
            }
            PathfindTrackData trackPathfindData = new PathfindTrackData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrackPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              trackPathfindData = this.m_TrackPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(trackPathfindData, accessRestriction);
          case RouteConnectionType.Air:
            PathfindConnectionData connectionPathfindData1 = new PathfindConnectionData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              connectionPathfindData1 = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData1, roadType, 0.0f, accessRestriction, requireAuthorization, allowEnter);
          default:
            return new PathSpecification();
        }
      }
    }

    [BurstCompile]
    private struct UpdatePathEdgeJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Waypoint> m_WaypointType;
      [ReadOnly]
      public ComponentTypeHandle<Position> m_PositionType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<AccessLane> m_AccessLaneType;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> m_RouteLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.Segment> m_SegmentType;
      [ReadOnly]
      public ComponentTypeHandle<TaxiStand> m_TaxiStandType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TakeoffLocation> m_TakeoffLocationType;
      [ReadOnly]
      public ComponentTypeHandle<Connected> m_ConnectedType;
      [ReadOnly]
      public ComponentTypeHandle<RouteInfo> m_RouteInfoType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public ComponentTypeHandle<WaitingPassengers> m_WaitingPassengersType;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStopData;
      [ReadOnly]
      public ComponentLookup<TransportLine> m_TransportLineData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<MasterLane> m_MasterLaneData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> m_TransportPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> m_PedestrianPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> m_CarPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> m_TrackPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> m_ConnectionPathfindData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      [WriteOnly]
      public NativeArray<UpdateActionData> m_Actions;

      public void Execute()
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<AccessLane> nativeArray3 = chunk.GetNativeArray<AccessLane>(ref this.m_AccessLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<RouteLane> nativeArray4 = chunk.GetNativeArray<RouteLane>(ref this.m_RouteLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.SpawnLocation> nativeArray5 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
          if (nativeArray3.Length != 0 || nativeArray4.Length != 0 || nativeArray5.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Waypoint> nativeArray6 = chunk.GetNativeArray<Waypoint>(ref this.m_WaypointType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Position> nativeArray7 = chunk.GetNativeArray<Position>(ref this.m_PositionType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Connected> nativeArray8 = chunk.GetNativeArray<Connected>(ref this.m_ConnectedType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Transform> nativeArray9 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Routes.TakeoffLocation> nativeArray10 = chunk.GetNativeArray<Game.Routes.TakeoffLocation>(ref this.m_TakeoffLocationType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<TaxiStand> nativeArray11 = chunk.GetNativeArray<TaxiStand>(ref this.m_TaxiStandType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<WaitingPassengers> nativeArray12 = chunk.GetNativeArray<WaitingPassengers>(ref this.m_WaitingPassengersType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              AccessLane accessLane = new AccessLane();
              if (nativeArray3.Length != 0)
                accessLane = nativeArray3[index2];
              Game.Objects.SpawnLocation spawnLocation = new Game.Objects.SpawnLocation();
              if (nativeArray5.Length != 0)
                spawnLocation = nativeArray5[index2];
              UpdateActionData updateActionData = new UpdateActionData();
              updateActionData.m_Owner = entity;
              Entity lane1 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneData.HasComponent(spawnLocation.m_ConnectedLane1))
              {
                lane1 = spawnLocation.m_ConnectedLane1;
                // ISSUE: reference to a compiler-generated field
                Lane lane2 = this.m_LaneData[spawnLocation.m_ConnectedLane1];
                updateActionData.m_StartNode = new PathNode(lane2.m_MiddleNode, spawnLocation.m_CurvePosition1);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneData.HasComponent(accessLane.m_Lane))
                {
                  lane1 = accessLane.m_Lane;
                  // ISSUE: reference to a compiler-generated field
                  Lane lane3 = this.m_LaneData[accessLane.m_Lane];
                  updateActionData.m_StartNode = new PathNode(lane3.m_MiddleNode, accessLane.m_CurvePos);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransportStopData.HasComponent(accessLane.m_Lane))
                  {
                    lane1 = accessLane.m_Lane;
                    updateActionData.m_StartNode = new PathNode(accessLane.m_Lane, (ushort) 2);
                  }
                  else
                    updateActionData.m_StartNode = new PathNode(entity, (ushort) 2);
                }
              }
              updateActionData.m_MiddleNode = new PathNode(entity, (ushort) 1);
              updateActionData.m_Location = nativeArray7.Length == 0 ? PathUtils.GetLocationSpecification(nativeArray9[index2].m_Position) : PathUtils.GetLocationSpecification(nativeArray7[index2].m_Position);
              if (nativeArray6.Length != 0)
              {
                updateActionData.m_EndNode = new PathNode(entity, (ushort) 0);
                Owner owner = new Owner();
                if (nativeArray2.Length != 0)
                  owner = nativeArray2[index2];
                Game.Routes.TransportStop transportStop = new Game.Routes.TransportStop();
                bool isWaypoint = true;
                if (nativeArray8.Length != 0)
                {
                  Connected connected = nativeArray8[index2];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransportStopData.HasComponent(connected.m_Connected))
                  {
                    // ISSUE: reference to a compiler-generated field
                    transportStop = this.m_TransportStopData[connected.m_Connected];
                    isWaypoint = false;
                  }
                }
                WaitingPassengers waitingPassengers = new WaitingPassengers();
                if (nativeArray12.Length != 0)
                  waitingPassengers = nativeArray12[index2];
                TransportLine transportLine;
                // ISSUE: reference to a compiler-generated method
                TransportLineData transportLineData = this.GetTransportLineData(owner.m_Owner, out transportLine);
                // ISSUE: reference to a compiler-generated method
                PathfindTransportData linePathfindData = this.GetTransportLinePathfindData(transportLineData);
                updateActionData.m_Specification = PathUtils.GetTransportStopSpecification(transportStop, transportLine, waitingPassengers, transportLineData, linePathfindData, isWaypoint);
              }
              else
              {
                RouteLane routeLane = new RouteLane();
                if (nativeArray4.Length != 0)
                  routeLane = nativeArray4[index2];
                // ISSUE: reference to a compiler-generated field
                if (this.m_LaneData.HasComponent(routeLane.m_EndLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  Lane lane4 = this.m_LaneData[routeLane.m_EndLane];
                  updateActionData.m_EndNode = new PathNode(lane4.m_MiddleNode, routeLane.m_EndCurvePos);
                }
                else
                  updateActionData.m_EndNode = new PathNode(entity, (ushort) 0);
                updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                if (nativeArray5.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  SpawnLocationData spawnLocationData = this.GetSpawnLocationData(entity);
                  if (spawnLocationData.m_ConnectionType != RouteConnectionType.None)
                  {
                    // ISSUE: reference to a compiler-generated method
                    updateActionData.m_Specification = this.GetSpawnLocationPathSpecification(updateActionData.m_Location.m_Line.a, spawnLocationData.m_ConnectionType, spawnLocationData.m_RoadTypes, spawnLocation.m_ConnectedLane1, spawnLocation.m_CurvePosition1, 0, spawnLocation.m_AccessRestriction, spawnLocationData.m_RequireAuthorization, (spawnLocation.m_Flags & SpawnLocationFlags.AllowEnter) != 0);
                    if (nativeArray11.Length != 0)
                    {
                      updateActionData.m_EndNode = new PathNode(entity, (ushort) 0);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated method
                      RouteConnectionData routeConnectionData = this.GetRouteConnectionData(entity);
                      if ((spawnLocationData.m_ConnectionType == RouteConnectionType.Road || spawnLocationData.m_ConnectionType == RouteConnectionType.Cargo || spawnLocationData.m_ConnectionType == RouteConnectionType.Parking) && spawnLocationData.m_ConnectionType != routeConnectionData.m_AccessConnectionType)
                      {
                        int laneCrossCount = 1;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_MasterLaneData.HasComponent(spawnLocation.m_ConnectedLane1))
                        {
                          // ISSUE: reference to a compiler-generated field
                          MasterLane masterLane = this.m_MasterLaneData[spawnLocation.m_ConnectedLane1];
                          laneCrossCount = (int) masterLane.m_MaxIndex - (int) masterLane.m_MinIndex + 1;
                        }
                        bool flag = false;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_LaneData.HasComponent(spawnLocation.m_ConnectedLane2))
                        {
                          // ISSUE: reference to a compiler-generated field
                          Lane lane5 = this.m_LaneData[spawnLocation.m_ConnectedLane2];
                          updateActionData.m_SecondaryStartNode = new PathNode(lane5.m_MiddleNode, spawnLocation.m_CurvePosition2);
                          updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                        }
                        else
                        {
                          flag = true;
                          updateActionData.m_SecondaryStartNode = new PathNode(entity, (ushort) 3);
                          updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                        }
                        // ISSUE: reference to a compiler-generated method
                        updateActionData.m_SecondarySpecification = this.GetSpawnLocationPathSpecification(updateActionData.m_Location.m_Line.a, spawnLocationData.m_ConnectionType, spawnLocationData.m_RoadTypes, spawnLocation.m_ConnectedLane2, spawnLocation.m_CurvePosition2, laneCrossCount, spawnLocation.m_AccessRestriction, spawnLocationData.m_RequireAuthorization, (spawnLocation.m_Flags & SpawnLocationFlags.AllowEnter) != 0);
                        if (flag)
                          updateActionData.m_SecondarySpecification.m_Flags &= ~(EdgeFlags.Forward | EdgeFlags.Backward);
                      }
                    }
                  }
                }
                else if (nativeArray10.Length != 0)
                {
                  Game.Routes.TakeoffLocation takeoffLocation = nativeArray10[index2];
                  // ISSUE: reference to a compiler-generated method
                  RouteConnectionData routeConnectionData = this.GetRouteConnectionData(entity);
                  // ISSUE: reference to a compiler-generated method
                  updateActionData.m_Specification = this.GetSpawnLocationPathSpecification(updateActionData.m_Location.m_Line.a, routeConnectionData.m_RouteConnectionType, routeConnectionData.m_RouteRoadType, routeLane.m_EndLane, routeLane.m_EndCurvePos, 0, takeoffLocation.m_AccessRestriction, false, (takeoffLocation.m_Flags & TakeoffLocationFlags.AllowEnter) != 0);
                  Game.Net.CarLane componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (routeConnectionData.m_RouteConnectionType == RouteConnectionType.Air && routeConnectionData.m_RouteRoadType == RoadTypes.Airplane && this.m_CarLaneData.TryGetComponent(accessLane.m_Lane, out componentData) && (componentData.m_Flags & CarLaneFlags.Twoway) == ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
                    updateActionData.m_Specification.m_Flags &= (double) accessLane.m_CurvePos >= 0.5 ? ~EdgeFlags.Backward : ~EdgeFlags.Forward;
                }
                if (nativeArray11.Length != 0)
                {
                  TaxiStand taxiStand = nativeArray11[index2];
                  Game.Routes.TransportStop transportStop = new Game.Routes.TransportStop();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_TransportStopData.HasComponent(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    transportStop = this.m_TransportStopData[entity];
                  }
                  WaitingPassengers waitingPassengers = new WaitingPassengers();
                  if (nativeArray12.Length != 0)
                    waitingPassengers = nativeArray12[index2];
                  // ISSUE: reference to a compiler-generated method
                  PathfindTransportData transportPathfindData = this.GetNetLaneTransportPathfindData(lane1);
                  updateActionData.m_SecondaryStartNode = updateActionData.m_StartNode;
                  updateActionData.m_SecondarySpecification = PathUtils.GetTaxiStopSpecification(transportStop, taxiStand, waitingPassengers, transportPathfindData);
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.m_Actions[num++] = updateActionData;
            }
          }
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Routes.Segment> nativeArray13 = chunk.GetNativeArray<Game.Routes.Segment>(ref this.m_SegmentType);
          if (nativeArray13.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<RouteInfo> nativeArray14 = chunk.GetNativeArray<RouteInfo>(ref this.m_RouteInfoType);
            for (int index3 = 0; index3 < nativeArray13.Length; ++index3)
            {
              Entity owner1 = nativeArray1[index3];
              Owner owner2 = nativeArray2[index3];
              Game.Routes.Segment segment = nativeArray13[index3];
              RouteInfo routeInfo = new RouteInfo();
              if (nativeArray14.Length != 0)
                routeInfo = nativeArray14[index3];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[owner2.m_Owner];
              int index4 = math.select(segment.m_Index + 1, 0, segment.m_Index == waypoint1.Length - 1);
              Entity waypoint2 = waypoint1[segment.m_Index].m_Waypoint;
              Entity waypoint3 = waypoint1[index4].m_Waypoint;
              // ISSUE: reference to a compiler-generated field
              Position position1 = this.m_PositionData[waypoint2];
              // ISSUE: reference to a compiler-generated field
              Position position2 = this.m_PositionData[waypoint3];
              // ISSUE: reference to a compiler-generated method
              TransportLineData transportLineData = this.GetTransportLineData(owner2.m_Owner, out TransportLine _);
              // ISSUE: reference to a compiler-generated method
              PathfindTransportData linePathfindData = this.GetTransportLinePathfindData(transportLineData);
              // ISSUE: reference to a compiler-generated field
              this.m_Actions[num++] = new UpdateActionData()
              {
                m_Owner = owner1,
                m_StartNode = new PathNode(waypoint2, (ushort) 0),
                m_MiddleNode = new PathNode(owner1, (ushort) 0),
                m_EndNode = new PathNode(waypoint3, (ushort) 0),
                m_Specification = PathUtils.GetTransportLineSpecification(transportLineData, linePathfindData, routeInfo),
                m_Location = PathUtils.GetLocationSpecification(position1.m_Position, position2.m_Position)
              };
            }
          }
        }
      }

      private SpawnLocationData GetSpawnLocationData(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabSpawnLocationData.HasComponent(prefabRef.m_Prefab) ? this.m_PrefabSpawnLocationData[prefabRef.m_Prefab] : new SpawnLocationData();
      }

      private RouteConnectionData GetRouteConnectionData(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_PrefabRouteConnectionData.HasComponent(prefabRef.m_Prefab) ? this.m_PrefabRouteConnectionData[prefabRef.m_Prefab] : new RouteConnectionData();
      }

      private TransportLineData GetTransportLineData(Entity owner, out TransportLine transportLine)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportLineData.HasComponent(owner))
        {
          // ISSUE: reference to a compiler-generated field
          transportLine = this.m_TransportLineData[owner];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_PrefabTransportLineData[this.m_PrefabRefData[owner].m_Prefab];
        }
        transportLine = new TransportLine();
        return new TransportLineData();
      }

      private PathfindTransportData GetTransportLinePathfindData(TransportLineData transportLineData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_TransportPathfindData.HasComponent(transportLineData.m_PathfindPrefab) ? this.m_TransportPathfindData[transportLineData.m_PathfindPrefab] : new PathfindTransportData();
      }

      private PathfindTransportData GetNetLaneTransportPathfindData(Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              return this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
            }
          }
        }
        return new PathfindTransportData();
      }

      private PathfindConnectionData GetNetLaneConnectionPathfindData(Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              return this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
            }
          }
        }
        return new PathfindConnectionData();
      }

      private PathSpecification GetSpawnLocationPathSpecification(
        float3 position,
        RouteConnectionType connectionType,
        RoadTypes roadType,
        Entity lane,
        float curvePos,
        int laneCrossCount,
        Entity accessRestriction,
        bool requireAuthorization,
        bool allowEnter)
      {
        NetLaneData netLaneData = new NetLaneData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(lane))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[lane];
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetLaneData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
          }
        }
        switch (connectionType)
        {
          case RouteConnectionType.Road:
          case RouteConnectionType.Cargo:
          case RouteConnectionType.Parking:
            float distance1 = 0.0f;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[lane];
              distance1 = math.distance(position, MathUtils.Position(curve.m_Bezier, curvePos));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
              return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData, roadType, distance1, accessRestriction, requireAuthorization, allowEnter);
            }
            Game.Net.CarLane carLane = new Game.Net.CarLane();
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              carLane = this.m_CarLaneData[lane];
            }
            else
              carLane.m_SpeedLimit = 277.777771f;
            PathfindCarData carPathfindData = new PathfindCarData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(connectionType, carPathfindData, carLane, distance1, laneCrossCount, accessRestriction, requireAuthorization, allowEnter);
          case RouteConnectionType.Pedestrian:
            float distance2 = 0.0f;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.HasComponent(lane))
            {
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[lane];
              distance2 = math.distance(position, MathUtils.Position(curve.m_Bezier, curvePos));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
              return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData, roadType, distance2, accessRestriction, requireAuthorization, allowEnter);
            }
            PathfindPedestrianData pedestrianPathfindData = new PathfindPedestrianData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              pedestrianPathfindData = this.m_PedestrianPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(pedestrianPathfindData, distance2, accessRestriction, requireAuthorization, allowEnter);
          case RouteConnectionType.Track:
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
              return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData, roadType, 0.0f, accessRestriction, requireAuthorization, allowEnter);
            }
            PathfindTrackData trackPathfindData = new PathfindTrackData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrackPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              trackPathfindData = this.m_TrackPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(trackPathfindData, accessRestriction);
          case RouteConnectionType.Air:
            PathfindConnectionData connectionPathfindData1 = new PathfindConnectionData();
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectionPathfindData.HasComponent(netLaneData.m_PathfindPrefab))
            {
              // ISSUE: reference to a compiler-generated field
              connectionPathfindData1 = this.m_ConnectionPathfindData[netLaneData.m_PathfindPrefab];
            }
            return PathUtils.GetSpawnLocationSpecification(connectionType, connectionPathfindData1, roadType, 0.0f, accessRestriction, requireAuthorization, allowEnter);
          default:
            return new PathSpecification();
        }
      }
    }

    [BurstCompile]
    private struct RemovePathEdgeJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [WriteOnly]
      public NativeArray<DeleteActionData> m_Actions;

      public void Execute()
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray = this.m_Chunks[index1].GetNativeArray(this.m_EntityType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Actions[num++] = new DeleteActionData()
            {
              m_Owner = nativeArray[index2]
            };
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Waypoint> __Game_Routes_Waypoint_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Position> __Game_Routes_Position_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AccessLane> __Game_Routes_AccessLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RouteLane> __Game_Routes_RouteLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.Segment> __Game_Routes_Segment_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TaxiStand> __Game_Routes_TaxiStand_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TakeoffLocation> __Game_Routes_TakeoffLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Connected> __Game_Routes_Connected_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RouteInfo> __Game_Routes_RouteInfo_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaitingPassengers> __Game_Routes_WaitingPassengers_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> __Game_Routes_TransportStop_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLine> __Game_Routes_TransportLine_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MasterLane> __Game_Net_MasterLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> __Game_Prefabs_PathfindTransportData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> __Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> __Game_Prefabs_PathfindCarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> __Game_Prefabs_PathfindTrackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> __Game_Prefabs_PathfindConnectionData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AccessLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TaxiStand_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TaxiStand>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TakeoffLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.TakeoffLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RouteInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaitingPassengers>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportLine_RO_ComponentLookup = state.GetComponentLookup<TransportLine>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_MasterLane_RO_ComponentLookup = state.GetComponentLookup<MasterLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup = state.GetComponentLookup<PathfindTransportData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup = state.GetComponentLookup<PathfindPedestrianData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindCarData_RO_ComponentLookup = state.GetComponentLookup<PathfindCarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup = state.GetComponentLookup<PathfindTrackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup = state.GetComponentLookup<PathfindConnectionData>(true);
      }
    }
  }
}
