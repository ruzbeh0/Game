// Decompiled with JetBrains decompiler
// Type: Game.City.ServiceFee
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.City
{
  public struct ServiceFee : IBufferElementData, ISerializable
  {
    public PlayerResource m_Resource;
    public float m_Fee;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((int) this.m_Resource);
      writer.Write(this.m_Fee);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.purpose == Purpose.NewGame)
      {
        int num;
        reader.Read(out num);
        this.m_Resource = (PlayerResource) num;
        reader.Read(out float _);
        this.m_Fee = this.GetDefaultFee(this.m_Resource);
      }
      else
      {
        int num;
        reader.Read(out num);
        this.m_Resource = (PlayerResource) num;
        reader.Read(out this.m_Fee);
        if (!(reader.context.version < Version.waterFeeReset) || this.m_Resource != PlayerResource.Water)
          return;
        this.m_Fee = 0.3f;
      }
    }

    public float GetDefaultFee(PlayerResource resource)
    {
      switch (resource)
      {
        case PlayerResource.Electricity:
          return 0.2f;
        case PlayerResource.Healthcare:
          return 100f;
        case PlayerResource.BasicEducation:
          return 100f;
        case PlayerResource.SecondaryEducation:
          return 200f;
        case PlayerResource.HigherEducation:
          return 300f;
        case PlayerResource.Garbage:
          return 0.1f;
        case PlayerResource.Water:
          return 0.1f;
        default:
          return 0.0f;
      }
    }
  }
}
