// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HelicopterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Vehicles;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct HelicopterData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public HelicopterType m_HelicopterType;
    public float m_FlyingMaxSpeed;
    public float m_FlyingAcceleration;
    public float m_FlyingAngularAcceleration;
    public float m_AccelerationSwayFactor;
    public float m_VelocitySwayFactor;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) this.m_HelicopterType);
      writer.Write(this.m_FlyingMaxSpeed);
      writer.Write(this.m_FlyingAcceleration);
      writer.Write(this.m_FlyingAngularAcceleration);
      writer.Write(this.m_AccelerationSwayFactor);
      writer.Write(this.m_VelocitySwayFactor);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_FlyingMaxSpeed);
      reader.Read(out this.m_FlyingAcceleration);
      reader.Read(out this.m_FlyingAngularAcceleration);
      reader.Read(out this.m_AccelerationSwayFactor);
      reader.Read(out this.m_VelocitySwayFactor);
      this.m_HelicopterType = (HelicopterType) num;
    }
  }
}
