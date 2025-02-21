// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HealthcareDispatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
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
  public class HealthcareDispatchSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private EntityQuery m_RequestQuery;
    private HealthcareDispatchSystem.TypeHandle __TypeHandle;

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
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthcareRequest>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex / 16U & 15U;
      uint num2 = (uint) ((int) num1 + 4 & 15);
      NativeQueue<HealthcareDispatchSystem.VehicleDispatch> nativeQueue = new NativeQueue<HealthcareDispatchSystem.VehicleDispatch>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Hearse_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Ambulance_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_DeathcareFacility_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Hospital_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HealthcareDispatchSystem.HealthcareDispatchJob jobData1 = new HealthcareDispatchSystem.HealthcareDispatchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_HealthcareRequestType = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentTypeHandle,
        m_DispatchedType = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle,
        m_HealthcareRequestData = this.__TypeHandle.__Game_Simulation_HealthcareRequest_RO_ComponentLookup,
        m_HelicopterData = this.__TypeHandle.__Game_Vehicles_Helicopter_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_CurrentBuildingData = this.__TypeHandle.__Game_Citizens_CurrentBuilding_RO_ComponentLookup,
        m_CurrentTransportData = this.__TypeHandle.__Game_Citizens_CurrentTransport_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_DistrictData = this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_NeedsHealthcareData = this.__TypeHandle.__Game_Citizens_HealthProblem_RW_ComponentLookup,
        m_HospitalData = this.__TypeHandle.__Game_Buildings_Hospital_RW_ComponentLookup,
        m_DeathcareFacilityData = this.__TypeHandle.__Game_Buildings_DeathcareFacility_RW_ComponentLookup,
        m_AmbulanceData = this.__TypeHandle.__Game_Vehicles_Ambulance_RW_ComponentLookup,
        m_HearseData = this.__TypeHandle.__Game_Vehicles_Hearse_RW_ComponentLookup,
        m_UpdateFrameIndex = num1,
        m_NextUpdateFrameIndex = num2,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out JobHandle _),
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
      HealthcareDispatchSystem.DispatchVehiclesJob jobData2 = new HealthcareDispatchSystem.DispatchVehiclesJob()
      {
        m_VehicleDispatches = nativeQueue,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<HealthcareDispatchSystem.HealthcareDispatchJob>(this.m_RequestQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<HealthcareDispatchSystem.DispatchVehiclesJob>(dependsOn);
      nativeQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle);
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
    public HealthcareDispatchSystem()
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
    private struct HealthcareDispatchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<HealthcareRequest> m_HealthcareRequestType;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> m_DispatchedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> m_HealthcareRequestData;
      [ReadOnly]
      public ComponentLookup<Helicopter> m_HelicopterData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> m_CurrentBuildingData;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> m_CurrentTransportData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<HealthProblem> m_NeedsHealthcareData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Hospital> m_HospitalData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<DeathcareFacility> m_DeathcareFacilityData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Ambulance> m_AmbulanceData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Hearse> m_HearseData;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_NextUpdateFrameIndex;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<HealthcareDispatchSystem.VehicleDispatch>.ParallelWriter m_VehicleDispatches;
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
          NativeArray<HealthcareRequest> nativeArray2 = chunk.GetNativeArray<HealthcareRequest>(ref this.m_HealthcareRequestType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ServiceRequest> nativeArray3 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            HealthcareRequest healthcareRequest = nativeArray2[index2];
            ServiceRequest serviceRequest = nativeArray3[index2];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, healthcareRequest.m_Citizen, healthcareRequest.m_Type))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleTarget(unfilteredChunkIndex, entity, healthcareRequest.m_Citizen, healthcareRequest.m_Type);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, healthcareRequest.m_Citizen))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleSource(unfilteredChunkIndex, entity, healthcareRequest.m_Citizen, healthcareRequest.m_Type);
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
        NativeArray<HealthcareRequest> nativeArray5 = chunk.GetNativeArray<HealthcareRequest>(ref this.m_HealthcareRequestType);
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
            HealthcareRequest healthcareRequest = nativeArray5[index3];
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
              if (!this.ValidateTarget(entity, healthcareRequest.m_Citizen))
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
            HealthcareRequest healthcareRequest = nativeArray5[index4];
            PathInformation pathInformation = nativeArray8[index4];
            ServiceRequest serviceRequest = nativeArray6[index4];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, healthcareRequest.m_Citizen, healthcareRequest.m_Type))
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
              if (!this.ValidateTarget(entity, healthcareRequest.m_Citizen))
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

      private bool ValidateReversed(Entity entity, Entity source, HealthcareRequestType type)
      {
        switch (type)
        {
          case HealthcareRequestType.Ambulance:
            Hospital componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HospitalData.TryGetComponent(source, out componentData1))
            {
              if ((componentData1.m_Flags & (HospitalFlags.HasAvailableAmbulances | HospitalFlags.HasAvailableMedicalHelicopters)) == (HospitalFlags) 0)
                return false;
              if (componentData1.m_TargetRequest != entity)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_HealthcareRequestData.HasComponent(componentData1.m_TargetRequest))
                  return false;
                componentData1.m_TargetRequest = entity;
                // ISSUE: reference to a compiler-generated field
                this.m_HospitalData[source] = componentData1;
              }
              return true;
            }
            Ambulance componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_AmbulanceData.TryGetComponent(source, out componentData2) || (componentData2.m_State & (AmbulanceFlags.Returning | AmbulanceFlags.Dispatched | AmbulanceFlags.Transporting | AmbulanceFlags.Disabled)) != AmbulanceFlags.Returning || this.m_ParkedCarData.HasComponent(source))
              return false;
            if (componentData2.m_TargetRequest != entity)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_HealthcareRequestData.HasComponent(componentData2.m_TargetRequest))
                return false;
              componentData2.m_TargetRequest = entity;
              // ISSUE: reference to a compiler-generated field
              this.m_AmbulanceData[source] = componentData2;
            }
            return true;
          case HealthcareRequestType.Hearse:
            DeathcareFacility componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_DeathcareFacilityData.TryGetComponent(source, out componentData3))
            {
              if ((componentData3.m_Flags & (DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies)) != (DeathcareFacilityFlags.HasAvailableHearses | DeathcareFacilityFlags.HasRoomForBodies))
                return false;
              if (componentData3.m_TargetRequest != entity)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_HealthcareRequestData.HasComponent(componentData3.m_TargetRequest))
                  return false;
                componentData3.m_TargetRequest = entity;
                // ISSUE: reference to a compiler-generated field
                this.m_DeathcareFacilityData[source] = componentData3;
              }
              return true;
            }
            Hearse componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_HearseData.TryGetComponent(source, out componentData4) || (componentData4.m_State & (HearseFlags.Returning | HearseFlags.Dispatched | HearseFlags.Transporting | HearseFlags.Disabled)) != HearseFlags.Returning || this.m_ParkedCarData.HasComponent(source))
              return false;
            if (componentData4.m_TargetRequest != entity)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_HealthcareRequestData.HasComponent(componentData4.m_TargetRequest))
                return false;
              componentData4.m_TargetRequest = entity;
              // ISSUE: reference to a compiler-generated field
              this.m_HearseData[source] = componentData4;
            }
            return true;
          default:
            return false;
        }
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
        HealthProblem componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_NeedsHealthcareData.TryGetComponent(target, out componentData) || (componentData.m_Flags & HealthProblemFlags.RequireTransport) == HealthProblemFlags.None)
          return false;
        if (componentData.m_HealthcareRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_HealthcareRequestData.HasComponent(componentData.m_HealthcareRequest))
            return false;
          componentData.m_HealthcareRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_NeedsHealthcareData[target] = componentData;
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
        HealthcareDispatchSystem.VehicleDispatch vehicleDispatch = new HealthcareDispatchSystem.VehicleDispatch(entity, pathInformation.m_Destination);
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
        if (this.m_ParkedCarData.HasComponent(entity1) && this.m_OwnerData.TryGetComponent(entity1, out componentData))
          entity1 = componentData.m_Owner;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        HealthcareDispatchSystem.VehicleDispatch vehicleDispatch = new HealthcareDispatchSystem.VehicleDispatch(entity, entity1);
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleDispatches.Enqueue(vehicleDispatch);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Dispatched>(jobIndex, entity, new Dispatched(entity1));
      }

      private void FindVehicleSource(
        int jobIndex,
        Entity requestEntity,
        Entity target,
        HealthcareRequestType type)
      {
        Entity entity = Entity.Null;
        CurrentBuilding componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentBuildingData.TryGetComponent(target, out componentData1))
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentDistrictData.HasComponent(componentData1.m_CurrentBuilding))
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_CurrentDistrictData[componentData1.m_CurrentBuilding].m_District;
          }
        }
        else
        {
          CurrentTransport componentData2;
          Transform componentData3;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurrentTransportData.TryGetComponent(target, out componentData2) && this.m_TransformData.TryGetComponent(componentData2.m_CurrentTransport, out componentData3))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            HealthcareDispatchSystem.HealthcareDispatchJob.DistrictIterator iterator = new HealthcareDispatchSystem.HealthcareDispatchJob.DistrictIterator()
            {
              m_Position = componentData3.m_Position.xz,
              m_DistrictData = this.m_DistrictData,
              m_Nodes = this.m_Nodes,
              m_Triangles = this.m_Triangles
            };
            // ISSUE: reference to a compiler-generated field
            this.m_AreaTree.Iterate<HealthcareDispatchSystem.HealthcareDispatchJob.DistrictIterator>(ref iterator);
            // ISSUE: reference to a compiler-generated field
            entity = iterator.m_Result;
          }
        }
        switch (type)
        {
          case HealthcareRequestType.Ambulance:
            PathfindParameters parameters1 = new PathfindParameters()
            {
              m_MaxSpeed = (float2) 277.777771f,
              m_WalkSpeed = (float2) 1.66666675f,
              m_Weights = new PathfindWeights(1f, 0.0f, 0.0f, 0.0f),
              m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Flying | PathMethod.Boarding,
              m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic,
              m_ParkingTarget = requestEntity
            };
            SetupQueueTarget setupQueueTarget1 = new SetupQueueTarget();
            setupQueueTarget1.m_Type = SetupTargetType.Ambulance;
            setupQueueTarget1.m_Methods = PathMethod.Road | PathMethod.Flying | PathMethod.Boarding;
            setupQueueTarget1.m_RoadTypes = RoadTypes.Car | RoadTypes.Helicopter;
            setupQueueTarget1.m_FlyingTypes = RoadTypes.Helicopter;
            setupQueueTarget1.m_Entity = entity;
            SetupQueueTarget origin1 = setupQueueTarget1;
            setupQueueTarget1 = new SetupQueueTarget();
            setupQueueTarget1.m_Type = SetupTargetType.CurrentLocation;
            setupQueueTarget1.m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Flying | PathMethod.Boarding;
            setupQueueTarget1.m_RoadTypes = RoadTypes.Car;
            setupQueueTarget1.m_FlyingTypes = RoadTypes.Helicopter;
            setupQueueTarget1.m_Entity = target;
            SetupQueueTarget destination1 = setupQueueTarget1;
            // ISSUE: reference to a compiler-generated field
            this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters1, origin1, destination1));
            break;
          case HealthcareRequestType.Hearse:
            PathfindParameters parameters2 = new PathfindParameters()
            {
              m_MaxSpeed = (float2) 111.111115f,
              m_WalkSpeed = (float2) 1.66666675f,
              m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
              m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Boarding,
              m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidSlowTraffic,
              m_ParkingTarget = requestEntity
            };
            SetupQueueTarget setupQueueTarget2 = new SetupQueueTarget();
            setupQueueTarget2.m_Type = SetupTargetType.Hearse;
            setupQueueTarget2.m_Methods = PathMethod.Road | PathMethod.Boarding;
            setupQueueTarget2.m_RoadTypes = RoadTypes.Car;
            setupQueueTarget2.m_Entity = entity;
            SetupQueueTarget origin2 = setupQueueTarget2;
            setupQueueTarget2 = new SetupQueueTarget();
            setupQueueTarget2.m_Type = SetupTargetType.CurrentLocation;
            setupQueueTarget2.m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Boarding;
            setupQueueTarget2.m_RoadTypes = RoadTypes.Car;
            setupQueueTarget2.m_Entity = target;
            SetupQueueTarget destination2 = setupQueueTarget2;
            // ISSUE: reference to a compiler-generated field
            this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters2, origin2, destination2));
            break;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, requestEntity);
      }

      private void FindVehicleTarget(
        int jobIndex,
        Entity requestEntity,
        Entity vehicleSource,
        HealthcareRequestType type)
      {
        switch (type)
        {
          case HealthcareRequestType.Ambulance:
            PathfindParameters parameters1 = new PathfindParameters()
            {
              m_MaxSpeed = (float2) 277.777771f,
              m_WalkSpeed = (float2) 1.66666675f,
              m_Weights = new PathfindWeights(1f, 0.0f, 0.0f, 0.0f),
              m_Methods = PathMethod.Road,
              m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidPrivateTraffic | RuleFlags.ForbidSlowTraffic,
              m_ParkingTarget = requestEntity
            };
            SetupQueueTarget setupQueueTarget1 = new SetupQueueTarget();
            setupQueueTarget1.m_Type = SetupTargetType.CurrentLocation;
            setupQueueTarget1.m_Methods = PathMethod.Road;
            setupQueueTarget1.m_Entity = vehicleSource;
            SetupQueueTarget origin1 = setupQueueTarget1;
            setupQueueTarget1 = new SetupQueueTarget();
            setupQueueTarget1.m_Type = SetupTargetType.HealthcareRequest;
            SetupQueueTarget destination1 = setupQueueTarget1;
            bool flag1 = false;
            bool flag2 = false;
            Hospital componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HospitalData.TryGetComponent(vehicleSource, out componentData))
            {
              flag1 = (componentData.m_Flags & HospitalFlags.HasAvailableAmbulances) != 0;
              flag2 = (componentData.m_Flags & HospitalFlags.HasAvailableMedicalHelicopters) != 0;
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
              parameters1.m_Methods |= PathMethod.Pedestrian | PathMethod.Boarding;
              origin1.m_Methods |= PathMethod.Boarding;
              origin1.m_RoadTypes |= RoadTypes.Car;
              destination1.m_Methods |= PathMethod.Pedestrian | PathMethod.Road | PathMethod.Boarding;
              destination1.m_RoadTypes |= RoadTypes.Car;
            }
            if (flag2)
            {
              parameters1.m_Methods |= PathMethod.Flying;
              origin1.m_Methods |= PathMethod.Flying;
              origin1.m_RoadTypes |= RoadTypes.Helicopter;
              origin1.m_FlyingTypes |= RoadTypes.Helicopter;
              destination1.m_Methods |= PathMethod.Flying;
              destination1.m_FlyingTypes |= RoadTypes.Helicopter;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters1, origin1, destination1));
            break;
          case HealthcareRequestType.Hearse:
            PathfindParameters parameters2 = new PathfindParameters()
            {
              m_MaxSpeed = (float2) 111.111115f,
              m_WalkSpeed = (float2) 1.66666675f,
              m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
              m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Boarding,
              m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidSlowTraffic,
              m_ParkingTarget = requestEntity
            };
            SetupQueueTarget setupQueueTarget2 = new SetupQueueTarget();
            setupQueueTarget2.m_Type = SetupTargetType.CurrentLocation;
            setupQueueTarget2.m_Methods = PathMethod.Road | PathMethod.Boarding;
            setupQueueTarget2.m_RoadTypes = RoadTypes.Car;
            setupQueueTarget2.m_Entity = vehicleSource;
            SetupQueueTarget origin2 = setupQueueTarget2;
            setupQueueTarget2 = new SetupQueueTarget();
            setupQueueTarget2.m_Type = SetupTargetType.HealthcareRequest;
            setupQueueTarget2.m_Methods = PathMethod.Pedestrian | PathMethod.Road | PathMethod.Boarding;
            setupQueueTarget2.m_RoadTypes = RoadTypes.Car;
            SetupQueueTarget destination2 = setupQueueTarget2;
            // ISSUE: reference to a compiler-generated field
            this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters2, origin2, destination2));
            break;
        }
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
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Position) || !this.m_DistrictData.HasComponent(areaItem.m_Area))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> node = this.m_Nodes[areaItem.m_Area];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Triangle> triangle = this.m_Triangles[areaItem.m_Area];
          // ISSUE: reference to a compiler-generated field
          if (triangle.Length <= areaItem.m_Triangle || !MathUtils.Intersect(AreaUtils.GetTriangle2(node, triangle[areaItem.m_Triangle]), this.m_Position, out float2 _))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Result = areaItem.m_Area;
        }
      }
    }

    [BurstCompile]
    private struct DispatchVehiclesJob : IJob
    {
      public NativeQueue<HealthcareDispatchSystem.VehicleDispatch> m_VehicleDispatches;
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        HealthcareDispatchSystem.VehicleDispatch vehicleDispatch;
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
      public ComponentTypeHandle<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> __Game_Simulation_Dispatched_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<HealthcareRequest> __Game_Simulation_HealthcareRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Helicopter> __Game_Vehicles_Helicopter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentBuilding> __Game_Citizens_CurrentBuilding_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentTransport> __Game_Citizens_CurrentTransport_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<District> __Game_Areas_District_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      public ComponentLookup<HealthProblem> __Game_Citizens_HealthProblem_RW_ComponentLookup;
      public ComponentLookup<Hospital> __Game_Buildings_Hospital_RW_ComponentLookup;
      public ComponentLookup<DeathcareFacility> __Game_Buildings_DeathcareFacility_RW_ComponentLookup;
      public ComponentLookup<Ambulance> __Game_Vehicles_Ambulance_RW_ComponentLookup;
      public ComponentLookup<Hearse> __Game_Vehicles_Hearse_RW_ComponentLookup;
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentLookup;
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HealthcareRequest_RO_ComponentLookup = state.GetComponentLookup<HealthcareRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Helicopter_RO_ComponentLookup = state.GetComponentLookup<Helicopter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentBuilding_RO_ComponentLookup = state.GetComponentLookup<CurrentBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_CurrentTransport_RO_ComponentLookup = state.GetComponentLookup<CurrentTransport>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RO_ComponentLookup = state.GetComponentLookup<District>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RW_ComponentLookup = state.GetComponentLookup<HealthProblem>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Hospital_RW_ComponentLookup = state.GetComponentLookup<Hospital>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_DeathcareFacility_RW_ComponentLookup = state.GetComponentLookup<DeathcareFacility>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Ambulance_RW_ComponentLookup = state.GetComponentLookup<Ambulance>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Hearse_RW_ComponentLookup = state.GetComponentLookup<Hearse>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentLookup = state.GetComponentLookup<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferLookup = state.GetBufferLookup<ServiceDispatch>();
      }
    }
  }
}
