// Decompiled with JetBrains decompiler
// Type: Game.City.PlayerMoney
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.City
{
  public struct PlayerMoney : IComponentData, IQueryTypeParameter, ISerializable
  {
    public const int kMaxMoney = 2000000000;
    private int m_Money;
    public bool m_Unlimited;

    public int money => !this.m_Unlimited ? this.m_Money : 2000000000;

    public PlayerMoney(int amount)
    {
      this.m_Money = math.clamp(amount, -2000000000, 2000000000);
      this.m_Unlimited = false;
    }

    public void Add(int value)
    {
      this.m_Money = math.clamp(this.m_Money + value, -2000000000, 2000000000);
    }

    public void Subtract(int amount) => this.Add(-amount);

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Money);
      writer.Write(this.m_Unlimited);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Money);
      if (!(reader.context.version >= Version.unlimitedMoneyAndUnlockAllOptions))
        return;
      reader.Read(out this.m_Unlimited);
    }
  }
}
