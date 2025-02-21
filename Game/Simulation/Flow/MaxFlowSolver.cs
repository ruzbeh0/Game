// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.MaxFlowSolver
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Unity.Assertions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct MaxFlowSolver
  {
    public const int kMaxNodes = 16777216;
    public int m_LayerHeight;
    public int m_SourceNode;
    public int m_SinkNode;
    public NativeArray<Node> m_Nodes;
    public NativeArray<Edge> m_Edges;
    public NativeArray<Connection> m_Connections;
    public NativeList<Layer> m_Layers;
    public NativeQueue<int> m_LabelQueue;
    public NativeArray<UnsafeList<int>> m_ActiveQueue;
    public bool m_Complete;
    public int m_CurrentLayerIndex;
    public int m_NextLayerIndex;
    public int m_CurrentLabelVersion;
    public int m_CurrentActiveVersion;
    public int m_StepCounter;

    public void InitializeState()
    {
      this.m_Complete = false;
      this.m_NextLayerIndex = 0;
      this.m_CurrentLabelVersion = 1;
      this.m_CurrentActiveVersion = 1;
      Node node = this.GetNode(this.m_SourceNode);
      Layer layer = new Layer(node.connectionCount, Allocator.Temp);
      for (int firstConnection = node.m_FirstConnection; firstConnection < node.m_LastConnection; ++firstConnection)
      {
        Connection connection = this.GetConnection(firstConnection);
        if (connection.GetOutgoingResidualCapacity(this.m_Edges) > 0)
        {
          int index = layer.AddCutElement(new CutElement()
          {
            m_Flags = CutElementFlags.Created | CutElementFlags.Admissible | CutElementFlags.Changed,
            m_StartNode = connection.m_StartNode,
            m_EndNode = connection.m_EndNode,
            m_Edge = connection.m_Edge,
            m_Version = this.m_CurrentLabelVersion,
            m_LinkedElements = -1,
            m_NextIndex = -1
          });
          layer.GetCutElement(index).m_Group = index;
          this.GetEdge(connection.m_Edge).m_CutElementId = new Identifier(index, this.m_CurrentLabelVersion);
        }
      }
      this.m_Layers.Clear();
      this.m_Layers.Add(in layer);
    }

    public void LoadState(
      NativeReference<MaxFlowSolverState> solverState,
      NativeList<LayerState> layerStates,
      NativeList<CutElement> layerElements,
      NativeList<CutElementRef> layerElementRefs)
    {
      MaxFlowSolverState maxFlowSolverState = solverState.Value;
      this.m_Complete = maxFlowSolverState.m_Complete;
      this.m_NextLayerIndex = maxFlowSolverState.m_NextLayerIndex;
      this.m_CurrentLabelVersion = maxFlowSolverState.m_CurrentLabelVersion;
      this.m_CurrentActiveVersion = maxFlowSolverState.m_CurrentActiveVersion;
      int elementIndex = 0;
      int elementRefIndex = 0;
      for (int index = 0; index < layerStates.Length; ++index)
        this.m_Layers.Add(Layer.Load(layerStates[index], layerElements.AsArray(), ref elementIndex, layerElementRefs.AsArray(), ref elementRefIndex));
    }

    public void SaveState(
      NativeReference<MaxFlowSolverState> solverState,
      NativeList<LayerState> layerStates,
      NativeList<CutElement> layerElements,
      NativeList<CutElementRef> layerElementRefs)
    {
      solverState.Value = new MaxFlowSolverState()
      {
        m_Complete = this.m_Complete,
        m_NextLayerIndex = this.m_NextLayerIndex,
        m_CurrentLabelVersion = this.m_CurrentLabelVersion,
        m_CurrentActiveVersion = this.m_CurrentActiveVersion
      };
      layerStates.Length = 0;
      layerElements.Length = 0;
      layerElementRefs.Length = 0;
      if (this.m_Complete)
        return;
      for (int index = 0; index < this.m_Layers.Length; ++index)
        this.m_Layers[index].Save(layerStates, layerElements, layerElementRefs);
    }

    public void ResetNetwork()
    {
      MaxFlowSolver.ResetNetwork(this.m_Nodes, this.m_Edges, this.m_SourceNode);
    }

    public static void ResetNetwork(
      NativeArray<Node> nodes,
      NativeArray<Edge> edges,
      int sourceNode)
    {
      for (int index = 0; index < nodes.Length; ++index)
      {
        ref Node local = ref nodes.ElementAt<Node>(index);
        local.m_Height = 16777216;
        local.m_Excess = 0;
        local.m_CutElementId = new Identifier();
        local.m_Version = 0;
        local.m_Retreat = false;
      }
      for (int index = 0; index < edges.Length; ++index)
      {
        ref Edge local = ref edges.ElementAt<Edge>(index);
        local.m_CutElementId = new Identifier();
        local.FinalizeTempFlow();
      }
      nodes.ElementAt<Node>(sourceNode).m_Height = 0;
    }

    public void Solve()
    {
      while (!this.m_Complete)
        this.SolveNextLayer();
    }

    public void SolveNextLayer()
    {
      Layer layer = this.GetLayer(this.m_NextLayerIndex);
      if (layer.isEmpty)
      {
        this.m_Complete = true;
      }
      else
      {
        this.m_CurrentLayerIndex = this.m_NextLayerIndex;
        this.m_NextLayerIndex = this.m_CurrentLayerIndex + 1;
        ++this.m_CurrentActiveVersion;
        if (this.m_Layers.Length <= this.m_NextLayerIndex)
          this.m_Layers.Add(new Layer(2 * math.max(layer.usedElementCount, layer.usedElementRefCount), Allocator.Temp));
        if (!this.LabelPreflow())
          return;
        int source = this.AdvanceToSource();
        if (source > this.m_CurrentLayerIndex)
          return;
        this.RetreatFromLayer(source);
      }
    }

    private bool LabelPreflow()
    {
      bool flag = false;
      ref Layer local1 = ref this.GetLayer(this.m_CurrentLayerIndex);
      int num1 = this.m_CurrentLayerIndex + 1;
      Layer layer = this.GetLayer(num1);
      int lowerCutHeight = this.GetLowerCutHeight(this.m_CurrentLayerIndex);
      int upperCutHeight = this.GetUpperCutHeight(this.m_CurrentLayerIndex);
      for (int index1 = 0; index1 < local1.m_Elements.Length; ++index1)
      {
        CutElement cutElement = local1.GetCutElement(index1);
        if (cutElement.isCreated && (cutElement.isChanged || cutElement.isDeleted))
        {
          int num2 = cutElement.m_Group;
          do
          {
            ref CutElement local2 = ref local1.GetCutElement(num2);
            int nextIndex = local2.m_NextIndex;
            this.DeleteLinkedElements(this.m_CurrentLayerIndex, num2);
            if (local2.isDeleted)
              local1.FreeCutElement(num2);
            else if (!local2.isAdmissible)
            {
              Node node = this.GetNode(local2.m_StartNode);
              int layerIndexForHeight = this.GetLayerIndexForHeight(node.m_Height);
              Assert.IsTrue(layerIndexForHeight < this.m_CurrentLayerIndex);
              ref Layer local3 = ref this.GetLayer(layerIndexForHeight);
              local1.FreeCutElement(num2);
              int index2 = node.m_CutElementId.m_Index;
              int currentLayerIndex = this.m_CurrentLayerIndex;
              int upperLayerElementIndex = num2;
              local3.RemoveElementLink(index2, currentLayerIndex, upperLayerElementIndex);
            }
            else
            {
              Identifier identifier = new Identifier(num2, this.m_CurrentLabelVersion);
              local2.m_Group = num2;
              local2.m_Version = this.m_CurrentLabelVersion;
              this.GetEdge(local2.m_Edge).m_CutElementId = identifier;
              local2.isChanged = false;
              local2.m_NextIndex = -1;
              ref Node local4 = ref this.GetNode(local2.m_EndNode);
              if (local4.m_CutElementId.m_Version != this.m_CurrentLabelVersion)
              {
                local4.m_Height = lowerCutHeight;
                local4.m_CutElementId = identifier;
                this.m_LabelQueue.Enqueue(local2.m_EndNode);
              }
              else
                local1.MergeGroups(local4.m_CutElementId.m_Index, num2);
            }
            num2 = nextIndex;
          }
          while (num2 != -1);
        }
      }
      int index;
      while (this.m_LabelQueue.TryDequeue(out index))
      {
        ++this.m_StepCounter;
        ref Node local5 = ref this.GetNode(index);
        for (int firstConnection = local5.m_FirstConnection; firstConnection < local5.m_LastConnection; ++firstConnection)
        {
          Connection connection = this.GetConnection(firstConnection);
          ref Edge local6 = ref this.GetEdge(connection.m_Edge);
          int endNode = connection.m_EndNode;
          local6.FinalizeTempFlow();
          if (endNode != this.m_SinkNode)
          {
            if (endNode != this.m_SourceNode)
            {
              ref Node local7 = ref this.GetNode(endNode);
              int nodeValidLayerIndex = this.GetNodeValidLayerIndex(local7);
              if (nodeValidLayerIndex == -1)
              {
                if (connection.GetOutgoingResidualCapacity(this.m_Edges) > 0)
                {
                  if (local5.m_Height != upperCutHeight)
                  {
                    local6.m_CutElementId = local5.m_CutElementId;
                    local7.m_Height = local5.m_Height + 1;
                    local7.m_CutElementId = local5.m_CutElementId;
                    local7.m_Retreat = false;
                    this.m_LabelQueue.Enqueue(endNode);
                  }
                  else
                  {
                    local7.m_Height = local5.m_Height + 1;
                    local7.m_CutElementId = new Identifier();
                    if (layer.ContainsCutElementForConnection(local6.m_CutElementId, connection, true))
                      this.BumpAdmissibleLayerCutElement(this.m_CurrentLayerIndex, num1, in connection);
                    else
                      this.AddAdmissibleLayerCutElement(this.m_CurrentLayerIndex, num1, in connection);
                  }
                }
                else
                {
                  local7.m_Height = 16777216;
                  local7.m_CutElementId = new Identifier();
                }
              }
              else if (nodeValidLayerIndex == this.m_CurrentLayerIndex)
              {
                if (!local7.m_CutElementId.Equals(local5.m_CutElementId))
                  local1.MergeGroups(local7.m_CutElementId.m_Index, local5.m_CutElementId.m_Index);
              }
              else if (nodeValidLayerIndex < this.m_CurrentLayerIndex)
              {
                if (connection.GetOutgoingResidualCapacity(this.m_Edges) > 0 && connection.GetIncomingResidualCapacity(this.m_Edges) == 0)
                  this.AddInadmissibleLayerCutElement(nodeValidLayerIndex, this.m_CurrentLayerIndex, connection.Reverse());
              }
              else if (local5.m_Height != upperCutHeight)
              {
                if (connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
                  this.BumpInadmissibleLayerCutElement(this.m_CurrentLayerIndex, nodeValidLayerIndex, in connection);
              }
              else if (connection.GetOutgoingResidualCapacity(this.m_Edges) > 0)
                this.BumpAdmissibleLayerCutElement(this.m_CurrentLayerIndex, nodeValidLayerIndex, in connection);
              else if (connection.GetIncomingResidualCapacity(this.m_Edges) > 0)
                this.BumpInadmissibleLayerCutElement(this.m_CurrentLayerIndex, nodeValidLayerIndex, in connection);
            }
          }
          else
          {
            int residualCapacity = connection.GetOutgoingResidualCapacity(this.m_Edges);
            if (residualCapacity > 0)
            {
              this.AugmentOutgoingTempFlow(in connection, residualCapacity);
              local5.m_Version = this.m_CurrentActiveVersion;
              local6.m_CutElementId = local5.m_CutElementId;
              this.m_ActiveQueue.ElementAt<UnsafeList<int>>(local5.m_Height - lowerCutHeight).Add(in index);
              flag = true;
            }
          }
        }
      }
      return flag;
    }

    private void DeleteLinkedElements(int lowerLayerIndex, int index)
    {
      ref Layer local1 = ref this.GetLayer(lowerLayerIndex);
      ref CutElement local2 = ref local1.GetCutElement(index);
      int index1 = local2.m_LinkedElements;
      local2.m_LinkedElements = -1;
      int nextIndex;
      for (; index1 != -1; index1 = nextIndex)
      {
        CutElementRef cutElementRef = local1.GetCutElementRef(index1);
        this.GetLayer(cutElementRef.m_Layer).GetCutElement(cutElementRef.m_Index).isDeleted = true;
        nextIndex = cutElementRef.m_NextIndex;
        local1.FreeCutElementRef(index1);
      }
    }

    private int GetNodeValidLayerIndex(Node node)
    {
      if (node.m_Height != 16777216 && node.m_CutElementId.m_Version != 0)
      {
        int layerIndexForHeight = this.GetLayerIndexForHeight(node.m_Height);
        if (this.GetLayer(layerIndexForHeight).ContainsCutElement(node.m_CutElementId))
          return layerIndexForHeight;
      }
      return -1;
    }

    private void AddAdmissibleLayerCutElement(
      int lowerLayerIndex,
      int higherLayerIndex,
      in Connection lhConnection)
    {
      ref Layer local = ref this.GetLayer(higherLayerIndex);
      int num = local.AddCutElement(new CutElement()
      {
        m_Flags = CutElementFlags.Created | CutElementFlags.Admissible | CutElementFlags.Changed,
        m_StartNode = lhConnection.m_StartNode,
        m_EndNode = lhConnection.m_EndNode,
        m_Edge = lhConnection.m_Edge,
        m_Version = this.m_CurrentLabelVersion,
        m_LinkedElements = -1,
        m_NextIndex = -1
      });
      local.GetCutElement(num).m_Group = num;
      this.GetEdge(lhConnection.m_Edge).m_CutElementId = new Identifier(num, this.m_CurrentLabelVersion);
      int index = this.GetNode(lhConnection.m_StartNode).m_CutElementId.m_Index;
      this.CreateElementLink(lowerLayerIndex, index, higherLayerIndex, num);
    }

    private void BumpAdmissibleLayerCutElement(
      int lowerLayerIndex,
      int higherLayerIndex,
      in Connection lhConnection)
    {
      Layer layer = this.GetLayer(higherLayerIndex);
      int index1 = this.GetEdge(lhConnection.m_Edge).m_CutElementId.m_Index;
      layer.GetCutElement(index1).isDeleted = false;
      int index2 = this.GetNode(lhConnection.m_StartNode).m_CutElementId.m_Index;
      this.CreateElementLink(lowerLayerIndex, index2, higherLayerIndex, index1);
    }

    private void AddInadmissibleLayerCutElement(
      int lowerLayerIndex,
      int higherLayerIndex,
      in Connection lhConnection)
    {
      ref Layer local1 = ref this.GetLayer(higherLayerIndex);
      int index1 = this.GetNode(lhConnection.m_EndNode).m_CutElementId.m_Index;
      ref CutElement local2 = ref local1.GetCutElement(index1);
      int num = local1.AddCutElement(new CutElement()
      {
        m_Flags = CutElementFlags.Created,
        m_StartNode = lhConnection.m_StartNode,
        m_EndNode = lhConnection.m_EndNode,
        m_Edge = lhConnection.m_Edge,
        m_Group = local2.m_Group,
        m_Version = this.m_CurrentLabelVersion,
        m_LinkedElements = -1,
        m_NextIndex = local2.m_NextIndex
      });
      local1.GetCutElement(index1).m_NextIndex = num;
      this.GetEdge(lhConnection.m_Edge).m_CutElementId = new Identifier(num, this.m_CurrentLabelVersion);
      int index2 = this.GetNode(lhConnection.m_StartNode).m_CutElementId.m_Index;
      this.CreateElementLink(lowerLayerIndex, index2, higherLayerIndex, num);
    }

    private void BumpInadmissibleLayerCutElement(
      int lowerLayerIndex,
      int higherLayerIndex,
      in Connection lhConnection)
    {
      Layer layer = this.GetLayer(higherLayerIndex);
      int index1 = this.GetEdge(lhConnection.m_Edge).m_CutElementId.m_Index;
      ref CutElement local = ref layer.GetCutElement(index1);
      local.isDeleted = false;
      if (local.isAdmissible)
      {
        local.isAdmissible = false;
        local.isChanged = true;
      }
      int index2 = this.GetNode(lhConnection.m_StartNode).m_CutElementId.m_Index;
      this.CreateElementLink(lowerLayerIndex, index2, higherLayerIndex, index1);
    }

    private void CreateElementLink(
      int lowerLayerIndex,
      int lowerElementIndex,
      int higherLayerIndex,
      int higherElementIndex)
    {
      ref Layer local1 = ref this.GetLayer(lowerLayerIndex);
      ref CutElement local2 = ref local1.GetCutElement(lowerElementIndex);
      int num = local1.AddCutElementRef(new CutElementRef()
      {
        m_Layer = higherLayerIndex,
        m_Index = higherElementIndex,
        m_NextIndex = local2.m_LinkedElements
      });
      local2.m_LinkedElements = num;
    }

    private int AdvanceToSource()
    {
      int retreatLayerIndex = this.m_CurrentLayerIndex + 1;
      for (int currentLayerIndex = this.m_CurrentLayerIndex; currentLayerIndex >= 0; --currentLayerIndex)
        this.AdvanceActiveLayer(currentLayerIndex, ref retreatLayerIndex);
      ref UnsafeList<int> local1 = ref this.m_ActiveQueue.ElementAt<UnsafeList<int>>(this.m_LayerHeight - 1);
      Assert.AreEqual(1, local1.Length);
      Assert.AreEqual(this.m_SourceNode, local1[0]);
      local1.Clear();
      ref Node local2 = ref this.GetNode(this.m_SourceNode);
      local2.m_Retreat = false;
      local2.m_Version = this.m_CurrentActiveVersion;
      if (this.m_NextLayerIndex <= this.m_CurrentLayerIndex)
        ++this.m_CurrentLabelVersion;
      return retreatLayerIndex;
    }

    private void AdvanceActiveLayer(int activeLayerIndex, ref int retreatLayerIndex)
    {
      bool flag1 = activeLayerIndex == this.m_CurrentLayerIndex;
      for (int index1 = this.m_LayerHeight - 1; index1 >= 0; --index1)
      {
        bool flag2 = index1 == 0;
        ref UnsafeList<int> local1 = ref this.m_ActiveQueue.ElementAt<UnsafeList<int>>(index1);
        for (int index2 = 0; index2 < local1.Length; ++index2)
        {
          ++this.m_StepCounter;
          int num1 = local1[index2];
          ref Node local2 = ref this.GetNode(num1);
          Assert.AreEqual(this.m_CurrentActiveVersion, local2.m_Version);
          Assert.AreNotEqual(this.m_SourceNode, num1);
          Assert.AreNotEqual(this.m_SinkNode, num1);
          Assert.IsFalse(local2.m_Excess <= 0);
          int num2 = local2.m_Height - 1;
          Assert.AreEqual(this.GetLowerCutHeight(activeLayerIndex) + index1 - 1, num2);
          bool condition = false;
          bool flag3 = false;
          for (int firstConnection = local2.m_FirstConnection; firstConnection < local2.m_LastConnection; ++firstConnection)
          {
            Connection connection = this.GetConnection(firstConnection);
            int endNode = connection.m_EndNode;
            ref Node local3 = ref this.GetNode(endNode);
            if (!flag1 && local3.m_Version != this.m_CurrentActiveVersion)
            {
              local3.m_Retreat = false;
              this.FinalizeTempFlow(in connection);
            }
            if (local3.m_Height == num2)
            {
              int residualCapacity = connection.GetIncomingResidualCapacity(this.m_Edges);
              int flow = math.min(residualCapacity, local2.m_Excess);
              if (flow != 0)
              {
                if (local3.m_Version != this.m_CurrentActiveVersion)
                {
                  local3.m_Retreat = false;
                  local3.m_Version = this.m_CurrentActiveVersion;
                  this.m_ActiveQueue.ElementAt<UnsafeList<int>>(flag2 ? this.m_LayerHeight - 1 : index1 - 1).Add(in endNode);
                }
                this.FinalizeTempFlow(in connection);
                this.AugmentIncomingTempFlow(in connection, flow);
                condition = true;
                if (local2.m_Retreat)
                  local3.m_Retreat = true;
                if (flow != residualCapacity)
                {
                  Assert.IsTrue(connection.GetIncomingResidualCapacity(this.m_Edges) > 0);
                  flag3 = true;
                }
                else
                  Assert.AreEqual(0, connection.GetIncomingResidualCapacity(this.m_Edges));
              }
            }
            if (!(flag1 & flag3) || local2.m_Excess != 0)
              Assert.IsFalse(local2.m_Excess < 0);
            else
              break;
          }
          Assert.IsTrue(condition);
          if (!flag3)
          {
            bool flag4 = local2.m_Excess != 0;
            if (num2 > 0)
            {
              this.m_NextLayerIndex = this.GetLayerIndexForHeight(num2);
              Layer layer = this.GetLayer(this.m_NextLayerIndex);
              if (flag4)
                retreatLayerIndex = activeLayerIndex;
              for (int firstConnection = local2.m_FirstConnection; firstConnection < local2.m_LastConnection; ++firstConnection)
              {
                ref Node local4 = ref this.GetNode(this.GetConnection(firstConnection).m_EndNode);
                if (local4.m_Height == num2)
                {
                  layer.GetCutElement(local4.m_CutElementId.m_Index).isChanged = true;
                  if (flag4)
                    local4.m_Retreat = true;
                }
              }
            }
            else
            {
              this.m_NextLayerIndex = 0;
              Layer layer = this.GetLayer(this.m_NextLayerIndex);
              if (flag4)
                retreatLayerIndex = 0;
              layer.GetCutElement(local2.m_CutElementId.m_Index).isDeleted = true;
            }
          }
          else
            Assert.AreEqual(0, local2.m_Excess);
        }
        local1.Clear();
      }
    }

    private void RetreatFromLayer(int retreatLayerIndex)
    {
      ++this.m_CurrentActiveVersion;
      Layer layer = this.GetLayer(retreatLayerIndex);
      ref UnsafeList<int> local1 = ref this.m_ActiveQueue.ElementAt<UnsafeList<int>>(0);
      for (int index = 0; index < layer.m_Elements.Length; ++index)
      {
        CutElement cutElement = layer.GetCutElement(index);
        if (cutElement.isAdmissible)
        {
          ref Node local2 = ref this.GetNode(cutElement.m_EndNode);
          if ((local2.m_Retreat || local2.m_Excess > 0) && local2.m_Version != this.m_CurrentActiveVersion)
          {
            local2.m_Version = this.m_CurrentActiveVersion;
            local1.Add(in cutElement.m_EndNode);
          }
        }
      }
      for (int activeLayerIndex = retreatLayerIndex; activeLayerIndex <= this.m_CurrentLayerIndex; ++activeLayerIndex)
        this.RetreatActiveLayer(activeLayerIndex);
    }

    private void RetreatActiveLayer(int activeLayerIndex)
    {
      bool flag1 = activeLayerIndex == this.m_CurrentLayerIndex;
      for (int index1 = 0; index1 < this.m_LayerHeight; ++index1)
      {
        bool flag2 = flag1 && index1 == 0;
        bool flag3 = index1 == this.m_LayerHeight - 1;
        ref UnsafeList<int> local1 = ref this.m_ActiveQueue.ElementAt<UnsafeList<int>>(index1);
        for (int index2 = 0; index2 < local1.Length; ++index2)
        {
          ++this.m_StepCounter;
          ref Node local2 = ref this.GetNode(local1[index2]);
          Assert.IsFalse(local2.m_Excess < 0);
          Assert.IsTrue(local2.m_Excess > 0 || local2.m_Retreat);
          local2.m_Retreat = false;
          int heightPlusOne = local2.m_Height + 1;
          int branchFlow;
          int sinkFlow;
          Connection connection1;
          this.GetTotalAdvancedFlow(local2, heightPlusOne, out branchFlow, out sinkFlow, out connection1);
          Assert.IsTrue(sinkFlow == 0 || this.m_CurrentLayerIndex == activeLayerIndex);
          Assert.IsFalse(branchFlow + sinkFlow < local2.m_Excess);
          if (flag2)
            this.GetLayer(activeLayerIndex).GetCutElement(local2.m_CutElementId.m_Index).isChanged = true;
          for (int firstConnection = local2.m_FirstConnection; firstConnection < local2.m_LastConnection; ++firstConnection)
          {
            Connection connection2 = this.GetConnection(firstConnection);
            int endNode = connection2.m_EndNode;
            if (endNode != this.m_SinkNode)
            {
              ref Node local3 = ref this.GetNode(endNode);
              if (local3.m_Height == heightPlusOne)
              {
                if (branchFlow != 0)
                {
                  int outgoingTempFlow = this.GetOutgoingTempFlow(in connection2);
                  if (outgoingTempFlow != 0)
                  {
                    float num = (float) outgoingTempFlow / (float) branchFlow;
                    branchFlow -= outgoingTempFlow;
                    int a = math.clamp(local2.m_Excess - branchFlow, 0, outgoingTempFlow);
                    int b = math.min(outgoingTempFlow, local2.m_Excess);
                    int flow = math.clamp(Mathf.RoundToInt((float) local2.m_Excess * num), a, b);
                    if (flow != 0)
                      this.AugmentIncomingTempFlow(in connection2, flow);
                  }
                }
                if (local3.m_Version != this.m_CurrentActiveVersion && (local3.m_Excess != 0 || local3.m_Retreat))
                {
                  local3.m_Version = this.m_CurrentActiveVersion;
                  this.m_ActiveQueue.ElementAt<UnsafeList<int>>(flag3 ? 0 : index1 + 1).Add(in endNode);
                }
              }
            }
          }
          if (local2.m_Excess > 0 && sinkFlow > 0)
          {
            Assert.IsTrue(local2.m_Excess <= sinkFlow);
            this.AugmentIncomingTempFlow(in connection1, local2.m_Excess);
          }
          Assert.AreEqual(0, local2.m_Excess);
        }
        local1.Clear();
      }
    }

    private void GetTotalAdvancedFlow(
      Node currentNode,
      int heightPlusOne,
      out int branchFlow,
      out int sinkFlow,
      out Connection sinkConnection)
    {
      sinkConnection = new Connection();
      branchFlow = 0;
      sinkFlow = 0;
      for (int firstConnection = currentNode.m_FirstConnection; firstConnection < currentNode.m_LastConnection; ++firstConnection)
      {
        Connection connection = this.GetConnection(firstConnection);
        if (connection.m_EndNode == this.m_SinkNode)
        {
          int outgoingTempFlow = this.GetOutgoingTempFlow(in connection);
          Assert.IsFalse(outgoingTempFlow < 0);
          Assert.IsTrue(sinkFlow == 0);
          sinkFlow = outgoingTempFlow;
          sinkConnection = connection;
        }
        else if (this.GetNode(connection.m_EndNode).m_Height == heightPlusOne)
        {
          int outgoingTempFlow = this.GetOutgoingTempFlow(in connection);
          Assert.IsFalse(outgoingTempFlow < 0);
          branchFlow += outgoingTempFlow;
        }
      }
    }

    private int GetLayerIndexForHeight(int height) => (height - 1) / this.m_LayerHeight;

    private int GetLowerCutHeight(int layerIndex) => layerIndex * this.m_LayerHeight + 1;

    private int GetUpperCutHeight(int layerIndex) => (layerIndex + 1) * this.m_LayerHeight;

    private void FinalizeTempFlow(in Connection connection)
    {
      this.GetEdge(connection.m_Edge).FinalizeTempFlow();
    }

    private void AugmentOutgoingTempFlow(in Connection connection, int flow)
    {
      Assert.IsTrue(flow >= 0);
      ref Node local1 = ref this.GetNode(connection.m_StartNode);
      ref Node local2 = ref this.GetNode(connection.m_EndNode);
      ref Edge local3 = ref this.GetEdge(connection.m_Edge);
      local1.m_Excess += flow;
      local3.m_TempFlow += connection.m_Backwards ? -flow : flow;
      local2.m_Excess -= flow;
      int flow1 = local3.flow;
      Assert.IsTrue(connection.m_StartNode == this.m_SinkNode || local1.m_Excess >= 0);
      Assert.IsTrue(connection.m_EndNode == this.m_SinkNode || local2.m_Excess >= 0);
      Assert.IsFalse(flow1 < -local3.GetCapacity(true));
      Assert.IsFalse(flow1 > local3.GetCapacity(false));
    }

    private void AugmentIncomingTempFlow(in Connection connection, int flow)
    {
      Assert.IsTrue(flow >= 0);
      ref Node local1 = ref this.GetNode(connection.m_StartNode);
      ref Node local2 = ref this.GetNode(connection.m_EndNode);
      ref Edge local3 = ref this.GetEdge(connection.m_Edge);
      local2.m_Excess += flow;
      local3.m_TempFlow += connection.m_Backwards ? flow : -flow;
      local1.m_Excess -= flow;
      int flow1 = local3.flow;
      Assert.IsTrue(connection.m_EndNode == this.m_SinkNode || local2.m_Excess >= 0);
      Assert.IsTrue(connection.m_StartNode == this.m_SinkNode || local1.m_Excess >= 0);
      Assert.IsFalse(flow1 < -local3.GetCapacity(true));
      Assert.IsFalse(flow1 > local3.GetCapacity(false));
    }

    private int GetOutgoingTempFlow(in Connection connection)
    {
      Edge edge = this.GetEdge(connection.m_Edge);
      return !connection.m_Backwards ? edge.m_TempFlow : -edge.m_TempFlow;
    }

    private ref Node GetNode(int index) => ref this.m_Nodes.ElementAt<Node>(index);

    private ref Edge GetEdge(int index) => ref this.m_Edges.ElementAt<Edge>(index);

    private Connection GetConnection(int index) => this.m_Connections[index];

    private ref Layer GetLayer(int index) => ref this.m_Layers.ElementAt(index);
  }
}
