// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CollectedCityServiceFeeData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct CollectedCityServiceFeeData : IBufferElementData, ISerializable
  {
    public int m_PlayerResource;
    public float m_Export;
    public float m_Import;
    public float m_Internal;
    public float m_ExportCount;
    public float m_ImportCount;
    public float m_InternalCount;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PlayerResource);
      reader.Read(out this.m_Export);
      reader.Read(out this.m_Import);
      reader.Read(out this.m_Internal);
      if (reader.context.version < Version.serviceFeeFix)
      {
        int num1;
        reader.Read(out num1);
        int num2;
        reader.Read(out num2);
        int num3;
        reader.Read(out num3);
        this.m_ExportCount = (float) num1;
        this.m_ImportCount = (float) num2;
        this.m_InternalCount = (float) num3;
      }
      else
      {
        reader.Read(out this.m_ExportCount);
        reader.Read(out this.m_ImportCount);
        reader.Read(out this.m_InternalCount);
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PlayerResource);
      writer.Write(this.m_Export);
      writer.Write(this.m_Import);
      writer.Write(this.m_Internal);
      writer.Write(this.m_ExportCount);
      writer.Write(this.m_ImportCount);
      writer.Write(this.m_InternalCount);
    }
  }
}
