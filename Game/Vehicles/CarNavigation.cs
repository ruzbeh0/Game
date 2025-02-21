// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.CarNavigation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Vehicles
{
  public struct CarNavigation : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_TargetPosition;
    public quaternion m_TargetRotation;
    public float m_MaxSpeed;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetPosition);
      writer.Write(this.m_TargetRotation);
      writer.Write(this.m_MaxSpeed);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TargetPosition);
      if (reader.context.version >= Version.parkingRotation)
      {
        reader.Read(out this.m_TargetRotation);
      }
      else
      {
        float3 forward;
        reader.Read(out forward);
        if (!forward.Equals(new float3()))
          this.m_TargetRotation = quaternion.LookRotationSafe(forward, math.up());
      }
      reader.Read(out this.m_MaxSpeed);
    }
  }
}
