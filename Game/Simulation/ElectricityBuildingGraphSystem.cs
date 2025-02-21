// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityBuildingGraphSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ElectricityBuildingGraphSystem : GameSystemBase
  {
    private ElectricityRoadConnectionGraphSystem m_ElectricityRoadConnectionGraphSystem;
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_UpdatedBuildingQuery;
    private ElectricityBuildingGraphSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<ElectricityRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBuildingQuery = this.GetEntityQuery(CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<ElectricityProducer>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[2]
      {
        ComponentType.ReadOnly<ElectricityConsumer>(),
        ComponentType.ReadOnly<Game.Net.SubNet>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[2]
      {
        ComponentType.ReadOnly<ElectricityConsumer>(),
        ComponentType.ReadOnly<InstalledUpgrade>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<Game.Buildings.Battery>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<Game.Buildings.Transformer>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<ElectricityBuildingConnection>()
      }));
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedBuildingQuery);

      static EntityQueryDesc CreatedUpdatedBuildingDesc(ComponentType[] all)
      {
        return new EntityQueryDesc()
        {
          All = ((IEnumerable<ComponentType>) all).Concat<ComponentType>((IEnumerable<ComponentType>) new ComponentType[1]
          {
            ComponentType.ReadOnly<Building>()
          }).ToArray<ComponentType>(),
          Any = new ComponentType[2]
          {
            ComponentType.ReadOnly<Created>(),
            ComponentType.ReadOnly<Updated>()
          },
          None = new ComponentType[2]
          {
            ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
            ComponentType.ReadOnly<Temp>()
          }
        };
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob jobData = new ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubNetType = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_TransformerType = this.__TypeHandle.__Game_Buildings_Transformer_RO_ComponentTypeHandle,
        m_ProducerType = this.__TypeHandle.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle,
        m_ConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle,
        m_BatteryType = this.__TypeHandle.__Game_Buildings_Battery_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_NetNodes = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_NetOrphans = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_ConnectedNetEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_Deleted = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ElectricityConnectionDatas = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
        m_ElectricityNodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_ElectricityValveConnections = this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdatedRoadEdges = this.m_ElectricityRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps).AsParallelWriter(),
        m_NodeArchetype = this.m_ElectricityFlowSystem.nodeArchetype,
        m_ChargeNodeArchetype = this.m_ElectricityFlowSystem.chargeNodeArchetype,
        m_DischargeNodeArchetype = this.m_ElectricityFlowSystem.dischargeNodeArchetype,
        m_EdgeArchetype = this.m_ElectricityFlowSystem.edgeArchetype,
        m_SourceNode = this.m_ElectricityFlowSystem.sourceNode,
        m_SinkNode = this.m_ElectricityFlowSystem.sinkNode
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob>(this.m_UpdatedBuildingQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ElectricityRoadConnectionGraphSystem.AddQueueWriter(this.Dependency);
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
    public ElectricityBuildingGraphSystem()
    {
    }

    [BurstCompile]
    private struct UpdateBuildingConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> m_SubNetType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Transformer> m_TransformerType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> m_ProducerType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> m_ConsumerType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Battery> m_BatteryType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NetNodes;
      [ReadOnly]
      public ComponentLookup<Orphan> m_NetOrphans;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedNetEdges;
      [ReadOnly]
      public ComponentLookup<Deleted> m_Deleted;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> m_ElectricityConnectionDatas;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityValveConnection> m_ElectricityValveConnections;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedRoadEdges;
      public EntityArchetype m_NodeArchetype;
      public EntityArchetype m_ChargeNodeArchetype;
      public EntityArchetype m_DischargeNodeArchetype;
      public EntityArchetype m_EdgeArchetype;
      public Entity m_SourceNode;
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
        BufferAccessor<Game.Net.SubNet> bufferAccessor1 = chunk.GetBufferAccessor<Game.Net.SubNet>(ref this.m_SubNetType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray2 = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        bool isTransformer = chunk.Has<Game.Buildings.Transformer>(ref this.m_TransformerType);
        // ISSUE: reference to a compiler-generated field
        bool isProducer = chunk.Has<ElectricityProducer>(ref this.m_ProducerType);
        // ISSUE: reference to a compiler-generated field
        bool isConsumer = chunk.Has<ElectricityConsumer>(ref this.m_ConsumerType);
        // ISSUE: reference to a compiler-generated field
        bool isBattery = chunk.Has<Game.Buildings.Battery>(ref this.m_BatteryType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Destroyed>(ref this.m_DestroyedType);
        NativeList<ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData> result = new NativeList<ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          result.Clear();
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindMarkerNodes(bufferAccessor1[index], result);
          }
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindMarkerNodes(bufferAccessor2[index], result);
          }
          if (result.Length > 0)
          {
            // ISSUE: variable of a compiler-generated type
            ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes buildingNodes;
            if (!flag && isTransformer | isProducer | isConsumer | isBattery)
            {
              ElectricityBuildingConnection connection = nativeArray2.Length != 0 ? nativeArray2[index] : new ElectricityBuildingConnection();
              // ISSUE: reference to a compiler-generated method
              buildingNodes = this.CreateOrUpdateBuildingNodes(unfilteredChunkIndex, isTransformer, isProducer, isConsumer, isBattery, ref connection);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<ElectricityBuildingConnection>(unfilteredChunkIndex, entity, connection);
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              buildingNodes = new ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes();
              if (nativeArray2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteBuildingNodes(unfilteredChunkIndex, entity, nativeArray2[index]);
              }
            }
            Entity roadEdge = nativeArray3[index].m_RoadEdge;
            // ISSUE: reference to a compiler-generated field
            if (roadEdge != Entity.Null && this.m_ElectricityNodeConnections.TryGetComponent(roadEdge, out ElectricityNodeConnection _) && isConsumer)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedRoadEdges.Enqueue(roadEdge);
            }
            foreach (ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData markerNodeData in result)
            {
              // ISSUE: reference to a compiler-generated method
              this.CreateOrUpdateMarkerNode(unfilteredChunkIndex, markerNodeData, buildingNodes);
            }
          }
          else if (nativeArray2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.DeleteBuildingNodes(unfilteredChunkIndex, entity, nativeArray2[index]);
          }
        }
      }

      private void FindMarkerNodes(
        DynamicBuffer<InstalledUpgrade> upgrades,
        NativeList<ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData> result)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          InstalledUpgrade upgrade = upgrades[index];
          DynamicBuffer<Game.Net.SubNet> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive) && this.m_SubNets.TryGetBuffer(upgrade.m_Upgrade, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.FindMarkerNodes(bufferData, result);
          }
        }
      }

      private void FindMarkerNodes(
        DynamicBuffer<Game.Net.SubNet> subNets,
        NativeList<ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData> result)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          PrefabRef componentData1;
          ElectricityConnectionData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetNodes.HasComponent(subNet) && (this.m_ElectricityValveConnections.HasComponent(subNet) || this.IsOrphan(subNet)) && !this.m_Deleted.HasComponent(subNet) && this.m_PrefabRefs.TryGetComponent(subNet, out componentData1) && this.m_ElectricityConnectionDatas.TryGetComponent(componentData1.m_Prefab, out componentData2))
          {
            // ISSUE: object of a compiler-generated type is created
            result.Add(new ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData()
            {
              m_NetNode = subNet,
              m_Capacity = componentData2.m_Capacity,
              m_Direction = componentData2.m_Direction,
              m_Voltage = componentData2.m_Voltage
            });
          }
        }
      }

      private bool IsOrphan(Entity netNode)
      {
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_NetOrphans.HasComponent(netNode) || !this.m_ConnectedNetEdges.TryGetBuffer(netNode, out bufferData))
          return true;
        foreach (ConnectedEdge connectedEdge in bufferData)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Owners.HasComponent(connectedEdge.m_Edge))
            return false;
        }
        return true;
      }

      private ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes CreateOrUpdateBuildingNodes(
        int jobIndex,
        bool isTransformer,
        bool isProducer,
        bool isConsumer,
        bool isBattery,
        ref ElectricityBuildingConnection connection)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes updateBuildingNodes = new ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes();
        if (isTransformer)
        {
          if (connection.m_TransformerNode == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            connection.m_TransformerNode = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype);
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_TransformerNode = new BufferedEntity(connection.m_TransformerNode, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_TransformerNode = new BufferedEntity(connection.m_TransformerNode, true);
          }
        }
        else if (connection.m_TransformerNode != Entity.Null)
        {
          connection.m_TransformerNode = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, connection.m_TransformerNode);
        }
        if (isProducer)
        {
          if (connection.m_ProducerEdge == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            connection.m_ProducerEdge = this.CreateFlowEdge(jobIndex, this.m_SourceNode, entity, FlowDirection.Forward, 0);
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_ProducerNode = new BufferedEntity(entity, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_ProducerNode = new BufferedEntity(connection.GetProducerNode(ref this.m_FlowEdges), true);
          }
        }
        else if (connection.m_ProducerEdge != Entity.Null)
        {
          connection.m_ProducerEdge = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, connection.GetProducerNode(ref this.m_FlowEdges));
        }
        if (isConsumer)
        {
          if (connection.m_ConsumerEdge == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            connection.m_ConsumerEdge = this.CreateFlowEdge(jobIndex, entity, this.m_SinkNode, FlowDirection.Forward, 0);
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_ConsumerNode = new BufferedEntity(entity, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_ConsumerNode = new BufferedEntity(connection.GetConsumerNode(ref this.m_FlowEdges), true);
          }
        }
        else if (connection.m_ConsumerEdge != Entity.Null)
        {
          connection.m_ConsumerEdge = Entity.Null;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, connection.GetConsumerNode(ref this.m_FlowEdges));
        }
        if (isBattery)
        {
          if (connection.m_ChargeEdge == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_ChargeNodeArchetype);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            connection.m_ChargeEdge = this.CreateFlowEdge(jobIndex, entity, this.m_SinkNode, FlowDirection.None, 0);
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_ChargeNode = new BufferedEntity(entity, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_ChargeNode = new BufferedEntity(connection.GetChargeNode(ref this.m_FlowEdges), true);
          }
          if (connection.m_DischargeEdge == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_DischargeNodeArchetype);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            connection.m_DischargeEdge = this.CreateFlowEdge(jobIndex, this.m_SourceNode, entity, FlowDirection.None, 0);
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_DischargeNode = new BufferedEntity(entity, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            updateBuildingNodes.m_DischargeNode = new BufferedEntity(connection.GetDischargeNode(ref this.m_FlowEdges), true);
          }
        }
        else
        {
          if (connection.m_ChargeEdge != Entity.Null)
          {
            connection.m_ChargeEdge = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, connection.GetChargeNode(ref this.m_FlowEdges));
          }
          if (connection.m_DischargeEdge != Entity.Null)
          {
            connection.m_DischargeEdge = Entity.Null;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, connection.GetDischargeNode(ref this.m_FlowEdges));
          }
        }
        if (isProducer & isConsumer)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, updateBuildingNodes.m_ProducerNode, updateBuildingNodes.m_ConsumerNode, FlowDirection.Forward, 1073741823);
        }
        if (isProducer & isBattery)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, updateBuildingNodes.m_ProducerNode, updateBuildingNodes.m_ChargeNode, FlowDirection.None, 1073741823);
        }
        if (isConsumer & isBattery)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, updateBuildingNodes.m_DischargeNode, updateBuildingNodes.m_ConsumerNode, FlowDirection.None, 1073741823);
        }
        return updateBuildingNodes;
      }

      private void DeleteBuildingNodes(
        int jobIndex,
        Entity building,
        ElectricityBuildingConnection connection)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ElectricityGraphUtils.DeleteBuildingNodes(this.m_CommandBuffer, jobIndex, connection, ref this.m_FlowConnections, ref this.m_FlowEdges);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<ElectricityBuildingConnection>(jobIndex, building);
      }

      private void CreateOrUpdateMarkerNode(
        int jobIndex,
        ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData markerNodeData,
        ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes buildingNodes)
      {
        ElectricityNodeConnection componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool component1 = this.m_ElectricityNodeConnections.TryGetComponent(markerNodeData.m_NetNode, out componentData1);
        if (!component1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          componentData1 = new ElectricityNodeConnection()
          {
            m_ElectricityNode = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<ElectricityNodeConnection>(jobIndex, markerNodeData.m_NetNode, componentData1);
        }
        BufferedEntity bufferedEntity1 = new BufferedEntity(componentData1.m_ElectricityNode, component1);
        ElectricityValveConnection componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool component2 = this.m_ElectricityValveConnections.TryGetComponent(markerNodeData.m_NetNode, out componentData2);
        if (!component2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          componentData2 = new ElectricityValveConnection()
          {
            m_ValveNode = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<ElectricityValveConnection>(jobIndex, markerNodeData.m_NetNode, componentData2);
        }
        BufferedEntity bufferedEntity2 = new BufferedEntity(componentData2.m_ValveNode, component2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CreateOrUpdateFlowEdge(jobIndex, bufferedEntity2, bufferedEntity1, markerNodeData.m_Direction, markerNodeData.m_Capacity);
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_TransformerNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, buildingNodes.m_TransformerNode, bufferedEntity2, markerNodeData.m_Direction, markerNodeData.m_Capacity);
        }
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_ProducerNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, buildingNodes.m_ProducerNode, bufferedEntity2, FlowDirection.Forward, markerNodeData.m_Capacity);
        }
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_ConsumerNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, bufferedEntity2, buildingNodes.m_ConsumerNode, FlowDirection.Forward, markerNodeData.m_Capacity);
        }
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_ChargeNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, bufferedEntity2, buildingNodes.m_ChargeNode, FlowDirection.None, markerNodeData.m_Capacity);
        }
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_DischargeNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, buildingNodes.m_DischargeNode, bufferedEntity2, FlowDirection.None, markerNodeData.m_Capacity);
        }
        // ISSUE: reference to a compiler-generated method
        this.EnsureMarkerNodeEdgeConnections(jobIndex, markerNodeData, bufferedEntity1);
      }

      private void EnsureMarkerNodeEdgeConnections(
        int jobIndex,
        ElectricityBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData markerNodeData,
        BufferedEntity markerNode)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (ConnectedEdge connectedEdge in this.m_ConnectedNetEdges[markerNodeData.m_NetNode])
        {
          ElectricityNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElectricityNodeConnections.TryGetComponent(connectedEdge.m_Edge, out componentData))
          {
            Entity electricityNode = componentData.m_ElectricityNode;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!markerNode.m_Stored || !ElectricityGraphUtils.HasAnyFlowEdge(markerNode.m_Value, electricityNode, ref this.m_FlowConnections, ref this.m_FlowEdges))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateFlowEdge(jobIndex, markerNode.m_Value, componentData.m_ElectricityNode, markerNodeData.m_Direction, markerNodeData.m_Capacity);
            }
          }
        }
      }

      private void CreateOrUpdateFlowEdge(
        int jobIndex,
        BufferedEntity startNode,
        BufferedEntity endNode,
        FlowDirection direction,
        int capacity)
      {
        Entity entity;
        ElectricityFlowEdge edge;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (startNode.m_Stored && endNode.m_Stored && ElectricityGraphUtils.TryGetFlowEdge(startNode.m_Value, endNode.m_Value, ref this.m_FlowConnections, ref this.m_FlowEdges, out entity, out edge))
        {
          if (edge.direction == direction && edge.m_Capacity == capacity)
            return;
          edge.direction = direction;
          edge.m_Capacity = capacity;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<ElectricityFlowEdge>(jobIndex, entity, edge);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateFlowEdge(jobIndex, startNode.m_Value, endNode.m_Value, direction, capacity);
        }
      }

      private Entity CreateFlowEdge(
        int jobIndex,
        Entity startNode,
        Entity endNode,
        FlowDirection direction,
        int capacity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return ElectricityGraphUtils.CreateFlowEdge(this.m_CommandBuffer, jobIndex, this.m_EdgeArchetype, startNode, endNode, direction, capacity);
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

      private struct BuildingNodes
      {
        public BufferedEntity m_TransformerNode;
        public BufferedEntity m_ProducerNode;
        public BufferedEntity m_ConsumerNode;
        public BufferedEntity m_ChargeNode;
        public BufferedEntity m_DischargeNode;
      }

      private struct MarkerNodeData
      {
        public Entity m_NetNode;
        public int m_Capacity;
        public FlowDirection m_Direction;
        public Game.Prefabs.ElectricityConnection.Voltage m_Voltage;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Transformer> __Game_Buildings_Transformer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityProducer> __Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.Battery> __Game_Buildings_Battery_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityValveConnection> __Game_Simulation_ElectricityValveConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Transformer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Transformer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityProducer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityProducer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Battery_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Battery>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup = state.GetComponentLookup<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityValveConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
      }
    }
  }
}
