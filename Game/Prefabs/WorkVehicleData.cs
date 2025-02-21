// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.WorkVehicleData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Economy;
using Game.Vehicles;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct WorkVehicleData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public VehicleWorkType m_WorkType;
    public MapFeature m_MapFeature;
    public Resource m_Resources;
    public float m_MaxWorkAmount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_WorkType);
      writer.Write((sbyte) this.m_MapFeature);
      writer.Write(this.m_MaxWorkAmount);
      writer.Write((ulong) this.m_Resources);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num1;
      reader.Read(out num1);
      sbyte num2;
      reader.Read(out num2);
      reader.Read(out this.m_MaxWorkAmount);
      this.m_WorkType = (VehicleWorkType) num1;
      this.m_MapFeature = (MapFeature) num2;
      if (!(reader.context.version >= Version.landfillVehicles))
        return;
      ulong num3;
      reader.Read(out num3);
      this.m_Resources = (Resource) num3;
    }
  }
}
