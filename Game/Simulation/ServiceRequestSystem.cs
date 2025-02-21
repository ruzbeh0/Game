// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ServiceRequestSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
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
  public class ServiceRequestSystem : GameSystemBase
  {
    private ModificationEndBarrier m_ModificationBarrier;
    private EntityQuery m_RequestGroupQuery;
    private EntityQuery m_HandleRequestQuery;
    private ServiceRequestSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_RequestGroupQuery = this.GetEntityQuery(ComponentType.ReadOnly<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.m_HandleRequestQuery = this.GetEntityQuery(ComponentType.ReadOnly<HandleRequest>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_RequestGroupQuery, this.m_HandleRequestQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RequestGroupQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_RequestGroup_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        JobHandle producerJob = new ServiceRequestSystem.UpdateRequestGroupJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_RequestGroupType = this.__TypeHandle.__Game_Simulation_RequestGroup_RO_ComponentTypeHandle,
          m_RandomSeed = RandomSeed.Next(),
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
        }.ScheduleParallel<ServiceRequestSystem.UpdateRequestGroupJob>(this.m_RequestGroupQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(producerJob);
        this.Dependency = producerJob;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_HandleRequestQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_HandleRequest_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle producerJob1 = new ServiceRequestSystem.HandleRequestJob()
      {
        m_HandleRequestType = this.__TypeHandle.__Game_Simulation_HandleRequest_RO_ComponentTypeHandle,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RW_ComponentLookup,
        m_ServiceRequestData = this.__TypeHandle.__Game_Simulation_ServiceRequest_RW_ComponentLookup,
        m_Chunks = this.m_HandleRequestQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<ServiceRequestSystem.HandleRequestJob>(JobHandle.CombineDependencies(outJobHandle, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(producerJob1);
      this.Dependency = producerJob1;
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
    public ServiceRequestSystem()
    {
    }

    [BurstCompile]
    private struct UpdateRequestGroupJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<RequestGroup> m_RequestGroupType;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RequestGroup> nativeArray2 = chunk.GetNativeArray<RequestGroup>(ref this.m_RequestGroupType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity e = nativeArray1[index1];
          RequestGroup requestGroup = nativeArray2[index1];
          uint index2 = random.NextUInt(requestGroup.m_GroupCount);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<RequestGroup>(unfilteredChunkIndex, e);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddSharedComponent<UpdateFrame>(unfilteredChunkIndex, e, new UpdateFrame(index2));
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
    private struct HandleRequestJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<HandleRequest> m_HandleRequestType;
      public ComponentLookup<Dispatched> m_DispatchedData;
      public ComponentLookup<ServiceRequest> m_ServiceRequestData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int capacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index];
          capacity += chunk.Count;
        }
        NativeParallelHashMap<Entity, HandleRequest> nativeParallelHashMap = new NativeParallelHashMap<Entity, HandleRequest>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<HandleRequest> nativeArray = this.m_Chunks[index1].GetNativeArray<HandleRequest>(ref this.m_HandleRequestType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            HandleRequest handleRequest1 = nativeArray[index2];
            HandleRequest handleRequest2;
            if (nativeParallelHashMap.TryGetValue(handleRequest1.m_Request, out handleRequest2))
            {
              if (handleRequest1.m_Completed)
                nativeParallelHashMap[handleRequest1.m_Request] = handleRequest1;
              else if (handleRequest1.m_PathConsumed)
              {
                handleRequest2.m_PathConsumed = true;
                nativeParallelHashMap[handleRequest1.m_Request] = handleRequest2;
              }
            }
            else
              nativeParallelHashMap.Add(handleRequest1.m_Request, handleRequest1);
          }
        }
        NativeParallelHashMap<Entity, HandleRequest>.Enumerator enumerator = nativeParallelHashMap.GetEnumerator();
        while (enumerator.MoveNext())
        {
          HandleRequest handleRequest = enumerator.Current.Value;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceRequestData.HasComponent(handleRequest.m_Request))
          {
            if (handleRequest.m_Completed)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.DestroyEntity(handleRequest.m_Request);
            }
            else if (handleRequest.m_Handler != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DispatchedData.HasComponent(handleRequest.m_Request))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_DispatchedData[handleRequest.m_Request] = new Dispatched(handleRequest.m_Handler);
                // ISSUE: reference to a compiler-generated field
                ServiceRequest serviceRequest = this.m_ServiceRequestData[handleRequest.m_Request] with
                {
                  m_Cooldown = 0
                };
                // ISSUE: reference to a compiler-generated field
                this.m_ServiceRequestData[handleRequest.m_Request] = serviceRequest;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Dispatched>(handleRequest.m_Request, new Dispatched(handleRequest.m_Handler));
              }
              if (handleRequest.m_PathConsumed)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<PathInformation>(handleRequest.m_Request);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<PathElement>(handleRequest.m_Request);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_DispatchedData.HasComponent(handleRequest.m_Request))
              {
                // ISSUE: reference to a compiler-generated field
                ServiceRequest serviceRequest = this.m_ServiceRequestData[handleRequest.m_Request];
                SimulationUtils.ResetFailedRequest(ref serviceRequest);
                // ISSUE: reference to a compiler-generated field
                this.m_ServiceRequestData[handleRequest.m_Request] = serviceRequest;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<PathInformation>(handleRequest.m_Request);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<PathElement>(handleRequest.m_Request);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Dispatched>(handleRequest.m_Request);
              }
            }
          }
        }
        enumerator.Dispose();
        nativeParallelHashMap.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RequestGroup> __Game_Simulation_RequestGroup_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HandleRequest> __Game_Simulation_HandleRequest_RO_ComponentTypeHandle;
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RW_ComponentLookup;
      public ComponentLookup<ServiceRequest> __Game_Simulation_ServiceRequest_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_RequestGroup_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RequestGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_HandleRequest_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HandleRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RW_ComponentLookup = state.GetComponentLookup<Dispatched>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceRequest_RW_ComponentLookup = state.GetComponentLookup<ServiceRequest>();
      }
    }
  }
}
