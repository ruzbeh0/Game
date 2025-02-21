// Decompiled with JetBrains decompiler
// Type: Game.Zones.Cell
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Zones
{
  [InternalBufferCapacity(60)]
  public struct Cell : IBufferElementData, IStrideSerializable, ISerializable
  {
    public CellFlags m_State;
    public ZoneType m_Zone;
    public short m_Height;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((ushort) this.m_State);
      writer.Write<ZoneType>(this.m_Zone);
      writer.Write(this.m_Height);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.cornerBuildings)
      {
        ushort num;
        reader.Read(out num);
        this.m_State = (CellFlags) num;
      }
      else
      {
        byte num;
        reader.Read(out num);
        this.m_State = (CellFlags) num;
      }
      reader.Read<ZoneType>(out this.m_Zone);
      if (reader.context.version >= Version.zoneHeightLimit)
        reader.Read(out this.m_Height);
      else
        this.m_Height = short.MaxValue;
    }

    public int GetStride(Context context)
    {
      return context.version >= Version.zoneHeightLimit ? 4 + this.m_Zone.GetStride(context) : 2 + this.m_Zone.GetStride(context);
    }
  }
}
