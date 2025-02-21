// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TelecomFacilityData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TelecomFacilityData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<TelecomFacilityData>,
    ISerializable
  {
    public float m_Range;
    public float m_NetworkCapacity;
    public bool m_PenetrateTerrain;

    public void Combine(TelecomFacilityData otherData)
    {
      this.m_Range += otherData.m_Range;
      this.m_NetworkCapacity += otherData.m_NetworkCapacity;
      this.m_PenetrateTerrain |= otherData.m_PenetrateTerrain;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Range);
      writer.Write(this.m_NetworkCapacity);
      writer.Write(this.m_PenetrateTerrain);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Range);
      reader.Read(out this.m_NetworkCapacity);
      reader.Read(out this.m_PenetrateTerrain);
    }
  }
}
