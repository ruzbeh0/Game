// Decompiled with JetBrains decompiler
// Type: Game.Buildings.WaterPowered
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct WaterPowered : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_Length;
    public float m_Height;
    public float m_Estimate;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Length);
      writer.Write(this.m_Height);
      writer.Write(this.m_Estimate);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Length);
      reader.Read(out this.m_Height);
      reader.Read(out this.m_Estimate);
    }
  }
}
