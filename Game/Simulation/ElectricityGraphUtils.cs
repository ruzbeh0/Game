// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityGraphUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Unity.Assertions;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public static class ElectricityGraphUtils
  {
    public static bool HasAnyFlowEdge(
      Entity node1,
      Entity node2,
      ref BufferLookup<ConnectedFlowEdge> flowConnections,
      ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      Assert.IsTrue(node1.Index > 0);
      Assert.IsTrue(node2.Index > 0);
      foreach (ConnectedFlowEdge connectedFlowEdge in flowConnections[node1])
      {
        ElectricityFlowEdge electricityFlowEdge = flowEdges[connectedFlowEdge.m_Edge];
        if (electricityFlowEdge.m_Start == node1 && electricityFlowEdge.m_End == node2 || electricityFlowEdge.m_Start == node2 && electricityFlowEdge.m_End == node1)
          return true;
      }
      return false;
    }

    public static bool TryGetFlowEdge(
      Entity startNode,
      Entity endNode,
      ref BufferLookup<ConnectedFlowEdge> flowConnections,
      ref ComponentLookup<ElectricityFlowEdge> flowEdges,
      out Entity entity)
    {
      return ElectricityGraphUtils.TryGetFlowEdge(startNode, endNode, ref flowConnections, ref flowEdges, out entity, out ElectricityFlowEdge _);
    }

    public static bool TryGetFlowEdge(
      Entity startNode,
      Entity endNode,
      ref BufferLookup<ConnectedFlowEdge> flowConnections,
      ref ComponentLookup<ElectricityFlowEdge> flowEdges,
      out ElectricityFlowEdge edge)
    {
      return ElectricityGraphUtils.TryGetFlowEdge(startNode, endNode, ref flowConnections, ref flowEdges, out Entity _, out edge);
    }

    public static bool TryGetFlowEdge(
      Entity startNode,
      Entity endNode,
      ref BufferLookup<ConnectedFlowEdge> flowConnections,
      ref ComponentLookup<ElectricityFlowEdge> flowEdges,
      out Entity entity,
      out ElectricityFlowEdge edge)
    {
      Assert.IsTrue(startNode.Index > 0);
      Assert.IsTrue(endNode.Index > 0);
      foreach (ConnectedFlowEdge connectedFlowEdge in flowConnections[startNode])
      {
        entity = connectedFlowEdge.m_Edge;
        edge = flowEdges[entity];
        if (edge.m_Start == startNode && edge.m_End == endNode)
          return true;
      }
      entity = new Entity();
      edge = new ElectricityFlowEdge();
      return false;
    }

    public static bool TrySetFlowEdge(
      Entity startNode,
      Entity endNode,
      FlowDirection direction,
      int capacity,
      ref BufferLookup<ConnectedFlowEdge> flowConnections,
      ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      Entity entity;
      ElectricityFlowEdge edge;
      if (!ElectricityGraphUtils.TryGetFlowEdge(startNode, endNode, ref flowConnections, ref flowEdges, out entity, out edge))
        return false;
      edge.direction = direction;
      edge.m_Capacity = capacity;
      flowEdges[entity] = edge;
      return true;
    }

    public static Entity CreateFlowEdge(
      EntityCommandBuffer commandBuffer,
      EntityArchetype edgeArchetype,
      Entity startNode,
      Entity endNode,
      FlowDirection direction,
      int capacity)
    {
      Assert.AreNotEqual<Entity>(startNode, Entity.Null);
      Assert.AreNotEqual<Entity>(endNode, Entity.Null);
      Entity entity = commandBuffer.CreateEntity(edgeArchetype);
      commandBuffer.SetComponent<ElectricityFlowEdge>(entity, new ElectricityFlowEdge()
      {
        m_Start = startNode,
        m_End = endNode,
        direction = direction,
        m_Capacity = capacity
      });
      commandBuffer.AppendToBuffer<ConnectedFlowEdge>(startNode, new ConnectedFlowEdge(entity));
      commandBuffer.AppendToBuffer<ConnectedFlowEdge>(endNode, new ConnectedFlowEdge(entity));
      return entity;
    }

    public static Entity CreateFlowEdge(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      EntityArchetype edgeArchetype,
      Entity startNode,
      Entity endNode,
      FlowDirection direction,
      int capacity)
    {
      Assert.AreNotEqual<Entity>(startNode, Entity.Null);
      Assert.AreNotEqual<Entity>(endNode, Entity.Null);
      Entity entity = commandBuffer.CreateEntity(jobIndex, edgeArchetype);
      commandBuffer.SetComponent<ElectricityFlowEdge>(jobIndex, entity, new ElectricityFlowEdge()
      {
        m_Start = startNode,
        m_End = endNode,
        direction = direction,
        m_Capacity = capacity
      });
      commandBuffer.AppendToBuffer<ConnectedFlowEdge>(jobIndex, startNode, new ConnectedFlowEdge(entity));
      commandBuffer.AppendToBuffer<ConnectedFlowEdge>(jobIndex, endNode, new ConnectedFlowEdge(entity));
      return entity;
    }

    public static Entity CreateFlowEdge(
      EntityManager entityManager,
      EntityArchetype edgeArchetype,
      Entity startNode,
      Entity endNode,
      FlowDirection direction,
      int capacity)
    {
      Assert.AreNotEqual<Entity>(startNode, Entity.Null);
      Assert.AreNotEqual<Entity>(endNode, Entity.Null);
      Entity entity = entityManager.CreateEntity(edgeArchetype);
      entityManager.SetComponentData<ElectricityFlowEdge>(entity, new ElectricityFlowEdge()
      {
        m_Start = startNode,
        m_End = endNode,
        direction = direction,
        m_Capacity = capacity
      });
      entityManager.GetBuffer<ConnectedFlowEdge>(startNode).Add(new ConnectedFlowEdge(entity));
      entityManager.GetBuffer<ConnectedFlowEdge>(endNode).Add(new ConnectedFlowEdge(entity));
      return entity;
    }

    public static void DeleteFlowNode(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      Entity node,
      ref BufferLookup<ConnectedFlowEdge> flowConnections)
    {
      commandBuffer.AddComponent<Deleted>(jobIndex, node);
      foreach (ConnectedFlowEdge connectedFlowEdge in flowConnections[node])
        commandBuffer.AddComponent<Deleted>(jobIndex, connectedFlowEdge.m_Edge);
    }

    public static void DeleteFlowNode(EntityManager entityManager, Entity node)
    {
      entityManager.AddComponent<Deleted>(node);
      foreach (ConnectedFlowEdge connectedFlowEdge in entityManager.GetBuffer<ConnectedFlowEdge>(node, true))
        entityManager.AddComponent<Deleted>(connectedFlowEdge.m_Edge);
    }

    public static void DeleteBuildingNodes(
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int jobIndex,
      ElectricityBuildingConnection connection,
      ref BufferLookup<ConnectedFlowEdge> flowConnections,
      ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      if (connection.m_TransformerNode != Entity.Null)
        ElectricityGraphUtils.DeleteFlowNode(commandBuffer, jobIndex, connection.m_TransformerNode, ref flowConnections);
      if (connection.m_ProducerEdge != Entity.Null)
        ElectricityGraphUtils.DeleteFlowNode(commandBuffer, jobIndex, connection.GetProducerNode(ref flowEdges), ref flowConnections);
      if (connection.m_ConsumerEdge != Entity.Null)
        ElectricityGraphUtils.DeleteFlowNode(commandBuffer, jobIndex, connection.GetConsumerNode(ref flowEdges), ref flowConnections);
      if (connection.m_ChargeEdge != Entity.Null)
        ElectricityGraphUtils.DeleteFlowNode(commandBuffer, jobIndex, connection.GetChargeNode(ref flowEdges), ref flowConnections);
      if (!(connection.m_DischargeEdge != Entity.Null))
        return;
      ElectricityGraphUtils.DeleteFlowNode(commandBuffer, jobIndex, connection.GetDischargeNode(ref flowEdges), ref flowConnections);
    }
  }
}
