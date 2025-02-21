// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.IndustrialProcessData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct IndustrialProcessData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public ResourceStack m_Input1;
    public ResourceStack m_Input2;
    public ResourceStack m_Output;
    public int m_WorkPerUnit;
    public float m_MaxWorkersPerCell;
    public byte m_IsImport;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write<ResourceStack>(this.m_Input1);
      writer.Write<ResourceStack>(this.m_Input2);
      writer.Write<ResourceStack>(this.m_Output);
      writer.Write(this.m_WorkPerUnit);
      writer.Write(this.m_MaxWorkersPerCell);
      writer.Write(this.m_IsImport);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read<ResourceStack>(out this.m_Input1);
      reader.Read<ResourceStack>(out this.m_Input2);
      reader.Read<ResourceStack>(out this.m_Output);
      reader.Read(out this.m_WorkPerUnit);
      reader.Read(out this.m_MaxWorkersPerCell);
      reader.Read(out this.m_IsImport);
    }
  }
}
