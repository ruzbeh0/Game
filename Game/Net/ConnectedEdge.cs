// Decompiled with JetBrains decompiler
// Type: Game.Net.ConnectedEdge
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  [InternalBufferCapacity(4)]
  public struct ConnectedEdge : IBufferElementData, IEquatable<ConnectedEdge>, IEmptySerializable
  {
    public Entity m_Edge;

    public ConnectedEdge(Entity edge) => this.m_Edge = edge;

    public bool Equals(ConnectedEdge other) => this.m_Edge.Equals(other.m_Edge);

    public override int GetHashCode() => this.m_Edge.GetHashCode();
  }
}
