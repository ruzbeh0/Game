// Decompiled with JetBrains decompiler
// Type: Game.Common.Destroyed
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Common
{
  public struct Destroyed : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Event;
    public float m_Cleared;

    public Destroyed(Entity _event)
    {
      this.m_Event = _event;
      this.m_Cleared = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Event);
      writer.Write(this.m_Cleared);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Event);
      if (!(reader.context.version >= Version.destroyedCleared))
        return;
      reader.Read(out this.m_Cleared);
    }
  }
}
