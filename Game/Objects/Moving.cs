// Decompiled with JetBrains decompiler
// Type: Game.Objects.Moving
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  public struct Moving : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_Velocity;
    public float3 m_AngularVelocity;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Velocity);
      writer.Write(this.m_AngularVelocity);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Velocity);
      reader.Read(out this.m_AngularVelocity);
      if (math.all(math.isfinite(this.m_Velocity)) && math.all(math.isfinite(this.m_AngularVelocity)))
        return;
      this.m_Velocity = new float3();
      this.m_AngularVelocity = new float3();
    }
  }
}
