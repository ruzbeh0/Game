// Decompiled with JetBrains decompiler
// Type: Game.Net.ParkingLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Pathfind;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct ParkingLane : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_AccessRestriction;
    public PathNode m_SecondaryStartNode;
    public ParkingLaneFlags m_Flags;
    public float m_FreeSpace;
    public ushort m_ParkingFee;
    public ushort m_ComfortFactor;
    public ushort m_TaxiAvailability;
    public ushort m_TaxiFee;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AccessRestriction);
      writer.Write<PathNode>(this.m_SecondaryStartNode);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_FreeSpace);
      writer.Write(this.m_ParkingFee);
      writer.Write(this.m_ComfortFactor);
      writer.Write(this.m_TaxiAvailability);
      writer.Write(this.m_TaxiFee);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.pathfindAccessRestriction)
        reader.Read(out this.m_AccessRestriction);
      if (reader.context.version >= Version.parkingLaneImprovement)
        reader.Read<PathNode>(out this.m_SecondaryStartNode);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_FreeSpace);
      Context context = reader.context;
      if (context.version >= Version.parkingLaneImprovement2)
      {
        reader.Read(out this.m_ParkingFee);
        reader.Read(out this.m_ComfortFactor);
      }
      context = reader.context;
      if (context.version >= Version.taxiDispatchCenter)
        reader.Read(out this.m_TaxiAvailability);
      context = reader.context;
      if (context.version >= Version.taxiFee)
        reader.Read(out this.m_TaxiFee);
      this.m_Flags = (ParkingLaneFlags) num;
    }
  }
}
