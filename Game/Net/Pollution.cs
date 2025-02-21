// Decompiled with JetBrains decompiler
// Type: Game.Net.Pollution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct Pollution : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float2 m_Pollution;
    public float2 m_Accumulation;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Pollution);
      writer.Write(this.m_Accumulation);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Pollution);
      if (reader.context.version >= Version.netPollutionAccumulation)
        reader.Read(out this.m_Accumulation);
      else
        this.m_Accumulation = this.m_Pollution * 2f;
    }
  }
}
