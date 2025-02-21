// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HealthcarePathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Tools;
using Game.Vehicles;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct HealthcarePathfindSetup
  {
    private EntityQuery m_AmbulanceQuery;
    private EntityQuery m_HospitalQuery;
    private EntityQuery m_HearseQuery;
    private EntityQuery m_HealthcareRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<HealthcareRequest> m_HealthcareRequestType;
    private ComponentTypeHandle<Hospital> m_HospitalType;
    private ComponentTypeHandle<DeathcareFacility> m_DeathcareFacilityType;
    private ComponentTypeHandle<Hearse> m_HearseType;
    private ComponentTypeHandle<Ambulance> m_AmbulanceType;
    private ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
    private ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
    private ComponentLookup<District> m_DistrictData;
    private ComponentLookup<Citizen> m_CitizenData;
    private ComponentLookup<HealthProblem> m_HealthProblemData;
    private ComponentLookup<Vehicle> m_VehicleData;
    private ComponentLookup<Ambulance> m_AmbulanceData;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private ComponentLookup<Game.City.City> m_CityData;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private CitySystem m_CitySystem;

    public HealthcarePathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_AmbulanceQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hospital>(),
          ComponentType.ReadOnly<Ambulance>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<ServiceUpgrade>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_HospitalQuery = system.GetSetupQuery(ComponentType.ReadOnly<Hospital>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated method
      this.m_HearseQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<DeathcareFacility>(),
          ComponentType.ReadOnly<Hearse>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<ServiceUpgrade>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_HealthcareRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<HealthcareRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_HealthcareRequestType = system.GetComponentTypeHandle<HealthcareRequest>(true);
      this.m_HospitalType = system.GetComponentTypeHandle<Hospital>(true);
      this.m_DeathcareFacilityType = system.GetComponentTypeHandle<DeathcareFacility>(true);
      this.m_HearseType = system.GetComponentTypeHandle<Hearse>(true);
      this.m_AmbulanceType = system.GetComponentTypeHandle<Ambulance>(true);
      this.m_HealthcareRequestData = system.GetComponentLookup<HealthcareRequest>(true);
      this.m_CurrentDistrictData = system.GetComponentLookup<CurrentDistrict>(true);
      this.m_DistrictData = system.GetComponentLookup<District>(true);
      this.m_CitizenData = system.GetComponentLookup<Citizen>(true);
      this.m_HealthProblemData = system.GetComponentLookup<HealthProblem>(true);
      this.m_VehicleData = system.GetComponentLookup<Vehicle>(true);
      this.m_AmbulanceData = system.GetComponentLookup<Ambulance>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_CityData = system.GetComponentLookup<Game.City.City>(true);
      this.m_AreaSearchSystem = system.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      this.m_CitySystem = system.World.GetOrCreateSystemManaged<CitySystem>();
    }

    public JobHandle SetupAmbulances(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_HospitalType.Update((SystemBase) system);
      this.m_AmbulanceType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      this.m_CityData.Update((SystemBase) system);
      return new HealthcarePathfindSetup.SetupAmbulancesJob()
      {
        m_EntityType = this.m_EntityType,
        m_HospitalType = this.m_HospitalType,
        m_AmbulanceType = this.m_AmbulanceType,
        m_OwnerType = this.m_OwnerType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_CityData = this.m_CityData,
        m_City = this.m_CitySystem.City,
        m_SetupData = setupData
      }.ScheduleParallel<HealthcarePathfindSetup.SetupAmbulancesJob>(this.m_AmbulanceQuery, inputDeps);
    }

    public JobHandle SetupHospitals(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_HospitalType.Update((SystemBase) system);
      this.m_CitizenData.Update((SystemBase) system);
      this.m_HealthProblemData.Update((SystemBase) system);
      this.m_AmbulanceData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new HealthcarePathfindSetup.SetupHospitalsJob()
      {
        m_EntityType = this.m_EntityType,
        m_HospitalType = this.m_HospitalType,
        m_CitizenData = this.m_CitizenData,
        m_HealthProblemData = this.m_HealthProblemData,
        m_AmbulanceData = this.m_AmbulanceData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<HealthcarePathfindSetup.SetupHospitalsJob>(this.m_HospitalQuery, inputDeps);
    }

    public JobHandle SetupHearses(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_DeathcareFacilityType.Update((SystemBase) system);
      this.m_HearseType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      this.m_CityData.Update((SystemBase) system);
      return new HealthcarePathfindSetup.SetupHearsesJob()
      {
        m_EntityType = this.m_EntityType,
        m_DeathcareFacilityType = this.m_DeathcareFacilityType,
        m_HearseType = this.m_HearseType,
        m_CityData = this.m_CityData,
        m_OwnerType = this.m_OwnerType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData,
        m_City = this.m_CitySystem.City
      }.ScheduleParallel<HealthcarePathfindSetup.SetupHearsesJob>(this.m_HearseQuery, inputDeps);
    }

    public JobHandle SetupHealthcareRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_HealthcareRequestType.Update((SystemBase) system);
      this.m_HealthcareRequestData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_DistrictData.Update((SystemBase) system);
      this.m_VehicleData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new HealthcarePathfindSetup.HealthcareRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_HealthcareRequestType = this.m_HealthcareRequestType,
        m_HealthcareRequestData = this.m_HealthcareRequestData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_DistrictData = this.m_DistrictData,
        m_VehicleData = this.m_VehicleData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
        m_SetupData = setupData
      }.ScheduleParallel<HealthcarePathfindSetup.HealthcareRequestsJob>(this.m_HealthcareRequestQuery, JobHandle.CombineDependencies(inputDeps, dependencies));
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      return jobHandle;
    }

    [BurstCompile]
    private struct SetupAmbulancesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Hospital> m_HospitalType;
      [ReadOnly]
      public ComponentTypeHandle<Ambulance> m_AmbulanceType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [ReadOnly]
      public ComponentLookup<Game.City.City> m_CityData;
      [ReadOnly]
      public Entity m_City;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        if (chunk.Has<Game.Objects.OutsideConnection>() && !CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
          return;
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<Hospital> nativeArray2 = chunk.GetNativeArray<Hospital>(ref this.m_HospitalType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Entity entity1 = nativeArray1[index1];
            Hospital hospital = nativeArray2[index1];
            for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
            {
              Entity entity2;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index2, out entity2, out targetSeeker);
              RoadTypes roadTypes1 = RoadTypes.None;
              if (AreaUtils.CheckServiceDistrict(entity2, entity1, this.m_ServiceDistricts))
              {
                if ((hospital.m_Flags & HospitalFlags.HasAvailableAmbulances) != (HospitalFlags) 0)
                  roadTypes1 |= RoadTypes.Car;
                if ((hospital.m_Flags & HospitalFlags.HasAvailableMedicalHelicopters) != (HospitalFlags) 0)
                  roadTypes1 |= RoadTypes.Helicopter;
              }
              RoadTypes roadTypes2 = roadTypes1 & (targetSeeker.m_SetupQueueTarget.m_RoadTypes | targetSeeker.m_SetupQueueTarget.m_FlyingTypes);
              if (roadTypes2 != RoadTypes.None)
              {
                float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                RoadTypes roadTypes3 = targetSeeker.m_SetupQueueTarget.m_RoadTypes;
                RoadTypes flyingTypes = targetSeeker.m_SetupQueueTarget.m_FlyingTypes;
                targetSeeker.m_SetupQueueTarget.m_RoadTypes &= roadTypes2;
                targetSeeker.m_SetupQueueTarget.m_FlyingTypes &= roadTypes2;
                targetSeeker.FindTargets(entity1, cost);
                targetSeeker.m_SetupQueueTarget.m_RoadTypes = roadTypes3;
                targetSeeker.m_SetupQueueTarget.m_FlyingTypes = flyingTypes;
              }
            }
          }
        }
        else
        {
          NativeArray<Ambulance> nativeArray3 = chunk.GetNativeArray<Ambulance>(ref this.m_AmbulanceType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Ambulance ambulance = nativeArray3[index3];
            if (nativeArray4.Length != 0)
            {
              if ((ambulance.m_State & (AmbulanceFlags.Returning | AmbulanceFlags.Dispatched | AmbulanceFlags.Transporting | AmbulanceFlags.Disabled)) != AmbulanceFlags.Returning)
                continue;
            }
            else if ((ambulance.m_State & AmbulanceFlags.Disabled) != (AmbulanceFlags) 0)
              continue;
            Entity entity3 = nativeArray1[index3];
            for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
            {
              Entity entity4;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index4, out entity4, out targetSeeker);
              if (nativeArray5.Length == 0 || AreaUtils.CheckServiceDistrict(entity4, nativeArray5[index3].m_Owner, this.m_ServiceDistricts))
                targetSeeker.FindTargets(entity3, 0.0f);
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct SetupHospitalsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Hospital> m_HospitalType;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;
      [ReadOnly]
      public ComponentLookup<HealthProblem> m_HealthProblemData;
      [ReadOnly]
      public ComponentLookup<Ambulance> m_AmbulanceData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<Hospital> nativeArray2 = chunk.GetNativeArray<Hospital>(ref this.m_HospitalType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          Entity owner1;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out owner1, out targetSeeker);
          int num = 0;
          HealthProblemFlags healthProblemFlags = HealthProblemFlags.None;
          Entity entity2 = owner1;
          Entity owner2 = Entity.Null;
          if (this.m_AmbulanceData.HasComponent(owner1))
          {
            entity2 = this.m_AmbulanceData[owner1].m_TargetPatient;
            if (targetSeeker.m_Owner.HasComponent(owner1))
              owner2 = targetSeeker.m_Owner[owner1].m_Owner;
          }
          if (this.m_CitizenData.HasComponent(entity2))
            num = (int) this.m_CitizenData[entity2].m_Health;
          if (this.m_HealthProblemData.HasComponent(entity2))
            healthProblemFlags = this.m_HealthProblemData[entity2].m_Flags;
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity3 = nativeArray1[index2];
            Hospital hospital = nativeArray2[index2];
            if (((healthProblemFlags & HealthProblemFlags.Sick) == HealthProblemFlags.None || (hospital.m_Flags & HospitalFlags.CanCureDisease) != (HospitalFlags) 0) && ((healthProblemFlags & HealthProblemFlags.Injured) == HealthProblemFlags.None || (hospital.m_Flags & HospitalFlags.CanCureInjury) != (HospitalFlags) 0) && num >= (int) hospital.m_MinHealth && num <= (int) hospital.m_MaxHealth)
            {
              PathMethod methods1 = targetSeeker.m_SetupQueueTarget.m_Methods;
              RoadTypes roadTypes1 = targetSeeker.m_SetupQueueTarget.m_RoadTypes;
              if (!AreaUtils.CheckServiceDistrict(entity1, entity3, this.m_ServiceDistricts))
                methods1 &= ~PathMethod.Pedestrian;
              if ((methods1 & PathMethod.Pedestrian) != (PathMethod) 0 || roadTypes1 != RoadTypes.None)
              {
                float cost = (float) (((double) byte.MaxValue - (double) hospital.m_TreatmentBonus) * 200.0 / (20.0 + (double) num));
                if (entity3 != owner2)
                  cost += 10f;
                if ((hospital.m_Flags & HospitalFlags.HasRoomForPatients) == (HospitalFlags) 0)
                  cost += 120f;
                PathMethod methods2 = targetSeeker.m_SetupQueueTarget.m_Methods;
                RoadTypes roadTypes2 = targetSeeker.m_SetupQueueTarget.m_RoadTypes;
                targetSeeker.m_SetupQueueTarget.m_Methods = methods1;
                targetSeeker.m_SetupQueueTarget.m_RoadTypes = roadTypes1;
                targetSeeker.FindTargets(entity3, cost);
                targetSeeker.m_SetupQueueTarget.m_Methods = methods2;
                targetSeeker.m_SetupQueueTarget.m_RoadTypes = roadTypes2;
              }
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct SetupHearsesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<DeathcareFacility> m_DeathcareFacilityType;
      [ReadOnly]
      public ComponentLookup<Game.City.City> m_CityData;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public ComponentTypeHandle<Hearse> m_HearseType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        if (chunk.Has<Game.Objects.OutsideConnection>() && !CityUtils.CheckOption(this.m_CityData[this.m_City], CityOption.ImportOutsideServices))
          return;
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<DeathcareFacility> nativeArray2 = chunk.GetNativeArray<DeathcareFacility>(ref this.m_DeathcareFacilityType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Entity entity1 = nativeArray1[index1];
            if ((nativeArray2[index1].m_Flags & (DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies)) == (DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies))
            {
              for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
              {
                Entity entity2;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index2, out entity2, out targetSeeker);
                if (AreaUtils.CheckServiceDistrict(entity2, entity1, this.m_ServiceDistricts))
                {
                  float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                  targetSeeker.FindTargets(entity1, cost);
                }
              }
            }
          }
        }
        else
        {
          NativeArray<Hearse> nativeArray3 = chunk.GetNativeArray<Hearse>(ref this.m_HearseType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Hearse hearse = nativeArray3[index3];
            if (nativeArray4.Length != 0)
            {
              if ((hearse.m_State & (HearseFlags.Returning | HearseFlags.Dispatched | HearseFlags.Transporting | HearseFlags.Disabled)) != HearseFlags.Returning)
                continue;
            }
            else if ((hearse.m_State & HearseFlags.Disabled) != (HearseFlags) 0)
              continue;
            Entity entity3 = nativeArray1[index3];
            for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
            {
              Entity entity4;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index4, out entity4, out targetSeeker);
              if (nativeArray5.Length == 0 || AreaUtils.CheckServiceDistrict(entity4, nativeArray5[index3].m_Owner, this.m_ServiceDistricts))
                targetSeeker.FindTargets(entity3, 0.0f);
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct HealthcareRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<HealthcareRequest> m_HealthcareRequestType;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<ServiceRequest> nativeArray2 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
        NativeArray<HealthcareRequest> nativeArray3 = chunk.GetNativeArray<HealthcareRequest>(ref this.m_HealthcareRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          HealthcareRequest componentData1;
          if (this.m_HealthcareRequestData.TryGetComponent(owner, out componentData1))
          {
            Entity service = Entity.Null;
            if (this.m_VehicleData.HasComponent(componentData1.m_Citizen))
            {
              Owner componentData2;
              if (targetSeeker.m_Owner.TryGetComponent(componentData1.m_Citizen, out componentData2))
                service = componentData2.m_Owner;
            }
            else if (targetSeeker.m_PrefabRef.HasComponent(componentData1.m_Citizen))
              service = componentData1.m_Citizen;
            else
              continue;
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                HealthcareRequest healthcareRequest = nativeArray3[index2];
                if (healthcareRequest.m_Type == componentData1.m_Type)
                {
                  Entity district = Entity.Null;
                  CurrentBuilding componentData3;
                  if (targetSeeker.m_CurrentBuilding.TryGetComponent(healthcareRequest.m_Citizen, out componentData3))
                  {
                    if (this.m_CurrentDistrictData.HasComponent(componentData3.m_CurrentBuilding))
                      district = this.m_CurrentDistrictData[componentData3.m_CurrentBuilding].m_District;
                  }
                  else
                  {
                    CurrentTransport componentData4;
                    Transform componentData5;
                    if (targetSeeker.m_CurrentTransport.TryGetComponent(healthcareRequest.m_Citizen, out componentData4) && targetSeeker.m_Transform.TryGetComponent(componentData4.m_CurrentTransport, out componentData5))
                    {
                      HealthcarePathfindSetup.HealthcareRequestsJob.DistrictIterator iterator = new HealthcarePathfindSetup.HealthcareRequestsJob.DistrictIterator()
                      {
                        m_Position = componentData5.m_Position.xz,
                        m_DistrictData = this.m_DistrictData,
                        m_Nodes = targetSeeker.m_AreaNode,
                        m_Triangles = targetSeeker.m_AreaTriangle
                      };
                      this.m_AreaTree.Iterate<HealthcarePathfindSetup.HealthcareRequestsJob.DistrictIterator>(ref iterator);
                      district = iterator.m_Result;
                    }
                  }
                  if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  {
                    targetSeeker.FindTargets(nativeArray1[index2], healthcareRequest.m_Citizen, 0.0f, EdgeFlags.DefaultMask, true, false);
                    Entity entity = healthcareRequest.m_Citizen;
                    if (targetSeeker.m_CurrentTransport.HasComponent(entity))
                      entity = targetSeeker.m_CurrentTransport[entity].m_CurrentTransport;
                    else if (targetSeeker.m_CurrentBuilding.HasComponent(entity))
                      entity = targetSeeker.m_CurrentBuilding[entity].m_CurrentBuilding;
                    Transform componentData6;
                    if (targetSeeker.m_Transform.TryGetComponent(entity, out componentData6) && (targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) != (PathMethod) 0 && (targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) != RoadTypes.None)
                    {
                      Entity lane = Entity.Null;
                      float curvePos = 0.0f;
                      float maxValue = float.MaxValue;
                      targetSeeker.m_AirwayData.helicopterMap.FindClosestLane(componentData6.m_Position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
                      if (lane != Entity.Null)
                        targetSeeker.m_Buffer.Enqueue(new PathTarget(nativeArray1[index2], lane, curvePos, 0.0f));
                    }
                  }
                }
              }
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }

      private struct DistrictIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public float2 m_Position;
        public ComponentLookup<District> m_DistrictData;
        public BufferLookup<Game.Areas.Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public Entity m_Result;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position) || !this.m_DistrictData.HasComponent(areaItem.m_Area))
            return;
          DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[areaItem.m_Area];
          DynamicBuffer<Triangle> triangle = this.m_Triangles[areaItem.m_Area];
          if (triangle.Length <= areaItem.m_Triangle || !MathUtils.Intersect(AreaUtils.GetTriangle2(node, triangle[areaItem.m_Triangle]), this.m_Position, out float2 _))
            return;
          this.m_Result = areaItem.m_Area;
        }
      }
    }
  }
}
