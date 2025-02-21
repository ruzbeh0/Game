// Decompiled with JetBrains decompiler
// Type: Game.Areas.Extractor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Vehicles;
using Unity.Entities;

#nullable disable
namespace Game.Areas
{
  public struct Extractor : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_ResourceAmount;
    public float m_MaxConcentration;
    public float m_ExtractedAmount;
    public float m_WorkAmount;
    public float m_HarvestedAmount;
    public float m_TotalExtracted;
    public VehicleWorkType m_WorkType;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ResourceAmount);
      writer.Write(this.m_MaxConcentration);
      writer.Write(this.m_ExtractedAmount);
      writer.Write(this.m_WorkAmount);
      writer.Write(this.m_HarvestedAmount);
      writer.Write((int) this.m_WorkType);
      writer.Write(this.m_TotalExtracted);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ResourceAmount);
      if (reader.context.version >= Version.resourceConcentration)
        reader.Read(out this.m_MaxConcentration);
      Context context = reader.context;
      if (context.version >= Version.extractedResources)
        reader.Read(out this.m_ExtractedAmount);
      context = reader.context;
      if (context.version >= Version.harvestedResources)
      {
        reader.Read(out this.m_WorkAmount);
        reader.Read(out this.m_HarvestedAmount);
        uint num;
        reader.Read(out num);
        this.m_WorkType = (VehicleWorkType) num;
      }
      context = reader.context;
      if (!(context.version >= Version.totalExtractedResources))
        return;
      reader.Read(out this.m_TotalExtracted);
    }
  }
}
