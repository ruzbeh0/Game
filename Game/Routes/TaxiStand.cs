// Decompiled with JetBrains decompiler
// Type: Game.Routes.TaxiStand
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  public struct TaxiStand : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TaxiRequest;
    public TaxiStandFlags m_Flags;
    public ushort m_StartingFee;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TaxiRequest);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_StartingFee);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TaxiRequest);
      if (reader.context.version >= Version.taxiStandFlags)
      {
        uint num;
        reader.Read(out num);
        this.m_Flags = (TaxiStandFlags) num;
      }
      if (!(reader.context.version >= Version.taxiFee))
        return;
      reader.Read(out this.m_StartingFee);
    }
  }
}
