// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.LanesModifiedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Prefabs;
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
  public class LanesModifiedSystem : GameSystemBase
  {
    private PathfindQueueSystem m_PathfindQueueSystem;
    private EntityQuery m_CreatedLanesQuery;
    private EntityQuery m_UpdatedLanesQuery;
    private EntityQuery m_DeletedLanesQuery;
    private EntityQuery m_AllLanesQuery;
    private bool m_Loaded;
    private LanesModifiedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedLanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<SlaveLane>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Net.TrackLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<SlaveLane>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Net.TrackLane>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PathfindUpdated>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<SlaveLane>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedLanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<SlaveLane>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Net.TrackLane>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllLanesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Net.CarLane>(),
          ComponentType.ReadOnly<Game.Net.ParkingLane>(),
          ComponentType.ReadOnly<Game.Net.PedestrianLane>(),
          ComponentType.ReadOnly<Game.Net.ConnectionLane>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<SlaveLane>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Lane>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Net.TrackLane>()
        },
        None = new ComponentType[2]
        {
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
        entityQuery = this.m_AllLanesQuery;
        size = 0;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        entityQuery = this.m_CreatedLanesQuery;
        // ISSUE: reference to a compiler-generated field
        size = this.m_UpdatedLanesQuery.CalculateEntityCount();
      }
      int entityCount1 = entityQuery.CalculateEntityCount();
      // ISSUE: reference to a compiler-generated field
      int entityCount2 = this.m_DeletedLanesQuery.CalculateEntityCount();
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
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LaneConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Density_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new LanesModifiedSystem.AddPathEdgeJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
          m_DensityData = this.__TypeHandle.__Game_Net_Density_RO_ComponentLookup,
          m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
          m_CarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
          m_ParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
          m_PedestrianPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup,
          m_CarPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup,
          m_TrackPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup,
          m_TransportPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup,
          m_ConnectionPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
          m_SlaveLaneType = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle,
          m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
          m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
          m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
          m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
          m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle,
          m_PedestrianLaneType = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle,
          m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle,
          m_GarageLaneType = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentTypeHandle,
          m_LaneConnectionType = this.__TypeHandle.__Game_Net_LaneConnection_RO_ComponentTypeHandle,
          m_OutsideConnectionType = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_Actions = action.m_CreateData
        }.Schedule<LanesModifiedSystem.AddPathEdgeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpdatedLanesQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LaneConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
        this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Density_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
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
        JobHandle jobHandle = new LanesModifiedSystem.UpdatePathEdgeJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
          m_DensityData = this.__TypeHandle.__Game_Net_Density_RO_ComponentLookup,
          m_NetLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
          m_CarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
          m_ParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
          m_PedestrianPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup,
          m_CarPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindCarData_RO_ComponentLookup,
          m_TrackPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup,
          m_TransportPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup,
          m_ConnectionPathfindData = this.__TypeHandle.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_LaneType = this.__TypeHandle.__Game_Net_Lane_RO_ComponentTypeHandle,
          m_SlaveLaneType = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentTypeHandle,
          m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
          m_CarLaneType = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentTypeHandle,
          m_EdgeLaneType = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentTypeHandle,
          m_TrackLaneType = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentTypeHandle,
          m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle,
          m_PedestrianLaneType = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentTypeHandle,
          m_ConnectionLaneType = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentTypeHandle,
          m_GarageLaneType = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentTypeHandle,
          m_LaneConnectionType = this.__TypeHandle.__Game_Net_LaneConnection_RO_ComponentTypeHandle,
          m_OutsideConnectionType = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_Actions = action.m_UpdateData
        }.Schedule<LanesModifiedSystem.UpdatePathEdgeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_DeletedLanesQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new LanesModifiedSystem.RemovePathEdgeJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_Actions = action.m_DeleteData
        }.Schedule<LanesModifiedSystem.RemovePathEdgeJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
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
    public LanesModifiedSystem()
    {
    }

    [BurstCompile]
    private struct AddPathEdgeJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Density> m_DensityData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> m_PedestrianPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> m_CarPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> m_TrackPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> m_TransportPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> m_ConnectionPathfindData;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> m_SlaveLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ParkingLane> m_ParkingLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.PedestrianLane> m_PedestrianLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ConnectionLane> m_ConnectionLaneType;
      [ReadOnly]
      public ComponentTypeHandle<GarageLane> m_GarageLaneType;
      [ReadOnly]
      public ComponentTypeHandle<LaneConnection> m_LaneConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [WriteOnly]
      public NativeArray<CreateActionData> m_Actions;

      public void Execute()
      {
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Lane> nativeArray2 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.CarLane> nativeArray5 = chunk.GetNativeArray<Game.Net.CarLane>(ref this.m_CarLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.TrackLane> nativeArray6 = chunk.GetNativeArray<Game.Net.TrackLane>(ref this.m_TrackLaneType);
          // ISSUE: reference to a compiler-generated field
          if (nativeArray5.Length != 0 && !chunk.Has<SlaveLane>(ref this.m_SlaveLaneType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray7 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<LaneConnection> nativeArray8 = chunk.GetNativeArray<LaneConnection>(ref this.m_LaneConnectionType);
            // ISSUE: reference to a compiler-generated field
            chunk.Has<EdgeLane>(ref this.m_EdgeLaneType);
            if (nativeArray6.Length != 0)
            {
              for (int index2 = 0; index2 < nativeArray5.Length; ++index2)
              {
                Lane lane = nativeArray2[index2];
                Curve curveData = nativeArray3[index2];
                Game.Net.CarLane carLaneData1 = nativeArray5[index2];
                Game.Net.TrackLane trackLaneData = nativeArray6[index2];
                PrefabRef prefabRef = nativeArray4[index2];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                CarLaneData carLaneData2 = this.m_CarLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                PathfindCarData carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                PathfindTransportData transportPathfindData = this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
                float num2 = 0.01f;
                if (nativeArray7.Length != 0)
                {
                  Owner owner = nativeArray7[index2];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DensityData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    num2 = math.max(num2, this.m_DensityData[owner.m_Owner].m_Density);
                  }
                }
                if (nativeArray8.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckLaneConnections(ref lane, nativeArray8[index2]);
                }
                CreateActionData createActionData = new CreateActionData();
                createActionData.m_Owner = nativeArray1[index2];
                createActionData.m_StartNode = lane.m_StartNode;
                createActionData.m_MiddleNode = lane.m_MiddleNode;
                createActionData.m_EndNode = lane.m_EndNode;
                createActionData.m_Specification = PathUtils.GetCarDriveSpecification(curveData, carLaneData1, trackLaneData, carPathfindData, num2);
                createActionData.m_Location = PathUtils.GetLocationSpecification(curveData);
                if (carLaneData2.m_RoadTypes == RoadTypes.Car)
                {
                  createActionData.m_SecondaryStartNode = createActionData.m_StartNode;
                  createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                  createActionData.m_SecondarySpecification = PathUtils.GetTaxiDriveSpecification(curveData, carLaneData1, carPathfindData, transportPathfindData, num2);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_Actions[num1++] = createActionData;
              }
            }
            else
            {
              for (int index3 = 0; index3 < nativeArray5.Length; ++index3)
              {
                Lane lane = nativeArray2[index3];
                Curve curveData = nativeArray3[index3];
                Game.Net.CarLane carLaneData3 = nativeArray5[index3];
                PrefabRef prefabRef = nativeArray4[index3];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                CarLaneData carLaneData4 = this.m_CarLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                PathfindCarData carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                PathfindTransportData transportPathfindData = this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
                float num3 = 0.01f;
                if (nativeArray7.Length != 0)
                {
                  Owner owner = nativeArray7[index3];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DensityData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    num3 = math.max(num3, this.m_DensityData[owner.m_Owner].m_Density);
                  }
                }
                if (nativeArray8.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckLaneConnections(ref lane, nativeArray8[index3]);
                }
                CreateActionData createActionData = new CreateActionData();
                createActionData.m_Owner = nativeArray1[index3];
                createActionData.m_StartNode = lane.m_StartNode;
                createActionData.m_MiddleNode = lane.m_MiddleNode;
                createActionData.m_EndNode = lane.m_EndNode;
                createActionData.m_Specification = PathUtils.GetCarDriveSpecification(curveData, carLaneData3, carPathfindData, num3);
                createActionData.m_Location = PathUtils.GetLocationSpecification(curveData);
                if (carLaneData4.m_RoadTypes == RoadTypes.Car)
                {
                  createActionData.m_SecondaryStartNode = createActionData.m_StartNode;
                  createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                  createActionData.m_SecondarySpecification = PathUtils.GetTaxiDriveSpecification(curveData, carLaneData3, carPathfindData, transportPathfindData, num3);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_Actions[num1++] = createActionData;
              }
            }
          }
          else if (nativeArray6.Length != 0)
          {
            for (int index4 = 0; index4 < nativeArray6.Length; ++index4)
            {
              Lane lane = nativeArray2[index4];
              Curve curveData = nativeArray3[index4];
              Game.Net.TrackLane trackLaneData = nativeArray6[index4];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PathfindTrackData trackPathfindData = this.m_TrackPathfindData[this.m_NetLaneData[nativeArray4[index4].m_Prefab].m_PathfindPrefab];
              // ISSUE: reference to a compiler-generated field
              this.m_Actions[num1++] = new CreateActionData()
              {
                m_Owner = nativeArray1[index4],
                m_StartNode = lane.m_StartNode,
                m_MiddleNode = lane.m_MiddleNode,
                m_EndNode = lane.m_EndNode,
                m_Specification = PathUtils.GetTrackDriveSpecification(curveData, trackLaneData, trackPathfindData),
                m_Location = PathUtils.GetLocationSpecification(curveData)
              };
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.ParkingLane> nativeArray9 = chunk.GetNativeArray<Game.Net.ParkingLane>(ref this.m_ParkingLaneType);
            if (nativeArray9.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<LaneConnection> nativeArray10 = chunk.GetNativeArray<LaneConnection>(ref this.m_LaneConnectionType);
              for (int index5 = 0; index5 < nativeArray9.Length; ++index5)
              {
                Lane lane = nativeArray2[index5];
                Curve curveData = nativeArray3[index5];
                Game.Net.ParkingLane parkingLane = nativeArray9[index5];
                PrefabRef prefabRef = nativeArray4[index5];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                ParkingLaneData parkingLaneData = this.m_ParkingLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                PathfindCarData carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                PathfindTransportData transportPathfindData = this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
                if (nativeArray10.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckLaneConnections(ref lane, nativeArray10[index5]);
                }
                CreateActionData createActionData = new CreateActionData();
                createActionData.m_Owner = nativeArray1[index5];
                createActionData.m_StartNode = lane.m_StartNode;
                createActionData.m_MiddleNode = lane.m_MiddleNode;
                createActionData.m_EndNode = lane.m_EndNode;
                createActionData.m_Specification = PathUtils.GetParkingSpaceSpecification(parkingLane, parkingLaneData, carPathfindData);
                if ((parkingLane.m_Flags & ParkingLaneFlags.SecondaryStart) != (ParkingLaneFlags) 0)
                {
                  createActionData.m_SecondaryStartNode = parkingLane.m_SecondaryStartNode;
                  createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                  createActionData.m_SecondarySpecification = PathUtils.GetParkingSpaceSpecification(parkingLane, parkingLaneData, carPathfindData);
                }
                else
                {
                  createActionData.m_SecondaryStartNode = createActionData.m_StartNode;
                  createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                  createActionData.m_SecondarySpecification = PathUtils.GetTaxiAccessSpecification(parkingLane, carPathfindData, transportPathfindData);
                }
                createActionData.m_Location = PathUtils.GetLocationSpecification(curveData, parkingLane);
                // ISSUE: reference to a compiler-generated field
                this.m_Actions[num1++] = createActionData;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Game.Net.PedestrianLane> nativeArray11 = chunk.GetNativeArray<Game.Net.PedestrianLane>(ref this.m_PedestrianLaneType);
              if (nativeArray11.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<LaneConnection> nativeArray12 = chunk.GetNativeArray<LaneConnection>(ref this.m_LaneConnectionType);
                for (int index6 = 0; index6 < nativeArray11.Length; ++index6)
                {
                  Lane lane = nativeArray2[index6];
                  Curve curveData = nativeArray3[index6];
                  Game.Net.PedestrianLane pedestrianLaneData = nativeArray11[index6];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  PathfindPedestrianData pedestrianPathfindData = this.m_PedestrianPathfindData[this.m_NetLaneData[nativeArray4[index6].m_Prefab].m_PathfindPrefab];
                  if (nativeArray12.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckLaneConnections(ref lane, nativeArray12[index6]);
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_Actions[num1++] = new CreateActionData()
                  {
                    m_Owner = nativeArray1[index6],
                    m_StartNode = lane.m_StartNode,
                    m_MiddleNode = lane.m_MiddleNode,
                    m_EndNode = lane.m_EndNode,
                    m_Specification = PathUtils.GetSpecification(curveData, pedestrianLaneData, pedestrianPathfindData),
                    m_Location = PathUtils.GetLocationSpecification(curveData)
                  };
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Game.Net.ConnectionLane> nativeArray13 = chunk.GetNativeArray<Game.Net.ConnectionLane>(ref this.m_ConnectionLaneType);
                if (nativeArray13.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<GarageLane> nativeArray14 = chunk.GetNativeArray<GarageLane>(ref this.m_GarageLaneType);
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<Game.Net.OutsideConnection> nativeArray15 = chunk.GetNativeArray<Game.Net.OutsideConnection>(ref this.m_OutsideConnectionType);
                  for (int index7 = 0; index7 < nativeArray13.Length; ++index7)
                  {
                    Lane lane = nativeArray2[index7];
                    Curve curveData = nativeArray3[index7];
                    Game.Net.ConnectionLane connectionLaneData = nativeArray13[index7];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[this.m_NetLaneData[nativeArray4[index7].m_Prefab].m_PathfindPrefab];
                    GarageLane garageLane = new GarageLane();
                    if (nativeArray14.Length != 0)
                      garageLane = nativeArray14[index7];
                    else
                      garageLane.m_VehicleCapacity = ushort.MaxValue;
                    Game.Net.OutsideConnection outsideConnection = new Game.Net.OutsideConnection();
                    if (nativeArray15.Length != 0)
                      outsideConnection = nativeArray15[index7];
                    CreateActionData createActionData = new CreateActionData();
                    createActionData.m_Owner = nativeArray1[index7];
                    createActionData.m_StartNode = lane.m_StartNode;
                    createActionData.m_MiddleNode = lane.m_MiddleNode;
                    createActionData.m_EndNode = lane.m_EndNode;
                    createActionData.m_Specification = PathUtils.GetSpecification(curveData, connectionLaneData, garageLane, outsideConnection, connectionPathfindData);
                    createActionData.m_Location = PathUtils.GetLocationSpecification(curveData);
                    if ((connectionLaneData.m_Flags & (ConnectionLaneFlags.SecondaryStart | ConnectionLaneFlags.SecondaryEnd)) != (ConnectionLaneFlags) 0)
                    {
                      createActionData.m_SecondaryStartNode = createActionData.m_StartNode;
                      createActionData.m_SecondaryEndNode = createActionData.m_EndNode;
                      createActionData.m_SecondarySpecification = PathUtils.GetSecondarySpecification(curveData, connectionLaneData, outsideConnection, connectionPathfindData);
                    }
                    // ISSUE: reference to a compiler-generated field
                    this.m_Actions[num1++] = createActionData;
                  }
                }
              }
            }
          }
        }
      }

      private void CheckLaneConnections(ref Lane lane, LaneConnection laneConnection)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneData.HasComponent(laneConnection.m_StartLane))
        {
          // ISSUE: reference to a compiler-generated field
          Lane lane1 = this.m_LaneData[laneConnection.m_StartLane];
          lane.m_StartNode = new PathNode(lane1.m_MiddleNode, laneConnection.m_StartPosition);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LaneData.HasComponent(laneConnection.m_EndLane))
          return;
        // ISSUE: reference to a compiler-generated field
        Lane lane2 = this.m_LaneData[laneConnection.m_EndLane];
        lane.m_EndNode = new PathNode(lane2.m_MiddleNode, laneConnection.m_EndPosition);
      }
    }

    [BurstCompile]
    private struct UpdatePathEdgeJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<Density> m_DensityData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_NetLaneData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> m_PedestrianPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> m_CarPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> m_TrackPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> m_TransportPathfindData;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> m_ConnectionPathfindData;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Lane> m_LaneType;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> m_SlaveLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.CarLane> m_CarLaneType;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> m_EdgeLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> m_TrackLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ParkingLane> m_ParkingLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.PedestrianLane> m_PedestrianLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ConnectionLane> m_ConnectionLaneType;
      [ReadOnly]
      public ComponentTypeHandle<GarageLane> m_GarageLaneType;
      [ReadOnly]
      public ComponentTypeHandle<LaneConnection> m_LaneConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [WriteOnly]
      public NativeArray<UpdateActionData> m_Actions;

      public void Execute()
      {
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Lane> nativeArray2 = chunk.GetNativeArray<Lane>(ref this.m_LaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.CarLane> nativeArray5 = chunk.GetNativeArray<Game.Net.CarLane>(ref this.m_CarLaneType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Net.TrackLane> nativeArray6 = chunk.GetNativeArray<Game.Net.TrackLane>(ref this.m_TrackLaneType);
          // ISSUE: reference to a compiler-generated field
          if (nativeArray5.Length != 0 && !chunk.Has<SlaveLane>(ref this.m_SlaveLaneType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray7 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<LaneConnection> nativeArray8 = chunk.GetNativeArray<LaneConnection>(ref this.m_LaneConnectionType);
            // ISSUE: reference to a compiler-generated field
            chunk.Has<EdgeLane>(ref this.m_EdgeLaneType);
            if (nativeArray6.Length != 0)
            {
              for (int index2 = 0; index2 < nativeArray5.Length; ++index2)
              {
                Lane lane = nativeArray2[index2];
                Curve curveData = nativeArray3[index2];
                Game.Net.CarLane carLaneData1 = nativeArray5[index2];
                Game.Net.TrackLane trackLaneData = nativeArray6[index2];
                PrefabRef prefabRef = nativeArray4[index2];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                CarLaneData carLaneData2 = this.m_CarLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                PathfindCarData carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                PathfindTransportData transportPathfindData = this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
                float num2 = 0.01f;
                if (nativeArray7.Length != 0)
                {
                  Owner owner = nativeArray7[index2];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DensityData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    num2 = math.max(num2, this.m_DensityData[owner.m_Owner].m_Density);
                  }
                }
                if (nativeArray8.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckLaneConnections(ref lane, nativeArray8[index2]);
                }
                UpdateActionData updateActionData = new UpdateActionData();
                updateActionData.m_Owner = nativeArray1[index2];
                updateActionData.m_StartNode = lane.m_StartNode;
                updateActionData.m_MiddleNode = lane.m_MiddleNode;
                updateActionData.m_EndNode = lane.m_EndNode;
                updateActionData.m_Specification = PathUtils.GetCarDriveSpecification(curveData, carLaneData1, trackLaneData, carPathfindData, num2);
                updateActionData.m_Location = PathUtils.GetLocationSpecification(curveData);
                if (carLaneData2.m_RoadTypes == RoadTypes.Car)
                {
                  updateActionData.m_SecondaryStartNode = updateActionData.m_StartNode;
                  updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                  updateActionData.m_SecondarySpecification = PathUtils.GetTaxiDriveSpecification(curveData, carLaneData1, carPathfindData, transportPathfindData, num2);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_Actions[num1++] = updateActionData;
              }
            }
            else
            {
              for (int index3 = 0; index3 < nativeArray5.Length; ++index3)
              {
                Lane lane = nativeArray2[index3];
                Curve curveData = nativeArray3[index3];
                Game.Net.CarLane carLaneData3 = nativeArray5[index3];
                PrefabRef prefabRef = nativeArray4[index3];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                CarLaneData carLaneData4 = this.m_CarLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                PathfindCarData carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                PathfindTransportData transportPathfindData = this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
                float num3 = 0.01f;
                if (nativeArray7.Length != 0)
                {
                  Owner owner = nativeArray7[index3];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DensityData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    num3 = math.max(num3, this.m_DensityData[owner.m_Owner].m_Density);
                  }
                }
                if (nativeArray8.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckLaneConnections(ref lane, nativeArray8[index3]);
                }
                UpdateActionData updateActionData = new UpdateActionData();
                updateActionData.m_Owner = nativeArray1[index3];
                updateActionData.m_StartNode = lane.m_StartNode;
                updateActionData.m_MiddleNode = lane.m_MiddleNode;
                updateActionData.m_EndNode = lane.m_EndNode;
                updateActionData.m_Specification = PathUtils.GetCarDriveSpecification(curveData, carLaneData3, carPathfindData, num3);
                updateActionData.m_Location = PathUtils.GetLocationSpecification(curveData);
                if (carLaneData4.m_RoadTypes == RoadTypes.Car)
                {
                  updateActionData.m_SecondaryStartNode = updateActionData.m_StartNode;
                  updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                  updateActionData.m_SecondarySpecification = PathUtils.GetTaxiDriveSpecification(curveData, carLaneData3, carPathfindData, transportPathfindData, num3);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_Actions[num1++] = updateActionData;
              }
            }
          }
          else if (nativeArray6.Length != 0)
          {
            for (int index4 = 0; index4 < nativeArray6.Length; ++index4)
            {
              Lane lane = nativeArray2[index4];
              Curve curveData = nativeArray3[index4];
              Game.Net.TrackLane trackLaneData = nativeArray6[index4];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              PathfindTrackData trackPathfindData = this.m_TrackPathfindData[this.m_NetLaneData[nativeArray4[index4].m_Prefab].m_PathfindPrefab];
              // ISSUE: reference to a compiler-generated field
              this.m_Actions[num1++] = new UpdateActionData()
              {
                m_Owner = nativeArray1[index4],
                m_StartNode = lane.m_StartNode,
                m_MiddleNode = lane.m_MiddleNode,
                m_EndNode = lane.m_EndNode,
                m_Specification = PathUtils.GetTrackDriveSpecification(curveData, trackLaneData, trackPathfindData),
                m_Location = PathUtils.GetLocationSpecification(curveData)
              };
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.ParkingLane> nativeArray9 = chunk.GetNativeArray<Game.Net.ParkingLane>(ref this.m_ParkingLaneType);
            if (nativeArray9.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<LaneConnection> nativeArray10 = chunk.GetNativeArray<LaneConnection>(ref this.m_LaneConnectionType);
              for (int index5 = 0; index5 < nativeArray9.Length; ++index5)
              {
                Lane lane = nativeArray2[index5];
                Curve curveData = nativeArray3[index5];
                Game.Net.ParkingLane parkingLane = nativeArray9[index5];
                PrefabRef prefabRef = nativeArray4[index5];
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_NetLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                ParkingLaneData parkingLaneData = this.m_ParkingLaneData[prefabRef.m_Prefab];
                // ISSUE: reference to a compiler-generated field
                PathfindCarData carPathfindData = this.m_CarPathfindData[netLaneData.m_PathfindPrefab];
                // ISSUE: reference to a compiler-generated field
                PathfindTransportData transportPathfindData = this.m_TransportPathfindData[netLaneData.m_PathfindPrefab];
                if (nativeArray10.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckLaneConnections(ref lane, nativeArray10[index5]);
                }
                UpdateActionData updateActionData = new UpdateActionData();
                updateActionData.m_Owner = nativeArray1[index5];
                updateActionData.m_StartNode = lane.m_StartNode;
                updateActionData.m_MiddleNode = lane.m_MiddleNode;
                updateActionData.m_EndNode = lane.m_EndNode;
                updateActionData.m_Specification = PathUtils.GetParkingSpaceSpecification(parkingLane, parkingLaneData, carPathfindData);
                if ((parkingLane.m_Flags & ParkingLaneFlags.SecondaryStart) != (ParkingLaneFlags) 0)
                {
                  updateActionData.m_SecondaryStartNode = parkingLane.m_SecondaryStartNode;
                  updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                  updateActionData.m_SecondarySpecification = PathUtils.GetParkingSpaceSpecification(parkingLane, parkingLaneData, carPathfindData);
                }
                else
                {
                  updateActionData.m_SecondaryStartNode = updateActionData.m_StartNode;
                  updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                  updateActionData.m_SecondarySpecification = PathUtils.GetTaxiAccessSpecification(parkingLane, carPathfindData, transportPathfindData);
                }
                updateActionData.m_Location = PathUtils.GetLocationSpecification(curveData, parkingLane);
                // ISSUE: reference to a compiler-generated field
                this.m_Actions[num1++] = updateActionData;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Game.Net.PedestrianLane> nativeArray11 = chunk.GetNativeArray<Game.Net.PedestrianLane>(ref this.m_PedestrianLaneType);
              if (nativeArray11.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<LaneConnection> nativeArray12 = chunk.GetNativeArray<LaneConnection>(ref this.m_LaneConnectionType);
                for (int index6 = 0; index6 < nativeArray11.Length; ++index6)
                {
                  Lane lane = nativeArray2[index6];
                  Curve curveData = nativeArray3[index6];
                  Game.Net.PedestrianLane pedestrianLaneData = nativeArray11[index6];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  PathfindPedestrianData pedestrianPathfindData = this.m_PedestrianPathfindData[this.m_NetLaneData[nativeArray4[index6].m_Prefab].m_PathfindPrefab];
                  if (nativeArray12.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckLaneConnections(ref lane, nativeArray12[index6]);
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_Actions[num1++] = new UpdateActionData()
                  {
                    m_Owner = nativeArray1[index6],
                    m_StartNode = lane.m_StartNode,
                    m_MiddleNode = lane.m_MiddleNode,
                    m_EndNode = lane.m_EndNode,
                    m_Specification = PathUtils.GetSpecification(curveData, pedestrianLaneData, pedestrianPathfindData),
                    m_Location = PathUtils.GetLocationSpecification(curveData)
                  };
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                NativeArray<Game.Net.ConnectionLane> nativeArray13 = chunk.GetNativeArray<Game.Net.ConnectionLane>(ref this.m_ConnectionLaneType);
                if (nativeArray13.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<GarageLane> nativeArray14 = chunk.GetNativeArray<GarageLane>(ref this.m_GarageLaneType);
                  // ISSUE: reference to a compiler-generated field
                  NativeArray<Game.Net.OutsideConnection> nativeArray15 = chunk.GetNativeArray<Game.Net.OutsideConnection>(ref this.m_OutsideConnectionType);
                  for (int index7 = 0; index7 < nativeArray13.Length; ++index7)
                  {
                    Lane lane = nativeArray2[index7];
                    Curve curveData = nativeArray3[index7];
                    Game.Net.ConnectionLane connectionLaneData = nativeArray13[index7];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    PathfindConnectionData connectionPathfindData = this.m_ConnectionPathfindData[this.m_NetLaneData[nativeArray4[index7].m_Prefab].m_PathfindPrefab];
                    GarageLane garageLane = new GarageLane();
                    if (nativeArray14.Length != 0)
                      garageLane = nativeArray14[index7];
                    else
                      garageLane.m_VehicleCapacity = ushort.MaxValue;
                    Game.Net.OutsideConnection outsideConnection = new Game.Net.OutsideConnection();
                    if (nativeArray15.Length != 0)
                      outsideConnection = nativeArray15[index7];
                    UpdateActionData updateActionData = new UpdateActionData();
                    updateActionData.m_Owner = nativeArray1[index7];
                    updateActionData.m_StartNode = lane.m_StartNode;
                    updateActionData.m_MiddleNode = lane.m_MiddleNode;
                    updateActionData.m_EndNode = lane.m_EndNode;
                    updateActionData.m_Specification = PathUtils.GetSpecification(curveData, connectionLaneData, garageLane, outsideConnection, connectionPathfindData);
                    updateActionData.m_Location = PathUtils.GetLocationSpecification(curveData);
                    if ((connectionLaneData.m_Flags & (ConnectionLaneFlags.SecondaryStart | ConnectionLaneFlags.SecondaryEnd)) != (ConnectionLaneFlags) 0)
                    {
                      updateActionData.m_SecondaryStartNode = updateActionData.m_StartNode;
                      updateActionData.m_SecondaryEndNode = updateActionData.m_EndNode;
                      updateActionData.m_SecondarySpecification = PathUtils.GetSecondarySpecification(curveData, connectionLaneData, outsideConnection, connectionPathfindData);
                    }
                    // ISSUE: reference to a compiler-generated field
                    this.m_Actions[num1++] = updateActionData;
                  }
                }
              }
            }
          }
        }
      }

      private void CheckLaneConnections(ref Lane lane, LaneConnection laneConnection)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneData.HasComponent(laneConnection.m_StartLane))
        {
          // ISSUE: reference to a compiler-generated field
          Lane lane1 = this.m_LaneData[laneConnection.m_StartLane];
          lane.m_StartNode = new PathNode(lane1.m_MiddleNode, laneConnection.m_StartPosition);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_LaneData.HasComponent(laneConnection.m_EndLane))
          return;
        // ISSUE: reference to a compiler-generated field
        Lane lane2 = this.m_LaneData[laneConnection.m_EndLane];
        lane.m_EndNode = new PathNode(lane2.m_MiddleNode, laneConnection.m_EndPosition);
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
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Density> __Game_Net_Density_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindPedestrianData> __Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindCarData> __Game_Prefabs_PathfindCarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTrackData> __Game_Prefabs_PathfindTrackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindTransportData> __Game_Prefabs_PathfindTransportData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathfindConnectionData> __Game_Prefabs_PathfindConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lane> __Game_Net_Lane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SlaveLane> __Game_Net_SlaveLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EdgeLane> __Game_Net_EdgeLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GarageLane> __Game_Net_GarageLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<LaneConnection> __Game_Net_LaneConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Density_RO_ComponentLookup = state.GetComponentLookup<Density>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindPedestrianData_RO_ComponentLookup = state.GetComponentLookup<PathfindPedestrianData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindCarData_RO_ComponentLookup = state.GetComponentLookup<PathfindCarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTrackData_RO_ComponentLookup = state.GetComponentLookup<PathfindTrackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindTransportData_RO_ComponentLookup = state.GetComponentLookup<PathfindTransportData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PathfindConnectionData_RO_ComponentLookup = state.GetComponentLookup<PathfindConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LaneConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
