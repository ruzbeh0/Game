// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ModifiedServiceCoverage
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct ModifiedServiceCoverage : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_Range;
    public float m_Capacity;
    public float m_Magnitude;

    public void ReplaceData(ref CoverageData coverage)
    {
      coverage.m_Capacity = this.m_Capacity;
      coverage.m_Range = this.m_Range;
      coverage.m_Magnitude = this.m_Magnitude;
    }

    public ModifiedServiceCoverage(CoverageData coverage)
    {
      this.m_Capacity = coverage.m_Capacity;
      this.m_Range = coverage.m_Range;
      this.m_Magnitude = coverage.m_Magnitude;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Capacity);
      writer.Write(this.m_Range);
      writer.Write(this.m_Magnitude);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      float num;
      reader.Read(out num);
      this.m_Capacity = num;
      reader.Read(out num);
      this.m_Range = num;
      reader.Read(out num);
      this.m_Magnitude = num;
    }
  }
}
