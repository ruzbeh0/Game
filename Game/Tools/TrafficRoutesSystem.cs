// Decompiled with JetBrains decompiler
// Type: Game.Tools.TrafficRoutesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class TrafficRoutesSystem : GameSystemBase
  {
    private ModificationBarrier2 m_ModificationBarrier;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_LivePathQuery;
    private EntityQuery m_PathSourceQuery;
    private EntityQuery m_RouteConfigQuery;
    private int m_UpdateFrameIndex;
    private TrafficRoutesSystem.TypeHandle __TypeHandle;

    public bool routesVisible { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LivePathQuery = this.GetEntityQuery(ComponentType.ReadOnly<LivePath>(), ComponentType.ReadOnly<Route>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PathSourceQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<PathOwner>(),
          ComponentType.ReadOnly<TrainCurrentLane>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RouteConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<RouteConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateFrameIndex = -1;
      this.routesVisible = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      Entity entity = this.routesVisible ? this.m_ToolSystem.selected : Entity.Null;
      // ISSUE: reference to a compiler-generated field
      if (entity == Entity.Null && this.m_LivePathQuery.IsEmptyIgnoreFilter)
        return;
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_LivePathQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>();
      JobHandle jobHandle1 = this.Dependency;
      if (!this.EntityManager.HasComponent<Building>(entity))
      {
        EntityManager entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Aggregate>(entity))
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Game.Net.Node>(entity))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Game.Net.Edge>(entity))
            {
              entityManager = this.EntityManager;
              if (!entityManager.HasComponent<Game.Routes.TransportStop>(entity))
              {
                entityManager = this.EntityManager;
                if (!entityManager.HasComponent<Game.Objects.OutsideConnection>(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_UpdateFrameIndex = -1;
                  goto label_11;
                }
              }
            }
          }
        }
      }
      NativeHashSet<Entity> nativeHashSet = new NativeHashSet<Entity>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      if (++this.m_UpdateFrameIndex == 16)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UpdateFrameIndex = 0;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_PathSourceQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PathSourceQuery.AddSharedComponentFilter<UpdateFrame>(new UpdateFrame((uint) this.m_UpdateFrameIndex));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      TrafficRoutesSystem.FillTargetMapJob jobData1 = new TrafficRoutesSystem.FillTargetMapJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_AggregateElements = this.__TypeHandle.__Game_Net_AggregateElement_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_ConnectedRoutes = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup,
        m_SelectedEntity = entity,
        m_SelectedIndex = this.m_ToolSystem.selectedIndex,
        m_TargetMap = nativeHashSet
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TrafficRoutesSystem.FindPathSourcesJob jobData2 = new TrafficRoutesSystem.FindPathSourcesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_HumanCurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle,
        m_CarCurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle,
        m_WatercraftCurrentLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle,
        m_AircraftCurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle,
        m_TrainCurrentLaneType = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle,
        m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
        m_PathElementType = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferTypeHandle,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RO_BufferTypeHandle,
        m_WatercraftNavigationLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RO_BufferTypeHandle,
        m_AircraftNavigationLaneType = this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RO_BufferTypeHandle,
        m_TrainNavigationLaneType = this.__TypeHandle.__Game_Vehicles_TrainNavigationLane_RO_BufferTypeHandle,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_TargetMap = nativeHashSet,
        m_PathSourceQueue = nativeQueue.AsParallelWriter()
      };
      JobHandle jobHandle2 = jobData1.Schedule<TrafficRoutesSystem.FillTargetMapJob>(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery pathSourceQuery = this.m_PathSourceQuery;
      JobHandle dependsOn = jobHandle2;
      JobHandle inputDeps = jobData2.ScheduleParallel<TrafficRoutesSystem.FindPathSourcesJob>(pathSourceQuery, dependsOn);
      nativeHashSet.Dispose(inputDeps);
      jobHandle1 = inputDeps;
label_11:
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathSource_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle3 = new TrafficRoutesSystem.UpdateLivePathsJob()
      {
        m_LivePathChunks = archetypeChunkListAsync,
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_RouteSegmentType = this.__TypeHandle.__Game_Routes_RouteSegment_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_PathSourceData = this.__TypeHandle.__Game_Routes_PathSource_RO_ComponentLookup,
        m_HumanData = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_WatercraftData = this.__TypeHandle.__Game_Vehicles_Watercraft_RO_ComponentLookup,
        m_AircraftData = this.__TypeHandle.__Game_Vehicles_Aircraft_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_PrefabRouteData = this.__TypeHandle.__Game_Prefabs_RouteData_RO_ComponentLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_SelectedEntity = entity,
        m_UpdateFrameIndex = this.m_UpdateFrameIndex,
        m_SourceCountLimit = 200,
        m_RouteConfigurationData = this.m_RouteConfigQuery.GetSingleton<RouteConfigurationData>(),
        m_PathSourceQueue = nativeQueue,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<TrafficRoutesSystem.UpdateLivePathsJob>(JobHandle.CombineDependencies(outJobHandle, jobHandle1));
      archetypeChunkListAsync.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle3);
      if (nativeQueue.IsCreated)
        nativeQueue.Dispose(jobHandle3);
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
    public TrafficRoutesSystem()
    {
    }

    private struct LivePathEntityData
    {
      public Entity m_Entity;
      public int m_SegmentCount;
      public bool m_HasNewSegments;
    }

    [BurstCompile]
    private struct FillTargetMapJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnectionData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BufferLookup<AggregateElement> m_AggregateElements;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> m_ConnectedRoutes;
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public int m_SelectedIndex;
      public NativeHashSet<Entity> m_TargetMap;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TargetMap.Add(this.m_SelectedEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddSubLanes(this.m_SelectedEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddSubNets(this.m_SelectedEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddSubAreas(this.m_SelectedEntity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.AddSubObjects(this.m_SelectedEntity);
        DynamicBuffer<SpawnLocationElement> bufferData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocations.TryGetBuffer(this.m_SelectedEntity, out bufferData1))
        {
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TargetMap.Add(bufferData1[index].m_SpawnLocation);
          }
          Attached componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_AttachedData.TryGetComponent(this.m_SelectedEntity, out componentData))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddSubLanes(componentData.m_Parent);
            // ISSUE: reference to a compiler-generated method
            this.AddSubNets(componentData.m_Parent);
            // ISSUE: reference to a compiler-generated method
            this.AddSubAreas(componentData.m_Parent);
            // ISSUE: reference to a compiler-generated method
            this.AddSubObjects(componentData.m_Parent);
          }
        }
        DynamicBuffer<Renter> bufferData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Renters.TryGetBuffer(this.m_SelectedEntity, out bufferData2))
        {
          for (int index = 0; index < bufferData2.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TargetMap.Add(bufferData2[index].m_Renter);
          }
        }
        DynamicBuffer<AggregateElement> bufferData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_AggregateElements.TryGetBuffer(this.m_SelectedEntity, out bufferData3))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedIndex >= 0 && this.m_SelectedIndex < bufferData3.Length)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.AddSubLanes(bufferData3[this.m_SelectedIndex].m_Edge);
          }
          else
          {
            for (int index = 0; index < bufferData3.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddSubLanes(bufferData3[index].m_Edge);
            }
          }
        }
        DynamicBuffer<ConnectedRoute> bufferData4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedRoutes.TryGetBuffer(this.m_SelectedEntity, out bufferData4))
        {
          for (int index = 0; index < bufferData4.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TargetMap.Add(bufferData4[index].m_Waypoint);
          }
        }
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OutsideConnectionData.HasComponent(this.m_SelectedEntity) || !this.m_OwnerData.TryGetComponent(this.m_SelectedEntity, out componentData1))
          return;
        // ISSUE: reference to a compiler-generated method
        this.AddSubLanes(componentData1.m_Owner);
      }

      private void AddSubObjects(Entity entity)
      {
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Game.Objects.SubObject subObject = bufferData[index];
          // ISSUE: reference to a compiler-generated method
          this.AddSubLanes(subObject.m_SubObject);
          // ISSUE: reference to a compiler-generated method
          this.AddSubNets(subObject.m_SubObject);
          // ISSUE: reference to a compiler-generated method
          this.AddSubAreas(subObject.m_SubObject);
          // ISSUE: reference to a compiler-generated method
          this.AddSubObjects(subObject.m_SubObject);
        }
      }

      private void AddSubNets(Entity entity)
      {
        DynamicBuffer<Game.Net.SubNet> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubNets.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.AddSubLanes(bufferData[index].m_SubNet);
        }
      }

      private void AddSubAreas(Entity entity)
      {
        DynamicBuffer<Game.Areas.SubArea> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubAreas.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Game.Areas.SubArea subArea = bufferData[index];
          // ISSUE: reference to a compiler-generated method
          this.AddSubLanes(subArea.m_Area);
          // ISSUE: reference to a compiler-generated method
          this.AddSubAreas(subArea.m_Area);
        }
      }

      private void AddSubLanes(Entity entity)
      {
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Game.Net.SubLane subLane = bufferData[index];
          if (subLane.m_PathMethods != (PathMethod) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TargetMap.Add(subLane.m_SubLane);
          }
        }
      }
    }

    [BurstCompile]
    private struct FindPathSourcesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> m_HumanCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> m_CarCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<WatercraftCurrentLane> m_WatercraftCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<AircraftCurrentLane> m_AircraftCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> m_TrainCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public BufferTypeHandle<CarNavigationLane> m_CarNavigationLaneType;
      [ReadOnly]
      public BufferTypeHandle<WatercraftNavigationLane> m_WatercraftNavigationLaneType;
      [ReadOnly]
      public BufferTypeHandle<AircraftNavigationLane> m_AircraftNavigationLaneType;
      [ReadOnly]
      public BufferTypeHandle<TrainNavigationLane> m_TrainNavigationLaneType;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public NativeHashSet<Entity> m_TargetMap;
      public NativeQueue<Entity>.ParallelWriter m_PathSourceQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray2 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray3 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray4 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
        // ISSUE: reference to a compiler-generated field
        bool flag = nativeArray4.Length != 0 && !chunk.Has<TransformFrame>(ref this.m_TransformFrames);
        NativeArray<HumanCurrentLane> nativeArray5 = new NativeArray<HumanCurrentLane>();
        NativeArray<CarCurrentLane> buffer1 = new NativeArray<CarCurrentLane>();
        NativeArray<WatercraftCurrentLane> buffer2 = new NativeArray<WatercraftCurrentLane>();
        NativeArray<AircraftCurrentLane> buffer3 = new NativeArray<AircraftCurrentLane>();
        NativeArray<TrainCurrentLane> buffer4 = new NativeArray<TrainCurrentLane>();
        NativeArray<Controller> buffer5 = new NativeArray<Controller>();
        BufferAccessor<CarNavigationLane> bufferAccessor2 = new BufferAccessor<CarNavigationLane>();
        BufferAccessor<WatercraftNavigationLane> bufferAccessor3 = new BufferAccessor<WatercraftNavigationLane>();
        BufferAccessor<AircraftNavigationLane> bufferAccessor4 = new BufferAccessor<AircraftNavigationLane>();
        BufferAccessor<TrainNavigationLane> bufferAccessor5 = new BufferAccessor<TrainNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanCurrentLane> nativeArray6 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
        if (nativeArray6.Length == 0 && nativeArray4.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          buffer1 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CarCurrentLaneType);
          // ISSUE: reference to a compiler-generated field
          buffer5 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
          if (buffer1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            bufferAccessor2 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            buffer2 = chunk.GetNativeArray<WatercraftCurrentLane>(ref this.m_WatercraftCurrentLaneType);
            if (buffer2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              bufferAccessor3 = chunk.GetBufferAccessor<WatercraftNavigationLane>(ref this.m_WatercraftNavigationLaneType);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              buffer3 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_AircraftCurrentLaneType);
              if (buffer3.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                bufferAccessor4 = chunk.GetBufferAccessor<AircraftNavigationLane>(ref this.m_AircraftNavigationLaneType);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                buffer4 = chunk.GetNativeArray<TrainCurrentLane>(ref this.m_TrainCurrentLaneType);
                if (buffer4.Length != 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  bufferAccessor5 = chunk.GetBufferAccessor<TrainNavigationLane>(ref this.m_TrainNavigationLaneType);
                }
              }
            }
          }
        }
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          PathOwner pathOwner;
          DynamicBuffer<PathElement> dynamicBuffer1;
          if (CollectionUtils.TryGet<PathOwner>(nativeArray3, index1, out pathOwner) && CollectionUtils.TryGet<PathElement>(bufferAccessor1, index1, out dynamicBuffer1))
          {
            for (int elementIndex = pathOwner.m_ElementIndex; elementIndex < dynamicBuffer1.Length; ++elementIndex)
            {
              PathElement pathElement = dynamicBuffer1[elementIndex];
              // ISSUE: reference to a compiler-generated field
              if ((pathElement.m_Flags & PathElementFlags.Action) == (PathElementFlags) 0 && this.m_TargetMap.Contains(pathElement.m_Target))
                goto label_47;
            }
          }
          Target target;
          // ISSUE: reference to a compiler-generated field
          if (!CollectionUtils.TryGet<Target>(nativeArray2, index1, out target) || !this.m_TargetMap.Contains(target.m_Target))
          {
            HumanCurrentLane humanCurrentLane;
            if (CollectionUtils.TryGet<HumanCurrentLane>(nativeArray6, index1, out humanCurrentLane))
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_TargetMap.Contains(humanCurrentLane.m_Lane))
                continue;
            }
            else
            {
              CarCurrentLane carCurrentLane;
              if (CollectionUtils.TryGet<CarCurrentLane>(buffer1, index1, out carCurrentLane))
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_TargetMap.Contains(carCurrentLane.m_Lane))
                {
                  DynamicBuffer<CarNavigationLane> dynamicBuffer2;
                  if (CollectionUtils.TryGet<CarNavigationLane>(bufferAccessor2, index1, out dynamicBuffer2))
                  {
                    for (int index2 = 0; index2 < dynamicBuffer2.Length; ++index2)
                    {
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_TargetMap.Contains(dynamicBuffer2[index2].m_Lane))
                        goto label_47;
                    }
                    continue;
                  }
                  continue;
                }
              }
              else
              {
                WatercraftCurrentLane watercraftCurrentLane;
                if (CollectionUtils.TryGet<WatercraftCurrentLane>(buffer2, index1, out watercraftCurrentLane))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_TargetMap.Contains(watercraftCurrentLane.m_Lane))
                  {
                    DynamicBuffer<WatercraftNavigationLane> dynamicBuffer3;
                    if (CollectionUtils.TryGet<WatercraftNavigationLane>(bufferAccessor3, index1, out dynamicBuffer3))
                    {
                      for (int index3 = 0; index3 < dynamicBuffer3.Length; ++index3)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_TargetMap.Contains(dynamicBuffer3[index3].m_Lane))
                          goto label_47;
                      }
                      continue;
                    }
                    continue;
                  }
                }
                else
                {
                  AircraftCurrentLane aircraftCurrentLane;
                  if (CollectionUtils.TryGet<AircraftCurrentLane>(buffer3, index1, out aircraftCurrentLane))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_TargetMap.Contains(aircraftCurrentLane.m_Lane))
                    {
                      DynamicBuffer<AircraftNavigationLane> dynamicBuffer4;
                      if (CollectionUtils.TryGet<AircraftNavigationLane>(bufferAccessor4, index1, out dynamicBuffer4))
                      {
                        for (int index4 = 0; index4 < dynamicBuffer4.Length; ++index4)
                        {
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_TargetMap.Contains(dynamicBuffer4[index4].m_Lane))
                            goto label_47;
                        }
                        continue;
                      }
                      continue;
                    }
                  }
                  else
                  {
                    TrainCurrentLane trainCurrentLane;
                    if (CollectionUtils.TryGet<TrainCurrentLane>(buffer4, index1, out trainCurrentLane))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (!this.m_TargetMap.Contains(trainCurrentLane.m_Front.m_Lane) && !this.m_TargetMap.Contains(trainCurrentLane.m_Rear.m_Lane))
                      {
                        DynamicBuffer<TrainNavigationLane> dynamicBuffer5;
                        if (CollectionUtils.TryGet<TrainNavigationLane>(bufferAccessor5, index1, out dynamicBuffer5))
                        {
                          for (int index5 = 0; index5 < dynamicBuffer5.Length; ++index5)
                          {
                            // ISSUE: reference to a compiler-generated field
                            if (this.m_TargetMap.Contains(dynamicBuffer5[index5].m_Lane))
                              goto label_47;
                          }
                          continue;
                        }
                        continue;
                      }
                    }
                    else
                      continue;
                  }
                }
              }
            }
          }
