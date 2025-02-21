// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GarbageTransferDispatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Economy;
using Game.Net;
using Game.Pathfind;
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
  public class GarbageTransferDispatchSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_RequestQuery;
    private GarbageTransferDispatchSystem.TypeHandle __TypeHandle;

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
      this.m_RequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<GarbageTransferRequest>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex >> 4 & 7U;
      uint num2 = (uint) ((int) num1 + 4 & 7);
      NativeQueue<GarbageTransferDispatchSystem.DispatchAction> nativeQueue = new NativeQueue<GarbageTransferDispatchSystem.DispatchAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GarbageTransferDispatchSystem.GarbageTransferDispatchJob jobData1 = new GarbageTransferDispatchSystem.GarbageTransferDispatchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_GarbageTransferRequestType = this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentTypeHandle,
        m_DispatchedType = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle,
        m_GarbageTransferRequestData = this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup,
        m_GarbageFacilityData = this.__TypeHandle.__Game_Buildings_GarbageFacility_RO_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_TripNeededs = this.__TypeHandle.__Game_Citizens_TripNeeded_RO_BufferLookup,
        m_UpdateFrameIndex = num1,
        m_NextUpdateFrameIndex = num2,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_DispatchActions = nativeQueue.AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_GarbageFacility_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GarbageTransferDispatchSystem.DispatchActionJob jobData2 = new GarbageTransferDispatchSystem.DispatchActionJob()
      {
        m_RequestData = this.__TypeHandle.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup,
        m_OutsideConnectionData = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_GarbageFacilityData = this.__TypeHandle.__Game_Buildings_GarbageFacility_RW_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup,
        m_TripNeededs = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_DispatchActions = nativeQueue,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle handle = jobData1.ScheduleParallel<GarbageTransferDispatchSystem.GarbageTransferDispatchJob>(this.m_RequestQuery, this.Dependency);
      JobHandle dependsOn = handle;
      JobHandle jobHandle = jobData2.Schedule<GarbageTransferDispatchSystem.DispatchActionJob>(dependsOn);
      nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(handle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public GarbageTransferDispatchSystem()
    {
    }

    private struct DispatchAction
    {
      public Entity m_Request;
      public Entity m_DispatchSource;
      public Entity m_DeliverFacility;
      public Entity m_ReceiveFacility;

      public DispatchAction(
        Entity request,
        Entity dispatchSource,
        Entity deliverFacility,
        Entity receiveFacility)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Request = request;
        // ISSUE: reference to a compiler-generated field
        this.m_DispatchSource = dispatchSource;
        // ISSUE: reference to a compiler-generated field
        this.m_DeliverFacility = deliverFacility;
        // ISSUE: reference to a compiler-generated field
        this.m_ReceiveFacility = receiveFacility;
      }
    }

    [BurstCompile]
    private struct GarbageTransferDispatchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<GarbageTransferRequest> m_GarbageTransferRequestType;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> m_DispatchedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentLookup<GarbageTransferRequest> m_GarbageTransferRequestData;
      [ReadOnly]
      public ComponentLookup<GarbageFacility> m_GarbageFacilityData;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [ReadOnly]
      public BufferLookup<TripNeeded> m_TripNeededs;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_NextUpdateFrameIndex;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<GarbageTransferDispatchSystem.DispatchAction>.ParallelWriter m_DispatchActions;
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
          NativeArray<GarbageTransferRequest> nativeArray2 = chunk.GetNativeArray<GarbageTransferRequest>(ref this.m_GarbageTransferRequestType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ServiceRequest> nativeArray3 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            GarbageTransferRequest requestData = nativeArray2[index2];
            ServiceRequest serviceRequest = nativeArray3[index2];
            // ISSUE: reference to a compiler-generated method
            if (!this.ValidateTarget(entity, requestData))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
            }
            else
            {
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleSource(unfilteredChunkIndex, entity, requestData);
              }
              nativeArray3[index2] = serviceRequest;
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((int) index1 != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Dispatched> nativeArray4 = chunk.GetNativeArray<Dispatched>(ref this.m_DispatchedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GarbageTransferRequest> nativeArray5 = chunk.GetNativeArray<GarbageTransferRequest>(ref this.m_GarbageTransferRequestType);
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
            GarbageTransferRequest requestData = nativeArray5[index3];
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
              if (!this.ValidateTarget(entity, requestData))
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
            GarbageTransferRequest requestData = nativeArray5[index4];
            PathInformation pathInformation = nativeArray8[index4];
            ServiceRequest serviceRequest = nativeArray6[index4];
            // ISSUE: reference to a compiler-generated method
            if (!this.ValidateTarget(entity, requestData))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
            }
            else
            {
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
              nativeArray6[index4] = serviceRequest;
            }
          }
        }
      }

      private bool ValidateHandler(Entity entity, Entity handler)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_ServiceDispatches.HasBuffer(handler))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ServiceDispatch> serviceDispatch = this.m_ServiceDispatches[handler];
          for (int index = 0; index < serviceDispatch.Length; ++index)
          {
            if (serviceDispatch[index].m_Request == entity)
              return true;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TripNeededs.HasBuffer(handler))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<TripNeeded> tripNeeded = this.m_TripNeededs[handler];
          for (int index = 0; index < tripNeeded.Length; ++index)
          {
            if (tripNeeded[index].m_TargetAgent == entity)
              return true;
          }
        }
        return false;
      }

      private bool ValidateTarget(Entity entity, GarbageTransferRequest requestData)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_GarbageFacilityData.HasComponent(requestData.m_Facility))
          return false;
        // ISSUE: reference to a compiler-generated field
        GarbageFacility garbageFacility = this.m_GarbageFacilityData[requestData.m_Facility];
        if ((requestData.m_Flags & GarbageTransferRequestFlags.Deliver) != (GarbageTransferRequestFlags) 0)
        {
          if ((double) garbageFacility.m_AcceptGarbagePriority <= 0.0)
            return false;
          if (garbageFacility.m_GarbageDeliverRequest != entity)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_GarbageTransferRequestData.HasComponent(garbageFacility.m_GarbageDeliverRequest))
              return false;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_DispatchActions.Enqueue(new GarbageTransferDispatchSystem.DispatchAction(entity, Entity.Null, requestData.m_Facility, Entity.Null));
          }
          return true;
        }
        if ((requestData.m_Flags & GarbageTransferRequestFlags.Receive) == (GarbageTransferRequestFlags) 0 || (double) garbageFacility.m_DeliverGarbagePriority <= 0.0)
          return false;
        if (garbageFacility.m_GarbageReceiveRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageTransferRequestData.HasComponent(garbageFacility.m_GarbageReceiveRequest))
            return false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_DispatchActions.Enqueue(new GarbageTransferDispatchSystem.DispatchAction(entity, Entity.Null, Entity.Null, requestData.m_Facility));
        }
        return true;
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
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_DispatchActions.Enqueue(new GarbageTransferDispatchSystem.DispatchAction(entity, pathInformation.m_Origin, Entity.Null, Entity.Null));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Dispatched>(jobIndex, entity, new Dispatched(pathInformation.m_Origin));
      }

      private void FindVehicleSource(
        int jobIndex,
        Entity requestEntity,
        GarbageTransferRequest requestData)
      {
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_IgnoredRules = RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget a = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.GarbageTransfer,
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_RoadTypes = RoadTypes.Car,
          m_Value = requestData.m_Amount,
          m_Value2 = requestData.m_Priority,
          m_Resource = Resource.Garbage,
          m_RandomCost = 30f
        };
        SetupQueueTarget b = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = requestData.m_Facility,
          m_RandomCost = 30f
        };
        if ((requestData.m_Flags & GarbageTransferRequestFlags.Receive) != (GarbageTransferRequestFlags) 0)
          a.m_Flags |= SetupTargetFlags.Import;
        if ((requestData.m_Flags & GarbageTransferRequestFlags.Deliver) != (GarbageTransferRequestFlags) 0)
          a.m_Flags |= SetupTargetFlags.Export;
        if ((requestData.m_Flags & GarbageTransferRequestFlags.RequireTransport) != (GarbageTransferRequestFlags) 0)
          a.m_Flags |= SetupTargetFlags.RequireTransport;
        else
          CommonUtils.Swap<SetupQueueTarget>(ref a, ref b);
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters, a, b));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, requestEntity);
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
    private struct DispatchActionJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<GarbageTransferRequest> m_RequestData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnectionData;
      public ComponentLookup<GarbageFacility> m_GarbageFacilityData;
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      public BufferLookup<TripNeeded> m_TripNeededs;
      public BufferLookup<Resources> m_Resources;
      public NativeQueue<GarbageTransferDispatchSystem.DispatchAction> m_DispatchActions;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        GarbageTransferDispatchSystem.DispatchAction dispatchAction;
        // ISSUE: reference to a compiler-generated field
        while (this.m_DispatchActions.TryDequeue(out dispatchAction))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_GarbageFacilityData.HasComponent(dispatchAction.m_DispatchSource) && !this.m_OutsideConnectionData.HasComponent(dispatchAction.m_DispatchSource))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ServiceDispatches.HasBuffer(dispatchAction.m_DispatchSource))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_ServiceDispatches[dispatchAction.m_DispatchSource].Add(new ServiceDispatch(dispatchAction.m_Request));
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_TripNeededs.HasBuffer(dispatchAction.m_DispatchSource))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              GarbageTransferRequest garbageTransferRequest = this.m_RequestData[dispatchAction.m_Request];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TripNeeded> tripNeeded = this.m_TripNeededs[dispatchAction.m_DispatchSource];
              TripNeeded elem = new TripNeeded();
              // ISSUE: reference to a compiler-generated field
              elem.m_TargetAgent = dispatchAction.m_Request;
              elem.m_Resource = Resource.Garbage;
              if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.RequireTransport) != (GarbageTransferRequestFlags) 0)
              {
                if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Deliver) != (GarbageTransferRequestFlags) 0)
                  elem.m_Purpose = Purpose.Exporting;
                if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Receive) != (GarbageTransferRequestFlags) 0)
                  elem.m_Purpose = Purpose.Collect;
              }
              else
              {
                if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Deliver) != (GarbageTransferRequestFlags) 0)
                  elem.m_Purpose = Purpose.Collect;
                if ((garbageTransferRequest.m_Flags & GarbageTransferRequestFlags.Receive) != (GarbageTransferRequestFlags) 0)
                  elem.m_Purpose = Purpose.Exporting;
              }
              elem.m_Data = garbageTransferRequest.m_Amount;
              if (elem.m_Purpose == Purpose.Exporting)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Resources> resource = this.m_Resources[dispatchAction.m_DispatchSource];
                int resources = EconomyUtils.GetResources(elem.m_Resource, resource);
                elem.m_Data = math.min(elem.m_Data, resources);
                if (elem.m_Data > 0)
                  EconomyUtils.AddResources(elem.m_Resource, -elem.m_Data, resource);
                else
                  continue;
              }
              else
              {
                elem.m_Purpose = Purpose.ReturnGarbage;
                elem.m_Resource = Resource.NoResource;
              }
              tripNeeded.Add(elem);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (dispatchAction.m_DeliverFacility != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            GarbageFacility garbageFacility = this.m_GarbageFacilityData[dispatchAction.m_DeliverFacility] with
            {
              m_GarbageDeliverRequest = dispatchAction.m_Request
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_GarbageFacilityData[dispatchAction.m_DeliverFacility] = garbageFacility;
          }
          // ISSUE: reference to a compiler-generated field
          if (dispatchAction.m_ReceiveFacility != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            GarbageFacility garbageFacility = this.m_GarbageFacilityData[dispatchAction.m_ReceiveFacility] with
            {
              m_GarbageReceiveRequest = dispatchAction.m_Request
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_GarbageFacilityData[dispatchAction.m_ReceiveFacility] = garbageFacility;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GarbageTransferRequest> __Game_Simulation_GarbageTransferRequest_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> __Game_Simulation_Dispatched_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<GarbageTransferRequest> __Game_Simulation_GarbageTransferRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GarbageFacility> __Game_Buildings_GarbageFacility_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TripNeeded> __Game_Citizens_TripNeeded_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;
      public ComponentLookup<GarbageFacility> __Game_Buildings_GarbageFacility_RW_ComponentLookup;
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferLookup;
      public BufferLookup<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferLookup;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageTransferRequest_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GarbageTransferRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_GarbageTransferRequest_RO_ComponentLookup = state.GetComponentLookup<GarbageTransferRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RO_ComponentLookup = state.GetComponentLookup<GarbageFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RO_BufferLookup = state.GetBufferLookup<TripNeeded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_GarbageFacility_RW_ComponentLookup = state.GetComponentLookup<GarbageFacility>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferLookup = state.GetBufferLookup<ServiceDispatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferLookup = state.GetBufferLookup<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
      }
    }
  }
}
