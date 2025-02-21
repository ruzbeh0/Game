// Decompiled with JetBrains decompiler
// Type: Game.Companies.TradeCost
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Companies
{
  public struct TradeCost : IBufferElementData, ISerializable
  {
    public Resource m_Resource;
    public float m_BuyCost;
    public float m_SellCost;
    public long m_LastTransferRequestTime;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resource));
      writer.Write(this.m_BuyCost);
      writer.Write(this.m_SellCost);
      writer.Write(this.m_LastTransferRequestTime);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      sbyte index;
      reader.Read(out index);
      reader.Read(out this.m_BuyCost);
      if (float.IsNaN(this.m_BuyCost))
        this.m_BuyCost = 0.0f;
      reader.Read(out this.m_SellCost);
      if (float.IsNaN(this.m_SellCost))
        this.m_SellCost = 0.0f;
      reader.Read(out this.m_LastTransferRequestTime);
      this.m_Resource = EconomyUtils.GetResource((int) index);
    }
  }
}
