// Decompiled with JetBrains decompiler
// Type: Game.Buildings.PostFacility
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct PostFacility : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_MailDeliverRequest;
    public Entity m_MailReceiveRequest;
    public Entity m_TargetRequest;
    public float m_AcceptMailPriority;
    public float m_DeliverMailPriority;
    public PostFacilityFlags m_Flags;
    public byte m_ProcessingFactor;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_MailDeliverRequest);
      writer.Write(this.m_MailReceiveRequest);
      writer.Write(this.m_AcceptMailPriority);
      writer.Write(this.m_DeliverMailPriority);
      writer.Write(this.m_TargetRequest);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_ProcessingFactor);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.transferRequestRefactoring)
      {
        reader.Read(out this.m_MailDeliverRequest);
        reader.Read(out this.m_MailReceiveRequest);
        reader.Read(out this.m_AcceptMailPriority);
        reader.Read(out this.m_DeliverMailPriority);
      }
      else
        reader.Read(out Entity _);
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      byte num;
      reader.Read(out num);
      if (reader.context.version >= Version.mailProcessing)
        reader.Read(out this.m_ProcessingFactor);
      this.m_Flags = (PostFacilityFlags) num;
    }
  }
}
