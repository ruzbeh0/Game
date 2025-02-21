// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipeFlowJob
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Net;
using Game.Simulation.Flow;
using System;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [BurstCompile]
  public struct WaterPipeFlowJob : IJob
  {
    private const int kShortageNodeLabel = -1;
    private const int kConnectedNodeLabel = -2;
    public const int kShortageEdgeLabel = -1;
    public const int kConnectedEdgeLabel = -2;
    private const int kSinkEdgeLabel = -200;
    public NativeReference<WaterPipeFlowJob.State> m_State;
    public NativeArray<Game.Simulation.Flow.Node> m_Nodes;
    public NativeArray<Game.Simulation.Flow.Edge> m_Edges;
    [ReadOnly]
    public NativeArray<Game.Simulation.Flow.Connection> m_Connections;
    [ReadOnly]
    public NativeReference<NodeIndices> m_NodeIndices;
    [ReadOnly]
    public NativeArray<int> m_TradeNodes;
    public NativeReference<MaxFlowSolverState> m_MaxFlowState;
    public NativeList<LayerState> m_LayerStates;
    public NativeList<CutElement> m_LayerElements;
    public NativeList<CutElementRef> m_LayerElementRefs;
    public NativeReference<FluidFlowSolverState> m_FluidFlowState;
    public int m_ImportCapacity;
    public int m_ExportCapacity;
    public bool m_FluidFlowEnabled;
    public int m_LayerHeight;
    public int m_FrameCount;
    public bool m_FinalFrame;

    public void Execute()
    {
      ref WaterPipeFlowJob.State local = ref this.m_State.ValueAsRef<WaterPipeFlowJob.State>();
      if (local.m_Error)
      {
        if (!this.m_FinalFrame)
          return;
        Debug.LogError((object) string.Format("Water pipe solver error in phase: {0}", (object) local.m_Phase));
        this.Finalize(ref local);
      }
      else
      {
        local.m_Error = true;
        int num = math.max(100, local.m_LastTotalSteps / 124);
        int maxSteps = local.m_StepCounter + this.m_FrameCount * num;
        while (local.m_Phase != WaterPipeFlowJob.Phase.Complete && (this.m_FinalFrame || local.m_StepCounter < maxSteps))
          this.ExecutePhase(ref local, maxSteps);
        local.m_Error = false;
        if (!this.m_FinalFrame)
          return;
        this.Finalize(ref local);
      }
    }

    private void Finalize(ref WaterPipeFlowJob.State state)
    {
      state.m_Phase = WaterPipeFlowJob.Phase.Initial;
      state.m_LastTotalSteps = state.m_StepCounter;
      state.m_StepCounter = 0;
      state.m_Error = false;
    }

    private void ExecutePhase(ref WaterPipeFlowJob.State state, int maxSteps)
    {
      if (state.m_Phase == WaterPipeFlowJob.Phase.Initial)
        this.InitialPhase(ref state);
      else if (state.m_Phase == WaterPipeFlowJob.Phase.Producer)
        this.MaxFlowPhase(ref state, maxSteps, WaterPipeFlowJob.Phase.PostProducer);
      else if (state.m_Phase == WaterPipeFlowJob.Phase.PostProducer)
        this.PostProducerPhase(ref state);
      else if (state.m_Phase == WaterPipeFlowJob.Phase.Trade)
        this.MaxFlowPhase(ref state, maxSteps, WaterPipeFlowJob.Phase.PostTrade);
      else if (state.m_Phase == WaterPipeFlowJob.Phase.PostTrade)
      {
        this.PostTradePhase(ref state);
      }
      else
      {
        if (state.m_Phase != WaterPipeFlowJob.Phase.FluidFlow)
          throw new Exception("Invalid phase");
        this.FluidFlowPhase(ref state, maxSteps);
      }
    }

    private void InitialPhase(ref WaterPipeFlowJob.State state)
    {
      this.ResetMaxFlowState();
      state.m_Phase = WaterPipeFlowJob.Phase.Producer;
    }

    private void ResetMaxFlowState()
    {
      this.m_MaxFlowState.Value = new MaxFlowSolverState();
      this.m_LayerStates.Length = 0;
      this.m_LayerElements.Length = 0;
      this.m_LayerElementRefs.Length = 0;
    }

    private void MaxFlowPhase(
      ref WaterPipeFlowJob.State state,
      int maxSteps,
      WaterPipeFlowJob.Phase phaseAfterCompletion)
    {
      NativeList<Game.Simulation.Flow.Layer> nativeList = new NativeList<Game.Simulation.Flow.Layer>(this.m_LayerStates.Capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      NativeQueue<int> nativeQueue = new NativeQueue<int>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      NativeArray<UnsafeList<int>> nativeArray = new NativeArray<UnsafeList<int>>(this.m_LayerHeight, Allocator.Temp);
      for (int index = 0; index < this.m_LayerHeight; ++index)
        nativeArray[index] = new UnsafeList<int>(128, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      MaxFlowSolver maxFlowSolver = new MaxFlowSolver()
      {
        m_LayerHeight = this.m_LayerHeight,
        m_SourceNode = nodeIndices.m_SourceNode,
        m_SinkNode = nodeIndices.m_SinkNode,
        m_Nodes = this.m_Nodes,
        m_Edges = this.m_Edges,
        m_Connections = this.m_Connections,
        m_Layers = nativeList,
        m_LabelQueue = nativeQueue,
        m_ActiveQueue = nativeArray,
        m_StepCounter = state.m_StepCounter
      };
      if (this.m_MaxFlowState.Value.m_CurrentLabelVersion == 0)
      {
        maxFlowSolver.ResetNetwork();
        maxFlowSolver.InitializeState();
      }
      else
        maxFlowSolver.LoadState(this.m_MaxFlowState, this.m_LayerStates, this.m_LayerElements, this.m_LayerElementRefs);
      while (!maxFlowSolver.m_Complete && (this.m_FinalFrame || maxFlowSolver.m_StepCounter < maxSteps))
        maxFlowSolver.SolveNextLayer();
      maxFlowSolver.SaveState(this.m_MaxFlowState, this.m_LayerStates, this.m_LayerElements, this.m_LayerElementRefs);
      foreach (Game.Simulation.Flow.Layer layer in nativeList)
        layer.Dispose();
      nativeList.Dispose();
      foreach (UnsafeList<int> unsafeList in nativeArray)
        unsafeList.Dispose();
      nativeQueue.Dispose();
      nativeArray.Dispose();
      state.m_StepCounter = maxFlowSolver.m_StepCounter;
      if (!maxFlowSolver.m_Complete)
        return;
      state.m_Phase = phaseAfterCompletion;
    }

    private void PostProducerPhase(ref WaterPipeFlowJob.State state)
    {
      this.LabelShortages(ref state);
      this.EnableTradeConnections();
      this.ResetMaxFlowState();
      state.m_Phase = WaterPipeFlowJob.Phase.Trade;
    }

    private void LabelShortages(ref WaterPipeFlowJob.State state)
    {
      NativeQueue<int> labelQueue = new NativeQueue<int>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Game.Simulation.Flow.Node node = this.m_Nodes[nodeIndices.m_SinkNode];
      for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
      {
        Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
        if (connection.m_EndNode != nodeIndices.m_SourceNode)
        {
          if (this.m_Nodes[connection.m_EndNode].m_CutElementId.m_Version == -1)
            this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = new Identifier(connection.m_EndNode, -1);
          else if (connection.GetIncomingCapacity(this.m_Edges) > 0 && connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
          {
            this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = new Identifier(connection.m_EndNode, -1);
            this.LabelShortageSubGraph(ref state, connection.m_EndNode, labelQueue);
          }
        }
      }
      labelQueue.Dispose();
    }

    private void LabelShortageSubGraph(
      ref WaterPipeFlowJob.State state,
      int initialNodeIndex,
      NativeQueue<int> labelQueue)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Assert.IsTrue(labelQueue.IsEmpty());
      Identifier expected = new Identifier(initialNodeIndex, -1);
      Identifier identifier = new Identifier(initialNodeIndex, -1);
      this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(initialNodeIndex).m_CutElementId = expected;
      labelQueue.Enqueue(initialNodeIndex);
      int index;
      while (labelQueue.TryDequeue(out index))
      {
        ++state.m_StepCounter;
        Game.Simulation.Flow.Node node = this.m_Nodes[index];
        Assert.AreEqual<Identifier>(expected, node.m_CutElementId);
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          int endNode = connection.m_EndNode;
          if (connection.GetIncomingCapacity(this.m_Edges) > 0)
          {
            this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = identifier;
            if (endNode != nodeIndices.m_SinkNode && endNode != nodeIndices.m_SourceNode)
            {
              ref Game.Simulation.Flow.Node local = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(endNode);
              if (local.m_CutElementId.m_Version != -1 && connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
              {
                local.m_CutElementId = expected;
                labelQueue.Enqueue(endNode);
              }
            }
          }
        }
      }
    }

    private void EnableTradeConnections()
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      foreach (int tradeNode in this.m_TradeNodes)
      {
        Game.Simulation.Flow.Node node = this.m_Nodes[tradeNode];
        bool flag = node.m_CutElementId.m_Version == -1;
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
          if (connection.m_EndNode == nodeIndices.m_SourceNode)
          {
            Assert.IsTrue(connection.m_Backwards);
            local.m_Capacity = flag ? this.m_ImportCapacity : 0;
          }
          else if (connection.m_EndNode == nodeIndices.m_SinkNode)
          {
            Assert.IsFalse(connection.m_Backwards);
            local.m_Capacity = flag ? 0 : this.m_ExportCapacity;
          }
        }
      }
    }

    private void PostTradePhase(ref WaterPipeFlowJob.State state)
    {
      this.SetTradeConnectionsEnabled(true, false);
      this.LabelConnected(ref state);
      this.LabelShortages(ref state);
      if (this.m_FluidFlowEnabled)
      {
        FluidFlowSolver.ResetNodes(this.m_Nodes);
        this.LimitImportEdgeCapacity();
        this.PreflowSinkEdges();
        this.ResetNonSinkEdges();
        this.m_FluidFlowState.Value = new FluidFlowSolverState();
        state.m_Phase = WaterPipeFlowJob.Phase.FluidFlow;
      }
      else
      {
        this.FinalizeFlows();
        state.m_Phase = WaterPipeFlowJob.Phase.Complete;
      }
    }

    private void SetTradeConnectionsEnabled(bool import, bool export)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      foreach (int tradeNode in this.m_TradeNodes)
      {
        Game.Simulation.Flow.Node node = this.m_Nodes[tradeNode];
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
          if (connection.m_EndNode == nodeIndices.m_SourceNode)
          {
            Assert.IsTrue(connection.m_Backwards);
            local.m_Capacity = import ? this.m_ImportCapacity : 0;
          }
          else if (connection.m_EndNode == nodeIndices.m_SinkNode)
          {
            Assert.IsFalse(connection.m_Backwards);
            local.m_Capacity = export ? this.m_ExportCapacity : 0;
          }
        }
      }
    }

    private void LabelConnected(ref WaterPipeFlowJob.State state)
    {
      NativeQueue<int> labelQueue = new NativeQueue<int>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      Game.Simulation.Flow.Node node = this.m_Nodes[this.m_NodeIndices.Value.m_SourceNode];
      for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
      {
        Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
        if (this.m_Nodes[connection.m_EndNode].m_CutElementId.m_Version == -2)
          this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = new Identifier(connection.m_EndNode, -2);
        else if (connection.GetOutgoingCapacity(this.m_Edges) > 0)
        {
          this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = new Identifier(connection.m_EndNode, -2);
          this.LabelConnectedSubGraph(ref state, connection.m_EndNode, labelQueue);
        }
      }
      labelQueue.Dispose();
    }

    private void LabelConnectedSubGraph(
      ref WaterPipeFlowJob.State state,
      int initialNodeIndex,
      NativeQueue<int> labelQueue)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Assert.IsTrue(labelQueue.IsEmpty());
      Identifier expected = new Identifier(initialNodeIndex, -2);
      Identifier identifier = new Identifier(initialNodeIndex, -2);
      this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(initialNodeIndex).m_CutElementId = expected;
      labelQueue.Enqueue(initialNodeIndex);
      int index;
      while (labelQueue.TryDequeue(out index))
      {
        ++state.m_StepCounter;
        Game.Simulation.Flow.Node node = this.m_Nodes[index];
        Assert.AreEqual<Identifier>(expected, node.m_CutElementId);
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          int endNode = connection.m_EndNode;
          if (connection.GetOutgoingCapacity(this.m_Edges) > 0 || endNode == nodeIndices.m_SinkNode)
          {
            this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = identifier;
            if (endNode != nodeIndices.m_SinkNode && endNode != nodeIndices.m_SourceNode)
            {
              ref Game.Simulation.Flow.Node local = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(endNode);
              if (local.m_CutElementId.m_Version != -2)
              {
                local.m_CutElementId = expected;
                labelQueue.Enqueue(endNode);
              }
            }
          }
        }
      }
    }

    private void LimitImportEdgeCapacity()
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      foreach (int tradeNode in this.m_TradeNodes)
      {
        Game.Simulation.Flow.Node node = this.m_Nodes[tradeNode];
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          if (connection.m_EndNode == nodeIndices.m_SourceNode)
          {
            ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
            Assert.IsTrue(local.flow >= 0);
            local.m_Capacity = local.flow;
          }
        }
      }
    }

    private void PreflowSinkEdges()
    {
      ref Game.Simulation.Flow.Node local1 = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(this.m_NodeIndices.Value.m_SinkNode);
      for (int firstConnection = local1.m_FirstConnection; firstConnection < local1.m_LastConnection; ++firstConnection)
      {
        Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
        ref Game.Simulation.Flow.Edge local2 = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
        Assert.IsTrue(local2.flow >= 0);
        local2.m_Direction = FlowDirection.None;
        local2.m_CutElementId = new Identifier(-200, local2.m_CutElementId.m_Version);
        local2.FinalizeTempFlow();
        this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(connection.m_EndNode).m_Excess = local2.flow;
        local1.m_Excess -= local2.flow;
      }
    }

    private void ResetNonSinkEdges()
    {
      for (int index = 0; index < this.m_Edges.Length; ++index)
      {
        ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(index);
        if (local.m_CutElementId.m_Index != -200)
        {
          local.m_FinalFlow = 0;
          local.m_TempFlow = 0;
        }
      }
    }

    private void FinalizeFlows()
    {
      for (int index = 0; index < this.m_Edges.Length; ++index)
        this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(index).FinalizeTempFlow();
    }

    private void FluidFlowPhase(ref WaterPipeFlowJob.State state, int maxSteps)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      FluidFlowSolver fluidFlowSolver = new FluidFlowSolver()
      {
        m_SourceNode = nodeIndices.m_SourceNode,
        m_SinkNode = nodeIndices.m_SinkNode,
        m_Nodes = this.m_Nodes,
        m_Edges = this.m_Edges,
        m_Connections = this.m_Connections,
        m_LabelQueue = new NativeMinHeap<LabelHeapData>(256, Allocator.Temp),
        m_PushQueue = new NativeMinHeap<PushHeapData>(2048, Allocator.Temp),
        m_StepCounter = state.m_StepCounter
      };
      if (this.m_FluidFlowState.Value.m_CurrentVersion == 0)
        fluidFlowSolver.InitializeState();
      else
        fluidFlowSolver.LoadState(this.m_FluidFlowState);
      while (!fluidFlowSolver.m_Complete && (this.m_FinalFrame || fluidFlowSolver.m_StepCounter < maxSteps))
        fluidFlowSolver.SolveStep();
      fluidFlowSolver.SaveState(this.m_FluidFlowState);
      fluidFlowSolver.m_LabelQueue.Dispose();
      fluidFlowSolver.m_PushQueue.Dispose();
      state.m_StepCounter = fluidFlowSolver.m_StepCounter;
      if (!fluidFlowSolver.m_Complete)
        return;
      state.m_Phase = WaterPipeFlowJob.Phase.Complete;
    }

    public enum Phase
    {
      Initial,
      Producer,
      PostProducer,
      Trade,
      PostTrade,
      FluidFlow,
      Complete,
    }

    public struct State
    {
      public WaterPipeFlowJob.Phase m_Phase;
      public int m_LastTotalSteps;
      public int m_StepCounter;
      public bool m_Error;

      public State(int lastTotalSteps)
        : this()
      {
        this.m_Phase = WaterPipeFlowJob.Phase.Initial;
        this.m_LastTotalSteps = lastTotalSteps;
      }
    }

    public struct Data : IDisposable
    {
      public NativeReference<WaterPipeFlowJob.State> m_State;
      public NativeList<Game.Simulation.Flow.Node> m_Nodes;
      public NativeList<Game.Simulation.Flow.Edge> m_Edges;
      public NativeReference<MaxFlowSolverState> m_MaxFlowState;
      public NativeList<LayerState> m_LayerStates;
      public NativeList<CutElement> m_LayerElements;
      public NativeList<CutElementRef> m_LayerElementRefs;
      public NativeReference<FluidFlowSolverState> m_FluidFlowState;

      public Data(int lastTotalSteps, Allocator allocator)
      {
        this.m_State = new NativeReference<WaterPipeFlowJob.State>(new WaterPipeFlowJob.State(lastTotalSteps), (AllocatorManager.AllocatorHandle) allocator);
        this.m_Nodes = new NativeList<Game.Simulation.Flow.Node>((AllocatorManager.AllocatorHandle) allocator);
        this.m_Edges = new NativeList<Game.Simulation.Flow.Edge>((AllocatorManager.AllocatorHandle) allocator);
        this.m_MaxFlowState = new NativeReference<MaxFlowSolverState>(new MaxFlowSolverState(), (AllocatorManager.AllocatorHandle) allocator);
        this.m_LayerStates = new NativeList<LayerState>((AllocatorManager.AllocatorHandle) allocator);
        this.m_LayerElements = new NativeList<CutElement>((AllocatorManager.AllocatorHandle) allocator);
        this.m_LayerElementRefs = new NativeList<CutElementRef>((AllocatorManager.AllocatorHandle) allocator);
        this.m_FluidFlowState = new NativeReference<FluidFlowSolverState>(new FluidFlowSolverState(), (AllocatorManager.AllocatorHandle) allocator);
      }

      public void Dispose()
      {
        this.m_State.Dispose();
        this.m_Nodes.Dispose();
        this.m_Edges.Dispose();
        this.m_MaxFlowState.Dispose();
        this.m_LayerStates.Dispose();
        this.m_LayerElements.Dispose();
        this.m_LayerElementRefs.Dispose();
        this.m_FluidFlowState.Dispose();
      }
    }
  }
}
