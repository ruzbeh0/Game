// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.ParkedVehiclesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  [CompilerGenerated]
  public class ParkedVehiclesSystem : GameSystemBase
  {
    private ModificationBarrier4B m_ModificationBarrier;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_BuildingQuery;
    private EntityQuery m_DeletedVehicleQuery;
    private EntityQuery m_PoliceCarQuery;
    private EntityQuery m_FireEngineQuery;
    private EntityQuery m_HealthcareVehicleQuery;
    private EntityQuery m_TransportVehicleQuery;
    private EntityQuery m_PostVanQuery;
    private EntityQuery m_MaintenanceVehicleQuery;
    private EntityQuery m_GarbageTruckQuery;
    private PoliceCarSelectData m_PoliceCarSelectData;
    private FireEngineSelectData m_FireEngineSelectData;
    private HealthcareVehicleSelectData m_HealthcareVehicleSelectData;
    private TransportVehicleSelectData m_TransportVehicleSelectData;
    private PostVanSelectData m_PostVanSelectData;
    private MaintenanceVehicleSelectData m_MaintenanceVehicleSelectData;
    private GarbageTruckSelectData m_GarbageTruckSelectData;
    private ParkedVehiclesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<OwnedVehicle>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Temp>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Applied>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedVehicleQuery = this.GetEntityQuery(ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Vehicle>(), ComponentType.ReadOnly<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceCarQuery = this.GetEntityQuery(PoliceCarSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_FireEngineQuery = this.GetEntityQuery(FireEngineSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleQuery = this.GetEntityQuery(HealthcareVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleQuery = this.GetEntityQuery(TransportVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanQuery = this.GetEntityQuery(PostVanSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleQuery = this.GetEntityQuery(MaintenanceVehicleSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckQuery = this.GetEntityQuery(GarbageTruckSelectData.GetEntityQueryDesc());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceCarSelectData = new PoliceCarSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_FireEngineSelectData = new FireEngineSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData = new HealthcareVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData = new TransportVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanSelectData = new PostVanSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleSelectData = new MaintenanceVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckSelectData = new GarbageTruckSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_BuildingQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Temp> componentTypeHandle = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
      this.EntityManager.CompleteDependencyBeforeRO<PrefabRef>();
      this.EntityManager.CompleteDependencyBeforeRO<Temp>();
      try
      {
        NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap = new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>();
        JobHandle deletedVehiclesDeps = new JobHandle();
        JobHandle dependency = this.Dependency;
        JobHandle job0 = new JobHandle();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<Temp> nativeArray2 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            Temp temp;
            bool isTemp = CollectionUtils.TryGet<Temp>(nativeArray2, index2, out temp);
            if ((temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) == (TempFlags) 0)
            {
              NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations = new NativeList<ParkedVehiclesSystem.ParkingLocation>();
              JobHandle parkingLocationDeps = new JobHandle();
              if (isTemp && temp.m_Original != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                // ISSUE: reference to a compiler-generated method
                this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                // ISSUE: reference to a compiler-generated method
                this.DuplicateVehicles(entity, temp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.PoliceStation>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnPoliceCars(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.FireStation>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnFireEngines(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.Hospital>(entity) || this.EntityManager.HasComponent<Game.Buildings.DeathcareFacility>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnHealthcareVehicles(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.TransportDepot>(entity) || this.EntityManager.HasComponent<Game.Buildings.Prison>(entity) || this.EntityManager.HasComponent<Game.Buildings.EmergencyShelter>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnTransportVehicles(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.PostFacility>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnPostVans(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.MaintenanceDepot>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnMaintenanceVehicles(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (this.EntityManager.HasComponent<Game.Buildings.GarbageFacility>(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindParkingLocations(entity, ref parkingLocations, dependency, ref parkingLocationDeps);
                if (isTemp)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CollectDeletedVehicles(ref deletedVehicleMap, dependency, ref deletedVehiclesDeps);
                }
                // ISSUE: reference to a compiler-generated method
                this.SpawnGarbageTrucks(entity, temp, isTemp, parkingLocations, deletedVehicleMap, ref parkingLocationDeps, ref deletedVehiclesDeps);
              }
              if (parkingLocations.IsCreated)
              {
                parkingLocations.Dispose(parkingLocationDeps);
                job0 = JobHandle.CombineDependencies(job0, parkingLocationDeps);
              }
            }
          }
        }
        if (deletedVehicleMap.IsCreated)
          deletedVehicleMap.Dispose(deletedVehiclesDeps);
        this.Dependency = job0;
      }
      finally
      {
        archetypeChunkArray.Dispose();
      }
    }

    private void FindParkingLocations(
      Entity entity,
      ref NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      JobHandle inputDeps,
      ref JobHandle parkingLocationDeps)
    {
      if (parkingLocations.IsCreated)
        return;
      parkingLocations = new NativeList<ParkedVehiclesSystem.ParkingLocation>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle = new ParkedVehiclesSystem.FindParkingLocationsJob()
      {
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_ParkingLaneData = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabParkingLaneData = this.__TypeHandle.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup,
        m_SpawnLocationElements = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RO_BufferLookup,
        m_LaneOverlaps = this.__TypeHandle.__Game_Net_LaneOverlap_RO_BufferLookup,
        m_Entity = entity,
        m_Locations = parkingLocations
      }.Schedule<ParkedVehiclesSystem.FindParkingLocationsJob>(inputDeps);
      parkingLocationDeps = jobHandle;
    }

    private void CollectDeletedVehicles(
      ref NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      JobHandle inputDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      if (deletedVehicleMap.IsCreated)
        return;
      deletedVehicleMap = new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>(0, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      ParkedVehiclesSystem.CollectDeletedVehiclesJob jobData = new ParkedVehiclesSystem.CollectDeletedVehiclesJob()
      {
        m_Chunks = this.m_DeletedVehicleQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_DeletedVehicleMap = deletedVehicleMap
      };
      JobHandle inputDeps1 = jobData.Schedule<ParkedVehiclesSystem.CollectDeletedVehiclesJob>(JobHandle.CombineDependencies(inputDeps, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      jobData.m_Chunks.Dispose(inputDeps1);
      deletedVehiclesDeps = inputDeps1;
    }

    private void DuplicateVehicles(
      Entity entity,
      Temp temp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MovingObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle producerJob = new ParkedVehiclesSystem.DuplicateVehiclesJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_HelicopterData = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_TrainData = this.__TypeHandle.__Game_Vehicles_Train_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_PrefabMovingObjectData = this.__TypeHandle.__Game_Prefabs_MovingObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_PrefabTrainObjectData = this.__TypeHandle.__Game_Prefabs_TrainObjectData_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = deletedVehicleMap,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.DuplicateVehiclesJob>(JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps));
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
      parkingLocationDeps = producerJob;
      deletedVehiclesDeps = producerJob;
    }

    private void SpawnPoliceCars(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceCarSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_PoliceCarQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnPoliceCarsJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabPoliceStationData = this.__TypeHandle.__Game_Prefabs_PoliceStationData_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_PoliceCarSelectData = this.m_PoliceCarSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.SpawnPoliceCarsJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceCarSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private void SpawnFireEngines(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_FireEngineSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_FireEngineQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_FireStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnFireEnginesJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabFireStationData = this.__TypeHandle.__Game_Prefabs_FireStationData_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_FireEngineSelectData = this.m_FireEngineSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.SpawnFireEnginesJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_FireEngineSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private void SpawnHealthcareVehicles(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_HealthcareVehicleQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnHealthcareVehiclesJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabHospitalData = this.__TypeHandle.__Game_Prefabs_HospitalData_RO_ComponentLookup,
        m_PrefabDeathcareFacilityData = this.__TypeHandle.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_HealthcareVehicleSelectData = this.m_HealthcareVehicleSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.SpawnHealthcareVehiclesJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_HealthcareVehicleSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private void SpawnTransportVehicles(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_TransportVehicleQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EmergencyShelterData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnTransportVehiclesJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
        m_PrefabPrisonData = this.__TypeHandle.__Game_Prefabs_PrisonData_RO_ComponentLookup,
        m_PrefabEmergencyShelterData = this.__TypeHandle.__Game_Prefabs_EmergencyShelterData_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_TransportVehicleSelectData = this.m_TransportVehicleSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      }.Schedule<ParkedVehiclesSystem.SpawnTransportVehiclesJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private void SpawnPostVans(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_PostVanQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnPostVansJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabPostFacilityData = this.__TypeHandle.__Game_Prefabs_PostFacilityData_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_PostVanSelectData = this.m_PostVanSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.SpawnPostVansJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_PostVanSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private void SpawnMaintenanceVehicles(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_MaintenanceVehicleQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnMaintenanceVehiclesJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabMaintenanceDepotData = this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_MaintenanceVehicleSelectData = this.m_MaintenanceVehicleSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.SpawnMaintenanceVehiclesJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceVehicleSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private void SpawnGarbageTrucks(
      Entity entity,
      Temp temp,
      bool isTemp,
      NativeList<ParkedVehiclesSystem.ParkingLocation> parkingLocations,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedVehicleMap,
      ref JobHandle parkingLocationDeps,
      ref JobHandle deletedVehiclesDeps)
    {
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_GarbageTruckQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle jobHandle2 = new ParkedVehiclesSystem.SpawnGarbageTrucksJob()
      {
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabGarbageFacilityData = this.__TypeHandle.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup,
        m_Entity = entity,
        m_Temp = temp,
        m_IsTemp = isTemp,
        m_GarbageTruckSelectData = this.m_GarbageTruckSelectData,
        m_Locations = parkingLocations,
        m_DeletedVehicleMap = (isTemp ? deletedVehicleMap : new NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData>()),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ParkedVehiclesSystem.SpawnGarbageTrucksJob>(isTemp ? JobHandle.CombineDependencies(parkingLocationDeps, deletedVehiclesDeps, jobHandle1) : JobHandle.CombineDependencies(parkingLocationDeps, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_GarbageTruckSelectData.PostUpdate(jobHandle2);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle2);
      parkingLocationDeps = jobHandle2;
      if (!isTemp)
        return;
      deletedVehiclesDeps = jobHandle2;
    }

    private static Entity GetSecondaryPrefab(
      Entity primaryPrefab,
      DynamicBuffer<LayoutElement> layoutElements,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<PrefabData> prefabDatas,
      out bool validLayout)
    {
      Entity prefab = Entity.Null;
      validLayout = true;
      for (int index = 0; index < layoutElements.Length; ++index)
      {
        Entity vehicle = layoutElements[index].m_Vehicle;
        PrefabRef componentData;
        if (prefabRefs.TryGetComponent(vehicle, out componentData) && componentData.m_Prefab != primaryPrefab)
        {
          if (prefab == Entity.Null)
            prefab = componentData.m_Prefab;
          validLayout &= prefabDatas.HasEnabledComponent<PrefabData>(componentData.m_Prefab);
        }
      }
      return prefab;
    }

    private static float4 GetMaxVehicleSize(
      NativeList<ParkedVehiclesSystem.ParkingLocation> locations,
      RoadTypes roadType)
    {
      float4 a = (float4) 0.0f;
      for (int index = 0; index < locations.Length; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        ParkedVehiclesSystem.ParkingLocation location = locations[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        a = math.select(a, location.m_MaxSize.xyxy, location.m_MaxSize.xxyy > a.xxww & (location.m_ParkingLaneData.m_RoadTypes & roadType) != 0);
      }
      return a;
    }

    private static bool SelectParkingSpace(
      ref Random random,
      NativeList<ParkedVehiclesSystem.ParkingLocation> locations,
      ObjectGeometryData objectGeometryData,
      RoadTypes roadType,
      TrackTypes trackType,
      out Transform transform,
      out Entity lane,
      out float curvePosition)
    {
      float offset;
      float2 parkingSize = VehicleUtils.GetParkingSize(objectGeometryData, out offset);
      int max = 0;
      int index1 = -1;
      for (int index2 = 0; index2 < locations.Length; ++index2)
      {
        // ISSUE: variable of a compiler-generated type
        ParkedVehiclesSystem.ParkingLocation location = locations[index2];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!math.any(parkingSize > location.m_MaxSize) && ((location.m_ParkingLaneData.m_RoadTypes & roadType) != RoadTypes.None || (location.m_TrackTypes & trackType) != TrackTypes.None))
        {
          int num = 100;
          max += num;
          if (random.NextInt(max) < num)
            index1 = index2;
        }
      }
      if (index1 != -1)
      {
        // ISSUE: variable of a compiler-generated type
        ParkedVehiclesSystem.ParkingLocation location = locations[index1];
        // ISSUE: reference to a compiler-generated field
        lane = location.m_Lane;
        // ISSUE: reference to a compiler-generated field
        curvePosition = location.m_CurvePos;
        // ISSUE: reference to a compiler-generated field
        if (location.m_SpawnLocationType == SpawnLocationType.ParkingLane)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) location.m_ParkingLaneData.m_SlotAngle <= 0.25)
          {
            if ((double) offset > 0.0)
            {
              Bounds1 t = new Bounds1(curvePosition, 1f);
              // ISSUE: reference to a compiler-generated field
              MathUtils.ClampLength(location.m_Curve.m_Bezier, ref t, offset);
              curvePosition = t.max;
            }
            else if ((double) offset < 0.0)
            {
              Bounds1 t = new Bounds1(0.0f, curvePosition);
              // ISSUE: reference to a compiler-generated field
              MathUtils.ClampLengthInverse(location.m_Curve.m_Bezier, ref t, -offset);
              curvePosition = t.min;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          transform = VehicleUtils.CalculateParkingSpaceTarget(location.m_ParkingLane, location.m_ParkingLaneData, objectGeometryData, location.m_Curve, location.m_OwnerTransform, curvePosition);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          transform = location.m_OwnerTransform;
        }
        locations.RemoveAtSwapBack(index1);
        return true;
      }
      transform = new Transform();
      lane = Entity.Null;
      curvePosition = 0.0f;
      return false;
    }

    private static Entity FindDeletedVehicle(
      Entity primaryPrefab,
      Entity secondaryPrefab,
      Transform transform,
      NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> deletedMap)
    {
      Entity entity = Entity.Null;
      // ISSUE: variable of a compiler-generated type
      ParkedVehiclesSystem.DeletedVehicleData deletedVehicleData;
      NativeParallelMultiHashMapIterator<Entity> it1;
      if (deletedMap.IsCreated && deletedMap.TryGetFirstValue(primaryPrefab, out deletedVehicleData, out it1))
      {
        float num1 = float.MaxValue;
        NativeParallelMultiHashMapIterator<Entity> it2 = new NativeParallelMultiHashMapIterator<Entity>();
        do
        {
          // ISSUE: reference to a compiler-generated field
          if (!(deletedVehicleData.m_SecondaryPrefab != secondaryPrefab))
          {
            // ISSUE: reference to a compiler-generated field
            float num2 = math.distance(deletedVehicleData.m_Transform.m_Position, transform.m_Position);
            if ((double) num2 < (double) num1)
            {
              // ISSUE: reference to a compiler-generated field
              entity = deletedVehicleData.m_Entity;
              num1 = num2;
              it2 = it1;
            }
          }
        }
        while (deletedMap.TryGetNextValue(out deletedVehicleData, ref it1));
        if (entity != Entity.Null)
          deletedMap.Remove(it2);
      }
      return entity;
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
    public ParkedVehiclesSystem()
    {
    }

    private struct ParkingLocation
    {
      public Game.Net.ParkingLane m_ParkingLane;
      public ParkingLaneData m_ParkingLaneData;
      public TrackTypes m_TrackTypes;
      public SpawnLocationType m_SpawnLocationType;
      public Transform m_OwnerTransform;
      public Curve m_Curve;
      public Entity m_Lane;
      public float2 m_MaxSize;
      public float m_CurvePos;
    }

    private struct DeletedVehicleData
    {
      public Entity m_Entity;
      public Entity m_SecondaryPrefab;
      public Transform m_Transform;
    }

    [BurstCompile]
    private struct FindParkingLocationsJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> m_ParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> m_PrefabParkingLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> m_SpawnLocationElements;
      [ReadOnly]
      public BufferLookup<LaneOverlap> m_LaneOverlaps;
      [ReadOnly]
      public Entity m_Entity;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;

      public void Execute()
      {
        DynamicBuffer<SpawnLocationElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SpawnLocationElements.TryGetBuffer(this.m_Entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          SpawnLocationElement spawnLocationElement = bufferData[index];
          switch (spawnLocationElement.m_Type)
          {
            case SpawnLocationType.SpawnLocation:
              PrefabRef componentData1;
              Game.Prefabs.SpawnLocationData componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.TryGetComponent(spawnLocationElement.m_SpawnLocation, out componentData1) && this.m_PrefabSpawnLocationData.TryGetComponent(componentData1.m_Prefab, out componentData2))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckSpawnLocation(spawnLocationElement.m_SpawnLocation, componentData2);
                break;
              }
              break;
            case SpawnLocationType.ParkingLane:
              Game.Net.ParkingLane componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkingLaneData.TryGetComponent(spawnLocationElement.m_SpawnLocation, out componentData3))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckParkingLane(spawnLocationElement.m_SpawnLocation, componentData3);
                break;
              }
              break;
          }
        }
      }

      private void CheckSpawnLocation(Entity entity, Game.Prefabs.SpawnLocationData spawnLocationData)
      {
        if (((spawnLocationData.m_RoadTypes & RoadTypes.Helicopter) == RoadTypes.None || spawnLocationData.m_ConnectionType != RouteConnectionType.Air) && spawnLocationData.m_ConnectionType != RouteConnectionType.Track)
          return;
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_Locations.Add(new ParkedVehiclesSystem.ParkingLocation()
        {
          m_ParkingLaneData = new ParkingLaneData()
          {
            m_RoadTypes = spawnLocationData.m_RoadTypes
          },
          m_TrackTypes = spawnLocationData.m_TrackTypes,
          m_SpawnLocationType = SpawnLocationType.SpawnLocation,
          m_OwnerTransform = transform,
          m_Lane = entity,
          m_MaxSize = (float2) float.MaxValue,
          m_CurvePos = 0.0f
        });
      }

      private void CheckParkingLane(Entity lane, Game.Net.ParkingLane parkingLane)
      {
        if ((parkingLane.m_Flags & (ParkingLaneFlags.VirtualLane | ParkingLaneFlags.SpecialVehicles)) != ParkingLaneFlags.SpecialVehicles)
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[lane];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[lane];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LaneOverlap> laneOverlap1 = this.m_LaneOverlaps[lane];
        // ISSUE: reference to a compiler-generated field
        ParkingLaneData parkingLaneData = this.m_PrefabParkingLaneData[prefabRef.m_Prefab];
        float2 parkingSize = VehicleUtils.GetParkingSize(parkingLaneData);
        Transform componentData1 = new Transform();
        Owner componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_OwnerData.TryGetComponent(lane, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TransformData.TryGetComponent(componentData2.m_Owner, out componentData1);
        }
        if ((double) parkingLaneData.m_SlotInterval == 0.0)
          return;
        int parkingSlotCount = NetUtils.GetParkingSlotCount(curve, parkingLane, parkingLaneData);
        float parkingSlotInterval = NetUtils.GetParkingSlotInterval(curve, parkingLane, parkingLaneData, parkingSlotCount);
        float3 x1 = curve.m_Bezier.a;
        float2 float2_1 = (float2) 0.0f;
        float num1 = 0.0f;
        float x2;
        switch (parkingLane.m_Flags & (ParkingLaneFlags.StartingLane | ParkingLaneFlags.EndingLane))
        {
          case ParkingLaneFlags.StartingLane:
            x2 = curve.m_Length - (float) parkingSlotCount * parkingSlotInterval;
            break;
          case ParkingLaneFlags.EndingLane:
            x2 = 0.0f;
            break;
          default:
            x2 = (float) (((double) curve.m_Length - (double) parkingSlotCount * (double) parkingSlotInterval) * 0.5);
            break;
        }
        float num2 = math.max(x2, 0.0f);
        int num3 = -1;
        float2 float2_2 = (float2) 2f;
        int num4 = 0;
        if (num4 < laneOverlap1.Length)
        {
          LaneOverlap laneOverlap2 = laneOverlap1[num4++];
          float2_2 = new float2((float) laneOverlap2.m_ThisStart, (float) laneOverlap2.m_ThisEnd) * 0.003921569f;
        }
        for (int index = 1; index <= 16; ++index)
        {
          float num5 = (float) index * (1f / 16f);
          float3 y = MathUtils.Position(curve.m_Bezier, num5);
          for (num1 += math.distance(x1, y); (double) num1 >= (double) num2 || index == 16 && num3 < parkingSlotCount; ++num3)
          {
            float2_1.y = math.select(num5, math.lerp(float2_1.x, num5, num2 / num1), (double) num2 < (double) num1);
            bool flag = false;
            if ((double) float2_2.x < (double) float2_1.y)
            {
              flag = true;
              if ((double) float2_2.y <= (double) float2_1.y)
              {
                float2_2 = (float2) 2f;
                while (num4 < laneOverlap1.Length)
                {
                  LaneOverlap laneOverlap3 = laneOverlap1[num4++];
                  float2 float2_3 = new float2((float) laneOverlap3.m_ThisStart, (float) laneOverlap3.m_ThisEnd) * 0.003921569f;
                  if ((double) float2_3.y > (double) float2_1.y)
                  {
                    float2_2 = float2_3;
                    break;
                  }
                }
              }
            }
            if (!flag && num3 >= 0 && num3 < parkingSlotCount)
            {
              float num6 = math.lerp(float2_1.x, float2_1.y, 0.5f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_Locations.Add(new ParkedVehiclesSystem.ParkingLocation()
              {
                m_ParkingLane = parkingLane,
                m_ParkingLaneData = parkingLaneData,
                m_SpawnLocationType = SpawnLocationType.ParkingLane,
                m_OwnerTransform = componentData1,
                m_Curve = curve,
                m_Lane = lane,
                m_MaxSize = parkingSize,
                m_CurvePos = num6
              });
            }
            num1 -= num2;
            float2_1.x = float2_1.y;
            num2 = parkingSlotInterval;
          }
          x1 = y;
        }
      }
    }

    [BurstCompile]
    private struct CollectDeletedVehiclesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;

      public void Execute()
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_Chunks[index].Count;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_DeletedVehicleMap.Capacity = num;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Controller> nativeArray3 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            Transform transform = nativeArray2[index2];
            PrefabRef prefabRef = nativeArray4[index2];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            ParkedVehiclesSystem.DeletedVehicleData deletedVehicleData = new ParkedVehiclesSystem.DeletedVehicleData()
            {
              m_Entity = entity,
              m_Transform = transform
            };
            Controller controller;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabData.HasEnabledComponent<PrefabData>(prefabRef.m_Prefab) && (!CollectionUtils.TryGet<Controller>(nativeArray3, index2, out controller) || !(controller.m_Controller != entity)))
            {
              DynamicBuffer<LayoutElement> layoutElements;
              if (CollectionUtils.TryGet<LayoutElement>(bufferAccessor, index2, out layoutElements))
              {
                bool validLayout;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                deletedVehicleData.m_SecondaryPrefab = ParkedVehiclesSystem.GetSecondaryPrefab(prefabRef.m_Prefab, layoutElements, ref this.m_PrefabRefData, ref this.m_PrefabData, out validLayout);
                if (!validLayout)
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_DeletedVehicleMap.Add(prefabRef.m_Prefab, deletedVehicleData);
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct DuplicateVehiclesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Helicopter> m_HelicopterData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<MovingObjectData> m_PrefabMovingObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public ComponentLookup<TrainObjectData> m_PrefabTrainObjectData;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Transform parentTransform = this.m_TransformData[this.m_Entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Transform inverseParentTransform = ObjectUtils.InverseTransform(this.m_TransformData[this.m_Temp.m_Original]);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<OwnedVehicle> ownedVehicle1 = this.m_OwnedVehicles[this.m_Temp.m_Original];
        NativeList<LayoutElement> nativeList = new NativeList<LayoutElement>();
        for (int index1 = 0; index1 < ownedVehicle1.Length; ++index1)
        {
          OwnedVehicle ownedVehicle2 = ownedVehicle1[index1];
          // ISSUE: reference to a compiler-generated field
          bool flag1 = this.m_ParkedCarData.HasComponent(ownedVehicle2.m_Vehicle);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = this.m_ParkedTrainData.HasComponent(ownedVehicle2.m_Vehicle);
          if (flag1 || flag2)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef component1 = this.m_PrefabRefData[ownedVehicle2.m_Vehicle];
            ObjectGeometryData componentData1;
            MovingObjectData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabObjectGeometryData.TryGetComponent(component1.m_Prefab, out componentData1) && this.m_PrefabMovingObjectData.TryGetComponent(component1.m_Prefab, out componentData2) && this.m_PrefabData.IsComponentEnabled(component1.m_Prefab))
            {
              Entity secondaryPrefab = Entity.Null;
              DynamicBuffer<LayoutElement> bufferData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LayoutElements.TryGetBuffer(ownedVehicle2.m_Vehicle, out bufferData1))
              {
                bool validLayout;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                secondaryPrefab = ParkedVehiclesSystem.GetSecondaryPrefab(component1.m_Prefab, bufferData1, ref this.m_PrefabRefData, ref this.m_PrefabData, out validLayout);
                if (!validLayout)
                  continue;
              }
              NativeArray<LayoutElement> v = new NativeArray<LayoutElement>();
              // ISSUE: reference to a compiler-generated field
              Transform local = ObjectUtils.WorldToLocal(inverseParentTransform, this.m_TransformData[ownedVehicle2.m_Vehicle]);
              Transform world = ObjectUtils.LocalToWorld(parentTransform, local);
              // ISSUE: reference to a compiler-generated field
              bool flag3 = (this.m_Temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) > (TempFlags) 0;
              Entity lane = Entity.Null;
              float curvePosition = 0.0f;
              if (!flag3)
              {
                RoadTypes roadType = RoadTypes.None;
                TrackTypes trackType = TrackTypes.None;
                // ISSUE: reference to a compiler-generated field
                if (this.m_HelicopterData.HasComponent(ownedVehicle2.m_Vehicle))
                {
                  roadType = RoadTypes.Helicopter;
                }
                else
                {
                  TrainData componentData3;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabTrainData.TryGetComponent(component1.m_Prefab, out componentData3))
                    trackType = componentData3.m_TrackType;
                  else
                    roadType = RoadTypes.Car;
                }
                // ISSUE: reference to a compiler-generated method
                this.SelectParkingSpace(componentData1, roadType, trackType, ref world, out lane, out curvePosition);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              Entity entity1 = ParkedVehiclesSystem.FindDeletedVehicle(component1.m_Prefab, secondaryPrefab, world, this.m_DeletedVehicleMap);
              DynamicBuffer<LayoutElement> bufferData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if ((this.m_LayoutElements.TryGetBuffer(entity1, out bufferData2) && bufferData2.Length != 0 || bufferData1.IsCreated && bufferData1.Length != 0) && !this.AreEqual(entity1, ownedVehicle2.m_Vehicle, bufferData2, bufferData1))
                entity1 = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (flag1 && this.m_ParkedCarData.HasComponent(entity1) || flag2 && this.m_ParkedTrainData.HasComponent(entity1))
              {
                if (bufferData2.IsCreated && bufferData2.Length != 0)
                {
                  v = bufferData2.AsNativeArray();
                  for (int index2 = 0; index2 < bufferData2.Length; ++index2)
                  {
                    Entity vehicle = bufferData2[index2].m_Vehicle;
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.RemoveComponent<Deleted>(vehicle);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(vehicle);
                  }
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<Deleted>(entity1);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(entity1);
                }
              }
              else if (bufferData1.IsCreated && bufferData1.Length != 0)
              {
                if (!nativeList.IsCreated)
                  nativeList = new NativeList<LayoutElement>(bufferData1.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                for (int index3 = 0; index3 < bufferData1.Length; ++index3)
                {
                  Entity vehicle = bufferData1[index3].m_Vehicle;
                  // ISSUE: reference to a compiler-generated field
                  PrefabRef component2 = this.m_PrefabRefData[vehicle];
                  Entity entity2;
                  if (vehicle == ownedVehicle2.m_Vehicle)
                  {
                    TrainObjectData componentData4;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    entity2 = !this.m_PrefabTrainObjectData.TryGetComponent(component2.m_Prefab, out componentData4) ? this.m_CommandBuffer.CreateEntity(componentData2.m_StoppedArchetype) : this.m_CommandBuffer.CreateEntity(componentData4.m_StoppedControllerArchetype);
                    entity1 = entity2;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    entity2 = this.m_CommandBuffer.CreateEntity(this.m_PrefabMovingObjectData[component2.m_Prefab].m_StoppedArchetype);
                  }
                  nativeList.Add(new LayoutElement(entity2));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PrefabRef>(entity2, component2);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Animation>(entity2, new Animation());
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<InterpolatedTransform>(entity2, new InterpolatedTransform());
                }
                v = nativeList.AsArray();
              }
              else
              {
                TrainObjectData componentData5;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                entity1 = !this.m_PrefabTrainObjectData.TryGetComponent(component1.m_Prefab, out componentData5) ? this.m_CommandBuffer.CreateEntity(componentData2.m_StoppedArchetype) : this.m_CommandBuffer.CreateEntity(componentData5.m_StoppedControllerArchetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<PrefabRef>(entity1, component1);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Animation>(entity1, new Animation());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<InterpolatedTransform>(entity1, new InterpolatedTransform());
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Owner>(entity1, new Owner(this.m_Entity));
              Temp component3 = new Temp();
              // ISSUE: reference to a compiler-generated field
              component3.m_Flags = flag3 || !(lane == Entity.Null) ? this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate) : TempFlags.Delete | TempFlags.Hidden;
              // ISSUE: reference to a compiler-generated field
              if (flag3 && this.m_UnspawnedData.HasComponent(ownedVehicle2.m_Vehicle))
                component3.m_Flags |= TempFlags.Hidden;
              if (v.IsCreated)
              {
                for (int index4 = 0; index4 < v.Length; ++index4)
                {
                  Entity vehicle = v[index4].m_Vehicle;
                  component3.m_Original = bufferData1[index4].m_Vehicle;
                  if (flag1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<ParkedCar>(vehicle, new ParkedCar(lane, curvePosition));
                  }
                  if (flag2)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<ParkedTrain>(vehicle, new ParkedTrain(lane));
                    Train componentData6;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TrainData.TryGetComponent(component3.m_Original, out componentData6))
                    {
                      componentData6.m_Flags &= TrainFlags.Reversed;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.SetComponent<Train>(vehicle, componentData6);
                    }
                  }
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Temp>(vehicle, component3);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Transform>(vehicle, world);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(vehicle, this.m_PseudoRandomSeedData[component3.m_Original]);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Hidden>(component3.m_Original, new Hidden());
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<BatchesUpdated>(component3.m_Original, new BatchesUpdated());
                }
              }
              else
              {
                component3.m_Original = ownedVehicle2.m_Vehicle;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<ParkedCar>(entity1, new ParkedCar(lane, curvePosition));
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Temp>(entity1, component3);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Transform>(entity1, world);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<PseudoRandomSeed>(entity1, this.m_PseudoRandomSeedData[component3.m_Original]);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Hidden>(component3.m_Original, new Hidden());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<BatchesUpdated>(component3.m_Original, new BatchesUpdated());
              }
              if (nativeList.IsCreated && nativeList.Length != 0)
              {
                for (int index5 = 0; index5 < v.Length; ++index5)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ControllerData.HasComponent(bufferData1[index5].m_Vehicle))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<Controller>(v[index5].m_Vehicle, new Controller(entity1));
                  }
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetBuffer<LayoutElement>(entity1).CopyFrom(v);
                nativeList.Clear();
              }
            }
          }
        }
        if (!nativeList.IsCreated)
          return;
        nativeList.Dispose();
      }

      private bool AreEqual(
        Entity entity1,
        Entity entity2,
        DynamicBuffer<LayoutElement> layout1,
        DynamicBuffer<LayoutElement> layout2)
      {
        if (!layout1.IsCreated || !layout2.IsCreated || layout1.Length != layout2.Length)
          return false;
        for (int index = 0; index < layout1.Length; ++index)
        {
          Entity vehicle1 = layout1[index].m_Vehicle;
          Entity vehicle2 = layout2[index].m_Vehicle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (vehicle1 == entity1 != (vehicle2 == entity2) || this.m_PrefabRefData[vehicle1].m_Prefab != this.m_PrefabRefData[vehicle2].m_Prefab)
            return false;
        }
        return true;
      }

      private bool SelectParkingSpace(
        ObjectGeometryData objectGeometryData,
        RoadTypes roadType,
        TrackTypes trackType,
        ref Transform transform,
        out Entity lane,
        out float curvePosition)
      {
        float offset;
        float2 parkingSize = VehicleUtils.GetParkingSize(objectGeometryData, out offset);
        Transform transform1 = new Transform();
        float num1 = float.MaxValue;
        int index1 = -1;
        // ISSUE: reference to a compiler-generated field
        for (int index2 = 0; index2 < this.m_Locations.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ParkedVehiclesSystem.ParkingLocation location = this.m_Locations[index2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!math.any(parkingSize > location.m_MaxSize) && ((location.m_ParkingLaneData.m_RoadTypes & roadType) != RoadTypes.None || (location.m_TrackTypes & trackType) != TrackTypes.None))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Transform transform2 = location.m_SpawnLocationType != SpawnLocationType.ParkingLane ? location.m_OwnerTransform : VehicleUtils.CalculateParkingSpaceTarget(location.m_ParkingLane, location.m_ParkingLaneData, objectGeometryData, location.m_Curve, location.m_OwnerTransform, location.m_CurvePos);
            float num2 = math.distancesq(transform.m_Position, transform2.m_Position);
            if ((double) num2 < (double) num1)
            {
              transform1 = transform2;
              num1 = num2;
              index1 = index2;
            }
          }
        }
        if (index1 != -1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ParkedVehiclesSystem.ParkingLocation location = this.m_Locations[index1];
          transform = transform1;
          // ISSUE: reference to a compiler-generated field
          lane = location.m_Lane;
          // ISSUE: reference to a compiler-generated field
          curvePosition = location.m_CurvePos;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (location.m_SpawnLocationType == SpawnLocationType.ParkingLane && (double) location.m_ParkingLaneData.m_SlotAngle <= 0.25)
          {
            if ((double) offset > 0.0)
            {
              Bounds1 t = new Bounds1(curvePosition, 1f);
              // ISSUE: reference to a compiler-generated field
              MathUtils.ClampLength(location.m_Curve.m_Bezier, ref t, offset);
              curvePosition = t.max;
            }
            else if ((double) offset < 0.0)
            {
              Bounds1 t = new Bounds1(0.0f, curvePosition);
              // ISSUE: reference to a compiler-generated field
              MathUtils.ClampLengthInverse(location.m_Curve.m_Bezier, ref t, -offset);
              curvePosition = t.min;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            transform = VehicleUtils.CalculateParkingSpaceTarget(location.m_ParkingLane, location.m_ParkingLaneData, objectGeometryData, location.m_Curve, location.m_OwnerTransform, curvePosition);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_Locations.RemoveAtSwapBack(index1);
          return true;
        }
        transform = new Transform();
        lane = Entity.Null;
        curvePosition = 0.0f;
        return false;
      }
    }

    [BurstCompile]
    private struct SpawnPoliceCarsJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<PoliceStationData> m_PrefabPoliceStationData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public PoliceCarSelectData m_PoliceCarSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        PoliceStationData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<PoliceStationData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabPoliceStationData, ref this.m_InstalledUpgrades);
        PoliceStationData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null && UpgradeUtils.TryGetCombinedComponent<PoliceStationData>(this.m_Temp.m_Original, out data2, ref this.m_PrefabRefData, ref this.m_PrefabPoliceStationData, ref this.m_InstalledUpgrades))
        {
          data1.m_PatrolCarCapacity -= data2.m_PatrolCarCapacity;
          data1.m_PoliceHelicopterCapacity -= data2.m_PoliceHelicopterCapacity;
        }
        for (int index = 0; index < data1.m_PatrolCarCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, data1, RoadTypes.Car);
        }
        for (int index = 0; index < data1.m_PoliceHelicopterCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, data1, RoadTypes.Helicopter);
        }
      }

      private void CreateVehicle(
        ref Random random,
        PoliceStationData policeStationData,
        RoadTypes roadType)
      {
        PolicePurpose purposeMask = policeStationData.m_PurposeMask;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_PoliceCarSelectData.SelectVehicle(ref random, ref purposeMask, roadType);
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, TrackTypes.None, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity e = ParkedVehiclesSystem.FindDeletedVehicle(entity, Entity.Null, transform, this.m_DeletedVehicleMap);
        if (e != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(e, transform);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(e);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          e = this.m_PoliceCarSelectData.CreateVehicle(this.m_CommandBuffer, ref random, transform, this.m_Entity, entity, ref purposeMask, roadType, true);
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Animation>(e, new Animation());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(e, new InterpolatedTransform());
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PoliceCar>(e, new PoliceCar(PoliceCarFlags.Empty | PoliceCarFlags.Disabled, 0, policeStationData.m_PurposeMask & purposeMask));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(e, new ParkedCar(lane, curvePosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(e, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IsTemp)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(e, new Temp()
        {
          m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
        });
      }
    }

    [BurstCompile]
    private struct SpawnFireEnginesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<FireStationData> m_PrefabFireStationData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public FireEngineSelectData m_FireEngineSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        FireStationData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<FireStationData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabFireStationData, ref this.m_InstalledUpgrades);
        FireStationData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null && UpgradeUtils.TryGetCombinedComponent<FireStationData>(this.m_Temp.m_Original, out data2, ref this.m_PrefabRefData, ref this.m_PrefabFireStationData, ref this.m_InstalledUpgrades))
        {
          data1.m_FireEngineCapacity -= data2.m_FireEngineCapacity;
          data1.m_FireHelicopterCapacity -= data2.m_FireHelicopterCapacity;
          data1.m_DisasterResponseCapacity -= data2.m_DisasterResponseCapacity;
        }
        for (int index = 0; index < data1.m_FireEngineCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, ref data1, RoadTypes.Car);
        }
        for (int index = 0; index < data1.m_FireHelicopterCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, ref data1, RoadTypes.Helicopter);
        }
      }

      private void CreateVehicle(
        ref Random random,
        ref FireStationData fireStationData,
        RoadTypes roadType)
      {
        float2 extinguishingCapacity = new float2(float.Epsilon, float.MaxValue);
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_FireEngineSelectData.SelectVehicle(ref random, ref extinguishingCapacity, roadType);
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, TrackTypes.None, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity e = ParkedVehiclesSystem.FindDeletedVehicle(entity, Entity.Null, transform, this.m_DeletedVehicleMap);
        if (e != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(e, transform);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(e);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          e = this.m_FireEngineSelectData.CreateVehicle(this.m_CommandBuffer, ref random, transform, this.m_Entity, entity, ref extinguishingCapacity, roadType, true);
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Animation>(e, new Animation());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(e, new InterpolatedTransform());
          }
        }
        FireEngineFlags state = FireEngineFlags.Disabled;
        if (fireStationData.m_DisasterResponseCapacity > 0)
        {
          state |= FireEngineFlags.DisasterResponse;
          --fireStationData.m_DisasterResponseCapacity;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<FireEngine>(e, new FireEngine(state, 0, extinguishingCapacity.y, fireStationData.m_VehicleEfficiency));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(e, new ParkedCar(lane, curvePosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(e, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IsTemp)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(e, new Temp()
        {
          m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
        });
      }
    }

    [BurstCompile]
    private struct SpawnHealthcareVehiclesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<HospitalData> m_PrefabHospitalData;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> m_PrefabDeathcareFacilityData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public HealthcareVehicleSelectData m_HealthcareVehicleSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        HospitalData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<HospitalData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabHospitalData, ref this.m_InstalledUpgrades);
        DeathcareFacilityData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<DeathcareFacilityData>(this.m_Entity, out data2, ref this.m_PrefabRefData, ref this.m_PrefabDeathcareFacilityData, ref this.m_InstalledUpgrades);
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null)
        {
          HospitalData data3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (UpgradeUtils.TryGetCombinedComponent<HospitalData>(this.m_Temp.m_Original, out data3, ref this.m_PrefabRefData, ref this.m_PrefabHospitalData, ref this.m_InstalledUpgrades))
          {
            data1.m_AmbulanceCapacity -= data3.m_AmbulanceCapacity;
            data1.m_MedicalHelicopterCapacity -= data3.m_MedicalHelicopterCapacity;
          }
          DeathcareFacilityData data4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (UpgradeUtils.TryGetCombinedComponent<DeathcareFacilityData>(this.m_Temp.m_Original, out data4, ref this.m_PrefabRefData, ref this.m_PrefabDeathcareFacilityData, ref this.m_InstalledUpgrades))
            data2.m_HearseCapacity -= data4.m_HearseCapacity;
        }
        for (int index = 0; index < data1.m_AmbulanceCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, HealthcareRequestType.Ambulance, RoadTypes.Car);
        }
        for (int index = 0; index < data2.m_HearseCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, HealthcareRequestType.Hearse, RoadTypes.Car);
        }
        for (int index = 0; index < data1.m_MedicalHelicopterCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, HealthcareRequestType.Ambulance, RoadTypes.Helicopter);
        }
      }

      private void CreateVehicle(
        ref Random random,
        HealthcareRequestType healthcareType,
        RoadTypes roadType)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_HealthcareVehicleSelectData.SelectVehicle(ref random, healthcareType, roadType);
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, TrackTypes.None, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity e = ParkedVehiclesSystem.FindDeletedVehicle(entity, Entity.Null, transform, this.m_DeletedVehicleMap);
        if (e != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(e, transform);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(e);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          e = this.m_HealthcareVehicleSelectData.CreateVehicle(this.m_CommandBuffer, ref random, transform, this.m_Entity, entity, healthcareType, roadType, true);
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Animation>(e, new Animation());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(e, new InterpolatedTransform());
          }
        }
        if (healthcareType == HealthcareRequestType.Ambulance)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Ambulance>(e, new Ambulance(Entity.Null, Entity.Null, AmbulanceFlags.Disabled));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Hearse>(e, new Hearse(Entity.Null, HearseFlags.Disabled));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(e, new ParkedCar(lane, curvePosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(e, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IsTemp)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(e, new Temp()
        {
          m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
        });
      }
    }

    [BurstCompile]
    private struct SpawnTransportVehiclesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      [ReadOnly]
      public ComponentLookup<PrisonData> m_PrefabPrisonData;
      [ReadOnly]
      public ComponentLookup<EmergencyShelterData> m_PrefabEmergencyShelterData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public TransportVehicleSelectData m_TransportVehicleSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        TransportDepotData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<TransportDepotData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabTransportDepotData, ref this.m_InstalledUpgrades);
        PrisonData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<PrisonData>(this.m_Entity, out data2, ref this.m_PrefabRefData, ref this.m_PrefabPrisonData, ref this.m_InstalledUpgrades);
        EmergencyShelterData data3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<EmergencyShelterData>(this.m_Entity, out data3, ref this.m_PrefabRefData, ref this.m_PrefabEmergencyShelterData, ref this.m_InstalledUpgrades);
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null)
        {
          TransportDepotData data4;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (UpgradeUtils.TryGetCombinedComponent<TransportDepotData>(this.m_Temp.m_Original, out data4, ref this.m_PrefabRefData, ref this.m_PrefabTransportDepotData, ref this.m_InstalledUpgrades))
            data1.m_VehicleCapacity -= data4.m_VehicleCapacity;
          PrisonData data5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (UpgradeUtils.TryGetCombinedComponent<PrisonData>(this.m_Temp.m_Original, out data5, ref this.m_PrefabRefData, ref this.m_PrefabPrisonData, ref this.m_InstalledUpgrades))
            data2.m_PrisonVanCapacity -= data5.m_PrisonVanCapacity;
          EmergencyShelterData data6;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (UpgradeUtils.TryGetCombinedComponent<EmergencyShelterData>(this.m_Temp.m_Original, out data6, ref this.m_PrefabRefData, ref this.m_PrefabEmergencyShelterData, ref this.m_InstalledUpgrades))
            data3.m_VehicleCapacity -= data6.m_VehicleCapacity;
        }
        NativeList<LayoutElement> layoutBuffer = new NativeList<LayoutElement>();
        RoadTypes roadType = RoadTypes.None;
        TrackTypes trackType = TrackTypes.None;
        bool flag = false;
        switch (data1.m_TransportType)
        {
          case TransportType.Bus:
            roadType = RoadTypes.Car;
            break;
          case TransportType.Train:
            trackType = TrackTypes.Train;
            flag = true;
            break;
          case TransportType.Taxi:
            roadType = RoadTypes.Car;
            break;
          case TransportType.Tram:
            trackType = TrackTypes.Tram;
            break;
          case TransportType.Subway:
            trackType = TrackTypes.Subway;
            break;
          default:
            data1.m_VehicleCapacity = 0;
            break;
        }
        for (int index = 0; index < data1.m_VehicleCapacity; ++index)
        {
          PublicTransportPurpose publicTransportPurpose = (PublicTransportPurpose) 0;
          Resource cargoResource = Resource.NoResource;
          if (flag && random.NextBool())
            cargoResource = Resource.Food;
          else
            publicTransportPurpose = data1.m_TransportType == TransportType.Taxi ? (PublicTransportPurpose) 0 : PublicTransportPurpose.TransportLine;
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, data1.m_TransportType, data1.m_EnergyTypes, publicTransportPurpose, cargoResource, roadType, trackType, (PublicTransportFlags) 0, ref layoutBuffer);
        }
        for (int index = 0; index < data2.m_PrisonVanCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, TransportType.Bus, EnergyTypes.FuelAndElectricity, PublicTransportPurpose.PrisonerTransport, Resource.NoResource, RoadTypes.Car, TrackTypes.None, PublicTransportFlags.PrisonerTransport, ref layoutBuffer);
        }
        for (int index = 0; index < data3.m_VehicleCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, TransportType.Bus, EnergyTypes.FuelAndElectricity, PublicTransportPurpose.Evacuation, Resource.NoResource, RoadTypes.Car, TrackTypes.None, PublicTransportFlags.Evacuating, ref layoutBuffer);
        }
        if (!layoutBuffer.IsCreated)
          return;
        layoutBuffer.Dispose();
      }

      private void CreateVehicle(
        ref Random random,
        TransportType transportType,
        EnergyTypes energyTypes,
        PublicTransportPurpose publicTransportPurpose,
        Resource cargoResource,
        RoadTypes roadType,
        TrackTypes trackType,
        PublicTransportFlags publicTransportFlags,
        ref NativeList<LayoutElement> layoutBuffer)
      {
        int2 passengerCapacity = (int2) 0;
        int2 cargoCapacity = (int2) 0;
        if (cargoResource != Resource.NoResource)
          cargoCapacity = new int2(1, int.MaxValue);
        else
          passengerCapacity = new int2(1, int.MaxValue);
        Entity primaryPrefab;
        Entity secondaryPrefab;
        // ISSUE: reference to a compiler-generated field
        this.m_TransportVehicleSelectData.SelectVehicle(ref random, transportType, energyTypes, publicTransportPurpose, cargoResource, out primaryPrefab, out secondaryPrefab, ref passengerCapacity, ref cargoCapacity);
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(primaryPrefab, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, trackType, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity = ParkedVehiclesSystem.FindDeletedVehicle(primaryPrefab, secondaryPrefab, transform, this.m_DeletedVehicleMap);
        NativeArray<LayoutElement> nativeArray = new NativeArray<LayoutElement>();
        if (entity != Entity.Null)
        {
          DynamicBuffer<LayoutElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LayoutElements.TryGetBuffer(entity, out bufferData) && bufferData.Length != 0)
          {
            nativeArray = bufferData.AsNativeArray();
            for (int index = 0; index < bufferData.Length; ++index)
            {
              Entity vehicle = bufferData[index].m_Vehicle;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Deleted>(0, vehicle);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Transform>(0, vehicle, transform);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(0, vehicle);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<Deleted>(0, entity);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Transform>(0, entity, transform);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(0, entity);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          entity = this.m_TransportVehicleSelectData.CreateVehicle(this.m_CommandBuffer, 0, ref random, transform, this.m_Entity, primaryPrefab, secondaryPrefab, transportType, energyTypes, publicTransportPurpose, cargoResource, ref passengerCapacity, ref cargoCapacity, true, ref layoutBuffer);
          if (layoutBuffer.IsCreated && layoutBuffer.Length != 0)
          {
            nativeArray = layoutBuffer.AsArray();
            // ISSUE: reference to a compiler-generated field
            if (this.m_IsTemp)
            {
              for (int index = 0; index < layoutBuffer.Length; ++index)
              {
                Entity vehicle = layoutBuffer[index].m_Vehicle;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Animation>(0, vehicle, new Animation());
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<InterpolatedTransform>(0, vehicle, new InterpolatedTransform());
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_IsTemp)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Animation>(0, entity, new Animation());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<InterpolatedTransform>(0, entity, new InterpolatedTransform());
            }
          }
        }
        if (transportType == TransportType.Taxi)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Taxi>(0, entity, new Taxi(TaxiFlags.Disabled));
        }
        if (publicTransportPurpose != (PublicTransportPurpose) 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<PublicTransport>(0, entity, new PublicTransport()
          {
            m_State = PublicTransportFlags.Disabled | publicTransportFlags
          });
        }
        if (cargoResource != Resource.NoResource)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<CargoTransport>(0, entity, new CargoTransport()
          {
            m_State = CargoTransportFlags.Disabled
          });
        }
        if (nativeArray.IsCreated)
        {
          for (int index = 0; index < nativeArray.Length; ++index)
          {
            Entity vehicle = nativeArray[index].m_Vehicle;
            if (roadType != RoadTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<ParkedCar>(0, vehicle, new ParkedCar(lane, curvePosition));
            }
            if (trackType != TrackTypes.None)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<ParkedTrain>(0, vehicle, new ParkedTrain(lane));
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ParkedCar>(0, entity, new ParkedCar(lane, curvePosition));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(0, entity, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsTemp)
        {
          Temp component = new Temp();
          // ISSUE: reference to a compiler-generated field
          component.m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate);
          if (nativeArray.IsCreated)
          {
            for (int index = 0; index < nativeArray.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Temp>(0, nativeArray[index].m_Vehicle, component);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Temp>(0, entity, component);
          }
        }
        if (!layoutBuffer.IsCreated)
          return;
        layoutBuffer.Clear();
      }
    }

    [BurstCompile]
    private struct SpawnPostVansJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<PostFacilityData> m_PrefabPostFacilityData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public PostVanSelectData m_PostVanSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        PostFacilityData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<PostFacilityData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabPostFacilityData, ref this.m_InstalledUpgrades);
        PostFacilityData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null && UpgradeUtils.TryGetCombinedComponent<PostFacilityData>(this.m_Temp.m_Original, out data2, ref this.m_PrefabRefData, ref this.m_PrefabPostFacilityData, ref this.m_InstalledUpgrades))
          data1.m_PostVanCapacity -= data2.m_PostVanCapacity;
        for (int index = 0; index < data1.m_PostVanCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, RoadTypes.Car);
        }
      }

      private void CreateVehicle(ref Random random, RoadTypes roadType)
      {
        int2 mailCapacity = new int2(1, int.MaxValue);
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_PostVanSelectData.SelectVehicle(ref random, ref mailCapacity);
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, TrackTypes.None, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity e = ParkedVehiclesSystem.FindDeletedVehicle(entity, Entity.Null, transform, this.m_DeletedVehicleMap);
        if (e != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(e, transform);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(e);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          e = this.m_PostVanSelectData.CreateVehicle(this.m_CommandBuffer, ref random, transform, this.m_Entity, entity, true);
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Animation>(e, new Animation());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(e, new InterpolatedTransform());
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PostVan>(e, new PostVan(PostVanFlags.Disabled, 0, 0));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(e, new ParkedCar(lane, curvePosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(e, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IsTemp)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(e, new Temp()
        {
          m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
        });
      }
    }

    [BurstCompile]
    private struct SpawnMaintenanceVehiclesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> m_PrefabMaintenanceDepotData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public MaintenanceVehicleSelectData m_MaintenanceVehicleSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        MaintenanceDepotData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<MaintenanceDepotData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabMaintenanceDepotData, ref this.m_InstalledUpgrades);
        MaintenanceDepotData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null && UpgradeUtils.TryGetCombinedComponent<MaintenanceDepotData>(this.m_Temp.m_Original, out data2, ref this.m_PrefabRefData, ref this.m_PrefabMaintenanceDepotData, ref this.m_InstalledUpgrades))
          data1.m_VehicleCapacity -= data2.m_VehicleCapacity;
        for (int index = 0; index < data1.m_VehicleCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, data1, RoadTypes.Car);
        }
      }

      private void CreateVehicle(
        ref Random random,
        MaintenanceDepotData maintenanceDepotData,
        RoadTypes roadType)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity = this.m_MaintenanceVehicleSelectData.SelectVehicle(ref random, MaintenanceType.None, maintenanceDepotData.m_MaintenanceType, ParkedVehiclesSystem.GetMaxVehicleSize(this.m_Locations, roadType));
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, TrackTypes.None, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity e = ParkedVehiclesSystem.FindDeletedVehicle(entity, Entity.Null, transform, this.m_DeletedVehicleMap);
        if (e != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(e, transform);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(e);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          e = this.m_MaintenanceVehicleSelectData.CreateVehicle(this.m_CommandBuffer, ref random, transform, this.m_Entity, entity, MaintenanceType.None, maintenanceDepotData.m_MaintenanceType, (float4) float.MaxValue, true);
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Animation>(e, new Animation());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(e, new InterpolatedTransform());
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<MaintenanceVehicle>(e, new MaintenanceVehicle(MaintenanceVehicleFlags.Disabled, 0, maintenanceDepotData.m_VehicleEfficiency));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(e, new ParkedCar(lane, curvePosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(e, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IsTemp)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(e, new Temp()
        {
          m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
        });
      }
    }

    [BurstCompile]
    private struct SpawnGarbageTrucksJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> m_PrefabGarbageFacilityData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public Temp m_Temp;
      [ReadOnly]
      public bool m_IsTemp;
      [ReadOnly]
      public GarbageTruckSelectData m_GarbageTruckSelectData;
      public NativeList<ParkedVehiclesSystem.ParkingLocation> m_Locations;
      [NativeDisableContainerSafetyRestriction]
      public NativeParallelMultiHashMap<Entity, ParkedVehiclesSystem.DeletedVehicleData> m_DeletedVehicleMap;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_PseudoRandomSeedData[this.m_Entity].GetRandom((uint) PseudoRandomSeed.kParkedCars);
        GarbageFacilityData data1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        UpgradeUtils.TryGetCombinedComponent<GarbageFacilityData>(this.m_Entity, out data1, ref this.m_PrefabRefData, ref this.m_PrefabGarbageFacilityData, ref this.m_InstalledUpgrades);
        GarbageFacilityData data2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Temp.m_Original != Entity.Null && UpgradeUtils.TryGetCombinedComponent<GarbageFacilityData>(this.m_Temp.m_Original, out data2, ref this.m_PrefabRefData, ref this.m_PrefabGarbageFacilityData, ref this.m_InstalledUpgrades))
          data1.m_VehicleCapacity -= data2.m_VehicleCapacity;
        for (int index = 0; index < data1.m_VehicleCapacity; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateVehicle(ref random, data1, RoadTypes.Car);
        }
      }

      private void CreateVehicle(
        ref Random random,
        GarbageFacilityData garbageFacilityData,
        RoadTypes roadType)
      {
        int2 garbageCapacity = new int2(1, int.MaxValue);
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_GarbageTruckSelectData.SelectVehicle(ref random, ref garbageCapacity);
        ObjectGeometryData componentData;
        Transform transform;
        Entity lane;
        float curvePosition;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabObjectGeometryData.TryGetComponent(entity, out componentData) || !ParkedVehiclesSystem.SelectParkingSpace(ref random, this.m_Locations, componentData, roadType, TrackTypes.None, out transform, out lane, out curvePosition))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity e = ParkedVehiclesSystem.FindDeletedVehicle(entity, Entity.Null, transform, this.m_DeletedVehicleMap);
        if (e != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Deleted>(e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(e, transform);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(e);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          e = this.m_GarbageTruckSelectData.CreateVehicle(this.m_CommandBuffer, ref random, transform, this.m_Entity, entity, ref garbageCapacity, true);
          // ISSUE: reference to a compiler-generated field
          if (this.m_IsTemp)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Animation>(e, new Animation());
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<InterpolatedTransform>(e, new InterpolatedTransform());
          }
        }
        GarbageTruckFlags flags = GarbageTruckFlags.Disabled;
        if (garbageFacilityData.m_IndustrialWasteOnly)
          flags |= GarbageTruckFlags.IndustrialWasteOnly;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<GarbageTruck>(e, new GarbageTruck(flags, 0));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ParkedCar>(e, new ParkedCar(lane, curvePosition));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Owner>(e, new Owner(this.m_Entity));
        // ISSUE: reference to a compiler-generated field
        if (!this.m_IsTemp)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Temp>(e, new Temp()
        {
          m_Flags = this.m_Temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Hidden | TempFlags.Duplicate)
        });
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.ParkingLane> __Game_Net_ParkingLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkingLaneData> __Game_Prefabs_ParkingLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LaneOverlap> __Game_Net_LaneOverlap_RO_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Train> __Game_Vehicles_Train_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovingObjectData> __Game_Prefabs_MovingObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainObjectData> __Game_Prefabs_TrainObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PoliceStationData> __Game_Prefabs_PoliceStationData_RO_ComponentLookup;
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<FireStationData> __Game_Prefabs_FireStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HospitalData> __Game_Prefabs_HospitalData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DeathcareFacilityData> __Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrisonData> __Game_Prefabs_PrisonData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EmergencyShelterData> __Game_Prefabs_EmergencyShelterData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PostFacilityData> __Game_Prefabs_PostFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> __Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageFacilityData> __Game_Prefabs_GarbageFacilityData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkingLaneData_RO_ComponentLookup = state.GetComponentLookup<ParkingLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RO_BufferLookup = state.GetBufferLookup<SpawnLocationElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneOverlap_RO_BufferLookup = state.GetBufferLookup<LaneOverlap>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentLookup = state.GetComponentLookup<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Train_RO_ComponentLookup = state.GetComponentLookup<Train>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MovingObjectData_RO_ComponentLookup = state.GetComponentLookup<MovingObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainObjectData_RO_ComponentLookup = state.GetComponentLookup<TrainObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PoliceStationData_RO_ComponentLookup = state.GetComponentLookup<PoliceStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RW_BufferLookup = state.GetBufferLookup<InstalledUpgrade>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_FireStationData_RO_ComponentLookup = state.GetComponentLookup<FireStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HospitalData_RO_ComponentLookup = state.GetComponentLookup<HospitalData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeathcareFacilityData_RO_ComponentLookup = state.GetComponentLookup<DeathcareFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrisonData_RO_ComponentLookup = state.GetComponentLookup<PrisonData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EmergencyShelterData_RO_ComponentLookup = state.GetComponentLookup<EmergencyShelterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PostFacilityData_RO_ComponentLookup = state.GetComponentLookup<PostFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup = state.GetComponentLookup<MaintenanceDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GarbageFacilityData_RO_ComponentLookup = state.GetComponentLookup<GarbageFacilityData>(true);
      }
    }
  }
}
