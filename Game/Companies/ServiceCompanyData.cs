// Decompiled with JetBrains decompiler
// Type: Game.Companies.ServiceCompanyData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Companies
{
  public struct ServiceCompanyData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_MaxService;
    public int m_WorkPerUnit;
    public float m_MaxWorkersPerCell;
    public int m_ServiceConsuming;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_MaxService);
      writer.Write(this.m_WorkPerUnit);
      writer.Write(this.m_MaxWorkersPerCell);
      writer.Write(this.m_ServiceConsuming);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_MaxService);
      reader.Read(out this.m_WorkPerUnit);
      reader.Read(out this.m_MaxWorkersPerCell);
      if (!(reader.context.version >= Version.serviceCompanyConsuming))
        return;
      reader.Read(out this.m_ServiceConsuming);
    }
  }
}
