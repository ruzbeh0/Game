// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MaintenanceVehicleAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class MaintenanceVehicleAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private SimulationSystem m_SimulationSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_DamageEventArchetype;
    private EntityArchetype m_MaintenanceRequestArchetype;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private MaintenanceVehicleAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 7;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<CarCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Game.Vehicles.MaintenanceVehicle>(), ComponentType.ReadWrite<Game.Common.Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_DamageEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Damage>());
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<MaintenanceRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedCarRemoveTypes = new ComponentTypeSet(new ComponentType[13]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Game.Common.Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>(),
        ComponentType.ReadWrite<PathInformation>(),
        ComponentType.ReadWrite<ServiceDispatch>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedAddTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<Stopped>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_VehicleQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<MaintenanceVehicleAISystem.MaintenanceAction> nativeQueue = new NativeQueue<MaintenanceVehicleAISystem.MaintenanceAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MaintenanceDepot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      MaintenanceVehicleAISystem.MaintenanceVehicleTickJob jobData1 = new MaintenanceVehicleAISystem.MaintenanceVehicleTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_MaintenanceVehicleType = this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_NetConditionData = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_PedestrianLaneData = this.__TypeHandle.__Game_Net_PedestrianLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_InvolvedInAccidentData = this.__TypeHandle.__Game_Events_InvolvedInAccident_RO_ComponentLookup,
        m_AccidentSiteData = this.__TypeHandle.__Game_Events_AccidentSite_RO_ComponentLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_ParkData = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup,
        m_MaintenanceDepotData = this.__TypeHandle.__Game_Buildings_MaintenanceDepot_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabMaintenanceVehicleData = this.__TypeHandle.__Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PrefabParkData = this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_LeftHandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_SimulationFrameIndex = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_MaintenanceRequestArchetype = this.m_MaintenanceRequestArchetype,
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_ActionQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      MaintenanceVehicleAISystem.MaintenanceJob jobData2 = new MaintenanceVehicleAISystem.MaintenanceJob()
      {
        m_DamageEventArchetype = this.m_DamageEventArchetype,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_CarData = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentLookup,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RW_ComponentLookup,
        m_MaintenanceVehicleData = this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup,
        m_MaintenanceConsumerData = this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RW_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RW_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RW_ComponentLookup,
        m_ParkData = this.__TypeHandle.__Game_Buildings_Park_RW_ComponentLookup,
        m_NetConditionData = this.__TypeHandle.__Game_Net_NetCondition_RW_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentLookup,
        m_ActionQueue = nativeQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle handle = jobData1.ScheduleParallel<MaintenanceVehicleAISystem.MaintenanceVehicleTickJob>(this.m_VehicleQuery, this.Dependency);
      JobHandle dependsOn = handle;
      JobHandle jobHandle = jobData2.Schedule<MaintenanceVehicleAISystem.MaintenanceJob>(dependsOn);
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(handle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public MaintenanceVehicleAISystem()
    {
    }

    private struct MaintenanceAction
    {
      public MaintenanceVehicleAISystem.MaintenanceActionType m_Type;
      public Entity m_Vehicle;
      public Entity m_Consumer;
      public Entity m_Request;
      public int m_VehicleCapacity;
      public int m_ConsumerCapacity;
      public int m_MaxMaintenanceAmount;
      public CarFlags m_WorkingFlags;
    }

    private enum MaintenanceActionType
    {
      AddRequest,
      ParkMaintenance,
      RoadMaintenance,
      RepairVehicle,
      ClearLane,
      BumpDispatchIndex,
    }

    [BurstCompile]
    private struct MaintenanceVehicleTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      public ComponentTypeHandle<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Game.Common.Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<CarNavigationLane> m_CarNavigationLaneType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<NetCondition> m_NetConditionData;
      [ReadOnly]
      public ComponentLookup<LaneCondition> m_LaneConditionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> m_PedestrianLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> m_InvolvedInAccidentData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_ParkData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.MaintenanceDepot> m_MaintenanceDepotData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicleData> m_PrefabMaintenanceVehicleData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<ParkData> m_PrefabParkData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public bool m_LeftHandTraffic;
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_MaintenanceRequestArchetype;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<MaintenanceVehicleAISystem.MaintenanceAction>.ParallelWriter m_ActionQueue;

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
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathInformation> nativeArray5 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray6 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.MaintenanceVehicle> nativeArray7 = chunk.GetNativeArray<Game.Vehicles.MaintenanceVehicle>(ref this.m_MaintenanceVehicleType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray8 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Common.Target> nativeArray9 = chunk.GetNativeArray<Game.Common.Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray10 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isStopped = chunk.Has<Stopped>(ref this.m_StoppedType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          Game.Objects.Transform transform = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          PathInformation pathInformation = nativeArray5[index];
          Game.Vehicles.MaintenanceVehicle maintenanceVehicle = nativeArray7[index];
          Car car = nativeArray8[index];
          CarCurrentLane currentLane = nativeArray6[index];
          PathOwner pathOwner = nativeArray10[index];
          Game.Common.Target target = nativeArray9[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor2[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, transform, prefabRef, pathInformation, navigationLanes, serviceDispatches, isStopped, ref random, ref maintenanceVehicle, ref car, ref currentLane, ref pathOwner, ref target);
          nativeArray7[index] = maintenanceVehicle;
          nativeArray8[index] = car;
          nativeArray6[index] = currentLane;
          nativeArray10[index] = pathOwner;
          nativeArray9[index] = target;
        }
      }

      private void Tick(
        int jobIndex,
        Entity vehicleEntity,
        Owner owner,
        Game.Objects.Transform transform,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isStopped,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        MaintenanceVehicleData prefabMaintenanceVehicleData = this.m_PrefabMaintenanceVehicleData[prefabRef.m_Prefab];
        prefabMaintenanceVehicleData.m_MaintenanceCapacity = Mathf.CeilToInt((float) prefabMaintenanceVehicleData.m_MaintenanceCapacity * maintenanceVehicle.m_Efficiency);
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, vehicleEntity, pathInformation, serviceDispatches, prefabMaintenanceVehicleData, ref random, ref maintenanceVehicle, ref car, ref currentLane, ref pathOwner);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if (VehicleUtils.IsStuck(pathOwner) || (maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) != (MaintenanceVehicleFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.ReturnToDepot(jobIndex, vehicleEntity, owner, serviceDispatches, ref car, ref maintenanceVehicle, ref pathOwner, ref target);
        }
        else if (VehicleUtils.PathEndReached(currentLane))
        {
          if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) != (MaintenanceVehicleFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working)) != MaintenanceVehicleFlags.TryWork)
          {
            if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.EdgeTarget) != (MaintenanceVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.TryMaintain(vehicleEntity, prefabMaintenanceVehicleData, ref car, ref maintenanceVehicle, ref currentLane);
              return;
            }
            if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.TransformTarget) != (MaintenanceVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.IsSecureSite(ref target))
              {
                // ISSUE: reference to a compiler-generated method
                this.TryMaintain(vehicleEntity, prefabMaintenanceVehicleData, ref car, ref maintenanceVehicle, ref currentLane, ref target);
                return;
              }
              if (isStopped)
                return;
              // ISSUE: reference to a compiler-generated method
              this.StopVehicle(jobIndex, vehicleEntity, ref currentLane);
              return;
            }
            maintenanceVehicle.m_State &= ~MaintenanceVehicleFlags.Working;
            return;
          }
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, prefabMaintenanceVehicleData, ref maintenanceVehicle, ref pathOwner);
          // ISSUE: reference to a compiler-generated method
          if (!this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, prefabMaintenanceVehicleData, ref maintenanceVehicle, ref car, ref currentLane, ref pathOwner, ref target))
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(jobIndex, vehicleEntity, owner, serviceDispatches, ref car, ref maintenanceVehicle, ref pathOwner, ref target);
          }
        }
        else
        {
          if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
          {
            if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) != (MaintenanceVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, vehicleEntity, owner, ref maintenanceVehicle, ref car, ref currentLane);
              return;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicleEntity, new Deleted());
            return;
          }
          if (VehicleUtils.WaypointReached(currentLane))
          {
            if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working | MaintenanceVehicleFlags.ClearingDebris)) != MaintenanceVehicleFlags.TryWork && (maintenanceVehicle.m_State & MaintenanceVehicleFlags.EdgeTarget) != (MaintenanceVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.TryMaintain(vehicleEntity, prefabMaintenanceVehicleData, ref car, ref maintenanceVehicle, ref currentLane);
            }
            else
            {
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
              maintenanceVehicle.m_State &= ~(MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working | MaintenanceVehicleFlags.ClearingDebris);
            }
          }
          else if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working)) != (MaintenanceVehicleFlags) 0)
            maintenanceVehicle.m_State &= ~(MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working | MaintenanceVehicleFlags.ClearingDebris);
          else if (isStopped)
          {
            // ISSUE: reference to a compiler-generated method
            this.StartVehicle(jobIndex, vehicleEntity, ref currentLane);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.TransformTarget) != (MaintenanceVehicleFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.IsBlocked) != (Game.Vehicles.CarLaneFlags) 0 && this.IsCloseEnough(transform, ref target))
            {
              // ISSUE: reference to a compiler-generated method
              this.EndNavigation(vehicleEntity, ref currentLane, ref pathOwner, navigationLanes);
            }
          }
        }
        if (maintenanceVehicle.m_Maintained >= prefabMaintenanceVehicleData.m_MaintenanceCapacity)
          maintenanceVehicle.m_State |= MaintenanceVehicleFlags.Full | MaintenanceVehicleFlags.EstimatedFull;
        else if (maintenanceVehicle.m_Maintained + maintenanceVehicle.m_MaintainEstimate >= prefabMaintenanceVehicleData.m_MaintenanceCapacity)
        {
          maintenanceVehicle.m_State |= MaintenanceVehicleFlags.EstimatedFull;
          maintenanceVehicle.m_State &= ~MaintenanceVehicleFlags.Full;
        }
        else
          maintenanceVehicle.m_State &= ~(MaintenanceVehicleFlags.Full | MaintenanceVehicleFlags.EstimatedFull);
        if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0)
        {
          if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.Full | MaintenanceVehicleFlags.Disabled)) != (MaintenanceVehicleFlags) 0)
          {
            if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.TryWork) == (MaintenanceVehicleFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(jobIndex, vehicleEntity, owner, serviceDispatches, ref car, ref maintenanceVehicle, ref pathOwner, ref target);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckMaintenancePresence(ref car, ref maintenanceVehicle, ref currentLane, navigationLanes);
          }
        }
        if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.Full | MaintenanceVehicleFlags.Disabled)) == (MaintenanceVehicleFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckServiceDispatches(vehicleEntity, serviceDispatches, prefabMaintenanceVehicleData, ref maintenanceVehicle, ref pathOwner);
          if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) != (MaintenanceVehicleFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.SelectNextDispatch(jobIndex, vehicleEntity, navigationLanes, serviceDispatches, prefabMaintenanceVehicleData, ref maintenanceVehicle, ref car, ref currentLane, ref pathOwner, ref target);
          }
          if (maintenanceVehicle.m_RequestCount <= 1 && (maintenanceVehicle.m_State & MaintenanceVehicleFlags.EstimatedFull) == (MaintenanceVehicleFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RequestTargetIfNeeded(jobIndex, vehicleEntity, ref maintenanceVehicle);
          }
        }
        else if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.TryWork) == (MaintenanceVehicleFlags) 0)
          serviceDispatches.Clear();
        if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working)) != (MaintenanceVehicleFlags) 0)
          return;
        if (VehicleUtils.RequireNewPath(pathOwner))
        {
          if (isStopped && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.ParkingSpace) == (Game.Vehicles.CarLaneFlags) 0)
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.EndOfPath;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.FindNewPath(vehicleEntity, prefabRef, prefabMaintenanceVehicleData, ref maintenanceVehicle, ref currentLane, ref pathOwner, ref target);
          }
        }
        else
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) != (PathFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingSpace(vehicleEntity, ref random, ref currentLane, ref pathOwner, navigationLanes);
        }
      }

      private void CheckParkingSpace(
        Entity entity,
        ref Unity.Mathematics.Random random,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[entity];
        ComponentLookup<Blocker> blockerData = new ComponentLookup<Blocker>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.ValidateParkingSpace(entity, ref random, ref currentLane, ref pathOwner, navigationLanes, pathElement, ref this.m_ParkedCarData, ref blockerData, ref this.m_CurveData, ref this.m_UnspawnedData, ref this.m_ParkingLaneData, ref this.m_GarageLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabParkingLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false, false, false);
      }

      private void ParkCar(
        int jobIndex,
        Entity entity,
        Owner owner,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref Car car,
        ref CarCurrentLane currentLane)
      {
        car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working | CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
        maintenanceVehicle.m_State = (MaintenanceVehicleFlags) 0;
        maintenanceVehicle.m_Maintained = 0;
        Game.Buildings.MaintenanceDepot componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceDepotData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & MaintenanceDepotFlags.HasAvailableVehicles) == (MaintenanceDepotFlags) 0)
          maintenanceVehicle.m_State |= MaintenanceVehicleFlags.Disabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_MovingToParkedCarRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_MovingToParkedAddTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(jobIndex, entity, new ParkedCar(currentLane.m_Lane, currentLane.m_CurvePosition.x));
        // ISSUE: reference to a compiler-generated field
        if (this.m_ParkingLaneData.HasComponent(currentLane.m_Lane) && currentLane.m_ChangeLane == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLane.m_Lane);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarageLaneData.HasComponent(currentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLane.m_Lane);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(currentLane.m_ChangeLane, entity));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(currentLane.m_ChangeLane, entity));
          }
        }
      }

      private bool IsSecureSite(ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_OnFireData.HasComponent(target.m_Target))
          return false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InvolvedInAccidentData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          InvolvedInAccident involvedInAccident = this.m_InvolvedInAccidentData[target.m_Target];
          // ISSUE: reference to a compiler-generated field
          if (this.m_TargetElements.HasBuffer(involvedInAccident.m_Event))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<TargetElement> targetElement = this.m_TargetElements[involvedInAccident.m_Event];
            for (int index = 0; index < targetElement.Length; ++index)
            {
              Entity entity = targetElement[index].m_Entity;
              // ISSUE: reference to a compiler-generated field
              if (this.m_AccidentSiteData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                return (this.m_AccidentSiteData[entity].m_Flags & AccidentSiteFlags.Secured) > (AccidentSiteFlags) 0;
              }
            }
          }
        }
        return true;
      }

      private void EndNavigation(
        Entity vehicleEntity,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
        currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.EndOfPath;
        navigationLanes.Clear();
        pathOwner.m_ElementIndex = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_PathElements[vehicleEntity].Clear();
      }

      private bool IsCloseEnough(Game.Objects.Transform transform, ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransformData.HasComponent(target.m_Target))
          return false;
        // ISSUE: reference to a compiler-generated field
        Game.Objects.Transform transform1 = this.m_TransformData[target.m_Target];
        return (double) math.distance(transform.m_Position, transform1.m_Position) <= 30.0;
      }

      private void StopVehicle(int jobIndex, Entity entity, ref CarCurrentLane currentLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Moving>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Swaying>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Stopped>(jobIndex, entity, new Stopped());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_Lane, new PathfindUpdated());
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(currentLaneData.m_ChangeLane))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_ChangeLane, new PathfindUpdated());
      }

      private void StartVehicle(int jobIndex, Entity entity, ref CarCurrentLane currentLaneData)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Stopped>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Moving>(jobIndex, entity, new Moving());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<InterpolatedTransform>(jobIndex, entity, new InterpolatedTransform());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Swaying>(jobIndex, entity, new Swaying());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.HasComponent(currentLaneData.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_Lane, new PathfindUpdated());
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CarLaneData.HasComponent(currentLaneData.m_ChangeLane))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLaneData.m_ChangeLane, new PathfindUpdated());
      }

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        // ISSUE: reference to a compiler-generated field
        CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) carData.m_MaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.SpecialParking,
          m_ParkingTarget = VehicleUtils.GetParkingSource(vehicleEntity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
          m_ParkingDelta = currentLane.m_CurvePosition.z,
          m_ParkingSize = VehicleUtils.GetParkingSize(vehicleEntity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData),
          m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData)
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.SpecialParking,
          m_RoadTypes = RoadTypes.Car
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = target.m_Target
        };
        if ((prefabMaintenanceVehicleData.m_MaintenanceType & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None)
          parameters.m_IgnoredRules |= RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic;
        if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.HasComponent(target.m_Target))
            destination.m_Value2 = 30f;
        }
        else
        {
          destination.m_Methods |= PathMethod.SpecialParking;
          destination.m_RandomCost = 30f;
        }
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref PathOwner pathOwner)
      {
        if (serviceDispatches.Length <= maintenanceVehicle.m_RequestCount)
          return;
        int num1 = -1;
        Entity entity = Entity.Null;
        PathElement pathElement1 = new PathElement();
        bool flag = false;
        int num2 = 0;
        if (maintenanceVehicle.m_RequestCount >= 1 && (maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[vehicleEntity];
          num2 = 1;
          if (pathOwner.m_ElementIndex < pathElement2.Length)
          {
            pathElement1 = pathElement2[pathElement2.Length - 1];
            flag = true;
          }
        }
        for (int index = num2; index < maintenanceVehicle.m_RequestCount; ++index)
        {
          DynamicBuffer<PathElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathElements.TryGetBuffer(serviceDispatches[index].m_Request, out bufferData) && bufferData.Length != 0)
          {
            pathElement1 = bufferData[bufferData.Length - 1];
            flag = true;
          }
        }
        for (int requestCount = maintenanceVehicle.m_RequestCount; requestCount < serviceDispatches.Length; ++requestCount)
        {
          Entity request = serviceDispatches[requestCount].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            MaintenanceRequest maintenanceRequest = this.m_MaintenanceRequestData[request];
            DynamicBuffer<PathElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (flag && this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
            {
              PathElement pathElement3 = bufferData[0];
              if (pathElement3.m_Target != pathElement1.m_Target || (double) pathElement3.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(maintenanceRequest.m_Target) && maintenanceRequest.m_Priority > num1)
            {
              num1 = maintenanceRequest.m_Priority;
              entity = request;
            }
          }
        }
        if (entity != Entity.Null)
        {
          serviceDispatches[maintenanceVehicle.m_RequestCount++] = new ServiceDispatch(entity);
          // ISSUE: reference to a compiler-generated field
          MaintenanceRequest maintenanceRequest = this.m_MaintenanceRequestData[entity];
          // ISSUE: reference to a compiler-generated field
          if ((prefabMaintenanceVehicleData.m_MaintenanceType & (MaintenanceType.Road | MaintenanceType.Snow)) != MaintenanceType.None && this.m_NetConditionData.HasComponent(maintenanceRequest.m_Target))
          {
            // ISSUE: reference to a compiler-generated method
            maintenanceVehicle.m_MaintainEstimate += this.PreAddMaintenanceRequests(entity);
          }
          if ((prefabMaintenanceVehicleData.m_MaintenanceType & MaintenanceType.Vehicle) != MaintenanceType.None)
          {
            Destroyed componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_DestroyedData.TryGetComponent(maintenanceRequest.m_Target, out componentData1))
            {
              float f = (float) (500.0 * (1.0 - (double) componentData1.m_Cleared));
              maintenanceVehicle.m_MaintainEstimate += Mathf.RoundToInt(f);
            }
            else
            {
              Damaged componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_DamagedData.TryGetComponent(maintenanceRequest.m_Target, out componentData2))
              {
                float f = math.min(500f, math.csum(componentData2.m_Damage) * 500f);
                maintenanceVehicle.m_MaintainEstimate += Mathf.RoundToInt(f);
              }
            }
          }
          Game.Buildings.Park componentData3;
          ParkData componentData4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((prefabMaintenanceVehicleData.m_MaintenanceType & MaintenanceType.Park) != MaintenanceType.None && this.m_ParkData.TryGetComponent(maintenanceRequest.m_Target, out componentData3) && this.m_PrefabParkData.TryGetComponent(this.m_PrefabRefData[maintenanceRequest.m_Target].m_Prefab, out componentData4))
            maintenanceVehicle.m_MaintainEstimate += math.max(0, (int) componentData4.m_MaintenancePool - (int) componentData3.m_Maintenance);
        }
        if (serviceDispatches.Length <= maintenanceVehicle.m_RequestCount)
          return;
        serviceDispatches.RemoveRange(maintenanceVehicle.m_RequestCount, serviceDispatches.Length - maintenanceVehicle.m_RequestCount);
      }

      private int BumpDispachIndex(Entity request)
      {
        int num = 0;
        MaintenanceRequest componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceRequestData.TryGetComponent(request, out componentData))
        {
          num = (int) componentData.m_DispatchIndex + 1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new MaintenanceVehicleAISystem.MaintenanceAction()
          {
            m_Type = MaintenanceVehicleAISystem.MaintenanceActionType.BumpDispatchIndex,
            m_Request = request
          });
        }
        return num;
      }

      private int PreAddMaintenanceRequests(Entity request)
      {
        int num = 0;
        DynamicBuffer<PathElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PathElements.TryGetBuffer(request, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          int dispatchIndex = this.BumpDispachIndex(request);
          Entity owner1 = Entity.Null;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            PathElement pathElement = bufferData[index];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_EdgeLaneData.HasComponent(pathElement.m_Target))
            {
              owner1 = Entity.Null;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Owner owner2 = this.m_OwnerData[pathElement.m_Target];
              if (!(owner2.m_Owner == owner1))
              {
                owner1 = owner2.m_Owner;
                // ISSUE: reference to a compiler-generated method
                if (this.HasSidewalk(owner2.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated method
                  num += this.AddMaintenanceRequests(owner2.m_Owner, request, dispatchIndex, true);
                }
              }
            }
          }
        }
        return num;
      }

      private bool HasSidewalk(Entity owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane = this.m_SubLanes[owner];
          for (int index = 0; index < subLane.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane[index].m_SubLane))
              return true;
          }
        }
        return false;
      }

      private void RequestTargetIfNeeded(
        int jobIndex,
        Entity entity,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceRequestData.HasComponent(maintenanceVehicle.m_TargetRequest) || ((int) this.m_SimulationFrameIndex & (int) math.max(512U, 16U) - 1) != 7)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MaintenanceRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ServiceRequest>(jobIndex, entity1, new ServiceRequest(true));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<MaintenanceRequest>(jobIndex, entity1, new MaintenanceRequest(entity, 1));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Game.Common.Target target)
      {
        if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0 && maintenanceVehicle.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          --maintenanceVehicle.m_RequestCount;
        }
        for (; maintenanceVehicle.m_RequestCount > 0 && serviceDispatches.Length > 0; --maintenanceVehicle.m_RequestCount)
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity target1 = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            target1 = this.m_MaintenanceRequestData[request].m_Target;
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(target1))
          {
            serviceDispatches.RemoveAt(0);
            maintenanceVehicle.m_MaintainEstimate -= maintenanceVehicle.m_MaintainEstimate / maintenanceVehicle.m_RequestCount;
          }
          else
          {
            MaintenanceVehicleFlags maintenanceVehicleFlags = (MaintenanceVehicleFlags) 0;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetConditionData.HasComponent(target1))
            {
              maintenanceVehicleFlags |= MaintenanceVehicleFlags.EdgeTarget;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(target1))
                maintenanceVehicleFlags |= MaintenanceVehicleFlags.TransformTarget;
            }
            if ((maintenanceVehicle.m_State & maintenanceVehicleFlags & MaintenanceVehicleFlags.EdgeTarget) == (MaintenanceVehicleFlags) 0)
              car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working | CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
            maintenanceVehicle.m_State &= ~(MaintenanceVehicleFlags.Returning | MaintenanceVehicleFlags.TransformTarget | MaintenanceVehicleFlags.EdgeTarget | MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working | MaintenanceVehicleFlags.ClearingDebris);
            maintenanceVehicle.m_State |= maintenanceVehicleFlags;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(request, vehicleEntity, false, true));
            // ISSUE: reference to a compiler-generated field
            if (this.m_MaintenanceRequestData.HasComponent(maintenanceVehicle.m_TargetRequest))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(maintenanceVehicle.m_TargetRequest, Entity.Null, true));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity, new EffectsUpdated());
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
                float num1 = maintenanceVehicle.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                int appendedCount;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes, out appendedCount))
                {
                  int num2 = 0;
                  bool collectMaintenance = maintenanceVehicle.m_RequestCount == 1;
                  if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.EdgeTarget) != (MaintenanceVehicleFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    int dispatchIndex = this.BumpDispachIndex(request);
                    int index1 = pathElement2.Length - appendedCount;
                    for (int index2 = 0; index2 < index1; ++index2)
                    {
                      PathElement pathElement3 = pathElement2[index2];
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PedestrianLaneData.HasComponent(pathElement3.m_Target))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Owner owner = this.m_OwnerData[pathElement3.m_Target];
                        // ISSUE: reference to a compiler-generated method
                        num2 += this.AddMaintenanceRequests(owner.m_Owner, request, dispatchIndex, collectMaintenance);
                      }
                    }
                    if (appendedCount > 0)
                    {
                      NativeArray<PathElement> nativeArray = new NativeArray<PathElement>(appendedCount, Allocator.Temp);
                      for (int index3 = 0; index3 < appendedCount; ++index3)
                        nativeArray[index3] = pathElement2[index1 + index3];
                      pathElement2.RemoveRange(index1, appendedCount);
                      Entity lastOwner = Entity.Null;
                      for (int index4 = 0; index4 < nativeArray.Length; ++index4)
                      {
                        // ISSUE: reference to a compiler-generated method
                        num2 += this.AddPathElement(pathElement2, nativeArray[index4], request, dispatchIndex, ref lastOwner, collectMaintenance);
                      }
                      nativeArray.Dispose();
                    }
                    car.m_Flags |= CarFlags.StayOnRoad;
                  }
                  else if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.TransformTarget) != (MaintenanceVehicleFlags) 0)
                  {
                    if (collectMaintenance)
                    {
                      if ((prefabMaintenanceVehicleData.m_MaintenanceType & MaintenanceType.Vehicle) != MaintenanceType.None)
                      {
                        Destroyed componentData1;
                        // ISSUE: reference to a compiler-generated field
                        if (this.m_DestroyedData.TryGetComponent(target1, out componentData1))
                        {
                          float f = (float) (500.0 * (1.0 - (double) componentData1.m_Cleared));
                          num2 += Mathf.RoundToInt(f);
                        }
                        else
                        {
                          Damaged componentData2;
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_DamagedData.TryGetComponent(target1, out componentData2))
                          {
                            float f = math.min(500f, math.csum(componentData2.m_Damage) * 500f);
                            num2 += Mathf.RoundToInt(f);
                          }
                        }
                      }
                      Game.Buildings.Park componentData3;
                      ParkData componentData4;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((prefabMaintenanceVehicleData.m_MaintenanceType & MaintenanceType.Park) != MaintenanceType.None && this.m_ParkData.TryGetComponent(target1, out componentData3) && this.m_PrefabParkData.TryGetComponent(this.m_PrefabRefData[target1].m_Prefab, out componentData4))
                        num2 += math.max(0, (int) componentData4.m_MaintenancePool - (int) componentData3.m_Maintenance);
                    }
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_VehicleData.HasComponent(target1))
                      car.m_Flags |= CarFlags.StayOnRoad;
                    else
                      car.m_Flags &= ~CarFlags.StayOnRoad;
                  }
                  else
                    car.m_Flags |= CarFlags.StayOnRoad;
                  if (collectMaintenance)
                    maintenanceVehicle.m_MaintainEstimate = num2;
                  if ((prefabMaintenanceVehicleData.m_MaintenanceType & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None)
                    car.m_Flags |= CarFlags.UsePublicTransportLanes;
                  maintenanceVehicle.m_PathElementTime = num1 / (float) math.max(1, pathElement2.Length);
                  target.m_Target = target1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref target, target1);
            return true;
          }
        }
        return false;
      }

      private void ReturnToDepot(
        int jobIndex,
        Entity vehicleEntity,
        Owner ownerData,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Car car,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref PathOwner pathOwnerData,
        ref Game.Common.Target targetData)
      {
        serviceDispatches.Clear();
        car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working | CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
        maintenanceVehicle.m_RequestCount = 0;
        maintenanceVehicle.m_MaintainEstimate = 0;
        maintenanceVehicle.m_State &= ~(MaintenanceVehicleFlags.TransformTarget | MaintenanceVehicleFlags.EdgeTarget | MaintenanceVehicleFlags.TryWork | MaintenanceVehicleFlags.Working | MaintenanceVehicleFlags.ClearingDebris);
        maintenanceVehicle.m_State |= MaintenanceVehicleFlags.Returning;
        VehicleUtils.SetTarget(ref pathOwnerData, ref targetData, ownerData.m_Owner);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, vehicleEntity, new EffectsUpdated());
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        ref Unity.Mathematics.Random random,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref Car carData,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PathUtils.ResetPath(ref currentLane, pathElement, this.m_SlaveLaneData, this.m_OwnerData, this.m_SubLanes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.ResetParkingLaneStatus(vehicleEntity, ref currentLane, ref pathOwner, pathElement, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetParkingCurvePos(vehicleEntity, ref random, currentLane, pathOwner, pathElement, ref this.m_ParkedCarData, ref this.m_UnspawnedData, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, ref this.m_PrefabParkingLaneData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false);
        currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Checked;
        if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0 && maintenanceVehicle.m_RequestCount > 0 && serviceDispatches.Length > 0)
        {
          Entity request = serviceDispatches[0].m_Request;
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            Entity target = this.m_MaintenanceRequestData[request].m_Target;
            int num = 0;
            bool collectMaintenance = maintenanceVehicle.m_RequestCount == 1;
            if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.EdgeTarget) != (MaintenanceVehicleFlags) 0)
            {
              NativeArray<PathElement> nativeArray = new NativeArray<PathElement>(pathElement.Length, Allocator.Temp);
              nativeArray.CopyFrom(pathElement.AsNativeArray());
              pathElement.Clear();
              Entity lastOwner = Entity.Null;
              // ISSUE: reference to a compiler-generated method
              int dispatchIndex = this.BumpDispachIndex(request);
              for (int index = 0; index < nativeArray.Length; ++index)
              {
                // ISSUE: reference to a compiler-generated method
                num += this.AddPathElement(pathElement, nativeArray[index], request, dispatchIndex, ref lastOwner, collectMaintenance);
              }
              nativeArray.Dispose();
              carData.m_Flags |= CarFlags.StayOnRoad;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.HasComponent(target))
              {
                if (collectMaintenance)
                {
                  if ((prefabMaintenanceVehicleData.m_MaintenanceType & MaintenanceType.Vehicle) != MaintenanceType.None)
                  {
                    Destroyed componentData1;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_DestroyedData.TryGetComponent(target, out componentData1))
                    {
                      float f = (float) (500.0 * (1.0 - (double) componentData1.m_Cleared));
                      num += Mathf.RoundToInt(f);
                    }
                    else
                    {
                      Damaged componentData2;
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_DamagedData.TryGetComponent(target, out componentData2))
                      {
                        float f = math.min(500f, math.csum(componentData2.m_Damage) * 500f);
                        num += Mathf.RoundToInt(f);
                      }
                    }
                  }
                  Game.Buildings.Park componentData3;
                  ParkData componentData4;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if ((prefabMaintenanceVehicleData.m_MaintenanceType & MaintenanceType.Park) != MaintenanceType.None && this.m_ParkData.TryGetComponent(target, out componentData3) && this.m_PrefabParkData.TryGetComponent(this.m_PrefabRefData[target].m_Prefab, out componentData4))
                    num += math.max(0, (int) componentData4.m_MaintenancePool - (int) componentData3.m_Maintenance);
                }
                // ISSUE: reference to a compiler-generated field
                if (this.m_VehicleData.HasComponent(target))
                  carData.m_Flags |= CarFlags.StayOnRoad;
                else
                  carData.m_Flags &= ~CarFlags.StayOnRoad;
              }
              else
                carData.m_Flags |= CarFlags.StayOnRoad;
            }
            if (collectMaintenance)
              maintenanceVehicle.m_MaintainEstimate = num;
          }
          else
            carData.m_Flags |= CarFlags.StayOnRoad;
        }
        else
          carData.m_Flags &= ~CarFlags.StayOnRoad;
        if ((prefabMaintenanceVehicleData.m_MaintenanceType & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None)
          carData.m_Flags |= CarFlags.UsePublicTransportLanes;
        maintenanceVehicle.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement.Length);
      }

      private int AddPathElement(
        DynamicBuffer<PathElement> path,
        PathElement pathElement,
        Entity request,
        int dispatchIndex,
        ref Entity lastOwner,
        bool collectMaintenance)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.HasComponent(pathElement.m_Target))
        {
          path.Add(pathElement);
          lastOwner = Entity.Null;
          return num;
        }
        // ISSUE: reference to a compiler-generated field
        Owner owner = this.m_OwnerData[pathElement.m_Target];
        if (owner.m_Owner == lastOwner)
        {
          path.Add(pathElement);
          return num;
        }
        lastOwner = owner.m_Owner;
        float y = pathElement.m_TargetDelta.y;
        Entity sidewalk;
        // ISSUE: reference to a compiler-generated method
        if (this.FindClosestSidewalk(pathElement.m_Target, owner.m_Owner, ref y, out sidewalk))
        {
          // ISSUE: reference to a compiler-generated method
          num += this.AddMaintenanceRequests(owner.m_Owner, request, dispatchIndex, collectMaintenance);
          path.Add(pathElement);
          path.Add(new PathElement(sidewalk, (float2) y));
        }
        else
          path.Add(pathElement);
        return num;
      }

      private bool FindClosestSidewalk(
        Entity lane,
        Entity owner,
        ref float curvePos,
        out Entity sidewalk)
      {
        bool closestSidewalk = false;
        sidewalk = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = MathUtils.Position(this.m_CurveData[lane].m_Bezier, curvePos);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
          float num1 = float.MaxValue;
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PedestrianLaneData.HasComponent(subLane2))
            {
              float t;
              // ISSUE: reference to a compiler-generated field
              float num2 = MathUtils.Distance(MathUtils.Line(this.m_CurveData[subLane2].m_Bezier), position, out t);
              if ((double) num2 < (double) num1)
              {
                curvePos = t;
                sidewalk = subLane2;
                num1 = num2;
                closestSidewalk = true;
              }
            }
          }
        }
        return closestSidewalk;
      }

      private int AddMaintenanceRequests(
        Entity edgeEntity,
        Entity request,
        int dispatchIndex,
        bool collectMaintenance)
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetConditionData.HasComponent(edgeEntity))
        {
          if (collectMaintenance)
          {
            // ISSUE: reference to a compiler-generated field
            Game.Net.Edge edge = this.m_EdgeData[edgeEntity];
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            num = Mathf.RoundToInt(this.CalculateTotalLaneWear(edgeEntity) + (float) (((double) this.CalculateTotalLaneWear(edge.m_Start) + (double) this.CalculateTotalLaneWear(edge.m_End)) * 0.5));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new MaintenanceVehicleAISystem.MaintenanceAction()
          {
            m_Type = MaintenanceVehicleAISystem.MaintenanceActionType.AddRequest,
            m_Consumer = edgeEntity,
            m_Request = request,
            m_VehicleCapacity = dispatchIndex
          });
        }
        return num;
      }

      private float CalculateTotalLaneWear(Entity owner)
      {
        float totalLaneWear = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneConditionData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              LaneCondition laneCondition = this.m_LaneConditionData[subLane2];
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane2];
              totalLaneWear += (float) ((double) laneCondition.m_Wear * (double) curve.m_Length * 0.0099999997764825821);
            }
          }
        }
        return totalLaneWear;
      }

      private void CheckMaintenancePresence(
        ref Car car,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref CarCurrentLane currentLane,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.ClearChecked) != (MaintenanceVehicleFlags) 0)
        {
          if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0)
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Checked;
          maintenanceVehicle.m_State &= ~MaintenanceVehicleFlags.ClearChecked;
        }
        if ((currentLane.m_LaneFlags & (Game.Vehicles.CarLaneFlags.Waypoint | Game.Vehicles.CarLaneFlags.Checked)) == Game.Vehicles.CarLaneFlags.Waypoint)
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.CheckMaintenancePresence(currentLane.m_Lane))
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
            car.m_Flags &= ~(CarFlags.Warning | CarFlags.Working | CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
            // ISSUE: reference to a compiler-generated field
            if (this.m_SlaveLaneData.HasComponent(currentLane.m_Lane))
              currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.FixedLane;
          }
          currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
        }
        if ((currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0)
        {
          car.m_Flags |= CarFlags.Warning;
          if ((car.m_Flags & CarFlags.Working) != (CarFlags) 0 || (double) math.abs(currentLane.m_CurvePosition.x - currentLane.m_CurvePosition.z) >= 0.5)
            return;
          // ISSUE: reference to a compiler-generated method
          car.m_Flags |= this.GetWorkingFlags(ref currentLane);
        }
        else
        {
          for (int index = 0; index < navigationLanes.Length; ++index)
          {
            ref CarNavigationLane local = ref navigationLanes.ElementAt(index);
            if ((local.m_Flags & Game.Vehicles.CarLaneFlags.Waypoint) != (Game.Vehicles.CarLaneFlags) 0 && (currentLane.m_LaneFlags & Game.Vehicles.CarLaneFlags.Checked) == (Game.Vehicles.CarLaneFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.CheckMaintenancePresence(local.m_Lane))
              {
                local.m_Flags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
                car.m_Flags &= ~CarFlags.Warning;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SlaveLaneData.HasComponent(local.m_Lane))
                  local.m_Flags &= ~Game.Vehicles.CarLaneFlags.FixedLane;
              }
              currentLane.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Checked;
              car.m_Flags &= ~(CarFlags.Working | CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
            }
            if ((local.m_Flags & (Game.Vehicles.CarLaneFlags.Reserved | Game.Vehicles.CarLaneFlags.Waypoint)) != Game.Vehicles.CarLaneFlags.Reserved)
            {
              car.m_Flags &= ~(CarFlags.Working | CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
              break;
            }
          }
        }
      }

      private CarFlags GetWorkingFlags(ref CarCurrentLane currentLaneData)
      {
        Game.Net.CarLaneFlags carLaneFlags = Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit;
        Game.Net.CarLane componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CarLaneData.TryGetComponent(currentLaneData.m_Lane, out componentData))
          carLaneFlags &= componentData.m_Flags;
        // ISSUE: reference to a compiler-generated field
        return carLaneFlags == Game.Net.CarLaneFlags.RightLimit || carLaneFlags != Game.Net.CarLaneFlags.LeftLimit && !this.m_LeftHandTraffic ? CarFlags.Working | CarFlags.SignalAnimation1 : CarFlags.Working | CarFlags.SignalAnimation2;
      }

      private bool CheckMaintenancePresence(Entity laneEntity)
      {
        Owner componentData1;
        NetCondition componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_EdgeLaneData.HasComponent(laneEntity) && this.m_OwnerData.TryGetComponent(laneEntity, out componentData1) && this.m_NetConditionData.TryGetComponent(componentData1.m_Owner, out componentData2) && math.any(componentData2.m_Wear >= 0.099999994f);
      }

      private void TryMaintain(
        Entity vehicleEntity,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        ref Car car,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref CarCurrentLane currentLaneData)
      {
        maintenanceVehicle.m_State |= MaintenanceVehicleFlags.TryWork;
        maintenanceVehicle.m_State &= ~MaintenanceVehicleFlags.Working;
        if (maintenanceVehicle.m_Maintained >= prefabMaintenanceVehicleData.m_MaintenanceCapacity)
          return;
        // ISSUE: reference to a compiler-generated method
        this.TryMaintainLane(vehicleEntity, prefabMaintenanceVehicleData, currentLaneData.m_Lane, ref currentLaneData);
      }

      private void TryMaintainLane(
        Entity vehicleEntity,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        Entity laneEntity,
        ref CarCurrentLane currentLaneData)
      {
        Owner componentData1;
        NetCondition componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EdgeLaneData.HasComponent(laneEntity) || !this.m_OwnerData.TryGetComponent(laneEntity, out componentData1) || !this.m_NetConditionData.TryGetComponent(componentData1.m_Owner, out componentData2) || !math.any(componentData2.m_Wear >= 0.099999994f))
          return;
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<MaintenanceVehicleAISystem.MaintenanceAction>.ParallelWriter local1 = ref this.m_ActionQueue;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MaintenanceVehicleAISystem.MaintenanceAction maintenanceAction1 = new MaintenanceVehicleAISystem.MaintenanceAction();
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_Type = MaintenanceVehicleAISystem.MaintenanceActionType.RoadMaintenance;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_Vehicle = vehicleEntity;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_Consumer = componentData1.m_Owner;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_VehicleCapacity = prefabMaintenanceVehicleData.m_MaintenanceCapacity;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_ConsumerCapacity = 0;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_MaxMaintenanceAmount = Mathf.RoundToInt((float) (prefabMaintenanceVehicleData.m_MaintenanceRate * 16) / 60f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        maintenanceAction1.m_WorkingFlags = this.GetWorkingFlags(ref currentLaneData);
        // ISSUE: variable of a compiler-generated type
        MaintenanceVehicleAISystem.MaintenanceAction maintenanceAction2 = maintenanceAction1;
        local1.Enqueue(maintenanceAction2);
        // ISSUE: reference to a compiler-generated field
        ref NativeQueue<MaintenanceVehicleAISystem.MaintenanceAction>.ParallelWriter local2 = ref this.m_ActionQueue;
        // ISSUE: object of a compiler-generated type is created
        maintenanceAction1 = new MaintenanceVehicleAISystem.MaintenanceAction();
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_Type = MaintenanceVehicleAISystem.MaintenanceActionType.ClearLane;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_Vehicle = vehicleEntity;
        // ISSUE: reference to a compiler-generated field
        maintenanceAction1.m_Consumer = laneEntity;
        // ISSUE: variable of a compiler-generated type
        MaintenanceVehicleAISystem.MaintenanceAction maintenanceAction3 = maintenanceAction1;
        local2.Enqueue(maintenanceAction3);
      }

      private void TryMaintain(
        Entity vehicleEntity,
        MaintenanceVehicleData prefabMaintenanceVehicleData,
        ref Car car,
        ref Game.Vehicles.MaintenanceVehicle maintenanceVehicle,
        ref CarCurrentLane currentLaneData,
        ref Game.Common.Target targetData)
      {
        maintenanceVehicle.m_State |= MaintenanceVehicleFlags.TryWork;
        maintenanceVehicle.m_State &= ~MaintenanceVehicleFlags.Working;
        // ISSUE: reference to a compiler-generated field
        if (maintenanceVehicle.m_Maintained >= prefabMaintenanceVehicleData.m_MaintenanceCapacity || !this.m_PrefabRefData.HasComponent(targetData.m_Target))
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[targetData.m_Target];
        // ISSUE: reference to a compiler-generated field
        if (this.m_VehicleData.HasComponent(targetData.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new MaintenanceVehicleAISystem.MaintenanceAction()
          {
            m_Type = MaintenanceVehicleAISystem.MaintenanceActionType.RepairVehicle,
            m_Vehicle = vehicleEntity,
            m_Consumer = targetData.m_Target,
            m_VehicleCapacity = prefabMaintenanceVehicleData.m_MaintenanceCapacity,
            m_MaxMaintenanceAmount = Mathf.RoundToInt((float) (prefabMaintenanceVehicleData.m_MaintenanceRate * 16) / 60f),
            m_WorkingFlags = this.GetWorkingFlags(ref currentLaneData)
          });
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabParkData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          ParkData parkData = this.m_PrefabParkData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ActionQueue.Enqueue(new MaintenanceVehicleAISystem.MaintenanceAction()
          {
            m_Type = MaintenanceVehicleAISystem.MaintenanceActionType.ParkMaintenance,
            m_Vehicle = vehicleEntity,
            m_Consumer = targetData.m_Target,
            m_VehicleCapacity = prefabMaintenanceVehicleData.m_MaintenanceCapacity,
            m_ConsumerCapacity = (int) parkData.m_MaintenancePool,
            m_MaxMaintenanceAmount = Mathf.RoundToInt((float) (prefabMaintenanceVehicleData.m_MaintenanceRate * 16) / 60f),
            m_WorkingFlags = CarFlags.Working
          });
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
    private struct MaintenanceJob : IJob
    {
      [ReadOnly]
      public EntityArchetype m_DamageEventArchetype;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      public ComponentLookup<Car> m_CarData;
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleData;
      public ComponentLookup<MaintenanceConsumer> m_MaintenanceConsumerData;
      public ComponentLookup<Damaged> m_DamagedData;
      public ComponentLookup<Destroyed> m_DestroyedData;
      public ComponentLookup<Game.Buildings.Park> m_ParkData;
      public ComponentLookup<NetCondition> m_NetConditionData;
      public ComponentLookup<LaneCondition> m_LaneConditionData;
      public NativeQueue<MaintenanceVehicleAISystem.MaintenanceAction> m_ActionQueue;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        MaintenanceVehicleAISystem.MaintenanceAction maintenanceAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ActionQueue.TryDequeue(out maintenanceAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          MaintenanceVehicleAISystem.MaintenanceActionType type = maintenanceAction.m_Type;
          switch (type)
          {
            case MaintenanceVehicleAISystem.MaintenanceActionType.AddRequest:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MaintenanceConsumer maintenanceConsumer = this.m_MaintenanceConsumerData[maintenanceAction.m_Consumer] with
              {
                m_Request = maintenanceAction.m_Request,
                m_DispatchIndex = (byte) maintenanceAction.m_VehicleCapacity
              };
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_MaintenanceConsumerData[maintenanceAction.m_Consumer] = maintenanceConsumer;
              continue;
            case MaintenanceVehicleAISystem.MaintenanceActionType.ParkMaintenance:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Car car1 = this.m_CarData[maintenanceAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.MaintenanceVehicle maintenanceVehicle1 = this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Buildings.Park park = this.m_ParkData[maintenanceAction.m_Consumer];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int num1 = math.min(math.min(maintenanceAction.m_VehicleCapacity - maintenanceVehicle1.m_Maintained, maintenanceAction.m_ConsumerCapacity - (int) park.m_Maintenance), maintenanceAction.m_MaxMaintenanceAmount);
              if (num1 > 0)
              {
                maintenanceVehicle1.m_Maintained += num1;
                maintenanceVehicle1.m_MaintainEstimate = math.max(0, maintenanceVehicle1.m_MaintainEstimate - num1);
                park.m_Maintenance += (short) num1;
                maintenanceVehicle1.m_State |= MaintenanceVehicleFlags.Working;
                // ISSUE: reference to a compiler-generated field
                car1.m_Flags |= CarFlags.Warning | maintenanceAction.m_WorkingFlags;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CarData[maintenanceAction.m_Vehicle] = car1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle] = maintenanceVehicle1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_ParkData[maintenanceAction.m_Consumer] = park;
                continue;
              }
              continue;
            case MaintenanceVehicleAISystem.MaintenanceActionType.RoadMaintenance:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Car car2 = this.m_CarData[maintenanceAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.MaintenanceVehicle maintenanceVehicle2 = this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Net.Edge edge = this.m_EdgeData[maintenanceAction.m_Consumer];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              float x1 = this.CalculateTotalLaneWear(maintenanceAction.m_Consumer) + this.CalculateTotalLaneWear(edge.m_Start) + this.CalculateTotalLaneWear(edge.m_End);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int num2 = math.min(math.min(maintenanceAction.m_VehicleCapacity - maintenanceVehicle2.m_Maintained, (int) math.ceil(x1)), maintenanceAction.m_MaxMaintenanceAmount);
              if (num2 > 0)
              {
                maintenanceVehicle2.m_Maintained += num2;
                maintenanceVehicle2.m_MaintainEstimate = math.max(0, maintenanceVehicle2.m_MaintainEstimate - num2);
                float maintainFactor = math.saturate((float) (1.0 - (double) num2 / (double) x1));
                maintenanceVehicle2.m_State |= MaintenanceVehicleFlags.Working;
                car2.m_Flags &= ~(CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
                // ISSUE: reference to a compiler-generated field
                car2.m_Flags |= CarFlags.Warning | maintenanceAction.m_WorkingFlags;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                float2 float2_1 = this.MaintainLanes(maintenanceAction.m_Consumer, maintainFactor);
                // ISSUE: reference to a compiler-generated method
                float2 float2_2 = this.MaintainLanes(edge.m_Start, maintainFactor);
                // ISSUE: reference to a compiler-generated method
                float2 float2_3 = this.MaintainLanes(edge.m_End, maintainFactor);
                NetCondition componentData1;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_NetConditionData.TryGetComponent(maintenanceAction.m_Consumer, out componentData1))
                {
                  componentData1.m_Wear = float2_1;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_NetConditionData[maintenanceAction.m_Consumer] = componentData1;
                }
                NetCondition componentData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NetConditionData.TryGetComponent(edge.m_Start, out componentData2))
                {
                  componentData2.m_Wear = float2_2;
                  // ISSUE: reference to a compiler-generated field
                  this.m_NetConditionData[edge.m_Start] = componentData2;
                }
                NetCondition componentData3;
                // ISSUE: reference to a compiler-generated field
                if (this.m_NetConditionData.TryGetComponent(edge.m_End, out componentData3))
                {
                  componentData3.m_Wear = float2_3;
                  // ISSUE: reference to a compiler-generated field
                  this.m_NetConditionData[edge.m_End] = componentData3;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CarData[maintenanceAction.m_Vehicle] = car2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle] = maintenanceVehicle2;
                continue;
              }
              continue;
            case MaintenanceVehicleAISystem.MaintenanceActionType.RepairVehicle:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Car car3 = this.m_CarData[maintenanceAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Game.Vehicles.MaintenanceVehicle maintenanceVehicle3 = this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_DestroyedData.HasComponent(maintenanceAction.m_Consumer))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Destroyed destroyed = this.m_DestroyedData[maintenanceAction.m_Consumer];
                float x2 = (float) (500.0 * (1.0 - (double) destroyed.m_Cleared));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int num3 = math.min(math.min(maintenanceAction.m_VehicleCapacity - maintenanceVehicle3.m_Maintained, (int) math.ceil(x2)), maintenanceAction.m_MaxMaintenanceAmount);
                if (num3 > 0)
                {
                  maintenanceVehicle3.m_Maintained += num3;
                  maintenanceVehicle3.m_MaintainEstimate = math.max(0, maintenanceVehicle3.m_MaintainEstimate - num3);
                  destroyed.m_Cleared = (float) (1.0 - (1.0 - (double) destroyed.m_Cleared) * (double) math.saturate((float) (1.0 - (double) num3 / (double) x2)));
                  maintenanceVehicle3.m_State |= MaintenanceVehicleFlags.Working;
                  car3.m_Flags &= ~(CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
                  // ISSUE: reference to a compiler-generated field
                  car3.m_Flags |= CarFlags.Warning | maintenanceAction.m_WorkingFlags;
                  if ((maintenanceVehicle3.m_State & MaintenanceVehicleFlags.ClearingDebris) == (MaintenanceVehicleFlags) 0)
                  {
                    maintenanceVehicle3.m_State |= MaintenanceVehicleFlags.ClearingDebris;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<EffectsUpdated>(maintenanceAction.m_Vehicle, new EffectsUpdated());
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CarData[maintenanceAction.m_Vehicle] = car3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle] = maintenanceVehicle3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_DestroyedData[maintenanceAction.m_Consumer] = destroyed;
                  continue;
                }
                continue;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_DamagedData.HasComponent(maintenanceAction.m_Consumer))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Damaged damaged = this.m_DamagedData[maintenanceAction.m_Consumer];
                float x3 = math.min(500f, math.csum(damaged.m_Damage) * 500f);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int num4 = math.min(math.min(maintenanceAction.m_VehicleCapacity - maintenanceVehicle3.m_Maintained, (int) math.ceil(x3)), maintenanceAction.m_MaxMaintenanceAmount);
                if (num4 > 0)
                {
                  maintenanceVehicle3.m_Maintained += num4;
                  maintenanceVehicle3.m_MaintainEstimate = math.max(0, maintenanceVehicle3.m_MaintainEstimate - num4);
                  damaged.m_Damage *= math.saturate((float) (1.0 - (double) num4 / (double) x3));
                  maintenanceVehicle3.m_State |= MaintenanceVehicleFlags.Working;
                  car3.m_Flags &= ~(CarFlags.SignalAnimation1 | CarFlags.SignalAnimation2);
                  // ISSUE: reference to a compiler-generated field
                  car3.m_Flags |= CarFlags.Warning | maintenanceAction.m_WorkingFlags;
                  if ((maintenanceVehicle3.m_State & MaintenanceVehicleFlags.ClearingDebris) == (MaintenanceVehicleFlags) 0)
                  {
                    maintenanceVehicle3.m_State |= MaintenanceVehicleFlags.ClearingDebris;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<EffectsUpdated>(maintenanceAction.m_Vehicle, new EffectsUpdated());
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CarData[maintenanceAction.m_Vehicle] = car3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_MaintenanceVehicleData[maintenanceAction.m_Vehicle] = maintenanceVehicle3;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_DamagedData[maintenanceAction.m_Consumer] = damaged;
                  if (!math.any(damaged.m_Damage > 0.0f))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<Damage>(this.m_CommandBuffer.CreateEntity(this.m_DamageEventArchetype), new Damage(maintenanceAction.m_Consumer, new float3(0.0f, 0.0f, 0.0f)));
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
            case MaintenanceVehicleAISystem.MaintenanceActionType.ClearLane:
              DynamicBuffer<LaneObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneObjects.TryGetBuffer(maintenanceAction.m_Consumer, out bufferData))
              {
                for (int index = 0; index < bufferData.Length; ++index)
                {
                  Entity laneObject = bufferData[index].m_LaneObject;
                  Game.Vehicles.MaintenanceVehicle componentData;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (laneObject != maintenanceAction.m_Vehicle && this.m_MaintenanceVehicleData.TryGetComponent(laneObject, out componentData))
                  {
                    componentData.m_State |= MaintenanceVehicleFlags.ClearChecked;
                    // ISSUE: reference to a compiler-generated field
                    this.m_MaintenanceVehicleData[laneObject] = componentData;
                  }
                }
                continue;
              }
              continue;
            case MaintenanceVehicleAISystem.MaintenanceActionType.BumpDispatchIndex:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              MaintenanceRequest maintenanceRequest = this.m_MaintenanceRequestData[maintenanceAction.m_Request];
              ++maintenanceRequest.m_DispatchIndex;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_MaintenanceRequestData[maintenanceAction.m_Request] = maintenanceRequest;
              continue;
            default:
              continue;
          }
        }
      }

      private float CalculateTotalLaneWear(Entity owner)
      {
        float totalLaneWear = 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneConditionData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              LaneCondition laneCondition = this.m_LaneConditionData[subLane2];
              // ISSUE: reference to a compiler-generated field
              Curve curve = this.m_CurveData[subLane2];
              totalLaneWear += (float) ((double) laneCondition.m_Wear * (double) curve.m_Length * 0.0099999997764825821);
            }
          }
        }
        return totalLaneWear;
      }

      private float2 MaintainLanes(Entity owner, float maintainFactor)
      {
        float2 float2 = (float2) 0.0f;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubLanes.HasBuffer(owner))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Net.SubLane> subLane1 = this.m_SubLanes[owner];
          for (int index = 0; index < subLane1.Length; ++index)
          {
            Entity subLane2 = subLane1[index].m_SubLane;
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneConditionData.HasComponent(subLane2))
            {
              // ISSUE: reference to a compiler-generated field
              LaneCondition laneCondition = this.m_LaneConditionData[subLane2];
              laneCondition.m_Wear *= maintainFactor;
              EdgeLane componentData;
              // ISSUE: reference to a compiler-generated field
              float2 = !this.m_EdgeLaneData.TryGetComponent(subLane2, out componentData) ? math.max(float2, (float2) laneCondition.m_Wear) : math.select(float2, (float2) laneCondition.m_Wear, new bool2(math.any(componentData.m_EdgeDelta == 0.0f), math.any(componentData.m_EdgeDelta == 1f)) & laneCondition.m_Wear > float2);
              // ISSUE: reference to a compiler-generated field
              this.m_LaneConditionData[subLane2] = laneCondition;
            }
          }
        }
        return float2;
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.MaintenanceVehicle> __Game_Vehicles_MaintenanceVehicle_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Common.Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCondition> __Game_Net_NetCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.PedestrianLane> __Game_Net_PedestrianLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InvolvedInAccident> __Game_Events_InvolvedInAccident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AccidentSite> __Game_Events_AccidentSite_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.MaintenanceDepot> __Game_Buildings_MaintenanceDepot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicleData> __Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkData> __Game_Prefabs_ParkData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RO_BufferLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;
      public ComponentLookup<Car> __Game_Vehicles_Car_RW_ComponentLookup;
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> __Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup;
      public ComponentLookup<MaintenanceConsumer> __Game_Simulation_MaintenanceConsumer_RW_ComponentLookup;
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RW_ComponentLookup;
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RW_ComponentLookup;
      public ComponentLookup<Game.Buildings.Park> __Game_Buildings_Park_RW_ComponentLookup;
      public ComponentLookup<NetCondition> __Game_Net_NetCondition_RW_ComponentLookup;
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RW_ComponentLookup;

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
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_MaintenanceVehicle_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.MaintenanceVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Common.Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentLookup = state.GetComponentLookup<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RO_ComponentLookup = state.GetComponentLookup<LaneCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_PedestrianLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.PedestrianLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InvolvedInAccident_RO_ComponentLookup = state.GetComponentLookup<InvolvedInAccident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_AccidentSite_RO_ComponentLookup = state.GetComponentLookup<AccidentSite>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MaintenanceDepot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.MaintenanceDepot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup = state.GetComponentLookup<MaintenanceVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkData_RO_ComponentLookup = state.GetComponentLookup<ParkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RO_BufferLookup = state.GetBufferLookup<TargetElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentLookup = state.GetComponentLookup<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RW_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.MaintenanceVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceConsumer_RW_ComponentLookup = state.GetComponentLookup<MaintenanceConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RW_ComponentLookup = state.GetComponentLookup<Damaged>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RW_ComponentLookup = state.GetComponentLookup<Destroyed>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RW_ComponentLookup = state.GetComponentLookup<Game.Buildings.Park>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RW_ComponentLookup = state.GetComponentLookup<NetCondition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RW_ComponentLookup = state.GetComponentLookup<LaneCondition>();
      }
    }
  }
}
