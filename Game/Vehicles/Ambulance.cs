// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.Ambulance
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct Ambulance : IComponentData, IQueryTypeParameter, ISerializable
  {
    public AmbulanceFlags m_State;
    public Entity m_TargetPatient;
    public Entity m_TargetLocation;
    public Entity m_TargetRequest;
    public float m_PathElementTime;

    public Ambulance(Entity targetPatient, Entity targetLocation, AmbulanceFlags state)
    {
      this.m_State = state;
      this.m_TargetPatient = targetPatient;
      this.m_TargetLocation = targetLocation;
      this.m_TargetRequest = Entity.Null;
      this.m_PathElementTime = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_State);
      writer.Write(this.m_TargetPatient);
      writer.Write(this.m_TargetLocation);
      writer.Write(this.m_TargetRequest);
      writer.Write(this.m_PathElementTime);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_TargetPatient);
      if (reader.context.version >= Version.healthcareImprovement2)
        reader.Read(out this.m_TargetLocation);
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      reader.Read(out this.m_PathElementTime);
      this.m_State = (AmbulanceFlags) num;
    }
  }
}
