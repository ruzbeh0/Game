// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityFlowJob
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Net;
using Game.Simulation.Flow;
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
  public struct ElectricityFlowJob : IJob
  {
    private const int kConnectedNodeLabel = -1;
    private const int kShortageNodeLabel = -2;
    private const int kBeforeBottleneckNodeLabel = -3;
    private const int kBeyondBottleneckNodeLabel = -4;
    public const int kConnectedEdgeLabel = -1;
    public const int kBottleneckEdgeLabel = -2;
    public const int kBeyondBottleneckEdgeLabel = -3;
    public NativeReference<ElectricityFlowJob.State> m_State;
    public NativeArray<Game.Simulation.Flow.Node> m_Nodes;
    public NativeArray<Game.Simulation.Flow.Edge> m_Edges;
    [ReadOnly]
    public NativeArray<Game.Simulation.Flow.Connection> m_Connections;
    [ReadOnly]
    public NativeReference<NodeIndices> m_NodeIndices;
    [ReadOnly]
    public NativeArray<int> m_ChargeNodes;
    [ReadOnly]
    public NativeArray<int> m_DischargeNodes;
    [ReadOnly]
    public NativeArray<int> m_TradeNodes;
    public NativeReference<MaxFlowSolverState> m_SolverState;
    public NativeList<LayerState> m_LayerStates;
    public NativeList<CutElement> m_LayerElements;
    public NativeList<CutElementRef> m_LayerElementRefs;
    public NativeQueue<int> m_LabelQueue;
    public int m_LayerHeight;
    public int m_FrameCount;
    public bool m_FinalFrame;

    public void Execute()
    {
      ref ElectricityFlowJob.State local = ref this.m_State.ValueAsRef<ElectricityFlowJob.State>();
      if (local.m_Error)
      {
        if (!this.m_FinalFrame)
          return;
        Debug.LogError((object) string.Format("Electricity solver error in phase: {0}", (object) local.m_Phase));
        this.Finalize(ref local);
      }
      else
      {
        local.m_Error = true;
        int num = math.max(100, local.m_LastTotalSteps / 124);
        int maxSteps = local.m_StepCounter + this.m_FrameCount * num;
        while (local.m_Phase != ElectricityFlowJob.Phase.Complete && (this.m_FinalFrame || local.m_StepCounter < maxSteps))
          this.ExecutePhase(ref local, maxSteps);
        local.m_Error = false;
        if (!this.m_FinalFrame)
          return;
        this.Finalize(ref local);
      }
    }

    private void Finalize(ref ElectricityFlowJob.State state)
    {
      state.m_Phase = ElectricityFlowJob.Phase.Initial;
      state.m_LastTotalSteps = state.m_StepCounter;
      state.m_StepCounter = 0;
      state.m_Error = false;
    }

    private void ExecutePhase(ref ElectricityFlowJob.State state, int maxSteps)
    {
      if (state.m_Phase == ElectricityFlowJob.Phase.Initial)
        this.InitialPhase(ref state);
      else if (state.m_Phase == ElectricityFlowJob.Phase.Producer)
        this.MaxFlowPhase(ref state, maxSteps, ElectricityFlowJob.Phase.PostProducer);
      else if (state.m_Phase == ElectricityFlowJob.Phase.PostProducer)
        this.PostProducerPhase(ref state);
      else if (state.m_Phase == ElectricityFlowJob.Phase.Battery)
        this.MaxFlowPhase(ref state, maxSteps, ElectricityFlowJob.Phase.PostBattery);
      else if (state.m_Phase == ElectricityFlowJob.Phase.PostBattery)
        this.PostBatteryPhase(ref state);
      else if (state.m_Phase == ElectricityFlowJob.Phase.Trade)
      {
        this.MaxFlowPhase(ref state, maxSteps, ElectricityFlowJob.Phase.PostTrade);
      }
      else
      {
        if (state.m_Phase != ElectricityFlowJob.Phase.PostTrade)
          return;
        this.PostTradePhase(ref state);
      }
    }

    private void InitialPhase(ref ElectricityFlowJob.State state)
    {
      this.ResetSolverState();
      state.m_Phase = ElectricityFlowJob.Phase.Producer;
    }

    private void ResetSolverState()
    {
      this.m_SolverState.Value = new MaxFlowSolverState();
      this.m_LayerStates.Length = 0;
      this.m_LayerElements.Length = 0;
      this.m_LayerElementRefs.Length = 0;
    }

    private void MaxFlowPhase(
      ref ElectricityFlowJob.State state,
      int maxSteps,
      ElectricityFlowJob.Phase phaseAfterCompletion)
    {
      NativeList<Game.Simulation.Flow.Layer> nativeList = new NativeList<Game.Simulation.Flow.Layer>(this.m_LayerStates.Capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
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
        m_LabelQueue = this.m_LabelQueue,
        m_ActiveQueue = nativeArray,
        m_StepCounter = state.m_StepCounter
      };
      if (this.m_SolverState.Value.m_CurrentLabelVersion == 0)
      {
        maxFlowSolver.ResetNetwork();
        maxFlowSolver.InitializeState();
      }
      else
        maxFlowSolver.LoadState(this.m_SolverState, this.m_LayerStates, this.m_LayerElements, this.m_LayerElementRefs);
      while (!maxFlowSolver.m_Complete && (this.m_FinalFrame || maxFlowSolver.m_StepCounter < maxSteps))
        maxFlowSolver.SolveNextLayer();
      maxFlowSolver.SaveState(this.m_SolverState, this.m_LayerStates, this.m_LayerElements, this.m_LayerElementRefs);
      foreach (Game.Simulation.Flow.Layer layer in nativeList)
        layer.Dispose();
      nativeList.Dispose();
      foreach (UnsafeList<int> unsafeList in nativeArray)
        unsafeList.Dispose();
      nativeArray.Dispose();
      state.m_StepCounter = maxFlowSolver.m_StepCounter;
      if (!maxFlowSolver.m_Complete)
        return;
      state.m_Phase = phaseAfterCompletion;
    }

    private void PostProducerPhase(ref ElectricityFlowJob.State state)
    {
      this.LabelShortages(ref state);
      this.EnableDischargeConnectionsIfShortage();
      this.EnableChargeConnectionsIfNoShortage();
      this.ResetSolverState();
      state.m_Phase = ElectricityFlowJob.Phase.Battery;
    }

    private void LabelShortages(ref ElectricityFlowJob.State state)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Game.Simulation.Flow.Node node = this.m_Nodes[nodeIndices.m_SinkNode];
      for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
      {
        Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
        if (connection.m_EndNode != nodeIndices.m_SourceNode && this.m_Nodes[connection.m_EndNode].m_CutElementId.m_Version >= 0 && connection.GetIncomingCapacity(this.m_Edges) > 0 && connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
          this.LabelShortageSubGraph(ref state, connection.m_EndNode);
      }
    }

    private void LabelShortageSubGraph(ref ElectricityFlowJob.State state, int initialNodeIndex)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Assert.IsTrue(this.m_LabelQueue.IsEmpty());
      Identifier expected = new Identifier(initialNodeIndex, -2);
      this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(initialNodeIndex).m_CutElementId = expected;
      this.m_LabelQueue.Enqueue(initialNodeIndex);
      int index;
      while (this.m_LabelQueue.TryDequeue(out index))
      {
        ++state.m_StepCounter;
        Game.Simulation.Flow.Node node = this.m_Nodes[index];
        Assert.AreEqual<Identifier>(expected, node.m_CutElementId);
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          int endNode = connection.m_EndNode;
          if (endNode != nodeIndices.m_SinkNode && endNode != nodeIndices.m_SourceNode && connection.GetIncomingCapacity(this.m_Edges) > 0)
          {
            ref Game.Simulation.Flow.Node local = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(endNode);
            if (local.m_CutElementId.m_Version != -2 && connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
            {
              local.m_CutElementId = expected;
              this.m_LabelQueue.Enqueue(endNode);
            }
          }
        }
      }
    }

    private void EnableDischargeConnectionsIfShortage()
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      foreach (int dischargeNode in this.m_DischargeNodes)
      {
        Game.Simulation.Flow.Node node1 = this.m_Nodes[dischargeNode];
        for (int firstConnection = node1.m_FirstConnection; firstConnection < node1.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
          Game.Simulation.Flow.Node node2 = this.m_Nodes[connection.m_EndNode];
          Assert.IsTrue(local.m_Direction == FlowDirection.None);
          if (connection.m_EndNode != nodeIndices.m_SourceNode)
          {
            Assert.IsFalse(connection.m_Backwards);
            if (node2.m_CutElementId.m_Version == -2)
              local.m_Direction = FlowDirection.Forward;
          }
          else
          {
            Assert.IsTrue(connection.m_Backwards);
            local.m_Direction = FlowDirection.Forward;
          }
        }
      }
    }

    private void EnableChargeConnectionsIfNoShortage()
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      foreach (int chargeNode in this.m_ChargeNodes)
      {
        Game.Simulation.Flow.Node node1 = this.m_Nodes[chargeNode];
        for (int firstConnection = node1.m_FirstConnection; firstConnection < node1.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
          Game.Simulation.Flow.Node node2 = this.m_Nodes[connection.m_EndNode];
          Assert.IsTrue(local.m_Direction == FlowDirection.None);
          if (connection.m_EndNode != nodeIndices.m_SinkNode)
          {
            Assert.IsTrue(connection.m_Backwards);
            if (node2.m_CutElementId.m_Version != -2)
              local.m_Direction = FlowDirection.Forward;
          }
          else
          {
            Assert.IsFalse(connection.m_Backwards);
            local.m_Direction = FlowDirection.Forward;
          }
        }
      }
    }

    private void PostBatteryPhase(ref ElectricityFlowJob.State state)
    {
      this.DisableConnections(this.m_ChargeNodes);
      this.DisableConnections(this.m_DischargeNodes);
      this.LabelShortages(ref state);
      this.EnableTradeConnections();
      this.ResetSolverState();
      state.m_Phase = ElectricityFlowJob.Phase.Trade;
    }

    private void DisableConnections(NativeArray<int> nodes)
    {
      foreach (int node1 in nodes)
      {
        Game.Simulation.Flow.Node node2 = this.m_Nodes[node1];
        for (int firstConnection = node2.m_FirstConnection; firstConnection < node2.m_LastConnection; ++firstConnection)
          this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(this.m_Connections[firstConnection].m_Edge).m_Direction = FlowDirection.None;
      }
    }

    private void EnableTradeConnections()
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      foreach (int tradeNode in this.m_TradeNodes)
      {
        Game.Simulation.Flow.Node node = this.m_Nodes[tradeNode];
        bool flag = node.m_CutElementId.m_Version == -2;
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          ref Game.Simulation.Flow.Edge local = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
          if (connection.m_EndNode == nodeIndices.m_SourceNode)
          {
            Assert.IsTrue(connection.m_Backwards);
            local.m_Direction = flag ? FlowDirection.Forward : FlowDirection.None;
          }
          else if (connection.m_EndNode == nodeIndices.m_SinkNode)
          {
            Assert.IsFalse(connection.m_Backwards);
            local.m_Direction = flag ? FlowDirection.None : FlowDirection.Forward;
          }
        }
      }
    }

    private void PostTradePhase(ref ElectricityFlowJob.State state)
    {
      this.SetTradeConnectionsEnabled(true, false);
      this.LabelConnected(ref state);
      this.LabelBottlenecks(ref state);
      state.m_Phase = ElectricityFlowJob.Phase.Complete;
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
            local.m_Direction = import ? FlowDirection.Forward : FlowDirection.None;
          }
          else if (connection.m_EndNode == nodeIndices.m_SinkNode)
          {
            Assert.IsFalse(connection.m_Backwards);
            local.m_Direction = export ? FlowDirection.Forward : FlowDirection.None;
          }
        }
      }
    }

    private void LabelConnected(ref ElectricityFlowJob.State state)
    {
      Game.Simulation.Flow.Node node = this.m_Nodes[this.m_NodeIndices.Value.m_SourceNode];
      for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
      {
        Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
        if (this.m_Nodes[connection.m_EndNode].m_CutElementId.m_Version == -1)
          this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = new Identifier(connection.m_EndNode, -1);
        else if (connection.GetOutgoingCapacity(this.m_Edges) > 0)
        {
          this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge).m_CutElementId = new Identifier(connection.m_EndNode, -1);
          this.LabelConnectedSubGraph(ref state, connection.m_EndNode);
        }
      }
    }

    private void LabelConnectedSubGraph(ref ElectricityFlowJob.State state, int initialNodeIndex)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Assert.IsTrue(this.m_LabelQueue.IsEmpty());
      Identifier expected = new Identifier(initialNodeIndex, -1);
      Identifier identifier = new Identifier(initialNodeIndex, -1);
      this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(initialNodeIndex).m_CutElementId = expected;
      this.m_LabelQueue.Enqueue(initialNodeIndex);
      int index;
      while (this.m_LabelQueue.TryDequeue(out index))
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
              if (local.m_CutElementId.m_Version != -1)
              {
                local.m_CutElementId = expected;
                this.m_LabelQueue.Enqueue(endNode);
              }
            }
          }
        }
      }
    }

    private void LabelBottlenecks(ref ElectricityFlowJob.State state)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      ref Game.Simulation.Flow.Node local = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(nodeIndices.m_SinkNode);
      local.m_CutElementId = new Identifier(nodeIndices.m_SinkNode, -2);
      for (int firstConnection = local.m_FirstConnection; firstConnection < local.m_LastConnection; ++firstConnection)
      {
        Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
        if (connection.m_EndNode != nodeIndices.m_SourceNode && this.m_Nodes[connection.m_EndNode].m_CutElementId.m_Version > -2 && connection.GetIncomingCapacity(this.m_Edges) > 0 && connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
          this.LabelBottleneckSubGraph(ref state, connection.m_EndNode);
      }
    }

    private void LabelBottleneckSubGraph(ref ElectricityFlowJob.State state, int initialNodeIndex)
    {
      NodeIndices nodeIndices = this.m_NodeIndices.Value;
      Assert.IsTrue(this.m_LabelQueue.IsEmpty());
      int num1 = 0;
      int num2 = 0;
      bool flag1 = false;
      Identifier expected = new Identifier(initialNodeIndex, -2);
      Identifier identifier1 = new Identifier(initialNodeIndex, -2);
      ref Game.Simulation.Flow.Node local1 = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(initialNodeIndex);
      local1.m_CutElementId = expected;
      this.m_LabelQueue.Enqueue(initialNodeIndex);
      int index1;
      while (this.m_LabelQueue.TryDequeue(out index1))
      {
        ++state.m_StepCounter;
        Game.Simulation.Flow.Node node = this.m_Nodes[index1];
        Assert.AreEqual<Identifier>(expected, node.m_CutElementId);
        for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
        {
          Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
          int endNode = connection.m_EndNode;
          ref Game.Simulation.Flow.Edge local2 = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
          if (endNode != nodeIndices.m_SinkNode)
          {
            if (connection.GetIncomingCapacity(this.m_Edges) > 0)
            {
              if (endNode != nodeIndices.m_SourceNode)
              {
                ref Game.Simulation.Flow.Node local3 = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(endNode);
                if (local2.m_CutElementId.m_Version == -2)
                {
                  Assert.AreEqual(0, connection.GetOutgoingResidualCapacity(this.m_Edges));
                  Assert.IsTrue(connection.GetIncomingResidualCapacity(this.m_Edges) > 0);
                  if (local2.m_CutElementId.m_Index == initialNodeIndex)
                  {
                    Assert.AreEqual(initialNodeIndex, local3.m_CutElementId.m_Index);
                    Assert.AreEqual(-2, local3.m_CutElementId.m_Version);
                    local2.m_CutElementId = Identifier.Null;
                    --num2;
                  }
                  else
                    Assert.AreNotEqual(initialNodeIndex, local3.m_CutElementId.m_Index);
                }
                else if (local3.m_CutElementId != expected)
                {
                  if (connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
                  {
                    if (local3.m_CutElementId.m_Version >= -1)
                    {
                      local3.m_CutElementId = expected;
                      this.m_LabelQueue.Enqueue(endNode);
                    }
                  }
                  else
                  {
                    local2.m_CutElementId = identifier1;
                    ++num2;
                  }
                }
              }
              else
              {
                Assert.AreEqual(0, connection.GetIncomingResidualCapacity(this.m_Edges));
                local2.m_CutElementId = identifier1;
                flag1 = true;
              }
            }
          }
          else
          {
            Assert.AreNotEqual(-2, local2.m_CutElementId.m_Version);
            num1 += connection.GetOutgoingResidualCapacity(this.m_Edges);
          }
        }
      }
      Assert.IsTrue(num1 > 0);
      if (num2 <= 0 || flag1)
        return;
      Identifier identifier2 = new Identifier(initialNodeIndex, -3);
      Identifier identifier3 = new Identifier(initialNodeIndex, -4);
      Identifier identifier4 = new Identifier(initialNodeIndex, -3);
      local1.m_CutElementId = identifier3;
      this.m_LabelQueue.Enqueue(initialNodeIndex);
      int index2;
      while (this.m_LabelQueue.TryDequeue(out index2))
      {
        ++state.m_StepCounter;
        Game.Simulation.Flow.Node node = this.m_Nodes[index2];
        Assert.IsTrue(node.m_CutElementId == identifier2 || node.m_CutElementId == identifier3);
        if (node.m_CutElementId == identifier3)
        {
          for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
          {
            Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
            int endNode = connection.m_EndNode;
            ref Game.Simulation.Flow.Edge local4 = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
            if (endNode != nodeIndices.m_SinkNode)
            {
              if (connection.GetIncomingCapacity(this.m_Edges) > 0)
              {
                Assert.AreNotEqual(nodeIndices.m_SourceNode, endNode);
                ref Game.Simulation.Flow.Node local5 = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(endNode);
                if (local4.m_CutElementId != identifier1)
                {
                  local4.m_CutElementId = identifier4;
                  if (local5.m_CutElementId == expected)
                  {
                    local5.m_CutElementId = identifier3;
                    this.m_LabelQueue.Enqueue(endNode);
                  }
                }
                else if (endNode != nodeIndices.m_SourceNode)
                {
                  Assert.AreNotEqual<Identifier>(expected, local5.m_CutElementId);
                  local5.m_CutElementId = identifier2;
                  this.m_LabelQueue.Enqueue(endNode);
                }
              }
            }
            else if (connection.GetOutgoingCapacity(this.m_Edges) > 0)
              local4.m_CutElementId = identifier4;
          }
        }
        else
        {
          bool flag2 = true;
          for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
          {
            Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
            if (connection.m_EndNode != nodeIndices.m_SinkNode && connection.GetIncomingCapacity(this.m_Edges) > 0 && this.m_Edges[connection.m_Edge].m_CutElementId != identifier1 && connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
            {
              flag2 = false;
              break;
            }
          }
          if (flag2)
          {
            for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
            {
              Game.Simulation.Flow.Connection connection = this.m_Connections[firstConnection];
              int endNode = connection.m_EndNode;
              if (endNode != nodeIndices.m_SinkNode && connection.GetIncomingCapacity(this.m_Edges) > 0)
              {
                ref Game.Simulation.Flow.Edge local6 = ref this.m_Edges.ElementAt<Game.Simulation.Flow.Edge>(connection.m_Edge);
                if (local6.m_CutElementId != identifier1)
                {
                  local6.m_CutElementId = identifier1;
                  if (endNode != nodeIndices.m_SourceNode)
                  {
                    ref Game.Simulation.Flow.Node local7 = ref this.m_Nodes.ElementAt<Game.Simulation.Flow.Node>(endNode);
                    if (local7.m_CutElementId != identifier2)
                    {
                      local7.m_CutElementId = identifier2;
                      this.m_LabelQueue.Enqueue(endNode);
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    public enum Phase
    {
      Initial,
      Producer,
      PostProducer,
      Battery,
      PostBattery,
      Trade,
      PostTrade,
      Complete,
    }

    public struct State
    {
      public ElectricityFlowJob.Phase m_Phase;
      public int m_LastTotalSteps;
      public int m_StepCounter;
      public bool m_Error;

      public State(int lastTotalSteps)
        : this()
      {
        this.m_Phase = ElectricityFlowJob.Phase.Initial;
        this.m_LastTotalSteps = lastTotalSteps;
      }
    }
  }
}
