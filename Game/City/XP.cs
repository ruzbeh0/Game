// Decompiled with JetBrains decompiler
// Type: Game.City.XP
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.City
{
  public struct XP : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_XP;
    public int m_MaximumPopulation;
    public int m_MaximumIncome;
    public XPRewardFlags m_XPRewardRecord;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_XP);
      writer.Write(this.m_MaximumPopulation);
      writer.Write(this.m_MaximumIncome);
      writer.Write((byte) this.m_XPRewardRecord);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_XP);
      if (reader.context.version >= Version.xpMaximumStats)
      {
        reader.Read(out this.m_MaximumPopulation);
        reader.Read(out this.m_MaximumIncome);
      }
      if (!(reader.context.version >= Version.xpRewardRecord))
        return;
      byte num;
      reader.Read(out num);
      this.m_XPRewardRecord = (XPRewardFlags) num;
    }
  }
}
