// Decompiled with JetBrains decompiler
// Type: Game.Simulation.DispatchWaterSystem
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

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class DispatchWaterSystem : GameSystemBase
  {
    public static readonly short kAlertCooldown = 2;
    public static readonly short kHealthPenaltyCooldown = 10;
    private const float kNotificationMaxDelay = 2f;
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_ConsumerQuery;
    private DispatchWaterSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1010455350_0;
    private EntityQuery __query_1010455350_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 62;

    public bool freshConsumptionDisabled { get; set; }

    public bool sewageConsumptionDisabled { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConsumerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadWrite<WaterConsumer>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ConsumerQuery);
      this.RequireForUpdate<WaterPipeParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_WaterPipeFlowSystem.ready)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      DispatchWaterSystem.DispatchWaterJob jobData = new DispatchWaterSystem.DispatchWaterJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle,
        m_ConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_NodeConnections = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_Parameters = this.__query_1010455350_0.GetSingleton<WaterPipeParameterData>(),
        m_EfficiencyParameters = this.__query_1010455350_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_SinkNode = this.m_WaterPipeFlowSystem.sinkNode,
        m_RandomSeed = RandomSeed.Next(),
        m_FreshConsumptionDisabled = this.freshConsumptionDisabled,
        m_SewageConsumptionDisabled = this.sewageConsumptionDisabled
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<DispatchWaterSystem.DispatchWaterJob>(this.m_ConsumerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1010455350_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<WaterPipeParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_1010455350_1 = state.GetEntityQuery(new EntityQueryDesc()
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
    public DispatchWaterSystem()
    {
    }

    [BurstCompile]
    private struct DispatchWaterJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeBuildingConnection> m_BuildingConnectionType;
      public ComponentTypeHandle<WaterConsumer> m_ConsumerType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> m_NodeConnections;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      public IconCommandBuffer m_IconCommandBuffer;
      public WaterPipeParameterData m_Parameters;
      public BuildingEfficiencyParameterData m_EfficiencyParameters;
      public Entity m_SinkNode;
      public RandomSeed m_RandomSeed;
      public bool m_FreshConsumptionDisabled;
      public bool m_SewageConsumptionDisabled;

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
        NativeArray<WaterPipeBuildingConnection> nativeArray3 = chunk.GetNativeArray<WaterPipeBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterConsumer> nativeArray4 = chunk.GetNativeArray<WaterConsumer>(ref this.m_ConsumerType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < chunk.Count; ++index)
        {
          ref WaterConsumer local = ref nativeArray4.ElementAt<WaterConsumer>(index);
          int num1 = 0;
          int num2 = 0;
          float num3 = 0.0f;
          WaterPipeEdgeFlags waterPipeEdgeFlags = WaterPipeEdgeFlags.None;
          if (nativeArray3.Length != 0)
          {
            if (nativeArray3[index].m_ConsumerEdge != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              WaterPipeEdge flowEdge = this.m_FlowEdges[nativeArray3[index].m_ConsumerEdge];
              num1 = flowEdge.m_FreshFlow;
              num2 = flowEdge.m_SewageFlow;
              num3 = flowEdge.m_FreshPollution;
              waterPipeEdgeFlags = flowEdge.m_Flags;
            }
            else
              UnityEngine.Debug.LogError((object) "WaterBuildingConnection is missing consumer edge!");
          }
          else
          {
            Entity roadEdge = nativeArray2[index].m_RoadEdge;
            WaterPipeNodeConnection componentData;
            WaterPipeEdge edge;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (roadEdge != Entity.Null && this.m_NodeConnections.TryGetComponent(roadEdge, out componentData) && WaterPipeGraphUtils.TryGetFlowEdge(componentData.m_WaterPipeNode, this.m_SinkNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out edge))
            {
              if (edge.m_FreshCapacity == edge.m_FreshFlow)
                num1 = local.m_WantedConsumption;
              else if (edge.m_FreshCapacity > 0)
              {
                float num4 = (float) edge.m_FreshFlow / (float) edge.m_FreshCapacity;
                num1 = (int) math.floor((float) local.m_WantedConsumption * num4);
              }
              if (edge.m_SewageCapacity == edge.m_SewageFlow)
                num2 = local.m_WantedConsumption;
              else if (edge.m_SewageCapacity > 0)
              {
                float num5 = (float) edge.m_SewageFlow / (float) edge.m_SewageCapacity;
                num2 = (int) math.floor((float) local.m_WantedConsumption * num5);
              }
              num3 = edge.m_FreshPollution;
              waterPipeEdgeFlags = edge.m_Flags;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_FreshConsumptionDisabled)
          {
            num1 = local.m_WantedConsumption;
            waterPipeEdgeFlags &= ~(WaterPipeEdgeFlags.WaterShortage | WaterPipeEdgeFlags.WaterDisconnected);
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_SewageConsumptionDisabled)
          {
            num2 = local.m_WantedConsumption;
            waterPipeEdgeFlags &= ~(WaterPipeEdgeFlags.SewageBackup | WaterPipeEdgeFlags.SewageDisconnected);
          }
          local.m_FulfilledFresh = num1;
          local.m_FulfilledSewage = num2;
          bool enabled1 = local.m_FulfilledFresh < local.m_WantedConsumption;
          bool enabled2 = local.m_FulfilledSewage < local.m_WantedConsumption;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.HandleCooldown(nativeArray1[index], this.m_Parameters.m_WaterNotification, enabled1, ref local.m_FreshCooldownCounter, ref random);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.HandleCooldown(nativeArray1[index], this.m_Parameters.m_SewageNotification, enabled2, ref local.m_SewageCooldownCounter, ref random);
          // ISSUE: reference to a compiler-generated field
          bool flag = (double) local.m_Pollution > (double) this.m_Parameters.m_MaxToleratedPollution;
          local.m_Pollution = num1 > 0 ? num3 : 0.0f;
          if (local.m_WantedConsumption == 0)
          {
            enabled1 = (waterPipeEdgeFlags & (WaterPipeEdgeFlags.WaterShortage | WaterPipeEdgeFlags.WaterDisconnected)) != 0;
            enabled2 = (waterPipeEdgeFlags & (WaterPipeEdgeFlags.SewageBackup | WaterPipeEdgeFlags.SewageDisconnected)) != 0;
          }
          local.m_Flags = WaterConsumerFlags.None;
          if (!enabled1)
            local.m_Flags |= WaterConsumerFlags.WaterConnected;
          if (!enabled2)
            local.m_Flags |= WaterConsumerFlags.SewageConnected;
          // ISSUE: reference to a compiler-generated field
          if ((double) local.m_Pollution > (double) this.m_Parameters.m_MaxToleratedPollution)
          {
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(nativeArray1[index], this.m_Parameters.m_DirtyWaterNotification, IconPriority.Problem);
            }
          }
          else if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_IconCommandBuffer.Remove(nativeArray1[index], this.m_Parameters.m_DirtyWaterNotification);
          }
          if (bufferAccessor.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float efficiency1 = (float) (1.0 - (double) this.m_EfficiencyParameters.m_WaterPenalty * (double) math.saturate((float) local.m_FreshCooldownCounter / this.m_EfficiencyParameters.m_WaterPenaltyDelay));
            BuildingUtils.SetEfficiencyFactor(bufferAccessor[index], EfficiencyFactor.WaterSupply, efficiency1);
            // ISSUE: reference to a compiler-generated field
            float efficiency2 = (float) (1.0 - (double) this.m_EfficiencyParameters.m_WaterPollutionPenalty * (double) math.round(local.m_Pollution * 100f) / 100.0);
            BuildingUtils.SetEfficiencyFactor(bufferAccessor[index], EfficiencyFactor.DirtyWater, efficiency2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float efficiency3 = (float) (1.0 - (double) this.m_EfficiencyParameters.m_SewagePenalty * (double) math.saturate((float) local.m_SewageCooldownCounter / this.m_EfficiencyParameters.m_SewagePenaltyDelay));
            BuildingUtils.SetEfficiencyFactor(bufferAccessor[index], EfficiencyFactor.SewageHandling, efficiency3);
          }
        }
      }

      private void HandleCooldown(
        Entity building,
        Entity notificationPrefab,
        bool enabled,
        ref byte cooldown,
        ref Unity.Mathematics.Random random)
      {
        // ISSUE: reference to a compiler-generated field
        bool flag = (int) cooldown >= (int) DispatchWaterSystem.kAlertCooldown;
        if (enabled)
        {
          if (cooldown < byte.MaxValue)
            ++cooldown;
          // ISSUE: reference to a compiler-generated field
          if (flag || (int) cooldown < (int) DispatchWaterSystem.kAlertCooldown)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(building, notificationPrefab, IconPriority.Problem);
        }
        else
        {
          cooldown = (byte) 0;
          if (!flag)
            return;
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
      public ComponentTypeHandle<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
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
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
      }
    }
  }
}
