// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GroundWaterPoweredData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct GroundWaterPoweredData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<GroundWaterPoweredData>,
    ISerializable
  {
    public int m_Production;
    public int m_MaximumGroundWater;

    public void Combine(GroundWaterPoweredData otherData)
    {
      this.m_Production += otherData.m_Production;
      this.m_MaximumGroundWater += otherData.m_MaximumGroundWater;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Production);
      writer.Write(this.m_MaximumGroundWater);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Production);
      reader.Read(out this.m_MaximumGroundWater);
    }
  }
}
