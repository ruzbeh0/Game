// Decompiled with JetBrains decompiler
// Type: Game.Citizens.LookingForPartner
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  public struct LookingForPartner : IBufferElementData, ISerializable
  {
    public Entity m_Citizen;
    public PartnerType m_PartnerType;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Citizen);
      int num;
      reader.Read(out num);
      this.m_PartnerType = (PartnerType) num;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Citizen);
      writer.Write((int) this.m_PartnerType);
    }
  }
}
