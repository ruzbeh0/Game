// Decompiled with JetBrains decompiler
// Type: Game.Objects.Relative
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  public struct Relative : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_Position;
    public quaternion m_Rotation;
    public int3 m_BoneIndex;

    public Relative(Transform localTransform, int3 boneIndex)
    {
      this.m_Position = localTransform.m_Position;
      this.m_Rotation = localTransform.m_Rotation;
      this.m_BoneIndex = boneIndex;
    }

    public Transform ToTransform() => new Transform(this.m_Position, this.m_Rotation);

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Position);
      writer.Write(this.m_Rotation);
      writer.Write(this.m_BoneIndex.x);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Position);
      reader.Read(out this.m_Rotation);
      if (reader.context.version >= Version.boneRelativeObjects)
        reader.Read(out this.m_BoneIndex.x);
      this.m_BoneIndex.yz = (int2) -1;
    }
  }
}
