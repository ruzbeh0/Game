// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HouseholdCitizen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  [InternalBufferCapacity(5)]
  public struct HouseholdCitizen : 
    IBufferElementData,
    IEquatable<HouseholdCitizen>,
    IEmptySerializable
  {
    public Entity m_Citizen;

    public HouseholdCitizen(Entity citizen) => this.m_Citizen = citizen;

    public bool Equals(HouseholdCitizen other) => this.m_Citizen.Equals(other.m_Citizen);

    public override int GetHashCode() => this.m_Citizen.GetHashCode();

    public static implicit operator Entity(HouseholdCitizen citizen) => citizen.m_Citizen;
  }
}
