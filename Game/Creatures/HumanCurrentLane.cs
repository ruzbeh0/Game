// Decompiled with JetBrains decompiler
// Type: Game.Creatures.HumanCurrentLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Pathfind;
using Game.Routes;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Creatures
{
  public struct HumanCurrentLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Lane;
    public Entity m_QueueEntity;
    public Sphere3 m_QueueArea;
    public float2 m_CurvePosition;
    public CreatureLaneFlags m_Flags;
    public float m_LanePosition;

    public HumanCurrentLane(AccessLane accessLane, CreatureLaneFlags flags)
    {
      this.m_Lane = accessLane.m_Lane;
      this.m_QueueEntity = Entity.Null;
      this.m_QueueArea = new Sphere3();
      this.m_CurvePosition = (float2) accessLane.m_CurvePos;
      this.m_Flags = flags;
      this.m_LanePosition = 0.0f;
    }

    public HumanCurrentLane(PathElement pathElement, CreatureLaneFlags flags)
    {
      this.m_Lane = pathElement.m_Target;
      this.m_QueueEntity = Entity.Null;
      this.m_QueueArea = new Sphere3();
      this.m_CurvePosition = pathElement.m_TargetDelta.xx;
      this.m_Flags = flags;
      this.m_LanePosition = 0.0f;
    }

    public HumanCurrentLane(CreatureLaneFlags flags)
    {
      this.m_Lane = Entity.Null;
      this.m_QueueEntity = Entity.Null;
      this.m_QueueArea = new Sphere3();
      this.m_CurvePosition = (float2) 0.0f;
      this.m_Flags = flags;
      this.m_LanePosition = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_LanePosition);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
      uint num;
      reader.Read(out num);
      if (reader.context.version >= Version.lanePosition)
        reader.Read(out this.m_LanePosition);
      this.m_Flags = (CreatureLaneFlags) num;
    }
  }
}
