// Decompiled with JetBrains decompiler
// Type: Game.Objects.Transform
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  public struct Transform : IComponentData, IQueryTypeParameter, IEquatable<Transform>, ISerializable
  {
    public float3 m_Position;
    public quaternion m_Rotation;

    public Transform(float3 position, quaternion rotation)
    {
      this.m_Position = position;
      this.m_Rotation = rotation;
    }

    public bool Equals(Transform other)
    {
      return this.m_Position.Equals(other.m_Position) && this.m_Rotation.Equals(other.m_Rotation);
    }

    public override int GetHashCode()
    {
      return (17 * 31 + this.m_Position.GetHashCode()) * 31 + this.m_Rotation.GetHashCode();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_Rotation);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      reader.Read(out this.m_Rotation);
      if (math.all(this.m_Position >= -100000f) && math.all(this.m_Position <= 100000f) && math.all(math.isfinite(this.m_Rotation.value)) && !math.all(this.m_Rotation.value == 0.0f))
        return;
      this.m_Position = new float3();
      this.m_Rotation = quaternion.identity;
    }
  }
}
