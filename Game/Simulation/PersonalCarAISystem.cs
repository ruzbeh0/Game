// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PersonalCarAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
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
  public class PersonalCarAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private CitySystem m_CitySystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private TimeSystem m_TimeSystem;
    private ServiceFeeSystem m_ServiceFeeSystem;
    private PersonalCarAISystem.Actions m_Actions;
    private EntityQuery m_VehicleQuery;
    private ComponentTypeSet m_MovingToParkedCarRemoveTypes;
    private ComponentTypeSet m_MovingToParkedCarAddTypes;
    private PersonalCarAISystem.TypeHandle __TypeHandle;

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
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeSystem = this.World.GetOrCreateSystemManaged<ServiceFeeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Actions = this.World.GetOrCreateSystemManaged<PersonalCarAISystem.Actions>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Vehicles.PersonalCar>(), ComponentType.ReadOnly<CarCurrentLane>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>(), ComponentType.Exclude<Destroyed>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedCarRemoveTypes = new ComponentTypeSet(new ComponentType[11]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<Swaying>(),
        ComponentType.ReadWrite<CarNavigation>(),
        ComponentType.ReadWrite<CarNavigationLane>(),
        ComponentType.ReadWrite<CarCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
        ComponentType.ReadWrite<Blocker>(),
        ComponentType.ReadWrite<PathElement>()
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedCarAddTypes = new ComponentTypeSet(ComponentType.ReadWrite<ParkedCar>(), ComponentType.ReadWrite<Stopped>(), ComponentType.ReadWrite<Updated>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_MoneyTransferQueue = new NativeQueue<PersonalCarAISystem.MoneyTransfer>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps1;
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new PersonalCarAISystem.PersonalCarTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_PersonalCarType = this.__TypeHandle.__Game_Vehicles_PersonalCar_RW_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle,
        m_CarNavigationLaneType = this.__TypeHandle.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabCreatureData = this.__TypeHandle.__Game_Prefabs_CreatureData_RO_ComponentLookup,
        m_PrefabHumanData = this.__TypeHandle.__Game_Prefabs_HumanData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_GarageLaneData = this.__TypeHandle.__Game_Net_GarageLane_RO_ComponentLookup,
        m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_SlaveLaneData = this.__TypeHandle.__Game_Net_SlaveLane_RO_ComponentLookup,
        m_ResidentData = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentLookup,
        m_DivertData = this.__TypeHandle.__Game_Creatures_Divert_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_HouseholdMemberData = this.__TypeHandle.__Game_Citizens_HouseholdMember_RO_ComponentLookup,
        m_HouseholdData = this.__TypeHandle.__Game_Citizens_Household_RO_ComponentLookup,
        m_WorkerData = this.__TypeHandle.__Game_Citizens_Worker_RO_ComponentLookup,
        m_TravelPurposeData = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_MovingAwayData = this.__TypeHandle.__Game_Agents_MovingAway_RO_ComponentLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RO_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RW_ComponentLookup,
        m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_City = this.m_CitySystem.City,
        m_TimeOfDay = this.m_TimeSystem.normalizedTime,
        m_MovingToParkedCarRemoveTypes = this.m_MovingToParkedCarRemoveTypes,
        m_MovingToParkedCarAddTypes = this.m_MovingToParkedCarAddTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_MoneyTransferQueue = this.m_Actions.m_MoneyTransferQueue.AsParallelWriter(),
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps1).AsParallelWriter(),
        m_FeeQueue = this.m_ServiceFeeSystem.GetFeeQueue(out deps2).AsParallelWriter()
      }.ScheduleParallel<PersonalCarAISystem.PersonalCarTickJob>(this.m_VehicleQuery, JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ServiceFeeSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Actions.m_Dependency = jobHandle;
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
    public PersonalCarAISystem()
    {
    }

    public struct MoneyTransfer
    {
      public Entity m_Payer;
      public Entity m_Recipient;
      public int m_Amount;
    }

    [BurstCompile]
    private struct PersonalCarTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<Game.Vehicles.PersonalCar> m_PersonalCarType;
      public ComponentTypeHandle<Car> m_CarType;
      public ComponentTypeHandle<CarCurrentLane> m_CurrentLaneType;
      public BufferTypeHandle<CarNavigationLane> m_CarNavigationLaneType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<CreatureData> m_PrefabCreatureData;
      [ReadOnly]
      public ComponentLookup<HumanData> m_PrefabHumanData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<GarageLane> m_GarageLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> m_ConnectionLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<SlaveLane> m_SlaveLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> m_ResidentData;
      [ReadOnly]
      public ComponentLookup<Divert> m_DivertData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> m_HouseholdMemberData;
      [ReadOnly]
      public ComponentLookup<Household> m_HouseholdData;
      [ReadOnly]
      public ComponentLookup<Worker> m_WorkerData;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeData;
      [ReadOnly]
      public ComponentLookup<MovingAway> m_MovingAwayData;
      [ReadOnly]
      public BufferLookup<Passenger> m_Passengers;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Target> m_TargetData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public float m_TimeOfDay;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedCarAddTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public NativeQueue<PersonalCarAISystem.MoneyTransfer>.ParallelWriter m_MoneyTransferQueue;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;
      public NativeQueue<ServiceFeeSystem.FeeEvent>.ParallelWriter m_FeeQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray3 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.PersonalCar> nativeArray4 = chunk.GetNativeArray<Game.Vehicles.PersonalCar>(ref this.m_PersonalCarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Car> nativeArray5 = chunk.GetNativeArray<Car>(ref this.m_CarType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor1 = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CarNavigationLane> bufferAccessor2 = chunk.GetBufferAccessor<CarNavigationLane>(ref this.m_CarNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          Game.Vehicles.PersonalCar personalCar = nativeArray4[index];
          Car car = nativeArray5[index];
          CarCurrentLane currentLane = nativeArray3[index];
          DynamicBuffer<CarNavigationLane> navigationLanes = bufferAccessor2[index];
          // ISSUE: reference to a compiler-generated field
          Target target = this.m_TargetData[entity];
          // ISSUE: reference to a compiler-generated field
          PathOwner pathOwner = this.m_PathOwnerData[entity];
          DynamicBuffer<LayoutElement> layout = new DynamicBuffer<LayoutElement>();
          if (bufferAccessor1.Length != 0)
            layout = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, prefabRef, layout, navigationLanes, ref personalCar, ref car, ref currentLane, ref pathOwner, ref target);
          // ISSUE: reference to a compiler-generated field
          this.m_TargetData[entity] = target;
          // ISSUE: reference to a compiler-generated field
          this.m_PathOwnerData[entity] = pathOwner;
          nativeArray4[index] = personalCar;
          nativeArray5[index] = car;
          nativeArray3[index] = currentLane;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        ref Game.Vehicles.PersonalCar personalCar,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(entity.Index);
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(entity, ref random, ref personalCar, ref car, ref currentLane, ref pathOwner);
        }
        // ISSUE: reference to a compiler-generated field
        if ((personalCar.m_State & (PersonalCarFlags.Transporting | PersonalCarFlags.Boarding | PersonalCarFlags.Disembarking)) == (PersonalCarFlags) 0 && !this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.RemovePath(entity, ref pathOwner);
          if ((personalCar.m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.StopDisembarking(entity, layout, ref personalCar, ref pathOwner))
              return;
            // ISSUE: reference to a compiler-generated method
            this.ParkCar(jobIndex, entity, layout, true, ref personalCar, ref currentLane);
            return;
          }
          if ((personalCar.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (this.StartDisembarking(jobIndex, entity, layout, ref personalCar, ref currentLane))
              return;
            // ISSUE: reference to a compiler-generated method
            this.ParkCar(jobIndex, entity, layout, true, ref personalCar, ref currentLane);
            return;
          }
          if ((personalCar.m_State & PersonalCarFlags.Boarding) != (PersonalCarFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            if (!this.StopBoarding(entity, layout, navigationLanes, ref personalCar, ref currentLane, ref pathOwner, ref target))
              return;
            if ((personalCar.m_State & PersonalCarFlags.Transporting) == (PersonalCarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, entity, layout, false, ref personalCar, ref currentLane);
              return;
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.ParkCar(jobIndex, entity, layout, false, ref personalCar, ref currentLane);
            return;
          }
        }
        else
        {
          if (VehicleUtils.PathEndReached(currentLane))
          {
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity, layout);
            return;
          }
          if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner) || (personalCar.m_State & (PersonalCarFlags.Boarding | PersonalCarFlags.Disembarking)) != (PersonalCarFlags) 0)
          {
            if ((personalCar.m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.StopDisembarking(entity, layout, ref personalCar, ref pathOwner))
                return;
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, entity, layout, false, ref personalCar, ref currentLane);
              return;
            }
            if ((personalCar.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (this.StartDisembarking(jobIndex, entity, layout, ref personalCar, ref currentLane))
                return;
              // ISSUE: reference to a compiler-generated method
              this.ParkCar(jobIndex, entity, layout, false, ref personalCar, ref currentLane);
              return;
            }
            if ((personalCar.m_State & PersonalCarFlags.Boarding) != (PersonalCarFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.StopBoarding(entity, layout, navigationLanes, ref personalCar, ref currentLane, ref pathOwner, ref target))
                return;
              if ((personalCar.m_State & PersonalCarFlags.Transporting) == (PersonalCarFlags) 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.ParkCar(jobIndex, entity, layout, false, ref personalCar, ref currentLane);
                return;
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (this.StartBoarding(entity, ref personalCar, ref car, ref target))
                return;
              // ISSUE: reference to a compiler-generated field
              VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity, layout);
              return;
            }
          }
          else if (VehicleUtils.WaypointReached(currentLane))
          {
            currentLane.m_LaneFlags &= ~Game.Vehicles.CarLaneFlags.Waypoint;
            pathOwner.m_State &= ~PathFlags.Failed;
            pathOwner.m_State |= PathFlags.Obsolete;
          }
        }
        if ((personalCar.m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
          return;
        if (VehicleUtils.RequireNewPath(pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.FindNewPath(entity, prefabRef, layout, ref personalCar, ref currentLane, ref pathOwner, ref target);
        }
        else
        {
          if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) != (PathFlags) 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.CheckParkingSpace(entity, ref random, ref currentLane, ref pathOwner, navigationLanes);
        }
      }

      private void CheckParkingSpace(
        Entity entity,
        ref Random random,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        DynamicBuffer<CarNavigationLane> navigationLanes)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[entity];
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
        if (VehicleUtils.ValidateParkingSpace(entity, ref random, ref currentLane, ref pathOwner, navigationLanes, pathElement1, ref this.m_ParkedCarData, ref blockerData, ref this.m_CurveData, ref this.m_UnspawnedData, ref this.m_ParkingLaneData, ref this.m_GarageLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabParkingLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false, true, false) != Entity.Null)
          return;
        int max = math.min(40000, pathElement1.Length - pathOwner.m_ElementIndex);
        if (max <= 0 || navigationLanes.Length <= 0)
          return;
        int num = random.NextInt(max) * (random.NextInt(max) + 1) / max;
        PathElement pathElement2 = pathElement1[pathOwner.m_ElementIndex + num];
        // ISSUE: reference to a compiler-generated field
        if (this.m_ParkingLaneData.HasComponent(pathElement2.m_Target))
        {
          float minT;
          if (num == 0)
          {
            CarNavigationLane navigationLane = navigationLanes[navigationLanes.Length - 1];
            minT = (navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Reserved) == (Game.Vehicles.CarLaneFlags) 0 ? navigationLane.m_CurvePosition.x : navigationLane.m_CurvePosition.y;
          }
          else
            minT = pathElement1[pathOwner.m_ElementIndex + num - 1].m_TargetDelta.x;
          float offset;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float y = VehicleUtils.GetParkingSize(entity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, out offset).y;
          float x = pathElement2.m_TargetDelta.x;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (VehicleUtils.FindFreeParkingSpace(ref random, pathElement2.m_Target, minT, y, offset, ref x, ref this.m_ParkedCarData, ref this.m_CurveData, ref this.m_UnspawnedData, ref this.m_ParkingLaneData, ref this.m_PrefabRefData, ref this.m_PrefabParkingLaneData, ref this.m_PrefabObjectGeometryData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false, true))
            return;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_GarageLaneData.HasComponent(pathElement2.m_Target))
            return;
          // ISSUE: reference to a compiler-generated field
          GarageLane garageLane = this.m_GarageLaneData[pathElement2.m_Target];
          // ISSUE: reference to a compiler-generated field
          Game.Net.ConnectionLane connectionLane = this.m_ConnectionLaneData[pathElement2.m_Target];
          if ((int) garageLane.m_VehicleCount < (int) garageLane.m_VehicleCapacity && (connectionLane.m_Flags & ConnectionLaneFlags.Disabled) == (ConnectionLaneFlags) 0)
            return;
        }
        for (int index = 0; index < num; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (VehicleUtils.IsParkingLane(pathElement1[pathOwner.m_ElementIndex + index].m_Target, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData))
            return;
        }
        pathOwner.m_State |= PathFlags.Obsolete;
      }

      private void ResetPath(
        Entity entity,
        ref Random random,
        ref Game.Vehicles.PersonalCar personalCar,
        ref Car car,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        if ((personalCar.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0)
          car.m_Flags |= CarFlags.StayOnRoad;
        else
          car.m_Flags &= ~CarFlags.StayOnRoad;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[entity];
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
        VehicleUtils.ResetParkingLaneStatus(entity, ref currentLane, ref pathOwner, pathElement, ref this.m_EntityLookup, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_CarLaneData, ref this.m_ConnectionLaneData, ref this.m_SpawnLocationData, ref this.m_PrefabRefData, ref this.m_PrefabSpawnLocationData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetParkingCurvePos(entity, ref random, currentLane, pathOwner, pathElement, ref this.m_ParkedCarData, ref this.m_UnspawnedData, ref this.m_CurveData, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData, ref this.m_PrefabParkingLaneData, ref this.m_LaneObjects, ref this.m_LaneOverlaps, false);
      }

      private void RemovePath(Entity entity, ref PathOwner pathOwner)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PathElements[entity].Clear();
        pathOwner.m_ElementIndex = 0;
      }

      private bool StartBoarding(
        Entity vehicleEntity,
        ref Game.Vehicles.PersonalCar personalCar,
        ref Car car,
        ref Target target)
      {
        if ((personalCar.m_State & PersonalCarFlags.DummyTraffic) != (PersonalCarFlags) 0)
          return false;
        personalCar.m_State |= PersonalCarFlags.Boarding;
        return true;
      }

      private bool HasPassengers(Entity vehicleEntity, DynamicBuffer<LayoutElement> layout)
      {
        if (layout.IsCreated && layout.Length != 0)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Passengers[layout[index].m_Vehicle].Length != 0)
              return true;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Passengers[vehicleEntity].Length != 0)
            return true;
        }
        return false;
      }

      private Entity FindLeader(Entity vehicleEntity, DynamicBuffer<LayoutElement> layout)
      {
        if (!layout.IsCreated || layout.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return this.FindLeader(this.m_Passengers[vehicleEntity]);
        }
        for (int index = 0; index < layout.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          Entity leader = this.FindLeader(this.m_Passengers[layout[index].m_Vehicle]);
          if (leader != Entity.Null)
            return leader;
        }
        return Entity.Null;
      }

      private Entity FindLeader(DynamicBuffer<Passenger> passengers)
      {
        for (int index = 0; index < passengers.Length; ++index)
        {
          Entity passenger = passengers[index].m_Passenger;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentVehicleData.HasComponent(passenger) && (this.m_CurrentVehicleData[passenger].m_Flags & CreatureVehicleFlags.Leader) != (CreatureVehicleFlags) 0)
            return passenger;
        }
        return Entity.Null;
      }

      private bool PassengersReady(
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout,
        out Entity leader)
      {
        leader = Entity.Null;
        if (!layout.IsCreated || layout.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return this.PassengersReady(this.m_Passengers[vehicleEntity], ref leader);
        }
        for (int index = 0; index < layout.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.PassengersReady(this.m_Passengers[layout[index].m_Vehicle], ref leader))
            return false;
        }
        return true;
      }

      private bool PassengersReady(DynamicBuffer<Passenger> passengers, ref Entity leader)
      {
        for (int index = 0; index < passengers.Length; ++index)
        {
          Entity passenger = passengers[index].m_Passenger;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentVehicleData.HasComponent(passenger))
          {
            // ISSUE: reference to a compiler-generated field
            CurrentVehicle currentVehicle = this.m_CurrentVehicleData[passenger];
            if ((currentVehicle.m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
              return false;
            if ((currentVehicle.m_Flags & CreatureVehicleFlags.Leader) != (CreatureVehicleFlags) 0)
              leader = passenger;
          }
        }
        return true;
      }

      private bool StopBoarding(
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout,
        DynamicBuffer<CarNavigationLane> navigationLanes,
        ref Game.Vehicles.PersonalCar personalCar,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        Entity leader;
        // ISSUE: reference to a compiler-generated method
        if (!this.PassengersReady(vehicleEntity, layout, out leader))
          return false;
        if (leader == Entity.Null)
        {
          personalCar.m_State &= ~PersonalCarFlags.Boarding;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement1 = this.m_PathElements[vehicleEntity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement2 = this.m_PathElements[leader];
        // ISSUE: reference to a compiler-generated field
        PathOwner pathOwner1 = this.m_PathOwnerData[leader];
        // ISSUE: reference to a compiler-generated field
        Target target1 = this.m_TargetData[leader];
        PathOwner sourceOwner = pathOwner1;
        DynamicBuffer<PathElement> targetElements = pathElement1;
        PathUtils.CopyPath(pathElement2, sourceOwner, 0, targetElements);
        pathOwner.m_ElementIndex = 0;
        pathOwner.m_State |= PathFlags.Updated;
        personalCar.m_State &= ~PersonalCarFlags.Boarding;
        personalCar.m_State |= PersonalCarFlags.Transporting;
        bool flag = false;
        target.m_Target = target1.m_Target;
        // ISSUE: reference to a compiler-generated field
        if (this.m_HouseholdMemberData.HasComponent(leader))
        {
          // ISSUE: reference to a compiler-generated field
          Entity household = this.m_HouseholdMemberData[leader].m_Household;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenterData.HasComponent(household))
          {
            // ISSUE: reference to a compiler-generated field
            Entity property = this.m_PropertyRenterData[household].m_Property;
            flag |= property == target.m_Target;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_DivertData.HasComponent(leader))
        {
          // ISSUE: reference to a compiler-generated field
          Divert divert = this.m_DivertData[leader];
          flag &= divert.m_Purpose == Game.Citizens.Purpose.None;
        }
        if (flag)
          personalCar.m_State |= PersonalCarFlags.HomeTarget;
        else
          personalCar.m_State &= ~PersonalCarFlags.HomeTarget;
        VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
        return true;
      }

      private bool StartDisembarking(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.PersonalCar personalCar,
        ref CarCurrentLane currentLane)
      {
        // ISSUE: reference to a compiler-generated method
        if (!this.HasPassengers(vehicleEntity, layout))
          return false;
        personalCar.m_State &= ~PersonalCarFlags.Transporting;
        personalCar.m_State |= PersonalCarFlags.Disembarking;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ParkingLaneData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          Game.Net.ParkingLane parkingLane = this.m_ParkingLaneData[currentLane.m_Lane];
          if (parkingLane.m_ParkingFee > (ushort) 0)
          {
            // ISSUE: reference to a compiler-generated method
            Entity leader = this.FindLeader(vehicleEntity, layout);
            // ISSUE: reference to a compiler-generated field
            if (this.m_ResidentData.HasComponent(leader))
            {
              // ISSUE: reference to a compiler-generated field
              Game.Creatures.Resident resident = this.m_ResidentData[leader];
              // ISSUE: reference to a compiler-generated field
              if (this.m_HouseholdMemberData.HasComponent(resident.m_Citizen))
              {
                // ISSUE: reference to a compiler-generated field
                HouseholdMember householdMember = this.m_HouseholdMemberData[resident.m_Citizen];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_MoneyTransferQueue.Enqueue(new PersonalCarAISystem.MoneyTransfer()
                {
                  m_Payer = householdMember.m_Household,
                  m_Recipient = this.m_City,
                  m_Amount = (int) parkingLane.m_ParkingFee
                });
                // ISSUE: reference to a compiler-generated field
                this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
                {
                  m_Statistic = StatisticType.Income,
                  m_Change = (float) parkingLane.m_ParkingFee,
                  m_Parameter = 9
                });
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_FeeQueue.Enqueue(new ServiceFeeSystem.FeeEvent()
                {
                  m_Amount = 1f,
                  m_Cost = (float) parkingLane.m_ParkingFee,
                  m_Resource = PlayerResource.Parking,
                  m_Outside = false
                });
              }
            }
          }
        }
        return true;
      }

      private bool StopDisembarking(
        Entity vehicleEntity,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.PersonalCar personalCar,
        ref PathOwner pathOwner)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.HasPassengers(vehicleEntity, layout))
          return false;
        // ISSUE: reference to a compiler-generated field
        this.m_PathElements[vehicleEntity].Clear();
        pathOwner.m_ElementIndex = 0;
        personalCar.m_State &= ~PersonalCarFlags.Disembarking;
        return true;
      }

      private void ParkCar(
        int jobIndex,
        Entity entity,
        DynamicBuffer<LayoutElement> layout,
        bool resetLocation,
        ref Game.Vehicles.PersonalCar personalCar,
        ref CarCurrentLane currentLane)
      {
        personalCar.m_State &= ~(PersonalCarFlags.Transporting | PersonalCarFlags.Boarding | PersonalCarFlags.Disembarking);
        if (layout.IsCreated)
        {
          for (int index = 0; index < layout.Length; ++index)
          {
            Entity vehicle = layout[index].m_Vehicle;
            if (!(vehicle == entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, vehicle);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<LayoutElement>(jobIndex, entity);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_MovingToParkedCarRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_MovingToParkedCarAddTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(jobIndex, entity, new ParkedCar(currentLane.m_Lane, currentLane.m_CurvePosition.x));
        if (resetLocation)
        {
          Entity property = Entity.Null;
          HouseholdMember componentData1;
          PropertyRenter componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_HouseholdMemberData.TryGetComponent(personalCar.m_Keeper, out componentData1) && this.m_PropertyRenterData.TryGetComponent(componentData1.m_Household, out componentData2) && ((this.m_HouseholdData[componentData1.m_Household].m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None ? 0 : (!this.m_MovingAwayData.HasComponent(componentData1.m_Household) ? 1 : 0)) != 0)
            property = componentData2.m_Property;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(currentLane.m_ChangeLane, property));
        }
        else
        {
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
      }

      private void FindNewPath(
        Entity entity,
        PrefabRef prefabRef,
        DynamicBuffer<LayoutElement> layout,
        ref Game.Vehicles.PersonalCar personalCar,
        ref CarCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        CarData carData = this.m_PrefabCarData[prefabRef.m_Prefab];
        pathOwner.m_State &= ~(PathFlags.AddDestination | PathFlags.Divert);
        bool flag = false;
        PathfindParameters parameters;
        SetupQueueTarget origin;
        SetupQueueTarget destination;
        if ((personalCar.m_State & PersonalCarFlags.Transporting) != (PersonalCarFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          parameters = new PathfindParameters()
          {
            m_MaxSpeed = new float2(carData.m_MaxSpeed, 277.777771f),
            m_WalkSpeed = (float2) 5.555556f,
            m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
            m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Parking,
            m_ParkingTarget = VehicleUtils.GetParkingSource(entity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
            m_ParkingDelta = currentLane.m_CurvePosition.z,
            m_ParkingSize = VehicleUtils.GetParkingSize(entity, ref this.m_PrefabRefData, ref this.m_PrefabObjectGeometryData),
            m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData),
            m_SecondaryIgnoredRules = VehicleUtils.GetIgnoredPathfindRulesTaxiDefaults()
          };
          SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
          setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
          setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.Parking;
          setupQueueTarget.m_RoadTypes = RoadTypes.Car;
          origin = setupQueueTarget;
          setupQueueTarget = new SetupQueueTarget();
          setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
          setupQueueTarget.m_Methods = PathMethod.Pedestrian;
          setupQueueTarget.m_Entity = target.m_Target;
          setupQueueTarget.m_RandomCost = 30f;
          destination = setupQueueTarget;
          // ISSUE: reference to a compiler-generated method
          Entity leader = this.FindLeader(entity, layout);
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResidentData.HasComponent(leader))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef1 = this.m_PrefabRefData[leader];
            // ISSUE: reference to a compiler-generated field
            Game.Creatures.Resident resident = this.m_ResidentData[leader];
            // ISSUE: reference to a compiler-generated field
            CreatureData creatureData = this.m_PrefabCreatureData[prefabRef1.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            HumanData humanData = this.m_PrefabHumanData[prefabRef1.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            parameters.m_MaxCost = CitizenBehaviorSystem.kMaxPathfindCost;
            parameters.m_WalkSpeed = (float2) humanData.m_WalkSpeed;
            // ISSUE: reference to a compiler-generated field
            parameters.m_Methods |= RouteUtils.GetTaxiMethods(resident) | RouteUtils.GetPublicTransportMethods(resident, this.m_TimeOfDay);
            destination.m_ActivityMask = creatureData.m_SupportedActivities;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdMemberData.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              Entity household = this.m_HouseholdMemberData[resident.m_Citizen].m_Household;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PropertyRenterData.HasComponent(household))
              {
                // ISSUE: reference to a compiler-generated field
                Entity property = this.m_PropertyRenterData[household].m_Property;
                parameters.m_Authorization1 = property;
                flag |= property == target.m_Target;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_WorkerData.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              Worker worker = this.m_WorkerData[resident.m_Citizen];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              parameters.m_Authorization2 = !this.m_PropertyRenterData.HasComponent(worker.m_Workplace) ? worker.m_Workplace : this.m_PropertyRenterData[worker.m_Workplace].m_Property;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_CitizenData.HasComponent(resident.m_Citizen))
            {
              // ISSUE: reference to a compiler-generated field
              Citizen citizen = this.m_CitizenData[resident.m_Citizen];
              // ISSUE: reference to a compiler-generated field
              Entity household1 = this.m_HouseholdMemberData[resident.m_Citizen].m_Household;
              // ISSUE: reference to a compiler-generated field
              Household household2 = this.m_HouseholdData[household1];
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<HouseholdCitizen> householdCitizen = this.m_HouseholdCitizens[household1];
              parameters.m_Weights = CitizenUtils.GetPathfindWeights(citizen, household2, householdCitizen.Length);
            }
            TravelPurpose componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TravelPurposeData.TryGetComponent(resident.m_Citizen, out componentData))
            {
              switch (componentData.m_Purpose)
              {
                case Game.Citizens.Purpose.MovingAway:
                  // ISSUE: reference to a compiler-generated field
                  parameters.m_MaxCost = CitizenBehaviorSystem.kMaxMovingAwayCost;
                  break;
                case Game.Citizens.Purpose.EmergencyShelter:
                  parameters.m_Weights = new PathfindWeights(1f, 0.2f, 0.0f, 0.1f);
                  break;
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_DivertData.HasComponent(leader))
            {
              // ISSUE: reference to a compiler-generated field
              Divert divert = this.m_DivertData[leader];
              CreatureUtils.DivertDestination(ref destination, ref pathOwner, divert);
              flag &= divert.m_Purpose == Game.Citizens.Purpose.None;
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          parameters = new PathfindParameters()
          {
            m_MaxSpeed = (float2) carData.m_MaxSpeed,
            m_WalkSpeed = (float2) 5.555556f,
            m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
            m_Methods = PathMethod.Road,
            m_ParkingTarget = VehicleUtils.GetParkingSource(entity, currentLane, ref this.m_ParkingLaneData, ref this.m_ConnectionLaneData),
            m_ParkingDelta = currentLane.m_CurvePosition.z,
            m_IgnoredRules = VehicleUtils.GetIgnoredPathfindRules(carData)
          };
          origin = new SetupQueueTarget()
          {
            m_Type = SetupTargetType.CurrentLocation,
            m_Methods = PathMethod.Road | PathMethod.Parking,
            m_RoadTypes = RoadTypes.Car
          };
          destination = new SetupQueueTarget()
          {
            m_Type = SetupTargetType.CurrentLocation,
            m_Methods = PathMethod.Road,
            m_RoadTypes = RoadTypes.Car,
            m_Entity = target.m_Target
          };
        }
        if (flag)
          personalCar.m_State |= PersonalCarFlags.HomeTarget;
        else
          personalCar.m_State &= ~PersonalCarFlags.HomeTarget;
        SetupQueueItem setupQueueItem = new SetupQueueItem(entity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
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
    private struct TransferMoneyJob : IJob
    {
      public BufferLookup<Resources> m_Resources;
      public NativeQueue<PersonalCarAISystem.MoneyTransfer> m_MoneyTransferQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PersonalCarAISystem.MoneyTransfer moneyTransfer;
        // ISSUE: reference to a compiler-generated field
        while (this.m_MoneyTransferQueue.TryDequeue(out moneyTransfer))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Resources.HasBuffer(moneyTransfer.m_Payer) && this.m_Resources.HasBuffer(moneyTransfer.m_Recipient))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Resources> resource1 = this.m_Resources[moneyTransfer.m_Payer];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Resources> resource2 = this.m_Resources[moneyTransfer.m_Recipient];
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Money, -moneyTransfer.m_Amount, resource1);
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Money, moneyTransfer.m_Amount, resource2);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle;
      public BufferTypeHandle<CarNavigationLane> __Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CreatureData> __Game_Prefabs_CreatureData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanData> __Game_Prefabs_HumanData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarageLane> __Game_Net_GarageLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ConnectionLane> __Game_Net_ConnectionLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SlaveLane> __Game_Net_SlaveLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Creatures.Resident> __Game_Creatures_Resident_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Divert> __Game_Creatures_Divert_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Household> __Game_Citizens_Household_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Worker> __Game_Citizens_Worker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingAway> __Game_Agents_MovingAway_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      public ComponentLookup<Target> __Game_Common_Target_RW_ComponentLookup;
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.PersonalCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Car>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<CarNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CreatureData_RO_ComponentLookup = state.GetComponentLookup<CreatureData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HumanData_RO_ComponentLookup = state.GetComponentLookup<HumanData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_GarageLane_RO_ComponentLookup = state.GetComponentLookup<GarageLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ConnectionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SlaveLane_RO_ComponentLookup = state.GetComponentLookup<SlaveLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentLookup = state.GetComponentLookup<Game.Creatures.Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Divert_RO_ComponentLookup = state.GetComponentLookup<Divert>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RO_ComponentLookup = state.GetComponentLookup<HouseholdMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RO_ComponentLookup = state.GetComponentLookup<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Worker_RO_ComponentLookup = state.GetComponentLookup<Worker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_MovingAway_RO_ComponentLookup = state.GetComponentLookup<MovingAway>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RO_BufferLookup = state.GetBufferLookup<Passenger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RO_BufferLookup = state.GetBufferLookup<LaneObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentLookup = state.GetComponentLookup<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentLookup = state.GetComponentLookup<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
      }
    }
  }
}
