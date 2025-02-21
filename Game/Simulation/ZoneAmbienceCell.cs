// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ZoneAmbienceCell
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ZoneAmbienceCell : IStrideSerializable, ISerializable
  {
    public ZoneAmbiences m_Accumulator;
    public ZoneAmbiences m_Value;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write<ZoneAmbiences>(this.m_Accumulator);
      writer.Write<ZoneAmbiences>(this.m_Value);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read<ZoneAmbiences>(out this.m_Accumulator);
      reader.Read<ZoneAmbiences>(out this.m_Value);
    }

    public int GetStride(Context context)
    {
      return this.m_Accumulator.GetStride(context) + this.m_Value.GetStride(context);
    }
  }
}
