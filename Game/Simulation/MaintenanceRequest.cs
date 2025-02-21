// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MaintenanceRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct MaintenanceRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Target;
    public int m_Priority;
    public byte m_DispatchIndex;

    public MaintenanceRequest(Entity target, int priority)
    {
      this.m_Target = target;
      this.m_Priority = priority;
      this.m_DispatchIndex = (byte) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      writer.Write(this.m_Priority);
      writer.Write(this.m_DispatchIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      reader.Read(out this.m_Priority);
      if (!(reader.context.version >= Version.requestDispatchIndex))
        return;
      reader.Read(out this.m_DispatchIndex);
    }
  }
}
