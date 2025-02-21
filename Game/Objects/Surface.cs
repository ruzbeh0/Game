// Decompiled with JetBrains decompiler
// Type: Game.Objects.Surface
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  public struct Surface : IComponentData, IQueryTypeParameter, ISerializable
  {
    public byte m_Wetness;
    public byte m_SnowAmount;
    public byte m_AccumulatedWetness;
    public byte m_AccumulatedSnow;
    public byte m_Dirtyness;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Wetness);
      writer.Write(this.m_SnowAmount);
      writer.Write(this.m_AccumulatedWetness);
      writer.Write(this.m_AccumulatedSnow);
      writer.Write(this.m_Dirtyness);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Wetness);
      reader.Read(out this.m_SnowAmount);
      reader.Read(out this.m_AccumulatedWetness);
      reader.Read(out this.m_AccumulatedSnow);
      reader.Read(out this.m_Dirtyness);
    }
  }
}
