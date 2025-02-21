// Decompiled with JetBrains decompiler
// Type: Game.Net.Node
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct Node : IComponentData, IQueryTypeParameter, IStrideSerializable, ISerializable
  {
    public float3 m_Position;
    public quaternion m_Rotation;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_Rotation);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      reader.Read(out this.m_Rotation);
    }

    public int GetStride(Context context)
    {
      return UnsafeUtility.SizeOf<float3>() + UnsafeUtility.SizeOf<quaternion>();
    }
  }
}
