// Decompiled with JetBrains decompiler
// Type: Game.Buildings.SewageOutlet
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct SewageOutlet : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Capacity;
    public int m_LastProcessed;
    public int m_LastPurified;
    public int m_UsedPurified;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Capacity);
      writer.Write(this.m_LastProcessed);
      writer.Write(this.m_LastPurified);
      writer.Write(this.m_UsedPurified);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version < Version.waterPipeFlowSim)
      {
        reader.Read(out int _);
        Context context = reader.context;
        if (context.version >= Version.stormWater)
          reader.Read(out int _);
        context = reader.context;
        if (!(context.version >= Version.sewageSelectedInfoFix))
          return;
        reader.Read(out int _);
      }
      else
      {
        reader.Read(out this.m_Capacity);
        reader.Read(out this.m_LastProcessed);
        reader.Read(out this.m_LastPurified);
        reader.Read(out this.m_UsedPurified);
      }
    }
  }
}
