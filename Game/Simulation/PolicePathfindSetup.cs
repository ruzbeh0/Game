// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PolicePathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Creatures;
using Game.Events;
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
  public struct PolicePathfindSetup
  {
    private EntityQuery m_PolicePatrolQuery;
    private EntityQuery m_CrimeProducerQuery;
    private EntityQuery m_PrisonerTransportQuery;
    private EntityQuery m_PrisonerTransportRequestQuery;
    private EntityQuery m_PoliceRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<PrisonerTransportRequest> m_PrisonerTransportRequestType;
    private ComponentTypeHandle<PolicePatrolRequest> m_PolicePatrolRequestType;
    private ComponentTypeHandle<PoliceEmergencyRequest> m_PoliceEmergencyRequestType;
    private ComponentTypeHandle<Game.Buildings.PoliceStation> m_PoliceStationType;
    private ComponentTypeHandle<CrimeProducer> m_CrimeProducerType;
    private ComponentTypeHandle<Game.Buildings.Prison> m_PrisonType;
    private ComponentTypeHandle<Game.Vehicles.PoliceCar> m_PoliceCarType;
    private ComponentTypeHandle<Helicopter> m_HelicopterType;
    private ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
    private BufferTypeHandle<PathElement> m_PathElementType;
    private BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
    private BufferTypeHandle<Passenger> m_PassengerType;
    private ComponentLookup<PathInformation> m_PathInformationData;
    private ComponentLookup<PolicePatrolRequest> m_PolicePatrolRequestData;
    private ComponentLookup<PoliceEmergencyRequest> m_PoliceEmergencyRequestData;
    private ComponentLookup<PrisonerTransportRequest> m_PrisonerTransportRequestData;
    private ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
    private ComponentLookup<Composition> m_CompositionData;
    private ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
    private ComponentLookup<District> m_DistrictData;
    private ComponentLookup<Game.Buildings.PoliceStation> m_PoliceStationData;
    private ComponentLookup<Creature> m_CreatureData;
    private ComponentLookup<Vehicle> m_VehicleData;
    private ComponentLookup<Game.Vehicles.PoliceCar> m_PoliceCarData;
    private ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
    private ComponentLookup<AccidentSite> m_AccidentSiteData;
    private ComponentLookup<NetCompositionData> m_NetCompositionData;
    private ComponentLookup<Game.City.City> m_CityData;
    private BufferLookup<PathElement> m_PathElements;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private BufferLookup<TargetElement> m_TargetElements;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private CitySystem m_CitySystem;

    public PolicePathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_PolicePatrolQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.PoliceStation>(),
          ComponentType.ReadOnly<Game.Vehicles.PoliceCar>()
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
      this.m_CrimeProducerQuery = system.GetSetupQuery(ComponentType.ReadOnly<CrimeProducer>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated method
      this.m_PrisonerTransportQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.Prison>(),
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
      this.m_PrisonerTransportRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<PrisonerTransportRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      // ISSUE: reference to a compiler-generated method
      this.m_PoliceRequestQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<PolicePatrolRequest>(),
          ComponentType.ReadOnly<PoliceEmergencyRequest>()
        },
        None = new ComponentType[2]
        {
          ComponentType.Exclude<Dispatched>(),
          ComponentType.Exclude<PathInformation>()
        }
      });
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_PrisonerTransportRequestType = system.GetComponentTypeHandle<PrisonerTransportRequest>(true);
      this.m_PolicePatrolRequestType = system.GetComponentTypeHandle<PolicePatrolRequest>(true);
      this.m_PoliceEmergencyRequestType = system.GetComponentTypeHandle<PoliceEmergencyRequest>(true);
      this.m_PoliceStationType = system.GetComponentTypeHandle<Game.Buildings.PoliceStation>(true);
      this.m_CrimeProducerType = system.GetComponentTypeHandle<CrimeProducer>(true);
      this.m_PrisonType = system.GetComponentTypeHandle<Game.Buildings.Prison>(true);
      this.m_PoliceCarType = system.GetComponentTypeHandle<Game.Vehicles.PoliceCar>(true);
      this.m_HelicopterType = system.GetComponentTypeHandle<Helicopter>(true);
      this.m_PublicTransportType = system.GetComponentTypeHandle<Game.Vehicles.PublicTransport>(true);
      this.m_PathElementType = system.GetBufferTypeHandle<PathElement>(true);
      this.m_ServiceDispatchType = system.GetBufferTypeHandle<ServiceDispatch>(true);
      this.m_PassengerType = system.GetBufferTypeHandle<Passenger>(true);
      this.m_PathInformationData = system.GetComponentLookup<PathInformation>(true);
      this.m_PolicePatrolRequestData = system.GetComponentLookup<PolicePatrolRequest>(true);
      this.m_PoliceEmergencyRequestData = system.GetComponentLookup<PoliceEmergencyRequest>(true);
      this.m_PrisonerTransportRequestData = system.GetComponentLookup<PrisonerTransportRequest>(true);
      this.m_OutsideConnections = system.GetComponentLookup<Game.Objects.OutsideConnection>(true);
      this.m_CompositionData = system.GetComponentLookup<Composition>(true);
      this.m_CurrentDistrictData = system.GetComponentLookup<CurrentDistrict>(true);
      this.m_DistrictData = system.GetComponentLookup<District>(true);
      this.m_PoliceStationData = system.GetComponentLookup<Game.Buildings.PoliceStation>(true);
      this.m_CreatureData = system.GetComponentLookup<Creature>(true);
      this.m_VehicleData = system.GetComponentLookup<Vehicle>(true);
      this.m_PoliceCarData = system.GetComponentLookup<Game.Vehicles.PoliceCar>(true);
      this.m_PublicTransportData = system.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
      this.m_AccidentSiteData = system.GetComponentLookup<AccidentSite>(true);
      this.m_NetCompositionData = system.GetComponentLookup<NetCompositionData>(true);
      this.m_CityData = system.GetComponentLookup<Game.City.City>(true);
      this.m_PathElements = system.GetBufferLookup<PathElement>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_TargetElements = system.GetBufferLookup<TargetElement>(true);
      this.m_AreaSearchSystem = system.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      this.m_NetSearchSystem = system.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      this.m_CitySystem = system.World.GetOrCreateSystemManaged<CitySystem>();
    }

    public JobHandle SetupPolicePatrols(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PoliceStationType.Update((SystemBase) system);
      this.m_PoliceCarType.Update((SystemBase) system);
      this.m_HelicopterType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PassengerType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_PoliceEmergencyRequestData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      this.m_OutsideConnections.Update((SystemBase) system);
      this.m_CityData.Update((SystemBase) system);
      return new PolicePathfindSetup.SetupPolicePatrolsJob()
      {
        m_EntityType = this.m_EntityType,
        m_PoliceStationType = this.m_PoliceStationType,
        m_PoliceCarType = this.m_PoliceCarType,
        m_HelicopterType = this.m_HelicopterType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PassengerType = this.m_PassengerType,
        m_PathInformationData = this.m_PathInformationData,
        m_PoliceEmergencyRequestData = this.m_PoliceEmergencyRequestData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_OutsideConnections = this.m_OutsideConnections,
        m_CityData = this.m_CityData,
        m_City = this.m_CitySystem.City,
        m_SetupData = setupData
      }.ScheduleParallel<PolicePathfindSetup.SetupPolicePatrolsJob>(this.m_PolicePatrolQuery, inputDeps);
    }

    public JobHandle SetupCrimeProducer(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_CrimeProducerType.Update((SystemBase) system);
      return new PolicePathfindSetup.SetupCrimeProducersJob()
      {
        m_EntityType = this.m_EntityType,
        m_CrimeProducerType = this.m_CrimeProducerType,
        m_SetupData = setupData
      }.ScheduleParallel<PolicePathfindSetup.SetupCrimeProducersJob>(this.m_CrimeProducerQuery, inputDeps);
    }

    public JobHandle SetupPrisonerTransport(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PrisonType.Update((SystemBase) system);
      this.m_PublicTransportType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new PolicePathfindSetup.SetupPrisonerTransportJob()
      {
        m_EntityType = this.m_EntityType,
        m_PrisonType = this.m_PrisonType,
        m_PublicTransportType = this.m_PublicTransportType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PathInformationData = this.m_PathInformationData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<PolicePathfindSetup.SetupPrisonerTransportJob>(this.m_PrisonerTransportQuery, inputDeps);
    }

    public JobHandle SetupPrisonerTransportRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_PrisonerTransportRequestType.Update((SystemBase) system);
      this.m_PrisonerTransportRequestData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_PublicTransportData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new PolicePathfindSetup.PrisonerTransportRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_PrisonerTransportRequestType = this.m_PrisonerTransportRequestType,
        m_PrisonerTransportRequestData = this.m_PrisonerTransportRequestData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_PublicTransportData = this.m_PublicTransportData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<PolicePathfindSetup.PrisonerTransportRequestsJob>(this.m_PrisonerTransportRequestQuery, inputDeps);
    }

    public JobHandle SetupPoliceRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_PolicePatrolRequestType.Update((SystemBase) system);
      this.m_PoliceEmergencyRequestType.Update((SystemBase) system);
      this.m_PolicePatrolRequestData.Update((SystemBase) system);
      this.m_PoliceEmergencyRequestData.Update((SystemBase) system);
      this.m_CompositionData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_DistrictData.Update((SystemBase) system);
      this.m_CreatureData.Update((SystemBase) system);
      this.m_VehicleData.Update((SystemBase) system);
      this.m_PoliceCarData.Update((SystemBase) system);
      this.m_PoliceStationData.Update((SystemBase) system);
      this.m_AccidentSiteData.Update((SystemBase) system);
      this.m_NetCompositionData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      this.m_TargetElements.Update((SystemBase) system);
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new PolicePathfindSetup.PoliceRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_PolicePatrolRequestType = this.m_PolicePatrolRequestType,
        m_PoliceEmergencyRequestType = this.m_PoliceEmergencyRequestType,
        m_PolicePatrolRequestData = this.m_PolicePatrolRequestData,
        m_PoliceEmergencyRequestData = this.m_PoliceEmergencyRequestData,
        m_CompositionData = this.m_CompositionData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_DistrictData = this.m_DistrictData,
        m_CreatureData = this.m_CreatureData,
        m_VehicleData = this.m_VehicleData,
        m_PoliceCarData = this.m_PoliceCarData,
        m_PoliceStationData = this.m_PoliceStationData,
        m_AccidentSiteData = this.m_AccidentSiteData,
        m_NetCompositionData = this.m_NetCompositionData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_TargetElements = this.m_TargetElements,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies1),
        m_NetTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_SetupData = setupData
      }.ScheduleParallel<PolicePathfindSetup.PoliceRequestsJob>(this.m_PoliceRequestQuery, JobHandle.CombineDependencies(inputDeps, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      return jobHandle;
    }

    [BurstCompile]
    private struct SetupPolicePatrolsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PoliceStation> m_PoliceStationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PoliceCar> m_PoliceCarType;
      [ReadOnly]
      public ComponentTypeHandle<Helicopter> m_HelicopterType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public BufferTypeHandle<Passenger> m_PassengerType;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> m_PoliceEmergencyRequestData;
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
        NativeArray<Game.Buildings.PoliceStation> nativeArray2 = chunk.GetNativeArray<Game.Buildings.PoliceStation>(ref this.m_PoliceStationType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Entity entity1 = nativeArray1[index1];
            Game.Buildings.PoliceStation policeStation = nativeArray2[index1];
            for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
            {
              Entity entity2;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index2, out entity2, out targetSeeker);
              PolicePurpose policePurpose = (PolicePurpose) targetSeeker.m_SetupQueueTarget.m_Value;
              if ((policeStation.m_PurposeMask & policePurpose) != (PolicePurpose) 0)
              {
                RoadTypes roadTypes1 = RoadTypes.None;
                if (AreaUtils.CheckServiceDistrict(entity2, entity1, this.m_ServiceDistricts))
                {
                  if ((policeStation.m_Flags & PoliceStationFlags.HasAvailablePatrolCars) != (PoliceStationFlags) 0)
                    roadTypes1 |= RoadTypes.Car;
                  if ((policeStation.m_Flags & PoliceStationFlags.HasAvailablePoliceHelicopters) != (PoliceStationFlags) 0)
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
        }
        else
        {
          NativeArray<Game.Vehicles.PoliceCar> nativeArray3 = chunk.GetNativeArray<Game.Vehicles.PoliceCar>(ref this.m_PoliceCarType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          BufferAccessor<Passenger> bufferAccessor3 = chunk.GetBufferAccessor<Passenger>(ref this.m_PassengerType);
          bool flag1 = chunk.Has<Helicopter>(ref this.m_HelicopterType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Entity entity3 = nativeArray1[index3];
            Game.Vehicles.PoliceCar policeCar = nativeArray3[index3];
            if ((policeCar.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Disabled)) == (PoliceCarFlags) 0)
            {
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity entity4;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity4, out targetSeeker);
                PolicePurpose policePurpose = (PolicePurpose) targetSeeker.m_SetupQueueTarget.m_Value;
                if ((policeCar.m_PurposeMask & policePurpose) != (PolicePurpose) 0 && !((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) == RoadTypes.None & flag1))
                {
                  float cost = 0.0f;
                  if ((policePurpose & PolicePurpose.Patrol) != (PolicePurpose) 0)
                  {
                    if ((policeCar.m_State & (PoliceCarFlags.Empty | PoliceCarFlags.EstimatedShiftEnd)) != PoliceCarFlags.Empty)
                      continue;
                  }
                  else if (bufferAccessor3.Length != 0)
                  {
                    DynamicBuffer<Passenger> dynamicBuffer = bufferAccessor3[index3];
                    cost += (float) dynamicBuffer.Length * 10f;
                  }
                  if (nativeArray5.Length != 0)
                  {
                    if (AreaUtils.CheckServiceDistrict(entity4, nativeArray5[index3].m_Owner, this.m_ServiceDistricts))
                    {
                      if (this.m_OutsideConnections.HasComponent(nativeArray5[index3].m_Owner))
                        cost += 30f;
                    }
                    else
                      continue;
                  }
                  if ((policeCar.m_State & PoliceCarFlags.Returning) != (PoliceCarFlags) 0 || nativeArray4.Length == 0)
                  {
                    targetSeeker.FindTargets(entity3, cost);
                  }
                  else
                  {
                    PathOwner pathOwner = nativeArray4[index3];
                    DynamicBuffer<ServiceDispatch> dynamicBuffer1 = bufferAccessor2[index3];
                    int num = math.min(policeCar.m_RequestCount, dynamicBuffer1.Length);
                    PathElement pathElement = new PathElement();
                    bool flag2 = false;
                    if (num >= 1 && ((policePurpose & (PolicePurpose.Emergency | PolicePurpose.Intelligence)) == (PolicePurpose) 0 || this.m_PoliceEmergencyRequestData.HasComponent(dynamicBuffer1[0].m_Request)))
                    {
                      DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                      if (pathOwner.m_ElementIndex < dynamicBuffer2.Length)
                      {
                        cost += (float) (dynamicBuffer2.Length - pathOwner.m_ElementIndex) * policeCar.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
                        pathElement = dynamicBuffer2[dynamicBuffer2.Length - 1];
                        flag2 = true;
                      }
                    }
                    for (int index5 = 1; index5 < num; ++index5)
                    {
                      Entity request = dynamicBuffer1[index5].m_Request;
                      bool flag3 = this.m_PoliceEmergencyRequestData.HasComponent(request);
                      if ((policePurpose & (PolicePurpose.Emergency | PolicePurpose.Intelligence)) == (PolicePurpose) 0 | flag3)
                      {
                        PathInformation componentData;
                        if (this.m_PathInformationData.TryGetComponent(request, out componentData))
                          cost += componentData.m_Duration * targetSeeker.m_PathfindParameters.m_Weights.time;
                        if (flag3)
                          cost += 30f * targetSeeker.m_PathfindParameters.m_Weights.time;
                        DynamicBuffer<PathElement> bufferData;
                        if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
                        {
                          pathElement = bufferData[bufferData.Length - 1];
                          flag2 = true;
                        }
                      }
                    }
                    if (flag2)
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
    private struct SetupCrimeProducersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CrimeProducer> m_CrimeProducerType;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<CrimeProducer> nativeArray2 = chunk.GetNativeArray<CrimeProducer>(ref this.m_CrimeProducerType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            float num = 10000f / math.sqrt(nativeArray2[index2].m_Crime);
            targetSeeker.FindTargets(entity, random.NextFloat(0.5f, 1.5f) * num);
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
    private struct SetupPrisonerTransportJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Prison> m_PrisonType;
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
        NativeArray<Game.Buildings.Prison> nativeArray2 = chunk.GetNativeArray<Game.Buildings.Prison>(ref this.m_PrisonType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Game.Buildings.Prison prison = nativeArray2[index1];
            for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
            {
              Entity entity1;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index2, out entity1, out targetSeeker);
              if ((prison.m_Flags & (PrisonFlags.HasAvailablePrisonVans | PrisonFlags.HasPrisonerSpace)) == (PrisonFlags.HasAvailablePrisonVans | PrisonFlags.HasPrisonerSpace))
              {
                Entity entity2 = nativeArray1[index1];
                if (AreaUtils.CheckServiceDistrict(entity1, entity2, this.m_ServiceDistricts))
                {
                  float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                  targetSeeker.FindTargets(entity2, cost);
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
            if ((publicTransport.m_State & (PublicTransportFlags.PrisonerTransport | PublicTransportFlags.Disabled | PublicTransportFlags.Full)) == PublicTransportFlags.PrisonerTransport)
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
    private struct PrisonerTransportRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<PrisonerTransportRequest> m_PrisonerTransportRequestType;
      [ReadOnly]
      public ComponentLookup<PrisonerTransportRequest> m_PrisonerTransportRequestData;
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
        NativeArray<PrisonerTransportRequest> nativeArray3 = chunk.GetNativeArray<PrisonerTransportRequest>(ref this.m_PrisonerTransportRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          PrisonerTransportRequest componentData1;
          if (this.m_PrisonerTransportRequestData.TryGetComponent(owner, out componentData1))
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
                PrisonerTransportRequest transportRequest = nativeArray3[index2];
                Entity district = Entity.Null;
                if (this.m_CurrentDistrictData.HasComponent(transportRequest.m_Target))
                  district = this.m_CurrentDistrictData[transportRequest.m_Target].m_District;
                if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  targetSeeker.FindTargets(nativeArray1[index2], transportRequest.m_Target, 0.0f, EdgeFlags.DefaultMask, true, false);
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
    private struct PoliceRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<PolicePatrolRequest> m_PolicePatrolRequestType;
      [ReadOnly]
      public ComponentTypeHandle<PoliceEmergencyRequest> m_PoliceEmergencyRequestType;
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> m_PolicePatrolRequestData;
      [ReadOnly]
      public ComponentLookup<PoliceEmergencyRequest> m_PoliceEmergencyRequestData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Creature> m_CreatureData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PoliceCar> m_PoliceCarData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PoliceStation> m_PoliceStationData;
      [ReadOnly]
      public ComponentLookup<AccidentSite> m_AccidentSiteData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      [ReadOnly]
      public BufferLookup<TargetElement> m_TargetElements;
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
        NativeArray<PolicePatrolRequest> nativeArray3 = chunk.GetNativeArray<PolicePatrolRequest>(ref this.m_PolicePatrolRequestType);
        NativeArray<PoliceEmergencyRequest> nativeArray4 = chunk.GetNativeArray<PoliceEmergencyRequest>(ref this.m_PoliceEmergencyRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          PolicePatrolRequest componentData1;
          Entity entity1;
          PolicePurpose policePurpose1;
          if (this.m_PolicePatrolRequestData.TryGetComponent(owner, out componentData1))
          {
            entity1 = componentData1.m_Target;
            policePurpose1 = PolicePurpose.Patrol | PolicePurpose.Emergency | PolicePurpose.Intelligence;
          }
          else
          {
            PoliceEmergencyRequest componentData2;
            if (this.m_PoliceEmergencyRequestData.TryGetComponent(owner, out componentData2))
            {
              entity1 = componentData2.m_Site;
              policePurpose1 = componentData2.m_Purpose;
            }
            else
              continue;
          }
          Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          Entity service = Entity.Null;
          Game.Vehicles.PoliceCar componentData3;
          PolicePurpose policePurpose2;
          if (this.m_PoliceCarData.TryGetComponent(entity1, out componentData3))
          {
            policePurpose2 = policePurpose1 & componentData3.m_PurposeMask;
            Owner componentData4;
            if (targetSeeker.m_Owner.TryGetComponent(entity1, out componentData4))
              service = componentData4.m_Owner;
          }
          else
          {
            Game.Buildings.PoliceStation componentData5;
            if (this.m_PoliceStationData.TryGetComponent(entity1, out componentData5))
            {
              policePurpose2 = policePurpose1 & componentData5.m_PurposeMask;
              service = entity1;
            }
            else
              continue;
          }
          if ((policePurpose2 & PolicePurpose.Patrol) != (PolicePurpose) 0)
          {
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                PolicePatrolRequest policePatrolRequest = nativeArray3[index2];
                Entity district = Entity.Null;
                if (this.m_CurrentDistrictData.HasComponent(policePatrolRequest.m_Target))
                  district = this.m_CurrentDistrictData[policePatrolRequest.m_Target].m_District;
                if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                {
                  float cost = random.NextFloat(30f);
                  targetSeeker.FindTargets(nativeArray1[index2], policePatrolRequest.m_Target, cost, EdgeFlags.DefaultMask, true, false);
                  Transform componentData6;
                  if (targetSeeker.m_Transform.TryGetComponent(policePatrolRequest.m_Target, out componentData6) && (targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) != (PathMethod) 0 && (targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) != RoadTypes.None)
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
          if ((policePurpose2 & (PolicePurpose.Emergency | PolicePurpose.Intelligence)) != (PolicePurpose) 0)
          {
            targetSeeker.m_SetupQueueTarget.m_Methods &= PathMethod.Road;
            targetSeeker.m_SetupQueueTarget.m_RoadTypes &= RoadTypes.Car;
            for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
            {
              if ((nativeArray2[index3].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                PoliceEmergencyRequest emergencyRequest = nativeArray4[index3];
                if ((policePurpose2 & emergencyRequest.m_Purpose) != (PolicePurpose) 0)
                {
                  Entity district = Entity.Null;
                  if (this.m_CurrentDistrictData.HasComponent(emergencyRequest.m_Target))
                  {
                    district = this.m_CurrentDistrictData[emergencyRequest.m_Target].m_District;
                  }
                  else
                  {
                    Transform componentData7;
                    if (targetSeeker.m_Transform.TryGetComponent(emergencyRequest.m_Target, out componentData7))
                    {
                      PolicePathfindSetup.PoliceRequestsJob.DistrictIterator iterator = new PolicePathfindSetup.PoliceRequestsJob.DistrictIterator()
                      {
                        m_Position = componentData7.m_Position.xz,
                        m_DistrictData = this.m_DistrictData,
                        m_Nodes = targetSeeker.m_AreaNode,
                        m_Triangles = targetSeeker.m_AreaTriangle
                      };
                      this.m_AreaTree.Iterate<PolicePathfindSetup.PoliceRequestsJob.DistrictIterator>(ref iterator);
                      district = iterator.m_Result;
                    }
                  }
                  if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  {
                    AccidentSite componentData8;
                    if (this.m_AccidentSiteData.TryGetComponent(emergencyRequest.m_Site, out componentData8))
                    {
                      DynamicBuffer<TargetElement> bufferData;
                      if (this.m_TargetElements.TryGetBuffer(componentData8.m_Event, out bufferData))
                      {
                        bool allowAccessRestriction = true;
                        this.CheckTarget(nativeArray1[index3], emergencyRequest.m_Site, componentData8, ref targetSeeker, ref allowAccessRestriction);
                        for (int index4 = 0; index4 < bufferData.Length; ++index4)
                        {
                          Entity entity2 = bufferData[index4].m_Entity;
                          if (entity2 != emergencyRequest.m_Site)
                            this.CheckTarget(nativeArray1[index3], entity2, componentData8, ref targetSeeker, ref allowAccessRestriction);
                        }
                      }
                    }
                    else
                      targetSeeker.FindTargets(nativeArray1[index3], emergencyRequest.m_Target, 0.0f, EdgeFlags.DefaultMask, true, false);
                  }
                }
              }
            }
          }
        }
      }

      private void CheckTarget(
        Entity target,
        Entity entity,
        AccidentSite accidentSite,
        ref PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker,
        ref bool allowAccessRestriction)
      {
        if ((accidentSite.m_Flags & AccidentSiteFlags.TrafficAccident) != (AccidentSiteFlags) 0 && !this.m_CreatureData.HasComponent(entity) && !this.m_VehicleData.HasComponent(entity))
          return;
        int targets = targetSeeker.FindTargets(target, entity, 0.0f, EdgeFlags.DefaultMask, allowAccessRestriction, false);
        allowAccessRestriction &= targets == 0;
        Entity entity1 = entity;
        if (targetSeeker.m_CurrentTransport.HasComponent(entity1))
          entity1 = targetSeeker.m_CurrentTransport[entity1].m_CurrentTransport;
        else if (targetSeeker.m_CurrentBuilding.HasComponent(entity1))
          entity1 = targetSeeker.m_CurrentBuilding[entity1].m_CurrentBuilding;
        if (!targetSeeker.m_Transform.HasComponent(entity1))
          return;
        float3 position = targetSeeker.m_Transform[entity1].m_Position;
        float num = 30f;
        CommonPathfindSetup.TargetIterator iterator = new CommonPathfindSetup.TargetIterator()
        {
          m_Entity = target,
          m_Bounds = new Bounds3(position - num, position + num),
          m_Position = position,
          m_MaxDistance = num,
          m_TargetSeeker = targetSeeker,
          m_Flags = EdgeFlags.DefaultMask,
          m_CompositionData = this.m_CompositionData,
          m_NetCompositionData = this.m_NetCompositionData
        };
        this.m_NetTree.Iterate<CommonPathfindSetup.TargetIterator>(ref iterator);
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
