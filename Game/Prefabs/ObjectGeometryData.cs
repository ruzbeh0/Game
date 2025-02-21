// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectGeometryData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Objects;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct ObjectGeometryData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Bounds3 m_Bounds;
    public float3 m_Size;
    public float3 m_Pivot;
    public float3 m_LegSize;
    public GeometryFlags m_Flags;
    public int m_MinLod;
    public MeshLayer m_Layers;
    public ObjectRequirementFlags m_SubObjectMask;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Bounds.min);
      reader.Read(out this.m_Bounds.max);
      reader.Read(out this.m_Size);
      reader.Read(out this.m_Pivot);
      reader.Read(out this.m_LegSize);
      uint num;
      reader.Read(out num);
      this.m_Flags = (GeometryFlags) num;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Bounds.min);
      writer.Write(this.m_Bounds.max);
      writer.Write(this.m_Size);
      writer.Write(this.m_Pivot);
      writer.Write(this.m_LegSize);
      writer.Write((uint) this.m_Flags);
    }
  }
}
