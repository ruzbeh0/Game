// Decompiled with JetBrains decompiler
// Type: Game.City.CityStatistic
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.City
{
  public struct CityStatistic : IBufferElementData, ISerializable
  {
    public double m_Value;
    public double m_TotalValue;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Value);
      writer.Write(this.m_TotalValue);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version < Version.statisticOverflowFix)
      {
        int num1;
        reader.Read(out num1);
        int num2;
        reader.Read(out num2);
        this.m_Value = (double) num1;
        this.m_TotalValue = (double) num2;
      }
      else if (reader.context.version < Version.statisticPrecisionFix)
      {
        long num3;
        reader.Read(out num3);
        long num4;
        reader.Read(out num4);
        this.m_Value = (double) num3;
        this.m_TotalValue = (double) num4;
      }
      else
      {
        reader.Read(out this.m_Value);
        reader.Read(out this.m_TotalValue);
      }
    }
  }
}
