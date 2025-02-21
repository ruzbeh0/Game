// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.TrainBogieCache
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
  public struct TrainBogieCache : ISerializable
  {
    public Entity m_Lane;
    public float2 m_CurvePosition;
    public TrainLaneFlags m_LaneFlags;

    public TrainBogieCache(TrainBogieLane lane)
    {
      this.m_Lane = lane.m_Lane;
      this.m_CurvePosition = lane.m_CurvePosition.xw;
      this.m_LaneFlags = lane.m_LaneFlags;
    }

    public TrainBogieCache(PathElement pathElement)
    {
      this.m_Lane = pathElement.m_Target;
      this.m_CurvePosition = pathElement.m_TargetDelta.xx;
      this.m_LaneFlags = (TrainLaneFlags) 0;
    }

    public TrainBogieCache(Entity lane, float curvePosition)
    {
      this.m_Lane = lane;
      this.m_CurvePosition = (float2) curvePosition;
      this.m_LaneFlags = (TrainLaneFlags) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
      writer.Write((uint) this.m_LaneFlags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
      uint num;
      reader.Read(out num);
      this.m_LaneFlags = (TrainLaneFlags) num;
    }
  }
}
