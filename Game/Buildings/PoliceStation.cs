// Decompiled with JetBrains decompiler
// Type: Game.Buildings.PoliceStation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct PoliceStation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_PrisonerTransportRequest;
    public Entity m_TargetRequest;
    public PoliceStationFlags m_Flags;
    public PolicePurpose m_PurposeMask;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PrisonerTransportRequest);
      writer.Write(this.m_TargetRequest);
      writer.Write((byte) this.m_Flags);
      writer.Write((int) this.m_PurposeMask);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.policeImprovement2)
        reader.Read(out this.m_PrisonerTransportRequest);
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      byte num1;
      reader.Read(out num1);
      this.m_Flags = (PoliceStationFlags) num1;
      if (reader.context.version >= Version.policeImprovement3)
      {
        int num2;
        reader.Read(out num2);
        this.m_PurposeMask = (PolicePurpose) num2;
      }
      else
        this.m_PurposeMask = PolicePurpose.Patrol | PolicePurpose.Emergency;
    }
  }
}
