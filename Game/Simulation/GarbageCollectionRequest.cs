// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GarbageCollectionRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct GarbageCollectionRequest : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Target;
    public int m_Priority;
    public GarbageCollectionRequestFlags m_Flags;
    public byte m_DispatchIndex;

    public GarbageCollectionRequest(
      Entity target,
      int priority,
      GarbageCollectionRequestFlags flags)
    {
      this.m_Target = target;
      this.m_Priority = priority;
      this.m_Flags = flags;
      this.m_DispatchIndex = (byte) 0;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Target);
      writer.Write(this.m_Priority);
      writer.Write((byte) this.m_Flags);
      writer.Write(this.m_DispatchIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Target);
      reader.Read(out this.m_Priority);
      if (reader.context.version >= Version.industrialWaste)
      {
        byte num;
        reader.Read(out num);
        this.m_Flags = (GarbageCollectionRequestFlags) num;
      }
      if (!(reader.context.version >= Version.requestDispatchIndex))
        return;
      reader.Read(out this.m_DispatchIndex);
    }
  }
}
