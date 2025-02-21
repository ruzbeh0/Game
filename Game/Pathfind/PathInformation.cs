// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathInformation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct PathInformation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Origin;
    public Entity m_Destination;
    public float m_Distance;
    public float m_Duration;
    public float m_TotalCost;
    public PathMethod m_Methods;
    public PathFlags m_State;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Origin);
      writer.Write(this.m_Destination);
      writer.Write(this.m_Distance);
      writer.Write(this.m_Duration);
      writer.Write(this.m_TotalCost);
      writer.Write((ushort) this.m_Methods);
      writer.Write((ushort) this.m_State);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Origin);
      reader.Read(out this.m_Destination);
      reader.Read(out this.m_Distance);
      reader.Read(out this.m_Duration);
      if (reader.context.version >= Version.totalPathfindCost)
        reader.Read(out this.m_TotalCost);
      Context context = reader.context;
      if (context.version >= Version.usedPathfindMethods)
      {
        ushort num;
        reader.Read(out num);
        this.m_Methods = (PathMethod) num;
      }
      context = reader.context;
      if (context.version >= Version.pathfindState)
      {
        ushort num;
        reader.Read(out num);
        this.m_State = (PathFlags) num;
      }
      if ((this.m_State & PathFlags.Pending) == (PathFlags) 0)
        return;
      this.m_State &= ~PathFlags.Pending;
      this.m_State |= PathFlags.Obsolete;
    }
  }
}
