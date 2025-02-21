// Decompiled with JetBrains decompiler
// Type: Game.Creatures.OwnedCreature
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  [InternalBufferCapacity(0)]
  public struct OwnedCreature : IBufferElementData, IEquatable<OwnedCreature>, IEmptySerializable
  {
    public Entity m_Creature;

    public OwnedCreature(Entity creature) => this.m_Creature = creature;

    public bool Equals(OwnedCreature other) => this.m_Creature.Equals(other.m_Creature);

    public override int GetHashCode() => this.m_Creature.GetHashCode();
  }
}
