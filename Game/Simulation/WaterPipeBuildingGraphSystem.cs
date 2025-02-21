// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipeBuildingGraphSystem
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
  public class WaterPipeBuildingGraphSystem : GameSystemBase
  {
    private WaterPipeRoadConnectionGraphSystem m_WaterPipeRoadConnectionGraphSystem;
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private ModificationBarrier4B m_ModificationBarrier;
    private EntityQuery m_UpdatedBuildingQuery;
    private WaterPipeBuildingGraphSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<WaterPipeRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4B>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedBuildingQuery = this.GetEntityQuery(CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<Game.Buildings.WaterPumpingStation>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<Game.Buildings.SewageOutlet>()
      }), CreatedUpdatedBuildingDesc(new ComponentType[1]
      {
        ComponentType.ReadOnly<WaterPipeBuildingConnection>()
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
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeValveConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob jobData = new WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_SubNetType = this.__TypeHandle.__Game_Net_SubNet_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_PumpingStationType = this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle,
        m_SewageOutletType = this.__TypeHandle.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle,
        m_ConsumerType = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_NetNodes = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_NetOrphans = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_ConnectedNetEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_Deleted = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_WaterPipeConnectionDatas = this.__TypeHandle.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup,
        m_WaterPipeNodeConnections = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup,
        m_WaterPipeValveConnections = this.__TypeHandle.__Game_Simulation_WaterPipeValveConnection_RO_ComponentLookup,
        m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_UpdatedRoadEdges = this.m_WaterPipeRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps).AsParallelWriter(),
        m_NodeArchetype = this.m_WaterPipeFlowSystem.nodeArchetype,
        m_EdgeArchetype = this.m_WaterPipeFlowSystem.edgeArchetype,
        m_SourceNode = this.m_WaterPipeFlowSystem.sourceNode,
        m_SinkNode = this.m_WaterPipeFlowSystem.sinkNode
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob>(this.m_UpdatedBuildingQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterPipeRoadConnectionGraphSystem.AddQueueWriter(this.Dependency);
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
    public WaterPipeBuildingGraphSystem()
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
      public ComponentTypeHandle<WaterPipeBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPumpingStation> m_PumpingStationType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> m_SewageOutletType;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> m_ConsumerType;
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
      public ComponentLookup<WaterPipeConnectionData> m_WaterPipeConnectionDatas;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> m_WaterPipeNodeConnections;
      [ReadOnly]
      public ComponentLookup<WaterPipeValveConnection> m_WaterPipeValveConnections;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<Entity>.ParallelWriter m_UpdatedRoadEdges;
      public EntityArchetype m_NodeArchetype;
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
        NativeArray<WaterPipeBuildingConnection> nativeArray2 = chunk.GetNativeArray<WaterPipeBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Game.Buildings.WaterPumpingStation>(ref this.m_PumpingStationType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Game.Buildings.SewageOutlet>(ref this.m_SewageOutletType);
        // ISSUE: reference to a compiler-generated field
        bool isConsumer = chunk.Has<WaterConsumer>(ref this.m_ConsumerType);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = chunk.Has<Destroyed>(ref this.m_DestroyedType);
        NativeList<WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData> result = new NativeList<WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
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
            WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes buildingNodes;
            if (!flag3 && flag1 | flag2 | isConsumer)
            {
              WaterPipeBuildingConnection connection = nativeArray2.Length != 0 ? nativeArray2[index] : new WaterPipeBuildingConnection();
              // ISSUE: reference to a compiler-generated method
              buildingNodes = this.CreateOrUpdateBuildingNodes(unfilteredChunkIndex, flag1 | flag2, isConsumer, ref connection);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<WaterPipeBuildingConnection>(unfilteredChunkIndex, entity, connection);
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              buildingNodes = new WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes();
              if (nativeArray2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.DeleteBuildingNodes(unfilteredChunkIndex, entity, nativeArray2[index]);
              }
            }
            Entity roadEdge = nativeArray3[index].m_RoadEdge;
            // ISSUE: reference to a compiler-generated field
            if (roadEdge != Entity.Null && this.m_WaterPipeNodeConnections.TryGetComponent(roadEdge, out WaterPipeNodeConnection _) && isConsumer)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedRoadEdges.Enqueue(roadEdge);
            }
            foreach (WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData markerNodeData in result)
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
        NativeList<WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData> result)
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
        NativeList<WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData> result)
      {
        for (int index = 0; index < subNets.Length; ++index)
        {
          Entity subNet = subNets[index].m_SubNet;
          PrefabRef componentData1;
          WaterPipeConnectionData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetNodes.HasComponent(subNet) && (this.m_WaterPipeValveConnections.HasComponent(subNet) || this.IsOrphan(subNet)) && !this.m_Deleted.HasComponent(subNet) && this.m_PrefabRefs.TryGetComponent(subNet, out componentData1) && this.m_WaterPipeConnectionDatas.TryGetComponent(componentData1.m_Prefab, out componentData2))
          {
            // ISSUE: object of a compiler-generated type is created
            result.Add(new WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData()
            {
              m_NetNode = subNet,
              m_FreshCapacity = componentData2.m_FreshCapacity,
              m_SewageCapacity = componentData2.m_SewageCapacity
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

      private WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes CreateOrUpdateBuildingNodes(
        int jobIndex,
        bool isProducer,
        bool isConsumer,
        ref WaterPipeBuildingConnection connection)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes updateBuildingNodes = new WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes();
        if (isProducer)
        {
          if (connection.m_ProducerEdge == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            connection.m_ProducerEdge = this.CreateFlowEdge(jobIndex, this.m_SourceNode, entity, 0, 0);
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
            connection.m_ConsumerEdge = this.CreateFlowEdge(jobIndex, entity, this.m_SinkNode, 0, 0);
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
        if (isProducer & isConsumer)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, updateBuildingNodes.m_ProducerNode, updateBuildingNodes.m_ConsumerNode, 1073741823, 1073741823);
        }
        return updateBuildingNodes;
      }

      private void DeleteBuildingNodes(
        int jobIndex,
        Entity building,
        WaterPipeBuildingConnection connection)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        WaterPipeGraphUtils.DeleteBuildingNodes(this.m_CommandBuffer, jobIndex, connection, ref this.m_FlowConnections, ref this.m_FlowEdges);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent<WaterPipeBuildingConnection>(jobIndex, building);
      }

      private void CreateOrUpdateMarkerNode(
        int jobIndex,
        WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData markerNodeData,
        WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.BuildingNodes buildingNodes)
      {
        WaterPipeNodeConnection componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool component1 = this.m_WaterPipeNodeConnections.TryGetComponent(markerNodeData.m_NetNode, out componentData1);
        if (!component1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          componentData1 = new WaterPipeNodeConnection()
          {
            m_WaterPipeNode = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<WaterPipeNodeConnection>(jobIndex, markerNodeData.m_NetNode, componentData1);
        }
        BufferedEntity bufferedEntity1 = new BufferedEntity(componentData1.m_WaterPipeNode, component1);
        WaterPipeValveConnection componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool component2 = this.m_WaterPipeValveConnections.TryGetComponent(markerNodeData.m_NetNode, out componentData2);
        if (!component2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          componentData2 = new WaterPipeValveConnection()
          {
            m_ValveNode = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_NodeArchetype)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<WaterPipeValveConnection>(jobIndex, markerNodeData.m_NetNode, componentData2);
        }
        BufferedEntity bufferedEntity2 = new BufferedEntity(componentData2.m_ValveNode, component2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CreateOrUpdateFlowEdge(jobIndex, bufferedEntity2, bufferedEntity1, markerNodeData.m_FreshCapacity, markerNodeData.m_SewageCapacity);
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_ProducerNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, buildingNodes.m_ProducerNode, bufferedEntity2, markerNodeData.m_FreshCapacity, markerNodeData.m_SewageCapacity);
        }
        // ISSUE: reference to a compiler-generated field
        if (buildingNodes.m_ConsumerNode.m_Value != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateOrUpdateFlowEdge(jobIndex, bufferedEntity2, buildingNodes.m_ConsumerNode, markerNodeData.m_FreshCapacity, markerNodeData.m_SewageCapacity);
        }
        // ISSUE: reference to a compiler-generated method
        this.EnsureMarkerNodeEdgeConnections(jobIndex, markerNodeData, bufferedEntity1);
      }

      private void EnsureMarkerNodeEdgeConnections(
        int jobIndex,
        WaterPipeBuildingGraphSystem.UpdateBuildingConnectionsJob.MarkerNodeData markerNodeData,
        BufferedEntity markerNode)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (ConnectedEdge connectedEdge in this.m_ConnectedNetEdges[markerNodeData.m_NetNode])
        {
          WaterPipeNodeConnection componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_WaterPipeNodeConnections.TryGetComponent(connectedEdge.m_Edge, out componentData))
          {
            Entity waterPipeNode = componentData.m_WaterPipeNode;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!markerNode.m_Stored || !WaterPipeGraphUtils.HasAnyFlowEdge(markerNode.m_Value, waterPipeNode, ref this.m_FlowConnections, ref this.m_FlowEdges))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.CreateFlowEdge(jobIndex, markerNode.m_Value, componentData.m_WaterPipeNode, markerNodeData.m_FreshCapacity, markerNodeData.m_SewageCapacity);
            }
          }
        }
      }

      private void CreateOrUpdateFlowEdge(
        int jobIndex,
        BufferedEntity startNode,
        BufferedEntity endNode,
        int freshCapacity,
        int sewageCapacity)
      {
        Entity entity;
        WaterPipeEdge edge;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (startNode.m_Stored && endNode.m_Stored && WaterPipeGraphUtils.TryGetFlowEdge(startNode.m_Value, endNode.m_Value, ref this.m_FlowConnections, ref this.m_FlowEdges, out entity, out edge))
        {
          if (edge.m_FreshCapacity == freshCapacity && edge.m_SewageCapacity == sewageCapacity)
            return;
          edge.m_FreshCapacity = freshCapacity;
          edge.m_SewageCapacity = sewageCapacity;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<WaterPipeEdge>(jobIndex, entity, edge);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.CreateFlowEdge(jobIndex, startNode.m_Value, endNode.m_Value, freshCapacity, sewageCapacity);
        }
      }

      private Entity CreateFlowEdge(
        int jobIndex,
        Entity startNode,
        Entity endNode,
        int freshCapacity,
        int sewageCapacity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return WaterPipeGraphUtils.CreateFlowEdge(this.m_CommandBuffer, jobIndex, this.m_EdgeArchetype, startNode, endNode, freshCapacity, sewageCapacity);
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
        public BufferedEntity m_ProducerNode;
        public BufferedEntity m_ConsumerNode;
      }

      private struct MarkerNodeData
      {
        public Entity m_NetNode;
        public int m_FreshCapacity;
        public int m_SewageCapacity;
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
      public ComponentTypeHandle<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPumpingStation> __Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> __Game_Buildings_SewageOutlet_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentTypeHandle;
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
      public ComponentLookup<WaterPipeConnectionData> __Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeValveConnection> __Game_Simulation_WaterPipeValveConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;

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
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPumpingStation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WaterPumpingStation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SewageOutlet_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.SewageOutlet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterConsumer>(true);
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
        this.__Game_Prefabs_WaterPipeConnectionData_RO_ComponentLookup = state.GetComponentLookup<WaterPipeConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeValveConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeValveConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
      }
    }
  }
}
