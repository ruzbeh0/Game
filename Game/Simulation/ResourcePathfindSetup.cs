// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ResourcePathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
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
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  public struct ResourcePathfindSetup
  {
    public static readonly float kOutsideConnectionAmountBasedPenalty = 1f / 1000f;
    public static readonly float kCargoStationAmountBasedPenalty = 0.0001f;
    public static readonly int kCargoStationPerRequestPenalty = 100;
    public static readonly int kCargoStationVehiclePenalty = 5000;
    public static readonly int kCargoStationMaxRequestAmount = 5;
    public static readonly int kCargoStationMaxTripNeededQueue = 5;
    private EntityQuery m_ResourceSellerQuery;
    private EntityQuery m_ExportTargetQuery;
    private EntityQuery m_StorageQuery;
    private ResourceSystem m_ResourceSystem;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
    private ComponentTypeHandle<Game.Companies.StorageCompany> m_StorageCompanyType;
    private ComponentTypeHandle<PrefabRef> m_PrefabType;
    private BufferTypeHandle<TradeCost> m_TradeCostType;
    private BufferTypeHandle<StorageTransferRequest> m_StorageTransferRequestType;
    private BufferTypeHandle<Game.Economy.Resources> m_ResourceType;
    private BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
    private BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
    private BufferTypeHandle<TripNeeded> m_TripNeededType;
    private ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
    private ComponentLookup<ServiceCompanyData> m_ServiceCompanies;
    private ComponentLookup<ServiceAvailable> m_ServiceAvailables;
    private ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanys;
    private ComponentLookup<StorageLimitData> m_StorageLimits;
    private ComponentLookup<TransportCompanyData> m_TransportCompanyData;
    private ComponentLookup<Game.Buildings.CargoTransportStation> m_CargoTransportStations;
    private ComponentLookup<PropertyRenter> m_PropertyRenters;
    private ComponentLookup<PrefabRef> m_Prefabs;
    private ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
    private ComponentLookup<ResourceData> m_ResourceDatas;
    private ComponentLookup<StorageCompanyData> m_StorageCompanyDatas;
    private ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
    private ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
    private BufferLookup<Game.Economy.Resources> m_Resources;
    private BufferLookup<TradeCost> m_TradeCosts;

    public ResourcePathfindSetup(PathfindSetupSystem system)
    {
      this.m_ResourceSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSellerQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Game.Economy.Resources>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Companies.StorageCompany>(),
          ComponentType.ReadOnly<Game.Buildings.CargoTransportStation>(),
          ComponentType.ReadOnly<ResourceSeller>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_ExportTargetQuery = system.GetSetupQuery(ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Game.Economy.Resources>(), ComponentType.ReadOnly<TradeCost>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated method
      this.m_StorageQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Companies.StorageCompany>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_OutsideConnectionType = system.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
      this.m_StorageCompanyType = system.GetComponentTypeHandle<Game.Companies.StorageCompany>(true);
      this.m_PrefabType = system.GetComponentTypeHandle<PrefabRef>(true);
      this.m_TradeCostType = system.GetBufferTypeHandle<TradeCost>(true);
      this.m_StorageTransferRequestType = system.GetBufferTypeHandle<StorageTransferRequest>(true);
      this.m_TripNeededType = system.GetBufferTypeHandle<TripNeeded>(true);
      this.m_OwnedVehicleType = system.GetBufferTypeHandle<OwnedVehicle>(true);
      this.m_ResourceType = system.GetBufferTypeHandle<Game.Economy.Resources>(true);
      this.m_InstalledUpgradeType = system.GetBufferTypeHandle<InstalledUpgrade>(true);
      this.m_OutsideConnections = system.GetComponentLookup<Game.Objects.OutsideConnection>(true);
      this.m_ServiceCompanies = system.GetComponentLookup<ServiceCompanyData>(true);
      this.m_ServiceAvailables = system.GetComponentLookup<ServiceAvailable>(true);
      this.m_StorageCompanys = system.GetComponentLookup<Game.Companies.StorageCompany>(true);
      this.m_StorageLimits = system.GetComponentLookup<StorageLimitData>(true);
      this.m_TransportCompanyData = system.GetComponentLookup<TransportCompanyData>(true);
      this.m_PropertyRenters = system.GetComponentLookup<PropertyRenter>(true);
      this.m_Prefabs = system.GetComponentLookup<PrefabRef>(true);
      this.m_IndustrialProcessDatas = system.GetComponentLookup<IndustrialProcessData>(true);
      this.m_ResourceDatas = system.GetComponentLookup<ResourceData>(true);
      this.m_StorageCompanyDatas = system.GetComponentLookup<StorageCompanyData>(true);
      this.m_SpawnableBuildingDatas = system.GetComponentLookup<SpawnableBuildingData>(true);
      this.m_BuildingDatas = system.GetComponentLookup<Game.Prefabs.BuildingData>(true);
      this.m_Resources = system.GetBufferLookup<Game.Economy.Resources>(true);
      this.m_TradeCosts = system.GetBufferLookup<TradeCost>(true);
      this.m_CargoTransportStations = system.GetComponentLookup<Game.Buildings.CargoTransportStation>(true);
    }

    public JobHandle SetupResourceSeller(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_Resources.Update((SystemBase) system);
      this.m_IndustrialProcessDatas.Update((SystemBase) system);
      this.m_CargoTransportStations.Update((SystemBase) system);
      this.m_StorageCompanyDatas.Update((SystemBase) system);
      this.m_PropertyRenters.Update((SystemBase) system);
      this.m_TradeCosts.Update((SystemBase) system);
      this.m_ServiceAvailables.Update((SystemBase) system);
      this.m_OutsideConnections.Update((SystemBase) system);
      this.m_StorageTransferRequestType.Update((SystemBase) system);
      this.m_TripNeededType.Update((SystemBase) system);
      this.m_Prefabs.Update((SystemBase) system);
      JobHandle handle = new ResourcePathfindSetup.SetupResourceSellerJob()
      {
        m_EntityType = this.m_EntityType,
        m_StorageTransferRequestType = this.m_StorageTransferRequestType,
        m_TripNeededType = this.m_TripNeededType,
        m_Resources = this.m_Resources,
        m_IndustrialProcessDatas = this.m_IndustrialProcessDatas,
        m_CargoTransportStations = this.m_CargoTransportStations,
        m_StorageCompanyDatas = this.m_StorageCompanyDatas,
        m_PropertyRenters = this.m_PropertyRenters,
        m_TradeCosts = this.m_TradeCosts,
        m_ServiceAvailables = this.m_ServiceAvailables,
        m_OutsideConnections = this.m_OutsideConnections,
        m_Prefabs = this.m_Prefabs,
        m_SetupData = setupData
      }.ScheduleParallel<ResourcePathfindSetup.SetupResourceSellerJob>(this.m_ResourceSellerQuery, inputDeps);
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(handle);
      return handle;
    }

    public JobHandle SetupResourceExport(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_StorageLimits.Update((SystemBase) system);
      this.m_PrefabType.Update((SystemBase) system);
      this.m_ResourceType.Update((SystemBase) system);
      this.m_OwnedVehicleType.Update((SystemBase) system);
      this.m_TripNeededType.Update((SystemBase) system);
      this.m_TradeCostType.Update((SystemBase) system);
      this.m_InstalledUpgradeType.Update((SystemBase) system);
      this.m_StorageCompanyDatas.Update((SystemBase) system);
      this.m_TransportCompanyData.Update((SystemBase) system);
      this.m_BuildingDatas.Update((SystemBase) system);
      this.m_SpawnableBuildingDatas.Update((SystemBase) system);
      this.m_Prefabs.Update((SystemBase) system);
      this.m_PropertyRenters.Update((SystemBase) system);
      this.m_CargoTransportStations.Update((SystemBase) system);
      return new ResourcePathfindSetup.SetupResourceExportJob()
      {
        m_EntityType = this.m_EntityType,
        m_Limits = this.m_StorageLimits,
        m_PrefabType = this.m_PrefabType,
        m_ResourceType = this.m_ResourceType,
        m_OwnedVehicles = this.m_OwnedVehicleType,
        m_TripNeededType = this.m_TripNeededType,
        m_TradeCosts = this.m_TradeCostType,
        m_InstalledUpgradeType = this.m_InstalledUpgradeType,
        m_StorageCompanyDatas = this.m_StorageCompanyDatas,
        m_TransportCompanyData = this.m_TransportCompanyData,
        m_BuildingDatas = this.m_BuildingDatas,
        m_SpawnableBuildingData = this.m_SpawnableBuildingDatas,
        m_Prefabs = this.m_Prefabs,
        m_Properties = this.m_PropertyRenters,
        m_CargoTransportStations = this.m_CargoTransportStations,
        m_SetupData = setupData
      }.ScheduleParallel<ResourcePathfindSetup.SetupResourceExportJob>(this.m_ExportTargetQuery, inputDeps);
    }

    public JobHandle SetupStorageTransfer(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_StorageCompanyType.Update((SystemBase) system);
      this.m_OutsideConnectionType.Update((SystemBase) system);
      this.m_CargoTransportStations.Update((SystemBase) system);
      this.m_PrefabType.Update((SystemBase) system);
      this.m_ResourceType.Update((SystemBase) system);
      this.m_TradeCostType.Update((SystemBase) system);
      this.m_InstalledUpgradeType.Update((SystemBase) system);
      this.m_StorageTransferRequestType.Update((SystemBase) system);
      this.m_TripNeededType.Update((SystemBase) system);
      this.m_OwnedVehicleType.Update((SystemBase) system);
      this.m_StorageLimits.Update((SystemBase) system);
      this.m_StorageCompanyDatas.Update((SystemBase) system);
      this.m_TransportCompanyData.Update((SystemBase) system);
      return new ResourcePathfindSetup.SetupStorageTransferJob()
      {
        m_EntityType = this.m_EntityType,
        m_StorageType = this.m_StorageCompanyType,
        m_OutsideConnectionType = this.m_OutsideConnectionType,
        m_CargoTransportStations = this.m_CargoTransportStations,
        m_PrefabType = this.m_PrefabType,
        m_ResourceType = this.m_ResourceType,
        m_TradeCostType = this.m_TradeCostType,
        m_InstalledUpgradeType = this.m_InstalledUpgradeType,
        m_StorageTransferRequestType = this.m_StorageTransferRequestType,
        m_TripNeededType = this.m_TripNeededType,
        m_OwnedVehicleType = this.m_OwnedVehicleType,
        m_StorageLimits = this.m_StorageLimits,
        m_StorageCompanyDatas = this.m_StorageCompanyDatas,
        m_TransportCompanyDatas = this.m_TransportCompanyData,
        m_SetupData = setupData
      }.ScheduleParallel<ResourcePathfindSetup.SetupStorageTransferJob>(this.m_StorageQuery, inputDeps);
    }

    [BurstCompile]
    private struct SetupResourceSellerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> m_ServiceAvailables;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      [ReadOnly]
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public BufferLookup<TradeCost> m_TradeCosts;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.CargoTransportStation> m_CargoTransportStations;
      [ReadOnly]
      public BufferTypeHandle<StorageTransferRequest> m_StorageTransferRequestType;
      [ReadOnly]
      public BufferTypeHandle<TripNeeded> m_TripNeededType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        BufferAccessor<StorageTransferRequest> bufferAccessor1 = chunk.GetBufferAccessor<StorageTransferRequest>(ref this.m_StorageTransferRequestType);
        BufferAccessor<TripNeeded> bufferAccessor2 = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripNeededType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out targetSeeker);
          Resource resource = targetSeeker.m_SetupQueueTarget.m_Resource;
          int num1 = targetSeeker.m_SetupQueueTarget.m_Value;
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            Entity entity2 = nativeArray[index2];
            Entity prefab = this.m_Prefabs[entity2].m_Prefab;
            int length = bufferAccessor1.Length > 0 ? bufferAccessor1[index2].Length : 0;
            if (!entity2.Equals(entity1))
            {
              bool flag1 = this.m_OutsideConnections.HasComponent(entity2);
              bool flag2 = this.m_CargoTransportStations.HasComponent(entity2);
              bool flag3 = this.m_StorageCompanyDatas.HasComponent(prefab) && !flag2 && !flag1;
              bool flag4 = this.m_ServiceAvailables.HasComponent(entity2);
              bool flag5 = this.m_IndustrialProcessDatas.HasComponent(prefab) && !flag4 && !flag3;
              bool flag6 = EconomyUtils.IsOfficeResource(resource);
              if (!(flag4 | flag5) || this.m_PropertyRenters.HasComponent(entity2) && !(this.m_PropertyRenters[entity2].m_Property == Entity.Null))
              {
                bool flag7 = false;
                if (flag6 & flag5 && (this.m_IndustrialProcessDatas[prefab].m_Output.m_Resource & resource) != Resource.NoResource)
                  flag7 = true;
                else if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Commercial) > SetupTargetFlags.None & flag4 && (this.m_IndustrialProcessDatas[prefab].m_Output.m_Resource & resource) != Resource.NoResource)
                  flag7 = true;
                else if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Industrial) > SetupTargetFlags.None & flag5 && (this.m_IndustrialProcessDatas[prefab].m_Output.m_Resource & resource) != Resource.NoResource)
                  flag7 = true;
                else if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) != SetupTargetFlags.None && flag1 | flag2 | flag3)
                  flag7 = true;
                if (flag7 && (!flag2 || length < ResourcePathfindSetup.kCargoStationMaxRequestAmount))
                {
                  int resources = EconomyUtils.GetResources(resource, this.m_Resources[entity2]);
                  if (resources > 0)
                  {
                    float num2 = 0.0f;
                    float num3;
                    if (this.m_ServiceAvailables.HasComponent(entity2))
                    {
                      ServiceAvailable serviceAvailable = this.m_ServiceAvailables[entity2];
                      num3 = num2 - (float) (math.min(resources, serviceAvailable.m_ServiceAvailable) * 100);
                    }
                    else
                    {
                      float num4 = math.min(1f, (float) resources * 1f / (float) num1);
                      num3 = num2 + (float) (100.0 * (1.0 - (double) num4));
                      if (flag2)
                      {
                        if (bufferAccessor2.Length <= 0 || bufferAccessor2[index2].Length < ResourcePathfindSetup.kCargoStationMaxTripNeededQueue)
                          num3 = num3 + ResourcePathfindSetup.kCargoStationAmountBasedPenalty * (float) num1 + (float) (ResourcePathfindSetup.kCargoStationPerRequestPenalty * length);
                        else
                          continue;
                      }
                      if (flag1)
                        num3 += ResourcePathfindSetup.kOutsideConnectionAmountBasedPenalty * (float) num1;
                    }
                    if (this.m_TradeCosts.HasBuffer(entity2))
                    {
                      DynamicBuffer<TradeCost> tradeCost1 = this.m_TradeCosts[entity2];
                      TradeCost tradeCost2 = EconomyUtils.GetTradeCost(resource, tradeCost1);
                      num3 += (float) ((double) tradeCost2.m_BuyCost * (double) num1 * 0.0099999997764825821);
                    }
                    targetSeeker.FindTargets(entity2, num3 * 100f);
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
    private struct SetupResourceExportJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Game.Economy.Resources> m_ResourceType;
      [ReadOnly]
      public BufferTypeHandle<TradeCost> m_TradeCosts;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferTypeHandle<TripNeeded> m_TripNeededType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyDatas;
      [ReadOnly]
      public ComponentLookup<TransportCompanyData> m_TransportCompanyData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_Properties;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.CargoTransportStation> m_CargoTransportStations;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        BufferAccessor<Game.Economy.Resources> bufferAccessor1 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourceType);
        BufferAccessor<TradeCost> bufferAccessor2 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCosts);
        BufferAccessor<OwnedVehicle> bufferAccessor3 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicles);
        BufferAccessor<TripNeeded> bufferAccessor4 = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripNeededType);
        BufferAccessor<InstalledUpgrade> bufferAccessor5 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out targetSeeker);
          Resource resource = targetSeeker.m_SetupQueueTarget.m_Resource;
          int num1 = targetSeeker.m_SetupQueueTarget.m_Value;
          if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.RequireTransport) == SetupTargetFlags.None || bufferAccessor3.Length != 0)
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity entity = nativeArray1[index2];
              Entity prefab1 = nativeArray2[index2].m_Prefab;
              bool flag = this.m_CargoTransportStations.HasComponent(entity);
              int num2 = num1;
              float num3 = 0.01f;
              if (this.m_Limits.HasComponent(prefab1))
              {
                StorageLimitData limit = this.m_Limits[prefab1];
                if (bufferAccessor5.Length != 0)
                  UpgradeUtils.CombineStats<StorageLimitData>(ref limit, bufferAccessor5[index2], ref targetSeeker.m_PrefabRef, ref this.m_Limits);
                if (this.m_Properties.HasComponent(entity))
                {
                  Entity property = this.m_Properties[entity].m_Property;
                  if (this.m_Prefabs.HasComponent(property))
                  {
                    Entity prefab2 = this.m_Prefabs[property].m_Prefab;
                    ref StorageLimitData local = ref limit;
                    SpawnableBuildingData spawnable;
                    if (!this.m_SpawnableBuildingData.HasComponent(prefab2))
                      spawnable = new SpawnableBuildingData()
                      {
                        m_Level = (byte) 1
                      };
                    else
                      spawnable = this.m_SpawnableBuildingData[prefab2];
                    Game.Prefabs.BuildingData building;
                    if (!this.m_SpawnableBuildingData.HasComponent(prefab2))
                      building = new Game.Prefabs.BuildingData()
                      {
                        m_LotSize = new int2(1, 1)
                      };
                    else
                      building = this.m_BuildingDatas[prefab2];
                    int adjustedLimit = local.GetAdjustedLimit(spawnable, building);
                    num2 = adjustedLimit - EconomyUtils.GetResources(resource, bufferAccessor1[index2]);
                    num3 = (float) num2 / math.max(1f, (float) adjustedLimit);
                  }
                }
              }
              StorageCompanyData storageCompanyData = this.m_StorageCompanyDatas[prefab1];
              if ((resource & storageCompanyData.m_StoredResources) != Resource.NoResource && num2 >= num1)
              {
                float num4 = 0.0f;
                if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.RequireTransport) != SetupTargetFlags.None)
                {
                  if (this.m_TransportCompanyData.HasComponent(prefab1))
                  {
                    TransportCompanyData transportCompanyData = this.m_TransportCompanyData[prefab1];
                    if (bufferAccessor3[index2].Length >= transportCompanyData.m_MaxTransports)
                      continue;
                  }
                  else
                    continue;
                }
                if (!flag || bufferAccessor4.Length <= 0 || bufferAccessor4[index2].Length < ResourcePathfindSetup.kCargoStationMaxTripNeededQueue)
                {
                  float num5 = (float) ((double) EconomyUtils.GetTradeCost(resource, bufferAccessor2[index2]).m_SellCost * (double) num1 * 0.0099999997764825821) + num4 * (float) ResourcePathfindSetup.kCargoStationVehiclePenalty;
                  targetSeeker.FindTargets(entity, math.max(0.0f, -2000f * num3 + num5));
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
    private struct SetupStorageTransferJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Companies.StorageCompany> m_StorageType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Game.Economy.Resources> m_ResourceType;
      [ReadOnly]
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<StorageTransferRequest> m_StorageTransferRequestType;
      [ReadOnly]
      public BufferTypeHandle<TripNeeded> m_TripNeededType;
      [ReadOnly]
      public BufferTypeHandle<OwnedVehicle> m_OwnedVehicleType;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.CargoTransportStation> m_CargoTransportStations;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyDatas;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimits;
      [ReadOnly]
      public ComponentLookup<TransportCompanyData> m_TransportCompanyDatas;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<Game.Companies.StorageCompany> nativeArray2 = chunk.GetNativeArray<Game.Companies.StorageCompany>(ref this.m_StorageType);
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        BufferAccessor<Game.Economy.Resources> bufferAccessor1 = chunk.GetBufferAccessor<Game.Economy.Resources>(ref this.m_ResourceType);
        BufferAccessor<TradeCost> bufferAccessor2 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
        BufferAccessor<InstalledUpgrade> bufferAccessor3 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        BufferAccessor<StorageTransferRequest> bufferAccessor4 = chunk.GetBufferAccessor<StorageTransferRequest>(ref this.m_StorageTransferRequestType);
        BufferAccessor<OwnedVehicle> bufferAccessor5 = chunk.GetBufferAccessor<OwnedVehicle>(ref this.m_OwnedVehicleType);
        BufferAccessor<TripNeeded> bufferAccessor6 = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripNeededType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity entity1;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out entity1, out targetSeeker);
          Resource resource = targetSeeker.m_SetupQueueTarget.m_Resource;
          int x1 = targetSeeker.m_SetupQueueTarget.m_Value;
          double num1 = (double) targetSeeker.m_SetupQueueTarget.m_Value2;
          long num2 = (long) targetSeeker.m_SetupQueueTarget.m_Value3;
          double num3 = (double) num2;
          long num4 = (long) Mathf.RoundToInt((float) (num1 * num3));
          bool flag1 = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          switch (resource)
          {
            case Resource.UnsortedMail:
            case Resource.OutgoingMail:
              if (x1 < 0 & flag1)
                break;
              goto default;
            case Resource.LocalMail:
              if (!(x1 > 0 & flag1))
                goto default;
              else
                break;
            default:
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                Entity entity2 = nativeArray1[index2];
                bool flag2 = this.m_CargoTransportStations.HasComponent(entity2);
                if (!entity2.Equals(entity1))
                {
                  Game.Companies.StorageCompany storageCompany = nativeArray2[index2];
                  Entity prefab = nativeArray3[index2].m_Prefab;
                  if (this.m_StorageCompanyDatas.HasComponent(prefab))
                  {
                    StorageCompanyData storageCompanyData = this.m_StorageCompanyDatas[prefab];
                    StorageLimitData storageLimit = this.m_StorageLimits[prefab];
                    int maxTransports = this.m_TransportCompanyDatas.HasComponent(prefab) ? this.m_TransportCompanyDatas[prefab].m_MaxTransports : 0;
                    DynamicBuffer<Game.Economy.Resources> resources1 = bufferAccessor1[index2];
                    DynamicBuffer<StorageTransferRequest> dynamicBuffer1 = bufferAccessor4[index2];
                    if (bufferAccessor3.Length != 0)
                      UpgradeUtils.CombineStats<StorageLimitData>(ref storageLimit, bufferAccessor3[index2], ref targetSeeker.m_PrefabRef, ref this.m_StorageLimits);
                    long resources2 = (long) EconomyUtils.GetResources(resource, resources1);
                    long num5 = 0;
                    for (int index3 = 0; index3 < dynamicBuffer1.Length; ++index3)
                    {
                      StorageTransferRequest storageTransferRequest = dynamicBuffer1[index3];
                      if (storageTransferRequest.m_Resource == resource)
                        num5 += (storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) != (StorageTransferFlags) 0 ? (long) storageTransferRequest.m_Amount : (long) -storageTransferRequest.m_Amount;
                    }
                    long y = resources2 + num5;
                    int num6 = math.max(1, EconomyUtils.CountResources(storageCompanyData.m_StoredResources));
                    long num7 = (long) (storageLimit.m_Limit / num6);
                    long x2 = (long) x1;
                    if (flag2)
                      x2 = x1 <= 0 ? -math.min((long) -x1, y) : math.min((long) x1, num7 - y);
                    else if (!flag1)
                    {
                      if (num7 + num2 > 0L)
                      {
                        x2 = (num7 * num4 - num2 * y) / (num7 + num2);
                        if (x1 > 0 && x2 < 0L || x1 < 0 && x2 > 0L)
                          x2 = 0L;
                      }
                      else
                        x2 = 0L;
                    }
                    if (math.abs(x2) >= 4000L)
                    {
                      float num8 = x1 != 0 ? 1000f * (float) math.abs(x2 / (long) x1) : 0.0f;
                      DynamicBuffer<TradeCost> costs = bufferAccessor2[index2];
                      TradeCost tradeCost = EconomyUtils.GetTradeCost(resource, costs);
                      float num9 = 0.01f * (float) x1 * math.max(0.1f, x1 > 0 ? tradeCost.m_SellCost : -tradeCost.m_BuyCost);
                      if (flag2)
                      {
                        num9 += (float) (dynamicBuffer1.Length * ResourcePathfindSetup.kCargoStationPerRequestPenalty);
                        if (bufferAccessor6.Length <= 0 || bufferAccessor6[index2].Length <= ResourcePathfindSetup.kCargoStationMaxTripNeededQueue)
                        {
                          if (bufferAccessor5.Length > 0)
                          {
                            DynamicBuffer<OwnedVehicle> dynamicBuffer2 = bufferAccessor5[index2];
                            if (dynamicBuffer2.Length >= maxTransports)
                            {
                              double num10 = (double) num9;
                              dynamicBuffer2 = bufferAccessor5[index2];
                              double num11 = 1.0 * (double) dynamicBuffer2.Length / (double) maxTransports * (double) ResourcePathfindSetup.kCargoStationVehiclePenalty;
                              num9 = (float) (num10 + num11);
                            }
                          }
                        }
                        else
                          continue;
                      }
                      if ((storageCompanyData.m_StoredResources & resource) != Resource.NoResource && (double) num8 > 0.0)
                        targetSeeker.FindTargets(entity2, num9 - num8);
                    }
                  }
                }
              }
              break;
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
