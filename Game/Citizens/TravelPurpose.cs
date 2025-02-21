// Decompiled with JetBrains decompiler
// Type: Game.Citizens.TravelPurpose
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct TravelPurpose : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Purpose m_Purpose;
    public int m_Data;
    public Resource m_Resource;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_Purpose);
      writer.Write(this.m_Data);
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resource));
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_Data);
      sbyte index;
      reader.Read(out index);
      this.m_Purpose = (Purpose) num;
      this.m_Resource = EconomyUtils.GetResource((int) index);
    }
  }
}
