// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GroundWater
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct GroundWater : IStrideSerializable, ISerializable
  {
    public short m_Amount;
    public short m_Polluted;
    public short m_Max;

    public void Consume(int amount)
    {
      if (this.m_Amount <= (short) 0)
        return;
      float num = (float) this.m_Polluted / (float) this.m_Amount;
      this.m_Amount -= (short) math.clamp(amount, 0, (int) this.m_Amount);
      this.m_Polluted = (short) math.clamp(math.round(num * (float) this.m_Amount), 0.0f, (float) this.m_Amount);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Amount);
      writer.Write(this.m_Polluted);
      writer.Write(this.m_Max);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Amount);
      reader.Read(out this.m_Polluted);
      reader.Read(out this.m_Max);
      if (!(reader.context.version < Version.groundWaterPollutionFix))
        return;
      this.m_Amount = (short) math.clamp((int) this.m_Amount, 0, (int) this.m_Max);
      this.m_Polluted = (short) math.clamp((int) this.m_Polluted, 0, (int) this.m_Amount);
    }

    public int GetStride(Context context) => 6;
  }
}
