// Decompiled with JetBrains decompiler
// Type: Game.Creatures.Resident
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct Resident : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Citizen;
    public ResidentFlags m_Flags;
    public int m_Timer;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_Flags);
      writer.Write(this.m_Timer);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      if (reader.context.version >= Version.transportWaitTimer)
        reader.Read(out this.m_Timer);
      this.m_Flags = (ResidentFlags) num;
      if (!(reader.context.version < Version.yogaAreaFix))
        return;
      this.m_Flags &= ~ResidentFlags.IgnoreAreas;
    }
  }
}
