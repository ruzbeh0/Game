// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Zones;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ZoneData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public ZoneType m_ZoneType;
    public AreaType m_AreaType;
    public ZoneFlags m_ZoneFlags;
    public ushort m_MinOddHeight;
    public ushort m_MinEvenHeight;
    public ushort m_MaxHeight;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write<ZoneType>(this.m_ZoneType);
      writer.Write((byte) this.m_AreaType);
      writer.Write((byte) this.m_ZoneFlags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read<ZoneType>(out this.m_ZoneType);
      byte num1;
      reader.Read(out num1);
      byte num2;
      reader.Read(out num2);
      this.m_AreaType = (AreaType) num1;
      this.m_ZoneFlags = (ZoneFlags) num2;
    }
  }
}
