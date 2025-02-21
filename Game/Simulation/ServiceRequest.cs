// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ServiceRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ServiceRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public byte m_FailCount;
    public byte m_Cooldown;
    public ServiceRequestFlags m_Flags;

    public ServiceRequest(bool reversed)
    {
      this.m_FailCount = (byte) 0;
      this.m_Cooldown = (byte) 0;
      this.m_Flags = reversed ? ServiceRequestFlags.Reversed : (ServiceRequestFlags) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_FailCount);
      writer.Write(this.m_Cooldown);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_FailCount);
      reader.Read(out this.m_Cooldown);
      if (!(reader.context.version >= Version.reverseServiceRequests))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (ServiceRequestFlags) num;
    }
  }
}
