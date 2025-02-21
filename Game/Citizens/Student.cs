// Decompiled with JetBrains decompiler
// Type: Game.Citizens.Student
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct Student : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_School;
    public float m_LastCommuteTime;
    public byte m_Level;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_School);
      writer.Write(this.m_LastCommuteTime);
      writer.Write(this.m_Level);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_School);
      reader.Read(out this.m_LastCommuteTime);
      if (reader.context.version >= Version.educationTrading)
        reader.Read(out this.m_Level);
      else
        this.m_Level = byte.MaxValue;
    }
  }
}
