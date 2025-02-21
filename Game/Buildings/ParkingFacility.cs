// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ParkingFacility
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct ParkingFacility : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_ComfortFactor;
    public ParkingFacilityFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ComfortFactor);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ComfortFactor);
      byte num;
      reader.Read(out num);
      this.m_Flags = (ParkingFacilityFlags) num;
    }
  }
}
