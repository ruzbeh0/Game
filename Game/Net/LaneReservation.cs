// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneReservation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct LaneReservation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Blocker;
    public ReservationData m_Next;
    public ReservationData m_Prev;

    public float GetOffset()
    {
      return (float) math.max((int) this.m_Next.m_Offset, (int) this.m_Prev.m_Offset) * 0.003921569f;
    }

    public int GetPriority()
    {
      return math.max((int) this.m_Next.m_Priority, (int) this.m_Prev.m_Priority);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Blocker);
      writer.Write(this.m_Next.m_Offset);
      writer.Write(this.m_Next.m_Priority);
      writer.Write(this.m_Prev.m_Offset);
      writer.Write(this.m_Prev.m_Priority);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.stuckTrainFix)
        reader.Read(out this.m_Blocker);
      reader.Read(out this.m_Next.m_Offset);
      reader.Read(out this.m_Next.m_Priority);
      reader.Read(out this.m_Prev.m_Offset);
      reader.Read(out this.m_Prev.m_Priority);
    }
  }
}
