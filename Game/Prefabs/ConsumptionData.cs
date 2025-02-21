// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ConsumptionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct ConsumptionData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<ConsumptionData>,
    ISerializable
  {
    public int m_Upkeep;
    public float m_ElectricityConsumption;
    public float m_WaterConsumption;
    public float m_GarbageAccumulation;
    public float m_TelecomNeed;

    public void AddArchetypeComponents(HashSet<ComponentType> components)
    {
      if ((double) this.m_ElectricityConsumption > 0.0)
        components.Add(ComponentType.ReadWrite<ElectricityConsumer>());
      if ((double) this.m_WaterConsumption > 0.0)
        components.Add(ComponentType.ReadWrite<WaterConsumer>());
      if ((double) this.m_GarbageAccumulation > 0.0)
        components.Add(ComponentType.ReadWrite<GarbageProducer>());
      if ((double) this.m_TelecomNeed <= 0.0)
        return;
      components.Add(ComponentType.ReadWrite<TelecomConsumer>());
    }

    public void Combine(ConsumptionData otherData)
    {
      this.m_Upkeep += otherData.m_Upkeep;
      this.m_ElectricityConsumption += otherData.m_ElectricityConsumption;
      this.m_WaterConsumption += otherData.m_WaterConsumption;
      this.m_GarbageAccumulation += otherData.m_GarbageAccumulation;
      this.m_TelecomNeed = math.max(this.m_TelecomNeed, otherData.m_TelecomNeed);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Upkeep);
      writer.Write(this.m_ElectricityConsumption);
      writer.Write(this.m_WaterConsumption);
      writer.Write(this.m_GarbageAccumulation);
      writer.Write(this.m_TelecomNeed);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Upkeep);
      reader.Read(out this.m_ElectricityConsumption);
      reader.Read(out this.m_WaterConsumption);
      reader.Read(out this.m_GarbageAccumulation);
      if (!(reader.context.version > Version.telecomNeed))
        return;
      reader.Read(out this.m_TelecomNeed);
    }
  }
}
