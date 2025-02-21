// Decompiled with JetBrains decompiler
// Type: Game.Citizens.Citizen
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Simulation;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Citizens
{
  public struct Citizen : IComponentData, IQueryTypeParameter, ISerializable
  {
    public ushort m_PseudoRandom;
    public CitizenFlags m_State;
    public byte m_WellBeing;
    public byte m_Health;
    public byte m_LeisureCounter;
    public byte m_PenaltyCounter;
    public int m_UnemploymentCounter;
    public short m_BirthDay;

    public float GetAgeInDays(uint simulationFrame, TimeData timeData)
    {
      // ISSUE: reference to a compiler-generated method
      return (float) (TimeSystem.GetDay(simulationFrame, timeData) - (int) this.m_BirthDay);
    }

    public Random GetPseudoRandom(CitizenPseudoRandom reason)
    {
      Random random = new Random((uint) ((ulong) reason ^ (ulong) ((int) this.m_PseudoRandom << 16 | (int) this.m_PseudoRandom)));
      int num = (int) random.NextUInt();
      uint a = random.NextUInt();
      return new Random(math.select(a, uint.MaxValue, a == 0U));
    }

    public int Happiness => ((int) this.m_WellBeing + (int) this.m_Health) / 2;

    public int GetEducationLevel()
    {
      return (this.m_State & CitizenFlags.EducationBit3) != CitizenFlags.None ? 4 : ((this.m_State & CitizenFlags.EducationBit1) != CitizenFlags.None ? 2 : 0) + ((this.m_State & CitizenFlags.EducationBit2) != CitizenFlags.None ? 1 : 0);
    }

    public void SetEducationLevel(int level)
    {
      if (level == 4)
        this.m_State |= CitizenFlags.EducationBit3;
      else
        this.m_State &= ~CitizenFlags.EducationBit3;
      if (level >= 2)
        this.m_State |= CitizenFlags.EducationBit1;
      else
        this.m_State &= ~CitizenFlags.EducationBit1;
      if (level % 2 != 0)
        this.m_State |= CitizenFlags.EducationBit2;
      else
        this.m_State &= ~CitizenFlags.EducationBit2;
    }

    public int GetFailedEducationCount()
    {
      return ((this.m_State & CitizenFlags.FailedEducationBit1) != CitizenFlags.None ? 2 : 0) + ((this.m_State & CitizenFlags.FailedEducationBit2) != CitizenFlags.None ? 1 : 0);
    }

    public void SetFailedEducationCount(int fails)
    {
      if (fails >= 2)
        this.m_State |= CitizenFlags.FailedEducationBit1;
      else
        this.m_State &= ~CitizenFlags.FailedEducationBit1;
      if (fails % 2 != 0)
        this.m_State |= CitizenFlags.FailedEducationBit2;
      else
        this.m_State &= ~CitizenFlags.FailedEducationBit2;
    }

    public void SetAge(CitizenAge newAge)
    {
      this.m_State = this.m_State & ~(CitizenFlags.AgeBit1 | CitizenFlags.AgeBit2) | ((newAge & CitizenAge.Adult) != CitizenAge.Child ? CitizenFlags.AgeBit1 : CitizenFlags.None) | ((int) newAge % 2 != 0 ? CitizenFlags.AgeBit2 : CitizenFlags.None);
    }

    public CitizenAge GetAge()
    {
      return (CitizenAge) (2 * ((this.m_State & CitizenFlags.AgeBit1) != CitizenFlags.None ? 1 : 0) + ((this.m_State & CitizenFlags.AgeBit2) != CitizenFlags.None ? 1 : 0));
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((short) this.m_State);
      writer.Write(this.m_WellBeing);
      writer.Write(this.m_Health);
      writer.Write(this.m_LeisureCounter);
      writer.Write(this.m_PenaltyCounter);
      writer.Write(this.m_BirthDay);
      writer.Write(this.m_PseudoRandom);
      writer.Write(this.m_UnemploymentCounter);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version < Version.saveOptimizations)
        reader.Read(out uint _);
      short num;
      reader.Read(out num);
      reader.Read(out this.m_WellBeing);
      reader.Read(out this.m_Health);
      reader.Read(out this.m_LeisureCounter);
      if (reader.context.version >= Version.penaltyCounter)
        reader.Read(out this.m_PenaltyCounter);
      reader.Read(out this.m_BirthDay);
      this.m_State = (CitizenFlags) num;
      Context context = reader.context;
      if (context.version >= Version.snow)
        reader.Read(out this.m_PseudoRandom);
      context = reader.context;
      if (!(context.version >= Version.economyFix))
        return;
      reader.Read(out this.m_UnemploymentCounter);
    }
  }
}
