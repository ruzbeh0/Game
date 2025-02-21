// Decompiled with JetBrains decompiler
// Type: Game.Economy.ResourceInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Economy
{
  public struct ResourceInfo : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Resource m_Resource;
    public float m_Price;
    public float m_TradeDistance;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resource));
      writer.Write(this.m_Price);
      writer.Write(this.m_TradeDistance);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      sbyte index;
      reader.Read(out index);
      reader.Read(out this.m_Price);
      reader.Read(out this.m_TradeDistance);
      this.m_Resource = EconomyUtils.GetResource((int) index);
    }
  }
}
