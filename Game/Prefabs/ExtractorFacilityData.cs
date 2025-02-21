// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ExtractorFacilityData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ExtractorFacilityData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Bounds1 m_RotationRange;
    public Bounds1 m_HeightOffset;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_RotationRange.min);
      writer.Write(this.m_RotationRange.max);
      writer.Write(this.m_HeightOffset.min);
      writer.Write(this.m_HeightOffset.max);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_RotationRange.min);
      reader.Read(out this.m_RotationRange.max);
      reader.Read(out this.m_HeightOffset.min);
      reader.Read(out this.m_HeightOffset.max);
    }
  }
}
