// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ConnectedBuilding
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  [InternalBufferCapacity(0)]
  public struct ConnectedBuilding : 
    IBufferElementData,
    IEquatable<ConnectedBuilding>,
    IEmptySerializable
  {
    public Entity m_Building;

    public ConnectedBuilding(Entity building) => this.m_Building = building;

    public bool Equals(ConnectedBuilding other) => this.m_Building.Equals(other.m_Building);

    public override int GetHashCode() => this.m_Building.GetHashCode();
  }
}
