// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TerrainAttractiveness
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct TerrainAttractiveness : IStrideSerializable, ISerializable
  {
    public float m_ShoreBonus;
    public float m_ForestBonus;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ShoreBonus);
      writer.Write(this.m_ForestBonus);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ShoreBonus);
      reader.Read(out this.m_ForestBonus);
    }

    public int GetStride(Context context) => 8;
  }
}
