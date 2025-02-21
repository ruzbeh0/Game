// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.FireEngineData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct FireEngineData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_ExtinguishingRate;
    public float m_ExtinguishingSpread;
    public float m_ExtinguishingCapacity;
    public float m_DestroyedClearDuration;

    public FireEngineData(
      float extinguishingRate,
      float extinguishingSpread,
      float extinguishingCapacity,
      float destroyedClearDuration)
    {
      this.m_ExtinguishingRate = extinguishingRate;
      this.m_ExtinguishingSpread = extinguishingSpread;
      this.m_ExtinguishingCapacity = extinguishingCapacity;
      this.m_DestroyedClearDuration = destroyedClearDuration;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ExtinguishingRate);
      writer.Write(this.m_ExtinguishingSpread);
      writer.Write(this.m_ExtinguishingCapacity);
      writer.Write(this.m_DestroyedClearDuration);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ExtinguishingRate);
      reader.Read(out this.m_ExtinguishingSpread);
      reader.Read(out this.m_ExtinguishingCapacity);
      reader.Read(out this.m_DestroyedClearDuration);
    }
  }
}
