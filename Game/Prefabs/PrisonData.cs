// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrisonData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PrisonData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<PrisonData>,
    ISerializable
  {
    public int m_PrisonVanCapacity;
    public int m_PrisonerCapacity;
    public sbyte m_PrisonerWellbeing;
    public sbyte m_PrisonerHealth;

    public void Combine(PrisonData otherData)
    {
      this.m_PrisonVanCapacity += otherData.m_PrisonVanCapacity;
      this.m_PrisonerCapacity += otherData.m_PrisonerCapacity;
      this.m_PrisonerWellbeing += otherData.m_PrisonerWellbeing;
      this.m_PrisonerHealth += otherData.m_PrisonerHealth;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PrisonVanCapacity);
      writer.Write(this.m_PrisonerCapacity);
      writer.Write(this.m_PrisonerWellbeing);
      writer.Write(this.m_PrisonerHealth);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PrisonVanCapacity);
      reader.Read(out this.m_PrisonerCapacity);
      if (!(reader.context.version >= Version.happinessAdjustRefactoring))
        return;
      reader.Read(out this.m_PrisonerWellbeing);
      reader.Read(out this.m_PrisonerHealth);
    }
  }
}
