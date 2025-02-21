// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.StorageCompanyData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct StorageCompanyData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Resource m_StoredResources;
    public int2 m_TransportInterval;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((ulong) this.m_StoredResources);
      writer.Write(this.m_TransportInterval);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      ulong num;
      reader.Read(out num);
      this.m_StoredResources = (Resource) num;
      if (!(reader.context.version > Version.transportInterval))
        return;
      reader.Read(out this.m_TransportInterval);
    }
  }
}
