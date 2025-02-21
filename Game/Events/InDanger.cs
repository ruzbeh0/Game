// Decompiled with JetBrains decompiler
// Type: Game.Events.InDanger
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public struct InDanger : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Event;
    public Entity m_EvacuationRequest;
    public DangerFlags m_Flags;
    public uint m_EndFrame;

    public InDanger(Entity _event, Entity evacuationRequest, DangerFlags flags, uint endFrame)
    {
      this.m_Event = _event;
      this.m_EvacuationRequest = evacuationRequest;
      this.m_Flags = flags;
      this.m_EndFrame = endFrame;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Event);
      writer.Write(this.m_EvacuationRequest);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_EndFrame);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Event);
      reader.Read(out this.m_EvacuationRequest);
      uint num;
      reader.Read(out num);
      this.m_Flags = (DangerFlags) num;
      if (!(reader.context.version >= Version.dangerTimeout))
        return;
      reader.Read(out this.m_EndFrame);
    }
  }
}
