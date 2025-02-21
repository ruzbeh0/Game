// Decompiled with JetBrains decompiler
// Type: Game.Net.CarLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct CarLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_AccessRestriction;
    public CarLaneFlags m_Flags;
    public float m_DefaultSpeedLimit;
    public float m_SpeedLimit;
    public float m_Curviness;
    public ushort m_CarriagewayGroup;
    public byte m_BlockageStart;
    public byte m_BlockageEnd;
    public byte m_CautionStart;
    public byte m_CautionEnd;
    public byte m_FlowOffset;
    public byte m_LaneCrossCount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AccessRestriction);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_DefaultSpeedLimit);
      writer.Write(this.m_SpeedLimit);
      writer.Write(this.m_Curviness);
      writer.Write(this.m_CarriagewayGroup);
      writer.Write(this.m_BlockageStart);
      writer.Write(this.m_BlockageEnd);
      writer.Write(this.m_CautionStart);
      writer.Write(this.m_CautionEnd);
      writer.Write(this.m_FlowOffset);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.pathfindAccessRestriction)
        reader.Read(out this.m_AccessRestriction);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_DefaultSpeedLimit);
      Context context = reader.context;
      if (context.version >= Version.modifiedSpeedLimit)
        reader.Read(out this.m_SpeedLimit);
      else
        this.m_SpeedLimit = this.m_DefaultSpeedLimit;
      reader.Read(out this.m_Curviness);
      reader.Read(out this.m_CarriagewayGroup);
      context = reader.context;
      if (context.version >= Version.carLaneBlockage)
      {
        reader.Read(out this.m_BlockageStart);
        reader.Read(out this.m_BlockageEnd);
      }
      else
      {
        this.m_BlockageStart = byte.MaxValue;
        this.m_BlockageEnd = (byte) 0;
      }
      context = reader.context;
      if (context.version >= Version.trafficImprovements)
      {
        reader.Read(out this.m_CautionStart);
        reader.Read(out this.m_CautionEnd);
      }
      else
      {
        this.m_CautionStart = byte.MaxValue;
        this.m_CautionEnd = (byte) 0;
      }
      context = reader.context;
      if (context.version >= Version.pathfindImprovement)
        reader.Read(out this.m_FlowOffset);
      this.m_Flags = (CarLaneFlags) num;
      context = reader.context;
      if (!(context.version < Version.roadSideConnectionImprovements))
        return;
      if ((this.m_Flags & CarLaneFlags.Unsafe) != ~(CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.SideConnection | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter))
        this.m_Flags |= CarLaneFlags.SideConnection;
      else
        this.m_Flags &= CarLaneFlags.Unsafe | CarLaneFlags.UTurnLeft | CarLaneFlags.Invert | CarLaneFlags.TurnLeft | CarLaneFlags.TurnRight | CarLaneFlags.LevelCrossing | CarLaneFlags.Twoway | CarLaneFlags.IsSecured | CarLaneFlags.Runway | CarLaneFlags.Yield | CarLaneFlags.Stop | CarLaneFlags.ForbidCombustionEngines | CarLaneFlags.ForbidTransitTraffic | CarLaneFlags.ForbidHeavyTraffic | CarLaneFlags.PublicOnly | CarLaneFlags.Highway | CarLaneFlags.UTurnRight | CarLaneFlags.GentleTurnLeft | CarLaneFlags.GentleTurnRight | CarLaneFlags.Forward | CarLaneFlags.Approach | CarLaneFlags.Roundabout | CarLaneFlags.RightLimit | CarLaneFlags.LeftLimit | CarLaneFlags.ForbidPassing | CarLaneFlags.RightOfWay | CarLaneFlags.TrafficLights | CarLaneFlags.ParkingLeft | CarLaneFlags.ParkingRight | CarLaneFlags.Forbidden | CarLaneFlags.AllowEnter;
    }

    public Bounds1 blockageBounds
    {
      get
      {
        return new Bounds1((float) this.m_BlockageStart * 0.003921569f, (float) this.m_BlockageEnd * 0.003921569f);
      }
    }

    public Bounds1 cautionBounds
    {
      get
      {
        return new Bounds1((float) this.m_CautionStart * 0.003921569f, (float) this.m_CautionEnd * 0.003921569f);
      }
    }
  }
}
