// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportWatercraftAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Creatures;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TransportWatercraftAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_TransportVehicleRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private TransportBoardingHelpers.BoardingLookupData m_BoardingLookupData;
    private TransportWatercraftAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 8;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingLookupData = new TransportBoardingHelpers.BoardingLookupData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadWrite<WatercraftCurrentLane>(),
          ComponentType.ReadOnly<Owner>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadWrite<PathOwner>(),
          ComponentType.ReadWrite<Target>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadWrite<Game.Vehicles.CargoTransport>(),
          ComponentType.ReadWrite<Game.Vehicles.PublicTransport>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<TripSource>(),
          ComponentType.ReadOnly<OutOfControl>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<TransportVehicleRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      TransportBoardingHelpers.BoardingData boardingData = new TransportBoardingHelpers.BoardingData(Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingLookupData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Watercraft_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new TransportWatercraftAISystem.TransportWatercraftTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_CurrentRouteType = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferTypeHandle,
        m_CargoTransportType = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle,
        m_PublicTransportType = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle,
        m_WatercraftType = this.__TypeHandle.__Game_Vehicles_Watercraft_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_OdometerType = this.__TypeHandle.__Game_Vehicles_Odometer_RW_ComponentTypeHandle,
        m_WatercraftNavigationLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TransportVehicleRequestData = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup,
        m_PrefabWatercraftData = this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PublicTransportVehicleData = this.__TypeHandle.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup,
        m_CargoTransportVehicleData = this.__TypeHandle.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup,
        m_WaypointData = this.__TypeHandle.__Game_Routes_Waypoint_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_BoardingVehicleData = this.__TypeHandle.__Game_Routes_BoardingVehicle_RO_ComponentLookup,
        m_RouteLaneData = this.__TypeHandle.__Game_Routes_RouteLane_RO_ComponentLookup,
        m_RouteColorData = this.__TypeHandle.__Game_Routes_Color_RO_ComponentLookup,
        m_StorageCompanyData = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_TransportStationData = this.__TypeHandle.__Game_Buildings_TransportStation_RO_ComponentLookup,
        m_LaneData = this.__TypeHandle.__Game_Net_Lane_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_LoadingResources = this.__TypeHandle.__Game_Vehicles_LoadingResources_RW_BufferLookup,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_TransportVehicleRequestArchetype = this.m_TransportVehicleRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_BoardingData = boardingData.ToConcurrent()
      }.ScheduleParallel<TransportWatercraftAISystem.TransportWatercraftTickJob>(this.m_VehicleQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = boardingData.ScheduleBoarding((SystemBase) this, this.m_CityStatisticsSystem, this.m_BoardingLookupData, this.m_SimulationSystem.frameIndex, jobHandle);
      boardingData.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
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
    public TransportWatercraftAISystem()
    {
    }

    [BurstCompile]
    private struct TransportWatercraftTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> m_CurrentRouteType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
      public ComponentTypeHandle<Watercraft> m_WatercraftType;
      public ComponentTypeHandle<WatercraftCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<Odometer> m_OdometerType;
      public BufferTypeHandle<WatercraftNavigationLane> m_WatercraftNavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<WatercraftData> m_PrefabWatercraftData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> m_PublicTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_CargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;
      [ReadOnly]
      public ComponentLookup<RouteLane> m_RouteLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanyData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> m_TransportStationData;
      [ReadOnly]
      public ComponentLookup<Lane> m_LaneData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<LoadingResources> m_LoadingResources;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public EntityArchetype m_TransportVehicleRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public TransportBoardingHelpers.BoardingData.Concurrent m_BoardingData;

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
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentRoute> nativeArray4 = chunk.GetNativeArray<CurrentRoute>(ref this.m_CurrentRouteType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WatercraftCurrentLane> nativeArray5 = chunk.GetNativeArray<WatercraftCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.CargoTransport> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.CargoTransport>(ref this.m_CargoTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.PublicTransport> nativeArray7 = chunk.GetNativeArray<Game.Vehicles.PublicTransport>(ref this.m_PublicTransportType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Watercraft> nativeArray8 = chunk.GetNativeArray<Watercraft>(ref this.m_WatercraftType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray9 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Odometer> nativeArray11 = chunk.GetNativeArray<Odometer>(ref this.m_OdometerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<WatercraftNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<WatercraftNavigationLane>(ref this.m_WatercraftNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor2 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          Watercraft watercraft = nativeArray8[index];
          WatercraftCurrentLane currentLane = nativeArray5[index];
          PathOwner pathOwner = nativeArray10[index];
          Target target = nativeArray9[index];
          Odometer odometer = nativeArray11[index];
          DynamicBuffer<WatercraftNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          CurrentRoute currentRoute = new CurrentRoute();
          if (nativeArray4.Length != 0)
            currentRoute = nativeArray4[index];
          Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
          if (nativeArray6.Length != 0)
            cargoTransport = nativeArray6[index];
          Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
          DynamicBuffer<Passenger> passengers = new DynamicBuffer<Passenger>();
          if (nativeArray7.Length != 0)
          {
            publicTransport = nativeArray7[index];
            passengers = bufferAccessor2[index];
          }
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, prefabRef, currentRoute, navigationLanes, passengers, serviceDispatches, ref cargoTransport, ref publicTransport, ref watercraft, ref currentLane, ref pathOwner, ref target, ref odometer);
          nativeArray8[index] = watercraft;
          nativeArray5[index] = currentLane;
          nativeArray10[index] = pathOwner;
          nativeArray9[index] = target;
          nativeArray11[index] = odometer;
          if (nativeArray6.Length != 0)
            nativeArray6[index] = cargoTransport;
          if (nativeArray7.Length != 0)
            nativeArray7[index] = publicTransport;
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        PrefabRef prefabRef,
        CurrentRoute currentRoute,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Watercraft watercraft,
        ref WatercraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        ref Odometer odometer)
      {
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(vehicleEntity, ref cargoTransport, ref publicTransport, ref watercraft, ref currentLane, ref pathOwner);
          DynamicBuffer<LoadingResources> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (((publicTransport.m_State & PublicTransportFlags.DummyTraffic) != (PublicTransportFlags) 0 || (cargoTransport.m_State & CargoTransportFlags.DummyTraffic) != (CargoTransportFlags) 0) && this.m_LoadingResources.TryGetBuffer(vehicleEntity, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckDummyResources(jobIndex, vehicleEntity, prefabRef, bufferData);
          }
        }
        int num = (cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0 ? 0 : ((publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0 ? 1 : 0);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PublicTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          PublicTransportVehicleData transportVehicleData = this.m_PublicTransportVehicleData[prefabRef.m_Prefab];
          if ((double) odometer.m_Distance >= (double) transportVehicleData.m_MaintenanceRange && (double) transportVehicleData.m_MaintenanceRange > 0.10000000149011612 && (publicTransport.m_State & PublicTransportFlags.Refueling) == (PublicTransportFlags) 0)
            publicTransport.m_State |= PublicTransportFlags.RequiresMaintenance;
        }
        bool isCargoVehicle = false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[prefabRef.m_Prefab];
          if ((double) odometer.m_Distance >= (double) transportVehicleData.m_MaintenanceRange && (double) transportVehicleData.m_MaintenanceRange > 0.10000000149011612 && (cargoTransport.m_State & CargoTransportFlags.Refueling) == (CargoTransportFlags) 0)
            cargoTransport.m_State |= CargoTransportFlags.RequiresMaintenance;
          isCargoVehicle = true;
        }
        watercraft.m_Flags |= WatercraftFlags.DeckLights;
        if (num != 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, ref cargoTransport, ref publicTransport);
          if (serviceDispatches.Length == 0 && (cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.DummyTraffic | CargoTransportFlags.Disabled)) == (CargoTransportFlags) 0 && (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.DummyTraffic | PublicTransportFlags.Disabled)) == (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref publicTransport, ref cargoTransport);
          }
        }
        else
        {
          serviceDispatches.Clear();
          cargoTransport.m_RequestCount = 0;
          publicTransport.m_RequestCount = 0;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.StopBoarding(jobIndex, vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, true);
          }
          if (VehicleUtils.IsStuck(pathOwner) || (cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          {
            if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (this.StopBoarding(jobIndex, vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, false) && !this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref watercraft, ref currentLane, ref pathOwner, ref target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
                return;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if ((!passengers.IsCreated || passengers.Length <= 0 || !this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, isCargoVehicle)) && !this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref watercraft, ref currentLane, ref pathOwner, ref target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
                return;
              }
            }
          }
          else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.StopBoarding(jobIndex, vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, false))
            {
              if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) || !this.m_WaypointData.HasComponent(target.m_Target))
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.StartBoarding(jobIndex, vehicleEntity, currentRoute, prefabRef, ref cargoTransport, ref publicTransport, ref target, isCargoVehicle))
              {
                if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.ReturnToDepot(jobIndex, vehicleEntity, currentRoute, owner, serviceDispatches, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
                }
              }
            }
          }
        }
        else if ((cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.StopBoarding(jobIndex, vehicleEntity, currentRoute, passengers, ref cargoTransport, ref publicTransport, ref target, ref odometer, isCargoVehicle, true);
        }
        Entity skipWaypoint = Entity.Null;
        if ((cargoTransport.m_State & CargoTransportFlags.Boarding) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0)
        {
          if ((cargoTransport.m_State & CargoTransportFlags.Returning) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0)
          {
            if (!passengers.IsCreated || passengers.Length == 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.SelectNextDispatch(jobIndex, vehicleEntity, currentRoute, navigationLanes, serviceDispatches, ref cargoTransport, ref publicTransport, ref watercraft, ref currentLane, ref pathOwner, ref target);
            }
          }
          else if ((cargoTransport.m_State & CargoTransportFlags.Arriving) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Arriving) == (PublicTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckNavigationLanes(currentRoute, navigationLanes, ref cargoTransport, ref publicTransport, ref currentLane, ref pathOwner, ref target, out skipWaypoint);
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.FindPathIfNeeded(vehicleEntity, prefabRef, skipWaypoint, ref currentLane, ref cargoTransport, ref publicTransport, ref pathOwner, ref target);
      }

      private void FindPathIfNeeded(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        Entity skipWaypoint,
        ref WatercraftCurrentLane currentLane,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if (!VehicleUtils.RequireNewPath(pathOwner))
          return;
        // ISSUE: reference to a compiler-generated field
        WatercraftData watercraftData = this.m_PrefabWatercraftData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) watercraftData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road;
        setupQueueTarget.m_RoadTypes = RoadTypes.Watercraft;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road;
        setupQueueTarget.m_RoadTypes = RoadTypes.Watercraft;
        setupQueueTarget.m_Entity = target.m_Target;
        SetupQueueTarget destination = setupQueueTarget;
        if (skipWaypoint != Entity.Null)
        {
          origin.m_Entity = skipWaypoint;
          pathOwner.m_State |= PathFlags.Append;
        }
        else
          pathOwner.m_State &= ~PathFlags.Append;
        if ((cargoTransport.m_State & (CargoTransportFlags.EnRoute | CargoTransportFlags.RouteSource)) == (CargoTransportFlags.EnRoute | CargoTransportFlags.RouteSource) || (publicTransport.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.RouteSource)) == (PublicTransportFlags.EnRoute | PublicTransportFlags.RouteSource))
          parameters.m_PathfindFlags = PathfindFlags.Stable | PathfindFlags.IgnoreFlow;
        else if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.EnRoute) == (PublicTransportFlags) 0)
        {
          cargoTransport.m_State &= ~CargoTransportFlags.RouteSource;
          publicTransport.m_State &= ~PublicTransportFlags.RouteSource;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckNavigationLanes(
        CurrentRoute currentRoute,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref WatercraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target,
        out Entity skipWaypoint)
      {
        skipWaypoint = Entity.Null;
        if (navigationLanes.Length == 0 || navigationLanes.Length == 8)
          return;
        WatercraftNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
        if ((navigationLane.m_Flags & WatercraftLaneFlags.EndOfPath) == (WatercraftLaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaypointData.HasComponent(target.m_Target) && this.m_RouteWaypoints.HasBuffer(currentRoute.m_Route) && (!this.m_ConnectedData.HasComponent(target.m_Target) || !this.m_BoardingVehicleData.HasComponent(this.m_ConnectedData[target.m_Target].m_Connected)))
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Obsolete)) != (PathFlags) 0)
            return;
          skipWaypoint = target.m_Target;
          // ISSUE: reference to a compiler-generated method
          this.SetNextWaypointTarget(currentRoute, ref pathOwner, ref target);
          if ((navigationLane.m_Flags & WatercraftLaneFlags.GroupTarget) != (WatercraftLaneFlags) 0)
          {
            navigationLanes.RemoveAt(navigationLanes.Length - 1);
          }
          else
          {
            navigationLane.m_Flags &= ~WatercraftLaneFlags.EndOfPath;
            navigationLanes[navigationLanes.Length - 1] = navigationLane;
          }
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
        }
        else
        {
          cargoTransport.m_State |= CargoTransportFlags.Arriving;
          publicTransport.m_State |= PublicTransportFlags.Arriving;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_RouteLaneData.HasComponent(target.m_Target))
            return;
          // ISSUE: reference to a compiler-generated field
          RouteLane routeLane = this.m_RouteLaneData[target.m_Target];
          if (routeLane.m_StartLane != routeLane.m_EndLane)
          {
            navigationLane.m_CurvePosition.y = 1f;
            WatercraftNavigationLane elem = new WatercraftNavigationLane();
            elem.m_Lane = navigationLane.m_Lane;
            // ISSUE: reference to a compiler-generated method
            if (this.FindNextLane(ref elem.m_Lane))
            {
              navigationLane.m_Flags &= ~WatercraftLaneFlags.EndOfPath;
              navigationLanes[navigationLanes.Length - 1] = navigationLane;
              elem.m_Flags |= WatercraftLaneFlags.EndOfPath | WatercraftLaneFlags.FixedLane;
              elem.m_CurvePosition = new float2(0.0f, routeLane.m_EndCurvePos);
              navigationLanes.Add(elem);
            }
            else
              navigationLanes[navigationLanes.Length - 1] = navigationLane;
          }
          else
          {
            navigationLane.m_CurvePosition.y = routeLane.m_EndCurvePos;
            navigationLanes[navigationLanes.Length - 1] = navigationLane;
          }
        }
      }

      private bool FindNextLane(ref Entity lane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_OwnerData.HasComponent(lane) || !this.m_LaneData.HasComponent(lane))
          return false;
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[lane];
        // ISSUE: reference to a compiler-generated field
        Lane lane1 = this.m_LaneData[lane];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.HasBuffer(owner.m_Owner))
          return false;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner.m_Owner];
        for (int index = 0; index < subLane1.Length; ++index)
        {
          Entity subLane2 = subLane1[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          Lane lane2 = this.m_LaneData[subLane2];
          if (lane1.m_EndNode.Equals(lane2.m_StartNode))
          {
            lane = subLane2;
            return true;
          }
        }
        return false;
      }

      private void ResetPath(
        Entity vehicleEntity,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Watercraft watercraft,
        ref WatercraftCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
        publicTransport.m_State &= ~PublicTransportFlags.Arriving;
        if ((pathOwner.m_State & PathFlags.Append) == (PathFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PathUtils.ResetPath(ref currentLane, pathElement, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
        }
        if ((cargoTransport.m_State & (CargoTransportFlags.Returning | CargoTransportFlags.DummyTraffic)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.Returning | PublicTransportFlags.DummyTraffic)) != (PublicTransportFlags) 0)
          watercraft.m_Flags &= ~WatercraftFlags.StayOnWaterway;
        else
          watercraft.m_Flags |= WatercraftFlags.StayOnWaterway;
      }

      private void CheckDummyResources(
        int jobIndex,
        Entity vehicleEntity,
        PrefabRef prefabRef,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        if (loadingResources.Length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CargoTransportVehicleData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          CargoTransportVehicleData transportVehicleData = this.m_CargoTransportVehicleData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Resources> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Resources>(jobIndex, vehicleEntity);
          for (int index = 0; index < loadingResources.Length && dynamicBuffer.Length < transportVehicleData.m_MaxResourceCount; ++index)
          {
            LoadingResources loadingResource = loadingResources[index];
            int num = math.min(loadingResource.m_Amount, transportVehicleData.m_CargoCapacity);
            loadingResource.m_Amount -= num;
            transportVehicleData.m_CargoCapacity -= num;
            if (num > 0)
              dynamicBuffer.Add(new Resources()
              {
                m_Resource = loadingResource.m_Resource,
                m_Amount = num
              });
          }
        }
        loadingResources.Clear();
        // ISSUE: reference to a compiler-generated method
        this.QuantityUpdated(jobIndex, vehicleEntity);
      }

      private void SetNextWaypointTarget(
        CurrentRoute currentRoute,
        ref PathOwner pathOwnerData,
        ref Target targetData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[currentRoute.m_Route];
        // ISSUE: reference to a compiler-generated field
        int a = this.m_WaypointData[targetData.m_Target].m_Index + 1;
        int index = math.select(a, 0, a >= routeWaypoint.Length);
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, routeWaypoint[index].m_Waypoint);
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport)
      {
        if (serviceDispatches.Length > 1)
          serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
        cargoTransport.m_RequestCount = math.min(1, cargoTransport.m_RequestCount);
        publicTransport.m_RequestCount = math.min(1, publicTransport.m_RequestCount);
        int index1 = cargoTransport.m_RequestCount + publicTransport.m_RequestCount;
        if (serviceDispatches.Length <= index1)
          return;
        float num = -1f;
        Entity request1 = Entity.Null;
        for (int index2 = index1; index2 < serviceDispatches.Length; ++index2)
        {
          Entity request2 = serviceDispatches[index2].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request2))
          {
            // ISSUE: reference to a compiler-generated field
            TransportVehicleRequest transportVehicleRequest = this.m_TransportVehicleRequestData[request2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(transportVehicleRequest.m_Route) && (double) transportVehicleRequest.m_Priority > (double) num)
            {
              num = transportVehicleRequest.m_Priority;
              request1 = request2;
            }
          }
        }
        if (request1 != Entity.Null)
        {
          serviceDispatches[index1++] = new ServiceDispatch(request1);
          ++publicTransport.m_RequestCount;
          ++cargoTransport.m_RequestCount;
        }
        if (serviceDispatches.Length <= index1)
          return;
        serviceDispatches.RemoveRange(index1, serviceDispatches.Length - index1);
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Game.Vehicles.CargoTransport cargoTransport)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportVehicleRequestData.HasComponent(publicTransport.m_TargetRequest) || this.m_TransportVehicleRequestData.HasComponent(cargoTransport.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(256U, 16U) - 1) != 8)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_TransportVehicleRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<TransportVehicleRequest>(jobIndex, entity1, new TransportVehicleRequest(entity, 1f));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(8U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<WatercraftNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Watercraft watercraft,
        ref WatercraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        if ((cargoTransport.m_State & CargoTransportFlags.Returning) == (CargoTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Returning) == (PublicTransportFlags) 0 && cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          cargoTransport.m_RequestCount = math.max(0, cargoTransport.m_RequestCount - 1);
          publicTransport.m_RequestCount = math.max(0, publicTransport.m_RequestCount - 1);
        }
        if ((cargoTransport.m_State & (CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.Disabled)) != (CargoTransportFlags) 0 || (publicTransport.m_State & (PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.Disabled)) != (PublicTransportFlags) 0)
        {
          cargoTransport.m_RequestCount = 0;
          publicTransport.m_RequestCount = 0;
          serviceDispatches.Clear();
          return false;
        }
        for (; cargoTransport.m_RequestCount + publicTransport.m_RequestCount > 0 && serviceDispatches.Length > 0; publicTransport.m_RequestCount = math.max(0, publicTransport.m_RequestCount - 1))
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity route = Entity.Null;
          Entity destination = Entity.Null;
          WatercraftFlags flags = watercraft.m_Flags;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            route = this.m_TransportVehicleRequestData[request].m_Route;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathInformationData.HasComponent(request))
            {
              // ISSUE: reference to a compiler-generated field
              destination = this.m_PathInformationData[request].m_Destination;
            }
            flags |= WatercraftFlags.StayOnWaterway;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(destination))
          {
            serviceDispatches.RemoveAt(0);
            cargoTransport.m_RequestCount = math.max(0, cargoTransport.m_RequestCount - 1);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(request))
            {
              serviceDispatches.Clear();
              cargoTransport.m_RequestCount = 0;
              publicTransport.m_RequestCount = 0;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(route))
              {
                if (currentRoute.m_Route != route)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<CurrentRoute>(jobIndex, vehicleEntity, new CurrentRoute(route));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AppendToBuffer<RouteVehicle>(jobIndex, route, new RouteVehicle(vehicleEntity));
                  Game.Routes.Color componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_RouteColorData.TryGetComponent(route, out componentData))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Game.Routes.Color>(jobIndex, vehicleEntity, componentData);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, vehicleEntity);
                  }
                }
                cargoTransport.m_State |= CargoTransportFlags.EnRoute;
                publicTransport.m_State |= PublicTransportFlags.EnRoute;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(request, vehicleEntity, true));
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(request, vehicleEntity, false, true));
            }
            cargoTransport.m_State &= ~CargoTransportFlags.Returning;
            publicTransport.m_State &= ~PublicTransportFlags.Returning;
            watercraft.m_Flags = flags;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(publicTransport.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(publicTransport.m_TargetRequest, Entity.Null, true));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(cargoTransport.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity, new HandleRequest(cargoTransport.m_TargetRequest, Entity.Null, true));
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathElements.HasBuffer(request))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[request];
              if (pathElement1.Length != 0)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[vehicleEntity];
                PathUtils.TrimPath(pathElement2, ref pathOwner);
                // ISSUE: reference to a compiler-generated field
                float num = math.max(cargoTransport.m_PathElementTime, publicTransport.m_PathElementTime) * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes))
                {
                  cargoTransport.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  publicTransport.m_PathElementTime = cargoTransport.m_PathElementTime;
                  target.m_Target = destination;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  cargoTransport.m_State &= ~CargoTransportFlags.Arriving;
                  publicTransport.m_State &= ~PublicTransportFlags.Arriving;
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, destination);
            return true;
          }
        }
        return false;
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        Owner owner,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref PathOwner pathOwner,
        ref Target target)
      {
        serviceDispatches.Clear();
        cargoTransport.m_RequestCount = 0;
        cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.Refueling | CargoTransportFlags.AbandonRoute);
        cargoTransport.m_State |= CargoTransportFlags.Returning;
        publicTransport.m_RequestCount = 0;
        publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.Refueling | PublicTransportFlags.AbandonRoute);
        publicTransport.m_State |= PublicTransportFlags.Returning;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        VehicleUtils.SetTarget(ref pathOwner, ref target, owner.m_Owner);
      }

      private bool StartBoarding(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        PrefabRef prefabRef,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        bool isCargoVehicle)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          Connected connected = this.m_ConnectedData[target.m_Target];
          // ISSUE: reference to a compiler-generated field
          if (this.m_BoardingVehicleData.HasComponent(connected.m_Connected))
          {
            // ISSUE: reference to a compiler-generated method
            Entity transportStationFromStop = this.GetTransportStationFromStop(connected.m_Connected);
            Entity nextStorageCompany = Entity.Null;
            bool refuel = false;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportStationData.HasComponent(transportStationFromStop))
            {
              // ISSUE: reference to a compiler-generated field
              WatercraftData watercraftData = this.m_PrefabWatercraftData[prefabRef.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              refuel = (this.m_TransportStationData[transportStationFromStop].m_WatercraftRefuelTypes & watercraftData.m_EnergyType) != 0;
            }
            if (!refuel && ((cargoTransport.m_State & CargoTransportFlags.RequiresMaintenance) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.RequiresMaintenance) != (PublicTransportFlags) 0) || (cargoTransport.m_State & CargoTransportFlags.AbandonRoute) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.AbandonRoute) != (PublicTransportFlags) 0)
            {
              cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.AbandonRoute);
              publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.AbandonRoute);
              if (currentRoute.m_Route != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
              }
            }
            else
            {
              cargoTransport.m_State &= ~CargoTransportFlags.RequiresMaintenance;
              publicTransport.m_State &= ~PublicTransportFlags.RequiresMaintenance;
              cargoTransport.m_State |= CargoTransportFlags.EnRoute;
              publicTransport.m_State |= PublicTransportFlags.EnRoute;
              if (isCargoVehicle)
              {
                // ISSUE: reference to a compiler-generated method
                nextStorageCompany = this.GetNextStorageCompany(currentRoute.m_Route, target.m_Target);
              }
            }
            cargoTransport.m_State |= CargoTransportFlags.RouteSource;
            publicTransport.m_State |= PublicTransportFlags.RouteSource;
            Entity storageCompanyFromStop = Entity.Null;
            if (isCargoVehicle)
            {
              // ISSUE: reference to a compiler-generated method
              storageCompanyFromStop = this.GetStorageCompanyFromStop(connected.m_Connected);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_BoardingData.BeginBoarding(vehicleEntity, currentRoute.m_Route, connected.m_Connected, target.m_Target, storageCompanyFromStop, nextStorageCompany, refuel);
            return true;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaypointData.HasComponent(target.m_Target))
        {
          cargoTransport.m_State |= CargoTransportFlags.RouteSource;
          publicTransport.m_State |= PublicTransportFlags.RouteSource;
          return false;
        }
        cargoTransport.m_State &= ~(CargoTransportFlags.EnRoute | CargoTransportFlags.AbandonRoute);
        publicTransport.m_State &= ~(PublicTransportFlags.EnRoute | PublicTransportFlags.AbandonRoute);
        if (currentRoute.m_Route != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<CurrentRoute>(jobIndex, vehicleEntity);
        }
        return false;
      }

      private bool StopBoarding(
        int jobIndex,
        Entity vehicleEntity,
        CurrentRoute currentRoute,
        DynamicBuffer<Passenger> passengers,
        ref Game.Vehicles.CargoTransport cargoTransport,
        ref Game.Vehicles.PublicTransport publicTransport,
        ref Target target,
        ref Odometer odometer,
        bool isCargoVehicle,
        bool forcedStop)
      {
        bool flag = false;
        Connected componentData1;
        BoardingVehicle componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedData.TryGetComponent(target.m_Target, out componentData1) && this.m_BoardingVehicleData.TryGetComponent(componentData1.m_Connected, out componentData2))
          flag = componentData2.m_Vehicle == vehicleEntity;
        if (!forcedStop)
        {
          publicTransport.m_MaxBoardingDistance = math.select(publicTransport.m_MinWaitingDistance + 1f, float.MaxValue, (double) publicTransport.m_MinWaitingDistance == 3.4028234663852886E+38 || (double) publicTransport.m_MinWaitingDistance == 0.0);
          publicTransport.m_MinWaitingDistance = float.MaxValue;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (flag && (this.m_SimulationFrameIndex < cargoTransport.m_DepartureFrame || this.m_SimulationFrameIndex < publicTransport.m_DepartureFrame || (double) publicTransport.m_MaxBoardingDistance != 3.4028234663852886E+38))
            return false;
          if (passengers.IsCreated)
          {
            for (int index = 0; index < passengers.Length; ++index)
            {
              Entity passenger = passengers[index].m_Passenger;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentVehicleData.HasComponent(passenger) && (this.m_CurrentVehicleData[passenger].m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
                return false;
            }
          }
        }
        if ((cargoTransport.m_State & CargoTransportFlags.Refueling) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.Refueling) != (PublicTransportFlags) 0)
          odometer.m_Distance = 0.0f;
        if (isCargoVehicle)
        {
          // ISSUE: reference to a compiler-generated method
          this.QuantityUpdated(jobIndex, vehicleEntity);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.PassengersUpdated(jobIndex, vehicleEntity);
        }
        if (flag)
        {
          Entity storageCompanyFromStop = Entity.Null;
          Entity nextStorageCompany = Entity.Null;
          if (!forcedStop && (cargoTransport.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            storageCompanyFromStop = this.GetStorageCompanyFromStop(componentData1.m_Connected);
            if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              nextStorageCompany = this.GetNextStorageCompany(currentRoute.m_Route, target.m_Target);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_BoardingData.EndBoarding(vehicleEntity, currentRoute.m_Route, componentData1.m_Connected, target.m_Target, storageCompanyFromStop, nextStorageCompany);
          return true;
        }
        cargoTransport.m_State &= ~(CargoTransportFlags.Boarding | CargoTransportFlags.Refueling);
        publicTransport.m_State &= ~(PublicTransportFlags.Boarding | PublicTransportFlags.Refueling);
        return true;
      }

      private void QuantityUpdated(int jobIndex, Entity vehicleEntity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, vehicleEntity, new Updated());
      }

      private void PassengersUpdated(int jobIndex, Entity vehicleEntity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<BatchesUpdated>(jobIndex, vehicleEntity, new BatchesUpdated());
      }

      private Entity GetTransportStationFromStop(Entity stop)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; !this.m_TransportStationData.HasComponent(stop); stop = this.m_OwnerData[stop].m_Owner)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.HasComponent(stop))
            return Entity.Null;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.HasComponent(stop))
        {
          // ISSUE: reference to a compiler-generated field
          Entity owner = this.m_OwnerData[stop].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportStationData.HasComponent(owner))
            return owner;
        }
        return stop;
      }

      private Entity GetStorageCompanyFromStop(Entity stop)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; !this.m_StorageCompanyData.HasComponent(stop); stop = this.m_OwnerData[stop].m_Owner)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_OwnerData.HasComponent(stop))
            return Entity.Null;
        }
        return stop;
      }

      private Entity GetNextStorageCompany(Entity route, Entity currentWaypoint)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[route];
        // ISSUE: reference to a compiler-generated field
        int a = this.m_WaypointData[currentWaypoint].m_Index + 1;
        for (int index1 = 0; index1 < routeWaypoint.Length; ++index1)
        {
          int index2 = math.select(a, 0, a >= routeWaypoint.Length);
          Entity waypoint = routeWaypoint[index2].m_Waypoint;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedData.HasComponent(waypoint))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity storageCompanyFromStop = this.GetStorageCompanyFromStop(this.m_ConnectedData[waypoint].m_Connected);
            if (storageCompanyFromStop != Entity.Null)
              return storageCompanyFromStop;
          }
          a = index2 + 1;
        }
        return Entity.Null;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Watercraft> __Game_Vehicles_Watercraft_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Odometer> __Game_Vehicles_Odometer_RW_ComponentTypeHandle;
      public BufferTypeHandle<WatercraftNavigationLane> __Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftData> __Game_Prefabs_WatercraftData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PublicTransportVehicleData> __Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> __Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Waypoint> __Game_Routes_Waypoint_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BoardingVehicle> __Game_Routes_BoardingVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteLane> __Game_Routes_RouteLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> __Game_Routes_Color_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportStation> __Game_Buildings_TransportStation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Lane> __Game_Net_Lane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public BufferLookup<LoadingResources> __Game_Vehicles_LoadingResources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.CargoTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PublicTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Watercraft_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Watercraft>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Odometer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Odometer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<WatercraftNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup = state.GetComponentLookup<TransportVehicleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WatercraftData_RO_ComponentLookup = state.GetComponentLookup<WatercraftData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PublicTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<PublicTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CargoTransportVehicleData_RO_ComponentLookup = state.GetComponentLookup<CargoTransportVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Waypoint_RO_ComponentLookup = state.GetComponentLookup<Waypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BoardingVehicle_RO_ComponentLookup = state.GetComponentLookup<BoardingVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteLane_RO_ComponentLookup = state.GetComponentLookup<RouteLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportStation_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RO_ComponentLookup = state.GetComponentLookup<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LoadingResources_RW_BufferLookup = state.GetBufferLookup<LoadingResources>();
      }
    }
  }
}
