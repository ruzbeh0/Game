// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  [InternalBufferCapacity(0)]
  public struct PathElement : IBufferElementData, ISerializable
  {
    public Entity m_Target;
    public float2 m_TargetDelta;
    public PathElementFlags m_Flags;

    public PathElement(Entity target, float2 targetDelta, PathElementFlags flags = (PathElementFlags) 0)
    {
      this.m_Target = target;
      this.m_TargetDelta = targetDelta;
      this.m_Flags = flags;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      int2 int2 = math.select(math.select(new int2(2), new int2(0), this.m_TargetDelta == 0.0f), new int2(1), this.m_TargetDelta == 1f);
      writer.Write((byte) (int2.x | int2.y << 4));
      if (int2.x == 2)
        writer.Write(this.m_TargetDelta.x);
      if (int2.y == 2)
        writer.Write(this.m_TargetDelta.y);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      byte num1;
      reader.Read(out num1);
      int2 int2 = new int2((int) num1 & 15, (int) num1 >> 4);
      this.m_TargetDelta = math.select((float2) 0.0f, (float2) 1f, int2 == 1);
      if (int2.x == 2)
        reader.Read(out this.m_TargetDelta.x);
      if (int2.y == 2)
        reader.Read(out this.m_TargetDelta.y);
      if (!(reader.context.version >= Version.taxiDispatchCenter))
        return;
      byte num2;
      reader.Read(out num2);
      this.m_Flags = (PathElementFlags) num2;
    }
  }
}
