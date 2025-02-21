// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ResourceProductionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct ResourceProductionData : IBufferElementData, ISerializable
  {
    public Resource m_Type;
    public int m_ProductionRate;
    public int m_StorageCapacity;

    public ResourceProductionData(Resource type, int productionRate, int storageCapacity)
    {
      this.m_Type = type;
      this.m_ProductionRate = productionRate;
      this.m_StorageCapacity = storageCapacity;
    }

    public static void Combine(
      NativeList<ResourceProductionData> resources,
      DynamicBuffer<ResourceProductionData> others)
    {
label_8:
      for (int index1 = 0; index1 < others.Length; ++index1)
      {
        ResourceProductionData other = others[index1];
        for (int index2 = 0; index2 < resources.Length; ++index2)
        {
          ResourceProductionData resource = resources[index2];
          if (resource.m_Type == other.m_Type)
          {
            resource.m_ProductionRate += other.m_ProductionRate;
            resource.m_StorageCapacity += other.m_StorageCapacity;
            resources[index2] = resource;
            goto label_8;
          }
        }
        resources.Add(in other);
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ProductionRate);
      writer.Write(this.m_StorageCapacity);
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Type));
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ProductionRate);
      reader.Read(out this.m_StorageCapacity);
      sbyte index;
      reader.Read(out index);
      this.m_Type = EconomyUtils.GetResource((int) index);
    }
  }
}
