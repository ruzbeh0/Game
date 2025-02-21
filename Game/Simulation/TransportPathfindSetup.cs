// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportPathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
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
  public struct TransportPathfindSetup
  {
    private EntityQuery m_TransportVehicleQuery;
    private EntityQuery m_TaxiQuery;
    private EntityQuery m_TransportVehicleRequestQuery;
    private EntityQuery m_TaxiRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<TransportVehicleRequest> m_TransportVehicleRequestType;
    private ComponentTypeHandle<TaxiRequest> m_TaxiRequestType;
    private ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
    private ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
    private ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
    private ComponentTypeHandle<Game.Vehicles.Taxi> m_TaxiType;
    private ComponentTypeHandle<Controller> m_ControllerType;
    private ComponentTypeHandle<Game.Routes.Color> m_RouteColorType;
    private ComponentTypeHandle<PrefabRef> m_PrefabRefType;
    private BufferTypeHandle<PathElement> m_PathElementType;
    private BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
    private BufferTypeHandle<LayoutElement> m_LayoutElementType;
    private ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
    private ComponentLookup<TaxiRequest> m_TaxiRequestData;
    private ComponentLookup<VehicleModel> m_VehicleModelData;
    private ComponentLookup<Game.Routes.Color> m_RouteColorData;
    private ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
    private ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
    private ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
    private ComponentLookup<TransportLineData> m_TransportLineData;
    private ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;
    private BufferLookup<RouteWaypoint> m_Waypoints;

    public TransportPathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_TransportVehicleQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.TransportDepot>(),
          ComponentType.ReadOnly<Game.Vehicles.CargoTransport>(),
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
      this.m_TaxiQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.TransportDepot>(),
          ComponentType.ReadOnly<Game.Vehicles.Taxi>()
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
      this.m_TransportVehicleRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<TransportVehicleRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      // ISSUE: reference to a compiler-generated method
      this.m_TaxiRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<TaxiRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_OutsideConnectionType = system.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_TransportVehicleRequestType = system.GetComponentTypeHandle<TransportVehicleRequest>(true);
      this.m_TaxiRequestType = system.GetComponentTypeHandle<TaxiRequest>(true);
      this.m_TransportDepotType = system.GetComponentTypeHandle<Game.Buildings.TransportDepot>(true);
      this.m_CargoTransportType = system.GetComponentTypeHandle<Game.Vehicles.CargoTransport>(true);
      this.m_PublicTransportType = system.GetComponentTypeHandle<Game.Vehicles.PublicTransport>(true);
      this.m_TaxiType = system.GetComponentTypeHandle<Game.Vehicles.Taxi>(true);
      this.m_ControllerType = system.GetComponentTypeHandle<Controller>(true);
      this.m_RouteColorType = system.GetComponentTypeHandle<Game.Routes.Color>(true);
      this.m_PrefabRefType = system.GetComponentTypeHandle<PrefabRef>(true);
      this.m_PathElementType = system.GetBufferTypeHandle<PathElement>(true);
      this.m_ServiceDispatchType = system.GetBufferTypeHandle<ServiceDispatch>(true);
      this.m_LayoutElementType = system.GetBufferTypeHandle<LayoutElement>(true);
      this.m_TransportVehicleRequestData = system.GetComponentLookup<TransportVehicleRequest>(true);
      this.m_TaxiRequestData = system.GetComponentLookup<TaxiRequest>(true);
      this.m_VehicleModelData = system.GetComponentLookup<VehicleModel>(true);
      this.m_RouteColorData = system.GetComponentLookup<Game.Routes.Color>(true);
      this.m_TransportDepotData = system.GetComponentLookup<Game.Buildings.TransportDepot>(true);
      this.m_CargoTransportData = system.GetComponentLookup<Game.Vehicles.CargoTransport>(true);
      this.m_PublicTransportData = system.GetComponentLookup<Game.Vehicles.PublicTransport>(true);
      this.m_TransportLineData = system.GetComponentLookup<TransportLineData>(true);
      this.m_PrefabTransportDepotData = system.GetComponentLookup<TransportDepotData>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
      this.m_Waypoints = system.GetBufferLookup<RouteWaypoint>(true);
    }

    public JobHandle SetupTransportVehicle(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_TransportDepotType.Update((SystemBase) system);
      this.m_CargoTransportType.Update((SystemBase) system);
      this.m_PublicTransportType.Update((SystemBase) system);
      this.m_ControllerType.Update((SystemBase) system);
      this.m_RouteColorType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_LayoutElementType.Update((SystemBase) system);
      this.m_TransportVehicleRequestData.Update((SystemBase) system);
      this.m_VehicleModelData.Update((SystemBase) system);
      this.m_RouteColorData.Update((SystemBase) system);
      this.m_PrefabTransportDepotData.Update((SystemBase) system);
      this.m_TransportLineData.Update((SystemBase) system);
      return new TransportPathfindSetup.SetupTransportVehiclesJob()
      {
        m_EntityType = this.m_EntityType,
        m_TransportDepotType = this.m_TransportDepotType,
        m_CargoTransportType = this.m_CargoTransportType,
        m_PublicTransportType = this.m_PublicTransportType,
        m_ControllerType = this.m_ControllerType,
        m_RouteColorType = this.m_RouteColorType,
        m_OwnerType = this.m_OwnerType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_LayoutElementType = this.m_LayoutElementType,
        m_TransportVehicleRequestData = this.m_TransportVehicleRequestData,
        m_VehicleModelData = this.m_VehicleModelData,
        m_RouteColorData = this.m_RouteColorData,
        m_PrefabTransportDepotData = this.m_PrefabTransportDepotData,
        m_TransportLineData = this.m_TransportLineData,
        m_SetupData = setupData
      }.ScheduleParallel<TransportPathfindSetup.SetupTransportVehiclesJob>(this.m_TransportVehicleQuery, inputDeps);
    }

    public JobHandle SetupTaxi(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_TransportDepotType.Update((SystemBase) system);
      this.m_TaxiType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OutsideConnectionType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_TaxiRequestData.Update((SystemBase) system);
      this.m_TransportDepotData.Update((SystemBase) system);
      this.m_PrefabTransportDepotData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new TransportPathfindSetup.SetupTaxisJob()
      {
        m_EntityType = this.m_EntityType,
        m_TransportDepotType = this.m_TransportDepotType,
        m_TaxiType = this.m_TaxiType,
        m_OwnerType = this.m_OwnerType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OutsideConnectionType = this.m_OutsideConnectionType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_TaxiRequestData = this.m_TaxiRequestData,
        m_TransportDepotData = this.m_TransportDepotData,
        m_PrefabTransportDepotData = this.m_PrefabTransportDepotData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<TransportPathfindSetup.SetupTaxisJob>(this.m_TaxiQuery, inputDeps);
    }

    public JobHandle SetupRouteWaypoints(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_Waypoints.Update((SystemBase) system);
      return new TransportPathfindSetup.SetupRouteWaypointsJob()
      {
        m_Waypoints = this.m_Waypoints,
        m_SetupData = setupData
      }.Schedule<TransportPathfindSetup.SetupRouteWaypointsJob>(setupData.Length, 1, inputDeps);
    }

    public JobHandle SetupTransportVehicleRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_TransportVehicleRequestType.Update((SystemBase) system);
      this.m_TransportVehicleRequestData.Update((SystemBase) system);
      this.m_VehicleModelData.Update((SystemBase) system);
      this.m_PublicTransportData.Update((SystemBase) system);
      this.m_CargoTransportData.Update((SystemBase) system);
      this.m_TransportLineData.Update((SystemBase) system);
      this.m_PrefabTransportDepotData.Update((SystemBase) system);
      this.m_Waypoints.Update((SystemBase) system);
      return new TransportPathfindSetup.TransportVehicleRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_TransportVehicleRequestType = this.m_TransportVehicleRequestType,
        m_TransportVehicleRequestData = this.m_TransportVehicleRequestData,
        m_VehicleModelData = this.m_VehicleModelData,
        m_PublicTransportData = this.m_PublicTransportData,
        m_CargoTransportData = this.m_CargoTransportData,
        m_TransportLineData = this.m_TransportLineData,
        m_TransportDepotData = this.m_PrefabTransportDepotData,
        m_Waypoints = this.m_Waypoints,
        m_SetupData = setupData
      }.ScheduleParallel<TransportPathfindSetup.TransportVehicleRequestsJob>(this.m_TransportVehicleRequestQuery, inputDeps);
    }

    public JobHandle SetupTaxiRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_TaxiRequestType.Update((SystemBase) system);
      this.m_TaxiRequestData.Update((SystemBase) system);
      this.m_TransportDepotData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new TransportPathfindSetup.TaxiRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_TaxiRequestType = this.m_TaxiRequestType,
        m_TaxiRequestData = this.m_TaxiRequestData,
        m_TransportDepotData = this.m_TransportDepotData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<TransportPathfindSetup.TaxiRequestsJob>(this.m_TaxiRequestQuery, inputDeps);
    }

    [BurstCompile]
    private struct SetupTransportVehiclesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.CargoTransport> m_CargoTransportType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PublicTransport> m_PublicTransportType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.Color> m_RouteColorType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<VehicleModel> m_VehicleModelData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Color> m_RouteColorData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineData;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<Game.Buildings.TransportDepot> nativeArray2 = chunk.GetNativeArray<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType);
        Entity entity1;
        if (nativeArray2.Length != 0)
        {
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
          {
            Entity owner;
            PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
            // ISSUE: reference to a compiler-generated method
            this.m_SetupData.GetItem(index1, out entity1, out owner, out targetSeeker);
            TransportVehicleRequest componentData1;
            this.m_TransportVehicleRequestData.TryGetComponent(owner, out componentData1);
            TransportLineData componentData2 = new TransportLineData();
            PrefabRef componentData3;
            if (targetSeeker.m_PrefabRef.TryGetComponent(componentData1.m_Route, out componentData3))
              this.m_TransportLineData.TryGetComponent(componentData3.m_Prefab, out componentData2);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & TransportDepotFlags.HasAvailableVehicles) != (TransportDepotFlags) 0)
              {
                componentData3 = nativeArray3[index2];
                TransportDepotData componentData4;
                if (this.m_PrefabTransportDepotData.TryGetComponent(componentData3.m_Prefab, out componentData4) && componentData4.m_TransportType == componentData2.m_TransportType)
                {
                  float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                  Entity entity2 = nativeArray1[index2];
                  targetSeeker.FindTargets(entity2, cost);
                }
              }
            }
          }
        }
        else
        {
          if (!chunk.Has<Owner>(ref this.m_OwnerType))
            return;
          NativeArray<Game.Vehicles.CargoTransport> nativeArray4 = chunk.GetNativeArray<Game.Vehicles.CargoTransport>(ref this.m_CargoTransportType);
          NativeArray<Game.Vehicles.PublicTransport> nativeArray5 = chunk.GetNativeArray<Game.Vehicles.PublicTransport>(ref this.m_PublicTransportType);
          NativeArray<Controller> nativeArray6 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
          NativeArray<Game.Routes.Color> nativeArray7 = chunk.GetNativeArray<Game.Routes.Color>(ref this.m_RouteColorType);
          NativeArray<PrefabRef> nativeArray8 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
          for (int index3 = 0; index3 < this.m_SetupData.Length; ++index3)
          {
            Entity owner;
            PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
            // ISSUE: reference to a compiler-generated method
            this.m_SetupData.GetItem(index3, out entity1, out owner, out targetSeeker);
            TransportVehicleRequest componentData5;
            this.m_TransportVehicleRequestData.TryGetComponent(owner, out componentData5);
            TransportLineData componentData6 = new TransportLineData();
            PrefabRef componentData7;
            if (targetSeeker.m_PrefabRef.TryGetComponent(componentData5.m_Route, out componentData7))
              this.m_TransportLineData.TryGetComponent(componentData7.m_Prefab, out componentData6);
            VehicleModel componentData8;
            bool component1 = this.m_VehicleModelData.TryGetComponent(componentData5.m_Route, out componentData8);
            Game.Routes.Color componentData9;
            bool component2 = this.m_RouteColorData.TryGetComponent(componentData5.m_Route, out componentData9);
            if (nativeArray4.Length != 0 == componentData6.m_CargoTransport && nativeArray5.Length != 0 == componentData6.m_PassengerTransport)
            {
              for (int index4 = 0; index4 < nativeArray1.Length; ++index4)
              {
                Entity entity3 = nativeArray1[index4];
                float cost = 0.0f;
                if (nativeArray6.Length != 0)
                {
                  Controller controller = nativeArray6[index4];
                  if (controller.m_Controller != Entity.Null && controller.m_Controller != entity3)
                    continue;
                }
                if (nativeArray4.Length != 0)
                {
                  Game.Vehicles.CargoTransport cargoTransport = nativeArray4[index4];
                  if (cargoTransport.m_RequestCount > 0 || (cargoTransport.m_State & (CargoTransportFlags.EnRoute | CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.DummyTraffic | CargoTransportFlags.Disabled)) != (CargoTransportFlags) 0)
                    continue;
                }
                if (nativeArray5.Length != 0)
                {
                  Game.Vehicles.PublicTransport publicTransport = nativeArray5[index4];
                  if (publicTransport.m_RequestCount > 0 || (publicTransport.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.Evacuating | PublicTransportFlags.PrisonerTransport | PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.DummyTraffic | PublicTransportFlags.Disabled)) != (PublicTransportFlags) 0)
                    continue;
                }
                if (component1)
                {
                  componentData7 = nativeArray8[index4];
                  DynamicBuffer<LayoutElement> layout = new DynamicBuffer<LayoutElement>();
                  if (bufferAccessor.Length != 0)
                    layout = bufferAccessor[index4];
                  if (!RouteUtils.CheckVehicleModel(componentData8, componentData7, layout, ref targetSeeker.m_PrefabRef))
                    continue;
                }
                Game.Routes.Color color;
                if (CollectionUtils.TryGet<Game.Routes.Color>(nativeArray7, index4, out color))
                {
                  if (component2 && (int) componentData9.m_Color.r == (int) color.m_Color.r && (int) componentData9.m_Color.g == (int) color.m_Color.g && (int) componentData9.m_Color.b == (int) color.m_Color.b && (int) componentData9.m_Color.a == (int) color.m_Color.a)
                    cost -= targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                }
                else
                  cost -= targetSeeker.m_PathfindParameters.m_Weights.time * math.select(10f, 5f, component2);
                targetSeeker.FindTargets(entity3, cost);
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
    private struct SetupTaxisJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TransportDepot> m_TransportDepotType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.Taxi> m_TaxiType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> m_TaxiRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
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
        NativeArray<Game.Buildings.TransportDepot> nativeArray2 = chunk.GetNativeArray<Game.Buildings.TransportDepot>(ref this.m_TransportDepotType);
        Entity entity1;
        if (nativeArray2.Length != 0)
        {
          NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          bool flag = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Game.Buildings.TransportDepot transportDepot = nativeArray2[index1];
            if ((transportDepot.m_Flags & TransportDepotFlags.HasAvailableVehicles) != (TransportDepotFlags) 0)
            {
              for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
              {
                Entity owner;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index2, out entity1, out owner, out targetSeeker);
                TaxiRequest componentData;
                this.m_TaxiRequestData.TryGetComponent(owner, out componentData);
                switch (componentData.m_Type)
                {
                  case TaxiRequestType.Stand:
                    if (!flag)
                      break;
                    continue;
                  case TaxiRequestType.Customer:
                    if ((transportDepot.m_Flags & TransportDepotFlags.HasDispatchCenter) != (TransportDepotFlags) 0)
                      break;
                    continue;
                  case TaxiRequestType.Outside:
                    if (!flag)
                      continue;
                    break;
                  default:
                    continue;
                }
                if (this.m_PrefabTransportDepotData[nativeArray3[index1].m_Prefab].m_TransportType == TransportType.Taxi)
                {
                  Entity entity2 = nativeArray1[index1];
                  if (AreaUtils.CheckServiceDistrict(componentData.m_District1, componentData.m_District2, entity2, this.m_ServiceDistricts))
                  {
                    float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                    targetSeeker.FindTargets(entity2, cost);
                  }
                }
              }
            }
          }
        }
        else
        {
          NativeArray<Game.Vehicles.Taxi> nativeArray4 = chunk.GetNativeArray<Game.Vehicles.Taxi>(ref this.m_TaxiType);
          if (nativeArray4.Length == 0)
            return;
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          NativeArray<PathOwner> nativeArray6 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
          {
            Entity entity3 = nativeArray1[index3];
            Game.Vehicles.Taxi taxi = nativeArray4[index3];
            Owner owner1 = nativeArray5[index3];
            Game.Buildings.TransportDepot componentData1;
            if ((taxi.m_State & (TaxiFlags.RequiresMaintenance | TaxiFlags.Dispatched | TaxiFlags.Disabled)) == (TaxiFlags) 0 && this.m_TransportDepotData.TryGetComponent(owner1.m_Owner, out componentData1))
            {
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity owner2;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity1, out owner2, out targetSeeker);
                TaxiRequest componentData2;
                this.m_TaxiRequestData.TryGetComponent(owner2, out componentData2);
                switch (componentData2.m_Type)
                {
                  case TaxiRequestType.Stand:
                    if ((taxi.m_State & (TaxiFlags.Returning | TaxiFlags.Transporting)) == (TaxiFlags) 0 && nativeArray6.Length != 0 || (taxi.m_State & TaxiFlags.FromOutside) != (TaxiFlags) 0)
                      continue;
                    break;
                  case TaxiRequestType.Customer:
                    if ((componentData1.m_Flags & TransportDepotFlags.HasDispatchCenter) != (TransportDepotFlags) 0)
                      break;
                    continue;
                  case TaxiRequestType.Outside:
                    if ((taxi.m_State & TaxiFlags.FromOutside) == (TaxiFlags) 0)
                      continue;
                    break;
                  default:
                    continue;
                }
                DynamicBuffer<ServiceDispatch> dynamicBuffer1;
                TaxiRequest componentData3;
                if (AreaUtils.CheckServiceDistrict(componentData2.m_District1, componentData2.m_District2, owner1.m_Owner, this.m_ServiceDistricts) && (!CollectionUtils.TryGet<ServiceDispatch>(bufferAccessor2, index3, out dynamicBuffer1) || dynamicBuffer1.Length == 0 || (taxi.m_State & TaxiFlags.Requested) == (TaxiFlags) 0 || !this.m_TaxiRequestData.TryGetComponent(dynamicBuffer1[0].m_Request, out componentData3) || componentData2.m_Type >= componentData3.m_Type && (componentData2.m_Type != componentData3.m_Type || componentData2.m_Priority > componentData3.m_Priority)))
                {
                  PathOwner pathOwner;
                  if (CollectionUtils.TryGet<PathOwner>(nativeArray6, index3, out pathOwner))
                  {
                    DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                    int num = dynamicBuffer2.Length - taxi.m_ExtraPathElementCount;
                    if (num > dynamicBuffer2.Length || (taxi.m_State & TaxiFlags.Transporting) == (TaxiFlags) 0)
                      targetSeeker.FindTargets(entity3, 0.0f);
                    else if (num <= pathOwner.m_ElementIndex)
                    {
                      targetSeeker.FindTargets(entity3, entity3, 0.0f, EdgeFlags.DefaultMask, true, true);
                    }
                    else
                    {
                      float cost = (float) (num - pathOwner.m_ElementIndex) * taxi.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
                      PathElement pathElement = dynamicBuffer2[num - 1];
                      targetSeeker.m_Buffer.Enqueue(new PathTarget(entity3, pathElement.m_Target, pathElement.m_TargetDelta.y, cost));
                    }
                  }
                  else
                    targetSeeker.FindTargets(entity3, 0.0f);
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
    private struct SetupRouteWaypointsJob : IJobParallelFor
    {
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(int index)
      {
        Entity entity;
        PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
        // ISSUE: reference to a compiler-generated method
        this.m_SetupData.GetItem(index, out entity, out targetSeeker);
        if (!this.m_Waypoints.HasBuffer(entity))
          return;
        DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[entity];
        for (int index1 = 0; index1 < waypoint1.Length; ++index1)
        {
          Entity waypoint2 = waypoint1[index1].m_Waypoint;
          if (targetSeeker.m_RouteLane.HasComponent(waypoint2))
          {
            RouteLane routeLane = targetSeeker.m_RouteLane[waypoint2];
            if (!(routeLane.m_StartLane == Entity.Null))
              targetSeeker.m_Buffer.Enqueue(new PathTarget(waypoint2, routeLane.m_StartLane, routeLane.m_StartCurvePos, 0.0f));
          }
        }
      }
    }

    [BurstCompile]
    private struct TransportVehicleRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<TransportVehicleRequest> m_TransportVehicleRequestType;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<VehicleModel> m_VehicleModelData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_TransportDepotData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_Waypoints;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<ServiceRequest> nativeArray2 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
        NativeArray<TransportVehicleRequest> nativeArray3 = chunk.GetNativeArray<TransportVehicleRequest>(ref this.m_TransportVehicleRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          TransportVehicleRequest componentData1;
          PrefabRef componentData2;
          if (this.m_TransportVehicleRequestData.TryGetComponent(owner, out componentData1) && targetSeeker.m_PrefabRef.TryGetComponent(componentData1.m_Route, out componentData2))
          {
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            DynamicBuffer<LayoutElement> bufferData = new DynamicBuffer<LayoutElement>();
            TransportDepotData componentData3;
            if (this.m_TransportDepotData.TryGetComponent(componentData2.m_Prefab, out componentData3))
            {
              flag1 = true;
            }
            else
            {
              flag2 = this.m_PublicTransportData.HasComponent(componentData1.m_Route);
              flag3 = this.m_CargoTransportData.HasComponent(componentData1.m_Route);
              targetSeeker.m_VehicleLayout.TryGetBuffer(componentData1.m_Route, out bufferData);
            }
            if (flag1 || flag2 || flag3)
            {
              for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
              {
                if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
                {
                  TransportVehicleRequest transportVehicleRequest = nativeArray3[index2];
                  PrefabRef componentData4;
                  TransportLineData componentData5;
                  if (targetSeeker.m_PrefabRef.TryGetComponent(transportVehicleRequest.m_Route, out componentData4) && this.m_TransportLineData.TryGetComponent(componentData4.m_Prefab, out componentData5))
                  {
                    if (flag1)
                    {
                      if (componentData5.m_TransportType != componentData3.m_TransportType)
                        continue;
                    }
                    else
                    {
                      VehicleModel componentData6;
                      if (flag3 != componentData5.m_CargoTransport || flag2 != componentData5.m_PassengerTransport || this.m_VehicleModelData.TryGetComponent(transportVehicleRequest.m_Route, out componentData6) && !RouteUtils.CheckVehicleModel(componentData6, componentData2, bufferData, ref targetSeeker.m_PrefabRef))
                        continue;
                    }
                    Entity target = nativeArray1[index2];
                    DynamicBuffer<RouteWaypoint> waypoint1 = this.m_Waypoints[transportVehicleRequest.m_Route];
                    for (int index3 = 0; index3 < waypoint1.Length; ++index3)
                    {
                      Entity waypoint2 = waypoint1[index3].m_Waypoint;
                      if (targetSeeker.m_RouteLane.HasComponent(waypoint2))
                      {
                        RouteLane routeLane = targetSeeker.m_RouteLane[waypoint2];
                        if (!(routeLane.m_StartLane == Entity.Null))
                          targetSeeker.m_Buffer.Enqueue(new PathTarget(target, routeLane.m_StartLane, routeLane.m_StartCurvePos, 0.0f));
                      }
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
    private struct TaxiRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<TaxiRequest> m_TaxiRequestType;
      [ReadOnly]
      public ComponentLookup<TaxiRequest> m_TaxiRequestData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
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
        NativeArray<TaxiRequest> nativeArray3 = chunk.GetNativeArray<TaxiRequest>(ref this.m_TaxiRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          TaxiRequest componentData1;
          if (this.m_TaxiRequestData.TryGetComponent(owner, out componentData1))
          {
            bool flag = false;
            Entity service = Entity.Null;
            if (componentData1.m_Type != TaxiRequestType.Outside)
            {
              Game.Buildings.TransportDepot componentData2;
              if (this.m_TransportDepotData.TryGetComponent(componentData1.m_Seeker, out componentData2))
              {
                flag = (componentData2.m_Flags & TransportDepotFlags.HasDispatchCenter) != 0;
                service = componentData1.m_Seeker;
              }
              else if (targetSeeker.m_PrefabRef.HasComponent(componentData1.m_Seeker))
              {
                Owner componentData3;
                if (targetSeeker.m_Owner.TryGetComponent(componentData1.m_Seeker, out componentData3) && this.m_TransportDepotData.TryGetComponent(componentData3.m_Owner, out componentData2))
                {
                  flag = (componentData2.m_Flags & TransportDepotFlags.HasDispatchCenter) != 0;
                  service = componentData3.m_Owner;
                }
              }
              else
                continue;
            }
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                TaxiRequest taxiRequest = nativeArray3[index2];
                switch (taxiRequest.m_Type)
                {
                  case TaxiRequestType.Stand:
                    if (componentData1.m_Type != TaxiRequestType.Outside)
                    {
                      targetSeeker.m_SetupQueueTarget.m_Methods = PathMethod.Road;
                      break;
                    }
                    continue;
                  case TaxiRequestType.Customer:
                    if (flag)
                    {
                      targetSeeker.m_SetupQueueTarget.m_Methods = PathMethod.Boarding;
                      break;
                    }
                    continue;
                  case TaxiRequestType.Outside:
                    if (componentData1.m_Type == TaxiRequestType.Outside)
                    {
                      targetSeeker.m_SetupQueueTarget.m_Methods = PathMethod.Boarding;
                      break;
                    }
                    continue;
                  default:
                    continue;
                }
                if (AreaUtils.CheckServiceDistrict(taxiRequest.m_District1, taxiRequest.m_District2, service, this.m_ServiceDistricts))
                  targetSeeker.FindTargets(nativeArray1[index2], taxiRequest.m_Seeker, 0.0f, EdgeFlags.DefaultMask, true, false);
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
