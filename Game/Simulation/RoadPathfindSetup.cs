// Decompiled with JetBrains decompiler
// Type: Game.Simulation.RoadPathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
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
  public struct RoadPathfindSetup
  {
    private EntityQuery m_MaintenanceProviderQuery;
    private EntityQuery m_RandomTrafficQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private EntityQuery m_MaintenanceRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<MaintenanceRequest> m_MaintenanceRequestType;
    private ComponentTypeHandle<Game.Buildings.MaintenanceDepot> m_MaintenanceDepotType;
    private ComponentTypeHandle<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleType;
    private ComponentTypeHandle<PrefabRef> m_PrefabRefType;
    private BufferTypeHandle<PathElement> m_PathElementType;
    private BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
    private ComponentLookup<PathInformation> m_PathInformationData;
    private ComponentLookup<RandomTrafficRequest> m_RandomTrafficRequestData;
    private ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
    private ComponentLookup<Game.Objects.Surface> m_SurfaceData;
    private ComponentLookup<Game.Buildings.Park> m_ParkData;
    private ComponentLookup<Game.Net.Edge> m_EdgeData;
    private ComponentLookup<NetCondition> m_NetConditionData;
    private ComponentLookup<Composition> m_CompositionData;
    private ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
    private ComponentLookup<BorderDistrict> m_BorderDistrictData;
    private ComponentLookup<District> m_DistrictData;
    private ComponentLookup<Vehicle> m_VehicleData;
    private ComponentLookup<MaintenanceDepotData> m_PrefabMaintenanceDepotData;
    private ComponentLookup<MaintenanceVehicleData> m_PrefabMaintenanceVehicleData;
    private ComponentLookup<TrafficSpawnerData> m_PrefabTrafficSpawnerData;
    private ComponentLookup<NetCompositionData> m_NetCompositionData;
    private BufferLookup<PathElement> m_PathElements;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;

    public RoadPathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_MaintenanceProviderQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.MaintenanceDepot>(),
          ComponentType.ReadOnly<Game.Vehicles.MaintenanceVehicle>()
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
      this.m_RandomTrafficQuery = system.GetSetupQuery(ComponentType.ReadOnly<Game.Buildings.TrafficSpawner>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated method
      this.m_OutsideConnectionQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        },
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Game.Objects.ElectricityOutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.WaterPipeOutsideConnection>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_MaintenanceRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<MaintenanceRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_OutsideConnectionType = system.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_MaintenanceRequestType = system.GetComponentTypeHandle<MaintenanceRequest>(true);
      this.m_MaintenanceDepotType = system.GetComponentTypeHandle<Game.Buildings.MaintenanceDepot>(true);
      this.m_MaintenanceVehicleType = system.GetComponentTypeHandle<Game.Vehicles.MaintenanceVehicle>(true);
      this.m_PrefabRefType = system.GetComponentTypeHandle<PrefabRef>(true);
      this.m_PathElementType = system.GetBufferTypeHandle<PathElement>(true);
      this.m_ServiceDispatchType = system.GetBufferTypeHandle<ServiceDispatch>(true);
      this.m_PathInformationData = system.GetComponentLookup<PathInformation>(true);
      this.m_RandomTrafficRequestData = system.GetComponentLookup<RandomTrafficRequest>(true);
      this.m_MaintenanceRequestData = system.GetComponentLookup<MaintenanceRequest>(true);
      this.m_SurfaceData = system.GetComponentLookup<Game.Objects.Surface>(true);
      this.m_ParkData = system.GetComponentLookup<Game.Buildings.Park>(true);
      this.m_EdgeData = system.GetComponentLookup<Game.Net.Edge>(true);
      this.m_NetConditionData = system.GetComponentLookup<NetCondition>(true);
      this.m_CompositionData = system.GetComponentLookup<Composition>(true);
      this.m_CurrentDistrictData = system.GetComponentLookup<CurrentDistrict>(true);
      this.m_BorderDistrictData = system.GetComponentLookup<BorderDistrict>(true);
      this.m_DistrictData = system.GetComponentLookup<District>(true);
      this.m_VehicleData = system.GetComponentLookup<Vehicle>(true);
      this.m_PrefabMaintenanceDepotData = system.GetComponentLookup<MaintenanceDepotData>(true);
      this.m_PrefabMaintenanceVehicleData = system.GetComponentLookup<MaintenanceVehicleData>(true);
      this.m_PrefabTrafficSpawnerData = system.GetComponentLookup<TrafficSpawnerData>(true);
      this.m_NetCompositionData = system.GetComponentLookup<NetCompositionData>(true);
      this.m_PathElements = system.GetBufferLookup<PathElement>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_AreaSearchSystem = system.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      this.m_NetSearchSystem = system.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
    }

    public JobHandle SetupMaintenanceProviders(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_MaintenanceDepotType.Update((SystemBase) system);
      this.m_MaintenanceVehicleType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_PrefabMaintenanceDepotData.Update((SystemBase) system);
      this.m_PrefabMaintenanceVehicleData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new RoadPathfindSetup.SetupMaintenanceProvidersJob()
      {
        m_EntityType = this.m_EntityType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_MaintenanceDepotType = this.m_MaintenanceDepotType,
        m_MaintenanceVehicleType = this.m_MaintenanceVehicleType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PathInformationData = this.m_PathInformationData,
        m_PrefabMaintenanceDepotData = this.m_PrefabMaintenanceDepotData,
        m_PrefabMaintenanceVehicleData = this.m_PrefabMaintenanceVehicleData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<RoadPathfindSetup.SetupMaintenanceProvidersJob>(this.m_MaintenanceProviderQuery, inputDeps);
    }

    public JobHandle SetupRandomTraffic(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_RandomTrafficRequestData.Update((SystemBase) system);
      this.m_PrefabTrafficSpawnerData.Update((SystemBase) system);
      return new RoadPathfindSetup.SetupRandomTrafficJob()
      {
        m_EntityType = this.m_EntityType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_RandomTrafficRequestData = this.m_RandomTrafficRequestData,
        m_PrefabTrafficSpawnerData = this.m_PrefabTrafficSpawnerData,
        m_SetupData = setupData
      }.ScheduleParallel<RoadPathfindSetup.SetupRandomTrafficJob>(this.m_RandomTrafficQuery, inputDeps);
    }

    public JobHandle SetupOutsideConnections(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_OutsideConnectionType.Update((SystemBase) system);
      return new RoadPathfindSetup.SetupOutsideConnectionsJob()
      {
        m_EntityType = this.m_EntityType,
        m_OutsideConnectionType = this.m_OutsideConnectionType,
        m_SetupData = setupData
      }.ScheduleParallel<RoadPathfindSetup.SetupOutsideConnectionsJob>(this.m_OutsideConnectionQuery, inputDeps);
    }

    public JobHandle SetupMaintenanceRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_MaintenanceRequestType.Update((SystemBase) system);
      this.m_MaintenanceRequestData.Update((SystemBase) system);
      this.m_SurfaceData.Update((SystemBase) system);
      this.m_ParkData.Update((SystemBase) system);
      this.m_EdgeData.Update((SystemBase) system);
      this.m_NetConditionData.Update((SystemBase) system);
      this.m_CompositionData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_BorderDistrictData.Update((SystemBase) system);
      this.m_DistrictData.Update((SystemBase) system);
      this.m_VehicleData.Update((SystemBase) system);
      this.m_NetCompositionData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      JobHandle jobHandle = new RoadPathfindSetup.MaintenanceRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_MaintenanceRequestType = this.m_MaintenanceRequestType,
        m_MaintenanceRequestData = this.m_MaintenanceRequestData,
        m_SurfaceData = this.m_SurfaceData,
        m_ParkData = this.m_ParkData,
        m_EdgeData = this.m_EdgeData,
        m_NetConditionData = this.m_NetConditionData,
        m_CompositionData = this.m_CompositionData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_BorderDistrictData = this.m_BorderDistrictData,
        m_DistrictData = this.m_DistrictData,
        m_VehicleData = this.m_VehicleData,
        m_NetCompositionData = this.m_NetCompositionData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies1),
        m_NetTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies2),
        m_SetupData = setupData
      }.ScheduleParallel<RoadPathfindSetup.MaintenanceRequestsJob>(this.m_MaintenanceRequestQuery, JobHandle.CombineDependencies(inputDeps, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      return jobHandle;
    }

    [BurstCompile]
    private struct SetupMaintenanceProvidersJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.MaintenanceDepot> m_MaintenanceDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleType;
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
      public ComponentLookup<MaintenanceDepotData> m_PrefabMaintenanceDepotData;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicleData> m_PrefabMaintenanceVehicleData;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        NativeArray<Game.Buildings.MaintenanceDepot> nativeArray3 = chunk.GetNativeArray<Game.Buildings.MaintenanceDepot>(ref this.m_MaintenanceDepotType);
        if (nativeArray3.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray3.Length; ++index1)
          {
            if ((nativeArray3[index1].m_Flags & MaintenanceDepotFlags.HasAvailableVehicles) != (MaintenanceDepotFlags) 0)
            {
              for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
              {
                Entity entity1;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index2, out entity1, out targetSeeker);
                Entity entity2 = nativeArray1[index1];
                if (AreaUtils.CheckServiceDistrict(entity1, entity2, this.m_ServiceDistricts) && (this.m_PrefabMaintenanceDepotData[nativeArray2[index1].m_Prefab].m_MaintenanceType & targetSeeker.m_SetupQueueTarget.m_MaintenanceType) == targetSeeker.m_SetupQueueTarget.m_MaintenanceType)
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
          NativeArray<Game.Vehicles.MaintenanceVehicle> nativeArray4 = chunk.GetNativeArray<Game.Vehicles.MaintenanceVehicle>(ref this.m_MaintenanceVehicleType);
          if (nativeArray4.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray5 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray6 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
          {
            Entity entity3 = nativeArray1[index3];
            Game.Vehicles.MaintenanceVehicle maintenanceVehicle = nativeArray4[index3];
            MaintenanceVehicleData maintenanceVehicleData = this.m_PrefabMaintenanceVehicleData[nativeArray2[index3].m_Prefab];
            if ((maintenanceVehicle.m_State & (MaintenanceVehicleFlags.EstimatedFull | MaintenanceVehicleFlags.Disabled)) == (MaintenanceVehicleFlags) 0)
            {
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity entity4;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity4, out targetSeeker);
                if ((maintenanceVehicleData.m_MaintenanceType & targetSeeker.m_SetupQueueTarget.m_MaintenanceType) == targetSeeker.m_SetupQueueTarget.m_MaintenanceType && (nativeArray6.Length == 0 || AreaUtils.CheckServiceDistrict(entity4, nativeArray6[index3].m_Owner, this.m_ServiceDistricts)))
                {
                  if ((maintenanceVehicle.m_State & MaintenanceVehicleFlags.Returning) != (MaintenanceVehicleFlags) 0 || nativeArray5.Length == 0)
                  {
                    targetSeeker.FindTargets(entity3, 0.0f);
                  }
                  else
                  {
                    PathOwner pathOwner = nativeArray5[index3];
                    DynamicBuffer<ServiceDispatch> dynamicBuffer1 = bufferAccessor2[index3];
                    int num = math.min(maintenanceVehicle.m_RequestCount, dynamicBuffer1.Length);
                    PathElement pathElement = new PathElement();
                    float cost = 0.0f;
                    bool flag = false;
                    if (num >= 1)
                    {
                      DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                      if (pathOwner.m_ElementIndex < dynamicBuffer2.Length)
                      {
                        cost += (float) (dynamicBuffer2.Length - pathOwner.m_ElementIndex) * maintenanceVehicle.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
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
    private struct SetupRandomTrafficJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<RandomTrafficRequest> m_RandomTrafficRequestData;
      [ReadOnly]
      public ComponentLookup<TrafficSpawnerData> m_PrefabTrafficSpawnerData;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out owner, out targetSeeker);
          Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          RandomTrafficRequest randomTrafficRequest = new RandomTrafficRequest();
          if (this.m_RandomTrafficRequestData.HasComponent(owner))
            randomTrafficRequest = this.m_RandomTrafficRequestData[owner];
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity2 = nativeArray1[index2];
            if (!(entity2 == entity1))
            {
              PrefabRef prefabRef = nativeArray2[index2];
              TrafficSpawnerData trafficSpawnerData = new TrafficSpawnerData();
              if (this.m_PrefabTrafficSpawnerData.HasComponent(prefabRef.m_Prefab))
                trafficSpawnerData = this.m_PrefabTrafficSpawnerData[prefabRef.m_Prefab];
              if ((randomTrafficRequest.m_RoadType & trafficSpawnerData.m_RoadType) != RoadTypes.None || (randomTrafficRequest.m_TrackType & trafficSpawnerData.m_TrackType) != TrackTypes.None)
              {
                float cost = random.NextFloat(10000f);
                targetSeeker.FindTargets(entity2, cost);
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
    private struct SetupOutsideConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          float max = targetSeeker.m_SetupQueueTarget.m_Value2;
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Entity entity = nativeArray[index2];
            float cost = 0.0f;
            if ((double) max > 0.0)
              cost = random.NextFloat(max);
            targetSeeker.FindTargets(entity, cost);
            if ((targetSeeker.m_SetupQueueTarget.m_Methods & PathMethod.Flying) != (PathMethod) 0 && targetSeeker.m_Transform.HasComponent(entity))
            {
              float3 position = targetSeeker.m_Transform[entity].m_Position;
              if ((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Helicopter) != RoadTypes.None)
              {
                Entity lane = Entity.Null;
                float curvePos = 0.0f;
                float maxValue = float.MaxValue;
                targetSeeker.m_AirwayData.helicopterMap.FindClosestLane(position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
                if (lane != Entity.Null)
                  targetSeeker.m_Buffer.Enqueue(new PathTarget(entity, lane, curvePos, cost));
              }
              if ((targetSeeker.m_SetupQueueTarget.m_FlyingTypes & RoadTypes.Airplane) != RoadTypes.None)
              {
                Entity lane = Entity.Null;
                float curvePos = 0.0f;
                float maxValue = float.MaxValue;
                targetSeeker.m_AirwayData.airplaneMap.FindClosestLane(position, targetSeeker.m_Curve, ref lane, ref curvePos, ref maxValue);
                if (lane != Entity.Null)
                  targetSeeker.m_Buffer.Enqueue(new PathTarget(entity, lane, curvePos, cost));
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
    private struct MaintenanceRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceRequest> m_MaintenanceRequestType;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Surface> m_SurfaceData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_ParkData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<NetCondition> m_NetConditionData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
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
        NativeArray<MaintenanceRequest> nativeArray3 = chunk.GetNativeArray<MaintenanceRequest>(ref this.m_MaintenanceRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          MaintenanceRequest componentData1;
          if (this.m_MaintenanceRequestData.TryGetComponent(owner, out componentData1))
          {
            Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
            Entity service = Entity.Null;
            if (this.m_VehicleData.HasComponent(componentData1.m_Target))
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
                MaintenanceRequest maintenanceRequest = nativeArray3[index2];
                MaintenanceType maintenanceType = BuildingUtils.GetMaintenanceType(maintenanceRequest.m_Target, ref this.m_ParkData, ref this.m_NetConditionData, ref this.m_EdgeData, ref this.m_SurfaceData, ref this.m_VehicleData);
                if ((maintenanceType & targetSeeker.m_SetupQueueTarget.m_MaintenanceType) == maintenanceType)
                {
                  float cost = 0.0f;
                  if ((maintenanceType & MaintenanceType.Vehicle) == MaintenanceType.None)
                    cost = random.NextFloat(30f);
                  Entity district = Entity.Null;
                  CurrentDistrict componentData3;
                  if (this.m_CurrentDistrictData.TryGetComponent(maintenanceRequest.m_Target, out componentData3))
                  {
                    district = componentData3.m_District;
                  }
                  else
                  {
                    BorderDistrict componentData4;
                    if (this.m_BorderDistrictData.TryGetComponent(maintenanceRequest.m_Target, out componentData4))
                    {
                      district = componentData4.m_Right;
                    }
                    else
                    {
                      Transform componentData5;
                      if (targetSeeker.m_Transform.TryGetComponent(maintenanceRequest.m_Target, out componentData5))
                      {
                        RoadPathfindSetup.MaintenanceRequestsJob.DistrictIterator iterator = new RoadPathfindSetup.MaintenanceRequestsJob.DistrictIterator()
                        {
                          m_Position = componentData5.m_Position.xz,
                          m_DistrictData = this.m_DistrictData,
                          m_Nodes = targetSeeker.m_AreaNode,
                          m_Triangles = targetSeeker.m_AreaTriangle
                        };
                        this.m_AreaTree.Iterate<RoadPathfindSetup.MaintenanceRequestsJob.DistrictIterator>(ref iterator);
                        district = iterator.m_Result;
                      }
                    }
                  }
                  if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  {
                    targetSeeker.FindTargets(nativeArray1[index2], maintenanceRequest.m_Target, cost, EdgeFlags.DefaultMask, true, false);
                    Transform componentData6;
                    if (targetSeeker.m_Transform.TryGetComponent(maintenanceRequest.m_Target, out componentData6))
                    {
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
