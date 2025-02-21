// Decompiled with JetBrains decompiler
// Type: Game.Net.MasterLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct MasterLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public uint m_Group;
    public ushort m_MinIndex;
    public ushort m_MaxIndex;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Group);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Group);
      if (!(reader.context.version < Version.laneCountOverflowFix))
        return;
      byte num;
      reader.Read(out num);
      reader.Read(out num);
    }
  }
}
