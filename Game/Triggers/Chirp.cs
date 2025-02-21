// Decompiled with JetBrains decompiler
// Type: Game.Triggers.Chirp
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Triggers
{
  public struct Chirp : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Sender;
    public uint m_CreationFrame;
    public uint m_Likes;
    public uint m_TargetLikes;
    public uint m_InactiveFrame;
    public int m_ViralFactor;
    public float m_ContinuousFactor;
    public ChirpFlags m_Flags;

    public Chirp(Entity sender, uint creationFrame)
    {
      this.m_Sender = sender;
      this.m_CreationFrame = creationFrame;
      this.m_Likes = 0U;
      this.m_Flags = (ChirpFlags) 0;
      this.m_TargetLikes = 0U;
      this.m_InactiveFrame = 0U;
      this.m_ViralFactor = 1;
      this.m_ContinuousFactor = 0.2f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Sender);
      writer.Write(this.m_CreationFrame);
      writer.Write(this.m_Likes);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_TargetLikes);
      writer.Write(this.m_InactiveFrame);
      writer.Write(this.m_ViralFactor);
      writer.Write(this.m_ContinuousFactor);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Sender);
      reader.Read(out this.m_CreationFrame);
      if (reader.context.version >= Version.chirpLikes)
      {
        reader.Read(out this.m_Likes);
        byte num;
        reader.Read(out num);
        this.m_Flags = (ChirpFlags) num;
      }
      Context context = reader.context;
      if (context.version >= Version.randomChirpLikes)
      {
        reader.Read(out this.m_TargetLikes);
        reader.Read(out this.m_InactiveFrame);
        reader.Read(out this.m_ViralFactor);
      }
      context = reader.context;
      if (!(context.version >= Version.continuousChirpLikes))
        return;
      reader.Read(out this.m_ContinuousFactor);
    }
  }
}
