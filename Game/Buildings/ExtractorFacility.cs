// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ExtractorFacility
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct ExtractorFacility : IComponentData, IQueryTypeParameter, ISerializable
  {
    public ExtractorFlags m_Flags;
    public byte m_Timer;
    public BuildingFlags m_MainBuildingFlags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_Timer);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_Timer);
      this.m_Flags = (ExtractorFlags) num;
    }
  }
}
