// Decompiled with JetBrains decompiler
// Type: Game.Net.NodeLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct NodeLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float2 m_WidthOffset;
    public byte m_SharedStartCount;
    public byte m_SharedEndCount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      if (math.all(this.m_WidthOffset != 0.0f))
      {
        writer.Write((byte) 3);
        writer.Write(this.m_WidthOffset.x);
        writer.Write(this.m_WidthOffset.y);
      }
      else if ((double) this.m_WidthOffset.x != 0.0)
      {
        writer.Write((byte) 1);
        writer.Write(this.m_WidthOffset.x);
      }
      else if ((double) this.m_WidthOffset.y != 0.0)
      {
        writer.Write((byte) 2);
        writer.Write(this.m_WidthOffset.y);
      }
      else
        writer.Write((byte) 0);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.saveOptimizations)
      {
        byte num;
        reader.Read(out num);
        if (((int) num & 1) != 0)
          reader.Read(out this.m_WidthOffset.x);
        if (((int) num & 2) == 0)
          return;
        reader.Read(out this.m_WidthOffset.y);
      }
      else
        reader.Read(out this.m_WidthOffset);
    }
  }
}
