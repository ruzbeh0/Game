// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PollutionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PollutionData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<PollutionData>,
    ISerializable
  {
    public float m_GroundPollution;
    public float m_AirPollution;
    public float m_NoisePollution;
    public bool m_ScaleWithRenters;

    public float GetValue(BuildingStatusType statusType)
    {
      switch (statusType)
      {
        case BuildingStatusType.AirPollutionSource:
          return this.m_AirPollution;
        case BuildingStatusType.GroundPollutionSource:
          return this.m_GroundPollution;
        case BuildingStatusType.NoisePollutionSource:
          return this.m_NoisePollution;
        default:
          return 0.0f;
      }
    }

    public void AddArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public void Combine(PollutionData otherData)
    {
      this.m_GroundPollution += otherData.m_GroundPollution;
      this.m_AirPollution += otherData.m_AirPollution;
      this.m_NoisePollution += otherData.m_NoisePollution;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_GroundPollution);
      writer.Write(this.m_AirPollution);
      writer.Write(this.m_NoisePollution);
      writer.Write(this.m_ScaleWithRenters);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version > Version.pollutionMultiplierChange)
        reader.Read(out this.m_ScaleWithRenters);
      else
        this.m_ScaleWithRenters = true;
      if (reader.context.version < Version.pollutionFloatFix)
      {
        int num1;
        reader.Read(out num1);
        this.m_GroundPollution = (float) num1;
        int num2;
        reader.Read(out num2);
        this.m_AirPollution = (float) num2;
        int num3;
        reader.Read(out num3);
        this.m_NoisePollution = (float) num3;
      }
      else
      {
        reader.Read(out this.m_GroundPollution);
        reader.Read(out this.m_AirPollution);
        reader.Read(out this.m_NoisePollution);
      }
    }
  }
}
