// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.ParkedTrain
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public struct ParkedTrain : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_ParkingLocation;
    public Entity m_FrontLane;
    public Entity m_RearLane;
    public float2 m_CurvePosition;

    public ParkedTrain(Entity location)
    {
      this.m_ParkingLocation = location;
      this.m_FrontLane = Entity.Null;
      this.m_RearLane = Entity.Null;
      this.m_CurvePosition = (float2) 0.0f;
    }

    public ParkedTrain(Entity location, TrainCurrentLane currentLane)
    {
      this.m_ParkingLocation = location;
      this.m_FrontLane = currentLane.m_Front.m_Lane;
      this.m_RearLane = currentLane.m_Rear.m_Lane;
      this.m_CurvePosition = new float2(currentLane.m_Front.m_CurvePosition.y, currentLane.m_Rear.m_CurvePosition.y);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ParkingLocation);
      writer.Write(this.m_FrontLane);
      writer.Write(this.m_RearLane);
      writer.Write(this.m_CurvePosition);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ParkingLocation);
      reader.Read(out this.m_FrontLane);
      reader.Read(out this.m_RearLane);
      reader.Read(out this.m_CurvePosition);
    }
  }
}
