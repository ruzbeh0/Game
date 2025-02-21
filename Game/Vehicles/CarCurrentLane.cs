// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.CarCurrentLane
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
  public struct CarCurrentLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Lane;
    public Entity m_ChangeLane;
    public float3 m_CurvePosition;
    public CarLaneFlags m_LaneFlags;
    public float m_ChangeProgress;
    public float m_Duration;
    public float m_Distance;
    public float m_LanePosition;

    public CarCurrentLane(ParkedCar parkedCar, CarLaneFlags flags)
    {
      this.m_Lane = parkedCar.m_Lane;
      this.m_ChangeLane = Entity.Null;
      this.m_CurvePosition = (float3) parkedCar.m_CurvePosition;
      this.m_LaneFlags = flags;
      this.m_ChangeProgress = 0.0f;
      this.m_Duration = 0.0f;
      this.m_Distance = 0.0f;
      this.m_LanePosition = 0.0f;
    }

    public CarCurrentLane(PathElement pathElement, CarLaneFlags flags)
    {
      this.m_Lane = pathElement.m_Target;
      this.m_ChangeLane = Entity.Null;
      this.m_CurvePosition = pathElement.m_TargetDelta.xxx;
      this.m_LaneFlags = flags;
      this.m_ChangeProgress = 0.0f;
      this.m_Duration = 0.0f;
      this.m_Distance = 0.0f;
      this.m_LanePosition = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_ChangeLane);
      writer.Write(this.m_CurvePosition);
      writer.Write((uint) this.m_LaneFlags);
      writer.Write(this.m_ChangeProgress);
      writer.Write(this.m_Duration);
      writer.Write(this.m_Distance);
      writer.Write(this.m_LanePosition);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_ChangeLane);
      reader.Read(out this.m_CurvePosition);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_ChangeProgress);
      reader.Read(out this.m_Duration);
      reader.Read(out this.m_Distance);
      if (reader.context.version >= Version.lanePosition)
        reader.Read(out this.m_LanePosition);
      this.m_LaneFlags = (CarLaneFlags) num;
    }
  }
}
