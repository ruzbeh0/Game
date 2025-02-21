// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TransportStationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Vehicles;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TransportStationData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<TransportStationData>,
    ISerializable
  {
    public float m_ComfortFactor;
    public float m_LoadingFactor;
    public EnergyTypes m_CarRefuelTypes;
    public EnergyTypes m_TrainRefuelTypes;
    public EnergyTypes m_WatercraftRefuelTypes;
    public EnergyTypes m_AircraftRefuelTypes;

    public void Combine(TransportStationData otherData)
    {
      this.m_ComfortFactor += otherData.m_ComfortFactor;
      this.m_LoadingFactor += otherData.m_LoadingFactor;
      this.m_CarRefuelTypes |= otherData.m_CarRefuelTypes;
      this.m_TrainRefuelTypes |= otherData.m_TrainRefuelTypes;
      this.m_WatercraftRefuelTypes |= otherData.m_WatercraftRefuelTypes;
      this.m_AircraftRefuelTypes |= otherData.m_AircraftRefuelTypes;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ComfortFactor);
      writer.Write(this.m_LoadingFactor);
      writer.Write((byte) this.m_CarRefuelTypes);
      writer.Write((byte) this.m_TrainRefuelTypes);
      writer.Write((byte) this.m_WatercraftRefuelTypes);
      writer.Write((byte) this.m_AircraftRefuelTypes);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ComfortFactor);
      reader.Read(out this.m_LoadingFactor);
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      byte num3;
      reader.Read(out num3);
      byte num4;
      reader.Read(out num4);
      this.m_CarRefuelTypes = (EnergyTypes) num1;
      this.m_TrainRefuelTypes = (EnergyTypes) num2;
      this.m_WatercraftRefuelTypes = (EnergyTypes) num3;
      this.m_AircraftRefuelTypes = (EnergyTypes) num4;
    }
  }
}
