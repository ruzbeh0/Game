// Decompiled with JetBrains decompiler
// Type: Game.Routes.Route
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct Route : IComponentData, IQueryTypeParameter, ISerializable
  {
    public RouteFlags m_Flags;
    public uint m_OptionMask;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_OptionMask);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      this.m_Flags = (RouteFlags) num;
      if (!(reader.context.version >= Version.routePolicies))
        return;
      reader.Read(out this.m_OptionMask);
    }
  }
}
