// Decompiled with JetBrains decompiler
// Type: Game.Creatures.AnimalNavigation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Objects;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Creatures
{
  public struct AnimalNavigation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_TargetPosition;
    public float3 m_TargetDirection;
    public float m_MaxSpeed;
    public TransformState m_TransformState;
    public byte m_LastActivity;
    public byte m_TargetActivity;

    public AnimalNavigation(float3 position)
    {
      this.m_TargetPosition = position;
      this.m_TargetDirection = new float3();
      this.m_MaxSpeed = 0.0f;
      this.m_TransformState = TransformState.Default;
      this.m_LastActivity = (byte) 0;
      this.m_TargetActivity = (byte) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetPosition);
      writer.Write(this.m_TargetDirection);
      writer.Write(this.m_TargetActivity);
      writer.Write((byte) this.m_TransformState);
      writer.Write(this.m_LastActivity);
      writer.Write(this.m_MaxSpeed);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TargetPosition);
      if (reader.context.version >= Version.creatureTargetDirection)
        reader.Read(out this.m_TargetDirection);
      Context context = reader.context;
      if (context.version >= Version.creatureTargetDirection2)
        reader.Read(out this.m_TargetActivity);
      context = reader.context;
      if (context.version >= Version.animationStateFix)
      {
        byte num;
        reader.Read(out num);
        reader.Read(out this.m_LastActivity);
        this.m_TransformState = (TransformState) num;
      }
      reader.Read(out this.m_MaxSpeed);
    }
  }
}
