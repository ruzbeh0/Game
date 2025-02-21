// Decompiled with JetBrains decompiler
// Type: Game.City.StatisticsEvent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;

#nullable disable
namespace Game.City
{
  [FormerlySerializedAs("Game.City.StatisticsEvent2, Game")]
  public struct StatisticsEvent : ISerializable
  {
    public StatisticType m_Statistic;
    public int m_Parameter;
    public float m_Change;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((int) this.m_Statistic);
      writer.Write(this.m_Parameter);
      writer.Write(this.m_Change);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num1;
      reader.Read(out num1);
      this.m_Statistic = (StatisticType) num1;
      reader.Read(out this.m_Parameter);
      if (reader.context.version < Version.statisticPrecisionFix)
      {
        int num2;
        reader.Read(out num2);
        this.m_Change = (float) num2;
      }
      else
        reader.Read(out this.m_Change);
    }
  }
}
