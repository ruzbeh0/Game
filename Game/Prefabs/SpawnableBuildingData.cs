// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SpawnableBuildingData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct SpawnableBuildingData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_ZonePrefab;
    public byte m_Level;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ZonePrefab);
      writer.Write(this.m_Level);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ZonePrefab);
      reader.Read(out this.m_Level);
    }
  }
}