label_47:
          // ISSUE: reference to a compiler-generated field
          if (!flag || this.m_PublicTransportData.HasComponent(nativeArray4[index1].m_Vehicle))
          {
            Controller controller;
            if (CollectionUtils.TryGet<Controller>(buffer5, index1, out controller) && controller.m_Controller != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PathSourceQueue.Enqueue(controller.m_Controller);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PathSourceQueue.Enqueue(nativeArray1[index1]);
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
    private struct UpdateLivePathsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_LivePathChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public BufferTypeHandle<RouteSegment> m_RouteSegmentType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<PathSource> m_PathSourceData;
      [ReadOnly]
      public ComponentLookup<Human> m_HumanData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Watercraft> m_WatercraftData;
      [ReadOnly]
      public ComponentLookup<Aircraft> m_AircraftData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public int m_UpdateFrameIndex;
      [ReadOnly]
      public int m_SourceCountLimit;
      [ReadOnly]
      public RouteConfigurationData m_RouteConfigurationData;
      [NativeDisableContainerSafetyRestriction]
      public NativeQueue<Entity> m_PathSourceQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        NativeHashMap<Entity, TrafficRoutesSystem.LivePathEntityData> livePathEntities = new NativeHashMap<Entity, TrafficRoutesSystem.LivePathEntityData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeHashMap<Entity, bool> pathSourceFound = new NativeHashMap<Entity, bool>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_LivePathChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk livePathChunk = this.m_LivePathChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = livePathChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray2 = livePathChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteSegment> bufferAccessor = livePathChunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            DynamicBuffer<RouteSegment> dynamicBuffer = bufferAccessor[index2];
            // ISSUE: object of a compiler-generated type is created
            livePathEntities[nativeArray2[index2].m_Prefab] = new TrafficRoutesSystem.LivePathEntityData()
            {
              m_Entity = nativeArray1[index2],
              m_SegmentCount = dynamicBuffer.Length
            };
            for (int index3 = 0; index3 < dynamicBuffer.Length; ++index3)
            {
              Entity segment = dynamicBuffer[index3].m_Segment;
              // ISSUE: reference to a compiler-generated field
              pathSourceFound[this.m_PathSourceData[segment].m_Entity] = false;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_SelectedEntity;
        CurrentTransport componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentTransportData.TryGetComponent(entity1, out componentData1))
          entity1 = componentData1.m_CurrentTransport;
        Controller componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControllerData.TryGetComponent(entity1, out componentData2) && componentData2.m_Controller != Entity.Null)
          entity1 = componentData2.m_Controller;
        // ISSUE: reference to a compiler-generated method
        this.AddLivePath(entity1, livePathEntities, pathSourceFound);
        CurrentVehicle componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentVehicleData.TryGetComponent(entity1, out componentData3))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.TryGetComponent(componentData3.m_Vehicle, out componentData2) && componentData2.m_Controller != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddLivePath(componentData2.m_Controller, livePathEntities, pathSourceFound);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddLivePath(componentData3.m_Vehicle, livePathEntities, pathSourceFound);
          }
        }
        DynamicBuffer<LayoutElement> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LayoutElements.TryGetBuffer(entity1, out bufferData1) && bufferData1.Length != 0)
        {
          for (int index4 = 0; index4 < bufferData1.Length; ++index4)
          {
            DynamicBuffer<Passenger> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Passengers.TryGetBuffer(bufferData1[index4].m_Vehicle, out bufferData2))
            {
              for (int index5 = 0; index5 < bufferData2.Length; ++index5)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddLivePath(bufferData2[index5].m_Passenger, livePathEntities, pathSourceFound);
              }
            }
          }
        }
        else
        {
          DynamicBuffer<Passenger> bufferData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Passengers.TryGetBuffer(entity1, out bufferData3))
          {
            for (int index = 0; index < bufferData3.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddLivePath(bufferData3[index].m_Passenger, livePathEntities, pathSourceFound);
            }
          }
        }
        DynamicBuffer<HouseholdCitizen> bufferData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdCitizens.TryGetBuffer(entity1, out bufferData4))
        {
          for (int index = 0; index < bufferData4.Length; ++index)
          {
            Entity entity2 = bufferData4[index].m_Citizen;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentTransportData.TryGetComponent(entity2, out componentData1))
              entity2 = componentData1.m_CurrentTransport;
            // ISSUE: reference to a compiler-generated method
            this.AddLivePath(entity2, livePathEntities, pathSourceFound);
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentVehicleData.TryGetComponent(entity2, out componentData3))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ControllerData.TryGetComponent(componentData3.m_Vehicle, out componentData2) && componentData2.m_Controller != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddLivePath(componentData2.m_Controller, livePathEntities, pathSourceFound);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.AddLivePath(componentData3.m_Vehicle, livePathEntities, pathSourceFound);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathSourceQueue.IsCreated)
        {
          Entity sourceEntity;
          // ISSUE: reference to a compiler-generated field
          while (this.m_PathSourceQueue.TryDequeue(out sourceEntity))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddLivePath(sourceEntity, livePathEntities, pathSourceFound);
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index6 = 0; index6 < this.m_LivePathChunks.Length; ++index6)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk livePathChunk = this.m_LivePathChunks[index6];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray3 = livePathChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = livePathChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<RouteSegment> bufferAccessor = livePathChunk.GetBufferAccessor<RouteSegment>(ref this.m_RouteSegmentType);
          for (int index7 = 0; index7 < bufferAccessor.Length; ++index7)
          {
            Entity e = nativeArray3[index7];
            DynamicBuffer<RouteSegment> dynamicBuffer = bufferAccessor[index7];
            int index8 = 0;
            for (int index9 = 0; index9 < dynamicBuffer.Length; ++index9)
            {
              RouteSegment routeSegment = dynamicBuffer[index9];
              // ISSUE: reference to a compiler-generated field
              PathSource pathSource = this.m_PathSourceData[routeSegment.m_Segment];
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              if (!pathSourceFound[pathSource.m_Entity] && this.GetUpdateFrameIndex(pathSource.m_Entity) == this.m_UpdateFrameIndex)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(routeSegment.m_Segment);
              }
              else
                dynamicBuffer[index8++] = routeSegment;
            }
            // ISSUE: reference to a compiler-generated field
            bool hasNewSegments = livePathEntities[nativeArray4[index7].m_Prefab].m_HasNewSegments;
            if (index8 < dynamicBuffer.Length)
            {
              dynamicBuffer.RemoveRange(index8, dynamicBuffer.Length - index8);
              if (index8 == 0 && !hasNewSegments)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(e);
              }
            }
            if (hasNewSegments)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(e);
            }
          }
        }
        livePathEntities.Dispose();
        pathSourceFound.Dispose();
      }

      private int GetUpdateFrameIndex(Entity sourceEntity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdateFrameIndex == -1 || !this.m_EntityLookup.Exists(sourceEntity))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_UpdateFrameIndex;
        }
        // ISSUE: reference to a compiler-generated field
        EntityStorageInfo entityStorageInfo = this.m_EntityLookup[sourceEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !entityStorageInfo.Chunk.Has<UpdateFrame>(this.m_UpdateFrameType) ? this.m_UpdateFrameIndex : (int) entityStorageInfo.Chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
      }

      private void AddLivePath(
        Entity sourceEntity,
        NativeHashMap<Entity, TrafficRoutesSystem.LivePathEntityData> livePathEntities,
        NativeHashMap<Entity, bool> pathSourceFound)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PathElements.HasBuffer(sourceEntity))
          return;
        bool flag;
        if (pathSourceFound.TryGetValue(sourceEntity, out flag))
        {
          if (flag)
            return;
          pathSourceFound[sourceEntity] = true;
        }
        else
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
          Entity entity1 = !this.m_HumanData.HasComponent(sourceEntity) ? (!this.m_WatercraftData.HasComponent(sourceEntity) ? (!this.m_AircraftData.HasComponent(sourceEntity) ? (!this.m_TrainData.HasComponent(sourceEntity) ? this.m_RouteConfigurationData.m_CarPathVisualization : this.m_RouteConfigurationData.m_TrainPathVisualization) : this.m_RouteConfigurationData.m_AircraftPathVisualization) : this.m_RouteConfigurationData.m_WatercraftPathVisualization) : this.m_RouteConfigurationData.m_HumanPathVisualization;
          // ISSUE: reference to a compiler-generated field
          RouteData routeData = this.m_PrefabRouteData[entity1];
          // ISSUE: variable of a compiler-generated type
          TrafficRoutesSystem.LivePathEntityData livePathEntityData;
          if (!livePathEntities.TryGetValue(entity1, out livePathEntityData))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            livePathEntityData.m_Entity = this.m_CommandBuffer.CreateEntity(routeData.m_RouteArchetype);
            // ISSUE: reference to a compiler-generated field
            livePathEntityData.m_SegmentCount = 1;
            // ISSUE: reference to a compiler-generated field
            livePathEntityData.m_HasNewSegments = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PrefabRef>(livePathEntityData.m_Entity, new PrefabRef(entity1));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Game.Routes.Color>(livePathEntityData.m_Entity, new Game.Routes.Color(routeData.m_Color));
            livePathEntities[entity1] = livePathEntityData;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (livePathEntityData.m_SegmentCount++ >= this.m_SourceCountLimit)
              return;
            // ISSUE: reference to a compiler-generated field
            livePathEntityData.m_HasNewSegments = true;
            livePathEntities[entity1] = livePathEntityData;
          }
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_CommandBuffer.CreateEntity(routeData.m_SegmentArchetype);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, new PrefabRef(entity1));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Owner>(entity2, new Owner(livePathEntityData.m_Entity));
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PathSource>(entity2, new PathSource()
          {
            m_Entity = sourceEntity
          });
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AppendToBuffer<RouteSegment>(livePathEntityData.m_Entity, new RouteSegment(entity2));
          pathSourceFound.Add(sourceEntity, true);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AggregateElement> __Game_Net_AggregateElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferLookup;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<PathElement> __Game_Pathfind_PathElement_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<WatercraftNavigationLane> __Game_Vehicles_WatercraftNavigationLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<AircraftNavigationLane> __Game_Vehicles_AircraftNavigationLane_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<TrainNavigationLane> __Game_Vehicles_TrainNavigationLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public BufferTypeHandle<RouteSegment> __Game_Routes_RouteSegment_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<PathSource> __Game_Routes_PathSource_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Human> __Game_Creatures_Human_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Watercraft> __Game_Vehicles_Watercraft_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aircraft> __Game_Vehicles_Aircraft_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteData> __Game_Prefabs_RouteData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_AggregateElement_RO_BufferLookup = state.GetBufferLookup<AggregateElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferLookup = state.GetBufferLookup<ConnectedRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigationLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<WatercraftNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigationLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<AircraftNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainNavigationLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<TrainNavigationLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RW_BufferTypeHandle = state.GetBufferTypeHandle<RouteSegment>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathSource_RO_ComponentLookup = state.GetComponentLookup<PathSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentLookup = state.GetComponentLookup<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Watercraft_RO_ComponentLookup = state.GetComponentLookup<Watercraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Aircraft_RO_ComponentLookup = state.GetComponentLookup<Aircraft>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RO_ComponentLookup = state.GetComponentLookup<RouteData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
      }
    }
  }
}
