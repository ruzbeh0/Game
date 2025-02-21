// Decompiled with JetBrains decompiler
// Type: Game.Areas.Node
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [InternalBufferCapacity(4)]
  public struct Node : IBufferElementData, IStrideSerializable, ISerializable
  {
    public float3 m_Position;
    public float m_Elevation;

    public Node(float3 position, float elevation)
    {
      this.m_Position = position;
      this.m_Elevation = elevation;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_Elevation);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      if (!(reader.context.version >= Version.laneElevation))
        return;
      reader.Read(out this.m_Elevation);
    }

    public int GetStride(Context context) => UnsafeUtility.SizeOf<float3>() + 4;
  }
}
