// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AirPollution
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct AirPollution : IPollution, IStrideSerializable, ISerializable
  {
    public short m_Pollution;

    public void Add(short amount)
    {
      this.m_Pollution = (short) math.min((int) short.MaxValue, (int) this.m_Pollution + (int) amount);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Pollution);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Pollution);
    }

    public int GetStride(Context context) => 2;
  }
}
