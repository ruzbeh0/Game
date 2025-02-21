// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AttractionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct AttractionData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<AttractionData>,
    ISerializable
  {
    public int m_Attractiveness;

    public void Combine(AttractionData otherData)
    {
      this.m_Attractiveness += otherData.m_Attractiveness;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Attractiveness);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Attractiveness);
    }
  }
}
