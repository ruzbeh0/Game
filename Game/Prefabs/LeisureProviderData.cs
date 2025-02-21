// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LeisureProviderData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Agents;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct LeisureProviderData : IComponentData, IQueryTypeParameter, ISerializable
  {
    public int m_Efficiency;
    public Resource m_Resources;
    public LeisureType m_LeisureType;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Efficiency);
      writer.Write((sbyte) EconomyUtils.GetResourceIndex(this.m_Resources));
      writer.Write((byte) this.m_LeisureType);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Efficiency);
      sbyte index;
      reader.Read(out index);
      byte num;
      reader.Read(out num);
      this.m_Resources = EconomyUtils.GetResource((int) index);
      this.m_LeisureType = (LeisureType) num;
    }
  }
}
