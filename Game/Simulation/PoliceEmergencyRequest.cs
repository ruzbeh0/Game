// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PoliceEmergencyRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct PoliceEmergencyRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Site;
    public Entity m_Target;
    public float m_Priority;
    public PolicePurpose m_Purpose;

    public PoliceEmergencyRequest(
      Entity site,
      Entity target,
      float priority,
      PolicePurpose purpose)
    {
      this.m_Site = site;
      this.m_Target = target;
      this.m_Priority = priority;
      this.m_Purpose = purpose;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Site);
      writer.Write(this.m_Target);
      writer.Write(this.m_Priority);
      writer.Write((int) this.m_Purpose);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Site);
      reader.Read(out this.m_Target);
      reader.Read(out this.m_Priority);
      if (reader.context.version >= Version.policeImprovement3)
      {
        int num;
        reader.Read(out num);
        this.m_Purpose = (PolicePurpose) num;
      }
      else
        this.m_Purpose = PolicePurpose.Patrol | PolicePurpose.Emergency;
    }
  }
}
