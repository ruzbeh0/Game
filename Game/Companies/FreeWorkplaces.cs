// Decompiled with JetBrains decompiler
// Type: Game.Companies.FreeWorkplaces
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Companies
{
  public struct FreeWorkplaces : IComponentData, IQueryTypeParameter, ISerializable
  {
    public byte m_Uneducated;
    public byte m_PoorlyEducated;
    public byte m_Educated;
    public byte m_WellEducated;
    public byte m_HighlyEducated;

    public FreeWorkplaces(Workplaces free)
    {
      this.m_Uneducated = (byte) math.clamp(free.m_Uneducated, 0, (int) byte.MaxValue);
      this.m_PoorlyEducated = (byte) math.clamp(free.m_PoorlyEducated, 0, (int) byte.MaxValue);
      this.m_Educated = (byte) math.clamp(free.m_Educated, 0, (int) byte.MaxValue);
      this.m_WellEducated = (byte) math.clamp(free.m_WellEducated, 0, (int) byte.MaxValue);
      this.m_HighlyEducated = (byte) math.clamp(free.m_HighlyEducated, 0, (int) byte.MaxValue);
    }

    public int Count
    {
      get
      {
        return (int) this.m_Uneducated + (int) this.m_PoorlyEducated + (int) this.m_Educated + (int) this.m_WellEducated + (int) this.m_HighlyEducated;
      }
    }

    public void Refresh(
      DynamicBuffer<Employee> employees,
      int maxWorkers,
      WorkplaceComplexity complexity,
      int level)
    {
      // ISSUE: reference to a compiler-generated method
      Workplaces numberOfWorkplaces = WorkProviderSystem.CalculateNumberOfWorkplaces(maxWorkers, complexity, level);
      for (int index = 0; index < employees.Length; ++index)
      {
        int level1 = (int) employees[index].m_Level;
        numberOfWorkplaces[level1]--;
      }
      this.m_Uneducated = (byte) math.clamp(numberOfWorkplaces.m_Uneducated, 0, (int) byte.MaxValue);
      this.m_PoorlyEducated = (byte) math.clamp(numberOfWorkplaces.m_PoorlyEducated, 0, (int) byte.MaxValue);
      this.m_Educated = (byte) math.clamp(numberOfWorkplaces.m_Educated, 0, (int) byte.MaxValue);
      this.m_WellEducated = (byte) math.clamp(numberOfWorkplaces.m_WellEducated, 0, (int) byte.MaxValue);
      this.m_HighlyEducated = (byte) math.clamp(numberOfWorkplaces.m_HighlyEducated, 0, (int) byte.MaxValue);
    }

    public byte GetLowestFree()
    {
      for (byte level = 0; level <= (byte) 4; ++level)
      {
        if (this.GetFree((int) level) > (byte) 0)
          return level;
      }
      return 5;
    }

    public int GetBestFor(int level)
    {
      for (int level1 = level; level1 >= 0; --level1)
      {
        if (this.GetFree((int) (byte) level1) > (byte) 0)
          return level1;
      }
      return -1;
    }

    public byte GetFree(int level)
    {
      switch (level)
      {
        case 0:
          return this.m_Uneducated;
        case 1:
          return this.m_PoorlyEducated;
        case 2:
          return this.m_Educated;
        case 3:
          return this.m_WellEducated;
        case 4:
          return this.m_HighlyEducated;
        default:
          return 0;
      }
    }

    private void SetFree(int level, byte amount)
    {
      switch (level)
      {
        case 0:
          this.m_Uneducated = amount;
          break;
        case 1:
          this.m_PoorlyEducated = amount;
          break;
        case 2:
          this.m_Educated = amount;
          break;
        case 3:
          this.m_WellEducated = amount;
          break;
        case 4:
          this.m_HighlyEducated = amount;
          break;
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Uneducated);
      writer.Write(this.m_PoorlyEducated);
      writer.Write(this.m_Educated);
      writer.Write(this.m_WellEducated);
      writer.Write(this.m_HighlyEducated);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Uneducated);
      reader.Read(out this.m_PoorlyEducated);
      reader.Read(out this.m_Educated);
      reader.Read(out this.m_WellEducated);
      reader.Read(out this.m_HighlyEducated);
    }
  }
}
