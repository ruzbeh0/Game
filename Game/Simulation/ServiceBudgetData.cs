// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ServiceBudgetData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ServiceBudgetData : IBufferElementData, ISerializable
  {
    public Entity m_Service;
    public int m_Budget;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Service);
      reader.Read(out this.m_Budget);
      if (!(reader.context.version < Version.serviceImportBudgets))
        return;
      reader.Read(out int _);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Service);
      writer.Write(this.m_Budget);
    }
  }
}
