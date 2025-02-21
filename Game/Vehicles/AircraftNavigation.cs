// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.AircraftNavigation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public struct AircraftNavigation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_TargetPosition;
    public float3 m_TargetDirection;
    public float m_MaxSpeed;
    public float m_MinClimbAngle;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetPosition);
      writer.Write(this.m_TargetDirection);
      writer.Write(this.m_MaxSpeed);
      writer.Write(this.m_MinClimbAngle);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TargetPosition);
      reader.Read(out this.m_TargetDirection);
      reader.Read(out this.m_MaxSpeed);
      if (!(reader.context.version >= Version.aircraftNavigation))
        return;
      reader.Read(out this.m_MinClimbAngle);
    }
  }
}
