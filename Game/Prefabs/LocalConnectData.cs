// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LocalConnectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct LocalConnectData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public LocalConnectFlags m_Flags;
    public Layer m_Layers;
    public Bounds1 m_HeightRange;
    public float m_SearchDistance;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_Flags);
      writer.Write((uint) this.m_Layers);
      writer.Write(this.m_HeightRange.min);
      writer.Write(this.m_HeightRange.max);
      writer.Write(this.m_SearchDistance);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num1;
      reader.Read(out num1);
      uint num2;
      reader.Read(out num2);
      reader.Read(out this.m_HeightRange.min);
      reader.Read(out this.m_HeightRange.max);
      reader.Read(out this.m_SearchDistance);
      this.m_Flags = (LocalConnectFlags) num1;
      this.m_Layers = (Layer) num2;
    }
  }
}
