// Decompiled with JetBrains decompiler
// Type: Game.Tools.Recent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Tools
{
  public struct Recent : IComponentData, IQueryTypeParameter, ISerializable
  {
    public uint m_ModificationFrame;
    public int m_ModificationCost;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ModificationFrame);
      writer.Write(this.m_ModificationCost);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ModificationFrame);
      reader.Read(out this.m_ModificationCost);
    }
  }
}
