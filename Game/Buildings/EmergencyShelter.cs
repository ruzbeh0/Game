// Decompiled with JetBrains decompiler
// Type: Game.Buildings.EmergencyShelter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct EmergencyShelter : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_TargetRequest;
    public EmergencyShelterFlags m_Flags;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_TargetRequest);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.reverseServiceRequests)
        reader.Read(out this.m_TargetRequest);
      byte num;
      reader.Read(out num);
      this.m_Flags = (EmergencyShelterFlags) num;
    }
  }
}
