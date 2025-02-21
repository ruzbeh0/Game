// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathInformations
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  [InternalBufferCapacity(0)]
  public struct PathInformations : IBufferElementData, ISerializable
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
      reader.Read(out this.m_TotalCost);
      ushort num1;
      reader.Read(out num1);
      ushort num2;
      reader.Read(out num2);
      this.m_Methods = (PathMethod) num1;
      this.m_State = (PathFlags) num2;
      if ((this.m_State & PathFlags.Pending) == (PathFlags) 0)
        return;
      this.m_State &= ~PathFlags.Pending;
      this.m_State |= PathFlags.Obsolete;
    }
  }
}
