// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.Passenger
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  [InternalBufferCapacity(0)]
  public struct Passenger : IBufferElementData, IEquatable<Passenger>, IEmptySerializable
  {
    public Entity m_Passenger;

    public Passenger(Entity passenger) => this.m_Passenger = passenger;

    public bool Equals(Passenger other) => this.m_Passenger.Equals(other.m_Passenger);

    public override int GetHashCode() => this.m_Passenger.GetHashCode();
  }
}
