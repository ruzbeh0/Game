// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.TrainBogiePosition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Objects;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public struct TrainBogiePosition : ISerializable
  {
    public float3 m_Position;
    public float3 m_Direction;

    public TrainBogiePosition(Transform transform)
    {
      this.m_Position = transform.m_Position;
      this.m_Direction = math.forward(transform.m_Rotation);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_Direction);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      reader.Read(out this.m_Direction);
    }
  }
}
