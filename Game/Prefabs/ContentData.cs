// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ContentData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ContentData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public ContentFlags m_Flags;
    public int m_DlcID;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_DlcID);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_DlcID);
      this.m_Flags = (ContentFlags) num;
    }
  }
}
