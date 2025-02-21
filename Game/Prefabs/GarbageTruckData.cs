// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.GarbageTruckData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct GarbageTruckData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_GarbageCapacity;
    public int m_UnloadRate;

    public GarbageTruckData(int garbageCapacity, int unloadRate)
    {
      this.m_GarbageCapacity = garbageCapacity;
      this.m_UnloadRate = unloadRate;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_GarbageCapacity);
      writer.Write(this.m_UnloadRate);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_GarbageCapacity);
      reader.Read(out this.m_UnloadRate);
    }
  }
}
