// Decompiled with JetBrains decompiler
// Type: Game.Simulation.RandomTrafficDispatchSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
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
  public class RandomTrafficDispatchSystem : GameSystemBase
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private SimulationSystem m_SimulationSystem;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EntityQuery m_RequestQuery;
    private RandomTrafficDispatchSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return phase == SystemUpdatePhase.LoadSimulation ? 1 : 16;
    }

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
      this.m_RequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<RandomTrafficRequest>(), ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      uint num1;
      uint num2;
      int maxDelayFrames;
      // ISSUE: reference to a compiler-generated field
      if ((double) this.m_SimulationSystem.loadingProgress != 1.0)
      {
        // ISSUE: reference to a compiler-generated field
        num1 = this.m_SimulationSystem.frameIndex & 15U;
        num2 = num1;
        maxDelayFrames = 16;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        num1 = this.m_SimulationSystem.frameIndex >> 4 & 15U;
        num2 = (uint) ((int) num1 + 4 & 15);
        maxDelayFrames = 64;
      }
      NativeQueue<RandomTrafficDispatchSystem.VehicleDispatch> nativeQueue = new NativeQueue<RandomTrafficDispatchSystem.VehicleDispatch>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TrafficSpawner_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_RandomTrafficRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Simulation_RandomTrafficRequest_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RandomTrafficDispatchSystem.RandomTrafficDispatchJob jobData1 = new RandomTrafficDispatchSystem.RandomTrafficDispatchJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RandomTrafficRequestType = this.__TypeHandle.__Game_Simulation_RandomTrafficRequest_RO_ComponentTypeHandle,
        m_DispatchedType = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentTypeHandle,
        m_PathInformationType = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_ServiceRequestType = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle,
        m_RandomTrafficRequestData = this.__TypeHandle.__Game_Simulation_RandomTrafficRequest_RO_ComponentLookup,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RO_BufferLookup,
        m_TrafficSpawnerData = this.__TypeHandle.__Game_Buildings_TrafficSpawner_RW_ComponentLookup,
        m_UpdateFrameIndex = num1,
        m_NextUpdateFrameIndex = num2,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_VehicleDispatches = nativeQueue.AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, maxDelayFrames).AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RandomTrafficDispatchSystem.DispatchVehiclesJob jobData2 = new RandomTrafficDispatchSystem.DispatchVehiclesJob()
      {
        m_VehicleDispatches = nativeQueue,
        m_ServiceDispatches = this.__TypeHandle.__Game_Simulation_ServiceDispatch_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<RandomTrafficDispatchSystem.RandomTrafficDispatchJob>(this.m_RequestQuery, this.Dependency);
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.Schedule<RandomTrafficDispatchSystem.DispatchVehiclesJob>(dependsOn);
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
    public RandomTrafficDispatchSystem()
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
    private struct RandomTrafficDispatchJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<RandomTrafficRequest> m_RandomTrafficRequestType;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> m_DispatchedType;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> m_PathInformationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentLookup<RandomTrafficRequest> m_RandomTrafficRequestData;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<TrafficSpawner> m_TrafficSpawnerData;
      [ReadOnly]
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public uint m_NextUpdateFrameIndex;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<RandomTrafficDispatchSystem.VehicleDispatch>.ParallelWriter m_VehicleDispatches;
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
          NativeArray<RandomTrafficRequest> nativeArray2 = chunk.GetNativeArray<RandomTrafficRequest>(ref this.m_RandomTrafficRequestType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<ServiceRequest> nativeArray3 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
          // ISSUE: reference to a compiler-generated field
          Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            RandomTrafficRequest trafficRequest = nativeArray2[index2];
            ServiceRequest serviceRequest = nativeArray3[index2];
            // ISSUE: reference to a compiler-generated method
            if (!this.ValidateTarget(entity, trafficRequest.m_Target))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.DestroyEntity(unfilteredChunkIndex, entity);
            }
            else
            {
              if (SimulationUtils.TickServiceRequest(ref serviceRequest))
              {
                // ISSUE: reference to a compiler-generated method
                this.FindVehicleSource(unfilteredChunkIndex, ref random, entity, trafficRequest);
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
        NativeArray<RandomTrafficRequest> nativeArray5 = chunk.GetNativeArray<RandomTrafficRequest>(ref this.m_RandomTrafficRequestType);
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
            RandomTrafficRequest randomTrafficRequest = nativeArray5[index3];
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
              if (!this.ValidateTarget(entity, randomTrafficRequest.m_Target))
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
            RandomTrafficRequest randomTrafficRequest = nativeArray5[index4];
            PathInformation pathInformation = nativeArray8[index4];
            ServiceRequest serviceRequest = nativeArray6[index4];
            // ISSUE: reference to a compiler-generated method
            if (!this.ValidateTarget(entity, randomTrafficRequest.m_Target))
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
        TrafficSpawner componentData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TrafficSpawnerData.TryGetComponent(target, out componentData))
          return false;
        if (componentData.m_TrafficRequest != entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_RandomTrafficRequestData.HasComponent(componentData.m_TrafficRequest))
            return false;
          componentData.m_TrafficRequest = entity;
          // ISSUE: reference to a compiler-generated field
          this.m_TrafficSpawnerData[target] = componentData;
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RandomTrafficDispatchSystem.VehicleDispatch vehicleDispatch = new RandomTrafficDispatchSystem.VehicleDispatch(entity, pathInformation.m_Origin);
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleDispatches.Enqueue(vehicleDispatch);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Dispatched>(jobIndex, entity, new Dispatched(pathInformation.m_Origin));
      }

      private void FindVehicleSource(
        int jobIndex,
        ref Random random,
        Entity requestEntity,
        RandomTrafficRequest trafficRequest)
      {
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(1f, 1f, 1f, 1f),
          m_IgnoredRules = RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget a = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.RandomTraffic,
          m_RoadTypes = trafficRequest.m_RoadType,
          m_TrackTypes = trafficRequest.m_TrackType,
          m_Entity = trafficRequest.m_Target
        };
        SetupQueueTarget b = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_RoadTypes = trafficRequest.m_RoadType,
          m_TrackTypes = trafficRequest.m_TrackType,
          m_Entity = trafficRequest.m_Target
        };
        if ((trafficRequest.m_RoadType & RoadTypes.Car) != RoadTypes.None)
        {
          parameters.m_MaxSpeed = math.max(parameters.m_MaxSpeed, (float2) 111.111115f);
          parameters.m_Methods |= PathMethod.Road;
          a.m_Methods |= PathMethod.Road;
          b.m_Methods |= PathMethod.Road;
          if (trafficRequest.m_SizeClass == SizeClass.Large)
          {
            parameters.m_Methods |= PathMethod.CargoLoading;
            a.m_Methods |= PathMethod.CargoLoading;
            b.m_Methods |= PathMethod.CargoLoading;
          }
        }
        if ((trafficRequest.m_RoadType & RoadTypes.Airplane) != RoadTypes.None)
        {
          parameters.m_MaxSpeed = math.max(parameters.m_MaxSpeed, (float2) 277.777771f);
          parameters.m_Methods |= PathMethod.Road | PathMethod.Flying;
          a.m_Methods |= PathMethod.Road;
          b.m_Methods |= PathMethod.Road;
        }
        if ((trafficRequest.m_RoadType & RoadTypes.Watercraft) != RoadTypes.None)
        {
          parameters.m_MaxSpeed = math.max(parameters.m_MaxSpeed, (float2) 55.5555573f);
          parameters.m_Methods |= PathMethod.Road;
          a.m_Methods |= PathMethod.Road;
          b.m_Methods |= PathMethod.Road;
        }
        if ((trafficRequest.m_TrackType & TrackTypes.Train) != TrackTypes.None)
        {
          parameters.m_MaxSpeed = math.max(parameters.m_MaxSpeed, (float2) 138.888885f);
          parameters.m_Methods |= PathMethod.Track;
          a.m_Methods |= PathMethod.Track;
          b.m_Methods |= PathMethod.Track;
        }
        if (trafficRequest.m_SizeClass < SizeClass.Large)
          parameters.m_IgnoredRules |= RuleFlags.ForbidHeavyTraffic;
        if (trafficRequest.m_EnergyTypes == EnergyTypes.Electricity)
          parameters.m_IgnoredRules |= RuleFlags.ForbidCombustionEngines;
        if (random.NextBool())
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
    private struct DispatchVehiclesJob : IJob
    {
      public NativeQueue<RandomTrafficDispatchSystem.VehicleDispatch> m_VehicleDispatches;
      public BufferLookup<ServiceDispatch> m_ServiceDispatches;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        RandomTrafficDispatchSystem.VehicleDispatch vehicleDispatch;
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
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RandomTrafficRequest> __Game_Simulation_RandomTrafficRequest_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Dispatched> __Game_Simulation_Dispatched_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<RandomTrafficRequest> __Game_Simulation_RandomTrafficRequest_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RO_BufferLookup;
      public ComponentLookup<TrafficSpawner> __Game_Buildings_TrafficSpawner_RW_ComponentLookup;
      public BufferLookup<ServiceDispatch> __Game_Simulation_ServiceDispatch_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_RandomTrafficRequest_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RandomTrafficRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_RandomTrafficRequest_RO_ComponentLookup = state.GetComponentLookup<RandomTrafficRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RO_BufferLookup = state.GetBufferLookup<ServiceDispatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TrafficSpawner_RW_ComponentLookup = state.GetComponentLookup<TrafficSpawner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceDispatch_RW_BufferLookup = state.GetBufferLookup<ServiceDispatch>();
      }
    }
  }
}
