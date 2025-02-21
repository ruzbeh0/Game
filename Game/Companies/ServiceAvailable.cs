// Decompiled with JetBrains decompiler
// Type: Game.Companies.ServiceAvailable
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Companies
{
  public struct ServiceAvailable : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_ServiceAvailable;
    public float m_MeanPriority;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ServiceAvailable);
      writer.Write(this.m_MeanPriority);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ServiceAvailable);
      reader.Read(out this.m_MeanPriority);
    }
  }
}
