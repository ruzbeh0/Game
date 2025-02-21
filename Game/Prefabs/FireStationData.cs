// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FireStationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct FireStationData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<FireStationData>,
    ISerializable
  {
    public int m_FireEngineCapacity;
    public int m_FireHelicopterCapacity;
    public int m_DisasterResponseCapacity;
    public float m_VehicleEfficiency;

    public void Combine(FireStationData otherData)
    {
      this.m_FireEngineCapacity += otherData.m_FireEngineCapacity;
      this.m_FireHelicopterCapacity += otherData.m_FireHelicopterCapacity;
      this.m_DisasterResponseCapacity += otherData.m_DisasterResponseCapacity;
      this.m_VehicleEfficiency += otherData.m_VehicleEfficiency;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_FireEngineCapacity);
      writer.Write(this.m_FireHelicopterCapacity);
      writer.Write(this.m_DisasterResponseCapacity);
      writer.Write(this.m_VehicleEfficiency);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_FireEngineCapacity);
      reader.Read(out this.m_FireHelicopterCapacity);
      reader.Read(out this.m_DisasterResponseCapacity);
      reader.Read(out this.m_VehicleEfficiency);
    }
  }
}
