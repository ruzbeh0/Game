// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NetDeteriorationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
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
  public class NetDeteriorationSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 16;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_LaneQuery;
    private EntityQuery m_EdgeQuery;
    private EntityArchetype m_MaintenanceRequestArchetype;
    private NetDeteriorationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (NetDeteriorationSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery = this.GetEntityQuery(ComponentType.ReadWrite<LaneCondition>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadWrite<NetCondition>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_MaintenanceRequestArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<ServiceRequest>(), ComponentType.ReadWrite<MaintenanceRequest>(), ComponentType.ReadWrite<RequestGroup>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_LaneQuery, this.m_EdgeQuery);
      // ISSUE: reference to a compiler-generated field
      Assert.IsTrue(262144 / NetDeteriorationSystem.kUpdatesPerDay >= 512);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_LaneQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, NetDeteriorationSystem.kUpdatesPerDay, 16)));
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, NetDeteriorationSystem.kUpdatesPerDay, 16)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetDeteriorationSystem.UpdateLaneConditionJob jobData1 = new NetDeteriorationSystem.UpdateLaneConditionJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_LaneDeteriorationData = this.__TypeHandle.__Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup,
        m_LaneConditionType = this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetDeteriorationSystem.UpdateNetConditionJob jobData2 = new NetDeteriorationSystem.UpdateNetConditionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NativeType = this.__TypeHandle.__Game_Common_Native_RO_ComponentTypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_MaintenanceConsumerType = this.__TypeHandle.__Game_Simulation_MaintenanceConsumer_RW_ComponentTypeHandle,
        m_NetConditionType = this.__TypeHandle.__Game_Net_NetCondition_RW_ComponentTypeHandle,
        m_EdgeLaneData = this.__TypeHandle.__Game_Net_EdgeLane_RO_ComponentLookup,
        m_MaintenanceRequestData = this.__TypeHandle.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup,
        m_LaneConditionData = this.__TypeHandle.__Game_Net_LaneCondition_RW_ComponentLookup,
        m_MaintenanceRequestArchetype = this.m_MaintenanceRequestArchetype,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.ScheduleParallel<NetDeteriorationSystem.UpdateLaneConditionJob>(this.m_LaneQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery edgeQuery = this.m_EdgeQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle producerJob = jobData2.ScheduleParallel<NetDeteriorationSystem.UpdateNetConditionJob>(edgeQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
    }

    public static int GetMaintenancePriority(NetCondition condition)
    {
      return (int) ((double) math.cmax(condition.m_Wear) / 10.0 * 100.0) - 10;
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
    public NetDeteriorationSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLaneConditionJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<LaneDeteriorationData> m_LaneDeteriorationData;
      public ComponentTypeHandle<LaneCondition> m_LaneConditionType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        float num = 1f / (float) NetDeteriorationSystem.kUpdatesPerDay;
        // ISSUE: reference to a compiler-generated field
        NativeArray<LaneCondition> nativeArray1 = chunk.GetNativeArray<LaneCondition>(ref this.m_LaneConditionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          LaneCondition laneCondition = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          LaneDeteriorationData deteriorationData = this.m_LaneDeteriorationData[nativeArray2[index].m_Prefab];
          laneCondition.m_Wear = math.min(laneCondition.m_Wear + num * deteriorationData.m_TimeFactor, 10f);
          nativeArray1[index] = laneCondition;
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
    private struct UpdateNetConditionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Native> m_NativeType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> m_SubLaneType;
      public ComponentTypeHandle<MaintenanceConsumer> m_MaintenanceConsumerType;
      public ComponentTypeHandle<NetCondition> m_NetConditionType;
      [ReadOnly]
      public ComponentLookup<EdgeLane> m_EdgeLaneData;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> m_MaintenanceRequestData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<LaneCondition> m_LaneConditionData;
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
        NativeArray<NetCondition> nativeArray2 = chunk.GetNativeArray<NetCondition>(ref this.m_NetConditionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MaintenanceConsumer> nativeArray3 = chunk.GetNativeArray<MaintenanceConsumer>(ref this.m_MaintenanceConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.SubLane> bufferAccessor = chunk.GetBufferAccessor<Game.Net.SubLane>(ref this.m_SubLaneType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Native>(ref this.m_NativeType))
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            ref NetCondition local = ref nativeArray2.ElementAt<NetCondition>(index1);
            DynamicBuffer<Game.Net.SubLane> dynamicBuffer = bufferAccessor[index1];
            float2 float2 = (float2) 0.0f;
            local.m_Wear = float2;
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity subLane = dynamicBuffer[index2].m_SubLane;
              LaneCondition componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneConditionData.TryGetComponent(subLane, out componentData))
              {
                componentData.m_Wear = 0.0f;
                // ISSUE: reference to a compiler-generated field
                this.m_LaneConditionData[subLane] = componentData;
              }
            }
          }
        }
        else
        {
          for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
          {
            Entity entity = nativeArray1[index3];
            ref NetCondition local = ref nativeArray2.ElementAt<NetCondition>(index3);
            DynamicBuffer<Game.Net.SubLane> dynamicBuffer = bufferAccessor[index3];
            local.m_Wear = (float2) 0.0f;
            for (int index4 = 0; index4 < dynamicBuffer.Length; ++index4)
            {
              Entity subLane = dynamicBuffer[index4].m_SubLane;
              LaneCondition componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_LaneConditionData.TryGetComponent(subLane, out componentData1))
              {
                EdgeLane componentData2;
                // ISSUE: reference to a compiler-generated field
                local.m_Wear = !this.m_EdgeLaneData.TryGetComponent(subLane, out componentData2) ? math.max(local.m_Wear, (float2) componentData1.m_Wear) : math.select(local.m_Wear, (float2) componentData1.m_Wear, new bool2(math.any(componentData2.m_EdgeDelta == 0.0f), math.any(componentData2.m_EdgeDelta == 1f)) & componentData1.m_Wear > local.m_Wear);
              }
            }
            if (nativeArray3.Length != 0)
            {
              MaintenanceConsumer maintenanceConsumer = nativeArray3[index3];
              // ISSUE: reference to a compiler-generated method
              this.RequestMaintenanceIfNeeded(unfilteredChunkIndex, entity, local, ref maintenanceConsumer);
              nativeArray3[index3] = maintenanceConsumer;
            }
          }
        }
      }

      private void RequestMaintenanceIfNeeded(
        int jobIndex,
        Entity entity,
        NetCondition condition,
        ref MaintenanceConsumer maintenanceConsumer)
      {
        // ISSUE: reference to a compiler-generated method
        int maintenancePriority = NetDeteriorationSystem.GetMaintenancePriority(condition);
        MaintenanceRequest componentData;
        // ISSUE: reference to a compiler-generated field
        if (maintenancePriority <= 0 || this.m_MaintenanceRequestData.TryGetComponent(maintenanceConsumer.m_Request, out componentData) && (componentData.m_Target == entity || (int) componentData.m_DispatchIndex == (int) maintenanceConsumer.m_DispatchIndex))
          return;
        maintenanceConsumer.m_Request = Entity.Null;
        maintenanceConsumer.m_DispatchIndex = (byte) 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_MaintenanceRequestArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<MaintenanceRequest>(jobIndex, entity1, new MaintenanceRequest(entity, maintenancePriority));
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<LaneDeteriorationData> __Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup;
      public ComponentTypeHandle<LaneCondition> __Game_Net_LaneCondition_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Native> __Game_Common_Native_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      public ComponentTypeHandle<MaintenanceConsumer> __Game_Simulation_MaintenanceConsumer_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetCondition> __Game_Net_NetCondition_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<EdgeLane> __Game_Net_EdgeLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MaintenanceRequest> __Game_Simulation_MaintenanceRequest_RO_ComponentLookup;
      public ComponentLookup<LaneCondition> __Game_Net_LaneCondition_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LaneDeteriorationData_RO_ComponentLookup = state.GetComponentLookup<LaneDeteriorationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LaneCondition>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MaintenanceConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetCondition>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeLane_RO_ComponentLookup = state.GetComponentLookup<EdgeLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_MaintenanceRequest_RO_ComponentLookup = state.GetComponentLookup<MaintenanceRequest>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneCondition_RW_ComponentLookup = state.GetComponentLookup<LaneCondition>();
      }
    }
  }
}
