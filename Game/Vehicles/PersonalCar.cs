// Decompiled with JetBrains decompiler
// Type: Game.Vehicles.PersonalCar
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Vehicles
{
  public struct PersonalCar : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Keeper;
    public PersonalCarFlags m_State;

    public PersonalCar(Entity keeper, PersonalCarFlags state)
    {
      this.m_Keeper = keeper;
      this.m_State = state;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Keeper);
      writer.Write((uint) this.m_State);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Keeper);
      uint num;
      reader.Read(out num);
      this.m_State = (PersonalCarFlags) num;
    }
  }
}
