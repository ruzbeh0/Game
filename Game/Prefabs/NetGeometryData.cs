// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetGeometryData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Net;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct NetGeometryData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public EntityArchetype m_NodeCompositionArchetype;
    public EntityArchetype m_EdgeCompositionArchetype;
    public Entity m_AggregateType;
    public Bounds1 m_DefaultHeightRange;
    public Bounds1 m_ElevatedHeightRange;
    public Bounds1 m_DefaultSurfaceHeight;
    public Bounds1 m_EdgeLengthRange;
    public Layer m_MergeLayers;
    public Layer m_IntersectLayers;
    public GeometryFlags m_Flags;
    public float m_DefaultWidth;
    public float m_ElevatedWidth;
    public float m_ElevatedLength;
    public float m_MinNodeOffset;
    public float m_ElevationLimit;
    public float m_MaxSlopeSteepness;
    public float m_Hanging;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_DefaultHeightRange.min);
      writer.Write(this.m_DefaultHeightRange.max);
      writer.Write(this.m_ElevatedHeightRange.min);
      writer.Write(this.m_ElevatedHeightRange.max);
      writer.Write(this.m_DefaultSurfaceHeight.min);
      writer.Write(this.m_DefaultSurfaceHeight.max);
      writer.Write(this.m_EdgeLengthRange.min);
      writer.Write(this.m_EdgeLengthRange.max);
      writer.Write((uint) this.m_MergeLayers);
      writer.Write((uint) this.m_IntersectLayers);
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_DefaultWidth);
      writer.Write(this.m_ElevatedWidth);
      writer.Write(this.m_MinNodeOffset);
      writer.Write(this.m_ElevationLimit);
      writer.Write(this.m_MaxSlopeSteepness);
      writer.Write(this.m_Hanging);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_DefaultHeightRange.min);
      reader.Read(out this.m_DefaultHeightRange.max);
      reader.Read(out this.m_ElevatedHeightRange.min);
      reader.Read(out this.m_ElevatedHeightRange.max);
      reader.Read(out this.m_DefaultSurfaceHeight.min);
      reader.Read(out this.m_DefaultSurfaceHeight.max);
      reader.Read(out this.m_EdgeLengthRange.min);
      reader.Read(out this.m_EdgeLengthRange.max);
      uint num1;
      reader.Read(out num1);
      uint num2;
      reader.Read(out num2);
      uint num3;
      reader.Read(out num3);
      reader.Read(out this.m_DefaultWidth);
      reader.Read(out this.m_ElevatedWidth);
      reader.Read(out this.m_MinNodeOffset);
      reader.Read(out this.m_ElevationLimit);
      reader.Read(out this.m_MaxSlopeSteepness);
      reader.Read(out this.m_Hanging);
      this.m_MergeLayers = (Layer) num1;
      this.m_IntersectLayers = (Layer) num2;
      this.m_Flags = (GeometryFlags) num3;
      this.m_ElevatedLength = this.m_EdgeLengthRange.max;
    }
  }
}
