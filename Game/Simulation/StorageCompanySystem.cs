// Decompiled with JetBrains decompiler
// Type: Game.Simulation.StorageCompanySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Routes;
using Game.Serialization;
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
  public class StorageCompanySystem : GameSystemBase, IPostDeserialize
  {
    private static readonly int kTransferCooldown = 400;
    private static readonly int kCostFadeProbability = 256;
    private static readonly float kMaxTransportUnitCost = 0.03f;
    private static readonly int kStorageLowStockAmount = 25000;
    private static readonly int kStationLowStockAmount = 100000;
    private static readonly int kStorageExportStartAmount = 100000;
    private static readonly int kStationExportStartAmount = 200000;
    private static readonly int kStorageMinimalTransferAmount = 10000;
    private static readonly int kStationMinimalTransferAmount = 30000;
    private SimulationSystem m_SimulationSystem;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private EntityQuery m_CompanyGroup;
    private EntityQuery m_StationGroup;
    private EndFrameBarrier m_EndFrameBarrier;
    private StorageCompanySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadWrite<Resources>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<CompanyData>(), ComponentType.Exclude<StorageTransfer>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_StationGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.StorageCompany>(), ComponentType.ReadOnly<CityServiceUpkeep>(), ComponentType.ReadWrite<Resources>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<CompanyData>(), ComponentType.Exclude<StorageTransfer>(), ComponentType.Exclude<Game.Prefabs.OutsideConnection>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_CompanyGroup, this.m_StationGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint frameWithInterval = SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageTransferRequest_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = new StorageCompanySystem.StorageJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CompanyResourceType = this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_TradeCostType = this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_Limits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_StorageCompanyDatas = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup,
        m_BuildingDatas = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SpawnableBuildingDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_StorageCompanies = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_StorageTransferRequests = this.__TypeHandle.__Game_Companies_StorageTransferRequest_RW_BufferLookup,
        m_Targets = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_Trucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdateFrameIndex = frameWithInterval,
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next()
      }.Schedule<StorageCompanySystem.StorageJob>(this.m_CompanyGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageTransferRequest_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new StorageCompanySystem.StationStorageJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CompanyResourceType = this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_TradeCostType = this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferTypeHandle,
        m_Limits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_StorageCompanyDatas = this.__TypeHandle.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
        m_StorageCompanies = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_StorageTransferRequests = this.__TypeHandle.__Game_Companies_StorageTransferRequest_RW_BufferLookup,
        m_Targets = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_Trucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
        m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
        m_ConnectedRouteBuffers = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup,
        m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ResourceBuffers = this.__TypeHandle.__Game_Economy_Resources_RO_BufferLookup,
        m_RouteVehicles = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
        m_SubObjectBuffers = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_TransportLineData = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_Connecteds = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_UpdateInterval = this.GetUpdateInterval(SystemUpdatePhase.GameSimulation)
      }.Schedule<StorageCompanySystem.StationStorageJob>(this.m_StationGroup, JobHandle.CombineDependencies(jobHandle1, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
    }

    private static bool RemoveFromRequests(
      Resource resource,
      int amount,
      Entity owner,
      Entity target1,
      Entity target2,
      ref BufferLookup<StorageTransferRequest> storageTransferRequests)
    {
      DynamicBuffer<StorageTransferRequest> dynamicBuffer = storageTransferRequests[owner];
      for (int index = 0; index < dynamicBuffer.Length; ++index)
      {
        StorageTransferRequest storageTransferRequest = dynamicBuffer[index];
        if ((storageTransferRequest.m_Target == target1 || storageTransferRequest.m_Target == target2) && storageTransferRequest.m_Resource == resource && (storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) == (StorageTransferFlags) 0)
        {
          if (storageTransferRequest.m_Amount > amount)
          {
            storageTransferRequest.m_Amount -= amount;
            dynamicBuffer[index] = storageTransferRequest;
            return true;
          }
          amount -= storageTransferRequest.m_Amount;
          dynamicBuffer.RemoveAtSwapBack(index);
          --index;
        }
      }
      return amount == 0;
    }

    private static bool ProcessStorage(
      int chunkIndex,
      Entity company,
      Entity building,
      Resource resource,
      StorageCompanyData storageCompanyData,
      DynamicBuffer<Resources> resourceBuffer,
      DynamicBuffer<StorageTransferRequest> requests,
      StorageLimitData limitData,
      SpawnableBuildingData spawnableData,
      Game.Prefabs.BuildingData buildingData,
      DeliveryTruckSelectData truckSelectData,
      uint simulationFrame,
      DynamicBuffer<TradeCost> tradeCosts,
      EntityCommandBuffer.ParallelWriter commandBuffer,
      bool station,
      bool hasConnectedRoute,
      int incomingAmount,
      ref Random random,
      ref ComponentLookup<Game.Companies.StorageCompany> storageCompanies,
      ref BufferLookup<OwnedVehicle> ownedVehicles,
      ref BufferLookup<StorageTransferRequest> storageTransferRequests,
      ref ComponentLookup<Game.Vehicles.DeliveryTruck> trucks,
      ref ComponentLookup<Target> targets,
      ref BufferLookup<LayoutElement> layoutElements,
      ref ComponentLookup<PropertyRenter> propertyRenters,
      ref ComponentLookup<Game.Objects.OutsideConnection> outsideConnections)
    {
      bool flag1 = false;
      int num1 = EconomyUtils.CountResources(storageCompanyData.m_StoredResources);
      int num2 = limitData.GetAdjustedLimit(spawnableData, buildingData) / num1;
      if ((storageCompanyData.m_StoredResources & resource) != Resource.NoResource)
      {
        int resources = EconomyUtils.GetResources(resource, resourceBuffer);
        int num3 = resources;
        for (int index1 = 0; index1 < requests.Length; ++index1)
        {
          StorageTransferRequest request = requests[index1];
          if (request.m_Resource == resource)
          {
            if (!storageCompanies.HasComponent(request.m_Target) || !storageTransferRequests.HasBuffer(request.m_Target) || !propertyRenters.HasComponent(request.m_Target) && !outsideConnections.HasComponent(request.m_Target))
            {
              requests.RemoveAtSwapBack(index1);
              --index1;
            }
            else
            {
              bool flag2 = (request.m_Flags & StorageTransferFlags.Incoming) != 0;
              if (flag2)
              {
                int num4 = 0;
                DynamicBuffer<StorageTransferRequest> dynamicBuffer1 = storageTransferRequests[request.m_Target];
                for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
                {
                  StorageTransferRequest storageTransferRequest = dynamicBuffer1[index2];
                  if ((storageTransferRequest.m_Target == company || storageTransferRequest.m_Target == building) && storageTransferRequest.m_Resource == resource && (storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) == (StorageTransferFlags) 0)
                    num4 += storageTransferRequest.m_Amount;
                }
                int num5 = 0;
                if (ownedVehicles.HasBuffer(request.m_Target))
                {
                  DynamicBuffer<OwnedVehicle> dynamicBuffer2 = ownedVehicles[request.m_Target];
                  for (int index3 = 0; index3 < dynamicBuffer2.Length; ++index3)
                  {
                    Entity vehicle1 = dynamicBuffer2[index3].m_Vehicle;
                    if (trucks.HasComponent(vehicle1) && targets.HasComponent(vehicle1))
                    {
                      Game.Vehicles.DeliveryTruck deliveryTruck1 = trucks[vehicle1];
                      Entity target = targets[vehicle1].m_Target;
                      if (target == company || target == building)
                      {
                        int num6 = 0;
                        if (deliveryTruck1.m_Resource == resource)
                          num6 += deliveryTruck1.m_Amount;
                        if (layoutElements.HasBuffer(vehicle1))
                        {
                          DynamicBuffer<LayoutElement> dynamicBuffer3 = layoutElements[vehicle1];
                          for (int index4 = 0; index4 < dynamicBuffer3.Length; ++index4)
                          {
                            Entity vehicle2 = dynamicBuffer3[index4].m_Vehicle;
                            if (trucks.HasComponent(vehicle2))
                            {
                              Game.Vehicles.DeliveryTruck deliveryTruck2 = trucks[vehicle2];
                              if (deliveryTruck2.m_Resource == resource)
                                num6 += deliveryTruck2.m_Amount;
                            }
                          }
                        }
                        num5 += num6;
                      }
                    }
                  }
                }
                if (station && num4 + num5 < request.m_Amount && incomingAmount > 0)
                {
                  int num7 = math.min(request.m_Amount - num5 - num4, incomingAmount);
                  num5 += num7;
                  incomingAmount -= num7;
                }
                if (num5 + num4 == 0)
                {
                  requests.RemoveAtSwapBack(index1);
                  --index1;
                  continue;
                }
                if (num5 + num4 < request.m_Amount)
                {
                  request.m_Amount = num5 + num4;
                  requests[index1] = request;
                }
              }
              else
              {
                int num8 = 0;
                DynamicBuffer<StorageTransferRequest> dynamicBuffer = storageTransferRequests[request.m_Target];
                for (int index5 = 0; index5 < dynamicBuffer.Length; ++index5)
                {
                  StorageTransferRequest storageTransferRequest = dynamicBuffer[index5];
                  if ((storageTransferRequest.m_Target == company || storageTransferRequest.m_Target == building) && storageTransferRequest.m_Resource == resource && (storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) != (StorageTransferFlags) 0)
                  {
                    num8 = storageTransferRequest.m_Amount;
                    break;
                  }
                }
                if (num8 == 0)
                {
                  requests.RemoveAtSwapBack(index1);
                  --index1;
                  continue;
                }
                if (num8 < request.m_Amount)
                {
                  request.m_Amount = num8;
                  requests[index1] = request;
                }
              }
              num3 += flag2 ? request.m_Amount : -request.m_Amount;
            }
          }
        }
        int num9 = num2 - resources;
        TradeCost tradeCost = EconomyUtils.GetTradeCost(resource, tradeCosts);
        long tradeRequestTime = EconomyUtils.GetLastTradeRequestTime(tradeCosts);
        if (station && tradeCost.m_LastTransferRequestTime == 0L)
        {
          // ISSUE: reference to a compiler-generated field
          tradeCost.m_LastTransferRequestTime = (long) simulationFrame - (long) (StorageCompanySystem.kTransferCooldown / 2);
          EconomyUtils.SetTradeCost(resource, tradeCost, tradeCosts, false);
        }
        // ISSUE: reference to a compiler-generated field
        if ((long) simulationFrame - tradeRequestTime >= (long) (StorageCompanySystem.kTransferCooldown + random.NextInt(storageCompanyData.m_TransportInterval.x, storageCompanyData.m_TransportInterval.y)) || tradeCost.m_LastTransferRequestTime == 0L)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int y1 = station ? StorageCompanySystem.kStationExportStartAmount : StorageCompanySystem.kStorageExportStartAmount;
          int num10 = (int) math.min((float) num2 * 0.8f, (float) y1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int y2 = station ? StorageCompanySystem.kStationLowStockAmount : StorageCompanySystem.kStorageLowStockAmount;
          int num11 = (int) math.min((float) num2 * 0.5f, (float) y2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int y3 = station ? StorageCompanySystem.kStationMinimalTransferAmount : StorageCompanySystem.kStorageMinimalTransferAmount;
          int y4 = (int) math.min((float) num2 * 0.5f, (float) y3);
          int num12 = (num10 - num11) / 2 + num11;
          if (resources > num10 && num3 > num10)
          {
            int num13 = 0;
            int num14 = resources - num11;
            if (!station)
            {
              DeliveryTruckSelectItem deliveryTruckSelectItem;
              truckSelectData.TrySelectItem(ref random, resource, num14, out deliveryTruckSelectItem);
              if (deliveryTruckSelectItem.m_Capacity > 0)
                num14 = deliveryTruckSelectItem.m_Capacity * math.max(num14 / deliveryTruckSelectItem.m_Capacity, 1);
              num13 = deliveryTruckSelectItem.m_Cost;
            }
            // ISSUE: reference to a compiler-generated field
            if (station || (double) num13 / (double) math.min(resources, num14) < (double) StorageCompanySystem.kMaxTransportUnitCost)
            {
              commandBuffer.AddComponent<StorageTransfer>(chunkIndex, company, new StorageTransfer()
              {
                m_Resource = resource,
                m_Amount = num14
              });
              tradeCost.m_LastTransferRequestTime = (long) simulationFrame;
              EconomyUtils.SetTradeCost(resource, tradeCost, tradeCosts, false);
              flag1 = true;
            }
          }
          else if (resources < num11 && num3 < num11)
          {
            if (station && !hasConnectedRoute)
              return false;
            StorageTransfer component = new StorageTransfer();
            component.m_Resource = resource;
            int capacity = math.min((int) ((double) num9 * 0.89999997615814209), math.max(num12 - resources, y4));
            component.m_Amount = -capacity;
            if (!station)
            {
              DeliveryTruckSelectItem deliveryTruckSelectItem;
              truckSelectData.TrySelectItem(ref random, resource, capacity, out deliveryTruckSelectItem);
              if (deliveryTruckSelectItem.m_Capacity > 0)
                component.m_Amount = -math.max(capacity / deliveryTruckSelectItem.m_Capacity, 1) * deliveryTruckSelectItem.m_Capacity;
            }
            commandBuffer.AddComponent<StorageTransfer>(chunkIndex, company, component);
            tradeCost.m_LastTransferRequestTime = (long) simulationFrame;
            flag1 = true;
          }
          // ISSUE: reference to a compiler-generated field
          if (random.NextInt(StorageCompanySystem.kCostFadeProbability) == 0)
          {
            tradeCost.m_BuyCost *= 0.99f;
            tradeCost.m_SellCost *= 0.99f;
            if (!flag1)
              EconomyUtils.SetTradeCost(resource, tradeCost, tradeCosts, false);
          }
          if (flag1)
            EconomyUtils.SetTradeCost(resource, tradeCost, tradeCosts, false);
        }
      }
      return flag1;
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Version.storageConditionReset))
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_CompanyGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      foreach (Entity entity in entityArray1)
        this.EntityManager.GetBuffer<TradeCost>(entity).Clear();
      entityArray1.Dispose();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray2 = this.m_StationGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      foreach (Entity entity in entityArray2)
        this.EntityManager.GetBuffer<TradeCost>(entity).Clear();
      entityArray2.Dispose();
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
    public StorageCompanySystem()
    {
    }

    [BurstCompile]
    private struct StorageJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_CompanyResourceType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDatas;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_BuildingDatas;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanies;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      public BufferLookup<StorageTransferRequest> m_StorageTransferRequests;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_Trucks;
      [ReadOnly]
      public ComponentLookup<Target> m_Targets;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;
      public uint m_SimulationFrame;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor1 = chunk.GetBufferAccessor<Resources>(ref this.m_CompanyResourceType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TradeCost> bufferAccessor2 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Prefabs.HasComponent(nativeArray3[index].m_Property))
          {
            DynamicBuffer<Resources> resourceBuffer = bufferAccessor1[index];
            DynamicBuffer<TradeCost> tradeCosts = bufferAccessor2[index];
            Entity prefab1 = nativeArray2[index].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            StorageLimitData limit = this.m_Limits[prefab1];
            // ISSUE: reference to a compiler-generated field
            StorageCompanyData storageCompanyData = this.m_StorageCompanyDatas[prefab1];
            // ISSUE: reference to a compiler-generated field
            Entity prefab2 = this.m_Prefabs[nativeArray3[index].m_Property].m_Prefab;
            SpawnableBuildingData spawnableBuildingData;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_SpawnableBuildingDatas.HasComponent(prefab2))
            {
              spawnableBuildingData = new SpawnableBuildingData()
              {
                m_Level = (byte) 1
              };
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              spawnableBuildingData = this.m_SpawnableBuildingDatas[prefab2];
            }
            SpawnableBuildingData spawnableData = spawnableBuildingData;
            Game.Prefabs.BuildingData buildingData1;
            // ISSUE: reference to a compiler-generated field
            if (!this.m_BuildingDatas.HasComponent(prefab2))
            {
              buildingData1 = new Game.Prefabs.BuildingData()
              {
                m_LotSize = new int2(1, 1)
              };
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              buildingData1 = this.m_BuildingDatas[prefab2];
            }
            Game.Prefabs.BuildingData buildingData2 = buildingData1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
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
            StorageCompanySystem.ProcessStorage(unfilteredChunkIndex, entity, nativeArray3[index].m_Property, storageCompanyData.m_StoredResources, storageCompanyData, resourceBuffer, this.m_StorageTransferRequests[entity], limit, spawnableData, buildingData2, this.m_DeliveryTruckSelectData, this.m_SimulationFrame, tradeCosts, this.m_CommandBuffer, false, false, 0, ref random, ref this.m_StorageCompanies, ref this.m_OwnedVehicles, ref this.m_StorageTransferRequests, ref this.m_Trucks, ref this.m_Targets, ref this.m_LayoutElements, ref this.m_PropertyRenters, ref this.m_OutsideConnections);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity);
          }
        }
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
    private struct StationStorageJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_CompanyResourceType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanies;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      public BufferLookup<StorageTransferRequest> m_StorageTransferRequests;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_Trucks;
      [ReadOnly]
      public ComponentLookup<Target> m_Targets;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjectBuffers;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineData;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> m_ConnectedRouteBuffers;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicles;
      [ReadOnly]
      public BufferLookup<Resources> m_ResourceBuffers;
      [ReadOnly]
      public ComponentLookup<Connected> m_Connecteds;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;
      public uint m_SimulationFrame;
      public RandomSeed m_RandomSeed;
      public int m_UpdateInterval;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor1 = chunk.GetBufferAccessor<Resources>(ref this.m_CompanyResourceType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TradeCost> bufferAccessor2 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor3 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Resource resource = EconomyUtils.GetResource((int) ((long) this.m_SimulationFrame / (long) this.m_UpdateInterval) % EconomyUtils.ResourceCount);
        for (int index = 0; index < chunk.Count; ++index)
        {
          int incomingAmount = 0;
          Entity entity = nativeArray1[index];
          DynamicBuffer<Resources> resourceBuffer = bufferAccessor1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          StorageLimitData limit = this.m_Limits[prefab];
          // ISSUE: reference to a compiler-generated field
          StorageCompanyData storageCompanyData = this.m_StorageCompanyDatas[prefab];
          DynamicBuffer<TradeCost> tradeCosts = bufferAccessor2[index];
          if (bufferAccessor3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<StorageLimitData>(ref limit, bufferAccessor3[index], ref this.m_PrefabRefData, ref this.m_Limits);
          }
          bool hasConnectedRoute = false;
          // ISSUE: reference to a compiler-generated method
          this.CheckConnectedRoute(entity, resource, ref hasConnectedRoute, ref incomingAmount);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
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
          StorageCompanySystem.ProcessStorage(unfilteredChunkIndex, entity, entity, resource, storageCompanyData, resourceBuffer, this.m_StorageTransferRequests[entity], limit, new SpawnableBuildingData()
          {
            m_Level = (byte) 1
          }, new Game.Prefabs.BuildingData()
          {
            m_LotSize = new int2(1, 1)
          }, this.m_DeliveryTruckSelectData, this.m_SimulationFrame, tradeCosts, this.m_CommandBuffer, true, (hasConnectedRoute ? 1 : 0) != 0, incomingAmount, ref random, ref this.m_StorageCompanies, ref this.m_OwnedVehicles, ref this.m_StorageTransferRequests, ref this.m_Trucks, ref this.m_Targets, ref this.m_LayoutElements, ref this.m_PropertyRenters, ref this.m_OutsideConnections);
        }
      }

      private void CheckConnectedRoute(
        Entity entity,
        Resource resource,
        ref bool hasConnectedRoute,
        ref int incomingAmount)
      {
        DynamicBuffer<ConnectedRoute> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedRouteBuffers.TryGetBuffer(entity, out bufferData1))
        {
          for (int index1 = 0; index1 < bufferData1.Length; ++index1)
          {
            Entity waypoint = bufferData1[index1].m_Waypoint;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Owners.HasComponent(waypoint))
            {
              // ISSUE: reference to a compiler-generated field
              Entity owner = this.m_Owners[waypoint].m_Owner;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_RouteVehicles.HasBuffer(owner) && this.m_PrefabRefData.HasComponent(owner) && this.m_TransportLineData.HasComponent(this.m_PrefabRefData[owner].m_Prefab) && this.m_TransportLineData[this.m_PrefabRefData[owner].m_Prefab].m_CargoTransport)
              {
                hasConnectedRoute = true;
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<RouteVehicle> routeVehicle = this.m_RouteVehicles[owner];
                for (int index2 = 0; index2 < routeVehicle.Length; ++index2)
                {
                  Entity vehicle1 = routeVehicle[index2].m_Vehicle;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Targets.HasComponent(vehicle1) && this.m_ResourceBuffers.HasBuffer(vehicle1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Entity entity1 = this.m_Targets[vehicle1].m_Target;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Connecteds.HasComponent(entity1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      entity1 = this.m_Connecteds[entity1].m_Connected;
                    }
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Owners.HasComponent(entity1))
                    {
                      // ISSUE: reference to a compiler-generated field
                      entity1 = this.m_Owners[entity1].m_Owner;
                    }
                    if (entity1 == entity)
                    {
                      // ISSUE: reference to a compiler-generated field
                      DynamicBuffer<Resources> resourceBuffer1 = this.m_ResourceBuffers[vehicle1];
                      incomingAmount += EconomyUtils.GetResources(resource, resourceBuffer1);
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_LayoutElements.HasBuffer(vehicle1))
                      {
                        // ISSUE: reference to a compiler-generated field
                        DynamicBuffer<LayoutElement> layoutElement = this.m_LayoutElements[vehicle1];
                        for (int index3 = 0; index3 < layoutElement.Length; ++index3)
                        {
                          Entity vehicle2 = layoutElement[index3].m_Vehicle;
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_ResourceBuffers.HasBuffer(vehicle2))
                          {
                            // ISSUE: reference to a compiler-generated field
                            DynamicBuffer<Resources> resourceBuffer2 = this.m_ResourceBuffers[vehicle2];
                            incomingAmount += EconomyUtils.GetResources(resource, resourceBuffer2);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjectBuffers.TryGetBuffer(entity, out bufferData2))
          return;
        for (int index = 0; index < bufferData2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckConnectedRoute(bufferData2[index].m_SubObject, resource, ref hasConnectedRoute, ref incomingAmount);
        }
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
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public BufferTypeHandle<TradeCost> __Game_Companies_TradeCost_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> __Game_Prefabs_StorageCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      public BufferLookup<StorageTransferRequest> __Game_Companies_StorageTransferRequest_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Resources> __Game_Economy_Resources_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferTypeHandle = state.GetBufferTypeHandle<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RW_BufferTypeHandle = state.GetBufferTypeHandle<TradeCost>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageCompanyData_RO_ComponentLookup = state.GetComponentLookup<StorageCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageTransferRequest_RW_BufferLookup = state.GetBufferLookup<StorageTransferRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferLookup = state.GetBufferLookup<ConnectedRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferLookup = state.GetBufferLookup<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RO_BufferLookup = state.GetBufferLookup<RouteVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
      }
    }
  }
}
