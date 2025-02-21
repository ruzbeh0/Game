// Decompiled with JetBrains decompiler
// Type: Game.Objects.Aligned
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  public struct Aligned : IComponentData, IQueryTypeParameter, ISerializable
  {
    public ushort m_SubObjectIndex;

    public Aligned(ushort subObjectIndex) => this.m_SubObjectIndex = subObjectIndex;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_SubObjectIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_SubObjectIndex);
    }
  }
}
