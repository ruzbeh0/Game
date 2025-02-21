// Decompiled with JetBrains decompiler
// Type: Game.Events.WaterLevelChange
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Events
{
  public struct WaterLevelChange : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float m_Intensity;
    public float m_MaxIntensity;
    public float m_DangerHeight;
    public float2 m_Direction;

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Intensity);
      reader.Read(out this.m_MaxIntensity);
      reader.Read(out this.m_DangerHeight);
      if (!(reader.context.version >= Version.tsunamiDirection))
        return;
      reader.Read(out float2 _);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Intensity);
      writer.Write(this.m_MaxIntensity);
      writer.Write(this.m_DangerHeight);
      writer.Write(this.m_Direction);
    }
  }
}
