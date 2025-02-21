// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CargoTransportVehicleData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct CargoTransportVehicleData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Resource m_Resources;
    public int m_CargoCapacity;
    public int m_MaxResourceCount;
    public float m_MaintenanceRange;

    public CargoTransportVehicleData(
      Resource resources,
      int cargoCapacity,
      int maxResourceCount,
      float maintenanceRange)
    {
      this.m_Resources = resources;
      this.m_CargoCapacity = cargoCapacity;
      this.m_MaxResourceCount = maxResourceCount;
      this.m_MaintenanceRange = maintenanceRange;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((ulong) this.m_Resources);
      writer.Write(this.m_CargoCapacity);
      writer.Write(this.m_MaxResourceCount);
      writer.Write(this.m_MaintenanceRange);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      ulong num;
      reader.Read(out num);
      reader.Read(out this.m_CargoCapacity);
      reader.Read(out this.m_MaxResourceCount);
      reader.Read(out this.m_MaintenanceRange);
      this.m_Resources = (Resource) num;
    }
  }
}
