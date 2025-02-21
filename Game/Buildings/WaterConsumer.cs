// Decompiled with JetBrains decompiler
// Type: Game.Buildings.WaterConsumer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct WaterConsumer : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_Pollution;
    public int m_WantedConsumption;
    public int m_FulfilledFresh;
    public int m_FulfilledSewage;
    public byte m_FreshCooldownCounter;
    public byte m_SewageCooldownCounter;
    public WaterConsumerFlags m_Flags;

    public bool waterConnected => (this.m_Flags & WaterConsumerFlags.WaterConnected) != 0;

    public bool sewageConnected => (this.m_Flags & WaterConsumerFlags.SewageConnected) != 0;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Pollution);
      writer.Write(this.m_WantedConsumption);
      writer.Write(this.m_FulfilledFresh);
      writer.Write(this.m_FulfilledSewage);
      writer.Write(this.m_FreshCooldownCounter);
      writer.Write(this.m_SewageCooldownCounter);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.waterPipePollution)
        reader.Read(out this.m_Pollution);
      else
        reader.Read(out int _);
      if (reader.context.version < Version.waterPipeFlowSim)
      {
        reader.Read(out int _);
        reader.Read(out int _);
      }
      if (reader.context.version >= Version.waterConsumption)
        reader.Read(out this.m_WantedConsumption);
      if (reader.context.version < Version.buildingEfficiencyRework)
      {
        if (reader.context.version >= Version.utilityFeePrecision)
          reader.Read(out float _);
        else if (reader.context.version >= Version.waterFee)
          reader.Read(out int _);
      }
      if (reader.context.version >= Version.waterPipeFlowSim)
      {
        reader.Read(out this.m_FulfilledFresh);
        reader.Read(out this.m_FulfilledSewage);
        reader.Read(out this.m_FreshCooldownCounter);
        reader.Read(out this.m_SewageCooldownCounter);
      }
      if (!(reader.context.version >= Version.waterConsumerFlags))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (WaterConsumerFlags) num;
    }
  }
}
