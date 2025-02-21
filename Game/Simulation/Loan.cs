// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Loan
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct Loan : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Amount;
    public uint m_LastModified;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Amount);
      if (reader.context.version < Version.noAutoPaybackLoans)
      {
        reader.Read(out float _);
        reader.Read(out int _);
      }
      if (!(reader.context.version > Version.loanLastModified))
        return;
      reader.Read(out this.m_LastModified);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Amount);
      writer.Write(this.m_LastModified);
    }
  }
}
