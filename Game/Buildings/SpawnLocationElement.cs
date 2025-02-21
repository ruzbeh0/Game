// Decompiled with JetBrains decompiler
// Type: Game.Buildings.SpawnLocationElement
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
  public struct SpawnLocationElement : 
    IBufferElementData,
    IEquatable<SpawnLocationElement>,
    IEmptySerializable
  {
    public Entity m_SpawnLocation;
    public SpawnLocationType m_Type;

    public SpawnLocationElement(Entity spawnLocation, SpawnLocationType type)
    {
      this.m_SpawnLocation = spawnLocation;
      this.m_Type = type;
    }

    public bool Equals(SpawnLocationElement other)
    {
      return this.m_SpawnLocation.Equals(other.m_SpawnLocation);
    }

    public override int GetHashCode() => this.m_SpawnLocation.GetHashCode();
  }
}
