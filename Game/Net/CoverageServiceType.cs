// Decompiled with JetBrains decompiler
// Type: Game.Net.CoverageServiceType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct CoverageServiceType : ISharedComponentData, IQueryTypeParameter, ISerializable
  {
    public CoverageService m_Service;

    public CoverageServiceType(CoverageService service) => this.m_Service = service;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_Service);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      this.m_Service = (CoverageService) num;
    }
  }
}
