// Decompiled with JetBrains decompiler
// Type: Game.Events.AccidentSite
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public struct AccidentSite : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Event;
    public Entity m_PoliceRequest;
    public AccidentSiteFlags m_Flags;
    public uint m_CreationFrame;
    public uint m_SecuredFrame;

    public AccidentSite(Entity _event, AccidentSiteFlags flags, uint currentFrame)
    {
      this.m_Event = _event;
      this.m_PoliceRequest = Entity.Null;
      this.m_Flags = flags;
      this.m_CreationFrame = currentFrame;
      this.m_SecuredFrame = 0U;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Event);
      writer.Write(this.m_PoliceRequest);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_CreationFrame);
      writer.Write(this.m_SecuredFrame);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Event);
      reader.Read(out this.m_PoliceRequest);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_CreationFrame);
      if (reader.context.version >= Version.policeImprovement)
        reader.Read(out this.m_SecuredFrame);
      this.m_Flags = (AccidentSiteFlags) num;
    }
  }
}
