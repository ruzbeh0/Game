// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.BuildingPropertyData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Game.Zones;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct BuildingPropertyData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_ResidentialProperties;
    public Resource m_AllowedSold;
    public Resource m_AllowedManufactured;
    public Resource m_AllowedStored;
    public float m_SpaceMultiplier;

    public int CountProperties(AreaType areaType)
    {
      switch (areaType)
      {
        case AreaType.Residential:
          return this.m_ResidentialProperties;
        case AreaType.Commercial:
          return this.m_AllowedSold == Resource.NoResource ? 0 : 1;
        case AreaType.Industrial:
          return this.m_AllowedStored != Resource.NoResource || this.m_AllowedManufactured != Resource.NoResource ? 1 : 0;
        default:
          return 0;
      }
    }

    public int CountProperties()
    {
      return this.CountProperties(AreaType.Residential) + this.CountProperties(AreaType.Commercial) + this.CountProperties(AreaType.Industrial);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ResidentialProperties);
      writer.Write(this.m_SpaceMultiplier);
      writer.Write((ulong) this.m_AllowedSold);
      writer.Write((ulong) this.m_AllowedManufactured);
      writer.Write((ulong) this.m_AllowedStored);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ResidentialProperties);
      reader.Read(out this.m_SpaceMultiplier);
      ulong num1;
      reader.Read(out num1);
      ulong num2;
      reader.Read(out num2);
      ulong num3;
      reader.Read(out num3);
      this.m_AllowedSold = (Resource) num1;
      this.m_AllowedManufactured = (Resource) num2;
      this.m_AllowedStored = (Resource) num3;
    }
  }
}
