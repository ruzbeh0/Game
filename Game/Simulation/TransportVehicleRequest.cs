// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransportVehicleRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct TransportVehicleRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Route;
    public float m_Priority;

    public TransportVehicleRequest(Entity route, float priority)
    {
      this.m_Route = route;
      this.m_Priority = priority;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Route);
      writer.Write(this.m_Priority);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Route);
      reader.Read(out this.m_Priority);
    }
  }
}
