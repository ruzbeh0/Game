// Decompiled with JetBrains decompiler
// Type: Game.Creatures.AnimalCurrentLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Creatures
{
  public struct AnimalCurrentLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Lane;
    public Entity m_NextLane;
    public Entity m_QueueEntity;
    public Sphere3 m_QueueArea;
    public float2 m_CurvePosition;
    public float2 m_NextPosition;
    public CreatureLaneFlags m_Flags;
    public CreatureLaneFlags m_NextFlags;
    public float m_LanePosition;

    public AnimalCurrentLane(Entity lane, float curvePosition, CreatureLaneFlags flags)
    {
      this.m_Lane = lane;
      this.m_NextLane = Entity.Null;
      this.m_QueueEntity = Entity.Null;
      this.m_QueueArea = new Sphere3();
      this.m_CurvePosition = (float2) curvePosition;
      this.m_NextPosition = (float2) 0.0f;
      this.m_Flags = flags;
      this.m_NextFlags = (CreatureLaneFlags) 0;
      this.m_LanePosition = 0.0f;
    }

    public AnimalCurrentLane(CreatureLaneFlags flags)
    {
      this.m_Lane = Entity.Null;
      this.m_NextLane = Entity.Null;
      this.m_QueueEntity = Entity.Null;
      this.m_QueueArea = new Sphere3();
      this.m_CurvePosition = (float2) 0.0f;
      this.m_NextPosition = (float2) 0.0f;
      this.m_Flags = flags;
      this.m_NextFlags = (CreatureLaneFlags) 0;
      this.m_LanePosition = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Lane);
      writer.Write(this.m_CurvePosition);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_LanePosition);
      writer.Write(this.m_NextLane);
      writer.Write(this.m_NextPosition);
      writer.Write((uint) this.m_NextFlags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Lane);
      reader.Read(out this.m_CurvePosition);
      uint num1;
      reader.Read(out num1);
      reader.Read(out this.m_LanePosition);
      this.m_Flags = (CreatureLaneFlags) num1;
      if (!(reader.context.version >= Version.animalNavigation))
        return;
      reader.Read(out this.m_NextLane);
      reader.Read(out this.m_NextPosition);
      uint num2;
      reader.Read(out num2);
      this.m_NextFlags = (CreatureLaneFlags) num2;
    }
  }
}
