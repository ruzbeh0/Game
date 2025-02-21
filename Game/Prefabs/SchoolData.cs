// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SchoolData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct SchoolData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<SchoolData>,
    ISerializable
  {
    public int m_StudentCapacity;
    public float m_GraduationModifier;
    public byte m_EducationLevel;
    public sbyte m_StudentWellbeing;
    public sbyte m_StudentHealth;

    public void Combine(SchoolData otherData)
    {
      this.m_StudentCapacity += otherData.m_StudentCapacity;
      this.m_EducationLevel = (byte) math.max((int) this.m_EducationLevel, (int) otherData.m_EducationLevel);
      this.m_GraduationModifier += otherData.m_GraduationModifier;
      this.m_StudentWellbeing += otherData.m_StudentWellbeing;
      this.m_StudentHealth += otherData.m_StudentHealth;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StudentCapacity);
      writer.Write(this.m_GraduationModifier);
      writer.Write(this.m_EducationLevel);
      writer.Write(this.m_StudentWellbeing);
      writer.Write(this.m_StudentHealth);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StudentCapacity);
      reader.Read(out this.m_GraduationModifier);
      reader.Read(out this.m_EducationLevel);
      if (!(reader.context.version >= Version.happinessAdjustRefactoring))
        return;
      reader.Read(out this.m_StudentWellbeing);
      reader.Read(out this.m_StudentHealth);
    }
  }
}
