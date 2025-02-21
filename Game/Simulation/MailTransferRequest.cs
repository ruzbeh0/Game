// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MailTransferRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct MailTransferRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Facility;
    public MailTransferRequestFlags m_Flags;
    public float m_Priority;
    public int m_Amount;

    public MailTransferRequest(
      Entity facility,
      MailTransferRequestFlags flags,
      float priority,
      int amount)
    {
      this.m_Facility = facility;
      this.m_Flags = flags;
      this.m_Priority = priority;
      this.m_Amount = amount;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Facility);
      writer.Write((ushort) this.m_Flags);
      writer.Write(this.m_Priority);
      writer.Write(this.m_Amount);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Facility);
      ushort num;
      reader.Read(out num);
      reader.Read(out this.m_Priority);
      reader.Read(out this.m_Amount);
      this.m_Flags = (MailTransferRequestFlags) num;
    }
  }
}
