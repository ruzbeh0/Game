// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GoodsDeliveryRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct GoodsDeliveryRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Target;
    public GoodsDeliveryFlags m_Flags;
    public Resource m_Resource;
    public int m_Amount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      writer.Write((ushort) this.m_Flags);
      writer.Write(EconomyUtils.GetResourceIndex(this.m_Resource));
      writer.Write(this.m_Amount);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      ushort num;
      reader.Read(out num);
      this.m_Flags = (GoodsDeliveryFlags) num;
      int index;
      reader.Read(out index);
      this.m_Resource = EconomyUtils.GetResource(index);
      reader.Read(out this.m_Amount);
    }
  }
}
