// Decompiled with JetBrains decompiler
// Type: Game.Citizens.Leisure
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct Leisure : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetAgent;
    public uint m_LastPossibleFrame;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetAgent);
      writer.Write(this.m_LastPossibleFrame);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TargetAgent);
      reader.Read(out this.m_LastPossibleFrame);
    }
  }
}
