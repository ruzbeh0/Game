// Decompiled with JetBrains decompiler
// Type: Game.Buildings.Patient
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct Patient : IBufferElementData, IEquatable<Patient>, ISerializable
  {
    public Entity m_Patient;

    public Patient(Entity patient) => this.m_Patient = patient;

    public bool Equals(Patient other) => this.m_Patient.Equals(other.m_Patient);

    public override int GetHashCode() => this.m_Patient.GetHashCode();

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Patient);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Patient);
    }
  }
}
