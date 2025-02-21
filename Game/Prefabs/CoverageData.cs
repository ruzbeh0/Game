// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CoverageData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct CoverageData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public CoverageService m_Service;
    public float m_Range;
    public float m_Capacity;
    public float m_Magnitude;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Range);
      writer.Write(this.m_Capacity);
      writer.Write(this.m_Magnitude);
      writer.Write((byte) this.m_Service);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Range);
      reader.Read(out this.m_Capacity);
      reader.Read(out this.m_Magnitude);
      byte num;
      reader.Read(out num);
      this.m_Service = (CoverageService) num;
    }
  }
}
