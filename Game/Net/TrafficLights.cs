// Decompiled with JetBrains decompiler
// Type: Game.Net.TrafficLights
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct TrafficLights : IComponentData, IQueryTypeParameter, ISerializable
  {
    public TrafficLightState m_State;
    public TrafficLightFlags m_Flags;
    public byte m_SignalGroupCount;
    public byte m_CurrentSignalGroup;
    public byte m_NextSignalGroup;
    public byte m_Timer;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_State);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_SignalGroupCount);
      writer.Write(this.m_CurrentSignalGroup);
      writer.Write(this.m_NextSignalGroup);
      writer.Write(this.m_Timer);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      reader.Read(out this.m_SignalGroupCount);
      reader.Read(out this.m_CurrentSignalGroup);
      if (reader.context.version >= Version.nextLaneSignal)
        reader.Read(out this.m_NextSignalGroup);
      reader.Read(out this.m_Timer);
      this.m_State = (TrafficLightState) num1;
      this.m_Flags = (TrafficLightFlags) num2;
    }
  }
}
