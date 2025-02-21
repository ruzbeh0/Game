// Decompiled with JetBrains decompiler
// Type: Game.Buildings.School
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct School : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_AverageGraduationTime;
    public float m_AverageFailProbability;
    public sbyte m_StudentWellbeing;
    public sbyte m_StudentHealth;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AverageGraduationTime);
      writer.Write(this.m_AverageFailProbability);
      writer.Write(this.m_StudentWellbeing);
      writer.Write(this.m_StudentHealth);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_AverageGraduationTime);
      reader.Read(out this.m_AverageFailProbability);
      if (!(reader.context.version >= Version.happinessAdjustRefactoring))
        return;
      reader.Read(out this.m_StudentWellbeing);
      reader.Read(out this.m_StudentHealth);
    }
  }
}
