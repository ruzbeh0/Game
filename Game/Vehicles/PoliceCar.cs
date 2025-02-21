// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.PoliceCar
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct PoliceCar : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public PoliceCarFlags m_State;
    public int m_RequestCount;
    public float m_PathElementTime;
    public uint m_ShiftTime;
    public uint m_EstimatedShift;
    public PolicePurpose m_PurposeMask;

    public PoliceCar(PoliceCarFlags flags, int requestCount, PolicePurpose purposeMask)
    {
      this.m_TargetRequest = Entity.Null;
      this.m_State = flags;
      this.m_RequestCount = requestCount;
      this.m_PathElementTime = 0.0f;
      this.m_ShiftTime = 0U;
      this.m_EstimatedShift = 0U;
      this.m_PurposeMask = purposeMask;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((uint) this.m_State);
      writer.Write(this.m_RequestCount);
      writer.Write(this.m_PathElementTime);
      writer.Write(this.m_ShiftTime);
      writer.Write(this.m_EstimatedShift);
      writer.Write((int) this.m_PurposeMask);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      uint num1;
      reader.Read(out num1);
      reader.Read(out this.m_RequestCount);
      reader.Read(out this.m_PathElementTime);
      reader.Read(out this.m_ShiftTime);
      if (reader.context.version >= Version.policeShiftEstimate)
        reader.Read(out this.m_EstimatedShift);
      this.m_State = (PoliceCarFlags) num1;
      if (reader.context.version >= Version.policeImprovement3)
      {
        int num2;
        reader.Read(out num2);
        this.m_PurposeMask = (PolicePurpose) num2;
      }
      else
        this.m_PurposeMask = PolicePurpose.Patrol | PolicePurpose.Emergency;
    }
  }
}
