// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.GarbageTruck
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct GarbageTruck : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public GarbageTruckFlags m_State;
    public int m_RequestCount;
    public int m_Garbage;
    public int m_EstimatedGarbage;
    public float m_PathElementTime;

    public GarbageTruck(GarbageTruckFlags flags, int requestCount)
    {
      this.m_TargetRequest = Entity.Null;
      this.m_State = flags;
      this.m_RequestCount = requestCount;
      this.m_Garbage = 0;
      this.m_EstimatedGarbage = 0;
      this.m_PathElementTime = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write(this.m_EstimatedGarbage);
      writer.Write((uint) this.m_State);
      writer.Write(this.m_RequestCount);
      writer.Write(this.m_Garbage);
      writer.Write(this.m_PathElementTime);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests2)
      {
        reader.Read(out this.m_TargetRequest);
        reader.Read(out this.m_EstimatedGarbage);
      }
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_RequestCount);
      reader.Read(out this.m_Garbage);
      reader.Read(out this.m_PathElementTime);
      this.m_State = (GarbageTruckFlags) num;
    }
  }
}
