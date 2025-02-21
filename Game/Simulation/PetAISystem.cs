// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PetAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Creatures;
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PetAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_CreatureQuery;
    private EntityArchetype m_ResetTripArchetype;
    private ComponentTypeSet m_CurrentLaneTypes;
    private PetAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 5;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatureQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Creatures.Pet>(), ComponentType.ReadOnly<Animal>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Stumbling>());
      // ISSUE: reference to a compiler-generated field
      this.m_ResetTripArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<ResetTrip>());
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentLaneTypes = new ComponentTypeSet(new ComponentType[6]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<AnimalNavigation>(),
        ComponentType.ReadWrite<AnimalCurrentLane>(),
        ComponentType.ReadWrite<Blocker>()
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatureQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeQueue<PetAISystem.Boarding> nativeQueue = new NativeQueue<PetAISystem.Boarding>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Pet_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PetAISystem.PetTickJob jobData = new PetAISystem.PetTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_GroupCreatureType = this.__TypeHandle.__Game_Creatures_GroupCreature_RO_BufferTypeHandle,
        m_AnimalType = this.__TypeHandle.__Game_Creatures_Animal_RW_ComponentTypeHandle,
        m_PetType = this.__TypeHandle.__Game_Creatures_Pet_RW_ComponentTypeHandle,
        m_CreatureType = this.__TypeHandle.__Game_Creatures_Creature_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_OnFireData = this.__TypeHandle.__Game_Events_OnFire_RO_ComponentLookup,
        m_PersonalCarData = this.__TypeHandle.__Game_Vehicles_PersonalCar_RO_ComponentLookup,
        m_TaxiData = this.__TypeHandle.__Game_Vehicles_Taxi_RO_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RO_ComponentLookup,
        m_PoliceCarData = this.__TypeHandle.__Game_Vehicles_PoliceCar_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PropertyRenterData = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_PathOwnerData = this.__TypeHandle.__Game_Pathfind_PathOwner_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabActivityLocationElements = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_LefthandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_ResetTripArchetype = this.m_ResetTripArchetype,
        m_BoardingQueue = nativeQueue.AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new PetAISystem.BoardingJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_Creatures = this.__TypeHandle.__Game_Creatures_Creature_RW_ComponentLookup,
        m_Passengers = this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup,
        m_CurrentLaneTypes = this.m_CurrentLaneTypes,
        m_BoardingQueue = nativeQueue,
        m_SearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(false, out dependencies),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      }.Schedule<PetAISystem.BoardingJob>(JobHandle.CombineDependencies(jobData.ScheduleParallel<PetAISystem.PetTickJob>(this.m_CreatureQuery, this.Dependency), dependencies));
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeWriter(jobHandle);
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
    public PetAISystem()
    {
    }

    private struct Boarding
    {
      public Entity m_Passenger;
      public Entity m_Vehicle;
      public AnimalCurrentLane m_CurrentLane;
      public CreatureVehicleFlags m_Flags;
      public float3 m_Position;
      public PetAISystem.BoardingType m_Type;

      public static PetAISystem.Boarding ExitVehicle(
        Entity passenger,
        Entity vehicle,
        AnimalCurrentLane newCurrentLane,
        float3 position)
      {
        // ISSUE: object of a compiler-generated type is created
        return new PetAISystem.Boarding()
        {
          m_Passenger = passenger,
          m_Vehicle = vehicle,
          m_CurrentLane = newCurrentLane,
          m_Position = position,
          m_Type = PetAISystem.BoardingType.Exit
        };
      }

      public static PetAISystem.Boarding TryEnterVehicle(
        Entity passenger,
        Entity vehicle,
        CreatureVehicleFlags flags)
      {
        // ISSUE: object of a compiler-generated type is created
        return new PetAISystem.Boarding()
        {
          m_Passenger = passenger,
          m_Vehicle = vehicle,
          m_Flags = flags,
          m_Type = PetAISystem.BoardingType.TryEnter
        };
      }

      public static PetAISystem.Boarding FinishEnterVehicle(
        Entity passenger,
        Entity vehicle,
        AnimalCurrentLane oldCurrentLane)
      {
        // ISSUE: object of a compiler-generated type is created
        return new PetAISystem.Boarding()
        {
          m_Passenger = passenger,
          m_Vehicle = vehicle,
          m_CurrentLane = oldCurrentLane,
          m_Type = PetAISystem.BoardingType.FinishEnter
        };
      }

      public static PetAISystem.Boarding CancelEnterVehicle(Entity passenger, Entity vehicle)
      {
        // ISSUE: object of a compiler-generated type is created
        return new PetAISystem.Boarding()
        {
          m_Passenger = passenger,
          m_Vehicle = vehicle,
          m_Type = PetAISystem.BoardingType.CancelEnter
        };
      }
    }

    private enum BoardingType
    {
      Exit,
      TryEnter,
      FinishEnter,
      CancelEnter,
    }

    [BurstCompile]
    private struct PetTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> m_UnspawnedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<GroupCreature> m_GroupCreatureType;
      public ComponentTypeHandle<Animal> m_AnimalType;
      public ComponentTypeHandle<Game.Creatures.Pet> m_PetType;
      public ComponentTypeHandle<Creature> m_CreatureType;
      public ComponentTypeHandle<AnimalCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<OnFire> m_OnFireData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> m_PersonalCarData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> m_TaxiData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PoliceCar> m_PoliceCarData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenterData;
      [ReadOnly]
      public ComponentLookup<PathOwner> m_PathOwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_PrefabActivityLocationElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public bool m_LefthandTraffic;
      [ReadOnly]
      public EntityArchetype m_ResetTripArchetype;
      public NativeQueue<PetAISystem.Boarding>.ParallelWriter m_BoardingQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

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
        NativeArray<Game.Creatures.Pet> nativeArray3 = chunk.GetNativeArray<Game.Creatures.Pet>(ref this.m_PetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Creature> nativeArray4 = chunk.GetNativeArray<Creature>(ref this.m_CreatureType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray5 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroupMember> nativeArray6 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray7 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalCurrentLane> nativeArray8 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (nativeArray7.Length != 0)
        {
          if (nativeArray6.Length != 0)
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray2[index];
              Game.Creatures.Pet pet = nativeArray3[index];
              Creature creature = nativeArray4[index];
              CurrentVehicle currentVehicle = nativeArray7[index];
              Target target = nativeArray5[index];
              GroupMember groupMember = nativeArray6[index];
              AnimalCurrentLane currentLane;
              CollectionUtils.TryGet<AnimalCurrentLane>(nativeArray8, index, out currentLane);
              // ISSUE: reference to a compiler-generated method
              this.TickGroupMemberInVehicle(ref random, unfilteredChunkIndex, entity, prefabRef, groupMember, currentVehicle, nativeArray8.Length != 0, ref pet, ref currentLane, ref target);
              // ISSUE: reference to a compiler-generated method
              this.TickQueue(ref creature, ref currentLane);
              nativeArray3[index] = pet;
              nativeArray4[index] = creature;
              nativeArray5[index] = target;
              CollectionUtils.TrySet<AnimalCurrentLane>(nativeArray8, index, currentLane);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<GroupCreature> bufferAccessor = chunk.GetBufferAccessor<GroupCreature>(ref this.m_GroupCreatureType);
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray2[index];
              Game.Creatures.Pet pet = nativeArray3[index];
              Creature creature = nativeArray4[index];
              CurrentVehicle currentVehicle = nativeArray7[index];
              Target target = nativeArray5[index];
              AnimalCurrentLane currentLane;
              CollectionUtils.TryGet<AnimalCurrentLane>(nativeArray8, index, out currentLane);
              DynamicBuffer<GroupCreature> groupCreatures;
              CollectionUtils.TryGet<GroupCreature>(bufferAccessor, index, out groupCreatures);
              // ISSUE: reference to a compiler-generated method
              this.TickInVehicle(ref random, unfilteredChunkIndex, entity, prefabRef, currentVehicle, nativeArray8.Length != 0, ref pet, ref currentLane, ref target, groupCreatures);
              // ISSUE: reference to a compiler-generated method
              this.TickQueue(ref creature, ref currentLane);
              nativeArray3[index] = pet;
              nativeArray4[index] = creature;
              nativeArray5[index] = target;
              CollectionUtils.TrySet<AnimalCurrentLane>(nativeArray8, index, currentLane);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Animal> nativeArray9 = chunk.GetNativeArray<Animal>(ref this.m_AnimalType);
          // ISSUE: reference to a compiler-generated field
          bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
          if (nativeArray6.Length != 0)
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray2[index];
              Animal animal = nativeArray9[index];
              Game.Creatures.Pet pet = nativeArray3[index];
              Creature creature = nativeArray4[index];
              Target target = nativeArray5[index];
              GroupMember groupMember = nativeArray6[index];
              AnimalCurrentLane currentLane;
              CollectionUtils.TryGet<AnimalCurrentLane>(nativeArray8, index, out currentLane);
              // ISSUE: reference to a compiler-generated field
              CreatureUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, animal, isUnspawned, this.m_CommandBuffer);
              // ISSUE: reference to a compiler-generated method
              this.TickGroupMemberWalking(unfilteredChunkIndex, entity, prefabRef, groupMember, ref pet, ref creature, ref currentLane, ref target);
              // ISSUE: reference to a compiler-generated method
              this.TickQueue(ref creature, ref currentLane);
              nativeArray9[index] = animal;
              nativeArray3[index] = pet;
              nativeArray4[index] = creature;
              nativeArray5[index] = target;
              CollectionUtils.TrySet<AnimalCurrentLane>(nativeArray8, index, currentLane);
            }
          }
          else
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Entity entity = nativeArray1[index];
              PrefabRef prefabRef = nativeArray2[index];
              Animal animal = nativeArray9[index];
              Game.Creatures.Pet pet = nativeArray3[index];
              Creature creature = nativeArray4[index];
              Target target = nativeArray5[index];
              AnimalCurrentLane currentLane;
              CollectionUtils.TryGet<AnimalCurrentLane>(nativeArray8, index, out currentLane);
              // ISSUE: reference to a compiler-generated field
              CreatureUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, animal, isUnspawned, this.m_CommandBuffer);
              // ISSUE: reference to a compiler-generated method
              this.TickWalking(unfilteredChunkIndex, entity, prefabRef, ref pet, ref creature, ref currentLane, ref target);
              // ISSUE: reference to a compiler-generated method
              this.TickQueue(ref creature, ref currentLane);
              nativeArray9[index] = animal;
              nativeArray3[index] = pet;
              nativeArray4[index] = creature;
              nativeArray5[index] = target;
              CollectionUtils.TrySet<AnimalCurrentLane>(nativeArray8, index, currentLane);
            }
          }
        }
      }

      private void TickGroupMemberInVehicle(
        ref Random random,
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        GroupMember groupMember,
        CurrentVehicle currentVehicle,
        bool hasCurrentLane,
        ref Game.Creatures.Pet pet,
        ref AnimalCurrentLane currentLane,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(currentVehicle.m_Vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
        }
        else
        {
          Entity entity1 = currentVehicle.m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.HasComponent(currentVehicle.m_Vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            Controller controller = this.m_ControllerData[currentVehicle.m_Vehicle];
            if (controller.m_Controller != Entity.Null)
              entity1 = controller.m_Controller;
          }
          if ((currentVehicle.m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
          {
            if (hasCurrentLane)
            {
              if (CreatureUtils.IsStuck(currentLane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                return;
              }
              // ISSUE: reference to a compiler-generated field
              if (!this.m_CurrentVehicleData.HasComponent(groupMember.m_Leader))
              {
                // ISSUE: reference to a compiler-generated method
                this.CancelEnterVehicle(entity, currentVehicle.m_Vehicle, ref currentLane);
                return;
              }
              Game.Vehicles.PublicTransport componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PublicTransportData.TryGetComponent(entity1, out componentData) && (componentData.m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0 && currentLane.m_Lane == currentVehicle.m_Vehicle)
                currentLane.m_Flags |= CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached;
              if (CreatureUtils.PathEndReached(currentLane))
              {
                // ISSUE: reference to a compiler-generated method
                this.FinishEnterVehicle(entity, currentVehicle.m_Vehicle, ref currentLane);
                hasCurrentLane = false;
              }
            }
            if (hasCurrentLane)
              return;
            currentVehicle.m_Flags |= CreatureVehicleFlags.Ready;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CurrentVehicle>(jobIndex, entity, currentVehicle);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if ((pet.m_Flags & PetFlags.Disembarking) == PetFlags.None && !this.m_CurrentVehicleData.HasComponent(groupMember.m_Leader))
            {
              // ISSUE: reference to a compiler-generated method
              this.GroupLeaderDisembarking(entity, ref pet);
            }
            if ((pet.m_Flags & PetFlags.Disembarking) == PetFlags.None)
              return;
            // ISSUE: reference to a compiler-generated method
            this.ExitVehicle(ref random, entity, entity1, prefabRef, currentVehicle);
          }
        }
      }

      private void TickInVehicle(
        ref Random random,
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        CurrentVehicle currentVehicle,
        bool hasCurrentLane,
        ref Game.Creatures.Pet pet,
        ref AnimalCurrentLane currentLane,
        ref Target target,
        DynamicBuffer<GroupCreature> groupCreatures)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabRefData.HasComponent(currentVehicle.m_Vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
        }
        else
        {
          Entity entity1 = currentVehicle.m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.HasComponent(currentVehicle.m_Vehicle))
          {
            // ISSUE: reference to a compiler-generated field
            Controller controller = this.m_ControllerData[currentVehicle.m_Vehicle];
            if (controller.m_Controller != Entity.Null)
              entity1 = controller.m_Controller;
          }
          if ((currentVehicle.m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
          {
            if (hasCurrentLane)
            {
              if (CreatureUtils.IsStuck(currentLane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                return;
              }
              Game.Vehicles.PublicTransport componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PublicTransportData.TryGetComponent(entity1, out componentData) && (componentData.m_State & PublicTransportFlags.Boarding) == (PublicTransportFlags) 0 && currentLane.m_Lane == currentVehicle.m_Vehicle)
                currentLane.m_Flags |= CreatureLaneFlags.EndOfPath | CreatureLaneFlags.EndReached;
              if (CreatureUtils.PathEndReached(currentLane))
              {
                // ISSUE: reference to a compiler-generated method
                this.FinishEnterVehicle(entity, currentVehicle.m_Vehicle, ref currentLane);
                hasCurrentLane = false;
              }
            }
            // ISSUE: reference to a compiler-generated method
            if (hasCurrentLane || !this.HasEveryoneBoarded(groupCreatures))
              return;
            currentVehicle.m_Flags |= CreatureVehicleFlags.Ready;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CurrentVehicle>(jobIndex, entity, currentVehicle);
          }
          else
          {
            if ((pet.m_Flags & PetFlags.Disembarking) == PetFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DestroyedData.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                if (!this.m_MovingData.HasComponent(entity1))
                  pet.m_Flags |= PetFlags.Disembarking;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_PersonalCarData.HasComponent(entity1))
                {
                  // ISSUE: reference to a compiler-generated field
                  if ((this.m_PersonalCarData[entity1].m_State & PersonalCarFlags.Disembarking) != (PersonalCarFlags) 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CurrentVehicleDisembarking(jobIndex, entity, entity1, ref pet, ref target);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PublicTransportData.HasComponent(entity1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    if ((this.m_PublicTransportData[entity1].m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CurrentVehicleBoarding(jobIndex, entity, entity1, ref pet, ref target);
                    }
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TaxiData.HasComponent(entity1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      if ((this.m_TaxiData[entity1].m_State & TaxiFlags.Disembarking) != (TaxiFlags) 0)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CurrentVehicleDisembarking(jobIndex, entity, entity1, ref pet, ref target);
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_PoliceCarData.HasComponent(entity1) && (this.m_PoliceCarData[entity1].m_State & PoliceCarFlags.Disembarking) != (PoliceCarFlags) 0)
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.CurrentVehicleDisembarking(jobIndex, entity, entity1, ref pet, ref target);
                      }
                    }
                  }
                }
              }
            }
            if ((pet.m_Flags & PetFlags.Disembarking) == PetFlags.None)
              return;
            // ISSUE: reference to a compiler-generated method
            this.ExitVehicle(ref random, entity, entity1, prefabRef, currentVehicle);
          }
        }
      }

      private void TickGroupMemberWalking(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        GroupMember groupMember,
        ref Game.Creatures.Pet pet,
        ref Creature creature,
        ref AnimalCurrentLane currentLane,
        ref Target target)
      {
        if ((pet.m_Flags & PetFlags.Disembarking) != PetFlags.None)
        {
          pet.m_Flags &= ~PetFlags.Disembarking;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(target.m_Target) || CreatureUtils.IsStuck(currentLane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
            return;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CurrentVehicleData.HasComponent(groupMember.m_Leader) || (currentLane.m_Flags & CreatureLaneFlags.EndReached) == (CreatureLaneFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        CurrentVehicle currentVehicle = this.m_CurrentVehicleData[groupMember.m_Leader];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BoardingQueue.Enqueue(PetAISystem.Boarding.TryEnterVehicle(entity, currentVehicle.m_Vehicle, (CreatureVehicleFlags) 0));
      }

      private void TickWalking(
        int jobIndex,
        Entity entity,
        PrefabRef prefabRef,
        ref Game.Creatures.Pet pet,
        ref Creature creature,
        ref AnimalCurrentLane currentLane,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated method
        if (this.CheckTarget(entity, ref currentLane, ref target))
          return;
        if ((pet.m_Flags & PetFlags.Disembarking) != PetFlags.None)
        {
          pet.m_Flags &= ~PetFlags.Disembarking;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabRefData.HasComponent(target.m_Target) || CreatureUtils.IsStuck(currentLane))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
          }
          else
          {
            if (!CreatureUtils.PathEndReached(currentLane))
              return;
            // ISSUE: reference to a compiler-generated method
            this.PathEndReached(jobIndex, entity, ref pet, ref currentLane, ref target);
          }
        }
      }

      private void TickQueue(ref Creature creature, ref AnimalCurrentLane currentLane)
      {
        creature.m_QueueEntity = currentLane.m_QueueEntity;
        creature.m_QueueArea = currentLane.m_QueueArea;
      }

      private void ExitVehicle(
        ref Random random,
        Entity entity,
        Entity controllerVehicle,
        PrefabRef prefabRef,
        CurrentVehicle currentVehicle)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(currentVehicle.m_Vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          Transform vehicleTransform = this.m_TransformData[currentVehicle.m_Vehicle];
          // ISSUE: reference to a compiler-generated field
          float3 position1 = this.m_TransformData[entity].m_Position;
          BufferLookup<SubMeshGroup> subMeshGroupBuffers = new BufferLookup<SubMeshGroup>();
          BufferLookup<CharacterElement> characterElementBuffers = new BufferLookup<CharacterElement>();
          BufferLookup<SubMesh> subMeshBuffers = new BufferLookup<SubMesh>();
          BufferLookup<AnimationClip> animationClipBuffers = new BufferLookup<AnimationClip>();
          BufferLookup<AnimationMotion> animationMotionBuffers = new BufferLookup<AnimationMotion>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 position2 = CreatureUtils.GetVehicleDoorPosition(ref random, ActivityType.Exit, (ActivityCondition) 0, vehicleTransform, position1, false, this.m_LefthandTraffic, prefabRef.m_Prefab, currentVehicle.m_Vehicle, new DynamicBuffer<MeshGroup>(), ref this.m_PublicTransportData, ref this.m_TrainData, ref this.m_ControllerData, ref this.m_PrefabRefData, ref this.m_PrefabCarData, ref this.m_PrefabActivityLocationElements, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, ref animationMotionBuffers, out ActivityMask _, out AnimatedPropID _).m_Position;
          AnimalCurrentLane newCurrentLane = new AnimalCurrentLane();
          // ISSUE: reference to a compiler-generated field
          if (this.m_UnspawnedData.HasComponent(currentVehicle.m_Vehicle))
            newCurrentLane.m_Flags |= CreatureLaneFlags.EmergeUnspawned;
          PathOwner componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathOwnerData.TryGetComponent(controllerVehicle, out componentData) && VehicleUtils.PathfindFailed(componentData))
            newCurrentLane.m_Flags |= CreatureLaneFlags.Stuck | CreatureLaneFlags.EmergeUnspawned;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_BoardingQueue.Enqueue(PetAISystem.Boarding.ExitVehicle(entity, currentVehicle.m_Vehicle, newCurrentLane, position2));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = this.m_TransformData[entity].m_Position;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_BoardingQueue.Enqueue(PetAISystem.Boarding.ExitVehicle(entity, currentVehicle.m_Vehicle, new AnimalCurrentLane(), position));
        }
      }

      private bool HasEveryoneBoarded(DynamicBuffer<GroupCreature> group)
      {
        if (group.IsCreated)
        {
          for (int index = 0; index < group.Length; ++index)
          {
            Entity creature = group[index].m_Creature;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_CurrentVehicleData.HasComponent(creature) || (this.m_CurrentVehicleData[creature].m_Flags & CreatureVehicleFlags.Ready) == (CreatureVehicleFlags) 0)
              return false;
          }
        }
        return true;
      }

      private bool CheckTarget(Entity entity, ref AnimalCurrentLane currentLane, ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_VehicleData.HasComponent(target.m_Target))
        {
          Entity entity1 = target.m_Target;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.HasComponent(target.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            Controller controller = this.m_ControllerData[target.m_Target];
            if (controller.m_Controller != Entity.Null)
              entity1 = controller.m_Controller;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_PublicTransportData.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PublicTransportData[entity1].m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0 && this.m_OwnerData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              Owner owner = this.m_OwnerData[entity1];
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingData.HasComponent(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated method
                this.TryEnterVehicle(entity, target.m_Target, ref currentLane);
                target.m_Target = owner.m_Owner;
                return true;
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PoliceCarData.HasComponent(entity1) && (this.m_PoliceCarData[entity1].m_State & PoliceCarFlags.AtTarget) != (PoliceCarFlags) 0 && this.m_OwnerData.HasComponent(entity1))
            {
              // ISSUE: reference to a compiler-generated field
              Owner owner = this.m_OwnerData[entity1];
              // ISSUE: reference to a compiler-generated field
              if (this.m_BuildingData.HasComponent(owner.m_Owner))
              {
                // ISSUE: reference to a compiler-generated method
                this.TryEnterVehicle(entity, target.m_Target, ref currentLane);
                target.m_Target = owner.m_Owner;
                return true;
              }
            }
          }
        }
        return false;
      }

      private void CurrentVehicleBoarding(
        int jobIndex,
        Entity entity,
        Entity controllerVehicle,
        ref Game.Creatures.Pet pet,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Vehicles.PublicTransport publicTransport = this.m_PublicTransportData[controllerVehicle];
        if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport)) != (PublicTransportFlags) 0 && (publicTransport.m_State & PublicTransportFlags.Returning) == (PublicTransportFlags) 0)
          return;
        pet.m_Flags |= PetFlags.Disembarking;
      }

      private void CurrentVehicleDisembarking(
        int jobIndex,
        Entity entity,
        Entity controllerVehicle,
        ref Game.Creatures.Pet pet,
        ref Target target)
      {
        pet.m_Flags |= PetFlags.Disembarking;
      }

      private void GroupLeaderDisembarking(Entity entity, ref Game.Creatures.Pet pet)
      {
        pet.m_Flags |= PetFlags.Disembarking;
      }

      private bool PathEndReached(
        int jobIndex,
        Entity entity,
        ref Game.Creatures.Pet pet,
        ref AnimalCurrentLane currentLane,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_VehicleData.HasComponent(target.m_Target))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.HasComponent(target.m_Target))
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner = this.m_OwnerData[target.m_Target];
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingData.HasComponent(owner.m_Owner))
            {
              target.m_Target = owner.m_Owner;
              return false;
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
          return true;
        }
        if ((pet.m_Flags & (PetFlags.Arrived | PetFlags.LeaderArrived)) == PetFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
          return true;
        }
        Entity entity1 = target.m_Target;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PropertyRenterData.HasComponent(entity1))
        {
          // ISSUE: reference to a compiler-generated field
          entity1 = this.m_PropertyRenterData[entity1].m_Property;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_OnFireData.HasComponent(entity1) || this.m_DestroyedData.HasComponent(entity1))
          return false;
        if ((currentLane.m_Flags & CreatureLaneFlags.Hangaround) != (CreatureLaneFlags) 0)
          pet.m_Flags |= PetFlags.Hangaround;
        else
          pet.m_Flags &= ~PetFlags.Hangaround;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(pet.m_HouseholdPet))
        {
          if ((pet.m_Flags & PetFlags.Hangaround) == PetFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<CurrentTransport>(jobIndex, pet.m_HouseholdPet);
          }
          if ((pet.m_Flags & PetFlags.Arrived) == PetFlags.None)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<CurrentBuilding>(jobIndex, pet.m_HouseholdPet, new CurrentBuilding(entity1));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ResetTripArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<ResetTrip>(jobIndex, entity2, new ResetTrip()
            {
              m_Creature = entity,
              m_Target = target.m_Target
            });
            pet.m_Flags |= PetFlags.Arrived;
          }
        }
        if ((pet.m_Flags & PetFlags.Hangaround) != PetFlags.None)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
        return true;
      }

      private void TryEnterVehicle(
        Entity entity,
        Entity vehicle,
        ref AnimalCurrentLane currentLane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BoardingQueue.Enqueue(PetAISystem.Boarding.TryEnterVehicle(entity, vehicle, CreatureVehicleFlags.Leader));
      }

      private void FinishEnterVehicle(
        Entity entity,
        Entity vehicle,
        ref AnimalCurrentLane currentLane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BoardingQueue.Enqueue(PetAISystem.Boarding.FinishEnterVehicle(entity, vehicle, currentLane));
      }

      private void CancelEnterVehicle(
        Entity entity,
        Entity vehicle,
        ref AnimalCurrentLane currentLane)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_BoardingQueue.Enqueue(PetAISystem.Boarding.CancelEnterVehicle(entity, vehicle));
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
    private struct BoardingJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      public ComponentLookup<Creature> m_Creatures;
      public BufferLookup<Passenger> m_Passengers;
      public BufferLookup<LaneObject> m_LaneObjects;
      [ReadOnly]
      public ComponentTypeSet m_CurrentLaneTypes;
      public NativeQueue<PetAISystem.Boarding> m_BoardingQueue;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PetAISystem.Boarding boarding;
        // ISSUE: reference to a compiler-generated field
        while (this.m_BoardingQueue.TryDequeue(out boarding))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          PetAISystem.BoardingType type = boarding.m_Type;
          switch (type)
          {
            case PetAISystem.BoardingType.Exit:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.ExitVehicle(boarding.m_Passenger, boarding.m_Vehicle, boarding.m_CurrentLane, boarding.m_Position);
              continue;
            case PetAISystem.BoardingType.TryEnter:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.TryEnterVehicle(boarding.m_Passenger, boarding.m_Vehicle, boarding.m_Flags);
              continue;
            case PetAISystem.BoardingType.FinishEnter:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.FinishEnterVehicle(boarding.m_Passenger, boarding.m_Vehicle, boarding.m_CurrentLane);
              continue;
            case PetAISystem.BoardingType.CancelEnter:
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CancelEnterVehicle(boarding.m_Passenger, boarding.m_Vehicle);
              continue;
            default:
              continue;
          }
        }
      }

      private void ExitVehicle(
        Entity passenger,
        Entity vehicle,
        AnimalCurrentLane newCurrentLane,
        float3 position)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Passengers.HasBuffer(vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.RemoveValue<Passenger>(this.m_Passengers[vehicle], new Passenger(passenger));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentVehicle>(passenger);
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneObjects.HasBuffer(newCurrentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          NetUtils.AddLaneObject(this.m_LaneObjects[newCurrentLane.m_Lane], passenger, (float2) newCurrentLane.m_CurvePosition.x);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData geometryData = this.m_ObjectGeometryData[this.m_PrefabRefData[passenger].m_Prefab];
          Bounds3 bounds = ObjectUtils.CalculateBounds(position, quaternion.identity, geometryData);
          // ISSUE: reference to a compiler-generated field
          this.m_SearchTree.Add(passenger, new QuadTreeBoundsXZ(bounds));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(passenger, in this.m_CurrentLaneTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(passenger, new Updated());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AnimalCurrentLane>(passenger, newCurrentLane);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<Transform>(passenger, new Transform(position, quaternion.identity));
      }

      private void TryEnterVehicle(Entity passenger, Entity vehicle, CreatureVehicleFlags flags)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Passengers.HasBuffer(vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Passengers[vehicle].Add(new Passenger(passenger));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CurrentVehicle>(passenger, new CurrentVehicle(vehicle, flags));
      }

      private void CancelEnterVehicle(Entity passenger, Entity vehicle)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Passengers.HasBuffer(vehicle))
        {
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.RemoveValue<Passenger>(this.m_Passengers[vehicle], new Passenger(passenger));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<CurrentVehicle>(passenger);
      }

      private void FinishEnterVehicle(
        Entity passenger,
        Entity vehicle,
        AnimalCurrentLane oldCurrentLane)
      {
        // ISSUE: reference to a compiler-generated field
        Creature creature = this.m_Creatures[passenger] with
        {
          m_QueueEntity = Entity.Null,
          m_QueueArea = new Sphere3()
        };
        // ISSUE: reference to a compiler-generated field
        this.m_Creatures[passenger] = creature;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LaneObjects.HasBuffer(oldCurrentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          NetUtils.RemoveLaneObject(this.m_LaneObjects[oldCurrentLane.m_Lane], passenger);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SearchTree.TryRemove(passenger);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(passenger, in this.m_CurrentLaneTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Unspawned>(passenger, new Unspawned());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(passenger, new Updated());
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> __Game_Creatures_GroupMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Unspawned> __Game_Objects_Unspawned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<GroupCreature> __Game_Creatures_GroupCreature_RO_BufferTypeHandle;
      public ComponentTypeHandle<Animal> __Game_Creatures_Animal_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Creatures.Pet> __Game_Creatures_Pet_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Creature> __Game_Creatures_Creature_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OnFire> __Game_Events_OnFire_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PersonalCar> __Game_Vehicles_PersonalCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.Taxi> __Game_Vehicles_Taxi_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PoliceCar> __Game_Vehicles_PoliceCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathOwner> __Game_Pathfind_PathOwner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      public ComponentLookup<Creature> __Game_Creatures_Creature_RW_ComponentLookup;
      public BufferLookup<Passenger> __Game_Vehicles_Passenger_RW_BufferLookup;
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupCreature_RO_BufferTypeHandle = state.GetBufferTypeHandle<GroupCreature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Animal_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Animal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Pet_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Creatures.Pet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Creature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_OnFire_RO_ComponentLookup = state.GetComponentLookup<OnFire>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PersonalCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PersonalCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Taxi_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.Taxi>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PoliceCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RO_ComponentLookup = state.GetComponentLookup<PathOwner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RW_ComponentLookup = state.GetComponentLookup<Creature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RW_BufferLookup = state.GetBufferLookup<Passenger>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferLookup = state.GetBufferLookup<LaneObject>();
      }
    }
  }
}
