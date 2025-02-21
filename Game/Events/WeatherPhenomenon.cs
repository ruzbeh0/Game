// Decompiled with JetBrains decompiler
// Type: Game.Events.WeatherPhenomenon
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Events
{
  public struct WeatherPhenomenon : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_PhenomenonPosition;
    public float3 m_HotspotPosition;
    public float3 m_HotspotVelocity;
    public float m_PhenomenonRadius;
    public float m_HotspotRadius;
    public float m_Intensity;
    public float m_LightningTimer;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_PhenomenonPosition);
      writer.Write(this.m_HotspotPosition);
      writer.Write(this.m_HotspotVelocity);
      writer.Write(this.m_PhenomenonRadius);
      writer.Write(this.m_HotspotRadius);
      writer.Write(this.m_Intensity);
      writer.Write(this.m_LightningTimer);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_PhenomenonPosition);
      reader.Read(out this.m_HotspotPosition);
      reader.Read(out this.m_HotspotVelocity);
      reader.Read(out this.m_PhenomenonRadius);
      reader.Read(out this.m_HotspotRadius);
      reader.Read(out this.m_Intensity);
      if (!(reader.context.version >= Version.lightningSimulation))
        return;
      if (reader.context.version < Version.weatherPhenomenonFix)
        reader.Read(out float _);
      reader.Read(out this.m_LightningTimer);
    }
  }
}
