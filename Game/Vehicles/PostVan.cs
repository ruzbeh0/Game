// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.PostVan
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct PostVan : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public PostVanFlags m_State;
    public int m_RequestCount;
    public float m_PathElementTime;
    public int m_DeliveringMail;
    public int m_CollectedMail;
    public int m_DeliveryEstimate;
    public int m_CollectEstimate;

    public PostVan(PostVanFlags flags, int requestCount, int deliveringMail)
    {
      this.m_TargetRequest = Entity.Null;
      this.m_State = flags;
      this.m_RequestCount = requestCount;
      this.m_PathElementTime = 0.0f;
      this.m_DeliveringMail = deliveringMail;
      this.m_CollectedMail = 0;
      this.m_DeliveryEstimate = 0;
      this.m_CollectEstimate = 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((uint) this.m_State);
      writer.Write(this.m_RequestCount);
      writer.Write(this.m_PathElementTime);
      writer.Write(this.m_DeliveringMail);
      writer.Write(this.m_CollectedMail);
      writer.Write(this.m_DeliveryEstimate);
      writer.Write(this.m_CollectEstimate);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_RequestCount);
      reader.Read(out this.m_PathElementTime);
      if (reader.context.version < Version.taxiDispatchCenter)
        reader.Read(out int _);
      reader.Read(out this.m_DeliveringMail);
      reader.Read(out this.m_CollectedMail);
      this.m_State = (PostVanFlags) num;
      if (!(reader.context.version >= Version.policeShiftEstimate))
        return;
      reader.Read(out this.m_DeliveryEstimate);
      reader.Read(out this.m_CollectEstimate);
    }
  }
}
