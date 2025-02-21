// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.FluidFlowSolver
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Assertions;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct FluidFlowSolver
  {
    public int m_SourceNode;
    public int m_SinkNode;
    public NativeArray<Node> m_Nodes;
    public NativeArray<Edge> m_Edges;
    public NativeArray<Connection> m_Connections;
    public NativeMinHeap<LabelHeapData> m_LabelQueue;
    public NativeMinHeap<PushHeapData> m_PushQueue;
    public bool m_Complete;
    public int m_CurrentVersion;
    public int m_StepCounter;

    public void InitializeState()
    {
      this.m_Complete = false;
      this.m_CurrentVersion = 0;
    }

    public void Preflow()
    {
      Node node = this.GetNode(this.m_SinkNode);
      for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
      {
        Connection connection = this.GetConnection(firstConnection);
        int residualCapacity = connection.GetIncomingResidualCapacity(this.m_Edges);
        if (residualCapacity > 0)
          this.AugmentOutgoingFinalFlow(connection.Reverse(), residualCapacity);
      }
    }

    public void LoadState(NativeReference<FluidFlowSolverState> solverState)
    {
      FluidFlowSolverState fluidFlowSolverState = solverState.Value;
      this.m_Complete = fluidFlowSolverState.m_Complete;
      this.m_CurrentVersion = fluidFlowSolverState.m_CurrentVersion;
    }

    public void SaveState(NativeReference<FluidFlowSolverState> solverState)
    {
      solverState.Value = new FluidFlowSolverState()
      {
        m_Complete = this.m_Complete,
        m_CurrentVersion = this.m_CurrentVersion
      };
    }

    public void ResetNodes() => FluidFlowSolver.ResetNodes(this.m_Nodes);

    public static void ResetNodes(NativeArray<Node> nodes)
    {
      for (int index = 0; index < nodes.Length; ++index)
      {
        ref Node local = ref nodes.ElementAt<Node>(index);
        local.m_Height = 0;
        local.m_Excess = 0;
        local.m_Version = 0;
        local.m_Distance = 0;
        local.m_Predecessor = 0;
        local.m_Enqueued = false;
      }
    }

    public void ResetFlows() => FluidFlowSolver.ResetFlows(this.m_Edges);

    public static void ResetFlows(NativeArray<Edge> edges)
    {
      for (int index = 0; index < edges.Length; ++index)
      {
        ref Edge local = ref edges.ElementAt<Edge>(index);
        local.m_FinalFlow = 0;
        local.m_TempFlow = 0;
      }
    }

    public void Solve()
    {
      while (!this.m_Complete)
        this.SolveStep();
    }

    public void SolveStep()
    {
      ++this.m_CurrentVersion;
      this.Label();
      if (this.m_PushQueue.Length != 0)
        this.Push();
      else
        this.m_Complete = true;
    }

    private void Label()
    {
      Assert.AreEqual(0, this.m_LabelQueue.Length);
      Assert.AreEqual(0, this.m_PushQueue.Length);
      ref Node local1 = ref this.GetNode(this.m_SourceNode);
      local1.m_Distance = 0;
      local1.m_Height = 0;
      local1.m_Version = this.m_CurrentVersion;
      local1.m_Enqueued = true;
      this.m_LabelQueue.Insert(new LabelHeapData(this.m_SourceNode, 0));
      while (this.m_LabelQueue.Length != 0)
      {
        LabelHeapData labelHeapData = this.m_LabelQueue.Extract();
        ref Node local2 = ref this.GetNode(labelHeapData.m_NodeIndex);
        if (local2.m_Distance >= labelHeapData.m_Distance)
        {
          ++this.m_StepCounter;
          Assert.IsTrue(local2.m_Distance == labelHeapData.m_Distance);
          int num = local2.m_Height + 1;
          for (int firstConnection = local2.m_FirstConnection; firstConnection < local2.m_LastConnection; ++firstConnection)
          {
            Connection connection = this.GetConnection(firstConnection);
            if (connection.GetOutgoingResidualCapacity(this.m_Edges) > 0)
            {
              Assert.IsTrue(connection.m_EndNode != this.m_SinkNode);
              ref Node local3 = ref this.GetNode(connection.m_EndNode);
              int distance = local2.m_Distance + this.GetLength(connection);
              if (local3.m_Version != this.m_CurrentVersion)
              {
                local3.m_Enqueued = false;
                local3.m_Height = num;
                local3.m_Version = this.m_CurrentVersion;
                local3.m_Distance = distance;
                local3.m_Predecessor = firstConnection;
                this.m_LabelQueue.Insert(new LabelHeapData(connection.m_EndNode, distance));
              }
              else if (local3.m_Distance > distance)
              {
                local3.m_Enqueued &= local3.m_Height == num;
                local3.m_Height = num;
                local3.m_Distance = distance;
                local3.m_Predecessor = firstConnection;
                this.m_LabelQueue.Insert(new LabelHeapData(connection.m_EndNode, distance));
              }
            }
          }
          if (!local2.m_Enqueued && local2.m_Excess > 0)
          {
            local2.m_Enqueued = true;
            this.m_PushQueue.Insert(new PushHeapData(labelHeapData.m_NodeIndex, local2.m_Height));
          }
        }
      }
    }

    private void Push()
    {
      Assert.AreEqual(0, this.m_LabelQueue.Length);
      Assert.AreNotEqual(0, this.m_PushQueue.Length);
      while (this.m_PushQueue.Length != 0)
      {
        PushHeapData pushHeapData = this.m_PushQueue.Extract();
        ref Node local1 = ref this.GetNode(pushHeapData.m_NodeIndex);
        if (local1.m_Enqueued && pushHeapData.m_Height == local1.m_Height)
        {
          ++this.m_StepCounter;
          Assert.IsTrue(local1.m_Excess > 0);
          Assert.AreNotEqual(0, local1.m_Predecessor);
          local1.m_Enqueued = false;
          Connection connection = this.GetConnection(local1.m_Predecessor);
          Assert.IsTrue(connection.GetOutgoingResidualCapacity(this.m_Edges) > 0);
          Assert.IsTrue(connection.m_EndNode == pushHeapData.m_NodeIndex);
          int flow = math.min(local1.m_Excess, this.GetMaxAdditionalOutgoingFlow(connection));
          Assert.IsTrue(flow > 0);
          ref Node local2 = ref this.GetNode(connection.m_StartNode);
          if (!local2.m_Enqueued)
          {
            local2.m_Enqueued = true;
            this.m_PushQueue.Insert(new PushHeapData(connection.m_StartNode, local2.m_Height));
          }
          this.AugmentOutgoingFinalFlow(in connection, flow);
        }
      }
    }

    private int GetLength(Connection connection)
    {
      int outgoingFinalFlow = connection.GetOutgoingFinalFlow(this.m_Edges);
      if (outgoingFinalFlow > 1)
        return 1 + math.ceillog2(outgoingFinalFlow);
      return outgoingFinalFlow < -1 ? 1 - math.ceillog2(-outgoingFinalFlow) : 1;
    }

    private int GetMaxAdditionalOutgoingFlow(Connection connection)
    {
      int outgoingFinalFlow = connection.GetOutgoingFinalFlow(this.m_Edges);
      int residualCapacity = connection.GetOutgoingResidualCapacity(this.m_Edges);
      return math.min((outgoingFinalFlow <= 1 ? (outgoingFinalFlow >= -2 ? (outgoingFinalFlow != -2 ? 2 : 1) : -(Mathf.NextPowerOfTwo(-outgoingFinalFlow) >> 2) - 1) : Mathf.NextPowerOfTwo(outgoingFinalFlow) << 1) - outgoingFinalFlow, residualCapacity);
    }

    private void AugmentOutgoingFinalFlow(in Connection connection, int flow)
    {
      Assert.IsTrue(flow >= 0);
      ref Node local1 = ref this.GetNode(connection.m_StartNode);
      ref Node local2 = ref this.GetNode(connection.m_EndNode);
      ref Edge local3 = ref this.GetEdge(connection.m_Edge);
      local1.m_Excess += flow;
      local3.m_FinalFlow += connection.m_Backwards ? -flow : flow;
      local2.m_Excess -= flow;
      int finalFlow = local3.m_FinalFlow;
      Assert.IsFalse(finalFlow < -local3.GetCapacity(true));
      Assert.IsFalse(finalFlow > local3.GetCapacity(false));
    }

    private ref Node GetNode(int index) => ref this.m_Nodes.ElementAt<Node>(index);

    private ref Edge GetEdge(int index) => ref this.m_Edges.ElementAt<Edge>(index);

    private Connection GetConnection(int index) => this.m_Connections[index];
  }
}
