// Decompiled with JetBrains decompiler
// Type: Game.Net.Road
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct Road : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float4 m_TrafficFlowDuration0;
    public float4 m_TrafficFlowDuration1;
    public float4 m_TrafficFlowDistance0;
    public float4 m_TrafficFlowDistance1;
    public RoadFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TrafficFlowDuration0);
      writer.Write(this.m_TrafficFlowDuration1);
      writer.Write(this.m_TrafficFlowDistance0);
      writer.Write(this.m_TrafficFlowDistance1);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.netInfoviewImprovements)
      {
        reader.Read(out this.m_TrafficFlowDuration0);
        reader.Read(out this.m_TrafficFlowDuration1);
        reader.Read(out this.m_TrafficFlowDistance0);
        reader.Read(out this.m_TrafficFlowDistance1);
      }
      else
      {
        reader.Read(out this.m_TrafficFlowDuration0);
        reader.Read(out this.m_TrafficFlowDistance0);
        if (reader.context.version < Version.trafficFlowFixes)
        {
          float4 float4 = this.m_TrafficFlowDuration0 + 1f;
          this.m_TrafficFlowDistance0 *= 0.01f;
          this.m_TrafficFlowDuration0 = math.select(this.m_TrafficFlowDistance0 / float4, (float4) 0.0f, float4 <= 0.0f);
        }
        this.m_TrafficFlowDuration0 *= 0.5f;
        this.m_TrafficFlowDistance0 *= 0.5f;
        this.m_TrafficFlowDuration1 = this.m_TrafficFlowDuration0;
        this.m_TrafficFlowDistance1 = this.m_TrafficFlowDistance0;
      }
      if (!(reader.context.version >= Version.roadFlags))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (RoadFlags) num;
    }
  }
}
