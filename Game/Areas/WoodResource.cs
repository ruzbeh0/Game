// Decompiled with JetBrains decompiler
// Type: Game.Areas.WoodResource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Areas
{
  [InternalBufferCapacity(0)]
  public struct WoodResource : IBufferElementData, ISerializable
  {
    public Entity m_Tree;

    public WoodResource(Entity tree) => this.m_Tree = tree;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Tree);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Tree);
    }
  }
}
