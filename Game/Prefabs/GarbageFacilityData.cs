// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GarbageFacilityData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct GarbageFacilityData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<GarbageFacilityData>,
    ISerializable
  {
    public int m_GarbageCapacity;
    public int m_VehicleCapacity;
    public int m_TransportCapacity;
    public int m_ProcessingSpeed;
    public bool m_IndustrialWasteOnly;
    public bool m_LongTermStorage;

    public void Combine(GarbageFacilityData otherData)
    {
      this.m_GarbageCapacity += otherData.m_GarbageCapacity;
      this.m_VehicleCapacity += otherData.m_VehicleCapacity;
      this.m_TransportCapacity += otherData.m_TransportCapacity;
      this.m_ProcessingSpeed += otherData.m_ProcessingSpeed;
      this.m_IndustrialWasteOnly |= otherData.m_IndustrialWasteOnly;
      this.m_LongTermStorage |= otherData.m_LongTermStorage;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_GarbageCapacity);
      writer.Write(this.m_VehicleCapacity);
      writer.Write(this.m_TransportCapacity);
      writer.Write(this.m_ProcessingSpeed);
      writer.Write(this.m_IndustrialWasteOnly);
      writer.Write(this.m_LongTermStorage);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_GarbageCapacity);
      reader.Read(out this.m_VehicleCapacity);
      reader.Read(out this.m_TransportCapacity);
      reader.Read(out this.m_ProcessingSpeed);
      reader.Read(out this.m_IndustrialWasteOnly);
      reader.Read(out this.m_LongTermStorage);
    }
  }
}
