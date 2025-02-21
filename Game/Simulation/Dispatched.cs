// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Dispatched
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct Dispatched : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Handler;

    public Dispatched(Entity handler) => this.m_Handler = handler;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Handler);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Handler);
      if (!(reader.context.version < Version.dispatchRefactoring))
        return;
      reader.Read(out uint _);
    }
  }
}
