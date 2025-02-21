// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficSpawnerData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TrafficSpawnerData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_SpawnRate;
    public RoadTypes m_RoadType;
    public TrackTypes m_TrackType;
    public bool m_NoSlowVehicles;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_SpawnRate);
      writer.Write(this.m_NoSlowVehicles);
      writer.Write((byte) this.m_RoadType);
      writer.Write((byte) this.m_TrackType);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_SpawnRate);
      reader.Read(out this.m_NoSlowVehicles);
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      this.m_RoadType = (RoadTypes) num1;
      this.m_TrackType = (TrackTypes) num2;
    }
  }
}
