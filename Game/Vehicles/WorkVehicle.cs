// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.WorkVehicle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct WorkVehicle : IComponentData, IQueryTypeParameter, ISerializable
  {
    public WorkVehicleFlags m_State;
    public float m_WorkAmount;
    public float m_DoneAmount;

    public WorkVehicle(WorkVehicleFlags flags, float workAmount)
    {
      this.m_State = flags;
      this.m_WorkAmount = workAmount;
      this.m_DoneAmount = 0.0f;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_State);
      writer.Write(this.m_WorkAmount);
      writer.Write(this.m_DoneAmount);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_WorkAmount);
      reader.Read(out this.m_DoneAmount);
      this.m_State = (WorkVehicleFlags) num;
    }
  }
}
