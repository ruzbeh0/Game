// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.MaintenanceVehicleData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Simulation;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct MaintenanceVehicleData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public MaintenanceType m_MaintenanceType;
    public int m_MaintenanceCapacity;
    public int m_MaintenanceRate;

    public MaintenanceVehicleData(
      MaintenanceType maintenanceType,
      int maintenanceCapacity,
      int maintenanceRate)
    {
      this.m_MaintenanceType = maintenanceType;
      this.m_MaintenanceCapacity = maintenanceCapacity;
      this.m_MaintenanceRate = maintenanceRate;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_MaintenanceType);
      writer.Write(this.m_MaintenanceCapacity);
      writer.Write(this.m_MaintenanceRate);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_MaintenanceCapacity);
      reader.Read(out this.m_MaintenanceRate);
      this.m_MaintenanceType = (MaintenanceType) num;
    }
  }
}
