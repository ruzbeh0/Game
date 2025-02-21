// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityFlowEdge
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ElectricityFlowEdge : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Index;
    public Entity m_Start;
    public Entity m_End;
    public int m_Capacity;
    public int m_Flow;
    public ElectricityFlowEdgeFlags m_Flags;

    public FlowDirection direction
    {
      get => (FlowDirection) (this.m_Flags & ElectricityFlowEdgeFlags.ForwardBackward);
      set
      {
        this.m_Flags &= ~ElectricityFlowEdgeFlags.ForwardBackward;
        this.m_Flags |= (ElectricityFlowEdgeFlags) value;
      }
    }

    public bool isBottleneck => (this.m_Flags & ElectricityFlowEdgeFlags.Bottleneck) != 0;

    public bool isBeyondBottleneck
    {
      get => (this.m_Flags & ElectricityFlowEdgeFlags.BeyondBottleneck) != 0;
    }

    public bool isDisconnected => (this.m_Flags & ElectricityFlowEdgeFlags.Disconnected) != 0;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Start);
      writer.Write(this.m_End);
      writer.Write(this.m_Flow);
      writer.Write(this.m_Capacity);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Start);
      reader.Read(out this.m_End);
      reader.Read(out this.m_Flow);
      reader.Read(out this.m_Capacity);
      if (reader.context.version > Version.electricityImprovements2)
      {
        byte num;
        reader.Read(out num);
        this.m_Flags = (ElectricityFlowEdgeFlags) num;
      }
      else if (reader.context.version >= Version.electricityImprovements)
      {
        reader.Read(out int _);
        this.m_Flags = ElectricityFlowEdgeFlags.ForwardBackward;
      }
      else
        this.m_Flags = ElectricityFlowEdgeFlags.ForwardBackward;
    }
  }
}
