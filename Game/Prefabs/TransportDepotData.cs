// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportDepotData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Vehicles;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TransportDepotData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<TransportDepotData>,
    ISerializable
  {
    public TransportType m_TransportType;
    public EnergyTypes m_EnergyTypes;
    public bool m_DispatchCenter;
    public int m_VehicleCapacity;
    public float m_ProductionDuration;
    public float m_MaintenanceDuration;

    public void Combine(TransportDepotData otherData)
    {
      this.m_EnergyTypes |= otherData.m_EnergyTypes;
      this.m_DispatchCenter |= otherData.m_DispatchCenter;
      this.m_VehicleCapacity += otherData.m_VehicleCapacity;
      this.m_ProductionDuration += otherData.m_ProductionDuration;
      this.m_MaintenanceDuration += otherData.m_MaintenanceDuration;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_DispatchCenter);
      writer.Write(this.m_VehicleCapacity);
      writer.Write(this.m_ProductionDuration);
      writer.Write(this.m_MaintenanceDuration);
      writer.Write((int) this.m_TransportType);
      writer.Write((byte) this.m_EnergyTypes);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_DispatchCenter);
      reader.Read(out this.m_VehicleCapacity);
      reader.Read(out this.m_ProductionDuration);
      reader.Read(out this.m_MaintenanceDuration);
      int num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      this.m_TransportType = (TransportType) num1;
      this.m_EnergyTypes = (EnergyTypes) num2;
    }
  }
}
