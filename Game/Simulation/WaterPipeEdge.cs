// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPipeEdge
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct WaterPipeEdge : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Index;
    public Entity m_Start;
    public Entity m_End;
    public int m_FreshFlow;
    public float m_FreshPollution;
    public int m_SewageFlow;
    public int m_FreshCapacity;
    public int m_SewageCapacity;
    public WaterPipeEdgeFlags m_Flags;

    public int2 flow => new int2(this.m_FreshFlow, this.m_SewageFlow);

    public int2 capacity => new int2(this.m_FreshCapacity, this.m_SewageCapacity);

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Start);
      writer.Write(this.m_End);
      writer.Write(this.m_FreshFlow);
      writer.Write(this.m_FreshPollution);
      writer.Write(this.m_SewageFlow);
      writer.Write(this.m_FreshCapacity);
      writer.Write(this.m_SewageCapacity);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Start);
      reader.Read(out this.m_End);
      reader.Read(out this.m_FreshFlow);
      if (reader.context.version >= Version.waterPipePollution)
        reader.Read(out this.m_FreshPollution);
      else
        reader.Read(out int _);
      reader.Read(out this.m_SewageFlow);
      reader.Read(out this.m_FreshCapacity);
      reader.Read(out this.m_SewageCapacity);
      Context context;
      if (reader.context.version >= Version.stormWater)
      {
        context = reader.context;
        if (context.version < Version.waterPipeFlowSim)
        {
          reader.Read(out int _);
          reader.Read(out int _);
        }
      }
      context = reader.context;
      if (!(context.version >= Version.waterPipeFlags))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (WaterPipeEdgeFlags) num;
    }
  }
}
