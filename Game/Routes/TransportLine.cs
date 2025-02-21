// Decompiled with JetBrains decompiler
// Type: Game.Routes.TransportLine
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct TransportLine : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_VehicleRequest;
    public float m_VehicleInterval;
    public float m_UnbunchingFactor;
    public TransportLineFlags m_Flags;
    public ushort m_TicketPrice;

    public TransportLine(TransportLineData transportLineData)
    {
      this.m_VehicleRequest = Entity.Null;
      this.m_VehicleInterval = transportLineData.m_DefaultVehicleInterval;
      this.m_UnbunchingFactor = transportLineData.m_DefaultUnbunchingFactor;
      this.m_Flags = (TransportLineFlags) 0;
      this.m_TicketPrice = (ushort) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_VehicleRequest);
      writer.Write(this.m_VehicleInterval);
      writer.Write(this.m_UnbunchingFactor);
      writer.Write((ushort) this.m_Flags);
      writer.Write(this.m_TicketPrice);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_VehicleRequest);
      if (reader.context.version < Version.routeVehicleInterval)
        reader.Read(out float _);
      reader.Read(out this.m_VehicleInterval);
      reader.Read(out this.m_UnbunchingFactor);
      if (reader.context.version >= Version.transportLineFlags)
      {
        ushort num;
        reader.Read(out num);
        this.m_Flags = (TransportLineFlags) num;
      }
      if (!(reader.context.version >= Version.transportLinePolicies))
        return;
      reader.Read(out this.m_TicketPrice);
    }
  }
}
