// Decompiled with JetBrains decompiler
// Type: Game.Net.Upgraded
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct Upgraded : IComponentData, IQueryTypeParameter, ISerializable
  {
    public CompositionFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write<CompositionFlags>(this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read<CompositionFlags>(out this.m_Flags);
    }
  }
}
