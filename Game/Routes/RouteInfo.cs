// Decompiled with JetBrains decompiler
// Type: Game.Routes.RouteInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct RouteInfo : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_Duration;
    public float m_Distance;
    public RouteInfoFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Duration);
      writer.Write(this.m_Distance);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Duration);
      reader.Read(out this.m_Distance);
      if (!(reader.context.version >= Version.transportLinePolicies))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (RouteInfoFlags) num;
    }
  }
}
