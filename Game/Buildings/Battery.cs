// Decompiled with JetBrains decompiler
// Type: Game.Buildings.Battery
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct Battery : IComponentData, IQueryTypeParameter, ISerializable
  {
    public long m_StoredEnergy;
    public int m_Capacity;
    public int m_LastFlow;

    public int storedEnergyHours => (int) (this.m_StoredEnergy / 85L);

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StoredEnergy);
      writer.Write(this.m_Capacity);
      writer.Write(this.m_LastFlow);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StoredEnergy);
      if (reader.context.version >= Version.batteryStats)
        reader.Read(out this.m_Capacity);
      if (!(reader.context.version >= Version.batteryLastFlow))
        return;
      reader.Read(out this.m_LastFlow);
    }
  }
}
