// Decompiled with JetBrains decompiler
// Type: Game.Creatures.Queue
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  [InternalBufferCapacity(0)]
  public struct Queue : IBufferElementData, ISerializable
  {
    public Entity m_TargetEntity;
    public Sphere3 m_TargetArea;
    public ushort m_ObsoleteTime;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetEntity);
      writer.Write(this.m_ObsoleteTime);
      writer.Write(this.m_TargetArea.radius);
      if ((double) this.m_TargetArea.radius <= 0.0)
        return;
      writer.Write(this.m_TargetArea.position);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_TargetEntity);
      reader.Read(out this.m_ObsoleteTime);
      reader.Read(out this.m_TargetArea.radius);
      if ((double) this.m_TargetArea.radius <= 0.0)
        return;
      reader.Read(out this.m_TargetArea.position);
    }
  }
}
