// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.CarTrailerLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public struct CarTrailerLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Lane;
    public Entity m_NextLane;
    public float2 m_CurvePosition;
    public float2 m_NextPosition;
    public float m_Duration;
    public float m_Distance;

    public CarTrailerLane(ParkedCar parkedCar)
    {
      this.m_Lane = parkedCar.m_Lane;
      this.m_NextLane = Entity.Null;
      this.m_CurvePosition = (float2) parkedCar.m_CurvePosition;
      this.m_NextPosition = (float2) 0.0f;
      this.m_Duration = 0.0f;
      this.m_Distance = 0.0f;
    }

    public CarTrailerLane(CarCurrentLane currentLane)
    {
      this.m_Lane = currentLane.m_Lane;
      this.m_NextLane = Entity.Null;
      this.m_CurvePosition = currentLane.m_CurvePosition.xy;
      this.m_NextPosition = (float2) 0.0f;
      this.m_Duration = 0.0f;
      this.m_Distance = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_NextLane);
      writer.Write(this.m_CurvePosition);
      writer.Write(this.m_NextPosition);
      writer.Write(this.m_Duration);
      writer.Write(this.m_Distance);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_NextLane);
      reader.Read(out this.m_CurvePosition);
      reader.Read(out this.m_NextPosition);
      reader.Read(out this.m_Duration);
      reader.Read(out this.m_Distance);
    }
  }
}
