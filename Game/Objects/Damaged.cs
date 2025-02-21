// Decompiled with JetBrains decompiler
// Type: Game.Objects.Damaged
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  public struct Damaged : IComponentData, IQueryTypeParameter, ISerializable
  {
    public float3 m_Damage;

    public Damaged(float3 damage) => this.m_Damage = damage;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Damage);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.damageTypes)
        reader.Read(out this.m_Damage);
      else
        reader.Read(out this.m_Damage.y);
    }
  }
}
