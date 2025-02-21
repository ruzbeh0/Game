// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.Connection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Collections;

#nullable disable
namespace Game.Simulation.Flow
{
  public struct Connection
  {
    public int m_StartNode;
    public int m_EndNode;
    public int m_Edge;
    public bool m_Backwards;

    public Connection Reverse()
    {
      return new Connection()
      {
        m_StartNode = this.m_EndNode,
        m_EndNode = this.m_StartNode,
        m_Edge = this.m_Edge,
        m_Backwards = !this.m_Backwards
      };
    }

    public int GetOutgoingCapacity(NativeArray<Edge> edges)
    {
      return edges[this.m_Edge].GetCapacity(this.m_Backwards);
    }

    public int GetIncomingCapacity(NativeArray<Edge> edges)
    {
      return edges[this.m_Edge].GetCapacity(!this.m_Backwards);
    }

    public int GetOutgoingFinalFlow(NativeArray<Edge> edges)
    {
      return edges[this.m_Edge].GetFinalFlow(this.m_Backwards);
    }

    public int GetIncomingFinalFlow(NativeArray<Edge> edges)
    {
      return edges[this.m_Edge].GetFinalFlow(!this.m_Backwards);
    }

    public int GetOutgoingResidualCapacity(NativeArray<Edge> edges)
    {
      return edges[this.m_Edge].GetResidualCapacity(this.m_Backwards);
    }

    public int GetIncomingResidualCapacity(NativeArray<Edge> edges)
    {
      return edges[this.m_Edge].GetResidualCapacity(!this.m_Backwards);
    }
  }
}
