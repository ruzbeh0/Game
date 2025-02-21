// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.MaintenanceVehicle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct MaintenanceVehicle : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public MaintenanceVehicleFlags m_State;
    public int m_Maintained;
    public int m_MaintainEstimate;
    public int m_RequestCount;
    public float m_PathElementTime;
    public float m_Efficiency;

    public MaintenanceVehicle(MaintenanceVehicleFlags flags, int requestCount, float efficiency)
    {
      this.m_TargetRequest = Entity.Null;
      this.m_State = flags;
      this.m_Maintained = 0;
      this.m_MaintainEstimate = 0;
      this.m_RequestCount = requestCount;
      this.m_PathElementTime = 0.0f;
      this.m_Efficiency = efficiency;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((uint) this.m_State);
      writer.Write(this.m_Maintained);
      writer.Write(this.m_MaintainEstimate);
      writer.Write(this.m_RequestCount);
      writer.Write(this.m_PathElementTime);
      writer.Write(this.m_Efficiency);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests2)
        reader.Read(out this.m_TargetRequest);
      uint num;
      reader.Read(out num);
      reader.Read(out this.m_Maintained);
      Context context = reader.context;
      if (context.version >= Version.policeShiftEstimate)
        reader.Read(out this.m_MaintainEstimate);
      reader.Read(out this.m_RequestCount);
      reader.Read(out this.m_PathElementTime);
      context = reader.context;
      if (context.version >= Version.maintenanceImprovement)
        reader.Read(out this.m_Efficiency);
      this.m_State = (MaintenanceVehicleFlags) num;
    }
  }
}
