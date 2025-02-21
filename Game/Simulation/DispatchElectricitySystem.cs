// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DispatchElectricitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DispatchElectricitySystem : GameSystemBase
  {
    public static readonly short kAlertCooldown = 2;
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_ConsumerQuery;
    private DispatchElectricitySystem.TypeHandle __TypeHandle;
    private EntityQuery __query_2129007938_0;
    private EntityQuery __query_2129007938_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 126;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<ElectricityConsumer>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ConsumerQuery);
      this.RequireForUpdate<ElectricityParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ElectricityFlowSystem.ready)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DispatchElectricitySystem.DispatchElectricityJob jobData = new DispatchElectricitySystem.DispatchElectricityJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_ConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_NodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_Parameters = this.__query_2129007938_0.GetSingleton<ElectricityParameterData>(),
        m_EfficiencyParameters = this.__query_2129007938_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_SinkNode = this.m_ElectricityFlowSystem.sinkNode
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<DispatchElectricitySystem.DispatchElectricityJob>(this.m_ConsumerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_2129007938_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ElectricityParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_2129007938_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public DispatchElectricitySystem()
    {
    }

    [BurstCompile]
    private struct DispatchElectricityJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      public ComponentTypeHandle<ElectricityConsumer> m_ConsumerType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_NodeConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      public IconCommandBuffer m_IconCommandBuffer;
      public ElectricityParameterData m_Parameters;
      public BuildingEfficiencyParameterData m_EfficiencyParameters;
      public Entity m_SinkNode;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray2 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray3 = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityConsumer> nativeArray4 = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          ref ElectricityConsumer local = ref nativeArray4.ElementAt<ElectricityConsumer>(index);
          ElectricityConsumerFlags flags = local.m_Flags;
          int x = 0;
          bool beyondBottleneck = false;
          bool flag = false;
          if (nativeArray3.Length != 0)
          {
            if (nativeArray3[index].m_ConsumerEdge != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              ElectricityFlowEdge flowEdge = this.m_FlowEdges[nativeArray3[index].m_ConsumerEdge];
              x = flowEdge.m_Flow;
              beyondBottleneck = flowEdge.isBeyondBottleneck;
              flag = flowEdge.isDisconnected;
            }
            else
              UnityEngine.Debug.LogError((object) "ElectricityBuildingConnection is missing consumer edge!");
          }
          else
          {
            Entity roadEdge = nativeArray2[index].m_RoadEdge;
            ElectricityNodeConnection componentData;
            ElectricityFlowEdge edge;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (roadEdge != Entity.Null && this.m_NodeConnections.TryGetComponent(roadEdge, out componentData) && ElectricityGraphUtils.TryGetFlowEdge(componentData.m_ElectricityNode, this.m_SinkNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out edge))
            {
              if (edge.m_Capacity == edge.m_Flow)
                x = local.m_WantedConsumption;
              else if (edge.m_Capacity > 0)
              {
                float num = (float) edge.m_Flow / (float) edge.m_Capacity;
                x = Mathf.FloorToInt((float) local.m_WantedConsumption * num);
              }
              flag = edge.isDisconnected;
            }
          }
          local.m_FulfilledConsumption = math.min(x, local.m_WantedConsumption);
          // ISSUE: reference to a compiler-generated method
          this.HandleCooldown(nativeArray1[index], beyondBottleneck, ref local, flags);
          if ((local.m_WantedConsumption > 0 ? (local.m_FulfilledConsumption >= local.m_WantedConsumption ? 1 : 0) : (!flag ? 1 : 0)) != 0)
            local.m_Flags |= ElectricityConsumerFlags.Connected;
          else
            local.m_Flags &= ~ElectricityConsumerFlags.Connected;
          if (bufferAccessor.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float efficiency = (float) (1.0 - (double) this.m_EfficiencyParameters.m_ElectricityPenalty * (double) math.saturate((float) local.m_CooldownCounter / this.m_EfficiencyParameters.m_ElectricityPenaltyDelay));
            BuildingUtils.SetEfficiencyFactor(bufferAccessor[index], EfficiencyFactor.ElectricitySupply, efficiency);
          }
        }
      }

      private void HandleCooldown(
        Entity building,
        bool beyondBottleneck,
        ref ElectricityConsumer consumer,
        ElectricityConsumerFlags oldFlags)
      {
        consumer.m_Flags &= ~(ElectricityConsumerFlags.NoElectricityWarning | ElectricityConsumerFlags.BottleneckWarning);
        if (consumer.m_FulfilledConsumption < consumer.m_WantedConsumption)
        {
          consumer.m_CooldownCounter = (short) math.min((int) consumer.m_CooldownCounter + 1, 10000);
          // ISSUE: reference to a compiler-generated field
          if ((int) consumer.m_CooldownCounter >= (int) DispatchElectricitySystem.kAlertCooldown)
            consumer.m_Flags |= beyondBottleneck ? ElectricityConsumerFlags.BottleneckWarning : ElectricityConsumerFlags.NoElectricityWarning;
        }
        else
          consumer.m_CooldownCounter = (short) 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetWarning(building, consumer, oldFlags, ElectricityConsumerFlags.NoElectricityWarning, this.m_Parameters.m_ElectricityNotificationPrefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.SetWarning(building, consumer, oldFlags, ElectricityConsumerFlags.BottleneckWarning, this.m_Parameters.m_BuildingBottleneckNotificationPrefab);
      }

      private void SetWarning(
        Entity building,
        ElectricityConsumer consumer,
        ElectricityConsumerFlags oldFlags,
        ElectricityConsumerFlags flag,
        Entity notificationPrefab)
      {
        if ((oldFlags & flag) == (consumer.m_Flags & flag))
          return;
        if ((consumer.m_Flags & flag) != ElectricityConsumerFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(building, notificationPrefab, IconPriority.Problem);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(building, notificationPrefab);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
      }
    }
  }
}
