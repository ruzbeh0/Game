// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CollectedCityServiceBudgetData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct CollectedCityServiceBudgetData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int3 m_Workplaces;
    public int m_Count;
    public int m_Export;
    public int m_BaseCost;
    public int m_Wages;
    public int m_FullWages;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Workplaces);
      reader.Read(out this.m_Count);
      reader.Read(out this.m_Wages);
      reader.Read(out this.m_FullWages);
      reader.Read(out this.m_Export);
      if (!(reader.context.version >= Version.netUpkeepCost))
        return;
      reader.Read(out this.m_BaseCost);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Workplaces);
      writer.Write(this.m_Count);
      writer.Write(this.m_Wages);
      writer.Write(this.m_FullWages);
      writer.Write(this.m_Export);
      writer.Write(this.m_BaseCost);
    }
  }
}
