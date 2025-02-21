// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ConnectedFlowEdge
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ConnectedFlowEdge : 
    IBufferElementData,
    IEmptySerializable,
    IEquatable<ConnectedFlowEdge>
  {
    public Entity m_Edge;

    public ConnectedFlowEdge(Entity edge) => this.m_Edge = edge;

    public bool Equals(ConnectedFlowEdge other) => this.m_Edge.Equals(other.m_Edge);

    public override int GetHashCode() => this.m_Edge.GetHashCode();

    public static implicit operator Entity(ConnectedFlowEdge element) => element.m_Edge;
  }
}
