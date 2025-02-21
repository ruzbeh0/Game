// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.TrainBogieLane
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
  public struct TrainBogieLane : ISerializable
  {
    public Entity m_Lane;
    public float4 m_CurvePosition;
    public TrainLaneFlags m_LaneFlags;

    public TrainBogieLane(TrainBogieCache cache)
    {
      this.m_Lane = cache.m_Lane;
      this.m_CurvePosition = cache.m_CurvePosition.xxxy;
      this.m_LaneFlags = cache.m_LaneFlags;
    }

    public TrainBogieLane(Entity lane, float4 curvePosition, TrainLaneFlags laneFlags)
    {
      this.m_Lane = lane;
      this.m_CurvePosition = curvePosition;
      this.m_LaneFlags = laneFlags;
    }

    public TrainBogieLane(TrainNavigationLane navLane)
    {
      this.m_Lane = navLane.m_Lane;
      this.m_CurvePosition = navLane.m_CurvePosition.xxxy;
      this.m_LaneFlags = navLane.m_Flags;
    }

    public TrainBogieLane(PathElement pathElement)
    {
      this.m_Lane = pathElement.m_Target;
      this.m_CurvePosition = pathElement.m_TargetDelta.xxxx;
      this.m_LaneFlags = (TrainLaneFlags) 0;
    }

    public TrainBogieLane(Entity lane, float curvePosition)
    {
      this.m_Lane = lane;
      this.m_CurvePosition = (float4) curvePosition;
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
      if (reader.context.version >= Version.tramNavigationImprovement)
      {
        reader.Read(out this.m_CurvePosition);
      }
      else
      {
        float3 float3;
        reader.Read(out float3);
        this.m_CurvePosition = float3.xyyz;
      }
      uint num;
      reader.Read(out num);
      this.m_LaneFlags = (TrainLaneFlags) num;
    }
  }
}
