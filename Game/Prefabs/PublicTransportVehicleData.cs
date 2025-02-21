// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PublicTransportVehicleData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PublicTransportVehicleData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public TransportType m_TransportType;
    public int m_PassengerCapacity;
    public PublicTransportPurpose m_PurposeMask;
    public float m_MaintenanceRange;

    public PublicTransportVehicleData(
      TransportType type,
      int passengerCapacity,
      PublicTransportPurpose purposeMask,
      float maintenanceRange)
    {
      this.m_TransportType = type;
      this.m_PassengerCapacity = passengerCapacity;
      this.m_PurposeMask = purposeMask;
      this.m_MaintenanceRange = maintenanceRange;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((sbyte) this.m_TransportType);
      writer.Write((uint) this.m_PurposeMask);
      writer.Write(this.m_PassengerCapacity);
      writer.Write(this.m_MaintenanceRange);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      sbyte num1;
      reader.Read(out num1);
      uint num2;
      reader.Read(out num2);
      reader.Read(out this.m_PassengerCapacity);
      reader.Read(out this.m_MaintenanceRange);
      this.m_TransportType = (TransportType) num1;
      this.m_PurposeMask = (PublicTransportPurpose) num2;
    }
  }
}
