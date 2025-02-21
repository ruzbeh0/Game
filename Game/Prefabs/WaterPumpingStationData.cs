// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WaterPumpingStationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct WaterPumpingStationData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<WaterPumpingStationData>,
    ISerializable
  {
    public AllowedWaterTypes m_Types;
    public int m_Capacity;
    public float m_Purification;

    public void Combine(WaterPumpingStationData otherData)
    {
      this.m_Types |= otherData.m_Types;
      this.m_Capacity += otherData.m_Capacity;
      this.m_Purification = (float) (1.0 - (1.0 - (double) this.m_Purification) * (1.0 - (double) otherData.m_Purification));
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Capacity);
      writer.Write(this.m_Purification);
      writer.Write((ushort) this.m_Types);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Capacity);
      reader.Read(out this.m_Purification);
      ushort num;
      reader.Read(out num);
      this.m_Types = (AllowedWaterTypes) num;
    }
  }
}
