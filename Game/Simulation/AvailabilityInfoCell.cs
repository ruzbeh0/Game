// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AvailabilityInfoCell
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct AvailabilityInfoCell : IAvailabilityInfoCell, IStrideSerializable, ISerializable
  {
    public float4 m_AvailabilityInfo;

    public void AddAttractiveness(float amount) => this.m_AvailabilityInfo.x += amount;

    public void AddConsumers(float amount) => this.m_AvailabilityInfo.y += amount;

    public void AddWorkplaces(float amount) => this.m_AvailabilityInfo.z += amount;

    public void AddServices(float amount) => this.m_AvailabilityInfo.w += amount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AvailabilityInfo);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_AvailabilityInfo);
    }

    public int GetStride(Context context) => UnsafeUtility.SizeOf<float4>();
  }
}
