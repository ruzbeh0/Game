// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DeliveryTruckData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct DeliveryTruckData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_CargoCapacity;
    public int m_CostToDrive;
    public Resource m_TransportedResources;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((ulong) this.m_TransportedResources);
      writer.Write(this.m_CargoCapacity);
      writer.Write(this.m_CostToDrive);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      ulong num;
      reader.Read(out num);
      reader.Read(out this.m_CargoCapacity);
      reader.Read(out this.m_CostToDrive);
      this.m_TransportedResources = (Resource) num;
    }
  }
}
