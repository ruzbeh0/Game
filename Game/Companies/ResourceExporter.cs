// Decompiled with JetBrains decompiler
// Type: Game.Companies.ResourceExporter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Companies
{
  public struct ResourceExporter : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Resource m_Resource;
    public int m_Amount;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resource));
      writer.Write(this.m_Amount);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      sbyte index;
      reader.Read(out index);
      reader.Read(out this.m_Amount);
      this.m_Resource = EconomyUtils.GetResource((int) index);
    }
  }
}
