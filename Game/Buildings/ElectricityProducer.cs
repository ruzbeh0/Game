// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ElectricityProducer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct ElectricityProducer : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Capacity;
    public int m_LastProduction;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Capacity);
      writer.Write(this.m_LastProduction);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Capacity);
      Context context;
      if (reader.context.version >= Version.powerPlantConsumption)
      {
        context = reader.context;
        if (context.version < Version.serviceConsumption)
          reader.Read(out int _);
      }
      context = reader.context;
      if (!(context.version >= Version.powerPlantLastFlow))
        return;
      reader.Read(out this.m_LastProduction);
    }
  }
}
