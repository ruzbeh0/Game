// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.AircraftCurrentLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Pathfind;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public struct AircraftCurrentLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Lane;
    public float3 m_CurvePosition;
    public AircraftLaneFlags m_LaneFlags;
    public float m_Duration;
    public float m_Distance;
    public float m_LanePosition;

    public AircraftCurrentLane(ParkedCar parkedCar, AircraftLaneFlags flags)
    {
      this.m_Lane = parkedCar.m_Lane;
      this.m_CurvePosition = (float3) parkedCar.m_CurvePosition;
      this.m_LaneFlags = flags;
      this.m_Duration = 0.0f;
      this.m_Distance = 0.0f;
      this.m_LanePosition = 0.0f;
    }

    public AircraftCurrentLane(PathElement pathElement, AircraftLaneFlags laneFlags)
    {
      this.m_Lane = pathElement.m_Target;
      this.m_CurvePosition = pathElement.m_TargetDelta.xxx;
      this.m_LaneFlags = laneFlags;
      this.m_Duration = 0.0f;
      this.m_Distance = 0.0f;
      this.m_LanePosition = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
      writer.Write((uint) this.m_LaneFlags);
      writer.Write(this.m_Duration);
      writer.Write(this.m_Distance);
      writer.Write(this.m_LanePosition);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_Duration);
      reader.Read(out this.m_Distance);
      reader.Read(out this.m_LanePosition);
      this.m_LaneFlags = (AircraftLaneFlags) num;
    }
  }
}
