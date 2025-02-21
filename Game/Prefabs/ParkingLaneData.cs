// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ParkingLaneData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct ParkingLaneData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float2 m_SlotSize;
    public float m_SlotAngle;
    public float m_SlotInterval;
    public float m_MaxCarLength;
    public RoadTypes m_RoadTypes;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_SlotSize);
      writer.Write(this.m_SlotAngle);
      writer.Write(this.m_SlotInterval);
      writer.Write(this.m_MaxCarLength);
      writer.Write((byte) this.m_RoadTypes);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_SlotSize);
      reader.Read(out this.m_SlotAngle);
      reader.Read(out this.m_SlotInterval);
      reader.Read(out this.m_MaxCarLength);
      if (reader.context.version >= Version.roadPatchImprovements)
      {
        byte num;
        reader.Read(out num);
        this.m_RoadTypes = (RoadTypes) num;
      }
      else
        this.m_RoadTypes = RoadTypes.Car;
    }
  }
}
