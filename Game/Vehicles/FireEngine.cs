// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.FireEngine
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct FireEngine : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public FireEngineFlags m_State;
    public int m_RequestCount;
    public float m_PathElementTime;
    public float m_ExtinguishingAmount;
    public float m_Efficiency;

    public FireEngine(
      FireEngineFlags state,
      int requestCount,
      float extinguishingAmount,
      float efficiency)
    {
      this.m_TargetRequest = Entity.Null;
      this.m_State = state;
      this.m_RequestCount = requestCount;
      this.m_PathElementTime = 0.0f;
      this.m_ExtinguishingAmount = extinguishingAmount;
      this.m_Efficiency = efficiency;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((uint) this.m_State);
      writer.Write(this.m_RequestCount);
      writer.Write(this.m_PathElementTime);
      writer.Write(this.m_ExtinguishingAmount);
      writer.Write(this.m_Efficiency);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_RequestCount);
      reader.Read(out this.m_PathElementTime);
      if (reader.context.version >= Version.aircraftNavigation)
        reader.Read(out this.m_ExtinguishingAmount);
      if (reader.context.version >= Version.disasterResponse)
        reader.Read(out this.m_Efficiency);
      else
        this.m_Efficiency = 1f;
      this.m_State = (FireEngineFlags) num;
    }
  }
}
