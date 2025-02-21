// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PowerPlantData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PowerPlantData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<PowerPlantData>,
    ISerializable
  {
    public int m_ElectricityProduction;

    public void Combine(PowerPlantData otherData)
    {
      this.m_ElectricityProduction += otherData.m_ElectricityProduction;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ElectricityProduction);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ElectricityProduction);
    }
  }
}
