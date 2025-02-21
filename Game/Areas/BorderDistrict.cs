// Decompiled with JetBrains decompiler
// Type: Game.Areas.BorderDistrict
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Areas
{
  public struct BorderDistrict : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Left;
    public Entity m_Right;

    public BorderDistrict(Entity left, Entity right)
    {
      this.m_Left = left;
      this.m_Right = right;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Left);
      writer.Write(this.m_Right);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Left);
      reader.Read(out this.m_Right);
    }
  }
}
