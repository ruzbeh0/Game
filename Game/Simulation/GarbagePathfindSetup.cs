// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GarbagePathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
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
  public struct GarbagePathfindSetup
  {
    private EntityQuery m_GarbageCollectorQuery;
    private EntityQuery m_GarbageTransferQuery;
    private EntityQuery m_GarbageCollectionRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<GarbageCollectionRequest> m_GarbageCollectionRequestType;
    private ComponentTypeHandle<Game.Buildings.GarbageFacility> m_GarbageFacilityType;
    private ComponentTypeHandle<Game.Vehicles.GarbageTruck> m_GarbageTruckType;
    private ComponentTypeHandle<PrefabRef> m_PrefabRefType;
    private BufferTypeHandle<PathElement> m_PathElementType;
    private BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
    private BufferTypeHandle<Resources> m_ResourcesType;
    private BufferTypeHandle<TradeCost> m_TradeCostType;
    private BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
    private ComponentLookup<PathInformation> m_PathInformationData;
    private ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
    private ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
    private ComponentLookup<Game.City.City> m_CityData;
    private ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
    private ComponentLookup<Game.Vehicles.GarbageTruck> m_GarbageTruckData;
    private ComponentLookup<StorageLimitData> m_StorageLimitData;
    private ComponentLookup<StorageCompanyData> m_StorageCompanyData;
    private BufferLookup<PathElement> m_PathElements;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private CitySystem m_CitySystem;

    public GarbagePathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_GarbageCollectorQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(),
          ComponentType.ReadOnly<Game.Vehicles.GarbageTruck>()
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
      this.m_GarbageTransferQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.GarbageFacility>(),
          ComponentType.ReadOnly<ServiceDispatch>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Companies.StorageCompany>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Resources>(),
          ComponentType.ReadOnly<TradeCost>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_GarbageCollectionRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<GarbageCollectionRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_OutsideConnectionType = system.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_GarbageCollectionRequestType = system.GetComponentTypeHandle<GarbageCollectionRequest>(true);
      this.m_GarbageFacilityType = system.GetComponentTypeHandle<Game.Buildings.GarbageFacility>(true);
      this.m_GarbageTruckType = system.GetComponentTypeHandle<Game.Vehicles.GarbageTruck>(true);
      this.m_PrefabRefType = system.GetComponentTypeHandle<PrefabRef>(true);
      this.m_PathElementType = system.GetBufferTypeHandle<PathElement>(true);
      this.m_ServiceDispatchType = system.GetBufferTypeHandle<ServiceDispatch>(true);
      this.m_ResourcesType = system.GetBufferTypeHandle<Resources>(true);
      this.m_TradeCostType = system.GetBufferTypeHandle<TradeCost>(true);
      this.m_InstalledUpgradeType = system.GetBufferTypeHandle<InstalledUpgrade>(true);
      this.m_PathInformationData = system.GetComponentLookup<PathInformation>(true);
      this.m_GarbageCollectionRequestData = system.GetComponentLookup<GarbageCollectionRequest>(true);
      this.m_OutsideConnections = system.GetComponentLookup<Game.Objects.OutsideConnection>(true);
      this.m_CurrentDistrictData = system.GetComponentLookup<CurrentDistrict>(true);
      this.m_GarbageTruckData = system.GetComponentLookup<Game.Vehicles.GarbageTruck>(true);
      this.m_StorageLimitData = system.GetComponentLookup<StorageLimitData>(true);
      this.m_StorageCompanyData = system.GetComponentLookup<StorageCompanyData>(true);
      this.m_CityData = system.GetComponentLookup<Game.City.City>(true);
      this.m_PathElements = system.GetBufferLookup<PathElement>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_CitySystem = system.World.GetOrCreateSystemManaged<CitySystem>();
    }

    public JobHandle SetupGarbageCollector(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_GarbageFacilityType.Update((SystemBase) system);
      this.m_GarbageTruckType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_GarbageCollectionRequestData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      this.m_OutsideConnections.Update((SystemBase) system);
      this.m_CityData.Update((SystemBase) system);
      return new GarbagePathfindSetup.SetupGarbageCollectorsJob()
      {
        m_EntityType = this.m_EntityType,
        m_GarbageFacilityType = this.m_GarbageFacilityType,
        m_GarbageTruckType = this.m_GarbageTruckType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PathInformationData = this.m_PathInformationData,
        m_GarbageCollectionRequestData = this.m_GarbageCollectionRequestData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_OutsideConnections = this.m_OutsideConnections,
        m_CityData = this.m_CityData,
        m_City = this.m_CitySystem.City,
        m_SetupData = setupData
      }.ScheduleParallel<GarbagePathfindSetup.SetupGarbageCollectorsJob>(this.m_GarbageCollectorQuery, inputDeps);
    }

    public JobHandle SetupGarbageTransfer(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_GarbageFacilityType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_OutsideConnectionType.Update((SystemBase) system);
      this.m_ResourcesType.Update((SystemBase) system);
      this.m_TradeCostType.Update((SystemBase) system);
      this.m_InstalledUpgradeType.Update((SystemBase) system);
      this.m_StorageCompanyData.Update((SystemBase) system);
      this.m_StorageLimitData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new GarbagePathfindSetup.SetupGarbageTransferJob()
      {
        m_EntityType = this.m_EntityType,
        m_GarbageFacilityType = this.m_GarbageFacilityType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_OutsideConnectionType = this.m_OutsideConnectionType,
        m_ResourcesType = this.m_ResourcesType,
        m_TradeCostType = this.m_TradeCostType,
        m_InstalledUpgradeType = this.m_InstalledUpgradeType,
        m_StorageCompanyData = this.m_StorageCompanyData,
        m_StorageLimitData = this.m_StorageLimitData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<GarbagePathfindSetup.SetupGarbageTransferJob>(this.m_GarbageTransferQuery, inputDeps);
    }

    public JobHandle SetupGarbageCollectorRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_GarbageCollectionRequestType.Update((SystemBase) system);
      this.m_GarbageCollectionRequestData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_GarbageTruckData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new GarbagePathfindSetup.GarbageCollectorRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_GarbageCollectionRequestType = this.m_GarbageCollectionRequestType,
        m_GarbageCollectionRequestData = this.m_GarbageCollectionRequestData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_GarbageTruckData = this.m_GarbageTruckData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<GarbagePathfindSetup.GarbageCollectorRequestsJob>(this.m_GarbageCollectionRequestQuery, inputDeps);
    }

    [BurstCompile]
    private struct SetupGarbageCollectorsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> m_GarbageFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.GarbageTruck> m_GarbageTruckType;
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
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
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
        NativeArray<Game.Buildings.GarbageFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.GarbageFacility>(ref this.m_GarbageFacilityType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Game.Buildings.GarbageFacility garbageFacility = nativeArray2[index1];
            if ((garbageFacility.m_Flags & (GarbageFacilityFlags.HasAvailableGarbageTrucks | GarbageFacilityFlags.HasAvailableSpace)) == (GarbageFacilityFlags.HasAvailableGarbageTrucks | GarbageFacilityFlags.HasAvailableSpace))
            {
              for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
              {
                Entity entity1;
                Entity owner;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index2, out entity1, out owner, out targetSeeker);
                GarbageCollectionRequest collectionRequest = new GarbageCollectionRequest();
                if (this.m_GarbageCollectionRequestData.HasComponent(owner))
                  collectionRequest = this.m_GarbageCollectionRequestData[owner];
                float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                if ((garbageFacility.m_Flags & GarbageFacilityFlags.IndustrialWasteOnly) != (GarbageFacilityFlags) 0)
                {
                  if ((collectionRequest.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) == (GarbageCollectionRequestFlags) 0)
                    continue;
                }
                else if ((collectionRequest.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) != (GarbageCollectionRequestFlags) 0)
                  cost += 30f;
                Entity entity2 = nativeArray1[index1];
                if (AreaUtils.CheckServiceDistrict(entity1, entity2, this.m_ServiceDistricts))
                  targetSeeker.FindTargets(entity2, cost);
              }
            }
          }
        }
        else
        {
          NativeArray<Game.Vehicles.GarbageTruck> nativeArray3 = chunk.GetNativeArray<Game.Vehicles.GarbageTruck>(ref this.m_GarbageTruckType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Entity entity3 = nativeArray1[index3];
            Game.Vehicles.GarbageTruck garbageTruck = nativeArray3[index3];
            if ((garbageTruck.m_State & (GarbageTruckFlags.Disabled | GarbageTruckFlags.EstimatedFull)) == (GarbageTruckFlags) 0)
            {
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity entity4;
                Entity owner;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity4, out owner, out targetSeeker);
                float cost = 0.0f;
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
                GarbageCollectionRequest componentData1;
                this.m_GarbageCollectionRequestData.TryGetComponent(owner, out componentData1);
                if ((garbageTruck.m_State & GarbageTruckFlags.IndustrialWasteOnly) != (GarbageTruckFlags) 0)
                {
                  if ((componentData1.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) == (GarbageCollectionRequestFlags) 0)
                    continue;
                }
                else if ((componentData1.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) != (GarbageCollectionRequestFlags) 0)
                  cost += 30f;
                if ((garbageTruck.m_State & GarbageTruckFlags.Returning) != (GarbageTruckFlags) 0 || nativeArray4.Length == 0)
                {
                  targetSeeker.FindTargets(entity3, cost);
                }
                else
                {
                  PathOwner pathOwner = nativeArray4[index3];
                  DynamicBuffer<ServiceDispatch> dynamicBuffer1 = bufferAccessor2[index3];
                  int num = math.min(garbageTruck.m_RequestCount, dynamicBuffer1.Length);
                  PathElement pathElement = new PathElement();
                  bool flag = false;
                  if (num >= 1)
                  {
                    DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                    if (pathOwner.m_ElementIndex < dynamicBuffer2.Length)
                    {
                      cost += (float) (dynamicBuffer2.Length - pathOwner.m_ElementIndex) * garbageTruck.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
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
    private struct SetupGarbageTransferJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.GarbageFacility> m_GarbageFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourcesType;
      [ReadOnly]
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimitData;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyData;
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
        if (chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType))
          return;
        NativeArray<Game.Buildings.GarbageFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.GarbageFacility>(ref this.m_GarbageFacilityType);
        if (nativeArray2.Length == 0)
          return;
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Game.Buildings.GarbageFacility garbageFacility = nativeArray2[index1];
          for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
          {
            PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
            // ISSUE: reference to a compiler-generated method
            this.m_SetupData.GetItem(index2, out Entity _, out targetSeeker);
            float num1 = targetSeeker.m_SetupQueueTarget.m_Value2;
            if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.RequireTransport) == SetupTargetFlags.None || (garbageFacility.m_Flags & GarbageFacilityFlags.HasAvailableDeliveryTrucks) != (GarbageFacilityFlags) 0)
            {
              float num2 = math.max(0.0f, 1f - num1);
              if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) != SetupTargetFlags.None)
              {
                if ((double) garbageFacility.m_AcceptGarbagePriority > (double) num2)
                {
                  Entity entity = nativeArray1[index1];
                  targetSeeker.FindTargets(entity, 120f * math.saturate((float) (1.0 - ((double) garbageFacility.m_AcceptGarbagePriority - (double) num2))));
                }
              }
              else if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Export) != SetupTargetFlags.None && (double) garbageFacility.m_DeliverGarbagePriority > (double) num2)
              {
                Entity entity = nativeArray1[index1];
                targetSeeker.FindTargets(entity, 120f * math.saturate((float) (1.0 - ((double) garbageFacility.m_DeliverGarbagePriority - (double) num2))));
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
    private struct GarbageCollectorRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<GarbageCollectionRequest> m_GarbageCollectionRequestType;
      [ReadOnly]
      public ComponentLookup<GarbageCollectionRequest> m_GarbageCollectionRequestData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.GarbageTruck> m_GarbageTruckData;
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
        NativeArray<GarbageCollectionRequest> nativeArray3 = chunk.GetNativeArray<GarbageCollectionRequest>(ref this.m_GarbageCollectionRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          GarbageCollectionRequest componentData1;
          if (this.m_GarbageCollectionRequestData.TryGetComponent(owner, out componentData1))
          {
            Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
            Entity service = Entity.Null;
            if (this.m_GarbageTruckData.HasComponent(componentData1.m_Target))
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
                GarbageCollectionRequest collectionRequest = nativeArray3[index2];
                Entity district = Entity.Null;
                if (this.m_CurrentDistrictData.HasComponent(collectionRequest.m_Target))
                  district = this.m_CurrentDistrictData[collectionRequest.m_Target].m_District;
                float cost = random.NextFloat(30f);
                if ((componentData1.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) != (GarbageCollectionRequestFlags) 0)
                {
                  if ((collectionRequest.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) == (GarbageCollectionRequestFlags) 0)
                    continue;
                }
                else if ((collectionRequest.m_Flags & GarbageCollectionRequestFlags.IndustrialWaste) != (GarbageCollectionRequestFlags) 0)
                  cost += 30f;
                if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  targetSeeker.FindTargets(nativeArray1[index2], collectionRequest.m_Target, cost, EdgeFlags.DefaultMask, true, false);
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
  }
}
