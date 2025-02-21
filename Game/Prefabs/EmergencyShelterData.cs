// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EmergencyShelterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct EmergencyShelterData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<EmergencyShelterData>,
    ISerializable
  {
    public int m_ShelterCapacity;
    public int m_VehicleCapacity;

    public void Combine(EmergencyShelterData otherData)
    {
      this.m_ShelterCapacity += otherData.m_ShelterCapacity;
      this.m_VehicleCapacity += otherData.m_VehicleCapacity;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ShelterCapacity);
      writer.Write(this.m_VehicleCapacity);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ShelterCapacity);
      reader.Read(out this.m_VehicleCapacity);
    }
  }
}
