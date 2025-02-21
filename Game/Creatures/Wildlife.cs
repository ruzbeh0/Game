// Decompiled with JetBrains decompiler
// Type: Game.Creatures.Wildlife
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct Wildlife : IComponentData, IQueryTypeParameter, ISerializable
  {
    public WildlifeFlags m_Flags;
    public ushort m_StateTime;
    public ushort m_LifeTime;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_StateTime);
      writer.Write(this.m_LifeTime);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_StateTime);
      reader.Read(out this.m_LifeTime);
      this.m_Flags = (WildlifeFlags) num;
    }
  }
}
