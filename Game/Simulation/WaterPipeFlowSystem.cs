// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipeFlowSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Net;
using Game.Serialization;
using Game.Simulation.Flow;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterPipeFlowSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    public const int kUpdateInterval = 128;
    public const int kUpdateOffset = 64;
    public const int kUpdatesPerDay = 2048;
    public const int kStartFrames = 2;
    public const int kAdjustFrame = 64;
    public const int kPrepareFrame = 65;
    public const int kFlowFrames = 124;
    public const int kFlowCompletionFrame = 61;
    public const int kEndFrames = 2;
    public const int kApplyFrame = 62;
    public const int kStatusFrame = 63;
    public const int kMaxEdgeCapacity = 1073741823;
    private const int kLayerHeight = 20;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_NodeGroup;
    private EntityQuery m_EdgeGroup;
    private EntityArchetype m_NodeArchetype;
    private EntityArchetype m_EdgeArchetype;
    private WaterPipeFlowJob.Data m_FreshData;
    private WaterPipeFlowJob.Data m_SewageData;
    private NativeList<Game.Simulation.Flow.Connection> m_Connections;
    private NativeReference<NodeIndices> m_NodeIndices;
    private NativeList<int> m_TradeNodes;
    private Entity m_SourceNode;
    private Entity m_SinkNode;
    private WaterPipeFlowSystem.Phase m_NextPhase;
    private JobHandle m_DataDependency;
    private WaterPipeFlowSystem.TypeHandle __TypeHandle;

    public bool ready { get; private set; }

    public EntityArchetype nodeArchetype => this.m_NodeArchetype;

    public EntityArchetype edgeArchetype => this.m_EdgeArchetype;

    public Entity sourceNode => this.m_SourceNode;

    public Entity sinkNode => this.m_SinkNode;

    public bool fluidFlowEnabled { get; set; } = true;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeGroup = this.GetEntityQuery(ComponentType.ReadWrite<WaterPipeNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeGroup = this.GetEntityQuery(ComponentType.ReadWrite<WaterPipeEdge>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_NodeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<WaterPipeNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<WaterPipeEdge>());
      // ISSUE: reference to a compiler-generated field
      this.m_FreshData = new WaterPipeFlowJob.Data(200000, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SewageData = new WaterPipeFlowJob.Data(200000, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Connections = new NativeList<Game.Simulation.Flow.Connection>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_NodeIndices = new NativeReference<NodeIndices>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TradeNodes = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_FreshData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SewageData.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Connections.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeNodes.Dispose();
      base.OnDestroy();
    }

    public void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = WaterPipeFlowSystem.Phase.Prepare;
      // ISSUE: reference to a compiler-generated field
      this.m_FreshData.m_State.Value = new WaterPipeFlowJob.State(200000);
      // ISSUE: reference to a compiler-generated field
      this.m_SewageData.m_State.Value = new WaterPipeFlowJob.State(200000);
      this.ready = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_NextPhase == WaterPipeFlowSystem.Phase.Prepare)
      {
        // ISSUE: reference to a compiler-generated method
        this.PreparePhase();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NextPhase == WaterPipeFlowSystem.Phase.Flow)
        {
          // ISSUE: reference to a compiler-generated method
          this.FlowPhase();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NextPhase != WaterPipeFlowSystem.Phase.Apply)
            return;
          // ISSUE: reference to a compiler-generated method
          this.ApplyPhase();
        }
      }
    }

    private void PreparePhase()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex % 128U != 65U)
        return;
      // ISSUE: reference to a compiler-generated field
      int chunkCapacity1 = this.m_NodeArchetype.ChunkCapacity;
      // ISSUE: reference to a compiler-generated field
      int chunkCapacity2 = this.m_EdgeArchetype.ChunkCapacity;
      // ISSUE: reference to a compiler-generated field
      int num1 = chunkCapacity1 * this.m_NodeGroup.CalculateChunkCountWithoutFiltering();
      // ISSUE: reference to a compiler-generated field
      int num2 = chunkCapacity2 * this.m_EdgeGroup.CalculateChunkCountWithoutFiltering();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle dependsOn = new WaterPipeFlowSystem.PrepareNetworkJob()
      {
        m_FreshNodes = this.m_FreshData.m_Nodes,
        m_SewageNodes = this.m_SewageData.m_Nodes,
        m_FreshEdges = this.m_FreshData.m_Edges,
        m_SewageEdges = this.m_SewageData.m_Edges,
        m_Connections = this.m_Connections,
        m_TradeNodes = this.m_TradeNodes,
        m_NodeCount = num1,
        m_EdgeCount = num2
      }.Schedule<WaterPipeFlowSystem.PrepareNetworkJob>(JobHandle.CombineDependencies(this.Dependency, this.m_DataDependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job0_1 = new WaterPipeFlowSystem.PrepareNodesJob()
      {
        m_FlowNodeType = this.__TypeHandle.__Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle,
        m_MaxChunkCapacity = chunkCapacity1
      }.ScheduleParallel<WaterPipeFlowSystem.PrepareNodesJob>(this.m_NodeGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job1_1 = new WaterPipeFlowSystem.PrepareEdgesJob()
      {
        m_FlowEdgeType = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle,
        m_FreshEdges = this.m_FreshData.m_Edges.AsDeferredJobArray(),
        m_SewageEdges = this.m_SewageData.m_Edges.AsDeferredJobArray(),
        m_MaxChunkCapacity = chunkCapacity2
      }.ScheduleParallel<WaterPipeFlowSystem.PrepareEdgesJob>(this.m_EdgeGroup, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNode_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TradeNode_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job0_2 = new WaterPipeFlowSystem.PrepareConnectionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ConnectedFlowEdgeType = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle,
        m_TradeNodeType = this.__TypeHandle.__Game_Simulation_TradeNode_RO_ComponentTypeHandle,
        m_FlowNodes = this.__TypeHandle.__Game_Simulation_WaterPipeNode_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup,
        m_FreshNodes = this.m_FreshData.m_Nodes.AsDeferredJobArray(),
        m_SewageNodes = this.m_SewageData.m_Nodes.AsDeferredJobArray(),
        m_Connections = this.m_Connections,
        m_TradeNodes = this.m_TradeNodes,
        m_MaxChunkCapacity = chunkCapacity1
      }.Schedule<WaterPipeFlowSystem.PrepareConnectionsJob>(this.m_NodeGroup, JobHandle.CombineDependencies(job0_1, job1_1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNode_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job1_2 = new WaterPipeFlowSystem.PopulateNodeIndicesJob()
      {
        m_FlowNodes = this.__TypeHandle.__Game_Simulation_WaterPipeNode_RO_ComponentLookup,
        m_NodeIndices = this.m_NodeIndices,
        m_SourceNode = this.m_SourceNode,
        m_SinkNode = this.m_SinkNode
      }.Schedule<WaterPipeFlowSystem.PopulateNodeIndicesJob>(JobHandle.CombineDependencies(job0_1, this.m_DataDependency));
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_DataDependency = JobHandle.CombineDependencies(job0_2, job1_2);
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = WaterPipeFlowSystem.Phase.Flow;
    }

    private void FlowPhase()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex % 128U;
      int num2;
      switch (num1)
      {
        case 62:
        case 64:
        case 65:
          num2 = 0;
          break;
        default:
          num2 = num1 != 63U ? 1 : 0;
          break;
      }
      Assert.IsTrue(num2 != 0);
      bool finalFrame = num1 == 61U;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DataDependency = JobHandle.CombineDependencies(this.ScheduleFlowJob(this.m_FreshData, 1073741823, 1073741823, finalFrame), this.ScheduleFlowJob(this.m_SewageData, 1073741823, 0, finalFrame));
      if (!finalFrame)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = WaterPipeFlowSystem.Phase.Apply;
    }

    private JobHandle ScheduleFlowJob(
      WaterPipeFlowJob.Data jobData,
      int importCapacity,
      int exportCapacity,
      bool finalFrame)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new WaterPipeFlowJob()
      {
        m_State = jobData.m_State,
        m_Nodes = jobData.m_Nodes.AsDeferredJobArray(),
        m_Edges = jobData.m_Edges.AsDeferredJobArray(),
        m_Connections = this.m_Connections.AsDeferredJobArray(),
        m_NodeIndices = this.m_NodeIndices,
        m_TradeNodes = this.m_TradeNodes.AsDeferredJobArray(),
        m_MaxFlowState = jobData.m_MaxFlowState,
        m_LayerStates = jobData.m_LayerStates,
        m_LayerElements = jobData.m_LayerElements,
        m_LayerElementRefs = jobData.m_LayerElementRefs,
        m_FluidFlowState = jobData.m_FluidFlowState,
        m_ImportCapacity = importCapacity,
        m_ExportCapacity = exportCapacity,
        m_FluidFlowEnabled = this.fluidFlowEnabled,
        m_LayerHeight = 20,
        m_FrameCount = 1,
        m_FinalFrame = finalFrame
      }.Schedule<WaterPipeFlowJob>(this.m_DataDependency);
    }

    private void ApplyPhase()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex % 128U != 62U)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_DataDependency = new WaterPipeFlowSystem.ApplyEdgesJob()
      {
        m_FlowEdgeType = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle,
        m_FreshEdges = this.m_FreshData.m_Edges.AsDeferredJobArray(),
        m_SewageEdges = this.m_SewageData.m_Edges.AsDeferredJobArray()
      }.ScheduleParallel<WaterPipeFlowSystem.ApplyEdgesJob>(this.m_EdgeGroup, JobHandle.CombineDependencies(this.Dependency, this.m_DataDependency));
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = WaterPipeFlowSystem.Phase.Prepare;
      this.ready = true;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_SourceNode);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_SinkNode);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_FreshData.m_State.Value.m_LastTotalSteps);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_SewageData.m_State.Value.m_LastTotalSteps);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      if (reader.context.version >= Version.waterPipeFlowSim)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_SourceNode);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_SinkNode);
      }
      else
      {
        if (reader.context.version >= Version.waterTrade)
          reader.Read(out Entity _);
        // ISSUE: reference to a compiler-generated field
        this.m_SourceNode = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_SinkNode = Entity.Null;
      }
      if (!(reader.context.version > Version.flowJobImprovements))
        return;
      // ISSUE: reference to a compiler-generated field
      ref WaterPipeFlowJob.State local1 = ref this.m_FreshData.m_State.ValueAsRef<WaterPipeFlowJob.State>();
      reader.Read(out local1.m_LastTotalSteps);
      // ISSUE: reference to a compiler-generated field
      ref WaterPipeFlowJob.State local2 = ref this.m_SewageData.m_State.ValueAsRef<WaterPipeFlowJob.State>();
      reader.Read(out local2.m_LastTotalSteps);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      // ISSUE: reference to a compiler-generated field
      this.m_SourceNode = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_SinkNode = Entity.Null;
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_SourceNode == Entity.Null && this.m_SinkNode == Entity.Null)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SourceNode = entityManager.CreateEntity(this.m_NodeArchetype);
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SinkNode = entityManager.CreateEntity(this.m_NodeArchetype);
      }
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      // ISSUE: variable of a compiler-generated type
      DispatchWaterSystem systemManaged = this.World.GetOrCreateSystemManaged<DispatchWaterSystem>();
      if (context.version < Version.waterPipeFlowSim && context.purpose == Purpose.LoadGame)
      {
        Debug.LogWarning((object) "Detected legacy water pipes, disabling water & sewage notifications!");
        systemManaged.freshConsumptionDisabled = true;
        systemManaged.sewageConsumptionDisabled = true;
      }
      else
      {
        systemManaged.freshConsumptionDisabled = false;
        systemManaged.sewageConsumptionDisabled = false;
      }
      if (!(context.version < Version.waterPipePollution))
        return;
      entityManager = this.EntityManager;
      using (EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<WaterPipeNodeConnection>()))
      {
        NativeArray<Entity> entityArray = entityQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeArray<WaterPipeNodeConnection> componentDataArray = entityQuery.ToComponentDataArray<WaterPipeNodeConnection>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          if (componentDataArray[index].m_WaterPipeNode == Entity.Null)
          {
            COSystemBase.baseLog.WarnFormat("{0} has null WaterPipeNode! Removing...", (object) entityArray[index]);
            entityManager = this.EntityManager;
            entityManager.RemoveComponent<WaterPipeNodeConnection>(entityArray[index]);
          }
        }
      }
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
    public WaterPipeFlowSystem()
    {
    }

    private enum Phase
    {
      Prepare,
      Flow,
      Apply,
    }

    [BurstCompile]
    private struct PrepareNetworkJob : IJob
    {
      public NativeList<Game.Simulation.Flow.Node> m_FreshNodes;
      public NativeList<Game.Simulation.Flow.Node> m_SewageNodes;
      public NativeList<Game.Simulation.Flow.Edge> m_FreshEdges;
      public NativeList<Game.Simulation.Flow.Edge> m_SewageEdges;
      public NativeList<Game.Simulation.Flow.Connection> m_Connections;
      public NativeList<int> m_TradeNodes;
      public int m_NodeCount;
      public int m_EdgeCount;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_FreshNodes.ResizeUninitialized(this.m_NodeCount + 1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SewageNodes.ResizeUninitialized(this.m_NodeCount + 1);
        // ISSUE: reference to a compiler-generated field
        ref NativeList<Game.Simulation.Flow.Node> local1 = ref this.m_FreshNodes;
        // ISSUE: reference to a compiler-generated field
        ref NativeList<Game.Simulation.Flow.Node> local2 = ref this.m_SewageNodes;
        Game.Simulation.Flow.Node node1 = new Game.Simulation.Flow.Node();
        Game.Simulation.Flow.Node node2 = node1;
        local2[0] = node2;
        Game.Simulation.Flow.Node node3 = node1;
        local1[0] = node3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_FreshEdges.ResizeUninitialized(this.m_EdgeCount + 1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SewageEdges.ResizeUninitialized(this.m_EdgeCount + 1);
        // ISSUE: reference to a compiler-generated field
        ref NativeList<Game.Simulation.Flow.Edge> local3 = ref this.m_FreshEdges;
        // ISSUE: reference to a compiler-generated field
        ref NativeList<Game.Simulation.Flow.Edge> local4 = ref this.m_SewageEdges;
        Game.Simulation.Flow.Edge edge1 = new Game.Simulation.Flow.Edge();
        Game.Simulation.Flow.Edge edge2 = edge1;
        local4[0] = edge2;
        Game.Simulation.Flow.Edge edge3 = edge1;
        local3[0] = edge3;
        // ISSUE: reference to a compiler-generated field
        this.m_Connections.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Connections.Capacity = 2 * this.m_EdgeCount + 1;
        // ISSUE: reference to a compiler-generated field
        this.m_Connections.Add(new Game.Simulation.Flow.Connection());
        // ISSUE: reference to a compiler-generated field
        this.m_TradeNodes.Clear();
      }
    }

    [BurstCompile]
    private struct PrepareNodesJob : IJobChunk
    {
      public ComponentTypeHandle<WaterPipeNode> m_FlowNodeType;
      public int m_MaxChunkCapacity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        int num = unfilteredChunkIndex * this.m_MaxChunkCapacity;
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeNode> nativeArray = chunk.GetNativeArray<WaterPipeNode>(ref this.m_FlowNodeType);
        for (int index = 0; index < nativeArray.Length; ++index)
          nativeArray.ElementAt<WaterPipeNode>(index).m_Index = num + index + 1;
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
    private struct PrepareEdgesJob : IJobChunk
    {
      public ComponentTypeHandle<WaterPipeEdge> m_FlowEdgeType;
      [NativeDisableParallelForRestriction]
      public NativeArray<Game.Simulation.Flow.Edge> m_FreshEdges;
      [NativeDisableParallelForRestriction]
      public NativeArray<Game.Simulation.Flow.Edge> m_SewageEdges;
      public int m_MaxChunkCapacity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        int num = unfilteredChunkIndex * this.m_MaxChunkCapacity;
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeEdge> nativeArray = chunk.GetNativeArray<WaterPipeEdge>(ref this.m_FlowEdgeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          ref WaterPipeEdge local = ref nativeArray.ElementAt<WaterPipeEdge>(index1);
          int index2 = num + index1 + 1;
          // ISSUE: reference to a compiler-generated field
          this.m_FreshEdges[index2] = new Game.Simulation.Flow.Edge()
          {
            m_Capacity = local.m_FreshCapacity,
            m_Direction = FlowDirection.Both
          };
          // ISSUE: reference to a compiler-generated field
          this.m_SewageEdges[index2] = new Game.Simulation.Flow.Edge()
          {
            m_Capacity = local.m_SewageCapacity,
            m_Direction = FlowDirection.Both
          };
          local.m_Index = index2;
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
    private struct PrepareConnectionsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> m_ConnectedFlowEdgeType;
      [ReadOnly]
      public ComponentTypeHandle<TradeNode> m_TradeNodeType;
      [ReadOnly]
      public ComponentLookup<WaterPipeNode> m_FlowNodes;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      public NativeArray<Game.Simulation.Flow.Node> m_FreshNodes;
      public NativeArray<Game.Simulation.Flow.Node> m_SewageNodes;
      public NativeList<Game.Simulation.Flow.Connection> m_Connections;
      public NativeList<int> m_TradeNodes;
      public int m_MaxChunkCapacity;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        int num = unfilteredChunkIndex * this.m_MaxChunkCapacity;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedFlowEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedFlowEdge>(ref this.m_ConnectedFlowEdgeType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<TradeNode>(ref this.m_TradeNodeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray[index1];
          DynamicBuffer<ConnectedFlowEdge> dynamicBuffer = bufferAccessor[index1];
          int index2 = num + index1 + 1;
          // ISSUE: reference to a compiler-generated field
          ref Game.Simulation.Flow.Node local1 = ref this.m_FreshNodes.ElementAt<Game.Simulation.Flow.Node>(index2);
          // ISSUE: reference to a compiler-generated field
          ref Game.Simulation.Flow.Node local2 = ref this.m_SewageNodes.ElementAt<Game.Simulation.Flow.Node>(index2);
          // ISSUE: reference to a compiler-generated field
          local1.m_FirstConnection = local2.m_FirstConnection = this.m_Connections.Length;
          // ISSUE: reference to a compiler-generated field
          local1.m_LastConnection = local2.m_LastConnection = this.m_Connections.Length + dynamicBuffer.Length;
          foreach (ConnectedFlowEdge connectedFlowEdge in dynamicBuffer)
          {
            // ISSUE: reference to a compiler-generated field
            WaterPipeEdge flowEdge = this.m_FlowEdges[connectedFlowEdge.m_Edge];
            bool flag2 = flowEdge.m_End == entity;
            // ISSUE: reference to a compiler-generated field
            WaterPipeNode flowNode = this.m_FlowNodes[flag2 ? flowEdge.m_Start : flowEdge.m_End];
            // ISSUE: reference to a compiler-generated field
            this.m_Connections.Add(new Game.Simulation.Flow.Connection()
            {
              m_StartNode = index2,
              m_EndNode = flowNode.m_Index,
              m_Edge = flowEdge.m_Index,
              m_Backwards = flag2
            });
          }
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TradeNodes.Add(in index2);
          }
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
    private struct PopulateNodeIndicesJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<WaterPipeNode> m_FlowNodes;
      public NativeReference<NodeIndices> m_NodeIndices;
      public Entity m_SourceNode;
      public Entity m_SinkNode;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NodeIndices.Value = new NodeIndices()
        {
          m_SourceNode = this.m_FlowNodes[this.m_SourceNode].m_Index,
          m_SinkNode = this.m_FlowNodes[this.m_SinkNode].m_Index
        };
      }
    }

    [BurstCompile]
    private struct ApplyEdgesJob : IJobChunk
    {
      public ComponentTypeHandle<WaterPipeEdge> m_FlowEdgeType;
      [ReadOnly]
      public NativeArray<Game.Simulation.Flow.Edge> m_FreshEdges;
      [ReadOnly]
      public NativeArray<Game.Simulation.Flow.Edge> m_SewageEdges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeEdge> nativeArray = chunk.GetNativeArray<WaterPipeEdge>(ref this.m_FlowEdgeType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ref WaterPipeEdge local = ref nativeArray.ElementAt<WaterPipeEdge>(index);
          // ISSUE: reference to a compiler-generated field
          Game.Simulation.Flow.Edge freshEdge = this.m_FreshEdges[local.m_Index];
          // ISSUE: reference to a compiler-generated field
          Game.Simulation.Flow.Edge sewageEdge = this.m_SewageEdges[local.m_Index];
          local.m_FreshFlow = freshEdge.m_FinalFlow;
          local.m_SewageFlow = sewageEdge.m_FinalFlow;
          local.m_Flags = WaterPipeEdgeFlags.None;
          if (freshEdge.m_CutElementId.m_Version == -1)
            local.m_Flags |= WaterPipeEdgeFlags.WaterShortage;
          else if (freshEdge.m_CutElementId.m_Version != -2)
            local.m_Flags |= WaterPipeEdgeFlags.WaterDisconnected;
          if (sewageEdge.m_CutElementId.m_Version == -1)
            local.m_Flags |= WaterPipeEdgeFlags.SewageBackup;
          else if (sewageEdge.m_CutElementId.m_Version != -2)
            local.m_Flags |= WaterPipeEdgeFlags.SewageDisconnected;
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
      public ComponentTypeHandle<WaterPipeNode> __Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle;
      public ComponentTypeHandle<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TradeNode> __Game_Simulation_TradeNode_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<WaterPipeNode> __Game_Simulation_WaterPipeNode_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNode_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TradeNode_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TradeNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNode_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
      }
    }
  }
}
