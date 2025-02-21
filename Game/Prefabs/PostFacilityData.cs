// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PostFacilityData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PostFacilityData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<PostFacilityData>,
    ISerializable
  {
    public int m_PostVanCapacity;
    public int m_PostTruckCapacity;
    public int m_MailCapacity;
    public int m_SortingRate;

    public void Combine(PostFacilityData otherData)
    {
      this.m_PostVanCapacity += otherData.m_PostVanCapacity;
      this.m_PostTruckCapacity += otherData.m_PostTruckCapacity;
      this.m_MailCapacity += otherData.m_MailCapacity;
      this.m_SortingRate += otherData.m_SortingRate;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PostVanCapacity);
      writer.Write(this.m_PostTruckCapacity);
      writer.Write(this.m_MailCapacity);
      writer.Write(this.m_SortingRate);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PostVanCapacity);
      reader.Read(out this.m_PostTruckCapacity);
      reader.Read(out this.m_MailCapacity);
      reader.Read(out this.m_SortingRate);
    }
  }
}
