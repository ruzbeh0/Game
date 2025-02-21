// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportBoardingHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Routes;
using Game.Vehicles;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public static class TransportBoardingHelpers
  {
    public struct BoardingLookupData
    {
      [ReadOnly]
      public ComponentLookup<TransportLine> m_TransportLineData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStopData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public ComponentLookup<CargoTransportVehicleData> m_PrefabCargoTransportVehicleData;
      [ReadOnly]
      public ComponentLookup<Train> m_TrainData;
      [ReadOnly]
      public ComponentLookup<Aircraft> m_AircraftData;
      [ReadOnly]
      public ComponentLookup<Watercraft> m_WatercraftData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTruckData;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public BufferLookup<RouteModifier> m_RouteModifiers;
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      public ComponentLookup<BoardingVehicle> m_BoardingVehicleData;
      public ComponentLookup<VehicleTiming> m_VehicleTimingData;
      public BufferLookup<StorageTransferRequest> m_StorageTransferRequests;
      public BufferLookup<Resources> m_EconomyResources;
      public BufferLookup<LoadingResources> m_LoadingResources;

      public BoardingLookupData(SystemBase system)
      {
        this.m_TransportLineData = system.GetComponentLookup<TransportLine>(true);
        this.m_ConnectedData = system.GetComponentLookup<Connected>(true);
        this.m_TransportStopData = system.GetComponentLookup<Game.Routes.TransportStop>(true);
        this.m_PrefabRefData = system.GetComponentLookup<PrefabRef>(true);
        this.m_PrefabTransportLineData = system.GetComponentLookup<TransportLineData>(true);
        this.m_PrefabCargoTransportVehicleData = system.GetComponentLookup<CargoTransportVehicleData>(true);
        this.m_LayoutElements = system.GetBufferLookup<LayoutElement>(true);
        this.m_RouteModifiers = system.GetBufferLookup<RouteModifier>(true);
        this.m_CargoTransportData = system.GetComponentLookup<Game.Vehicles.CargoTransport>(false);
        this.m_PublicTransportData = system.GetComponentLookup<Game.Vehicles.PublicTransport>(false);
        this.m_BoardingVehicleData = system.GetComponentLookup<BoardingVehicle>(false);
        this.m_VehicleTimingData = system.GetComponentLookup<VehicleTiming>(false);
        this.m_StorageTransferRequests = system.GetBufferLookup<StorageTransferRequest>(false);
        this.m_EconomyResources = system.GetBufferLookup<Resources>(false);
        this.m_LoadingResources = system.GetBufferLookup<LoadingResources>(false);
        this.m_DeliveryTruckData = system.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        this.m_AircraftData = system.GetComponentLookup<Aircraft>(true);
        this.m_WatercraftData = system.GetComponentLookup<Watercraft>(true);
        this.m_TrainData = system.GetComponentLookup<Train>(true);
      }

      public void Update(SystemBase system)
      {
        this.m_TransportLineData.Update(system);
        this.m_ConnectedData.Update(system);
        this.m_TransportStopData.Update(system);
        this.m_PrefabRefData.Update(system);
        this.m_PrefabTransportLineData.Update(system);
        this.m_PrefabCargoTransportVehicleData.Update(system);
        this.m_LayoutElements.Update(system);
        this.m_RouteModifiers.Update(system);
        this.m_CargoTransportData.Update(system);
        this.m_PublicTransportData.Update(system);
        this.m_BoardingVehicleData.Update(system);
        this.m_VehicleTimingData.Update(system);
        this.m_StorageTransferRequests.Update(system);
        this.m_EconomyResources.Update(system);
        this.m_LoadingResources.Update(system);
        this.m_DeliveryTruckData.Update(system);
        this.m_AircraftData.Update(system);
        this.m_WatercraftData.Update(system);
        this.m_TrainData.Update(system);
      }
    }

    public struct BoardingData
    {
      private NativeQueue<TransportBoardingHelpers.BoardingItem> m_BoardingQueue;

      public BoardingData(Allocator allocator)
      {
        this.m_BoardingQueue = new NativeQueue<TransportBoardingHelpers.BoardingItem>((AllocatorManager.AllocatorHandle) allocator);
      }

      public void Dispose() => this.m_BoardingQueue.Dispose();

      public void Dispose(JobHandle inputDeps) => this.m_BoardingQueue.Dispose(inputDeps);

      public JobHandle ScheduleBoarding(
        SystemBase system,
        CityStatisticsSystem statsSystem,
        TransportBoardingHelpers.BoardingLookupData lookupData,
        uint simulationFrameIndex,
        JobHandle inputDeps)
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated method
        JobHandle writer = new TransportBoardingHelpers.TransportBoardingJob()
        {
          m_SimulationFrameIndex = simulationFrameIndex,
          m_BoardingLookupData = lookupData,
          m_BoardingQueue = this.m_BoardingQueue,
          m_StatisticsEventQueue = statsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
        }.Schedule<TransportBoardingHelpers.TransportBoardingJob>(JobHandle.CombineDependencies(inputDeps, deps));
        // ISSUE: reference to a compiler-generated method
        statsSystem.AddWriter(writer);
        return writer;
      }

      public TransportBoardingHelpers.BoardingData.Concurrent ToConcurrent()
      {
        return new TransportBoardingHelpers.BoardingData.Concurrent(this);
      }

      public struct Concurrent
      {
        private NativeQueue<TransportBoardingHelpers.BoardingItem>.ParallelWriter m_BoardingQueue;

        public Concurrent(TransportBoardingHelpers.BoardingData data)
        {
          this.m_BoardingQueue = data.m_BoardingQueue.AsParallelWriter();
        }

        public void BeginBoarding(
          Entity vehicle,
          Entity route,
          Entity stop,
          Entity waypoint,
          Entity currentStation,
          Entity nextStation,
          bool refuel)
        {
          TransportBoardingHelpers.BoardingItem boardingItem;
          boardingItem.m_Begin = true;
          boardingItem.m_Refuel = refuel;
          boardingItem.m_Testing = false;
          boardingItem.m_Vehicle = vehicle;
          boardingItem.m_Route = route;
          boardingItem.m_Stop = stop;
          boardingItem.m_Waypoint = waypoint;
          boardingItem.m_CurrentStation = currentStation;
          boardingItem.m_NextStation = nextStation;
          this.m_BoardingQueue.Enqueue(boardingItem);
        }

        public void EndBoarding(
          Entity vehicle,
          Entity route,
          Entity stop,
          Entity waypoint,
          Entity currentStation,
          Entity nextStation)
        {
          TransportBoardingHelpers.BoardingItem boardingItem;
          boardingItem.m_Begin = false;
          boardingItem.m_Refuel = false;
          boardingItem.m_Testing = false;
          boardingItem.m_Vehicle = vehicle;
          boardingItem.m_Route = route;
          boardingItem.m_Stop = stop;
          boardingItem.m_Waypoint = waypoint;
          boardingItem.m_CurrentStation = currentStation;
          boardingItem.m_NextStation = nextStation;
          this.m_BoardingQueue.Enqueue(boardingItem);
        }

        public void BeginTesting(Entity vehicle, Entity route, Entity stop, Entity waypoint)
        {
          TransportBoardingHelpers.BoardingItem boardingItem;
          boardingItem.m_Begin = true;
          boardingItem.m_Refuel = false;
          boardingItem.m_Testing = true;
          boardingItem.m_Vehicle = vehicle;
          boardingItem.m_Route = route;
          boardingItem.m_Stop = stop;
          boardingItem.m_Waypoint = waypoint;
          boardingItem.m_CurrentStation = Entity.Null;
          boardingItem.m_NextStation = Entity.Null;
          this.m_BoardingQueue.Enqueue(boardingItem);
        }

        public void EndTesting(Entity vehicle, Entity route, Entity stop, Entity waypoint)
        {
          TransportBoardingHelpers.BoardingItem boardingItem;
          boardingItem.m_Begin = false;
          boardingItem.m_Refuel = false;
          boardingItem.m_Testing = true;
          boardingItem.m_Vehicle = vehicle;
          boardingItem.m_Route = route;
          boardingItem.m_Stop = stop;
          boardingItem.m_Waypoint = waypoint;
          boardingItem.m_CurrentStation = Entity.Null;
          boardingItem.m_NextStation = Entity.Null;
          this.m_BoardingQueue.Enqueue(boardingItem);
        }
      }
    }

    private struct BoardingItem
    {
      public bool m_Begin;
      public bool m_Refuel;
      public bool m_Testing;
      public Entity m_Vehicle;
      public Entity m_Route;
      public Entity m_Stop;
      public Entity m_Waypoint;
      public Entity m_CurrentStation;
      public Entity m_NextStation;
    }

    [BurstCompile]
    private struct TransportBoardingJob : IJob
    {
      [ReadOnly]
      public uint m_SimulationFrameIndex;
      public TransportBoardingHelpers.BoardingLookupData m_BoardingLookupData;
      public NativeQueue<TransportBoardingHelpers.BoardingItem> m_BoardingQueue;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      public void Execute()
      {
        TransportBoardingHelpers.BoardingItem data;
        while (this.m_BoardingQueue.TryDequeue(out data))
        {
          if (data.m_Testing)
          {
            if (data.m_Begin)
              this.BeginTesting(data);
            else
              this.EndTesting(data);
          }
          else if (data.m_Begin)
            this.BeginBoarding(data);
          else
            this.EndBoarding(data);
        }
      }

      private void BeginTesting(TransportBoardingHelpers.BoardingItem data)
      {
        BoardingVehicle boardingVehicle = this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop];
        Game.Vehicles.CargoTransport componentData1;
        Game.Vehicles.PublicTransport componentData2;
        if (boardingVehicle.m_Testing != Entity.Null && boardingVehicle.m_Testing != data.m_Vehicle && (this.m_BoardingLookupData.m_CargoTransportData.TryGetComponent(boardingVehicle.m_Testing, out componentData1) && (componentData1.m_State & CargoTransportFlags.Testing) != (CargoTransportFlags) 0 || this.m_BoardingLookupData.m_PublicTransportData.TryGetComponent(boardingVehicle.m_Testing, out componentData2) && (componentData2.m_State & PublicTransportFlags.Testing) != (PublicTransportFlags) 0))
          return;
        boardingVehicle.m_Testing = data.m_Vehicle;
        this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop] = boardingVehicle;
        if (this.m_BoardingLookupData.m_CargoTransportData.HasComponent(data.m_Vehicle))
        {
          Game.Vehicles.CargoTransport cargoTransport = this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle];
          cargoTransport.m_State &= ~CargoTransportFlags.RequireStop;
          cargoTransport.m_State |= CargoTransportFlags.Testing;
          this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle] = cargoTransport;
        }
        if (!this.m_BoardingLookupData.m_PublicTransportData.HasComponent(data.m_Vehicle))
          return;
        Game.Vehicles.PublicTransport publicTransport = this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle];
        publicTransport.m_State &= ~PublicTransportFlags.RequireStop;
        publicTransport.m_State |= PublicTransportFlags.Testing;
        this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle] = publicTransport;
      }

      private void EndTesting(TransportBoardingHelpers.BoardingItem data)
      {
        BoardingVehicle boardingVehicle = this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop];
        if (boardingVehicle.m_Testing == data.m_Vehicle)
        {
          boardingVehicle.m_Testing = Entity.Null;
          this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop] = boardingVehicle;
        }
        if (this.m_BoardingLookupData.m_CargoTransportData.HasComponent(data.m_Vehicle))
        {
          Game.Vehicles.CargoTransport cargoTransport = this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle];
          cargoTransport.m_State &= ~CargoTransportFlags.Testing;
          this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle] = cargoTransport;
        }
        if (!this.m_BoardingLookupData.m_PublicTransportData.HasComponent(data.m_Vehicle))
          return;
        Game.Vehicles.PublicTransport publicTransport = this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle];
        publicTransport.m_State &= ~PublicTransportFlags.Testing;
        this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle] = publicTransport;
      }

      private void BeginBoarding(TransportBoardingHelpers.BoardingItem data)
      {
        BoardingVehicle boardingVehicle = this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop];
        Game.Vehicles.CargoTransport componentData1;
        Game.Vehicles.PublicTransport componentData2;
        if (boardingVehicle.m_Vehicle != Entity.Null && boardingVehicle.m_Vehicle != data.m_Vehicle && (this.m_BoardingLookupData.m_CargoTransportData.TryGetComponent(boardingVehicle.m_Vehicle, out componentData1) && (componentData1.m_State & CargoTransportFlags.Boarding) != (CargoTransportFlags) 0 || this.m_BoardingLookupData.m_PublicTransportData.TryGetComponent(boardingVehicle.m_Vehicle, out componentData2) && (componentData2.m_State & PublicTransportFlags.Boarding) != (PublicTransportFlags) 0))
          return;
        PrefabRef prefabRef = this.m_BoardingLookupData.m_PrefabRefData[data.m_Route];
        TransportLine transportLine = this.m_BoardingLookupData.m_TransportLineData[data.m_Route];
        VehicleTiming vehicleTiming = this.m_BoardingLookupData.m_VehicleTimingData[data.m_Waypoint];
        Connected connected = this.m_BoardingLookupData.m_ConnectedData[data.m_Waypoint];
        DynamicBuffer<RouteModifier> routeModifier = this.m_BoardingLookupData.m_RouteModifiers[data.m_Route];
        Game.Vehicles.CargoTransport cargoTransport = new Game.Vehicles.CargoTransport();
        Game.Vehicles.PublicTransport publicTransport = new Game.Vehicles.PublicTransport();
        uint departureFrame = 0;
        if (this.m_BoardingLookupData.m_CargoTransportData.HasComponent(data.m_Vehicle))
        {
          cargoTransport = this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle];
          departureFrame = cargoTransport.m_DepartureFrame;
        }
        if (this.m_BoardingLookupData.m_PublicTransportData.HasComponent(data.m_Vehicle))
        {
          publicTransport = this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle];
          departureFrame = publicTransport.m_DepartureFrame;
        }
        TransportLineData prefabLineData = this.m_BoardingLookupData.m_PrefabTransportLineData[prefabRef.m_Prefab];
        float targetStopTime = !this.m_BoardingLookupData.m_TransportStopData.HasComponent(connected.m_Connected) ? prefabLineData.m_StopDuration : RouteUtils.GetStopDuration(prefabLineData, this.m_BoardingLookupData.m_TransportStopData[connected.m_Connected]);
        boardingVehicle.m_Vehicle = data.m_Vehicle;
        vehicleTiming.m_AverageTravelTime = RouteUtils.UpdateAverageTravelTime(vehicleTiming.m_AverageTravelTime, departureFrame, this.m_SimulationFrameIndex);
        uint num;
        if ((cargoTransport.m_State & CargoTransportFlags.EnRoute) != (CargoTransportFlags) 0 || (publicTransport.m_State & PublicTransportFlags.EnRoute) != (PublicTransportFlags) 0)
        {
          num = RouteUtils.CalculateDepartureFrame(transportLine, prefabLineData, routeModifier, targetStopTime, vehicleTiming.m_LastDepartureFrame, this.m_SimulationFrameIndex);
          vehicleTiming.m_LastDepartureFrame = num;
        }
        else
          num = this.m_SimulationFrameIndex + 60U;
        this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop] = boardingVehicle;
        this.m_BoardingLookupData.m_VehicleTimingData[data.m_Waypoint] = vehicleTiming;
        if (this.m_BoardingLookupData.m_CargoTransportData.HasComponent(data.m_Vehicle))
        {
          cargoTransport.m_State |= CargoTransportFlags.Boarding;
          if (data.m_Refuel)
            cargoTransport.m_State |= CargoTransportFlags.Refueling;
          cargoTransport.m_DepartureFrame = num;
          this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle] = cargoTransport;
        }
        if (this.m_BoardingLookupData.m_PublicTransportData.HasComponent(data.m_Vehicle))
        {
          publicTransport.m_State |= PublicTransportFlags.Boarding;
          if (data.m_Refuel)
            publicTransport.m_State |= PublicTransportFlags.Refueling;
          publicTransport.m_DepartureFrame = num;
          publicTransport.m_MaxBoardingDistance = 0.0f;
          publicTransport.m_MinWaitingDistance = float.MaxValue;
          this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle] = publicTransport;
        }
        DynamicBuffer<LoadingResources> loadingResources = new DynamicBuffer<LoadingResources>();
        if (this.m_BoardingLookupData.m_LoadingResources.HasBuffer(data.m_Vehicle))
        {
          loadingResources = this.m_BoardingLookupData.m_LoadingResources[data.m_Vehicle];
          loadingResources.Clear();
        }
        this.UnloadResources(data.m_Vehicle, data.m_CurrentStation);
        this.LoadResources(data.m_Vehicle, data.m_CurrentStation, data.m_NextStation, loadingResources);
      }

      private void EndBoarding(TransportBoardingHelpers.BoardingItem data)
      {
        BoardingVehicle boardingVehicle = this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop];
        if (boardingVehicle.m_Vehicle == data.m_Vehicle)
        {
          boardingVehicle.m_Vehicle = Entity.Null;
          this.m_BoardingLookupData.m_BoardingVehicleData[data.m_Stop] = boardingVehicle;
          this.LoadResources(data.m_Vehicle, data.m_CurrentStation, data.m_NextStation, new DynamicBuffer<LoadingResources>());
        }
        if (this.m_BoardingLookupData.m_CargoTransportData.HasComponent(data.m_Vehicle))
        {
          Game.Vehicles.CargoTransport cargoTransport = this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle];
          cargoTransport.m_State &= ~(CargoTransportFlags.Boarding | CargoTransportFlags.Refueling);
          this.m_BoardingLookupData.m_CargoTransportData[data.m_Vehicle] = cargoTransport;
        }
        if (!this.m_BoardingLookupData.m_PublicTransportData.HasComponent(data.m_Vehicle))
          return;
        Game.Vehicles.PublicTransport publicTransport = this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle];
        publicTransport.m_State &= ~(PublicTransportFlags.Boarding | PublicTransportFlags.Refueling);
        this.m_BoardingLookupData.m_PublicTransportData[data.m_Vehicle] = publicTransport;
      }

      private void UnloadResources(Entity vehicle, Entity target)
      {
        if (!this.m_BoardingLookupData.m_EconomyResources.HasBuffer(target))
          return;
        DynamicBuffer<Resources> economyResource = this.m_BoardingLookupData.m_EconomyResources[target];
        if (this.m_BoardingLookupData.m_LayoutElements.HasBuffer(vehicle))
        {
          DynamicBuffer<LayoutElement> layoutElement = this.m_BoardingLookupData.m_LayoutElements[vehicle];
          for (int index = 0; index < layoutElement.Length; ++index)
          {
            Entity vehicle1 = layoutElement[index].m_Vehicle;
            if (this.m_BoardingLookupData.m_EconomyResources.HasBuffer(vehicle1))
              this.UnloadResources(this.m_BoardingLookupData.m_EconomyResources[vehicle1], economyResource);
          }
        }
        else
        {
          if (!this.m_BoardingLookupData.m_EconomyResources.HasBuffer(vehicle))
            return;
          this.UnloadResources(this.m_BoardingLookupData.m_EconomyResources[vehicle], economyResource);
        }
      }

      private void UnloadResources(
        DynamicBuffer<Resources> sourceResources,
        DynamicBuffer<Resources> targetResources)
      {
        for (int index = 0; index < sourceResources.Length; ++index)
        {
          Resources sourceResource = sourceResources[index];
          EconomyUtils.AddResources(sourceResource.m_Resource, sourceResource.m_Amount, targetResources);
        }
        sourceResources.Clear();
      }

      private void LoadResources(
        Entity vehicle,
        Entity source,
        Entity target,
        DynamicBuffer<LoadingResources> loadingResources)
      {
        if (this.m_BoardingLookupData.m_EconomyResources.HasBuffer(source) && this.m_BoardingLookupData.m_StorageTransferRequests.HasBuffer(source))
        {
          DynamicBuffer<Resources> economyResource = this.m_BoardingLookupData.m_EconomyResources[source];
          DynamicBuffer<StorageTransferRequest> storageTransferRequest1 = this.m_BoardingLookupData.m_StorageTransferRequests[source];
          int index1 = 0;
          while (index1 < storageTransferRequest1.Length)
          {
            StorageTransferRequest storageTransferRequest2 = storageTransferRequest1[index1];
            if ((storageTransferRequest2.m_Flags & StorageTransferFlags.Incoming) != (StorageTransferFlags) 0 || (storageTransferRequest2.m_Flags & StorageTransferFlags.Transport) == (StorageTransferFlags) 0 || storageTransferRequest2.m_Target != target)
            {
              ++index1;
            }
            else
            {
              int amount = storageTransferRequest2.m_Amount;
              if (storageTransferRequest2.m_Amount > 0)
              {
                int resources = EconomyUtils.GetResources(storageTransferRequest2.m_Resource, economyResource);
                this.LoadResources(storageTransferRequest2.m_Resource, ref amount, vehicle, resources);
                if (amount < storageTransferRequest2.m_Amount)
                  EconomyUtils.AddResources(storageTransferRequest2.m_Resource, amount - storageTransferRequest2.m_Amount, economyResource);
              }
              if (amount == 0)
              {
                storageTransferRequest1.RemoveAt(index1);
              }
              else
              {
                storageTransferRequest2.m_Amount = amount;
                storageTransferRequest1[index1++] = storageTransferRequest2;
                if (loadingResources.IsCreated)
                {
                  for (int index2 = 0; index2 < loadingResources.Length; ++index2)
                  {
                    LoadingResources loadingResource = loadingResources[index2];
                    if (loadingResource.m_Resource == storageTransferRequest2.m_Resource)
                    {
                      loadingResource.m_Amount += storageTransferRequest2.m_Amount;
                      loadingResources[index2] = loadingResource;
                      storageTransferRequest2.m_Amount = 0;
                      break;
                    }
                  }
                  if (storageTransferRequest2.m_Amount != 0)
                    loadingResources.Add(new LoadingResources()
                    {
                      m_Resource = storageTransferRequest2.m_Resource,
                      m_Amount = storageTransferRequest2.m_Amount
                    });
                }
              }
            }
          }
        }
        if (!loadingResources.IsCreated || loadingResources.Length < 2)
          return;
        loadingResources.AsNativeArray().Sort<LoadingResources, TransportBoardingHelpers.TransportBoardingJob.LoadingResourceComparer>(new TransportBoardingHelpers.TransportBoardingJob.LoadingResourceComparer());
      }

      private void LoadResources(
        Resource resource,
        ref int requestNewAmount,
        Entity vehicle,
        int sourceStoredAmount)
      {
        if (this.m_BoardingLookupData.m_LayoutElements.HasBuffer(vehicle))
        {
          DynamicBuffer<LayoutElement> layoutElement = this.m_BoardingLookupData.m_LayoutElements[vehicle];
          for (int index = 0; index < layoutElement.Length; ++index)
          {
            Entity vehicle1 = layoutElement[index].m_Vehicle;
            if (this.m_BoardingLookupData.m_EconomyResources.HasBuffer(vehicle1))
            {
              CargoTransportVehicleData vehicleData = this.m_BoardingLookupData.m_PrefabCargoTransportVehicleData[this.m_BoardingLookupData.m_PrefabRefData[vehicle1].m_Prefab];
              DynamicBuffer<Resources> economyResource = this.m_BoardingLookupData.m_EconomyResources[vehicle1];
              TransportType vehicleType = this.GetVehicleType(vehicle1);
              this.LoadResources(resource, ref requestNewAmount, vehicleData, economyResource, vehicleType, ref sourceStoredAmount);
              if (requestNewAmount == 0 || sourceStoredAmount <= 0)
                break;
            }
          }
        }
        else
        {
          if (!this.m_BoardingLookupData.m_EconomyResources.HasBuffer(vehicle))
            return;
          CargoTransportVehicleData vehicleData = this.m_BoardingLookupData.m_PrefabCargoTransportVehicleData[this.m_BoardingLookupData.m_PrefabRefData[vehicle].m_Prefab];
          DynamicBuffer<Resources> economyResource = this.m_BoardingLookupData.m_EconomyResources[vehicle];
          TransportType vehicleType = this.GetVehicleType(vehicle);
          this.LoadResources(resource, ref requestNewAmount, vehicleData, economyResource, vehicleType, ref sourceStoredAmount);
        }
      }

      private void LoadResources(
        Resource resource,
        ref int requestNewAmount,
        CargoTransportVehicleData vehicleData,
        DynamicBuffer<Resources> targetResources,
        TransportType transportType,
        ref int sourceStoredAmount)
      {
        int cargoCapacity = vehicleData.m_CargoCapacity;
        int num1 = -1;
        for (int index = 0; index < targetResources.Length; ++index)
        {
          Resources targetResource = targetResources[index];
          cargoCapacity -= targetResource.m_Amount;
          num1 = math.select(num1, index, targetResource.m_Resource == resource);
        }
        int num2 = math.min(cargoCapacity, math.min(requestNewAmount, math.max(sourceStoredAmount, 0)));
        if (num2 == 0)
          return;
        if (num1 >= 0)
        {
          Resources targetResource = targetResources[num1];
          targetResource.m_Amount += num2;
          targetResources[num1] = targetResource;
        }
        else
        {
          if (targetResources.Length >= vehicleData.m_MaxResourceCount || (vehicleData.m_Resources & resource) == Resource.NoResource)
            return;
          targetResources.Add(new Resources()
          {
            m_Resource = resource,
            m_Amount = num2
          });
        }
        requestNewAmount -= num2;
        sourceStoredAmount -= num2;
        switch (transportType)
        {
          case TransportType.Train:
            this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
            {
              m_Statistic = StatisticType.CargoCountTrain,
              m_Change = (float) num2
            });
            break;
          case TransportType.Ship:
            this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
            {
              m_Statistic = StatisticType.CargoCountShip,
              m_Change = (float) num2
            });
            break;
          case TransportType.Airplane:
            this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
            {
              m_Statistic = StatisticType.CargoCountAirplane,
              m_Change = (float) num2
            });
            break;
        }
      }

      private TransportType GetVehicleType(Entity vehicle)
      {
        if (this.m_BoardingLookupData.m_AircraftData.HasComponent(vehicle))
          return TransportType.Airplane;
        if (this.m_BoardingLookupData.m_TrainData.HasComponent(vehicle))
          return TransportType.Train;
        if (this.m_BoardingLookupData.m_WatercraftData.HasComponent(vehicle))
          return TransportType.Ship;
        return this.m_BoardingLookupData.m_DeliveryTruckData.HasComponent(vehicle) ? TransportType.Bus : TransportType.None;
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct LoadingResourceComparer : IComparer<LoadingResources>
      {
        public int Compare(LoadingResources x, LoadingResources y) => x.m_Amount - y.m_Amount;
      }
    }
  }
}
