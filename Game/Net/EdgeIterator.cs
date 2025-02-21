// Decompiled with JetBrains decompiler
// Type: Game.Net.EdgeIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Tools;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct EdgeIterator
  {
    private BufferLookup<ConnectedEdge> m_Edges;
    private ComponentLookup<Edge> m_EdgeData;
    private ComponentLookup<Temp> m_TempData;
    private ComponentLookup<Hidden> m_HiddenData;
    private DynamicBuffer<ConnectedEdge> m_Buffer;
    private int m_Iterator;
    private Entity m_Node;
    private Entity m_Edge;
    private Entity m_OriginalEdge;
    private bool m_Permanent;
    private bool m_Delete;
    private bool m_Middles;

    public EdgeIterator(
      Entity edge,
      Entity node,
      BufferLookup<ConnectedEdge> edges,
      ComponentLookup<Edge> edgeData,
      ComponentLookup<Temp> tempData,
      ComponentLookup<Hidden> hiddenData,
      bool includeMiddleConnections = false)
    {
      this.m_Node = node;
      this.m_Edge = edge;
      this.m_OriginalEdge = Entity.Null;
      this.m_Edges = edges;
      this.m_EdgeData = edgeData;
      this.m_TempData = tempData;
      this.m_HiddenData = hiddenData;
      this.m_Buffer = this.m_Edges[node];
      this.m_Iterator = 0;
      this.m_Permanent = !this.m_TempData.HasComponent(node);
      this.m_Delete = false;
      this.m_Middles = includeMiddleConnections;
      if (edge != Entity.Null)
      {
        this.m_Delete = this.GetDelete(edge, out this.m_OriginalEdge);
      }
      else
      {
        if (this.m_Permanent)
          return;
        this.m_Delete = this.GetDelete(node);
      }
    }

    private bool GetDelete(Entity entity)
    {
      Temp componentData;
      return this.m_TempData.TryGetComponent(entity, out componentData) && (componentData.m_Flags & TempFlags.Delete) > (TempFlags) 0;
    }

    private bool GetDelete(Entity entity, out Entity original)
    {
      Temp componentData;
      if (this.m_TempData.TryGetComponent(entity, out componentData))
      {
        original = componentData.m_Original;
        return (componentData.m_Flags & TempFlags.Delete) > (TempFlags) 0;
      }
      original = Entity.Null;
      return false;
    }

    public bool GetNext(out EdgeIteratorValue value)
    {
      while (true)
      {
        bool flag = this.m_Buffer.Length > this.m_Iterator;
        value.m_Edge = !flag ? Entity.Null : this.m_Buffer[this.m_Iterator++].m_Edge;
        while (flag)
        {
          if (this.m_Permanent)
          {
            Edge edge = this.m_EdgeData[value.m_Edge];
            value.m_End = edge.m_End == this.m_Node;
            if (value.m_End || edge.m_Start == this.m_Node)
            {
              value.m_Middle = false;
              return true;
            }
            if (this.m_Middles)
            {
              value.m_Middle = true;
              return true;
            }
          }
          else if (this.m_Delete)
          {
            if (value.m_Edge == this.m_Edge || this.m_HiddenData.HasComponent(value.m_Edge) && value.m_Edge != this.m_OriginalEdge)
            {
              Edge edge = this.m_EdgeData[value.m_Edge];
              value.m_End = edge.m_End == this.m_Node;
              if (value.m_End || edge.m_Start == this.m_Node)
              {
                value.m_Middle = false;
                return true;
              }
              if (this.m_Middles)
              {
                value.m_Middle = true;
                return true;
              }
            }
          }
          else if (!this.m_HiddenData.HasComponent(value.m_Edge) && !this.GetDelete(value.m_Edge))
          {
            Edge edge = this.m_EdgeData[value.m_Edge];
            value.m_End = edge.m_End == this.m_Node;
            if (value.m_End || edge.m_Start == this.m_Node)
            {
              value.m_Middle = false;
              return true;
            }
            if (this.m_Middles)
            {
              value.m_Middle = true;
              return true;
            }
          }
          flag = this.m_Buffer.Length > this.m_Iterator;
          value.m_Edge = !flag ? Entity.Null : this.m_Buffer[this.m_Iterator++].m_Edge;
        }
        Temp componentData;
        if (this.m_TempData.TryGetComponent(this.m_Node, out componentData))
        {
          this.m_Node = componentData.m_Original;
          if (this.m_Edges.TryGetBuffer(this.m_Node, out this.m_Buffer))
            this.m_Iterator = 0;
          else
            break;
        }
        else
          break;
      }
      value.m_Edge = Entity.Null;
      value.m_End = false;
      value.m_Middle = false;
      return false;
    }
  }
}
