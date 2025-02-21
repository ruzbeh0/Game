// Decompiled with JetBrains decompiler
// Type: Game.Buildings.Occupant
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct Occupant : IBufferElementData, IEquatable<Occupant>, ISerializable
  {
    public Entity m_Occupant;

    public Occupant(Entity occupant) => this.m_Occupant = occupant;

    public bool Equals(Occupant other) => this.m_Occupant.Equals(other.m_Occupant);

    public override int GetHashCode() => this.m_Occupant.GetHashCode();

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Occupant);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Occupant);
    }

    public static implicit operator Entity(Occupant occupant) => occupant.m_Occupant;
  }
}
