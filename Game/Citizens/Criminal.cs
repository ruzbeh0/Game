// Decompiled with JetBrains decompiler
// Type: Game.Citizens.Criminal
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct Criminal : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Event;
    public ushort m_JailTime;
    public CriminalFlags m_Flags;

    public Criminal(Entity _event, CriminalFlags flags)
    {
      this.m_Event = _event;
      this.m_JailTime = (ushort) 0;
      this.m_Flags = flags;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Event);
      writer.Write(this.m_JailTime);
      writer.Write((ushort) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Event);
      if (reader.context.version >= Version.policeImprovement2)
      {
        reader.Read(out this.m_JailTime);
        ushort num;
        reader.Read(out num);
        this.m_Flags = (CriminalFlags) num;
      }
      else
      {
        byte num;
        reader.Read(out num);
        this.m_Flags = (CriminalFlags) num;
      }
    }
  }
}
