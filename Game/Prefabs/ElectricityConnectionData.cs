// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ElectricityConnectionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ElectricityConnectionData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Capacity;
    public FlowDirection m_Direction;
    public ElectricityConnection.Voltage m_Voltage;
    public CompositionFlags m_CompositionAll;
    public CompositionFlags m_CompositionAny;
    public CompositionFlags m_CompositionNone;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Capacity);
      writer.Write((byte) this.m_Direction);
      writer.Write((byte) this.m_Voltage);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Capacity);
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      this.m_Direction = (FlowDirection) num1;
      this.m_Voltage = (ElectricityConnection.Voltage) num2;
    }
  }
}
