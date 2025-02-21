// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportVehicleDispatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
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
  public class TransportVehicleDispatchSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_RequestQuery;
    private TransportVehicleDispatchSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<TransportVehicleRequest>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex >> 4 & 7U;
      uint num2 = (uint) ((int) num1 + 4 & 7);
      NativeQueue<TransportVehicleDispatchSystem.VehicleDispatch> nativeQueue = new NativeQueue<TransportVehicleDispatchSystem.VehicleDispatch>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TransportDepot_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportLine_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_DispatchedRequest_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AirplaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TransportVehicleDispatchSystem.TransportVehicleDispatchJob jobData1 = new TransportVehicleDispatchSystem.TransportVehicleDispatchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransportVehicleRequestType = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentTypeHandle,
        m_DispatchedType = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle,
        m_TransportVehicleRequestData = this.__TypeHandle.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabTransportDepotData = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
        m_PrefabCarData = this.__TypeHandle.__Game_Prefabs_CarData_RO_ComponentLookup,
        m_PrefabWatercraftData = this.__TypeHandle.__Game_Prefabs_WatercraftData_RO_ComponentLookup,
        m_PrefabAirplaneData = this.__TypeHandle.__Game_Prefabs_AirplaneData_RO_ComponentLookup,
        m_PrefabHelicopterData = this.__TypeHandle.__Game_Prefabs_HelicopterData_RO_ComponentLookup,
        m_PrefabTrainData = this.__TypeHandle.__Game_Prefabs_TrainData_RO_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_DispatchedRequests = this.__TypeHandle.__Game_Routes_DispatchedRequest_RO_BufferLookup,
        m_TransportLineData = this.__TypeHandle.__Game_Routes_TransportLine_RW_ComponentLookup,
        m_TransportDepotData = this.__TypeHandle.__Game_Buildings_TransportDepot_RW_ComponentLookup,
        m_PublicTransportData = this.__TypeHandle.__Game_Vehicles_PublicTransport_RW_ComponentLookup,
        m_CargoTransportData = this.__TypeHandle.__Game_Vehicles_CargoTransport_RW_ComponentLookup,
        m_UpdateFrameIndex = num1,
        m_NextUpdateFrameIndex = num2,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_VehicleDispatches = nativeQueue.AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TransportVehicleDispatchSystem.DispatchVehiclesJob jobData2 = new TransportVehicleDispatchSystem.DispatchVehiclesJob()
      {
        m_VehicleDispatches = nativeQueue,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<TransportVehicleDispatchSystem.TransportVehicleDispatchJob>(this.m_RequestQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<TransportVehicleDispatchSystem.DispatchVehiclesJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = inputDeps;
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
    public TransportVehicleDispatchSystem()
    {
    }

    private struct VehicleDispatch
    {
      public Entity m_Request;
      public Entity m_Source;

      public VehicleDispatch(Entity request, Entity source)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Request = request;
        // ISSUE: reference to a compiler-generated field
        this.m_Source = source;
      }
    }

    [BurstCompile]
    private struct TransportVehicleDispatchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<TransportVehicleRequest> m_TransportVehicleRequestType;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> m_DispatchedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> m_TransportVehicleRequestData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_PrefabTransportDepotData;
      [ReadOnly]
      public ComponentLookup<CarData> m_PrefabCarData;
      [ReadOnly]
      public ComponentLookup<WatercraftData> m_PrefabWatercraftData;
      [ReadOnly]
      public ComponentLookup<AirplaneData> m_PrefabAirplaneData;
      [ReadOnly]
      public ComponentLookup<HelicopterData> m_PrefabHelicopterData;
      [ReadOnly]
      public ComponentLookup<TrainData> m_PrefabTrainData;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [ReadOnly]
      public BufferLookup<DispatchedRequest> m_DispatchedRequests;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TransportLine> m_TransportLineData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Buildings.TransportDepot> m_TransportDepotData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.PublicTransport> m_PublicTransportData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.CargoTransport> m_CargoTransportData;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_NextUpdateFrameIndex;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<TransportVehicleDispatchSystem.VehicleDispatch>.ParallelWriter m_VehicleDispatches;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        uint index1 = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) index1 == (int) this.m_NextUpdateFrameIndex && !chunk.Has<Dispatched>(ref this.m_DispatchedType) && !chunk.Has<PathInformation>(ref this.m_PathInformationType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TransportVehicleRequest> nativeArray2 = chunk.GetNativeArray<TransportVehicleRequest>(ref this.m_TransportVehicleRequestType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ServiceRequest> nativeArray3 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            TransportVehicleRequest transportVehicleRequest = nativeArray2[index2];
            ServiceRequest serviceRequest = nativeArray3[index2];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, transportVehicleRequest.m_Route))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleTarget(unfilteredChunkIndex, entity, transportVehicleRequest.m_Route);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, transportVehicleRequest.m_Route, false))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleSource(unfilteredChunkIndex, entity, transportVehicleRequest.m_Route);
              }
            }
            nativeArray3[index2] = serviceRequest;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((int) index1 != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Dispatched> nativeArray4 = chunk.GetNativeArray<Dispatched>(ref this.m_DispatchedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TransportVehicleRequest> nativeArray5 = chunk.GetNativeArray<TransportVehicleRequest>(ref this.m_TransportVehicleRequestType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceRequest> nativeArray6 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
        if (nativeArray4.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray7 = chunk.GetNativeArray(this.m_EntityType);
          for (int index3 = 0; index3 < nativeArray4.Length; ++index3)
          {
            Entity entity = nativeArray7[index3];
            Dispatched dispatched = nativeArray4[index3];
            TransportVehicleRequest transportVehicleRequest = nativeArray5[index3];
            ServiceRequest serviceRequest = nativeArray6[index3];
            // ISSUE: reference to a compiler-generated method
            if (this.ValidateHandler(entity, dispatched.m_Handler))
              serviceRequest.m_Cooldown = (byte) 0;
            else if (serviceRequest.m_Cooldown == (byte) 0)
            {
              serviceRequest.m_Cooldown = (byte) 1;
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, transportVehicleRequest.m_Route, true))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              // ISSUE: reference to a compiler-generated method
              this.ResetFailedRequest(unfilteredChunkIndex, entity, true, ref serviceRequest);
            }
            nativeArray6[index3] = serviceRequest;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<PathInformation> nativeArray8 = chunk.GetNativeArray<PathInformation>(ref this.m_PathInformationType);
          if (nativeArray8.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray9 = chunk.GetNativeArray(this.m_EntityType);
          for (int index4 = 0; index4 < nativeArray5.Length; ++index4)
          {
            Entity entity = nativeArray9[index4];
            TransportVehicleRequest transportVehicleRequest = nativeArray5[index4];
            PathInformation pathInformation = nativeArray8[index4];
            ServiceRequest serviceRequest = nativeArray6[index4];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, transportVehicleRequest.m_Route))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (pathInformation.m_Destination != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.ResetReverseRequest(unfilteredChunkIndex, entity, pathInformation, ref serviceRequest);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.ResetFailedRequest(unfilteredChunkIndex, entity, false, ref serviceRequest);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, transportVehicleRequest.m_Route, false))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (pathInformation.m_Origin != Entity.Null)
              {
                // ISSUE: reference to a compiler-generated method
                this.DispatchVehicle(unfilteredChunkIndex, entity, pathInformation);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.ResetFailedRequest(unfilteredChunkIndex, entity, false, ref serviceRequest);
              }
            }
            nativeArray6[index4] = serviceRequest;
          }
        }
      }

      private bool ValidateReversed(Entity entity, Entity source)
      {
        Game.Buildings.TransportDepot componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransportDepotData.TryGetComponent(source, out componentData1))
        {
          if ((componentData1.m_Flags & TransportDepotFlags.HasAvailableVehicles) == (TransportDepotFlags) 0)
            return false;
          if (componentData1.m_TargetRequest != entity)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(componentData1.m_TargetRequest))
              return false;
            componentData1.m_TargetRequest = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_TransportDepotData[source] = componentData1;
          }
          return true;
        }
        Game.Vehicles.PublicTransport componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PublicTransportData.TryGetComponent(source, out componentData2))
        {
          // ISSUE: reference to a compiler-generated field
          if ((componentData2.m_State & (PublicTransportFlags.EnRoute | PublicTransportFlags.RequiresMaintenance | PublicTransportFlags.Disabled)) != (PublicTransportFlags) 0 || componentData2.m_RequestCount > 0 || this.m_ParkedCarData.HasComponent(source))
            return false;
          if (componentData2.m_TargetRequest != entity)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransportVehicleRequestData.HasComponent(componentData2.m_TargetRequest))
              return false;
            componentData2.m_TargetRequest = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_PublicTransportData[source] = componentData2;
          }
          return true;
        }
        Game.Vehicles.CargoTransport componentData3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CargoTransportData.TryGetComponent(source, out componentData3) || (componentData3.m_State & (CargoTransportFlags.EnRoute | CargoTransportFlags.RequiresMaintenance | CargoTransportFlags.Disabled)) != (CargoTransportFlags) 0 || componentData3.m_RequestCount > 0 || this.m_ParkedCarData.HasComponent(source) || this.m_ParkedTrainData.HasComponent(source))
          return false;
        if (componentData3.m_TargetRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(componentData3.m_TargetRequest))
            return false;
          componentData3.m_TargetRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_CargoTransportData[source] = componentData3;
        }
        return true;
      }

      private bool ValidateHandler(Entity entity, Entity handler)
      {
        DynamicBuffer<ServiceDispatch> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceDispatches.TryGetBuffer(handler, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            if (bufferData[index].m_Request == entity)
              return true;
          }
        }
        return false;
      }

      private bool ValidateTarget(Entity entity, Entity target, bool dispatched)
      {
        TransportLine componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TransportLineData.TryGetComponent(target, out componentData) || (componentData.m_Flags & TransportLineFlags.RequireVehicles) == (TransportLineFlags) 0)
          return false;
        DynamicBuffer<DispatchedRequest> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_DispatchedRequests.TryGetBuffer(target, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            if (bufferData[index].m_VehicleRequest == entity)
              return !dispatched;
          }
        }
        if (componentData.m_VehicleRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransportVehicleRequestData.HasComponent(componentData.m_VehicleRequest))
            return false;
          componentData.m_VehicleRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_TransportLineData[target] = componentData;
        }
        return true;
      }

      private void ResetReverseRequest(
        int jobIndex,
        Entity entity,
        PathInformation pathInformation,
        ref ServiceRequest serviceRequest)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TransportVehicleDispatchSystem.VehicleDispatch vehicleDispatch = new TransportVehicleDispatchSystem.VehicleDispatch(entity, pathInformation.m_Destination);
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleDispatches.Enqueue(vehicleDispatch);
        SimulationUtils.ResetReverseRequest(ref serviceRequest);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<PathInformation>(jobIndex, entity);
      }

      private void ResetFailedRequest(
        int jobIndex,
        Entity entity,
        bool dispatched,
        ref ServiceRequest serviceRequest)
      {
        SimulationUtils.ResetFailedRequest(ref serviceRequest);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<PathInformation>(jobIndex, entity);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<PathElement>(jobIndex, entity);
        if (!dispatched)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<Dispatched>(jobIndex, entity);
      }

      private void DispatchVehicle(int jobIndex, Entity entity, PathInformation pathInformation)
      {
        Entity entity1 = pathInformation.m_Origin;
        Owner componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((this.m_ParkedCarData.HasComponent(entity1) || this.m_ParkedTrainData.HasComponent(entity1)) && this.m_OwnerData.TryGetComponent(entity1, out componentData))
          entity1 = componentData.m_Owner;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TransportVehicleDispatchSystem.VehicleDispatch vehicleDispatch = new TransportVehicleDispatchSystem.VehicleDispatch(entity, entity1);
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleDispatches.Enqueue(vehicleDispatch);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Dispatched>(jobIndex, entity, new Dispatched(entity1));
      }

      private void FindVehicleSource(int jobIndex, Entity requestEntity, Entity targetRoute)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[this.m_PrefabRefData[targetRoute].m_Prefab];
        PathMethod pathMethods = RouteUtils.GetPathMethods(routeConnectionData.m_RouteConnectionType, routeConnectionData.m_RouteTrackType, routeConnectionData.m_RouteRoadType);
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 277.777771f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = pathMethods,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic,
          m_PathfindFlags = PathfindFlags.IgnoreExtraEndAccessRequirements
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.TransportVehicle,
          m_Methods = pathMethods,
          m_TrackTypes = routeConnectionData.m_RouteTrackType,
          m_RoadTypes = routeConnectionData.m_RouteRoadType
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.RouteWaypoints,
          m_Methods = pathMethods,
          m_TrackTypes = routeConnectionData.m_RouteTrackType,
          m_RoadTypes = routeConnectionData.m_RouteRoadType,
          m_Entity = targetRoute
        };
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters, origin, destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, requestEntity);
      }

      private void FindVehicleTarget(int jobIndex, Entity requestEntity, Entity vehicleSource)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[vehicleSource];
        PathMethod pathMethod = (PathMethod) 0;
        TrackTypes trackTypes = TrackTypes.None;
        RoadTypes roadTypes = RoadTypes.None;
        TransportDepotData componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabTransportDepotData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
        {
          switch (componentData1.m_TransportType)
          {
            case TransportType.Bus:
              pathMethod = PathMethod.Road;
              roadTypes = RoadTypes.Car;
              break;
            case TransportType.Train:
              pathMethod = PathMethod.Track;
              trackTypes = TrackTypes.Train;
              break;
            case TransportType.Tram:
              pathMethod = PathMethod.Track;
              trackTypes = TrackTypes.Tram;
              break;
            case TransportType.Ship:
              pathMethod = PathMethod.Road;
              roadTypes = RoadTypes.Watercraft;
              break;
            case TransportType.Helicopter:
              pathMethod = PathMethod.Road | PathMethod.Flying;
              roadTypes = RoadTypes.Helicopter;
              break;
            case TransportType.Airplane:
              pathMethod = PathMethod.Road | PathMethod.Flying;
              roadTypes = RoadTypes.Airplane;
              break;
            case TransportType.Subway:
              pathMethod = PathMethod.Track;
              trackTypes = TrackTypes.Subway;
              break;
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabCarData.HasComponent(prefabRef.m_Prefab))
          {
            pathMethod = PathMethod.Road;
            roadTypes = RoadTypes.Car;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabWatercraftData.HasComponent(prefabRef.m_Prefab))
            {
              pathMethod = PathMethod.Road;
              roadTypes = RoadTypes.Watercraft;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabAirplaneData.HasComponent(prefabRef.m_Prefab))
              {
                pathMethod = PathMethod.Road | PathMethod.Flying;
                roadTypes = RoadTypes.Airplane;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabHelicopterData.HasComponent(prefabRef.m_Prefab))
                {
                  pathMethod = PathMethod.Road | PathMethod.Flying;
                  roadTypes = RoadTypes.Helicopter;
                }
                else
                {
                  TrainData componentData2;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabTrainData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
                  {
                    pathMethod = PathMethod.Track;
                    trackTypes = componentData2.m_TrackType;
                  }
                }
              }
            }
          }
        }
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 277.777771f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = pathMethod,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = pathMethod;
        setupQueueTarget.m_TrackTypes = trackTypes;
        setupQueueTarget.m_RoadTypes = roadTypes;
        setupQueueTarget.m_Entity = vehicleSource;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.TransportVehicleRequest;
        setupQueueTarget.m_Methods = pathMethod;
        setupQueueTarget.m_TrackTypes = trackTypes;
        setupQueueTarget.m_RoadTypes = roadTypes;
        SetupQueueTarget destination = setupQueueTarget;
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters, origin, destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation());
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
    private struct DispatchVehiclesJob : IJob
    {
      public NativeQueue<TransportVehicleDispatchSystem.VehicleDispatch> m_VehicleDispatches;
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        TransportVehicleDispatchSystem.VehicleDispatch vehicleDispatch;
        // ISSUE: reference to a compiler-generated field
        while (this.m_VehicleDispatches.TryDequeue(out vehicleDispatch))
        {
          DynamicBuffer<ServiceDispatch> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceDispatches.TryGetBuffer(vehicleDispatch.m_Source, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            bufferData.Add(new ServiceDispatch(vehicleDispatch.m_Request));
          }
          else
          {
            ServiceRequest componentData;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceRequestData.TryGetComponent(vehicleDispatch.m_Source, out componentData))
            {
              componentData.m_Flags |= ServiceRequestFlags.SkipCooldown;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ServiceRequestData[vehicleDispatch.m_Source] = componentData;
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> __Game_Simulation_Dispatched_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<TransportVehicleRequest> __Game_Simulation_TransportVehicleRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarData> __Game_Prefabs_CarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftData> __Game_Prefabs_WatercraftData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AirplaneData> __Game_Prefabs_AirplaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HelicopterData> __Game_Prefabs_HelicopterData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainData> __Game_Prefabs_TrainData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<DispatchedRequest> __Game_Routes_DispatchedRequest_RO_BufferLookup;
      public ComponentLookup<TransportLine> __Game_Routes_TransportLine_RW_ComponentLookup;
      public ComponentLookup<Game.Buildings.TransportDepot> __Game_Buildings_TransportDepot_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.PublicTransport> __Game_Vehicles_PublicTransport_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.CargoTransport> __Game_Vehicles_CargoTransport_RW_ComponentLookup;
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentLookup;
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportVehicleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TransportVehicleRequest_RO_ComponentLookup = state.GetComponentLookup<TransportVehicleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarData_RO_ComponentLookup = state.GetComponentLookup<CarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WatercraftData_RO_ComponentLookup = state.GetComponentLookup<WatercraftData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AirplaneData_RO_ComponentLookup = state.GetComponentLookup<AirplaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_HelicopterData_RO_ComponentLookup = state.GetComponentLookup<HelicopterData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrainData_RO_ComponentLookup = state.GetComponentLookup<TrainData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_DispatchedRequest_RO_BufferLookup = state.GetBufferLookup<DispatchedRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportLine_RW_ComponentLookup = state.GetComponentLookup<TransportLine>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TransportDepot_RW_ComponentLookup = state.GetComponentLookup<Game.Buildings.TransportDepot>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PublicTransport_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PublicTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CargoTransport_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.CargoTransport>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentLookup = state.GetComponentLookup<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferLookup = state.GetBufferLookup<ServiceDispatch>();
      }
    }
  }
}
