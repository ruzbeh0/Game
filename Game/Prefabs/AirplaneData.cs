// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AirplaneData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct AirplaneData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float2 m_FlyingSpeed;
    public float m_FlyingAcceleration;
    public float m_FlyingBraking;
    public float m_FlyingTurning;
    public float m_FlyingAngularAcceleration;
    public float m_ClimbAngle;
    public float m_SlowPitchAngle;
    public float m_TurningRollFactor;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_FlyingSpeed);
      writer.Write(this.m_FlyingAcceleration);
      writer.Write(this.m_FlyingBraking);
      writer.Write(this.m_FlyingTurning);
      writer.Write(this.m_FlyingAngularAcceleration);
      writer.Write(this.m_ClimbAngle);
      writer.Write(this.m_SlowPitchAngle);
      writer.Write(this.m_TurningRollFactor);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_FlyingSpeed);
      reader.Read(out this.m_FlyingAcceleration);
      reader.Read(out this.m_FlyingBraking);
      reader.Read(out this.m_FlyingTurning);
      reader.Read(out this.m_FlyingAngularAcceleration);
      reader.Read(out this.m_ClimbAngle);
      reader.Read(out this.m_SlowPitchAngle);
      reader.Read(out this.m_TurningRollFactor);
    }
  }
}
