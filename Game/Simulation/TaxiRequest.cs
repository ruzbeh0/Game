// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TaxiRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct TaxiRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Seeker;
    public Entity m_District1;
    public Entity m_District2;
    public int m_Priority;
    public TaxiRequestType m_Type;

    public TaxiRequest(
      Entity seeker,
      Entity district1,
      Entity district2,
      TaxiRequestType type,
      int priority)
    {
      this.m_Seeker = seeker;
      this.m_District1 = district1;
      this.m_District2 = district2;
      this.m_Priority = priority;
      this.m_Type = type;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Seeker);
      writer.Write(this.m_District1);
      writer.Write(this.m_District2);
      writer.Write(this.m_Priority);
      writer.Write((byte) this.m_Type);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Seeker);
      if (reader.context.version >= Version.taxiServiceDistricts)
      {
        reader.Read(out this.m_District1);
        reader.Read(out this.m_District2);
      }
      reader.Read(out this.m_Priority);
      if (!(reader.context.version >= Version.taxiDispatchCenter))
        return;
      byte num;
      reader.Read(out num);
      this.m_Type = (TaxiRequestType) num;
    }
  }
}
