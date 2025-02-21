// Decompiled with JetBrains decompiler
// Type: Game.City.Population
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.City
{
  public struct Population : IComponentData, IQueryTypeParameter, IDefaultSerializable, ISerializable
  {
    public int m_Population;
    public int m_PopulationWithMoveIn;
    public int m_AverageHappiness;
    public int m_AverageHealth;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Population);
      reader.Read(out this.m_PopulationWithMoveIn);
      reader.Read(out this.m_AverageHappiness);
      if (reader.context.version >= Version.averageHealth)
        reader.Read(out this.m_AverageHealth);
      else
        this.m_AverageHealth = 50;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Population);
      writer.Write(this.m_PopulationWithMoveIn);
      writer.Write(this.m_AverageHappiness);
      writer.Write(this.m_AverageHealth);
    }

    public void SetDefaults(Context context)
    {
      this.m_Population = 0;
      this.m_PopulationWithMoveIn = 0;
      this.m_AverageHappiness = 50;
      this.m_AverageHealth = 50;
    }
  }
}
