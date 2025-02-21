// Decompiled with JetBrains decompiler
// Type: Game.Buildings.PropertyRenter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct PropertyRenter : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Property;
    public int m_Rent;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Property);
      writer.Write(this.m_Rent);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Property);
      reader.Read(out this.m_Rent);
      if (!(reader.context.version < Version.economyFix))
        return;
      reader.Read(out int _);
    }
  }
}
