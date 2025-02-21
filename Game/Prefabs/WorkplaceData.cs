// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WorkplaceData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct WorkplaceData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<WorkplaceData>,
    ISerializable
  {
    public WorkplaceComplexity m_Complexity;
    public int m_MaxWorkers;
    public float m_EveningShiftProbability;
    public float m_NightShiftProbability;
    public int m_MinimumWorkersLimit;

    public void Combine(WorkplaceData other)
    {
      int maxWorkers = this.m_MaxWorkers;
      this.m_MaxWorkers += other.m_MaxWorkers;
      this.m_MinimumWorkersLimit += this.m_MinimumWorkersLimit;
      this.m_EveningShiftProbability = math.lerp(other.m_EveningShiftProbability, this.m_EveningShiftProbability, (float) (maxWorkers / this.m_MaxWorkers));
      this.m_NightShiftProbability = math.lerp(other.m_NightShiftProbability, this.m_NightShiftProbability, (float) (maxWorkers / this.m_MaxWorkers));
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_MaxWorkers);
      writer.Write(this.m_EveningShiftProbability);
      writer.Write(this.m_NightShiftProbability);
      writer.Write((byte) this.m_Complexity);
      writer.Write(this.m_MinimumWorkersLimit);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_MaxWorkers);
      reader.Read(out this.m_EveningShiftProbability);
      reader.Read(out this.m_NightShiftProbability);
      byte num;
      reader.Read(out num);
      this.m_Complexity = (WorkplaceComplexity) num;
      if (!(reader.context.version > Version.addMinimumWorkersLimit))
        return;
      reader.Read(out this.m_MinimumWorkersLimit);
    }
  }
}
