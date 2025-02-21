// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DamagedVehicleSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DamagedVehicleSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 512;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_DamagedQuery;
    private EntityArchetype m_MaintenanceRequestArchetype;
    private DamagedVehicleSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 512;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_DamagedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Damaged>(), ComponentType.ReadOnly<Stopped>(), ComponentType.ReadOnly<Car>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<MaintenanceRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DamagedQuery);
      Assert.IsTrue(true);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new DamagedVehicleSystem.DamagedVehicleJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_MaintenanceConsumerType = this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle,
        m_DamagedType = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentTypeHandle,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_MaintenanceRequestArchetype = this.m_MaintenanceRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<DamagedVehicleSystem.DamagedVehicleJob>(this.m_DamagedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public DamagedVehicleSystem()
    {
    }

    [BurstCompile]
    private struct DamagedVehicleJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceConsumer> m_MaintenanceConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> m_DamagedType;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [ReadOnly]
      public EntityArchetype m_MaintenanceRequestArchetype;
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
        NativeArray<Destroyed> nativeArray2 = chunk.GetNativeArray<Destroyed>(ref this.m_DestroyedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MaintenanceConsumer> nativeArray3 = chunk.GetNativeArray<MaintenanceConsumer>(ref this.m_MaintenanceConsumerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Damaged> nativeArray4 = chunk.GetNativeArray<Damaged>(ref this.m_DamagedType);
        if (nativeArray2.Length != 0)
        {
          if (nativeArray3.Length != 0)
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              Destroyed destroyed = nativeArray2[index];
              Entity entity = nativeArray1[index];
              if ((double) destroyed.m_Cleared < 1.0)
              {
                MaintenanceConsumer maintenanceConsumer = nativeArray3[index];
                // ISSUE: reference to a compiler-generated method
                this.RequestMaintenanceIfNeeded(unfilteredChunkIndex, entity, maintenanceConsumer);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<MaintenanceConsumer>(unfilteredChunkIndex, entity);
              }
            }
          }
          else
          {
            for (int index = 0; index < nativeArray2.Length; ++index)
            {
              if ((double) nativeArray2[index].m_Cleared < 1.0)
              {
                Entity entity = nativeArray1[index];
                MaintenanceConsumer maintenanceConsumer = new MaintenanceConsumer();
                // ISSUE: reference to a compiler-generated method
                this.RequestMaintenanceIfNeeded(unfilteredChunkIndex, entity, maintenanceConsumer);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<MaintenanceConsumer>(unfilteredChunkIndex, entity, maintenanceConsumer);
              }
            }
          }
        }
        else if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            Damaged damaged = nativeArray4[index];
            Entity entity = nativeArray1[index];
            if (math.any(damaged.m_Damage > 0.0f))
            {
              MaintenanceConsumer maintenanceConsumer = nativeArray3[index];
              // ISSUE: reference to a compiler-generated method
              this.RequestMaintenanceIfNeeded(unfilteredChunkIndex, entity, maintenanceConsumer);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<MaintenanceConsumer>(unfilteredChunkIndex, entity);
            }
          }
        }
        else
        {
          for (int index = 0; index < nativeArray4.Length; ++index)
          {
            if (math.any(nativeArray4[index].m_Damage > 0.0f))
            {
              Entity entity = nativeArray1[index];
              MaintenanceConsumer maintenanceConsumer = new MaintenanceConsumer();
              // ISSUE: reference to a compiler-generated method
              this.RequestMaintenanceIfNeeded(unfilteredChunkIndex, entity, maintenanceConsumer);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<MaintenanceConsumer>(unfilteredChunkIndex, entity, maintenanceConsumer);
            }
          }
        }
      }

      private void RequestMaintenanceIfNeeded(
        int jobIndex,
        Entity entity,
        MaintenanceConsumer maintenanceConsumer)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_MaintenanceRequestData.HasComponent(maintenanceConsumer.m_Request))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MaintenanceRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<MaintenanceRequest>(jobIndex, entity1, new MaintenanceRequest(entity, 100));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<RequestGroup>(jobIndex, entity1, new RequestGroup(32U));
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
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MaintenanceConsumer> __Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Damaged> __Game_Objects_Damaged_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MaintenanceConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
      }
    }
  }
}
