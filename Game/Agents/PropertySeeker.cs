// Decompiled with JetBrains decompiler
// Type: Game.Agents.PropertySeeker
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Agents
{
  public struct PropertySeeker : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetProperty;
    public Entity m_BestProperty;
    public float m_BestPropertyScore;
    public byte m_PropertiesEvaluated;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetProperty);
      writer.Write(this.m_BestProperty);
      writer.Write(this.m_BestPropertyScore);
      writer.Write(this.m_PropertiesEvaluated);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TargetProperty);
      reader.Read(out this.m_BestProperty);
      reader.Read(out this.m_BestPropertyScore);
      reader.Read(out this.m_PropertiesEvaluated);
    }
  }
}
