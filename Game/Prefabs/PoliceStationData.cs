// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PoliceStationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PoliceStationData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<PoliceStationData>,
    ISerializable
  {
    public int m_PatrolCarCapacity;
    public int m_PoliceHelicopterCapacity;
    public int m_JailCapacity;
    public PolicePurpose m_PurposeMask;

    public void Combine(PoliceStationData otherData)
    {
      this.m_PatrolCarCapacity += otherData.m_PatrolCarCapacity;
      this.m_PoliceHelicopterCapacity += otherData.m_PoliceHelicopterCapacity;
      this.m_JailCapacity += otherData.m_JailCapacity;
      this.m_PurposeMask |= otherData.m_PurposeMask;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PatrolCarCapacity);
      writer.Write(this.m_PoliceHelicopterCapacity);
      writer.Write(this.m_JailCapacity);
      writer.Write((byte) this.m_PurposeMask);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PatrolCarCapacity);
      reader.Read(out this.m_PoliceHelicopterCapacity);
      reader.Read(out this.m_JailCapacity);
      byte num;
      reader.Read(out num);
      this.m_PurposeMask = (PolicePurpose) num;
    }
  }
}
