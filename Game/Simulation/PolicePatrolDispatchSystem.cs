// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PolicePatrolDispatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PolicePatrolDispatchSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_RequestQuery;
    private EntityQuery m_PoliceConfigurationQuery;
    private PolicePatrolDispatchSystem.TypeHandle __TypeHandle;

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
      this.m_RequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<PolicePatrolRequest>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.m_PoliceConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex >> 4 & 31U;
      uint num2 = (uint) ((int) num1 + 4 & 31);
      NativeQueue<PolicePatrolDispatchSystem.VehicleDispatch> nativeQueue = new NativeQueue<PolicePatrolDispatchSystem.VehicleDispatch>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_PoliceCar_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PoliceStation_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PolicePatrolDispatchSystem.PoliceDispatchJob jobData1 = new PolicePatrolDispatchSystem.PoliceDispatchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DispatchedType = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PolicePatrolRequestType = this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RW_ComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle,
        m_PolicePatrolRequestData = this.__TypeHandle.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup,
        m_HelicopterData = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_CrimeProducerData = this.__TypeHandle.__Game_Buildings_CrimeProducer_RW_ComponentLookup,
        m_PoliceStationData = this.__TypeHandle.__Game_Buildings_PoliceStation_RW_ComponentLookup,
        m_PoliceCarData = this.__TypeHandle.__Game_Vehicles_PoliceCar_RW_ComponentLookup,
        m_UpdateFrameIndex = num1,
        m_NextUpdateFrameIndex = num2,
        m_PoliceConfigurationData = this.m_PoliceConfigurationQuery.GetSingleton<PoliceConfigurationData>(),
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
      PolicePatrolDispatchSystem.DispatchVehiclesJob jobData2 = new PolicePatrolDispatchSystem.DispatchVehiclesJob()
      {
        m_VehicleDispatches = nativeQueue,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<PolicePatrolDispatchSystem.PoliceDispatchJob>(this.m_RequestQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<PolicePatrolDispatchSystem.DispatchVehiclesJob>(dependsOn);
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
    public PolicePatrolDispatchSystem()
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
    private struct PoliceDispatchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> m_DispatchedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<PolicePatrolRequest> m_PolicePatrolRequestType;
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> m_PolicePatrolRequestData;
      [ReadOnly]
      public ComponentLookup<Helicopter> m_HelicopterData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CrimeProducer> m_CrimeProducerData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Buildings.PoliceStation> m_PoliceStationData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.PoliceCar> m_PoliceCarData;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_NextUpdateFrameIndex;
      [ReadOnly]
      public PoliceConfigurationData m_PoliceConfigurationData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<PolicePatrolDispatchSystem.VehicleDispatch>.ParallelWriter m_VehicleDispatches;
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
          NativeArray<PolicePatrolRequest> nativeArray2 = chunk.GetNativeArray<PolicePatrolRequest>(ref this.m_PolicePatrolRequestType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ServiceRequest> nativeArray3 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            PolicePatrolRequest policePatrolRequest = nativeArray2[index2];
            ServiceRequest serviceRequest = nativeArray3[index2];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, policePatrolRequest.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleTarget(unfilteredChunkIndex, entity, policePatrolRequest.m_Target);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, policePatrolRequest.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleSource(unfilteredChunkIndex, entity, policePatrolRequest.m_Target);
              }
            }
            nativeArray2[index2] = policePatrolRequest;
            nativeArray3[index2] = serviceRequest;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((int) index1 != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Dispatched> nativeArray4 = chunk.GetNativeArray<Dispatched>(ref this.m_DispatchedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PolicePatrolRequest> nativeArray5 = chunk.GetNativeArray<PolicePatrolRequest>(ref this.m_PolicePatrolRequestType);
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
            PolicePatrolRequest policePatrolRequest = nativeArray5[index3];
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
              if (!this.ValidateTarget(entity, policePatrolRequest.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              // ISSUE: reference to a compiler-generated method
              this.ResetFailedRequest(unfilteredChunkIndex, entity, true, ref policePatrolRequest, ref serviceRequest);
            }
            nativeArray5[index3] = policePatrolRequest;
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
            PolicePatrolRequest policePatrolRequest = nativeArray5[index4];
            PathInformation pathInformation = nativeArray8[index4];
            ServiceRequest serviceRequest = nativeArray6[index4];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, policePatrolRequest.m_Target))
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
                this.ResetFailedRequest(unfilteredChunkIndex, entity, false, ref policePatrolRequest, ref serviceRequest);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, policePatrolRequest.m_Target))
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
                this.ResetFailedRequest(unfilteredChunkIndex, entity, false, ref policePatrolRequest, ref serviceRequest);
              }
            }
            nativeArray5[index4] = policePatrolRequest;
            nativeArray6[index4] = serviceRequest;
          }
        }
      }

      private bool ValidateReversed(Entity entity, Entity source)
      {
        Game.Buildings.PoliceStation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PoliceStationData.TryGetComponent(source, out componentData1))
        {
          if ((componentData1.m_Flags & (PoliceStationFlags.HasAvailablePatrolCars | PoliceStationFlags.HasAvailablePoliceHelicopters)) == (PoliceStationFlags) 0 || (componentData1.m_PurposeMask & PolicePurpose.Patrol) == (PolicePurpose) 0)
            return false;
          if (componentData1.m_TargetRequest != entity)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PolicePatrolRequestData.HasComponent(componentData1.m_TargetRequest))
              return false;
            componentData1.m_TargetRequest = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_PoliceStationData[source] = componentData1;
          }
          return true;
        }
        Game.Vehicles.PoliceCar componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PoliceCarData.TryGetComponent(source, out componentData2) || (componentData2.m_State & (PoliceCarFlags.ShiftEnded | PoliceCarFlags.Empty | PoliceCarFlags.EstimatedShiftEnd | PoliceCarFlags.Disabled)) != PoliceCarFlags.Empty || componentData2.m_RequestCount > 1 || (componentData2.m_PurposeMask & PolicePurpose.Patrol) == (PolicePurpose) 0 || this.m_ParkedCarData.HasComponent(source))
          return false;
        if (componentData2.m_TargetRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PolicePatrolRequestData.HasComponent(componentData2.m_TargetRequest))
            return false;
          componentData2.m_TargetRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_PoliceCarData[source] = componentData2;
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

      private bool ValidateTarget(Entity entity, Entity target)
      {
        CrimeProducer componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_CrimeProducerData.TryGetComponent(target, out componentData) || (double) componentData.m_Crime < (double) this.m_PoliceConfigurationData.m_CrimeAccumulationTolerance)
          return false;
        if (componentData.m_PatrolRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_PolicePatrolRequestData.HasComponent(componentData.m_PatrolRequest))
            return false;
          componentData.m_PatrolRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_CrimeProducerData[target] = componentData;
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
        PolicePatrolDispatchSystem.VehicleDispatch vehicleDispatch = new PolicePatrolDispatchSystem.VehicleDispatch(entity, pathInformation.m_Destination);
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
        ref PolicePatrolRequest policePatrolRequest,
        ref ServiceRequest serviceRequest)
      {
        SimulationUtils.ResetFailedRequest(ref serviceRequest);
        ++policePatrolRequest.m_DispatchIndex;
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
        if (this.m_ParkedCarData.HasComponent(entity1) && this.m_OwnerData.TryGetComponent(entity1, out componentData))
          entity1 = componentData.m_Owner;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PolicePatrolDispatchSystem.VehicleDispatch vehicleDispatch = new PolicePatrolDispatchSystem.VehicleDispatch(entity, entity1);
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleDispatches.Enqueue(vehicleDispatch);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Dispatched>(jobIndex, entity, new Dispatched(entity1));
      }

      private void FindVehicleSource(int jobIndex, Entity requestEntity, Entity targetEntity)
      {
        Entity district = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentDistrictData.HasComponent(targetEntity))
        {
          // ISSUE: reference to a compiler-generated field
          district = this.m_CurrentDistrictData[targetEntity].m_District;
        }
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 277.777771f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road | PathMethod.Flying,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.PolicePatrol;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.Flying;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car | RoadTypes.Helicopter;
        setupQueueTarget.m_FlyingTypes = RoadTypes.Helicopter;
        setupQueueTarget.m_Entity = district;
        setupQueueTarget.m_Value = 1;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road | PathMethod.Flying;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_FlyingTypes = RoadTypes.Helicopter;
        setupQueueTarget.m_Entity = targetEntity;
        SetupQueueTarget destination = setupQueueTarget;
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters, origin, destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, requestEntity);
      }

      private void FindVehicleTarget(int jobIndex, Entity requestEntity, Entity vehicleSource)
      {
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 277.777771f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road,
          m_Entity = vehicleSource
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.PoliceRequest
        };
        bool flag1 = false;
        bool flag2 = false;
        Game.Buildings.PoliceStation componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PoliceStationData.TryGetComponent(vehicleSource, out componentData1))
        {
          flag1 = (componentData1.m_Flags & PoliceStationFlags.HasAvailablePatrolCars) != 0;
          flag2 = (componentData1.m_Flags & PoliceStationFlags.HasAvailablePoliceHelicopters) != 0;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_HelicopterData.HasComponent(vehicleSource))
            flag2 = true;
          else
            flag1 = true;
        }
        if (flag1)
        {
          origin.m_RoadTypes |= RoadTypes.Car;
          destination.m_Methods |= PathMethod.Road;
          destination.m_RoadTypes |= RoadTypes.Car;
        }
        if (flag2)
        {
          parameters.m_Methods |= PathMethod.Flying;
          origin.m_Methods |= PathMethod.Flying;
          origin.m_RoadTypes |= RoadTypes.Helicopter;
          origin.m_FlyingTypes |= RoadTypes.Helicopter;
          destination.m_Methods |= PathMethod.Flying;
          destination.m_FlyingTypes |= RoadTypes.Helicopter;
        }
        Game.Vehicles.PoliceCar componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PoliceCarData.TryGetComponent(vehicleSource, out componentData2) && (componentData2.m_State & PoliceCarFlags.Returning) == (PoliceCarFlags) 0)
          origin.m_Flags |= SetupTargetFlags.PathEnd;
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
      public NativeQueue<PolicePatrolDispatchSystem.VehicleDispatch> m_VehicleDispatches;
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        PolicePatrolDispatchSystem.VehicleDispatch vehicleDispatch;
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
      public ComponentTypeHandle<Dispatched> __Game_Simulation_Dispatched_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<PolicePatrolRequest> __Game_Simulation_PolicePatrolRequest_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PolicePatrolRequest> __Game_Simulation_PolicePatrolRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      public ComponentLookup<CrimeProducer> __Game_Buildings_CrimeProducer_RW_ComponentLookup;
      public ComponentLookup<Game.Buildings.PoliceStation> __Game_Buildings_PoliceStation_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.PoliceCar> __Game_Vehicles_PoliceCar_RW_ComponentLookup;
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentLookup;
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PolicePatrolRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PolicePatrolRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_PolicePatrolRequest_RO_ComponentLookup = state.GetComponentLookup<PolicePatrolRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentLookup = state.GetComponentLookup<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CrimeProducer_RW_ComponentLookup = state.GetComponentLookup<CrimeProducer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PoliceStation_RW_ComponentLookup = state.GetComponentLookup<Game.Buildings.PoliceStation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_PoliceCar_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.PoliceCar>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentLookup = state.GetComponentLookup<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferLookup = state.GetBufferLookup<ServiceDispatch>();
      }
    }
  }
}
