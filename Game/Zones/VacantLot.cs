// Decompiled with JetBrains decompiler
// Type: Game.Zones.VacantLot
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  [InternalBufferCapacity(1)]
  public struct VacantLot : IBufferElementData, IEquatable<VacantLot>, ISerializable
  {
    public int4 m_Area;
    public ZoneType m_Type;
    public short m_Height;
    public LotFlags m_Flags;

    public VacantLot(int2 min, int2 max, ZoneType type, int height, LotFlags flags)
    {
      this.m_Area = new int4(min.x, max.x, min.y, max.y);
      this.m_Type = type;
      this.m_Height = (short) height;
      this.m_Flags = flags;
    }

    public bool Equals(VacantLot other) => this.m_Area.Equals(other.m_Area);

    public override int GetHashCode()
    {
      return (17 * 31 + this.m_Area.GetHashCode()) * 31 + this.m_Type.GetHashCode();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Area);
      writer.Write<ZoneType>(this.m_Type);
      writer.Write(this.m_Height);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Area);
      reader.Read<ZoneType>(out this.m_Type);
      if (reader.context.version >= Game.Version.zoneHeightLimit)
        reader.Read(out this.m_Height);
      else
        this.m_Height = short.MaxValue;
      if (!(reader.context.version >= Game.Version.cornerBuildings))
        return;
      byte num;
      reader.Read(out num);
      this.m_Flags = (LotFlags) num;
    }
  }
}
