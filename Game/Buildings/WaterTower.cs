// Decompiled with JetBrains decompiler
// Type: Game.Buildings.WaterTower
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct WaterTower : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_StoredWater;
    public int m_Polluted;
    public int m_LastStoredWater;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StoredWater);
      writer.Write(this.m_Polluted);
      writer.Write(this.m_LastStoredWater);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StoredWater);
      reader.Read(out this.m_Polluted);
      if (!(reader.context.version >= Version.waterSelectedInfoFix))
        return;
      reader.Read(out this.m_LastStoredWater);
    }
  }
}
