// Decompiled with JetBrains decompiler
// Type: Game.Objects.SpawnLocation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  public struct SpawnLocation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_AccessRestriction;
    public Entity m_ConnectedLane1;
    public Entity m_ConnectedLane2;
    public float m_CurvePosition1;
    public float m_CurvePosition2;
    public int m_GroupIndex;
    public SpawnLocationFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AccessRestriction);
      writer.Write(this.m_ConnectedLane1);
      writer.Write(this.m_ConnectedLane2);
      writer.Write(this.m_CurvePosition1);
      writer.Write(this.m_CurvePosition2);
      writer.Write(this.m_GroupIndex);
      writer.Write((uint) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.pathfindAccessRestriction)
        reader.Read(out this.m_AccessRestriction);
      if (reader.context.version >= Version.spawnLocationRefactor)
      {
        reader.Read(out this.m_ConnectedLane1);
        reader.Read(out this.m_ConnectedLane2);
        reader.Read(out this.m_CurvePosition1);
        reader.Read(out this.m_CurvePosition2);
      }
      else
      {
        reader.Read(out Entity _);
        reader.Read(out float _);
      }
      if (reader.context.version >= Version.spawnLocationGroup)
        reader.Read(out this.m_GroupIndex);
      if (!(reader.context.version >= Version.pathfindRestrictions))
        return;
      uint num;
      reader.Read(out num);
      this.m_Flags = (SpawnLocationFlags) num;
    }
  }
}
