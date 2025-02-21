// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ElectricityBuildingConnection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ElectricityBuildingConnection : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TransformerNode;
    public Entity m_ProducerEdge;
    public Entity m_ConsumerEdge;
    public Entity m_ChargeEdge;
    public Entity m_DischargeEdge;

    public Entity GetProducerNode(ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      return flowEdges[this.m_ProducerEdge].m_End;
    }

    public Entity GetConsumerNode(ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      return flowEdges[this.m_ConsumerEdge].m_Start;
    }

    public Entity GetChargeNode(ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      return flowEdges[this.m_ChargeEdge].m_Start;
    }

    public Entity GetDischargeNode(ref ComponentLookup<ElectricityFlowEdge> flowEdges)
    {
      return flowEdges[this.m_DischargeEdge].m_End;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TransformerNode);
      writer.Write(this.m_ProducerEdge);
      writer.Write(this.m_ConsumerEdge);
      writer.Write(this.m_ChargeEdge);
      writer.Write(this.m_DischargeEdge);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TransformerNode);
      reader.Read(out this.m_ProducerEdge);
      reader.Read(out this.m_ConsumerEdge);
      reader.Read(out this.m_ChargeEdge);
      reader.Read(out this.m_DischargeEdge);
    }
  }
}
