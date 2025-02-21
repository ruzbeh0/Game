// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.GuestVehicle
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
  public struct GuestVehicle : IBufferElementData, IEquatable<GuestVehicle>, IEmptySerializable
  {
    public Entity m_Vehicle;

    public GuestVehicle(Entity vehicle) => this.m_Vehicle = vehicle;

    public bool Equals(GuestVehicle other) => this.m_Vehicle.Equals(other.m_Vehicle);

    public override int GetHashCode() => this.m_Vehicle.GetHashCode();
  }
}
