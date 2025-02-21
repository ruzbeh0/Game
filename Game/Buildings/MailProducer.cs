// Decompiled with JetBrains decompiler
// Type: Game.Buildings.MailProducer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct MailProducer : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_MailRequest;
    public ushort m_SendingMail;
    public ushort m_ReceivingMail;
    public byte m_DispatchIndex;

    public int receivingMail
    {
      get => (int) this.m_ReceivingMail & (int) short.MaxValue;
      set => this.m_ReceivingMail = (ushort) ((int) this.m_ReceivingMail & 32768 | value);
    }

    public bool mailDelivered
    {
      get => ((uint) this.m_ReceivingMail & 32768U) > 0U;
      set
      {
        if (value)
          this.m_ReceivingMail |= (ushort) 32768;
        else
          this.m_ReceivingMail &= (ushort) short.MaxValue;
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_MailRequest);
      writer.Write(this.m_SendingMail);
      writer.Write(this.m_ReceivingMail);
      writer.Write(this.m_DispatchIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_MailRequest);
      reader.Read(out this.m_SendingMail);
      reader.Read(out this.m_ReceivingMail);
      if (!(reader.context.version >= Version.requestDispatchIndex))
        return;
      reader.Read(out this.m_DispatchIndex);
    }
  }
}
