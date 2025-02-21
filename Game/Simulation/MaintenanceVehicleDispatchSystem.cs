// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MaintenanceVehicleDispatchSystem
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
  public class MaintenanceVehicleDispatchSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private EntityQuery m_RequestQuery;
    private MaintenanceVehicleDispatchSystem.TypeHandle __TypeHandle;

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
      this.m_RequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<MaintenanceRequest>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex >> 4 & 31U;
      uint num2 = (uint) ((int) num1 + 4 & 31);
      NativeQueue<MaintenanceVehicleDispatchSystem.VehicleDispatch> nativeQueue = new NativeQueue<MaintenanceVehicleDispatchSystem.VehicleDispatch>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_MaintenanceDepot_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle dependencies;
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
      MaintenanceVehicleDispatchSystem.MaintenanceVehicleDispatchJob jobData1 = new MaintenanceVehicleDispatchSystem.MaintenanceVehicleDispatchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DispatchedType = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_MaintenanceRequestType = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RW_ComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ParkData = this.__TypeHandle.__Game_Buildings_Park_RO_ComponentLookup,
        m_NetConditionData = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_VehicleData = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_SurfaceData = this.__TypeHandle.__Game_Objects_Surface_RO_ComponentLookup,
        m_CurrentDistrictData = this.__TypeHandle.__Game_Areas_CurrentDistrict_RO_ComponentLookup,
        m_BorderDistrictData = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentLookup,
        m_DistrictData = this.__TypeHandle.__Game_Areas_District_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabParkData = this.__TypeHandle.__Game_Prefabs_ParkData_RO_ComponentLookup,
        m_PrefabMaintenanceDepotData = this.__TypeHandle.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup,
        m_PrefabMaintenanceVehicleData = this.__TypeHandle.__Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup,
        m_Nodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_MaintenanceConsumerData = this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RW_ComponentLookup,
        m_MaintenanceDepotData = this.__TypeHandle.__Game_Buildings_MaintenanceDepot_RW_ComponentLookup,
        m_MaintenanceVehicleData = this.__TypeHandle.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup,
        m_UpdateFrameIndex = num1,
        m_NextUpdateFrameIndex = num2,
        m_AreaTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies),
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
      MaintenanceVehicleDispatchSystem.DispatchVehiclesJob jobData2 = new MaintenanceVehicleDispatchSystem.DispatchVehiclesJob()
      {
        m_VehicleDispatches = nativeQueue,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<MaintenanceVehicleDispatchSystem.MaintenanceVehicleDispatchJob>(this.m_RequestQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<MaintenanceVehicleDispatchSystem.DispatchVehiclesJob>(dependsOn);
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
    public MaintenanceVehicleDispatchSystem()
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
    private struct MaintenanceVehicleDispatchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> m_DispatchedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<MaintenanceRequest> m_MaintenanceRequestType;
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> m_ParkData;
      [ReadOnly]
      public ComponentLookup<NetCondition> m_NetConditionData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Vehicle> m_VehicleData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Surface> m_SurfaceData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> m_BorderDistrictData;
      [ReadOnly]
      public ComponentLookup<District> m_DistrictData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ParkData> m_PrefabParkData;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> m_PrefabMaintenanceDepotData;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicleData> m_PrefabMaintenanceVehicleData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_Nodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<MaintenanceConsumer> m_MaintenanceConsumerData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Buildings.MaintenanceDepot> m_MaintenanceDepotData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> m_MaintenanceVehicleData;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_NextUpdateFrameIndex;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<MaintenanceVehicleDispatchSystem.VehicleDispatch>.ParallelWriter m_VehicleDispatches;
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
          NativeArray<MaintenanceRequest> nativeArray2 = chunk.GetNativeArray<MaintenanceRequest>(ref this.m_MaintenanceRequestType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ServiceRequest> nativeArray3 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            MaintenanceRequest maintenanceRequest = nativeArray2[index2];
            ServiceRequest serviceRequest = nativeArray3[index2];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, maintenanceRequest.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleTarget(unfilteredChunkIndex, entity, maintenanceRequest.m_Target);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, maintenanceRequest.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleSource(unfilteredChunkIndex, entity, maintenanceRequest.m_Target);
              }
            }
            nativeArray2[index2] = maintenanceRequest;
            nativeArray3[index2] = serviceRequest;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if ((int) index1 != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Dispatched> nativeArray4 = chunk.GetNativeArray<Dispatched>(ref this.m_DispatchedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MaintenanceRequest> nativeArray5 = chunk.GetNativeArray<MaintenanceRequest>(ref this.m_MaintenanceRequestType);
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
            MaintenanceRequest maintenanceRequest = nativeArray5[index3];
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
              if (!this.ValidateTarget(entity, maintenanceRequest.m_Target))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
                continue;
              }
              // ISSUE: reference to a compiler-generated method
              this.ResetFailedRequest(unfilteredChunkIndex, entity, true, ref maintenanceRequest, ref serviceRequest);
            }
            nativeArray5[index3] = maintenanceRequest;
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
            MaintenanceRequest maintenanceRequest = nativeArray5[index4];
            PathInformation pathInformation = nativeArray8[index4];
            ServiceRequest serviceRequest = nativeArray6[index4];
            if ((serviceRequest.m_Flags & ServiceRequestFlags.Reversed) != (ServiceRequestFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateReversed(entity, maintenanceRequest.m_Target))
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
                this.ResetFailedRequest(unfilteredChunkIndex, entity, false, ref maintenanceRequest, ref serviceRequest);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              if (!this.ValidateTarget(entity, maintenanceRequest.m_Target))
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
                this.ResetFailedRequest(unfilteredChunkIndex, entity, false, ref maintenanceRequest, ref serviceRequest);
              }
            }
            nativeArray5[index4] = maintenanceRequest;
            nativeArray6[index4] = serviceRequest;
          }
        }
      }

      private bool ValidateReversed(Entity entity, Entity source)
      {
        Game.Buildings.MaintenanceDepot componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceDepotData.TryGetComponent(source, out componentData1))
        {
          if ((componentData1.m_Flags & MaintenanceDepotFlags.HasAvailableVehicles) == (MaintenanceDepotFlags) 0)
            return false;
          if (componentData1.m_TargetRequest != entity)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_MaintenanceRequestData.HasComponent(componentData1.m_TargetRequest))
              return false;
            componentData1.m_TargetRequest = entity;
            // ISSUE: reference to a compiler-generated field
            this.m_MaintenanceDepotData[source] = componentData1;
          }
          return true;
        }
        Game.Vehicles.MaintenanceVehicle componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MaintenanceVehicleData.TryGetComponent(source, out componentData2) || (componentData2.m_State & (MaintenanceVehicleFlags.EstimatedFull | MaintenanceVehicleFlags.Disabled)) != (MaintenanceVehicleFlags) 0 || componentData2.m_RequestCount > 1 || this.m_ParkedCarData.HasComponent(source))
          return false;
        if (componentData2.m_TargetRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceRequestData.HasComponent(componentData2.m_TargetRequest))
            return false;
          componentData2.m_TargetRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_MaintenanceVehicleData[source] = componentData2;
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
        MaintenanceConsumer componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_MaintenanceConsumerData.TryGetComponent(target, out componentData))
          return false;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ParkData.HasComponent(target))
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[target];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (ParkAISystem.GetMaintenancePriority(this.m_ParkData[target], this.m_PrefabParkData[prefabRef.m_Prefab]) <= 0)
            return false;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_NetConditionData.HasComponent(target) && NetDeteriorationSystem.GetMaintenancePriority(this.m_NetConditionData[target]) <= 0)
            return false;
        }
        if (componentData.m_Request != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_MaintenanceRequestData.HasComponent(componentData.m_Request))
            return false;
          componentData.m_Request = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_MaintenanceConsumerData[target] = componentData;
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
        MaintenanceVehicleDispatchSystem.VehicleDispatch vehicleDispatch = new MaintenanceVehicleDispatchSystem.VehicleDispatch(entity, pathInformation.m_Destination);
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
        ref MaintenanceRequest maintenanceRequest,
        ref ServiceRequest serviceRequest)
      {
        SimulationUtils.ResetFailedRequest(ref serviceRequest);
        ++maintenanceRequest.m_DispatchIndex;
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
        MaintenanceVehicleDispatchSystem.VehicleDispatch vehicleDispatch = new MaintenanceVehicleDispatchSystem.VehicleDispatch(entity, entity1);
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleDispatches.Enqueue(vehicleDispatch);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Dispatched>(jobIndex, entity, new Dispatched(entity1));
      }

      private void FindVehicleSource(int jobIndex, Entity requestEntity, Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MaintenanceType maintenanceType = BuildingUtils.GetMaintenanceType(target, ref this.m_ParkData, ref this.m_NetConditionData, ref this.m_EdgeData, ref this.m_SurfaceData, ref this.m_VehicleData);
        Entity entity = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurrentDistrictData.HasComponent(target))
        {
          // ISSUE: reference to a compiler-generated field
          entity = this.m_CurrentDistrictData[target].m_District;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_BorderDistrictData.HasComponent(target))
          {
            // ISSUE: reference to a compiler-generated field
            entity = this.m_BorderDistrictData[target].m_Right;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformData.HasComponent(target))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              MaintenanceVehicleDispatchSystem.MaintenanceVehicleDispatchJob.DistrictIterator iterator = new MaintenanceVehicleDispatchSystem.MaintenanceVehicleDispatchJob.DistrictIterator()
              {
                m_Position = this.m_TransformData[target].m_Position.xz,
                m_DistrictData = this.m_DistrictData,
                m_Nodes = this.m_Nodes,
                m_Triangles = this.m_Triangles
              };
              // ISSUE: reference to a compiler-generated field
              this.m_AreaTree.Iterate<MaintenanceVehicleDispatchSystem.MaintenanceVehicleDispatchJob.DistrictIterator>(ref iterator);
              // ISSUE: reference to a compiler-generated field
              entity = iterator.m_Result;
            }
          }
        }
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.Maintenance,
          m_Methods = PathMethod.Road,
          m_RoadTypes = RoadTypes.Car,
          m_MaintenanceType = maintenanceType,
          m_Entity = entity
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road,
          m_RoadTypes = RoadTypes.Car,
          m_Entity = target
        };
        if ((maintenanceType & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None)
          parameters.m_IgnoredRules |= RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidPrivateTraffic;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.HasComponent(target))
          destination.m_Value2 = 30f;
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(requestEntity, parameters, origin, destination));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(jobIndex, requestEntity, new PathInformation());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(jobIndex, requestEntity);
      }

      private void FindVehicleTarget(int jobIndex, Entity requestEntity, Entity vehicleSource)
      {
        MaintenanceType maintenanceType = MaintenanceType.None;
        PrefabRef componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.TryGetComponent(vehicleSource, out componentData1))
        {
          MaintenanceDepotData componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabMaintenanceDepotData.TryGetComponent(componentData1.m_Prefab, out componentData2))
          {
            maintenanceType = componentData2.m_MaintenanceType;
          }
          else
          {
            MaintenanceVehicleData componentData3;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabMaintenanceVehicleData.TryGetComponent(componentData1.m_Prefab, out componentData3))
              maintenanceType = componentData3.m_MaintenanceType;
          }
        }
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_Methods = PathMethod.Road,
          m_IgnoredRules = RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.CurrentLocation;
        setupQueueTarget.m_Methods = PathMethod.Road;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_Entity = vehicleSource;
        SetupQueueTarget origin = setupQueueTarget;
        setupQueueTarget = new SetupQueueTarget();
        setupQueueTarget.m_Type = SetupTargetType.MaintenanceRequest;
        setupQueueTarget.m_Methods = PathMethod.Road;
        setupQueueTarget.m_RoadTypes = RoadTypes.Car;
        setupQueueTarget.m_MaintenanceType = maintenanceType;
        SetupQueueTarget destination = setupQueueTarget;
        if ((maintenanceType & (MaintenanceType.Road | MaintenanceType.Snow | MaintenanceType.Vehicle)) != MaintenanceType.None)
          parameters.m_IgnoredRules |= RuleFlags.ForbidTransitTraffic | RuleFlags.ForbidPrivateTraffic;
        Game.Vehicles.MaintenanceVehicle componentData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceVehicleData.TryGetComponent(vehicleSource, out componentData4) && (componentData4.m_State & MaintenanceVehicleFlags.Returning) == (MaintenanceVehicleFlags) 0)
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
      public NativeQueue<MaintenanceVehicleDispatchSystem.VehicleDispatch> m_VehicleDispatches;
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        MaintenanceVehicleDispatchSystem.VehicleDispatch vehicleDispatch;
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
      public ComponentTypeHandle<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Park> __Game_Buildings_Park_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCondition> __Game_Net_NetCondition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Surface> __Game_Objects_Surface_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> __Game_Areas_CurrentDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<District> __Game_Areas_District_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkData> __Game_Prefabs_ParkData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceDepotData> __Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceVehicleData> __Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      public ComponentLookup<MaintenanceConsumer> __Game_Simulation_MaintenanceConsumer_RW_ComponentLookup;
      public ComponentLookup<Game.Buildings.MaintenanceDepot> __Game_Buildings_MaintenanceDepot_RW_ComponentLookup;
      public ComponentLookup<Game.Vehicles.MaintenanceVehicle> __Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup;
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
        this.__Game_Simulation_MaintenanceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MaintenanceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Park_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Park>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentLookup = state.GetComponentLookup<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentLookup = state.GetComponentLookup<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Surface>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_CurrentDistrict_RO_ComponentLookup = state.GetComponentLookup<CurrentDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentLookup = state.GetComponentLookup<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RO_ComponentLookup = state.GetComponentLookup<District>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ParkData_RO_ComponentLookup = state.GetComponentLookup<ParkData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceDepotData_RO_ComponentLookup = state.GetComponentLookup<MaintenanceDepotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MaintenanceVehicleData_RO_ComponentLookup = state.GetComponentLookup<MaintenanceVehicleData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceConsumer_RW_ComponentLookup = state.GetComponentLookup<MaintenanceConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_MaintenanceDepot_RW_ComponentLookup = state.GetComponentLookup<Game.Buildings.MaintenanceDepot>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_MaintenanceVehicle_RW_ComponentLookup = state.GetComponentLookup<Game.Vehicles.MaintenanceVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentLookup = state.GetComponentLookup<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferLookup = state.GetBufferLookup<ServiceDispatch>();
      }
    }
  }
}
