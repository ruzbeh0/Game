// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AdditionalBuildingTerraformElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct AdditionalBuildingTerraformElement : IBufferElementData, ISerializable
  {
    public Bounds2 m_Area;
    public float m_HeightOffset;
    public bool m_Circular;
    public bool m_DontRaise;
    public bool m_DontLower;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Area.min);
      writer.Write(this.m_Area.max);
      writer.Write(this.m_HeightOffset);
      writer.Write(this.m_Circular);
      writer.Write(this.m_DontRaise);
      writer.Write(this.m_DontLower);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Area.min);
      reader.Read(out this.m_Area.max);
      reader.Read(out this.m_HeightOffset);
      reader.Read(out this.m_Circular);
      reader.Read(out this.m_DontRaise);
      if (!(reader.context.version >= Version.pillarTerrainModification))
        return;
      reader.Read(out this.m_DontLower);
    }
  }
}
