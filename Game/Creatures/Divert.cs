// Decompiled with JetBrains decompiler
// Type: Game.Creatures.Divert
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct Divert : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Target;
    public Resource m_Resource;
    public int m_Data;
    public Game.Citizens.Purpose m_Purpose;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      writer.Write((byte) this.m_Purpose);
      writer.Write(this.m_Data);
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resource));
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      byte num;
      reader.Read(out num);
      this.m_Purpose = (Game.Citizens.Purpose) num;
      if (!(reader.context.version >= Version.divertResources))
        return;
      reader.Read(out this.m_Data);
      sbyte index;
      reader.Read(out index);
      this.m_Resource = EconomyUtils.GetResource((int) index);
    }
  }
}
