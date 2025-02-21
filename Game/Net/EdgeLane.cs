// Decompiled with JetBrains decompiler
// Type: Game.Net.EdgeLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct EdgeLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float2 m_EdgeDelta;
    public byte m_ConnectedStartCount;
    public byte m_ConnectedEndCount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      bool3 bool3_1 = this.m_EdgeDelta.x == new float3(0.0f, 0.5f, 1f);
      bool3 bool3_2 = this.m_EdgeDelta.y == new float3(0.0f, 0.5f, 1f);
      byte num = (byte) math.csum(math.select((int3) 0, new int3(1, 2, 3), bool3_1.xyx & bool3_2.yzz) + math.select((int3) 0, new int3(4, 5, 6), bool3_1.zyz & bool3_2.yxx));
      writer.Write(num);
      if (num != (byte) 0)
        return;
      writer.Write(this.m_EdgeDelta);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.saveOptimizations)
      {
        byte num;
        reader.Read(out num);
        switch (num)
        {
          case 1:
            this.m_EdgeDelta = new float2(0.0f, 0.5f);
            break;
          case 2:
            this.m_EdgeDelta = new float2(0.5f, 1f);
            break;
          case 3:
            this.m_EdgeDelta = new float2(0.0f, 1f);
            break;
          case 4:
            this.m_EdgeDelta = new float2(1f, 0.5f);
            break;
          case 5:
            this.m_EdgeDelta = new float2(0.5f, 0.0f);
            break;
          case 6:
            this.m_EdgeDelta = new float2(1f, 0.0f);
            break;
          default:
            reader.Read(out this.m_EdgeDelta);
            break;
        }
      }
      else
        reader.Read(out this.m_EdgeDelta);
    }
  }
}
