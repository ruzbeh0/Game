// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PoliceCarData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PoliceCarData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_CriminalCapacity;
    public float m_CrimeReductionRate;
    public uint m_ShiftDuration;
    public PolicePurpose m_PurposeMask;

    public PoliceCarData(
      int criminalCapacity,
      float crimeReductionRate,
      uint shiftDuration,
      PolicePurpose purposeMask)
    {
      this.m_CriminalCapacity = criminalCapacity;
      this.m_CrimeReductionRate = crimeReductionRate;
      this.m_ShiftDuration = shiftDuration;
      this.m_PurposeMask = purposeMask;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_PurposeMask);
      writer.Write(this.m_CriminalCapacity);
      writer.Write(this.m_CrimeReductionRate);
      writer.Write(this.m_ShiftDuration);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_CriminalCapacity);
      reader.Read(out this.m_CrimeReductionRate);
      reader.Read(out this.m_ShiftDuration);
      this.m_PurposeMask = (PolicePurpose) num;
    }
  }
}
