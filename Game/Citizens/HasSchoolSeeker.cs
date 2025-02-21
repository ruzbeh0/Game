// Decompiled with JetBrains decompiler
// Type: Game.Citizens.HasSchoolSeeker
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct HasSchoolSeeker : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Seeker;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Seeker);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (!(reader.context.version >= Version.seekerReferences))
        return;
      reader.Read(out this.m_Seeker);
    }
  }
}
