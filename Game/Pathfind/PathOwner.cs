// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathOwner
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  public struct PathOwner : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_ElementIndex;
    public PathFlags m_State;

    public PathOwner(PathFlags state)
    {
      this.m_ElementIndex = 0;
      this.m_State = state;
    }

    public PathOwner(int elementIndex, PathFlags state)
    {
      this.m_ElementIndex = elementIndex;
      this.m_State = state;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ElementIndex);
      writer.Write((ushort) this.m_State);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ElementIndex);
      ushort num;
      reader.Read(out num);
      this.m_State = (PathFlags) num;
      if ((this.m_State & PathFlags.Pending) == (PathFlags) 0)
        return;
      this.m_State &= ~PathFlags.Pending;
      this.m_State |= PathFlags.Obsolete;
    }
  }
}
