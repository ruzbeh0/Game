// Decompiled with JetBrains decompiler
// Type: Game.Citizens.CoordinatedMeeting
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct CoordinatedMeeting : IComponentData, IQueryTypeParameter, ISerializable
  {
    public MeetingStatus m_Status;
    public int m_Phase;
    public Entity m_Target;
    public uint m_PhaseEndTime;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num;
      reader.Read(out num);
      this.m_Status = (MeetingStatus) num;
      if (reader.context.version < Version.timoSerializationFlow)
        reader.Read<TravelPurpose>(out TravelPurpose _);
      reader.Read(out this.m_Target);
      if (reader.context.version < Version.timoSerializationFlow)
      {
        this.m_Phase = 0;
        this.m_PhaseEndTime = 0U;
      }
      else
      {
        reader.Read(out this.m_Phase);
        reader.Read(out this.m_PhaseEndTime);
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((int) this.m_Status);
      writer.Write(this.m_Target);
      writer.Write(this.m_Phase);
      writer.Write(this.m_PhaseEndTime);
    }
  }
}
