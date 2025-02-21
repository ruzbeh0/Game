// Decompiled with JetBrains decompiler
// Type: Game.Creatures.Pet
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct Pet : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_HouseholdPet;
    public PetFlags m_Flags;

    public Pet(Entity householdPet)
    {
      this.m_HouseholdPet = householdPet;
      this.m_Flags = PetFlags.None;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      uint num;
      reader.Read(out num);
      this.m_Flags = (PetFlags) num;
    }
  }
}
