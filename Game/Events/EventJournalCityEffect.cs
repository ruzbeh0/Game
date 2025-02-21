// Decompiled with JetBrains decompiler
// Type: Game.Events.EventJournalCityEffect
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public struct EventJournalCityEffect : IBufferElementData, ISerializable
  {
    public EventCityEffectTrackingType m_Type;
    public int m_StartValue;
    public int m_Value;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num;
      reader.Read(out num);
      this.m_Type = (EventCityEffectTrackingType) num;
      reader.Read(out this.m_StartValue);
      reader.Read(out this.m_Value);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((int) this.m_Type);
      writer.Write(this.m_StartValue);
      writer.Write(this.m_Value);
    }
  }
}
