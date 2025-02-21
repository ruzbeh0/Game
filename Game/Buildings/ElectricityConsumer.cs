// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ElectricityConsumer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct ElectricityConsumer : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_WantedConsumption;
    public int m_FulfilledConsumption;
    public short m_CooldownCounter;
    public ElectricityConsumerFlags m_Flags;

    public bool electricityConnected => (this.m_Flags & ElectricityConsumerFlags.Connected) != 0;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_WantedConsumption);
      writer.Write(this.m_FulfilledConsumption);
      writer.Write(this.m_CooldownCounter);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_WantedConsumption);
      reader.Read(out this.m_FulfilledConsumption);
      if (reader.context.version < Version.electricityFlashFix)
        reader.Read(out int _);
      else
        reader.Read(out this.m_CooldownCounter);
      Context context = reader.context;
      if (context.version >= Version.notificationData)
      {
        context = reader.context;
        if (context.version >= Version.bottleneckNotification)
        {
          byte num;
          reader.Read(out num);
          this.m_Flags = (ElectricityConsumerFlags) num;
        }
        else
        {
          bool flag;
          reader.Read(out flag);
          if (!flag)
            this.m_Flags = ElectricityConsumerFlags.Connected;
        }
      }
      context = reader.context;
      if (!(context.version < Version.buildingEfficiencyRework))
        return;
      context = reader.context;
      if (context.version >= Version.utilityFeePrecision)
      {
        reader.Read(out float _);
      }
      else
      {
        context = reader.context;
        if (!(context.version >= Version.electricityFeeEffect))
          return;
        reader.Read(out int _);
      }
    }
  }
}
