// Decompiled with JetBrains decompiler
// Type: Game.Objects.TrafficLight
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  public struct TrafficLight : IComponentData, IQueryTypeParameter, ISerializable
  {
    public TrafficLightState m_State;
    public ushort m_GroupMask0;
    public ushort m_GroupMask1;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((ushort) this.m_State);
      writer.Write(this.m_GroupMask0);
      writer.Write(this.m_GroupMask1);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      ushort num;
      reader.Read(out num);
      if (reader.context.version >= Version.trafficLightGroups)
      {
        reader.Read(out this.m_GroupMask0);
        reader.Read(out this.m_GroupMask1);
      }
      this.m_State = (TrafficLightState) num;
    }
  }
}
