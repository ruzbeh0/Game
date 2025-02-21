// Decompiled with JetBrains decompiler
// Type: Game.Simulation.EvacuationRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct EvacuationRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Target;
    public float m_Priority;

    public EvacuationRequest(Entity target, float priority)
    {
      this.m_Target = target;
      this.m_Priority = priority;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      writer.Write(this.m_Priority);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      reader.Read(out this.m_Priority);
    }
  }
}
