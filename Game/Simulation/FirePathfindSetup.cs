// Decompiled with JetBrains decompiler
// Type: Game.Simulation.FirePathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
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
  public struct FirePathfindSetup
  {
    private EntityQuery m_FireEngineQuery;
    private EntityQuery m_EmergencyShelterQuery;
    private EntityQuery m_EvacuationTransportQuery;
    private EntityQuery m_EvacuationRequestQuery;
    private EntityQuery m_FireRescueRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<FireRescueRequest> m_FireRescueRequestType;
    private ComponentTypeHandle<EvacuationRequest> m_EvacuationRequestType;
    private ComponentTypeHandle<Game.Buildings.FireStation> m_FireStationType;
    private ComponentTypeHandle<Game.Buildings.EmergencyShelter> m_EmergencyShelterType;
    private ComponentTypeHandle<Game.Vehicles.FireEngine> m_FireEngineType;
    private ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
    private BufferTypeHandle<PathElement> m_PathElementType;
    private BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
    private ComponentLookup<PathInformation> m_PathInformationData;
    private ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
    private ComponentLookup<EvacuationRequest> m_EvacuationRequestData;
    private ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
    private ComponentLookup<Composition> m_CompositionData;
    private ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
    private ComponentLookup<District> m_DistrictData;
    private ComponentLookup<Game.Buildings.FireStation> m_FireStationData;
    private ComponentLookup<Game.Vehicles.FireEngine> m_FireEngineData;
    private ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
    private ComponentLookup<NetCompositionData> m_NetCompositionData;
    private BufferLookup<PathElement> m_PathElements;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private ComponentLookup<Game.City.City> m_CityData;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private CitySystem m_CitySystem;

    public FirePathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_FireEngineQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.FireStation>(),
          ComponentType.ReadOnly<Game.Vehicles.FireEngine>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_EmergencyShelterQuery = system.GetSetupQuery(ComponentType.ReadOnly<Game.Buildings.EmergencyShelter>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated method
      this.m_EvacuationTransportQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.EmergencyShelter>(),
          ComponentType.ReadOnly<Game.Vehicles.PublicTransport>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_EvacuationRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<EvacuationRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      // ISSUE: reference to a compiler-generated method
      this.m_FireRescueRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<FireRescueRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_FireRescueRequestType = system.GetComponentTypeHandle<FireRescueRequest>(true);
      this.m_EvacuationRequestType = system.GetComponentTypeHandle<EvacuationRequest>(true);
      this.m_FireStationType = system.GetComponentTypeHandle<Game.Buildings.FireStation>(true);
      this.m_EmergencyShelterType = system.GetComponentTypeHandle<Game.Buildings.EmergencyShelter>(true);
      this.m_FireEngineType = system.GetComponentTypeHandle<Game.Vehicles.FireEngine>(true);
      this.m_PublicTransportType = system.GetComponentTypeHandle<Game.Vehicles.PublicTransport>(true);
      this.m_PathElementType = system.GetBufferTypeHandle<PathElement>(true);
      this.m_ServiceDispatchType = system.GetBufferTypeHandle<ServiceDispatch>(true);
      this.m_PathInformationData = system.GetComponentLookup<PathInformation>(true);
      this.m_FireRescueRequestData = system.GetComponentLookup<FireRescueRequest>(true);
      this.m_EvacuationRequestData = system.GetComponentLookup<EvacuationRequest>(true);
      this.m_OutsideConnections = system.GetComponentLookup<Game.Objects.OutsideConnection>(true);
      this.m_CompositionData = system.GetComponentLookup<Composition>(true);
      this.m_CurrentDistrictData = system.GetComponentLookup<CurrentDistrict>(true);
      this.m_DistrictData = system.GetComponentLookup<District>(true);
      this.m_FireStationData = system.GetComponentLookup<Game.Buildings.FireStation>(true);
      this.m_FireEngineData = system.GetComponentLookup<Game.Vehicles.FireEngine>(true);
      this.m_PublicTransportData = system.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
      this.m_NetCompositionData = system.GetComponentLookup<NetCompositionData>(true);
      this.m_PathElements = system.GetBufferLookup<PathElement>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_CityData = system.GetComponentLookup<Game.City.City>(true);
      this.m_AreaSearchSystem = system.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      this.m_NetSearchSystem = system.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      this.m_CitySystem = system.World.GetOrCreateSystemManaged<CitySystem>();
    }

    public JobHandle SetupFireEngines(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_FireStationType.Update((SystemBase) system);
      this.m_FireEngineType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_FireRescueRequestData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      this.m_OutsideConnections.Update((SystemBase) system);
      this.m_CityData.Update((SystemBase) system);
      return new FirePathfindSetup.SetupFireEnginesJob()
      {
        m_EntityType = this.m_EntityType,
        m_FireStationType = this.m_FireStationType,
        m_FireEngineType = this.m_FireEngineType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PathInformationData = this.m_PathInformationData,
        m_FireRescueRequestData = this.m_FireRescueRequestData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_OutsideConnections = this.m_OutsideConnections,
        m_CityData = this.m_CityData,
        m_City = this.m_CitySystem.City,
        m_SetupData = setupData
      }.ScheduleParallel<FirePathfindSetup.SetupFireEnginesJob>(this.m_FireEngineQuery, inputDeps);
    }

    public JobHandle SetupEmergencyShelters(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_EmergencyShelterType.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new FirePathfindSetup.SetupEmergencySheltersJob()
      {
        m_EntityType = this.m_EntityType,
        m_EmergencyShelterType = this.m_EmergencyShelterType,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<FirePathfindSetup.SetupEmergencySheltersJob>(this.m_EmergencyShelterQuery, inputDeps);
    }

    public JobHandle SetupEvacuationTransport(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_EmergencyShelterType.Update((SystemBase) system);
      this.m_PublicTransportType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new FirePathfindSetup.SetupEvacuationTransportJob()
      {
        m_EntityType = this.m_EntityType,
        m_EmergencyShelterType = this.m_EmergencyShelterType,
        m_PublicTransportType = this.m_PublicTransportType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PathInformationData = this.m_PathInformationData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<FirePathfindSetup.SetupEvacuationTransportJob>(this.m_EvacuationTransportQuery, inputDeps);
    }

    public JobHandle SetupEvacuationRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_EvacuationRequestType.Update((SystemBase) system);
      this.m_EvacuationRequestData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_PublicTransportData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new FirePathfindSetup.EvacuationRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_EvacuationRequestType = this.m_EvacuationRequestType,
        m_EvacuationRequestData = this.m_EvacuationRequestData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_PublicTransportData = this.m_PublicTransportData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<FirePathfindSetup.EvacuationRequestsJob>(this.m_EvacuationRequestQuery, inputDeps);
    }

    public JobHandle SetupFireRescueRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_FireRescueRequestType.Update((SystemBase) system);
      this.m_FireRescueRequestData.Update((SystemBase) system);
      this.m_CompositionData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_DistrictData.Update((SystemBase) system);
      this.m_FireEngineData.Update((SystemBase) system);
      this.m_FireStationData.Update((SystemBase) system);
      this.m_NetCompositionData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new FirePathfindSetup.FireRescueRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_FireRescueRequestType = this.m_FireRescueRequestType,
        m_FireRescueRequestData = this.m_FireRescueRequestData,
        m_CompositionData = this.m_CompositionData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_DistrictData = this.m_DistrictData,
        m_FireEngineData = this.m_FireEngineData,
        m_FireStationData = this.m_FireStationData,
        m_NetCompositionData = this.m_NetCompositionData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies1),
        m_NetTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_SetupData = setupData
      }.ScheduleParallel<FirePathfindSetup.FireRescueRequestsJob>(this.m_FireRescueRequestQuery, JobHandle.CombineDependencies(inputDeps, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      return jobHandle;
    }

    [BurstCompile]
    private struct SetupFireEnginesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FireStation> m_FireStationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.FireEngine> m_FireEngineType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
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
        NativeArray<Game.Buildings.FireStation> nativeArray2 = chunk.GetNativeArray<Game.Buildings.FireStation>(ref this.m_FireStationType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Entity entity1 = nativeArray1[index1];
            Game.Buildings.FireStation fireStation = nativeArray2[index1];
            for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
            {
              Entity entity2;
              Entity owner;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index2, out entity2, out owner, out targetSeeker);
              FireRescueRequest componentData;
              this.m_FireRescueRequestData.TryGetComponent(owner, out componentData);
              RoadTypes roadTypes1 = RoadTypes.None;
              if (componentData.m_Target == entity1)
              {
                if ((fireStation.m_Flags & FireStationFlags.HasFreeFireEngines) != (FireStationFlags) 0)
                  roadTypes1 |= RoadTypes.Car;
                if ((fireStation.m_Flags & FireStationFlags.HasFreeFireHelicopters) != (FireStationFlags) 0)
                  roadTypes1 |= RoadTypes.Helicopter;
              }
              else if (AreaUtils.CheckServiceDistrict(entity2, entity1, this.m_ServiceDistricts))
              {
                if ((fireStation.m_Flags & FireStationFlags.HasAvailableFireEngines) != (FireStationFlags) 0)
                  roadTypes1 |= RoadTypes.Car;
                if ((fireStation.m_Flags & FireStationFlags.HasAvailableFireHelicopters) != (FireStationFlags) 0)
                  roadTypes1 |= RoadTypes.Helicopter;
              }
              if (componentData.m_Type == FireRescueRequestType.Disaster && (fireStation.m_Flags & FireStationFlags.DisasterResponseAvailable) == (FireStationFlags) 0)
                roadTypes1 = RoadTypes.None;
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
          NativeArray<Game.Vehicles.FireEngine> nativeArray3 = chunk.GetNativeArray<Game.Vehicles.FireEngine>(ref this.m_FireEngineType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Entity entity3 = nativeArray1[index3];
            Game.Vehicles.FireEngine fireEngine = nativeArray3[index3];
            if ((fireEngine.m_State & (FireEngineFlags.Empty | FireEngineFlags.EstimatedEmpty)) == (FireEngineFlags) 0)
            {
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity entity4;
                Entity owner1;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity4, out owner1, out targetSeeker);
                FireRescueRequest componentData1;
                this.m_FireRescueRequestData.TryGetComponent(owner1, out componentData1);
                if (componentData1.m_Type != FireRescueRequestType.Disaster || (fireEngine.m_State & FireEngineFlags.DisasterResponse) != (FireEngineFlags) 0)
                {
                  float cost = 0.0f;
                  Owner owner2;
                  if (CollectionUtils.TryGet<Owner>(nativeArray5, index3, out owner2))
                  {
                    if (AreaUtils.CheckServiceDistrict(entity4, owner2.m_Owner, this.m_ServiceDistricts))
                    {
                      if (this.m_OutsideConnections.HasComponent(owner2.m_Owner))
                        cost += 30f;
                    }
                    else
                      continue;
                  }
                  if (!(componentData1.m_Target != owner2.m_Owner) || (fireEngine.m_State & FireEngineFlags.Disabled) == (FireEngineFlags) 0)
                  {
                    if ((fireEngine.m_State & FireEngineFlags.Returning) != (FireEngineFlags) 0 || nativeArray4.Length == 0)
                    {
                      targetSeeker.FindTargets(entity3, cost);
                    }
                    else
                    {
                      PathOwner pathOwner = nativeArray4[index3];
                      DynamicBuffer<ServiceDispatch> dynamicBuffer1 = bufferAccessor2[index3];
                      int num = math.min(fireEngine.m_RequestCount, dynamicBuffer1.Length);
                      PathElement pathElement = new PathElement();
                      bool flag = false;
                      if (num >= 1)
                      {
                        DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                        if (pathOwner.m_ElementIndex < dynamicBuffer2.Length)
                        {
                          cost += (float) (dynamicBuffer2.Length - pathOwner.m_ElementIndex) * fireEngine.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
                          pathElement = dynamicBuffer2[dynamicBuffer2.Length - 1];
                          flag = true;
                        }
                      }
                      for (int index5 = 1; index5 < num; ++index5)
                      {
                        Entity request = dynamicBuffer1[index5].m_Request;
                        PathInformation componentData2;
                        if (this.m_PathInformationData.TryGetComponent(request, out componentData2))
                          cost += componentData2.m_Duration * targetSeeker.m_PathfindParameters.m_Weights.time;
                        cost += 10f * targetSeeker.m_PathfindParameters.m_Weights.time;
                        DynamicBuffer<PathElement> bufferData;
                        if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
                        {
                          pathElement = bufferData[bufferData.Length - 1];
                          flag = true;
                        }
                      }
                      if (flag)
                        targetSeeker.m_Buffer.Enqueue(new PathTarget(entity3, pathElement.m_Target, pathElement.m_TargetDelta.y, cost));
                      else
                        targetSeeker.FindTargets(entity3, entity3, cost, EdgeFlags.DefaultMask, true, num >= 1);
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
    }

    [BurstCompile]
    private struct SetupEmergencySheltersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.EmergencyShelter> m_EmergencyShelterType;
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
        NativeArray<Game.Buildings.EmergencyShelter> nativeArray2 = chunk.GetNativeArray<Game.Buildings.EmergencyShelter>(ref this.m_EmergencyShelterType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out targetSeeker);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity2 = nativeArray1[index2];
            if ((nativeArray2[index2].m_Flags & EmergencyShelterFlags.HasShelterSpace) != (EmergencyShelterFlags) 0 && AreaUtils.CheckServiceDistrict(entity1, entity2, this.m_ServiceDistricts))
              targetSeeker.FindTargets(entity2, 0.0f);
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
    private struct SetupEvacuationTransportJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.EmergencyShelter> m_EmergencyShelterType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
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
        NativeArray<Game.Buildings.EmergencyShelter> nativeArray2 = chunk.GetNativeArray<Game.Buildings.EmergencyShelter>(ref this.m_EmergencyShelterType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            if ((nativeArray2[index1].m_Flags & (EmergencyShelterFlags.HasAvailableVehicles | EmergencyShelterFlags.HasShelterSpace)) == (EmergencyShelterFlags.HasAvailableVehicles | EmergencyShelterFlags.HasShelterSpace))
            {
              Entity entity1 = nativeArray1[index1];
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
          NativeArray<Game.Vehicles.PublicTransport> nativeArray3 = chunk.GetNativeArray<Game.Vehicles.PublicTransport>(ref this.m_PublicTransportType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Game.Vehicles.PublicTransport publicTransport = nativeArray3[index3];
            if ((publicTransport.m_State & (PublicTransportFlags.Evacuating | PublicTransportFlags.Disabled | PublicTransportFlags.Full)) == PublicTransportFlags.Evacuating)
            {
              Entity entity3 = nativeArray1[index3];
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity entity4;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity4, out targetSeeker);
                if ((nativeArray5.Length == 0 || AreaUtils.CheckServiceDistrict(entity4, nativeArray5[index3].m_Owner, this.m_ServiceDistricts)) && (publicTransport.m_State & PublicTransportFlags.DummyTraffic) == (PublicTransportFlags) 0)
                {
                  if ((publicTransport.m_State & PublicTransportFlags.Returning) != (PublicTransportFlags) 0 || nativeArray4.Length == 0)
                  {
                    targetSeeker.FindTargets(entity3, 0.0f);
                  }
                  else
                  {
                    PathOwner pathOwner = nativeArray4[index3];
                    DynamicBuffer<ServiceDispatch> dynamicBuffer1 = bufferAccessor2[index3];
                    int num = math.min(publicTransport.m_RequestCount, dynamicBuffer1.Length);
                    PathElement pathElement = new PathElement();
                    float cost = 0.0f;
                    bool flag = false;
                    if (num >= 1)
                    {
                      DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                      if (pathOwner.m_ElementIndex < dynamicBuffer2.Length)
                      {
                        cost += (float) (dynamicBuffer2.Length - pathOwner.m_ElementIndex) * publicTransport.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
                        pathElement = dynamicBuffer2[dynamicBuffer2.Length - 1];
                        flag = true;
                      }
                    }
                    for (int index5 = 1; index5 < num; ++index5)
                    {
                      Entity request = dynamicBuffer1[index5].m_Request;
                      PathInformation componentData;
                      if (this.m_PathInformationData.TryGetComponent(request, out componentData))
                        cost += componentData.m_Duration * targetSeeker.m_PathfindParameters.m_Weights.time;
                      DynamicBuffer<PathElement> bufferData;
                      if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
                      {
                        pathElement = bufferData[bufferData.Length - 1];
                        flag = true;
                      }
                    }
                    if (flag)
                      targetSeeker.m_Buffer.Enqueue(new PathTarget(entity3, pathElement.m_Target, pathElement.m_TargetDelta.y, cost));
                    else
                      targetSeeker.FindTargets(entity3, entity3, cost, EdgeFlags.DefaultMask, true, num >= 1);
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
    }

    [BurstCompile]
    private struct EvacuationRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<EvacuationRequest> m_EvacuationRequestType;
      [ReadOnly]
      public ComponentLookup<EvacuationRequest> m_EvacuationRequestData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
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
        NativeArray<ServiceRequest> nativeArray2 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
        NativeArray<EvacuationRequest> nativeArray3 = chunk.GetNativeArray<EvacuationRequest>(ref this.m_EvacuationRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          EvacuationRequest componentData1;
          if (this.m_EvacuationRequestData.TryGetComponent(owner, out componentData1))
          {
            Entity service = Entity.Null;
            if (this.m_PublicTransportData.TryGetComponent(componentData1.m_Target, out Game.Vehicles.PublicTransport _))
            {
              Owner componentData2;
              if (targetSeeker.m_Owner.TryGetComponent(componentData1.m_Target, out componentData2))
                service = componentData2.m_Owner;
            }
            else if (targetSeeker.m_PrefabRef.HasComponent(componentData1.m_Target))
              service = componentData1.m_Target;
            else
              continue;
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                EvacuationRequest evacuationRequest = nativeArray3[index2];
                Entity district = Entity.Null;
                if (this.m_CurrentDistrictData.HasComponent(evacuationRequest.m_Target))
                  district = this.m_CurrentDistrictData[evacuationRequest.m_Target].m_District;
                if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  targetSeeker.FindTargets(nativeArray1[index2], evacuationRequest.m_Target, 0.0f, EdgeFlags.DefaultMask, true, false);
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
    private struct FireRescueRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<FireRescueRequest> m_FireRescueRequestType;
      [ReadOnly]
      public ComponentLookup<FireRescueRequest> m_FireRescueRequestData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.FireEngine> m_FireEngineData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.FireStation> m_FireStationData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetTree;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<ServiceRequest> nativeArray2 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
        NativeArray<FireRescueRequest> nativeArray3 = chunk.GetNativeArray<FireRescueRequest>(ref this.m_FireRescueRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          FireRescueRequest componentData1;
          if (this.m_FireRescueRequestData.TryGetComponent(owner, out componentData1))
          {
            Entity service = Entity.Null;
            Game.Vehicles.FireEngine componentData2;
            bool flag;
            if (this.m_FireEngineData.TryGetComponent(componentData1.m_Target, out componentData2))
            {
              Owner componentData3;
              if (targetSeeker.m_Owner.TryGetComponent(componentData1.m_Target, out componentData3))
                service = componentData3.m_Owner;
              flag = (componentData2.m_State & FireEngineFlags.DisasterResponse) > (FireEngineFlags) 0;
            }
            else
            {
              Game.Buildings.FireStation componentData4;
              if (this.m_FireStationData.TryGetComponent(componentData1.m_Target, out componentData4))
              {
                service = componentData1.m_Target;
                flag = (componentData4.m_Flags & FireStationFlags.DisasterResponseAvailable) != 0;
              }
              else
                continue;
            }
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                FireRescueRequest fireRescueRequest = nativeArray3[index2];
                if (fireRescueRequest.m_Type != FireRescueRequestType.Disaster || flag)
                {
                  Entity district = Entity.Null;
                  if (this.m_CurrentDistrictData.HasComponent(fireRescueRequest.m_Target))
                  {
                    district = this.m_CurrentDistrictData[fireRescueRequest.m_Target].m_District;
                  }
                  else
                  {
                    Transform componentData5;
                    if (targetSeeker.m_Transform.TryGetComponent(fireRescueRequest.m_Target, out componentData5))
                    {
                      FirePathfindSetup.FireRescueRequestsJob.DistrictIterator iterator = new FirePathfindSetup.FireRescueRequestsJob.DistrictIterator()
                      {
                        m_Position = componentData5.m_Position.xz,
                        m_DistrictData = this.m_DistrictData,
                        m_Nodes = targetSeeker.m_AreaNode,
                        m_Triangles = targetSeeker.m_AreaTriangle
                      };
                      this.m_AreaTree.Iterate<FirePathfindSetup.FireRescueRequestsJob.DistrictIterator>(ref iterator);
                      district = iterator.m_Result;
                    }
                  }
                  if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  {
                    targetSeeker.FindTargets(nativeArray1[index2], fireRescueRequest.m_Target, 0.0f, EdgeFlags.DefaultMask, true, false);
                    Transform componentData6;
                    if (targetSeeker.m_Transform.TryGetComponent(fireRescueRequest.m_Target, out componentData6))
                    {
                      if ((targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) != (PathMethod) 0 && (targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) != RoadTypes.None)
                      {
                        Entity lane = Entity.Null;
                        float curvePos = 0.0f;
                        float maxValue = float.MaxValue;
                        targetSeeker.m_AirwayData.helicopterMap.FindClosestLane(componentData6.m_Position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
                        if (lane != Entity.Null)
                          targetSeeker.m_Buffer.Enqueue(new PathTarget(nativeArray1[index2], lane, curvePos, 0.0f));
                      }
                      float num = 30f;
                      CommonPathfindSetup.TargetIterator iterator = new CommonPathfindSetup.TargetIterator()
                      {
                        m_Entity = nativeArray1[index2],
                        m_Bounds = new Bounds3(componentData6.m_Position - num, componentData6.m_Position + num),
                        m_Position = componentData6.m_Position,
                        m_MaxDistance = num,
                        m_TargetSeeker = targetSeeker,
                        m_Flags = EdgeFlags.DefaultMask,
                        m_CompositionData = this.m_CompositionData,
                        m_NetCompositionData = this.m_NetCompositionData
                      };
                      this.m_NetTree.Iterate<CommonPathfindSetup.TargetIterator>(ref iterator);
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
