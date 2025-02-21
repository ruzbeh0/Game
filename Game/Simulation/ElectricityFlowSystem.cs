// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityFlowSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Serialization;
using Game.Simulation.Flow;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ElectricityFlowSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    public const int kUpdateInterval = 128;
    public const int kUpdatesPerDay = 2048;
    public const int kUpdatesPerHour = 85;
    public const int kStartFrames = 2;
    public const int kAdjustFrame = 0;
    public const int kPrepareFrame = 1;
    public const int kFlowFrames = 124;
    public const int kFlowCompletionFrame = 125;
    public const int kEndFrames = 2;
    public const int kApplyFrame = 126;
    public const int kStatusFrame = 127;
    public const int kMaxEdgeCapacity = 1073741823;
    private const int kLayerHeight = 20;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_NodeGroup;
    private EntityQuery m_EdgeGroup;
    private EntityArchetype m_NodeArchetype;
    private EntityArchetype m_ChargeNodeArchetype;
    private EntityArchetype m_DischargeNodeArchetype;
    private EntityArchetype m_EdgeArchetype;
    private NativeList<Game.Simulation.Flow.Node> m_Nodes;
    private NativeList<Game.Simulation.Flow.Edge> m_Edges;
    private NativeList<Game.Simulation.Flow.Connection> m_Connections;
    private NativeReference<NodeIndices> m_NodeIndices;
    private NativeList<int> m_ChargeNodes;
    private NativeList<int> m_DischargeNodes;
    private NativeList<int> m_TradeNodes;
    private NativeReference<ElectricityFlowJob.State> m_FlowJobState;
    private NativeReference<MaxFlowSolverState> m_SolverState;
    private NativeList<LayerState> m_LayerStates;
    private NativeList<CutElement> m_LayerElements;
    private NativeList<CutElementRef> m_LayerElementRefs;
    private Entity m_SourceNode;
    private Entity m_SinkNode;
    private Entity m_LegacyOutsideSourceNode;
    private Entity m_LegacyOutsideSinkNode;
    private ElectricityFlowSystem.Phase m_NextPhase;
    private JobHandle m_DataDependency;
    private ElectricityFlowSystem.TypeHandle __TypeHandle;

    public bool ready { get; private set; }

    public EntityArchetype nodeArchetype => this.m_NodeArchetype;

    public EntityArchetype chargeNodeArchetype => this.m_ChargeNodeArchetype;

    public EntityArchetype dischargeNodeArchetype => this.m_DischargeNodeArchetype;

    public EntityArchetype edgeArchetype => this.m_EdgeArchetype;

    public Entity sourceNode => this.m_SourceNode;

    public Entity sinkNode => this.m_SinkNode;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeGroup = this.GetEntityQuery(ComponentType.ReadWrite<ElectricityFlowNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeGroup = this.GetEntityQuery(ComponentType.ReadWrite<ElectricityFlowEdge>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_NodeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<ElectricityFlowNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>());
      // ISSUE: reference to a compiler-generated field
      this.m_ChargeNodeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<ElectricityFlowNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>(), ComponentType.ReadOnly<BatteryChargeNode>());
      // ISSUE: reference to a compiler-generated field
      this.m_DischargeNodeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<ElectricityFlowNode>(), ComponentType.ReadOnly<ConnectedFlowEdge>(), ComponentType.ReadOnly<BatteryDischargeNode>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadOnly<ElectricityFlowEdge>());
      // ISSUE: reference to a compiler-generated field
      this.m_Nodes = new NativeList<Game.Simulation.Flow.Node>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Edges = new NativeList<Game.Simulation.Flow.Edge>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Connections = new NativeList<Game.Simulation.Flow.Connection>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_NodeIndices = new NativeReference<NodeIndices>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ChargeNodes = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DischargeNodes = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TradeNodes = new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FlowJobState = new NativeReference<ElectricityFlowJob.State>(new ElectricityFlowJob.State(20000), (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_SolverState = new NativeReference<MaxFlowSolverState>(new MaxFlowSolverState(), (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LayerStates = new NativeList<LayerState>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LayerElements = new NativeList<CutElement>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LayerElementRefs = new NativeList<CutElementRef>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_Nodes.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Edges.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Connections.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NodeIndices.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ChargeNodes.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DischargeNodes.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TradeNodes.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_FlowJobState.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_SolverState.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LayerStates.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LayerElements.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LayerElementRefs.Dispose();
      base.OnDestroy();
    }

    public void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = ElectricityFlowSystem.Phase.Prepare;
      // ISSUE: reference to a compiler-generated field
      this.m_FlowJobState.Value = new ElectricityFlowJob.State(20000);
      this.ready = false;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_NextPhase == ElectricityFlowSystem.Phase.Prepare)
      {
        // ISSUE: reference to a compiler-generated method
        this.PreparePhase();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NextPhase == ElectricityFlowSystem.Phase.Flow)
        {
          // ISSUE: reference to a compiler-generated method
          this.FlowPhase();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NextPhase != ElectricityFlowSystem.Phase.Apply)
            return;
          // ISSUE: reference to a compiler-generated method
          this.ApplyPhase();
        }
      }
    }

    private void PreparePhase()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex % 128U != 1U)
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
      JobHandle dependsOn = new ElectricityFlowSystem.PrepareNetworkJob()
      {
        m_Nodes = this.m_Nodes,
        m_Edges = this.m_Edges,
        m_Connections = this.m_Connections,
        m_ChargeNodes = this.m_ChargeNodes,
        m_DischargeNodes = this.m_DischargeNodes,
        m_TradeNodes = this.m_TradeNodes,
        m_NodeCount = num1,
        m_EdgeCount = num2
      }.Schedule<ElectricityFlowSystem.PrepareNetworkJob>(JobHandle.CombineDependencies(this.Dependency, this.m_DataDependency));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowNode_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job0_1 = new ElectricityFlowSystem.PrepareNodesJob()
      {
        m_FlowNodeType = this.__TypeHandle.__Game_Simulation_ElectricityFlowNode_RW_ComponentTypeHandle,
        m_MaxChunkCapacity = chunkCapacity1
      }.ScheduleParallel<ElectricityFlowSystem.PrepareNodesJob>(this.m_NodeGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job1_1 = new ElectricityFlowSystem.PrepareEdgesJob()
      {
        m_FlowEdgeType = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentTypeHandle,
        m_Edges = this.m_Edges.AsDeferredJobArray(),
        m_MaxChunkCapacity = chunkCapacity2
      }.ScheduleParallel<ElectricityFlowSystem.PrepareEdgesJob>(this.m_EdgeGroup, dependsOn);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowNode_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_TradeNode_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_BatteryDischargeNode_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_BatteryChargeNode_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job0_2 = new ElectricityFlowSystem.PrepareConnectionsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ConnectedFlowEdgeType = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle,
        m_ChargeNodeType = this.__TypeHandle.__Game_Simulation_BatteryChargeNode_RO_ComponentTypeHandle,
        m_DischargeNodeType = this.__TypeHandle.__Game_Simulation_BatteryDischargeNode_RO_ComponentTypeHandle,
        m_TradeNodeType = this.__TypeHandle.__Game_Simulation_TradeNode_RO_ComponentTypeHandle,
        m_FlowNodes = this.__TypeHandle.__Game_Simulation_ElectricityFlowNode_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_Nodes = this.m_Nodes.AsDeferredJobArray(),
        m_Connections = this.m_Connections,
        m_ChargeNodes = this.m_ChargeNodes,
        m_DischargeNodes = this.m_DischargeNodes,
        m_TradeNodes = this.m_TradeNodes,
        m_MaxChunkCapacity = chunkCapacity1
      }.Schedule<ElectricityFlowSystem.PrepareConnectionsJob>(this.m_NodeGroup, JobHandle.CombineDependencies(job0_1, job1_1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowNode_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle job1_2 = new ElectricityFlowSystem.PopulateNodeIndicesJob()
      {
        m_FlowNodes = this.__TypeHandle.__Game_Simulation_ElectricityFlowNode_RO_ComponentLookup,
        m_NodeIndices = this.m_NodeIndices,
        m_SourceNode = this.m_SourceNode,
        m_SinkNode = this.m_SinkNode
      }.Schedule<ElectricityFlowSystem.PopulateNodeIndicesJob>(JobHandle.CombineDependencies(job0_1, this.m_DataDependency));
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_DataDependency = JobHandle.CombineDependencies(job0_2, job1_2);
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = ElectricityFlowSystem.Phase.Flow;
    }

    private void FlowPhase()
    {
      // ISSUE: reference to a compiler-generated field
      uint num1 = this.m_SimulationSystem.frameIndex % 128U;
      int num2;
      switch (num1)
      {
        case 0:
        case 1:
        case 126:
          num2 = 0;
          break;
        default:
          num2 = num1 != (uint) sbyte.MaxValue ? 1 : 0;
          break;
      }
      Assert.IsTrue(num2 != 0);
      bool flag = num1 >= 125U;
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
      ElectricityFlowJob jobData = new ElectricityFlowJob()
      {
        m_State = this.m_FlowJobState,
        m_Nodes = this.m_Nodes.AsDeferredJobArray(),
        m_Edges = this.m_Edges.AsDeferredJobArray(),
        m_Connections = this.m_Connections.AsDeferredJobArray(),
        m_NodeIndices = this.m_NodeIndices,
        m_ChargeNodes = this.m_ChargeNodes.AsDeferredJobArray(),
        m_DischargeNodes = this.m_DischargeNodes.AsDeferredJobArray(),
        m_TradeNodes = this.m_TradeNodes.AsDeferredJobArray(),
        m_SolverState = this.m_SolverState,
        m_LayerStates = this.m_LayerStates,
        m_LayerElements = this.m_LayerElements,
        m_LayerElementRefs = this.m_LayerElementRefs,
        m_LabelQueue = new NativeQueue<int>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_LayerHeight = 20,
        m_FrameCount = 1,
        m_FinalFrame = flag
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = jobData.Schedule<ElectricityFlowJob>(this.m_DataDependency);
      jobData.m_LabelQueue.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency = inputDeps;
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = ElectricityFlowSystem.Phase.Apply;
    }

    private void ApplyPhase()
    {
      // ISSUE: reference to a compiler-generated field
      Assert.IsFalse(this.m_SimulationSystem.frameIndex % 128U > 126U);
      // ISSUE: reference to a compiler-generated field
      if (this.m_SimulationSystem.frameIndex % 128U != 126U)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_DataDependency = new ElectricityFlowSystem.ApplyEdgesJob()
      {
        m_FlowEdgeType = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentTypeHandle,
        m_Edges = this.m_Edges.AsDeferredJobArray()
      }.ScheduleParallel<ElectricityFlowSystem.ApplyEdgesJob>(this.m_EdgeGroup, JobHandle.CombineDependencies(this.Dependency, this.m_DataDependency));
      // ISSUE: reference to a compiler-generated field
      this.m_NextPhase = ElectricityFlowSystem.Phase.Prepare;
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
      writer.Write(this.m_FlowJobState.Value.m_LastTotalSteps);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DataDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_SourceNode);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_SinkNode);
      if (reader.context.version >= Version.electricityTrading && reader.context.version < Version.batteryRework2)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LegacyOutsideSourceNode);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LegacyOutsideSinkNode);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LegacyOutsideSourceNode = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_LegacyOutsideSinkNode = Entity.Null;
      }
      if (reader.context.version >= Version.waterElectricityID && reader.context.version < Version.electricityImprovements)
      {
        int num;
        reader.Read(out num);
        for (int index = 0; index < num; ++index)
          reader.Read(out Entity _);
        reader.Read(new NativeList<int>((AllocatorManager.AllocatorHandle) Allocator.Temp));
      }
      if (reader.context.version >= Version.electricityImprovements && reader.context.version < Version.electricityImprovements2)
        reader.Read(out int _);
      if (reader.context.version >= Version.electricityImprovements2 && reader.context.version < Version.flowJobImprovements)
      {
        reader.Read(out int _);
        reader.Read(out int _);
        reader.Read(out int _);
      }
      if (!(reader.context.version > Version.flowJobImprovements))
        return;
      // ISSUE: reference to a compiler-generated field
      ref ElectricityFlowJob.State local = ref this.m_FlowJobState.ValueAsRef<ElectricityFlowJob.State>();
      reader.Read(out local.m_LastTotalSteps);
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
      // ISSUE: reference to a compiler-generated field
      if (context.purpose == Purpose.NewMap || context.purpose == Purpose.NewGame && context.version < Version.timoSerializationFlow || context.purpose == Purpose.LoadMap && this.m_SourceNode == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SourceNode = this.EntityManager.CreateEntity(this.m_NodeArchetype);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_SinkNode = this.EntityManager.CreateEntity(this.m_NodeArchetype);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LegacyOutsideSourceNode != Entity.Null || this.m_LegacyOutsideSinkNode != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LegacyOutsideSourceNode != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          ElectricityGraphUtils.DeleteFlowNode(this.EntityManager, this.m_LegacyOutsideSourceNode);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_LegacyOutsideSinkNode != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          ElectricityGraphUtils.DeleteFlowNode(this.EntityManager, this.m_LegacyOutsideSinkNode);
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_EdgeGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityFlowEdge> componentDataArray1 = this.m_EdgeGroup.ToComponentDataArray<ElectricityFlowEdge>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        try
        {
          for (int index = 0; index < entityArray.Length; ++index)
          {
            Entity entity = entityArray[index];
            ElectricityFlowEdge electricityFlowEdge = componentDataArray1[index];
            EntityManager entityManager;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (electricityFlowEdge.m_Start == this.m_LegacyOutsideSourceNode || electricityFlowEdge.m_End == this.m_LegacyOutsideSourceNode)
            {
              entityManager = this.EntityManager;
              entityManager.AddComponent<Deleted>(entity);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (electricityFlowEdge.m_Start == this.m_LegacyOutsideSinkNode || electricityFlowEdge.m_End == this.m_LegacyOutsideSinkNode)
              {
                entityManager = this.EntityManager;
                entityManager.AddComponent<Deleted>(entity);
              }
            }
          }
        }
        finally
        {
          entityArray.Dispose();
          componentDataArray1.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LegacyOutsideSourceNode = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_LegacyOutsideSinkNode = Entity.Null;
        EntityQuery entityQuery = this.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<ElectricityOutsideConnection>(), ComponentType.ReadOnly<Owner>(), ComponentType.Exclude<Temp>());
        NativeArray<Owner> componentDataArray2 = entityQuery.ToComponentDataArray<Owner>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        try
        {
          foreach (Owner owner in componentDataArray2)
          {
            ElectricityNodeConnection component;
            if (this.EntityManager.TryGetComponent<ElectricityNodeConnection>(owner.m_Owner, out component))
            {
              this.EntityManager.AddComponent<TradeNode>(component.m_ElectricityNode);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ElectricityGraphUtils.CreateFlowEdge(this.EntityManager, this.m_EdgeArchetype, this.m_SourceNode, component.m_ElectricityNode, FlowDirection.None, 1073741823);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ElectricityGraphUtils.CreateFlowEdge(this.EntityManager, this.m_EdgeArchetype, component.m_ElectricityNode, this.m_SinkNode, FlowDirection.None, 1073741823);
            }
          }
        }
        finally
        {
          entityQuery.Dispose();
          componentDataArray2.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.Reset();
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
    public ElectricityFlowSystem()
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
      public NativeList<Game.Simulation.Flow.Node> m_Nodes;
      public NativeList<Game.Simulation.Flow.Edge> m_Edges;
      public NativeList<Game.Simulation.Flow.Connection> m_Connections;
      public NativeList<int> m_ChargeNodes;
      public NativeList<int> m_DischargeNodes;
      public NativeList<int> m_TradeNodes;
      public int m_NodeCount;
      public int m_EdgeCount;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Nodes.ResizeUninitialized(this.m_NodeCount + 1);
        // ISSUE: reference to a compiler-generated field
        this.m_Nodes[0] = new Game.Simulation.Flow.Node();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Edges.ResizeUninitialized(this.m_EdgeCount + 1);
        // ISSUE: reference to a compiler-generated field
        this.m_Edges[0] = new Game.Simulation.Flow.Edge();
        // ISSUE: reference to a compiler-generated field
        this.m_Connections.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Connections.Capacity = 2 * this.m_EdgeCount + 1;
        // ISSUE: reference to a compiler-generated field
        this.m_Connections.Add(new Game.Simulation.Flow.Connection());
        // ISSUE: reference to a compiler-generated field
        this.m_ChargeNodes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_DischargeNodes.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_TradeNodes.Clear();
      }
    }

    [BurstCompile]
    private struct PrepareNodesJob : IJobChunk
    {
      public ComponentTypeHandle<ElectricityFlowNode> m_FlowNodeType;
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
        NativeArray<ElectricityFlowNode> nativeArray = chunk.GetNativeArray<ElectricityFlowNode>(ref this.m_FlowNodeType);
        for (int index = 0; index < nativeArray.Length; ++index)
          nativeArray.ElementAt<ElectricityFlowNode>(index).m_Index = num + index + 1;
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
      public ComponentTypeHandle<ElectricityFlowEdge> m_FlowEdgeType;
      [NativeDisableParallelForRestriction]
      public NativeArray<Game.Simulation.Flow.Edge> m_Edges;
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
        NativeArray<ElectricityFlowEdge> nativeArray = chunk.GetNativeArray<ElectricityFlowEdge>(ref this.m_FlowEdgeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          ref ElectricityFlowEdge local = ref nativeArray.ElementAt<ElectricityFlowEdge>(index1);
          int index2 = num + index1 + 1;
          // ISSUE: reference to a compiler-generated field
          this.m_Edges[index2] = new Game.Simulation.Flow.Edge()
          {
            m_Capacity = local.m_Capacity,
            m_Direction = local.direction
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
      public ComponentTypeHandle<BatteryChargeNode> m_ChargeNodeType;
      [ReadOnly]
      public ComponentTypeHandle<BatteryDischargeNode> m_DischargeNodeType;
      [ReadOnly]
      public ComponentTypeHandle<TradeNode> m_TradeNodeType;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowNode> m_FlowNodes;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public NativeArray<Game.Simulation.Flow.Node> m_Nodes;
      public NativeList<Game.Simulation.Flow.Connection> m_Connections;
      public NativeList<int> m_ChargeNodes;
      public NativeList<int> m_DischargeNodes;
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
        bool flag1 = chunk.Has<BatteryChargeNode>(ref this.m_ChargeNodeType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<BatteryDischargeNode>(ref this.m_DischargeNodeType);
        // ISSUE: reference to a compiler-generated field
        bool flag3 = chunk.Has<TradeNode>(ref this.m_TradeNodeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray[index1];
          DynamicBuffer<ConnectedFlowEdge> dynamicBuffer = bufferAccessor[index1];
          int index2 = num + index1 + 1;
          // ISSUE: reference to a compiler-generated field
          ref Game.Simulation.Flow.Node local = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(index2);
          // ISSUE: reference to a compiler-generated field
          local.m_FirstConnection = this.m_Connections.Length;
          // ISSUE: reference to a compiler-generated field
          local.m_LastConnection = this.m_Connections.Length + dynamicBuffer.Length;
          foreach (ConnectedFlowEdge connectedFlowEdge in dynamicBuffer)
          {
            // ISSUE: reference to a compiler-generated field
            ElectricityFlowEdge flowEdge = this.m_FlowEdges[connectedFlowEdge.m_Edge];
            bool flag4 = flowEdge.m_End == entity;
            // ISSUE: reference to a compiler-generated field
            ElectricityFlowNode flowNode = this.m_FlowNodes[flag4 ? flowEdge.m_Start : flowEdge.m_End];
            // ISSUE: reference to a compiler-generated field
            this.m_Connections.Add(new Game.Simulation.Flow.Connection()
            {
              m_StartNode = index2,
              m_EndNode = flowNode.m_Index,
              m_Edge = flowEdge.m_Index,
              m_Backwards = flag4
            });
          }
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ChargeNodes.Add(in index2);
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_DischargeNodes.Add(in index2);
          }
          if (flag3)
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
      public ComponentLookup<ElectricityFlowNode> m_FlowNodes;
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
      public ComponentTypeHandle<ElectricityFlowEdge> m_FlowEdgeType;
      [ReadOnly]
      public NativeArray<Game.Simulation.Flow.Edge> m_Edges;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityFlowEdge> nativeArray = chunk.GetNativeArray<ElectricityFlowEdge>(ref this.m_FlowEdgeType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ref ElectricityFlowEdge local = ref nativeArray.ElementAt<ElectricityFlowEdge>(index);
          // ISSUE: reference to a compiler-generated field
          Game.Simulation.Flow.Edge edge = this.m_Edges[local.m_Index];
          local.m_Flow = edge.flow;
          local.m_Flags &= ElectricityFlowEdgeFlags.ForwardBackward;
          // ISSUE: reference to a compiler-generated method
          local.m_Flags |= this.GetBottleneckFlag(edge.m_CutElementId.m_Version);
        }
      }

      private ElectricityFlowEdgeFlags GetBottleneckFlag(int label)
      {
        switch (label)
        {
          case -3:
            return ElectricityFlowEdgeFlags.BeyondBottleneck;
          case -2:
            return ElectricityFlowEdgeFlags.Bottleneck;
          case -1:
            return ElectricityFlowEdgeFlags.None;
          default:
            return ElectricityFlowEdgeFlags.Disconnected;
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
      public ComponentTypeHandle<ElectricityFlowNode> __Game_Simulation_ElectricityFlowNode_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatteryChargeNode> __Game_Simulation_BatteryChargeNode_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BatteryDischargeNode> __Game_Simulation_BatteryDischargeNode_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TradeNode> __Game_Simulation_TradeNode_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowNode> __Game_Simulation_ElectricityFlowNode_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowNode_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityFlowNode>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityFlowEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_BatteryChargeNode_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatteryChargeNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_BatteryDischargeNode_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BatteryDischargeNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_TradeNode_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TradeNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowNode_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
      }
    }
  }
}
