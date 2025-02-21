// Decompiled with JetBrains decompiler
// Type: Game.Net.ConnectionLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct ConnectionLane : 
    IComponentData,
    IQueryTypeParameter,
    ISerializable,
    IEquatable<ConnectionLane>
  {
    public Entity m_AccessRestriction;
    public ConnectionLaneFlags m_Flags;
    public TrackTypes m_TrackTypes;
    public RoadTypes m_RoadTypes;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AccessRestriction);
      writer.Write((uint) this.m_Flags);
      writer.Write((byte) this.m_TrackTypes);
      writer.Write((byte) this.m_RoadTypes);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Game.Version.pathfindAccessRestriction)
        reader.Read(out this.m_AccessRestriction);
      uint num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      if (reader.context.version >= Game.Version.shipLanes)
      {
        byte num3;
        reader.Read(out num3);
        this.m_RoadTypes = (RoadTypes) num3;
      }
      this.m_Flags = (ConnectionLaneFlags) num1;
      this.m_TrackTypes = (TrackTypes) num2;
    }

    public bool Equals(ConnectionLane other)
    {
      return this.m_Flags == other.m_Flags && this.m_TrackTypes == other.m_TrackTypes && this.m_RoadTypes == other.m_RoadTypes;
    }
  }
}
