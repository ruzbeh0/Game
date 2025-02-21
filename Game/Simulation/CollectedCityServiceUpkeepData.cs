// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CollectedCityServiceUpkeepData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct CollectedCityServiceUpkeepData : IBufferElementData, ISerializable
  {
    public Resource m_Resource;
    public int m_FullCost;
    public int m_Amount;
    public int m_Cost;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num;
      reader.Read(out num);
      this.m_Resource = (Resource) num;
      reader.Read(out this.m_FullCost);
      reader.Read(out this.m_Amount);
      reader.Read(out this.m_Cost);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((int) this.m_Resource);
      writer.Write(this.m_FullCost);
      writer.Write(this.m_Amount);
      writer.Write(this.m_Cost);
    }
  }
}
