// Decompiled with JetBrains decompiler
// Type: Game.Creatures.CurrentVehicle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct CurrentVehicle : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Vehicle;
    public CreatureVehicleFlags m_Flags;

    public CurrentVehicle(Entity vehicle, CreatureVehicleFlags flags)
    {
      this.m_Vehicle = vehicle;
      this.m_Flags = flags;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Vehicle);
      writer.Write((uint) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Vehicle);
      uint num;
      reader.Read(out num);
      this.m_Flags = (CreatureVehicleFlags) num;
    }
  }
}
