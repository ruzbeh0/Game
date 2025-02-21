// Decompiled with JetBrains decompiler
// Type: Game.Objects.TransformFrame
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [InternalBufferCapacity(4)]
  public struct TransformFrame : IBufferElementData, ISerializable
  {
    public float3 m_Position;
    public float3 m_Velocity;
    public quaternion m_Rotation;
    public TransformFlags m_Flags;
    public ushort m_StateTimer;
    public TransformState m_State;
    public byte m_Activity;

    public TransformFrame(Transform transform)
    {
      this.m_Position = transform.m_Position;
      this.m_Velocity = new float3();
      this.m_Rotation = transform.m_Rotation;
      this.m_Flags = (TransformFlags) 0;
      this.m_StateTimer = (ushort) 0;
      this.m_State = TransformState.Default;
      this.m_Activity = (byte) 0;
    }

    public TransformFrame(Transform transform, Moving moving)
    {
      this.m_Position = transform.m_Position;
      this.m_Velocity = moving.m_Velocity;
      this.m_Rotation = transform.m_Rotation;
      this.m_Flags = (TransformFlags) 0;
      this.m_StateTimer = (ushort) 0;
      this.m_State = TransformState.Default;
      this.m_Activity = (byte) 0;
    }

    public TransformFrame(float3 position, quaternion rotation, float3 velocity)
    {
      this.m_Position = position;
      this.m_Velocity = velocity;
      this.m_Rotation = rotation;
      this.m_Flags = (TransformFlags) 0;
      this.m_StateTimer = (ushort) 0;
      this.m_State = TransformState.Default;
      this.m_Activity = (byte) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StateTimer);
      writer.Write((byte) this.m_State);
      writer.Write(this.m_Activity);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StateTimer);
      byte num;
      reader.Read(out num);
      reader.Read(out this.m_Activity);
      this.m_State = (TransformState) num;
    }
  }
}
