// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PersonalCarData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PersonalCarData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_PassengerCapacity;
    public int m_BaggageCapacity;
    public int m_CostToDrive;
    public int m_Probability;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PassengerCapacity);
      writer.Write(this.m_BaggageCapacity);
      writer.Write(this.m_CostToDrive);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PassengerCapacity);
      reader.Read(out this.m_BaggageCapacity);
      reader.Read(out this.m_CostToDrive);
    }
  }
}
