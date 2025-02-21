// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingTerraformData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct BuildingTerraformData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_FlatX0;
    public float3 m_FlatZ0;
    public float3 m_FlatX1;
    public float3 m_FlatZ1;
    public float4 m_Smooth;
    public float m_HeightOffset;
    public bool m_DontRaise;
    public bool m_DontLower;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_FlatX0);
      writer.Write(this.m_FlatZ0);
      writer.Write(this.m_FlatX1);
      writer.Write(this.m_FlatZ1);
      writer.Write(this.m_Smooth);
      writer.Write(this.m_HeightOffset);
      writer.Write(this.m_DontRaise);
      writer.Write(this.m_DontLower);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_FlatX0);
      reader.Read(out this.m_FlatZ0);
      reader.Read(out this.m_FlatX1);
      reader.Read(out this.m_FlatZ1);
      reader.Read(out this.m_Smooth);
      if (!(reader.context.version >= Version.pillarTerrainModification))
        return;
      reader.Read(out this.m_HeightOffset);
      reader.Read(out this.m_DontRaise);
      reader.Read(out this.m_DontLower);
    }
  }
}
