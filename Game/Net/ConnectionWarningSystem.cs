// Decompiled with JetBrains decompiler
// Type: Game.Net.ConnectionWarningSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class ConnectionWarningSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private IconCommandSystem m_IconCommandSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Areas.UpdateCollectSystem m_AreaUpdateCollectSystem;
    private SearchSystem m_NetSearchSystem;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private EntityQuery m_UpdateQuery;
    private EntityQuery m_NewGameQuery;
    private EntityQuery m_WaterConfigQuery;
    private EntityQuery m_ElectricityConfigQuery;
    private EntityQuery m_TrafficConfigQuery;
    private bool m_IsNewGame;
    private ConnectionWarningSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Areas.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Node>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
          ComponentType.ReadOnly<Game.Routes.TakeoffLocation>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<RoadConnectionUpdated>(),
          ComponentType.ReadOnly<Game.Common.Event>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_NewGameQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<Node>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>(),
          ComponentType.ReadOnly<Game.Routes.TakeoffLocation>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_WaterConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TrafficConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<TrafficConfigurationData>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      this.m_IsNewGame = serializationContext.purpose == Colossal.Serialization.Entities.Purpose.NewGame;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = this.m_IsNewGame ? this.m_NewGameQuery : this.m_UpdateQuery;
      // ISSUE: reference to a compiler-generated field
      this.m_IsNewGame = false;
      bool flag = !entityQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      bool mapTilesUpdated = this.m_AreaUpdateCollectSystem.mapTilesUpdated;
      if (!flag && !mapTilesUpdated)
        return;
      NativeList<Entity> list = new NativeList<Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle job0 = this.Dependency;
      if (flag)
      {
        JobHandle outJobHandle;
        NativeList<ArchetypeChunk> archetypeChunkListAsync = entityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        JobHandle inputDeps = new ConnectionWarningSystem.CollectOwnersJob()
        {
          m_Chunks = archetypeChunkListAsync,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
          m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
          m_RoadConnectionUpdatedType = this.__TypeHandle.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
          m_Owners = list
        }.Schedule<ConnectionWarningSystem.CollectOwnersJob>(JobHandle.CombineDependencies(job0, outJobHandle));
        archetypeChunkListAsync.Dispose(inputDeps);
        job0 = inputDeps;
      }
      if (mapTilesUpdated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        JobHandle dependencies1;
        JobHandle dependencies2;
        JobHandle dependencies3;
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
        // ISSUE: object of a compiler-generated type is created
        JobHandle jobHandle = new ConnectionWarningSystem.CollectOwnersJob2()
        {
          m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
          m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
          m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
          m_ObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
          m_Bounds = this.m_AreaUpdateCollectSystem.GetUpdatedMapTileBounds(out dependencies3),
          m_Owners = list
        }.Schedule<ConnectionWarningSystem.CollectOwnersJob2>(JobUtils.CombineDependencies(job0, dependencies1, dependencies2, dependencies3));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaUpdateCollectSystem.AddMapTileBoundsReader(jobHandle);
        job0 = jobHandle;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle1 = new ConnectionWarningSystem.CheckOwnersJob()
      {
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_Owners = list.AsDeferredJobArray(),
        m_WaterPipeParameterData = this.GetConfigData<WaterPipeParameterData>(this.m_WaterConfigQuery),
        m_ElectricityParameterData = this.GetConfigData<ElectricityParameterData>(this.m_ElectricityConfigQuery),
        m_TrafficConfigurationData = this.GetConfigData<TrafficConfigurationData>(this.m_TrafficConfigQuery),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_LaneConnectionData = this.__TypeHandle.__Game_Net_LaneConnection_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Net_OutsideConnection_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_TakeoffLocationData = this.__TypeHandle.__Game_Routes_TakeoffLocation_RO_ComponentLookup,
        m_AccessLaneData = this.__TypeHandle.__Game_Routes_AccessLane_RO_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_ElectricityProducerData = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentLookup,
        m_TransformerData = this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_MapTileData = this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetData = this.__TypeHandle.__Game_Prefabs_NetData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabLocalConnectData = this.__TypeHandle.__Game_Prefabs_LocalConnectData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_ConnectedNodes = this.__TypeHandle.__Game_Net_ConnectedNode_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_IconElements = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferLookup
      }.Schedule<ConnectionWarningSystem.CheckOwnersJob, Entity>(list, 1, JobHandle.CombineDependencies(job0, dependencies));
      list.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle1);
      this.Dependency = jobHandle1;
    }

    private T GetConfigData<T>(EntityQuery query) where T : unmanaged, IComponentData
    {
      return query.IsEmptyIgnoreFilter ? default (T) : query.GetSingleton<T>();
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
    public ConnectionWarningSystem()
    {
    }

    [BurstCompile]
    private struct CollectOwnersJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<RoadConnectionUpdated> m_RoadConnectionUpdatedType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      public NativeList<Entity> m_Owners;

      public void Execute()
      {
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Owners.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          nativeParallelHashSet.Add(this.m_Owners[index]);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<RoadConnectionUpdated> nativeArray1 = chunk.GetNativeArray<RoadConnectionUpdated>(ref this.m_RoadConnectionUpdatedType);
          if (nativeArray1.Length != 0)
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              RoadConnectionUpdated connectionUpdated = nativeArray1[index2];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_TempData.HasComponent(connectionUpdated.m_Building) && nativeParallelHashSet.Add(connectionUpdated.m_Building))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Owners.Add(in connectionUpdated.m_Building);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            if (nativeArray3.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              bool flag = chunk.Has<Building>(ref this.m_BuildingType);
              for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
              {
                Owner owner = nativeArray3[index3];
                if (flag)
                {
                  Entity entity = nativeArray2[index3];
                  if (nativeParallelHashSet.Add(entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_Owners.Add(in entity);
                  }
                }
                Owner componentData;
                for (; nativeParallelHashSet.Add(owner.m_Owner); owner = componentData)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Owners.Add(in owner.m_Owner);
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData))
                    break;
                }
              }
            }
            else
            {
              for (int index4 = 0; index4 < nativeArray2.Length; ++index4)
              {
                Entity entity = nativeArray2[index4];
                if (nativeParallelHashSet.Add(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_Owners.Add(in entity);
                }
              }
            }
          }
        }
        nativeParallelHashSet.Dispose();
      }
    }

    [BurstCompile]
    private struct CollectOwnersJob2 : IJob
    {
      [ReadOnly]
      public ComponentLookup<Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_ObjectSearchTree;
      [ReadOnly]
      public NativeList<Bounds2> m_Bounds;
      public NativeList<Entity> m_Owners;

      public void Execute()
      {
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Owners.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          nativeParallelHashSet.Add(this.m_Owners[index]);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ConnectionWarningSystem.CollectOwnersJob2.NodeIterator iterator1 = new ConnectionWarningSystem.CollectOwnersJob2.NodeIterator()
        {
          m_NodeData = this.m_NodeData,
          m_OwnerData = this.m_OwnerData,
          m_OwnerSet = nativeParallelHashSet,
          m_Owners = this.m_Owners
        };
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ConnectionWarningSystem.CollectOwnersJob2.BuildingIterator iterator2 = new ConnectionWarningSystem.CollectOwnersJob2.BuildingIterator()
        {
          m_BuildingData = this.m_BuildingData,
          m_OwnerData = this.m_OwnerData,
          m_OwnerSet = nativeParallelHashSet,
          m_Owners = this.m_Owners
        };
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Bounds.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          iterator1.m_Bounds = iterator2.m_Bounds = this.m_Bounds[index];
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<ConnectionWarningSystem.CollectOwnersJob2.NodeIterator>(ref iterator1);
          // ISSUE: reference to a compiler-generated field
          this.m_ObjectSearchTree.Iterate<ConnectionWarningSystem.CollectOwnersJob2.BuildingIterator>(ref iterator2);
        }
        nativeParallelHashSet.Dispose();
      }

      public struct NodeIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Node> m_NodeData;
        public ComponentLookup<Owner> m_OwnerData;
        public NativeParallelHashSet<Entity> m_OwnerSet;
        public NativeList<Entity> m_Owners;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!this.Intersect(bounds) || !this.m_NodeData.HasComponent(entity))
            return;
          Owner componentData1;
          Owner componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(entity, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            for (; this.m_OwnerSet.Add(componentData1.m_Owner); componentData1 = componentData2)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Owners.Add(in componentData1.m_Owner);
              // ISSUE: reference to a compiler-generated field
              if (!this.m_OwnerData.TryGetComponent(componentData1.m_Owner, out componentData2))
                break;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OwnerSet.Add(entity))
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_Owners.Add(in entity);
          }
        }
      }

      public struct BuildingIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Owner> m_OwnerData;
        public NativeParallelHashSet<Entity> m_OwnerSet;
        public NativeList<Entity> m_Owners;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!this.Intersect(bounds) || !this.m_BuildingData.HasComponent(entity))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerSet.Add(entity))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Owners.Add(in entity);
          }
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.TryGetComponent(entity, out componentData1))
            return;
          Owner componentData2;
          // ISSUE: reference to a compiler-generated field
          for (; this.m_OwnerSet.Add(componentData1.m_Owner); componentData1 = componentData2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Owners.Add(in componentData1.m_Owner);
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OwnerData.TryGetComponent(componentData1.m_Owner, out componentData2))
              break;
          }
        }
      }
    }

    private struct PathfindElement
    {
      public PathNode m_StartNode;
      public PathNode m_MiddleNode;
      public PathNode m_EndNode;
      public Entity m_Entity;
      public bool2 m_Connected;
      public bool2 m_Directions;
      public byte m_IconType;
      public byte m_IconLocation;
      public sbyte m_Priority;
      public bool m_CanIgnore;
      public bool m_Optional;
      public sbyte m_SubConnection;
    }

    private struct BufferElement
    {
      public PathNode m_Node;
      public bool2 m_Connected;
    }

    private struct Connection
    {
      public RoadTypes m_RoadTypes;
      public TrackTypes m_TrackTypes;
    }

    [BurstCompile]
    private struct CheckOwnersJob : IJobParallelForDefer
    {
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeArray<Entity> m_Owners;
      [ReadOnly]
      public WaterPipeParameterData m_WaterPipeParameterData;
      [ReadOnly]
      public ElectricityParameterData m_ElectricityParameterData;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      [ReadOnly]
      public TrafficConfigurationData m_TrafficConfigurationData;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public IconCommandBuffer m_IconCommandBuffer;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<LaneConnection> m_LaneConnectionData;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> m_TakeoffLocationData;
      [ReadOnly]
      public ComponentLookup<AccessLane> m_AccessLaneData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [ReadOnly]
      public ComponentLookup<ElectricityProducer> m_ElectricityProducerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Transformer> m_TransformerData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<MapTile> m_MapTileData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> m_PrefabLocalConnectData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public BufferLookup<SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<ConnectedNode> m_ConnectedNodes;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<IconElement> m_IconElements;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity owner = this.m_Owners[index];
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          Node node = this.m_NodeData[owner];
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.UpdateNodeConnectionWarnings(owner, this.IsNativeMapTile(node.m_Position));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BuildingData.HasComponent(owner))
            return;
          // ISSUE: reference to a compiler-generated field
          Building building = this.m_BuildingData[owner];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          BuildingData buildingData = this.m_PrefabBuildingData[this.m_PrefabRefData[owner].m_Prefab];
          bool isSubBuilding = false;
          // ISSUE: reference to a compiler-generated field
          bool flag1 = !this.m_DestroyedData.HasComponent(owner);
          Owner componentData1;
          bool flag2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(owner, out componentData1))
          {
            isSubBuilding = true;
            PrefabRef componentData2;
            BuildingData componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            flag2 = ((flag1 ? 1 : 0) & ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) != (Game.Prefabs.BuildingFlags) 0 ? 1 : (!this.m_PrefabRefData.TryGetComponent(componentData1.m_Owner, out componentData2) || !this.m_PrefabBuildingData.TryGetComponent(componentData2.m_Prefab, out componentData3) ? 0 : ((componentData3.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) > (Game.Prefabs.BuildingFlags) 0 ? 1 : 0)))) != 0;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateSubnetConnectionWarnings(owner);
            // ISSUE: reference to a compiler-generated field
            flag2 = ((flag1 ? 1 : 0) & (!this.m_PrefabRefData.HasComponent(building.m_RoadEdge) ? 0 : ((buildingData.m_Flags & Game.Prefabs.BuildingFlags.RequireRoad) > (Game.Prefabs.BuildingFlags) 0 ? 1 : 0))) != 0;
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform transform = this.m_TransformData[owner];
            // ISSUE: reference to a compiler-generated method
            if (this.IsNativeMapTile(transform.m_Position))
            {
              // ISSUE: reference to a compiler-generated method
              this.ClearPathfindIslandWarnings(owner);
            }
            else
            {
              float2 lotSize = (float2) buildingData.m_LotSize;
              Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, new Bounds3()
              {
                min = {
                  xz = lotSize * -4f
                },
                max = {
                  xz = lotSize * 4f
                }
              });
              // ISSUE: reference to a compiler-generated method
              this.UpdatePathfindIslandWarnings(owner, baseCorners, isSubBuilding);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.ClearPathfindIslandWarnings(owner);
          }
        }
      }

      private bool IsNativeMapTile(float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ConnectionWarningSystem.CheckOwnersJob.MapTileIterator iterator = new ConnectionWarningSystem.CheckOwnersJob.MapTileIterator()
        {
          m_Position = position,
          m_NativeData = this.m_NativeData,
          m_MapTileData = this.m_MapTileData,
          m_AreaNodes = this.m_AreaNodes,
          m_AreaTriangles = this.m_AreaTriangles
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<ConnectionWarningSystem.CheckOwnersJob.MapTileIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NativeData = iterator.m_NativeData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MapTileData = iterator.m_MapTileData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaNodes = iterator.m_AreaNodes;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaTriangles = iterator.m_AreaTriangles;
        // ISSUE: reference to a compiler-generated field
        return iterator.m_Result == 2;
      }

      private void ClearPathfindIslandWarnings(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IconElements.HasBuffer(owner))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<IconElement> iconElement = this.m_IconElements[owner];
        for (int index = 0; index < iconElement.Length; ++index)
        {
          Entity icon = iconElement[index].m_Icon;
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_PrefabRefData[icon].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((prefab == this.m_TrafficConfigurationData.m_CarConnectionNotification || prefab == this.m_TrafficConfigurationData.m_ShipConnectionNotification || prefab == this.m_TrafficConfigurationData.m_PedestrianConnectionNotification || prefab == this.m_TrafficConfigurationData.m_TrainConnectionNotification || prefab == this.m_TrafficConfigurationData.m_RoadConnectionNotification) && this.m_TargetData.HasComponent(icon))
          {
            // ISSUE: reference to a compiler-generated field
            Entity target = this.m_TargetData[icon].m_Target;
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(owner, prefab, target);
          }
        }
      }

      private void UpdatePathfindIslandWarnings(Entity owner, Quad3 lot, bool isSubBuilding)
      {
        NativeList<ConnectionWarningSystem.PathfindElement> ownedElements = new NativeList<ConnectionWarningSystem.PathfindElement>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelMultiHashMap<PathNode, int> nodeMap = new NativeParallelMultiHashMap<PathNode, int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashSet<PathNode> externalNodes = new NativeParallelHashSet<PathNode>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        this.AddPathfindElements(ownedElements, nodeMap, externalNodes, owner, owner, lot, false, isSubBuilding);
        Owner componentData;
        DynamicBuffer<SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (isSubBuilding && this.m_OwnerData.TryGetComponent(owner, out componentData) && this.m_SubLanes.TryGetBuffer(componentData.m_Owner, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          this.AddPathfindElements(ownedElements, nodeMap, bufferData, true, false, true, (bool2) false, (float2) -1f);
        }
        bool canIgnore;
        // ISSUE: reference to a compiler-generated method
        this.CheckConnectedElements(ownedElements, nodeMap, externalNodes, out canIgnore);
        // ISSUE: reference to a compiler-generated method
        this.UpdatePathfindWarnings(ownedElements, owner, canIgnore);
        ownedElements.Dispose();
        nodeMap.Dispose();
        externalNodes.Dispose();
      }

      private void UpdatePathfindWarnings(
        NativeList<ConnectionWarningSystem.PathfindElement> ownedElements,
        Entity owner,
        bool canIgnore)
      {
        NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>();
        if (!canIgnore)
        {
          bool flag = false;
          for (int index = 0; index < ownedElements.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            ConnectionWarningSystem.PathfindElement ownedElement = ownedElements[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!math.all(ownedElement.m_Connected) && ownedElement.m_Priority >= (sbyte) 0 && !ownedElement.m_Optional && (ownedElement.m_Priority != (sbyte) 0 || ownedElement.m_SubConnection == (sbyte) 0) && ownedElement.m_IconType == (byte) 4)
            {
              flag = true;
              break;
            }
          }
          for (int index = 0; index < ownedElements.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            ConnectionWarningSystem.PathfindElement ownedElement = ownedElements[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!math.all(ownedElement.m_Connected) && ownedElement.m_Priority >= (sbyte) 0 && !ownedElement.m_Optional && (ownedElement.m_Priority != (sbyte) 0 || ownedElement.m_SubConnection == (sbyte) 0))
            {
              Entity connectionNotification;
              // ISSUE: reference to a compiler-generated field
              switch (ownedElement.m_IconType)
              {
                case 1:
                  // ISSUE: reference to a compiler-generated field
                  connectionNotification = this.m_TrafficConfigurationData.m_CarConnectionNotification;
                  break;
                case 2:
                  // ISSUE: reference to a compiler-generated field
                  connectionNotification = this.m_TrafficConfigurationData.m_PedestrianConnectionNotification;
                  break;
                case 3:
                  // ISSUE: reference to a compiler-generated field
                  connectionNotification = this.m_TrafficConfigurationData.m_TrainConnectionNotification;
                  break;
                case 4:
                  // ISSUE: reference to a compiler-generated field
                  connectionNotification = this.m_TrafficConfigurationData.m_RoadConnectionNotification;
                  break;
                case 5:
                  // ISSUE: reference to a compiler-generated field
                  connectionNotification = this.m_TrafficConfigurationData.m_ShipConnectionNotification;
                  break;
                default:
                  connectionNotification = Entity.Null;
                  break;
              }
              if (connectionNotification != Entity.Null)
              {
                if (!nativeParallelHashSet.IsCreated)
                  nativeParallelHashSet = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                // ISSUE: reference to a compiler-generated field
                if (ownedElement.m_IconType == (byte) 4)
                {
                  EdgeLane componentData1;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_EdgeLaneData.TryGetComponent(ownedElement.m_Entity, out componentData1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    float num = math.lerp(componentData1.m_EdgeDelta.x, componentData1.m_EdgeDelta.y, (float) ownedElement.m_IconLocation * 0.003921569f);
                    // ISSUE: reference to a compiler-generated field
                    ownedElement.m_IconLocation = (byte) math.clamp(Mathf.RoundToInt(num * (float) byte.MaxValue), 0, (int) byte.MaxValue);
                  }
                  Owner componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.TryGetComponent(ownedElement.m_Entity, out componentData2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    ownedElement.m_Entity = componentData2.m_Owner;
                  }
                }
                else if (flag)
                  continue;
                // ISSUE: reference to a compiler-generated field
                if (nativeParallelHashSet.Add(ownedElement.m_Entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_CurveData.HasComponent(ownedElement.m_Entity))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float3 location = MathUtils.Position(this.m_CurveData[ownedElement.m_Entity].m_Bezier, (float) ownedElement.m_IconLocation * 0.003921569f);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Add(owner, connectionNotification, location, IconPriority.Warning, flags: IconFlags.TargetLocation, target: ownedElement.m_Entity);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_IconCommandBuffer.Add(owner, connectionNotification, IconPriority.Warning, flags: IconFlags.TargetLocation, target: ownedElement.m_Entity);
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_IconElements.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<IconElement> iconElement = this.m_IconElements[owner];
          for (int index = 0; index < iconElement.Length; ++index)
          {
            Entity icon = iconElement[index].m_Icon;
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_PrefabRefData[icon].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((prefab == this.m_TrafficConfigurationData.m_CarConnectionNotification || prefab == this.m_TrafficConfigurationData.m_ShipConnectionNotification || prefab == this.m_TrafficConfigurationData.m_PedestrianConnectionNotification || prefab == this.m_TrafficConfigurationData.m_TrainConnectionNotification || prefab == this.m_TrafficConfigurationData.m_RoadConnectionNotification) && this.m_TargetData.HasComponent(icon))
            {
              // ISSUE: reference to a compiler-generated field
              Entity target = this.m_TargetData[icon].m_Target;
              if (!nativeParallelHashSet.IsCreated || !nativeParallelHashSet.Contains(target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_IconCommandBuffer.Remove(owner, prefab, target);
              }
            }
          }
        }
        if (!nativeParallelHashSet.IsCreated)
          return;
        nativeParallelHashSet.Dispose();
      }

      private void CheckConnectedElements(
        NativeList<ConnectionWarningSystem.PathfindElement> ownedElements,
        NativeParallelMultiHashMap<PathNode, int> nodeMap,
        NativeParallelHashSet<PathNode> externalNodes,
        out bool canIgnore)
      {
        NativeList<ConnectionWarningSystem.BufferElement> nativeList = new NativeList<ConnectionWarningSystem.BufferElement>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        canIgnore = true;
        // ISSUE: variable of a compiler-generated type
        ConnectionWarningSystem.BufferElement bufferElement1;
        for (int index1 = 0; index1 < ownedElements.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          ConnectionWarningSystem.PathfindElement ownedElement = ownedElements[index1];
          // ISSUE: reference to a compiler-generated field
          canIgnore &= ownedElement.m_CanIgnore;
          // ISSUE: reference to a compiler-generated field
          if (!math.all(ownedElement.m_Connected))
          {
            // ISSUE: reference to a compiler-generated field
            if (math.all(ownedElement.m_Directions))
            {
              // ISSUE: reference to a compiler-generated field
              if (externalNodes.Contains(ownedElement.m_StartNode.StripCurvePos()))
              {
                ref NativeList<ConnectionWarningSystem.BufferElement> local1 = ref nativeList;
                // ISSUE: object of a compiler-generated type is created
                bufferElement1 = new ConnectionWarningSystem.BufferElement();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Node = ownedElement.m_StartNode;
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Connected = (bool2) true;
                ref ConnectionWarningSystem.BufferElement local2 = ref bufferElement1;
                local1.Add(in local2);
              }
              // ISSUE: reference to a compiler-generated field
              if (externalNodes.Contains(ownedElement.m_EndNode.StripCurvePos()))
              {
                ref NativeList<ConnectionWarningSystem.BufferElement> local3 = ref nativeList;
                // ISSUE: object of a compiler-generated type is created
                bufferElement1 = new ConnectionWarningSystem.BufferElement();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Node = ownedElement.m_EndNode;
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Connected = (bool2) true;
                ref ConnectionWarningSystem.BufferElement local4 = ref bufferElement1;
                local3.Add(in local4);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (ownedElement.m_Directions.x)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!ownedElement.m_Connected.x && externalNodes.Contains(ownedElement.m_StartNode.StripCurvePos()))
                {
                  ref NativeList<ConnectionWarningSystem.BufferElement> local5 = ref nativeList;
                  // ISSUE: object of a compiler-generated type is created
                  bufferElement1 = new ConnectionWarningSystem.BufferElement();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  bufferElement1.m_Node = ownedElement.m_StartNode;
                  // ISSUE: reference to a compiler-generated field
                  bufferElement1.m_Connected = new bool2(true, false);
                  ref ConnectionWarningSystem.BufferElement local6 = ref bufferElement1;
                  local5.Add(in local6);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!ownedElement.m_Connected.y && externalNodes.Contains(ownedElement.m_EndNode.StripCurvePos()))
                {
                  ref NativeList<ConnectionWarningSystem.BufferElement> local7 = ref nativeList;
                  // ISSUE: object of a compiler-generated type is created
                  bufferElement1 = new ConnectionWarningSystem.BufferElement();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  bufferElement1.m_Node = ownedElement.m_EndNode;
                  // ISSUE: reference to a compiler-generated field
                  bufferElement1.m_Connected = new bool2(false, true);
                  ref ConnectionWarningSystem.BufferElement local8 = ref bufferElement1;
                  local7.Add(in local8);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (ownedElement.m_Directions.y)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!ownedElement.m_Connected.x && externalNodes.Contains(ownedElement.m_EndNode.StripCurvePos()))
                  {
                    ref NativeList<ConnectionWarningSystem.BufferElement> local9 = ref nativeList;
                    // ISSUE: object of a compiler-generated type is created
                    bufferElement1 = new ConnectionWarningSystem.BufferElement();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    bufferElement1.m_Node = ownedElement.m_EndNode;
                    // ISSUE: reference to a compiler-generated field
                    bufferElement1.m_Connected = new bool2(true, false);
                    ref ConnectionWarningSystem.BufferElement local10 = ref bufferElement1;
                    local9.Add(in local10);
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!ownedElement.m_Connected.y && externalNodes.Contains(ownedElement.m_StartNode.StripCurvePos()))
                  {
                    ref NativeList<ConnectionWarningSystem.BufferElement> local11 = ref nativeList;
                    // ISSUE: object of a compiler-generated type is created
                    bufferElement1 = new ConnectionWarningSystem.BufferElement();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    bufferElement1.m_Node = ownedElement.m_StartNode;
                    // ISSUE: reference to a compiler-generated field
                    bufferElement1.m_Connected = new bool2(false, true);
                    ref ConnectionWarningSystem.BufferElement local12 = ref bufferElement1;
                    local11.Add(in local12);
                  }
                }
              }
            }
            while (nativeList.Length > 0)
            {
              // ISSUE: variable of a compiler-generated type
              ConnectionWarningSystem.BufferElement bufferElement2 = nativeList[nativeList.Length - 1];
              nativeList.RemoveAt(nativeList.Length - 1);
              int index2;
              NativeParallelMultiHashMapIterator<PathNode> it;
              // ISSUE: reference to a compiler-generated field
              if (nodeMap.TryGetFirstValue(bufferElement2.m_Node.StripCurvePos(), out index2, out it))
              {
                do
                {
                  ownedElement = ownedElements[index2];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (ownedElement.m_StartNode.EqualsIgnoreCurvePos(bufferElement2.m_Node))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    bool2 x = bufferElement2.m_Connected & ownedElement.m_Directions & !ownedElement.m_Connected;
                    if (math.any(x))
                    {
                      // ISSUE: reference to a compiler-generated field
                      ownedElement.m_Connected |= x;
                      ownedElements[index2] = ownedElement;
                      ref NativeList<ConnectionWarningSystem.BufferElement> local13 = ref nativeList;
                      // ISSUE: object of a compiler-generated type is created
                      bufferElement1 = new ConnectionWarningSystem.BufferElement();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      bufferElement1.m_Node = ownedElement.m_MiddleNode;
                      // ISSUE: reference to a compiler-generated field
                      bufferElement1.m_Connected = x;
                      ref ConnectionWarningSystem.BufferElement local14 = ref bufferElement1;
                      local13.Add(in local14);
                      ref NativeList<ConnectionWarningSystem.BufferElement> local15 = ref nativeList;
                      // ISSUE: object of a compiler-generated type is created
                      bufferElement1 = new ConnectionWarningSystem.BufferElement();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      bufferElement1.m_Node = ownedElement.m_EndNode;
                      // ISSUE: reference to a compiler-generated field
                      bufferElement1.m_Connected = x;
                      ref ConnectionWarningSystem.BufferElement local16 = ref bufferElement1;
                      local15.Add(in local16);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (ownedElement.m_MiddleNode.EqualsIgnoreCurvePos(bufferElement2.m_Node))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      bool2 x = bufferElement2.m_Connected & !ownedElement.m_Connected;
                      if (math.any(x))
                      {
                        // ISSUE: reference to a compiler-generated field
                        ownedElement.m_Connected |= x;
                        ownedElements[index2] = ownedElement;
                        // ISSUE: reference to a compiler-generated field
                        if (math.any(x & ownedElement.m_Directions))
                        {
                          ref NativeList<ConnectionWarningSystem.BufferElement> local17 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_EndNode;
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Connected = x & ownedElement.m_Directions;
                          ref ConnectionWarningSystem.BufferElement local18 = ref bufferElement1;
                          local17.Add(in local18);
                        }
                        // ISSUE: reference to a compiler-generated field
                        if (math.any(x & ownedElement.m_Directions.yx))
                        {
                          ref NativeList<ConnectionWarningSystem.BufferElement> local19 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_StartNode;
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Connected = x & ownedElement.m_Directions.yx;
                          ref ConnectionWarningSystem.BufferElement local20 = ref bufferElement1;
                          local19.Add(in local20);
                        }
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (ownedElement.m_EndNode.EqualsIgnoreCurvePos(bufferElement2.m_Node))
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        bool2 x = bufferElement2.m_Connected & ownedElement.m_Directions.yx & !ownedElement.m_Connected;
                        if (math.any(x))
                        {
                          // ISSUE: reference to a compiler-generated field
                          ownedElement.m_Connected |= x;
                          ownedElements[index2] = ownedElement;
                          ref NativeList<ConnectionWarningSystem.BufferElement> local21 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_MiddleNode;
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Connected = x;
                          ref ConnectionWarningSystem.BufferElement local22 = ref bufferElement1;
                          local21.Add(in local22);
                          ref NativeList<ConnectionWarningSystem.BufferElement> local23 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_StartNode;
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Connected = x;
                          ref ConnectionWarningSystem.BufferElement local24 = ref bufferElement1;
                          local23.Add(in local24);
                        }
                      }
                    }
                  }
                }
                while (nodeMap.TryGetNextValue(out index2, ref it));
              }
            }
          }
        }
        for (int index3 = 0; index3 < ownedElements.Length; ++index3)
        {
          // ISSUE: variable of a compiler-generated type
          ConnectionWarningSystem.PathfindElement ownedElement = ownedElements[index3];
          // ISSUE: reference to a compiler-generated field
          if (!math.all(ownedElement.m_Connected))
          {
            // ISSUE: reference to a compiler-generated field
            if (ownedElement.m_Priority > (sbyte) 0)
            {
              // ISSUE: reference to a compiler-generated field
              bool optional = ownedElement.m_Optional;
              bool flag = false;
              ref NativeList<ConnectionWarningSystem.BufferElement> local25 = ref nativeList;
              // ISSUE: object of a compiler-generated type is created
              bufferElement1 = new ConnectionWarningSystem.BufferElement();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bufferElement1.m_Node = ownedElement.m_StartNode;
              ref ConnectionWarningSystem.BufferElement local26 = ref bufferElement1;
              local25.Add(in local26);
              ref NativeList<ConnectionWarningSystem.BufferElement> local27 = ref nativeList;
              // ISSUE: object of a compiler-generated type is created
              bufferElement1 = new ConnectionWarningSystem.BufferElement();
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bufferElement1.m_Node = ownedElement.m_EndNode;
              ref ConnectionWarningSystem.BufferElement local28 = ref bufferElement1;
              local27.Add(in local28);
              while (nativeList.Length > 0)
              {
                // ISSUE: variable of a compiler-generated type
                ConnectionWarningSystem.BufferElement bufferElement3 = nativeList[nativeList.Length - 1];
                nativeList.RemoveAt(nativeList.Length - 1);
                int index4;
                NativeParallelMultiHashMapIterator<PathNode> it;
                // ISSUE: reference to a compiler-generated field
                if (nodeMap.TryGetFirstValue(bufferElement3.m_Node.StripCurvePos(), out index4, out it))
                {
                  do
                  {
                    ownedElement = ownedElements[index4];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    flag |= optional & !ownedElement.m_Optional & !math.all(ownedElement.m_Connected);
                    // ISSUE: reference to a compiler-generated field
                    if (ownedElement.m_Priority == (sbyte) 0)
                    {
                      // ISSUE: reference to a compiler-generated field
                      ownedElement.m_Priority = (sbyte) -1;
                      ownedElements[index4] = ownedElement;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (ownedElement.m_StartNode.EqualsIgnoreCurvePos(bufferElement3.m_Node))
                      {
                        ref NativeList<ConnectionWarningSystem.BufferElement> local29 = ref nativeList;
                        // ISSUE: object of a compiler-generated type is created
                        bufferElement1 = new ConnectionWarningSystem.BufferElement();
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        bufferElement1.m_Node = ownedElement.m_MiddleNode;
                        ref ConnectionWarningSystem.BufferElement local30 = ref bufferElement1;
                        local29.Add(in local30);
                        ref NativeList<ConnectionWarningSystem.BufferElement> local31 = ref nativeList;
                        // ISSUE: object of a compiler-generated type is created
                        bufferElement1 = new ConnectionWarningSystem.BufferElement();
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        bufferElement1.m_Node = ownedElement.m_EndNode;
                        ref ConnectionWarningSystem.BufferElement local32 = ref bufferElement1;
                        local31.Add(in local32);
                      }
                      else
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (ownedElement.m_MiddleNode.EqualsIgnoreCurvePos(bufferElement3.m_Node))
                        {
                          ref NativeList<ConnectionWarningSystem.BufferElement> local33 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_StartNode;
                          ref ConnectionWarningSystem.BufferElement local34 = ref bufferElement1;
                          local33.Add(in local34);
                          ref NativeList<ConnectionWarningSystem.BufferElement> local35 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_EndNode;
                          ref ConnectionWarningSystem.BufferElement local36 = ref bufferElement1;
                          local35.Add(in local36);
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (ownedElement.m_EndNode.EqualsIgnoreCurvePos(bufferElement3.m_Node))
                          {
                            ref NativeList<ConnectionWarningSystem.BufferElement> local37 = ref nativeList;
                            // ISSUE: object of a compiler-generated type is created
                            bufferElement1 = new ConnectionWarningSystem.BufferElement();
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            bufferElement1.m_Node = ownedElement.m_MiddleNode;
                            ref ConnectionWarningSystem.BufferElement local38 = ref bufferElement1;
                            local37.Add(in local38);
                            ref NativeList<ConnectionWarningSystem.BufferElement> local39 = ref nativeList;
                            // ISSUE: object of a compiler-generated type is created
                            bufferElement1 = new ConnectionWarningSystem.BufferElement();
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            bufferElement1.m_Node = ownedElement.m_StartNode;
                            ref ConnectionWarningSystem.BufferElement local40 = ref bufferElement1;
                            local39.Add(in local40);
                          }
                        }
                      }
                    }
                  }
                  while (nodeMap.TryGetNextValue(out index4, ref it));
                }
              }
              if (flag)
              {
                ownedElement = ownedElements[index3] with
                {
                  m_Optional = false
                };
                ownedElements[index3] = ownedElement;
                ref NativeList<ConnectionWarningSystem.BufferElement> local41 = ref nativeList;
                // ISSUE: object of a compiler-generated type is created
                bufferElement1 = new ConnectionWarningSystem.BufferElement();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Node = ownedElement.m_StartNode;
                ref ConnectionWarningSystem.BufferElement local42 = ref bufferElement1;
                local41.Add(in local42);
                ref NativeList<ConnectionWarningSystem.BufferElement> local43 = ref nativeList;
                // ISSUE: object of a compiler-generated type is created
                bufferElement1 = new ConnectionWarningSystem.BufferElement();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Node = ownedElement.m_EndNode;
                ref ConnectionWarningSystem.BufferElement local44 = ref bufferElement1;
                local43.Add(in local44);
                while (nativeList.Length > 0)
                {
                  // ISSUE: variable of a compiler-generated type
                  ConnectionWarningSystem.BufferElement bufferElement4 = nativeList[nativeList.Length - 1];
                  nativeList.RemoveAt(nativeList.Length - 1);
                  int index5;
                  NativeParallelMultiHashMapIterator<PathNode> it;
                  // ISSUE: reference to a compiler-generated field
                  if (nodeMap.TryGetFirstValue(bufferElement4.m_Node.StripCurvePos(), out index5, out it))
                  {
                    do
                    {
                      ownedElement = ownedElements[index5];
                      // ISSUE: reference to a compiler-generated field
                      if (ownedElement.m_Optional)
                      {
                        // ISSUE: reference to a compiler-generated field
                        ownedElement.m_Optional = false;
                        ownedElements[index5] = ownedElement;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (ownedElement.m_StartNode.EqualsIgnoreCurvePos(bufferElement4.m_Node))
                        {
                          ref NativeList<ConnectionWarningSystem.BufferElement> local45 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_MiddleNode;
                          ref ConnectionWarningSystem.BufferElement local46 = ref bufferElement1;
                          local45.Add(in local46);
                          ref NativeList<ConnectionWarningSystem.BufferElement> local47 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_EndNode;
                          ref ConnectionWarningSystem.BufferElement local48 = ref bufferElement1;
                          local47.Add(in local48);
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (ownedElement.m_MiddleNode.EqualsIgnoreCurvePos(bufferElement4.m_Node))
                          {
                            ref NativeList<ConnectionWarningSystem.BufferElement> local49 = ref nativeList;
                            // ISSUE: object of a compiler-generated type is created
                            bufferElement1 = new ConnectionWarningSystem.BufferElement();
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            bufferElement1.m_Node = ownedElement.m_StartNode;
                            ref ConnectionWarningSystem.BufferElement local50 = ref bufferElement1;
                            local49.Add(in local50);
                            ref NativeList<ConnectionWarningSystem.BufferElement> local51 = ref nativeList;
                            // ISSUE: object of a compiler-generated type is created
                            bufferElement1 = new ConnectionWarningSystem.BufferElement();
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            bufferElement1.m_Node = ownedElement.m_EndNode;
                            ref ConnectionWarningSystem.BufferElement local52 = ref bufferElement1;
                            local51.Add(in local52);
                          }
                          else
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            if (ownedElement.m_EndNode.EqualsIgnoreCurvePos(bufferElement4.m_Node))
                            {
                              ref NativeList<ConnectionWarningSystem.BufferElement> local53 = ref nativeList;
                              // ISSUE: object of a compiler-generated type is created
                              bufferElement1 = new ConnectionWarningSystem.BufferElement();
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              bufferElement1.m_Node = ownedElement.m_MiddleNode;
                              ref ConnectionWarningSystem.BufferElement local54 = ref bufferElement1;
                              local53.Add(in local54);
                              ref NativeList<ConnectionWarningSystem.BufferElement> local55 = ref nativeList;
                              // ISSUE: object of a compiler-generated type is created
                              bufferElement1 = new ConnectionWarningSystem.BufferElement();
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              bufferElement1.m_Node = ownedElement.m_StartNode;
                              ref ConnectionWarningSystem.BufferElement local56 = ref bufferElement1;
                              local55.Add(in local56);
                            }
                          }
                        }
                      }
                    }
                    while (nodeMap.TryGetNextValue(out index5, ref it));
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (ownedElement.m_SubConnection > (sbyte) 0)
              {
                ref NativeList<ConnectionWarningSystem.BufferElement> local57 = ref nativeList;
                // ISSUE: object of a compiler-generated type is created
                bufferElement1 = new ConnectionWarningSystem.BufferElement();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Node = ownedElement.m_StartNode;
                ref ConnectionWarningSystem.BufferElement local58 = ref bufferElement1;
                local57.Add(in local58);
                ref NativeList<ConnectionWarningSystem.BufferElement> local59 = ref nativeList;
                // ISSUE: object of a compiler-generated type is created
                bufferElement1 = new ConnectionWarningSystem.BufferElement();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                bufferElement1.m_Node = ownedElement.m_EndNode;
                ref ConnectionWarningSystem.BufferElement local60 = ref bufferElement1;
                local59.Add(in local60);
                while (nativeList.Length > 0)
                {
                  // ISSUE: variable of a compiler-generated type
                  ConnectionWarningSystem.BufferElement bufferElement5 = nativeList[nativeList.Length - 1];
                  nativeList.RemoveAt(nativeList.Length - 1);
                  int index6;
                  NativeParallelMultiHashMapIterator<PathNode> it;
                  // ISSUE: reference to a compiler-generated field
                  if (nodeMap.TryGetFirstValue(bufferElement5.m_Node.StripCurvePos(), out index6, out it))
                  {
                    do
                    {
                      ownedElement = ownedElements[index6];
                      // ISSUE: reference to a compiler-generated field
                      if (ownedElement.m_SubConnection == (sbyte) 0)
                      {
                        // ISSUE: reference to a compiler-generated field
                        ownedElement.m_SubConnection = (sbyte) -1;
                        ownedElements[index6] = ownedElement;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (ownedElement.m_StartNode.EqualsIgnoreCurvePos(bufferElement5.m_Node))
                        {
                          ref NativeList<ConnectionWarningSystem.BufferElement> local61 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_MiddleNode;
                          ref ConnectionWarningSystem.BufferElement local62 = ref bufferElement1;
                          local61.Add(in local62);
                          ref NativeList<ConnectionWarningSystem.BufferElement> local63 = ref nativeList;
                          // ISSUE: object of a compiler-generated type is created
                          bufferElement1 = new ConnectionWarningSystem.BufferElement();
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          bufferElement1.m_Node = ownedElement.m_EndNode;
                          ref ConnectionWarningSystem.BufferElement local64 = ref bufferElement1;
                          local63.Add(in local64);
                        }
                        else
                        {
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (ownedElement.m_MiddleNode.EqualsIgnoreCurvePos(bufferElement5.m_Node))
                          {
                            ref NativeList<ConnectionWarningSystem.BufferElement> local65 = ref nativeList;
                            // ISSUE: object of a compiler-generated type is created
                            bufferElement1 = new ConnectionWarningSystem.BufferElement();
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            bufferElement1.m_Node = ownedElement.m_StartNode;
                            ref ConnectionWarningSystem.BufferElement local66 = ref bufferElement1;
                            local65.Add(in local66);
                            ref NativeList<ConnectionWarningSystem.BufferElement> local67 = ref nativeList;
                            // ISSUE: object of a compiler-generated type is created
                            bufferElement1 = new ConnectionWarningSystem.BufferElement();
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            bufferElement1.m_Node = ownedElement.m_EndNode;
                            ref ConnectionWarningSystem.BufferElement local68 = ref bufferElement1;
                            local67.Add(in local68);
                          }
                          else
                          {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            if (ownedElement.m_EndNode.EqualsIgnoreCurvePos(bufferElement5.m_Node))
                            {
                              ref NativeList<ConnectionWarningSystem.BufferElement> local69 = ref nativeList;
                              // ISSUE: object of a compiler-generated type is created
                              bufferElement1 = new ConnectionWarningSystem.BufferElement();
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              bufferElement1.m_Node = ownedElement.m_MiddleNode;
                              ref ConnectionWarningSystem.BufferElement local70 = ref bufferElement1;
                              local69.Add(in local70);
                              ref NativeList<ConnectionWarningSystem.BufferElement> local71 = ref nativeList;
                              // ISSUE: object of a compiler-generated type is created
                              bufferElement1 = new ConnectionWarningSystem.BufferElement();
                              // ISSUE: reference to a compiler-generated field
                              // ISSUE: reference to a compiler-generated field
                              bufferElement1.m_Node = ownedElement.m_StartNode;
                              ref ConnectionWarningSystem.BufferElement local72 = ref bufferElement1;
                              local71.Add(in local72);
                            }
                          }
                        }
                      }
                    }
                    while (nodeMap.TryGetNextValue(out index6, ref it));
                  }
                }
              }
            }
          }
        }
        nativeList.Dispose();
      }

      private bool IsDeadEnd(
        Entity edge,
        Entity node,
        Entity topOwner,
        Entity owner,
        out bool maybe)
      {
        maybe = false;
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ConnectedEdges.TryGetBuffer(node, out bufferData))
          return false;
        if (bufferData.Length == 1)
          return true;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity edge1 = bufferData[index].m_Edge;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          if (!(edge1 == edge) && this.m_OwnerData.TryGetComponent(edge1, out componentData))
          {
            Entity owner1 = componentData.m_Owner;
            if (owner1 == owner)
              return false;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(owner1, out componentData))
              owner1 = componentData.m_Owner;
            if (owner1 == topOwner)
              return false;
          }
        }
        maybe = true;
        return true;
      }

      private void AddPathfindElements(
        NativeList<ConnectionWarningSystem.PathfindElement> ownedElements,
        NativeParallelMultiHashMap<PathNode, int> nodeMap,
        NativeParallelHashSet<PathNode> externalNodes,
        Entity topOwner,
        Entity owner,
        Quad3 lot,
        bool isRoad,
        bool isSubBuilding)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubNet> subNet1 = this.m_SubNets[owner];
          for (int index1 = 0; index1 < subNet1.Length; ++index1)
          {
            Entity subNet2 = subNet1[index1].m_SubNet;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            bool isRoad1 = (this.m_PrefabNetData[this.m_PrefabRefData[subNet2].m_Prefab].m_RequiredLayers & Layer.Road) > Layer.None;
            // ISSUE: reference to a compiler-generated method
            this.AddPathfindElements(ownedElements, nodeMap, externalNodes, topOwner, subNet2, lot, isRoad1, isSubBuilding);
            DynamicBuffer<ConnectedEdge> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedEdges.TryGetBuffer(subNet2, out bufferData1))
            {
              for (int index2 = 0; index2 < bufferData1.Length; ++index2)
              {
                ConnectedEdge connectedEdge1 = bufferData1[index2];
                // ISSUE: reference to a compiler-generated method
                if (this.AddExternalNodes(externalNodes, topOwner, owner, connectedEdge1.m_Edge))
                {
                  // ISSUE: reference to a compiler-generated field
                  Edge edge = this.m_EdgeData[connectedEdge1.m_Edge];
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<ConnectedNode> connectedNode1 = this.m_ConnectedNodes[connectedEdge1.m_Edge];
                  // ISSUE: reference to a compiler-generated method
                  if (this.AddExternalNodes(externalNodes, topOwner, owner, edge.m_Start))
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<ConnectedEdge> connectedEdge2 = this.m_ConnectedEdges[edge.m_Start];
                    for (int index3 = 0; index3 < connectedEdge2.Length; ++index3)
                    {
                      ConnectedEdge connectedEdge3 = connectedEdge2[index3];
                      if (connectedEdge3.m_Edge != connectedEdge1.m_Edge)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.AddExternalNodes(externalNodes, topOwner, owner, connectedEdge3.m_Edge);
                      }
                    }
                  }
                  // ISSUE: reference to a compiler-generated method
                  if (this.AddExternalNodes(externalNodes, topOwner, owner, edge.m_End))
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<ConnectedEdge> connectedEdge4 = this.m_ConnectedEdges[edge.m_End];
                    for (int index4 = 0; index4 < connectedEdge4.Length; ++index4)
                    {
                      ConnectedEdge connectedEdge5 = connectedEdge4[index4];
                      if (connectedEdge5.m_Edge != connectedEdge1.m_Edge)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.AddExternalNodes(externalNodes, topOwner, owner, connectedEdge5.m_Edge);
                      }
                    }
                  }
                  for (int index5 = 0; index5 < connectedNode1.Length; ++index5)
                  {
                    ConnectedNode connectedNode2 = connectedNode1[index5];
                    // ISSUE: reference to a compiler-generated method
                    if (this.AddExternalNodes(externalNodes, topOwner, owner, connectedNode2.m_Node))
                    {
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<ConnectedEdge> connectedEdge6 = this.m_ConnectedEdges[connectedNode2.m_Node];
                      for (int index6 = 0; index6 < connectedEdge6.Length; ++index6)
                      {
                        ConnectedEdge connectedEdge7 = connectedEdge6[index6];
                        if (connectedEdge7.m_Edge != connectedEdge1.m_Edge)
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.AddExternalNodes(externalNodes, topOwner, owner, connectedEdge7.m_Edge);
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              Edge componentData;
              DynamicBuffer<ConnectedNode> bufferData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_EdgeData.TryGetComponent(subNet2, out componentData) && this.m_ConnectedNodes.TryGetBuffer(subNet2, out bufferData2))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddExternalNodes(externalNodes, topOwner, owner, componentData.m_Start);
                // ISSUE: reference to a compiler-generated method
                this.AddExternalNodes(externalNodes, topOwner, owner, componentData.m_End);
                for (int index7 = 0; index7 < bufferData2.Length; ++index7)
                {
                  ConnectedNode connectedNode = bufferData2[index7];
                  // ISSUE: reference to a compiler-generated method
                  if (this.AddExternalNodes(externalNodes, topOwner, owner, connectedNode.m_Node))
                  {
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<ConnectedEdge> connectedEdge8 = this.m_ConnectedEdges[connectedNode.m_Node];
                    for (int index8 = 0; index8 < connectedEdge8.Length; ++index8)
                    {
                      ConnectedEdge connectedEdge9 = connectedEdge8[index8];
                      if (connectedEdge9.m_Edge != subNet2)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.AddExternalNodes(externalNodes, topOwner, owner, connectedEdge9.m_Edge);
                      }
                    }
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubAreas.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.SubArea> subArea = this.m_SubAreas[owner];
          for (int index = 0; index < subArea.Length; ++index)
          {
            Entity area = subArea[index].m_Area;
            // ISSUE: reference to a compiler-generated method
            this.AddPathfindElements(ownedElements, nodeMap, externalNodes, topOwner, area, lot, false, isSubBuilding);
          }
        }
        float t;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[owner];
          bool pedestrianIcon = true;
          bool2 maybeDeadEnd = (bool2) false;
          float2 errorLocation = (float2) -1f;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNetData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            pedestrianIcon = (this.m_PrefabNetData[prefabRef.m_Prefab].m_RequiredLayers & (Layer.TrainTrack | Layer.Waterway | Layer.TramTrack | Layer.SubwayTrack)) == Layer.None;
            float num = 0.0f;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabNetGeometryData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              NetGeometryData netGeometryData = this.m_PrefabNetGeometryData[prefabRef.m_Prefab];
              num += netGeometryData.m_DefaultWidth * 0.5f;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabLocalConnectData.HasComponent(prefabRef.m_Prefab))
            {
              // ISSUE: reference to a compiler-generated field
              LocalConnectData localConnectData = this.m_PrefabLocalConnectData[prefabRef.m_Prefab];
              num += localConnectData.m_SearchDistance;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.HasComponent(owner))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge = this.m_EdgeData[owner];
              // ISSUE: reference to a compiler-generated method
              if (this.IsDeadEnd(owner, edge.m_Start, topOwner, owner, out maybeDeadEnd.x))
              {
                // ISSUE: reference to a compiler-generated field
                Node node = this.m_NodeData[edge.m_Start];
                if (!MathUtils.Intersect(lot.xz, node.m_Position.xz) || (double) MathUtils.Distance(lot.ab.xz, node.m_Position.xz, out t) <= (double) num)
                  errorLocation.x = 0.0f;
              }
              // ISSUE: reference to a compiler-generated method
              if (this.IsDeadEnd(owner, edge.m_End, topOwner, owner, out maybeDeadEnd.y))
              {
                // ISSUE: reference to a compiler-generated field
                Node node = this.m_NodeData[edge.m_End];
                if (!MathUtils.Intersect(lot.xz, node.m_Position.xz) || (double) MathUtils.Distance(lot.ab.xz, node.m_Position.xz, out t) <= (double) num)
                  errorLocation.y = 1f;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddPathfindElements(ownedElements, nodeMap, this.m_SubLanes[owner], pedestrianIcon, isRoad, false, maybeDeadEnd, errorLocation);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(owner))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Objects.SubObject> subObject1 = this.m_SubObjects[owner];
        for (int index = 0; index < subObject1.Length; ++index)
        {
          Entity subObject2 = subObject1[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_BuildingData.HasComponent(subObject2))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddPathfindElements(ownedElements, nodeMap, externalNodes, topOwner, subObject2, lot, false, isSubBuilding);
            // ISSUE: variable of a compiler-generated type
            ConnectionWarningSystem.PathfindElement pathfindElement1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnLocationData.HasComponent(subObject2))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Objects.SpawnLocation spawnLocation = this.m_SpawnLocationData[subObject2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Prefabs.SpawnLocationData spawnLocationData = this.m_PrefabSpawnLocationData[this.m_PrefabRefData[subObject2].m_Prefab];
              // ISSUE: object of a compiler-generated type is created
              pathfindElement1 = new ConnectionWarningSystem.PathfindElement();
              // ISSUE: reference to a compiler-generated field
              pathfindElement1.m_Entity = subObject2;
              // ISSUE: reference to a compiler-generated field
              pathfindElement1.m_Directions = (bool2) true;
              // ISSUE: variable of a compiler-generated type
              ConnectionWarningSystem.PathfindElement pathfindElement2 = pathfindElement1 with
              {
                m_StartNode = new PathNode(subObject2, (ushort) 2),
                m_MiddleNode = new PathNode(subObject2, (ushort) 1),
                m_EndNode = new PathNode(subObject2, (ushort) 0)
              };
              switch (spawnLocationData.m_ConnectionType)
              {
                case RouteConnectionType.Road:
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement2.m_IconType = (byte) 1;
                  break;
                case RouteConnectionType.Pedestrian:
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement2.m_IconType = (byte) 2;
                  break;
                case RouteConnectionType.Track:
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement2.m_IconType = (byte) 3;
                  break;
                case RouteConnectionType.Air:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement2.m_IconType = (byte) math.select(1, 0, this.m_EditorMode);
                  break;
                case RouteConnectionType.Parking:
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement2.m_IconType = (byte) 1;
                  break;
                default:
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneData.HasComponent(spawnLocation.m_ConnectedLane1))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddExternalNode(externalNodes, topOwner, spawnLocation.m_ConnectedLane1);
                // ISSUE: reference to a compiler-generated field
                Lane lane = this.m_LaneData[spawnLocation.m_ConnectedLane1];
                // ISSUE: reference to a compiler-generated field
                pathfindElement2.m_StartNode = new PathNode(lane.m_MiddleNode, spawnLocation.m_CurvePosition1);
              }
              else if (spawnLocationData.m_ConnectionType == RouteConnectionType.Pedestrian)
              {
                // ISSUE: reference to a compiler-generated field
                pathfindElement2.m_CanIgnore = true;
              }
              // ISSUE: reference to a compiler-generated field
              pathfindElement2.m_SubConnection = isSubBuilding ? (sbyte) 1 : (sbyte) 0;
              int length = ownedElements.Length;
              ownedElements.Add(in pathfindElement2);
              // ISSUE: reference to a compiler-generated field
              nodeMap.Add(pathfindElement2.m_StartNode.StripCurvePos(), length);
              // ISSUE: reference to a compiler-generated field
              nodeMap.Add(pathfindElement2.m_MiddleNode.StripCurvePos(), length);
              // ISSUE: reference to a compiler-generated field
              nodeMap.Add(pathfindElement2.m_EndNode.StripCurvePos(), length);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TakeoffLocationData.HasComponent(subObject2))
              {
                // ISSUE: reference to a compiler-generated field
                AccessLane accessLane = this.m_AccessLaneData[subObject2];
                // ISSUE: reference to a compiler-generated field
                RouteLane routeLane = this.m_RouteLaneData[subObject2];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[this.m_PrefabRefData[subObject2].m_Prefab];
                // ISSUE: object of a compiler-generated type is created
                pathfindElement1 = new ConnectionWarningSystem.PathfindElement();
                // ISSUE: reference to a compiler-generated field
                pathfindElement1.m_Entity = subObject2;
                // ISSUE: reference to a compiler-generated field
                pathfindElement1.m_Directions = (bool2) true;
                // ISSUE: variable of a compiler-generated type
                ConnectionWarningSystem.PathfindElement pathfindElement3 = pathfindElement1 with
                {
                  m_StartNode = new PathNode(subObject2, (ushort) 2),
                  m_MiddleNode = new PathNode(subObject2, (ushort) 1),
                  m_EndNode = new PathNode(subObject2, (ushort) 0)
                };
                switch (routeConnectionData.m_AccessConnectionType)
                {
                  case RouteConnectionType.Road:
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement3.m_IconType = (byte) 1;
                    break;
                  case RouteConnectionType.Pedestrian:
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement3.m_IconType = (byte) 2;
                    break;
                  case RouteConnectionType.Air:
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement3.m_IconType = (byte) math.select(1, 0, this.m_EditorMode);
                    break;
                  default:
                    continue;
                }
                // ISSUE: reference to a compiler-generated field
                int num = this.m_LaneData.HasComponent(accessLane.m_Lane) ? 1 : 0;
                // ISSUE: reference to a compiler-generated field
                bool flag = this.m_LaneData.HasComponent(routeLane.m_EndLane);
                if ((num & (flag ? 1 : 0)) != 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.AddExternalNode(externalNodes, topOwner, accessLane.m_Lane);
                  // ISSUE: reference to a compiler-generated method
                  this.AddExternalNode(externalNodes, topOwner, routeLane.m_EndLane);
                }
                if (num != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  Lane lane = this.m_LaneData[accessLane.m_Lane];
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement3.m_StartNode = new PathNode(lane.m_MiddleNode, accessLane.m_CurvePos);
                }
                if (flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  Lane lane = this.m_LaneData[routeLane.m_EndLane];
                  // ISSUE: reference to a compiler-generated field
                  pathfindElement3.m_EndNode = new PathNode(lane.m_MiddleNode, routeLane.m_EndCurvePos);
                }
                if (num == 0 || !flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  Game.Objects.Transform transform = this.m_TransformData[subObject2];
                  if (!MathUtils.Intersect(lot.xz, transform.m_Position.xz) || (double) MathUtils.Distance(lot.ab.xz, transform.m_Position.xz, out t) <= 10.0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement3.m_Priority = (sbyte) 1;
                  }
                }
                int length = ownedElements.Length;
                ownedElements.Add(in pathfindElement3);
                // ISSUE: reference to a compiler-generated field
                nodeMap.Add(pathfindElement3.m_StartNode.StripCurvePos(), length);
                // ISSUE: reference to a compiler-generated field
                nodeMap.Add(pathfindElement3.m_MiddleNode.StripCurvePos(), length);
                // ISSUE: reference to a compiler-generated field
                nodeMap.Add(pathfindElement3.m_EndNode.StripCurvePos(), length);
              }
            }
          }
        }
      }

      private void AddPathfindElements(
        NativeList<ConnectionWarningSystem.PathfindElement> ownedElements,
        NativeParallelMultiHashMap<PathNode, int> nodeMap,
        DynamicBuffer<SubLane> subLanes,
        bool pedestrianIcon,
        bool isRoad,
        bool onlyExisting,
        bool2 maybeDeadEnd,
        float2 errorLocation)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ConnectionWarningSystem.PathfindElement pathfindElement = new ConnectionWarningSystem.PathfindElement()
          {
            m_Entity = subLane,
            m_IconLocation = 128
          };
          bool2 bool2 = (bool2) false;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SlaveLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              CarLane carLane = this.m_CarLaneData[subLane];
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_Directions = new bool2(true, (carLane.m_Flags & CarLaneFlags.Twoway) > ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter));
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_IconType = (byte) math.select(1, 4, isRoad);
              PrefabRef componentData1;
              CarLaneData componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.TryGetComponent(subLane, out componentData1) && this.m_PrefabCarLaneData.TryGetComponent(componentData1.m_Prefab, out componentData2))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                pathfindElement.m_IconType = (byte) math.select((int) pathfindElement.m_IconType, 5, (componentData2.m_RoadTypes & (RoadTypes.Car | RoadTypes.Watercraft)) == RoadTypes.Watercraft);
              }
            }
            else
              continue;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane))
            {
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_Directions = (bool2) true;
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_IconType = (byte) math.select(2, 4, isRoad);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_IconType = (byte) math.select(0, (int) pathfindElement.m_IconType, pedestrianIcon);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TrackLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                TrackLane trackLane = this.m_TrackLaneData[subLane];
                // ISSUE: reference to a compiler-generated field
                pathfindElement.m_Directions = new bool2(true, (trackLane.m_Flags & TrackLaneFlags.Twoway) != 0);
                // ISSUE: reference to a compiler-generated field
                pathfindElement.m_IconType = (byte) 3;
                // ISSUE: reference to a compiler-generated field
                pathfindElement.m_Optional = (trackLane.m_Flags & TrackLaneFlags.Twoway) == (TrackLaneFlags) 0;
                bool2.x = (trackLane.m_Flags & TrackLaneFlags.StartingLane) != 0;
                bool2.y = (trackLane.m_Flags & TrackLaneFlags.EndingLane) != 0;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_ConnectionLaneData.HasComponent(subLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  ConnectionLane connectionLane = this.m_ConnectionLaneData[subLane];
                  if ((connectionLane.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_Directions = (bool2) true;
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_IconType = (byte) math.select(1, 4, isRoad);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_IconType = (byte) math.select((int) pathfindElement.m_IconType, 5, (connectionLane.m_RoadTypes & (RoadTypes.Car | RoadTypes.Watercraft)) == RoadTypes.Watercraft);
                  }
                  else if ((connectionLane.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_Directions = (bool2) true;
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_IconType = (byte) math.select(2, 4, isRoad);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_IconType = (byte) math.select(0, (int) pathfindElement.m_IconType, pedestrianIcon);
                  }
                  else if ((connectionLane.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_Directions = (bool2) true;
                    // ISSUE: reference to a compiler-generated field
                    pathfindElement.m_IconType = (byte) 3;
                  }
                  else
                    continue;
                }
                else
                  continue;
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (math.any(errorLocation >= 0.0f) && pathfindElement.m_IconType != (byte) 0 && this.m_EdgeLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            EdgeLane edgeLane = this.m_EdgeLaneData[subLane];
            bool2 x1 = edgeLane.m_EdgeDelta.x == errorLocation & (bool2.x | !maybeDeadEnd);
            bool2 x2 = edgeLane.m_EdgeDelta.y == errorLocation & (bool2.y | !maybeDeadEnd);
            if (math.any(x1))
            {
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_Priority = (sbyte) 1;
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_IconLocation = (byte) 0;
            }
            else if (math.any(x2))
            {
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_Priority = (sbyte) 1;
              // ISSUE: reference to a compiler-generated field
              pathfindElement.m_IconLocation = byte.MaxValue;
            }
          }
          // ISSUE: reference to a compiler-generated field
          Lane lane = this.m_LaneData[subLane];
          LaneConnection componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneConnectionData.TryGetComponent(subLane, out componentData3))
          {
            Lane componentData4;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneData.TryGetComponent(componentData3.m_StartLane, out componentData4))
              lane.m_StartNode = new PathNode(componentData4.m_MiddleNode, componentData3.m_StartPosition);
            Lane componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneData.TryGetComponent(componentData3.m_EndLane, out componentData5))
              lane.m_EndNode = new PathNode(componentData5.m_MiddleNode, componentData3.m_EndPosition);
          }
          if (!onlyExisting || nodeMap.ContainsKey(lane.m_StartNode.StripCurvePos()) && nodeMap.ContainsKey(lane.m_EndNode.StripCurvePos()))
          {
            // ISSUE: reference to a compiler-generated field
            pathfindElement.m_StartNode = lane.m_StartNode;
            // ISSUE: reference to a compiler-generated field
            pathfindElement.m_MiddleNode = lane.m_MiddleNode;
            // ISSUE: reference to a compiler-generated field
            pathfindElement.m_EndNode = lane.m_EndNode;
            int length = ownedElements.Length;
            ownedElements.Add(in pathfindElement);
            nodeMap.Add(lane.m_StartNode.StripCurvePos(), length);
            nodeMap.Add(lane.m_MiddleNode.StripCurvePos(), length);
            nodeMap.Add(lane.m_EndNode.StripCurvePos(), length);
          }
        }
      }

      private void AddExternalNode(
        NativeParallelHashSet<PathNode> externalNodes,
        Entity topOwner,
        Entity laneEntity)
      {
        Entity entity = laneEntity;
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out componentData) && !this.m_BuildingData.HasComponent(entity))
          entity = componentData.m_Owner;
        if (entity == topOwner)
          return;
        // ISSUE: reference to a compiler-generated field
        Lane lane = this.m_LaneData[laneEntity];
        externalNodes.Add(lane.m_StartNode.StripCurvePos());
        externalNodes.Add(lane.m_MiddleNode.StripCurvePos());
        externalNodes.Add(lane.m_EndNode.StripCurvePos());
      }

      private bool AddExternalNodes(
        NativeParallelHashSet<PathNode> externalNodes,
        Entity topOwner,
        Entity owner,
        Entity netEntity)
      {
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(netEntity, out componentData))
        {
          Entity owner1 = componentData.m_Owner;
          if (owner1 == owner)
            return false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          while (this.m_OwnerData.TryGetComponent(owner1, out componentData) && !this.m_BuildingData.HasComponent(owner1))
            owner1 = componentData.m_Owner;
          if (owner1 == topOwner)
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(netEntity))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubLane> subLane = this.m_SubLanes[netEntity];
          for (int index = 0; index < subLane.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane[index].m_SubLane];
            externalNodes.Add(lane.m_StartNode.StripCurvePos());
            externalNodes.Add(lane.m_MiddleNode.StripCurvePos());
            externalNodes.Add(lane.m_EndNode.StripCurvePos());
          }
        }
        return true;
      }

      private void UpdateSubnetConnectionWarnings(Entity owner)
      {
        Layer connectedLayers = Layer.None;
        Layer disconnectedLayers1 = Layer.None;
        DynamicBuffer<SubNet> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.TryGetBuffer(owner, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckConnectedLayers(ref connectedLayers, ref disconnectedLayers1, owner, bufferData1);
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(owner, out bufferData2))
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckConnectedLayers(ref connectedLayers, ref disconnectedLayers1, bufferData2);
        }
        Layer allLayers = connectedLayers | disconnectedLayers1;
        Building componentData1;
        PrefabRef componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((disconnectedLayers1 & (Layer.PowerlineLow | Layer.PowerlineHigh | Layer.WaterPipe | Layer.SewagePipe | Layer.StormwaterPipe)) != Layer.None && this.m_BuildingData.TryGetComponent(owner, out componentData1) && this.m_PrefabRefData.TryGetComponent(componentData1.m_RoadEdge, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_PrefabNetData[componentData2.m_Prefab];
          connectedLayers |= netData.m_LocalConnectLayers & ~(Layer.PowerlineLow | Layer.WaterPipe | Layer.SewagePipe);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((disconnectedLayers1 & Layer.PowerlineHigh) != Layer.None && (connectedLayers & Layer.PowerlineLow) != Layer.None && this.m_ElectricityProducerData.HasComponent(owner) && this.m_TransformerData.HasComponent(owner))
          disconnectedLayers1 &= ~Layer.PowerlineHigh;
        Layer disconnectedLayers2 = disconnectedLayers1 & ~connectedLayers;
        if (allLayers != Layer.None)
        {
          if (bufferData1.IsCreated)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateConnectionWarnings(allLayers, connectedLayers, disconnectedLayers2, owner, bufferData1);
          }
          if (bufferData2.IsCreated)
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateConnectionWarnings(allLayers, connectedLayers, disconnectedLayers2, bufferData2);
          }
        }
        if (!bufferData1.IsCreated)
          return;
        NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection> nodeConnections = new NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection>();
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          Entity subNet = bufferData1[index1].m_SubNet;
          DynamicBuffer<ConnectedEdge> bufferData3;
          DynamicBuffer<SubLane> bufferData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.TryGetBuffer(subNet, out bufferData3) && this.m_SubLanes.TryGetBuffer(subNet, out bufferData4))
          {
            bool flag = false;
            for (int index2 = 0; index2 < bufferData3.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_OwnerData.HasComponent(bufferData3[index2].m_Edge))
              {
                flag = true;
                break;
              }
            }
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              bool allConnected = this.IsNativeMapTile(this.m_NodeData[subNet].m_Position) || this.m_OutsideConnectionData.HasComponent(subNet);
              // ISSUE: reference to a compiler-generated method
              this.FillNodeConnections(bufferData4, ref nodeConnections);
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeConnections(bufferData3, nodeConnections, subNet, allConnected, true);
              if (nodeConnections.IsCreated)
                nodeConnections.Clear();
            }
          }
        }
        if (!nodeConnections.IsCreated)
          return;
        nodeConnections.Dispose();
      }

      private void CheckConnectedLayers(
        ref Layer connectedLayers,
        ref Layer disconnectedLayers,
        DynamicBuffer<Game.Objects.SubObject> subObjects)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          DynamicBuffer<SubNet> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubNets.TryGetBuffer(subObject, out bufferData1))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckConnectedLayers(ref connectedLayers, ref disconnectedLayers, subObject, bufferData1);
          }
          DynamicBuffer<Game.Objects.SubObject> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData2))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckConnectedLayers(ref connectedLayers, ref disconnectedLayers, bufferData2);
          }
        }
      }

      private void CheckConnectedLayers(
        ref Layer connectedLayers,
        ref Layer disconnectedLayers,
        Entity owner,
        DynamicBuffer<SubNet> subNets)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.HasBuffer(subNet))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            NetData netData = this.m_PrefabNetData[this.m_PrefabRefData[subNet].m_Prefab];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[subNet];
            Layer requiredLayers = netData.m_RequiredLayers;
            Layer layer = Layer.None;
            // ISSUE: reference to a compiler-generated method
            this.FindEdgeConnections(subNet, connectedEdge, owner, netData.m_RequiredLayers, ref requiredLayers, ref layer);
            // ISSUE: reference to a compiler-generated field
            if (this.m_OutsideConnectionData.HasComponent(subNet))
              layer |= requiredLayers;
            connectedLayers |= netData.m_RequiredLayers & layer;
            disconnectedLayers |= netData.m_RequiredLayers & ~layer;
            if ((requiredLayers & ~netData.m_RequiredLayers & ~layer) != Layer.None)
            {
              // ISSUE: reference to a compiler-generated method
              this.FindSecondaryConnections(subNet, connectedEdge, ref layer);
            }
            if (requiredLayers != Layer.None)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateConnectionWarnings(subNet, Entity.Null, requiredLayers, netData.m_RequiredLayers | layer);
            }
          }
        }
      }

      private void UpdateConnectionWarnings(
        Layer allLayers,
        Layer connectedLayers,
        Layer disconnectedLayers,
        DynamicBuffer<Game.Objects.SubObject> subObjects)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          DynamicBuffer<SubNet> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubNets.TryGetBuffer(subObject, out bufferData1))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateConnectionWarnings(allLayers, connectedLayers, disconnectedLayers, subObject, bufferData1);
          }
          DynamicBuffer<Game.Objects.SubObject> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData2))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateConnectionWarnings(allLayers, connectedLayers, disconnectedLayers, bufferData2);
          }
        }
      }

      private void UpdateConnectionWarnings(
        Layer allLayers,
        Layer connectedLayers,
        Layer disconnectedLayers,
        Entity owner,
        DynamicBuffer<SubNet> subNets)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedEdges.HasBuffer(subNet))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Layer required = this.m_PrefabNetData[this.m_PrefabRefData[subNet].m_Prefab].m_RequiredLayers & allLayers;
            if (required != Layer.None)
            {
              // ISSUE: reference to a compiler-generated method
              this.UpdateConnectionWarnings(owner, subNet, required, connectedLayers | ~disconnectedLayers);
            }
          }
        }
      }

      private void UpdateNodeConnectionWarnings(Entity node, bool allConnected)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        NetData netData = this.m_PrefabNetData[this.m_PrefabRefData[node].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[node];
        Layer connectedOnce = Layer.None;
        Layer connected = Layer.None;
        // ISSUE: reference to a compiler-generated method
        this.FindEdgeConnections(node, connectedEdge, netData.m_RequiredLayers, ref connectedOnce, ref connected);
        Layer required = netData.m_RequiredLayers | connectedOnce;
        // ISSUE: reference to a compiler-generated field
        allConnected |= this.m_OutsideConnectionData.HasComponent(node);
        if ((required & ~connected) != Layer.None)
        {
          // ISSUE: reference to a compiler-generated method
          this.FindSecondaryConnections(node, connectedEdge, ref connected);
          if (allConnected)
            connected |= connectedOnce;
        }
        if (required != Layer.None)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateConnectionWarnings(node, Entity.Null, required, connected);
        }
        DynamicBuffer<SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.TryGetBuffer(node, out bufferData))
          return;
        NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection> nodeConnections = new NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection>();
        // ISSUE: reference to a compiler-generated method
        this.FillNodeConnections(bufferData, ref nodeConnections);
        // ISSUE: reference to a compiler-generated method
        this.CheckNodeConnections(connectedEdge, nodeConnections, node, allConnected, false);
        if (!nodeConnections.IsCreated)
          return;
        nodeConnections.Dispose();
      }

      private void FillNodeConnections(
        DynamicBuffer<SubLane> subLanes,
        ref NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection> nodeConnections)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[subLane];
          CarLaneData componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabCarLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData1) && !this.m_SlaveLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane];
            if (!nodeConnections.IsCreated)
              nodeConnections = new NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection>(subLanes.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: variable of a compiler-generated type
            ConnectionWarningSystem.Connection connection;
            if (nodeConnections.TryGetValue(lane.m_StartNode, out connection))
            {
              // ISSUE: reference to a compiler-generated field
              if ((connection.m_RoadTypes & componentData1.m_RoadTypes) != componentData1.m_RoadTypes)
              {
                // ISSUE: reference to a compiler-generated field
                connection.m_RoadTypes |= componentData1.m_RoadTypes;
                nodeConnections[lane.m_StartNode] = connection;
              }
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              nodeConnections.Add(lane.m_StartNode, new ConnectionWarningSystem.Connection()
              {
                m_RoadTypes = componentData1.m_RoadTypes
              });
            }
            if (nodeConnections.TryGetValue(lane.m_EndNode, out connection))
            {
              // ISSUE: reference to a compiler-generated field
              if ((connection.m_RoadTypes & componentData1.m_RoadTypes) != componentData1.m_RoadTypes)
              {
                // ISSUE: reference to a compiler-generated field
                connection.m_RoadTypes |= componentData1.m_RoadTypes;
                nodeConnections[lane.m_EndNode] = connection;
              }
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              nodeConnections.Add(lane.m_EndNode, new ConnectionWarningSystem.Connection()
              {
                m_RoadTypes = componentData1.m_RoadTypes
              });
            }
          }
          TrackLaneData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabTrackLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane];
            if (!nodeConnections.IsCreated)
              nodeConnections = new NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection>(subLanes.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: variable of a compiler-generated type
            ConnectionWarningSystem.Connection connection;
            if (nodeConnections.TryGetValue(lane.m_StartNode, out connection))
            {
              // ISSUE: reference to a compiler-generated field
              if ((connection.m_TrackTypes & componentData2.m_TrackTypes) != componentData2.m_TrackTypes)
              {
                // ISSUE: reference to a compiler-generated field
                connection.m_TrackTypes |= componentData2.m_TrackTypes;
                nodeConnections[lane.m_StartNode] = connection;
              }
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              nodeConnections.Add(lane.m_StartNode, new ConnectionWarningSystem.Connection()
              {
                m_TrackTypes = componentData2.m_TrackTypes
              });
            }
            if (nodeConnections.TryGetValue(lane.m_EndNode, out connection))
            {
              // ISSUE: reference to a compiler-generated field
              if ((connection.m_TrackTypes & componentData2.m_TrackTypes) != componentData2.m_TrackTypes)
              {
                // ISSUE: reference to a compiler-generated field
                connection.m_TrackTypes |= componentData2.m_TrackTypes;
                nodeConnections[lane.m_EndNode] = connection;
              }
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              nodeConnections.Add(lane.m_EndNode, new ConnectionWarningSystem.Connection()
              {
                m_TrackTypes = componentData2.m_TrackTypes
              });
            }
          }
        }
      }

      private void CheckNodeConnections(
        DynamicBuffer<ConnectedEdge> connectedEdges,
        NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection> nodeConnections,
        Entity node,
        bool allConnected,
        bool standaloneOnly)
      {
        for (int index = 0; index < connectedEdges.Length; ++index)
        {
          Entity edge1 = connectedEdges[index].m_Edge;
          DynamicBuffer<SubLane> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((!standaloneOnly || !this.m_OwnerData.HasComponent(edge1)) && this.m_SubLanes.TryGetBuffer(edge1, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            Edge edge2 = this.m_EdgeData[edge1];
            if (edge2.m_Start == node)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeConnections(bufferData, nodeConnections, 0.0f, allConnected);
            }
            else if (edge2.m_End == node)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckNodeConnections(bufferData, nodeConnections, 1f, allConnected);
            }
          }
        }
      }

      private void CheckNodeConnections(
        DynamicBuffer<SubLane> subLanes,
        NativeParallelHashMap<PathNode, ConnectionWarningSystem.Connection> nodeConnections,
        float edgeDelta,
        bool allConnected)
      {
        for (int index = 0; index < subLanes.Length; ++index)
        {
          Entity subLane = subLanes[index].m_SubLane;
          EdgeLane componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeLaneData.TryGetComponent(subLane, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            Lane lane = this.m_LaneData[subLane];
            // ISSUE: reference to a compiler-generated field
            Curve curve = this.m_CurveData[subLane];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[subLane];
            CarLane componentData2;
            CarLaneData componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_CarLaneData.TryGetComponent(subLane, out componentData2) && !this.m_SlaveLaneData.HasComponent(subLane) && this.m_PrefabCarLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
            {
              if ((double) componentData1.m_EdgeDelta.x == (double) edgeDelta)
              {
                // ISSUE: variable of a compiler-generated type
                ConnectionWarningSystem.Connection connection;
                // ISSUE: reference to a compiler-generated field
                if (allConnected || (componentData2.m_Flags & CarLaneFlags.Twoway) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || nodeConnections.IsCreated && nodeConnections.TryGetValue(lane.m_StartNode, out connection) && (connection.m_RoadTypes & componentData3.m_RoadTypes) == componentData3.m_RoadTypes)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(subLane, this.m_TrafficConfigurationData.m_DeadEndNotification, Entity.Null);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(subLane, this.m_TrafficConfigurationData.m_DeadEndNotification, curve.m_Bezier.a, IconPriority.Warning);
                }
              }
              if ((double) componentData1.m_EdgeDelta.y == (double) edgeDelta)
              {
                // ISSUE: variable of a compiler-generated type
                ConnectionWarningSystem.Connection connection;
                // ISSUE: reference to a compiler-generated field
                if (allConnected || (componentData2.m_Flags & CarLaneFlags.Twoway) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter) || nodeConnections.IsCreated && nodeConnections.TryGetValue(lane.m_EndNode, out connection) && (connection.m_RoadTypes & componentData3.m_RoadTypes) == componentData3.m_RoadTypes)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(subLane, this.m_TrafficConfigurationData.m_DeadEndNotification, Entity.Null, IconFlags.SecondaryLocation);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(subLane, this.m_TrafficConfigurationData.m_DeadEndNotification, curve.m_Bezier.d, IconPriority.Warning, flags: IconFlags.SecondaryLocation);
                }
              }
            }
            TrackLane componentData4;
            TrackLaneData componentData5;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrackLaneData.TryGetComponent(subLane, out componentData4) && this.m_PrefabTrackLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData5))
            {
              if ((double) componentData1.m_EdgeDelta.x == (double) edgeDelta)
              {
                // ISSUE: variable of a compiler-generated type
                ConnectionWarningSystem.Connection connection;
                // ISSUE: reference to a compiler-generated field
                if (allConnected || (componentData4.m_Flags & TrackLaneFlags.Twoway) != (TrackLaneFlags) 0 || nodeConnections.IsCreated && nodeConnections.TryGetValue(lane.m_StartNode, out connection) && (connection.m_TrackTypes & componentData5.m_TrackTypes) == componentData5.m_TrackTypes)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(subLane, this.m_TrafficConfigurationData.m_TrackConnectionNotification, Entity.Null);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(subLane, this.m_TrafficConfigurationData.m_TrackConnectionNotification, curve.m_Bezier.a, IconPriority.Warning);
                }
              }
              if ((double) componentData1.m_EdgeDelta.y == (double) edgeDelta)
              {
                // ISSUE: variable of a compiler-generated type
                ConnectionWarningSystem.Connection connection;
                // ISSUE: reference to a compiler-generated field
                if (allConnected || (componentData4.m_Flags & TrackLaneFlags.Twoway) != (TrackLaneFlags) 0 || nodeConnections.IsCreated && nodeConnections.TryGetValue(lane.m_EndNode, out connection) && (connection.m_TrackTypes & componentData5.m_TrackTypes) == componentData5.m_TrackTypes)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Remove(subLane, this.m_TrafficConfigurationData.m_TrackConnectionNotification, Entity.Null, IconFlags.SecondaryLocation);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_IconCommandBuffer.Add(subLane, this.m_TrafficConfigurationData.m_TrackConnectionNotification, curve.m_Bezier.d, IconPriority.Warning, flags: IconFlags.SecondaryLocation);
                }
              }
            }
          }
        }
      }

      private void FindEdgeConnections(
        Entity node,
        DynamicBuffer<ConnectedEdge> connectedEdges,
        Layer nodeLayers,
        ref Layer connectedOnce,
        ref Layer connectedTwice)
      {
        for (int index = 0; index < connectedEdges.Length; ++index)
        {
          Entity edge1 = connectedEdges[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_PrefabNetData[this.m_PrefabRefData[edge1].m_Prefab];
          Layer layer = netData.m_RequiredLayers | nodeLayers & netData.m_LocalConnectLayers;
          connectedTwice |= edge2.m_Start == node || edge2.m_End == node ? connectedOnce & layer : layer;
          connectedOnce |= layer;
        }
      }

      private void FindEdgeConnections(
        Entity node,
        DynamicBuffer<ConnectedEdge> connectedEdges,
        Entity owner,
        Layer nodeLayers,
        ref Layer connectedOnce,
        ref Layer connectedTwice)
      {
        Layer layer1 = Layer.None;
        Layer layer2 = Layer.None;
        for (int index = 0; index < connectedEdges.Length; ++index)
        {
          Entity edge1 = connectedEdges[index].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetData netData = this.m_PrefabNetData[this.m_PrefabRefData[edge1].m_Prefab];
          Layer layer3 = netData.m_RequiredLayers | nodeLayers & netData.m_LocalConnectLayers;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(edge1, out componentData) && componentData.m_Owner == owner)
          {
            connectedTwice |= edge2.m_Start == node || edge2.m_End == node ? layer1 & layer3 : layer3;
            layer1 |= layer3;
          }
          else
          {
            connectedTwice |= edge2.m_Start == node || edge2.m_End == node ? layer2 & layer3 : layer3;
            layer2 |= layer3;
          }
        }
        connectedTwice |= (layer1 | connectedOnce) & layer2;
        connectedOnce |= layer1 | layer2;
      }

      private void FindSecondaryConnections(
        Entity node,
        DynamicBuffer<ConnectedEdge> connectedEdges,
        ref Layer connected)
      {
        for (int index1 = 0; index1 < connectedEdges.Length; ++index1)
        {
          Entity edge1 = connectedEdges[index1].m_Edge;
          // ISSUE: reference to a compiler-generated field
          Edge edge2 = this.m_EdgeData[edge1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetData netData1 = this.m_PrefabNetData[this.m_PrefabRefData[edge1].m_Prefab];
          if (edge2.m_Start == node)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedNode> connectedNode1 = this.m_ConnectedNodes[edge1];
            for (int index2 = 0; index2 < connectedNode1.Length; ++index2)
            {
              ConnectedNode connectedNode2 = connectedNode1[index2];
              if ((double) connectedNode2.m_CurvePosition <= 0.5)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                NetData netData2 = this.m_PrefabNetData[this.m_PrefabRefData[connectedNode2.m_Node].m_Prefab];
                connected |= netData1.m_RequiredLayers & netData2.m_RequiredLayers;
              }
            }
          }
          else if (edge2.m_End == node)
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<ConnectedNode> connectedNode3 = this.m_ConnectedNodes[edge1];
            for (int index3 = 0; index3 < connectedNode3.Length; ++index3)
            {
              ConnectedNode connectedNode4 = connectedNode3[index3];
              if ((double) connectedNode4.m_CurvePosition >= 0.5)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                NetData netData3 = this.m_PrefabNetData[this.m_PrefabRefData[connectedNode4.m_Node].m_Prefab];
                connected |= netData1.m_RequiredLayers & netData3.m_RequiredLayers;
              }
            }
          }
        }
      }

      private void UpdateConnectionWarnings(
        Entity owner,
        Entity subNet,
        Layer required,
        Layer connected)
      {
        if ((required & (Layer.WaterPipe | Layer.SewagePipe)) != Layer.None)
        {
          Layer layer = required & ~connected;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateConnectionWarning(owner, subNet, this.m_WaterPipeParameterData.m_WaterPipeNotConnectedNotification, (layer & Layer.WaterPipe) > Layer.None);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.UpdateConnectionWarning(owner, subNet, this.m_WaterPipeParameterData.m_SewagePipeNotConnectedNotification, (layer & Layer.SewagePipe) > Layer.None);
        }
        if ((required & (Layer.PowerlineLow | Layer.PowerlineHigh)) == Layer.None)
          return;
        Layer layer1 = required & ~connected;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateConnectionWarning(owner, subNet, this.m_ElectricityParameterData.m_LowVoltageNotConnectedPrefab, (layer1 & Layer.PowerlineLow) > Layer.None);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.UpdateConnectionWarning(owner, subNet, this.m_ElectricityParameterData.m_HighVoltageNotConnectedPrefab, (layer1 & Layer.PowerlineHigh) > Layer.None);
      }

      private void UpdateConnectionWarning(Entity owner, Entity subNet, Entity icon, bool active)
      {
        if (!(icon != Entity.Null))
          return;
        if (subNet != Entity.Null)
        {
          if (active)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Add(owner, icon, IconPriority.Warning, flags: IconFlags.TargetLocation, target: subNet);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(owner, icon, subNet);
          }
        }
        else if (active)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(owner, icon, IconPriority.Warning);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(owner, icon);
        }
      }

      public struct MapTileIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public int m_Result;
        public float3 m_Position;
        public ComponentLookup<Native> m_NativeData;
        public ComponentLookup<MapTile> m_MapTileData;
        public BufferLookup<Game.Areas.Node> m_AreaNodes;
        public BufferLookup<Triangle> m_AreaTriangles;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_Result == 0 && MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position.xz);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.Intersect(bounds) || !this.m_MapTileData.HasComponent(item.m_Area) || !MathUtils.Intersect(AreaUtils.GetTriangle3(this.m_AreaNodes[item.m_Area], this.m_AreaTriangles[item.m_Area][item.m_Triangle]).xz, this.m_Position.xz, out float2 _))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Result = math.select(1, 2, this.m_NativeData.HasComponent(item.m_Area));
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
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RoadConnectionUpdated> __Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneConnection> __Game_Net_LaneConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OutsideConnection> __Game_Net_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Common.Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TakeoffLocation> __Game_Routes_TakeoffLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccessLane> __Game_Routes_AccessLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityProducer> __Game_Buildings_ElectricityProducer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Transformer> __Game_Buildings_Transformer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MapTile> __Game_Areas_MapTile_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetData> __Game_Prefabs_NetData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalConnectData> __Game_Prefabs_LocalConnectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedNode> __Game_Net_ConnectedNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<IconElement> __Game_Notifications_IconElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RoadConnectionUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RoadConnectionUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneConnection_RO_ComponentLookup = state.GetComponentLookup<LaneConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Game.Common.Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TakeoffLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TakeoffLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_AccessLane_RO_ComponentLookup = state.GetComponentLookup<AccessLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RO_ComponentLookup = state.GetComponentLookup<ElectricityProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Transformer_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Transformer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapTile_RO_ComponentLookup = state.GetComponentLookup<MapTile>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RO_ComponentLookup = state.GetComponentLookup<NetData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalConnectData_RO_ComponentLookup = state.GetComponentLookup<LocalConnectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedNode_RO_BufferLookup = state.GetBufferLookup<ConnectedNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferLookup = state.GetBufferLookup<IconElement>(true);
      }
    }
  }
}
