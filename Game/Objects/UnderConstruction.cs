// Decompiled with JetBrains decompiler
// Type: Game.Objects.UnderConstruction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  [FormerlySerializedAs("Game.Buildings.SetLevel, Game")]
  public struct UnderConstruction : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_NewPrefab;
    public byte m_Progress;
    public byte m_Speed;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_NewPrefab);
      writer.Write(this.m_Progress);
      writer.Write(this.m_Speed);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_NewPrefab);
      if (reader.context.version >= Version.constructionProgress)
        reader.Read(out this.m_Progress);
      else
        this.m_Progress = byte.MaxValue;
      if (reader.context.version >= Version.constructionSpeed)
        reader.Read(out this.m_Speed);
      else
        this.m_Speed = (byte) 50;
    }
  }
}
