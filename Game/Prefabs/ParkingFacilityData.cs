// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ParkingFacilityData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ParkingFacilityData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<ParkingFacilityData>,
    ISerializable
  {
    public float m_ComfortFactor;
    public int m_GarageMarkerCapacity;

    public void Combine(ParkingFacilityData otherData)
    {
      this.m_ComfortFactor += otherData.m_ComfortFactor;
      this.m_GarageMarkerCapacity += otherData.m_GarageMarkerCapacity;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ComfortFactor);
      writer.Write(this.m_GarageMarkerCapacity);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ComfortFactor);
      reader.Read(out this.m_GarageMarkerCapacity);
    }
  }
}
