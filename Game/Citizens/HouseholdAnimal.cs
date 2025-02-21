// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HouseholdAnimal
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  [InternalBufferCapacity(1)]
  public struct HouseholdAnimal : IBufferElementData, IEquatable<HouseholdAnimal>, IEmptySerializable
  {
    public Entity m_HouseholdPet;

    public HouseholdAnimal(Entity householdPet) => this.m_HouseholdPet = householdPet;

    public bool Equals(HouseholdAnimal other) => this.m_HouseholdPet.Equals(other.m_HouseholdPet);

    public override int GetHashCode() => this.m_HouseholdPet.GetHashCode();
  }
}
