// Decompiled with JetBrains decompiler
// Type: Game.Net.Bottleneck
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct Bottleneck : IComponentData, IQueryTypeParameter, ISerializable
  {
    public byte m_Position;
    public byte m_MinPos;
    public byte m_MaxPos;
    public byte m_Timer;

    public Bottleneck(byte minPos, byte maxPos, byte timer)
    {
      this.m_Position = (byte) ((int) minPos + (int) maxPos + 1 >> 1);
      this.m_MinPos = minPos;
      this.m_MaxPos = maxPos;
      this.m_Timer = timer;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_MinPos);
      writer.Write(this.m_MaxPos);
      writer.Write(this.m_Timer);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      if (reader.context.version >= Version.trafficBottleneckPosition)
      {
        reader.Read(out this.m_MinPos);
        reader.Read(out this.m_MaxPos);
      }
      else
      {
        this.m_MinPos = this.m_Position;
        this.m_MaxPos = this.m_Position;
      }
      reader.Read(out this.m_Timer);
    }
  }
}
