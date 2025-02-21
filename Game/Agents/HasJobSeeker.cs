// Decompiled with JetBrains decompiler
// Type: Game.Agents.HasJobSeeker
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Agents
{
  public struct HasJobSeeker : 
    IComponentData,
    IQueryTypeParameter,
    ISerializable,
    IEnableableComponent
  {
    public Entity m_Seeker;
    public uint m_LastJobSeekFrameIndex;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Seeker);
      writer.Write(this.m_LastJobSeekFrameIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.seekerReferences)
        reader.Read(out this.m_Seeker);
      if (!(reader.context.version >= Version.findJobOptimize))
        return;
      reader.Read(out this.m_LastJobSeekFrameIndex);
    }
  }
}
