// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.UnsafePathfindData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  [GenerateTestsForBurstCompatibility]
  public struct UnsafePathfindData : IDisposable
  {
    public UnsafeList<Edge> m_Edges;
    public UnsafeList<EdgeID> m_FreeIDs;
    public UnsafeParallelHashMap<Entity, EdgeID> m_PathEdges;
    public UnsafeParallelHashMap<Entity, EdgeID> m_SecondaryEdges;
    public UnsafeParallelHashMap<PathNode, NodeID> m_NodeIDs;
    public UnsafeParallelHashMap<NodeID, PathNode> m_PathNodes;
    private UnsafeHeapAllocator m_ConnectionAllocator;
    private unsafe void* m_Connections;
    private unsafe void* m_ReversedConnections;
    private int m_NodeCount;
    private readonly Allocator m_AllocatorLabel;

    public unsafe UnsafePathfindData(Allocator allocator)
    {
      this.m_Edges = new UnsafeList<Edge>(1000, (AllocatorManager.AllocatorHandle) allocator);
      this.m_FreeIDs = new UnsafeList<EdgeID>(100, (AllocatorManager.AllocatorHandle) allocator);
      this.m_PathEdges = new UnsafeParallelHashMap<Entity, EdgeID>(1000, (AllocatorManager.AllocatorHandle) allocator);
      this.m_SecondaryEdges = new UnsafeParallelHashMap<Entity, EdgeID>(1000, (AllocatorManager.AllocatorHandle) allocator);
      this.m_NodeIDs = new UnsafeParallelHashMap<PathNode, NodeID>(1000, (AllocatorManager.AllocatorHandle) allocator);
      this.m_PathNodes = new UnsafeParallelHashMap<NodeID, PathNode>(1000, (AllocatorManager.AllocatorHandle) allocator);
      this.m_ConnectionAllocator = new UnsafeHeapAllocator(1000U, 2U, allocator);
      this.m_Connections = UnsafeUtility.Malloc((long) (this.m_ConnectionAllocator.Size * 4U), (int) this.m_ConnectionAllocator.MinimumAlignment * 4, allocator);
      this.m_ReversedConnections = UnsafeUtility.Malloc((long) (this.m_ConnectionAllocator.Size * 4U), (int) this.m_ConnectionAllocator.MinimumAlignment * 4, allocator);
      this.m_AllocatorLabel = allocator;
      this.m_NodeCount = 0;
    }

    public unsafe void Dispose()
    {
      this.m_Edges.Dispose();
      this.m_FreeIDs.Dispose();
      this.m_PathEdges.Dispose();
      this.m_SecondaryEdges.Dispose();
      this.m_NodeIDs.Dispose();
      this.m_PathNodes.Dispose();
      this.m_ConnectionAllocator.Dispose();
      UnsafeUtility.Free(this.m_Connections, this.m_AllocatorLabel);
      UnsafeUtility.Free(this.m_ReversedConnections, this.m_AllocatorLabel);
    }

    public void Clear()
    {
      this.m_Edges.Clear();
      this.m_FreeIDs.Clear();
      this.m_PathEdges.Clear();
      this.m_SecondaryEdges.Clear();
      this.m_NodeIDs.Clear();
      this.m_PathNodes.Clear();
      this.m_ConnectionAllocator.Clear();
    }

    public void GetMemoryStats(out uint used, out uint allocated)
    {
      used = (uint) (this.m_Edges.Length * (sizeof (Edge) + sizeof (Entity) + sizeof (EdgeID)) + this.m_FreeIDs.Length * sizeof (EdgeID) + this.m_NodeCount * (sizeof (PathNode) * 2 + sizeof (NodeID) * 2) + (int) this.m_ConnectionAllocator.UsedSpace * 4);
      allocated = (uint) (this.m_Edges.Capacity * sizeof (Edge) + this.m_FreeIDs.Capacity * sizeof (EdgeID) + (this.m_PathEdges.Capacity + this.m_SecondaryEdges.Capacity) * (sizeof (Entity) + sizeof (EdgeID)) + (int) (uint) ((ulong) this.m_NodeIDs.Capacity + (ulong) (uint) this.m_PathNodes.Capacity) * (sizeof (PathNode) + sizeof (NodeID)) + (int) this.m_ConnectionAllocator.Size * 4);
    }

    public EdgeID CreateEdge(
      PathNode startNode,
      PathNode middleNode,
      PathNode endNode,
      PathSpecification specification,
      LocationSpecification location)
    {
      EdgeID edgeID;
      if (this.m_FreeIDs.Length > 0)
      {
        int index = this.m_FreeIDs.Length - 1;
        edgeID = this.m_FreeIDs[index];
        this.m_FreeIDs.Length = index;
      }
      else
      {
        edgeID = new EdgeID()
        {
          m_Index = this.m_Edges.Length
        };
        this.m_Edges.Add(new Edge());
      }
      ref Edge local = ref this.m_Edges.ElementAt(edgeID.m_Index);
      local.m_StartID = new NodeID() { m_Index = -1 };
      local.m_MiddleID = new NodeID() { m_Index = -1 };
      local.m_EndID = new NodeID() { m_Index = -1 };
      int accessIndex = math.select(-1, specification.m_AccessRequirement, (specification.m_Flags & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary));
      if ((specification.m_Flags & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        local.m_StartID = this.AddConnection(startNode.StripCurvePos(), edgeID, accessIndex);
        local.m_EndID = this.AddReversedConnection(endNode.StripCurvePos(), edgeID, accessIndex);
      }
      if ((specification.m_Flags & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        local.m_MiddleID = this.AddConnection(middleNode.StripCurvePos(), edgeID, accessIndex);
        local.m_MiddleID = this.AddReversedConnection(middleNode.StripCurvePos(), edgeID, accessIndex);
      }
      if ((specification.m_Flags & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        local.m_EndID = this.AddConnection(endNode.StripCurvePos(), edgeID, accessIndex);
        local.m_StartID = this.AddReversedConnection(startNode.StripCurvePos(), edgeID, accessIndex);
      }
      local.m_StartCurvePos = startNode.GetCurvePos();
      local.m_EndCurvePos = endNode.GetCurvePos();
      local.m_Specification = specification;
      local.m_Location = location;
      return edgeID;
    }

    public void UpdateEdge(
      EdgeID edgeID,
      PathNode startNode,
      PathNode middleNode,
      PathNode endNode,
      PathSpecification specification,
      LocationSpecification location)
    {
      ref Edge local = ref this.m_Edges.ElementAt(edgeID.m_Index);
      EdgeFlags edgeFlags1 = local.m_Specification.m_Flags & specification.m_Flags;
      EdgeFlags edgeFlags2 = local.m_Specification.m_Flags & ~specification.m_Flags;
      EdgeFlags edgeFlags3 = ~local.m_Specification.m_Flags & specification.m_Flags;
      EdgeFlags edgeFlags4 = edgeFlags2;
      EdgeFlags edgeFlags5 = edgeFlags3;
      NodeID other1;
      if (!this.m_NodeIDs.TryGetValue(startNode.StripCurvePos(), out other1))
        other1 = new NodeID() { m_Index = -2 };
      NodeID other2;
      if (!this.m_NodeIDs.TryGetValue(middleNode.StripCurvePos(), out other2))
        other2 = new NodeID() { m_Index = -2 };
      NodeID other3;
      if (!this.m_NodeIDs.TryGetValue(endNode.StripCurvePos(), out other3))
        other3 = new NodeID() { m_Index = -2 };
      EdgeFlags edgeFlags6 = edgeFlags1 & EdgeFlags.Forward;
      EdgeFlags edgeFlags7 = edgeFlags1 & EdgeFlags.AllowMiddle;
      EdgeFlags edgeFlags8 = edgeFlags1 & EdgeFlags.Backward;
      if (!local.m_StartID.Equals(other1))
      {
        edgeFlags2 |= edgeFlags6;
        edgeFlags3 |= edgeFlags6;
        edgeFlags4 |= edgeFlags8;
        edgeFlags5 |= edgeFlags8;
      }
      if (!local.m_MiddleID.Equals(other2))
      {
        edgeFlags2 |= edgeFlags7;
        edgeFlags3 |= edgeFlags7;
        edgeFlags4 |= edgeFlags7;
        edgeFlags5 |= edgeFlags7;
      }
      if (!local.m_EndID.Equals(other3))
      {
        edgeFlags2 |= edgeFlags8;
        edgeFlags3 |= edgeFlags8;
        edgeFlags4 |= edgeFlags6;
        edgeFlags5 |= edgeFlags6;
      }
      int accessIndex = math.select(-1, specification.m_AccessRequirement, (specification.m_Flags & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary));
      int num1 = math.select(-1, local.m_Specification.m_AccessRequirement, (local.m_Specification.m_Flags & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary));
      Edge edge = local;
      if ((edgeFlags2 & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        this.RemoveConnection(edge.m_StartID, edgeID);
      if ((edgeFlags2 & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        this.RemoveConnection(edge.m_MiddleID, edgeID);
      if ((edgeFlags2 & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        this.RemoveConnection(edge.m_EndID, edgeID);
      if ((edgeFlags3 & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_StartID = this.AddConnection(startNode.StripCurvePos(), edgeID, accessIndex);
      if ((edgeFlags3 & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_MiddleID = this.AddConnection(middleNode.StripCurvePos(), edgeID, accessIndex);
      if ((edgeFlags3 & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_EndID = this.AddConnection(endNode.StripCurvePos(), edgeID, accessIndex);
      if ((edgeFlags4 & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        this.RemoveReversedConnection(edge.m_EndID, edgeID);
      if ((edgeFlags4 & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        this.RemoveReversedConnection(edge.m_MiddleID, edgeID);
      if ((edgeFlags4 & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        this.RemoveReversedConnection(edge.m_StartID, edgeID);
      if ((edgeFlags5 & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_EndID = this.AddReversedConnection(endNode.StripCurvePos(), edgeID, accessIndex);
      if ((edgeFlags5 & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_MiddleID = this.AddReversedConnection(middleNode.StripCurvePos(), edgeID, accessIndex);
      if ((edgeFlags5 & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_StartID = this.AddReversedConnection(startNode.StripCurvePos(), edgeID, accessIndex);
      if (accessIndex != num1)
      {
        EdgeFlags edgeFlags9 = edgeFlags1 & ~edgeFlags3;
        int num2 = (int) (edgeFlags1 & ~edgeFlags5);
        if ((edgeFlags9 & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          this.UpdateAccessRequirement(local.m_StartID, edgeID, accessIndex);
        if ((edgeFlags9 & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          this.UpdateAccessRequirement(local.m_MiddleID, edgeID, accessIndex);
        if ((edgeFlags9 & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
          this.UpdateAccessRequirement(local.m_EndID, edgeID, accessIndex);
        if ((num2 & 1) != 0)
          this.UpdateReversedAccessRequirement(local.m_EndID, edgeID, accessIndex);
        if ((num2 & 4) != 0)
          this.UpdateReversedAccessRequirement(local.m_MiddleID, edgeID, accessIndex);
        if ((num2 & 2) != 0)
          this.UpdateReversedAccessRequirement(local.m_StartID, edgeID, accessIndex);
      }
      if ((specification.m_Flags & (EdgeFlags.Forward | EdgeFlags.Backward)) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        local.m_StartID = new NodeID() { m_Index = -1 };
        local.m_EndID = new NodeID() { m_Index = -1 };
      }
      if ((specification.m_Flags & EdgeFlags.AllowMiddle) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        local.m_MiddleID = new NodeID() { m_Index = -1 };
      local.m_StartCurvePos = startNode.GetCurvePos();
      local.m_EndCurvePos = endNode.GetCurvePos();
      local.m_Specification = specification;
      local.m_Location = location;
    }

    public void SetEdgeDirections(
      EdgeID edgeID,
      PathNode startNode,
      PathNode endNode,
      bool enableForward,
      bool enableBackward)
    {
      ref Edge local = ref this.m_Edges.ElementAt(edgeID.m_Index);
      int accessIndex = math.select(-1, local.m_Specification.m_AccessRequirement, (local.m_Specification.m_Flags & EdgeFlags.AllowEnter) == ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary));
      if (enableForward != ((local.m_Specification.m_Flags & EdgeFlags.Forward) != 0))
      {
        if (enableForward)
        {
          local.m_StartID = this.AddConnection(startNode.StripCurvePos(), edgeID, accessIndex);
          local.m_EndID = this.AddReversedConnection(endNode.StripCurvePos(), edgeID, accessIndex);
          local.m_Specification.m_Flags |= EdgeFlags.Forward;
        }
        else
        {
          this.RemoveConnection(local.m_StartID, edgeID);
          this.RemoveReversedConnection(local.m_EndID, edgeID);
          local.m_Specification.m_Flags &= ~EdgeFlags.Forward;
        }
      }
      if (enableBackward != ((local.m_Specification.m_Flags & EdgeFlags.Backward) != 0))
      {
        if (enableBackward)
        {
          local.m_EndID = this.AddConnection(endNode.StripCurvePos(), edgeID, accessIndex);
          local.m_StartID = this.AddReversedConnection(startNode.StripCurvePos(), edgeID, accessIndex);
          local.m_Specification.m_Flags |= EdgeFlags.Backward;
        }
        else
        {
          this.RemoveConnection(local.m_EndID, edgeID);
          this.RemoveReversedConnection(local.m_StartID, edgeID);
          local.m_Specification.m_Flags &= ~EdgeFlags.Backward;
        }
      }
      if ((local.m_Specification.m_Flags & (EdgeFlags.Forward | EdgeFlags.Backward)) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
        return;
      local.m_StartID = new NodeID() { m_Index = -1 };
      local.m_EndID = new NodeID() { m_Index = -1 };
    }

    public void DestroyEdge(EdgeID edgeID)
    {
      ref Edge local = ref this.m_Edges.ElementAt(edgeID.m_Index);
      if ((local.m_Specification.m_Flags & EdgeFlags.Forward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        this.RemoveConnection(local.m_StartID, edgeID);
        this.RemoveReversedConnection(local.m_EndID, edgeID);
      }
      if ((local.m_Specification.m_Flags & EdgeFlags.AllowMiddle) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        this.RemoveConnection(local.m_MiddleID, edgeID);
        this.RemoveReversedConnection(local.m_MiddleID, edgeID);
      }
      if ((local.m_Specification.m_Flags & EdgeFlags.Backward) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
      {
        this.RemoveConnection(local.m_EndID, edgeID);
        this.RemoveReversedConnection(local.m_StartID, edgeID);
      }
      if (edgeID.m_Index == this.m_Edges.Length - 1)
      {
        this.m_Edges.RemoveAt(edgeID.m_Index);
      }
      else
      {
        this.m_Edges[edgeID.m_Index] = new Edge();
        this.m_FreeIDs.Add(in edgeID);
      }
    }

    public void AddEdge(Entity owner, EdgeID edgeID)
    {
      this.m_Edges.ElementAt(edgeID.m_Index).m_Owner = owner;
      this.m_PathEdges.Add(owner, edgeID);
    }

    public void AddSecondaryEdge(Entity owner, EdgeID edgeID)
    {
      this.m_Edges.ElementAt(edgeID.m_Index).m_Owner = owner;
      this.m_SecondaryEdges.Add(owner, edgeID);
    }

    public bool GetEdge(Entity owner, out EdgeID edgeID)
    {
      return this.m_PathEdges.TryGetValue(owner, out edgeID);
    }

    public bool GetSecondaryEdge(Entity owner, out EdgeID edgeID)
    {
      return this.m_SecondaryEdges.TryGetValue(owner, out edgeID);
    }

    public bool RemoveEdge(Entity owner, out EdgeID edgeID)
    {
      if (!this.m_PathEdges.TryGetValue(owner, out edgeID))
        return false;
      this.m_PathEdges.Remove(owner);
      return true;
    }

    public bool RemoveSecondaryEdge(Entity owner, out EdgeID edgeID)
    {
      if (!this.m_SecondaryEdges.TryGetValue(owner, out edgeID))
        return false;
      this.m_SecondaryEdges.Remove(owner);
      return true;
    }

    public unsafe void SwapConnections()
    {
      void* connections = this.m_Connections;
      this.m_Connections = this.m_ReversedConnections;
      this.m_ReversedConnections = connections;
    }

    private NodeID AddConnection(PathNode pathNode, EdgeID edgeID, int accessIndex)
    {
      NodeID nodeID;
      if (this.m_NodeIDs.TryGetValue(pathNode, out nodeID))
      {
        ref int local1 = ref this.GetConnectionCount(nodeID);
        ref int local2 = ref this.GetConnectionCapacity(nodeID);
        int connectionCapacity = (local1 << 1) + 2;
        if (connectionCapacity > local2)
        {
          this.m_PathNodes.Remove(nodeID);
          this.ResizeConnections(ref nodeID, connectionCapacity);
          local1 = ref this.GetConnectionCount(nodeID);
          this.m_NodeIDs[pathNode] = nodeID;
          this.m_PathNodes.Add(nodeID, pathNode);
        }
        ref int local3 = ref this.GetConnection(nodeID, local1);
        ref int local4 = ref this.GetAccessRequirement(nodeID, local1);
        local3 = edgeID.m_Index;
        int num = accessIndex;
        local4 = num;
        ++local1;
        return nodeID;
      }
      NodeID connections = this.CreateConnections(2);
      ref int local5 = ref this.GetConnectionCount(connections);
      ref int local6 = ref this.GetConnection(connections, local5);
      ref int local7 = ref this.GetAccessRequirement(connections, local5);
      local6 = edgeID.m_Index;
      int num1 = accessIndex;
      local7 = num1;
      ++local5;
      this.m_NodeIDs.Add(pathNode, connections);
      this.m_PathNodes.Add(connections, pathNode);
      ++this.m_NodeCount;
      return connections;
    }

    private NodeID AddReversedConnection(PathNode pathNode, EdgeID edgeID, int accessIndex)
    {
      NodeID nodeID;
      if (this.m_NodeIDs.TryGetValue(pathNode, out nodeID))
      {
        ref int local1 = ref this.GetReversedConnectionCount(nodeID);
        ref int local2 = ref this.GetReversedConnectionCapacity(nodeID);
        int connectionCapacity = (local1 << 1) + 2;
        if (connectionCapacity > local2)
        {
          this.m_PathNodes.Remove(nodeID);
          this.ResizeConnections(ref nodeID, connectionCapacity);
          local1 = ref this.GetReversedConnectionCount(nodeID);
          this.m_NodeIDs[pathNode] = nodeID;
          this.m_PathNodes.Add(nodeID, pathNode);
        }
        ref int local3 = ref this.GetReversedConnection(nodeID, local1);
        ref int local4 = ref this.GetReversedAccessRequirement(nodeID, local1);
        local3 = edgeID.m_Index;
        int num = accessIndex;
        local4 = num;
        ++local1;
        return nodeID;
      }
      NodeID connections = this.CreateConnections(2);
      ref int local5 = ref this.GetReversedConnectionCount(connections);
      ref int local6 = ref this.GetReversedConnection(connections, local5);
      ref int local7 = ref this.GetReversedAccessRequirement(connections, local5);
      local6 = edgeID.m_Index;
      int num1 = accessIndex;
      local7 = num1;
      ++local5;
      this.m_NodeIDs.Add(pathNode, connections);
      this.m_PathNodes.Add(connections, pathNode);
      ++this.m_NodeCount;
      return connections;
    }

    private void RemoveConnection(NodeID nodeID, EdgeID edgeID)
    {
      ref int local1 = ref this.GetConnectionCount(nodeID);
      for (int connectionIndex = 0; connectionIndex < local1; ++connectionIndex)
      {
        ref int local2 = ref this.GetConnection(nodeID, connectionIndex);
        if (local2 == edgeID.m_Index)
        {
          int num1 = connectionIndex;
          ref int local3 = ref local1;
          int num2 = local1 - 1;
          int num3 = num2;
          local3 = num3;
          int num4 = num2;
          if (num1 != num4)
          {
            ref int local4 = ref this.GetAccessRequirement(nodeID, connectionIndex);
            local2 = this.GetConnection(nodeID, local1);
            int num5 = this.GetAccessRequirement(nodeID, local1);
            local4 = num5;
            break;
          }
          if (local1 != 0 || this.GetReversedConnectionCount(nodeID) != 0)
            break;
          this.m_NodeIDs.Remove(this.m_PathNodes[nodeID]);
          this.m_PathNodes.Remove(nodeID);
          this.DestroyConnections(nodeID);
          --this.m_NodeCount;
          break;
        }
      }
    }

    private void RemoveReversedConnection(NodeID nodeID, EdgeID edgeID)
    {
      ref int local1 = ref this.GetReversedConnectionCount(nodeID);
      for (int connectionIndex = 0; connectionIndex < local1; ++connectionIndex)
      {
        ref int local2 = ref this.GetReversedConnection(nodeID, connectionIndex);
        if (local2 == edgeID.m_Index)
        {
          int num1 = connectionIndex;
          ref int local3 = ref local1;
          int num2 = local1 - 1;
          int num3 = num2;
          local3 = num3;
          int num4 = num2;
          if (num1 != num4)
          {
            ref int local4 = ref this.GetReversedAccessRequirement(nodeID, connectionIndex);
            local2 = this.GetReversedConnection(nodeID, local1);
            int num5 = this.GetReversedAccessRequirement(nodeID, local1);
            local4 = num5;
            break;
          }
          if (local1 != 0 || this.GetConnectionCount(nodeID) != 0)
            break;
          this.m_NodeIDs.Remove(this.m_PathNodes[nodeID]);
          this.m_PathNodes.Remove(nodeID);
          this.DestroyConnections(nodeID);
          --this.m_NodeCount;
          break;
        }
      }
    }

    private void UpdateAccessRequirement(NodeID nodeID, EdgeID edgeID, int accessIndex)
    {
      ref int local = ref this.GetConnectionCount(nodeID);
      for (int connectionIndex = 0; connectionIndex < local; ++connectionIndex)
      {
        if (this.GetConnection(nodeID, connectionIndex) == edgeID.m_Index)
        {
          this.GetAccessRequirement(nodeID, connectionIndex) = accessIndex;
          break;
        }
      }
    }

    private void UpdateReversedAccessRequirement(NodeID nodeID, EdgeID edgeID, int accessIndex)
    {
      ref int local = ref this.GetReversedConnectionCount(nodeID);
      for (int connectionIndex = 0; connectionIndex < local; ++connectionIndex)
      {
        if (this.GetReversedConnection(nodeID, connectionIndex) == edgeID.m_Index)
        {
          this.GetReversedAccessRequirement(nodeID, connectionIndex) = accessIndex;
          break;
        }
      }
    }

    private NodeID CreateConnections(int connectionCapacity)
    {
      UnsafeHeapBlock unsafeHeapBlock = this.AllocateConnections(connectionCapacity);
      NodeID nodeID = new NodeID()
      {
        m_Index = (int) unsafeHeapBlock.begin
      };
      ref int local1 = ref this.GetConnectionCount(nodeID);
      ref int local2 = ref this.GetConnectionCapacity(nodeID);
      local1 = 0;
      int num1 = (int) unsafeHeapBlock.end - (int) unsafeHeapBlock.begin - 2;
      local2 = num1;
      ref int local3 = ref this.GetReversedConnectionCount(nodeID);
      ref int local4 = ref this.GetReversedConnectionCapacity(nodeID);
      local3 = 0;
      int num2 = (int) unsafeHeapBlock.end - (int) unsafeHeapBlock.begin - 2;
      local4 = num2;
      return nodeID;
    }

    private void ResizeConnections(ref NodeID nodeID, int connectionCapacity)
    {
      UnsafeHeapBlock unsafeHeapBlock = this.AllocateConnections(connectionCapacity);
      UnsafeHeapBlock block = new UnsafeHeapBlock((uint) nodeID.m_Index, (uint) (nodeID.m_Index + this.GetConnectionCapacity(nodeID) + 2));
      NodeID nodeID1 = new NodeID()
      {
        m_Index = (int) unsafeHeapBlock.begin
      };
      ref int local1 = ref this.GetConnectionCount(nodeID);
      ref int local2 = ref this.GetConnectionCount(nodeID1);
      ref int local3 = ref this.GetConnectionCapacity(nodeID1);
      local2 = local1;
      int num1 = (int) unsafeHeapBlock.end - (int) unsafeHeapBlock.begin - 2;
      local3 = num1;
      for (int connectionIndex = 0; connectionIndex < local1; ++connectionIndex)
      {
        ref int local4 = ref this.GetConnection(nodeID, connectionIndex);
        ref int local5 = ref this.GetAccessRequirement(nodeID, connectionIndex);
        ref int local6 = ref this.GetConnection(nodeID1, connectionIndex);
        ref int local7 = ref this.GetAccessRequirement(nodeID1, connectionIndex);
        local6 = local4;
        int num2 = local5;
        local7 = num2;
        ref Edge local8 = ref this.m_Edges.ElementAt(local4);
        local8.m_StartID.m_Index = math.select(local8.m_StartID.m_Index, nodeID1.m_Index, local8.m_StartID.Equals(nodeID));
        local8.m_MiddleID.m_Index = math.select(local8.m_MiddleID.m_Index, nodeID1.m_Index, local8.m_MiddleID.Equals(nodeID));
        local8.m_EndID.m_Index = math.select(local8.m_EndID.m_Index, nodeID1.m_Index, local8.m_EndID.Equals(nodeID));
      }
      ref int local9 = ref this.GetReversedConnectionCount(nodeID);
      ref int local10 = ref this.GetReversedConnectionCount(nodeID1);
      ref int local11 = ref this.GetReversedConnectionCapacity(nodeID1);
      local10 = local9;
      int num3 = (int) unsafeHeapBlock.end - (int) unsafeHeapBlock.begin - 2;
      local11 = num3;
      for (int connectionIndex = 0; connectionIndex < local9; ++connectionIndex)
      {
        ref int local12 = ref this.GetReversedConnection(nodeID, connectionIndex);
        ref int local13 = ref this.GetReversedAccessRequirement(nodeID, connectionIndex);
        ref int local14 = ref this.GetReversedConnection(nodeID1, connectionIndex);
        ref int local15 = ref this.GetReversedAccessRequirement(nodeID1, connectionIndex);
        local14 = local12;
        int num4 = local13;
        local15 = num4;
        ref Edge local16 = ref this.m_Edges.ElementAt(local12);
        local16.m_StartID.m_Index = math.select(local16.m_StartID.m_Index, nodeID1.m_Index, local16.m_StartID.Equals(nodeID));
        local16.m_MiddleID.m_Index = math.select(local16.m_MiddleID.m_Index, nodeID1.m_Index, local16.m_MiddleID.Equals(nodeID));
        local16.m_EndID.m_Index = math.select(local16.m_EndID.m_Index, nodeID1.m_Index, local16.m_EndID.Equals(nodeID));
      }
      this.m_ConnectionAllocator.Release(block);
      nodeID = nodeID1;
    }

    private void DestroyConnections(NodeID nodeID)
    {
      ref int local = ref this.GetConnectionCapacity(nodeID);
      this.m_ConnectionAllocator.Release(new UnsafeHeapBlock((uint) nodeID.m_Index, (uint) (nodeID.m_Index + local + 2)));
    }

    private unsafe UnsafeHeapBlock AllocateConnections(int connectionCapacity)
    {
      uint size1 = (uint) (connectionCapacity + 2);
      UnsafeHeapBlock unsafeHeapBlock = this.m_ConnectionAllocator.Allocate(size1);
      if (!unsafeHeapBlock.Empty)
        return unsafeHeapBlock;
      this.m_ConnectionAllocator.Resize(math.max(this.m_ConnectionAllocator.Size * 3U / 2U, this.m_ConnectionAllocator.Size + size1));
      void* destination1 = UnsafeUtility.Malloc((long) (this.m_ConnectionAllocator.Size * 4U), (int) this.m_ConnectionAllocator.MinimumAlignment * 4, this.m_AllocatorLabel);
      void* destination2 = UnsafeUtility.Malloc((long) (this.m_ConnectionAllocator.Size * 4U), (int) this.m_ConnectionAllocator.MinimumAlignment * 4, this.m_AllocatorLabel);
      uint size2 = this.m_ConnectionAllocator.OnePastHighestUsedAddress * 4U;
      if (size2 != 0U)
      {
        UnsafeUtility.MemCpy(destination1, this.m_Connections, (long) size2);
        UnsafeUtility.MemCpy(destination2, this.m_ReversedConnections, (long) size2);
      }
      UnsafeUtility.Free(this.m_Connections, this.m_AllocatorLabel);
      UnsafeUtility.Free(this.m_ReversedConnections, this.m_AllocatorLabel);
      this.m_Connections = destination1;
      this.m_ReversedConnections = destination2;
      return this.m_ConnectionAllocator.Allocate(size1);
    }

    public ref Edge GetEdge(EdgeID edgeID) => ref this.m_Edges.ElementAt(edgeID.m_Index);

    public unsafe ref int GetConnectionCount(NodeID nodeID)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_Connections + (IntPtr) nodeID.m_Index * 4);
    }

    public unsafe ref int GetReversedConnectionCount(NodeID nodeID)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_ReversedConnections + (IntPtr) nodeID.m_Index * 4);
    }

    public unsafe ref int GetConnectionCapacity(NodeID nodeID)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_Connections + (IntPtr) (nodeID.m_Index + 1) * 4);
    }

    public unsafe ref int GetReversedConnectionCapacity(NodeID nodeID)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_ReversedConnections + (IntPtr) (nodeID.m_Index + 1) * 4);
    }

    public unsafe ref int GetConnection(NodeID nodeID, int connectionIndex)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_Connections + (IntPtr) (nodeID.m_Index + (connectionIndex << 1) + 2) * 4);
    }

    public unsafe ref int GetReversedConnection(NodeID nodeID, int connectionIndex)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_ReversedConnections + (IntPtr) (nodeID.m_Index + (connectionIndex << 1) + 2) * 4);
    }

    public unsafe ref int GetAccessRequirement(NodeID nodeID, int connectionIndex)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_Connections + (IntPtr) (nodeID.m_Index + (connectionIndex << 1) + 3) * 4);
    }

    public unsafe ref int GetReversedAccessRequirement(NodeID nodeID, int connectionIndex)
    {
      // ISSUE: cast to a reference type
      return (int&) ((IntPtr) this.m_ReversedConnections + (IntPtr) (nodeID.m_Index + (connectionIndex << 1) + 3) * 4);
    }
  }
}
