// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MedicalAircraftAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
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
  public class MedicalAircraftAISystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_VehicleQuery;
    private EntityArchetype m_HandleRequestArchetype;
    private ComponentTypeSet m_MovingToParkedAircraftRemoveTypes;
    private ComponentTypeSet m_MovingToParkedAddTypes;
    private MedicalAircraftAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 10;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Vehicles.Ambulance>(), ComponentType.ReadWrite<AircraftCurrentLane>(), ComponentType.ReadOnly<Owner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadWrite<PathOwner>(), ComponentType.ReadWrite<Target>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<TripSource>(), ComponentType.Exclude<OutOfControl>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<HandleRequest>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingToParkedAircraftRemoveTypes = new ComponentTypeSet(new ComponentType[12]
      {
        ComponentType.ReadWrite<Moving>(),
        ComponentType.ReadWrite<TransformFrame>(),
        ComponentType.ReadWrite<InterpolatedTransform>(),
        ComponentType.ReadWrite<AircraftNavigation>(),
        ComponentType.ReadWrite<AircraftNavigationLane>(),
        ComponentType.ReadWrite<AircraftCurrentLane>(),
        ComponentType.ReadWrite<PathOwner>(),
        ComponentType.ReadWrite<Target>(),
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Aircraft_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new MedicalAircraftAISystem.MedicalAircraftTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_UnspawnedType = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_StoppedType = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentTypeHandle,
        m_AmbulanceType = this.__TypeHandle.__Game_Vehicles_Ambulance_RW_ComponentTypeHandle,
        m_AircraftType = this.__TypeHandle.__Game_Vehicles_Aircraft_RW_ComponentTypeHandle,
        m_CurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RW_ComponentTypeHandle,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_AircraftNavigationLaneType = this.__TypeHandle.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle,
        m_PassengerType = this.__TypeHandle.__Game_Vehicles_Passenger_RW_BufferTypeHandle,
        m_ServiceDispatchType = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle,
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_PathInformationData = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_PrefabHelicopterData = this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup,
        m_HealthcareRequestData = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_HospitalData = this.__TypeHandle.__Game_Buildings_Hospital_RO_ComponentLookup,
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_HealthProblemData = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentLookup,
        m_TravelPurposeData = this.__TypeHandle.__Game_Citizens_TravelPurpose_RO_ComponentLookup,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_BlockerData = this.__TypeHandle.__Game_Vehicles_Blocker_RW_ComponentLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RW_BufferLookup,
        m_RandomSeed = RandomSeed.Next(),
        m_HandleRequestArchetype = this.m_HandleRequestArchetype,
        m_MovingToParkedAircraftRemoveTypes = this.m_MovingToParkedAircraftRemoveTypes,
        m_MovingToParkedAddTypes = this.m_MovingToParkedAddTypes,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter()
      }.ScheduleParallel<MedicalAircraftAISystem.MedicalAircraftTickJob>(this.m_VehicleQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
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
    public MedicalAircraftAISystem()
    {
    }

    [BurstCompile]
    private struct MedicalAircraftTickJob : IJobChunk
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
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> m_StoppedType;
      public ComponentTypeHandle<Game.Vehicles.Ambulance> m_AmbulanceType;
      public ComponentTypeHandle<Aircraft> m_AircraftType;
      public ComponentTypeHandle<AircraftCurrentLane> m_CurrentLaneType;
      public ComponentTypeHandle<Target> m_TargetType;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public BufferTypeHandle<AircraftNavigationLane> m_AircraftNavigationLaneType;
      public BufferTypeHandle<Passenger> m_PassengerType;
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<HelicopterData> m_PrefabHelicopterData;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> m_HospitalData;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemData;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> m_TravelPurposeData;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Blocker> m_BlockerData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_HandleRequestArchetype;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAircraftRemoveTypes;
      [ReadOnly]
      public ComponentTypeSet m_MovingToParkedAddTypes;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;

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
        NativeArray<PathInformation> nativeArray4 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AircraftCurrentLane> nativeArray5 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_CurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Vehicles.Ambulance> nativeArray6 = chunk.GetNativeArray<Game.Vehicles.Ambulance>(ref this.m_AmbulanceType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Aircraft> nativeArray7 = chunk.GetNativeArray<Aircraft>(ref this.m_AircraftType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray8 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray9 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<AircraftNavigationLane> bufferAccessor1 = chunk.GetBufferAccessor<AircraftNavigationLane>(ref this.m_AircraftNavigationLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Passenger> bufferAccessor2 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceDispatch> bufferAccessor3 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
        // ISSUE: reference to a compiler-generated field
        bool isStopped = chunk.Has<Stopped>(ref this.m_StoppedType);
        // ISSUE: reference to a compiler-generated field
        bool isUnspawned = chunk.Has<Unspawned>(ref this.m_UnspawnedType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          PathInformation pathInformation = nativeArray4[index];
          Game.Vehicles.Ambulance ambulance = nativeArray6[index];
          Aircraft aircraft = nativeArray7[index];
          AircraftCurrentLane currentLane = nativeArray5[index];
          PathOwner pathOwner = nativeArray9[index];
          Target target = nativeArray8[index];
          DynamicBuffer<AircraftNavigationLane> navigationLanes = bufferAccessor1[index];
          DynamicBuffer<Passenger> passengers = bufferAccessor2[index];
          DynamicBuffer<ServiceDispatch> serviceDispatches = bufferAccessor3[index];
          // ISSUE: reference to a compiler-generated field
          VehicleUtils.CheckUnspawned(unfilteredChunkIndex, entity, currentLane, isUnspawned, this.m_CommandBuffer);
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, owner, prefabRef, pathInformation, navigationLanes, passengers, serviceDispatches, isStopped, ref random, ref ambulance, ref aircraft, ref currentLane, ref pathOwner, ref target);
          nativeArray6[index] = ambulance;
          nativeArray7[index] = aircraft;
          nativeArray5[index] = currentLane;
          nativeArray9[index] = pathOwner;
          nativeArray8[index] = target;
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        Owner owner,
        PrefabRef prefabRef,
        PathInformation pathInformation,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isStopped,
        ref Random random,
        ref Game.Vehicles.Ambulance ambulance,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckServiceDispatches(entity, serviceDispatches, ref ambulance);
        if (VehicleUtils.ResetUpdatedPath(ref pathOwner))
        {
          // ISSUE: reference to a compiler-generated method
          this.ResetPath(jobIndex, entity, pathInformation, passengers, serviceDispatches, ref ambulance, ref aircraft, ref currentLane, ref target);
        }
        if (VehicleUtils.IsStuck(pathOwner))
        {
          // ISSUE: reference to a compiler-generated field
          Blocker blocker = this.m_BlockerData[entity];
          // ISSUE: reference to a compiler-generated field
          int num = this.m_ParkedCarData.HasComponent(blocker.m_Blocker) ? 1 : 0;
          if (num != 0)
          {
            Entity entity1 = blocker.m_Blocker;
            Controller componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ControllerData.TryGetComponent(entity1, out componentData))
              entity1 = componentData.m_Controller;
            DynamicBuffer<LayoutElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_LayoutElements.TryGetBuffer(entity1, out bufferData);
            // ISSUE: reference to a compiler-generated field
            VehicleUtils.DeleteVehicle(this.m_CommandBuffer, jobIndex, entity1, bufferData);
          }
          if (num != 0 || blocker.m_Blocker == Entity.Null)
          {
            pathOwner.m_State &= ~PathFlags.Stuck;
            // ISSUE: reference to a compiler-generated field
            this.m_BlockerData[entity] = new Blocker();
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_EntityLookup.Exists(target.m_Target) || VehicleUtils.PathfindFailed(pathOwner))
        {
          if (VehicleUtils.IsStuck(pathOwner) || (ambulance.m_State & AmbulanceFlags.Returning) != (AmbulanceFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.ReturnToDepot(owner, serviceDispatches, ref ambulance, ref aircraft, ref pathOwner, ref target);
          }
        }
        else
        {
          if (VehicleUtils.PathEndReached(currentLane) || (ambulance.m_State & (AmbulanceFlags.AtTarget | AmbulanceFlags.Disembarking)) != (AmbulanceFlags) 0)
          {
            if ((ambulance.m_State & AmbulanceFlags.Returning) != (AmbulanceFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.UnloadPatients(passengers, ref ambulance))
                return;
              if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
              {
                // ISSUE: reference to a compiler-generated method
                this.ParkAircraft(jobIndex, entity, owner, ref aircraft, ref ambulance, ref currentLane);
                return;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
              return;
            }
            if ((ambulance.m_State & AmbulanceFlags.Transporting) != (AmbulanceFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.UnloadPatients(passengers, ref ambulance))
                return;
              // ISSUE: reference to a compiler-generated method
              if (!this.SelectNextDispatch(jobIndex, entity, navigationLanes, serviceDispatches, ref ambulance, ref aircraft, ref currentLane, ref pathOwner, ref target))
              {
                if (target.m_Target == owner.m_Owner)
                {
                  if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.ParkAircraft(jobIndex, entity, owner, ref aircraft, ref ambulance, ref currentLane);
                    return;
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                  return;
                }
                // ISSUE: reference to a compiler-generated method
                this.ReturnToDepot(owner, serviceDispatches, ref ambulance, ref aircraft, ref pathOwner, ref target);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (this.LoadPatients(jobIndex, entity, passengers, serviceDispatches, isStopped, ref random, ref ambulance, ref target))
              {
                if ((ambulance.m_State & AmbulanceFlags.Transporting) != (AmbulanceFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.TransportToHospital(owner, serviceDispatches, ref ambulance, ref aircraft, ref pathOwner, ref target);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  if (!this.SelectNextDispatch(jobIndex, entity, navigationLanes, serviceDispatches, ref ambulance, ref aircraft, ref currentLane, ref pathOwner, ref target))
                  {
                    if (target.m_Target == owner.m_Owner)
                    {
                      if (VehicleUtils.ParkingSpaceReached(currentLane, pathOwner))
                      {
                        // ISSUE: reference to a compiler-generated method
                        this.ParkAircraft(jobIndex, entity, owner, ref aircraft, ref ambulance, ref currentLane);
                        return;
                      }
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, entity, new Deleted());
                      return;
                    }
                    // ISSUE: reference to a compiler-generated method
                    this.ReturnToDepot(owner, serviceDispatches, ref ambulance, ref aircraft, ref pathOwner, ref target);
                  }
                }
              }
            }
          }
          else
          {
            if ((ambulance.m_State & (AmbulanceFlags.Returning | AmbulanceFlags.Transporting | AmbulanceFlags.Disabled)) == AmbulanceFlags.Disabled)
            {
              // ISSUE: reference to a compiler-generated method
              this.ReturnToDepot(owner, serviceDispatches, ref ambulance, ref aircraft, ref pathOwner, ref target);
            }
            if (isStopped)
            {
              // ISSUE: reference to a compiler-generated method
              this.StartVehicle(jobIndex, entity);
            }
          }
          // ISSUE: reference to a compiler-generated method
          if ((ambulance.m_State & (AmbulanceFlags.Returning | AmbulanceFlags.Dispatched | AmbulanceFlags.Transporting)) == (AmbulanceFlags.Returning | AmbulanceFlags.Dispatched) && !this.SelectNextDispatch(jobIndex, entity, navigationLanes, serviceDispatches, ref ambulance, ref aircraft, ref currentLane, ref pathOwner, ref target))
          {
            serviceDispatches.Clear();
            ambulance.m_State &= ~AmbulanceFlags.Dispatched;
          }
          if ((ambulance.m_State & (AmbulanceFlags.AtTarget | AmbulanceFlags.Disembarking)) != (AmbulanceFlags) 0)
            return;
          if (VehicleUtils.RequireNewPath(pathOwner))
          {
            // ISSUE: reference to a compiler-generated method
            this.FindNewPath(entity, prefabRef, ref ambulance, ref aircraft, ref currentLane, ref pathOwner, ref target);
          }
          else
          {
            if ((pathOwner.m_State & (PathFlags.Pending | PathFlags.Failed | PathFlags.Stuck)) != (PathFlags) 0)
              return;
            // ISSUE: reference to a compiler-generated method
            this.CheckParkingSpace(ref aircraft, ref currentLane, ref pathOwner);
          }
        }
      }

      private void CheckParkingSpace(
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner)
      {
        Game.Objects.SpawnLocation componentData;
        // ISSUE: reference to a compiler-generated field
        if ((currentLane.m_LaneFlags & AircraftLaneFlags.EndOfPath) == (AircraftLaneFlags) 0 || !this.m_SpawnLocationData.TryGetComponent(currentLane.m_Lane, out componentData))
          return;
        if ((componentData.m_Flags & SpawnLocationFlags.ParkedVehicle) != (SpawnLocationFlags) 0)
        {
          if ((aircraft.m_Flags & AircraftFlags.IgnoreParkedVehicle) != (AircraftFlags) 0)
            return;
          aircraft.m_Flags |= AircraftFlags.IgnoreParkedVehicle;
          pathOwner.m_State |= PathFlags.Obsolete;
        }
        else
          aircraft.m_Flags &= ~AircraftFlags.IgnoreParkedVehicle;
      }

      private void ParkAircraft(
        int jobIndex,
        Entity entity,
        Owner owner,
        ref Aircraft aircraft,
        ref Game.Vehicles.Ambulance ambulance,
        ref AircraftCurrentLane currentLane)
      {
        aircraft.m_Flags &= ~(AircraftFlags.Emergency | AircraftFlags.IgnoreParkedVehicle);
        ambulance.m_State = (AmbulanceFlags) 0;
        Game.Buildings.Hospital componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_HospitalData.TryGetComponent(owner.m_Owner, out componentData) && (componentData.m_Flags & HospitalFlags.HasAvailableMedicalHelicopters) == (HospitalFlags) 0)
          ambulance.m_State |= AmbulanceFlags.Disabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(jobIndex, entity, in this.m_MovingToParkedAircraftRemoveTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(jobIndex, entity, in this.m_MovingToParkedAddTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(jobIndex, entity, new ParkedCar(currentLane.m_Lane, currentLane.m_CurvePosition.x));
        // ISSUE: reference to a compiler-generated field
        if (this.m_SpawnLocationData.HasComponent(currentLane.m_Lane))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<PathfindUpdated>(jobIndex, currentLane.m_Lane);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<FixParkingLocation>(jobIndex, entity, new FixParkingLocation(Entity.Null, entity));
      }

      private void StopVehicle(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Moving>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<TransformFrame>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Stopped>(jobIndex, entity, new Stopped());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
      }

      private void StartVehicle(int jobIndex, Entity entity)
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
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
      }

      private bool LoadPatients(
        int jobIndex,
        Entity entity,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        bool isStopped,
        ref Random random,
        ref Game.Vehicles.Ambulance ambulance,
        ref Target target)
      {
        ambulance.m_State |= AmbulanceFlags.AtTarget;
        bool flag = false;
        if (serviceDispatches.Length == 0 || (ambulance.m_State & AmbulanceFlags.Dispatched) == (AmbulanceFlags) 0)
        {
          ambulance.m_TargetPatient = Entity.Null;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HealthProblemData.HasComponent(ambulance.m_TargetPatient))
        {
          ambulance.m_TargetPatient = Entity.Null;
          return true;
        }
        // ISSUE: reference to a compiler-generated field
        HealthProblem healthProblem = this.m_HealthProblemData[ambulance.m_TargetPatient];
        if (healthProblem.m_HealthcareRequest != serviceDispatches[0].m_Request || (healthProblem.m_Flags & HealthProblemFlags.RequireTransport) == HealthProblemFlags.None)
        {
          ambulance.m_TargetPatient = Entity.Null;
          return true;
        }
        CurrentBuilding componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentBuildingData.TryGetComponent(ambulance.m_TargetPatient, out componentData1) && componentData1.m_CurrentBuilding != Entity.Null)
          flag |= componentData1.m_CurrentBuilding == target.m_Target;
        CurrentTransport componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentTransportData.TryGetComponent(ambulance.m_TargetPatient, out componentData2) && componentData2.m_CurrentTransport != Entity.Null)
        {
          flag |= componentData2.m_CurrentTransport == target.m_Target;
          TravelPurpose componentData3;
          // ISSUE: reference to a compiler-generated field
          if (!flag && (this.m_TravelPurposeData.TryGetComponent(ambulance.m_TargetPatient, out componentData3) && componentData3.m_Purpose == Game.Citizens.Purpose.Hospital || componentData3.m_Purpose == Game.Citizens.Purpose.Deathcare))
            flag = true;
          if (flag)
          {
            for (int index = 0; index < passengers.Length; ++index)
            {
              Entity passenger = passengers[index].m_Passenger;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (passenger == componentData2.m_CurrentTransport && this.m_CurrentVehicleData.HasComponent(passenger) && (this.m_CurrentVehicleData[passenger].m_Flags & CreatureVehicleFlags.Ready) != (CreatureVehicleFlags) 0)
              {
                ambulance.m_State |= AmbulanceFlags.Transporting;
                Citizen componentData4;
                // ISSUE: reference to a compiler-generated field
                if (this.m_CitizenData.TryGetComponent(ambulance.m_TargetPatient, out componentData4) && random.NextInt(100) >= (int) componentData4.m_Health)
                  ambulance.m_State |= AmbulanceFlags.Critical;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity1, new HandleRequest(healthProblem.m_HealthcareRequest, entity, true));
                return true;
              }
            }
          }
        }
        if (flag)
        {
          if (!isStopped)
          {
            // ISSUE: reference to a compiler-generated method
            this.StopVehicle(jobIndex, entity);
          }
          return false;
        }
        ambulance.m_TargetPatient = Entity.Null;
        return true;
      }

      private bool UnloadPatients(DynamicBuffer<Passenger> passengers, ref Game.Vehicles.Ambulance ambulance)
      {
        if (passengers.Length > 0)
        {
          ambulance.m_State |= AmbulanceFlags.Disembarking;
          return false;
        }
        passengers.Clear();
        ambulance.m_State &= ~(AmbulanceFlags.Transporting | AmbulanceFlags.Disembarking | AmbulanceFlags.Critical);
        ambulance.m_TargetPatient = Entity.Null;
        return true;
      }

      private void FindNewPath(
        Entity vehicleEntity,
        PrefabRef prefabRef,
        ref Game.Vehicles.Ambulance ambulance,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        HelicopterData helicopterData = this.m_PrefabHelicopterData[prefabRef.m_Prefab];
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) helicopterData.m_FlyingMaxSpeed,
          m_WalkSpeed = (float2) 5.555556f,
          m_Methods = PathMethod.Road | PathMethod.Flying,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.Flying,
          m_RoadTypes = RoadTypes.Helicopter,
          m_FlyingTypes = RoadTypes.Helicopter
        };
        SetupQueueTarget destination = new SetupQueueTarget();
        if ((ambulance.m_State & AmbulanceFlags.FindHospital) != (AmbulanceFlags) 0)
        {
          // ISSUE: reference to a compiler-generated method
          destination.m_Entity = this.FindDistrict(ambulance.m_TargetLocation);
          destination.m_Type = SetupTargetType.Hospital;
          destination.m_Methods = PathMethod.Road;
          destination.m_RoadTypes = RoadTypes.Helicopter;
        }
        else if ((ambulance.m_State & AmbulanceFlags.Returning) != (AmbulanceFlags) 0)
        {
          destination.m_Type = SetupTargetType.CurrentLocation;
          destination.m_Methods = PathMethod.Road;
          destination.m_RoadTypes = RoadTypes.Helicopter;
          destination.m_Entity = target.m_Target;
        }
        else
        {
          destination.m_Type = SetupTargetType.CurrentLocation;
          destination.m_Methods = PathMethod.Road | PathMethod.Flying;
          destination.m_RoadTypes = RoadTypes.Helicopter;
          destination.m_FlyingTypes = RoadTypes.Helicopter;
          destination.m_Entity = target.m_Target;
        }
        if ((ambulance.m_State & (AmbulanceFlags.Returning | AmbulanceFlags.Dispatched | AmbulanceFlags.Transporting)) == AmbulanceFlags.Dispatched || (ambulance.m_State & (AmbulanceFlags.Transporting | AmbulanceFlags.Critical)) == (AmbulanceFlags.Transporting | AmbulanceFlags.Critical))
        {
          parameters.m_Weights = new PathfindWeights(1f, 0.0f, 0.0f, 0.0f);
          parameters.m_IgnoredRules = RuleFlags.ForbidHeavyTraffic;
        }
        else
          parameters.m_Weights = new PathfindWeights(1f, 1f, 1f, 1f);
        if ((ambulance.m_State & (AmbulanceFlags.Returning | AmbulanceFlags.Dispatched | AmbulanceFlags.Transporting)) != AmbulanceFlags.Dispatched)
          destination.m_RandomCost = 30f;
        SetupQueueItem setupQueueItem = new SetupQueueItem(vehicleEntity, parameters, origin, destination);
        // ISSUE: reference to a compiler-generated field
        VehicleUtils.SetupPathfind(ref currentLane, ref pathOwner, this.m_PathfindQueue, setupQueueItem);
      }

      private Entity FindDistrict(Entity building)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_CurrentDistrictData.HasComponent(building) ? this.m_CurrentDistrictData[building].m_District : Entity.Null;
      }

      private void CheckServiceDispatches(
        Entity vehicleEntity,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Ambulance ambulance)
      {
        if ((ambulance.m_State & AmbulanceFlags.Transporting) != (AmbulanceFlags) 0)
          serviceDispatches.Clear();
        else if ((ambulance.m_State & AmbulanceFlags.Dispatched) != (AmbulanceFlags) 0)
        {
          if (serviceDispatches.Length <= 1)
            return;
          serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
        }
        else
        {
          Entity request1 = Entity.Null;
          for (int index = 0; index < serviceDispatches.Length; ++index)
          {
            Entity request2 = serviceDispatches[index].m_Request;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HealthcareRequestData.HasComponent(request2))
            {
              // ISSUE: reference to a compiler-generated field
              HealthcareRequest healthcareRequest = this.m_HealthcareRequestData[request2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CurrentTransportData.HasComponent(healthcareRequest.m_Citizen) || this.m_CurrentBuildingData.HasComponent(healthcareRequest.m_Citizen))
              {
                request1 = request2;
                break;
              }
            }
          }
          if (request1 != Entity.Null)
          {
            serviceDispatches[0] = new ServiceDispatch(request1);
            if (serviceDispatches.Length > 1)
              serviceDispatches.RemoveRange(1, serviceDispatches.Length - 1);
            ambulance.m_State |= AmbulanceFlags.Dispatched;
          }
          else
            serviceDispatches.Clear();
        }
      }

      private bool SelectNextDispatch(
        int jobIndex,
        Entity vehicleEntity,
        DynamicBuffer<AircraftNavigationLane> navigationLanes,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Ambulance ambulance,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref PathOwner pathOwner,
        ref Target targetData)
      {
        if ((ambulance.m_State & AmbulanceFlags.Returning) == (AmbulanceFlags) 0 && (ambulance.m_State & AmbulanceFlags.Dispatched) != (AmbulanceFlags) 0 && serviceDispatches.Length > 0)
        {
          serviceDispatches.RemoveAt(0);
          ambulance.m_State &= ~AmbulanceFlags.Dispatched;
        }
        for (; (ambulance.m_State & AmbulanceFlags.Dispatched) != (AmbulanceFlags) 0 && serviceDispatches.Length > 0; ambulance.m_State &= ~AmbulanceFlags.Dispatched)
        {
          Entity request = serviceDispatches[0].m_Request;
          Entity citizen = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_HealthcareRequestData.HasComponent(request))
          {
            // ISSUE: reference to a compiler-generated field
            citizen = this.m_HealthcareRequestData[request].m_Citizen;
          }
          Entity entity1 = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentTransportData.HasComponent(citizen))
          {
            // ISSUE: reference to a compiler-generated field
            entity1 = this.m_CurrentTransportData[citizen].m_CurrentTransport;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentBuildingData.HasComponent(citizen))
            {
              // ISSUE: reference to a compiler-generated field
              entity1 = this.m_CurrentBuildingData[citizen].m_CurrentBuilding;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_EntityLookup.Exists(entity1))
          {
            serviceDispatches.RemoveAt(0);
          }
          else
          {
            aircraft.m_Flags &= ~AircraftFlags.IgnoreParkedVehicle;
            ambulance.m_TargetPatient = citizen;
            ambulance.m_TargetLocation = entity1;
            ambulance.m_State &= ~(AmbulanceFlags.Returning | AmbulanceFlags.FindHospital | AmbulanceFlags.AtTarget);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_HandleRequestArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<HandleRequest>(jobIndex, entity2, new HandleRequest(request, vehicleEntity, false, true));
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
                float num = ambulance.m_PathElementTime * (float) pathElement2.Length + this.m_PathInformationData[request].m_Duration;
                if (PathUtils.TryAppendPath(ref currentLane, navigationLanes, pathElement2, pathElement1))
                {
                  ambulance.m_PathElementTime = num / (float) math.max(1, pathElement2.Length);
                  targetData.m_Target = entity1;
                  VehicleUtils.ClearEndOfPath(ref currentLane, navigationLanes);
                  aircraft.m_Flags |= AircraftFlags.Emergency;
                  return true;
                }
              }
            }
            VehicleUtils.SetTarget(ref pathOwner, ref targetData, entity1);
            return true;
          }
        }
        return false;
      }

      private void TransportToHospital(
        Owner owner,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Ambulance ambulance,
        ref Aircraft aircraft,
        ref PathOwner pathOwner,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated method
        this.ReturnToDepot(owner, serviceDispatches, ref ambulance, ref aircraft, ref pathOwner, ref target);
      }

      private void ReturnToDepot(
        Owner owner,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Ambulance ambulance,
        ref Aircraft aircraft,
        ref PathOwner pathOwner,
        ref Target target)
      {
        serviceDispatches.Clear();
        aircraft.m_Flags &= ~AircraftFlags.IgnoreParkedVehicle;
        ambulance.m_State &= ~(AmbulanceFlags.Dispatched | AmbulanceFlags.FindHospital | AmbulanceFlags.AtTarget);
        ambulance.m_State |= AmbulanceFlags.Returning;
        VehicleUtils.SetTarget(ref pathOwner, ref target, owner.m_Owner);
      }

      private void ResetPath(
        int jobIndex,
        Entity vehicleEntity,
        PathInformation pathInformation,
        DynamicBuffer<Passenger> passengers,
        DynamicBuffer<ServiceDispatch> serviceDispatches,
        ref Game.Vehicles.Ambulance ambulance,
        ref Aircraft aircraft,
        ref AircraftCurrentLane currentLane,
        ref Target target)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PathElement> pathElement = this.m_PathElements[vehicleEntity];
        PathUtils.ResetPath(ref currentLane, pathElement);
        if ((ambulance.m_State & AmbulanceFlags.FindHospital) != (AmbulanceFlags) 0)
        {
          target.m_Target = pathInformation.m_Destination;
          ambulance.m_State &= ~AmbulanceFlags.FindHospital;
          for (int index = 0; index < passengers.Length; ++index)
          {
            Entity passenger = passengers[index].m_Passenger;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurrentVehicleData.HasComponent(passenger))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Target>(jobIndex, passenger, target);
            }
          }
        }
        if ((ambulance.m_State & AmbulanceFlags.Dispatched) != (AmbulanceFlags) 0 && serviceDispatches.Length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_HealthcareRequestData.HasComponent(serviceDispatches[0].m_Request))
            aircraft.m_Flags |= AircraftFlags.Emergency;
          else
            aircraft.m_Flags &= ~AircraftFlags.Emergency;
        }
        else if ((ambulance.m_State & AmbulanceFlags.Critical) != (AmbulanceFlags) 0)
          aircraft.m_Flags |= AircraftFlags.Emergency;
        else
          aircraft.m_Flags &= ~AircraftFlags.Emergency;
        ambulance.m_PathElementTime = pathInformation.m_Duration / (float) math.max(1, pathElement.Length);
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
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stopped> __Game_Objects_Stopped_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Vehicles.Ambulance> __Game_Vehicles_Ambulance_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Aircraft> __Game_Vehicles_Aircraft_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Target> __Game_Common_Target_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public BufferTypeHandle<AircraftNavigationLane> __Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle;
      public BufferTypeHandle<Passenger> __Game_Vehicles_Passenger_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HelicopterData> __Game_Prefabs_HelicopterData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Hospital> __Game_Buildings_Hospital_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TravelPurpose> __Game_Citizens_TravelPurpose_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      public ComponentLookup<Blocker> __Game_Vehicles_Blocker_RW_ComponentLookup;
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RW_BufferLookup;

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
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Vehicles.Ambulance>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Aircraft_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Aircraft>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Target>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftNavigationLane_RW_BufferTypeHandle = state.GetBufferTypeHandle<AircraftNavigationLane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Passenger_RW_BufferTypeHandle = state.GetBufferTypeHandle<Passenger>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HelicopterData_RO_ComponentLookup = state.GetComponentLookup<HelicopterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentLookup = state.GetComponentLookup<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Hospital>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentLookup = state.GetComponentLookup<HealthProblem>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TravelPurpose_RO_ComponentLookup = state.GetComponentLookup<TravelPurpose>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RW_ComponentLookup = state.GetComponentLookup<Blocker>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RW_BufferLookup = state.GetBufferLookup<PathElement>();
      }
    }
  }
}
